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
            this.lblShukkaFlag = new DSWControl.DSWLabel.DSWLabel();
            this.txtShukkaFlag = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblNonyusakiCD = new DSWControl.DSWLabel.DSWLabel();
            this.txtNonyusakiCD = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblBoxNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtBoxNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblPassword.ChildPanel.SuspendLayout();
            this.lblPassword.SuspendLayout();
            this.lblUser.ChildPanel.SuspendLayout();
            this.lblUser.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.lblClass.ChildPanel.SuspendLayout();
            this.lblClass.SuspendLayout();
            this.lblShukkaFlag.ChildPanel.SuspendLayout();
            this.lblShukkaFlag.SuspendLayout();
            this.lblNonyusakiCD.ChildPanel.SuspendLayout();
            this.lblNonyusakiCD.SuspendLayout();
            this.lblBoxNo.ChildPanel.SuspendLayout();
            this.lblBoxNo.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(47, 219);
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
            this.btnClose.Location = new System.Drawing.Point(238, 219);
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
            this.pnlMain.Controls.Add(this.lblBoxNo);
            this.pnlMain.Controls.Add(this.lblNonyusakiCD);
            this.pnlMain.Controls.Add(this.lblShukkaFlag);
            this.pnlMain.Controls.Add(this.lblClass);
            this.pnlMain.Size = new System.Drawing.Size(378, 257);
            this.pnlMain.Controls.SetChildIndex(this.btnClose, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnLogin, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblClass, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblPassword, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblUser, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblShukkaFlag, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblNonyusakiCD, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblBoxNo, 0);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(378, 58);
            // 
            // fbrFunction
            // 
            this.fbrFunction.Location = new System.Drawing.Point(156, 199);
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
            // lblShukkaFlag
            // 
            // 
            // lblShukkaFlag.ChildPanel
            // 
            this.lblShukkaFlag.ChildPanel.Controls.Add(this.txtShukkaFlag);
            this.lblShukkaFlag.IsFocusChangeColor = false;
            this.lblShukkaFlag.IsNecessary = true;
            this.lblShukkaFlag.LabelWidth = 80;
            this.lblShukkaFlag.Location = new System.Drawing.Point(47, 117);
            this.lblShukkaFlag.Name = "lblShukkaFlag";
            this.lblShukkaFlag.Size = new System.Drawing.Size(278, 20);
            this.lblShukkaFlag.SplitterWidth = 0;
            this.lblShukkaFlag.TabIndex = 5;
            this.lblShukkaFlag.Text = "出荷区分";
            // 
            // txtShukkaFlag
            // 
            this.txtShukkaFlag.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtShukkaFlag.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtShukkaFlag.InputRegulation = "n";
            this.txtShukkaFlag.IsInputRegulation = true;
            this.txtShukkaFlag.Location = new System.Drawing.Point(0, 0);
            this.txtShukkaFlag.MaxLength = 1;
            this.txtShukkaFlag.Name = "txtShukkaFlag";
            this.txtShukkaFlag.OneLineMaxLength = 1;
            this.txtShukkaFlag.PadType = DSWControl.DSWTextBox.DSWTextBox.RightLeftType.Left;
            this.txtShukkaFlag.ProhibitionChar = null;
            this.txtShukkaFlag.Size = new System.Drawing.Size(198, 19);
            this.txtShukkaFlag.TabIndex = 0;
            // 
            // lblNonyusakiCD
            // 
            // 
            // lblNonyusakiCD.ChildPanel
            // 
            this.lblNonyusakiCD.ChildPanel.Controls.Add(this.txtNonyusakiCD);
            this.lblNonyusakiCD.IsFocusChangeColor = false;
            this.lblNonyusakiCD.IsNecessary = true;
            this.lblNonyusakiCD.LabelWidth = 80;
            this.lblNonyusakiCD.Location = new System.Drawing.Point(47, 143);
            this.lblNonyusakiCD.Name = "lblNonyusakiCD";
            this.lblNonyusakiCD.Size = new System.Drawing.Size(278, 20);
            this.lblNonyusakiCD.SplitterWidth = 0;
            this.lblNonyusakiCD.TabIndex = 6;
            this.lblNonyusakiCD.Text = "納入先コード";
            // 
            // txtNonyusakiCD
            // 
            this.txtNonyusakiCD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNonyusakiCD.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtNonyusakiCD.InputRegulation = "n";
            this.txtNonyusakiCD.IsInputRegulation = true;
            this.txtNonyusakiCD.Location = new System.Drawing.Point(0, 0);
            this.txtNonyusakiCD.MaxLength = 4;
            this.txtNonyusakiCD.Name = "txtNonyusakiCD";
            this.txtNonyusakiCD.OneLineMaxLength = 4;
            this.txtNonyusakiCD.PadType = DSWControl.DSWTextBox.DSWTextBox.RightLeftType.Left;
            this.txtNonyusakiCD.ProhibitionChar = null;
            this.txtNonyusakiCD.Size = new System.Drawing.Size(198, 19);
            this.txtNonyusakiCD.TabIndex = 0;
            // 
            // lblBoxNo
            // 
            // 
            // lblBoxNo.ChildPanel
            // 
            this.lblBoxNo.ChildPanel.Controls.Add(this.txtBoxNo);
            this.lblBoxNo.IsFocusChangeColor = false;
            this.lblBoxNo.IsNecessary = true;
            this.lblBoxNo.LabelWidth = 80;
            this.lblBoxNo.Location = new System.Drawing.Point(47, 169);
            this.lblBoxNo.Name = "lblBoxNo";
            this.lblBoxNo.Size = new System.Drawing.Size(278, 20);
            this.lblBoxNo.SplitterWidth = 0;
            this.lblBoxNo.TabIndex = 7;
            this.lblBoxNo.Text = "Box No.";
            // 
            // txtBoxNo
            // 
            this.txtBoxNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtBoxNo.InputRegulation = "na";
            this.txtBoxNo.IsInputRegulation = true;
            this.txtBoxNo.Location = new System.Drawing.Point(0, 0);
            this.txtBoxNo.MaxByteLengthMode = true;
            this.txtBoxNo.MaxLength = 6;
            this.txtBoxNo.Name = "txtBoxNo";
            this.txtBoxNo.OneLineMaxLength = 6;
            this.txtBoxNo.PaddingChar = '\0';
            this.txtBoxNo.PadType = DSWControl.DSWTextBox.DSWTextBox.RightLeftType.Left;
            this.txtBoxNo.ProhibitionChar = null;
            this.txtBoxNo.Size = new System.Drawing.Size(198, 19);
            this.txtBoxNo.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(378, 337);
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
            this.lblShukkaFlag.ChildPanel.ResumeLayout(false);
            this.lblShukkaFlag.ChildPanel.PerformLayout();
            this.lblShukkaFlag.ResumeLayout(false);
            this.lblNonyusakiCD.ChildPanel.ResumeLayout(false);
            this.lblNonyusakiCD.ChildPanel.PerformLayout();
            this.lblNonyusakiCD.ResumeLayout(false);
            this.lblBoxNo.ChildPanel.ResumeLayout(false);
            this.lblBoxNo.ChildPanel.PerformLayout();
            this.lblBoxNo.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DSWControl.DSWLabel.DSWLabel lblClass;
        private DSWControl.DSWComboBox.DSWComboBox cboClass;
        private DSWControl.DSWLabel.DSWLabel lblBoxNo;
        private DSWControl.DSWTextBox.DSWTextBox txtBoxNo;
        private DSWControl.DSWLabel.DSWLabel lblNonyusakiCD;
        private DSWControl.DSWTextBox.DSWTextBox txtNonyusakiCD;
        private DSWControl.DSWLabel.DSWLabel lblShukkaFlag;
        private DSWControl.DSWTextBox.DSWTextBox txtShukkaFlag;
    }
}
