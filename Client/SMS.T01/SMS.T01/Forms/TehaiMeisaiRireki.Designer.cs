namespace SMS.T01.Forms
{
    partial class TehaiMeisaiRireki
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TehaiMeisaiRireki));
            this.shtMeisaiRireki = new GrapeCity.Win.ElTabelle.Sheet();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisaiRireki)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.shtMeisaiRireki);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtMeisaiRireki, 0);
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // shtMeisaiRireki
            // 
            resources.ApplyResources(this.shtMeisaiRireki, "shtMeisaiRireki");
            this.shtMeisaiRireki.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtMeisaiRireki.Data")));
            this.shtMeisaiRireki.Name = "shtMeisaiRireki";
            // 
            // TehaiMeisaiRireki
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "TehaiMeisaiRireki";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisaiRireki)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GrapeCity.Win.ElTabelle.Sheet shtMeisaiRireki;
    }
}