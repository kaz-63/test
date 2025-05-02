namespace SMS.K02.Forms
{
    partial class BoxListHakko
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BoxListHakko));
            this.btnAllNotCheck = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnAllCheck = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.grpMode = new System.Windows.Forms.GroupBox();
            this.rdoSaiHakko = new System.Windows.Forms.RadioButton();
            this.rdoSinki = new System.Windows.Forms.RadioButton();
            this.shtBoxList = new GrapeCity.Win.ElTabelle.Sheet();
            this.btnRangeNotCheck = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnRangeCheck = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.pnlMain.SuspendLayout();
            this.grpMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtBoxList)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.btnRangeNotCheck);
            this.pnlMain.Controls.Add(this.btnRangeCheck);
            this.pnlMain.Controls.Add(this.shtBoxList);
            this.pnlMain.Controls.Add(this.grpMode);
            this.pnlMain.Controls.Add(this.btnAllNotCheck);
            this.pnlMain.Controls.Add(this.btnAllCheck);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnAllCheck, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnAllNotCheck, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpMode, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtBoxList, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnRangeCheck, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnRangeNotCheck, 0);
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
            // fbrFunction.F07Button
            // 
            resources.ApplyResources(this.fbrFunction.F07Button, "fbrFunction.F07Button");
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // btnAllNotCheck
            // 
            resources.ApplyResources(this.btnAllNotCheck, "btnAllNotCheck");
            this.btnAllNotCheck.Name = "btnAllNotCheck";
            this.btnAllNotCheck.Click += new System.EventHandler(this.btnAllNotCheck_Click);
            // 
            // btnAllCheck
            // 
            resources.ApplyResources(this.btnAllCheck, "btnAllCheck");
            this.btnAllCheck.Name = "btnAllCheck";
            this.btnAllCheck.Click += new System.EventHandler(this.btnAllCheck_Click);
            // 
            // grpMode
            // 
            this.grpMode.Controls.Add(this.rdoSaiHakko);
            this.grpMode.Controls.Add(this.rdoSinki);
            resources.ApplyResources(this.grpMode, "grpMode");
            this.grpMode.Name = "grpMode";
            this.grpMode.TabStop = false;
            // 
            // rdoSaiHakko
            // 
            resources.ApplyResources(this.rdoSaiHakko, "rdoSaiHakko");
            this.rdoSaiHakko.Name = "rdoSaiHakko";
            this.rdoSaiHakko.UseVisualStyleBackColor = true;
            this.rdoSaiHakko.CheckedChanged += new System.EventHandler(this.rdoSaiHakko_CheckedChanged);
            // 
            // rdoSinki
            // 
            resources.ApplyResources(this.rdoSinki, "rdoSinki");
            this.rdoSinki.Name = "rdoSinki";
            this.rdoSinki.UseVisualStyleBackColor = true;
            this.rdoSinki.CheckedChanged += new System.EventHandler(this.rdoSinki_CheckedChanged);
            // 
            // shtBoxList
            // 
            resources.ApplyResources(this.shtBoxList, "shtBoxList");
            this.shtBoxList.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtBoxList.Data")));
            this.shtBoxList.Name = "shtBoxList";
            this.shtBoxList.LeaveEdit += new GrapeCity.Win.ElTabelle.LeaveEditEventHandler(this.shtBoxList_LeaveEdit);
            this.shtBoxList.ClippingData += new GrapeCity.Win.ElTabelle.ClippingDataEventHandler(this.shtBoxList_ClippingData);
            // 
            // btnRangeNotCheck
            // 
            resources.ApplyResources(this.btnRangeNotCheck, "btnRangeNotCheck");
            this.btnRangeNotCheck.Name = "btnRangeNotCheck";
            this.btnRangeNotCheck.Click += new System.EventHandler(this.btnRangeNotCheck_Click);
            // 
            // btnRangeCheck
            // 
            resources.ApplyResources(this.btnRangeCheck, "btnRangeCheck");
            this.btnRangeCheck.Name = "btnRangeCheck";
            this.btnRangeCheck.Click += new System.EventHandler(this.btnRangeCheck_Click);
            // 
            // BoxListHakko
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "BoxListHakko";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.grpMode.ResumeLayout(false);
            this.grpMode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtBoxList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DSWControl.DSWFunctionButton.DSWFunctionButton btnAllNotCheck;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnAllCheck;
        private GrapeCity.Win.ElTabelle.Sheet shtBoxList;
        private System.Windows.Forms.GroupBox grpMode;
        private System.Windows.Forms.RadioButton rdoSaiHakko;
        private System.Windows.Forms.RadioButton rdoSinki;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnRangeNotCheck;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnRangeCheck;
    }
}