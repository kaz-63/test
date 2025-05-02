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
    /// RepKonpoMeisaisho の概要の説明です。
    /// </summary>
    public partial class RepKonpoMeisaisho : DataDynamics.ActiveReports.ActiveReport
    {

        public RepKonpoMeisaisho()
        {
            //
            // ActiveReport デザイナ サポートに必要です。
            //
            InitializeComponent();
        }

        private void RepKonpoMeisaisho_ReportStart(object sender, EventArgs e)
        {
            DataSet ds = this.DataSource as DataSet;
            if (ds != null && ds.Tables.Contains(Def_T_KIWAKU.Name))
            {
                DataRow dr = ds.Tables[Def_T_KIWAKU.Name].Rows[0];

                lblKojiName.Text = ComFunc.GetFld(dr, Def_T_KIWAKU.KOJI_NAME);
                lblShip.Text = ComFunc.GetFld(dr, Def_T_KIWAKU.SHIP);
                lblDeliveryDate.Text = ComFunc.GetFld(dr, Def_T_KIWAKU.DELIVERY_DATE);
                lblDeliveryNo.Text = ComFunc.GetFld(dr, Def_T_KIWAKU.DELIVERY_NO);
                lblDeliveryPoint.Text = ComFunc.GetFld(dr, Def_T_KIWAKU.DELIVERY_POINT);
                lblPortDestination.Text = ComFunc.GetFld(dr, Def_T_KIWAKU.PORT_OF_DESTINATION);
                lblAirBoat.Text = ComFunc.GetFld(dr, Def_T_KIWAKU.AIR_BOAT);
                lblFactory.Text = ComFunc.GetFld(dr, Def_T_KIWAKU.FACTORY);
                lblRemarks.Text = ComFunc.GetFld(dr, Def_T_KIWAKU.REMARKS);

                if (!string.IsNullOrEmpty(dr[ComDefine.FLD_CASE_MARK].ToString()))
                {
                    Bitmap bmp = new Bitmap(dr[ComDefine.FLD_CASE_MARK] as Image);
                    caseMark.Image = bmp;
                }

            }

            if (ds != null && ds.Tables.Contains(Def_T_KIWAKU.Name))
            {
                double dFontSize_KojiName = 14.25;

                // 工事No
                for (int i = 0; ComFunc.GetFld(ds, Def_T_KIWAKU.Name, i, Def_T_KIWAKU.KOJI_NAME, "") != ""; i = i + 2)
                {
                    string bytecheck = ComFunc.GetFld(ds, Def_T_KIWAKU.Name, i, Def_T_KIWAKU.KOJI_NAME, "");

                    byte[] byteOfString = System.Text.Encoding.Default.GetBytes(bytecheck);

                    if (byteOfString.Length > 52 && dFontSize_KojiName > 9.75)
                    {
                        dFontSize_KojiName = 9.75;
                    }
                    else if (byteOfString.Length > 48 && dFontSize_KojiName > 11.25)
                    {
                        dFontSize_KojiName = 11.25;
                    }
                    else if (byteOfString.Length > 40 && dFontSize_KojiName > 12)
                    {
                        dFontSize_KojiName = 12;
                    }

                    float fontSize = (float)dFontSize_KojiName;
                    Font ft = new Font(this.lblKojiName.Font.Name, fontSize);
                    this.lblKojiName.Font = ft;
                }

            }
        }

        private void detail_Format(object sender, EventArgs e)
        {
            // 仮のPageを生成します。
            Page MeasurePage = new Page();

            double dFontSize_InvName = 8.25;
            double dFontSize_Description2 = 8.25;

            // Inv付加名
            if (!string.IsNullOrEmpty(this.label30.Text))
            {
                byte[] byteOfString = System.Text.Encoding.Default.GetBytes(this.label30.Text);
                if (byteOfString.Length > 36)
                {
                    dFontSize_InvName = 6;
                }
                else if (byteOfString.Length > 30)
                {
                    dFontSize_InvName = 6.75;
                }

                float fontSize = (float)dFontSize_InvName;
                Font ft = new Font(this.label30.Font.Name, fontSize);
                this.label30.Font = ft;
            }

            // DESCRIPTION_2
            if (!string.IsNullOrEmpty(this.label31.Text))
            {
                byte[] byteOfString = System.Text.Encoding.Default.GetBytes(this.label31.Text);
                if (byteOfString.Length > 85)
                {
                    dFontSize_Description2 = 5.25;
                }
                else if (byteOfString.Length > 36)
                {
                    dFontSize_Description2 = 6;
                }
                else if (byteOfString.Length > 30)
                {
                    dFontSize_Description2 = 6.75;
                }

                float fontSize = (float)dFontSize_Description2;
                Font ft = new Font(this.label31.Font.Name, fontSize);
                this.label31.Font = ft;
            }
        }

        private void groupFooter1_BeforePrint(object sender, EventArgs e)
        {
            double dFontSize_NetW = 8.25;
            double dFontSize_GrossW = 8.25;

            // NET_W
            if (!string.IsNullOrEmpty(this.textBox19.Text))
            {
                byte[] byteOfString = System.Text.Encoding.Default.GetBytes(this.textBox19.Text);
                if (byteOfString.Length > 9)
                {
                    dFontSize_NetW = 6;
                }
                else if (byteOfString.Length > 8)
                {
                    dFontSize_NetW = 6.75;
                }

                float fontSize = (float)dFontSize_NetW;
                Font ft = new Font(this.textBox19.Font.Name, fontSize);
                this.textBox19.Font = ft;
            }

            // GROSS_W
            if (!string.IsNullOrEmpty(this.textBox18.Text))
            {
                byte[] byteOfString = System.Text.Encoding.Default.GetBytes(this.textBox18.Text);
                if (byteOfString.Length > 10)
                {
                    dFontSize_GrossW = 6;
                }
                else if (byteOfString.Length > 9)
                {
                    dFontSize_GrossW = 6.75;
                }

                float fontSize = (float)dFontSize_GrossW;
                Font ft = new Font(this.textBox18.Font.Name, fontSize);
                this.textBox18.Font = ft;
            }
        }
    }
}
