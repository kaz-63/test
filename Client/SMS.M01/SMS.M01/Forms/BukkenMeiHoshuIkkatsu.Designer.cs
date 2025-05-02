namespace SMS.M01.Forms
{
    partial class BukkenMeiHoshuIkkatsu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BukkenMeiHoshuIkkatsu));
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.btnShinchokuKanriNotify = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnARListNotify = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnCommonNotify = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnDelete = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnUpdate = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnInsert = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblSearchBukkenName = new DSWControl.DSWLabel.DSWLabel();
            this.txtSearchBukkenName = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnSearch = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.shtResult = new GrapeCity.Win.ElTabelle.Sheet();
            this.grpEdit = new System.Windows.Forms.GroupBox();
            this.lblJuchunNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtJuchuNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblArTagNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtArTagNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblMailNotify = new DSWControl.DSWLabel.DSWLabel();
            this.cboMailNotify = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblNormalTagNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtNormalTagNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblBukkenName = new DSWControl.DSWLabel.DSWLabel();
            this.txtBukkenName = new DSWControl.DSWTextBox.DSWTextBox();
            this.dswTextBox1 = new DSWControl.DSWTextBox.DSWTextBox();
            this.pnlMain.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.lblSearchBukkenName.ChildPanel.SuspendLayout();
            this.lblSearchBukkenName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).BeginInit();
            this.grpEdit.SuspendLayout();
            this.lblJuchunNo.ChildPanel.SuspendLayout();
            this.lblJuchunNo.SuspendLayout();
            this.lblArTagNo.ChildPanel.SuspendLayout();
            this.lblArTagNo.SuspendLayout();
            this.lblMailNotify.ChildPanel.SuspendLayout();
            this.lblMailNotify.SuspendLayout();
            this.lblNormalTagNo.ChildPanel.SuspendLayout();
            this.lblNormalTagNo.SuspendLayout();
            this.lblBukkenName.ChildPanel.SuspendLayout();
            this.lblBukkenName.SuspendLayout();
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
            this.grpEdit.Controls.Add(this.lblJuchunNo);
            this.grpEdit.Controls.Add(this.lblArTagNo);
            this.grpEdit.Controls.Add(this.lblMailNotify);
            this.grpEdit.Controls.Add(this.lblNormalTagNo);
            this.grpEdit.Controls.Add(this.lblBukkenName);
            this.grpEdit.Name = "grpEdit";
            this.grpEdit.TabStop = false;
            // 
            // lblJuchunNo
            // 
            // 
            // lblJuchunNo.ChildPanel
            // 
            this.lblJuchunNo.ChildPanel.Controls.Add(this.txtJuchuNo);
            this.lblJuchunNo.IsFocusChangeColor = false;
            this.lblJuchunNo.IsNecessary = true;
            this.lblJuchunNo.LabelWidth = 95;
            resources.ApplyResources(this.lblJuchunNo, "lblJuchunNo");
            this.lblJuchunNo.Name = "lblJuchunNo";
            this.lblJuchunNo.SplitterWidth = 0;
            // 
            // txtJuchuNo
            // 
            resources.ApplyResources(this.txtJuchuNo, "txtJuchuNo");
            this.txtJuchuNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtJuchuNo.InputRegulation = "F";
            this.txtJuchuNo.MaxByteLengthMode = true;
            this.txtJuchuNo.Name = "txtJuchuNo";
            this.txtJuchuNo.OneLineMaxLength = 30;
            this.txtJuchuNo.ProhibitionChar = null;
            // 
            // lblArTagNo
            // 
            // 
            // lblArTagNo.ChildPanel
            // 
            this.lblArTagNo.ChildPanel.Controls.Add(this.txtArTagNo);
            this.lblArTagNo.IsFocusChangeColor = false;
            this.lblArTagNo.IsNecessary = true;
            this.lblArTagNo.LabelWidth = 90;
            resources.ApplyResources(this.lblArTagNo, "lblArTagNo");
            this.lblArTagNo.Name = "lblArTagNo";
            this.lblArTagNo.SplitterWidth = 0;
            // 
            // txtArTagNo
            // 
            this.txtArTagNo.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtArTagNo, "txtArTagNo");
            this.txtArTagNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtArTagNo.InputRegulation = "";
            this.txtArTagNo.Name = "txtArTagNo";
            this.txtArTagNo.ProhibitionChar = null;
            this.txtArTagNo.ReadOnly = true;
            this.txtArTagNo.TabStop = false;
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
            // lblNormalTagNo
            // 
            // 
            // lblNormalTagNo.ChildPanel
            // 
            this.lblNormalTagNo.ChildPanel.Controls.Add(this.txtNormalTagNo);
            this.lblNormalTagNo.IsFocusChangeColor = false;
            this.lblNormalTagNo.IsNecessary = true;
            this.lblNormalTagNo.LabelWidth = 90;
            resources.ApplyResources(this.lblNormalTagNo, "lblNormalTagNo");
            this.lblNormalTagNo.Name = "lblNormalTagNo";
            this.lblNormalTagNo.SplitterWidth = 0;
            // 
            // txtNormalTagNo
            // 
            this.txtNormalTagNo.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtNormalTagNo, "txtNormalTagNo");
            this.txtNormalTagNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtNormalTagNo.InputRegulation = "";
            this.txtNormalTagNo.Name = "txtNormalTagNo";
            this.txtNormalTagNo.ProhibitionChar = null;
            this.txtNormalTagNo.ReadOnly = true;
            this.txtNormalTagNo.TabStop = false;
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
            // dswTextBox1
            // 
            resources.ApplyResources(this.dswTextBox1, "dswTextBox1");
            this.dswTextBox1.FocusBackColor = System.Drawing.SystemColors.Window;
            this.dswTextBox1.InputRegulation = "F";
            this.dswTextBox1.MaxByteLengthMode = true;
            this.dswTextBox1.Name = "dswTextBox1";
            this.dswTextBox1.OneLineMaxLength = 40;
            this.dswTextBox1.ProhibitionChar = null;
            // 
            // BukkenMeiHoshuIkkatsu
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "BukkenMeiHoshuIkkatsu";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.grpSearch.ResumeLayout(false);
            this.lblSearchBukkenName.ChildPanel.ResumeLayout(false);
            this.lblSearchBukkenName.ChildPanel.PerformLayout();
            this.lblSearchBukkenName.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).EndInit();
            this.grpEdit.ResumeLayout(false);
            this.lblJuchunNo.ChildPanel.ResumeLayout(false);
            this.lblJuchunNo.ChildPanel.PerformLayout();
            this.lblJuchunNo.ResumeLayout(false);
            this.lblArTagNo.ChildPanel.ResumeLayout(false);
            this.lblArTagNo.ChildPanel.PerformLayout();
            this.lblArTagNo.ResumeLayout(false);
            this.lblMailNotify.ChildPanel.ResumeLayout(false);
            this.lblMailNotify.ResumeLayout(false);
            this.lblNormalTagNo.ChildPanel.ResumeLayout(false);
            this.lblNormalTagNo.ChildPanel.PerformLayout();
            this.lblNormalTagNo.ResumeLayout(false);
            this.lblBukkenName.ChildPanel.ResumeLayout(false);
            this.lblBukkenName.ChildPanel.PerformLayout();
            this.lblBukkenName.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSearch;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnARListNotify;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnCommonNotify;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnDelete;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnUpdate;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnInsert;
        private DSWControl.DSWLabel.DSWLabel lblSearchBukkenName;
        private DSWControl.DSWTextBox.DSWTextBox txtSearchBukkenName;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnSearch;
        private GrapeCity.Win.ElTabelle.Sheet shtResult;
        private System.Windows.Forms.GroupBox grpEdit;
        private DSWControl.DSWLabel.DSWLabel lblMailNotify;
        private DSWControl.DSWComboBox.DSWComboBox cboMailNotify;
        private DSWControl.DSWLabel.DSWLabel lblNormalTagNo;
        private DSWControl.DSWTextBox.DSWTextBox txtNormalTagNo;
        private DSWControl.DSWLabel.DSWLabel lblBukkenName;
        private DSWControl.DSWTextBox.DSWTextBox txtBukkenName;
        private DSWControl.DSWLabel.DSWLabel lblArTagNo;
        private DSWControl.DSWTextBox.DSWTextBox txtArTagNo;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnShinchokuKanriNotify;
        private DSWControl.DSWLabel.DSWLabel lblJuchunNo;
        private DSWControl.DSWTextBox.DSWTextBox txtJuchuNo;
        private DSWControl.DSWTextBox.DSWTextBox dswTextBox1;

    }
}