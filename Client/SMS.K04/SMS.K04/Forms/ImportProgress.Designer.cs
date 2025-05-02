namespace SMS.K04.Forms
{
    partial class ImportProgress
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportProgress));
            this.lsvProgress = new System.Windows.Forms.ListView();
            this.colImportProgress = new System.Windows.Forms.ColumnHeader();
            this.imlImage = new System.Windows.Forms.ImageList(this.components);
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.lsvProgress);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.lsvProgress, 0);
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // lsvProgress
            // 
            resources.ApplyResources(this.lsvProgress, "lsvProgress");
            this.lsvProgress.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colImportProgress});
            this.lsvProgress.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lsvProgress.MultiSelect = false;
            this.lsvProgress.Name = "lsvProgress";
            this.lsvProgress.ShowGroups = false;
            this.lsvProgress.SmallImageList = this.imlImage;
            this.lsvProgress.UseCompatibleStateImageBehavior = false;
            this.lsvProgress.View = System.Windows.Forms.View.Details;
            // 
            // colImportProgress
            // 
            resources.ApplyResources(this.colImportProgress, "colImportProgress");
            // 
            // imlImage
            // 
            this.imlImage.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlImage.ImageStream")));
            this.imlImage.TransparentColor = System.Drawing.Color.Transparent;
            this.imlImage.Images.SetKeyName(0, "Error");
            this.imlImage.Images.SetKeyName(1, "Information");
            this.imlImage.Images.SetKeyName(2, "Question");
            this.imlImage.Images.SetKeyName(3, "Warning");
            // 
            // ImportProgress
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CloseBoxEnabled = false;
            this.Name = "ImportProgress";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lsvProgress;
        private System.Windows.Forms.ColumnHeader colImportProgress;
        private System.Windows.Forms.ImageList imlImage;
    }
}