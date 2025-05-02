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
    /// <create>Y.Higuchi 2010/07/20</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ExportShukkaMeisai : BaseExport
    {
        #region 単価表示プロパティ　2022/10/17
        //【Step_1_2】出荷情報照会画面の権限制御 2022/10/17（TW-Tsuji）
        //
        //　単価（JPY）列の出力を制御するためのフラグをメンバに追加する.
        //　（デフォルトは false ）
        public bool IsUnitPriceDisable { private get; set; }
        #endregion
        
        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public ExportShukkaMeisai()
            : base()
        {
            //【Step_1_2】出荷情報照会画面の権限制御 2022/10/17（TW-Tsuji）
            //
            //　単価（JPY）列の出力を制御するためのフラグをメンバに追加する.
            //　（デフォルトは false ）
            IsUnitPriceDisable = false;
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
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update>H.Tajimi 2015/11/18 Free1、Free2をExcel出力対象列に追加</update>
        /// <update>H.Tajimi 2015/11/20 備考列追加</update>
        /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
        /// <update>H.Tajimi 2019/08/09 写真管理方式変更</update>
        /// <update>H.Tajimi 2019/09/09 印刷C/NO対応</update>
        /// <update>J.Chen 2022/12/19 Excel出荷データ追加（STEP15）</update>
        /// <update>J.Jeong 2024/07/11 Excel出荷データ追加（STEP17）</update>
        /// <update>J.Chen 2024/11/07 通関確認状態追加（STEP17）</update>
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
                // 状態
                colName = ComDefine.FLD_JYOTAI_NAME;
                colCaption = Resources.ExportTehaiMeisai_JyotaiName;
                expInfoCollection.Add(colName, colCaption);
                // Tag No.
                colName = Def_T_SHUKKA_MEISAI.TAG_NO;
                colCaption = Resources.ExportShukkaMeisai_TagNo;
                expInfoCollection.Add(colName, colCaption);
                // 製番
                colName = Def_T_SHUKKA_MEISAI.SEIBAN;
                colCaption = Resources.ExportShukkaMeisai_ProductNumber;
                expInfoCollection.Add(colName, colCaption);
                // CODE
                colName = Def_T_SHUKKA_MEISAI.CODE;
                colCaption = Resources.ExportShukkaMeisai_CODE;
                expInfoCollection.Add(colName, colCaption);
                // 図面追番
                colName = Def_T_SHUKKA_MEISAI.ZUMEN_OIBAN;
                colCaption = Resources.ExportShukkaMeisai_DrawingSerialNumber;
                expInfoCollection.Add(colName, colCaption);
                // 納入先
                colName = Def_M_NONYUSAKI.NONYUSAKI_NAME;
                colCaption = Resources.ExportShukkaMeisai_DeliveryDestination;
                expInfoCollection.Add(colName, colCaption);
                // 出荷便
                colName = Def_M_NONYUSAKI.SHIP;
                colCaption = Resources.ExportShukkaMeisai_Ship;
                expInfoCollection.Add(colName, colCaption);
                // Area
                colName = Def_T_SHUKKA_MEISAI.AREA;
                colCaption = Resources.ExportShukkaMeisai_Area;
                expInfoCollection.Add(colName, colCaption);
                // Floor
                colName = Def_T_SHUKKA_MEISAI.FLOOR;
                colCaption = Resources.ExportShukkaMeisai_Floor;
                expInfoCollection.Add(colName, colCaption);
                // 機種
                colName = Def_T_SHUKKA_MEISAI.KISHU;
                colCaption = Resources.ExportShukkaMeisai_Model;
                expInfoCollection.Add(colName, colCaption);
                // ST-No.
                colName = Def_T_SHUKKA_MEISAI.ST_NO;
                colCaption = Resources.ExportShukkaMeisai_STNo;
                expInfoCollection.Add(colName, colCaption);
                // 品名(和文)
                colName = Def_T_SHUKKA_MEISAI.HINMEI_JP;
                colCaption = Resources.ExportShukkaMeisai_JpName;
                expInfoCollection.Add(colName, colCaption);
                // 品名
                colName = Def_T_SHUKKA_MEISAI.HINMEI;
                colCaption = Resources.ExportShukkaMeisai_Name;
                expInfoCollection.Add(colName, colCaption);
                // 図面/形式
                colName = Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI;
                colCaption = Resources.ExportShukkaMeisai_DrawingNumberFormat;
                expInfoCollection.Add(colName, colCaption);
                // 2022/12/19 J.Chen 図面/形式2
                colName = Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI2;
                colCaption = Resources.ExportShukkaMeisai_DrawingNumberFormat2;
                expInfoCollection.Add(colName, colCaption);
                // 区割 No.
                colName = Def_T_SHUKKA_MEISAI.KUWARI_NO;
                colCaption = Resources.ExportShukkaMeisai_SectioningNo;
                expInfoCollection.Add(colName, colCaption);
                // 数量
                colName = Def_T_SHUKKA_MEISAI.NUM;
                colCaption = Resources.ExportShukkaMeisai_Quantity;
                expInfoCollection.Add(colName, colCaption);
                // 出荷便
                colName = Def_M_NONYUSAKI.SHIP;
                colCaption = Resources.ExportShukkaMeisai_Ship;
                expInfoCollection.Add(colName, colCaption);
                // Box No.
                colName = Def_T_SHUKKA_MEISAI.BOX_NO;
                colCaption = Resources.ExportShukkaMeisai_BoxNo;
                expInfoCollection.Add(colName, colCaption);
                // パレット No.
                colName = Def_T_SHUKKA_MEISAI.PALLET_NO;
                // 2011/02/18 K.Tsutsumi Change No32
                //colCaption = "パレット No.";
                colCaption = Resources.ExportShukkaMeisai_PalletNo;
                // ↑
                expInfoCollection.Add(colName, colCaption);
                // 木枠 No.
                colName = Def_T_KIWAKU_MEISAI.PRINT_CASE_NO;
                colCaption = Resources.ExportShukkaMeisai_WoodFrameNo;
                expInfoCollection.Add(colName, colCaption);
                // AR No.
                colName = Def_T_SHUKKA_MEISAI.AR_NO;
                colCaption = Resources.ExportShukkaMeisai_ARNo;
                expInfoCollection.Add(colName, colCaption);
                // 集荷日付
                colName = Def_T_SHUKKA_MEISAI.SHUKA_DATE;
                colCaption = Resources.ExportShukkaMeisai_PickUpDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // Box梱包日付
                colName = Def_T_SHUKKA_MEISAI.BOXKONPO_DATE;
                colCaption = Resources.ExportShukkaMeisai_BoxPackingDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // パレット梱包日付
                colName = Def_T_SHUKKA_MEISAI.PALLETKONPO_DATE;
                // 2011/02/18 K.Tsutsumi Change 
                //colCaption = "パレット梱包日付";
                colCaption = Resources.ExportShukkaMeisai_PalletPackingDate;
                // ↑
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 木枠梱包日付
                colName = Def_T_SHUKKA_MEISAI.KIWAKUKONPO_DATE;
                colCaption = Resources.ExportShukkaMeisai_FramePackingDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 出荷日付
                colName = Def_T_SHUKKA_MEISAI.SHUKKA_DATE;
                colCaption = Resources.ExportShukkaMeisai_ShippingDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 運送会社
                colName = Def_T_SHUKKA_MEISAI.UNSOKAISHA_NAME;
                colCaption = Resources.ExportShukkaMeisai_ShippingCompany;
                expInfoCollection.Add(colName, colCaption);
                // インボイスNo.
                colName = Def_T_SHUKKA_MEISAI.INVOICE_NO;
                // 2011/02/18 K.Tsutsumi Change 
                //colCaption = "インボイスNo.";
                colCaption = Resources.ExportShukkaMeisai_InvoiceNo;
                // ↑
                expInfoCollection.Add(colName, colCaption);
                // 送り状No.
                colName = Def_T_SHUKKA_MEISAI.OKURIJYO_NO;
                colCaption = Resources.ExportShukkaMeisai_OkurijoNo;
                expInfoCollection.Add(colName, colCaption);
                // BLNo.
                colName = Def_T_SHUKKA_MEISAI.BL_NO;
                colCaption = Resources.ExportShukkaMeisai_BLNo;
                expInfoCollection.Add(colName, colCaption);
                // 受入日付
                colName = Def_T_SHUKKA_MEISAI.UKEIRE_DATE;
                colCaption = Resources.ExportShukkaMeisai_AccessionDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 受入担当者
                colName = Def_T_SHUKKA_MEISAI.UKEIRE_USER_NAME;
                colCaption = Resources.ExportShukkaMeisai_AccessionUser;
                expInfoCollection.Add(colName, colCaption);
                // 2015/11/18 H.Tajimi Free1,Free2を列追加
                // Free1
                colName = Def_T_SHUKKA_MEISAI.FREE1;
                colCaption = Resources.ExportShukkaMeisai_Free1;
                expInfoCollection.Add(colName, colCaption);
                // Free2
                colName = Def_T_SHUKKA_MEISAI.FREE2;
                colCaption = Resources.ExportShukkaMeisai_Free2;
                expInfoCollection.Add(colName, colCaption);
                // ↑
                // 2015/11/20 H.Tajimi 備考列追加
                colName = Def_T_SHUKKA_MEISAI.BIKO;
                colCaption = Resources.ExportShukkaMeisai_Memo;
                expInfoCollection.Add(colName, colCaption);
                //// 通関確認状態列追加
                //colName = Def_T_SHUKKA_MEISAI.CUSTOMS_STATUS;
                //colCaption = Resources.ExportShukkaMeisai_CustomsStatus;
                //expInfoCollection.Add(colName, colCaption);
                // ↑
                // 2015/12/09 H.Tajimi M_NO列追加
                colName = Def_T_SHUKKA_MEISAI.M_NO;
                colCaption = Resources.ExportShukkaMeisai_MNo;
                expInfoCollection.Add(colName, colCaption);
                // ↑
                // 2018/11/15 T.Nakata GRWT列追加
                colName = Def_T_SHUKKA_MEISAI.GRWT;
                colCaption = Resources.ExportShukkaMeisai_GRWT;
                expInfoCollection.Add(colName, colCaption);
                // ↑
                // 2018/11/15 T.Nakata TEHAI_RENKEI_NO列追加
                colName = Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO;
                colCaption = Resources.ExportShukkaMeisai_TEHAI_RENKEI_NO;
                expInfoCollection.Add(colName, colCaption);
                // ↑
                // 2019/08/09 H.Tajimi 写真列追加
                colName = ComDefine.FLD_EXISTS_PICTURE;
                colCaption = Resources.ExportShukkaMeisai_Picture;
                expInfoCollection.Add(colName, colCaption);
                // ↑
                // 2022/12/19 J.Chen 出荷元列追加
                colName = Def_M_SHIP_FROM.SHIP_FROM_NAME;
                colCaption = Resources.ExportShukkaMeisai_SipFromName;
                expInfoCollection.Add(colName, colCaption);
                // ↑
                // 2022/12/19 J.Chen TAG便名列追加
                colName = Def_T_SHUKKA_MEISAI.TAG_SHIP;
                colCaption = Resources.ExportShukkaMeisai_TagShip;
                expInfoCollection.Add(colName, colCaption);
                // ↑
                bool ret = this.ExportExcel(filePath, dt, expInfoCollection, out msgID, out args);
                if (ret)
                {
                    // 出荷明細Excelファイルを出力しました。
                    msgID = "E0100020001";
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

        #region 出荷明細出力(with 手配情報)

        /// --------------------------------------------------
        /// <summary>
        /// 出荷明細Excelの出力(with 手配情報)
        /// </summary>
        /// <param name="filePath">ファイル名</param>
        /// <param name="dt">出力データ</param>
        /// <param name="msgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>K.Tsutsumi 2019/03/08</create>
        /// <update>H.Tajimi 2019/08/09 写真管理方式変更</update>
        /// <update>H.Tajimi 2019/09/09 印刷C/NO対応</update>
        /// <update>J.Chen 2022/12/19 Excel出荷データ追加（STEP15分）</update>
        /// <update>J.Jeong 2024/07/09 重複列を削除（出荷便）</update>
        /// <update>J.Chen 2024/11/07 通関確認状態追加（STEP17）</update>
        /// --------------------------------------------------
        public bool ExportExcelwithProcurement(string filePath, System.Data.DataTable dt, out string msgID, out string[] args)
        {
            msgID = string.Empty;
            args = null;
            try
            {
                string colName = string.Empty;
                string colCaption = string.Empty;
                string dateTimeFormat = "yyyy/MM/dd";
                ExportInfoCollection expInfoCollection = new ExportInfoCollection();
                // 設定納期
                colName = Def_T_TEHAI_MEISAI.SETTEI_DATE;
                colCaption = Resources.ExportShukkaMeisai_SetteiDate;
                expInfoCollection.Add(colName, colCaption);
                // 納品先
                colName = Def_T_TEHAI_MEISAI.NOUHIN_SAKI;
                colCaption = Resources.ExportShukkaMeisai_NouhinSaki;
                expInfoCollection.Add(colName, colCaption);
                // 出荷先
                colName = Def_T_TEHAI_MEISAI.SYUKKA_SAKI;
                colCaption = Resources.ExportShukkaMeisai_SyukkaSaki;
                expInfoCollection.Add(colName, colCaption);
                // Tag No.
                colName = Def_T_SHUKKA_MEISAI.TAG_NO;
                colCaption = Resources.ExportShukkaMeisai_TagNo;
                expInfoCollection.Add(colName, colCaption);
                // 製番
                colName = Def_T_SHUKKA_MEISAI.SEIBAN;
                colCaption = Resources.ExportShukkaMeisai_ProductNumber;
                expInfoCollection.Add(colName, colCaption);
                // CODE
                colName = Def_T_SHUKKA_MEISAI.CODE;
                colCaption = Resources.ExportShukkaMeisai_CODE;
                expInfoCollection.Add(colName, colCaption);
                // 図面追番
                colName = Def_T_SHUKKA_MEISAI.ZUMEN_OIBAN;
                colCaption = Resources.ExportShukkaMeisai_DrawingSerialNumber;
                expInfoCollection.Add(colName, colCaption);
                // 納入先
                colName = Def_M_NONYUSAKI.NONYUSAKI_NAME;
                colCaption = Resources.ExportShukkaMeisai_DeliveryDestination;
                expInfoCollection.Add(colName, colCaption);
                // 出荷便
                colName = Def_M_NONYUSAKI.SHIP;
                colCaption = Resources.ExportShukkaMeisai_Ship;
                expInfoCollection.Add(colName, colCaption);
                // Area
                colName = Def_T_SHUKKA_MEISAI.AREA;
                colCaption = Resources.ExportShukkaMeisai_Area;
                expInfoCollection.Add(colName, colCaption);
                // Floor
                colName = Def_T_SHUKKA_MEISAI.FLOOR;
                colCaption = Resources.ExportShukkaMeisai_Floor;
                expInfoCollection.Add(colName, colCaption);
                // 機種
                colName = Def_T_SHUKKA_MEISAI.KISHU;
                colCaption = Resources.ExportShukkaMeisai_Model;
                expInfoCollection.Add(colName, colCaption);
                // ST-No.
                colName = Def_T_SHUKKA_MEISAI.ST_NO;
                colCaption = Resources.ExportShukkaMeisai_STNo;
                expInfoCollection.Add(colName, colCaption);
                // 品名(和文)
                colName = Def_T_SHUKKA_MEISAI.HINMEI_JP;
                colCaption = Resources.ExportShukkaMeisai_JpName;
                expInfoCollection.Add(colName, colCaption);
                // 品名
                colName = Def_T_SHUKKA_MEISAI.HINMEI;
                colCaption = Resources.ExportShukkaMeisai_Name;
                expInfoCollection.Add(colName, colCaption);
                // 図面/形式
                colName = Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI;
                colCaption = Resources.ExportShukkaMeisai_DrawingNumberFormat;
                expInfoCollection.Add(colName, colCaption);
                // 図番/型式2
                colName = Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI2;
                colCaption = Resources.ExportShukkaMeisai_ZumenKeishiki2;
                expInfoCollection.Add(colName, colCaption);
                // 区割 No.
                colName = Def_T_SHUKKA_MEISAI.KUWARI_NO;
                colCaption = Resources.ExportShukkaMeisai_SectioningNo;
                expInfoCollection.Add(colName, colCaption);
                // 数量
                colName = Def_T_SHUKKA_MEISAI.NUM;
                colCaption = Resources.ExportShukkaMeisai_Quantity;
                expInfoCollection.Add(colName, colCaption);
                // 出荷便
                //colName = Def_M_NONYUSAKI.SHIP;
                //colCaption = Resources.ExportShukkaMeisai_Ship;
                //expInfoCollection.Add(colName, colCaption);
                // Box No.
                colName = Def_T_SHUKKA_MEISAI.BOX_NO;
                colCaption = Resources.ExportShukkaMeisai_BoxNo;
                expInfoCollection.Add(colName, colCaption);
                // パレット No.
                colName = Def_T_SHUKKA_MEISAI.PALLET_NO;
                // 2011/02/18 K.Tsutsumi Change No32
                //colCaption = "パレット No.";
                colCaption = Resources.ExportShukkaMeisai_PalletNo;
                // ↑
                expInfoCollection.Add(colName, colCaption);
                // 木枠 No.
                colName = Def_T_KIWAKU_MEISAI.PRINT_CASE_NO;
                colCaption = Resources.ExportShukkaMeisai_WoodFrameNo;
                expInfoCollection.Add(colName, colCaption);
                // AR No.
                colName = Def_T_SHUKKA_MEISAI.AR_NO;
                colCaption = Resources.ExportShukkaMeisai_ARNo;
                expInfoCollection.Add(colName, colCaption);
                // 集荷日付
                colName = Def_T_SHUKKA_MEISAI.SHUKA_DATE;
                colCaption = Resources.ExportShukkaMeisai_PickUpDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // Box梱包日付
                colName = Def_T_SHUKKA_MEISAI.BOXKONPO_DATE;
                colCaption = Resources.ExportShukkaMeisai_BoxPackingDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // パレット梱包日付
                colName = Def_T_SHUKKA_MEISAI.PALLETKONPO_DATE;
                // 2011/02/18 K.Tsutsumi Change 
                //colCaption = "パレット梱包日付";
                colCaption = Resources.ExportShukkaMeisai_PalletPackingDate;
                // ↑
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 木枠梱包日付
                colName = Def_T_SHUKKA_MEISAI.KIWAKUKONPO_DATE;
                colCaption = Resources.ExportShukkaMeisai_FramePackingDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 出荷日付
                colName = Def_T_SHUKKA_MEISAI.SHUKKA_DATE;
                colCaption = Resources.ExportShukkaMeisai_ShippingDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 運送会社
                colName = Def_T_SHUKKA_MEISAI.UNSOKAISHA_NAME;
                colCaption = Resources.ExportShukkaMeisai_ShippingCompany;
                expInfoCollection.Add(colName, colCaption);
                // インボイスNo.
                colName = Def_T_SHUKKA_MEISAI.INVOICE_NO;
                // 2011/02/18 K.Tsutsumi Change 
                //colCaption = "インボイスNo.";
                colCaption = Resources.ExportShukkaMeisai_InvoiceNo;
                // ↑
                expInfoCollection.Add(colName, colCaption);
                // 送り状No.
                colName = Def_T_SHUKKA_MEISAI.OKURIJYO_NO;
                colCaption = Resources.ExportShukkaMeisai_OkurijoNo;
                expInfoCollection.Add(colName, colCaption);
                // BLNo.
                colName = Def_T_SHUKKA_MEISAI.BL_NO;
                colCaption = Resources.ExportShukkaMeisai_BLNo;
                expInfoCollection.Add(colName, colCaption);
                // 受入日付
                colName = Def_T_SHUKKA_MEISAI.UKEIRE_DATE;
                colCaption = Resources.ExportShukkaMeisai_AccessionDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 受入担当者
                colName = Def_T_SHUKKA_MEISAI.UKEIRE_USER_NAME;
                colCaption = Resources.ExportShukkaMeisai_AccessionUser;
                expInfoCollection.Add(colName, colCaption);
                // 2015/11/18 H.Tajimi Free1,Free2を列追加
                // Free1
                colName = Def_T_SHUKKA_MEISAI.FREE1;
                colCaption = Resources.ExportShukkaMeisai_Free1;
                expInfoCollection.Add(colName, colCaption);
                // Free2
                colName = Def_T_SHUKKA_MEISAI.FREE2;
                colCaption = Resources.ExportShukkaMeisai_Free2;
                expInfoCollection.Add(colName, colCaption);
                // ↑
                // Maker
                colName = Def_T_TEHAI_MEISAI.MAKER;
                colCaption = Resources.ExportShukkaMeisai_Maker;
                expInfoCollection.Add(colName, colCaption);
                // 単価(JPY)

                //【Step15_1_2】出荷情報照会画面の権限制御 2022/10/17（TW-Tsuji）
                //
                //　単価（JPY）列の出力をフラグで切り替える.
                if (this.IsUnitPriceDisable == false)
                {
                    colName = Def_T_TEHAI_MEISAI.UNIT_PRICE;
                }
                else
                {
                    colName = "UNIT_PRICE_BLANK";
                }
                //---修正ここまで

                colCaption = Resources.ExportShukkaMeisai_UnitPrice;
                expInfoCollection.Add(colName, colCaption);
                // 2015/11/20 H.Tajimi 備考列追加
                colName = Def_T_SHUKKA_MEISAI.BIKO;
                colCaption = Resources.ExportShukkaMeisai_Memo;
                expInfoCollection.Add(colName, colCaption);
                // 通関確認状態列追加
                colName = Def_T_SHUKKA_MEISAI.CUSTOMS_STATUS;
                colCaption = Resources.ExportShukkaMeisai_CustomsStatus;
                expInfoCollection.Add(colName, colCaption);
                // ↑
                // 2015/12/09 H.Tajimi M_NO列追加
                colName = Def_T_SHUKKA_MEISAI.M_NO;
                colCaption = Resources.ExportShukkaMeisai_MNo;
                expInfoCollection.Add(colName, colCaption);
                // ↑
                // 2018/11/15 T.Nakata GRWT列追加
                colName = Def_T_SHUKKA_MEISAI.GRWT;
                colCaption = Resources.ExportShukkaMeisai_GRWT;
                expInfoCollection.Add(colName, colCaption);
                // ↑
                // 2019/08/09 H.Tajimi 写真列追加
                colName = ComDefine.FLD_EXISTS_PICTURE;
                colCaption = Resources.ExportShukkaMeisai_Picture;
                expInfoCollection.Add(colName, colCaption);
                // ↑
                // 2018/11/15 T.Nakata TEHAI_RENKEI_NO列追加
                colName = Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO;
                colCaption = Resources.ExportShukkaMeisai_TEHAI_RENKEI_NO;
                expInfoCollection.Add(colName, colCaption);
                // ↑
                // 期
                colName = Def_T_TEHAI_MEISAI.ECS_QUOTA;
                colCaption = Resources.ExportShukkaMeisai_EcsQuota;
                expInfoCollection.Add(colName, colCaption);
                // 手配区分名称
                colName = ComDefine.FLD_TEHAI_FLAG_NAME;
                colCaption = Resources.ExportShukkaMeisai_TehaiFlagName;
                expInfoCollection.Add(colName, colCaption);
                // 数量単位名称
                colName = ComDefine.FLD_QUANTITY_UNIT_NAME;
                colCaption = Resources.ExportShukkaMeisai_QuantityUnitName;
                expInfoCollection.Add(colName, colCaption);
                // 有償/無償名称
                colName = ComDefine.FLD_ESTIMATE_FLAG_NAME;
                colCaption = Resources.ExportShukkaMeisai_EstimateFlagName;
                expInfoCollection.Add(colName, colCaption);

                // 出荷元列追加による 2022/10/07（TW-Tsuji）
                //　Export用のリソース（SMS.E01のリソース）に表示名称を追加
                //　　
                colName = Def_M_SHIP_FROM.SHIP_FROM_NAME;               //マスタの定義名を使用
                colCaption = Resources.ExportShukkaMeisai_SipFromName;
                expInfoCollection.Add(colName, colCaption);
                //---修正ここまで

                // 2022/12/19 J.Chen TAG便名列追加
                colName = Def_T_SHUKKA_MEISAI.TAG_SHIP;
                colCaption = Resources.ExportShukkaMeisai_TagShip;
                expInfoCollection.Add(colName, colCaption);

                bool ret = this.ExportExcel(filePath, dt, expInfoCollection, out msgID, out args);
                if (ret)
                {
                    // 出荷明細Excelファイルを出力しました。
                    msgID = "E0100020001";
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
