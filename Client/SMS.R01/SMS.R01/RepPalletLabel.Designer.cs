namespace SMS.R01
{
    /// <summary>
    /// RepPalletLabel の概要の説明です。
    /// </summary>
    partial class RepPalletLabel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RepPalletLabel));
            this.pageHeader = new DataDynamics.ActiveReports.PageHeader();
            this.detail = new DataDynamics.ActiveReports.Detail();
            this.shape1 = new DataDynamics.ActiveReports.Shape();
            this.line11 = new DataDynamics.ActiveReports.Line();
            this.textBox1 = new DataDynamics.ActiveReports.TextBox();
            this.barcode1 = new DataDynamics.ActiveReports.Barcode();
            this.label2 = new DataDynamics.ActiveReports.Label();
            this.pageFooter = new DataDynamics.ActiveReports.PageFooter();
            this.groupHeader1 = new DataDynamics.ActiveReports.GroupHeader();
            this.line6 = new DataDynamics.ActiveReports.Line();
            this.line3 = new DataDynamics.ActiveReports.Line();
            this.line1 = new DataDynamics.ActiveReports.Line();
            this.line2 = new DataDynamics.ActiveReports.Line();
            this.line4 = new DataDynamics.ActiveReports.Line();
            this.line5 = new DataDynamics.ActiveReports.Line();
            this.line7 = new DataDynamics.ActiveReports.Line();
            this.line8 = new DataDynamics.ActiveReports.Line();
            this.line9 = new DataDynamics.ActiveReports.Line();
            this.line10 = new DataDynamics.ActiveReports.Line();
            this.groupFooter1 = new DataDynamics.ActiveReports.GroupFooter();
            ((System.ComponentModel.ISupportInitialize)(this.textBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Height = 0F;
            this.pageHeader.Name = "pageHeader";
            // 
            // detail
            // 
            this.detail.ColumnCount = 2;
            this.detail.ColumnSpacing = 0F;
            this.detail.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
            this.shape1,
            this.line11,
            this.textBox1,
            this.barcode1,
            this.label2});
            this.detail.Height = 1.074F;
            this.detail.Name = "detail";
            // 
            // shape1
            // 
            resources.ApplyResources(this.shape1, "shape1");
            this.shape1.Name = "shape1";
            this.shape1.RoundingRadius = 9.999999F;
            // 
            // line11
            // 
            resources.ApplyResources(this.line11, "line11");
            this.line11.LineWeight = 1F;
            this.line11.Name = "line11";
            this.line11.X1 = 0.4F;
            this.line11.X2 = 0.4F;
            this.line11.Y1 = 0.1125984F;
            this.line11.Y2 = 0.9645669F;
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            // 
            // barcode1
            // 
            this.barcode1.CheckSumEnabled = false;
            this.barcode1.DataField = "CURRENT_NO";
            this.barcode1.Font = new System.Drawing.Font("Courier New", 8F);
            resources.ApplyResources(this.barcode1, "barcode1");
            this.barcode1.Name = "barcode1";
            this.barcode1.Style = DataDynamics.ActiveReports.BarCodeStyle.Code39;
            // 
            // label2
            // 
            this.label2.DataField = "CURRENT_NO";
            resources.ApplyResources(this.label2, "label2");
            this.label2.HyperLink = null;
            this.label2.Name = "label2";
            // 
            // pageFooter
            // 
            this.pageFooter.Height = 0F;
            this.pageFooter.Name = "pageFooter";
            // 
            // groupHeader1
            // 
            this.groupHeader1.CanShrink = true;
            this.groupHeader1.ColumnLayout = false;
            this.groupHeader1.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
            this.line6,
            this.line3,
            this.line1,
            this.line2,
            this.line4,
            this.line5,
            this.line7,
            this.line8,
            this.line9,
            this.line10});
            this.groupHeader1.Height = 10.75103F;
            this.groupHeader1.Name = "groupHeader1";
            this.groupHeader1.RepeatStyle = DataDynamics.ActiveReports.RepeatStyle.OnPage;
            this.groupHeader1.UnderlayNext = true;
            // 
            // line6
            // 
            resources.ApplyResources(this.line6, "line6");
            this.line6.LineStyle = DataDynamics.ActiveReports.LineStyle.Dash;
            this.line6.LineWeight = 1F;
            this.line6.Name = "line6";
            this.line6.X1 = 3.651F;
            this.line6.X2 = 3.651F;
            this.line6.Y1 = 0F;
            this.line6.Y2 = 10.74409F;
            // 
            // line3
            // 
            resources.ApplyResources(this.line3, "line3");
            this.line3.LineStyle = DataDynamics.ActiveReports.LineStyle.Dash;
            this.line3.LineWeight = 1F;
            this.line3.Name = "line3";
            this.line3.X1 = 0F;
            this.line3.X2 = 7.326771F;
            this.line3.Y1 = 1.074803F;
            this.line3.Y2 = 1.074803F;
            // 
            // line1
            // 
            resources.ApplyResources(this.line1, "line1");
            this.line1.LineStyle = DataDynamics.ActiveReports.LineStyle.Dash;
            this.line1.LineWeight = 1F;
            this.line1.Name = "line1";
            this.line1.X1 = 0F;
            this.line1.X2 = 7.326771F;
            this.line1.Y1 = 2.149606F;
            this.line1.Y2 = 2.149606F;
            // 
            // line2
            // 
            resources.ApplyResources(this.line2, "line2");
            this.line2.LineStyle = DataDynamics.ActiveReports.LineStyle.Dash;
            this.line2.LineWeight = 1F;
            this.line2.Name = "line2";
            this.line2.X1 = 0F;
            this.line2.X2 = 7.326771F;
            this.line2.Y1 = 3.224409F;
            this.line2.Y2 = 3.224409F;
            // 
            // line4
            // 
            resources.ApplyResources(this.line4, "line4");
            this.line4.LineStyle = DataDynamics.ActiveReports.LineStyle.Dash;
            this.line4.LineWeight = 1F;
            this.line4.Name = "line4";
            this.line4.X1 = 0F;
            this.line4.X2 = 7.326771F;
            this.line4.Y1 = 4.299212F;
            this.line4.Y2 = 4.299212F;
            // 
            // line5
            // 
            resources.ApplyResources(this.line5, "line5");
            this.line5.LineStyle = DataDynamics.ActiveReports.LineStyle.Dash;
            this.line5.LineWeight = 1F;
            this.line5.Name = "line5";
            this.line5.X1 = 0F;
            this.line5.X2 = 7.326771F;
            this.line5.Y1 = 5.374016F;
            this.line5.Y2 = 5.374016F;
            // 
            // line7
            // 
            resources.ApplyResources(this.line7, "line7");
            this.line7.LineStyle = DataDynamics.ActiveReports.LineStyle.Dash;
            this.line7.LineWeight = 1F;
            this.line7.Name = "line7";
            this.line7.X1 = 0F;
            this.line7.X2 = 7.326771F;
            this.line7.Y1 = 6.448819F;
            this.line7.Y2 = 6.448819F;
            // 
            // line8
            // 
            resources.ApplyResources(this.line8, "line8");
            this.line8.LineStyle = DataDynamics.ActiveReports.LineStyle.Dash;
            this.line8.LineWeight = 1F;
            this.line8.Name = "line8";
            this.line8.X1 = 0F;
            this.line8.X2 = 7.326771F;
            this.line8.Y1 = 7.523625F;
            this.line8.Y2 = 7.523625F;
            // 
            // line9
            // 
            resources.ApplyResources(this.line9, "line9");
            this.line9.LineStyle = DataDynamics.ActiveReports.LineStyle.Dash;
            this.line9.LineWeight = 1F;
            this.line9.Name = "line9";
            this.line9.X1 = 0F;
            this.line9.X2 = 7.326771F;
            this.line9.Y1 = 8.598424F;
            this.line9.Y2 = 8.598424F;
            // 
            // line10
            // 
            resources.ApplyResources(this.line10, "line10");
            this.line10.LineStyle = DataDynamics.ActiveReports.LineStyle.Dash;
            this.line10.LineWeight = 1F;
            this.line10.Name = "line10";
            this.line10.X1 = 0F;
            this.line10.X2 = 7.326771F;
            this.line10.Y1 = 9.673229F;
            this.line10.Y2 = 9.673229F;
            // 
            // groupFooter1
            // 
            this.groupFooter1.Height = 0F;
            this.groupFooter1.Name = "groupFooter1";
            // 
            // RepPalletLabel
            // 
            this.MasterReport = false;
            this.PageSettings.DefaultPaperSize = false;
            this.PageSettings.Margins.Bottom = 0.472441F;
            this.PageSettings.Margins.Left = 0.472441F;
            this.PageSettings.Margins.Right = 0.472441F;
            this.PageSettings.Margins.Top = 0.472441F;
            this.PageSettings.Orientation = DataDynamics.ActiveReports.Document.PageOrientation.Portrait;
            this.PageSettings.PaperHeight = 11.69291F;
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.PageSettings.PaperWidth = 8.267716F;
            this.PrintWidth = 7.318898F;
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
            ((System.ComponentModel.ISupportInitialize)(this.textBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private DataDynamics.ActiveReports.Shape shape1;
        private DataDynamics.ActiveReports.Line line11;
        private DataDynamics.ActiveReports.TextBox textBox1;
        private DataDynamics.ActiveReports.Barcode barcode1;
        private DataDynamics.ActiveReports.Label label2;
        private DataDynamics.ActiveReports.GroupHeader groupHeader1;
        private DataDynamics.ActiveReports.GroupFooter groupFooter1;
        private DataDynamics.ActiveReports.Line line6;
        private DataDynamics.ActiveReports.Line line3;
        private DataDynamics.ActiveReports.Line line1;
        private DataDynamics.ActiveReports.Line line2;
        private DataDynamics.ActiveReports.Line line4;
        private DataDynamics.ActiveReports.Line line5;
        private DataDynamics.ActiveReports.Line line7;
        private DataDynamics.ActiveReports.Line line8;
        private DataDynamics.ActiveReports.Line line9;
        private DataDynamics.ActiveReports.Line line10;

    }
}
