namespace SMS.P02.Forms
{
    partial class RirekiShokai 
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RirekiShokai));
            this.lblNonyusakiName = new DSWControl.DSWLabel.DSWLabel();
            this.txtNonyusakiName = new DSWControl.DSWTextBox.DSWTextBox();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.btnListSelect = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblUpdateDate = new DSWControl.DSWLabel.DSWLabel();
            this.dtpUpdateDateTo = new DSWControl.DSWDateTimePicker();
            this.lblKara = new System.Windows.Forms.Label();
            this.dtpUpdateDateFrom = new DSWControl.DSWDateTimePicker();
            this.lblOperationFlag = new DSWControl.DSWLabel.DSWLabel();
            this.cboOperationFlag = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblUpdateUserName = new DSWControl.DSWLabel.DSWLabel();
            this.txtUpdateUserName = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblARNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtARNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblAR = new DSWControl.DSWControlLinkLabel.DSWControlLinkLabel();
            this.lblShip = new DSWControl.DSWLabel.DSWLabel();
            this.txtShip = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblShukkaFlag = new DSWControl.DSWLabel.DSWLabel();
            this.cboShukkaFlag = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblGamenFlag = new DSWControl.DSWLabel.DSWLabel();
            this.cboGamenFlag = new DSWControl.DSWComboBox.DSWComboBox();
            this.btnSearch = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.shtResult = new GrapeCity.Win.ElTabelle.Sheet();
            this.pnlMain.SuspendLayout();
            this.lblNonyusakiName.ChildPanel.SuspendLayout();
            this.lblNonyusakiName.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.lblUpdateDate.ChildPanel.SuspendLayout();
            this.lblUpdateDate.SuspendLayout();
            this.lblOperationFlag.ChildPanel.SuspendLayout();
            this.lblOperationFlag.SuspendLayout();
            this.lblUpdateUserName.ChildPanel.SuspendLayout();
            this.lblUpdateUserName.SuspendLayout();
            this.lblARNo.ChildPanel.SuspendLayout();
            this.lblARNo.SuspendLayout();
            this.lblShip.ChildPanel.SuspendLayout();
            this.lblShip.SuspendLayout();
            this.lblShukkaFlag.ChildPanel.SuspendLayout();
            this.lblShukkaFlag.SuspendLayout();
            this.lblGamenFlag.ChildPanel.SuspendLayout();
            this.lblGamenFlag.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.shtResult);
            this.pnlMain.Controls.Add(this.grpSearch);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtResult, 0);
            // 
            // fbrFunction
            // 
            // 
            // fbrFunction.F06Button
            // 
            resources.ApplyResources(this.fbrFunction.F06Button, "fbrFunction.F06Button");
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
            // lblNonyusakiName
            // 
            // 
            // lblNonyusakiName.ChildPanel
            // 
            this.lblNonyusakiName.ChildPanel.Controls.Add(this.txtNonyusakiName);
            this.lblNonyusakiName.IsFocusChangeColor = false;
            this.lblNonyusakiName.IsNecessary = true;
            this.lblNonyusakiName.LabelWidth = 80;
            resources.ApplyResources(this.lblNonyusakiName, "lblNonyusakiName");
            this.lblNonyusakiName.Name = "lblNonyusakiName";
            this.lblNonyusakiName.SplitterWidth = 0;
            // 
            // txtNonyusakiName
            // 
            resources.ApplyResources(this.txtNonyusakiName, "txtNonyusakiName");
            this.txtNonyusakiName.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtNonyusakiName.InputRegulation = "F";
            this.txtNonyusakiName.MaxByteLengthMode = true;
            this.txtNonyusakiName.Name = "txtNonyusakiName";
            this.txtNonyusakiName.OneLineMaxLength = 60;
            this.txtNonyusakiName.ProhibitionChar = null;
            // 
            // grpSearch
            // 
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Controls.Add(this.btnListSelect);
            this.grpSearch.Controls.Add(this.lblUpdateDate);
            this.grpSearch.Controls.Add(this.lblOperationFlag);
            this.grpSearch.Controls.Add(this.lblUpdateUserName);
            this.grpSearch.Controls.Add(this.lblARNo);
            this.grpSearch.Controls.Add(this.lblShip);
            this.grpSearch.Controls.Add(this.lblShukkaFlag);
            this.grpSearch.Controls.Add(this.lblGamenFlag);
            this.grpSearch.Controls.Add(this.lblNonyusakiName);
            this.grpSearch.Controls.Add(this.btnSearch);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
            // 
            // btnListSelect
            // 
            resources.ApplyResources(this.btnListSelect, "btnListSelect");
            this.btnListSelect.Name = "btnListSelect";
            // 
            // lblUpdateDate
            // 
            // 
            // lblUpdateDate.ChildPanel
            // 
            this.lblUpdateDate.ChildPanel.Controls.Add(this.dtpUpdateDateTo);
            this.lblUpdateDate.ChildPanel.Controls.Add(this.lblKara);
            this.lblUpdateDate.ChildPanel.Controls.Add(this.dtpUpdateDateFrom);
            this.lblUpdateDate.IsFocusChangeColor = false;
            this.lblUpdateDate.IsNecessary = true;
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
            // lblOperationFlag
            // 
            // 
            // lblOperationFlag.ChildPanel
            // 
            this.lblOperationFlag.ChildPanel.Controls.Add(this.cboOperationFlag);
            this.lblOperationFlag.IsFocusChangeColor = false;
            this.lblOperationFlag.IsNecessary = true;
            this.lblOperationFlag.LabelWidth = 80;
            resources.ApplyResources(this.lblOperationFlag, "lblOperationFlag");
            this.lblOperationFlag.Name = "lblOperationFlag";
            this.lblOperationFlag.SplitterWidth = 0;
            // 
            // cboOperationFlag
            // 
            resources.ApplyResources(this.cboOperationFlag, "cboOperationFlag");
            this.cboOperationFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOperationFlag.Name = "cboOperationFlag";
            // 
            // lblUpdateUserName
            // 
            // 
            // lblUpdateUserName.ChildPanel
            // 
            this.lblUpdateUserName.ChildPanel.Controls.Add(this.txtUpdateUserName);
            this.lblUpdateUserName.IsFocusChangeColor = false;
            this.lblUpdateUserName.IsNecessary = true;
            this.lblUpdateUserName.LabelWidth = 80;
            resources.ApplyResources(this.lblUpdateUserName, "lblUpdateUserName");
            this.lblUpdateUserName.Name = "lblUpdateUserName";
            this.lblUpdateUserName.SplitterWidth = 0;
            // 
            // txtUpdateUserName
            // 
            resources.ApplyResources(this.txtUpdateUserName, "txtUpdateUserName");
            this.txtUpdateUserName.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtUpdateUserName.InputRegulation = "F";
            this.txtUpdateUserName.MaxByteLengthMode = true;
            this.txtUpdateUserName.Name = "txtUpdateUserName";
            this.txtUpdateUserName.OneLineMaxLength = 40;
            this.txtUpdateUserName.ProhibitionChar = null;
            // 
            // lblARNo
            // 
            // 
            // lblARNo.ChildPanel
            // 
            this.lblARNo.ChildPanel.Controls.Add(this.txtARNo);
            this.lblARNo.ChildPanel.Controls.Add(this.lblAR);
            this.lblARNo.IsFocusChangeColor = false;
            this.lblARNo.IsNecessary = true;
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
            this.txtARNo.OneLineMaxLength = 4;
            this.txtARNo.ProhibitionChar = null;
            // 
            // lblAR
            // 
            this.lblAR.BackColor = System.Drawing.Color.LightSkyBlue;
            resources.ApplyResources(this.lblAR, "lblAR");
            this.lblAR.IsFocusChangeColor = false;
            this.lblAR.Name = "lblAR";
            this.lblAR.NormalBackColor = System.Drawing.Color.LightSkyBlue;
            // 
            // lblShip
            // 
            // 
            // lblShip.ChildPanel
            // 
            this.lblShip.ChildPanel.Controls.Add(this.txtShip);
            this.lblShip.IsFocusChangeColor = false;
            this.lblShip.IsNecessary = true;
            this.lblShip.LabelWidth = 80;
            resources.ApplyResources(this.lblShip, "lblShip");
            this.lblShip.Name = "lblShip";
            this.lblShip.SplitterWidth = 0;
            // 
            // txtShip
            // 
            resources.ApplyResources(this.txtShip, "txtShip");
            this.txtShip.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtShip.InputRegulation = "F";
            this.txtShip.MaxByteLengthMode = true;
            this.txtShip.Name = "txtShip";
            this.txtShip.OneLineMaxLength = 10;
            this.txtShip.ProhibitionChar = null;
            // 
            // lblShukkaFlag
            // 
            // 
            // lblShukkaFlag.ChildPanel
            // 
            this.lblShukkaFlag.ChildPanel.Controls.Add(this.cboShukkaFlag);
            this.lblShukkaFlag.IsFocusChangeColor = false;
            this.lblShukkaFlag.IsNecessary = true;
            this.lblShukkaFlag.LabelWidth = 80;
            resources.ApplyResources(this.lblShukkaFlag, "lblShukkaFlag");
            this.lblShukkaFlag.Name = "lblShukkaFlag";
            this.lblShukkaFlag.SplitterWidth = 0;
            // 
            // cboShukkaFlag
            // 
            resources.ApplyResources(this.cboShukkaFlag, "cboShukkaFlag");
            this.cboShukkaFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboShukkaFlag.Name = "cboShukkaFlag";
            this.cboShukkaFlag.SelectionChangeCommitted += new System.EventHandler(this.cboShukkaFlag_SelectionChangeCommitted);
            // 
            // lblGamenFlag
            // 
            // 
            // lblGamenFlag.ChildPanel
            // 
            this.lblGamenFlag.ChildPanel.Controls.Add(this.cboGamenFlag);
            this.lblGamenFlag.IsFocusChangeColor = false;
            this.lblGamenFlag.IsNecessary = true;
            this.lblGamenFlag.LabelWidth = 80;
            resources.ApplyResources(this.lblGamenFlag, "lblGamenFlag");
            this.lblGamenFlag.Name = "lblGamenFlag";
            this.lblGamenFlag.SplitterWidth = 0;
            // 
            // cboGamenFlag
            // 
            resources.ApplyResources(this.cboGamenFlag, "cboGamenFlag");
            this.cboGamenFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGamenFlag.Name = "cboGamenFlag";
            this.cboGamenFlag.SelectionChangeCommitted += new System.EventHandler(this.cboGamenFlag_SelectionChangeCommitted);
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
            // RirekiShokai
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "RirekiShokai";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.lblNonyusakiName.ChildPanel.ResumeLayout(false);
            this.lblNonyusakiName.ChildPanel.PerformLayout();
            this.lblNonyusakiName.ResumeLayout(false);
            this.grpSearch.ResumeLayout(false);
            this.lblUpdateDate.ChildPanel.ResumeLayout(false);
            this.lblUpdateDate.ResumeLayout(false);
            this.lblOperationFlag.ChildPanel.ResumeLayout(false);
            this.lblOperationFlag.ResumeLayout(false);
            this.lblUpdateUserName.ChildPanel.ResumeLayout(false);
            this.lblUpdateUserName.ChildPanel.PerformLayout();
            this.lblUpdateUserName.ResumeLayout(false);
            this.lblARNo.ChildPanel.ResumeLayout(false);
            this.lblARNo.ChildPanel.PerformLayout();
            this.lblARNo.ResumeLayout(false);
            this.lblShip.ChildPanel.ResumeLayout(false);
            this.lblShip.ChildPanel.PerformLayout();
            this.lblShip.ResumeLayout(false);
            this.lblShukkaFlag.ChildPanel.ResumeLayout(false);
            this.lblShukkaFlag.ResumeLayout(false);
            this.lblGamenFlag.ChildPanel.ResumeLayout(false);
            this.lblGamenFlag.ResumeLayout(false);
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
        private DSWControl.DSWLabel.DSWLabel lblShip;
        private DSWControl.DSWTextBox.DSWTextBox txtShip;
        private DSWControl.DSWLabel.DSWLabel lblShukkaFlag;
        private DSWControl.DSWComboBox.DSWComboBox cboShukkaFlag;
        private DSWControl.DSWLabel.DSWLabel lblGamenFlag;
        private DSWControl.DSWTextBox.DSWTextBox txtARNo;
        private DSWControl.DSWControlLinkLabel.DSWControlLinkLabel lblAR;
        private DSWControl.DSWLabel.DSWLabel lblOperationFlag;
        private DSWControl.DSWComboBox.DSWComboBox cboOperationFlag;
        private DSWControl.DSWLabel.DSWLabel lblUpdateUserName;
        private DSWControl.DSWTextBox.DSWTextBox txtUpdateUserName;
        private DSWControl.DSWLabel.DSWLabel lblUpdateDate;
        private GrapeCity.Win.ElTabelle.Sheet shtResult;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnListSelect;
        private DSWControl.DSWComboBox.DSWComboBox cboGamenFlag;
        private DSWControl.DSWDateTimePicker dtpUpdateDateTo;
        private System.Windows.Forms.Label lblKara;
        private DSWControl.DSWDateTimePicker dtpUpdateDateFrom;
    }
}