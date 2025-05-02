namespace SMS.R01
{
    /// <summary>
    /// RepBoxTagList の概要の説明です。
    /// </summary>
    partial class RepBoxTagList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RepBoxTagList));
            this.shape1 = new DataDynamics.ActiveReports.Shape();
            this.bcdBoxNo = new DataDynamics.ActiveReports.Barcode();
            this.lblNonyusakiName = new DataDynamics.ActiveReports.Label();
            this.lblMNo = new DataDynamics.ActiveReports.Label();
            this.lblBoxNo = new DataDynamics.ActiveReports.Label();
            this.lblShip = new DataDynamics.ActiveReports.Label();
            this.lblKishu = new DataDynamics.ActiveReports.Label();
            this.lblFloor = new DataDynamics.ActiveReports.Label();
            this.line3 = new DataDynamics.ActiveReports.Line();
            this.line4 = new DataDynamics.ActiveReports.Line();
            this.line5 = new DataDynamics.ActiveReports.Line();
            this.line6 = new DataDynamics.ActiveReports.Line();
            this.line8 = new DataDynamics.ActiveReports.Line();
            this.label3 = new DataDynamics.ActiveReports.Label();
            this.line7 = new DataDynamics.ActiveReports.Line();
            this.line1 = new DataDynamics.ActiveReports.Line();
            this.line2 = new DataDynamics.ActiveReports.Line();
            this.pageHeader = new DataDynamics.ActiveReports.PageHeader();
            this.detail = new DataDynamics.ActiveReports.Detail();
            this.pageFooter = new DataDynamics.ActiveReports.PageFooter();
            this.groupHeader1 = new DataDynamics.ActiveReports.GroupHeader();
            this.groupFooter1 = new DataDynamics.ActiveReports.GroupFooter();
            ((System.ComponentModel.ISupportInitialize)(this.lblNonyusakiName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBoxNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblShip)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblKishu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFloor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // shape1
            // 
            resources.ApplyResources(this.shape1, "shape1");
            this.shape1.Name = "shape1";
            this.shape1.RoundingRadius = 9.999999F;
            // 
            // bcdBoxNo
            // 
            this.bcdBoxNo.CheckSumEnabled = false;
            this.bcdBoxNo.DataField = "BOX_NO";
            this.bcdBoxNo.Font = new System.Drawing.Font("Courier New", 8F);
            resources.ApplyResources(this.bcdBoxNo, "bcdBoxNo");
            this.bcdBoxNo.Name = "bcdBoxNo";
            this.bcdBoxNo.Style = DataDynamics.ActiveReports.BarCodeStyle.Code39;
            // 
            // lblNonyusakiName
            // 
            this.lblNonyusakiName.DataField = "NONYUSAKI_NAME";
            resources.ApplyResources(this.lblNonyusakiName, "lblNonyusakiName");
            this.lblNonyusakiName.HyperLink = null;
            this.lblNonyusakiName.Name = "lblNonyusakiName";
            // 
            // lblMNo
            // 
            this.lblMNo.DataField = "M_NO";
            resources.ApplyResources(this.lblMNo, "lblMNo");
            this.lblMNo.HyperLink = null;
            this.lblMNo.Name = "lblMNo";
            // 
            // lblBoxNo
            // 
            this.lblBoxNo.DataField = "BOX_NO";
            resources.ApplyResources(this.lblBoxNo, "lblBoxNo");
            this.lblBoxNo.HyperLink = null;
            this.lblBoxNo.Name = "lblBoxNo";
            // 
            // lblShip
            // 
            this.lblShip.DataField = "SHIP";
            resources.ApplyResources(this.lblShip, "lblShip");
            this.lblShip.HyperLink = null;
            this.lblShip.Name = "lblShip";
            // 
            // lblKishu
            // 
            this.lblKishu.DataField = "KISHU";
            resources.ApplyResources(this.lblKishu, "lblKishu");
            this.lblKishu.HyperLink = null;
            this.lblKishu.Name = "lblKishu";
            // 
            // lblFloor
            // 
            this.lblFloor.DataField = "FLOOR";
            resources.ApplyResources(this.lblFloor, "lblFloor");
            this.lblFloor.HyperLink = null;
            this.lblFloor.Name = "lblFloor";
            // 
            // line3
            // 
            resources.ApplyResources(this.line3, "line3");
            this.line3.LineWeight = 1F;
            this.line3.Name = "line3";
            this.line3.X1 = 0.357874F;
            this.line3.X2 = 6.968898F;
            this.line3.Y1 = 1.135433F;
            this.line3.Y2 = 1.135433F;
            // 
            // line4
            // 
            resources.ApplyResources(this.line4, "line4");
            this.line4.LineWeight = 1F;
            this.line4.Name = "line4";
            this.line4.X1 = 0.357874F;
            this.line4.X2 = 6.968896F;
            this.line4.Y1 = 2.533465F;
            this.line4.Y2 = 2.533465F;
            // 
            // line5
            // 
            resources.ApplyResources(this.line5, "line5");
            this.line5.LineWeight = 1F;
            this.line5.Name = "line5";
            this.line5.X1 = 4.572048F;
            this.line5.X2 = 4.572048F;
            this.line5.Y1 = 0.2165356F;
            this.line5.Y2 = 1.135433F;
            // 
            // line6
            // 
            resources.ApplyResources(this.line6, "line6");
            this.line6.LineWeight = 1F;
            this.line6.Name = "line6";
            this.line6.X1 = 2.383858F;
            this.line6.X2 = 2.383858F;
            this.line6.Y1 = 2.533465F;
            this.line6.Y2 = 3.366142F;
            // 
            // line8
            // 
            resources.ApplyResources(this.line8, "line8");
            this.line8.LineWeight = 1F;
            this.line8.Name = "line8";
            this.line8.X1 = 5.680709F;
            this.line8.X2 = 5.680709F;
            this.line8.Y1 = 2.533465F;
            this.line8.Y2 = 3.366143F;
            // 
            // label3
            // 
            this.label3.DataField = "AREA";
            resources.ApplyResources(this.label3, "label3");
            this.label3.HyperLink = null;
            this.label3.Name = "label3";
            // 
            // line7
            // 
            resources.ApplyResources(this.line7, "line7");
            this.line7.LineWeight = 1F;
            this.line7.Name = "line7";
            this.line7.X1 = 4.707087F;
            this.line7.X2 = 4.707087F;
            this.line7.Y1 = 2.533465F;
            this.line7.Y2 = 3.366142F;
            // 
            // line1
            // 
            resources.ApplyResources(this.line1, "line1");
            this.line1.LineWeight = 1F;
            this.line1.Name = "line1";
            this.line1.X1 = 0.1968504F;
            this.line1.X2 = 7.086614F;
            this.line1.Y1 = 3.582677F;
            this.line1.Y2 = 3.582677F;
            // 
            // line2
            // 
            resources.ApplyResources(this.line2, "line2");
            this.line2.LineWeight = 1F;
            this.line2.Name = "line2";
            this.line2.X1 = 0.1968504F;
            this.line2.X2 = 7.086611F;
            this.line2.Y1 = 7.165355F;
            this.line2.Y2 = 7.165355F;
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
            this.detail.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
            this.shape1,
            this.bcdBoxNo,
            this.lblNonyusakiName,
            this.lblMNo,
            this.lblBoxNo,
            this.lblShip,
            this.lblKishu,
            this.lblFloor,
            this.line3,
            this.line4,
            this.line5,
            this.line6,
            this.line8,
            this.label3,
            this.line7});
            this.detail.Height = 3.58F;
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
            this.line1,
            this.line2});
            this.groupHeader1.Height = 7.387387F;
            this.groupHeader1.Name = "groupHeader1";
            this.groupHeader1.RepeatStyle = DataDynamics.ActiveReports.RepeatStyle.OnPage;
            this.groupHeader1.UnderlayNext = true;
            // 
            // groupFooter1
            // 
            this.groupFooter1.Height = 0F;
            this.groupFooter1.Name = "groupFooter1";
            // 
            // RepBoxTagList
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
            this.PrintWidth = 7.322835F;
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
            this.ReportStart += new System.EventHandler(this.RepBoxTagList_ReportStart);
            ((System.ComponentModel.ISupportInitialize)(this.lblNonyusakiName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBoxNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblShip)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblKishu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFloor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private DataDynamics.ActiveReports.Shape shape1;
        private DataDynamics.ActiveReports.Barcode bcdBoxNo;
        private DataDynamics.ActiveReports.Label lblNonyusakiName;
        private DataDynamics.ActiveReports.Label lblMNo;
        private DataDynamics.ActiveReports.Label lblBoxNo;
        private DataDynamics.ActiveReports.Label lblShip;
        private DataDynamics.ActiveReports.Label lblKishu;
        private DataDynamics.ActiveReports.Label lblFloor;
        private DataDynamics.ActiveReports.GroupHeader groupHeader1;
        private DataDynamics.ActiveReports.Line line1;
        private DataDynamics.ActiveReports.Line line2;
        private DataDynamics.ActiveReports.GroupFooter groupFooter1;
        private DataDynamics.ActiveReports.Line line3;
        private DataDynamics.ActiveReports.Line line4;
        private DataDynamics.ActiveReports.Line line5;
        private DataDynamics.ActiveReports.Line line6;
        private DataDynamics.ActiveReports.Line line7;
        private DataDynamics.ActiveReports.Line line8;
        private DataDynamics.ActiveReports.Label label3;

    }
}
