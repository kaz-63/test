namespace SMS.S01.Forms
{
    partial class ShukkakeikakuRenkei
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShukkakeikakuRenkei));
            this.cboShukkaFlag = new DSWControl.DSWComboBox.DSWComboBox();
            this.shtTagRenkeiList = new GrapeCity.Win.ElTabelle.Sheet();
            this.btnAllCheck = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnAllNotCheck = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblSyukaFlag = new DSWControl.DSWLabel.DSWLabel();
            this.lblSeiban = new DSWControl.DSWLabel.DSWLabel();
            this.txtSeiban = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblECS = new DSWControl.DSWLabel.DSWLabel();
            this.txtECS = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnDisp = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.lblNounyusakiCD = new DSWControl.DSWLabel.DSWLabel();
            this.txtNounyusakiCD = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblShipData = new DSWControl.DSWLabel.DSWLabel();
            this.cboShipData = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblShip = new DSWControl.DSWLabel.DSWLabel();
            this.cboShip = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblBukkenmei = new DSWControl.DSWLabel.DSWLabel();
            this.cboBukkenmei = new DSWControl.DSWComboBox.DSWComboBox();
            this.btnRangeNotCheck = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnRangeCheck = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtTagRenkeiList)).BeginInit();
            this.lblSyukaFlag.ChildPanel.SuspendLayout();
            this.lblSyukaFlag.SuspendLayout();
            this.lblSeiban.ChildPanel.SuspendLayout();
            this.lblSeiban.SuspendLayout();
            this.lblECS.ChildPanel.SuspendLayout();
            this.lblECS.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.lblNounyusakiCD.ChildPanel.SuspendLayout();
            this.lblNounyusakiCD.SuspendLayout();
            this.lblShipData.ChildPanel.SuspendLayout();
            this.lblShipData.SuspendLayout();
            this.lblShip.ChildPanel.SuspendLayout();
            this.lblShip.SuspendLayout();
            this.lblBukkenmei.ChildPanel.SuspendLayout();
            this.lblBukkenmei.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.btnRangeNotCheck);
            this.pnlMain.Controls.Add(this.btnRangeCheck);
            this.pnlMain.Controls.Add(this.btnAllNotCheck);
            this.pnlMain.Controls.Add(this.btnAllCheck);
            this.pnlMain.Controls.Add(this.grpSearch);
            this.pnlMain.Controls.Add(this.shtTagRenkeiList);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.SetChildIndex(this.shtTagRenkeiList, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnAllCheck, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnAllNotCheck, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnRangeCheck, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnRangeNotCheck, 0);
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
            // fbrFunction.F04Button
            // 
            resources.ApplyResources(this.fbrFunction.F04Button, "fbrFunction.F04Button");
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
            // cboShukkaFlag
            // 
            resources.ApplyResources(this.cboShukkaFlag, "cboShukkaFlag");
            this.cboShukkaFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboShukkaFlag.Name = "cboShukkaFlag";
            this.cboShukkaFlag.SelectedIndexChanged += new System.EventHandler(this.cboShukkaFlag_SelectedIndexChanged);
            // 
            // shtTagRenkeiList
            // 
            resources.ApplyResources(this.shtTagRenkeiList, "shtTagRenkeiList");
            this.shtTagRenkeiList.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtTagRenkeiList.Data")));
            this.shtTagRenkeiList.Name = "shtTagRenkeiList";
            // 
            // btnAllCheck
            // 
            resources.ApplyResources(this.btnAllCheck, "btnAllCheck");
            this.btnAllCheck.Name = "btnAllCheck";
            this.btnAllCheck.Click += new System.EventHandler(this.btnAllCheck_Click);
            // 
            // btnAllNotCheck
            // 
            resources.ApplyResources(this.btnAllNotCheck, "btnAllNotCheck");
            this.btnAllNotCheck.Name = "btnAllNotCheck";
            this.btnAllNotCheck.Click += new System.EventHandler(this.btnAllNotCheck_Click);
            // 
            // lblSyukaFlag
            // 
            // 
            // lblSyukaFlag.ChildPanel
            // 
            this.lblSyukaFlag.ChildPanel.Controls.Add(this.cboShukkaFlag);
            this.lblSyukaFlag.IsFocusChangeColor = false;
            this.lblSyukaFlag.IsNecessary = true;
            this.lblSyukaFlag.LabelWidth = 100;
            resources.ApplyResources(this.lblSyukaFlag, "lblSyukaFlag");
            this.lblSyukaFlag.Name = "lblSyukaFlag";
            this.lblSyukaFlag.SplitterWidth = 0;
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
            this.txtSeiban.MaxByteLengthMode = true;
            this.txtSeiban.Name = "txtSeiban";
            this.txtSeiban.OneLineMaxLength = 110;
            this.txtSeiban.ProhibitionChar = null;
            // 
            // lblECS
            // 
            // 
            // lblECS.ChildPanel
            // 
            this.lblECS.ChildPanel.Controls.Add(this.txtECS);
            this.lblECS.IsFocusChangeColor = false;
            this.lblECS.LabelWidth = 80;
            resources.ApplyResources(this.lblECS, "lblECS");
            this.lblECS.Name = "lblECS";
            this.lblECS.SplitterWidth = 0;
            // 
            // txtECS
            // 
            resources.ApplyResources(this.txtECS, "txtECS");
            this.txtECS.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtECS.InputRegulation = "F";
            this.txtECS.MaxByteLengthMode = true;
            this.txtECS.Name = "txtECS";
            this.txtECS.OneLineMaxLength = 77;
            this.txtECS.ProhibitionChar = ((System.Collections.Generic.Dictionary<int, string>)(resources.GetObject("txtECS.ProhibitionChar")));
            // 
            // btnDisp
            // 
            resources.ApplyResources(this.btnDisp, "btnDisp");
            this.btnDisp.Name = "btnDisp";
            this.btnDisp.Click += new System.EventHandler(this.btnDisp_Click);
            // 
            // grpSearch
            // 
            this.grpSearch.Controls.Add(this.lblNounyusakiCD);
            this.grpSearch.Controls.Add(this.lblShipData);
            this.grpSearch.Controls.Add(this.lblShip);
            this.grpSearch.Controls.Add(this.lblBukkenmei);
            this.grpSearch.Controls.Add(this.btnDisp);
            this.grpSearch.Controls.Add(this.lblSyukaFlag);
            this.grpSearch.Controls.Add(this.lblECS);
            this.grpSearch.Controls.Add(this.lblSeiban);
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
            // 
            // lblNounyusakiCD
            // 
            // 
            // lblNounyusakiCD.ChildPanel
            // 
            this.lblNounyusakiCD.ChildPanel.Controls.Add(this.txtNounyusakiCD);
            this.lblNounyusakiCD.IsFocusChangeColor = false;
            this.lblNounyusakiCD.LabelWidth = 80;
            resources.ApplyResources(this.lblNounyusakiCD, "lblNounyusakiCD");
            this.lblNounyusakiCD.Name = "lblNounyusakiCD";
            this.lblNounyusakiCD.SplitterWidth = 0;
            // 
            // txtNounyusakiCD
            // 
            resources.ApplyResources(this.txtNounyusakiCD, "txtNounyusakiCD");
            this.txtNounyusakiCD.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtNounyusakiCD.InputRegulation = "F";
            this.txtNounyusakiCD.Name = "txtNounyusakiCD";
            this.txtNounyusakiCD.OneLineMaxLength = 77;
            this.txtNounyusakiCD.ProhibitionChar = ((System.Collections.Generic.Dictionary<int, string>)(resources.GetObject("txtNounyusakiCD.ProhibitionChar")));
            // 
            // lblShipData
            // 
            // 
            // lblShipData.ChildPanel
            // 
            this.lblShipData.ChildPanel.Controls.Add(this.cboShipData);
            this.lblShipData.IsFocusChangeColor = false;
            this.lblShipData.IsNecessary = true;
            this.lblShipData.LabelWidth = 80;
            resources.ApplyResources(this.lblShipData, "lblShipData");
            this.lblShipData.Name = "lblShipData";
            this.lblShipData.SplitterWidth = 0;
            // 
            // cboShipData
            // 
            resources.ApplyResources(this.cboShipData, "cboShipData");
            this.cboShipData.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboShipData.Name = "cboShipData";
            // 
            // lblShip
            // 
            // 
            // lblShip.ChildPanel
            // 
            this.lblShip.ChildPanel.Controls.Add(this.cboShip);
            this.lblShip.IsFocusChangeColor = false;
            this.lblShip.IsNecessary = true;
            this.lblShip.LabelWidth = 100;
            resources.ApplyResources(this.lblShip, "lblShip");
            this.lblShip.Name = "lblShip";
            this.lblShip.SplitterWidth = 0;
            // 
            // cboShip
            // 
            this.cboShip.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboShip.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.cboShip, "cboShip");
            this.cboShip.Name = "cboShip";
            this.cboShip.SelectedIndexChanged += new System.EventHandler(this.cboShip_SelectedIndexChanged);
            // 
            // lblBukkenmei
            // 
            // 
            // lblBukkenmei.ChildPanel
            // 
            this.lblBukkenmei.ChildPanel.Controls.Add(this.cboBukkenmei);
            this.lblBukkenmei.IsFocusChangeColor = false;
            this.lblBukkenmei.IsNecessary = true;
            this.lblBukkenmei.LabelWidth = 100;
            resources.ApplyResources(this.lblBukkenmei, "lblBukkenmei");
            this.lblBukkenmei.Name = "lblBukkenmei";
            this.lblBukkenmei.SplitterWidth = 0;
            // 
            // cboBukkenmei
            // 
            this.cboBukkenmei.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboBukkenmei.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.cboBukkenmei, "cboBukkenmei");
            this.cboBukkenmei.Name = "cboBukkenmei";
            this.cboBukkenmei.SelectedIndexChanged += new System.EventHandler(this.cboBukkenmei_SelectedIndexChanged);
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
            // ShukkakeikakuRenkei
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ShukkakeikakuRenkei";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtTagRenkeiList)).EndInit();
            this.lblSyukaFlag.ChildPanel.ResumeLayout(false);
            this.lblSyukaFlag.ResumeLayout(false);
            this.lblSeiban.ChildPanel.ResumeLayout(false);
            this.lblSeiban.ChildPanel.PerformLayout();
            this.lblSeiban.ResumeLayout(false);
            this.lblECS.ChildPanel.ResumeLayout(false);
            this.lblECS.ChildPanel.PerformLayout();
            this.lblECS.ResumeLayout(false);
            this.grpSearch.ResumeLayout(false);
            this.lblNounyusakiCD.ChildPanel.ResumeLayout(false);
            this.lblNounyusakiCD.ChildPanel.PerformLayout();
            this.lblNounyusakiCD.ResumeLayout(false);
            this.lblShipData.ChildPanel.ResumeLayout(false);
            this.lblShipData.ResumeLayout(false);
            this.lblShip.ChildPanel.ResumeLayout(false);
            this.lblShip.ResumeLayout(false);
            this.lblBukkenmei.ChildPanel.ResumeLayout(false);
            this.lblBukkenmei.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DSWControl.DSWComboBox.DSWComboBox cboShukkaFlag;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnDisp;
        private DSWControl.DSWLabel.DSWLabel lblECS;
        private DSWControl.DSWTextBox.DSWTextBox txtECS;
        private DSWControl.DSWLabel.DSWLabel lblSeiban;
        private DSWControl.DSWTextBox.DSWTextBox txtSeiban;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnAllNotCheck;
        private DSWControl.DSWLabel.DSWLabel lblSyukaFlag;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnAllCheck;
        private System.Windows.Forms.GroupBox grpSearch;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnRangeNotCheck;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnRangeCheck;
        private DSWControl.DSWLabel.DSWLabel lblShip;
        private DSWControl.DSWComboBox.DSWComboBox cboShip;
        private DSWControl.DSWLabel.DSWLabel lblBukkenmei;
        private DSWControl.DSWComboBox.DSWComboBox cboBukkenmei;
        private DSWControl.DSWLabel.DSWLabel lblShipData;
        private DSWControl.DSWComboBox.DSWComboBox cboShipData;
        private DSWControl.DSWLabel.DSWLabel lblNounyusakiCD;
        private DSWControl.DSWTextBox.DSWTextBox txtNounyusakiCD;
        private GrapeCity.Win.ElTabelle.Sheet shtTagRenkeiList;

    }
}