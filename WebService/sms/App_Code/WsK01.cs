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
/// 集荷作業処理クラス（トランザクション層）
/// </summary>
/// <create>Y.Higuchi 2010/06/22</create>
/// <update></update>
/// --------------------------------------------------
[WebService(Description = "出荷管理システム", Namespace = "http://smssrv/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。 
// [System.Web.Script.Services.ScriptService]
public class WsK01 : WsBase
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/06/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsK01()
        : base()
    {
    }

    #endregion

    /// --------------------------------------------------
    /// <summary>
    /// 出荷データ取得
    /// </summary>
    /// <param name="cond"></param>
    /// <param name="ds"></param>
    /// <param name="errorMsgID"></param>
    /// <param name="kanriNo"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/06/28</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "チェック・検索を行い、出荷データを取得します")]
    public bool GetShukaData(CondK01 cond, out DataSet ds, out string errorMsgID, out string kanriNo)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                bool ret = false;
                using (WsK01Impl impl = new WsK01Impl())
                {
                    ret = impl.GetShukaData(dbHelper, cond, out ds, out errorMsgID, out kanriNo);
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

    /// --------------------------------------------------
    /// <summary>
    /// 発行日付のUPDATE
    /// </summary>
    /// <param name="cond"></param>
    /// <param name="dtTagNo"></param>
    /// <create>H.Tsunamura 2010/07/07</create>
    /// <update>J.Chen 履歴登録処理追加</update>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "発行日付をアップデートします。")]
    public void UpdMeisai(CondK01 cond, DataTable dtTagNo)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 更新日時セット
                cond.UpdateDate = DateTime.Now;

                // 実行
                int ret = 0;
                using (WsK01Impl impl = new WsK01Impl())
                {
                    ret = impl.UpdMeisai(dbHelper, cond, dtTagNo);

                    if (ret > 0)
                    {
                        impl.InsRireki(dbHelper, cond);
                    }
                }
                return;
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
    /// ロック/解除状態のUPDATE
    /// </summary>
    /// <param name="cond"></param>
    /// <param name="ret"></param>
    /// <create>T.SASAYAMA 2023/06/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "ロック/解除")]
    public void LockUnLock(CondK01 cond, bool ret)
    {
        using (DatabaseHelper dbHelper = this.GetDatabaseHelper())
        {
            try
            {
                // 接続
                this.DbOpen(dbHelper);

                // 実行
                using (WsK01Impl impl = new WsK01Impl())
                {
                    impl.LockUnLock(dbHelper, cond, ret);
                }
                return;
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


}

