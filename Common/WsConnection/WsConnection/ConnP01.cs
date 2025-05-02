using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WsConnection.WebRefP01;

namespace WsConnection
{
    /// --------------------------------------------------
    /// <summary>
    /// 共通コネクションクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/06/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [DSWUtil.AOP.AspectClass(typeof(AspectConnectionRetry), true)]
    public class ConnP01 : ConnBase
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
        public ConnP01()
        {
        }

        #endregion
    }
}
