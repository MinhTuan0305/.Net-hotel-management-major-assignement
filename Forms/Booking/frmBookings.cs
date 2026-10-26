using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HotelManagement.Core;
using Hotel_management_system.Forms.Billing;

namespace Hotel_management_system.Forms.Booking
{
    /// <summary>
    /// Danh sách phiếu đặt (Admin, Manager, Receptionist). Đặc tả: KE_HOACH mục 3; nghiệp vụ mục 8.
    /// - Lưới trên: vw_BookingSummary, lọc trạng thái / khoảng ngày nhận / mã phiếu, tên, SĐT.
    ///   Phiếu chờ cọc tô cam và hiện thời gian giữ chỗ còn lại (24 giờ từ lúc tạo).
    /// - Lưới dưới: các phòng của phiếu đang chọn (vw_RoomStays + vw_RoomStayFolio).
    /// - Mở màn hình thì gọi sp_ExpirePendingBookings (SQL Express không có SQL Agent để tự chạy).
    /// </summary>
    public partial class frmBookings : Form
    {
        private const string SqlList = @"
SELECT b.BookingId, b.Status, b.DepositRate, b.CustomerId, b.CustomerName, b.CustomerPhone,
       b.CreatedAt, b.CreatedByName, b.FirstCheckIn, b.LastCheckOut, b.RoomCount, b.BookedCount,
       b.InHouseCount, b.RequiredDeposit, b.NetPaid, b.MissingDeposit, b.Note, b.CancelReason,
       CASE WHEN b.Status = 'Pending' AND b.ConfirmedAt IS NULL
            THEN DATEDIFF(MINUTE, SYSDATETIME(), DATEADD(HOUR, 24, b.CreatedAt)) END AS HoldMinutesLeft
FROM dbo.vw_BookingSummary b
WHERE (@Status IS NULL OR b.Status = @Status)
  AND (@From IS NULL OR b.FirstCheckIn >= @From)
  AND (@To IS NULL OR b.FirstCheckIn <= @To)
  AND (@Kw IS NULL OR b.CustomerName COLLATE Vietnamese_CI_AI LIKE @Kw OR b.CustomerPhone LIKE @Kw
       OR CAST(b.BookingId AS VARCHAR(10)) = @IdText)
ORDER BY b.CreatedAt DESC;";

        private const string SqlRooms = @"
SELECT s.BookingRoomId, s.RoomNumber, s.TypeName, s.CheckInDate, s.CheckOutDate, s.Nights, s.Adults, s.Children,
       s.PricePerNight, s.StayStatus, s.RoomStatus, s.GuestName, s.ActualCheckIn, s.ActualCheckOut, s.CancelReason,
       f.RequiredDeposit, f.NetPaid
FROM dbo.vw_RoomStays s
JOIN dbo.vw_RoomStayFolio f ON f.BookingRoomId = s.BookingRoomId
WHERE s.BookingId = @Id
ORDER BY s.RoomNumber;";

        private static readonly Color PendingColor = Color.FromArgb(255, 224, 178);
        private static readonly Color CancelledColor = SystemColors.ControlLight;

        private bool _loading;
        private bool _reloadPending;

        public frmBookings()
        {
            InitializeComponent();
            UiHelper.StyleGrid(dgvBookings);
            UiHelper.StyleGrid(dgvRooms);
            UiHelper.TranslateColumn(dgvBookings, "Status", BookingStatus.ToVietnamese);
            UiHelper.TranslateColumn(dgvBookings, "HoldMinutesLeft", FormatHold);
            UiHelper.ColorRows(dgvBookings, "Status", ColorByStatus);
            UiHelper.TranslateColumn(dgvRooms, "StayStatus", StayStatus.ToVietnamese);
            UiHelper.TranslateColumn(dgvRooms, "RoomStatus", RoomStatus.ToVietnamese);
        }

        private void frmBookings_Load(object sender, EventArgs e)
        {
            if (!UiHelper.RequireRole(this, Roles.Admin, Roles.Manager, Roles.Receptionist)) return;

            _loading = true;
            try
            {
                Lookups.LoadBookingStatuses(cboStatus, Lookups.AllText);
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
            dtpFrom.Value = DateTime.Today;
            dtpTo.Value = DateTime.Today.AddDays(30);
            _loading = false;

            ExpirePending();
            LoadData();
            FormManager.Subscribe(this, OnDataChanged);
        }

        private void ExpirePending()
        {
            try
            {
                DataTable expired = DbHelper.Query("dbo.sp_ExpirePendingBookings");
                if (expired.Rows.Count > 0)
                {
                    UiHelper.Warn(expired.Rows.Count + " phiếu vừa bị hủy vì quá 24 giờ chưa đủ cọc nhưng đã có tiền thu. " +
                                  "Hãy liên hệ khách và ghi hoàn tiền thủ công ở Sổ thu.", this);
                    FormManager.NotifyDataChanged(DataArea.Bookings);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        // ==================================================================
        // Nạp dữ liệu
        // ==================================================================

        private void LoadData()
        {
            try
            {
                string search = txtSearch.Text.Trim().TrimStart('#');
                DataTable dt = DbHelper.QueryText(SqlList,
                    DbHelper.P("@Status", Lookups.SelectedString(cboStatus)),
                    DbHelper.P("@From", chkDateFilter.Checked ? (object)dtpFrom.Value.Date : null),
                    DbHelper.P("@To", chkDateFilter.Checked ? (object)dtpTo.Value.Date : null),
                    DbHelper.P("@Kw", DbHelper.LikeContains(search)),
                    DbHelper.P("@IdText", search));
                UiHelper.Bind(dgvBookings, dt, "BookingId");
                UiHelper.ShowColumns(dgvBookings,
                    "BookingId", "Mã phiếu",
                    "Status", "Trạng thái",
                    "HoldMinutesLeft", "Hạn giữ chỗ",
                    "CustomerName", "Khách đặt",
                    "CustomerPhone", "SĐT",
                    "FirstCheckIn", "Nhận",
                    "LastCheckOut", "Trả",
                    "RoomCount", "Số phòng",
                    "DepositRate", "Cọc %",
                    "RequiredDeposit", "Cọc yêu cầu",
                    "NetPaid", "Đã thu",
                    "MissingDeposit", "Thiếu cọc",
                    "CreatedAt", "Ngày tạo",
                    "CreatedByName", "Người tạo");
                UiHelper.FormatDateColumns(dgvBookings, "FirstCheckIn", "LastCheckOut");
                Text = "Phiếu đặt phòng (" + dt.Rows.Count + ")";
                LoadRooms();
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        private void LoadRooms()
        {
            int? id = UiHelper.SelectedId(dgvBookings, "BookingId");
            if (id == null)
            {
                dgvRooms.DataSource = null;
                lblRooms.Text = "Các phòng của phiếu";
                UpdateButtons();
                return;
            }
            try
            {
                DataTable dt = DbHelper.QueryText(SqlRooms, DbHelper.P("@Id", id.Value));
                UiHelper.Bind(dgvRooms, dt, "BookingRoomId");
                UiHelper.ShowColumns(dgvRooms,
                    "RoomNumber", "Phòng",
                    "TypeName", "Loại",
                    "CheckInDate", "Ngày nhận",
                    "CheckOutDate", "Ngày trả",
                    "Nights", "Đêm",
                    "Adults", "NL",
                    "Children", "TE",
                    "PricePerNight", "Giá/đêm",
                    "StayStatus", "Trạng thái",
                    "RoomStatus", "Tình trạng phòng",
                    "RequiredDeposit", "Cọc yêu cầu",
                    "NetPaid", "Đã thu",
                    "GuestName", "Người ở",
                    "CancelReason", "Lý do hủy");
                lblRooms.Text = "Các phòng của phiếu #" + id.Value + " (" + dt.Rows.Count + ")";
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            DataRow b = UiHelper.SelectedRow(dgvBookings);
            DataRow s = UiHelper.SelectedRow(dgvRooms);
            int booked = b == null ? 0 : DbHelper.Value<int>(b, "BookedCount");
            btnEdit.Enabled = b != null;
            btnDeposit.Enabled = booked > 0;
            btnCancelBooking.Enabled = booked > 0 && DbHelper.Value<int>(b, "InHouseCount") == 0;
            btnCancelRoom.Enabled = s != null && DbHelper.Value<string>(s, "StayStatus") == StayStatus.Booked;
        }

        private static string FormatHold(string value)
        {
            int minutes;
            if (!int.TryParse(value, out minutes)) return "";
            return minutes > 0 ? "còn " + Formatters.Duration(minutes) : "đã quá hạn";
        }

        private static Color ColorByStatus(string status)
        {
            if (status == BookingStatus.Pending) return PendingColor;
            if (status == BookingStatus.Cancelled) return CancelledColor;
            return Color.Empty;
        }

        // ==================================================================
        // Sự kiện
        // ==================================================================

        private void filter_Changed(object sender, EventArgs e)
        {
            if (!_loading) LoadData();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ExpirePending();
            LoadData();
        }

        private void dgvBookings_SelectionChanged(object sender, EventArgs e)
        {
            LoadRooms();
        }

        private void dgvRooms_SelectionChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void dgvBookings_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) btnEdit_Click(sender, e);
        }

        private void OnDataChanged(string area)
        {
            if (area != DataArea.Bookings && area != DataArea.Payments && area != DataArea.Stays) return;
            if (_reloadPending) return;
            _reloadPending = true;
            BeginInvoke(new Action(() =>
            {
                _reloadPending = false;
                if (!IsDisposed) LoadData();
            }));
        }

        // ==================================================================
        // Thao tác
        // ==================================================================

        private void btnNew_Click(object sender, EventArgs e)
        {
            FormManager.Open("new", () => new frmBookingEdit(null));
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int? id = UiHelper.SelectedId(dgvBookings, "BookingId");
            if (id == null) return;
            int bookingId = id.Value;
            FormManager.Open("booking-" + bookingId, () => new frmBookingEdit(bookingId));
        }

        private void btnDeposit_Click(object sender, EventArgs e)
        {
            int? id = UiHelper.SelectedId(dgvBookings, "BookingId");
            if (id == null) return;
            if (FormManager.ShowDialog(new dlgPayment(PaymentMode.Deposit, id.Value), this)) LoadData();
        }

        private void btnCancelRoom_Click(object sender, EventArgs e)
        {
            int? bookingId = UiHelper.SelectedId(dgvBookings, "BookingId");
            int? stayId = UiHelper.SelectedId(dgvRooms, "BookingRoomId");
            if (bookingId == null || stayId == null) return;
            if (FormManager.ShowDialog(new dlgCancel(bookingId.Value, stayId), this)) LoadData();
        }

        private void btnCancelBooking_Click(object sender, EventArgs e)
        {
            int? bookingId = UiHelper.SelectedId(dgvBookings, "BookingId");
            if (bookingId == null) return;
            if (FormManager.ShowDialog(new dlgCancel(bookingId.Value, null), this)) LoadData();
        }
    }
}
