using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HotelManagement.Core;
using Hotel_management_system.Forms.Billing;

namespace Hotel_management_system.Forms.Stay
{
    /// <summary>
    /// Nhận phòng cho MỘT lượt lưu trú (một phòng của phiếu) – người làm: Tuấn. Quyền: A, M, R.
    /// Đặc tả: docs/KE_HOACH_TRIEN_KHAI.md mục 4; nghiệp vụ: docs/Tai_lieu_nghiep_vu.md mục 9.
    ///
    /// Cách làm:
    ///   1. Đọc lượt lưu trú (vw_RoomStays + vw_RoomStayFolio) và hiện 4 điều kiện nhận phòng
    ///      (phiếu đã xác nhận, đủ cọc, đúng ngày, phòng sẵn sàng) để lễ tân thấy trước lý do.
    ///   2. Lễ tân đối chiếu, sửa tên + CCCD người ở thực tế nếu khác người đặt.
    ///   3. Bấm "Nhận phòng" -> dbo.sp_CheckInRoom. Procedure kiểm tra lại toàn bộ điều kiện
    ///      trong transaction (DB là nơi quyết định cuối), ghi giờ nhận, phòng -> Occupied,
    ///      tính lại trạng thái phiếu.
    /// Các điều kiện ở bước 1 tính theo ngày giờ của SQL Server (GETDATE) cho khớp procedure,
    /// không theo đồng hồ máy lễ tân.
    /// </summary>
    public partial class dlgCheckIn : Form
    {
        private const string SqlLoad = @"
SELECT s.BookingRoomId, s.BookingId, s.BookingStatus, s.DepositRate,
       s.CustomerName, s.CustomerPhone,
       s.GuestName, s.GuestIdNumber,
       s.RoomNumber, s.Floor, s.RoomStatus, s.TypeName,
       s.CheckInDate, s.CheckOutDate, s.Nights, s.Adults, s.Children, s.PricePerNight,
       s.StayStatus,
       f.RequiredDeposit, f.NetPaid,
       CAST(SYSDATETIME() AS DATETIME2(0)) AS ServerNow
FROM dbo.vw_RoomStays s
JOIN dbo.vw_RoomStayFolio f ON f.BookingRoomId = s.BookingRoomId
WHERE s.BookingRoomId = @BookingRoomId;";

        private static readonly Color OkColor = Color.FromArgb(0, 128, 0);
        private static readonly Color FailColor = Color.FromArgb(192, 0, 0);
        private const string OkMark = "✔";
        private const string FailMark = "✖";

        private readonly int _bookingRoomId;
        private int _bookingId;
        private string _roomNumber;

        // Tên + CCCD người ở chỉ điền sẵn lần nạp đầu; "Kiểm tra lại" không ghi đè chữ lễ tân đã sửa.
        private bool _guestFilled;

        // Constructor đã chốt (CLAUDE.md mục 7) – không đổi tham số.
        public dlgCheckIn(int bookingRoomId)
        {
            InitializeComponent();
            _bookingRoomId = bookingRoomId;
            UiHelper.SetupDialog(this, btnOk, btnCancel);
        }

        // ==================================================================
        // Nạp dữ liệu
        // ==================================================================

        private void dlgCheckIn_Load(object sender, EventArgs e)
        {
            bool ok;
            try
            {
                ok = LoadStay();
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
                ok = false;
            }

            // Không có gì để nhận thì đóng hộp thoại. Gọi Close qua BeginInvoke vì đóng
            // ngay trong sự kiện Load của hộp thoại dễ làm WinForms xử lý sai.
            if (!ok) BeginInvoke(new Action(Close));
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            Reload();
        }

        /// <summary>Đọc lại điều kiện (sau khi thu cọc, sau khi buồng phòng dọn xong, sau khi lỗi).</summary>
        private void Reload()
        {
            try
            {
                if (!LoadStay())
                {
                    // Phòng không còn chờ nhận (vd. máy khác vừa check-in): báo màn hình lễ tân
                    // nạp lại cho đúng rồi đóng hộp thoại.
                    FormManager.NotifyDataChanged(DataArea.Stays);
                    Close();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        /// <summary>
        /// Nạp lượt lưu trú lên form. Trả false nếu không có hoặc không còn chờ nhận
        /// (đã báo cho người dùng, form gọi sẽ đóng hộp thoại).
        /// </summary>
        private bool LoadStay()
        {
            DataTable dt;
            using (UiHelper.Busy(this))
            {
                dt = DbHelper.QueryText(SqlLoad, DbHelper.P("@BookingRoomId", _bookingRoomId));
            }

            if (dt.Rows.Count == 0)
            {
                UiHelper.Warn("Không tìm thấy lượt lưu trú này. Hãy làm mới màn hình lễ tân rồi chọn lại.", this);
                return false;
            }

            DataRow r = dt.Rows[0];
            string stayStatus = DbHelper.Value<string>(r, "StayStatus");
            if (stayStatus != StayStatus.Booked)
            {
                UiHelper.Warn("Phòng này không còn chờ nhận (hiện là: " + StayStatus.ToVietnamese(stayStatus) +
                              "). Hãy làm mới màn hình lễ tân.", this);
                return false;
            }

            _bookingId = DbHelper.Value<int>(r, "BookingId");
            _roomNumber = DbHelper.Value<string>(r, "RoomNumber");
            ShowBookingInfo(r);
            bool ready = ShowConditions(r);
            FillGuest(r);

            btnOk.Enabled = ready;
            return true;
        }

        private void ShowBookingInfo(DataRow r)
        {
            DateTime checkIn = DbHelper.Value<DateTime>(r, "CheckInDate");
            DateTime checkOut = DbHelper.Value<DateTime>(r, "CheckOutDate");
            int nights = DbHelper.Value<int>(r, "Nights");
            int adults = DbHelper.Value<int>(r, "Adults");
            int children = DbHelper.Value<int>(r, "Children");

            lblBookingId.Text = "#" + _bookingId +
                                "   (cọc " + DbHelper.Value<int>(r, "DepositRate") + "%, " +
                                BookingStatus.ToVietnamese(DbHelper.Value<string>(r, "BookingStatus")) + ")";
            lblCustomer.Text = DbHelper.Value<string>(r, "CustomerName") + "  –  " +
                               DbHelper.Value<string>(r, "CustomerPhone", "");
            lblRoom.Text = _roomNumber + "  –  " + DbHelper.Value<string>(r, "TypeName") +
                           "  –  tầng " + DbHelper.Value<int>(r, "Floor");
            lblStay.Text = Formatters.Date(checkIn) + "  →  " + Formatters.Date(checkOut) +
                           "   (" + Formatters.Nights(nights) + ")";
            lblGuests.Text = adults + " người lớn" + (children > 0 ? ", " + children + " trẻ em" : "") +
                             "   ·   " + Formatters.Money(DbHelper.Value<decimal>(r, "PricePerNight")) + "/đêm";

            Text = "Nhận phòng " + _roomNumber;
        }

        /// <summary>
        /// Hiện 4 điều kiện nhận phòng (cùng thứ tự kiểm tra với dbo.sp_CheckInRoom).
        /// Trả true nếu đủ cả 4.
        /// </summary>
        private bool ShowConditions(DataRow r)
        {
            DateTime now = DbHelper.Value<DateTime>(r, "ServerNow");
            DateTime today = now.Date;
            DateTime checkIn = DbHelper.Value<DateTime>(r, "CheckInDate");
            DateTime checkOut = DbHelper.Value<DateTime>(r, "CheckOutDate");

            // 1. Phiếu đã xác nhận (Confirmed, hoặc CheckedIn khi phòng khác cùng phiếu đã nhận trước).
            string bookingStatus = DbHelper.Value<string>(r, "BookingStatus");
            bool bookingOk = bookingStatus == BookingStatus.Confirmed || bookingStatus == BookingStatus.CheckedIn;
            SetCondition(lblBookingStatusMark, lblBookingStatusText, bookingOk,
                bookingOk
                    ? "Phiếu đã được xác nhận."
                    : bookingStatus == BookingStatus.Pending
                        ? "Phiếu chưa xác nhận (chờ cọc) – thu đủ cọc thì phiếu tự chuyển sang Đã xác nhận."
                        : "Phiếu đang ở trạng thái \"" + BookingStatus.ToVietnamese(bookingStatus) + "\", không nhận phòng được.");

            // 2. Đủ cọc của RIÊNG phòng này (tiền đã thu thực tế >= cọc yêu cầu).
            decimal required = DbHelper.Value<decimal>(r, "RequiredDeposit");
            decimal paid = DbHelper.Value<decimal>(r, "NetPaid");
            decimal missing = required > paid ? required - paid : 0m;
            bool depositOk = missing == 0m;
            SetCondition(lblDepositMark, lblDepositText, depositOk,
                depositOk
                    ? "Đã đủ cọc: đã thu " + Formatters.Money(paid) + " / yêu cầu " + Formatters.Money(required) + "."
                    : "Thiếu cọc " + Formatters.Money(missing) + " (đã thu " + Formatters.Money(paid) +
                      " / yêu cầu " + Formatters.Money(required) + ").");
            btnDeposit.Visible = !depositOk;

            // 3. Ngày: từ 00:00 ngày nhận đến trước ngày trả. Nhận sớm trong ngày (trước 14:00) miễn phí;
            //    khách đến trễ hơn ngày nhận vẫn nhận được, không giảm tiền các đêm đã giữ.
            bool dateOk;
            string dateText;
            if (today < checkIn)
            {
                dateOk = false;
                dateText = "Chưa đến ngày nhận phòng (" + Formatters.Date(checkIn) +
                           "). Muốn nhận sớm hơn phải sửa phiếu thêm đêm.";
            }
            else if (today >= checkOut)
            {
                dateOk = false;
                dateText = "Đã đến/qua ngày trả phòng (" + Formatters.Date(checkOut) + ") của lượt này.";
            }
            else if (today == checkIn)
            {
                dateOk = true;
                dateText = "Hôm nay là ngày nhận phòng." +
                           (now.TimeOfDay < BusinessRules.CheckInTime ? " Nhận sớm trước 14:00 – miễn phí." : "");
            }
            else
            {
                dateOk = true;
                dateText = "Khách đến trễ (ngày nhận " + Formatters.Date(checkIn) +
                           ") – vẫn nhận được, không giảm tiền các đêm đã giữ.";
            }
            SetCondition(lblDateMark, lblDateText, dateOk, dateText);

            // 4. Phòng phải trống (Available).
            string roomStatus = DbHelper.Value<string>(r, "RoomStatus");
            bool roomOk = roomStatus == RoomStatus.Available;
            string roomText;
            switch (roomStatus)
            {
                case RoomStatus.Available:
                    roomText = "Phòng trống, đã sẵn sàng.";
                    break;
                case RoomStatus.Cleaning:
                    roomText = "Phòng đang dọn – chờ buồng phòng dọn xong rồi bấm \"Kiểm tra lại\".";
                    break;
                case RoomStatus.Maintenance:
                    roomText = "Phòng đang bảo trì – cần đổi sang phòng khác (nút Đổi phòng ở màn hình lễ tân).";
                    break;
                case RoomStatus.Occupied:
                    roomText = "Phòng đang có khách khác – cần đổi phòng hoặc chờ khách cũ trả.";
                    break;
                default:
                    roomText = "Tình trạng phòng: " + RoomStatus.ToVietnamese(roomStatus) + ".";
                    break;
            }
            SetCondition(lblRoomStatusMark, lblRoomStatusText, roomOk, roomText);

            lblNow.Text = "Giờ nhận phòng sẽ ghi theo máy chủ lúc bấm \"Nhận phòng\" (hiện tại: " +
                          Formatters.DateTimeText(now) + ").";

            return bookingOk && depositOk && dateOk && roomOk;
        }

        private static void SetCondition(Label mark, Label text, bool ok, string message)
        {
            mark.Text = ok ? OkMark : FailMark;
            mark.ForeColor = ok ? OkColor : FailColor;
            text.Text = message;
            text.ForeColor = ok ? SystemColors.ControlText : FailColor;
        }

        /// <summary>Điền sẵn người ở = thông tin đã ghi trên phiếu (mặc định là người đặt).</summary>
        private void FillGuest(DataRow r)
        {
            if (_guestFilled) return;
            txtGuestName.Text = DbHelper.Value<string>(r, "GuestName", "");
            txtGuestIdNumber.Text = DbHelper.Value<string>(r, "GuestIdNumber", "");
            _guestFilled = true;
        }

        // ==================================================================
        // Thao tác
        // ==================================================================

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!ValidateGuest()) return;

            try
            {
                using (UiHelper.Busy(this))
                {
                    DbHelper.Exec("dbo.sp_CheckInRoom",
                        DbHelper.P("@BookingRoomId", _bookingRoomId),
                        DbHelper.P("@GuestName", txtGuestName.Text),
                        DbHelper.P("@GuestIdNumber", txtGuestIdNumber.Text),
                        Session.EmployeeParam());
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
                // Tình hình có thể đã đổi từ lúc mở hộp thoại (máy khác vừa check-in phòng này,
                // phòng chuyển sang dọn...): nạp lại để 4 dòng điều kiện chỉ ra lý do.
                Reload();
                return;
            }

            FormManager.NotifyDataChanged(DataArea.Stays);
            FormManager.NotifyDataChanged(DataArea.Rooms);
            FormManager.NotifyDataChanged(DataArea.Bookings);

            UiHelper.Info("Đã nhận phòng " + _roomNumber + " lúc " + Formatters.Time(DateTime.Now) + ".", this);
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Kiểm tra sơ bộ thông tin người ở (procedure vẫn kiểm tra lại):
        /// có CCCD thì phải có tên; CCCD/hộ chiếu chỉ gồm chữ và số, 6–20 ký tự.
        /// Để trống cả hai thì giữ nguyên thông tin trên phiếu.
        /// </summary>
        private bool ValidateGuest()
        {
            string name = txtGuestName.Text.Trim();
            string idNumber = txtGuestIdNumber.Text.Trim();

            if (idNumber.Length > 0 && name.Length == 0)
            {
                UiHelper.Warn("Nhập CCCD/hộ chiếu thì phải nhập họ tên người ở.", this);
                txtGuestName.Focus();
                return false;
            }

            if (idNumber.Length > 0)
            {
                bool valid = idNumber.Length >= 6;
                foreach (char c in idNumber)
                {
                    if (!char.IsLetterOrDigit(c) || c > 127) valid = false;
                }
                if (!valid)
                {
                    UiHelper.Warn("CCCD/hộ chiếu chỉ gồm chữ cái không dấu và chữ số, dài 6–20 ký tự.", this);
                    txtGuestIdNumber.Focus();
                    return false;
                }
            }
            return true;
        }

        private void btnDeposit_Click(object sender, EventArgs e)
        {
            // Cọc tính theo cả phiếu nên truyền BookingId; thu xong nạp lại để dòng "đủ cọc" chuyển xanh.
            if (FormManager.ShowDialog(new dlgPayment(PaymentMode.Deposit, _bookingId), this)) Reload();
        }
    }
}
