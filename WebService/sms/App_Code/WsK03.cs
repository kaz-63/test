using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Services;

using Condition;
using Commons;
using DSWUtil.DbUtil;

/// --------------------------------------------------
/// <summary>
/// 木枠作成処理クラス（トランザクション層）
/// </summary>
/// <create>Y.Higuchi 2010/06/22</create>
/// <update></update>
/// --------------------------------------------------
[WebService(Description = "出荷管理システム", Namespace = "http://smssrv/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。 
// [System.Web.Script.Services.ScriptService]
public class WsK03 : WsBase
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/06/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsK03()
        : base()
    {
    }

    #endregion

    #region K0300010:木枠登録

    #region 木枠取得チェック

    /// --------------------------------------------------
    /// <summary>
    /// 木枠チェック
    /// </summary>
    /// <param name="cond">K03用コンディション</param>
    /// <returns>true:存在する false:存在しない</returns>
    /// <create>M.Tsutsumi 2010/07/26</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "木枠取得チェック")]
    public bool CheckExistenceKiwaku(CondK03 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                bool ret = false;
                using (WsK03Impl impl = new WsK03Impl())
                {
                    ret = impl.IsExistenceKiwaku(dbHelper, cond);
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

    #region 木枠取得

    /// --------------------------------------------------
    /// <summary>
    /// 木枠データ取得
    /// </summary>
    /// <param name="cond">K03用コンディション</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>データセット</returns>
    /// <create>M.Tsutsumi 2010/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "木枠データ取得")]
    public DataSet GetKiwaku(CondK03 cond, out string errMsgID, out string[] args)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                errMsgID = string.Empty;
                args = null;

                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds = null;
                using (WsK03Impl impl = new WsK03Impl())
                {
                    ds = impl.GetKiwaku(dbHelper, cond, ref errMsgID, ref args);
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

    #region 木枠登録

    /// --------------------------------------------------
    /// <summary>
    /// 木枠データ登録
    /// </summary>
    /// <param name="cond">K03用コンディション</param>
    /// <param name="ds">データセット</param>
    /// <param name="kojiNo">工事識別管理No</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>M.Tsutsumi 2010/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "木枠データ登録")]
    public bool InsKiwaku(CondK03 cond, DataSet ds, out string kojiNo, out string errMsgID, out string[] args)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                kojiNo = string.Empty;
                errMsgID = string.Empty;
                args = null;

                // 接続
                this.DbOpen(dbHelper);

                // トランザクション開始
                dbHelper.BeginTransaction();

                // 実行
                bool ret = false;
                using (WsK03Impl impl = new WsK03Impl())
                {
                    ret = impl.InsKiwaku(dbHelper, cond, ds, ref kojiNo, ref errMsgID, ref args);
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

    #region 木枠更新

    /// --------------------------------------------------
    /// <summary>
    /// 木枠データ更新
    /// </summary>
    /// <param name="cond">K03用コンディション</param>
    /// <param name="ds">データセット</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>M.Tsutsumi 2010/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "木枠データ更新")]
    public bool UpdKiwaku(CondK03 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (WsK03Impl impl = new WsK03Impl())
                {
                    ret = impl.UpdKiwaku(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region 木枠削除

    /// --------------------------------------------------
    /// <summary>
    /// 木枠データ削除
    /// </summary>
    /// <param name="cond">K03用コンディション</param>
    /// <param name="ds">データセット</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>M.Tsutsumi 2010/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "木枠データ削除")]
    public bool DelKiwaku(CondK03 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (WsK03Impl impl = new WsK03Impl())
                {
                    ret = impl.DelKiwaku(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region K0300020:木枠登録(社外)
    #endregion

}

