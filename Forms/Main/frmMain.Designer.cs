namespace Hotel_management_system.Forms.Main
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.mnuSystem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuChangePassword = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSystemSep = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCatalog = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCustomers = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRoomTypes = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAmenities = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRooms = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuServices = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCatalogSep = new System.Windows.Forms.ToolStripSeparator();
            this.mnuEmployees = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBooking = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBookings = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNewBooking = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFrontDesk = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFrontDeskScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBilling = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInvoices = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPaymentLedger = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHousekeeping = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHousekeepingQueue = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuReports = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuReportsScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.statusMain = new System.Windows.Forms.StatusStrip();
            this.lblStatusUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusRole = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusSpring = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusClock = new System.Windows.Forms.ToolStripStatusLabel();
            this.flpDashboard = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlBoardHeader = new System.Windows.Forms.Panel();
            this.lblLegend = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblBoardTitle = new System.Windows.Forms.Label();
            this.flpRooms = new System.Windows.Forms.FlowLayoutPanel();
            this.timerClock = new System.Windows.Forms.Timer(this.components);
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.toolTipRoom = new System.Windows.Forms.ToolTip(this.components);
            this.menuMain.SuspendLayout();
            this.statusMain.SuspendLayout();
            this.pnlBoardHeader.SuspendLayout();
            this.SuspendLayout();
            //
            // menuMain
            //
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSystem,
            this.mnuCatalog,
            this.mnuBooking,
            this.mnuFrontDesk,
            this.mnuBilling,
            this.mnuHousekeeping,
            this.mnuReports});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(1184, 24);
            this.menuMain.TabIndex = 0;
            //
            // mnuSystem
            //
            this.mnuSystem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuChangePassword,
            this.mnuLogout,
            this.mnuSystemSep,
            this.mnuExit});
            this.mnuSystem.Name = "mnuSystem";
            this.mnuSystem.Size = new System.Drawing.Size(69, 20);
            this.mnuSystem.Text = "&Hệ thống";
            //
            // mnuChangePassword
            //
            this.mnuChangePassword.Name = "mnuChangePassword";
            this.mnuChangePassword.Size = new System.Drawing.Size(180, 22);
            this.mnuChangePassword.Text = "Đổi mật khẩu";
            this.mnuChangePassword.Click += new System.EventHandler(this.mnuChangePassword_Click);
            //
            // mnuLogout
            //
            this.mnuLogout.Name = "mnuLogout";
            this.mnuLogout.Size = new System.Drawing.Size(180, 22);
            this.mnuLogout.Text = "Đăng xuất";
            this.mnuLogout.Click += new System.EventHandler(this.mnuLogout_Click);
            //
            // mnuSystemSep
            //
            this.mnuSystemSep.Name = "mnuSystemSep";
            this.mnuSystemSep.Size = new System.Drawing.Size(177, 6);
            //
            // mnuExit
            //
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(180, 22);
            this.mnuExit.Text = "Thoát";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            //
            // mnuCatalog
            //
            this.mnuCatalog.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCustomers,
            this.mnuRoomTypes,
            this.mnuAmenities,
            this.mnuRooms,
            this.mnuServices,
            this.mnuCatalogSep,
            this.mnuEmployees});
            this.mnuCatalog.Name = "mnuCatalog";
            this.mnuCatalog.Size = new System.Drawing.Size(73, 20);
            this.mnuCatalog.Text = "&Danh mục";
            //
            // mnuCustomers
            //
            this.mnuCustomers.Name = "mnuCustomers";
            this.mnuCustomers.Size = new System.Drawing.Size(180, 22);
            this.mnuCustomers.Text = "Khách hàng";
            this.mnuCustomers.Click += new System.EventHandler(this.mnuCustomers_Click);
            //
            // mnuRoomTypes
            //
            this.mnuRoomTypes.Name = "mnuRoomTypes";
            this.mnuRoomTypes.Size = new System.Drawing.Size(180, 22);
            this.mnuRoomTypes.Text = "Loại phòng";
            this.mnuRoomTypes.Click += new System.EventHandler(this.mnuRoomTypes_Click);
            //
            // mnuAmenities
            //
            this.mnuAmenities.Name = "mnuAmenities";
            this.mnuAmenities.Size = new System.Drawing.Size(180, 22);
            this.mnuAmenities.Text = "Tiện nghi";
            this.mnuAmenities.Click += new System.EventHandler(this.mnuAmenities_Click);
            //
            // mnuRooms
            //
            this.mnuRooms.Name = "mnuRooms";
            this.mnuRooms.Size = new System.Drawing.Size(180, 22);
            this.mnuRooms.Text = "Phòng";
            this.mnuRooms.Click += new System.EventHandler(this.mnuRooms_Click);
            //
            // mnuServices
            //
            this.mnuServices.Name = "mnuServices";
            this.mnuServices.Size = new System.Drawing.Size(180, 22);
            this.mnuServices.Text = "Dịch vụ";
            this.mnuServices.Click += new System.EventHandler(this.mnuServices_Click);
            //
            // mnuCatalogSep
            //
            this.mnuCatalogSep.Name = "mnuCatalogSep";
            this.mnuCatalogSep.Size = new System.Drawing.Size(177, 6);
            //
            // mnuEmployees
            //
            this.mnuEmployees.Name = "mnuEmployees";
            this.mnuEmployees.Size = new System.Drawing.Size(180, 22);
            this.mnuEmployees.Text = "Nhân viên và vai trò";
            this.mnuEmployees.Click += new System.EventHandler(this.mnuEmployees_Click);
            //
            // mnuBooking
            //
            this.mnuBooking.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuBookings,
            this.mnuNewBooking});
            this.mnuBooking.Name = "mnuBooking";
            this.mnuBooking.Size = new System.Drawing.Size(76, 20);
            this.mnuBooking.Text = "Đặt &phòng";
            //
            // mnuBookings
            //
            this.mnuBookings.Name = "mnuBookings";
            this.mnuBookings.Size = new System.Drawing.Size(180, 22);
            this.mnuBookings.Text = "Danh sách phiếu đặt";
            this.mnuBookings.Click += new System.EventHandler(this.mnuBookings_Click);
            //
            // mnuNewBooking
            //
            this.mnuNewBooking.Name = "mnuNewBooking";
            this.mnuNewBooking.Size = new System.Drawing.Size(180, 22);
            this.mnuNewBooking.Text = "Tạo phiếu đặt mới";
            this.mnuNewBooking.Click += new System.EventHandler(this.mnuNewBooking_Click);
            //
            // mnuFrontDesk
            //
            this.mnuFrontDesk.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFrontDeskScreen});
            this.mnuFrontDesk.Name = "mnuFrontDesk";
            this.mnuFrontDesk.Size = new System.Drawing.Size(57, 20);
            this.mnuFrontDesk.Text = "&Lễ tân";
            //
            // mnuFrontDeskScreen
            //
            this.mnuFrontDeskScreen.Name = "mnuFrontDeskScreen";
            this.mnuFrontDeskScreen.Size = new System.Drawing.Size(220, 22);
            this.mnuFrontDeskScreen.Text = "Màn hình lễ tân (nhận/trả phòng)";
            this.mnuFrontDeskScreen.Click += new System.EventHandler(this.mnuFrontDeskScreen_Click);
            //
            // mnuBilling
            //
            this.mnuBilling.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuInvoices,
            this.mnuPaymentLedger});
            this.mnuBilling.Name = "mnuBilling";
            this.mnuBilling.Size = new System.Drawing.Size(66, 20);
            this.mnuBilling.Text = "Hóa đơ&n";
            //
            // mnuInvoices
            //
            this.mnuInvoices.Name = "mnuInvoices";
            this.mnuInvoices.Size = new System.Drawing.Size(180, 22);
            this.mnuInvoices.Text = "Danh sách hóa đơn";
            this.mnuInvoices.Click += new System.EventHandler(this.mnuInvoices_Click);
            //
            // mnuPaymentLedger
            //
            this.mnuPaymentLedger.Name = "mnuPaymentLedger";
            this.mnuPaymentLedger.Size = new System.Drawing.Size(180, 22);
            this.mnuPaymentLedger.Text = "Sổ thu";
            this.mnuPaymentLedger.Click += new System.EventHandler(this.mnuPaymentLedger_Click);
            //
            // mnuHousekeeping
            //
            this.mnuHousekeeping.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHousekeepingQueue});
            this.mnuHousekeeping.Name = "mnuHousekeeping";
            this.mnuHousekeeping.Size = new System.Drawing.Size(77, 20);
            this.mnuHousekeeping.Text = "Dọn phòn&g";
            //
            // mnuHousekeepingQueue
            //
            this.mnuHousekeepingQueue.Name = "mnuHousekeepingQueue";
            this.mnuHousekeepingQueue.Size = new System.Drawing.Size(180, 22);
            this.mnuHousekeepingQueue.Text = "Hàng đợi dọn phòng";
            this.mnuHousekeepingQueue.Click += new System.EventHandler(this.mnuHousekeepingQueue_Click);
            //
            // mnuReports
            //
            this.mnuReports.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuReportsScreen});
            this.mnuReports.Name = "mnuReports";
            this.mnuReports.Size = new System.Drawing.Size(61, 20);
            this.mnuReports.Text = "&Báo cáo";
            //
            // mnuReportsScreen
            //
            this.mnuReportsScreen.Name = "mnuReportsScreen";
            this.mnuReportsScreen.Size = new System.Drawing.Size(180, 22);
            this.mnuReportsScreen.Text = "Báo cáo doanh thu";
            this.mnuReportsScreen.Click += new System.EventHandler(this.mnuReportsScreen_Click);
            //
            // statusMain
            //
            this.statusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatusUser,
            this.lblStatusRole,
            this.lblStatusSpring,
            this.lblStatusClock});
            this.statusMain.Location = new System.Drawing.Point(0, 739);
            this.statusMain.Name = "statusMain";
            this.statusMain.Size = new System.Drawing.Size(1184, 22);
            this.statusMain.TabIndex = 4;
            //
            // lblStatusUser
            //
            this.lblStatusUser.Name = "lblStatusUser";
            this.lblStatusUser.Size = new System.Drawing.Size(65, 17);
            this.lblStatusUser.Text = "Nhân viên:";
            //
            // lblStatusRole
            //
            this.lblStatusRole.Name = "lblStatusRole";
            this.lblStatusRole.Size = new System.Drawing.Size(48, 17);
            this.lblStatusRole.Text = "Vai trò:";
            //
            // lblStatusSpring
            //
            this.lblStatusSpring.Name = "lblStatusSpring";
            this.lblStatusSpring.Size = new System.Drawing.Size(1000, 17);
            this.lblStatusSpring.Spring = true;
            //
            // lblStatusClock
            //
            this.lblStatusClock.Name = "lblStatusClock";
            this.lblStatusClock.Size = new System.Drawing.Size(56, 17);
            this.lblStatusClock.Text = "00:00:00";
            //
            // flpDashboard
            //
            this.flpDashboard.Dock = System.Windows.Forms.DockStyle.Top;
            this.flpDashboard.Location = new System.Drawing.Point(0, 24);
            this.flpDashboard.Name = "flpDashboard";
            this.flpDashboard.Padding = new System.Windows.Forms.Padding(6);
            this.flpDashboard.Size = new System.Drawing.Size(1184, 96);
            this.flpDashboard.TabIndex = 1;
            //
            // pnlBoardHeader
            //
            this.pnlBoardHeader.Controls.Add(this.lblLegend);
            this.pnlBoardHeader.Controls.Add(this.btnRefresh);
            this.pnlBoardHeader.Controls.Add(this.lblBoardTitle);
            this.pnlBoardHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBoardHeader.Location = new System.Drawing.Point(0, 120);
            this.pnlBoardHeader.Name = "pnlBoardHeader";
            this.pnlBoardHeader.Size = new System.Drawing.Size(1184, 36);
            this.pnlBoardHeader.TabIndex = 2;
            //
            // lblLegend
            //
            this.lblLegend.AutoSize = true;
            this.lblLegend.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblLegend.Location = new System.Drawing.Point(140, 11);
            this.lblLegend.Name = "lblLegend";
            this.lblLegend.Size = new System.Drawing.Size(470, 15);
            this.lblLegend.TabIndex = 1;
            this.lblLegend.Text = "Xanh: trống   ·   Đỏ: có khách   ·   Vàng: đang dọn   ·   Xám: bảo trì   ·   Nhấp đúp để thao tác";
            //
            // btnRefresh
            //
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(1082, 5);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 27);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            //
            // lblBoardTitle
            //
            this.lblBoardTitle.AutoSize = true;
            this.lblBoardTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblBoardTitle.Location = new System.Drawing.Point(10, 8);
            this.lblBoardTitle.Name = "lblBoardTitle";
            this.lblBoardTitle.Size = new System.Drawing.Size(107, 20);
            this.lblBoardTitle.TabIndex = 0;
            this.lblBoardTitle.Text = "Sơ đồ phòng";
            //
            // flpRooms
            //
            this.flpRooms.AutoScroll = true;
            this.flpRooms.BackColor = System.Drawing.SystemColors.Window;
            this.flpRooms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpRooms.Location = new System.Drawing.Point(0, 156);
            this.flpRooms.Name = "flpRooms";
            this.flpRooms.Padding = new System.Windows.Forms.Padding(8);
            this.flpRooms.Size = new System.Drawing.Size(1184, 583);
            this.flpRooms.TabIndex = 3;
            //
            // timerClock
            //
            this.timerClock.Interval = 1000;
            this.timerClock.Tick += new System.EventHandler(this.timerClock_Tick);
            //
            // timerRefresh
            //
            this.timerRefresh.Interval = 60000;
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            //
            // frmMain
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 761);
            this.Controls.Add(this.flpRooms);
            this.Controls.Add(this.pnlBoardHeader);
            this.Controls.Add(this.flpDashboard);
            this.Controls.Add(this.statusMain);
            this.Controls.Add(this.menuMain);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MainMenuStrip = this.menuMain;
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý khách sạn";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.statusMain.ResumeLayout(false);
            this.statusMain.PerformLayout();
            this.pnlBoardHeader.ResumeLayout(false);
            this.pnlBoardHeader.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuSystem;
        private System.Windows.Forms.ToolStripMenuItem mnuChangePassword;
        private System.Windows.Forms.ToolStripMenuItem mnuLogout;
        private System.Windows.Forms.ToolStripSeparator mnuSystemSep;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStripMenuItem mnuCatalog;
        private System.Windows.Forms.ToolStripMenuItem mnuCustomers;
        private System.Windows.Forms.ToolStripMenuItem mnuRoomTypes;
        private System.Windows.Forms.ToolStripMenuItem mnuAmenities;
        private System.Windows.Forms.ToolStripMenuItem mnuRooms;
        private System.Windows.Forms.ToolStripMenuItem mnuServices;
        private System.Windows.Forms.ToolStripSeparator mnuCatalogSep;
        private System.Windows.Forms.ToolStripMenuItem mnuEmployees;
        private System.Windows.Forms.ToolStripMenuItem mnuBooking;
        private System.Windows.Forms.ToolStripMenuItem mnuBookings;
        private System.Windows.Forms.ToolStripMenuItem mnuNewBooking;
        private System.Windows.Forms.ToolStripMenuItem mnuFrontDesk;
        private System.Windows.Forms.ToolStripMenuItem mnuFrontDeskScreen;
        private System.Windows.Forms.ToolStripMenuItem mnuBilling;
        private System.Windows.Forms.ToolStripMenuItem mnuInvoices;
        private System.Windows.Forms.ToolStripMenuItem mnuPaymentLedger;
        private System.Windows.Forms.ToolStripMenuItem mnuHousekeeping;
        private System.Windows.Forms.ToolStripMenuItem mnuHousekeepingQueue;
        private System.Windows.Forms.ToolStripMenuItem mnuReports;
        private System.Windows.Forms.ToolStripMenuItem mnuReportsScreen;
        private System.Windows.Forms.StatusStrip statusMain;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusUser;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusRole;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusSpring;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusClock;
        private System.Windows.Forms.FlowLayoutPanel flpDashboard;
        private System.Windows.Forms.Panel pnlBoardHeader;
        private System.Windows.Forms.Label lblBoardTitle;
        private System.Windows.Forms.Label lblLegend;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.FlowLayoutPanel flpRooms;
        private System.Windows.Forms.Timer timerClock;
        private System.Windows.Forms.Timer timerRefresh;
        private System.Windows.Forms.ToolTip toolTipRoom;
    }
}
