using System;
using System.Data;
using System.Windows.Forms;
using HotelManagement.Core;

namespace Hotel_management_system.Forms.Catalog
{
    /// <summary>
    /// Danh mục dịch vụ (Admin, Manager sửa; Receptionist chỉ xem). Đặc tả: KE_HOACH mục 2; nghiệp vụ mục 7A.
    /// - Tên dịch vụ duy nhất, đơn giá >= 0. Đổi giá không ảnh hưởng dịch vụ đã ghi
    ///   (ServiceUsages lưu đơn giá tại thời điểm dùng).
    /// - Không xóa: dùng "Ngừng bán" (IsActive = 0) để lịch sử vẫn đọc được.
    /// - Dòng IsSystem = 1 (Phụ thu trả phòng trễ) do hệ thống ghi khi check-out: chỉ xem.
    /// </summary>
    public partial class frmServices : Form
    {
        private const string SqlList = @"
SELECT ServiceId, ServiceName, Category, Unit, Price, IsActive, IsSystem
FROM dbo.Services
WHERE (@ShowInactive = 1 OR IsActive = 1)
  AND (@Category IS NULL OR Category = @Category)
  AND (@Kw IS NULL OR ServiceName COLLATE Vietnamese_CI_AI LIKE @Kw)
ORDER BY IsSystem, Category, ServiceName;";

        private enum EditMode { View, Add, Edit }

        private EditMode _mode = EditMode.View;
        private bool _canEdit;
        private bool _loading;

        public frmServices()
        {
            InitializeComponent();
            UiHelper.StyleGrid(dgvServices);
            UiHelper.SetupMoneyInput(nudPrice);
            UiHelper.ColorRows(dgvServices, "IsActive",
                v => v == "False" ? System.Drawing.SystemColors.ControlLight : System.Drawing.Color.Empty);
        }

        private void frmServices_Load(object sender, EventArgs e)
        {
            if (!UiHelper.RequireRole(this, Roles.Admin, Roles.Manager, Roles.Receptionist)) return;

            // Lễ tân chỉ xem danh mục (ma trận quyền I.1).
            _canEdit = Session.HasRole(Roles.Admin, Roles.Manager);
            if (!_canEdit) Text += " (chỉ xem)";

            LoadCategories();
            LoadData();
            SetMode(EditMode.View);
        }

        private void LoadCategories()
        {
            try
            {
                _loading = true;
                Lookups.LoadServiceCategories(cboCategoryFilter, Lookups.AllText);
                DataTable dt = DbHelper.QueryText(
                    "SELECT DISTINCT Category FROM dbo.Services WHERE Category IS NOT NULL ORDER BY Category");
                cboCategory.Items.Clear();
                foreach (DataRow r in dt.Rows) cboCategory.Items.Add(DbHelper.Value<string>(r, "Category"));
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
            finally
            {
                _loading = false;
            }
        }

        // ==================================================================
        // Nạp dữ liệu
        // ==================================================================

        private void LoadData(int? selectId = null)
        {
            try
            {
                DataTable dt = DbHelper.QueryText(SqlList,
                    DbHelper.P("@ShowInactive", chkShowInactive.Checked),
                    DbHelper.P("@Category", Lookups.SelectedString(cboCategoryFilter)),
                    DbHelper.P("@Kw", DbHelper.LikeContains(txtSearch.Text)));
                UiHelper.Bind(dgvServices, dt, "ServiceId");
                UiHelper.ShowColumns(dgvServices,
                    "ServiceName", "Tên dịch vụ",
                    "Category", "Nhóm",
                    "Unit", "Đơn vị",
                    "Price", "Đơn giá",
                    "IsActive", "Đang bán",
                    "IsSystem", "Hệ thống");
                if (selectId.HasValue) UiHelper.SelectRowById(dgvServices, "ServiceId", selectId.Value);
                ShowDetail();
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        private DataRow Selected()
        {
            return UiHelper.SelectedRow(dgvServices);
        }

        private bool SelectedIsSystem()
        {
            DataRow r = Selected();
            return r != null && DbHelper.Value<bool>(r, "IsSystem");
        }

        private void ShowDetail()
        {
            DataRow r = Selected();
            if (r == null)
            {
                ClearFields();
            }
            else
            {
                txtName.Text = DbHelper.Value<string>(r, "ServiceName");
                cboCategory.Text = DbHelper.Value<string>(r, "Category", "");
                txtUnit.Text = DbHelper.Value<string>(r, "Unit");
                nudPrice.Value = DbHelper.Value<decimal>(r, "Price");
                chkActive.Checked = DbHelper.Value<bool>(r, "IsActive");
            }
            lblSystemNote.Text = SelectedIsSystem()
                ? "Dịch vụ hệ thống: giá ghi tự động lúc check-out trễ, không sửa được."
                : "";
            UpdateButtons();
        }

        private void ClearFields()
        {
            txtName.Text = "";
            cboCategory.Text = "";
            txtUnit.Text = "";
            nudPrice.Value = 0;
            chkActive.Checked = true;
        }

        // ==================================================================
        // Chế độ Xem / Thêm / Sửa
        // ==================================================================

        private void SetMode(EditMode mode)
        {
            _mode = mode;
            bool editing = mode != EditMode.View;
            txtName.ReadOnly = !editing;
            txtUnit.ReadOnly = !editing;
            cboCategory.Enabled = editing;
            nudPrice.Enabled = editing;
            chkActive.Enabled = editing;
            dgvServices.Enabled = !editing;
            pnlTop.Enabled = !editing;
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            bool editing = _mode != EditMode.View;
            DataRow r = Selected();
            bool editableRow = r != null && !SelectedIsSystem();

            btnAdd.Visible = btnEdit.Visible = btnToggleActive.Visible = _canEdit;
            btnSave.Visible = btnCancel.Visible = _canEdit;

            btnAdd.Enabled = !editing;
            btnEdit.Enabled = !editing && editableRow;
            btnToggleActive.Enabled = !editing && editableRow;
            btnToggleActive.Text = r != null && !DbHelper.Value<bool>(r, "IsActive") ? "Bán lại" : "Ngừng bán";
            btnSave.Enabled = editing;
            btnCancel.Enabled = editing;
        }

        private void dgvServices_SelectionChanged(object sender, EventArgs e)
        {
            if (_mode == EditMode.View) ShowDetail();
        }

        private void filter_Changed(object sender, EventArgs e)
        {
            if (_loading || _mode != EditMode.View) return;
            LoadData();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadCategories();
            LoadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearFields();
            lblSystemNote.Text = "";
            SetMode(EditMode.Add);
            txtName.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!UiHelper.RequireSelectedRow(dgvServices, "một dịch vụ")) return;
            SetMode(EditMode.Edit);
            txtName.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetMode(EditMode.View);
            ShowDetail();
        }

        // ==================================================================
        // Lưu / Ngừng bán
        // ==================================================================

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!UiHelper.RequireText(txtName, "tên dịch vụ")) return;
            if (!UiHelper.RequireText(txtUnit, "đơn vị tính")) return;

            var ps = new[]
            {
                DbHelper.P("@Name", txtName.Text),
                DbHelper.P("@Category", cboCategory.Text),
                DbHelper.P("@Unit", txtUnit.Text),
                DbHelper.P("@Price", nudPrice.Value),
                DbHelper.P("@Active", chkActive.Checked)
            };

            try
            {
                int id;
                if (_mode == EditMode.Add)
                {
                    id = DbHelper.Scalar<int>(
                        "INSERT INTO dbo.Services (ServiceName, Category, Unit, Price, IsActive, IsSystem) " +
                        "VALUES (@Name, @Category, @Unit, @Price, @Active, 0); SELECT CAST(SCOPE_IDENTITY() AS INT);", ps);
                }
                else
                {
                    id = UiHelper.SelectedId(dgvServices, "ServiceId").Value;
                    // "AND IsSystem = 0": chặn sửa dịch vụ hệ thống kể cả khi giao diện để lọt.
                    DbHelper.ExecText(
                        "UPDATE dbo.Services SET ServiceName = @Name, Category = @Category, Unit = @Unit, " +
                        "Price = @Price, IsActive = @Active WHERE ServiceId = @Id AND IsSystem = 0",
                        ps[0], ps[1], ps[2], ps[3], ps[4], DbHelper.P("@Id", id));
                }

                FormManager.NotifyDataChanged(DataArea.Catalog);
                SetMode(EditMode.View);
                LoadCategories();
                LoadData(id);
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        private void btnToggleActive_Click(object sender, EventArgs e)
        {
            DataRow r = Selected();
            if (r == null || SelectedIsSystem()) return;
            int id = DbHelper.Value<int>(r, "ServiceId");
            bool active = DbHelper.Value<bool>(r, "IsActive");
            string name = DbHelper.Value<string>(r, "ServiceName");

            string question = active
                ? "Ngừng bán dịch vụ \"" + name + "\"? Dịch vụ sẽ không còn trong danh sách khi ghi dịch vụ cho phòng."
                : "Bán lại dịch vụ \"" + name + "\"?";
            if (!UiHelper.Confirm(question, owner: this)) return;

            try
            {
                DbHelper.ExecText("UPDATE dbo.Services SET IsActive = @Active WHERE ServiceId = @Id AND IsSystem = 0",
                    DbHelper.P("@Active", !active), DbHelper.P("@Id", id));
                FormManager.NotifyDataChanged(DataArea.Catalog);
                LoadData(id);
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }
    }
}
