using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Document;
using Commons;
using System.Text;

namespace SMS.R01
{
    /// <summary>
    /// RepBoxTagList の概要の説明です。
    /// </summary>
    public partial class RepBoxTagList : DataDynamics.ActiveReports.ActiveReport
    {

        public RepBoxTagList()
        {
            //
            // ActiveReport デザイナ サポートに必要です。
            //
            InitializeComponent();
        }

        private void RepBoxTagList_ReportStart(object sender, EventArgs e)
        {
            DataTable dt = this.DataSource as DataTable;

            if (dt != null && dt.TableName == Def_T_BOXLIST_MANAGE.Name)
            {
                double byteOfEcsno = 0;
                double byteOfKeishiki = 0;
                double byteOfFloor = 0;
                double byteOfNonyusaki = 0;
                double byteOfMNo = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    // M_NO
                    if (Encoding.GetEncoding("Shift-JIS").GetByteCount(ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.M_NO)) > byteOfMNo)
                    {
                        byteOfMNo = Encoding.GetEncoding("Shift-JIS").GetByteCount(ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.M_NO));
                    }

                    // 型式
                    if (Encoding.GetEncoding("Shift-JIS").GetByteCount(ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.KISHU)) > byteOfKeishiki)
                    {
                        byteOfKeishiki = Encoding.GetEncoding("Shift-JIS").GetByteCount(ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.KISHU));
                    }

                    // 納入先
                    if (Encoding.GetEncoding("Shift-JIS").GetByteCount(ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_NAME)) > byteOfNonyusaki)
                    {
                        byteOfNonyusaki = Encoding.GetEncoding("Shift-JIS").GetByteCount(ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_NAME));
                    }

                    // ECSNo
                    if (Encoding.GetEncoding("Shift-JIS").GetByteCount(ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.AREA)) > byteOfEcsno)
                    {
                        byteOfEcsno = Encoding.GetEncoding("Shift-JIS").GetByteCount(ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.AREA));
                    }

                    // Floor
                    if (Encoding.GetEncoding("Shift-JIS").GetByteCount(ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.FLOOR)) > byteOfFloor)
                    {
                        byteOfFloor = Encoding.GetEncoding("Shift-JIS").GetByteCount(ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.FLOOR));
                    }

                }

                if (byteOfEcsno > 16)
                {
                    Font ft = new Font(this.label3.Font.Name, 18f);
                    this.label3.Font = ft;
                }

                if (byteOfKeishiki > 32)
                {
                    Font ft = new Font(this.lblKishu.Font.Name, 18f);
                    this.lblKishu.Font = ft;
                }

                if (byteOfFloor > 12)
                {
                    Font ft = new Font(this.lblFloor.Font.Name, 18f);
                    this.lblFloor.Font = ft;
                }

                if (byteOfNonyusaki > 42)
                {
                    Font ft = new Font(this.lblNonyusakiName.Font.Name, 26.25f);
                    if (byteOfNonyusaki > 44)
                    {
                        ft = new Font(this.lblNonyusakiName.Font.Name, 24f);
                        if (byteOfNonyusaki > 48)
                        {
                            ft = new Font(this.lblNonyusakiName.Font.Name, 21.75f);
                            if (byteOfNonyusaki > 54)
                            {
                                ft = new Font(this.lblNonyusakiName.Font.Name, 20.25f);
                            }
                        }
                    }
                    this.lblNonyusakiName.Font = ft;
                }

                if (byteOfMNo > 22)
                {
                    Font ft = new Font(this.lblMNo.Font.Name, 26.25f);
                    if (byteOfMNo > 24)
                    {
                        ft = new Font(this.lblMNo.Font.Name, 24f);
                        if (byteOfMNo > 26)
                        {
                            ft = new Font(this.lblMNo.Font.Name, 21f);
                        }
                    }
                    this.lblMNo.Font = ft;
                }


            }

        }

    }
}
