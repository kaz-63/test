namespace SMS.K04.Forms
{
    partial class HandyDataErrorMeisaiPallet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HandyDataErrorMeisaiPallet));
            this.lblPalletNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtPalletNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.shtMeisai = new GrapeCity.Win.ElTabelle.Sheet();
            this.pnlMain.SuspendLayout();
            this.lblPalletNo.ChildPanel.SuspendLayout();
            this.lblPalletNo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.lblPalletNo);
            this.pnlMain.Controls.Add(this.shtMeisai);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtMeisai, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblPalletNo, 0);
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // lblPalletNo
            // 
            // 
            // lblPalletNo.ChildPanel
            // 
            this.lblPalletNo.ChildPanel.Controls.Add(this.txtPalletNo);
            this.lblPalletNo.IsFocusChangeColor = false;
            this.lblPalletNo.LabelWidth = 80;
            resources.ApplyResources(this.lblPalletNo, "lblPalletNo");
            this.lblPalletNo.Name = "lblPalletNo";
            this.lblPalletNo.SplitterWidth = 0;
            // 
            // txtPalletNo
            // 
            resources.ApplyResources(this.txtPalletNo, "txtPalletNo");
            this.txtPalletNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtPalletNo.InputRegulation = "";
            this.txtPalletNo.Name = "txtPalletNo";
            this.txtPalletNo.ProhibitionChar = null;
            this.txtPalletNo.ReadOnly = true;
            // 
            // shtMeisai
            // 
            resources.ApplyResources(this.shtMeisai, "shtMeisai");
            this.shtMeisai.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtMeisai.Data")));
            this.shtMeisai.Name = "shtMeisai";
            // 
            // HandyDataErrorMeisaiPallet
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HandyDataErrorMeisaiPallet";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.lblPalletNo.ChildPanel.ResumeLayout(false);
            this.lblPalletNo.ChildPanel.PerformLayout();
            this.lblPalletNo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DSWControl.DSWLabel.DSWLabel lblPalletNo;
        private DSWControl.DSWTextBox.DSWTextBox txtPalletNo;
        private GrapeCity.Win.ElTabelle.Sheet shtMeisai;

    }
}