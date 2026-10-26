namespace Hotel_management_system.Forms.Billing
{
    partial class dlgPayment
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
            this.grpInfo = new System.Windows.Forms.GroupBox();
            this.lblValue3 = new System.Windows.Forms.Label();
            this.lblCaption3 = new System.Windows.Forms.Label();
            this.lblValue2 = new System.Windows.Forms.Label();
            this.lblCaption2 = new System.Windows.Forms.Label();
            this.lblValue1 = new System.Windows.Forms.Label();
            this.lblCaption1 = new System.Windows.Forms.Label();
            this.lblAmount = new System.Windows.Forms.Label();
            this.nudAmount = new System.Windows.Forms.NumericUpDown();
            this.lblAmountHint = new System.Windows.Forms.Label();
            this.lblMethod = new System.Windows.Forms.Label();
            this.cboMethod = new System.Windows.Forms.ComboBox();
            this.lblReference = new System.Windows.Forms.Label();
            this.txtReference = new System.Windows.Forms.TextBox();
            this.lblNote = new System.Windows.Forms.Label();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAmount)).BeginInit();
            this.SuspendLayout();
            //
            // lblTitle
            //
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(456, 22);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Thu tiền";
            //
            // lblSubtitle
            //
            this.lblSubtitle.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblSubtitle.Location = new System.Drawing.Point(12, 34);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(456, 18);
            this.lblSubtitle.TabIndex = 1;
            //
            // grpInfo
            //
            this.grpInfo.Controls.Add(this.lblValue3);
            this.grpInfo.Controls.Add(this.lblCaption3);
            this.grpInfo.Controls.Add(this.lblValue2);
            this.grpInfo.Controls.Add(this.lblCaption2);
            this.grpInfo.Controls.Add(this.lblValue1);
            this.grpInfo.Controls.Add(this.lblCaption1);
            this.grpInfo.Location = new System.Drawing.Point(12, 58);
            this.grpInfo.Name = "grpInfo";
            this.grpInfo.Size = new System.Drawing.Size(456, 96);
            this.grpInfo.TabIndex = 2;
            this.grpInfo.TabStop = false;
            this.grpInfo.Text = "Sổ tiền";
            //
            // lblValue3
            //
            this.lblValue3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblValue3.Location = new System.Drawing.Point(220, 66);
            this.lblValue3.Name = "lblValue3";
            this.lblValue3.Size = new System.Drawing.Size(220, 18);
            this.lblValue3.TabIndex = 5;
            this.lblValue3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // lblCaption3
            //
            this.lblCaption3.Location = new System.Drawing.Point(14, 66);
            this.lblCaption3.Name = "lblCaption3";
            this.lblCaption3.Size = new System.Drawing.Size(200, 18);
            this.lblCaption3.TabIndex = 4;
            //
            // lblValue2
            //
            this.lblValue2.Location = new System.Drawing.Point(220, 44);
            this.lblValue2.Name = "lblValue2";
            this.lblValue2.Size = new System.Drawing.Size(220, 18);
            this.lblValue2.TabIndex = 3;
            this.lblValue2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // lblCaption2
            //
            this.lblCaption2.Location = new System.Drawing.Point(14, 44);
            this.lblCaption2.Name = "lblCaption2";
            this.lblCaption2.Size = new System.Drawing.Size(200, 18);
            this.lblCaption2.TabIndex = 2;
            //
            // lblValue1
            //
            this.lblValue1.Location = new System.Drawing.Point(220, 22);
            this.lblValue1.Name = "lblValue1";
            this.lblValue1.Size = new System.Drawing.Size(220, 18);
            this.lblValue1.TabIndex = 1;
            this.lblValue1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // lblCaption1
            //
            this.lblCaption1.Location = new System.Drawing.Point(14, 22);
            this.lblCaption1.Name = "lblCaption1";
            this.lblCaption1.Size = new System.Drawing.Size(200, 18);
            this.lblCaption1.TabIndex = 0;
            //
            // lblAmount
            //
            this.lblAmount.AutoSize = true;
            this.lblAmount.Location = new System.Drawing.Point(12, 169);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(70, 15);
            this.lblAmount.TabIndex = 3;
            this.lblAmount.Text = "Số tiền (đ):*";
            //
            // nudAmount
            //
            this.nudAmount.Location = new System.Drawing.Point(130, 166);
            this.nudAmount.Name = "nudAmount";
            this.nudAmount.Size = new System.Drawing.Size(170, 23);
            this.nudAmount.TabIndex = 4;
            //
            // lblAmountHint
            //
            this.lblAmountHint.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblAmountHint.Location = new System.Drawing.Point(306, 169);
            this.lblAmountHint.Name = "lblAmountHint";
            this.lblAmountHint.Size = new System.Drawing.Size(162, 18);
            this.lblAmountHint.TabIndex = 5;
            //
            // lblMethod
            //
            this.lblMethod.AutoSize = true;
            this.lblMethod.Location = new System.Drawing.Point(12, 203);
            this.lblMethod.Name = "lblMethod";
            this.lblMethod.Size = new System.Drawing.Size(73, 15);
            this.lblMethod.TabIndex = 6;
            this.lblMethod.Text = "Phương thức:";
            //
            // cboMethod
            //
            this.cboMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMethod.Location = new System.Drawing.Point(130, 200);
            this.cboMethod.Name = "cboMethod";
            this.cboMethod.Size = new System.Drawing.Size(170, 23);
            this.cboMethod.TabIndex = 7;
            this.cboMethod.SelectedIndexChanged += new System.EventHandler(this.cboMethod_SelectedIndexChanged);
            //
            // lblReference
            //
            this.lblReference.AutoSize = true;
            this.lblReference.Location = new System.Drawing.Point(12, 237);
            this.lblReference.Name = "lblReference";
            this.lblReference.Size = new System.Drawing.Size(83, 15);
            this.lblReference.TabIndex = 8;
            this.lblReference.Text = "Mã giao dịch:";
            //
            // txtReference
            //
            this.txtReference.Location = new System.Drawing.Point(130, 234);
            this.txtReference.MaxLength = 50;
            this.txtReference.Name = "txtReference";
            this.txtReference.Size = new System.Drawing.Size(338, 23);
            this.txtReference.TabIndex = 9;
            //
            // lblNote
            //
            this.lblNote.AutoSize = true;
            this.lblNote.Location = new System.Drawing.Point(12, 271);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(51, 15);
            this.lblNote.TabIndex = 10;
            this.lblNote.Text = "Ghi chú:";
            //
            // txtNote
            //
            this.txtNote.Location = new System.Drawing.Point(130, 268);
            this.txtNote.MaxLength = 200;
            this.txtNote.Multiline = true;
            this.txtNote.Name = "txtNote";
            this.txtNote.Size = new System.Drawing.Size(338, 48);
            this.txtNote.TabIndex = 11;
            //
            // btnOk
            //
            this.btnOk.Location = new System.Drawing.Point(282, 330);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 30);
            this.btnOk.TabIndex = 12;
            this.btnOk.Text = "Ghi nhận";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(378, 330);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = true;
            //
            // dlgPayment
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 372);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblSubtitle);
            this.Controls.Add(this.grpInfo);
            this.Controls.Add(this.lblAmount);
            this.Controls.Add(this.nudAmount);
            this.Controls.Add(this.lblAmountHint);
            this.Controls.Add(this.lblMethod);
            this.Controls.Add(this.cboMethod);
            this.Controls.Add(this.lblReference);
            this.Controls.Add(this.txtReference);
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.txtNote);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "dlgPayment";
            this.Text = "Thu tiền";
            this.Load += new System.EventHandler(this.dlgPayment_Load);
            this.grpInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudAmount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.GroupBox grpInfo;
        private System.Windows.Forms.Label lblCaption1;
        private System.Windows.Forms.Label lblValue1;
        private System.Windows.Forms.Label lblCaption2;
        private System.Windows.Forms.Label lblValue2;
        private System.Windows.Forms.Label lblCaption3;
        private System.Windows.Forms.Label lblValue3;
        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.NumericUpDown nudAmount;
        private System.Windows.Forms.Label lblAmountHint;
        private System.Windows.Forms.Label lblMethod;
        private System.Windows.Forms.ComboBox cboMethod;
        private System.Windows.Forms.Label lblReference;
        private System.Windows.Forms.TextBox txtReference;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}
