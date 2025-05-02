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
            this.lblPassword.ChildPanel.SuspendLayout();
            this.lblPassword.SuspendLayout();
            this.lblUser.ChildPanel.SuspendLayout();
            this.lblUser.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblPassword
            // 
            // 
            // lblUser
            // 
            // 
            // txtPassword
            // 
            this.txtPassword.Text = "";
            // 
            // txtUserID
            // 
            this.txtUserID.Text = "";
            // 
            // pnlMain
            // 
            this.pnlMain.Size = new System.Drawing.Size(374, 173);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(374, 58);
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(255, 7);
            // 
            // txtRoleName
            // 
            this.txtRoleName.Location = new System.Drawing.Point(133, 7);
            // 
            // pnlTitleSpace
            // 
            this.pnlTitleSpace.Location = new System.Drawing.Point(88, 4);
            this.pnlTitleSpace.Size = new System.Drawing.Size(42, 28);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 253);
            this.Name = "Form1";
            this.Text = "Form1";
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

    }
}

