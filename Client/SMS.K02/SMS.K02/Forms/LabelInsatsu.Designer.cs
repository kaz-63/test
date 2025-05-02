namespace SMS.K02.Forms
{
    partial class LabelInsatsu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LabelInsatsu));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtNum = new DSWControl.DSWTextBox.DSWTextBox();
            this.rdoBox = new System.Windows.Forms.RadioButton();
            this.rdoPallet = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlMain.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.panel1);
            this.pnlMain.Controls.Add(this.txtNum);
            this.pnlMain.Controls.Add(this.label4);
            this.pnlMain.Controls.Add(this.label3);
            this.pnlMain.Controls.Add(this.label2);
            this.pnlMain.Controls.Add(this.label1);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.SetChildIndex(this.label1, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.label2, 0);
            this.pnlMain.Controls.SetChildIndex(this.label3, 0);
            this.pnlMain.Controls.SetChildIndex(this.label4, 0);
            this.pnlMain.Controls.SetChildIndex(this.txtNum, 0);
            this.pnlMain.Controls.SetChildIndex(this.panel1, 0);
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
            // 
            // fbrFunction.F05Button
            // 
            resources.ApplyResources(this.fbrFunction.F05Button, "fbrFunction.F05Button");
            // 
            // fbrFunction.F06Button
            // 
            resources.ApplyResources(this.fbrFunction.F06Button, "fbrFunction.F06Button");
            // 
            // fbrFunction.F07Button
            // 
            resources.ApplyResources(this.fbrFunction.F07Button, "fbrFunction.F07Button");
            // 
            // fbrFunction.F08Button
            // 
            resources.ApplyResources(this.fbrFunction.F08Button, "fbrFunction.F08Button");
            // 
            // fbrFunction.F09Button
            // 
            resources.ApplyResources(this.fbrFunction.F09Button, "fbrFunction.F09Button");
            // 
            // fbrFunction.F10Button
            // 
            resources.ApplyResources(this.fbrFunction.F10Button, "fbrFunction.F10Button");
            // 
            // fbrFunction.F11Button
            // 
            resources.ApplyResources(this.fbrFunction.F11Button, "fbrFunction.F11Button");
            // 
            // fbrFunction.F12Button
            // 
            resources.ApplyResources(this.fbrFunction.F12Button, "fbrFunction.F12Button");
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // txtNum
            // 
            resources.ApplyResources(this.txtNum, "txtNum");
            this.txtNum.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtNum.InputRegulation = "n";
            this.txtNum.IsInputRegulation = true;
            this.txtNum.Name = "txtNum";
            this.txtNum.OneLineMaxLength = 2;
            this.txtNum.ProhibitionChar = null;
            // 
            // rdoBox
            // 
            resources.ApplyResources(this.rdoBox, "rdoBox");
            this.rdoBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(217)))), ((int)(((byte)(238)))));
            this.rdoBox.Checked = true;
            this.rdoBox.Name = "rdoBox";
            this.rdoBox.TabStop = true;
            this.rdoBox.UseVisualStyleBackColor = false;
            // 
            // rdoPallet
            // 
            resources.ApplyResources(this.rdoPallet, "rdoPallet");
            this.rdoPallet.Name = "rdoPallet";
            this.rdoPallet.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.rdoBox);
            this.panel1.Controls.Add(this.rdoPallet);
            this.panel1.Name = "panel1";
            // 
            // LabelInsatsu
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "LabelInsatsu";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdoPallet;
        private System.Windows.Forms.RadioButton rdoBox;
        private DSWControl.DSWTextBox.DSWTextBox txtNum;
        private System.Windows.Forms.Panel panel1;
    }
}