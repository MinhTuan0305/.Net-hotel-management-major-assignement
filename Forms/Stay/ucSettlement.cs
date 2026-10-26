using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using HotelManagement.Core;

namespace Hotel_management_system.Forms.Stay
{
    /// <summary>
    /// Phần nhập khi chốt hóa đơn, dùng chung cho frmCheckOut (sp_CheckOutRoom) và lập lại hóa đơn
    /// (dlgReissueInvoice -> sp_ReissueInvoice). Đặc tả: KE_HOACH mục 4–5; nghiệp vụ mục 11–13.
    ///   1. Giảm giá + lý do (có giảm phải có lý do; lễ tân tối đa 10% – DB chặn 50001).
    ///   2. Người mua trên hóa đơn (có mã số thuế thì phải có tên đơn vị).
    ///   3. Thu hoặc hoàn: còn phải thu -> số thu = đúng số còn thiếu + phương thức;
    ///      khách trả dư -> chọn cách hoàn. Chuyển khoản/ví điện tử phải có mã giao dịch.
    /// Form cha gọi SetQuote(...) mỗi khi có bảng tính mới, ValidateInput() trước khi lưu,
    /// rồi ghép GetParameters() vào lời gọi procedure (các tham số trùng tên ở cả 2 procedure).
    /// </summary>
    public partial class ucSettlement : UserControl
    {
        private static readonly Color DueColor = Color.FromArgb(192, 0, 0);
        private static readonly Color RefundColor = Color.FromArgb(0, 110, 0);

        private bool _hasQuote;
        private decimal _subTotal;
        private decimal _balance;
        private bool _methodsLoaded;

        /// <summary>Phát ra khi số giảm giá đổi: form cha biết bảng tính cần tính lại.</summary>
        public event EventHandler DiscountChanged;

        public ucSettlement()
        {
            InitializeComponent();
            UiHelper.SetupMoneyInput(nudDiscount);
            UiHelper.SetupMoneyInput(nudPayAmount);
            SetMoneyMode(0m);
            lblBalance.Text = "Chưa có bảng tính";
        }

        /// <summary>
        /// Nạp danh sách phương thức khi control đã nằm trên form (ComboBox gán DataSource
        /// cần BindingContext của form cha, chưa có trong constructor).
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode || _methodsLoaded) return;
            Lookups.LoadPaymentMethods(cboPayMethod, Lookups.ChooseText);
            Lookups.LoadPaymentMethods(cboRefundMethod, Lookups.ChooseText);
            _methodsLoaded = true;
            UpdateReferenceLabels();
        }

        // ==================================================================
        // API cho form cha
        // ==================================================================

        /// <summary>Số tiền giảm giá đang nhập (truyền vào bảng tính xem trước).</summary>
        public decimal Discount
        {
            get { return nudDiscount.Value; }
        }

        /// <summary>
        /// Cập nhật theo bảng tính mới.
        /// subTotal = tiền phòng + dịch vụ (trước giảm giá, trước VAT);
        /// balance = tổng hóa đơn − đã thu (> 0 còn phải thu, < 0 khách trả dư).
        /// </summary>
        public void SetQuote(decimal subTotal, decimal balance)
        {
            _hasQuote = true;
            _subTotal = subTotal;
            _balance = balance;
            nudDiscount.Maximum = Math.Max(0m, subTotal);   // giảm giá không vượt tổng (DB cũng chặn)
            UpdatePercent();
            SetMoneyMode(balance);
        }

        /// <summary>Điền sẵn từ hóa đơn cũ (lập lại hóa đơn sau void).</summary>
        public void Prefill(decimal discount, string discountReason, string buyerName, string buyerCompany,
                            string buyerTaxCode, string buyerAddress)
        {
            nudDiscount.Value = Math.Min(nudDiscount.Maximum, Math.Max(0m, discount));
            txtDiscountReason.Text = discountReason ?? "";
            txtBuyerName.Text = buyerName ?? "";
            txtBuyerCompany.Text = buyerCompany ?? "";
            txtBuyerTaxCode.Text = buyerTaxCode ?? "";
            txtBuyerAddress.Text = buyerAddress ?? "";
        }

        /// <summary>Kiểm tra sơ bộ trước khi lưu (procedure vẫn kiểm tra lại). Sai thì báo và trả false.</summary>
        public bool ValidateInput(IWin32Window owner)
        {
            if (!_hasQuote)
            {
                UiHelper.Warn("Chưa có bảng tính. Hãy bấm \"Tính lại\".", owner);
                return false;
            }
            if (nudDiscount.Value > 0)
            {
                if (!UiHelper.RequireText(txtDiscountReason, "lý do giảm giá")) return false;
                if (nudDiscount.Value * 100 > _subTotal * BusinessRules.ReceptionistMaxDiscountPercent &&
                    !Session.HasRole(Roles.Admin, Roles.Manager))
                {
                    UiHelper.Warn("Giảm giá trên " + Formatters.Percent(BusinessRules.ReceptionistMaxDiscountPercent) +
                                  " phải do Manager/Admin thực hiện.", owner);
                    nudDiscount.Focus();
                    return false;
                }
            }
            if (txtBuyerTaxCode.Text.Trim().Length > 0 && txtBuyerCompany.Text.Trim().Length == 0)
            {
                UiHelper.Warn("Hóa đơn có mã số thuế thì phải có tên đơn vị.", owner);
                txtBuyerCompany.Focus();
                return false;
            }
            if (_balance > 0)
            {
                if (!UiHelper.RequireSelection(cboPayMethod, "phương thức thu tiền")) return false;
                if (PayMethod.NeedsReference(Lookups.SelectedString(cboPayMethod)) &&
                    !UiHelper.RequireText(txtPayReference, "mã giao dịch")) return false;
            }
            else if (_balance < 0)
            {
                if (!UiHelper.RequireSelection(cboRefundMethod, "phương thức hoàn tiền")) return false;
                if (PayMethod.NeedsReference(Lookups.SelectedString(cboRefundMethod)) &&
                    !UiHelper.RequireText(txtRefundReference, "mã giao dịch hoàn tiền")) return false;
            }
            return true;
        }

        /// <summary>
        /// Tham số chung của sp_CheckOutRoom và sp_ReissueInvoice: giảm giá, thuế, người mua, thu, hoàn.
        /// Form cha thêm @BookingRoomId, @CheckOutAt (check-out), @EmployeeId, @InvoiceId OUTPUT.
        /// </summary>
        public SqlParameter[] GetParameters()
        {
            bool pay = _balance > 0;
            bool refund = _balance < 0;
            return new[]
            {
                DbHelper.P("@Discount", nudDiscount.Value),
                DbHelper.P("@DiscountReason", txtDiscountReason.Text),
                DbHelper.P("@TaxRate", BusinessRules.DefaultTaxRate),
                DbHelper.P("@BuyerName", txtBuyerName.Text),
                DbHelper.P("@BuyerCompanyName", txtBuyerCompany.Text),
                DbHelper.P("@BuyerTaxCode", txtBuyerTaxCode.Text),
                DbHelper.P("@BuyerAddress", txtBuyerAddress.Text),
                DbHelper.P("@PayAmount", pay ? nudPayAmount.Value : 0m),
                DbHelper.P("@PayMethod", pay ? Lookups.SelectedString(cboPayMethod) : null),
                DbHelper.P("@PayReference", pay ? txtPayReference.Text : null),
                DbHelper.P("@RefundMethod", refund ? Lookups.SelectedString(cboRefundMethod) : null),
                DbHelper.P("@RefundReference", refund ? txtRefundReference.Text : null)
            };
        }

        // ==================================================================
        // Nội bộ
        // ==================================================================

        private void SetMoneyMode(decimal balance)
        {
            bool pay = balance > 0;
            bool refund = balance < 0;

            lblPayAmount.Visible = nudPayAmount.Visible = pay;
            lblPayMethod.Visible = cboPayMethod.Visible = pay;
            lblPayReference.Visible = txtPayReference.Visible = pay;
            lblRefundMethod.Visible = cboRefundMethod.Visible = refund;
            lblRefundReference.Visible = txtRefundReference.Visible = refund;

            if (pay)
            {
                // Phải thu ĐÚNG số còn thiếu: thu ít hơn thì DB báo "chưa thu đủ", nhiều hơn thì báo "vượt".
                nudPayAmount.Minimum = 0;
                nudPayAmount.Maximum = balance;
                nudPayAmount.Value = balance;
                nudPayAmount.Minimum = balance;
                lblBalance.Text = "Còn phải thu: " + Formatters.Money(balance);
                lblBalance.ForeColor = DueColor;
            }
            else if (refund)
            {
                lblBalance.Text = "Khách đã trả dư, phải hoàn: " + Formatters.Money(-balance);
                lblBalance.ForeColor = RefundColor;
            }
            else
            {
                lblBalance.Text = "Đã thu đủ, không cần thu thêm.";
                lblBalance.ForeColor = SystemColors.ControlText;
            }
        }

        private void UpdatePercent()
        {
            lblDiscountPercent.Text = _subTotal > 0 && nudDiscount.Value > 0
                ? "≈ " + Formatters.Percent(Math.Round(nudDiscount.Value * 100 / _subTotal, 1)) + " tổng trước giảm"
                : "";
            lblDiscountReason.Text = nudDiscount.Value > 0 ? "Lý do:*" : "Lý do:";
        }

        private void nudDiscount_ValueChanged(object sender, EventArgs e)
        {
            UpdatePercent();
            EventHandler handler = DiscountChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private void method_Changed(object sender, EventArgs e)
        {
            UpdateReferenceLabels();
        }

        private void UpdateReferenceLabels()
        {
            lblPayReference.Text = PayMethod.NeedsReference(Lookups.SelectedString(cboPayMethod)) ? "Mã giao dịch:*" : "Mã giao dịch:";
            lblRefundReference.Text = PayMethod.NeedsReference(Lookups.SelectedString(cboRefundMethod)) ? "Mã giao dịch:*" : "Mã giao dịch:";
        }
    }
}
