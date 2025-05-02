using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using WsConnection.WebRefMaster;

namespace WsConnection
{
    /// --------------------------------------------------
    /// <summary>
    /// マスタ系コネクションクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/06/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [DSWUtil.AOP.AspectClass(typeof(AspectConnectionRetry), true)]
    public class ConnMaster : ConnBase
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
        public ConnMaster()
        {
        }

        #endregion

        #region 納入先マスタ

        #region 納入先マスタ取得

        /// --------------------------------------------------
        /// <summary>
        /// 納入先マスタ取得
        /// </summary>
        /// <param name="cond">納入先マスタ用コンディション</param>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetNonyusaki(CondNonyusaki cond)
        {
            try
            {
                using (WsMaster ws = this.GetWsMaster())
                {
                    return ws.GetNonyusaki(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #endregion

        #region 権限マスタ

        #region 権限マスタ取得

        /// --------------------------------------------------
        /// <summary>
        /// 権限マスタ取得
        /// </summary>
        /// <param name="cond">権限マスタ用コンディション</param>
        /// <returns>権限マスタ</returns>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetRole(CondRole cond)
        {
            try
            {
                using (WsMaster ws = this.GetWsMaster())
                {
                    return ws.GetRole(cond);
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
