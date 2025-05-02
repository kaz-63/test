using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using WsConnection.WebRefI01;

namespace WsConnection
{
    /// --------------------------------------------------
    /// <summary>
    /// 常駐コネクションクラス
    /// </summary>
    /// <create>T.Wakamatsu 2013/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [DSWUtil.AOP.AspectClass(typeof(AspectConnectionRetry), true)]
    public class ConnI01 : ConnBase
    {
        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        [DSWUtil.AOP.AspectExecute(false)]
        public ConnI01()
        {
        }

        #endregion

        #region 共通処理

        #region ロケーションコンボボックス用データ取得

        /// --------------------------------------------------
        /// <summary>
        /// ロケーションコンボボックス用データ取得
        /// </summary>
        /// <param name="cond">I01用コンディション</param>
        /// <returns>ロケーションマスタ</returns>
        /// <create>T.Wakamatsu 2013/07/31</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetLocationCombo(CondI01 cond)
        {
            try
            {
                using (WsI01 ws = this.GetWsI01())
                {
                    return ws.GetLocationCombo(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region ロケーション保守

        #region ロケーション取得(完全一致・物件管理No必須)

        /// --------------------------------------------------
        /// <summary>
        /// ロケーション取得(完全一致・出荷区分、物件管理No、ロケーション必須)
        /// </summary>
        /// <param name="cond">I01用コンディション</param>
        /// <returns>ロケーションマスタ</returns>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetLocation(CondI01 cond)
        {
            try
            {
                using (WsI01 ws = this.GetWsI01())
                {
                    return ws.GetLocation(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ロケーション取得(あいまい検索)

        /// --------------------------------------------------
        /// <summary>
        /// ロケーション取得(あいまい検索)
        /// </summary>
        /// <param name="cond">I01用コンディション</param>
        /// <returns>ロケーションマスタ</returns>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetLocationLikeSearch(CondI01 cond)
        {
            try
            {
                using (WsI01 ws = this.GetWsI01())
                {
                    return ws.GetLocationLikeSearch(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ロケーションの追加

        /// --------------------------------------------------
        /// <summary>
        /// ロケーションの追加
        /// </summary>
        /// <param name="cond">I01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InsLocationData(CondI01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsI01 ws = this.GetWsI01())
                {
                    return ws.InsLocationData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ロケーションの更新

        /// --------------------------------------------------
        /// <summary>
        /// ロケーションの更新
        /// </summary>
        /// <param name="cond">I01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdLocationData(CondI01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsI01 ws = this.GetWsI01())
                {
                    return ws.UpdLocationData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ロケーションの削除

        /// --------------------------------------------------
        /// <summary>
        /// ロケーションの削除
        /// </summary>
        /// <param name="cond">I01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelLocationData(CondI01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsI01 ws = this.GetWsI01())
                {
                    return ws.DelLocationData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 入庫設定

        #region 出荷・在庫データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 出荷・在庫データ取得
        /// </summary>
        /// <param name="cond">I01用コンディション</param>
        /// <returns>出荷・在庫データ</returns>
        /// <create>T.Wakamatsu 2013/07/31</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetShukkaZaiko(CondI01 cond)
        {
            try
            {
                using (WsI01 ws = this.GetWsI01())
                {
                    return ws.GetShukkaZaiko(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 入庫処理

        /// --------------------------------------------------
        /// <summary>
        /// 入庫処理
        /// </summary>
        /// <param name="cond">I01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InsZaikoData(CondI01 cond, DataTable dt, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsI01 ws = this.GetWsI01())
                {
                    return ws.InsZaikoData(cond, dt, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 出庫設定

        #region 完了データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 完了データ取得
        /// </summary>
        /// <param name="cond">I01用コンディション</param>
        /// <returns>在庫データ</returns>
        /// <create>T.Wakamatsu 2013/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetKanryoData(CondI01 cond)
        {
            try
            {
                using (WsI01 ws = this.GetWsI01())
                {
                    return ws.GetKanryoData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 完了処理

        /// --------------------------------------------------
        /// <summary>
        /// 完了処理
        /// </summary>
        /// <param name="cond">I01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.Wakamatsu 2013/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelKanryoData(CondI01 cond, DataTable dt, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsI01 ws = this.GetWsI01())
                {
                    return ws.DelKanryoData(cond, dt, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 棚卸差異照会

        #region 棚卸ロケーションコンボボックス用データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 棚卸ロケーションコンボボックス用データ取得
        /// </summary>
        /// <param name="cond">I01用コンディション</param>
        /// <returns>ロケーションマスタ</returns>
        /// <create>T.Wakamatsu 2013/10/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetTanaoroshiLocationCombo(CondI01 cond)
        {
            try
            {
                using (WsI01 ws = this.GetWsI01())
                {
                    return ws.GetTanaoroshiLocationCombo(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 棚卸データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 棚卸データ取得
        /// </summary>
        /// <param name="cond">I01用コンディション</param>
        /// <returns>棚卸データ</returns>
        /// <create>T.Wakamatsu 2013/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetTanaoroshiData(CondI01 cond)
        {
            try
            {
                using (WsI01 ws = this.GetWsI01())
                {
                    return ws.GetTanaoroshiData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 棚卸処理

        /// --------------------------------------------------
        /// <summary>
        /// 棚卸処理
        /// </summary>
        /// <param name="cond">I01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.Wakamatsu 2013/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdTanaoroshiData(CondI01 cond, DataTable dt, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsI01 ws = this.GetWsI01())
                {
                    return ws.UpdTanaoroshiData(cond, dt, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 在庫問合せ・メンテ

        #region 在庫メンテ権限取得

        /// --------------------------------------------------
        /// <summary>
        /// 在庫メンテ権限取得
        /// </summary>
        /// <param name="role">ユーザー権限</param>
        /// <returns>在庫メンテ権限</returns>
        /// <create>T.Wakamatsu 2013/09/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool GetZaikoMainteRole(string role)
        {
            try
            {
                using (WsI01 ws = this.GetWsI01())
                {
                    return ws.GetZaikoMainteRole(role);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 在庫データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 在庫データ取得
        /// </summary>
        /// <param name="cond">I01用コンディション</param>
        /// <returns>在庫データ</returns>
        /// <create>T.Wakamatsu 2013/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetZaikoHoshu(CondI01 cond)
        {
            try
            {
                using (WsI01 ws = this.GetWsI01())
                {
                    return ws.GetZaikoHoshu(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 在庫データ更新

        /// --------------------------------------------------
        /// <summary>
        /// 在庫データ更新
        /// </summary>
        /// <param name="cond">I01用コンディション</param>
        /// <param name="dtUpd">更新データ</param>
        /// <param name="dtDel">削除データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.Wakamatsu 2013/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdZaikoHoshu(CondI01 cond, DataTable dtUpd, DataTable dtDel, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsI01 ws = this.GetWsI01())
                {
                    return ws.UpdZaikoHoshu(cond, dtUpd, dtDel, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region オペ履歴照会

        #region オペ履歴取得

        /// --------------------------------------------------
        /// <summary>
        /// オペ履歴取得
        /// </summary>
        /// <param name="cond">I01用コンディション</param>
        /// <returns>オペ履歴データ</returns>
        /// <create>T.Wakamatsu 2013/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetJisseki(CondI01 cond)
        {
            try
            {
                using (WsI01 ws = this.GetWsI01())
                {
                    return ws.GetJisseki(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 取込処理

        #endregion

    }
}
