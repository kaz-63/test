using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;

namespace Condition
{
    /// --------------------------------------------------
    /// <summary>
    /// S02用コンディション
    /// </summary>
    /// <create>Y.Higuchi 2010/07/21</create>
    /// <update></update>
    /// --------------------------------------------------
    public class CondS02 : CondBase
    {
        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 出荷No.(出荷情報登録)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _shukkaNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// AR No.
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _arNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _nonyusakiCD = null;
        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード(複数)
        /// </summary>
        /// <create>H.Tajimi 2015/12/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private string[] _nonyusakiCDs = null;
        /// --------------------------------------------------
        /// <summary>
        /// 管理区分
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _kanriFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// 作業区分
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _sagyoFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// 状態区分
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _jyotaiFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷日付
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private object _shukkaDate = DBNull.Value;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷ユーザーID
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private object _shukkaUserID = DBNull.Value;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷ユーザー名
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private object _shukkaUserName = DBNull.Value;
        /// --------------------------------------------------
        /// <summary>
        /// 運送会社
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private object _unsokaishaName = DBNull.Value;
        /// --------------------------------------------------
        /// <summary>
        /// インボイスNo
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private object _invoiceNo = DBNull.Value;
        /// --------------------------------------------------
        /// <summary>
        /// 送り状No
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private object _okurijyoNo = DBNull.Value;
        /// --------------------------------------------------
        /// <summary>
        /// BLNo
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private object _blNo = DBNull.Value;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _shukkaFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// Box No.
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _boxNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷便
        /// </summary>
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _ship = null;
        /// --------------------------------------------------
        /// <summary>
        /// 表示選択
        /// </summary>
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _dispSelect = null;
        /// --------------------------------------------------
        /// <summary>
        /// 納入先名称
        /// </summary>
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _nonyusakiName = null;
        /// --------------------------------------------------
        /// <summary>
        /// 品名(和文)
        /// </summary>
        /// <create>Y.Higuchi 2010/10/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _hinmeiJp = null;
        /// --------------------------------------------------
        /// <summary>
        /// 品名
        /// </summary>
        /// <create>Y.Higuchi 2010/10/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _hinmei = null;
        /// --------------------------------------------------
        /// <summary>
        /// 図番/形式
        /// </summary>
        /// <create>Y.Higuchi 2010/10/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _zumenKeishiki = null;
        /// --------------------------------------------------
        /// <summary>
        /// 文字列：梱包済み
        /// </summary>
        /// <create>D.Okumura 2018/10/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _textKonpozumi = null;
        /// --------------------------------------------------
        /// <summary>
        /// 文字列：出荷
        /// </summary>
        /// <create>D.Okumura 2018/10/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _textShukka = null;
        /// --------------------------------------------------
        /// <summary>
        /// 文字列:詳細
        /// </summary>
        /// <create>S.Furugo 2018/10/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _textShosai = null;
        /// --------------------------------------------------
        /// <summary>
        /// 国内外
        /// </summary>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _kokunaigai = null;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷日
        /// </summary>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _syukkadate = null;
        /// --------------------------------------------------
        /// <summary>
        /// 発行状態
        /// </summary>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _hakkouflag = null;
        /// --------------------------------------------------
        /// <summary>
        /// 荷姿CD
        /// </summary>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _packingno = null;
        /// --------------------------------------------------
        /// <summary>
        /// 手配連携No
        /// </summary>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _tehairenkeino = null;
        /// --------------------------------------------------
        /// <summary>
        /// 見積No
        /// </summary>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _estimateno = null;
        /// --------------------------------------------------
        /// <summary>
        /// 工事識別管理NO
        /// </summary>
        /// <create>T.Nakata 2018/12/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _kojino = null;
        /// --------------------------------------------------
        /// <summary>
        /// 荷受CD
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _consigncd = null;
        /// --------------------------------------------------
        /// <summary>
        /// 配送先CD
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _delivercd = null;
        /// --------------------------------------------------
        /// <summary>
        /// 運送会社CD
        /// </summary>
        /// <create>T.Nakata 2018/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _unsokaishano = null;
        /// --------------------------------------------------
        /// <summary>
        /// 内部見積番号
        /// </summary>
        /// <create>T.Nakata 2018/12/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _internalpono = null;
        /// --------------------------------------------------
        /// <summary>
        /// 元TAG便
        /// </summary>
        /// <create>T.Sasayama 2023/6/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _tagship = null;
        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>K.Tsutsumi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondS02()
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
        public CondS02(LoginInfo target)
            : base(target)
        {
        }
        
        #endregion

        #region 出荷No.(出荷情報登録)

        /// --------------------------------------------------
        /// <summary>
        /// 出荷No.(出荷情報登録)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShukkaNo
        {
            get { return this._shukkaNo; }
            set { this._shukkaNo = value; }
        }

        #endregion
        
        #region 出荷区分

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShukkaFlag
        {
            get { return this._shukkaFlag; }
            set { this._shukkaFlag = value; }
        }

        #endregion
        
        #region 納入先コード

        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public string NonyusakiCD
        {
            get { return this._nonyusakiCD; }
            set { this._nonyusakiCD = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード（複数）
        /// </summary>
        /// <create>H.Tajimi 2015/12/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public string[] NonyusakiCDs
        {
            get { return this._nonyusakiCDs; }
            set { this._nonyusakiCDs = value; }
        }

        #endregion
        
        #region AR No.

        /// --------------------------------------------------
        /// <summary>
        /// AR No.
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ARNo
        {
            get { return this._arNo; }
            set { this._arNo = value; }
        }

        #endregion

        #region 管理区分

        /// --------------------------------------------------
        /// <summary>
        /// 管理区分
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public string KanriFlag
        {
            get { return this._kanriFlag; }
            set { this._kanriFlag = value; }
        }

        #endregion
        
        #region 作業区分

        /// --------------------------------------------------
        /// <summary>
        /// 作業区分
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public string SagyoFlag
        {
            get { return this._sagyoFlag; }
            set { this._sagyoFlag = value; }
        }

        #endregion
        
        #region 状態区分

        /// --------------------------------------------------
        /// <summary>
        /// 状態区分
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public string JyotaiFlag
        {
            get { return this._jyotaiFlag; }
            set { this._jyotaiFlag = value; }
        }

        #endregion

        #region 出荷日付

        /// --------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public object ShukkaDate
        {
            get { return this._shukkaDate; }
            set { this._shukkaDate = value; }
        }

        #endregion
        
        #region 出荷ユーザーID

        /// --------------------------------------------------
        /// <summary>
        /// 出荷ユーザーID
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public object ShukkaUserID
        {
            get { return this._shukkaUserID; }
            set { this._shukkaUserID = value; }
        }

        #endregion

        #region 出荷ユーザー名

        /// --------------------------------------------------
        /// <summary>
        /// 出荷ユーザー名
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public object ShukkaUserName
        {
            get { return this._shukkaUserName; }
            set { this._shukkaUserName = value; }
        }

        #endregion

        #region 運送会社

        /// --------------------------------------------------
        /// <summary>
        /// 運送会社
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public object UnsokaishaName
        {
            get { return this._unsokaishaName; }
            set { this._unsokaishaName = value; }
        }

        #endregion

        #region インボイスNo

        /// --------------------------------------------------
        /// <summary>
        /// インボイスNo
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public object InvoiceNo
        {
            get { return this._invoiceNo; }
            set { this._invoiceNo = value; }
        }

        #endregion

        #region 送り状No

        /// --------------------------------------------------
        /// <summary>
        /// 送り状No
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public object OkurijyoNo
        {
            get { return this._okurijyoNo; }
            set { this._okurijyoNo = value; }
        }

        #endregion

        #region BLNo

        /// --------------------------------------------------
        /// <summary>
        /// BLNo
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public object BLNo
        {
            get { return this._blNo; }
            set { this._blNo = value; }
        }

        #endregion

        #region Box No.

        /// --------------------------------------------------
        /// <summary>
        /// Box No.
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public string BoxNo
        {
            get { return this._boxNo; }
            set { this._boxNo = value; }
        }

        #endregion

        #region 納入先名称

        /// --------------------------------------------------
        /// <summary>
        /// 納入先名称
        /// </summary>
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public string NonyusakiName
        {
            get { return this._nonyusakiName; }
            set { this._nonyusakiName = value; }
        }

        #endregion
        
        #region 出荷便

        /// --------------------------------------------------
        /// <summary>
        /// 出荷便
        /// </summary>
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Ship
        {
            get { return this._ship; }
            set { this._ship = value; }
        }

        #endregion

        #region 表示選択

        /// --------------------------------------------------
        /// <summary>
        /// 表示選択
        /// </summary>
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public string DispSelect
        {
            get { return this._dispSelect; }
            set { this._dispSelect = value; }
        }

        #endregion

        #region 品名(和文)

        /// --------------------------------------------------
        /// <summary>
        /// 品名(和文)
        /// </summary>
        /// <create>Y.Higuchi 2010/10/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string HinmeiJp
        {
            get { return this._hinmeiJp; }
            set { this._hinmeiJp = value; }
        }

        #endregion

        #region 品名

        /// --------------------------------------------------
        /// <summary>
        /// 品名
        /// </summary>
        /// <create>Y.Higuchi 2010/10/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Hinmei
        {
            get { return this._hinmei; }
            set { this._hinmei = value; }
        }

        #endregion

        #region 図番/形式

        /// --------------------------------------------------
        /// <summary>
        /// 図番/形式
        /// </summary>
        /// <create>Y.Higuchi 2010/10/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ZumenKeishiki
        {
            get { return this._zumenKeishiki; }
            set { this._zumenKeishiki = value; }
        }

        #endregion

        #region 木枠No

        /// --------------------------------------------------
        /// <summary>
        /// 木枠No
        /// </summary>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public string KiwakuNo { get; set; }

        #endregion

        #region C/NO

        /// --------------------------------------------------
        /// <summary>
        /// C/NO
        /// </summary>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public string CaseNo { get; set; }

        #endregion

        #region パレットNo

        /// --------------------------------------------------
        /// <summary>
        /// パレットNo
        /// </summary>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public string PalletNo { get; set; }

        #endregion

        #region TagNo

        /// --------------------------------------------------
        /// <summary>
        /// TagNo
        /// </summary>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TagNo { get; set; }

        #endregion

        #region 梱包済みから出荷に変更

        /// --------------------------------------------------
        /// <summary>
        /// 梱包済みから出荷に変更
        /// </summary>
        /// <create>T.Sakiori 2012/04/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool IsKonpo2Shukka { get; set; }

        #endregion

        #region 出荷から出荷に変更

        /// --------------------------------------------------
        /// <summary>
        /// 出荷から出荷に変更
        /// </summary>
        /// <create>T.Sakiori 2012/04/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool IsShukka2Shukka { get; set; }

        #endregion

        #region 文字列：梱包済み
        /// --------------------------------------------------
        /// <summary>
        /// 文字列：梱包済み
        /// </summary>
        /// <create>D.Okumura 2018/10/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TextKonpozumi
        {
            get { return this._textKonpozumi; }
            set { this._textKonpozumi = value; }
        }
        #endregion

        #region 文字列：出荷
        /// --------------------------------------------------
        /// <summary>
        /// 文字列：出荷
        /// </summary>
        /// <create>D.Okumura 2018/10/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TextShukka
        {
            get { return this._textShukka; }
            set { this._textShukka = value; }
        }
        #endregion

        #region 文字列:詳細
        /// --------------------------------------------------
        /// <summary>
        /// 文字列:詳細
        /// </summary>
        /// <create>S.Fuugo 2018/10/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TextShosai
        {
            get { return this._textShosai; }
            set { this._textShosai = value; }
        }

        #endregion

        #region 国内外
        /// --------------------------------------------------
        /// <summary>
        /// 国内外
        /// </summary>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Kokunaigai
        {
            get { return this._kokunaigai; }
            set { this._kokunaigai = value; }
        }

        #endregion

        #region 出荷日
        /// --------------------------------------------------
        /// <summary>
        /// 出荷日
        /// </summary>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public string SyukkaDate
        {
            get { return this._syukkadate; }
            set { this._syukkadate = value; }
        }

        #endregion

        #region 発行状態
        /// --------------------------------------------------
        /// <summary>
        /// 発行状態
        /// </summary>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public string HakkouFlag
        {
            get { return this._hakkouflag; }
            set { this._hakkouflag = value; }
        }

        #endregion

        #region 荷姿CD
        /// --------------------------------------------------
        /// <summary>
        /// 荷姿CD
        /// </summary>
        /// <create>T.Nakata 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public string PackingNo
        {
            get { return this._packingno; }
            set { this._packingno = value; }
        }

        #endregion

        #region 手配連携No
        /// --------------------------------------------------
        /// <summary>
        /// 手配連携No
        /// </summary>
        /// <create>T.Nakata 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TehaiRenkeiNo
        {
            get { return this._tehairenkeino; }
            set { this._tehairenkeino = value; }
        }

        #endregion

        #region 見積No
        /// --------------------------------------------------
        /// <summary>
        /// 見積No
        /// </summary>
        /// <create>T.Nakata 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public string EstimateNo
        {
            get { return this._estimateno; }
            set { this._estimateno = value; }
        }

        #endregion

        #region 工事識別管理NO
        /// --------------------------------------------------
        /// <summary>
        /// 工事識別管理NO
        /// </summary>
        /// <create>T.Nakata 2018/12/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public string KojiNo
        {
            get { return this._kojino; }
            set { this._kojino = value; }
        }

        #endregion

        #region 荷受CD
        /// --------------------------------------------------
        /// <summary>
        /// 荷受CD
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ConsignCd
        {
            get { return this._consigncd; }
            set { this._consigncd = value; }
        }

        #endregion

        #region 配送先CD
        /// --------------------------------------------------
        /// <summary>
        /// 配送先CD
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public string DeliverCd
        {
            get { return this._delivercd; }
            set { this._delivercd = value; }
        }

        #endregion

        #region 運送会社CD
        /// --------------------------------------------------
        /// <summary>
        /// 運送会社CD
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public string UnsokaishaNo
        {
            get { return this._unsokaishano; }
            set { this._unsokaishano = value; }
        }

        #endregion

        #region 内部見積番号
        /// --------------------------------------------------
        /// <summary>
        /// 内部見積番号
        /// </summary>
        /// <create>T.Nakata 2018/12/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public string InternalPoNo
        {
            get { return this._internalpono; }
            set { this._internalpono = value; }
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
    }
}
