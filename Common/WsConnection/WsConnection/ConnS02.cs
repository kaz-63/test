using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using WsConnection.WebRefS02;

namespace WsConnection
{
    /// --------------------------------------------------
    /// <summary>
    /// 出荷情報コネクションクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/06/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [DSWUtil.AOP.AspectClass(typeof(AspectConnectionRetry), true)]
    public class ConnS02 : ConnBase
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
        public ConnS02()
        {
        }

        #endregion

        #region S0200010:出荷情報登録

        #region Box出荷データ取得

        /// --------------------------------------------------
        /// <summary>
        /// Box出荷データ取得
        /// </summary>
        /// <param name="cond">S02用コンディション</param>
        /// <param name="errMsgID">エラーメッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>DataSet</returns>
        /// <create>Y.Higuchi 2010/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetBoxData(CondS02 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsS02 ws = this.GetWsS02())
                {
                    return ws.GetBoxData(cond, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region パレット出荷データ取得

        /// --------------------------------------------------
        /// <summary>
        /// パレット出荷データ取得
        /// </summary>
        /// <param name="cond">S02用コンディション</param>
        /// <param name="errMsgID">エラーメッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>DataSet</returns>
        /// <create>Y.Higuchi 2010/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetPalletData(CondS02 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsS02 ws = this.GetWsS02())
                {
                    return ws.GetPalletData(cond, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 木枠出荷データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 木枠出荷データ取得
        /// </summary>
        /// <param name="cond">S02用コンディション</param>
        /// <param name="errMsgID">エラーメッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>DataSet</returns>
        /// <create>Y.Higuchi 2010/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetKiwakuData(CondS02 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsS02 ws = this.GetWsS02())
                {
                    return ws.GetKiwakuData(cond, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Box出荷データ更新

        /// --------------------------------------------------
        /// <summary>
        /// Box出荷データ更新
        /// </summary>
        /// <param name="cond">S02用コンディション</param>
        /// <param name="dt">更新データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:</returns>
        /// <create>Y.Higuchi 2010/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdBoxData(CondS02 cond, DataTable dt, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsS02 ws = this.GetWsS02())
                {
                    return ws.UpdBoxData(cond, dt, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region パレット出荷データ更新

        /// --------------------------------------------------
        /// <summary>
        /// パレット出荷データ更新
        /// </summary>
        /// <param name="cond">S02用コンディション</param>
        /// <param name="dt">更新データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:</returns>
        /// <create>Y.Higuchi 2010/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdPalletData(CondS02 cond, DataTable dt, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsS02 ws = this.GetWsS02())
                {
                    return ws.UpdPalletData(cond, dt, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 木枠出荷データ更新

        /// --------------------------------------------------
        /// <summary>
        /// 木枠出荷データ更新
        /// </summary>
        /// <param name="cond">S02用コンディション</param>
        /// <param name="dt">更新データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:</returns>
        /// <create>Y.Higuchi 2010/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdKiwakuData(CondS02 cond, DataTable dt, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsS02 ws = this.GetWsS02())
                {
                    return ws.UpdKiwakuData(cond, dt, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region S0200020:出荷情報明細

        #region BoxNoの出荷明細データを取得

        /// --------------------------------------------------
        /// <summary>
        /// BoxNoの出荷明細データを取得
        /// </summary>
        /// <param name="cond">S02用コンディション</param>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetBoxMeisai(CondS02 cond)
        {
            try
            {
                using (WsS02 ws = this.GetWsS02())
                {
                    return ws.GetBoxMeisai(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region S0200030:出荷情報照会

        #region 出荷明細データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 出荷明細データ取得
        /// </summary>
        /// <param name="cond">S02用コンディション</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>表示データ</returns>
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetShukkaMeisai(CondS02 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsS02 ws = this.GetWsS02())
                {
                    return ws.GetShukkaMeisai(cond, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #endregion

        #region S0200040:ShippingDocument作成

        #region 荷受マスタ取得

        /// --------------------------------------------------
        /// <summary>
        /// 荷受マスタ取得
        /// </summary>
        /// <param name="cond">S02用コンディション</param>
        /// <returns>表示データ</returns>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetConsign(CondS02 cond)
        {
            try
            {
                using (WsS02 ws = this.GetWsS02())
                {
                    return ws.GetConsign(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #region 配送先マスタ取得

        /// --------------------------------------------------
        /// <summary>
        /// 配送先マスタ取得
        /// </summary>
        /// <param name="cond">S02用コンディション</param>
        /// <returns>表示データ</returns>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetDeliver(CondS02 cond)
        {
            try
            {
                using (WsS02 ws = this.GetWsS02())
                {
                    return ws.GetDeliver(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #region 荷姿情報取得

        /// --------------------------------------------------
        /// <summary>
        /// 荷姿情報取得
        /// </summary>
        /// <param name="cond">S02用コンディション</param>
        /// <returns>表示データ</returns>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetNisugata(CondS02 cond)
        {
            try
            {
                using (WsS02 ws = this.GetWsS02())
                {
                    return ws.GetNisugata(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #region Invoice作成事前確認

        /// --------------------------------------------------
        /// <summary>
        /// Invoice作成事前確認
        /// </summary>
        /// <param name="cond">S02用コンディション</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>表示データ</returns>
        /// <create>T.Nakata 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataTable CheckInvoice(CondS02 cond, DataTable dt)
        {
            try
            {
                using (WsS02 ws = this.GetWsS02())
                {
                    return ws.CheckInvoice(cond, dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #region 各種テーブル更新

        /// --------------------------------------------------
        /// <summary>
        /// 各種テーブル更新
        /// </summary>
        /// <param name="cond">S02用コンディション</param>
        /// <param name="dt">更新データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:</returns>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdShippingData(CondS02 cond, DataTable dt, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsS02 ws = this.GetWsS02())
                {
                    return ws.UpdShippingData(cond, dt, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 帳票用データ取得

        #region  帳票用データ取得ShippingDocument

        /// --------------------------------------------------
        /// <summary>
        /// 帳票用データ取得ShippingDocument
        /// </summary>
        /// <param name="cond">S02用コンディション</param>
        /// <returns>表示データ</returns>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update>D.Okumura 2021/01/15 EFA_SMS-184 帳票データ取得処理を統合</update>
        /// --------------------------------------------------
        public DataSet GetShippingDocument(CondS02 cond)
        {
            try
            {
                using (WsS02 ws = this.GetWsS02())
                {
                    return ws.GetShippingDocument(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #region 帳票用データ取得D

        /// --------------------------------------------------
        /// <summary>
        /// 帳票用データ取得D
        /// </summary>
        /// <param name="cond">S02用コンディション</param>
        /// <returns>表示データ</returns>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetTyouhyouDataD(CondS02 cond)
        {
            try
            {
                using (WsS02 ws = this.GetWsS02())
                {
                    return ws.GetTyouhyouDataD(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #region 帳票用データ取得E

        /// --------------------------------------------------
        /// <summary>
        /// 帳票用データ取得E
        /// </summary>
        /// <param name="cond">S02用コンディション</param>
        /// <returns>表示データ</returns>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetTyouhyouDataE(CondS02 cond)
        {
            try
            {
                using (WsS02 ws = this.GetWsS02())
                {
                    return ws.GetTyouhyouDataE(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #region 帳票用データ取得G

        /// --------------------------------------------------
        /// <summary>
        /// 帳票用データ取得G
        /// </summary>
        /// <param name="cond">S02用コンディション</param>
        /// <returns>表示データ</returns>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetTyouhyouDataG(CondS02 cond)
        {
            try
            {
                using (WsS02 ws = this.GetWsS02())
                {
                    return ws.GetTyouhyouDataG(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #region 帳票用データ取得H

        /// --------------------------------------------------
        /// <summary>
        /// 帳票用データ取得H
        /// </summary>
        /// <param name="cond">S02用コンディション</param>
        /// <returns>表示データ</returns>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetTyouhyouDataH(CondS02 cond)
        {
            try
            {
                using (WsS02 ws = this.GetWsS02())
                {
                    return ws.GetTyouhyouDataH(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #endregion

        #endregion

    }
}
