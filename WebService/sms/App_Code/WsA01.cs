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
/// AR品管理処理クラス（トランザクション層）
/// </summary>
/// <create>Y.Higuchi 2010/06/22</create>
/// <update></update>
/// --------------------------------------------------
[WebService(Description = "出荷管理システム", Namespace = "http://smssrv/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。 
// [System.Web.Script.Services.ScriptService]
public class WsA01 : WsBase
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/06/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsA01()
        : base()
    {
    }

    #endregion

    #region A0100010:AR情報登録

    #region 納入先取得チェック

    /// --------------------------------------------------
    /// <summary>
    /// 納入先存在チェック
    /// </summary>
    /// <param name="cond">A01用コンディション</param>
    /// <returns>true:存在する false:存在しない</returns>
    /// <create>M.Tsutsumi 2010/08/12</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "納入先存在チェック")]
    public bool IsExistenceNonyusaki(CondA1 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                string kanriFlag = string.Empty;

                // 接続
                this.DbOpen(dbHelper);

                // 実行
                bool ret = false;
                using (WsA01Impl impl = new WsA01Impl())
                {
                    ret = impl.IsExistenceNonyusaki(dbHelper, cond, ref kanriFlag);
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

    #region AR情報データ取得

    /// --------------------------------------------------
    /// <summary>
    /// AR情報データ取得
    /// </summary>
    /// <param name="cond">A01用コンディション</param>
    /// <returns>データセット</returns>
    /// <create>M.Tsutsumi 2010/08/12</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "AR情報データ取得")]
    public DataSet GetArDataList(CondA1 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsA01Impl impl = new WsA01Impl())
                {
                    ds = impl.GetArDataList(dbHelper, cond);
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

    #region 納入先マスタの存在確認後、Excelデータの件数を取得する

    /// --------------------------------------------------
    /// <summary>
    /// 納入先マスタの存在確認後、Excelデータの件数を取得する
    /// </summary>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="errMsgID">エラーメッセージID</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/05/09</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "納入先マスタの存在確認後、Excelデータの件数を取得する")]
    public bool ExistNonyusakiAndExcle(CondA1 cond, out string errMsgID)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                errMsgID = string.Empty;

                // 接続
                this.DbOpen(dbHelper);

                // 実行
                bool ret = false;
                using (WsA01Impl impl = new WsA01Impl())
                {
                    ret = impl.ExistNonyusakiAndExcle(dbHelper, cond, ref errMsgID);
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

    #region 全体Excel用データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 全体Excel用データ取得
    /// </summary>
    /// <param name="cond">A01用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/04/26</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "全体Excel用データ取得")]
    public DataTable GetAllARListData(CondA1 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataTable dt;
                using (WsA01Impl impl = new WsA01Impl())
                {
                    dt = impl.GetAllARList(dbHelper, cond);
                }
                return dt;
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

    #region 納入先マスタの存在確認後、費用Excelデータの件数を取得する

    /// --------------------------------------------------
    /// <summary>
    /// 納入先マスタの存在確認後、費用Excelデータの件数を取得する
    /// </summary>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="errMsgID">エラーメッセージID</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/10/16</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "納入先マスタの存在確認後、費用Excelデータの件数を取得する")]
    public bool ExistNonyusakiAndHiyouExcel(CondA1 cond, out string errMsgID)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                errMsgID = string.Empty;

                // 接続
                this.DbOpen(dbHelper);

                // 実行
                bool ret = false;
                using (WsA01Impl impl = new WsA01Impl())
                {
                    ret = impl.ExistNonyusakiAndHiyouExcel(dbHelper, cond, ref errMsgID);
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

    #region 全体費用Excel用データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 全体費用Excel用データ取得
    /// </summary>
    /// <param name="cond">A01用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/10/16</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "全体費用Excel用データ取得")]
    public DataTable GetAllARCostListData(CondA1 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataTable dt;
                using (WsA01Impl impl = new WsA01Impl())
                {
                    dt = impl.GetAllARCostList(dbHelper, cond);
                }
                return dt;
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

    #region メール送信情報を取得

    /// --------------------------------------------------
    /// <summary>
    /// メール送信情報を取得
    /// </summary>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:存在する false:存在しない</returns>
    /// <create>R.Katsuo 2017/09/14</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "メール送信情報を取得")]
    public DataSet GetSendMailInfo(CondA1 cond, out string errMsgID, out string[] args)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                var ds = new DataSet();
                errMsgID = string.Empty;
                args = null;

                // 接続
                this.DbOpen(dbHelper);

                // 実行
                using (WsA01Impl impl = new WsA01Impl())
                {
                    return impl.GetSendMailInfo(dbHelper, cond, ref errMsgID, ref args);
                }
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

    #region A0100020:AR情報明細登録

    #region 納入先マスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// 納入先マスタ取得
    /// </summary>
    /// <param name="cond">A01用コンディション</param>
    /// <returns></returns>
    /// <create>M.Tsutsumi 2010/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "納入先マスタ取得")]
    public DataSet GetNonyusaki(CondA1 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds = new DataSet();
                DataTable dt;
                using (WsA01Impl impl = new WsA01Impl())
                {
                    dt = impl.GetNonyusaki(dbHelper, cond);
                }
                ds.Tables.Add(dt.Copy());
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

    #region AR情報データ取得

    /// --------------------------------------------------
    /// <summary>
    /// AR情報データ取得
    /// </summary>
    /// <param name="cond">A01用コンディション</param>
    /// <returns>データセット</returns>
    /// <create>M.Tsutsumi 2010/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "AR情報データ取得")]
    public DataSet GetARData(CondA1 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsA01Impl impl = new WsA01Impl())
                {
                    ds = impl.GetArData(dbHelper, cond);
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

    #region AR情報データ インターロック＆データ取得

    /// --------------------------------------------------
    /// <summary>
    /// AR情報データ インターロック＆データ取得
    /// </summary>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="ds">データセット</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>M.Tsutsumi 2010/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "AR情報データ インターロック＆データ取得")]
    public bool GetARandInterLock(CondA1 cond, out DataSet ds, out string errMsgID)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                errMsgID = string.Empty;
                ds = new DataSet();

                // 接続
                this.DbOpen(dbHelper);

                // トランザクション開始
                dbHelper.BeginTransaction();

                // 実行
                bool ret = false;
                using (WsA01Impl impl = new WsA01Impl())
                {
                    ret = impl.ARInterLock(dbHelper, cond, ref errMsgID);
                }

                if (ret)
                {
                    dbHelper.Commit();
                }
                else
                {
                    dbHelper.Rollback();
                }

                // データ取得
                if (ret)
                {
                    using (WsA01Impl impl = new WsA01Impl())
                    {
                        ds = impl.GetArData(dbHelper, cond);
                    }
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

    #region AR情報データ インターロック解除

    /// --------------------------------------------------
    /// <summary>
    /// AR情報データ インターロック解除
    /// </summary>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dt">ARデータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>M.Tsutsumi 2010/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "AR情報データ インターロック解除")]
    public bool UpdARInterUnLock(CondA1 cond, DataTable dt)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // トランザクション開始
                dbHelper.BeginTransaction();

                // 実行
                bool ret = false;
                using (WsA01Impl impl = new WsA01Impl())
                {
                    ret = impl.ARInterUnLock(dbHelper, cond, dt);
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

    #region AR情報データ登録

    /// --------------------------------------------------
    /// <summary>
    /// AR情報データ登録
    /// </summary>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="ds">データセット</param>
    /// <param name="nonyusakiCd">採番/取得した納入先コード</param>
    /// <param name="arNo">採番したARNo</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>M.Tsutsumi 2010/08/24</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "AR情報データ登録")]
    public bool InsAR(CondA1 cond, DataSet ds, out string nonyusakiCd, out string arNo, out string errMsgID, out string[] args)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                nonyusakiCd = string.Empty;
                arNo = string.Empty;
                errMsgID = string.Empty;
                args = null;

                // 接続
                this.DbOpen(dbHelper);

                // トランザクション開始
                dbHelper.BeginTransaction();

                // 実行
                bool ret = false;
                using (WsA01Impl impl = new WsA01Impl())
                {
                    try
                    {
                        ret = impl.InsArMeisai(dbHelper, cond, ds, ref nonyusakiCd, ref arNo, ref errMsgID, ref args);
                    }
                    catch (Exception exIns)
                    {
                        // 一意制約違反チェック
                        if (!impl.IsDbDuplicationError(exIns))
                        {
                            throw new Exception(exIns.Message, exIns);
                        }
                        dbHelper.RollbackAndBeginTransaction();
                        ret = impl.InsArMeisai(dbHelper, cond, ds, ref nonyusakiCd, ref arNo, ref errMsgID, ref args);
                    }
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

    #region AR情報データ更新

    /// --------------------------------------------------
    /// <summary>
    /// AR情報データ更新
    /// </summary>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="ds">データセット</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>M.Tsutsumi 2010/08/24</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "AR情報データ更新")]
    public bool UpdAR(CondA1 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (WsA01Impl impl = new WsA01Impl())
                {
                    ret = impl.UpdArMeisai(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region AR情報メール設定取得

    /// --------------------------------------------------
    /// <summary>
    /// AR情報メール設定取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション(bukkenNo,listFlag,IsToroku)</param>
    /// <param name="errMsgID">エラーメッセージ</param>
    /// <param name="args">エラーパラメータ</param>
    /// <returns>データセット</returns>
    /// <create>D.Okumura 2019/08/07 AR進捗対応</create>
    /// <update>J.Chen 2024/08/22 メール通知フラグ取得</update>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "AR情報データ更新")]
    public DataSet GetArMailInfo(CondA1 cond, out string errMsgID, out string[] args, out bool _isNotify)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                errMsgID = string.Empty;
                args = null;
                _isNotify = false;

                // 接続
                this.DbOpen(dbHelper);

                // トランザクション開始
                dbHelper.BeginTransaction();

                // 実行
                DataSet ret = null;
                using (WsA01Impl impl = new WsA01Impl())
                {
                    ret = impl.GetArMailInfo(dbHelper, cond, ref errMsgID, ref args, ref _isNotify);
                }
                dbHelper.Rollback();

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

    #region 出荷明細データのチェック
    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データのチェック
    /// </summary>
    /// <param name="cond"></param>
    /// <param name="ds"></param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/10/29</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "出荷明細データのチェック")]
    public bool GetCheckMeisaiData(CondA1 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (WsA01Impl impl = new WsA01Impl())
                {
                    ret = impl.GetCheckMeisaiData(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region 名称マスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// 名称マスタ取得
    /// </summary>
    /// <param name="cond">A01用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/04/26</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "名称マスタ取得")]
    public DataTable GetSelectItem(CondA1 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                using (WsA01Impl impl = new WsA01Impl())
                {
                    return impl.GetSelectItem(dbHelper, cond);
                }
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

    #region ユーザー情報を取得

    /// --------------------------------------------------
    /// <summary>
    /// ユーザー情報を取得
    /// </summary>
    /// <param name="cond">A01用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/10/12</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ユーザー取得")]
    public DataTable GetUser(CondA1 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                DataTable dt;
                using (WsA01Impl impl = new WsA01Impl())
                {
                    dt = impl.GetUser(dbHelper, cond);
                }
                return dt;
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

    #region リスト区分取得

    /// --------------------------------------------------
    /// <summary>
    /// リスト区分取得
    /// </summary>
    /// <param name="cond">A01用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/10/15</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "リスト区分取得")]
    public DataTable GetCommonListFlag(CondA1 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                using (WsA01Impl impl = new WsA01Impl())
                {
                    return impl.GetCommonListFlag(dbHelper, cond);
                }
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

    #region 登録号機数の取得

    /// --------------------------------------------------
    /// <summary>
    /// 登録号機数の取得
    /// </summary>
    /// <param name="cond">A01用コンディション</param>
    /// <returns></returns>
    /// <create>Y.Nakasato 2019/07/31</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "登録号機数の取得")]
    public int GetGokiNum(CondA1 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                using (WsA01Impl impl = new WsA01Impl())
                {
                    return impl.GetGokiNum(dbHelper, cond);
                }
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

    #region 進捗管理通知設定取得

    /// --------------------------------------------------
    /// <summary>
    /// 進捗管理通知設定取得
    /// </summary>
    /// <param name="cond">M01用条件</param>
    /// <returns></returns>
    /// <create>Y.Nakasato 2019/08/02</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "進捗管理通知設定取得")]
    public DataTable GetShinchokuKanriNotify(CondA1 cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                using (var impl = new WsA01Impl())
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

    #region MAIL_ID取得

    /// --------------------------------------------------
    /// <summary>
    /// MAIL_ID取得
    /// </summary>
    /// <param name="cond"></param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <create>J.Chen 2024/08/05</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "MAIL_ID取得")]
    public string GetMailIDWithoutUpdate(CondA1 cond, out string errMsgID, out string[] args)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            string mailID = string.Empty;
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
                using (WsSmsImpl smsImpl = new WsSmsImpl())
                {
                    var condSms = new CondSms(cond.LoginInfo);
                    condSms.SaibanFlag = SAIBAN_FLAG.MAIL_ID_VALUE1;
                    condSms.UpdateUserID = cond.UpdateUserID;
                    condSms.UpdateUserName = cond.UpdateUserName;
                    condSms.UpdateFlag = false;
                    ret = smsImpl.GetSaiban(dbHelper, condSms, out mailID, out errMsgID);
                }

                if (ret)
                {
                    dbHelper.Commit();
                }
                else
                {
                    dbHelper.Rollback();
                    return string.Empty;
                }
                return mailID;
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

    #region AR_No取得

    /// --------------------------------------------------
    /// <summary>
    /// AR_No取得
    /// </summary>
    /// <param name="cond"></param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <create>J.Chen 2024/08/05</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "AR_No取得")]
    public string GetARNoWithoutUpdate(CondA1 cond, out string errMsgID, out string[] args)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            string arNo = string.Empty;
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
                using (WsSmsImpl smsImpl = new WsSmsImpl())
                {
                    var condSms = new CondSms(cond.LoginInfo);
                    condSms.SaibanFlag = SAIBAN_FLAG.AR_NO_VALUE1;
                    condSms.ARUS = cond.NonyusakiCD;
                    condSms.ListFlag = cond.ListFlag;
                    condSms.UpdateFlag = false;
                    ret = smsImpl.GetSaiban(dbHelper, condSms, out arNo, out errMsgID);
                }

                if (ret)
                {
                    dbHelper.Commit();
                }
                else
                {
                    dbHelper.Rollback();
                    return string.Empty;
                }
                return arNo;
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

    #region A0100040:員数表取込

    #region 号機チェック
    /// --------------------------------------------------
    /// <summary>
    /// 号機チェック
    /// </summary>
    /// <param name="cond"></param>
    /// <param name="dt"></param>
    /// <param name="errMsgID"></param>
    /// <param name="dtMessage"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <create>T.Nakata 2019/07/08</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "号機チェック")]
    public bool CheckGoki(CondA1 cond, DataTable dt, out string errMsgID, out DataTable dtMessage, out string[] args)
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
                using (WsA01Impl impl = new WsA01Impl())
                {
                    dtMessage = ComFunc.GetSchemeMultiMessage();
                    ret = impl.CheckGoki(dbHelper, cond, dt, ref dtMessage);
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

    #region 員数表登録
    /// --------------------------------------------------
    /// <summary>
    /// 員数表登録
    /// </summary>
    /// <param name="cond"></param>
    /// <param name="dt"></param>
    /// <param name="errMsgID"></param>
    /// <param name="dtMessage"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <create>T.Nakata 2019/07/08</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "員数表登録")]
    public bool InsInzuhyo(CondA1 cond, DataTable dt, out string errMsgID, out DataTable dtMessage, out string[] args)
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
                using (WsA01Impl impl = new WsA01Impl())
                {
                    dtMessage = ComFunc.GetSchemeMultiMessage();
                    ret = impl.InsInzuhyo(dbHelper, cond, dt, ref errMsgID, ref dtMessage, ref args);
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

    #region A0100050:ＡＲ進捗管理

    #region 進捗情報検索
    /// --------------------------------------------------
    /// <summary>
    /// 進捗情報検索
    /// </summary>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>T.Nakata 2019/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "進捗情報検索")]
    public DataTable GetArShinchokuList(CondA1 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataTable dt;
                using (WsA01Impl impl = new WsA01Impl())
                {
                    dt = impl.GetARShinchokuList(dbHelper, cond);
                }
                return dt;
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

    #region AR進捗のメール送信情報を取得
    /// --------------------------------------------------
    /// <summary>
    /// AR進捗のメール送信情報を取得
    /// </summary>
    /// <param name="cond"></param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <create>T.Nakata 2019/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "AR進捗のメール送信情報を取得")]
    public DataSet GetArShinchokuMailInfo(CondA1 cond, out string errMsgID, out string[] args)
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
                using (WsA01Impl impl = new WsA01Impl())
                {
                    return impl.GetArShinchokuMailInfo(dbHelper, cond, ref errMsgID, ref args);
                }
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

    #region AR進捗のメール送信
    /// --------------------------------------------------
    /// <summary>
    /// AR進捗のメール送信
    /// </summary>
    /// <param name="cond"></param>
    /// <param name="ds"></param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <create>T.Nakata 2019/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "AR進捗のメール送信")]
    public bool SendArShinchokuMail(CondA1 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (WsA01Impl impl = new WsA01Impl())
                {
                    ret = impl.SendArShinchokuMail(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region A0100051:ＡＲ進捗管理日付登録

    #region AR情報のインターンロックを解除
    /// --------------------------------------------------
    /// <summary>
    /// AR情報のインターンロックを解除
    /// </summary>
    /// <param name="cond"></param>
    /// <param name="dt"></param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <create>T.Nakata 2019/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "AR情報のインターンロックを解除")]
    public bool UpdARUnLockShinchoku(CondA1 cond, DataTable dt, out string errMsgID, out string[] args)
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
                using (WsA01Impl impl = new WsA01Impl())
                {
                    ret = impl.UpdArUnLockShinchoku(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region AR進捗情報取得
    /// --------------------------------------------------
    /// <summary>
    /// AR進捗情報取得
    /// </summary>
    /// <param name="cond"></param>
    /// <param name="dt"></param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <create>T.Nakata 2019/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "AR進捗情報取得")]
    public DataSet GetARInterLockAndShinchokuInfo(CondA1 cond, DataTable dt, out string errMsgID, out string[] args)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                errMsgID = string.Empty;
                args = null;

                // 接続
                this.DbOpen(dbHelper);

                DataSet ds = null;
                using (WsA01Impl impl = new WsA01Impl())
                {
                    ds = impl.GetArInterLockAndShinchokuInfo(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region AR進捗情報登録
    /// --------------------------------------------------
    /// <summary>
    /// AR進捗情報登録
    /// </summary>
    /// <param name="cond"></param>
    /// <param name="ds"></param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <create>T.Nakata 2019/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "AR進捗情報登録")]
    public bool UpdARShinchokuInfo(CondA1 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (WsA01Impl impl = new WsA01Impl())
                {
                    ret = impl.UpdArShinchokuInfo(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region A0100052:機種ダイアログ

    /// --------------------------------------------------
    /// <summary>
    /// 機種取得
    /// </summary>
    /// <param name="cond">A01用コンディション</param>
    /// <returns></returns>
    /// <create>Y.Nakasato 2019/07/16</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "機種取得")]
    public DataTable GetKishu(CondA1 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                DataTable dt;
                using (WsA01Impl impl = new WsA01Impl())
                {
                    dt = impl.GetKishu(dbHelper, cond);
                }
                return dt;
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

    #region A0100053:号機ダイアログ

    /// --------------------------------------------------
    /// <summary>
    /// 号機取得
    /// </summary>
    /// <param name="cond">A01用コンディション</param>
    /// <returns></returns>
    /// <create>Y.Nakasato 2019/07/16</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "号機取得")]
    public DataTable GetGoki(CondA1 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                DataTable dt;
                using (WsA01Impl impl = new WsA01Impl())
                {
                    dt = impl.GetGoki(dbHelper, cond);
                }
                return dt;
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

    #region A0100060:変更履歴

    /// --------------------------------------------------
    /// <summary>
    /// 変更履歴取得
    /// </summary>
    /// <param name="cond">A01用コンディション</param>
    /// <returns></returns>
    /// <create>Y.Nakasato 2019/07/16</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "変更履歴取得")]
    public DataTable GetRireki(CondA1 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                DataTable dt;
                using (WsA01Impl impl = new WsA01Impl())
                {
                    dt = impl.GetRireki(dbHelper, cond);
                }
                return dt;
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

}

