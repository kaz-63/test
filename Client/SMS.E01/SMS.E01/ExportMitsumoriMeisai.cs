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
    /// 見積明細Excel出力クラス
    /// </summary>
    /// <create>J.Chen 2024/02/13</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ExportMitsumoriMeisai : BaseExportXlsx
    {
        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>J.Chen 2024/02/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public ExportMitsumoriMeisai()
            : base()
        {

        }

        #endregion

        #region 見積明細出力

        /// --------------------------------------------------
        /// <summary>
        /// 見積明細Excelの出力
        /// </summary>
        /// <param name="filePath">ファイル名</param>
        /// <param name="dt">出力データ</param>
        /// <param name="msgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>J.Chen 2024/02/13</create>
        /// <update>J.Chen 2024/10/30 変更履歴追加</update>
        /// <update>J.Chen 2024/11/08 通関確認状態追加</update>
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
                ExportDataType colDecimalDataType = ExportDataType.Decimal;
                ExportInfoCollection expInfoCollection = new ExportInfoCollection();

                // 手配連携No.
                colName = Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO;
                colCaption = Resources.ExportMitsumoriMeisai_TehaiRenkeiNo;
                expInfoCollection.Add(colName, colCaption);
                // 設定納期
                colName = Def_T_TEHAI_MEISAI.SETTEI_DATE;
                colCaption = Resources.ExportMitsumoriMeisai_SetteiDate;
                expInfoCollection.Add(colName, colCaption);
                // 納品先
                colName = Def_T_TEHAI_MEISAI.NOUHIN_SAKI;
                colCaption = Resources.ExportMitsumoriMeisai_NouhinSaki;
                expInfoCollection.Add(colName, colCaption);
                // 出荷先
                colName = Def_T_TEHAI_MEISAI.SYUKKA_SAKI;
                colCaption = Resources.ExportMitsumoriMeisai_SyukkaSaki;
                expInfoCollection.Add(colName, colCaption);
                // 製番
                colName = Def_M_ECS.SEIBAN;
                colCaption = Resources.ExportMitsumoriMeisai_Seiban;
                expInfoCollection.Add(colName, colCaption);
                // CODE
                colName = Def_M_ECS.CODE;
                colCaption = Resources.ExportMitsumoriMeisai_Code;
                expInfoCollection.Add(colName, colCaption);
                // 追番
                colName = Def_T_TEHAI_MEISAI.ZUMEN_OIBAN;
                colCaption = Resources.ExportMitsumoriMeisai_ZumenOiban;
                expInfoCollection.Add(colName, colCaption);
                // AR No.
                colName = Def_M_ECS.AR_NO;
                colCaption = Resources.ExportMitsumoriMeisai_ARNo;
                expInfoCollection.Add(colName, colCaption);
                // ECS No.
                colName = Def_T_TEHAI_MEISAI.ECS_NO;
                colCaption = Resources.ExportMitsumoriMeisai_ECSNo;
                expInfoCollection.Add(colName, colCaption);
                // Floor
                colName = Def_T_TEHAI_MEISAI.FLOOR;
                colCaption = Resources.ExportMitsumoriMeisai_Floor;
                expInfoCollection.Add(colName, colCaption);
                // 機種
                colName = Def_M_ECS.KISHU;
                colCaption = Resources.ExportMitsumoriMeisai_Kishu;
                expInfoCollection.Add(colName, colCaption);
                // ST No.
                colName = Def_T_TEHAI_MEISAI.ST_NO;
                colCaption = Resources.ExportMitsumoriMeisai_STNo;
                expInfoCollection.Add(colName, colCaption);
                // 品名（和文）
                colName = Def_T_TEHAI_MEISAI.HINMEI_JP;
                colCaption = Resources.ExportMitsumoriMeisai_HinmeiJP;
                expInfoCollection.Add(colName, colCaption);
                // 品名
                colName = Def_T_TEHAI_MEISAI.HINMEI;
                colCaption = Resources.ExportMitsumoriMeisai_Hinmei;
                expInfoCollection.Add(colName, colCaption);
                // 品名（INV）
                colName = Def_T_TEHAI_MEISAI.HINMEI_INV;
                colCaption = Resources.ExportMitsumoriMeisai_HinmeiINV;
                expInfoCollection.Add(colName, colCaption);
                // 図番/型式
                colName = Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI;
                colCaption = Resources.ExportMitsumoriMeisai_Zumenkeishiki;
                expInfoCollection.Add(colName, colCaption);
                // 手配数
                colName = Def_T_TEHAI_MEISAI.TEHAI_QTY;
                colCaption = Resources.ExportMitsumoriMeisai_TehaiQty;
                expInfoCollection.Add(colName, colCaption);
                // 手配区分
                colName = ComDefine.FLD_TEHAI_FLAG_NAME;
                colCaption = Resources.ExportMitsumoriMeisai_TehaiFlagName;
                expInfoCollection.Add(colName, colCaption);
                // 発注数
                colName = Def_T_TEHAI_MEISAI.HACCHU_QTY;
                colCaption = Resources.ExportMitsumoriMeisai_HacchuQty;
                expInfoCollection.Add(colName, colCaption);
                // 出荷数
                colName = Def_T_TEHAI_MEISAI.SHUKKA_QTY;
                colCaption = Resources.ExportMitsumoriMeisai_ShukkaQty;
                expInfoCollection.Add(colName, colCaption);
                // Free1
                colName = Def_T_TEHAI_MEISAI.FREE1;
                colCaption = Resources.ExportMitsumoriMeisai_Free1;
                expInfoCollection.Add(colName, colCaption);
                // Free2
                colName = Def_T_TEHAI_MEISAI.FREE2;
                colCaption = Resources.ExportMitsumoriMeisai_Free2;
                expInfoCollection.Add(colName, colCaption);
                // 数量単位
                colName = ComDefine.FLD_QUANTITY_UNIT_NAME;
                colCaption = Resources.ExportMitsumoriMeisai_QuantityUnitName;
                expInfoCollection.Add(colName, colCaption);
                // 図番/型式2
                colName = Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI2;
                colCaption = Resources.ExportMitsumoriMeisai_ZumenKeishiki2;
                expInfoCollection.Add(colName, colCaption);
                // 備考
                colName = Def_T_TEHAI_MEISAI.NOTE;
                colCaption = Resources.ExportMitsumoriMeisai_Note;
                expInfoCollection.Add(colName, colCaption);
                // 通関確認状態
                colName = Def_T_TEHAI_MEISAI.CUSTOMS_STATUS;
                colCaption = Resources.ExportMitsumoriMeisai_CustomsStatus;
                expInfoCollection.Add(colName, colCaption);
                // Maker
                colName = Def_T_TEHAI_MEISAI.MAKER;
                colCaption = Resources.ExportMitsumoriMeisai_Maker;
                expInfoCollection.Add(colName, colCaption);
                // 手配No.
                colName = Def_T_TEHAI_MEISAI_SKS.TEHAI_NO;
                colCaption = Resources.ExportMitsumoriMeisai_TehaiNo;
                expInfoCollection.Add(colName, colCaption);
                // 単価（JPY）
                colName = Def_T_TEHAI_MEISAI.UNIT_PRICE;
                colCaption = Resources.ExportMitsumoriMeisai_UnitPrice;
                expInfoCollection.Add(colName, colCaption);
                // 有償
                colName = ComDefine.FLD_ESTIMATE_FLAG_NAME;
                colCaption = Resources.ExportMitsumoriMeisai_EstimateFlagName;
                expInfoCollection.Add(colName, colCaption);
                // 通貨
                colName = ComDefine.FLD_CURRENCY_FLAG_NAME;
                colCaption = Resources.ExportMitsumoriMeisai_CurrencyFlagName;
                expInfoCollection.Add(colName, colCaption);
                // ER（JPY）
                colName = Def_T_TEHAI_ESTIMATE.RATE_JPY;
                colCaption = Resources.ExportMitsumoriMeisai_RateJPY;
                expInfoCollection.Add(colName, colCaption, null, colDecimalDataType);
                // 販管（％）
                colName = Def_T_TEHAI_ESTIMATE.SALSES_PER;
                colCaption = Resources.ExportMitsumoriMeisai_SalsesPER;
                expInfoCollection.Add(colName, colCaption);
                // 運賃（％）
                colName = Def_T_TEHAI_ESTIMATE.ROB_PER;
                colCaption = Resources.ExportMitsumoriMeisai_RobPER;
                expInfoCollection.Add(colName, colCaption);
                // Inv単価
                colName = Def_T_TEHAI_MEISAI.INVOICE_UNIT_PRICE;
                colCaption = Resources.ExportMitsumoriMeisai_InvoiceUnitPrice;
                expInfoCollection.Add(colName, colCaption);
                // INV Value
                colName = Def_T_TEHAI_MEISAI.INVOICE_VALUE;
                colCaption = Resources.ExportMitsumoriMeisai_InvoiceValue;
                expInfoCollection.Add(colName, colCaption);
                // 見積No.
                colName = Def_T_TEHAI_MEISAI.ESTIMATE_NO;
                colCaption = Resources.ExportMitsumoriMeisai_EstimateNo;
                expInfoCollection.Add(colName, colCaption);
                // 見積名称
                colName = ComDefine.FLD_ESTIMATE_NAME;
                colCaption = Resources.ExportMitsumoriMeisai_EstimateName;
                expInfoCollection.Add(colName, colCaption);
                // PO No.
                colName = Def_T_TEHAI_ESTIMATE.PO_NO;
                colCaption = Resources.ExportMitsumoriMeisai_PONo;
                expInfoCollection.Add(colName, colCaption);
                // 出荷制限
                colName = ComDefine.FLD_HENKYAKUHIN_FLAG_NAME;
                colCaption = Resources.ExportMitsumoriMeisai_HenkyakuhinFlagName;
                expInfoCollection.Add(colName, colCaption);
                // 出荷状況
                colName = ComDefine.FLD_JYOTAI_NAME;
                colCaption = Resources.ExportMitsumoriMeisai_JyotaiName;
                expInfoCollection.Add(colName, colCaption);
                // 出荷日
                colName = Def_T_SHUKKA_MEISAI.SHUKKA_DATE;
                colCaption = Resources.ExportMitsumoriMeisai_ShukkaDate;
                expInfoCollection.Add(colName, colCaption);
                // 便名
                colName = Def_M_NONYUSAKI.SHIP;
                colCaption = Resources.ExportMitsumoriMeisai_Ship;
                expInfoCollection.Add(colName, colCaption);
                // TAG No.
                colName = Def_T_SHUKKA_MEISAI.TAG_NO;
                colCaption = Resources.ExportMitsumoriMeisai_TagNo;
                expInfoCollection.Add(colName, colCaption);
                // Invoice No.
                colName = Def_T_SHUKKA_MEISAI.INVOICE_NO;
                colCaption = Resources.ExportMitsumoriMeisai_InvoiceNo;
                expInfoCollection.Add(colName, colCaption);
                // 変更履歴
                colName = Def_T_TEHAI_MEISAI.ESTIMATE_RIREKI;
                colCaption = Resources.ExportMitsumoriMeisai_EstimateRireki;
                expInfoCollection.Add(colName, colCaption);

                // ↑
                bool ret = this.ExportExcel(filePath, dt, expInfoCollection, out msgID, out args);
                if (ret)
                {
                    // 見積明細ExcelFilesを出力しました。
                    msgID = "T0100050012";
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
