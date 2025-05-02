namespace SMS.S01.Forms
{
    partial class ShukkaKeikakuBunkatsu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShukkaKeikakuBunkatsu));
            this.ChildPanel = new DSWControl.DSWLabel.CustomPanel();
            this.txtNumSum = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblNumSum = new DSWControl.DSWLabel.DSWLabel();
            this.dswTextBox2 = new DSWControl.DSWTextBox.DSWTextBox();
            this.txtNumSource = new DSWControl.DSWTextBox.DSWTextBox();
            this.txtNumDestination = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnDecision = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnClose = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblSum = new System.Windows.Forms.Label();
            this.lblSource = new System.Windows.Forms.Label();
            this.lblDestination = new System.Windows.Forms.Label();
            this.pnlMain.SuspendLayout();
            this.lblNumSum.ChildPanel.SuspendLayout();
            this.lblNumSum.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.Add(this.lblDestination);
            this.pnlMain.Controls.Add(this.lblSource);
            this.pnlMain.Controls.Add(this.lblSum);
            this.pnlMain.Controls.Add(this.btnClose);
            this.pnlMain.Controls.Add(this.btnDecision);
            this.pnlMain.Controls.Add(this.txtNumDestination);
            this.pnlMain.Controls.Add(this.txtNumSource);
            this.pnlMain.Controls.Add(this.lblNumSum);
            this.pnlMain.Controls.SetChildIndex(this.lblNumSum, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.txtNumSource, 0);
            this.pnlMain.Controls.SetChildIndex(this.txtNumDestination, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnDecision, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnClose, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblSum, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblSource, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblDestination, 0);
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
            // txtNumSum
            // 
            resources.ApplyResources(this.txtNumSum, "txtNumSum");
            this.txtNumSum.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtNumSum.InputRegulation = "n";
            this.txtNumSum.IsInputRegulation = true;
            this.txtNumSum.MaxByteLengthMode = true;
            this.txtNumSum.Name = "txtNumSum";
            this.txtNumSum.OneLineMaxLength = 6;
            this.txtNumSum.PadType = DSWControl.DSWTextBox.DSWTextBox.RightLeftType.Right;
            this.txtNumSum.ProhibitionChar = null;
            this.txtNumSum.ReadOnly = true;
            // 
            // lblNumSum
            // 
            // 
            // lblNumSum.ChildPanel
            // 
            this.lblNumSum.ChildPanel.Controls.Add(this.txtNumSum);
            this.lblNumSum.IsFocusChangeColor = false;
            this.lblNumSum.IsNecessary = true;
            this.lblNumSum.LabelWidth = 60;
            resources.ApplyResources(this.lblNumSum, "lblNumSum");
            this.lblNumSum.Name = "lblNumSum";
            this.lblNumSum.SplitterWidth = 0;
            // 
            // dswTextBox2
            // 
            resources.ApplyResources(this.dswTextBox2, "dswTextBox2");
            this.dswTextBox2.FocusBackColor = System.Drawing.SystemColors.Window;
            this.dswTextBox2.InputRegulation = "n";
            this.dswTextBox2.Name = "dswTextBox2";
            this.dswTextBox2.ProhibitionChar = null;
            // 
            // txtNumSource
            // 
            this.txtNumSource.FocusBackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.txtNumSource, "txtNumSource");
            this.txtNumSource.InputRegulation = "n";
            this.txtNumSource.IsInputRegulation = true;
            this.txtNumSource.MaxByteLengthMode = true;
            this.txtNumSource.Name = "txtNumSource";
            this.txtNumSource.OneLineMaxLength = 6;
            this.txtNumSource.PadType = DSWControl.DSWTextBox.DSWTextBox.RightLeftType.Right;
            this.txtNumSource.ProhibitionChar = null;
            this.txtNumSource.ValueChanged += new System.EventHandler(this.txtNumSource_ValueChanged);
            this.txtNumSource.Leave += new System.EventHandler(this.txtNumSource_Leave);
            this.txtNumSource.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtNumSource_KeyUp);
            // 
            // txtNumDestination
            // 
            this.txtNumDestination.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtNumDestination.InputRegulation = "n";
            resources.ApplyResources(this.txtNumDestination, "txtNumDestination");
            this.txtNumDestination.MaxByteLengthMode = true;
            this.txtNumDestination.Name = "txtNumDestination";
            this.txtNumDestination.OneLineMaxLength = 6;
            this.txtNumDestination.PadType = DSWControl.DSWTextBox.DSWTextBox.RightLeftType.Right;
            this.txtNumDestination.ProhibitionChar = null;
            this.txtNumDestination.ReadOnly = true;
            // 
            // btnDecision
            // 
            this.btnDecision.LinkFunctionKey = DSWControl.DSWFunctionButton.FunctionKeys.F01;
            resources.ApplyResources(this.btnDecision, "btnDecision");
            this.btnDecision.Name = "btnDecision";
            this.btnDecision.Click += new System.EventHandler(this.btnDecision_Click);
            // 
            // btnClose
            // 
            this.btnClose.LinkFunctionKey = DSWControl.DSWFunctionButton.FunctionKeys.F12;
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblSum
            // 
            this.lblSum.BackColor = System.Drawing.Color.SkyBlue;
            this.lblSum.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lblSum, "lblSum");
            this.lblSum.Name = "lblSum";
            // 
            // lblSource
            // 
            this.lblSource.BackColor = System.Drawing.Color.SkyBlue;
            this.lblSource.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lblSource, "lblSource");
            this.lblSource.Name = "lblSource";
            // 
            // lblDestination
            // 
            this.lblDestination.BackColor = System.Drawing.Color.SkyBlue;
            this.lblDestination.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lblDestination, "lblDestination");
            this.lblDestination.Name = "lblDestination";
            // 
            // ShukkaKeikakuBunkatsu
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShukkaKeikakuBunkatsu";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.lblNumSum.ChildPanel.ResumeLayout(false);
            this.lblNumSum.ChildPanel.PerformLayout();
            this.lblNumSum.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DSWControl.DSWLabel.DSWLabel lblNumSum;
        private DSWControl.DSWTextBox.DSWTextBox txtNumSum;
        private DSWControl.DSWTextBox.DSWTextBox dswTextBox2;
        private DSWControl.DSWTextBox.DSWTextBox txtNumDestination;
        private DSWControl.DSWTextBox.DSWTextBox txtNumSource;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnDecision;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnClose;
        private System.Windows.Forms.Label lblSum;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.Label lblDestination;
        private DSWControl.DSWLabel.CustomPanel ChildPanel;

    }
}