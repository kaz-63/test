using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WsConnection.WebRefSms;

namespace WsConnection
{
    /// --------------------------------------------------
    /// <summary>
    /// 製番用共通コネクションクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/06/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [DSWUtil.AOP.AspectClass(typeof(AspectConnectionRetry), true)]
    public class ConnSms : ConnBase
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
        public ConnSms()
        {
        }

        #endregion

        public bool GetSaiban(CondSms cond, out string saiban, out string errorMsgID)
        {
            try
            {
                using (WsSms ws = new WsSms())
                {
                    return ws.GetSaiban(cond,out saiban,out errorMsgID);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

    }
}
