using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HotelManagement.Core;

namespace Hotel_management_system.Forms.Operations
{
    /// <summary>
    /// Hàng đợi dọn phòng (Housekeeping, Admin, Manager; Receptionist chỉ xem). Đặc tả: KE_HOACH mục 6; nghiệp vụ mục 14.
    /// - Việc dọn sinh tự động khi check-out (CheckOut) hoặc kết thúc bảo trì (AfterMaintenance).
    /// - Nhận việc -> dbo.sp_StartHousekeepingTask (Pending -> InProgress, ghi người nhận).
    /// - Hoàn tất -> dbo.sp_CompleteHousekeepingTask: Sạch -> phòng Available;
    ///   Cần bảo trì (bắt buộc ghi chú) -> phòng Maintenance.
    /// - Buồng phòng mặc định chỉ thấy việc của mình và việc chưa ai nhận. Tự làm mới mỗi 60 giây.
    /// </summary>
    public partial class frmHousekeeping : Form
    {
        // Việc đang mở lấy từ vw_HousekeepingQueue; việc đã xong hôm nay đọc thẳng bảng (view chỉ có việc chưa xong).
        private const string SqlList = @"
SELECT x.* FROM (
    SELECT q.TaskId, q.RoomId, q.RoomNumber, q.Floor, q.TypeName, q.RoomStatus, q.BookingRoomId, q.Source, q.Status,
           q.AssignedTo, q.AssignedToName, q.CreatedAt, q.StartedAt,
           CAST(NULL AS DATETIME2(0)) AS CompletedAt, CAST(NULL AS VARCHAR(20)) AS Result, q.Note, q.WaitingMinutes
    FROM dbo.vw_HousekeepingQueue q
    UNION ALL
    SELECT t.TaskId, t.RoomId, r.RoomNumber, r.Floor, rt.TypeName, r.Status, t.BookingRoomId, t.Source, t.Status,
           t.AssignedTo, e.FullName, t.CreatedAt, t.StartedAt, t.CompletedAt, t.Result, t.Note, NULL
    FROM dbo.HousekeepingTasks t
    JOIN dbo.Rooms r      ON r.RoomId = t.RoomId
    JOIN dbo.RoomTypes rt ON rt.RoomTypeId = r.RoomTypeId
    LEFT JOIN dbo.Employees e ON e.EmployeeId = t.AssignedTo
    WHERE t.Status = 'Done' AND t.CompletedAt >= CAST(CAST(GETDATE() AS DATE) AS DATETIME2(0))
) x
WHERE ((@Status IS NULL AND x.Status <> 'Done') OR x.Status = @Status)
  AND (@Mine = 0 OR x.AssignedTo IS NULL OR x.AssignedTo = @Me)
ORDER BY CASE x.Status WHEN 'InProgress' THEN 0 WHEN 'Pending' THEN 1 ELSE 2 END,
         x.CreatedAt, x.RoomNumber;";

        private static readonly Color PendingColor = Color.FromArgb(255, 235, 156);
        private static readonly Color InProgressColor = Color.FromArgb(204, 228, 247);
        private static readonly Color DoneColor = Color.FromArgb(198, 239, 206);

        private bool _loading;
        private bool _canWork;
        private bool _reloadPending;

        public frmHousekeeping()
        {
            InitializeComponent();
            UiHelper.StyleGrid(dgvTasks);
            UiHelper.TranslateColumn(dgvTasks, "Status", HousekeepingStatus.ToVietnamese);
            UiHelper.TranslateColumn(dgvTasks, "Source", HousekeepingSource.ToVietnamese);
            UiHelper.TranslateColumn(dgvTasks, "Result", HousekeepingResult.ToVietnamese);
            UiHelper.TranslateColumn(dgvTasks, "RoomStatus", RoomStatus.ToVietnamese);
            UiHelper.TranslateColumn(dgvTasks, "WaitingMinutes", FormatWaiting);
            UiHelper.ColorRows(dgvTasks, "Status", ColorByStatus);
        }

        private void frmHousekeeping_Load(object sender, EventArgs e)
        {
            if (!UiHelper.RequireRole(this, Roles.Housekeeping, Roles.Admin, Roles.Manager, Roles.Receptionist)) return;

            // Lễ tân chỉ xem (ma trận quyền I.1).
            _canWork = Session.HasRole(Roles.Housekeeping, Roles.Admin, Roles.Manager);
            btnStart.Visible = btnClean.Visible = btnNeedMaintenance.Visible = _canWork;
            lblReadOnly.Visible = !_canWork;
            if (!_canWork) Text += " (chỉ xem)";

            _loading = true;
            Lookups.Bind(cboView, new List<ComboItem>
            {
                new ComboItem(null, "Việc đang mở (chờ dọn + đang dọn)"),
                new ComboItem(HousekeepingStatus.Pending, "Chỉ việc chờ dọn"),
                new ComboItem(HousekeepingStatus.InProgress, "Chỉ việc đang dọn"),
                new ComboItem(HousekeepingStatus.Done, "Đã hoàn tất hôm nay")
            });
            chkMine.Checked = Session.IsHousekeeping;
            chkMine.Visible = _canWork;
            _loading = false;

            LoadData();
            FormManager.Subscribe(this, OnDataChanged);
            timerRefresh.Start();
        }

        private void LoadData(bool fromTimer = false)
        {
            try
            {
                DataTable dt = DbHelper.QueryText(SqlList,
                    DbHelper.P("@Status", Lookups.SelectedString(cboView)),
                    DbHelper.P("@Mine", chkMine.Visible && chkMine.Checked),
                    DbHelper.P("@Me", Session.EmployeeId));
                UiHelper.Bind(dgvTasks, dt, "TaskId");
                UiHelper.ShowColumns(dgvTasks,
                    "RoomNumber", "Phòng",
                    "Floor", "Tầng",
                    "TypeName", "Loại phòng",
                    "Status", "Trạng thái",
                    "Source", "Nguồn",
                    "WaitingMinutes", "Đã chờ",
                    "AssignedToName", "Người nhận",
                    "CreatedAt", "Tạo lúc",
                    "StartedAt", "Bắt đầu",
                    "CompletedAt", "Xong lúc",
                    "Result", "Kết quả",
                    "Note", "Ghi chú");
                lblUpdated.Text = dt.Rows.Count + " việc – cập nhật lúc " + Formatters.Time(DateTime.Now);
            }
            catch (Exception ex)
            {
                if (fromTimer) ErrorHandler.Log(ex);
                else ErrorHandler.Show(ex, this);
            }
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            DataRow r = UiHelper.SelectedRow(dgvTasks);
            string status = r == null ? null : DbHelper.Value<string>(r, "Status");
            int? assigned = r == null ? null : DbHelper.Value<int?>(r, "AssignedTo");
            // Việc đang do người khác làm: chỉ Admin/Manager được hoàn tất thay (DB cũng chặn).
            bool mineOrFree = assigned == null || assigned == Session.EmployeeId || Session.HasRole(Roles.Admin, Roles.Manager);

            btnStart.Enabled = status == HousekeepingStatus.Pending;
            btnClean.Enabled = (status == HousekeepingStatus.Pending || status == HousekeepingStatus.InProgress) && mineOrFree;
            btnNeedMaintenance.Enabled = btnClean.Enabled;
        }

        private static string FormatWaiting(string value)
        {
            int minutes;
            return int.TryParse(value, out minutes) && minutes > 0 ? Formatters.Duration(minutes) : "";
        }

        private static Color ColorByStatus(string status)
        {
            switch (status)
            {
                case HousekeepingStatus.Pending: return PendingColor;
                case HousekeepingStatus.InProgress: return InProgressColor;
                case HousekeepingStatus.Done: return DoneColor;
                default: return Color.Empty;
            }
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

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            LoadData(fromTimer: true);
        }

        private void dgvTasks_SelectionChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void OnDataChanged(string area)
        {
            if (area != DataArea.Housekeeping && area != DataArea.Rooms) return;
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

        private void btnStart_Click(object sender, EventArgs e)
        {
            DataRow r = UiHelper.SelectedRow(dgvTasks);
            if (r == null) return;
            int taskId = DbHelper.Value<int>(r, "TaskId");
            try
            {
                DbHelper.Exec("dbo.sp_StartHousekeepingTask", DbHelper.P("@TaskId", taskId), Session.EmployeeParam());
            }
            catch (Exception ex)
            {
                // Người khác vừa nhận trước (50003) -> báo và nạp lại.
                ErrorHandler.Show(ex, this);
                LoadData();
                return;
            }
            FormManager.NotifyDataChanged(DataArea.Housekeeping);
            LoadData();
            UiHelper.SelectRowById(dgvTasks, "TaskId", taskId);
        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            DataRow r = UiHelper.SelectedRow(dgvTasks);
            if (r == null) return;
            if (!UiHelper.Confirm("Phòng " + DbHelper.Value<string>(r, "RoomNumber") + " đã dọn sạch, sẵn sàng đón khách?",
                    owner: this)) return;
            Complete(r, HousekeepingResult.Clean, null);
        }

        private void btnNeedMaintenance_Click(object sender, EventArgs e)
        {
            DataRow r = UiHelper.SelectedRow(dgvTasks);
            if (r == null) return;
            string note;
            if (!UiHelper.InputText(this, "Báo phòng " + DbHelper.Value<string>(r, "RoomNumber") + " cần bảo trì",
                    "Mô tả tình trạng hỏng (bắt buộc):", out note, maxLength: 500)) return;
            Complete(r, HousekeepingResult.NeedMaintenance, note);
        }

        private void Complete(DataRow r, string result, string note)
        {
            string room = DbHelper.Value<string>(r, "RoomNumber");
            try
            {
                DbHelper.Exec("dbo.sp_CompleteHousekeepingTask",
                    DbHelper.P("@TaskId", DbHelper.Value<int>(r, "TaskId")),
                    DbHelper.P("@Result", result),
                    DbHelper.P("@Note", note),
                    Session.EmployeeParam());
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
                LoadData();
                return;
            }
            FormManager.NotifyDataChanged(DataArea.Housekeeping);
            FormManager.NotifyDataChanged(DataArea.Rooms);
            LoadData();
            UiHelper.Info(result == HousekeepingResult.Clean
                ? "Phòng " + room + " đã sẵn sàng (Trống)."
                : "Phòng " + room + " chuyển sang Bảo trì. Quản lý kết thúc bảo trì ở form Phòng khi sửa xong.", this);
        }
    }
}
