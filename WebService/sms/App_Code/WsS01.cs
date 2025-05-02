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
/// 出荷計画処理クラス（トランザクション層）
/// </summary>
/// <create>Y.Higuchi 2010/06/22</create>
/// <update></update>
/// --------------------------------------------------
[WebService(Description = "出荷管理システム", Namespace = "http://smssrv/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。 
// [System.Web.Script.Services.ScriptService]
public class WsS01 : WsBase
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/04/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsS01()
        : base()
    {
    }

    #endregion

    #region S0100010:出荷計画登録

    #region 初期データ取得(出荷計画)

    /// --------------------------------------------------
    /// <summary>
    /// 初期データ取得(出荷計画)
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>初期データ</returns>
    /// <create>H.Tajimi 2020/04/14</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "初期データ取得(出荷計画)")]
    public DataSet GetInitShukkaKeikaku(CondS01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds = null;
                using (WsS01Impl impl = new WsS01Impl())
                {
                    ds = impl.GetInitShukkaKeikaku(dbHelper, cond);
                }
                return ds;
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

    #region 出荷計画データ登録

    /// --------------------------------------------------
    /// <summary>
    /// 出荷計画データ登録
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="dt">出荷データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tajimi 2018/10/31</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "納入先データ更新")]
    public bool InsShukkaKeikaku(CondS01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                //DataTable dt = ds.Tables[Def_M_NONYUSAKI.Name];
                bool ret = false;
                using (WsS01Impl impl = new WsS01Impl())
                {
                    ret = impl.InsShukkaKeikaku(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region 物件名一覧取得

    /// --------------------------------------------------
    /// <summary>
    /// 物件名一覧取得
    /// </summary>
    /// <returns>DataSet</returns>
    /// <create>R.Miyoshi 2023/07/14</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "物件名一覧取得")]
    public DataSet GetBukkenNameListForTorikomi()
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (var impl = new WsS01Impl())
                {
                    ds = impl.GetBukkenNameListForTorikomi(dbHelper);
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

    #region 荷受先名一覧取得

    /// --------------------------------------------------
    /// <summary>
    /// 荷受先名一覧取得
    /// </summary>
    /// <returns>DataSet</returns>
    /// <create>R.Miyoshi 2023/07/18</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "荷受先名一覧取得")]
    public DataSet GetConsignList()
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsS01Impl impl = new WsS01Impl())
                {
                    ds = impl.GetConsignList(dbHelper);
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

    #region 出荷元一覧取得

    /// --------------------------------------------------
    /// <summary>
    /// 出荷元一覧取得
    /// </summary>
    /// <returns>DataSet</returns>
    /// <create>T.SASAYAMA 2023/07/31</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "出荷元一覧取得")]
    public DataSet GetShukkamoto()
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (var impl = new WsS01Impl())
                {
                    ds = impl.GetShukkamoto(dbHelper);
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

    #region 出荷先一覧取得

    /// --------------------------------------------------
    /// <summary>
    /// 出荷先一覧取得
    /// </summary>
    /// <returns>DataSet</returns>
    /// <create>T.SASAYAMA 2023/07/31</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "出荷先一覧取得")]
    public DataSet GetShukkasaki()
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (var impl = new WsS01Impl())
                {
                    ds = impl.GetShukkasaki(dbHelper);
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

    #region 納入先取得

    /// --------------------------------------------------
    /// <summary>
    /// 納入先取得
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <returns>ユーザーマスタ</returns>
    /// <create>T.SASAYAMA 2023/07/25</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "納入先取得")]
    public DataSet GetNonyusaki(CondS01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsS01Impl impl = new WsS01Impl())
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
                // 切断
                dbHelper.Close();
            }
        }
    }

    #endregion

    #region 出荷計画のDB更新

    /// --------------------------------------------------
    /// <summary>
    /// 出荷計画のDB更新
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="ds">メール送信データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns></returns>
    /// <create>Y.Gwon 2023/09/01</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "出荷計画のDB更新")]
    public bool UpdPlanning(CondS01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (var impl = new WsS01Impl())
                {
                    ret = impl.UpdPlanning(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region リビジョン登録

    /// --------------------------------------------------
    /// <summary>
    /// リビジョン登録
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <returns></returns>
    /// <create>T.SASAYAMA 2023/08/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "リビジョン登録")]
    public bool InsertRevision(CondS01 cond, out string errMsgID, out string[] args)
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
                bool ret = false;
                using (WsS01Impl impl = new WsS01Impl())
                {
                    ret = impl.InsertRevision(dbHelper, cond, ref errMsgID, ref args);
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

    #region 排他取得

    /// --------------------------------------------------
    /// <summary>
    /// 排他取得
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <returns></returns>
    /// <create>J.Chen 2024/01/17</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "排他取得")]
    public DataTable GetHaitaData(CondS01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataTable ds = null;
                using (WsS01Impl impl = new WsS01Impl())
                {
                    ds = impl.GetHaitaData(dbHelper, cond);
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

    #region 排他登録

    /// --------------------------------------------------
    /// <summary>
    /// 排他登録
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <returns></returns>
    /// <create>J.Chen 2024/01/17</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "排他登録")]
    public int UpdateHaita(CondS01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                int ds = 0;
                using (WsS01Impl impl = new WsS01Impl())
                {
                    ds = impl.UpdateHaita(dbHelper, cond);
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

    #region 排他クリア

    /// --------------------------------------------------
    /// <summary>
    /// 排他クリア
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <returns></returns>
    /// <create>J.Chen 2024/01/17</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "排他クリア")]
    public int UpdateNullHaita(CondS01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                int ds = 0;
                using (WsS01Impl impl = new WsS01Impl())
                {
                    ds = impl.UpdateNullHaita(dbHelper, cond);
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

    #region S0100020:出荷計画明細登録

    #region 表示条件チェック

    /// --------------------------------------------------
    /// <summary>
    /// 表示条件チェック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="ds">納入先マスタ、AR情報データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>true;成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/07/08</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "表示条件チェック")]
    // 2012/04/24 K.Tsutsumi Change 納入先は事前に登録されている（よって、納入先コードは画面から渡される）
    //public bool CheckDisplayCondition(CondS01 cond, out DataSet ds, out string nonyusakiCD, out string errMsgID, out string[] args)
    public bool CheckDisplayCondition(CondS01 cond, out DataSet ds, out string errMsgID, out string[] args)
    // ↑
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                ds = null;
                // 2012/04/24 K.Tsutsumi Delete 納入先は事前に登録されている（よって、納入先コードは画面から渡される）
                //nonyusakiCD = string.Empty;
                // ↑
                errMsgID = string.Empty;
                args = null;
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                bool ret;
                using (WsS01Impl impl = new WsS01Impl())
                {
                    // 2012/04/24 K.Tsutsumi Change 納入先は事前に登録されている（よって、納入先コードは画面から渡される）
                    //ret = impl.CheckDisplayCondition(dbHelper, cond, ref ds, ref nonyusakiCD, ref errMsgID, ref args);
                    string bukkenNO = string.Empty;
                    ret = impl.CheckDisplayCondition(dbHelper, cond, ref ds, ref bukkenNO, ref errMsgID, ref args);
                    // ↑
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

    #region 表示データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 表示データ取得
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>表示データ</returns>
    /// <create>Y.Higuchi 2010/07/08</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "表示データ取得")]
    public DataSet GetShukkaMeisai(CondS01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds = null;
                using (WsS01Impl impl = new WsS01Impl())
                {
                    ds = impl.GetShukkaMeisai(dbHelper, cond);
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

    #region 写真有無取得

    /// --------------------------------------------------
    /// <summary>
    /// 写真有無取得
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/08/27</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "写真有無取得")]
    public DataTable GetExistsPicture(CondS01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataTable dt = null;
                using (WsS01Impl impl = new WsS01Impl())
                {
                    dt = impl.GetExistsPicture(dbHelper, cond);
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

    /// --------------------------------------------------
    /// <summary>
    /// 写真有無取得
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="dt">DataTable</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/08/27</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "写真有無取得")]
    public DataTable GetExistsPictureFromDataTable(CondS01 cond, DataTable dt)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataTable ret = null;
                using (WsS01Impl impl = new WsS01Impl())
                {
                    ret = impl.GetExistsPicture(dbHelper, cond, dt);
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

    #region 出荷計画明細データ登録

    /// --------------------------------------------------
    /// <summary>
    /// 出荷計画明細データ登録
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="dt">出荷明細データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/07/13</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "出荷計画明細データ登録")]
    public bool InsShukkaKeikakuMeisai(CondS01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                DataTable dt = ds.Tables[Def_T_SHUKKA_MEISAI.Name];
                bool ret = false;
                using (WsS01Impl impl = new WsS01Impl())
                {
                    try
                    {
                        ret = impl.InsShukkaKeikakuMeisai(dbHelper, cond, dt, ref errMsgID, ref args);
                    }
                    catch (Exception exIns)
                    {
                        // 一意制約違反チェック
                        if(!impl.IsDbDuplicationError(exIns))
                        {
                            throw new Exception(exIns.Message, exIns);
                        }
                        dbHelper.RollbackAndBeginTransaction();
                        ret = impl.InsShukkaKeikakuMeisai(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region 出荷計画明細データ更新

    /// --------------------------------------------------
    /// <summary>
    /// 出荷計画明細データ更新
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="ds">更新データのDataSet</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/07/15</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "出荷計画明細データ更新")]
    public bool UpdShukkaKeikakuMeisai(CondS01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (WsS01Impl impl = new WsS01Impl())
                {
                    ret = impl.UpdShukkaKeikakuMeisai(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region 出荷計画明細データ削除

    /// --------------------------------------------------
    /// <summary>
    /// 出荷計画明細データ削除
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="ds">削除データのDataSet</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/07/15</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "出荷計画明細データ削除")]
    public bool DelShukkaKeikakuMeisai(CondS01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (WsS01Impl impl = new WsS01Impl())
                {
                    ret = impl.DelShukkaKeikakuMeisai(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region S0100030:便間移動

    #region ツリー明細取得

    /// --------------------------------------------------
    /// <summary>
    /// ツリー明細取得
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>ツリー明細データ</returns>
    /// <create>T.Wakamatsu 2016/01/06</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ツリー明細取得")]
    public DataSet GetTreeMeisai(CondS01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds = null;
                using (WsS01Impl impl = new WsS01Impl())
                {
                    ds = impl.GetTreeMeisai(dbHelper, cond);
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

    #region 便間移動更新

    /// --------------------------------------------------
    /// <summary>
    /// 便間移動更新
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Wakamatsu 2016/01/08</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "便間移動更新")]
    public bool UpdMoveShip(CondS01 cond)
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
                using (WsS01Impl impl = new WsS01Impl())
                {
                    ret = impl.UpdMoveShip(dbHelper, cond);
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

    #region 移動先木枠取得

    /// --------------------------------------------------
    /// <summary>
    /// 移動先木枠取得
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>木枠データ</returns>
    /// <create>T.Wakamatsu 2016/01/06</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "移動先木枠取得")]
    public DataSet GetKiwaku(CondS01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds = null;
                using (WsS01Impl impl = new WsS01Impl())
                {
                    ds = impl.GetKiwaku(dbHelper, cond);
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

    #region C/NO、印刷C/NO重複チェック

    /// --------------------------------------------------
    /// <summary>
    /// C/NO、印刷C/NO重複チェック
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>木枠データ</returns>
    /// <create>T.Wakamatsu 2016/01/06</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "C/NO、印刷C/NO重複チェック")]
    public bool DuplicateCaseNo(CondS01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds = null;
                using (WsS01Impl impl = new WsS01Impl())
                {
                    ds = impl.GetKiwaku(dbHelper, cond);
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

    #endregion

    #region S0100023:TAG登録連携

    #region 物件名一覧取得

    /// --------------------------------------------------
    /// <summary>
    /// 物件名一覧取得
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>T.Nakata 2018/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "物件名一覧取得")]
    public DataSet GetBukkenNameList(CondS01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (var impl = new WsS01Impl())
                {
                    ds = impl.GetBukkenNameList(dbHelper, cond);
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

    #region 便一覧取得

    /// --------------------------------------------------
    /// <summary>
    /// 便一覧取得
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>T.Nakata 2018/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "便一覧取得")]
    public DataSet GetShipList(CondS01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (var impl = new WsS01Impl())
                {
                    ds = impl.GetShipList(dbHelper, cond);
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

    #region TAG連携一覧取得

    /// --------------------------------------------------
    /// <summary>
    /// TAG連携一覧取得
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>T.Nakata 2018/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "TAG連携一覧取得")]
    public DataSet GetTagRenkeiList(CondS01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (var impl = new WsS01Impl())
                {
                    ds = impl.GetTagRenkeiList(dbHelper, cond);
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

    #region TAG連携一覧取得(ロック用)

    /// --------------------------------------------------
    /// <summary>
    /// TAG連携一覧取得
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>T.Nakata 2018/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "TAG連携一覧取得(ロック用)")]
    public DataTable GetAndLockTagRenkeiList(CondS01 cond, DataSet ds, bool isLock)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataTable retdt;
                using (var impl = new WsS01Impl())
                {
                    retdt = impl.GetAndLockTagRenkeiList(dbHelper, cond, ds, isLock);
                }
                return retdt;
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

    #region 有償/無償取得

    /// --------------------------------------------------
    /// <summary>
    /// 有償/無償取得
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>T.Nakata 2018/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "有償/無償取得")]
    public DataSet GetEstimateFlag(CondS01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (var impl = new WsS01Impl())
                {
                    ds = impl.GetEstimateFlag(dbHelper, cond);
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

    #region TAG連携と手配Noの出荷先をチェック

    /// --------------------------------------------------
    /// <summary>
    /// TAG連携と手配Noの出荷先をチェック
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>DataTable</returns>
    /// <create>2022/05/31 STEP14</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "TAG連携と手配Noの出荷先をチェック")]
    public DataTable GetShipToForTehaiNo(CondS01 cond, string tehaiNo)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataTable dt;
                using (var impl = new WsS01Impl())
                {
                    dt = impl.GetShipToForTehaiNo(dbHelper, cond, tehaiNo);
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

    #region 手配明細バージョン更新

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細バージョン更新
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <create>T.Nakata 2018/11/09</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "手配明細バージョン更新")]
    public bool UpdTehaimeisaiVersionData(CondS01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (WsS01Impl impl = new WsS01Impl())
                {
                    ret = impl.UpdTehaimeisaiVersionData(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region TAG連携メールデータ登録

    /// --------------------------------------------------
    /// <summary>
    /// TAG連携メールデータ登録
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/08/17</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "TAG連携メールデータ登録")]
    public bool InsTagRenkeiMail(CondS01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (WsCommonImpl comImpl = new WsCommonImpl())
                {
                    var condCommon = new CondCommon(cond.LoginInfo);
                    ret = comImpl.SaveMail(dbHelper, condCommon, ds.Tables[Def_T_MAIL.Name].Rows[0], ref errMsgID, ref args);
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

    #region S0100040:一括アップロード

    #region 表示データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 表示データ取得
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="dsSearch">検索データ</param>
    /// <returns>表示データ</returns>
    /// <create>H.Tajimi 2019/08/08</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "表示データ取得")]
    public DataSet GetTehaiMeisaiDataForIkkatsuUpload(CondS01 cond, DataSet dsSearch)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds = null;
                using (WsS01Impl impl = new WsS01Impl())
                {
                    ds = impl.GetTehaiMeisaiDataForIkkatsuUpload(dbHelper, cond, dsSearch);
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

    #region 図番/型式管理データの登録

    /// --------------------------------------------------
    /// <summary>
    /// 図番/型式管理データの登録
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="ds">更新データのDataSet</param>
    /// <param name="dtZumenKeishiki">図番/型式データテーブル</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tajimi 2019/08/07</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "図番/型式管理データの登録")]
    public bool InsManageZumenKeishiki(CondS01 cond, DataSet ds, out DataTable dtZumenKeishiki, out string errMsgID, out string[] args)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                errMsgID = string.Empty;
                args = null;
                dtZumenKeishiki = null;

                // 接続
                this.DbOpen(dbHelper);

                // トランザクション開始
                dbHelper.BeginTransaction();

                // 実行
                bool ret = false;
                using (WsS01Impl impl = new WsS01Impl())
                {
                    ret = impl.InsManageZumenKeishiki(dbHelper, cond, ds, ref dtZumenKeishiki, ref errMsgID, ref args);
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

    #region S0100050:荷姿表登録

    #region 初期データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 初期データ取得
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>K.Tsutsumi 2019/03/13</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "初期データ取得")]
    public DataSet S0100050_GetInit(CondS01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (var impl = new WsS01Impl())
                {
                    ds = impl.S0100050_Ctrl_GetInit(dbHelper, cond);
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
    /// <param name="cond">S01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>K.Tsutsumi 2019/03/13</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "表示データ取得")]
    public DataSet S0100050_GetDisp(CondS01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (var impl = new WsS01Impl())
                {
                    ds = impl.S0100050_Ctrl_GetDisp(dbHelper, cond);
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

    #region 指定出荷日納入情報一覧取得

    /// --------------------------------------------------
    /// <summary>
    /// 指定出荷日納入情報一覧取得
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>T.nakata 2018/11/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "指定出荷日納入情報一覧取得")]
    public DataSet GetNisugataBaseData(CondS01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (var impl = new WsS01Impl())
                {
                    ds = impl.GetNisugataBaseData(dbHelper, cond);
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

    #region C/Noより木枠の有無確認（取得できた場合はサイズと重量も取得）

    /// --------------------------------------------------
    /// <summary>
    /// C/Noより木枠の有無確認（取得できた場合はサイズと重量も取得）
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>K.Tsutsumi 2019/03/14</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "C/Noより木枠の有無確認（取得できた場合はサイズと重量も取得）")]
    public DataSet S0100050_GetKiwakuMeisai(CondS01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (var impl = new WsS01Impl())
                {
                    ds = impl.S0100050_Ctrl_GetKiwakuMeisai(dbHelper, cond);
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

    #region Box No.より、Boxリスト管理データ取得

    /// --------------------------------------------------
    /// <summary>
    /// Box No.より、Boxリスト管理データ取得
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">メッセージパラメータ</param>
    /// <returns>true:存在した false:存在しない</returns>
    /// <create>K.Tsutsumi 2019/03/14</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "Box No.より、Boxリスト管理データ取得")]
    public bool S0100050_GetBoxListManage(CondS01 cond, out string errMsgID, out string[] args)
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
                bool isExists = false;
                using (var impl = new WsS01Impl())
                {
                    isExists = impl.S0100050_Ctrl_GetBoxListManage(dbHelper, cond, ref errMsgID, ref args);
                }
                return isExists;
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

    #region Pallet No.より、Palletリスト管理データ取得

    /// --------------------------------------------------
    /// <summary>
    /// Pallet No.より、Palletリスト管理データ取得
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">メッセージパラメータ</param>
    /// <returns>true:存在した false:存在しない</returns>
    /// <create>K.Tsutsumi 2019/03/14</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "Pallet No.より、Palletリスト管理データ取得")]
    public bool S0100050_GetPalletListManage(CondS01 cond, out string errMsgID, out string[] args)
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
                bool isExists = false;
                using (var impl = new WsS01Impl())
                {
                    isExists = impl.S0100050_Ctrl_GetPalletListManage(dbHelper, cond, ref errMsgID, ref args);
                }
                return isExists;
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

    #region 荷姿/明細 更新

    /// --------------------------------------------------
    /// <summary>
    /// 荷姿/明細 更新
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <create>T.Nakata 2018/12/03</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "荷姿/明細 更新")]
    public bool UpdNisugata(CondS01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (WsS01Impl impl = new WsS01Impl())
                {
                    ret = impl.UpdNisugata(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region 荷姿/明細 削除

    /// --------------------------------------------------
    /// <summary>
    /// 荷姿/明細 削除
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <create>T.Nakata 2018/12/03</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "荷姿/明細 削除")]
    public bool DelNisugata(CondS01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (WsS01Impl impl = new WsS01Impl())
                {
                    ret = impl.DelNisugata(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region 荷姿表Excel出力データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 荷姿表Excel出力データ取得
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/12/03</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "荷姿表Excel出力データ取得")]
    public DataSet GetNisugataExcelData(CondS01 cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (var impl = new WsS01Impl())
                {
                    ds = impl.GetNisugataExcelData(dbHelper, cond);
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

    #region MAIL_ID採番

    /// --------------------------------------------------
    /// <summary>
    /// MAIL_ID採番
    /// </summary>
    /// <param name="cond"></param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/12/03</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "MAIL_ID採番")]
    public string GetMailID(CondS01 cond, out string errMsgID, out string[] args)
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

    #region 荷姿更新(Excel出力用)

    /// --------------------------------------------------
    /// <summary>
    /// 荷姿更新(Excel出力用)
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="ds">荷姿データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/12/03</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "荷姿更新(Excel出力用)")]
    public bool UpdPackingForExcelData(CondS01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (var impl = new WsS01Impl())
                {
                    ret = impl.UpdPackingForExcelData(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region S0100060:Mail送信履歴

    #region Mail送信履歴取得

    /// --------------------------------------------------
    /// <summary>
    /// Mail送信履歴取得
    /// </summary>
    /// <param name="cond">S01用条件</param>
    /// <returns></returns>
    /// <create>Y.Gwon 2023/08/17</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "Mail送信履歴取得")]
    public DataSet GetMailSousinRireki(CondS01 cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                //接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (var impl = new WsS01Impl())
                {
                    ds = impl.GetMailSousinRireki(dbHelper, cond);
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                //切断
                dbHelper.Close();
            }
        }
    }



    #endregion

    #endregion

    #region S0100070:出荷計画照会

    #region 出荷計画一覧取得

    /// --------------------------------------------------
    /// <summary>
    /// 出荷計画一覧取得
    /// </summary>
    /// <returns>DataSet</returns>
    /// <create>J.Chen 2023/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "出荷計画一覧取得")]
    public DataSet GetNonyusakiForShokai(CondS01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (var impl = new WsS01Impl())
                {
                    ds = impl.GetNonyusakiForShokai(dbHelper, cond);
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

    #region 製番一覧取得

    /// --------------------------------------------------
    /// <summary>
    /// 製番一覧取得
    /// </summary>
    /// <returns>DataSet</returns>
    /// <create>J.Chen 2023/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "製番一覧取得")]
    public DataSet GetSeiban()
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (var impl = new WsS01Impl())
                {
                    ds = impl.GetSeiban(dbHelper);
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

    #region 機種一覧取得

    /// --------------------------------------------------
    /// <summary>
    /// 機種一覧取得
    /// </summary>
    /// <returns>DataSet</returns>
    /// <create>J.Chen 2023/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "機種一覧取得")]
    public DataSet GetKishu()
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (var impl = new WsS01Impl())
                {
                    ds = impl.GetKishu(dbHelper);
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

    #region 現場用ステータス更新

    /// --------------------------------------------------
    /// <summary>
    /// 現場用ステータス更新
    /// </summary>
    /// <returns>DataSet</returns>
    /// <create>J.Chen 2024/10/03</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "現場用ステータス更新")]
    public bool UpdNonyusakiForGenbayo(CondS01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (var impl = new WsS01Impl())
                {
                    ret = impl.UpdNonyusakiForGenbayo(dbHelper, cond, ds, ref errMsgID, ref args);
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
