namespace Hotel_management_system.Forms.Main
{
    partial class dlgChangePassword
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
            this.lblAccount = new System.Windows.Forms.Label();
            this.lblOld = new System.Windows.Forms.Label();
            this.txtOld = new System.Windows.Forms.TextBox();
            this.lblNew = new System.Windows.Forms.Label();
            this.txtNew = new System.Windows.Forms.TextBox();
            this.lblConfirm = new System.Windows.Forms.Label();
            this.txtConfirm = new System.Windows.Forms.TextBox();
            this.lblHint = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblAccount
            //
            this.lblAccount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblAccount.Location = new System.Drawing.Point(12, 12);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new System.Drawing.Size(376, 20);
            this.lblAccount.TabIndex = 0;
            this.lblAccount.Text = "Tài khoản:";
            //
            // lblOld
            //
            this.lblOld.AutoSize = true;
            this.lblOld.Location = new System.Drawing.Point(12, 48);
            this.lblOld.Name = "lblOld";
            this.lblOld.Size = new System.Drawing.Size(107, 15);
            this.lblOld.TabIndex = 1;
            this.lblOld.Text = "Mật khẩu hiện tại:";
            //
            // txtOld
            //
            this.txtOld.Location = new System.Drawing.Point(150, 45);
            this.txtOld.MaxLength = 100;
            this.txtOld.Name = "txtOld";
            this.txtOld.Size = new System.Drawing.Size(238, 23);
            this.txtOld.TabIndex = 2;
            this.txtOld.UseSystemPasswordChar = true;
            //
            // lblNew
            //
            this.lblNew.AutoSize = true;
            this.lblNew.Location = new System.Drawing.Point(12, 82);
            this.lblNew.Name = "lblNew";
            this.lblNew.Size = new System.Drawing.Size(85, 15);
            this.lblNew.TabIndex = 3;
            this.lblNew.Text = "Mật khẩu mới:";
            //
            // txtNew
            //
            this.txtNew.Location = new System.Drawing.Point(150, 79);
            this.txtNew.MaxLength = 100;
            this.txtNew.Name = "txtNew";
            this.txtNew.Size = new System.Drawing.Size(238, 23);
            this.txtNew.TabIndex = 4;
            this.txtNew.UseSystemPasswordChar = true;
            //
            // lblConfirm
            //
            this.lblConfirm.AutoSize = true;
            this.lblConfirm.Location = new System.Drawing.Point(12, 116);
            this.lblConfirm.Name = "lblConfirm";
            this.lblConfirm.Size = new System.Drawing.Size(129, 15);
            this.lblConfirm.TabIndex = 5;
            this.lblConfirm.Text = "Nhập lại mật khẩu mới:";
            //
            // txtConfirm
            //
            this.txtConfirm.Location = new System.Drawing.Point(150, 113);
            this.txtConfirm.MaxLength = 100;
            this.txtConfirm.Name = "txtConfirm";
            this.txtConfirm.Size = new System.Drawing.Size(238, 23);
            this.txtConfirm.TabIndex = 6;
            this.txtConfirm.UseSystemPasswordChar = true;
            //
            // lblHint
            //
            this.lblHint.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblHint.Location = new System.Drawing.Point(150, 140);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(238, 20);
            this.lblHint.TabIndex = 7;
            this.lblHint.Text = "Ít nhất 6 ký tự, khác mật khẩu cũ.";
            //
            // btnOk
            //
            this.btnOk.Location = new System.Drawing.Point(202, 172);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 30);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "Đổi mật khẩu";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(298, 172);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = true;
            //
            // dlgChangePassword
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 214);
            this.Controls.Add(this.lblAccount);
            this.Controls.Add(this.lblOld);
            this.Controls.Add(this.txtOld);
            this.Controls.Add(this.lblNew);
            this.Controls.Add(this.txtNew);
            this.Controls.Add(this.lblConfirm);
            this.Controls.Add(this.txtConfirm);
            this.Controls.Add(this.lblHint);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "dlgChangePassword";
            this.Text = "Đổi mật khẩu";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAccount;
        private System.Windows.Forms.Label lblOld;
        private System.Windows.Forms.TextBox txtOld;
        private System.Windows.Forms.Label lblNew;
        private System.Windows.Forms.TextBox txtNew;
        private System.Windows.Forms.Label lblConfirm;
        private System.Windows.Forms.TextBox txtConfirm;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}
