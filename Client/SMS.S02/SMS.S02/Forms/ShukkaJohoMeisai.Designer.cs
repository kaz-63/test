namespace SMS.S02.Forms
{
    partial class ShukkaJohoMeisai
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShukkaJohoMeisai));
            this.lblNonyusakiName = new DSWControl.DSWLabel.DSWLabel();
            this.txtNonyusakiName = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblShip = new DSWControl.DSWLabel.DSWLabel();
            this.txtShip = new DSWControl.DSWTextBox.DSWTextBox();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.lblBoxNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtBoxNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.shtMeisai = new GrapeCity.Win.ElTabelle.Sheet();
            this.pnlMain.SuspendLayout();
            this.lblNonyusakiName.ChildPanel.SuspendLayout();
            this.lblNonyusakiName.SuspendLayout();
            this.lblShip.ChildPanel.SuspendLayout();
            this.lblShip.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.lblBoxNo.ChildPanel.SuspendLayout();
            this.lblBoxNo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.shtMeisai);
            this.pnlMain.Controls.Add(this.grpSearch);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtMeisai, 0);
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // lblNonyusakiName
            // 
            // 
            // lblNonyusakiName.ChildPanel
            // 
            this.lblNonyusakiName.ChildPanel.Controls.Add(this.txtNonyusakiName);
            this.lblNonyusakiName.IsFocusChangeColor = false;
            this.lblNonyusakiName.LabelWidth = 80;
            resources.ApplyResources(this.lblNonyusakiName, "lblNonyusakiName");
            this.lblNonyusakiName.Name = "lblNonyusakiName";
            this.lblNonyusakiName.SplitterWidth = 0;
            // 
            // txtNonyusakiName
            // 
            this.txtNonyusakiName.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtNonyusakiName, "txtNonyusakiName");
            this.txtNonyusakiName.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtNonyusakiName.InputRegulation = global::SMS.S02.Properties.Resources.ShukkaJohoKokunai_Print;
            this.txtNonyusakiName.Name = "txtNonyusakiName";
            this.txtNonyusakiName.ProhibitionChar = null;
            this.txtNonyusakiName.ReadOnly = true;
            this.txtNonyusakiName.TabStop = false;
            // 
            // lblShip
            // 
            // 
            // lblShip.ChildPanel
            // 
            this.lblShip.ChildPanel.Controls.Add(this.txtShip);
            this.lblShip.IsFocusChangeColor = false;
            this.lblShip.LabelWidth = 80;
            resources.ApplyResources(this.lblShip, "lblShip");
            this.lblShip.Name = "lblShip";
            this.lblShip.SplitterWidth = 0;
            // 
            // txtShip
            // 
            this.txtShip.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtShip, "txtShip");
            this.txtShip.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtShip.InputRegulation = global::SMS.S02.Properties.Resources.ShukkaJohoKokunai_Print;
            this.txtShip.Name = "txtShip";
            this.txtShip.OneLineMaxLength = 10;
            this.txtShip.ProhibitionChar = null;
            this.txtShip.ReadOnly = true;
            this.txtShip.TabStop = false;
            // 
            // grpSearch
            // 
            this.grpSearch.Controls.Add(this.lblBoxNo);
            this.grpSearch.Controls.Add(this.lblNonyusakiName);
            this.grpSearch.Controls.Add(this.lblShip);
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
            // 
            // lblBoxNo
            // 
            // 
            // lblBoxNo.ChildPanel
            // 
            this.lblBoxNo.ChildPanel.Controls.Add(this.txtBoxNo);
            this.lblBoxNo.IsFocusChangeColor = false;
            this.lblBoxNo.LabelWidth = 80;
            resources.ApplyResources(this.lblBoxNo, "lblBoxNo");
            this.lblBoxNo.Name = "lblBoxNo";
            this.lblBoxNo.SplitterWidth = 0;
            // 
            // txtBoxNo
            // 
            this.txtBoxNo.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtBoxNo, "txtBoxNo");
            this.txtBoxNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtBoxNo.InputRegulation = global::SMS.S02.Properties.Resources.ShukkaJohoKokunai_Print;
            this.txtBoxNo.Name = "txtBoxNo";
            this.txtBoxNo.ProhibitionChar = null;
            this.txtBoxNo.ReadOnly = true;
            this.txtBoxNo.TabStop = false;
            // 
            // shtMeisai
            // 
            resources.ApplyResources(this.shtMeisai, "shtMeisai");
            this.shtMeisai.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtMeisai.Data")));
            this.shtMeisai.Name = "shtMeisai";
            // 
            // ShukkaJohoMeisai
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ShukkaJohoMeisai";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.lblNonyusakiName.ChildPanel.ResumeLayout(false);
            this.lblNonyusakiName.ChildPanel.PerformLayout();
            this.lblNonyusakiName.ResumeLayout(false);
            this.lblShip.ChildPanel.ResumeLayout(false);
            this.lblShip.ChildPanel.PerformLayout();
            this.lblShip.ResumeLayout(false);
            this.grpSearch.ResumeLayout(false);
            this.lblBoxNo.ChildPanel.ResumeLayout(false);
            this.lblBoxNo.ChildPanel.PerformLayout();
            this.lblBoxNo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DSWControl.DSWLabel.DSWLabel lblNonyusakiName;
        private DSWControl.DSWTextBox.DSWTextBox txtNonyusakiName;
        private DSWControl.DSWLabel.DSWLabel lblShip;
        private DSWControl.DSWTextBox.DSWTextBox txtShip;
        private System.Windows.Forms.GroupBox grpSearch;
        private DSWControl.DSWLabel.DSWLabel lblBoxNo;
        private DSWControl.DSWTextBox.DSWTextBox txtBoxNo;
        private GrapeCity.Win.ElTabelle.Sheet shtMeisai;
    }
}