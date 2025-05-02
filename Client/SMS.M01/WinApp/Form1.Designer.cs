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
            this.lblClass = new DSWControl.DSWLabel.DSWLabel();
            this.cboClass = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblPassword.ChildPanel.SuspendLayout();
            this.lblPassword.SuspendLayout();
            this.lblUser.ChildPanel.SuspendLayout();
            this.lblUser.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.lblClass.ChildPanel.SuspendLayout();
            this.lblClass.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(47, 143);
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(47, 92);
            // 
            // lblUser
            // 
            this.lblUser.Location = new System.Drawing.Point(47, 56);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(238, 143);
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
            this.pnlMain.Controls.Add(this.lblClass);
            this.pnlMain.Size = new System.Drawing.Size(378, 184);
            this.pnlMain.Controls.SetChildIndex(this.lblClass, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblPassword, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnClose, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnLogin, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblUser, 0);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(378, 58);
            // 
            // fbrFunction
            // 
            this.fbrFunction.Location = new System.Drawing.Point(156, 123);
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(259, 7);
            // 
            // txtRoleName
            // 
            this.txtRoleName.Location = new System.Drawing.Point(137, 7);
            // 
            // pnlTitleSpace
            // 
            this.pnlTitleSpace.Location = new System.Drawing.Point(88, 4);
            this.pnlTitleSpace.Size = new System.Drawing.Size(46, 28);
            // 
            // lblClass
            // 
            // 
            // lblClass.ChildPanel
            // 
            this.lblClass.ChildPanel.Controls.Add(this.cboClass);
            this.lblClass.IsFocusChangeColor = false;
            this.lblClass.IsNecessary = true;
            this.lblClass.LabelWidth = 80;
            this.lblClass.Location = new System.Drawing.Point(47, 20);
            this.lblClass.Name = "lblClass";
            this.lblClass.Size = new System.Drawing.Size(278, 20);
            this.lblClass.SplitterWidth = 0;
            this.lblClass.TabIndex = 4;
            this.lblClass.Text = "画面";
            // 
            // cboClass
            // 
            this.cboClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboClass.Location = new System.Drawing.Point(0, 0);
            this.cboClass.Name = "cboClass";
            this.cboClass.Size = new System.Drawing.Size(198, 20);
            this.cboClass.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(378, 264);
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
            this.lblClass.ChildPanel.ResumeLayout(false);
            this.lblClass.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DSWControl.DSWLabel.DSWLabel lblClass;
        private DSWControl.DSWComboBox.DSWComboBox cboClass;
    }
}
