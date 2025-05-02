namespace SMS.K02.Forms
{
    partial class KiwakuKonpoMeisai
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
            System.Windows.Forms.TableLayoutPanel tlpInputBasic;
            System.Windows.Forms.TableLayoutPanel tlpInputDetail;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KiwakuKonpoMeisai));
            this.txtCaseNo = new DSWControl.DSWNumericTextBox();
            this.lblCaseNo = new DSWControl.DSWControlLinkLabel.DSWControlLinkLabel();
            this.txtNetW = new DSWControl.DSWNumericTextBox();
            this.lblStyle = new DSWControl.DSWControlLinkLabel.DSWControlLinkLabel();
            this.lblDimention = new DSWControl.DSWControlLinkLabel.DSWControlLinkLabel();
            this.txtMokuzaiJyuryo = new DSWControl.DSWNumericTextBox();
            this.lblDimentionL = new DSWControl.DSWControlLinkLabel.DSWControlLinkLabel();
            this.txtMMNet = new DSWControl.DSWNumericTextBox();
            this.lblDimentionW = new DSWControl.DSWControlLinkLabel.DSWControlLinkLabel();
            this.txtDimentionH = new DSWControl.DSWNumericTextBox();
            this.lblDimentionH = new DSWControl.DSWControlLinkLabel.DSWControlLinkLabel();
            this.txtDimentionW = new DSWControl.DSWNumericTextBox();
            this.lblMMNet = new DSWControl.DSWControlLinkLabel.DSWControlLinkLabel();
            this.txtDimentionL = new DSWControl.DSWNumericTextBox();
            this.lblGrossW = new DSWControl.DSWControlLinkLabel.DSWControlLinkLabel();
            this.txtStyle = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblNetW = new DSWControl.DSWControlLinkLabel.DSWControlLinkLabel();
            this.lblMokuzaiJyuryo = new DSWControl.DSWControlLinkLabel.DSWControlLinkLabel();
            this.nudGrossW = new System.Windows.Forms.NumericUpDown();
            this.shtMeisai = new GrapeCity.Win.ElTabelle.Sheet();
            this.lblNote = new System.Windows.Forms.Label();
            this.lblItem = new DSWControl.DSWLabel.DSWLabel();
            this.txtItem = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblDescription1 = new DSWControl.DSWLabel.DSWLabel();
            this.txtDescription1 = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblDescription2 = new DSWControl.DSWLabel.DSWLabel();
            this.txtDescription2 = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblShip = new DSWControl.DSWLabel.DSWLabel();
            this.txtShip = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblKojiName = new DSWControl.DSWLabel.DSWLabel();
            this.txtKojiName = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblKojiNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtKojiNo = new DSWControl.DSWTextBox.DSWTextBox();
            tlpInputBasic = new System.Windows.Forms.TableLayoutPanel();
            tlpInputDetail = new System.Windows.Forms.TableLayoutPanel();
            this.pnlMain.SuspendLayout();
            tlpInputBasic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrossW)).BeginInit();
            tlpInputDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).BeginInit();
            this.lblItem.ChildPanel.SuspendLayout();
            this.lblItem.SuspendLayout();
            this.lblDescription1.ChildPanel.SuspendLayout();
            this.lblDescription1.SuspendLayout();
            this.lblDescription2.ChildPanel.SuspendLayout();
            this.lblDescription2.SuspendLayout();
            this.lblShip.ChildPanel.SuspendLayout();
            this.lblShip.SuspendLayout();
            this.lblKojiName.ChildPanel.SuspendLayout();
            this.lblKojiName.SuspendLayout();
            this.lblKojiNo.ChildPanel.SuspendLayout();
            this.lblKojiNo.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(tlpInputDetail);
            this.pnlMain.Controls.Add(tlpInputBasic);
            this.pnlMain.Controls.Add(this.lblKojiNo);
            this.pnlMain.Controls.Add(this.lblShip);
            this.pnlMain.Controls.Add(this.lblKojiName);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.SetChildIndex(this.lblKojiName, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblShip, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblKojiNo, 0);
            this.pnlMain.Controls.SetChildIndex(tlpInputBasic, 0);
            this.pnlMain.Controls.SetChildIndex(tlpInputDetail, 0);
            // 
            // fbrFunction
            // 
            // 
            // fbrFunction.F01Button
            // 
            resources.ApplyResources(this.fbrFunction.F01Button, "fbrFunction.F01Button");
            resources.ApplyResources(this.fbrFunction, "fbrFunction");
            // 
            // fbrFunction.F04Button
            // 
            resources.ApplyResources(this.fbrFunction.F04Button, "fbrFunction.F04Button");
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // tlpInputBasic
            // 
            resources.ApplyResources(tlpInputBasic, "tlpInputBasic");
            tlpInputBasic.Controls.Add(this.txtCaseNo, 0, 2);
            tlpInputBasic.Controls.Add(this.lblCaseNo, 0, 0);
            tlpInputBasic.Controls.Add(this.txtNetW, 8, 2);
            tlpInputBasic.Controls.Add(this.lblStyle, 1, 0);
            tlpInputBasic.Controls.Add(this.lblDimention, 2, 0);
            tlpInputBasic.Controls.Add(this.txtMokuzaiJyuryo, 6, 2);
            tlpInputBasic.Controls.Add(this.lblDimentionL, 2, 1);
            tlpInputBasic.Controls.Add(this.txtMMNet, 5, 2);
            tlpInputBasic.Controls.Add(this.lblDimentionW, 3, 1);
            tlpInputBasic.Controls.Add(this.txtDimentionH, 4, 2);
            tlpInputBasic.Controls.Add(this.lblDimentionH, 4, 1);
            tlpInputBasic.Controls.Add(this.txtDimentionW, 3, 2);
            tlpInputBasic.Controls.Add(this.lblMMNet, 5, 0);
            tlpInputBasic.Controls.Add(this.txtDimentionL, 2, 2);
            tlpInputBasic.Controls.Add(this.lblGrossW, 7, 0);
            tlpInputBasic.Controls.Add(this.txtStyle, 1, 2);
            tlpInputBasic.Controls.Add(this.lblNetW, 8, 0);
            tlpInputBasic.Controls.Add(this.lblMokuzaiJyuryo, 6, 0);
            tlpInputBasic.Controls.Add(this.nudGrossW, 7, 2);
            tlpInputBasic.Name = "tlpInputBasic";
            // 
            // txtCaseNo
            // 
            this.txtCaseNo.AllowEmpty = true;
            resources.ApplyResources(this.txtCaseNo, "txtCaseNo");
            this.txtCaseNo.IsThousands = true;
            this.txtCaseNo.Name = "txtCaseNo";
            this.txtCaseNo.ReadOnly = true;
            this.txtCaseNo.TabStop = false;
            this.txtCaseNo.Value = new decimal(new int[] {
            999,
            0,
            0,
            0});
            // 
            // lblCaseNo
            // 
            resources.ApplyResources(this.lblCaseNo, "lblCaseNo");
            this.lblCaseNo.Name = "lblCaseNo";
            tlpInputBasic.SetRowSpan(this.lblCaseNo, 2);
            // 
            // txtNetW
            // 
            this.txtNetW.AllowEmpty = true;
            this.txtNetW.BackColor = System.Drawing.SystemColors.Control;
            this.txtNetW.DecimalLength = 2;
            resources.ApplyResources(this.txtNetW, "txtNetW");
            this.txtNetW.IsThousands = true;
            this.txtNetW.Name = "txtNetW";
            this.txtNetW.TabStop = false;
            this.txtNetW.Value = new decimal(new int[] {
            9999999,
            0,
            0,
            131072});
            // 
            // lblStyle
            // 
            resources.ApplyResources(this.lblStyle, "lblStyle");
            this.lblStyle.Name = "lblStyle";
            tlpInputBasic.SetRowSpan(this.lblStyle, 2);
            // 
            // lblDimention
            // 
            this.lblDimention.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            tlpInputBasic.SetColumnSpan(this.lblDimention, 3);
            resources.ApplyResources(this.lblDimention, "lblDimention");
            this.lblDimention.Name = "lblDimention";
            // 
            // txtMokuzaiJyuryo
            // 
            this.txtMokuzaiJyuryo.DecimalLength = 2;
            resources.ApplyResources(this.txtMokuzaiJyuryo, "txtMokuzaiJyuryo");
            this.txtMokuzaiJyuryo.IsThousands = true;
            this.txtMokuzaiJyuryo.Name = "txtMokuzaiJyuryo";
            this.txtMokuzaiJyuryo.ReadOnly = true;
            this.txtMokuzaiJyuryo.TabStop = false;
            this.txtMokuzaiJyuryo.Value = new decimal(new int[] {
            9999999,
            0,
            0,
            131072});
            // 
            // lblDimentionL
            // 
            resources.ApplyResources(this.lblDimentionL, "lblDimentionL");
            this.lblDimentionL.Name = "lblDimentionL";
            // 
            // txtMMNet
            // 
            this.txtMMNet.DecimalLength = 3;
            resources.ApplyResources(this.txtMMNet, "txtMMNet");
            this.txtMMNet.IsThousands = true;
            this.txtMMNet.Name = "txtMMNet";
            this.txtMMNet.ReadOnly = true;
            this.txtMMNet.TabStop = false;
            this.txtMMNet.Value = new decimal(new int[] {
            999999999,
            0,
            0,
            196608});
            // 
            // lblDimentionW
            // 
            resources.ApplyResources(this.lblDimentionW, "lblDimentionW");
            this.lblDimentionW.Name = "lblDimentionW";
            // 
            // txtDimentionH
            // 
            resources.ApplyResources(this.txtDimentionH, "txtDimentionH");
            this.txtDimentionH.IsThousands = true;
            this.txtDimentionH.Name = "txtDimentionH";
            this.txtDimentionH.ReadOnly = true;
            this.txtDimentionH.TabStop = false;
            this.txtDimentionH.Value = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            // 
            // lblDimentionH
            // 
            resources.ApplyResources(this.lblDimentionH, "lblDimentionH");
            this.lblDimentionH.Name = "lblDimentionH";
            // 
            // txtDimentionW
            // 
            resources.ApplyResources(this.txtDimentionW, "txtDimentionW");
            this.txtDimentionW.IsThousands = true;
            this.txtDimentionW.Name = "txtDimentionW";
            this.txtDimentionW.ReadOnly = true;
            this.txtDimentionW.TabStop = false;
            this.txtDimentionW.Value = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            // 
            // lblMMNet
            // 
            resources.ApplyResources(this.lblMMNet, "lblMMNet");
            this.lblMMNet.Name = "lblMMNet";
            tlpInputBasic.SetRowSpan(this.lblMMNet, 2);
            // 
            // txtDimentionL
            // 
            resources.ApplyResources(this.txtDimentionL, "txtDimentionL");
            this.txtDimentionL.IsThousands = true;
            this.txtDimentionL.Name = "txtDimentionL";
            this.txtDimentionL.ReadOnly = true;
            this.txtDimentionL.TabStop = false;
            this.txtDimentionL.Value = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            // 
            // lblGrossW
            // 
            this.lblGrossW.BackColor = System.Drawing.Color.PaleGreen;
            resources.ApplyResources(this.lblGrossW, "lblGrossW");
            this.lblGrossW.IsNecessary = true;
            this.lblGrossW.Name = "lblGrossW";
            this.lblGrossW.NormalBackColor = System.Drawing.Color.PaleGreen;
            tlpInputBasic.SetRowSpan(this.lblGrossW, 2);
            // 
            // txtStyle
            // 
            this.txtStyle.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtStyle, "txtStyle");
            this.txtStyle.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtStyle.InputRegulation = "";
            this.txtStyle.Name = "txtStyle";
            this.txtStyle.ProhibitionChar = null;
            this.txtStyle.ReadOnly = true;
            this.txtStyle.TabStop = false;
            // 
            // lblNetW
            // 
            resources.ApplyResources(this.lblNetW, "lblNetW");
            this.lblNetW.Name = "lblNetW";
            tlpInputBasic.SetRowSpan(this.lblNetW, 2);
            // 
            // lblMokuzaiJyuryo
            // 
            resources.ApplyResources(this.lblMokuzaiJyuryo, "lblMokuzaiJyuryo");
            this.lblMokuzaiJyuryo.Name = "lblMokuzaiJyuryo";
            tlpInputBasic.SetRowSpan(this.lblMokuzaiJyuryo, 2);
            // 
            // nudGrossW
            // 
            this.nudGrossW.DecimalPlaces = 2;
            resources.ApplyResources(this.nudGrossW, "nudGrossW");
            this.nudGrossW.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            131072});
            this.nudGrossW.Name = "nudGrossW";
            this.nudGrossW.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudGrossW.ValueChanged += new System.EventHandler(this.nudGrossW_ValueChanged);
            this.nudGrossW.Enter += new System.EventHandler(this.nudGrossW_Enter);
            // 
            // tlpInputDetail
            // 
            resources.ApplyResources(tlpInputDetail, "tlpInputDetail");
            tlpInputDetail.Controls.Add(this.shtMeisai, 0, 0);
            tlpInputDetail.Controls.Add(this.lblNote, 0, 3);
            tlpInputDetail.Controls.Add(this.lblItem, 2, 0);
            tlpInputDetail.Controls.Add(this.lblDescription1, 2, 1);
            tlpInputDetail.Controls.Add(this.lblDescription2, 2, 2);
            tlpInputDetail.Name = "tlpInputDetail";
            // 
            // shtMeisai
            // 
            this.shtMeisai.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtMeisai.Data")));
            resources.ApplyResources(this.shtMeisai, "shtMeisai");
            this.shtMeisai.Name = "shtMeisai";
            tlpInputDetail.SetRowSpan(this.shtMeisai, 3);
            this.shtMeisai.LeaveEdit += new GrapeCity.Win.ElTabelle.LeaveEditEventHandler(this.shtMeisai_LeaveEdit);
            this.shtMeisai.ClippingData += new GrapeCity.Win.ElTabelle.ClippingDataEventHandler(this.shtMeisai_ClippingData);
            // 
            // lblNote
            // 
            resources.ApplyResources(this.lblNote, "lblNote");
            this.lblNote.Name = "lblNote";
            // 
            // lblItem
            // 
            // 
            // lblItem.ChildPanel
            // 
            this.lblItem.ChildPanel.Controls.Add(this.txtItem);
            this.lblItem.IsFocusChangeColor = false;
            this.lblItem.LabelWidth = 150;
            resources.ApplyResources(this.lblItem, "lblItem");
            this.lblItem.Name = "lblItem";
            this.lblItem.SplitterWidth = 0;
            // 
            // txtItem
            // 
            resources.ApplyResources(this.txtItem, "txtItem");
            this.txtItem.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtItem.InputRegulation = "abnls";
            this.txtItem.IsInputRegulation = true;
            this.txtItem.MaxByteLengthMode = true;
            this.txtItem.Name = "txtItem";
            this.txtItem.OneLineMaxLength = 30;
            this.txtItem.ProhibitionChar = null;
            // 
            // lblDescription1
            // 
            // 
            // lblDescription1.ChildPanel
            // 
            this.lblDescription1.ChildPanel.Controls.Add(this.txtDescription1);
            this.lblDescription1.IsFocusChangeColor = false;
            this.lblDescription1.LabelWidth = 150;
            resources.ApplyResources(this.lblDescription1, "lblDescription1");
            this.lblDescription1.Name = "lblDescription1";
            this.lblDescription1.SplitterWidth = 0;
            // 
            // txtDescription1
            // 
            this.txtDescription1.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtDescription1, "txtDescription1");
            this.txtDescription1.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtDescription1.InputRegulation = "abnls";
            this.txtDescription1.IsInputRegulation = true;
            this.txtDescription1.MaxByteLengthMode = true;
            this.txtDescription1.Name = "txtDescription1";
            this.txtDescription1.OneLineMaxLength = 30;
            this.txtDescription1.ProhibitionChar = null;
            this.txtDescription1.ReadOnly = true;
            // 
            // lblDescription2
            // 
            // 
            // lblDescription2.ChildPanel
            // 
            this.lblDescription2.ChildPanel.Controls.Add(this.txtDescription2);
            this.lblDescription2.IsFocusChangeColor = false;
            this.lblDescription2.LabelWidth = 150;
            resources.ApplyResources(this.lblDescription2, "lblDescription2");
            this.lblDescription2.Name = "lblDescription2";
            this.lblDescription2.SplitterWidth = 0;
            // 
            // txtDescription2
            // 
            resources.ApplyResources(this.txtDescription2, "txtDescription2");
            this.txtDescription2.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtDescription2.InputRegulation = "abnls";
            this.txtDescription2.IsInputRegulation = true;
            this.txtDescription2.MaxByteLengthMode = true;
            this.txtDescription2.Name = "txtDescription2";
            this.txtDescription2.OneLineMaxLength = 100;
            this.txtDescription2.ProhibitionChar = null;
            // 
            // lblShip
            // 
            // 
            // lblShip.ChildPanel
            // 
            this.lblShip.ChildPanel.Controls.Add(this.txtShip);
            this.lblShip.IsFocusChangeColor = false;
            this.lblShip.LabelWidth = 80;
            resources.ApplyResources(this.lblShip, "lblShip");
            this.lblShip.Name = "lblShip";
            this.lblShip.SplitterWidth = 0;
            // 
            // txtShip
            // 
            this.txtShip.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtShip, "txtShip");
            this.txtShip.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtShip.InputRegulation = "";
            this.txtShip.Name = "txtShip";
            this.txtShip.ProhibitionChar = null;
            this.txtShip.ReadOnly = true;
            this.txtShip.TabStop = false;
            // 
            // lblKojiName
            // 
            this.lblKojiName.BackColor = System.Drawing.Color.Transparent;
            // 
            // lblKojiName.ChildPanel
            // 
            this.lblKojiName.ChildPanel.Controls.Add(this.txtKojiName);
            this.lblKojiName.IsFocusChangeColor = false;
            this.lblKojiName.LabelWidth = 100;
            resources.ApplyResources(this.lblKojiName, "lblKojiName");
            this.lblKojiName.Name = "lblKojiName";
            this.lblKojiName.SplitterWidth = 0;
            // 
            // txtKojiName
            // 
            this.txtKojiName.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtKojiName, "txtKojiName");
            this.txtKojiName.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtKojiName.InputRegulation = "";
            this.txtKojiName.Name = "txtKojiName";
            this.txtKojiName.ProhibitionChar = null;
            this.txtKojiName.ReadOnly = true;
            this.txtKojiName.TabStop = false;
            // 
            // lblKojiNo
            // 
            // 
            // lblKojiNo.ChildPanel
            // 
            this.lblKojiNo.ChildPanel.Controls.Add(this.txtKojiNo);
            this.lblKojiNo.IsFocusChangeColor = false;
            this.lblKojiNo.LabelWidth = 80;
            resources.ApplyResources(this.lblKojiNo, "lblKojiNo");
            this.lblKojiNo.Name = "lblKojiNo";
            this.lblKojiNo.SplitterWidth = 0;
            this.lblKojiNo.TabStop = false;
            // 
            // txtKojiNo
            // 
            this.txtKojiNo.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtKojiNo, "txtKojiNo");
            this.txtKojiNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtKojiNo.InputRegulation = "";
            this.txtKojiNo.Name = "txtKojiNo";
            this.txtKojiNo.ProhibitionChar = null;
            this.txtKojiNo.ReadOnly = true;
            this.txtKojiNo.TabStop = false;
            // 
            // KiwakuKonpoMeisai
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "KiwakuKonpoMeisai";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            tlpInputBasic.ResumeLayout(false);
            tlpInputBasic.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrossW)).EndInit();
            tlpInputDetail.ResumeLayout(false);
            tlpInputDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).EndInit();
            this.lblItem.ChildPanel.ResumeLayout(false);
            this.lblItem.ChildPanel.PerformLayout();
            this.lblItem.ResumeLayout(false);
            this.lblDescription1.ChildPanel.ResumeLayout(false);
            this.lblDescription1.ChildPanel.PerformLayout();
            this.lblDescription1.ResumeLayout(false);
            this.lblDescription2.ChildPanel.ResumeLayout(false);
            this.lblDescription2.ChildPanel.PerformLayout();
            this.lblDescription2.ResumeLayout(false);
            this.lblShip.ChildPanel.ResumeLayout(false);
            this.lblShip.ChildPanel.PerformLayout();
            this.lblShip.ResumeLayout(false);
            this.lblKojiName.ChildPanel.ResumeLayout(false);
            this.lblKojiName.ChildPanel.PerformLayout();
            this.lblKojiName.ResumeLayout(false);
            this.lblKojiNo.ChildPanel.ResumeLayout(false);
            this.lblKojiNo.ChildPanel.PerformLayout();
            this.lblKojiNo.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected DSWControl.DSWLabel.DSWLabel lblShip;
        protected DSWControl.DSWTextBox.DSWTextBox txtShip;
        protected DSWControl.DSWLabel.DSWLabel lblKojiName;
        protected DSWControl.DSWTextBox.DSWTextBox txtKojiName;
        protected DSWControl.DSWLabel.DSWLabel lblKojiNo;
        protected DSWControl.DSWTextBox.DSWTextBox txtKojiNo;
        protected DSWControl.DSWControlLinkLabel.DSWControlLinkLabel lblCaseNo;
        protected DSWControl.DSWNumericTextBox txtNetW;
        protected DSWControl.DSWNumericTextBox txtMokuzaiJyuryo;
        protected DSWControl.DSWNumericTextBox txtMMNet;
        protected DSWControl.DSWNumericTextBox txtDimentionH;
        protected DSWControl.DSWNumericTextBox txtDimentionW;
        protected DSWControl.DSWNumericTextBox txtDimentionL;
        protected DSWControl.DSWTextBox.DSWTextBox txtStyle;
        protected DSWControl.DSWControlLinkLabel.DSWControlLinkLabel lblNetW;
        protected DSWControl.DSWControlLinkLabel.DSWControlLinkLabel lblGrossW;
        protected DSWControl.DSWControlLinkLabel.DSWControlLinkLabel lblMokuzaiJyuryo;
        protected DSWControl.DSWControlLinkLabel.DSWControlLinkLabel lblMMNet;
        protected DSWControl.DSWControlLinkLabel.DSWControlLinkLabel lblDimentionH;
        protected DSWControl.DSWControlLinkLabel.DSWControlLinkLabel lblDimentionW;
        protected DSWControl.DSWControlLinkLabel.DSWControlLinkLabel lblDimentionL;
        protected DSWControl.DSWControlLinkLabel.DSWControlLinkLabel lblDimention;
        protected DSWControl.DSWControlLinkLabel.DSWControlLinkLabel lblStyle;
        protected DSWControl.DSWNumericTextBox txtCaseNo;
        protected GrapeCity.Win.ElTabelle.Sheet shtMeisai;
        protected System.Windows.Forms.Label lblNote;
        protected DSWControl.DSWLabel.DSWLabel lblItem;
        protected DSWControl.DSWLabel.DSWLabel lblDescription1;
        protected DSWControl.DSWLabel.DSWLabel lblDescription2;
        protected DSWControl.DSWTextBox.DSWTextBox txtItem;
        protected DSWControl.DSWTextBox.DSWTextBox txtDescription1;
        protected DSWControl.DSWTextBox.DSWTextBox txtDescription2;
        protected System.Windows.Forms.NumericUpDown nudGrossW;
    }
}