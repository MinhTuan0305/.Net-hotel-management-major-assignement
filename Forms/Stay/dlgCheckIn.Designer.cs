namespace Hotel_management_system.Forms.Stay
{
    partial class dlgCheckIn
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
            this.lblGuests = new System.Windows.Forms.Label();
            this.lblGuestsCaption = new System.Windows.Forms.Label();
            this.lblStay = new System.Windows.Forms.Label();
            this.lblStayCaption = new System.Windows.Forms.Label();
            this.lblRoom = new System.Windows.Forms.Label();
            this.lblRoomCaption = new System.Windows.Forms.Label();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.lblCustomerCaption = new System.Windows.Forms.Label();
            this.lblBookingId = new System.Windows.Forms.Label();
            this.lblBookingIdCaption = new System.Windows.Forms.Label();
            this.grpConditions = new System.Windows.Forms.GroupBox();
            this.lblRoomStatusText = new System.Windows.Forms.Label();
            this.lblRoomStatusMark = new System.Windows.Forms.Label();
            this.lblDateText = new System.Windows.Forms.Label();
            this.lblDateMark = new System.Windows.Forms.Label();
            this.btnDeposit = new System.Windows.Forms.Button();
            this.lblDepositText = new System.Windows.Forms.Label();
            this.lblDepositMark = new System.Windows.Forms.Label();
            this.lblBookingStatusText = new System.Windows.Forms.Label();
            this.lblBookingStatusMark = new System.Windows.Forms.Label();
            this.grpGuest = new System.Windows.Forms.GroupBox();
            this.lblGuestHint = new System.Windows.Forms.Label();
            this.txtGuestIdNumber = new System.Windows.Forms.TextBox();
            this.lblGuestIdCaption = new System.Windows.Forms.Label();
            this.txtGuestName = new System.Windows.Forms.TextBox();
            this.lblGuestNameCaption = new System.Windows.Forms.Label();
            this.lblNow = new System.Windows.Forms.Label();
            this.btnReload = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpBooking.SuspendLayout();
            this.grpConditions.SuspendLayout();
            this.grpGuest.SuspendLayout();
            this.SuspendLayout();
            //
            // grpBooking
            //
            this.grpBooking.Controls.Add(this.lblGuests);
            this.grpBooking.Controls.Add(this.lblGuestsCaption);
            this.grpBooking.Controls.Add(this.lblStay);
            this.grpBooking.Controls.Add(this.lblStayCaption);
            this.grpBooking.Controls.Add(this.lblRoom);
            this.grpBooking.Controls.Add(this.lblRoomCaption);
            this.grpBooking.Controls.Add(this.lblCustomer);
            this.grpBooking.Controls.Add(this.lblCustomerCaption);
            this.grpBooking.Controls.Add(this.lblBookingId);
            this.grpBooking.Controls.Add(this.lblBookingIdCaption);
            this.grpBooking.Location = new System.Drawing.Point(12, 12);
            this.grpBooking.Name = "grpBooking";
            this.grpBooking.Size = new System.Drawing.Size(536, 150);
            this.grpBooking.TabIndex = 0;
            this.grpBooking.TabStop = false;
            this.grpBooking.Text = "Thông tin đặt phòng";
            //
            // lblBookingIdCaption
            //
            this.lblBookingIdCaption.AutoSize = true;
            this.lblBookingIdCaption.Location = new System.Drawing.Point(14, 26);
            this.lblBookingIdCaption.Name = "lblBookingIdCaption";
            this.lblBookingIdCaption.Size = new System.Drawing.Size(56, 15);
            this.lblBookingIdCaption.TabIndex = 0;
            this.lblBookingIdCaption.Text = "Mã phiếu:";
            //
            // lblBookingId
            //
            this.lblBookingId.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblBookingId.Location = new System.Drawing.Point(110, 26);
            this.lblBookingId.Name = "lblBookingId";
            this.lblBookingId.Size = new System.Drawing.Size(410, 20);
            this.lblBookingId.TabIndex = 1;
            this.lblBookingId.Text = "-";
            //
            // lblCustomerCaption
            //
            this.lblCustomerCaption.AutoSize = true;
            this.lblCustomerCaption.Location = new System.Drawing.Point(14, 50);
            this.lblCustomerCaption.Name = "lblCustomerCaption";
            this.lblCustomerCaption.Size = new System.Drawing.Size(61, 15);
            this.lblCustomerCaption.TabIndex = 2;
            this.lblCustomerCaption.Text = "Người đặt:";
            //
            // lblCustomer
            //
            this.lblCustomer.Location = new System.Drawing.Point(110, 50);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(410, 20);
            this.lblCustomer.TabIndex = 3;
            this.lblCustomer.Text = "-";
            //
            // lblRoomCaption
            //
            this.lblRoomCaption.AutoSize = true;
            this.lblRoomCaption.Location = new System.Drawing.Point(14, 74);
            this.lblRoomCaption.Name = "lblRoomCaption";
            this.lblRoomCaption.Size = new System.Drawing.Size(42, 15);
            this.lblRoomCaption.TabIndex = 4;
            this.lblRoomCaption.Text = "Phòng:";
            //
            // lblRoom
            //
            this.lblRoom.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblRoom.Location = new System.Drawing.Point(110, 74);
            this.lblRoom.Name = "lblRoom";
            this.lblRoom.Size = new System.Drawing.Size(410, 20);
            this.lblRoom.TabIndex = 5;
            this.lblRoom.Text = "-";
            //
            // lblStayCaption
            //
            this.lblStayCaption.AutoSize = true;
            this.lblStayCaption.Location = new System.Drawing.Point(14, 98);
            this.lblStayCaption.Name = "lblStayCaption";
            this.lblStayCaption.Size = new System.Drawing.Size(61, 15);
            this.lblStayCaption.TabIndex = 6;
            this.lblStayCaption.Text = "Thời gian:";
            //
            // lblStay
            //
            this.lblStay.Location = new System.Drawing.Point(110, 98);
            this.lblStay.Name = "lblStay";
            this.lblStay.Size = new System.Drawing.Size(410, 20);
            this.lblStay.TabIndex = 7;
            this.lblStay.Text = "-";
            //
            // lblGuestsCaption
            //
            this.lblGuestsCaption.AutoSize = true;
            this.lblGuestsCaption.Location = new System.Drawing.Point(14, 122);
            this.lblGuestsCaption.Name = "lblGuestsCaption";
            this.lblGuestsCaption.Size = new System.Drawing.Size(58, 15);
            this.lblGuestsCaption.TabIndex = 8;
            this.lblGuestsCaption.Text = "Số khách:";
            //
            // lblGuests
            //
            this.lblGuests.Location = new System.Drawing.Point(110, 122);
            this.lblGuests.Name = "lblGuests";
            this.lblGuests.Size = new System.Drawing.Size(410, 20);
            this.lblGuests.TabIndex = 9;
            this.lblGuests.Text = "-";
            //
            // grpConditions
            //
            this.grpConditions.Controls.Add(this.lblRoomStatusText);
            this.grpConditions.Controls.Add(this.lblRoomStatusMark);
            this.grpConditions.Controls.Add(this.lblDateText);
            this.grpConditions.Controls.Add(this.lblDateMark);
            this.grpConditions.Controls.Add(this.btnDeposit);
            this.grpConditions.Controls.Add(this.lblDepositText);
            this.grpConditions.Controls.Add(this.lblDepositMark);
            this.grpConditions.Controls.Add(this.lblBookingStatusText);
            this.grpConditions.Controls.Add(this.lblBookingStatusMark);
            this.grpConditions.Location = new System.Drawing.Point(12, 170);
            this.grpConditions.Name = "grpConditions";
            this.grpConditions.Size = new System.Drawing.Size(536, 134);
            this.grpConditions.TabIndex = 1;
            this.grpConditions.TabStop = false;
            this.grpConditions.Text = "Điều kiện nhận phòng";
            //
            // lblBookingStatusMark
            //
            this.lblBookingStatusMark.Font = new System.Drawing.Font("Segoe UI Symbol", 11F, System.Drawing.FontStyle.Bold);
            this.lblBookingStatusMark.Location = new System.Drawing.Point(14, 22);
            this.lblBookingStatusMark.Name = "lblBookingStatusMark";
            this.lblBookingStatusMark.Size = new System.Drawing.Size(24, 24);
            this.lblBookingStatusMark.TabIndex = 0;
            this.lblBookingStatusMark.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblBookingStatusText
            //
            this.lblBookingStatusText.Location = new System.Drawing.Point(40, 22);
            this.lblBookingStatusText.Name = "lblBookingStatusText";
            this.lblBookingStatusText.Size = new System.Drawing.Size(480, 24);
            this.lblBookingStatusText.TabIndex = 1;
            this.lblBookingStatusText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblDepositMark
            //
            this.lblDepositMark.Font = new System.Drawing.Font("Segoe UI Symbol", 11F, System.Drawing.FontStyle.Bold);
            this.lblDepositMark.Location = new System.Drawing.Point(14, 49);
            this.lblDepositMark.Name = "lblDepositMark";
            this.lblDepositMark.Size = new System.Drawing.Size(24, 24);
            this.lblDepositMark.TabIndex = 2;
            this.lblDepositMark.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblDepositText
            //
            this.lblDepositText.Location = new System.Drawing.Point(40, 49);
            this.lblDepositText.Name = "lblDepositText";
            this.lblDepositText.Size = new System.Drawing.Size(372, 24);
            this.lblDepositText.TabIndex = 3;
            this.lblDepositText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // btnDeposit
            //
            this.btnDeposit.Location = new System.Drawing.Point(420, 47);
            this.btnDeposit.Name = "btnDeposit";
            this.btnDeposit.Size = new System.Drawing.Size(100, 28);
            this.btnDeposit.TabIndex = 4;
            this.btnDeposit.Text = "Thu cọc...";
            this.btnDeposit.UseVisualStyleBackColor = true;
            this.btnDeposit.Visible = false;
            this.btnDeposit.Click += new System.EventHandler(this.btnDeposit_Click);
            //
            // lblDateMark
            //
            this.lblDateMark.Font = new System.Drawing.Font("Segoe UI Symbol", 11F, System.Drawing.FontStyle.Bold);
            this.lblDateMark.Location = new System.Drawing.Point(14, 76);
            this.lblDateMark.Name = "lblDateMark";
            this.lblDateMark.Size = new System.Drawing.Size(24, 24);
            this.lblDateMark.TabIndex = 5;
            this.lblDateMark.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblDateText
            //
            this.lblDateText.Location = new System.Drawing.Point(40, 76);
            this.lblDateText.Name = "lblDateText";
            this.lblDateText.Size = new System.Drawing.Size(480, 24);
            this.lblDateText.TabIndex = 6;
            this.lblDateText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblRoomStatusMark
            //
            this.lblRoomStatusMark.Font = new System.Drawing.Font("Segoe UI Symbol", 11F, System.Drawing.FontStyle.Bold);
            this.lblRoomStatusMark.Location = new System.Drawing.Point(14, 103);
            this.lblRoomStatusMark.Name = "lblRoomStatusMark";
            this.lblRoomStatusMark.Size = new System.Drawing.Size(24, 24);
            this.lblRoomStatusMark.TabIndex = 7;
            this.lblRoomStatusMark.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblRoomStatusText
            //
            this.lblRoomStatusText.Location = new System.Drawing.Point(40, 103);
            this.lblRoomStatusText.Name = "lblRoomStatusText";
            this.lblRoomStatusText.Size = new System.Drawing.Size(480, 24);
            this.lblRoomStatusText.TabIndex = 8;
            this.lblRoomStatusText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // grpGuest
            //
            this.grpGuest.Controls.Add(this.lblGuestHint);
            this.grpGuest.Controls.Add(this.txtGuestIdNumber);
            this.grpGuest.Controls.Add(this.lblGuestIdCaption);
            this.grpGuest.Controls.Add(this.txtGuestName);
            this.grpGuest.Controls.Add(this.lblGuestNameCaption);
            this.grpGuest.Location = new System.Drawing.Point(12, 312);
            this.grpGuest.Name = "grpGuest";
            this.grpGuest.Size = new System.Drawing.Size(536, 114);
            this.grpGuest.TabIndex = 2;
            this.grpGuest.TabStop = false;
            this.grpGuest.Text = "Người ở thực tế";
            //
            // lblGuestNameCaption
            //
            this.lblGuestNameCaption.AutoSize = true;
            this.lblGuestNameCaption.Location = new System.Drawing.Point(14, 31);
            this.lblGuestNameCaption.Name = "lblGuestNameCaption";
            this.lblGuestNameCaption.Size = new System.Drawing.Size(46, 15);
            this.lblGuestNameCaption.TabIndex = 0;
            this.lblGuestNameCaption.Text = "Họ tên:";
            //
            // txtGuestName
            //
            this.txtGuestName.Location = new System.Drawing.Point(130, 28);
            this.txtGuestName.MaxLength = 100;
            this.txtGuestName.Name = "txtGuestName";
            this.txtGuestName.Size = new System.Drawing.Size(390, 23);
            this.txtGuestName.TabIndex = 1;
            //
            // lblGuestIdCaption
            //
            this.lblGuestIdCaption.AutoSize = true;
            this.lblGuestIdCaption.Location = new System.Drawing.Point(14, 62);
            this.lblGuestIdCaption.Name = "lblGuestIdCaption";
            this.lblGuestIdCaption.Size = new System.Drawing.Size(88, 15);
            this.lblGuestIdCaption.TabIndex = 2;
            this.lblGuestIdCaption.Text = "CCCD/Hộ chiếu:";
            //
            // txtGuestIdNumber
            //
            this.txtGuestIdNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtGuestIdNumber.Location = new System.Drawing.Point(130, 59);
            this.txtGuestIdNumber.MaxLength = 20;
            this.txtGuestIdNumber.Name = "txtGuestIdNumber";
            this.txtGuestIdNumber.Size = new System.Drawing.Size(200, 23);
            this.txtGuestIdNumber.TabIndex = 3;
            //
            // lblGuestHint
            //
            this.lblGuestHint.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblGuestHint.Location = new System.Drawing.Point(130, 87);
            this.lblGuestHint.Name = "lblGuestHint";
            this.lblGuestHint.Size = new System.Drawing.Size(390, 18);
            this.lblGuestHint.TabIndex = 4;
            this.lblGuestHint.Text = "Mặc định là người đặt phòng. Sửa lại nếu người ở là người khác.";
            //
            // lblNow
            //
            this.lblNow.Location = new System.Drawing.Point(12, 436);
            this.lblNow.Name = "lblNow";
            this.lblNow.Size = new System.Drawing.Size(536, 20);
            this.lblNow.TabIndex = 3;
            this.lblNow.Text = "-";
            //
            // btnReload
            //
            this.btnReload.Location = new System.Drawing.Point(12, 466);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(110, 32);
            this.btnReload.TabIndex = 4;
            this.btnReload.Text = "Kiểm tra lại";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            //
            // btnOk
            //
            this.btnOk.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnOk.Location = new System.Drawing.Point(338, 466);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(110, 32);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "Nhận phòng";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(454, 466);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 32);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Đóng";
            this.btnCancel.UseVisualStyleBackColor = true;
            //
            // dlgCheckIn
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 510);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.lblNow);
            this.Controls.Add(this.grpGuest);
            this.Controls.Add(this.grpConditions);
            this.Controls.Add(this.grpBooking);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "dlgCheckIn";
            this.Text = "Nhận phòng (check-in)";
            this.Load += new System.EventHandler(this.dlgCheckIn_Load);
            this.grpBooking.ResumeLayout(false);
            this.grpBooking.PerformLayout();
            this.grpConditions.ResumeLayout(false);
            this.grpGuest.ResumeLayout(false);
            this.grpGuest.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBooking;
        private System.Windows.Forms.Label lblBookingIdCaption;
        private System.Windows.Forms.Label lblBookingId;
        private System.Windows.Forms.Label lblCustomerCaption;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.Label lblRoomCaption;
        private System.Windows.Forms.Label lblRoom;
        private System.Windows.Forms.Label lblStayCaption;
        private System.Windows.Forms.Label lblStay;
        private System.Windows.Forms.Label lblGuestsCaption;
        private System.Windows.Forms.Label lblGuests;
        private System.Windows.Forms.GroupBox grpConditions;
        private System.Windows.Forms.Label lblBookingStatusMark;
        private System.Windows.Forms.Label lblBookingStatusText;
        private System.Windows.Forms.Label lblDepositMark;
        private System.Windows.Forms.Label lblDepositText;
        private System.Windows.Forms.Button btnDeposit;
        private System.Windows.Forms.Label lblDateMark;
        private System.Windows.Forms.Label lblDateText;
        private System.Windows.Forms.Label lblRoomStatusMark;
        private System.Windows.Forms.Label lblRoomStatusText;
        private System.Windows.Forms.GroupBox grpGuest;
        private System.Windows.Forms.Label lblGuestNameCaption;
        private System.Windows.Forms.TextBox txtGuestName;
        private System.Windows.Forms.Label lblGuestIdCaption;
        private System.Windows.Forms.TextBox txtGuestIdNumber;
        private System.Windows.Forms.Label lblGuestHint;
        private System.Windows.Forms.Label lblNow;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}
