using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WsConnection.WebRefP02;

namespace WsConnection
{
    /// --------------------------------------------------
    /// <summary>
    /// 共通ﾀﾞｲｱﾛｸﾞコネクションクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/06/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [DSWUtil.AOP.AspectClass(typeof(AspectConnectionRetry), true)]
    public class ConnP02 : ConnBase
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
        public ConnP02()
        {
        }

        #endregion

        #region P0200010:納入先一覧

        #region 納入先一覧取得

        /// --------------------------------------------------
        /// <summary>
        /// 納入先一覧取得
        /// </summary>
        /// <param name="cond">納入先マスタ用コンディション</param>
        /// <returns>DataSet</returns>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public System.Data.DataSet GetNonyusakiIchiran(CondNonyusaki cond)
        {
            try
            {
                using (WsP02 ws = this.GetWsP02())
                {
                    return ws.GetNonyusakiIchiran(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #region 履歴データに紐付く物件名一覧取得

        /// --------------------------------------------------
        /// <summary>
        /// 履歴データに紐付く物件名一覧取得
        /// </summary>
        /// <param name="cond">納入先マスタ用コンディション</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2012/05/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public System.Data.DataSet GetRirekiBukkenIchiran(CondNonyusaki cond)
        {
            try
            {
                using (WsP02 ws = this.GetWsP02())
                {
                    return ws.GetRirekiBukkenIchiran(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region P0200030:工事識別一覧

        #region 工事識別一覧取得

        /// --------------------------------------------------
        /// <summary>
        /// 工事識別一覧取得
        /// </summary>
        /// <param name="cond">木枠データ用コンディション</param>
        /// <returns>DataSet</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public System.Data.DataSet GetKojiShikibetsuIchiran(CondKiwaku cond)
        {
            try
            {
                using (WsP02 ws = this.GetWsP02())
                {
                    return ws.GetKojiShikibetsuIchiran(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #endregion

        #region P0200040:物件名一覧取得

        #region 物件名一覧取得

        /// --------------------------------------------------
        /// <summary>
        /// 物件名一覧取得
        /// </summary>
        /// <param name="cond">物件データ用コンディション</param>
        /// <returns>DataSet</returns>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public System.Data.DataSet GetBukkenName(CondBukken cond)
        {
            try
            {
                using (WsP02 ws = this.GetWsP02())
                {
                    return ws.GetBukkenNameIchiran(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region P0200050:履歴照会

        #region 処理名コンボボックスデータ取得

        /// --------------------------------------------------
        /// <summary>
        /// 処理名コンボボックスデータ取得
        /// </summary>
        /// <param name="cond">P02用コンディション</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public System.Data.DataSet GetOperationFlag(CondP02 cond)
        {
            try
            {
                using (WsP02 ws = this.GetWsP02())
                {
                    return ws.GetOperationFlag(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 履歴一覧取得

        /// --------------------------------------------------
        /// <summary>
        /// 履歴一覧取得
        /// </summary>
        /// <param name="cond">P02用コンディション</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public System.Data.DataSet GetRireki(CondP02 cond)
        {
            try
            {
                using (WsP02 ws = this.GetWsP02())
                {
                    return ws.GetRireki(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 締めマスタ取得

        /// --------------------------------------------------
        /// <summary>
        /// 締めマスタ取得
        /// </summary>
        /// <returns></returns>
        /// <create>T.Sakiori 2012/06/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public System.Data.DataSet GetShime()
        {
            try
            {
                using (WsP02 ws = this.GetWsP02())
                {
                    return ws.GetShime();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region P0200060:送信先設定

        #region 送信先取得

        /// --------------------------------------------------
        /// <summary>
        /// 送信先取得
        /// </summary>
        /// <param name="cond">P02用コンディション</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public System.Data.DataTable GetSendUser(CondP02 cond)
        {
            try
            {
                using (var ws = this.GetWsP02())
                {
                    return ws.GetSendUser(cond);
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
