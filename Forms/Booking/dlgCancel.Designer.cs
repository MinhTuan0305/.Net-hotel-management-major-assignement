namespace Hotel_management_system.Forms.Booking
{
    partial class dlgCancel
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
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.dgvRooms = new System.Windows.Forms.DataGridView();
            this.lblPolicy = new System.Windows.Forms.Label();
            this.chkNoShow = new System.Windows.Forms.CheckBox();
            this.lblReason = new System.Windows.Forms.Label();
            this.txtReason = new System.Windows.Forms.TextBox();
            this.grpRefund = new System.Windows.Forms.GroupBox();
            this.txtRefundReference = new System.Windows.Forms.TextBox();
            this.lblRefundReference = new System.Windows.Forms.Label();
            this.cboRefundMethod = new System.Windows.Forms.ComboBox();
            this.lblRefundMethod = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRooms)).BeginInit();
            this.grpRefund.SuspendLayout();
            this.SuspendLayout();
            //
            // lblTitle
            //
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(576, 22);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Hủy đặt phòng";
            //
            // lblSubtitle
            //
            this.lblSubtitle.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblSubtitle.Location = new System.Drawing.Point(12, 34);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(576, 18);
            this.lblSubtitle.TabIndex = 1;
            //
            // dgvRooms
            //
            this.dgvRooms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRooms.Location = new System.Drawing.Point(12, 58);
            this.dgvRooms.Name = "dgvRooms";
            this.dgvRooms.Size = new System.Drawing.Size(576, 120);
            this.dgvRooms.TabIndex = 2;
            //
            // lblPolicy
            //
            this.lblPolicy.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPolicy.Location = new System.Drawing.Point(12, 184);
            this.lblPolicy.Name = "lblPolicy";
            this.lblPolicy.Size = new System.Drawing.Size(576, 38);
            this.lblPolicy.TabIndex = 3;
            //
            // chkNoShow
            //
            this.chkNoShow.AutoSize = true;
            this.chkNoShow.Location = new System.Drawing.Point(110, 226);
            this.chkNoShow.Name = "chkNoShow";
            this.chkNoShow.Size = new System.Drawing.Size(268, 19);
            this.chkNoShow.TabIndex = 4;
            this.chkNoShow.Text = "Khách không đến (no-show) – không hoàn tiền";
            this.chkNoShow.UseVisualStyleBackColor = true;
            this.chkNoShow.CheckedChanged += new System.EventHandler(this.chkNoShow_CheckedChanged);
            //
            // lblReason
            //
            this.lblReason.AutoSize = true;
            this.lblReason.Location = new System.Drawing.Point(12, 255);
            this.lblReason.Name = "lblReason";
            this.lblReason.Size = new System.Drawing.Size(67, 15);
            this.lblReason.TabIndex = 5;
            this.lblReason.Text = "Lý do hủy:*";
            //
            // txtReason
            //
            this.txtReason.Location = new System.Drawing.Point(110, 252);
            this.txtReason.MaxLength = 200;
            this.txtReason.Multiline = true;
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(478, 44);
            this.txtReason.TabIndex = 6;
            //
            // grpRefund
            //
            this.grpRefund.Controls.Add(this.txtRefundReference);
            this.grpRefund.Controls.Add(this.lblRefundReference);
            this.grpRefund.Controls.Add(this.cboRefundMethod);
            this.grpRefund.Controls.Add(this.lblRefundMethod);
            this.grpRefund.Location = new System.Drawing.Point(12, 304);
            this.grpRefund.Name = "grpRefund";
            this.grpRefund.Size = new System.Drawing.Size(576, 62);
            this.grpRefund.TabIndex = 7;
            this.grpRefund.TabStop = false;
            this.grpRefund.Text = "Hoàn tiền";
            //
            // txtRefundReference
            //
            this.txtRefundReference.Location = new System.Drawing.Point(388, 24);
            this.txtRefundReference.MaxLength = 50;
            this.txtRefundReference.Name = "txtRefundReference";
            this.txtRefundReference.Size = new System.Drawing.Size(176, 23);
            this.txtRefundReference.TabIndex = 3;
            //
            // lblRefundReference
            //
            this.lblRefundReference.AutoSize = true;
            this.lblRefundReference.Location = new System.Drawing.Point(296, 27);
            this.lblRefundReference.Name = "lblRefundReference";
            this.lblRefundReference.Size = new System.Drawing.Size(83, 15);
            this.lblRefundReference.TabIndex = 2;
            this.lblRefundReference.Text = "Mã giao dịch:";
            //
            // cboRefundMethod
            //
            this.cboRefundMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRefundMethod.Location = new System.Drawing.Point(98, 24);
            this.cboRefundMethod.Name = "cboRefundMethod";
            this.cboRefundMethod.Size = new System.Drawing.Size(180, 23);
            this.cboRefundMethod.TabIndex = 1;
            //
            // lblRefundMethod
            //
            this.lblRefundMethod.AutoSize = true;
            this.lblRefundMethod.Location = new System.Drawing.Point(10, 27);
            this.lblRefundMethod.Name = "lblRefundMethod";
            this.lblRefundMethod.Size = new System.Drawing.Size(65, 15);
            this.lblRefundMethod.TabIndex = 0;
            this.lblRefundMethod.Text = "Hoàn bằng:";
            //
            // btnOk
            //
            this.btnOk.Location = new System.Drawing.Point(388, 378);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 30);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "Xác nhận hủy";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(494, 378);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 30);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Đóng";
            this.btnCancel.UseVisualStyleBackColor = true;
            //
            // dlgCancel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 420);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblSubtitle);
            this.Controls.Add(this.dgvRooms);
            this.Controls.Add(this.lblPolicy);
            this.Controls.Add(this.chkNoShow);
            this.Controls.Add(this.lblReason);
            this.Controls.Add(this.txtReason);
            this.Controls.Add(this.grpRefund);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "dlgCancel";
            this.Text = "Hủy đặt phòng";
            this.Load += new System.EventHandler(this.dlgCancel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRooms)).EndInit();
            this.grpRefund.ResumeLayout(false);
            this.grpRefund.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.DataGridView dgvRooms;
        private System.Windows.Forms.Label lblPolicy;
        private System.Windows.Forms.CheckBox chkNoShow;
        private System.Windows.Forms.Label lblReason;
        private System.Windows.Forms.TextBox txtReason;
        private System.Windows.Forms.GroupBox grpRefund;
        private System.Windows.Forms.Label lblRefundMethod;
        private System.Windows.Forms.ComboBox cboRefundMethod;
        private System.Windows.Forms.Label lblRefundReference;
        private System.Windows.Forms.TextBox txtRefundReference;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}
