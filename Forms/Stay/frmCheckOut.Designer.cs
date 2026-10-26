namespace Hotel_management_system.Forms.Stay
{
    partial class frmCheckOut
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
            this.components = new System.ComponentModel.Container();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblStay = new System.Windows.Forms.Label();
            this.lblRoom = new System.Windows.Forms.Label();
            this.ucSettlement = new Hotel_management_system.Forms.Stay.ucSettlement();
            this.grpQuote = new System.Windows.Forms.GroupBox();
            this.btnExtend = new System.Windows.Forms.Button();
            this.lblMustExtend = new System.Windows.Forms.Label();
            this.lblStale = new System.Windows.Forms.Label();
            this.lblValidity = new System.Windows.Forms.Label();
            this.lblBalance = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblQuoteValues = new System.Windows.Forms.Label();
            this.lblQuoteCaptions = new System.Windows.Forms.Label();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnServices = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnRecalc = new System.Windows.Forms.Button();
            this.timerValidity = new System.Windows.Forms.Timer(this.components);
            this.pnlHeader.SuspendLayout();
            this.grpQuote.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlHeader
            //
            this.pnlHeader.Controls.Add(this.lblStay);
            this.pnlHeader.Controls.Add(this.lblRoom);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(944, 58);
            this.pnlHeader.TabIndex = 0;
            //
            // lblStay
            //
            this.lblStay.Location = new System.Drawing.Point(12, 32);
            this.lblStay.Name = "lblStay";
            this.lblStay.Size = new System.Drawing.Size(920, 18);
            this.lblStay.TabIndex = 1;
            //
            // lblRoom
            //
            this.lblRoom.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblRoom.Location = new System.Drawing.Point(12, 6);
            this.lblRoom.Name = "lblRoom";
            this.lblRoom.Size = new System.Drawing.Size(920, 24);
            this.lblRoom.TabIndex = 0;
            this.lblRoom.Text = "Trả phòng";
            //
            // ucSettlement
            //
            this.ucSettlement.Dock = System.Windows.Forms.DockStyle.Right;
            this.ucSettlement.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ucSettlement.Location = new System.Drawing.Point(498, 58);
            this.ucSettlement.Name = "ucSettlement";
            this.ucSettlement.Padding = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.ucSettlement.Size = new System.Drawing.Size(446, 400);
            this.ucSettlement.TabIndex = 2;
            this.ucSettlement.DiscountChanged += new System.EventHandler(this.ucSettlement_DiscountChanged);
            //
            // grpQuote
            //
            this.grpQuote.Controls.Add(this.btnExtend);
            this.grpQuote.Controls.Add(this.lblMustExtend);
            this.grpQuote.Controls.Add(this.lblStale);
            this.grpQuote.Controls.Add(this.lblValidity);
            this.grpQuote.Controls.Add(this.lblBalance);
            this.grpQuote.Controls.Add(this.lblTotal);
            this.grpQuote.Controls.Add(this.lblQuoteValues);
            this.grpQuote.Controls.Add(this.lblQuoteCaptions);
            this.grpQuote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpQuote.Location = new System.Drawing.Point(0, 58);
            this.grpQuote.Name = "grpQuote";
            this.grpQuote.Size = new System.Drawing.Size(498, 400);
            this.grpQuote.TabIndex = 1;
            this.grpQuote.TabStop = false;
            this.grpQuote.Text = "Bảng tính trả phòng";
            //
            // btnExtend
            //
            this.btnExtend.Location = new System.Drawing.Point(330, 356);
            this.btnExtend.Name = "btnExtend";
            this.btnExtend.Size = new System.Drawing.Size(150, 30);
            this.btnExtend.TabIndex = 7;
            this.btnExtend.Text = "Gia hạn thêm đêm...";
            this.btnExtend.UseVisualStyleBackColor = true;
            this.btnExtend.Visible = false;
            this.btnExtend.Click += new System.EventHandler(this.btnExtend_Click);
            //
            // lblMustExtend
            //
            this.lblMustExtend.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblMustExtend.Location = new System.Drawing.Point(14, 352);
            this.lblMustExtend.Name = "lblMustExtend";
            this.lblMustExtend.Size = new System.Drawing.Size(310, 38);
            this.lblMustExtend.TabIndex = 6;
            this.lblMustExtend.Text = "Trả phòng sau 18:00 ngày trả: phải gia hạn thêm 1 đêm trước khi check-out.";
            this.lblMustExtend.Visible = false;
            //
            // lblStale
            //
            this.lblStale.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(96)))), ((int)(((byte)(0)))));
            this.lblStale.Location = new System.Drawing.Point(14, 330);
            this.lblStale.Name = "lblStale";
            this.lblStale.Size = new System.Drawing.Size(466, 18);
            this.lblStale.TabIndex = 5;
            this.lblStale.Text = "Giảm giá đã đổi – bấm \"Tính lại\" trước khi xác nhận.";
            this.lblStale.Visible = false;
            //
            // lblValidity
            //
            this.lblValidity.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblValidity.Location = new System.Drawing.Point(14, 308);
            this.lblValidity.Name = "lblValidity";
            this.lblValidity.Size = new System.Drawing.Size(466, 18);
            this.lblValidity.TabIndex = 4;
            //
            // lblBalance
            //
            this.lblBalance.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblBalance.Location = new System.Drawing.Point(14, 278);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(466, 24);
            this.lblBalance.TabIndex = 3;
            //
            // lblTotal
            //
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(78)))), ((int)(((byte)(121)))));
            this.lblTotal.Location = new System.Drawing.Point(14, 248);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(466, 28);
            this.lblTotal.TabIndex = 2;
            //
            // lblQuoteValues
            //
            this.lblQuoteValues.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lblQuoteValues.Location = new System.Drawing.Point(250, 26);
            this.lblQuoteValues.Name = "lblQuoteValues";
            this.lblQuoteValues.Size = new System.Drawing.Size(230, 216);
            this.lblQuoteValues.TabIndex = 1;
            this.lblQuoteValues.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // lblQuoteCaptions
            //
            this.lblQuoteCaptions.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lblQuoteCaptions.Location = new System.Drawing.Point(14, 26);
            this.lblQuoteCaptions.Name = "lblQuoteCaptions";
            this.lblQuoteCaptions.Size = new System.Drawing.Size(240, 216);
            this.lblQuoteCaptions.TabIndex = 0;
            //
            // pnlButtons
            //
            this.pnlButtons.Controls.Add(this.btnServices);
            this.pnlButtons.Controls.Add(this.btnClose);
            this.pnlButtons.Controls.Add(this.btnConfirm);
            this.pnlButtons.Controls.Add(this.btnRecalc);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 458);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(944, 52);
            this.pnlButtons.TabIndex = 3;
            //
            // btnServices
            //
            this.btnServices.Location = new System.Drawing.Point(12, 10);
            this.btnServices.Name = "btnServices";
            this.btnServices.Size = new System.Drawing.Size(150, 32);
            this.btnServices.TabIndex = 0;
            this.btnServices.Text = "Ghi thêm dịch vụ...";
            this.btnServices.UseVisualStyleBackColor = true;
            this.btnServices.Click += new System.EventHandler(this.btnServices_Click);
            //
            // btnClose
            //
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(842, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 32);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = true;
            //
            // btnConfirm
            //
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnConfirm.Location = new System.Drawing.Point(666, 10);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(170, 32);
            this.btnConfirm.TabIndex = 2;
            this.btnConfirm.Text = "Xác nhận trả phòng";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            //
            // btnRecalc
            //
            this.btnRecalc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRecalc.Location = new System.Drawing.Point(560, 10);
            this.btnRecalc.Name = "btnRecalc";
            this.btnRecalc.Size = new System.Drawing.Size(100, 32);
            this.btnRecalc.TabIndex = 1;
            this.btnRecalc.Text = "Tính lại";
            this.btnRecalc.UseVisualStyleBackColor = true;
            this.btnRecalc.Click += new System.EventHandler(this.btnRecalc_Click);
            //
            // timerValidity
            //
            this.timerValidity.Interval = 1000;
            this.timerValidity.Tick += new System.EventHandler(this.timerValidity_Tick);
            //
            // frmCheckOut
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(944, 510);
            this.Controls.Add(this.grpQuote);
            this.Controls.Add(this.ucSettlement);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCheckOut";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Trả phòng";
            this.Load += new System.EventHandler(this.frmCheckOut_Load);
            this.pnlHeader.ResumeLayout(false);
            this.grpQuote.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblRoom;
        private System.Windows.Forms.Label lblStay;
        private System.Windows.Forms.GroupBox grpQuote;
        private System.Windows.Forms.Label lblQuoteCaptions;
        private System.Windows.Forms.Label lblQuoteValues;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.Label lblValidity;
        private System.Windows.Forms.Label lblStale;
        private System.Windows.Forms.Label lblMustExtend;
        private System.Windows.Forms.Button btnExtend;
        private Hotel_management_system.Forms.Stay.ucSettlement ucSettlement;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnServices;
        private System.Windows.Forms.Button btnRecalc;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Timer timerValidity;
    }
}
