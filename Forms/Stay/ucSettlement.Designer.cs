namespace Hotel_management_system.Forms.Stay
{
    partial class ucSettlement
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpDiscount = new System.Windows.Forms.GroupBox();
            this.txtDiscountReason = new System.Windows.Forms.TextBox();
            this.lblDiscountReason = new System.Windows.Forms.Label();
            this.lblDiscountPercent = new System.Windows.Forms.Label();
            this.nudDiscount = new System.Windows.Forms.NumericUpDown();
            this.lblDiscount = new System.Windows.Forms.Label();
            this.grpBuyer = new System.Windows.Forms.GroupBox();
            this.txtBuyerAddress = new System.Windows.Forms.TextBox();
            this.lblBuyerAddress = new System.Windows.Forms.Label();
            this.txtBuyerTaxCode = new System.Windows.Forms.TextBox();
            this.lblBuyerTaxCode = new System.Windows.Forms.Label();
            this.txtBuyerCompany = new System.Windows.Forms.TextBox();
            this.lblBuyerCompany = new System.Windows.Forms.Label();
            this.txtBuyerName = new System.Windows.Forms.TextBox();
            this.lblBuyerName = new System.Windows.Forms.Label();
            this.grpMoney = new System.Windows.Forms.GroupBox();
            this.txtRefundReference = new System.Windows.Forms.TextBox();
            this.lblRefundReference = new System.Windows.Forms.Label();
            this.cboRefundMethod = new System.Windows.Forms.ComboBox();
            this.lblRefundMethod = new System.Windows.Forms.Label();
            this.txtPayReference = new System.Windows.Forms.TextBox();
            this.lblPayReference = new System.Windows.Forms.Label();
            this.cboPayMethod = new System.Windows.Forms.ComboBox();
            this.lblPayMethod = new System.Windows.Forms.Label();
            this.nudPayAmount = new System.Windows.Forms.NumericUpDown();
            this.lblPayAmount = new System.Windows.Forms.Label();
            this.lblBalance = new System.Windows.Forms.Label();
            this.grpDiscount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDiscount)).BeginInit();
            this.grpBuyer.SuspendLayout();
            this.grpMoney.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPayAmount)).BeginInit();
            this.SuspendLayout();
            //
            // grpDiscount
            //
            this.grpDiscount.Controls.Add(this.txtDiscountReason);
            this.grpDiscount.Controls.Add(this.lblDiscountReason);
            this.grpDiscount.Controls.Add(this.lblDiscountPercent);
            this.grpDiscount.Controls.Add(this.nudDiscount);
            this.grpDiscount.Controls.Add(this.lblDiscount);
            this.grpDiscount.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpDiscount.Location = new System.Drawing.Point(0, 0);
            this.grpDiscount.Name = "grpDiscount";
            this.grpDiscount.Size = new System.Drawing.Size(440, 84);
            this.grpDiscount.TabIndex = 0;
            this.grpDiscount.TabStop = false;
            this.grpDiscount.Text = "Giảm giá";
            //
            // txtDiscountReason
            //
            this.txtDiscountReason.Location = new System.Drawing.Point(110, 52);
            this.txtDiscountReason.MaxLength = 200;
            this.txtDiscountReason.Name = "txtDiscountReason";
            this.txtDiscountReason.Size = new System.Drawing.Size(316, 23);
            this.txtDiscountReason.TabIndex = 4;
            //
            // lblDiscountReason
            //
            this.lblDiscountReason.AutoSize = true;
            this.lblDiscountReason.Location = new System.Drawing.Point(10, 55);
            this.lblDiscountReason.Name = "lblDiscountReason";
            this.lblDiscountReason.Size = new System.Drawing.Size(39, 15);
            this.lblDiscountReason.TabIndex = 3;
            this.lblDiscountReason.Text = "Lý do:";
            //
            // lblDiscountPercent
            //
            this.lblDiscountPercent.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblDiscountPercent.Location = new System.Drawing.Point(266, 25);
            this.lblDiscountPercent.Name = "lblDiscountPercent";
            this.lblDiscountPercent.Size = new System.Drawing.Size(160, 18);
            this.lblDiscountPercent.TabIndex = 2;
            //
            // nudDiscount
            //
            this.nudDiscount.Location = new System.Drawing.Point(110, 22);
            this.nudDiscount.Name = "nudDiscount";
            this.nudDiscount.Size = new System.Drawing.Size(150, 23);
            this.nudDiscount.TabIndex = 1;
            this.nudDiscount.ValueChanged += new System.EventHandler(this.nudDiscount_ValueChanged);
            //
            // lblDiscount
            //
            this.lblDiscount.AutoSize = true;
            this.lblDiscount.Location = new System.Drawing.Point(10, 25);
            this.lblDiscount.Name = "lblDiscount";
            this.lblDiscount.Size = new System.Drawing.Size(77, 15);
            this.lblDiscount.TabIndex = 0;
            this.lblDiscount.Text = "Số tiền (đ):";
            //
            // grpBuyer
            //
            this.grpBuyer.Controls.Add(this.txtBuyerAddress);
            this.grpBuyer.Controls.Add(this.lblBuyerAddress);
            this.grpBuyer.Controls.Add(this.txtBuyerTaxCode);
            this.grpBuyer.Controls.Add(this.lblBuyerTaxCode);
            this.grpBuyer.Controls.Add(this.txtBuyerCompany);
            this.grpBuyer.Controls.Add(this.lblBuyerCompany);
            this.grpBuyer.Controls.Add(this.txtBuyerName);
            this.grpBuyer.Controls.Add(this.lblBuyerName);
            this.grpBuyer.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpBuyer.Location = new System.Drawing.Point(0, 84);
            this.grpBuyer.Name = "grpBuyer";
            this.grpBuyer.Size = new System.Drawing.Size(440, 142);
            this.grpBuyer.TabIndex = 1;
            this.grpBuyer.TabStop = false;
            this.grpBuyer.Text = "Người mua trên hóa đơn (không bắt buộc)";
            //
            // txtBuyerAddress
            //
            this.txtBuyerAddress.Location = new System.Drawing.Point(110, 109);
            this.txtBuyerAddress.MaxLength = 255;
            this.txtBuyerAddress.Name = "txtBuyerAddress";
            this.txtBuyerAddress.Size = new System.Drawing.Size(316, 23);
            this.txtBuyerAddress.TabIndex = 7;
            //
            // lblBuyerAddress
            //
            this.lblBuyerAddress.AutoSize = true;
            this.lblBuyerAddress.Location = new System.Drawing.Point(10, 112);
            this.lblBuyerAddress.Name = "lblBuyerAddress";
            this.lblBuyerAddress.Size = new System.Drawing.Size(46, 15);
            this.lblBuyerAddress.TabIndex = 6;
            this.lblBuyerAddress.Text = "Địa chỉ:";
            //
            // txtBuyerTaxCode
            //
            this.txtBuyerTaxCode.Location = new System.Drawing.Point(110, 80);
            this.txtBuyerTaxCode.MaxLength = 20;
            this.txtBuyerTaxCode.Name = "txtBuyerTaxCode";
            this.txtBuyerTaxCode.Size = new System.Drawing.Size(150, 23);
            this.txtBuyerTaxCode.TabIndex = 5;
            //
            // lblBuyerTaxCode
            //
            this.lblBuyerTaxCode.AutoSize = true;
            this.lblBuyerTaxCode.Location = new System.Drawing.Point(10, 83);
            this.lblBuyerTaxCode.Name = "lblBuyerTaxCode";
            this.lblBuyerTaxCode.Size = new System.Drawing.Size(73, 15);
            this.lblBuyerTaxCode.TabIndex = 4;
            this.lblBuyerTaxCode.Text = "Mã số thuế:";
            //
            // txtBuyerCompany
            //
            this.txtBuyerCompany.Location = new System.Drawing.Point(110, 51);
            this.txtBuyerCompany.MaxLength = 200;
            this.txtBuyerCompany.Name = "txtBuyerCompany";
            this.txtBuyerCompany.Size = new System.Drawing.Size(316, 23);
            this.txtBuyerCompany.TabIndex = 3;
            //
            // lblBuyerCompany
            //
            this.lblBuyerCompany.AutoSize = true;
            this.lblBuyerCompany.Location = new System.Drawing.Point(10, 54);
            this.lblBuyerCompany.Name = "lblBuyerCompany";
            this.lblBuyerCompany.Size = new System.Drawing.Size(75, 15);
            this.lblBuyerCompany.TabIndex = 2;
            this.lblBuyerCompany.Text = "Tên đơn vị:";
            //
            // txtBuyerName
            //
            this.txtBuyerName.Location = new System.Drawing.Point(110, 22);
            this.txtBuyerName.MaxLength = 150;
            this.txtBuyerName.Name = "txtBuyerName";
            this.txtBuyerName.Size = new System.Drawing.Size(316, 23);
            this.txtBuyerName.TabIndex = 1;
            //
            // lblBuyerName
            //
            this.lblBuyerName.AutoSize = true;
            this.lblBuyerName.Location = new System.Drawing.Point(10, 25);
            this.lblBuyerName.Name = "lblBuyerName";
            this.lblBuyerName.Size = new System.Drawing.Size(90, 15);
            this.lblBuyerName.TabIndex = 0;
            this.lblBuyerName.Text = "Họ tên người mua:";
            //
            // grpMoney
            //
            this.grpMoney.Controls.Add(this.txtRefundReference);
            this.grpMoney.Controls.Add(this.lblRefundReference);
            this.grpMoney.Controls.Add(this.cboRefundMethod);
            this.grpMoney.Controls.Add(this.lblRefundMethod);
            this.grpMoney.Controls.Add(this.txtPayReference);
            this.grpMoney.Controls.Add(this.lblPayReference);
            this.grpMoney.Controls.Add(this.cboPayMethod);
            this.grpMoney.Controls.Add(this.lblPayMethod);
            this.grpMoney.Controls.Add(this.nudPayAmount);
            this.grpMoney.Controls.Add(this.lblPayAmount);
            this.grpMoney.Controls.Add(this.lblBalance);
            this.grpMoney.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpMoney.Location = new System.Drawing.Point(0, 226);
            this.grpMoney.Name = "grpMoney";
            this.grpMoney.Size = new System.Drawing.Size(440, 154);
            this.grpMoney.TabIndex = 2;
            this.grpMoney.TabStop = false;
            this.grpMoney.Text = "Thu / hoàn tiền";
            //
            // txtRefundReference
            //
            this.txtRefundReference.Location = new System.Drawing.Point(110, 80);
            this.txtRefundReference.MaxLength = 50;
            this.txtRefundReference.Name = "txtRefundReference";
            this.txtRefundReference.Size = new System.Drawing.Size(316, 23);
            this.txtRefundReference.TabIndex = 10;
            //
            // lblRefundReference
            //
            this.lblRefundReference.AutoSize = true;
            this.lblRefundReference.Location = new System.Drawing.Point(10, 83);
            this.lblRefundReference.Name = "lblRefundReference";
            this.lblRefundReference.Size = new System.Drawing.Size(83, 15);
            this.lblRefundReference.TabIndex = 9;
            this.lblRefundReference.Text = "Mã giao dịch:";
            //
            // cboRefundMethod
            //
            this.cboRefundMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRefundMethod.Location = new System.Drawing.Point(110, 51);
            this.cboRefundMethod.Name = "cboRefundMethod";
            this.cboRefundMethod.Size = new System.Drawing.Size(180, 23);
            this.cboRefundMethod.TabIndex = 8;
            this.cboRefundMethod.SelectedIndexChanged += new System.EventHandler(this.method_Changed);
            //
            // lblRefundMethod
            //
            this.lblRefundMethod.AutoSize = true;
            this.lblRefundMethod.Location = new System.Drawing.Point(10, 54);
            this.lblRefundMethod.Name = "lblRefundMethod";
            this.lblRefundMethod.Size = new System.Drawing.Size(70, 15);
            this.lblRefundMethod.TabIndex = 7;
            this.lblRefundMethod.Text = "Hoàn bằng:*";
            //
            // txtPayReference
            //
            this.txtPayReference.Location = new System.Drawing.Point(110, 109);
            this.txtPayReference.MaxLength = 50;
            this.txtPayReference.Name = "txtPayReference";
            this.txtPayReference.Size = new System.Drawing.Size(316, 23);
            this.txtPayReference.TabIndex = 6;
            //
            // lblPayReference
            //
            this.lblPayReference.AutoSize = true;
            this.lblPayReference.Location = new System.Drawing.Point(10, 112);
            this.lblPayReference.Name = "lblPayReference";
            this.lblPayReference.Size = new System.Drawing.Size(83, 15);
            this.lblPayReference.TabIndex = 5;
            this.lblPayReference.Text = "Mã giao dịch:";
            //
            // cboPayMethod
            //
            this.cboPayMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPayMethod.Location = new System.Drawing.Point(110, 80);
            this.cboPayMethod.Name = "cboPayMethod";
            this.cboPayMethod.Size = new System.Drawing.Size(180, 23);
            this.cboPayMethod.TabIndex = 4;
            this.cboPayMethod.SelectedIndexChanged += new System.EventHandler(this.method_Changed);
            //
            // lblPayMethod
            //
            this.lblPayMethod.AutoSize = true;
            this.lblPayMethod.Location = new System.Drawing.Point(10, 83);
            this.lblPayMethod.Name = "lblPayMethod";
            this.lblPayMethod.Size = new System.Drawing.Size(80, 15);
            this.lblPayMethod.TabIndex = 3;
            this.lblPayMethod.Text = "Phương thức:*";
            //
            // nudPayAmount
            //
            this.nudPayAmount.Location = new System.Drawing.Point(110, 51);
            this.nudPayAmount.Name = "nudPayAmount";
            this.nudPayAmount.Size = new System.Drawing.Size(150, 23);
            this.nudPayAmount.TabIndex = 2;
            //
            // lblPayAmount
            //
            this.lblPayAmount.AutoSize = true;
            this.lblPayAmount.Location = new System.Drawing.Point(10, 54);
            this.lblPayAmount.Name = "lblPayAmount";
            this.lblPayAmount.Size = new System.Drawing.Size(76, 15);
            this.lblPayAmount.TabIndex = 1;
            this.lblPayAmount.Text = "Số thu (đ):*";
            //
            // lblBalance
            //
            this.lblBalance.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblBalance.Location = new System.Drawing.Point(10, 22);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(416, 22);
            this.lblBalance.TabIndex = 0;
            this.lblBalance.Text = "Chưa có bảng tính";
            //
            // ucSettlement
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpMoney);
            this.Controls.Add(this.grpBuyer);
            this.Controls.Add(this.grpDiscount);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "ucSettlement";
            this.Size = new System.Drawing.Size(440, 380);
            this.grpDiscount.ResumeLayout(false);
            this.grpDiscount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDiscount)).EndInit();
            this.grpBuyer.ResumeLayout(false);
            this.grpBuyer.PerformLayout();
            this.grpMoney.ResumeLayout(false);
            this.grpMoney.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPayAmount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpDiscount;
        private System.Windows.Forms.Label lblDiscount;
        private System.Windows.Forms.NumericUpDown nudDiscount;
        private System.Windows.Forms.Label lblDiscountPercent;
        private System.Windows.Forms.Label lblDiscountReason;
        private System.Windows.Forms.TextBox txtDiscountReason;
        private System.Windows.Forms.GroupBox grpBuyer;
        private System.Windows.Forms.Label lblBuyerName;
        private System.Windows.Forms.TextBox txtBuyerName;
        private System.Windows.Forms.Label lblBuyerCompany;
        private System.Windows.Forms.TextBox txtBuyerCompany;
        private System.Windows.Forms.Label lblBuyerTaxCode;
        private System.Windows.Forms.TextBox txtBuyerTaxCode;
        private System.Windows.Forms.Label lblBuyerAddress;
        private System.Windows.Forms.TextBox txtBuyerAddress;
        private System.Windows.Forms.GroupBox grpMoney;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.Label lblPayAmount;
        private System.Windows.Forms.NumericUpDown nudPayAmount;
        private System.Windows.Forms.Label lblPayMethod;
        private System.Windows.Forms.ComboBox cboPayMethod;
        private System.Windows.Forms.Label lblPayReference;
        private System.Windows.Forms.TextBox txtPayReference;
        private System.Windows.Forms.Label lblRefundMethod;
        private System.Windows.Forms.ComboBox cboRefundMethod;
        private System.Windows.Forms.Label lblRefundReference;
        private System.Windows.Forms.TextBox txtRefundReference;
    }
}
