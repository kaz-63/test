namespace SMS.M01.Forms
{
    partial class MailSendInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MailSendInfo));
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.dtpDateTo = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDateFrom = new DSWControl.DSWLabel.DSWLabel();
            this.dtpDateFrom = new System.Windows.Forms.DateTimePicker();
            this.lblMailStatus = new DSWControl.DSWLabel.DSWLabel();
            this.cboMailStatus = new DSWControl.DSWComboBox.DSWComboBox();
            this.btnDisp = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.shtMeisai = new GrapeCity.Win.ElTabelle.Sheet();
            this.pnlMain.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.lblDateFrom.ChildPanel.SuspendLayout();
            this.lblDateFrom.SuspendLayout();
            this.lblMailStatus.ChildPanel.SuspendLayout();
            this.lblMailStatus.SuspendLayout();
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
            // fbrFunction.F01Button
            // 
            resources.ApplyResources(this.fbrFunction.F01Button, "fbrFunction.F01Button");
            resources.ApplyResources(this.fbrFunction, "fbrFunction");
            // 
            // fbrFunction.F04Button
            // 
            resources.ApplyResources(this.fbrFunction.F04Button, "fbrFunction.F04Button");
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
            this.grpSearch.Controls.Add(this.dtpDateTo);
            this.grpSearch.Controls.Add(this.label1);
            this.grpSearch.Controls.Add(this.lblDateFrom);
            this.grpSearch.Controls.Add(this.lblMailStatus);
            this.grpSearch.Controls.Add(this.btnDisp);
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
            // 
            // dtpDateTo
            // 
            this.dtpDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpDateTo, "dtpDateTo");
            this.dtpDateTo.Name = "dtpDateTo";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lblDateFrom
            // 
            // 
            // lblDateFrom.ChildPanel
            // 
            this.lblDateFrom.ChildPanel.Controls.Add(this.dtpDateFrom);
            this.lblDateFrom.IsFocusChangeColor = false;
            this.lblDateFrom.LabelWidth = 80;
            resources.ApplyResources(this.lblDateFrom, "lblDateFrom");
            this.lblDateFrom.Name = "lblDateFrom";
            this.lblDateFrom.SplitterWidth = 0;
            // 
            // dtpDateFrom
            // 
            resources.ApplyResources(this.dtpDateFrom, "dtpDateFrom");
            this.dtpDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDateFrom.Name = "dtpDateFrom";
            // 
            // lblMailStatus
            // 
            // 
            // lblMailStatus.ChildPanel
            // 
            this.lblMailStatus.ChildPanel.Controls.Add(this.cboMailStatus);
            this.lblMailStatus.IsFocusChangeColor = false;
            this.lblMailStatus.IsNecessary = true;
            this.lblMailStatus.LabelWidth = 80;
            resources.ApplyResources(this.lblMailStatus, "lblMailStatus");
            this.lblMailStatus.Name = "lblMailStatus";
            this.lblMailStatus.SplitterWidth = 0;
            // 
            // cboMailStatus
            // 
            resources.ApplyResources(this.cboMailStatus, "cboMailStatus");
            this.cboMailStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMailStatus.Name = "cboMailStatus";
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
            // MailSendInfo
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "MailSendInfo";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.grpSearch.ResumeLayout(false);
            this.grpSearch.PerformLayout();
            this.lblDateFrom.ChildPanel.ResumeLayout(false);
            this.lblDateFrom.ResumeLayout(false);
            this.lblMailStatus.ChildPanel.ResumeLayout(false);
            this.lblMailStatus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSearch;
        private System.Windows.Forms.DateTimePicker dtpDateTo;
        private System.Windows.Forms.Label label1;
        private DSWControl.DSWLabel.DSWLabel lblDateFrom;
        private System.Windows.Forms.DateTimePicker dtpDateFrom;
        private DSWControl.DSWLabel.DSWLabel lblMailStatus;
        private DSWControl.DSWComboBox.DSWComboBox cboMailStatus;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnDisp;
        private GrapeCity.Win.ElTabelle.Sheet shtMeisai;

    }
}