using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using HotelManagement.Core;

namespace Hotel_management_system.Forms.Catalog
{
    /// <summary>
    /// Danh mục phòng (Admin, Manager sửa; Receptionist chỉ xem). Đặc tả: KE_HOACH mục 2; nghiệp vụ mục 6.
    /// - Sửa số phòng (duy nhất), tầng, loại phòng (chỉ loại đang dùng), ghi chú, IsActive.
    /// - KHÔNG sửa trực tiếp Rooms.Status: trạng thái đổi qua procedure (check-in/out, dọn phòng,
    ///   sp_SetRoomMaintenance, sp_FinishMaintenance).
    /// - Trước khi ngừng khai thác: phòng không còn lượt Booked/CheckedIn.
    /// </summary>
    public partial class frmRooms : Form
    {
        private const string SqlList = @"
SELECT r.RoomId, r.RoomNumber, r.Floor, r.RoomTypeId, rt.TypeName, rt.BasePrice, r.Status, r.Note, r.IsActive,
       (SELECT COUNT(*) FROM dbo.BookingRooms br
        WHERE br.RoomId = r.RoomId AND br.Status IN ('Booked', 'CheckedIn')) AS ActiveStays
FROM dbo.Rooms r
JOIN dbo.RoomTypes rt ON rt.RoomTypeId = r.RoomTypeId
WHERE (@ShowInactive = 1 OR r.IsActive = 1)
  AND (@Floor IS NULL OR r.Floor = @Floor)
  AND (@TypeId IS NULL OR r.RoomTypeId = @TypeId)
  AND (@Status IS NULL OR r.Status = @Status)
  AND (@Kw IS NULL OR r.RoomNumber LIKE @Kw)
ORDER BY r.Floor, r.RoomNumber;";

        private enum EditMode { View, Add, Edit }

        private EditMode _mode = EditMode.View;
        private bool _canEdit;
        private bool _loading;

        public frmRooms()
        {
            InitializeComponent();
            UiHelper.StyleGrid(dgvRooms);
            UiHelper.TranslateColumn(dgvRooms, "Status", RoomStatus.ToVietnamese);
            UiHelper.ColorRows(dgvRooms, "Status", RoomStatus.ToColor);
        }

        private void frmRooms_Load(object sender, EventArgs e)
        {
            if (!UiHelper.RequireRole(this, Roles.Admin, Roles.Manager, Roles.Receptionist)) return;
            _canEdit = Session.HasRole(Roles.Admin, Roles.Manager);
            if (!_canEdit) Text += " (chỉ xem)";

            LoadFilters();
            LoadData();
            SetMode(EditMode.View);
        }

        private void LoadFilters()
        {
            try
            {
                _loading = true;
                Lookups.LoadFloors(cboFloorFilter, Lookups.AllText);
                Lookups.LoadRoomTypes(cboTypeFilter, false, Lookups.AllText);
                Lookups.LoadRoomStatuses(cboStatusFilter, Lookups.AllText);
                Lookups.LoadRoomTypes(cboType, false);
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
                    DbHelper.P("@Floor", Lookups.SelectedInt(cboFloorFilter)),
                    DbHelper.P("@TypeId", Lookups.SelectedInt(cboTypeFilter)),
                    DbHelper.P("@Status", Lookups.SelectedString(cboStatusFilter)),
                    DbHelper.P("@Kw", DbHelper.LikeContains(txtSearch.Text)));
                UiHelper.Bind(dgvRooms, dt, "RoomId");
                UiHelper.ShowColumns(dgvRooms,
                    "RoomNumber", "Số phòng",
                    "Floor", "Tầng",
                    "TypeName", "Loại phòng",
                    "BasePrice", "Giá/đêm",
                    "Status", "Tình trạng",
                    "ActiveStays", "Lượt đặt/ở",
                    "Note", "Ghi chú",
                    "IsActive", "Khai thác");
                if (selectId.HasValue) UiHelper.SelectRowById(dgvRooms, "RoomId", selectId.Value);
                ShowDetail();
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        private void ShowDetail()
        {
            DataRow r = UiHelper.SelectedRow(dgvRooms);
            if (r == null)
            {
                txtRoomNumber.Text = "";
                nudFloor.Value = 1;
                cboType.SelectedIndex = cboType.Items.Count > 0 ? 0 : -1;
                txtNote.Text = "";
                chkActive.Checked = true;
                lblStatusValue.Text = "";
            }
            else
            {
                txtRoomNumber.Text = DbHelper.Value<string>(r, "RoomNumber");
                nudFloor.Value = DbHelper.Value<int>(r, "Floor");
                Lookups.SelectValue(cboType, r["RoomTypeId"]);
                txtNote.Text = DbHelper.Value<string>(r, "Note", "");
                chkActive.Checked = DbHelper.Value<bool>(r, "IsActive");
                string status = DbHelper.Value<string>(r, "Status");
                lblStatusValue.Text = RoomStatus.ToVietnamese(status);
                lblStatusValue.BackColor = RoomStatus.ToColor(status);
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
            txtRoomNumber.ReadOnly = !editing;
            nudFloor.Enabled = editing;
            cboType.Enabled = editing;
            txtNote.ReadOnly = !editing;
            chkActive.Enabled = editing;
            dgvRooms.Enabled = !editing;
            pnlTop.Enabled = !editing;

            // Thêm/sửa phòng chỉ được chọn loại đang dùng; ở chế độ xem nạp đủ để hiện đúng loại cũ.
            try
            {
                object current = Lookups.SelectedInt(cboType);
                Lookups.LoadRoomTypes(cboType, editing);
                if (current != null) Lookups.SelectValue(cboType, current);
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            bool editing = _mode != EditMode.View;
            DataRow r = UiHelper.SelectedRow(dgvRooms);
            string status = r == null ? null : DbHelper.Value<string>(r, "Status");
            bool active = r != null && DbHelper.Value<bool>(r, "IsActive");

            foreach (Control c in new Control[] { btnAdd, btnEdit, btnSave, btnCancel, btnMaintenance, btnFinishMaintenance })
                c.Visible = _canEdit;

            btnAdd.Enabled = !editing;
            btnEdit.Enabled = !editing && r != null;
            btnSave.Enabled = editing;
            btnCancel.Enabled = editing;
            btnMaintenance.Enabled = !editing && active && status == RoomStatus.Available;
            btnFinishMaintenance.Enabled = !editing && status == RoomStatus.Maintenance;
        }

        private void dgvRooms_SelectionChanged(object sender, EventArgs e)
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
            LoadFilters();
            LoadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            txtRoomNumber.Text = "";
            txtNote.Text = "";
            chkActive.Checked = true;
            lblStatusValue.Text = RoomStatus.ToVietnamese(RoomStatus.Available) + " (phòng mới)";
            lblStatusValue.BackColor = RoomStatus.ToColor(RoomStatus.Available);
            SetMode(EditMode.Add);
            txtRoomNumber.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!UiHelper.RequireSelectedRow(dgvRooms, "một phòng")) return;
            SetMode(EditMode.Edit);
            txtRoomNumber.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetMode(EditMode.View);
            ShowDetail();
        }

        // ==================================================================
        // Lưu
        // ==================================================================

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!UiHelper.RequireText(txtRoomNumber, "số phòng")) return;
            if (!UiHelper.RequireSelection(cboType, "loại phòng (đang dùng)")) return;

            try
            {
                int id;
                if (_mode == EditMode.Add)
                {
                    // Phòng mới luôn Available (giá trị mặc định của cột Status).
                    id = DbHelper.Scalar<int>(
                        "INSERT INTO dbo.Rooms (RoomTypeId, RoomNumber, Floor, Note, IsActive) " +
                        "VALUES (@TypeId, @Number, @Floor, @Note, @Active); SELECT CAST(SCOPE_IDENTITY() AS INT);",
                        DbHelper.P("@TypeId", Lookups.SelectedInt(cboType)),
                        DbHelper.P("@Number", txtRoomNumber.Text),
                        DbHelper.P("@Floor", (short)nudFloor.Value),
                        DbHelper.P("@Note", txtNote.Text),
                        DbHelper.P("@Active", chkActive.Checked));
                }
                else
                {
                    DataRow r = UiHelper.SelectedRow(dgvRooms);
                    id = DbHelper.Value<int>(r, "RoomId");

                    // Ngừng khai thác: kiểm tra lại ngay trong câu UPDATE (không có lượt Booked/CheckedIn).
                    if (!chkActive.Checked && DbHelper.Value<bool>(r, "IsActive"))
                    {
                        int stays = DbHelper.Scalar<int>(
                            "SELECT COUNT(*) FROM dbo.BookingRooms WHERE RoomId = @Id AND Status IN ('Booked', 'CheckedIn')",
                            DbHelper.P("@Id", id));
                        if (stays > 0)
                        {
                            UiHelper.Warn("Phòng còn " + stays + " lượt đặt/đang ở. Hãy đổi phòng hoặc chờ trả phòng " +
                                          "trước khi ngừng khai thác.", this);
                            return;
                        }
                    }

                    DbHelper.ExecText(
                        "UPDATE dbo.Rooms SET RoomNumber = @Number, Floor = @Floor, RoomTypeId = @TypeId, Note = @Note, " +
                        "IsActive = @Active WHERE RoomId = @Id " +
                        "AND (@Active = 1 OR NOT EXISTS (SELECT 1 FROM dbo.BookingRooms WHERE RoomId = @Id " +
                        "AND Status IN ('Booked', 'CheckedIn')))",
                        DbHelper.P("@Number", txtRoomNumber.Text),
                        DbHelper.P("@Floor", (short)nudFloor.Value),
                        DbHelper.P("@TypeId", Lookups.SelectedInt(cboType)),
                        DbHelper.P("@Note", txtNote.Text),
                        DbHelper.P("@Active", chkActive.Checked),
                        DbHelper.P("@Id", id));
                }

                FormManager.NotifyDataChanged(DataArea.Rooms);
                FormManager.NotifyDataChanged(DataArea.Catalog);
                SetMode(EditMode.View);
                LoadFilters();
                LoadData(id);
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        // ==================================================================
        // Bảo trì
        // ==================================================================

        private void btnMaintenance_Click(object sender, EventArgs e)
        {
            DataRow r = UiHelper.SelectedRow(dgvRooms);
            if (r == null) return;
            int id = DbHelper.Value<int>(r, "RoomId");
            string number = DbHelper.Value<string>(r, "RoomNumber");

            string note;
            if (!UiHelper.InputText(this, "Đưa phòng " + number + " vào bảo trì",
                    "Lý do bảo trì (bắt buộc):", out note)) return;

            try
            {
                DataTable affected = DbHelper.Query("dbo.sp_SetRoomMaintenance",
                    DbHelper.P("@RoomId", id), DbHelper.P("@Note", note), Session.EmployeeParam());

                FormManager.NotifyDataChanged(DataArea.Rooms);
                LoadData(id);

                // Procedure trả các lượt đã đặt sắp tới của phòng: lễ tân cần đổi phòng cho khách.
                if (affected.Rows.Count > 0)
                {
                    var sb = new StringBuilder("Phòng " + number + " đã vào bảo trì. Các lượt đặt sau cần đổi phòng " +
                                               "(Đặt phòng > Danh sách phiếu > Sửa):\n\n");
                    foreach (DataRow a in affected.Rows)
                    {
                        sb.AppendLine("  • Phiếu #" + DbHelper.Value<int>(a, "BookingId") + " – " +
                                      DbHelper.Value<string>(a, "CustomerName") + " (" +
                                      DbHelper.Value<string>(a, "CustomerPhone", "") + "): " +
                                      Formatters.Date(a["CheckInDate"]) + " → " + Formatters.Date(a["CheckOutDate"]));
                    }
                    UiHelper.Warn(sb.ToString(), this);
                }
                else
                {
                    UiHelper.Info("Phòng " + number + " đã vào bảo trì.", this);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        private void btnFinishMaintenance_Click(object sender, EventArgs e)
        {
            DataRow r = UiHelper.SelectedRow(dgvRooms);
            if (r == null) return;
            int id = DbHelper.Value<int>(r, "RoomId");
            string number = DbHelper.Value<string>(r, "RoomNumber");

            string note;
            if (!UiHelper.InputText(this, "Kết thúc bảo trì phòng " + number,
                    "Ghi chú kết quả sửa chữa (có thể để trống):", out note, required: false)) return;

            try
            {
                DbHelper.Exec("dbo.sp_FinishMaintenance",
                    DbHelper.P("@RoomId", id), DbHelper.P("@Note", note), Session.EmployeeParam());
                FormManager.NotifyDataChanged(DataArea.Rooms);
                FormManager.NotifyDataChanged(DataArea.Housekeeping);
                LoadData(id);
                UiHelper.Info("Phòng " + number + " chuyển sang \"Đang dọn\" và đã có việc dọn mới trong hàng đợi.", this);
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }
    }
}
