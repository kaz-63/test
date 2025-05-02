namespace SMS.S01.Forms
{
    partial class ShukkaKeikaku
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShukkaKeikaku));
            this.lblExcel = new DSWControl.DSWLabel.DSWLabel();
            this.txtExcel = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblShipSeiban = new DSWControl.DSWLabel.DSWLabel();
            this.txtShipSeiban = new DSWControl.DSWTextBox.DSWTextBox();
            this.shtMeisai = new GrapeCity.Win.ElTabelle.Sheet();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.btnStart = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnReference = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.ofdExcel = new System.Windows.Forms.OpenFileDialog();
            this.lblSeiban = new DSWControl.DSWLabel.DSWLabel();
            this.txtRevision = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnSearch = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblNiukesaki = new DSWControl.DSWLabel.DSWLabel();
            this.cboNiukesaki = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblBoxAssign = new DSWControl.DSWLabel.DSWLabel();
            this.txtBoxAssign = new DSWControl.DSWTextBox.DSWTextBox();
            this.txtRev = new DSWControl.DSWTextBox.DSWTextBox();
            this.cboBukkenName = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblBukkenNameTorikomi = new DSWControl.DSWLabel.DSWLabel();
            this.cboBukkenNameTorikomi = new DSWControl.DSWComboBox.DSWComboBox();
            this.dswComboBox2 = new DSWControl.DSWComboBox.DSWComboBox();
            this.pnlMain.SuspendLayout();
            this.lblExcel.ChildPanel.SuspendLayout();
            this.lblExcel.SuspendLayout();
            this.lblShipSeiban.ChildPanel.SuspendLayout();
            this.lblShipSeiban.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).BeginInit();
            this.grpSearch.SuspendLayout();
            this.lblSeiban.ChildPanel.SuspendLayout();
            this.lblSeiban.SuspendLayout();
            this.lblNiukesaki.ChildPanel.SuspendLayout();
            this.lblNiukesaki.SuspendLayout();
            this.lblBoxAssign.ChildPanel.SuspendLayout();
            this.lblBoxAssign.SuspendLayout();
            this.lblBukkenNameTorikomi.ChildPanel.SuspendLayout();
            this.lblBukkenNameTorikomi.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.lblBukkenNameTorikomi);
            this.pnlMain.Controls.Add(this.lblBoxAssign);
            this.pnlMain.Controls.Add(this.lblSeiban);
            this.pnlMain.Controls.Add(this.lblNiukesaki);
            this.pnlMain.Controls.Add(this.btnSearch);
            this.pnlMain.Controls.Add(this.grpSearch);
            this.pnlMain.Controls.Add(this.shtMeisai);
            this.pnlMain.Controls.Add(this.lblShipSeiban);
            this.pnlMain.Controls.SetChildIndex(this.lblShipSeiban, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtMeisai, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnSearch, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblNiukesaki, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblSeiban, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblBoxAssign, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblBukkenNameTorikomi, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
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
            // fbrFunction.F03Button
            // 
            resources.ApplyResources(this.fbrFunction.F03Button, "fbrFunction.F03Button");
            // 
            // fbrFunction.F06Button
            // 
            resources.ApplyResources(this.fbrFunction.F06Button, "fbrFunction.F06Button");
            // 
            // fbrFunction.F07Button
            // 
            resources.ApplyResources(this.fbrFunction.F07Button, "fbrFunction.F07Button");
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
            // fbrFunction.F11Button
            // 
            resources.ApplyResources(this.fbrFunction.F11Button, "fbrFunction.F11Button");
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // lblExcel
            // 
            // 
            // lblExcel.ChildPanel
            // 
            this.lblExcel.ChildPanel.Controls.Add(this.txtExcel);
            this.lblExcel.IsFocusChangeColor = false;
            this.lblExcel.IsNecessary = true;
            this.lblExcel.LabelWidth = 100;
            resources.ApplyResources(this.lblExcel, "lblExcel");
            this.lblExcel.Name = "lblExcel";
            this.lblExcel.SplitterWidth = 0;
            this.lblExcel.TabStop = false;
            // 
            // txtExcel
            // 
            this.txtExcel.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtExcel, "txtExcel");
            this.txtExcel.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtExcel.InputRegulation = "ABNRLKSHFabnrlksz";
            this.txtExcel.Name = "txtExcel";
            this.txtExcel.ProhibitionChar = null;
            this.txtExcel.ReadOnly = true;
            this.txtExcel.TabStop = false;
            // 
            // lblShipSeiban
            // 
            resources.ApplyResources(this.lblShipSeiban, "lblShipSeiban");
            // 
            // lblShipSeiban.ChildPanel
            // 
            this.lblShipSeiban.ChildPanel.Controls.Add(this.txtShipSeiban);
            this.lblShipSeiban.IsFocusChangeColor = false;
            this.lblShipSeiban.IsNecessary = true;
            this.lblShipSeiban.LabelWidth = 100;
            this.lblShipSeiban.Name = "lblShipSeiban";
            this.lblShipSeiban.SplitterWidth = 0;
            // 
            // txtShipSeiban
            // 
            resources.ApplyResources(this.txtShipSeiban, "txtShipSeiban");
            this.txtShipSeiban.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtShipSeiban.InputRegulation = "abn";
            this.txtShipSeiban.IsInputRegulation = true;
            this.txtShipSeiban.MaxByteLengthMode = true;
            this.txtShipSeiban.Name = "txtShipSeiban";
            this.txtShipSeiban.OneLineMaxLength = 10;
            this.txtShipSeiban.ProhibitionChar = null;
            this.txtShipSeiban.TextChanged += new System.EventHandler(this.txtShipSeiban_TextChanged);
            // 
            // shtMeisai
            // 
            resources.ApplyResources(this.shtMeisai, "shtMeisai");
            this.shtMeisai.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtMeisai.Data")));
            this.shtMeisai.Name = "shtMeisai";
            this.shtMeisai.BindingError += new System.EventHandler<GrapeCity.Win.ElTabelle.BindingErrorEventArgs>(this.shtMeisai_BindingError);
            this.shtMeisai.ValueChanged += new GrapeCity.Win.ElTabelle.ValueChangedEventHandler(this.shtMeisai_ValueChanged);
            this.shtMeisai.EnterEdit += new GrapeCity.Win.ElTabelle.EnterEditEventHandler(this.shtMeisai_EnterEdit);
            // 
            // grpSearch
            // 
            this.grpSearch.Controls.Add(this.btnStart);
            this.grpSearch.Controls.Add(this.btnReference);
            this.grpSearch.Controls.Add(this.lblExcel);
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
            // 
            // btnStart
            // 
            resources.ApplyResources(this.btnStart, "btnStart");
            this.btnStart.Name = "btnStart";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnReference
            // 
            resources.ApplyResources(this.btnReference, "btnReference");
            this.btnReference.Name = "btnReference";
            this.btnReference.TabStop = false;
            this.btnReference.Click += new System.EventHandler(this.btnReference_Click);
            // 
            // ofdExcel
            // 
            resources.ApplyResources(this.ofdExcel, "ofdExcel");
            // 
            // lblSeiban
            // 
            resources.ApplyResources(this.lblSeiban, "lblSeiban");
            // 
            // lblSeiban.ChildPanel
            // 
            this.lblSeiban.ChildPanel.Controls.Add(this.txtRevision);
            this.lblSeiban.IsFocusChangeColor = false;
            this.lblSeiban.IsNecessary = true;
            this.lblSeiban.LabelWidth = 55;
            this.lblSeiban.Name = "lblSeiban";
            this.lblSeiban.NecessaryBackColor = System.Drawing.Color.LightSkyBlue;
            this.lblSeiban.SplitterWidth = 0;
            // 
            // txtRevision
            // 
            this.txtRevision.AutoPad = true;
            this.txtRevision.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtRevision, "txtRevision");
            this.txtRevision.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtRevision.InputRegulation = "F";
            this.txtRevision.MaxByteLengthMode = true;
            this.txtRevision.Name = "txtRevision";
            this.txtRevision.OneLineMaxLength = 3;
            this.txtRevision.PaddingChar = '0';
            this.txtRevision.ProhibitionChar = null;
            this.txtRevision.ReadOnly = true;
            this.txtRevision.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // btnSearch
            // 
            resources.ApplyResources(this.btnSearch, "btnSearch");
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Click += new System.EventHandler(this.btnKaishi_Click);
            // 
            // lblNiukesaki
            // 
            // 
            // lblNiukesaki.ChildPanel
            // 
            this.lblNiukesaki.ChildPanel.Controls.Add(this.cboNiukesaki);
            this.lblNiukesaki.IsFocusChangeColor = false;
            this.lblNiukesaki.IsNecessary = true;
            this.lblNiukesaki.LabelWidth = 60;
            resources.ApplyResources(this.lblNiukesaki, "lblNiukesaki");
            this.lblNiukesaki.Name = "lblNiukesaki";
            this.lblNiukesaki.SplitterWidth = 0;
            // 
            // cboNiukesaki
            // 
            this.cboNiukesaki.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboNiukesaki.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.cboNiukesaki, "cboNiukesaki");
            this.cboNiukesaki.Name = "cboNiukesaki";
            this.cboNiukesaki.SelectedValueChanged += new System.EventHandler(this.cboNiukesaki_SelectedValueChanged);
            // 
            // lblBoxAssign
            // 
            resources.ApplyResources(this.lblBoxAssign, "lblBoxAssign");
            // 
            // lblBoxAssign.ChildPanel
            // 
            this.lblBoxAssign.ChildPanel.Controls.Add(this.txtBoxAssign);
            this.lblBoxAssign.IsFocusChangeColor = false;
            this.lblBoxAssign.LabelWidth = 130;
            this.lblBoxAssign.Name = "lblBoxAssign";
            this.lblBoxAssign.SplitterWidth = 0;
            // 
            // txtBoxAssign
            // 
            this.txtBoxAssign.AcceptsReturn = true;
            this.txtBoxAssign.AcceptsTab = true;
            this.txtBoxAssign.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.txtBoxAssign, "txtBoxAssign");
            this.txtBoxAssign.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtBoxAssign.InputRegulation = "F";
            this.txtBoxAssign.MaxByteLengthMode = true;
            this.txtBoxAssign.MaxLineCount = 100;
            this.txtBoxAssign.Name = "txtBoxAssign";
            this.txtBoxAssign.OneLineMaxLength = 70;
            this.txtBoxAssign.ProhibitionChar = null;
            // 
            // txtRev
            // 
            this.txtRev.AutoPad = true;
            resources.ApplyResources(this.txtRev, "txtRev");
            this.txtRev.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtRev.InputRegulation = "n";
            this.txtRev.IsInputRegulation = true;
            this.txtRev.MaxByteLengthMode = true;
            this.txtRev.Name = "txtRev";
            this.txtRev.OneLineMaxLength = 3;
            this.txtRev.PaddingChar = '0';
            this.txtRev.ProhibitionChar = null;
            // 
            // cboBukkenName
            // 
            this.cboBukkenName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboBukkenName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.cboBukkenName, "cboBukkenName");
            this.cboBukkenName.Name = "cboBukkenName";
            // 
            // lblBukkenNameTorikomi
            // 
            // 
            // lblBukkenNameTorikomi.ChildPanel
            // 
            this.lblBukkenNameTorikomi.ChildPanel.Controls.Add(this.cboBukkenNameTorikomi);
            this.lblBukkenNameTorikomi.IsFocusChangeColor = false;
            this.lblBukkenNameTorikomi.IsNecessary = true;
            this.lblBukkenNameTorikomi.LabelWidth = 100;
            resources.ApplyResources(this.lblBukkenNameTorikomi, "lblBukkenNameTorikomi");
            this.lblBukkenNameTorikomi.Name = "lblBukkenNameTorikomi";
            this.lblBukkenNameTorikomi.SplitterWidth = 0;
            // 
            // cboBukkenNameTorikomi
            // 
            this.cboBukkenNameTorikomi.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboBukkenNameTorikomi.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.cboBukkenNameTorikomi, "cboBukkenNameTorikomi");
            this.cboBukkenNameTorikomi.Name = "cboBukkenNameTorikomi";
            // 
            // dswComboBox2
            // 
            this.dswComboBox2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.dswComboBox2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.dswComboBox2, "dswComboBox2");
            this.dswComboBox2.Name = "dswComboBox2";
            // 
            // ShukkaKeikaku
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ShukkaKeikaku";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ShukkaKeikaku_FormClosed);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.lblExcel.ChildPanel.ResumeLayout(false);
            this.lblExcel.ChildPanel.PerformLayout();
            this.lblExcel.ResumeLayout(false);
            this.lblShipSeiban.ChildPanel.ResumeLayout(false);
            this.lblShipSeiban.ChildPanel.PerformLayout();
            this.lblShipSeiban.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).EndInit();
            this.grpSearch.ResumeLayout(false);
            this.lblSeiban.ChildPanel.ResumeLayout(false);
            this.lblSeiban.ChildPanel.PerformLayout();
            this.lblSeiban.ResumeLayout(false);
            this.lblNiukesaki.ChildPanel.ResumeLayout(false);
            this.lblNiukesaki.ResumeLayout(false);
            this.lblBoxAssign.ChildPanel.ResumeLayout(false);
            this.lblBoxAssign.ChildPanel.PerformLayout();
            this.lblBoxAssign.ResumeLayout(false);
            this.lblBukkenNameTorikomi.ChildPanel.ResumeLayout(false);
            this.lblBukkenNameTorikomi.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GrapeCity.Win.ElTabelle.Sheet shtMeisai;
        private System.Windows.Forms.GroupBox grpSearch;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnStart;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnReference;
        private DSWControl.DSWLabel.DSWLabel lblExcel;
        private DSWControl.DSWTextBox.DSWTextBox txtExcel;
        private DSWControl.DSWLabel.DSWLabel lblShipSeiban;
        private DSWControl.DSWTextBox.DSWTextBox txtShipSeiban;
        private System.Windows.Forms.OpenFileDialog ofdExcel;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnSearch;
        private DSWControl.DSWLabel.DSWLabel lblSeiban;
        private DSWControl.DSWTextBox.DSWTextBox txtRevision;
        private DSWControl.DSWLabel.DSWLabel lblNiukesaki;
        private DSWControl.DSWComboBox.DSWComboBox cboNiukesaki;
        private DSWControl.DSWLabel.DSWLabel lblBoxAssign;
        private DSWControl.DSWTextBox.DSWTextBox txtBoxAssign;
        private DSWControl.DSWTextBox.DSWTextBox txtRev;
        private DSWControl.DSWLabel.DSWLabel lblBukkenNameTorikomi;
        private DSWControl.DSWComboBox.DSWComboBox cboBukkenNameTorikomi;
        private DSWControl.DSWComboBox.DSWComboBox cboBukkenName;
        private DSWControl.DSWComboBox.DSWComboBox dswComboBox2;
        //private DSWControl.DSWTextBox.DSWTextBox txtBukkenName;
    }
}