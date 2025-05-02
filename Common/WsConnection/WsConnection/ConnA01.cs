using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using WsConnection.WebRefA01;

namespace WsConnection
{
    /// --------------------------------------------------
    /// <summary>
    /// AR品管理コネクションクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/06/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [DSWUtil.AOP.AspectClass(typeof(AspectConnectionRetry), true)]
    public class ConnA01 : ConnBase
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
        public ConnA01()
        {
        }

        #endregion

        #region A0100010:AR情報登録

        #region 納入先マスタ存在チェック

        /// --------------------------------------------------
        /// <summary>
        /// 納入先マスタ存在チェック
        /// </summary>
        /// <param name="cond">A01用コンディション</param>
        /// <returns>true:存在する false:存在しない</returns>
        /// <create>M.Tsutsumi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool IsExistenceNonyusaki(CondA1 cond)
        {
            try
            {
                using (WsA01 ws = this.GetWsA01())
                {
                    return ws.IsExistenceNonyusaki(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region AR情報データリスト取得

        /// --------------------------------------------------
        /// <summary>
        /// AR情報データリスト 取得
        /// </summary>
        /// <param name="cond">A01用コンディション</param>
        /// <returns>データセット</returns>
        /// <create>M.Tsutsumi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetArDataList(CondA1 cond)
        {
            try
            {
                using (WsA01 ws = this.GetWsA01())
                {
                    return ws.GetArDataList(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 納入先マスタの存在確認後、Excelデータの件数を取得する

        /// --------------------------------------------------
        /// <summary>
        /// 納入先マスタの存在確認後、Excelデータの件数を取得する
        /// </summary>
        /// <param name="cond">A01用コンディション</param>
        /// <param name="errMsgID">エラーメッセージID</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2012/05/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool ExistNonyusakiAndExcle(CondA1 cond, out string errMsgID)
        {
            try
            {
                using (WsA01 ws = this.GetWsA01())
                {
                    return ws.ExistNonyusakiAndExcle(cond, out errMsgID);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 全体Excel用データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 全体Excel用データ取得
        /// </summary>
        /// <param name="cond">A01用コンディション</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2012/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataTable GetAllARListData(CondA1 cond)
        {
            try
            {
                using (WsA01 ws = this.GetWsA01())
                {
                    return ws.GetAllARListData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 納入先マスタの存在確認後、費用Excelデータの件数を取得する

        /// --------------------------------------------------
        /// <summary>
        /// 納入先マスタの存在確認後、費用Excelデータの件数を取得する
        /// </summary>
        /// <param name="cond">A01用コンディション</param>
        /// <param name="errMsgID">エラーメッセージID</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/10/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool ExistNonyusakiAndHiyouExcel(CondA1 cond, out string errMsgID)
        {
            try
            {
                using (WsA01 ws = this.GetWsA01())
                {
                    return ws.ExistNonyusakiAndHiyouExcel(cond, out errMsgID);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 全体費用Excel用データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 全体費用Excel用データ取得
        /// </summary>
        /// <param name="cond">A01用コンディション</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/10/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataTable GetAllARCostListData(CondA1 cond)
        {
            try
            {
                using (WsA01 ws = this.GetWsA01())
                {
                    return ws.GetAllARCostListData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region メール送信情報を取得

        /// --------------------------------------------------
        /// <summary>
        /// メール送信情報を取得
        /// </summary>
        /// <param name="cond">A01用コンディション</param>
        /// <param name="errMsgID">メッセージ</param>
        /// <param name="args">パラメータ</param>
        /// <returns></returns>
        /// <create>R.Katsuo 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetSendMailInfo(CondA1 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsA01 ws = this.GetWsA01())
                {
                    return ws.GetSendMailInfo(cond, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region A0100020:AR情報明細登録

        #region AR情報データ取得

        /// --------------------------------------------------
        /// <summary>
        /// AR情報データ取得
        /// </summary>
        /// <param name="cond">A01用コンディション</param>
        /// <returns>データセット</returns>
        /// <create>M.Tsutsumi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetARData(CondA1 cond)
        {
            try
            {
                using (WsA01 ws = this.GetWsA01())
                {
                    return ws.GetARData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region AR情報データ取得＆インターロック

        /// --------------------------------------------------
        /// <summary>
        /// AR情報データ取得＆インターロック
        /// </summary>
        /// <param name="cond">A01用コンディション</param>
        /// <param name="ds">データセット</param>
        /// <param name="errMsgID">メッセージ</param>
        /// <returns>true:インターロック成功 false:インターロック失敗</returns>
        /// <create>M.Tsutsumi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool GetARandInterLock(CondA1 cond, out DataSet ds, out string errMsgID)
        {
            try
            {
                bool ret = false;
                
                using (WsA01 ws = this.GetWsA01())
                {
                    ret = ws.GetARandInterLock(cond, out ds, out errMsgID);
                }

                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 納入先マスタ取得

        /// --------------------------------------------------
        /// <summary>
        /// 納入先マスタ取得
        /// </summary>
        /// <param name="cond">A01用コンディション</param>
        /// <returns>納入先マスタ</returns>
        /// <create>M.Tsutsumi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetNonyusaki(CondA1 cond)
        {
            try
            {
                using (WsA01 ws = this.GetWsA01())
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

        #region AR情報データ登録

        /// --------------------------------------------------
        /// <summary>
        /// AR情報データ登録
        /// </summary>
        /// <param name="cond">A01用コンディション</param>
        /// <param name="ds">データセット</param>
        /// <param name="nonyusakiCd">採番/取得した納入先コード</param>
        /// <param name="arNo">採番したARNo</param>
        /// <param name="errMsgID">メッセージ</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>M.Tsutsumi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InsAR(CondA1 cond, DataSet ds, out string nonyusakiCd, out string arNo, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsA01 ws = this.GetWsA01())
                {
                    return ws.InsAR(cond, ds, out nonyusakiCd, out arNo, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region AR情報データ更新

        /// --------------------------------------------------
        /// <summary>
        /// AR情報データ更新
        /// </summary>
        /// <param name="cond">A01用コンディション</param>
        /// <param name="ds">データセット</param>
        /// <param name="errMsgID">メッセージ</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>M.Tsutsumi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdAR(CondA1 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsA01 ws = this.GetWsA01())
                {
                    return ws.UpdAR(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region AR情報データ インターロック解除

        /// --------------------------------------------------
        /// <summary>
        /// AR情報データ インターロック解除
        /// </summary>
        /// <param name="cond">A01用コンディション</param>
        /// <param name="dt">ARデータ</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>M.Tsutsumi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool ARInterUnLock(CondA1 cond, DataTable dt)
        {
            try
            {
                using (WsA01 ws = this.GetWsA01())
                {
                    return ws.UpdARInterUnLock(cond, dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 出荷明細データのチェック
        /// --------------------------------------------------
        /// <summary>
        /// 出荷明細データのチェック
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="ds"></param>
        /// <param name="errMsgID"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <create>H.Tsunamura 2010/10/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool ChackMeisaiData(CondA1 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsA01 ws = this.GetWsA01())
                {
                    return ws.GetCheckMeisaiData(cond, ds, out errMsgID, out args);
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
        /// <param name="cond">A01用コンディション</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2012/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataTable GetSelectItem(CondA1 cond)
        {
            try
            {
                using (WsA01 ws = this.GetWsA01())
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

        #region ユーザー情報を取得

        /// --------------------------------------------------
        /// <summary>
        /// ユーザー情報を取得
        /// </summary>
        /// <param name="cond">A01用コンディション</param>
        /// <returns>DataTable</returns>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataTable GetUser(CondA1 cond)
        {
            try
            {
                using (WsA01 ws = this.GetWsA01())
                {
                    return ws.GetUser(cond);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region リスト区分取得

        /// --------------------------------------------------
        /// <summary>
        /// リスト区分取得
        /// </summary>
        /// <param name="cond">A01用コンディション</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataTable GetCommonListFlag(CondA1 cond)
        {
            try
            {
                using (WsA01 ws = this.GetWsA01())
                {
                    return ws.GetCommonListFlag(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 登録号機数の取得

        /// --------------------------------------------------
        /// <summary>
        /// 登録号機数の取得
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        /// <create>Y.Nakasato 2019/07/31</create>
        /// <update></update>
        /// --------------------------------------------------
        public int GetGokiNum(CondA1 cond)
        {
            try
            {
                using (var ws = this.GetWsA01())
                {
                    return ws.GetGokiNum(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 通知設定の取得

        /// --------------------------------------------------
        /// <summary>
        /// 通知設定の取得
        /// </summary>
        /// <param name="cond">A01用条件</param>
        /// <returns></returns>
        /// <create>Y.Nakasato 2019/08/02 AR進捗通知対応</create>
        /// <update>J.Chen 2024/08/22 メール通知フラグ取得</update>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetArMailInfo(CondA1 cond, out string errMsgID, out string[] args, out bool _isNotify)
        {
            try
            {
                using (var ws = this.GetWsA01())
                {
                    return ws.GetArMailInfo(cond, out errMsgID, out args, out _isNotify);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region MAIL_ID取得

        /// --------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="errMsgID"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <create>J.Chen 2024/08/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public string GetMailIDWithoutUpdate(CondA1 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsA01 ws = this.GetWsA01())
                {
                    return ws.GetMailIDWithoutUpdate(cond, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region AR_No取得

        /// --------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="errMsgID"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <create>J.Chen 2024/08/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public string GetARNoWithoutUpdate(CondA1 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsA01 ws = this.GetWsA01())
                {
                    return ws.GetARNoWithoutUpdate(cond, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region A0100040:員数表取込

        #region 号機チェック
        /// --------------------------------------------------
        /// <summary>
        /// 号機チェック
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="dt"></param>
        /// <param name="errMsgID"></param>
        /// <param name="dtMessage"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <create>T.Nakata 2019/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool CheckGoki(CondA1 cond, DataTable dt, out string errMsgID, out DataTable dtMessage, out string[] args)
        {
            try
            {
                using (WsA01 ws = this.GetWsA01())
                {
                    return ws.CheckGoki(cond, dt, out errMsgID, out dtMessage, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 員数表登録
        /// --------------------------------------------------
        /// <summary>
        /// 員数表登録
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="dt"></param>
        /// <param name="errMsgID"></param>
        /// <param name="dtMessage"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <create>T.Nakata 2019/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InsInzuhyo(CondA1 cond, DataTable dt, out string errMsgID, out DataTable dtMessage, out string[] args)
        {
            try
            {
                using (WsA01 ws = this.GetWsA01())
                {
                    return ws.InsInzuhyo(cond, dt, out errMsgID, out dtMessage, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region A0100050:ＡＲ進捗管理

        #region 進捗情報検索
        /// --------------------------------------------------
        /// <summary>
        /// 進捗情報検索
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataTable GetArShinchokuList(CondA1 cond)
        {
            try
            {
                using (WsA01 ws = this.GetWsA01())
                {
                    return ws.GetArShinchokuList(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region AR進捗のメール送信情報を取得
        /// --------------------------------------------------
        /// <summary>
        /// AR進捗のメール送信情報を取得
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="errMsgID"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetArShinchokuMailInfo(CondA1 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsA01 ws = this.GetWsA01())
                {
                    return ws.GetArShinchokuMailInfo(cond, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region AR進捗のメール送信
        /// --------------------------------------------------
        /// <summary>
        /// AR進捗のメール送信
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="errMsgID"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool SendArShinchokuMail(CondA1 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsA01 ws = this.GetWsA01())
                {
                    return ws.SendArShinchokuMail(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #endregion

        #region A0100051:ＡＲ進捗管理日付登録

        #region AR情報のインターンロックを解除

        /// --------------------------------------------------
        /// <summary>
        /// AR情報のインターンロックを解除
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="dt"></param>
        /// <param name="errMsgID"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <create>T.Nakata 2019/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdARUnLockShinchoku(CondA1 cond, DataTable dt, out string errMsgID, out string[] args)
        {
            try
            {
                using (var ws = this.GetWsA01())
                {
                    return ws.UpdARUnLockShinchoku(cond, dt, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region AR進捗情報取得

        /// --------------------------------------------------
        /// <summary>
        /// AR進捗情報取得
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="dt"></param>
        /// <param name="errMsgID"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <create>T.Nakata 2019/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetARInterLockAndShinchokuInfo(CondA1 cond, DataTable dt, out string errMsgID, out string[] args)
        {
            try
            {
                using (var ws = this.GetWsA01())
                {
                    return ws.GetARInterLockAndShinchokuInfo(cond, dt, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region AR進捗情報登録

        /// --------------------------------------------------
        /// <summary>
        /// AR進捗情報登録
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="dt"></param>
        /// <param name="errMsgID"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <create>T.Nakata 2019/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdARShinchokuInfo(CondA1 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (var ws = this.GetWsA01())
                {
                    return ws.UpdARShinchokuInfo(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region A0100052:機種ダイアログ

        #region 機種の取得

        /// --------------------------------------------------
        /// <summary>
        /// 機種の取得
        /// </summary>
        /// <param name="cond">A01用条件</param>
        /// <returns></returns>
        /// <create>Y.Nakasato 2019/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataTable GetKishu(CondA1 cond)
        {
            try
            {
                using (var ws = this.GetWsA01())
                {
                    return ws.GetKishu(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region A0100053:号機ダイアログ

        #region 号機の取得

        /// --------------------------------------------------
        /// <summary>
        /// 号機の取得
        /// </summary>
        /// <param name="cond">A01用条件</param>
        /// <returns></returns>
        /// <create>Y.Nakasato 2019/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataTable GetGoki(CondA1 cond)
        {
            try
            {
                using (var ws = this.GetWsA01())
                {
                    return ws.GetGoki(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region A0100060:変更履歴

        #region 変更履歴の取得

        /// --------------------------------------------------
        /// <summary>
        /// 変更履歴の取得
        /// </summary>
        /// <param name="cond">A01用条件</param>
        /// <returns></returns>
        /// <create>Y.Nakasato 2019/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataTable GetRireki(CondA1 cond)
        {
            try
            {
                using (var ws = this.GetWsA01())
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

        #endregion
    }
}
