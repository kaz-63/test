using System;
using System.Data;
using System.Web.Services;
using Commons;
using Condition;
using DSWUtil.DbUtil;

/// --------------------------------------------------
/// <summary>
/// マスタ&保守処理クラス（トランザクション層）
/// </summary>
/// <create>Y.Higuchi 2010/06/23</create>
/// <update></update>
/// --------------------------------------------------
[WebService(Description = "出荷管理システム", Namespace = "http://smssrv/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。 
// [System.Web.Script.Services.ScriptService]
public class WsM01 : WsBase
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/06/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsM01()
        : base()
    {
    }

    #endregion

    #region ユーザーマスタ

    #region ユーザー取得(完全一致・ユーザーID必須)

    /// --------------------------------------------------
    /// <summary>
    /// ユーザー取得(完全一致・ユーザーID必須)
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>ユーザーマスタ</returns>
    /// <create>Y.Higuchi 2010/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ユーザー取得(完全一致・ユーザーID必須)")]
    public DataSet GetUser(CondM01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ds = impl.GetUser(dbHelper, cond);
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

    #region ユーザー取得(あいまい検索)

    /// --------------------------------------------------
    /// <summary>
    /// ユーザー取得(あいまい検索)
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>ユーザーマスタ</returns>
    /// <create>Y.Higuchi 2010/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ユーザー取得(あいまい検索)")]
    public DataSet GetUserLikeSearch(CondM01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ds = impl.GetUserLikeSearch(dbHelper, cond);
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

    #region ユーザーの追加

    /// --------------------------------------------------
    /// <summary>
    /// ユーザーの追加
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ユーザーの追加")]
    public bool InsUserData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                DataTable dt = ds.Tables[Def_M_USER.Name];
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.InsUserData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region ユーザーの更新

    /// --------------------------------------------------
    /// <summary>
    /// ユーザーの更新
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ユーザーの更新")]
    public bool UpdUserData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                DataTable dt = ds.Tables[Def_M_USER.Name];
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.UpdUserData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region ユーザーの削除

    /// --------------------------------------------------
    /// <summary>
    /// ユーザーの削除
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="isMultiError">複数エラーの場合はtrue</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/25</create>
    /// <update>H.Tsuji 2019/06/24 メール送信対象者の削除チェック</update>
    /// --------------------------------------------------
    [WebMethod(Description = "ユーザーの削除")]
    public bool DelUserData(CondM01 cond, ref DataSet ds, out bool isMultiError)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                isMultiError = false;

                // 接続
                this.DbOpen(dbHelper);

                // トランザクション開始
                dbHelper.BeginTransaction();

                // 実行
                bool ret = false;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.DelUserData(dbHelper, cond, ref ds, ref isMultiError);
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

    #region 納入先保守

    #region 納入先取得(あいまい検索)

    /// --------------------------------------------------
    /// <summary>
    /// 納入先取得(あいまい検索)
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>納入先マスタ</returns>
    /// <create>Y.Higuchi 2010/08/27</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "納入先取得(あいまい検索)")]
    public DataSet GetNonyusakiLikeSearch(CondM01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ds = impl.GetNonyusakiLikeSearch(dbHelper, cond);
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

    #region 未完了AR情報件数取得

    /// --------------------------------------------------
    /// <summary>
    /// 未完了AR情報件数取得
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>未完了AR情報件数</returns>
    /// <create>Y.Higuchi 2010/08/27</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "未完了AR情報件数取得")]
    public DataSet GetMikanryoAR(CondM01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ds = impl.GetMikanryoAR(dbHelper, cond);
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

    #region 納入先の更新

    /// --------------------------------------------------
    /// <summary>
    /// 納入先の更新
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/27</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "納入先の更新")]
    public bool UpdNonyusakiData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                DataTable dt = ds.Tables[Def_M_NONYUSAKI.Name];
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.UpdNonyusakiData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region 納入先の削除

    /// --------------------------------------------------
    /// <summary>
    /// 納入先の削除
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:削除</returns>
    /// <create>M.Tsutsumi 2011/02/18</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "納入先の削除")]
    public bool DelNonyusakiData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                errMsgID = string.Empty;
                args = null;
                // 接続
                dbHelper.Open();

                // トランザクション開始
                dbHelper.BeginTransaction();

                // 実行
                bool ret = false;
                DataTable dt = ds.Tables[Def_M_NONYUSAKI.Name];
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.DelNonyusakiData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region 納入先取得(存在確認用)

    /// --------------------------------------------------
    /// <summary>
    /// 納入先取得(存在確認用)
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/04/13</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "納入先取得(存在確認用)")]
    public DataSet GetNonyusaki(CondM01 cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                DataSet ds;
                using (var impl = new WsM01Impl())
                {
                    ds = impl.GetNonyusaki(dbHelper, cond);
                }
                return ds;
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

    #region 納入先の登録

    /// --------------------------------------------------
    /// <summary>
    /// 納入先の登録
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Sakiori 2012/04/13</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "納入先の登録")]
    public bool InsNonyusakiData(CondM01 cond, out string errMsgID, out string[] args)
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
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.InsNonyusakiData(dbHelper, cond, ref errMsgID, ref args);
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

    #region 未出荷明細件数取得

    /// --------------------------------------------------
    /// <summary>
    /// 未出荷明細件数取得
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/04/16</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "未出荷明細件数取得")]
    public DataSet GetMishukkaMeisai(CondM01 cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                DataSet ds;
                using (var impl = new WsM01Impl())
                {
                    ds = impl.GetMishukkaMeisai(dbHelper, cond);
                }
                return ds;
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

    #endregion

    #region 梱包情報保守

    #region Box梱包データ取得

    /// --------------------------------------------------
    /// <summary>
    /// Box梱包データ取得
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="errMsgID">エラーメッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns></returns>
    /// <create>Y.Higuchi 2010/08/26</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "Box梱包データ取得")]
    public DataSet GetBoxData(CondM01 cond, out string errMsgID, out string[] args)
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
                DataSet ds;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ds = impl.GetBoxData(dbHelper, cond, ref errMsgID, ref args);
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

    #region パレット梱包データ取得

    /// --------------------------------------------------
    /// <summary>
    /// パレット梱包データ取得
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="errMsgID">エラーメッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns></returns>
    /// <create>Y.Higuchi 2010/08/26</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "パレット梱包データ取得")]
    public DataSet GetPalletData(CondM01 cond, out string errMsgID, out string[] args)
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
                DataSet ds;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ds = impl.GetPalletData(dbHelper, cond, ref errMsgID, ref args);
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


    #region Box梱包データ更新

    /// --------------------------------------------------
    /// <summary>
    /// Box梱包データ更新
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dtManage">ボックスリスト管理データ</param>
    /// <param name="ds">更新データ</param>
    /// <param name="dtKonpo">追加梱包データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:</returns>
    /// <create>Y.Higuchi 2010/08/26</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "Box梱包データ更新")]
    // 2012/05/18 K.Tsutsumi Change 管理データのバージョンチェック
    //public bool UpdBoxData(CondM01 cond, DataTable ds, DataTable dtKonpo, out string errMsgID, out string[] args)
    public bool UpdBoxData(CondM01 cond, DataTable dtManage, DataTable dt, DataTable dtKonpo, out string errMsgID, out string[] args)
    // ↑
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
                bool ret;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    // 2012/05/18 K.Tsutsumi Change 管理データのバージョンチェック
                    //ret = impl.UpdBoxData(dbHelper, cond, ds, dtKonpo, ref errMsgID, ref args);
                    ret = impl.UpdBoxData(dbHelper, cond, dtManage, dt, dtKonpo, ref errMsgID, ref args);
                    // ↑
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

    #region パレット梱包データ更新

    /// --------------------------------------------------
    /// <summary>
    /// パレット梱包データ更新
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dtManage">パレットリスト管理データ</param>
    /// <param name="ds">更新データ</param>
    /// <param name="dtKonpo">追加梱包データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:</returns>
    /// <create>Y.Higuchi 2010/08/26</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "パレット梱包データ更新")]
    // 2012/05/18 K.Tsutsumi Change 管理データのバージョンチェック
    //public bool UpdPalletData(CondM01 cond, DataTable ds, DataTable dtKonpo, out string errMsgID, out string[] args)
    public bool UpdPalletData(CondM01 cond, DataTable dtManage, DataTable dt, DataTable dtKonpo, out string errMsgID, out string[] args)
    // ↑
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
                bool ret;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    // 2012/05/18 K.Tsutsumi Change 管理データのバージョンチェック
                    //ret = impl.UpdPalletData(dbHelper, cond, ds, dtKonpo, ref errMsgID, ref args);
                    ret = impl.UpdPalletData(dbHelper, cond, dtManage, dt, dtKonpo, ref errMsgID, ref args);
                    // ↑
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


    #region Box梱包データ取得(追加)

    /// --------------------------------------------------
    /// <summary>
    /// 追加するBox梱包データを取得
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="errMsgID">エラーメッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>DataSet</returns>
    /// <create>M.Tsutsumi 2011/03/08</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "Box梱包データ取得(追加)")]
    public DataSet GetBoxDataAdd(CondM01 cond, out string errMsgID, out string[] args)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                errMsgID = string.Empty;
                args = null;

                // 接続
                dbHelper.Open();

                // 実行
                DataSet ds;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ds = impl.GetBoxDataAdd(dbHelper, cond, ref errMsgID, ref args);
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

    #region パレット梱包データ取得(追加)

    /// --------------------------------------------------
    /// <summary>
    /// 追加するパレット梱包データを取得
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="errMsgID">エラーメッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>DataSet</returns>
    /// <create>M.Tsutsumi 2011/03/08</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "パレット梱包データ取得(追加)")]
    public DataSet GetPalletDataAdd(CondM01 cond, out string errMsgID, out string[] args)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                errMsgID = string.Empty;
                args = null;

                // 接続
                dbHelper.Open();

                // 実行
                DataSet ds;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ds = impl.GetPalletDataAdd(dbHelper, cond, ref errMsgID, ref args);
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

    #region 物件名保守

    #region 物件名取得(完全一致・物件管理No必須)

    /// --------------------------------------------------
    /// <summary>
    /// 物件名取得(完全一致・物件管理No必須)
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>物件マスタ</returns>
    /// <create>K.Saeki 2012/03/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "物件名取得(完全一致・物件管理No必須)")]
    public DataSet GetBukken(CondM01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ds = impl.GetBukken(dbHelper, cond);
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

    #region 物件名取得(あいまい検索)

    /// --------------------------------------------------
    /// <summary>
    /// 物件名取得(あいまい検索)
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>物件マスタ</returns>
    /// <create>K.Saeki 2012/03/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "物件名取得(あいまい検索)")]
    public DataSet GetBukkenLikeSearch(CondM01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ds = impl.GetBukkenLikeSearch(dbHelper, cond);
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

    #region 物件名の追加

    /// --------------------------------------------------
    /// <summary>
    /// 物件名の追加
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>K.Saeki 2012/03/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "物件名の追加")]
    public bool InsBukkenData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.InsBukkenData(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region 物件名の更新

    /// --------------------------------------------------
    /// <summary>
    /// 物件名の更新
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>K.Saeki 2012/03/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "物件名の更新")]
    public bool UpdBukkenData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                DataTable dt = ds.Tables[Def_M_BUKKEN.Name];
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.UpdBukkenData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region 物件名の削除

    /// --------------------------------------------------
    /// <summary>
    /// 物件名の削除
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>K.Saeki 2012/03/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "物件名の削除")]
    public bool DelBukkenData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                DataTable dt = ds.Tables[Def_M_BUKKEN.Name];
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.DelBukkenData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region 物件メールマスタ(共通)の存在確認

    /// --------------------------------------------------
    /// <summary>
    /// 物件メールマスタ(共通)の存在確認
    /// </summary>
    /// <param name="cond">M01用条件</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/20</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "物件メールマスタ(共通)の存在確認")]
    public bool ExistsBukkenMail(CondM01 cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                using (var impl = new WsM01Impl())
                {
                    return impl.GetBukkenMailCount(dbHelper, cond, MAIL_KBN.COMMON_VALUE1) > 0;
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


    #region 進捗管理通知設定(Default)の存在確認

    /// --------------------------------------------------
    /// <summary>
    /// 進捗管理通知設定(Default)の存在確認
    /// </summary>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>Y.Nakasato 2019/07/08</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "進捗管理通知設定(Default)の存在確認")]
    public bool ExistsShinchokuKanriMail(CondM01 cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                using (var impl = new WsM01Impl())
                {
                    return impl.GetBasicNotifyCount(dbHelper, cond, MAIL_KBN.ARSHINCHOKU_VALUE1) > 0;
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

    #endregion

    #region 名称保守

    #region 名称取得

    /// --------------------------------------------------
    /// <summary>
    /// 名称取得
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>名称</returns>
    /// <create>T.Sakiori 2012/04/06</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "名称取得")]
    public DataSet GetSelectItem(CondM01 cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (var impl = new WsM01Impl())
                {
                    ds = impl.GetSelectItemLike(dbHelper, cond);
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

    #region 名称の追加

    /// --------------------------------------------------
    /// <summary>
    /// 名称の追加
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Sakiori 2012/04/06</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "名称の追加")]
    public bool InsSelectItemData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
    {
        using (var dbHelper = this.GetDatabaseHelper())
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
                DataTable dt = ds.Tables[Def_M_SELECT_ITEM.Name];
                using (var impl = new WsM01Impl())
                {
                    ret = impl.InsSelectItemData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region 名称の更新

    /// --------------------------------------------------
    /// <summary>
    /// 名称の更新
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Sakiori 2012/04/06</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "名称の更新")]
    public bool UpdSelectItemData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
    {
        using (var dbHelper = this.GetDatabaseHelper())
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
                DataTable dt = ds.Tables[Def_M_SELECT_ITEM.Name];
                using (var impl = new WsM01Impl())
                {
                    ret = impl.UpdSelectItemData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region 名称の削除

    /// --------------------------------------------------
    /// <summary>
    /// 名称の削除
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Sakiori 2012/04/06</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "名称の削除")]
    public bool DelSelectItemData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
    {
        using (var dbHelper = this.GetDatabaseHelper())
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
                DataTable dt = ds.Tables[Def_M_SELECT_ITEM.Name];
                using (var impl = new WsM01Impl())
                {
                    ret = impl.DelSelectItemData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region 基本通知設定

    #region 基本通知設定取得

    /// --------------------------------------------------
    /// <summary>
    /// 基本通知設定取得
    /// </summary>
    /// <param name="cond">M01用条件</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/07</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "基本通知設定取得")]
    public DataSet GetBasicNotify(CondM01 cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                var ds = new DataSet();
                using (var impl = new WsM01Impl())
                {
                    ds.Tables.Add(impl.GetBasicNotify(dbHelper, cond));
                    ds.Tables.Add(impl.GetMailSetting(dbHelper, cond));
                }
                return ds;
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

    #region 通知基本設定の保存

    /// --------------------------------------------------
    /// <summary>
    /// 通知基本設定の保存
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgId">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/12</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "通知基本設定の保存")]
    public bool SaveBasicNotify(CondM01 cond, DataSet ds, out string errMsgId, out string[] args)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                errMsgId = string.Empty;
                args = null;

                this.DbOpen(dbHelper);

                dbHelper.BeginTransaction();

                bool ret = false;
                using (var impl = new WsM01Impl())
                {
                    ret = impl.SaveBasicNotify(dbHelper, cond, ds, ref errMsgId, ref args);
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
                throw new ExecutionEngineException(ex.Message, ex);
            }
            finally
            {
                dbHelper.Close();
            }
        }
    }

    #endregion

    #region メールアドレス変更権限を保持しているか

    /// --------------------------------------------------
    /// <summary>
    /// メールアドレス変更権限を保持しているか
    /// </summary>
    /// <param name="cond">ログインユーザー</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "メールアドレス変更権限を保持しているか")]
    public bool ExistsMailChangeRole(CondM01 cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                using (var impl = new WsM01Impl())
                {
                    return impl.ExistsMailChangeRole(dbHelper, cond);
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

    #endregion

    #region 共通通知設定

    #region 共通通知設定取得

    /// --------------------------------------------------
    /// <summary>
    /// 共通通知設定取得
    /// </summary>
    /// <param name="cond">M01用条件</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/14</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "共通通知設定取得")]
    public DataTable GetCommonNotify(CondM01 cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                using (var impl = new WsM01Impl())
                {
                    return impl.GetCommonNotify(dbHelper, cond);
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

    #region 共通通知設定の保存

    /// --------------------------------------------------
    /// <summary>
    /// 共通通知設定の保存
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgId">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/14</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "共通通知設定の保存")]
    public bool SaveCommonNotify(CondM01 cond, DataSet ds, out string errMsgId, out string[] args)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                errMsgId = string.Empty;
                args = null;

                this.DbOpen(dbHelper);

                dbHelper.BeginTransaction();

                bool ret = false;
                using (var impl = new WsM01Impl())
                {
                    ret = impl.SaveCommonNotify(dbHelper, cond, ds, ref errMsgId, ref args);
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
                throw new ExecutionEngineException(ex.Message, ex);
            }
            finally
            {
                dbHelper.Close();
            }
        }
    }

    #endregion

    #endregion

    #region AR List単位通知設定

    #region リスト区分取得

    /// --------------------------------------------------
    /// <summary>
    /// リスト区分取得
    /// </summary>
    /// <param name="cond">M01用条件</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/19</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "リスト区分取得")]
    public DataTable GetListFlag(CondM01 cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                using (var impl = new WsM01Impl())
                {
                    return impl.GetListFlag(dbHelper, cond);
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

    #region AR List単位通知設定取得

    /// --------------------------------------------------
    /// <summary>
    /// AR List単位通知設定取得
    /// </summary>
    /// <param name="cond">M01用条件</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/15</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "AR List単位通知設定取得")]
    public DataTable GetARListNotify(CondM01 cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                using (var impl = new WsM01Impl())
                {
                    return impl.GetARListNotify(dbHelper, cond);
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

    #region AR List単位通知設定削除

    /// --------------------------------------------------
    /// <summary>
    /// AR List単位通知設定削除
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgId">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns></returns>
    /// <create>K.Tsutsumi 2020/01/03</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "AR List単位通知設定削除")]
    public bool DelARListNotify(CondM01 cond, DataSet ds, out string errMsgId, out string[] args)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                errMsgId = string.Empty;
                args = null;

                this.DbOpen(dbHelper);

                dbHelper.BeginTransaction();

                bool ret = false;
                using (var impl = new WsM01Impl())
                {
                    ret = impl.DelARListNotifyData(dbHelper, cond, ds, ref errMsgId, ref args);
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
                throw new ExecutionEngineException(ex.Message, ex);
            }
            finally
            {
                dbHelper.Close();
            }
        }
    }

    #endregion


    #endregion

    #region メール送信情報メンテナンス

    #region メールデータの取得

    /// --------------------------------------------------
    /// <summary>
    /// メールデータの取得
    /// </summary>
    /// <param name="cond">M01用条件</param>
    /// <returns></returns>
    /// <create>R.Katsuo 2017/09/13</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "メールデータの取得")]
    public DataSet GetMail(CondM01 cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                var ds = new DataSet();
                using (var impl = new WsM01Impl())
                {
                    ds.Tables.Add(impl.GetMail(dbHelper, cond));
                }
                return ds;
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

    #region メールの再送信

    /// --------------------------------------------------
    /// <summary>
    /// メールの再送信
    /// </summary>
    /// <param name="cond">条件</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>R.Katsuo 2017/09/13</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "メールの再送信")]
    public bool InsMailRetry(CondM01 cond, DataTable dt, out string errMsgID, out string[] args)
    {
        using (var dbHelper = this.GetDatabaseHelper())
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
                using (WsM01Impl impl = new WsM01Impl())
                using (WsSmsImpl smsImpl = new WsSmsImpl())
                {
                    // 採番
                    string mailID = string.Empty;
                    var cs = new CondSms(cond.LoginInfo);
                    cs.SaibanFlag = SAIBAN_FLAG.MAIL_ID_VALUE1;
                    cs.UpdateUserID = cond.UpdateUserID;
                    cs.UpdateUserName = cond.UpdateUserName;
                    if (!smsImpl.GetSaiban(dbHelper, cs, out mailID, out errMsgID))
                    {
                        dbHelper.Rollback();
                        return false;
                    }

                    var filePath = DSWUtil.UtilData.GetFld(dt, 0, Def_T_MAIL.APPENDIX_FILE_PATH);
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        if (!System.IO.File.Exists(filePath))
                        {
                            // ファイルの添付に失敗しました。
                            errMsgID = "M0100110008";
                            dbHelper.Rollback();
                            return false;
                        }

                        var dirPath = System.IO.Path.GetDirectoryName(filePath);
                        var oldMailID = System.IO.Path.GetFileName(dirPath);
                        var newFilePath = filePath.Replace(oldMailID, mailID);
                        try
                        {
                            DSWUtil.UtilFile.CopyFile(filePath, newFilePath);
                            DSWUtil.UtilData.SetFld(dt, 0, Def_T_MAIL.APPENDIX_FILE_PATH, newFilePath);
                        }
                        catch
                        {
                            // ファイルの添付に失敗しました。
                            errMsgID = "M0100110008";
                            dbHelper.Rollback();
                            return false;
                        }
                    }

                    ret = impl.InsMailRetry(dbHelper, cond, dt, DSWUtil.UtilConvert.ToDecimal(mailID)) == dt.Rows.Count;
                }

                if (ret)
                {
                    dbHelper.Commit();
                }
                else
                {
                    // 再送信に失敗しました。
                    errMsgID = "M0100110003";
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

    #region メールの強制終了

    /// --------------------------------------------------
    /// <summary>
    /// メールの強制終了
    /// </summary>
    /// <param name="cond">条件</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>R.Katsuo 2017/09/13</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "メールの強制終了")]
    public bool UpdMailAbort(CondM01 cond, DataTable dt, out string errMsgID, out string[] args)
    {
        using (var dbHelper = this.GetDatabaseHelper())
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
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.UpdMailAbort(dbHelper, cond, dt) == dt.Rows.Count;
                }

                if (ret)
                {
                    dbHelper.Commit();
                }
                else
                {
                    // 強制終了に失敗しました。
                    errMsgID = "M0100110004";
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

    #region パーツ名翻訳マスタ

    #region パーツ名取得

    /// --------------------------------------------------
    /// <summary>
    /// パーツ名取得
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>パーツ名</returns>
    /// <create>S.Furugo 2018/10/29</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "パーツ名取得")]
    public DataSet GetPartName(CondM01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ds = impl.GetPartsName(dbHelper, cond);
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

    #region パーツ名取得(あいまい検索)

    /// --------------------------------------------------
    /// <summary>
    /// パーツ名取得(あいまい検索)
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>パーツ名</returns>
    /// <create>S.Furugo 2018/10/29</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "パーツ名取得(あいまい検索)")]
    public DataSet GetPartNameLike(CondM01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ds = impl.GetPartsNameSearch(dbHelper, cond);
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

    #region パーツ名取得(Excel)

    /// --------------------------------------------------
    /// <summary>
    /// パーツ名取得(Excel)
    /// </summary>
    /// <param name="cond">条件</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/07/03</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "パーツ名取得(Excel)")]
    public DataSet GetPartsNameExcelData(CondM01 cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ds = impl.GetPartsNameExcelData(dbHelper, cond);
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

    #region パーツ名の追加

    /// --------------------------------------------------
    /// <summary>
    /// パーツ名の追加
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>S.Furugo 2018/10/29</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "パーツ名の追加")]
    public bool InsPartData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
    {
        using (var dbHelper = this.GetDatabaseHelper())
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
                DataTable dt = ds.Tables[Def_M_PARTS_NAME.Name];
                using (var impl = new WsM01Impl())
                {
                    ret = impl.InsPartsData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region パーツ名の更新

    /// --------------------------------------------------
    /// <summary>
    /// パーツ名の更新
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>S.Furugo 2018/10/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "パーツ名の更新")]
    public bool UpdPartData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
    {
        using (var dbHelper = this.GetDatabaseHelper())
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
                DataTable dt = ds.Tables[Def_M_PARTS_NAME.Name];
                using (var impl = new WsM01Impl())
                {
                    ret = impl.UpdPartsData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region パーツ名の削除

    /// --------------------------------------------------
    /// <summary>
    /// パーツ名の削除
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>S.Furugo 2018/10/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "パーツ名の削除")]
    public bool DelPartData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
    {
        using (var dbHelper = this.GetDatabaseHelper())
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
                DataTable dt = ds.Tables[Def_M_PARTS_NAME.Name];
                using (var impl = new WsM01Impl())
                {
                    ret = impl.DelPartsData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region パーツ名翻訳マスタ(取込)

    #region パーツ名の追加

    /// --------------------------------------------------
    /// <summary>
    /// パーツ名の追加
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="dtMessage">メッセージテーブル</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tajimi 2019/07/26</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "パーツ名の追加(取込)")]
    public bool InsImportedPartsData(CondM01 cond, DataSet ds, ref DataTable dtMessage, out string errMsgID, out string[] args)
    {
        using (var dbHelper = this.GetDatabaseHelper())
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
                DataTable dt = ds.Tables[Def_M_PARTS_NAME.Name];
                using (var impl = new WsM01Impl())
                {
                    ret = impl.InsImportedPartsData(dbHelper, cond, dt, ref dtMessage, ref errMsgID, ref args);
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

    #region 荷受保守

    #region 荷受マスタ(あいまい検索)

    /// --------------------------------------------------
    /// <summary>
    /// 荷受マスタ(あいまい検索)
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>荷受マスタ</returns>
    /// <create>H.Tsuji 2018/12/05</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "荷受マスタ(あいまい検索)")]
    public DataSet GetConsignLikeSearch(CondM01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ds = impl.GetConsignLikeSearch(dbHelper, cond);
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

    #region 荷受マスタ(完全一致・荷受CD必須)

    /// --------------------------------------------------
    /// <summary>
    /// 荷受マスタ(完全一致・荷受CD必須)
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>荷受マスタ</returns>
    /// <create>H.Tsuji 2018/12/05</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "荷受マスタ(完全一致・荷受CD必須)")]
    public DataSet GetConsign(CondM01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ds = impl.GetConsign(dbHelper, cond);
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

    #region 荷受の追加

    /// --------------------------------------------------
    /// <summary>
    /// 荷受の追加
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/12/05</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "荷受の追加")]
    public bool InsConsignData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                DataTable dt = ds.Tables[Def_M_CONSIGN.Name];
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.InsConsignData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region 荷受の更新

    /// --------------------------------------------------
    /// <summary>
    /// 荷受の更新
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/12/06</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "荷受の更新")]
    public bool UpdConsignData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                DataTable dt = ds.Tables[Def_M_CONSIGN.Name];
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.UpdConsignData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region 荷受の削除

    /// --------------------------------------------------
    /// <summary>
    /// 荷受の削除
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/12/06</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "荷受の削除")]
    public bool DelConsignData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                DataTable dt = ds.Tables[Def_M_CONSIGN.Name];
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.DelConsignData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region 配送先保守

    #region 配送先マスタ(あいまい検索)

    /// --------------------------------------------------
    /// <summary>
    /// 配送先マスタ(あいまい検索)
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>配送先マスタ</returns>
    /// <create>H.Tsuji 2018/12/10</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "配送先マスタ(あいまい検索)")]
    public DataSet GetDeliverLikeSearch(CondM01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ds = impl.GetDeliverLikeSearch(dbHelper, cond);
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

    #region 配送先マスタ(完全一致・配送先CD必須)

    /// --------------------------------------------------
    /// <summary>
    /// 配送先マスタ(完全一致・配送先CD必須)
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>配送先マスタ</returns>
    /// <create>H.Tsuji 2018/12/10</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "配送先マスタ(完全一致・配送先CD必須)")]
    public DataSet GetDeliver(CondM01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ds = impl.GetDeliver(dbHelper, cond);
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

    #region 配送先の追加

    /// --------------------------------------------------
    /// <summary>
    /// 配送先の追加
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/12/10</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "配送先の追加")]
    public bool InsDeliverData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                DataTable dt = ds.Tables[Def_M_DELIVER.Name];
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.InsDeliverData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region 配送先の更新

    /// --------------------------------------------------
    /// <summary>
    /// 配送先の更新
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/12/10</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "配送先の更新")]
    public bool UpdDeliverData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                DataTable dt = ds.Tables[Def_M_DELIVER.Name];
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.UpdDeliverData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region 配送先の削除

    /// --------------------------------------------------
    /// <summary>
    /// 配送先の削除
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/12/10</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "配送先の削除")]
    public bool DelDeliverData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                DataTable dt = ds.Tables[Def_M_DELIVER.Name];
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.DelDeliverData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region 運送会社保守

    #region 物件名取得(運送会社CD/国内外フラグ/運送会社名)

    /// --------------------------------------------------
    /// <summary>
    /// 運送会社取得(運送会社CD/国内外フラグ/運送会社名)
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>運送会社マスタ</returns>
    /// <create>T.Nakata 2018/11/02</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "運送会社取得(運送会社CD/国内外フラグ/運送会社名)")]
    public DataSet GetUnsokaisya(CondM01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ds = impl.GetUnsokaisya(dbHelper, cond);
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

    #region 運送会社取得(あいまい検索:国内外フラグ/運送会社名)

    /// --------------------------------------------------
    /// <summary>
    /// 運送会社取得(あいまい検索:国内外フラグ/運送会社名)
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>物件マスタ</returns>
    /// <create>T.Nakata 2018/11/02</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "運送会社取得(あいまい検索:国内外フラグ/運送会社名)")]
    public DataSet GetUnsokaisyaLikeSearch(CondM01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ds = impl.GetUnsokaisyaLikeSearch(dbHelper, cond);
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

    #region 運送会社の追加

    /// --------------------------------------------------
    /// <summary>
    /// 運送会社の追加
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Nakata 2018/11/02</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "運送会社の追加")]
    public bool InsUnsoKaisyaData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.InsUnsoKaisyaData(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region 運送会社の更新

    /// --------------------------------------------------
    /// <summary>
    /// 運送会社の更新
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Nakata 2018/11/02</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "運送会社の更新")]
    public bool UpdUnsoKaisyaData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                DataTable dt = ds.Tables[Def_M_UNSOKAISHA.Name];
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.UpdUnsoKaisyaData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region 運送会社の削除

    /// --------------------------------------------------
    /// <summary>
    /// 運送会社の削除
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Nakata 2018/11/02</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "運送会社の削除")]
    public bool DelUnsoKaisyaData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                DataTable dt = ds.Tables[Def_M_UNSOKAISHA.Name];
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.DelUnsoKaisyaData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region 技連保守

    #region プロジェクトマスタ取得
    /// --------------------------------------------------
    /// <summary>
    /// プロジェクトマスタ取得
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>ユーザーマスタ</returns>
    /// <create>H.Tsuji 2018/11/20</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "プロジェクトマスタ取得")]
    public DataSet GetProject(CondM01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ds = impl.GetProject(dbHelper, cond);
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

    #region 名称マスタ取得(機種一覧用)
    /// --------------------------------------------------
    /// <summary>
    /// 名称マスタ取得(機種一覧用)
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>名称マスタ</returns>
    /// <create>H.Tsuji 2018/11/21</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "名称マスタ取得(機種一覧用)")]
    public DataSet GetSelectItemForKishu(CondM01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ds = impl.GetSelectItemForKishu(dbHelper, cond);
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

    #region 技連マスタ(完全一致・期、ECSNo.必須)
    /// --------------------------------------------------
    /// <summary>
    /// 技連マスタ(完全一致・期、ECSNo.必須)
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>技連マスタ</returns>
    /// <create>H.Tsuji 2018/11/21</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "技連マスタ(完全一致・期、ECSNo.必須)")]
    public DataSet GetEcs(CondM01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ds = impl.GetEcs(dbHelper, cond);
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

    #region 技連マスタ(あいまい検索)
    /// --------------------------------------------------
    /// <summary>
    /// 技連マスタ(あいまい検索)
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>技連マスタ</returns>
    /// <create>H.Tsuji 2018/11/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "技連マスタ(あいまい検索)")]
    public DataSet GetEcsLikeSearch(CondM01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ds = impl.GetEcsLikeSearch(dbHelper, cond);
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

    #region 技連の追加
    /// --------------------------------------------------
    /// <summary>
    /// 技連の追加
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/11/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "技連の追加")]
    public bool InsEcsData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                DataTable dt = ds.Tables[Def_M_ECS.Name];
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.InsEcsData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region 技連の更新
    /// --------------------------------------------------
    /// <summary>
    /// 技連の更新
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/11/27</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "技連の更新")]
    public bool UpdEcsData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                DataTable dt = ds.Tables[Def_M_ECS.Name];
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.UpdEcsData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region 技連の削除
    /// --------------------------------------------------
    /// <summary>
    /// 技連の削除
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/11/27</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "技連の削除")]
    public bool DelEcsData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                DataTable dt = ds.Tables[Def_M_ECS.Name];
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.DelEcsData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region 物件名保守一括

    #region 物件名保守一括の物件名取得(あいまい検索)

    /// --------------------------------------------------
    /// <summary>
    /// 物件名保守一括の物件名取得(あいまい検索)
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>物件マスタ</returns>
    /// <create>H.Tsuji 2018/12/14</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "物件名保守一括の物件名取得(あいまい検索)")]
    public DataSet GetBukkenIkkatsuLikeSearch(CondM01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ds = impl.GetBukkenIkkatsuLikeSearch(dbHelper, cond);
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

    #region 物件名保守一括の物件名取得

    /// --------------------------------------------------
    /// <summary>
    /// 物件名保守一括の物件名取得
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>物件マスタ</returns>
    /// <create>H.Tsuji 2018/12/17</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "物件名保守一括の物件名取得")]
    public DataSet GetBukkenIkkatsuSearch(CondM01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ds = impl.GetBukkenIkkatsuSearch(dbHelper, cond);
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

    #region 物件名保守一括の追加

    /// --------------------------------------------------
    /// <summary>
    /// 物件名保守一括の追加
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/12/14</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "物件名保守一括の追加")]
    public bool InsBukkenIkkatsuData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.InsBukkenIkkatsuData(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region 物件名保守一括の更新

    /// --------------------------------------------------
    /// <summary>
    /// 物件名保守一括の更新
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/12/18</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "物件名保守一括の更新")]
    public bool UpdBukkenIkkatsuData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.UpdBukkenIkkatsuData(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region 物件名保守一括の削除

    /// --------------------------------------------------
    /// <summary>
    /// 物件名保守一括の削除
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/12/18</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "物件名保守一括の削除")]
    public bool DelBukkenIkkatsuData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.DelBukkenIkkatsuData(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region 進捗管理通知設定

    #region 進捗管理通知設定取得

    /// --------------------------------------------------
    /// <summary>
    /// 進捗管理通知設定取得
    /// </summary>
    /// <param name="cond">M01用条件</param>
    /// <returns></returns>
    /// <create>Y.Nakasato 2019/07/08 AR進捗通知対応</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "進捗管理通知設定取得")]
    public DataTable GetShinchokuKanriNotify(CondM01 cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                using (var impl = new WsM01Impl())
                {
                    return impl.GetShinchokuKanriNotify(dbHelper, cond);
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

    #region 進捗管理通知設定の保存
    /// --------------------------------------------------
    /// <summary>
    /// 進捗管理通知設定の保存
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgId">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns></returns>
    /// <create>Y.Nakasato 2019/07/08 AR進捗通知対応</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "進捗管理通知設定の保存")]
    public bool SaveShinchokuKanriNotify(CondM01 cond, DataSet ds, out string errMsgId, out string[] args)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                errMsgId = string.Empty;
                args = null;

                this.DbOpen(dbHelper);

                dbHelper.BeginTransaction();

                bool ret = false;
                using (var impl = new WsM01Impl())
                {
                    ret = impl.SaveShinchokuKanriNotify(dbHelper, cond, ds, ref errMsgId, ref args);
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
                throw new ExecutionEngineException(ex.Message, ex);
            }
            finally
            {
                dbHelper.Close();
            }
        }
    }

    #endregion

    #endregion

    #region 出荷元保守

    #region 初期データ取得(出荷元保守)

    /// --------------------------------------------------
    /// <summary>
    /// 初期データ取得(出荷元保守)
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2020/04/14</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "初期データ取得(出荷元保守)")]
    public DataSet GetInitShukkamotoHoshu(CondM01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ds = impl.GetInitShukkamotoHoshu(dbHelper, cond);
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

    #region 出荷元マスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// 出荷元マスタ取得
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>出荷元マスタ</returns>
    /// <create>H.Tajimi 2020/04/14</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "出荷元マスタ取得")]
    public DataSet GetShipFrom(CondM01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ds = impl.GetShipFrom(dbHelper, cond);
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

    #region 出荷元マスタの追加

    /// --------------------------------------------------
    /// <summary>
    /// 出荷元マスタの追加
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tajimi 2020/04/14</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "出荷元マスタの追加")]
    public bool InsShipFrom(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                DataTable dt = ds.Tables[Def_M_SHIP_FROM.Name];
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.InsShipFrom(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region 出荷元マスタの更新

    /// --------------------------------------------------
    /// <summary>
    /// 出荷元マスタの更新
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tajimi 2020/04/14</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "出荷元マスタの更新")]
    public bool UpdShipFrom(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                DataTable dt = ds.Tables[Def_M_SHIP_FROM.Name];
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.UpdShipFrom(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region 出荷元マスタの削除

    /// --------------------------------------------------
    /// <summary>
    /// 出荷元マスタの削除
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tajimi 2020/04/14</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "出荷元マスタの削除")]
    public bool DelShipFrom(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                DataTable dt = ds.Tables[Def_M_SHIP_FROM.Name];
                using (WsM01Impl impl = new WsM01Impl())
                {
                    ret = impl.DelShipFrom(dbHelper, cond, dt, ref errMsgID, ref args);
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



    #region　権限付与の確認（権限マスタ、権限マップマスタ）2022/10/14
    /// --------------------------------------------------
    /// <summary>
    /// 指定された機能の利用権限を保持しているか
    /// </summary>
    /// <param name="cond">ログインユーザー</param>
    /// <param name="cond">ログインユーザー</param>
    /// <param name="cond">ログインユーザー</param>
    /// <returns>true: 権限あり　
    ///          false:権限なし</returns>
    /// <create>TW-Tsuji 2022/10/14</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "指定された機能の利用権限を保持しているか")]
    public bool ExistsRoleAndRolemap(CondM01 cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                using (var impl = new WsM01Impl())
                {
                    return impl.ExistsRoleAndRolemap(dbHelper, cond);
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


    #region 計画取込通知設定

    #region 計画取込通知設定取得

    /// --------------------------------------------------
    /// <summary>
    /// 計画取込通知設定取得
    /// </summary>
    /// <param name="cond">M01用条件</param>
    /// <returns></returns>
    /// <create>Y.Gwon 2023/07/19</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "計画取込通知設定取得")]
    public DataTable GetKeikakuTorikomiNotify(CondM01 cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                using (var impl = new WsM01Impl())
                {
                    return impl.GetKeikakuTorikomiNotify(dbHelper, cond);
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

    #region 計画取込通知設定の保存

    /// --------------------------------------------------
    /// <summary>
    /// 計画取込通知設定の保存
    /// </summary>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgId">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns></returns>
    /// <create>Y.Gwon 2023/07/19</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "計画取込通知設定の保存")]
    public bool SaveKeikakuTorikomiNotify(CondM01 cond, DataSet ds, out string errMsgId, out string[] args)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                errMsgId = string.Empty;
                args = null;

                this.DbOpen(dbHelper);

                dbHelper.BeginTransaction();

                bool ret = false;
                using (var impl = new WsM01Impl())
                {
                    ret = impl.SaveKeikakuTorikomiNotify(dbHelper, cond, ds, ref errMsgId, ref args);
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
                throw new ExecutionEngineException(ex.Message, ex);
            }
            finally
            {
                dbHelper.Close();
            }
        }
    }

    #endregion

    #endregion

}

