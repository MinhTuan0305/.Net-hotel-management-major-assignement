using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using HotelManagement.Core;

namespace Hotel_management_system.Forms.Stay
{
    /// <summary>
    /// Dịch vụ đã dùng của MỘT phòng (Admin, Manager, Receptionist). Đặc tả: KE_HOACH mục 4; nghiệp vụ mục 10.
    /// - Thêm: dbo.sp_AddServiceUsage (đơn giá lấy theo giá hiện tại của dịch vụ, DB tự ghi).
    /// - Sửa số lượng / thời điểm / ghi chú: dbo.sp_UpdateServiceUsage. Xóa: dbo.sp_DeleteServiceUsage.
    /// - Dịch vụ hệ thống (phụ thu trả trễ) do check-out ghi, không sửa/xóa ở đây.
    /// - Phòng đã có hóa đơn hiệu lực thì chỉ xem. Sau khi void hóa đơn, chỉ Admin/Manager sửa được
    ///   (dbo.usp_CheckServiceEditable chặn trong DB; form chỉ ẩn nút cho dễ hiểu).
    /// </summary>
    public partial class frmServiceUsage : Form
    {
        private const string SqlStay = @"
SELECT s.BookingRoomId, s.BookingId, s.RoomNumber, s.TypeName, s.GuestName, s.CheckInDate, s.CheckOutDate,
       s.ActualCheckIn, s.ActualCheckOut, s.StayStatus, f.HasActiveInvoice
FROM dbo.vw_RoomStays s
JOIN dbo.vw_RoomStayFolio f ON f.BookingRoomId = s.BookingRoomId
WHERE s.BookingRoomId = @Id;";

        private const string SqlUsages = @"
SELECT su.UsageId, su.ServiceId, su.UsedAt, s.ServiceName, s.Category, s.IsSystem, su.Quantity, s.Unit,
       su.UnitPrice, su.Amount, su.Note, e.FullName AS CreatedByName
FROM dbo.ServiceUsages su
JOIN dbo.Services s  ON s.ServiceId = su.ServiceId
JOIN dbo.Employees e ON e.EmployeeId = su.CreatedBy
WHERE su.BookingRoomId = @Id
ORDER BY su.UsedAt, su.UsageId;";

        private readonly int _bookingRoomId;
        private bool _editable;
        private int? _editingUsageId;
        private DateTime? _checkOutAt;   // phòng đã trả: thời điểm mặc định khi sửa sau void

        // Constructor đã chốt (CLAUDE.md mục 7) – không đổi tham số.
        public frmServiceUsage(int bookingRoomId)
        {
            InitializeComponent();
            _bookingRoomId = bookingRoomId;
            UiHelper.StyleGrid(dgvUsages);
            UiHelper.ColorRows(dgvUsages, "IsSystem", v => v == "True" ? Color.FromArgb(255, 235, 156) : Color.Empty);
        }

        private void frmServiceUsage_Load(object sender, EventArgs e)
        {
            if (!UiHelper.RequireRole(this, Roles.Admin, Roles.Manager, Roles.Receptionist)) return;
            try
            {
                Lookups.LoadServices(cboService, true, false, Lookups.ChooseText);
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
            LoadAll();
        }

        // ==================================================================
        // Nạp dữ liệu
        // ==================================================================

        private void LoadAll()
        {
            try
            {
                DataTable stay = DbHelper.QueryText(SqlStay, DbHelper.P("@Id", _bookingRoomId));
                if (stay.Rows.Count == 0)
                {
                    UiHelper.Warn("Không tìm thấy lượt lưu trú.", this);
                    BeginInvoke(new Action(Close));
                    return;
                }
                ShowStay(stay.Rows[0]);
                LoadUsages();
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        private void ShowStay(DataRow r)
        {
            string room = DbHelper.Value<string>(r, "RoomNumber");
            string status = DbHelper.Value<string>(r, "StayStatus");
            bool hasInvoice = DbHelper.Value<bool>(r, "HasActiveInvoice");
            _checkOutAt = DbHelper.Value<DateTime?>(r, "ActualCheckOut");

            Text = "Dịch vụ phòng " + room;
            lblRoom.Text = "Phòng " + room + " – " + DbHelper.Value<string>(r, "TypeName") + " – " +
                           DbHelper.Value<string>(r, "GuestName") + "   (phiếu #" + DbHelper.Value<int>(r, "BookingId") + ")";
            lblStay.Text = "Nhận lúc " + Formatters.DateTimeText(r["ActualCheckIn"]) +
                           (_checkOutAt.HasValue
                               ? "   ·   Đã trả lúc " + Formatters.DateTimeText(_checkOutAt.Value)
                               : "   ·   Dự kiến trả " + Formatters.Date(r["CheckOutDate"])) +
                           "   ·   " + StayStatus.ToVietnamese(status);

            // Ai được sửa dịch vụ (cùng quy tắc với dbo.usp_CheckServiceEditable):
            //   đang ở -> A/M/R;  đã trả + hóa đơn đã void -> chỉ A/M;  có hóa đơn hiệu lực -> không ai.
            if (status == StayStatus.CheckedIn)
            {
                _editable = true;
                lblLock.Text = "";
            }
            else if (status == StayStatus.CheckedOut && !hasInvoice)
            {
                _editable = Session.HasRole(Roles.Admin, Roles.Manager);
                lblLock.Text = _editable
                    ? "Hóa đơn của phòng đã void: đang sửa dịch vụ sau trả phòng. Sửa xong hãy lập lại hóa đơn ở màn Hóa đơn."
                    : "Phòng đã trả và hóa đơn đã void: chỉ Admin/Manager được sửa dịch vụ.";
            }
            else
            {
                _editable = false;
                lblLock.Text = hasInvoice
                    ? "Phòng đã có hóa đơn nên dịch vụ bị khóa (chỉ xem). Muốn sửa phải void hóa đơn trước."
                    : "Chỉ ghi dịch vụ cho phòng đang ở (chỉ xem).";
            }
            grpInput.Enabled = _editable;
            if (!_editingUsageId.HasValue) ResetInput();
        }

        private void LoadUsages()
        {
            DataTable dt = DbHelper.QueryText(SqlUsages, DbHelper.P("@Id", _bookingRoomId));
            UiHelper.Bind(dgvUsages, dt, "UsageId");
            UiHelper.ShowColumns(dgvUsages,
                "UsedAt", "Thời điểm",
                "ServiceName", "Dịch vụ",
                "Quantity", "SL",
                "Unit", "Đơn vị",
                "UnitPrice", "Đơn giá",
                "Amount", "Thành tiền",
                "Note", "Ghi chú",
                "CreatedByName", "Người ghi");

            decimal total = 0;
            foreach (DataRow r in dt.Rows) total += DbHelper.Value<decimal>(r, "Amount");
            lblTotal.Text = dt.Rows.Count + " dòng   ·   Tổng tiền dịch vụ (chưa VAT): " + Formatters.Money(total);
            UpdateButtons();
        }

        private void ResetInput()
        {
            _editingUsageId = null;
            cboService.Enabled = true;
            if (cboService.Items.Count > 0) cboService.SelectedIndex = 0;
            nudQuantity.Value = 1;
            dtpUsedAt.Value = _checkOutAt ?? DateTime.Now;
            txtNote.Text = "";
            grpInput.Text = "Ghi dịch vụ";
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            bool editing = _editingUsageId.HasValue;
            DataRow r = UiHelper.SelectedRow(dgvUsages);
            bool rowEditable = _editable && r != null && !DbHelper.Value<bool>(r, "IsSystem");

            btnAdd.Enabled = _editable && !editing;
            btnEdit.Visible = btnDelete.Visible = !editing;
            btnSaveEdit.Visible = btnCancelEdit.Visible = editing;
            btnEdit.Enabled = rowEditable;
            btnDelete.Enabled = rowEditable;
            dgvUsages.Enabled = !editing;
        }

        private void dgvUsages_SelectionChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void dgvUsages_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && btnEdit.Enabled) btnEdit_Click(sender, e);
        }

        // ==================================================================
        // Thêm / sửa / xóa
        // ==================================================================

        /// <summary>Thời điểm dùng làm tròn tới phút (cột UsedAt là DATETIME2(0)).</summary>
        private DateTime UsedAt()
        {
            DateTime v = dtpUsedAt.Value;
            return new DateTime(v.Year, v.Month, v.Day, v.Hour, v.Minute, 0);
        }

        /// <summary>Số lượng lớn bất thường thì hỏi lại (tránh gõ nhầm 100 thay vì 10).</summary>
        private bool ConfirmQuantity()
        {
            if (nudQuantity.Value <= BusinessRules.ServiceQuantityWarning) return true;
            return UiHelper.Confirm("Số lượng " + nudQuantity.Value + " khá lớn. Bạn chắc chắn?", defaultNo: true, owner: this);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!UiHelper.RequireSelection(cboService, "dịch vụ")) return;
            if (!ConfirmQuantity()) return;

            try
            {
                SqlParameter usageId = DbHelper.Out("@UsageId", SqlDbType.Int);
                DbHelper.Exec("dbo.sp_AddServiceUsage",
                    DbHelper.P("@BookingRoomId", _bookingRoomId),
                    DbHelper.P("@ServiceId", Lookups.SelectedInt(cboService)),
                    DbHelper.P("@Quantity", (int)nudQuantity.Value),
                    DbHelper.P("@UsedAt", SqlDbType.DateTime2, UsedAt()),
                    DbHelper.P("@Note", txtNote.Text),
                    Session.EmployeeParam(),
                    usageId);

                NotifyChanged();
                ResetInput();
                LoadUsages();
                UiHelper.SelectRowById(dgvUsages, "UsageId", DbHelper.OutValue<int>(usageId));
            }
            catch (Exception ex)
            {
                // Thời điểm ngoài thời gian lưu trú (50007), phòng đã khóa (50003)...
                ErrorHandler.Show(ex, this);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            DataRow r = UiHelper.SelectedRow(dgvUsages);
            if (r == null || DbHelper.Value<bool>(r, "IsSystem")) return;

            _editingUsageId = DbHelper.Value<int>(r, "UsageId");
            // sp_UpdateServiceUsage không đổi được loại dịch vụ: muốn đổi thì xóa dòng và thêm lại.
            Lookups.SelectValue(cboService, r["ServiceId"]);
            cboService.Enabled = false;
            nudQuantity.Value = Math.Min(nudQuantity.Maximum, DbHelper.Value<int>(r, "Quantity"));
            dtpUsedAt.Value = DbHelper.Value<DateTime>(r, "UsedAt");
            txtNote.Text = DbHelper.Value<string>(r, "Note", "");
            grpInput.Text = "Sửa dòng dịch vụ: " + DbHelper.Value<string>(r, "ServiceName");
            UpdateButtons();
            nudQuantity.Focus();
        }

        private void btnSaveEdit_Click(object sender, EventArgs e)
        {
            if (!_editingUsageId.HasValue) return;
            if (!ConfirmQuantity()) return;
            int id = _editingUsageId.Value;

            try
            {
                DbHelper.Exec("dbo.sp_UpdateServiceUsage",
                    DbHelper.P("@UsageId", id),
                    DbHelper.P("@Quantity", (int)nudQuantity.Value),
                    DbHelper.P("@UsedAt", SqlDbType.DateTime2, UsedAt()),
                    DbHelper.P("@Note", txtNote.Text),
                    Session.EmployeeParam());

                NotifyChanged();
                ResetInput();
                LoadUsages();
                UiHelper.SelectRowById(dgvUsages, "UsageId", id);
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        private void btnCancelEdit_Click(object sender, EventArgs e)
        {
            ResetInput();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataRow r = UiHelper.SelectedRow(dgvUsages);
            if (r == null || DbHelper.Value<bool>(r, "IsSystem")) return;
            if (!UiHelper.Confirm("Xóa dòng \"" + DbHelper.Value<string>(r, "ServiceName") + " × " +
                                  DbHelper.Value<int>(r, "Quantity") + "\"?", defaultNo: true, owner: this)) return;

            try
            {
                DbHelper.Exec("dbo.sp_DeleteServiceUsage",
                    DbHelper.P("@UsageId", DbHelper.Value<int>(r, "UsageId")),
                    Session.EmployeeParam());
                NotifyChanged();
                LoadUsages();
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        /// <summary>Dịch vụ đổi làm đổi số tiền tạm tính ở màn lễ tân / check-out.</summary>
        private static void NotifyChanged()
        {
            FormManager.NotifyDataChanged(DataArea.Services);
            FormManager.NotifyDataChanged(DataArea.Stays);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAll();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
