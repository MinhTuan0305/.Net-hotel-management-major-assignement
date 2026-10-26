using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HotelManagement.Core;

namespace Hotel_management_system.Forms.Catalog
{
    /// <summary>
    /// Nhân viên và vai trò (chỉ Admin). Đặc tả: KE_HOACH mục 2; nghiệp vụ mục 2, 3.
    /// - Thêm mới: nhập mật khẩu ban đầu, băm bằng PasswordHasher.Hash (không lưu mật khẩu thô).
    /// - Không xóa cứng: khóa/mở khóa (IsActive). Đặt lại mật khẩu về PasswordHasher.DefaultResetPassword.
    /// - Chặn tự khóa chính mình; chặn khóa hoặc đổi vai trò của Admin cuối cùng đang hoạt động.
    /// - Tab Vai trò: 4 vai trò cố định, chỉ sửa Description.
    /// Không bao giờ hiện PasswordHash trên lưới (nghiệp vụ mục 15).
    /// </summary>
    public partial class frmEmployees : Form
    {
        private const string SqlList = @"
SELECT e.EmployeeId, e.FullName, e.Username, e.RoleId, r.RoleName, e.Phone, e.Email, e.HireDate, e.IsActive, e.CreatedAt
FROM dbo.Employees e
JOIN dbo.Roles r ON r.RoleId = e.RoleId
WHERE (@ShowLocked = 1 OR e.IsActive = 1)
  AND (@Kw IS NULL OR e.FullName COLLATE Vietnamese_CI_AI LIKE @Kw
                   OR e.Username LIKE @Kw OR e.Phone LIKE @Kw)
ORDER BY e.IsActive DESC, r.RoleId, e.FullName;";

        private const string SqlRoles = @"
SELECT r.RoleId, r.RoleName, r.Description,
       (SELECT COUNT(*) FROM dbo.Employees e WHERE e.RoleId = r.RoleId AND e.IsActive = 1) AS ActiveEmployees
FROM dbo.Roles r
ORDER BY r.RoleId;";

        // Số Admin đang hoạt động KHÁC nhân viên @Id (dùng để chặn khóa/đổi vai trò Admin cuối cùng).
        private const string SqlOtherActiveAdmins = @"
SELECT COUNT(*) FROM dbo.Employees e JOIN dbo.Roles r ON r.RoleId = e.RoleId
WHERE r.RoleName = 'Admin' AND e.IsActive = 1 AND e.EmployeeId <> @Id;";

        private enum EditMode { View, Add, Edit }

        private EditMode _mode = EditMode.View;

        public frmEmployees()
        {
            InitializeComponent();
            UiHelper.StyleGrid(dgvEmployees);
            UiHelper.StyleGrid(dgvRoles);
            UiHelper.TranslateColumn(dgvEmployees, "RoleName", Roles.ToVietnamese);
            UiHelper.TranslateColumn(dgvRoles, "RoleName", r => Roles.ToVietnamese(r) + " (" + r + ")");
            UiHelper.ColorRows(dgvEmployees, "IsActive", v => v == "False" ? SystemColors.ControlLight : Color.Empty);
        }

        private void frmEmployees_Load(object sender, EventArgs e)
        {
            if (!UiHelper.RequireRole(this, Roles.Admin)) return;
            try
            {
                Lookups.LoadRoles(cboRole);
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
            LoadData();
            LoadRoles();
            SetMode(EditMode.View);
        }

        // ==================================================================
        // Nhân viên – nạp dữ liệu
        // ==================================================================

        private void LoadData(int? selectId = null)
        {
            try
            {
                DataTable dt = DbHelper.QueryText(SqlList,
                    DbHelper.P("@ShowLocked", chkShowLocked.Checked),
                    DbHelper.P("@Kw", DbHelper.LikeContains(txtSearch.Text)));
                UiHelper.Bind(dgvEmployees, dt, "EmployeeId");
                UiHelper.ShowColumns(dgvEmployees,
                    "FullName", "Họ tên",
                    "Username", "Tài khoản",
                    "RoleName", "Vai trò",
                    "Phone", "SĐT",
                    "Email", "Email",
                    "HireDate", "Ngày vào làm",
                    "IsActive", "Hoạt động");
                if (selectId.HasValue) UiHelper.SelectRowById(dgvEmployees, "EmployeeId", selectId.Value);
                ShowDetail();
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        private void ShowDetail()
        {
            DataRow r = UiHelper.SelectedRow(dgvEmployees);
            if (r == null)
            {
                ClearFields();
            }
            else
            {
                txtFullName.Text = DbHelper.Value<string>(r, "FullName");
                txtUsername.Text = DbHelper.Value<string>(r, "Username");
                Lookups.SelectValue(cboRole, r["RoleId"]);
                txtPhone.Text = DbHelper.Value<string>(r, "Phone", "");
                txtEmail.Text = DbHelper.Value<string>(r, "Email", "");
                dtpHireDate.Value = DbHelper.Value<DateTime>(r, "HireDate");
                txtPassword.Text = "";
            }
            UpdateButtons();
        }

        private void ClearFields()
        {
            txtFullName.Text = "";
            txtUsername.Text = "";
            if (cboRole.Items.Count > 0) cboRole.SelectedIndex = 0;
            txtPhone.Text = "";
            txtEmail.Text = "";
            dtpHireDate.Value = DateTime.Today;
            txtPassword.Text = "";
        }

        // ==================================================================
        // Chế độ Xem / Thêm / Sửa
        // ==================================================================

        private void SetMode(EditMode mode)
        {
            _mode = mode;
            bool editing = mode != EditMode.View;
            txtFullName.ReadOnly = !editing;
            txtUsername.ReadOnly = !editing;
            txtPhone.ReadOnly = !editing;
            txtEmail.ReadOnly = !editing;
            cboRole.Enabled = editing;
            dtpHireDate.Enabled = editing;
            // Mật khẩu chỉ nhập khi thêm mới; đổi về sau dùng "Đặt lại mật khẩu".
            txtPassword.Enabled = mode == EditMode.Add;
            lblPassword.Enabled = mode == EditMode.Add;
            dgvEmployees.Enabled = !editing;
            pnlTop.Enabled = !editing;
            tabRoles.Enabled = !editing;
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            bool editing = _mode != EditMode.View;
            DataRow r = UiHelper.SelectedRow(dgvEmployees);
            bool hasRow = r != null;
            btnAdd.Enabled = !editing;
            btnEdit.Enabled = !editing && hasRow;
            btnSave.Enabled = editing;
            btnCancel.Enabled = editing;
            btnToggleLock.Enabled = !editing && hasRow;
            btnResetPassword.Enabled = !editing && hasRow;
            btnToggleLock.Text = hasRow && !DbHelper.Value<bool>(r, "IsActive") ? "Mở khóa tài khoản" : "Khóa tài khoản";
        }

        private void dgvEmployees_SelectionChanged(object sender, EventArgs e)
        {
            if (_mode == EditMode.View) ShowDetail();
        }

        private void filter_Changed(object sender, EventArgs e)
        {
            if (_mode == EditMode.View) LoadData();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            LoadRoles();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearFields();
            SetMode(EditMode.Add);
            txtFullName.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!UiHelper.RequireSelectedRow(dgvEmployees, "một nhân viên")) return;
            SetMode(EditMode.Edit);
            txtFullName.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetMode(EditMode.View);
            ShowDetail();
        }

        // ==================================================================
        // Lưu / Khóa / Đặt lại mật khẩu
        // ==================================================================

        private bool ValidateInput()
        {
            if (!UiHelper.RequireText(txtFullName, "họ tên")) return false;
            if (!UiHelper.RequireText(txtUsername, "tên đăng nhập")) return false;
            foreach (char c in txtUsername.Text.Trim())
            {
                bool ok = (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '_' || c == '.';
                if (!ok)
                {
                    UiHelper.Warn("Tên đăng nhập chỉ gồm chữ cái không dấu, chữ số, dấu chấm và gạch dưới.", this);
                    txtUsername.Focus();
                    return false;
                }
            }
            if (!UiHelper.RequireSelection(cboRole, "vai trò")) return false;
            if (!UiHelper.IsValidPhone(txtPhone.Text))
            {
                UiHelper.Warn("Số điện thoại không hợp lệ (8–15 chữ số).", this);
                txtPhone.Focus();
                return false;
            }
            if (!UiHelper.IsValidEmail(txtEmail.Text))
            {
                UiHelper.Warn("Email không đúng định dạng.", this);
                txtEmail.Focus();
                return false;
            }
            if (_mode == EditMode.Add)
            {
                string error;
                if (!PasswordHasher.IsValidNewPassword(txtPassword.Text, out error))
                {
                    UiHelper.Warn(error, this);
                    txtPassword.Focus();
                    return false;
                }
            }
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;
            int roleId = Lookups.SelectedInt(cboRole).Value;

            try
            {
                int id;
                if (_mode == EditMode.Add)
                {
                    id = DbHelper.Scalar<int>(
                        "INSERT INTO dbo.Employees (RoleId, FullName, Username, PasswordHash, Phone, Email, HireDate, IsActive) " +
                        "VALUES (@RoleId, @FullName, @Username, @Hash, @Phone, @Email, @HireDate, 1); " +
                        "SELECT CAST(SCOPE_IDENTITY() AS INT);",
                        DbHelper.P("@RoleId", roleId),
                        DbHelper.P("@FullName", txtFullName.Text),
                        DbHelper.P("@Username", txtUsername.Text),
                        DbHelper.P("@Hash", PasswordHasher.Hash(txtPassword.Text)),
                        DbHelper.P("@Phone", txtPhone.Text),
                        DbHelper.P("@Email", txtEmail.Text),
                        DbHelper.P("@HireDate", dtpHireDate.Value.Date));
                }
                else
                {
                    DataRow r = UiHelper.SelectedRow(dgvEmployees);
                    id = DbHelper.Value<int>(r, "EmployeeId");

                    // Đổi một Admin đang hoạt động sang vai trò khác: phải còn ít nhất một Admin khác.
                    bool wasAdmin = DbHelper.Value<string>(r, "RoleName") == Roles.Admin;
                    bool stillAdmin = DbHelper.Scalar<string>("SELECT RoleName FROM dbo.Roles WHERE RoleId = @R",
                        DbHelper.P("@R", roleId)) == Roles.Admin;
                    if (wasAdmin && !stillAdmin && DbHelper.Value<bool>(r, "IsActive") && !OtherActiveAdminExists(id))
                    {
                        UiHelper.Warn("Đây là Admin cuối cùng đang hoạt động nên không đổi vai trò được.", this);
                        return;
                    }

                    DbHelper.ExecText(
                        "UPDATE dbo.Employees SET RoleId = @RoleId, FullName = @FullName, Username = @Username, " +
                        "Phone = @Phone, Email = @Email, HireDate = @HireDate WHERE EmployeeId = @Id",
                        DbHelper.P("@RoleId", roleId),
                        DbHelper.P("@FullName", txtFullName.Text),
                        DbHelper.P("@Username", txtUsername.Text),
                        DbHelper.P("@Phone", txtPhone.Text),
                        DbHelper.P("@Email", txtEmail.Text),
                        DbHelper.P("@HireDate", dtpHireDate.Value.Date),
                        DbHelper.P("@Id", id));
                }

                SetMode(EditMode.View);
                LoadData(id);
                LoadRoles();
            }
            catch (Exception ex)
            {
                // Trùng tài khoản (UQ_Employees_Username) -> "Tên đăng nhập đã được dùng."
                ErrorHandler.Show(ex, this);
            }
        }

        private bool OtherActiveAdminExists(int employeeId)
        {
            return DbHelper.Scalar<int>(SqlOtherActiveAdmins, DbHelper.P("@Id", employeeId)) > 0;
        }

        private void btnToggleLock_Click(object sender, EventArgs e)
        {
            DataRow r = UiHelper.SelectedRow(dgvEmployees);
            if (r == null) return;
            int id = DbHelper.Value<int>(r, "EmployeeId");
            bool active = DbHelper.Value<bool>(r, "IsActive");
            string name = DbHelper.Value<string>(r, "FullName");

            try
            {
                if (active)
                {
                    if (id == Session.EmployeeId)
                    {
                        UiHelper.Warn("Không thể tự khóa tài khoản đang đăng nhập.", this);
                        return;
                    }
                    if (DbHelper.Value<string>(r, "RoleName") == Roles.Admin && !OtherActiveAdminExists(id))
                    {
                        UiHelper.Warn("Đây là Admin cuối cùng đang hoạt động nên không khóa được.", this);
                        return;
                    }
                    if (!UiHelper.Confirm("Khóa tài khoản của " + name + "? Nhân viên sẽ không đăng nhập được nữa.",
                            defaultNo: true, owner: this)) return;
                }
                else if (!UiHelper.Confirm("Mở khóa tài khoản của " + name + "?", owner: this))
                {
                    return;
                }

                DbHelper.ExecText("UPDATE dbo.Employees SET IsActive = @Active WHERE EmployeeId = @Id",
                    DbHelper.P("@Active", !active), DbHelper.P("@Id", id));
                LoadData(id);
                LoadRoles();
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            DataRow r = UiHelper.SelectedRow(dgvEmployees);
            if (r == null) return;
            string username = DbHelper.Value<string>(r, "Username");
            if (!UiHelper.Confirm("Đặt lại mật khẩu của tài khoản \"" + username + "\" về \"" +
                                  PasswordHasher.DefaultResetPassword + "\"?", defaultNo: true, owner: this)) return;

            try
            {
                DbHelper.ExecText("UPDATE dbo.Employees SET PasswordHash = @Hash WHERE EmployeeId = @Id",
                    DbHelper.P("@Hash", PasswordHasher.Hash(PasswordHasher.DefaultResetPassword)),
                    DbHelper.P("@Id", DbHelper.Value<int>(r, "EmployeeId")));
                UiHelper.Info("Đã đặt lại mật khẩu của \"" + username + "\" về " + PasswordHasher.DefaultResetPassword +
                              ". Hãy nhắc nhân viên đổi mật khẩu sau khi đăng nhập.", this);
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        // ==================================================================
        // Tab Vai trò (chỉ sửa mô tả)
        // ==================================================================

        private void LoadRoles()
        {
            try
            {
                DataTable dt = DbHelper.QueryText(SqlRoles);
                UiHelper.Bind(dgvRoles, dt, "RoleId");
                UiHelper.ShowColumns(dgvRoles,
                    "RoleName", "Vai trò",
                    "Description", "Mô tả",
                    "ActiveEmployees", "Số nhân viên hoạt động");
                ShowRole();
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        private void ShowRole()
        {
            DataRow r = UiHelper.SelectedRow(dgvRoles);
            txtRoleDescription.Text = r == null ? "" : DbHelper.Value<string>(r, "Description", "");
            btnSaveRole.Enabled = r != null;
        }

        private void dgvRoles_SelectionChanged(object sender, EventArgs e)
        {
            ShowRole();
        }

        private void btnSaveRole_Click(object sender, EventArgs e)
        {
            int? id = UiHelper.SelectedId(dgvRoles, "RoleId");
            if (id == null) return;
            try
            {
                DbHelper.ExecText("UPDATE dbo.Roles SET Description = @Desc WHERE RoleId = @Id",
                    DbHelper.P("@Desc", txtRoleDescription.Text), DbHelper.P("@Id", id.Value));
                LoadRoles();
                UiHelper.Info("Đã lưu mô tả vai trò.", this);
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }
    }
}
