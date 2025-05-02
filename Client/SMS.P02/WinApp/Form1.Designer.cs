namespace WinApp
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.rdoP0200010 = new System.Windows.Forms.RadioButton();
            this.rdoP0200030 = new System.Windows.Forms.RadioButton();
            this.rdoP0200040 = new System.Windows.Forms.RadioButton();
            this.rdoP0200050 = new System.Windows.Forms.RadioButton();
            this.lblPassword.ChildPanel.SuspendLayout();
            this.lblPassword.SuspendLayout();
            this.lblUser.ChildPanel.SuspendLayout();
            this.lblUser.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(47, 138);
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(47, 87);
            // 
            // lblUser
            // 
            this.lblUser.Location = new System.Drawing.Point(47, 51);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(238, 138);
            // 
            // txtPassword
            // 
            this.txtPassword.MaxLength = 0;
            this.txtPassword.OneLineMaxLength = 0;
            this.txtPassword.Text = "";
            // 
            // txtUserID
            // 
            this.txtUserID.MaxLength = 0;
            this.txtUserID.OneLineMaxLength = 0;
            this.txtUserID.Text = "";
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.rdoP0200050);
            this.pnlMain.Controls.Add(this.rdoP0200040);
            this.pnlMain.Controls.Add(this.rdoP0200010);
            this.pnlMain.Controls.Add(this.rdoP0200030);
            this.pnlMain.Size = new System.Drawing.Size(390, 174);
            this.pnlMain.Controls.SetChildIndex(this.rdoP0200030, 0);
            this.pnlMain.Controls.SetChildIndex(this.rdoP0200010, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnLogin, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnClose, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblPassword, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblUser, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.rdoP0200040, 0);
            this.pnlMain.Controls.SetChildIndex(this.rdoP0200050, 0);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(390, 58);
            // 
            // fbrFunction
            // 
            this.fbrFunction.Location = new System.Drawing.Point(156, 118);
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(271, 7);
            // 
            // txtRoleName
            // 
            this.txtRoleName.Location = new System.Drawing.Point(149, 7);
            // 
            // pnlTitleSpace
            // 
            this.pnlTitleSpace.Location = new System.Drawing.Point(75, 4);
            this.pnlTitleSpace.Size = new System.Drawing.Size(71, 28);
            // 
            // rdoP0200010
            // 
            this.rdoP0200010.AutoSize = true;
            this.rdoP0200010.Checked = true;
            this.rdoP0200010.Location = new System.Drawing.Point(47, 7);
            this.rdoP0200010.Name = "rdoP0200010";
            this.rdoP0200010.Size = new System.Drawing.Size(83, 16);
            this.rdoP0200010.TabIndex = 4;
            this.rdoP0200010.TabStop = true;
            this.rdoP0200010.Text = "納入先一覧";
            this.rdoP0200010.UseVisualStyleBackColor = true;
            // 
            // rdoP0200030
            // 
            this.rdoP0200030.AutoSize = true;
            this.rdoP0200030.Location = new System.Drawing.Point(136, 7);
            this.rdoP0200030.Name = "rdoP0200030";
            this.rdoP0200030.Size = new System.Drawing.Size(95, 16);
            this.rdoP0200030.TabIndex = 5;
            this.rdoP0200030.Text = "工事識別一覧";
            this.rdoP0200030.UseVisualStyleBackColor = true;
            // 
            // rdoP0200040
            // 
            this.rdoP0200040.AutoSize = true;
            this.rdoP0200040.Location = new System.Drawing.Point(47, 29);
            this.rdoP0200040.Name = "rdoP0200040";
            this.rdoP0200040.Size = new System.Drawing.Size(83, 16);
            this.rdoP0200040.TabIndex = 6;
            this.rdoP0200040.Text = "物件名一覧";
            this.rdoP0200040.UseVisualStyleBackColor = true;
            // 
            // rdoP0200050
            // 
            this.rdoP0200050.AutoSize = true;
            this.rdoP0200050.Location = new System.Drawing.Point(136, 29);
            this.rdoP0200050.Name = "rdoP0200050";
            this.rdoP0200050.Size = new System.Drawing.Size(71, 16);
            this.rdoP0200050.TabIndex = 7;
            this.rdoP0200050.Text = "履歴照会";
            this.rdoP0200050.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(390, 254);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "ログイン";
            this.lblPassword.ChildPanel.ResumeLayout(false);
            this.lblPassword.ChildPanel.PerformLayout();
            this.lblPassword.ResumeLayout(false);
            this.lblUser.ChildPanel.ResumeLayout(false);
            this.lblUser.ChildPanel.PerformLayout();
            this.lblUser.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoP0200010;
        private System.Windows.Forms.RadioButton rdoP0200030;
        private System.Windows.Forms.RadioButton rdoP0200040;
        private System.Windows.Forms.RadioButton rdoP0200050;
    }
}
