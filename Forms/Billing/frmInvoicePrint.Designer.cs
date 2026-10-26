namespace Hotel_management_system.Forms.Billing
{
    partial class frmInvoicePrint
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblInfo = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.ppcInvoice = new System.Windows.Forms.PrintPreviewControl();
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.printDialog = new System.Windows.Forms.PrintDialog();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlTop
            //
            this.pnlTop.Controls.Add(this.lblInfo);
            this.pnlTop.Controls.Add(this.btnClose);
            this.pnlTop.Controls.Add(this.btnZoomOut);
            this.pnlTop.Controls.Add(this.btnZoomIn);
            this.pnlTop.Controls.Add(this.btnPrint);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(884, 46);
            this.pnlTop.TabIndex = 0;
            //
            // lblInfo
            //
            this.lblInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblInfo.Location = new System.Drawing.Point(330, 14);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(440, 18);
            this.lblInfo.TabIndex = 3;
            //
            // btnClose
            //
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(782, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 30);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            //
            // btnZoomOut
            //
            this.btnZoomOut.Location = new System.Drawing.Point(222, 8);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(90, 30);
            this.btnZoomOut.TabIndex = 2;
            this.btnZoomOut.Text = "Thu nhỏ";
            this.btnZoomOut.UseVisualStyleBackColor = true;
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            //
            // btnZoomIn
            //
            this.btnZoomIn.Location = new System.Drawing.Point(126, 8);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(90, 30);
            this.btnZoomIn.TabIndex = 1;
            this.btnZoomIn.Text = "Phóng to";
            this.btnZoomIn.UseVisualStyleBackColor = true;
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            //
            // btnPrint
            //
            this.btnPrint.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnPrint.Location = new System.Drawing.Point(12, 8);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(108, 30);
            this.btnPrint.TabIndex = 0;
            this.btnPrint.Text = "In hóa đơn...";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            //
            // ppcInvoice
            //
            this.ppcInvoice.AutoZoom = true;
            this.ppcInvoice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ppcInvoice.Location = new System.Drawing.Point(0, 46);
            this.ppcInvoice.Name = "ppcInvoice";
            this.ppcInvoice.Size = new System.Drawing.Size(884, 715);
            this.ppcInvoice.TabIndex = 1;
            //
            // printDocument
            //
            this.printDocument.DocumentName = "HoaDon";
            this.printDocument.BeginPrint += new System.Drawing.Printing.PrintEventHandler(this.printDocument_BeginPrint);
            this.printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument_PrintPage);
            //
            // printDialog
            //
            this.printDialog.UseEXDialog = true;
            //
            // frmInvoicePrint
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(884, 761);
            this.Controls.Add(this.ppcInvoice);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(700, 600);
            this.Name = "frmInvoicePrint";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Hóa đơn";
            this.Load += new System.EventHandler(this.frmInvoicePrint_Load);
            this.pnlTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnZoomIn;
        private System.Windows.Forms.Button btnZoomOut;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.PrintPreviewControl ppcInvoice;
        private System.Drawing.Printing.PrintDocument printDocument;
        private System.Windows.Forms.PrintDialog printDialog;
    }
}
