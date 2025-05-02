namespace SMS.R01
{
    /// <summary>
    /// RepBoxTagList の概要の説明です。
    /// </summary>
    partial class RepPalletTagList
    {
        private DataDynamics.ActiveReports.PageHeader pageHeader;
        private DataDynamics.ActiveReports.Detail detail;
        private DataDynamics.ActiveReports.PageFooter pageFooter;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }

        #region ActiveReport デザイナで生成されたコード
        /// <summary>
        /// デザイナ サポートに必要なメソッドです。
        /// このメソッドの内容をコード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RepPalletTagList));
            this.shape2 = new DataDynamics.ActiveReports.Shape();
            this.barcode1 = new DataDynamics.ActiveReports.Barcode();
            this.label5 = new DataDynamics.ActiveReports.Label();
            this.label6 = new DataDynamics.ActiveReports.Label();
            this.label7 = new DataDynamics.ActiveReports.Label();
            this.label8 = new DataDynamics.ActiveReports.Label();
            this.line9 = new DataDynamics.ActiveReports.Line();
            this.line10 = new DataDynamics.ActiveReports.Line();
            this.label1 = new DataDynamics.ActiveReports.Label();
            this.pageHeader = new DataDynamics.ActiveReports.PageHeader();
            this.detail = new DataDynamics.ActiveReports.Detail();
            this.pageFooter = new DataDynamics.ActiveReports.PageFooter();
            this.groupHeader1 = new DataDynamics.ActiveReports.GroupHeader();
            this.groupFooter1 = new DataDynamics.ActiveReports.GroupFooter();
            ((System.ComponentModel.ISupportInitialize)(this.label5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // shape2
            // 
            resources.ApplyResources(this.shape2, "shape2");
            this.shape2.Name = "shape2";
            this.shape2.RoundingRadius = 9.999999F;
            // 
            // barcode1
            // 
            this.barcode1.CheckSumEnabled = false;
            this.barcode1.DataField = "PALLET_NO";
            this.barcode1.Font = new System.Drawing.Font("Courier New", 8F);
            resources.ApplyResources(this.barcode1, "barcode1");
            this.barcode1.Name = "barcode1";
            this.barcode1.Style = DataDynamics.ActiveReports.BarCodeStyle.Code39;
            // 
            // label5
            // 
            this.label5.DataField = "NONYUSAKI_NAME";
            resources.ApplyResources(this.label5, "label5");
            this.label5.HyperLink = null;
            this.label5.MultiLine = false;
            this.label5.Name = "label5";
            // 
            // label6
            // 
            this.label6.DataField = "SHIP";
            resources.ApplyResources(this.label6, "label6");
            this.label6.HyperLink = null;
            this.label6.Name = "label6";
            // 
            // label7
            // 
            this.label7.DataField = "PALLET_NO";
            resources.ApplyResources(this.label7, "label7");
            this.label7.HyperLink = null;
            this.label7.Name = "label7";
            // 
            // label8
            // 
            this.label8.DataField = "COUNT_BOX";
            resources.ApplyResources(this.label8, "label8");
            this.label8.HyperLink = null;
            this.label8.Name = "label8";
            // 
            // line9
            // 
            resources.ApplyResources(this.line9, "line9");
            this.line9.LineWeight = 1F;
            this.line9.Name = "line9";
            this.line9.X1 = 0.2F;
            this.line9.X2 = 11.2F;
            this.line9.Y1 = 2F;
            this.line9.Y2 = 2F;
            // 
            // line10
            // 
            resources.ApplyResources(this.line10, "line10");
            this.line10.LineWeight = 1F;
            this.line10.Name = "line10";
            this.line10.X1 = 0.2F;
            this.line10.X2 = 11.2F;
            this.line10.Y1 = 5.998F;
            this.line10.Y2 = 6F;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.HyperLink = null;
            this.label1.Name = "label1";
            // 
            // pageHeader
            // 
            this.pageHeader.Height = 0F;
            this.pageHeader.Name = "pageHeader";
            // 
            // detail
            // 
            this.detail.CanGrow = false;
            this.detail.ColumnSpacing = 0F;
            this.detail.Height = 0F;
            this.detail.Name = "detail";
            // 
            // pageFooter
            // 
            this.pageFooter.Height = 0F;
            this.pageFooter.Name = "pageFooter";
            // 
            // groupHeader1
            // 
            this.groupHeader1.CanShrink = true;
            this.groupHeader1.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
            this.shape2,
            this.barcode1,
            this.label5,
            this.label6,
            this.label7,
            this.label8,
            this.line9,
            this.line10,
            this.label1});
            this.groupHeader1.Height = 8F;
            this.groupHeader1.Name = "groupHeader1";
            this.groupHeader1.RepeatStyle = DataDynamics.ActiveReports.RepeatStyle.OnPage;
            this.groupHeader1.UnderlayNext = true;
            this.groupHeader1.Format += new System.EventHandler(this.RepPalletTagList_ReportStart);
            // 
            // groupFooter1
            // 
            this.groupFooter1.Height = 0F;
            this.groupFooter1.Name = "groupFooter1";
            // 
            // RepPalletTagList
            // 
            this.MasterReport = false;
            this.PageSettings.DefaultPaperSize = false;
            this.PageSettings.Margins.Bottom = 0.13F;
            this.PageSettings.Margins.Left = 0.14F;
            this.PageSettings.Margins.Right = 0.14F;
            this.PageSettings.Margins.Top = 0.13F;
            this.PageSettings.Orientation = DataDynamics.ActiveReports.Document.PageOrientation.Landscape;
            this.PageSettings.PaperHeight = 11.69291F;
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.PageSettings.PaperWidth = 8.267716F;
            this.PrintWidth = 11.4F;
            this.Sections.Add(this.pageHeader);
            this.Sections.Add(this.groupHeader1);
            this.Sections.Add(this.detail);
            this.Sections.Add(this.groupFooter1);
            this.Sections.Add(this.pageFooter);
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; font-size: 10pt; " +
                        "color: Black; font-family: MS UI Gothic; ddo-char-set: 128", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold", "Heading2", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"));
            ((System.ComponentModel.ISupportInitialize)(this.label5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private DataDynamics.ActiveReports.GroupHeader groupHeader1;
        private DataDynamics.ActiveReports.GroupFooter groupFooter1;
        private DataDynamics.ActiveReports.Shape shape2;
        private DataDynamics.ActiveReports.Barcode barcode1;
        private DataDynamics.ActiveReports.Label label5;
        private DataDynamics.ActiveReports.Label label6;
        private DataDynamics.ActiveReports.Label label7;
        private DataDynamics.ActiveReports.Label label8;
        private DataDynamics.ActiveReports.Line line9;
        private DataDynamics.ActiveReports.Line line10;
        private DataDynamics.ActiveReports.Label label1;

    }
}
