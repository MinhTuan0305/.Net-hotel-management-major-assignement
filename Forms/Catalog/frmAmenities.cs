using System;
using System.Data;
using System.Windows.Forms;
using HotelManagement.Core;

namespace Hotel_management_system.Forms.Catalog
{
    /// <summary>
    /// Danh mục tiện nghi (Admin, Manager). Đặc tả: KE_HOACH mục 2; nghiệp vụ mục 7.
    /// Đây là form MẪU cho các form danh mục: lưới trái + khung chi tiết phải,
    /// 3 chế độ Xem / Thêm / Sửa (SetMode), ghi bằng DbHelper.ExecText có tham số.
    /// Xóa chỉ được khi tiện nghi chưa gán cho loại phòng nào; nếu đang gán, SQL Server
    /// báo lỗi khóa ngoại FK_RTA_Amenities và ErrorHandler đổi thành câu tiếng Việt.
    /// </summary>
    public partial class frmAmenities : Form
    {
        private const string SqlList = @"
SELECT a.AmenityId, a.AmenityName, a.Description, COUNT(rta.RoomTypeId) AS RoomTypeCount
FROM dbo.Amenities a
LEFT JOIN dbo.RoomTypeAmenities rta ON rta.AmenityId = a.AmenityId
WHERE (@Kw IS NULL OR a.AmenityName COLLATE Vietnamese_CI_AI LIKE @Kw
                   OR a.Description COLLATE Vietnamese_CI_AI LIKE @Kw)
GROUP BY a.AmenityId, a.AmenityName, a.Description
ORDER BY a.AmenityName;";

        private enum EditMode { View, Add, Edit }

        private EditMode _mode = EditMode.View;

        public frmAmenities()
        {
            InitializeComponent();
            UiHelper.StyleGrid(dgvAmenities);
        }

        private void frmAmenities_Load(object sender, EventArgs e)
        {
            if (!UiHelper.RequireRole(this, Roles.Admin, Roles.Manager)) return;
            LoadData();
            SetMode(EditMode.View);
        }

        // ==================================================================
        // Nạp dữ liệu
        // ==================================================================

        private void LoadData(int? selectId = null)
        {
            try
            {
                DataTable dt = DbHelper.QueryText(SqlList, DbHelper.P("@Kw", DbHelper.LikeContains(txtSearch.Text)));
                UiHelper.Bind(dgvAmenities, dt, "AmenityId");
                UiHelper.ShowColumns(dgvAmenities,
                    "AmenityName", "Tên tiện nghi",
                    "Description", "Mô tả",
                    "RoomTypeCount", "Số loại phòng dùng");
                if (selectId.HasValue) UiHelper.SelectRowById(dgvAmenities, "AmenityId", selectId.Value);
                ShowDetail();
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        private void ShowDetail()
        {
            DataRow r = UiHelper.SelectedRow(dgvAmenities);
            if (r == null)
            {
                txtName.Text = "";
                txtDescription.Text = "";
                lblUsage.Text = "";
            }
            else
            {
                txtName.Text = DbHelper.Value<string>(r, "AmenityName");
                txtDescription.Text = DbHelper.Value<string>(r, "Description", "");
                int count = DbHelper.Value<int>(r, "RoomTypeCount");
                lblUsage.Text = count == 0
                    ? "Chưa gán cho loại phòng nào – xóa được."
                    : "Đang gán cho " + count + " loại phòng – bỏ gán ở form Loại phòng trước khi xóa.";
            }
            UpdateButtons();
        }

        // ==================================================================
        // Chế độ Xem / Thêm / Sửa
        // ==================================================================

        private void SetMode(EditMode mode)
        {
            _mode = mode;
            bool editing = mode != EditMode.View;
            txtName.ReadOnly = !editing;
            txtDescription.ReadOnly = !editing;
            dgvAmenities.Enabled = !editing;
            txtSearch.Enabled = !editing;
            btnRefresh.Enabled = !editing;
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            bool editing = _mode != EditMode.View;
            bool hasRow = UiHelper.SelectedRow(dgvAmenities) != null;
            btnAdd.Enabled = !editing;
            btnEdit.Enabled = !editing && hasRow;
            btnDelete.Enabled = !editing && hasRow;
            btnSave.Enabled = editing;
            btnCancel.Enabled = editing;
        }

        private void dgvAmenities_SelectionChanged(object sender, EventArgs e)
        {
            if (_mode == EditMode.View) ShowDetail();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (_mode == EditMode.View) LoadData();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            txtName.Text = "";
            txtDescription.Text = "";
            lblUsage.Text = "";
            SetMode(EditMode.Add);
            txtName.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!UiHelper.RequireSelectedRow(dgvAmenities, "một tiện nghi")) return;
            SetMode(EditMode.Edit);
            txtName.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetMode(EditMode.View);
            ShowDetail();
        }

        // ==================================================================
        // Lưu / Xóa
        // ==================================================================

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!UiHelper.RequireText(txtName, "tên tiện nghi")) return;

            try
            {
                int id;
                if (_mode == EditMode.Add)
                {
                    id = DbHelper.Scalar<int>(
                        "INSERT INTO dbo.Amenities (AmenityName, Description) VALUES (@Name, @Desc); " +
                        "SELECT CAST(SCOPE_IDENTITY() AS INT);",
                        DbHelper.P("@Name", txtName.Text),
                        DbHelper.P("@Desc", txtDescription.Text));
                }
                else
                {
                    id = UiHelper.SelectedId(dgvAmenities, "AmenityId").Value;
                    DbHelper.ExecText(
                        "UPDATE dbo.Amenities SET AmenityName = @Name, Description = @Desc WHERE AmenityId = @Id",
                        DbHelper.P("@Name", txtName.Text),
                        DbHelper.P("@Desc", txtDescription.Text),
                        DbHelper.P("@Id", id));
                }

                FormManager.NotifyDataChanged(DataArea.Catalog);
                SetMode(EditMode.View);
                LoadData(id);
            }
            catch (Exception ex)
            {
                // Trùng tên (UQ_Amenities_AmenityName) -> "Tên tiện nghi đã tồn tại." Giữ nguyên chữ đã nhập để sửa.
                ErrorHandler.Show(ex, this);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int? id = UiHelper.SelectedId(dgvAmenities, "AmenityId");
            if (id == null) return;
            if (!UiHelper.Confirm("Xóa tiện nghi \"" + txtName.Text + "\"?", defaultNo: true, owner: this)) return;

            try
            {
                DbHelper.ExecText("DELETE FROM dbo.Amenities WHERE AmenityId = @Id", DbHelper.P("@Id", id.Value));
                FormManager.NotifyDataChanged(DataArea.Catalog);
                LoadData();
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }
    }
}
