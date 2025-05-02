using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using WsConnection.WebRefK01;

namespace WsConnection
{
    /// --------------------------------------------------
    /// <summary>
    /// 集荷作業コネクションクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/06/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [DSWUtil.AOP.AspectClass(typeof(AspectConnectionRetry), true)]
    public class ConnK01 : ConnBase
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
        public ConnK01()
        {
        }

        #endregion

        /// --------------------------------------------------
        /// <summary>
        /// 出荷明細データを取得します。
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="ds"></param>
        /// <param name="errorMsgID"></param>
        /// <param name="kanriNo"></param>
        /// <returns></returns>
        /// <create>H.Tsunamura 2010/07/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool GetShukaData(CondK01 cond, out DataSet ds, out string errorMsgID, out string kanriNo)
        {
            try
            {
                using (WsK01 ws = this.GetWsK01())
                {
                    return ws.GetShukaData(cond, out ds, out errorMsgID, out kanriNo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 発行日付をアップデートします。
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="dtTagNo">TagNoのリスト</param>
        /// <create>H.Tsunamura 2010/07/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public void UpdMeisai(CondK01 cond, DataTable dtTagNo)
        {
            try
            {
                using (WsK01 ws = this.GetWsK01())
                {
                    ws.UpdMeisai(cond, dtTagNo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        /// --------------------------------------------------
        /// <summary>
        /// ロック/解除状態を更新します。
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="ret">ロックか解除か</param>
        /// <create>T.SASAYAMA 2023/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public void LockUnLock(CondK01 cond, bool ret)
        {
            try
            {
                using (WsK01 ws = this.GetWsK01())
                {
                    ws.LockUnLock(cond, ret);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
