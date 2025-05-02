namespace ActiveReportsHelper
{
    partial class ReportPreview
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportPreview));
            this.vwrView = new DataDynamics.ActiveReports.Viewer.Viewer();
            this.SuspendLayout();
            // 
            // vwrView
            // 
            this.vwrView.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.vwrView, "vwrView");
            this.vwrView.Document = new DataDynamics.ActiveReports.Document.Document("ARNet Document");
            this.vwrView.Name = "vwrView";
            this.vwrView.ReportViewer.CurrentPage = 0;
            this.vwrView.ReportViewer.MultiplePageCols = 3;
            this.vwrView.ReportViewer.MultiplePageRows = 2;
            this.vwrView.ReportViewer.ViewType = DataDynamics.ActiveReports.Viewer.ViewType.Normal;
            this.vwrView.TableOfContents.Text = "見出し一覧";
            this.vwrView.TableOfContents.Width = 200;
            this.vwrView.TabTitleLength = 35;
            this.vwrView.Toolbar.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.vwrView.ToolClick += new DataDynamics.ActiveReports.Toolbar.ToolClickEventHandler(this.vwrView_ToolClick);
            // 
            // ReportPreview
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.vwrView);
            this.Name = "ReportPreview";
            this.ResumeLayout(false);

        }

        #endregion

        private DataDynamics.ActiveReports.Viewer.Viewer vwrView;
    }
}