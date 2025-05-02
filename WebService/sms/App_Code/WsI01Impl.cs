using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using Commons;
using Condition;
using DSWUtil.DbUtil;
using DSWUtil;

//// --------------------------------------------------
/// <summary>
/// 現地部品管理処理（データアクセス層） 
/// </summary>
/// <create>T.Wakamatsu 2013/07/29</create>
/// <update></update>
/// --------------------------------------------------
public class WsI01Impl : WsBaseImpl
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>T.Wakamatsu 2013/07/29</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsI01Impl()
        : base()
    {
    }

    #endregion

    #region I01:共通処理

    #region 制御

    #region 出荷受入処理

    /// --------------------------------------------------
    /// <summary>
    /// 出荷受入処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Wakamatsu 2013/08/01</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdShukkaUkeire(DatabaseHelper dbHelper, CondI01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 出荷データ更新 
            this.UpdShukka(dbHelper, cond, dt, ref errMsgID, ref args);

            // 受入データ更新 
            this.UpdUkeire(dbHelper, cond, dt, ref errMsgID, ref args);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 出荷処理

    /// --------------------------------------------------
    /// <summary>
    /// 出荷処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Wakamatsu 2013/08/01</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdShukka(DatabaseHelper dbHelper, CondI01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            DataTable dtKiwaku;
            DataTable dtPallet;
            DataTable dtBox;
            DataTable dtShukka;
            // 上位階層のデータもマージして取得する。
            GetRelateTable(dbHelper, dt, true, true, out dtKiwaku, out dtPallet, out dtBox, out dtShukka);

            // 出荷明細データ更新
            // 直接木枠梱包されたTagに関する更新
            this.UpdShukkaMeisaiShukkaKiwaku(dbHelper, cond, dtKiwaku);
            this.UpdShukkaMeisaiShukkaBox(dbHelper, cond, dtBox);
            this.UpdShukkaMeisaiShukkaTag(dbHelper, cond, dtShukka);

            // AR情報データ更新
            this.UpdARShukka(dbHelper, cond, dtBox, dtKiwaku);

            // ボックスリスト管理データ更新
            this.UpdBoxListManageShukka(dbHelper, cond, dtBox);

            // パレットリスト管理データ更新
            this.UpdPalletListManageShukka(dbHelper, cond, dtPallet);

            // 木枠明細データ更新
            this.UpdKiwakuMeisai(dbHelper, cond, dtKiwaku);

            // 木枠データ更新
            this.UpdKiwaku(dbHelper, cond, dtKiwaku);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 関連テーブル取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dt">データ</param>
    /// <param name="merged">上位階層からのデータをマージするか</param>
    /// <param name="isShukka">出荷対象データ抽出かどうか</param>
    /// <param name="dtKiwaku">木枠データ</param>
    /// <param name="dtPallet">パレットデータ</param>
    /// <param name="dtBox">ボックスデータ</param>
    /// <param name="dtShukka">出荷明細データ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Wakamatsu 2013/09/05</create>
    /// <update></update>
    /// --------------------------------------------------
    private void GetRelateTable(DatabaseHelper dbHelper, DataTable dt, bool merged, bool isShukka,
        out DataTable dtKiwaku,
        out DataTable dtPallet,
        out DataTable dtBox,
        out DataTable dtShukka)
    {
        // 出荷対象データを集める
        DataView dv = dt.DefaultView;

        // 木枠梱包されていて、未受入のデータ
        {
            dv.RowFilter = Def_T_SHUKKA_MEISAI.KIWAKUKONPO_DATE + " IS NOT NULL" +
            " AND " + Def_T_SHUKKA_MEISAI.UKEIRE_DATE + " IS NULL";
            dv.Sort = Def_T_SHUKKA_MEISAI.KOJI_NO;

            dtKiwaku = dv.ToTable(Def_T_KIWAKU_MEISAI.Name, true,
                Def_T_SHUKKA_MEISAI.KOJI_NO);
            if (isShukka && dtKiwaku.Rows.Count > 0)
            {
                // 出荷日取得
                dtKiwaku = GetKiwakuShukkaDate(dbHelper, dtKiwaku);
            }
        }

        // パレット梱包されていて、木枠梱包されていない、未受入のデータ
        {
            dv.RowFilter = Def_T_SHUKKA_MEISAI.PALLETKONPO_DATE + " IS NOT NULL" +
            " AND " + Def_T_SHUKKA_MEISAI.KIWAKUKONPO_DATE + " IS NULL" +
            " AND " + Def_T_SHUKKA_MEISAI.UKEIRE_DATE + " IS NULL";

            dtPallet = dv.ToTable(Def_T_PALLETLIST_MANAGE.Name, true,
                Def_T_SHUKKA_MEISAI.PALLET_NO,
                Def_T_SHUKKA_MEISAI.PALLETKONPO_DATE);
            dtPallet.Columns[Def_T_SHUKKA_MEISAI.PALLETKONPO_DATE].ColumnName = Def_T_SHUKKA_MEISAI.SHUKKA_DATE;

            // 上位階層からのデータをマージ
            if (merged)
            {
                dtPallet.Merge(this.GetPalletForKiwaku(dbHelper, dtKiwaku), true);
            }
        }

        // ボックス梱包されていて、パレット梱包されていない、未受入のデータ
        {
            dv.RowFilter = Def_T_SHUKKA_MEISAI.BOXKONPO_DATE + " IS NOT NULL" +
            " AND " + Def_T_SHUKKA_MEISAI.PALLETKONPO_DATE + " IS NULL" +
            " AND " + Def_T_SHUKKA_MEISAI.KIWAKUKONPO_DATE + " IS NULL" +
            " AND " + Def_T_SHUKKA_MEISAI.UKEIRE_DATE + " IS NULL";

            dtBox = dv.ToTable(Def_T_BOXLIST_MANAGE.Name, true,
                Def_T_SHUKKA_MEISAI.BOX_NO,
                Def_T_SHUKKA_MEISAI.BOXKONPO_DATE);
            dtBox.Columns[Def_T_SHUKKA_MEISAI.BOXKONPO_DATE].ColumnName = Def_T_SHUKKA_MEISAI.SHUKKA_DATE;

            // 上位階層からのデータをマージ
            if (merged)
            {
                dtBox.Merge(this.GetBoxForPallet(dbHelper, dtPallet));
            }
        }

        // 
        if (isShukka)
        {
            dv.RowFilter = Def_T_SHUKKA_MEISAI.BOXKONPO_DATE + " IS NULL" +
            " AND " + Def_T_SHUKKA_MEISAI.KIWAKUKONPO_DATE + " IS NULL" +
            " AND " + Def_T_SHUKKA_MEISAI.UKEIRE_DATE + " IS NULL";

            dtShukka = dv.ToTable(Def_T_SHUKKA_MEISAI.Name, true,
                Def_T_SHUKKA_MEISAI.SHUKKA_FLAG,
                Def_T_SHUKKA_MEISAI.NONYUSAKI_CD,
                Def_T_SHUKKA_MEISAI.TAG_NO);
            // 出荷日取得
            dtShukka = GetShukkaMeisaiShukkaDate(dbHelper, dtShukka);
        }
        else
        {
            dtShukka = new DataTable();
        }
    }

    #endregion

    #region 受入処理

    /// --------------------------------------------------
    /// <summary>
    /// 受入処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Wakamatsu 2013/08/01</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdUkeire(DatabaseHelper dbHelper, CondI01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            DataTable dtKiwaku;
            DataTable dtPallet;
            DataTable dtBox;
            DataTable dtShukka;
            //上位階層のデータはマージしないで取得する。
            GetRelateTable(dbHelper, dt, true, false, out dtKiwaku, out dtPallet, out dtBox, out dtShukka);

            // 出荷明細データ更新 
            this.UpdShukkaMeisaiUkeire(dbHelper, cond, dt);

            // AR情報データ更新 
            this.UpdARUkeire(dbHelper, cond, dt);

            // ボックスリスト管理データ更新
            this.UpdBoxListManageUkeire(dbHelper, cond, dtBox);

            // パレットリスト管理データ更新
            this.UpdPalletListManageUkeire(dbHelper, cond, dtPallet);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region クエリ

    #region ロケーションコンボボックス用データ取得

    /// --------------------------------------------------
    /// <summary>
    /// ロケーションコンボボックス用データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <returns>ロケーションマスタ</returns>
    /// <create>T.Wakamatsu 2013/07/31</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetLocationCombo(DatabaseHelper dbHelper, CondI01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       ML.LOCATION");
            sb.ApdL("  FROM");
            sb.ApdL("       M_LOCATION ML");
            sb.ApdL("  INNER JOIN M_BUKKEN MB ON MB.SHUKKA_FLAG = ML.SHUKKA_FLAG");
            sb.ApdL("                         AND MB.BUKKEN_NO = ML.BUKKEN_NO");
            sb.ApdL("  LEFT JOIN M_COMMON COM1 ON COM1.GROUP_CD = 'SHUKKA_FLAG'");
            sb.ApdL("                         AND COM1.VALUE1 = ML.SHUKKA_FLAG");
            sb.ApdN("                         AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL(" WHERE");
            sb.ApdN("       ML.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND ML.BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       ML.LOCATION");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_LOCATION.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 木枠出荷日取得

    /// --------------------------------------------------
    /// <summary>
    /// 木枠出荷日取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dt">木枠データ</param>
    /// <returns>パレットデータ</returns>
    /// <create>T.Wakamatsu 2013/09/12</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetKiwakuShukkaDate(DatabaseHelper dbHelper, DataTable dt)
    {
        try
        {
            DataTable retDt = new DataTable();
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;
            DbParamCollection paramCollection = new DbParamCollection();

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       KOJI_NO");
            sb.ApdL("     , COALESCE(MAX(SHUKKA_DATE), MAX(KIWAKUKONPO_DATE)) AS SHUKKA_DATE");
            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI SM");
            sb.ApdL(" WHERE");
            sb.ApdN("       KOJI_NO IN (");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                if (i == 0)
                {
                    sb.ApdN(this.BindPrefix).ApdN("KOJI_NO" + i.ToString());
                }
                else
                {
                    sb.ApdN(",").ApdN(this.BindPrefix).ApdN("KOJI_NO" + i.ToString());
                }

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO" + i.ToString(), ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.KOJI_NO)));
            }
            sb.ApdL(")");
            sb.ApdL(" GROUP BY");
            sb.ApdL("       KOJI_NO");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, retDt);

            return retDt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 出荷明細出荷日取得

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細出荷日取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dt">出荷明細データ</param>
    /// <returns>パレットデータ</returns>
    /// <create>T.Wakamatsu 2013/09/12</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetShukkaMeisaiShukkaDate(DatabaseHelper dbHelper, DataTable dt)
    {
        try
        {
            DataTable retDt = new DataTable();
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , TAG_NO");
            sb.ApdL("     , COALESCE(SHUKKA_DATE, SHUKA_DATE, TAGHAKKO_DATE) AS SHUKKA_DATE");
            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI SM");
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.TAG_NO)));

                DataTable dtBuf = new DataTable();
                // SQL実行
                dbHelper.Fill(sb.ToString(), paramCollection, dtBuf);

                retDt.Merge(dtBuf);
            }

            return retDt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 同一木枠内のパレットデータ取得

    /// --------------------------------------------------
    /// <summary>
    /// 同一木枠内のパレットデータ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dtKiwaku">木枠データ</param>
    /// <returns>パレットデータ</returns>
    /// <create>T.Wakamatsu 2013/08/02</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetPalletForKiwaku(DatabaseHelper dbHelper, DataTable dtKiwaku)
    {
        try
        {
            DataTable retDt = new DataTable();
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT DISTINCT");
            sb.ApdL("       PALLET_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdN("SHUKKA_DATE").ApdL(" AS SHUKKA_DATE");
            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI SM");
            sb.ApdL(" WHERE");
            sb.ApdN("       KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdL("   AND PALLET_NO IS NOT NULL ");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       PALLET_NO");

            foreach (DataRow dr in dtKiwaku.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.KOJI_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_DATE", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_DATE)));

                // SQL実行
                DataTable dt = new DataTable();
                dbHelper.Fill(sb.ToString(), paramCollection, dt);
                retDt.Merge(dt);
            }

            return retDt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 同一パレット内のボックスデータ取得

    /// --------------------------------------------------
    /// <summary>
    /// 木枠データ内のボックスデータ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dtPallet">パレットデータ</param>
    /// <returns>ボックスデータ</returns>
    /// <create>T.Wakamatsu 2013/08/02</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetBoxForPallet(DatabaseHelper dbHelper, DataTable dtPallet)
    {
        try
        {
            DataTable retDt = new DataTable();
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT DISTINCT");
            sb.ApdL("       BOX_NO");
            sb.ApdL("     , ").ApdN(this.BindPrefix).ApdN("SHUKKA_DATE").ApdL(" AS SHUKKA_DATE");
            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI");
            sb.ApdL(" WHERE");
            sb.ApdN("       PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
            sb.ApdL("   AND BOX_NO IS NOT NULL ");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       BOX_NO");

            foreach (DataRow dr in dtPallet.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.PALLET_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_DATE", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_DATE)));

                // SQL実行
                DataTable dt = new DataTable();
                dbHelper.Fill(sb.ToString(), paramCollection, dt);
                retDt.Merge(dt);
            }

            return retDt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 木枠データ更新

    /// --------------------------------------------------
    /// <summary>
    /// 木枠データ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">木枠データ</param>
    /// <returns>更新件数</returns>
    /// <create>T.Wakamatsu 2013/08/02</create>
    /// <update></update>
    /// --------------------------------------------------
    private int UpdKiwaku(DatabaseHelper dbHelper, CondI01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE KW");
            sb.ApdL("SET");
            sb.ApdN("       KW.SAGYO_FLAG = ").ApdN(this.BindPrefix).ApdL("SAGYO_FLAG");
            sb.ApdL("     , KW.SHUKKA_DATE = SMM.SHUKKA_DATE");
            sb.ApdN("     , KW.SHUKKA_USER_ID = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_ID");
            sb.ApdN("     , KW.SHUKKA_USER_NAME = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_NAME");
            sb.ApdN("     , KW.UPDATE_DATE = ").ApdL(this.SysTimestamp); ;
            sb.ApdN("     , KW.UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , KW.UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , KW.VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL("  FROM");
            sb.ApdL("       T_KIWAKU KW");
            sb.ApdL(" INNER JOIN (");
            sb.ApdL("       SELECT");
            sb.ApdL("              SM.KOJI_NO");
            sb.ApdL("            , MAX(SM.KIWAKUKONPO_DATE) AS SHUKKA_DATE");
            sb.ApdL("         FROM");
            sb.ApdL("              T_SHUKKA_MEISAI SM");
            sb.ApdL("        WHERE");
            sb.ApdN("              SM.KOJI_NO =").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdL("        GROUP BY");
            sb.ApdL("              SM.KOJI_NO");
            sb.ApdL("       ) SMM ON SMM.KOJI_NO = KW.KOJI_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       KW.SHUKKA_DATE IS NULL");

            string curKojiNo = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                string kojiNo = ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.KOJI_NO);
                if (curKojiNo != kojiNo)
                {
                    DbParamCollection paramCollection = new DbParamCollection();

                    // バインド変数設定
                    paramCollection.Add(iNewParam.NewDbParameter("SAGYO_FLAG", SAGYO_FLAG.SHUKKAZUMI_VALUE1));
                    paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_ID", this.GetUpdateUserID(cond)));
                    paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_NAME", this.GetUpdateUserName(cond)));
                    paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                    paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                    paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", kojiNo));

                    // SQL実行
                    record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);


                    curKojiNo = kojiNo;
                }
            }

            return record;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 木枠明細データ更新

    /// --------------------------------------------------
    /// <summary>
    /// 木枠明細データ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">木枠データ</param>
    /// <returns>更新件数</returns>
    /// <create>T.Wakamatsu 2013/08/02</create>
    /// <update></update>
    /// --------------------------------------------------
    private int UpdKiwakuMeisai(DatabaseHelper dbHelper, CondI01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE KM");
            sb.ApdL("SET");
            sb.ApdL("       KM.SHUKKA_DATE = KW.SHUKKA_DATE");
            sb.ApdN("     , KM.SHUKKA_USER_ID = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_ID");
            sb.ApdN("     , KM.SHUKKA_USER_NAME = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_NAME");
            sb.ApdN("     , KM.UPDATE_DATE = ").ApdL(this.SysTimestamp); ;
            sb.ApdN("     , KM.UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , KM.UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , KM.VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL("  FROM");
            sb.ApdL("       T_KIWAKU_MEISAI KM");
            sb.ApdL(" INNER JOIN T_KIWAKU KW ON KW.KOJI_NO = KM.KOJI_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       KM.KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdN("   AND KM.SHUKKA_DATE IS NULL");
            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.KOJI_NO)));

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

    #endregion

    #region パレットリスト管理データ更新

    /// --------------------------------------------------
    /// <summary>
    /// パレットリスト管理データ出荷更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">パレットデータ</param>
    /// <returns>更新件数</returns>
    /// <create>T.Wakamatsu 2013/08/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdPalletListManageShukka(DatabaseHelper dbHelper, CondI01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_PALLETLIST_MANAGE");
            sb.ApdL("SET");
            sb.ApdN("       SHUKKA_DATE = ").ApdN(this.BindPrefix).ApdL("SHUKKA_DATE");
            sb.ApdN("     , SHUKKA_USER_ID = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_ID");
            sb.ApdN("     , SHUKKA_USER_NAME = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_NAME");
            sb.ApdN("     , KANRI_FLAG = ").ApdN(this.BindPrefix).ApdL("KANRI_FLAG");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
            sb.ApdN("   AND SHUKKA_DATE IS NULL");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_DATE", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_DATE)));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("KANRI_FLAG", KANRI_FLAG.KANRYO_VALUE1));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.PALLET_NO)));

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
    /// パレットリスト管理データ受入更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">パレットデータ</param>
    /// <returns>更新件数</returns>
    /// <create>T.Wakamatsu 2013/08/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdPalletListManageUkeire(DatabaseHelper dbHelper, CondI01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_PALLETLIST_MANAGE");
            sb.ApdL("SET");
            sb.ApdN("       UKEIRE_DATE = ").ApdN(this.BindPrefix).ApdL("UKEIRE_DATE");
            sb.ApdN("     , UKEIRE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UKEIRE_USER_ID");
            sb.ApdN("     , UKEIRE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UKEIRE_USER_NAME");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL("  FROM T_PALLETLIST_MANAGE TPM");
            sb.ApdL(" WHERE");
            sb.ApdN("       PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
            sb.ApdL("   AND NOT EXISTS (");
            sb.ApdL("                   SELECT 1");
            sb.ApdL("                     FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("                    WHERE TSM.PALLET_NO = TPM.PALLET_NO");
            sb.ApdN("                      AND TSM.UKEIRE_DATE IS NULL");
            sb.ApdL("                  )");
            sb.ApdN("   AND UKEIRE_DATE IS NULL");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_DATE", cond.StockDate));
                paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.PALLET_NO)));

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

    #endregion

    #region BOXリスト管理データ更新

    /// --------------------------------------------------
    /// <summary>
    /// BOXリスト管理データ出荷更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">ボックスデータ</param>
    /// <returns>更新件数</returns>
    /// <create>T.Wakamatsu 2013/08/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdBoxListManageShukka(DatabaseHelper dbHelper, CondI01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_BOXLIST_MANAGE");
            sb.ApdL("SET");
            sb.ApdN("       SHUKKA_DATE = ").ApdN(this.BindPrefix).ApdL("SHUKKA_DATE");
            sb.ApdN("     , SHUKKA_USER_ID = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_ID");
            sb.ApdN("     , SHUKKA_USER_NAME = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_NAME");
            sb.ApdN("     , KANRI_FLAG = ").ApdN(this.BindPrefix).ApdL("KANRI_FLAG");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
            sb.ApdN("   AND SHUKKA_DATE IS NULL");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_DATE", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_DATE)));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("KANRI_FLAG", KANRI_FLAG.KANRYO_VALUE1));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.BOX_NO)));

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
    /// BOXリスト管理データ受入更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">ボックスデータ</param>
    /// <returns>更新件数</returns>
    /// <create>T.Wakamatsu 2013/08/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdBoxListManageUkeire(DatabaseHelper dbHelper, CondI01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE TBM");
            sb.ApdL("SET");
            sb.ApdN("       UKEIRE_DATE = ").ApdN(this.BindPrefix).ApdL("UKEIRE_DATE");
            sb.ApdN("     , UKEIRE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UKEIRE_USER_ID");
            sb.ApdN("     , UKEIRE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UKEIRE_USER_NAME");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL("  FROM T_BOXLIST_MANAGE TBM");
            sb.ApdL(" WHERE");
            sb.ApdN("       BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
            sb.ApdL("   AND NOT EXISTS (");
            sb.ApdL("                   SELECT 1");
            sb.ApdL("                     FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("                    WHERE TSM.BOX_NO = TBM.BOX_NO");
            sb.ApdN("                      AND TSM.UKEIRE_DATE IS NULL");
            sb.ApdL("                  )");
            sb.ApdN("   AND UKEIRE_DATE IS NULL");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_DATE", cond.StockDate));
                paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.BOX_NO)));

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

    #endregion

    #region AR情報更新

    /// --------------------------------------------------
    /// <summary>
    /// AR情報出荷更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dtBox">ボックスデータ</param>
    /// <param name="dtKiwaku">木枠データ</param>
    /// <returns>更新件数</returns>
    /// <create>T.Wakamatsu 2013/08/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdARShukka(DatabaseHelper dbHelper, CondI01 cond, DataTable dtBox, DataTable dtKiwaku)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE AR");
            sb.ApdL("SET");
            sb.ApdN("       AR.SHUKKA_DATE = ").ApdN(this.BindPrefix).ApdL("SHUKKA_DATE");
            sb.ApdN("     , AR.SHUKKA_USER_ID = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_ID");
            sb.ApdN("     , AR.SHUKKA_USER_NAME = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_NAME");
            sb.ApdN("     , AR.UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , AR.UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , AR.UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , AR.VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL("  FROM");
            sb.ApdL("       T_AR AR");
            sb.ApdL(" INNER JOIN T_SHUKKA_MEISAI SM ON SM.NONYUSAKI_CD = AR.NONYUSAKI_CD");
            sb.ApdL("                              AND SM.AR_NO = AR.AR_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       SM.BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
            sb.ApdN("   AND AR.SHUKKA_DATE IS NULL");

            foreach (DataRow dr in dtBox.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_DATE", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_DATE)));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.BOX_NO)));

                // SQL実行
                record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            }

            // SQL文
            sb = new StringBuilder();
            sb.ApdL("UPDATE AR");
            sb.ApdL("SET");
            sb.ApdN("       AR.SHUKKA_DATE = ").ApdN(this.BindPrefix).ApdL("SHUKKA_DATE");
            sb.ApdN("     , AR.SHUKKA_USER_ID = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_ID");
            sb.ApdN("     , AR.SHUKKA_USER_NAME = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_NAME");
            sb.ApdN("     , AR.UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , AR.UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , AR.UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , AR.VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL("  FROM");
            sb.ApdL("       T_AR AR");
            sb.ApdL(" INNER JOIN T_SHUKKA_MEISAI SM ON SM.NONYUSAKI_CD = AR.NONYUSAKI_CD");
            sb.ApdL("                              AND SM.AR_NO = AR.AR_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       SM.KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdN("   AND AR.SHUKKA_DATE IS NULL");

            DataTable dt = dtKiwaku.DefaultView.ToTable(true, Def_T_KIWAKU.KOJI_NO);
            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_DATE", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_DATE)));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.KOJI_NO)));

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
    /// AR情報受入更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">ボックスデータ</param>
    /// <returns>更新件数</returns>
    /// <create>T.Wakamatsu 2013/08/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdARUkeire(DatabaseHelper dbHelper, CondI01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE AR");
            sb.ApdL("SET");
            sb.ApdN("       AR.UKEIRE_DATE = ").ApdN(this.BindPrefix).ApdL("UKEIRE_DATE");
            sb.ApdN("     , AR.UKEIRE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UKEIRE_USER_ID");
            sb.ApdN("     , AR.UKEIRE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UKEIRE_USER_NAME");
            sb.ApdN("     , AR.UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , AR.UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , AR.UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , AR.VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL("  FROM");
            sb.ApdL("       T_AR AR");
            sb.ApdL(" INNER JOIN T_SHUKKA_MEISAI SM ON SM.NONYUSAKI_CD = AR.NONYUSAKI_CD");
            sb.ApdL("                              AND SM.AR_NO = AR.AR_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       SM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND SM.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND SM.TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");
            sb.ApdN("   AND AR.UKEIRE_DATE IS NULL");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_DATE", cond.StockDate));
                paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.TAG_NO)));

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

    #endregion

    #region 出荷明細更新

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細出荷更新（タグデータ）
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">タグデータ</param>
    /// <returns>更新件数</returns>
    /// <create>T.Wakamatsu 2013/09/12</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdShukkaMeisaiShukkaTag(DatabaseHelper dbHelper, CondI01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_SHUKKA_MEISAI");
            sb.ApdL("SET");
            sb.ApdN("       JYOTAI_FLAG = ").ApdN(this.BindPrefix).ApdL("JYOTAI_FLAG");
            sb.ApdN("     , SHUKKA_DATE = ").ApdN(this.BindPrefix).ApdL("SHUKKA_DATE");
            sb.ApdN("     , SHUKKA_USER_ID = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_ID");
            sb.ApdN("     , SHUKKA_USER_NAME = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_NAME");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");
            sb.ApdN("   AND SHUKKA_DATE IS NULL");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("JYOTAI_FLAG", JYOTAI_FLAG.SHUKKAZUMI_VALUE1));
                if (string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_DATE)))
                    paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_DATE", cond.StockDate));
                else
                    paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_DATE", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_DATE)));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.TAG_NO)));

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
    /// 出荷明細出荷更新（ボックスデータ）
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">ボックスデータ</param>
    /// <returns>更新件数</returns>
    /// <create>T.Wakamatsu 2013/08/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdShukkaMeisaiShukkaBox(DatabaseHelper dbHelper, CondI01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_SHUKKA_MEISAI");
            sb.ApdL("SET");
            sb.ApdN("       JYOTAI_FLAG = ").ApdN(this.BindPrefix).ApdL("JYOTAI_FLAG");
            sb.ApdN("     , SHUKKA_DATE = ").ApdN(this.BindPrefix).ApdL("SHUKKA_DATE");
            sb.ApdN("     , SHUKKA_USER_ID = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_ID");
            sb.ApdN("     , SHUKKA_USER_NAME = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_NAME");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
            sb.ApdN("   AND SHUKKA_DATE IS NULL");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("JYOTAI_FLAG", JYOTAI_FLAG.SHUKKAZUMI_VALUE1));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_DATE", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_DATE)));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.BOX_NO)));

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
    /// 出荷明細出荷更新（木枠データ）
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">木枠データ</param>
    /// <returns>更新件数</returns>
    /// <create>T.Wakamatsu 2013/09/11</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdShukkaMeisaiShukkaKiwaku(DatabaseHelper dbHelper, CondI01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_SHUKKA_MEISAI");
            sb.ApdL("SET");
            sb.ApdN("       JYOTAI_FLAG = ").ApdN(this.BindPrefix).ApdL("JYOTAI_FLAG");
            sb.ApdN("     , SHUKKA_DATE = ").ApdN(this.BindPrefix).ApdL("SHUKKA_DATE");
            sb.ApdN("     , SHUKKA_USER_ID = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_ID");
            sb.ApdN("     , SHUKKA_USER_NAME = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_NAME");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdN("   AND SHUKKA_DATE IS NULL");


            string curKojiNo = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                string kojiNo = ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.KOJI_NO);
                if (curKojiNo != kojiNo)
                {
                    curKojiNo = kojiNo;
                    DbParamCollection paramCollection = new DbParamCollection();

                    // バインド変数設定
                    paramCollection.Add(iNewParam.NewDbParameter("JYOTAI_FLAG", JYOTAI_FLAG.SHUKKAZUMI_VALUE1));
                    paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_DATE", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_DATE)));
                    paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_ID", this.GetUpdateUserID(cond)));
                    paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_NAME", this.GetUpdateUserName(cond)));
                    paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                    paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                    paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", kojiNo));

                    // SQL実行
                    record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
                }
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
    /// 出荷明細受入更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">データ</param>
    /// <returns>更新件数</returns>
    /// <create>T.Wakamatsu 2013/08/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdShukkaMeisaiUkeire(DatabaseHelper dbHelper, CondI01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_SHUKKA_MEISAI");
            sb.ApdL("SET");
            sb.ApdN("       JYOTAI_FLAG = ").ApdN(this.BindPrefix).ApdL("JYOTAI_FLAG");
            sb.ApdN("     , UKEIRE_DATE = ").ApdN(this.BindPrefix).ApdL("UKEIRE_DATE");
            sb.ApdN("     , UKEIRE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UKEIRE_USER_ID");
            sb.ApdN("     , UKEIRE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UKEIRE_USER_NAME");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");
            sb.ApdN("   AND UKEIRE_DATE IS NULL");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("JYOTAI_FLAG", JYOTAI_FLAG.UKEIREZUMI_VALUE1));
                paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_DATE", cond.StockDate));
                paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.TAG_NO)));

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

    #endregion
    
    #region 実績データ登録

    /// --------------------------------------------------
    /// <summary>
    /// 実績データ登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/08/05</create>
    /// <update>H.Tajimi 2015/11/20 備考対応</update>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update></update>
    /// --------------------------------------------------
    public int InsJisseki(DatabaseHelper dbHelper, CondI01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_JISSEKI");
            sb.ApdL("(");
            sb.ApdL("       JISSEKI_DATE");
            sb.ApdL("     , SAGYO_FLAG");
            sb.ApdL("     , TAG_CODE");
            sb.ApdL("     , WORK_USER_ID");
            sb.ApdL("     , WORK_USER_NAME");
            sb.ApdL("     , BUKKEN_NO");
            sb.ApdL("     , LOCATION");
            sb.ApdL("     , STATUS");
            sb.ApdL("     , STOCK_DATE");
            sb.ApdL("     , SHUKKA_FLAG");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , NONYUSAKI_NAME");
            sb.ApdL("     , TAG_NO");
            sb.ApdL("     , AR_NO");
            sb.ApdL("     , SEIBAN");
            sb.ApdL("     , CODE");
            sb.ApdL("     , ZUMEN_OIBAN");
            sb.ApdL("     , AREA");
            sb.ApdL("     , FLOOR");
            sb.ApdL("     , KISHU");
            sb.ApdL("     , ST_NO");
            sb.ApdL("     , HINMEI_JP");
            sb.ApdL("     , HINMEI");
            sb.ApdL("     , ZUMEN_KEISHIKI");
            sb.ApdL("     , KUWARI_NO");
            sb.ApdL("     , NUM");
            sb.ApdL("     , JYOTAI_FLAG");
            sb.ApdL("     , TAGHAKKO_FLAG");
            sb.ApdL("     , TAGHAKKO_DATE");
            sb.ApdL("     , SHUKA_DATE");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , BOXKONPO_DATE");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("     , PALLETKONPO_DATE");
            sb.ApdL("     , KOJI_NO");
            sb.ApdL("     , CASE_ID");
            sb.ApdL("     , KIWAKUKONPO_DATE");
            sb.ApdL("     , SHUKKA_DATE");
            sb.ApdL("     , SHUKKA_USER_ID");
            sb.ApdL("     , SHUKKA_USER_NAME");
            sb.ApdL("     , UNSOKAISHA_NAME");
            sb.ApdL("     , INVOICE_NO");
            sb.ApdL("     , OKURIJYO_NO");
            sb.ApdL("     , BL_NO");
            sb.ApdL("     , UKEIRE_DATE");
            sb.ApdL("     , UKEIRE_USER_ID");
            sb.ApdL("     , UKEIRE_USER_NAME");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , BIKO");
            sb.ApdL("     , M_NO");
            sb.ApdL(")");
            sb.ApdL("SELECT");
            sb.ApdN("       ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SAGYO_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("TAG_CODE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("WORK_USER_ID");
            sb.ApdL("     , MU.USER_NAME");
            sb.ApdL("     , MN.BUKKEN_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LOCATION");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("STATUS");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("STOCK_DATE");
            sb.ApdL("     , SM.SHUKKA_FLAG");
            sb.ApdL("     , SM.NONYUSAKI_CD");
            sb.ApdL("     , MN.NONYUSAKI_NAME");
            sb.ApdL("     , SM.TAG_NO");
            sb.ApdL("     , SM.AR_NO");
            sb.ApdL("     , SM.SEIBAN");
            sb.ApdL("     , SM.CODE");
            sb.ApdL("     , SM.ZUMEN_OIBAN");
            sb.ApdL("     , SM.AREA");
            sb.ApdL("     , SM.FLOOR");
            sb.ApdL("     , SM.KISHU");
            sb.ApdL("     , SM.ST_NO");
            sb.ApdL("     , SM.HINMEI_JP");
            sb.ApdL("     , SM.HINMEI");
            sb.ApdL("     , SM.ZUMEN_KEISHIKI");
            sb.ApdL("     , SM.KUWARI_NO");
            sb.ApdL("     , SM.NUM");
            sb.ApdL("     , SM.JYOTAI_FLAG");
            sb.ApdL("     , SM.TAGHAKKO_FLAG");
            sb.ApdL("     , SM.TAGHAKKO_DATE");
            sb.ApdL("     , SM.SHUKA_DATE");
            sb.ApdL("     , SM.BOX_NO");
            sb.ApdL("     , SM.BOXKONPO_DATE");
            sb.ApdL("     , SM.PALLET_NO");
            sb.ApdL("     , SM.PALLETKONPO_DATE");
            sb.ApdL("     , SM.KOJI_NO");
            sb.ApdL("     , SM.CASE_ID");
            sb.ApdL("     , SM.KIWAKUKONPO_DATE");
            sb.ApdL("     , SM.SHUKKA_DATE");
            sb.ApdL("     , SM.SHUKKA_USER_ID");
            sb.ApdL("     , SM.SHUKKA_USER_NAME");
            sb.ApdL("     , SM.UNSOKAISHA_NAME");
            sb.ApdL("     , SM.INVOICE_NO");
            sb.ApdL("     , SM.OKURIJYO_NO");
            sb.ApdL("     , SM.BL_NO");
            sb.ApdL("     , SM.UKEIRE_DATE");
            sb.ApdL("     , SM.UKEIRE_USER_ID");
            sb.ApdL("     , SM.UKEIRE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdL("     , SM.BIKO");
            sb.ApdL("     , SM.M_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI SM");
            sb.ApdL("  INNER JOIN M_USER MU ON MU.USER_ID = ").ApdN(this.BindPrefix).ApdL("WORK_USER_ID");
            sb.ApdL("  INNER JOIN M_NONYUSAKI MN ON MN.SHUKKA_FLAG = SM.SHUKKA_FLAG");
            sb.ApdL("                         AND MN.NONYUSAKI_CD = SM.NONYUSAKI_CD");
            sb.ApdL("  LEFT JOIN T_STOCK STK ON STK.SHUKKA_FLAG = SM.SHUKKA_FLAG");
            sb.ApdL("                         AND STK.NONYUSAKI_CD = SM.NONYUSAKI_CD");
            sb.ApdL("                         AND STK.TAG_NO = SM.TAG_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       SM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND SM.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND SM.TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.TAG_NO)));


                paramCollection.Add(iNewParam.NewDbParameter("WORK_USER_ID", cond.WorkUserID));
                
                paramCollection.Add(iNewParam.NewDbParameter("SAGYO_FLAG", cond.SagyoFlag));
                paramCollection.Add(iNewParam.NewDbParameter("TAG_CODE", ComFunc.GetFld(dr, Def_T_STOCK.TAG_CODE)));

                string status = ComFunc.GetFld(dr, Def_T_STOCK.STATUS);

                if (cond.SagyoFlag == ZAIKO_SAGYO_FLAG.ZAIKO_VALUE1 ||
                    cond.SagyoFlag == ZAIKO_SAGYO_FLAG.TANAOROSHI_ZAIKO_VALUE1 ||
                    cond.SagyoFlag == ZAIKO_SAGYO_FLAG.MAINTE_ZAIKO_VALUE1)
                {
                    paramCollection.Add(iNewParam.NewDbParameter("LOCATION", ComFunc.GetFld(dr, ComDefine.FLD_NYUKO_LOCATION)));
                }
                else
                {
                    paramCollection.Add(iNewParam.NewDbParameter("LOCATION", ComFunc.GetFld(dr, Def_T_STOCK.LOCATION)));
                }

                if (cond.SagyoFlag == ZAIKO_SAGYO_FLAG.ZAIKO_VALUE1 ||
                    cond.SagyoFlag == ZAIKO_SAGYO_FLAG.TANAOROSHI_ZAIKO_VALUE1 ||
                    cond.SagyoFlag == ZAIKO_SAGYO_FLAG.MAINTE_ZAIKO_VALUE1)
                {
                    if (string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_STOCK.LOCATION)))
                        paramCollection.Add(iNewParam.NewDbParameter("STATUS", ZAIKO_STATUS.ZAIKO_VALUE1));
                    else
                        paramCollection.Add(iNewParam.NewDbParameter("STATUS", ZAIKO_STATUS.IDO_ZAIKO_VALUE1));
                }
                else
                {
                    paramCollection.Add(iNewParam.NewDbParameter("STATUS", ZAIKO_STATUS.KANRYO_VALUE1));
                }
                paramCollection.Add(iNewParam.NewDbParameter("STOCK_DATE", cond.StockDate));
                paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

                // SQL実行
                record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            }

            // 実績登録時間をずらす
            Thread.Sleep(1);

            return record;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #endregion

    #region I0100010:ロケーション保守

    #region 制御

    #region ロケーションの追加

    /// --------------------------------------------------
    /// <summary>
    /// ロケーションの追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Wakamatsu 2013/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool InsLocationData(DatabaseHelper dbHelper, CondI01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // ロケーションの存在チェック
            if (this.GetLocationCount(dbHelper, cond) > 0)
            {
                // 既に登録されているロケーションです。
                errMsgID = "I0100010001";
                return false;
            }

            // 登録実行
            this.InsLocation(dbHelper, cond);

            return true;
        }
        catch (Exception ex)
        {
            // 一意制約違反チェック
            if (this.IsDbDuplicationError(ex))
            {
                // 既に登録されているロケーションです。
                errMsgID = "I0100010001";
                return false;
            }
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region ロケーションの更新

    /// --------------------------------------------------
    /// <summary>
    /// ロケーションの更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Wakamatsu 2013/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdLocationData(DatabaseHelper dbHelper, CondI01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 存在チェック
            DataSet ds = this.GetLocation(dbHelper, cond);
            if (!ComFunc.IsExistsData(ds, Def_M_LOCATION.Name))
            {
                // 既に削除されたロケーションです。
                errMsgID = "I0100010002";
                return false;
            }
            // バージョンチェック
            int index;
            int[] notFoundIndex;
            index = this.CheckSameData(dt, ds.Tables[Def_M_LOCATION.Name], out notFoundIndex, Def_M_LOCATION.BUKKEN_NO, Def_M_LOCATION.SHUKKA_FLAG, Def_M_LOCATION.LOCATION);
            if (0 <= index)
            {
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }
            else if (notFoundIndex != null)
            {
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }
            // ロケーションの存在チェック
            if (this.GetLocationCount(dbHelper, cond) > 0)
            {
                // 既に登録されているロケーションです。
                errMsgID = "I0100010001";
                return false;
            }

            // 更新実行
            this.UpdLocation(dbHelper, cond, dt);

            return true;
        }
        catch (Exception ex)
        {
            // 一意制約違反チェック
            if (this.IsDbDuplicationError(ex))
            {
                // 既に登録されているロケーションです。
                errMsgID = "I0100010001";
                return false;
            }
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region ロケーションの削除

    /// --------------------------------------------------
    /// <summary>
    /// ロケーションの削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Wakamatsu 2013/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool DelLocationData(DatabaseHelper dbHelper, CondI01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 存在チェック
            DataSet ds = this.GetLocation(dbHelper, cond);
            if (!ComFunc.IsExistsData(ds, Def_M_LOCATION.Name))
            {
                // 既に削除されたロケーションです。
                errMsgID = "I0100010002";
                return false;
            }
            // バージョンチェック
            int index;
            int[] notFoundIndex;
            index = this.CheckSameData(dt, ds.Tables[Def_M_LOCATION.Name], out notFoundIndex, Def_M_LOCATION.BUKKEN_NO, Def_M_LOCATION.SHUKKA_FLAG, Def_M_LOCATION.LOCATION);
            if (0 <= index)
            {
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }
            else if (notFoundIndex != null)
            {
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }
            // 在庫存在チェック
            if (this.ExistsZaiko(dbHelper, cond))
            {
                // 在庫データがある為、削除できません。
                errMsgID = "I0100010005";
                return false;
            }

            // 削除実行
            this.DelLocation(dbHelper, cond, dt);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region クエリ

    #region SELECT

    #region ロケーション取得(完全一致・完全一致・物件管理No、出荷区分、ロケーション必須必須)

    /// --------------------------------------------------
    /// <summary>
    /// ロケーション取得(完全一致・完全一致・物件管理No、出荷区分、ロケーション必須o必須)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <returns>ロケーションマスタ</returns>
    /// <create>T.Wakamatsu 2013/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetLocation(DatabaseHelper dbHelper, CondI01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       ML.SHUKKA_FLAG");
            sb.ApdL("     , COM1.ITEM_NAME AS SHUKKA_FLAG_NAME");
            sb.ApdL("     , ML.BUKKEN_NO");
            sb.ApdL("     , MB.BUKKEN_NAME");
            sb.ApdL("     , ML.LOCATION");
            sb.ApdL("     , ML.VERSION");
            sb.ApdL("  FROM");
            sb.ApdL("       M_LOCATION ML");
            sb.ApdL("  INNER JOIN M_BUKKEN MB ON MB.SHUKKA_FLAG = ML.SHUKKA_FLAG");
            sb.ApdL("                         AND MB.BUKKEN_NO = ML.BUKKEN_NO");
            sb.ApdL("  LEFT JOIN M_COMMON COM1 ON COM1.GROUP_CD = 'SHUKKA_FLAG'");
            sb.ApdL("                         AND COM1.VALUE1 = ML.SHUKKA_FLAG");
            sb.ApdN("                         AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL(" WHERE");
            sb.ApdN("       ML.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND ML.BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdN("   AND ML.LOCATION = ").ApdN(this.BindPrefix).ApdL("LOCATION");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       ML.SHUKKA_FLAG");
            sb.ApdL("     , ML.LOCATION");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));
            paramCollection.Add(iNewParam.NewDbParameter("LOCATION", cond.Location));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_LOCATION.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region ロケーション取得(あいまい検索)

    /// --------------------------------------------------
    /// <summary>
    /// ロケーション取得(あいまい検索)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <returns>ロケーションマスタ</returns>
    /// <create>T.Wakamatsu 2013/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetLocationLikeSearch(DatabaseHelper dbHelper, CondI01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       ML.SHUKKA_FLAG");
            sb.ApdL("     , COM1.ITEM_NAME AS SHUKKA_FLAG_NAME");
            sb.ApdL("     , ML.BUKKEN_NO");
            sb.ApdL("     , MB.BUKKEN_NAME");
            sb.ApdL("     , ML.LOCATION");
            sb.ApdL("     , ML.VERSION");
            sb.ApdL("  FROM");
            sb.ApdL("       M_LOCATION ML");
            sb.ApdL("  INNER JOIN M_BUKKEN MB ON MB.SHUKKA_FLAG = ML.SHUKKA_FLAG");
            sb.ApdL("                         AND MB.BUKKEN_NO = ML.BUKKEN_NO");
            sb.ApdL("  LEFT JOIN M_COMMON COM1 ON COM1.GROUP_CD = 'SHUKKA_FLAG'");
            sb.ApdL("                         AND COM1.VALUE1 = ML.SHUKKA_FLAG");
            sb.ApdN("                         AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL(" WHERE");
            sb.ApdN("       ML.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND ML.BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            if (!string.IsNullOrEmpty(cond.Location))
            {
                sb.ApdN("   AND ML.LOCATION LIKE ").ApdN(this.BindPrefix).ApdL("LOCATION");
                paramCollection.Add(iNewParam.NewDbParameter("LOCATION", cond.Location + "%"));
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("       ML.SHUKKA_FLAG");
            sb.ApdL("     , ML.LOCATION");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_LOCATION.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 指定ロケーションのロケーション情報数を取得(完全一致・ロケーション必須)

    /// --------------------------------------------------
    /// <summary>
    /// 指定ロケーションのロケーション情報数を取得(完全一致・ロケーション称必須)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <returns>ロケーションマスタ件数</returns>
    /// <create>T.Wakamatsu 2013/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public int GetLocationCount(DatabaseHelper dbHelper, CondI01 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       COUNT(1) AS CNT");
            sb.ApdL("  FROM");
            sb.ApdL("       M_LOCATION ML");
            sb.ApdL(" WHERE");
            sb.ApdN("       ML.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND ML.BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdN("   AND ML.LOCATION = ").ApdN(this.BindPrefix).ApdL("LOCATION");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LOCATION", cond.Location));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_CNT);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 在庫存在チェック

    /// --------------------------------------------------
    /// <summary>
    /// 在庫存在チェック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <returns>在庫が存在するか</returns>
    /// <create>T.Wakamatsu 2013/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool ExistsZaiko(DatabaseHelper dbHelper, CondI01 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       COUNT(1) AS CNT");
            sb.ApdL("  FROM");
            sb.ApdL("       T_STOCK ST");
            sb.ApdL(" INNER JOIN M_LOCATION ML ON ML.SHUKKA_FLAG = ST.SHUKKA_FLAG");
            sb.ApdL("                         AND ML.BUKKEN_NO = ST.BUKKEN_NO");
            sb.ApdL("                         AND ML.LOCATION = ST.LOCATION");
            sb.ApdL(" WHERE");
            sb.ApdN("       ML.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND ML.BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdN("   AND ML.LOCATION = ").ApdN(this.BindPrefix).ApdL("LOCATION");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LOCATION", cond.Location));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_CNT) > 0;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region INSERT

    /// --------------------------------------------------
    /// <summary>
    /// ロケーションの追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsLocation(DatabaseHelper dbHelper, CondI01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO M_LOCATION");
            sb.ApdL("(");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("     , BUKKEN_NO");
            sb.ApdL("     , LOCATION");
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
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LOCATION");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdL(")");

            paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));
            paramCollection.Add(iNewParam.NewDbParameter("LOCATION", cond.Location));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
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

    #region UPDATE

    /// --------------------------------------------------
    /// <summary>
    /// ロケーションの更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">更新データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdLocation(DatabaseHelper dbHelper, CondI01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE M_LOCATION");
            sb.ApdL("SET");
            sb.ApdN("       UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdN("   AND LOCATION = ").ApdN(this.BindPrefix).ApdL("LOCATION");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_ID", this.GetMainteUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_NAME", this.GetMainteUserName(cond)));

                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFldObject(dr, Def_M_LOCATION.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", ComFunc.GetFldObject(dr, Def_M_LOCATION.BUKKEN_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("LOCATION", ComFunc.GetFldObject(dr, Def_M_LOCATION.LOCATION)));

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

    #endregion

    #region DELETE

    /// --------------------------------------------------
    /// <summary>
    /// ロケーションの削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelLocation(DatabaseHelper dbHelper, CondI01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM M_LOCATION");
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdN("   AND LOCATION = ").ApdN(this.BindPrefix).ApdL("LOCATION");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFldObject(dr, Def_M_LOCATION.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", ComFunc.GetFldObject(dr, Def_M_LOCATION.BUKKEN_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("LOCATION", ComFunc.GetFldObject(dr, Def_M_LOCATION.LOCATION)));

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

    #endregion

    #endregion

    #endregion

    #region I0100020:入庫設定

    #region 制御

    #region 入庫処理

    /// --------------------------------------------------
    /// <summary>
    /// 入庫処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Wakamatsu 2013/08/01</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool InsZaikoData(DatabaseHelper dbHelper, CondI01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // データチェック（木枠データが2:梱包完了か、9:出荷済の場合だけ処理を進める）
            if (this.HasPackedKiwaku(dbHelper, cond, dt))
            {
                // 木枠梱包未完了です。梱包登録で確認して下さい。
                errMsgID = "I0100020010";
                return false;
            }

            // ロケーションチェック
            DataTable dtLoc = dt.DefaultView.ToTable(true, ComDefine.FLD_NYUKO_LOCATION);
            if (!this.ExistsLocation(dbHelper, cond, dtLoc))
            {
                // 入庫先Locationが存在しません。再表示後Locationを選択しなおしてください。
                errMsgID = "I0100020012";
                return false; ;
            }

            // 新規登録テーブル
            DataTable dtIns = dt.Copy();
            // 更新テーブル
            DataTable dtUpd = this.LockStock(dbHelper, ref dtIns);

            // 出荷対象が存在すれば出荷済みに更新
            bool ret = this.UpdShukkaUkeire(dbHelper, cond, dt, ref errMsgID, ref args);
            if (!ret) return false;

            // 在庫登録
            int updCnt = this.InsZaiko(dbHelper, cond, dtIns);
            updCnt += this.UpdZaiko(dbHelper, cond, dtUpd);

            // 区分更新
            foreach (DataRow dr in dtIns.Rows)
            {
                dr[Def_T_STOCK.STATUS] = ZAIKO_STATUS.ZAIKO_VALUE1;
            }
            foreach (DataRow dr in dtUpd.Rows)
            {
                dr[Def_T_STOCK.STATUS] = ZAIKO_STATUS.IDO_ZAIKO_VALUE1;
            }

            // 実績登録
            if (cond.SagyoFlag == ZAIKO_SAGYO_FLAG.TANAOROSHI_ZAIKO_VALUE1)
            {
                cond.SagyoFlag = ZAIKO_SAGYO_FLAG.TANAOROSHI_ZAIKO_VALUE1;
                this.InsJisseki(dbHelper, cond, dtIns);
                cond.SagyoFlag = ZAIKO_SAGYO_FLAG.TANAOROSHI_KANRYO_VALUE1;
                this.InsJisseki(dbHelper, cond, dtUpd);
                cond.SagyoFlag = ZAIKO_SAGYO_FLAG.TANAOROSHI_ZAIKO_VALUE1;
                this.InsJisseki(dbHelper, cond, dtUpd);
            }
            else
            {
                cond.SagyoFlag = ZAIKO_SAGYO_FLAG.ZAIKO_VALUE1;
                this.InsJisseki(dbHelper, cond, dtIns);
                cond.SagyoFlag = ZAIKO_SAGYO_FLAG.KANRYO_VALUE1;
                this.InsJisseki(dbHelper, cond, dtUpd);
                cond.SagyoFlag = ZAIKO_SAGYO_FLAG.ZAIKO_VALUE1;
                this.InsJisseki(dbHelper, cond, dtUpd);
            }
            ret = (updCnt > 0);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region クエリ

    #region 出荷・在庫データ取得
    /// --------------------------------------------------
    /// <summary>
    /// 出荷・在庫データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <returns>出荷・在庫データ</returns>
    /// <create>T.Wakamatsu 2013/07/30</create>
    /// <update>H.Tajimi 2015/11/20 備考対応</update>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetShukkaZaiko(DatabaseHelper dbHelper, CondI01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       SM.SHUKKA_FLAG + RIGHT('0000' + CONVERT(nvarchar, SM.TAG_NONYUSAKI_CD), 4) + SM.TAG_NO AS TAG_CODE");
            sb.ApdL("     , '' AS NYUKO_LOCATION");
            sb.ApdL("     , STK.STATUS");
            sb.ApdL("     , COM1.ITEM_NAME AS STATUS_NAME");
            sb.ApdL("     , STK.LOCATION");
            sb.ApdL("     , STK.STOCK_DATE");
            sb.ApdL("     , SM.SHUKKA_FLAG");
            sb.ApdL("     , SM.NONYUSAKI_CD");
            sb.ApdL("     , SM.TAG_NO");
            sb.ApdL("     , SM.SEIBAN");
            sb.ApdL("     , SM.CODE");
            sb.ApdL("     , SM.ZUMEN_OIBAN");
            sb.ApdL("     , SM.AREA");
            sb.ApdL("     , SM.FLOOR");
            sb.ApdL("     , SM.KISHU");
            sb.ApdL("     , SM.ST_NO");
            sb.ApdL("     , SM.HINMEI_JP");
            sb.ApdL("     , SM.HINMEI");
            sb.ApdL("     , SM.ZUMEN_KEISHIKI");
            sb.ApdL("     , SM.KUWARI_NO");
            sb.ApdL("     , SM.NUM");
            sb.ApdL("     , MN.NONYUSAKI_NAME");
            sb.ApdL("     , MN.SHIP");
            sb.ApdL("     , SM.AR_NO");
            sb.ApdL("     , SM.SHUKA_DATE");
            sb.ApdL("     , SM.BOX_NO");
            sb.ApdL("     , SM.PALLET_NO");
            sb.ApdL("     , SM.KOJI_NO");
            sb.ApdL("     , SM.CASE_ID");
            sb.ApdL("     , KW.SHIP + '-' + CONVERT(nvarchar, KM.CASE_NO) AS KIWAKU_NO");
            sb.ApdL("     , SM.BOXKONPO_DATE");
            sb.ApdL("     , SM.PALLETKONPO_DATE");
            sb.ApdL("     , SM.KIWAKUKONPO_DATE");
            sb.ApdL("     , SM.SHUKKA_DATE");
            sb.ApdL("     , SM.UNSOKAISHA_NAME");
            sb.ApdL("     , SM.INVOICE_NO");
            sb.ApdL("     , SM.OKURIJYO_NO");
            sb.ApdL("     , SM.BL_NO");
            sb.ApdL("     , SM.UKEIRE_DATE");
            sb.ApdL("     , SM.UKEIRE_USER_NAME");
            sb.ApdL("     , MN.BUKKEN_NO");
            sb.ApdL("     , STK.VERSION");
            sb.ApdL("     , SM.BIKO");
            sb.ApdL("     , SM.M_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI SM");
            sb.ApdL("  INNER JOIN M_NONYUSAKI MN ON MN.SHUKKA_FLAG = SM.SHUKKA_FLAG");
            sb.ApdL("                         AND MN.NONYUSAKI_CD = SM.NONYUSAKI_CD");
            sb.ApdL("  LEFT JOIN T_STOCK STK ON STK.SHUKKA_FLAG = SM.SHUKKA_FLAG");
            sb.ApdL("                         AND STK.NONYUSAKI_CD = SM.NONYUSAKI_CD");
            sb.ApdL("                         AND STK.TAG_NO = SM.TAG_NO");
            sb.ApdL("  LEFT JOIN T_KIWAKU KW ON KW.KOJI_NO = SM.KOJI_NO");
            sb.ApdL("  LEFT JOIN T_KIWAKU_MEISAI KM ON KM.CASE_ID = SM.CASE_ID");
            sb.ApdL("  LEFT JOIN M_COMMON COM1 ON COM1.GROUP_CD = 'ZAIKO_STATUS'");
            sb.ApdL("                         AND COM1.VALUE1 = STK.STATUS");
            sb.ApdN("                         AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL(" WHERE 1 = 1");
            
            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));

            if (!string.IsNullOrEmpty(cond.ShukkaFlag))
            {
                sb.ApdN("   AND SM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            }
            if (!string.IsNullOrEmpty(cond.BukkenNo))
            {
                sb.ApdN("   AND MN.BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
                paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));
            }
            if (cond.ZaikoNo.StartsWith(ZAIKO_TANI.PALLET_VALUE1))
            {
                sb.ApdN("   AND SM.PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", cond.ZaikoNo));
            }
            else if (cond.ZaikoNo.StartsWith(ZAIKO_TANI.BOX_VALUE1))
            {
                sb.ApdN("   AND SM.BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
                paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", cond.ZaikoNo));
            }
            else
            {
                sb.ApdN("   AND SM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG2");
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG2", cond.ZaikoNo.Substring(0, 1)));
                sb.ApdN("   AND SM.TAG_NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.ZaikoNo.Substring(1, 4)));
                sb.ApdN("   AND SM.TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", cond.ZaikoNo.Substring(5)));
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("       SM.TAG_NO");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_STOCK.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 在庫データ取得＆ロック

    /// --------------------------------------------------
    /// <summary>
    /// 在庫データ取得＆ロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dtTag">データ</param>
    /// <returns>在庫データ</returns>
    /// <create>T.Wakamatsu 2013/08/02</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable LockStock(DatabaseHelper dbHelper, ref DataTable dtTag)
    {
        try
        {
            DataTable retDt = new DataTable();
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , TAG_NO");
            sb.ApdL("     , LOCATION");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("NYUKO_LOCATION AS NYUKO_LOCATION");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("STATUS AS STATUS");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("STOCK_DATE AS STOCK_DATE");
            sb.ApdL("     , VERSION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_STOCK");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");

            foreach (DataRow dr in dtTag.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("NYUKO_LOCATION", ComFunc.GetFld(dr, ComDefine.FLD_NYUKO_LOCATION)));
                paramCollection.Add(iNewParam.NewDbParameter("STATUS", ComFunc.GetFld(dr, Def_T_STOCK.STATUS)));
                paramCollection.Add(iNewParam.NewDbParameter("STOCK_DATE", ComFunc.GetFld(dr, Def_T_STOCK.STOCK_DATE)));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.TAG_NO)));

                // SQL実行
                DataTable dt = new DataTable();
                dbHelper.Fill(sb.ToString(), paramCollection, dt);

                if (dt.Rows.Count > 0)
                {
                    retDt.Merge(dt);
                    dr.Delete();
                }
            }
            dtTag.AcceptChanges();

            retDt.Columns.Add(Def_T_STOCK.TAG_CODE, typeof(String));
            foreach (DataRow dr in retDt.Rows)
            {
                dr[Def_T_STOCK.TAG_CODE] = ComFunc.GetFld(dr, Def_T_STOCK.SHUKKA_FLAG)
                    + ComFunc.GetFld(dr, Def_T_STOCK.NONYUSAKI_CD).PadLeft(4, '0')
                    + ComFunc.GetFld(dr, Def_T_STOCK.TAG_NO);
            }

            return retDt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 木枠梱包未完了データ取得
    /// --------------------------------------------------
    /// <summary>
    /// 木枠梱包未完了データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <returns>出荷・在庫データ</returns>
    /// <create>T.Wakamatsu 2013/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool HasPackedKiwaku(DatabaseHelper dbHelper, CondI01 cond, DataTable dt)
    {
        try
        {
            bool ret = false;
            DataTable dtMikan = new DataTable();
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       1");
            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI SM");
            sb.ApdL("  LEFT JOIN T_KIWAKU KW ON KW.KOJI_NO = SM.KOJI_NO");
            sb.ApdL("  LEFT JOIN T_KIWAKU_MEISAI KM ON KM.CASE_ID = SM.CASE_ID");
            sb.ApdL(" WHERE");
            sb.ApdL("       KW.SAGYO_FLAG IN (").ApdN(this.BindPrefix).ApdN("SAGYO_FLAG1").ApdN(", ").ApdN(this.BindPrefix).ApdN("SAGYO_FLAG2").ApdL(")");

            if (cond.ZaikoNo.StartsWith(ZAIKO_TANI.PALLET_VALUE1))
            {
                sb.ApdN("   AND SM.PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");

                DbParamCollection paramCollection = new DbParamCollection();
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", cond.ZaikoNo));
                paramCollection.Add(iNewParam.NewDbParameter("SAGYO_FLAG1", SAGYO_FLAG.KIWAKUMEISAI_VALUE1));
                paramCollection.Add(iNewParam.NewDbParameter("SAGYO_FLAG2", SAGYO_FLAG.KONPOTOROKU_VALUE1));

                // SQL実行
                dbHelper.Fill(sb.ToString(), paramCollection, dtMikan);
                ret = (dtMikan.Rows.Count > 0);
            }
            else if (cond.ZaikoNo.StartsWith(ZAIKO_TANI.BOX_VALUE1))
            {
                sb.ApdN("   AND SM.BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");

                DbParamCollection paramCollection = new DbParamCollection();
                paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", cond.ZaikoNo));
                paramCollection.Add(iNewParam.NewDbParameter("SAGYO_FLAG1", SAGYO_FLAG.KIWAKUMEISAI_VALUE1));
                paramCollection.Add(iNewParam.NewDbParameter("SAGYO_FLAG2", SAGYO_FLAG.KONPOTOROKU_VALUE1));

                // SQL実行
                dbHelper.Fill(sb.ToString(), paramCollection, dtMikan);
                ret = (dtMikan.Rows.Count > 0);
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    sb.ApdN("   AND SM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
                    sb.ApdN("   AND SM.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
                    sb.ApdN("   AND SM.TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");

                    DbParamCollection paramCollection = new DbParamCollection();
                    paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG)));
                    paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD)));
                    paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.TAG_NO)));
                    paramCollection.Add(iNewParam.NewDbParameter("SAGYO_FLAG1", SAGYO_FLAG.KIWAKUMEISAI_VALUE1));
                    paramCollection.Add(iNewParam.NewDbParameter("SAGYO_FLAG2", SAGYO_FLAG.KONPOTOROKU_VALUE1));

                    // SQL実行
                    dbHelper.Fill(sb.ToString(), paramCollection, dtMikan);
                    ret = (dtMikan.Rows.Count > 0);

                    if (ret) break;
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

    #region 入庫処理

    #region INSERT

    /// --------------------------------------------------
    /// <summary>
    /// 入庫処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/08/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsZaiko(DatabaseHelper dbHelper, CondI01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_STOCK");
            sb.ApdL("(");
            sb.ApdL("       TAG_CODE");
            sb.ApdL("     , BUKKEN_NO");
            sb.ApdL("     , SHUKKA_FLAG");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , TAG_NO");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , LOCATION");
            sb.ApdL("     , STATUS");
            sb.ApdL("     , STOCK_DATE");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL(")");
            sb.ApdL("SELECT");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("TAG_CODE");
            sb.ApdN("     , MN.BUKKEN_NO");
            sb.ApdL("     , SM.SHUKKA_FLAG");
            sb.ApdL("     , SM.NONYUSAKI_CD");
            sb.ApdL("     , SM.TAG_NO");
            sb.ApdN("     , SM.BOX_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LOCATION");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("STATUS");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("STOCK_DATE");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI SM");
            sb.ApdL(" INNER JOIN M_NONYUSAKI MN ON MN.SHUKKA_FLAG = SM.SHUKKA_FLAG");
            sb.ApdL("                         AND MN.NONYUSAKI_CD = SM.NONYUSAKI_CD");
            sb.ApdL("  LEFT JOIN T_STOCK STK ON STK.SHUKKA_FLAG = SM.SHUKKA_FLAG");
            sb.ApdL("                         AND STK.NONYUSAKI_CD = SM.NONYUSAKI_CD");
            sb.ApdL("                         AND STK.TAG_NO = SM.TAG_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       SM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND SM.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND SM.TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");

            foreach (DataRow dr in dt.Rows)
	        {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.TAG_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("TAG_CODE", ComFunc.GetFld(dr, Def_T_STOCK.TAG_CODE)));
                paramCollection.Add(iNewParam.NewDbParameter("LOCATION", ComFunc.GetFld(dr, ComDefine.FLD_NYUKO_LOCATION)));
                paramCollection.Add(iNewParam.NewDbParameter("STATUS", ZAIKO_STATUS.ZAIKO_VALUE1));
                paramCollection.Add(iNewParam.NewDbParameter("STOCK_DATE", cond.StockDate));
                paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

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

    #endregion

    #region UPDATE

    /// --------------------------------------------------
    /// <summary>
    /// 入庫処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/08/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdZaiko(DatabaseHelper dbHelper, CondI01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_STOCK");
            sb.ApdL("SET");
            sb.ApdN("       LOCATION = ").ApdN(this.BindPrefix).ApdL("LOCATION");
            sb.ApdN("     , STATUS = ").ApdN(this.BindPrefix).ApdL("STATUS");
            sb.ApdN("     , STOCK_DATE = ").ApdN(this.BindPrefix).ApdL("STOCK_DATE");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();
                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.TAG_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("LOCATION", ComFunc.GetFld(dr, ComDefine.FLD_NYUKO_LOCATION)));
                paramCollection.Add(iNewParam.NewDbParameter("STATUS", ZAIKO_STATUS.IDO_ZAIKO_VALUE1));
                paramCollection.Add(iNewParam.NewDbParameter("STOCK_DATE", cond.StockDate));
                paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

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

    #endregion

    #endregion

    #endregion

    #endregion

    #region I0100030:出庫設定

    #region 制御

    #region 完了処理

    /// --------------------------------------------------
    /// <summary>
    /// 完了処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Wakamatsu 2013/08/13</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool DelKanryoData(DatabaseHelper dbHelper, CondI01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 在庫登録
            int cnt = this.DelKanryo(dbHelper, dt);

            // 実績登録
            if (cnt > 0)
            {
                cond.SagyoFlag = ZAIKO_SAGYO_FLAG.KANRYO_VALUE1;
                this.InsJisseki(dbHelper, cond, dt);
            }

            return (cnt > 0);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region クエリ

    #region 完了データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 完了データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <returns>在庫データ</returns>
    /// <create>T.Wakamatsu 2013/08/13</create>
    /// <update>H.Tajimi 2015/11/20 備考対応</update>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetKanryoData(DatabaseHelper dbHelper, CondI01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       STK.TAG_CODE");
            sb.ApdL("     , STK.STATUS");
            sb.ApdL("     , COM1.ITEM_NAME AS STATUS_NAME");
            sb.ApdL("     , STK.LOCATION");
            sb.ApdL("     , STK.STOCK_DATE");
            sb.ApdL("     , SM.SHUKKA_FLAG");
            sb.ApdL("     , SM.NONYUSAKI_CD");
            sb.ApdL("     , SM.TAG_NO");
            sb.ApdL("     , SM.SEIBAN");
            sb.ApdL("     , SM.CODE");
            sb.ApdL("     , SM.ZUMEN_OIBAN");
            sb.ApdL("     , SM.AREA");
            sb.ApdL("     , SM.FLOOR");
            sb.ApdL("     , SM.KISHU");
            sb.ApdL("     , SM.ST_NO");
            sb.ApdL("     , SM.HINMEI_JP");
            sb.ApdL("     , SM.HINMEI");
            sb.ApdL("     , SM.ZUMEN_KEISHIKI");
            sb.ApdL("     , SM.KUWARI_NO");
            sb.ApdL("     , SM.NUM");
            sb.ApdL("     , MN.NONYUSAKI_NAME");
            sb.ApdL("     , MN.SHIP");
            sb.ApdL("     , SM.AR_NO");
            sb.ApdL("     , SM.SHUKA_DATE");
            sb.ApdL("     , SM.BOX_NO");
            sb.ApdL("     , SM.PALLET_NO");
            sb.ApdL("     , SM.KOJI_NO");
            sb.ApdL("     , SM.CASE_ID");
            sb.ApdL("     , KW.SHIP + '-' + CONVERT(nvarchar, KM.CASE_NO) AS KIWAKU_NO");
            sb.ApdL("     , SM.BOXKONPO_DATE");
            sb.ApdL("     , SM.PALLETKONPO_DATE");
            sb.ApdL("     , SM.KIWAKUKONPO_DATE");
            sb.ApdL("     , SM.SHUKKA_DATE");
            sb.ApdL("     , SM.UNSOKAISHA_NAME");
            sb.ApdL("     , SM.INVOICE_NO");
            sb.ApdL("     , SM.OKURIJYO_NO");
            sb.ApdL("     , SM.BL_NO");
            sb.ApdL("     , SM.UKEIRE_DATE");
            sb.ApdL("     , SM.UKEIRE_USER_NAME");
            sb.ApdL("     , STK.VERSION");
            sb.ApdL("     , SM.BIKO");
            sb.ApdL("     , SM.M_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_STOCK STK");
            sb.ApdL("  INNER JOIN M_NONYUSAKI MN ON MN.SHUKKA_FLAG = STK.SHUKKA_FLAG");
            sb.ApdL("                         AND MN.NONYUSAKI_CD = STK.NONYUSAKI_CD");
            sb.ApdL("  INNER JOIN T_SHUKKA_MEISAI SM ON SM.SHUKKA_FLAG = STK.SHUKKA_FLAG");
            sb.ApdL("                         AND SM.NONYUSAKI_CD = STK.NONYUSAKI_CD");
            sb.ApdL("                         AND SM.TAG_NO = STK.TAG_NO");
            sb.ApdL("  LEFT JOIN T_KIWAKU KW ON KW.KOJI_NO = SM.KOJI_NO");
            sb.ApdL("  LEFT JOIN T_KIWAKU_MEISAI KM ON KM.CASE_ID = SM.CASE_ID");
            sb.ApdL("  LEFT JOIN M_COMMON COM1 ON COM1.GROUP_CD = 'ZAIKO_STATUS'");
            sb.ApdL("                         AND COM1.VALUE1 = STK.STATUS");
            sb.ApdN("                         AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL(" WHERE");
            
            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            
            if (cond.ZaikoNo.StartsWith(ZAIKO_TANI.PALLET_VALUE1))
            {
                return ds;
            }
            else if (cond.ZaikoNo.StartsWith(ZAIKO_TANI.BOX_VALUE1))
            {
                sb.ApdN("       SM.BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
                paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", cond.ZaikoNo));
            }
            else
            {
                sb.ApdN("       STK.TAG_CODE = ").ApdN(this.BindPrefix).ApdL("TAG_CODE");
                paramCollection.Add(iNewParam.NewDbParameter("TAG_CODE", cond.ZaikoNo));
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("       STK.TAG_CODE");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_STOCK.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 完了処理

    #region DELETE

    /// --------------------------------------------------
    /// <summary>
    /// 完了処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dt">データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/08/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelKanryo(DatabaseHelper dbHelper, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE");
            sb.ApdL("  FROM");
            sb.ApdL("       T_STOCK");
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFld(dr, Def_T_STOCK.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFld(dr, Def_T_STOCK.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", ComFunc.GetFld(dr, Def_T_STOCK.TAG_NO)));

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

    #endregion

    #endregion

    #endregion

    #endregion

    #region I0100040:棚卸差異照会

    #region 制御

    #region 棚卸データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 棚卸データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <returns>棚卸データ</returns>
    /// <create>T.Wakamatsu 2013/08/21</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetTanaoroshiData(DatabaseHelper dbHelper, CondI01 cond)
    {
        if (cond.Location == ComFunc.GetFld(GetLostLocation(dbHelper, cond), 0, Def_M_COMMON.ITEM_NAME))
            return GetTanaoroshiLost(dbHelper, cond);
        else
            return GetTanaoroshi(dbHelper, cond);
    }

    #endregion

    #region 棚卸処理

    /// --------------------------------------------------
    /// <summary>
    /// 棚卸処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Wakamatsu 2013/08/13</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdTanaoroshiData(DatabaseHelper dbHelper, CondI01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 不明在庫では更新しない
            if (cond.Location == ComFunc.GetFld(GetLostLocation(dbHelper, cond), 0, Def_M_COMMON.ITEM_NAME))
            {
                // Locationが不明のため更新できません。
                errMsgID = "I0100040004";
                return false;
            }

            // 在庫日はシステム日付
            cond.StockDate = DateTime.Today.ToString("yyyy/MM/dd");

            // 棚卸入庫・棚卸移動在庫
            DataTable dtIns = dt.Copy();
            foreach (DataRow dr in dtIns.Rows)
            {
                if (String.IsNullOrEmpty(ComFunc.GetFld(dr, ComDefine.FLD_NYUKO_LOCATION)))
                    dr.Delete();
            }
            dtIns.AcceptChanges();

            // 棚卸更新のロケーションチェック
            DataTable dtLoc = dtIns.DefaultView.ToTable(true, ComDefine.FLD_NYUKO_LOCATION);
            if (!this.ExistsLocation(dbHelper, cond, dtLoc))
            {
                // 入庫先Locationが存在しません。Locationを再確認してください。
                errMsgID = "I0100040002";
                return false; ;
            }

            // 在庫登録
            int cnt = dtIns.Rows.Count;
            if (cnt > 0)
            {
                foreach (DataRow dr in dtIns.Rows)
                {
                    CondI01 condI01 = new CondI01(cond.LoginInfo);
                    condI01.LoginInfo = cond.LoginInfo.Clone();
                    condI01.ZaikoNo = ComFunc.GetFld(dr, Def_T_INVENT.TAG_CODE);
                    condI01.WorkUserID = ComFunc.GetFld(dr, Def_T_INVENT.WORK_USER_ID);
                    condI01.StockDate = DateTime.Now.ToString("yyyy/MM/dd");

                    DataTable dtTag = this.GetShukkaZaiko(dbHelper, condI01).Tables[0];
                    condI01.ShukkaFlag = ComFunc.GetFld(dtTag, 0, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG);
                    condI01.BukkenNo = ComFunc.GetFld(dtTag, 0, Def_M_NONYUSAKI.BUKKEN_NO);
                    condI01.SagyoFlag = ZAIKO_SAGYO_FLAG.TANAOROSHI_ZAIKO_VALUE1;
                    dtTag.Rows[0][ComDefine.FLD_NYUKO_LOCATION] = ComFunc.GetFld(dr, ComDefine.FLD_NYUKO_LOCATION);
                    bool ret = this.InsZaikoData(dbHelper, condI01, dtTag, ref errMsgID, ref args);

                    if (!ret)
                    {
                        // メッセージを変更
                        // 「入庫先Locationが存在しません。再表示後Locationを選択しなおしてください。」から下記に
                        // 入庫先Locationが存在しません。Locationを再確認してください。
                        if (errMsgID == "I0100020012")
                        {
                            errMsgID = "I0100040002";
                        }
                        return ret;
                    }
                }
            }

            this.DelTanaoroshi(dbHelper, cond);
            //this.InsJissekiTanaoroshi(dbHelper, cond, dt);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region クエリ

    #region 棚卸ロケーション取得

    /// --------------------------------------------------
    /// <summary>
    /// 棚卸ロケーション取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <returns>ロケーションマスタ</returns>
    /// <create>T.Wakamatsu 2013/10/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetTanaoroshiLocationCombo(DatabaseHelper dbHelper, CondI01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       ML.LOCATION");
            sb.ApdL("  FROM");
            sb.ApdL("       M_LOCATION ML");
            sb.ApdL(" WHERE EXISTS");
            sb.ApdL("       (SELECT 1"); ;
            sb.ApdL("          FROM T_INVENT IVT");
            sb.ApdL("          LEFT JOIN T_STOCK STK ON STK.SHUKKA_FLAG = IVT.SHUKKA_FLAG");
            sb.ApdL("                               AND STK.NONYUSAKI_CD = IVT.NONYUSAKI_CD");
            sb.ApdL("                               AND STK.TAG_NO = IVT.TAG_NO");
            sb.ApdL("         WHERE");
            sb.ApdL("               IVT.SHUKKA_FLAG = ML.SHUKKA_FLAG");
            sb.ApdL("           AND IVT.BUKKEN_NO = ML.BUKKEN_NO");
            sb.ApdL("           AND IVT.LOCATION = ML.LOCATION");
            sb.ApdN("           AND IVT.INVENT_DATE = ").ApdN(this.BindPrefix).ApdL("INVENT_DATE");
            sb.ApdN("           AND IVT.KANRYO_FLAG = ").ApdN(this.BindPrefix).ApdL("KANRYO_FLAG");
            sb.ApdL("           AND (IVT.LOCATION != STK.LOCATION OR STK.LOCATION IS NULL)");
            sb.ApdL("       )");
            sb.ApdN("   AND ML.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND ML.BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       ML.LOCATION");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));
            paramCollection.Add(iNewParam.NewDbParameter("INVENT_DATE", cond.InventDate));
            paramCollection.Add(iNewParam.NewDbParameter("KANRYO_FLAG", ZAIKO_KANRYO_FLAG.MIKAN_VALUE1));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_LOCATION.Name);

            // 棚卸データがあれば不明在庫追加
            sb = new StringBuilder();
            paramCollection = new DbParamCollection();

            sb.ApdL("SELECT");
            sb.ApdL("       ITEM_NAME AS LOCATION");
            sb.ApdL("  FROM");
            sb.ApdL("       M_COMMON");
            sb.ApdL(" WHERE");
            sb.ApdN("       GROUP_CD = ").ApdN(this.BindPrefix).ApdL("GROUP_CD");
            sb.ApdN("   AND LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdN("   AND VALUE1 = ").ApdN(this.BindPrefix).ApdL("VALUE1");
            sb.ApdL("   AND EXISTS");
            sb.ApdL("       (SELECT 1"); ;
            sb.ApdL("          FROM T_STOCK STK");
            sb.ApdL("          LEFT JOIN T_INVENT IVT ON IVT.SHUKKA_FLAG = STK.SHUKKA_FLAG");
            sb.ApdL("                                AND IVT.NONYUSAKI_CD = STK.NONYUSAKI_CD");
            sb.ApdL("                                AND IVT.TAG_NO = STK.TAG_NO");
            sb.ApdN("                                AND IVT.INVENT_DATE = ").ApdN(this.BindPrefix).ApdL("INVENT_DATE");
            sb.ApdL("         WHERE");
            sb.ApdN("               STK.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("           AND STK.BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdL("           AND IVT.TAG_NO IS NULL");
            sb.ApdL("       )");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));
            paramCollection.Add(iNewParam.NewDbParameter("INVENT_DATE", cond.InventDate));
            paramCollection.Add(iNewParam.NewDbParameter("GROUP_CD", ZAIKO_LOST_LOCATION.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("VALUE1", ZAIKO_LOST_LOCATION.LOST_VALUE1));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_LOCATION.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 不明ロケーション取得

    /// --------------------------------------------------
    /// <summary>
    /// 不明ロケーション取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <returns>不明ロケーションデータ</returns>
    /// <create>T.Wakamatsu 2013/10/31</create>
    /// <update>S.Furugo 2018/10/03 言語切り替え対応</update>
    /// --------------------------------------------------
    public DataTable GetLostLocation(DatabaseHelper dbHelper, CondI01 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       ITEM_NAME");
            sb.ApdL("  FROM");
            sb.ApdL("       M_COMMON");
            sb.ApdL(" WHERE");
            sb.ApdN("       GROUP_CD = ").ApdN(this.BindPrefix).ApdL("GROUP_CD");
            sb.ApdN("   AND LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdN("   AND VALUE1 = ").ApdN(this.BindPrefix).ApdL("VALUE1");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("GROUP_CD", ZAIKO_LOST_LOCATION.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("VALUE1", ZAIKO_LOST_LOCATION.LOST_VALUE1));

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

    #region 棚卸データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 棚卸データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <returns>棚卸データ</returns>
    /// <create>T.Wakamatsu 2013/08/21</create>
    /// <update>H.Tajimi 2015/12/03 物件名をDBから取得するよう変更</update>
    /// <update>H.Tajimi 2015/11/20 備考対応</update>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetTanaoroshi(DatabaseHelper dbHelper, CondI01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       SM.SHUKKA_FLAG + RIGHT('0000' + CONVERT(nvarchar, SM.TAG_NONYUSAKI_CD), 4) + SM.TAG_NO AS TAG_CODE");
            sb.ApdL("     , STK.STATUS");
            sb.ApdL("     , COM1.ITEM_NAME AS STATUS_NAME");
            sb.ApdL("     , STK.LOCATION");
            sb.ApdL("     , IVT.LOCATION NYUKO_LOCATION");
            sb.ApdL("     , IVT.WORK_USER_ID");
            sb.ApdL("     , STK.STOCK_DATE");
            sb.ApdL("     , SM.SHUKKA_FLAG");
            sb.ApdL("     , SM.NONYUSAKI_CD");
            sb.ApdL("     , SM.TAG_NO");
            sb.ApdL("     , SM.BOX_NO");
            sb.ApdL("     , SM.SEIBAN");
            sb.ApdL("     , SM.CODE");
            sb.ApdL("     , SM.ZUMEN_OIBAN");
            sb.ApdL("     , SM.AREA");
            sb.ApdL("     , SM.FLOOR");
            sb.ApdL("     , SM.KISHU");
            sb.ApdL("     , SM.ST_NO");
            sb.ApdL("     , SM.HINMEI_JP");
            sb.ApdL("     , SM.HINMEI");
            sb.ApdL("     , SM.ZUMEN_KEISHIKI");
            sb.ApdL("     , SM.KUWARI_NO");
            sb.ApdL("     , SM.NUM");
            sb.ApdL("     , MN.NONYUSAKI_NAME");
            sb.ApdL("     , MN.SHIP");
            sb.ApdL("     , SM.AR_NO");
            sb.ApdL("     , STK.VERSION");
            sb.ApdL("     , BK.BUKKEN_NAME");
            sb.ApdL("     , SM.BIKO");
            sb.ApdL("     , SM.M_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI SM");
            sb.ApdL(" INNER JOIN M_NONYUSAKI MN ON MN.SHUKKA_FLAG = SM.SHUKKA_FLAG");
            sb.ApdL("                         AND MN.NONYUSAKI_CD = SM.NONYUSAKI_CD");
            sb.ApdL("  LEFT JOIN T_STOCK STK ON STK.SHUKKA_FLAG = SM.SHUKKA_FLAG");
            sb.ApdL("                         AND STK.NONYUSAKI_CD = SM.NONYUSAKI_CD");
            sb.ApdL("                         AND STK.TAG_NO = SM.TAG_NO");
            sb.ApdL("  LEFT JOIN T_INVENT IVT ON IVT.SHUKKA_FLAG = SM.SHUKKA_FLAG");
            sb.ApdL("                         AND IVT.NONYUSAKI_CD = SM.NONYUSAKI_CD");
            sb.ApdL("                         AND IVT.TAG_NO = SM.TAG_NO");
            sb.ApdN("                         AND IVT.INVENT_DATE = ").ApdN(this.BindPrefix).ApdL("INVENT_DATE");
            sb.ApdN("                         AND IVT.KANRYO_FLAG = ").ApdN(this.BindPrefix).ApdL("KANRYO_FLAG");
            sb.ApdL("  LEFT JOIN T_KIWAKU KW ON KW.KOJI_NO = SM.KOJI_NO");
            sb.ApdL("  LEFT JOIN M_COMMON COM1 ON COM1.GROUP_CD = 'ZAIKO_STATUS'");
            sb.ApdL("                         AND COM1.VALUE1 = STK.STATUS");
            sb.ApdN("                         AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_BUKKEN BK ON BK.SHUKKA_FLAG = MN.SHUKKA_FLAG");
            sb.ApdL("                       AND BK.BUKKEN_NO = MN.BUKKEN_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       SM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND MN.BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdN("   AND IVT.LOCATION = ").ApdN(this.BindPrefix).ApdL("LOCATION");
            sb.ApdL("   AND (STK.LOCATION != IVT.LOCATION OR STK.LOCATION IS NULL)");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       SM.TAG_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));
            paramCollection.Add(iNewParam.NewDbParameter("LOCATION", cond.Location));
            paramCollection.Add(iNewParam.NewDbParameter("INVENT_DATE", cond.InventDate));
            paramCollection.Add(iNewParam.NewDbParameter("KANRYO_FLAG", ZAIKO_KANRYO_FLAG.MIKAN_VALUE1));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_STOCK.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 棚卸不明データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 棚卸不明データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <returns>棚卸データ</returns>
    /// <create>T.Wakamatsu 2013/10/31</create>
    /// <update>H.Tajimi 2015/12/03 物件名をDBから取得するよう変更</update>
    /// <update>H.Tajimi 2015/11/20 備考対応</update>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetTanaoroshiLost(DatabaseHelper dbHelper, CondI01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       SM.SHUKKA_FLAG + RIGHT('0000' + CONVERT(nvarchar, SM.TAG_NONYUSAKI_CD), 4) + SM.TAG_NO AS TAG_CODE");
            sb.ApdL("     , STK.STATUS");
            sb.ApdL("     , COM1.ITEM_NAME AS STATUS_NAME");
            sb.ApdL("     , STK.LOCATION");
            sb.ApdL("     , COM2.ITEM_NAME NYUKO_LOCATION");
            sb.ApdL("     , IVT.WORK_USER_ID");
            sb.ApdL("     , STK.STOCK_DATE");
            sb.ApdL("     , SM.SHUKKA_FLAG");
            sb.ApdL("     , SM.NONYUSAKI_CD");
            sb.ApdL("     , SM.TAG_NO");
            sb.ApdL("     , SM.BOX_NO");
            sb.ApdL("     , SM.SEIBAN");
            sb.ApdL("     , SM.CODE");
            sb.ApdL("     , SM.ZUMEN_OIBAN");
            sb.ApdL("     , SM.AREA");
            sb.ApdL("     , SM.FLOOR");
            sb.ApdL("     , SM.KISHU");
            sb.ApdL("     , SM.ST_NO");
            sb.ApdL("     , SM.HINMEI_JP");
            sb.ApdL("     , SM.HINMEI");
            sb.ApdL("     , SM.ZUMEN_KEISHIKI");
            sb.ApdL("     , SM.KUWARI_NO");
            sb.ApdL("     , SM.NUM");
            sb.ApdL("     , MN.NONYUSAKI_NAME");
            sb.ApdL("     , MN.SHIP");
            sb.ApdL("     , SM.AR_NO");
            sb.ApdL("     , STK.VERSION");
            sb.ApdL("     , BK.BUKKEN_NAME");
            sb.ApdL("     , SM.BIKO");
            sb.ApdL("     , SM.M_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI SM");
            sb.ApdL(" INNER JOIN M_NONYUSAKI MN ON MN.SHUKKA_FLAG = SM.SHUKKA_FLAG");
            sb.ApdL("                         AND MN.NONYUSAKI_CD = SM.NONYUSAKI_CD");
            sb.ApdL(" INNER JOIN T_STOCK STK ON STK.SHUKKA_FLAG = SM.SHUKKA_FLAG");
            sb.ApdL("                         AND STK.NONYUSAKI_CD = SM.NONYUSAKI_CD");
            sb.ApdL("                         AND STK.TAG_NO = SM.TAG_NO");
            sb.ApdL("  LEFT JOIN T_INVENT IVT ON IVT.SHUKKA_FLAG = SM.SHUKKA_FLAG");
            sb.ApdL("                         AND IVT.NONYUSAKI_CD = SM.NONYUSAKI_CD");
            sb.ApdL("                         AND IVT.TAG_NO = SM.TAG_NO");
            sb.ApdN("                         AND IVT.INVENT_DATE = ").ApdN(this.BindPrefix).ApdL("INVENT_DATE");
            sb.ApdL("  LEFT JOIN T_KIWAKU KW ON KW.KOJI_NO = SM.KOJI_NO");
            sb.ApdL("  LEFT JOIN M_COMMON COM1 ON COM1.GROUP_CD = 'ZAIKO_STATUS'");
            sb.ApdL("                         AND COM1.VALUE1 = STK.STATUS");
            sb.ApdN("                         AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdN("  LEFT JOIN M_COMMON COM2 ON COM2.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("GROUP_CD");
            sb.ApdN("                         AND COM2.VALUE1 = ").ApdN(this.BindPrefix).ApdL("VALUE1");
            sb.ApdN("                         AND COM2.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_BUKKEN BK ON BK.SHUKKA_FLAG = MN.SHUKKA_FLAG");
            sb.ApdL("                       AND BK.BUKKEN_NO = MN.BUKKEN_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       SM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND MN.BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdL("   AND IVT.LOCATION IS NULL");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       SM.TAG_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));
            paramCollection.Add(iNewParam.NewDbParameter("INVENT_DATE", cond.InventDate));
            paramCollection.Add(iNewParam.NewDbParameter("GROUP_CD", ZAIKO_LOST_LOCATION.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("VALUE1", ZAIKO_LOST_LOCATION.LOST_VALUE1));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_STOCK.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 棚卸処理
    
    #region INSERT


    /// --------------------------------------------------
    /// <summary>
    /// 棚卸実施実績データ登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">棚卸データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/10/21</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsJissekiTanaoroshi(DatabaseHelper dbHelper, CondI01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_JISSEKI");
            sb.ApdL("(");
            sb.ApdL("       JISSEKI_DATE");
            sb.ApdL("     , SAGYO_FLAG");
            sb.ApdL("     , WORK_USER_ID");
            sb.ApdL("     , WORK_USER_NAME");
            sb.ApdL("     , BUKKEN_NO");
            sb.ApdL("     , LOCATION");
            sb.ApdL("     , SHUKKA_FLAG");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL(")");
            sb.ApdL("SELECT");
            sb.ApdN("       ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SAGYO_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("WORK_USER_ID");
            sb.ApdL("     , MU.USER_NAME");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LOCATION");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdL("  FROM");
            sb.ApdL("       M_USER MU");
            sb.ApdL(" WHERE");
            sb.ApdN("       MU.USER_ID = ").ApdN(this.BindPrefix).ApdL("WORK_USER_ID");

            DbParamCollection paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("WORK_USER_ID", cond.WorkUserID));

            paramCollection.Add(iNewParam.NewDbParameter("SAGYO_FLAG", ZAIKO_SAGYO_FLAG.TANAOROSHI_JISSHI_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));
            paramCollection.Add(iNewParam.NewDbParameter("LOCATION", cond.Location));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFld(dt, 0, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG)));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFld(dt, 0, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD)));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
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

    #region UPDATE

    /// --------------------------------------------------
    /// <summary>
    /// 棚卸処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/10/21</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelTanaoroshi(DatabaseHelper dbHelper, CondI01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_INVENT");
            sb.ApdL("SET");
            sb.ApdN("       KANRYO_FLAG = ").ApdN(this.BindPrefix).ApdL("KANRYO_FLAG");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdN("   AND LOCATION = ").ApdN(this.BindPrefix).ApdL("LOCATION");
            sb.ApdN("   AND INVENT_DATE = ").ApdN(this.BindPrefix).ApdL("INVENT_DATE");

            DbParamCollection paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));
            paramCollection.Add(iNewParam.NewDbParameter("LOCATION", cond.Location));
            paramCollection.Add(iNewParam.NewDbParameter("INVENT_DATE", cond.InventDate));
            paramCollection.Add(iNewParam.NewDbParameter("KANRYO_FLAG", ZAIKO_KANRYO_FLAG.KANRYO_VALUE1));
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

    #endregion

    #endregion

    #region I0100050:在庫問合せ・メンテ

    #region 制御

    #region 在庫データ更新

    /// --------------------------------------------------
    /// <summary>
    /// 在庫データ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dtUpd">更新データ</param>
    /// <param name="dtDel">削除データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Wakamatsu 2013/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdZaikoHoshu(DatabaseHelper dbHelper, CondI01 cond, DataTable dtUpd, DataTable dtDel, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 在庫日はシステム日付
            cond.StockDate = DateTime.Today.ToString("yyyy/MM/dd");

            // ロケーションチェック
            DataTable dtLoc = dtUpd.DefaultView.ToTable(true, ComDefine.FLD_NYUKO_LOCATION);
            if (!this.ExistsLocation(dbHelper, cond, dtLoc))
            {
                // 入庫先Locationが存在しません。再表示後Locationを選択しなおしてください。
                errMsgID = "I0100050003";
                return false; ;
            }

            // 在庫データ更新
            int updCnt = this.UpdZaikoHoshuExec(dbHelper, cond, dtUpd);

            // 実績登録
            if (updCnt > 0)
            {
                cond.SagyoFlag = ZAIKO_SAGYO_FLAG.MAINTE_KANRYO_VALUE1;
                this.InsJisseki(dbHelper, cond, dtUpd);
                cond.SagyoFlag = ZAIKO_SAGYO_FLAG.MAINTE_ZAIKO_VALUE1;
                this.InsJisseki(dbHelper, cond, dtUpd);
            }

            // 在庫データ削除
            int delCnt = this.DelZaikoHoshuExec(dbHelper, cond, dtDel);

            // 実績登録
            if (delCnt > 0)
            {
                cond.SagyoFlag = ZAIKO_SAGYO_FLAG.MAINTE_KANRYO_VALUE1;
                this.InsJisseki(dbHelper, cond, dtDel);
            }

            return (updCnt + delCnt > 0);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region クエリ

    #region 在庫メンテ権限取得

    /// --------------------------------------------------
    /// <summary>
    /// 在庫メンテ権限取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="role">ユーザー権限</param>
    /// <returns>在庫メンテ権限</returns>
    /// <create>T.Wakamatsu 2013/09/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetZaikoMainteRole(DatabaseHelper dbHelper, string role)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            string fieldName = string.Empty;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       1");
            sb.ApdL("  FROM");
            sb.ApdL("       M_COMMON");
            sb.ApdL(" WHERE");
            sb.ApdN("       GROUP_CD = ").ApdN(this.BindPrefix).ApdL("GROUP_CD");
            sb.ApdN("   AND LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdN("   AND VALUE1 = ").ApdN(this.BindPrefix).ApdL("VALUE1");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", /*cond.LoginInfo.Language*/ "JP")); //多言語化を意識する必要はないため、JP固定とする
            paramCollection.Add(iNewParam.NewDbParameter("GROUP_CD", ZAIKO_MAITE_ROLE.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("VALUE1", role));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return dt.Rows.Count > 0;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 在庫データ取得
    /// --------------------------------------------------
    /// <summary>
    /// 在庫データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <returns>在庫データ</returns>
    /// <create>T.Wakamatsu 2013/08/19</create>
    /// <update>H.Tajimi 2015/12/03 物件名をDBから取得するよう変更</update>
    /// <update>H.Tajimi 2015/11/20 備考対応</update>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetZaikoHoshu(DatabaseHelper dbHelper, CondI01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            string fieldName = string.Empty;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       SM.SHUKKA_FLAG + RIGHT('0000' + CONVERT(nvarchar, SM.TAG_NONYUSAKI_CD), 4) + SM.TAG_NO AS TAG_CODE");
            sb.ApdL("     , '' AS NYUKO_LOCATION");
            sb.ApdL("     , STK.STATUS");
            sb.ApdL("     , COM1.ITEM_NAME AS STATUS_NAME");
            sb.ApdL("     , STK.LOCATION");
            sb.ApdL("     , STK.STOCK_DATE");
            sb.ApdL("     , SM.SHUKKA_FLAG");
            sb.ApdL("     , SM.NONYUSAKI_CD");
            sb.ApdL("     , SM.TAG_NO");
            sb.ApdL("     , SM.SEIBAN");
            sb.ApdL("     , SM.CODE");
            sb.ApdL("     , SM.ZUMEN_OIBAN");
            sb.ApdL("     , SM.AREA");
            sb.ApdL("     , SM.FLOOR");
            sb.ApdL("     , SM.KISHU");
            sb.ApdL("     , SM.ST_NO");
            sb.ApdL("     , SM.HINMEI_JP");
            sb.ApdL("     , SM.HINMEI");
            sb.ApdL("     , SM.ZUMEN_KEISHIKI");
            sb.ApdL("     , SM.KUWARI_NO");
            sb.ApdL("     , SM.NUM");
            sb.ApdL("     , MN.NONYUSAKI_NAME");
            sb.ApdL("     , MN.SHIP");
            sb.ApdL("     , SM.AR_NO");
            sb.ApdL("     , SM.SHUKA_DATE");
            sb.ApdL("     , SM.BOX_NO");
            sb.ApdL("     , SM.PALLET_NO");
            sb.ApdL("     , SM.KOJI_NO");
            sb.ApdL("     , SM.CASE_ID");
            sb.ApdL("     , KW.SHIP + '-' + CONVERT(nvarchar, KM.CASE_NO) AS KIWAKU_NO");
            sb.ApdL("     , SM.BOXKONPO_DATE");
            sb.ApdL("     , SM.PALLETKONPO_DATE");
            sb.ApdL("     , SM.KIWAKUKONPO_DATE");
            sb.ApdL("     , SM.SHUKKA_DATE");
            sb.ApdL("     , SM.UNSOKAISHA_NAME");
            sb.ApdL("     , SM.INVOICE_NO");
            sb.ApdL("     , SM.OKURIJYO_NO");
            sb.ApdL("     , SM.BL_NO");
            sb.ApdL("     , SM.UKEIRE_DATE");
            sb.ApdL("     , SM.UKEIRE_USER_NAME");
            sb.ApdL("     , STK.VERSION");
            sb.ApdL("     , BK.BUKKEN_NAME");
            sb.ApdL("     , SM.BIKO");
            sb.ApdL("     , SM.M_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI SM");
            sb.ApdL("  INNER JOIN M_NONYUSAKI MN ON MN.SHUKKA_FLAG = SM.SHUKKA_FLAG");
            sb.ApdL("                         AND MN.NONYUSAKI_CD = SM.NONYUSAKI_CD");
            sb.ApdL("  INNER JOIN T_STOCK STK ON STK.SHUKKA_FLAG = SM.SHUKKA_FLAG");
            sb.ApdL("                         AND STK.NONYUSAKI_CD = SM.NONYUSAKI_CD");
            sb.ApdL("                         AND STK.TAG_NO = SM.TAG_NO");
            sb.ApdL("  LEFT JOIN T_KIWAKU KW ON KW.KOJI_NO = SM.KOJI_NO");
            sb.ApdL("  LEFT JOIN T_KIWAKU_MEISAI KM ON KM.KOJI_NO = SM.KOJI_NO");
            sb.ApdL("                               AND KM.CASE_ID = SM.CASE_ID");
            sb.ApdL("  LEFT JOIN M_COMMON COM1 ON COM1.GROUP_CD = 'ZAIKO_STATUS'");
            sb.ApdL("                         AND COM1.VALUE1 = STK.STATUS");
            sb.ApdN("                         AND LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_BUKKEN BK ON BK.SHUKKA_FLAG = MN.SHUKKA_FLAG");
            sb.ApdL("                       AND BK.BUKKEN_NO = MN.BUKKEN_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       SM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND MN.BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            // Location
            if (!string.IsNullOrEmpty(cond.Location))
            {
                fieldName = "LOCATION";
                sb.ApdN("   AND ").ApdN("STK." + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.Location));
            }
            // AR No.
            if (cond.ARNo != null)
            {
                fieldName = "AR_NO";
                sb.ApdN("   AND ").ApdN("SM." + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ARNo));
            }
            // 品名(和文)
            if (!string.IsNullOrEmpty(cond.HinmeiJp))
            {
                fieldName = "HINMEI_JP";
                sb.ApdN("   AND ").ApdN("SM." + fieldName).ApdN(" LIKE ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, "%" + cond.HinmeiJp + "%"));
            }
            // 品名
            if (!string.IsNullOrEmpty(cond.Hinmei))
            {
                fieldName = "HINMEI";
                sb.ApdN("   AND ").ApdN("SM." + fieldName).ApdN(" LIKE ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, "%" + cond.Hinmei + "%"));
            }
            // 図番/形式
            if (!string.IsNullOrEmpty(cond.ZumenKeishiki))
            {
                fieldName = "ZUMEN_KEISHIKI";
                sb.ApdN("   AND ").ApdN("SM." + fieldName).ApdN(" LIKE ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, "%" + cond.ZumenKeishiki + "%"));
            }
            // エリア
            if (!string.IsNullOrEmpty(cond.Area))
            {
                fieldName = "AREA";
                sb.ApdN("   AND ").ApdN("SM." + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.Area));
            }
            // フロア
            if (!string.IsNullOrEmpty(cond.Floor))
            {
                fieldName = "FLOOR";
                sb.ApdN("   AND ").ApdN("SM." + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.Floor));
            }
            // 区割No.
            if (!string.IsNullOrEmpty(cond.KuwariNo))
            {
                fieldName = "KUWARI_NO";
                sb.ApdN("   AND ").ApdN("SM." + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.KuwariNo));
            }
            // 木枠No/Case No/Pallet No/Box No/Tag No
            if (!string.IsNullOrEmpty(cond.KiwakuNo)
               && !string.IsNullOrEmpty(cond.CaseNo))
            {
                fieldName = "SHIP";
                sb.ApdN("   AND ").ApdN("KW." + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.KiwakuNo));
                fieldName = "CASE_NO";
                sb.ApdN("   AND ").ApdN("KM." + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.CaseNo));
            }
            else if (!string.IsNullOrEmpty(cond.KiwakuNo))
            {
                fieldName = "SHIP";
                sb.ApdN("   AND ").ApdN("KW." + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.KiwakuNo));
            }
            else if (!string.IsNullOrEmpty(cond.CaseNo))
            {
                fieldName = "CASE_NO";
                sb.ApdN("   AND ").ApdN("KM." + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.CaseNo));
            }
            else if (!string.IsNullOrEmpty(cond.PalletNo))
            {
                fieldName = "PALLET_NO";
                sb.ApdN("   AND ").ApdN("SM." + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.PalletNo));
            }
            else if (!string.IsNullOrEmpty(cond.BoxNo))
            {
                fieldName = "BOX_NO";
                sb.ApdN("   AND ").ApdN("SM." + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.BoxNo));
            }
            else if (!string.IsNullOrEmpty(cond.TagNo))
            {
                fieldName = "TAG_NO";
                sb.ApdN("   AND ").ApdN("SM." + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.TagNo));
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("       SM.TAG_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_STOCK.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 在庫データ取得＆ロック

    /// --------------------------------------------------
    /// <summary>
    /// 在庫データ取得＆ロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dtTag">データ</param>
    /// <returns>在庫データ</returns>
    /// <create>T.Wakamatsu 2013/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable LockZaikoHoshu(DatabaseHelper dbHelper, CondI01 cond, ref DataTable dtTag)
    {
        try
        {
            DataTable retDt = new DataTable();
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , TAG_NO");
            sb.ApdL("     , LOCATION");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("NYUKO_LOCATION AS NYUKO_LOCATION");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("STATUS AS STATUS");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("STOCK_DATE AS STOCK_DATE");
            sb.ApdL("     , VERSION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_STOCK");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");

            foreach (DataRow dr in dtTag.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("NYUKO_LOCATION", ComFunc.GetFld(dr, ComDefine.FLD_NYUKO_LOCATION)));
                paramCollection.Add(iNewParam.NewDbParameter("STATUS", ComFunc.GetFld(dr, Def_T_STOCK.STATUS)));
                paramCollection.Add(iNewParam.NewDbParameter("STOCK_DATE", ComFunc.GetFld(dr, Def_T_STOCK.STOCK_DATE)));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.TAG_NO)));

                // SQL実行
                DataTable dt = new DataTable();
                dbHelper.Fill(sb.ToString(), paramCollection, dt);

                if (dt.Rows.Count > 0)
                {
                    retDt.Merge(dt);
                    dr.Delete();
                }
            }
            dtTag.AcceptChanges();

            retDt.Columns.Add(Def_T_STOCK.TAG_CODE, typeof(String));
            foreach (DataRow dr in retDt.Rows)
            {
                dr[Def_T_STOCK.TAG_CODE] = ComFunc.GetFld(dr, Def_T_STOCK.SHUKKA_FLAG)
                    + ComFunc.GetFld(dr, Def_T_STOCK.NONYUSAKI_CD).PadLeft(4, '0')
                    + ComFunc.GetFld(dr, Def_T_STOCK.TAG_NO);
            }

            return retDt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region Location存在チェック

    /// --------------------------------------------------
    /// <summary>
    /// Location存在チェック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dtUpd">更新データ</param>
    /// <returns>Location存在</returns>
    /// <create>T.Wakamatsu 2013/09/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool ExistsLocation(DatabaseHelper dbHelper, CondI01 cond, DataTable dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;
            string fieldName = string.Empty;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       1");
            sb.ApdL("  FROM");
            sb.ApdL("       M_LOCATION");
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdN("   AND LOCATION = ").ApdN(this.BindPrefix).ApdL("LOCATION");

            foreach (DataRow dr in dt.Rows)
            {
                // バインド変数設定
                DbParamCollection paramCollection = new DbParamCollection();
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
                paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));
                paramCollection.Add(iNewParam.NewDbParameter("LOCATION", ComFunc.GetFld(dr, ComDefine.FLD_NYUKO_LOCATION)));
                
                // SQL実行
                DataTable dtLoc = new DataTable();
                dbHelper.Fill(sb.ToString(), paramCollection, dtLoc);

                if (dtLoc.Rows.Count == 0)
                    return false;
            }

            return true;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 在庫メンテナンス処理

    #region UPDATE

    /// --------------------------------------------------
    /// <summary>
    /// 在庫データ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdZaikoHoshuExec(DatabaseHelper dbHelper, CondI01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_STOCK");
            sb.ApdL("SET");
            sb.ApdN("       LOCATION = ").ApdN(this.BindPrefix).ApdL("LOCATION");
            sb.ApdN("     , STATUS = ").ApdN(this.BindPrefix).ApdL("STATUS");
            sb.ApdN("     , STOCK_DATE = ").ApdN(this.BindPrefix).ApdL("STOCK_DATE");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.TAG_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("LOCATION", ComFunc.GetFld(dr, ComDefine.FLD_NYUKO_LOCATION)));
                paramCollection.Add(iNewParam.NewDbParameter("STATUS", ZAIKO_STATUS.IDO_ZAIKO_VALUE1));
                paramCollection.Add(iNewParam.NewDbParameter("STOCK_DATE", cond.StockDate));
                paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

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

    #endregion

    #region DELETE

    /// --------------------------------------------------
    /// <summary>
    /// 在庫データ削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelZaikoHoshuExec(DatabaseHelper dbHelper, CondI01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE");
            sb.ApdL("  FROM");
            sb.ApdL("       T_STOCK");
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFld(dr, Def_T_STOCK.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFld(dr, Def_T_STOCK.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", ComFunc.GetFld(dr, Def_T_STOCK.TAG_NO)));

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

    #endregion

    #endregion

    #endregion

    #endregion

    #region I0100060:オペ履歴照会

    #region オペ履歴取得取得

    /// --------------------------------------------------
    /// <summary>
    /// オペ履歴取得取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I01用コンディション</param>
    /// <returns>オペ履歴取得データ</returns>
    /// <create>T.Wakamatsu 2013/08/20</create>
    /// <update>H.Tajimi 2015/12/03 物件名をDBから取得するよう変更</update>
    /// <update>H.Tajimi 2015/11/20 備考対応</update>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetJisseki(DatabaseHelper dbHelper, CondI01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            string fieldName = string.Empty;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       JSK.JISSEKI_DATE");
            sb.ApdL("     , JSK.SAGYO_FLAG");
            sb.ApdL("     , JSK.TAG_CODE");
            sb.ApdL("     , JSK.STATUS");
            sb.ApdL("     , JSK.WORK_USER_NAME");
            sb.ApdL("     , COM2.ITEM_NAME AS SAGYO_FLAG_NAME");
            sb.ApdL("     , COM1.ITEM_NAME AS STATUS_NAME");
            sb.ApdL("     , JSK.LOCATION");
            sb.ApdL("     , JSK.STOCK_DATE");
            sb.ApdL("     , JSK.SHUKKA_FLAG");
            sb.ApdL("     , JSK.NONYUSAKI_CD");
            sb.ApdL("     , JSK.TAG_NO");
            sb.ApdL("     , JSK.SEIBAN");
            sb.ApdL("     , JSK.CODE");
            sb.ApdL("     , JSK.ZUMEN_OIBAN");
            sb.ApdL("     , JSK.AREA");
            sb.ApdL("     , JSK.FLOOR");
            sb.ApdL("     , JSK.KISHU");
            sb.ApdL("     , JSK.ST_NO");
            sb.ApdL("     , JSK.HINMEI_JP");
            sb.ApdL("     , JSK.HINMEI");
            sb.ApdL("     , JSK.ZUMEN_KEISHIKI");
            sb.ApdL("     , JSK.KUWARI_NO");
            sb.ApdL("     , JSK.NUM");
            sb.ApdL("     , MN.NONYUSAKI_NAME");
            sb.ApdL("     , MN.SHIP");
            sb.ApdL("     , JSK.AR_NO");
            sb.ApdL("     , JSK.SHUKA_DATE");
            sb.ApdL("     , JSK.BOX_NO");
            sb.ApdL("     , JSK.PALLET_NO");
            sb.ApdL("     , JSK.KOJI_NO");
            sb.ApdL("     , JSK.CASE_ID");
            sb.ApdL("     , KW.SHIP + '-' + CONVERT(nvarchar, KM.CASE_NO) AS KIWAKU_NO");
            sb.ApdL("     , JSK.BOXKONPO_DATE");
            sb.ApdL("     , JSK.PALLETKONPO_DATE");
            sb.ApdL("     , JSK.KIWAKUKONPO_DATE");
            sb.ApdL("     , JSK.SHUKKA_DATE");
            sb.ApdL("     , JSK.UNSOKAISHA_NAME");
            sb.ApdL("     , JSK.INVOICE_NO");
            sb.ApdL("     , JSK.OKURIJYO_NO");
            sb.ApdL("     , JSK.BL_NO");
            sb.ApdL("     , JSK.UKEIRE_DATE");
            sb.ApdL("     , JSK.UKEIRE_USER_NAME");
            sb.ApdL("     , BK.BUKKEN_NAME");
            sb.ApdL("     , JSK.BIKO");
            sb.ApdL("     , JSK.M_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_JISSEKI JSK");
            sb.ApdL("  INNER JOIN M_NONYUSAKI MN ON MN.SHUKKA_FLAG = JSK.SHUKKA_FLAG");
            sb.ApdL("                         AND MN.NONYUSAKI_CD = JSK.NONYUSAKI_CD");
            sb.ApdL("  LEFT JOIN T_KIWAKU KW ON KW.KOJI_NO = JSK.KOJI_NO");
            sb.ApdL("  LEFT JOIN T_KIWAKU_MEISAI KM ON KM.CASE_ID = JSK.CASE_ID");
            sb.ApdL("  LEFT JOIN M_COMMON COM1 ON COM1.GROUP_CD = 'ZAIKO_STATUS'");
            sb.ApdL("                         AND COM1.VALUE1 = JSK.STATUS");
            sb.ApdN("                         AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_COMMON COM2 ON COM2.GROUP_CD = 'ZAIKO_SAGYO_FLAG'");
            sb.ApdL("                         AND COM2.VALUE1 = JSK.SAGYO_FLAG");
            sb.ApdN("                         AND COM2.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_BUKKEN BK ON BK.SHUKKA_FLAG = MN.SHUKKA_FLAG");
            sb.ApdL("                       AND BK.BUKKEN_NO = MN.BUKKEN_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       JSK.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND MN.BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            if (cond.SagyoFlag != ZAIKO_SAGYO_FLAG.ALL_VALUE1)
            {
                sb.ApdN("   AND JSK.SAGYO_FLAG = ").ApdN(this.BindPrefix).ApdL("SAGYO_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("SAGYO_FLAG", cond.SagyoFlag));
            }

            if (!String.IsNullOrEmpty(cond.JissekiDateFrom))
            {
                sb.ApdN("   AND JSK.JISSEKI_DATE >= ").ApdN(this.BindPrefix).ApdL("JISSEKI_DATE_FROM");
                paramCollection.Add(iNewParam.NewDbParameter("JISSEKI_DATE_FROM", cond.JissekiDateFrom));
            }
            if (!String.IsNullOrEmpty(cond.JissekiDateTo))
            {
                sb.ApdN("   AND JSK.JISSEKI_DATE < ").ApdN(this.BindPrefix).ApdL("JISSEKI_DATE_TO");
                paramCollection.Add(iNewParam.NewDbParameter("JISSEKI_DATE_TO", cond.JissekiDateTo));
            }

            // AR No.
            if (cond.ARNo != null)
            {
                fieldName = "AR_NO";
                sb.ApdN("   AND ").ApdN("JSK." + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ARNo));
            }
            // 品名(和文)
            if (!string.IsNullOrEmpty(cond.HinmeiJp))
            {
                fieldName = "HINMEI_JP";
                sb.ApdN("   AND ").ApdN("JSK." + fieldName).ApdN(" LIKE ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, "%" + cond.HinmeiJp + "%"));
            }
            // 品名
            if (!string.IsNullOrEmpty(cond.Hinmei))
            {
                fieldName = "HINMEI";
                sb.ApdN("   AND ").ApdN("JSK." + fieldName).ApdN(" LIKE ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, "%" + cond.Hinmei + "%"));
            }
            // 図番/形式
            if (!string.IsNullOrEmpty(cond.ZumenKeishiki))
            {
                fieldName = "ZUMEN_KEISHIKI";
                sb.ApdN("   AND ").ApdN("JSK." + fieldName).ApdN(" LIKE ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, "%" + cond.ZumenKeishiki + "%"));
            }
            // エリア
            if (!string.IsNullOrEmpty(cond.Area))
            {
                fieldName = "AREA";
                sb.ApdN("   AND ").ApdN("JSK." + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.Area));
            }
            // フロア
            if (!string.IsNullOrEmpty(cond.Floor))
            {
                fieldName = "FLOOR";
                sb.ApdN("   AND ").ApdN("JSK." + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.Floor));
            }
            // 区割No.
            if (!string.IsNullOrEmpty(cond.KuwariNo))
            {
                fieldName = "KUWARI_NO";
                sb.ApdN("   AND ").ApdN("JSK." + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.KuwariNo));
            }
            // 木枠No/Case No/Pallet No/Box No/Tag No
            if (!string.IsNullOrEmpty(cond.KiwakuNo)
               && !string.IsNullOrEmpty(cond.CaseNo))
            {
                fieldName = "SHIP";
                sb.ApdN("   AND ").ApdN("KW." + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.KiwakuNo));
                fieldName = "CASE_NO";
                sb.ApdN("   AND ").ApdN("KM." + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.CaseNo));
            }
            else if (!string.IsNullOrEmpty(cond.KiwakuNo))
            {
                fieldName = "SHIP";
                sb.ApdN("   AND ").ApdN("KW." + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.KiwakuNo));
            }
            else if (!string.IsNullOrEmpty(cond.CaseNo))
            {
                fieldName = "CASE_NO";
                sb.ApdN("   AND ").ApdN("KM." + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.CaseNo));
            }
            else if (!string.IsNullOrEmpty(cond.PalletNo))
            {
                fieldName = "PALLET_NO";
                sb.ApdN("   AND ").ApdN("JSK." + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.PalletNo));
            }
            else if (!string.IsNullOrEmpty(cond.BoxNo))
            {
                fieldName = "BOX_NO";
                sb.ApdN("   AND ").ApdN("JSK." + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.BoxNo));
            }
            else if (!string.IsNullOrEmpty(cond.TagNo))
            {
                fieldName = "TAG_NO";
                sb.ApdN("   AND ").ApdN("JSK." + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.TagNo));
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("       JSK.JISSEKI_DATE DESC");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_JISSEKI.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

}
