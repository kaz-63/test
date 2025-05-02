namespace SMS.A01.Forms
{
    partial class ARJohoMeisaiNote
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ARJohoMeisaiNote));
            this.lblNonyusakiName = new DSWControl.DSWLabel.DSWLabel();
            this.txtNonyusakiName = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblArNo = new DSWControl.DSWLabel.DSWLabel();
            this.txtArNo = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblListFlag = new DSWControl.DSWLabel.DSWLabel();
            this.txtListFlag = new DSWControl.DSWTextBox.DSWTextBox();
            this.tblNonyusaki = new System.Windows.Forms.TableLayoutPanel();
            this.tblContent = new System.Windows.Forms.TableLayoutPanel();
            this.rtbTextBox = new SMS.A01.Controls.ARJohoRichTextBox(this.components);
            this.pnlMain.SuspendLayout();
            this.lblNonyusakiName.ChildPanel.SuspendLayout();
            this.lblNonyusakiName.SuspendLayout();
            this.lblArNo.ChildPanel.SuspendLayout();
            this.lblArNo.SuspendLayout();
            this.lblListFlag.ChildPanel.SuspendLayout();
            this.lblListFlag.SuspendLayout();
            this.tblNonyusaki.SuspendLayout();
            this.tblContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.tblContent);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.SetChildIndex(this.tblContent, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            // 
            // fbrFunction
            // 
            // 
            // fbrFunction.F01Button
            // 
            resources.ApplyResources(this.fbrFunction.F01Button, "fbrFunction.F01Button");
            resources.ApplyResources(this.fbrFunction, "fbrFunction");
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
            resources.ApplyResources(this.lblNonyusakiName, "lblNonyusakiName");
            this.lblNonyusakiName.IsFocusChangeColor = false;
            this.lblNonyusakiName.IsNecessary = true;
            this.lblNonyusakiName.LabelWidth = 120;
            this.lblNonyusakiName.Name = "lblNonyusakiName";
            this.lblNonyusakiName.SplitterWidth = 0;
            this.lblNonyusakiName.TabStop = false;
            // 
            // txtNonyusakiName
            // 
            this.txtNonyusakiName.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtNonyusakiName, "txtNonyusakiName");
            this.txtNonyusakiName.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtNonyusakiName.InputRegulation = "";
            this.txtNonyusakiName.Name = "txtNonyusakiName";
            this.txtNonyusakiName.ProhibitionChar = null;
            this.txtNonyusakiName.ReadOnly = true;
            this.txtNonyusakiName.TabStop = false;
            // 
            // lblArNo
            // 
            // 
            // lblArNo.ChildPanel
            // 
            this.lblArNo.ChildPanel.Controls.Add(this.txtArNo);
            this.lblArNo.IsFocusChangeColor = false;
            this.lblArNo.LabelWidth = 80;
            resources.ApplyResources(this.lblArNo, "lblArNo");
            this.lblArNo.Name = "lblArNo";
            this.lblArNo.SplitterWidth = 0;
            this.lblArNo.TabStop = false;
            // 
            // txtArNo
            // 
            this.txtArNo.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtArNo, "txtArNo");
            this.txtArNo.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtArNo.InputRegulation = "";
            this.txtArNo.Name = "txtArNo";
            this.txtArNo.ProhibitionChar = null;
            this.txtArNo.ReadOnly = true;
            this.txtArNo.TabStop = false;
            // 
            // lblListFlag
            // 
            // 
            // lblListFlag.ChildPanel
            // 
            this.lblListFlag.ChildPanel.Controls.Add(this.txtListFlag);
            this.lblListFlag.IsFocusChangeColor = false;
            this.lblListFlag.IsNecessary = true;
            this.lblListFlag.LabelWidth = 100;
            resources.ApplyResources(this.lblListFlag, "lblListFlag");
            this.lblListFlag.Name = "lblListFlag";
            this.lblListFlag.SplitterWidth = 0;
            this.lblListFlag.TabStop = false;
            // 
            // txtListFlag
            // 
            this.txtListFlag.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtListFlag, "txtListFlag");
            this.txtListFlag.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtListFlag.InputRegulation = "";
            this.txtListFlag.Name = "txtListFlag";
            this.txtListFlag.ProhibitionChar = null;
            this.txtListFlag.ReadOnly = true;
            this.txtListFlag.TabStop = false;
            // 
            // tblNonyusaki
            // 
            resources.ApplyResources(this.tblNonyusaki, "tblNonyusaki");
            this.tblContent.SetColumnSpan(this.tblNonyusaki, 2);
            this.tblNonyusaki.Controls.Add(this.lblArNo, 2, 0);
            this.tblNonyusaki.Controls.Add(this.lblListFlag, 1, 0);
            this.tblNonyusaki.Controls.Add(this.lblNonyusakiName, 0, 0);
            this.tblNonyusaki.MinimumSize = new System.Drawing.Size(600, 0);
            this.tblNonyusaki.Name = "tblNonyusaki";
            // 
            // tblContent
            // 
            resources.ApplyResources(this.tblContent, "tblContent");
            this.tblContent.Controls.Add(this.tblNonyusaki, 0, 0);
            this.tblContent.Controls.Add(this.rtbTextBox, 0, 1);
            this.tblContent.Name = "tblContent";
            // 
            // rtbTextBox
            // 
            this.tblContent.SetColumnSpan(this.rtbTextBox, 2);
            resources.ApplyResources(this.rtbTextBox, "rtbTextBox");
            this.rtbTextBox.FocusBackColor = System.Drawing.Color.Empty;
            this.rtbTextBox.HideSelection = false;
            this.rtbTextBox.InputRegulation = "F";
            this.rtbTextBox.MaxByteLengthMode = true;
            this.rtbTextBox.MaxLineCount = 2000;
            this.rtbTextBox.Name = "rtbTextBox";
            this.rtbTextBox.ProhibitionChar = null;
            this.rtbTextBox.Pasted += new System.EventHandler<DSWControl.DSWRichTextBox.DSWRichTextBoxPasteEventArgs>(this.rtbTextBox_Pasted);
            // 
            // ARJohoMeisaiNote
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ARJohoMeisaiNote";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.lblNonyusakiName.ChildPanel.ResumeLayout(false);
            this.lblNonyusakiName.ChildPanel.PerformLayout();
            this.lblNonyusakiName.ResumeLayout(false);
            this.lblArNo.ChildPanel.ResumeLayout(false);
            this.lblArNo.ChildPanel.PerformLayout();
            this.lblArNo.ResumeLayout(false);
            this.lblListFlag.ChildPanel.ResumeLayout(false);
            this.lblListFlag.ChildPanel.PerformLayout();
            this.lblListFlag.ResumeLayout(false);
            this.tblNonyusaki.ResumeLayout(false);
            this.tblContent.ResumeLayout(false);
            this.tblContent.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DSWControl.DSWLabel.DSWLabel lblNonyusakiName;
        private DSWControl.DSWTextBox.DSWTextBox txtNonyusakiName;
        private DSWControl.DSWLabel.DSWLabel lblArNo;
        private DSWControl.DSWTextBox.DSWTextBox txtArNo;
        private DSWControl.DSWLabel.DSWLabel lblListFlag;
        private DSWControl.DSWTextBox.DSWTextBox txtListFlag;
        private System.Windows.Forms.TableLayoutPanel tblNonyusaki;
        private System.Windows.Forms.TableLayoutPanel tblContent;
        private SMS.A01.Controls.ARJohoRichTextBox rtbTextBox;
    }
}