namespace SMS.K02.Forms
{
    partial class KiwakuKonpo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KiwakuKonpo));
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.btnView = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnListSelect = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblShip = new DSWControl.DSWLabel.DSWLabel();
            this.txtShip = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblKojiName = new DSWControl.DSWLabel.DSWLabel();
            this.txtKojiName = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblKojiNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtKojiNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.shtMeisai = new MultiRowTabelle.MultiRowSheet();
            this.lblCaseMarkFile = new DSWControl.DSWLabel.DSWLabel();
            this.txtCaseMarkFile = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblDeliveryNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtDeliveryNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblPortOfDestination = new DSWControl.DSWLabel.DSWLabel();
            this.txtPortOfDestination = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblAirBoat = new DSWControl.DSWLabel.DSWLabel();
            this.txtAirBoat = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblDeliveryDate = new DSWControl.DSWLabel.DSWLabel();
            this.txtDeliveryDate = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblDeliveryPoint = new DSWControl.DSWLabel.DSWLabel();
            this.txtDeliveryPoint = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblFactory = new DSWControl.DSWLabel.DSWLabel();
            this.txtFactory = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblRemarks = new DSWControl.DSWLabel.DSWLabel();
            this.txtRemarks = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblSagyoFlag = new DSWControl.DSWControlLinkLabel.DSWControlLinkLabel();
            this.pnlSagyoFlag = new System.Windows.Forms.Panel();
            this.rdoKonpoKanryo = new System.Windows.Forms.RadioButton();
            this.rdoKonpoToroku = new System.Windows.Forms.RadioButton();
            this.pnlView = new System.Windows.Forms.Panel();
            this.pnlMain.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.lblShip.ChildPanel.SuspendLayout();
            this.lblShip.SuspendLayout();
            this.lblKojiName.ChildPanel.SuspendLayout();
            this.lblKojiName.SuspendLayout();
            this.lblKojiNo.ChildPanel.SuspendLayout();
            this.lblKojiNo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).BeginInit();
            this.lblCaseMarkFile.ChildPanel.SuspendLayout();
            this.lblCaseMarkFile.SuspendLayout();
            this.lblDeliveryNo.ChildPanel.SuspendLayout();
            this.lblDeliveryNo.SuspendLayout();
            this.lblPortOfDestination.ChildPanel.SuspendLayout();
            this.lblPortOfDestination.SuspendLayout();
            this.lblAirBoat.ChildPanel.SuspendLayout();
            this.lblAirBoat.SuspendLayout();
            this.lblDeliveryDate.ChildPanel.SuspendLayout();
            this.lblDeliveryDate.SuspendLayout();
            this.lblDeliveryPoint.ChildPanel.SuspendLayout();
            this.lblDeliveryPoint.SuspendLayout();
            this.lblFactory.ChildPanel.SuspendLayout();
            this.lblFactory.SuspendLayout();
            this.lblRemarks.ChildPanel.SuspendLayout();
            this.lblRemarks.SuspendLayout();
            this.pnlSagyoFlag.SuspendLayout();
            this.pnlView.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.grpSearch);
            this.pnlMain.Controls.Add(this.shtMeisai);
            this.pnlMain.Controls.Add(this.pnlView);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.SetChildIndex(this.pnlView, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtMeisai, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
            // 
            // fbrFunction
            // 
            // 
            // fbrFunction.F01Button
            // 
            resources.ApplyResources(this.fbrFunction.F01Button, "fbrFunction.F01Button");
            resources.ApplyResources(this.fbrFunction, "fbrFunction");
            // 
            // fbrFunction.F06Button
            // 
            resources.ApplyResources(this.fbrFunction.F06Button, "fbrFunction.F06Button");
            // 
            // fbrFunction.F07Button
            // 
            resources.ApplyResources(this.fbrFunction.F07Button, "fbrFunction.F07Button");
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // grpSearch
            // 
            this.grpSearch.Controls.Add(this.btnView);
            this.grpSearch.Controls.Add(this.btnListSelect);
            this.grpSearch.Controls.Add(this.lblShip);
            this.grpSearch.Controls.Add(this.lblKojiName);
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
            // 
            // btnView
            // 
            resources.ApplyResources(this.btnView, "btnView");
            this.btnView.Name = "btnView";
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnListSelect
            // 
            resources.ApplyResources(this.btnListSelect, "btnListSelect");
            this.btnListSelect.Name = "btnListSelect";
            this.btnListSelect.TabStop = false;
            // 
            // lblShip
            // 
            // 
            // lblShip.ChildPanel
            // 
            this.lblShip.ChildPanel.Controls.Add(this.txtShip);
            this.lblShip.IsFocusChangeColor = false;
            this.lblShip.IsNecessary = true;
            this.lblShip.LabelWidth = 80;
            resources.ApplyResources(this.lblShip, "lblShip");
            this.lblShip.Name = "lblShip";
            this.lblShip.SplitterWidth = 0;
            // 
            // txtShip
            // 
            resources.ApplyResources(this.txtShip, "txtShip");
            this.txtShip.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtShip.InputRegulation = "F";
            this.txtShip.MaxByteLengthMode = true;
            this.txtShip.Name = "txtShip";
            this.txtShip.OneLineMaxLength = 10;
            this.txtShip.ProhibitionChar = null;
            // 
            // lblKojiName
            // 
            // 
            // lblKojiName.ChildPanel
            // 
            this.lblKojiName.ChildPanel.Controls.Add(this.txtKojiName);
            this.lblKojiName.IsFocusChangeColor = false;
            this.lblKojiName.IsNecessary = true;
            this.lblKojiName.LabelWidth = 100;
            resources.ApplyResources(this.lblKojiName, "lblKojiName");
            this.lblKojiName.Name = "lblKojiName";
            this.lblKojiName.SplitterWidth = 0;
            // 
            // txtKojiName
            // 
            resources.ApplyResources(this.txtKojiName, "txtKojiName");
            this.txtKojiName.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtKojiName.InputRegulation = "F";
            this.txtKojiName.MaxByteLengthMode = true;
            this.txtKojiName.Name = "txtKojiName";
            this.txtKojiName.OneLineMaxLength = 60;
            this.txtKojiName.ProhibitionChar = null;
            // 
            // lblKojiNo
            // 
            this.lblKojiNo.BackColor = System.Drawing.Color.Transparent;
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
            this.txtKojiNo.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.txtKojiNo, "txtKojiNo");
            this.txtKojiNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtKojiNo.InputRegulation = "";
            this.txtKojiNo.Name = "txtKojiNo";
            this.txtKojiNo.ProhibitionChar = null;
            this.txtKojiNo.ReadOnly = true;
            this.txtKojiNo.TabStop = false;
            // 
            // shtMeisai
            // 
            resources.ApplyResources(this.shtMeisai, "shtMeisai");
            this.shtMeisai.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtMeisai.Data")));
            this.shtMeisai.Name = "shtMeisai";
            this.shtMeisai.CellNotify += new GrapeCity.Win.ElTabelle.CellNotifyEventHandler(this.shtMeisai_CellNotify);
            // 
            // lblCaseMarkFile
            // 
            this.lblCaseMarkFile.BackColor = System.Drawing.Color.Transparent;
            // 
            // lblCaseMarkFile.ChildPanel
            // 
            this.lblCaseMarkFile.ChildPanel.Controls.Add(this.txtCaseMarkFile);
            this.lblCaseMarkFile.IsFocusChangeColor = false;
            this.lblCaseMarkFile.LabelWidth = 160;
            resources.ApplyResources(this.lblCaseMarkFile, "lblCaseMarkFile");
            this.lblCaseMarkFile.Name = "lblCaseMarkFile";
            this.lblCaseMarkFile.SplitterWidth = 0;
            this.lblCaseMarkFile.TabStop = false;
            // 
            // txtCaseMarkFile
            // 
            resources.ApplyResources(this.txtCaseMarkFile, "txtCaseMarkFile");
            this.txtCaseMarkFile.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtCaseMarkFile.InputRegulation = "";
            this.txtCaseMarkFile.Name = "txtCaseMarkFile";
            this.txtCaseMarkFile.ProhibitionChar = null;
            this.txtCaseMarkFile.ReadOnly = true;
            this.txtCaseMarkFile.TabStop = false;
            // 
            // lblDeliveryNo
            // 
            this.lblDeliveryNo.BackColor = System.Drawing.Color.Transparent;
            // 
            // lblDeliveryNo.ChildPanel
            // 
            this.lblDeliveryNo.ChildPanel.Controls.Add(this.txtDeliveryNo);
            this.lblDeliveryNo.IsFocusChangeColor = false;
            this.lblDeliveryNo.LabelWidth = 160;
            resources.ApplyResources(this.lblDeliveryNo, "lblDeliveryNo");
            this.lblDeliveryNo.Name = "lblDeliveryNo";
            this.lblDeliveryNo.SplitterWidth = 0;
            this.lblDeliveryNo.TabStop = false;
            // 
            // txtDeliveryNo
            // 
            resources.ApplyResources(this.txtDeliveryNo, "txtDeliveryNo");
            this.txtDeliveryNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtDeliveryNo.InputRegulation = "";
            this.txtDeliveryNo.Name = "txtDeliveryNo";
            this.txtDeliveryNo.ProhibitionChar = null;
            this.txtDeliveryNo.ReadOnly = true;
            this.txtDeliveryNo.TabStop = false;
            // 
            // lblPortOfDestination
            // 
            this.lblPortOfDestination.BackColor = System.Drawing.Color.Transparent;
            // 
            // lblPortOfDestination.ChildPanel
            // 
            this.lblPortOfDestination.ChildPanel.Controls.Add(this.txtPortOfDestination);
            this.lblPortOfDestination.IsFocusChangeColor = false;
            this.lblPortOfDestination.LabelWidth = 160;
            resources.ApplyResources(this.lblPortOfDestination, "lblPortOfDestination");
            this.lblPortOfDestination.Name = "lblPortOfDestination";
            this.lblPortOfDestination.SplitterWidth = 0;
            this.lblPortOfDestination.TabStop = false;
            // 
            // txtPortOfDestination
            // 
            resources.ApplyResources(this.txtPortOfDestination, "txtPortOfDestination");
            this.txtPortOfDestination.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtPortOfDestination.InputRegulation = "";
            this.txtPortOfDestination.Name = "txtPortOfDestination";
            this.txtPortOfDestination.ProhibitionChar = null;
            this.txtPortOfDestination.ReadOnly = true;
            this.txtPortOfDestination.TabStop = false;
            // 
            // lblAirBoat
            // 
            this.lblAirBoat.BackColor = System.Drawing.Color.Transparent;
            // 
            // lblAirBoat.ChildPanel
            // 
            this.lblAirBoat.ChildPanel.Controls.Add(this.txtAirBoat);
            this.lblAirBoat.IsFocusChangeColor = false;
            this.lblAirBoat.LabelWidth = 160;
            resources.ApplyResources(this.lblAirBoat, "lblAirBoat");
            this.lblAirBoat.Name = "lblAirBoat";
            this.lblAirBoat.SplitterWidth = 0;
            this.lblAirBoat.TabStop = false;
            // 
            // txtAirBoat
            // 
            resources.ApplyResources(this.txtAirBoat, "txtAirBoat");
            this.txtAirBoat.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtAirBoat.InputRegulation = "";
            this.txtAirBoat.Name = "txtAirBoat";
            this.txtAirBoat.ProhibitionChar = null;
            this.txtAirBoat.ReadOnly = true;
            this.txtAirBoat.TabStop = false;
            // 
            // lblDeliveryDate
            // 
            this.lblDeliveryDate.BackColor = System.Drawing.Color.Transparent;
            // 
            // lblDeliveryDate.ChildPanel
            // 
            this.lblDeliveryDate.ChildPanel.Controls.Add(this.txtDeliveryDate);
            this.lblDeliveryDate.IsFocusChangeColor = false;
            this.lblDeliveryDate.LabelWidth = 120;
            resources.ApplyResources(this.lblDeliveryDate, "lblDeliveryDate");
            this.lblDeliveryDate.Name = "lblDeliveryDate";
            this.lblDeliveryDate.SplitterWidth = 0;
            this.lblDeliveryDate.TabStop = false;
            // 
            // txtDeliveryDate
            // 
            resources.ApplyResources(this.txtDeliveryDate, "txtDeliveryDate");
            this.txtDeliveryDate.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtDeliveryDate.InputRegulation = "";
            this.txtDeliveryDate.Name = "txtDeliveryDate";
            this.txtDeliveryDate.ProhibitionChar = null;
            this.txtDeliveryDate.ReadOnly = true;
            this.txtDeliveryDate.TabStop = false;
            // 
            // lblDeliveryPoint
            // 
            this.lblDeliveryPoint.BackColor = System.Drawing.Color.Transparent;
            // 
            // lblDeliveryPoint.ChildPanel
            // 
            this.lblDeliveryPoint.ChildPanel.Controls.Add(this.txtDeliveryPoint);
            this.lblDeliveryPoint.IsFocusChangeColor = false;
            this.lblDeliveryPoint.LabelWidth = 120;
            resources.ApplyResources(this.lblDeliveryPoint, "lblDeliveryPoint");
            this.lblDeliveryPoint.Name = "lblDeliveryPoint";
            this.lblDeliveryPoint.SplitterWidth = 0;
            this.lblDeliveryPoint.TabStop = false;
            // 
            // txtDeliveryPoint
            // 
            resources.ApplyResources(this.txtDeliveryPoint, "txtDeliveryPoint");
            this.txtDeliveryPoint.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtDeliveryPoint.InputRegulation = "";
            this.txtDeliveryPoint.Name = "txtDeliveryPoint";
            this.txtDeliveryPoint.ProhibitionChar = null;
            this.txtDeliveryPoint.ReadOnly = true;
            this.txtDeliveryPoint.TabStop = false;
            // 
            // lblFactory
            // 
            this.lblFactory.BackColor = System.Drawing.Color.Transparent;
            // 
            // lblFactory.ChildPanel
            // 
            this.lblFactory.ChildPanel.Controls.Add(this.txtFactory);
            this.lblFactory.IsFocusChangeColor = false;
            this.lblFactory.LabelWidth = 120;
            resources.ApplyResources(this.lblFactory, "lblFactory");
            this.lblFactory.Name = "lblFactory";
            this.lblFactory.SplitterWidth = 0;
            this.lblFactory.TabStop = false;
            // 
            // txtFactory
            // 
            resources.ApplyResources(this.txtFactory, "txtFactory");
            this.txtFactory.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtFactory.InputRegulation = "";
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.ProhibitionChar = null;
            this.txtFactory.ReadOnly = true;
            this.txtFactory.TabStop = false;
            // 
            // lblRemarks
            // 
            this.lblRemarks.BackColor = System.Drawing.Color.Transparent;
            // 
            // lblRemarks.ChildPanel
            // 
            this.lblRemarks.ChildPanel.Controls.Add(this.txtRemarks);
            this.lblRemarks.IsFocusChangeColor = false;
            this.lblRemarks.LabelWidth = 120;
            resources.ApplyResources(this.lblRemarks, "lblRemarks");
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.SplitterWidth = 0;
            this.lblRemarks.TabStop = false;
            // 
            // txtRemarks
            // 
            resources.ApplyResources(this.txtRemarks, "txtRemarks");
            this.txtRemarks.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtRemarks.InputRegulation = "";
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.ProhibitionChar = null;
            this.txtRemarks.ReadOnly = true;
            this.txtRemarks.TabStop = false;
            // 
            // lblSagyoFlag
            // 
            resources.ApplyResources(this.lblSagyoFlag, "lblSagyoFlag");
            this.lblSagyoFlag.Name = "lblSagyoFlag";
            // 
            // pnlSagyoFlag
            // 
            this.pnlSagyoFlag.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlSagyoFlag.Controls.Add(this.rdoKonpoKanryo);
            this.pnlSagyoFlag.Controls.Add(this.rdoKonpoToroku);
            resources.ApplyResources(this.pnlSagyoFlag, "pnlSagyoFlag");
            this.pnlSagyoFlag.Name = "pnlSagyoFlag";
            // 
            // rdoKonpoKanryo
            // 
            resources.ApplyResources(this.rdoKonpoKanryo, "rdoKonpoKanryo");
            this.rdoKonpoKanryo.Name = "rdoKonpoKanryo";
            this.rdoKonpoKanryo.TabStop = true;
            this.rdoKonpoKanryo.UseVisualStyleBackColor = true;
            // 
            // rdoKonpoToroku
            // 
            resources.ApplyResources(this.rdoKonpoToroku, "rdoKonpoToroku");
            this.rdoKonpoToroku.Name = "rdoKonpoToroku";
            this.rdoKonpoToroku.TabStop = true;
            this.rdoKonpoToroku.UseVisualStyleBackColor = true;
            // 
            // pnlView
            // 
            this.pnlView.Controls.Add(this.lblKojiNo);
            this.pnlView.Controls.Add(this.lblDeliveryNo);
            this.pnlView.Controls.Add(this.lblDeliveryPoint);
            this.pnlView.Controls.Add(this.lblDeliveryDate);
            this.pnlView.Controls.Add(this.lblFactory);
            this.pnlView.Controls.Add(this.lblAirBoat);
            this.pnlView.Controls.Add(this.lblRemarks);
            this.pnlView.Controls.Add(this.lblPortOfDestination);
            this.pnlView.Controls.Add(this.pnlSagyoFlag);
            this.pnlView.Controls.Add(this.lblSagyoFlag);
            this.pnlView.Controls.Add(this.lblCaseMarkFile);
            resources.ApplyResources(this.pnlView, "pnlView");
            this.pnlView.Name = "pnlView";
            // 
            // KiwakuKonpo
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "KiwakuKonpo";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.grpSearch.ResumeLayout(false);
            this.lblShip.ChildPanel.ResumeLayout(false);
            this.lblShip.ChildPanel.PerformLayout();
            this.lblShip.ResumeLayout(false);
            this.lblKojiName.ChildPanel.ResumeLayout(false);
            this.lblKojiName.ChildPanel.PerformLayout();
            this.lblKojiName.ResumeLayout(false);
            this.lblKojiNo.ChildPanel.ResumeLayout(false);
            this.lblKojiNo.ChildPanel.PerformLayout();
            this.lblKojiNo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).EndInit();
            this.lblCaseMarkFile.ChildPanel.ResumeLayout(false);
            this.lblCaseMarkFile.ChildPanel.PerformLayout();
            this.lblCaseMarkFile.ResumeLayout(false);
            this.lblDeliveryNo.ChildPanel.ResumeLayout(false);
            this.lblDeliveryNo.ChildPanel.PerformLayout();
            this.lblDeliveryNo.ResumeLayout(false);
            this.lblPortOfDestination.ChildPanel.ResumeLayout(false);
            this.lblPortOfDestination.ChildPanel.PerformLayout();
            this.lblPortOfDestination.ResumeLayout(false);
            this.lblAirBoat.ChildPanel.ResumeLayout(false);
            this.lblAirBoat.ChildPanel.PerformLayout();
            this.lblAirBoat.ResumeLayout(false);
            this.lblDeliveryDate.ChildPanel.ResumeLayout(false);
            this.lblDeliveryDate.ChildPanel.PerformLayout();
            this.lblDeliveryDate.ResumeLayout(false);
            this.lblDeliveryPoint.ChildPanel.ResumeLayout(false);
            this.lblDeliveryPoint.ChildPanel.PerformLayout();
            this.lblDeliveryPoint.ResumeLayout(false);
            this.lblFactory.ChildPanel.ResumeLayout(false);
            this.lblFactory.ChildPanel.PerformLayout();
            this.lblFactory.ResumeLayout(false);
            this.lblRemarks.ChildPanel.ResumeLayout(false);
            this.lblRemarks.ChildPanel.PerformLayout();
            this.lblRemarks.ResumeLayout(false);
            this.pnlSagyoFlag.ResumeLayout(false);
            this.pnlSagyoFlag.PerformLayout();
            this.pnlView.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.GroupBox grpSearch;
        protected DSWControl.DSWFunctionButton.DSWFunctionButton btnView;
        protected DSWControl.DSWFunctionButton.DSWFunctionButton btnListSelect;
        protected DSWControl.DSWLabel.DSWLabel lblKojiNo;
        protected DSWControl.DSWTextBox.DSWTextBox txtKojiNo;
        protected DSWControl.DSWLabel.DSWLabel lblShip;
        protected DSWControl.DSWTextBox.DSWTextBox txtShip;
        protected DSWControl.DSWLabel.DSWLabel lblKojiName;
        protected DSWControl.DSWTextBox.DSWTextBox txtKojiName;
        protected MultiRowTabelle.MultiRowSheet shtMeisai;
        protected DSWControl.DSWLabel.DSWLabel lblCaseMarkFile;
        protected DSWControl.DSWTextBox.DSWTextBox txtCaseMarkFile;
        protected DSWControl.DSWLabel.DSWLabel lblAirBoat;
        protected DSWControl.DSWTextBox.DSWTextBox txtAirBoat;
        protected DSWControl.DSWLabel.DSWLabel lblPortOfDestination;
        protected DSWControl.DSWTextBox.DSWTextBox txtPortOfDestination;
        protected DSWControl.DSWLabel.DSWLabel lblRemarks;
        protected DSWControl.DSWTextBox.DSWTextBox txtRemarks;
        protected DSWControl.DSWLabel.DSWLabel lblDeliveryNo;
        protected DSWControl.DSWTextBox.DSWTextBox txtDeliveryNo;
        protected DSWControl.DSWLabel.DSWLabel lblFactory;
        protected DSWControl.DSWTextBox.DSWTextBox txtFactory;
        protected DSWControl.DSWLabel.DSWLabel lblDeliveryPoint;
        protected DSWControl.DSWTextBox.DSWTextBox txtDeliveryPoint;
        protected DSWControl.DSWLabel.DSWLabel lblDeliveryDate;
        protected DSWControl.DSWTextBox.DSWTextBox txtDeliveryDate;
        protected System.Windows.Forms.Panel pnlSagyoFlag;
        protected DSWControl.DSWControlLinkLabel.DSWControlLinkLabel lblSagyoFlag;
        protected System.Windows.Forms.RadioButton rdoKonpoKanryo;
        protected System.Windows.Forms.RadioButton rdoKonpoToroku;
        private System.Windows.Forms.Panel pnlView;


    }
}