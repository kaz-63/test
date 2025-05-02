using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using WsConnection.WebRefZ99;

namespace WsConnection
{
    /// --------------------------------------------------
    /// <summary>
    /// ツールコネクションクラス
    /// </summary>
    /// <create>T.Sakiori 2012/04/09</create>
    /// <update></update>
    /// --------------------------------------------------
    [DSWUtil.AOP.AspectClass(typeof(AspectConnectionRetry), true)]
    public class ConnZ99 : ConnBase
    {
        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>T.Sakiori 2012/04/09</create>
        /// <update></update>
        /// --------------------------------------------------
        [DSWUtil.AOP.AspectExecute(false)]
        public ConnZ99()
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
        public bool InsBukken(out string errMsgID, out string[] args)
        {
            try
            {
                using (WsZ99 ws = this.GetWsZ99())
                {
                    return ws.InsBukken(out errMsgID, out args);
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
