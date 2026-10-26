namespace Hotel_management_system.Forms.Billing
{
    partial class frmInvoices
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
            this.btnRefresh = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.pnlActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnView = new System.Windows.Forms.Button();
            this.btnVoid = new System.Windows.Forms.Button();
            this.btnServices = new System.Windows.Forms.Button();
            this.btnReissue = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.lblHint = new System.Windows.Forms.Label();
            this.dgvInvoices = new System.Windows.Forms.DataGridView();
            this.pnlLines = new System.Windows.Forms.Panel();
            this.dgvLines = new System.Windows.Forms.DataGridView();
            this.lblLines = new System.Windows.Forms.Label();
            this.lblSummary = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            this.pnlActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvoices)).BeginInit();
            this.pnlLines.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLines)).BeginInit();
            this.SuspendLayout();
            //
            // pnlTop
            //
            this.pnlTop.Controls.Add(this.btnRefresh);
            this.pnlTop.Controls.Add(this.txtSearch);
            this.pnlTop.Controls.Add(this.lblSearch);
            this.pnlTop.Controls.Add(this.cboStatus);
            this.pnlTop.Controls.Add(this.lblStatus);
            this.pnlTop.Controls.Add(this.dtpTo);
            this.pnlTop.Controls.Add(this.lblTo);
            this.pnlTop.Controls.Add(this.dtpFrom);
            this.pnlTop.Controls.Add(this.lblFrom);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1184, 44);
            this.pnlTop.TabIndex = 0;
            //
            // btnRefresh
            //
            this.btnRefresh.Location = new System.Drawing.Point(940, 9);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 27);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            //
            // txtSearch
            //
            this.txtSearch.Location = new System.Drawing.Point(714, 11);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(210, 23);
            this.txtSearch.TabIndex = 7;
            this.txtSearch.TextChanged += new System.EventHandler(this.filter_Changed);
            //
            // lblSearch
            //
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(530, 14);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(181, 15);
            this.lblSearch.TabIndex = 6;
            this.lblSearch.Text = "Tìm (số HĐ, phòng, tên khách):";
            //
            // cboStatus
            //
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.Location = new System.Drawing.Point(384, 11);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(130, 23);
            this.cboStatus.TabIndex = 5;
            this.cboStatus.SelectedIndexChanged += new System.EventHandler(this.filter_Changed);
            //
            // lblStatus
            //
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(320, 14);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(59, 15);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Trạng thái:";
            //
            // dtpTo
            //
            this.dtpTo.CustomFormat = "dd/MM/yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(200, 11);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(106, 23);
            this.dtpTo.TabIndex = 3;
            this.dtpTo.ValueChanged += new System.EventHandler(this.filter_Changed);
            //
            // lblTo
            //
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(170, 14);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(27, 15);
            this.lblTo.TabIndex = 2;
            this.lblTo.Text = "đến";
            //
            // dtpFrom
            //
            this.dtpFrom.CustomFormat = "dd/MM/yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(58, 11);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(106, 23);
            this.dtpFrom.TabIndex = 1;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.filter_Changed);
            //
            // lblFrom
            //
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(12, 14);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(43, 15);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "Lập từ:";
            //
            // pnlActions
            //
            this.pnlActions.Controls.Add(this.btnView);
            this.pnlActions.Controls.Add(this.btnVoid);
            this.pnlActions.Controls.Add(this.btnServices);
            this.pnlActions.Controls.Add(this.btnReissue);
            this.pnlActions.Controls.Add(this.btnExport);
            this.pnlActions.Controls.Add(this.lblHint);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlActions.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlActions.Location = new System.Drawing.Point(1014, 44);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Padding = new System.Windows.Forms.Padding(8, 12, 8, 8);
            this.pnlActions.Size = new System.Drawing.Size(170, 617);
            this.pnlActions.TabIndex = 3;
            this.pnlActions.WrapContents = false;
            //
            // btnView
            //
            this.btnView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnView.Location = new System.Drawing.Point(8, 12);
            this.btnView.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(152, 34);
            this.btnView.TabIndex = 0;
            this.btnView.Text = "Xem / In";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            //
            // btnVoid
            //
            this.btnVoid.Location = new System.Drawing.Point(8, 52);
            this.btnVoid.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnVoid.Name = "btnVoid";
            this.btnVoid.Size = new System.Drawing.Size(152, 34);
            this.btnVoid.TabIndex = 1;
            this.btnVoid.Text = "Void hóa đơn";
            this.btnVoid.UseVisualStyleBackColor = true;
            this.btnVoid.Click += new System.EventHandler(this.btnVoid_Click);
            //
            // btnServices
            //
            this.btnServices.Location = new System.Drawing.Point(8, 92);
            this.btnServices.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnServices.Name = "btnServices";
            this.btnServices.Size = new System.Drawing.Size(152, 34);
            this.btnServices.TabIndex = 2;
            this.btnServices.Text = "Sửa dịch vụ phòng";
            this.btnServices.UseVisualStyleBackColor = true;
            this.btnServices.Click += new System.EventHandler(this.btnServices_Click);
            //
            // btnReissue
            //
            this.btnReissue.Location = new System.Drawing.Point(8, 132);
            this.btnReissue.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnReissue.Name = "btnReissue";
            this.btnReissue.Size = new System.Drawing.Size(152, 34);
            this.btnReissue.TabIndex = 3;
            this.btnReissue.Text = "Lập lại hóa đơn";
            this.btnReissue.UseVisualStyleBackColor = true;
            this.btnReissue.Click += new System.EventHandler(this.btnReissue_Click);
            //
            // btnExport
            //
            this.btnExport.Location = new System.Drawing.Point(8, 172);
            this.btnExport.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(152, 34);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "Xuất CSV";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            //
            // lblHint
            //
            this.lblHint.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblHint.Location = new System.Drawing.Point(8, 218);
            this.lblHint.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(152, 120);
            this.lblHint.TabIndex = 5;
            this.lblHint.Text = "Sửa sai sau khi đã lập hóa đơn: Void → Sửa dịch vụ → Lập lại. Hóa đơn cũ vẫn giữ ở trạng thái đã hủy.";
            //
            // dgvInvoices
            //
            this.dgvInvoices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInvoices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInvoices.Location = new System.Drawing.Point(0, 44);
            this.dgvInvoices.Name = "dgvInvoices";
            this.dgvInvoices.Size = new System.Drawing.Size(1014, 365);
            this.dgvInvoices.TabIndex = 1;
            this.dgvInvoices.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvInvoices_CellDoubleClick);
            this.dgvInvoices.SelectionChanged += new System.EventHandler(this.dgvInvoices_SelectionChanged);
            //
            // pnlLines
            //
            this.pnlLines.Controls.Add(this.dgvLines);
            this.pnlLines.Controls.Add(this.lblLines);
            this.pnlLines.Controls.Add(this.lblSummary);
            this.pnlLines.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlLines.Location = new System.Drawing.Point(0, 409);
            this.pnlLines.Name = "pnlLines";
            this.pnlLines.Size = new System.Drawing.Size(1014, 252);
            this.pnlLines.TabIndex = 2;
            //
            // dgvLines
            //
            this.dgvLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLines.Location = new System.Drawing.Point(0, 24);
            this.dgvLines.Name = "dgvLines";
            this.dgvLines.Size = new System.Drawing.Size(1014, 200);
            this.dgvLines.TabIndex = 1;
            //
            // lblLines
            //
            this.lblLines.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLines.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblLines.Location = new System.Drawing.Point(0, 0);
            this.lblLines.Name = "lblLines";
            this.lblLines.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.lblLines.Size = new System.Drawing.Size(1014, 24);
            this.lblLines.TabIndex = 0;
            this.lblLines.Text = "Các dòng của hóa đơn";
            this.lblLines.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblSummary
            //
            this.lblSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblSummary.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSummary.Location = new System.Drawing.Point(0, 224);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.lblSummary.Size = new System.Drawing.Size(1014, 28);
            this.lblSummary.TabIndex = 2;
            this.lblSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // frmInvoices
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 661);
            this.Controls.Add(this.dgvInvoices);
            this.Controls.Add(this.pnlLines);
            this.Controls.Add(this.pnlActions);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1100, 560);
            this.Name = "frmInvoices";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hóa đơn";
            this.Load += new System.EventHandler(this.frmInvoices_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvoices)).EndInit();
            this.pnlLines.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLines)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.FlowLayoutPanel pnlActions;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnVoid;
        private System.Windows.Forms.Button btnServices;
        private System.Windows.Forms.Button btnReissue;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.DataGridView dgvInvoices;
        private System.Windows.Forms.Panel pnlLines;
        private System.Windows.Forms.Label lblLines;
        private System.Windows.Forms.DataGridView dgvLines;
        private System.Windows.Forms.Label lblSummary;
    }
}
