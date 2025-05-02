using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Document;

using Commons;
using SMS.R01.Properties;

namespace SMS.R01
{
    /// <summary>
    /// RepTagList の概要の説明です。
    /// </summary>
    public partial class RepTagList : DataDynamics.ActiveReports.ActiveReport
    {

        public RepTagList()
        {
            //
            // ActiveReport デザイナ サポートに必要です。
            //
            InitializeComponent();
        }


        private void RepTagList_ReportStart(object sender, EventArgs e)
        {
            this.txtHakkoDate.Text = DateTime.Now.ToString(Resources.RepTagList_PublishDate);

            DataSet ds = this.DataSource as DataSet;

            if (ds != null && ds.Tables.Contains(ComDefine.DTTBL_TAGLIST))
            {
                double dFontSize_Nonyusaki = 24;

                // 納入先
                for (int i = 0; ComFunc.GetFld(ds, ComDefine.DTTBL_TAGLIST, i, Def_M_NONYUSAKI.NONYUSAKI_NAME, "") != ""; i = i + 2)
                {
                    string bytecheck = ComFunc.GetFld(ds, ComDefine.DTTBL_TAGLIST, i, Def_M_NONYUSAKI.NONYUSAKI_NAME, "");

                    byte[] byteOfString = System.Text.Encoding.Default.GetBytes(bytecheck);

                    if (byteOfString.Length > 52 && dFontSize_Nonyusaki > 15.75)
                    {
                        dFontSize_Nonyusaki = 15.75;
                    }
                    else if (byteOfString.Length > 46 && dFontSize_Nonyusaki > 18)
                    {
                        dFontSize_Nonyusaki = 18;
                    }
                    else if (byteOfString.Length > 44 && dFontSize_Nonyusaki > 20.25)
                    {
                        dFontSize_Nonyusaki = 20.25;
                    }
                    else if (byteOfString.Length > 40 && dFontSize_Nonyusaki > 21.75)
                    {
                        dFontSize_Nonyusaki = 21.75;
                    }

                    float fontSize = (float)dFontSize_Nonyusaki;
                    Font ft = new Font(this.lblNonyusaki.Font.Name, fontSize);
                    this.lblNonyusaki.Font = ft;
                }

            }

        }

        private void detail_Format(object sender, EventArgs e)
        {
            // 仮のPageを生成します。
            Page MeasurePage = new Page();

            double dFontSize_Area = 9;
            double dFontSize_Floor = 9;
            double dFontSize_Kishu = 9;
            double dFontSize_Mno = 9;
            double dFontSize_Hinmeijp = 9;
            double dFontSize_Hinmei = 9;
            double dFontSize_Zumenkeishiki = 9;

            // AREA
            if (!string.IsNullOrEmpty(this.txtArea.Text))
            {
                byte[] byteOfString = System.Text.Encoding.Default.GetBytes(this.txtArea.Text);
                if (byteOfString.Length > 16)
                {
                    dFontSize_Area = 6.75;
                }
                else if (byteOfString.Length > 15)
                {
                    dFontSize_Area = 8.25;
                }

                float fontSize = (float)dFontSize_Area;
                Font ft = new Font(this.txtArea.Font.Name, fontSize);
                this.txtArea.Font = ft;
            }

            // Floor
            if (!string.IsNullOrEmpty(this.txtFloor.Text))
            {
                byte[] byteOfString = System.Text.Encoding.Default.GetBytes(this.txtFloor.Text);
                if (byteOfString.Length > 16)
                {
                    dFontSize_Floor = 6.75;
                }
                else if (byteOfString.Length > 15)
                {
                    dFontSize_Floor = 8.25;
                }

                float fontSize = (float)dFontSize_Floor;
                Font ft = new Font(this.txtFloor.Font.Name, fontSize);
                this.txtFloor.Font = ft;
            }

            // 機種
            if (!string.IsNullOrEmpty(this.txtKishu.Text))
            {
                byte[] byteOfString = System.Text.Encoding.Default.GetBytes(this.txtKishu.Text);
                if (byteOfString.Length > 34)
                {
                    dFontSize_Kishu = 3.75;
                }
                else if (byteOfString.Length > 30)
                {
                    dFontSize_Kishu = 5.25;
                }
                else if (byteOfString.Length > 26)
                {
                    dFontSize_Kishu = 6;
                }
                else if (byteOfString.Length > 22)
                {
                    dFontSize_Kishu = 6.75;
                }
                else if (byteOfString.Length > 20)
                {
                    dFontSize_Kishu = 8.25;
                }

                float fontSize = (float)dFontSize_Kishu;
                Font ft = new Font(this.txtKishu.Font.Name, fontSize);
                this.txtKishu.Font = ft;
            }

            // M_NO
            if (!string.IsNullOrEmpty(this.txtStNo.Text))
            {
                byte[] byteOfString = System.Text.Encoding.Default.GetBytes(this.txtStNo.Text);
                if (byteOfString.Length > 34)
                {
                    dFontSize_Mno = 3.75;
                }
                else if (byteOfString.Length > 30)
                {
                    dFontSize_Mno = 5.25;
                }
                else if (byteOfString.Length > 26)
                {
                    dFontSize_Mno = 6;
                }
                else if (byteOfString.Length > 22)
                {
                    dFontSize_Mno = 6.75;
                }
                else if (byteOfString.Length > 20)
                {
                    dFontSize_Mno = 8.25;
                }

                float fontSize = (float)dFontSize_Mno;
                Font ft = new Font(this.txtStNo.Font.Name, fontSize);
                this.txtStNo.Font = ft;
            }

            // 品名（和文）
            if (!string.IsNullOrEmpty(this.txtHinmeiJ.Text))
            {
                byte[] byteOfString = System.Text.Encoding.Default.GetBytes(this.txtHinmeiJ.Text);
                if (byteOfString.Length > 76)
                {
                    dFontSize_Hinmeijp = 3.75;
                }
                else if (byteOfString.Length > 66)
                {
                    dFontSize_Hinmeijp = 5.25;
                }
                else if (byteOfString.Length > 58)
                {
                    dFontSize_Hinmeijp = 6;
                }
                else if (byteOfString.Length > 48)
                {
                    dFontSize_Hinmeijp = 6.75;
                }
                else if (byteOfString.Length > 44)
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
                if (byteOfString.Length > 76)
                {
                    dFontSize_Hinmei = 3.75;
                }
                else if (byteOfString.Length > 66)
                {
                    dFontSize_Hinmei = 5.25;
                }
                else if (byteOfString.Length > 58)
                {
                    dFontSize_Hinmei = 6;
                }
                else if (byteOfString.Length > 48)
                {
                    dFontSize_Hinmei = 6.75;
                }
                else if (byteOfString.Length > 44)
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
                if (byteOfString.Length > 69)
                {
                    dFontSize_Zumenkeishiki = 6.75;
                }
                else if (byteOfString.Length > 63)
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
