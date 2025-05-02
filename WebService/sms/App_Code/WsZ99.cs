using System;
using System.Web.Services;

/// --------------------------------------------------
/// <summary>
/// ツール処理クラス（トランザクション層）
/// </summary>
/// <create>T.Sakiori 2012/04/09</create>
/// <update></update>
/// --------------------------------------------------
[WebService(Description = "出荷管理システム", Namespace = "http://smssrv/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。 
// [System.Web.Script.Services.ScriptService]
public class WsZ99 : WsBase
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>T.Sakiori 2012/04/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsZ99()
        : base()
    {
    }

    #endregion

    #region Z9900010:物件名マスタ作成

    #region 物件名マスタデータ作成

    /// --------------------------------------------------
    /// <summary>
    /// 物件名マスタデータ作成
    /// </summary>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/04/09</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "物件名マスタデータ作成")]
    public bool InsBukken(out string errMsgID, out string[] args)
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
                using (var impl = new WsZ99Impl())
                {
                    ret = impl.InsBukken(dbHelper, ref errMsgID, ref args);
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

