namespace Hotel_management_system.Forms.Billing
{
    partial class dlgReissueInvoice
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
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.ucSettlement = new Hotel_management_system.Forms.Stay.ucSettlement();
            this.grpQuote = new System.Windows.Forms.GroupBox();
            this.lblStale = new System.Windows.Forms.Label();
            this.lblBalance = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblQuoteValues = new System.Windows.Forms.Label();
            this.lblQuoteCaptions = new System.Windows.Forms.Label();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnRecalc = new System.Windows.Forms.Button();
            this.pnlHeader.SuspendLayout();
            this.grpQuote.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlHeader
            //
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(904, 58);
            this.pnlHeader.TabIndex = 0;
            //
            // lblSubtitle
            //
            this.lblSubtitle.Location = new System.Drawing.Point(12, 32);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(880, 18);
            this.lblSubtitle.TabIndex = 1;
            //
            // lblTitle
            //
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(880, 24);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Lập lại hóa đơn";
            //
            // ucSettlement
            //
            this.ucSettlement.Dock = System.Windows.Forms.DockStyle.Right;
            this.ucSettlement.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ucSettlement.Location = new System.Drawing.Point(458, 58);
            this.ucSettlement.Name = "ucSettlement";
            this.ucSettlement.Padding = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.ucSettlement.Size = new System.Drawing.Size(446, 390);
            this.ucSettlement.TabIndex = 2;
            this.ucSettlement.DiscountChanged += new System.EventHandler(this.ucSettlement_DiscountChanged);
            //
            // grpQuote
            //
            this.grpQuote.Controls.Add(this.lblStale);
            this.grpQuote.Controls.Add(this.lblBalance);
            this.grpQuote.Controls.Add(this.lblTotal);
            this.grpQuote.Controls.Add(this.lblQuoteValues);
            this.grpQuote.Controls.Add(this.lblQuoteCaptions);
            this.grpQuote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpQuote.Location = new System.Drawing.Point(0, 58);
            this.grpQuote.Name = "grpQuote";
            this.grpQuote.Size = new System.Drawing.Size(458, 390);
            this.grpQuote.TabIndex = 1;
            this.grpQuote.TabStop = false;
            this.grpQuote.Text = "Hóa đơn mới (tính lại từ dữ liệu hiện tại)";
            //
            // lblStale
            //
            this.lblStale.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(96)))), ((int)(((byte)(0)))));
            this.lblStale.Location = new System.Drawing.Point(14, 300);
            this.lblStale.Name = "lblStale";
            this.lblStale.Size = new System.Drawing.Size(430, 18);
            this.lblStale.TabIndex = 4;
            this.lblStale.Text = "Giảm giá đã đổi – bấm \"Tính lại\" trước khi lập hóa đơn.";
            this.lblStale.Visible = false;
            //
            // lblBalance
            //
            this.lblBalance.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblBalance.Location = new System.Drawing.Point(14, 268);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(430, 24);
            this.lblBalance.TabIndex = 3;
            //
            // lblTotal
            //
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(78)))), ((int)(((byte)(121)))));
            this.lblTotal.Location = new System.Drawing.Point(14, 238);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(430, 28);
            this.lblTotal.TabIndex = 2;
            //
            // lblQuoteValues
            //
            this.lblQuoteValues.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lblQuoteValues.Location = new System.Drawing.Point(240, 26);
            this.lblQuoteValues.Name = "lblQuoteValues";
            this.lblQuoteValues.Size = new System.Drawing.Size(204, 200);
            this.lblQuoteValues.TabIndex = 1;
            this.lblQuoteValues.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // lblQuoteCaptions
            //
            this.lblQuoteCaptions.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lblQuoteCaptions.Location = new System.Drawing.Point(14, 26);
            this.lblQuoteCaptions.Name = "lblQuoteCaptions";
            this.lblQuoteCaptions.Size = new System.Drawing.Size(230, 200);
            this.lblQuoteCaptions.TabIndex = 0;
            //
            // pnlButtons
            //
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnOk);
            this.pnlButtons.Controls.Add(this.btnRecalc);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 448);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(904, 52);
            this.pnlButtons.TabIndex = 3;
            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(802, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 32);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = true;
            //
            // btnOk
            //
            this.btnOk.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnOk.Location = new System.Drawing.Point(636, 10);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(160, 32);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Lập hóa đơn mới";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            //
            // btnRecalc
            //
            this.btnRecalc.Location = new System.Drawing.Point(530, 10);
            this.btnRecalc.Name = "btnRecalc";
            this.btnRecalc.Size = new System.Drawing.Size(100, 32);
            this.btnRecalc.TabIndex = 0;
            this.btnRecalc.Text = "Tính lại";
            this.btnRecalc.UseVisualStyleBackColor = true;
            this.btnRecalc.Click += new System.EventHandler(this.btnRecalc_Click);
            //
            // dlgReissueInvoice
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(904, 500);
            this.Controls.Add(this.grpQuote);
            this.Controls.Add(this.ucSettlement);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "dlgReissueInvoice";
            this.Text = "Lập lại hóa đơn";
            this.Load += new System.EventHandler(this.dlgReissueInvoice_Load);
            this.pnlHeader.ResumeLayout(false);
            this.grpQuote.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.GroupBox grpQuote;
        private System.Windows.Forms.Label lblQuoteCaptions;
        private System.Windows.Forms.Label lblQuoteValues;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.Label lblStale;
        private Hotel_management_system.Forms.Stay.ucSettlement ucSettlement;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnRecalc;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}
