namespace SMS.M01.Forms
{
    partial class MeishoHoshu 
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MeishoHoshu));
            this.lblItemName = new DSWControl.DSWLabel.DSWLabel();
            this.txtItemName = new DSWControl.DSWTextBox.DSWTextBox();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.lblSearchType = new DSWControl.DSWLabel.DSWLabel();
            this.cboSearchType = new DSWControl.DSWComboBox.DSWComboBox();
            this.btnInsert = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnDelete = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnUpdate = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnSearch = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.shtResult = new GrapeCity.Win.ElTabelle.Sheet();
            this.lblType = new DSWControl.DSWLabel.DSWLabel();
            this.cboType = new DSWControl.DSWComboBox.DSWComboBox();
            this.grpEdit = new System.Windows.Forms.GroupBox();
            this.lblRegItemName = new DSWControl.DSWLabel.DSWLabel();
            this.txtRegItemName = new DSWControl.DSWTextBox.DSWTextBox();
            this.pnlMain.SuspendLayout();
            this.lblItemName.ChildPanel.SuspendLayout();
            this.lblItemName.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.lblSearchType.ChildPanel.SuspendLayout();
            this.lblSearchType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).BeginInit();
            this.lblType.ChildPanel.SuspendLayout();
            this.lblType.SuspendLayout();
            this.grpEdit.SuspendLayout();
            this.lblRegItemName.ChildPanel.SuspendLayout();
            this.lblRegItemName.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.grpEdit);
            this.pnlMain.Controls.Add(this.grpSearch);
            resources.ApplyResources(this.pnlMain, "pnlMain");
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
            // txtUserName
            // 
            resources.ApplyResources(this.txtUserName, "txtUserName");
            // 
            // txtRoleName
            // 
            resources.ApplyResources(this.txtRoleName, "txtRoleName");
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // lblItemName
            // 
            // 
            // lblItemName.ChildPanel
            // 
            this.lblItemName.ChildPanel.Controls.Add(this.txtItemName);
            this.lblItemName.IsFocusChangeColor = false;
            this.lblItemName.LabelWidth = 80;
            resources.ApplyResources(this.lblItemName, "lblItemName");
            this.lblItemName.Name = "lblItemName";
            this.lblItemName.SplitterWidth = 0;
            // 
            // txtItemName
            // 
            resources.ApplyResources(this.txtItemName, "txtItemName");
            this.txtItemName.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtItemName.InputRegulation = "F";
            this.txtItemName.MaxByteLengthMode = true;
            this.txtItemName.Name = "txtItemName";
            this.txtItemName.OneLineMaxLength = 42;
            this.txtItemName.ProhibitionChar = null;
            // 
            // grpSearch
            // 
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Controls.Add(this.lblSearchType);
            this.grpSearch.Controls.Add(this.btnInsert);
            this.grpSearch.Controls.Add(this.btnDelete);
            this.grpSearch.Controls.Add(this.lblItemName);
            this.grpSearch.Controls.Add(this.btnUpdate);
            this.grpSearch.Controls.Add(this.btnSearch);
            this.grpSearch.Controls.Add(this.shtResult);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
            // 
            // lblSearchType
            // 
            // 
            // lblSearchType.ChildPanel
            // 
            this.lblSearchType.ChildPanel.Controls.Add(this.cboSearchType);
            this.lblSearchType.IsFocusChangeColor = false;
            this.lblSearchType.LabelWidth = 80;
            resources.ApplyResources(this.lblSearchType, "lblSearchType");
            this.lblSearchType.Name = "lblSearchType";
            this.lblSearchType.SplitterWidth = 0;
            // 
            // cboSearchType
            // 
            resources.ApplyResources(this.cboSearchType, "cboSearchType");
            this.cboSearchType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSearchType.Name = "cboSearchType";
            this.cboSearchType.SelectedValueChanged += new System.EventHandler(this.cboSearchType_SelectedValueChanged);
            // 
            // btnInsert
            // 
            resources.ApplyResources(this.btnInsert, "btnInsert");
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
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
            // lblType
            // 
            // 
            // lblType.ChildPanel
            // 
            this.lblType.ChildPanel.Controls.Add(this.cboType);
            this.lblType.IsFocusChangeColor = false;
            this.lblType.IsNecessary = true;
            this.lblType.LabelWidth = 80;
            resources.ApplyResources(this.lblType, "lblType");
            this.lblType.Name = "lblType";
            this.lblType.SplitterWidth = 0;
            // 
            // cboType
            // 
            resources.ApplyResources(this.cboType, "cboType");
            this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboType.Name = "cboType";
            this.cboType.SelectedValueChanged += new System.EventHandler(this.cboType_SelectedValueChanged);
            // 
            // grpEdit
            // 
            resources.ApplyResources(this.grpEdit, "grpEdit");
            this.grpEdit.Controls.Add(this.lblType);
            this.grpEdit.Controls.Add(this.lblRegItemName);
            this.grpEdit.Name = "grpEdit";
            this.grpEdit.TabStop = false;
            // 
            // lblRegItemName
            // 
            // 
            // lblRegItemName.ChildPanel
            // 
            this.lblRegItemName.ChildPanel.Controls.Add(this.txtRegItemName);
            this.lblRegItemName.IsFocusChangeColor = false;
            this.lblRegItemName.IsNecessary = true;
            this.lblRegItemName.LabelWidth = 80;
            resources.ApplyResources(this.lblRegItemName, "lblRegItemName");
            this.lblRegItemName.Name = "lblRegItemName";
            this.lblRegItemName.SplitterWidth = 0;
            // 
            // txtRegItemName
            // 
            resources.ApplyResources(this.txtRegItemName, "txtRegItemName");
            this.txtRegItemName.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtRegItemName.InputRegulation = "F";
            this.txtRegItemName.MaxByteLengthMode = true;
            this.txtRegItemName.Name = "txtRegItemName";
            this.txtRegItemName.OneLineMaxLength = 42;
            this.txtRegItemName.ProhibitionChar = null;
            // 
            // MeishoHoshu
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "MeishoHoshu";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.lblItemName.ChildPanel.ResumeLayout(false);
            this.lblItemName.ChildPanel.PerformLayout();
            this.lblItemName.ResumeLayout(false);
            this.grpSearch.ResumeLayout(false);
            this.lblSearchType.ChildPanel.ResumeLayout(false);
            this.lblSearchType.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).EndInit();
            this.lblType.ChildPanel.ResumeLayout(false);
            this.lblType.ResumeLayout(false);
            this.grpEdit.ResumeLayout(false);
            this.lblRegItemName.ChildPanel.ResumeLayout(false);
            this.lblRegItemName.ChildPanel.PerformLayout();
            this.lblRegItemName.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSearch;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnUpdate;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnSearch;
        private GrapeCity.Win.ElTabelle.Sheet shtResult;
        private System.Windows.Forms.GroupBox grpEdit;
        private DSWControl.DSWTextBox.DSWTextBox txtItemName;
        private DSWControl.DSWLabel.DSWLabel lblItemName;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnInsert;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnDelete;
        private DSWControl.DSWLabel.DSWLabel lblRegItemName;
        private DSWControl.DSWTextBox.DSWTextBox txtRegItemName;
        private DSWControl.DSWLabel.DSWLabel lblSearchType;
        private DSWControl.DSWComboBox.DSWComboBox cboSearchType;
        private DSWControl.DSWLabel.DSWLabel lblType;
        private DSWControl.DSWComboBox.DSWComboBox cboType;
    }
}