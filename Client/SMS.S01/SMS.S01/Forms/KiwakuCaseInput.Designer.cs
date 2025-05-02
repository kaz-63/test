namespace SMS.S01.Forms
{
    partial class KiwakuCaseInput
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KiwakuCaseInput));
            this.lblKojiName = new DSWControl.DSWLabel.DSWLabel();
            this.txtKojiName = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnClose = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnSelect = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.dswTextBox1 = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblShip = new DSWControl.DSWLabel.DSWLabel();
            this.txtShip = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblCaseNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtCaseNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblPrintCaseNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtPrintCaseNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.pnlMain.SuspendLayout();
            this.lblKojiName.ChildPanel.SuspendLayout();
            this.lblKojiName.SuspendLayout();
            this.lblShip.ChildPanel.SuspendLayout();
            this.lblShip.SuspendLayout();
            this.lblCaseNo.ChildPanel.SuspendLayout();
            this.lblCaseNo.SuspendLayout();
            this.lblPrintCaseNo.ChildPanel.SuspendLayout();
            this.lblPrintCaseNo.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.lblPrintCaseNo);
            this.pnlMain.Controls.Add(this.lblCaseNo);
            this.pnlMain.Controls.Add(this.lblShip);
            this.pnlMain.Controls.Add(this.btnSelect);
            this.pnlMain.Controls.Add(this.btnClose);
            this.pnlMain.Controls.Add(this.lblKojiName);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblKojiName, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnClose, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnSelect, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblShip, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCaseNo, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblPrintCaseNo, 0);
            // 
            // pnlTitle
            // 
            resources.ApplyResources(this.pnlTitle, "pnlTitle");
            // 
            // fbrFunction
            // 
            resources.ApplyResources(this.fbrFunction, "fbrFunction");
            // 
            // txtUserName
            // 
            resources.ApplyResources(this.txtUserName, "txtUserName");
            // 
            // txtRoleName
            // 
            resources.ApplyResources(this.txtRoleName, "txtRoleName");
            // 
            // lblCorner
            // 
            resources.ApplyResources(this.lblCorner, "lblCorner");
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // lblKojiName
            // 
            this.lblKojiName.BackColor = System.Drawing.Color.LightSkyBlue;
            // 
            // lblKojiName.ChildPanel
            // 
            this.lblKojiName.ChildPanel.Controls.Add(this.txtKojiName);
            this.lblKojiName.IsFocusChangeColor = false;
            this.lblKojiName.LabelWidth = 84;
            resources.ApplyResources(this.lblKojiName, "lblKojiName");
            this.lblKojiName.Name = "lblKojiName";
            this.lblKojiName.NormalBackColor = System.Drawing.Color.LightSkyBlue;
            this.lblKojiName.SplitterWidth = 0;
            // 
            // txtKojiName
            // 
            resources.ApplyResources(this.txtKojiName, "txtKojiName");
            this.txtKojiName.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtKojiName.InputRegulation = "";
            this.txtKojiName.MaxByteLengthMode = true;
            this.txtKojiName.Name = "txtKojiName";
            this.txtKojiName.OneLineMaxLength = 10;
            this.txtKojiName.ProhibitionChar = null;
            this.txtKojiName.ReadOnly = true;
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.LinkFunctionKey = DSWControl.DSWFunctionButton.FunctionKeys.F12;
            this.btnClose.Name = "btnClose";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSelect
            // 
            resources.ApplyResources(this.btnSelect, "btnSelect");
            this.btnSelect.LinkFunctionKey = DSWControl.DSWFunctionButton.FunctionKeys.F01;
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.UseVisualStyleBackColor = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // dswTextBox1
            // 
            resources.ApplyResources(this.dswTextBox1, "dswTextBox1");
            this.dswTextBox1.FocusBackColor = System.Drawing.SystemColors.Window;
            this.dswTextBox1.InputRegulation = "";
            this.dswTextBox1.MaxByteLengthMode = true;
            this.dswTextBox1.Name = "dswTextBox1";
            this.dswTextBox1.OneLineMaxLength = 10;
            this.dswTextBox1.ProhibitionChar = null;
            // 
            // lblShip
            // 
            this.lblShip.BackColor = System.Drawing.Color.LightSkyBlue;
            // 
            // lblShip.ChildPanel
            // 
            this.lblShip.ChildPanel.Controls.Add(this.txtShip);
            this.lblShip.IsFocusChangeColor = false;
            this.lblShip.LabelWidth = 84;
            resources.ApplyResources(this.lblShip, "lblShip");
            this.lblShip.Name = "lblShip";
            this.lblShip.NormalBackColor = System.Drawing.Color.LightSkyBlue;
            this.lblShip.SplitterWidth = 0;
            // 
            // txtShip
            // 
            resources.ApplyResources(this.txtShip, "txtShip");
            this.txtShip.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtShip.InputRegulation = "";
            this.txtShip.MaxByteLengthMode = true;
            this.txtShip.Name = "txtShip";
            this.txtShip.OneLineMaxLength = 10;
            this.txtShip.ProhibitionChar = null;
            this.txtShip.ReadOnly = true;
            // 
            // lblCaseNo
            // 
            // 
            // lblCaseNo.ChildPanel
            // 
            this.lblCaseNo.ChildPanel.Controls.Add(this.txtCaseNo);
            this.lblCaseNo.IsFocusChangeColor = false;
            this.lblCaseNo.IsNecessary = true;
            this.lblCaseNo.LabelWidth = 84;
            resources.ApplyResources(this.lblCaseNo, "lblCaseNo");
            this.lblCaseNo.Name = "lblCaseNo";
            this.lblCaseNo.SplitterWidth = 0;
            // 
            // txtCaseNo
            // 
            resources.ApplyResources(this.txtCaseNo, "txtCaseNo");
            this.txtCaseNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtCaseNo.InputRegulation = "";
            this.txtCaseNo.MaxByteLengthMode = true;
            this.txtCaseNo.Name = "txtCaseNo";
            this.txtCaseNo.OneLineMaxLength = 10;
            this.txtCaseNo.ProhibitionChar = null;
            // 
            // lblPrintCaseNo
            // 
            // 
            // lblPrintCaseNo.ChildPanel
            // 
            this.lblPrintCaseNo.ChildPanel.Controls.Add(this.txtPrintCaseNo);
            this.lblPrintCaseNo.IsFocusChangeColor = false;
            this.lblPrintCaseNo.IsNecessary = true;
            this.lblPrintCaseNo.LabelWidth = 84;
            resources.ApplyResources(this.lblPrintCaseNo, "lblPrintCaseNo");
            this.lblPrintCaseNo.Name = "lblPrintCaseNo";
            this.lblPrintCaseNo.SplitterWidth = 0;
            // 
            // txtPrintCaseNo
            // 
            resources.ApplyResources(this.txtPrintCaseNo, "txtPrintCaseNo");
            this.txtPrintCaseNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtPrintCaseNo.InputRegulation = "";
            this.txtPrintCaseNo.MaxByteLengthMode = true;
            this.txtPrintCaseNo.Name = "txtPrintCaseNo";
            this.txtPrintCaseNo.OneLineMaxLength = 10;
            this.txtPrintCaseNo.ProhibitionChar = null;
            // 
            // KiwakuCaseInput
            // 
            resources.ApplyResources(this, "$this");
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KiwakuCaseInput";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.lblKojiName.ChildPanel.ResumeLayout(false);
            this.lblKojiName.ChildPanel.PerformLayout();
            this.lblKojiName.ResumeLayout(false);
            this.lblShip.ChildPanel.ResumeLayout(false);
            this.lblShip.ChildPanel.PerformLayout();
            this.lblShip.ResumeLayout(false);
            this.lblCaseNo.ChildPanel.ResumeLayout(false);
            this.lblCaseNo.ChildPanel.PerformLayout();
            this.lblCaseNo.ResumeLayout(false);
            this.lblPrintCaseNo.ChildPanel.ResumeLayout(false);
            this.lblPrintCaseNo.ChildPanel.PerformLayout();
            this.lblPrintCaseNo.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DSWControl.DSWLabel.DSWLabel lblKojiName;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnClose;
        protected DSWControl.DSWFunctionButton.DSWFunctionButton btnSelect;
        private DSWControl.DSWLabel.DSWLabel lblPrintCaseNo;
        private DSWControl.DSWTextBox.DSWTextBox txtPrintCaseNo;
        private DSWControl.DSWLabel.DSWLabel lblCaseNo;
        private DSWControl.DSWTextBox.DSWTextBox txtCaseNo;
        private DSWControl.DSWLabel.DSWLabel lblShip;
        private DSWControl.DSWTextBox.DSWTextBox txtShip;
        private DSWControl.DSWTextBox.DSWTextBox dswTextBox1;
        private DSWControl.DSWTextBox.DSWTextBox txtKojiName;
    }
}
