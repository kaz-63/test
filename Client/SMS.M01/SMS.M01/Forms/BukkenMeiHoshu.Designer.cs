namespace SMS.M01.Forms
{
    partial class BukkenMeiHoshu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BukkenMeiHoshu));
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.btnShinchokuKanriNotify = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnARListNotify = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnCommonNotify = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnDelete = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnUpdate = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnInsert = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblSearchBukkenName = new DSWControl.DSWLabel.DSWLabel();
            this.txtSearchBukkenName = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblSearchShukkaFlag = new DSWControl.DSWLabel.DSWLabel();
            this.cboSearchShukkaFlag = new DSWControl.DSWComboBox.DSWComboBox();
            this.btnSearch = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.shtResult = new GrapeCity.Win.ElTabelle.Sheet();
            this.grpEdit = new System.Windows.Forms.GroupBox();
            this.lblMailNotify = new DSWControl.DSWLabel.DSWLabel();
            this.cboMailNotify = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblBukkenNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtBukkenNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblIssuedTagNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtIssuedTagNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblBukkenName = new DSWControl.DSWLabel.DSWLabel();
            this.txtBukkenName = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblShukkaFlag = new DSWControl.DSWLabel.DSWLabel();
            this.cboShukkaFlag = new DSWControl.DSWComboBox.DSWComboBox();
            this.pnlMain.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.lblSearchBukkenName.ChildPanel.SuspendLayout();
            this.lblSearchBukkenName.SuspendLayout();
            this.lblSearchShukkaFlag.ChildPanel.SuspendLayout();
            this.lblSearchShukkaFlag.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).BeginInit();
            this.grpEdit.SuspendLayout();
            this.lblMailNotify.ChildPanel.SuspendLayout();
            this.lblMailNotify.SuspendLayout();
            this.lblBukkenNo.ChildPanel.SuspendLayout();
            this.lblBukkenNo.SuspendLayout();
            this.lblIssuedTagNo.ChildPanel.SuspendLayout();
            this.lblIssuedTagNo.SuspendLayout();
            this.lblBukkenName.ChildPanel.SuspendLayout();
            this.lblBukkenName.SuspendLayout();
            this.lblShukkaFlag.ChildPanel.SuspendLayout();
            this.lblShukkaFlag.SuspendLayout();
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
            // grpSearch
            // 
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Controls.Add(this.btnShinchokuKanriNotify);
            this.grpSearch.Controls.Add(this.btnARListNotify);
            this.grpSearch.Controls.Add(this.btnCommonNotify);
            this.grpSearch.Controls.Add(this.btnDelete);
            this.grpSearch.Controls.Add(this.btnUpdate);
            this.grpSearch.Controls.Add(this.btnInsert);
            this.grpSearch.Controls.Add(this.lblSearchBukkenName);
            this.grpSearch.Controls.Add(this.lblSearchShukkaFlag);
            this.grpSearch.Controls.Add(this.btnSearch);
            this.grpSearch.Controls.Add(this.shtResult);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
            // 
            // btnShinchokuKanriNotify
            // 
            resources.ApplyResources(this.btnShinchokuKanriNotify, "btnShinchokuKanriNotify");
            this.btnShinchokuKanriNotify.Name = "btnShinchokuKanriNotify";
            this.btnShinchokuKanriNotify.Click += new System.EventHandler(this.btnShinchokuKanriNotify_Click);
            // 
            // btnARListNotify
            // 
            resources.ApplyResources(this.btnARListNotify, "btnARListNotify");
            this.btnARListNotify.Name = "btnARListNotify";
            this.btnARListNotify.Click += new System.EventHandler(this.btnARListNotify_Click);
            // 
            // btnCommonNotify
            // 
            resources.ApplyResources(this.btnCommonNotify, "btnCommonNotify");
            this.btnCommonNotify.Name = "btnCommonNotify";
            this.btnCommonNotify.Click += new System.EventHandler(this.btnCommonNotify_Click);
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
            // lblSearchBukkenName
            // 
            // 
            // lblSearchBukkenName.ChildPanel
            // 
            this.lblSearchBukkenName.ChildPanel.Controls.Add(this.txtSearchBukkenName);
            this.lblSearchBukkenName.IsFocusChangeColor = false;
            this.lblSearchBukkenName.LabelWidth = 90;
            resources.ApplyResources(this.lblSearchBukkenName, "lblSearchBukkenName");
            this.lblSearchBukkenName.Name = "lblSearchBukkenName";
            this.lblSearchBukkenName.SplitterWidth = 0;
            // 
            // txtSearchBukkenName
            // 
            resources.ApplyResources(this.txtSearchBukkenName, "txtSearchBukkenName");
            this.txtSearchBukkenName.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtSearchBukkenName.InputRegulation = "F";
            this.txtSearchBukkenName.MaxByteLengthMode = true;
            this.txtSearchBukkenName.Name = "txtSearchBukkenName";
            this.txtSearchBukkenName.OneLineMaxLength = 60;
            this.txtSearchBukkenName.ProhibitionChar = null;
            // 
            // lblSearchShukkaFlag
            // 
            // 
            // lblSearchShukkaFlag.ChildPanel
            // 
            this.lblSearchShukkaFlag.ChildPanel.Controls.Add(this.cboSearchShukkaFlag);
            this.lblSearchShukkaFlag.IsFocusChangeColor = false;
            this.lblSearchShukkaFlag.IsNecessary = true;
            this.lblSearchShukkaFlag.LabelWidth = 80;
            resources.ApplyResources(this.lblSearchShukkaFlag, "lblSearchShukkaFlag");
            this.lblSearchShukkaFlag.Name = "lblSearchShukkaFlag";
            this.lblSearchShukkaFlag.SplitterWidth = 0;
            // 
            // cboSearchShukkaFlag
            // 
            resources.ApplyResources(this.cboSearchShukkaFlag, "cboSearchShukkaFlag");
            this.cboSearchShukkaFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSearchShukkaFlag.Name = "cboSearchShukkaFlag";
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
            // grpEdit
            // 
            resources.ApplyResources(this.grpEdit, "grpEdit");
            this.grpEdit.Controls.Add(this.lblMailNotify);
            this.grpEdit.Controls.Add(this.lblBukkenNo);
            this.grpEdit.Controls.Add(this.lblIssuedTagNo);
            this.grpEdit.Controls.Add(this.lblBukkenName);
            this.grpEdit.Controls.Add(this.lblShukkaFlag);
            this.grpEdit.Name = "grpEdit";
            this.grpEdit.TabStop = false;
            // 
            // lblMailNotify
            // 
            // 
            // lblMailNotify.ChildPanel
            // 
            this.lblMailNotify.ChildPanel.Controls.Add(this.cboMailNotify);
            this.lblMailNotify.IsFocusChangeColor = false;
            this.lblMailNotify.IsNecessary = true;
            this.lblMailNotify.LabelWidth = 95;
            resources.ApplyResources(this.lblMailNotify, "lblMailNotify");
            this.lblMailNotify.Name = "lblMailNotify";
            this.lblMailNotify.SplitterWidth = 0;
            // 
            // cboMailNotify
            // 
            resources.ApplyResources(this.cboMailNotify, "cboMailNotify");
            this.cboMailNotify.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMailNotify.Name = "cboMailNotify";
            // 
            // lblBukkenNo
            // 
            // 
            // lblBukkenNo.ChildPanel
            // 
            this.lblBukkenNo.ChildPanel.Controls.Add(this.txtBukkenNo);
            this.lblBukkenNo.IsFocusChangeColor = false;
            this.lblBukkenNo.IsNecessary = true;
            this.lblBukkenNo.LabelWidth = 90;
            resources.ApplyResources(this.lblBukkenNo, "lblBukkenNo");
            this.lblBukkenNo.Name = "lblBukkenNo";
            this.lblBukkenNo.SplitterWidth = 0;
            // 
            // txtBukkenNo
            // 
            this.txtBukkenNo.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtBukkenNo, "txtBukkenNo");
            this.txtBukkenNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtBukkenNo.InputRegulation = "";
            this.txtBukkenNo.Name = "txtBukkenNo";
            this.txtBukkenNo.ProhibitionChar = null;
            this.txtBukkenNo.ReadOnly = true;
            this.txtBukkenNo.TabStop = false;
            // 
            // lblIssuedTagNo
            // 
            // 
            // lblIssuedTagNo.ChildPanel
            // 
            this.lblIssuedTagNo.ChildPanel.Controls.Add(this.txtIssuedTagNo);
            this.lblIssuedTagNo.IsFocusChangeColor = false;
            this.lblIssuedTagNo.IsNecessary = true;
            this.lblIssuedTagNo.LabelWidth = 90;
            resources.ApplyResources(this.lblIssuedTagNo, "lblIssuedTagNo");
            this.lblIssuedTagNo.Name = "lblIssuedTagNo";
            this.lblIssuedTagNo.SplitterWidth = 0;
            // 
            // txtIssuedTagNo
            // 
            this.txtIssuedTagNo.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtIssuedTagNo, "txtIssuedTagNo");
            this.txtIssuedTagNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtIssuedTagNo.InputRegulation = "";
            this.txtIssuedTagNo.Name = "txtIssuedTagNo";
            this.txtIssuedTagNo.ProhibitionChar = null;
            this.txtIssuedTagNo.ReadOnly = true;
            this.txtIssuedTagNo.TabStop = false;
            // 
            // lblBukkenName
            // 
            // 
            // lblBukkenName.ChildPanel
            // 
            this.lblBukkenName.ChildPanel.Controls.Add(this.txtBukkenName);
            this.lblBukkenName.IsFocusChangeColor = false;
            this.lblBukkenName.IsNecessary = true;
            this.lblBukkenName.LabelWidth = 95;
            resources.ApplyResources(this.lblBukkenName, "lblBukkenName");
            this.lblBukkenName.Name = "lblBukkenName";
            this.lblBukkenName.SplitterWidth = 0;
            // 
            // txtBukkenName
            // 
            resources.ApplyResources(this.txtBukkenName, "txtBukkenName");
            this.txtBukkenName.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtBukkenName.InputRegulation = "F";
            this.txtBukkenName.MaxByteLengthMode = true;
            this.txtBukkenName.Name = "txtBukkenName";
            this.txtBukkenName.OneLineMaxLength = 60;
            this.txtBukkenName.ProhibitionChar = null;
            // 
            // lblShukkaFlag
            // 
            // 
            // lblShukkaFlag.ChildPanel
            // 
            this.lblShukkaFlag.ChildPanel.Controls.Add(this.cboShukkaFlag);
            this.lblShukkaFlag.IsFocusChangeColor = false;
            this.lblShukkaFlag.IsNecessary = true;
            this.lblShukkaFlag.LabelWidth = 80;
            resources.ApplyResources(this.lblShukkaFlag, "lblShukkaFlag");
            this.lblShukkaFlag.Name = "lblShukkaFlag";
            this.lblShukkaFlag.SplitterWidth = 0;
            // 
            // cboShukkaFlag
            // 
            resources.ApplyResources(this.cboShukkaFlag, "cboShukkaFlag");
            this.cboShukkaFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboShukkaFlag.Name = "cboShukkaFlag";
            this.cboShukkaFlag.SelectionChangeCommitted += new System.EventHandler(this.cboShukkaFlag_SelectionChangeCommitted);
            // 
            // BukkenMeiHoshu
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "BukkenMeiHoshu";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.grpSearch.ResumeLayout(false);
            this.lblSearchBukkenName.ChildPanel.ResumeLayout(false);
            this.lblSearchBukkenName.ChildPanel.PerformLayout();
            this.lblSearchBukkenName.ResumeLayout(false);
            this.lblSearchShukkaFlag.ChildPanel.ResumeLayout(false);
            this.lblSearchShukkaFlag.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).EndInit();
            this.grpEdit.ResumeLayout(false);
            this.lblMailNotify.ChildPanel.ResumeLayout(false);
            this.lblMailNotify.ResumeLayout(false);
            this.lblBukkenNo.ChildPanel.ResumeLayout(false);
            this.lblBukkenNo.ChildPanel.PerformLayout();
            this.lblBukkenNo.ResumeLayout(false);
            this.lblIssuedTagNo.ChildPanel.ResumeLayout(false);
            this.lblIssuedTagNo.ChildPanel.PerformLayout();
            this.lblIssuedTagNo.ResumeLayout(false);
            this.lblBukkenName.ChildPanel.ResumeLayout(false);
            this.lblBukkenName.ChildPanel.PerformLayout();
            this.lblBukkenName.ResumeLayout(false);
            this.lblShukkaFlag.ChildPanel.ResumeLayout(false);
            this.lblShukkaFlag.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSearch;
        private System.Windows.Forms.GroupBox grpEdit;
        private DSWControl.DSWLabel.DSWLabel lblSearchShukkaFlag;
        private DSWControl.DSWLabel.DSWLabel lblSearchBukkenName;
        private DSWControl.DSWLabel.DSWLabel lblShukkaFlag;
        private DSWControl.DSWLabel.DSWLabel lblBukkenName;
        private DSWControl.DSWLabel.DSWLabel lblIssuedTagNo;
        private DSWControl.DSWLabel.DSWLabel lblBukkenNo;
        private DSWControl.DSWComboBox.DSWComboBox cboSearchShukkaFlag;
        private DSWControl.DSWTextBox.DSWTextBox txtSearchBukkenName;
        private GrapeCity.Win.ElTabelle.Sheet shtResult;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnSearch;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnDelete;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnUpdate;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnInsert;
        private DSWControl.DSWComboBox.DSWComboBox cboShukkaFlag;
        private DSWControl.DSWTextBox.DSWTextBox txtBukkenName;
        private DSWControl.DSWTextBox.DSWTextBox txtIssuedTagNo;
        private DSWControl.DSWTextBox.DSWTextBox txtBukkenNo;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnCommonNotify;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnARListNotify;
        private DSWControl.DSWLabel.DSWLabel lblMailNotify;
        private DSWControl.DSWComboBox.DSWComboBox cboMailNotify;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnShinchokuKanriNotify;
    }
}