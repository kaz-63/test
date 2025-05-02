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
/// 共通処理クラス（トランザクション層）
/// </summary>
/// <create>Y.Higuchi 2010/04/15</create>
/// <update></update>
/// --------------------------------------------------
[WebService(Description = "出荷管理システム", Namespace = "http://smssrv/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。 
// [System.Web.Script.Services.ScriptService]
public class WsCommon : WsBase
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/04/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsCommon()
        : base()
    {
    }

    #endregion

    #region サーバーの時間を取得

    /// --------------------------------------------------
    /// <summary>
    /// サーバーの時間を取得
    /// </summary>
    /// <returns>現在時間</returns>
    /// <create>Y.Higuchi 2010/07/01</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "サーバーの時間を取得する")]
    public DateTime GetNowDateTime()
    {
        return DateTime.Now;
    }


    #endregion

    #region M_SYSTEM_PARAMETER

    /// --------------------------------------------------
    /// <summary>
    /// システムパラメーター取得
    /// </summary>
    /// <returns>システムパラメーター</returns>
    /// <create>Y.Higuchi 2010/04/23</create>
    /// <update>H.Tajimi 2018/09/03 多言語対応</update>
    /// --------------------------------------------------
    [WebMethod(Description = "システムパラメーター取得")]
    public DataSet GetSystemParameter(CondCommon cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsCommonImpl impl = new WsCommonImpl())
                {
                    ds = impl.GetSystemParameter(dbHelper, cond);
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

    #region ログイン情報取得

    /// --------------------------------------------------
    /// <summary>
    /// ログイン情報取得
    /// </summary>
    /// <param name="cond">検索条件</param>
    /// <returns>ログイン情報</returns>
    /// <create>Y.Higuchi 2010/04/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ログイン情報取得")]
    public DataSet GetLoginUser(CondCommon cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsCommonImpl impl = new WsCommonImpl())
                {
                    ds = impl.GetLoginUser(dbHelper, cond, false);
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

    #region メニュー取得

    /// --------------------------------------------------
    /// <summary>
    /// メニュー取得
    /// </summary>
    /// <param name="cond">検索条件</param>
    /// <returns>メニュー</returns>
    /// <create>Y.Higuchi 2010/04/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "メニュー取得")]
    public DataSet GetMenu(CondCommon cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsCommonImpl impl = new WsCommonImpl())
                {
                    ds = impl.GetMenu(dbHelper, cond);
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

    #region メッセージ取得

    /// --------------------------------------------------
    /// <summary>
    /// メッセージ取得
    /// </summary>
    /// <param name="cond">検索条件</param>
    /// <returns>メッセージ</returns>
    /// <create>Y.Higuchi 2010/04/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "メッセージ取得")]
    public DataSet GetMessage(CondCommon cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsCommonImpl impl = new WsCommonImpl())
                {
                    ds = impl.GetMessage(dbHelper, cond);
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

    /// --------------------------------------------------
    /// <summary>
    /// 複数メッセージ取得
    /// </summary>
    /// <param name="cond">検索条件</param>
    /// <returns>メッセージ</returns>
    /// <create>Y.Higuchi 2010/07/16</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "メッセージ取得")]
    public DataSet GetMultiMessage(CondCommon cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsCommonImpl impl = new WsCommonImpl())
                {

                    ds = impl.GetMessage(dbHelper, cond);
                    DataTable dt = impl.GetMultiMessage(dbHelper, cond);
                    dt.TableName = ComDefine.DTTBL_MULTIRESULT;
                    ds.Tables.Add(dt);
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

    #region 汎用マスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// 汎用マスタ取得
    /// </summary>
    /// <param name="cond">検索条件</param>
    /// <returns>汎用マスタ</returns>
    /// <create>Y.Higuchi 2010/04/27</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "汎用マスタ取得")]
    public DataSet GetCommon(CondCommon cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsCommonImpl impl = new WsCommonImpl())
                {
                    ds = impl.GetCommon(dbHelper, cond);
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

    #region パスワード更新

    /// --------------------------------------------------
    /// <summary>
    /// パスワード更新
    /// </summary>
    /// <param name="cond">パスワード変更コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">メッセージのパラメーター</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/07/02</create>
    /// <update>H.Tajimi 2018/09/03 多言語対応</update>
    /// --------------------------------------------------
    [WebMethod(Description = "パスワード更新")]
    public bool UpdUserPassword(CondUserPassword cond, out DataSet ds, out string errMsgID, out string[] args)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);


                using (WsCommonImpl impl = new WsCommonImpl())
                {
                    // 初期化
                    errMsgID = string.Empty;
                    args = null;
                    ds = new DataSet();
                    // システムパラメーター取得
            		var condCommon = new CondCommon(cond.LoginInfo) { LoginInfo = cond.LoginInfo };
                    DataTable dtSysParam = impl.GetSystemParameter(dbHelper, condCommon).Tables[Def_M_SYSTEM_PARAMETER.Name];
                    int minPassword = ComFunc.GetFldToInt32(dtSysParam, 0, Def_M_SYSTEM_PARAMETER.MIN_PASSWORD);
                    int maxPassword = ComFunc.GetFldToInt32(dtSysParam, 0, Def_M_SYSTEM_PARAMETER.MAX_PASSWORD);
                    string passwordCheck = ComFunc.GetFld(dtSysParam, 0, Def_M_SYSTEM_PARAMETER.PASSWORD_CHECK);
                    string duplicationPastPassword = ComFunc.GetFld(dtSysParam, 0, Def_M_SYSTEM_PARAMETER.DUPLICATION_PAST_PASSWORD);
                    // パスワードチェック
                    if (!impl.CheckInputPassword(dbHelper, cond, minPassword, maxPassword, passwordCheck, duplicationPastPassword, out errMsgID, out args))
                    {
                        return false;
                    }

                    try
                    {
                        // トランザクション開始
                        dbHelper.BeginTransaction();

                        // 実行
                        int record = 0;
                        // パスワード更新
                        record = impl.UpdUserPassword(dbHelper, cond);
                        if (0 == record)
                        {
                            dbHelper.Rollback();
                            // 更新に失敗しました。
                            errMsgID = "A9999999014";
                            return false;
                        }

                        if (duplicationPastPassword == DUPLICATION_PAST_PASSWORD.ENABLE_VALUE1)
                        {
                            record = 0;
                            record = impl.InsPastPassword(dbHelper, cond);
                            if (0 == record)
                            {
                                dbHelper.Rollback();
                                // 保存に失敗しました。
                                errMsgID = "A9999999013";
                                return false;
                            }
                        }

                        // 新しいユーザー情報を取得する。

                        CondCommon condCom = new CondCommon(cond.LoginInfo);
                        condCom.UserID = cond.UserID;
                        condCom.Password = cond.NewPassword;
                        condCom.LoginInfo = cond.LoginInfo;
                        ds = impl.GetLoginUser(dbHelper, condCom, false);

                        dbHelper.Commit();
                    }
                    catch (Exception exTran)
                    {
                        dbHelper.Rollback();
                        throw new Exception(exTran.Message, exTran);
                    }
                }
                return true;
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

    #region メール関連

    /// --------------------------------------------------
    /// <summary>
    /// メール登録前の確認
    /// </summary>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "メール登録前の確認")]
    public DataSet CheckMail(CondCommon cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                using (var impl = new WsCommonImpl())
                {
                    return impl.CheckMail(dbHelper, cond);
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

    /// --------------------------------------------------
    /// <summary>
    /// メール登録前の確認(荷姿用)
    /// </summary>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/12/04</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "メール登録前の確認(荷姿用)")]
    public DataSet CheckPackingMail(CondCommon cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                using (var impl = new WsCommonImpl())
                {
                    return impl.CheckPackingMail(dbHelper, cond);
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

    /// --------------------------------------------------
    /// <summary>
    /// メール登録前の確認(TAG連携用)
    /// </summary>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/08/17</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "メール登録前の確認(TAG連携用)")]
    public DataSet CheckTagRenkeiMail(CondCommon cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                using (var impl = new WsCommonImpl())
                {
                    return impl.CheckTagRenkeiMail(dbHelper, cond);
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

    /// --------------------------------------------------
    /// <summary>
    /// メール登録前の確認(出荷計画用)
    /// </summary>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>Y.Gwon 2023/08/28</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "メール登録前の確認(出荷計画用)")]
    public DataSet CheckPlanningMail(CondCommon cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                using (var impl = new WsCommonImpl())
                {
                    return impl.CheckPlanningMail(dbHelper, cond);
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

    /// --------------------------------------------------
    /// <summary>
    /// 登録に必要なメールデータ取得
    /// </summary>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "登録に必要なメールデータ取得")]
    public DataTable GetMailData(CondCommon cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                using (var impl = new WsCommonImpl())
                {
                    var dt = impl.GetMailData(dbHelper, cond);
                    if (cond.MailKbn == MAIL_KBN.ARLIST_VALUE1
                        && dt.Rows.Count == 0)
                    {
                        cond.MailKbn = MAIL_KBN.COMMON_VALUE1;
                        dt = impl.GetMailData(dbHelper, cond);
                    }
                    return dt;
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

    /// --------------------------------------------------
    /// <summary>
    /// メールデータの登録
    /// </summary>
    /// <param name="cond">ユーザー情報</param>
    /// <param name="dt">登録データ</param>
    /// <param name="errMsgId">エラーメッセージ</param>
    /// <param name="args">エラーメッセージ引数</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "メールデータの登録")]
    public bool SaveMail(CondCommon cond, DataTable dt, out string errMsgId, out string[] args)
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
                using (var impl = new WsCommonImpl())
                {
                    ret = impl.SaveMail(dbHelper, cond, dt.Rows[0], ref errMsgId, ref args);
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
}

