namespace SMS.S01.Forms
{
    partial class ShukkaKeikakuMeisai
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShukkaKeikakuMeisai));
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.tblSearchCondition = new System.Windows.Forms.TableLayoutPanel();
            this.grpMode = new System.Windows.Forms.GroupBox();
            this.flpCondition = new System.Windows.Forms.FlowLayoutPanel();
            this.rdoInsert = new System.Windows.Forms.RadioButton();
            this.rdoExcel = new System.Windows.Forms.RadioButton();
            this.rdoUpdate = new System.Windows.Forms.RadioButton();
            this.rdoDelete = new System.Windows.Forms.RadioButton();
            this.rdoView = new System.Windows.Forms.RadioButton();
            this.btnStart = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblExcel = new DSWControl.DSWLabel.DSWLabel();
            this.txtExcel = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblDispSelect = new DSWControl.DSWLabel.DSWLabel();
            this.cboDispSelect = new DSWControl.DSWComboBox.DSWComboBox();
            this.btnListSelect = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblARNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtARNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblAR = new DSWControl.DSWControlLinkLabel.DSWControlLinkLabel();
            this.lblShukkaFlag = new DSWControl.DSWLabel.DSWLabel();
            this.cboShukkaFlag = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblShip = new DSWControl.DSWLabel.DSWLabel();
            this.txtShip = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblNonyusakiCD = new DSWControl.DSWLabel.DSWLabel();
            this.txtNonyusakiCD = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnSelect = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblNonyusakiName = new DSWControl.DSWLabel.DSWLabel();
            this.txtNonyusakiName = new DSWControl.DSWTextBox.DSWTextBox();
            this.shtMeisai = new GrapeCity.Win.ElTabelle.Sheet();
            this.ofdExcel = new System.Windows.Forms.OpenFileDialog();
            this.grpKeyAction = new System.Windows.Forms.GroupBox();
            this.flpKeyAction = new System.Windows.Forms.FlowLayoutPanel();
            this.rdoRight = new System.Windows.Forms.RadioButton();
            this.rdoDown = new System.Windows.Forms.RadioButton();
            this.pnlProgress = new System.Windows.Forms.Panel();
            this.lblHikiwatashiShuka = new DSWControl.DSWLabel.DSWLabel();
            this.txtHikiwatashiShuka = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblHikiwatashi = new DSWControl.DSWLabel.DSWLabel();
            this.txtHikiwatashi = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblPalletKonpo = new DSWControl.DSWLabel.DSWLabel();
            this.txtPalletKonpo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblBoxKonpo = new DSWControl.DSWLabel.DSWLabel();
            this.txtBoxKonpo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblShuka = new DSWControl.DSWLabel.DSWLabel();
            this.txtShuka = new DSWControl.DSWTextBox.DSWTextBox();
            this.pnlMain.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.tblSearchCondition.SuspendLayout();
            this.grpMode.SuspendLayout();
            this.flpCondition.SuspendLayout();
            this.lblExcel.ChildPanel.SuspendLayout();
            this.lblExcel.SuspendLayout();
            this.lblDispSelect.ChildPanel.SuspendLayout();
            this.lblDispSelect.SuspendLayout();
            this.lblARNo.ChildPanel.SuspendLayout();
            this.lblARNo.SuspendLayout();
            this.lblShukkaFlag.ChildPanel.SuspendLayout();
            this.lblShukkaFlag.SuspendLayout();
            this.lblShip.ChildPanel.SuspendLayout();
            this.lblShip.SuspendLayout();
            this.lblNonyusakiCD.ChildPanel.SuspendLayout();
            this.lblNonyusakiCD.SuspendLayout();
            this.lblNonyusakiName.ChildPanel.SuspendLayout();
            this.lblNonyusakiName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).BeginInit();
            this.grpKeyAction.SuspendLayout();
            this.flpKeyAction.SuspendLayout();
            this.pnlProgress.SuspendLayout();
            this.lblHikiwatashiShuka.ChildPanel.SuspendLayout();
            this.lblHikiwatashiShuka.SuspendLayout();
            this.lblHikiwatashi.ChildPanel.SuspendLayout();
            this.lblHikiwatashi.SuspendLayout();
            this.lblPalletKonpo.ChildPanel.SuspendLayout();
            this.lblPalletKonpo.SuspendLayout();
            this.lblBoxKonpo.ChildPanel.SuspendLayout();
            this.lblBoxKonpo.SuspendLayout();
            this.lblShuka.ChildPanel.SuspendLayout();
            this.lblShuka.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnlProgress);
            this.pnlMain.Controls.Add(this.grpKeyAction);
            this.pnlMain.Controls.Add(this.grpSearch);
            this.pnlMain.Controls.Add(this.shtMeisai);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.SetChildIndex(this.shtMeisai, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpKeyAction, 0);
            this.pnlMain.Controls.SetChildIndex(this.pnlProgress, 0);
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
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // grpSearch
            // 
            this.grpSearch.Controls.Add(this.tblSearchCondition);
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
            // 
            // tblSearchCondition
            // 
            resources.ApplyResources(this.tblSearchCondition, "tblSearchCondition");
            this.tblSearchCondition.Controls.Add(this.grpMode, 0, 0);
            this.tblSearchCondition.Controls.Add(this.btnStart, 3, 4);
            this.tblSearchCondition.Controls.Add(this.lblExcel, 0, 1);
            this.tblSearchCondition.Controls.Add(this.lblDispSelect, 1, 4);
            this.tblSearchCondition.Controls.Add(this.btnListSelect, 3, 2);
            this.tblSearchCondition.Controls.Add(this.lblARNo, 2, 3);
            this.tblSearchCondition.Controls.Add(this.lblShukkaFlag, 0, 2);
            this.tblSearchCondition.Controls.Add(this.lblShip, 1, 3);
            this.tblSearchCondition.Controls.Add(this.lblNonyusakiCD, 0, 4);
            this.tblSearchCondition.Controls.Add(this.btnSelect, 3, 1);
            this.tblSearchCondition.Controls.Add(this.lblNonyusakiName, 1, 2);
            this.tblSearchCondition.Name = "tblSearchCondition";
            // 
            // grpMode
            // 
            this.tblSearchCondition.SetColumnSpan(this.grpMode, 4);
            this.grpMode.Controls.Add(this.flpCondition);
            resources.ApplyResources(this.grpMode, "grpMode");
            this.grpMode.Name = "grpMode";
            this.grpMode.TabStop = false;
            // 
            // flpCondition
            // 
            resources.ApplyResources(this.flpCondition, "flpCondition");
            this.flpCondition.Controls.Add(this.rdoInsert);
            this.flpCondition.Controls.Add(this.rdoExcel);
            this.flpCondition.Controls.Add(this.rdoUpdate);
            this.flpCondition.Controls.Add(this.rdoDelete);
            this.flpCondition.Controls.Add(this.rdoView);
            this.flpCondition.Name = "flpCondition";
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
            // rdoExcel
            // 
            resources.ApplyResources(this.rdoExcel, "rdoExcel");
            this.rdoExcel.Name = "rdoExcel";
            this.rdoExcel.UseVisualStyleBackColor = true;
            this.rdoExcel.CheckedChanged += new System.EventHandler(this.rdoExcel_CheckedChanged);
            // 
            // rdoUpdate
            // 
            resources.ApplyResources(this.rdoUpdate, "rdoUpdate");
            this.rdoUpdate.Name = "rdoUpdate";
            this.rdoUpdate.UseVisualStyleBackColor = true;
            this.rdoUpdate.CheckedChanged += new System.EventHandler(this.rdoUpdate_CheckedChanged);
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
            // btnStart
            // 
            resources.ApplyResources(this.btnStart, "btnStart");
            this.btnStart.Name = "btnStart";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblExcel
            // 
            // 
            // lblExcel.ChildPanel
            // 
            this.lblExcel.ChildPanel.Controls.Add(this.txtExcel);
            this.tblSearchCondition.SetColumnSpan(this.lblExcel, 3);
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
            // lblDispSelect
            // 
            // 
            // lblDispSelect.ChildPanel
            // 
            this.lblDispSelect.ChildPanel.Controls.Add(this.cboDispSelect);
            this.lblDispSelect.IsFocusChangeColor = false;
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
            // 
            // btnListSelect
            // 
            resources.ApplyResources(this.btnListSelect, "btnListSelect");
            this.btnListSelect.Name = "btnListSelect";
            this.btnListSelect.TabStop = false;
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
            this.txtARNo.InputRegulation = "n";
            this.txtARNo.IsInputRegulation = true;
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
            // lblShukkaFlag
            // 
            // 
            // lblShukkaFlag.ChildPanel
            // 
            this.lblShukkaFlag.ChildPanel.Controls.Add(this.cboShukkaFlag);
            this.lblShukkaFlag.IsFocusChangeColor = false;
            this.lblShukkaFlag.IsNecessary = true;
            this.lblShukkaFlag.LabelWidth = 100;
            resources.ApplyResources(this.lblShukkaFlag, "lblShukkaFlag");
            this.lblShukkaFlag.Name = "lblShukkaFlag";
            this.lblShukkaFlag.SplitterWidth = 0;
            // 
            // cboShukkaFlag
            // 
            resources.ApplyResources(this.cboShukkaFlag, "cboShukkaFlag");
            this.cboShukkaFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboShukkaFlag.Name = "cboShukkaFlag";
            this.cboShukkaFlag.SelectedIndexChanged += new System.EventHandler(this.cboShukkaFlag_SelectedIndexChanged);
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
            // lblNonyusakiCD
            // 
            // 
            // lblNonyusakiCD.ChildPanel
            // 
            this.lblNonyusakiCD.ChildPanel.Controls.Add(this.txtNonyusakiCD);
            this.lblNonyusakiCD.IsFocusChangeColor = false;
            this.lblNonyusakiCD.LabelWidth = 100;
            resources.ApplyResources(this.lblNonyusakiCD, "lblNonyusakiCD");
            this.lblNonyusakiCD.Name = "lblNonyusakiCD";
            this.lblNonyusakiCD.SplitterWidth = 0;
            this.lblNonyusakiCD.TabStop = false;
            // 
            // txtNonyusakiCD
            // 
            this.txtNonyusakiCD.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtNonyusakiCD, "txtNonyusakiCD");
            this.txtNonyusakiCD.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtNonyusakiCD.InputRegulation = "F";
            this.txtNonyusakiCD.Name = "txtNonyusakiCD";
            this.txtNonyusakiCD.ProhibitionChar = null;
            this.txtNonyusakiCD.ReadOnly = true;
            this.txtNonyusakiCD.TabStop = false;
            // 
            // btnSelect
            // 
            resources.ApplyResources(this.btnSelect, "btnSelect");
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.TabStop = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // lblNonyusakiName
            // 
            // 
            // lblNonyusakiName.ChildPanel
            // 
            this.lblNonyusakiName.ChildPanel.Controls.Add(this.txtNonyusakiName);
            this.tblSearchCondition.SetColumnSpan(this.lblNonyusakiName, 2);
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
            // shtMeisai
            // 
            resources.ApplyResources(this.shtMeisai, "shtMeisai");
            this.shtMeisai.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtMeisai.Data")));
            this.shtMeisai.Name = "shtMeisai";
            this.shtMeisai.LeaveEdit += new GrapeCity.Win.ElTabelle.LeaveEditEventHandler(this.shtMeisai_LeaveEdit);
            this.shtMeisai.ClippingData += new GrapeCity.Win.ElTabelle.ClippingDataEventHandler(this.shtMeisai_ClippingData);
            // 
            // ofdExcel
            // 
            resources.ApplyResources(this.ofdExcel, "ofdExcel");
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
            // pnlProgress
            // 
            this.pnlProgress.Controls.Add(this.lblHikiwatashiShuka);
            this.pnlProgress.Controls.Add(this.lblHikiwatashi);
            this.pnlProgress.Controls.Add(this.lblPalletKonpo);
            this.pnlProgress.Controls.Add(this.lblBoxKonpo);
            this.pnlProgress.Controls.Add(this.lblShuka);
            resources.ApplyResources(this.pnlProgress, "pnlProgress");
            this.pnlProgress.Name = "pnlProgress";
            // 
            // lblHikiwatashiShuka
            // 
            // 
            // lblHikiwatashiShuka.ChildPanel
            // 
            this.lblHikiwatashiShuka.ChildPanel.Controls.Add(this.txtHikiwatashiShuka);
            this.lblHikiwatashiShuka.IsFocusChangeColor = false;
            this.lblHikiwatashiShuka.LabelWidth = 80;
            resources.ApplyResources(this.lblHikiwatashiShuka, "lblHikiwatashiShuka");
            this.lblHikiwatashiShuka.Name = "lblHikiwatashiShuka";
            this.lblHikiwatashiShuka.NormalBackColor = System.Drawing.Color.PaleGreen;
            this.lblHikiwatashiShuka.SplitterWidth = 0;
            // 
            // txtHikiwatashiShuka
            // 
            this.txtHikiwatashiShuka.BackColor = System.Drawing.SystemColors.Control;
            this.txtHikiwatashiShuka.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtHikiwatashiShuka.InputRegulation = "";
            resources.ApplyResources(this.txtHikiwatashiShuka, "txtHikiwatashiShuka");
            this.txtHikiwatashiShuka.Name = "txtHikiwatashiShuka";
            this.txtHikiwatashiShuka.ProhibitionChar = null;
            this.txtHikiwatashiShuka.ReadOnly = true;
            // 
            // lblHikiwatashi
            // 
            // 
            // lblHikiwatashi.ChildPanel
            // 
            this.lblHikiwatashi.ChildPanel.Controls.Add(this.txtHikiwatashi);
            this.lblHikiwatashi.IsFocusChangeColor = false;
            this.lblHikiwatashi.LabelWidth = 80;
            resources.ApplyResources(this.lblHikiwatashi, "lblHikiwatashi");
            this.lblHikiwatashi.Name = "lblHikiwatashi";
            this.lblHikiwatashi.NormalBackColor = System.Drawing.Color.PaleGreen;
            this.lblHikiwatashi.SplitterWidth = 0;
            // 
            // txtHikiwatashi
            // 
            this.txtHikiwatashi.BackColor = System.Drawing.SystemColors.Control;
            this.txtHikiwatashi.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtHikiwatashi.InputRegulation = "";
            resources.ApplyResources(this.txtHikiwatashi, "txtHikiwatashi");
            this.txtHikiwatashi.Name = "txtHikiwatashi";
            this.txtHikiwatashi.ProhibitionChar = null;
            this.txtHikiwatashi.ReadOnly = true;
            // 
            // lblPalletKonpo
            // 
            // 
            // lblPalletKonpo.ChildPanel
            // 
            this.lblPalletKonpo.ChildPanel.Controls.Add(this.txtPalletKonpo);
            this.lblPalletKonpo.IsFocusChangeColor = false;
            this.lblPalletKonpo.LabelWidth = 80;
            resources.ApplyResources(this.lblPalletKonpo, "lblPalletKonpo");
            this.lblPalletKonpo.Name = "lblPalletKonpo";
            this.lblPalletKonpo.NormalBackColor = System.Drawing.Color.PaleGreen;
            this.lblPalletKonpo.SplitterWidth = 0;
            // 
            // txtPalletKonpo
            // 
            this.txtPalletKonpo.BackColor = System.Drawing.SystemColors.Control;
            this.txtPalletKonpo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtPalletKonpo.InputRegulation = "";
            resources.ApplyResources(this.txtPalletKonpo, "txtPalletKonpo");
            this.txtPalletKonpo.Name = "txtPalletKonpo";
            this.txtPalletKonpo.ProhibitionChar = null;
            this.txtPalletKonpo.ReadOnly = true;
            // 
            // lblBoxKonpo
            // 
            // 
            // lblBoxKonpo.ChildPanel
            // 
            this.lblBoxKonpo.ChildPanel.Controls.Add(this.txtBoxKonpo);
            this.lblBoxKonpo.IsFocusChangeColor = false;
            this.lblBoxKonpo.LabelWidth = 80;
            resources.ApplyResources(this.lblBoxKonpo, "lblBoxKonpo");
            this.lblBoxKonpo.Name = "lblBoxKonpo";
            this.lblBoxKonpo.NormalBackColor = System.Drawing.Color.PaleGreen;
            this.lblBoxKonpo.SplitterWidth = 0;
            // 
            // txtBoxKonpo
            // 
            this.txtBoxKonpo.BackColor = System.Drawing.SystemColors.Control;
            this.txtBoxKonpo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtBoxKonpo.InputRegulation = "";
            resources.ApplyResources(this.txtBoxKonpo, "txtBoxKonpo");
            this.txtBoxKonpo.Name = "txtBoxKonpo";
            this.txtBoxKonpo.ProhibitionChar = null;
            this.txtBoxKonpo.ReadOnly = true;
            // 
            // lblShuka
            // 
            // 
            // lblShuka.ChildPanel
            // 
            this.lblShuka.ChildPanel.Controls.Add(this.txtShuka);
            this.lblShuka.IsFocusChangeColor = false;
            this.lblShuka.LabelWidth = 80;
            resources.ApplyResources(this.lblShuka, "lblShuka");
            this.lblShuka.Name = "lblShuka";
            this.lblShuka.NormalBackColor = System.Drawing.Color.PaleGreen;
            this.lblShuka.SplitterWidth = 0;
            // 
            // txtShuka
            // 
            this.txtShuka.BackColor = System.Drawing.SystemColors.Control;
            this.txtShuka.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtShuka.InputRegulation = "";
            resources.ApplyResources(this.txtShuka, "txtShuka");
            this.txtShuka.Name = "txtShuka";
            this.txtShuka.ProhibitionChar = null;
            this.txtShuka.ReadOnly = true;
            // 
            // ShukkaKeikakuMeisai
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ShukkaKeikakuMeisai";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.grpSearch.ResumeLayout(false);
            this.tblSearchCondition.ResumeLayout(false);
            this.grpMode.ResumeLayout(false);
            this.grpMode.PerformLayout();
            this.flpCondition.ResumeLayout(false);
            this.flpCondition.PerformLayout();
            this.lblExcel.ChildPanel.ResumeLayout(false);
            this.lblExcel.ChildPanel.PerformLayout();
            this.lblExcel.ResumeLayout(false);
            this.lblDispSelect.ChildPanel.ResumeLayout(false);
            this.lblDispSelect.ResumeLayout(false);
            this.lblARNo.ChildPanel.ResumeLayout(false);
            this.lblARNo.ChildPanel.PerformLayout();
            this.lblARNo.ResumeLayout(false);
            this.lblShukkaFlag.ChildPanel.ResumeLayout(false);
            this.lblShukkaFlag.ResumeLayout(false);
            this.lblShip.ChildPanel.ResumeLayout(false);
            this.lblShip.ChildPanel.PerformLayout();
            this.lblShip.ResumeLayout(false);
            this.lblNonyusakiCD.ChildPanel.ResumeLayout(false);
            this.lblNonyusakiCD.ChildPanel.PerformLayout();
            this.lblNonyusakiCD.ResumeLayout(false);
            this.lblNonyusakiName.ChildPanel.ResumeLayout(false);
            this.lblNonyusakiName.ChildPanel.PerformLayout();
            this.lblNonyusakiName.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).EndInit();
            this.grpKeyAction.ResumeLayout(false);
            this.grpKeyAction.PerformLayout();
            this.flpKeyAction.ResumeLayout(false);
            this.flpKeyAction.PerformLayout();
            this.pnlProgress.ResumeLayout(false);
            this.lblHikiwatashiShuka.ChildPanel.ResumeLayout(false);
            this.lblHikiwatashiShuka.ChildPanel.PerformLayout();
            this.lblHikiwatashiShuka.ResumeLayout(false);
            this.lblHikiwatashi.ChildPanel.ResumeLayout(false);
            this.lblHikiwatashi.ChildPanel.PerformLayout();
            this.lblHikiwatashi.ResumeLayout(false);
            this.lblPalletKonpo.ChildPanel.ResumeLayout(false);
            this.lblPalletKonpo.ChildPanel.PerformLayout();
            this.lblPalletKonpo.ResumeLayout(false);
            this.lblBoxKonpo.ChildPanel.ResumeLayout(false);
            this.lblBoxKonpo.ChildPanel.PerformLayout();
            this.lblBoxKonpo.ResumeLayout(false);
            this.lblShuka.ChildPanel.ResumeLayout(false);
            this.lblShuka.ChildPanel.PerformLayout();
            this.lblShuka.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSearch;
        private DSWControl.DSWLabel.DSWLabel lblDispSelect;
        private DSWControl.DSWLabel.DSWLabel lblARNo;
        private DSWControl.DSWLabel.DSWLabel lblShip;
        private DSWControl.DSWLabel.DSWLabel lblNonyusakiName;
        private DSWControl.DSWLabel.DSWLabel lblShukkaFlag;
        private DSWControl.DSWLabel.DSWLabel lblExcel;
        private System.Windows.Forms.GroupBox grpMode;
        private System.Windows.Forms.RadioButton rdoView;
        private System.Windows.Forms.RadioButton rdoInsert;
        private System.Windows.Forms.RadioButton rdoDelete;
        private System.Windows.Forms.RadioButton rdoExcel;
        private System.Windows.Forms.RadioButton rdoUpdate;
        private DSWControl.DSWLabel.DSWLabel lblNonyusakiCD;
        private DSWControl.DSWTextBox.DSWTextBox txtNonyusakiCD;
        private DSWControl.DSWComboBox.DSWComboBox cboDispSelect;
        private DSWControl.DSWTextBox.DSWTextBox txtARNo;
        private DSWControl.DSWTextBox.DSWTextBox txtShip;
        private DSWControl.DSWTextBox.DSWTextBox txtNonyusakiName;
        private DSWControl.DSWComboBox.DSWComboBox cboShukkaFlag;
        private DSWControl.DSWTextBox.DSWTextBox txtExcel;
        private GrapeCity.Win.ElTabelle.Sheet shtMeisai;
        private DSWControl.DSWControlLinkLabel.DSWControlLinkLabel lblAR;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnListSelect;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnSelect;
        private System.Windows.Forms.OpenFileDialog ofdExcel;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnStart;
        private System.Windows.Forms.FlowLayoutPanel flpCondition;
        private System.Windows.Forms.TableLayoutPanel tblSearchCondition;
        private System.Windows.Forms.GroupBox grpKeyAction;
        private System.Windows.Forms.FlowLayoutPanel flpKeyAction;
        private System.Windows.Forms.RadioButton rdoRight;
        private System.Windows.Forms.RadioButton rdoDown;
        private System.Windows.Forms.Panel pnlProgress;
        private DSWControl.DSWLabel.DSWLabel lblHikiwatashi;
        private DSWControl.DSWTextBox.DSWTextBox txtHikiwatashi;
        private DSWControl.DSWLabel.DSWLabel lblPalletKonpo;
        private DSWControl.DSWTextBox.DSWTextBox txtPalletKonpo;
        private DSWControl.DSWLabel.DSWLabel lblBoxKonpo;
        private DSWControl.DSWTextBox.DSWTextBox txtBoxKonpo;
        private DSWControl.DSWLabel.DSWLabel lblShuka;
        private DSWControl.DSWTextBox.DSWTextBox txtShuka;
        private DSWControl.DSWLabel.DSWLabel lblHikiwatashiShuka;
        private DSWControl.DSWTextBox.DSWTextBox txtHikiwatashiShuka;
    }
}