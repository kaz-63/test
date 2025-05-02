namespace SMS.I01.Forms
{
    partial class NyukoSettei
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NyukoSettei));
            this.shtMeisai = new GrapeCity.Win.ElTabelle.Sheet();
            this.lblBukkenName = new DSWControl.DSWLabel.DSWLabel();
            this.txtBukkenName = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblZaikoNo = new DSWControl.DSWLabel.DSWLabel();
            this.lblZaikoNoPrefix = new DSWControl.DSWLabel.DSWLabel();
            this.txtZaikoNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnDisp = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.lblBukkenNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtBukkenNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnBukkenIchiran = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblSearchShukkaFlag = new DSWControl.DSWLabel.DSWLabel();
            this.cboShukkaFlag = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblZaikoTani = new DSWControl.DSWLabel.DSWLabel();
            this.cboZaikoTani = new DSWControl.DSWComboBox.DSWComboBox();
            this.ChildPanel = new DSWControl.DSWLabel.CustomPanel();
            this.lblLocation = new DSWControl.DSWLabel.DSWLabel();
            this.cboLocation = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblStockDate = new DSWControl.DSWLabel.DSWLabel();
            this.dtpStockDate = new System.Windows.Forms.DateTimePicker();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).BeginInit();
            this.lblBukkenName.ChildPanel.SuspendLayout();
            this.lblBukkenName.SuspendLayout();
            this.lblZaikoNo.ChildPanel.SuspendLayout();
            this.lblZaikoNo.SuspendLayout();
            this.lblZaikoNoPrefix.ChildPanel.SuspendLayout();
            this.lblZaikoNoPrefix.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.lblBukkenNo.ChildPanel.SuspendLayout();
            this.lblBukkenNo.SuspendLayout();
            this.lblSearchShukkaFlag.ChildPanel.SuspendLayout();
            this.lblSearchShukkaFlag.SuspendLayout();
            this.lblZaikoTani.ChildPanel.SuspendLayout();
            this.lblZaikoTani.SuspendLayout();
            this.lblLocation.ChildPanel.SuspendLayout();
            this.lblLocation.SuspendLayout();
            this.lblStockDate.ChildPanel.SuspendLayout();
            this.lblStockDate.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.lblStockDate);
            this.pnlMain.Controls.Add(this.grpSearch);
            this.pnlMain.Controls.Add(this.shtMeisai);
            this.pnlMain.Controls.Add(this.lblLocation);
            this.pnlMain.Controls.SetChildIndex(this.lblLocation, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtMeisai, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblStockDate, 0);
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
            // txtUserName
            // 
            resources.ApplyResources(this.txtUserName, "txtUserName");
            // 
            // txtRoleName
            // 
            resources.ApplyResources(this.txtRoleName, "txtRoleName");
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
            // lblBukkenName
            // 
            // 
            // lblBukkenName.ChildPanel
            // 
            this.lblBukkenName.ChildPanel.Controls.Add(this.txtBukkenName);
            this.lblBukkenName.IsFocusChangeColor = false;
            this.lblBukkenName.IsNecessary = true;
            this.lblBukkenName.LabelWidth = 80;
            resources.ApplyResources(this.lblBukkenName, "lblBukkenName");
            this.lblBukkenName.Name = "lblBukkenName";
            this.lblBukkenName.SplitterWidth = 0;
            // 
            // txtBukkenName
            // 
            resources.ApplyResources(this.txtBukkenName, "txtBukkenName");
            this.txtBukkenName.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtBukkenName.InputRegulation = "F";
            this.txtBukkenName.MaxByteLengthMode = true;
            this.txtBukkenName.Name = "txtBukkenName";
            this.txtBukkenName.OneLineMaxLength = 60;
            this.txtBukkenName.ProhibitionChar = null;
            // 
            // lblZaikoNo
            // 
            // 
            // lblZaikoNo.ChildPanel
            // 
            this.lblZaikoNo.ChildPanel.Controls.Add(this.lblZaikoNoPrefix);
            this.lblZaikoNo.IsFocusChangeColor = false;
            this.lblZaikoNo.IsNecessary = true;
            this.lblZaikoNo.LabelWidth = 80;
            resources.ApplyResources(this.lblZaikoNo, "lblZaikoNo");
            this.lblZaikoNo.Name = "lblZaikoNo";
            this.lblZaikoNo.SplitterWidth = 0;
            // 
            // lblZaikoNoPrefix
            // 
            // 
            // lblZaikoNoPrefix.ChildPanel
            // 
            this.lblZaikoNoPrefix.ChildPanel.Controls.Add(this.txtZaikoNo);
            resources.ApplyResources(this.lblZaikoNoPrefix, "lblZaikoNoPrefix");
            this.lblZaikoNoPrefix.IsFocusChangeColor = false;
            this.lblZaikoNoPrefix.LabelWidth = 20;
            this.lblZaikoNoPrefix.Name = "lblZaikoNoPrefix";
            this.lblZaikoNoPrefix.NormalBackColor = System.Drawing.Color.LightSkyBlue;
            this.lblZaikoNoPrefix.SplitterWidth = 0;
            // 
            // txtZaikoNo
            // 
            this.txtZaikoNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            resources.ApplyResources(this.txtZaikoNo, "txtZaikoNo");
            this.txtZaikoNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtZaikoNo.InputRegulation = "nab";
            this.txtZaikoNo.IsInputRegulation = true;
            this.txtZaikoNo.Name = "txtZaikoNo";
            this.txtZaikoNo.OneLineMaxLength = 5;
            this.txtZaikoNo.ProhibitionChar = null;
            this.txtZaikoNo.Leave += new System.EventHandler(this.txtZaikoNo_Leave);
            // 
            // btnDisp
            // 
            resources.ApplyResources(this.btnDisp, "btnDisp");
            this.btnDisp.Name = "btnDisp";
            this.btnDisp.Click += new System.EventHandler(this.btnDisp_Click);
            // 
            // grpSearch
            // 
            this.grpSearch.Controls.Add(this.lblBukkenNo);
            this.grpSearch.Controls.Add(this.btnBukkenIchiran);
            this.grpSearch.Controls.Add(this.lblSearchShukkaFlag);
            this.grpSearch.Controls.Add(this.lblZaikoTani);
            this.grpSearch.Controls.Add(this.lblZaikoNo);
            this.grpSearch.Controls.Add(this.btnDisp);
            this.grpSearch.Controls.Add(this.lblBukkenName);
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
            // 
            // lblBukkenNo
            // 
            // 
            // lblBukkenNo.ChildPanel
            // 
            this.lblBukkenNo.ChildPanel.Controls.Add(this.txtBukkenNo);
            this.lblBukkenNo.IsFocusChangeColor = false;
            this.lblBukkenNo.IsNecessary = true;
            this.lblBukkenNo.LabelWidth = 80;
            resources.ApplyResources(this.lblBukkenNo, "lblBukkenNo");
            this.lblBukkenNo.Name = "lblBukkenNo";
            this.lblBukkenNo.SplitterWidth = 0;
            // 
            // txtBukkenNo
            // 
            this.txtBukkenNo.BackColor = System.Drawing.SystemColors.Control;
            this.txtBukkenNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            resources.ApplyResources(this.txtBukkenNo, "txtBukkenNo");
            this.txtBukkenNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtBukkenNo.InputRegulation = "";
            this.txtBukkenNo.Name = "txtBukkenNo";
            this.txtBukkenNo.OneLineMaxLength = 12;
            this.txtBukkenNo.ProhibitionChar = null;
            this.txtBukkenNo.ReadOnly = true;
            // 
            // btnBukkenIchiran
            // 
            resources.ApplyResources(this.btnBukkenIchiran, "btnBukkenIchiran");
            this.btnBukkenIchiran.Name = "btnBukkenIchiran";
            // 
            // lblSearchShukkaFlag
            // 
            // 
            // lblSearchShukkaFlag.ChildPanel
            // 
            this.lblSearchShukkaFlag.ChildPanel.Controls.Add(this.cboShukkaFlag);
            this.lblSearchShukkaFlag.IsFocusChangeColor = false;
            this.lblSearchShukkaFlag.IsNecessary = true;
            this.lblSearchShukkaFlag.LabelWidth = 80;
            resources.ApplyResources(this.lblSearchShukkaFlag, "lblSearchShukkaFlag");
            this.lblSearchShukkaFlag.Name = "lblSearchShukkaFlag";
            this.lblSearchShukkaFlag.SplitterWidth = 0;
            // 
            // cboShukkaFlag
            // 
            resources.ApplyResources(this.cboShukkaFlag, "cboShukkaFlag");
            this.cboShukkaFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboShukkaFlag.Name = "cboShukkaFlag";
            this.cboShukkaFlag.SelectedIndexChanged += new System.EventHandler(this.cboShukkaFlag_SelectedIndexChanged);
            // 
            // lblZaikoTani
            // 
            // 
            // lblZaikoTani.ChildPanel
            // 
            this.lblZaikoTani.ChildPanel.Controls.Add(this.cboZaikoTani);
            this.lblZaikoTani.IsFocusChangeColor = false;
            this.lblZaikoTani.IsNecessary = true;
            this.lblZaikoTani.LabelWidth = 80;
            resources.ApplyResources(this.lblZaikoTani, "lblZaikoTani");
            this.lblZaikoTani.Name = "lblZaikoTani";
            this.lblZaikoTani.SplitterWidth = 0;
            // 
            // cboZaikoTani
            // 
            resources.ApplyResources(this.cboZaikoTani, "cboZaikoTani");
            this.cboZaikoTani.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboZaikoTani.Name = "cboZaikoTani";
            this.cboZaikoTani.SelectedIndexChanged += new System.EventHandler(this.cboZaikoTani_SelectedIndexChanged);
            // 
            // lblLocation
            // 
            // 
            // lblLocation.ChildPanel
            // 
            this.lblLocation.ChildPanel.Controls.Add(this.cboLocation);
            this.lblLocation.IsFocusChangeColor = false;
            this.lblLocation.IsNecessary = true;
            this.lblLocation.LabelWidth = 80;
            resources.ApplyResources(this.lblLocation, "lblLocation");
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.SplitterWidth = 0;
            // 
            // cboLocation
            // 
            resources.ApplyResources(this.cboLocation, "cboLocation");
            this.cboLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLocation.Name = "cboLocation";
            this.cboLocation.SelectedIndexChanged += new System.EventHandler(this.cboLocation_SelectedIndexChanged);
            // 
            // lblStockDate
            // 
            // 
            // lblStockDate.ChildPanel
            // 
            this.lblStockDate.ChildPanel.Controls.Add(this.dtpStockDate);
            this.lblStockDate.IsFocusChangeColor = false;
            this.lblStockDate.LabelWidth = 80;
            resources.ApplyResources(this.lblStockDate, "lblStockDate");
            this.lblStockDate.Name = "lblStockDate";
            this.lblStockDate.SplitterWidth = 0;
            // 
            // dtpStockDate
            // 
            resources.ApplyResources(this.dtpStockDate, "dtpStockDate");
            this.dtpStockDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStockDate.Name = "dtpStockDate";
            // 
            // NyukoSettei
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "NyukoSettei";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).EndInit();
            this.lblBukkenName.ChildPanel.ResumeLayout(false);
            this.lblBukkenName.ChildPanel.PerformLayout();
            this.lblBukkenName.ResumeLayout(false);
            this.lblZaikoNo.ChildPanel.ResumeLayout(false);
            this.lblZaikoNo.ResumeLayout(false);
            this.lblZaikoNoPrefix.ChildPanel.ResumeLayout(false);
            this.lblZaikoNoPrefix.ChildPanel.PerformLayout();
            this.lblZaikoNoPrefix.ResumeLayout(false);
            this.grpSearch.ResumeLayout(false);
            this.lblBukkenNo.ChildPanel.ResumeLayout(false);
            this.lblBukkenNo.ChildPanel.PerformLayout();
            this.lblBukkenNo.ResumeLayout(false);
            this.lblSearchShukkaFlag.ChildPanel.ResumeLayout(false);
            this.lblSearchShukkaFlag.ResumeLayout(false);
            this.lblZaikoTani.ChildPanel.ResumeLayout(false);
            this.lblZaikoTani.ResumeLayout(false);
            this.lblLocation.ChildPanel.ResumeLayout(false);
            this.lblLocation.ResumeLayout(false);
            this.lblStockDate.ChildPanel.ResumeLayout(false);
            this.lblStockDate.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DSWControl.DSWLabel.DSWLabel lblZaikoNo;
        private DSWControl.DSWLabel.DSWLabel lblZaikoNoPrefix;
        private DSWControl.DSWTextBox.DSWTextBox txtZaikoNo;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnDisp;
        private DSWControl.DSWLabel.DSWLabel lblBukkenName;
        private DSWControl.DSWTextBox.DSWTextBox txtBukkenName;
        private GrapeCity.Win.ElTabelle.Sheet shtMeisai;
        private System.Windows.Forms.GroupBox grpSearch;
        private DSWControl.DSWLabel.DSWLabel lblZaikoTani;
        private DSWControl.DSWComboBox.DSWComboBox cboZaikoTani;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnBukkenIchiran;
        private DSWControl.DSWLabel.DSWLabel lblSearchShukkaFlag;
        private DSWControl.DSWComboBox.DSWComboBox cboShukkaFlag;
        private DSWControl.DSWLabel.DSWLabel lblLocation;
        private DSWControl.DSWComboBox.DSWComboBox cboLocation;
        private DSWControl.DSWLabel.DSWLabel lblBukkenNo;
        private DSWControl.DSWTextBox.DSWTextBox txtBukkenNo;
        private DSWControl.DSWLabel.CustomPanel ChildPanel;
        private DSWControl.DSWLabel.DSWLabel lblStockDate;
        private System.Windows.Forms.DateTimePicker dtpStockDate;
    }
}