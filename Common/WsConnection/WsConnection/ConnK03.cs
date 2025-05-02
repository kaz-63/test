using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using WsConnection.WebRefK03;

namespace WsConnection
{
    /// --------------------------------------------------
    /// <summary>
    /// 木枠作成コネクションクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/06/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [DSWUtil.AOP.AspectClass(typeof(AspectConnectionRetry), true)]
    public class ConnK03 : ConnBase
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
        public ConnK03()
        {
        }

        #endregion

        #region K0300010:木枠登録

        #region 木枠データ存在チェック

        /// --------------------------------------------------
        /// <summary>
        /// 木枠データ存在チェック
        /// </summary>
        /// <param name="cond">K03用コンディション</param>
        /// <returns>true:存在する false:存在しない</returns>
        /// <create>M.Tsutsumi 2010/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool CheckExistenceKiwaku(CondK03 cond)
        {
            try
            {
                using (WsK03 ws = this.GetWsK03())
                {
                    return ws.CheckExistenceKiwaku(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 木枠データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 木枠データ取得
        /// </summary>
        /// <param name="cond">K03用コンディション</param>
        /// <param name="errMsgID">メッセージ</param>
        /// <param name="args">パラメータ</param>
        /// <returns>データセット</returns>
        /// <create>M.Tsutsumi 2010/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetKiwaku(CondK03 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsK03 ws = this.GetWsK03())
                {
                    return ws.GetKiwaku(cond, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 木枠データ登録

        /// --------------------------------------------------
        /// <summary>
        /// 木枠データ登録
        /// </summary>
        /// <param name="cond">K03用コンディション</param>
        /// <param name="ds">データセット</param>
        /// <param name="kojiNo">工事識別管理No</param>
        /// <param name="errMsgID">メッセージ</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>M.Tsutsumi 2010/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InsKiwaku(CondK03 cond, DataSet ds, out string kojiNo, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsK03 ws = this.GetWsK03())
                {
                    return ws.InsKiwaku(cond, ds, out kojiNo, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 木枠データ更新

        /// --------------------------------------------------
        /// <summary>
        /// 木枠データ更新
        /// </summary>
        /// <param name="cond">K03用コンディション</param>
        /// <param name="ds">データセット</param>
        /// <param name="errMsgID">メッセージ</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>M.Tsutsumi 2010/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdKiwaku(CondK03 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsK03 ws = this.GetWsK03())
                {
                    return ws.UpdKiwaku(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 木枠データ削除

        /// --------------------------------------------------
        /// <summary>
        /// 木枠データ削除
        /// </summary>
        /// <param name="cond">K03用コンディション</param>
        /// <param name="ds">データセット</param>
        /// <param name="errMsgID">メッセージ</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>M.Tsutsumi 2010/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelKiwaku(CondK03 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsK03 ws = this.GetWsK03())
                {
                    return ws.DelKiwaku(cond, ds, out errMsgID, out args);
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
