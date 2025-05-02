namespace SMS.P02.Forms
{
    partial class NonyusakiIchiranEx
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NonyusakiIchiranEx));
            this.lblNonyusakiName = new DSWControl.DSWLabel.DSWLabel();
            this.txtNonyusakiName = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblShip = new DSWControl.DSWLabel.DSWLabel();
            this.txtShip = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblShukkaFlag = new DSWControl.DSWLabel.DSWLabel();
            this.cboShukkaFlag = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblKanriFlag = new DSWControl.DSWLabel.DSWLabel();
            this.cboKanriFlag = new DSWControl.DSWComboBox.DSWComboBox();
            this.grpSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.lblNonyusakiName.ChildPanel.SuspendLayout();
            this.lblNonyusakiName.SuspendLayout();
            this.lblShip.ChildPanel.SuspendLayout();
            this.lblShip.SuspendLayout();
            this.lblShukkaFlag.ChildPanel.SuspendLayout();
            this.lblShukkaFlag.SuspendLayout();
            this.lblKanriFlag.ChildPanel.SuspendLayout();
            this.lblKanriFlag.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSearch
            // 
            this.grpSearch.Controls.Add(this.lblKanriFlag);
            this.grpSearch.Controls.Add(this.lblNonyusakiName);
            this.grpSearch.Controls.Add(this.lblShip);
            this.grpSearch.Controls.Add(this.lblShukkaFlag);
            this.grpSearch.Controls.SetChildIndex(this.btnSearchAll, 0);
            this.grpSearch.Controls.SetChildIndex(this.btnSearch, 0);
            this.grpSearch.Controls.SetChildIndex(this.lblShukkaFlag, 0);
            this.grpSearch.Controls.SetChildIndex(this.lblShip, 0);
            this.grpSearch.Controls.SetChildIndex(this.lblNonyusakiName, 0);
            this.grpSearch.Controls.SetChildIndex(this.lblKanriFlag, 0);
            // 
            // shtResult
            // 
            this.shtResult.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtResult.Data")));
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // lblNonyusakiName
            // 
            // 
            // lblNonyusakiName.ChildPanel
            // 
            this.lblNonyusakiName.ChildPanel.Controls.Add(this.txtNonyusakiName);
            this.lblNonyusakiName.IsFocusChangeColor = false;
            this.lblNonyusakiName.LabelWidth = 100;
            resources.ApplyResources(this.lblNonyusakiName, "lblNonyusakiName");
            this.lblNonyusakiName.Name = "lblNonyusakiName";
            this.lblNonyusakiName.SplitterWidth = 0;
            // 
            // txtNonyusakiName
            // 
            resources.ApplyResources(this.txtNonyusakiName, "txtNonyusakiName");
            this.txtNonyusakiName.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtNonyusakiName.InputRegulation = "F";
            this.txtNonyusakiName.MaxByteLengthMode = true;
            this.txtNonyusakiName.Name = "txtNonyusakiName";
            this.txtNonyusakiName.OneLineMaxLength = 60;
            this.txtNonyusakiName.ProhibitionChar = null;
            this.txtNonyusakiName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNonyusakiName_KeyDown);
            // 
            // lblShip
            // 
            // 
            // lblShip.ChildPanel
            // 
            this.lblShip.ChildPanel.Controls.Add(this.txtShip);
            this.lblShip.IsFocusChangeColor = false;
            this.lblShip.LabelWidth = 100;
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
            // lblShukkaFlag
            // 
            // 
            // lblShukkaFlag.ChildPanel
            // 
            this.lblShukkaFlag.ChildPanel.Controls.Add(this.cboShukkaFlag);
            this.lblShukkaFlag.IsFocusChangeColor = false;
            this.lblShukkaFlag.LabelWidth = 60;
            resources.ApplyResources(this.lblShukkaFlag, "lblShukkaFlag");
            this.lblShukkaFlag.Name = "lblShukkaFlag";
            this.lblShukkaFlag.SplitterWidth = 0;
            // 
            // cboShukkaFlag
            // 
            resources.ApplyResources(this.cboShukkaFlag, "cboShukkaFlag");
            this.cboShukkaFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboShukkaFlag.Name = "cboShukkaFlag";
            // 
            // lblKanriFlag
            // 
            // 
            // lblKanriFlag.ChildPanel
            // 
            this.lblKanriFlag.ChildPanel.Controls.Add(this.cboKanriFlag);
            this.lblKanriFlag.IsFocusChangeColor = false;
            this.lblKanriFlag.LabelWidth = 60;
            resources.ApplyResources(this.lblKanriFlag, "lblKanriFlag");
            this.lblKanriFlag.Name = "lblKanriFlag";
            this.lblKanriFlag.SplitterWidth = 0;
            // 
            // cboKanriFlag
            // 
            resources.ApplyResources(this.cboKanriFlag, "cboKanriFlag");
            this.cboKanriFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboKanriFlag.Name = "cboKanriFlag";
            // 
            // NonyusakiIchiranEx
            // 
            resources.ApplyResources(this, "$this");
            this.Name = "NonyusakiIchiranEx";
            this.grpSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.lblNonyusakiName.ChildPanel.ResumeLayout(false);
            this.lblNonyusakiName.ChildPanel.PerformLayout();
            this.lblNonyusakiName.ResumeLayout(false);
            this.lblShip.ChildPanel.ResumeLayout(false);
            this.lblShip.ChildPanel.PerformLayout();
            this.lblShip.ResumeLayout(false);
            this.lblShukkaFlag.ChildPanel.ResumeLayout(false);
            this.lblShukkaFlag.ResumeLayout(false);
            this.lblKanriFlag.ChildPanel.ResumeLayout(false);
            this.lblKanriFlag.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DSWControl.DSWLabel.DSWLabel lblKanriFlag;
        private DSWControl.DSWComboBox.DSWComboBox cboKanriFlag;
        private DSWControl.DSWLabel.DSWLabel lblNonyusakiName;
        private DSWControl.DSWTextBox.DSWTextBox txtNonyusakiName;
        private DSWControl.DSWLabel.DSWLabel lblShip;
        private DSWControl.DSWTextBox.DSWTextBox txtShip;
        private DSWControl.DSWLabel.DSWLabel lblShukkaFlag;
        private DSWControl.DSWComboBox.DSWComboBox cboShukkaFlag;
    }
}
