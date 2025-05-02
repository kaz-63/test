using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Commons;
using SystemBase.Excel;
using SMS.E01.Properties;

namespace SMS.E01
{
    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細Excel出力クラス
    /// </summary>
    /// <create>J.Chen 2023/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ExportShukkaKeikakuShokai : BaseExportXlsx
    {
        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>J.Chen 2023/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public ExportShukkaKeikakuShokai()
            : base()
        {

        }

        #endregion

        #region 出荷明細出力

        /// --------------------------------------------------
        /// <summary>
        /// 出荷明細Excelの出力
        /// </summary>
        /// <param name="filePath">ファイル名</param>
        /// <param name="dt">出力データ</param>
        /// <param name="msgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>J.Chen 2023/08/30</create>
        /// <update>J.Chen 2024/10/22 現場用データ出力追加</update>
        /// <update></update>
        /// --------------------------------------------------
        public bool ExportExcel(string filePath, System.Data.DataTable dt, out string msgID, out string[] args)
        {
            msgID = string.Empty;
            args = null;
            try
            {
                string colName = string.Empty;
                string colCaption = string.Empty;
                string dateTimeFormat = "yyyy/mm/dd";
                ExportInfoCollection expInfoCollection = new ExportInfoCollection();
                // 物件名
                colName = Def_M_BUKKEN.BUKKEN_NAME;
                colCaption = Resources.ExportShukkaKeikakuShokai_BukkenName;
                expInfoCollection.Add(colName, colCaption);
                // 運賃・梱包製番
                colName = Def_M_NONYUSAKI.SHIP_SEIBAN;
                colCaption = Resources.ExportShukkaKeikakuShokai_ShipSeiban;
                expInfoCollection.Add(colName, colCaption);
                // 便名
                colName = Def_M_NONYUSAKI.SHIP;
                colCaption = Resources.ExportShukkaKeikakuShokai_Ship;
                expInfoCollection.Add(colName, colCaption);
                // 有償/無償
                colName = ComDefine.FLD_ESTIMATE_FLAG_NAME;
                colCaption = Resources.ExportShukkaKeikakuShokai_EstimateFlag;
                expInfoCollection.Add(colName, colCaption);
                // AIR/SHIP
                colName = Def_M_NONYUSAKI.TRANSPORT_FLAG;
                colCaption = Resources.ExportShukkaKeikakuShokai_TransportFlag;
                expInfoCollection.Add(colName, colCaption);
                // 出荷元
                colName = Def_M_NONYUSAKI.SHIP_FROM;
                colCaption = Resources.ExportShukkaKeikakuShokai_ShukkaFrom;
                expInfoCollection.Add(colName, colCaption);
                // 出荷先
                colName = Def_M_NONYUSAKI.SHIP_TO;
                colCaption = Resources.ExportShukkaKeikakuShokai_ShukkaTo;
                expInfoCollection.Add(colName, colCaption);
                // 出荷日
                colName = Def_M_NONYUSAKI.SHIP_DATE;
                colCaption = Resources.ExportShukkaKeikakuShokai_ShukkaDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 製番
                colName = Def_M_NONYUSAKI.SEIBAN;
                colCaption = Resources.ExportShukkaKeikakuShokai_Seiban;
                expInfoCollection.Add(colName, colCaption);
                // 機種
                colName = Def_T_SHUKKA_MEISAI.KISHU;
                colCaption = Resources.ExportShukkaKeikakuShokai_KiShu;
                expInfoCollection.Add(colName, colCaption);
                // 内容
                colName = Def_M_NONYUSAKI.NAIYO;
                colCaption = Resources.ExportShukkaKeikakuShokai_Naiyo;
                expInfoCollection.Add(colName, colCaption);
                // 到着予定日
                colName = Def_M_NONYUSAKI.TOUCHAKUYOTEI_DATE;
                colCaption = Resources.ExportShukkaKeikakuShokai_TouchakuyoteiDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 機械Parts
                colName = Def_M_NONYUSAKI.KIKAI_PARTS;
                colCaption = Resources.ExportShukkaKeikakuShokai_KikaiParts;
                expInfoCollection.Add(colName, colCaption);
                // 制御Parts
                colName = Def_M_NONYUSAKI.SEIGYO_PARTS;
                colCaption = Resources.ExportShukkaKeikakuShokai_SeigyoParts;
                expInfoCollection.Add(colName, colCaption);
                // 備考
                colName = Def_M_NONYUSAKI.BIKO;
                colCaption = Resources.ExportShukkaKeikakuShokai_Biko;
                expInfoCollection.Add(colName, colCaption);
                // TAG発行状況
                colName = ComDefine.FLD_TAG_NUM;
                colCaption = Resources.ExportShukkaKeikakuShokai_TagNum;
                expInfoCollection.Add(colName, colCaption);
                // ステータス
                colName = ComDefine.FLD_TAG_STATUS;
                colCaption = Resources.ExportShukkaKeikakuShokai_TagStatus;
                expInfoCollection.Add(colName, colCaption);
                // 現場用ステータス（状態）
                colName = ComDefine.FLD_GENBA_YO_STATUS_NAME;
                colCaption = Resources.ExportShukkaKeikakuShokai_GenbayoStatus;
                expInfoCollection.Add(colName, colCaption);
                // 物量
                colName = Def_M_NONYUSAKI.GENBA_YO_BUTSURYO;
                colCaption = Resources.ExportShukkaKeikakuShokai_GenbayoButsuryo;
                expInfoCollection.Add(colName, colCaption);
                // ↑
                bool ret = this.ExportExcel(filePath, dt, expInfoCollection, out msgID, out args);
                if (ret)
                {
                    // 出荷計画Excelファイルを出力しました。
                    msgID = "S0100010020";
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
