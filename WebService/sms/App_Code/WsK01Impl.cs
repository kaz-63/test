using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;

using Condition;
using Commons;
using DSWUtil;
using DSWUtil.DbUtil;

//// --------------------------------------------------
/// <summary>
/// 集荷作業処理（データアクセス層） 
/// </summary>
/// <create>Y.Higuchi 2010/06/23</create>
/// <update></update>
/// --------------------------------------------------
public class WsK01Impl : WsBaseImpl
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/06/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsK01Impl()
        : base()
    {
    }

    #endregion

    #region チェック振り分け 制御
    /// --------------------------------------------------
    /// <summary>
    /// 管理Ｎｏの取得・ＡＲＮｏのチェック・出荷明細データの取得を行います
    /// </summary>
    /// <param name="dbHelper">ＤＢヘルパー</param>
    /// <param name="cond">コンディション</param>
    /// <param name="ds">出荷明細データ</param>
    /// <param name="errorMsgID">エラーＩＤ</param>
    /// <param name="kangiNo">管理Ｎｏ(納入先コード)</param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/07/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetShukaData(DatabaseHelper dbHelper, CondK01 condk01, out DataSet ds, out string errorMsgID, out string kanriNo)
    {
        try
        {
            // 初期化
            ds = new DataSet();
            errorMsgID = string.Empty;
            kanriNo = string.Empty;

            WsSmsImpl sms = new WsSmsImpl();
            CondSms cond = new CondSms(condk01.LoginInfo);

            cond.ShukkaFlag = condk01.ShukkaFlag;
            cond.NonyusakiCD = condk01.NounyusakiCD;
            cond.ARNo = condk01.ARNo;

            // 2011/03/09 K.Tsutsumi Change T_ARが存在しなくても続行可能
            //bool result = sms.CheckNonyusakiAndARNo(dbHelper, cond, out errorMsgID, out kanriNo, out ds);          
            DataSet dsSms = new DataSet();
            bool result = sms.CheckNonyusakiAndARNo(dbHelper, cond, false, out errorMsgID, out kanriNo, out dsSms);
            // ↑

            if (result)
            {
                // 2011/03/09 K.Tsutsumi Add T_ARが存在しなくても続行可能
                if ((string.IsNullOrEmpty(cond.ARNo) == false) && (ComFunc.IsExistsData(dsSms, Def_T_AR.Name) == true))
                {
                    ds.Tables.Add(dsSms.Tables[Def_T_AR.Name].Copy());
                }
                // ↑

                // 2011/03/09 K.Tsutsumi Change 進捗件数取得
                //ds = this.GetPickupData(dbHelper, condk01, kanriNo);
                //if (!ds.Tables.Contains(Def_T_SHUKKA_MEISAI.Name) || ds.Tables[Def_T_SHUKKA_MEISAI.Name].Rows.Count < 1)
                //{
                //    // 該当する明細は存在しません。
                //    errorMsgID = "A9999999022";
                //    return false;
                //}

                condk01.NounyusakiCD = kanriNo;
                DataSet dsProgress = this.GetProgress(dbHelper, condk01);
                if (ComFunc.IsExistsData(dsProgress, ComDefine.DTTBL_PROGRESS) == false)
                {
                    // 進捗件数が取得できませんでした。
                    errorMsgID = "K0100010006";
                    return false;
                }
                ds.Tables.Add(dsProgress.Tables[ComDefine.DTTBL_PROGRESS].Copy());

                DataSet dsShukka = this.GetPickupData(dbHelper, condk01, kanriNo);
                if (!dsShukka.Tables.Contains(Def_T_SHUKKA_MEISAI.Name) || dsShukka.Tables[Def_T_SHUKKA_MEISAI.Name].Rows.Count < 1)
                {
                    // 該当する明細は存在しません。
                    errorMsgID = "A9999999022";
                    return false;
                }
                ds.Tables.Add(dsShukka.Tables[Def_T_SHUKKA_MEISAI.Name].Copy());
                // ↑
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region K0100010:集荷開始

    #region SELECT

    #region 出荷明細データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データを取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/07/01</create>
    /// <update>K.Tsutsumi 2014/11/08 Free1, Free2対応</update>
    /// <update>H.Tajimi 2015/11/20 備考対応</update>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update>D.Okumura 2020/10/26 EFA_SMS-150 QRコードのSHIP欄にARNoを出力する対応:AR_NO/QRCODEを追加</update>
    /// <update>H.Iimuro 2022/10/04 図面/型式２の追加対応</update>
    /// <update>H.Iimuro 2022/12/19 TAG便名追加</update>
    /// <update>J.Chen 2023/02/22 手配No追加</update>
    /// <update>J.Chen 2024/08/08 発行制限対応（手配連携No、PONo、有償フラグ追加）</update>
    /// <update>J.Chen 2024/11/08 通関確認状態追加</update>
    /// --------------------------------------------------
    public DataSet GetPickupData(DatabaseHelper dbHelper, CondK01 cond, string kanrino)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            string fieldName = string.Empty;
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("        1 AS INSATU");
            sb.ApdL("      , COM1.ITEM_NAME AS JYOTAI");
            sb.ApdL("      , TSM.TAG_NO");
            sb.ApdL("      , TSM.SEIBAN");
            sb.ApdL("      , TSM.CODE");
            sb.ApdL("      , TSM.ZUMEN_OIBAN");
            sb.ApdL("      , TSM.AREA");
            sb.ApdL("      , '' AS AREA_1");
            sb.ApdL("      , '' AS AREA_2");
            sb.ApdL("      , TSM.FLOOR");
            sb.ApdL("      , '' AS FLOOR_1");
            sb.ApdL("      , '' AS FLOOR_2");
            sb.ApdL("      , TSM.KISHU");
            sb.ApdL("      , TSM.ST_NO");
            sb.ApdL("      , TSM.HINMEI_JP");
            sb.ApdL("      , TSM.HINMEI");
            sb.ApdL("      , TSM.ZUMEN_KEISHIKI");
            sb.ApdL("      , TSM.ZUMEN_KEISHIKI2");
            sb.ApdL("      , TSM.KUWARI_NO");
            sb.ApdL("      , TSM.NUM");
            sb.ApdL("      , TSM.SHUKA_DATE");
            sb.ApdL("      , TSM.BOX_NO");
            sb.ApdL("      , TSM.PALLET_NO");
            sb.ApdL("      , TSM.SHUKKA_FLAG");
            sb.ApdL("      , BK.BUKKEN_NAME AS NONYUSAKI_NAME");
            sb.ApdL("      , '' AS NONYUSAKI_NAME_1");
            sb.ApdL("      , '' AS NONYUSAKI_NAME_2");
            sb.ApdL("      , TSM.TAG_NONYUSAKI_CD AS NONYUSAKI_CD");
            sb.ApdL("      , NONYU.SHIP");
            sb.ApdL("      , '' AS BERCODE");
            sb.ApdL("      , TSM.JYOTAI_FLAG");
            sb.ApdL("      , TSM.FREE1");
            sb.ApdL("      , TSM.FREE2");
            sb.ApdL("      , TSM.BIKO");
            sb.ApdL("      , TSM.CUSTOMS_STATUS");
            sb.ApdL("      , TSM.M_NO");
            sb.ApdN("      , (CASE WHEN TMZK.ZUMEN_KEISHIKI IS NOT NULL THEN ").ApdN(this.BindPrefix).ApdN("REPORT_CHECK_VALUE").ApdL(" END) AS PHOTO");
            sb.ApdL("      , TSM.AR_NO");
            sb.ApdL("      , '' AS QRCODE");

            // 出荷元列追加による 2022/10/11（TW-Tsuji）
            //　外部結合されている納入先マスタ（M_NONYUSAKI)　に出荷元（M_SHIP_FROM）を更に外部結合
            //　　出荷元CD、名称を取得
            //　　　SHIP_FROM_NO    出荷目CD（表示しないが項目抽出） 
            //　　　SHIP_FROM_NAME  名称　  （出荷元として表示する）
            sb.ApdL("      , MSF.SHIP_FROM_NO");
            sb.ApdL("      , MSF.SHIP_FROM_NAME");

            // TAG便名
            sb.ApdL("      , TSM.TAG_SHIP");

            // 手配No
            sb.ApdL("      , (");
            sb.ApdL("          SELECT");
            sb.ApdL("                  T_TEHAI_MEISAI_SKS.TEHAI_NO + ' '");
            sb.ApdL("            FROM");
            sb.ApdL("                  T_TEHAI_MEISAI_SKS");
            sb.ApdL("           WHERE");
            sb.ApdL("                  T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO = TSM.TEHAI_RENKEI_NO");
            sb.ApdL("             FOR XML PATH ('')");
            sb.ApdL("         ) AS TEHAI_NO");
            sb.ApdL("      , '' AS TEHAINO_1");
            sb.ApdL("      , '' AS TEHAINO_2");
            sb.ApdL("      , TSM.TEHAI_RENKEI_NO");
            sb.ApdL("      , TE.PO_NO");
            sb.ApdL("      , TM.ESTIMATE_FLAG");

            sb.ApdL("  FROM ");
            sb.ApdL("       T_SHUKKA_MEISAI TSM");
            sb.ApdL("  LEFT JOIN M_COMMON COM1 ON COM1.GROUP_CD = 'DISP_JYOTAI_FLAG'");
            sb.ApdL("                         AND COM1.VALUE1 = TSM.JYOTAI_FLAG");
            sb.ApdN("                         AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_NONYUSAKI NONYU ON NONYU.NONYUSAKI_CD = TSM.TAG_NONYUSAKI_CD");
            sb.ApdL("                         AND NONYU.SHUKKA_FLAG = TSM.SHUKKA_FLAG");

            // 出荷元列追加による 2022/10/11（TW-Tsuji）
            //　外部結合されている納入先マスタ（M_NONYUSAKI)　に出荷元（M_SHIP_FROM）を更に外部結合
            //　 抽出条件は、未使用フラグが"使用"　定数：UNUSED_FLAG.USED_VALUE1　'0'
            sb.ApdL("  LEFT JOIN M_SHIP_FROM MSF ON MSF.SHIP_FROM_NO = NONYU.SHIP_FROM_CD");
            sb.ApdN("                         AND MSF.UNUSED_FLAG = ").ApdN(this.BindPrefix).ApdL("UNUSED_FLAG");



            sb.ApdL("  LEFT JOIN M_BUKKEN BK ON BK.SHUKKA_FLAG = NONYU.SHUKKA_FLAG");
            sb.ApdL("                       AND BK.BUKKEN_NO = NONYU.BUKKEN_NO");
            sb.ApdL("  LEFT JOIN T_MANAGE_ZUMEN_KEISHIKI TMZK ON TMZK.ZUMEN_KEISHIKI = TSM.ZUMEN_KEISHIKI");
            sb.ApdL("  LEFT JOIN T_TEHAI_MEISAI TM ON TM.TEHAI_RENKEI_NO = TSM.TEHAI_RENKEI_NO");
            sb.ApdL("  LEFT JOIN T_TEHAI_ESTIMATE TE ON TE.ESTIMATE_NO = TM.ESTIMATE_NO");

            sb.ApdL(" WHERE ");
            sb.ApdL("       1 = 1");

            if (!string.IsNullOrEmpty(cond.ShukkaFlag))
            {
                fieldName = "SHUKKA_FLAG";
                sb.ApdN("   AND TSM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.ShukkaFlag));
            }

            if (!string.IsNullOrEmpty(kanrino))
            {
                fieldName = "NONYUSAKI_CD";
                sb.ApdN("   AND TSM.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, kanrino));
            }

            if (!string.IsNullOrEmpty(cond.Seiban))
            {
                fieldName = "SEIBAN";
                sb.ApdN("   AND TSM.SEIBAN = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.Seiban));
            }

            if (!string.IsNullOrEmpty(cond.Code))
            {
                fieldName = "CODE";
                sb.ApdN("   AND TSM.CODE = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.Code));
            }

            if (!string.IsNullOrEmpty(cond.ARNo))
            {
                fieldName = "AR_NO";
                sb.ApdN("   AND TSM.AR_NO = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.ARNo));
            }

            if (!string.IsNullOrEmpty(cond.DisplaySelect))
            {
                if (cond.DisplaySelect != DISP_SELECT_SYUKA.ALL_VALUE1)
                {

                    if (cond.DisplaySelect == DISP_SELECT_SYUKA.MISHUKA_VALUE1)
                    {
                        // 未集荷
                        sb.ApdL("   AND TSM.SHUKA_DATE IS NULL");
                    }
                    else if (cond.DisplaySelect == DISP_SELECT_SYUKA.BOXZUMI_VALUE1)
                    {
                        // B梱包済
                        fieldName = "JYOTAI_FLAG";
                        sb.ApdN("   AND TSM.JYOTAI_FLAG = ").ApdN(this.BindPrefix).ApdL(fieldName);
                        paramCollection.Add(iNewParameter.NewDbParameter(fieldName, JYOTAI_FLAG.BOXZUMI_VALUE1));
                    }
                    else if (cond.DisplaySelect == DISP_SELECT_SYUKA.PALLETZUMI_VALUE1)
                    {
                        // P梱包済
                        fieldName = "JYOTAI_FLAG";
                        sb.ApdN("   AND TSM.JYOTAI_FLAG = ").ApdN(this.BindPrefix).ApdL(fieldName);
                        paramCollection.Add(iNewParameter.NewDbParameter(fieldName, JYOTAI_FLAG.PALLETZUMI_VALUE1));
                    }
                    else if (cond.DisplaySelect == DISP_SELECT_SYUKA.KIWAKUKONPO_VALUE1)
                    {
                        // 木枠梱包済
                        fieldName = "JYOTAI_FLAG";
                        sb.ApdN("   AND TSM.JYOTAI_FLAG = ").ApdN(this.BindPrefix).ApdL(fieldName);
                        paramCollection.Add(iNewParameter.NewDbParameter(fieldName, JYOTAI_FLAG.KIWAKUKONPO_VALUE1));
                    }
                    else
                    {
                        // TAG未発行
                        sb.ApdL("   AND TSM.TAGHAKKO_DATE IS NULL");
                    }
                }
            }
            // TAGNO順
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TSM.TAG_NO");

            // バインド変数設定
            paramCollection.Add(iNewParameter.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParameter.NewDbParameter("REPORT_CHECK_VALUE", ComDefine.REPORT_CHECK_VALUE));

            // 出荷元列追加による 2022/10/11（TW-Tsuji）
            //　出荷元マスタの抽出条件をバインド変数設定
            //　抽出条件は、未使用フラグが"使用"　定数：UNUSED_FLAG.USED_VALUE1　'0'
            paramCollection.Add(iNewParameter.NewDbParameter("UNUSED_FLAG", UNUSED_FLAG.USED_VALUE1));
  

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_SHUKKA_MEISAI.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 納入先、便 or ARNo単位の進捗件数取得
    /// --------------------------------------------------
    /// <summary>
    /// 納入先、便 or ARNo単位の進捗件数取得
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <returns></returns>
    /// <create>K.Tsutsumi 2011/03/09</create>
    /// <update>R.Kubota 2023/12/06 「引渡済」のTAG状態集計を追加</update>
    /// <update>J.Chen 2024/10/02 「引渡/集荷済」のTAG状態集計を追加</update>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetProgress(DatabaseHelper dbHelper, CondK01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       COUNT(1) AS COUNT_ALL");
            sb.ApdL("     , COUNT(SHUKA_DATE) AS COUNT_SHUKA");
            sb.ApdL("     , COUNT(BOXKONPO_DATE) AS COUNT_BOXKONPO");
            sb.ApdL("     , COUNT(PALLETKONPO_DATE) AS COUNT_PALLETKONPO");
            sb.ApdL("     , COUNT(HIKIWATASHI_DATE) AS COUNT_HIKIWATASHI");
            sb.ApdL("     , COUNT(CASE WHEN HIKIWATASHI_DATE IS NOT NULL AND SHUKA_DATE IS NULL THEN 1 END) AS COUNT_HIKIWATASHI_NO_SHUKA");
            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");
            sb.ApdN("   AND SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            if (string.IsNullOrEmpty(cond.ARNo) == false)
            {
                sb.ApdN("   AND AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");
                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("AR_NO", cond.ARNo));
            }

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NounyusakiCD));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, ComDefine.DTTBL_PROGRESS);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region INSERT

    #region 履歴データ追加
    /// --------------------------------------------------
    /// <summary>
    /// 履歴データの追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>J.Chen 2024/10/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsRireki(DatabaseHelper dbHelper, CondK01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_RIREKI");
            sb.ApdL("(");
            sb.ApdL("       GAMEN_FLAG");
            sb.ApdL("     , SHUKKA_FLAG");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , SHIP");
            sb.ApdL("     , AR_NO");
            sb.ApdL("     , BUKKEN_NO");
            sb.ApdL("     , OPERATION_FLAG");
            sb.ApdL("     , UPDATE_PC_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("GAMEN_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHIP");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("AR_NO");
            sb.ApdN("     , (SELECT BUKKEN_NO FROM M_NONYUSAKI WHERE NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("        AND SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG)");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("OPERATION_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_PC_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdL(")");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("GAMEN_FLAG", GAMEN_FLAG.K0100010_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NounyusakiCD));
            if (cond.Ship == null)
            {
                cond.Ship = string.Empty;
            }
            paramCollection.Add(iNewParam.NewDbParameter("SHIP", cond.Ship));
            string arNo = string.Empty;
            if (cond.ARNo != null)
            {
                arNo = cond.ARNo;
            }
            paramCollection.Add(iNewParam.NewDbParameter("AR_NO", arNo));
            paramCollection.Add(iNewParam.NewDbParameter("OPERATION_FLAG", cond.OperationFlag));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_PC_NAME", cond.UpdatePCName));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

            // SQL実行
            record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            return record;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    #endregion

    #endregion

    #region UPDATE

    /// --------------------------------------------------
    /// <summary>
    /// 発行日付のUPDATE
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <param name="dtTagNo">JYOTAI_FLAG:アップデートする状態区分,TAG_NO:アップデートするTAGNo</param>
    /// <create>H.Tsunamura 2010/08/10</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdMeisai(DatabaseHelper dbHelper, CondK01 cond, DataTable dtTagNo)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            string fieldName = string.Empty;
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_SHUKKA_MEISAI ");
            sb.ApdL("SET");
            sb.ApdN("       JYOTAI_FLAG = CASE WHEN JYOTAI_FLAG = 0 THEN 1 ELSE JYOTAI_FLAG END");
            sb.ApdL("     , TAGHAKKO_FLAG = 1");
            sb.ApdN("     , TAGHAKKO_DATE = ").ApdN(this.BindPrefix).ApdL("NOWDATE");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE ");
            sb.ApdN("       TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");
            if (!string.IsNullOrEmpty(cond.ShukkaFlag))
            {
                sb.ApdN("   AND SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            }
            if (!string.IsNullOrEmpty(cond.NounyusakiCD))
            {
                sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            }

            foreach (DataRow dr in dtTagNo.Rows)
            {
                paramCollection = new DbParamCollection();
                paramCollection.Add(iNewParameter.NewDbParameter("NOWDATE", cond.UpdateDate == null ? null : UtilConvert.ToDateTime(cond.UpdateDate).ToShortDateString()));
                paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_ID", this.GetCreateUserID(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_NAME", this.GetCreateUserName(cond)));

                if (!string.IsNullOrEmpty(cond.ShukkaFlag))
                {
                    paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
                }
                if (!string.IsNullOrEmpty(cond.NounyusakiCD))
                {
                    paramCollection.Add(iNewParameter.NewDbParameter("NONYUSAKI_CD", cond.NounyusakiCD));
                }

                // バインド変数設定
                paramCollection.Add(dbHelper.NewDbParameter("TAG_NO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.TAG_NO, "")));

                // SQL実行
                record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            }

            return record;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// ロック/解除状態のUPDATE
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <param name="ret">trueでロック、falseで解除</param>
    /// <create>T.SASAYAMA 2023/06/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public void LockUnLock(DatabaseHelper dbHelper, CondK01 cond, bool ret)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();

            // SQL文
            sb.ApdL(" UPDATE ");
            sb.ApdL("       M_NONYUSAKI");
            sb.ApdL(" SET ");
            sb.ApdL("       LOCK_FLAG = ");
            sb.ApdL(ret ? "1" : "0");  // retがtrueの場合は1を、falseの場合は0を設定
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE ");
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NonyusakiCD");

            // バインド変数設定
            paramCollection.Add(dbHelper.NewDbParameter("NonyusakiCD", cond.NounyusakiCD));
            paramCollection.Add(dbHelper.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(dbHelper.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_NONYUSAKI.Name);

            return;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

}
