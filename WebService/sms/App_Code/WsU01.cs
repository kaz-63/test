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
/// 受入作業処理クラス（トランザクション層）
/// </summary>
/// <create>Y.Higuchi 2010/06/22</create>
/// <update></update>
/// --------------------------------------------------
[WebService(Description = "出荷管理システム", Namespace = "http://smssrv/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。 
// [System.Web.Script.Services.ScriptService]
public class WsU01 : WsBase
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/04/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsU01()
        : base()
    {
    }

    #endregion

    #region 受入情報登録

    #region 受入データ取得
    /// --------------------------------------------------
    /// <summary>
    /// BoxNoから受入データを取得します
    /// </summary>
    /// <param name="cond"></param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/08/20</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "BoxNoから受入データを取得します。")]
    public DataSet GetBoxData(CondU01 cond, out string errMsgID, out string[] args)
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
                using (WsU01Impl impl = new WsU01Impl())
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

    /// --------------------------------------------------
    /// <summary>
    /// パレットNoから受入データを取得します
    /// </summary>
    /// <param name="cond"></param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/08/20</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "パレットNoから受入データを取得します。")]
    public DataSet GetPalletData(CondU01 cond, out string errMsgID, out string[] args)
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
                using (WsU01Impl impl = new WsU01Impl())
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

    #region 更新処理

    /// --------------------------------------------------
    /// <summary>
    /// 受入データをアップデートします
    /// </summary>
    /// <param name="cond"></param>
    /// <param name="dt"></param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <create>T.Wakamatsu 2013/09/04</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "受入データをアップデートします。")]
    public bool UpdUkeireData(CondU01 cond, DataTable dt, out string errMsgID, out string[] args)
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

                using (WsU01Impl impl = new WsU01Impl())
                {
                    ret = impl.UpdUkeireData(dbHelper, cond, dt, ref errMsgID, ref args);
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

    #region 受入情報明細

    /// --------------------------------------------------
    /// <summary>
    /// BoxNoの出荷明細データを取得
    /// </summary>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/08/20</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "BoxNoの出荷明細データを取得します。")]
    public DataSet GetBoxMeisai(CondU01 cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsU01Impl impl = new WsU01Impl())
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
}

