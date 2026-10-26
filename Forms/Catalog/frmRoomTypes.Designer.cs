namespace Hotel_management_system.Forms.Catalog
{
    partial class frmRoomTypes
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
            this.btnRefresh = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.grpDetail = new System.Windows.Forms.GroupBox();
            this.lblPriceNote = new System.Windows.Forms.Label();
            this.clbAmenities = new System.Windows.Forms.CheckedListBox();
            this.lblAmenities = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtBedType = new System.Windows.Forms.TextBox();
            this.lblBedType = new System.Windows.Forms.Label();
            this.nudMaxOccupancy = new System.Windows.Forms.NumericUpDown();
            this.lblMaxOccupancy = new System.Windows.Forms.Label();
            this.nudBasePrice = new System.Windows.Forms.NumericUpDown();
            this.lblBasePrice = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnToggleActive = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.dgvRoomTypes = new System.Windows.Forms.DataGridView();
            this.pnlTop.SuspendLayout();
            this.grpDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxOccupancy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBasePrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoomTypes)).BeginInit();
            this.SuspendLayout();
            //
            // pnlTop
            //
            this.pnlTop.Controls.Add(this.chkShowInactive);
            this.pnlTop.Controls.Add(this.btnRefresh);
            this.pnlTop.Controls.Add(this.txtSearch);
            this.pnlTop.Controls.Add(this.lblSearch);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1004, 44);
            this.pnlTop.TabIndex = 0;
            //
            // chkShowInactive
            //
            this.chkShowInactive.AutoSize = true;
            this.chkShowInactive.Location = new System.Drawing.Point(290, 13);
            this.chkShowInactive.Name = "chkShowInactive";
            this.chkShowInactive.Size = new System.Drawing.Size(154, 19);
            this.chkShowInactive.TabIndex = 2;
            this.chkShowInactive.Text = "Hiện cả loại ngừng dùng";
            this.chkShowInactive.UseVisualStyleBackColor = true;
            this.chkShowInactive.CheckedChanged += new System.EventHandler(this.filter_Changed);
            //
            // btnRefresh
            //
            this.btnRefresh.Location = new System.Drawing.Point(470, 9);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 27);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            //
            // txtSearch
            //
            this.txtSearch.Location = new System.Drawing.Point(50, 11);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(220, 23);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.filter_Changed);
            //
            // lblSearch
            //
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(12, 14);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(30, 15);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Tìm:";
            //
            // grpDetail
            //
            this.grpDetail.Controls.Add(this.lblPriceNote);
            this.grpDetail.Controls.Add(this.clbAmenities);
            this.grpDetail.Controls.Add(this.lblAmenities);
            this.grpDetail.Controls.Add(this.chkActive);
            this.grpDetail.Controls.Add(this.txtDescription);
            this.grpDetail.Controls.Add(this.lblDescription);
            this.grpDetail.Controls.Add(this.txtBedType);
            this.grpDetail.Controls.Add(this.lblBedType);
            this.grpDetail.Controls.Add(this.nudMaxOccupancy);
            this.grpDetail.Controls.Add(this.lblMaxOccupancy);
            this.grpDetail.Controls.Add(this.nudBasePrice);
            this.grpDetail.Controls.Add(this.lblBasePrice);
            this.grpDetail.Controls.Add(this.txtName);
            this.grpDetail.Controls.Add(this.lblName);
            this.grpDetail.Controls.Add(this.btnCancel);
            this.grpDetail.Controls.Add(this.btnSave);
            this.grpDetail.Controls.Add(this.btnToggleActive);
            this.grpDetail.Controls.Add(this.btnEdit);
            this.grpDetail.Controls.Add(this.btnAdd);
            this.grpDetail.Dock = System.Windows.Forms.DockStyle.Right;
            this.grpDetail.Location = new System.Drawing.Point(624, 44);
            this.grpDetail.Name = "grpDetail";
            this.grpDetail.Size = new System.Drawing.Size(380, 557);
            this.grpDetail.TabIndex = 2;
            this.grpDetail.TabStop = false;
            this.grpDetail.Text = "Thông tin loại phòng";
            //
            // lblPriceNote
            //
            this.lblPriceNote.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblPriceNote.Location = new System.Drawing.Point(120, 87);
            this.lblPriceNote.Name = "lblPriceNote";
            this.lblPriceNote.Size = new System.Drawing.Size(244, 18);
            this.lblPriceNote.TabIndex = 4;
            this.lblPriceNote.Text = "Đổi giá không ảnh hưởng phiếu đã đặt.";
            //
            // clbAmenities
            //
            this.clbAmenities.CheckOnClick = true;
            this.clbAmenities.FormattingEnabled = true;
            this.clbAmenities.Location = new System.Drawing.Point(120, 274);
            this.clbAmenities.Name = "clbAmenities";
            this.clbAmenities.Size = new System.Drawing.Size(244, 166);
            this.clbAmenities.TabIndex = 14;
            //
            // lblAmenities
            //
            this.lblAmenities.AutoSize = true;
            this.lblAmenities.Location = new System.Drawing.Point(14, 274);
            this.lblAmenities.Name = "lblAmenities";
            this.lblAmenities.Size = new System.Drawing.Size(58, 15);
            this.lblAmenities.TabIndex = 13;
            this.lblAmenities.Text = "Tiện nghi:";
            //
            // chkActive
            //
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(120, 246);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(77, 19);
            this.chkActive.TabIndex = 12;
            this.chkActive.Text = "Đang dùng";
            this.chkActive.UseVisualStyleBackColor = true;
            //
            // txtDescription
            //
            this.txtDescription.Location = new System.Drawing.Point(120, 180);
            this.txtDescription.MaxLength = 500;
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(244, 58);
            this.txtDescription.TabIndex = 11;
            //
            // lblDescription
            //
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(14, 183);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(42, 15);
            this.lblDescription.TabIndex = 10;
            this.lblDescription.Text = "Mô tả:";
            //
            // txtBedType
            //
            this.txtBedType.Location = new System.Drawing.Point(120, 146);
            this.txtBedType.MaxLength = 50;
            this.txtBedType.Name = "txtBedType";
            this.txtBedType.Size = new System.Drawing.Size(244, 23);
            this.txtBedType.TabIndex = 9;
            //
            // lblBedType
            //
            this.lblBedType.AutoSize = true;
            this.lblBedType.Location = new System.Drawing.Point(14, 149);
            this.lblBedType.Name = "lblBedType";
            this.lblBedType.Size = new System.Drawing.Size(69, 15);
            this.lblBedType.TabIndex = 8;
            this.lblBedType.Text = "Loại giường:";
            //
            // nudMaxOccupancy
            //
            this.nudMaxOccupancy.Location = new System.Drawing.Point(120, 112);
            this.nudMaxOccupancy.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudMaxOccupancy.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMaxOccupancy.Name = "nudMaxOccupancy";
            this.nudMaxOccupancy.Size = new System.Drawing.Size(80, 23);
            this.nudMaxOccupancy.TabIndex = 7;
            this.nudMaxOccupancy.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            //
            // lblMaxOccupancy
            //
            this.lblMaxOccupancy.AutoSize = true;
            this.lblMaxOccupancy.Location = new System.Drawing.Point(14, 115);
            this.lblMaxOccupancy.Name = "lblMaxOccupancy";
            this.lblMaxOccupancy.Size = new System.Drawing.Size(98, 15);
            this.lblMaxOccupancy.TabIndex = 6;
            this.lblMaxOccupancy.Text = "Sức chứa (người):*";
            //
            // nudBasePrice
            //
            this.nudBasePrice.Location = new System.Drawing.Point(120, 63);
            this.nudBasePrice.Name = "nudBasePrice";
            this.nudBasePrice.Size = new System.Drawing.Size(150, 23);
            this.nudBasePrice.TabIndex = 3;
            //
            // lblBasePrice
            //
            this.lblBasePrice.AutoSize = true;
            this.lblBasePrice.Location = new System.Drawing.Point(14, 66);
            this.lblBasePrice.Name = "lblBasePrice";
            this.lblBasePrice.Size = new System.Drawing.Size(89, 15);
            this.lblBasePrice.TabIndex = 2;
            this.lblBasePrice.Text = "Giá/đêm (đ):*";
            //
            // txtName
            //
            this.txtName.Location = new System.Drawing.Point(120, 28);
            this.txtName.MaxLength = 50;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(244, 23);
            this.txtName.TabIndex = 1;
            //
            // lblName
            //
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(14, 31);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(92, 15);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Tên loại phòng:*";
            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(186, 496);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // btnSave
            //
            this.btnSave.Location = new System.Drawing.Point(80, 496);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.TabIndex = 18;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // btnToggleActive
            //
            this.btnToggleActive.Location = new System.Drawing.Point(242, 454);
            this.btnToggleActive.Name = "btnToggleActive";
            this.btnToggleActive.Size = new System.Drawing.Size(100, 30);
            this.btnToggleActive.TabIndex = 17;
            this.btnToggleActive.Text = "Ngừng dùng";
            this.btnToggleActive.UseVisualStyleBackColor = true;
            this.btnToggleActive.Click += new System.EventHandler(this.btnToggleActive_Click);
            //
            // btnEdit
            //
            this.btnEdit.Location = new System.Drawing.Point(134, 454);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(100, 30);
            this.btnEdit.TabIndex = 16;
            this.btnEdit.Text = "Sửa";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            //
            // btnAdd
            //
            this.btnAdd.Location = new System.Drawing.Point(26, 454);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 30);
            this.btnAdd.TabIndex = 15;
            this.btnAdd.Text = "Thêm";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            //
            // dgvRoomTypes
            //
            this.dgvRoomTypes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRoomTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRoomTypes.Location = new System.Drawing.Point(0, 44);
            this.dgvRoomTypes.Name = "dgvRoomTypes";
            this.dgvRoomTypes.Size = new System.Drawing.Size(624, 557);
            this.dgvRoomTypes.TabIndex = 1;
            this.dgvRoomTypes.SelectionChanged += new System.EventHandler(this.dgvRoomTypes_SelectionChanged);
            //
            // frmRoomTypes
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 601);
            this.Controls.Add(this.dgvRoomTypes);
            this.Controls.Add(this.grpDetail);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(880, 560);
            this.Name = "frmRoomTypes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Loại phòng";
            this.Load += new System.EventHandler(this.frmRoomTypes_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.grpDetail.ResumeLayout(false);
            this.grpDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxOccupancy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBasePrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoomTypes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.CheckBox chkShowInactive;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.GroupBox grpDetail;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblBasePrice;
        private System.Windows.Forms.NumericUpDown nudBasePrice;
        private System.Windows.Forms.Label lblPriceNote;
        private System.Windows.Forms.Label lblMaxOccupancy;
        private System.Windows.Forms.NumericUpDown nudMaxOccupancy;
        private System.Windows.Forms.Label lblBedType;
        private System.Windows.Forms.TextBox txtBedType;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Label lblAmenities;
        private System.Windows.Forms.CheckedListBox clbAmenities;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnToggleActive;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView dgvRoomTypes;
    }
}
