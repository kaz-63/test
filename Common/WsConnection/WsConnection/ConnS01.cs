using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using WsConnection.WebRefS01;

namespace WsConnection
{
    /// --------------------------------------------------
    /// <summary>
    /// 出荷計画コネクションクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/06/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [DSWUtil.AOP.AspectClass(typeof(AspectConnectionRetry), true)]
    public class ConnS01 : ConnBase
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
        public ConnS01()
        {
        }

        #endregion

        #region S0100010:出荷計画登録

        #region 初期表示(出荷計画)

        /// --------------------------------------------------
        /// <summary>
        /// 初期表示(出荷計画)
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <returns>DataSet</returns>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetInitShukkaKeikaku(CondS01 cond)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.GetInitShukkaKeikaku(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 出荷計画更新

        /// --------------------------------------------------
        /// <summary>
        /// 出荷計画更新
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <param name="dt">出荷データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tajimi 2018/10/31</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InsShukkaKeikaku(CondS01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.InsShukkaKeikaku(cond, ds, out errMsgID, out args);
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
        /// <create>R.Miyoshi 2023/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetBukkenName()
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.GetBukkenNameListForTorikomi();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 荷受先名一覧取得

        /// --------------------------------------------------
        /// <summary>
        /// 荷受先名一覧取得
        /// </summary>
        /// <returns>DataSet</returns>
        /// <create>R.Miyoshi 2023/07/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetConsignList()
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.GetConsignList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 出荷元一覧取得
        /// --------------------------------------------------
        /// <summary>
        /// 出荷元コンボボックスの初期化
        /// </summary>
        /// <create>T.SASAYAMA 2023/07/31</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetShukkamoto()
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.GetShukkamoto();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 出荷先一覧取得
        /// --------------------------------------------------
        /// <summary>
        /// 出荷先コンボボックスの初期化
        /// </summary>
        /// <create>T.SASAYAMA 2023/07/31</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetShukkasaki()
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.GetShukkasaki();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 納入先データ取得
        /// --------------------------------------------------
        /// <summary>
        /// 納入先データ取得
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        /// <create>T.SASAYAMA 2023/07/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetNonyusaki(CondS01 cond)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
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

        #region 出荷計画DB更新

        /// --------------------------------------------------
        /// <summary>
        /// 出荷計画DB更の更新
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="ds"></param>
        /// <param name="errMsgID"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <create>Y.Gwon 2023/09/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdPlanning(CondS01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.UpdPlanning(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region リビジョン登録

        /// --------------------------------------------------
        /// <summary>
        /// リビジョン登録
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.SASAYAMA 2023/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InsertRevision(CondS01 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.InsertRevision(cond, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 排他取得

        /// --------------------------------------------------
        /// <summary>
        /// 排他取得
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>J.Chen 2024/01/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataTable GetHaitaData(CondS01 cond)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.GetHaitaData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 排他登録

        /// --------------------------------------------------
        /// <summary>
        /// 排他登録
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>J.Chen 2024/01/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public int UpdateHaita(CondS01 cond)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.UpdateHaita(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 排他クリア

        /// --------------------------------------------------
        /// <summary>
        /// 排他クリア
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>J.Chen 2024/01/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public int UpdateNullHaita(CondS01 cond)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.UpdateNullHaita(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region S0100020:出荷計画明細登録

        #region 表示条件チェック

        /// --------------------------------------------------
        /// <summary>
        /// 表示条件チェック
        /// </summary>
        /// <param name="dbHelper">DatabaseHelper</param>
        /// <param name="cond">S01用コンディション</param>
        /// <param name="ds">納入先マスタ、AR情報データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>true;成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/07/08</create>
        /// <update>K.Tsutsumi 2012/04/24</update>
        /// --------------------------------------------------
        // 2012/04/24 K.Tsutsumi Change 納入先は事前に登録されている（よって、納入先コードは画面から渡される）
        //public bool CheckDisplayCondition(CondS01 cond, out DataSet ds, out string nonyusakiCD, out string errMsgID, out string[] args)
        public bool CheckDisplayCondition(CondS01 cond, out DataSet ds, out string errMsgID, out string[] args)
        // ↑
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    // 2012/04/24 K.Tsutsumi Change 納入先は事前に登録されている（よって、納入先コードは画面から渡される）
                    //return ws.CheckDisplayCondition(cond, out ds, out nonyusakiCD, out errMsgID, out args);
                    return ws.CheckDisplayCondition(cond, out ds, out errMsgID, out args);
                    // ↑
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
        /// <param name="cond">S01用コンディション</param>
        /// <returns>表示データ</returns>
        /// <create>Y.Higuchi 2010/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetShukkaMeisai(CondS01 cond)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.GetShukkaMeisai(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #region 写真有無取得

        /// --------------------------------------------------
        /// <summary>
        /// 写真有無取得
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <returns>表示データ</returns>
        /// <create>H.Tajimi 2019/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataTable GetExistsPicture(CondS01 cond)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.GetExistsPicture(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 写真有無取得
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <param name="dt">DataTable</param>
        /// <returns>表示データ</returns>
        /// <create>H.Tajimi 2019/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataTable GetExistsPictureFromDataTable(CondS01 cond, DataTable dt)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.GetExistsPictureFromDataTable(cond, dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 出荷計画明細データ登録

        /// --------------------------------------------------
        /// <summary>
        /// 出荷計画明細データ登録
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <param name="dt">出荷明細データ</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/07/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InsShukkaKeikakuMeisai(CondS01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.InsShukkaKeikakuMeisai(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 出荷計画明細データ更新

        /// --------------------------------------------------
        /// <summary>
        /// 出荷計画明細データ更新
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <param name="ds">更新データのDataSet</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdShukkaKeikakuMeisai(CondS01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.UpdShukkaKeikakuMeisai(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #region 出荷計画明細データ削除

        /// --------------------------------------------------
        /// <summary>
        /// 出荷計画明細データ削除
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <param name="ds">削除データのDataSet</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelShukkaKeikakuMeisai(CondS01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.DelShukkaKeikakuMeisai(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #endregion

        #region S0100030:便間移動

        #region ツリー明細取得

        /// --------------------------------------------------
        /// <summary>
        /// ツリー明細取得
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <returns>表示データ</returns>
        /// <create>T.Wakamatsu 2016/01/06</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetTreeMeisai(CondS01 cond)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.GetTreeMeisai(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 便間移動更新

        /// --------------------------------------------------
        /// <summary>
        /// 便間移動更新
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.Wakamatsu 2016/01/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdMoveShip(CondS01 cond)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.UpdMoveShip(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 移動先木枠取得

        /// --------------------------------------------------
        /// <summary>
        /// 移動先木枠取得
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <returns>木枠データ</returns>
        /// <create>T.Wakamatsu 2016/03/31</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetKiwaku(CondS01 cond)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.GetKiwaku(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region S0100023:TAG登録連携

        #region 物件名一覧取得

        /// --------------------------------------------------
        /// <summary>
        /// 物件名一覧取得
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <returns>DataSet</returns>
        /// <create>T.nakata 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public System.Data.DataSet GetBukkenName(CondS01 cond)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.GetBukkenNameList(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 便一覧取得

        /// --------------------------------------------------
        /// <summary>
        /// 便一覧取得
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <returns>DataSet</returns>
        /// <create>T.nakata 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public System.Data.DataSet GetShipList(CondS01 cond)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.GetShipList(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region TAG連携一覧取得

        /// --------------------------------------------------
        /// <summary>
        /// TAG連携一覧取得
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <returns>DataSet</returns>
        /// <create>T.nakata 2018/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public System.Data.DataSet GetTagRenkeiList(CondS01 cond)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.GetTagRenkeiList(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region TAG連携一覧取得(ロック用)

        /// --------------------------------------------------
        /// <summary>
        /// TAG連携一覧取得(ロック用)
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <returns>DataSet</returns>
        /// <create>T.nakata 2018/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public System.Data.DataTable GetAndLockTagRenkeiList(CondS01 cond, DataSet ds, bool isLock)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.GetAndLockTagRenkeiList(cond, ds, isLock);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 有償/無償取得

        /// --------------------------------------------------
        /// <summary>
        /// 有償/無償取得
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <returns>DataSet</returns>
        /// <create>T.nakata 2018/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public System.Data.DataSet GetEstimateFlag(CondS01 cond)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.GetEstimateFlag(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region TAG連携と手配Noの出荷先をチェック

        /// --------------------------------------------------
        /// <summary>
        /// TAG連携と手配Noの出荷先をチェック
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <param name="tehaiNo">手配連携No</param>
        /// <returns>DataTable</returns>
        /// <create>2022/06/01 STEP14</create>
        /// <update></update>
        /// --------------------------------------------------
        public System.Data.DataTable GetShipToForTehaiNo(CondS01 cond, string tehaiNo)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.GetShipToForTehaiNo(cond, tehaiNo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 手配明細バージョン更新

        /// --------------------------------------------------
        /// <summary>
        /// 手配明細バージョン更新
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.nakata 2018/11/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdTehaimeisaiVersionData(CondS01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.UpdTehaimeisaiVersionData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #region TAG連携メールデータ登録

        /// --------------------------------------------------
        /// <summary>
        /// TAG連携メールデータ登録
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <param name="ds"></param>
        /// <param name="errMsgID"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/08/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InsTagRenkeiMail(CondS01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.InsTagRenkeiMail(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region S0100040:一括アップロード

        #region 表示データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 表示データ取得
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <returns>表示データ</returns>
        /// <create>H.Tajimi 2019/08/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetTehaiMeisaiDataForIkkatsuUpload(CondS01 cond, DataSet dsSearch)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.GetTehaiMeisaiDataForIkkatsuUpload(cond, dsSearch);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 図番/型式管理データの登録

        /// --------------------------------------------------
        /// <summary>
        /// 図番/型式管理データの登録
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <param name="ds">更新データのDataSet</param>
        /// <param name="dtZumenKeishiki">図番/型式データテーブル</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tajimi 2019/08/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InsManageZumenKeishiki(CondS01 cond, DataSet ds, out DataTable dtZumenKeishiki, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.InsManageZumenKeishiki(cond, ds, out dtZumenKeishiki, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region S0100050:荷姿表登録

        #region 初期データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 初期データ取得
        /// </summary>
        /// <param name="cond">条件</param>
        /// <returns>初期データ一式</returns>
        /// <create>K.Tsutsumi 2019/03/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet S0100050_GetInit(CondS01 cond)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.S0100050_GetInit(cond);
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
        /// <param name="cond">条件</param>
        /// <returns>物件一覧、納入先一覧、表示データ</returns>
        /// <create>K.Tsutsumi 2019/03/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet S0100050_GetDisp(CondS01 cond)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.S0100050_GetDisp(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 指定出荷日納入情報一覧取得

        /// --------------------------------------------------
        /// <summary>
        /// 指定出荷日納入情報一覧取得
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <returns>DataSet</returns>
        /// <create>T.nakata 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public System.Data.DataSet GetNisugataBaseData(CondS01 cond)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.GetNisugataBaseData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region C/Noより木枠の有無確認（取得できた場合はサイズと重量も取得）

        /// --------------------------------------------------
        /// <summary>
        /// C/Noより木枠の有無確認（取得できた場合はサイズと重量も取得）
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <returns>DataSet</returns>
        /// <create>K.Tsutsumi 2019/03/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet S0100050_GetKiwakuMeisai(CondS01 cond)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.S0100050_GetKiwakuMeisai(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Box No.より、Boxリスト管理データ取得

        /// --------------------------------------------------
        /// <summary>
        /// Box No.より、Boxリスト管理データ取得
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">メッセージパラメータ</param>
        /// <returns>true:存在した false:存在しない</returns>
        /// <create>K.Tsutsumi 2019/03/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool S0100050_GetBoxListManage(CondS01 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.S0100050_GetBoxListManage(cond, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Pallet No.より、Palletリスト管理データ取得

        /// --------------------------------------------------
        /// <summary>
        /// Pallet No.より、Palletリスト管理データ取得
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">メッセージパラメータ</param>
        /// <returns>true:存在した false:存在しない</returns>
        /// <create>K.Tsutsumi 2019/03/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool S0100050_GetPalletListManage(CondS01 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.S0100050_GetPalletListManage(cond, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 荷姿/明細 更新

        /// --------------------------------------------------
        /// <summary>
        /// 荷姿/明細 更新
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.Nakata 2018/12/03</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdNisugata(CondS01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.UpdNisugata(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 荷姿/明細 削除

        /// --------------------------------------------------
        /// <summary>
        /// 荷姿/明細 削除
        /// </summary>
        /// <param name="cond">S01用コンディション</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.Nakata 2018/12/03</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelNisugata(CondS01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.DelNisugata(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 荷姿表Excel出力データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 荷姿表Excel出力データ取得
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/12/03</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetNisugataExcelData(CondS01 cond)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.GetNisugataExcelData(cond);
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
        /// <create>H.Tajimi 2018/12/03</create>
        /// <update></update>
        /// --------------------------------------------------
        public string GetMailID(CondS01 cond, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.GetMailID(cond, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 荷姿更新(荷姿Excel出力用)

        /// --------------------------------------------------
        /// <summary>
        /// 荷姿更新(荷姿Excel出力用)
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="ds"></param>
        /// <param name="errMsgID"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/12/03</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdPackingForExcelData(CondS01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.UpdPackingForExcelData(cond, ds, out errMsgID, out args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region S0100060:Mail送信履歴

        #region Mail送信履歴の取得

        /// --------------------------------------------------
        /// <summary>
        /// Mail送信履歴の取得
        /// </summary>
        /// <param name="cond">S01用条件</param>
        /// <returns></returns>
        /// <create>Y.Gwon 2023/08/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetMailSousinRireki(CondS01 cond)
        {
            try
            {
                using (var ws = this.GetWsS01())
                {
                    return ws.GetMailSousinRireki(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region S0100070:出荷計画照会

        #region 出荷計画一覧取得
        /// --------------------------------------------------
        /// <summary>
        /// 出荷計画一覧取得
        /// </summary>
        /// <create>J.Chen 2023/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetNonyusakiForShokai(CondS01 cond)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.GetNonyusakiForShokai(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 機種一覧取得
        /// --------------------------------------------------
        /// <summary>
        /// 機種コンボボックスの初期化
        /// </summary>
        /// <create>J.Chen 2023/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetKishu()
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.GetKishu();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 製番一覧取得
        /// --------------------------------------------------
        /// <summary>
        /// 製番コンボボックスの初期化
        /// </summary>
        /// <create>J.Chen 2023/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetSeiban()
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.GetSeiban();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 現場用ステータス更新
        /// --------------------------------------------------
        /// <summary>
        /// 現場用ステータス更新
        /// </summary>
        /// <create>J.Chen 2024/10/03</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdNonyusakiForGenbayo(CondS01 cond, DataSet ds, out string errMsgID, out string[] args)
        {
            try
            {
                using (WsS01 ws = this.GetWsS01())
                {
                    return ws.UpdNonyusakiForGenbayo(cond, ds, out errMsgID, out args);
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
