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
    /// 棚卸ListExcel出力クラス
    /// </summary>
    /// <create>T.Wakamatsu 2013/09/05</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ExportTanaoroshiList : BaseExport
    {
        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>T.Wakamatsu 2013/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public ExportTanaoroshiList()
            : base()
        {
        }

        #endregion

        #region 棚卸List出力

        /// --------------------------------------------------
        /// <summary>
        /// 棚卸ListExcelの出力
        /// </summary>
        /// <param name="filePath">ファイル名</param>
        /// <param name="dt">出力データ</param>
        /// <param name="msgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.Wakamatsu 2013/09/05</create>
        /// <update>H.Tajimi 2015/11/20 備考対応</update>
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
                ExportInfoCollection expInfoCollection = new ExportInfoCollection();
                
                // 物件名
                colName = Def_M_BUKKEN.BUKKEN_NAME;
                colCaption = Resources.ExportTanaoroshiList_PropertyName;
                expInfoCollection.Add(colName, colCaption);
                // 棚卸Ｌｏｃａｔｉｏｎ
                colName = ComDefine.FLD_NYUKO_LOCATION;
                colCaption = Resources.ExportTanaoroshiList_StocktakingLocation;
                expInfoCollection.Add(colName, colCaption);
                // 在庫Ｌｏｃａｔｉｏｎ
                colName = Def_T_STOCK.LOCATION;
                colCaption = Resources.ExportTanaoroshiList_StockLocation;
                expInfoCollection.Add(colName, colCaption);
                // Tag Code
                colName = ComDefine.FLD_TAG_CODE;
                colCaption = Resources.ExportTanaoroshiList_TagCode;
                expInfoCollection.Add(colName, colCaption);
                // Box No.
                colName = Def_T_SHUKKA_MEISAI.BOX_NO;
                colCaption = Resources.ExportTanaoroshiList_BoxNo;
                expInfoCollection.Add(colName, colCaption);
                // 状態
                colName = ComDefine.FLD_STATUS_NAME;
                colCaption = Resources.ExportTanaoroshiList_Status;
                expInfoCollection.Add(colName, colCaption);
                // 日付
                colName = Def_T_STOCK.STOCK_DATE;
                colCaption = Resources.ExportTanaoroshiList_Date;
                expInfoCollection.Add(colName, colCaption);
                // 製番
                colName = Def_T_SHUKKA_MEISAI.SEIBAN;
                colCaption = Resources.ExportTanaoroshiList_ProductNumber;
                expInfoCollection.Add(colName, colCaption);
                // CODE
                colName = Def_T_SHUKKA_MEISAI.CODE;
                colCaption = Resources.ExportTanaoroshiList_Code;
                expInfoCollection.Add(colName, colCaption);
                // 図面追番
                colName = Def_T_SHUKKA_MEISAI.ZUMEN_OIBAN;
                colCaption = Resources.ExportTanaoroshiList_DrawingSerialNumber;
                expInfoCollection.Add(colName, colCaption);
                // 品名
                colName = Def_T_SHUKKA_MEISAI.HINMEI;
                colCaption = Resources.ExportTanaoroshiList_Name;
                expInfoCollection.Add(colName, colCaption);
                // 図面/形式
                colName = Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI;
                colCaption = Resources.ExportTanaoroshiList_DrawingNumberFormat;
                expInfoCollection.Add(colName, colCaption);
                // 数量
                colName = Def_T_SHUKKA_MEISAI.NUM;
                colCaption = Resources.ExportTanaoroshiList_Quantity;
                expInfoCollection.Add(colName, colCaption);
                // 品名(和文)
                colName = Def_T_SHUKKA_MEISAI.HINMEI_JP;
                colCaption = Resources.ExportTanaoroshiList_JpName;
                expInfoCollection.Add(colName, colCaption);
                // 区割 No.
                colName = Def_T_SHUKKA_MEISAI.KUWARI_NO;
                colCaption = Resources.ExportTanaoroshiList_SectioningNo;
                expInfoCollection.Add(colName, colCaption);
                // Area
                colName = Def_T_SHUKKA_MEISAI.AREA;
                colCaption = Resources.ExportTanaoroshiList_Area;
                expInfoCollection.Add(colName, colCaption);
                // Floor
                colName = Def_T_SHUKKA_MEISAI.FLOOR;
                colCaption = Resources.ExportTanaoroshiList_Floor;
                expInfoCollection.Add(colName, colCaption);
                // 機種
                colName = Def_T_SHUKKA_MEISAI.KISHU;
                colCaption = Resources.ExportTanaoroshiList_Model;
                expInfoCollection.Add(colName, colCaption);
                // ST-No.
                colName = Def_T_SHUKKA_MEISAI.ST_NO;
                colCaption = Resources.ExportTanaoroshiList_STNo;
                expInfoCollection.Add(colName, colCaption);
                // 納入先
                colName = Def_M_NONYUSAKI.NONYUSAKI_NAME;
                colCaption = Resources.ExportTanaoroshiList_DeliveryDestination;
                expInfoCollection.Add(colName, colCaption);
                // 出荷便
                colName = Def_M_NONYUSAKI.SHIP;
                colCaption = Resources.ExportTanaoroshiList_Ship;
                expInfoCollection.Add(colName, colCaption);
                // AR No.
                colName = Def_T_SHUKKA_MEISAI.AR_NO;
                colCaption = Resources.ExportTanaoroshiList_ARNo;
                expInfoCollection.Add(colName, colCaption);
                // 2015/11/20 H.Tajimi 備考列追加
                colName = Def_T_SHUKKA_MEISAI.BIKO;
                colCaption = Resources.ExportTanaoroshiList_Memo;
                expInfoCollection.Add(colName, colCaption);
                // ↑
                // 2015/12/09 H.Tajimi M_NO列追加
                colName = Def_T_SHUKKA_MEISAI.M_NO;
                colCaption = Resources.ExportTanaoroshiList_MNo;
                expInfoCollection.Add(colName, colCaption);
                // ↑
                bool ret = this.ExportExcel(filePath, dt, expInfoCollection, out msgID, out args);
                if (ret)
                {
                    // 棚卸ListExcelファイルを出力しました。
                    msgID = "E0100070001";
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
