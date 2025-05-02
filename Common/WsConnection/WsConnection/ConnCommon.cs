using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using WsConnection.WebRefCommon;

namespace WsConnection
{
    /// --------------------------------------------------
    /// <summary>
    /// 共通処理用コネクションクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/04/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [DSWUtil.AOP.AspectClass(typeof(AspectConnectionRetry), true)]
    public class ConnCommon : ConnBase
    {
        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        [DSWUtil.AOP.AspectExecute(false)]
        public ConnCommon()
        {
        }

        #endregion

        #region サーバーの時間を取得

        /// --------------------------------------------------
        /// <summary>
        /// サーバーの時間を取得
        /// </summary>
        /// <returns>現在時間</returns>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public DateTime GetNowDateTime()
        {
            try
            {
                using (WsCommon ws = this.GetWsCommon())
                {
                    return ws.GetNowDateTime();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region SELECT

        #region システムパラメーター取得

        /// --------------------------------------------------
        /// <summary>
        /// システムパラメーター取得
        /// </summary>
        /// <returns>システムパラメーター</returns>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update>H.Tajimi 2018/09/03 多言語対応</update>
        /// --------------------------------------------------
        public DataSet GetSystemParameter(CondCommon cond)
        {
            try
            {
                using (WsCommon ws = this.GetWsCommon())
                {
                    return ws.GetSystemParameter(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ログインユーザー情報取得

        /// --------------------------------------------------
        /// <summary>
        /// ログインユーザー情報取得
        /// </summary>
        /// <param name="cond">検索条件</param>
        /// <returns>ログインユーザー情報</returns>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetLoginUser(CondCommon cond)
        {
            try
            {
                using (WsCommon ws = this.GetWsCommon())
                {
                    return ws.GetLoginUser(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region メニュー取得

        /// --------------------------------------------------
        /// <summary>
        /// メニュー取得
        /// </summary>
        /// <param name="cond">検索条件</param>
        /// <returns>メニュー</returns>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetMenu(CondCommon cond)
        {
            try
            {
                using (WsCommon ws = this.GetWsCommon())
                {
                    return ws.GetMenu(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #region メッセージ取得

        /// --------------------------------------------------
        /// <summary>
        /// メッセージ取得
        /// </summary>
        /// <param name="cond">検索条件</param>
        /// <returns>メッセージ</returns>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetMessage(CondCommon cond)
        {
            try
            {
                using (WsCommon ws = this.GetWsCommon())
                {
                    return ws.GetMessage(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 複数メッセージ取得
        /// </summary>
        /// <param name="cond">検索条件</param>
        /// <returns>メッセージ</returns>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetMultiMessage(CondCommon cond)
        {
            try
            {
                using (WsCommon ws = this.GetWsCommon())
                {
                    return ws.GetMultiMessage(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 汎用マスタ取得

        /// --------------------------------------------------
        /// <summary>
        /// 汎用マスタ取得
        /// </summary>
        /// <param name="cond">検索条件</param>
        /// <returns>汎用マスタ</returns>
        /// <create>Y.Higuchi 2010/04/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetCommon(CondCommon cond)
        {
            try
            {
                using (WsCommon ws = this.GetWsCommon())
                {
                    return ws.GetCommon(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region メール登録前の確認

        /// --------------------------------------------------
        /// <summary>
        /// メール登録前の確認
        /// </summary>
        /// <param name="cond">条件</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2017/09/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet CheckMail(CondCommon cond)
        {
            try
            {
                using (var ws = this.GetWsCommon())
                {
                    return ws.CheckMail(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region メール登録前の確認(荷姿用)

        /// --------------------------------------------------
        /// <summary>
        /// メール登録前の確認(荷姿用)
        /// </summary>
        /// <param name="cond">条件</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet CheckPackingMail(CondCommon cond)
        {
            try
            {
                using (var ws = this.GetWsCommon())
                {
                    return ws.CheckPackingMail(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region メール登録前の確認(TAG連携用)

        /// --------------------------------------------------
        /// <summary>
        /// メール登録前の確認(TAG連携用)
        /// </summary>
        /// <param name="cond">条件</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/08/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet CheckTagRenkeiMail(CondCommon cond)
        {
            try
            {
                using (var ws = this.GetWsCommon())
                {
                    return ws.CheckTagRenkeiMail(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region メール登録前の確認(出荷計画用)

        /// --------------------------------------------------
        /// <summary>
        /// メール登録前の確認(出荷計画用)
        /// </summary>
        /// <param name="cond">条件</param>
        /// <returns></returns>
        /// <create>Y.Gwon 2023/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet CheckPlanningMail(CondCommon cond)
        {
            try
            {
                using (var ws = this.GetWsCommon())
                {
                    return ws.CheckPlanningMail(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 登録に必要なメールデータ取得

        /// --------------------------------------------------
        /// <summary>
        /// 登録に必要なメールデータ取得
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        /// <create>T.Sakiori 2017/09/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataTable GetMailData(CondCommon cond)
        {
            try
            {
                using (var ws = this.GetWsCommon())
                {
                    return ws.GetMailData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region INSERT

        #region メールデータの登録

        /// --------------------------------------------------
        /// <summary>
        /// メールデータの登録
        /// </summary>
        /// <param name="cond">ユーザー情報</param>
        /// <param name="dt">登録データ</param>
        /// <param name="errMsgId">エラーメッセージ</param>
        /// <param name="args">エラーメッセージ引数</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2017/09/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool SaveMail(CondCommon cond, DataTable dt, out string errMsgId, out string[] args)
        {
            try
            {
                using (var ws = this.GetWsCommon())
                {
                    return ws.SaveMail(cond, dt, out errMsgId, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region UPDATE

        /// --------------------------------------------------
        /// <summary>
        /// パスワード変更
        /// </summary>
        /// <param name="cond">パスワード変更コンディション</param>
        /// <param name="ds">ログイン情報</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">メッセージのパラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdUserPassword(CondUserPassword cond, out DataSet ds, out  string errMsgID, out string[] args)
        {
            try
            {
                using (WsCommon ws = this.GetWsCommon())
                {
                    return ws.UpdUserPassword(cond, out ds, out errMsgID, out args);
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
