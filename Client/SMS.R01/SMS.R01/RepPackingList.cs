using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Document;

using Commons;
using System.Globalization;

namespace SMS.R01
{
    /// <summary>
    /// RepPackingList の概要の説明です。
    /// </summary>
    public partial class RepPackingList : DataDynamics.ActiveReports.ActiveReport
    {
        #region 定数
        // 単位１
        private const string PCE = "PCE";
        // 単位複数
        private const string PCS = "PCS";
        // 重量単位
        private const string kg = "kg";
        #endregion

        public RepPackingList()
        {
            //
            // ActiveReport デザイナ サポートに必要です。
            //
            InitializeComponent();
        }

        private void RepPackingList_ReportStart(object sender, EventArgs e)
        {
            // 2012/07/04 K.Tsutsumi Delete
            //this.lblDate.Text = DateTime.Now.ToString("dd-MMM-yyyy", DateTimeFormatInfo.InvariantInfo);
            // 

            DataSet ds = this.DataSource as DataSet;
            if (ds != null && ds.Tables.Contains(Def_T_KIWAKU.Name))
            {
                DataRow dr = ds.Tables[Def_T_KIWAKU.Name].Rows[0];
                if (!string.IsNullOrEmpty(dr[ComDefine.FLD_CASE_MARK].ToString()))
                {
                    Bitmap bmp = new Bitmap(dr[ComDefine.FLD_CASE_MARK] as Image);
                    caseMark.Image = bmp;
                }
            }

            if (ds != null && ds.Tables.Contains(Def_T_SHUKKA_MEISAI.Name))
            {
                bool isExistence = false;
                foreach (DataRow dr in ds.Tables[Def_T_SHUKKA_MEISAI.Name].Rows)
                {
                    if (!string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.GRWT)))
                    {
                        isExistence = true;
                    }
                }

                if (!isExistence)
                {
                    label4.Text = "";
                }
            }
        }

        private void detail_Format(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lblMno.Text))
            {
                lblKishu1.Visible = true;
                lblKishu2.Visible = true;
            }
            else
            {
                lblKishu1.Visible = false;
                lblKishu2.Visible = false;
            }

           if (!string.IsNullOrEmpty(txtGRWT.Text))
           {
                lblkg.Text = kg;
            }
            else
            {
                lblkg.Text = "";
            }


            if (txtNum.Text == "1")
            {
                lblPce.Text = PCE;
            }
            else if (string.IsNullOrEmpty(txtNum.Text))
            {
                if (lblTagNo.Text == null)
                {
                    lblEnd.Visible = true;
                }
                else
                {
                    lblEnd.Visible = false;
                }
                                
                lblPce.Text = "";
                
            }
            else
            {
                lblPce.Text = PCS;
            }


            // 仮のPageを生成します。
            Page MeasurePage = new Page();

            double dFontSize_M_NO = 11.25;
            double dFontSize_Kishu = 11.25;
            double dFontSize_Hinmei = 9.75;
            double dFontSize_Zumen = 9.75;
            double dFontSize_GRWT = 11.25;
            //double dFontSize_Floor = 9.75;

            // M_NO
            if (!string.IsNullOrEmpty(this.lblMno.Text))
            {
                byte[] byteOfString = System.Text.Encoding.Default.GetBytes(this.lblMno.Text);

                if (byteOfString.Length > 36)
                {
                    dFontSize_M_NO = 9;
                }
                else if (byteOfString.Length > 32)
                {
                    dFontSize_M_NO = 9.75;
                }

                float fontSize = (float)dFontSize_M_NO;
                Font ft = new Font(this.lblMno.Font.Name, fontSize);
                this.lblMno.Font = ft;
            }

            // 機種
            if (!string.IsNullOrEmpty(this.lblKishu1.Text))
            {
                byte[] byteOfString = System.Text.Encoding.Default.GetBytes(this.lblKishu1.Text);

                if (byteOfString.Length > 36)
                {
                    dFontSize_Kishu = 9;
                }
                else if (byteOfString.Length > 32)
                {
                    dFontSize_Kishu = 9.75;
                }

                float fontSize = (float)dFontSize_Kishu;
                Font ft = new Font(this.lblKishu1.Font.Name, fontSize);
                this.lblKishu1.Font = ft;
            }

            // 品名
            if (!string.IsNullOrEmpty(this.txtHinmei.Text))
            {
                byte[] byteOfString = System.Text.Encoding.Default.GetBytes(this.txtHinmei.Text);
                if (byteOfString.Length > 39)
                {
                    dFontSize_Hinmei = 6.75;
                }
                else if (byteOfString.Length > 36)
                {
                    dFontSize_Hinmei = 8.25;
                }
                else if (byteOfString.Length > 33)
                {
                    dFontSize_Hinmei = 9;
                }

                float fontSize = (float)dFontSize_Hinmei;
                Font ft = new Font(this.txtHinmei.Font.Name, fontSize);
                this.txtHinmei.Font = ft;
            }

            // 図面
            if (!string.IsNullOrEmpty(this.txtZumen.Text))
            {
                byte[] byteOfString = System.Text.Encoding.Default.GetBytes(this.txtZumen.Text);
                if (byteOfString.Length > 39)
                {
                    dFontSize_Zumen = 6.75;
                }
                else if (byteOfString.Length > 36)
                {
                    dFontSize_Zumen = 8.25;
                }
                else if (byteOfString.Length > 33)
                {
                    dFontSize_Zumen = 9;
                }

                float fontSize = (float)dFontSize_Zumen;
                Font ft = new Font(this.txtZumen.Font.Name, fontSize);
                this.txtZumen.Font = ft;
            }

            // Floor
            //if (!string.IsNullOrEmpty(this.txtFloor.Text))
            //{
            //    byte[] byteOfString = System.Text.Encoding.Default.GetBytes(this.txtFloor.Text);
            //    if (byteOfString.Length > 18)
            //    {
            //        dFontSize_Floor = 6.75;
            //    }
            //    else if (byteOfString.Length > 13)
            //    {
            //        dFontSize_Floor = 8.25;
            //    }
            //    else if (byteOfString.Length > 12)
            //    {
            //        dFontSize_Floor = 9;
            //    }

            //    float fontSize = (float)dFontSize_Floor;
            //    Font ft = new Font(this.txtFloor.Font.Name, fontSize);
            //    this.txtFloor.Font = ft;
            //}

            // 重量
            if (!string.IsNullOrEmpty(this.txtGRWT.Text))
            {
                byte[] byteOfString = System.Text.Encoding.Default.GetBytes(this.txtGRWT.Text);
                if (byteOfString.Length > 6)
                {
                    dFontSize_GRWT = 8.25;
                }


                float fontSize = (float)dFontSize_GRWT;
                Font ft = new Font(this.txtGRWT.Font.Name, fontSize);
                this.txtGRWT.Font = ft;
            }
        }

    }
}
