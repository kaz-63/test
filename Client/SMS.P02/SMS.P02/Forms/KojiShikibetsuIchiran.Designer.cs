namespace SMS.P02.Forms
{
    partial class KojiShikibetsuIchiran
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KojiShikibetsuIchiran));
            this.lblKojiName = new DSWControl.DSWLabel.DSWLabel();
            this.txtKojiName = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblShip = new DSWControl.DSWLabel.DSWLabel();
            this.txtShip = new DSWControl.DSWTextBox.DSWTextBox();
            this.grpSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.lblKojiName.ChildPanel.SuspendLayout();
            this.lblKojiName.SuspendLayout();
            this.lblShip.ChildPanel.SuspendLayout();
            this.lblShip.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSearch
            // 
            this.grpSearch.Controls.Add(this.lblKojiName);
            this.grpSearch.Controls.Add(this.lblShip);
            this.grpSearch.Controls.SetChildIndex(this.btnSearchAll, 0);
            this.grpSearch.Controls.SetChildIndex(this.lblShip, 0);
            this.grpSearch.Controls.SetChildIndex(this.lblKojiName, 0);
            this.grpSearch.Controls.SetChildIndex(this.btnSearch, 0);
            // 
            // shtResult
            // 
            this.shtResult.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtResult.Data")));
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // lblKojiName
            // 
            // 
            // lblKojiName.ChildPanel
            // 
            this.lblKojiName.ChildPanel.Controls.Add(this.txtKojiName);
            this.lblKojiName.IsFocusChangeColor = false;
            this.lblKojiName.LabelWidth = 100;
            resources.ApplyResources(this.lblKojiName, "lblKojiName");
            this.lblKojiName.Name = "lblKojiName";
            this.lblKojiName.SplitterWidth = 0;
            // 
            // txtKojiName
            // 
            resources.ApplyResources(this.txtKojiName, "txtKojiName");
            this.txtKojiName.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtKojiName.InputRegulation = "F";
            this.txtKojiName.MaxByteLengthMode = true;
            this.txtKojiName.Name = "txtKojiName";
            this.txtKojiName.OneLineMaxLength = 60;
            this.txtKojiName.ProhibitionChar = null;
            this.txtKojiName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKojiName_KeyDown);
            // 
            // lblShip
            // 
            // 
            // lblShip.ChildPanel
            // 
            this.lblShip.ChildPanel.Controls.Add(this.txtShip);
            this.lblShip.IsFocusChangeColor = false;
            this.lblShip.LabelWidth = 100;
            resources.ApplyResources(this.lblShip, "lblShip");
            this.lblShip.Name = "lblShip";
            this.lblShip.SplitterWidth = 0;
            // 
            // txtShip
            // 
            resources.ApplyResources(this.txtShip, "txtShip");
            this.txtShip.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtShip.InputRegulation = "F";
            this.txtShip.MaxByteLengthMode = true;
            this.txtShip.Name = "txtShip";
            this.txtShip.OneLineMaxLength = 10;
            this.txtShip.ProhibitionChar = null;
            // 
            // KojiShikibetsuIchiran
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KojiShikibetsuIchiran";
            this.grpSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.lblKojiName.ChildPanel.ResumeLayout(false);
            this.lblKojiName.ChildPanel.PerformLayout();
            this.lblKojiName.ResumeLayout(false);
            this.lblShip.ChildPanel.ResumeLayout(false);
            this.lblShip.ChildPanel.PerformLayout();
            this.lblShip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DSWControl.DSWLabel.DSWLabel lblKojiName;
        private DSWControl.DSWTextBox.DSWTextBox txtKojiName;
        private DSWControl.DSWLabel.DSWLabel lblShip;
        private DSWControl.DSWTextBox.DSWTextBox txtShip;
    }
}