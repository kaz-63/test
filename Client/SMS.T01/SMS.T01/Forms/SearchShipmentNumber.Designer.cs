namespace SMS.T01.Forms
{
    partial class SearchShipmentNumber
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchShipmentNumber));
            this.lblShukkaQty = new DSWControl.DSWLabel.DSWLabel();
            this.txtShukkaQty = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblTehaiQty = new DSWControl.DSWLabel.DSWLabel();
            this.txtTehaiQty = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblRenkeiNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtRenkeiNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblHinmei = new DSWControl.DSWLabel.DSWLabel();
            this.txtHinmei = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblHinmeiJp = new DSWControl.DSWLabel.DSWLabel();
            this.txtHinmeiJp = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblProjectName = new DSWControl.DSWLabel.DSWLabel();
            this.txtProjectName = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblShipmentNumbers = new DSWControl.DSWLabel.DSWLabel();
            this.txtShipmentNumbers = new DSWControl.DSWTextBox.DSWTextBox();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.shtMeisai = new GrapeCity.Win.ElTabelle.Sheet();
            this.pnlMain.SuspendLayout();
            this.lblShukkaQty.ChildPanel.SuspendLayout();
            this.lblShukkaQty.SuspendLayout();
            this.lblTehaiQty.ChildPanel.SuspendLayout();
            this.lblTehaiQty.SuspendLayout();
            this.lblRenkeiNo.ChildPanel.SuspendLayout();
            this.lblRenkeiNo.SuspendLayout();
            this.lblHinmei.ChildPanel.SuspendLayout();
            this.lblHinmei.SuspendLayout();
            this.lblHinmeiJp.ChildPanel.SuspendLayout();
            this.lblHinmeiJp.SuspendLayout();
            this.lblProjectName.ChildPanel.SuspendLayout();
            this.lblProjectName.SuspendLayout();
            this.lblShipmentNumbers.ChildPanel.SuspendLayout();
            this.lblShipmentNumbers.SuspendLayout();
            this.grpSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.shtMeisai);
            this.pnlMain.Controls.Add(this.lblShipmentNumbers);
            this.pnlMain.Controls.Add(this.grpSearch);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblShipmentNumbers, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtMeisai, 0);
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // lblShukkaQty
            // 
            // 
            // lblShukkaQty.ChildPanel
            // 
            this.lblShukkaQty.ChildPanel.Controls.Add(this.txtShukkaQty);
            this.lblShukkaQty.IsFocusChangeColor = false;
            this.lblShukkaQty.LabelWidth = 130;
            resources.ApplyResources(this.lblShukkaQty, "lblShukkaQty");
            this.lblShukkaQty.Name = "lblShukkaQty";
            this.lblShukkaQty.NormalBackColor = System.Drawing.Color.LightSkyBlue;
            this.lblShukkaQty.SplitterWidth = 0;
            // 
            // txtShukkaQty
            // 
            this.txtShukkaQty.AutoPad = true;
            this.txtShukkaQty.BackColor = System.Drawing.SystemColors.Control;
            this.txtShukkaQty.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            resources.ApplyResources(this.txtShukkaQty, "txtShukkaQty");
            this.txtShukkaQty.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtShukkaQty.InputRegulation = "nab";
            this.txtShukkaQty.IsInputRegulation = true;
            this.txtShukkaQty.MaxByteLengthMode = true;
            this.txtShukkaQty.Name = "txtShukkaQty";
            this.txtShukkaQty.OneLineMaxLength = 8;
            this.txtShukkaQty.PaddingChar = '0';
            this.txtShukkaQty.ProhibitionChar = null;
            this.txtShukkaQty.ReadOnly = true;
            // 
            // lblTehaiQty
            // 
            // 
            // lblTehaiQty.ChildPanel
            // 
            this.lblTehaiQty.ChildPanel.Controls.Add(this.txtTehaiQty);
            this.lblTehaiQty.IsFocusChangeColor = false;
            this.lblTehaiQty.LabelWidth = 130;
            resources.ApplyResources(this.lblTehaiQty, "lblTehaiQty");
            this.lblTehaiQty.Name = "lblTehaiQty";
            this.lblTehaiQty.NormalBackColor = System.Drawing.Color.LightSkyBlue;
            this.lblTehaiQty.SplitterWidth = 0;
            // 
            // txtTehaiQty
            // 
            this.txtTehaiQty.AutoPad = true;
            this.txtTehaiQty.BackColor = System.Drawing.SystemColors.Control;
            this.txtTehaiQty.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            resources.ApplyResources(this.txtTehaiQty, "txtTehaiQty");
            this.txtTehaiQty.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtTehaiQty.InputRegulation = "nab";
            this.txtTehaiQty.IsInputRegulation = true;
            this.txtTehaiQty.MaxByteLengthMode = true;
            this.txtTehaiQty.Name = "txtTehaiQty";
            this.txtTehaiQty.OneLineMaxLength = 8;
            this.txtTehaiQty.PaddingChar = '0';
            this.txtTehaiQty.ProhibitionChar = null;
            this.txtTehaiQty.ReadOnly = true;
            // 
            // lblRenkeiNo
            // 
            // 
            // lblRenkeiNo.ChildPanel
            // 
            this.lblRenkeiNo.ChildPanel.Controls.Add(this.txtRenkeiNo);
            this.lblRenkeiNo.IsFocusChangeColor = false;
            this.lblRenkeiNo.LabelWidth = 130;
            resources.ApplyResources(this.lblRenkeiNo, "lblRenkeiNo");
            this.lblRenkeiNo.Name = "lblRenkeiNo";
            this.lblRenkeiNo.NormalBackColor = System.Drawing.Color.LightSkyBlue;
            this.lblRenkeiNo.SplitterWidth = 0;
            // 
            // txtRenkeiNo
            // 
            this.txtRenkeiNo.AutoPad = true;
            this.txtRenkeiNo.BackColor = System.Drawing.SystemColors.Control;
            this.txtRenkeiNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            resources.ApplyResources(this.txtRenkeiNo, "txtRenkeiNo");
            this.txtRenkeiNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtRenkeiNo.InputRegulation = "nab";
            this.txtRenkeiNo.IsInputRegulation = true;
            this.txtRenkeiNo.MaxByteLengthMode = true;
            this.txtRenkeiNo.Name = "txtRenkeiNo";
            this.txtRenkeiNo.OneLineMaxLength = 8;
            this.txtRenkeiNo.PaddingChar = '0';
            this.txtRenkeiNo.ProhibitionChar = null;
            this.txtRenkeiNo.ReadOnly = true;
            // 
            // lblHinmei
            // 
            // 
            // lblHinmei.ChildPanel
            // 
            this.lblHinmei.ChildPanel.Controls.Add(this.txtHinmei);
            this.lblHinmei.IsFocusChangeColor = false;
            this.lblHinmei.LabelWidth = 130;
            resources.ApplyResources(this.lblHinmei, "lblHinmei");
            this.lblHinmei.Name = "lblHinmei";
            this.lblHinmei.NormalBackColor = System.Drawing.Color.LightSkyBlue;
            this.lblHinmei.SplitterWidth = 0;
            // 
            // txtHinmei
            // 
            this.txtHinmei.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtHinmei, "txtHinmei");
            this.txtHinmei.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtHinmei.InputRegulation = "F";
            this.txtHinmei.MaxByteLengthMode = true;
            this.txtHinmei.Name = "txtHinmei";
            this.txtHinmei.OneLineMaxLength = 100;
            this.txtHinmei.ProhibitionChar = null;
            this.txtHinmei.ReadOnly = true;
            // 
            // lblHinmeiJp
            // 
            // 
            // lblHinmeiJp.ChildPanel
            // 
            this.lblHinmeiJp.ChildPanel.Controls.Add(this.txtHinmeiJp);
            this.lblHinmeiJp.IsFocusChangeColor = false;
            this.lblHinmeiJp.LabelWidth = 130;
            resources.ApplyResources(this.lblHinmeiJp, "lblHinmeiJp");
            this.lblHinmeiJp.Name = "lblHinmeiJp";
            this.lblHinmeiJp.NormalBackColor = System.Drawing.Color.LightSkyBlue;
            this.lblHinmeiJp.SplitterWidth = 0;
            // 
            // txtHinmeiJp
            // 
            this.txtHinmeiJp.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtHinmeiJp, "txtHinmeiJp");
            this.txtHinmeiJp.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtHinmeiJp.InputRegulation = "F";
            this.txtHinmeiJp.MaxByteLengthMode = true;
            this.txtHinmeiJp.Name = "txtHinmeiJp";
            this.txtHinmeiJp.OneLineMaxLength = 100;
            this.txtHinmeiJp.ProhibitionChar = null;
            this.txtHinmeiJp.ReadOnly = true;
            // 
            // lblProjectName
            // 
            // 
            // lblProjectName.ChildPanel
            // 
            this.lblProjectName.ChildPanel.Controls.Add(this.txtProjectName);
            this.lblProjectName.IsFocusChangeColor = false;
            this.lblProjectName.LabelWidth = 130;
            resources.ApplyResources(this.lblProjectName, "lblProjectName");
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.NormalBackColor = System.Drawing.Color.LightSkyBlue;
            this.lblProjectName.SplitterWidth = 0;
            // 
            // txtProjectName
            // 
            this.txtProjectName.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtProjectName, "txtProjectName");
            this.txtProjectName.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtProjectName.InputRegulation = "F";
            this.txtProjectName.MaxByteLengthMode = true;
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.OneLineMaxLength = 60;
            this.txtProjectName.ProhibitionChar = null;
            this.txtProjectName.ReadOnly = true;
            // 
            // lblShipmentNumbers
            // 
            // 
            // lblShipmentNumbers.ChildPanel
            // 
            this.lblShipmentNumbers.ChildPanel.Controls.Add(this.txtShipmentNumbers);
            this.lblShipmentNumbers.IsFocusChangeColor = false;
            this.lblShipmentNumbers.LabelWidth = 120;
            resources.ApplyResources(this.lblShipmentNumbers, "lblShipmentNumbers");
            this.lblShipmentNumbers.Name = "lblShipmentNumbers";
            this.lblShipmentNumbers.NormalBackColor = System.Drawing.Color.LightSkyBlue;
            this.lblShipmentNumbers.SplitterWidth = 0;
            // 
            // txtShipmentNumbers
            // 
            this.txtShipmentNumbers.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtShipmentNumbers, "txtShipmentNumbers");
            this.txtShipmentNumbers.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtShipmentNumbers.InputRegulation = "F";
            this.txtShipmentNumbers.MaxByteLengthMode = true;
            this.txtShipmentNumbers.Name = "txtShipmentNumbers";
            this.txtShipmentNumbers.OneLineMaxLength = 40;
            this.txtShipmentNumbers.ProhibitionChar = null;
            this.txtShipmentNumbers.ReadOnly = true;
            // 
            // grpSearch
            // 
            this.grpSearch.Controls.Add(this.lblShukkaQty);
            this.grpSearch.Controls.Add(this.lblTehaiQty);
            this.grpSearch.Controls.Add(this.lblRenkeiNo);
            this.grpSearch.Controls.Add(this.lblHinmei);
            this.grpSearch.Controls.Add(this.lblHinmeiJp);
            this.grpSearch.Controls.Add(this.lblProjectName);
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
            // 
            // shtMeisai
            // 
            resources.ApplyResources(this.shtMeisai, "shtMeisai");
            this.shtMeisai.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtMeisai.Data")));
            this.shtMeisai.Name = "shtMeisai";
            // 
            // SearchShipmentNumber
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "SearchShipmentNumber";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.lblShukkaQty.ChildPanel.ResumeLayout(false);
            this.lblShukkaQty.ChildPanel.PerformLayout();
            this.lblShukkaQty.ResumeLayout(false);
            this.lblTehaiQty.ChildPanel.ResumeLayout(false);
            this.lblTehaiQty.ChildPanel.PerformLayout();
            this.lblTehaiQty.ResumeLayout(false);
            this.lblRenkeiNo.ChildPanel.ResumeLayout(false);
            this.lblRenkeiNo.ChildPanel.PerformLayout();
            this.lblRenkeiNo.ResumeLayout(false);
            this.lblHinmei.ChildPanel.ResumeLayout(false);
            this.lblHinmei.ChildPanel.PerformLayout();
            this.lblHinmei.ResumeLayout(false);
            this.lblHinmeiJp.ChildPanel.ResumeLayout(false);
            this.lblHinmeiJp.ChildPanel.PerformLayout();
            this.lblHinmeiJp.ResumeLayout(false);
            this.lblProjectName.ChildPanel.ResumeLayout(false);
            this.lblProjectName.ChildPanel.PerformLayout();
            this.lblProjectName.ResumeLayout(false);
            this.lblShipmentNumbers.ChildPanel.ResumeLayout(false);
            this.lblShipmentNumbers.ChildPanel.PerformLayout();
            this.lblShipmentNumbers.ResumeLayout(false);
            this.grpSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSearch;
        private DSWControl.DSWLabel.DSWLabel lblProjectName;
        private DSWControl.DSWTextBox.DSWTextBox txtProjectName;
        private DSWControl.DSWLabel.DSWLabel lblRenkeiNo;
        private DSWControl.DSWTextBox.DSWTextBox txtRenkeiNo;
        private DSWControl.DSWLabel.DSWLabel lblHinmei;
        private DSWControl.DSWTextBox.DSWTextBox txtHinmei;
        private DSWControl.DSWLabel.DSWLabel lblHinmeiJp;
        private DSWControl.DSWTextBox.DSWTextBox txtHinmeiJp;
        private DSWControl.DSWLabel.DSWLabel lblTehaiQty;
        private DSWControl.DSWTextBox.DSWTextBox txtTehaiQty;
        private DSWControl.DSWLabel.DSWLabel lblShipmentNumbers;
        private DSWControl.DSWTextBox.DSWTextBox txtShipmentNumbers;
        private GrapeCity.Win.ElTabelle.Sheet shtMeisai;
        private DSWControl.DSWLabel.DSWLabel lblShukkaQty;
        private DSWControl.DSWTextBox.DSWTextBox txtShukkaQty;
    }
}