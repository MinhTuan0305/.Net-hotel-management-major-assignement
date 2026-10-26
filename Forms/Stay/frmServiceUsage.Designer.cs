namespace Hotel_management_system.Forms.Stay
{
    partial class frmServiceUsage
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblLock = new System.Windows.Forms.Label();
            this.lblStay = new System.Windows.Forms.Label();
            this.lblRoom = new System.Windows.Forms.Label();
            this.grpInput = new System.Windows.Forms.GroupBox();
            this.btnCancelEdit = new System.Windows.Forms.Button();
            this.btnSaveEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.lblNote = new System.Windows.Forms.Label();
            this.dtpUsedAt = new System.Windows.Forms.DateTimePicker();
            this.lblUsedAt = new System.Windows.Forms.Label();
            this.nudQuantity = new System.Windows.Forms.NumericUpDown();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.cboService = new System.Windows.Forms.ComboBox();
            this.lblService = new System.Windows.Forms.Label();
            this.dgvUsages = new System.Windows.Forms.DataGridView();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.grpInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsages)).BeginInit();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlHeader
            //
            this.pnlHeader.Controls.Add(this.lblLock);
            this.pnlHeader.Controls.Add(this.lblStay);
            this.pnlHeader.Controls.Add(this.lblRoom);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(944, 70);
            this.pnlHeader.TabIndex = 0;
            //
            // lblLock
            //
            this.lblLock.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblLock.Location = new System.Drawing.Point(12, 48);
            this.lblLock.Name = "lblLock";
            this.lblLock.Size = new System.Drawing.Size(920, 18);
            this.lblLock.TabIndex = 2;
            //
            // lblStay
            //
            this.lblStay.Location = new System.Drawing.Point(12, 30);
            this.lblStay.Name = "lblStay";
            this.lblStay.Size = new System.Drawing.Size(920, 18);
            this.lblStay.TabIndex = 1;
            //
            // lblRoom
            //
            this.lblRoom.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblRoom.Location = new System.Drawing.Point(12, 6);
            this.lblRoom.Name = "lblRoom";
            this.lblRoom.Size = new System.Drawing.Size(920, 22);
            this.lblRoom.TabIndex = 0;
            this.lblRoom.Text = "Phòng";
            //
            // grpInput
            //
            this.grpInput.Controls.Add(this.btnCancelEdit);
            this.grpInput.Controls.Add(this.btnSaveEdit);
            this.grpInput.Controls.Add(this.btnDelete);
            this.grpInput.Controls.Add(this.btnEdit);
            this.grpInput.Controls.Add(this.btnAdd);
            this.grpInput.Controls.Add(this.txtNote);
            this.grpInput.Controls.Add(this.lblNote);
            this.grpInput.Controls.Add(this.dtpUsedAt);
            this.grpInput.Controls.Add(this.lblUsedAt);
            this.grpInput.Controls.Add(this.nudQuantity);
            this.grpInput.Controls.Add(this.lblQuantity);
            this.grpInput.Controls.Add(this.cboService);
            this.grpInput.Controls.Add(this.lblService);
            this.grpInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpInput.Location = new System.Drawing.Point(0, 70);
            this.grpInput.Name = "grpInput";
            this.grpInput.Size = new System.Drawing.Size(944, 106);
            this.grpInput.TabIndex = 1;
            this.grpInput.TabStop = false;
            this.grpInput.Text = "Ghi dịch vụ";
            //
            // btnCancelEdit
            //
            this.btnCancelEdit.Location = new System.Drawing.Point(830, 62);
            this.btnCancelEdit.Name = "btnCancelEdit";
            this.btnCancelEdit.Size = new System.Drawing.Size(100, 30);
            this.btnCancelEdit.TabIndex = 12;
            this.btnCancelEdit.Text = "Hủy sửa";
            this.btnCancelEdit.UseVisualStyleBackColor = true;
            this.btnCancelEdit.Click += new System.EventHandler(this.btnCancelEdit_Click);
            //
            // btnSaveEdit
            //
            this.btnSaveEdit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSaveEdit.Location = new System.Drawing.Point(724, 62);
            this.btnSaveEdit.Name = "btnSaveEdit";
            this.btnSaveEdit.Size = new System.Drawing.Size(100, 30);
            this.btnSaveEdit.TabIndex = 11;
            this.btnSaveEdit.Text = "Lưu sửa";
            this.btnSaveEdit.UseVisualStyleBackColor = true;
            this.btnSaveEdit.Click += new System.EventHandler(this.btnSaveEdit_Click);
            //
            // btnDelete
            //
            this.btnDelete.Location = new System.Drawing.Point(830, 62);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 30);
            this.btnDelete.TabIndex = 10;
            this.btnDelete.Text = "Xóa dòng";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            //
            // btnEdit
            //
            this.btnEdit.Location = new System.Drawing.Point(724, 62);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(100, 30);
            this.btnEdit.TabIndex = 9;
            this.btnEdit.Text = "Sửa dòng";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            //
            // btnAdd
            //
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAdd.Location = new System.Drawing.Point(724, 24);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(206, 30);
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Text = "Thêm dịch vụ";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            //
            // txtNote
            //
            this.txtNote.Location = new System.Drawing.Point(320, 66);
            this.txtNote.MaxLength = 200;
            this.txtNote.Name = "txtNote";
            this.txtNote.Size = new System.Drawing.Size(390, 23);
            this.txtNote.TabIndex = 7;
            //
            // lblNote
            //
            this.lblNote.AutoSize = true;
            this.lblNote.Location = new System.Drawing.Point(262, 69);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(51, 15);
            this.lblNote.TabIndex = 6;
            this.lblNote.Text = "Ghi chú:";
            //
            // dtpUsedAt
            //
            this.dtpUsedAt.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpUsedAt.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpUsedAt.Location = new System.Drawing.Point(90, 66);
            this.dtpUsedAt.Name = "dtpUsedAt";
            this.dtpUsedAt.Size = new System.Drawing.Size(150, 23);
            this.dtpUsedAt.TabIndex = 5;
            //
            // lblUsedAt
            //
            this.lblUsedAt.AutoSize = true;
            this.lblUsedAt.Location = new System.Drawing.Point(12, 69);
            this.lblUsedAt.Name = "lblUsedAt";
            this.lblUsedAt.Size = new System.Drawing.Size(72, 15);
            this.lblUsedAt.TabIndex = 4;
            this.lblUsedAt.Text = "Thời điểm:*";
            //
            // nudQuantity
            //
            this.nudQuantity.Location = new System.Drawing.Point(620, 28);
            this.nudQuantity.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nudQuantity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudQuantity.Name = "nudQuantity";
            this.nudQuantity.Size = new System.Drawing.Size(90, 23);
            this.nudQuantity.TabIndex = 3;
            this.nudQuantity.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            //
            // lblQuantity
            //
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new System.Drawing.Point(550, 31);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(64, 15);
            this.lblQuantity.TabIndex = 2;
            this.lblQuantity.Text = "Số lượng:*";
            //
            // cboService
            //
            this.cboService.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboService.Location = new System.Drawing.Point(90, 28);
            this.cboService.Name = "cboService";
            this.cboService.Size = new System.Drawing.Size(440, 23);
            this.cboService.TabIndex = 1;
            //
            // lblService
            //
            this.lblService.AutoSize = true;
            this.lblService.Location = new System.Drawing.Point(12, 31);
            this.lblService.Name = "lblService";
            this.lblService.Size = new System.Drawing.Size(55, 15);
            this.lblService.TabIndex = 0;
            this.lblService.Text = "Dịch vụ:*";
            //
            // dgvUsages
            //
            this.dgvUsages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUsages.Location = new System.Drawing.Point(0, 176);
            this.dgvUsages.Name = "dgvUsages";
            this.dgvUsages.Size = new System.Drawing.Size(944, 279);
            this.dgvUsages.TabIndex = 2;
            this.dgvUsages.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUsages_CellDoubleClick);
            this.dgvUsages.SelectionChanged += new System.EventHandler(this.dgvUsages_SelectionChanged);
            //
            // pnlFooter
            //
            this.pnlFooter.Controls.Add(this.btnClose);
            this.pnlFooter.Controls.Add(this.btnRefresh);
            this.pnlFooter.Controls.Add(this.lblTotal);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 455);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(944, 46);
            this.pnlFooter.TabIndex = 3;
            //
            // btnClose
            //
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(842, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 30);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            //
            // btnRefresh
            //
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(746, 8);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 30);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            //
            // lblTotal
            //
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotal.Location = new System.Drawing.Point(12, 12);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(700, 22);
            this.lblTotal.TabIndex = 0;
            //
            // frmServiceUsage
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 501);
            this.Controls.Add(this.dgvUsages);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.grpInput);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(960, 480);
            this.Name = "frmServiceUsage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dịch vụ của phòng";
            this.Load += new System.EventHandler(this.frmServiceUsage_Load);
            this.pnlHeader.ResumeLayout(false);
            this.grpInput.ResumeLayout(false);
            this.grpInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsages)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblRoom;
        private System.Windows.Forms.Label lblStay;
        private System.Windows.Forms.Label lblLock;
        private System.Windows.Forms.GroupBox grpInput;
        private System.Windows.Forms.Label lblService;
        private System.Windows.Forms.ComboBox cboService;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.NumericUpDown nudQuantity;
        private System.Windows.Forms.Label lblUsedAt;
        private System.Windows.Forms.DateTimePicker dtpUsedAt;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSaveEdit;
        private System.Windows.Forms.Button btnCancelEdit;
        private System.Windows.Forms.DataGridView dgvUsages;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
    }
}
