using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;

using Condition;
using Commons;
using DSWUtil.DbUtil;
using DSWUtil;

/// --------------------------------------------------
/// <summary>
/// 常駐処理クラス（トランザクション層）
/// </summary>
/// <create>Y.Higuchi 2010/06/22</create>
/// <update></update>
/// --------------------------------------------------
[WebService(Description = "出荷管理システム", Namespace = "http://smssrv/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。 
// [System.Web.Script.Services.ScriptService]
public class WsJ01 : WsBase
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/06/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsJ01()
        : base()
    {
    }

    #endregion

    #region J0100020:日次処理

    #region 締めマスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// 締めマスタ取得
    /// </summary>
    /// <returns>締めマスタ</returns>
    /// <create>Y.Higuchi 2010/08/27</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "締めマスタ取得")]
    public DataSet GetShime()
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsJ01Impl impl = new WsJ01Impl())
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

    #region 締め開始処理

    /// --------------------------------------------------
    /// <summary>
    /// 締め開始処理
    /// </summary>
    /// <param name="ds">締め処理マスタ</param>
    /// <param name="isProcessed">処理中かどうか</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "締め開始処理")]
    public bool StartShime(out DataSet ds, out bool isProcessed)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                ds = null;
                isProcessed = false;
                // 接続
                this.DbOpen(dbHelper);

                // トランザクション開始
                dbHelper.BeginTransaction();

                // 実行
                bool ret = false;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    ret = impl.StartShime(dbHelper, ref ds, ref isProcessed);
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

    #region 締めマスタ更新処理

    /// --------------------------------------------------
    /// <summary>
    /// 締めマスタ更新処理
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "締めマスタ更新処理")]
    public bool UpdShime(CondJ01 cond)
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
                int record = 0;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    record = impl.UpdShime(dbHelper, cond);
                }

                bool ret = true;
                if (0 < record)
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

    #region 本体の納入先マスタ完了処理

    /// --------------------------------------------------
    /// <summary>
    /// 本体の納入先マスタ完了処理
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "本体の納入先マスタ完了処理")]
    public bool UpdHontaiNonyusakiKanryo(CondJ01 cond)
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
                int record = 0;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    record = impl.UpdHontaiNonyusakiKanryo(dbHelper, cond);
                }

                bool ret = true;
                if (0 < record)
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

    #region 本体の納入先マスタ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 本体の納入先マスタ削除処理
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "本体の納入先マスタ削除処理")]
    public bool DelHontaiNonyusakiData(CondJ01 cond)
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
                int record = 0;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    record = impl.DelHontaiNonyusaki(dbHelper, cond);
                }

                bool ret = true;
                if (0 < record)
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

    #region 本体の納入先マスタ削除処理(指定年で無条件)

    /// --------------------------------------------------
    /// <summary>
    /// 本体の納入先マスタ削除処理(指定年で無条件)
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Sakiori 2015/06/11</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "本体の納入先マスタ削除処理(指定年で無条件)")]
    public bool DelHontaiNonyusakiDataUncondition(CondJ01 cond)
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
                int record = 0;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    record = impl.DelHontaiNonyusakiUncondition(dbHelper, cond);
                }

                bool ret = true;
                if (0 < record)
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

    #region 本体の物件名マスタ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 本体の物件名マスタ削除処理
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/04/19</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "本体の物件名マスタ削除処理")]
    public bool DelHontaiBukkenData(CondJ01 cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                dbHelper.BeginTransaction();

                int record = 0;
                using (var impl = new WsJ01Impl())
                {
                    record = impl.DelHontaiBukken(dbHelper, cond);
                }

                bool ret = true;
                if (0 < record)
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
            catch (Exception ex)
            {
                dbHelper.Rollback();
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                dbHelper.Close();
            }
        }
    }

    #endregion

    #region 本体の出荷明細データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 本体の出荷明細データ削除処理
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "本体の出荷明細データ削除処理")]
    public bool DelHontaiShukkaMeisaiData(CondJ01 cond)
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
                int record = 0;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    record = impl.DelHontaiShukkaMeisai(dbHelper, cond);
                }

                bool ret = true;
                if (0 < record)
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

    #region ARの納入先マスタ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// ARの納入先マスタ削除処理
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ARの納入先マスタ削除処理")]
    public bool DelARNonyusakiData(CondJ01 cond)
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
                int record = 0;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    record = impl.DelARNonyusaki(dbHelper, cond);
                }

                bool ret = true;
                if (0 < record)
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

    #region ARの納入先マスタ削除処理(指定年で無条件)

    /// --------------------------------------------------
    /// <summary>
    /// ARの納入先マスタ削除処理(指定年で無条件)
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Sakiori 2015/06/11</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ARの納入先マスタ削除処理(指定年で無条件)")]
    public bool DelARNonyusakiDataUncondition(CondJ01 cond)
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
                int record = 0;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    record = impl.DelARNonyusakiUncondition(dbHelper, cond);
                }

                bool ret = true;
                if (0 < record)
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

    #region ARの物件名マスタ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// ARの物件名マスタ削除処理
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/04/19</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ARの物件名マスタ削除処理")]
    public bool DelARBukkenData(CondJ01 cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                dbHelper.BeginTransaction();

                int record = 0;
                using (var impl = new WsJ01Impl())
                {
                    record = impl.DelARBukken(dbHelper, cond);
                }

                bool ret = true;
                if (0 < record)
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
            catch (Exception ex)
            {
                dbHelper.Rollback();
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                dbHelper.Close();
            }
        }
    }

    #endregion

    #region AR情報データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// AR情報データ削除処理
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <param name="ds">削除対象データ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "AR情報データ削除処理")]
    public bool DelARData(CondJ01 cond, out DataSet ds)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                ds = new DataSet();
                // 接続
                this.DbOpen(dbHelper);

                // トランザクション開始
                dbHelper.BeginTransaction();

                // 実行
                bool ret = true;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    ret = impl.DelARData(dbHelper, cond, ref ds);
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

    #region ARの出荷明細データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// ARの出荷明細データ削除処理
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ARの出荷明細データ削除処理")]
    public bool DelARShukkaMeisaiData(CondJ01 cond)
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
                int record = 0;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    record = impl.DelARShukkaMeisai(dbHelper, cond);
                }

                bool ret = true;
                if (0 < record)
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

    #region BOXリスト管理データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// BOXリスト管理データ削除処理
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "BOXリスト管理データ削除処理")]
    public bool DelBoxlistManageData(CondJ01 cond)
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
                int record = 0;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    record = impl.DelBoxlistManage(dbHelper, cond);
                }

                bool ret = true;
                if (0 < record)
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

    #region パレットリスト管理データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// パレットリスト管理データ削除処理
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "パレットリスト管理データ削除処理")]
    public bool DelPalletlistManageData(CondJ01 cond)
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
                int record = 0;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    record = impl.DelPalletlistManage(dbHelper, cond);
                }

                bool ret = true;
                if (0 < record)
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

    #region 木枠データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 木枠データ削除処理
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <param name="ds">削除対象データ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "木枠データ削除処理")]
    public bool DelKiwakuData(CondJ01 cond, out DataSet ds)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                ds = new DataSet();
                // 接続
                this.DbOpen(dbHelper);

                // トランザクション開始
                dbHelper.BeginTransaction();

                // 実行
                bool ret = true;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    ret = impl.DelKiwakuData(dbHelper, cond, ref ds);
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

    #region 木枠明細データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 木枠明細データ削除処理
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "木枠明細データ削除処理")]
    public bool DelKiwakuMeisaiData(CondJ01 cond)
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
                int record = 0;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    record = impl.DelKiwakuMeisai(dbHelper, cond);
                }

                bool ret = true;
                if (0 < record)
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

    #region 社外木枠明細データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 社外木枠明細データ削除処理
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "社外木枠明細データ削除処理")]
    public bool DelShagaiKiwakuMeisaiData(CondJ01 cond)
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
                int record = 0;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    record = impl.DelShagaiKiwakuMeisai(dbHelper, cond);
                }

                bool ret = true;
                if (0 < record)
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

    #region 採番マスタ削除処理(ARNo.)

    /// --------------------------------------------------
    /// <summary>
    /// 採番マスタ削除処理(ARNo.)
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/09/13</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "採番マスタ削除処理(ARNo.)")]
    public bool DelSaibanARNo(CondJ01 cond)
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
                int record = 0;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    record = impl.DelSaibanARNo(dbHelper);
                }

                bool ret = true;
                if (0 < record)
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

    #region 一時作業データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 一時作業データ削除処理
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "一時作業データ削除処理")]
    public bool DelTempworkData(CondJ01 cond)
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
                bool ret = true;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    ret = impl.DelTempworkData(dbHelper, cond);
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

    #region 本体の履歴データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 本体の履歴データ削除処理
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/05/10</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "本体の履歴データ削除処理")]
    public bool DelHontaiRirekiData(CondJ01 cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                dbHelper.BeginTransaction();

                int record = 0;
                using (var impl = new WsJ01Impl())
                {
                    record = impl.DelHontaiRireki(dbHelper, cond);
                }

                bool ret = true;
                if (0 < record)
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
            catch (Exception ex)
            {
                dbHelper.Rollback();
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                dbHelper.Close();
            }
        }
    }

    #endregion

    #region ARの履歴データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// ARの履歴データ削除処理
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/05/11</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ARの履歴データ削除処理")]
    public bool DelARRirekiData(CondJ01 cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                dbHelper.BeginTransaction();

                int record = 0;
                using (var impl = new WsJ01Impl())
                {
                    record = impl.DelARRireki(dbHelper, cond);
                }

                bool ret = true;
                if (0 < record)
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
            catch (Exception ex)
            {
                dbHelper.Rollback();
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                dbHelper.Close();
            }
        }
    }

    #endregion

    #region ロケーションマスタ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// ロケーションマスタ削除処理
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/09/03</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ロケーションマスタ削除処理")]
    public bool DelLocationData(CondJ01 cond)
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
                int record = 0;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    record = impl.DelLocation(dbHelper);
                }

                bool ret = true;
                if (0 < record)
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

    #region 在庫データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 在庫データ削除処理
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/09/03</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "在庫データ削除処理")]
    public bool DelStockData(CondJ01 cond)
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
                int record = 0;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    record = impl.DelStock(dbHelper);
                }

                bool ret = true;
                if (0 < record)
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

    #region 棚卸データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 棚卸データ削除処理
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/09/03</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "棚卸データ削除処理")]
    public bool DelInventData(CondJ01 cond)
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
                int record = 0;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    record = impl.DelInvent(dbHelper);
                }

                bool ret = true;
                if (0 < record)
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

    #region 実績データ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 実績データ削除処理
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/09/03</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "実績データ削除処理")]
    public bool DelJissekiData(CondJ01 cond)
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
                int record = 0;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    record = impl.DelJisseki(dbHelper);
                }

                bool ret = true;
                if (0 < record)
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

    #region 一時作業データ（現地部品管理）削除処理

    /// --------------------------------------------------
    /// <summary>
    /// 一時作業データ（現地部品管理）削除処理
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/09/03</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "一時作業データ（現地部品管理）削除処理")]
    public bool DelBuhinTempworkData(CondJ01 cond)
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
                bool ret = true;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    ret = impl.DelBuhinTempworkData(dbHelper, cond);
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

    #region DBバックアップ処理

    /// --------------------------------------------------
    /// <summary>
    /// DBバックアップ処理
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "DBバックアップ処理")]
    public bool DBBackup(CondJ01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                bool ret = true;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    ret = impl.DBBackup(dbHelper, cond);
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

    #region メールデータ削除処理

    /// --------------------------------------------------
    /// <summary>
    /// メールデータ削除処理
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/27</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "メールデータ削除処理")]
    public bool DelMail(CondJ01 cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                this.DbOpen(dbHelper);

                dbHelper.BeginTransaction();

                bool ret = true;
                using (var impl = new WsJ01Impl())
                {
                    ret = impl.DelMail(dbHelper, cond);
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
                dbHelper.Close();
            }
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
    /// <returns></returns>
    /// <create>R.Katsuo 2017/09/07</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "メール設定マスタ取得")]
    public DataSet GetMailSetting()
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    ds = impl.GetMailSetting(dbHelper);
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

    #region メールデータ取得

    /// --------------------------------------------------
    /// <summary>
    /// メールデータ取得
    /// </summary>
    /// <returns></returns>
    /// <create>R.Katsuo 2017/09/07</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "メールデータ取得")]
    public DataSet GetMail()
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    ds = impl.GetMail(dbHelper);
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

    #region メールデータ更新処理

    /// --------------------------------------------------
    /// <summary>
    /// メールデータ更新処理
    /// </summary>
    /// <param name="dt"></param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>R.Katsuo 2017/09/07</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "メールデータ更新処理")]
    public bool UpdMail(DataTable dt)
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
                int record = 0;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    record = impl.UpdMail(dbHelper, dt, DateTime.Now);
                }

                bool ret = true;
                if (0 < record)
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

    #region J0100040:SKS連携処理

    #region 初期化処理

    /// --------------------------------------------------
    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <returns>SKS連携マスタ/SKS手配明細WORKスキーマ</returns>
    /// <create>H.Tajimi 2018/11/15</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "初期化取得")]
    public DataSet GetInit()
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    ds = impl.GetSKS(dbHelper);
                    ds.Merge(impl.GetTehaiSKSWorkScheme(dbHelper));
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

    #region SKS連携データ取得処理

    /// --------------------------------------------------
    /// <summary>
    /// SKS連携データ取得処理
    /// </summary>
    /// <returns>SKS連携マスタ/SKS手配明細WORKスキーマ</returns>
    /// <create>H.Tajimi 2018/11/15</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "SKS連携データ取得処理")]
    public DataSet GetSKS()
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    ds = impl.GetSKS(dbHelper);
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

    #region SKS連携開始処理

    /// --------------------------------------------------
    /// <summary>
    /// SKS連携開始処理
    /// </summary>
    /// <param name="ds">SKS連携マスタ</param>
    /// <param name="isProcessed">処理中かどうか</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tajimi 2018/11/15</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "SKS連携開始処理")]
    public bool StartSKS(out DataSet ds, out bool isProcessed)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                ds = null;
                isProcessed = false;
                // 接続
                this.DbOpen(dbHelper);

                // トランザクション開始
                dbHelper.BeginTransaction();

                // 実行
                bool ret = false;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    ret = impl.StartSKS(dbHelper, ref ds, ref isProcessed);
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

    #region SKS連携マスタ更新処理

    /// --------------------------------------------------
    /// <summary>
    /// SKS連携マスタ更新処理
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tajimi 2018/11/15</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "SKS連携マスタ更新処理")]
    public bool UpdSKS(CondJ01 cond)
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
                int record = 0;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    record = impl.UpdSKS(dbHelper, cond);
                }

                bool ret = true;
                if (0 < record)
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

    #region SKS手配明細WORK 削除処理(全レコード削除)

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細WORK 削除処理(全レコード削除)
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/22</create>
    /// <update>K.Tsutsumi 2018/01/24 分割アップロード</update>
    /// --------------------------------------------------
    [WebMethod(Description = "SKS手配明細WORK 削除処理(全レコード削除)")]
    public bool DelTehaiSKSWork(CondJ01 cond, out int count)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                count = 0;

                // 接続
                this.DbOpen(dbHelper);

                // トランザクション開始
                dbHelper.BeginTransaction();

                // 実行
                bool ret = false;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    // SKS手配明細WORKデータ削除実行
                    count = impl.TruncateTehaiSKSWork(dbHelper);
                    // SKS手配明細WORKデータ件数確認
                    ret = impl.IsEmptyTehaiSKSWork(dbHelper);
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

    #region SKS手配明細WORK 登録処理

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細WORK 登録処理
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <param name="dt">DataTable(CSVから読み込んだデータ)</param>
    /// <param name="ds">DataSet(エラーメッセージなど)</param>
    /// <returns></returns>
    /// <create>K.Tsutsumi 2018/01/24</create>
    /// --------------------------------------------------
    [WebMethod(Description = "SKS手配明細WORK 登録処理")]
    public bool InsTehaiSKSWork(CondJ01 cond, DataTable dt, out DataSet ds)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                ds = null;
                // 接続
                this.DbOpen(dbHelper);

                // トランザクション開始
                dbHelper.BeginTransaction();

                // 実行
                bool ret = false;
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    // SKS手配明細WORKデータ件数確認
                    ret = impl.InsTehaiSKSWork(dbHelper, cond, dt, ref ds);
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

    #region SKS手配明細WORK 更新処理(回答納期が「11111111」のときは完納とする)

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細WORK 更新処理(回答納期が「11111111」のときは完納とする)
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <param name="count">影響を与えた行数</param>
    /// <returns></returns>
    /// <create>K.Tsutsumi 2018/01/24</create>
    /// --------------------------------------------------
    [WebMethod(Description = "SKS手配明細WORK 更新処理(回答納期が「11111111」のときは完納とする)")]
    public bool UpdSKSTehaiWorkHacchuZyotai(CondJ01 cond, out int count)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                count = 0;

                // 接続
                this.DbOpen(dbHelper);

                // トランザクション開始
                dbHelper.BeginTransaction();

                // 実行
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    count = impl.UpdSKSTehaiWorkHacchuZyotai(dbHelper, cond);
                }

                dbHelper.Commit();
                return true;
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

    #region SKS手配明細WORK 更新処理(見積状態のときは単価を0円にする)

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細WORK 更新処理(見積状態のときは単価を0円にする)
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <param name="count">影響を与えた行数</param>
    /// <returns></returns>
    /// <create>K.Tsutsumi 2018/01/24</create>
    /// --------------------------------------------------
    [WebMethod(Description = "SKS手配明細WORK 更新処理(見積状態のときは単価を0円にする)")]
    public bool UpdSKSTehaiWorkTehaiUnitPrice(CondJ01 cond, out int count)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                count = 0;

                // 接続
                this.DbOpen(dbHelper);

                // トランザクション開始
                dbHelper.BeginTransaction();

                // 実行
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    count = impl.UpdSKSTehaiWorkTehaiUnitPrice(dbHelper, cond);
                }

                dbHelper.Commit();
                return true;
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

    #region SKS手配明細更新

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細更新
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <param name="ds">DataSet(エラーメッセージなど)</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "SKS手配明細更新")]
    public bool UpdTehaiSKS(CondJ01 cond, out DataSet ds)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                ds = new DataSet();
                // 実行
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    // エラーメッセージ用のスキーマ取得
                    var dtMessage = impl.GetErrorMessageScheme();

                    // SKS手配明細WORK取得
                    var dtTehaiSKS = impl.GetTehaiSKSWork(dbHelper);
                    if (UtilData.ExistsData(dtTehaiSKS))
                    {
                        // トランザクション開始
                        dbHelper.BeginTransaction();

                        foreach (DataRow drTehaiSKS in dtTehaiSKS.Rows)
                        {
                            bool ret = false;
                            var tehaiNo = ComFunc.GetFld(drTehaiSKS, Def_T_TEHAI_SKS.TEHAI_NO);
                            try
                            {
                                // SKS手配明細の行ロック
                                var dtTmp = impl.LockTehaiSKS(dbHelper, drTehaiSKS);
                                if (!UtilData.ExistsData(dtTmp))
                                {
                                    impl.AddErrorMessage(dtMessage, "SKS手配明細は他端末で処理中です。(手配No:{0})", tehaiNo);
                                    continue;
                                }

                                // SKS手配明細の更新
                                if (impl.UpdTehaiSKS(dbHelper, drTehaiSKS) != 1)
                                {
                                    impl.AddErrorMessage(dtMessage, "SKS手配明細の更新に失敗しました。(手配No:{0})", tehaiNo);
                                    continue;
                                }
                                ret = true;
                            }
                            catch (Exception ex)
                            {
                                impl.AddErrorMessage(dtMessage, "SKS手配明細の更新に失敗しました。(手配No:{0}、({1}))", tehaiNo, ex.Message);
                            }
                            finally
                            {
                                if (ret)
                                {
                                    dbHelper.CommitAndBeginTransaction();
                                }
                                else
                                {
                                    dbHelper.RollbackAndBeginTransaction();
                                }
                            }
                        }

                        // トランザクション終了
                        dbHelper.Commit();
                    }
                    ds.Merge(dtMessage);
                    return true;
                }
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

    #region SKS手配明細登録

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細登録
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <param name="ds">DataSet(エラーメッセージなど)</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/22</create>
    /// <update>H.Tajimi 2019/08/22 手配連携Noと手配Noが１対１でない場合は自動で連携させない</update>
    /// <update>T.Nukaga 2021/12/08 STEP14 検索条件を分割して段階的にSKS連携処理を実施するように変更</update>
    /// <update>T.Nukaga 2021/12/22 STEP14 SKS連携処理済データか判定して複数回処理しないように修正</update>
    /// --------------------------------------------------
    [WebMethod(Description = "SKS手配明細登録")]
    public bool InsTehaiSKS(CondJ01 cond, out DataSet ds)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                ds = new DataSet();

                // 実行
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    // エラーメッセージ用のスキーマ取得
                    var dtMessage = impl.GetErrorMessageScheme();

                    // 検索条件を分けて順番に実施
                    foreach (ComDefine.SKSRenkeiZyoken zyoken in Enum.GetValues(typeof(ComDefine.SKSRenkeiZyoken)))
                    {

                        // SKS手配明細＋技連マスタ取得
                        var dtTehaiMeisaiAndEcs = impl.GetTehaiMeisaiAndEcs(dbHelper, zyoken);
                        if (UtilData.ExistsData(dtTehaiMeisaiAndEcs))
                        {
                            // トランザクション開始
                            dbHelper.BeginTransaction();

                            var serverTime = DateTime.Now;

                            foreach (DataRow drTehaiMeisaiAndEcs in dtTehaiMeisaiAndEcs.Rows)
                            {
                                bool ret = false;
                                var tehaiRenkeiNo = ComFunc.GetFld(drTehaiMeisaiAndEcs, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO);
                                var tehaiNo = ComFunc.GetFld(drTehaiMeisaiAndEcs, Def_T_TEHAI_SKS_WORK.TEHAI_NO);
                                try
                                {
                                    // 製番、図面追番、手配数量などの情報が全て同じで異なる手配Noのものが存在した場合
                                    // TEHAI_RENKEI_NO：TEHAI_NO が N：1になるので自動では連携させない
                                    if (dtTehaiMeisaiAndEcs.AsEnumerable().Count(x => x.Field<string>(Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO) == tehaiRenkeiNo) > 1)
                                    {
                                        impl.AddErrorMessage(dtMessage, "{2} 複数の手配が存在するため自動連携はスキップします。(手配連携No:{0}、手配No:{1})", tehaiRenkeiNo, tehaiNo, zyoken.ToString());
                                        continue;
                                    }

                                    // 登録しようとしている手配Noが既に他の手配Noと連携済の場合
                                    // TEHAI_RENKEI_NO：TEHAI_NO が 1：Nになるので自動では連携させない
                                    var dtTehaiMeisaiSKS = impl.GetTehaiMeisaiSKS(dbHelper, tehaiNo);
                                    if (UtilData.ExistsData(dtTehaiMeisaiSKS))
                                    {
                                        impl.AddErrorMessage(dtMessage, "{1} 既に連携済です。(手配No:{0})", tehaiNo, zyoken.ToString());
                                        continue;
                                    }

                                    // 手配明細の行ロック
                                    var dtTmp = impl.LockTehaiMeisai(dbHelper, drTehaiMeisaiAndEcs);
                                    if (!UtilData.ExistsData(dtTmp))
                                    {
                                        impl.AddErrorMessage(dtMessage, "{1} 手配明細は他端末で処理中です。(手配連携No:{0})", tehaiRenkeiNo, zyoken.ToString());
                                        continue;
                                    }

                                    // SKS手配明細の存在チェック：画面で連携削除後、夜間で連携が行われた場合に紐づけが成功しない場合に対応
                                    if (impl.IsExistsTehaiSKS(dbHelper, tehaiNo))
                                    {
                                        impl.AddErrorMessage(dtMessage, "SKS手配明細は既に登録済のためSkipします。(手配No:{0})", tehaiNo);
                                    }
                                    else
                                    {
                                        // SKS手配明細の登録
                                        if (impl.InsTehaiSKS(dbHelper, drTehaiMeisaiAndEcs, serverTime) != 1)
                                        {
                                            impl.AddErrorMessage(dtMessage, "SKS手配明細の登録に失敗しました。(手配連携No:{0})", tehaiRenkeiNo);
                                            continue;
                                        }
                                    }

                                    // 手配SKS連携の登録
                                    if (impl.InsTehaiMeisaiSKS(dbHelper, drTehaiMeisaiAndEcs) != 1)
                                    {
                                        impl.AddErrorMessage(dtMessage, "{2} 手配SKS連携の登録に失敗しました。(手配連携No:{0}、手配No:{1})", tehaiRenkeiNo, tehaiNo, zyoken.ToString());
                                        continue;
                                    }

                                    // 手配明細の更新
                                    if (impl.UpdTehaiMeisaiTehaiKindFlag(dbHelper, drTehaiMeisaiAndEcs) != 1)
                                    {
                                        impl.AddErrorMessage(dtMessage, "{1} 手配明細の更新に失敗しました。(手配連携No:{0})", tehaiRenkeiNo, zyoken.ToString());
                                        continue;
                                    }
                                    ret = true;
                                }
                                catch (Exception ex)
                                {
                                    impl.AddErrorMessage(dtMessage, "{3} SKS手配明細登録の登録に失敗しました。(手配連携No:{0}、手配No:{1}、({2}))", tehaiRenkeiNo, tehaiNo, ex.Message, zyoken.ToString());
                                }
                                finally
                                {
                                    if (ret)
                                    {
                                        dbHelper.CommitAndBeginTransaction();
                                    }
                                    else
                                    {
                                        dbHelper.RollbackAndBeginTransaction();
                                    }
                                }
                            }

                            // トランザクション終了
                            dbHelper.Commit();
                        }
                    }

                    ds.Merge(dtMessage);
                    return true;
                }
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

    #region 手配明細更新

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細更新
    /// </summary>
    /// <param name="cond">J01用コンディション</param>
    /// <param name="ds">DataSet(エラーメッセージなど)</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/22</create>
    /// <update>T.Nukaga 2021/12/3 Step14 対象の手配種別に見積都合追加</update>
    /// --------------------------------------------------
    [WebMethod(Description = "手配明細更新")]
    public bool UpdTehaiMeisai(CondJ01 cond, out DataSet ds)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                ds = new DataSet();
                // 実行
                using (WsJ01Impl impl = new WsJ01Impl())
                {
                    // エラーメッセージ用のスキーマ取得
                    var dtMessage = impl.GetErrorMessageScheme();

                    // 手配種別設定
                    var tehaiKinds = new string[] { TEHAI_KIND_FLAG.ACROSS_VALUE1, TEHAI_KIND_FLAG.ANOTHER_VALUE1, TEHAI_KIND_FLAG.ESTIMATE_VALUE1 };
                    foreach (string tehaiKind in tehaiKinds)
                    {
                        // 手配明細取得
                        var dtTehaiMeisai = impl.GetTehaiMeisaiForUpdUnitPrice(dbHelper, tehaiKind);
                        if (UtilData.ExistsData(dtTehaiMeisai))
                        {
                            // トランザクション開始
                            dbHelper.BeginTransaction();

                            foreach (DataRow drTehaiMeisai in dtTehaiMeisai.Rows)
                            {
                                bool ret = false;
                                var tehaiRenkeiNo = ComFunc.GetFld(drTehaiMeisai, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO);
                                try
                                {
                                    // 手配明細の行ロック
                                    var dtTmp = impl.LockTehaiMeisai(dbHelper, drTehaiMeisai);
                                    if (!UtilData.ExistsData(dtTmp))
                                    {
                                        impl.AddErrorMessage(dtMessage, "手配明細は他端末で処理中です。(手配連携No:{0})", tehaiRenkeiNo);
                                        continue;
                                    }

                                    if (tehaiKind == TEHAI_KIND_FLAG.ACROSS_VALUE1)
                                    {
                                        // 手配明細の更新
                                        if (impl.UpdTehaiMeisaiUnitPriceForAcross(dbHelper, drTehaiMeisai) != 1)
                                        {
                                            impl.AddErrorMessage(dtMessage, "手配明細の更新に失敗しました。(手配連携No:{0})", tehaiRenkeiNo);
                                            continue;
                                        }
                                    }
                                    else if (tehaiKind == TEHAI_KIND_FLAG.ANOTHER_VALUE1)
                                    {
                                        // 手配明細の更新
                                        if (impl.UpdTehaiMeisaiUnitPriceForAnother(dbHelper, drTehaiMeisai) != 1)
                                        {
                                            impl.AddErrorMessage(dtMessage, "手配明細の更新に失敗しました。(手配連携No:{0})", tehaiRenkeiNo);
                                            continue;
                                        }
                                    }
                                    ret = true;
                                }
                                catch (Exception ex)
                                {
                                    impl.AddErrorMessage(dtMessage, "手配明細の更新に失敗しました。(手配連携No:{0}、({1}))", tehaiRenkeiNo, ex.Message);
                                }
                                finally
                                {
                                    if (ret)
                                    {
                                        dbHelper.CommitAndBeginTransaction();
                                    }
                                    else
                                    {
                                        dbHelper.RollbackAndBeginTransaction();
                                    }
                                }
                            }

                            // トランザクション終了
                            dbHelper.Commit();
                        }


                        // 手配明細(連携済み単価更新用)
                        var dtTehaiMeisai2 = impl.GetTehaiMeisaiForUpdUnitPrice2(dbHelper, tehaiKind);
                        if (UtilData.ExistsData(dtTehaiMeisai2))
                        {
                            // トランザクション開始
                            dbHelper.BeginTransaction();

                            foreach (DataRow drTehaiMeisai in dtTehaiMeisai2.Rows)
                            {
                                bool ret = false;
                                var tehaiRenkeiNo = ComFunc.GetFld(drTehaiMeisai, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO);
                                try
                                {
                                    // 手配明細の行ロック
                                    var dtTmp = impl.LockTehaiMeisai(dbHelper, drTehaiMeisai);
                                    if (!UtilData.ExistsData(dtTmp))
                                    {
                                        impl.AddErrorMessage(dtMessage, "手配明細は他端末で処理中です。(手配連携No:{0})", tehaiRenkeiNo);
                                        continue;
                                    }

                                    if (tehaiKind == TEHAI_KIND_FLAG.ACROSS_VALUE1)
                                    {
                                        // 手配明細の更新
                                        if (impl.UpdTehaiMeisaiUnitPriceForAcross(dbHelper, drTehaiMeisai) != 1)
                                        {
                                            impl.AddErrorMessage(dtMessage, "手配明細の更新に失敗しました。(手配連携No:{0})", tehaiRenkeiNo);
                                            continue;
                                        }
                                    }
                                    else if (tehaiKind == TEHAI_KIND_FLAG.ANOTHER_VALUE1)
                                    {
                                        // 手配明細の更新
                                        if (impl.UpdTehaiMeisaiUnitPriceForAnother(dbHelper, drTehaiMeisai) != 1)
                                        {
                                            impl.AddErrorMessage(dtMessage, "手配明細の更新に失敗しました。(手配連携No:{0})", tehaiRenkeiNo);
                                            continue;
                                        }
                                    }
                                    ret = true;
                                }
                                catch (Exception ex)
                                {
                                    impl.AddErrorMessage(dtMessage, "手配明細の更新に失敗しました。(手配連携No:{0}、({1}))", tehaiRenkeiNo, ex.Message);
                                }
                                finally
                                {
                                    if (ret)
                                    {
                                        dbHelper.CommitAndBeginTransaction();
                                    }
                                    else
                                    {
                                        dbHelper.RollbackAndBeginTransaction();
                                    }
                                }
                            }

                            // トランザクション終了
                            dbHelper.Commit();
                        }

                    }
                    ds.Merge(dtMessage);
                    return true;
                }
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

