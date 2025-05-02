namespace SMS.A01.Forms
{
    partial class ShinchokuKanriHenkoRireki 
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShinchokuKanriHenkoRireki));
            this.lblNonyusakiName = new DSWControl.DSWLabel.DSWLabel();
            this.txtNonyusakiName = new DSWControl.DSWTextBox.DSWTextBox();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.tlpSearch = new System.Windows.Forms.TableLayoutPanel();
            this.lblUpdateDate = new DSWControl.DSWLabel.DSWLabel();
            this.dtpUpdateDateTo = new DSWControl.DSWDateTimePicker();
            this.lblKara = new System.Windows.Forms.Label();
            this.dtpUpdateDateFrom = new DSWControl.DSWDateTimePicker();
            this.lblKishu = new DSWControl.DSWLabel.DSWLabel();
            this.txtKishu = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnKishu = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblARNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtARNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblGoki = new DSWControl.DSWLabel.DSWLabel();
            this.txtGoki = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnGoki = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblDate = new DSWControl.DSWLabel.DSWLabel();
            this.cboDatekubun = new DSWControl.DSWComboBox.DSWComboBox();
            this.btnSearch = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.shtResult = new GrapeCity.Win.ElTabelle.Sheet();
            this.pnlMain.SuspendLayout();
            this.lblNonyusakiName.ChildPanel.SuspendLayout();
            this.lblNonyusakiName.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.tlpSearch.SuspendLayout();
            this.lblUpdateDate.ChildPanel.SuspendLayout();
            this.lblUpdateDate.SuspendLayout();
            this.lblKishu.ChildPanel.SuspendLayout();
            this.lblKishu.SuspendLayout();
            this.lblARNo.ChildPanel.SuspendLayout();
            this.lblARNo.SuspendLayout();
            this.lblGoki.ChildPanel.SuspendLayout();
            this.lblGoki.SuspendLayout();
            this.lblDate.ChildPanel.SuspendLayout();
            this.lblDate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.shtResult);
            this.pnlMain.Controls.Add(this.grpSearch);
            this.pnlMain.Controls.Add(this.lblNonyusakiName);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.SetChildIndex(this.lblNonyusakiName, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtResult, 0);
            // 
            // fbrFunction
            // 
            resources.ApplyResources(this.fbrFunction, "fbrFunction");
            // 
            // lblCorner
            // 
            resources.ApplyResources(this.lblCorner, "lblCorner");
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // lblNonyusakiName
            // 
            // 
            // lblNonyusakiName.ChildPanel
            // 
            this.lblNonyusakiName.ChildPanel.Controls.Add(this.txtNonyusakiName);
            this.lblNonyusakiName.IsFocusChangeColor = false;
            this.lblNonyusakiName.LabelWidth = 80;
            resources.ApplyResources(this.lblNonyusakiName, "lblNonyusakiName");
            this.lblNonyusakiName.Name = "lblNonyusakiName";
            this.lblNonyusakiName.SplitterWidth = 0;
            // 
            // txtNonyusakiName
            // 
            this.txtNonyusakiName.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtNonyusakiName, "txtNonyusakiName");
            this.txtNonyusakiName.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtNonyusakiName.InputRegulation = "F";
            this.txtNonyusakiName.MaxByteLengthMode = true;
            this.txtNonyusakiName.Name = "txtNonyusakiName";
            this.txtNonyusakiName.OneLineMaxLength = 60;
            this.txtNonyusakiName.ProhibitionChar = null;
            this.txtNonyusakiName.ReadOnly = true;
            // 
            // grpSearch
            // 
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Controls.Add(this.tlpSearch);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
            // 
            // tlpSearch
            // 
            resources.ApplyResources(this.tlpSearch, "tlpSearch");
            this.tlpSearch.Controls.Add(this.lblARNo, 0, 0);
            this.tlpSearch.Controls.Add(this.btnSearch, 6, 1);
            this.tlpSearch.Controls.Add(this.lblGoki, 3, 1);
            this.tlpSearch.Controls.Add(this.lblKishu, 0, 1);
            this.tlpSearch.Controls.Add(this.lblUpdateDate, 4, 0);
            this.tlpSearch.Controls.Add(this.lblDate, 2, 0);
            this.tlpSearch.Name = "tlpSearch";
            // 
            // lblUpdateDate
            // 
            // 
            // lblUpdateDate.ChildPanel
            // 
            this.lblUpdateDate.ChildPanel.Controls.Add(this.dtpUpdateDateTo);
            this.lblUpdateDate.ChildPanel.Controls.Add(this.lblKara);
            this.lblUpdateDate.ChildPanel.Controls.Add(this.dtpUpdateDateFrom);
            this.tlpSearch.SetColumnSpan(this.lblUpdateDate, 3);
            this.lblUpdateDate.IsFocusChangeColor = false;
            this.lblUpdateDate.LabelWidth = 80;
            resources.ApplyResources(this.lblUpdateDate, "lblUpdateDate");
            this.lblUpdateDate.Name = "lblUpdateDate";
            this.lblUpdateDate.SplitterWidth = 0;
            // 
            // dtpUpdateDateTo
            // 
            resources.ApplyResources(this.dtpUpdateDateTo, "dtpUpdateDateTo");
            this.dtpUpdateDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpUpdateDateTo.Name = "dtpUpdateDateTo";
            this.dtpUpdateDateTo.NullValue = "";
            this.dtpUpdateDateTo.Value = new System.DateTime(2010, 8, 12, 19, 42, 26, 0);
            // 
            // lblKara
            // 
            resources.ApplyResources(this.lblKara, "lblKara");
            this.lblKara.Name = "lblKara";
            // 
            // dtpUpdateDateFrom
            // 
            resources.ApplyResources(this.dtpUpdateDateFrom, "dtpUpdateDateFrom");
            this.dtpUpdateDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpUpdateDateFrom.Name = "dtpUpdateDateFrom";
            this.dtpUpdateDateFrom.NullValue = "";
            this.dtpUpdateDateFrom.Value = new System.DateTime(2010, 8, 12, 19, 42, 26, 0);
            // 
            // lblKishu
            // 
            // 
            // lblKishu.ChildPanel
            // 
            this.lblKishu.ChildPanel.Controls.Add(this.txtKishu);
            this.lblKishu.ChildPanel.Controls.Add(this.btnKishu);
            this.tlpSearch.SetColumnSpan(this.lblKishu, 3);
            this.lblKishu.IsFocusChangeColor = false;
            this.lblKishu.LabelWidth = 80;
            resources.ApplyResources(this.lblKishu, "lblKishu");
            this.lblKishu.Name = "lblKishu";
            this.lblKishu.SplitterWidth = 0;
            // 
            // txtKishu
            // 
            resources.ApplyResources(this.txtKishu, "txtKishu");
            this.txtKishu.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtKishu.InputRegulation = "F";
            this.txtKishu.MaxByteLengthMode = true;
            this.txtKishu.Name = "txtKishu";
            this.txtKishu.ProhibitionChar = null;
            // 
            // btnKishu
            // 
            resources.ApplyResources(this.btnKishu, "btnKishu");
            this.btnKishu.Name = "btnKishu";
            this.btnKishu.Click += new System.EventHandler(this.btnKishu_Click);
            // 
            // lblARNo
            // 
            // 
            // lblARNo.ChildPanel
            // 
            this.lblARNo.ChildPanel.Controls.Add(this.txtARNo);
            this.tlpSearch.SetColumnSpan(this.lblARNo, 2);
            this.lblARNo.IsFocusChangeColor = false;
            this.lblARNo.LabelWidth = 80;
            resources.ApplyResources(this.lblARNo, "lblARNo");
            this.lblARNo.Name = "lblARNo";
            this.lblARNo.SplitterWidth = 0;
            // 
            // txtARNo
            // 
            resources.ApplyResources(this.txtARNo, "txtARNo");
            this.txtARNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtARNo.InputRegulation = "F";
            this.txtARNo.MaxByteLengthMode = true;
            this.txtARNo.Name = "txtARNo";
            this.txtARNo.OneLineMaxLength = 9;
            this.txtARNo.ProhibitionChar = null;
            // 
            // lblGoki
            // 
            // 
            // lblGoki.ChildPanel
            // 
            this.lblGoki.ChildPanel.Controls.Add(this.txtGoki);
            this.lblGoki.ChildPanel.Controls.Add(this.btnGoki);
            this.tlpSearch.SetColumnSpan(this.lblGoki, 3);
            this.lblGoki.IsFocusChangeColor = false;
            this.lblGoki.LabelWidth = 80;
            resources.ApplyResources(this.lblGoki, "lblGoki");
            this.lblGoki.Name = "lblGoki";
            this.lblGoki.SplitterWidth = 0;
            // 
            // txtGoki
            // 
            resources.ApplyResources(this.txtGoki, "txtGoki");
            this.txtGoki.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtGoki.InputRegulation = "F";
            this.txtGoki.MaxByteLengthMode = true;
            this.txtGoki.Name = "txtGoki";
            this.txtGoki.ProhibitionChar = null;
            // 
            // btnGoki
            // 
            resources.ApplyResources(this.btnGoki, "btnGoki");
            this.btnGoki.Name = "btnGoki";
            this.btnGoki.Click += new System.EventHandler(this.btnGoki_Click);
            // 
            // lblDate
            // 
            // 
            // lblDate.ChildPanel
            // 
            this.lblDate.ChildPanel.Controls.Add(this.cboDatekubun);
            this.tlpSearch.SetColumnSpan(this.lblDate, 2);
            this.lblDate.IsFocusChangeColor = false;
            this.lblDate.LabelWidth = 80;
            resources.ApplyResources(this.lblDate, "lblDate");
            this.lblDate.Name = "lblDate";
            this.lblDate.SplitterWidth = 0;
            // 
            // cboDatekubun
            // 
            this.cboDatekubun.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboDatekubun.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.cboDatekubun, "cboDatekubun");
            this.cboDatekubun.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDatekubun.Name = "cboDatekubun";
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
            // ShinchokuKanriHenkoRireki
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ShinchokuKanriHenkoRireki";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.lblNonyusakiName.ChildPanel.ResumeLayout(false);
            this.lblNonyusakiName.ChildPanel.PerformLayout();
            this.lblNonyusakiName.ResumeLayout(false);
            this.grpSearch.ResumeLayout(false);
            this.tlpSearch.ResumeLayout(false);
            this.lblUpdateDate.ChildPanel.ResumeLayout(false);
            this.lblUpdateDate.ResumeLayout(false);
            this.lblKishu.ChildPanel.ResumeLayout(false);
            this.lblKishu.ChildPanel.PerformLayout();
            this.lblKishu.ResumeLayout(false);
            this.lblARNo.ChildPanel.ResumeLayout(false);
            this.lblARNo.ChildPanel.PerformLayout();
            this.lblARNo.ResumeLayout(false);
            this.lblGoki.ChildPanel.ResumeLayout(false);
            this.lblGoki.ChildPanel.PerformLayout();
            this.lblGoki.ResumeLayout(false);
            this.lblDate.ChildPanel.ResumeLayout(false);
            this.lblDate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSearch;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnSearch;
        private DSWControl.DSWTextBox.DSWTextBox txtNonyusakiName;
        private DSWControl.DSWLabel.DSWLabel lblNonyusakiName;
        private DSWControl.DSWLabel.DSWLabel lblARNo;
        private DSWControl.DSWLabel.DSWLabel lblGoki;
        private DSWControl.DSWTextBox.DSWTextBox txtGoki;
        private DSWControl.DSWLabel.DSWLabel lblDate;
        private DSWControl.DSWComboBox.DSWComboBox cboDatekubun;
        private DSWControl.DSWTextBox.DSWTextBox txtARNo;
        private DSWControl.DSWLabel.DSWLabel lblKishu;
        private DSWControl.DSWTextBox.DSWTextBox txtKishu;
        private GrapeCity.Win.ElTabelle.Sheet shtResult;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnKishu;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnGoki;
        private DSWControl.DSWLabel.DSWLabel lblUpdateDate;
        private DSWControl.DSWDateTimePicker dtpUpdateDateTo;
        private System.Windows.Forms.Label lblKara;
        private DSWControl.DSWDateTimePicker dtpUpdateDateFrom;
        private System.Windows.Forms.TableLayoutPanel tlpSearch;
    }
}