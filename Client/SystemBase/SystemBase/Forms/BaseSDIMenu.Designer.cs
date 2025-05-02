namespace SystemBase.Forms
{
    partial class BaseSDIMenu
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
            DSWControl.DSWMenuCategoryBindSetting dswMenuCategoryBindSetting1 = new DSWControl.DSWMenuCategoryBindSetting();
            DSWControl.DSWMenuItemBindSetting dswMenuItemBindSetting1 = new DSWControl.DSWMenuItemBindSetting();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseSDIMenu));
            this.btnLogout = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnClose = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.mnuMenu = new DSWControl.DSWMenu();
            this.btnPasswordChange = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.btnPasswordChange);
            this.pnlMain.Controls.Add(this.mnuMenu);
            this.pnlMain.Controls.Add(this.btnLogout);
            this.pnlMain.Controls.Add(this.btnClose);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnClose, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnLogout, 0);
            this.pnlMain.Controls.SetChildIndex(this.mnuMenu, 0);
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
            // btnLogout
            // 
            resources.ApplyResources(this.btnLogout, "btnLogout");
            this.btnLogout.LinkFunctionKey = DSWControl.DSWFunctionButton.FunctionKeys.F04;
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.LinkFunctionKey = DSWControl.DSWFunctionButton.FunctionKeys.F12;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // mnuMenu
            // 
            resources.ApplyResources(this.mnuMenu, "mnuMenu");
            this.mnuMenu.CategoryBindSetting = dswMenuCategoryBindSetting1;
            this.mnuMenu.CategoryFont = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.mnuMenu.ItemBindSetting = dswMenuItemBindSetting1;
            this.mnuMenu.ItemFont = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.mnuMenu.Name = "mnuMenu";
            this.mnuMenu.MenuItemClick += new DSWControl.MenuItemClickHandler(this.mnuMenu_MenuItemClick);
            // 
            // btnPasswordChange
            // 
            resources.ApplyResources(this.btnPasswordChange, "btnPasswordChange");
            this.btnPasswordChange.LinkFunctionKey = DSWControl.DSWFunctionButton.FunctionKeys.F08;
            this.btnPasswordChange.Name = "btnPasswordChange";
            this.btnPasswordChange.Click += new System.EventHandler(this.btnPasswordChange_Click);
            // 
            // BaseSDIMenu
            // 
            resources.ApplyResources(this, "$this");
            this.Name = "BaseSDIMenu";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DSWControl.DSWFunctionButton.DSWFunctionButton btnLogout;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnClose;
        private DSWControl.DSWMenu mnuMenu;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnPasswordChange;
    }
}