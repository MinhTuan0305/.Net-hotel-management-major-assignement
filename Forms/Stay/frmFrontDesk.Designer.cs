namespace Hotel_management_system.Forms.Stay
{
    partial class frmFrontDesk
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblUpdated = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.pnlActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCheckIn = new System.Windows.Forms.Button();
            this.btnChangeRoom = new System.Windows.Forms.Button();
            this.btnDeposit = new System.Windows.Forms.Button();
            this.btnCancelBooking = new System.Windows.Forms.Button();
            this.btnServices = new System.Windows.Forms.Button();
            this.btnPayment = new System.Windows.Forms.Button();
            this.btnExtend = new System.Windows.Forms.Button();
            this.btnCheckOut = new System.Windows.Forms.Button();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabArrivals = new System.Windows.Forms.TabPage();
            this.dgvArrivals = new System.Windows.Forms.DataGridView();
            this.tabInHouse = new System.Windows.Forms.TabPage();
            this.dgvInHouse = new System.Windows.Forms.DataGridView();
            this.tabDepartures = new System.Windows.Forms.TabPage();
            this.dgvDepartures = new System.Windows.Forms.DataGridView();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.pnlTop.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabArrivals.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvArrivals)).BeginInit();
            this.tabInHouse.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInHouse)).BeginInit();
            this.tabDepartures.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDepartures)).BeginInit();
            this.SuspendLayout();
            //
            // pnlTop
            //
            this.pnlTop.Controls.Add(this.lblUpdated);
            this.pnlTop.Controls.Add(this.btnRefresh);
            this.pnlTop.Controls.Add(this.txtSearch);
            this.pnlTop.Controls.Add(this.lblSearch);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1084, 44);
            this.pnlTop.TabIndex = 0;
            //
            // lblUpdated
            //
            this.lblUpdated.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUpdated.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblUpdated.Location = new System.Drawing.Point(772, 12);
            this.lblUpdated.Name = "lblUpdated";
            this.lblUpdated.Size = new System.Drawing.Size(300, 20);
            this.lblUpdated.TabIndex = 3;
            this.lblUpdated.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // btnRefresh
            //
            this.btnRefresh.Location = new System.Drawing.Point(522, 9);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 27);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            //
            // txtSearch
            //
            this.txtSearch.Location = new System.Drawing.Point(246, 11);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(260, 23);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            //
            // lblSearch
            //
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(12, 14);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(219, 15);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Tìm (phòng, khách, SĐT, mã phiếu):";
            //
            // pnlActions
            //
            this.pnlActions.Controls.Add(this.btnCheckIn);
            this.pnlActions.Controls.Add(this.btnChangeRoom);
            this.pnlActions.Controls.Add(this.btnDeposit);
            this.pnlActions.Controls.Add(this.btnCancelBooking);
            this.pnlActions.Controls.Add(this.btnServices);
            this.pnlActions.Controls.Add(this.btnPayment);
            this.pnlActions.Controls.Add(this.btnExtend);
            this.pnlActions.Controls.Add(this.btnCheckOut);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlActions.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlActions.Location = new System.Drawing.Point(934, 44);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Padding = new System.Windows.Forms.Padding(8, 24, 8, 8);
            this.pnlActions.Size = new System.Drawing.Size(150, 557);
            this.pnlActions.TabIndex = 2;
            this.pnlActions.WrapContents = false;
            //
            // btnCheckIn
            //
            this.btnCheckIn.Location = new System.Drawing.Point(8, 24);
            this.btnCheckIn.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnCheckIn.Name = "btnCheckIn";
            this.btnCheckIn.Size = new System.Drawing.Size(132, 34);
            this.btnCheckIn.TabIndex = 0;
            this.btnCheckIn.Text = "Check-in";
            this.btnCheckIn.UseVisualStyleBackColor = true;
            this.btnCheckIn.Click += new System.EventHandler(this.btnCheckIn_Click);
            //
            // btnChangeRoom
            //
            this.btnChangeRoom.Location = new System.Drawing.Point(8, 64);
            this.btnChangeRoom.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnChangeRoom.Name = "btnChangeRoom";
            this.btnChangeRoom.Size = new System.Drawing.Size(132, 34);
            this.btnChangeRoom.TabIndex = 1;
            this.btnChangeRoom.Text = "Đổi phòng";
            this.btnChangeRoom.UseVisualStyleBackColor = true;
            this.btnChangeRoom.Click += new System.EventHandler(this.btnChangeRoom_Click);
            //
            // btnDeposit
            //
            this.btnDeposit.Location = new System.Drawing.Point(8, 104);
            this.btnDeposit.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnDeposit.Name = "btnDeposit";
            this.btnDeposit.Size = new System.Drawing.Size(132, 34);
            this.btnDeposit.TabIndex = 2;
            this.btnDeposit.Text = "Thu cọc";
            this.btnDeposit.UseVisualStyleBackColor = true;
            this.btnDeposit.Click += new System.EventHandler(this.btnDeposit_Click);
            //
            // btnCancelBooking
            //
            this.btnCancelBooking.Location = new System.Drawing.Point(8, 144);
            this.btnCancelBooking.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnCancelBooking.Name = "btnCancelBooking";
            this.btnCancelBooking.Size = new System.Drawing.Size(132, 34);
            this.btnCancelBooking.TabIndex = 3;
            this.btnCancelBooking.Text = "Hủy / No-show";
            this.btnCancelBooking.UseVisualStyleBackColor = true;
            this.btnCancelBooking.Click += new System.EventHandler(this.btnCancelBooking_Click);
            //
            // btnServices
            //
            this.btnServices.Location = new System.Drawing.Point(8, 184);
            this.btnServices.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnServices.Name = "btnServices";
            this.btnServices.Size = new System.Drawing.Size(132, 34);
            this.btnServices.TabIndex = 4;
            this.btnServices.Text = "Dịch vụ";
            this.btnServices.UseVisualStyleBackColor = true;
            this.btnServices.Click += new System.EventHandler(this.btnServices_Click);
            //
            // btnPayment
            //
            this.btnPayment.Location = new System.Drawing.Point(8, 224);
            this.btnPayment.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnPayment.Name = "btnPayment";
            this.btnPayment.Size = new System.Drawing.Size(132, 34);
            this.btnPayment.TabIndex = 5;
            this.btnPayment.Text = "Thu tiền";
            this.btnPayment.UseVisualStyleBackColor = true;
            this.btnPayment.Click += new System.EventHandler(this.btnPayment_Click);
            //
            // btnExtend
            //
            this.btnExtend.Location = new System.Drawing.Point(8, 264);
            this.btnExtend.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnExtend.Name = "btnExtend";
            this.btnExtend.Size = new System.Drawing.Size(132, 34);
            this.btnExtend.TabIndex = 6;
            this.btnExtend.Text = "Gia hạn";
            this.btnExtend.UseVisualStyleBackColor = true;
            this.btnExtend.Click += new System.EventHandler(this.btnExtend_Click);
            //
            // btnCheckOut
            //
            this.btnCheckOut.Location = new System.Drawing.Point(8, 304);
            this.btnCheckOut.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnCheckOut.Name = "btnCheckOut";
            this.btnCheckOut.Size = new System.Drawing.Size(132, 34);
            this.btnCheckOut.TabIndex = 7;
            this.btnCheckOut.Text = "Check-out";
            this.btnCheckOut.UseVisualStyleBackColor = true;
            this.btnCheckOut.Click += new System.EventHandler(this.btnCheckOut_Click);
            //
            // tabMain
            //
            this.tabMain.Controls.Add(this.tabArrivals);
            this.tabMain.Controls.Add(this.tabInHouse);
            this.tabMain.Controls.Add(this.tabDepartures);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 44);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(934, 557);
            this.tabMain.TabIndex = 1;
            this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
            //
            // tabArrivals
            //
            this.tabArrivals.Controls.Add(this.dgvArrivals);
            this.tabArrivals.Location = new System.Drawing.Point(4, 24);
            this.tabArrivals.Name = "tabArrivals";
            this.tabArrivals.Padding = new System.Windows.Forms.Padding(3);
            this.tabArrivals.Size = new System.Drawing.Size(926, 529);
            this.tabArrivals.TabIndex = 0;
            this.tabArrivals.Text = "Khách đến hôm nay";
            this.tabArrivals.UseVisualStyleBackColor = true;
            //
            // dgvArrivals
            //
            this.dgvArrivals.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvArrivals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvArrivals.Location = new System.Drawing.Point(3, 3);
            this.dgvArrivals.Name = "dgvArrivals";
            this.dgvArrivals.Size = new System.Drawing.Size(920, 523);
            this.dgvArrivals.TabIndex = 0;
            this.dgvArrivals.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellDoubleClick);
            this.dgvArrivals.SelectionChanged += new System.EventHandler(this.grid_SelectionChanged);
            //
            // tabInHouse
            //
            this.tabInHouse.Controls.Add(this.dgvInHouse);
            this.tabInHouse.Location = new System.Drawing.Point(4, 24);
            this.tabInHouse.Name = "tabInHouse";
            this.tabInHouse.Padding = new System.Windows.Forms.Padding(3);
            this.tabInHouse.Size = new System.Drawing.Size(926, 529);
            this.tabInHouse.TabIndex = 1;
            this.tabInHouse.Text = "Đang ở";
            this.tabInHouse.UseVisualStyleBackColor = true;
            //
            // dgvInHouse
            //
            this.dgvInHouse.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInHouse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInHouse.Location = new System.Drawing.Point(3, 3);
            this.dgvInHouse.Name = "dgvInHouse";
            this.dgvInHouse.Size = new System.Drawing.Size(920, 523);
            this.dgvInHouse.TabIndex = 0;
            this.dgvInHouse.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellDoubleClick);
            this.dgvInHouse.SelectionChanged += new System.EventHandler(this.grid_SelectionChanged);
            //
            // tabDepartures
            //
            this.tabDepartures.Controls.Add(this.dgvDepartures);
            this.tabDepartures.Location = new System.Drawing.Point(4, 24);
            this.tabDepartures.Name = "tabDepartures";
            this.tabDepartures.Padding = new System.Windows.Forms.Padding(3);
            this.tabDepartures.Size = new System.Drawing.Size(926, 529);
            this.tabDepartures.TabIndex = 2;
            this.tabDepartures.Text = "Dự kiến trả hôm nay";
            this.tabDepartures.UseVisualStyleBackColor = true;
            //
            // dgvDepartures
            //
            this.dgvDepartures.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDepartures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDepartures.Location = new System.Drawing.Point(3, 3);
            this.dgvDepartures.Name = "dgvDepartures";
            this.dgvDepartures.Size = new System.Drawing.Size(920, 523);
            this.dgvDepartures.TabIndex = 0;
            this.dgvDepartures.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellDoubleClick);
            this.dgvDepartures.SelectionChanged += new System.EventHandler(this.grid_SelectionChanged);
            //
            // timerRefresh
            //
            this.timerRefresh.Interval = 60000;
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            //
            // frmFrontDesk
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 601);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.pnlActions);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(900, 500);
            this.Name = "frmFrontDesk";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lễ tân";
            this.Load += new System.EventHandler(this.frmFrontDesk_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlActions.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.tabArrivals.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvArrivals)).EndInit();
            this.tabInHouse.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInHouse)).EndInit();
            this.tabDepartures.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDepartures)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblUpdated;
        private System.Windows.Forms.FlowLayoutPanel pnlActions;
        private System.Windows.Forms.Button btnCheckIn;
        private System.Windows.Forms.Button btnChangeRoom;
        private System.Windows.Forms.Button btnDeposit;
        private System.Windows.Forms.Button btnCancelBooking;
        private System.Windows.Forms.Button btnServices;
        private System.Windows.Forms.Button btnPayment;
        private System.Windows.Forms.Button btnExtend;
        private System.Windows.Forms.Button btnCheckOut;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabArrivals;
        private System.Windows.Forms.DataGridView dgvArrivals;
        private System.Windows.Forms.TabPage tabInHouse;
        private System.Windows.Forms.DataGridView dgvInHouse;
        private System.Windows.Forms.TabPage tabDepartures;
        private System.Windows.Forms.DataGridView dgvDepartures;
        private System.Windows.Forms.Timer timerRefresh;
    }
}
