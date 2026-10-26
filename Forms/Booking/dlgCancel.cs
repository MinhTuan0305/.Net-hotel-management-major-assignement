using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using HotelManagement.Core;

namespace Hotel_management_system.Forms.Booking
{
    /// <summary>
    /// Hủy một phòng / cả phiếu / đánh dấu no-show (Admin, Manager, Receptionist).
    /// Đặc tả: KE_HOACH mục 3; nghiệp vụ mục 8.
    /// Chính sách (đã nằm trong procedure, form chỉ HIỂN THỊ trước để lễ tân báo khách):
    ///   - Hủy trước 14:00 ngày nhận ít nhất 48 giờ: hoàn 100% tiền đã thu; muộn hơn: mất cọc.
    ///   - No-show chỉ đánh dấu được sau 12:00 ngày hôm sau ngày nhận, không hoàn tiền.
    /// bookingRoomId = null: hủy cả phiếu (sp_CancelBooking), ngược lại hủy một phòng (sp_CancelBookingRoom).
    /// </summary>
    public partial class dlgCancel : Form
    {
        // Số hoàn dự kiến và quyền no-show tính theo giờ SQL Server, cùng công thức với sp_CancelBookingRoom.
        private const string SqlRooms = @"
SELECT s.BookingRoomId, s.BookingId, s.CustomerName, s.RoomNumber, s.TypeName, s.CheckInDate, s.CheckOutDate,
       f.NetPaid,
       CAST(CASE WHEN SYSDATETIME() <= DATEADD(HOUR, -48, DATEADD(HOUR, 14, CAST(s.CheckInDate AS DATETIME2(0))))
                  AND f.NetPaid > 0 THEN f.NetPaid ELSE 0 END AS DECIMAL(14,0)) AS ExpectedRefund,
       CAST(CASE WHEN SYSDATETIME() >= DATEADD(HOUR, 12, CAST(DATEADD(DAY, 1, s.CheckInDate) AS DATETIME2(0)))
                 THEN 1 ELSE 0 END AS BIT) AS CanNoShow
FROM dbo.vw_RoomStays s
JOIN dbo.vw_RoomStayFolio f ON f.BookingRoomId = s.BookingRoomId
WHERE s.BookingId = @BookingId AND s.StayStatus = 'Booked'
  AND (@StayId IS NULL OR s.BookingRoomId = @StayId)
ORDER BY s.RoomNumber;";

        private readonly int _bookingId;
        private readonly int? _bookingRoomId;
        private decimal _expectedRefund;
        private decimal _paid;

        // Constructor đã chốt (CLAUDE.md mục 7) – không đổi tham số.
        public dlgCancel(int bookingId, int? bookingRoomId)
        {
            InitializeComponent();
            _bookingId = bookingId;
            _bookingRoomId = bookingRoomId;
            UiHelper.SetupDialog(this, btnOk, btnCancel);
            UiHelper.StyleGrid(dgvRooms);
        }

        private void dlgCancel_Load(object sender, EventArgs e)
        {
            if (!UiHelper.RequireRole(this, Roles.Admin, Roles.Manager, Roles.Receptionist)) return;
            try
            {
                Lookups.LoadPaymentMethods(cboRefundMethod, Lookups.ChooseText);
                if (!LoadRooms()) BeginInvoke(new Action(Close));
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
                BeginInvoke(new Action(Close));
            }
        }

        private bool LoadRooms()
        {
            if (_bookingRoomId == null)
            {
                // Cả phiếu: procedure không cho hủy khi có phòng đang ở; báo trước cho rõ.
                int inHouse = DbHelper.Scalar<int>(
                    "SELECT COUNT(*) FROM dbo.BookingRooms WHERE BookingId = @Id AND Status = 'CheckedIn'",
                    DbHelper.P("@Id", _bookingId));
                if (inHouse > 0)
                {
                    UiHelper.Warn("Phiếu #" + _bookingId + " đang có phòng lưu trú nên không hủy cả phiếu được. " +
                                  "Hãy hủy từng phòng chưa nhận.", this);
                    return false;
                }
            }

            DataTable dt = DbHelper.QueryText(SqlRooms,
                DbHelper.P("@BookingId", _bookingId), DbHelper.P("@StayId", _bookingRoomId));
            if (dt.Rows.Count == 0)
            {
                UiHelper.Warn("Không còn phòng nào chờ nhận để hủy.", this);
                return false;
            }

            UiHelper.Bind(dgvRooms, dt);
            UiHelper.ShowColumns(dgvRooms,
                "RoomNumber", "Phòng",
                "TypeName", "Loại",
                "CheckInDate", "Ngày nhận",
                "CheckOutDate", "Ngày trả",
                "NetPaid", "Đã thu",
                "ExpectedRefund", "Hoàn dự kiến");

            _paid = 0;
            _expectedRefund = 0;
            bool canNoShow = true;
            foreach (DataRow r in dt.Rows)
            {
                _paid += DbHelper.Value<decimal>(r, "NetPaid");
                _expectedRefund += DbHelper.Value<decimal>(r, "ExpectedRefund");
                canNoShow &= DbHelper.Value<bool>(r, "CanNoShow");
            }

            string customer = DbHelper.Value<string>(dt.Rows[0], "CustomerName");
            if (_bookingRoomId == null)
            {
                Text = "Hủy cả phiếu #" + _bookingId;
                lblTitle.Text = Text + " (" + dt.Rows.Count + " phòng chưa nhận)";
                // No-show chỉ đánh dấu theo từng phòng (sp_CancelBooking không có tùy chọn này).
                chkNoShow.Visible = false;
            }
            else
            {
                Text = "Hủy phòng " + DbHelper.Value<string>(dt.Rows[0], "RoomNumber") + " – phiếu #" + _bookingId;
                lblTitle.Text = Text;
                chkNoShow.Enabled = canNoShow;
                if (!canNoShow) chkNoShow.Text += " (chỉ sau 12:00 ngày hôm sau ngày nhận)";
            }
            lblSubtitle.Text = "Khách: " + customer + "   ·   Đã thu: " + Formatters.Money(_paid);
            UpdatePolicy();
            return true;
        }

        private void chkNoShow_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePolicy();
        }

        /// <summary>Hiện kết quả dự kiến và bật/tắt phần hoàn tiền.</summary>
        private void UpdatePolicy()
        {
            bool noShow = chkNoShow.Visible && chkNoShow.Checked;
            decimal refund = noShow ? 0 : _expectedRefund;

            if (noShow)
            {
                lblPolicy.Text = "No-show: không hoàn tiền (giữ " + Formatters.Money(_paid) + ").";
                lblPolicy.ForeColor = Color.FromArgb(192, 0, 0);
            }
            else if (refund > 0)
            {
                lblPolicy.Text = "Hủy trước giờ nhận ít nhất " + BusinessRules.FreeCancelHours + " giờ: hoàn " +
                                 Formatters.Money(refund) + ". Chọn cách hoàn bên dưới.";
                lblPolicy.ForeColor = Color.FromArgb(0, 128, 0);
            }
            else if (_paid > 0)
            {
                lblPolicy.Text = "Hủy muộn (dưới " + BusinessRules.FreeCancelHours + " giờ trước 14:00 ngày nhận): " +
                                 "không hoàn tiền, khách mất " + Formatters.Money(_paid) + ".";
                lblPolicy.ForeColor = Color.FromArgb(192, 0, 0);
            }
            else
            {
                lblPolicy.Text = "Chưa thu tiền nên không phát sinh hoàn tiền.";
                lblPolicy.ForeColor = SystemColors.ControlText;
            }

            grpRefund.Enabled = refund > 0;
            lblReason.Text = noShow ? "Ghi chú:" : "Lý do hủy:*";
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            bool noShow = chkNoShow.Visible && chkNoShow.Checked;
            if (!noShow && !UiHelper.RequireText(txtReason, "lý do hủy")) return;

            string refundMethod = grpRefund.Enabled ? Lookups.SelectedString(cboRefundMethod) : null;
            if (grpRefund.Enabled)
            {
                if (!UiHelper.RequireSelection(cboRefundMethod, "phương thức hoàn tiền")) return;
                if (PayMethod.NeedsReference(refundMethod) &&
                    !UiHelper.RequireText(txtRefundReference, "mã giao dịch hoàn tiền")) return;
            }

            string question = _bookingRoomId == null
                ? "Hủy toàn bộ các phòng chưa nhận của phiếu #" + _bookingId + "?"
                : (noShow ? "Đánh dấu no-show cho phòng này?" : "Hủy phòng này?");
            if (!UiHelper.Confirm(question, defaultNo: true, owner: this)) return;

            decimal refunded;
            try
            {
                using (UiHelper.Busy(this))
                {
                    if (_bookingRoomId == null)
                    {
                        SqlParameter total = DbHelper.Out("@TotalRefund", SqlDbType.Decimal);
                        DbHelper.Exec("dbo.sp_CancelBooking",
                            DbHelper.P("@BookingId", _bookingId),
                            DbHelper.P("@Reason", txtReason.Text),
                            DbHelper.P("@RefundMethod", refundMethod),
                            DbHelper.P("@RefundReference", txtRefundReference.Text),
                            Session.EmployeeParam(),
                            total);
                        refunded = DbHelper.OutValue<decimal>(total);
                    }
                    else
                    {
                        SqlParameter amount = DbHelper.Out("@RefundAmount", SqlDbType.Decimal);
                        DbHelper.Exec("dbo.sp_CancelBookingRoom",
                            DbHelper.P("@BookingRoomId", _bookingRoomId.Value),
                            DbHelper.P("@Reason", txtReason.Text),
                            DbHelper.P("@IsNoShow", noShow),
                            DbHelper.P("@RefundMethod", refundMethod),
                            DbHelper.P("@RefundReference", txtRefundReference.Text),
                            Session.EmployeeParam(),
                            amount);
                        refunded = DbHelper.OutValue<decimal>(amount);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
                return;
            }

            FormManager.NotifyDataChanged(DataArea.Bookings);
            FormManager.NotifyDataChanged(DataArea.Stays);
            FormManager.NotifyDataChanged(DataArea.Payments);

            // Số hoàn lấy từ OUTPUT của procedure (số chính thức), không phải số dự kiến trên form.
            UiHelper.Info((noShow ? "Đã đánh dấu no-show." : "Đã hủy.") +
                          (refunded > 0 ? " Hoàn cho khách " + Formatters.Money(refunded) + "." : " Không hoàn tiền."), this);
            DialogResult = DialogResult.OK;
        }
    }
}
