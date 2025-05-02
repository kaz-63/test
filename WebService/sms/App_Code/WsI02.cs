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
/// 取込結果処理（部品管理用）クラス（トランザクション層）
/// </summary>
/// <create>T.Wakamatsu 2013/08/23</create>
/// <update></update>
/// --------------------------------------------------
[WebService(Description = "出荷管理システム", Namespace = "http://smssrv/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。 
// [System.Web.Script.Services.ScriptService]
public class WsI02 : WsBase
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsI02()
        : base()
    {
    }

    #endregion

    #region I0200010:取込エラー一覧

    #region 一時取込データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込データ取得
    /// </summary>
    /// <param name="cond">I02用コンディション</param>
    /// <returns>一時取込データ</returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "一時取込データ取得")]
    public DataSet GetTempwork(CondI02 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsI02Impl impl = new WsI02Impl())
                {
                    ds = impl.GetTempworkData(dbHelper, cond);
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

    #region ハンディデータ取込

    /// --------------------------------------------------
    /// <summary>
    /// ハンディデータ取込
    /// </summary>
    /// <param name="cond">I02用コンディション</param>
    /// <param name="ds">取込データ</param>
    /// <param name="dtMessage">メッセージテーブル</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ハンディデータ取込")]
    public bool ImportData(CondI02 cond, DataSet ds, ref DataTable dtMessage)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                bool ret = true;
                bool isSuccess = false;
                bool isImportError = false;

                bool excTana = false;
                bool excNyushu = false;
                CondI02 condTana = (CondI02)cond.Clone();
                CondI02 condNyush = (CondI02)cond.Clone();

                using (WsI02Impl impl = new WsI02Impl())
                {
                    // 取込日時セット
                    cond.TorikomiDate = DateTime.Now;

                    // ----- 採番処理 -----
                    // トランザクション開始
                    dbHelper.BeginTransaction();
                    isSuccess = impl.SetTempID(dbHelper, cond, ds, dtMessage);
                    if (isSuccess)
                    {
                        dbHelper.Commit();
                    }
                    else
                    {
                        dbHelper.Rollback();
                        return false;
                    }

                    // ----- 一時取込データ、一時取込明細データの登録 -----
                    // トランザクション開始
                    dbHelper.BeginTransaction();

                    isSuccess = impl.InsTempworkData(dbHelper, cond, ds);
                    if (isSuccess)
                    {
                        dbHelper.Commit();
                    }
                    else
                    {
                        dbHelper.Rollback();
                        return false;
                    }

                    // ----- 取込処理 -----
                    DataTable dt = ds.Tables[Def_T_BUHIN_TEMPWORK.Name].DefaultView.ToTable(true, Def_T_BUHIN_TEMPWORK.TEMP_ID, Def_T_BUHIN_TEMPWORK.TORIKOMI_FLAG);
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (ComFunc.GetFld(dr, Def_T_BUHIN_TEMPWORK.TORIKOMI_FLAG) == ZAIKO_TORIKOMI_FLAG.TANAOROSHI_VALUE1)
                        {
                            condTana.TempID = ComFunc.GetFld(dr, Def_T_BUHIN_TEMPWORK.TEMP_ID);
                            excTana = true;
                        }
                        else
                        {
                            condNyush.TempID = ComFunc.GetFld(dr, Def_T_BUHIN_TEMPWORK.TEMP_ID);
                            excNyushu = true;
                        }
                    }

                    // 取込処理はTEMP_ID単位でトランザクションをかける。
                    // トランザクション開始
                    dbHelper.BeginTransaction();
                    isSuccess = true;
                    if (excNyushu)
                    {
                        isSuccess &= impl.LockImportData(dbHelper, condNyush, dtMessage);
                    }
                    if (excTana)
                    {
                        isSuccess &= impl.LockImportData(dbHelper, condTana, dtMessage);
                    }
                    if (isSuccess)
                    {
                        dbHelper.Commit();
                    }
                    else
                    {
                        dbHelper.Rollback();
                        ret = false;
                        return ret;
                    }

                    if (excNyushu)
                    {
                        try
                        {
                            // 入出庫
                            isSuccess = this.ImportCheckAndRegist(dbHelper, condNyush, dtMessage, impl, ref isImportError);
                            if (!isSuccess)
                            {
                                ret = false;
                            }
                        }
                        catch (Exception exImport)
                        {
                            dbHelper.Rollback();
                            throw new Exception(exImport.Message, exImport);
                        }
                        finally
                        {
                            // トランザクション開始
                            dbHelper.BeginTransaction();
                            // 一時取込データの状態区分を処理が終わった状態とする。
                            isSuccess = impl.ImportEndStatus(dbHelper, condNyush);
                            if (isSuccess)
                            {
                                dbHelper.Commit();
                            }
                            else
                            {
                                dbHelper.Rollback();
                                ret = false;
                            }
                        }
                    }

                if (excTana)
                {
                    try
                    {
                        // 棚卸
                        isSuccess = this.ImportCheckAndRegist(dbHelper, condTana, dtMessage, impl, ref isImportError);
                        if (!isSuccess)
                        {
                            ret = false;
                        }
                    }
                    catch (Exception exImport)
                    {
                        dbHelper.Rollback();
                        throw new Exception(exImport.Message, exImport);
                    }
                    finally
                    {
                        // トランザクション開始
                        dbHelper.BeginTransaction();
                        // 一時取込データの状態区分を処理が終わった状態とする。
                        isSuccess = impl.ImportEndStatus(dbHelper, condTana);
                        if (isSuccess)
                        {
                            dbHelper.Commit();
                        }
                        else
                        {
                            dbHelper.Rollback();
                            ret = false;
                        }
                    }
                }
                }
                if (isImportError)
                {
                    // 取込めない物があった場合戻り値をfalseにする。
                    ret = false;
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

    #region ハンディデータ取込再試行

    /// --------------------------------------------------
    /// <summary>
    /// ハンディデータ取込再試行
    /// </summary>
    /// <param name="cond">I02用コンディション</param>
    /// <param name="ds">取込データ</param>
    /// <param name="dtMessage">メッセージテーブル</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ハンディデータ取込再試行")]
    public bool ImportRetry(CondI02 cond, ref DataTable dtMessage)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                bool ret = true;
                bool isSuccess = false;
                bool isImportError = false;

                using (WsI02Impl impl = new WsI02Impl())
                {
                    // 取込処理はTEMP_ID単位でトランザクションをかける。
                    // トランザクション開始
                    dbHelper.BeginTransaction();

                    isSuccess = impl.LockImportData(dbHelper, cond, dtMessage);
                    if (isSuccess)
                    {
                        dbHelper.Commit();
                    }
                    else
                    {
                        dbHelper.Rollback();
                        ret = false;
                        return false;
                    }
                    try
                    {
                        // 取込データのデータチェック処理
                        isSuccess = this.ImportCheckAndRegist(dbHelper, cond, dtMessage, impl, ref isImportError);
                        if (!isSuccess)
                        {
                            ret = false;
                        }
                    }
                    catch (Exception exImport)
                    {
                        dbHelper.Rollback();
                        throw new Exception(exImport.Message, exImport);
                    }
                    finally
                    {
                        // トランザクション開始
                        dbHelper.BeginTransaction();
                        // 一時取込データの状態区分を処理が終わった状態とする。
                        isSuccess = impl.ImportEndStatus(dbHelper, cond);
                        if (isSuccess)
                        {
                            dbHelper.Commit();
                        }
                        else
                        {
                            dbHelper.Rollback();
                            ret = false;
                        }
                    }
                }
                if (isImportError)
                {
                    // 取込めない物があった場合戻り値をfalseにする。
                    ret = false;
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

    #region ハンディデータ破棄

    /// --------------------------------------------------
    /// <summary>
    /// ハンディデータ破棄
    /// </summary>
    /// <param name="cond"></param>
    /// <param name="dtMessage"></param>
    /// <returns></returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ハンディデータ破棄")]
    public bool DestroyData(CondI02 cond, ref DataTable dtMessage)
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
                using (WsI02Impl impl = new WsI02Impl())
                {
                    ret = impl.DestroyData(dbHelper, cond, dtMessage);
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

    #region 取込＆再試行の制御部分(トランザクション層)

    /// --------------------------------------------------
    /// <summary>
    /// 取込＆再試行の制御部分(トランザクション層)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I02用コンディション</param>
    /// <param name="dtMessage">メッセージテーブル</param>
    /// <param name="impl">I02実装クラスインスタンス</param>
    /// <param name="isImportError">取込時にエラーがあったかどうか</param>
    /// <returns></returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update>K.Tsutsumi 2018/09/05</update>
    /// <update></update>
    /// --------------------------------------------------
    public bool ImportCheckAndRegist(DatabaseHelper dbHelper, CondI02 cond, DataTable dtMessage, WsI02Impl impl, ref bool isImportError)
    {
        bool ret = true;
        bool isSuccess = false;
        // 取込データのデータチェック処理
        // トランザクション開始
        dbHelper.BeginTransaction();

        isSuccess = impl.ImportCheckData(dbHelper, cond, dtMessage, ref isImportError);
        if (isSuccess)
        {
            dbHelper.Commit();
        }
        else
        {
            dbHelper.Rollback();
            ret = false;
            return false;
        }

        // データ取込、チェック後の結果を各テーブルに反映させる。
        // トランザクション開始
        dbHelper.BeginTransaction();
        // 取込結果反映(他テーブル)
        DataTable dtErr = new DataTable();
        isSuccess = impl.ImportResultUpdateOtherTable(dbHelper, cond, dtMessage, ref dtErr);
        
        // チェック内容でエラーがあれば更新しない
        if (isSuccess && !isImportError)
        {
            dbHelper.Commit();
        }
        else
        {
            dbHelper.Rollback();
            ret = false;
        }

        if (dtErr.Rows.Count > 0)
        {
            // トランザクション開始
            dbHelper.BeginTransaction();

            // 別途宣言
            CondI02 condMsg = new CondI02(cond.LoginInfo);

            // Locationに存在しません。
            condMsg.MessageID = "I0200010006";
            string message = impl.GetMessage(dbHelper, cond);
            impl.UpdTempworkMeisaiError(dbHelper, dtErr, message);
            dbHelper.Commit();
        }
        return ret;
    }

    #endregion

    #endregion

    #region 取込エラー詳細

    #region 一時取込明細データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込明細データ取得
    /// </summary>
    /// <param name="cond">I02用コンディション</param>
    /// <returns>一時取込明細データ</returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "一時取込明細データ取得")]
    public DataSet GetTempworkMeisai(CondI02 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsI02Impl impl = new WsI02Impl())
                {
                    ds = impl.GetTempworkMeisai(dbHelper, cond);
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

    #region 一時取込明細データ削除

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込明細データ削除
    /// </summary>
    /// <param name="cond">I02用コンディション</param>
    /// <param name="dt">削除データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Wakamatsu 2013/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "一時取込明細データ削除")]
    public bool DelTempWorkMeisai(CondI02 cond, DataTable dt, out string errMsgID, out string[] args)
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
                using (WsI02Impl impl = new WsI02Impl())
                {
                    ret = impl.DelTempWorkMeisai(dbHelper, cond, dt);
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


}

