namespace SMS.P02.Forms
{
    partial class SenderSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SenderSetting));
            this.txtSearchUserName = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblSearchUserName = new DSWControl.DSWLabel.DSWLabel();
            this.shtSend = new GrapeCity.Win.ElTabelle.Sheet();
            this.btnSet = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnRowDelete = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnAllClear = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.txtMassAdd = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnMassAdd = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.grpSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.lblSearchUserName.ChildPanel.SuspendLayout();
            this.lblSearchUserName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtSend)).BeginInit();
            this.SuspendLayout();
            // 
            // grpSearch
            // 
            this.grpSearch.Controls.Add(this.lblSearchUserName);
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Controls.SetChildIndex(this.lblSearchUserName, 0);
            this.grpSearch.Controls.SetChildIndex(this.btnSearch, 0);
            this.grpSearch.Controls.SetChildIndex(this.btnSearchAll, 0);
            // 
            // btnSearch
            // 
            resources.ApplyResources(this.btnSearch, "btnSearch");
            // 
            // btnSelect
            // 
            resources.ApplyResources(this.btnSelect, "btnSelect");
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            // 
            // shtResult
            // 
            resources.ApplyResources(this.shtResult, "shtResult");
            this.shtResult.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtResult.Data")));
            this.shtResult.CellDoubleClick += new GrapeCity.Win.ElTabelle.ClickEventHandler(this.shtResult_CellDoubleClick);
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.btnMassAdd);
            this.pnlMain.Controls.Add(this.txtMassAdd);
            this.pnlMain.Controls.Add(this.btnAllClear);
            this.pnlMain.Controls.Add(this.btnRowDelete);
            this.pnlMain.Controls.Add(this.btnSet);
            this.pnlMain.Controls.Add(this.shtSend);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.SetChildIndex(this.shtSend, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnSet, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnRowDelete, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnAllClear, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnClose, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnSelect, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtResult, 0);
            this.pnlMain.Controls.SetChildIndex(this.txtMassAdd, 0);
            this.pnlMain.Controls.SetChildIndex(this.btnMassAdd, 0);
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
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // txtSearchUserName
            // 
            resources.ApplyResources(this.txtSearchUserName, "txtSearchUserName");
            this.txtSearchUserName.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtSearchUserName.InputRegulation = "F";
            this.txtSearchUserName.MaxByteLengthMode = true;
            this.txtSearchUserName.Name = "txtSearchUserName";
            this.txtSearchUserName.OneLineMaxLength = 40;
            this.txtSearchUserName.ProhibitionChar = null;
            this.txtSearchUserName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchUserName_KeyDown);
            // 
            // lblSearchUserName
            // 
            // 
            // lblSearchUserName.ChildPanel
            // 
            this.lblSearchUserName.ChildPanel.Controls.Add(this.txtSearchUserName);
            this.lblSearchUserName.IsFocusChangeColor = false;
            this.lblSearchUserName.LabelWidth = 100;
            resources.ApplyResources(this.lblSearchUserName, "lblSearchUserName");
            this.lblSearchUserName.Name = "lblSearchUserName";
            this.lblSearchUserName.SplitterWidth = 0;
            // 
            // shtSend
            // 
            this.shtSend.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtSend.Data")));
            resources.ApplyResources(this.shtSend, "shtSend");
            this.shtSend.Name = "shtSend";
            this.shtSend.CellDoubleClick += new GrapeCity.Win.ElTabelle.ClickEventHandler(this.shtSend_CellDoubleClick);
            // 
            // btnSet
            // 
            resources.ApplyResources(this.btnSet, "btnSet");
            this.btnSet.Name = "btnSet";
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // btnRowDelete
            // 
            resources.ApplyResources(this.btnRowDelete, "btnRowDelete");
            this.btnRowDelete.Name = "btnRowDelete";
            this.btnRowDelete.Click += new System.EventHandler(this.btnRowDelete_Click);
            // 
            // btnAllClear
            // 
            resources.ApplyResources(this.btnAllClear, "btnAllClear");
            this.btnAllClear.Name = "btnAllClear";
            this.btnAllClear.Click += new System.EventHandler(this.btnAllClear_Click);
            // 
            // txtMassAdd
            // 
            this.txtMassAdd.FocusBackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.txtMassAdd, "txtMassAdd");
            this.txtMassAdd.InputRegulation = "F";
            this.txtMassAdd.MaxByteLengthMode = true;
            this.txtMassAdd.Name = "txtMassAdd";
            this.txtMassAdd.OneLineMaxLength = 500;
            this.txtMassAdd.ProhibitionChar = null;
            // 
            // btnMassAdd
            // 
            resources.ApplyResources(this.btnMassAdd, "btnMassAdd");
            this.btnMassAdd.Name = "btnMassAdd";
            this.btnMassAdd.Click += new System.EventHandler(this.btnMassAdd_Click);
            // 
            // SenderSetting
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SenderSetting";
            this.grpSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtResult)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.lblSearchUserName.ChildPanel.ResumeLayout(false);
            this.lblSearchUserName.ChildPanel.PerformLayout();
            this.lblSearchUserName.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtSend)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DSWControl.DSWLabel.DSWLabel lblSearchUserName;
        private DSWControl.DSWTextBox.DSWTextBox txtSearchUserName;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnAllClear;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnRowDelete;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnSet;
        private DSWControl.DSWTextBox.DSWTextBox txtMassAdd;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnMassAdd;
        private GrapeCity.Win.ElTabelle.Sheet shtSend;
    }
}