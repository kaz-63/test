namespace SystemBase.Forms
{
    partial class PasswordChangeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PasswordChangeForm));
            this.lblOldPassword = new DSWControl.DSWLabel.DSWLabel();
            this.txtOldPassword = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblNewPassword = new DSWControl.DSWLabel.DSWLabel();
            this.txtNewPassword = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblConfirmPassword = new DSWControl.DSWLabel.DSWLabel();
            this.txtConfirmPassword = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnChange = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnClose = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.pnlMain.SuspendLayout();
            this.lblOldPassword.ChildPanel.SuspendLayout();
            this.lblOldPassword.SuspendLayout();
            this.lblNewPassword.ChildPanel.SuspendLayout();
            this.lblNewPassword.SuspendLayout();
            this.lblConfirmPassword.ChildPanel.SuspendLayout();
            this.lblConfirmPassword.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.lblOldPassword);
            this.pnlMain.Controls.Add(this.lblNewPassword);
            this.pnlMain.Controls.Add(this.btnChange);
            this.pnlMain.Controls.Add(this.btnClose);
            this.pnlMain.Controls.Add(this.lblConfirmPassword);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblConfirmPassword, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnClose, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnChange, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblNewPassword, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblOldPassword, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
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
            // lblOldPassword
            // 
            // 
            // lblOldPassword.ChildPanel
            // 
            this.lblOldPassword.ChildPanel.Controls.Add(this.txtOldPassword);
            this.lblOldPassword.IsFocusChangeColor = false;
            this.lblOldPassword.IsNecessary = true;
            this.lblOldPassword.LabelWidth = 140;
            resources.ApplyResources(this.lblOldPassword, "lblOldPassword");
            this.lblOldPassword.Name = "lblOldPassword";
            this.lblOldPassword.SplitterWidth = 0;
            // 
            // txtOldPassword
            // 
            resources.ApplyResources(this.txtOldPassword, "txtOldPassword");
            this.txtOldPassword.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtOldPassword.InputRegulation = "abnl";
            this.txtOldPassword.IsInputRegulation = true;
            this.txtOldPassword.MaxByteLengthMode = true;
            this.txtOldPassword.Name = "txtOldPassword";
            this.txtOldPassword.ProhibitionChar = null;
            // 
            // lblNewPassword
            // 
            // 
            // lblNewPassword.ChildPanel
            // 
            this.lblNewPassword.ChildPanel.Controls.Add(this.txtNewPassword);
            this.lblNewPassword.IsFocusChangeColor = false;
            this.lblNewPassword.IsNecessary = true;
            this.lblNewPassword.LabelWidth = 140;
            resources.ApplyResources(this.lblNewPassword, "lblNewPassword");
            this.lblNewPassword.Name = "lblNewPassword";
            this.lblNewPassword.SplitterWidth = 0;
            // 
            // txtNewPassword
            // 
            resources.ApplyResources(this.txtNewPassword, "txtNewPassword");
            this.txtNewPassword.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtNewPassword.InputRegulation = "abnl";
            this.txtNewPassword.IsInputRegulation = true;
            this.txtNewPassword.MaxByteLengthMode = true;
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.ProhibitionChar = null;
            // 
            // lblConfirmPassword
            // 
            // 
            // lblConfirmPassword.ChildPanel
            // 
            this.lblConfirmPassword.ChildPanel.Controls.Add(this.txtConfirmPassword);
            this.lblConfirmPassword.IsFocusChangeColor = false;
            this.lblConfirmPassword.IsNecessary = true;
            this.lblConfirmPassword.LabelWidth = 140;
            resources.ApplyResources(this.lblConfirmPassword, "lblConfirmPassword");
            this.lblConfirmPassword.Name = "lblConfirmPassword";
            this.lblConfirmPassword.SplitterWidth = 0;
            // 
            // txtConfirmPassword
            // 
            resources.ApplyResources(this.txtConfirmPassword, "txtConfirmPassword");
            this.txtConfirmPassword.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtConfirmPassword.InputRegulation = "abnl";
            this.txtConfirmPassword.IsInputRegulation = true;
            this.txtConfirmPassword.MaxByteLengthMode = true;
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.ProhibitionChar = null;
            // 
            // btnChange
            // 
            resources.ApplyResources(this.btnChange, "btnChange");
            this.btnChange.LinkFunctionKey = DSWControl.DSWFunctionButton.FunctionKeys.F01;
            this.btnChange.Name = "btnChange";
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.LinkFunctionKey = DSWControl.DSWFunctionButton.FunctionKeys.F12;
            this.btnClose.Name = "btnClose";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // PasswordChangeForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PasswordChangeForm";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.lblOldPassword.ChildPanel.ResumeLayout(false);
            this.lblOldPassword.ChildPanel.PerformLayout();
            this.lblOldPassword.ResumeLayout(false);
            this.lblNewPassword.ChildPanel.ResumeLayout(false);
            this.lblNewPassword.ChildPanel.PerformLayout();
            this.lblNewPassword.ResumeLayout(false);
            this.lblConfirmPassword.ChildPanel.ResumeLayout(false);
            this.lblConfirmPassword.ChildPanel.PerformLayout();
            this.lblConfirmPassword.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected DSWControl.DSWLabel.DSWLabel lblConfirmPassword;
        protected DSWControl.DSWLabel.DSWLabel lblNewPassword;
        protected DSWControl.DSWLabel.DSWLabel lblOldPassword;
        protected DSWControl.DSWFunctionButton.DSWFunctionButton btnClose;
        protected DSWControl.DSWFunctionButton.DSWFunctionButton btnChange;
        protected DSWControl.DSWTextBox.DSWTextBox txtOldPassword;
        protected DSWControl.DSWTextBox.DSWTextBox txtNewPassword;
        protected DSWControl.DSWTextBox.DSWTextBox txtConfirmPassword;

    }
}