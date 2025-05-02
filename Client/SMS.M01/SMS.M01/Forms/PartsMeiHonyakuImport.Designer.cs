namespace SMS.M01.Forms
{
    partial class PartsMeiHonyakuImport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PartsMeiHonyakuImport));
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.btnSelect = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblExcel = new DSWControl.DSWLabel.DSWLabel();
            this.txtExcel = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnStart = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.shtResult = new GrapeCity.Win.ElTabelle.Sheet();
            this.pnlMain.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.lblExcel.ChildPanel.SuspendLayout();
            this.lblExcel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.shtResult);
            this.pnlMain.Controls.Add(this.grpSearch);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
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
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Controls.Add(this.btnSelect);
            this.grpSearch.Controls.Add(this.lblExcel);
            this.grpSearch.Controls.Add(this.btnStart);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
            // 
            // btnSelect
            // 
            resources.ApplyResources(this.btnSelect, "btnSelect");
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.TabStop = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
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
            this.txtExcel.InputRegulation = "";
            this.txtExcel.Name = "txtExcel";
            this.txtExcel.ProhibitionChar = null;
            this.txtExcel.ReadOnly = true;
            this.txtExcel.TabStop = false;
            // 
            // btnStart
            // 
            resources.ApplyResources(this.btnStart, "btnStart");
            this.btnStart.Name = "btnStart";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // shtResult
            // 
            resources.ApplyResources(this.shtResult, "shtResult");
            this.shtResult.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtResult.Data")));
            this.shtResult.Name = "shtResult";
            // 
            // PartsMeiHonyakuImport
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "PartsMeiHonyakuImport";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.grpSearch.ResumeLayout(false);
            this.lblExcel.ChildPanel.ResumeLayout(false);
            this.lblExcel.ChildPanel.PerformLayout();
            this.lblExcel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSearch;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnStart;
        private DSWControl.DSWLabel.DSWLabel lblExcel;
        private DSWControl.DSWTextBox.DSWTextBox txtExcel;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnSelect;
        private GrapeCity.Win.ElTabelle.Sheet shtResult;
    }
}