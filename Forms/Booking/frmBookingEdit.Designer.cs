namespace Hotel_management_system.Forms.Booking
{
    partial class frmBookingEdit
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
            this.grpBooking = new System.Windows.Forms.GroupBox();
            this.lblBookingInfo = new System.Windows.Forms.Label();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.lblNote = new System.Windows.Forms.Label();
            this.cboDepositRate = new System.Windows.Forms.ComboBox();
            this.lblDepositRate = new System.Windows.Forms.Label();
            this.btnPickCustomer = new System.Windows.Forms.Button();
            this.txtCustomer = new System.Windows.Forms.TextBox();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.dgvAvailable = new System.Windows.Forms.DataGridView();
            this.pnlSearchInputs = new System.Windows.Forms.Panel();
            this.btnUpdateRoom = new System.Windows.Forms.Button();
            this.btnAddRoom = new System.Windows.Forms.Button();
            this.txtReason = new System.Windows.Forms.TextBox();
            this.chkKeepPrice = new System.Windows.Forms.CheckBox();
            this.txtGuestIdNumber = new System.Windows.Forms.TextBox();
            this.lblGuestIdNumber = new System.Windows.Forms.Label();
            this.txtGuestName = new System.Windows.Forms.TextBox();
            this.lblGuestName = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.nudChildren = new System.Windows.Forms.NumericUpDown();
            this.lblChildren = new System.Windows.Forms.Label();
            this.nudAdults = new System.Windows.Forms.NumericUpDown();
            this.lblAdults = new System.Windows.Forms.Label();
            this.cboRoomType = new System.Windows.Forms.ComboBox();
            this.lblRoomType = new System.Windows.Forms.Label();
            this.lblNights = new System.Windows.Forms.Label();
            this.dtpCheckOut = new System.Windows.Forms.DateTimePicker();
            this.lblCheckOut = new System.Windows.Forms.Label();
            this.dtpCheckIn = new System.Windows.Forms.DateTimePicker();
            this.lblCheckIn = new System.Windows.Forms.Label();
            this.grpRooms = new System.Windows.Forms.GroupBox();
            this.dgvRooms = new System.Windows.Forms.DataGridView();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnDeposit = new System.Windows.Forms.Button();
            this.lblTotals = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRemoveRoom = new System.Windows.Forms.Button();
            this.grpBooking.SuspendLayout();
            this.grpSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvailable)).BeginInit();
            this.pnlSearchInputs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudChildren)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAdults)).BeginInit();
            this.grpRooms.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRooms)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            //
            // grpBooking
            //
            this.grpBooking.Controls.Add(this.lblBookingInfo);
            this.grpBooking.Controls.Add(this.txtNote);
            this.grpBooking.Controls.Add(this.lblNote);
            this.grpBooking.Controls.Add(this.cboDepositRate);
            this.grpBooking.Controls.Add(this.lblDepositRate);
            this.grpBooking.Controls.Add(this.btnPickCustomer);
            this.grpBooking.Controls.Add(this.txtCustomer);
            this.grpBooking.Controls.Add(this.lblCustomer);
            this.grpBooking.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpBooking.Location = new System.Drawing.Point(0, 0);
            this.grpBooking.Name = "grpBooking";
            this.grpBooking.Size = new System.Drawing.Size(1104, 96);
            this.grpBooking.TabIndex = 0;
            this.grpBooking.TabStop = false;
            this.grpBooking.Text = "Phiếu đặt";
            //
            // lblBookingInfo
            //
            this.lblBookingInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblBookingInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(78)))), ((int)(((byte)(121)))));
            this.lblBookingInfo.Location = new System.Drawing.Point(560, 26);
            this.lblBookingInfo.Name = "lblBookingInfo";
            this.lblBookingInfo.Size = new System.Drawing.Size(530, 22);
            this.lblBookingInfo.TabIndex = 7;
            //
            // txtNote
            //
            this.txtNote.Location = new System.Drawing.Point(110, 60);
            this.txtNote.MaxLength = 500;
            this.txtNote.Name = "txtNote";
            this.txtNote.Size = new System.Drawing.Size(980, 23);
            this.txtNote.TabIndex = 6;
            //
            // lblNote
            //
            this.lblNote.AutoSize = true;
            this.lblNote.Location = new System.Drawing.Point(12, 63);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(51, 15);
            this.lblNote.TabIndex = 5;
            this.lblNote.Text = "Ghi chú:";
            //
            // cboDepositRate
            //
            this.cboDepositRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDepositRate.Location = new System.Drawing.Point(450, 24);
            this.cboDepositRate.Name = "cboDepositRate";
            this.cboDepositRate.Size = new System.Drawing.Size(100, 23);
            this.cboDepositRate.TabIndex = 4;
            this.cboDepositRate.SelectedIndexChanged += new System.EventHandler(this.cboDepositRate_SelectedIndexChanged);
            //
            // lblDepositRate
            //
            this.lblDepositRate.AutoSize = true;
            this.lblDepositRate.Location = new System.Drawing.Point(390, 27);
            this.lblDepositRate.Name = "lblDepositRate";
            this.lblDepositRate.Size = new System.Drawing.Size(56, 15);
            this.lblDepositRate.TabIndex = 3;
            this.lblDepositRate.Text = "Tỷ lệ cọc:";
            //
            // btnPickCustomer
            //
            this.btnPickCustomer.Location = new System.Drawing.Point(300, 23);
            this.btnPickCustomer.Name = "btnPickCustomer";
            this.btnPickCustomer.Size = new System.Drawing.Size(76, 25);
            this.btnPickCustomer.TabIndex = 2;
            this.btnPickCustomer.Text = "Chọn...";
            this.btnPickCustomer.UseVisualStyleBackColor = true;
            this.btnPickCustomer.Click += new System.EventHandler(this.btnPickCustomer_Click);
            //
            // txtCustomer
            //
            this.txtCustomer.Location = new System.Drawing.Point(110, 24);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.ReadOnly = true;
            this.txtCustomer.Size = new System.Drawing.Size(184, 23);
            this.txtCustomer.TabIndex = 1;
            //
            // lblCustomer
            //
            this.lblCustomer.AutoSize = true;
            this.lblCustomer.Location = new System.Drawing.Point(12, 27);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(69, 15);
            this.lblCustomer.TabIndex = 0;
            this.lblCustomer.Text = "Khách đặt:*";
            //
            // grpSearch
            //
            this.grpSearch.Controls.Add(this.dgvAvailable);
            this.grpSearch.Controls.Add(this.pnlSearchInputs);
            this.grpSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSearch.Location = new System.Drawing.Point(0, 96);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.Size = new System.Drawing.Size(1104, 286);
            this.grpSearch.TabIndex = 1;
            this.grpSearch.TabStop = false;
            this.grpSearch.Text = "Tìm phòng trống";
            //
            // dgvAvailable
            //
            this.dgvAvailable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAvailable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAvailable.Location = new System.Drawing.Point(3, 113);
            this.dgvAvailable.Name = "dgvAvailable";
            this.dgvAvailable.Size = new System.Drawing.Size(1098, 170);
            this.dgvAvailable.TabIndex = 1;
            this.dgvAvailable.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAvailable_CellDoubleClick);
            //
            // pnlSearchInputs
            //
            this.pnlSearchInputs.Controls.Add(this.btnUpdateRoom);
            this.pnlSearchInputs.Controls.Add(this.btnAddRoom);
            this.pnlSearchInputs.Controls.Add(this.txtReason);
            this.pnlSearchInputs.Controls.Add(this.chkKeepPrice);
            this.pnlSearchInputs.Controls.Add(this.txtGuestIdNumber);
            this.pnlSearchInputs.Controls.Add(this.lblGuestIdNumber);
            this.pnlSearchInputs.Controls.Add(this.txtGuestName);
            this.pnlSearchInputs.Controls.Add(this.lblGuestName);
            this.pnlSearchInputs.Controls.Add(this.btnSearch);
            this.pnlSearchInputs.Controls.Add(this.nudChildren);
            this.pnlSearchInputs.Controls.Add(this.lblChildren);
            this.pnlSearchInputs.Controls.Add(this.nudAdults);
            this.pnlSearchInputs.Controls.Add(this.lblAdults);
            this.pnlSearchInputs.Controls.Add(this.cboRoomType);
            this.pnlSearchInputs.Controls.Add(this.lblRoomType);
            this.pnlSearchInputs.Controls.Add(this.lblNights);
            this.pnlSearchInputs.Controls.Add(this.dtpCheckOut);
            this.pnlSearchInputs.Controls.Add(this.lblCheckOut);
            this.pnlSearchInputs.Controls.Add(this.dtpCheckIn);
            this.pnlSearchInputs.Controls.Add(this.lblCheckIn);
            this.pnlSearchInputs.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearchInputs.Location = new System.Drawing.Point(3, 19);
            this.pnlSearchInputs.Name = "pnlSearchInputs";
            this.pnlSearchInputs.Size = new System.Drawing.Size(1098, 94);
            this.pnlSearchInputs.TabIndex = 0;
            //
            // btnUpdateRoom
            //
            this.btnUpdateRoom.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnUpdateRoom.Location = new System.Drawing.Point(860, 56);
            this.btnUpdateRoom.Name = "btnUpdateRoom";
            this.btnUpdateRoom.Size = new System.Drawing.Size(226, 30);
            this.btnUpdateRoom.TabIndex = 19;
            this.btnUpdateRoom.Text = "Cập nhật phòng đang chọn ↓";
            this.btnUpdateRoom.UseVisualStyleBackColor = true;
            this.btnUpdateRoom.Click += new System.EventHandler(this.btnUpdateRoom_Click);
            //
            // btnAddRoom
            //
            this.btnAddRoom.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddRoom.Location = new System.Drawing.Point(860, 56);
            this.btnAddRoom.Name = "btnAddRoom";
            this.btnAddRoom.Size = new System.Drawing.Size(226, 30);
            this.btnAddRoom.TabIndex = 18;
            this.btnAddRoom.Text = "Thêm phòng vào phiếu ↓";
            this.btnAddRoom.UseVisualStyleBackColor = true;
            this.btnAddRoom.Click += new System.EventHandler(this.btnAddRoom_Click);
            //
            // txtReason
            //
            this.txtReason.Location = new System.Drawing.Point(560, 60);
            this.txtReason.MaxLength = 200;
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(290, 23);
            this.txtReason.TabIndex = 17;
            //
            // chkKeepPrice
            //
            this.chkKeepPrice.AutoSize = true;
            this.chkKeepPrice.Location = new System.Drawing.Point(330, 62);
            this.chkKeepPrice.Name = "chkKeepPrice";
            this.chkKeepPrice.Size = new System.Drawing.Size(225, 19);
            this.chkKeepPrice.TabIndex = 16;
            this.chkKeepPrice.Text = "Giữ giá cũ (KS đổi phòng) – lý do:";
            this.chkKeepPrice.UseVisualStyleBackColor = true;
            //
            // txtGuestIdNumber
            //
            this.txtGuestIdNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtGuestIdNumber.Location = new System.Drawing.Point(560, 60);
            this.txtGuestIdNumber.MaxLength = 20;
            this.txtGuestIdNumber.Name = "txtGuestIdNumber";
            this.txtGuestIdNumber.Size = new System.Drawing.Size(150, 23);
            this.txtGuestIdNumber.TabIndex = 15;
            //
            // lblGuestIdNumber
            //
            this.lblGuestIdNumber.AutoSize = true;
            this.lblGuestIdNumber.Location = new System.Drawing.Point(452, 63);
            this.lblGuestIdNumber.Name = "lblGuestIdNumber";
            this.lblGuestIdNumber.Size = new System.Drawing.Size(104, 15);
            this.lblGuestIdNumber.TabIndex = 14;
            this.lblGuestIdNumber.Text = "CCCD người ở:";
            //
            // txtGuestName
            //
            this.txtGuestName.Location = new System.Drawing.Point(110, 60);
            this.txtGuestName.MaxLength = 100;
            this.txtGuestName.Name = "txtGuestName";
            this.txtGuestName.Size = new System.Drawing.Size(330, 23);
            this.txtGuestName.TabIndex = 13;
            //
            // lblGuestName
            //
            this.lblGuestName.AutoSize = true;
            this.lblGuestName.Location = new System.Drawing.Point(9, 63);
            this.lblGuestName.Name = "lblGuestName";
            this.lblGuestName.Size = new System.Drawing.Size(95, 15);
            this.lblGuestName.TabIndex = 12;
            this.lblGuestName.Text = "Người ở (nếu khác):";
            //
            // btnSearch
            //
            this.btnSearch.Location = new System.Drawing.Point(960, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(126, 27);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "Tìm phòng trống";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            //
            // nudChildren
            //
            this.nudChildren.Location = new System.Drawing.Point(900, 14);
            this.nudChildren.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.nudChildren.Name = "nudChildren";
            this.nudChildren.Size = new System.Drawing.Size(48, 23);
            this.nudChildren.TabIndex = 10;
            //
            // lblChildren
            //
            this.lblChildren.AutoSize = true;
            this.lblChildren.Location = new System.Drawing.Point(850, 17);
            this.lblChildren.Name = "lblChildren";
            this.lblChildren.Size = new System.Drawing.Size(45, 15);
            this.lblChildren.TabIndex = 9;
            this.lblChildren.Text = "Trẻ em:";
            //
            // nudAdults
            //
            this.nudAdults.Location = new System.Drawing.Point(792, 14);
            this.nudAdults.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudAdults.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudAdults.Name = "nudAdults";
            this.nudAdults.Size = new System.Drawing.Size(48, 23);
            this.nudAdults.TabIndex = 8;
            this.nudAdults.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            //
            // lblAdults
            //
            this.lblAdults.AutoSize = true;
            this.lblAdults.Location = new System.Drawing.Point(728, 17);
            this.lblAdults.Name = "lblAdults";
            this.lblAdults.Size = new System.Drawing.Size(62, 15);
            this.lblAdults.TabIndex = 7;
            this.lblAdults.Text = "Người lớn:";
            //
            // cboRoomType
            //
            this.cboRoomType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRoomType.Location = new System.Drawing.Point(560, 14);
            this.cboRoomType.Name = "cboRoomType";
            this.cboRoomType.Size = new System.Drawing.Size(160, 23);
            this.cboRoomType.TabIndex = 6;
            //
            // lblRoomType
            //
            this.lblRoomType.AutoSize = true;
            this.lblRoomType.Location = new System.Drawing.Point(486, 17);
            this.lblRoomType.Name = "lblRoomType";
            this.lblRoomType.Size = new System.Drawing.Size(68, 15);
            this.lblRoomType.TabIndex = 5;
            this.lblRoomType.Text = "Loại phòng:";
            //
            // lblNights
            //
            this.lblNights.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblNights.Location = new System.Drawing.Point(420, 17);
            this.lblNights.Name = "lblNights";
            this.lblNights.Size = new System.Drawing.Size(60, 18);
            this.lblNights.TabIndex = 4;
            this.lblNights.Text = "1 đêm";
            //
            // dtpCheckOut
            //
            this.dtpCheckOut.CustomFormat = "dd/MM/yyyy";
            this.dtpCheckOut.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpCheckOut.Location = new System.Drawing.Point(300, 14);
            this.dtpCheckOut.Name = "dtpCheckOut";
            this.dtpCheckOut.Size = new System.Drawing.Size(110, 23);
            this.dtpCheckOut.TabIndex = 3;
            this.dtpCheckOut.ValueChanged += new System.EventHandler(this.dates_ValueChanged);
            //
            // lblCheckOut
            //
            this.lblCheckOut.AutoSize = true;
            this.lblCheckOut.Location = new System.Drawing.Point(234, 17);
            this.lblCheckOut.Name = "lblCheckOut";
            this.lblCheckOut.Size = new System.Drawing.Size(58, 15);
            this.lblCheckOut.TabIndex = 2;
            this.lblCheckOut.Text = "Ngày trả:";
            //
            // dtpCheckIn
            //
            this.dtpCheckIn.CustomFormat = "dd/MM/yyyy";
            this.dtpCheckIn.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpCheckIn.Location = new System.Drawing.Point(110, 14);
            this.dtpCheckIn.Name = "dtpCheckIn";
            this.dtpCheckIn.Size = new System.Drawing.Size(110, 23);
            this.dtpCheckIn.TabIndex = 1;
            this.dtpCheckIn.ValueChanged += new System.EventHandler(this.dates_ValueChanged);
            //
            // lblCheckIn
            //
            this.lblCheckIn.AutoSize = true;
            this.lblCheckIn.Location = new System.Drawing.Point(9, 17);
            this.lblCheckIn.Name = "lblCheckIn";
            this.lblCheckIn.Size = new System.Drawing.Size(66, 15);
            this.lblCheckIn.TabIndex = 0;
            this.lblCheckIn.Text = "Ngày nhận:";
            //
            // grpRooms
            //
            this.grpRooms.Controls.Add(this.dgvRooms);
            this.grpRooms.Controls.Add(this.pnlBottom);
            this.grpRooms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpRooms.Location = new System.Drawing.Point(0, 382);
            this.grpRooms.Name = "grpRooms";
            this.grpRooms.Size = new System.Drawing.Size(1104, 299);
            this.grpRooms.TabIndex = 2;
            this.grpRooms.TabStop = false;
            this.grpRooms.Text = "Phòng trong phiếu";
            //
            // dgvRooms
            //
            this.dgvRooms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRooms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRooms.Location = new System.Drawing.Point(3, 19);
            this.dgvRooms.Name = "dgvRooms";
            this.dgvRooms.Size = new System.Drawing.Size(1098, 227);
            this.dgvRooms.TabIndex = 0;
            this.dgvRooms.SelectionChanged += new System.EventHandler(this.dgvRooms_SelectionChanged);
            //
            // pnlBottom
            //
            this.pnlBottom.Controls.Add(this.btnDeposit);
            this.pnlBottom.Controls.Add(this.lblTotals);
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Controls.Add(this.btnSave);
            this.pnlBottom.Controls.Add(this.btnRemoveRoom);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(3, 246);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1098, 50);
            this.pnlBottom.TabIndex = 1;
            //
            // btnDeposit
            //
            this.btnDeposit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeposit.Location = new System.Drawing.Point(764, 10);
            this.btnDeposit.Name = "btnDeposit";
            this.btnDeposit.Size = new System.Drawing.Size(100, 32);
            this.btnDeposit.TabIndex = 2;
            this.btnDeposit.Text = "Thu cọc";
            this.btnDeposit.UseVisualStyleBackColor = true;
            this.btnDeposit.Click += new System.EventHandler(this.btnDeposit_Click);
            //
            // lblTotals
            //
            this.lblTotals.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotals.Location = new System.Drawing.Point(140, 6);
            this.lblTotals.Name = "lblTotals";
            this.lblTotals.Size = new System.Drawing.Size(610, 40);
            this.lblTotals.TabIndex = 1;
            this.lblTotals.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // btnClose
            //
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(990, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 32);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            //
            // btnSave
            //
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(870, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(114, 32);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Lưu phiếu đặt";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // btnRemoveRoom
            //
            this.btnRemoveRoom.Location = new System.Drawing.Point(8, 10);
            this.btnRemoveRoom.Name = "btnRemoveRoom";
            this.btnRemoveRoom.Size = new System.Drawing.Size(120, 32);
            this.btnRemoveRoom.TabIndex = 0;
            this.btnRemoveRoom.Text = "Bỏ phòng đã chọn";
            this.btnRemoveRoom.UseVisualStyleBackColor = true;
            this.btnRemoveRoom.Click += new System.EventHandler(this.btnRemoveRoom_Click);
            //
            // frmBookingEdit
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1104, 681);
            this.Controls.Add(this.grpRooms);
            this.Controls.Add(this.grpSearch);
            this.Controls.Add(this.grpBooking);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1120, 640);
            this.Name = "frmBookingEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Phiếu đặt phòng";
            this.Load += new System.EventHandler(this.frmBookingEdit_Load);
            this.grpBooking.ResumeLayout(false);
            this.grpBooking.PerformLayout();
            this.grpSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvailable)).EndInit();
            this.pnlSearchInputs.ResumeLayout(false);
            this.pnlSearchInputs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudChildren)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAdults)).EndInit();
            this.grpRooms.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRooms)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBooking;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.TextBox txtCustomer;
        private System.Windows.Forms.Button btnPickCustomer;
        private System.Windows.Forms.Label lblDepositRate;
        private System.Windows.Forms.ComboBox cboDepositRate;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.Label lblBookingInfo;
        private System.Windows.Forms.GroupBox grpSearch;
        private System.Windows.Forms.Panel pnlSearchInputs;
        private System.Windows.Forms.Label lblCheckIn;
        private System.Windows.Forms.DateTimePicker dtpCheckIn;
        private System.Windows.Forms.Label lblCheckOut;
        private System.Windows.Forms.DateTimePicker dtpCheckOut;
        private System.Windows.Forms.Label lblNights;
        private System.Windows.Forms.Label lblRoomType;
        private System.Windows.Forms.ComboBox cboRoomType;
        private System.Windows.Forms.Label lblAdults;
        private System.Windows.Forms.NumericUpDown nudAdults;
        private System.Windows.Forms.Label lblChildren;
        private System.Windows.Forms.NumericUpDown nudChildren;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblGuestName;
        private System.Windows.Forms.TextBox txtGuestName;
        private System.Windows.Forms.Label lblGuestIdNumber;
        private System.Windows.Forms.TextBox txtGuestIdNumber;
        private System.Windows.Forms.CheckBox chkKeepPrice;
        private System.Windows.Forms.TextBox txtReason;
        private System.Windows.Forms.Button btnAddRoom;
        private System.Windows.Forms.Button btnUpdateRoom;
        private System.Windows.Forms.DataGridView dgvAvailable;
        private System.Windows.Forms.GroupBox grpRooms;
        private System.Windows.Forms.DataGridView dgvRooms;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnRemoveRoom;
        private System.Windows.Forms.Label lblTotals;
        private System.Windows.Forms.Button btnDeposit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
    }
}
