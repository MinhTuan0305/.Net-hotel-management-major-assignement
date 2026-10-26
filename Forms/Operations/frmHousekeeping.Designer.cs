namespace Hotel_management_system.Forms.Operations
{
    partial class frmHousekeeping
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
            this.chkMine = new System.Windows.Forms.CheckBox();
            this.cboView = new System.Windows.Forms.ComboBox();
            this.lblView = new System.Windows.Forms.Label();
            this.pnlActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnClean = new System.Windows.Forms.Button();
            this.btnNeedMaintenance = new System.Windows.Forms.Button();
            this.lblReadOnly = new System.Windows.Forms.Label();
            this.dgvTasks = new System.Windows.Forms.DataGridView();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.pnlTop.SuspendLayout();
            this.pnlActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTasks)).BeginInit();
            this.SuspendLayout();
            //
            // pnlTop
            //
            this.pnlTop.Controls.Add(this.lblUpdated);
            this.pnlTop.Controls.Add(this.btnRefresh);
            this.pnlTop.Controls.Add(this.chkMine);
            this.pnlTop.Controls.Add(this.cboView);
            this.pnlTop.Controls.Add(this.lblView);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1004, 44);
            this.pnlTop.TabIndex = 0;
            //
            // lblUpdated
            //
            this.lblUpdated.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUpdated.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblUpdated.Location = new System.Drawing.Point(732, 13);
            this.lblUpdated.Name = "lblUpdated";
            this.lblUpdated.Size = new System.Drawing.Size(260, 18);
            this.lblUpdated.TabIndex = 4;
            this.lblUpdated.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // btnRefresh
            //
            this.btnRefresh.Location = new System.Drawing.Point(560, 9);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 27);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            //
            // chkMine
            //
            this.chkMine.AutoSize = true;
            this.chkMine.Location = new System.Drawing.Point(320, 13);
            this.chkMine.Name = "chkMine";
            this.chkMine.Size = new System.Drawing.Size(225, 19);
            this.chkMine.TabIndex = 2;
            this.chkMine.Text = "Chỉ việc của tôi và việc chưa ai nhận";
            this.chkMine.UseVisualStyleBackColor = true;
            this.chkMine.CheckedChanged += new System.EventHandler(this.filter_Changed);
            //
            // cboView
            //
            this.cboView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboView.Location = new System.Drawing.Point(66, 11);
            this.cboView.Name = "cboView";
            this.cboView.Size = new System.Drawing.Size(240, 23);
            this.cboView.TabIndex = 1;
            this.cboView.SelectedIndexChanged += new System.EventHandler(this.filter_Changed);
            //
            // lblView
            //
            this.lblView.AutoSize = true;
            this.lblView.Location = new System.Drawing.Point(12, 14);
            this.lblView.Name = "lblView";
            this.lblView.Size = new System.Drawing.Size(51, 15);
            this.lblView.TabIndex = 0;
            this.lblView.Text = "Hiển thị:";
            //
            // pnlActions
            //
            this.pnlActions.Controls.Add(this.btnStart);
            this.pnlActions.Controls.Add(this.btnClean);
            this.pnlActions.Controls.Add(this.btnNeedMaintenance);
            this.pnlActions.Controls.Add(this.lblReadOnly);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlActions.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlActions.Location = new System.Drawing.Point(834, 44);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Padding = new System.Windows.Forms.Padding(8, 12, 8, 8);
            this.pnlActions.Size = new System.Drawing.Size(170, 517);
            this.pnlActions.TabIndex = 2;
            this.pnlActions.WrapContents = false;
            //
            // btnStart
            //
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStart.Location = new System.Drawing.Point(8, 12);
            this.btnStart.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(152, 38);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Nhận việc";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            //
            // btnClean
            //
            this.btnClean.Location = new System.Drawing.Point(8, 56);
            this.btnClean.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnClean.Name = "btnClean";
            this.btnClean.Size = new System.Drawing.Size(152, 38);
            this.btnClean.TabIndex = 1;
            this.btnClean.Text = "Hoàn tất – Sạch";
            this.btnClean.UseVisualStyleBackColor = true;
            this.btnClean.Click += new System.EventHandler(this.btnClean_Click);
            //
            // btnNeedMaintenance
            //
            this.btnNeedMaintenance.Location = new System.Drawing.Point(8, 100);
            this.btnNeedMaintenance.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnNeedMaintenance.Name = "btnNeedMaintenance";
            this.btnNeedMaintenance.Size = new System.Drawing.Size(152, 38);
            this.btnNeedMaintenance.TabIndex = 2;
            this.btnNeedMaintenance.Text = "Báo cần bảo trì...";
            this.btnNeedMaintenance.UseVisualStyleBackColor = true;
            this.btnNeedMaintenance.Click += new System.EventHandler(this.btnNeedMaintenance_Click);
            //
            // lblReadOnly
            //
            this.lblReadOnly.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblReadOnly.Location = new System.Drawing.Point(8, 150);
            this.lblReadOnly.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.lblReadOnly.Name = "lblReadOnly";
            this.lblReadOnly.Size = new System.Drawing.Size(152, 80);
            this.lblReadOnly.TabIndex = 3;
            this.lblReadOnly.Text = "Lễ tân chỉ xem hàng đợi để biết phòng nào sắp dọn xong.";
            this.lblReadOnly.Visible = false;
            //
            // dgvTasks
            //
            this.dgvTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTasks.Location = new System.Drawing.Point(0, 44);
            this.dgvTasks.Name = "dgvTasks";
            this.dgvTasks.Size = new System.Drawing.Size(834, 517);
            this.dgvTasks.TabIndex = 1;
            this.dgvTasks.SelectionChanged += new System.EventHandler(this.dgvTasks_SelectionChanged);
            //
            // timerRefresh
            //
            this.timerRefresh.Interval = 60000;
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            //
            // frmHousekeeping
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 561);
            this.Controls.Add(this.dgvTasks);
            this.Controls.Add(this.pnlActions);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(900, 450);
            this.Name = "frmHousekeeping";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dọn phòng";
            this.Load += new System.EventHandler(this.frmHousekeeping_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTasks)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblView;
        private System.Windows.Forms.ComboBox cboView;
        private System.Windows.Forms.CheckBox chkMine;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblUpdated;
        private System.Windows.Forms.FlowLayoutPanel pnlActions;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnClean;
        private System.Windows.Forms.Button btnNeedMaintenance;
        private System.Windows.Forms.Label lblReadOnly;
        private System.Windows.Forms.DataGridView dgvTasks;
        private System.Windows.Forms.Timer timerRefresh;
    }
}
