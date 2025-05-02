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
/// 取込結果処理クラス（トランザクション層）
/// </summary>
/// <create>Y.Higuchi 2010/06/22</create>
/// <update></update>
/// --------------------------------------------------
[WebService(Description = "出荷管理システム", Namespace = "http://smssrv/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。 
// [System.Web.Script.Services.ScriptService]
public class WsK04 : WsBase
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/06/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsK04()
        : base()
    {
    }

    #endregion

    #region K0400010:取込エラー一覧

    #region 一時取込データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込データ取得
    /// </summary>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>一時取込データ</returns>
    /// <create>Y.Higuchi 2010/08/10</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "一時取込データ取得")]
    public DataSet GetTempwork(CondK04 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsK04Impl impl = new WsK04Impl())
                {
                    ds = impl.GetTempwork(dbHelper, cond);
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
    /// <param name="cond">K04用コンディション</param>
    /// <param name="ds">取込データ</param>
    /// <param name="dtMessage">メッセージテーブル</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ハンディデータ取込")]
    public bool ImportData(CondK04 cond, DataSet ds, ref DataTable dtMessage)
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
                using (WsK04Impl impl = new WsK04Impl())
                {
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
                    foreach (DataRow dr in ds.Tables[Def_T_TEMPWORK.Name].Rows)
                    {
                        cond.TempID = ComFunc.GetFld(dr, Def_T_TEMPWORK.TEMP_ID);

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
                            continue;
                        }
                        try
                        {
                            bool isDuplicationError;
                            isSuccess = this.ImportCheckAndRegist(dbHelper, cond, dtMessage, impl, ref isImportError, out isDuplicationError);
                            if (!isSuccess && !isDuplicationError)
                            {
                                ret = false;
                            }
                            if (isDuplicationError)
                            {
                                // 一意制約違反が発生したので再度チェックからやり直す
                                isSuccess = this.ImportCheckAndRegist(dbHelper, cond, dtMessage, impl, ref isImportError, out isDuplicationError);
                                if (!isSuccess)
                                {
                                    ret = false;
                                }
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

    #region ハンディデータ取込再試行

    /// --------------------------------------------------
    /// <summary>
    /// ハンディデータ取込再試行
    /// </summary>
    /// <param name="cond">K04用コンディション</param>
    /// <param name="ds">取込データ</param>
    /// <param name="dtMessage">メッセージテーブル</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ハンディデータ取込再試行")]
    public bool ImportRetry(CondK04 cond, ref DataTable dtMessage)
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

                using (WsK04Impl impl = new WsK04Impl())
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
                        bool isDuplicationError;
                        isSuccess = this.ImportCheckAndRegist(dbHelper, cond, dtMessage, impl, ref isImportError, out isDuplicationError);
                        if (!isSuccess && !isDuplicationError)
                        {
                            ret = false;
                        }
                        if (isDuplicationError)
                        {
                            // 一意制約違反が発生したので再度チェックからやり直す
                            isSuccess = this.ImportCheckAndRegist(dbHelper, cond, dtMessage, impl, ref isImportError, out isDuplicationError);
                            if (!isSuccess)
                            {
                                ret = false;
                            }
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

    #region ハンディデータ破棄

    /// --------------------------------------------------
    /// <summary>
    /// ハンディデータ破棄
    /// </summary>
    /// <param name="cond"></param>
    /// <param name="dtMessage"></param>
    /// <returns></returns>
    /// <create>Y.Higuchi 2010/08/24</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ハンディデータ破棄")]
    public bool DestroyData(CondK04 cond, ref DataTable dtMessage)
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
                using (WsK04Impl impl = new WsK04Impl())
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

    #region 取込＆再試行の制御部分(トランザクション層)

    /// --------------------------------------------------
    /// <summary>
    /// 取込＆再試行の制御部分(トランザクション層)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <param name="dtMessage">メッセージテーブル</param>
    /// <param name="impl">K04実装クラスインスタンス</param>
    /// <param name="isImportError">取込時にエラーがあったかどうか</param>
    /// <param name="isDuplicationError">一意制約違反があったかどうか</param>
    /// <returns></returns>
    /// <create>Y.Higuchi 2010/08/24</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool ImportCheckAndRegist(DatabaseHelper dbHelper, CondK04 cond, DataTable dtMessage, WsK04Impl impl, ref bool isImportError, out bool isDuplicationError)
    {
        bool ret = true;
        bool isSuccess = false;
        isDuplicationError = false;
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
        isSuccess = impl.ImportResultUpdateOtherTable(dbHelper, cond, dtMessage, out isDuplicationError);
        if (isSuccess)
        {
            dbHelper.Commit();
        }
        else
        {
            dbHelper.Rollback();
            ret = false;
        }
        return ret;
    }

    #endregion

    #endregion

    #region 取込エラー詳細

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込明細データ取得
    /// </summary>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>一時取込明細データ</returns>
    /// <create>Y.Higuchi 2010/08/10</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "一時取込明細データ取得")]
    public DataSet GetTempworkMeisai(CondK04 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsK04Impl impl = new WsK04Impl())
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

    #region K0400050:取込エラー詳細(検品)

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込明細データ(検品)取得
    /// </summary>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>一時取込明細(検品)データ</returns>
    /// <create>H.Tsuji 2020/06/17</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "一時取込明細データ(検品)取得")]
    public DataSet GetTempworkMeisaiKenpin(CondK04 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsK04Impl impl = new WsK04Impl())
                {
                    ds = impl.GetTempworkMeisaiKenpin(dbHelper, cond);
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
    /// OK分の検品データ取込を再試行する
    /// </summary>
    /// <param name="cond">K04用コンディション</param>
    /// <param name="ds">取込データ</param>
    /// <param name="dtMessage">メッセージテーブル</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2020/06/17</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "OK分の検品データ取込を再試行する")]
    public bool ImportRetryKenpin(CondK04 cond, DataSet ds, ref DataTable dtMessage)
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
                using (WsK04Impl impl = new WsK04Impl())
                {
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

                    // ----- 旧一時取込テーブルロック処理 -----
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
                        return false;
                    }

                    try
                    {
                        // ----- 検品OK分の一時取込データ、一時取込明細データの登録 -----
                        // トランザクション開始
                        dbHelper.BeginTransaction();
                        isSuccess = impl.InsTempworkDataKenpinOK(dbHelper, cond, ds);
                        if (isSuccess)
                        {
                            dbHelper.Commit();
                        }
                        else
                        {
                            dbHelper.Rollback();
                            return false;
                        }
                    }
                    catch (Exception exInsert)
                    {
                        dbHelper.Rollback();
                        throw new Exception(exInsert.Message, exInsert);
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
                    if (!ret) return ret;

                    // 採番した一時取込IDをセットする
                    cond.TempID = ComFunc.GetFld(ds.Tables[Def_T_TEMPWORK.Name], 0, Def_T_TEMPWORK.TEMP_ID);

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
                        return false;
                    }

                    try
                    {
                        // ----- 取込データのデータチェック処理 -----
                        bool isDuplicationError;
                        isSuccess = this.ImportCheckAndRegist(dbHelper, cond, dtMessage, impl, ref isImportError, out isDuplicationError);
                        if (!isSuccess && !isDuplicationError)
                        {
                            ret = false;
                        }
                        if (isDuplicationError)
                        {
                            // 一意制約違反が発生したので再度チェックからやり直す
                            isSuccess = this.ImportCheckAndRegist(dbHelper, cond, dtMessage, impl, ref isImportError, out isDuplicationError);
                            if (!isSuccess)
                            {
                                ret = false;
                            }
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

    #region K0400070:Handy操作履歴照会

    #region 初期化データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 初期化データ取得
    /// </summary>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>コンボボックスなどの初期化データ</returns>
    /// <create>H.Tajimi 2019/08/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "初期化データ取得")]
    public DataSet GetInitHandyOpeRirekiShokai(CondK04 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsK04Impl impl = new WsK04Impl())
                {
                    ds = impl.GetInitHandyOpeRirekiShokai(dbHelper, cond);
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

    #region 表示データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 表示データ取得
    /// </summary>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>表示データ取得</returns>
    /// <create>H.Tajimi 2019/08/26</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "表示データ取得")]
    public DataSet GetHandyOpeRireki(CondK04 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsK04Impl impl = new WsK04Impl())
                {
                    ds = impl.GetHandyOpeRireki(dbHelper, cond);
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

