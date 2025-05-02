namespace SMS.M01.Forms
{
    partial class UnsoKaisyaHoshu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UnsoKaisyaHoshu));
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.lblSearchKokunaigai = new DSWControl.DSWLabel.DSWLabel();
            this.cboSearchKokunaigai = new DSWControl.DSWComboBox.DSWComboBox();
            this.btnSearch = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.shtResult = new GrapeCity.Win.ElTabelle.Sheet();
            this.lblSearchUnsokaisyaName = new DSWControl.DSWLabel.DSWLabel();
            this.txtSearchUnsokaisyaName = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnDelete = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnUpdate = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnInsert = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblTuukanFlag = new DSWControl.DSWLabel.DSWLabel();
            this.cboTuukanFlag = new DSWControl.DSWComboBox.DSWComboBox();
            this.grpEdit = new System.Windows.Forms.GroupBox();
            this.lblSortNo = new DSWControl.DSWLabel.DSWLabel();
            this.numSortNo = new DSWControl.DSWNumericTextBox();
            this.lblVersion = new DSWControl.DSWLabel.DSWLabel();
            this.txtVersion = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblUnsokaisyaNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtUnsokaisyaNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblTuukanAttn = new DSWControl.DSWLabel.DSWLabel();
            this.txtTuukanAttn = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblInvoice = new DSWControl.DSWLabel.DSWLabel();
            this.cboInvoice = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblPackingList = new DSWControl.DSWLabel.DSWLabel();
            this.cboPackingList = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblUnsokaisyaName = new DSWControl.DSWLabel.DSWLabel();
            this.txtUnsokaisyaName = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblKokunaigai = new DSWControl.DSWLabel.DSWLabel();
            this.cboKokunaigai = new DSWControl.DSWComboBox.DSWComboBox();
            this.pnlMain.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.lblSearchKokunaigai.ChildPanel.SuspendLayout();
            this.lblSearchKokunaigai.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).BeginInit();
            this.lblSearchUnsokaisyaName.ChildPanel.SuspendLayout();
            this.lblSearchUnsokaisyaName.SuspendLayout();
            this.lblTuukanFlag.ChildPanel.SuspendLayout();
            this.lblTuukanFlag.SuspendLayout();
            this.grpEdit.SuspendLayout();
            this.lblSortNo.ChildPanel.SuspendLayout();
            this.lblSortNo.SuspendLayout();
            this.lblVersion.ChildPanel.SuspendLayout();
            this.lblVersion.SuspendLayout();
            this.lblUnsokaisyaNo.ChildPanel.SuspendLayout();
            this.lblUnsokaisyaNo.SuspendLayout();
            this.lblTuukanAttn.ChildPanel.SuspendLayout();
            this.lblTuukanAttn.SuspendLayout();
            this.lblInvoice.ChildPanel.SuspendLayout();
            this.lblInvoice.SuspendLayout();
            this.lblPackingList.ChildPanel.SuspendLayout();
            this.lblPackingList.SuspendLayout();
            this.lblUnsokaisyaName.ChildPanel.SuspendLayout();
            this.lblUnsokaisyaName.SuspendLayout();
            this.lblKokunaigai.ChildPanel.SuspendLayout();
            this.lblKokunaigai.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.btnDelete);
            this.pnlMain.Controls.Add(this.grpSearch);
            this.pnlMain.Controls.Add(this.btnUpdate);
            this.pnlMain.Controls.Add(this.grpEdit);
            this.pnlMain.Controls.Add(this.btnInsert);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.SetChildIndex(this.btnInsert, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpEdit, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnUpdate, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnDelete, 0);
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
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Controls.Add(this.lblSearchKokunaigai);
            this.grpSearch.Controls.Add(this.btnSearch);
            this.grpSearch.Controls.Add(this.shtResult);
            this.grpSearch.Controls.Add(this.lblSearchUnsokaisyaName);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
            // 
            // lblSearchKokunaigai
            // 
            // 
            // lblSearchKokunaigai.ChildPanel
            // 
            this.lblSearchKokunaigai.ChildPanel.Controls.Add(this.cboSearchKokunaigai);
            this.lblSearchKokunaigai.IsFocusChangeColor = false;
            this.lblSearchKokunaigai.LabelWidth = 80;
            resources.ApplyResources(this.lblSearchKokunaigai, "lblSearchKokunaigai");
            this.lblSearchKokunaigai.Name = "lblSearchKokunaigai";
            this.lblSearchKokunaigai.SplitterWidth = 0;
            // 
            // cboSearchKokunaigai
            // 
            resources.ApplyResources(this.cboSearchKokunaigai, "cboSearchKokunaigai");
            this.cboSearchKokunaigai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSearchKokunaigai.Name = "cboSearchKokunaigai";
            // 
            // btnSearch
            // 
            resources.ApplyResources(this.btnSearch, "btnSearch");
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // shtResult
            // 
            resources.ApplyResources(this.shtResult, "shtResult");
            this.shtResult.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtResult.Data")));
            this.shtResult.Name = "shtResult";
            // 
            // lblSearchUnsokaisyaName
            // 
            // 
            // lblSearchUnsokaisyaName.ChildPanel
            // 
            this.lblSearchUnsokaisyaName.ChildPanel.Controls.Add(this.txtSearchUnsokaisyaName);
            this.lblSearchUnsokaisyaName.IsFocusChangeColor = false;
            this.lblSearchUnsokaisyaName.LabelWidth = 80;
            resources.ApplyResources(this.lblSearchUnsokaisyaName, "lblSearchUnsokaisyaName");
            this.lblSearchUnsokaisyaName.Name = "lblSearchUnsokaisyaName";
            this.lblSearchUnsokaisyaName.SplitterWidth = 0;
            // 
            // txtSearchUnsokaisyaName
            // 
            resources.ApplyResources(this.txtSearchUnsokaisyaName, "txtSearchUnsokaisyaName");
            this.txtSearchUnsokaisyaName.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtSearchUnsokaisyaName.InputRegulation = "F";
            this.txtSearchUnsokaisyaName.MaxByteLengthMode = true;
            this.txtSearchUnsokaisyaName.Name = "txtSearchUnsokaisyaName";
            this.txtSearchUnsokaisyaName.OneLineMaxLength = 20;
            this.txtSearchUnsokaisyaName.ProhibitionChar = null;
            // 
            // btnDelete
            // 
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            resources.ApplyResources(this.btnUpdate, "btnUpdate");
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnInsert
            // 
            resources.ApplyResources(this.btnInsert, "btnInsert");
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // lblTuukanFlag
            // 
            // 
            // lblTuukanFlag.ChildPanel
            // 
            this.lblTuukanFlag.ChildPanel.Controls.Add(this.cboTuukanFlag);
            this.lblTuukanFlag.IsFocusChangeColor = false;
            this.lblTuukanFlag.IsNecessary = true;
            this.lblTuukanFlag.LabelWidth = 120;
            resources.ApplyResources(this.lblTuukanFlag, "lblTuukanFlag");
            this.lblTuukanFlag.Name = "lblTuukanFlag";
            this.lblTuukanFlag.SplitterWidth = 0;
            // 
            // cboTuukanFlag
            // 
            resources.ApplyResources(this.cboTuukanFlag, "cboTuukanFlag");
            this.cboTuukanFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTuukanFlag.Name = "cboTuukanFlag";
            this.cboTuukanFlag.SelectedValueChanged += new System.EventHandler(this.cboTuukanFlag_SelectedValueChanged);
            // 
            // grpEdit
            // 
            resources.ApplyResources(this.grpEdit, "grpEdit");
            this.grpEdit.Controls.Add(this.lblSortNo);
            this.grpEdit.Controls.Add(this.lblVersion);
            this.grpEdit.Controls.Add(this.lblUnsokaisyaNo);
            this.grpEdit.Controls.Add(this.lblTuukanAttn);
            this.grpEdit.Controls.Add(this.lblInvoice);
            this.grpEdit.Controls.Add(this.lblPackingList);
            this.grpEdit.Controls.Add(this.lblUnsokaisyaName);
            this.grpEdit.Controls.Add(this.lblKokunaigai);
            this.grpEdit.Controls.Add(this.lblTuukanFlag);
            this.grpEdit.Name = "grpEdit";
            this.grpEdit.TabStop = false;
            // 
            // lblSortNo
            // 
            // 
            // lblSortNo.ChildPanel
            // 
            this.lblSortNo.ChildPanel.Controls.Add(this.numSortNo);
            this.lblSortNo.IsFocusChangeColor = false;
            this.lblSortNo.IsNecessary = true;
            this.lblSortNo.LabelWidth = 90;
            resources.ApplyResources(this.lblSortNo, "lblSortNo");
            this.lblSortNo.Name = "lblSortNo";
            this.lblSortNo.SplitterWidth = 0;
            // 
            // numSortNo
            // 
            resources.ApplyResources(this.numSortNo, "numSortNo");
            this.numSortNo.IntLength = 7;
            this.numSortNo.MaxValue = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numSortNo.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numSortNo.Name = "numSortNo";
            // 
            // lblVersion
            // 
            // 
            // lblVersion.ChildPanel
            // 
            this.lblVersion.ChildPanel.Controls.Add(this.txtVersion);
            this.lblVersion.IsFocusChangeColor = false;
            this.lblVersion.LabelWidth = 80;
            resources.ApplyResources(this.lblVersion, "lblVersion");
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.SplitterWidth = 0;
            // 
            // txtVersion
            // 
            resources.ApplyResources(this.txtVersion, "txtVersion");
            this.txtVersion.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtVersion.InputRegulation = "anbrl";
            this.txtVersion.IsInputRegulation = true;
            this.txtVersion.MaxByteLengthMode = true;
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.OneLineMaxLength = 100;
            this.txtVersion.ProhibitionChar = null;
            // 
            // lblUnsokaisyaNo
            // 
            // 
            // lblUnsokaisyaNo.ChildPanel
            // 
            this.lblUnsokaisyaNo.ChildPanel.Controls.Add(this.txtUnsokaisyaNo);
            this.lblUnsokaisyaNo.IsFocusChangeColor = false;
            this.lblUnsokaisyaNo.LabelWidth = 80;
            resources.ApplyResources(this.lblUnsokaisyaNo, "lblUnsokaisyaNo");
            this.lblUnsokaisyaNo.Name = "lblUnsokaisyaNo";
            this.lblUnsokaisyaNo.SplitterWidth = 0;
            // 
            // txtUnsokaisyaNo
            // 
            resources.ApplyResources(this.txtUnsokaisyaNo, "txtUnsokaisyaNo");
            this.txtUnsokaisyaNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtUnsokaisyaNo.InputRegulation = "anbrl";
            this.txtUnsokaisyaNo.IsInputRegulation = true;
            this.txtUnsokaisyaNo.Name = "txtUnsokaisyaNo";
            this.txtUnsokaisyaNo.OneLineMaxLength = 10;
            this.txtUnsokaisyaNo.ProhibitionChar = null;
            // 
            // lblTuukanAttn
            // 
            // 
            // lblTuukanAttn.ChildPanel
            // 
            this.lblTuukanAttn.ChildPanel.Controls.Add(this.txtTuukanAttn);
            this.lblTuukanAttn.IsFocusChangeColor = false;
            this.lblTuukanAttn.IsNecessary = true;
            this.lblTuukanAttn.LabelWidth = 150;
            resources.ApplyResources(this.lblTuukanAttn, "lblTuukanAttn");
            this.lblTuukanAttn.Name = "lblTuukanAttn";
            this.lblTuukanAttn.SplitterWidth = 0;
            // 
            // txtTuukanAttn
            // 
            resources.ApplyResources(this.txtTuukanAttn, "txtTuukanAttn");
            this.txtTuukanAttn.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtTuukanAttn.InputRegulation = "F";
            this.txtTuukanAttn.MaxByteLengthMode = true;
            this.txtTuukanAttn.Name = "txtTuukanAttn";
            this.txtTuukanAttn.OneLineMaxLength = 40;
            this.txtTuukanAttn.ProhibitionChar = null;
            // 
            // lblInvoice
            // 
            // 
            // lblInvoice.ChildPanel
            // 
            this.lblInvoice.ChildPanel.Controls.Add(this.cboInvoice);
            this.lblInvoice.IsFocusChangeColor = false;
            this.lblInvoice.IsNecessary = true;
            this.lblInvoice.LabelWidth = 80;
            resources.ApplyResources(this.lblInvoice, "lblInvoice");
            this.lblInvoice.Name = "lblInvoice";
            this.lblInvoice.SplitterWidth = 0;
            // 
            // cboInvoice
            // 
            resources.ApplyResources(this.cboInvoice, "cboInvoice");
            this.cboInvoice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboInvoice.Name = "cboInvoice";
            // 
            // lblPackingList
            // 
            // 
            // lblPackingList.ChildPanel
            // 
            this.lblPackingList.ChildPanel.Controls.Add(this.cboPackingList);
            this.lblPackingList.IsFocusChangeColor = false;
            this.lblPackingList.IsNecessary = true;
            this.lblPackingList.LabelWidth = 80;
            resources.ApplyResources(this.lblPackingList, "lblPackingList");
            this.lblPackingList.Name = "lblPackingList";
            this.lblPackingList.SplitterWidth = 0;
            // 
            // cboPackingList
            // 
            resources.ApplyResources(this.cboPackingList, "cboPackingList");
            this.cboPackingList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPackingList.Name = "cboPackingList";
            // 
            // lblUnsokaisyaName
            // 
            // 
            // lblUnsokaisyaName.ChildPanel
            // 
            this.lblUnsokaisyaName.ChildPanel.Controls.Add(this.txtUnsokaisyaName);
            this.lblUnsokaisyaName.IsFocusChangeColor = false;
            this.lblUnsokaisyaName.IsNecessary = true;
            this.lblUnsokaisyaName.LabelWidth = 80;
            resources.ApplyResources(this.lblUnsokaisyaName, "lblUnsokaisyaName");
            this.lblUnsokaisyaName.Name = "lblUnsokaisyaName";
            this.lblUnsokaisyaName.SplitterWidth = 0;
            // 
            // txtUnsokaisyaName
            // 
            resources.ApplyResources(this.txtUnsokaisyaName, "txtUnsokaisyaName");
            this.txtUnsokaisyaName.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtUnsokaisyaName.InputRegulation = "F";
            this.txtUnsokaisyaName.MaxByteLengthMode = true;
            this.txtUnsokaisyaName.Name = "txtUnsokaisyaName";
            this.txtUnsokaisyaName.OneLineMaxLength = 20;
            this.txtUnsokaisyaName.ProhibitionChar = null;
            // 
            // lblKokunaigai
            // 
            // 
            // lblKokunaigai.ChildPanel
            // 
            this.lblKokunaigai.ChildPanel.Controls.Add(this.cboKokunaigai);
            this.lblKokunaigai.IsFocusChangeColor = false;
            this.lblKokunaigai.IsNecessary = true;
            this.lblKokunaigai.LabelWidth = 80;
            resources.ApplyResources(this.lblKokunaigai, "lblKokunaigai");
            this.lblKokunaigai.Name = "lblKokunaigai";
            this.lblKokunaigai.SplitterWidth = 0;
            // 
            // cboKokunaigai
            // 
            resources.ApplyResources(this.cboKokunaigai, "cboKokunaigai");
            this.cboKokunaigai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboKokunaigai.Name = "cboKokunaigai";
            this.cboKokunaigai.SelectedValueChanged += new System.EventHandler(this.cboKokunaigai_SelectedValueChanged);
            // 
            // UnsoKaisyaHoshu
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UnsoKaisyaHoshu";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.grpSearch.ResumeLayout(false);
            this.lblSearchKokunaigai.ChildPanel.ResumeLayout(false);
            this.lblSearchKokunaigai.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).EndInit();
            this.lblSearchUnsokaisyaName.ChildPanel.ResumeLayout(false);
            this.lblSearchUnsokaisyaName.ChildPanel.PerformLayout();
            this.lblSearchUnsokaisyaName.ResumeLayout(false);
            this.lblTuukanFlag.ChildPanel.ResumeLayout(false);
            this.lblTuukanFlag.ResumeLayout(false);
            this.grpEdit.ResumeLayout(false);
            this.lblSortNo.ChildPanel.ResumeLayout(false);
            this.lblSortNo.ChildPanel.PerformLayout();
            this.lblSortNo.ResumeLayout(false);
            this.lblVersion.ChildPanel.ResumeLayout(false);
            this.lblVersion.ChildPanel.PerformLayout();
            this.lblVersion.ResumeLayout(false);
            this.lblUnsokaisyaNo.ChildPanel.ResumeLayout(false);
            this.lblUnsokaisyaNo.ChildPanel.PerformLayout();
            this.lblUnsokaisyaNo.ResumeLayout(false);
            this.lblTuukanAttn.ChildPanel.ResumeLayout(false);
            this.lblTuukanAttn.ChildPanel.PerformLayout();
            this.lblTuukanAttn.ResumeLayout(false);
            this.lblInvoice.ChildPanel.ResumeLayout(false);
            this.lblInvoice.ResumeLayout(false);
            this.lblPackingList.ChildPanel.ResumeLayout(false);
            this.lblPackingList.ResumeLayout(false);
            this.lblUnsokaisyaName.ChildPanel.ResumeLayout(false);
            this.lblUnsokaisyaName.ChildPanel.PerformLayout();
            this.lblUnsokaisyaName.ResumeLayout(false);
            this.lblKokunaigai.ChildPanel.ResumeLayout(false);
            this.lblKokunaigai.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSearch;
        private DSWControl.DSWLabel.DSWLabel lblSearchUnsokaisyaName;
        private DSWControl.DSWLabel.DSWLabel lblTuukanFlag;
        private System.Windows.Forms.GroupBox grpEdit;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnDelete;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnUpdate;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnInsert;
        private DSWControl.DSWTextBox.DSWTextBox txtSearchUnsokaisyaName;
        private GrapeCity.Win.ElTabelle.Sheet shtResult;
        private DSWControl.DSWComboBox.DSWComboBox cboTuukanFlag;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnSearch;
        private DSWControl.DSWLabel.DSWLabel lblUnsokaisyaName;
        private DSWControl.DSWTextBox.DSWTextBox txtUnsokaisyaName;
        private DSWControl.DSWLabel.DSWLabel lblKokunaigai;
        private DSWControl.DSWComboBox.DSWComboBox cboKokunaigai;
        private DSWControl.DSWLabel.DSWLabel lblPackingList;
        private DSWControl.DSWComboBox.DSWComboBox cboPackingList;
        private DSWControl.DSWLabel.DSWLabel lblInvoice;
        private DSWControl.DSWComboBox.DSWComboBox cboInvoice;
        private DSWControl.DSWLabel.DSWLabel lblTuukanAttn;
        private DSWControl.DSWTextBox.DSWTextBox txtTuukanAttn;
        private DSWControl.DSWLabel.DSWLabel lblSearchKokunaigai;
        private DSWControl.DSWComboBox.DSWComboBox cboSearchKokunaigai;
        private DSWControl.DSWLabel.DSWLabel lblVersion;
        private DSWControl.DSWTextBox.DSWTextBox txtVersion;
        private DSWControl.DSWLabel.DSWLabel lblUnsokaisyaNo;
        private DSWControl.DSWTextBox.DSWTextBox txtUnsokaisyaNo;
        private DSWControl.DSWLabel.DSWLabel lblSortNo;
        private DSWControl.DSWNumericTextBox numSortNo;
    }
}