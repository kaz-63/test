using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WsConnection.WebRefU01;
using System.Data;

namespace WsConnection
{
    /// --------------------------------------------------
    /// <summary>
    /// 受入作業コネクションクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/06/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [DSWUtil.AOP.AspectClass(typeof(AspectConnectionRetry), true)]
    public class ConnU01 : ConnBase
    {
        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>Y.Higuchi 2010/06/23</create>
        /// <update></update>
        /// --------------------------------------------------
        [DSWUtil.AOP.AspectExecute(false)]
        public ConnU01()
        {
        }

        #endregion

        /// --------------------------------------------------
        /// <summary>
        /// BoxNoから受入データ取得
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="errMsgID"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <create>H.Tsunamura 2010/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public System.Data.DataSet GetBoxData(CondU01 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsU01 ws = this.GetWsU01())
                {
                    return ws.GetBoxData(cond, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// パレットNoから受入データ取得
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="errMsgID"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <create>H.Tsunamura 2010/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public System.Data.DataSet GetPalletData(CondU01 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsU01 ws = this.GetWsU01())
                {
                    return ws.GetPalletData(cond, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 受入データ更新
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="dt"></param>
        /// <param name="errMsgID"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <create>T.Wakamatsu 2013/09/04</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdUkeireData(CondU01 cond, DataTable dt, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsU01 ws = this.GetWsU01())
                {
                    return ws.UpdUkeireData(cond, dt, out  errMsgID, out  args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// BoxNoから明細データを取得
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        /// <create>H.Tsunamura 2010/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetBoxMeisai(CondU01 cond)
        {
            try
            {
                using (WsU01 ws = this.GetWsU01())
                {
                    return ws.GetBoxMeisai(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

    }
}
