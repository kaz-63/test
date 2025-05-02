namespace SMS.K02.Forms
{
    partial class PalletListHakko
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PalletListHakko));
            this.shtPalletList = new GrapeCity.Win.ElTabelle.Sheet();
            this.grpMode = new System.Windows.Forms.GroupBox();
            this.rdoSaiHakko = new System.Windows.Forms.RadioButton();
            this.rdoSinki = new System.Windows.Forms.RadioButton();
            this.btnAllNotCheck = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnAllCheck = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnRangeNotCheck = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnRangeCheck = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.dswLabel1 = new DSWControl.DSWLabel.DSWLabel();
            this.cboHaccoSelect = new DSWControl.DSWComboBox.DSWComboBox();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtPalletList)).BeginInit();
            this.grpMode.SuspendLayout();
            this.dswLabel1.ChildPanel.SuspendLayout();
            this.dswLabel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.dswLabel1);
            this.pnlMain.Controls.Add(this.btnRangeNotCheck);
            this.pnlMain.Controls.Add(this.btnRangeCheck);
            this.pnlMain.Controls.Add(this.shtPalletList);
            this.pnlMain.Controls.Add(this.grpMode);
            this.pnlMain.Controls.Add(this.btnAllNotCheck);
            this.pnlMain.Controls.Add(this.btnAllCheck);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnAllCheck, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnAllNotCheck, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpMode, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtPalletList, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnRangeCheck, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnRangeNotCheck, 0);
            this.pnlMain.Controls.SetChildIndex(this.dswLabel1, 0);
            // 
            // fbrFunction
            // 
            // 
            // fbrFunction.F01Button
            // 
            resources.ApplyResources(this.fbrFunction.F01Button, "fbrFunction.F01Button");
            resources.ApplyResources(this.fbrFunction, "fbrFunction");
            // 
            // fbrFunction.F02Button
            // 
            resources.ApplyResources(this.fbrFunction.F02Button, "fbrFunction.F02Button");
            // 
            // fbrFunction.F07Button
            // 
            resources.ApplyResources(this.fbrFunction.F07Button, "fbrFunction.F07Button");
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // shtPalletList
            // 
            resources.ApplyResources(this.shtPalletList, "shtPalletList");
            this.shtPalletList.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtPalletList.Data")));
            this.shtPalletList.Name = "shtPalletList";
            this.shtPalletList.LeaveEdit += new GrapeCity.Win.ElTabelle.LeaveEditEventHandler(this.shtPalletList_LeaveEdit);
            // 
            // grpMode
            // 
            this.grpMode.Controls.Add(this.rdoSaiHakko);
            this.grpMode.Controls.Add(this.rdoSinki);
            resources.ApplyResources(this.grpMode, "grpMode");
            this.grpMode.Name = "grpMode";
            this.grpMode.TabStop = false;
            // 
            // rdoSaiHakko
            // 
            resources.ApplyResources(this.rdoSaiHakko, "rdoSaiHakko");
            this.rdoSaiHakko.Name = "rdoSaiHakko";
            this.rdoSaiHakko.UseVisualStyleBackColor = true;
            this.rdoSaiHakko.CheckedChanged += new System.EventHandler(this.rdoSaiHakko_CheckedChanged);
            // 
            // rdoSinki
            // 
            resources.ApplyResources(this.rdoSinki, "rdoSinki");
            this.rdoSinki.Name = "rdoSinki";
            this.rdoSinki.UseVisualStyleBackColor = true;
            this.rdoSinki.CheckedChanged += new System.EventHandler(this.rdoSinki_CheckedChanged);
            // 
            // btnAllNotCheck
            // 
            resources.ApplyResources(this.btnAllNotCheck, "btnAllNotCheck");
            this.btnAllNotCheck.Name = "btnAllNotCheck";
            this.btnAllNotCheck.Click += new System.EventHandler(this.btnAllNotCheck_Click);
            // 
            // btnAllCheck
            // 
            resources.ApplyResources(this.btnAllCheck, "btnAllCheck");
            this.btnAllCheck.Name = "btnAllCheck";
            this.btnAllCheck.Click += new System.EventHandler(this.btnAllCheck_Click);
            // 
            // btnRangeNotCheck
            // 
            resources.ApplyResources(this.btnRangeNotCheck, "btnRangeNotCheck");
            this.btnRangeNotCheck.Name = "btnRangeNotCheck";
            this.btnRangeNotCheck.Click += new System.EventHandler(this.btnRangeNotCheck_Click);
            // 
            // btnRangeCheck
            // 
            resources.ApplyResources(this.btnRangeCheck, "btnRangeCheck");
            this.btnRangeCheck.Name = "btnRangeCheck";
            this.btnRangeCheck.Click += new System.EventHandler(this.btnRangeCheck_Click);
            // 
            // dswLabel1
            // 
            resources.ApplyResources(this.dswLabel1, "dswLabel1");
            // 
            // dswLabel1.ChildPanel
            // 
            this.dswLabel1.ChildPanel.Controls.Add(this.cboHaccoSelect);
            this.dswLabel1.IsFocusChangeColor = false;
            this.dswLabel1.LabelTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dswLabel1.LabelWidth = 63;
            this.dswLabel1.Name = "dswLabel1";
            this.dswLabel1.SplitterWidth = 0;
            // 
            // cboHaccoSelect
            // 
            resources.ApplyResources(this.cboHaccoSelect, "cboHaccoSelect");
            this.cboHaccoSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHaccoSelect.Name = "cboHaccoSelect";
            // 
            // PalletListHakko
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "PalletListHakko";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtPalletList)).EndInit();
            this.grpMode.ResumeLayout(false);
            this.grpMode.PerformLayout();
            this.dswLabel1.ChildPanel.ResumeLayout(false);
            this.dswLabel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GrapeCity.Win.ElTabelle.Sheet shtPalletList;
        private System.Windows.Forms.GroupBox grpMode;
        private System.Windows.Forms.RadioButton rdoSaiHakko;
        private System.Windows.Forms.RadioButton rdoSinki;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnAllNotCheck;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnAllCheck;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnRangeNotCheck;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnRangeCheck;
        private DSWControl.DSWLabel.DSWLabel dswLabel1;
        private DSWControl.DSWComboBox.DSWComboBox cboHaccoSelect;
    }
}