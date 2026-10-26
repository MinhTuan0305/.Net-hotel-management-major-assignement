using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using HotelManagement.Core;

namespace Hotel_management_system.Forms.Billing
{
    /// <summary>
    /// Lập lại hóa đơn cho phòng đã trả có hóa đơn đã void (chỉ Admin, Manager).
    /// Đặc tả: KE_HOACH mục 5; nghiệp vụ mục 12. Form bổ sung, tách khỏi frmInvoices cho gọn.
    /// dbo.sp_ReissueInvoice tính lại từ dữ liệu HIỆN TẠI: tiền phòng = số đêm × giá đã chốt,
    /// dịch vụ = mọi dòng ServiceUsages (kể cả phụ thu trễ đã ghi lúc check-out, KHÔNG tính lại phụ thu),
    /// rồi thu thêm hoặc hoàn chênh lệch và gắn ReplacesInvoiceId = hóa đơn void gần nhất.
    /// Số trên form chỉ là tạm tính cùng công thức; số chính thức do procedure tính.
    /// </summary>
    public partial class dlgReissueInvoice : Form
    {
        private const string SqlStay = @"
SELECT br.BookingRoomId, br.Status, br.Nights, br.PricePerNight, r.RoomNumber,
       CAST(br.PricePerNight * br.Nights AS DECIMAL(14,0)) AS RoomCharge,
       CAST(ISNULL((SELECT SUM(su.Amount) FROM dbo.ServiceUsages su WHERE su.BookingRoomId = br.BookingRoomId), 0)
            AS DECIMAL(14,0)) AS ServiceCharge,
       dbo.fn_StayNetPaid(br.BookingRoomId) AS NetPaid
FROM dbo.BookingRooms br
JOIN dbo.Rooms r ON r.RoomId = br.RoomId
WHERE br.BookingRoomId = @Id;";

        private const string SqlOldInvoice = "SELECT * FROM dbo.vw_InvoiceDetail WHERE InvoiceId = @Id;";

        private static readonly Color DueColor = Color.FromArgb(192, 0, 0);
        private static readonly Color RefundColor = Color.FromArgb(0, 110, 0);

        private readonly int _bookingRoomId;
        private readonly int _voidInvoiceId;
        private decimal _total;
        private decimal _balance;
        private bool _quoted;
        private bool _stale;

        public dlgReissueInvoice(int bookingRoomId, int voidInvoiceId)
        {
            InitializeComponent();
            _bookingRoomId = bookingRoomId;
            _voidInvoiceId = voidInvoiceId;
            UiHelper.SetupDialog(this, btnOk, btnCancel);
        }

        private void dlgReissueInvoice_Load(object sender, EventArgs e)
        {
            if (!UiHelper.RequireRole(this, Roles.Admin, Roles.Manager)) return;
            try
            {
                DataTable old = DbHelper.QueryText(SqlOldInvoice, DbHelper.P("@Id", _voidInvoiceId));
                if (old.Rows.Count == 0 || DbHelper.Value<string>(old.Rows[0], "Status") != InvoiceStatus.Void)
                {
                    UiHelper.Warn("Chỉ lập lại được khi hóa đơn cũ đã void.", this);
                    BeginInvoke(new Action(Close));
                    return;
                }
                DataRow o = old.Rows[0];
                lblTitle.Text = "Lập lại hóa đơn – phòng " + DbHelper.Value<string>(o, "RoomNumber") +
                                " (thay cho " + DbHelper.Value<string>(o, "InvoiceNo") + ")";
                lblSubtitle.Text = "Khách: " + DbHelper.Value<string>(o, "GuestName") + "   ·   Hóa đơn cũ " +
                                   Formatters.Money(DbHelper.Value<decimal>(o, "TotalAmount")) + ", void lúc " +
                                   Formatters.DateTimeText(o["VoidedAt"]) + ": " + DbHelper.Value<string>(o, "VoidReason", "");

                // Điền sẵn giảm giá + người mua từ hóa đơn cũ, người dùng sửa nếu cần.
                ucSettlement.Prefill(DbHelper.Value<decimal>(o, "Discount"),
                                     DbHelper.Value<string>(o, "DiscountReason"),
                                     DbHelper.Value<string>(o, "BuyerName"),
                                     DbHelper.Value<string>(o, "BuyerCompanyName"),
                                     DbHelper.Value<string>(o, "BuyerTaxCode"),
                                     DbHelper.Value<string>(o, "BuyerAddress"));
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
                BeginInvoke(new Action(Close));
                return;
            }
            Quote();
        }

        /// <summary>Tạm tính hóa đơn mới với giảm giá đang nhập (cùng công thức cột Invoices.TotalAmount).</summary>
        private void Quote()
        {
            try
            {
                DataTable dt = DbHelper.QueryText(SqlStay, DbHelper.P("@Id", _bookingRoomId));
                if (dt.Rows.Count == 0) return;
                DataRow r = dt.Rows[0];
                if (DbHelper.Value<string>(r, "Status") != StayStatus.CheckedOut)
                {
                    UiHelper.Warn("Chỉ lập lại hóa đơn cho phòng đã trả.", this);
                    BeginInvoke(new Action(Close));
                    return;
                }

                decimal room = DbHelper.Value<decimal>(r, "RoomCharge");
                decimal services = DbHelper.Value<decimal>(r, "ServiceCharge");
                decimal netPaid = DbHelper.Value<decimal>(r, "NetPaid");
                decimal subTotal = room + services;
                decimal discount = Math.Min(ucSettlement.Discount, subTotal);
                decimal taxRate = BusinessRules.DefaultTaxRate;
                _total = Math.Round((subTotal - discount) * (1 + taxRate / 100m), 0, MidpointRounding.AwayFromZero);
                decimal tax = _total - (subTotal - discount);
                _balance = _total - netPaid;

                lblQuoteCaptions.Text = string.Join("\n", new[]
                {
                    "Tiền phòng (" + Formatters.Nights(DbHelper.Value<int>(r, "Nights")) + " × " +
                        Formatters.Number(DbHelper.Value<decimal>(r, "PricePerNight")) + "):",
                    "Dịch vụ (gồm phụ thu đã ghi):",
                    "Cộng trước giảm giá:",
                    "Giảm giá:",
                    "Thuế GTGT " + Formatters.Percent(taxRate) + ":",
                    "",
                    "Đã thu ròng của phòng:"
                });
                lblQuoteValues.Text = string.Join("\n", new[]
                {
                    Formatters.Money(room),
                    Formatters.Money(services),
                    Formatters.Money(subTotal),
                    discount > 0 ? "− " + Formatters.Money(discount) : Formatters.Money(0),
                    Formatters.Money(tax),
                    "",
                    Formatters.Money(netPaid)
                });
                lblTotal.Text = "TỔNG HÓA ĐƠN MỚI: " + Formatters.Money(_total);
                if (_balance > 0)
                {
                    lblBalance.Text = "Phải thu thêm: " + Formatters.Money(_balance);
                    lblBalance.ForeColor = DueColor;
                }
                else if (_balance < 0)
                {
                    lblBalance.Text = "Phải hoàn cho khách: " + Formatters.Money(-_balance);
                    lblBalance.ForeColor = RefundColor;
                }
                else
                {
                    lblBalance.Text = "Khớp số đã thu, không thu/hoàn thêm.";
                    lblBalance.ForeColor = SystemColors.ControlText;
                }

                ucSettlement.SetQuote(subTotal, _balance);
                _quoted = true;
                _stale = false;
                lblStale.Visible = false;
                btnOk.Enabled = true;
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        private void ucSettlement_DiscountChanged(object sender, EventArgs e)
        {
            if (!_quoted) return;
            _stale = true;
            lblStale.Visible = true;
            btnOk.Enabled = false;
        }

        private void btnRecalc_Click(object sender, EventArgs e)
        {
            Quote();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (_stale)
            {
                UiHelper.Warn("Giảm giá đã đổi. Hãy bấm \"Tính lại\".", this);
                return;
            }
            if (!ucSettlement.ValidateInput(this)) return;
            if (!UiHelper.Confirm("Lập hóa đơn mới " + Formatters.Money(_total) + " thay cho hóa đơn đã void?", owner: this)) return;

            int newId;
            try
            {
                using (UiHelper.Busy(this))
                {
                    var ps = new List<SqlParameter> { DbHelper.P("@BookingRoomId", _bookingRoomId) };
                    ps.AddRange(ucSettlement.GetParameters());
                    ps.Add(Session.EmployeeParam());
                    SqlParameter outId = DbHelper.Out("@InvoiceId", SqlDbType.Int);
                    ps.Add(outId);
                    DbHelper.Exec("dbo.sp_ReissueInvoice", ps.ToArray());
                    newId = DbHelper.OutValue<int>(outId);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
                Quote();   // dữ liệu có thể vừa đổi (dịch vụ, tiền thu): tính lại cho đúng
                return;
            }

            FormManager.NotifyDataChanged(DataArea.Invoices);
            FormManager.NotifyDataChanged(DataArea.Payments);
            FormManager.ShowDialog(new frmInvoicePrint(newId), this);
            DialogResult = DialogResult.OK;
        }
    }
}
