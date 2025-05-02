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
/// 共通ﾀﾞｲｱﾛｸﾞ処理クラス（トランザクション層）
/// </summary>
/// <create>Y.Higuchi 2010/06/22</create>
/// <update></update>
/// --------------------------------------------------
[WebService(Description = "出荷管理システム", Namespace = "http://smssrv/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。 
// [System.Web.Script.Services.ScriptService]
public class WsP02 : WsBase
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/06/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsP02()
        : base()
    {
    }

    #endregion

    #region P0200010:納入先一覧

    #region 納入先一覧取得

    /// --------------------------------------------------
    /// <summary>
    /// 納入先一覧取得
    /// </summary>
    /// <param name="cond">納入先マスタ用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>Y.Higuchi 2010/06/25</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "納入先一覧取得")]
    public DataSet GetNonyusakiIchiran(CondNonyusaki cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsP02Impl impl = new WsP02Impl())
                {
                    ds = impl.GetNonyusakiIchiran(dbHelper,cond);
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

    #region 履歴データに紐付く物件名一覧取得

    /// --------------------------------------------------
    /// <summary>
    /// 履歴データに紐付く物件名一覧取得
    /// </summary>
    /// <param name="cond">納入先マスタ用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/05/09</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "履歴データに紐付く物件名一覧取得")]
    public DataSet GetRirekiBukkenIchiran(CondNonyusaki cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsP02Impl impl = new WsP02Impl())
                {
                    ds = impl.GetRirekiBukkenIchiran(dbHelper, cond);
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

    #region P0200030:工事識別一覧

    #region 工事識別一覧取得

    /// --------------------------------------------------
    /// <summary>
    /// 工事識別一覧取得
    /// </summary>
    /// <param name="cond">木枠データ用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>Y.Higuchi 2010/06/25</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "納入先一覧取得")]
    public DataSet GetKojiShikibetsuIchiran(CondKiwaku cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsP02Impl impl = new WsP02Impl())
                {
                    ds = impl.GetKojiShikibetsuIchiran(dbHelper, cond);
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

    #region P0200040:物件名一覧

    #region 物件名一覧取得

    /// --------------------------------------------------
    /// <summary>
    /// 物件名一覧取得
    /// </summary>
    /// <param name="cond">物件データ用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>T.Sakiori 2012/04/05</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "物件名一覧取得")]
    public DataSet GetBukkenNameIchiran(CondBukken cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (var impl = new WsP02Impl())
                {
                    ds = impl.GetBukkenNameIchiran(dbHelper, cond);
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

    #region P0200050:履歴照会

    #region 処理名コンボボックスデータ取得

    /// --------------------------------------------------
    /// <summary>
    /// 処理名コンボボックスデータ取得
    /// </summary>
    /// <param name="cond">P02用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/04/16</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "処理名コンボボックスデータ取得")]
    public DataSet GetOperationFlag(CondP02 cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                DataSet ds;
                using (var impl = new WsP02Impl())
                {
                    ds = impl.GetOperationFlag(dbHelper, cond);
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

    #region 履歴一覧取得

    /// --------------------------------------------------
    /// <summary>
    /// 履歴一覧取得
    /// </summary>
    /// <param name="cond">P02用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/04/16</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "履歴一覧取得")]
    public DataSet GetRireki(CondP02 cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                DataSet ds;
                using (var impl = new WsP02Impl())
                {
                    ds = impl.GetRireki(dbHelper, cond);
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

    #region 締めマスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// 締めマスタ取得
    /// </summary>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/06/01</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "締めマスタ取得")]
    public DataSet GetShime()
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                DataSet ds;
                using (var impl = new WsP02Impl())
                {
                    ds = impl.GetShime(dbHelper);
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

    #region P0200060:送信先設定

    /// --------------------------------------------------
    /// <summary>
    /// 送信先取得
    /// </summary>
    /// <param name="cond">P02用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/14</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description="送信先設定")]
    public DataTable GetSendUser(CondP02 cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                using (var impl = new WsP02Impl())
                {
                    return impl.GetSendUser(dbHelper, cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                dbHelper.Close();
            }
        }
    }

    #endregion
}

