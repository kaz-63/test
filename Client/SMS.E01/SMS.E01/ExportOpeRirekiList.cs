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
    /// Ope履歴ListExcel出力クラス
    /// </summary>
    /// <create>T.Wakamatsu 2013/09/05</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ExportOpeRirekiList : BaseExport
    {
        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>T.Wakamatsu 2013/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public ExportOpeRirekiList()
            : base()
        {
        }

        #endregion

        #region Ope履歴List出力

        /// --------------------------------------------------
        /// <summary>
        /// Ope履歴ListExcelの出力
        /// </summary>
        /// <param name="filePath">ファイル名</param>
        /// <param name="dt">出力データ</param>
        /// <param name="msgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.Wakamatsu 2013/09/05</create>
        /// <update>H.Tajimi 2015/11/20 備考列追加</update>
        /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
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
                string dateTimeFormat = "yyyy/MM/dd";
                ExportInfoCollection expInfoCollection = new ExportInfoCollection();
                
                // 物件名
                colName = Def_M_BUKKEN.BUKKEN_NAME;
                colCaption = Resources.ExportOpeRirekiList_PropertyName;
                expInfoCollection.Add(colName, colCaption);
                // 日付
                colName = Def_T_JISSEKI.JISSEKI_DATE;
                colCaption = Resources.ExportOpeRirekiList_Date;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // Tag Code
                colName = ComDefine.FLD_TAG_CODE;
                colCaption = Resources.ExportOpeRirekiList_TagCode;
                expInfoCollection.Add(colName, colCaption);
                // 在庫Ｌｏｃａｔｉｏｎ
                colName = Def_T_STOCK.LOCATION;
                colCaption = Resources.ExportOpeRirekiList_Location;
                expInfoCollection.Add(colName, colCaption);
                // 作業区分
                colName = ComDefine.FLD_SAGYO_FLAG_NAME;
                colCaption = Resources.ExportOpeRirekiList_DivisionOfWork;
                expInfoCollection.Add(colName, colCaption);
                // 状態
                colName = ComDefine.FLD_STATUS_NAME;
                colCaption = Resources.ExportOpeRirekiList_Status;
                expInfoCollection.Add(colName, colCaption);
                // 作業者
                colName = Def_T_JISSEKI.WORK_USER_NAME;
                colCaption = Resources.ExportOpeRirekiList_Worker;
                expInfoCollection.Add(colName, colCaption);
                // 在庫/完了日付
                colName = Def_T_JISSEKI.STOCK_DATE;
                colCaption = Resources.ExportOpeRirekiList_StockComletionDate;
                expInfoCollection.Add(colName, colCaption);
                // 製番
                colName = Def_T_SHUKKA_MEISAI.SEIBAN;
                colCaption = Resources.ExportOpeRirekiList_ProductNumber;
                expInfoCollection.Add(colName, colCaption);
                // CODE
                colName = Def_T_SHUKKA_MEISAI.CODE;
                colCaption = Resources.ExportOpeRirekiList_Code;
                expInfoCollection.Add(colName, colCaption);
                // 図面追番
                colName = Def_T_SHUKKA_MEISAI.ZUMEN_OIBAN;
                colCaption = Resources.ExportOpeRirekiList_DrawingSerialNumber;
                expInfoCollection.Add(colName, colCaption);
                // 品名
                colName = Def_T_SHUKKA_MEISAI.HINMEI;
                colCaption = Resources.ExportOpeRirekiList_Name;
                expInfoCollection.Add(colName, colCaption);
                // 図面/形式
                colName = Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI;
                colCaption = Resources.ExportOpeRirekiList_DrawingNumberFormat;
                expInfoCollection.Add(colName, colCaption);
                // 数量
                colName = Def_T_SHUKKA_MEISAI.NUM;
                colCaption = Resources.ExportOpeRirekiList_Quantity;
                expInfoCollection.Add(colName, colCaption);
                // 品名(和文)
                colName = Def_T_SHUKKA_MEISAI.HINMEI_JP;
                colCaption = Resources.ExportOpeRirekiList_JpName;
                expInfoCollection.Add(colName, colCaption);
                // 区割 No.
                colName = Def_T_SHUKKA_MEISAI.KUWARI_NO;
                colCaption = Resources.ExportOpeRirekiList_SectioningNo;
                expInfoCollection.Add(colName, colCaption);
                // Area
                colName = Def_T_SHUKKA_MEISAI.AREA;
                colCaption = Resources.ExportOpeRirekiList_Area;
                expInfoCollection.Add(colName, colCaption);
                // Floor
                colName = Def_T_SHUKKA_MEISAI.FLOOR;
                colCaption = Resources.ExportOpeRirekiList_Floor;
                expInfoCollection.Add(colName, colCaption);
                // 機種
                colName = Def_T_SHUKKA_MEISAI.KISHU;
                colCaption = Resources.ExportOpeRirekiList_Model;
                expInfoCollection.Add(colName, colCaption);
                // ST-No.
                colName = Def_T_SHUKKA_MEISAI.ST_NO;
                colCaption = Resources.ExportOpeRirekiList_STNo;
                expInfoCollection.Add(colName, colCaption);
                // 納入先
                colName = Def_M_NONYUSAKI.NONYUSAKI_NAME;
                colCaption = Resources.ExportOpeRirekiList_DeliveryDestination;
                expInfoCollection.Add(colName, colCaption);
                // 出荷便
                colName = Def_M_NONYUSAKI.SHIP;
                colCaption = Resources.ExportOpeRirekiList_Ship;
                expInfoCollection.Add(colName, colCaption);
                // AR No.
                colName = Def_T_SHUKKA_MEISAI.AR_NO;
                colCaption = Resources.ExportOpeRirekiList_ARNo;
                expInfoCollection.Add(colName, colCaption);
                // 2015/11/20 H.Tajimi 備考列追加
                colName = Def_T_SHUKKA_MEISAI.BIKO;
                colCaption = Resources.ExportOpeRirekiList_Memo;
                expInfoCollection.Add(colName, colCaption);
                // ↑
                // 2015/12/09 H.Tajimi M_NO列追加
                colName = Def_T_SHUKKA_MEISAI.M_NO;
                colCaption = Resources.ExportOpeRirekiList_MNo;
                expInfoCollection.Add(colName, colCaption);
                // ↑
                bool ret = this.ExportExcel(filePath, dt, expInfoCollection, out msgID, out args);
                if (ret)
                {
                    // Ope履歴ListExcelファイルを出力しました。
                    msgID = "E0100080001";
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
