using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Web;

using Condition;
using Commons;
using DSWUtil;
using DSWUtil.DbUtil;
using System.Reflection;

//// --------------------------------------------------
/// <summary>
/// 常駐処理（データアクセス層） 
/// </summary>
/// <create>Y.Higuchi 2010/06/23</create>
/// <update></update>
/// --------------------------------------------------
public class WsJ01Impl : WsBaseImpl
{
    #region 定数
    private const string BIND_BY_NAME_PROPERTY_NAME = "BindByName";
    #endregion

    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/06/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsJ01Impl()
        : base()
    {
    }

    #endregion

    #region 制御

    #region J0100020:日次処理

    #region 締め開始処理

    /// --------------------------------------------------
    /// <summary>
    /// 締め開始処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="ds">締め処理マスタ</param>
    /// <param name="isProcessed">処理中かどうか</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool StartShime(DatabaseHelper dbHelper, ref DataSet ds, ref bool isProcessed)
    {
        try
        {
            ds = this.LockShime(dbHelper);
            // 存在チェック
            if (!ComFunc.IsExistsData(ds, Def_M_SHIME.Name))
            {
                return false;
            }

            // 処理中チェック
            if (ComFunc.GetFld(ds, Def_M_SHIME.Name, 0, Def_M_SHIME.SHORI_FLAG) != SHORI_FLAG.MISHORI_VALUE1)
            {
                isProcessed = true;
                return false;
            }

            // 処理日チェック
            DateTime nichiji = ComFunc.GetFldToDateTime(ds, Def_M_SHIME.Name, 0, Def_M_SHIME.NICHIJI_DATE);
            DateTime getsuji = ComFunc.GetFldToDateTime(ds, Def_M_SHIME.Name, 0, Def_M_SHIME.GETSUJI_DATE);
            if (DateTime.Today <= nichiji.Date)
            {
                isProcessed = true;
                return false;
            }

            // 処理フラグ更新
            CondJ01 cond = new CondJ01();
            cond.ShoriFlag = SHORI_FLAG.SHORITYU_VALUE1;
            this.UpdShime(dbHelper, cond);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region ARデータの削除処理

    /// --------------------------------------------------
    /// <summary>
    /// ARデータの削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <param name="ds">削除対象データ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update>D.Okumura 2019/06/21 AR添付ファイル対応</update>
    /// <update>D.Okumura 2019/08/07 AR進捗対応</update>
    /// --------------------------------------------------
    public bool DelARData(DatabaseHelper dbHelper, CondJ01 cond, ref DataSet ds)
    {
        try
        {
            ds = this.GetARDeleteTarget(dbHelper, cond);
            this.DelARForDate(dbHelper, cond);
            this.DelARForNonyusaki(dbHelper, cond);
            this.DelARFile(dbHelper, cond);
            this.DelARGoki(dbHelper, cond);
            this.DelARShinchoku(dbHelper, cond);
            this.DelARShinchokuRireki(dbHelper, cond);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 木枠データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 木枠データ削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <param name="ds">削除対象データ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool DelKiwakuData(DatabaseHelper dbHelper, CondJ01 cond, ref DataSet ds)
    {
        try
        {
            ds = this.GetKiwakuDeleteTarget(dbHelper, cond);
            this.DelKiwaku(dbHelper, cond);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 一時作業データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 一時作業データ削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/09/01</create>
    /// <update>K.Tsutsumi 2020/06/26 7日間保持する</update>
    /// --------------------------------------------------
    public bool DelTempworkData(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            this.DelTempwork(dbHelper, cond);
            this.DelTempworkMeisai(dbHelper);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 一時作業データ（現地部品管理）削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 一時作業データ（現地部品管理）削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/09/01</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool DelBuhinTempworkData(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            this.DelBuhinTempwork(dbHelper);
            this.DelBuhinTempworkMeisai(dbHelper);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region DBバックアップ処理

    /// --------------------------------------------------
    /// <summary>
    /// DBバックアップ処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update>K.Tsutsumi 2020/06/26 ファイルの削除はバッチファイルで行う</update>
    /// <update>K.Tsutsumi 2020/06/26 トランザクションログを切り捨てる</update>
    /// --------------------------------------------------
    public bool DBBackup(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            bool ret = false;

            // フォルダの作成
            if (!Directory.Exists(cond.ExpBackupPath))
            {
                Directory.CreateDirectory(cond.ExpBackupPath);
            }

            // DBバックアップ
            ret = this.DBBackupExec(dbHelper, cond);

            if (ret)
            {
                // トランザクションログの切り捨て
                ret = TranBackupExec(dbHelper, cond);
            }

            if (ret)
            { 
                // トランザクションログの圧縮（復旧モデルをSimpleへ）
                ret = SetRecoverySimpleExec(dbHelper, cond);
            }

            if (ret)
            {
                // トランザクションログの圧縮（DBCC SHRINKFILE）
                ret = DbccShrinkFileExec(dbHelper, cond);
            }

            if (ret)
            {
                // トランザクションログの圧縮（復旧モデルをFullへ）
                ret = SetRecoveryFullExec(dbHelper, cond);
            }

            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region メールデータ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// メールデータ削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool DelMail(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            this.DelMail(dbHelper);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region J0100040:SKS連携処理

    #region SKS連携開始処理

    /// --------------------------------------------------
    /// <summary>
    /// SKS連携開始処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="ds">SKS連携マスタ</param>
    /// <param name="isProcessed">処理中かどうか</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tajimi 2018/11/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool StartSKS(DatabaseHelper dbHelper, ref DataSet ds, ref bool isProcessed)
    {
        try
        {
            ds = this.LockSKS(dbHelper);
            // 存在チェック
            if (!ComFunc.IsExistsData(ds, Def_M_SKS.Name))
            {
                return false;
            }

            // 処理中チェック
            if (ComFunc.GetFld(ds, Def_M_SKS.Name, 0, Def_M_SKS.SHORI_FLAG) != SHORI_FLAG.MISHORI_VALUE1)
            {
                isProcessed = true;
                return false;
            }

            // 処理日チェック
            DateTime sksRenkei = ComFunc.GetFldToDateTime(ds, Def_M_SKS.Name, 0, Def_M_SKS.LASTEST_DATE);
            if (DateTime.Today <= sksRenkei.Date)
            {
                isProcessed = true;
                return false;
            }

            // 処理フラグ更新
            CondJ01 cond = new CondJ01();
            cond.ShoriFlag = SHORI_FLAG.SHORITYU_VALUE1;
            this.UpdSKS(dbHelper, cond);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region SKS手配明細ワーク追加

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細ワーク追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <param name="dt">登録データ</param>
    /// <param name="ds">エラーメッセージ返却用DataSet</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/22</create>
    /// <update>K.Tsutsumi 2020/05/15 技連No.10桁対応</update>
    /// --------------------------------------------------
    public bool InsTehaiSKSWork(DatabaseHelper dbHelper, CondJ01 cond, DataTable dt, ref DataSet ds)
    {
        try
        {
            ds = new DataSet();
            var dtMessage = this.GetErrorMessageScheme();
            for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
            {
                var dr = dt.Rows[rowIndex];
                var tehaiNo = ComFunc.GetFld(dr, Def_T_TEHAI_SKS_WORK.TEHAI_NO);
                // 必須入力チェック
                if (string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_TEHAI_SKS_WORK.SEIBAN_CODE)))
                {
                    this.AddErrorMessage(dtMessage, "[{0}], {1}行目の製番が入力されていません。", new string[] { tehaiNo, (rowIndex + 1).ToString() });
                    continue;
                }
                if (string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_TEHAI_SKS_WORK.HINBAN)))
                {
                    this.AddErrorMessage(dtMessage, "[{0}], {1}行目の品番が入力されていません。", new string[] { tehaiNo, (rowIndex + 1).ToString() });
                    continue;
                }
                var ecsNo = ComFunc.GetFld(dr, Def_T_TEHAI_SKS_WORK.ECS_NO);
                if (string.IsNullOrEmpty(ecsNo))
                {
                    //技連未入力のデータがあまりにも多いため、出力を断念
                    //this.AddErrorMessage(dtMessage, "{0}行目の技連Noが入力されていません。", new string[] { (rowIndex + 1).ToString() });
                    continue;
                }

                // 指定技連マスタの完了状態が未完了であるか確認
                // 技連が登録されていない場合は、無関係の発注データだが本システムへ登録していないだけかもしれないので取り込む                   
                var dtEcs = this.GetEcs(dbHelper, ecsNo);
                if (UtilData.ExistsData(dtEcs))
                {
                    if (ComFunc.GetFld(dtEcs, 0, Def_M_ECS.KANRI_FLAG) == KANRI_FLAG.KANRYO_VALUE1)
                    {
                        this.AddErrorMessage(dtMessage, "[{0}], {1}行目の技連No({2})は既に完了してます。", new string[] { tehaiNo, (rowIndex + 1).ToString(), ecsNo });
                        continue;
                    }
                }

                try
                {
                    // SKS手配明細WORK登録
                    if (this.InsTehaiSKSWorkExec(dbHelper, cond, dr) != 1)
                    {
                        this.AddErrorMessage(dtMessage, "[{0}], {1}行目のSKS手配明細WORK登録に失敗しました。", new string[] { tehaiNo, (rowIndex + 1).ToString() });
                        continue;
                    }
                }
                catch(Exception ex)
                {
                    this.AddErrorMessage(dtMessage, "[{0}], {1}行目のSKS手配明細WORK登録に失敗しました。({2})", new string[] { tehaiNo, (rowIndex + 1).ToString(), ex.Message });
                    continue;
                }
            }

            ds.Merge(dtMessage);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #endregion

    #region SQL実行

    #region J0100020:日次処理

    #region SELECT

    #region 締めマスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// 締めマスタ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns>締めマスタ</returns>
    /// <create>Y.Higuchi 2010/08/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetShime(DatabaseHelper dbHelper)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       SHORI_FLAG");
            sb.ApdL("     , NICHIJI_DATE");
            sb.ApdL("     , GETSUJI_DATE");
            sb.ApdL("     , FILE_BACKUP_PATH");
            sb.ApdL("     , EXP_BACKUP_PATH");
            sb.ApdL("     , HOJIKIKAN");
            sb.ApdL("     , START_TIME");
            sb.ApdL("  FROM ");
            sb.ApdL("       M_SHIME");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_SHIME.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 締めマスタ取得(行レベルロック)

    /// --------------------------------------------------
    /// <summary>
    /// 締めマスタ取得(行レベルロック)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns>締めマスタ</returns>
    /// <create>Y.Higuchi 2010/08/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet LockShime(DatabaseHelper dbHelper)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       SHORI_FLAG");
            sb.ApdL("     , NICHIJI_DATE");
            sb.ApdL("     , GETSUJI_DATE");
            sb.ApdL("     , FILE_BACKUP_PATH");
            sb.ApdL("     , EXP_BACKUP_PATH");
            sb.ApdL("     , HOJIKIKAN");
            sb.ApdL("     , START_TIME");
            sb.ApdL("  FROM ");
            sb.ApdL("       M_SHIME");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_SHIME.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region ARデータの削除対象取得

    /// --------------------------------------------------
    /// <summary>
    /// ARデータの削除対象取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>ARデータ</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update>T.Nukaga 2019/11/26 STEP12 AR7000番運用対応</update>
    /// --------------------------------------------------
    public DataSet GetARDeleteTarget(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       NONYUSAKI_CD");
            sb.ApdL("     , LIST_FLAG");
            sb.ApdL("     , AR_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_AR");
            sb.ApdL(" WHERE");
            sb.ApdN("       JYOKYO_FLAG = ").ApdN(this.BindPrefix).ApdL("JYOKYO_FLAG");
            sb.ApdN("   AND SHUKKA_DATE < ").ApdN(this.BindPrefix).ApdL("SHUKKA_DATE");
            sb.ApdL("   AND NOT EXISTS (");
            sb.ApdL("               SELECT 1 FROM T_AR ta1");
            sb.ApdL("               WHERE ta1.MOTO_AR_NO = T_AR.AR_NO");
            sb.ApdL("               AND ta1.NONYUSAKI_CD = T_AR.NONYUSAKI_CD)");
            sb.ApdL("UNION");
            sb.ApdL("SELECT");
            sb.ApdL("       NONYUSAKI_CD");
            sb.ApdL("     , LIST_FLAG");
            sb.ApdL("     , AR_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_AR");
            sb.ApdL(" WHERE");
            sb.ApdL("       NOT EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM M_NONYUSAKI MN");
            sb.ApdL("                WHERE");
            sb.ApdN("                      MN.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdL("                  AND MN.NONYUSAKI_CD = T_AR.NONYUSAKI_CD");
            sb.ApdL("                  )");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("JYOKYO_FLAG", JYOKYO_FLAG.KANRYO_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_DATE", cond.TargetDate));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));

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

    #region 木枠データ削除対象取得

    /// --------------------------------------------------
    /// <summary>
    /// 木枠データ削除対象取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>木枠データ</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetKiwakuDeleteTarget(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       KOJI_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_KIWAKU");
            sb.ApdL(" WHERE");
            sb.ApdL("       NOT EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("                WHERE");
            sb.ApdL("                      TSM.KOJI_NO = T_KIWAKU.KOJI_NO");
            sb.ApdL("                  )");
            sb.ApdN("   AND SAGYO_FLAG = ").ApdN(this.BindPrefix).ApdL("SAGYO_FLAG");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SAGYO_FLAG", SAGYO_FLAG.SHUKKAZUMI_VALUE1));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_KIWAKU.Name);

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
    #endregion

    #region UPDATE

    #region 締めマスタ更新処理

    /// --------------------------------------------------
    /// <summary>
    /// 締めマスタ更新処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdShime(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE M_SHIME");
            sb.ApdL("SET");
            sb.ApdN("       SHORI_FLAG = ").ApdN(this.BindPrefix).ApdL("SHORI_FLAG");
            if (cond.NichijiDate != null)
            {
                sb.ApdN("     , NICHIJI_DATE = ").ApdN(this.BindPrefix).ApdL("NICHIJI_DATE");
                paramCollection.Add(iNewParam.NewDbParameter("NICHIJI_DATE", cond.NichijiDate));
            }
            if (cond.GetsujiDate != null)
            {
                sb.ApdN("     , GETSUJI_DATE = ").ApdN(this.BindPrefix).ApdL("GETSUJI_DATE");
                paramCollection.Add(iNewParam.NewDbParameter("GETSUJI_DATE", cond.GetsujiDate));
            }

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHORI_FLAG", cond.ShoriFlag));

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

    #region 本体の納入先マスタ完了処理

    /// --------------------------------------------------
    /// <summary>
    /// 本体の納入先マスタ完了処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update>T.Nukaga 2019/11/26 STEP12 本体の納入先マスタ完了処理条件変更対応</update>
    /// <update>J.Chen 2024/02/21 条件変更（出荷済から半年後）</update>
    /// --------------------------------------------------
    public int UpdHontaiNonyusakiKanryo(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            const int ELAPSED_TIME = -365;
            DateTime date = DateTime.Now.AddDays(ELAPSED_TIME);
            DateTime halfYearAgo = DateTime.Now.AddMonths(-6);

            // SQL文
            sb.ApdL("UPDATE M_NONYUSAKI");
            sb.ApdL("SET");
            sb.ApdN("       KANRI_FLAG = ").ApdN(this.BindPrefix).ApdL("KANRI_FLAG");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdL("       NOT EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("                WHERE");
            sb.ApdN("                      TSM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdL("                  AND (");
            sb.ApdN("                         (TSM.JYOTAI_FLAG NOT IN (");
            sb.ApdN(this.BindPrefix).ApdN("SHUKKA").ApdN(" ,").ApdN(this.BindPrefix).ApdN("UKEIRE").ApdL("))");
            sb.ApdL("                      OR ");
            sb.ApdN("                         (TSM.JYOTAI_FLAG IN (");
            sb.ApdN(this.BindPrefix).ApdN("SHUKKA").ApdN(" ,").ApdN(this.BindPrefix).ApdN("UKEIRE").ApdN(") AND TSM.SHUKKA_DATE > ").ApdN(this.BindPrefix).ApdN("SHUKKA_DATE").ApdL(")");
            sb.ApdL("                      )");
            sb.ApdL("                  AND TSM.SHUKKA_FLAG = M_NONYUSAKI.SHUKKA_FLAG");
            sb.ApdL("                  AND TSM.NONYUSAKI_CD = M_NONYUSAKI.NONYUSAKI_CD");
            sb.ApdL("                  )");
            sb.ApdN("   AND SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND KANRI_FLAG = ").ApdN(this.BindPrefix).ApdL("KANRI_FLAG_MIKAN");
            sb.ApdN("   AND CREATE_DATE < ").ApdN(this.BindPrefix).ApdL("CREATE_DATE");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("KANRI_FLAG", KANRI_FLAG.KANRYO_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", ComDefine.SHIME_UPDATE_USER_ID));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", ComDefine.SHIME_UPDATE_USER_NAME));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.NORMAL_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA", JYOTAI_FLAG.SHUKKAZUMI_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("UKEIRE", JYOTAI_FLAG.UKEIREZUMI_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("KANRI_FLAG_MIKAN", KANRI_FLAG.MIKAN_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_DATE", date));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_DATE", halfYearAgo));

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

    #region DELETE

    #region 本体の納入先マスタ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 本体の納入先マスタ削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelHontaiNonyusaki(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE M_NONYUSAKI");
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND KANRI_FLAG = ").ApdN(this.BindPrefix).ApdL("KANRI_FLAG");
            // @@@ 2011/02/16 M.Tsutsumi Change Step2 No.37
            //sb.ApdN("   AND UPDATE_DATE < ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdL("   AND (");
            sb.ApdN("       (");
            sb.ApdN("       UPDATE_DATE < ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("   AND MAINTE_DATE < ").ApdN(this.BindPrefix).ApdL("MAINTE_DATE");
            sb.ApdN("       )");
            sb.ApdN("    OR REMOVE_FLAG = ").ApdN(this.BindPrefix).ApdL("REMOVE_FLAG");
            sb.ApdL("       )");
            // @@@ ↑

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.NORMAL_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("KANRI_FLAG", KANRI_FLAG.KANRYO_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.TargetDate));
            paramCollection.Add(iNewParam.NewDbParameter("MAINTE_DATE", cond.TargetDate));
            //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.37
            paramCollection.Add(iNewParam.NewDbParameter("REMOVE_FLAG", REMOVE_FLAG.JYOGAI_VALUE1));
            // @@@ ↑

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

    #region 本体の納入先マスタ削除処理(指定年で無条件)

    /// --------------------------------------------------
    /// <summary>
    /// 本体の納入先マスタ削除処理(指定年で無条件)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Sakiori 2015/06/11</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelHontaiNonyusakiUncondition(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE MNN");
            sb.ApdL("  FROM M_NONYUSAKI AS MNN");
            sb.ApdL("  LEFT JOIN M_SYSTEM_PARAMETER AS MSP");
            sb.ApdL("    ON 1 = 1");
            sb.ApdL(" WHERE MNN.CREATE_DATE < DATEADD(YEAR, - 1 * MSP.HONTAI_DELETE_YEAR, CONVERT(DATE, GETDATE()))");
            sb.ApdL("   AND SHUKKA_FLAG = {0}SHUKKA_FLAG");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.NORMAL_VALUE1));

            // SQL実行
            record += dbHelper.ExecuteNonQuery(string.Format(sb.ToString(), this.BindPrefix), paramCollection);

            return record;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 本体の物件名マスタ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 本体の物件名マスタ削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/04/19</create>
    /// <update>K.Tsutsumi 2019/09/07 旧物件名で登録したもののみ対象とする</update>
    /// --------------------------------------------------
    public int DelHontaiBukken(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("DELETE");
            sb.ApdL("  FROM M_BUKKEN");
            sb.ApdN(" WHERE SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND UPDATE_DATE < ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdL("   AND NOT EXISTS (");
            sb.ApdL("       SELECT 1");
            sb.ApdL("         FROM M_NONYUSAKI MN");
            sb.ApdN("        WHERE MN.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdL("          AND MN.SHUKKA_FLAG = M_BUKKEN.SHUKKA_FLAG");
            sb.ApdL("          AND MN.BUKKEN_NO = M_BUKKEN.BUKKEN_NO");
            sb.ApdL("   )");
            sb.ApdL("   AND PROJECT_NO IS NULL");

            paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.NORMAL_VALUE1));
            paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_DATE", cond.TargetDate));

            return dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 本体の出荷明細データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 本体の出荷明細データ削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelHontaiShukkaMeisai(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE T_SHUKKA_MEISAI");
            sb.ApdL(" WHERE");
            sb.ApdL("       NOT EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM M_NONYUSAKI MN");
            sb.ApdL("                WHERE");
            sb.ApdN("                      MN.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdL("                  AND MN.SHUKKA_FLAG = T_SHUKKA_MEISAI.SHUKKA_FLAG");
            sb.ApdL("                  AND MN.NONYUSAKI_CD = T_SHUKKA_MEISAI.NONYUSAKI_CD");
            sb.ApdL("                  )");
            sb.ApdN("   AND SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.NORMAL_VALUE1));

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

    #region ARの納入先マスタ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 本体の納入先マスタ削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelARNonyusaki(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE M_NONYUSAKI");
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND KANRI_FLAG = ").ApdN(this.BindPrefix).ApdL("KANRI_FLAG");
            // @@@ 2011/02/16 M.Tsutsumi Change Step2 No.37
            //sb.ApdN("   AND MAINTE_DATE < ").ApdN(this.BindPrefix).ApdL("MAINTE_DATE");
            sb.ApdL("   AND (");
            sb.ApdN("       MAINTE_DATE < ").ApdN(this.BindPrefix).ApdL("MAINTE_DATE");
            sb.ApdN("    OR REMOVE_FLAG = ").ApdN(this.BindPrefix).ApdL("REMOVE_FLAG");
            sb.ApdL("       )");
            // @@@ ↑

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("KANRI_FLAG", KANRI_FLAG.KANRYO_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("MAINTE_DATE", cond.TargetDate));
            //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.37
            paramCollection.Add(iNewParam.NewDbParameter("REMOVE_FLAG", REMOVE_FLAG.JYOGAI_VALUE1));
            // @@@ ↑

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

    #region ARの納入先マスタ削除処理(指定年で無条件)

    /// --------------------------------------------------
    /// <summary>
    /// ARの納入先マスタ削除処理(指定年で無条件)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Sakiori 2015/06/11</create>
    /// <update>K.Tsutsumi 2020/06/26 リージョンとコメントの間違いを修正</update>
    /// --------------------------------------------------
    public int DelARNonyusakiUncondition(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE MNN");
            sb.ApdL("  FROM M_NONYUSAKI AS MNN");
            sb.ApdL("  LEFT JOIN M_SYSTEM_PARAMETER AS MSP");
            sb.ApdL("    ON 1 = 1");
            sb.ApdL(" WHERE MNN.CREATE_DATE < DATEADD(YEAR, - 1 * MSP.AR_DELETE_YEAR, CONVERT(DATE, GETDATE()))");
            sb.ApdN("   AND MNN.SHUKKA_FLAG = {0}SHUKKA_FLAG");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));

            // SQL実行
            record += dbHelper.ExecuteNonQuery(string.Format(sb.ToString(), this.BindPrefix), paramCollection);

            return record;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region ARの物件名マスタ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// ARの物件名マスタ削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/04/19</create>
    /// <update>K.Tsutsumi 2019/09/07 旧物件名で登録したもののみ対象とする</update>
    /// --------------------------------------------------
    public int DelARBukken(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("DELETE");
            sb.ApdL("  FROM M_BUKKEN");
            sb.ApdN(" WHERE SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND UPDATE_DATE < ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdL("   AND NOT EXISTS (");
            sb.ApdL("       SELECT 1");
            sb.ApdL("         FROM M_NONYUSAKI MN");
            sb.ApdN("        WHERE MN.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdL("          AND MN.SHUKKA_FLAG = M_BUKKEN.SHUKKA_FLAG");
            sb.ApdL("          AND MN.BUKKEN_NO = M_BUKKEN.BUKKEN_NO");
            sb.ApdL("   )");
            sb.ApdL("   AND PROJECT_NO IS NULL");

            paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));
            paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_DATE", cond.TargetDate));

            return dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region ARデータの削除処理

    /// --------------------------------------------------
    /// <summary>
    /// ARデータの削除処理(期間)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelARForDate(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE T_AR");
            sb.ApdL(" WHERE");
            sb.ApdN("       JYOKYO_FLAG = ").ApdN(this.BindPrefix).ApdL("JYOKYO_FLAG");
            sb.ApdN("   AND SHUKKA_DATE < ").ApdN(this.BindPrefix).ApdL("SHUKKA_DATE");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("JYOKYO_FLAG", JYOKYO_FLAG.KANRYO_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_DATE", cond.TargetDate));

            // SQL実行
            record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            return record;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// ARデータの削除処理(納入先マスタの有無)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelARForNonyusaki(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE T_AR");
            sb.ApdL(" WHERE");
            sb.ApdL("       NOT EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM M_NONYUSAKI MN");
            sb.ApdL("                WHERE");
            sb.ApdN("                      MN.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdL("                  AND MN.NONYUSAKI_CD = T_AR.NONYUSAKI_CD");
            sb.ApdL("                  )");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));

            // SQL実行
            record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            return record;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// AR添付ファイルデータの削除処理
    /// 紐づけがなくなったレコードを削除する
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>D.Okumura 2019/06/21</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelARFile(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE T_AR_FILE");
            sb.ApdL(" WHERE");
            sb.ApdL("       NOT EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_AR");
            sb.ApdL("                WHERE");
            sb.ApdN("                      T_AR.NONYUSAKI_CD = T_AR_FILE.NONYUSAKI_CD");
            sb.ApdL("                  AND T_AR.AR_NO = T_AR_FILE.AR_NO");
            sb.ApdL("                  AND T_AR.LIST_FLAG = T_AR_FILE.LIST_FLAG");
            sb.ApdL("                  )");

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
    
    /// --------------------------------------------------
    /// <summary>
    /// AR進捗データの削除処理
    /// 紐づけがなくなったレコードを削除する
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>D.Okumura 2019/08/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelARShinchoku(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE T_AR_SHINCHOKU");
            sb.ApdL(" WHERE");
            sb.ApdL("       NOT EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_AR");
            sb.ApdL("                WHERE");
            sb.ApdN("                      T_AR.NONYUSAKI_CD = T_AR_SHINCHOKU.NONYUSAKI_CD");
            sb.ApdL("                  AND T_AR.AR_NO = T_AR_SHINCHOKU.AR_NO");
            sb.ApdL("                  AND T_AR.LIST_FLAG = T_AR_SHINCHOKU.LIST_FLAG");
            sb.ApdL("                  )");

            // SQL実行
            record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            return record;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    
    /// --------------------------------------------------
    /// <summary>
    /// AR進捗データの削除処理
    /// 紐づけがなくなったレコードを削除する
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>D.Okumura 2019/08/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelARShinchokuRireki(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE T_AR_SHINCHOKU_RIREKI");
            sb.ApdL(" WHERE");
            sb.ApdL("       NOT EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_AR");
            sb.ApdL("                WHERE");
            sb.ApdN("                      T_AR.NONYUSAKI_CD = T_AR_SHINCHOKU_RIREKI.NONYUSAKI_CD");
            sb.ApdL("                  AND T_AR.AR_NO = T_AR_SHINCHOKU_RIREKI.AR_NO");
            sb.ApdL("                  AND T_AR.LIST_FLAG = T_AR_SHINCHOKU_RIREKI.LIST_FLAG");
            sb.ApdL("                  )");

            // SQL実行
            record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            return record;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    
    /// --------------------------------------------------
    /// <summary>
    /// AR号機データの削除処理
    /// 紐づけがなくなったレコードを削除する
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>D.Okumura 2019/08/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelARGoki(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE T_AR_GOKI");
            sb.ApdL(" WHERE");
            sb.ApdL("       NOT EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM M_NONYUSAKI MN");
            sb.ApdL("                WHERE");
            sb.ApdN("                      MN.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdL("                  AND MN.NONYUSAKI_CD = T_AR_GOKI.NONYUSAKI_CD");
            sb.ApdL("                  )");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));

            // SQL実行
            record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            return record;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #region ARの出荷明細データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// ARの出荷明細データ削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelARShukkaMeisai(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE T_SHUKKA_MEISAI");
            sb.ApdL(" WHERE");
            sb.ApdL("       NOT EXISTS (");
            sb.ApdL("               SELECT 1");
            // @@@ 2011/03/07 M.Tsutsumi Change No.41
            //sb.ApdL("                 FROM T_AR");
            //sb.ApdL("                WHERE");
            //sb.ApdL("                      T_AR.NONYUSAKI_CD = T_SHUKKA_MEISAI.NONYUSAKI_CD");
            //sb.ApdL("                  AND T_AR.AR_NO = T_SHUKKA_MEISAI.AR_NO");
            sb.ApdL("                 FROM M_NONYUSAKI MN");
            sb.ApdL("                WHERE");
            sb.ApdN("                      MN.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdL("                  AND MN.NONYUSAKI_CD = T_SHUKKA_MEISAI.NONYUSAKI_CD");
            // @@@ ↑
            sb.ApdL("                  )");
            sb.ApdN("   AND SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));

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

    #region BOXリスト管理データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// BOXリスト管理データ削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelBoxlistManage(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE T_BOXLIST_MANAGE");
            sb.ApdL(" WHERE");
            sb.ApdL("       NOT EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("                WHERE");
            sb.ApdL("                      TSM.SHUKKA_FLAG = T_BOXLIST_MANAGE.SHUKKA_FLAG");
            sb.ApdL("                  AND TSM.NONYUSAKI_CD = T_BOXLIST_MANAGE.NONYUSAKI_CD");
            sb.ApdL("                  AND TSM.BOX_NO = T_BOXLIST_MANAGE.BOX_NO");
            sb.ApdL("                  )");

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

    #region パレットリスト管理データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// パレットリスト管理データ削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelPalletlistManage(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE T_PALLETLIST_MANAGE");
            sb.ApdL(" WHERE");
            sb.ApdL("       NOT EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("                WHERE");
            sb.ApdL("                      TSM.SHUKKA_FLAG = T_PALLETLIST_MANAGE.SHUKKA_FLAG");
            sb.ApdL("                  AND TSM.NONYUSAKI_CD = T_PALLETLIST_MANAGE.NONYUSAKI_CD");
            sb.ApdL("                  AND TSM.PALLET_NO = T_PALLETLIST_MANAGE.PALLET_NO");
            sb.ApdL("                  )");

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

    #region 木枠データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 木枠データ削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelKiwaku(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE T_KIWAKU");
            sb.ApdL(" WHERE");
            sb.ApdL("       NOT EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("                WHERE");
            sb.ApdL("                      TSM.KOJI_NO = T_KIWAKU.KOJI_NO");
            sb.ApdL("                  )");
            sb.ApdN("   AND SAGYO_FLAG = ").ApdN(this.BindPrefix).ApdL("SAGYO_FLAG");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SAGYO_FLAG", SAGYO_FLAG.SHUKKAZUMI_VALUE1));


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

    #region 木枠明細データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 木枠明細データ削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelKiwakuMeisai(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE T_KIWAKU_MEISAI");
            sb.ApdL(" WHERE");
            sb.ApdL("       NOT EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_KIWAKU TK");
            sb.ApdL("                WHERE");
            sb.ApdL("                      TK.KOJI_NO = T_KIWAKU_MEISAI.KOJI_NO");
            sb.ApdL("                  )");

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

    #region 社外木枠明細データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 社外木枠明細データ削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelShagaiKiwakuMeisai(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE T_SHAGAI_KIWAKU_MEISAI");
            sb.ApdL(" WHERE");
            sb.ApdL("       NOT EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_KIWAKU TK");
            sb.ApdL("                WHERE");
            sb.ApdL("                      TK.KOJI_NO = T_SHAGAI_KIWAKU_MEISAI.KOJI_NO");
            sb.ApdL("                  )");

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

    #region 採番マスタ削除處理(ARNo)

    /// --------------------------------------------------
    /// <summary>
    /// 採番マスタ削除處理(ARNo)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns></returns>
    /// <create>Y.Higuchi 2010/09/13</create>
    /// <update>K.Tsutsumi 2018/01/22 Step8</update>
    /// <update>K.Tsutsumi 2020/06/26 SQLがレコード数に耐えれていない</update>
    /// <update>K.Tsutsumi 2020/09/14 EFA_SMS-133 出荷元保守で新規登録できないことへの対応</update>
    /// --------------------------------------------------
    public int DelSaibanARNo(DatabaseHelper dbHelper)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM M_SAIBAN");
            sb.ApdL(" WHERE");
            sb.ApdL("       NOT EXISTS (");
            sb.ApdL("                   SELECT 1");
            sb.ApdL("                     FROM M_NONYUSAKI");
            sb.ApdL("                    WHERE");
            sb.ApdN("                          M_NONYUSAKI.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdL("                      AND M_SAIBAN.SAIBAN_CD LIKE M_NONYUSAKI.NONYUSAKI_CD + '%'");
            sb.ApdL("                  )");
            sb.ApdN("   AND SAIBAN_CD NOT IN ( ");
            sb.ApdN(this.BindPrefix).ApdL("NONYUSAKI");
            sb.ApdN(", ").ApdN(this.BindPrefix).ApdN("AR_NONYUSAKI");
            sb.ApdN(", ").ApdN(this.BindPrefix).ApdN("BOX_NO");
            sb.ApdN(", ").ApdN(this.BindPrefix).ApdN("PALLET_NO");
            sb.ApdN(", ").ApdN(this.BindPrefix).ApdN("KOJI_NO");
            sb.ApdN(", ").ApdN(this.BindPrefix).ApdN("CASE_ID");
            sb.ApdN(", ").ApdN(this.BindPrefix).ApdN("TEMP_ID");
            sb.ApdN(", ").ApdN(this.BindPrefix).ApdN("BKUS_ID");
            sb.ApdN(", ").ApdN(this.BindPrefix).ApdN("BKARUS_ID");
            sb.ApdN(", ").ApdN(this.BindPrefix).ApdN("ZAIKO_TEMP_ID");
            sb.ApdN(", ").ApdN(this.BindPrefix).ApdN("MAIL");
            sb.ApdN(", ").ApdN(this.BindPrefix).ApdN("MAIL_ID");
            sb.ApdN(", ").ApdN(this.BindPrefix).ApdN("UNSOKAISHA_NO");
            sb.ApdN(", ").ApdN(this.BindPrefix).ApdN("CONSIGN_CD");
            sb.ApdN(", ").ApdN(this.BindPrefix).ApdN("DELIVER_CD");
            sb.ApdN(", ").ApdN(this.BindPrefix).ApdN("TEHAI_RENKEI");
            sb.ApdN(", ").ApdN(this.BindPrefix).ApdN("PACKING_NO");
            sb.ApdN(", ").ApdN(this.BindPrefix).ApdN("ESTIMATE_NO");
            sb.ApdN(", ").ApdN(this.BindPrefix).ApdN("PROJECT_NO");
            sb.ApdN(", ").ApdN(this.BindPrefix).ApdN("ASSY");
            sb.ApdN(", ").ApdN(this.BindPrefix).ApdN("GTMP");
            sb.ApdN(", ").ApdN(this.BindPrefix).ApdN("SIPF");
            sb.ApdL(" )");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI", SAIBAN_FLAG.US_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("AR_NONYUSAKI", SAIBAN_FLAG.ARUS_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", SAIBAN_FLAG.BOX_NO_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", SAIBAN_FLAG.PALLET_NO_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", SAIBAN_FLAG.KOJI_NO_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("CASE_ID", SAIBAN_FLAG.CASE_ID_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", SAIBAN_FLAG.TEMP_ID_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("BKUS_ID", SAIBAN_FLAG.BKUS_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("BKARUS_ID", SAIBAN_FLAG.BKARUS_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("ZAIKO_TEMP_ID", SAIBAN_FLAG.ZAIKO_TEMP_ID_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("MAIL", SAIBAN_FLAG.MAIL_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("MAIL_ID", SAIBAN_FLAG.MAIL_ID_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("UNSOKAISHA_NO", SAIBAN_FLAG.UNSOKAISHA_NO_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("CONSIGN_CD", SAIBAN_FLAG.CONSIGN_CD_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("DELIVER_CD", SAIBAN_FLAG.DELIVER_CD_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI", SAIBAN_FLAG.TEHAI_RENKEI_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("PACKING_NO", SAIBAN_FLAG.PACKING_NO_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_NO", SAIBAN_FLAG.ESTIMATE_NO_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("PROJECT_NO", SAIBAN_FLAG.PROJECT_NO_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("ASSY", SAIBAN_FLAG.ASSY_NO_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("GTMP", SAIBAN_FLAG.TEMP_AR_GOKI_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SIPF", SAIBAN_FLAG.SHIP_FROM_CD_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));

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

    #region 一時作業データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 一時作業データ削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/09/01</create>
    /// <update>K.Tsutsumi 2020/06/26 7日間保持する</update>
    /// --------------------------------------------------
    public int DelTempwork(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            const int ELAPSED_TIME = -7;
            DateTime date = DateTime.Now.AddDays(ELAPSED_TIME);

            // SQL文
            sb.ApdL("DELETE FROM T_TEMPWORK");
            sb.ApdL(" WHERE");
            sb.ApdN("       TORIKOMI_DATE < ").ApdN(this.BindPrefix).ApdL("TORIKOMI_DATE");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TORIKOMI_DATE", date));

            // SQL実行
            record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            return record;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 一時作業明細データ削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/09/01</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelTempworkMeisai(DatabaseHelper dbHelper)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM T_TEMPWORK_MEISAI");
            sb.ApdL(" WHERE");
            sb.ApdL("       NOT EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_TEMPWORK TT");
            sb.ApdL("                WHERE");
            sb.ApdL("                      TT.TEMP_ID = T_TEMPWORK_MEISAI.TEMP_ID");
            sb.ApdL("                  )");

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

    #region 本体の履歴データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 本体の履歴データ削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/05/10</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelHontaiRireki(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("DELETE");
            sb.ApdL("  FROM T_RIREKI");
            sb.ApdN(" WHERE UPDATE_DATE < ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("   AND SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdL("    OR NOT EXISTS (");
            sb.ApdL("       SELECT 1");
            sb.ApdL("         FROM M_BUKKEN MB");
            sb.ApdL("        WHERE MB.SHUKKA_FLAG = T_RIREKI.SHUKKA_FLAG");
            sb.ApdL("          AND MB.BUKKEN_NO = T_RIREKI.BUKKEN_NO");
            sb.ApdL("    )");

            paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_DATE", cond.TargetDate));
            paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.NORMAL_VALUE1));

            return dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region ARの履歴データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// ARの履歴データ削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/05/11</create>
    /// <update>K.Tsutsumi 2020/06/30 念の為、SHUKKA_FLAGを条件に追加</update>
    /// --------------------------------------------------
    public int DelARRireki(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            int ret = 0;
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("DELETE RIREKI FROM T_RIREKI AS RIREKI");
            sb.ApdL(" WHERE RIREKI.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdL("   AND");
            sb.ApdL("       EXISTS(");
            sb.ApdL("  SELECT 1");
            sb.ApdL("    FROM (");
            sb.ApdL("    SELECT ROW_NUMBER() OVER(PARTITION BY GAMEN_FLAG, SHUKKA_FLAG, NONYUSAKI_CD, AR_NO ORDER BY UPDATE_DATE DESC) AS RANK_NO");
            sb.ApdL("          ,GAMEN_FLAG");
            sb.ApdL("          ,UPDATE_PC_NAME");
            sb.ApdL("          ,UPDATE_DATE");
            sb.ApdL("          ,UPDATE_USER_ID");
            sb.ApdL("          ,OPERATION_FLAG");
            sb.ApdL("          ,NONYUSAKI_CD");
            sb.ApdL("          ,AR_NO");
            sb.ApdL("      FROM T_RIREKI");
            sb.ApdL("     WHERE");
            sb.ApdL("           T_RIREKI.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdL("    ) TR");
            sb.ApdL("   INNER JOIN T_AR TA");
            sb.ApdL("           ON TA.NONYUSAKI_CD = TR.NONYUSAKI_CD");
            sb.ApdL("          AND TA.AR_NO = TR.AR_NO");
            sb.ApdL("   WHERE TR.RANK_NO > 5");
            sb.ApdN("     AND TA.CREATE_DATE < ").ApdN(this.BindPrefix).ApdL("CREATE_DATE");
            sb.ApdL("     AND TR.GAMEN_FLAG = RIREKI.GAMEN_FLAG");
            sb.ApdL("     AND TR.UPDATE_PC_NAME = RIREKI.UPDATE_PC_NAME");
            sb.ApdL("     AND TR.UPDATE_DATE = RIREKI.UPDATE_DATE");
            sb.ApdL("     AND TR.UPDATE_USER_ID = RIREKI.UPDATE_USER_ID");
            sb.ApdL("     AND TR.OPERATION_FLAG = RIREKI.OPERATION_FLAG");
            sb.ApdL(" ) ");

            paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));
            paramCollection.Add(iNewParameter.NewDbParameter("CREATE_DATE", cond.TargetDate));
            ret = dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            sb = new StringBuilder();
            paramCollection = new DbParamCollection();

            sb.ApdL("DELETE RIREKI FROM T_RIREKI AS RIREKI");
            sb.ApdL(" WHERE RIREKI.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdL("   AND");
            sb.ApdL("       NOT EXISTS (");
            sb.ApdL("  SELECT 1");
            sb.ApdL("    FROM T_AR TA");
            sb.ApdL("   WHERE TA.NONYUSAKI_CD = RIREKI.NONYUSAKI_CD");
            sb.ApdL("     AND TA.AR_NO = RIREKI.AR_NO");
            sb.ApdL(" )");

            paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));
            ret = dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region ロケーションマスタ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// ロケーションマスタ削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/09/03</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelLocation(DatabaseHelper dbHelper)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE M_LOCATION");
            sb.ApdL(" WHERE");
            sb.ApdL("       NOT EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM M_NONYUSAKI MN");
            sb.ApdL("                WHERE");
            sb.ApdN("                      MN.SHUKKA_FLAG = M_LOCATION.SHUKKA_FLAG");
            sb.ApdL("                  AND MN.BUKKEN_NO = M_LOCATION.BUKKEN_NO");
            sb.ApdL("                  )");

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

    #region 在庫データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 在庫データ削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/09/03</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelStock(DatabaseHelper dbHelper)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE T_STOCK");
            sb.ApdL(" WHERE");
            sb.ApdL("       NOT EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM M_NONYUSAKI MN");
            sb.ApdL("                WHERE");
            sb.ApdN("                      MN.SHUKKA_FLAG = T_STOCK.SHUKKA_FLAG");
            sb.ApdL("                  AND MN.NONYUSAKI_CD = T_STOCK.NONYUSAKI_CD");
            sb.ApdL("                  )");

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

    #region 棚卸データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 棚卸データ削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/09/03</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelInvent(DatabaseHelper dbHelper)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE T_INVENT");
            sb.ApdL(" WHERE");
            sb.ApdN("       INVENT_DATE < ").ApdN(this.BindPrefix).ApdL("INVENT_DATE");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("INVENT_DATE", DateTime.Today.AddDays(-1).ToString("yyyy/MM/dd")));

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

    #region 実績データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 実績データ削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/09/03</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelJisseki(DatabaseHelper dbHelper)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE T_JISSEKI");
            sb.ApdL(" WHERE");
            sb.ApdL("       NOT EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM M_NONYUSAKI MN");
            sb.ApdL("                WHERE");
            sb.ApdN("                      MN.SHUKKA_FLAG = T_JISSEKI.SHUKKA_FLAG");
            sb.ApdL("                  AND MN.NONYUSAKI_CD = T_JISSEKI.NONYUSAKI_CD");
            sb.ApdL("                  )");

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

    #region 一時作業データ（現地部品管理）削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 一時作業データ（現地部品管理）削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/09/03</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelBuhinTempwork(DatabaseHelper dbHelper)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM T_BUHIN_TEMPWORK");
            sb.ApdL(" WHERE");
            sb.ApdN("       TORIKOMI_DATE < ").ApdN(this.BindPrefix).ApdL("TORIKOMI_DATE");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TORIKOMI_DATE", DateTime.Today));

            // SQL実行
            record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            return record;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 一時作業明細データ（現地部品管理）削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/09/03</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelBuhinTempworkMeisai(DatabaseHelper dbHelper)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM T_BUHIN_TEMPWORK_MEISAI");
            sb.ApdL(" WHERE");
            sb.ApdL("       NOT EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_BUHIN_TEMPWORK TBT");
            sb.ApdL("                WHERE");
            sb.ApdL("                      TBT.TEMP_ID = T_BUHIN_TEMPWORK_MEISAI.TEMP_ID");
            sb.ApdL("                  )");

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

    #region メールデータ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// メールデータ削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelMail(DatabaseHelper dbHelper)
    {
        try
        {
            int rec = 0;
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("DELETE FROM T_MAIL");
            sb.ApdL(" WHERE UPDATE_DATE < @UPDATE_DATE");
            sb.ApdL("   AND MAIL_STATUS <> @MAIL_STATUS");

            pc.Add(iNewParam.NewDbParameter("UPDATE_DATE", DateTime.Today.AddDays(-14).ToString("yyyy/MM/dd")));
            pc.Add(iNewParam.NewDbParameter("MAIL_STATUS", MAIL_STATUS.MI_VALUE1));

            rec += dbHelper.ExecuteNonQuery(sb.ToString(), pc);

            return rec;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region DBバックアップ処理

    /// --------------------------------------------------
    /// <summary>
    /// DBバックアップ処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool DBBackupExec(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("BACKUP");
            sb.ApdN("       DATABASE ").ApdN(this.BindPrefix).ApdL("DB_NAME");
            sb.ApdN("       TO DISK = ").ApdN(this.BindPrefix).ApdL("FILE_NAME");
            sb.ApdL("       WITH NOFORMAT");
            sb.ApdL("     , INIT");
            sb.ApdN("     , NAME = ").ApdN(this.BindPrefix).ApdL("BACKUP_NAME");
            sb.ApdL("     , SKIP");
            sb.ApdL("     , NOREWIND");
            sb.ApdL("     , NOUNLOAD");
            sb.ApdL("     , STATS = 10");

            // バインド変数設定
            string fileName = Path.Combine(cond.ExpBackupPath, string.Format(ComDefine.DB_BACKUP_FILE_NAME, DateTime.Today.ToString("yyyyMMdd")));
            string backupName = string.Format(ComDefine.DB_BACKUP_BACKUP_NAME, DateTime.Today.ToString("yyyy/MM/dd"));
            paramCollection.Add(iNewParam.NewDbParameter("DB_NAME", ComDefine.DB_BACKUP_DBNAME));
            paramCollection.Add(iNewParam.NewDbParameter("FILE_NAME", fileName));
            paramCollection.Add(iNewParam.NewDbParameter("BACKUP_NAME", backupName));

            // SQL実行
            //dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            using (DbCommand cmd = dbHelper.NewDbCommand(sb.ToString(), CommandType.Text))
            {
                cmd.CommandTimeout = 600;
                this.SetDbCommandParameters(cmd, paramCollection, dbHelper.BindByName);
                cmd.ExecuteNonQuery();
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #region タイムアウト時間を設定してＳＱＬを実行

    #region SetDbCommandParameters(DbCommand cmd, DbParamCollection parameterCollection)

    /// --------------------------------------------------
    /// <summary>
    /// DbCommandにパラメータを設定する時の共通設定
    /// </summary>
    /// <param name="cmd">DbCommand</param>
    /// <param name="parameterCollection">Parameterのコレクション</param>
    /// <create>2013/12/06 K.Tsutsumi</create>
    /// <update></update>
    /// --------------------------------------------------
    private void SetDbCommandParameters(DbCommand cmd, DbParamCollection parameterCollection, bool bindByName)
    {
        //Parameterコレクションがnullであれば処理を抜ける
        if (parameterCollection == null)
        {
            return;
        }

        this.SetBindByName(cmd, bindByName);
        for (int i = 0; i < parameterCollection.Count; i++)
        {
            cmd.Parameters.Add(parameterCollection[i]);
        }
    }

    #endregion

    #region SetBindByName(DbCommand cmd, bool value)

    /// --------------------------------------------------
    /// <summary>
    /// BindByNameプロパティがある場合プロパティの設定を行う。
    /// </summary>
    /// <param name="cmd">DbCommand</param>
    /// <param name="value">プロパティの設定値</param>
    /// <create>2013/12/06 K.Tsutsumi</create>
    /// <update></update>
    /// --------------------------------------------------
    private void SetBindByName(DbCommand cmd, bool value)
    {
        Type cmdType = cmd.GetType();
        PropertyInfo propInfo = cmdType.GetProperty(BIND_BY_NAME_PROPERTY_NAME);
        if (propInfo != null)
        {
            //プロパティがある場合
            propInfo.SetValue(cmd, value, null);
        }
    }

    #endregion

    #endregion

    #endregion

    #region トランザクションログの切り捨て
    /// --------------------------------------------------
    /// <summary>
    /// トランザクションログの切り捨て
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>K.Tsutsumi 2020/06/26</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool TranBackupExec(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("BACKUP");
            sb.ApdN("       LOG ").ApdN(this.BindPrefix).ApdL("DB_NAME");
            sb.ApdN("       TO DISK = N'NUL'");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("DB_NAME", ComDefine.DB_BACKUP_DBNAME));

            // SQL実行
            using (DbCommand cmd = dbHelper.NewDbCommand(sb.ToString(), CommandType.Text))
            {
                cmd.CommandTimeout = 600;
                this.SetDbCommandParameters(cmd, paramCollection, dbHelper.BindByName);
                cmd.ExecuteNonQuery();
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    #endregion

    #region トランザクションログの圧縮（復旧モデルをSimpleへ）
    /// --------------------------------------------------
    /// <summary>
    /// トランザクションログの圧縮（復旧モデルをSimpleへ）
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>K.Tsutsumi 2020/06/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool SetRecoverySimpleExec(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdN("ALTER DATABASE ").ApdN(ComDefine.DB_BACKUP_DBNAME).ApdL(" SET RECOVERY SIMPLE");

            // SQL実行
            using (DbCommand cmd = dbHelper.NewDbCommand(sb.ToString(), CommandType.Text))
            {
                cmd.CommandTimeout = 600;
                this.SetDbCommandParameters(cmd, paramCollection, dbHelper.BindByName);
                cmd.ExecuteNonQuery();
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    #endregion

    #region トランザクションログの圧縮（復旧モデルをFullへ）
    /// --------------------------------------------------
    /// <summary>
    /// トランザクションログの圧縮（復旧モデルをFullへ）
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>K.Tsutsumi 2020/06/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool SetRecoveryFullExec(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdN("ALTER DATABASE ").ApdN(ComDefine.DB_BACKUP_DBNAME).ApdL(" SET RECOVERY FULL");

            // SQL実行
            using (DbCommand cmd = dbHelper.NewDbCommand(sb.ToString(), CommandType.Text))
            {
                cmd.CommandTimeout = 600;
                this.SetDbCommandParameters(cmd, paramCollection, dbHelper.BindByName);
                cmd.ExecuteNonQuery();
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    #endregion

    #region トランザクションログの圧縮（DBCC SHRINKFILE）
    /// --------------------------------------------------
    /// <summary>
    /// トランザクションログの圧縮（DBCC SHRINKFILE）
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>K.Tsutsumi 2020/06/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool DbccShrinkFileExec(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DBCC SHRINKFILE (2 , 1024)");

            // SQL実行
            using (DbCommand cmd = dbHelper.NewDbCommand(sb.ToString(), CommandType.Text))
            {
                cmd.CommandTimeout = 600;
                this.SetDbCommandParameters(cmd, paramCollection, dbHelper.BindByName);
                cmd.ExecuteNonQuery();
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region J0100030:メール送信

    #region メール設定マスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// メール設定マスタ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns></returns>
    /// <create>R.Katsuo 2017/09/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetMailSetting(DatabaseHelper dbHelper)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT *");
            sb.ApdL("  FROM M_MAIL_SETTING");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_MAIL_SETTING.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region メールデータ取得

    /// --------------------------------------------------
    /// <summary>
    /// メールデータ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns></returns>
    /// <create>R.Katsuo 2017/09/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetMail(DatabaseHelper dbHelper)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT *");
            sb.ApdL("  FROM T_MAIL");
            sb.ApdN(" WHERE T_MAIL.MAIL_STATUS = ").ApdN(this.BindPrefix).ApdL("MAIL_STATUS");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("MAIL_STATUS", MAIL_STATUS.MI_VALUE1));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_MAIL.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region メールデータ更新処理

    /// --------------------------------------------------
    /// <summary>
    /// メールデータ更新処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dt">更新データ</param>
    /// <param name="serverTime"></param>
    /// <returns>影響を与えた行数</returns>
    /// <create>R.Katsuo 2017/09/07</create>
    /// <update>H.Tajimi 2018/12/05 添付ファイル対応</update>
    /// --------------------------------------------------
    public int UpdMail(DatabaseHelper dbHelper, DataTable dt, DateTime serverTime)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_MAIL");
            sb.ApdN("   SET MAIL_STATUS = ").ApdN(this.BindPrefix).ApdL("MAIL_STATUS");
            sb.ApdN("     , RETRY_COUNT = ").ApdN(this.BindPrefix).ApdL("RETRY_COUNT");
            sb.ApdN("     , REASON = ").ApdN(this.BindPrefix).ApdL("REASON");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , DISP_REASON = ").ApdN(this.BindPrefix).ApdL("DISP_REASON");
            sb.ApdN(" WHERE MAIL_ID = ").ApdN(this.BindPrefix).ApdL("MAIL_ID");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("MAIL_STATUS", UtilData.GetFld(dt, 0, Def_T_MAIL.MAIL_STATUS)));
            paramCollection.Add(iNewParam.NewDbParameter("RETRY_COUNT", UtilData.GetFld(dt, 0, Def_T_MAIL.RETRY_COUNT)));
            paramCollection.Add(iNewParam.NewDbParameter("REASON", UtilData.GetFld(dt, 0, Def_T_MAIL.REASON)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", serverTime));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", "system"));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", "MailSend"));
            paramCollection.Add(iNewParam.NewDbParameter("MAIL_ID", UtilData.GetFldToDecimal(dt, 0, Def_T_MAIL.MAIL_ID)));
            paramCollection.Add(iNewParam.NewDbParameter("DISP_REASON", UtilData.GetFld(dt, 0, Def_T_MAIL.DISP_REASON)));

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

    #region J0100030:SKS連携処理

    #region SELECT

    #region SKS連携マスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// SKS連携マスタ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns>SKS連携マスタ</returns>
    /// <create>H.Tajimi 2018/11/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetSKS(DatabaseHelper dbHelper)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       SHORI_FLAG");
            sb.ApdL("     , LASTEST_DATE");
            sb.ApdL("     , START_TIME");
            sb.ApdL("     , SKS_FOLDER");
            sb.ApdL("     , SKS_FILE_NAME");
            sb.ApdL("     , SKS_LOG_FOLDER");
            sb.ApdL("  FROM ");
            sb.ApdL("       M_SKS");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_SKS.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region SKS連携マスタ取得(行レベルロック)

    /// --------------------------------------------------
    /// <summary>
    /// SKS連携マスタ取得(行レベルロック)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns>SKS連携マスタ</returns>
    /// <create>H.Tajimi 2018/11/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet LockSKS(DatabaseHelper dbHelper)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       SHORI_FLAG");
            sb.ApdL("     , LASTEST_DATE");
            sb.ApdL("     , START_TIME");
            sb.ApdL("     , SKS_FOLDER");
            sb.ApdL("     , SKS_FILE_NAME");
            sb.ApdL("     , SKS_LOG_FOLDER");
            sb.ApdL("  FROM ");
            sb.ApdL("       M_SKS");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_SKS.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region SKS手配明細WORKデータ件数確認

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細WORKデータ件数確認
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/20</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool IsEmptyTehaiSKSWork(DatabaseHelper dbHelper)
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
            sb.ApdL("  FROM ");
            sb.ApdL("       T_TEHAI_SKS_WORK");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);
            if (0 == ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_CNT))
            {
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

    #region SKS手配明細取得

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/21</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTehaiSKS(DatabaseHelper dbHelper)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEHAI_SKS.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       T_TEHAI_SKS.TEHAI_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEHAI_SKS");
            sb.ApdL(" INNER JOIN T_TEHAI_SKS_WORK");
            sb.ApdL("         ON T_TEHAI_SKS_WORK.TEHAI_NO = T_TEHAI_SKS.TEHAI_NO");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細存在有無
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="tehaiNo">手配No</param>
    /// <returns>存在有無</returns>
    /// <create>D.Okumura 2022/01/04</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool IsExistsTehaiSKS(DatabaseHelper dbHelper, string tehaiNo)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEHAI_SKS_WORK.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       1");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEHAI_SKS");
            sb.ApdL(" WHERE");
            sb.ApdN("       T_TEHAI_SKS.TEHAI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NO", tehaiNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return UtilData.ExistsData(dt);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    #endregion

    #region SKS手配明細(行レベルロック)

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dr">[手配No]DataRow</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/21</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable LockTehaiSKS(DatabaseHelper dbHelper, DataRow dr)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEHAI_SKS.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       T_TEHAI_SKS.TEHAI_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEHAI_SKS");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdN(" WHERE T_TEHAI_SKS.TEHAI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NO", ComFunc.GetFld(dr, Def_T_TEHAI_SKS.TEHAI_NO)));

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

    #region 手配SKS連携データの取得

    /// --------------------------------------------------
    /// <summary>
    /// 手配SKS連携データの取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="tehaiNo">手配No</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/08/22</create>
    /// <update>J.Chen 2022/11/17 手配No事前登録対応</update>
    /// --------------------------------------------------
    public DataTable GetTehaiMeisaiSKS(DatabaseHelper dbHelper, string tehaiNo)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEHAI_SKS_WORK.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO");
            sb.ApdL("     , T_TEHAI_MEISAI_SKS.TEHAI_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEHAI_MEISAI_SKS");
            sb.ApdL("LEFT JOIN T_TEHAI_SKS");
            sb.ApdL("        ON T_TEHAI_MEISAI_SKS.TEHAI_NO = T_TEHAI_SKS.TEHAI_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       T_TEHAI_MEISAI_SKS.TEHAI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_NO");
            sb.ApdN("   AND T_TEHAI_SKS.TEHAI_NO IS NOT NULL");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NO", tehaiNo));

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

    #region 技連マスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// 技連マスタ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="ecsNo">技連No</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/22</create>
    /// <update>K.Tsutsumi 2018/01/24 ログを減らす</update>
    /// --------------------------------------------------
    public DataTable GetEcs(DatabaseHelper dbHelper, string ecsNo)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       M_ECS.ECS_QUOTA");
            sb.ApdL("     , M_ECS.ECS_NO");
            sb.ApdL("     , M_ECS.PROJECT_NO");
            sb.ApdL("     , M_ECS.SEIBAN");
            sb.ApdL("     , M_ECS.CODE");
            sb.ApdL("     , M_ECS.KISHU");
            sb.ApdL("     , M_ECS.SEIBAN_CODE");
            sb.ApdL("     , M_ECS.AR_NO");
            sb.ApdL("     , M_ECS.KANRI_FLAG");
            sb.ApdL("     , M_ECS.CREATE_USER_ID");
            sb.ApdL("     , M_ECS.CREATE_USER_NAME");
            sb.ApdL("     , M_ECS.UPDATE_USER_ID");
            sb.ApdL("     , M_ECS.UPDATE_USER_NAME");
            sb.ApdL("     , M_ECS.MAINTE_DATE");
            sb.ApdL("     , M_ECS.MAINTE_USER_ID");
            sb.ApdL("     , M_ECS.MAINTE_USER_NAME");
            sb.ApdL("     , M_ECS.VERSION");
            sb.ApdL("  FROM ");
            sb.ApdL("       M_ECS");
            sb.ApdL(" WHERE ");
            sb.ApdL("       ECS_QUOTA = (SELECT MAX(ECS_QUOTA) FROM M_ECS)");
            sb.ApdN("   AND ECS_NO = ").ApdN(this.BindPrefix).ApdL("ECS_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("ECS_NO", ecsNo));

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

    #region 手配明細取得(単価更新用)

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細取得(単価更新用)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="tehaiKindFlag">手配種別</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/22</create>
    /// <update>T.Nukaga 2021/12/02 Step14 単価更新対象データの検索条件変更</update>
    /// <update>T.Nukaga 2021/12/24 Step14 わたり発注の場合、発注状態のT_TEHAI_SKSがあれば手配明細更新対象にしない</update>
    /// --------------------------------------------------
    public DataTable GetTehaiMeisaiForUpdUnitPrice(DatabaseHelper dbHelper, string tehaiKindFlag)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEHAI_SKS_WORK.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT T_TEHAI_MEISAI.TEHAI_RENKEI_NO");
            sb.ApdL("     , T_TEHAI_MEISAI.TEHAI_KIND_FLAG");
            sb.ApdL("  FROM T_TEHAI_MEISAI");
            sb.ApdL(" INNER JOIN T_TEHAI_MEISAI_SKS");
            sb.ApdL("         ON T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO = T_TEHAI_MEISAI.TEHAI_RENKEI_NO");
            // わたり発注の場合、手配明細に紐づくSKS手配明細が見積のデータが1件でもあれば単価更新対象外とする
            if (tehaiKindFlag == TEHAI_KIND_FLAG.ACROSS_VALUE1)
            {
                sb.ApdL("    AND NOT EXISTS (");
                sb.ApdL("    SELECT TTMS.TEHAI_RENKEI_NO FROM T_TEHAI_MEISAI_SKS TTMS");
                sb.ApdL("    INNER JOIN T_TEHAI_SKS");
                sb.ApdL("    ON T_TEHAI_SKS.TEHAI_NO = TTMS.TEHAI_NO");
                sb.ApdN("    AND T_TEHAI_SKS.HACCHU_FLAG = ").ApdN(this.BindPrefix).ApdL("HACCHU_FLAG_MITSUMORI");
                sb.ApdL("    WHERE TTMS.TEHAI_RENKEI_NO = T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO)");
            }
            sb.ApdL(" INNER JOIN T_TEHAI_SKS");
            sb.ApdL("         ON T_TEHAI_SKS.TEHAI_NO = T_TEHAI_MEISAI_SKS.TEHAI_NO");
            sb.ApdL("        AND T_TEHAI_SKS.HACCHU_FLAG <> T_TEHAI_SKS.ZENKAI_HACCHU_FLAG");
            sb.ApdN("        AND T_TEHAI_SKS.ZENKAI_HACCHU_FLAG = ").ApdN(this.BindPrefix).ApdL("HACCHU_FLAG_MITSUMORI");
            sb.ApdN(" WHERE T_TEHAI_MEISAI.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG");
            sb.ApdN("   AND T_TEHAI_MEISAI.TEHAI_KIND_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_FLAG");
            sb.ApdL("   AND NOT EXISTS (");
            sb.ApdL("      SELECT 1");
            sb.ApdL("        FROM T_SHUKKA_MEISAI");
            sb.ApdL("       WHERE T_SHUKKA_MEISAI.TEHAI_RENKEI_NO = T_TEHAI_MEISAI.TEHAI_RENKEI_NO");
            sb.ApdN("         AND T_SHUKKA_MEISAI.JYOTAI_FLAG in (").ApdN(this.BindPrefix).ApdN("JYOTAI_FLAG_SHUKKAZUMI").ApdN(", ").ApdN(this.BindPrefix).ApdL("JYOTAI_FLAG_UKEIREZUMI");
            sb.ApdL("   ))");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_FLAG", TEHAI_FLAG.ORDERED_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_KIND_FLAG", tehaiKindFlag));
            paramCollection.Add(iNewParam.NewDbParameter("HACCHU_FLAG_MITSUMORI", HACCHU_FLAG.STATUS_1_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("JYOTAI_FLAG_SHUKKAZUMI", JYOTAI_FLAG.SHUKKAZUMI_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("JYOTAI_FLAG_UKEIREZUMI", JYOTAI_FLAG.UKEIREZUMI_VALUE1));

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

    #region 手配明細取得(連携済み単価更新用)

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細取得(連携済み単価更新用)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="tehaiNo">手配種別</param>
    /// <returns></returns>
    /// <create>J.Chen 2022/12/29</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTehaiMeisaiForUpdUnitPrice2(DatabaseHelper dbHelper, string tehaiKindFlag)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEHAI_SKS_WORK.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT DISTINCT T_TEHAI_MEISAI.TEHAI_RENKEI_NO");
            sb.ApdL("     , T_TEHAI_MEISAI.TEHAI_KIND_FLAG");
            sb.ApdL("  FROM T_TEHAI_MEISAI");
            sb.ApdL(" INNER JOIN T_TEHAI_MEISAI_SKS");
            sb.ApdL("         ON T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO = T_TEHAI_MEISAI.TEHAI_RENKEI_NO");
            // わたり発注の場合、手配明細に紐づくSKS手配明細が見積のデータが1件でもあれば単価更新対象外とする
            if (tehaiKindFlag == TEHAI_KIND_FLAG.ACROSS_VALUE1)
            {
                sb.ApdL("    AND NOT EXISTS (");
                sb.ApdL("    SELECT TTMS.TEHAI_RENKEI_NO FROM T_TEHAI_MEISAI_SKS TTMS");
                sb.ApdL("    INNER JOIN T_TEHAI_SKS");
                sb.ApdL("    ON T_TEHAI_SKS.TEHAI_NO = TTMS.TEHAI_NO");
                sb.ApdN("    AND T_TEHAI_SKS.HACCHU_FLAG = ").ApdN(this.BindPrefix).ApdL("HACCHU_FLAG_MITSUMORI");
                sb.ApdL("    WHERE TTMS.TEHAI_RENKEI_NO = T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO)");
            }
            sb.ApdL(" INNER JOIN T_TEHAI_SKS");
            sb.ApdL("         ON T_TEHAI_SKS.TEHAI_NO = T_TEHAI_MEISAI_SKS.TEHAI_NO");
            // SKS手配明細と手配明細の単価が一致しないデータを絞り込む
            sb.ApdL("        AND T_TEHAI_SKS.TEHAI_UNIT_PRICE <> IsNull(T_TEHAI_MEISAI.UNIT_PRICE , 0)");
            // SKS手配明細WORKに存在するデータを絞り込む
            sb.ApdL(" INNER JOIN T_TEHAI_SKS_WORK");
            sb.ApdL("         ON T_TEHAI_SKS_WORK.TEHAI_NO = T_TEHAI_SKS.TEHAI_NO");
            sb.ApdN(" WHERE T_TEHAI_MEISAI.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG");
            sb.ApdN("   AND T_TEHAI_MEISAI.TEHAI_KIND_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_FLAG");

            // 見積済みのデータは対象外とする
            sb.ApdL("   AND T_TEHAI_MEISAI.ESTIMATE_NO IS NULL");

            sb.ApdL("   AND NOT EXISTS (");
            sb.ApdL("      SELECT 1");
            sb.ApdL("        FROM T_SHUKKA_MEISAI");
            sb.ApdL("       WHERE T_SHUKKA_MEISAI.TEHAI_RENKEI_NO = T_TEHAI_MEISAI.TEHAI_RENKEI_NO");
            sb.ApdN("         AND T_SHUKKA_MEISAI.JYOTAI_FLAG in (").ApdN(this.BindPrefix).ApdN("JYOTAI_FLAG_SHUKKAZUMI").ApdN(", ").ApdN(this.BindPrefix).ApdL("JYOTAI_FLAG_UKEIREZUMI");
            sb.ApdL("   ))");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_FLAG", TEHAI_FLAG.ORDERED_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_KIND_FLAG", tehaiKindFlag));
            paramCollection.Add(iNewParam.NewDbParameter("HACCHU_FLAG_MITSUMORI", HACCHU_FLAG.STATUS_1_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("JYOTAI_FLAG_SHUKKAZUMI", JYOTAI_FLAG.SHUKKAZUMI_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("JYOTAI_FLAG_UKEIREZUMI", JYOTAI_FLAG.UKEIREZUMI_VALUE1));

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

    #region 手配明細＋技連マスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細＋技連マスタ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="zyoken">検索条件判定用</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/21</create>
    /// <update>K.Tsutsumi 2019/02/11 手配明細の手配No.がnullのものだけを取得する</update>
    /// <update>K.Tsutsumi 2020/05/15 技連No.10桁対応</update>
    /// <update>K.Tsutsumi 2020/05/16 マッチング条件から図面追番を外す</update>
    /// <update>T.Nukaga 2021/12/02 Step14 発注フラグ、受入日、検収日追加</update>
    /// <update>T.Nukaga 2021/12/08 Step14 対象データ検索条件変更,引数追加</update>
    /// <update>J.Chen 2022/11/17 Step15 手配No事前登録可能に変更、</update>
    /// --------------------------------------------------
    public DataTable GetTehaiMeisaiAndEcs(DatabaseHelper dbHelper, ComDefine.SKSRenkeiZyoken zyoken)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEHAI_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       T_TEHAI_MEISAI.TEHAI_RENKEI_NO");
            sb.ApdL("     , T_TEHAI_SKS_WORK.SEIBAN_CODE");
            sb.ApdL("     , T_TEHAI_SKS_WORK.HINBAN");
            sb.ApdL("     , T_TEHAI_SKS_WORK.PDM_WORK_NAME");
            sb.ApdL("     , T_TEHAI_SKS_WORK.TEHAI_NO");
            sb.ApdL("     , T_TEHAI_SKS_WORK.TEHAI_QTY");
            sb.ApdL("     , T_TEHAI_SKS_WORK.KATASHIKI");
            sb.ApdL("     , T_TEHAI_SKS_WORK.KAITO_DATE");
            sb.ApdL("     , T_TEHAI_SKS_WORK.TEHAI_UNIT_PRICE");
            sb.ApdL("     , T_TEHAI_SKS_WORK.ECS_NO");
            sb.ApdL("     , T_TEHAI_SKS_WORK.ZUMEN_OIBAN");
            sb.ApdL("     , T_TEHAI_SKS_WORK.HACCHU_ZYOTAI_FLAG");
            sb.ApdL("     , T_TEHAI_SKS_WORK.ZUMEN_KEISHIKI_S");
            sb.ApdL("     , T_TEHAI_SKS_WORK.HACCHU_FLAG");
            sb.ApdL("     , T_TEHAI_SKS_WORK.UKEIRE_DATE");
            sb.ApdL("     , T_TEHAI_SKS_WORK.KENSHU_DATE");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEHAI_MEISAI");
            sb.ApdL(" INNER JOIN M_ECS");
            sb.ApdL("         ON T_TEHAI_MEISAI.ECS_QUOTA = M_ECS.ECS_QUOTA");
            sb.ApdL("        AND T_TEHAI_MEISAI.ECS_NO = M_ECS.ECS_NO");
            sb.ApdL(" INNER JOIN T_TEHAI_SKS_WORK");
            sb.ApdL("         ON T_TEHAI_SKS_WORK.SEIBAN_CODE = M_ECS.SEIBAN_CODE");

            // Excel運用でも「図面追番」を条件にせずとも問題なく取得できているので、入力負荷軽減のため条件を外す
            //sb.ApdL("        AND IsNULL(T_TEHAI_SKS_WORK.ZUMEN_OIBAN, '') = IsNULL(T_TEHAI_MEISAI.ZUMEN_OIBAN, '')");
            
            sb.ApdL("        AND T_TEHAI_SKS_WORK.TEHAI_QTY = T_TEHAI_MEISAI.TEHAI_QTY");

            // 技連No.は、以下のルールで比較する
            // 「CF」から始まる技連No.は、7桁で比較する
            //  上記以外の技連No.は、10桁で比較する
            //sb.ApdL("        AND CASE WHEN LEN(T_TEHAI_SKS_WORK.ECS_NO) < 8");
            //sb.ApdL("                 THEN T_TEHAI_SKS_WORK.ECS_NO");
            //sb.ApdL("            ELSE SUBSTRING(T_TEHAI_SKS_WORK.ECS_NO, 1, 7) END = M_ECS.ECS_NO");
            //sb.ApdL("        AND CASE WHEN SUBSTRING(T_TEHAI_SKS_WORK.ECS_NO, 1, 2) = 'CF'");
            sb.ApdL("        AND CASE WHEN SUBSTRING(T_TEHAI_SKS_WORK.ECS_NO, 1, 2) = 'CF' THEN SUBSTRING(T_TEHAI_SKS_WORK.ECS_NO, 1, 7)");
            sb.ApdL("                 ELSE SUBSTRING(T_TEHAI_SKS_WORK.ECS_NO, 1, 10)");
            sb.ApdL("            END");
            sb.ApdL("            = ");
            sb.ApdL("            CASE WHEN SUBSTRING(M_ECS.ECS_NO, 1, 2) = 'CF' THEN SUBSTRING(M_ECS.ECS_NO, 1, 7)");
            sb.ApdL("                 ELSE SUBSTRING(M_ECS.ECS_NO, 1, 10)");
            sb.ApdL("            END");
            sb.ApdL("        AND ");

            // (SKS)品番が(手配)図番型式と一致
            if (zyoken == ComDefine.SKSRenkeiZyoken.SKSHinban)
            {
                sb.ApdL("            T_TEHAI_SKS_WORK.HINBAN = T_TEHAI_MEISAI.ZUMEN_KEISHIKI");
            }
            // (SKS)型式が(手配)図番型式と一致
            else if (zyoken == ComDefine.SKSRenkeiZyoken.SKSKatashiki)
            {
                sb.ApdL("            T_TEHAI_SKS_WORK.KATASHIKI = T_TEHAI_MEISAI.ZUMEN_KEISHIKI");
            }
            // 品番が図番型式2と一致、または、一致用図番形式(PDM作業名)が一致用図番形式と一致
            else if (zyoken == ComDefine.SKSRenkeiZyoken.SKSHinbanPDM)
            {
                sb.ApdL("            (T_TEHAI_SKS_WORK.HINBAN = T_TEHAI_MEISAI.ZUMEN_KEISHIKI2");
                sb.ApdL("             OR T_TEHAI_SKS_WORK.ZUMEN_KEISHIKI_S = T_TEHAI_MEISAI.ZUMEN_KEISHIKI_S");
                sb.ApdL("            )");
            }
            sb.ApdN(" WHERE T_TEHAI_MEISAI.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG");
            sb.ApdL("        AND NOT EXISTS (");
            sb.ApdL("            SELECT 1 ");
            sb.ApdL("              FROM ");
            sb.ApdL("                   T_TEHAI_MEISAI_SKS");
            // 手配SKS連携は存在しているが、SKS手配明細にない場合、登録可能
            sb.ApdL("            LEFT JOIN T_TEHAI_SKS");
            sb.ApdL("                    ON T_TEHAI_MEISAI_SKS.TEHAI_NO = T_TEHAI_SKS.TEHAI_NO");
            sb.ApdL("             WHERE T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO = T_TEHAI_MEISAI.TEHAI_RENKEI_NO");
            sb.ApdL("               AND T_TEHAI_SKS.TEHAI_NO IS NOT NULL"); // SKS手配明細にない
            sb.ApdL("        )");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_FLAG", TEHAI_FLAG.ORDERED_VALUE1));

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

    #region 手配明細(行レベルロック)

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dr">[手配連携No]DataRow</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/21</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable LockTehaiMeisai(DatabaseHelper dbHelper, DataRow dr)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEHAI_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       T_TEHAI_MEISAI.TEHAI_RENKEI_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEHAI_MEISAI");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdN(" WHERE T_TEHAI_MEISAI.TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO", ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO)));

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

    #region SKS手配明細WORK取得

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細WORK取得(スキーマ)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTehaiSKSWorkScheme(DatabaseHelper dbHelper)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEHAI_SKS_WORK.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       *");
            sb.ApdL("     , '' AS HACCHU_ZYOTAI_NAME");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEHAI_SKS_WORK");
            sb.ApdL(" WHERE 1 = 0");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細WORK取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/21</create>
    /// <update>T.Nukaga 2021/12/02 Step14 発注フラグ/受入日/検収日追加</update>
    /// --------------------------------------------------
    public DataTable GetTehaiSKSWork(DatabaseHelper dbHelper)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEHAI_SKS_WORK.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       T_TEHAI_SKS_WORK.TEHAI_NO");
            sb.ApdL("     , T_TEHAI_SKS_WORK.TEHAI_QTY");
            sb.ApdL("     , T_TEHAI_SKS_WORK.TEHAI_UNIT_PRICE");
            sb.ApdL("     , T_TEHAI_SKS_WORK.KAITO_DATE");
            sb.ApdL("     , T_TEHAI_SKS_WORK.HACCHU_ZYOTAI_FLAG");
            sb.ApdL("     , T_TEHAI_SKS_WORK.HACCHU_FLAG");
            sb.ApdL("     , T_TEHAI_SKS_WORK.UKEIRE_DATE");
            sb.ApdL("     , T_TEHAI_SKS_WORK.KENSHU_DATE");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEHAI_SKS_WORK");
            sb.ApdL(" WHERE EXISTS (");
            sb.ApdL("     SELECT 1");
            sb.ApdL("       FROM T_TEHAI_SKS");
            sb.ApdL("      WHERE T_TEHAI_SKS.TEHAI_NO = T_TEHAI_SKS_WORK.TEHAI_NO");
            sb.ApdL(" )");

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

    #endregion SELECT

    #region INSERT

    #region SKS手配明細WORKデータ追加

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細WORKデータ追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <param name="dr">SKS手配明細WORKデータ</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/20</create>
    /// <update>H.Tsuji 2020/06/17 入荷検品エラー Error出力</update>
    /// <update>T.Nukaga 2021/12/02 Step14 発注フラグ/受入日/検収日追加</update>
    /// <update>J.Chen 2023/02/09 Step15 見積・仮受入完納を見積状態に変更（単価取り込まれないように修正）</update>
    /// --------------------------------------------------
    public int InsTehaiSKSWorkExec(DatabaseHelper dbHelper, CondJ01 cond, DataRow dr)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;
            DbParamCollection paramCollection = new DbParamCollection();

            sb.ApdL("INSERT INTO T_TEHAI_SKS_WORK");
            sb.ApdL("(");
            sb.ApdL("       SEIBAN_CODE");
            sb.ApdL("     , HINBAN");
            sb.ApdL("     , PDM_WORK_NAME");
            sb.ApdL("     , TEHAI_NO");
            sb.ApdL("     , TEHAI_QTY");
            sb.ApdL("     , KATASHIKI");
            sb.ApdL("     , KAITO_DATE");
            sb.ApdL("     , TEHAI_UNIT_PRICE");
            sb.ApdL("     , ECS_NO");
            sb.ApdL("     , ZUMEN_OIBAN");
            sb.ApdL("     , HACCHU_ZYOTAI_FLAG");
            sb.ApdL("     , ZUMEN_KEISHIKI_S");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CUSTOMER_NAME");
            sb.ApdL("     , NONYUBASHO");
            sb.ApdL("     , CHUMONSHO_HINMOKU");
            sb.ApdL("     , HACCHU_FLAG");
            sb.ApdL("     , UKEIRE_DATE");
            sb.ApdL("     , KENSHU_DATE");
            sb.ApdL(")");
            sb.ApdL("SELECT");
            sb.ApdL("       SEIBAN_CODE");
            sb.ApdL("     , HINBAN");
            sb.ApdL("     , PDM_WORK_NAME");
            sb.ApdL("     , TEHAI_NO");
            sb.ApdL("     , TEHAI_QTY");
            sb.ApdL("     , KATASHIKI");
            sb.ApdL("     , KAITO_DATE");
            sb.ApdL("     , TEHAI_UNIT_PRICE");
            sb.ApdL("     , ECS_NO");
            sb.ApdL("     , ZUMEN_OIBAN");
            // 仮受入完納の場合、単価を取り込まれないように変更します。
            sb.ApdL("     , CASE WHEN MAIN.HACCHU_ZYOTAI_NAME = '見積・仮受入完納'");
            sb.ApdN("            THEN ").ApdL(HACCHU_FLAG.STATUS_1_VALUE1);
            sb.ApdL("            WHEN COM1.VALUE1 IS NULL");
            sb.ApdN("            THEN ").ApdN(this.BindPrefix).ApdL("HACCHU_ZYOTAI_FLAG_OTHER");
            sb.ApdL("       ELSE COM1.VALUE3 END");
            sb.ApdL("     , ZUMEN_KEISHIKI_S");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdL("     , CUSTOMER_NAME");
            sb.ApdL("     , NONYUBASHO");
            sb.ApdL("     , CHUMONSHO_HINMOKU");
            // 仮受入完納の場合、単価を取り込まれないように変更します。
            sb.ApdL("     , CASE WHEN MAIN.HACCHU_ZYOTAI_NAME = '見積・仮受入完納'");
            sb.ApdN("            THEN ").ApdL(HACCHU_FLAG.STATUS_1_VALUE1);
            sb.ApdL("            WHEN COM1.VALUE1 IS NULL");
            sb.ApdN("            THEN ").ApdN(this.BindPrefix).ApdL("HACCHU_ZYOTAI_FLAG_OTHER");
            sb.ApdL("       ELSE COM1.VALUE1 END");
            sb.ApdL("     , UKEIRE_DATE");
            sb.ApdL("     , KENSHU_DATE");
            sb.ApdL("  FROM (");
            sb.ApdL("    SELECT");
            sb.ApdN("          ").ApdN(this.BindPrefix).ApdL("HACCHU_ZYOTAI_NAME AS HACCHU_ZYOTAI_NAME");
            sb.ApdN("        , ").ApdN(this.BindPrefix).ApdL("SEIBAN_CODE AS SEIBAN_CODE");
            sb.ApdN("        , ").ApdN(this.BindPrefix).ApdL("HINBAN AS HINBAN");
            sb.ApdN("        , ").ApdN(this.BindPrefix).ApdL("PDM_WORK_NAME AS PDM_WORK_NAME");
            sb.ApdN("        , ").ApdN(this.BindPrefix).ApdL("TEHAI_NO AS TEHAI_NO");
            sb.ApdN("        , ").ApdN(this.BindPrefix).ApdL("TEHAI_QTY AS TEHAI_QTY");
            sb.ApdN("        , ").ApdN(this.BindPrefix).ApdL("KATASHIKI AS KATASHIKI");
            sb.ApdN("        , ").ApdN(this.BindPrefix).ApdL("KAITO_DATE AS KAITO_DATE");
            sb.ApdN("        , ").ApdN(this.BindPrefix).ApdL("TEHAI_UNIT_PRICE AS TEHAI_UNIT_PRICE");
            sb.ApdN("        , ").ApdN(this.BindPrefix).ApdL("ECS_NO AS ECS_NO");
            sb.ApdN("        , ").ApdN(this.BindPrefix).ApdL("ZUMEN_OIBAN AS ZUMEN_OIBAN");
            sb.ApdN("        , ").ApdN(this.BindPrefix).ApdL("HACCHU_ZYOTAI_FLAG AS HACCHU_ZYOTAI_FLAG");
            sb.ApdN("        , ").ApdN(this.BindPrefix).ApdL("ZUMEN_KEISHIKI_S AS ZUMEN_KEISHIKI_S");
            sb.ApdN("        , ").ApdN(this.BindPrefix).ApdL("CUSTOMER_NAME AS CUSTOMER_NAME");
            sb.ApdN("        , ").ApdN(this.BindPrefix).ApdL("NONYUBASHO AS NONYUBASHO");
            sb.ApdN("        , ").ApdN(this.BindPrefix).ApdL("CHUMONSHO_HINMOKU AS CHUMONSHO_HINMOKU");
            sb.ApdN("        , ").ApdN(this.BindPrefix).ApdL("HACCHU_FLAG AS HACCHU_FLAG");
            sb.ApdN("        , ").ApdN(this.BindPrefix).ApdL("UKEIRE_DATE AS UKEIRE_DATE");
            sb.ApdN("        , ").ApdN(this.BindPrefix).ApdL("KENSHU_DATE AS KENSHU_DATE");
            sb.ApdL("  ) MAIN");
            sb.ApdL("  LEFT JOIN M_COMMON COM1");
            sb.ApdN("         ON COM1.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("GC_HACCHU_FLAG");
            sb.ApdL("        AND COM1.VALUE2 = MAIN.HACCHU_ZYOTAI_NAME");
            sb.ApdN("        AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("HACCHU_ZYOTAI_FLAG_OTHER", HACCHU_FLAG.STATUS_0_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("HACCHU_ZYOTAI_NAME", ComFunc.GetFldObject(dr, ComDefine.FLD_HACCHU_ZYOTAI_NAME)));
            paramCollection.Add(iNewParam.NewDbParameter("SEIBAN_CODE", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.SEIBAN_CODE)));
            paramCollection.Add(iNewParam.NewDbParameter("HINBAN", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.HINBAN)));
            paramCollection.Add(iNewParam.NewDbParameter("PDM_WORK_NAME", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.PDM_WORK_NAME)));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NO", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.TEHAI_NO)));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_QTY", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.TEHAI_QTY)));
            paramCollection.Add(iNewParam.NewDbParameter("KATASHIKI", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.KATASHIKI)));
            paramCollection.Add(iNewParam.NewDbParameter("KAITO_DATE", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.KAITO_DATE)));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_UNIT_PRICE", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.TEHAI_UNIT_PRICE)));
            paramCollection.Add(iNewParam.NewDbParameter("ECS_NO", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.ECS_NO)));
            paramCollection.Add(iNewParam.NewDbParameter("ZUMEN_OIBAN", this.GetFldObjectEx(dr, Def_T_TEHAI_SKS_WORK.ZUMEN_OIBAN)));
            paramCollection.Add(iNewParam.NewDbParameter("HACCHU_ZYOTAI_FLAG", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.HACCHU_ZYOTAI_FLAG)));
            paramCollection.Add(iNewParam.NewDbParameter("ZUMEN_KEISHIKI_S", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.ZUMEN_KEISHIKI_S)));
            paramCollection.Add(iNewParam.NewDbParameter("CUSTOMER_NAME", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.CUSTOMER_NAME)));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUBASHO", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.NONYUBASHO)));
            paramCollection.Add(iNewParam.NewDbParameter("CHUMONSHO_HINMOKU", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.CHUMONSHO_HINMOKU)));
            paramCollection.Add(iNewParam.NewDbParameter("GC_HACCHU_FLAG", HACCHU_FLAG.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.Lang));
            paramCollection.Add(iNewParam.NewDbParameter("HACCHU_FLAG", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.HACCHU_FLAG)));
            paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_DATE", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.UKEIRE_DATE)));
            paramCollection.Add(iNewParam.NewDbParameter("KENSHU_DATE", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.KENSHU_DATE)));

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

    #region SKS手配明細データ追加

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細データ追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dr">SKS手配明細WORKデータ</param>
    /// <param name="serverTime">サーバー日時</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/21</create>
    /// <update>K.Tsutsumi 2019/02/11 SQLエラー修正</update>
    /// <update>K.tsutsumi 2020/05/15 技連No.10桁対応</update>
    /// <update>K.Tsutsumi 2020/11/18 EFA_SMS-158 T_TEHAI_SKS_WORK.KATASHIKI 格納領域拡張</update>
    /// <update>T.Nukaga 2021/12/02 Step14 発注フラグ/受入日/検収日追加</update>
    /// <update>T.Nukaga 2021/12/09 Step14 型式未設定の場合に空文字を設定するように変更</update>
    /// <update>D.Okumura 2022/01/04 Step14 前回発注フラグを空白設定するように変更(NULLだと、単価が反映されない)</update>
    /// <update>D.Okumura 2022/01/20 Step14 前回発注フラグに見積を設定するように変更(新規連携時に単価が反映されない)</update>
    /// --------------------------------------------------
    public int InsTehaiSKS(DatabaseHelper dbHelper, DataRow dr, DateTime serverTime)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;
            DbParamCollection paramCollection = new DbParamCollection();

            sb.ApdL("INSERT INTO T_TEHAI_SKS");
            sb.ApdL("(");
            sb.ApdL("       TEHAI_NO");
            sb.ApdL("     , TEHAI_QTY");
            sb.ApdL("     , TEHAI_UNIT_PRICE");
            sb.ApdL("     , KAITO_DATE");
            sb.ApdL("     , SKS_UPDATE_DATE");
            sb.ApdL("     , ZUMEN_OIBAN");
            sb.ApdL("     , HACCHU_ZYOTAI_FLAG");
            sb.ApdL("     , SEIBAN_CODE");
            sb.ApdL("     , HINBAN");
            sb.ApdL("     , PDM_WORK_NAME");
            sb.ApdL("     , KATASHIKI");
            sb.ApdL("     , ECS_NO");
            sb.ApdL("     , KENPIN_UMU");
            sb.ApdL("     , ARRIVAL_QTY");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL("     , HACCHU_FLAG");
            sb.ApdL("     , UKEIRE_DATE");
            sb.ApdL("     , KENSHU_DATE");
            sb.ApdL("     , ZENKAI_HACCHU_FLAG");
            sb.ApdL(") VALUES (");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("TEHAI_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("TEHAI_QTY");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("TEHAI_UNIT_PRICE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("KAITO_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SKS_UPDATE_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("ZUMEN_OIBAN");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("HACCHU_ZYOTAI_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SEIBAN_CODE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("HINBAN");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PDM_WORK_NAME");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("KATASHIKI");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("ECS_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("KENPIN_UMU");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("ARRIVAL_QTY");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdL("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("HACCHU_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UKEIRE_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("KENSHU_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("ZENKAI_HACCHU_FLAG");
            sb.ApdL(")");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NO", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.TEHAI_NO)));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_QTY", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.TEHAI_QTY)));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_UNIT_PRICE", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.TEHAI_UNIT_PRICE)));
            paramCollection.Add(iNewParam.NewDbParameter("KAITO_DATE", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.KAITO_DATE)));
            paramCollection.Add(iNewParam.NewDbParameter("SKS_UPDATE_DATE", serverTime.ToString("yyyy/MM/dd HH:mm")));
            paramCollection.Add(iNewParam.NewDbParameter("ZUMEN_OIBAN", this.GetFldObjectEx(dr, Def_T_TEHAI_SKS_WORK.ZUMEN_OIBAN)));
            paramCollection.Add(iNewParam.NewDbParameter("HACCHU_ZYOTAI_FLAG", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.HACCHU_ZYOTAI_FLAG)));
            paramCollection.Add(iNewParam.NewDbParameter("SEIBAN_CODE", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.SEIBAN_CODE)));
            paramCollection.Add(iNewParam.NewDbParameter("HINBAN", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.HINBAN)));
            paramCollection.Add(iNewParam.NewDbParameter("PDM_WORK_NAME", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.PDM_WORK_NAME)));
            // SKS側の型式は、40文字制限であるため、T_TEHAI_SKS_WORK.KATASHIKIは、NVARCHAR(40)へ拡張した。
            // SMS側の型式は、30文字制限であるため、調整する。
            var katashiki = ComFunc.GetFld(dr, Def_T_TEHAI_SKS_WORK.KATASHIKI);
            string[] arKatashiki = ComFunc.DivideStringEx(katashiki, ComDefine.MAX_BYTE_LENGTH_ZUMEN_KEISHIKI);
            paramCollection.Add(iNewParam.NewDbParameter("KATASHIKI", arKatashiki[0] == null ? string.Empty : arKatashiki[0]));
            
            paramCollection.Add(iNewParam.NewDbParameter("ECS_NO", ComFunc.GetFld(dr, Def_T_TEHAI_SKS_WORK.ECS_NO)));

            paramCollection.Add(iNewParam.NewDbParameter("KENPIN_UMU", KENPIN_UMU.ON_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("ARRIVAL_QTY", 0M));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_ID", ComDefine.SKS_RENKEI_USER_ID));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", ComDefine.SKS_RENKEI_USER_NAME));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", ComDefine.SKS_RENKEI_USER_ID));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", ComDefine.SKS_RENKEI_USER_NAME));
            paramCollection.Add(iNewParam.NewDbParameter("HACCHU_FLAG", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.HACCHU_FLAG)));
            paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_DATE", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.UKEIRE_DATE)));
            paramCollection.Add(iNewParam.NewDbParameter("KENSHU_DATE", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.KENSHU_DATE)));
            paramCollection.Add(iNewParam.NewDbParameter("ZENKAI_HACCHU_FLAG", HACCHU_FLAG.STATUS_1_VALUE1)); // 見積

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

    #region 手配SKS連携データ追加

    /// --------------------------------------------------
    /// <summary>
    /// 手配SKS連携データ追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dr">SKS手配明細WORKデータ</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/21</create>
    /// <update>K.Tsutsumi 2019/02/11 SQLエラー修正</update>
    /// <update>J.Chen 2022/11/17 既に存在した場合UPDATEにする</update>
    /// --------------------------------------------------
    public int InsTehaiMeisaiSKS(DatabaseHelper dbHelper, DataRow dr)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;
            DbParamCollection paramCollection = new DbParamCollection();

            //sb.ApdL("INSERT INTO T_TEHAI_MEISAI_SKS");
            //sb.ApdL("(");
            //sb.ApdL("       TEHAI_RENKEI_NO");
            //sb.ApdL("     , TEHAI_NO");
            //sb.ApdL(") VALUES (");
            //sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO");
            //sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("TEHAI_NO");
            //sb.ApdL(")");

            sb.ApdL(" MERGE INTO T_TEHAI_MEISAI_SKS AS A");
            sb.ApdL(" USING (SELECT ");
            sb.ApdN("           ").ApdN(BindPrefix).ApdN("TEHAI_RENKEI_NO").ApdL(" AS TEHAI_RENKEI_NO");
            sb.ApdN("         , ").ApdN(BindPrefix).ApdN("TEHAI_NO").ApdL(" AS TEHAI_NO ) AS B");
            sb.ApdL("    ON A.TEHAI_RENKEI_NO = B.TEHAI_RENKEI_NO");
            sb.ApdL("   AND A.TEHAI_NO = B.TEHAI_NO");
            sb.ApdL("  WHEN MATCHED THEN");
            sb.ApdL("    UPDATE");
            sb.ApdL("    SET TEHAI_RENKEI_NO = B.TEHAI_RENKEI_NO");
            sb.ApdN("      , TEHAI_NO = B.TEHAI_NO");
            sb.ApdL("  WHEN NOT MATCHED THEN");
            sb.ApdL("    INSERT (");
            sb.ApdL("       TEHAI_RENKEI_NO");
            sb.ApdL("      ,TEHAI_NO");
            sb.ApdL("    ) VALUES (");
            sb.ApdL("       B.TEHAI_RENKEI_NO");
            sb.ApdL("      ,B.TEHAI_NO");
            sb.ApdL("    )");
            sb.ApdL(";");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO)));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NO", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI_SKS.TEHAI_NO)));

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

    #region SKS連携マスタ更新処理

    /// --------------------------------------------------
    /// <summary>
    /// SKS連携マスタ更新処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tajimi 2018/11/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdSKS(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE M_SKS");
            sb.ApdL("SET");
            sb.ApdN("       SHORI_FLAG = ").ApdN(this.BindPrefix).ApdL("SHORI_FLAG");
            if (cond.LastestDate != null)
            {
                sb.ApdN("     , LASTEST_DATE = ").ApdN(this.BindPrefix).ApdL("LASTEST_DATE");
                paramCollection.Add(iNewParam.NewDbParameter("LASTEST_DATE", cond.LastestDate));
            }

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHORI_FLAG", cond.ShoriFlag));

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

    #region SKS手配明細WORK更新処理

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細WORK更新処理(回答納期=完納)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tajimi 2018/11/20</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdSKSTehaiWorkHacchuZyotai(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_TEHAI_SKS_WORK");
            sb.ApdL("SET");
            sb.ApdL("       T_TEHAI_SKS_WORK.HACCHU_ZYOTAI_FLAG = M_COMMON.VALUE3");
            sb.ApdL("  FROM T_TEHAI_SKS_WORK");
            sb.ApdL("  LEFT JOIN M_COMMON");
            sb.ApdN("         ON M_COMMON.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("GROUP_CD_HACCHU_FLAG");
            sb.ApdN("        AND M_COMMON.VALUE1 = ").ApdN(this.BindPrefix).ApdL("STATUS_2_VALUE1");
            sb.ApdN("        AND M_COMMON.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdN(" WHERE T_TEHAI_SKS_WORK.KAITO_DATE = ").ApdN(this.BindPrefix).ApdL("KAITO_DATE_KANNOU");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("GROUP_CD_HACCHU_FLAG", HACCHU_FLAG.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("STATUS_2_VALUE1", HACCHU_FLAG.STATUS_2_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.Lang));
            paramCollection.Add(iNewParam.NewDbParameter("KAITO_DATE_KANNOU", ComDefine.KAITO_DATE_KANNOU));

            // SQL実行
            record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            return record;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細WORK更新処理(手配単価更新)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tajimi 2018/11/20</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdSKSTehaiWorkTehaiUnitPrice(DatabaseHelper dbHelper, CondJ01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_TEHAI_SKS_WORK");
            sb.ApdL("SET");
            sb.ApdL("       T_TEHAI_SKS_WORK.TEHAI_UNIT_PRICE = 0");
            sb.ApdN(" WHERE T_TEHAI_SKS_WORK.HACCHU_ZYOTAI_FLAG = ").ApdN(this.BindPrefix).ApdL("HACCHU_ZYOTAI_MITSUMORI");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("HACCHU_ZYOTAI_MITSUMORI", HACCHU_FLAG.STATUS_1_VALUE1));

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

    #region SKS手配明細更新処理

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細更新処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dr">DataRow</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/21</create>
    /// <update>T.Nukaga 2021/12/02 Step14 発注フラグ/受入日/検収日追加</update>
    /// --------------------------------------------------
    public int UpdTehaiSKS(DatabaseHelper dbHelper, DataRow dr)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_TEHAI_SKS");
            sb.ApdL("SET");
            sb.ApdN("       TEHAI_QTY = ").ApdN(this.BindPrefix).ApdL("TEHAI_QTY");
            sb.ApdN("     , TEHAI_UNIT_PRICE = ").ApdN(this.BindPrefix).ApdL("TEHAI_UNIT_PRICE");
            sb.ApdN("     , KAITO_DATE = ").ApdN(this.BindPrefix).ApdL("KAITO_DATE");
            sb.ApdN("     , HACCHU_ZYOTAI_FLAG = ").ApdN(this.BindPrefix).ApdL("HACCHU_ZYOTAI_FLAG");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , HACCHU_FLAG = ").ApdN(this.BindPrefix).ApdL("HACCHU_FLAG");
            sb.ApdN("     , UKEIRE_DATE = ").ApdN(this.BindPrefix).ApdL("UKEIRE_DATE");
            sb.ApdN("     , KENSHU_DATE = ").ApdN(this.BindPrefix).ApdL("KENSHU_DATE");
            sb.ApdN("     , ZENKAI_HACCHU_FLAG = HACCHU_FLAG");
            sb.ApdN(" WHERE TEHAI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_QTY", ComFunc.GetFld(dr, Def_T_TEHAI_SKS_WORK.TEHAI_QTY)));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_UNIT_PRICE", ComFunc.GetFld(dr, Def_T_TEHAI_SKS_WORK.TEHAI_UNIT_PRICE)));
            paramCollection.Add(iNewParam.NewDbParameter("KAITO_DATE", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS_WORK.KAITO_DATE)));
            paramCollection.Add(iNewParam.NewDbParameter("HACCHU_ZYOTAI_FLAG", ComFunc.GetFld(dr, Def_T_TEHAI_SKS_WORK.HACCHU_ZYOTAI_FLAG)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", ComDefine.SKS_RENKEI_USER_ID));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", ComDefine.SKS_RENKEI_USER_NAME));
            paramCollection.Add(iNewParam.NewDbParameter("HACCHU_FLAG", ComFunc.GetFld(dr, Def_T_TEHAI_SKS_WORK.HACCHU_FLAG)));
            paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_DATE", ComFunc.GetFld(dr, Def_T_TEHAI_SKS_WORK.UKEIRE_DATE)));
            paramCollection.Add(iNewParam.NewDbParameter("KENSHU_DATE", ComFunc.GetFld(dr, Def_T_TEHAI_SKS_WORK.KENSHU_DATE)));

            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NO", ComFunc.GetFld(dr, Def_T_TEHAI_SKS_WORK.TEHAI_NO)));

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

    #region 手配明細更新処理(手配種別)

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細更新処理(手配種別)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dr">DataRow</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/21</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdTehaiMeisaiTehaiKindFlag(DatabaseHelper dbHelper, DataRow dr)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_TEHAI_MEISAI");
            sb.ApdL("SET");
            sb.ApdN("       TEHAI_KIND_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_FLAG");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdN(" WHERE TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_KIND_FLAG", TEHAI_KIND_FLAG.ANOTHER_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", ComDefine.SKS_RENKEI_USER_ID));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", ComDefine.SKS_RENKEI_USER_NAME));

            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO", ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO)));

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

    #region 手配明細更新処理(単価(JPY) [その他])

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細更新処理(単価(JPY) [その他])
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dr">DataRow</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/21</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdTehaiMeisaiUnitPriceForAnother(DatabaseHelper dbHelper, DataRow dr)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_TEHAI_MEISAI");
            sb.ApdL("SET");
            sb.ApdL("       UNIT_PRICE = (SELECT MAX(T_TEHAI_SKS.TEHAI_UNIT_PRICE)");
            sb.ApdL("                       FROM T_TEHAI_SKS");
            sb.ApdL("                      INNER JOIN T_TEHAI_MEISAI_SKS");
            sb.ApdL("                              ON T_TEHAI_SKS.TEHAI_NO = T_TEHAI_MEISAI_SKS.TEHAI_NO");
            sb.ApdN("                             AND T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO)");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdN(" WHERE TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", ComDefine.SKS_RENKEI_USER_ID));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", ComDefine.SKS_RENKEI_USER_NAME));

            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO", ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO)));

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

    #region 手配明細更新処理(単価(JPY) [わたり発注])

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細更新処理(単価(JPY) [わたり発注])
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dr">DataRow</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/21</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdTehaiMeisaiUnitPriceForAcross(DatabaseHelper dbHelper, DataRow dr)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_TEHAI_MEISAI");
            sb.ApdL("SET");
            sb.ApdL("       UNIT_PRICE = (SELECT SUM(T_TEHAI_SKS.TEHAI_UNIT_PRICE)");
            sb.ApdL("                       FROM T_TEHAI_SKS");
            sb.ApdL("                      INNER JOIN T_TEHAI_MEISAI_SKS");
            sb.ApdL("                              ON T_TEHAI_SKS.TEHAI_NO = T_TEHAI_MEISAI_SKS.TEHAI_NO");
            sb.ApdN("                             AND T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO)");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdN(" WHERE TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", ComDefine.SKS_RENKEI_USER_ID));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", ComDefine.SKS_RENKEI_USER_NAME));

            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO", ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO)));

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

    #region DELETE

    #region SKS手配明細WORK削除処理

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細WORK削除処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public int TruncateTehaiSKSWork(DatabaseHelper dbHelper)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("TRUNCATE TABLE T_TEHAI_SKS_WORK");

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

    #region エラーメッセージスキーマ取得

    /// --------------------------------------------------
    /// <summary>
    /// エラーメッセージスキーマ取得
    /// </summary>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/22</create>
    /// <update></update>
    /// --------------------------------------------------
    internal DataTable GetErrorMessageScheme()
    {
        var dtMessage = new DataTable(ComDefine.DTTBL_RESULT);
        dtMessage.Columns.Add(ComDefine.FLD_RESULT);
        return dtMessage;
    }

    #endregion

    #region エラーメッセージ追加

    /// --------------------------------------------------
    /// <summary>
    /// エラーメッセージ追加
    /// </summary>
    /// <param name="dt">DataTable</param>
    /// <param name="message">メッセージ文字列</param>
    /// <param name="args">メッセージ引数</param>
    /// <create>H.Tajimi 2018/11/22</create>
    /// <update></update>
    /// --------------------------------------------------
    internal void AddErrorMessage(DataTable dt, string message, params string[] args)
    {
        var dr = dt.NewRow();
        dr[ComDefine.FLD_RESULT] = string.Format(message, args);
        dt.Rows.Add(dr);
    }

    #endregion

    #region GetFldObjectEx

    /// --------------------------------------------------
    /// <summary>
    /// データロウアクセス
    /// ※空の場合にDBNull.Valueを返却する
    /// </summary>
    /// <param name="dr">データロウ</param>
    /// <param name="columnName">列名</param>
    /// <returns>オブジェクト</returns>
    /// <create>H.Tajimi 2018/11/27</create>
    /// <update></update>
    /// --------------------------------------------------
    internal object GetFldObjectEx(DataRow dr, string columnName)
    {
        object defaultValue = DBNull.Value;
        try
        {
            if (dr == null)
            {
                return defaultValue;
            }
            if (dr[columnName] == DBNull.Value)
            {
                return defaultValue;
            }
            if (string.IsNullOrEmpty(ComFunc.GetFld(dr, columnName)))
            {
                return defaultValue;
            }
            return dr[columnName];
        }
        catch
        {
            return defaultValue;
        }
    }

    #endregion
}
