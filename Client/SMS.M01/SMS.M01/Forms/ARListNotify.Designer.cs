namespace SMS.M01.Forms
{
    partial class ARListNotify
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ARListNotify));
            this.grpEvent = new System.Windows.Forms.GroupBox();
            this.btnSearch = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblBukkenName = new DSWControl.DSWLabel.DSWLabel();
            this.txtBukkenName = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblBukkenNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtBukkenNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblListFlag = new DSWControl.DSWLabel.DSWLabel();
            this.cboListFlag = new DSWControl.DSWComboBox.DSWComboBox();
            this.grpEdit = new System.Windows.Forms.GroupBox();
            this.btnBcc = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnCc = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnTo = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblBcc = new DSWControl.DSWLabel.DSWLabel();
            this.txtBcc = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblCc = new DSWControl.DSWLabel.DSWLabel();
            this.txtCc = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblTo = new DSWControl.DSWLabel.DSWLabel();
            this.txtTo = new DSWControl.DSWTextBox.DSWTextBox();
            this.pnlMain.SuspendLayout();
            this.grpEvent.SuspendLayout();
            this.lblBukkenName.ChildPanel.SuspendLayout();
            this.lblBukkenName.SuspendLayout();
            this.lblBukkenNo.ChildPanel.SuspendLayout();
            this.lblBukkenNo.SuspendLayout();
            this.lblListFlag.ChildPanel.SuspendLayout();
            this.lblListFlag.SuspendLayout();
            this.grpEdit.SuspendLayout();
            this.lblBcc.ChildPanel.SuspendLayout();
            this.lblBcc.SuspendLayout();
            this.lblCc.ChildPanel.SuspendLayout();
            this.lblCc.SuspendLayout();
            this.lblTo.ChildPanel.SuspendLayout();
            this.lblTo.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.grpEdit);
            this.pnlMain.Controls.Add(this.grpEvent);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpEvent, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpEdit, 0);
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
            // fbrFunction.F07Button
            // 
            resources.ApplyResources(this.fbrFunction.F07Button, "fbrFunction.F07Button");
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // grpEvent
            // 
            resources.ApplyResources(this.grpEvent, "grpEvent");
            this.grpEvent.Controls.Add(this.btnSearch);
            this.grpEvent.Controls.Add(this.lblBukkenName);
            this.grpEvent.Controls.Add(this.lblBukkenNo);
            this.grpEvent.Controls.Add(this.lblListFlag);
            this.grpEvent.Name = "grpEvent";
            this.grpEvent.TabStop = false;
            // 
            // btnSearch
            // 
            resources.ApplyResources(this.btnSearch, "btnSearch");
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblBukkenName
            // 
            // 
            // lblBukkenName.ChildPanel
            // 
            this.lblBukkenName.ChildPanel.Controls.Add(this.txtBukkenName);
            this.lblBukkenName.IsFocusChangeColor = false;
            this.lblBukkenName.LabelWidth = 100;
            resources.ApplyResources(this.lblBukkenName, "lblBukkenName");
            this.lblBukkenName.Name = "lblBukkenName";
            this.lblBukkenName.NormalBackColor = System.Drawing.Color.LightSkyBlue;
            this.lblBukkenName.SplitterWidth = 0;
            // 
            // txtBukkenName
            // 
            this.txtBukkenName.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtBukkenName, "txtBukkenName");
            this.txtBukkenName.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtBukkenName.InputRegulation = "F";
            this.txtBukkenName.MaxByteLengthMode = true;
            this.txtBukkenName.Name = "txtBukkenName";
            this.txtBukkenName.OneLineMaxLength = 60;
            this.txtBukkenName.ProhibitionChar = null;
            this.txtBukkenName.ReadOnly = true;
            // 
            // lblBukkenNo
            // 
            // 
            // lblBukkenNo.ChildPanel
            // 
            this.lblBukkenNo.ChildPanel.Controls.Add(this.txtBukkenNo);
            this.lblBukkenNo.IsFocusChangeColor = false;
            this.lblBukkenNo.LabelWidth = 100;
            resources.ApplyResources(this.lblBukkenNo, "lblBukkenNo");
            this.lblBukkenNo.Name = "lblBukkenNo";
            this.lblBukkenNo.NormalBackColor = System.Drawing.Color.LightSkyBlue;
            this.lblBukkenNo.SplitterWidth = 0;
            // 
            // txtBukkenNo
            // 
            this.txtBukkenNo.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtBukkenNo, "txtBukkenNo");
            this.txtBukkenNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtBukkenNo.InputRegulation = "F";
            this.txtBukkenNo.MaxByteLengthMode = true;
            this.txtBukkenNo.Name = "txtBukkenNo";
            this.txtBukkenNo.OneLineMaxLength = 40;
            this.txtBukkenNo.ProhibitionChar = null;
            this.txtBukkenNo.ReadOnly = true;
            // 
            // lblListFlag
            // 
            // 
            // lblListFlag.ChildPanel
            // 
            this.lblListFlag.ChildPanel.Controls.Add(this.cboListFlag);
            this.lblListFlag.IsFocusChangeColor = false;
            this.lblListFlag.IsNecessary = true;
            this.lblListFlag.LabelWidth = 100;
            resources.ApplyResources(this.lblListFlag, "lblListFlag");
            this.lblListFlag.Name = "lblListFlag";
            this.lblListFlag.SplitterWidth = 0;
            // 
            // cboListFlag
            // 
            resources.ApplyResources(this.cboListFlag, "cboListFlag");
            this.cboListFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboListFlag.Name = "cboListFlag";
            // 
            // grpEdit
            // 
            resources.ApplyResources(this.grpEdit, "grpEdit");
            this.grpEdit.Controls.Add(this.btnBcc);
            this.grpEdit.Controls.Add(this.btnCc);
            this.grpEdit.Controls.Add(this.btnTo);
            this.grpEdit.Controls.Add(this.lblBcc);
            this.grpEdit.Controls.Add(this.lblCc);
            this.grpEdit.Controls.Add(this.lblTo);
            this.grpEdit.Name = "grpEdit";
            this.grpEdit.TabStop = false;
            // 
            // btnBcc
            // 
            resources.ApplyResources(this.btnBcc, "btnBcc");
            this.btnBcc.Name = "btnBcc";
            this.btnBcc.Click += new System.EventHandler(this.btnBcc_Click);
            // 
            // btnCc
            // 
            resources.ApplyResources(this.btnCc, "btnCc");
            this.btnCc.Name = "btnCc";
            this.btnCc.Click += new System.EventHandler(this.btnCc_Click);
            // 
            // btnTo
            // 
            resources.ApplyResources(this.btnTo, "btnTo");
            this.btnTo.Name = "btnTo";
            this.btnTo.Click += new System.EventHandler(this.btnTo_Click);
            // 
            // lblBcc
            // 
            // 
            // lblBcc.ChildPanel
            // 
            this.lblBcc.ChildPanel.Controls.Add(this.txtBcc);
            this.lblBcc.IsFocusChangeColor = false;
            this.lblBcc.LabelWidth = 100;
            resources.ApplyResources(this.lblBcc, "lblBcc");
            this.lblBcc.Name = "lblBcc";
            this.lblBcc.SplitterWidth = 0;
            // 
            // txtBcc
            // 
            this.txtBcc.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtBcc, "txtBcc");
            this.txtBcc.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtBcc.InputRegulation = "F";
            this.txtBcc.MaxByteLengthMode = true;
            this.txtBcc.Name = "txtBcc";
            this.txtBcc.OneLineMaxLength = 40;
            this.txtBcc.ProhibitionChar = null;
            this.txtBcc.ReadOnly = true;
            // 
            // lblCc
            // 
            // 
            // lblCc.ChildPanel
            // 
            this.lblCc.ChildPanel.Controls.Add(this.txtCc);
            this.lblCc.IsFocusChangeColor = false;
            this.lblCc.LabelWidth = 100;
            resources.ApplyResources(this.lblCc, "lblCc");
            this.lblCc.Name = "lblCc";
            this.lblCc.SplitterWidth = 0;
            // 
            // txtCc
            // 
            this.txtCc.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtCc, "txtCc");
            this.txtCc.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtCc.InputRegulation = "F";
            this.txtCc.MaxByteLengthMode = true;
            this.txtCc.Name = "txtCc";
            this.txtCc.OneLineMaxLength = 40;
            this.txtCc.ProhibitionChar = null;
            this.txtCc.ReadOnly = true;
            // 
            // lblTo
            // 
            // 
            // lblTo.ChildPanel
            // 
            this.lblTo.ChildPanel.Controls.Add(this.txtTo);
            this.lblTo.IsFocusChangeColor = false;
            this.lblTo.IsNecessary = true;
            this.lblTo.LabelWidth = 100;
            resources.ApplyResources(this.lblTo, "lblTo");
            this.lblTo.Name = "lblTo";
            this.lblTo.SplitterWidth = 0;
            // 
            // txtTo
            // 
            this.txtTo.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtTo, "txtTo");
            this.txtTo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtTo.InputRegulation = "F";
            this.txtTo.MaxByteLengthMode = true;
            this.txtTo.Name = "txtTo";
            this.txtTo.OneLineMaxLength = 40;
            this.txtTo.ProhibitionChar = null;
            this.txtTo.ReadOnly = true;
            // 
            // ARListNotify
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ARListNotify";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.grpEvent.ResumeLayout(false);
            this.lblBukkenName.ChildPanel.ResumeLayout(false);
            this.lblBukkenName.ChildPanel.PerformLayout();
            this.lblBukkenName.ResumeLayout(false);
            this.lblBukkenNo.ChildPanel.ResumeLayout(false);
            this.lblBukkenNo.ChildPanel.PerformLayout();
            this.lblBukkenNo.ResumeLayout(false);
            this.lblListFlag.ChildPanel.ResumeLayout(false);
            this.lblListFlag.ResumeLayout(false);
            this.grpEdit.ResumeLayout(false);
            this.lblBcc.ChildPanel.ResumeLayout(false);
            this.lblBcc.ChildPanel.PerformLayout();
            this.lblBcc.ResumeLayout(false);
            this.lblCc.ChildPanel.ResumeLayout(false);
            this.lblCc.ChildPanel.PerformLayout();
            this.lblCc.ResumeLayout(false);
            this.lblTo.ChildPanel.ResumeLayout(false);
            this.lblTo.ChildPanel.PerformLayout();
            this.lblTo.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpEvent;
        private System.Windows.Forms.GroupBox grpEdit;
        private DSWControl.DSWLabel.DSWLabel lblBcc;
        private DSWControl.DSWTextBox.DSWTextBox txtBcc;
        private DSWControl.DSWLabel.DSWLabel lblCc;
        private DSWControl.DSWTextBox.DSWTextBox txtCc;
        private DSWControl.DSWLabel.DSWLabel lblTo;
        private DSWControl.DSWTextBox.DSWTextBox txtTo;
        private DSWControl.DSWLabel.DSWLabel lblListFlag;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnBcc;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnCc;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnTo;
        private DSWControl.DSWLabel.DSWLabel lblBukkenName;
        private DSWControl.DSWTextBox.DSWTextBox txtBukkenName;
        private DSWControl.DSWLabel.DSWLabel lblBukkenNo;
        private DSWControl.DSWTextBox.DSWTextBox txtBukkenNo;
        private DSWControl.DSWComboBox.DSWComboBox cboListFlag;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnSearch;
    }
}