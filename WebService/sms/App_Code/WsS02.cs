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
/// 出荷情報処理クラス（トランザクション層）
/// </summary>
/// <create>Y.Higuchi 2010/06/22</create>
/// <update></update>
/// --------------------------------------------------
[WebService(Description = "出荷管理システム", Namespace = "http://smssrv/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。 
// [System.Web.Script.Services.ScriptService]
public class WsS02 : WsBase
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/06/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsS02()
        : base()
    {
    }

    #endregion

    #region S0200010:出荷情報登録

    #region Box出荷データ取得

    /// --------------------------------------------------
    /// <summary>
    /// Box出荷データ取得
    /// </summary>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="errMsgID">エラーメッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns></returns>
    /// <create>Y.Higuchi 2010/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "Box出荷データ取得")]
    public DataSet GetBoxData(CondS02 cond, out string errMsgID, out string[] args)
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
                using (WsS02Impl impl = new WsS02Impl())
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

    #region パレット出荷データ取得

    /// --------------------------------------------------
    /// <summary>
    /// パレット出荷データ取得
    /// </summary>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="errMsgID">エラーメッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns></returns>
    /// <create>Y.Higuchi 2010/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "パレット出荷データ取得")]
    public DataSet GetPalletData(CondS02 cond, out string errMsgID, out string[] args)
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
                using (WsS02Impl impl = new WsS02Impl())
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

    #region 木枠出荷データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 木枠出荷データ取得
    /// </summary>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="errMsgID">エラーメッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns></returns>
    /// <create>Y.Higuchi 2010/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "木枠出荷データ取得")]
    public DataSet GetKiwakuData(CondS02 cond, out string errMsgID, out string[] args)
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
                using (WsS02Impl impl = new WsS02Impl())
                {
                    ds = impl.GetKiwakuData(dbHelper, cond, ref errMsgID, ref args);
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

    #region Box出荷データ更新

    /// --------------------------------------------------
    /// <summary>
    /// Box出荷データ更新
    /// </summary>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="dt">更新データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:</returns>
    /// <create>Y.Higuchi 2010/07/26</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "Box出荷データ更新")]
    public bool UpdBoxData(CondS02 cond, DataTable dt, out string errMsgID, out string[] args)
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
                using (WsS02Impl impl = new WsS02Impl())
                {
                    ret = impl.UpdBoxData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region パレット出荷データ更新

    /// --------------------------------------------------
    /// <summary>
    /// パレット出荷データ更新
    /// </summary>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="dt">更新データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:</returns>
    /// <create>Y.Higuchi 2010/07/26</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "パレット出荷データ更新")]
    public bool UpdPalletData(CondS02 cond, DataTable dt, out string errMsgID, out string[] args)
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
                using (WsS02Impl impl = new WsS02Impl())
                {
                    ret = impl.UpdPalletData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region 木枠出荷データ更新

    /// --------------------------------------------------
    /// <summary>
    /// 木枠出荷データ更新
    /// </summary>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="dt">更新データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:</returns>
    /// <create>Y.Higuchi 2010/07/26</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "木枠出荷データ更新")]
    public bool UpdKiwakuData(CondS02 cond, DataTable dt, out string errMsgID, out string[] args)
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
                using (WsS02Impl impl = new WsS02Impl())
                {
                    ret = impl.UpdKiwakuData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region S0200020:出荷情報明細

    #region BoxNoの出荷明細データを取得

    /// --------------------------------------------------
    /// <summary>
    /// BoxNoの出荷明細データを取得
    /// </summary>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>BoxNoの出荷明細データ</returns>
    /// <create>Y.Higuchi 2010/07/27</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "BoxNoの出荷明細データを取得")]
    public DataSet GetBoxMeisai(CondS02 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsS02Impl impl = new WsS02Impl())
                {
                    ds = impl.GetBoxMeisai(dbHelper, cond);
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

    #region S0200030:出荷情報照会

    #region 出荷明細データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ取得
    /// </summary>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>表示データ</returns>
    /// <create>Y.Higuchi 2010/08/10</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "出荷明細データ取得")]
    public DataSet GetShukkaMeisai(CondS02 cond, out string errMsgID, out string[] args)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            errMsgID = string.Empty;
            args = null;
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsS02Impl impl = new WsS02Impl())
                {
                    ds = impl.GetShukkaMeisai(dbHelper, cond, ref errMsgID, ref args);
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

    #region S0200040:ShippingDocument作成

    #region 荷受マスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// 荷受マスタ取得
    /// </summary>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>表示データ</returns>
    /// <create>T.Nakata 2018/12/11</create>
    /// <update>D.Okumura 2021/01/15 EFA_SMS-184 DataTableを返却するように変更</update>
    /// --------------------------------------------------
    [WebMethod(Description = "荷受マスタ取得")]
    public DataSet GetConsign(CondS02 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsS02Impl impl = new WsS02Impl())
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

    #region 配送先マスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// 配送先マスタ取得
    /// </summary>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>表示データ</returns>
    /// <create>T.Nakata 2018/12/11</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "配送先マスタ取得")]
    public DataSet GetDeliver(CondS02 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsS02Impl impl = new WsS02Impl())
                {
                    ds = impl.GetDeliverExec(dbHelper, cond);
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

    #region 荷姿情報取得

    /// --------------------------------------------------
    /// <summary>
    /// 荷姿情報取得
    /// </summary>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>表示データ</returns>
    /// <create>T.Nakata 2018/12/11</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "荷姿情報取得")]
    public DataSet GetNisugata(CondS02 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsS02Impl impl = new WsS02Impl())
                {
                    ds = impl.GetNisugataExec(dbHelper, cond);
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

    #region Invoice作成事前確認

    /// --------------------------------------------------
    /// <summary>
    /// Invoice作成事前確認
    /// </summary>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>表示データ</returns>
    /// <create>T.Nakata 2018/12/13</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "Invoice作成事前確認")]
    public DataTable CheckInvoice(CondS02 cond, DataTable dt)
    {
        DataTable ret;
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                using (WsS02Impl impl = new WsS02Impl())
                {
                    ret = impl.CheckInvoice(dbHelper, cond, dt);
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
            return ret;
        }
    }

    #endregion

    #region 各種テーブル更新

    /// --------------------------------------------------
    /// <summary>
    /// 各種テーブル更新
    /// </summary>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="dt">更新データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:</returns>
    /// <create>T.Nakata 2018/12/18</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "各種テーブル更新")]
    public bool UpdShippingData(CondS02 cond, DataTable dt, out string errMsgID, out string[] args)
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
                using (WsS02Impl impl = new WsS02Impl())
                {
                    ret = impl.UpdShippingData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region 帳票用データ取得ShippingDocument

    /// --------------------------------------------------
    /// <summary>
    /// 帳票用データ取得A/B/C/F
    /// </summary>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>表示データ</returns>
    /// <create>D.Okumura 2021/01/15 EFA_SMS-184 DataTableを返却するように変更</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "帳票用データ取得ShippingDocument")]
    public DataSet GetShippingDocument(CondS02 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsS02Impl impl = new WsS02Impl())
                {
                    ds = impl.GetShippingDocument(dbHelper, cond);
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

    #region 帳票用データ取得D

    /// --------------------------------------------------
    /// <summary>
    /// 帳票用データ取得D
    /// </summary>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>表示データ</returns>
    /// <create>T.Nakata 2018/12/19</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "帳票用データ取得D")]
    public DataSet GetTyouhyouDataD(CondS02 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsS02Impl impl = new WsS02Impl())
                {
                    ds = impl.GetTyouhyouDataD(dbHelper, cond);
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

    #region 帳票用データ取得E

    /// --------------------------------------------------
    /// <summary>
    /// 帳票用データ取得E
    /// </summary>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>表示データ</returns>
    /// <create>T.Nakata 2018/12/19</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "帳票用データ取得E")]
    public DataSet GetTyouhyouDataE(CondS02 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsS02Impl impl = new WsS02Impl())
                {
                    ds = impl.GetTyouhyouDataE(dbHelper, cond);
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

    #region 帳票用データ取得G

    /// --------------------------------------------------
    /// <summary>
    /// 帳票用データ取得G
    /// </summary>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>表示データ</returns>
    /// <create>T.Nakata 2018/12/19</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "帳票用データ取得G")]
    public DataSet GetTyouhyouDataG(CondS02 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsS02Impl impl = new WsS02Impl())
                {
                    ds = impl.GetTyouhyouDataG(dbHelper, cond);
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

    #region 帳票用データ取得H

    /// --------------------------------------------------
    /// <summary>
    /// 帳票用データ取得H
    /// </summary>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>表示データ</returns>
    /// <create>T.Nakata 2018/12/19</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "帳票用データ取得H")]
    public DataSet GetTyouhyouDataH(CondS02 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsS02Impl impl = new WsS02Impl())
                {
                    ds = impl.GetTyouhyouDataH(dbHelper, cond);
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

