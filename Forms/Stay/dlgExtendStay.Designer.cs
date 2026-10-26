namespace Hotel_management_system.Forms.Stay
{
    partial class dlgExtendStay
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblGuest = new System.Windows.Forms.Label();
            this.lblCurrentCaption = new System.Windows.Forms.Label();
            this.lblCurrent = new System.Windows.Forms.Label();
            this.lblPriceCaption = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblNewCaption = new System.Windows.Forms.Label();
            this.dtpNewCheckOut = new System.Windows.Forms.DateTimePicker();
            this.lblExtra = new System.Windows.Forms.Label();
            this.lblNextBooking = new System.Windows.Forms.Label();
            this.lblReason = new System.Windows.Forms.Label();
            this.txtReason = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblTitle
            //
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(436, 22);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Gia hạn lưu trú";
            //
            // lblGuest
            //
            this.lblGuest.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblGuest.Location = new System.Drawing.Point(12, 34);
            this.lblGuest.Name = "lblGuest";
            this.lblGuest.Size = new System.Drawing.Size(436, 18);
            this.lblGuest.TabIndex = 1;
            //
            // lblCurrentCaption
            //
            this.lblCurrentCaption.AutoSize = true;
            this.lblCurrentCaption.Location = new System.Drawing.Point(12, 66);
            this.lblCurrentCaption.Name = "lblCurrentCaption";
            this.lblCurrentCaption.Size = new System.Drawing.Size(113, 15);
            this.lblCurrentCaption.TabIndex = 2;
            this.lblCurrentCaption.Text = "Ngày trả hiện tại:";
            //
            // lblCurrent
            //
            this.lblCurrent.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCurrent.Location = new System.Drawing.Point(150, 66);
            this.lblCurrent.Name = "lblCurrent";
            this.lblCurrent.Size = new System.Drawing.Size(298, 18);
            this.lblCurrent.TabIndex = 3;
            //
            // lblPriceCaption
            //
            this.lblPriceCaption.AutoSize = true;
            this.lblPriceCaption.Location = new System.Drawing.Point(12, 90);
            this.lblPriceCaption.Name = "lblPriceCaption";
            this.lblPriceCaption.Size = new System.Drawing.Size(102, 15);
            this.lblPriceCaption.TabIndex = 4;
            this.lblPriceCaption.Text = "Giá/đêm đã chốt:";
            //
            // lblPrice
            //
            this.lblPrice.Location = new System.Drawing.Point(150, 90);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(298, 18);
            this.lblPrice.TabIndex = 5;
            //
            // lblNewCaption
            //
            this.lblNewCaption.AutoSize = true;
            this.lblNewCaption.Location = new System.Drawing.Point(12, 121);
            this.lblNewCaption.Name = "lblNewCaption";
            this.lblNewCaption.Size = new System.Drawing.Size(93, 15);
            this.lblNewCaption.TabIndex = 6;
            this.lblNewCaption.Text = "Ngày trả mới:*";
            //
            // dtpNewCheckOut
            //
            this.dtpNewCheckOut.CustomFormat = "dd/MM/yyyy";
            this.dtpNewCheckOut.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNewCheckOut.Location = new System.Drawing.Point(150, 118);
            this.dtpNewCheckOut.Name = "dtpNewCheckOut";
            this.dtpNewCheckOut.Size = new System.Drawing.Size(130, 23);
            this.dtpNewCheckOut.TabIndex = 7;
            this.dtpNewCheckOut.ValueChanged += new System.EventHandler(this.dtpNewCheckOut_ValueChanged);
            //
            // lblExtra
            //
            this.lblExtra.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblExtra.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(78)))), ((int)(((byte)(121)))));
            this.lblExtra.Location = new System.Drawing.Point(150, 146);
            this.lblExtra.Name = "lblExtra";
            this.lblExtra.Size = new System.Drawing.Size(298, 18);
            this.lblExtra.TabIndex = 8;
            //
            // lblNextBooking
            //
            this.lblNextBooking.Location = new System.Drawing.Point(150, 166);
            this.lblNextBooking.Name = "lblNextBooking";
            this.lblNextBooking.Size = new System.Drawing.Size(298, 34);
            this.lblNextBooking.TabIndex = 9;
            //
            // lblReason
            //
            this.lblReason.AutoSize = true;
            this.lblReason.Location = new System.Drawing.Point(12, 209);
            this.lblReason.Name = "lblReason";
            this.lblReason.Size = new System.Drawing.Size(39, 15);
            this.lblReason.TabIndex = 10;
            this.lblReason.Text = "Lý do:";
            //
            // txtReason
            //
            this.txtReason.Location = new System.Drawing.Point(150, 206);
            this.txtReason.MaxLength = 200;
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(298, 23);
            this.txtReason.TabIndex = 11;
            //
            // btnOk
            //
            this.btnOk.Location = new System.Drawing.Point(262, 246);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 30);
            this.btnOk.TabIndex = 12;
            this.btnOk.Text = "Gia hạn";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(358, 246);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = true;
            //
            // dlgExtendStay
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 288);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblGuest);
            this.Controls.Add(this.lblCurrentCaption);
            this.Controls.Add(this.lblCurrent);
            this.Controls.Add(this.lblPriceCaption);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.lblNewCaption);
            this.Controls.Add(this.dtpNewCheckOut);
            this.Controls.Add(this.lblExtra);
            this.Controls.Add(this.lblNextBooking);
            this.Controls.Add(this.lblReason);
            this.Controls.Add(this.txtReason);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "dlgExtendStay";
            this.Text = "Gia hạn lưu trú";
            this.Load += new System.EventHandler(this.dlgExtendStay_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblGuest;
        private System.Windows.Forms.Label lblCurrentCaption;
        private System.Windows.Forms.Label lblCurrent;
        private System.Windows.Forms.Label lblPriceCaption;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblNewCaption;
        private System.Windows.Forms.DateTimePicker dtpNewCheckOut;
        private System.Windows.Forms.Label lblExtra;
        private System.Windows.Forms.Label lblNextBooking;
        private System.Windows.Forms.Label lblReason;
        private System.Windows.Forms.TextBox txtReason;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}
