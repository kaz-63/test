namespace SMS.I01.Forms
{
    partial class TanaoroshiSaiShokai
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TanaoroshiSaiShokai));
            this.shtMeisai = new GrapeCity.Win.ElTabelle.Sheet();
            this.lblBukkenName = new DSWControl.DSWLabel.DSWLabel();
            this.txtBukkenName = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnDisp = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.lblBukkenNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtBukkenNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnBukkenIchiran = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblSearchShukkaFlag = new DSWControl.DSWLabel.DSWLabel();
            this.cboShukkaFlag = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblLocation = new DSWControl.DSWLabel.DSWLabel();
            this.cboLocation = new DSWControl.DSWComboBox.DSWComboBox();
            this.ChildPanel = new DSWControl.DSWLabel.CustomPanel();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).BeginInit();
            this.lblBukkenName.ChildPanel.SuspendLayout();
            this.lblBukkenName.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.lblBukkenNo.ChildPanel.SuspendLayout();
            this.lblBukkenNo.SuspendLayout();
            this.lblSearchShukkaFlag.ChildPanel.SuspendLayout();
            this.lblSearchShukkaFlag.SuspendLayout();
            this.lblLocation.ChildPanel.SuspendLayout();
            this.lblLocation.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.grpSearch);
            this.pnlMain.Controls.Add(this.shtMeisai);
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
            // fbrFunction.F06Button
            // 
            resources.ApplyResources(this.fbrFunction.F06Button, "fbrFunction.F06Button");
            // 
            // fbrFunction.F07Button
            // 
            resources.ApplyResources(this.fbrFunction.F07Button, "fbrFunction.F07Button");
            // 
            // fbrFunction.F10Button
            // 
            resources.ApplyResources(this.fbrFunction.F10Button, "fbrFunction.F10Button");
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
            this.txtBukkenName.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtBukkenName, "txtBukkenName");
            this.txtBukkenName.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtBukkenName.InputRegulation = "F";
            this.txtBukkenName.Name = "txtBukkenName";
            this.txtBukkenName.OneLineMaxLength = 60;
            this.txtBukkenName.ProhibitionChar = null;
            this.txtBukkenName.ReadOnly = true;
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
            this.grpSearch.Controls.Add(this.lblLocation);
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
            this.txtBukkenNo.InputRegulation = global::SMS.I01.Properties.Resources.OpeRirekiShokai_sfdExcel_Title;
            this.txtBukkenNo.Name = "txtBukkenNo";
            this.txtBukkenNo.OneLineMaxLength = 12;
            this.txtBukkenNo.ProhibitionChar = null;
            this.txtBukkenNo.ReadOnly = true;
            // 
            // btnBukkenIchiran
            // 
            resources.ApplyResources(this.btnBukkenIchiran, "btnBukkenIchiran");
            this.btnBukkenIchiran.Name = "btnBukkenIchiran";
            this.btnBukkenIchiran.Click += new System.EventHandler(this.btnBukkenIchiran_Click);
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
            // 
            // TanaoroshiSaiShokai
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "TanaoroshiSaiShokai";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).EndInit();
            this.lblBukkenName.ChildPanel.ResumeLayout(false);
            this.lblBukkenName.ChildPanel.PerformLayout();
            this.lblBukkenName.ResumeLayout(false);
            this.grpSearch.ResumeLayout(false);
            this.lblBukkenNo.ChildPanel.ResumeLayout(false);
            this.lblBukkenNo.ChildPanel.PerformLayout();
            this.lblBukkenNo.ResumeLayout(false);
            this.lblSearchShukkaFlag.ChildPanel.ResumeLayout(false);
            this.lblSearchShukkaFlag.ResumeLayout(false);
            this.lblLocation.ChildPanel.ResumeLayout(false);
            this.lblLocation.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DSWControl.DSWFunctionButton.DSWFunctionButton btnDisp;
        private DSWControl.DSWLabel.DSWLabel lblBukkenName;
        private DSWControl.DSWTextBox.DSWTextBox txtBukkenName;
        private GrapeCity.Win.ElTabelle.Sheet shtMeisai;
        private System.Windows.Forms.GroupBox grpSearch;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnBukkenIchiran;
        private DSWControl.DSWLabel.DSWLabel lblSearchShukkaFlag;
        private DSWControl.DSWComboBox.DSWComboBox cboShukkaFlag;
        private DSWControl.DSWLabel.DSWLabel lblLocation;
        private DSWControl.DSWComboBox.DSWComboBox cboLocation;
        private DSWControl.DSWLabel.DSWLabel lblBukkenNo;
        private DSWControl.DSWTextBox.DSWTextBox txtBukkenNo;
        private DSWControl.DSWLabel.CustomPanel ChildPanel;
    }
}