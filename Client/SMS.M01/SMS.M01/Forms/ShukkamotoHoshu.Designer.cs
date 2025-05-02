namespace SMS.M01.Forms
{
    partial class ShukkamotoHoshu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShukkamotoHoshu));
            this.lblUnusedFlag = new DSWControl.DSWLabel.DSWLabel();
            this.cboUnusedFlag = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblDispNo = new DSWControl.DSWLabel.DSWLabel();
            this.numDispNo = new DSWControl.DSWNumericTextBox();
            this.lblShipFrom = new DSWControl.DSWLabel.DSWLabel();
            this.txtShipFrom = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblShanaigaiFlag = new DSWControl.DSWLabel.DSWLabel();
            this.cboShanaigaiFlag = new DSWControl.DSWComboBox.DSWComboBox();
            this.slblUnusedFlag = new DSWControl.DSWLabel.DSWLabel();
            this.scboUnusedFlag = new DSWControl.DSWComboBox.DSWComboBox();
            this.slbShipFrom = new DSWControl.DSWLabel.DSWLabel();
            this.stxtShipFrom = new DSWControl.DSWTextBox.DSWTextBox();
            this.slblShanaigaiFlag = new DSWControl.DSWLabel.DSWLabel();
            this.scboShanaigaiFlag = new DSWControl.DSWComboBox.DSWComboBox();
            this.grpEdit = new System.Windows.Forms.GroupBox();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.btnDelete = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnUpdate = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnInsert = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnSearch = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.shtResult = new GrapeCity.Win.ElTabelle.Sheet();
            this.pnlMain.SuspendLayout();
            this.lblUnusedFlag.ChildPanel.SuspendLayout();
            this.lblUnusedFlag.SuspendLayout();
            this.lblDispNo.ChildPanel.SuspendLayout();
            this.lblDispNo.SuspendLayout();
            this.lblShipFrom.ChildPanel.SuspendLayout();
            this.lblShipFrom.SuspendLayout();
            this.lblShanaigaiFlag.ChildPanel.SuspendLayout();
            this.lblShanaigaiFlag.SuspendLayout();
            this.slblUnusedFlag.ChildPanel.SuspendLayout();
            this.slblUnusedFlag.SuspendLayout();
            this.slbShipFrom.ChildPanel.SuspendLayout();
            this.slbShipFrom.SuspendLayout();
            this.slblShanaigaiFlag.ChildPanel.SuspendLayout();
            this.slblShanaigaiFlag.SuspendLayout();
            this.grpEdit.SuspendLayout();
            this.grpSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.grpEdit);
            this.pnlMain.Controls.Add(this.grpSearch);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpEdit, 0);
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
            // lblUnusedFlag
            // 
            // 
            // lblUnusedFlag.ChildPanel
            // 
            this.lblUnusedFlag.ChildPanel.Controls.Add(this.cboUnusedFlag);
            this.lblUnusedFlag.IsFocusChangeColor = false;
            this.lblUnusedFlag.IsNecessary = true;
            this.lblUnusedFlag.LabelWidth = 80;
            resources.ApplyResources(this.lblUnusedFlag, "lblUnusedFlag");
            this.lblUnusedFlag.Name = "lblUnusedFlag";
            this.lblUnusedFlag.SplitterWidth = 0;
            // 
            // cboUnusedFlag
            // 
            resources.ApplyResources(this.cboUnusedFlag, "cboUnusedFlag");
            this.cboUnusedFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUnusedFlag.Name = "cboUnusedFlag";
            // 
            // lblDispNo
            // 
            // 
            // lblDispNo.ChildPanel
            // 
            this.lblDispNo.ChildPanel.Controls.Add(this.numDispNo);
            this.lblDispNo.IsFocusChangeColor = false;
            this.lblDispNo.IsNecessary = true;
            this.lblDispNo.LabelWidth = 90;
            resources.ApplyResources(this.lblDispNo, "lblDispNo");
            this.lblDispNo.Name = "lblDispNo";
            this.lblDispNo.SplitterWidth = 0;
            // 
            // numDispNo
            // 
            resources.ApplyResources(this.numDispNo, "numDispNo");
            this.numDispNo.IntLength = 3;
            this.numDispNo.MaxValue = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numDispNo.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numDispNo.Name = "numDispNo";
            // 
            // lblShipFrom
            // 
            // 
            // lblShipFrom.ChildPanel
            // 
            this.lblShipFrom.ChildPanel.Controls.Add(this.txtShipFrom);
            this.lblShipFrom.IsFocusChangeColor = false;
            this.lblShipFrom.IsNecessary = true;
            this.lblShipFrom.LabelWidth = 80;
            resources.ApplyResources(this.lblShipFrom, "lblShipFrom");
            this.lblShipFrom.Name = "lblShipFrom";
            this.lblShipFrom.SplitterWidth = 0;
            // 
            // txtShipFrom
            // 
            resources.ApplyResources(this.txtShipFrom, "txtShipFrom");
            this.txtShipFrom.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtShipFrom.InputRegulation = "F";
            this.txtShipFrom.MaxByteLengthMode = true;
            this.txtShipFrom.Name = "txtShipFrom";
            this.txtShipFrom.OneLineMaxLength = 10;
            this.txtShipFrom.ProhibitionChar = null;
            // 
            // lblShanaigaiFlag
            // 
            // 
            // lblShanaigaiFlag.ChildPanel
            // 
            this.lblShanaigaiFlag.ChildPanel.Controls.Add(this.cboShanaigaiFlag);
            this.lblShanaigaiFlag.IsFocusChangeColor = false;
            this.lblShanaigaiFlag.IsNecessary = true;
            this.lblShanaigaiFlag.LabelWidth = 80;
            resources.ApplyResources(this.lblShanaigaiFlag, "lblShanaigaiFlag");
            this.lblShanaigaiFlag.Name = "lblShanaigaiFlag";
            this.lblShanaigaiFlag.SplitterWidth = 0;
            // 
            // cboShanaigaiFlag
            // 
            resources.ApplyResources(this.cboShanaigaiFlag, "cboShanaigaiFlag");
            this.cboShanaigaiFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboShanaigaiFlag.Name = "cboShanaigaiFlag";
            // 
            // slblUnusedFlag
            // 
            // 
            // slblUnusedFlag.ChildPanel
            // 
            this.slblUnusedFlag.ChildPanel.Controls.Add(this.scboUnusedFlag);
            this.slblUnusedFlag.IsFocusChangeColor = false;
            this.slblUnusedFlag.LabelWidth = 80;
            resources.ApplyResources(this.slblUnusedFlag, "slblUnusedFlag");
            this.slblUnusedFlag.Name = "slblUnusedFlag";
            this.slblUnusedFlag.SplitterWidth = 0;
            // 
            // scboUnusedFlag
            // 
            resources.ApplyResources(this.scboUnusedFlag, "scboUnusedFlag");
            this.scboUnusedFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.scboUnusedFlag.Name = "scboUnusedFlag";
            // 
            // slbShipFrom
            // 
            // 
            // slbShipFrom.ChildPanel
            // 
            this.slbShipFrom.ChildPanel.Controls.Add(this.stxtShipFrom);
            this.slbShipFrom.IsFocusChangeColor = false;
            this.slbShipFrom.LabelWidth = 80;
            resources.ApplyResources(this.slbShipFrom, "slbShipFrom");
            this.slbShipFrom.Name = "slbShipFrom";
            this.slbShipFrom.SplitterWidth = 0;
            // 
            // stxtShipFrom
            // 
            resources.ApplyResources(this.stxtShipFrom, "stxtShipFrom");
            this.stxtShipFrom.FocusBackColor = System.Drawing.SystemColors.Window;
            this.stxtShipFrom.InputRegulation = "F";
            this.stxtShipFrom.MaxByteLengthMode = true;
            this.stxtShipFrom.Name = "stxtShipFrom";
            this.stxtShipFrom.OneLineMaxLength = 10;
            this.stxtShipFrom.ProhibitionChar = null;
            // 
            // slblShanaigaiFlag
            // 
            // 
            // slblShanaigaiFlag.ChildPanel
            // 
            this.slblShanaigaiFlag.ChildPanel.Controls.Add(this.scboShanaigaiFlag);
            this.slblShanaigaiFlag.IsFocusChangeColor = false;
            this.slblShanaigaiFlag.LabelWidth = 80;
            resources.ApplyResources(this.slblShanaigaiFlag, "slblShanaigaiFlag");
            this.slblShanaigaiFlag.Name = "slblShanaigaiFlag";
            this.slblShanaigaiFlag.SplitterWidth = 0;
            // 
            // scboShanaigaiFlag
            // 
            resources.ApplyResources(this.scboShanaigaiFlag, "scboShanaigaiFlag");
            this.scboShanaigaiFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.scboShanaigaiFlag.Name = "scboShanaigaiFlag";
            // 
            // grpEdit
            // 
            resources.ApplyResources(this.grpEdit, "grpEdit");
            this.grpEdit.Controls.Add(this.lblUnusedFlag);
            this.grpEdit.Controls.Add(this.lblDispNo);
            this.grpEdit.Controls.Add(this.lblShipFrom);
            this.grpEdit.Controls.Add(this.lblShanaigaiFlag);
            this.grpEdit.Name = "grpEdit";
            this.grpEdit.TabStop = false;
            // 
            // grpSearch
            // 
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Controls.Add(this.slblUnusedFlag);
            this.grpSearch.Controls.Add(this.slbShipFrom);
            this.grpSearch.Controls.Add(this.btnDelete);
            this.grpSearch.Controls.Add(this.btnUpdate);
            this.grpSearch.Controls.Add(this.btnInsert);
            this.grpSearch.Controls.Add(this.slblShanaigaiFlag);
            this.grpSearch.Controls.Add(this.btnSearch);
            this.grpSearch.Controls.Add(this.shtResult);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
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
            // ShukkamotoHoshu
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ShukkamotoHoshu";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.lblUnusedFlag.ChildPanel.ResumeLayout(false);
            this.lblUnusedFlag.ResumeLayout(false);
            this.lblDispNo.ChildPanel.ResumeLayout(false);
            this.lblDispNo.ChildPanel.PerformLayout();
            this.lblDispNo.ResumeLayout(false);
            this.lblShipFrom.ChildPanel.ResumeLayout(false);
            this.lblShipFrom.ChildPanel.PerformLayout();
            this.lblShipFrom.ResumeLayout(false);
            this.lblShanaigaiFlag.ChildPanel.ResumeLayout(false);
            this.lblShanaigaiFlag.ResumeLayout(false);
            this.slblUnusedFlag.ChildPanel.ResumeLayout(false);
            this.slblUnusedFlag.ResumeLayout(false);
            this.slbShipFrom.ChildPanel.ResumeLayout(false);
            this.slbShipFrom.ChildPanel.PerformLayout();
            this.slbShipFrom.ResumeLayout(false);
            this.slblShanaigaiFlag.ChildPanel.ResumeLayout(false);
            this.slblShanaigaiFlag.ResumeLayout(false);
            this.grpEdit.ResumeLayout(false);
            this.grpSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpEdit;
        private DSWControl.DSWLabel.DSWLabel lblDispNo;
        private DSWControl.DSWLabel.DSWLabel lblShipFrom;
        private DSWControl.DSWTextBox.DSWTextBox txtShipFrom;
        private DSWControl.DSWLabel.DSWLabel lblShanaigaiFlag;
        private DSWControl.DSWComboBox.DSWComboBox cboShanaigaiFlag;
        private System.Windows.Forms.GroupBox grpSearch;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnDelete;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnUpdate;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnInsert;
        private DSWControl.DSWLabel.DSWLabel slblShanaigaiFlag;
        private DSWControl.DSWComboBox.DSWComboBox scboShanaigaiFlag;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnSearch;
        private GrapeCity.Win.ElTabelle.Sheet shtResult;
        private DSWControl.DSWLabel.DSWLabel slbShipFrom;
        private DSWControl.DSWTextBox.DSWTextBox stxtShipFrom;
        private DSWControl.DSWLabel.DSWLabel slblUnusedFlag;
        private DSWControl.DSWComboBox.DSWComboBox scboUnusedFlag;
        private DSWControl.DSWLabel.DSWLabel lblUnusedFlag;
        private DSWControl.DSWComboBox.DSWComboBox cboUnusedFlag;
        private DSWControl.DSWNumericTextBox numDispNo;
    }
}