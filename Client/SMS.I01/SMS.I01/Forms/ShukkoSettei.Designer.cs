namespace SMS.I01.Forms
{
    partial class ShukkoSettei
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShukkoSettei));
            this.shtMeisai = new GrapeCity.Win.ElTabelle.Sheet();
            this.lblKanryoNo = new DSWControl.DSWLabel.DSWLabel();
            this.lblKanryoNoPrefix = new DSWControl.DSWLabel.DSWLabel();
            this.txtKanryoNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnDisp = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.lblKanryoTani = new DSWControl.DSWLabel.DSWLabel();
            this.cboKanryoTani = new DSWControl.DSWComboBox.DSWComboBox();
            this.ChildPanel = new DSWControl.DSWLabel.CustomPanel();
            this.lblKanryoDate = new DSWControl.DSWLabel.DSWLabel();
            this.dtpKanryoDate = new System.Windows.Forms.DateTimePicker();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).BeginInit();
            this.lblKanryoNo.ChildPanel.SuspendLayout();
            this.lblKanryoNo.SuspendLayout();
            this.lblKanryoNoPrefix.ChildPanel.SuspendLayout();
            this.lblKanryoNoPrefix.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.lblKanryoTani.ChildPanel.SuspendLayout();
            this.lblKanryoTani.SuspendLayout();
            this.lblKanryoDate.ChildPanel.SuspendLayout();
            this.lblKanryoDate.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.grpSearch);
            this.pnlMain.Controls.Add(this.shtMeisai);
            this.pnlMain.Controls.Add(this.lblKanryoDate);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.SetChildIndex(this.lblKanryoDate, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtMeisai, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
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
            // fbrFunction.F03Button
            // 
            resources.ApplyResources(this.fbrFunction.F03Button, "fbrFunction.F03Button");
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
            this.shtMeisai.KeyDown += new System.Windows.Forms.KeyEventHandler(this.shtMeisai_KeyDown);
            // 
            // lblKanryoNo
            // 
            // 
            // lblKanryoNo.ChildPanel
            // 
            this.lblKanryoNo.ChildPanel.Controls.Add(this.lblKanryoNoPrefix);
            this.lblKanryoNo.IsFocusChangeColor = false;
            this.lblKanryoNo.IsNecessary = true;
            this.lblKanryoNo.LabelWidth = 80;
            resources.ApplyResources(this.lblKanryoNo, "lblKanryoNo");
            this.lblKanryoNo.Name = "lblKanryoNo";
            this.lblKanryoNo.SplitterWidth = 0;
            // 
            // lblKanryoNoPrefix
            // 
            // 
            // lblKanryoNoPrefix.ChildPanel
            // 
            this.lblKanryoNoPrefix.ChildPanel.Controls.Add(this.txtKanryoNo);
            resources.ApplyResources(this.lblKanryoNoPrefix, "lblKanryoNoPrefix");
            this.lblKanryoNoPrefix.IsFocusChangeColor = false;
            this.lblKanryoNoPrefix.LabelWidth = 20;
            this.lblKanryoNoPrefix.Name = "lblKanryoNoPrefix";
            this.lblKanryoNoPrefix.NormalBackColor = System.Drawing.Color.LightSkyBlue;
            this.lblKanryoNoPrefix.SplitterWidth = 0;
            // 
            // txtKanryoNo
            // 
            this.txtKanryoNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            resources.ApplyResources(this.txtKanryoNo, "txtKanryoNo");
            this.txtKanryoNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtKanryoNo.InputRegulation = "nab";
            this.txtKanryoNo.IsInputRegulation = true;
            this.txtKanryoNo.Name = "txtKanryoNo";
            this.txtKanryoNo.OneLineMaxLength = 5;
            this.txtKanryoNo.ProhibitionChar = null;
            this.txtKanryoNo.Leave += new System.EventHandler(this.txtKanryoNo_Leave);
            // 
            // btnDisp
            // 
            resources.ApplyResources(this.btnDisp, "btnDisp");
            this.btnDisp.Name = "btnDisp";
            this.btnDisp.Click += new System.EventHandler(this.btnDisp_Click);
            // 
            // grpSearch
            // 
            this.grpSearch.Controls.Add(this.lblKanryoTani);
            this.grpSearch.Controls.Add(this.lblKanryoNo);
            this.grpSearch.Controls.Add(this.btnDisp);
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
            // 
            // lblKanryoTani
            // 
            // 
            // lblKanryoTani.ChildPanel
            // 
            this.lblKanryoTani.ChildPanel.Controls.Add(this.cboKanryoTani);
            this.lblKanryoTani.IsFocusChangeColor = false;
            this.lblKanryoTani.IsNecessary = true;
            this.lblKanryoTani.LabelWidth = 80;
            resources.ApplyResources(this.lblKanryoTani, "lblKanryoTani");
            this.lblKanryoTani.Name = "lblKanryoTani";
            this.lblKanryoTani.SplitterWidth = 0;
            // 
            // cboKanryoTani
            // 
            resources.ApplyResources(this.cboKanryoTani, "cboKanryoTani");
            this.cboKanryoTani.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboKanryoTani.Name = "cboKanryoTani";
            this.cboKanryoTani.SelectedIndexChanged += new System.EventHandler(this.cboKanryoTani_SelectedIndexChanged);
            // 
            // lblKanryoDate
            // 
            // 
            // lblKanryoDate.ChildPanel
            // 
            this.lblKanryoDate.ChildPanel.Controls.Add(this.dtpKanryoDate);
            this.lblKanryoDate.IsFocusChangeColor = false;
            this.lblKanryoDate.LabelWidth = 80;
            resources.ApplyResources(this.lblKanryoDate, "lblKanryoDate");
            this.lblKanryoDate.Name = "lblKanryoDate";
            this.lblKanryoDate.SplitterWidth = 0;
            // 
            // dtpKanryoDate
            // 
            resources.ApplyResources(this.dtpKanryoDate, "dtpKanryoDate");
            this.dtpKanryoDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpKanryoDate.Name = "dtpKanryoDate";
            // 
            // ShukkoSettei
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ShukkoSettei";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).EndInit();
            this.lblKanryoNo.ChildPanel.ResumeLayout(false);
            this.lblKanryoNo.ResumeLayout(false);
            this.lblKanryoNoPrefix.ChildPanel.ResumeLayout(false);
            this.lblKanryoNoPrefix.ChildPanel.PerformLayout();
            this.lblKanryoNoPrefix.ResumeLayout(false);
            this.grpSearch.ResumeLayout(false);
            this.lblKanryoTani.ChildPanel.ResumeLayout(false);
            this.lblKanryoTani.ResumeLayout(false);
            this.lblKanryoDate.ChildPanel.ResumeLayout(false);
            this.lblKanryoDate.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DSWControl.DSWLabel.DSWLabel lblKanryoNo;
        private DSWControl.DSWLabel.DSWLabel lblKanryoNoPrefix;
        private DSWControl.DSWTextBox.DSWTextBox txtKanryoNo;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnDisp;
        private GrapeCity.Win.ElTabelle.Sheet shtMeisai;
        private System.Windows.Forms.GroupBox grpSearch;
        private DSWControl.DSWLabel.DSWLabel lblKanryoTani;
        private DSWControl.DSWComboBox.DSWComboBox cboKanryoTani;
        private DSWControl.DSWLabel.DSWLabel lblKanryoDate;
        private System.Windows.Forms.DateTimePicker dtpKanryoDate;
        private DSWControl.DSWLabel.CustomPanel ChildPanel;
    }
}