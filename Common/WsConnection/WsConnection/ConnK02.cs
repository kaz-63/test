using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WsConnection.WebRefK02;

namespace WsConnection
{
    /// --------------------------------------------------
    /// <summary>
    /// 梱包作業コネクションクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/06/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [DSWUtil.AOP.AspectClass(typeof(AspectConnectionRetry), true)]
    public class ConnK02 : ConnBase
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
        public ConnK02()
        {
        }

        #endregion

        #region ラベル印刷
        /// --------------------------------------------------
        /// <summary>
        /// NoListを取得します。
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="dt"></param>
        /// <param name="errorMsgID"></param>
        /// <returns></returns>
        /// <create>H.Tsunamura 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool GetNoList(CondK02 cond, out DataTable dt, out string errorMsgID)
        {
            try
            {
                using (WsK02 ws = this.GetWsK02())
                {
                    return ws.GetNoList(cond, out dt, out errorMsgID);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Boxリスト発行
        /// --------------------------------------------------
        /// <summary>
        /// ボックスリストを発行していないボックスNoを取得
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tsunamura 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataTable GetBoxList()
        {
            try
            {
                using (WsK02 ws = this.GetWsK02())
                {
                    return ws.GetBoxList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// --------------------------------------------------
        /// <summary>
        /// BOXNoListから明細取得
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        /// <create>H.Tsunamura 2010/07/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetMeisai(DataTable dt)
        {
            try
            {
                using (WsK02 ws = this.GetWsK02())
                {
                    return ws.GetMeisai(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// BOXNoから納入先を取得
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        /// <create>H.Tsunamura 2010/07/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataTable GetNonyusaki(CondK02 cond)
        {
            try
            {
                using (WsK02 ws = this.GetWsK02())
                {
                    return ws.GetNonyusaki(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// BOXLISTMANAGEアップデート
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="dt"></param>
        /// <create>H.Tsunamura 2010/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public void UpdBoxNoKanri(CondK02 cond, DataTable dt)
        {
            try
            {
                using (WsK02 ws = this.GetWsK02())
                {
                    ws.UpdBoxNoKanri(cond, dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region パレットリスト発行
        /// --------------------------------------------------
        /// <summary>
        /// パレットリストを発行していないパレットNoを取得
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tsunamura 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataTable GetPalletList()
        {
            try
            {
                using (WsK02 ws = this.GetWsK02())
                {
                    return ws.GetPalletList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// --------------------------------------------------
        /// <summary>
        /// PALLETNoListから明細取得
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        /// <create>H.Tsunamura 2010/07/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetBoxData(DataTable dt)
        {
            try
            {
                using (WsK02 ws = this.GetWsK02())
                {
                    return ws.GetBoxData(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// PALLETNoから納入先を取得
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        /// <create>H.Tsunamura 2010/07/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataTable GetPalletNo(CondK02 cond)
        {
            try
            {
                using (WsK02 ws = this.GetWsK02())
                {
                    return ws.GetPalletNo(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// PALLETLISTMANAGEアップデート
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="dt"></param>
        /// <create>H.Tsunamura 2010/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public void UpdPalletNoKanri(CondK02 cond, DataTable dt)
        {
            try
            {
                using (WsK02 ws = this.GetWsK02())
                {
                    ws.UpdPalletNoKanri(cond, dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region パッキングリスト発行

        /// --------------------------------------------------
        /// <summary>
        /// パッキングリスト取得
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        /// <create>H.Tsunamura 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetPackingList(CondK02 cond)
        {
            try
            {
                using (WsK02 ws = this.GetWsK02())
                {
                    return ws.GetPackingList(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 印刷用明細取得
        /// </summary>
        /// <param name="cond">K02用コンディション</param>
        /// <param name="dt"></param>
        /// <returns></returns>
        /// <create>H.Tsunamura 2010/07/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetPackingMeisai(CondK02 cond, DataTable dt)
        {
            try
            {
                using (WsK02 ws = this.GetWsK02())
                {
                    return ws.GetPackingMeisai(cond, dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region K0200040:木枠梱包登録 & K0200060:木枠梱包登録(社外)

        #region 木枠梱包登録用データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 木枠梱包登録用データ取得
        /// </summary>
        /// <param name="cond">K02用コンディション</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>表示用データ</returns>
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetKiwakuKonpoTorokuData(CondK02 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsK02 ws = this.GetWsK02())
                {
                    return ws.GetKiwakuKonpoTorokuData(cond, out errMsgID, out  args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #region 編集時チェック

        /// --------------------------------------------------
        /// <summary>
        /// 編集時チェック
        /// </summary>
        /// <param name="dbHelper">DatabaseHelper</param>
        /// <param name="cond">K02用コンディション</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool CheckKiwakuKonpoTorokuEdit(CondK02 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsK02 ws = this.GetWsK02())
                {
                    return ws.CheckKiwakuKonpoTorokuEdit(cond, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #region 木枠梱包登録更新処理

        /// --------------------------------------------------
        /// <summary>
        /// 木枠梱包登録更新処理
        /// </summary>
        /// <param name="cond">K02用コンディション</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdKiwakuKonpoToroku(CondK02 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsK02 ws = this.GetWsK02())
                {
                    return ws.UpdKiwakuKonpoToroku(cond, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #endregion

        #region K0200050:木枠梱包明細登録 & K0200070:木枠梱包明細登録(社外)

        #region 木枠梱包データの取得

        /// --------------------------------------------------
        /// <summary>
        /// 木枠梱包データの取得
        /// </summary>
        /// <param name="cond">K02用コンディション</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>表示用データ</returns>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetKiwakuKonpoMeisaiData(CondK02 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsK02 ws = this.GetWsK02())
                {
                    return ws.GetKiwakuKonpoMeisaiData(cond,out errMsgID,out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 出荷明細データ1件取得

        /// --------------------------------------------------
        /// <summary>
        /// 出荷明細データ1件取得
        /// </summary>
        /// <param name="cond">K02用コンディション</param>
        /// <returns>出荷明細データ1件</returns>
        /// <create>Y.Higuchi 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetKiwakuKonpoShukkaMeisaiFirstRowData(CondK02 cond)
        {
            try
            {
                using (WsK02 ws = this.GetWsK02())
                {
                    return ws.GetKiwakuKonpoShukkaMeisaiFirstRowData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 便間移動データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 便間移動データ取得
        /// </summary>
        /// <param name="ds">更新データ</param>
        /// <param name="cond">K02用コンディション</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/01/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetMoveShipData(DataSet ds, CondK02 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsK02 ws = this.GetWsK02())
                {
                    return ws.GetMoveShipData(ds, cond, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 木枠梱包明細登録更新処理

        /// --------------------------------------------------
        /// <summary>
        /// 木枠梱包明細登録更新処理
        /// </summary>
        /// <param name="dbHelper">DatabaseHelper</param>
        /// <param name="ds">更新データ</param>
        /// <param name="cond">K02用コンディション</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdKiwakuKonpoMeisaiToroku(DataSet ds, CondK02 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsK02 ws = this.GetWsK02())
                {
                    return ws.UpdKiwakuKonpoMeisaiToroku(ds, cond, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #endregion

        #region K0200110:木枠まとめ発行

        #region 木枠まとめデータの取得
        /// --------------------------------------------------
        /// <summary>
        /// 木枠データの取得
        /// </summary>
        /// <param name="cond">K02用コンディション</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>表示用データ(T_KIWAKU/T_KIWAKU_MEISAI/M_NONYUSAKI)</returns>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetKiwakuMatomeData(CondK02 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsK02 ws = this.GetWsK02())
                {
                    return ws.GetKiwakuMatomeData(cond, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region 木枠まとめ印刷用データ取得
        /// --------------------------------------------------
        /// <summary>
        /// 木枠まとめ印刷用データ取得
        /// </summary>
        /// <param name="cond">K02用コンディション</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>印刷用データ(KOJI_NO/T_KIWAKU_MEISAI/T_SHUKKA_MEISAI)</returns>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetMatomePackingMeisai(CondK02 cond, DataTable dt)
        {
            try
            {
                using (WsK02 ws = this.GetWsK02())
                {
                    return ws.GetMatomePackingMeisai(cond, dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #endregion //K0200110:木枠まとめ発行
    }
}
