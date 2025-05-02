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
    /// RepGenpinTag の概要の説明です。
    /// </summary>
    public partial class RepGenpinTag : DataDynamics.ActiveReports.ActiveReport
    {

        public RepGenpinTag()
        {
            //
            // ActiveReport デザイナ サポートに必要です。
            //
            InitializeComponent();
        }

        private void RepGenpinTag_ReportStart(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = this.DataSource as DataSet;
                int pos = 0;
                if (ds != null && ds.Tables.Contains(ComDefine.DTTBL_CONF))
                {
                    // 印刷開始位置の修正
                    pos = ComFunc.GetFldToInt32(ds, ComDefine.DTTBL_CONF, 0, ComDefine.FLD_POS, 0);
                    if (pos == 1)
                    {
                        groupHeader2.Visible = false;
                    }
                    groupHeader2.Height *= (pos - 1);

                }
            }
            catch (Exception) { }
        }

        private void detail_Format(object sender, EventArgs e)
        {
            // 仮のPageを生成します。
            Page MeasurePage = new Page();

            double dFontSize_Zumenkeishiki = 9.75;
            double dFontSize_Free2 = 9.75;
            double dFontSize_Floor = 9.75;
            double dFontSize_Kishu = 9.75;
            double dFontSize_Mno = 9.75;
            double dFontSize_Hinmejp = 9.75;
            double dFontSize_Hinme = 9.75;

            // 図番型式
            if (!string.IsNullOrEmpty(this.txtKeishiki.Text))
            {
                byte[] byteOfString = Encoding.Default.GetBytes(this.txtKeishiki.Text);
                if (byteOfString.Length > 40)
                {
                    dFontSize_Zumenkeishiki = 6.75;
                    this.txtKeishiki.WordWrap = true;
                }
                else if (byteOfString.Length > 38)
                {
                    dFontSize_Zumenkeishiki = 8.25;
                }
                else if (byteOfString.Length > 34)
                {
                    dFontSize_Zumenkeishiki = 9;
                }

                float fontSize = (float)dFontSize_Zumenkeishiki;
                Font ft = new Font(this.txtKeishiki.Font.Name, fontSize);
                this.txtKeishiki.Font = ft;
            }

            // 原産国
            if (!string.IsNullOrEmpty(this.textBox1.Text))
            {
                byte[] byteOfString = Encoding.Default.GetBytes(this.textBox1.Text);
                if (byteOfString.Length > 26)
                {
                    dFontSize_Free2 = 6;
                }
                else if (byteOfString.Length > 21)
                {
                    dFontSize_Free2 = 6.75;
                }
                else if (byteOfString.Length > 18)
                {
                    dFontSize_Free2 = 8.25;
                }

                float fontSize = (float)dFontSize_Free2;
                Font ft = new Font(this.textBox1.Font.Name, fontSize);
                this.textBox1.Font = ft;
            }

            // Floor
            if (!string.IsNullOrEmpty(this.txtFloor.Text))
            {
                byte[] byteOfString = Encoding.Default.GetBytes(this.txtFloor.Text);
                if (byteOfString.Length > 18)
                {
                    dFontSize_Floor = 6;
                }
                else if (byteOfString.Length > 15)
                {
                    dFontSize_Floor = 6.75;
                }
                else if (byteOfString.Length > 13)
                {
                    dFontSize_Floor = 8.25;
                }
                else if (byteOfString.Length > 12)
                {
                    dFontSize_Floor = 9;
                }

                float fontSize = (float)dFontSize_Floor;
                Font ft = new Font(this.txtFloor.Font.Name, fontSize);
                this.txtFloor.Font = ft;
            }

            // 機種（Type）
            if (!string.IsNullOrEmpty(this.txtKishu.Text))
            {
                byte[] byteOfString = Encoding.Default.GetBytes(this.txtKishu.Text);
                if (byteOfString.Length > 37)
                {
                    dFontSize_Kishu = 8.25;
                }
                else if (byteOfString.Length > 34)
                {
                    dFontSize_Kishu = 9;
                }

                float fontSize = (float)dFontSize_Kishu;
                Font ft = new Font(this.txtKishu.Font.Name, fontSize);
                this.txtKishu.Font = ft;
            }

            // M-No
            if (!string.IsNullOrEmpty(this.txtStNo.Text))
            {
                byte[] byteOfString = Encoding.Default.GetBytes(this.txtStNo.Text);
                if (byteOfString.Length > 37)
                {
                    dFontSize_Mno = 6;
                }
                else if (byteOfString.Length > 30)
                {
                    dFontSize_Mno = 6.75;
                }
                else if (byteOfString.Length > 27)
                {
                    dFontSize_Mno = 8.25;
                }
                else if (byteOfString.Length > 25)
                {
                    dFontSize_Mno = 9;
                }

                float fontSize = (float)dFontSize_Mno;
                Font ft = new Font(this.txtStNo.Font.Name, fontSize);
                this.txtStNo.Font = ft;
            }

            // 品名（和文）
            if (!string.IsNullOrEmpty(this.txtHinmeiJ.Text))
            {
                byte[] byteOfString = Encoding.Default.GetBytes(this.txtHinmeiJ.Text);
                if (byteOfString.Length > 51)
                {
                    dFontSize_Hinmejp = 6.75;
                    this.txtHinmeiJ.WordWrap = true;
                }
                else if (byteOfString.Length > 47)
                {
                    dFontSize_Hinmejp = 8.25;
                }
                else if (byteOfString.Length > 43)
                {
                    dFontSize_Hinmejp = 9;
                }

                float fontSize = (float)dFontSize_Hinmejp;
                Font ft = new Font(this.txtHinmeiJ.Font.Name, fontSize);
                this.txtHinmeiJ.Font = ft;
            }

            // 品名
            if (!string.IsNullOrEmpty(this.txtHinmei.Text))
            {
                byte[] byteOfString = Encoding.Default.GetBytes(this.txtHinmei.Text);
                if (byteOfString.Length > 51)
                {
                    dFontSize_Hinme = 6.75;
                    this.txtHinmei.WordWrap = true;
                }
                else if (byteOfString.Length > 47)
                {
                    dFontSize_Hinme = 8.25;
                }
                else if (byteOfString.Length > 43)
                {
                    dFontSize_Hinme = 9;
                }

                float fontSize = (float)dFontSize_Hinme;
                Font ft = new Font(this.txtHinmei.Font.Name, fontSize);
                this.txtHinmei.Font = ft;
            }
            
        }
    }
}
