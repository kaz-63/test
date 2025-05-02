namespace SMS.M02.Forms
{
    partial class PrinterSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrinterSetting));
            this.lblNormalPrinter = new DSWControl.DSWLabel.DSWLabel();
            this.cboNormalPrinter = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblTagPrinter = new DSWControl.DSWLabel.DSWLabel();
            this.cboTagPrinter = new DSWControl.DSWComboBox.DSWComboBox();
            this.pnlMain.SuspendLayout();
            this.lblNormalPrinter.ChildPanel.SuspendLayout();
            this.lblNormalPrinter.SuspendLayout();
            this.lblTagPrinter.ChildPanel.SuspendLayout();
            this.lblTagPrinter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.lblNormalPrinter);
            this.pnlMain.Controls.Add(this.lblTagPrinter);
            this.pnlMain.Controls.SetChildIndex(this.lblTagPrinter, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblNormalPrinter, 0);
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
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // lblNormalPrinter
            // 
            // 
            // lblNormalPrinter.ChildPanel
            // 
            this.lblNormalPrinter.ChildPanel.Controls.Add(this.cboNormalPrinter);
            resources.ApplyResources(this.lblNormalPrinter, "lblNormalPrinter");
            this.lblNormalPrinter.IsFocusChangeColor = false;
            this.lblNormalPrinter.LabelWidth = 190;
            this.lblNormalPrinter.Name = "lblNormalPrinter";
            this.lblNormalPrinter.SplitterWidth = 0;
            // 
            // cboNormalPrinter
            // 
            resources.ApplyResources(this.cboNormalPrinter, "cboNormalPrinter");
            this.cboNormalPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNormalPrinter.Name = "cboNormalPrinter";
            // 
            // lblTagPrinter
            // 
            // 
            // lblTagPrinter.ChildPanel
            // 
            this.lblTagPrinter.ChildPanel.Controls.Add(this.cboTagPrinter);
            resources.ApplyResources(this.lblTagPrinter, "lblTagPrinter");
            this.lblTagPrinter.IsFocusChangeColor = false;
            this.lblTagPrinter.LabelWidth = 190;
            this.lblTagPrinter.Name = "lblTagPrinter";
            this.lblTagPrinter.SplitterWidth = 0;
            // 
            // cboTagPrinter
            // 
            resources.ApplyResources(this.cboTagPrinter, "cboTagPrinter");
            this.cboTagPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTagPrinter.Name = "cboTagPrinter";
            // 
            // PrinterSetting
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "PrinterSetting";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.lblNormalPrinter.ChildPanel.ResumeLayout(false);
            this.lblNormalPrinter.ResumeLayout(false);
            this.lblTagPrinter.ChildPanel.ResumeLayout(false);
            this.lblTagPrinter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DSWControl.DSWLabel.DSWLabel lblNormalPrinter;
        private DSWControl.DSWComboBox.DSWComboBox cboNormalPrinter;
        private DSWControl.DSWLabel.DSWLabel lblTagPrinter;
        private DSWControl.DSWComboBox.DSWComboBox cboTagPrinter;
    }
}