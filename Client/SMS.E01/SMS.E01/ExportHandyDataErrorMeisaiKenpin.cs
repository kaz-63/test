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
    /// 検品取込ERROR詳細Excel出力クラス
    /// </summary>
    /// <create>H.Tsuji 2020/06/17</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ExportHandyDataErrorMeisaiKenpin : BaseExportXlsx
    {
        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>H.Tsuji 2020/06/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public ExportHandyDataErrorMeisaiKenpin()
            : base()
        {
        }

        #endregion

        #region 検品取込ERROR詳細Excel出力

        /// --------------------------------------------------
        /// <summary>
        /// 検品取込ERROR詳細Excel出力
        /// </summary>
        /// <param name="filePath">ファイル名</param>
        /// <param name="dt">出力データ</param>
        /// <param name="msgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tsuji 2020/06/17</create>
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
                string dateTimeFormat = "yyyy/MM/dd hh:mm:ss";
                ExportInfoCollection expInfoCollection = new ExportInfoCollection();
                // 手配No.
                colName = Def_T_TEMPWORK_MEISAI.TEHAI_NO;
                colCaption = Resources.ExportHandyDataErrorMeisaiKenpin_TehaiNo;
                expInfoCollection.Add(colName, colCaption);
                // 結果
                colName = ComDefine.FLD_RESULT_STRING;
                colCaption = Resources.ExportHandyDataErrorMeisaiKenpin_Result;
                expInfoCollection.Add(colName, colCaption);
                // Error内容
                colName = Def_T_TEMPWORK_MEISAI.DESCRIPTION;
                colCaption = Resources.ExportHandyDataErrorMeisaiKenpin_ErrotDetail;
                expInfoCollection.Add(colName, colCaption);
                // 納入先
                colName = Def_T_TEMPWORK_MEISAI.NONYUSAKI_NAME;
                colCaption = Resources.ExportHandyDataErrorMeisaiKenpin_DeliveryDestination;
                expInfoCollection.Add(colName, colCaption);
                // 便/AR No.
                colName = ComDefine.FLD_SHIP_AR_NO;
                colCaption = Resources.ExportHandyDataErrorMeisaiKenpin_ShipARNo;
                expInfoCollection.Add(colName, colCaption);
                // 取込日時
                colName = Def_T_TEMPWORK.TORIKOMI_DATE;
                colCaption = Resources.ExportHandyDataErrorMeisaiKenpin_TorikomiDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // Customer名
                colName = Def_T_TEHAI_SKS_WORK.CUSTOMER_NAME;
                colCaption = Resources.ExportHandyDataErrorMeisaiKenpin_CustomerName;
                expInfoCollection.Add(colName, colCaption);
                // 納入場所
                colName = Def_T_TEHAI_SKS_WORK.NONYUBASHO;
                colCaption = Resources.ExportHandyDataErrorMeisaiKenpin_Nonyubasho;
                expInfoCollection.Add(colName, colCaption);
                // 製番CODE
                colName = Def_T_TEHAI_SKS_WORK.SEIBAN_CODE;
                colCaption = Resources.ExportHandyDataErrorMeisaiKenpin_SeibanCode;
                expInfoCollection.Add(colName, colCaption);
                // ECS No.
                colName = Def_T_TEHAI_SKS_WORK.ECS_NO;
                colCaption = Resources.ExportHandyDataErrorMeisaiKenpin_EcsNo;
                expInfoCollection.Add(colName, colCaption);
                // 品番
                colName = Def_T_TEHAI_SKS_WORK.HINBAN;
                colCaption = Resources.ExportHandyDataErrorMeisaiKenpin_Hinban;
                expInfoCollection.Add(colName, colCaption);
                // 型式
                colName = Def_T_TEHAI_SKS_WORK.KATASHIKI;
                colCaption = Resources.ExportHandyDataErrorMeisaiKenpin_Katashiki;
                expInfoCollection.Add(colName, colCaption);
                // 注文書品目名称
                colName = Def_T_TEHAI_SKS_WORK.CHUMONSHO_HINMOKU;
                colCaption = Resources.ExportHandyDataErrorMeisaiKenpin_ChumonshoHinmoku;
                expInfoCollection.Add(colName, colCaption);
                // 手配数
                colName = Def_T_TEHAI_SKS_WORK.TEHAI_QTY;
                colCaption = Resources.ExportHandyDataErrorMeisaiKenpin_TehaiQty;
                expInfoCollection.Add(colName, colCaption);

                bool ret = this.ExportExcel(filePath, dt, expInfoCollection, out msgID, out args);
                if (ret)
                {
                    // 検品取込ERROR詳細ExcelFilesを出力しました。
                    msgID = "E0100160001";
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
