namespace SMS.T01.Forms
{
    partial class TehaiMeisaiListImport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TehaiMeisaiListImport));
            this.ChildPanel = new DSWControl.DSWLabel.CustomPanel();
            this.dswTextBox2 = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnDecision = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnClose = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblCellStart = new DSWControl.DSWLabel.DSWLabel();
            this.txtCellStart = new DSWControl.DSWTextBox.DSWTextBox();
            this.txtCellEnd = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnReference = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblExcel = new DSWControl.DSWLabel.DSWLabel();
            this.txtExcel = new DSWControl.DSWTextBox.DSWTextBox();
            this.tblImport = new System.Windows.Forms.TableLayoutPanel();
            this.lblFrom = new System.Windows.Forms.Label();
            this.ofdExcel = new System.Windows.Forms.OpenFileDialog();
            this.pnlMain.SuspendLayout();
            this.lblCellStart.ChildPanel.SuspendLayout();
            this.lblCellStart.SuspendLayout();
            this.lblExcel.ChildPanel.SuspendLayout();
            this.lblExcel.SuspendLayout();
            this.tblImport.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.Add(this.tblImport);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.tblImport, 0);
            // 
            // pnlTitle
            // 
            resources.ApplyResources(this.pnlTitle, "pnlTitle");
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
            // fbrFunction.F03Button
            // 
            resources.ApplyResources(this.fbrFunction.F03Button, "fbrFunction.F03Button");
            // 
            // fbrFunction.F04Button
            // 
            resources.ApplyResources(this.fbrFunction.F04Button, "fbrFunction.F04Button");
            this.fbrFunction.F05toF08Visible = false;
            // 
            // fbrFunction.F12Button
            // 
            resources.ApplyResources(this.fbrFunction.F12Button, "fbrFunction.F12Button");
            // 
            // txtUserName
            // 
            resources.ApplyResources(this.txtUserName, "txtUserName");
            // 
            // txtRoleName
            // 
            resources.ApplyResources(this.txtRoleName, "txtRoleName");
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // dswTextBox2
            // 
            resources.ApplyResources(this.dswTextBox2, "dswTextBox2");
            this.dswTextBox2.FocusBackColor = System.Drawing.SystemColors.Window;
            this.dswTextBox2.InputRegulation = global::SMS.T01.Properties.Resources.TehaiShokai_GWRT;
            this.dswTextBox2.Name = "dswTextBox2";
            this.dswTextBox2.ProhibitionChar = null;
            // 
            // btnDecision
            // 
            this.btnDecision.LinkFunctionKey = DSWControl.DSWFunctionButton.FunctionKeys.F01;
            resources.ApplyResources(this.btnDecision, "btnDecision");
            this.btnDecision.Name = "btnDecision";
            this.btnDecision.Click += new System.EventHandler(this.fbrFunction_F01Button_Click);
            // 
            // btnClose
            // 
            this.btnClose.LinkFunctionKey = DSWControl.DSWFunctionButton.FunctionKeys.F12;
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.Click += new System.EventHandler(this.fbrFunction_F12Button_Click);
            // 
            // lblCellStart
            // 
            // 
            // lblCellStart.ChildPanel
            // 
            this.lblCellStart.ChildPanel.Controls.Add(this.txtCellStart);
            this.lblCellStart.IsFocusChangeColor = false;
            this.lblCellStart.IsNecessary = true;
            this.lblCellStart.LabelWidth = 110;
            resources.ApplyResources(this.lblCellStart, "lblCellStart");
            this.lblCellStart.Name = "lblCellStart";
            this.lblCellStart.SplitterWidth = 0;
            // 
            // txtCellStart
            // 
            resources.ApplyResources(this.txtCellStart, "txtCellStart");
            this.txtCellStart.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtCellStart.InputRegulation = "abn";
            this.txtCellStart.IsInputRegulation = true;
            this.txtCellStart.MaxByteLengthMode = true;
            this.txtCellStart.Name = "txtCellStart";
            this.txtCellStart.OneLineMaxLength = 10;
            this.txtCellStart.ProhibitionChar = null;
            // 
            // txtCellEnd
            // 
            this.txtCellEnd.FocusBackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.txtCellEnd, "txtCellEnd");
            this.txtCellEnd.InputRegulation = "abn";
            this.txtCellEnd.IsInputRegulation = true;
            this.txtCellEnd.MaxByteLengthMode = true;
            this.txtCellEnd.Name = "txtCellEnd";
            this.txtCellEnd.OneLineMaxLength = 10;
            this.txtCellEnd.ProhibitionChar = null;
            // 
            // btnReference
            // 
            resources.ApplyResources(this.btnReference, "btnReference");
            this.btnReference.Name = "btnReference";
            this.btnReference.TabStop = false;
            this.btnReference.Click += new System.EventHandler(this.btnReference_Click);
            // 
            // lblExcel
            // 
            // 
            // lblExcel.ChildPanel
            // 
            this.lblExcel.ChildPanel.Controls.Add(this.txtExcel);
            this.tblImport.SetColumnSpan(this.lblExcel, 3);
            this.lblExcel.IsFocusChangeColor = false;
            this.lblExcel.IsNecessary = true;
            this.lblExcel.LabelWidth = 110;
            resources.ApplyResources(this.lblExcel, "lblExcel");
            this.lblExcel.Name = "lblExcel";
            this.lblExcel.SplitterWidth = 0;
            this.lblExcel.TabStop = false;
            // 
            // txtExcel
            // 
            this.txtExcel.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtExcel, "txtExcel");
            this.txtExcel.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtExcel.InputRegulation = global::SMS.T01.Properties.Resources.TehaiShokai_GWRT;
            this.txtExcel.Name = "txtExcel";
            this.txtExcel.ProhibitionChar = null;
            this.txtExcel.ReadOnly = true;
            this.txtExcel.TabStop = false;
            // 
            // tblImport
            // 
            resources.ApplyResources(this.tblImport, "tblImport");
            this.tblImport.Controls.Add(this.btnDecision, 0, 2);
            this.tblImport.Controls.Add(this.btnReference, 3, 0);
            this.tblImport.Controls.Add(this.btnClose, 3, 2);
            this.tblImport.Controls.Add(this.lblExcel, 0, 0);
            this.tblImport.Controls.Add(this.txtCellEnd, 2, 1);
            this.tblImport.Controls.Add(this.lblFrom, 1, 1);
            this.tblImport.Controls.Add(this.lblCellStart, 0, 1);
            this.tblImport.Name = "tblImport";
            // 
            // lblFrom
            // 
            resources.ApplyResources(this.lblFrom, "lblFrom");
            this.lblFrom.Name = "lblFrom";
            // 
            // ofdExcel
            // 
            this.ofdExcel.FileName = "ofdExcel";
            resources.ApplyResources(this.ofdExcel, "ofdExcel");
            this.ofdExcel.InitialDirectory = "%DESKTOP%";
            // 
            // TehaiMeisaiListImport
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.IsRunEditAfterClear = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TehaiMeisaiListImport";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.lblCellStart.ChildPanel.ResumeLayout(false);
            this.lblCellStart.ChildPanel.PerformLayout();
            this.lblCellStart.ResumeLayout(false);
            this.lblExcel.ChildPanel.ResumeLayout(false);
            this.lblExcel.ChildPanel.PerformLayout();
            this.lblExcel.ResumeLayout(false);
            this.tblImport.ResumeLayout(false);
            this.tblImport.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DSWControl.DSWTextBox.DSWTextBox dswTextBox2;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnDecision;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnClose;
        private DSWControl.DSWTextBox.DSWTextBox txtCellEnd;
        private DSWControl.DSWLabel.DSWLabel lblCellStart;
        private DSWControl.DSWTextBox.DSWTextBox txtCellStart;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnReference;
        private DSWControl.DSWLabel.DSWLabel lblExcel;
        private DSWControl.DSWTextBox.DSWTextBox txtExcel;
        private System.Windows.Forms.TableLayoutPanel tblImport;
        private System.Windows.Forms.Label lblFrom;
        private DSWControl.DSWLabel.CustomPanel ChildPanel;
        private System.Windows.Forms.OpenFileDialog ofdExcel;

    }
}