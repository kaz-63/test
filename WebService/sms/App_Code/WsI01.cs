using System;
using System.Data;
using System.Web.Services;
using Commons;
using Condition;
using DSWUtil.DbUtil;

/// --------------------------------------------------
/// <summary>
/// 現地部品管理処理クラス（トランザクション層）
/// </summary>
/// <create>T.Wakamatsu 2013/07/29</create>
/// <update></update>
/// --------------------------------------------------
[WebService(Description = "出荷管理システム", Namespace = "http://smssrv/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。 
// [System.Web.Script.Services.ScriptService]
public class WsI01 : WsBase
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>T.Wakamatsu 2013/07/29</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsI01()
        : base()
    {
    }

    #endregion

    #region I01:共通処理

    #region ロケーションコンボボックス用データ取得

    /// --------------------------------------------------
    /// <summary>
    /// ロケーションコンボボックス用データ取得
    /// </summary>
    /// <param name="cond">I01用コンディション</param>
    /// <returns>ロケーションマスタ</returns>
    /// <create>T.Wakamatsu 2013/07/31</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ロケーションコンボボックス用データ取得")]
    public DataSet GetLocationCombo(CondI01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsI01Impl impl = new WsI01Impl())
                {
                    ds = impl.GetLocationCombo(dbHelper, cond);
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                // 切断
                dbHelper.Close();
            }
        }
    }

    #endregion

    #endregion

    #region I0100010:ロケーションマスタ

    #region ロケーション取得(完全一致・物件管理No、出荷区分、ロケーション必須)

    /// --------------------------------------------------
    /// <summary>
    /// ロケーション取得(完全一致・物件管理No、出荷区分、ロケーション必須)
    /// </summary>
    /// <param name="cond">I01用コンディション</param>
    /// <returns>ロケーションマスタ</returns>
    /// <create>T.Wakamatsu 2013/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ロケーション取得(完全一致・物件管理No、出荷区分、ロケーション必須)")]
    public DataSet GetLocation(CondI01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsI01Impl impl = new WsI01Impl())
                {
                    ds = impl.GetLocation(dbHelper, cond);
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                // 切断
                dbHelper.Close();
            }
        }
    }

    #endregion

    #region ロケーション取得(あいまい検索)

    /// --------------------------------------------------
    /// <summary>
    /// ロケーション取得(あいまい検索)
    /// </summary>
    /// <param name="cond">I01用コンディション</param>
    /// <returns>ロケーションマスタ</returns>
    /// <create>T.Wakamatsu 2013/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ロケーション取得(あいまい検索)")]
    public DataSet GetLocationLikeSearch(CondI01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsI01Impl impl = new WsI01Impl())
                {
                    ds = impl.GetLocationLikeSearch(dbHelper, cond);
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                // 切断
                dbHelper.Close();
            }
        }
    }


    #endregion

    #region ロケーションの追加

    /// --------------------------------------------------
    /// <summary>
    /// ロケーションの追加
    /// </summary>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Wakamatsu 2013/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ロケーションの追加")]
    public bool InsLocationData(CondI01 cond, DataSet ds, out string errMsgID, out string[] args)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                errMsgID = string.Empty;
                args = null;
                // 接続
                this.DbOpen(dbHelper);

                // トランザクション開始
                dbHelper.BeginTransaction();

                // 実行
                bool ret = false;
                DataTable dt = ds.Tables[Def_M_LOCATION.Name];
                using (WsI01Impl impl = new WsI01Impl())
                {
                    ret = impl.InsLocationData(dbHelper, cond, dt, ref errMsgID, ref args);
                }

                if (ret)
                {
                    dbHelper.Commit();
                }
                else
                {
                    dbHelper.Rollback();
                }

                return ret;
            }
            catch (Exception ex)
            {
                dbHelper.Rollback();
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                // 切断
                dbHelper.Close();
            }
        }
    }

    #endregion

    #region ロケーションの更新

    /// --------------------------------------------------
    /// <summary>
    /// ロケーションの更新
    /// </summary>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Wakamatsu 2013/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ロケーションの更新")]
    public bool UpdLocationData(CondI01 cond, DataSet ds, out string errMsgID, out string[] args)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                errMsgID = string.Empty;
                args = null;
                // 接続
                this.DbOpen(dbHelper);

                // トランザクション開始
                dbHelper.BeginTransaction();

                // 実行
                bool ret = false;
                DataTable dt = ds.Tables[Def_M_LOCATION.Name];
                using (WsI01Impl impl = new WsI01Impl())
                {
                    ret = impl.UpdLocationData(dbHelper, cond, dt, ref errMsgID, ref args);
                }

                if (ret)
                {
                    dbHelper.Commit();
                }
                else
                {
                    dbHelper.Rollback();
                }

                return ret;
            }
            catch (Exception ex)
            {
                dbHelper.Rollback();
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                // 切断
                dbHelper.Close();
            }
        }
    }

    #endregion

    #region ロケーションの削除

    /// --------------------------------------------------
    /// <summary>
    /// ロケーションの削除
    /// </summary>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Wakamatsu 2013/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ロケーションの削除")]
    public bool DelLocationData(CondI01 cond, DataSet ds, out string errMsgID, out string[] args)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                errMsgID = string.Empty;
                args = null;
                // 接続
                this.DbOpen(dbHelper);

                // トランザクション開始
                dbHelper.BeginTransaction();

                // 実行
                bool ret = false;
                DataTable dt = ds.Tables[Def_M_LOCATION.Name];
                using (WsI01Impl impl = new WsI01Impl())
                {
                    ret = impl.DelLocationData(dbHelper, cond, dt, ref errMsgID, ref args);
                }

                if (ret)
                {
                    dbHelper.Commit();
                }
                else
                {
                    dbHelper.Rollback();
                }

                return ret;
            }
            catch (Exception ex)
            {
                dbHelper.Rollback();
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                // 切断
                dbHelper.Close();
            }
        }
    }

    #endregion


    #endregion

    #region I0100020:入庫設定

    #region 出荷・在庫データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 出荷・在庫データ取得
    /// </summary>
    /// <param name="cond">I01用コンディション</param>
    /// <returns>出荷・在庫データ</returns>
    /// <create>T.Wakamatsu 2013/07/31</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "出荷・在庫データ取得")]
    public DataSet GetShukkaZaiko(CondI01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsI01Impl impl = new WsI01Impl())
                {
                    ds = impl.GetShukkaZaiko(dbHelper, cond);
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                // 切断
                dbHelper.Close();
            }
        }
    }

    #endregion

    #region 入庫処理

    /// --------------------------------------------------
    /// <summary>
    /// 入庫処理
    /// </summary>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Wakamatsu 2013/08/01</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "入庫処理")]
    public bool InsZaikoData(CondI01 cond, DataTable dt, out string errMsgID, out string[] args)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                errMsgID = string.Empty;
                args = null;
                // 接続
                this.DbOpen(dbHelper);

                // トランザクション開始
                dbHelper.BeginTransaction();

                // 実行
                bool ret = false;
                using (WsI01Impl impl = new WsI01Impl())
                {
                    ret = impl.InsZaikoData(dbHelper, cond, dt, ref errMsgID, ref args);
                }

                if (ret)
                {
                    dbHelper.Commit();
                }
                else
                {
                    dbHelper.Rollback();
                }

                return ret;
            }
            catch (Exception ex)
            {
                dbHelper.Rollback();
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                // 切断
                dbHelper.Close();
            }
        }
    }

    #endregion

    #endregion

    #region I0100030:出庫設定

    #region 完了データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 完了データ取得
    /// </summary>
    /// <param name="cond">I01用コンディション</param>
    /// <returns>在庫データ</returns>
    /// <create>T.Wakamatsu 2013/08/12</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "完了データ取得")]
    public DataSet GetKanryoData(CondI01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsI01Impl impl = new WsI01Impl())
                {
                    ds = impl.GetKanryoData(dbHelper, cond);
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                // 切断
                dbHelper.Close();
            }
        }
    }

    #endregion

    #region 完了処理

    /// --------------------------------------------------
    /// <summary>
    /// 完了処理
    /// </summary>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dt">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Wakamatsu 2013/08/12</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "完了処理")]
    public bool DelKanryoData(CondI01 cond, DataTable dt, out string errMsgID, out string[] args)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                errMsgID = string.Empty;
                args = null;
                // 接続
                this.DbOpen(dbHelper);

                // トランザクション開始
                dbHelper.BeginTransaction();

                // 実行
                bool ret = false;
                using (WsI01Impl impl = new WsI01Impl())
                {
                    ret = impl.DelKanryoData(dbHelper, cond, dt, ref errMsgID, ref args);
                }

                if (ret)
                {
                    dbHelper.Commit();
                }
                else
                {
                    dbHelper.Rollback();
                }

                return ret;
            }
            catch (Exception ex)
            {
                dbHelper.Rollback();
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                // 切断
                dbHelper.Close();
            }
        }
    }

    #endregion

    #endregion

    #region I0100040:棚卸差異照会

    #region 棚卸ロケーションコンボボックス用データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 棚卸ロケーションコンボボックス用データ取得
    /// </summary>
    /// <param name="cond">I01用コンディション</param>
    /// <returns>ロケーションマスタ</returns>
    /// <create>T.Wakamatsu 2013/10/02</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "棚卸ロケーションコンボボックス用データ取得")]
    public DataSet GetTanaoroshiLocationCombo(CondI01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsI01Impl impl = new WsI01Impl())
                {
                    ds = impl.GetTanaoroshiLocationCombo(dbHelper, cond);
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                // 切断
                dbHelper.Close();
            }
        }
    }

    #endregion

    #region 棚卸データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 棚卸データ取得
    /// </summary>
    /// <param name="cond">I01用コンディション</param>
    /// <returns>棚卸データ</returns>
    /// <create>T.Wakamatsu 2013/08/21</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "棚卸データ取得")]
    public DataSet GetTanaoroshiData(CondI01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsI01Impl impl = new WsI01Impl())
                {
                    ds = impl.GetTanaoroshiData(dbHelper, cond);
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                // 切断
                dbHelper.Close();
            }
        }
    }

    #endregion

    #region 棚卸処理

    /// --------------------------------------------------
    /// <summary>
    /// 棚卸処理
    /// </summary>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Wakamatsu 2013/08/21</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "棚卸処理")]
    public bool UpdTanaoroshiData(CondI01 cond, DataTable dt, out string errMsgID, out string[] args)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                errMsgID = string.Empty;
                args = null;
                // 接続
                this.DbOpen(dbHelper);

                // トランザクション開始
                dbHelper.BeginTransaction();

                // 実行
                bool ret = false;
                using (WsI01Impl impl = new WsI01Impl())
                {
                    ret = impl.UpdTanaoroshiData(dbHelper, cond, dt, ref errMsgID, ref args);
                }

                if (ret)
                {
                    dbHelper.Commit();
                }
                else
                {
                    dbHelper.Rollback();
                }

                return ret;
            }
            catch (Exception ex)
            {
                dbHelper.Rollback();
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                // 切断
                dbHelper.Close();
            }
        }
    }

    #endregion

    #endregion

    #region I0100050:在庫問合せ・メンテ

    #region 在庫メンテ権限取得

    /// --------------------------------------------------
    /// <summary>
    /// 在庫メンテ権限取得
    /// </summary>
    /// <param name="role">ユーザー権限</param>
    /// <returns>在庫メンテ権限</returns>
    /// <create>T.Wakamatsu 2013/09/19</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "在庫メンテ権限取得")]
    public bool GetZaikoMainteRole(string role)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                bool ret;
                using (WsI01Impl impl = new WsI01Impl())
                {
                    ret = impl.GetZaikoMainteRole(dbHelper, role);
                }
                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                // 切断
                dbHelper.Close();
            }
        }
    }

    #endregion

    #region 在庫データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 在庫データ取得
    /// </summary>
    /// <param name="cond">I01用コンディション</param>
    /// <returns>在庫データ</returns>
    /// <create>T.Wakamatsu 2013/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "在庫データ取得")]
    public DataSet GetZaikoHoshu(CondI01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsI01Impl impl = new WsI01Impl())
                {
                    ds = impl.GetZaikoHoshu(dbHelper, cond);
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                // 切断
                dbHelper.Close();
            }
        }
    }

    #endregion

    #region 在庫データ更新

    /// --------------------------------------------------
    /// <summary>
    /// 在庫データ更新
    /// </summary>
    /// <param name="cond">I01用コンディション</param>
    /// <param name="dtUpd">更新データ</param>
    /// <param name="dtDel">削除データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Wakamatsu 2013/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "在庫データ更新")]
    public bool UpdZaikoHoshu(CondI01 cond, DataTable dtUpd, DataTable dtDel, out string errMsgID, out string[] args)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                errMsgID = string.Empty;
                args = null;
                // 接続
                this.DbOpen(dbHelper);

                // トランザクション開始
                dbHelper.BeginTransaction();

                // 実行
                bool ret = false;
                using (WsI01Impl impl = new WsI01Impl())
                {
                    ret = impl.UpdZaikoHoshu(dbHelper, cond, dtUpd, dtDel, ref errMsgID, ref args);
                }

                if (ret)
                {
                    dbHelper.Commit();
                }
                else
                {
                    dbHelper.Rollback();
                }

                return ret;
            }
            catch (Exception ex)
            {
                dbHelper.Rollback();
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                // 切断
                dbHelper.Close();
            }
        }
    }

    #endregion

    #endregion

    #region I0100060:オペ履歴照会

    #region オペ履歴取得

    /// --------------------------------------------------
    /// <summary>
    /// オペ履歴取得
    /// </summary>
    /// <param name="cond">I01用コンディション</param>
    /// <returns>オペ履歴データ</returns>
    /// <create>T.Wakamatsu 2013/08/20</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "オペ履歴取得")]
    public DataSet GetJisseki(CondI01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsI01Impl impl = new WsI01Impl())
                {
                    ds = impl.GetJisseki(dbHelper, cond);
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                // 切断
                dbHelper.Close();
            }
        }
    }

    #endregion

    #endregion
}

