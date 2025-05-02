namespace SMS.A01.Forms
{
    partial class ARJohoMeisaiGokiIchiran
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ARJohoMeisaiGokiIchiran));
            this.btnRangeRelease = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnRangeSelect = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnAllRelease = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnAllSelect = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblKishu = new DSWControl.DSWLabel.DSWLabel();
            this.txtKishu = new DSWControl.DSWTextBox.DSWTextBox();
            this.chkCheckResult = new System.Windows.Forms.CheckBox();
            this.lblGoki = new DSWControl.DSWLabel.DSWLabel();
            this.tlpGoki = new System.Windows.Forms.TableLayoutPanel();
            this.txtGoki = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnGoki = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblKishuSearch = new DSWControl.DSWLabel.DSWLabel();
            this.cboKishu = new DSWControl.DSWComboBox.DSWComboBox();
            this.lstGoki = new SMS.A01.Controls.DoubleBufferingListView();
            this.grpSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.lblKishu.ChildPanel.SuspendLayout();
            this.lblKishu.SuspendLayout();
            this.lblGoki.ChildPanel.SuspendLayout();
            this.lblGoki.SuspendLayout();
            this.tlpGoki.SuspendLayout();
            this.lblKishuSearch.ChildPanel.SuspendLayout();
            this.lblKishuSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSearch
            // 
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Controls.Add(this.lblKishuSearch);
            this.grpSearch.Controls.Add(this.lblGoki);
            this.grpSearch.Controls.Add(this.chkCheckResult);
            this.grpSearch.Controls.SetChildIndex(this.btnSearchAll, 0);
            this.grpSearch.Controls.SetChildIndex(this.chkCheckResult, 0);
            this.grpSearch.Controls.SetChildIndex(this.btnSearch, 0);
            this.grpSearch.Controls.SetChildIndex(this.lblGoki, 0);
            this.grpSearch.Controls.SetChildIndex(this.lblKishuSearch, 0);
            // 
            // btnSearch
            // 
            resources.ApplyResources(this.btnSearch, "btnSearch");
            // 
            // btnSearchAll
            // 
            resources.ApplyResources(this.btnSearchAll, "btnSearchAll");
            // 
            // btnSelect
            // 
            resources.ApplyResources(this.btnSelect, "btnSelect");
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            // 
            // shtResult
            // 
            resources.ApplyResources(this.shtResult, "shtResult");
            this.shtResult.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtResult.Data")));
            this.shtResult.TabStop = false;
            // 
            // pnlMain
            // 
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.Add(this.lstGoki);
            this.pnlMain.Controls.Add(this.btnRangeRelease);
            this.pnlMain.Controls.Add(this.btnRangeSelect);
            this.pnlMain.Controls.Add(this.btnAllRelease);
            this.pnlMain.Controls.Add(this.btnAllSelect);
            this.pnlMain.Controls.Add(this.lblKishu);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblKishu, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnAllSelect, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnAllRelease, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtResult, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnRangeSelect, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnRangeRelease, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnClose, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnSelect, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.lstGoki, 0);
            // 
            // pnlTitle
            // 
            resources.ApplyResources(this.pnlTitle, "pnlTitle");
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
            // fbrFunction.F03Button
            // 
            resources.ApplyResources(this.fbrFunction.F03Button, "fbrFunction.F03Button");
            // 
            // fbrFunction.F04Button
            // 
            resources.ApplyResources(this.fbrFunction.F04Button, "fbrFunction.F04Button");
            // 
            // fbrFunction.F05Button
            // 
            resources.ApplyResources(this.fbrFunction.F05Button, "fbrFunction.F05Button");
            // 
            // fbrFunction.F06Button
            // 
            resources.ApplyResources(this.fbrFunction.F06Button, "fbrFunction.F06Button");
            // 
            // fbrFunction.F07Button
            // 
            resources.ApplyResources(this.fbrFunction.F07Button, "fbrFunction.F07Button");
            // 
            // fbrFunction.F08Button
            // 
            resources.ApplyResources(this.fbrFunction.F08Button, "fbrFunction.F08Button");
            // 
            // fbrFunction.F09Button
            // 
            resources.ApplyResources(this.fbrFunction.F09Button, "fbrFunction.F09Button");
            // 
            // fbrFunction.F10Button
            // 
            resources.ApplyResources(this.fbrFunction.F10Button, "fbrFunction.F10Button");
            // 
            // fbrFunction.F11Button
            // 
            resources.ApplyResources(this.fbrFunction.F11Button, "fbrFunction.F11Button");
            // 
            // fbrFunction.F12Button
            // 
            resources.ApplyResources(this.fbrFunction.F12Button, "fbrFunction.F12Button");
            // 
            // txtUserName
            // 
            resources.ApplyResources(this.txtUserName, "txtUserName");
            // 
            // txtRoleName
            // 
            resources.ApplyResources(this.txtRoleName, "txtRoleName");
            // 
            // lblCorner
            // 
            resources.ApplyResources(this.lblCorner, "lblCorner");
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // btnRangeRelease
            // 
            resources.ApplyResources(this.btnRangeRelease, "btnRangeRelease");
            this.btnRangeRelease.Name = "btnRangeRelease";
            this.btnRangeRelease.Click += new System.EventHandler(this.btnRangeRelease_Click);
            // 
            // btnRangeSelect
            // 
            resources.ApplyResources(this.btnRangeSelect, "btnRangeSelect");
            this.btnRangeSelect.Name = "btnRangeSelect";
            this.btnRangeSelect.Click += new System.EventHandler(this.btnRangeSelect_Click);
            // 
            // btnAllRelease
            // 
            resources.ApplyResources(this.btnAllRelease, "btnAllRelease");
            this.btnAllRelease.Name = "btnAllRelease";
            this.btnAllRelease.Click += new System.EventHandler(this.btnAllRelease_Click);
            // 
            // btnAllSelect
            // 
            resources.ApplyResources(this.btnAllSelect, "btnAllSelect");
            this.btnAllSelect.Name = "btnAllSelect";
            this.btnAllSelect.Click += new System.EventHandler(this.btnAllSelect_Click);
            // 
            // lblKishu
            // 
            // 
            // lblKishu.ChildPanel
            // 
            this.lblKishu.ChildPanel.Controls.Add(this.txtKishu);
            this.lblKishu.IsFocusChangeColor = false;
            this.lblKishu.LabelWidth = 80;
            resources.ApplyResources(this.lblKishu, "lblKishu");
            this.lblKishu.Name = "lblKishu";
            this.lblKishu.NormalBackColor = System.Drawing.Color.LightSkyBlue;
            this.lblKishu.SplitterWidth = 0;
            // 
            // txtKishu
            // 
            this.txtKishu.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtKishu, "txtKishu");
            this.txtKishu.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtKishu.InputRegulation = global::SMS.A01.Properties.Resources.ARJoho_ElTabelleColumnEdit;
            this.txtKishu.Name = "txtKishu";
            this.txtKishu.ProhibitionChar = null;
            this.txtKishu.ReadOnly = true;
            // 
            // chkCheckResult
            // 
            resources.ApplyResources(this.chkCheckResult, "chkCheckResult");
            this.chkCheckResult.Name = "chkCheckResult";
            this.chkCheckResult.UseVisualStyleBackColor = true;
            // 
            // lblGoki
            // 
            // 
            // lblGoki.ChildPanel
            // 
            this.lblGoki.ChildPanel.Controls.Add(this.tlpGoki);
            this.lblGoki.IsFocusChangeColor = false;
            this.lblGoki.LabelWidth = 80;
            resources.ApplyResources(this.lblGoki, "lblGoki");
            this.lblGoki.Name = "lblGoki";
            this.lblGoki.SplitterWidth = 0;
            // 
            // tlpGoki
            // 
            resources.ApplyResources(this.tlpGoki, "tlpGoki");
            this.tlpGoki.Controls.Add(this.txtGoki, 0, 0);
            this.tlpGoki.Controls.Add(this.btnGoki, 1, 0);
            this.tlpGoki.Name = "tlpGoki";
            // 
            // txtGoki
            // 
            resources.ApplyResources(this.txtGoki, "txtGoki");
            this.txtGoki.BackColor = System.Drawing.SystemColors.Window;
            this.txtGoki.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtGoki.InputRegulation = "F";
            this.txtGoki.Name = "txtGoki";
            this.txtGoki.ProhibitionChar = null;
            // 
            // btnGoki
            // 
            resources.ApplyResources(this.btnGoki, "btnGoki");
            this.btnGoki.Name = "btnGoki";
            this.btnGoki.Click += new System.EventHandler(this.btnGoki_Click);
            // 
            // lblKishuSearch
            // 
            // 
            // lblKishuSearch.ChildPanel
            // 
            this.lblKishuSearch.ChildPanel.Controls.Add(this.cboKishu);
            this.lblKishuSearch.IsFocusChangeColor = false;
            this.lblKishuSearch.LabelWidth = 80;
            resources.ApplyResources(this.lblKishuSearch, "lblKishuSearch");
            this.lblKishuSearch.Name = "lblKishuSearch";
            this.lblKishuSearch.SplitterWidth = 0;
            // 
            // cboKishu
            // 
            this.cboKishu.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboKishu.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.cboKishu, "cboKishu");
            this.cboKishu.Name = "cboKishu";
            this.cboKishu.SelectedIndexChanged += new System.EventHandler(this.cboKishu_SelectedIndexChanged);
            // 
            // lstGoki
            // 
            resources.ApplyResources(this.lstGoki, "lstGoki");
            this.lstGoki.CheckBoxes = true;
            this.lstGoki.HideSelection = false;
            this.lstGoki.Name = "lstGoki";
            this.lstGoki.UseCompatibleStateImageBehavior = false;
            this.lstGoki.View = System.Windows.Forms.View.SmallIcon;
            this.lstGoki.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstGoki_ItemCheck);
            // 
            // ARJohoMeisaiGokiIchiran
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ARJohoMeisaiGokiIchiran";
            this.grpSearch.ResumeLayout(false);
            this.grpSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.lblKishu.ChildPanel.ResumeLayout(false);
            this.lblKishu.ChildPanel.PerformLayout();
            this.lblKishu.ResumeLayout(false);
            this.lblGoki.ChildPanel.ResumeLayout(false);
            this.lblGoki.ResumeLayout(false);
            this.tlpGoki.ResumeLayout(false);
            this.tlpGoki.PerformLayout();
            this.lblKishuSearch.ChildPanel.ResumeLayout(false);
            this.lblKishuSearch.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DSWControl.DSWFunctionButton.DSWFunctionButton btnRangeRelease;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnRangeSelect;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnAllRelease;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnAllSelect;
        private DSWControl.DSWLabel.DSWLabel lblKishu;
        private DSWControl.DSWTextBox.DSWTextBox txtKishu;
        private System.Windows.Forms.CheckBox chkCheckResult;
        private DSWControl.DSWLabel.DSWLabel lblGoki;
        private System.Windows.Forms.TableLayoutPanel tlpGoki;
        private DSWControl.DSWTextBox.DSWTextBox txtGoki;
        private DSWControl.DSWLabel.DSWLabel lblKishuSearch;
        private DSWControl.DSWComboBox.DSWComboBox cboKishu;
        private SMS.A01.Controls.DoubleBufferingListView lstGoki;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnGoki;
    }
}