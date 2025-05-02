using System;
using System.Data;
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
    /// RepPalletList の概要の説明です。
    /// </summary>
    public partial class RepPalletList : DataDynamics.ActiveReports.ActiveReport
    {

        public RepPalletList()
        {
            //
            // ActiveReport デザイナ サポートに必要です。
            //
            InitializeComponent();
        }

        private void RepPalletList_ReportStart(object sender, EventArgs e)
        {
            this.lblDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            DataSet ds = this.DataSource as DataSet;
            if (ds != null && ds.Tables.Contains(Def_T_PALLETLIST_MANAGE.Name))
            {
                lblPalletNo.Text = ComFunc.GetFld(ds, Def_T_PALLETLIST_MANAGE.Name, 0, Def_T_SHUKKA_MEISAI.PALLET_NO, "");
                lblNonyusaki.Text = ComFunc.GetFld(ds, Def_T_PALLETLIST_MANAGE.Name, 0, Def_M_NONYUSAKI.NONYUSAKI_NAME, "");
                lblShip.Text = ComFunc.GetFld(ds, Def_T_PALLETLIST_MANAGE.Name, 0, Def_M_NONYUSAKI.SHIP, "");

                bcdPalletNo.Text = ComFunc.GetFld(ds, Def_T_PALLETLIST_MANAGE.Name, 0, Def_T_SHUKKA_MEISAI.PALLET_NO, "");
                txtNum.Text = ComFunc.GetFld(ds, Def_T_PALLETLIST_MANAGE.Name, 0, "COUNT_BOX", "");
            }

            if (ds != null && ds.Tables.Contains(Def_T_PALLETLIST_MANAGE.Name))
            {
                double dFontSize_Nonyusaki = 21.75;

                // 納入先
                for (int i = 0; ComFunc.GetFld(ds, Def_T_PALLETLIST_MANAGE.Name, i, Def_M_NONYUSAKI.NONYUSAKI_NAME, "") != ""; i = i + 2)
                {
                    string bytecheck = ComFunc.GetFld(ds, Def_T_PALLETLIST_MANAGE.Name, i, Def_M_NONYUSAKI.NONYUSAKI_NAME, "");

                    byte[] byteOfString = System.Text.Encoding.Default.GetBytes(bytecheck);

                    if (byteOfString.Length > 55 && dFontSize_Nonyusaki > 14.25)
                    {
                        dFontSize_Nonyusaki = 14.25;
                    }
                    else if (byteOfString.Length > 48 && dFontSize_Nonyusaki > 15.75)
                    {
                        dFontSize_Nonyusaki = 15.75;
                    }
                    else if (byteOfString.Length > 42 && dFontSize_Nonyusaki > 18)
                    {
                        dFontSize_Nonyusaki = 18;
                    }
                    else if (byteOfString.Length > 40 && dFontSize_Nonyusaki > 20.25)
                    {
                        dFontSize_Nonyusaki = 20.25;
                    }

                    float fontSize = (float)dFontSize_Nonyusaki;
                    Font ft = new Font(this.lblNonyusaki.Font.Name, fontSize);
                    this.lblNonyusaki.Font = ft;
                }

            }

        }
    }
}
