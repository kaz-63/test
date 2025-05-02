namespace SMS.T01.Forms
{
    partial class TehaiRenkei
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TehaiRenkei));
            this.lblBukkenName = new DSWControl.DSWLabel.DSWLabel();
            this.cboBukkenName = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblTehaiNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtTehaiNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblZumenKeishiki = new DSWControl.DSWLabel.DSWLabel();
            this.txtZumenKeishiki = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblEcsNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtEcsNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblCode = new DSWControl.DSWLabel.DSWLabel();
            this.txtCode = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblSeiban = new DSWControl.DSWLabel.DSWLabel();
            this.txtSeiban = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblCond = new DSWControl.DSWLabel.DSWLabel();
            this.cboCond = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblLastLink = new DSWControl.DSWLabel.DSWLabel();
            this.txtLastLink = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblTehaiUnitPrice = new DSWControl.DSWLabel.DSWLabel();
            this.numTehaiUnitPrice = new DSWControl.DSWNumericTextBox();
            this.lblShukkaQty = new DSWControl.DSWLabel.DSWLabel();
            this.numShukkaQty = new DSWControl.DSWNumericTextBox();
            this.lblHacchuQty = new DSWControl.DSWLabel.DSWLabel();
            this.numHacchuQty = new DSWControl.DSWNumericTextBox();
            this.lblCheckQty = new DSWControl.DSWLabel.DSWLabel();
            this.numCheckQty = new DSWControl.DSWNumericTextBox();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.btnStart = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnWatari = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnMatomeAll = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnMatome = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnEstimateAll = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnOther = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.grpDetail = new System.Windows.Forms.GroupBox();
            this.shtMeisai = new GrapeCity.Win.ElTabelle.Sheet();
            this.shtResult = new GrapeCity.Win.ElTabelle.Sheet();
            this.grpOperation = new System.Windows.Forms.GroupBox();
            this.pnlMain.SuspendLayout();
            this.lblBukkenName.ChildPanel.SuspendLayout();
            this.lblBukkenName.SuspendLayout();
            this.lblTehaiNo.ChildPanel.SuspendLayout();
            this.lblTehaiNo.SuspendLayout();
            this.lblZumenKeishiki.ChildPanel.SuspendLayout();
            this.lblZumenKeishiki.SuspendLayout();
            this.lblEcsNo.ChildPanel.SuspendLayout();
            this.lblEcsNo.SuspendLayout();
            this.lblCode.ChildPanel.SuspendLayout();
            this.lblCode.SuspendLayout();
            this.lblSeiban.ChildPanel.SuspendLayout();
            this.lblSeiban.SuspendLayout();
            this.lblCond.ChildPanel.SuspendLayout();
            this.lblCond.SuspendLayout();
            this.lblLastLink.ChildPanel.SuspendLayout();
            this.lblLastLink.SuspendLayout();
            this.lblTehaiUnitPrice.ChildPanel.SuspendLayout();
            this.lblTehaiUnitPrice.SuspendLayout();
            this.lblShukkaQty.ChildPanel.SuspendLayout();
            this.lblShukkaQty.SuspendLayout();
            this.lblHacchuQty.ChildPanel.SuspendLayout();
            this.lblHacchuQty.SuspendLayout();
            this.lblCheckQty.ChildPanel.SuspendLayout();
            this.lblCheckQty.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.grpDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).BeginInit();
            this.grpOperation.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.shtResult);
            this.pnlMain.Controls.Add(this.grpDetail);
            this.pnlMain.Controls.Add(this.lblLastLink);
            this.pnlMain.Controls.Add(this.grpSearch);
            this.pnlMain.Controls.Add(this.grpOperation);
            this.pnlMain.Controls.SetChildIndex(this.grpOperation, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblLastLink, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpDetail, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtResult, 0);
            // 
            // fbrFunction
            // 
            // 
            // fbrFunction.F01Button
            // 
            resources.ApplyResources(this.fbrFunction.F01Button, "fbrFunction.F01Button");
            resources.ApplyResources(this.fbrFunction, "fbrFunction");
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
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // lblBukkenName
            // 
            // 
            // lblBukkenName.ChildPanel
            // 
            this.lblBukkenName.ChildPanel.Controls.Add(this.cboBukkenName);
            this.lblBukkenName.IsFocusChangeColor = false;
            this.lblBukkenName.IsNecessary = true;
            this.lblBukkenName.LabelWidth = 100;
            resources.ApplyResources(this.lblBukkenName, "lblBukkenName");
            this.lblBukkenName.Name = "lblBukkenName";
            this.lblBukkenName.SplitterWidth = 0;
            // 
            // cboBukkenName
            // 
            this.cboBukkenName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboBukkenName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.cboBukkenName, "cboBukkenName");
            this.cboBukkenName.Name = "cboBukkenName";
            // 
            // lblTehaiNo
            // 
            // 
            // lblTehaiNo.ChildPanel
            // 
            this.lblTehaiNo.ChildPanel.Controls.Add(this.txtTehaiNo);
            this.lblTehaiNo.IsFocusChangeColor = false;
            this.lblTehaiNo.LabelWidth = 100;
            resources.ApplyResources(this.lblTehaiNo, "lblTehaiNo");
            this.lblTehaiNo.Name = "lblTehaiNo";
            this.lblTehaiNo.SplitterWidth = 0;
            // 
            // txtTehaiNo
            // 
            resources.ApplyResources(this.txtTehaiNo, "txtTehaiNo");
            this.txtTehaiNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtTehaiNo.InputRegulation = "F";
            this.txtTehaiNo.Name = "txtTehaiNo";
            this.txtTehaiNo.OneLineMaxLength = 8;
            this.txtTehaiNo.ProhibitionChar = null;
            this.txtTehaiNo.Leave += new System.EventHandler(this.txtTehaiNo_Leave);
            // 
            // lblZumenKeishiki
            // 
            // 
            // lblZumenKeishiki.ChildPanel
            // 
            this.lblZumenKeishiki.ChildPanel.Controls.Add(this.txtZumenKeishiki);
            this.lblZumenKeishiki.IsFocusChangeColor = false;
            this.lblZumenKeishiki.LabelWidth = 110;
            resources.ApplyResources(this.lblZumenKeishiki, "lblZumenKeishiki");
            this.lblZumenKeishiki.Name = "lblZumenKeishiki";
            this.lblZumenKeishiki.SplitterWidth = 0;
            // 
            // txtZumenKeishiki
            // 
            resources.ApplyResources(this.txtZumenKeishiki, "txtZumenKeishiki");
            this.txtZumenKeishiki.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtZumenKeishiki.InputRegulation = "F";
            this.txtZumenKeishiki.Name = "txtZumenKeishiki";
            this.txtZumenKeishiki.OneLineMaxLength = 100;
            this.txtZumenKeishiki.ProhibitionChar = null;
            // 
            // lblEcsNo
            // 
            // 
            // lblEcsNo.ChildPanel
            // 
            this.lblEcsNo.ChildPanel.Controls.Add(this.txtEcsNo);
            this.lblEcsNo.IsFocusChangeColor = false;
            this.lblEcsNo.LabelWidth = 100;
            resources.ApplyResources(this.lblEcsNo, "lblEcsNo");
            this.lblEcsNo.Name = "lblEcsNo";
            this.lblEcsNo.SplitterWidth = 0;
            // 
            // txtEcsNo
            // 
            resources.ApplyResources(this.txtEcsNo, "txtEcsNo");
            this.txtEcsNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtEcsNo.InputRegulation = "F";
            this.txtEcsNo.Name = "txtEcsNo";
            this.txtEcsNo.OneLineMaxLength = 20;
            this.txtEcsNo.ProhibitionChar = null;
            // 
            // lblCode
            // 
            // 
            // lblCode.ChildPanel
            // 
            this.lblCode.ChildPanel.Controls.Add(this.txtCode);
            this.lblCode.IsFocusChangeColor = false;
            this.lblCode.LabelWidth = 100;
            resources.ApplyResources(this.lblCode, "lblCode");
            this.lblCode.Name = "lblCode";
            this.lblCode.SplitterWidth = 0;
            // 
            // txtCode
            // 
            resources.ApplyResources(this.txtCode, "txtCode");
            this.txtCode.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtCode.InputRegulation = "F";
            this.txtCode.Name = "txtCode";
            this.txtCode.OneLineMaxLength = 3;
            this.txtCode.ProhibitionChar = null;
            // 
            // lblSeiban
            // 
            // 
            // lblSeiban.ChildPanel
            // 
            this.lblSeiban.ChildPanel.Controls.Add(this.txtSeiban);
            this.lblSeiban.IsFocusChangeColor = false;
            this.lblSeiban.LabelWidth = 100;
            resources.ApplyResources(this.lblSeiban, "lblSeiban");
            this.lblSeiban.Name = "lblSeiban";
            this.lblSeiban.SplitterWidth = 0;
            // 
            // txtSeiban
            // 
            resources.ApplyResources(this.txtSeiban, "txtSeiban");
            this.txtSeiban.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtSeiban.InputRegulation = "F";
            this.txtSeiban.Name = "txtSeiban";
            this.txtSeiban.OneLineMaxLength = 12;
            this.txtSeiban.ProhibitionChar = null;
            // 
            // lblCond
            // 
            // 
            // lblCond.ChildPanel
            // 
            this.lblCond.ChildPanel.Controls.Add(this.cboCond);
            this.lblCond.IsFocusChangeColor = false;
            this.lblCond.IsNecessary = true;
            this.lblCond.LabelWidth = 100;
            resources.ApplyResources(this.lblCond, "lblCond");
            this.lblCond.Name = "lblCond";
            this.lblCond.SplitterWidth = 0;
            // 
            // cboCond
            // 
            resources.ApplyResources(this.cboCond, "cboCond");
            this.cboCond.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCond.Name = "cboCond";
            // 
            // lblLastLink
            // 
            resources.ApplyResources(this.lblLastLink, "lblLastLink");
            // 
            // lblLastLink.ChildPanel
            // 
            this.lblLastLink.ChildPanel.Controls.Add(this.txtLastLink);
            this.lblLastLink.IsFocusChangeColor = false;
            this.lblLastLink.LabelWidth = 130;
            this.lblLastLink.Name = "lblLastLink";
            this.lblLastLink.NormalBackColor = System.Drawing.Color.DeepSkyBlue;
            this.lblLastLink.SplitterWidth = 0;
            // 
            // txtLastLink
            // 
            this.txtLastLink.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtLastLink, "txtLastLink");
            this.txtLastLink.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtLastLink.Name = "txtLastLink";
            this.txtLastLink.OneLineMaxLength = 11;
            this.txtLastLink.ProhibitionChar = null;
            this.txtLastLink.ReadOnly = true;
            // 
            // lblTehaiUnitPrice
            // 
            resources.ApplyResources(this.lblTehaiUnitPrice, "lblTehaiUnitPrice");
            // 
            // lblTehaiUnitPrice.ChildPanel
            // 
            this.lblTehaiUnitPrice.ChildPanel.Controls.Add(this.numTehaiUnitPrice);
            this.lblTehaiUnitPrice.IsFocusChangeColor = false;
            this.lblTehaiUnitPrice.LabelWidth = 100;
            this.lblTehaiUnitPrice.Name = "lblTehaiUnitPrice";
            this.lblTehaiUnitPrice.NormalBackColor = System.Drawing.Color.DeepSkyBlue;
            this.lblTehaiUnitPrice.SplitterWidth = 0;
            // 
            // numTehaiUnitPrice
            // 
            this.numTehaiUnitPrice.AllowEmpty = true;
            resources.ApplyResources(this.numTehaiUnitPrice, "numTehaiUnitPrice");
            this.numTehaiUnitPrice.IntLength = 9;
            this.numTehaiUnitPrice.IsSelectAll = true;
            this.numTehaiUnitPrice.IsThousands = true;
            this.numTehaiUnitPrice.MaxValue = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numTehaiUnitPrice.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTehaiUnitPrice.Name = "numTehaiUnitPrice";
            this.numTehaiUnitPrice.ReadOnly = true;
            // 
            // lblShukkaQty
            // 
            resources.ApplyResources(this.lblShukkaQty, "lblShukkaQty");
            // 
            // lblShukkaQty.ChildPanel
            // 
            this.lblShukkaQty.ChildPanel.Controls.Add(this.numShukkaQty);
            this.lblShukkaQty.IsFocusChangeColor = false;
            this.lblShukkaQty.LabelWidth = 100;
            this.lblShukkaQty.Name = "lblShukkaQty";
            this.lblShukkaQty.NormalBackColor = System.Drawing.Color.PaleGreen;
            this.lblShukkaQty.SplitterWidth = 0;
            // 
            // numShukkaQty
            // 
            this.numShukkaQty.AllowEmpty = true;
            resources.ApplyResources(this.numShukkaQty, "numShukkaQty");
            this.numShukkaQty.IntLength = 6;
            this.numShukkaQty.IsSelectAll = true;
            this.numShukkaQty.IsThousands = true;
            this.numShukkaQty.MaxValue = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numShukkaQty.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numShukkaQty.Name = "numShukkaQty";
            this.numShukkaQty.ReadOnly = true;
            // 
            // lblHacchuQty
            // 
            resources.ApplyResources(this.lblHacchuQty, "lblHacchuQty");
            // 
            // lblHacchuQty.ChildPanel
            // 
            this.lblHacchuQty.ChildPanel.Controls.Add(this.numHacchuQty);
            this.lblHacchuQty.IsFocusChangeColor = false;
            this.lblHacchuQty.LabelWidth = 100;
            this.lblHacchuQty.Name = "lblHacchuQty";
            this.lblHacchuQty.NormalBackColor = System.Drawing.Color.PaleGreen;
            this.lblHacchuQty.SplitterWidth = 0;
            // 
            // numHacchuQty
            // 
            this.numHacchuQty.AllowEmpty = true;
            resources.ApplyResources(this.numHacchuQty, "numHacchuQty");
            this.numHacchuQty.IntLength = 6;
            this.numHacchuQty.IsSelectAll = true;
            this.numHacchuQty.IsThousands = true;
            this.numHacchuQty.MaxValue = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numHacchuQty.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numHacchuQty.Name = "numHacchuQty";
            this.numHacchuQty.ReadOnly = true;
            // 
            // lblCheckQty
            // 
            resources.ApplyResources(this.lblCheckQty, "lblCheckQty");
            // 
            // lblCheckQty.ChildPanel
            // 
            this.lblCheckQty.ChildPanel.Controls.Add(this.numCheckQty);
            this.lblCheckQty.IsFocusChangeColor = false;
            this.lblCheckQty.LabelWidth = 100;
            this.lblCheckQty.Name = "lblCheckQty";
            this.lblCheckQty.NormalBackColor = System.Drawing.Color.DeepSkyBlue;
            this.lblCheckQty.SplitterWidth = 0;
            // 
            // numCheckQty
            // 
            this.numCheckQty.AllowEmpty = true;
            resources.ApplyResources(this.numCheckQty, "numCheckQty");
            this.numCheckQty.IntLength = 7;
            this.numCheckQty.IsThousands = true;
            this.numCheckQty.MaxValue = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numCheckQty.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCheckQty.Name = "numCheckQty";
            this.numCheckQty.ReadOnly = true;
            // 
            // grpSearch
            // 
            this.grpSearch.Controls.Add(this.lblBukkenName);
            this.grpSearch.Controls.Add(this.btnStart);
            this.grpSearch.Controls.Add(this.lblTehaiNo);
            this.grpSearch.Controls.Add(this.lblZumenKeishiki);
            this.grpSearch.Controls.Add(this.lblEcsNo);
            this.grpSearch.Controls.Add(this.lblCode);
            this.grpSearch.Controls.Add(this.lblSeiban);
            this.grpSearch.Controls.Add(this.lblCond);
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
            // btnWatari
            // 
            resources.ApplyResources(this.btnWatari, "btnWatari");
            this.btnWatari.Name = "btnWatari";
            this.btnWatari.Click += new System.EventHandler(this.btnWatari_Click);
            // 
            // btnMatomeAll
            // 
            resources.ApplyResources(this.btnMatomeAll, "btnMatomeAll");
            this.btnMatomeAll.Name = "btnMatomeAll";
            this.btnMatomeAll.Click += new System.EventHandler(this.btnMatomeAll_Click);
            // 
            // btnMatome
            // 
            resources.ApplyResources(this.btnMatome, "btnMatome");
            this.btnMatome.Name = "btnMatome";
            this.btnMatome.Click += new System.EventHandler(this.btnMatome_Click);
            // 
            // btnEstimateAll
            // 
            resources.ApplyResources(this.btnEstimateAll, "btnEstimateAll");
            this.btnEstimateAll.Name = "btnEstimateAll";
            this.btnEstimateAll.Click += new System.EventHandler(this.btnEstimateAll_Click);
            // 
            // btnOther
            // 
            resources.ApplyResources(this.btnOther, "btnOther");
            this.btnOther.Name = "btnOther";
            this.btnOther.Click += new System.EventHandler(this.btnOther_Click);
            // 
            // grpDetail
            // 
            resources.ApplyResources(this.grpDetail, "grpDetail");
            this.grpDetail.Controls.Add(this.shtMeisai);
            this.grpDetail.Controls.Add(this.lblTehaiUnitPrice);
            this.grpDetail.Controls.Add(this.lblShukkaQty);
            this.grpDetail.Controls.Add(this.lblHacchuQty);
            this.grpDetail.Controls.Add(this.lblCheckQty);
            this.grpDetail.Name = "grpDetail";
            this.grpDetail.TabStop = false;
            // 
            // shtMeisai
            // 
            resources.ApplyResources(this.shtMeisai, "shtMeisai");
            this.shtMeisai.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtMeisai.Data")));
            this.shtMeisai.Name = "shtMeisai";
            this.shtMeisai.LeaveEdit += new GrapeCity.Win.ElTabelle.LeaveEditEventHandler(this.shtMeisai_LeaveEdit);
            // 
            // shtResult
            // 
            resources.ApplyResources(this.shtResult, "shtResult");
            this.shtResult.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtResult.Data")));
            this.shtResult.Name = "shtResult";
            // 
            // grpOperation
            // 
            resources.ApplyResources(this.grpOperation, "grpOperation");
            this.grpOperation.Controls.Add(this.btnOther);
            this.grpOperation.Controls.Add(this.btnEstimateAll);
            this.grpOperation.Controls.Add(this.btnMatome);
            this.grpOperation.Controls.Add(this.btnMatomeAll);
            this.grpOperation.Controls.Add(this.btnWatari);
            this.grpOperation.Name = "grpOperation";
            this.grpOperation.TabStop = false;
            // 
            // TehaiRenkei
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "TehaiRenkei";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.lblBukkenName.ChildPanel.ResumeLayout(false);
            this.lblBukkenName.ResumeLayout(false);
            this.lblTehaiNo.ChildPanel.ResumeLayout(false);
            this.lblTehaiNo.ChildPanel.PerformLayout();
            this.lblTehaiNo.ResumeLayout(false);
            this.lblZumenKeishiki.ChildPanel.ResumeLayout(false);
            this.lblZumenKeishiki.ChildPanel.PerformLayout();
            this.lblZumenKeishiki.ResumeLayout(false);
            this.lblEcsNo.ChildPanel.ResumeLayout(false);
            this.lblEcsNo.ChildPanel.PerformLayout();
            this.lblEcsNo.ResumeLayout(false);
            this.lblCode.ChildPanel.ResumeLayout(false);
            this.lblCode.ChildPanel.PerformLayout();
            this.lblCode.ResumeLayout(false);
            this.lblSeiban.ChildPanel.ResumeLayout(false);
            this.lblSeiban.ChildPanel.PerformLayout();
            this.lblSeiban.ResumeLayout(false);
            this.lblCond.ChildPanel.ResumeLayout(false);
            this.lblCond.ResumeLayout(false);
            this.lblLastLink.ChildPanel.ResumeLayout(false);
            this.lblLastLink.ChildPanel.PerformLayout();
            this.lblLastLink.ResumeLayout(false);
            this.lblTehaiUnitPrice.ChildPanel.ResumeLayout(false);
            this.lblTehaiUnitPrice.ChildPanel.PerformLayout();
            this.lblTehaiUnitPrice.ResumeLayout(false);
            this.lblShukkaQty.ChildPanel.ResumeLayout(false);
            this.lblShukkaQty.ChildPanel.PerformLayout();
            this.lblShukkaQty.ResumeLayout(false);
            this.lblHacchuQty.ChildPanel.ResumeLayout(false);
            this.lblHacchuQty.ChildPanel.PerformLayout();
            this.lblHacchuQty.ResumeLayout(false);
            this.lblCheckQty.ChildPanel.ResumeLayout(false);
            this.lblCheckQty.ChildPanel.PerformLayout();
            this.lblCheckQty.ResumeLayout(false);
            this.grpSearch.ResumeLayout(false);
            this.grpDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).EndInit();
            this.grpOperation.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSearch;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnStart;
        private DSWControl.DSWLabel.DSWLabel lblTehaiNo;
        private DSWControl.DSWTextBox.DSWTextBox txtTehaiNo;
        private DSWControl.DSWLabel.DSWLabel lblZumenKeishiki;
        private DSWControl.DSWTextBox.DSWTextBox txtZumenKeishiki;
        private DSWControl.DSWLabel.DSWLabel lblEcsNo;
        private DSWControl.DSWTextBox.DSWTextBox txtEcsNo;
        private DSWControl.DSWLabel.DSWLabel lblCode;
        private DSWControl.DSWTextBox.DSWTextBox txtCode;
        private DSWControl.DSWLabel.DSWLabel lblSeiban;
        private DSWControl.DSWTextBox.DSWTextBox txtSeiban;
        private DSWControl.DSWLabel.DSWLabel lblCond;
        private DSWControl.DSWComboBox.DSWComboBox cboCond;
        private DSWControl.DSWLabel.DSWLabel lblBukkenName;
        private DSWControl.DSWComboBox.DSWComboBox cboBukkenName;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnWatari;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnOther;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnEstimateAll;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnMatome;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnMatomeAll;
        private DSWControl.DSWLabel.DSWLabel lblLastLink;
        private DSWControl.DSWTextBox.DSWTextBox txtLastLink;
        private System.Windows.Forms.GroupBox grpDetail;
        private DSWControl.DSWLabel.DSWLabel lblCheckQty;
        private DSWControl.DSWLabel.DSWLabel lblHacchuQty;
        private DSWControl.DSWLabel.DSWLabel lblTehaiUnitPrice;
        private DSWControl.DSWLabel.DSWLabel lblShukkaQty;
        private GrapeCity.Win.ElTabelle.Sheet shtResult;
        private GrapeCity.Win.ElTabelle.Sheet shtMeisai;
        private System.Windows.Forms.GroupBox grpOperation;
        private DSWControl.DSWNumericTextBox numCheckQty;
        private DSWControl.DSWNumericTextBox numTehaiUnitPrice;
        private DSWControl.DSWNumericTextBox numShukkaQty;
        private DSWControl.DSWNumericTextBox numHacchuQty;
    }
}