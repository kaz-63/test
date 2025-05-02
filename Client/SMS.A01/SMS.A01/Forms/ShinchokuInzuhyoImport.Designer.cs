namespace SMS.A01.Forms
{
    partial class ShinchokuInzuhyoImport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShinchokuInzuhyoImport));
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnFileSelect = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblExcel = new DSWControl.DSWLabel.DSWLabel();
            this.txtExcel = new DSWControl.DSWTextBox.DSWTextBox();
            this.btnRead = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.shtMeisai = new MultiRowTabelle.MultiRowSheet();
            this.tblNonyusaki = new System.Windows.Forms.TableLayoutPanel();
            this.lblNonyusakiName = new DSWControl.DSWLabel.DSWLabel();
            this.txtNonyusakiName = new DSWControl.DSWTextBox.DSWTextBox();
            this.ofdExcel = new System.Windows.Forms.OpenFileDialog();
            this.pnlMain.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.lblExcel.ChildPanel.SuspendLayout();
            this.lblExcel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).BeginInit();
            this.tblNonyusaki.SuspendLayout();
            this.lblNonyusakiName.ChildPanel.SuspendLayout();
            this.lblNonyusakiName.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.tblNonyusaki);
            this.pnlMain.Controls.Add(this.shtMeisai);
            this.pnlMain.Controls.Add(this.grpSearch);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
            this.pnlMain.Controls.SetChildIndex(this.shtMeisai, 0);
            this.pnlMain.Controls.SetChildIndex(this.tblNonyusaki, 0);
            // 
            // fbrFunction
            // 
            // 
            // fbrFunction.F01Button
            // 
            resources.ApplyResources(this.fbrFunction.F01Button, "fbrFunction.F01Button");
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
            this.lblCorner.ImageKey = global::SMS.A01.Properties.Resources.ARJoho_ElTabelleColumnEdit;
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // grpSearch
            // 
            this.grpSearch.Controls.Add(this.tableLayoutPanel1);
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.btnFileSelect, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblExcel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnRead, 1, 0);
            this.tableLayoutPanel1.MinimumSize = new System.Drawing.Size(600, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // btnFileSelect
            // 
            resources.ApplyResources(this.btnFileSelect, "btnFileSelect");
            this.btnFileSelect.Name = "btnFileSelect";
            this.btnFileSelect.Click += new System.EventHandler(this.btnFileSelect_Click);
            // 
            // lblExcel
            // 
            // 
            // lblExcel.ChildPanel
            // 
            this.lblExcel.ChildPanel.Controls.Add(this.txtExcel);
            resources.ApplyResources(this.lblExcel, "lblExcel");
            this.lblExcel.IsFocusChangeColor = false;
            this.lblExcel.IsNecessary = true;
            this.lblExcel.LabelWidth = 110;
            this.lblExcel.Name = "lblExcel";
            this.lblExcel.SplitterWidth = 0;
            // 
            // txtExcel
            // 
            this.txtExcel.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtExcel, "txtExcel");
            this.txtExcel.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtExcel.InputRegulation = "";
            this.txtExcel.Name = "txtExcel";
            this.txtExcel.ProhibitionChar = null;
            this.txtExcel.ReadOnly = true;
            // 
            // btnRead
            // 
            resources.ApplyResources(this.btnRead, "btnRead");
            this.btnRead.Name = "btnRead";
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // shtMeisai
            // 
            resources.ApplyResources(this.shtMeisai, "shtMeisai");
            this.shtMeisai.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtMeisai.Data")));
            this.shtMeisai.Name = "shtMeisai";
            // 
            // tblNonyusaki
            // 
            resources.ApplyResources(this.tblNonyusaki, "tblNonyusaki");
            this.tblNonyusaki.Controls.Add(this.lblNonyusakiName, 0, 0);
            this.tblNonyusaki.MinimumSize = new System.Drawing.Size(600, 0);
            this.tblNonyusaki.Name = "tblNonyusaki";
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
            this.lblNonyusakiName.LabelWidth = 110;
            this.lblNonyusakiName.Name = "lblNonyusakiName";
            this.lblNonyusakiName.SplitterWidth = 0;
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
            // ofdExcel
            // 
            resources.ApplyResources(this.ofdExcel, "ofdExcel");
            // 
            // ShinchokuInzuhyoImport
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ShinchokuInzuhyoImport";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.grpSearch.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.lblExcel.ChildPanel.ResumeLayout(false);
            this.lblExcel.ChildPanel.PerformLayout();
            this.lblExcel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).EndInit();
            this.tblNonyusaki.ResumeLayout(false);
            this.lblNonyusakiName.ChildPanel.ResumeLayout(false);
            this.lblNonyusakiName.ChildPanel.PerformLayout();
            this.lblNonyusakiName.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSearch;
        private MultiRowTabelle.MultiRowSheet shtMeisai;
        private DSWControl.DSWLabel.DSWLabel lblExcel;
        private DSWControl.DSWTextBox.DSWTextBox txtExcel;
        private System.Windows.Forms.TableLayoutPanel tblNonyusaki;
        private DSWControl.DSWLabel.DSWLabel lblNonyusakiName;
        private DSWControl.DSWTextBox.DSWTextBox txtNonyusakiName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnRead;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnFileSelect;
        private System.Windows.Forms.OpenFileDialog ofdExcel;
    }
}