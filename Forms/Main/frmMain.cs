using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HotelManagement.Core;
using Hotel_management_system.Forms.Billing;
using Hotel_management_system.Forms.Booking;
using Hotel_management_system.Forms.Catalog;
using Hotel_management_system.Forms.Operations;
using Hotel_management_system.Forms.Stay;

namespace Hotel_management_system.Forms.Main
{
    /// <summary>
    /// Màn hình chính sau khi đăng nhập (mọi vai trò). Đặc tả: KE_HOACH mục 1; nghiệp vụ mục 1, 15.
    ///   - Menu ẩn/hiện theo vai trò (ma trận quyền I.1). Housekeeping chỉ thấy Hệ thống và Dọn phòng.
    ///   - Dải ô chỉ số trong ngày (sp_DashboardSummary; doanh thu chỉ Admin/Manager thấy).
    ///   - Sơ đồ phòng (vw_RoomBoard) tô màu theo trạng thái, tự nạp lại khi form khác báo dữ liệu đổi.
    ///   - Mở app thì gọi sp_ExpirePendingBookings để hủy phiếu chờ cọc quá 24 giờ.
    /// Program.cs đọc LoggedOut để biết người dùng bấm Đăng xuất (mở lại frmLogin) hay Thoát.
    /// </summary>
    public partial class frmMain : Form
    {
        /// <summary>True khi người dùng bấm Đăng xuất (Program.cs dùng để mở lại frmLogin).</summary>
        public bool LoggedOut { get; private set; }

        private const string SqlRoomBoard = @"
SELECT RoomId, RoomNumber, Floor, TypeName, RoomStatus, Note,
       CurrentStayId, CurrentGuest, ExpectedCheckOut, ArrivalStayId, ArrivingGuest
FROM dbo.vw_RoomBoard
WHERE IsActive = 1
ORDER BY Floor, RoomNumber;";

        private static readonly Color TileBack = Color.FromArgb(235, 239, 245);
        private static readonly Color TileValue = Color.FromArgb(31, 78, 121);

        private bool _reloadPending;

        public frmMain()
        {
            InitializeComponent();
        }

        // ==================================================================
        // Khởi động
        // ==================================================================

        private void frmMain_Load(object sender, EventArgs e)
        {
            FormManager.Init(this);
            Text = HotelInfo.Name + " – Quản lý khách sạn";
            lblStatusUser.Text = "Nhân viên: " + Session.FullName + " (" + Session.Username + ")";
            lblStatusRole.Text = "Vai trò: " + Roles.ToVietnamese(Session.RoleName);
            ApplyRoleMenus();
            UpdateClock();

            ExpirePendingBookings();
            RefreshAll();

            FormManager.Subscribe(this, OnDataChanged);
            timerClock.Start();
            timerRefresh.Start();

            // Buồng phòng chỉ làm việc với hàng đợi dọn phòng: mở sẵn cho tiện.
            if (Session.IsHousekeeping) FormManager.Open<frmHousekeeping>();
        }

        /// <summary>Ẩn/hiện menu theo ma trận quyền (tài liệu nghiệp vụ mục I.1).</summary>
        private void ApplyRoleMenus()
        {
            const string A = Roles.Admin, M = Roles.Manager, R = Roles.Receptionist, H = Roles.Housekeeping;

            UiHelper.VisibleFor(mnuCatalog, A, M, R);
            UiHelper.VisibleFor(mnuCustomers, A, M, R);
            UiHelper.VisibleFor(mnuRoomTypes, A, M, R);   // R chỉ xem
            UiHelper.VisibleFor(mnuAmenities, A, M);
            UiHelper.VisibleFor(mnuRooms, A, M, R);       // R chỉ xem
            UiHelper.VisibleFor(mnuServices, A, M, R);    // R chỉ xem
            UiHelper.VisibleFor(mnuCatalogSep, A);
            UiHelper.VisibleFor(mnuEmployees, A);

            UiHelper.VisibleFor(mnuBooking, A, M, R);
            UiHelper.VisibleFor(mnuFrontDesk, A, M, R);
            UiHelper.VisibleFor(mnuBilling, A, M, R);
            UiHelper.VisibleFor(mnuPaymentLedger, A, M);
            UiHelper.VisibleFor(mnuHousekeeping, H, A, M, R);   // R chỉ xem
            UiHelper.VisibleFor(mnuReports, A, M);
        }

        /// <summary>
        /// Hủy phiếu chờ cọc quá 24 giờ (SQL Express không có SQL Agent nên app tự gọi).
        /// Phiếu bị hủy mà đã thu tiền thì nhắc lễ tân xử lý hoàn tiền thủ công.
        /// </summary>
        private void ExpirePendingBookings()
        {
            try
            {
                DataTable expired = DbHelper.Query("dbo.sp_ExpirePendingBookings");
                if (expired.Rows.Count == 0 || Session.IsHousekeeping) return;

                var sb = new StringBuilder("Các phiếu sau vừa bị hủy vì quá 24 giờ chưa đủ cọc nhưng đã có tiền thu.\n" +
                                           "Hãy liên hệ khách và ghi hoàn tiền thủ công (Hóa đơn > Sổ thu):\n\n");
                foreach (DataRow r in expired.Rows)
                {
                    sb.AppendLine("  • Phiếu #" + DbHelper.Value<int>(r, "BookingId") + " – " +
                                  DbHelper.Value<string>(r, "CustomerName") + " (" +
                                  DbHelper.Value<string>(r, "Phone", "") + "): đã thu " +
                                  Formatters.Money(DbHelper.Value<decimal>(r, "PaidAmount")));
                }
                UiHelper.Warn(sb.ToString(), this);
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        // ==================================================================
        // Nạp dashboard + sơ đồ phòng
        // ==================================================================

        private void RefreshAll(bool fromTimer = false)
        {
            try
            {
                LoadDashboard();
                LoadRoomBoard();
            }
            catch (Exception ex)
            {
                if (fromTimer) ErrorHandler.Log(ex);
                else ErrorHandler.Show(ex, this);
            }
        }

        private void LoadDashboard()
        {
            flpDashboard.SuspendLayout();
            ClearControls(flpDashboard);

            if (Session.IsHousekeeping)
            {
                // Buồng phòng không xem dashboard vận hành; chỉ đếm hàng đợi dọn phòng.
                DataTable q = DbHelper.QueryText(
                    "SELECT SUM(CASE WHEN Status = 'Pending' THEN 1 ELSE 0 END) AS PendingTasks, " +
                    "SUM(CASE WHEN Status = 'InProgress' THEN 1 ELSE 0 END) AS InProgressTasks " +
                    "FROM dbo.vw_HousekeepingQueue");
                DataRow h = q.Rows[0];
                AddTile("Việc chờ dọn", DbHelper.Value<int>(h, "PendingTasks").ToString());
                AddTile("Đang dọn", DbHelper.Value<int>(h, "InProgressTasks").ToString());
                flpDashboard.ResumeLayout();
                return;
            }

            DataTable dt = DbHelper.Query("dbo.sp_DashboardSummary", Session.EmployeeParam());
            DataRow r = dt.Rows[0];
            AddTile("Tổng số phòng", DbHelper.Value<int>(r, "TotalRooms").ToString());
            AddTile("Phòng trống", DbHelper.Value<int>(r, "AvailableRooms").ToString(), RoomStatus.ToColor(RoomStatus.Available));
            AddTile("Có khách", DbHelper.Value<int>(r, "OccupiedRooms").ToString(), RoomStatus.ToColor(RoomStatus.Occupied));
            AddTile("Đang dọn", DbHelper.Value<int>(r, "CleaningRooms").ToString(), RoomStatus.ToColor(RoomStatus.Cleaning));
            AddTile("Bảo trì", DbHelper.Value<int>(r, "MaintenanceRooms").ToString(), RoomStatus.ToColor(RoomStatus.Maintenance));
            AddTile("Khách đến hôm nay", DbHelper.Value<int>(r, "ArrivalsToday").ToString());
            AddTile("Dự kiến trả", DbHelper.Value<int>(r, "DeparturesDue").ToString());
            AddTile("Đã nhận hôm nay", DbHelper.Value<int>(r, "CheckedInToday").ToString());
            AddTile("Đã trả hôm nay", DbHelper.Value<int>(r, "CheckedOutToday").ToString());
            AddTile("Phiếu mới hôm nay", DbHelper.Value<int>(r, "NewBookingsToday").ToString());
            AddTile("Công suất", Formatters.Percent(DbHelper.Value<decimal>(r, "OccupancyRate")));

            // Procedure trả NULL cho lễ tân (không được xem doanh thu).
            decimal? revenue = DbHelper.Value<decimal?>(r, "RevenueToday");
            if (revenue.HasValue) AddTile("Thực thu hôm nay", Formatters.Money(revenue.Value));

            flpDashboard.ResumeLayout();
        }

        private void AddTile(string title, string value, Color? back = null)
        {
            var tile = new Panel
            {
                Size = new Size(140, 78),
                Margin = new Padding(4),
                BackColor = back ?? TileBack,
                BorderStyle = BorderStyle.FixedSingle
            };
            var lblValue = new Label
            {
                Text = value,
                Font = new Font(Font.FontFamily, 16F, FontStyle.Bold),
                ForeColor = TileValue,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            var lblTitle = new Label
            {
                Text = title,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                Height = 22
            };
            tile.Controls.Add(lblValue);
            tile.Controls.Add(lblTitle);
            flpDashboard.Controls.Add(tile);
        }

        private void LoadRoomBoard()
        {
            DataTable dt = DbHelper.QueryText(SqlRoomBoard);

            flpRooms.SuspendLayout();
            ClearControls(flpRooms);
            toolTipRoom.RemoveAll();

            int currentFloor = int.MinValue;
            foreach (DataRow r in dt.Rows)
            {
                int floor = DbHelper.Value<int>(r, "Floor");
                if (floor != currentFloor)
                {
                    currentFloor = floor;
                    flpRooms.Controls.Add(new Label
                    {
                        Text = "Tầng " + floor,
                        Font = new Font(Font, FontStyle.Bold),
                        AutoSize = false,
                        Width = FloorHeaderWidth(),
                        Height = 24,
                        TextAlign = ContentAlignment.BottomLeft,
                        Margin = new Padding(4, 8, 4, 2)
                    });
                }
                flpRooms.Controls.Add(CreateRoomTile(r));
            }
            flpRooms.ResumeLayout();
        }

        private Label CreateRoomTile(DataRow r)
        {
            string status = DbHelper.Value<string>(r, "RoomStatus");
            string guest = DbHelper.Value<string>(r, "CurrentGuest");
            string arriving = DbHelper.Value<string>(r, "ArrivingGuest");

            string line3;
            if (guest != null) line3 = guest;
            else if (arriving != null) line3 = "Đến: " + arriving;
            else line3 = RoomStatus.ToVietnamese(status);

            var tile = new Label
            {
                AutoSize = false,
                Size = new Size(124, 72),
                Margin = new Padding(4),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = RoomStatus.ToColor(status),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = DbHelper.Value<string>(r, "RoomNumber") + "\n" +
                       DbHelper.Value<string>(r, "TypeName") + "\n" + line3,
                Tag = r,
                Cursor = Cursors.Hand
            };
            tile.DoubleClick += RoomTile_DoubleClick;

            var tip = new StringBuilder();
            tip.AppendLine("Phòng " + DbHelper.Value<string>(r, "RoomNumber") + " – " + RoomStatus.ToVietnamese(status));
            if (guest != null)
                tip.AppendLine("Đang ở: " + guest + " (trả " + Formatters.Date(r["ExpectedCheckOut"]) + ")");
            if (arriving != null) tip.AppendLine("Khách đến hôm nay: " + arriving);
            string note = DbHelper.Value<string>(r, "Note");
            if (!string.IsNullOrEmpty(note)) tip.AppendLine("Ghi chú: " + note);
            toolTipRoom.SetToolTip(tile, tip.ToString().TrimEnd());
            return tile;
        }

        /// <summary>
        /// Nhấp đúp ô phòng: phòng có khách -> ghi dịch vụ; có khách đến hôm nay -> nhận phòng;
        /// phòng đang dọn -> hàng đợi dọn phòng. Thao tác ngoài quyền thì không làm gì.
        /// </summary>
        private void RoomTile_DoubleClick(object sender, EventArgs e)
        {
            var r = ((Control)sender).Tag as DataRow;
            if (r == null) return;
            bool frontDesk = Session.HasRole(Roles.Admin, Roles.Manager, Roles.Receptionist);

            int? stayId = DbHelper.Value<int?>(r, "CurrentStayId");
            int? arrivalId = DbHelper.Value<int?>(r, "ArrivalStayId");
            string status = DbHelper.Value<string>(r, "RoomStatus");

            if (stayId.HasValue && frontDesk)
            {
                int id = stayId.Value;
                FormManager.Open("stay-" + id, () => new frmServiceUsage(id));
            }
            else if (arrivalId.HasValue && frontDesk && status == RoomStatus.Available)
            {
                FormManager.ShowDialog(new dlgCheckIn(arrivalId.Value), this);
            }
            else if (status == RoomStatus.Cleaning)
            {
                FormManager.Open<frmHousekeeping>();
            }
        }

        private int FloorHeaderWidth()
        {
            return Math.Max(200, flpRooms.ClientSize.Width - flpRooms.Padding.Horizontal - 12);
        }

        private static void ClearControls(Control parent)
        {
            while (parent.Controls.Count > 0)
            {
                Control c = parent.Controls[0];
                parent.Controls.RemoveAt(0);
                c.Dispose();
            }
        }

        // ==================================================================
        // Làm mới
        // ==================================================================

        private void OnDataChanged(string area)
        {
            if (area == DataArea.Catalog) return;   // danh mục không ảnh hưởng sơ đồ/chỉ số
            if (_reloadPending) return;
            _reloadPending = true;
            BeginInvoke(new Action(() =>
            {
                _reloadPending = false;
                if (!IsDisposed) RefreshAll();
            }));
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshAll();
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            RefreshAll(fromTimer: true);
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {
            UpdateClock();
        }

        private void UpdateClock()
        {
            lblStatusClock.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy HH:mm:ss", Formatters.Vi);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (flpRooms == null) return;
            foreach (Control c in flpRooms.Controls)
            {
                if (!(c.Tag is DataRow)) c.Width = FloorHeaderWidth();   // dòng tiêu đề tầng chiếm cả hàng
            }
        }

        // ==================================================================
        // Menu
        // ==================================================================

        private void mnuChangePassword_Click(object sender, EventArgs e)
        {
            FormManager.ShowDialog(new dlgChangePassword(), this);
        }

        private void mnuLogout_Click(object sender, EventArgs e)
        {
            if (!UiHelper.Confirm("Đăng xuất khỏi tài khoản " + Session.Username + "?", owner: this)) return;
            timerRefresh.Stop();
            FormManager.CloseAll();
            Session.Logout();
            LoggedOut = true;
            Close();
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mnuCustomers_Click(object sender, EventArgs e) { FormManager.Open<frmCustomers>(); }
        private void mnuRoomTypes_Click(object sender, EventArgs e) { FormManager.Open<frmRoomTypes>(); }
        private void mnuAmenities_Click(object sender, EventArgs e) { FormManager.Open<frmAmenities>(); }
        private void mnuRooms_Click(object sender, EventArgs e) { FormManager.Open<frmRooms>(); }
        private void mnuServices_Click(object sender, EventArgs e) { FormManager.Open<frmServices>(); }
        private void mnuEmployees_Click(object sender, EventArgs e) { FormManager.Open<frmEmployees>(); }
        private void mnuBookings_Click(object sender, EventArgs e) { FormManager.Open<frmBookings>(); }
        private void mnuNewBooking_Click(object sender, EventArgs e) { FormManager.Open("new", () => new frmBookingEdit(null)); }
        private void mnuFrontDeskScreen_Click(object sender, EventArgs e) { FormManager.Open<frmFrontDesk>(); }
        private void mnuInvoices_Click(object sender, EventArgs e) { FormManager.Open<frmInvoices>(); }
        private void mnuPaymentLedger_Click(object sender, EventArgs e) { FormManager.Open<frmPaymentLedger>(); }
        private void mnuHousekeepingQueue_Click(object sender, EventArgs e) { FormManager.Open<frmHousekeeping>(); }
        private void mnuReportsScreen_Click(object sender, EventArgs e) { FormManager.Open<frmReports>(); }
    }
}
