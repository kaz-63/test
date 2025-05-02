namespace SMS.I01.Forms
{
    partial class LocationHoshu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LocationHoshu));
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.lblSearchBukkenNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtSearchBukkenNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblSearchBukkenName = new DSWControl.DSWLabel.DSWLabel();
            this.txtSearchBukkenName = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnSearchBukkenichiran = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblSearchLocation = new DSWControl.DSWLabel.DSWLabel();
            this.txtSearchLocation = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnDelete = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnUpdate = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnInsert = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblSearchShukkaFlag = new DSWControl.DSWLabel.DSWLabel();
            this.cboSearchShukkaFlag = new DSWControl.DSWComboBox.DSWComboBox();
            this.btnSearch = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.shtResult = new GrapeCity.Win.ElTabelle.Sheet();
            this.grpEdit = new System.Windows.Forms.GroupBox();
            this.lblBukkenName = new DSWControl.DSWLabel.DSWLabel();
            this.txtBukkenName = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnBukkenIchiran = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblLocation = new DSWControl.DSWLabel.DSWLabel();
            this.txtLocation = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblBukkenNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtBukkenNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblShukkaFlag = new DSWControl.DSWLabel.DSWLabel();
            this.cboShukkaFlag = new DSWControl.DSWComboBox.DSWComboBox();
            this.pnlMain.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.lblSearchBukkenNo.ChildPanel.SuspendLayout();
            this.lblSearchBukkenNo.SuspendLayout();
            this.lblSearchBukkenName.ChildPanel.SuspendLayout();
            this.lblSearchBukkenName.SuspendLayout();
            this.lblSearchLocation.ChildPanel.SuspendLayout();
            this.lblSearchLocation.SuspendLayout();
            this.lblSearchShukkaFlag.ChildPanel.SuspendLayout();
            this.lblSearchShukkaFlag.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).BeginInit();
            this.grpEdit.SuspendLayout();
            this.lblBukkenName.ChildPanel.SuspendLayout();
            this.lblBukkenName.SuspendLayout();
            this.lblLocation.ChildPanel.SuspendLayout();
            this.lblLocation.SuspendLayout();
            this.lblBukkenNo.ChildPanel.SuspendLayout();
            this.lblBukkenNo.SuspendLayout();
            this.lblShukkaFlag.ChildPanel.SuspendLayout();
            this.lblShukkaFlag.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.grpEdit);
            this.pnlMain.Controls.Add(this.grpSearch);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpEdit, 0);
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
            // grpSearch
            // 
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Controls.Add(this.lblSearchBukkenNo);
            this.grpSearch.Controls.Add(this.lblSearchBukkenName);
            this.grpSearch.Controls.Add(this.btnSearchBukkenichiran);
            this.grpSearch.Controls.Add(this.lblSearchLocation);
            this.grpSearch.Controls.Add(this.btnDelete);
            this.grpSearch.Controls.Add(this.btnUpdate);
            this.grpSearch.Controls.Add(this.btnInsert);
            this.grpSearch.Controls.Add(this.lblSearchShukkaFlag);
            this.grpSearch.Controls.Add(this.btnSearch);
            this.grpSearch.Controls.Add(this.shtResult);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
            // 
            // lblSearchBukkenNo
            // 
            // 
            // lblSearchBukkenNo.ChildPanel
            // 
            this.lblSearchBukkenNo.ChildPanel.Controls.Add(this.txtSearchBukkenNo);
            this.lblSearchBukkenNo.IsFocusChangeColor = false;
            this.lblSearchBukkenNo.IsNecessary = true;
            this.lblSearchBukkenNo.LabelWidth = 80;
            resources.ApplyResources(this.lblSearchBukkenNo, "lblSearchBukkenNo");
            this.lblSearchBukkenNo.Name = "lblSearchBukkenNo";
            this.lblSearchBukkenNo.SplitterWidth = 0;
            // 
            // txtSearchBukkenNo
            // 
            this.txtSearchBukkenNo.BackColor = System.Drawing.SystemColors.Control;
            this.txtSearchBukkenNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            resources.ApplyResources(this.txtSearchBukkenNo, "txtSearchBukkenNo");
            this.txtSearchBukkenNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtSearchBukkenNo.InputRegulation = "";
            this.txtSearchBukkenNo.Name = "txtSearchBukkenNo";
            this.txtSearchBukkenNo.OneLineMaxLength = 12;
            this.txtSearchBukkenNo.ProhibitionChar = null;
            this.txtSearchBukkenNo.ReadOnly = true;
            // 
            // lblSearchBukkenName
            // 
            // 
            // lblSearchBukkenName.ChildPanel
            // 
            this.lblSearchBukkenName.ChildPanel.Controls.Add(this.txtSearchBukkenName);
            this.lblSearchBukkenName.IsFocusChangeColor = false;
            this.lblSearchBukkenName.IsNecessary = true;
            this.lblSearchBukkenName.LabelWidth = 80;
            resources.ApplyResources(this.lblSearchBukkenName, "lblSearchBukkenName");
            this.lblSearchBukkenName.Name = "lblSearchBukkenName";
            this.lblSearchBukkenName.SplitterWidth = 0;
            // 
            // txtSearchBukkenName
            // 
            resources.ApplyResources(this.txtSearchBukkenName, "txtSearchBukkenName");
            this.txtSearchBukkenName.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtSearchBukkenName.InputRegulation = "F";
            this.txtSearchBukkenName.MaxByteLengthMode = true;
            this.txtSearchBukkenName.Name = "txtSearchBukkenName";
            this.txtSearchBukkenName.OneLineMaxLength = 60;
            this.txtSearchBukkenName.ProhibitionChar = null;
            // 
            // btnSearchBukkenichiran
            // 
            resources.ApplyResources(this.btnSearchBukkenichiran, "btnSearchBukkenichiran");
            this.btnSearchBukkenichiran.Name = "btnSearchBukkenichiran";
            // 
            // lblSearchLocation
            // 
            // 
            // lblSearchLocation.ChildPanel
            // 
            this.lblSearchLocation.ChildPanel.Controls.Add(this.txtSearchLocation);
            this.lblSearchLocation.IsFocusChangeColor = false;
            this.lblSearchLocation.LabelWidth = 80;
            resources.ApplyResources(this.lblSearchLocation, "lblSearchLocation");
            this.lblSearchLocation.Name = "lblSearchLocation";
            this.lblSearchLocation.SplitterWidth = 0;
            // 
            // txtSearchLocation
            // 
            this.txtSearchLocation.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            resources.ApplyResources(this.txtSearchLocation, "txtSearchLocation");
            this.txtSearchLocation.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtSearchLocation.InputRegulation = "";
            this.txtSearchLocation.Name = "txtSearchLocation";
            this.txtSearchLocation.OneLineMaxLength = 12;
            this.txtSearchLocation.ProhibitionChar = null;
            // 
            // btnDelete
            // 
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            resources.ApplyResources(this.btnUpdate, "btnUpdate");
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnInsert
            // 
            resources.ApplyResources(this.btnInsert, "btnInsert");
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // lblSearchShukkaFlag
            // 
            // 
            // lblSearchShukkaFlag.ChildPanel
            // 
            this.lblSearchShukkaFlag.ChildPanel.Controls.Add(this.cboSearchShukkaFlag);
            this.lblSearchShukkaFlag.IsFocusChangeColor = false;
            this.lblSearchShukkaFlag.IsNecessary = true;
            this.lblSearchShukkaFlag.LabelWidth = 80;
            resources.ApplyResources(this.lblSearchShukkaFlag, "lblSearchShukkaFlag");
            this.lblSearchShukkaFlag.Name = "lblSearchShukkaFlag";
            this.lblSearchShukkaFlag.SplitterWidth = 0;
            // 
            // cboSearchShukkaFlag
            // 
            resources.ApplyResources(this.cboSearchShukkaFlag, "cboSearchShukkaFlag");
            this.cboSearchShukkaFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSearchShukkaFlag.Name = "cboSearchShukkaFlag";
            this.cboSearchShukkaFlag.SelectedIndexChanged += new System.EventHandler(this.cboSearchShukkaFlag_SelectedIndexChanged);
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
            // grpEdit
            // 
            resources.ApplyResources(this.grpEdit, "grpEdit");
            this.grpEdit.Controls.Add(this.lblBukkenName);
            this.grpEdit.Controls.Add(this.btnBukkenIchiran);
            this.grpEdit.Controls.Add(this.lblLocation);
            this.grpEdit.Controls.Add(this.lblBukkenNo);
            this.grpEdit.Controls.Add(this.lblShukkaFlag);
            this.grpEdit.Name = "grpEdit";
            this.grpEdit.TabStop = false;
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
            this.txtBukkenName.InputRegulation = "";
            this.txtBukkenName.Name = "txtBukkenName";
            this.txtBukkenName.OneLineMaxLength = 12;
            this.txtBukkenName.ProhibitionChar = null;
            this.txtBukkenName.ReadOnly = true;
            // 
            // btnBukkenIchiran
            // 
            resources.ApplyResources(this.btnBukkenIchiran, "btnBukkenIchiran");
            this.btnBukkenIchiran.Name = "btnBukkenIchiran";
            this.btnBukkenIchiran.Click += new System.EventHandler(this.btnBukkenIchiran_Click);
            // 
            // lblLocation
            // 
            // 
            // lblLocation.ChildPanel
            // 
            this.lblLocation.ChildPanel.Controls.Add(this.txtLocation);
            this.lblLocation.IsFocusChangeColor = false;
            this.lblLocation.IsNecessary = true;
            this.lblLocation.LabelWidth = 80;
            resources.ApplyResources(this.lblLocation, "lblLocation");
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.SplitterWidth = 0;
            // 
            // txtLocation
            // 
            this.txtLocation.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            resources.ApplyResources(this.txtLocation, "txtLocation");
            this.txtLocation.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtLocation.InputRegulation = "abr";
            this.txtLocation.IsInputRegulation = true;
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.OneLineMaxLength = 12;
            this.txtLocation.ProhibitionChar = ((System.Collections.Generic.Dictionary<int, string>)(resources.GetObject("txtLocation.ProhibitionChar")));
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
            resources.ApplyResources(this.txtBukkenNo, "txtBukkenNo");
            this.txtBukkenNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtBukkenNo.InputRegulation = "n";
            this.txtBukkenNo.IsInputRegulation = true;
            this.txtBukkenNo.MaxByteLengthMode = true;
            this.txtBukkenNo.Name = "txtBukkenNo";
            this.txtBukkenNo.OneLineMaxLength = 10;
            this.txtBukkenNo.ProhibitionChar = null;
            this.txtBukkenNo.ReadOnly = true;
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
            this.cboShukkaFlag.SelectedIndexChanged += new System.EventHandler(this.cboShukkaFlag_SelectedIndexChanged);
            // 
            // LocationHoshu
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "LocationHoshu";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.grpSearch.ResumeLayout(false);
            this.lblSearchBukkenNo.ChildPanel.ResumeLayout(false);
            this.lblSearchBukkenNo.ChildPanel.PerformLayout();
            this.lblSearchBukkenNo.ResumeLayout(false);
            this.lblSearchBukkenName.ChildPanel.ResumeLayout(false);
            this.lblSearchBukkenName.ChildPanel.PerformLayout();
            this.lblSearchBukkenName.ResumeLayout(false);
            this.lblSearchLocation.ChildPanel.ResumeLayout(false);
            this.lblSearchLocation.ChildPanel.PerformLayout();
            this.lblSearchLocation.ResumeLayout(false);
            this.lblSearchShukkaFlag.ChildPanel.ResumeLayout(false);
            this.lblSearchShukkaFlag.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).EndInit();
            this.grpEdit.ResumeLayout(false);
            this.lblBukkenName.ChildPanel.ResumeLayout(false);
            this.lblBukkenName.ChildPanel.PerformLayout();
            this.lblBukkenName.ResumeLayout(false);
            this.lblLocation.ChildPanel.ResumeLayout(false);
            this.lblLocation.ChildPanel.PerformLayout();
            this.lblLocation.ResumeLayout(false);
            this.lblBukkenNo.ChildPanel.ResumeLayout(false);
            this.lblBukkenNo.ChildPanel.PerformLayout();
            this.lblBukkenNo.ResumeLayout(false);
            this.lblShukkaFlag.ChildPanel.ResumeLayout(false);
            this.lblShukkaFlag.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSearch;
        private System.Windows.Forms.GroupBox grpEdit;
        private DSWControl.DSWLabel.DSWLabel lblSearchShukkaFlag;
        private DSWControl.DSWLabel.DSWLabel lblShukkaFlag;
        private DSWControl.DSWComboBox.DSWComboBox cboSearchShukkaFlag;
        private GrapeCity.Win.ElTabelle.Sheet shtResult;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnSearch;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnDelete;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnUpdate;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnInsert;
        private DSWControl.DSWComboBox.DSWComboBox cboShukkaFlag;
        private DSWControl.DSWLabel.DSWLabel lblBukkenNo;
        private DSWControl.DSWTextBox.DSWTextBox txtBukkenNo;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnBukkenIchiran;
        private DSWControl.DSWLabel.DSWLabel lblLocation;
        private DSWControl.DSWTextBox.DSWTextBox txtLocation;
        private DSWControl.DSWLabel.DSWLabel lblSearchLocation;
        private DSWControl.DSWTextBox.DSWTextBox txtSearchLocation;
        private DSWControl.DSWLabel.DSWLabel lblSearchBukkenNo;
        private DSWControl.DSWTextBox.DSWTextBox txtSearchBukkenNo;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnSearchBukkenichiran;
        private DSWControl.DSWLabel.DSWLabel lblBukkenName;
        private DSWControl.DSWTextBox.DSWTextBox txtBukkenName;
        private DSWControl.DSWLabel.DSWLabel lblSearchBukkenName;
        private DSWControl.DSWTextBox.DSWTextBox txtSearchBukkenName;
    }
}