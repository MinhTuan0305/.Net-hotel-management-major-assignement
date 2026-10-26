namespace Hotel_management_system.Forms.Catalog
{
    partial class frmEmployees
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
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabEmployees = new System.Windows.Forms.TabPage();
            this.dgvEmployees = new System.Windows.Forms.DataGridView();
            this.grpDetail = new System.Windows.Forms.GroupBox();
            this.lblPasswordHint = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.dtpHireDate = new System.Windows.Forms.DateTimePicker();
            this.lblHireDate = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.cboRole = new System.Windows.Forms.ComboBox();
            this.lblRole = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.lblFullName = new System.Windows.Forms.Label();
            this.btnResetPassword = new System.Windows.Forms.Button();
            this.btnToggleLock = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.chkShowLocked = new System.Windows.Forms.CheckBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.tabRoles = new System.Windows.Forms.TabPage();
            this.dgvRoles = new System.Windows.Forms.DataGridView();
            this.pnlRoleEdit = new System.Windows.Forms.Panel();
            this.lblRoleNote = new System.Windows.Forms.Label();
            this.btnSaveRole = new System.Windows.Forms.Button();
            this.txtRoleDescription = new System.Windows.Forms.TextBox();
            this.lblRoleDescription = new System.Windows.Forms.Label();
            this.tabMain.SuspendLayout();
            this.tabEmployees.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmployees)).BeginInit();
            this.grpDetail.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.tabRoles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoles)).BeginInit();
            this.pnlRoleEdit.SuspendLayout();
            this.SuspendLayout();
            //
            // tabMain
            //
            this.tabMain.Controls.Add(this.tabEmployees);
            this.tabMain.Controls.Add(this.tabRoles);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(1084, 541);
            this.tabMain.TabIndex = 0;
            //
            // tabEmployees
            //
            this.tabEmployees.Controls.Add(this.dgvEmployees);
            this.tabEmployees.Controls.Add(this.grpDetail);
            this.tabEmployees.Controls.Add(this.pnlTop);
            this.tabEmployees.Location = new System.Drawing.Point(4, 24);
            this.tabEmployees.Name = "tabEmployees";
            this.tabEmployees.Padding = new System.Windows.Forms.Padding(3);
            this.tabEmployees.Size = new System.Drawing.Size(1076, 513);
            this.tabEmployees.TabIndex = 0;
            this.tabEmployees.Text = "Nhân viên";
            this.tabEmployees.UseVisualStyleBackColor = true;
            //
            // dgvEmployees
            //
            this.dgvEmployees.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEmployees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEmployees.Location = new System.Drawing.Point(3, 47);
            this.dgvEmployees.Name = "dgvEmployees";
            this.dgvEmployees.Size = new System.Drawing.Size(710, 463);
            this.dgvEmployees.TabIndex = 1;
            this.dgvEmployees.SelectionChanged += new System.EventHandler(this.dgvEmployees_SelectionChanged);
            //
            // grpDetail
            //
            this.grpDetail.Controls.Add(this.lblPasswordHint);
            this.grpDetail.Controls.Add(this.txtPassword);
            this.grpDetail.Controls.Add(this.lblPassword);
            this.grpDetail.Controls.Add(this.dtpHireDate);
            this.grpDetail.Controls.Add(this.lblHireDate);
            this.grpDetail.Controls.Add(this.txtEmail);
            this.grpDetail.Controls.Add(this.lblEmail);
            this.grpDetail.Controls.Add(this.txtPhone);
            this.grpDetail.Controls.Add(this.lblPhone);
            this.grpDetail.Controls.Add(this.cboRole);
            this.grpDetail.Controls.Add(this.lblRole);
            this.grpDetail.Controls.Add(this.txtUsername);
            this.grpDetail.Controls.Add(this.lblUsername);
            this.grpDetail.Controls.Add(this.txtFullName);
            this.grpDetail.Controls.Add(this.lblFullName);
            this.grpDetail.Controls.Add(this.btnResetPassword);
            this.grpDetail.Controls.Add(this.btnToggleLock);
            this.grpDetail.Controls.Add(this.btnCancel);
            this.grpDetail.Controls.Add(this.btnSave);
            this.grpDetail.Controls.Add(this.btnEdit);
            this.grpDetail.Controls.Add(this.btnAdd);
            this.grpDetail.Dock = System.Windows.Forms.DockStyle.Right;
            this.grpDetail.Location = new System.Drawing.Point(713, 47);
            this.grpDetail.Name = "grpDetail";
            this.grpDetail.Size = new System.Drawing.Size(360, 463);
            this.grpDetail.TabIndex = 2;
            this.grpDetail.TabStop = false;
            this.grpDetail.Text = "Thông tin nhân viên";
            //
            // lblPasswordHint
            //
            this.lblPasswordHint.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblPasswordHint.Location = new System.Drawing.Point(110, 260);
            this.lblPasswordHint.Name = "lblPasswordHint";
            this.lblPasswordHint.Size = new System.Drawing.Size(234, 30);
            this.lblPasswordHint.TabIndex = 14;
            this.lblPasswordHint.Text = "Chỉ nhập khi thêm mới. Đổi mật khẩu sau này dùng nút Đặt lại.";
            //
            // txtPassword
            //
            this.txtPassword.Location = new System.Drawing.Point(110, 232);
            this.txtPassword.MaxLength = 100;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(234, 23);
            this.txtPassword.TabIndex = 13;
            this.txtPassword.UseSystemPasswordChar = true;
            //
            // lblPassword
            //
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(14, 235);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(66, 15);
            this.lblPassword.TabIndex = 12;
            this.lblPassword.Text = "Mật khẩu:*";
            //
            // dtpHireDate
            //
            this.dtpHireDate.CustomFormat = "dd/MM/yyyy";
            this.dtpHireDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpHireDate.Location = new System.Drawing.Point(110, 198);
            this.dtpHireDate.Name = "dtpHireDate";
            this.dtpHireDate.Size = new System.Drawing.Size(140, 23);
            this.dtpHireDate.TabIndex = 11;
            //
            // lblHireDate
            //
            this.lblHireDate.AutoSize = true;
            this.lblHireDate.Location = new System.Drawing.Point(14, 201);
            this.lblHireDate.Name = "lblHireDate";
            this.lblHireDate.Size = new System.Drawing.Size(76, 15);
            this.lblHireDate.TabIndex = 10;
            this.lblHireDate.Text = "Ngày vào làm:";
            //
            // txtEmail
            //
            this.txtEmail.Location = new System.Drawing.Point(110, 164);
            this.txtEmail.MaxLength = 100;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(234, 23);
            this.txtEmail.TabIndex = 9;
            //
            // lblEmail
            //
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(14, 167);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(39, 15);
            this.lblEmail.TabIndex = 8;
            this.lblEmail.Text = "Email:";
            //
            // txtPhone
            //
            this.txtPhone.Location = new System.Drawing.Point(110, 130);
            this.txtPhone.MaxLength = 20;
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(160, 23);
            this.txtPhone.TabIndex = 7;
            //
            // lblPhone
            //
            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new System.Drawing.Point(14, 133);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(79, 15);
            this.lblPhone.TabIndex = 6;
            this.lblPhone.Text = "Số điện thoại:";
            //
            // cboRole
            //
            this.cboRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRole.Location = new System.Drawing.Point(110, 96);
            this.cboRole.Name = "cboRole";
            this.cboRole.Size = new System.Drawing.Size(234, 23);
            this.cboRole.TabIndex = 5;
            //
            // lblRole
            //
            this.lblRole.AutoSize = true;
            this.lblRole.Location = new System.Drawing.Point(14, 99);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(50, 15);
            this.lblRole.TabIndex = 4;
            this.lblRole.Text = "Vai trò:*";
            //
            // txtUsername
            //
            this.txtUsername.Location = new System.Drawing.Point(110, 62);
            this.txtUsername.MaxLength = 50;
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(160, 23);
            this.txtUsername.TabIndex = 3;
            //
            // lblUsername
            //
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(14, 65);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(92, 15);
            this.lblUsername.TabIndex = 2;
            this.lblUsername.Text = "Tên đăng nhập:*";
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
            // btnResetPassword
            //
            this.btnResetPassword.Location = new System.Drawing.Point(178, 386);
            this.btnResetPassword.Name = "btnResetPassword";
            this.btnResetPassword.Size = new System.Drawing.Size(154, 30);
            this.btnResetPassword.TabIndex = 20;
            this.btnResetPassword.Text = "Đặt lại mật khẩu";
            this.btnResetPassword.UseVisualStyleBackColor = true;
            this.btnResetPassword.Click += new System.EventHandler(this.btnResetPassword_Click);
            //
            // btnToggleLock
            //
            this.btnToggleLock.Location = new System.Drawing.Point(16, 386);
            this.btnToggleLock.Name = "btnToggleLock";
            this.btnToggleLock.Size = new System.Drawing.Size(154, 30);
            this.btnToggleLock.TabIndex = 19;
            this.btnToggleLock.Text = "Khóa tài khoản";
            this.btnToggleLock.UseVisualStyleBackColor = true;
            this.btnToggleLock.Click += new System.EventHandler(this.btnToggleLock_Click);
            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(232, 340);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // btnSave
            //
            this.btnSave.Location = new System.Drawing.Point(124, 340);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // btnEdit
            //
            this.btnEdit.Location = new System.Drawing.Point(124, 300);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(100, 30);
            this.btnEdit.TabIndex = 16;
            this.btnEdit.Text = "Sửa";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            //
            // btnAdd
            //
            this.btnAdd.Location = new System.Drawing.Point(16, 300);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 30);
            this.btnAdd.TabIndex = 15;
            this.btnAdd.Text = "Thêm";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            //
            // pnlTop
            //
            this.pnlTop.Controls.Add(this.chkShowLocked);
            this.pnlTop.Controls.Add(this.btnRefresh);
            this.pnlTop.Controls.Add(this.txtSearch);
            this.pnlTop.Controls.Add(this.lblSearch);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(3, 3);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1070, 44);
            this.pnlTop.TabIndex = 0;
            //
            // chkShowLocked
            //
            this.chkShowLocked.AutoSize = true;
            this.chkShowLocked.Checked = true;
            this.chkShowLocked.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowLocked.Location = new System.Drawing.Point(380, 13);
            this.chkShowLocked.Name = "chkShowLocked";
            this.chkShowLocked.Size = new System.Drawing.Size(146, 19);
            this.chkShowLocked.TabIndex = 2;
            this.chkShowLocked.Text = "Hiện cả tài khoản khóa";
            this.chkShowLocked.UseVisualStyleBackColor = true;
            this.chkShowLocked.CheckedChanged += new System.EventHandler(this.filter_Changed);
            //
            // btnRefresh
            //
            this.btnRefresh.Location = new System.Drawing.Point(546, 9);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 27);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            //
            // txtSearch
            //
            this.txtSearch.Location = new System.Drawing.Point(156, 11);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(210, 23);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.filter_Changed);
            //
            // lblSearch
            //
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(9, 14);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(144, 15);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Tìm (tên, tài khoản, SĐT):";
            //
            // tabRoles
            //
            this.tabRoles.Controls.Add(this.dgvRoles);
            this.tabRoles.Controls.Add(this.pnlRoleEdit);
            this.tabRoles.Location = new System.Drawing.Point(4, 24);
            this.tabRoles.Name = "tabRoles";
            this.tabRoles.Padding = new System.Windows.Forms.Padding(3);
            this.tabRoles.Size = new System.Drawing.Size(1076, 513);
            this.tabRoles.TabIndex = 1;
            this.tabRoles.Text = "Vai trò";
            this.tabRoles.UseVisualStyleBackColor = true;
            //
            // dgvRoles
            //
            this.dgvRoles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRoles.Location = new System.Drawing.Point(3, 3);
            this.dgvRoles.Name = "dgvRoles";
            this.dgvRoles.Size = new System.Drawing.Size(1070, 397);
            this.dgvRoles.TabIndex = 0;
            this.dgvRoles.SelectionChanged += new System.EventHandler(this.dgvRoles_SelectionChanged);
            //
            // pnlRoleEdit
            //
            this.pnlRoleEdit.Controls.Add(this.lblRoleNote);
            this.pnlRoleEdit.Controls.Add(this.btnSaveRole);
            this.pnlRoleEdit.Controls.Add(this.txtRoleDescription);
            this.pnlRoleEdit.Controls.Add(this.lblRoleDescription);
            this.pnlRoleEdit.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlRoleEdit.Location = new System.Drawing.Point(3, 400);
            this.pnlRoleEdit.Name = "pnlRoleEdit";
            this.pnlRoleEdit.Size = new System.Drawing.Size(1070, 110);
            this.pnlRoleEdit.TabIndex = 1;
            //
            // lblRoleNote
            //
            this.lblRoleNote.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblRoleNote.Location = new System.Drawing.Point(110, 78);
            this.lblRoleNote.Name = "lblRoleNote";
            this.lblRoleNote.Size = new System.Drawing.Size(600, 20);
            this.lblRoleNote.TabIndex = 3;
            this.lblRoleNote.Text = "4 vai trò cố định, không thêm/xóa/đổi tên (phân quyền dựa vào tên vai trò). Chỉ sửa mô tả.";
            //
            // btnSaveRole
            //
            this.btnSaveRole.Location = new System.Drawing.Point(620, 12);
            this.btnSaveRole.Name = "btnSaveRole";
            this.btnSaveRole.Size = new System.Drawing.Size(110, 30);
            this.btnSaveRole.TabIndex = 2;
            this.btnSaveRole.Text = "Lưu mô tả";
            this.btnSaveRole.UseVisualStyleBackColor = true;
            this.btnSaveRole.Click += new System.EventHandler(this.btnSaveRole_Click);
            //
            // txtRoleDescription
            //
            this.txtRoleDescription.Location = new System.Drawing.Point(110, 14);
            this.txtRoleDescription.MaxLength = 200;
            this.txtRoleDescription.Multiline = true;
            this.txtRoleDescription.Name = "txtRoleDescription";
            this.txtRoleDescription.Size = new System.Drawing.Size(500, 58);
            this.txtRoleDescription.TabIndex = 1;
            //
            // lblRoleDescription
            //
            this.lblRoleDescription.AutoSize = true;
            this.lblRoleDescription.Location = new System.Drawing.Point(10, 17);
            this.lblRoleDescription.Name = "lblRoleDescription";
            this.lblRoleDescription.Size = new System.Drawing.Size(42, 15);
            this.lblRoleDescription.TabIndex = 0;
            this.lblRoleDescription.Text = "Mô tả:";
            //
            // frmEmployees
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 541);
            this.Controls.Add(this.tabMain);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(980, 520);
            this.Name = "frmEmployees";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nhân viên và vai trò";
            this.Load += new System.EventHandler(this.frmEmployees_Load);
            this.tabMain.ResumeLayout(false);
            this.tabEmployees.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmployees)).EndInit();
            this.grpDetail.ResumeLayout(false);
            this.grpDetail.PerformLayout();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.tabRoles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoles)).EndInit();
            this.pnlRoleEdit.ResumeLayout(false);
            this.pnlRoleEdit.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabEmployees;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.CheckBox chkShowLocked;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView dgvEmployees;
        private System.Windows.Forms.GroupBox grpDetail;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.ComboBox cboRole;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblHireDate;
        private System.Windows.Forms.DateTimePicker dtpHireDate;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPasswordHint;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnToggleLock;
        private System.Windows.Forms.Button btnResetPassword;
        private System.Windows.Forms.TabPage tabRoles;
        private System.Windows.Forms.DataGridView dgvRoles;
        private System.Windows.Forms.Panel pnlRoleEdit;
        private System.Windows.Forms.Label lblRoleDescription;
        private System.Windows.Forms.TextBox txtRoleDescription;
        private System.Windows.Forms.Button btnSaveRole;
        private System.Windows.Forms.Label lblRoleNote;
    }
}
