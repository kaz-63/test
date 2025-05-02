namespace SMS.S01.Forms
{
    partial class IkkatsuUpload
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IkkatsuUpload));
            this.lblPicture = new DSWControl.DSWLabel.DSWLabel();
            this.txtPicture = new DSWControl.DSWTextBox.DSWTextBox();
            this.shtMeisai = new GrapeCity.Win.ElTabelle.Sheet();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.btnStart = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnReference = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.pnlMain.SuspendLayout();
            this.lblPicture.ChildPanel.SuspendLayout();
            this.lblPicture.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).BeginInit();
            this.grpSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.grpSearch);
            this.pnlMain.Controls.Add(this.shtMeisai);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtMeisai, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
            // 
            // fbrFunction
            // 
            // 
            // fbrFunction.F01Button
            // 
            resources.ApplyResources(this.fbrFunction.F01Button, "fbrFunction.F01Button");
            resources.ApplyResources(this.fbrFunction, "fbrFunction");
            // 
            // fbrFunction.F06Button
            // 
            resources.ApplyResources(this.fbrFunction.F06Button, "fbrFunction.F06Button");
            // 
            // fbrFunction.F07Button
            // 
            resources.ApplyResources(this.fbrFunction.F07Button, "fbrFunction.F07Button");
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // lblPicture
            // 
            // 
            // lblPicture.ChildPanel
            // 
            this.lblPicture.ChildPanel.Controls.Add(this.txtPicture);
            this.lblPicture.IsFocusChangeColor = false;
            this.lblPicture.IsNecessary = true;
            this.lblPicture.LabelWidth = 100;
            resources.ApplyResources(this.lblPicture, "lblPicture");
            this.lblPicture.Name = "lblPicture";
            this.lblPicture.SplitterWidth = 0;
            this.lblPicture.TabStop = false;
            // 
            // txtPicture
            // 
            this.txtPicture.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtPicture, "txtPicture");
            this.txtPicture.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtPicture.InputRegulation = "";
            this.txtPicture.Name = "txtPicture";
            this.txtPicture.ProhibitionChar = null;
            this.txtPicture.ReadOnly = true;
            this.txtPicture.TabStop = false;
            // 
            // shtMeisai
            // 
            resources.ApplyResources(this.shtMeisai, "shtMeisai");
            this.shtMeisai.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtMeisai.Data")));
            this.shtMeisai.Name = "shtMeisai";
            this.shtMeisai.CellNotify += new GrapeCity.Win.ElTabelle.CellNotifyEventHandler(this.shtMeisai_CellNotify);
            // 
            // grpSearch
            // 
            this.grpSearch.Controls.Add(this.btnStart);
            this.grpSearch.Controls.Add(this.btnReference);
            this.grpSearch.Controls.Add(this.lblPicture);
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
            // 
            // btnStart
            // 
            resources.ApplyResources(this.btnStart, "btnStart");
            this.btnStart.Name = "btnStart";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnReference
            // 
            resources.ApplyResources(this.btnReference, "btnReference");
            this.btnReference.Name = "btnReference";
            this.btnReference.TabStop = false;
            this.btnReference.Click += new System.EventHandler(this.btnReference_Click);
            // 
            // IkkatsuUpload
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "IkkatsuUpload";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.lblPicture.ChildPanel.ResumeLayout(false);
            this.lblPicture.ChildPanel.PerformLayout();
            this.lblPicture.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).EndInit();
            this.grpSearch.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSearch;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnStart;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnReference;
        private DSWControl.DSWLabel.DSWLabel lblPicture;
        private DSWControl.DSWTextBox.DSWTextBox txtPicture;
        protected GrapeCity.Win.ElTabelle.Sheet shtMeisai;
    }
}