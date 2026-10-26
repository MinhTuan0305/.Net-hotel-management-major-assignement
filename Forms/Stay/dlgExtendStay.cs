using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HotelManagement.Core;

namespace Hotel_management_system.Forms.Stay
{
    /// <summary>
    /// Gia hạn ngày trả của một phòng đang ở (Admin, Manager, Receptionist). Đặc tả: KE_HOACH mục 4; nghiệp vụ mục 11.
    /// dbo.sp_ExtendStay kiểm tra ngày mới phải sau ngày cũ và các đêm thêm không trùng lịch khách khác (50005).
    /// Đêm thêm tính theo giá đêm đã chốt của lượt lưu trú. Form chỉ tạm tính và báo trước ngày khách kế tiếp nhận phòng.
    /// Trả phòng sau 18:00 ngày trả cũng phải gia hạn thêm 1 đêm (frmCheckOut mở hộp thoại này).
    /// </summary>
    public partial class dlgExtendStay : Form
    {
        private const string SqlStay = @"
SELECT s.RoomId, s.RoomNumber, s.GuestName, s.CheckInDate, s.CheckOutDate, s.PricePerNight, s.StayStatus,
       (SELECT MIN(br.CheckInDate) FROM dbo.BookingRooms br
        WHERE br.RoomId = s.RoomId AND br.BookingRoomId <> s.BookingRoomId
          AND br.Status IN ('Booked', 'CheckedIn') AND br.CheckInDate >= s.CheckOutDate) AS NextCheckIn
FROM dbo.vw_RoomStays s
WHERE s.BookingRoomId = @Id;";

        private readonly int _bookingRoomId;
        private DateTime _oldCheckOut;
        private decimal _price;
        private DateTime? _nextCheckIn;
        private string _roomNumber;

        // Constructor đã chốt (CLAUDE.md mục 7) – không đổi tham số.
        public dlgExtendStay(int bookingRoomId)
        {
            InitializeComponent();
            _bookingRoomId = bookingRoomId;
            UiHelper.SetupDialog(this, btnOk, btnCancel);
        }

        private void dlgExtendStay_Load(object sender, EventArgs e)
        {
            if (!UiHelper.RequireRole(this, Roles.Admin, Roles.Manager, Roles.Receptionist)) return;
            try
            {
                DataTable dt = DbHelper.QueryText(SqlStay, DbHelper.P("@Id", _bookingRoomId));
                if (dt.Rows.Count == 0 || DbHelper.Value<string>(dt.Rows[0], "StayStatus") != StayStatus.CheckedIn)
                {
                    UiHelper.Warn("Chỉ gia hạn được phòng đang ở.", this);
                    BeginInvoke(new Action(Close));
                    return;
                }

                DataRow r = dt.Rows[0];
                _roomNumber = DbHelper.Value<string>(r, "RoomNumber");
                _oldCheckOut = DbHelper.Value<DateTime>(r, "CheckOutDate");
                _price = DbHelper.Value<decimal>(r, "PricePerNight");
                _nextCheckIn = DbHelper.Value<DateTime?>(r, "NextCheckIn");

                Text = "Gia hạn phòng " + _roomNumber;
                lblTitle.Text = Text;
                lblGuest.Text = DbHelper.Value<string>(r, "GuestName") + " – nhận ngày " + Formatters.Date(r["CheckInDate"]);
                lblCurrent.Text = Formatters.Date(_oldCheckOut);
                lblPrice.Text = Formatters.Money(_price);

                if (_nextCheckIn.HasValue)
                {
                    lblNextBooking.Text = "Phòng có khách khác nhận ngày " + Formatters.Date(_nextCheckIn.Value) +
                                          ": chỉ gia hạn được tới ngày đó.";
                }

                // Mặc định thêm 1 đêm. Không đặt MinDate để tránh lỗi khi ngày trả cũ đã qua.
                dtpNewCheckOut.Value = _oldCheckOut.AddDays(1);
                UpdateEstimate();
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
                BeginInvoke(new Action(Close));
            }
        }

        private void dtpNewCheckOut_ValueChanged(object sender, EventArgs e)
        {
            UpdateEstimate();
        }

        private void UpdateEstimate()
        {
            int extra = (dtpNewCheckOut.Value.Date - _oldCheckOut).Days;
            if (extra <= 0)
            {
                lblExtra.Text = "Ngày trả mới phải sau " + Formatters.Date(_oldCheckOut) + ".";
                lblExtra.ForeColor = Color.FromArgb(192, 0, 0);
                btnOk.Enabled = false;
                return;
            }

            lblExtra.Text = "Thêm " + Formatters.Nights(extra) + " × " + Formatters.Money(_price) + " = " +
                            Formatters.Money(extra * _price) + " (chưa VAT)";
            lblExtra.ForeColor = Color.FromArgb(31, 78, 121);
            bool conflict = _nextCheckIn.HasValue && dtpNewCheckOut.Value.Date > _nextCheckIn.Value;
            lblNextBooking.ForeColor = conflict ? Color.FromArgb(192, 0, 0) : SystemColors.ControlText;
            btnOk.Enabled = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                using (UiHelper.Busy(this))
                {
                    DbHelper.Exec("dbo.sp_ExtendStay",
                        DbHelper.P("@BookingRoomId", _bookingRoomId),
                        DbHelper.P("@NewCheckOutDate", SqlDbType.Date, dtpNewCheckOut.Value.Date),
                        DbHelper.P("@Reason", txtReason.Text),
                        Session.EmployeeParam());
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
                if (ErrorHandler.GetSqlNumber(ex) == SqlErrorCodes.ScheduleConflict)
                {
                    UiHelper.Info("Hãy chọn ngày trả sớm hơn (trước ngày khách kế tiếp nhận phòng). Nếu khách cần ở lâu hơn, " +
                                  "lễ tân có thể check-out phòng này rồi đặt phòng khác cho khách.", this);
                }
                return;
            }

            FormManager.NotifyDataChanged(DataArea.Stays);
            UiHelper.Info("Đã gia hạn phòng " + _roomNumber + " đến " + Formatters.Date(dtpNewCheckOut.Value) + ".", this);
            DialogResult = DialogResult.OK;
        }
    }
}
