namespace SMS.S01.Forms
{
    partial class NisugataHakko
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NisugataHakko));
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.slblSortOrder = new DSWControl.DSWLabel.DSWLabel();
            this.scboSortOrder = new DSWControl.DSWComboBox.DSWComboBox();
            this.slblShipFrom = new DSWControl.DSWLabel.DSWLabel();
            this.scboShipFrom = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblShukkaDate = new DSWControl.DSWLabel.DSWLabel();
            this.dtpShukkaDateFrom = new DSWControl.DSWDateTimePicker();
            this.btnStart = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.grpMode = new System.Windows.Forms.GroupBox();
            this.flpMode = new System.Windows.Forms.FlowLayoutPanel();
            this.rdoInsert = new System.Windows.Forms.RadioButton();
            this.rdoDelete = new System.Windows.Forms.RadioButton();
            this.rdoView = new System.Windows.Forms.RadioButton();
            this.lblMailTitle = new DSWControl.DSWLabel.DSWLabel();
            this.cboMailTitle = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblRev = new DSWControl.DSWLabel.DSWLabel();
            this.txtRev = new DSWControl.DSWTextBox.DSWTextBox();
            this.shtNisugata = new GrapeCity.Win.ElTabelle.Sheet();
            this.grpKeyAction = new System.Windows.Forms.GroupBox();
            this.flpKeyAction = new System.Windows.Forms.FlowLayoutPanel();
            this.rdoRight = new System.Windows.Forms.RadioButton();
            this.rdoDown = new System.Windows.Forms.RadioButton();
            this.pnlMain.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.slblSortOrder.ChildPanel.SuspendLayout();
            this.slblSortOrder.SuspendLayout();
            this.slblShipFrom.ChildPanel.SuspendLayout();
            this.slblShipFrom.SuspendLayout();
            this.lblShukkaDate.ChildPanel.SuspendLayout();
            this.lblShukkaDate.SuspendLayout();
            this.grpMode.SuspendLayout();
            this.flpMode.SuspendLayout();
            this.lblMailTitle.ChildPanel.SuspendLayout();
            this.lblMailTitle.SuspendLayout();
            this.lblRev.ChildPanel.SuspendLayout();
            this.lblRev.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtNisugata)).BeginInit();
            this.grpKeyAction.SuspendLayout();
            this.flpKeyAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.grpKeyAction);
            this.pnlMain.Controls.Add(this.grpSearch);
            this.pnlMain.Controls.Add(this.shtNisugata);
            this.pnlMain.Controls.Add(this.lblMailTitle);
            this.pnlMain.Controls.Add(this.lblRev);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.SetChildIndex(this.lblRev, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblMailTitle, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtNisugata, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpKeyAction, 0);
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
            this.grpSearch.Controls.Add(this.slblSortOrder);
            this.grpSearch.Controls.Add(this.slblShipFrom);
            this.grpSearch.Controls.Add(this.lblShukkaDate);
            this.grpSearch.Controls.Add(this.btnStart);
            this.grpSearch.Controls.Add(this.grpMode);
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
            // 
            // slblSortOrder
            // 
            // 
            // slblSortOrder.ChildPanel
            // 
            this.slblSortOrder.ChildPanel.Controls.Add(this.scboSortOrder);
            this.slblSortOrder.IsFocusChangeColor = false;
            this.slblSortOrder.IsNecessary = true;
            this.slblSortOrder.LabelWidth = 80;
            resources.ApplyResources(this.slblSortOrder, "slblSortOrder");
            this.slblSortOrder.Name = "slblSortOrder";
            this.slblSortOrder.SplitterWidth = 0;
            // 
            // scboSortOrder
            // 
            resources.ApplyResources(this.scboSortOrder, "scboSortOrder");
            this.scboSortOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.scboSortOrder.Name = "scboSortOrder";
            // 
            // slblShipFrom
            // 
            // 
            // slblShipFrom.ChildPanel
            // 
            this.slblShipFrom.ChildPanel.Controls.Add(this.scboShipFrom);
            this.slblShipFrom.IsFocusChangeColor = false;
            this.slblShipFrom.IsNecessary = true;
            this.slblShipFrom.LabelWidth = 80;
            resources.ApplyResources(this.slblShipFrom, "slblShipFrom");
            this.slblShipFrom.Name = "slblShipFrom";
            this.slblShipFrom.SplitterWidth = 0;
            // 
            // scboShipFrom
            // 
            resources.ApplyResources(this.scboShipFrom, "scboShipFrom");
            this.scboShipFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.scboShipFrom.Name = "scboShipFrom";
            // 
            // lblShukkaDate
            // 
            // 
            // lblShukkaDate.ChildPanel
            // 
            this.lblShukkaDate.ChildPanel.Controls.Add(this.dtpShukkaDateFrom);
            this.lblShukkaDate.IsFocusChangeColor = false;
            this.lblShukkaDate.IsNecessary = true;
            this.lblShukkaDate.LabelWidth = 80;
            resources.ApplyResources(this.lblShukkaDate, "lblShukkaDate");
            this.lblShukkaDate.Name = "lblShukkaDate";
            this.lblShukkaDate.SplitterWidth = 0;
            // 
            // dtpShukkaDateFrom
            // 
            resources.ApplyResources(this.dtpShukkaDateFrom, "dtpShukkaDateFrom");
            this.dtpShukkaDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpShukkaDateFrom.Name = "dtpShukkaDateFrom";
            this.dtpShukkaDateFrom.NullValue = "";
            this.dtpShukkaDateFrom.Value = new System.DateTime(2010, 8, 12, 19, 42, 26, 0);
            // 
            // btnStart
            // 
            resources.ApplyResources(this.btnStart, "btnStart");
            this.btnStart.Name = "btnStart";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // grpMode
            // 
            resources.ApplyResources(this.grpMode, "grpMode");
            this.grpMode.Controls.Add(this.flpMode);
            this.grpMode.Name = "grpMode";
            this.grpMode.TabStop = false;
            // 
            // flpMode
            // 
            resources.ApplyResources(this.flpMode, "flpMode");
            this.flpMode.Controls.Add(this.rdoInsert);
            this.flpMode.Controls.Add(this.rdoDelete);
            this.flpMode.Controls.Add(this.rdoView);
            this.flpMode.Name = "flpMode";
            // 
            // rdoInsert
            // 
            resources.ApplyResources(this.rdoInsert, "rdoInsert");
            this.rdoInsert.Checked = true;
            this.rdoInsert.Name = "rdoInsert";
            this.rdoInsert.TabStop = true;
            this.rdoInsert.UseVisualStyleBackColor = true;
            this.rdoInsert.CheckedChanged += new System.EventHandler(this.rdoInsert_CheckedChanged);
            // 
            // rdoDelete
            // 
            resources.ApplyResources(this.rdoDelete, "rdoDelete");
            this.rdoDelete.Name = "rdoDelete";
            this.rdoDelete.UseVisualStyleBackColor = true;
            this.rdoDelete.CheckedChanged += new System.EventHandler(this.rdoDelete_CheckedChanged);
            // 
            // rdoView
            // 
            resources.ApplyResources(this.rdoView, "rdoView");
            this.rdoView.Name = "rdoView";
            this.rdoView.UseVisualStyleBackColor = true;
            this.rdoView.CheckedChanged += new System.EventHandler(this.rdoView_CheckedChanged);
            // 
            // lblMailTitle
            // 
            resources.ApplyResources(this.lblMailTitle, "lblMailTitle");
            // 
            // lblMailTitle.ChildPanel
            // 
            this.lblMailTitle.ChildPanel.Controls.Add(this.cboMailTitle);
            this.lblMailTitle.IsFocusChangeColor = false;
            this.lblMailTitle.IsNecessary = true;
            this.lblMailTitle.LabelWidth = 80;
            this.lblMailTitle.Name = "lblMailTitle";
            this.lblMailTitle.SplitterWidth = 0;
            // 
            // cboMailTitle
            // 
            resources.ApplyResources(this.cboMailTitle, "cboMailTitle");
            this.cboMailTitle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMailTitle.Name = "cboMailTitle";
            // 
            // lblRev
            // 
            resources.ApplyResources(this.lblRev, "lblRev");
            // 
            // lblRev.ChildPanel
            // 
            this.lblRev.ChildPanel.Controls.Add(this.txtRev);
            this.lblRev.IsFocusChangeColor = false;
            this.lblRev.IsNecessary = true;
            this.lblRev.LabelWidth = 55;
            this.lblRev.Name = "lblRev";
            this.lblRev.SplitterWidth = 0;
            // 
            // txtRev
            // 
            resources.ApplyResources(this.txtRev, "txtRev");
            this.txtRev.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtRev.InputRegulation = "n";
            this.txtRev.IsInputRegulation = true;
            this.txtRev.MaxByteLengthMode = true;
            this.txtRev.Name = "txtRev";
            this.txtRev.OneLineMaxLength = 3;
            this.txtRev.ProhibitionChar = null;
            // 
            // shtNisugata
            // 
            resources.ApplyResources(this.shtNisugata, "shtNisugata");
            this.shtNisugata.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtNisugata.Data")));
            this.shtNisugata.Name = "shtNisugata";
            this.shtNisugata.CellNotify += new GrapeCity.Win.ElTabelle.CellNotifyEventHandler(this.shtNisugata_CellNotify);
            this.shtNisugata.RowInserted += new GrapeCity.Win.ElTabelle.RowInsertedEventHandler(this.shtNisugata_RowInserted);
            this.shtNisugata.Leave += new System.EventHandler(this.shtNisugata_Leave);
            this.shtNisugata.RowFilling += new GrapeCity.Win.ElTabelle.RowFillingEventHandler(this.shtNisugata_RowFilling);
            this.shtNisugata.BindingError += new System.EventHandler<GrapeCity.Win.ElTabelle.BindingErrorEventArgs>(this.shtNisugata_BindingError);
            this.shtNisugata.LeaveCell += new GrapeCity.Win.ElTabelle.LeaveCellEventHandler(this.shtNisugata_LeaveCell);
            this.shtNisugata.ValueChanged += new GrapeCity.Win.ElTabelle.ValueChangedEventHandler(this.shtNisugata_ValueChanged);
            // 
            // grpKeyAction
            // 
            this.grpKeyAction.Controls.Add(this.flpKeyAction);
            resources.ApplyResources(this.grpKeyAction, "grpKeyAction");
            this.grpKeyAction.Name = "grpKeyAction";
            this.grpKeyAction.TabStop = false;
            // 
            // flpKeyAction
            // 
            resources.ApplyResources(this.flpKeyAction, "flpKeyAction");
            this.flpKeyAction.Controls.Add(this.rdoRight);
            this.flpKeyAction.Controls.Add(this.rdoDown);
            this.flpKeyAction.Name = "flpKeyAction";
            // 
            // rdoRight
            // 
            resources.ApplyResources(this.rdoRight, "rdoRight");
            this.rdoRight.Checked = true;
            this.rdoRight.Name = "rdoRight";
            this.rdoRight.TabStop = true;
            this.rdoRight.UseVisualStyleBackColor = true;
            this.rdoRight.CheckedChanged += new System.EventHandler(this.rdoKeyAction_CheckedChanged);
            // 
            // rdoDown
            // 
            resources.ApplyResources(this.rdoDown, "rdoDown");
            this.rdoDown.Name = "rdoDown";
            this.rdoDown.UseVisualStyleBackColor = true;
            this.rdoDown.CheckedChanged += new System.EventHandler(this.rdoKeyAction_CheckedChanged);
            // 
            // NisugataHakko
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "NisugataHakko";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.grpSearch.ResumeLayout(false);
            this.grpSearch.PerformLayout();
            this.slblSortOrder.ChildPanel.ResumeLayout(false);
            this.slblSortOrder.ResumeLayout(false);
            this.slblShipFrom.ChildPanel.ResumeLayout(false);
            this.slblShipFrom.ResumeLayout(false);
            this.lblShukkaDate.ChildPanel.ResumeLayout(false);
            this.lblShukkaDate.ResumeLayout(false);
            this.grpMode.ResumeLayout(false);
            this.grpMode.PerformLayout();
            this.flpMode.ResumeLayout(false);
            this.flpMode.PerformLayout();
            this.lblMailTitle.ChildPanel.ResumeLayout(false);
            this.lblMailTitle.ResumeLayout(false);
            this.lblRev.ChildPanel.ResumeLayout(false);
            this.lblRev.ChildPanel.PerformLayout();
            this.lblRev.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtNisugata)).EndInit();
            this.grpKeyAction.ResumeLayout(false);
            this.grpKeyAction.PerformLayout();
            this.flpKeyAction.ResumeLayout(false);
            this.flpKeyAction.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSearch;
        private DSWControl.DSWLabel.DSWLabel lblMailTitle;
        private DSWControl.DSWLabel.DSWLabel lblRev;
        private System.Windows.Forms.GroupBox grpMode;
        private System.Windows.Forms.RadioButton rdoView;
        private System.Windows.Forms.RadioButton rdoInsert;
        private System.Windows.Forms.RadioButton rdoDelete;
        private DSWControl.DSWComboBox.DSWComboBox cboMailTitle;
        private DSWControl.DSWTextBox.DSWTextBox txtRev;
        private GrapeCity.Win.ElTabelle.Sheet shtNisugata;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnStart;
        private DSWControl.DSWLabel.DSWLabel lblShukkaDate;
        private DSWControl.DSWDateTimePicker dtpShukkaDateFrom;
        private System.Windows.Forms.FlowLayoutPanel flpMode;
        private System.Windows.Forms.GroupBox grpKeyAction;
        private System.Windows.Forms.FlowLayoutPanel flpKeyAction;
        private System.Windows.Forms.RadioButton rdoRight;
        private System.Windows.Forms.RadioButton rdoDown;
        private DSWControl.DSWLabel.DSWLabel slblShipFrom;
        private DSWControl.DSWComboBox.DSWComboBox scboShipFrom;
        private DSWControl.DSWLabel.DSWLabel slblSortOrder;
        private DSWControl.DSWComboBox.DSWComboBox scboSortOrder;
    }
}