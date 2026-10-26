using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HotelManagement.Core;

namespace Hotel_management_system.Forms.Catalog
{
    /// <summary>
    /// Danh mục loại phòng (Admin, Manager sửa; Receptionist chỉ xem). Đặc tả: KE_HOACH mục 2; nghiệp vụ mục 5.
    /// - TypeName duy nhất, BasePrice > 0, MaxOccupancy 1–10 (CHECK trong DB).
    /// - Tiện nghi của loại phòng lưu cùng một transaction: xóa hết rồi chèn lại (DbHelper.InTransaction).
    /// - Đổi giá không ảnh hưởng phiếu cũ vì giá đã chốt vào BookingRooms.PricePerNight lúc đặt.
    /// - Không xóa loại phòng: "Ngừng dùng" (IsActive = 0) thì phòng thuộc loại này không đặt mới được.
    /// </summary>
    public partial class frmRoomTypes : Form
    {
        private const string SqlList = @"
SELECT rt.RoomTypeId, rt.TypeName, rt.BasePrice, rt.MaxOccupancy, rt.BedType, rt.Description, rt.IsActive,
       (SELECT COUNT(*) FROM dbo.Rooms r WHERE r.RoomTypeId = rt.RoomTypeId) AS RoomCount
FROM dbo.RoomTypes rt
WHERE (@ShowInactive = 1 OR rt.IsActive = 1)
  AND (@Kw IS NULL OR rt.TypeName COLLATE Vietnamese_CI_AI LIKE @Kw
                   OR rt.BedType COLLATE Vietnamese_CI_AI LIKE @Kw)
ORDER BY rt.BasePrice, rt.TypeName;";

        private enum EditMode { View, Add, Edit }

        private EditMode _mode = EditMode.View;
        private bool _canEdit;

        public frmRoomTypes()
        {
            InitializeComponent();
            UiHelper.StyleGrid(dgvRoomTypes);
            UiHelper.SetupMoneyInput(nudBasePrice);
            UiHelper.ColorRows(dgvRoomTypes, "IsActive", v => v == "False" ? SystemColors.ControlLight : Color.Empty);
        }

        private void frmRoomTypes_Load(object sender, EventArgs e)
        {
            if (!UiHelper.RequireRole(this, Roles.Admin, Roles.Manager, Roles.Receptionist)) return;
            _canEdit = Session.HasRole(Roles.Admin, Roles.Manager);
            if (!_canEdit) Text += " (chỉ xem)";

            LoadAmenities();
            LoadData();
            SetMode(EditMode.View);
        }

        private void LoadAmenities()
        {
            try
            {
                DataTable dt = DbHelper.QueryText("SELECT AmenityId, AmenityName FROM dbo.Amenities ORDER BY AmenityName");
                clbAmenities.Items.Clear();
                foreach (ComboItem item in Lookups.FromTable(dt, "AmenityId", "AmenityName")) clbAmenities.Items.Add(item);
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
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
                    DbHelper.P("@Kw", DbHelper.LikeContains(txtSearch.Text)));
                UiHelper.Bind(dgvRoomTypes, dt, "RoomTypeId");
                UiHelper.ShowColumns(dgvRoomTypes,
                    "TypeName", "Loại phòng",
                    "BasePrice", "Giá/đêm",
                    "MaxOccupancy", "Sức chứa",
                    "BedType", "Loại giường",
                    "RoomCount", "Số phòng",
                    "IsActive", "Đang dùng");
                if (selectId.HasValue) UiHelper.SelectRowById(dgvRoomTypes, "RoomTypeId", selectId.Value);
                ShowDetail();
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        private void ShowDetail()
        {
            DataRow r = UiHelper.SelectedRow(dgvRoomTypes);
            if (r == null)
            {
                ClearFields();
                UpdateButtons();
                return;
            }

            txtName.Text = DbHelper.Value<string>(r, "TypeName");
            nudBasePrice.Value = DbHelper.Value<decimal>(r, "BasePrice");
            nudMaxOccupancy.Value = DbHelper.Value<int>(r, "MaxOccupancy");
            txtBedType.Text = DbHelper.Value<string>(r, "BedType", "");
            txtDescription.Text = DbHelper.Value<string>(r, "Description", "");
            chkActive.Checked = DbHelper.Value<bool>(r, "IsActive");

            try
            {
                DataTable assigned = DbHelper.QueryText(
                    "SELECT AmenityId FROM dbo.RoomTypeAmenities WHERE RoomTypeId = @Id",
                    DbHelper.P("@Id", DbHelper.Value<int>(r, "RoomTypeId")));
                var ids = new HashSet<int>();
                foreach (DataRow a in assigned.Rows) ids.Add(DbHelper.Value<int>(a, "AmenityId"));
                for (int i = 0; i < clbAmenities.Items.Count; i++)
                {
                    var item = (ComboItem)clbAmenities.Items[i];
                    clbAmenities.SetItemChecked(i, ids.Contains(Convert.ToInt32(item.Value)));
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
            UpdateButtons();
        }

        private void ClearFields()
        {
            txtName.Text = "";
            nudBasePrice.Value = 0;
            nudMaxOccupancy.Value = 2;
            txtBedType.Text = "";
            txtDescription.Text = "";
            chkActive.Checked = true;
            for (int i = 0; i < clbAmenities.Items.Count; i++) clbAmenities.SetItemChecked(i, false);
        }

        // ==================================================================
        // Chế độ Xem / Thêm / Sửa
        // ==================================================================

        private void SetMode(EditMode mode)
        {
            _mode = mode;
            bool editing = mode != EditMode.View;
            txtName.ReadOnly = !editing;
            txtBedType.ReadOnly = !editing;
            txtDescription.ReadOnly = !editing;
            nudBasePrice.Enabled = editing;
            nudMaxOccupancy.Enabled = editing;
            chkActive.Enabled = editing;
            clbAmenities.Enabled = editing;
            dgvRoomTypes.Enabled = !editing;
            pnlTop.Enabled = !editing;
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            bool editing = _mode != EditMode.View;
            DataRow r = UiHelper.SelectedRow(dgvRoomTypes);

            btnAdd.Visible = btnEdit.Visible = btnToggleActive.Visible = _canEdit;
            btnSave.Visible = btnCancel.Visible = _canEdit;

            btnAdd.Enabled = !editing;
            btnEdit.Enabled = !editing && r != null;
            btnToggleActive.Enabled = !editing && r != null;
            btnToggleActive.Text = r != null && !DbHelper.Value<bool>(r, "IsActive") ? "Dùng lại" : "Ngừng dùng";
            btnSave.Enabled = editing;
            btnCancel.Enabled = editing;
        }

        private void dgvRoomTypes_SelectionChanged(object sender, EventArgs e)
        {
            if (_mode == EditMode.View) ShowDetail();
        }

        private void filter_Changed(object sender, EventArgs e)
        {
            if (_mode == EditMode.View) LoadData();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAmenities();
            LoadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearFields();
            SetMode(EditMode.Add);
            txtName.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!UiHelper.RequireSelectedRow(dgvRoomTypes, "một loại phòng")) return;
            SetMode(EditMode.Edit);
            txtName.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetMode(EditMode.View);
            ShowDetail();
        }

        // ==================================================================
        // Lưu / Ngừng dùng
        // ==================================================================

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!UiHelper.RequireText(txtName, "tên loại phòng")) return;
            if (nudBasePrice.Value <= 0)
            {
                UiHelper.Warn("Giá phòng phải lớn hơn 0.", this);
                nudBasePrice.Focus();
                return;
            }

            var amenityIds = new List<int>();
            foreach (ComboItem item in clbAmenities.CheckedItems) amenityIds.Add(Convert.ToInt32(item.Value));

            int? editId = _mode == EditMode.Edit ? UiHelper.SelectedId(dgvRoomTypes, "RoomTypeId") : null;
            int savedId = 0;

            try
            {
                // Loại phòng + danh sách tiện nghi phải lưu cùng nhau: lỗi ở bước nào thì hủy hết.
                DbHelper.InTransaction((conn, tx) =>
                {
                    var ps = new[]
                    {
                        DbHelper.P("@Name", txtName.Text),
                        DbHelper.P("@Price", nudBasePrice.Value),
                        DbHelper.P("@Max", (byte)nudMaxOccupancy.Value),
                        DbHelper.P("@Bed", txtBedType.Text),
                        DbHelper.P("@Desc", txtDescription.Text),
                        DbHelper.P("@Active", chkActive.Checked)
                    };

                    if (editId == null)
                    {
                        savedId = DbHelper.ScalarTx<int>(conn, tx,
                            "INSERT INTO dbo.RoomTypes (TypeName, BasePrice, MaxOccupancy, BedType, Description, IsActive) " +
                            "VALUES (@Name, @Price, @Max, @Bed, @Desc, @Active); SELECT CAST(SCOPE_IDENTITY() AS INT);",
                            ps);
                    }
                    else
                    {
                        savedId = editId.Value;
                        DbHelper.ExecTextTx(conn, tx,
                            "UPDATE dbo.RoomTypes SET TypeName = @Name, BasePrice = @Price, MaxOccupancy = @Max, " +
                            "BedType = @Bed, Description = @Desc, IsActive = @Active WHERE RoomTypeId = @Id",
                            ps[0], ps[1], ps[2], ps[3], ps[4], ps[5], DbHelper.P("@Id", savedId));
                    }

                    DbHelper.ExecTextTx(conn, tx, "DELETE FROM dbo.RoomTypeAmenities WHERE RoomTypeId = @Id",
                        DbHelper.P("@Id", savedId));
                    foreach (int amenityId in amenityIds)
                    {
                        DbHelper.ExecTextTx(conn, tx,
                            "INSERT INTO dbo.RoomTypeAmenities (RoomTypeId, AmenityId) VALUES (@Id, @A)",
                            DbHelper.P("@Id", savedId), DbHelper.P("@A", amenityId));
                    }
                });

                FormManager.NotifyDataChanged(DataArea.Catalog);
                SetMode(EditMode.View);
                LoadData(savedId);
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        private void btnToggleActive_Click(object sender, EventArgs e)
        {
            DataRow r = UiHelper.SelectedRow(dgvRoomTypes);
            if (r == null) return;
            int id = DbHelper.Value<int>(r, "RoomTypeId");
            bool active = DbHelper.Value<bool>(r, "IsActive");
            int roomCount = DbHelper.Value<int>(r, "RoomCount");

            string question = active
                ? "Ngừng dùng loại phòng \"" + DbHelper.Value<string>(r, "TypeName") + "\"?" +
                  (roomCount > 0 ? "\n" + roomCount + " phòng thuộc loại này sẽ không đặt mới được (phiếu đã đặt vẫn giữ)." : "")
                : "Dùng lại loại phòng \"" + DbHelper.Value<string>(r, "TypeName") + "\"?";
            if (!UiHelper.Confirm(question, owner: this)) return;

            try
            {
                DbHelper.ExecText("UPDATE dbo.RoomTypes SET IsActive = @Active WHERE RoomTypeId = @Id",
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
