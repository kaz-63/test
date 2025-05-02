using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Document;
using System.Globalization;
using System.Data;
using Commons;

namespace SMS.R01
{
    /// <summary>
    /// RepMasterPackingList の概要の説明です。
    /// </summary>
    public partial class RepMasterPackingList : DataDynamics.ActiveReports.ActiveReport
    {

        public RepMasterPackingList()
        {
            //
            // ActiveReport デザイナ サポートに必要です。
            //
            InitializeComponent();
        }

        private void RepMasterPackingList_ReportStart(object sender, EventArgs e)
        {
            //this.lblDate.Text = DateTime.Now.ToString("dd-MMM-yyyy", DateTimeFormatInfo.InvariantInfo);

            DataSet ds = this.DataSource as DataSet;
            if (ds != null && ds.Tables.Contains(Def_T_KIWAKU.Name))
            {
                DataTable dt = ds.Tables[Def_T_KIWAKU.Name];
                Image img = ComFunc.GetFldObject(dt, 0, ComDefine.FLD_CASE_MARK) as Image;
                if (img != null)
                {
                    Bitmap bmp = new Bitmap(img);
                    caseMark.Image = bmp;
                }
            }
        }

    }
}
