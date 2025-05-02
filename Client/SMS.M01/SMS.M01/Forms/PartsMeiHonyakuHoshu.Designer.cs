namespace SMS.M01.Forms
{
    partial class PartsMeiHonyakuHoshu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PartsMeiHonyakuHoshu));
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.btnStart = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnDelete = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnUpdate = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnInsert = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.shtResult = new GrapeCity.Win.ElTabelle.Sheet();
            this.lblSearchPartName = new DSWControl.DSWLabel.DSWLabel();
            this.txtSearchPartName = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblSearchType = new DSWControl.DSWLabel.DSWLabel();
            this.txtSearchType = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblSearchPartNameJa = new DSWControl.DSWLabel.DSWLabel();
            this.txtSearchPartNameJa = new DSWControl.DSWTextBox.DSWTextBox();
            this.grpEdit = new System.Windows.Forms.GroupBox();
            this.lblCustomsStatus = new DSWControl.DSWLabel.DSWLabel();
            this.txtCustomsStatus = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblNotes = new DSWControl.DSWLabel.DSWLabel();
            this.txtNotes = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblCustomer = new DSWControl.DSWLabel.DSWLabel();
            this.txtCustomer = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblOriginCountry = new DSWControl.DSWLabel.DSWLabel();
            this.txtOriginCountry = new DSWControl.DSWTextBox.DSWTextBox();
            this.LblMaker = new DSWControl.DSWLabel.DSWLabel();
            this.txtMaker = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblInvName = new DSWControl.DSWLabel.DSWLabel();
            this.txtInvName = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblPartName = new DSWControl.DSWLabel.DSWLabel();
            this.txtPartName = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblType = new DSWControl.DSWLabel.DSWLabel();
            this.txtType = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblPartNameJa = new DSWControl.DSWLabel.DSWLabel();
            this.txtPartNameJa = new DSWControl.DSWTextBox.DSWTextBox();
            this.dswTextBox2 = new DSWControl.DSWTextBox.DSWTextBox();
            this.pnlMain.SuspendLayout();
            this.grpSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).BeginInit();
            this.lblSearchPartName.ChildPanel.SuspendLayout();
            this.lblSearchPartName.SuspendLayout();
            this.lblSearchType.ChildPanel.SuspendLayout();
            this.lblSearchType.SuspendLayout();
            this.lblSearchPartNameJa.ChildPanel.SuspendLayout();
            this.lblSearchPartNameJa.SuspendLayout();
            this.grpEdit.SuspendLayout();
            this.lblCustomsStatus.ChildPanel.SuspendLayout();
            this.lblCustomsStatus.SuspendLayout();
            this.lblNotes.ChildPanel.SuspendLayout();
            this.lblNotes.SuspendLayout();
            this.lblCustomer.ChildPanel.SuspendLayout();
            this.lblCustomer.SuspendLayout();
            this.lblOriginCountry.ChildPanel.SuspendLayout();
            this.lblOriginCountry.SuspendLayout();
            this.LblMaker.ChildPanel.SuspendLayout();
            this.LblMaker.SuspendLayout();
            this.lblInvName.ChildPanel.SuspendLayout();
            this.lblInvName.SuspendLayout();
            this.lblPartName.ChildPanel.SuspendLayout();
            this.lblPartName.SuspendLayout();
            this.lblType.ChildPanel.SuspendLayout();
            this.lblType.SuspendLayout();
            this.lblPartNameJa.ChildPanel.SuspendLayout();
            this.lblPartNameJa.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.grpEdit);
            this.pnlMain.Controls.Add(this.grpSearch);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpEdit, 0);
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
            // fbrFunction.F09Button
            // 
            resources.ApplyResources(this.fbrFunction.F09Button, "fbrFunction.F09Button");
            // 
            // fbrFunction.F10Button
            // 
            resources.ApplyResources(this.fbrFunction.F10Button, "fbrFunction.F10Button");
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // grpSearch
            // 
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Controls.Add(this.btnStart);
            this.grpSearch.Controls.Add(this.btnDelete);
            this.grpSearch.Controls.Add(this.btnUpdate);
            this.grpSearch.Controls.Add(this.btnInsert);
            this.grpSearch.Controls.Add(this.shtResult);
            this.grpSearch.Controls.Add(this.lblSearchPartName);
            this.grpSearch.Controls.Add(this.lblSearchType);
            this.grpSearch.Controls.Add(this.lblSearchPartNameJa);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
            // 
            // btnStart
            // 
            resources.ApplyResources(this.btnStart, "btnStart");
            this.btnStart.Name = "btnStart";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
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
            // shtResult
            // 
            resources.ApplyResources(this.shtResult, "shtResult");
            this.shtResult.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtResult.Data")));
            this.shtResult.Name = "shtResult";
            // 
            // lblSearchPartName
            // 
            // 
            // lblSearchPartName.ChildPanel
            // 
            this.lblSearchPartName.ChildPanel.Controls.Add(this.txtSearchPartName);
            this.lblSearchPartName.IsFocusChangeColor = false;
            this.lblSearchPartName.LabelWidth = 120;
            resources.ApplyResources(this.lblSearchPartName, "lblSearchPartName");
            this.lblSearchPartName.Name = "lblSearchPartName";
            this.lblSearchPartName.SplitterWidth = 0;
            // 
            // txtSearchPartName
            // 
            resources.ApplyResources(this.txtSearchPartName, "txtSearchPartName");
            this.txtSearchPartName.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtSearchPartName.InputRegulation = "";
            this.txtSearchPartName.MaxByteLengthMode = true;
            this.txtSearchPartName.Name = "txtSearchPartName";
            this.txtSearchPartName.OneLineMaxLength = 100;
            this.txtSearchPartName.ProhibitionChar = null;
            // 
            // lblSearchType
            // 
            // 
            // lblSearchType.ChildPanel
            // 
            this.lblSearchType.ChildPanel.Controls.Add(this.txtSearchType);
            this.lblSearchType.IsFocusChangeColor = false;
            this.lblSearchType.LabelWidth = 120;
            resources.ApplyResources(this.lblSearchType, "lblSearchType");
            this.lblSearchType.Name = "lblSearchType";
            this.lblSearchType.SplitterWidth = 0;
            // 
            // txtSearchType
            // 
            resources.ApplyResources(this.txtSearchType, "txtSearchType");
            this.txtSearchType.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtSearchType.InputRegulation = "";
            this.txtSearchType.MaxByteLengthMode = true;
            this.txtSearchType.Name = "txtSearchType";
            this.txtSearchType.OneLineMaxLength = 100;
            this.txtSearchType.ProhibitionChar = null;
            // 
            // lblSearchPartNameJa
            // 
            // 
            // lblSearchPartNameJa.ChildPanel
            // 
            this.lblSearchPartNameJa.ChildPanel.Controls.Add(this.txtSearchPartNameJa);
            this.lblSearchPartNameJa.IsFocusChangeColor = false;
            this.lblSearchPartNameJa.LabelWidth = 120;
            resources.ApplyResources(this.lblSearchPartNameJa, "lblSearchPartNameJa");
            this.lblSearchPartNameJa.Name = "lblSearchPartNameJa";
            this.lblSearchPartNameJa.SplitterWidth = 0;
            // 
            // txtSearchPartNameJa
            // 
            resources.ApplyResources(this.txtSearchPartNameJa, "txtSearchPartNameJa");
            this.txtSearchPartNameJa.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtSearchPartNameJa.InputRegulation = "F";
            this.txtSearchPartNameJa.MaxByteLengthMode = true;
            this.txtSearchPartNameJa.Name = "txtSearchPartNameJa";
            this.txtSearchPartNameJa.OneLineMaxLength = 100;
            this.txtSearchPartNameJa.ProhibitionChar = null;
            // 
            // grpEdit
            // 
            resources.ApplyResources(this.grpEdit, "grpEdit");
            this.grpEdit.Controls.Add(this.lblCustomsStatus);
            this.grpEdit.Controls.Add(this.lblNotes);
            this.grpEdit.Controls.Add(this.lblCustomer);
            this.grpEdit.Controls.Add(this.lblOriginCountry);
            this.grpEdit.Controls.Add(this.LblMaker);
            this.grpEdit.Controls.Add(this.lblInvName);
            this.grpEdit.Controls.Add(this.lblPartName);
            this.grpEdit.Controls.Add(this.lblType);
            this.grpEdit.Controls.Add(this.lblPartNameJa);
            this.grpEdit.Name = "grpEdit";
            this.grpEdit.TabStop = false;
            // 
            // lblCustomsStatus
            // 
            // 
            // lblCustomsStatus.ChildPanel
            // 
            this.lblCustomsStatus.ChildPanel.Controls.Add(this.txtCustomsStatus);
            this.lblCustomsStatus.IsFocusChangeColor = false;
            this.lblCustomsStatus.LabelWidth = 120;
            resources.ApplyResources(this.lblCustomsStatus, "lblCustomsStatus");
            this.lblCustomsStatus.Name = "lblCustomsStatus";
            this.lblCustomsStatus.SplitterWidth = 0;
            // 
            // txtCustomsStatus
            // 
            resources.ApplyResources(this.txtCustomsStatus, "txtCustomsStatus");
            this.txtCustomsStatus.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtCustomsStatus.InputRegulation = "";
            this.txtCustomsStatus.MaxByteLengthMode = true;
            this.txtCustomsStatus.Name = "txtCustomsStatus";
            this.txtCustomsStatus.OneLineMaxLength = 60;
            this.txtCustomsStatus.ProhibitionChar = null;
            // 
            // lblNotes
            // 
            // 
            // lblNotes.ChildPanel
            // 
            this.lblNotes.ChildPanel.Controls.Add(this.txtNotes);
            this.lblNotes.IsFocusChangeColor = false;
            this.lblNotes.LabelWidth = 120;
            resources.ApplyResources(this.lblNotes, "lblNotes");
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.SplitterWidth = 0;
            // 
            // txtNotes
            // 
            resources.ApplyResources(this.txtNotes, "txtNotes");
            this.txtNotes.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtNotes.InputRegulation = "";
            this.txtNotes.MaxByteLengthMode = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.OneLineMaxLength = 60;
            this.txtNotes.ProhibitionChar = null;
            // 
            // lblCustomer
            // 
            // 
            // lblCustomer.ChildPanel
            // 
            this.lblCustomer.ChildPanel.Controls.Add(this.txtCustomer);
            this.lblCustomer.IsFocusChangeColor = false;
            this.lblCustomer.LabelWidth = 120;
            resources.ApplyResources(this.lblCustomer, "lblCustomer");
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.SplitterWidth = 0;
            // 
            // txtCustomer
            // 
            resources.ApplyResources(this.txtCustomer, "txtCustomer");
            this.txtCustomer.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtCustomer.InputRegulation = "";
            this.txtCustomer.MaxByteLengthMode = true;
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.OneLineMaxLength = 60;
            this.txtCustomer.ProhibitionChar = null;
            // 
            // lblOriginCountry
            // 
            // 
            // lblOriginCountry.ChildPanel
            // 
            this.lblOriginCountry.ChildPanel.Controls.Add(this.txtOriginCountry);
            this.lblOriginCountry.IsFocusChangeColor = false;
            this.lblOriginCountry.LabelWidth = 120;
            resources.ApplyResources(this.lblOriginCountry, "lblOriginCountry");
            this.lblOriginCountry.Name = "lblOriginCountry";
            this.lblOriginCountry.SplitterWidth = 0;
            // 
            // txtOriginCountry
            // 
            resources.ApplyResources(this.txtOriginCountry, "txtOriginCountry");
            this.txtOriginCountry.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtOriginCountry.InputRegulation = "";
            this.txtOriginCountry.MaxByteLengthMode = true;
            this.txtOriginCountry.Name = "txtOriginCountry";
            this.txtOriginCountry.OneLineMaxLength = 30;
            this.txtOriginCountry.ProhibitionChar = null;
            // 
            // LblMaker
            // 
            // 
            // LblMaker.ChildPanel
            // 
            this.LblMaker.ChildPanel.Controls.Add(this.txtMaker);
            this.LblMaker.IsFocusChangeColor = false;
            this.LblMaker.LabelWidth = 120;
            resources.ApplyResources(this.LblMaker, "LblMaker");
            this.LblMaker.Name = "LblMaker";
            this.LblMaker.SplitterWidth = 0;
            // 
            // txtMaker
            // 
            resources.ApplyResources(this.txtMaker, "txtMaker");
            this.txtMaker.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtMaker.InputRegulation = "";
            this.txtMaker.MaxByteLengthMode = true;
            this.txtMaker.Name = "txtMaker";
            this.txtMaker.OneLineMaxLength = 40;
            this.txtMaker.ProhibitionChar = null;
            // 
            // lblInvName
            // 
            // 
            // lblInvName.ChildPanel
            // 
            this.lblInvName.ChildPanel.Controls.Add(this.txtInvName);
            this.lblInvName.IsFocusChangeColor = false;
            this.lblInvName.LabelWidth = 120;
            resources.ApplyResources(this.lblInvName, "lblInvName");
            this.lblInvName.Name = "lblInvName";
            this.lblInvName.SplitterWidth = 0;
            // 
            // txtInvName
            // 
            resources.ApplyResources(this.txtInvName, "txtInvName");
            this.txtInvName.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtInvName.InputRegulation = "";
            this.txtInvName.MaxByteLengthMode = true;
            this.txtInvName.Name = "txtInvName";
            this.txtInvName.OneLineMaxLength = 40;
            this.txtInvName.ProhibitionChar = null;
            // 
            // lblPartName
            // 
            // 
            // lblPartName.ChildPanel
            // 
            this.lblPartName.ChildPanel.Controls.Add(this.txtPartName);
            this.lblPartName.IsFocusChangeColor = false;
            this.lblPartName.IsNecessary = true;
            this.lblPartName.LabelWidth = 120;
            resources.ApplyResources(this.lblPartName, "lblPartName");
            this.lblPartName.Name = "lblPartName";
            this.lblPartName.SplitterWidth = 0;
            // 
            // txtPartName
            // 
            resources.ApplyResources(this.txtPartName, "txtPartName");
            this.txtPartName.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtPartName.InputRegulation = "";
            this.txtPartName.MaxByteLengthMode = true;
            this.txtPartName.Name = "txtPartName";
            this.txtPartName.OneLineMaxLength = 100;
            this.txtPartName.ProhibitionChar = null;
            // 
            // lblType
            // 
            // 
            // lblType.ChildPanel
            // 
            this.lblType.ChildPanel.Controls.Add(this.txtType);
            this.lblType.IsFocusChangeColor = false;
            this.lblType.LabelWidth = 120;
            resources.ApplyResources(this.lblType, "lblType");
            this.lblType.Name = "lblType";
            this.lblType.SplitterWidth = 0;
            // 
            // txtType
            // 
            resources.ApplyResources(this.txtType, "txtType");
            this.txtType.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtType.InputRegulation = "";
            this.txtType.MaxByteLengthMode = true;
            this.txtType.Name = "txtType";
            this.txtType.OneLineMaxLength = 100;
            this.txtType.ProhibitionChar = null;
            // 
            // lblPartNameJa
            // 
            // 
            // lblPartNameJa.ChildPanel
            // 
            this.lblPartNameJa.ChildPanel.Controls.Add(this.txtPartNameJa);
            this.lblPartNameJa.IsFocusChangeColor = false;
            this.lblPartNameJa.IsNecessary = true;
            this.lblPartNameJa.LabelWidth = 120;
            resources.ApplyResources(this.lblPartNameJa, "lblPartNameJa");
            this.lblPartNameJa.Name = "lblPartNameJa";
            this.lblPartNameJa.SplitterWidth = 0;
            // 
            // txtPartNameJa
            // 
            resources.ApplyResources(this.txtPartNameJa, "txtPartNameJa");
            this.txtPartNameJa.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtPartNameJa.InputRegulation = "F";
            this.txtPartNameJa.MaxByteLengthMode = true;
            this.txtPartNameJa.Name = "txtPartNameJa";
            this.txtPartNameJa.OneLineMaxLength = 100;
            this.txtPartNameJa.ProhibitionChar = null;
            // 
            // dswTextBox2
            // 
            resources.ApplyResources(this.dswTextBox2, "dswTextBox2");
            this.dswTextBox2.FocusBackColor = System.Drawing.SystemColors.Window;
            this.dswTextBox2.InputRegulation = "";
            this.dswTextBox2.Name = "dswTextBox2";
            this.dswTextBox2.ProhibitionChar = null;
            // 
            // PartsMeiHonyakuHoshu
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "PartsMeiHonyakuHoshu";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.grpSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).EndInit();
            this.lblSearchPartName.ChildPanel.ResumeLayout(false);
            this.lblSearchPartName.ChildPanel.PerformLayout();
            this.lblSearchPartName.ResumeLayout(false);
            this.lblSearchType.ChildPanel.ResumeLayout(false);
            this.lblSearchType.ChildPanel.PerformLayout();
            this.lblSearchType.ResumeLayout(false);
            this.lblSearchPartNameJa.ChildPanel.ResumeLayout(false);
            this.lblSearchPartNameJa.ChildPanel.PerformLayout();
            this.lblSearchPartNameJa.ResumeLayout(false);
            this.grpEdit.ResumeLayout(false);
            this.lblCustomsStatus.ChildPanel.ResumeLayout(false);
            this.lblCustomsStatus.ChildPanel.PerformLayout();
            this.lblCustomsStatus.ResumeLayout(false);
            this.lblNotes.ChildPanel.ResumeLayout(false);
            this.lblNotes.ChildPanel.PerformLayout();
            this.lblNotes.ResumeLayout(false);
            this.lblCustomer.ChildPanel.ResumeLayout(false);
            this.lblCustomer.ChildPanel.PerformLayout();
            this.lblCustomer.ResumeLayout(false);
            this.lblOriginCountry.ChildPanel.ResumeLayout(false);
            this.lblOriginCountry.ChildPanel.PerformLayout();
            this.lblOriginCountry.ResumeLayout(false);
            this.LblMaker.ChildPanel.ResumeLayout(false);
            this.LblMaker.ChildPanel.PerformLayout();
            this.LblMaker.ResumeLayout(false);
            this.lblInvName.ChildPanel.ResumeLayout(false);
            this.lblInvName.ChildPanel.PerformLayout();
            this.lblInvName.ResumeLayout(false);
            this.lblPartName.ChildPanel.ResumeLayout(false);
            this.lblPartName.ChildPanel.PerformLayout();
            this.lblPartName.ResumeLayout(false);
            this.lblType.ChildPanel.ResumeLayout(false);
            this.lblType.ChildPanel.PerformLayout();
            this.lblType.ResumeLayout(false);
            this.lblPartNameJa.ChildPanel.ResumeLayout(false);
            this.lblPartNameJa.ChildPanel.PerformLayout();
            this.lblPartNameJa.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSearch;
        private DSWControl.DSWLabel.DSWLabel lblSearchPartNameJa;
        private DSWControl.DSWTextBox.DSWTextBox txtSearchPartNameJa;
        private DSWControl.DSWLabel.DSWLabel lblSearchType;
        private DSWControl.DSWTextBox.DSWTextBox txtSearchType;
        private GrapeCity.Win.ElTabelle.Sheet shtResult;
        private DSWControl.DSWLabel.DSWLabel lblSearchPartName;
        private DSWControl.DSWTextBox.DSWTextBox txtSearchPartName;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnInsert;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnUpdate;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnDelete;
        private System.Windows.Forms.GroupBox grpEdit;
        private DSWControl.DSWLabel.DSWLabel lblPartNameJa;
        private DSWControl.DSWTextBox.DSWTextBox txtPartNameJa;
        private DSWControl.DSWLabel.DSWLabel lblType;
        private DSWControl.DSWTextBox.DSWTextBox txtType;
        private DSWControl.DSWLabel.DSWLabel lblPartName;
        private DSWControl.DSWTextBox.DSWTextBox txtPartName;
        private DSWControl.DSWTextBox.DSWTextBox dswTextBox2;
        private DSWControl.DSWLabel.DSWLabel LblMaker;
        private DSWControl.DSWTextBox.DSWTextBox txtMaker;
        private DSWControl.DSWLabel.DSWLabel lblInvName;
        private DSWControl.DSWTextBox.DSWTextBox txtInvName;
        private DSWControl.DSWLabel.DSWLabel lblOriginCountry;
        private DSWControl.DSWTextBox.DSWTextBox txtOriginCountry;
        private DSWControl.DSWLabel.DSWLabel lblCustomer;
        private DSWControl.DSWTextBox.DSWTextBox txtCustomer;
        private DSWControl.DSWLabel.DSWLabel lblNotes;
        private DSWControl.DSWTextBox.DSWTextBox txtNotes;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnStart;
        private DSWControl.DSWLabel.DSWLabel lblCustomsStatus;
        private DSWControl.DSWTextBox.DSWTextBox txtCustomsStatus;
    }
}