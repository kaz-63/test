namespace SystemBase.Forms
{
    partial class BaseLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseLogin));
            this.lblUser = new DSWControl.DSWLabel.DSWLabel();
            this.txtUserID = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblPassword = new DSWControl.DSWLabel.DSWLabel();
            this.txtPassword = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnLogin = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnClose = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnPasswordChange = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.pnlMain.SuspendLayout();
            this.lblUser.ChildPanel.SuspendLayout();
            this.lblUser.SuspendLayout();
            this.lblPassword.ChildPanel.SuspendLayout();
            this.lblPassword.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.btnPasswordChange);
            this.pnlMain.Controls.Add(this.lblUser);
            this.pnlMain.Controls.Add(this.btnLogin);
            this.pnlMain.Controls.Add(this.btnClose);
            this.pnlMain.Controls.Add(this.lblPassword);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblPassword, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnClose, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnLogin, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblUser, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnPasswordChange, 0);
            // 
            // pnlTitle
            // 
            resources.ApplyResources(this.pnlTitle, "pnlTitle");
            // 
            // fbrFunction
            // 
            resources.ApplyResources(this.fbrFunction, "fbrFunction");
            // 
            // txtUserName
            // 
            resources.ApplyResources(this.txtUserName, "txtUserName");
            // 
            // txtRoleName
            // 
            resources.ApplyResources(this.txtRoleName, "txtRoleName");
            // 
            // lblCorner
            // 
            resources.ApplyResources(this.lblCorner, "lblCorner");
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // lblUser
            // 
            // 
            // lblUser.ChildPanel
            // 
            this.lblUser.ChildPanel.Controls.Add(this.txtUserID);
            this.lblUser.IsFocusChangeColor = false;
            this.lblUser.IsNecessary = true;
            this.lblUser.LabelWidth = 80;
            resources.ApplyResources(this.lblUser, "lblUser");
            this.lblUser.Name = "lblUser";
            this.lblUser.SplitterWidth = 0;
            // 
            // txtUserID
            // 
            resources.ApplyResources(this.txtUserID, "txtUserID");
            this.txtUserID.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtUserID.InputRegulation = "abnl";
            this.txtUserID.IsInputRegulation = true;
            this.txtUserID.MaxByteLengthMode = true;
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.ProhibitionChar = null;
            // 
            // lblPassword
            // 
            // 
            // lblPassword.ChildPanel
            // 
            this.lblPassword.ChildPanel.Controls.Add(this.txtPassword);
            this.lblPassword.IsFocusChangeColor = false;
            this.lblPassword.IsNecessary = true;
            this.lblPassword.LabelWidth = 80;
            resources.ApplyResources(this.lblPassword, "lblPassword");
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.SplitterWidth = 0;
            // 
            // txtPassword
            // 
            resources.ApplyResources(this.txtPassword, "txtPassword");
            this.txtPassword.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtPassword.InputRegulation = "abnl";
            this.txtPassword.IsInputRegulation = true;
            this.txtPassword.MaxByteLengthMode = true;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.ProhibitionChar = null;
            // 
            // btnLogin
            // 
            resources.ApplyResources(this.btnLogin, "btnLogin");
            this.btnLogin.LinkFunctionKey = DSWControl.DSWFunctionButton.FunctionKeys.F02;
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.LinkFunctionKey = DSWControl.DSWFunctionButton.FunctionKeys.F12;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPasswordChange
            // 
            resources.ApplyResources(this.btnPasswordChange, "btnPasswordChange");
            this.btnPasswordChange.LinkFunctionKey = DSWControl.DSWFunctionButton.FunctionKeys.F08;
            this.btnPasswordChange.Name = "btnPasswordChange";
            this.btnPasswordChange.UseVisualStyleBackColor = false;
            this.btnPasswordChange.Click += new System.EventHandler(this.btnPasswordChange_Click);
            // 
            // BaseLogin
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BaseLogin";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.lblUser.ChildPanel.ResumeLayout(false);
            this.lblUser.ChildPanel.PerformLayout();
            this.lblUser.ResumeLayout(false);
            this.lblPassword.ChildPanel.ResumeLayout(false);
            this.lblPassword.ChildPanel.PerformLayout();
            this.lblPassword.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected DSWControl.DSWFunctionButton.DSWFunctionButton btnLogin;
        protected DSWControl.DSWLabel.DSWLabel lblPassword;
        protected DSWControl.DSWLabel.DSWLabel lblUser;
        protected DSWControl.DSWFunctionButton.DSWFunctionButton btnClose;
        protected DSWControl.DSWTextBox.DSWTextBox txtPassword;
        protected DSWControl.DSWTextBox.DSWTextBox txtUserID;
        protected DSWControl.DSWFunctionButton.DSWFunctionButton btnPasswordChange;


    }
}