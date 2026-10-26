using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HotelManagement.Core;
using Hotel_management_system.Forms.Stay;

namespace Hotel_management_system.Forms.Billing
{
    /// <summary>
    /// Danh sách hóa đơn (Admin, Manager, Receptionist xem/in; void và lập lại chỉ Admin, Manager).
    /// Đặc tả: KE_HOACH mục 5; nghiệp vụ mục 12.
    /// - Lưới trên: vw_InvoiceDetail lọc theo ngày lập, trạng thái, số HĐ / phòng / tên khách.
    ///   Cột "Thay cho" là số hóa đơn cũ (ReplacesInvoiceId). Lưới dưới: vw_InvoiceLines.
    /// - Void: bắt buộc lý do, hỏi xác nhận mặc định "Không" -> dbo.sp_VoidInvoice (không đụng tiền đã thu).
    /// - Lập lại: chỉ khi phòng đã trả và hóa đơn MỚI NHẤT của phòng là Void -> dlgReissueInvoice.
    /// Hóa đơn chỉ có 2 trạng thái Paid / Void, mỗi phòng tối đa 1 hóa đơn Paid.
    /// </summary>
    public partial class frmInvoices : Form
    {
        private const string SqlList = @"
SELECT d.InvoiceId, d.InvoiceNo, d.Status, d.IssuedAt, d.IssuedByName, d.BookingRoomId, d.BookingId,
       d.CustomerName, d.GuestName, d.RoomNumber, d.Nights, d.RoomCharge, d.ServiceCharge, d.Discount,
       d.TaxAmount, d.TotalAmount, d.StayNetPaid, d.BuyerCompanyName, d.BuyerTaxCode,
       d.VoidedAt, d.VoidedByName, d.VoidReason, d.ReplacesInvoiceId, o.InvoiceNo AS ReplacesInvoiceNo,
       br.Status AS StayStatus,
       CAST(CASE WHEN d.InvoiceId = (SELECT MAX(i2.InvoiceId) FROM dbo.Invoices i2 WHERE i2.BookingRoomId = d.BookingRoomId)
                 THEN 1 ELSE 0 END AS BIT) AS IsLatest
FROM dbo.vw_InvoiceDetail d
JOIN dbo.BookingRooms br ON br.BookingRoomId = d.BookingRoomId
LEFT JOIN dbo.Invoices o ON o.InvoiceId = d.ReplacesInvoiceId
WHERE d.IssuedAt >= @From AND d.IssuedAt < @ToNext
  AND (@Status IS NULL OR d.Status = @Status)
  AND (@Kw IS NULL OR d.InvoiceNo LIKE @Kw OR d.RoomNumber LIKE @Kw
       OR d.CustomerName COLLATE Vietnamese_CI_AI LIKE @Kw OR d.GuestName COLLATE Vietnamese_CI_AI LIKE @Kw)
ORDER BY d.IssuedAt DESC, d.InvoiceId DESC;";

        private const string SqlLines = @"
SELECT LineNumber, Description, Quantity, Unit, UnitPrice, Amount, UsedAt
FROM dbo.vw_InvoiceLines WHERE InvoiceId = @Id ORDER BY LineNumber;";

        private static readonly Color VoidColor = Color.FromArgb(255, 205, 210);

        private bool _loading;
        private bool _canManage;
        private bool _reloadPending;

        public frmInvoices()
        {
            InitializeComponent();
            UiHelper.StyleGrid(dgvInvoices);
            UiHelper.StyleGrid(dgvLines);
            UiHelper.TranslateColumn(dgvInvoices, "Status", InvoiceStatus.ToVietnamese);
            UiHelper.ColorRows(dgvInvoices, "Status", s => s == InvoiceStatus.Void ? VoidColor : Color.Empty);
        }

        private void frmInvoices_Load(object sender, EventArgs e)
        {
            if (!UiHelper.RequireRole(this, Roles.Admin, Roles.Manager, Roles.Receptionist)) return;
            _canManage = Session.HasRole(Roles.Admin, Roles.Manager);
            btnVoid.Visible = btnServices.Visible = btnReissue.Visible = lblHint.Visible = _canManage;

            _loading = true;
            try
            {
                Lookups.LoadInvoiceStatuses(cboStatus, Lookups.AllText);
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
            dtpFrom.Value = DateTime.Today.AddDays(-30);
            dtpTo.Value = DateTime.Today;
            _loading = false;

            LoadData();
            FormManager.Subscribe(this, OnDataChanged);
        }

        // ==================================================================
        // Nạp dữ liệu
        // ==================================================================

        private void LoadData(int? selectId = null)
        {
            if (dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                lblSummary.Text = "Khoảng ngày không hợp lệ: \"từ ngày\" phải trước hoặc bằng \"đến ngày\".";
                return;
            }
            try
            {
                // Lọc [từ 00:00 ngày đầu, 00:00 ngày SAU ngày cuối) để không mất hóa đơn của ngày cuối.
                DataTable dt = DbHelper.QueryText(SqlList,
                    DbHelper.P("@From", dtpFrom.Value.Date),
                    DbHelper.P("@ToNext", dtpTo.Value.Date.AddDays(1)),
                    DbHelper.P("@Status", Lookups.SelectedString(cboStatus)),
                    DbHelper.P("@Kw", DbHelper.LikeContains(txtSearch.Text)));
                UiHelper.Bind(dgvInvoices, dt, "InvoiceId");
                UiHelper.ShowColumns(dgvInvoices,
                    "InvoiceNo", "Số HĐ",
                    "Status", "Trạng thái",
                    "IssuedAt", "Ngày lập",
                    "RoomNumber", "Phòng",
                    "GuestName", "Khách",
                    "BookingId", "Phiếu",
                    "RoomCharge", "Tiền phòng",
                    "ServiceCharge", "Dịch vụ",
                    "Discount", "Giảm giá",
                    "TaxAmount", "VAT",
                    "TotalAmount", "Tổng",
                    "ReplacesInvoiceNo", "Thay cho",
                    "BuyerCompanyName", "Đơn vị mua",
                    "IssuedByName", "Người lập",
                    "VoidReason", "Lý do void");
                if (selectId.HasValue) UiHelper.SelectRowById(dgvInvoices, "InvoiceId", selectId.Value);

                int paidCount = 0;
                decimal paidTotal = 0;
                foreach (DataRow r in dt.Rows)
                {
                    if (DbHelper.Value<string>(r, "Status") != InvoiceStatus.Paid) continue;
                    paidCount++;
                    paidTotal += DbHelper.Value<decimal>(r, "TotalAmount");
                }
                lblSummary.Text = dt.Rows.Count + " hóa đơn   ·   Hiệu lực: " + paidCount + " hóa đơn, tổng " +
                                  Formatters.Money(paidTotal) + "   ·   Đã hủy: " + (dt.Rows.Count - paidCount);
                LoadLines();
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        private void LoadLines()
        {
            DataRow r = UiHelper.SelectedRow(dgvInvoices);
            if (r == null)
            {
                dgvLines.DataSource = null;
                lblLines.Text = "Các dòng của hóa đơn";
                UpdateButtons();
                return;
            }
            try
            {
                DataTable dt = DbHelper.QueryText(SqlLines, DbHelper.P("@Id", DbHelper.Value<int>(r, "InvoiceId")));
                UiHelper.Bind(dgvLines, dt);
                UiHelper.ShowColumns(dgvLines,
                    "LineNumber", "STT",
                    "Description", "Nội dung",
                    "Quantity", "SL",
                    "Unit", "ĐVT",
                    "UnitPrice", "Đơn giá",
                    "Amount", "Thành tiền",
                    "UsedAt", "Thời điểm dùng");
                lblLines.Text = "Các dòng của hóa đơn " + DbHelper.Value<string>(r, "InvoiceNo") +
                                (DbHelper.Value<string>(r, "Status") == InvoiceStatus.Void
                                    ? " (đã hủy lúc " + Formatters.DateTimeText(r["VoidedAt"]) + " bởi " +
                                      DbHelper.Value<string>(r, "VoidedByName", "") + ")"
                                    : "");
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
            UpdateButtons();
        }

        /// <summary>Hóa đơn void mới nhất của phòng đã trả: được sửa dịch vụ và lập lại.</summary>
        private bool CanReissue(DataRow r)
        {
            return r != null
                   && DbHelper.Value<string>(r, "Status") == InvoiceStatus.Void
                   && DbHelper.Value<bool>(r, "IsLatest")
                   && DbHelper.Value<string>(r, "StayStatus") == StayStatus.CheckedOut;
        }

        private void UpdateButtons()
        {
            DataRow r = UiHelper.SelectedRow(dgvInvoices);
            btnView.Enabled = r != null;
            btnVoid.Enabled = _canManage && r != null && DbHelper.Value<string>(r, "Status") == InvoiceStatus.Paid;
            btnServices.Enabled = _canManage && CanReissue(r);
            btnReissue.Enabled = _canManage && CanReissue(r);
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
            LoadData();
        }

        private void dgvInvoices_SelectionChanged(object sender, EventArgs e)
        {
            LoadLines();
        }

        private void dgvInvoices_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) btnView_Click(sender, e);
        }

        private void OnDataChanged(string area)
        {
            if (area != DataArea.Invoices && area != DataArea.Services) return;
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

        private void btnView_Click(object sender, EventArgs e)
        {
            int? id = UiHelper.SelectedId(dgvInvoices, "InvoiceId");
            if (id == null) return;
            FormManager.ShowDialog(new frmInvoicePrint(id.Value), this);
        }

        private void btnVoid_Click(object sender, EventArgs e)
        {
            DataRow r = UiHelper.SelectedRow(dgvInvoices);
            if (r == null || DbHelper.Value<string>(r, "Status") != InvoiceStatus.Paid) return;
            int id = DbHelper.Value<int>(r, "InvoiceId");
            string no = DbHelper.Value<string>(r, "InvoiceNo");

            string reason;
            if (!UiHelper.InputText(this, "Void hóa đơn " + no, "Lý do void (bắt buộc):", out reason)) return;
            if (!UiHelper.Confirm("Void hóa đơn " + no + " (" + Formatters.Money(DbHelper.Value<decimal>(r, "TotalAmount")) +
                                  ")?\nTiền đã thu KHÔNG bị hoàn tự động. Sau khi void có thể sửa dịch vụ rồi lập lại hóa đơn.",
                                  defaultNo: true, owner: this)) return;

            try
            {
                DbHelper.Exec("dbo.sp_VoidInvoice",
                    DbHelper.P("@InvoiceId", id),
                    DbHelper.P("@Reason", reason),
                    Session.EmployeeParam());
            }
            catch (Exception ex)
            {
                // Lễ tân bấm (nếu lọt) -> 50001 "Chỉ Admin/Manager được void hóa đơn."
                ErrorHandler.Show(ex, this);
                return;
            }

            FormManager.NotifyDataChanged(DataArea.Invoices);
            LoadData(id);
            UiHelper.Info("Đã void hóa đơn " + no + ". Bước tiếp: \"Sửa dịch vụ phòng\" (nếu cần) rồi \"Lập lại hóa đơn\".", this);
        }

        private void btnServices_Click(object sender, EventArgs e)
        {
            DataRow r = UiHelper.SelectedRow(dgvInvoices);
            if (!CanReissue(r)) return;
            FormManager.ShowDialog(new frmServiceUsage(DbHelper.Value<int>(r, "BookingRoomId")), this);
            LoadData(DbHelper.Value<int>(r, "InvoiceId"));
        }

        private void btnReissue_Click(object sender, EventArgs e)
        {
            DataRow r = UiHelper.SelectedRow(dgvInvoices);
            if (!CanReissue(r)) return;
            if (FormManager.ShowDialog(new dlgReissueInvoice(DbHelper.Value<int>(r, "BookingRoomId"),
                                                             DbHelper.Value<int>(r, "InvoiceId")), this))
            {
                // Hóa đơn mới lập hôm nay: mở rộng khoảng lọc tới hôm nay để thấy ngay.
                if (dtpTo.Value.Date < DateTime.Today)
                {
                    _loading = true;
                    dtpTo.Value = DateTime.Today;
                    _loading = false;
                }
                LoadData();
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            UiHelper.ExportCsv(dgvInvoices, "HoaDon_" + DateTime.Today.ToString("yyyyMMdd") + ".csv", this);
        }
    }
}
