namespace SMS.K04.Forms
{
    partial class HandyDataErrorMeisaiBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HandyDataErrorMeisaiBox));
            this.shtMeisai = new GrapeCity.Win.ElTabelle.Sheet();
            this.lblBoxNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtBoxNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).BeginInit();
            this.lblBoxNo.ChildPanel.SuspendLayout();
            this.lblBoxNo.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.lblBoxNo);
            this.pnlMain.Controls.Add(this.shtMeisai);
            this.pnlMain.Controls.SetChildIndex(this.shtMeisai, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblBoxNo, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
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
            // lblBoxNo
            // 
            // 
            // lblBoxNo.ChildPanel
            // 
            this.lblBoxNo.ChildPanel.Controls.Add(this.txtBoxNo);
            this.lblBoxNo.IsFocusChangeColor = false;
            this.lblBoxNo.LabelWidth = 80;
            resources.ApplyResources(this.lblBoxNo, "lblBoxNo");
            this.lblBoxNo.Name = "lblBoxNo";
            this.lblBoxNo.SplitterWidth = 0;
            // 
            // txtBoxNo
            // 
            resources.ApplyResources(this.txtBoxNo, "txtBoxNo");
            this.txtBoxNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtBoxNo.InputRegulation = "";
            this.txtBoxNo.Name = "txtBoxNo";
            this.txtBoxNo.ProhibitionChar = null;
            this.txtBoxNo.ReadOnly = true;
            // 
            // HandyDataErrorMeisaiBox
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HandyDataErrorMeisaiBox";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).EndInit();
            this.lblBoxNo.ChildPanel.ResumeLayout(false);
            this.lblBoxNo.ChildPanel.PerformLayout();
            this.lblBoxNo.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GrapeCity.Win.ElTabelle.Sheet shtMeisai;
        private DSWControl.DSWLabel.DSWLabel lblBoxNo;
        private DSWControl.DSWTextBox.DSWTextBox txtBoxNo;
    }
}