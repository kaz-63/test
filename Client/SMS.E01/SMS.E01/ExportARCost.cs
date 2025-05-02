using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Commons;
using SystemBase.Excel;
using SMS.E01.Properties;

namespace SMS.E01
{
    /// --------------------------------------------------
    /// <summary>
    /// AR対応費用Excel出力クラス
    /// </summary>
    /// <create>H.Tajimi 2018/10/16</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ExportARCost : BaseExport
    {
        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>H.Tajimi 2018/10/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public ExportARCost()
            : base()
        {
        }

        #endregion

        #region AR対応費用出力

        /// --------------------------------------------------
        /// <summary>
        /// AR対応費用出力
        /// </summary>
        /// <param name="filePath">出力先パス</param>
        /// <param name="dt">出力データ</param>
        /// <param name="msgID">エラーメッセージ</param>
        /// <param name="args">パラメータ</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/10/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool ExportExcel(string filePath, DataTable dt, out string msgID, out string[] args)
        {
            msgID = string.Empty;
            args = null;
            try
            {
                var info = new ExportInfoCollection();

                // 納入先
                info.Add(Def_M_NONYUSAKI.NONYUSAKI_NAME, Resources.ExportAllARJoho_DeliveryDestination);
                // ARNO
                info.Add(Def_T_AR_COST.AR_NO, Resources.ExportAllARJoho_ARNo);
                // 発生原因
                info.Add(Def_T_AR.HASSEI_YOUIN, Resources.ExportAllARCost_HasseiYouin);
                // 行No
                info.Add(Def_T_AR_COST.LINE_NO, Resources.ExportAllARCost_LineNo);
                // 項目
                info.Add(Def_T_AR_COST.ITEM_CD, Resources.ExportAllARCost_Item);
                // 作業時間/台
                info.Add(Def_T_AR_COST.WORK_TIME, Resources.ExportAllARCost_WorkTime);
                // 作業人員
                info.Add(Def_T_AR_COST.WORKERS, Resources.ExportAllARCost_Workers);
                // 対象台数
                info.Add(Def_T_AR_COST.NUMBER, Resources.ExportAllARCost_Number);
                // 単価
                info.Add(Def_T_AR_COST.RATE, Resources.ExportAllARCost_Rate);
                // 合計
                info.Add(Def_T_AR_COST.TOTAL, Resources.ExportAllARCost_Total);

                bool ret = this.ExportExcel(filePath, dt, info, out msgID, out args);
                if (ret)
                {
                    // AR対応費用Excelファイルを出力しました。
                    msgID = "A0100010013";
                    args = null;
                }
                return ret;
            }
            catch (Exception)
            {
                if (string.IsNullOrEmpty(msgID))
                {
                    // Excel出力に失敗しました。
                    msgID = "A7777777001";
                }
                return false;
            }
        }

        #endregion
    }
}
