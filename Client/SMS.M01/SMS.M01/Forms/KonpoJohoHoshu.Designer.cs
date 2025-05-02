namespace SMS.M01.Forms
{
    partial class KonpoJohoHoshu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KonpoJohoHoshu));
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.btnView = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblKonpoNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtKonpoNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblKonpo = new DSWControl.DSWControlLinkLabel.DSWControlLinkLabel();
            this.lblKonpoTani = new DSWControl.DSWLabel.DSWLabel();
            this.cboKonpoTani = new DSWControl.DSWComboBox.DSWComboBox();
            this.shtMeisai = new GrapeCity.Win.ElTabelle.Sheet();
            this.btnAllDeselect = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnAllSelect = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnKonpoAdd = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.pnlMain.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.lblKonpoNo.ChildPanel.SuspendLayout();
            this.lblKonpoNo.SuspendLayout();
            this.lblKonpoTani.ChildPanel.SuspendLayout();
            this.lblKonpoTani.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.btnKonpoAdd);
            this.pnlMain.Controls.Add(this.btnAllDeselect);
            this.pnlMain.Controls.Add(this.btnAllSelect);
            this.pnlMain.Controls.Add(this.shtMeisai);
            this.pnlMain.Controls.Add(this.grpSearch);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtMeisai, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnAllSelect, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnAllDeselect, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnKonpoAdd, 0);
            // 
            // fbrFunction
            // 
            // 
            // fbrFunction.F01Button
            // 
            resources.ApplyResources(this.fbrFunction.F01Button, "fbrFunction.F01Button");
            resources.ApplyResources(this.fbrFunction, "fbrFunction");
            // 
            // fbrFunction.F03Button
            // 
            resources.ApplyResources(this.fbrFunction.F03Button, "fbrFunction.F03Button");
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
            this.grpSearch.Controls.Add(this.lblKonpoNo);
            this.grpSearch.Controls.Add(this.lblKonpoTani);
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
            // lblKonpoNo
            // 
            // 
            // lblKonpoNo.ChildPanel
            // 
            this.lblKonpoNo.ChildPanel.Controls.Add(this.txtKonpoNo);
            this.lblKonpoNo.ChildPanel.Controls.Add(this.lblKonpo);
            this.lblKonpoNo.IsFocusChangeColor = false;
            this.lblKonpoNo.IsNecessary = true;
            this.lblKonpoNo.LabelWidth = 80;
            resources.ApplyResources(this.lblKonpoNo, "lblKonpoNo");
            this.lblKonpoNo.Name = "lblKonpoNo";
            this.lblKonpoNo.SplitterWidth = 0;
            // 
            // txtKonpoNo
            // 
            this.txtKonpoNo.AutoPad = true;
            this.txtKonpoNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            resources.ApplyResources(this.txtKonpoNo, "txtKonpoNo");
            this.txtKonpoNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtKonpoNo.InputRegulation = "nab";
            this.txtKonpoNo.IsInputRegulation = true;
            this.txtKonpoNo.MaxByteLengthMode = true;
            this.txtKonpoNo.Name = "txtKonpoNo";
            this.txtKonpoNo.OneLineMaxLength = 5;
            this.txtKonpoNo.PaddingChar = '0';
            this.txtKonpoNo.ProhibitionChar = null;
            // 
            // lblKonpo
            // 
            this.lblKonpo.BackColor = System.Drawing.Color.LightSkyBlue;
            resources.ApplyResources(this.lblKonpo, "lblKonpo");
            this.lblKonpo.IsFocusChangeColor = false;
            this.lblKonpo.Name = "lblKonpo";
            this.lblKonpo.NormalBackColor = System.Drawing.Color.LightSkyBlue;
            // 
            // lblKonpoTani
            // 
            // 
            // lblKonpoTani.ChildPanel
            // 
            this.lblKonpoTani.ChildPanel.Controls.Add(this.cboKonpoTani);
            this.lblKonpoTani.IsFocusChangeColor = false;
            this.lblKonpoTani.IsNecessary = true;
            this.lblKonpoTani.LabelWidth = 80;
            resources.ApplyResources(this.lblKonpoTani, "lblKonpoTani");
            this.lblKonpoTani.Name = "lblKonpoTani";
            this.lblKonpoTani.SplitterWidth = 0;
            // 
            // cboKonpoTani
            // 
            resources.ApplyResources(this.cboKonpoTani, "cboKonpoTani");
            this.cboKonpoTani.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboKonpoTani.Name = "cboKonpoTani";
            this.cboKonpoTani.SelectedIndexChanged += new System.EventHandler(this.cboShukkaTani_SelectedIndexChanged);
            // 
            // shtMeisai
            // 
            resources.ApplyResources(this.shtMeisai, "shtMeisai");
            this.shtMeisai.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtMeisai.Data")));
            this.shtMeisai.Name = "shtMeisai";
            this.shtMeisai.CellNotify += new GrapeCity.Win.ElTabelle.CellNotifyEventHandler(this.shtMeisai_CellNotify);
            this.shtMeisai.RowInserted += new GrapeCity.Win.ElTabelle.RowInsertedEventHandler(this.shtMeisai_RowInserted);
            this.shtMeisai.LeaveEdit += new GrapeCity.Win.ElTabelle.LeaveEditEventHandler(this.shtMeisai_LeaveEdit);
            this.shtMeisai.ClippingData += new GrapeCity.Win.ElTabelle.ClippingDataEventHandler(this.shtMeisai_ClippingData);
            // 
            // btnAllDeselect
            // 
            resources.ApplyResources(this.btnAllDeselect, "btnAllDeselect");
            this.btnAllDeselect.Name = "btnAllDeselect";
            this.btnAllDeselect.TabStop = false;
            this.btnAllDeselect.Click += new System.EventHandler(this.btnAllDeselect_Click);
            // 
            // btnAllSelect
            // 
            resources.ApplyResources(this.btnAllSelect, "btnAllSelect");
            this.btnAllSelect.Name = "btnAllSelect";
            this.btnAllSelect.TabStop = false;
            this.btnAllSelect.Click += new System.EventHandler(this.btnAllSelect_Click);
            // 
            // btnKonpoAdd
            // 
            resources.ApplyResources(this.btnKonpoAdd, "btnKonpoAdd");
            this.btnKonpoAdd.Name = "btnKonpoAdd";
            this.btnKonpoAdd.TabStop = false;
            this.btnKonpoAdd.Click += new System.EventHandler(this.btnKonpoAdd_Click);
            // 
            // KonpoJohoHoshu
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "KonpoJohoHoshu";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.grpSearch.ResumeLayout(false);
            this.lblKonpoNo.ChildPanel.ResumeLayout(false);
            this.lblKonpoNo.ChildPanel.PerformLayout();
            this.lblKonpoNo.ResumeLayout(false);
            this.lblKonpoTani.ChildPanel.ResumeLayout(false);
            this.lblKonpoTani.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DSWControl.DSWFunctionButton.DSWFunctionButton btnAllDeselect;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnAllSelect;
        private GrapeCity.Win.ElTabelle.Sheet shtMeisai;
        private System.Windows.Forms.GroupBox grpSearch;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnView;
        private DSWControl.DSWLabel.DSWLabel lblKonpoNo;
        private DSWControl.DSWTextBox.DSWTextBox txtKonpoNo;
        private DSWControl.DSWControlLinkLabel.DSWControlLinkLabel lblKonpo;
        private DSWControl.DSWLabel.DSWLabel lblKonpoTani;
        private DSWControl.DSWComboBox.DSWComboBox cboKonpoTani;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnKonpoAdd;
    }
}