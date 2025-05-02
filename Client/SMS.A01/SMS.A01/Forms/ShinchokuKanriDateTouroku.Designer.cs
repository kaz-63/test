namespace SMS.A01.Forms
{
    partial class ShinchokuKanriDateTouroku
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShinchokuKanriDateTouroku));
            this.lblDate = new DSWControl.DSWLabel.DSWLabel();
            this.dtpDate = new DSWControl.DSWDateTimePicker();
            this.lblBiko = new DSWControl.DSWLabel.DSWLabel();
            this.txtBiko = new DSWControl.DSWTextBox.DSWTextBox();
            this.grpSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.lblDate.ChildPanel.SuspendLayout();
            this.lblDate.SuspendLayout();
            this.lblBiko.ChildPanel.SuspendLayout();
            this.lblBiko.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSearch
            // 
            resources.ApplyResources(this.grpSearch, "grpSearch");
            // 
            // btnSearch
            // 
            resources.ApplyResources(this.btnSearch, "btnSearch");
            // 
            // btnSearchAll
            // 
            resources.ApplyResources(this.btnSearchAll, "btnSearchAll");
            // 
            // btnSelect
            // 
            resources.ApplyResources(this.btnSelect, "btnSelect");
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            // 
            // shtResult
            // 
            resources.ApplyResources(this.shtResult, "shtResult");
            this.shtResult.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtResult.Data")));
            // 
            // pnlMain
            // 
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.Add(this.lblBiko);
            this.pnlMain.Controls.Add(this.lblDate);
            this.pnlMain.Controls.SetChildIndex(this.btnClose, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblDate, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblBiko, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnSelect, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtResult, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            // 
            // pnlTitle
            // 
            resources.ApplyResources(this.pnlTitle, "pnlTitle");
            // 
            // fbrFunction
            // 
            resources.ApplyResources(this.fbrFunction, "fbrFunction");
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
            // lblDate
            // 
            // 
            // lblDate.ChildPanel
            // 
            this.lblDate.ChildPanel.Controls.Add(this.dtpDate);
            this.lblDate.IsFocusChangeColor = false;
            this.lblDate.LabelWidth = 60;
            resources.ApplyResources(this.lblDate, "lblDate");
            this.lblDate.Name = "lblDate";
            this.lblDate.SplitterWidth = 0;
            // 
            // dtpDate
            // 
            resources.ApplyResources(this.dtpDate, "dtpDate");
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.NullValue = "";
            this.dtpDate.Value = null;
            // 
            // lblBiko
            // 
            // 
            // lblBiko.ChildPanel
            // 
            this.lblBiko.ChildPanel.Controls.Add(this.txtBiko);
            resources.ApplyResources(this.lblBiko, "lblBiko");
            this.lblBiko.IsFocusChangeColor = false;
            this.lblBiko.LabelWidth = 60;
            this.lblBiko.Name = "lblBiko";
            this.lblBiko.SplitterWidth = 0;
            // 
            // txtBiko
            // 
            resources.ApplyResources(this.txtBiko, "txtBiko");
            this.txtBiko.FocusBackColor = System.Drawing.Color.Empty;
            this.txtBiko.InputRegulation = "F";
            this.txtBiko.MaxByteLengthMode = true;
            this.txtBiko.Name = "txtBiko";
            this.txtBiko.OneLineMaxLength = 40;
            this.txtBiko.ProhibitionChar = null;
            // 
            // ShinchokuKanriDateTouroku
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShinchokuKanriDateTouroku";
            this.grpSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.lblDate.ChildPanel.ResumeLayout(false);
            this.lblDate.ResumeLayout(false);
            this.lblBiko.ChildPanel.ResumeLayout(false);
            this.lblBiko.ChildPanel.PerformLayout();
            this.lblBiko.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DSWControl.DSWLabel.DSWLabel lblDate;
        private DSWControl.DSWDateTimePicker dtpDate;
        private DSWControl.DSWTextBox.DSWTextBox txtBiko;
        private DSWControl.DSWLabel.DSWLabel lblBiko;
    }
}