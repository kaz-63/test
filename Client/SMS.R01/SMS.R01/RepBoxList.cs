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
    /// RepBoxList の概要の説明です。
    /// </summary>
    public partial class RepBoxList : DataDynamics.ActiveReports.ActiveReport
    {

        public RepBoxList()
        {
            //
            // ActiveReport デザイナ サポートに必要です。
            //
            InitializeComponent();
        }

        private void RepBoxList_ReportStart(object sender, EventArgs e)
        {
            try
            {
                this.lblDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
                DataSet ds = this.DataSource as DataSet;
                if (ds != null && ds.Tables.Contains(Def_T_BOXLIST_MANAGE.Name))
                {
                    lblBoxNo.Text = ComFunc.GetFld(ds, Def_T_BOXLIST_MANAGE.Name, 0, Def_T_SHUKKA_MEISAI.BOX_NO, "");
                    lblNonyusaki.Text = ComFunc.GetFld(ds, Def_T_BOXLIST_MANAGE.Name, 0, Def_M_NONYUSAKI.NONYUSAKI_NAME, "");
                    lblShip.Text = ComFunc.GetFld(ds, Def_T_BOXLIST_MANAGE.Name, 0, Def_M_NONYUSAKI.SHIP, "");
                    lblArea.Text = ComFunc.GetFld(ds, Def_T_BOXLIST_MANAGE.Name, 0, Def_T_SHUKKA_MEISAI.AREA, "");
                    lblFloor.Text = ComFunc.GetFld(ds, Def_T_BOXLIST_MANAGE.Name, 0, Def_T_SHUKKA_MEISAI.FLOOR, "");
                    bcdBoxNo.Text = ComFunc.GetFld(ds, Def_T_BOXLIST_MANAGE.Name, 0, Def_T_SHUKKA_MEISAI.BOX_NO, "");

                }


                if (ds != null && ds.Tables.Contains(Def_T_BOXLIST_MANAGE.Name))
                {
                    double dFontSize_Nonyusaki = 20.25;


                    // 納入先
                    for (int i = 0; ComFunc.GetFld(ds, Def_T_BOXLIST_MANAGE.Name, i, Def_M_NONYUSAKI.NONYUSAKI_NAME, "") != ""; i = i + 2)
                    {
                        string bytecheck = ComFunc.GetFld(ds, Def_T_BOXLIST_MANAGE.Name, i, Def_M_NONYUSAKI.NONYUSAKI_NAME, "");

                        byte[] byteOfString = System.Text.Encoding.Default.GetBytes(bytecheck);

                        if (byteOfString.Length > 52 && dFontSize_Nonyusaki > 15.75)
                        {
                            dFontSize_Nonyusaki = 15.75;
                        }
                        else if (byteOfString.Length > 46 && dFontSize_Nonyusaki > 18)
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
            catch (Exception) { }
        }

        private void detail_Format(object sender, EventArgs e)
        {
            // 仮のPageを生成します。
            Page MeasurePage = new Page();

            double dFontSize_Kishu = 9.75;
            double dFontSize_Mno = 9.75;
            double dFontSize_Hinmeijp = 9;
            double dFontSize_Hinmei = 9;
            double dFontSize_Zumenkeishiki = 9.75;


            // 機種
            if (!string.IsNullOrEmpty(this.txtKishu.Text))
            {
                byte[] byteOfString = System.Text.Encoding.Default.GetBytes(this.txtKishu.Text);
                if (byteOfString.Length > 29)
                {
                    dFontSize_Kishu = 5.25;
                }
                else if (byteOfString.Length > 26)
                {
                    dFontSize_Kishu = 6;
                }
                else if (byteOfString.Length > 21)
                {
                    dFontSize_Kishu = 6.75;
                }
                else if (byteOfString.Length > 19)
                {
                    dFontSize_Kishu = 8.25;
                }
                else if (byteOfString.Length > 17)
                {
                    dFontSize_Kishu = 9;
                }

                float fontSize = (float)dFontSize_Kishu;
                Font ft = new Font(this.txtKishu.Font.Name, fontSize);
                this.txtKishu.Font = ft;
            }

            // M_NO
            if (!string.IsNullOrEmpty(this.textBox5.Text))
            {
                byte[] byteOfString = System.Text.Encoding.Default.GetBytes(this.textBox5.Text);
                if (byteOfString.Length > 37)
                {
                    dFontSize_Mno = 6.75;
                }
                else if (byteOfString.Length > 33)
                {
                    dFontSize_Mno = 8.25;
                }
                else if (byteOfString.Length > 31)
                {
                    dFontSize_Mno = 9;
                }

                float fontSize = (float)dFontSize_Mno;
                Font ft = new Font(this.textBox5.Font.Name, fontSize);
                this.textBox5.Font = ft;
            }

            // 品名（和文）
            if (!string.IsNullOrEmpty(this.txtHinmeiJ.Text))
            {
                byte[] byteOfString = System.Text.Encoding.Default.GetBytes(this.txtHinmeiJ.Text);
                if (byteOfString.Length > 73)
                {
                    dFontSize_Hinmeijp = 3.75;
                }
                else if (byteOfString.Length > 63)
                {
                    dFontSize_Hinmeijp = 5.25;
                }
                else if (byteOfString.Length > 55)
                {
                    dFontSize_Hinmeijp = 6;
                }
                else if (byteOfString.Length > 45)
                {
                    dFontSize_Hinmeijp = 6.75;
                }
                else if (byteOfString.Length > 41)
                {
                    dFontSize_Hinmeijp = 8.25;
                }

                float fontSize = (float)dFontSize_Hinmeijp;
                Font ft = new Font(this.txtHinmeiJ.Font.Name, fontSize);
                this.txtHinmeiJ.Font = ft;
            }

            // 品名
            if (!string.IsNullOrEmpty(this.txtHinmei.Text))
            {
                byte[] byteOfString = System.Text.Encoding.Default.GetBytes(this.txtHinmei.Text);
                if (byteOfString.Length > 73)
                {
                    dFontSize_Hinmei = 3.75;
                }
                else if (byteOfString.Length > 63)
                {
                    dFontSize_Hinmei = 5.25;
                }
                else if (byteOfString.Length > 55)
                {
                    dFontSize_Hinmei = 6;
                }
                else if (byteOfString.Length > 45)
                {
                    dFontSize_Hinmei = 6.75;
                }
                else if (byteOfString.Length > 41)
                {
                    dFontSize_Hinmei = 8.25;
                }

                float fontSize = (float)dFontSize_Hinmei;
                Font ft = new Font(this.txtHinmei.Font.Name, fontSize);
                this.txtHinmei.Font = ft;
            }

            // 図番型式
            if (!string.IsNullOrEmpty(this.txtKeishiki.Text))
            {
                byte[] byteOfString = System.Text.Encoding.Default.GetBytes(this.txtKeishiki.Text);
                if (byteOfString.Length > 81)
                {
                    dFontSize_Zumenkeishiki = 6.75;
                }
                else if (byteOfString.Length > 73)
                {
                    dFontSize_Zumenkeishiki = 8.25;
                }

                float fontSize = (float)dFontSize_Zumenkeishiki;
                Font ft = new Font(this.txtKeishiki.Font.Name, fontSize);
                this.txtKeishiki.Font = ft;
            }

        }
    }
}
