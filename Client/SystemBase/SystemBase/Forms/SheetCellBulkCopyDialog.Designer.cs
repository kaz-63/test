namespace SystemBase.Forms
{
    partial class SheetCellBulkCopyDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SheetCellBulkCopyDialog));
            this.chkOverwrite = new System.Windows.Forms.CheckBox();
            this.chkToEnd = new System.Windows.Forms.CheckBox();
            this.lblCount = new DSWControl.DSWLabel.DSWLabel();
            this.txtCount = new DSWControl.DSWNumericTextBox();
            this.lblReference = new DSWControl.DSWLabel.DSWLabel();
            this.txtReference = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnSelect = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnClose = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.pnlMain.SuspendLayout();
            this.lblCount.ChildPanel.SuspendLayout();
            this.lblCount.SuspendLayout();
            this.lblReference.ChildPanel.SuspendLayout();
            this.lblReference.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.btnClose);
            this.pnlMain.Controls.Add(this.btnSelect);
            this.pnlMain.Controls.Add(this.lblReference);
            this.pnlMain.Controls.Add(this.lblCount);
            this.pnlMain.Controls.Add(this.chkToEnd);
            this.pnlMain.Controls.Add(this.chkOverwrite);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.SetChildIndex(this.chkOverwrite, 0);
            this.pnlMain.Controls.SetChildIndex(this.chkToEnd, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCount, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblReference, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnSelect, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnClose, 0);
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
            // chkOverwrite
            // 
            resources.ApplyResources(this.chkOverwrite, "chkOverwrite");
            this.chkOverwrite.Name = "chkOverwrite";
            this.chkOverwrite.UseVisualStyleBackColor = true;
            // 
            // chkToEnd
            // 
            resources.ApplyResources(this.chkToEnd, "chkToEnd");
            this.chkToEnd.Name = "chkToEnd";
            this.chkToEnd.UseVisualStyleBackColor = true;
            this.chkToEnd.CheckedChanged += new System.EventHandler(this.chkToEnd_CheckedChanged);
            // 
            // lblCount
            // 
            // 
            // lblCount.ChildPanel
            // 
            this.lblCount.ChildPanel.Controls.Add(this.txtCount);
            this.lblCount.IsFocusChangeColor = false;
            this.lblCount.LabelWidth = 100;
            resources.ApplyResources(this.lblCount, "lblCount");
            this.lblCount.Name = "lblCount";
            this.lblCount.SplitterWidth = 0;
            // 
            // txtCount
            // 
            resources.ApplyResources(this.txtCount, "txtCount");
            this.txtCount.IsThousands = true;
            this.txtCount.Name = "txtCount";
            this.txtCount.ReadOnly = true;
            this.txtCount.TabStop = false;
            this.txtCount.Value = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            // 
            // lblReference
            // 
            // 
            // lblReference.ChildPanel
            // 
            this.lblReference.ChildPanel.Controls.Add(this.txtReference);
            this.lblReference.IsFocusChangeColor = false;
            this.lblReference.LabelWidth = 100;
            resources.ApplyResources(this.lblReference, "lblReference");
            this.lblReference.Name = "lblReference";
            this.lblReference.NormalBackColor = System.Drawing.Color.LightSkyBlue;
            this.lblReference.SplitterWidth = 0;
            // 
            // txtReference
            // 
            resources.ApplyResources(this.txtReference, "txtReference");
            this.txtReference.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtReference.InputRegulation = "F";
            this.txtReference.MaxByteLengthMode = true;
            this.txtReference.Name = "txtReference";
            this.txtReference.OneLineMaxLength = 40;
            this.txtReference.ProhibitionChar = null;
            // 
            // btnSelect
            // 
            resources.ApplyResources(this.btnSelect, "btnSelect");
            this.btnSelect.LinkFunctionKey = DSWControl.DSWFunctionButton.FunctionKeys.F01;
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.UseVisualStyleBackColor = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.LinkFunctionKey = DSWControl.DSWFunctionButton.FunctionKeys.F12;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // SheetCellBulkCopyDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SheetCellBulkCopyDialog";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.lblCount.ChildPanel.ResumeLayout(false);
            this.lblCount.ChildPanel.PerformLayout();
            this.lblCount.ResumeLayout(false);
            this.lblReference.ChildPanel.ResumeLayout(false);
            this.lblReference.ChildPanel.PerformLayout();
            this.lblReference.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkOverwrite;
        private System.Windows.Forms.CheckBox chkToEnd;
        protected DSWControl.DSWLabel.DSWLabel lblCount;
        protected DSWControl.DSWNumericTextBox txtCount;
        private DSWControl.DSWLabel.DSWLabel lblReference;
        private DSWControl.DSWTextBox.DSWTextBox txtReference;
        protected DSWControl.DSWFunctionButton.DSWFunctionButton btnSelect;
        protected DSWControl.DSWFunctionButton.DSWFunctionButton btnClose;
    }
}