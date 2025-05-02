namespace SMS.P02.Forms
{
    partial class BukkenMeiIchiran
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BukkenMeiIchiran));
            this.cboShukkaFlag = new DSWControl.DSWComboBox.DSWComboBox();
            this.txtBukkenName = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblShukkaFlag = new DSWControl.DSWLabel.DSWLabel();
            this.lblBukkenName = new DSWControl.DSWLabel.DSWLabel();
            this.grpSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.lblShukkaFlag.ChildPanel.SuspendLayout();
            this.lblShukkaFlag.SuspendLayout();
            this.lblBukkenName.ChildPanel.SuspendLayout();
            this.lblBukkenName.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSearch
            // 
            this.grpSearch.Controls.Add(this.lblBukkenName);
            this.grpSearch.Controls.Add(this.lblShukkaFlag);
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Controls.SetChildIndex(this.lblShukkaFlag, 0);
            this.grpSearch.Controls.SetChildIndex(this.btnSearch, 0);
            this.grpSearch.Controls.SetChildIndex(this.lblBukkenName, 0);
            this.grpSearch.Controls.SetChildIndex(this.btnSearchAll, 0);
            // 
            // btnSearch
            // 
            resources.ApplyResources(this.btnSearch, "btnSearch");
            // 
            // shtResult
            // 
            this.shtResult.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtResult.Data")));
            resources.ApplyResources(this.shtResult, "shtResult");
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // cboShukkaFlag
            // 
            resources.ApplyResources(this.cboShukkaFlag, "cboShukkaFlag");
            this.cboShukkaFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboShukkaFlag.Name = "cboShukkaFlag";
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
            this.txtBukkenName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBukkenName_KeyDown);
            // 
            // lblShukkaFlag
            // 
            // 
            // lblShukkaFlag.ChildPanel
            // 
            this.lblShukkaFlag.ChildPanel.Controls.Add(this.cboShukkaFlag);
            this.lblShukkaFlag.IsFocusChangeColor = false;
            this.lblShukkaFlag.LabelWidth = 80;
            resources.ApplyResources(this.lblShukkaFlag, "lblShukkaFlag");
            this.lblShukkaFlag.Name = "lblShukkaFlag";
            this.lblShukkaFlag.SplitterWidth = 0;
            // 
            // lblBukkenName
            // 
            // 
            // lblBukkenName.ChildPanel
            // 
            this.lblBukkenName.ChildPanel.Controls.Add(this.txtBukkenName);
            this.lblBukkenName.IsFocusChangeColor = false;
            this.lblBukkenName.LabelWidth = 100;
            resources.ApplyResources(this.lblBukkenName, "lblBukkenName");
            this.lblBukkenName.Name = "lblBukkenName";
            this.lblBukkenName.SplitterWidth = 0;
            // 
            // BukkenMeiIchiran
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BukkenMeiIchiran";
            this.grpSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.lblShukkaFlag.ChildPanel.ResumeLayout(false);
            this.lblShukkaFlag.ResumeLayout(false);
            this.lblBukkenName.ChildPanel.ResumeLayout(false);
            this.lblBukkenName.ChildPanel.PerformLayout();
            this.lblBukkenName.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DSWControl.DSWLabel.DSWLabel lblBukkenName;
        private DSWControl.DSWTextBox.DSWTextBox txtBukkenName;
        private DSWControl.DSWLabel.DSWLabel lblShukkaFlag;
        private DSWControl.DSWComboBox.DSWComboBox cboShukkaFlag;
    }
}