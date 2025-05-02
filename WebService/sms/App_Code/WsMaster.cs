using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Services;

using Condition;
using Commons;
using DSWUtil.DbUtil;

//// --------------------------------------------------
/// <summary>
/// マスタ系処理（トランザクション層） 
/// </summary>
/// <create>Y.Higuchi 2010/04/15</create>
/// <update></update>
/// --------------------------------------------------
[WebService(Description = "出荷管理システム", Namespace = "http://smssrv/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。 
// [System.Web.Script.Services.ScriptService]
public class WsMaster : WsBase
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/04/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsMaster()
        : base()
    {
    }

    #endregion

    #region 納入先マスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// 納入先マスタ取得
    /// </summary>
    /// <param name="cond">納入先マスタ用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>Y.Higuchi 2010/06/25</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "納入先マスタ取得")]
    public DataSet GetNonyusaki(CondNonyusaki cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsMasterImpl impl = new WsMasterImpl())
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

    #region 権限マスタ

    /// --------------------------------------------------
    /// <summary>
    /// 権限マスタ取得
    /// </summary>
    /// <param name="cond">権限マスタ用コンディション</param>
    /// <returns>権限マスタ</returns>
    /// <create>Y.Higuchi 2010/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "権限マスタ取得")]
    public DataSet GetRole(CondRole cond)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                DataSet ds;
                using (WsMasterImpl impl = new WsMasterImpl())
                {
                    ds = impl.GetRole(dbHelper, cond);
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

