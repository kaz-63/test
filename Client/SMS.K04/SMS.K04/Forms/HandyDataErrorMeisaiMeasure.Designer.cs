namespace SMS.K04.Forms
{
    partial class HandyDataErrorMeisaiMeasure
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HandyDataErrorMeisaiMeasure));
            this.shtMeisai = new GrapeCity.Win.ElTabelle.Sheet();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.shtMeisai);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtMeisai, 0);
            // 
            // pnlTitleSpace
            // 
            this.pnlTitleSpace.Location = new System.Drawing.Point(18, 4);
            this.pnlTitleSpace.Size = new System.Drawing.Size(684, 28);
            // 
            // shtMeisai
            // 
            this.shtMeisai.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.shtMeisai.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtMeisai.Data")));
            this.shtMeisai.Location = new System.Drawing.Point(12, 31);
            this.shtMeisai.Name = "shtMeisai";
            this.shtMeisai.Size = new System.Drawing.Size(922, 504);
            this.shtMeisai.TabIndex = 7;
            // 
            // HandyDataErrorMeisaiMeasure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 664);
            this.Name = "HandyDataErrorMeisaiMeasure";
            this.Text = "HandyDataErrorMeisaiMeasure";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GrapeCity.Win.ElTabelle.Sheet shtMeisai;
    }
}