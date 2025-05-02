namespace SMS.S01.Forms
{
    partial class BinkanIdo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BinkanIdo));
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.grpDest = new System.Windows.Forms.GroupBox();
            this.btnStartDest = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblNonyusakiNameDest = new DSWControl.DSWLabel.DSWLabel();
            this.txtNonyusakiNameDest = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblNonyusakiCDDest = new DSWControl.DSWLabel.DSWLabel();
            this.txtNonyusakiCDDest = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblShipDest = new DSWControl.DSWLabel.DSWLabel();
            this.txtShipDest = new DSWControl.DSWTextBox.DSWTextBox();
            this.grpOrig = new System.Windows.Forms.GroupBox();
            this.btnStartOrig = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblNonyusakiNameOrig = new DSWControl.DSWLabel.DSWLabel();
            this.txtNonyusakiNameOrig = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblNonyusakiCDOrig = new DSWControl.DSWLabel.DSWLabel();
            this.txtNonyusakiCDOrig = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblShipOrig = new DSWControl.DSWLabel.DSWLabel();
            this.txtShipOrig = new DSWControl.DSWTextBox.DSWTextBox();
            this.grDetailOrig = new System.Windows.Forms.GroupBox();
            this.tvOrig = new System.Windows.Forms.TreeView();
            this.grpDetailDest = new System.Windows.Forms.GroupBox();
            this.tvDest = new System.Windows.Forms.TreeView();
            this.pnlMain.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.grpDest.SuspendLayout();
            this.lblNonyusakiNameDest.ChildPanel.SuspendLayout();
            this.lblNonyusakiNameDest.SuspendLayout();
            this.lblNonyusakiCDDest.ChildPanel.SuspendLayout();
            this.lblNonyusakiCDDest.SuspendLayout();
            this.lblShipDest.ChildPanel.SuspendLayout();
            this.lblShipDest.SuspendLayout();
            this.grpOrig.SuspendLayout();
            this.lblNonyusakiNameOrig.ChildPanel.SuspendLayout();
            this.lblNonyusakiNameOrig.SuspendLayout();
            this.lblNonyusakiCDOrig.ChildPanel.SuspendLayout();
            this.lblNonyusakiCDOrig.SuspendLayout();
            this.lblShipOrig.ChildPanel.SuspendLayout();
            this.lblShipOrig.SuspendLayout();
            this.grDetailOrig.SuspendLayout();
            this.grpDetailDest.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.grpDetailDest);
            this.pnlMain.Controls.Add(this.grpSearch);
            this.pnlMain.Controls.Add(this.grDetailOrig);
            this.pnlMain.Controls.SetChildIndex(this.grDetailOrig, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpDetailDest, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            // 
            // fbrFunction
            // 
            // 
            // fbrFunction.F06Button
            // 
            resources.ApplyResources(this.fbrFunction.F06Button, "fbrFunction.F06Button");
            resources.ApplyResources(this.fbrFunction, "fbrFunction");
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
            this.grpSearch.Controls.Add(this.grpDest);
            this.grpSearch.Controls.Add(this.grpOrig);
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
            // 
            // grpDest
            // 
            this.grpDest.Controls.Add(this.btnStartDest);
            this.grpDest.Controls.Add(this.lblNonyusakiNameDest);
            this.grpDest.Controls.Add(this.lblNonyusakiCDDest);
            this.grpDest.Controls.Add(this.lblShipDest);
            resources.ApplyResources(this.grpDest, "grpDest");
            this.grpDest.Name = "grpDest";
            this.grpDest.TabStop = false;
            // 
            // btnStartDest
            // 
            resources.ApplyResources(this.btnStartDest, "btnStartDest");
            this.btnStartDest.Name = "btnStartDest";
            this.btnStartDest.Click += new System.EventHandler(this.btnStartDest_Click);
            // 
            // lblNonyusakiNameDest
            // 
            // 
            // lblNonyusakiNameDest.ChildPanel
            // 
            this.lblNonyusakiNameDest.ChildPanel.Controls.Add(this.txtNonyusakiNameDest);
            this.lblNonyusakiNameDest.IsFocusChangeColor = false;
            this.lblNonyusakiNameDest.IsNecessary = true;
            this.lblNonyusakiNameDest.LabelWidth = 80;
            resources.ApplyResources(this.lblNonyusakiNameDest, "lblNonyusakiNameDest");
            this.lblNonyusakiNameDest.Name = "lblNonyusakiNameDest";
            this.lblNonyusakiNameDest.SplitterWidth = 0;
            // 
            // txtNonyusakiNameDest
            // 
            resources.ApplyResources(this.txtNonyusakiNameDest, "txtNonyusakiNameDest");
            this.txtNonyusakiNameDest.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtNonyusakiNameDest.InputRegulation = "F";
            this.txtNonyusakiNameDest.MaxByteLengthMode = true;
            this.txtNonyusakiNameDest.Name = "txtNonyusakiNameDest";
            this.txtNonyusakiNameDest.OneLineMaxLength = 60;
            this.txtNonyusakiNameDest.ProhibitionChar = null;
            // 
            // lblNonyusakiCDDest
            // 
            // 
            // lblNonyusakiCDDest.ChildPanel
            // 
            this.lblNonyusakiCDDest.ChildPanel.Controls.Add(this.txtNonyusakiCDDest);
            this.lblNonyusakiCDDest.IsFocusChangeColor = false;
            this.lblNonyusakiCDDest.LabelWidth = 100;
            resources.ApplyResources(this.lblNonyusakiCDDest, "lblNonyusakiCDDest");
            this.lblNonyusakiCDDest.Name = "lblNonyusakiCDDest";
            this.lblNonyusakiCDDest.SplitterWidth = 0;
            this.lblNonyusakiCDDest.TabStop = false;
            // 
            // txtNonyusakiCDDest
            // 
            this.txtNonyusakiCDDest.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtNonyusakiCDDest, "txtNonyusakiCDDest");
            this.txtNonyusakiCDDest.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtNonyusakiCDDest.InputRegulation = "";
            this.txtNonyusakiCDDest.Name = "txtNonyusakiCDDest";
            this.txtNonyusakiCDDest.ProhibitionChar = null;
            this.txtNonyusakiCDDest.ReadOnly = true;
            this.txtNonyusakiCDDest.TabStop = false;
            // 
            // lblShipDest
            // 
            // 
            // lblShipDest.ChildPanel
            // 
            this.lblShipDest.ChildPanel.Controls.Add(this.txtShipDest);
            this.lblShipDest.IsFocusChangeColor = false;
            this.lblShipDest.IsNecessary = true;
            this.lblShipDest.LabelWidth = 80;
            resources.ApplyResources(this.lblShipDest, "lblShipDest");
            this.lblShipDest.Name = "lblShipDest";
            this.lblShipDest.SplitterWidth = 0;
            // 
            // txtShipDest
            // 
            resources.ApplyResources(this.txtShipDest, "txtShipDest");
            this.txtShipDest.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtShipDest.InputRegulation = "F";
            this.txtShipDest.MaxByteLengthMode = true;
            this.txtShipDest.Name = "txtShipDest";
            this.txtShipDest.OneLineMaxLength = 10;
            this.txtShipDest.ProhibitionChar = null;
            // 
            // grpOrig
            // 
            this.grpOrig.Controls.Add(this.btnStartOrig);
            this.grpOrig.Controls.Add(this.lblNonyusakiNameOrig);
            this.grpOrig.Controls.Add(this.lblNonyusakiCDOrig);
            this.grpOrig.Controls.Add(this.lblShipOrig);
            resources.ApplyResources(this.grpOrig, "grpOrig");
            this.grpOrig.Name = "grpOrig";
            this.grpOrig.TabStop = false;
            // 
            // btnStartOrig
            // 
            resources.ApplyResources(this.btnStartOrig, "btnStartOrig");
            this.btnStartOrig.Name = "btnStartOrig";
            this.btnStartOrig.Click += new System.EventHandler(this.btnStartOrig_Click);
            // 
            // lblNonyusakiNameOrig
            // 
            // 
            // lblNonyusakiNameOrig.ChildPanel
            // 
            this.lblNonyusakiNameOrig.ChildPanel.Controls.Add(this.txtNonyusakiNameOrig);
            this.lblNonyusakiNameOrig.IsFocusChangeColor = false;
            this.lblNonyusakiNameOrig.IsNecessary = true;
            this.lblNonyusakiNameOrig.LabelWidth = 80;
            resources.ApplyResources(this.lblNonyusakiNameOrig, "lblNonyusakiNameOrig");
            this.lblNonyusakiNameOrig.Name = "lblNonyusakiNameOrig";
            this.lblNonyusakiNameOrig.SplitterWidth = 0;
            // 
            // txtNonyusakiNameOrig
            // 
            resources.ApplyResources(this.txtNonyusakiNameOrig, "txtNonyusakiNameOrig");
            this.txtNonyusakiNameOrig.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtNonyusakiNameOrig.InputRegulation = "F";
            this.txtNonyusakiNameOrig.MaxByteLengthMode = true;
            this.txtNonyusakiNameOrig.Name = "txtNonyusakiNameOrig";
            this.txtNonyusakiNameOrig.OneLineMaxLength = 60;
            this.txtNonyusakiNameOrig.ProhibitionChar = null;
            // 
            // lblNonyusakiCDOrig
            // 
            // 
            // lblNonyusakiCDOrig.ChildPanel
            // 
            this.lblNonyusakiCDOrig.ChildPanel.Controls.Add(this.txtNonyusakiCDOrig);
            this.lblNonyusakiCDOrig.IsFocusChangeColor = false;
            this.lblNonyusakiCDOrig.LabelWidth = 100;
            resources.ApplyResources(this.lblNonyusakiCDOrig, "lblNonyusakiCDOrig");
            this.lblNonyusakiCDOrig.Name = "lblNonyusakiCDOrig";
            this.lblNonyusakiCDOrig.SplitterWidth = 0;
            this.lblNonyusakiCDOrig.TabStop = false;
            // 
            // txtNonyusakiCDOrig
            // 
            this.txtNonyusakiCDOrig.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtNonyusakiCDOrig, "txtNonyusakiCDOrig");
            this.txtNonyusakiCDOrig.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtNonyusakiCDOrig.InputRegulation = "";
            this.txtNonyusakiCDOrig.Name = "txtNonyusakiCDOrig";
            this.txtNonyusakiCDOrig.ProhibitionChar = null;
            this.txtNonyusakiCDOrig.ReadOnly = true;
            this.txtNonyusakiCDOrig.TabStop = false;
            // 
            // lblShipOrig
            // 
            // 
            // lblShipOrig.ChildPanel
            // 
            this.lblShipOrig.ChildPanel.Controls.Add(this.txtShipOrig);
            this.lblShipOrig.IsFocusChangeColor = false;
            this.lblShipOrig.IsNecessary = true;
            this.lblShipOrig.LabelWidth = 80;
            resources.ApplyResources(this.lblShipOrig, "lblShipOrig");
            this.lblShipOrig.Name = "lblShipOrig";
            this.lblShipOrig.SplitterWidth = 0;
            // 
            // txtShipOrig
            // 
            resources.ApplyResources(this.txtShipOrig, "txtShipOrig");
            this.txtShipOrig.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtShipOrig.InputRegulation = "F";
            this.txtShipOrig.MaxByteLengthMode = true;
            this.txtShipOrig.Name = "txtShipOrig";
            this.txtShipOrig.OneLineMaxLength = 10;
            this.txtShipOrig.ProhibitionChar = null;
            // 
            // grDetailOrig
            // 
            this.grDetailOrig.Controls.Add(this.tvOrig);
            resources.ApplyResources(this.grDetailOrig, "grDetailOrig");
            this.grDetailOrig.Name = "grDetailOrig";
            this.grDetailOrig.TabStop = false;
            // 
            // tvOrig
            // 
            this.tvOrig.AllowDrop = true;
            resources.ApplyResources(this.tvOrig, "tvOrig");
            this.tvOrig.Name = "tvOrig";
            this.tvOrig.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvOrig_ItemDrag);
            this.tvOrig.DragOver += new System.Windows.Forms.DragEventHandler(this.tvOrig_DragOver);
            // 
            // grpDetailDest
            // 
            this.grpDetailDest.Controls.Add(this.tvDest);
            resources.ApplyResources(this.grpDetailDest, "grpDetailDest");
            this.grpDetailDest.Name = "grpDetailDest";
            this.grpDetailDest.TabStop = false;
            // 
            // tvDest
            // 
            this.tvDest.AllowDrop = true;
            resources.ApplyResources(this.tvDest, "tvDest");
            this.tvDest.Name = "tvDest";
            this.tvDest.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvDest_DragDrop);
            this.tvDest.DragOver += new System.Windows.Forms.DragEventHandler(this.tvDest_DragOver);
            // 
            // BinkanIdo
            // 
            resources.ApplyResources(this, "$this");
            this.Name = "BinkanIdo";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.grpSearch.ResumeLayout(false);
            this.grpDest.ResumeLayout(false);
            this.lblNonyusakiNameDest.ChildPanel.ResumeLayout(false);
            this.lblNonyusakiNameDest.ChildPanel.PerformLayout();
            this.lblNonyusakiNameDest.ResumeLayout(false);
            this.lblNonyusakiCDDest.ChildPanel.ResumeLayout(false);
            this.lblNonyusakiCDDest.ChildPanel.PerformLayout();
            this.lblNonyusakiCDDest.ResumeLayout(false);
            this.lblShipDest.ChildPanel.ResumeLayout(false);
            this.lblShipDest.ChildPanel.PerformLayout();
            this.lblShipDest.ResumeLayout(false);
            this.grpOrig.ResumeLayout(false);
            this.lblNonyusakiNameOrig.ChildPanel.ResumeLayout(false);
            this.lblNonyusakiNameOrig.ChildPanel.PerformLayout();
            this.lblNonyusakiNameOrig.ResumeLayout(false);
            this.lblNonyusakiCDOrig.ChildPanel.ResumeLayout(false);
            this.lblNonyusakiCDOrig.ChildPanel.PerformLayout();
            this.lblNonyusakiCDOrig.ResumeLayout(false);
            this.lblShipOrig.ChildPanel.ResumeLayout(false);
            this.lblShipOrig.ChildPanel.PerformLayout();
            this.lblShipOrig.ResumeLayout(false);
            this.grDetailOrig.ResumeLayout(false);
            this.grpDetailDest.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSearch;
        private System.Windows.Forms.GroupBox grpDest;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnStartDest;
        private DSWControl.DSWLabel.DSWLabel lblNonyusakiNameDest;
        private DSWControl.DSWTextBox.DSWTextBox txtNonyusakiNameDest;
        private DSWControl.DSWLabel.DSWLabel lblNonyusakiCDDest;
        private DSWControl.DSWTextBox.DSWTextBox txtNonyusakiCDDest;
        private DSWControl.DSWLabel.DSWLabel lblShipDest;
        private DSWControl.DSWTextBox.DSWTextBox txtShipDest;
        private System.Windows.Forms.GroupBox grpOrig;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnStartOrig;
        private DSWControl.DSWLabel.DSWLabel lblNonyusakiNameOrig;
        private DSWControl.DSWTextBox.DSWTextBox txtNonyusakiNameOrig;
        private DSWControl.DSWLabel.DSWLabel lblNonyusakiCDOrig;
        private DSWControl.DSWTextBox.DSWTextBox txtNonyusakiCDOrig;
        private DSWControl.DSWLabel.DSWLabel lblShipOrig;
        private DSWControl.DSWTextBox.DSWTextBox txtShipOrig;
        private System.Windows.Forms.GroupBox grDetailOrig;
        private System.Windows.Forms.GroupBox grpDetailDest;
        private System.Windows.Forms.TreeView tvDest;
        private System.Windows.Forms.TreeView tvOrig;
    }
}
