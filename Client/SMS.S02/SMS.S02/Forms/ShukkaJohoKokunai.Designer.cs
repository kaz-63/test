namespace SMS.S02.Forms
{
    partial class ShukkaJohoKokunai
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShukkaJohoKokunai));
            this.ChildPanel = new DSWControl.DSWLabel.CustomPanel();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.lblShukkaDate = new DSWControl.DSWLabel.DSWLabel();
            this.dtpShukkaDateFrom = new DSWControl.DSWDateTimePicker();
            this.btnStart = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.shtShipping = new GrapeCity.Win.ElTabelle.Sheet();
            this.pnlMain.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.lblShukkaDate.ChildPanel.SuspendLayout();
            this.lblShukkaDate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtShipping)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.shtShipping);
            this.pnlMain.Controls.Add(this.grpSearch);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtShipping, 0);
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
            this.grpSearch.Controls.Add(this.lblShukkaDate);
            this.grpSearch.Controls.Add(this.btnStart);
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
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
            this.dtpShukkaDateFrom.CustomFormat = global::SMS.S02.Properties.Resources.ShukkaJohoKokunai_Print;
            resources.ApplyResources(this.dtpShukkaDateFrom, "dtpShukkaDateFrom");
            this.dtpShukkaDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpShukkaDateFrom.Name = "dtpShukkaDateFrom";
            this.dtpShukkaDateFrom.NullValue = global::SMS.S02.Properties.Resources.ShukkaJohoKokunai_Print;
            this.dtpShukkaDateFrom.Value = new System.DateTime(2010, 8, 12, 19, 42, 26, 0);
            // 
            // btnStart
            // 
            resources.ApplyResources(this.btnStart, "btnStart");
            this.btnStart.Name = "btnStart";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // shtShipping
            // 
            resources.ApplyResources(this.shtShipping, "shtShipping");
            this.shtShipping.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtShipping.Data")));
            this.shtShipping.Name = "shtShipping";
            // 
            // ShukkaJohoKokunai
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ShukkaJohoKokunai";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.grpSearch.ResumeLayout(false);
            this.lblShukkaDate.ChildPanel.ResumeLayout(false);
            this.lblShukkaDate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtShipping)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSearch;
        private DSWControl.DSWLabel.DSWLabel lblShukkaDate;
        private DSWControl.DSWDateTimePicker dtpShukkaDateFrom;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnStart;
        private DSWControl.DSWLabel.CustomPanel ChildPanel;
        private GrapeCity.Win.ElTabelle.Sheet shtShipping;

    }
}