namespace SMS.K04.Forms
{
    partial class HandyOpeRirekiShokai
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HandyOpeRirekiShokai));
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.lblWorkerName = new DSWControl.DSWLabel.DSWLabel();
            this.txtWorkerName = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblShip = new DSWControl.DSWLabel.DSWLabel();
            this.txtShip = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblTehaiNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtTehaiNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblBukkenName = new DSWControl.DSWLabel.DSWLabel();
            this.cboBukkenName = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblPalletNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtPalletNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblPallet = new DSWControl.DSWControlLinkLabel.DSWControlLinkLabel();
            this.lblBoxNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtBoxNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblBox = new DSWControl.DSWControlLinkLabel.DSWControlLinkLabel();
            this.lblTagNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtTagNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblARNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtARNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblAR = new DSWControl.DSWControlLinkLabel.DSWControlLinkLabel();
            this.dtpUpdateDateTo = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.lblUpdateDateFrom = new DSWControl.DSWLabel.DSWLabel();
            this.dtpUpdateDateFrom = new System.Windows.Forms.DateTimePicker();
            this.lblSearchShukkaFlag = new DSWControl.DSWLabel.DSWLabel();
            this.cboShukkaFlag = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblHandyOpeFlag = new DSWControl.DSWLabel.DSWLabel();
            this.cboHandyOpeFlag = new DSWControl.DSWComboBox.DSWComboBox();
            this.btnDisp = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.shtMeisai = new GrapeCity.Win.ElTabelle.Sheet();
            this.txtSearchUserID = new DSWControl.DSWTextBox.DSWTextBox();
            this.pnlMain.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.lblWorkerName.ChildPanel.SuspendLayout();
            this.lblWorkerName.SuspendLayout();
            this.lblShip.ChildPanel.SuspendLayout();
            this.lblShip.SuspendLayout();
            this.lblTehaiNo.ChildPanel.SuspendLayout();
            this.lblTehaiNo.SuspendLayout();
            this.lblBukkenName.ChildPanel.SuspendLayout();
            this.lblBukkenName.SuspendLayout();
            this.lblPalletNo.ChildPanel.SuspendLayout();
            this.lblPalletNo.SuspendLayout();
            this.lblBoxNo.ChildPanel.SuspendLayout();
            this.lblBoxNo.SuspendLayout();
            this.lblTagNo.ChildPanel.SuspendLayout();
            this.lblTagNo.SuspendLayout();
            this.lblARNo.ChildPanel.SuspendLayout();
            this.lblARNo.SuspendLayout();
            this.lblUpdateDateFrom.ChildPanel.SuspendLayout();
            this.lblUpdateDateFrom.SuspendLayout();
            this.lblSearchShukkaFlag.ChildPanel.SuspendLayout();
            this.lblSearchShukkaFlag.SuspendLayout();
            this.lblHandyOpeFlag.ChildPanel.SuspendLayout();
            this.lblHandyOpeFlag.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.shtMeisai);
            this.pnlMain.Controls.Add(this.grpSearch);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtMeisai, 0);
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
            // grpSearch
            // 
            this.grpSearch.Controls.Add(this.lblWorkerName);
            this.grpSearch.Controls.Add(this.lblShip);
            this.grpSearch.Controls.Add(this.lblTehaiNo);
            this.grpSearch.Controls.Add(this.lblBukkenName);
            this.grpSearch.Controls.Add(this.lblPalletNo);
            this.grpSearch.Controls.Add(this.lblBoxNo);
            this.grpSearch.Controls.Add(this.lblTagNo);
            this.grpSearch.Controls.Add(this.lblARNo);
            this.grpSearch.Controls.Add(this.dtpUpdateDateTo);
            this.grpSearch.Controls.Add(this.label1);
            this.grpSearch.Controls.Add(this.lblUpdateDateFrom);
            this.grpSearch.Controls.Add(this.lblSearchShukkaFlag);
            this.grpSearch.Controls.Add(this.lblHandyOpeFlag);
            this.grpSearch.Controls.Add(this.btnDisp);
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
            // 
            // lblWorkerName
            // 
            // 
            // lblWorkerName.ChildPanel
            // 
            this.lblWorkerName.ChildPanel.Controls.Add(this.txtWorkerName);
            this.lblWorkerName.IsFocusChangeColor = false;
            this.lblWorkerName.LabelWidth = 80;
            resources.ApplyResources(this.lblWorkerName, "lblWorkerName");
            this.lblWorkerName.Name = "lblWorkerName";
            this.lblWorkerName.SplitterWidth = 0;
            // 
            // txtWorkerName
            // 
            resources.ApplyResources(this.txtWorkerName, "txtWorkerName");
            this.txtWorkerName.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtWorkerName.InputRegulation = "abn";
            this.txtWorkerName.IsInputRegulation = true;
            this.txtWorkerName.MaxByteLengthMode = true;
            this.txtWorkerName.Name = "txtWorkerName";
            this.txtWorkerName.OneLineMaxLength = 10;
            this.txtWorkerName.ProhibitionChar = null;
            // 
            // lblShip
            // 
            // 
            // lblShip.ChildPanel
            // 
            this.lblShip.ChildPanel.Controls.Add(this.txtShip);
            this.lblShip.IsFocusChangeColor = false;
            this.lblShip.LabelWidth = 100;
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
            // lblTehaiNo
            // 
            // 
            // lblTehaiNo.ChildPanel
            // 
            this.lblTehaiNo.ChildPanel.Controls.Add(this.txtTehaiNo);
            this.lblTehaiNo.IsFocusChangeColor = false;
            this.lblTehaiNo.LabelWidth = 100;
            resources.ApplyResources(this.lblTehaiNo, "lblTehaiNo");
            this.lblTehaiNo.Name = "lblTehaiNo";
            this.lblTehaiNo.SplitterWidth = 0;
            // 
            // txtTehaiNo
            // 
            this.txtTehaiNo.AutoPad = true;
            this.txtTehaiNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            resources.ApplyResources(this.txtTehaiNo, "txtTehaiNo");
            this.txtTehaiNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtTehaiNo.InputRegulation = "abn";
            this.txtTehaiNo.IsInputRegulation = true;
            this.txtTehaiNo.MaxByteLengthMode = true;
            this.txtTehaiNo.Name = "txtTehaiNo";
            this.txtTehaiNo.OneLineMaxLength = 8;
            this.txtTehaiNo.PaddingChar = '0';
            this.txtTehaiNo.ProhibitionChar = null;
            // 
            // lblBukkenName
            // 
            // 
            // lblBukkenName.ChildPanel
            // 
            this.lblBukkenName.ChildPanel.Controls.Add(this.cboBukkenName);
            this.lblBukkenName.IsFocusChangeColor = false;
            this.lblBukkenName.IsNecessary = true;
            this.lblBukkenName.LabelWidth = 80;
            resources.ApplyResources(this.lblBukkenName, "lblBukkenName");
            this.lblBukkenName.Name = "lblBukkenName";
            this.lblBukkenName.SplitterWidth = 0;
            // 
            // cboBukkenName
            // 
            this.cboBukkenName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboBukkenName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.cboBukkenName, "cboBukkenName");
            this.cboBukkenName.Name = "cboBukkenName";
            // 
            // lblPalletNo
            // 
            // 
            // lblPalletNo.ChildPanel
            // 
            this.lblPalletNo.ChildPanel.Controls.Add(this.txtPalletNo);
            this.lblPalletNo.ChildPanel.Controls.Add(this.lblPallet);
            this.lblPalletNo.IsFocusChangeColor = false;
            this.lblPalletNo.LabelWidth = 60;
            resources.ApplyResources(this.lblPalletNo, "lblPalletNo");
            this.lblPalletNo.Name = "lblPalletNo";
            this.lblPalletNo.SplitterWidth = 0;
            // 
            // txtPalletNo
            // 
            this.txtPalletNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            resources.ApplyResources(this.txtPalletNo, "txtPalletNo");
            this.txtPalletNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtPalletNo.InputRegulation = "nab";
            this.txtPalletNo.IsInputRegulation = true;
            this.txtPalletNo.MaxByteLengthMode = true;
            this.txtPalletNo.Name = "txtPalletNo";
            this.txtPalletNo.OneLineMaxLength = 5;
            this.txtPalletNo.ProhibitionChar = null;
            // 
            // lblPallet
            // 
            this.lblPallet.BackColor = System.Drawing.Color.LightSkyBlue;
            resources.ApplyResources(this.lblPallet, "lblPallet");
            this.lblPallet.IsFocusChangeColor = false;
            this.lblPallet.Name = "lblPallet";
            this.lblPallet.NormalBackColor = System.Drawing.Color.LightSkyBlue;
            // 
            // lblBoxNo
            // 
            // 
            // lblBoxNo.ChildPanel
            // 
            this.lblBoxNo.ChildPanel.Controls.Add(this.txtBoxNo);
            this.lblBoxNo.ChildPanel.Controls.Add(this.lblBox);
            this.lblBoxNo.IsFocusChangeColor = false;
            this.lblBoxNo.LabelWidth = 60;
            resources.ApplyResources(this.lblBoxNo, "lblBoxNo");
            this.lblBoxNo.Name = "lblBoxNo";
            this.lblBoxNo.SplitterWidth = 0;
            // 
            // txtBoxNo
            // 
            this.txtBoxNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            resources.ApplyResources(this.txtBoxNo, "txtBoxNo");
            this.txtBoxNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtBoxNo.InputRegulation = "nab";
            this.txtBoxNo.IsInputRegulation = true;
            this.txtBoxNo.MaxByteLengthMode = true;
            this.txtBoxNo.Name = "txtBoxNo";
            this.txtBoxNo.OneLineMaxLength = 5;
            this.txtBoxNo.ProhibitionChar = null;
            // 
            // lblBox
            // 
            this.lblBox.BackColor = System.Drawing.Color.LightSkyBlue;
            resources.ApplyResources(this.lblBox, "lblBox");
            this.lblBox.IsFocusChangeColor = false;
            this.lblBox.Name = "lblBox";
            this.lblBox.NormalBackColor = System.Drawing.Color.LightSkyBlue;
            // 
            // lblTagNo
            // 
            // 
            // lblTagNo.ChildPanel
            // 
            this.lblTagNo.ChildPanel.Controls.Add(this.txtTagNo);
            this.lblTagNo.IsFocusChangeColor = false;
            this.lblTagNo.LabelWidth = 100;
            resources.ApplyResources(this.lblTagNo, "lblTagNo");
            this.lblTagNo.Name = "lblTagNo";
            this.lblTagNo.SplitterWidth = 0;
            // 
            // txtTagNo
            // 
            resources.ApplyResources(this.txtTagNo, "txtTagNo");
            this.txtTagNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtTagNo.InputRegulation = "n";
            this.txtTagNo.IsInputRegulation = true;
            this.txtTagNo.MaxByteLengthMode = true;
            this.txtTagNo.Name = "txtTagNo";
            this.txtTagNo.OneLineMaxLength = 5;
            this.txtTagNo.ProhibitionChar = null;
            // 
            // lblARNo
            // 
            // 
            // lblARNo.ChildPanel
            // 
            this.lblARNo.ChildPanel.Controls.Add(this.txtARNo);
            this.lblARNo.ChildPanel.Controls.Add(this.lblAR);
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
            // dtpUpdateDateTo
            // 
            this.dtpUpdateDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpUpdateDateTo, "dtpUpdateDateTo");
            this.dtpUpdateDateTo.Name = "dtpUpdateDateTo";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lblUpdateDateFrom
            // 
            // 
            // lblUpdateDateFrom.ChildPanel
            // 
            this.lblUpdateDateFrom.ChildPanel.Controls.Add(this.dtpUpdateDateFrom);
            this.lblUpdateDateFrom.IsFocusChangeColor = false;
            this.lblUpdateDateFrom.IsNecessary = true;
            this.lblUpdateDateFrom.LabelWidth = 80;
            resources.ApplyResources(this.lblUpdateDateFrom, "lblUpdateDateFrom");
            this.lblUpdateDateFrom.Name = "lblUpdateDateFrom";
            this.lblUpdateDateFrom.SplitterWidth = 0;
            // 
            // dtpUpdateDateFrom
            // 
            resources.ApplyResources(this.dtpUpdateDateFrom, "dtpUpdateDateFrom");
            this.dtpUpdateDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpUpdateDateFrom.Name = "dtpUpdateDateFrom";
            // 
            // lblSearchShukkaFlag
            // 
            // 
            // lblSearchShukkaFlag.ChildPanel
            // 
            this.lblSearchShukkaFlag.ChildPanel.Controls.Add(this.cboShukkaFlag);
            this.lblSearchShukkaFlag.IsFocusChangeColor = false;
            this.lblSearchShukkaFlag.IsNecessary = true;
            this.lblSearchShukkaFlag.LabelWidth = 100;
            resources.ApplyResources(this.lblSearchShukkaFlag, "lblSearchShukkaFlag");
            this.lblSearchShukkaFlag.Name = "lblSearchShukkaFlag";
            this.lblSearchShukkaFlag.SplitterWidth = 0;
            // 
            // cboShukkaFlag
            // 
            resources.ApplyResources(this.cboShukkaFlag, "cboShukkaFlag");
            this.cboShukkaFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboShukkaFlag.Name = "cboShukkaFlag";
            this.cboShukkaFlag.SelectionChangeCommitted += new System.EventHandler(this.cboShukkaFlag_SelectionChangeCommitted);
            // 
            // lblHandyOpeFlag
            // 
            // 
            // lblHandyOpeFlag.ChildPanel
            // 
            this.lblHandyOpeFlag.ChildPanel.Controls.Add(this.cboHandyOpeFlag);
            this.lblHandyOpeFlag.IsFocusChangeColor = false;
            this.lblHandyOpeFlag.IsNecessary = true;
            this.lblHandyOpeFlag.LabelWidth = 100;
            resources.ApplyResources(this.lblHandyOpeFlag, "lblHandyOpeFlag");
            this.lblHandyOpeFlag.Name = "lblHandyOpeFlag";
            this.lblHandyOpeFlag.SplitterWidth = 0;
            // 
            // cboHandyOpeFlag
            // 
            resources.ApplyResources(this.cboHandyOpeFlag, "cboHandyOpeFlag");
            this.cboHandyOpeFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHandyOpeFlag.Name = "cboHandyOpeFlag";
            this.cboHandyOpeFlag.SelectionChangeCommitted += new System.EventHandler(this.cboOpeFlag_SelectionChangeCommitted);
            // 
            // btnDisp
            // 
            resources.ApplyResources(this.btnDisp, "btnDisp");
            this.btnDisp.Name = "btnDisp";
            this.btnDisp.Click += new System.EventHandler(this.btnDisp_Click);
            // 
            // shtMeisai
            // 
            resources.ApplyResources(this.shtMeisai, "shtMeisai");
            this.shtMeisai.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtMeisai.Data")));
            this.shtMeisai.Name = "shtMeisai";
            // 
            // txtSearchUserID
            // 
            resources.ApplyResources(this.txtSearchUserID, "txtSearchUserID");
            this.txtSearchUserID.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtSearchUserID.InputRegulation = "abn";
            this.txtSearchUserID.IsInputRegulation = true;
            this.txtSearchUserID.MaxByteLengthMode = true;
            this.txtSearchUserID.Name = "txtSearchUserID";
            this.txtSearchUserID.OneLineMaxLength = 10;
            this.txtSearchUserID.ProhibitionChar = null;
            // 
            // HandyOpeRirekiShokai
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HandyOpeRirekiShokai";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.grpSearch.ResumeLayout(false);
            this.grpSearch.PerformLayout();
            this.lblWorkerName.ChildPanel.ResumeLayout(false);
            this.lblWorkerName.ChildPanel.PerformLayout();
            this.lblWorkerName.ResumeLayout(false);
            this.lblShip.ChildPanel.ResumeLayout(false);
            this.lblShip.ChildPanel.PerformLayout();
            this.lblShip.ResumeLayout(false);
            this.lblTehaiNo.ChildPanel.ResumeLayout(false);
            this.lblTehaiNo.ChildPanel.PerformLayout();
            this.lblTehaiNo.ResumeLayout(false);
            this.lblBukkenName.ChildPanel.ResumeLayout(false);
            this.lblBukkenName.ResumeLayout(false);
            this.lblPalletNo.ChildPanel.ResumeLayout(false);
            this.lblPalletNo.ChildPanel.PerformLayout();
            this.lblPalletNo.ResumeLayout(false);
            this.lblBoxNo.ChildPanel.ResumeLayout(false);
            this.lblBoxNo.ChildPanel.PerformLayout();
            this.lblBoxNo.ResumeLayout(false);
            this.lblTagNo.ChildPanel.ResumeLayout(false);
            this.lblTagNo.ChildPanel.PerformLayout();
            this.lblTagNo.ResumeLayout(false);
            this.lblARNo.ChildPanel.ResumeLayout(false);
            this.lblARNo.ChildPanel.PerformLayout();
            this.lblARNo.ResumeLayout(false);
            this.lblUpdateDateFrom.ChildPanel.ResumeLayout(false);
            this.lblUpdateDateFrom.ResumeLayout(false);
            this.lblSearchShukkaFlag.ChildPanel.ResumeLayout(false);
            this.lblSearchShukkaFlag.ResumeLayout(false);
            this.lblHandyOpeFlag.ChildPanel.ResumeLayout(false);
            this.lblHandyOpeFlag.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSearch;
        private DSWControl.DSWLabel.DSWLabel lblPalletNo;
        private DSWControl.DSWTextBox.DSWTextBox txtPalletNo;
        private DSWControl.DSWControlLinkLabel.DSWControlLinkLabel lblPallet;
        private DSWControl.DSWLabel.DSWLabel lblBoxNo;
        private DSWControl.DSWTextBox.DSWTextBox txtBoxNo;
        private DSWControl.DSWControlLinkLabel.DSWControlLinkLabel lblBox;
        private DSWControl.DSWLabel.DSWLabel lblTagNo;
        private DSWControl.DSWTextBox.DSWTextBox txtTagNo;
        private DSWControl.DSWLabel.DSWLabel lblARNo;
        private DSWControl.DSWTextBox.DSWTextBox txtARNo;
        private DSWControl.DSWControlLinkLabel.DSWControlLinkLabel lblAR;
        private System.Windows.Forms.DateTimePicker dtpUpdateDateTo;
        private System.Windows.Forms.Label label1;
        private DSWControl.DSWLabel.DSWLabel lblUpdateDateFrom;
        private System.Windows.Forms.DateTimePicker dtpUpdateDateFrom;
        private DSWControl.DSWLabel.DSWLabel lblSearchShukkaFlag;
        private DSWControl.DSWComboBox.DSWComboBox cboShukkaFlag;
        private DSWControl.DSWLabel.DSWLabel lblHandyOpeFlag;
        private DSWControl.DSWComboBox.DSWComboBox cboHandyOpeFlag;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnDisp;
        private GrapeCity.Win.ElTabelle.Sheet shtMeisai;
        private DSWControl.DSWLabel.DSWLabel lblBukkenName;
        private DSWControl.DSWComboBox.DSWComboBox cboBukkenName;
        private DSWControl.DSWLabel.DSWLabel lblTehaiNo;
        private DSWControl.DSWTextBox.DSWTextBox txtTehaiNo;
        private DSWControl.DSWLabel.DSWLabel lblShip;
        private DSWControl.DSWTextBox.DSWTextBox txtShip;
        private DSWControl.DSWLabel.DSWLabel lblWorkerName;
        private DSWControl.DSWTextBox.DSWTextBox txtWorkerName;
        private DSWControl.DSWTextBox.DSWTextBox txtSearchUserID;
    }
}