using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using WsConnection.WebRefI02;

namespace WsConnection
{
    /// --------------------------------------------------
    /// <summary>
    /// 取込結果コネクションクラス
    /// </summary>
    /// <create>T.Wakamatsu 2013/08/21</create>
    /// <update></update>
    /// --------------------------------------------------
    [DSWUtil.AOP.AspectClass(typeof(AspectConnectionRetry), true)]
    public class ConnI02 : ConnBase
    {
        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        [DSWUtil.AOP.AspectExecute(false)]
        public ConnI02()
        {
        }

        #endregion

        #region I0200010:取込エラー一覧

        #region 一時取込データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 一時取込データ取得
        /// </summary>
        /// <param name="cond">I02用コンディション</param>
        /// <returns>一時取込データ</returns>
        /// <create>T.Wakamatsu 2013/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetTempwork(CondI02 cond)
        {
            try
            {
                using (WsI02 ws = this.GetWsI02())
                {
                    return ws.GetTempwork(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ハンディデータ取込

        /// --------------------------------------------------
        /// <summary>
        /// ハンディデータ取込
        /// </summary>
        /// <param name="cond">I02用コンディション</param>
        /// <param name="ds">取込データ</param>
        /// <param name="dtMessage">メッセージテーブル</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.Wakamatsu 2013/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool ImportData(CondI02 cond, DataSet ds, ref DataTable dtMessage)
        {
            try
            {
                using (WsI02 ws = this.GetWsI02())
                {
                    return ws.ImportData(cond, ds, ref dtMessage);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ハンディデータ取込再試行

        /// --------------------------------------------------
        /// <summary>
        /// ハンディデータ取込再試行
        /// </summary>
        /// <param name="cond">I02用コンディション</param>
        /// <param name="dtMessage">メッセージテーブル</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.Wakamatsu 2013/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool ImportRetry(CondI02 cond, ref DataTable dtMessage)
        {
            try
            {
                using (WsI02 ws = this.GetWsI02())
                {
                    return ws.ImportRetry(cond, ref dtMessage);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #region ハンディデータ破棄

        /// --------------------------------------------------
        /// <summary>
        /// ハンディデータ破棄
        /// </summary>
        /// <param name="cond">I02用コンディション</param>
        /// <param name="dtMessage">メッセージテーブル</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.Wakamatsu 2013/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DestroyData(CondI02 cond, ref DataTable dtMessage)
        {
            try
            {
                using (WsI02 ws = this.GetWsI02())
                {
                    return ws.DestroyData(cond,ref dtMessage);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #endregion

        #region 取込エラー詳細

        #region 一時取込明細データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 一時取込明細データ取得
        /// </summary>
        /// <param name="cond">I02用コンディション</param>
        /// <returns>一時取込明細データ</returns>
        /// <create>T.Wakamatsu 2013/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetTempworkMeisai(CondI02 cond)
        {
            try
            {
                using (WsI02 ws = this.GetWsI02())
                {
                    return ws.GetTempworkMeisai(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 一時取込明細データ削除

        /// --------------------------------------------------
        /// <summary>
        /// 一時取込明細データ削除
        /// </summary>
        /// <param name="cond">I01用コンディション</param>
        /// <param name="dt">削除データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.Wakamatsu 2013/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelTempWorkMeisai(CondI02 cond, DataTable dt, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsI02 ws = this.GetWsI02())
                {
                    return ws.DelTempWorkMeisai(cond, dt, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

    }
}
