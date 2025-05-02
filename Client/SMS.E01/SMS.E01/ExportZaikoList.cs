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
    /// 在庫ListExcel出力クラス
    /// </summary>
    /// <create>T.Wakamatsu 2013/09/05</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ExportZaikoList : BaseExport
    {
        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>T.Wakamatsu 2013/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public ExportZaikoList()
            : base()
        {
        }

        #endregion

        #region 在庫List出力

        /// --------------------------------------------------
        /// <summary>
        /// 在庫ListExcelの出力
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
                colCaption = Resources.ExportZaikoList_PropertyName;
                expInfoCollection.Add(colName, colCaption);
                // Tag Code
                colName = ComDefine.FLD_TAG_CODE;
                colCaption = Resources.ExportZaikoList_TagCode;
                expInfoCollection.Add(colName, colCaption);
                // Box No.
                colName = Def_T_SHUKKA_MEISAI.BOX_NO;
                colCaption = Resources.ExportZaikoList_BoxNo;
                expInfoCollection.Add(colName, colCaption);
                // 在庫Ｌｏｃａｔｉｏｎ
                colName = Def_T_STOCK.LOCATION;
                colCaption = Resources.ExportZaikoList_StockLocation;
                expInfoCollection.Add(colName, colCaption);
                // 状態
                colName = ComDefine.FLD_STATUS_NAME;
                colCaption = Resources.ExportZaikoList_Status;
                expInfoCollection.Add(colName, colCaption);
                // 日付
                colName = Def_T_STOCK.STOCK_DATE;
                colCaption = Resources.ExportZaikoList_Date;
                expInfoCollection.Add(colName, colCaption);
                // 製番
                colName = Def_T_SHUKKA_MEISAI.SEIBAN;
                colCaption = Resources.ExportZaikoList_ProductNumber;
                expInfoCollection.Add(colName, colCaption);
                // CODE
                colName = Def_T_SHUKKA_MEISAI.CODE;
                colCaption = Resources.ExportZaikoList_Code;
                expInfoCollection.Add(colName, colCaption);
                // 図面追番
                colName = Def_T_SHUKKA_MEISAI.ZUMEN_OIBAN;
                colCaption = Resources.ExportZaikoList_DrawingSerialNumber;
                expInfoCollection.Add(colName, colCaption);
                // 品名
                colName = Def_T_SHUKKA_MEISAI.HINMEI;
                colCaption = Resources.ExportZaikoList_Name;
                expInfoCollection.Add(colName, colCaption);
                // 図面/形式
                colName = Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI;
                colCaption = Resources.ExportZaikoList_DrawingNumberFormat;
                expInfoCollection.Add(colName, colCaption);
                // 数量
                colName = Def_T_SHUKKA_MEISAI.NUM;
                colCaption = Resources.ExportZaikoList_Quantity;
                expInfoCollection.Add(colName, colCaption);
                // 品名(和文)
                colName = Def_T_SHUKKA_MEISAI.HINMEI_JP;
                colCaption = Resources.ExportZaikoList_JpName;
                expInfoCollection.Add(colName, colCaption);
                // 区割 No.
                colName = Def_T_SHUKKA_MEISAI.KUWARI_NO;
                colCaption = Resources.ExportZaikoList_SectioningNo;
                expInfoCollection.Add(colName, colCaption);
                // Area
                colName = Def_T_SHUKKA_MEISAI.AREA;
                colCaption = Resources.ExportZaikoList_Area;
                expInfoCollection.Add(colName, colCaption);
                // Floor
                colName = Def_T_SHUKKA_MEISAI.FLOOR;
                colCaption = Resources.ExportZaikoList_Floor;
                expInfoCollection.Add(colName, colCaption);
                // 機種
                colName = Def_T_SHUKKA_MEISAI.KISHU;
                colCaption = Resources.ExportZaikoList_Model;
                expInfoCollection.Add(colName, colCaption);
                // ST-No.
                colName = Def_T_SHUKKA_MEISAI.ST_NO;
                colCaption = Resources.ExportZaikoList_STNo;
                expInfoCollection.Add(colName, colCaption);
                // 納入先
                colName = Def_M_NONYUSAKI.NONYUSAKI_NAME;
                colCaption = Resources.ExportZaikoList_DeliveryDestination;
                expInfoCollection.Add(colName, colCaption);
                // 出荷便
                colName = Def_M_NONYUSAKI.SHIP;
                colCaption = Resources.ExportZaikoList_Ship;
                expInfoCollection.Add(colName, colCaption);
                // AR No.
                colName = Def_T_SHUKKA_MEISAI.AR_NO;
                colCaption = Resources.ExportZaikoList_ARNo;
                expInfoCollection.Add(colName, colCaption);
                // パレット No.
                colName = Def_T_SHUKKA_MEISAI.PALLET_NO;
                colCaption = Resources.ExportZaikoList_PalletNo;
                expInfoCollection.Add(colName, colCaption);
                // 木枠 No.
                colName = ComDefine.FLD_KIWAKU_NO;
                colCaption = Resources.ExportZaikoList_WoodFrameNo;
                expInfoCollection.Add(colName, colCaption);
                // 集荷日付
                colName = Def_T_SHUKKA_MEISAI.SHUKA_DATE;
                colCaption = Resources.ExportZaikoList_PickUpDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // Box梱包日付
                colName = Def_T_SHUKKA_MEISAI.BOXKONPO_DATE;
                colCaption = Resources.ExportZaikoList_BoxPackingDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // パレット梱包日付
                colName = Def_T_SHUKKA_MEISAI.PALLETKONPO_DATE;
                colCaption = Resources.ExportZaikoList_PalletPackingDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 木枠梱包日付
                colName = Def_T_SHUKKA_MEISAI.KIWAKUKONPO_DATE;
                colCaption = Resources.ExportZaikoList_FramePackingDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 出荷日付
                colName = Def_T_SHUKKA_MEISAI.SHUKKA_DATE;
                colCaption = Resources.ExportZaikoList_ShippingDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 運送会社
                colName = Def_T_SHUKKA_MEISAI.UNSOKAISHA_NAME;
                colCaption = Resources.ExportZaikoList_ShippingCompany;
                expInfoCollection.Add(colName, colCaption);
                // インボイスNo.
                colName = Def_T_SHUKKA_MEISAI.INVOICE_NO;
                colCaption = Resources.ExportZaikoList_InvoiceNo;
                expInfoCollection.Add(colName, colCaption);
                // 送り状No.
                colName = Def_T_SHUKKA_MEISAI.OKURIJYO_NO;
                colCaption = Resources.ExportZaikoList_OkurijoNo;
                expInfoCollection.Add(colName, colCaption);
                // BLNo.
                colName = Def_T_SHUKKA_MEISAI.BL_NO;
                colCaption = Resources.ExportZaikoList_BLNo;
                expInfoCollection.Add(colName, colCaption);
                // 受入日付
                colName = Def_T_SHUKKA_MEISAI.UKEIRE_DATE;
                colCaption = Resources.ExportZaikoList_AccessionDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 受入担当者
                colName = Def_T_SHUKKA_MEISAI.UKEIRE_USER_NAME;
                colCaption = Resources.ExportZaikoList_AccessionUser;
                expInfoCollection.Add(colName, colCaption);
                // 2015/11/20 H.Tajimi 備考列追加
                colName = Def_T_SHUKKA_MEISAI.BIKO;
                colCaption = Resources.ExportZaikoList_Memo;
                expInfoCollection.Add(colName, colCaption);
                // ↑
                // 2015/12/09 H.Tajimi M_NO列追加
                colName = Def_T_SHUKKA_MEISAI.M_NO;
                colCaption = Resources.ExportZaikoList_MNo;
                expInfoCollection.Add(colName, colCaption);
                // ↑
                bool ret = this.ExportExcel(filePath, dt, expInfoCollection, out msgID, out args);
                if (ret)
                {
                    // 在庫ListExcelファイルを出力しました。
                    msgID = "E0100060001";
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
