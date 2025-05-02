namespace SMS.S01.Forms
{
    partial class ShukkaKeikakuShokai
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShukkaKeikakuShokai));
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.tblSearchCondition = new System.Windows.Forms.TableLayoutPanel();
            this.lblKishu = new DSWControl.DSWLabel.DSWLabel();
            this.cboKishu = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblSeiban = new DSWControl.DSWLabel.DSWLabel();
            this.cboSeiban = new DSWControl.DSWComboBox.DSWComboBox();
            this.btnStart = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblBukkenName = new DSWControl.DSWLabel.DSWLabel();
            this.cboBukkenName = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblNiukesaki = new DSWControl.DSWLabel.DSWLabel();
            this.cboNiukesaki = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblShukkamoto = new DSWControl.DSWLabel.DSWLabel();
            this.cboShukkamoto = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblShukkasaki = new DSWControl.DSWLabel.DSWLabel();
            this.cboShukkasaki = new DSWControl.DSWComboBox.DSWComboBox();
            this.lblShukkaDate = new DSWControl.DSWLabel.DSWLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpShukkaDateStart = new System.Windows.Forms.DateTimePicker();
            this.dtpShukkaDateEnd = new System.Windows.Forms.DateTimePicker();
            this.shtMeisai = new GrapeCity.Win.ElTabelle.Sheet();
            this.ofdExcel = new System.Windows.Forms.OpenFileDialog();
            this.dswComboBox1 = new DSWControl.DSWComboBox.DSWComboBox();
            this.ChildPanel = new DSWControl.DSWLabel.CustomPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePicker4 = new System.Windows.Forms.DateTimePicker();
            this.dswFunctionButton1 = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dswLabel6 = new DSWControl.DSWLabel.DSWLabel();
            this.dswComboBox5 = new DSWControl.DSWComboBox.DSWComboBox();
            this.dswLabel7 = new DSWControl.DSWLabel.DSWLabel();
            this.dswComboBox6 = new DSWControl.DSWComboBox.DSWComboBox();
            this.dswLabel8 = new DSWControl.DSWLabel.DSWLabel();
            this.dswComboBox7 = new DSWControl.DSWComboBox.DSWComboBox();
            this.dswLabel9 = new DSWControl.DSWLabel.DSWLabel();
            this.dswComboBox8 = new DSWControl.DSWComboBox.DSWComboBox();
            this.dswLabel10 = new DSWControl.DSWLabel.DSWLabel();
            this.dswComboBox9 = new DSWControl.DSWComboBox.DSWComboBox();
            this.dswLabel11 = new DSWControl.DSWLabel.DSWLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.dateTimePicker5 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker6 = new System.Windows.Forms.DateTimePicker();
            this.pnlMain.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.tblSearchCondition.SuspendLayout();
            this.lblKishu.ChildPanel.SuspendLayout();
            this.lblKishu.SuspendLayout();
            this.lblSeiban.ChildPanel.SuspendLayout();
            this.lblSeiban.SuspendLayout();
            this.lblBukkenName.ChildPanel.SuspendLayout();
            this.lblBukkenName.SuspendLayout();
            this.lblNiukesaki.ChildPanel.SuspendLayout();
            this.lblNiukesaki.SuspendLayout();
            this.lblShukkamoto.ChildPanel.SuspendLayout();
            this.lblShukkamoto.SuspendLayout();
            this.lblShukkasaki.ChildPanel.SuspendLayout();
            this.lblShukkasaki.SuspendLayout();
            this.lblShukkaDate.ChildPanel.SuspendLayout();
            this.lblShukkaDate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.dswLabel6.ChildPanel.SuspendLayout();
            this.dswLabel6.SuspendLayout();
            this.dswLabel7.ChildPanel.SuspendLayout();
            this.dswLabel7.SuspendLayout();
            this.dswLabel8.ChildPanel.SuspendLayout();
            this.dswLabel8.SuspendLayout();
            this.dswLabel9.ChildPanel.SuspendLayout();
            this.dswLabel9.SuspendLayout();
            this.dswLabel10.ChildPanel.SuspendLayout();
            this.dswLabel10.SuspendLayout();
            this.dswLabel11.ChildPanel.SuspendLayout();
            this.dswLabel11.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.grpSearch);
            this.pnlMain.Controls.Add(this.shtMeisai);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.SetChildIndex(this.shtMeisai, 0);
            this.pnlMain.Controls.SetChildIndex(this.grpSearch, 0);
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
            // fbrFunction.F06Button
            // 
            resources.ApplyResources(this.fbrFunction.F06Button, "fbrFunction.F06Button");
            // 
            // fbrFunction.F07Button
            // 
            resources.ApplyResources(this.fbrFunction.F07Button, "fbrFunction.F07Button");
            // 
            // fbrFunction.F10Button
            // 
            resources.ApplyResources(this.fbrFunction.F10Button, "fbrFunction.F10Button");
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            // 
            // grpSearch
            // 
            this.grpSearch.Controls.Add(this.tblSearchCondition);
            resources.ApplyResources(this.grpSearch, "grpSearch");
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.TabStop = false;
            // 
            // tblSearchCondition
            // 
            resources.ApplyResources(this.tblSearchCondition, "tblSearchCondition");
            this.tblSearchCondition.Controls.Add(this.lblKishu, 2, 1);
            this.tblSearchCondition.Controls.Add(this.lblSeiban, 1, 1);
            this.tblSearchCondition.Controls.Add(this.btnStart, 3, 1);
            this.tblSearchCondition.Controls.Add(this.lblBukkenName, 0, 0);
            this.tblSearchCondition.Controls.Add(this.lblNiukesaki, 3, 0);
            this.tblSearchCondition.Controls.Add(this.lblShukkamoto, 1, 0);
            this.tblSearchCondition.Controls.Add(this.lblShukkasaki, 2, 0);
            this.tblSearchCondition.Controls.Add(this.lblShukkaDate, 0, 1);
            this.tblSearchCondition.Name = "tblSearchCondition";
            // 
            // lblKishu
            // 
            // 
            // lblKishu.ChildPanel
            // 
            this.lblKishu.ChildPanel.Controls.Add(this.cboKishu);
            this.lblKishu.IsFocusChangeColor = false;
            this.lblKishu.LabelWidth = 60;
            resources.ApplyResources(this.lblKishu, "lblKishu");
            this.lblKishu.Name = "lblKishu";
            this.lblKishu.SplitterWidth = 0;
            // 
            // cboKishu
            // 
            this.cboKishu.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboKishu.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.cboKishu, "cboKishu");
            this.cboKishu.Name = "cboKishu";
            this.cboKishu.SelectionChangeCommitted += new System.EventHandler(this.ComboBox_Filter);
            this.cboKishu.TextChanged += new System.EventHandler(this.ComboBox_Filter);
            // 
            // lblSeiban
            // 
            // 
            // lblSeiban.ChildPanel
            // 
            this.lblSeiban.ChildPanel.Controls.Add(this.cboSeiban);
            this.lblSeiban.IsFocusChangeColor = false;
            this.lblSeiban.LabelWidth = 60;
            resources.ApplyResources(this.lblSeiban, "lblSeiban");
            this.lblSeiban.Name = "lblSeiban";
            this.lblSeiban.SplitterWidth = 0;
            // 
            // cboSeiban
            // 
            this.cboSeiban.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSeiban.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.cboSeiban, "cboSeiban");
            this.cboSeiban.Name = "cboSeiban";
            this.cboSeiban.SelectionChangeCommitted += new System.EventHandler(this.ComboBox_Filter);
            this.cboSeiban.TextChanged += new System.EventHandler(this.ComboBox_Filter);
            // 
            // btnStart
            // 
            resources.ApplyResources(this.btnStart, "btnStart");
            this.btnStart.Name = "btnStart";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblBukkenName
            // 
            // 
            // lblBukkenName.ChildPanel
            // 
            this.lblBukkenName.ChildPanel.Controls.Add(this.cboBukkenName);
            this.lblBukkenName.IsFocusChangeColor = false;
            this.lblBukkenName.IsNecessary = true;
            this.lblBukkenName.LabelWidth = 80;
            resources.ApplyResources(this.lblBukkenName, "lblBukkenName");
            this.lblBukkenName.Name = "lblBukkenName";
            this.lblBukkenName.SplitterWidth = 0;
            // 
            // cboBukkenName
            // 
            this.cboBukkenName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboBukkenName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.cboBukkenName, "cboBukkenName");
            this.cboBukkenName.Name = "cboBukkenName";
            // 
            // lblNiukesaki
            // 
            // 
            // lblNiukesaki.ChildPanel
            // 
            this.lblNiukesaki.ChildPanel.Controls.Add(this.cboNiukesaki);
            this.lblNiukesaki.IsFocusChangeColor = false;
            this.lblNiukesaki.LabelWidth = 60;
            resources.ApplyResources(this.lblNiukesaki, "lblNiukesaki");
            this.lblNiukesaki.Name = "lblNiukesaki";
            this.lblNiukesaki.SplitterWidth = 0;
            // 
            // cboNiukesaki
            // 
            this.cboNiukesaki.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboNiukesaki.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.cboNiukesaki, "cboNiukesaki");
            this.cboNiukesaki.Name = "cboNiukesaki";
            this.cboNiukesaki.SelectionChangeCommitted += new System.EventHandler(this.ComboBox_Filter);
            this.cboNiukesaki.TextChanged += new System.EventHandler(this.ComboBox_Filter);
            // 
            // lblShukkamoto
            // 
            // 
            // lblShukkamoto.ChildPanel
            // 
            this.lblShukkamoto.ChildPanel.Controls.Add(this.cboShukkamoto);
            this.lblShukkamoto.IsFocusChangeColor = false;
            this.lblShukkamoto.LabelWidth = 60;
            resources.ApplyResources(this.lblShukkamoto, "lblShukkamoto");
            this.lblShukkamoto.Name = "lblShukkamoto";
            this.lblShukkamoto.SplitterWidth = 0;
            // 
            // cboShukkamoto
            // 
            this.cboShukkamoto.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboShukkamoto.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.cboShukkamoto, "cboShukkamoto");
            this.cboShukkamoto.Name = "cboShukkamoto";
            this.cboShukkamoto.SelectionChangeCommitted += new System.EventHandler(this.ComboBox_Filter);
            this.cboShukkamoto.TextChanged += new System.EventHandler(this.ComboBox_Filter);
            // 
            // lblShukkasaki
            // 
            // 
            // lblShukkasaki.ChildPanel
            // 
            this.lblShukkasaki.ChildPanel.Controls.Add(this.cboShukkasaki);
            this.lblShukkasaki.IsFocusChangeColor = false;
            this.lblShukkasaki.LabelWidth = 60;
            resources.ApplyResources(this.lblShukkasaki, "lblShukkasaki");
            this.lblShukkasaki.Name = "lblShukkasaki";
            this.lblShukkasaki.SplitterWidth = 0;
            // 
            // cboShukkasaki
            // 
            this.cboShukkasaki.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboShukkasaki.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.cboShukkasaki, "cboShukkasaki");
            this.cboShukkasaki.Name = "cboShukkasaki";
            this.cboShukkasaki.SelectionChangeCommitted += new System.EventHandler(this.ComboBox_Filter);
            this.cboShukkasaki.TextChanged += new System.EventHandler(this.ComboBox_Filter);
            // 
            // lblShukkaDate
            // 
            // 
            // lblShukkaDate.ChildPanel
            // 
            this.lblShukkaDate.ChildPanel.Controls.Add(this.label2);
            this.lblShukkaDate.ChildPanel.Controls.Add(this.dtpShukkaDateStart);
            this.lblShukkaDate.ChildPanel.Controls.Add(this.dtpShukkaDateEnd);
            this.lblShukkaDate.IsFocusChangeColor = false;
            this.lblShukkaDate.LabelWidth = 80;
            resources.ApplyResources(this.lblShukkaDate, "lblShukkaDate");
            this.lblShukkaDate.Name = "lblShukkaDate";
            this.lblShukkaDate.SplitterWidth = 0;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // dtpShukkaDateStart
            // 
            resources.ApplyResources(this.dtpShukkaDateStart, "dtpShukkaDateStart");
            this.dtpShukkaDateStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpShukkaDateStart.Name = "dtpShukkaDateStart";
            // 
            // dtpShukkaDateEnd
            // 
            resources.ApplyResources(this.dtpShukkaDateEnd, "dtpShukkaDateEnd");
            this.dtpShukkaDateEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpShukkaDateEnd.Name = "dtpShukkaDateEnd";
            // 
            // shtMeisai
            // 
            resources.ApplyResources(this.shtMeisai, "shtMeisai");
            this.shtMeisai.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("shtMeisai.Data")));
            this.shtMeisai.Name = "shtMeisai";
            // 
            // ofdExcel
            // 
            resources.ApplyResources(this.ofdExcel, "ofdExcel");
            // 
            // dswComboBox1
            // 
            this.dswComboBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.dswComboBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.dswComboBox1, "dswComboBox1");
            this.dswComboBox1.Name = "dswComboBox1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dateTimePicker4);
            this.groupBox1.Controls.Add(this.dswFunctionButton1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // dateTimePicker4
            // 
            resources.ApplyResources(this.dateTimePicker4, "dateTimePicker4");
            this.dateTimePicker4.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker4.Name = "dateTimePicker4";
            // 
            // dswFunctionButton1
            // 
            resources.ApplyResources(this.dswFunctionButton1, "dswFunctionButton1");
            this.dswFunctionButton1.Name = "dswFunctionButton1";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.dswLabel6, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.dswLabel7, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dswLabel8, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.dswLabel9, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.dswLabel10, 2, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // dswLabel6
            // 
            // 
            // dswLabel6.ChildPanel
            // 
            this.dswLabel6.ChildPanel.Controls.Add(this.dswComboBox5);
            this.dswLabel6.IsFocusChangeColor = false;
            this.dswLabel6.LabelWidth = 9;
            resources.ApplyResources(this.dswLabel6, "dswLabel6");
            this.dswLabel6.Name = "dswLabel6";
            this.dswLabel6.SplitterWidth = 0;
            // 
            // dswComboBox5
            // 
            this.dswComboBox5.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.dswComboBox5.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.dswComboBox5, "dswComboBox5");
            this.dswComboBox5.Name = "dswComboBox5";
            // 
            // dswLabel7
            // 
            // 
            // dswLabel7.ChildPanel
            // 
            this.dswLabel7.ChildPanel.Controls.Add(this.dswComboBox6);
            this.dswLabel7.IsFocusChangeColor = false;
            this.dswLabel7.IsNecessary = true;
            this.dswLabel7.LabelWidth = 9;
            resources.ApplyResources(this.dswLabel7, "dswLabel7");
            this.dswLabel7.Name = "dswLabel7";
            this.dswLabel7.SplitterWidth = 0;
            // 
            // dswComboBox6
            // 
            this.dswComboBox6.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.dswComboBox6.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.dswComboBox6, "dswComboBox6");
            this.dswComboBox6.Name = "dswComboBox6";
            // 
            // dswLabel8
            // 
            // 
            // dswLabel8.ChildPanel
            // 
            this.dswLabel8.ChildPanel.Controls.Add(this.dswComboBox7);
            this.dswLabel8.IsFocusChangeColor = false;
            this.dswLabel8.LabelWidth = 60;
            resources.ApplyResources(this.dswLabel8, "dswLabel8");
            this.dswLabel8.Name = "dswLabel8";
            this.dswLabel8.SplitterWidth = 0;
            // 
            // dswComboBox7
            // 
            this.dswComboBox7.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.dswComboBox7.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.dswComboBox7, "dswComboBox7");
            this.dswComboBox7.Name = "dswComboBox7";
            // 
            // dswLabel9
            // 
            // 
            // dswLabel9.ChildPanel
            // 
            this.dswLabel9.ChildPanel.Controls.Add(this.dswComboBox8);
            this.dswLabel9.IsFocusChangeColor = false;
            this.dswLabel9.LabelWidth = 9;
            resources.ApplyResources(this.dswLabel9, "dswLabel9");
            this.dswLabel9.Name = "dswLabel9";
            this.dswLabel9.SplitterWidth = 0;
            // 
            // dswComboBox8
            // 
            this.dswComboBox8.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.dswComboBox8.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.dswComboBox8, "dswComboBox8");
            this.dswComboBox8.Name = "dswComboBox8";
            // 
            // dswLabel10
            // 
            // 
            // dswLabel10.ChildPanel
            // 
            this.dswLabel10.ChildPanel.Controls.Add(this.dswComboBox9);
            this.dswLabel10.IsFocusChangeColor = false;
            this.dswLabel10.LabelWidth = 9;
            resources.ApplyResources(this.dswLabel10, "dswLabel10");
            this.dswLabel10.Name = "dswLabel10";
            this.dswLabel10.SplitterWidth = 0;
            // 
            // dswComboBox9
            // 
            this.dswComboBox9.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.dswComboBox9.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.dswComboBox9, "dswComboBox9");
            this.dswComboBox9.Name = "dswComboBox9";
            // 
            // dswLabel11
            // 
            // 
            // dswLabel11.ChildPanel
            // 
            this.dswLabel11.ChildPanel.Controls.Add(this.label4);
            this.dswLabel11.ChildPanel.Controls.Add(this.dateTimePicker5);
            this.dswLabel11.ChildPanel.Controls.Add(this.dateTimePicker6);
            this.dswLabel11.IsFocusChangeColor = false;
            this.dswLabel11.LabelWidth = 70;
            resources.ApplyResources(this.dswLabel11, "dswLabel11");
            this.dswLabel11.Name = "dswLabel11";
            this.dswLabel11.SplitterWidth = 0;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // dateTimePicker5
            // 
            resources.ApplyResources(this.dateTimePicker5, "dateTimePicker5");
            this.dateTimePicker5.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker5.Name = "dateTimePicker5";
            // 
            // dateTimePicker6
            // 
            resources.ApplyResources(this.dateTimePicker6, "dateTimePicker6");
            this.dateTimePicker6.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker6.Name = "dateTimePicker6";
            // 
            // ShukkaKeikakuShokai
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ShukkaKeikakuShokai";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.grpSearch.ResumeLayout(false);
            this.tblSearchCondition.ResumeLayout(false);
            this.lblKishu.ChildPanel.ResumeLayout(false);
            this.lblKishu.ResumeLayout(false);
            this.lblSeiban.ChildPanel.ResumeLayout(false);
            this.lblSeiban.ResumeLayout(false);
            this.lblBukkenName.ChildPanel.ResumeLayout(false);
            this.lblBukkenName.ResumeLayout(false);
            this.lblNiukesaki.ChildPanel.ResumeLayout(false);
            this.lblNiukesaki.ResumeLayout(false);
            this.lblShukkamoto.ChildPanel.ResumeLayout(false);
            this.lblShukkamoto.ResumeLayout(false);
            this.lblShukkasaki.ChildPanel.ResumeLayout(false);
            this.lblShukkasaki.ResumeLayout(false);
            this.lblShukkaDate.ChildPanel.ResumeLayout(false);
            this.lblShukkaDate.ChildPanel.PerformLayout();
            this.lblShukkaDate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shtMeisai)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.dswLabel6.ChildPanel.ResumeLayout(false);
            this.dswLabel6.ResumeLayout(false);
            this.dswLabel7.ChildPanel.ResumeLayout(false);
            this.dswLabel7.ResumeLayout(false);
            this.dswLabel8.ChildPanel.ResumeLayout(false);
            this.dswLabel8.ResumeLayout(false);
            this.dswLabel9.ChildPanel.ResumeLayout(false);
            this.dswLabel9.ResumeLayout(false);
            this.dswLabel10.ChildPanel.ResumeLayout(false);
            this.dswLabel10.ResumeLayout(false);
            this.dswLabel11.ChildPanel.ResumeLayout(false);
            this.dswLabel11.ChildPanel.PerformLayout();
            this.dswLabel11.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSearch;
        private GrapeCity.Win.ElTabelle.Sheet shtMeisai;
        private System.Windows.Forms.OpenFileDialog ofdExcel;
        private System.Windows.Forms.TableLayoutPanel tblSearchCondition;
        private DSWControl.DSWLabel.DSWLabel lblBukkenName;
        private DSWControl.DSWComboBox.DSWComboBox cboBukkenName;
        private DSWControl.DSWFunctionButton.DSWFunctionButton btnStart;
        private DSWControl.DSWLabel.DSWLabel lblShukkamoto;
        private DSWControl.DSWComboBox.DSWComboBox cboShukkamoto;
        private DSWControl.DSWComboBox.DSWComboBox dswComboBox1;
        private DSWControl.DSWLabel.DSWLabel lblNiukesaki;
        private DSWControl.DSWComboBox.DSWComboBox cboNiukesaki;
        private DSWControl.DSWLabel.DSWLabel lblShukkasaki;
        private DSWControl.DSWComboBox.DSWComboBox cboShukkasaki;
        private DSWControl.DSWLabel.DSWLabel lblShukkaDate;
        private System.Windows.Forms.DateTimePicker dtpShukkaDateEnd;
        private System.Windows.Forms.DateTimePicker dtpShukkaDateStart;
        private DSWControl.DSWLabel.DSWLabel lblSeiban;
        private DSWControl.DSWComboBox.DSWComboBox cboSeiban;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePicker4;
        private DSWControl.DSWFunctionButton.DSWFunctionButton dswFunctionButton1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DSWControl.DSWLabel.DSWLabel dswLabel6;
        private DSWControl.DSWComboBox.DSWComboBox dswComboBox5;
        private DSWControl.DSWLabel.DSWLabel dswLabel7;
        private DSWControl.DSWComboBox.DSWComboBox dswComboBox6;
        private DSWControl.DSWLabel.DSWLabel dswLabel8;
        private DSWControl.DSWComboBox.DSWComboBox dswComboBox7;
        private DSWControl.DSWLabel.DSWLabel dswLabel9;
        private DSWControl.DSWComboBox.DSWComboBox dswComboBox8;
        private DSWControl.DSWLabel.DSWLabel dswLabel10;
        private DSWControl.DSWComboBox.DSWComboBox dswComboBox9;
        private DSWControl.DSWLabel.DSWLabel dswLabel11;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTimePicker5;
        private System.Windows.Forms.DateTimePicker dateTimePicker6;
        private DSWControl.DSWLabel.DSWLabel lblKishu;
        private DSWControl.DSWComboBox.DSWComboBox cboKishu;
        private DSWControl.DSWLabel.CustomPanel ChildPanel;
    }
}