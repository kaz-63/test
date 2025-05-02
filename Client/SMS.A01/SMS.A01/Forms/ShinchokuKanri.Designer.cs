namespace SMS.A01.Forms
{
    partial class ShinchokuKanri
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShinchokuKanri));
            this.lblJyokyoFlag = new DSWControl.DSWLabel.DSWLabel();
            this.cboJyokyoFlag = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblCount = new DSWControl.DSWLabel.DSWLabel();
            this.txtCount = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblNonyusakiName = new DSWControl.DSWLabel.DSWLabel();
            this.txtNonyusakiName = new DSWControl.DSWTextBox.DSWTextBox();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.tlpSearch = new System.Windows.Forms.TableLayoutPanel();
            this.lblKishu = new DSWControl.DSWLabel.DSWLabel();
            this.txtKishu = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnKishu = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblArno = new DSWControl.DSWLabel.DSWLabel();
            this.txtArno = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnView = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblGoki = new DSWControl.DSWLabel.DSWLabel();
            this.txtGoki = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnGoki = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblKikan = new DSWControl.DSWLabel.DSWLabel();
            this.tlpKikan = new System.Windows.Forms.TableLayoutPanel();
            this.dtpKikanTo = new DSWControl.DSWDateTimePicker();
            this.cboKikan = new DSWControl.DSWComboBox.DSWComboBox();
            this.labelRange = new System.Windows.Forms.Label();
            this.dtpKikanFrom = new DSWControl.DSWDateTimePicker();
            this.contextMenuGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemDateSite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDateJapan = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDateLocal = new System.Windows.Forms.ToolStripMenuItem();
            this.lblDisplay = new DSWControl.DSWLabel.DSWLabel();
            this.cboDisplay = new DSWControl.DSWComboBox.DSWComboBox();
            this.tlpTitle = new System.Windows.Forms.TableLayoutPanel();
            this.shtSintyoku = new GrapeCity.Win.ElTabelle.Sheet();
            this.pnlMain.SuspendLayout();
            this.lblJyokyoFlag.ChildPanel.SuspendLayout();
            this.lblJyokyoFlag.SuspendLayout();
            this.lblCount.ChildPanel.SuspendLayout();
            this.lblCount.SuspendLayout();
            this.lblNonyusakiName.ChildPanel.SuspendLayout();
            this.lblNonyusakiName.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.tlpSearch.SuspendLayout();
            this.lblKishu.ChildPanel.SuspendLayout();
            this.lblKishu.SuspendLayout();
            this.lblArno.ChildPanel.SuspendLayout();
            this.lblArno.SuspendLayout();
            this.lblGoki.ChildPanel.SuspendLayout();
            this.lblGoki.SuspendLayout();
            this.lblKikan.ChildPanel.SuspendLayout();
            this.lblKikan.SuspendLayout();
            this.tlpKikan.SuspendLayout();
            this.contextMenuGrid.SuspendLayout();
            this.lblDisplay.ChildPanel.SuspendLayout();
            this.lblDisplay.SuspendLayout();
            this.tlpTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtSintyoku)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.shtSintyoku);
            this.pnlMain.Controls.Add(this.tlpTitle);
            this.pnlMain.Controls.Add(this.grpSearch);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
            this.pnlMain.Controls.SetChildIndex(this.tlpTitle, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtSintyoku, 0);
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
            // fbrFunction.F02Button
            // 
            resources.ApplyResources(this.fbrFunction.F02Button, "fbrFunction.F02Button");
            // 
            // fbrFunction.F10Button
            // 
            resources.ApplyResources(this.fbrFunction.F10Button, "fbrFunction.F10Button");
            // 
            // fbrFunction.F11Button
            // 
            resources.ApplyResources(this.fbrFunction.F11Button, "fbrFunction.F11Button");
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
            this.lblCorner.ImageKey = global::SMS.A01.Properties.Resources.ARJoho_ElTabelleColumnEdit;
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // lblJyokyoFlag
            // 
            // 
            // lblJyokyoFlag.ChildPanel
            // 
            this.lblJyokyoFlag.ChildPanel.Controls.Add(this.cboJyokyoFlag);
            this.tlpSearch.SetColumnSpan(this.lblJyokyoFlag, 2);
            this.lblJyokyoFlag.IsFocusChangeColor = false;
            this.lblJyokyoFlag.IsNecessary = true;
            this.lblJyokyoFlag.LabelWidth = 100;
            resources.ApplyResources(this.lblJyokyoFlag, "lblJyokyoFlag");
            this.lblJyokyoFlag.Name = "lblJyokyoFlag";
            this.lblJyokyoFlag.SplitterWidth = 0;
            // 
            // cboJyokyoFlag
            // 
            resources.ApplyResources(this.cboJyokyoFlag, "cboJyokyoFlag");
            this.cboJyokyoFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboJyokyoFlag.Name = "cboJyokyoFlag";
            // 
            // lblCount
            // 
            // 
            // lblCount.ChildPanel
            // 
            this.lblCount.ChildPanel.Controls.Add(this.txtCount);
            this.lblCount.IsFocusChangeColor = false;
            this.lblCount.LabelWidth = 80;
            resources.ApplyResources(this.lblCount, "lblCount");
            this.lblCount.Name = "lblCount";
            this.lblCount.SplitterWidth = 0;
            this.lblCount.TabStop = false;
            // 
            // txtCount
            // 
            this.txtCount.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtCount, "txtCount");
            this.txtCount.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtCount.InputRegulation = "";
            this.txtCount.Name = "txtCount";
            this.txtCount.ProhibitionChar = null;
            this.txtCount.ReadOnly = true;
            this.txtCount.TabStop = false;
            // 
            // lblNonyusakiName
            // 
            // 
            // lblNonyusakiName.ChildPanel
            // 
            this.lblNonyusakiName.ChildPanel.Controls.Add(this.txtNonyusakiName);
            resources.ApplyResources(this.lblNonyusakiName, "lblNonyusakiName");
            this.lblNonyusakiName.IsFocusChangeColor = false;
            this.lblNonyusakiName.IsNecessary = true;
            this.lblNonyusakiName.LabelWidth = 100;
            this.lblNonyusakiName.Name = "lblNonyusakiName";
            this.lblNonyusakiName.SplitterWidth = 0;
            // 
            // txtNonyusakiName
            // 
            this.txtNonyusakiName.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtNonyusakiName, "txtNonyusakiName");
            this.txtNonyusakiName.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtNonyusakiName.InputRegulation = global::SMS.A01.Properties.Resources.ARJoho_ElTabelleColumnEdit;
            this.txtNonyusakiName.Name = "txtNonyusakiName";
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
            this.tlpSearch.Controls.Add(this.lblJyokyoFlag, 0, 0);
            this.tlpSearch.Controls.Add(this.lblKishu, 1, 1);
            this.tlpSearch.Controls.Add(this.lblArno, 0, 1);
            this.tlpSearch.Controls.Add(this.btnView, 2, 2);
            this.tlpSearch.Controls.Add(this.lblGoki, 2, 1);
            this.tlpSearch.Controls.Add(this.lblKikan, 0, 2);
            this.tlpSearch.MinimumSize = new System.Drawing.Size(600, 0);
            this.tlpSearch.Name = "tlpSearch";
            // 
            // lblKishu
            // 
            // 
            // lblKishu.ChildPanel
            // 
            this.lblKishu.ChildPanel.Controls.Add(this.txtKishu);
            this.lblKishu.ChildPanel.Controls.Add(this.btnKishu);
            resources.ApplyResources(this.lblKishu, "lblKishu");
            this.lblKishu.IsFocusChangeColor = false;
            this.lblKishu.LabelWidth = 80;
            this.lblKishu.Name = "lblKishu";
            this.lblKishu.SplitterWidth = 0;
            // 
            // txtKishu
            // 
            this.txtKishu.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.txtKishu, "txtKishu");
            this.txtKishu.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtKishu.InputRegulation = "F";
            this.txtKishu.Name = "txtKishu";
            this.txtKishu.ProhibitionChar = null;
            // 
            // btnKishu
            // 
            resources.ApplyResources(this.btnKishu, "btnKishu");
            this.btnKishu.Name = "btnKishu";
            this.btnKishu.Click += new System.EventHandler(this.btnKishu_Click);
            // 
            // lblArno
            // 
            // 
            // lblArno.ChildPanel
            // 
            this.lblArno.ChildPanel.Controls.Add(this.txtArno);
            this.lblArno.IsFocusChangeColor = false;
            this.lblArno.LabelWidth = 100;
            resources.ApplyResources(this.lblArno, "lblArno");
            this.lblArno.Name = "lblArno";
            this.lblArno.SplitterWidth = 0;
            // 
            // txtArno
            // 
            this.txtArno.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.txtArno, "txtArno");
            this.txtArno.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtArno.InputRegulation = "nl";
            this.txtArno.IsInputRegulation = true;
            this.txtArno.Name = "txtArno";
            this.txtArno.OneLineMaxLength = 9;
            this.txtArno.ProhibitionChar = null;
            // 
            // btnView
            // 
            resources.ApplyResources(this.btnView, "btnView");
            this.btnView.Name = "btnView";
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // lblGoki
            // 
            // 
            // lblGoki.ChildPanel
            // 
            this.lblGoki.ChildPanel.Controls.Add(this.txtGoki);
            this.lblGoki.ChildPanel.Controls.Add(this.btnGoki);
            resources.ApplyResources(this.lblGoki, "lblGoki");
            this.lblGoki.IsFocusChangeColor = false;
            this.lblGoki.LabelWidth = 80;
            this.lblGoki.Name = "lblGoki";
            this.lblGoki.SplitterWidth = 0;
            // 
            // txtGoki
            // 
            this.txtGoki.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.txtGoki, "txtGoki");
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
            // lblKikan
            // 
            // 
            // lblKikan.ChildPanel
            // 
            this.lblKikan.ChildPanel.Controls.Add(this.tlpKikan);
            this.tlpSearch.SetColumnSpan(this.lblKikan, 2);
            this.lblKikan.IsFocusChangeColor = false;
            this.lblKikan.LabelWidth = 100;
            resources.ApplyResources(this.lblKikan, "lblKikan");
            this.lblKikan.Name = "lblKikan";
            this.lblKikan.SplitterWidth = 0;
            // 
            // tlpKikan
            // 
            resources.ApplyResources(this.tlpKikan, "tlpKikan");
            this.tlpKikan.Controls.Add(this.dtpKikanTo, 6, 0);
            this.tlpKikan.Controls.Add(this.cboKikan, 0, 0);
            this.tlpKikan.Controls.Add(this.labelRange, 4, 0);
            this.tlpKikan.Controls.Add(this.dtpKikanFrom, 2, 0);
            this.tlpKikan.Name = "tlpKikan";
            // 
            // dtpKikanTo
            // 
            resources.ApplyResources(this.dtpKikanTo, "dtpKikanTo");
            this.dtpKikanTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpKikanTo.Name = "dtpKikanTo";
            this.dtpKikanTo.NullValue = "";
            this.dtpKikanTo.Value = null;
            // 
            // cboKikan
            // 
            resources.ApplyResources(this.cboKikan, "cboKikan");
            this.cboKikan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboKikan.Name = "cboKikan";
            // 
            // labelRange
            // 
            resources.ApplyResources(this.labelRange, "labelRange");
            this.labelRange.Name = "labelRange";
            // 
            // dtpKikanFrom
            // 
            resources.ApplyResources(this.dtpKikanFrom, "dtpKikanFrom");
            this.dtpKikanFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpKikanFrom.Name = "dtpKikanFrom";
            this.dtpKikanFrom.NullValue = "";
            this.dtpKikanFrom.Value = null;
            // 
            // contextMenuGrid
            // 
            this.contextMenuGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemDateSite,
            this.toolStripMenuItemDateJapan,
            this.toolStripMenuItemDateLocal});
            this.contextMenuGrid.Name = "contextMenuGrid";
            resources.ApplyResources(this.contextMenuGrid, "contextMenuGrid");
            this.contextMenuGrid.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuGrid_Opening);
            // 
            // toolStripMenuItemDateSite
            // 
            this.toolStripMenuItemDateSite.Name = "toolStripMenuItemDateSite";
            resources.ApplyResources(this.toolStripMenuItemDateSite, "toolStripMenuItemDateSite");
            this.toolStripMenuItemDateSite.Click += new System.EventHandler(this.toolStripMenuItemDateSite_Click);
            // 
            // toolStripMenuItemDateJapan
            // 
            this.toolStripMenuItemDateJapan.Name = "toolStripMenuItemDateJapan";
            resources.ApplyResources(this.toolStripMenuItemDateJapan, "toolStripMenuItemDateJapan");
            this.toolStripMenuItemDateJapan.Click += new System.EventHandler(this.toolStripMenuItemDateJapan_Click);
            // 
            // toolStripMenuItemDateLocal
            // 
            this.toolStripMenuItemDateLocal.Name = "toolStripMenuItemDateLocal";
            resources.ApplyResources(this.toolStripMenuItemDateLocal, "toolStripMenuItemDateLocal");
            this.toolStripMenuItemDateLocal.Click += new System.EventHandler(this.toolStripMenuItemDateLocal_Click);
            // 
            // lblDisplay
            // 
            // 
            // lblDisplay.ChildPanel
            // 
            this.lblDisplay.ChildPanel.Controls.Add(this.cboDisplay);
            this.lblDisplay.IsFocusChangeColor = false;
            this.lblDisplay.IsNecessary = true;
            this.lblDisplay.LabelWidth = 80;
            resources.ApplyResources(this.lblDisplay, "lblDisplay");
            this.lblDisplay.Name = "lblDisplay";
            this.lblDisplay.SplitterWidth = 0;
            // 
            // cboDisplay
            // 
            resources.ApplyResources(this.cboDisplay, "cboDisplay");
            this.cboDisplay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDisplay.Name = "cboDisplay";
            this.cboDisplay.SelectedIndexChanged += new System.EventHandler(this.cboDisplay_SelectedIndexChanged);
            // 
            // tlpTitle
            // 
            resources.ApplyResources(this.tlpTitle, "tlpTitle");
            this.tlpTitle.Controls.Add(this.lblCount, 3, 0);
            this.tlpTitle.Controls.Add(this.lblDisplay, 1, 0);
            this.tlpTitle.Controls.Add(this.lblNonyusakiName, 0, 0);
            this.tlpTitle.MinimumSize = new System.Drawing.Size(600, 0);
            this.tlpTitle.Name = "tlpTitle";
            // 
            // shtSintyoku
            // 
            resources.ApplyResources(this.shtSintyoku, "shtSintyoku");
            this.shtSintyoku.ContextMenuStrip = this.contextMenuGrid;
            this.shtSintyoku.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtSintyoku.Data")));
            this.shtSintyoku.Name = "shtSintyoku";
            // 
            // ShinchokuKanri
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ShinchokuKanri";
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlMain, 0);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.lblJyokyoFlag.ChildPanel.ResumeLayout(false);
            this.lblJyokyoFlag.ResumeLayout(false);
            this.lblCount.ChildPanel.ResumeLayout(false);
            this.lblCount.ChildPanel.PerformLayout();
            this.lblCount.ResumeLayout(false);
            this.lblNonyusakiName.ChildPanel.ResumeLayout(false);
            this.lblNonyusakiName.ChildPanel.PerformLayout();
            this.lblNonyusakiName.ResumeLayout(false);
            this.grpSearch.ResumeLayout(false);
            this.tlpSearch.ResumeLayout(false);
            this.lblKishu.ChildPanel.ResumeLayout(false);
            this.lblKishu.ChildPanel.PerformLayout();
            this.lblKishu.ResumeLayout(false);
            this.lblArno.ChildPanel.ResumeLayout(false);
            this.lblArno.ChildPanel.PerformLayout();
            this.lblArno.ResumeLayout(false);
            this.lblGoki.ChildPanel.ResumeLayout(false);
            this.lblGoki.ChildPanel.PerformLayout();
            this.lblGoki.ResumeLayout(false);
            this.lblKikan.ChildPanel.ResumeLayout(false);
            this.lblKikan.ResumeLayout(false);
            this.tlpKikan.ResumeLayout(false);
            this.contextMenuGrid.ResumeLayout(false);
            this.lblDisplay.ChildPanel.ResumeLayout(false);
            this.lblDisplay.ResumeLayout(false);
            this.tlpTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtSintyoku)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSearch;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnView;
        private DSWControl.DSWLabel.DSWLabel lblCount;
        private DSWControl.DSWTextBox.DSWTextBox txtCount;
        private DSWControl.DSWLabel.DSWLabel lblNonyusakiName;
        private DSWControl.DSWTextBox.DSWTextBox txtNonyusakiName;
        private DSWControl.DSWLabel.DSWLabel lblJyokyoFlag;
        private DSWControl.DSWComboBox.DSWComboBox cboJyokyoFlag;
        private System.Windows.Forms.TableLayoutPanel tlpSearch;
        private DSWControl.DSWLabel.DSWLabel lblKishu;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnKishu;
        private DSWControl.DSWLabel.DSWLabel lblGoki;
        private DSWControl.DSWTextBox.DSWTextBox txtGoki;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnGoki;
        private DSWControl.DSWLabel.DSWLabel lblArno;
        private DSWControl.DSWTextBox.DSWTextBox txtArno;
        private DSWControl.DSWComboBox.DSWComboBox cboKikan;
        private DSWControl.DSWLabel.DSWLabel lblKikan;
        private DSWControl.DSWDateTimePicker dtpKikanTo;
        private System.Windows.Forms.Label labelRange;
        private DSWControl.DSWDateTimePicker dtpKikanFrom;
        private DSWControl.DSWTextBox.DSWTextBox txtKishu;
        private DSWControl.DSWLabel.DSWLabel lblDisplay;
        private DSWControl.DSWComboBox.DSWComboBox cboDisplay;
        private System.Windows.Forms.TableLayoutPanel tlpTitle;
        private System.Windows.Forms.ContextMenuStrip contextMenuGrid;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDateSite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDateJapan;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDateLocal;
        private GrapeCity.Win.ElTabelle.Sheet shtSintyoku;
        private System.Windows.Forms.TableLayoutPanel tlpKikan;
    }
}