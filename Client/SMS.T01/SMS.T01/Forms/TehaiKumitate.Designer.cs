namespace SMS.T01.Forms
{
    partial class TehaiKumitate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TehaiKumitate));
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.lblBukkenName = new DSWControl.DSWLabel.DSWLabel();
            this.cboBukkenName = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblCODE = new DSWControl.DSWLabel.DSWLabel();
            this.txtCODE = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblSeiban = new DSWControl.DSWLabel.DSWLabel();
            this.txtSeiban = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblARNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtARNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblAR = new DSWControl.DSWControlLinkLabel.DSWControlLinkLabel();
            this.lblECSNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtECSNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblSettiNoukiDate = new DSWControl.DSWLabel.DSWLabel();
            this.cboSetteiNoukiDate = new DSWControl.DSWComboBox.DSWComboBox();
            this.dtpSetteiNoukiDate = new System.Windows.Forms.DateTimePicker();
            this.btnStart = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.shtMeisai = new GrapeCity.Win.ElTabelle.Sheet();
            this.btnAllDeselect = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnAllSelect = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnRangeDeselect = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnRangeSelect = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.ChildPanel = new DSWControl.DSWLabel.CustomPanel();
            this.pnlMain.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.lblBukkenName.ChildPanel.SuspendLayout();
            this.lblBukkenName.SuspendLayout();
            this.lblCODE.ChildPanel.SuspendLayout();
            this.lblCODE.SuspendLayout();
            this.lblSeiban.ChildPanel.SuspendLayout();
            this.lblSeiban.SuspendLayout();
            this.lblARNo.ChildPanel.SuspendLayout();
            this.lblARNo.SuspendLayout();
            this.lblECSNo.ChildPanel.SuspendLayout();
            this.lblECSNo.SuspendLayout();
            this.lblSettiNoukiDate.ChildPanel.SuspendLayout();
            this.lblSettiNoukiDate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.btnRangeDeselect);
            this.pnlMain.Controls.Add(this.btnRangeSelect);
            this.pnlMain.Controls.Add(this.btnAllDeselect);
            this.pnlMain.Controls.Add(this.btnAllSelect);
            this.pnlMain.Controls.Add(this.shtMeisai);
            this.pnlMain.Controls.Add(this.grpSearch);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtMeisai, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnAllSelect, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnAllDeselect, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnRangeSelect, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnRangeDeselect, 0);
            // 
            // fbrFunction
            // 
            // 
            // fbrFunction.F01Button
            // 
            resources.ApplyResources(this.fbrFunction.F01Button, "fbrFunction.F01Button");
            resources.ApplyResources(this.fbrFunction, "fbrFunction");
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
            // grpSearch
            // 
            this.grpSearch.Controls.Add(this.lblBukkenName);
            this.grpSearch.Controls.Add(this.lblCODE);
            this.grpSearch.Controls.Add(this.lblSeiban);
            this.grpSearch.Controls.Add(this.lblARNo);
            this.grpSearch.Controls.Add(this.lblECSNo);
            this.grpSearch.Controls.Add(this.lblSettiNoukiDate);
            this.grpSearch.Controls.Add(this.btnStart);
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
            // 
            // lblBukkenName
            // 
            // 
            // lblBukkenName.ChildPanel
            // 
            this.lblBukkenName.ChildPanel.Controls.Add(this.cboBukkenName);
            this.lblBukkenName.IsFocusChangeColor = false;
            this.lblBukkenName.LabelWidth = 80;
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
            // lblCODE
            // 
            // 
            // lblCODE.ChildPanel
            // 
            this.lblCODE.ChildPanel.Controls.Add(this.txtCODE);
            this.lblCODE.IsFocusChangeColor = false;
            this.lblCODE.LabelWidth = 80;
            resources.ApplyResources(this.lblCODE, "lblCODE");
            this.lblCODE.Name = "lblCODE";
            this.lblCODE.SplitterWidth = 0;
            // 
            // txtCODE
            // 
            resources.ApplyResources(this.txtCODE, "txtCODE");
            this.txtCODE.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtCODE.InputRegulation = "F";
            this.txtCODE.MaxByteLengthMode = true;
            this.txtCODE.Name = "txtCODE";
            this.txtCODE.OneLineMaxLength = 3;
            this.txtCODE.ProhibitionChar = ((System.Collections.Generic.Dictionary<int, string>)(resources.GetObject("txtCODE.ProhibitionChar")));
            // 
            // lblSeiban
            // 
            // 
            // lblSeiban.ChildPanel
            // 
            this.lblSeiban.ChildPanel.Controls.Add(this.txtSeiban);
            this.lblSeiban.IsFocusChangeColor = false;
            this.lblSeiban.LabelWidth = 80;
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
            this.txtSeiban.OneLineMaxLength = 12;
            this.txtSeiban.ProhibitionChar = null;
            // 
            // lblARNo
            // 
            // 
            // lblARNo.ChildPanel
            // 
            this.lblARNo.ChildPanel.Controls.Add(this.txtARNo);
            this.lblARNo.ChildPanel.Controls.Add(this.lblAR);
            this.lblARNo.IsFocusChangeColor = false;
            this.lblARNo.LabelWidth = 80;
            resources.ApplyResources(this.lblARNo, "lblARNo");
            this.lblARNo.Name = "lblARNo";
            this.lblARNo.SplitterWidth = 0;
            // 
            // txtARNo
            // 
            resources.ApplyResources(this.txtARNo, "txtARNo");
            this.txtARNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtARNo.InputRegulation = "n";
            this.txtARNo.IsInputRegulation = true;
            this.txtARNo.MaxByteLengthMode = true;
            this.txtARNo.Name = "txtARNo";
            this.txtARNo.OneLineMaxLength = 4;
            this.txtARNo.ProhibitionChar = null;
            // 
            // lblAR
            // 
            this.lblAR.BackColor = System.Drawing.Color.LightSkyBlue;
            resources.ApplyResources(this.lblAR, "lblAR");
            this.lblAR.IsFocusChangeColor = false;
            this.lblAR.Name = "lblAR";
            this.lblAR.NormalBackColor = System.Drawing.Color.LightSkyBlue;
            // 
            // lblECSNo
            // 
            // 
            // lblECSNo.ChildPanel
            // 
            this.lblECSNo.ChildPanel.Controls.Add(this.txtECSNo);
            this.lblECSNo.IsFocusChangeColor = false;
            this.lblECSNo.LabelWidth = 80;
            resources.ApplyResources(this.lblECSNo, "lblECSNo");
            this.lblECSNo.Name = "lblECSNo";
            this.lblECSNo.SplitterWidth = 0;
            // 
            // txtECSNo
            // 
            resources.ApplyResources(this.txtECSNo, "txtECSNo");
            this.txtECSNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtECSNo.InputRegulation = "F";
            this.txtECSNo.MaxByteLengthMode = true;
            this.txtECSNo.Name = "txtECSNo";
            this.txtECSNo.OneLineMaxLength = 40;
            this.txtECSNo.ProhibitionChar = null;
            // 
            // lblSettiNoukiDate
            // 
            // 
            // lblSettiNoukiDate.ChildPanel
            // 
            this.lblSettiNoukiDate.ChildPanel.Controls.Add(this.cboSetteiNoukiDate);
            this.lblSettiNoukiDate.ChildPanel.Controls.Add(this.dtpSetteiNoukiDate);
            this.lblSettiNoukiDate.IsFocusChangeColor = false;
            this.lblSettiNoukiDate.LabelWidth = 80;
            resources.ApplyResources(this.lblSettiNoukiDate, "lblSettiNoukiDate");
            this.lblSettiNoukiDate.Name = "lblSettiNoukiDate";
            this.lblSettiNoukiDate.SplitterWidth = 0;
            // 
            // cboSetteiNoukiDate
            // 
            resources.ApplyResources(this.cboSetteiNoukiDate, "cboSetteiNoukiDate");
            this.cboSetteiNoukiDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSetteiNoukiDate.Name = "cboSetteiNoukiDate";
            this.cboSetteiNoukiDate.SelectionChangeCommitted += new System.EventHandler(this.cboSetteiNoukiDate_SelectionChangeCommitted);
            // 
            // dtpSetteiNoukiDate
            // 
            resources.ApplyResources(this.dtpSetteiNoukiDate, "dtpSetteiNoukiDate");
            this.dtpSetteiNoukiDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSetteiNoukiDate.Name = "dtpSetteiNoukiDate";
            // 
            // btnStart
            // 
            resources.ApplyResources(this.btnStart, "btnStart");
            this.btnStart.Name = "btnStart";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // shtMeisai
            // 
            resources.ApplyResources(this.shtMeisai, "shtMeisai");
            this.shtMeisai.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtMeisai.Data")));
            this.shtMeisai.Name = "shtMeisai";
            // 
            // btnAllDeselect
            // 
            resources.ApplyResources(this.btnAllDeselect, "btnAllDeselect");
            this.btnAllDeselect.Name = "btnAllDeselect";
            this.btnAllDeselect.TabStop = false;
            this.btnAllDeselect.Click += new System.EventHandler(this.btnAllDeselect_Click);
            // 
            // btnAllSelect
            // 
            resources.ApplyResources(this.btnAllSelect, "btnAllSelect");
            this.btnAllSelect.Name = "btnAllSelect";
            this.btnAllSelect.TabStop = false;
            this.btnAllSelect.Click += new System.EventHandler(this.btnAllSelect_Click);
            // 
            // btnRangeDeselect
            // 
            resources.ApplyResources(this.btnRangeDeselect, "btnRangeDeselect");
            this.btnRangeDeselect.Name = "btnRangeDeselect";
            this.btnRangeDeselect.Click += new System.EventHandler(this.btnRangeDeselect_Click);
            // 
            // btnRangeSelect
            // 
            resources.ApplyResources(this.btnRangeSelect, "btnRangeSelect");
            this.btnRangeSelect.Name = "btnRangeSelect";
            this.btnRangeSelect.Click += new System.EventHandler(this.btnRangeSelect_Click);
            // 
            // TehaiKumitate
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.IsRunEditAfterClear = false;
            this.Name = "TehaiKumitate";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.grpSearch.ResumeLayout(false);
            this.lblBukkenName.ChildPanel.ResumeLayout(false);
            this.lblBukkenName.ResumeLayout(false);
            this.lblCODE.ChildPanel.ResumeLayout(false);
            this.lblCODE.ChildPanel.PerformLayout();
            this.lblCODE.ResumeLayout(false);
            this.lblSeiban.ChildPanel.ResumeLayout(false);
            this.lblSeiban.ChildPanel.PerformLayout();
            this.lblSeiban.ResumeLayout(false);
            this.lblARNo.ChildPanel.ResumeLayout(false);
            this.lblARNo.ChildPanel.PerformLayout();
            this.lblARNo.ResumeLayout(false);
            this.lblECSNo.ChildPanel.ResumeLayout(false);
            this.lblECSNo.ChildPanel.PerformLayout();
            this.lblECSNo.ResumeLayout(false);
            this.lblSettiNoukiDate.ChildPanel.ResumeLayout(false);
            this.lblSettiNoukiDate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSearch;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnStart;
        private DSWControl.DSWLabel.DSWLabel lblSettiNoukiDate;
        private System.Windows.Forms.DateTimePicker dtpSetteiNoukiDate;
        private DSWControl.DSWComboBox.DSWComboBox cboSetteiNoukiDate;
        private DSWControl.DSWLabel.DSWLabel lblECSNo;
        private DSWControl.DSWTextBox.DSWTextBox txtECSNo;
        private DSWControl.DSWLabel.DSWLabel lblARNo;
        private DSWControl.DSWTextBox.DSWTextBox txtARNo;
        private DSWControl.DSWControlLinkLabel.DSWControlLinkLabel lblAR;
        private GrapeCity.Win.ElTabelle.Sheet shtMeisai;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnAllDeselect;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnAllSelect;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnRangeDeselect;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnRangeSelect;
        private DSWControl.DSWLabel.DSWLabel lblSeiban;
        private DSWControl.DSWTextBox.DSWTextBox txtSeiban;
        private DSWControl.DSWLabel.DSWLabel lblCODE;
        private DSWControl.DSWTextBox.DSWTextBox txtCODE;
        private DSWControl.DSWLabel.CustomPanel ChildPanel;
        private DSWControl.DSWLabel.DSWLabel lblBukkenName;
        private DSWControl.DSWComboBox.DSWComboBox cboBukkenName;
    }
}