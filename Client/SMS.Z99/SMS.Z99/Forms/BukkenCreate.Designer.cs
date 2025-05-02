namespace SMS.Z99.Forms
{
    partial class BukkenCreate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BukkenCreate));
            this.ofdExcel = new System.Windows.Forms.OpenFileDialog();
            this.sfdExcel = new System.Windows.Forms.SaveFileDialog();
            this.lblSubTitle = new System.Windows.Forms.Label();
            this.btnStart = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnExit = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.btnExit);
            this.pnlMain.Controls.Add(this.btnStart);
            this.pnlMain.Controls.Add(this.lblSubTitle);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.SetChildIndex(this.lblSubTitle, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnStart, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnExit, 0);
            // 
            // pnlTitle
            // 
            resources.ApplyResources(this.pnlTitle, "pnlTitle");
            // 
            // fbrFunction
            // 
            resources.ApplyResources(this.fbrFunction, "fbrFunction");
            // 
            // fbrFunction.F01Button
            // 
            resources.ApplyResources(this.fbrFunction.F01Button, "fbrFunction.F01Button");
            // 
            // fbrFunction.F03Button
            // 
            resources.ApplyResources(this.fbrFunction.F03Button, "fbrFunction.F03Button");
            // 
            // fbrFunction.F05Button
            // 
            resources.ApplyResources(this.fbrFunction.F05Button, "fbrFunction.F05Button");
            // 
            // fbrFunction.F06Button
            // 
            resources.ApplyResources(this.fbrFunction.F06Button, "fbrFunction.F06Button");
            // 
            // fbrFunction.F08Button
            // 
            resources.ApplyResources(this.fbrFunction.F08Button, "fbrFunction.F08Button");
            // 
            // fbrFunction.F09Button
            // 
            resources.ApplyResources(this.fbrFunction.F09Button, "fbrFunction.F09Button");
            // 
            // fbrFunction.F10Button
            // 
            resources.ApplyResources(this.fbrFunction.F10Button, "fbrFunction.F10Button");
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
            // ofdExcel
            // 
            resources.ApplyResources(this.ofdExcel, "ofdExcel");
            // 
            // sfdExcel
            // 
            resources.ApplyResources(this.sfdExcel, "sfdExcel");
            // 
            // lblSubTitle
            // 
            resources.ApplyResources(this.lblSubTitle, "lblSubTitle");
            this.lblSubTitle.Name = "lblSubTitle";
            // 
            // btnStart
            // 
            resources.ApplyResources(this.btnStart, "btnStart");
            this.btnStart.Name = "btnStart";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnExit
            // 
            resources.ApplyResources(this.btnExit, "btnExit");
            this.btnExit.Name = "btnExit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // BukkenCreate
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.MaximizeBox = false;
            this.Name = "BukkenCreate";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ofdExcel;
        private System.Windows.Forms.SaveFileDialog sfdExcel;
        private System.Windows.Forms.Label lblSubTitle;
        protected DSWControl.DSWFunctionButton.DSWFunctionButton btnExit;
        protected DSWControl.DSWFunctionButton.DSWFunctionButton btnStart;
    }
}