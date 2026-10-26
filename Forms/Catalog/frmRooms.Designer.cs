namespace Hotel_management_system.Forms.Catalog
{
    partial class frmRooms
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
            this.chkShowInactive = new System.Windows.Forms.CheckBox();
            this.cboStatusFilter = new System.Windows.Forms.ComboBox();
            this.lblStatusFilter = new System.Windows.Forms.Label();
            this.cboTypeFilter = new System.Windows.Forms.ComboBox();
            this.lblTypeFilter = new System.Windows.Forms.Label();
            this.cboFloorFilter = new System.Windows.Forms.ComboBox();
            this.lblFloorFilter = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.grpDetail = new System.Windows.Forms.GroupBox();
            this.btnFinishMaintenance = new System.Windows.Forms.Button();
            this.btnMaintenance = new System.Windows.Forms.Button();
            this.lblStatusValue = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.lblNote = new System.Windows.Forms.Label();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.nudFloor = new System.Windows.Forms.NumericUpDown();
            this.lblFloor = new System.Windows.Forms.Label();
            this.txtRoomNumber = new System.Windows.Forms.TextBox();
            this.lblRoomNumber = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.dgvRooms = new System.Windows.Forms.DataGridView();
            this.pnlTop.SuspendLayout();
            this.grpDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFloor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRooms)).BeginInit();
            this.SuspendLayout();
            //
            // pnlTop
            //
            this.pnlTop.Controls.Add(this.chkShowInactive);
            this.pnlTop.Controls.Add(this.cboStatusFilter);
            this.pnlTop.Controls.Add(this.lblStatusFilter);
            this.pnlTop.Controls.Add(this.cboTypeFilter);
            this.pnlTop.Controls.Add(this.lblTypeFilter);
            this.pnlTop.Controls.Add(this.cboFloorFilter);
            this.pnlTop.Controls.Add(this.lblFloorFilter);
            this.pnlTop.Controls.Add(this.btnRefresh);
            this.pnlTop.Controls.Add(this.txtSearch);
            this.pnlTop.Controls.Add(this.lblSearch);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1084, 44);
            this.pnlTop.TabIndex = 0;
            //
            // chkShowInactive
            //
            this.chkShowInactive.AutoSize = true;
            this.chkShowInactive.Location = new System.Drawing.Point(778, 13);
            this.chkShowInactive.Name = "chkShowInactive";
            this.chkShowInactive.Size = new System.Drawing.Size(150, 19);
            this.chkShowInactive.TabIndex = 8;
            this.chkShowInactive.Text = "Hiện cả phòng ngừng";
            this.chkShowInactive.UseVisualStyleBackColor = true;
            this.chkShowInactive.CheckedChanged += new System.EventHandler(this.filter_Changed);
            //
            // cboStatusFilter
            //
            this.cboStatusFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatusFilter.Location = new System.Drawing.Point(632, 11);
            this.cboStatusFilter.Name = "cboStatusFilter";
            this.cboStatusFilter.Size = new System.Drawing.Size(130, 23);
            this.cboStatusFilter.TabIndex = 7;
            this.cboStatusFilter.SelectedIndexChanged += new System.EventHandler(this.filter_Changed);
            //
            // lblStatusFilter
            //
            this.lblStatusFilter.AutoSize = true;
            this.lblStatusFilter.Location = new System.Drawing.Point(570, 14);
            this.lblStatusFilter.Name = "lblStatusFilter";
            this.lblStatusFilter.Size = new System.Drawing.Size(59, 15);
            this.lblStatusFilter.TabIndex = 6;
            this.lblStatusFilter.Text = "Trạng thái:";
            //
            // cboTypeFilter
            //
            this.cboTypeFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTypeFilter.Location = new System.Drawing.Point(410, 11);
            this.cboTypeFilter.Name = "cboTypeFilter";
            this.cboTypeFilter.Size = new System.Drawing.Size(150, 23);
            this.cboTypeFilter.TabIndex = 5;
            this.cboTypeFilter.SelectedIndexChanged += new System.EventHandler(this.filter_Changed);
            //
            // lblTypeFilter
            //
            this.lblTypeFilter.AutoSize = true;
            this.lblTypeFilter.Location = new System.Drawing.Point(376, 14);
            this.lblTypeFilter.Name = "lblTypeFilter";
            this.lblTypeFilter.Size = new System.Drawing.Size(31, 15);
            this.lblTypeFilter.TabIndex = 4;
            this.lblTypeFilter.Text = "Loại:";
            //
            // cboFloorFilter
            //
            this.cboFloorFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFloorFilter.Location = new System.Drawing.Point(250, 11);
            this.cboFloorFilter.Name = "cboFloorFilter";
            this.cboFloorFilter.Size = new System.Drawing.Size(116, 23);
            this.cboFloorFilter.TabIndex = 3;
            this.cboFloorFilter.SelectedIndexChanged += new System.EventHandler(this.filter_Changed);
            //
            // lblFloorFilter
            //
            this.lblFloorFilter.AutoSize = true;
            this.lblFloorFilter.Location = new System.Drawing.Point(212, 14);
            this.lblFloorFilter.Name = "lblFloorFilter";
            this.lblFloorFilter.Size = new System.Drawing.Size(35, 15);
            this.lblFloorFilter.TabIndex = 2;
            this.lblFloorFilter.Text = "Tầng:";
            //
            // btnRefresh
            //
            this.btnRefresh.Location = new System.Drawing.Point(944, 9);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 27);
            this.btnRefresh.TabIndex = 9;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            //
            // txtSearch
            //
            this.txtSearch.Location = new System.Drawing.Point(80, 11);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(120, 23);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.filter_Changed);
            //
            // lblSearch
            //
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(12, 14);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(64, 15);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Số phòng:";
            //
            // grpDetail
            //
            this.grpDetail.Controls.Add(this.btnFinishMaintenance);
            this.grpDetail.Controls.Add(this.btnMaintenance);
            this.grpDetail.Controls.Add(this.lblStatusValue);
            this.grpDetail.Controls.Add(this.lblStatus);
            this.grpDetail.Controls.Add(this.chkActive);
            this.grpDetail.Controls.Add(this.txtNote);
            this.grpDetail.Controls.Add(this.lblNote);
            this.grpDetail.Controls.Add(this.cboType);
            this.grpDetail.Controls.Add(this.lblType);
            this.grpDetail.Controls.Add(this.nudFloor);
            this.grpDetail.Controls.Add(this.lblFloor);
            this.grpDetail.Controls.Add(this.txtRoomNumber);
            this.grpDetail.Controls.Add(this.lblRoomNumber);
            this.grpDetail.Controls.Add(this.btnCancel);
            this.grpDetail.Controls.Add(this.btnSave);
            this.grpDetail.Controls.Add(this.btnEdit);
            this.grpDetail.Controls.Add(this.btnAdd);
            this.grpDetail.Dock = System.Windows.Forms.DockStyle.Right;
            this.grpDetail.Location = new System.Drawing.Point(724, 44);
            this.grpDetail.Name = "grpDetail";
            this.grpDetail.Size = new System.Drawing.Size(360, 477);
            this.grpDetail.TabIndex = 2;
            this.grpDetail.TabStop = false;
            this.grpDetail.Text = "Thông tin phòng";
            //
            // btnFinishMaintenance
            //
            this.btnFinishMaintenance.Location = new System.Drawing.Point(178, 382);
            this.btnFinishMaintenance.Name = "btnFinishMaintenance";
            this.btnFinishMaintenance.Size = new System.Drawing.Size(154, 30);
            this.btnFinishMaintenance.TabIndex = 16;
            this.btnFinishMaintenance.Text = "Kết thúc bảo trì";
            this.btnFinishMaintenance.UseVisualStyleBackColor = true;
            this.btnFinishMaintenance.Click += new System.EventHandler(this.btnFinishMaintenance_Click);
            //
            // btnMaintenance
            //
            this.btnMaintenance.Location = new System.Drawing.Point(16, 382);
            this.btnMaintenance.Name = "btnMaintenance";
            this.btnMaintenance.Size = new System.Drawing.Size(154, 30);
            this.btnMaintenance.TabIndex = 15;
            this.btnMaintenance.Text = "Đưa vào bảo trì";
            this.btnMaintenance.UseVisualStyleBackColor = true;
            this.btnMaintenance.Click += new System.EventHandler(this.btnMaintenance_Click);
            //
            // lblStatusValue
            //
            this.lblStatusValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatusValue.Location = new System.Drawing.Point(110, 210);
            this.lblStatusValue.Name = "lblStatusValue";
            this.lblStatusValue.Size = new System.Drawing.Size(234, 20);
            this.lblStatusValue.TabIndex = 10;
            //
            // lblStatus
            //
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(14, 212);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(62, 15);
            this.lblStatus.TabIndex = 9;
            this.lblStatus.Text = "Tình trạng:";
            //
            // chkActive
            //
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(110, 182);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(100, 19);
            this.chkActive.TabIndex = 8;
            this.chkActive.Text = "Đang khai thác";
            this.chkActive.UseVisualStyleBackColor = true;
            //
            // txtNote
            //
            this.txtNote.Location = new System.Drawing.Point(110, 136);
            this.txtNote.MaxLength = 200;
            this.txtNote.Multiline = true;
            this.txtNote.Name = "txtNote";
            this.txtNote.Size = new System.Drawing.Size(234, 40);
            this.txtNote.TabIndex = 7;
            //
            // lblNote
            //
            this.lblNote.AutoSize = true;
            this.lblNote.Location = new System.Drawing.Point(14, 139);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(51, 15);
            this.lblNote.TabIndex = 6;
            this.lblNote.Text = "Ghi chú:";
            //
            // cboType
            //
            this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboType.Location = new System.Drawing.Point(110, 100);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(234, 23);
            this.cboType.TabIndex = 5;
            //
            // lblType
            //
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(14, 103);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(73, 15);
            this.lblType.TabIndex = 4;
            this.lblType.Text = "Loại phòng:*";
            //
            // nudFloor
            //
            this.nudFloor.Location = new System.Drawing.Point(110, 65);
            this.nudFloor.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudFloor.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            -2147483648});
            this.nudFloor.Name = "nudFloor";
            this.nudFloor.Size = new System.Drawing.Size(80, 23);
            this.nudFloor.TabIndex = 3;
            this.nudFloor.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            //
            // lblFloor
            //
            this.lblFloor.AutoSize = true;
            this.lblFloor.Location = new System.Drawing.Point(14, 68);
            this.lblFloor.Name = "lblFloor";
            this.lblFloor.Size = new System.Drawing.Size(42, 15);
            this.lblFloor.TabIndex = 2;
            this.lblFloor.Text = "Tầng:*";
            //
            // txtRoomNumber
            //
            this.txtRoomNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtRoomNumber.Location = new System.Drawing.Point(110, 30);
            this.txtRoomNumber.MaxLength = 10;
            this.txtRoomNumber.Name = "txtRoomNumber";
            this.txtRoomNumber.Size = new System.Drawing.Size(120, 23);
            this.txtRoomNumber.TabIndex = 1;
            //
            // lblRoomNumber
            //
            this.lblRoomNumber.AutoSize = true;
            this.lblRoomNumber.Location = new System.Drawing.Point(14, 33);
            this.lblRoomNumber.Name = "lblRoomNumber";
            this.lblRoomNumber.Size = new System.Drawing.Size(69, 15);
            this.lblRoomNumber.TabIndex = 0;
            this.lblRoomNumber.Text = "Số phòng:*";
            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(232, 290);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // btnSave
            //
            this.btnSave.Location = new System.Drawing.Point(124, 290);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // btnEdit
            //
            this.btnEdit.Location = new System.Drawing.Point(124, 250);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(100, 30);
            this.btnEdit.TabIndex = 12;
            this.btnEdit.Text = "Sửa";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            //
            // btnAdd
            //
            this.btnAdd.Location = new System.Drawing.Point(16, 250);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 30);
            this.btnAdd.TabIndex = 11;
            this.btnAdd.Text = "Thêm";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            //
            // dgvRooms
            //
            this.dgvRooms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRooms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRooms.Location = new System.Drawing.Point(0, 44);
            this.dgvRooms.Name = "dgvRooms";
            this.dgvRooms.Size = new System.Drawing.Size(724, 477);
            this.dgvRooms.TabIndex = 1;
            this.dgvRooms.SelectionChanged += new System.EventHandler(this.dgvRooms_SelectionChanged);
            //
            // frmRooms
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 521);
            this.Controls.Add(this.dgvRooms);
            this.Controls.Add(this.grpDetail);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1000, 480);
            this.Name = "frmRooms";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Phòng";
            this.Load += new System.EventHandler(this.frmRooms_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.grpDetail.ResumeLayout(false);
            this.grpDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFloor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRooms)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblFloorFilter;
        private System.Windows.Forms.ComboBox cboFloorFilter;
        private System.Windows.Forms.Label lblTypeFilter;
        private System.Windows.Forms.ComboBox cboTypeFilter;
        private System.Windows.Forms.Label lblStatusFilter;
        private System.Windows.Forms.ComboBox cboStatusFilter;
        private System.Windows.Forms.CheckBox chkShowInactive;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.GroupBox grpDetail;
        private System.Windows.Forms.Label lblRoomNumber;
        private System.Windows.Forms.TextBox txtRoomNumber;
        private System.Windows.Forms.Label lblFloor;
        private System.Windows.Forms.NumericUpDown nudFloor;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cboType;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblStatusValue;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnMaintenance;
        private System.Windows.Forms.Button btnFinishMaintenance;
        private System.Windows.Forms.DataGridView dgvRooms;
    }
}
