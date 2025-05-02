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
/// 手配明細処理クラス
/// </summary>
/// <create>S.Furugo 2018/10/28</create>
/// <update></update>
/// --------------------------------------------------
[WebService(Description = "出荷管理システム", Namespace = "http://smssrv/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。 
// [System.Web.Script.Services.ScriptService]
public class WsT01 : WsBase
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>S.Furugo 2018/10/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsT01()
        : base()
    {
    }

    #endregion

    #region T0100010:手配明細情報登録

    #region 手配明細取得
    /// --------------------------------------------------
    /// <summary>
    /// 手配明細取得
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <returns>ユーザーマスタ</returns>
    /// <create>S.Furugo 2018/10/29</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "手配明細取得")]
    public DataSet GetTehaiMeisai(CondT01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsT01Impl impl = new WsT01Impl())
                {
                    ds = impl.GetTehaiMeisai(dbHelper, cond);
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

    #region 手配明細登録
    /// --------------------------------------------------
    /// <summary>
    /// 手配明細データ登録
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="ds">データセット</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>D. Naito 2018/11/26</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "手配明細データ登録")]
    public bool InsTehaiMeisai(CondT01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (WsT01Impl impl = new WsT01Impl())
                {
                    // 技連マスタ・手配明細登録
                    ret = impl.InsTehaiMeisai(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region 手配明細更新
    /// --------------------------------------------------
    /// <summary>
    /// 手配明細データ更新
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="ds">データセット</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>S.Furugo 2018/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "手配明細データ更新")]
    public bool UpdTehaiMeisai(CondT01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (WsT01Impl impl = new WsT01Impl())
                {
                    ret = impl.UpdTehaiMeisai(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region 手配明細削除
    /// --------------------------------------------------
    /// <summary>
    /// 手配明細データ削除
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="ds">データセット</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>S.Furugo 2018/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "手配明細データ削除")]
    public bool DelTehaiMeisai(CondT01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (WsT01Impl impl = new WsT01Impl())
                {
                    ret = impl.DelTehaiMeisai(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region 技連情報取得
    /// --------------------------------------------------
    /// <summary>
    /// 技連情報取得
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <returns>ユーザーマスタ</returns>
    /// <create>S.Furugo 2018/10/29</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "技連情報取得")]
    public DataSet GetGiren(CondT01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsT01Impl impl = new WsT01Impl())
                {
                    ds = impl.GetGiren(dbHelper, cond);
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

    #region 名称マスタ取得
    /// --------------------------------------------------
    /// <summary>
    /// 名称マスタ取得
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <returns></returns>
    /// <create>D.Naito 2018/11/28</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "名称マスタ取得")]
    public DataSet GetSelectItem(CondT01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                using (WsT01Impl impl = new WsT01Impl())
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

    #region パーツ名マスタ取得
    /// --------------------------------------------------
    /// <summary>
    /// パーツ名マスタ取得
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <returns></returns>
    /// <create>D.Naito 2018/11/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "パーツ名マスタ取得")]
    public DataSet GetPartsName(CondT01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                using (WsT01Impl impl = new WsT01Impl())
                {
                    return impl.GetPartsName(dbHelper, cond);
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

    #region 図番/型式画像ファイル管理データ取得(IN検索)

    /// --------------------------------------------------
    /// <summary>
    /// 図番/型式画像ファイル管理データ取得(IN検索)
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tsuji 2019/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "図番/型式画像ファイル管理データ取得(IN検索)")]
    public DataSet GetManageZumenKeishikiInSearch(CondT01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                using (WsT01Impl impl = new WsT01Impl())
                {
                    return impl.GetManageZumenKeishikiInSearch(dbHelper, cond);
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

    #region 図番/型式の入力補助関連データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 図番/型式の入力補助関連データ取得
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tsuji 2019/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "図番/型式の入力補助関連データ取得")]
    public DataSet GetZumenKeishikiInputAssistance(CondT01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                using (WsT01Impl impl = new WsT01Impl())
                {
                    return impl.GetZumenKeishikiInputAssistance(dbHelper, cond);
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

    //#region Assy単価計算データ取得

    ///// --------------------------------------------------
    ///// <summary>
    ///// Assy単価計算データ取得
    ///// </summary>
    ///// <param name="cond">T01用コンディション</param>
    ///// <returns></returns>
    ///// <create>Y.Shioshi 2022/05/12</create>
    ///// <update></update>
    ///// --------------------------------------------------
    //[WebMethod(Description = "Assy単価計算データ取得")]
    //public DataSet GetAssyUnitPrice(CondT01 cond)
    //{
    //    using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
    //    {
    //        try
    //        {
    //            // 接続
    //            this.DbOpen(dbHelper);

    //            // 実行
    //            using (WsT01Impl impl = new WsT01Impl())
    //            {
    //                return impl.GetAssyUnitPrice(dbHelper, cond);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception(ex.Message, ex);
    //        }
    //        finally
    //        {
    //            // 切断
    //            dbHelper.Close();
    //        }
    //    }
    //}

    //#endregion

    #region SKS手配明細WORKバージョン取得

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細WORKバージョン取得
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <returns></returns>
    /// <create>J.Chen 2022/10/31</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "SKS手配明細WORKバージョン取得")]
    public DataSet GetTehaiSKSWorkVersion(CondT01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                using (WsT01Impl impl = new WsT01Impl())
                {
                    return impl.GetTehaiSKSWorkVersion(dbHelper, cond);
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

    #region T0100020:SKS手配連携

    #region 表示データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 表示データ取得
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/16</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "手配明細取得")]
    public DataSet GetSKSTehaiRenkeiDispData(CondT01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsT01Impl impl = new WsT01Impl())
                {
                    ds = impl.GetSKSTehaiRenkeiDispData(dbHelper, cond);
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

    #region SKS手配明細取得

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細取得
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="ds">表示データ</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/17</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "SKS手配明細取得")]
    public DataTable GetSKSTehaiRenkeiTehaiSKS(CondT01 cond, DataSet ds)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataTable dt;
                using (WsT01Impl impl = new WsT01Impl())
                {
                    dt = impl.GetSKSTehaiRenkeiTehaiSKS(dbHelper, cond, ds.Tables[Def_T_TEHAI_MEISAI.Name]);
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

    #region SKS手配明細WORK取得

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細WORK取得
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>SKS手配明細WORK</returns>
    /// <create>H.Tajimi 2019/01/17</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "SKS手配明細WORK取得")]
    public DataSet GetSKSTehaiRenkeiTehaiSKSWork(CondT01 cond, out string errMsgID, out string[] args)
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
                var ds = new DataSet();
                using (WsT01Impl impl = new WsT01Impl())
                {
                    ds = impl.GetSKSTehaiRenkeiTehaiSKSWork(dbHelper, cond, ref errMsgID, ref args);
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

    #region SKS手配連携更新

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配連携更新
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="ds">更新データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:</returns>
    /// <create>H.Tajimi 2019/01/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "SKS手配連携更新")]
    public bool UpdSKSTehaiRenkei(CondT01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (WsT01Impl impl = new WsT01Impl())
                {
                    ret = impl.UpdSKSTehaiRenkei(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region T0100030:手配明細照会

    #region 手配明細取得
    /// --------------------------------------------------
    /// <summary>
    /// 手配明細取得
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <returns>ユーザーマスタ</returns>
    /// <create>S.Furugo 2018/12/13</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "手配明細取得")]
    public DataSet GetTehaiShokai(CondT01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsT01Impl impl = new WsT01Impl())
                {
                    ds = impl.GetTehaiShokai(dbHelper, cond);
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

    #region 手配情報照会

    #region 初期表示(手配情報照会)

    /// --------------------------------------------------
    /// <summary>
    /// 初期表示(手配情報照会)
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/08/28</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "初期表示(手配情報照会)")]
    public DataSet GetInitTehaiJohoShokai(CondT01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsT01Impl impl = new WsT01Impl())
                {
                    ds = impl.GetInitTehaiJohoShokai(dbHelper, cond);
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

    #region 表示データ取得(手配情報照会)

    /// --------------------------------------------------
    /// <summary>
    /// 表示データ取得(手配情報照会)
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/08/29</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "手配明細取得")]
    public DataSet GetTehaiJohoShokai(CondT01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsT01Impl impl = new WsT01Impl())
                {
                    ds = impl.GetTehaiJohoShokai(dbHelper, cond);
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

    #region 手配履歴更新

    /// --------------------------------------------------
    /// <summary>
    /// 手配履歴更新
    /// </summary>
    /// <returns>DataSet</returns>
    /// <create>J.Chen 2024/10/25</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "手配履歴更新")]
    public bool UpdTehaiJohoRireki(CondT01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (var impl = new WsT01Impl())
                {
                    ret = impl.UpdTehaiJohoRireki(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region T0100040:手配見積

    #region 見積明細取得

    /// --------------------------------------------------
    /// <summary>
    /// 手配見積取得
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <returns>ユーザーマスタ</returns>
    /// <create>S.Furugo 2018/11/26</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "手配見積取得")]
    public DataSet GetTehaiMitsumori(CondT01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsT01Impl impl = new WsT01Impl())
                {
                    ds = impl.GetTehaiEstimate(dbHelper, cond);
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

    #region 見積情報取得

    /// --------------------------------------------------
    /// <summary>
    /// 見積情報取得
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <returns>ユーザーマスタ</returns>
    /// <create>S.Furugo 2018/12/3</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "見積情報取得")]
    public DataSet GetEstimateInformation(CondT01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsT01Impl impl = new WsT01Impl())
                {
                    ds = impl.GetTehaiEstimate(dbHelper, cond);
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

    #region 手配見積登録
    /// --------------------------------------------------
    /// <summary>
    /// 手配見積データ登録
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="ds">データセット</param>
    /// <param name="saiban">採番コード</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>S.Furugo 2018/12/04</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "手配見積データ登録")]
    public bool InsTehaiMitsumori(CondT01 cond, DataSet ds, out string saiban, out string errMsgID, out string[] args)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                errMsgID = string.Empty;
                args = null;
                saiban = null;

                // 接続
                this.DbOpen(dbHelper);

                // トランザクション開始
                dbHelper.BeginTransaction();

                // 実行
                bool ret = false;
                string tmpEstimateNo;

                while (true)
                {
                    //採番
                    using (var smsImpl = new WsSmsImpl())
                    {
                        var condSms = new CondSms(cond.LoginInfo);
                        condSms.SaibanFlag = SAIBAN_FLAG.ESTIMATE_NO_VALUE1;
                        ret = smsImpl.GetSaiban(dbHelper, condSms, out tmpEstimateNo, out errMsgID);
                        cond.MitsumoriNo = tmpEstimateNo;
                        saiban = tmpEstimateNo;
                    }
                    if (!ret)
                        break;

                    // 手配見積登録(採番した番号を用いる)
                    using (WsT01Impl impl = new WsT01Impl())
                    {
                        ret = impl.InsTehaiMitsumori(dbHelper, cond, ds, tmpEstimateNo, ref errMsgID, ref args);
                    }
                    break;
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

    #region 手配見積更新
    /// --------------------------------------------------
    /// <summary>
    /// 手配見積データ更新
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="ds">データセット</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>S.Furugo 2018/12/04</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "手配見積データ更新")]
    public bool UpdTehaiMitsumori(CondT01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (WsT01Impl impl = new WsT01Impl())
                {
                    ret = impl.UpdTehaiMitsumori(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region 手配見積削除
    /// --------------------------------------------------
    /// <summary>
    /// 手配見積データ削除
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="ds">データセット</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>D.Okumura 2018/12/21</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "手配見積データ削除")]
    public bool DelTehaiMitsumori(CondT01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (WsT01Impl impl = new WsT01Impl())
                {
                    ret = impl.DelTehaiMitsumori(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region 手配見積受注状態更新
    /// --------------------------------------------------
    /// <summary>
    /// 手配見積受注状態更新
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="ds">更新データ</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>S.Furugo 2018/12/10</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "手配見積受注状態更新")]
    public bool UpdTehaiMitsumoriOrder(CondT01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (WsT01Impl impl = new WsT01Impl())
                {
                    ret = impl.UpdTehaiEstimateOrder(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region 見積MAIL配信

    /// --------------------------------------------------
    /// <summary>
    /// 見積MAIL配信
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="ds">荷姿データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns></returns>
    /// <create>J.Chen 2024/01/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "見積MAIL配信")]
    public bool UpdTehaiEstimateMail(CondT01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (var impl = new WsT01Impl())
                {
                    ret = impl.UpdTehaiEstimateMail(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region 見積情報取得

    /// --------------------------------------------------
    /// <summary>
    /// MAIL配信用累計金額取得
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <returns>ユーザーマスタ</returns>
    /// <create>J.Chen 2024/08/07</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "MAIL配信用累計金額取得")]
    public DataSet GetTotalAmountForMail(CondT01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsT01Impl impl = new WsT01Impl())
                {
                    ds = impl.GetTotalAmountForMail(dbHelper, cond);
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

    #endregion // T0100040:手配見積

    #region T0100050:手配見積明細

    #region 見積明細取得

    /// --------------------------------------------------
    /// <summary>
    /// 手配見積情報取得
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <returns>ユーザーマスタ</returns>
    /// <create>S.Furugo 2018/11/26</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "手配見積情報取得")]
    public DataSet GetTehaiMitsumoriMeisai(CondT01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsT01Impl impl = new WsT01Impl())
                {
                    ds = impl.GetTehaiMitsumoriMeisai(dbHelper, cond);
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

    #region 手配見積明細有償/無償更新

    /// --------------------------------------------------
    /// <summary>
    /// 手配見積明細有償/無償更新
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <create>S.Furugo 2018/11/27</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "手配見積明細有償/無償更新")]
    public bool UpdTehaiMitsumoriMeisaiVersionData(CondT01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (WsT01Impl impl = new WsT01Impl())
                {
                    ret = impl.UpdTehaiMitsumoriMeisaiVersionData(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region T0100060:手配入荷検品

    #region 手配明細取得

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細取得
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <returns>ユーザーマスタ</returns>
    /// <create>S.Furugo 2018/11/16</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "手配明細取得")]
    public DataSet GetTehaiKenpin(CondT01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsT01Impl impl = new WsT01Impl())
                {
                    ds = impl.GetTehaiKenpin(dbHelper, cond);
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

    #region CSV手配明細取得
    /// --------------------------------------------------
    /// <summary>
    /// CSV手配明細取得
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="value">手配No</param>
    /// <returns>ユーザーマスタ</returns>
    /// <create>T.SASAYAMA 2023/07/04</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "手配明細取得CSV")]
    public DataSet GetTehaiKenpinCsv(CondT01 cond, string[] value)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsT01Impl impl = new WsT01Impl())
                {
                    ds = impl.GetTehaiKenpinCsv(dbHelper, cond, value);
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

    #region 手配入荷検品データ更新
    /// --------------------------------------------------
    /// <summary>
    /// 手配入荷検品データ更新
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <create>S.Furugo 2018/11/20</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "手配入荷検品データ更新")]
    public bool UpdTehaiKenpin(CondT01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (WsT01Impl impl = new WsT01Impl())
                {
                    ret = impl.UpdTehaiKenpin(dbHelper, cond, ds, ref errMsgID, ref args);
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
    /// <create>S.Furugo 2018/11/16</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "物件名一覧取得")]
    public DataSet GetBukkenNameList()
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (var impl = new WsT01Impl())
                {
                    ds = impl.GetBukkenNameList(dbHelper);
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

    #region 初期データ取得(入荷検品)

    /// --------------------------------------------------
    /// <summary>
    /// 初期データ取得(入荷検品)
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>K.Tsutsumi 2020/05/31</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "初期データ取得(入荷検品)")]
    public DataSet GetInitTehaiKenpin(CondT01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (var impl = new WsT01Impl())
                {
                    ds = impl.GetInitTehaiKenpin(dbHelper, cond);
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

    #region SKS最終連携日時取得

    /// --------------------------------------------------
    /// <summary>
    /// SKS最終連携日時取得
    /// </summary>
    /// <returns>DataSet</returns>
    /// <create>S.Furugo 2018/11/16</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "SKS最終連携日時取得")]
    public DataSet GetSksLastLink()
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (var impl = new WsT01Impl())
                {
                    ds = impl.GetSksLastLink(dbHelper);
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

    #region T0100070:組立実績登録

    #region 組立実績データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 組立実績データ取得
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/08</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "組立実績データ取得")]
    public DataSet GetKumitateJisseki(CondT01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsT01Impl impl = new WsT01Impl())
                {
                    ds = impl.GetKumitateJisseki(dbHelper, cond);
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

    #region 組立実績データ更新

    /// --------------------------------------------------
    /// <summary>
    /// 組立実績データ更新
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="ds">更新データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:</returns>
    /// <create>H.Tajimi 2018/11/08</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "組立実績データ更新")]
    public bool UpdKumitateJisseki(CondT01 cond, DataSet ds, out string errMsgID, out string[] args)
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
                using (WsT01Impl impl = new WsT01Impl())
                {
                    ret = impl.UpdKumitateJisseki(dbHelper, cond, ds, ref errMsgID, ref args);
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

    #region T0100080:Ship照会

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
    public DataSet GetShukkaMeisai(CondT01 cond, out string errMsgID, out string[] args)
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
                using (WsT01Impl impl = new WsT01Impl())
                {
                    ds = impl.Ctrl_GetShukkaMeisai(dbHelper, cond, ref errMsgID, ref args);
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

    #region T0100090:VALUE照会

    #region 見積明細取得

    /// --------------------------------------------------
    /// <summary>
    /// VALUE情報取得
    /// </summary>
    /// <param name="cond">T01用コンディション</param>
    /// <returns>ユーザーマスタ</returns>
    /// <create>J.Chen 2024/02/20</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "VALUE情報取得")]
    public DataSet GetTehaiMitsumoriValue(CondT01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsT01Impl impl = new WsT01Impl())
                {
                    ds = impl.GetTehaiMitsumoriValue(dbHelper, cond);
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

    #region T0100100:手配明細履歴

    #region 手配明細履歴取得

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細履歴取得
    /// </summary>
    /// <param name="cond">T01用条件</param>
    /// <returns></returns>
    /// <create>J.Chen 2024/11/06</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "手配明細履歴取得")]
    public DataSet GetTehaiMeisaiRireki(CondT01 cond)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                //接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (var impl = new WsT01Impl())
                {
                    ds = impl.GetTehaiMeisaiRireki(dbHelper, cond);
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

}


