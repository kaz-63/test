namespace SMS.A01.Forms
{
    partial class ShinchokuKishuIchiran
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShinchokuKishuIchiran));
            this.btnRangeRelease = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnRangeSelect = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnAllRelease = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnAllSelect = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lstKishu = new SMS.A01.Controls.DoubleBufferingListView();
            this.grpSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSearch
            // 
            resources.ApplyResources(this.grpSearch, "grpSearch");
            // 
            // btnSearch
            // 
            resources.ApplyResources(this.btnSearch, "btnSearch");
            // 
            // btnSearchAll
            // 
            resources.ApplyResources(this.btnSearchAll, "btnSearchAll");
            // 
            // btnSelect
            // 
            resources.ApplyResources(this.btnSelect, "btnSelect");
            this.btnSelect.DialogResult = System.Windows.Forms.DialogResult.OK;
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            // 
            // shtResult
            // 
            resources.ApplyResources(this.shtResult, "shtResult");
            this.shtResult.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtResult.Data")));
            this.shtResult.TabStop = false;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.lstKishu);
            this.pnlMain.Controls.Add(this.btnRangeRelease);
            this.pnlMain.Controls.Add(this.btnRangeSelect);
            this.pnlMain.Controls.Add(this.btnAllRelease);
            this.pnlMain.Controls.Add(this.btnAllSelect);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.SetChildIndex(this.btnClose, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnSelect, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtResult, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnAllSelect, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnAllRelease, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnRangeSelect, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnRangeRelease, 0);
            this.pnlMain.Controls.SetChildIndex(this.lstKishu, 0);
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
            // btnRangeRelease
            // 
            resources.ApplyResources(this.btnRangeRelease, "btnRangeRelease");
            this.btnRangeRelease.Name = "btnRangeRelease";
            this.btnRangeRelease.Click += new System.EventHandler(this.btnRangeRelease_Click);
            // 
            // btnRangeSelect
            // 
            resources.ApplyResources(this.btnRangeSelect, "btnRangeSelect");
            this.btnRangeSelect.Name = "btnRangeSelect";
            this.btnRangeSelect.Click += new System.EventHandler(this.btnRangeSelect_Click);
            // 
            // btnAllRelease
            // 
            resources.ApplyResources(this.btnAllRelease, "btnAllRelease");
            this.btnAllRelease.Name = "btnAllRelease";
            this.btnAllRelease.Click += new System.EventHandler(this.btnAllRelease_Click);
            // 
            // btnAllSelect
            // 
            resources.ApplyResources(this.btnAllSelect, "btnAllSelect");
            this.btnAllSelect.Name = "btnAllSelect";
            this.btnAllSelect.Click += new System.EventHandler(this.btnAllSelect_Click);
            // 
            // lstKishu
            // 
            resources.ApplyResources(this.lstKishu, "lstKishu");
            this.lstKishu.CheckBoxes = true;
            this.lstKishu.HideSelection = false;
            this.lstKishu.Name = "lstKishu";
            this.lstKishu.UseCompatibleStateImageBehavior = false;
            this.lstKishu.View = System.Windows.Forms.View.SmallIcon;
            this.lstKishu.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstKishu_ItemCheck);
            // 
            // ShinchokuKishuIchiran
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ShinchokuKishuIchiran";
            this.grpSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DSWControl.DSWFunctionButton.DSWFunctionButton btnRangeRelease;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnRangeSelect;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnAllRelease;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnAllSelect;
        private SMS.A01.Controls.DoubleBufferingListView lstKishu;
    }
}