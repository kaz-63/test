using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using WsConnection.WebRefJ01;

namespace WsConnection
{
    /// --------------------------------------------------
    /// <summary>
    /// 常駐コネクションクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/06/23</create>
    /// <update></update>
    /// --------------------------------------------------
    [DSWUtil.AOP.AspectClass(typeof(AspectConnectionRetry), true)]
    public class ConnJ01 : ConnBase
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
        public ConnJ01()
        {
        }

        #endregion

        #region J0100020:日次処理

        #region 締めマスタ取得

        /// --------------------------------------------------
        /// <summary>
        /// 締めマスタ取得
        /// </summary>
        /// <returns>締めマスタ</returns>
        /// <create>Y.Higuchi 2010/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetShime()
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.GetShime();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #region 締め開始処理

        /// --------------------------------------------------
        /// <summary>
        /// 締め開始処理
        /// </summary>
        /// <param name="ds">締め処理マスタ</param>
        /// <param name="isProcessed">処理中かどうか</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool StartShime(out DataSet ds, out bool isProcessed)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.StartShime(out ds, out isProcessed);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #region 締めマスタ更新処理

        /// --------------------------------------------------
        /// <summary>
        /// 締めマスタ更新処理
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdShime(CondJ01 cond)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.UpdShime(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #region 本体の納入先マスタ完了処理

        /// --------------------------------------------------
        /// <summary>
        /// 本体の納入先マスタ完了処理
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdHontaiNonyusakiKanryo(CondJ01 cond)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.UpdHontaiNonyusakiKanryo(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 本体の納入先マスタ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 本体の納入先マスタ削除処理
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelHontaiNonyusakiData(CondJ01 cond)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.DelHontaiNonyusakiData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 本体の納入先マスタ削除処理(指定年で無条件)

        /// --------------------------------------------------
        /// <summary>
        /// 本体の納入先マスタ削除処理(指定年で無条件)
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.Sakiori 2015/06/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelHontaiNonyusakiDataUncondition(CondJ01 cond)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.DelHontaiNonyusakiDataUncondition(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 本体の物件名マスタ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 本体の物件名マスタ削除処理
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2012/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelHontaiBukkenData(CondJ01 cond)
        {
            try
            {
                using (var ws = this.GetWsJ01())
                {
                    return ws.DelHontaiBukkenData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 本体の出荷明細データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 本体の出荷明細データ削除処理
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelHontaiShukkaMeisaiData(CondJ01 cond)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.DelHontaiShukkaMeisaiData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ARの納入先マスタ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// ARの納入先マスタ削除処理
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelARNonyusakiData(CondJ01 cond)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.DelARNonyusakiData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ARの納入先マスタ削除処理(指定年で無条件)

        /// --------------------------------------------------
        /// <summary>
        /// ARの納入先マスタ削除処理(指定年で無条件)
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.Sakiori 2015/06/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelARNonyusakiDataUncondition(CondJ01 cond)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.DelARNonyusakiDataUncondition(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ARの物件名マスタ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// ARの物件名マスタ削除処理
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2012/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelARBukkenData(CondJ01 cond)
        {
            try
            {
                using (var ws = this.GetWsJ01())
                {
                    return ws.DelARBukkenData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region AR情報データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// AR情報データ削除処理
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <param name="ds">削除対象データ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelARData(CondJ01 cond, out DataSet ds)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.DelARData(cond, out ds);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ARの出荷明細データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// ARの出荷明細データ削除処理
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelARShukkaMeisaiData(CondJ01 cond)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.DelARShukkaMeisaiData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region BOXリスト管理データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// BOXリスト管理データ削除処理
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelBoxlistManageData(CondJ01 cond)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.DelBoxlistManageData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region パレットリスト管理データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// パレットリスト管理データ削除処理
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelPalletlistManageData(CondJ01 cond)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.DelPalletlistManageData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 木枠データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 木枠データ削除処理
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <param name="ds">削除対象データ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelKiwakuData(CondJ01 cond, out DataSet ds)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.DelKiwakuData(cond, out ds);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 木枠明細データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 木枠明細データ削除処理
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelKiwakuMeisaiData(CondJ01 cond)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.DelKiwakuMeisaiData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 社外木枠明細データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 社外木枠明細データ削除処理
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelShagaiKiwakuMeisaiData(CondJ01 cond)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.DelShagaiKiwakuMeisaiData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 採番マスタ削除処理(ARNo.)

        /// --------------------------------------------------
        /// <summary>
        /// 採番マスタ削除処理(ARNo.)
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelSaibanARNo(CondJ01 cond)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.DelSaibanARNo(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #region 一時作業データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 一時作業データ削除処理
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/09/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelTempworkData(CondJ01 cond)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.DelTempworkData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 本体の履歴データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 本体の履歴データ削除処理
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2012/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelHontaiRirekiData(CondJ01 cond)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.DelHontaiRirekiData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ARの履歴データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// ARの履歴データ削除処理
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2012/05/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelARRirekiData(CondJ01 cond)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.DelARRirekiData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ロケーションマスタ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// ロケーションマスタ削除処理
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <returns>影響を与えた行数</returns>
        /// <create>T.Wakamatsu 2013/09/03</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelLocationData(CondJ01 cond)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.DelLocationData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 在庫データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 在庫データ削除処理
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <returns>影響を与えた行数</returns>
        /// <create>T.Wakamatsu 2013/09/03</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelStockData(CondJ01 cond)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.DelStockData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 棚卸データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 棚卸データ削除処理
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <returns>影響を与えた行数</returns>
        /// <create>T.Wakamatsu 2013/09/03</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelInventData(CondJ01 cond)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.DelInventData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 実績データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 実績データ削除処理
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <returns>影響を与えた行数</returns>
        /// <create>T.Wakamatsu 2013/09/03</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelJissekiData(CondJ01 cond)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.DelJissekiData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 一時作業データ（現地部品管理）削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 一時作業データ（現地部品管理）削除処理
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <returns>影響を与えた行数</returns>
        /// <create>T.Wakamatsu 2013/09/03</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelBuhinTempworkData(CondJ01 cond)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.DelBuhinTempworkData(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region DBバックアップ

        /// --------------------------------------------------
        /// <summary>
        /// DBバックアップ
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DBBackup(CondJ01 cond)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.DBBackup(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #region メールデータ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// メールデータ削除処理
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2017/09/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelMail(CondJ01 cond)
        {
            try
            {
                using (var ws = this.GetWsJ01())
                {
                    return ws.DelMail(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region J0100030:メール送信

        #region メール設定マスタ取得

        /// --------------------------------------------------
        /// <summary>
        /// メール設定マスタ取得
        /// </summary>
        /// <returns></returns>
        /// <create>R.Katsuo 2017/09/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetMailSetting()
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.GetMailSetting();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region メールデータ取得

        /// --------------------------------------------------
        /// <summary>
        /// メールデータ取得
        /// </summary>
        /// <returns></returns>
        /// <create>R.Katsuo 2017/09/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetMail()
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.GetMail();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region メールデータ更新処理

        /// --------------------------------------------------
        /// <summary>
        /// メールデータ更新処理
        /// </summary>
        /// <param name="dt">更新データ</param>
        /// <returns></returns>
        /// <create>R.Katsuo 2017/09/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdMail(DataTable dt)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.UpdMail(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region J0100040:SKS連携処理

        #region 初期化取得

        /// --------------------------------------------------
        /// <summary>
        /// 初期化取得
        /// </summary>
        /// <returns>SKS連携マスタ/SKS手配明細WORK</returns>
        /// <create>H.Tajimi 2018/11/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetInit()
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.GetInit();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region SKS連携データ取得

        /// --------------------------------------------------
        /// <summary>
        /// SKS連携データ取得
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataSet GetSKS()
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.GetSKS();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region SKS連携開始処理

        /// --------------------------------------------------
        /// <summary>
        /// SKS連携開始処理
        /// </summary>
        /// <param name="ds">SKS連携マスタ</param>
        /// <param name="isProcessed">処理中かどうか</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tajimi 2018/11/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool StartSKS(out DataSet ds, out bool isProcessed)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.StartSKS(out ds, out isProcessed);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region SKS連携マスタ更新処理

        /// --------------------------------------------------
        /// <summary>
        /// SKS連携マスタ更新処理
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tajimi 2018/11/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdSKS(CondJ01 cond)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.UpdSKS(cond);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region SKS手配明細WORK 削除処理(全レコード削除)

        /// --------------------------------------------------
        /// <summary>
        /// SKS手配明細WORK 削除処理(全レコード削除)
        /// </summary>
        /// <param name="cond">J01 コンディション</param>
        /// <param name="count">影響を受けたレコード数</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tajimi 2018/11/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool DelTehaiSKSWork(CondJ01 cond, out int count)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.DelTehaiSKSWork(cond, out count);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region SKS手配明細WORK 登録処理

        /// --------------------------------------------------
        /// <summary>
        /// SKS手配明細WORK 登録処理
        /// </summary>
        /// <param name="cond">J01 コンディション</param>
        /// <param name="dt">SKS手配明細WORK</param>
        /// <param name="ds">結果</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>K.Tsutsumi 2018/01/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InsTehaiSKSWork(CondJ01 cond, DataTable dt, out DataSet ds)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.InsTehaiSKSWork(cond, dt, out ds);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region SKS手配明細WORK 更新処理(回答納期が「11111111」のときは完納とする)

        /// --------------------------------------------------
        /// <summary>
        /// SKS手配明細WORK 更新処理(回答納期が「11111111」のときは完納とする)
        /// </summary>
        /// <param name="cond">J01 コンディション</param>
        /// <param name="count">影響を受けたレコード数</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>K.Tsutsumi 2018/01/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdSKSTehaiWorkHacchuZyotai(CondJ01 cond, out int count)
        {
            try
            {
                count = 0;
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.UpdSKSTehaiWorkHacchuZyotai(cond, out count);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region SKS手配明細WORK 更新処理(見積状態のときは単価を0円にする)

        /// --------------------------------------------------
        /// <summary>
        /// SKS手配明細WORK 更新処理(見積状態のときは単価を0円にする)
        /// </summary>
        /// <param name="cond">J01 コンディション</param>
        /// <param name="count">影響を受けたレコード数</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>K.Tsutsumi 2018/01/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdSKSTehaiWorkTehaiUnitPrice(CondJ01 cond, out int count)
        {
            try
            {
                count = 0;
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.UpdSKSTehaiWorkTehaiUnitPrice(cond, out count);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region SKS手配明細更新処理

        /// --------------------------------------------------
        /// <summary>
        /// SKS手配明細更新処理
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <param name="ds">結果</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tajimi 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdTehaiSKS(CondJ01 cond, out DataSet ds)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.UpdTehaiSKS(cond, out ds);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region SKS手配明細登録処理

        /// --------------------------------------------------
        /// <summary>
        /// SKS手配明細登録処理
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <param name="ds">結果</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tajimi 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InsTehaiSKS(CondJ01 cond, out DataSet ds)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.InsTehaiSKS(cond, out ds);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 手配明細更新処理

        /// --------------------------------------------------
        /// <summary>
        /// 手配明細更新処理
        /// </summary>
        /// <param name="cond">J01用コンディション</param>
        /// <param name="ds">結果</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tajimi 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdTehaiMeisai(CondJ01 cond, out DataSet ds)
        {
            try
            {
                using (WsJ01 ws = this.GetWsJ01())
                {
                    return ws.UpdTehaiMeisai(cond, out ds);
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
