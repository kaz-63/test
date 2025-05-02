namespace SMS.U01.Forms
{
    partial class UkeireSettei
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UkeireSettei));
            this.shtMeisai = new GrapeCity.Win.ElTabelle.Sheet();
            this.btnAllNotCheck = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnAllCheck = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblNonyusaki = new DSWControl.DSWLabel.DSWLabel();
            this.txtNonyuusaki = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblShip = new DSWControl.DSWLabel.DSWLabel();
            this.txtShip = new DSWControl.DSWTextBox.DSWTextBox();
            this.dswLabel1 = new DSWControl.DSWLabel.DSWLabel();
            this.lblPrefix = new DSWControl.DSWLabel.DSWLabel();
            this.txtUkeireNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnDisp = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblDispSelect = new DSWControl.DSWLabel.DSWLabel();
            this.cboDispSelect = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblCODE = new DSWControl.DSWLabel.DSWLabel();
            this.dtpUkeireDate = new System.Windows.Forms.DateTimePicker();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).BeginInit();
            this.lblNonyusaki.ChildPanel.SuspendLayout();
            this.lblNonyusaki.SuspendLayout();
            this.lblShip.ChildPanel.SuspendLayout();
            this.lblShip.SuspendLayout();
            this.dswLabel1.ChildPanel.SuspendLayout();
            this.dswLabel1.SuspendLayout();
            this.lblPrefix.ChildPanel.SuspendLayout();
            this.lblPrefix.SuspendLayout();
            this.lblDispSelect.ChildPanel.SuspendLayout();
            this.lblDispSelect.SuspendLayout();
            this.lblCODE.ChildPanel.SuspendLayout();
            this.lblCODE.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.grpSearch);
            this.pnlMain.Controls.Add(this.lblCODE);
            this.pnlMain.Controls.Add(this.lblNonyusaki);
            this.pnlMain.Controls.Add(this.shtMeisai);
            this.pnlMain.Controls.Add(this.lblShip);
            this.pnlMain.Controls.Add(this.btnAllNotCheck);
            this.pnlMain.Controls.Add(this.btnAllCheck);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.SetChildIndex(this.btnAllCheck, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnAllNotCheck, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblShip, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtMeisai, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblNonyusaki, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCODE, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
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
            // shtMeisai
            // 
            resources.ApplyResources(this.shtMeisai, "shtMeisai");
            this.shtMeisai.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtMeisai.Data")));
            this.shtMeisai.Name = "shtMeisai";
            this.shtMeisai.CellNotify += new GrapeCity.Win.ElTabelle.CellNotifyEventHandler(this.shtMeisai_CellNotify);
            // 
            // btnAllNotCheck
            // 
            resources.ApplyResources(this.btnAllNotCheck, "btnAllNotCheck");
            this.btnAllNotCheck.Name = "btnAllNotCheck";
            this.btnAllNotCheck.Click += new System.EventHandler(this.btnAllDeselect_Click);
            // 
            // btnAllCheck
            // 
            resources.ApplyResources(this.btnAllCheck, "btnAllCheck");
            this.btnAllCheck.Name = "btnAllCheck";
            this.btnAllCheck.Click += new System.EventHandler(this.btnAllSelect_Click);
            // 
            // lblNonyusaki
            // 
            // 
            // lblNonyusaki.ChildPanel
            // 
            this.lblNonyusaki.ChildPanel.Controls.Add(this.txtNonyuusaki);
            this.lblNonyusaki.IsFocusChangeColor = false;
            this.lblNonyusaki.LabelWidth = 80;
            resources.ApplyResources(this.lblNonyusaki, "lblNonyusaki");
            this.lblNonyusaki.Name = "lblNonyusaki";
            this.lblNonyusaki.SplitterWidth = 0;
            // 
            // txtNonyuusaki
            // 
            this.txtNonyuusaki.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtNonyuusaki, "txtNonyuusaki");
            this.txtNonyuusaki.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtNonyuusaki.InputRegulation = "F";
            this.txtNonyuusaki.Name = "txtNonyuusaki";
            this.txtNonyuusaki.OneLineMaxLength = 60;
            this.txtNonyuusaki.ProhibitionChar = null;
            this.txtNonyuusaki.ReadOnly = true;
            // 
            // lblShip
            // 
            // 
            // lblShip.ChildPanel
            // 
            this.lblShip.ChildPanel.Controls.Add(this.txtShip);
            this.lblShip.IsFocusChangeColor = false;
            this.lblShip.LabelWidth = 80;
            resources.ApplyResources(this.lblShip, "lblShip");
            this.lblShip.Name = "lblShip";
            this.lblShip.SplitterWidth = 0;
            // 
            // txtShip
            // 
            this.txtShip.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtShip, "txtShip");
            this.txtShip.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtShip.InputRegulation = "abnls";
            this.txtShip.IsInputRegulation = true;
            this.txtShip.Name = "txtShip";
            this.txtShip.OneLineMaxLength = 5;
            this.txtShip.ProhibitionChar = null;
            this.txtShip.ReadOnly = true;
            // 
            // dswLabel1
            // 
            // 
            // dswLabel1.ChildPanel
            // 
            this.dswLabel1.ChildPanel.Controls.Add(this.lblPrefix);
            this.dswLabel1.IsFocusChangeColor = false;
            this.dswLabel1.IsNecessary = true;
            this.dswLabel1.LabelWidth = 80;
            resources.ApplyResources(this.dswLabel1, "dswLabel1");
            this.dswLabel1.Name = "dswLabel1";
            this.dswLabel1.SplitterWidth = 0;
            // 
            // lblPrefix
            // 
            // 
            // lblPrefix.ChildPanel
            // 
            this.lblPrefix.ChildPanel.Controls.Add(this.txtUkeireNo);
            resources.ApplyResources(this.lblPrefix, "lblPrefix");
            this.lblPrefix.IsFocusChangeColor = false;
            this.lblPrefix.LabelWidth = 20;
            this.lblPrefix.Name = "lblPrefix";
            this.lblPrefix.NormalBackColor = System.Drawing.Color.LightSkyBlue;
            this.lblPrefix.SplitterWidth = 0;
            // 
            // txtUkeireNo
            // 
            this.txtUkeireNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            resources.ApplyResources(this.txtUkeireNo, "txtUkeireNo");
            this.txtUkeireNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtUkeireNo.InputRegulation = "nab";
            this.txtUkeireNo.IsInputRegulation = true;
            this.txtUkeireNo.Name = "txtUkeireNo";
            this.txtUkeireNo.OneLineMaxLength = 5;
            this.txtUkeireNo.ProhibitionChar = null;
            this.txtUkeireNo.Leave += new System.EventHandler(this.txtUkeireNo_Leave);
            // 
            // btnDisp
            // 
            resources.ApplyResources(this.btnDisp, "btnDisp");
            this.btnDisp.Name = "btnDisp";
            this.btnDisp.Click += new System.EventHandler(this.btnDisp_Click);
            // 
            // lblDispSelect
            // 
            // 
            // lblDispSelect.ChildPanel
            // 
            this.lblDispSelect.ChildPanel.Controls.Add(this.cboDispSelect);
            this.lblDispSelect.IsFocusChangeColor = false;
            this.lblDispSelect.IsNecessary = true;
            this.lblDispSelect.LabelWidth = 80;
            resources.ApplyResources(this.lblDispSelect, "lblDispSelect");
            this.lblDispSelect.Name = "lblDispSelect";
            this.lblDispSelect.SplitterWidth = 0;
            // 
            // cboDispSelect
            // 
            resources.ApplyResources(this.cboDispSelect, "cboDispSelect");
            this.cboDispSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDispSelect.Name = "cboDispSelect";
            this.cboDispSelect.SelectedIndexChanged += new System.EventHandler(this.cboDispSelect_SelectedIndexChanged);
            // 
            // lblCODE
            // 
            // 
            // lblCODE.ChildPanel
            // 
            this.lblCODE.ChildPanel.Controls.Add(this.dtpUkeireDate);
            this.lblCODE.IsFocusChangeColor = false;
            this.lblCODE.LabelWidth = 90;
            resources.ApplyResources(this.lblCODE, "lblCODE");
            this.lblCODE.Name = "lblCODE";
            this.lblCODE.SplitterWidth = 0;
            // 
            // dtpUkeireDate
            // 
            resources.ApplyResources(this.dtpUkeireDate, "dtpUkeireDate");
            this.dtpUkeireDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpUkeireDate.Name = "dtpUkeireDate";
            // 
            // grpSearch
            // 
            this.grpSearch.Controls.Add(this.lblDispSelect);
            this.grpSearch.Controls.Add(this.dswLabel1);
            this.grpSearch.Controls.Add(this.btnDisp);
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
            // 
            // UkeireSettei
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UkeireSettei";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).EndInit();
            this.lblNonyusaki.ChildPanel.ResumeLayout(false);
            this.lblNonyusaki.ChildPanel.PerformLayout();
            this.lblNonyusaki.ResumeLayout(false);
            this.lblShip.ChildPanel.ResumeLayout(false);
            this.lblShip.ChildPanel.PerformLayout();
            this.lblShip.ResumeLayout(false);
            this.dswLabel1.ChildPanel.ResumeLayout(false);
            this.dswLabel1.ResumeLayout(false);
            this.lblPrefix.ChildPanel.ResumeLayout(false);
            this.lblPrefix.ChildPanel.PerformLayout();
            this.lblPrefix.ResumeLayout(false);
            this.lblDispSelect.ChildPanel.ResumeLayout(false);
            this.lblDispSelect.ResumeLayout(false);
            this.lblCODE.ChildPanel.ResumeLayout(false);
            this.lblCODE.ResumeLayout(false);
            this.grpSearch.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DSWControl.DSWLabel.DSWLabel dswLabel1;
        private DSWControl.DSWLabel.DSWLabel lblPrefix;
        private DSWControl.DSWTextBox.DSWTextBox txtUkeireNo;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnDisp;
        private DSWControl.DSWLabel.DSWLabel lblDispSelect;
        private DSWControl.DSWComboBox.DSWComboBox cboDispSelect;
        private DSWControl.DSWLabel.DSWLabel lblCODE;
        private DSWControl.DSWLabel.DSWLabel lblNonyusaki;
        private DSWControl.DSWTextBox.DSWTextBox txtNonyuusaki;
        private GrapeCity.Win.ElTabelle.Sheet shtMeisai;
        private DSWControl.DSWLabel.DSWLabel lblShip;
        private DSWControl.DSWTextBox.DSWTextBox txtShip;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnAllNotCheck;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnAllCheck;
        private System.Windows.Forms.DateTimePicker dtpUkeireDate;
        private System.Windows.Forms.GroupBox grpSearch;
    }
}