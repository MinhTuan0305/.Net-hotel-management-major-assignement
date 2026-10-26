using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HotelManagement.Core;

namespace Hotel_management_system.Forms.Billing
{
    /// <summary>
    /// Sổ thu (Admin, Manager). Đặc tả: KE_HOACH mục 5; nghiệp vụ mục 13, 15.
    /// - Lưới: vw_PaymentLedger lọc theo khoảng ngày thu (PaidAt), loại, phương thức, người thu.
    /// - Dòng tổng: tổng thu (cọc + thanh toán), tổng hoàn, THỰC THU = Σ SignedAmount; tổng theo phương thức.
    ///   Thực thu tính theo ngày nhận tiền (gồm cả cọc), khác doanh số theo hóa đơn.
    /// - "Ghi hoàn thủ công" (bút toán ngược ngoài chính sách) -> dlgPayment(Refund) cho phòng của dòng đang chọn.
    /// </summary>
    public partial class frmPaymentLedger : Form
    {
        private const string SqlList = @"
SELECT PaymentId, PaidAt, Type, Method, Amount, SignedAmount, ReferenceNo, Note,
       BookingRoomId, BookingId, RoomNumber, CustomerName, InvoiceNo, EmployeeId, EmployeeName
FROM dbo.vw_PaymentLedger
WHERE PaidAt >= @From AND PaidAt < @ToNext
  AND (@Type IS NULL OR Type = @Type)
  AND (@Method IS NULL OR Method = @Method)
  AND (@EmployeeId IS NULL OR EmployeeId = @EmployeeId)
ORDER BY PaidAt DESC, PaymentId DESC;";

        private static readonly Color RefundColor = Color.FromArgb(255, 205, 210);
        private static readonly Color DepositColor = Color.FromArgb(225, 239, 255);

        private bool _loading;
        private bool _reloadPending;

        public frmPaymentLedger()
        {
            InitializeComponent();
            UiHelper.StyleGrid(dgvLedger);
            UiHelper.TranslateColumn(dgvLedger, "Type", PaymentType.ToVietnamese);
            UiHelper.TranslateColumn(dgvLedger, "Method", PayMethod.ToVietnamese);
            UiHelper.ColorRows(dgvLedger, "Type", t =>
                t == PaymentType.Refund ? RefundColor : t == PaymentType.Deposit ? DepositColor : Color.Empty);
        }

        private void frmPaymentLedger_Load(object sender, EventArgs e)
        {
            if (!UiHelper.RequireRole(this, Roles.Admin, Roles.Manager)) return;

            _loading = true;
            try
            {
                Lookups.LoadPaymentTypes(cboType, Lookups.AllText);
                Lookups.LoadPaymentMethods(cboMethod, Lookups.AllText);
                Lookups.LoadEmployees(cboEmployee, null, false, Lookups.AllText);
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
            dtpFrom.Value = DateTime.Today;
            dtpTo.Value = DateTime.Today;
            _loading = false;

            LoadData();
            FormManager.Subscribe(this, OnDataChanged);
        }

        private void LoadData()
        {
            if (dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                lblSummary.Text = "Khoảng ngày không hợp lệ: \"từ ngày\" phải trước hoặc bằng \"đến ngày\".";
                lblByMethod.Text = "";
                return;
            }
            try
            {
                // [từ 00:00 ngày đầu, 00:00 ngày sau ngày cuối) – không mất các khoản của ngày cuối.
                DataTable dt = DbHelper.QueryText(SqlList,
                    DbHelper.P("@From", dtpFrom.Value.Date),
                    DbHelper.P("@ToNext", dtpTo.Value.Date.AddDays(1)),
                    DbHelper.P("@Type", Lookups.SelectedString(cboType)),
                    DbHelper.P("@Method", Lookups.SelectedString(cboMethod)),
                    DbHelper.P("@EmployeeId", Lookups.SelectedInt(cboEmployee)));
                UiHelper.Bind(dgvLedger, dt, "PaymentId");
                UiHelper.ShowColumns(dgvLedger,
                    "PaidAt", "Thời điểm",
                    "Type", "Loại",
                    "Method", "Phương thức",
                    "Amount", "Số tiền",
                    "RoomNumber", "Phòng",
                    "BookingId", "Phiếu",
                    "CustomerName", "Khách",
                    "InvoiceNo", "Hóa đơn",
                    "ReferenceNo", "Mã GD",
                    "EmployeeName", "Người thu",
                    "Note", "Ghi chú");
                ShowSummary(dt);
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
            UpdateButtons();
        }

        private void ShowSummary(DataTable dt)
        {
            decimal received = 0, refunded = 0;
            var byMethod = new Dictionary<string, decimal>();
            foreach (string m in PayMethod.All) byMethod[m] = 0;

            foreach (DataRow r in dt.Rows)
            {
                decimal amount = DbHelper.Value<decimal>(r, "Amount");
                if (DbHelper.Value<string>(r, "Type") == PaymentType.Refund) refunded += amount;
                else received += amount;

                string method = DbHelper.Value<string>(r, "Method");
                if (byMethod.ContainsKey(method)) byMethod[method] += DbHelper.Value<decimal>(r, "SignedAmount");
            }

            lblSummary.Text = dt.Rows.Count + " khoản   ·   Tổng thu (cọc + thanh toán): " + Formatters.Money(received) +
                              "   ·   Tổng hoàn: " + Formatters.Money(refunded) +
                              "   ·   THỰC THU: " + Formatters.Money(received - refunded);
            var parts = new List<string>();
            foreach (KeyValuePair<string, decimal> kv in byMethod)
                parts.Add(PayMethod.ToVietnamese(kv.Key) + ": " + Formatters.Money(kv.Value));
            lblByMethod.Text = "Thực thu theo phương thức – " + string.Join("   ·   ", parts);
        }

        private void UpdateButtons()
        {
            btnRefund.Enabled = UiHelper.SelectedRow(dgvLedger) != null;
        }

        private void filter_Changed(object sender, EventArgs e)
        {
            if (!_loading) LoadData();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgvLedger_SelectionChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void OnDataChanged(string area)
        {
            if (area != DataArea.Payments) return;
            if (_reloadPending) return;
            _reloadPending = true;
            BeginInvoke(new Action(() =>
            {
                _reloadPending = false;
                if (!IsDisposed) LoadData();
            }));
        }

        private void btnRefund_Click(object sender, EventArgs e)
        {
            int? stayId = UiHelper.SelectedId(dgvLedger, "BookingRoomId");
            if (stayId == null)
            {
                UiHelper.Warn("Hãy chọn một khoản thu của phòng cần hoàn.", this);
                return;
            }
            if (FormManager.ShowDialog(new dlgPayment(PaymentMode.Refund, stayId.Value), this)) LoadData();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            UiHelper.ExportCsv(dgvLedger, "SoThu_" + dtpFrom.Value.ToString("yyyyMMdd") + "_" +
                                          dtpTo.Value.ToString("yyyyMMdd") + ".csv", this);
        }
    }
}
