using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using WsConnection.WebRefK04;

namespace WsConnection
{
    /// --------------------------------------------------
    /// <summary>
    /// 取込結果コネクションクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/06/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [DSWUtil.AOP.AspectClass(typeof(AspectConnectionRetry), true)]
    public class ConnK04 : ConnBase
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
        public ConnK04()
        {
        }

        #endregion

        #region K0400010:取込エラー一覧

        #region 一時取込データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 一時取込データ取得
        /// </summary>
        /// <param name="cond">K04用コンディション</param>
        /// <returns>一時取込データ</returns>
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetTempwork(CondK04 cond)
        {
            try
            {
                using (WsK04 ws = this.GetWsK04())
                {
                    return ws.GetTempwork(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ハンディデータ取込

        /// --------------------------------------------------
        /// <summary>
        /// ハンディデータ取込
        /// </summary>
        /// <param name="cond">K04用コンディション</param>
        /// <param name="ds">取込データ</param>
        /// <param name="dtMessage">メッセージテーブル</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool ImportData(CondK04 cond, DataSet ds, ref DataTable dtMessage)
        {
            try
            {
                using (WsK04 ws = this.GetWsK04())
                {
                    return ws.ImportData(cond, ds, ref dtMessage);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ハンディデータ取込再試行

        /// --------------------------------------------------
        /// <summary>
        /// ハンディデータ取込再試行
        /// </summary>
        /// <param name="cond">K04用コンディション</param>
        /// <param name="dtMessage">メッセージテーブル</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool ImportRetry(CondK04 cond, ref DataTable dtMessage)
        {
            try
            {
                using (WsK04 ws = this.GetWsK04())
                {
                    return ws.ImportRetry(cond, ref dtMessage);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #region ハンディデータ破棄

        /// --------------------------------------------------
        /// <summary>
        /// ハンディデータ破棄
        /// </summary>
        /// <param name="cond">K04用コンディション</param>
        /// <param name="dtMessage">メッセージテーブル</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DestroyData(CondK04 cond, ref DataTable dtMessage)
        {
            try
            {
                using (WsK04 ws = this.GetWsK04())
                {
                    return ws.DestroyData(cond,ref dtMessage);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #endregion

        #region 一時取込明細データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 一時取込明細データ取得
        /// </summary>
        /// <param name="cond">K04用コンディション</param>
        /// <returns>一時取込明細データ</returns>
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetTempworkMeisai(CondK04 cond)
        {
            try
            {
                using (WsK04 ws = this.GetWsK04())
                {
                    return ws.GetTempworkMeisai(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #region K0400050:取込エラー詳細(検品)

        /// --------------------------------------------------
        /// <summary>
        /// 一時取込明細データ(検品)取得
        /// </summary>
        /// <param name="cond">K04用コンディション</param>
        /// <returns>一時取込明細(検品)データ</returns>
        /// <create>H.Tsuji 2020/06/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetTempworkMeisaiKenpin(CondK04 cond)
        {
            try
            {
                using (WsK04 ws = this.GetWsK04())
                {
                    return ws.GetTempworkMeisaiKenpin(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// OK分の検品データ取込を再試行する
        /// </summary>
        /// <param name="cond">K04用コンディション</param>
        /// <param name="ds">取込データ</param>
        /// <param name="dtMessage">メッセージテーブル</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tsuji 2020/06/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool ImportRetryKenpin(CondK04 cond, DataSet ds, ref DataTable dtMessage)
        {
            try
            {
                using (WsK04 ws = this.GetWsK04())
                {
                    return ws.ImportRetryKenpin(cond, ds, ref dtMessage);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region K0400070:Handy操作履歴照会

        #region 初期化データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 初期化データ取得
        /// </summary>
        /// <param name="cond">K04用コンディション</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetInitHandyOpeRirekiShokai(CondK04 cond)
        {
            try
            {
                using (WsK04 ws = this.GetWsK04())
                {
                    return ws.GetInitHandyOpeRirekiShokai(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        
        #endregion

        #region 表示データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 表示データ取得
        /// </summary>
        /// <param name="cond">K04用コンディション</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetHandyOpeRireki(CondK04 cond)
        {
            try
            {
                using (WsK04 ws = this.GetWsK04())
                {
                    return ws.GetHandyOpeRireki(cond);
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
