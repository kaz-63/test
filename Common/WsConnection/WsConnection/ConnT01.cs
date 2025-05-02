using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WsConnection.WebRefT01;
using System.Data;

namespace WsConnection
{
    /// --------------------------------------------------
    /// <summary>
    /// 手配明細コネクションクラス
    /// </summary>
    /// <create>S.Furugo 2018/10/26</create>
    /// <update></update>
    /// --------------------------------------------------
    [DSWUtil.AOP.AspectClass(typeof(AspectConnectionRetry), true)]
    public class ConnT01 : ConnBase
    {
        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>S.Furugo 2018/11/05</create>
        /// <update></update>
        /// --------------------------------------------------
        [DSWUtil.AOP.AspectExecute(false)]
        public ConnT01()
        {
        }

        #endregion

        #region 手配明細登録

        #region 明細取得
        /// --------------------------------------------------
        /// <summary>
        /// 手配明細データ取得
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <returns>データセット</returns>
        /// <create>S.Furugo 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetTehaiMeisai(CondT01 cond)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.GetTehaiMeisai(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 技連マスタ取得
        /// --------------------------------------------------
        /// <summary>
        /// 技連マスタ取得
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <returns>データセット</returns>
        /// <create>S.Furugo 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetGiren(CondT01 cond)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.GetGiren(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 名称マスタ取得
        /// --------------------------------------------------
        /// <summary>
        /// 名称マスタ取得
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <returns>データセット</returns>
        /// <create>D.Naito 2018/11/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetSelectItem(CondT01 cond)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.GetSelectItem(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region パーツ名マスタ取得
        /// --------------------------------------------------
        /// <summary>
        /// パーツ名マスタ取得
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <returns>データセット</returns>
        /// <create>D.Naito 2018/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetPartsName(CondT01 cond)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.GetPartsName(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 図番/型式画像ファイル管理データ取得(IN検索)

        /// --------------------------------------------------
        /// <summary>
        /// 図番/型式画像ファイル管理データ取得(IN検索)
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <returns>データセット</returns>
        /// <create>H.Tsuji 2019/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetManageZumenKeishikiInSearch(CondT01 cond)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.GetManageZumenKeishikiInSearch(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 図番/型式の入力補助関連データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 図番/型式の入力補助関連データ取得
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <returns>データセット</returns>
        /// <create>H.Tsuji 2019/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetZumenKeishikiInputAssistance(CondT01 cond)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.GetZumenKeishikiInputAssistance(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 手配明細データ登録
        /// --------------------------------------------------
        /// <summary>
        /// 手配明細データ登録
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <param name="ds">データセット</param>
        /// <param name="errMsgID">メッセージ</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>S.Furugo 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InsTehaiMeisai(CondT01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.InsTehaiMeisai(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 手配明細データ更新

        /// --------------------------------------------------
        /// <summary>
        /// 手配明細データ更新
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <param name="ds">データセット</param>
        /// <param name="errMsgID">メッセージ</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>S.Furugo 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdTehaiMeisai(CondT01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.UpdTehaiMeisai(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 手配明細データ削除
        /// --------------------------------------------------
        /// <summary>
        /// 手配明細データ削除
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <param name="ds">データセット</param>
        /// <param name="errMsgID">メッセージ</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>S.Furugo 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelTehaiMeisai(CondT01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.DelTehaiMeisai(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        //#region Assy単価計算データ取得
        ///// --------------------------------------------------
        ///// <summary>
        ///// Assy単価計算計算データ取得
        ///// </summary>
        ///// <param name="cond">T01用コンディション</param>
        ///// <returns>データセット</returns>
        ///// <create>Y.shioshi 2022/05/12</create>
        ///// <update></update>
        ///// --------------------------------------------------
        //public DataSet GetAssyUnitPrice(CondT01 cond)
        //{
        //    try
        //    {
        //        using (WsT01 ws = this.GetWsT01())
        //        {
        //            return ws.GetAssyUnitPrice(cond);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message, ex);
        //    }
        //}

        //#endregion

        #region SKS手配明細WORKバージョン取得
        /// --------------------------------------------------
        /// <summary>
        /// SKS手配明細WORKバージョン取得
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <returns>データセット</returns>
        /// <create>J.Chen 2022/10/31</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetTehaiSKSWorkVersion(CondT01 cond)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.GetTehaiSKSWorkVersion(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion


        #endregion

        #region 手配明細履歴

        #region 手配明細履歴の取得

        /// --------------------------------------------------
        /// <summary>
        /// 手配明細履歴の取得
        /// </summary>
        /// <param name="cond">T01用条件</param>
        /// <returns></returns>
        /// <create>J.Chen 2024/11/06</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetTehaiMeisaiRireki(CondT01 cond)
        {
            try
            {
                using (var ws = this.GetWsT01())
                {
                    return ws.GetTehaiMeisaiRireki(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region SKS手配連携

        #region 表示データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 表示データ取得
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <returns>データセット</returns>
        /// <create>H.Tajimi 2019/01/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetSKSTehaiRenkeiDispData(CondT01 cond)
        {
            try 
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.GetSKSTehaiRenkeiDispData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        
        #endregion

        #region SKS手配明細取得

        /// --------------------------------------------------
        /// <summary>
        /// SKS手配明細取得
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <param name="ds">表示データ</param>
        /// <returns>SKS手配明細</returns>
        /// <create>H.Tajimi 2019/01/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataTable GetSKSTehaiRenkeiTehaiSKS(CondT01 cond, DataSet ds)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.GetSKSTehaiRenkeiTehaiSKS(cond, ds);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region SKS手配明細WORK取得

        /// --------------------------------------------------
        /// <summary>
        /// SKS手配明細WORK取得
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>SKS手配明細WORK</returns>
        /// <create>H.Tajimi 2019/01/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetSKSTehaiRenkeiTehaiSKSWork(CondT01 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.GetSKSTehaiRenkeiTehaiSKSWork(cond, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region SKS手配連携更新

        /// --------------------------------------------------
        /// <summary>
        /// SKS手配連携更新
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <param name="ds">更新データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/01/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdSKSTehaiRenkei(CondT01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.UpdSKSTehaiRenkei(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 手配明細照会

        #region 明細取得
        /// --------------------------------------------------
        /// <summary>
        /// 手配明細データ取得
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <returns>データセット</returns>
        /// <create>S.Furugo 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetTehaiShokai(CondT01 cond)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.GetTehaiShokai(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 手配情報照会

        #region 初期表示(手配情報照会)

        /// --------------------------------------------------
        /// <summary>
        /// 初期表示(手配情報照会)
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetInitTehaiJohoShokai(CondT01 cond)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.GetInitTehaiJohoShokai(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 表示データ取得(手配情報照会)

        /// --------------------------------------------------
        /// <summary>
        /// 表示データ取得(手配情報照会)
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <returns>データセット</returns>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetTehaiJohoShokai(CondT01 cond)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.GetTehaiJohoShokai(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 手配履歴更新
        /// --------------------------------------------------
        /// <summary>
        /// 手配履歴更新
        /// </summary>
        /// <create>J.Chen 2024/10/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdTehaiJohoRireki(CondT01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.UpdTehaiJohoRireki(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 手配見積

        #region 見積明細取得
        /// --------------------------------------------------
        /// <summary>
        /// 見積明細取得
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <returns></returns>
        /// <create>S.Furugo 2018/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetTehaiMitsumori(CondT01 cond)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.GetTehaiMitsumori(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 見積情報取得
        /// --------------------------------------------------
        /// <summary>
        /// 見積情報取得
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <returns></returns>
        /// <create>S.Furugo 2018/12/3</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetEstimateInformation(CondT01 cond)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.GetEstimateInformation(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 手配見積データ登録
        /// --------------------------------------------------
        /// <summary>
        /// 手配見積データ登録
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <param name="ds">データセット</param>
        /// <param name="EstimateNo">採番した見積番号</param>
        /// <param name="errMsgID">メッセージ</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>S.Furugo 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InsTehaiMitsumori(CondT01 cond, DataSet ds, out string EstimateNo, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.InsTehaiMitsumori(cond, ds, out EstimateNo, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion
        
        #region 手配見積データ更新

        /// --------------------------------------------------
        /// <summary>
        /// 手配見積データ更新
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <param name="ds">データセット</param>
        /// <param name="errMsgID">メッセージ</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>S.Furugo 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdTehaiMitsumori(CondT01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.UpdTehaiMitsumori(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 手配見積データ削除

        /// --------------------------------------------------
        /// <summary>
        /// 手配見積データ削除
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <param name="ds">データセット</param>
        /// <param name="errMsgID">メッセージ</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>S.Furugo 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelTehaiMitsumori(CondT01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.DelTehaiMitsumori(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 受注状態更新
        /// --------------------------------------------------
        /// <summary>
        /// 受注状態更新
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <param name="errMsgID">メッセージ</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>S.Furugo 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdTehaiMitsumoriOrder(CondT01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.UpdTehaiMitsumoriOrder(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region 見積MAIL配信
        /// --------------------------------------------------
        /// <summary>
        /// 見積MAIL配信
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <param name="errMsgID">メッセージ</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>J.Chen 2024/01/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdTehaiEstimateMail(CondT01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.UpdTehaiEstimateMail(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region MAIL配信用累計金額取得
        /// --------------------------------------------------
        /// <summary>
        /// MAIL配信用累計金額取得
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>J.Chen 2024/08/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetTotalAmountForMail(CondT01 cond)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.GetTotalAmountForMail(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #endregion

        #region 手配見積明細

        #region 見積明細取得
        /// --------------------------------------------------
        /// <summary>
        /// 見積明細情報取得
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <returns>データ</returns>
        /// <create>S.Furugo 2018/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetTehaiMitsumoriMeisai(CondT01 cond)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.GetTehaiMitsumoriMeisai(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 手配見積明細バージョン更新

        /// --------------------------------------------------
        /// <summary>
        /// 手配見積明細バージョン更新
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>S.Furugo 2018/11/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdTehaiMitsumoriMeisaiVersionData(CondT01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.UpdTehaiMitsumoriMeisaiVersionData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #endregion

        #region VALUE照会

        #region VALUE明細取得
        /// --------------------------------------------------
        /// <summary>
        /// VALUE明細情報取得
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <returns>データ</returns>
        /// <create>J.Chen 2024/02/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetTehaiMitsumoriValue(CondT01 cond)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.GetTehaiMitsumoriValue(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 手配入荷検品

        #region 手配入荷検品データ取得
        /// --------------------------------------------------
        /// <summary>
        /// 手配入荷検品取得
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        /// <create>S.Furugo 2018/11/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetTehaiKenpin(CondT01 cond)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.GetTehaiKenpin(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region CSVより手配入荷検品データ取得
        /// --------------------------------------------------
        /// <summary>
        /// CSVより手配入荷検品データ取得
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <create>T.SASAYAMA 2023/07/04</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetTehaiKenpinCsv(CondT01 cond, string[] value)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.GetTehaiKenpinCsv(cond, value);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 手配入荷検品データ更新
        /// --------------------------------------------------
        /// <summary>
        /// 手配入荷検品データ更新
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>S.Furugo 2018/11/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdTehaiKenpin(CondT01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.UpdTehaiKenpin(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 物件名一覧取得
        /// --------------------------------------------------
        /// <summary>
        /// 物件名一覧取得
        /// </summary>
        /// <returns>取得データ</returns>
        /// <create>S.Furugo 2018/11/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetBukkenName()
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.GetBukkenNameList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 初期データ取得(入荷検品)
        /// --------------------------------------------------
        /// <summary>
        /// 初期データ取得(入荷検品)
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <returns>取得データ</returns>
        /// <create>K.Tsutsumi 2020/05/31</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetInitTehaiKenpin(CondT01 cond)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.GetInitTehaiKenpin(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region SKS最終連携日時取得
        /// --------------------------------------------------
        /// <summary>
        /// SKS最終連携日時取得
        /// </summary>
        /// <returns>取得データ</returns>
        /// <create>S.Furugo 2018/11/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetSksLastLink()
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.GetSksLastLink();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 組立実績登録

        #region 組立実績登録データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 組立実績登録データ取得
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetKumitateJisseki(CondT01 cond)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.GetKumitateJisseki(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 組立実績登録データ更新

        /// --------------------------------------------------
        /// <summary>
        /// 組立実績登録データ更新
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <param name="ds">更新用データのDataSet</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/11/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdKumitateJisseki(CondT01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
                {
                    return ws.UpdKumitateJisseki(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region Ship照会

        /// --------------------------------------------------
        /// <summary>
        /// 出荷明細データ取得
        /// </summary>
        /// <param name="cond">T01用コンディション</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>表示データ</returns>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetShukkaMeisai(CondT01 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsT01 ws = this.GetWsT01())
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
    }
}
