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
    /// 手配明細Excel出力クラス
    /// </summary>
    /// <create>K.Tsutsumi 2019/03/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ExportTehaiMeisai : BaseExportXlsx
    {

        #region 単価表示プロパティ　2022/10/17
        //【Step_1_1】手配情報照会画面の権限制御 2022/10/17（TW-Tsuji）
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
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public ExportTehaiMeisai()
            : base()
        {
            //【Step_1_1】手配情報照会画面の権限制御 2022/10/17（TW-Tsuji）
            //
            //　単価（JPY）列の出力を制御するためのフラグをメンバに追加する.
            //　（デフォルトは false ）
            IsUnitPriceDisable = false;
        }

        #endregion

        #region 手配明細出力

        /// --------------------------------------------------
        /// <summary>
        /// 手配明細Excelの出力
        /// </summary>
        /// <param name="filePath">ファイル名</param>
        /// <param name="dt">出力データ</param>
        /// <param name="msgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update>K.Tsutsumi 2019/09/07 フィールド追加</update>
        /// <update>T.Nukaga 2019/11/18 返却品対応 返却品列追加</update>
        /// <update>2022/04/19 STEP14</update>
        /// <update>Y.Shioshi 2022/05/09 STEP14列追加対応</update>
        /// <update>J.Chen 2022/07/06 受入日と受入担当者非表示</update>
        /// <update>J.Chen 2024/10/29 変更履歴追加</update>
        /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
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
                // 手配連携No.
                colName = Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO;
                colCaption = Resources.ExportTehaiMeisai_TehaiRenkeiNo;
                expInfoCollection.Add(colName, colCaption);
                // 物件名
                colName = Def_M_BUKKEN.BUKKEN_NAME;
                colCaption = Resources.ExportTehaiMeisai_BukkenName;
                expInfoCollection.Add(colName, colCaption);
                // 設定納期
                colName = Def_T_TEHAI_MEISAI.SETTEI_DATE;
                colCaption = Resources.ExportTehaiMeisai_SetteiDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 納入状態
                colName = ComDefine.FLD_NONYU_JYOTAI;
                colCaption = Resources.ExportTehaiMeisai_NonyuJyotai;
                expInfoCollection.Add(colName, colCaption);
                // 納品先
                colName = Def_T_TEHAI_MEISAI.NOUHIN_SAKI;
                colCaption = Resources.ExportTehaiMeisai_NouhinSaki;
                expInfoCollection.Add(colName, colCaption);
                // 出荷先
                colName = Def_T_TEHAI_MEISAI.SYUKKA_SAKI;
                colCaption = Resources.ExportTehaiMeisai_SyukkaSaki;
                expInfoCollection.Add(colName, colCaption);
                // 製番
                colName = Def_M_ECS.SEIBAN;
                colCaption = Resources.ExportTehaiMeisai_Seiban;
                expInfoCollection.Add(colName, colCaption);
                // CODE
                colName = Def_M_ECS.CODE;
                colCaption = Resources.ExportTehaiMeisai_Code;
                expInfoCollection.Add(colName, colCaption);
                // 便/AR No.
                colName = ComDefine.FLD_SHIP_AR_NO;
                colCaption = Resources.ExportTehaiMeisai_Ship_ARNo;
                expInfoCollection.Add(colName, colCaption);
                // ECS No.
                colName = Def_T_TEHAI_MEISAI.ECS_NO;
                colCaption = Resources.ExportTehaiMeisai_EcsNo;
                expInfoCollection.Add(colName, colCaption);               
                // 追番
                colName = Def_T_TEHAI_MEISAI.ZUMEN_OIBAN;
                colCaption = Resources.ExportTehaiMeisai_ZumenOiban;
                expInfoCollection.Add(colName, colCaption);
                // Floor
                colName = Def_T_TEHAI_MEISAI.FLOOR;
                colCaption = Resources.ExportTehaiMeisai_Floor;
                expInfoCollection.Add(colName, colCaption);
                // 機種
                colName = Def_M_ECS.KISHU;
                colCaption = Resources.ExportTehaiMeisai_Kishu;
                expInfoCollection.Add(colName, colCaption);
                // ST-No.
                colName = Def_T_TEHAI_MEISAI.ST_NO;
                colCaption = Resources.ExportTehaiMeisai_STNo;
                expInfoCollection.Add(colName, colCaption);
                // 品名(和文)
                colName = Def_T_TEHAI_MEISAI.HINMEI_JP;
                colCaption = Resources.ExportTehaiMeisai_HinmeiJp;
                expInfoCollection.Add(colName, colCaption);
                // 品名
                colName = Def_T_TEHAI_MEISAI.HINMEI;
                colCaption = Resources.ExportTehaiMeisai_Hinmei;
                expInfoCollection.Add(colName, colCaption);
                // 品名(INV)
                colName = Def_T_TEHAI_MEISAI.HINMEI_INV;
                colCaption = Resources.ExportTehaiMeisai_HinmeiInv;
                expInfoCollection.Add(colName, colCaption);
                // 図面/形式
                colName = Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI;
                colCaption = Resources.ExportTehaiMeisai_ZumenKeishiki;
                expInfoCollection.Add(colName, colCaption);
                // 図番/型式2
                colName = Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI2;
                colCaption = Resources.ExportTehaiMeisai_ZumenKeishiki2;
                expInfoCollection.Add(colName, colCaption);
                // 区割No.
                colName = Def_T_SHUKKA_MEISAI.KUWARI_NO;
                colCaption = Resources.ExportTehaiMeisai_KuwariNo;
                expInfoCollection.Add(colName, colCaption);
                // 手配数
                colName = Def_T_TEHAI_MEISAI.TEHAI_QTY;
                colCaption = Resources.ExportTehaiMeisai_TehaiQty;
                expInfoCollection.Add(colName, colCaption);
                // Free1 -> 材質
                colName = Def_T_TEHAI_MEISAI.FREE1;
                colCaption = Resources.ExportTehaiMeisai_Free1;
                expInfoCollection.Add(colName, colCaption);
                // Free2 -> 原産国
                colName = Def_T_TEHAI_MEISAI.FREE2;
                colCaption = Resources.ExportTehaiMeisai_Free2;
                expInfoCollection.Add(colName, colCaption);
                // 重量
                colName = Def_T_SHUKKA_MEISAI.GRWT;
                colCaption = Resources.ExportTehaiMeisai_GRWT;
                expInfoCollection.Add(colName, colCaption);
                // 数量単位
                colName = ComDefine.FLD_QUANTITY_UNIT_NAME;
                colCaption = Resources.ExportTehaiMeisai_QuantityUnitName;
                expInfoCollection.Add(colName, colCaption);
                // 備考1
                colName = Def_T_TEHAI_MEISAI.NOTE;
                colCaption = Resources.ExportTehaiMeisai_Note;
                expInfoCollection.Add(colName, colCaption);
                // 備考2
                colName = Def_T_TEHAI_MEISAI.NOTE2;
                colCaption = Resources.ExportTehaiMeisai_Note2;
                expInfoCollection.Add(colName, colCaption);
                // 備考3
                colName = Def_T_TEHAI_MEISAI.NOTE3;
                colCaption = Resources.ExportTehaiMeisai_Note3;
                expInfoCollection.Add(colName, colCaption);
                // 通関確認状態
                colName = Def_T_TEHAI_MEISAI.CUSTOMS_STATUS;
                colCaption = Resources.ExportTehaiMeisai_CustomsStatus;
                expInfoCollection.Add(colName, colCaption);
                // Maker
                colName = Def_T_TEHAI_MEISAI.MAKER;
                colCaption = Resources.ExportTehaiMeisai_Maker;
                expInfoCollection.Add(colName, colCaption);
                // 単価(JPY)

                //【Step_1_1】手配情報照会画面の権限制御 2022/10/17（TW-Tsuji）
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

                colCaption = Resources.ExportTehaiMeisai_UnitPrice;
                expInfoCollection.Add(colName, colCaption);
                // 手配No.
                colName = Def_T_TEHAI_MEISAI_SKS.TEHAI_NO;
                colCaption = Resources.ExportTehaiMeisai_TehaiNo;
                expInfoCollection.Add(colName, colCaption);
                // 手配区分
                colName = ComDefine.FLD_TEHAI_FLAG_NAME;
                colCaption = Resources.ExportTehaiMeisai_TehaiFlagName;
                expInfoCollection.Add(colName, colCaption);
                // 入荷状態
                colName = ComDefine.FLD_TEHAI_NYUKA_FLAG_NAME;
                colCaption = Resources.ExportTehaiMeisai_TehaiNyukaFlagName;
                expInfoCollection.Add(colName, colCaption);
                // 組立状態
                colName = ComDefine.FLD_TEHAI_ASSY_FLAG_NAME;
                colCaption = Resources.ExportTehaiMeisai_TehaiAssyFlagName;
                expInfoCollection.Add(colName, colCaption);
                // TAG登録状況
                colName = ComDefine.FLD_TEHAI_TAG_TOUROKU_FLAG_NAME;
                colCaption = Resources.ExportTehaiMeisai_TehaiTagTourokuFlagName;
                expInfoCollection.Add(colName, colCaption);
                // 出荷状態
                colName = ComDefine.FLD_TEHAI_SYUKKA_FLAG_NAME;
                colCaption = Resources.ExportTehaiMeisai_SyukkaFlagName;
                expInfoCollection.Add(colName, colCaption);
                // 有償
                colName = ComDefine.FLD_ESTIMATE_FLAG_NAME;
                colCaption = Resources.ExportTehaiMeisai_EstimateFlagName;
                expInfoCollection.Add(colName, colCaption);
                // PONo.
                colName = Def_T_TEHAI_ESTIMATE.PO_NO;
                colCaption = Resources.ExportTehaiMeisai_PONo;
                expInfoCollection.Add(colName, colCaption);
                // 状態
                colName = ComDefine.FLD_JYOTAI_NAME;
                colCaption = Resources.ExportTehaiMeisai_JyotaiName;
                expInfoCollection.Add(colName, colCaption);
                // 出荷日
                colName = Def_T_SHUKKA_MEISAI.SHUKKA_DATE;
                colCaption = Resources.ExportTehaiMeisai_ShukkaDate;
                expInfoCollection.Add(colName, colCaption);
                // TagNo.
                colName = Def_T_SHUKKA_MEISAI.TAG_NO;
                colCaption = Resources.ExportTehaiMeisai_TagNo;
                expInfoCollection.Add(colName, colCaption);
                // Area
                colName = Def_T_SHUKKA_MEISAI.AREA;
                colCaption = Resources.ExportTehaiMeisai_Area;
                expInfoCollection.Add(colName, colCaption);
                // M-No.
                colName = Def_T_SHUKKA_MEISAI.M_NO;
                colCaption = Resources.ExportTehaiMeisai_MNo;
                expInfoCollection.Add(colName, colCaption);
                // 集荷日
                colName = Def_T_SHUKKA_MEISAI.SHUKA_DATE;
                colCaption = Resources.ExportTehaiMeisai_ShukaDate;
                expInfoCollection.Add(colName, colCaption);
                // BoxNo.
                colName = Def_T_SHUKKA_MEISAI.BOX_NO;
                colCaption = Resources.ExportTehaiMeisai_BoxNo;
                expInfoCollection.Add(colName, colCaption);
                // Box梱包日
                colName = Def_T_SHUKKA_MEISAI.BOXKONPO_DATE;
                colCaption = Resources.ExportTehaiMeisai_BoxPackingDate;
                expInfoCollection.Add(colName, colCaption);
                // PalletNo.
                colName = Def_T_SHUKKA_MEISAI.PALLET_NO;
                colCaption = Resources.ExportTehaiMeisai_PalletNo;
                expInfoCollection.Add(colName, colCaption);
                // Pallet梱包日
                colName = Def_T_SHUKKA_MEISAI.PALLETKONPO_DATE;
                colCaption = Resources.ExportTehaiMeisai_PalletPackingDate;
                expInfoCollection.Add(colName, colCaption);
                // 木枠便
                colName = ComDefine.FLD_KIWAKU_NO;
                colCaption = Resources.ExportTehaiMeisai_WoodFrameShip;
                expInfoCollection.Add(colName, colCaption);
                // C/No
                colName = Def_T_KIWAKU_MEISAI.CASE_NO;
                colCaption = Resources.ExportTehaiMeisai_CNo;
                expInfoCollection.Add(colName, colCaption);
                // 木枠梱包日
                colName = Def_T_SHUKKA_MEISAI.KIWAKUKONPO_DATE;
                colCaption = Resources.ExportTehaiMeisai_WoodFramePackingDate;
                expInfoCollection.Add(colName, colCaption);
                // 運送会社
                colName = Def_T_SHUKKA_MEISAI.UNSOKAISHA_NAME;
                colCaption = Resources.ExportTehaiMeisai_ShippingCompany;
                expInfoCollection.Add(colName, colCaption);
                // INVOICENo.
                colName = Def_T_SHUKKA_MEISAI.INVOICE_NO;
                colCaption = Resources.ExportTehaiMeisai_InvoiceNo;
                expInfoCollection.Add(colName, colCaption);
                // 送り状No.
                colName = Def_T_SHUKKA_MEISAI.OKURIJYO_NO;
                colCaption = Resources.ExportTehaiMeisai_OkurijyoNo;
                expInfoCollection.Add(colName, colCaption);
                // BLNo.
                colName = Def_T_SHUKKA_MEISAI.BL_NO;
                colCaption = Resources.ExportTehaiMeisai_BLNo;
                expInfoCollection.Add(colName, colCaption);
                //// 受入日
                //colName = Def_T_SHUKKA_MEISAI.UKEIRE_DATE;
                //colCaption = Resources.ExportTehaiMeisai_AcceptanceDate;
                //expInfoCollection.Add(colName, colCaption);
                //// 受入担当者
                //colName = Def_T_SHUKKA_MEISAI.UKEIRE_USER_NAME;
                //colCaption = Resources.ExportTehaiMeisai_AcceptanceStaff;
                //expInfoCollection.Add(colName, colCaption);
                // 期
                colName = Def_T_TEHAI_MEISAI.ECS_QUOTA;
                colCaption = Resources.ExportTehaiMeisai_EcsQuota;
                expInfoCollection.Add(colName, colCaption);
                // 写真
                colName = ComDefine.FLD_EXISTS_PICTURE;
                colCaption = Resources.ExportTehaiMeisai_Photo;
                expInfoCollection.Add(colName, colCaption);
                // 変更履歴
                colName = Def_T_TEHAI_MEISAI.TEHAI_RIREKI;
                colCaption = Resources.ExportTehaiMeisai_TehaiRireki;
                expInfoCollection.Add(colName, colCaption);
                // 返却品
                colName = ComDefine.FLD_HENKYAKUHIN_FLAG_NAME;
                colCaption = Resources.ExportTehaiMeisai_HenkyakuhinFlag;
                expInfoCollection.Add(colName, colCaption);
                // 登録者
                colName = Def_T_TEHAI_MEISAI.CREATE_USER_NAME;
                colCaption = Resources.ExportTehaiMeisai_CreateUserName;
                expInfoCollection.Add(colName, colCaption);
                // 登録日時
                colName = Def_T_TEHAI_MEISAI.CREATE_DATE;
                colCaption = Resources.ExportTehaiMeisai_CreateDate;
                expInfoCollection.Add(colName, colCaption);

                bool ret = this.ExportExcel(filePath, dt, expInfoCollection, out msgID, out args);
                if (ret)
                {
                    // 手配明細Excelファイルを出力しました。
                    msgID = "E0100150001";
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
