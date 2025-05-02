using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using WsConnection.WebRefM01;

namespace WsConnection
{
    /// --------------------------------------------------
    /// <summary>
    /// マスタ&保守コネクションクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/06/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [DSWUtil.AOP.AspectClass(typeof(AspectConnectionRetry), true)]
    public class ConnM01 : ConnBase
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
        public ConnM01()
        {
        }

        #endregion

        #region ユーザーマスタ

        #region ユーザー取得(完全一致・ユーザーID必須)

        /// --------------------------------------------------
        /// <summary>
        /// ユーザー取得(完全一致・ユーザーID必須)
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <returns>ユーザーマスタ</returns>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetUser(CondM01 cond)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
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

        #region ユーザー取得(あいまい検索)

        /// --------------------------------------------------
        /// <summary>
        /// ユーザー取得(あいまい検索)
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <returns>ユーザーマスタ</returns>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetUserLikeSearch(CondM01 cond)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.GetUserLikeSearch(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ユーザーの追加

        /// --------------------------------------------------
        /// <summary>
        /// ユーザーの追加
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InsUserData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.InsUserData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ユーザーの更新

        /// --------------------------------------------------
        /// <summary>
        /// ユーザーの更新
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdUserData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.UpdUserData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ユーザーの削除

        /// --------------------------------------------------
        /// <summary>
        /// ユーザーの削除
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="isMultiError">複数エラーの場合はtrue</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update>H.Tsuji 2019/06/24 メール送信対象者の削除チェック</update>
        /// --------------------------------------------------
        public bool DelUserData(CondM01 cond, ref DataSet ds, out bool isMultiError)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.DelUserData(cond, ref ds, out isMultiError);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 納入先保守

        #region 納入先取得(あいまい検索)

        /// --------------------------------------------------
        /// <summary>
        /// 納入先取得(あいまい検索)
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <returns>納入先マスタ</returns>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetNonyusakiLikeSearch(CondM01 cond)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.GetNonyusakiLikeSearch(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 未完了AR情報件数取得

        /// --------------------------------------------------
        /// <summary>
        /// 未完了AR情報件数取得
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <returns>未完了AR情報件数</returns>
        /// <create>Y.Higuchi 2010/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetMikanryoAR(CondM01 cond)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.GetMikanryoAR(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #region 納入先の更新

        /// --------------------------------------------------
        /// <summary>
        /// 納入先の更新
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdNonyusakiData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.UpdNonyusakiData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 納入先の削除

        /// --------------------------------------------------
        /// <summary>
        /// 納入先の削除
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>M.Tsutsumi 2011/02/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelNonyusakiData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.DelNonyusakiData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 納入先取得(存在確認用)

        /// --------------------------------------------------
        /// <summary>
        /// 納入先取得(存在確認用)
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2012/04/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetNonyusaki(CondM01 cond)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
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

        #region 納入先の更新

        /// --------------------------------------------------
        /// <summary>
        /// 納入先の登録
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.Sakiori 2012/04/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InsNonyusakiData(CondM01 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.InsNonyusakiData(cond, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 未出荷明細件数取得

        /// --------------------------------------------------
        /// <summary>
        /// 未出荷明細件数取得
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetMishukkaMeisai(CondM01 cond)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.GetMishukkaMeisai(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 梱包情報保守

        #region Box梱包データ取得

        /// --------------------------------------------------
        /// <summary>
        /// Box梱包データ取得
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="errMsgID">エラーメッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>DataSet</returns>
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetBoxData(CondM01 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
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

        #region パレット梱包データ取得

        /// --------------------------------------------------
        /// <summary>
        /// パレット梱包データ取得
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="errMsgID">エラーメッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>DataSet</returns>
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetPalletData(CondM01 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
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

        #region Box梱包データ更新

        /// --------------------------------------------------
        /// <summary>
        /// Box梱包データ更新
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="dtManage">ボックスリスト管理データ</param>
        /// <param name="dt">更新データ</param>
        /// <param name="dtKonpo">追加梱包データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:</returns>
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        // 2012/05/18 K.Tsutsumi Change 管理データのバージョンチェック
        //public bool UpdBoxData(CondM01 cond, DataTable dt, DataTable dtKonpo, out string errMsgID, out string[] args)
        public bool UpdBoxData(CondM01 cond, DataTable dtManage, DataTable dt, DataTable dtKonpo, out string errMsgID, out string[] args)
        // ↑
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    // 2012/05/18 K.Tsutsumi Change 管理データのバージョンチェック
                    //return ws.UpdBoxData(cond, dt, dtKonpo, out errMsgID, out args);
                    return ws.UpdBoxData(cond, dtManage, dt, dtKonpo, out errMsgID, out args);
                    // ↑
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region パレット梱包データ更新

        /// --------------------------------------------------
        /// <summary>
        /// パレット梱包データ更新
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="dtManage">パレットリスト管理データ</param>
        /// <param name="dt">更新データ</param>
        /// <param name="dtKonpo">追加梱包データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:</returns>
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        // 2012/05/18 K.Tsutsumi Change 管理データのバージョンチェック
        //public bool UpdPalletData(CondM01 cond, DataTable dt, DataTable dtKonpo, out string errMsgID, out string[] args)
        public bool UpdPalletData(CondM01 cond, DataTable dtManage, DataTable dt, DataTable dtKonpo, out string errMsgID, out string[] args)
        // ↑
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    // 2012/05/18 K.Tsutsumi Change 管理データのバージョンチェック
                    //return ws.UpdPalletData(cond, dt, dtKonpo, out errMsgID, out args);
                    return ws.UpdPalletData(cond, dtManage, dt, dtKonpo, out errMsgID, out args);
                    // ↑
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Box梱包データ取得(追加)

        /// --------------------------------------------------
        /// <summary>
        /// 追加するBox梱包データを取得
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="errMsgID">エラーメッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>DataSet</returns>
        /// <create>M.Tsutsumi 2011/03/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetBoxDataAdd(CondM01 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.GetBoxDataAdd(cond, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region パレット梱包データ取得(追加)

        /// --------------------------------------------------
        /// <summary>
        /// 追加するパレット梱包データ取得
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="errMsgID">エラーメッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>DataSet</returns>
        /// <create>M.Tsutsumi 2011/03/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetPalletDataAdd(CondM01 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.GetPalletDataAdd(cond, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 物件名保守

        #region 物件名取得(完全一致・物件管理No必須)

        /// --------------------------------------------------
        /// <summary>
        /// 物件名取得(完全一致・物件管理No必須)
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <returns>物件マスタ</returns>
        /// <create>K.Saeki 2012/03/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetBukken(CondM01 cond)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.GetBukken(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 物件名取得(あいまい検索)

        /// --------------------------------------------------
        /// <summary>
        /// 物件名取得(あいまい検索)
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <returns>物件マスタ</returns>
        /// <create>K.Saeki 2012/03/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetBukkenLikeSearch(CondM01 cond)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.GetBukkenLikeSearch(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 物件名の追加

        /// --------------------------------------------------
        /// <summary>
        /// 物件名の追加
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>K.Saeki 2012/03/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InsBukkenData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.InsBukkenData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 物件名の更新

        /// --------------------------------------------------
        /// <summary>
        /// 物件名の更新
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>K.Saeki 2012/03/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdBukkenData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.UpdBukkenData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 物件名の削除

        /// --------------------------------------------------
        /// <summary>
        /// 物件名の削除
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>K.Saeki 2012/03/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelBukkenData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.DelBukkenData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 物件メールマスタ(共通)の存在確認

        /// --------------------------------------------------
        /// <summary>
        /// 物件メールマスタ(共通)の存在確認
        /// </summary>
        /// <param name="cond">M01用条件</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2017/09/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool ExistsBukkenMail(CondM01 cond)
        {
            try
            {
                using (var ws = this.GetWsM01())
                {
                    return ws.ExistsBukkenMail(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion


        #region 進捗管理通知設定(Default)の存在確認

        /// --------------------------------------------------
        /// <summary>
        /// 進捗管理通知設定(Default)の存在確認
        /// </summary>
        /// <param name="cond">M01用条件</param>
        /// <returns></returns>
        /// <create>Y.Nakasato 2019/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool ExistsShinchokuKanriMail(CondM01 cond)
        {
            try
            {
                using (var ws = this.GetWsM01())
                {
                    return ws.ExistsShinchokuKanriMail(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 名称保守

        #region 名称取得

        /// --------------------------------------------------
        /// <summary>
        /// 名称取得
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <returns>名称</returns>
        /// <create>T.Sakiori 2012/04/06</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetSelectItem(CondM01 cond)
        {
            try
            {
                using (var ws = this.GetWsM01())
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

        #region 名称の追加

        /// --------------------------------------------------
        /// <summary>
        /// 名称の追加
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.Sakiori 2012/04/06</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InsSelectItemData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (var ws = this.GetWsM01())
                {
                    return ws.InsSelectItemData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 名称の更新

        /// --------------------------------------------------
        /// <summary>
        /// 名称の更新
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.Sakiori 2012/04/06</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdSelectItemData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (var ws = this.GetWsM01())
                {
                    return ws.UpdSelectItemData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 名称の削除

        /// --------------------------------------------------
        /// <summary>
        /// 名称の削除
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.Sakiori 2012/04/06</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelSelectItemData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (var ws = this.GetWsM01())
                {
                    return ws.DelSelectItemData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 基本通知設定

        #region 表示データの取得

        /// --------------------------------------------------
        /// <summary>
        /// 表示データの取得
        /// </summary>
        /// <param name="cond">M01用条件</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2017/09/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetBasicNotify(CondM01 cond)
        {
            try
            {
                using (var ws = this.GetWsM01())
                {
                    return ws.GetBasicNotify(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 基本通知設定の保存

        /// --------------------------------------------------
        /// <summary>
        /// 基本通知設定の保存
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool SaveBasicNotify(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (var ws = this.GetWsM01())
                {
                    return ws.SaveBasicNotify(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 共通通知設定

        #region 共通通知設定の取得

        /// --------------------------------------------------
        /// <summary>
        /// 共通通知設定の取得
        /// </summary>
        /// <param name="cond">M01用条件</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataTable GetCommonNotify(CondM01 cond)
        {
            try
            {
                using (var ws = this.GetWsM01())
                {
                    return ws.GetCommonNotify(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 共通通知設定の保存

        /// --------------------------------------------------
        /// <summary>
        /// 共通通知設定の保存
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool SaveCommonNotify(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (var ws = this.GetWsM01())
                {
                    return ws.SaveCommonNotify(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region メールアドレス変更権限を保持しているか

        /// --------------------------------------------------
        /// <summary>
        /// メールアドレス変更権限を保持しているか
        /// </summary>
        /// <param name="cond">ログインユーザー</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2017/09/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool ExistsMailChangeRole(CondM01 cond)
        {
            try
            {
                using (var ws = this.GetWsM01())
                {
                    return ws.ExistsMailChangeRole(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region AR List単位通知設定

        #region リスト区分取得

        /// --------------------------------------------------
        /// <summary>
        /// リスト区分取得
        /// </summary>
        /// <param name="cond">M01用条件</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2017/09/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataTable GetListFlag(CondM01 cond)
        {
            try
            {
                using (var ws = this.GetWsM01())
                {
                    return ws.GetListFlag(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 表示データの取得

        /// --------------------------------------------------
        /// <summary>
        /// 表示データの取得
        /// </summary>
        /// <param name="cond">M01用条件</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2017/09/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataTable GetARListNotify(CondM01 cond)
        {
            try
            {
                using (var ws = this.GetWsM01())
                {
                    return ws.GetARListNotify(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 削除処理
        /// --------------------------------------------------
        /// <summary>
        /// 削除処理
        /// </summary>
        /// <param name="cond">M01用条件</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2017/09/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelARListNotify(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (var ws = this.GetWsM01())
                {
                    return ws.DelARListNotify(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region メール送信情報メンテナンス

        #region 表示データの取得

        /// --------------------------------------------------
        /// <summary>
        /// 表示データの取得
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        /// <create>R.Katsuo 2017/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetMail(CondM01 cond)
        {
            try
            {
                using (var ws = this.GetWsM01())
                {
                    return ws.GetMail(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region メール再送信

        /// --------------------------------------------------
        /// <summary>
        /// メール再送信
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="dt"></param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns></returns>
        /// <create>R.Katsuo 2017/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InsMailRetry(CondM01 cond, DataTable dt, out string errMsgID, out string[] args)
        {
            try
            {
                using (var ws = this.GetWsM01())
                {
                    return ws.InsMailRetry(cond, dt, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region メールの強制終了

        /// --------------------------------------------------
        /// <summary>
        /// メールの強制終了
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="dt"></param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns></returns>
        /// <create>R.Katsuo 2017/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdMailAbort(CondM01 cond, DataTable dt, out string errMsgID, out string[] args)
        {
            try
            {
                using (var ws = this.GetWsM01())
                {
                    return ws.UpdMailAbort(cond, dt, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region パーツ名翻訳マスタ

        #region パーツ名取得

        /// --------------------------------------------------
        /// <summary>
        /// パーツ名取得
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <returns></returns>
        /// <create>S.Furugo 2018/10/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetPartName(CondM01 cond)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.GetPartName(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region パーツ名取得

        /// --------------------------------------------------
        /// <summary>
        /// パーツ名取得(あいまい検索)
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <returns></returns>
        /// <create>S.Furugo 2018/10/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetPartNameLike(CondM01 cond)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.GetPartNameLike(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region パーツ名取得(Excel)

        /// --------------------------------------------------
        /// <summary>
        /// パーツ名取得(Excel)
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/07/03</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetPartsNameExcelData(CondM01 cond)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.GetPartsNameExcelData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region パーツ名の追加

        /// --------------------------------------------------
        /// <summary>
        /// パーツ名の追加
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>S.Furugo 2018/10/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InsPartData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (var ws = this.GetWsM01())
                {
                    return ws.InsPartData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region パーツ名の更新

        /// --------------------------------------------------
        /// <summary>
        /// パーツ名の更新
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>S.Furugo 2018/10/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdPartData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (var ws = this.GetWsM01())
                {
                    return ws.UpdPartData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region パーツ名の削除

        /// --------------------------------------------------
        /// <summary>
        /// パーツ名の削除
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>S.Furugo 2018/10/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelPartData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (var ws = this.GetWsM01())
                {
                    return ws.DelPartData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region パーツ名翻訳マスタ(取込)

        #region パーツ名の追加

        /// --------------------------------------------------
        /// <summary>
        /// パーツ名の追加
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="dtMessage">マルチ用エラーメッセージ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tajimi 2019/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InsImportedPartsData(CondM01 cond, DataSet ds, ref DataTable dtMessage, out string errMsgID, out string[] args)
        {
            try
            {
                using (var ws = this.GetWsM01())
                {
                    return ws.InsImportedPartsData(cond, ds, ref dtMessage, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 荷受保守

        #region 荷受マスタ(あいまい検索)

        /// --------------------------------------------------
        /// <summary>
        /// 荷受マスタ(あいまい検索)
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <returns>荷受マスタ</returns>
        /// <create>H.Tsuji 2018/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetConsignLikeSearch(CondM01 cond)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.GetConsignLikeSearch(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 荷受マスタ(完全一致・荷受CD必須)

        /// --------------------------------------------------
        /// <summary>
        /// 荷受マスタ(完全一致・荷受CD必須)
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <returns>荷受マスタ</returns>
        /// <create>H.Tsuji 2018/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetConsign(CondM01 cond)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
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

        #region 荷受の追加

        /// --------------------------------------------------
        /// <summary>
        /// 荷受の追加
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tsuji 2018/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InsConsignData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.InsConsignData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 荷受の更新

        /// --------------------------------------------------
        /// <summary>
        /// 荷受の更新
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tsuji 2018/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdConsignData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.UpdConsignData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 荷受の削除

        /// --------------------------------------------------
        /// <summary>
        /// 荷受の削除
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tsuji 2018/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelConsignData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.DelConsignData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 計画取込Mail通知設定

        #region 計画取込Mail通知設定の取得

        /// --------------------------------------------------
        /// <summary>
        /// 計画取込Mail通知設定の取得
        /// </summary>
        /// <param name="cond">M01用条件</param>
        /// <returns></returns>
        /// <create>Y.Gwon 2023/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataTable GetKeikakuTorikomiNotify(CondM01 cond)
        {
            try
            {
                using (var ws = this.GetWsM01())
                {
                    return ws.GetKeikakuTorikomiNotify(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 計画取込Mail通知設定の保存

        /// --------------------------------------------------
        /// <summary>
        /// 計画取込Mail通知設定の保存
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns></returns>
        /// <create>Y.Gwon 2023/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool SaveKeikakuTorikomiNotify(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (var ws = this.GetWsM01())
                {
                    return ws.SaveKeikakuTorikomiNotify(cond, ds, out errMsgID, out args);
                    
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 配送先保守

        #region 配送先マスタ(あいまい検索)

        /// --------------------------------------------------
        /// <summary>
        /// 配送先マスタ(あいまい検索)
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <returns>配送先マスタ</returns>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetDeliverLikeSearch(CondM01 cond)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.GetDeliverLikeSearch(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 配送先マスタ(完全一致・配送先CD必須)

        /// --------------------------------------------------
        /// <summary>
        /// 配送先マスタ(完全一致・配送先CD必須)
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <returns>配送先マスタ</returns>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetDeliver(CondM01 cond)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
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

        #region 配送先の追加

        /// --------------------------------------------------
        /// <summary>
        /// 配送先の追加
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InsDeliverData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.InsDeliverData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 配送先の更新

        /// --------------------------------------------------
        /// <summary>
        /// 配送先の更新
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdDeliverData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.UpdDeliverData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 配送先の削除

        /// --------------------------------------------------
        /// <summary>
        /// 配送先の削除
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelDeliverData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.DelDeliverData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 運送会社保守

        #region 運送会社取得(運送会社CD/国内外フラグ/運送会社名)

        /// --------------------------------------------------
        /// <summary>
        /// 運送会社取得(運送会社CD/国内外フラグ/運送会社名)
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <returns>運送会社マスタ</returns>
        /// <create>T.Nakata 2018/11/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetUnsokaisya(CondM01 cond)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.GetUnsokaisya(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 運送会社取得(あいまい検索:国内外フラグ/運送会社名)

        /// --------------------------------------------------
        /// <summary>
        /// 運送会社取得(あいまい検索:国内外フラグ/運送会社名)
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <returns>運送会社マスタ</returns>
        /// <create>T.Nakata 2018/11/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetUnsokaisyaLikeSearch(CondM01 cond)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.GetUnsokaisyaLikeSearch(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 運送会社の追加

        /// --------------------------------------------------
        /// <summary>
        /// 運送会社の追加
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.Nakata 2018/11/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InsUnsokaisyaData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.InsUnsoKaisyaData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 運送会社の更新

        /// --------------------------------------------------
        /// <summary>
        /// 運送会社の更新
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.nakata 2018/11/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdUnsokaisyaData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.UpdUnsoKaisyaData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 運送会社の削除

        /// --------------------------------------------------
        /// <summary>
        /// 運送会社の削除
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.Nakata 2018/11/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelUnsokaisyaData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.DelUnsoKaisyaData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 技連保守

        #region プロジェクトマスタ取得

        /// --------------------------------------------------
        /// <summary>
        /// プロジェクトマスタ取得
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <returns>プロジェクトマスタ</returns>
        /// <create>H.Tsuji 2018/11/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetProject(CondM01 cond)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.GetProject(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 名称マスタ取得(機種一覧用)

        /// --------------------------------------------------
        /// <summary>
        /// 名称マスタ取得(機種一覧用)
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <returns>名称マスタ</returns>
        /// <create>H.Tsuji 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetSelectItemForKishu(CondM01 cond)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.GetSelectItemForKishu(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 技連マスタ(完全一致・期、ECSNo.必須)

        /// --------------------------------------------------
        /// <summary>
        /// 技連マスタ(完全一致・期、ECSNo.必須)
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <returns>技連マスタ</returns>
        /// <create>H.Tsuji 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetEcs(CondM01 cond)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.GetEcs(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 技連マスタ(あいまい検索)

        /// --------------------------------------------------
        /// <summary>
        /// 技連マスタ(あいまい検索)
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <returns>技連マスタ</returns>
        /// <create>H.Tsuji 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetEcsLikeSearch(CondM01 cond)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.GetEcsLikeSearch(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 技連の追加

        /// --------------------------------------------------
        /// <summary>
        /// 技連の追加
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tsuji 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InsEcsData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.InsEcsData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 技連の更新
        /// --------------------------------------------------
        /// <summary>
        /// 技連の更新
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tsuji 2018/11/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdEcsData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.UpdEcsData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region 技連の削除
        /// --------------------------------------------------
        /// <summary>
        /// 技連の削除
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tsuji 2018/11/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelEcsData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.DelEcsData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #endregion

        #region 物件名保守一括

        #region 物件名保守一括の物件名取得(あいまい検索)

        /// --------------------------------------------------
        /// <summary>
        /// 物件名保守一括の物件名取得(あいまい検索)
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <returns>物件マスタ</returns>
        /// <create>H.Tsuji 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetBukkenIkkatsuLikeSearch(CondM01 cond)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.GetBukkenIkkatsuLikeSearch(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 物件名保守一括の物件名取得

        /// --------------------------------------------------
        /// <summary>
        /// 物件名保守一括の物件名取得
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <returns>物件マスタ</returns>
        /// <create>H.Tsuji 2018/12/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetBukkenIkkatsuSearch(CondM01 cond)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.GetBukkenIkkatsuSearch(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 物件名の追加

        /// --------------------------------------------------
        /// <summary>
        /// 物件名の追加
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tsuji 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InsBukkenIkkatsuData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.InsBukkenIkkatsuData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 物件名の更新

        /// --------------------------------------------------
        /// <summary>
        /// 物件名の更新
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tsuji 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdBukkenIkkatsuData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.UpdBukkenIkkatsuData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 物件名の削除

        /// --------------------------------------------------
        /// <summary>
        /// 物件名の削除
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tsuji 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelBukkenIkkatsuData(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.DelBukkenIkkatsuData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 進捗管理通知設定

        #region 進捗管理通知設定の取得

        /// --------------------------------------------------
        /// <summary>
        /// 進捗管理通知設定の取得
        /// </summary>
        /// <param name="cond">M01用条件</param>
        /// <returns></returns>
        /// <create>Y.Nakasato 2019/07/08 AR進捗通知対応</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataTable GetShinchokuKanriNotify(CondM01 cond)
        {
            try
            {
                using (var ws = this.GetWsM01())
                {
                    return ws.GetShinchokuKanriNotify(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 進捗管理通知設定の保存

        /// --------------------------------------------------
        /// <summary>
        /// 進捗管理通知設定の保存
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns></returns>
        /// <create>Y.Nakasato 2019/07/08 AR進捗通知対応</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool SaveShinchokuKanriNotify(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (var ws = this.GetWsM01())
                {
                    return ws.SaveShinchokuKanriNotify(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 出荷元保守

        #region 初期表示(出荷元保守)

        /// --------------------------------------------------
        /// <summary>
        /// 初期表示(出荷元保守)
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetInitShukkamotoHoshu(CondM01 cond)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.GetInitShukkamotoHoshu(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        
        #endregion

        #region 出荷元マスタの取得

        /// --------------------------------------------------
        /// <summary>
        /// 出荷元マスタの取得
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <returns>技連マスタ</returns>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetShipFrom(CondM01 cond)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.GetShipFrom(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 出荷元マスタの追加

        /// --------------------------------------------------
        /// <summary>
        /// 出荷元マスタの追加
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InsShipFrom(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.InsShipFrom(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 出荷元マスタの更新

        /// --------------------------------------------------
        /// <summary>
        /// 出荷元マスタの更新
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdShipFrom(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.UpdShipFrom(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 出荷元マスタの削除

        /// --------------------------------------------------
        /// <summary>
        /// 出荷元マスタの削除
        /// </summary>
        /// <param name="cond">M01用コンディション</param>
        /// <param name="ds">データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelShipFrom(CondM01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsM01 ws = this.GetWsM01())
                {
                    return ws.DelShipFrom(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region　権限付与の確認（権限マスタ、権限マップマスタ）2022/10/14
        /// --------------------------------------------------
        /// <summary>
        /// 権限の付与確認
        /// </summary>
        /// <create>TW-Tsuji 2012/10/14</create>
        /// <update></update>
        /// <notes>
        /// 特定の機能について利用権限が付与されているかを
        /// 確認する.
        /// </notes>
        /// --------------------------------------------------
        public bool ExistsRoleAndRolemap(CondM01 cond)
        {
           try
            {
                using (var ws = this.GetWsM01())
                {
                    // 権限・権限マップの検索
                    return ws.ExistsRoleAndRolemap(cond);
                }
            }
            catch (Exception ex)
            {
                // エラーハンドル（念のため）
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion
    }
}
