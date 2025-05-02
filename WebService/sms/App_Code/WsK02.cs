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
/// 梱包作業処理クラス（トランザクション層）
/// </summary>
/// <create>Y.Higuchi 2010/06/22</create>
/// <update></update>
/// --------------------------------------------------
[WebService(Description = "出荷管理システム", Namespace = "http://smssrv/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。 
// [System.Web.Script.Services.ScriptService]
public class WsK02 : WsBase
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/06/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsK02()
        : base()
    {
    }

    #endregion

    #region ラベル印刷
    /// --------------------------------------------------
    /// <summary>
    /// NoList取得
    /// </summary>
    /// <param name="cond"></param>
    /// <param name="noList"></param>
    /// <param name="errorMsgID"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/07/15</create>
    /// <update>D.Okumura 2021/03/18 PalletListNo重複出力不具合対応、印刷時もトランザクションを張るように修正</update>
    /// --------------------------------------------------
    [WebMethod(Description = "BOXNO・パレットNoのリストを取得します")]
    public bool GetNoList(CondK02 cond, out DataTable noList, out string errorMsgID)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                dbHelper.BeginTransaction();

                // 実行
                bool ret = false;
                using (WsK02Impl impl = new WsK02Impl())
                {
                    ret = impl.GetNoList(dbHelper, cond, out noList, out errorMsgID);
                }
                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (cond.IsPreview)
                {
                    dbHelper.Rollback();
                }
                else
                {
                    dbHelper.Commit();
                }
                // 切断
                dbHelper.Close();
            }
        }
    }

    #endregion

    #region BOXリスト発行
    /// --------------------------------------------------
    /// <summary>
    /// ボックスリストを発行していないボックスNoを取得。
    /// </summary>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/07/15</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ボックスリストを発行していないボックスNoを取得。")]
    public DataTable GetBoxList()
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataTable dt;
                using (WsK02Impl impl = new WsK02Impl())
                {
                    dt = impl.GetBoxNo(dbHelper, "");
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
    /// BOXNoListから明細を取得
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/07/21</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "出荷明細データを取得")]
    public DataSet GetMeisai(DataTable dt)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet retds;
                using (WsK02Impl impl = new WsK02Impl())
                {
                    retds = impl.GetMeisai(dbHelper, dt);
                }
                return retds;
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
    /// BOXNoから納入先を取得
    /// </summary>
    /// <param name="boxNo"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/07/21</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "BOXNoから納入先を取得")]
    public DataTable GetNonyusaki(CondK02 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataTable dt;
                using (WsK02Impl impl = new WsK02Impl())
                {
                    dt = impl.GetBoxNo(dbHelper, cond.BoxNo);
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
    /// ボックスリストマネージャーアップデート
    /// </summary>
    /// <param name="cond"></param>
    /// <param name="dt"></param>
    /// <create>H.Tsunamura 2010/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ボックスリストマネージャーをアップデートします")]
    public void UpdBoxNoKanri(CondK02 cond, DataTable dt)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                using (WsK02Impl impl = new WsK02Impl())
                {
                    impl.UpdBoxNoKanri(dbHelper, cond, dt);
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

    #region パレットリスト発行

    /// --------------------------------------------------
    /// <summary>
    /// 未発行のパレットNoリストを取得
    /// </summary>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "未発行のパレットNoリストを取得")]
    public DataTable GetPalletList()
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataTable dt;
                using (WsK02Impl impl = new WsK02Impl())
                {
                    dt = impl.GetPalletNo(dbHelper, "");
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
    /// パレットNoから納入先・便を取得
    /// </summary>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "パレットNoから納入先・便を取得")]
    public DataTable GetPalletNo(CondK02 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataTable dt;
                using (WsK02Impl impl = new WsK02Impl())
                {
                    dt = impl.GetPalletNo(dbHelper, cond.PalletNo);
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
    /// パレットNoリストからそれぞれのBoxNoを取得
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "パレットNoリストからそれぞれのBoxNoを取得")]
    public DataSet GetBoxData(DataTable dt)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsK02Impl impl = new WsK02Impl())
                {
                    ds = impl.GetBoxData(dbHelper, dt);
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
    /// パレットリストマネージャーをアップデートします
    /// </summary>
    /// <param name="cond"></param>
    /// <param name="dt"></param>
    /// <create>H.Tsunamura 2010/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "パレットリストマネージャーをアップデートします")]
    public void UpdPalletNoKanri(CondK02 cond, DataTable dt)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                using (WsK02Impl impl = new WsK02Impl())
                {
                    impl.UpdPalletNoKanri(dbHelper, cond, dt);
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

    #region パッキングリスト発行
    /// --------------------------------------------------
    /// <summary>
    /// 工事識別No＋便　もしくは発行選択から管理No・作業状況等を取得
    /// </summary>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/07/28</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "管理No・作業状況を返却します。")]
    public DataSet GetPackingList(CondK02 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsK02Impl impl = new WsK02Impl())
                {
                    if (impl.kojiCheck(dbHelper, cond))
                    {
                        return null;
                    }

                    ds = impl.GetPackingList(dbHelper, cond);
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
    /// パッキング発行の帳票用データを取得
    /// </summary>
    /// <param name="cond">K02用コンディション(CondBaseのみ使用)</param>
    /// <param name="dt"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/08/18</create>
    /// <update>D.Okumura 2018/08/31 多言語化対応K02用コンディションを追加</update>
    /// --------------------------------------------------
    [WebMethod(Description = "パッキング発行の帳票用データを取得します。")]
    public DataSet GetPackingMeisai(CondK02 cond, DataTable dt)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsK02Impl impl = new WsK02Impl())
                {
                    ds = impl.GetPackingMeisai(dbHelper, dt, cond.LoginInfo.Language);
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

    #region K0200040:木枠梱包登録 & K0200060:木枠梱包登録(社外)

    #region 木枠梱包登録用データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 木枠梱包登録用データ取得
    /// </summary>
    /// <param name="cond">K02用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>表示用データ</returns>
    /// <create>Y.Higuchi 2010/07/29</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "木枠梱包登録用データ取得")]
    public DataSet GetKiwakuKonpoTorokuData(CondK02 cond, out string errMsgID, out string[] args)
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
                using (WsK02Impl impl = new WsK02Impl())
                {
                    ds = impl.GetKiwakuKonpoTorokuData(dbHelper, cond, ref errMsgID, ref args);
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

    #region 編集時チェック

    /// --------------------------------------------------
    /// <summary>
    /// 編集時チェック
    /// </summary>
    /// <param name="cond">K02用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns></returns>
    /// <create>Y.Higuchi 2010/07/29</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "編集時チェック")]
    public bool CheckKiwakuKonpoTorokuEdit(CondK02 cond, out string errMsgID, out string[] args)
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
                bool ret;
                using (WsK02Impl impl = new WsK02Impl())
                {
                    ret = impl.CheckKiwakuKonpoTorokuEdit(dbHelper, cond, ref errMsgID, ref args);
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

    #region 木枠梱包登録更新処理

    /// --------------------------------------------------
    /// <summary>
    /// 木枠梱包登録更新処理
    /// </summary>
    /// <param name="cond">K02用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "木枠梱包登録更新処理")]
    public bool UpdKiwakuKonpoToroku(CondK02 cond, out string errMsgID, out string[] args)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            errMsgID = string.Empty;
            args = null;
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // トランザクション開始
                dbHelper.BeginTransaction();

                // 実行
                bool ret = true;
                using (WsK02Impl impl = new WsK02Impl())
                {
                    ret = impl.UpdKiwakuKonpoToroku(dbHelper, cond, ref errMsgID, ref args);
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

    #region K0200050:木枠梱包明細登録 & K0200070:木枠梱包明細登録(社外)

    #region 木枠梱包データの取得

    /// --------------------------------------------------
    /// <summary>
    /// 木枠梱包データの取得
    /// </summary>
    /// <param name="cond">K02用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>表示用データ</returns>
    /// <create>Y.Higuchi 2010/08/02</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "木枠梱包データの取得")]
    public DataSet GetKiwakuKonpoMeisaiData(CondK02 cond, out string errMsgID, out string[] args)
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
                using (WsK02Impl impl = new WsK02Impl())
                {
                    ds = impl.GetKiwakuKonpoMeisaiData(dbHelper, cond, ref errMsgID, ref args);
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

    #region 便間移動データの取得

    /// --------------------------------------------------
    /// <summary>
    /// 便間移動データの取得
    /// </summary>
    /// <param name="ds">更新データ</param>
    /// <param name="cond">K02用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/08</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "便間移動データ取得")]
    public DataSet GetMoveShipData(DataSet ds, CondK02 cond, out string errMsgID, out string[] args)
    {
        using (var dbHelper = this.GetDatabaseHelper())
        {
            errMsgID = string.Empty;
            args = null;
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet retDs;
                using (WsK02Impl impl = new WsK02Impl())
                {
                    // 便間移動チェックが必要かどうか
                    if (!impl.IsNeedMoveShipCheck(dbHelper, cond))
                    {
                        return null;
                    }

                    // 便間移動データ取得
                    retDs = impl.GetMoveShipData(dbHelper, ds, cond, ref errMsgID, ref args);
                }
                return retDs;
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

    #region 出荷明細データ1件取得

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ1件取得
    /// </summary>
    /// <param name="cond">K02用コンディション</param>
    /// <returns>出荷明細データ1件</returns>
    /// <create>Y.Higuchi 2010/08/03</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "出荷明細データ1件取得")]
    public DataSet GetKiwakuKonpoShukkaMeisaiFirstRowData(CondK02 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsK02Impl impl = new WsK02Impl())
                {
                    ds = impl.GetKiwakuKonpoShukkaMeisaiFirstRowData(dbHelper, cond);
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

    #region 木枠梱包明細登録更新処理

    /// --------------------------------------------------
    /// <summary>
    /// 木枠梱包明細登録更新処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="ds">更新データ</param>
    /// <param name="cond">K02用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/03</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "木枠梱包明細登録更新処理")]
    public bool UpdKiwakuKonpoMeisaiToroku(DataSet ds, CondK02 cond, out string errMsgID, out string[] args)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            errMsgID = string.Empty;
            args = null;
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // トランザクション開始
                dbHelper.BeginTransaction();

                // 実行
                bool ret = false;
                using (WsK02Impl impl = new WsK02Impl())
                {
                    ret = impl.UpdKiwakuKonpoMeisaiToroku(dbHelper, ds, cond, ref errMsgID, ref args);
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

    #region K0200110:木枠まとめ発行

    #region 木枠まとめデータの取得
    /// --------------------------------------------------
    /// <summary>
    /// 木枠梱包データの取得
    /// </summary>
    /// <param name="cond">K02用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>表示データ(T_KIWAKU, T_KIWAKU_MEISAI, M_NONYUSAKI)</returns>
    /// <create>D.Okumura 2019/09/02</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "木枠まとめデータの取得")]
    public DataSet GetKiwakuMatomeData(CondK02 cond, out string errMsgID, out string[] args)
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
                using (WsK02Impl impl = new WsK02Impl())
                {
                    ds = impl.GetKiwakuMatomeData(dbHelper, cond, ref errMsgID, ref args);
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

    #region 木枠まとめ印刷用データの取得
    /// --------------------------------------------------
    /// <summary>
    /// 木枠まとめ印刷用データの取得
    /// </summary>
    /// <param name="cond">K02用コンディション</param>
    /// <param name="dt">取得対象データ</param>
    /// <returns>印刷用データ(T_KIWAKU_MEISAI, KOJI_NO, T_SHUKKA_MEISAI)</returns>
    /// <create>D.Okumura 2019/09/02</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "木枠まとめ印刷用データの取得")]
    public DataSet GetMatomePackingMeisai(CondK02 cond, DataTable dt)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsK02Impl impl = new WsK02Impl())
                {
                    ds = impl.GetMatomePackingMeisai(dbHelper, dt, cond.LoginInfo.Language);
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
    

    #endregion //K0200110:木枠まとめ発行
}