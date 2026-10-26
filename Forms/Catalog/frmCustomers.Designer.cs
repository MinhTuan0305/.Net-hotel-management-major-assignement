namespace Hotel_management_system.Forms.Catalog
{
    partial class frmCustomers
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
            this.btnPick = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.grpDetail = new System.Windows.Forms.GroupBox();
            this.txtNationality = new System.Windows.Forms.TextBox();
            this.lblNationality = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtIdNumber = new System.Windows.Forms.TextBox();
            this.lblIdNumber = new System.Windows.Forms.Label();
            this.dtpDateOfBirth = new System.Windows.Forms.DateTimePicker();
            this.lblDateOfBirth = new System.Windows.Forms.Label();
            this.cboGender = new System.Windows.Forms.ComboBox();
            this.lblGender = new System.Windows.Forms.Label();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.lblFullName = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.dgvCustomers = new System.Windows.Forms.DataGridView();
            this.pnlHistory = new System.Windows.Forms.Panel();
            this.dgvHistory = new System.Windows.Forms.DataGridView();
            this.lblHistory = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            this.grpDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).BeginInit();
            this.pnlHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).BeginInit();
            this.SuspendLayout();
            //
            // pnlTop
            //
            this.pnlTop.Controls.Add(this.btnPick);
            this.pnlTop.Controls.Add(this.btnRefresh);
            this.pnlTop.Controls.Add(this.txtSearch);
            this.pnlTop.Controls.Add(this.lblSearch);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1084, 44);
            this.pnlTop.TabIndex = 0;
            //
            // btnPick
            //
            this.btnPick.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnPick.Location = new System.Drawing.Point(480, 8);
            this.btnPick.Name = "btnPick";
            this.btnPick.Size = new System.Drawing.Size(150, 29);
            this.btnPick.TabIndex = 3;
            this.btnPick.Text = "Chọn khách này";
            this.btnPick.UseVisualStyleBackColor = true;
            this.btnPick.Visible = false;
            this.btnPick.Click += new System.EventHandler(this.btnPick_Click);
            //
            // btnRefresh
            //
            this.btnRefresh.Location = new System.Drawing.Point(372, 9);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 27);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            //
            // txtSearch
            //
            this.txtSearch.Location = new System.Drawing.Point(156, 11);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(200, 23);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            //
            // lblSearch
            //
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(12, 14);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(140, 15);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Tìm (tên, SĐT, CCCD):";
            //
            // grpDetail
            //
            this.grpDetail.Controls.Add(this.txtNationality);
            this.grpDetail.Controls.Add(this.lblNationality);
            this.grpDetail.Controls.Add(this.txtAddress);
            this.grpDetail.Controls.Add(this.lblAddress);
            this.grpDetail.Controls.Add(this.txtEmail);
            this.grpDetail.Controls.Add(this.lblEmail);
            this.grpDetail.Controls.Add(this.txtPhone);
            this.grpDetail.Controls.Add(this.lblPhone);
            this.grpDetail.Controls.Add(this.txtIdNumber);
            this.grpDetail.Controls.Add(this.lblIdNumber);
            this.grpDetail.Controls.Add(this.dtpDateOfBirth);
            this.grpDetail.Controls.Add(this.lblDateOfBirth);
            this.grpDetail.Controls.Add(this.cboGender);
            this.grpDetail.Controls.Add(this.lblGender);
            this.grpDetail.Controls.Add(this.txtFullName);
            this.grpDetail.Controls.Add(this.lblFullName);
            this.grpDetail.Controls.Add(this.btnCancel);
            this.grpDetail.Controls.Add(this.btnSave);
            this.grpDetail.Controls.Add(this.btnDelete);
            this.grpDetail.Controls.Add(this.btnEdit);
            this.grpDetail.Controls.Add(this.btnAdd);
            this.grpDetail.Dock = System.Windows.Forms.DockStyle.Right;
            this.grpDetail.Location = new System.Drawing.Point(724, 44);
            this.grpDetail.Name = "grpDetail";
            this.grpDetail.Size = new System.Drawing.Size(360, 517);
            this.grpDetail.TabIndex = 3;
            this.grpDetail.TabStop = false;
            this.grpDetail.Text = "Thông tin khách hàng";
            //
            // txtNationality
            //
            this.txtNationality.Location = new System.Drawing.Point(110, 286);
            this.txtNationality.MaxLength = 50;
            this.txtNationality.Name = "txtNationality";
            this.txtNationality.Size = new System.Drawing.Size(234, 23);
            this.txtNationality.TabIndex = 15;
            //
            // lblNationality
            //
            this.lblNationality.AutoSize = true;
            this.lblNationality.Location = new System.Drawing.Point(14, 289);
            this.lblNationality.Name = "lblNationality";
            this.lblNationality.Size = new System.Drawing.Size(61, 15);
            this.lblNationality.TabIndex = 14;
            this.lblNationality.Text = "Quốc tịch:";
            //
            // txtAddress
            //
            this.txtAddress.Location = new System.Drawing.Point(110, 232);
            this.txtAddress.MaxLength = 255;
            this.txtAddress.Multiline = true;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(234, 44);
            this.txtAddress.TabIndex = 13;
            //
            // lblAddress
            //
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(14, 235);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(46, 15);
            this.lblAddress.TabIndex = 12;
            this.lblAddress.Text = "Địa chỉ:";
            //
            // txtEmail
            //
            this.txtEmail.Location = new System.Drawing.Point(110, 198);
            this.txtEmail.MaxLength = 100;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(234, 23);
            this.txtEmail.TabIndex = 11;
            //
            // lblEmail
            //
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(14, 201);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(39, 15);
            this.lblEmail.TabIndex = 10;
            this.lblEmail.Text = "Email:";
            //
            // txtPhone
            //
            this.txtPhone.Location = new System.Drawing.Point(110, 164);
            this.txtPhone.MaxLength = 20;
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(160, 23);
            this.txtPhone.TabIndex = 9;
            //
            // lblPhone
            //
            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new System.Drawing.Point(14, 167);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(84, 15);
            this.lblPhone.TabIndex = 8;
            this.lblPhone.Text = "Số điện thoại:*";
            //
            // txtIdNumber
            //
            this.txtIdNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtIdNumber.Location = new System.Drawing.Point(110, 130);
            this.txtIdNumber.MaxLength = 20;
            this.txtIdNumber.Name = "txtIdNumber";
            this.txtIdNumber.Size = new System.Drawing.Size(160, 23);
            this.txtIdNumber.TabIndex = 7;
            //
            // lblIdNumber
            //
            this.lblIdNumber.AutoSize = true;
            this.lblIdNumber.Location = new System.Drawing.Point(14, 133);
            this.lblIdNumber.Name = "lblIdNumber";
            this.lblIdNumber.Size = new System.Drawing.Size(90, 15);
            this.lblIdNumber.TabIndex = 6;
            this.lblIdNumber.Text = "CCCD/Hộ chiếu:*";
            //
            // dtpDateOfBirth
            //
            this.dtpDateOfBirth.CustomFormat = "dd/MM/yyyy";
            this.dtpDateOfBirth.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateOfBirth.Location = new System.Drawing.Point(110, 96);
            this.dtpDateOfBirth.Name = "dtpDateOfBirth";
            this.dtpDateOfBirth.ShowCheckBox = true;
            this.dtpDateOfBirth.Size = new System.Drawing.Size(160, 23);
            this.dtpDateOfBirth.TabIndex = 5;
            //
            // lblDateOfBirth
            //
            this.lblDateOfBirth.AutoSize = true;
            this.lblDateOfBirth.Location = new System.Drawing.Point(14, 99);
            this.lblDateOfBirth.Name = "lblDateOfBirth";
            this.lblDateOfBirth.Size = new System.Drawing.Size(63, 15);
            this.lblDateOfBirth.TabIndex = 4;
            this.lblDateOfBirth.Text = "Ngày sinh:";
            //
            // cboGender
            //
            this.cboGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGender.Location = new System.Drawing.Point(110, 62);
            this.cboGender.Name = "cboGender";
            this.cboGender.Size = new System.Drawing.Size(120, 23);
            this.cboGender.TabIndex = 3;
            //
            // lblGender
            //
            this.lblGender.AutoSize = true;
            this.lblGender.Location = new System.Drawing.Point(14, 65);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(55, 15);
            this.lblGender.TabIndex = 2;
            this.lblGender.Text = "Giới tính:";
            //
            // txtFullName
            //
            this.txtFullName.Location = new System.Drawing.Point(110, 28);
            this.txtFullName.MaxLength = 100;
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(234, 23);
            this.txtFullName.TabIndex = 1;
            //
            // lblFullName
            //
            this.lblFullName.AutoSize = true;
            this.lblFullName.Location = new System.Drawing.Point(14, 31);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(51, 15);
            this.lblFullName.TabIndex = 0;
            this.lblFullName.Text = "Họ tên:*";
            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(176, 376);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // btnSave
            //
            this.btnSave.Location = new System.Drawing.Point(70, 376);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.TabIndex = 19;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // btnDelete
            //
            this.btnDelete.Location = new System.Drawing.Point(232, 334);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 30);
            this.btnDelete.TabIndex = 18;
            this.btnDelete.Text = "Xóa";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            //
            // btnEdit
            //
            this.btnEdit.Location = new System.Drawing.Point(124, 334);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(100, 30);
            this.btnEdit.TabIndex = 17;
            this.btnEdit.Text = "Sửa";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            //
            // btnAdd
            //
            this.btnAdd.Location = new System.Drawing.Point(16, 334);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 30);
            this.btnAdd.TabIndex = 16;
            this.btnAdd.Text = "Thêm";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            //
            // dgvCustomers
            //
            this.dgvCustomers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCustomers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCustomers.Location = new System.Drawing.Point(0, 44);
            this.dgvCustomers.Name = "dgvCustomers";
            this.dgvCustomers.Size = new System.Drawing.Size(724, 337);
            this.dgvCustomers.TabIndex = 1;
            this.dgvCustomers.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCustomers_CellDoubleClick);
            this.dgvCustomers.SelectionChanged += new System.EventHandler(this.dgvCustomers_SelectionChanged);
            //
            // pnlHistory
            //
            this.pnlHistory.Controls.Add(this.dgvHistory);
            this.pnlHistory.Controls.Add(this.lblHistory);
            this.pnlHistory.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlHistory.Location = new System.Drawing.Point(0, 381);
            this.pnlHistory.Name = "pnlHistory";
            this.pnlHistory.Size = new System.Drawing.Size(724, 180);
            this.pnlHistory.TabIndex = 2;
            //
            // dgvHistory
            //
            this.dgvHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHistory.Location = new System.Drawing.Point(0, 24);
            this.dgvHistory.Name = "dgvHistory";
            this.dgvHistory.Size = new System.Drawing.Size(724, 156);
            this.dgvHistory.TabIndex = 1;
            //
            // lblHistory
            //
            this.lblHistory.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHistory.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblHistory.Location = new System.Drawing.Point(0, 0);
            this.lblHistory.Name = "lblHistory";
            this.lblHistory.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.lblHistory.Size = new System.Drawing.Size(724, 24);
            this.lblHistory.TabIndex = 0;
            this.lblHistory.Text = "Lịch sử đặt phòng của khách";
            this.lblHistory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // frmCustomers
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 561);
            this.Controls.Add(this.dgvCustomers);
            this.Controls.Add(this.pnlHistory);
            this.Controls.Add(this.grpDetail);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(980, 520);
            this.Name = "frmCustomers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Khách hàng";
            this.Load += new System.EventHandler(this.frmCustomers_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.grpDetail.ResumeLayout(false);
            this.grpDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).EndInit();
            this.pnlHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnPick;
        private System.Windows.Forms.GroupBox grpDetail;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.ComboBox cboGender;
        private System.Windows.Forms.Label lblDateOfBirth;
        private System.Windows.Forms.DateTimePicker dtpDateOfBirth;
        private System.Windows.Forms.Label lblIdNumber;
        private System.Windows.Forms.TextBox txtIdNumber;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lblNationality;
        private System.Windows.Forms.TextBox txtNationality;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView dgvCustomers;
        private System.Windows.Forms.Panel pnlHistory;
        private System.Windows.Forms.Label lblHistory;
        private System.Windows.Forms.DataGridView dgvHistory;
    }
}
