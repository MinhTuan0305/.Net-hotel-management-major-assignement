using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HotelManagement.Core;
using Hotel_management_system.Forms.Billing;

namespace Hotel_management_system.Forms.Stay
{
    /// <summary>
    /// Trả phòng (check-out) MỘT phòng (Admin, Manager, Receptionist). Đặc tả: KE_HOACH mục 4; nghiệp vụ mục 11–13.
    ///   1. Load -> dbo.sp_PreviewCheckOut: bảng tính tại thời điểm hiện tại (tiền phòng, dịch vụ, phụ thu trễ,
    ///      VAT, đã thu, còn phải thu/hoàn). GIỮ NGUYÊN cột CheckOutAt trả về để truyền vào bước 3.
    ///   2. Nhập giảm giá + lý do -> "Tính lại" (xem trước lại với @Discount). Nhập người mua, cách thu/hoàn (ucSettlement).
    ///   3. Xác nhận -> dbo.sp_CheckOutRoom với đúng CheckOutAt ở bước 1 (bảng tính hiệu lực 10 phút).
    ///      DB ghi phụ thu trễ, lập hóa đơn Paid, thu đủ/hoàn dư, phòng -> Cleaning, tạo việc dọn, tính lại phiếu.
    ///   4. Thành công -> in hóa đơn (frmInvoicePrint).
    /// Trả sau 18:00 ngày trả: DB chặn (MustExtend) -> hiện nút gia hạn thêm đêm.
    /// </summary>
    public partial class frmCheckOut : Form
    {
        private const string SqlStay = @"
SELECT s.BookingId, s.RoomNumber, s.TypeName, s.GuestName, s.CustomerName, s.CheckInDate, s.CheckOutDate,
       s.ActualCheckIn, s.PricePerNight, s.StayStatus
FROM dbo.vw_RoomStays s
WHERE s.BookingRoomId = @Id;";

        private static readonly Color DueColor = Color.FromArgb(192, 0, 0);
        private static readonly Color RefundColor = Color.FromArgb(0, 110, 0);

        private readonly int _bookingRoomId;
        private string _roomNumber;
        private DateTime? _checkOutAt;   // CheckOutAt của bảng tính hiện tại (truyền nguyên vào sp_CheckOutRoom)
        private DateTime _quotedAt;      // giờ máy lúc lấy bảng tính (đếm ngược 10 phút)
        private decimal _total;
        private decimal _balance;
        private bool _mustExtend;
        private bool _stale;             // giảm giá đổi sau lần tính gần nhất

        // Constructor đã chốt (CLAUDE.md mục 7) – không đổi tham số.
        public frmCheckOut(int bookingRoomId)
        {
            InitializeComponent();
            _bookingRoomId = bookingRoomId;
        }

        private void frmCheckOut_Load(object sender, EventArgs e)
        {
            if (!UiHelper.RequireRole(this, Roles.Admin, Roles.Manager, Roles.Receptionist)) return;
            try
            {
                DataTable dt = DbHelper.QueryText(SqlStay, DbHelper.P("@Id", _bookingRoomId));
                if (dt.Rows.Count == 0 || DbHelper.Value<string>(dt.Rows[0], "StayStatus") != StayStatus.CheckedIn)
                {
                    UiHelper.Warn("Chỉ trả phòng được cho phòng đang ở. Hãy làm mới màn hình lễ tân.", this);
                    BeginInvoke(new Action(Close));
                    return;
                }
                DataRow r = dt.Rows[0];
                _roomNumber = DbHelper.Value<string>(r, "RoomNumber");
                Text = "Trả phòng " + _roomNumber;
                lblRoom.Text = "Trả phòng " + _roomNumber + " – " + DbHelper.Value<string>(r, "TypeName") + " – " +
                               DbHelper.Value<string>(r, "GuestName") + "   (phiếu #" + DbHelper.Value<int>(r, "BookingId") + ")";
                lblStay.Text = "Nhận lúc " + Formatters.DateTimeText(r["ActualCheckIn"]) + "   ·   Ngày trả theo phiếu " +
                               Formatters.Date(r["CheckOutDate"]) + " (trước 12:00)   ·   Giá/đêm " +
                               Formatters.Money(DbHelper.Value<decimal>(r, "PricePerNight"));
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
                BeginInvoke(new Action(Close));
                return;
            }

            Preview();
            timerValidity.Start();
        }

        // ==================================================================
        // Bảng tính (sp_PreviewCheckOut)
        // ==================================================================

        /// <summary>Lấy bảng tính mới tại thời điểm hiện tại với số giảm giá đang nhập.</summary>
        private bool Preview()
        {
            DataTable dt;
            try
            {
                using (UiHelper.Busy(this))
                {
                    dt = DbHelper.Query("dbo.sp_PreviewCheckOut",
                        DbHelper.P("@BookingRoomId", _bookingRoomId),
                        DbHelper.P("@CheckOutAt", null),
                        DbHelper.P("@Discount", ucSettlement.Discount),
                        DbHelper.P("@TaxRate", BusinessRules.DefaultTaxRate));
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
                _checkOutAt = null;
                btnConfirm.Enabled = false;
                return false;
            }
            if (dt.Rows.Count == 0) return false;

            DataRow q = dt.Rows[0];
            _checkOutAt = DbHelper.Value<DateTime>(q, "CheckOutAt");
            _quotedAt = DateTime.Now;
            _total = DbHelper.Value<decimal>(q, "TotalAmount");
            _balance = DbHelper.Value<decimal>(q, "Balance");
            _mustExtend = DbHelper.Value<bool>(q, "MustExtend");
            ShowQuote(q);
            return true;
        }

        private void ShowQuote(DataRow q)
        {
            int nights = DbHelper.Value<int>(q, "Nights");
            decimal price = DbHelper.Value<decimal>(q, "PricePerNight");
            decimal roomCharge = DbHelper.Value<decimal>(q, "RoomCharge");
            decimal services = DbHelper.Value<decimal>(q, "ServiceChargeBefore");
            int lateMinutes = DbHelper.Value<int>(q, "LateMinutes");
            decimal? lateFee = DbHelper.Value<decimal?>(q, "LateFee");
            decimal subTotal = DbHelper.Value<decimal>(q, "SubTotal");
            decimal discount = ucSettlement.Discount;
            decimal netPaid = DbHelper.Value<decimal>(q, "NetPaid");
            decimal tax = _total - (subTotal - discount);

            string lateText;
            if (lateMinutes <= 0) lateText = "không trễ";
            else if (lateMinutes <= BusinessRules.LateFreeMinutes) lateText = "trễ " + Formatters.Duration(lateMinutes) + " (miễn phí)";
            else lateText = "trễ " + Formatters.Duration(lateMinutes);

            var captions = new List<string>
            {
                "Thời điểm trả phòng:",
                "Tiền phòng (" + Formatters.Nights(nights) + " × " + Formatters.Number(price) + "):",
                "Dịch vụ đã ghi:",
                "Phụ thu trả trễ (" + lateText + "):",
                "Cộng trước giảm giá:",
                "Giảm giá:",
                "Thuế GTGT " + Formatters.Percent(BusinessRules.DefaultTaxRate) + ":",
                "",
                "Đã thu (cọc + thanh toán − hoàn):"
            };
            var values = new List<string>
            {
                Formatters.DateTimeText(_checkOutAt.Value),
                Formatters.Money(roomCharge),
                Formatters.Money(services),
                lateFee.HasValue ? Formatters.Money(lateFee.Value) : "phải gia hạn",
                Formatters.Money(subTotal),
                discount > 0 ? "− " + Formatters.Money(discount) : Formatters.Money(0),
                Formatters.Money(tax),
                "",
                Formatters.Money(netPaid)
            };
            // Hai label đặt cạnh nhau, cùng số dòng nên mỗi dòng chú thích khớp đúng dòng số tiền.
            lblQuoteCaptions.Text = string.Join("\n", captions);
            lblQuoteValues.Text = string.Join("\n", values);

            lblTotal.Text = "TỔNG HÓA ĐƠN: " + Formatters.Money(_total);
            if (_balance > 0)
            {
                lblBalance.Text = "Còn phải thu: " + Formatters.Money(_balance);
                lblBalance.ForeColor = DueColor;
            }
            else if (_balance < 0)
            {
                lblBalance.Text = "Khách trả dư, phải hoàn: " + Formatters.Money(-_balance);
                lblBalance.ForeColor = RefundColor;
            }
            else
            {
                lblBalance.Text = "Đã thu đủ.";
                lblBalance.ForeColor = SystemColors.ControlText;
            }

            ucSettlement.SetQuote(subTotal, _balance);
            // Đặt SAU SetQuote: SetQuote có thể hạ giảm giá cho khỏi vượt tổng và bắn DiscountChanged.
            _stale = false;
            lblMustExtend.Visible = btnExtend.Visible = _mustExtend;
            lblStale.Visible = false;
            UpdateValidity();
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            btnConfirm.Enabled = _checkOutAt.HasValue && !_mustExtend && !_stale;
        }

        // ==================================================================
        // Hiệu lực 10 phút của bảng tính
        // ==================================================================

        private TimeSpan Remaining()
        {
            return TimeSpan.FromMinutes(BusinessRules.CheckOutQuoteValidMinutes) - (DateTime.Now - _quotedAt);
        }

        private void UpdateValidity()
        {
            if (!_checkOutAt.HasValue)
            {
                lblValidity.Text = "";
                return;
            }
            TimeSpan left = Remaining();
            if (left < TimeSpan.Zero) left = TimeSpan.Zero;
            lblValidity.Text = "Bảng tính lập lúc " + Formatters.Time(_checkOutAt.Value) + ", còn hiệu lực " +
                               ((int)left.TotalMinutes).ToString("00") + ":" + left.Seconds.ToString("00") +
                               " (quá " + BusinessRules.CheckOutQuoteValidMinutes + " phút sẽ tự tính lại).";
        }

        private void timerValidity_Tick(object sender, EventArgs e)
        {
            if (!_checkOutAt.HasValue) return;
            if (Remaining() <= TimeSpan.Zero)
            {
                // Hết hạn: tính lại theo giờ mới (phụ thu trễ có thể tăng).
                timerValidity.Stop();
                Preview();
                timerValidity.Start();
                return;
            }
            UpdateValidity();
        }

        // ==================================================================
        // Thao tác
        // ==================================================================

        private void ucSettlement_DiscountChanged(object sender, EventArgs e)
        {
            if (!_checkOutAt.HasValue) return;
            _stale = true;
            lblStale.Visible = true;
            UpdateButtons();
        }

        private void btnRecalc_Click(object sender, EventArgs e)
        {
            Preview();
        }

        private void btnServices_Click(object sender, EventArgs e)
        {
            // Ghi nốt dịch vụ phát sinh trước khi trả (minibar...), đóng lại thì tính lại bảng.
            FormManager.ShowDialog(new frmServiceUsage(_bookingRoomId), this);
            Preview();
        }

        private void btnExtend_Click(object sender, EventArgs e)
        {
            if (FormManager.ShowDialog(new dlgExtendStay(_bookingRoomId), this)) Preview();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (!_checkOutAt.HasValue || _mustExtend) return;
            if (_stale)
            {
                UiHelper.Warn("Giảm giá đã đổi. Hãy bấm \"Tính lại\" để xem số tiền mới.", this);
                return;
            }
            // Gần hết 10 phút thì tính lại trước, để procedure không từ chối.
            if (Remaining() < TimeSpan.FromSeconds(30))
            {
                if (Preview()) UiHelper.Info("Bảng tính vừa được lập lại theo giờ hiện tại. Hãy kiểm tra rồi xác nhận lại.", this);
                return;
            }
            if (!ucSettlement.ValidateInput(this)) return;

            var question = new StringBuilder("Trả phòng " + _roomNumber + " lúc " + Formatters.DateTimeText(_checkOutAt.Value) +
                                             "?\nTổng hóa đơn: " + Formatters.Money(_total));
            if (_balance > 0) question.Append("\nThu thêm: " + Formatters.Money(_balance));
            else if (_balance < 0) question.Append("\nHoàn cho khách: " + Formatters.Money(-_balance));
            if (!UiHelper.Confirm(question.ToString(), owner: this)) return;

            int invoiceId;
            try
            {
                using (UiHelper.Busy(this))
                {
                    var ps = new List<SqlParameter>
                    {
                        DbHelper.P("@BookingRoomId", _bookingRoomId),
                        DbHelper.P("@CheckOutAt", SqlDbType.DateTime2, _checkOutAt.Value)
                    };
                    ps.AddRange(ucSettlement.GetParameters());
                    ps.Add(Session.EmployeeParam());
                    SqlParameter outId = DbHelper.Out("@InvoiceId", SqlDbType.Int);
                    ps.Add(outId);

                    DbHelper.Exec("dbo.sp_CheckOutRoom", ps.ToArray());
                    invoiceId = DbHelper.OutValue<int>(outId);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
                // 50007: bảng tính quá 10 phút hoặc đã sau 18:00 -> lập lại bảng tính cho đúng tình hình.
                if (ErrorHandler.GetSqlNumber(ex) == SqlErrorCodes.DateTimeRule) Preview();
                return;
            }

            timerValidity.Stop();
            foreach (string area in new[] { DataArea.Stays, DataArea.Rooms, DataArea.Invoices, DataArea.Payments,
                                            DataArea.Housekeeping, DataArea.Bookings, DataArea.Services })
                FormManager.NotifyDataChanged(area);

            UiHelper.Info("Đã trả phòng " + _roomNumber + ". Phòng chuyển sang \"Đang dọn\" và đã có việc dọn trong hàng đợi.", this);
            FormManager.ShowDialog(new frmInvoicePrint(invoiceId), this);
            DialogResult = DialogResult.OK;
        }
    }
}
