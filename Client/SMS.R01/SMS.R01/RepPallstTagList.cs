using System;
using System.Data;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Document;
using Commons;

namespace SMS.R01
{
    /// <summary>
    /// RepBoxTagList の概要の説明です。
    /// </summary>
    public partial class RepPalletTagList : DataDynamics.ActiveReports.ActiveReport
    {

        public RepPalletTagList()
        {
            //
            // ActiveReport デザイナ サポートに必要です。
            //
            InitializeComponent();
        }

        /// <summary>
        /// DBから該当物件の納入先名を取得
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepPalletTagList_ReportStart(object sender, EventArgs e)
        {
            try
            {

                DataSet ds = this.DataSource as DataSet;
                if (ds != null && ds.Tables.Contains(Def_T_PALLETLIST_MANAGE.Name))
                {
                    string nonyusaki_name = ComFunc.GetFld(ds, Def_T_PALLETLIST_MANAGE.Name, 0, Def_M_NONYUSAKI.NONYUSAKI_NAME, "");
                    int[] fontSizes = { 70, 64, 58, 54, 50, 48, 44, 42, 40, 38, 36, 35, 34, 32, 31, 29, 28, 28, 26, 25};

                    // 納入先名byte数チェック
                    double byteOfName = Encoding.GetEncoding("Shift-JIS").GetByteCount(nonyusaki_name);
                    if (byteOfName <= 20)
                    {
                        return;
                    }
                    double fontSize;
                    fontSize = Math.Ceiling((byteOfName - 20) / 2);

                    Font ft = new Font(this.label5.Font.Name, fontSizes[(int)fontSize - 1]);
                    this.label5.Font = ft;
                }
            }

            catch (Exception) { }

        }

    }
}
