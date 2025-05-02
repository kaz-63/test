using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;

using Condition;
using Commons;
using DSWUtil;
using DSWUtil.DbUtil;

/// --------------------------------------------------
/// <summary>
/// 製番用共通処理クラス（データアクセス層）
/// </summary>
/// <create>Y.Higuchi 2010/04/15</create>
/// <update></update>
/// --------------------------------------------------
public class WsSmsImpl : WsBaseImpl
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/04/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsSmsImpl()
        : base()
    {
    }

    #endregion

    #region 採番処理

    #region SELECT

    #region 共通

    /// --------------------------------------------------
    /// <summary>
    /// 採番取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <param name="saiban">取得した採番の値</param>
    /// <param name="errorMsgID">エラーメッセージID</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/06/28</create>
    /// <update>D.Okumura 2019/07/30 AR進捗対応</update>
    /// <update>H.Tajimi 2020/04/14 荷姿表対応</update>
    /// <update>J.Chen 2024/08/06 ARメール添付ファイル対応（番号取得のみ、更新なし処理追加）</update>
    /// --------------------------------------------------
    public bool GetSaiban(DatabaseHelper dbHelper, CondSms cond, out string saiban, out string errorMsgID)
    {
        try
        {
            bool ret = false;
            saiban = string.Empty;
            errorMsgID = string.Empty;

            if (SAIBAN_FLAG.US_VALUE1.Equals(cond.SaibanFlag))
            {
                // 本体US(本体・納入先コード)
                ret = this.GetSaiban_US(dbHelper, cond, out saiban, out errorMsgID);
            }
            else if (SAIBAN_FLAG.ARUS_VALUE1.Equals(cond.SaibanFlag))
            {
                // ARUS(AR・納入先コード)
                ret = this.GetSaiban_ARUS(dbHelper, cond, out saiban, out errorMsgID);
            }
            else if (SAIBAN_FLAG.BOX_NO_VALUE1.Equals(cond.SaibanFlag))
            {
                // BOXNo.
                ret = this.GetSaiban_BoxNo(dbHelper, cond, out saiban, out errorMsgID);
            }
            else if (SAIBAN_FLAG.PALLET_NO_VALUE1.Equals(cond.SaibanFlag))
            {
                // パレットNo.
                ret = this.GetSaiban_PalletNo(dbHelper, cond, out saiban, out errorMsgID);
            }
            else if (SAIBAN_FLAG.KOJI_NO_VALUE1.Equals(cond.SaibanFlag))
            {
                // 工事識別管理No.
                ret = this.GetSaiban_KojiNo(dbHelper, cond, out saiban, out errorMsgID);
            }
            else if (SAIBAN_FLAG.CASE_ID_VALUE1.Equals(cond.SaibanFlag))
            {
                // 木枠明細データの内部管理用キー
                ret = this.GetSaiban_CaseID(dbHelper, cond, out saiban, out errorMsgID);
            }
            else if (SAIBAN_FLAG.AR_NO_VALUE1.Equals(cond.SaibanFlag))
            {
                if (cond.UpdateFlag)
                {
                    // AR No.
                    ret = this.GetSaiban_ARNo(dbHelper, cond, out saiban, out errorMsgID);
                }
                else 
                {
                    // AR No.（取得のみ）
                    ret = this.GetSaiban_ARNoWithoutUpdate(dbHelper, cond, out saiban, out errorMsgID);
                }
            }
            else if (SAIBAN_FLAG.TEMP_ID_VALUE1.Equals(cond.SaibanFlag))
            {
                // 一時取込ID
                ret = this.GetSaiban_TempID(dbHelper, cond, out saiban, out errorMsgID);
            }
            else if (SAIBAN_FLAG.BKUS_VALUE1.Equals(cond.SaibanFlag))
            {
                // 本体物件(本体・物件管理No.)
                ret = this.GetSaiban_BKUS(dbHelper, cond, out saiban, out errorMsgID);
            }
            else if (SAIBAN_FLAG.BKARUS_VALUE1.Equals(cond.SaibanFlag))
            {
                // ＡＲ物件(AR・物件管理No.)
                ret = this.GetSaiban_BKARUS(dbHelper, cond, out saiban, out errorMsgID);
            }
            else if (SAIBAN_FLAG.ZAIKO_TEMP_ID_VALUE1.Equals(cond.SaibanFlag))
            {
                // 在庫一時取込ID
                ret = this.GetSaiban_ZaikoTempID(dbHelper, cond, out saiban, out errorMsgID);
            }
            else if (SAIBAN_FLAG.MAIL_VALUE1.Equals(cond.SaibanFlag))
            {
                // メールヘッダID
                ret = this.GetSaiban_MailHeaderID(dbHelper, cond, out saiban, out errorMsgID);
            }
            else if (SAIBAN_FLAG.MAIL_ID_VALUE1.Equals(cond.SaibanFlag))
            {
                if (cond.UpdateFlag)
                {
                    // メールID
                    ret = this.GetSaiban_MailID(dbHelper, cond, out saiban, out errorMsgID);
                }
                else 
                {
                    // メールID（取得のみ）
                    ret = this.GetSaiban_MailIDWithoutUpdate(dbHelper, cond, out saiban, out errorMsgID);
                }
            }
            else if (SAIBAN_FLAG.UNSOKAISHA_NO_VALUE1.Equals(cond.SaibanFlag))
            {
                // 運送会社CD
                ret = this.GetSaiban_UnsokaishaNo(dbHelper, cond, out saiban, out errorMsgID);
            }
            else if (SAIBAN_FLAG.TEHAI_RENKEI_VALUE1.Equals(cond.SaibanFlag))
            { 
                // 手配連携No
                ret = this.GetSaiban_TehaiRenkeiNo(dbHelper, cond, out saiban, out errorMsgID);
            }
            else if (SAIBAN_FLAG.CONSIGN_CD_VALUE1.Equals(cond.SaibanFlag))
            {
                // 荷受CD
                ret = this.GetSaiban_ConsignCD(dbHelper, cond, out saiban, out errorMsgID);
            }
            else if (SAIBAN_FLAG.DELIVER_CD_VALUE1.Equals(cond.SaibanFlag))
            {
                // 配送先CD
                ret = this.GetSaiban_DeliverCD(dbHelper, cond, out saiban, out errorMsgID);
            }
            else if (SAIBAN_FLAG.ESTIMATE_NO_VALUE1.Equals(cond.SaibanFlag))
            {
                // 見積No
                ret = this.GetSaiban_EstimateNo(dbHelper, cond, out saiban, out errorMsgID);
            }
            else if (SAIBAN_FLAG.PACKING_NO_VALUE1.Equals(cond.SaibanFlag))
            {
                // 荷姿CD
                ret = this.GetSaiban_PackingNo(dbHelper, cond, out saiban, out errorMsgID);
            }
            else if (SAIBAN_FLAG.PROJECT_NO_VALUE1.Equals(cond.SaibanFlag))
            {
                // ProjectNo
                ret = this.GetSaiban_ProjectNo(dbHelper, cond, out saiban, out errorMsgID);
            }
            else if (SAIBAN_FLAG.TEMP_AR_GOKI_VALUE1.Equals(cond.SaibanFlag))
            {
                // 号機一覧一時取込ID
                ret = this.GetSaiban_GokiTempID(dbHelper, cond, out saiban, out errorMsgID);
            }
            else if (SAIBAN_FLAG.ASSY_NO_VALUE1.Equals(cond.SaibanFlag))
            {
                // Assy no.
                ret = this.GetSaiban_AssyNo(dbHelper, cond, out saiban, out errorMsgID);
            }
            else if (SAIBAN_FLAG.SHIP_FROM_CD_VALUE1.Equals(cond.SaibanFlag))
            {
                // 出荷元CD
                ret = this.GetSaiban_ShipFromCD(dbHelper, cond, out saiban, out errorMsgID);
            }


            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 採番マスタ取得(行ロック)

    /// --------------------------------------------------
    /// <summary>
    /// 採番マスタ取得(行ロック)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="saibanCD">採番コード</param>
    /// <returns></returns>
    /// <create>Y.Higuchi 2010/06/28</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetSaibanData(DatabaseHelper dbHelper, string saibanCD)
    {
        try
        {
            DataTable dt = new DataTable(Def_M_SAIBAN.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("       SAIBAN_CD");
            sb.ApdL("     , CURRENT_NO");
            sb.ApdL("     , MINVALUE");
            sb.ApdL("     , MAXVALUE");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("  FROM ");
            sb.ApdL("       M_SAIBAN");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL(" WHERE ");
            sb.ApdN("       SAIBAN_CD = ").ApdN(this.BindPrefix).ApdL("SAIBAN_CD");

            // バインド変数設定
            paramCollection.Add(iNewParameter.NewDbParameter("SAIBAN_CD", saibanCD));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    #endregion

    #region 納入先コード取得

    #region 本体US採番

    /// --------------------------------------------------
    /// <summary>
    /// 本体の納入先コード採番
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <param name="saiban">取得した採番の値</param>
    /// <param name="errorMsgID">エラーメッセージID</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/06/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetSaiban_US(DatabaseHelper dbHelper, CondSms cond, out string saiban, out string errorMsgID)
    {
        try
        {
            saiban = string.Empty;
            errorMsgID = string.Empty;

            // 採番データ取得
            DataTable dt = this.GetSaibanData(dbHelper, SAIBAN_FLAG.US_VALUE1);

            if (dt == null || dt.Rows.Count < 1)
            {
                // 使用可能な納入先の管理No.がありません。
                errorMsgID = "A9999999005";
                return false;
            }

            decimal currentNo = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.CURRENT_NO);
            // 現在番号以上の空き番号を取得する。
            decimal nextNo = GetSaiban_Nonyusaki_Akiban(dbHelper, SHUKKA_FLAG.NORMAL_VALUE1, currentNo);
            // 最小、最大値のチェック
            decimal minValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MINVALUE);
            decimal maxValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MAXVALUE);

            if (nextNo < minValue || maxValue < nextNo)
            {
                nextNo = GetSaiban_Nonyusaki_Akiban(dbHelper, SHUKKA_FLAG.NORMAL_VALUE1, minValue);
                if (nextNo < minValue || maxValue < nextNo)
                {
                    // 使用可能な納入先の管理No.がありません。
                    errorMsgID = "A9999999005";
                    return false;
                }
            }
            saiban = ConvertBase36(nextNo);

            // 次の取得番号セット
            nextNo++;
            if (nextNo < minValue || maxValue < nextNo)
            {
                nextNo = minValue;
            }
            this.UpdSaiban(dbHelper, SAIBAN_FLAG.US_VALUE1, nextNo, cond);

            return true;
        }
        catch (Exception ex)
        {
            saiban = string.Empty;
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region ARUS採番

    /// --------------------------------------------------
    /// <summary>
    /// ARの納入先コード採番
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <param name="saiban">取得した採番の値</param>
    /// <param name="errorMsgID">エラーメッセージID</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/06/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetSaiban_ARUS(DatabaseHelper dbHelper, CondSms cond, out string saiban, out string errorMsgID)
    {
        try
        {
            saiban = string.Empty;
            errorMsgID = string.Empty;

            // 採番データ取得
            DataTable dt = this.GetSaibanData(dbHelper, SAIBAN_FLAG.ARUS_VALUE1);

            if (dt == null || dt.Rows.Count < 1)
            {
                // 使用可能な納入先の管理No.がありません。
                errorMsgID = "A9999999005";
                return false;
            }

            decimal currentNo = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.CURRENT_NO);
            // 現在番号以上の空き番号を取得する。
            decimal nextNo = GetSaiban_Nonyusaki_Akiban(dbHelper, SHUKKA_FLAG.AR_VALUE1, currentNo);
            // 最小、最大値のチェック
            decimal minValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MINVALUE);
            decimal maxValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MAXVALUE);

            if (nextNo < minValue || maxValue < nextNo)
            {
                nextNo = GetSaiban_Nonyusaki_Akiban(dbHelper, SHUKKA_FLAG.AR_VALUE1, minValue);
                if (nextNo < minValue || maxValue < nextNo)
                {
                    // 使用可能な納入先の管理No.がありません。
                    errorMsgID = "A9999999005";
                    return false;
                }
            }
            saiban = ConvertBase36(nextNo);

            // 次の取得番号セット
            nextNo++;
            if (nextNo < minValue || maxValue < nextNo)
            {
                nextNo = minValue;
            }
            this.UpdSaiban(dbHelper, SAIBAN_FLAG.ARUS_VALUE1, nextNo, cond);

            return true;
        }
        catch (Exception ex)
        {
            saiban = string.Empty;
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 共通

    /// <summary>
    /// 10進数を36進数に変換
    /// </summary>
    /// <returns></returns>
    private string ConvertBase36(decimal num)
    {
        string ret = string.Empty;

        // 取得No.の36進表記取得
        decimal quotient = num;
        decimal remainder = 0;
        while (quotient > 0)
        {
            remainder = quotient % 36;
            if (remainder < 10)
                ret = remainder.ToString() + ret;
            else
                ret = Encoding.ASCII.GetString(new byte[] { byte.Parse((remainder + 55).ToString()) }) + ret;

            quotient = Math.Floor(quotient / 36);
        }

        return ret.PadLeft(4, '0');
    }

    /// --------------------------------------------------
    /// <summary>
    /// 空き番号取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="shukkaFlag">出荷区分</param>
    /// <param name="currentNo">現在番号</param>
    /// <returns>次の番号</returns>
    /// <create>Y.Higuchi 2010/06/28</create>
    /// <update></update>
    /// --------------------------------------------------
    private decimal GetSaiban_Nonyusaki_Akiban(DatabaseHelper dbHelper, string shukkaFlag, decimal currentNo)
    {
        try
        {
            decimal ret = 0;
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // 一時表の存在確認
            sb.ApdL("IF OBJECT_ID('tempdb..#TEMP_NONYUSAKI') IS NULL");
            sb.ApdL("BEGIN");

            // 一時表作成
            sb.ApdL("CREATE TABLE #TEMP_NONYUSAKI (");
            sb.ApdL("       NONYUSAKI_CD DECIMAL(10) NOT NULL");
            sb.ApdL(")");

            // 一時表にインデックス作成
            sb.ApdL("CREATE NONCLUSTERED INDEX IX_1 ON #TEMP_NONYUSAKI");
            sb.ApdL("(");
            sb.ApdL("       NONYUSAKI_CD ASC");
            sb.ApdL(")");

            // 一時表にデータ挿入
            sb.ApdL("INSERT INTO #TEMP_NONYUSAKI (");
            sb.ApdL("       NONYUSAKI_CD");
            sb.ApdL(")");
            sb.ApdL("SELECT SMS.convertDecimal(NONYUSAKI_CD) AS NONYUSAKI_CD");
            sb.ApdL("  FROM M_NONYUSAKI");
            sb.ApdN(" WHERE SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD >= SMS.convertBase36(").ApdN(this.BindPrefix).ApdL("CURRENT_NO)");

            // SQL文
            sb.ApdL("SELECT NONYU.EMPTYNO");
            sb.ApdL("  FROM (");
            // CURRENT_NOがそのまま使用できる場合
            sb.ApdN("SELECT ").ApdN(this.BindPrefix).ApdN("CURRENT_NO").ApdL(" AS EMPTYNO");
            sb.ApdL("  FROM #TEMP_NONYUSAKI ");
            sb.ApdN("HAVING MIN(NONYUSAKI_CD) > ").ApdN(this.BindPrefix).ApdL("CURRENT_NO");
            sb.ApdL("UNION ALL");
            // CURRENT_NOより大きい空き番号を探索する場合
            sb.ApdL("SELECT NONYUSAKI_CD + 1 AS EMPTYNO");
            sb.ApdL("  FROM #TEMP_NONYUSAKI MN1");
            sb.ApdN(" WHERE NOT EXISTS( ");
            sb.ApdL("       SELECT 1 ");
            sb.ApdL("         FROM #TEMP_NONYUSAKI MN2 ");
            sb.ApdL("        WHERE MN2.NONYUSAKI_CD = MN1.NONYUSAKI_CD + 1 ");
            sb.ApdL("       ) ");
            sb.ApdL("       ) NONYU ");
            sb.ApdN(" WHERE NONYU.EMPTYNO >= ").ApdN(this.BindPrefix).ApdL("CURRENT_NO");
            sb.ApdL(" ORDER BY NONYU.EMPTYNO");

            sb.ApdL("END");

            // バインド変数設定
            paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", shukkaFlag));
            paramCollection.Add(iNewParameter.NewDbParameter("CURRENT_NO", currentNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            if (dt != null)
            {
                if (0 < dt.Rows.Count)
                {
                    ret = ComFunc.GetFldToDecimal(dt, 0, "EMPTYNO", 0d);
                }
                else
                {
                    ret = currentNo;
                }
            }
            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region BoxNo採番

    /// --------------------------------------------------
    /// <summary>
    /// BoxNo採番
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <param name="saiban">取得した採番の値</param>
    /// <param name="errorMsgID">エラーメッセージID</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/06/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetSaiban_BoxNo(DatabaseHelper dbHelper, CondSms cond, out string saiban, out string errorMsgID)
    {
        try
        {
            saiban = string.Empty;
            errorMsgID = string.Empty;

            // 採番データ取得
            DataTable dt = this.GetSaibanData(dbHelper, SAIBAN_FLAG.BOX_NO_VALUE1);

            if (dt == null || dt.Rows.Count < 1)
            {
                // 使用可能なBoxNo.がありません。
                errorMsgID = "A9999999006";
                return false;
            }

            decimal currentNo = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.CURRENT_NO);
            // 現在番号以上の空き番号を取得する。
            decimal nextNo = GetSaiban_BoxNo_Akiban(dbHelper, currentNo);
            // 最小、最大値のチェック
            decimal minValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MINVALUE);
            decimal maxValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MAXVALUE);

            if (nextNo < minValue || maxValue < nextNo)
            {
                nextNo = GetSaiban_BoxNo_Akiban(dbHelper, minValue);
                if (nextNo < minValue || maxValue < nextNo)
                {
                    // 使用可能なBoxNo.がありません。
                    errorMsgID = "A9999999006";
                    return false;
                }
            }

            var ret = string.Empty;
            decimal baseNum = nextNo;

            Action<int> toAscii = radix =>
            {
                var tmp = Math.Floor(baseNum / radix);
                if (tmp < 10)
                {
                    ret += Encoding.ASCII.GetString(new byte[] { byte.Parse((tmp + 48).ToString()) });
                }
                else
                {
                    ret += Encoding.ASCII.GetString(new byte[] { byte.Parse((tmp + 55).ToString()) });
                }
                baseNum -= radix * tmp;
            };

            // 1桁目
            toAscii(36000);

            // 2桁目
            toAscii(1000);

            // 3桁目
            toAscii(100);

            // 4桁目
            toAscii(10);

            // 5桁目
            toAscii(1);

            saiban = ComDefine.PREFIX_BOXNO + ret;

            nextNo++;
            if (nextNo < minValue || maxValue < nextNo)
            {
                nextNo = minValue;
            }
            this.UpdSaiban(dbHelper, SAIBAN_FLAG.BOX_NO_VALUE1, nextNo, cond);

            return true;
        }
        catch (Exception ex)
        {
            saiban = string.Empty;
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// BoxNo管理データの空き番号取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="currentNo">現在番号</param>
    /// <returns>次の番号</returns>
    /// <create>D.Okumura 2021/07/30 EFA_SMS-212対応</create>
    /// <update></update>
    /// --------------------------------------------------
    private decimal GetSaiban_BoxNo_Akiban(DatabaseHelper dbHelper, decimal currentNo)
    {
        decimal? ret = 0;
        ret = GetSaiban_BoxNo_Akiban(dbHelper, currentNo, 1000);
        if (ret != null)
        {
            return ret.Value;
        }
        ret = GetSaiban_BoxNo_Akiban(dbHelper, currentNo, null);
        if (ret != null)
        {
            return ret.Value;
        }
        return currentNo;
    }

    /// --------------------------------------------------
    /// <summary>
    /// BoxNo管理データの空き番号取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="currentNo">現在番号</param>
    /// <param name="limit">検索範囲上限</param>
    /// <returns>次の番号(nullあり)</returns>
    /// <create>Y.Higuchi 2010/06/28</create>
    /// <update>D.Okumura 2021/06/29 EFA_SMS-208対応</update>
    /// <update>D.Okumura 2021/08/19 EFA_SMS-212対応空き番検索を実テーブルで行うように変更</update>
    /// --------------------------------------------------
    private decimal? GetSaiban_BoxNo_Akiban(DatabaseHelper dbHelper, decimal currentNo, decimal? limit)
    {
        try
        {
            decimal? ret = 0;
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // 未採番のデータを補正する
            sb.ApdL("UPDATE T_BOXLIST_MANAGE SET BOX_NO_NUM = SMS.getDecimal(SUBSTRING(BOX_NO,2,5)) WHERE BOX_NO_NUM IS NULL");

            dbHelper.ExecuteNonQuery(sb.ToString());

            sb = new StringBuilder();

            // SQL文 (最大10件、必ず件数を制限すること)
            sb.ApdL("SELECT TOP 10 N AS EMPTYNO");
            // 連番を取得
            sb.ApdN("FROM (SELECT SEQ.N + ").ApdN(this.BindPrefix).ApdN("CURRENT_NO").ApdL(" AS N FROM (SELECT 0 AS N ");
            sb.ApdN("  UNION ALL SELECT (ROW_NUMBER() OVER (ORDER BY BOX_NO_NUM)) AS N ");
            sb.ApdL("  FROM T_BOXLIST_MANAGE ) AS SEQ ");
            if (limit != null)
            {
                sb.ApdN("  WHERE SEQ.N < ").ApdN(this.BindPrefix).ApdL("RECORD_LIMIT");
                paramCollection.Add(iNewParameter.NewDbParameter("RECORD_LIMIT",  limit.Value));
            }
            sb.ApdL(") AS NUM");
            // 取得した行番号を元に、BOX番号を当てて、存在しない番号を取得する
            sb.ApdL("  WHERE NOT EXISTS (SELECT 1 FROM T_BOXLIST_MANAGE AS BOXLIST_MANAGE WHERE BOXLIST_MANAGE.BOX_NO_NUM = NUM.N )");
            sb.ApdL("ORDER BY N");

            // バインド変数設定
            paramCollection.Add(iNewParameter.NewDbParameter("CURRENT_NO", currentNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            if (dt != null)
            {
                if (0 < dt.Rows.Count)
                {
                    ret = ComFunc.GetFldToDecimal(dt, 0, "EMPTYNO", 0d);
                }
                else
                {
                    ret = null;
                }
            }
            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region パレットNo採番

    /// --------------------------------------------------
    /// <summary>
    /// パレットNo採番
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <param name="saiban">取得した採番の値</param>
    /// <param name="errorMsgID">エラーメッセージID</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/06/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetSaiban_PalletNo(DatabaseHelper dbHelper, CondSms cond, out string saiban, out string errorMsgID)
    {
        try
        {
            saiban = string.Empty;
            errorMsgID = string.Empty;

            // 採番データ取得
            DataTable dt = this.GetSaibanData(dbHelper, SAIBAN_FLAG.PALLET_NO_VALUE1);

            if (dt == null || dt.Rows.Count < 1)
            {
                // 使用可能なパレットNo.がありません。
                errorMsgID = "A9999999007";
                return false;
            }

            decimal currentNo = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.CURRENT_NO);
            // 現在番号以上の空き番号を取得する。
            decimal nextNo = GetSaiban_PalletNo_Akiban(dbHelper, currentNo);
            // 最小、最大値のチェック
            decimal minValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MINVALUE);
            decimal maxValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MAXVALUE);

            if (nextNo < minValue || maxValue < nextNo)
            {
                nextNo = GetSaiban_PalletNo_Akiban(dbHelper, minValue);
                if (nextNo < minValue || maxValue < nextNo)
                {
                    // 使用可能なパレットNo.がありません。
                    errorMsgID = "A9999999007";
                    return false;
                }
            }
            
            var ret = string.Empty;
            decimal baseNum = nextNo;

            Action<int> toAscii = radix =>
            {
                var tmp = Math.Floor(baseNum / radix);
                if (tmp < 10)
                {
                    ret += Encoding.ASCII.GetString(new byte[] { byte.Parse((tmp + 48).ToString()) });
                }
                else
                {
                    ret += Encoding.ASCII.GetString(new byte[] { byte.Parse((tmp + 55).ToString()) });
                }
                baseNum -= radix * tmp;
            };

            // 1桁目
            toAscii(36000);

            // 2桁目
            toAscii(1000);

            // 3桁目
            toAscii(100);

            // 4桁目
            toAscii(10);

            // 5桁目
            toAscii(1);

            saiban = ComDefine.PREFIX_PALLETNO + ret;

            nextNo++;
            if (nextNo < minValue || maxValue < nextNo)
            {
                nextNo = minValue;
            }
            this.UpdSaiban(dbHelper, SAIBAN_FLAG.PALLET_NO_VALUE1, nextNo, cond);

            return true;
        }
        catch (Exception ex)
        {
            saiban = string.Empty;
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// パレット管理データの空き番号取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="currentNo">現在番号</param>
    /// <returns>次の番号</returns>
    /// <create>D.Okumura 2021/08/19 EFA_SMS-212対応</create>
    /// <update></update>
    /// --------------------------------------------------
    private decimal GetSaiban_PalletNo_Akiban(DatabaseHelper dbHelper, decimal currentNo)
    {
        decimal? ret = 0;
        ret = GetSaiban_PalletNo_Akiban(dbHelper, currentNo, 1000);
        if (ret != null)
        {
            return ret.Value;
        }
        ret = GetSaiban_PalletNo_Akiban(dbHelper, currentNo, null);
        if (ret != null)
        {
            return ret.Value;
        }
        return currentNo;
    }

    /// --------------------------------------------------
    /// <summary>
    /// パレット管理データの空き番号取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="currentNo">現在番号</param>
    /// <param name="limit">検索範囲上限</param>
    /// <returns>次の番号(nullあり)</returns>
    /// <create>Y.Higuchi 2010/06/28</create>
    /// <update>D.Okumura 2021/06/29 EFA_SMS-208対応</update>
    /// <update>D.Okumura 2021/07/30 EFA_SMS-212対応空き番検索を実テーブルで行うように変更</update>
    /// --------------------------------------------------
    private decimal? GetSaiban_PalletNo_Akiban(DatabaseHelper dbHelper, decimal currentNo, decimal? limit)
    {
        try
        {
            decimal? ret = 0;
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // 未採番のデータを補正する
            sb.ApdL("UPDATE T_PALLETLIST_MANAGE SET PALLET_NO_NUM = SMS.getDecimal(SUBSTRING(PALLET_NO,2,5)) WHERE PALLET_NO_NUM IS NULL");

            dbHelper.ExecuteNonQuery(sb.ToString());

            sb = new StringBuilder();

            // SQL文 (最大10件、必ず件数を制限すること)
            sb.ApdL("SELECT TOP 10 N AS EMPTYNO");
            // 連番を取得
            sb.ApdN("FROM (SELECT SEQ.N + ").ApdN(this.BindPrefix).ApdN("CURRENT_NO").ApdL(" AS N FROM (SELECT 0 AS N ");
            sb.ApdN("  UNION ALL SELECT (ROW_NUMBER() OVER (ORDER BY PALLET_NO_NUM)) AS N ");
            sb.ApdL("  FROM T_PALLETLIST_MANAGE ) AS SEQ ");
            if (limit != null)
            {
                sb.ApdN("  WHERE SEQ.N < ").ApdN(this.BindPrefix).ApdL("RECORD_LIMIT");
                paramCollection.Add(iNewParameter.NewDbParameter("RECORD_LIMIT", limit.Value));
            }
            sb.ApdL(") AS NUM");
            // 取得した行番号を元に、パレット番号を当てて、存在しない番号を取得する
            sb.ApdL("  WHERE NOT EXISTS (SELECT 1 FROM T_PALLETLIST_MANAGE AS PALLET_MANAGE WHERE PALLET_MANAGE.PALLET_NO_NUM = NUM.N )");
            sb.ApdL("ORDER BY N");

            // バインド変数設定
            paramCollection.Add(iNewParameter.NewDbParameter("CURRENT_NO", currentNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            if (dt != null)
            {
                if (0 < dt.Rows.Count)
                {
                    ret = ComFunc.GetFldToDecimal(dt, 0, "EMPTYNO", 0d);
                }
                else
                {
                    ret = null;
                }
            }
            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 工事識別管理No採番

    /// --------------------------------------------------
    /// <summary>
    /// 工事識別管理No採番
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <param name="saiban">取得した採番の値</param>
    /// <param name="errorMsgID">エラーメッセージID</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/06/28</create>
    /// <update>K.Tsutsumi 2019/01/13 工事識別No.36進数化</update>
    /// --------------------------------------------------
    public bool GetSaiban_KojiNo(DatabaseHelper dbHelper, CondSms cond, out string saiban, out string errorMsgID)
    {
        try
        {
            saiban = string.Empty;
            errorMsgID = string.Empty;

            // 採番データ取得
            DataTable dt = this.GetSaibanData(dbHelper, SAIBAN_FLAG.KOJI_NO_VALUE1);

            if (dt == null || dt.Rows.Count < 1)
            {
                // 使用可能な工事識別No.の管理No.がありません。
                errorMsgID = "A9999999008";
                return false;
            }

            decimal currentNo = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.CURRENT_NO);
            // 現在番号以上の空き番号を取得する。
            decimal nextNo = GetSaiban_KojiNo_Akiban(dbHelper, currentNo);
            // 最小、最大値のチェック
            decimal minValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MINVALUE);
            decimal maxValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MAXVALUE);

            if (nextNo < minValue || maxValue < nextNo)
            {
                nextNo = GetSaiban_KojiNo_Akiban(dbHelper, minValue);
                if (nextNo < minValue || maxValue < nextNo)
                {
                    // 使用可能な工事識別No.の管理No.がありません。
                    errorMsgID = "A9999999008";
                    return false;
                }
            }
            saiban = ConvertBase36(nextNo);

            nextNo++;
            if (nextNo < minValue || maxValue < nextNo)
            {
                nextNo = minValue;
            }
            this.UpdSaiban(dbHelper, SAIBAN_FLAG.KOJI_NO_VALUE1, nextNo, cond);

            return true;
        }
        catch (Exception ex)
        {
            saiban = string.Empty;
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 木枠データの空き番号取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="currentNo">現在番号</param>
    /// <returns>次の番号</returns>
    /// <create>Y.Higuchi 2010/06/28</create>
    /// <update>K.Tsutsumi 2019/01/13 工事識別No.36進数化</update>
    /// --------------------------------------------------
    private decimal GetSaiban_KojiNo_Akiban(DatabaseHelper dbHelper, decimal currentNo)
    {
        try
        {
            decimal ret = 0;
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // 一時表の存在確認
            sb.ApdL("IF OBJECT_ID('tempdb..#TEMP_KIWAKU') IS NULL");
            sb.ApdL("BEGIN");

            // 一時表作成
            sb.ApdL("CREATE TABLE #TEMP_KIWAKU (");
            sb.ApdL("       KOJI_NO DECIMAL(10) NOT NULL");
            sb.ApdL(")");

            // 一時表にインデックス作成
            sb.ApdL("CREATE NONCLUSTERED INDEX IX_1 ON #TEMP_KIWAKU");
            sb.ApdL("(");
            sb.ApdL("       KOJI_NO ASC");
            sb.ApdL(")");

            // 一時表にデータ挿入
            sb.ApdL("INSERT INTO #TEMP_KIWAKU (");
            sb.ApdL("       KOJI_NO");
            sb.ApdL(")");
            sb.ApdL("SELECT SMS.convertDecimal(KOJI_NO) AS KOJI_NO");
            sb.ApdL("  FROM T_KIWAKU");
            sb.ApdN(" WHERE KOJI_NO >= SMS.convertBase36(").ApdN(this.BindPrefix).ApdL("CURRENT_NO)");

            // SQL文
            sb.ApdL("SELECT KIWAKU.EMPTYNO");
            sb.ApdL("  FROM (");
            // CURRENT_NOがそのまま使用できる場合
            sb.ApdN("SELECT ").ApdN(this.BindPrefix).ApdN("CURRENT_NO").ApdL(" AS EMPTYNO");
            sb.ApdL("  FROM #TEMP_KIWAKU ");
            sb.ApdN("HAVING MIN(KOJI_NO) > ").ApdN(this.BindPrefix).ApdL("CURRENT_NO");
            sb.ApdL("UNION ALL");
            // CURRENT_NOより大きい空き番号を探索する場合
            sb.ApdL("SELECT KOJI_NO + 1 AS EMPTYNO");
            sb.ApdL("  FROM #TEMP_KIWAKU TK1");
            sb.ApdN(" WHERE NOT EXISTS( ");
            sb.ApdL("       SELECT 1 ");
            sb.ApdL("         FROM #TEMP_KIWAKU TK2 ");
            sb.ApdL("        WHERE TK2.KOJI_NO = TK1.KOJI_NO + 1 ");
            sb.ApdL("       ) ");
            sb.ApdL("       ) KIWAKU ");
            sb.ApdN(" WHERE KIWAKU.EMPTYNO >= ").ApdN(this.BindPrefix).ApdL("CURRENT_NO");
            sb.ApdL(" ORDER BY KIWAKU.EMPTYNO");

            sb.ApdL("END");

            // バインド変数設定
            paramCollection.Add(iNewParameter.NewDbParameter("CURRENT_NO", currentNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            if (dt != null)
            {
                if (0 < dt.Rows.Count)
                {
                    ret = ComFunc.GetFldToDecimal(dt, 0, "EMPTYNO", 0d);
                }
                else
                {
                    ret = currentNo;
                }
            }
            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region AR No.採番

    /// --------------------------------------------------
    /// <summary>
    /// AR No.採番
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <param name="saiban">取得した採番の値</param>
    /// <param name="errorMsgID">エラーメッセージID</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/06/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetSaiban_ARNo(DatabaseHelper dbHelper, CondSms cond, out string saiban, out string errorMsgID)
    {
        try
        {
            saiban = string.Empty;
            errorMsgID = string.Empty;

            // 採番データ取得
            string saibanCD = cond.ARUS.PadLeft(4, '0') + cond.ListFlag;
            DataTable dt = this.GetSaibanData(dbHelper, saibanCD);

            if (dt == null || dt.Rows.Count < 1)
            {
                this.InsSaiban(dbHelper, cond, saibanCD, 1, 1, 999);
                dt = this.GetSaibanData(dbHelper, saibanCD);
            }

            if (dt == null || dt.Rows.Count < 1)
            {
                // 使用可能なAR No.がありません。
                errorMsgID = "A9999999009";
                return false;
            }

            decimal currentNo = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.CURRENT_NO);
            // 最小、最大値のチェック
            decimal minValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MINVALUE);
            decimal maxValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MAXVALUE);

            if (currentNo < minValue || maxValue < currentNo)
            {
                // 使用可能なAR No.がありません。
                errorMsgID = "A9999999009";
                return false;
            }
            saiban = ComDefine.PREFIX_ARNO + cond.ListFlag + currentNo.ToString().PadLeft(3, '0');

            this.UpdSaiban(dbHelper, saibanCD, ++currentNo, cond);

            return true;
        }
        catch (Exception ex)
        {
            saiban = string.Empty;
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 木枠明細(内部管理用キー)採番

    /// --------------------------------------------------
    /// <summary>
    /// 木枠明細(内部管理用キー)採番
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <param name="saiban">取得した採番の値</param>
    /// <param name="errorMsgID">エラーメッセージID</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/06/29</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetSaiban_CaseID(DatabaseHelper dbHelper, CondSms cond, out string saiban, out string errorMsgID)
    {
        try
        {
            saiban = string.Empty;
            errorMsgID = string.Empty;

            // 採番データ取得
            DataTable dt = this.GetSaibanData(dbHelper, SAIBAN_FLAG.CASE_ID_VALUE1);

            if (dt == null || dt.Rows.Count < 1)
            {
                // 使用可能な管理キーがありません。
                errorMsgID = "A9999999012";
                return false;
            }

            decimal currentNo = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.CURRENT_NO);
            DateTime updateDate = ComFunc.GetFldToDateTime(dt, 0, Def_M_SAIBAN.UPDATE_DATE);
            // 最小、最大値のチェック
            decimal minValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MINVALUE);
            decimal maxValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MAXVALUE);
            if (updateDate.Date < DateTime.Today)
            {
                currentNo = minValue;
            }

            if (currentNo < minValue || maxValue < currentNo)
            {
                // 使用可能な管理キーがありません。
                errorMsgID = "A9999999012";
                return false;
            }
            saiban = DateTime.Today.ToString("yyyyMMdd") + currentNo.ToString().PadLeft(4, '0');

            this.UpdSaiban(dbHelper, SAIBAN_FLAG.CASE_ID_VALUE1, ++currentNo, cond);

            return true;
        }
        catch (Exception ex)
        {
            saiban = string.Empty;
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 一時取込ID採番

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込ID採番
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <param name="saiban">取得した採番の値</param>
    /// <param name="errorMsgID">エラーメッセージID</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/06/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetSaiban_TempID(DatabaseHelper dbHelper, CondSms cond, out string saiban, out string errorMsgID)
    {
        try
        {
            saiban = string.Empty;
            errorMsgID = string.Empty;

            // 採番データ取得
            DataTable dt = this.GetSaibanData(dbHelper, SAIBAN_FLAG.TEMP_ID_VALUE1);

            if (dt == null || dt.Rows.Count < 1)
            {
                // 使用可能な取込IDがありません。
                errorMsgID = "A9999999010";
                return false;
            }

            decimal currentNo = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.CURRENT_NO);
            DateTime updateDate = ComFunc.GetFldToDateTime(dt, 0, Def_M_SAIBAN.UPDATE_DATE);
            // 最小、最大値のチェック
            decimal minValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MINVALUE);
            decimal maxValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MAXVALUE);
            if (updateDate.Date < DateTime.Today)
            {
                currentNo = minValue;
            }

            if (currentNo < minValue || maxValue < currentNo)
            {
                // 使用可能な取込IDがありません。
                errorMsgID = "A9999999010";
                return false;
            }
            saiban = DateTime.Today.ToString("yyyyMMdd") + currentNo.ToString().PadLeft(4, '0');

            this.UpdSaiban(dbHelper, SAIBAN_FLAG.TEMP_ID_VALUE1, ++currentNo, cond);


            return true;
        }
        catch (Exception ex)
        {
            saiban = string.Empty;
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 物件管理No取得

    #region 本体US採番

    /// --------------------------------------------------
    /// <summary>
    /// 本体の物件管理No採番
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <param name="saiban">取得した採番の値</param>
    /// <param name="errorMsgID">エラーメッセージID</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>K.Saeki 2012/03/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetSaiban_BKUS(DatabaseHelper dbHelper, CondSms cond, out string saiban, out string errorMsgID)
    {
        try
        {
            saiban = string.Empty;
            errorMsgID = string.Empty;

            // 採番データ取得
            DataTable dt = this.GetSaibanData(dbHelper, SAIBAN_FLAG.BKUS_VALUE1);

            if (dt == null || dt.Rows.Count < 1)
            {
                // 使用可能な物件管理No.がありません。
                errorMsgID = "A9999999049";
                return false;
            }

            decimal currentNo = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.CURRENT_NO);
            // 現在番号以上の空き番号を取得する。
            decimal nextNo = GetSaiban_Bukken_Akiban(dbHelper, SHUKKA_FLAG.NORMAL_VALUE1, currentNo);
            // 最小、最大値のチェック
            decimal minValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MINVALUE);
            decimal maxValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MAXVALUE);

            if (nextNo < minValue || maxValue < nextNo)
            {
                nextNo = GetSaiban_Bukken_Akiban(dbHelper, SHUKKA_FLAG.NORMAL_VALUE1, minValue);
                if (nextNo < minValue || maxValue < nextNo)
                {
                    // 使用可能な物件管理No.がありません。
                    errorMsgID = "A9999999049";
                    return false;
                }
            }
            saiban = nextNo.ToString();

            nextNo++;
            if (nextNo < minValue || maxValue < nextNo)
            {
                nextNo = minValue;
            }
            this.UpdSaiban(dbHelper, SAIBAN_FLAG.BKUS_VALUE1, nextNo, cond);

            return true;
        }
        catch (Exception ex)
        {
            saiban = string.Empty;
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region ARUS採番

    /// --------------------------------------------------
    /// <summary>
    /// ＡＲの物件管理No採番
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <param name="saiban">取得した採番の値</param>
    /// <param name="errorMsgID">エラーメッセージID</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>K.Saeki 2012/03/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetSaiban_BKARUS(DatabaseHelper dbHelper, CondSms cond, out string saiban, out string errorMsgID)
    {
        try
        {
            saiban = string.Empty;
            errorMsgID = string.Empty;

            // 採番データ取得
            DataTable dt = this.GetSaibanData(dbHelper, SAIBAN_FLAG.BKARUS_VALUE1);

            if (dt == null || dt.Rows.Count < 1)
            {
                // 使用可能な物件管理No.がありません。
                errorMsgID = "A9999999049";
                return false;
            }

            decimal currentNo = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.CURRENT_NO);
            // 現在番号以上の空き番号を取得する。
            decimal nextNo = GetSaiban_Bukken_Akiban(dbHelper, SHUKKA_FLAG.AR_VALUE1, currentNo);
            // 最小、最大値のチェック
            decimal minValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MINVALUE);
            decimal maxValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MAXVALUE);

            if (nextNo < minValue || maxValue < nextNo)
            {
                nextNo = GetSaiban_Bukken_Akiban(dbHelper, SHUKKA_FLAG.AR_VALUE1, minValue);
                if (nextNo < minValue || maxValue < nextNo)
                {
                    // 使用可能な物件管理No.がありません。
                    errorMsgID = "A9999999049";
                    return false;
                }
            }
            saiban = nextNo.ToString();

            nextNo++;
            if (nextNo < minValue || maxValue < nextNo)
            {
                nextNo = minValue;
            }
            this.UpdSaiban(dbHelper, SAIBAN_FLAG.BKARUS_VALUE1, nextNo, cond);

            return true;
        }
        catch (Exception ex)
        {
            saiban = string.Empty;
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 共通

    /// --------------------------------------------------
    /// <summary>
    /// 空き番号取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="shukkaFlag">出荷区分</param>
    /// <param name="currentNo">現在番号</param>
    /// <returns>次の番号</returns>
    /// <create>K.Saeki 2012/03/23</create>
    /// <update></update>
    /// --------------------------------------------------
    private decimal GetSaiban_Bukken_Akiban(DatabaseHelper dbHelper, string shukkaFlag, decimal currentNo)
    {
        try
        {
            decimal ret = 0;
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT BUKKEN.EMPTYNO");
            sb.ApdL("  FROM (");
            // 1取得用SQL
            sb.ApdN("SELECT ").ApdN(this.BindPrefix).ApdN("CURRENT_NO").ApdL(" AS EMPTYNO");
            sb.ApdL("  FROM M_BUKKEN ");
            sb.ApdN(" WHERE SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("HAVING MIN(BUKKEN_NO) > ").ApdN(this.BindPrefix).ApdL("CURRENT_NO");
            sb.ApdL("UNION ALL");
            // 空いている番号取得用SQL
            sb.ApdL("SELECT BUKKEN_NO + 1 AS EMPTYNO");
            sb.ApdL("  FROM M_BUKKEN MN1");
            sb.ApdN(" WHERE SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdL("   AND NOT EXISTS( ");
            sb.ApdL("       SELECT 1 ");
            sb.ApdL("         FROM M_BUKKEN MN2 ");
            sb.ApdL("        WHERE MN2.SHUKKA_FLAG = MN1.SHUKKA_FLAG ");
            sb.ApdL("          AND MN2.BUKKEN_NO = MN1.BUKKEN_NO + 1 ");
            sb.ApdL("       ) ");
            sb.ApdL("       ) BUKKEN ");
            sb.ApdN(" WHERE BUKKEN.EMPTYNO >= ").ApdN(this.BindPrefix).ApdL("CURRENT_NO");
            sb.ApdL(" ORDER BY BUKKEN.EMPTYNO");

            // バインド変数設定
            paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", shukkaFlag));
            paramCollection.Add(iNewParameter.NewDbParameter("CURRENT_NO", currentNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            if (dt != null)
            {
                if (0 < dt.Rows.Count)
                {
                    ret = ComFunc.GetFldToDecimal(dt, 0, "EMPTYNO", 0d);
                }
                else
                {
                    ret = currentNo;
                }
            }
            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region 在庫一時取込ID採番

    /// --------------------------------------------------
    /// <summary>
    /// 在庫一時取込ID採番
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <param name="saiban">取得した採番の値</param>
    /// <param name="errorMsgID">エラーメッセージID</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Wakamatsu 2010/08/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetSaiban_ZaikoTempID(DatabaseHelper dbHelper, CondSms cond, out string saiban, out string errorMsgID)
    {
        try
        {
            saiban = string.Empty;
            errorMsgID = string.Empty;

            // 採番データ取得
            DataTable dt = this.GetSaibanData(dbHelper, SAIBAN_FLAG.ZAIKO_TEMP_ID_VALUE1);

            if (dt == null || dt.Rows.Count < 1)
            {
                // 使用可能な取込IDがありません。
                errorMsgID = "A9999999010";
                return false;
            }

            decimal currentNo = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.CURRENT_NO);
            DateTime updateDate = ComFunc.GetFldToDateTime(dt, 0, Def_M_SAIBAN.UPDATE_DATE);
            // 最小、最大値のチェック
            decimal minValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MINVALUE);
            decimal maxValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MAXVALUE);
            if (updateDate.Date < DateTime.Today)
            {
                currentNo = minValue;
            }

            if (currentNo < minValue || maxValue < currentNo)
            {
                // 使用可能な取込IDがありません。
                errorMsgID = "A9999999010";
                return false;
            }
            saiban = DateTime.Today.ToString("yyyyMMdd") + currentNo.ToString().PadLeft(4, '0');

            this.UpdSaiban(dbHelper, SAIBAN_FLAG.ZAIKO_TEMP_ID_VALUE1, ++currentNo, cond);


            return true;
        }
        catch (Exception ex)
        {
            saiban = string.Empty;
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region メールヘッダID採番

    /// --------------------------------------------------
    /// <summary>
    /// メールヘッダID採番
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <param name="saiban">取得した採番の値</param>
    /// <param name="errorMsgID">エラーメッセージID</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Sakiori 2017/09/11</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetSaiban_MailHeaderID(DatabaseHelper dbHelper, CondSms cond, out string saiban, out string errorMsgID)
    {
        try
        {
            saiban = string.Empty;
            errorMsgID = string.Empty;

            // 採番データ取得
            DataTable dt = this.GetSaibanData(dbHelper, SAIBAN_FLAG.MAIL_VALUE1);

            if (dt == null || dt.Rows.Count < 1)
            {
                // 使用可能なメールヘッダIDがありません。
                errorMsgID = "A9999999054";
                return false;
            }

            decimal currentNo = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.CURRENT_NO);
            DateTime updateDate = ComFunc.GetFldToDateTime(dt, 0, Def_M_SAIBAN.UPDATE_DATE);
            // 最小、最大値のチェック
            decimal minValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MINVALUE);
            decimal maxValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MAXVALUE);
            if (updateDate.Date < DateTime.Today)
            {
                currentNo = minValue;
            }

            if (currentNo < minValue || maxValue < currentNo)
            {
                // 使用可能なメールヘッダIDがありません。
                errorMsgID = "A9999999054";
                return false;
            }
            saiban = DateTime.Today.ToString("yyyyMMdd") + currentNo.ToString().PadLeft(4, '0');

            this.UpdSaiban(dbHelper, SAIBAN_FLAG.MAIL_VALUE1, ++currentNo, cond);
            
            return true;
        }
        catch (Exception ex)
        {
            saiban = string.Empty;
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region メールID採番

    /// --------------------------------------------------
    /// <summary>
    /// メールID採番
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <param name="saiban">取得した採番の値</param>
    /// <param name="errorMsgID">エラーメッセージID</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>R.Katsuo 2017/09/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetSaiban_MailID(DatabaseHelper dbHelper, CondSms cond, out string saiban, out string errorMsgID)
    {
        try
        {
            saiban = string.Empty;
            errorMsgID = string.Empty;

            // 採番データ取得
            DataTable dt = this.GetSaibanData(dbHelper, SAIBAN_FLAG.MAIL_ID_VALUE1);

            if (dt == null || dt.Rows.Count < 1)
            {
                // 使用可能なMailIDがありません。
                errorMsgID = "A9999999055";
                return false;
            }

            decimal currentNo = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.CURRENT_NO);
            DateTime updateDate = ComFunc.GetFldToDateTime(dt, 0, Def_M_SAIBAN.UPDATE_DATE);
            // 最小、最大値のチェック
            decimal minValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MINVALUE);
            decimal maxValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MAXVALUE);
            if (updateDate.Date < DateTime.Today)
            {
                currentNo = minValue;
            }

            if (currentNo < minValue || maxValue < currentNo)
            {
                // 使用可能なMailIDがありません。
                errorMsgID = "A9999999055";
                return false;
            }
            saiban = DateTime.Today.ToString("yyyyMMdd") + currentNo.ToString().PadLeft(4, '0');

            this.UpdSaiban(dbHelper, SAIBAN_FLAG.MAIL_ID_VALUE1, ++currentNo, cond);

            return true;
        }
        catch (Exception ex)
        {
            saiban = string.Empty;
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 運送会社CD採番

    /// --------------------------------------------------
    /// <summary>
    /// 運送会社CD採番
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <param name="saiban">取得した採番の値</param>
    /// <param name="errorMsgID">エラーメッセージID</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Nakata 2018/11/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetSaiban_UnsokaishaNo(DatabaseHelper dbHelper, CondSms cond, out string saiban, out string errorMsgID)
    {
        try
        {
            saiban = string.Empty;
            errorMsgID = string.Empty;

            // 採番データ取得
            DataTable dt = this.GetSaibanData(dbHelper, SAIBAN_FLAG.UNSOKAISHA_NO_VALUE1);

            if (dt == null || dt.Rows.Count < 1)
            {
                // 使用可能な運送会社CDがありません。
                errorMsgID = "A9999999061";
                return false;
            }

            decimal currentNo = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.CURRENT_NO);
            DateTime updateDate = ComFunc.GetFldToDateTime(dt, 0, Def_M_SAIBAN.UPDATE_DATE);
            // 最小、最大値のチェック
            decimal minValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MINVALUE);
            decimal maxValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MAXVALUE);
            if (currentNo < minValue || maxValue < currentNo)
            {
                // 使用可能な運送会社CDがありません。
                errorMsgID = "A9999999061";
                return false;
            }
            saiban = ConvertBase36(currentNo).PadLeft(4, '0');

            this.UpdSaiban(dbHelper, SAIBAN_FLAG.UNSOKAISHA_NO_VALUE1, ++currentNo, cond);

            return true;
        }
        catch (Exception ex)
        {
            saiban = string.Empty;
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 手配連携No採番

    /// --------------------------------------------------
    /// <summary>
    /// 手配連携採番
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <param name="saiban">取得した採番の値</param>
    /// <param name="errorMsgID">エラーメッセージID</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>S.Furugo 2018/11/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetSaiban_TehaiRenkeiNo(DatabaseHelper dbHelper, CondSms cond, out string saiban, out string errorMsgID)
    {
        try
        {
            saiban = string.Empty;
            errorMsgID = string.Empty;

            // 採番データ取得
            DataTable dt = this.GetSaibanData(dbHelper, SAIBAN_FLAG.TEHAI_RENKEI_VALUE1);

            if (dt == null || dt.Rows.Count < 1)
            {
                // 使用可能な手配連携No.がありません。
                errorMsgID = "A9999999064";
                return false;
            }

            decimal currentNo = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.CURRENT_NO);
            DateTime updateDate = ComFunc.GetFldToDateTime(dt, 0, Def_M_SAIBAN.UPDATE_DATE);
            // 最小、最大値のチェック
            decimal minValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MINVALUE);
            decimal maxValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MAXVALUE);
            if (currentNo < minValue || maxValue < currentNo)
            {
                // 使用可能な手配連携No.がありません。
                errorMsgID = "A9999999064";
                return false;
            }
            saiban = ConvertBase36(currentNo).PadLeft(8, '0');

            this.UpdSaiban(dbHelper, SAIBAN_FLAG.TEHAI_RENKEI_VALUE1, ++currentNo, cond);

            return true;
        }
        catch (Exception ex)
        {
            saiban = string.Empty;
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 荷受CD採番

    /// --------------------------------------------------
    /// <summary>
    /// 荷受CD採番
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <param name="saiban">取得した採番の値</param>
    /// <param name="errorMsgID">エラーメッセージID</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/12/06</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetSaiban_ConsignCD(DatabaseHelper dbHelper, CondSms cond, out string saiban, out string errorMsgID)
    {
        try
        {
            saiban = string.Empty;
            errorMsgID = string.Empty;

            // 採番データ取得
            DataTable dt = this.GetSaibanData(dbHelper, SAIBAN_FLAG.CONSIGN_CD_VALUE1);

            if (dt == null || dt.Rows.Count < 1)
            {
                // 使用可能な荷受CDがありません。
                errorMsgID = "A9999999062";
                return false;
            }

            decimal currentNo = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.CURRENT_NO);
            DateTime updateDate = ComFunc.GetFldToDateTime(dt, 0, Def_M_SAIBAN.UPDATE_DATE);
            // 最小、最大値のチェック
            decimal minValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MINVALUE);
            decimal maxValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MAXVALUE);
            if (currentNo < minValue || maxValue < currentNo)
            {
                // 使用可能な荷受CDがありません。
                errorMsgID = "A9999999062";
                return false;
            }
            saiban = ConvertBase36(currentNo).PadLeft(4, '0');

            this.UpdSaiban(dbHelper, SAIBAN_FLAG.CONSIGN_CD_VALUE1, ++currentNo, cond);

            return true;
        }
        catch (Exception ex)
        {
            saiban = string.Empty;
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 配送先CD採番

    /// --------------------------------------------------
    /// <summary>
    /// 配送先CD採番
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <param name="saiban">取得した採番の値</param>
    /// <param name="errorMsgID">エラーメッセージID</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/12/10</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetSaiban_DeliverCD(DatabaseHelper dbHelper, CondSms cond, out string saiban, out string errorMsgID)
    {
        try
        {
            saiban = string.Empty;
            errorMsgID = string.Empty;

            // 採番データ取得
            DataTable dt = this.GetSaibanData(dbHelper, SAIBAN_FLAG.DELIVER_CD_VALUE1);

            if (dt == null || dt.Rows.Count < 1)
            {
                // 使用可能な配送先CDがありません。
                errorMsgID = "A9999999063";
                return false;
            }

            decimal currentNo = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.CURRENT_NO);
            DateTime updateDate = ComFunc.GetFldToDateTime(dt, 0, Def_M_SAIBAN.UPDATE_DATE);
            // 最小、最大値のチェック
            decimal minValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MINVALUE);
            decimal maxValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MAXVALUE);
            if (currentNo < minValue || maxValue < currentNo)
            {
                // 使用可能な配送先CDがありません。
                errorMsgID = "A9999999063";
                return false;
            }
            saiban = ConvertBase36(currentNo).PadLeft(4, '0');

            this.UpdSaiban(dbHelper, SAIBAN_FLAG.DELIVER_CD_VALUE1, ++currentNo, cond);

            return true;
        }
        catch (Exception ex)
        {
            saiban = string.Empty;
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 見積No採番

    /// --------------------------------------------------
    /// <summary>
    /// 見積No.採番
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <param name="saiban">取得した採番の値</param>
    /// <param name="errorMsgID">エラーメッセージID</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>S.Furugo 2018/11/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetSaiban_EstimateNo(DatabaseHelper dbHelper, CondSms cond, out string saiban, out string errorMsgID)
    {
        try
        {
            saiban = string.Empty;
            errorMsgID = string.Empty;
            string dtToday = DateTime.Today.ToString("yyyyMMdd");

            // 採番データ取得
            DataTable dt = this.GetSaibanData(dbHelper, SAIBAN_FLAG.ESTIMATE_NO_VALUE1);

            if (dt == null || dt.Rows.Count < 1)
            {
                // 使用可能な手配見積1No.がありません。
                errorMsgID = "A9999999066";
                return false;
            }
            string updateDate = ComFunc.GetFldToDateTime(dt, 0, Def_M_SAIBAN.UPDATE_DATE).ToString("yyyyMMdd");
            decimal currentNo = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.CURRENT_NO);

            if (dtToday != updateDate)
                currentNo = 1;

            // 最小、最大値のチェック
            decimal minValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MINVALUE);
            decimal maxValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MAXVALUE);
            if (currentNo < minValue || maxValue < currentNo)
            {
                // 使用可能な手配見積No.がありません。
                errorMsgID = "A9999999066";
                return false;
            }
            saiban = dtToday + currentNo.ToString().PadLeft(6, '0');

            this.UpdSaiban(dbHelper, SAIBAN_FLAG.ESTIMATE_NO_VALUE1, ++currentNo, cond);

            return true;
        }
        catch (Exception ex)
        {
            saiban = string.Empty;
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 荷姿CD採番

    /// --------------------------------------------------
    /// <summary>
    /// 荷姿CD採番
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <param name="saiban">取得した採番の値</param>
    /// <param name="errorMsgID">エラーメッセージID</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Nakata 2018/11/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetSaiban_PackingNo(DatabaseHelper dbHelper, CondSms cond, out string saiban, out string errorMsgID)
    {
        try
        {
            saiban = string.Empty;
            errorMsgID = string.Empty;

            // 採番データ取得
            DataTable dt = this.GetSaibanData(dbHelper, SAIBAN_FLAG.PACKING_NO_VALUE1);

            if (dt == null || dt.Rows.Count < 1)
            {
                // 使用可能な荷受CDがありません。
                errorMsgID = "A9999999062";
                return false;
            }

            decimal currentNo = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.CURRENT_NO);
            DateTime updateDate = ComFunc.GetFldToDateTime(dt, 0, Def_M_SAIBAN.UPDATE_DATE);
            // 最小、最大値のチェック
            decimal minValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MINVALUE);
            decimal maxValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MAXVALUE);
            if (currentNo < minValue || maxValue < currentNo)
            {
                // 使用可能な荷受CDがありません。
                errorMsgID = "A9999999062";
                return false;
            }
            saiban = ConvertBase36(currentNo).PadLeft(8, '0');

            this.UpdSaiban(dbHelper, SAIBAN_FLAG.PACKING_NO_VALUE1, ++currentNo, cond);

            return true;
        }
        catch (Exception ex)
        {
            saiban = string.Empty;
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region ProjectNo.採番

    /// --------------------------------------------------
    /// <summary>
    /// ProjectNo.採番
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <param name="saiban">取得した採番の値</param>
    /// <param name="errorMsgID">エラーメッセージID</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/12/18</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetSaiban_ProjectNo(DatabaseHelper dbHelper, CondSms cond, out string saiban, out string errorMsgID)
    {
        try
        {
            saiban = string.Empty;
            errorMsgID = string.Empty;

            // 採番データ取得
            DataTable dt = this.GetSaibanData(dbHelper, SAIBAN_FLAG.PROJECT_NO_VALUE1);

            if (dt == null || dt.Rows.Count < 1)
            {
                // 使用可能なProjectNo.がありません。
                errorMsgID = "A9999999068";
                return false;
            }

            decimal currentNo = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.CURRENT_NO);
            DateTime updateDate = ComFunc.GetFldToDateTime(dt, 0, Def_M_SAIBAN.UPDATE_DATE);
            // 最小、最大値のチェック
            decimal minValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MINVALUE);
            decimal maxValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MAXVALUE);
            if (currentNo < minValue || maxValue < currentNo)
            {
                // 使用可能なProjectNo.がありません。
                errorMsgID = "A9999999068";
                return false;
            }
            saiban = ConvertBase36(currentNo).PadLeft(4, '0');

            this.UpdSaiban(dbHelper, SAIBAN_FLAG.PROJECT_NO_VALUE1, ++currentNo, cond);

            return true;
        }
        catch (Exception ex)
        {
            saiban = string.Empty;
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 号機一覧一時取込ID採番

    /// --------------------------------------------------
    /// <summary>
    /// 号機一覧一時取込ID採番
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <param name="saiban">取得した採番の値</param>
    /// <param name="errorMsgID">エラーメッセージID</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>D.Okumura 2019/07/24</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetSaiban_GokiTempID(DatabaseHelper dbHelper, CondSms cond, out string saiban, out string errorMsgID)
    {
        try
        {
            saiban = string.Empty;
            errorMsgID = string.Empty;

            // 採番データ取得
            DataTable dt = this.GetSaibanData(dbHelper, SAIBAN_FLAG.TEMP_AR_GOKI_VALUE1);

            if (dt == null || dt.Rows.Count < 1)
            {
                // 使用可能な取込IDがありません。
                errorMsgID = "A9999999010";
                return false;
            }

            decimal currentNo = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.CURRENT_NO);
            DateTime updateDate = ComFunc.GetFldToDateTime(dt, 0, Def_M_SAIBAN.UPDATE_DATE);
            // 最小、最大値のチェック
            decimal minValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MINVALUE);
            decimal maxValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MAXVALUE);
            if (updateDate.Date < DateTime.Today)
            {
                currentNo = minValue;
            }

            if (currentNo < minValue || maxValue < currentNo)
            {
                // 使用可能な取込IDがありません。
                errorMsgID = "A9999999010";
                return false;
            }
            saiban = DateTime.Today.ToString("yyyyMMdd") + currentNo.ToString().PadLeft(4, '0');

            this.UpdSaiban(dbHelper, SAIBAN_FLAG.TEMP_AR_GOKI_VALUE1, ++currentNo, cond);


            return true;
        }
        catch (Exception ex)
        {
            saiban = string.Empty;
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region Assy no.採番

    /// --------------------------------------------------
    /// <summary>
    /// Assy no.採番
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <param name="saiban">取得した採番の値</param>
    /// <param name="errorMsgID">エラーメッセージID</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2019/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetSaiban_AssyNo(DatabaseHelper dbHelper, CondSms cond, out string saiban, out string errorMsgID)
    {
        try
        {
            saiban = string.Empty;
            errorMsgID = string.Empty;

            // 採番データ取得
            DataTable dt = this.GetSaibanData(dbHelper, SAIBAN_FLAG.ASSY_NO_VALUE1);

            if (dt == null || dt.Rows.Count < 1)
            {
                // 使用可能なAssy no.がありません。
                errorMsgID = "A9999999083";
                return false;
            }

            decimal currentNo = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.CURRENT_NO);
            // 現在番号以上の空き番号を取得する。
            decimal nextNo = GetSaiban_AssyNo_Akiban(dbHelper, currentNo);
            // 最小、最大値のチェック
            decimal minValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MINVALUE);
            decimal maxValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MAXVALUE);

            if (nextNo < minValue || maxValue < nextNo)
            {
                nextNo = GetSaiban_AssyNo_Akiban(dbHelper, minValue);
                if (nextNo < minValue || maxValue < nextNo)
                {
                    // 使用可能なAssy no.がありません。
                    errorMsgID = "A9999999083";
                    return false;
                }
            }
            saiban = ConvertBase36(nextNo).PadLeft(5, '0');

            // 次の取得番号セット
            nextNo++;
            if (nextNo < minValue || maxValue < nextNo)
            {
                nextNo = minValue;
            }
            this.UpdSaiban(dbHelper, SAIBAN_FLAG.ASSY_NO_VALUE1, nextNo, cond);

            return true;
        }
        catch (Exception ex)
        {
            saiban = string.Empty;
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 空き番号取得(Assy no.)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="currentNo">現在番号</param>
    /// <returns>次の番号</returns>
    /// <create>H.Tsuji 2019/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    private decimal GetSaiban_AssyNo_Akiban(DatabaseHelper dbHelper, decimal currentNo)
    {
        try
        {
            decimal assyLength = 5;
            decimal ret = 0;
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // 一時表の存在確認
            sb.ApdL("IF OBJECT_ID('tempdb..#TEMP_ASSY_NO') IS NULL");
            sb.ApdL("BEGIN");

            // 一時表作成
            sb.ApdL("CREATE TABLE #TEMP_ASSY_NO (");
            sb.ApdL("       ASSY_NO DECIMAL(13) NOT NULL");
            sb.ApdL(")");

            // 一時表にインデックス作成
            sb.ApdL("CREATE NONCLUSTERED INDEX IX_1 ON #TEMP_ASSY_NO");
            sb.ApdL("(");
            sb.ApdL("       ASSY_NO ASC");
            sb.ApdL(")");

            // 一時表にデータ挿入
            sb.ApdL("INSERT INTO #TEMP_ASSY_NO (");
            sb.ApdL("       ASSY_NO");
            sb.ApdL(")");
            sb.ApdN("SELECT SMS.convertDecimalEx(ASSY_NO, ").ApdN(this.BindPrefix).ApdL("ASSY_LENGTH) AS ASSY_NO");
            sb.ApdL("  FROM T_TEHAI_MEISAI");
            sb.ApdN(" WHERE ASSY_NO >= SMS.convertBase36Ex(").ApdN(this.BindPrefix).ApdN("CURRENT_NO, ").ApdN(this.BindPrefix).ApdL("ASSY_LENGTH)");

            // SQL文
            sb.ApdL("SELECT ASSY.EMPTYNO");
            sb.ApdL("  FROM (");
            // CURRENT_NOがそのまま使用できる場合
            sb.ApdN("SELECT ").ApdN(this.BindPrefix).ApdN("CURRENT_NO").ApdL(" AS EMPTYNO");
            sb.ApdL("  FROM #TEMP_ASSY_NO ");
            sb.ApdN("HAVING MIN(ASSY_NO) > ").ApdN(this.BindPrefix).ApdL("CURRENT_NO");
            sb.ApdL("UNION ALL");
            // CURRENT_NOより大きい空き番号を探索する場合
            sb.ApdL("SELECT ASSY_NO + 1 AS EMPTYNO");
            sb.ApdL("  FROM #TEMP_ASSY_NO TAN1");
            sb.ApdN(" WHERE NOT EXISTS( ");
            sb.ApdL("       SELECT 1 ");
            sb.ApdL("         FROM #TEMP_ASSY_NO TAN2 ");
            sb.ApdL("        WHERE TAN2.ASSY_NO = TAN1.ASSY_NO + 1 ");
            sb.ApdL("       ) ");
            sb.ApdL("       ) ASSY ");
            sb.ApdN(" WHERE ASSY.EMPTYNO >= ").ApdN(this.BindPrefix).ApdL("CURRENT_NO");
            sb.ApdL(" ORDER BY ASSY.EMPTYNO");

            sb.ApdL("END");

            // バインド変数設定
            paramCollection.Add(iNewParameter.NewDbParameter("CURRENT_NO", currentNo));
            paramCollection.Add(iNewParameter.NewDbParameter("ASSY_LENGTH", assyLength));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            if (dt != null)
            {
                if (0 < dt.Rows.Count)
                {
                    ret = ComFunc.GetFldToDecimal(dt, 0, "EMPTYNO", 0d);
                }
                else
                {
                    ret = currentNo;
                }
            }
            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 出荷元CD採番

    /// --------------------------------------------------
    /// <summary>
    /// 出荷元CD採番
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <param name="saiban">取得した採番の値</param>
    /// <param name="errorMsgID">エラーメッセージID</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tajimi 2020/04/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetSaiban_ShipFromCD(DatabaseHelper dbHelper, CondSms cond, out string saiban, out string errorMsgID)
    {
        try
        {
            saiban = string.Empty;
            errorMsgID = string.Empty;

            // 採番データ取得
            DataTable dt = this.GetSaibanData(dbHelper, SAIBAN_FLAG.SHIP_FROM_CD_VALUE1);

            if (dt == null || dt.Rows.Count < 1)
            {
                // 使用可能な配送先CDがありません。
                errorMsgID = "A9999999086";
                return false;
            }

            decimal currentNo = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.CURRENT_NO);
            DateTime updateDate = ComFunc.GetFldToDateTime(dt, 0, Def_M_SAIBAN.UPDATE_DATE);
            // 最小、最大値のチェック
            decimal minValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MINVALUE);
            decimal maxValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MAXVALUE);
            if (currentNo < minValue || maxValue < currentNo)
            {
                // 使用可能な配送先CDがありません。
                errorMsgID = "A9999999086";
                return false;
            }
            saiban = ConvertBase36(currentNo).PadLeft(4, '0');

            this.UpdSaiban(dbHelper, SAIBAN_FLAG.SHIP_FROM_CD_VALUE1, ++currentNo, cond);

            return true;
        }
        catch (Exception ex)
        {
            saiban = string.Empty;
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region メールID取得

    /// --------------------------------------------------
    /// <summary>
    /// メールID取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <param name="saiban">取得した採番の値</param>
    /// <param name="errorMsgID">エラーメッセージID</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>J.Chen 2024/08/06</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetSaiban_MailIDWithoutUpdate(DatabaseHelper dbHelper, CondSms cond, out string saiban, out string errorMsgID)
    {
        try
        {
            saiban = string.Empty;
            errorMsgID = string.Empty;

            // 採番データ取得
            DataTable dt = this.GetSaibanData(dbHelper, SAIBAN_FLAG.MAIL_ID_VALUE1);

            if (dt == null || dt.Rows.Count < 1)
            {
                // 使用可能なMailIDがありません。
                errorMsgID = "A9999999055";
                return false;
            }

            decimal currentNo = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.CURRENT_NO);
            DateTime updateDate = ComFunc.GetFldToDateTime(dt, 0, Def_M_SAIBAN.UPDATE_DATE);
            // 最小、最大値のチェック
            decimal minValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MINVALUE);
            decimal maxValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MAXVALUE);
            if (updateDate.Date < DateTime.Today)
            {
                currentNo = minValue;
            }

            if (currentNo < minValue || maxValue < currentNo)
            {
                // 使用可能なMailIDがありません。
                errorMsgID = "A9999999055";
                return false;
            }
            saiban = DateTime.Today.ToString("yyyyMMdd") + currentNo.ToString().PadLeft(4, '0');

            return true;
        }
        catch (Exception ex)
        {
            saiban = string.Empty;
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region AR No.取得

    /// --------------------------------------------------
    /// <summary>
    /// AR No.取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <param name="saiban">取得した採番の値</param>
    /// <param name="errorMsgID">エラーメッセージID</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>J.Chen 2024/08/06</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetSaiban_ARNoWithoutUpdate(DatabaseHelper dbHelper, CondSms cond, out string saiban, out string errorMsgID)
    {
        try
        {
            saiban = string.Empty;
            errorMsgID = string.Empty;

            // 採番データ取得
            string saibanCD = cond.ARUS.PadLeft(4, '0') + cond.ListFlag;
            DataTable dt = this.GetSaibanData(dbHelper, saibanCD);

            if (dt == null || dt.Rows.Count < 1)
            {
                this.InsSaiban(dbHelper, cond, saibanCD, 1, 1, 999);
                dt = this.GetSaibanData(dbHelper, saibanCD);
            }

            if (dt == null || dt.Rows.Count < 1)
            {
                // 使用可能なAR No.がありません。
                errorMsgID = "A9999999009";
                return false;
            }

            decimal currentNo = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.CURRENT_NO);
            // 最小、最大値のチェック
            decimal minValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MINVALUE);
            decimal maxValue = ComFunc.GetFldToDecimal(dt, 0, Def_M_SAIBAN.MAXVALUE);

            if (currentNo < minValue || maxValue < currentNo)
            {
                // 使用可能なAR No.がありません。
                errorMsgID = "A9999999009";
                return false;
            }
            saiban = ComDefine.PREFIX_ARNO + cond.ListFlag + currentNo.ToString().PadLeft(3, '0');

            return true;
        }
        catch (Exception ex)
        {
            saiban = string.Empty;
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region INSERT

    /// --------------------------------------------------
    /// <summary>
    /// ARNoの採番データをINSERT
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <returns>影響を及ぼしたレコード数</returns>
    /// <create>Y.Higuchi 2010/06/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsSaiban(DatabaseHelper dbHelper, CondSms cond, string saibanCD, decimal currentNo, decimal minValue, decimal maxValue)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO M_SAIBAN");
            sb.ApdL("(");
            sb.ApdL("       SAIBAN_CD");
            sb.ApdL("     , CURRENT_NO");
            sb.ApdL("     , MINVALUE");
            sb.ApdL("     , MAXVALUE");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("SAIBAN_CD");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CURRENT_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MINVALUE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MAXVALUE");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdL(")");

            // バインド変数設定
            paramCollection.Add(iNewParameter.NewDbParameter("SAIBAN_CD", saibanCD));
            paramCollection.Add(iNewParameter.NewDbParameter("CURRENT_NO", currentNo));
            paramCollection.Add(iNewParameter.NewDbParameter("MINVALUE", minValue));
            paramCollection.Add(iNewParameter.NewDbParameter("MAXVALUE", maxValue));
            paramCollection.Add(iNewParameter.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
            paramCollection.Add(iNewParameter.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
            paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_NAME", this.GetCreateUserName(cond)));

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

    #region UPDATE

    /// --------------------------------------------------
    /// <summary>
    /// 採番マスタ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="saibanCD">採番コード</param>
    /// <param name="currentNo">現在番号</param>
    /// <param name="cond">コンディション</param>
    /// <returns>影響を及ぼしたレコード数</returns>
    /// <create>Y.Higuchi 2010/06/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdSaiban(DatabaseHelper dbHelper, string saibanCD, decimal currentNo, CondSms cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("UPDATE M_SAIBAN ");
            sb.ApdL("SET");
            sb.ApdN("       CURRENT_NO = ").ApdN(this.BindPrefix).ApdL("CURRENT_NO");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdL(" WHERE");
            sb.ApdN("       SAIBAN_CD = ").ApdN(this.BindPrefix).ApdL("SAIBAN_CD");

            // バインド変数設定
            paramCollection.Add(iNewParameter.NewDbParameter("CURRENT_NO", currentNo));
            paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParameter.NewDbParameter("SAIBAN_CD", saibanCD));

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

    #region DELETE
    #endregion

    #endregion

    #region 納入先、便、AR No.チェック

    /// --------------------------------------------------
    /// <summary>
    /// 納入先、便、AR No.チェック（※isReturnARError = false の場合は、T_ARが取得できない場合でも True を戻します）
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">コンディション</param>
    /// <param name="isReturnARError">true:ＡＲ情報データ取得時のエラーを返す false:返さない</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="kanriNo">納入先コード</param>
    /// <param name="dsResult">納入先マスタとAR情報データ</param>
    /// <returns>true:OK/false:NG</returns>
    /// <create>Y.Higuchi 2010/07/08</create>
    /// <update></update>
    /// --------------------------------------------------
    // 2011/03/09 K.Tsutsumi Change T_ARが存在しなくても続行可能
    //public bool CheckNonyusakiAndARNo(DatabaseHelper dbHelper, CondSms cond, out string errMsgID, out string kanriNo, out DataSet dsResult)
    public bool CheckNonyusakiAndARNo(DatabaseHelper dbHelper, CondSms cond, bool isReturnARError, out string errMsgID, out string kanriNo, out DataSet dsResult)
    // ↑
    {
        try
        {
            // 初期化
            errMsgID = string.Empty;
            kanriNo = string.Empty;
            dsResult = null;

            // 納入先を取得する為のコンディションを設定
            CondNonyusaki condsaki = new CondNonyusaki(cond.LoginInfo);
            condsaki.ShukkaFlag = cond.ShukkaFlag;
            condsaki.NonyusakiCD = cond.NonyusakiCD;
            // 納入先の取得
            using (WsMasterImpl master = new WsMasterImpl())
            {
                dsResult = master.GetNonyusaki(dbHelper, condsaki);
            }
            if (dsResult == null || !dsResult.Tables.Contains(Def_M_NONYUSAKI.Name) || dsResult.Tables[Def_M_NONYUSAKI.Name].Rows.Count < 1)
            {
                // 納入先一覧から納入先を選択して下さい。
                errMsgID = "K0100010007";
                return false;
            }
            kanriNo = ComFunc.GetFld(dsResult, Def_M_NONYUSAKI.Name, 0, Def_M_NONYUSAKI.NONYUSAKI_CD);

            // AR情報にAR Noが存在するか確認
            if (cond.ShukkaFlag == SHUKKA_FLAG.AR_VALUE1)
            {
                CondSms condSms = new CondSms(cond.LoginInfo);
                condSms.NonyusakiCD = kanriNo;
                condSms.ARNo = cond.ARNo;
                DataSet ds = this.GetAR(dbHelper, condSms);
                // 2011/03/09 K.Tsutsumi Change T_ARが存在しなくても続行可能
                //if (ds == null || !ds.Tables.Contains(Def_T_AR.Name) || ds.Tables[Def_T_AR.Name].Rows.Count < 1)
                //{
                //    // AR No.が存在しません。
                //    errMsgID = "A9999999020";
                //    return false;
                //}
                //dsResult.Tables.Add(ds.Tables[Def_T_AR.Name].Copy());

                if (ComFunc.IsExistsData(ds, Def_T_AR.Name) == true)
                {
                    // ＡＲ情報データの待避
                    dsResult.Tables.Add(ds.Tables[Def_T_AR.Name].Copy());
                }
                else
                {
                    // エラー通知するか？
                    if (isReturnARError == true)
                    {
                        // AR No.が存在しません。
                        errMsgID = "A9999999020";
                        return false;
                    }
                }
                // ↑
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// AR No.の存在チェック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="nonyusakiCd">納入先コード</param>
    /// <param name="arNo">AR No.</param>
    /// <returns>AR情報データ</returns>
    /// <create>Y.Higuchi 2010/07/08</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetAR(DatabaseHelper dbHelper, CondSms cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            string fieldName = string.Empty;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("       NONYUSAKI_CD");
            sb.ApdL("     , LIST_FLAG");
            sb.ApdL("     , AR_NO");
            sb.ApdL("     , JYOKYO_FLAG");
            sb.ApdL("     , HASSEI_DATE");
            sb.ApdL("     , RENRAKUSHA");
            sb.ApdL("     , KISHU");
            sb.ApdL("     , GOKI");
            sb.ApdL("     , GENBA_TOTYAKUKIBOU_DATE");
            sb.ApdL("     , HUGUAI");
            sb.ApdL("     , TAISAKU");
            sb.ApdL("     , BIKO");
            sb.ApdL("     , GENCHI_TEHAISAKI");
            sb.ApdL("     , GENCHI_SETTEINOKI_DATE");
            sb.ApdL("     , GENCHI_SHUKKAYOTEI_DATE");
            sb.ApdL("     , GENCHI_KOJYOSHUKKA_DATE");
            sb.ApdL("     , SHUKKAHOHO");
            sb.ApdL("     , JP_SETTEINOKI_DATE");
            sb.ApdL("     , JP_SHUKKAYOTEI_DATE");
            sb.ApdL("     , JP_KOJYOSHUKKA_DATE");
            sb.ApdL("     , JP_UNSOKAISHA_NAME");
            sb.ApdL("     , JP_OKURIJYO_NO");
            sb.ApdL("     , GMS_HAKKO_NO");
            sb.ApdL("     , SHIYORENRAKU_NO");
            sb.ApdL("     , TAIO_BUSHO");
            sb.ApdL("     , GIREN_NO_1");
            sb.ApdL("     , GIREN_FILE_1");
            sb.ApdL("     , GIREN_NO_2");
            sb.ApdL("     , GIREN_FILE_2");
            sb.ApdL("     , GIREN_NO_3");
            sb.ApdL("     , GIREN_FILE_3");
            sb.ApdL("     , SHUKKA_DATE");
            sb.ApdL("     , SHUKKA_USER_ID");
            sb.ApdL("     , SHUKKA_USER_NAME");
            sb.ApdL("     , UKEIRE_DATE");
            sb.ApdL("     , UKEIRE_USER_ID");
            sb.ApdL("     , UKEIRE_USER_NAME");
            sb.ApdL("     , LOCK_USER_ID");
            sb.ApdL("     , LOCK_STARTDATE");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_AR");
            sb.ApdL(" WHERE ");
            sb.ApdL("       1 = 1");
            // 納入先コード
            if (cond.NonyusakiCD != null)
            {
                fieldName = "NONYUSAKI_CD";
                sb.ApdN("   AND ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.NonyusakiCD));
            }
            // リスト区分
            if (cond.ListFlag != null)
            {
                fieldName = "LIST_FLAG";
                sb.ApdN("   AND ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ListFlag));
            }
            // AR No.
            if (cond.ARNo != null)
            {
                fieldName = "AR_NO";
                sb.ApdN("   AND ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ARNo));
            }
            // 状況区分
            if (cond.ListFlag != null)
            {
                fieldName = "JYOKYO_FLAG";
                sb.ApdN("   AND ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.JyokyoFlag));
            }

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_AR.Name);

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

}
