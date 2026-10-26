namespace Hotel_management_system.Forms.Booking
{
    partial class frmBookings
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.chkDateFilter = new System.Windows.Forms.CheckBox();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.pnlActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDeposit = new System.Windows.Forms.Button();
            this.btnCancelRoom = new System.Windows.Forms.Button();
            this.btnCancelBooking = new System.Windows.Forms.Button();
            this.dgvBookings = new System.Windows.Forms.DataGridView();
            this.pnlRooms = new System.Windows.Forms.Panel();
            this.dgvRooms = new System.Windows.Forms.DataGridView();
            this.lblRooms = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            this.pnlActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBookings)).BeginInit();
            this.pnlRooms.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRooms)).BeginInit();
            this.SuspendLayout();
            //
            // pnlTop
            //
            this.pnlTop.Controls.Add(this.btnRefresh);
            this.pnlTop.Controls.Add(this.dtpTo);
            this.pnlTop.Controls.Add(this.lblTo);
            this.pnlTop.Controls.Add(this.dtpFrom);
            this.pnlTop.Controls.Add(this.chkDateFilter);
            this.pnlTop.Controls.Add(this.cboStatus);
            this.pnlTop.Controls.Add(this.lblStatus);
            this.pnlTop.Controls.Add(this.txtSearch);
            this.pnlTop.Controls.Add(this.lblSearch);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1184, 44);
            this.pnlTop.TabIndex = 0;
            //
            // btnRefresh
            //
            this.btnRefresh.Location = new System.Drawing.Point(1000, 9);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 27);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            //
            // dtpTo
            //
            this.dtpTo.CustomFormat = "dd/MM/yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(878, 11);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(106, 23);
            this.dtpTo.TabIndex = 7;
            this.dtpTo.ValueChanged += new System.EventHandler(this.filter_Changed);
            //
            // lblTo
            //
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(850, 14);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(27, 15);
            this.lblTo.TabIndex = 6;
            this.lblTo.Text = "đến";
            //
            // dtpFrom
            //
            this.dtpFrom.CustomFormat = "dd/MM/yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(738, 11);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(106, 23);
            this.dtpFrom.TabIndex = 5;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.filter_Changed);
            //
            // chkDateFilter
            //
            this.chkDateFilter.AutoSize = true;
            this.chkDateFilter.Location = new System.Drawing.Point(620, 13);
            this.chkDateFilter.Name = "chkDateFilter";
            this.chkDateFilter.Size = new System.Drawing.Size(113, 19);
            this.chkDateFilter.TabIndex = 4;
            this.chkDateFilter.Text = "Ngày nhận từ";
            this.chkDateFilter.UseVisualStyleBackColor = true;
            this.chkDateFilter.CheckedChanged += new System.EventHandler(this.filter_Changed);
            //
            // cboStatus
            //
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.Location = new System.Drawing.Point(456, 11);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(150, 23);
            this.cboStatus.TabIndex = 3;
            this.cboStatus.SelectedIndexChanged += new System.EventHandler(this.filter_Changed);
            //
            // lblStatus
            //
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(392, 14);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(59, 15);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Trạng thái:";
            //
            // txtSearch
            //
            this.txtSearch.Location = new System.Drawing.Point(176, 11);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(200, 23);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.filter_Changed);
            //
            // lblSearch
            //
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(12, 14);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(160, 15);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Tìm (mã phiếu, tên, SĐT):";
            //
            // pnlActions
            //
            this.pnlActions.Controls.Add(this.btnNew);
            this.pnlActions.Controls.Add(this.btnEdit);
            this.pnlActions.Controls.Add(this.btnDeposit);
            this.pnlActions.Controls.Add(this.btnCancelRoom);
            this.pnlActions.Controls.Add(this.btnCancelBooking);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlActions.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlActions.Location = new System.Drawing.Point(1024, 44);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Padding = new System.Windows.Forms.Padding(8, 12, 8, 8);
            this.pnlActions.Size = new System.Drawing.Size(160, 597);
            this.pnlActions.TabIndex = 3;
            this.pnlActions.WrapContents = false;
            //
            // btnNew
            //
            this.btnNew.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnNew.Location = new System.Drawing.Point(8, 12);
            this.btnNew.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(142, 34);
            this.btnNew.TabIndex = 0;
            this.btnNew.Text = "Tạo phiếu mới";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            //
            // btnEdit
            //
            this.btnEdit.Location = new System.Drawing.Point(8, 52);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(142, 34);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "Xem / sửa phiếu";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            //
            // btnDeposit
            //
            this.btnDeposit.Location = new System.Drawing.Point(8, 92);
            this.btnDeposit.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnDeposit.Name = "btnDeposit";
            this.btnDeposit.Size = new System.Drawing.Size(142, 34);
            this.btnDeposit.TabIndex = 2;
            this.btnDeposit.Text = "Thu cọc";
            this.btnDeposit.UseVisualStyleBackColor = true;
            this.btnDeposit.Click += new System.EventHandler(this.btnDeposit_Click);
            //
            // btnCancelRoom
            //
            this.btnCancelRoom.Location = new System.Drawing.Point(8, 132);
            this.btnCancelRoom.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnCancelRoom.Name = "btnCancelRoom";
            this.btnCancelRoom.Size = new System.Drawing.Size(142, 34);
            this.btnCancelRoom.TabIndex = 3;
            this.btnCancelRoom.Text = "Hủy phòng / No-show";
            this.btnCancelRoom.UseVisualStyleBackColor = true;
            this.btnCancelRoom.Click += new System.EventHandler(this.btnCancelRoom_Click);
            //
            // btnCancelBooking
            //
            this.btnCancelBooking.Location = new System.Drawing.Point(8, 172);
            this.btnCancelBooking.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnCancelBooking.Name = "btnCancelBooking";
            this.btnCancelBooking.Size = new System.Drawing.Size(142, 34);
            this.btnCancelBooking.TabIndex = 4;
            this.btnCancelBooking.Text = "Hủy cả phiếu";
            this.btnCancelBooking.UseVisualStyleBackColor = true;
            this.btnCancelBooking.Click += new System.EventHandler(this.btnCancelBooking_Click);
            //
            // dgvBookings
            //
            this.dgvBookings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBookings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBookings.Location = new System.Drawing.Point(0, 44);
            this.dgvBookings.Name = "dgvBookings";
            this.dgvBookings.Size = new System.Drawing.Size(1024, 367);
            this.dgvBookings.TabIndex = 1;
            this.dgvBookings.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBookings_CellDoubleClick);
            this.dgvBookings.SelectionChanged += new System.EventHandler(this.dgvBookings_SelectionChanged);
            //
            // pnlRooms
            //
            this.pnlRooms.Controls.Add(this.dgvRooms);
            this.pnlRooms.Controls.Add(this.lblRooms);
            this.pnlRooms.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlRooms.Location = new System.Drawing.Point(0, 411);
            this.pnlRooms.Name = "pnlRooms";
            this.pnlRooms.Size = new System.Drawing.Size(1024, 230);
            this.pnlRooms.TabIndex = 2;
            //
            // dgvRooms
            //
            this.dgvRooms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRooms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRooms.Location = new System.Drawing.Point(0, 24);
            this.dgvRooms.Name = "dgvRooms";
            this.dgvRooms.Size = new System.Drawing.Size(1024, 206);
            this.dgvRooms.TabIndex = 1;
            this.dgvRooms.SelectionChanged += new System.EventHandler(this.dgvRooms_SelectionChanged);
            //
            // lblRooms
            //
            this.lblRooms.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRooms.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblRooms.Location = new System.Drawing.Point(0, 0);
            this.lblRooms.Name = "lblRooms";
            this.lblRooms.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.lblRooms.Size = new System.Drawing.Size(1024, 24);
            this.lblRooms.TabIndex = 0;
            this.lblRooms.Text = "Các phòng của phiếu";
            this.lblRooms.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // frmBookings
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 641);
            this.Controls.Add(this.dgvBookings);
            this.Controls.Add(this.pnlRooms);
            this.Controls.Add(this.pnlActions);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1120, 560);
            this.Name = "frmBookings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Phiếu đặt phòng";
            this.Load += new System.EventHandler(this.frmBookings_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBookings)).EndInit();
            this.pnlRooms.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRooms)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.CheckBox chkDateFilter;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.FlowLayoutPanel pnlActions;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDeposit;
        private System.Windows.Forms.Button btnCancelRoom;
        private System.Windows.Forms.Button btnCancelBooking;
        private System.Windows.Forms.DataGridView dgvBookings;
        private System.Windows.Forms.Panel pnlRooms;
        private System.Windows.Forms.Label lblRooms;
        private System.Windows.Forms.DataGridView dgvRooms;
    }
}
