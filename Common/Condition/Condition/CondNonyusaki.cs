using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;

namespace Condition
{
    /// --------------------------------------------------
    /// <summary>
    /// 納入先マスタ用コンディションクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/06/25</create>
    /// <update>R.Miyoshi 2023/07/21(処理フラグ～備考の追加)</update>
    /// --------------------------------------------------
    [System.Xml.Serialization.SoapType()]
    [System.Serializable()]
    public class CondNonyusaki : CondBase
    {
        #region Fields

        //出荷区分
        private string _shukkaFlag = null;
        //納入先コード
        private string _nonyusakiCD = null;
        //納入先
        private string _nonyusakiName = null;
        //出荷便
        private string _ship = null;
        //管理区分
        private string _kanriFlag = null;
        
        //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36、37
        // リスト区分名称0
        private string _listFlagName0 = null;
        // リスト区分名称1
        private string _listFlagName1 = null;
        // リスト区分名称2
        private string _listFlagName2 = null;
        // リスト区分名称3
        private string _listFlagName3 = null;
        // リスト区分名称4
        private string _listFlagName4 = null;
        // リスト区分名称5
        private string _listFlagName5 = null;
        // リスト区分名称6
        private string _listFlagName6 = null;
        // リスト区分名称7
        private string _listFlagName7 = null;
        // 除外フラグ
        private string _removeFlag = null;
        // @@@ ↑
        private string _bukkenNo = null;

        // 運送区分
        private string _transportFlag = null;
        // 有償・無償
        private string _estimateFlag = null;
        // 出荷日
        private string _shipDate = null;
        // 出荷元
        private string _shipFrom = null;
        // 出荷先
        private string _shipTo = null;
        // 案件管理No
        private string _shipNo = null;
        // 案件管理No
        private string _shipSeiban = null;
        // 出荷元CD
        private string _shipFromCD = null;
        // TAG便
        private string _tagship = null;
        // 処理フラグ
        private string _syoriFlag = null;
        // 製番
        private string _seiban = null;
        // 機種
        private string _kishu = null;
        // 内容
        private string _naiyo = null;
        // 到着予定日
        private string _touchakuyoteiDate = null;
        // 機械Parts
        private string _kikaiParts = null;
        // 制御Parts
        private string _seigyoParts = null;
        // 備考
        private string _biko = null;
        // 荷受先名称
        private string _consignName = null;



        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>K.Tsutsumi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondNonyusaki()
            : this(new LoginInfo())
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target">ログインユーザー情報</param>
        /// <create>K.Tsutsumi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public CondNonyusaki(LoginInfo target)
            : base(target)
        {
        }

        #endregion

        #region 出荷区分

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShukkaFlag
        {
            get
            {
                return _shukkaFlag;
            }
            set
            {
                _shukkaFlag = value;
            }
        }

        #endregion

        #region 納入先コード

        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string NonyusakiCD
        {
            get
            {
                return _nonyusakiCD;
            }
            set
            {
                _nonyusakiCD = value;
            }
        }

        #endregion

        #region 納入先

        /// --------------------------------------------------
        /// <summary>
        /// 納入先
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string NonyusakiName
        {
            get
            {
                return _nonyusakiName;
            }
            set
            {
                _nonyusakiName = value;
            }
        }

        #endregion

        #region 出荷便

        /// --------------------------------------------------
        /// <summary>
        /// 出荷便
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Ship
        {
            get
            {
                return _ship;
            }
            set
            {
                _ship = value;
            }
        }

        #endregion

        #region 管理区分

        /// --------------------------------------------------
        /// <summary>
        /// 管理区分
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string KanriFlag
        {
            get
            {
                return _kanriFlag;
            }
            set
            {
                _kanriFlag = value;
            }
        }

        #endregion

        #region リスト区分名称０

        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名称０(Step2 No.36)
        /// </summary>
        /// <create>M.Tsutsumi 2011/02/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ListFlagName0
        {
            get
            {
                return _listFlagName0;
            }
            set
            {
                _listFlagName0 = value;
            }
        }

        #endregion

        #region リスト区分名称１

        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名称１(Step2 No.36)
        /// </summary>
        /// <create>M.Tsutsumi 2011/02/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ListFlagName1
        {
            get
            {
                return _listFlagName1;
            }
            set
            {
                _listFlagName1 = value;
            }
        }

        #endregion

        #region リスト区分名称２

        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名称２(Step2 No.36)
        /// </summary>
        /// <create>M.Tsutsumi 2011/02/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ListFlagName2
        {
            get
            {
                return _listFlagName2;
            }
            set
            {
                _listFlagName2 = value;
            }
        }

        #endregion

        #region リスト区分名称３

        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名称３(Step2 No.36)
        /// </summary>
        /// <create>M.Tsutsumi 2011/02/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ListFlagName3
        {
            get
            {
                return _listFlagName3;
            }
            set
            {
                _listFlagName3 = value;
            }
        }

        #endregion

        #region リスト区分名称４

        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名称４(Step2 No.36)
        /// </summary>
        /// <create>M.Tsutsumi 2011/02/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ListFlagName4
        {
            get
            {
                return _listFlagName4;
            }
            set
            {
                _listFlagName4 = value;
            }
        }

        #endregion

        #region リスト区分名称５

        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名称５(Step2 No.36)
        /// </summary>
        /// <create>M.Tsutsumi 2011/02/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ListFlagName5
        {
            get
            {
                return _listFlagName5;
            }
            set
            {
                _listFlagName5 = value;
            }
        }

        #endregion

        #region リスト区分名称６

        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名称６(Step2 No.36)
        /// </summary>
        /// <create>M.Tsutsumi 2011/02/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ListFlagName6
        {
            get
            {
                return _listFlagName6;
            }
            set
            {
                _listFlagName6 = value;
            }
        }

        #endregion

        #region リスト区分名称７

        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名称７(Step2 No.36)
        /// </summary>
        /// <create>M.Tsutsumi 2011/02/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ListFlagName7
        {
            get
            {
                return _listFlagName7;
            }
            set
            {
                _listFlagName7 = value;
            }
        }

        #endregion

        #region 除外フラグ

        /// --------------------------------------------------
        /// <summary>
        /// 除外フラグ (Step2 No.37)
        /// </summary>
        /// <create>M.Tsutsumi 2011/02/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public string RemoveFlag
        {
            get
            {
                return _removeFlag;
            }
            set
            {
                _removeFlag = value;
            }
        }

        #endregion

        #region 物件管理No

        /// --------------------------------------------------
        /// <summary>
        /// 物件管理No
        /// </summary>
        /// <create>T.Sakiori 2012/04/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public string BukkenNo
        {
            get
            {
                return _bukkenNo;
            }
            set
            {
                _bukkenNo = value;
            }
        }

        #endregion

        #region 画面区分

        /// --------------------------------------------------
        /// <summary>
        /// 画面区分
        /// </summary>
        /// <create>T.Sakiori 2012/05/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public string GamenFlag { get; set; }

        #endregion

        #region 運送区分

        /// --------------------------------------------------
        /// <summary>
        /// 運送区分
        /// </summary>
        /// <create>H.Tajimi 2018/10/31</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TransportFlag
        {
            get
            {
                return this._transportFlag;
            }
            set
            {
                this._transportFlag = value;
            }
        }

        #endregion

        #region 有償・無償

        /// --------------------------------------------------
        /// <summary>
        /// 有償・無償
        /// </summary>
        /// <create>H.Tajimi 2018/10/31</create>
        /// <update></update>
        /// --------------------------------------------------
        public string EstimateFlag
        {
            get
            {
                return this._estimateFlag;
            }
            set
            {
                this._estimateFlag = value;
            }
        }

        #endregion

        #region 出荷日

        /// --------------------------------------------------
        /// <summary>
        /// 出荷日
        /// </summary>
        /// <create>H.Tajimi 2018/10/31</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShipDate
        {
            get
            {
                return this._shipDate;
            }
            set
            {
                this._shipDate = value;
            }
        }

        #endregion

        #region 出荷元

        /// --------------------------------------------------
        /// <summary>
        /// 出荷元
        /// </summary>
        /// <create>H.Tajimi 2018/10/31</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShipFrom
        {
            get
            {
                return this._shipFrom;
            }
            set
            {
                this._shipFrom = value;
            }
        }

        #endregion

        #region 出荷先

        /// --------------------------------------------------
        /// <summary>
        /// 出荷先
        /// </summary>
        /// <create>H.Tajimi 2018/10/31</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShipTo
        {
            get
            {
                return this._shipTo;
            }
            set
            {
                this._shipTo = value;
            }
        }

        #endregion

        #region 案件管理No

        /// --------------------------------------------------
        /// <summary>
        /// 案件管理No
        /// </summary>
        /// <create>H.Tajimi 2018/10/31</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShipNo
        {
            get
            {
                return this._shipNo;
            }
            set
            {
                this._shipNo = value;
            }
        }

        #endregion

        #region 運賃梱包製番

        /// --------------------------------------------------
        /// <summary>
        /// 運賃梱包製番
        /// </summary>
        /// <create>H.Tajimi 2018/10/31</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShipSeiban
        {
            get
            {
                return this._shipSeiban;
            }
            set
            {
                this._shipSeiban = value;
            }
        }

        #endregion

        #region 出荷元CD

        /// --------------------------------------------------
        /// <summary>
        /// 出荷元CD
        /// </summary>
        /// <create>H.Tajimi 2020/04/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShipFromCD
        {
            get
            {
                return this._shipFromCD;
            }
            set
            {
                this._shipFromCD = value;
            }
        }

        #endregion

        #region 元TAG便
        /// --------------------------------------------------
        /// <summary>
        /// 元TAG便
        /// </summary>
        /// <create>T.Sasayama 2023/6/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TagShip
        {
            get { return this._tagship; }
            set { this._tagship = value; }
        }

        #endregion

        #region 処理フラグ

        /// --------------------------------------------------
        /// <summary>
        /// 処理フラグ
        /// </summary>
        /// <create>R.Miyoshi 2023/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public string SyoriFlag
        {
            get
            {
                return this._syoriFlag;
            }
            set
            {
                this._syoriFlag = value;
            }
        }

        #endregion

        #region 製番

        /// --------------------------------------------------
        /// <summary>
        /// 製番
        /// </summary>
        /// <create>R.Miyoshi 2023/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Seiban
        {
            get
            {
                return this._seiban;
            }
            set
            {
                this._seiban = value;
            }
        }

        #endregion

        #region 機種

        /// --------------------------------------------------
        /// <summary>
        /// 機種
        /// </summary>
        /// <create>R.Miyoshi 2023/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Kishu
        {
            get
            {
                return this._kishu;
            }
            set
            {
                this._kishu = value;
            }
        }

        #endregion

        #region 内容

        /// --------------------------------------------------
        /// <summary>
        /// 内容
        /// </summary>
        /// <create>R.Miyoshi 2023/07/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Naiyo
        {
            get
            {
                return this._naiyo;
            }
            set
            {
                this._naiyo = value;
            }
        }

        #endregion

        #region 到着予定日

        /// --------------------------------------------------
        /// <summary>
        /// 到着予定日
        /// </summary>
        /// <create>R.Miyoshi 2023/07/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TouchakuyoteiDate
        {
            get
            {
                return this._touchakuyoteiDate;
            }
            set
            {
                this._touchakuyoteiDate = value;
            }
        }

        #endregion

        #region 機械Parts

        /// --------------------------------------------------
        /// <summary>
        /// 機械Parts
        /// </summary>
        /// <create>R.Miyoshi 2023/07/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public string KikaiParts
        {
            get
            {
                return this._kikaiParts;
            }
            set
            {
                this._kikaiParts = value;
            }
        }

        #endregion

        #region 制御Parts

        /// --------------------------------------------------
        /// <summary>
        /// 制御Parts
        /// </summary>
        /// <create>R.Miyoshi 2023/07/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public string SeigyoParts
        {
            get
            {
                return this._seigyoParts;
            }
            set
            {
                this._seigyoParts = value;
            }
        }

        #endregion

        #region 備考

        /// --------------------------------------------------
        /// <summary>
        /// 備考
        /// </summary>
        /// <create>R.Miyoshi 2023/07/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Biko
        {
            get
            {
                return this._biko;
            }
            set
            {
                this._biko = value;
            }
        }

        #endregion

        #region 荷受先名称
        /// --------------------------------------------------
        /// <summary>
        /// 荷受先名称
        /// </summary>
        /// <create>R.Miyoshi 2023/07/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ConsignName
        {
            get { return this._consignName; }
            set { this._consignName = value; }
        }
        #endregion


    }
}
