namespace SMS.K02.Forms
{
    partial class PackingListHakko
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackingListHakko));
            this.btnAllCheck = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnAllNotCheck = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.shtPackingList = new GrapeCity.Win.ElTabelle.Sheet();
            this.lblKojiShikibetsu = new DSWControl.DSWLabel.DSWLabel();
            this.txtKojiShikibetsu = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblShip = new DSWControl.DSWLabel.DSWLabel();
            this.txtShip = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblHakkoSelect = new DSWControl.DSWLabel.DSWLabel();
            this.cboHakkoSelect = new DSWControl.DSWComboBox.DSWComboBox();
            this.btnDisp = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnRangeNotCheck = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnRangeCheck = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.pnlListSelect = new System.Windows.Forms.Panel();
            this.btnDisp_Select = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.pnlKoji = new System.Windows.Forms.Panel();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtPackingList)).BeginInit();
            this.lblKojiShikibetsu.ChildPanel.SuspendLayout();
            this.lblKojiShikibetsu.SuspendLayout();
            this.lblShip.ChildPanel.SuspendLayout();
            this.lblShip.SuspendLayout();
            this.lblHakkoSelect.ChildPanel.SuspendLayout();
            this.lblHakkoSelect.SuspendLayout();
            this.pnlListSelect.SuspendLayout();
            this.pnlKoji.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnlListSelect);
            this.pnlMain.Controls.Add(this.btnRangeNotCheck);
            this.pnlMain.Controls.Add(this.btnRangeCheck);
            this.pnlMain.Controls.Add(this.shtPackingList);
            this.pnlMain.Controls.Add(this.btnAllNotCheck);
            this.pnlMain.Controls.Add(this.btnAllCheck);
            this.pnlMain.Controls.Add(this.pnlKoji);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.SetChildIndex(this.pnlKoji, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnAllCheck, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnAllNotCheck, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtPackingList, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnRangeCheck, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnRangeNotCheck, 0);
            this.pnlMain.Controls.SetChildIndex(this.pnlListSelect, 0);
            // 
            // fbrFunction
            // 
            // 
            // fbrFunction.F01Button
            // 
            resources.ApplyResources(this.fbrFunction.F01Button, "fbrFunction.F01Button");
            resources.ApplyResources(this.fbrFunction, "fbrFunction");
            // 
            // fbrFunction.F02Button
            // 
            resources.ApplyResources(this.fbrFunction.F02Button, "fbrFunction.F02Button");
            // 
            // fbrFunction.F06Button
            // 
            resources.ApplyResources(this.fbrFunction.F06Button, "fbrFunction.F06Button");
            // 
            // fbrFunction.F07Button
            // 
            resources.ApplyResources(this.fbrFunction.F07Button, "fbrFunction.F07Button");
            // 
            // fbrFunction.F10Button
            // 
            resources.ApplyResources(this.fbrFunction.F10Button, "fbrFunction.F10Button");
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // btnAllCheck
            // 
            resources.ApplyResources(this.btnAllCheck, "btnAllCheck");
            this.btnAllCheck.Name = "btnAllCheck";
            this.btnAllCheck.Click += new System.EventHandler(this.btnAllCheck_Click);
            // 
            // btnAllNotCheck
            // 
            resources.ApplyResources(this.btnAllNotCheck, "btnAllNotCheck");
            this.btnAllNotCheck.Name = "btnAllNotCheck";
            this.btnAllNotCheck.Click += new System.EventHandler(this.btnAllNotCheck_Click);
            // 
            // shtPackingList
            // 
            resources.ApplyResources(this.shtPackingList, "shtPackingList");
            this.shtPackingList.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtPackingList.Data")));
            this.shtPackingList.Name = "shtPackingList";
            // 
            // lblKojiShikibetsu
            // 
            // 
            // lblKojiShikibetsu.ChildPanel
            // 
            this.lblKojiShikibetsu.ChildPanel.Controls.Add(this.txtKojiShikibetsu);
            this.lblKojiShikibetsu.IsFocusChangeColor = false;
            this.lblKojiShikibetsu.IsNecessary = true;
            this.lblKojiShikibetsu.LabelWidth = 100;
            resources.ApplyResources(this.lblKojiShikibetsu, "lblKojiShikibetsu");
            this.lblKojiShikibetsu.Name = "lblKojiShikibetsu";
            this.lblKojiShikibetsu.SplitterWidth = 0;
            // 
            // txtKojiShikibetsu
            // 
            resources.ApplyResources(this.txtKojiShikibetsu, "txtKojiShikibetsu");
            this.txtKojiShikibetsu.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtKojiShikibetsu.InputRegulation = "F";
            this.txtKojiShikibetsu.MaxByteLengthMode = true;
            this.txtKojiShikibetsu.Name = "txtKojiShikibetsu";
            this.txtKojiShikibetsu.OneLineMaxLength = 60;
            this.txtKojiShikibetsu.ProhibitionChar = null;
            this.txtKojiShikibetsu.Leave += new System.EventHandler(this.txtKojiShikibetsu_Leave);
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
            resources.ApplyResources(this.txtShip, "txtShip");
            this.txtShip.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtShip.InputRegulation = "F";
            this.txtShip.MaxByteLengthMode = true;
            this.txtShip.Name = "txtShip";
            this.txtShip.OneLineMaxLength = 10;
            this.txtShip.ProhibitionChar = null;
            this.txtShip.Leave += new System.EventHandler(this.txtShip_Leave);
            // 
            // lblHakkoSelect
            // 
            // 
            // lblHakkoSelect.ChildPanel
            // 
            this.lblHakkoSelect.ChildPanel.Controls.Add(this.cboHakkoSelect);
            this.lblHakkoSelect.IsFocusChangeColor = false;
            this.lblHakkoSelect.LabelTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHakkoSelect.LabelWidth = 63;
            resources.ApplyResources(this.lblHakkoSelect, "lblHakkoSelect");
            this.lblHakkoSelect.Name = "lblHakkoSelect";
            this.lblHakkoSelect.SplitterWidth = 0;
            // 
            // cboHakkoSelect
            // 
            resources.ApplyResources(this.cboHakkoSelect, "cboHakkoSelect");
            this.cboHakkoSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHakkoSelect.Name = "cboHakkoSelect";
            // 
            // btnDisp
            // 
            resources.ApplyResources(this.btnDisp, "btnDisp");
            this.btnDisp.Name = "btnDisp";
            this.btnDisp.Click += new System.EventHandler(this.btnDisp_Click);
            // 
            // btnRangeNotCheck
            // 
            resources.ApplyResources(this.btnRangeNotCheck, "btnRangeNotCheck");
            this.btnRangeNotCheck.Name = "btnRangeNotCheck";
            this.btnRangeNotCheck.Click += new System.EventHandler(this.btnRangeNotCheck_Click);
            // 
            // btnRangeCheck
            // 
            resources.ApplyResources(this.btnRangeCheck, "btnRangeCheck");
            this.btnRangeCheck.Name = "btnRangeCheck";
            this.btnRangeCheck.Click += new System.EventHandler(this.btnRangeCheck_Click);
            // 
            // pnlListSelect
            // 
            this.pnlListSelect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlListSelect.Controls.Add(this.btnDisp_Select);
            this.pnlListSelect.Controls.Add(this.lblHakkoSelect);
            resources.ApplyResources(this.pnlListSelect, "pnlListSelect");
            this.pnlListSelect.Name = "pnlListSelect";
            // 
            // btnDisp_Select
            // 
            resources.ApplyResources(this.btnDisp_Select, "btnDisp_Select");
            this.btnDisp_Select.Name = "btnDisp_Select";
            this.btnDisp_Select.Click += new System.EventHandler(this.btnDisp_Select_Click);
            // 
            // pnlKoji
            // 
            this.pnlKoji.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlKoji.Controls.Add(this.lblKojiShikibetsu);
            this.pnlKoji.Controls.Add(this.btnDisp);
            this.pnlKoji.Controls.Add(this.lblShip);
            resources.ApplyResources(this.pnlKoji, "pnlKoji");
            this.pnlKoji.Name = "pnlKoji";
            // 
            // PackingListHakko
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "PackingListHakko";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtPackingList)).EndInit();
            this.lblKojiShikibetsu.ChildPanel.ResumeLayout(false);
            this.lblKojiShikibetsu.ChildPanel.PerformLayout();
            this.lblKojiShikibetsu.ResumeLayout(false);
            this.lblShip.ChildPanel.ResumeLayout(false);
            this.lblShip.ChildPanel.PerformLayout();
            this.lblShip.ResumeLayout(false);
            this.lblHakkoSelect.ChildPanel.ResumeLayout(false);
            this.lblHakkoSelect.ResumeLayout(false);
            this.pnlListSelect.ResumeLayout(false);
            this.pnlKoji.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DSWControl.DSWFunctionButton.DSWFunctionButton btnAllCheck;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnAllNotCheck;
        private GrapeCity.Win.ElTabelle.Sheet shtPackingList;
        private DSWControl.DSWLabel.DSWLabel lblKojiShikibetsu;
        private DSWControl.DSWTextBox.DSWTextBox txtKojiShikibetsu;
        private DSWControl.DSWLabel.DSWLabel lblShip;
        private DSWControl.DSWTextBox.DSWTextBox txtShip;
        private DSWControl.DSWLabel.DSWLabel lblHakkoSelect;
        private DSWControl.DSWComboBox.DSWComboBox cboHakkoSelect;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnDisp;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnRangeNotCheck;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnRangeCheck;
        private System.Windows.Forms.Panel pnlListSelect;
        private System.Windows.Forms.Panel pnlKoji;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnDisp_Select;
    }
}