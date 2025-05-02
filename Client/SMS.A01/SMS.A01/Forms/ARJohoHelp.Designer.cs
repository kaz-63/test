namespace SMS.A01.Forms
{
    partial class ARJohoHelp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ARJohoHelp));
            this.tbcMain = new System.Windows.Forms.TabControl();
            this.tbpARList = new System.Windows.Forms.TabPage();
            this.picHelp1 = new System.Windows.Forms.PictureBox();
            this.tbpARJohoToroku = new System.Windows.Forms.TabPage();
            this.picHelp2 = new System.Windows.Forms.PictureBox();
            this.btnClose = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.pnlMain.SuspendLayout();
            this.tbcMain.SuspendLayout();
            this.tbpARList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHelp1)).BeginInit();
            this.tbpARJohoToroku.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHelp2)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.tbcMain);
            this.pnlMain.Controls.Add(this.btnClose);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnClose, 0);
            this.pnlMain.Controls.SetChildIndex(this.tbcMain, 0);
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
            // tbcMain
            // 
            resources.ApplyResources(this.tbcMain, "tbcMain");
            this.tbcMain.Controls.Add(this.tbpARList);
            this.tbcMain.Controls.Add(this.tbpARJohoToroku);
            this.tbcMain.Name = "tbcMain";
            this.tbcMain.SelectedIndex = 0;
            // 
            // tbpARList
            // 
            resources.ApplyResources(this.tbpARList, "tbpARList");
            this.tbpARList.Controls.Add(this.picHelp1);
            this.tbpARList.Name = "tbpARList";
            this.tbpARList.UseVisualStyleBackColor = true;
            // 
            // picHelp1
            // 
            resources.ApplyResources(this.picHelp1, "picHelp1");
            this.picHelp1.Name = "picHelp1";
            this.picHelp1.TabStop = false;
            // 
            // tbpARJohoToroku
            // 
            resources.ApplyResources(this.tbpARJohoToroku, "tbpARJohoToroku");
            this.tbpARJohoToroku.Controls.Add(this.picHelp2);
            this.tbpARJohoToroku.Name = "tbpARJohoToroku";
            this.tbpARJohoToroku.UseVisualStyleBackColor = true;
            // 
            // picHelp2
            // 
            resources.ApplyResources(this.picHelp2, "picHelp2");
            this.picHelp2.Name = "picHelp2";
            this.picHelp2.TabStop = false;
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.LinkFunctionKey = DSWControl.DSWFunctionButton.FunctionKeys.F12;
            this.btnClose.Name = "btnClose";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ARJohoHelp
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ARJohoHelp";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.tbcMain.ResumeLayout(false);
            this.tbpARList.ResumeLayout(false);
            this.tbpARList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHelp1)).EndInit();
            this.tbpARJohoToroku.ResumeLayout(false);
            this.tbpARJohoToroku.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHelp2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tbcMain;
        private System.Windows.Forms.TabPage tbpARList;
        private System.Windows.Forms.TabPage tbpARJohoToroku;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnClose;
        private System.Windows.Forms.PictureBox picHelp1;
        private System.Windows.Forms.PictureBox picHelp2;
    }
}