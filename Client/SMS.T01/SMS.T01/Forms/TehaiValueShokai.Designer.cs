namespace SMS.T01.Forms
{
    partial class TehaiValueShokai
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TehaiValueShokai));
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.lblBukkenName = new DSWControl.DSWLabel.DSWLabel();
            this.cboBukkenName = new DSWControl.DSWComboBox.DSWComboBox();
            this.btnSearch = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.shtResult = new GrapeCity.Win.ElTabelle.Sheet();
            this.grpEdit = new System.Windows.Forms.GroupBox();
            this.lblRemainingValueTotal = new DSWControl.DSWLabel.DSWLabel();
            this.txtRemainingValueTotal = new DSWControl.DSWNumericTextBox();
            this.lblTotalUsedValue = new DSWControl.DSWLabel.DSWLabel();
            this.txtlblTotalUsedValue = new DSWControl.DSWNumericTextBox();
            this.lblTotalPOAmount = new DSWControl.DSWLabel.DSWLabel();
            this.txtTotalPOAmount = new DSWControl.DSWNumericTextBox();
            this.lblTotalPartitionAmount = new DSWControl.DSWLabel.DSWLabel();
            this.txtTotalPartitionAmount = new DSWControl.DSWNumericTextBox();
            this.dswTextBox1 = new DSWControl.DSWTextBox.DSWTextBox();
            this.ChildPanel = new DSWControl.DSWLabel.CustomPanel();
            this.lbCurrency1 = new System.Windows.Forms.Label();
            this.lbCurrency2 = new System.Windows.Forms.Label();
            this.lbCurrency3 = new System.Windows.Forms.Label();
            this.lbCurrency4 = new System.Windows.Forms.Label();
            this.pnlMain.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.lblBukkenName.ChildPanel.SuspendLayout();
            this.lblBukkenName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).BeginInit();
            this.grpEdit.SuspendLayout();
            this.lblRemainingValueTotal.ChildPanel.SuspendLayout();
            this.lblRemainingValueTotal.SuspendLayout();
            this.lblTotalUsedValue.ChildPanel.SuspendLayout();
            this.lblTotalUsedValue.SuspendLayout();
            this.lblTotalPOAmount.ChildPanel.SuspendLayout();
            this.lblTotalPOAmount.SuspendLayout();
            this.lblTotalPartitionAmount.ChildPanel.SuspendLayout();
            this.lblTotalPartitionAmount.SuspendLayout();
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
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Controls.Add(this.lblBukkenName);
            this.grpSearch.Controls.Add(this.btnSearch);
            this.grpSearch.Controls.Add(this.shtResult);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
            // 
            // lblBukkenName
            // 
            // 
            // lblBukkenName.ChildPanel
            // 
            this.lblBukkenName.ChildPanel.Controls.Add(this.cboBukkenName);
            this.lblBukkenName.IsFocusChangeColor = false;
            this.lblBukkenName.IsNecessary = true;
            this.lblBukkenName.LabelWidth = 90;
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
            this.grpEdit.Controls.Add(this.lbCurrency4);
            this.grpEdit.Controls.Add(this.lbCurrency3);
            this.grpEdit.Controls.Add(this.lbCurrency2);
            this.grpEdit.Controls.Add(this.lbCurrency1);
            this.grpEdit.Controls.Add(this.lblRemainingValueTotal);
            this.grpEdit.Controls.Add(this.lblTotalUsedValue);
            this.grpEdit.Controls.Add(this.lblTotalPOAmount);
            this.grpEdit.Controls.Add(this.lblTotalPartitionAmount);
            this.grpEdit.Name = "grpEdit";
            this.grpEdit.TabStop = false;
            // 
            // lblRemainingValueTotal
            // 
            // 
            // lblRemainingValueTotal.ChildPanel
            // 
            this.lblRemainingValueTotal.ChildPanel.Controls.Add(this.txtRemainingValueTotal);
            this.lblRemainingValueTotal.IsFocusChangeColor = false;
            this.lblRemainingValueTotal.IsNecessary = true;
            this.lblRemainingValueTotal.LabelWidth = 150;
            resources.ApplyResources(this.lblRemainingValueTotal, "lblRemainingValueTotal");
            this.lblRemainingValueTotal.Name = "lblRemainingValueTotal";
            this.lblRemainingValueTotal.NecessaryBackColor = System.Drawing.Color.LightSkyBlue;
            this.lblRemainingValueTotal.SplitterWidth = 0;
            // 
            // txtRemainingValueTotal
            // 
            this.txtRemainingValueTotal.AllowEmpty = true;
            this.txtRemainingValueTotal.AllowNegative = true;
            resources.ApplyResources(this.txtRemainingValueTotal, "txtRemainingValueTotal");
            this.txtRemainingValueTotal.IsThousands = true;
            this.txtRemainingValueTotal.MaxValue = new decimal(new int[] {
            -1530494977,
            232830,
            0,
            196608});
            this.txtRemainingValueTotal.MinValue = new decimal(new int[] {
            -1530494977,
            232830,
            0,
            -2147287040});
            this.txtRemainingValueTotal.Name = "txtRemainingValueTotal";
            this.txtRemainingValueTotal.ReadOnly = true;
            // 
            // lblTotalUsedValue
            // 
            // 
            // lblTotalUsedValue.ChildPanel
            // 
            this.lblTotalUsedValue.ChildPanel.Controls.Add(this.txtlblTotalUsedValue);
            this.lblTotalUsedValue.IsFocusChangeColor = false;
            this.lblTotalUsedValue.IsNecessary = true;
            this.lblTotalUsedValue.LabelWidth = 150;
            resources.ApplyResources(this.lblTotalUsedValue, "lblTotalUsedValue");
            this.lblTotalUsedValue.Name = "lblTotalUsedValue";
            this.lblTotalUsedValue.NecessaryBackColor = System.Drawing.Color.LightSkyBlue;
            this.lblTotalUsedValue.SplitterWidth = 0;
            // 
            // txtlblTotalUsedValue
            // 
            this.txtlblTotalUsedValue.AllowEmpty = true;
            this.txtlblTotalUsedValue.AllowNegative = true;
            resources.ApplyResources(this.txtlblTotalUsedValue, "txtlblTotalUsedValue");
            this.txtlblTotalUsedValue.IsThousands = true;
            this.txtlblTotalUsedValue.MaxValue = new decimal(new int[] {
            -1530494977,
            232830,
            0,
            196608});
            this.txtlblTotalUsedValue.MinValue = new decimal(new int[] {
            -1530494977,
            232830,
            0,
            -2147287040});
            this.txtlblTotalUsedValue.Name = "txtlblTotalUsedValue";
            this.txtlblTotalUsedValue.ReadOnly = true;
            // 
            // lblTotalPOAmount
            // 
            // 
            // lblTotalPOAmount.ChildPanel
            // 
            this.lblTotalPOAmount.ChildPanel.Controls.Add(this.txtTotalPOAmount);
            this.lblTotalPOAmount.IsFocusChangeColor = false;
            this.lblTotalPOAmount.IsNecessary = true;
            this.lblTotalPOAmount.LabelWidth = 150;
            resources.ApplyResources(this.lblTotalPOAmount, "lblTotalPOAmount");
            this.lblTotalPOAmount.Name = "lblTotalPOAmount";
            this.lblTotalPOAmount.NecessaryBackColor = System.Drawing.Color.LightSkyBlue;
            this.lblTotalPOAmount.SplitterWidth = 0;
            // 
            // txtTotalPOAmount
            // 
            this.txtTotalPOAmount.AllowEmpty = true;
            this.txtTotalPOAmount.AllowNegative = true;
            resources.ApplyResources(this.txtTotalPOAmount, "txtTotalPOAmount");
            this.txtTotalPOAmount.IsThousands = true;
            this.txtTotalPOAmount.MaxValue = new decimal(new int[] {
            -1530494977,
            232830,
            0,
            196608});
            this.txtTotalPOAmount.MinValue = new decimal(new int[] {
            -1530494977,
            232830,
            0,
            -2147287040});
            this.txtTotalPOAmount.Name = "txtTotalPOAmount";
            this.txtTotalPOAmount.ReadOnly = true;
            // 
            // lblTotalPartitionAmount
            // 
            // 
            // lblTotalPartitionAmount.ChildPanel
            // 
            this.lblTotalPartitionAmount.ChildPanel.Controls.Add(this.txtTotalPartitionAmount);
            this.lblTotalPartitionAmount.IsFocusChangeColor = false;
            this.lblTotalPartitionAmount.IsNecessary = true;
            this.lblTotalPartitionAmount.LabelWidth = 150;
            resources.ApplyResources(this.lblTotalPartitionAmount, "lblTotalPartitionAmount");
            this.lblTotalPartitionAmount.Name = "lblTotalPartitionAmount";
            this.lblTotalPartitionAmount.NecessaryBackColor = System.Drawing.Color.LightSkyBlue;
            this.lblTotalPartitionAmount.SplitterWidth = 0;
            // 
            // txtTotalPartitionAmount
            // 
            this.txtTotalPartitionAmount.AllowEmpty = true;
            this.txtTotalPartitionAmount.AllowNegative = true;
            resources.ApplyResources(this.txtTotalPartitionAmount, "txtTotalPartitionAmount");
            this.txtTotalPartitionAmount.IsThousands = true;
            this.txtTotalPartitionAmount.MaxValue = new decimal(new int[] {
            -1530494977,
            232830,
            0,
            196608});
            this.txtTotalPartitionAmount.MinValue = new decimal(new int[] {
            -1530494977,
            232830,
            0,
            -2147287040});
            this.txtTotalPartitionAmount.Name = "txtTotalPartitionAmount";
            this.txtTotalPartitionAmount.ReadOnly = true;
            // 
            // dswTextBox1
            // 
            resources.ApplyResources(this.dswTextBox1, "dswTextBox1");
            this.dswTextBox1.FocusBackColor = System.Drawing.SystemColors.Window;
            this.dswTextBox1.InputRegulation = "F";
            this.dswTextBox1.MaxByteLengthMode = true;
            this.dswTextBox1.Name = "dswTextBox1";
            this.dswTextBox1.OneLineMaxLength = 40;
            this.dswTextBox1.ProhibitionChar = null;
            // 
            // lbCurrency1
            // 
            resources.ApplyResources(this.lbCurrency1, "lbCurrency1");
            this.lbCurrency1.Name = "lbCurrency1";
            // 
            // lbCurrency2
            // 
            resources.ApplyResources(this.lbCurrency2, "lbCurrency2");
            this.lbCurrency2.Name = "lbCurrency2";
            // 
            // lbCurrency3
            // 
            resources.ApplyResources(this.lbCurrency3, "lbCurrency3");
            this.lbCurrency3.Name = "lbCurrency3";
            // 
            // lbCurrency4
            // 
            resources.ApplyResources(this.lbCurrency4, "lbCurrency4");
            this.lbCurrency4.Name = "lbCurrency4";
            // 
            // TehaiValueShokai
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "TehaiValueShokai";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.grpSearch.ResumeLayout(false);
            this.lblBukkenName.ChildPanel.ResumeLayout(false);
            this.lblBukkenName.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).EndInit();
            this.grpEdit.ResumeLayout(false);
            this.grpEdit.PerformLayout();
            this.lblRemainingValueTotal.ChildPanel.ResumeLayout(false);
            this.lblRemainingValueTotal.ChildPanel.PerformLayout();
            this.lblRemainingValueTotal.ResumeLayout(false);
            this.lblTotalUsedValue.ChildPanel.ResumeLayout(false);
            this.lblTotalUsedValue.ChildPanel.PerformLayout();
            this.lblTotalUsedValue.ResumeLayout(false);
            this.lblTotalPOAmount.ChildPanel.ResumeLayout(false);
            this.lblTotalPOAmount.ChildPanel.PerformLayout();
            this.lblTotalPOAmount.ResumeLayout(false);
            this.lblTotalPartitionAmount.ChildPanel.ResumeLayout(false);
            this.lblTotalPartitionAmount.ChildPanel.PerformLayout();
            this.lblTotalPartitionAmount.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSearch;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnSearch;
        private GrapeCity.Win.ElTabelle.Sheet shtResult;
        private System.Windows.Forms.GroupBox grpEdit;
        private DSWControl.DSWLabel.DSWLabel lblTotalPartitionAmount;
        private DSWControl.DSWLabel.DSWLabel lblTotalPOAmount;
        private DSWControl.DSWTextBox.DSWTextBox dswTextBox1;
        private DSWControl.DSWLabel.DSWLabel lblRemainingValueTotal;
        private DSWControl.DSWLabel.DSWLabel lblTotalUsedValue;
        private DSWControl.DSWLabel.DSWLabel lblBukkenName;
        private DSWControl.DSWComboBox.DSWComboBox cboBukkenName;
        private DSWControl.DSWLabel.CustomPanel ChildPanel;
        private DSWControl.DSWNumericTextBox txtRemainingValueTotal;
        private DSWControl.DSWNumericTextBox txtlblTotalUsedValue;
        private DSWControl.DSWNumericTextBox txtTotalPOAmount;
        private DSWControl.DSWNumericTextBox txtTotalPartitionAmount;
        private System.Windows.Forms.Label lbCurrency1;
        private System.Windows.Forms.Label lbCurrency4;
        private System.Windows.Forms.Label lbCurrency3;
        private System.Windows.Forms.Label lbCurrency2;

    }
}