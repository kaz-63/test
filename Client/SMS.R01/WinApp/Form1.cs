using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ActiveReportsHelper;

namespace WinApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnExec_Click(object sender, EventArgs e)
        {
            string printerName = txtPrinter.Text;
            List<object> reps = new List<object>();

            List<object> reports = new List<object>();
            int index = 0;
            string className = "";
            className= "SMS.R01.RepBoxLabel";
            reports.Add( ReportHelper.GetReport(className, "Boxラベル"));
            index++;
            className = "SMS.R01.RepBoxList";
            reports.Add(ReportHelper.GetReport(className, "Boxリスト"));
            index++;
            className = "SMS.R01.RepBoxTagList";
            reports.Add( ReportHelper.GetReport(className, "Boxタグリスト"));
            if (this.chkPreview.Checked)
            {
                ReportHelper.Preview(this, reports.ToArray());
            }
            else
            {
                ReportHelper.Print(printerName, reports.ToArray());
            }
        }
    }
}
