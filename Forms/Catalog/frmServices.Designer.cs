namespace Hotel_management_system.Forms.Catalog
{
    partial class frmServices
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
            this.cboCategoryFilter = new System.Windows.Forms.ComboBox();
            this.lblCategoryFilter = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.grpDetail = new System.Windows.Forms.GroupBox();
            this.lblSystemNote = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.nudPrice = new System.Windows.Forms.NumericUpDown();
            this.lblPrice = new System.Windows.Forms.Label();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.lblUnit = new System.Windows.Forms.Label();
            this.cboCategory = new System.Windows.Forms.ComboBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnToggleActive = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.dgvServices = new System.Windows.Forms.DataGridView();
            this.pnlTop.SuspendLayout();
            this.grpDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvServices)).BeginInit();
            this.SuspendLayout();
            //
            // pnlTop
            //
            this.pnlTop.Controls.Add(this.chkShowInactive);
            this.pnlTop.Controls.Add(this.cboCategoryFilter);
            this.pnlTop.Controls.Add(this.lblCategoryFilter);
            this.pnlTop.Controls.Add(this.btnRefresh);
            this.pnlTop.Controls.Add(this.txtSearch);
            this.pnlTop.Controls.Add(this.lblSearch);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(984, 44);
            this.pnlTop.TabIndex = 0;
            //
            // chkShowInactive
            //
            this.chkShowInactive.AutoSize = true;
            this.chkShowInactive.Location = new System.Drawing.Point(520, 13);
            this.chkShowInactive.Name = "chkShowInactive";
            this.chkShowInactive.Size = new System.Drawing.Size(131, 19);
            this.chkShowInactive.TabIndex = 4;
            this.chkShowInactive.Text = "Hiện cả dịch vụ ngừng";
            this.chkShowInactive.UseVisualStyleBackColor = true;
            this.chkShowInactive.CheckedChanged += new System.EventHandler(this.filter_Changed);
            //
            // cboCategoryFilter
            //
            this.cboCategoryFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCategoryFilter.Location = new System.Drawing.Point(334, 11);
            this.cboCategoryFilter.Name = "cboCategoryFilter";
            this.cboCategoryFilter.Size = new System.Drawing.Size(170, 23);
            this.cboCategoryFilter.TabIndex = 3;
            this.cboCategoryFilter.SelectedIndexChanged += new System.EventHandler(this.filter_Changed);
            //
            // lblCategoryFilter
            //
            this.lblCategoryFilter.AutoSize = true;
            this.lblCategoryFilter.Location = new System.Drawing.Point(282, 14);
            this.lblCategoryFilter.Name = "lblCategoryFilter";
            this.lblCategoryFilter.Size = new System.Drawing.Size(39, 15);
            this.lblCategoryFilter.TabIndex = 2;
            this.lblCategoryFilter.Text = "Nhóm:";
            //
            // btnRefresh
            //
            this.btnRefresh.Location = new System.Drawing.Point(680, 9);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 27);
            this.btnRefresh.TabIndex = 5;
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
            this.grpDetail.Controls.Add(this.lblSystemNote);
            this.grpDetail.Controls.Add(this.chkActive);
            this.grpDetail.Controls.Add(this.nudPrice);
            this.grpDetail.Controls.Add(this.lblPrice);
            this.grpDetail.Controls.Add(this.txtUnit);
            this.grpDetail.Controls.Add(this.lblUnit);
            this.grpDetail.Controls.Add(this.cboCategory);
            this.grpDetail.Controls.Add(this.lblCategory);
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
            this.grpDetail.Size = new System.Drawing.Size(360, 437);
            this.grpDetail.TabIndex = 2;
            this.grpDetail.TabStop = false;
            this.grpDetail.Text = "Thông tin dịch vụ";
            //
            // lblSystemNote
            //
            this.lblSystemNote.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblSystemNote.Location = new System.Drawing.Point(14, 196);
            this.lblSystemNote.Name = "lblSystemNote";
            this.lblSystemNote.Size = new System.Drawing.Size(330, 36);
            this.lblSystemNote.TabIndex = 9;
            //
            // chkActive
            //
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(110, 168);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(80, 19);
            this.chkActive.TabIndex = 8;
            this.chkActive.Text = "Đang bán";
            this.chkActive.UseVisualStyleBackColor = true;
            //
            // nudPrice
            //
            this.nudPrice.Location = new System.Drawing.Point(110, 136);
            this.nudPrice.Name = "nudPrice";
            this.nudPrice.Size = new System.Drawing.Size(150, 23);
            this.nudPrice.TabIndex = 7;
            //
            // lblPrice
            //
            this.lblPrice.AutoSize = true;
            this.lblPrice.Location = new System.Drawing.Point(14, 139);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(74, 15);
            this.lblPrice.TabIndex = 6;
            this.lblPrice.Text = "Đơn giá (đ):*";
            //
            // txtUnit
            //
            this.txtUnit.Location = new System.Drawing.Point(110, 101);
            this.txtUnit.MaxLength = 20;
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(150, 23);
            this.txtUnit.TabIndex = 5;
            //
            // lblUnit
            //
            this.lblUnit.AutoSize = true;
            this.lblUnit.Location = new System.Drawing.Point(14, 104);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(52, 15);
            this.lblUnit.TabIndex = 4;
            this.lblUnit.Text = "Đơn vị:*";
            //
            // cboCategory
            //
            this.cboCategory.Location = new System.Drawing.Point(110, 66);
            this.cboCategory.MaxLength = 50;
            this.cboCategory.Name = "cboCategory";
            this.cboCategory.Size = new System.Drawing.Size(234, 23);
            this.cboCategory.TabIndex = 3;
            //
            // lblCategory
            //
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(14, 69);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(39, 15);
            this.lblCategory.TabIndex = 2;
            this.lblCategory.Text = "Nhóm:";
            //
            // txtName
            //
            this.txtName.Location = new System.Drawing.Point(110, 30);
            this.txtName.MaxLength = 100;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(234, 23);
            this.txtName.TabIndex = 1;
            //
            // lblName
            //
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(14, 33);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(80, 15);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Tên dịch vụ:*";
            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(176, 292);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // btnSave
            //
            this.btnSave.Location = new System.Drawing.Point(70, 292);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // btnToggleActive
            //
            this.btnToggleActive.Location = new System.Drawing.Point(232, 250);
            this.btnToggleActive.Name = "btnToggleActive";
            this.btnToggleActive.Size = new System.Drawing.Size(100, 30);
            this.btnToggleActive.TabIndex = 12;
            this.btnToggleActive.Text = "Ngừng bán";
            this.btnToggleActive.UseVisualStyleBackColor = true;
            this.btnToggleActive.Click += new System.EventHandler(this.btnToggleActive_Click);
            //
            // btnEdit
            //
            this.btnEdit.Location = new System.Drawing.Point(124, 250);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(100, 30);
            this.btnEdit.TabIndex = 11;
            this.btnEdit.Text = "Sửa";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            //
            // btnAdd
            //
            this.btnAdd.Location = new System.Drawing.Point(16, 250);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 30);
            this.btnAdd.TabIndex = 10;
            this.btnAdd.Text = "Thêm";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            //
            // dgvServices
            //
            this.dgvServices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvServices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvServices.Location = new System.Drawing.Point(0, 44);
            this.dgvServices.Name = "dgvServices";
            this.dgvServices.Size = new System.Drawing.Size(624, 437);
            this.dgvServices.TabIndex = 1;
            this.dgvServices.SelectionChanged += new System.EventHandler(this.dgvServices_SelectionChanged);
            //
            // frmServices
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 481);
            this.Controls.Add(this.dgvServices);
            this.Controls.Add(this.grpDetail);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(860, 420);
            this.Name = "frmServices";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dịch vụ";
            this.Load += new System.EventHandler(this.frmServices_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.grpDetail.ResumeLayout(false);
            this.grpDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvServices)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblCategoryFilter;
        private System.Windows.Forms.ComboBox cboCategoryFilter;
        private System.Windows.Forms.CheckBox chkShowInactive;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.GroupBox grpDetail;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox cboCategory;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.NumericUpDown nudPrice;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Label lblSystemNote;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnToggleActive;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView dgvServices;
    }
}
