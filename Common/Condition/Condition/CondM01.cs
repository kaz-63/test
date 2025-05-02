using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;

namespace Condition
{
    /// --------------------------------------------------
    /// <summary>
    /// M01用コンディション
    /// </summary>
    /// <create>Y.Higuchi 2010/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public class CondM01 : CondBase
    {
        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// ユーザーコード
        /// </summary>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _userID = null;
        /// --------------------------------------------------
        /// <summary>
        /// ユーザー名
        /// </summary>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _userName = null;
        /// --------------------------------------------------
        /// <summary>
        /// 梱包No.
        /// </summary>
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _konpoNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// 状態区分
        /// </summary>
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _jyotaiFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>Y.Higuchi 2010/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _shukkaFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// 納入先名称
        /// </summary>
        /// <create>Y.Higuchi 2010/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _nonyusakiName = null;
        /// --------------------------------------------------
        /// <summary>
        /// 便
        /// </summary>
        /// <create>Y.Higuchi 2010/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _ship = null;
        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>Y.Higuchi 2010/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _nonyusakiCD = null;
        /// --------------------------------------------------
        /// <summary>
        /// 権限ID
        /// </summary>
        /// <create>Y.Higuchi 2010/09/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _roleID = null;
        /// --------------------------------------------------
        /// <summary>
        /// パレットNo
        /// </summary>
        /// <create>M.Tsutsumi 2011/03/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _palletNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// BoxNo
        /// </summary>
        /// <create>M.Tsutsumi 2011/03/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _boxNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// TagNo
        /// </summary>
        /// <create>M.Tsutsumi 2011/03/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _tagNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// 物件管理No
        /// </summary>
        /// <create>K.Saeki 2012/03/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _bukkenNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// 物件名称
        /// </summary>
        /// <create>K.Saeki 2012/03/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _bukkenName = null;
        /// --------------------------------------------------
        /// <summary>
        /// 文字列:梱包
        /// </summary>
        /// <create>D.Okumura 2018/10/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _textKonpo = null;
        /// --------------------------------------------------
        /// <summary>
        /// 文字列:登録
        /// </summary>
        /// <create>D.Okumura 2018/10/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _textTouroku = null;
        /// --------------------------------------------------
        /// <summary>
        /// 品名(和文)
        /// </summary>
        /// <create>S.Furugo 2018/10/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _partNameJa = null;
        /// --------------------------------------------------
        /// <summary>
        /// 型式
        /// </summary>
        /// <create>S.Furugo 2018/10/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _type = null;
        /// --------------------------------------------------
        /// <summary>
        /// 品名
        /// </summary>
        /// <create>S.Furugo 2018/10/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _partName = null;
        /// --------------------------------------------------
        /// <summary>
        /// 国内/国外フラグ
        /// </summary>
        /// <create>T.nakata 2018/11/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _kokunaigaiFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// 運送会社名
        /// </summary>
        /// <create>T.nakata 2018/11/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _unsokaishaName = null;
        /// --------------------------------------------------
        /// <summary>
        /// 運送会社CD
        /// </summary>
        /// <create>T.Nakata 2018/11/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _unsoKaishaNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// 期
        /// </summary>
        /// <create>H.Tsuji 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _ecsQuota = null;
        /// --------------------------------------------------
        /// <summary>
        /// ECSNo
        /// </summary>
        /// <create>H.Tsuji 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _ecsNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// 製番
        /// </summary>
        /// <create>H.Tsuji 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _seiban = null;
        /// --------------------------------------------------
        /// <summary>
        /// CODE
        /// </summary>
        /// <create>H.Tsuji 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _code = null;
        /// --------------------------------------------------
        /// <summary>
        /// ARNo
        /// </summary>
        /// <create>H.Tsuji 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _arNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// 機種
        /// </summary>
        /// <create>H.Tsuji 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _kishu = null;
        /// --------------------------------------------------
        /// <summary>
        /// 管理区分
        /// </summary>
        /// <create>H.Tsuji 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _kanriFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// 荷受CD
        /// </summary>
        /// <create>H.Tsuji 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _consignCD = null;
        /// --------------------------------------------------
        /// <summary>
        /// 荷受名称
        /// </summary>
        /// <create>H.Tsuji 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _consignName = null;
        /// --------------------------------------------------
        /// <summary>
        /// 中国向けフラグ
        /// </summary>
        /// <create>H.Tsuji 2018/12/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _chinaFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// 配送先CD
        /// </summary>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _deliverCD = null;
        /// --------------------------------------------------
        /// <summary>
        /// 配送先名称
        /// </summary>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _deliverName = null;
        /// --------------------------------------------------
        /// <summary>
        /// 荷姿表送信対象フラグ
        /// </summary>
        /// <create>H.Tsuji 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _mailPackingFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// ProjectNo
        /// </summary>
        /// <create>H.Tsuji 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _projectNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// TAG連携送信対象フラグ
        /// </summary>
        /// <create>H.Tajimi 2019/08/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _mailTagRenkeiFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// 社内外フラグ
        /// </summary>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _shanaigaiFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷元CD
        /// </summary>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _shipFromCd = null;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷元名称
        /// </summary>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _shipFromName = null;
        /// --------------------------------------------------
        /// <summary>
        /// 非表示フラグ
        /// </summary>
        /// <create>H.Tajimi 2020/03/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _unusedFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// 受注No
        /// </summary>
        /// <create>J.Chen 2022/09/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _juchuNo = null;
        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>K.Tsutsumi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondM01()
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
        public CondM01(LoginInfo target)
            : base(target)
        {
        }

        #endregion

        #region ユーザーコード

        /// --------------------------------------------------
        /// <summary>
        /// ユーザーコード
        /// </summary>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string UserID
        {
            get { return this._userID; }
            set { this._userID = value; }
        }

        #endregion

        #region ユーザー名

        /// --------------------------------------------------
        /// <summary>
        /// ユーザー名
        /// </summary>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string UserName
        {
            get { return this._userName; }
            set { this._userName = value; }
        }

        #endregion

        #region 梱包No.

        /// --------------------------------------------------
        /// <summary>
        /// 梱包No.
        /// </summary>
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public string KonpoNo
        {
            get { return this._konpoNo; }
            set { this._konpoNo = value; }
        }

        #endregion

        #region 状態区分

        /// --------------------------------------------------
        /// <summary>
        /// 状態区分
        /// </summary>
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public string JyotaiFlag
        {
            get { return this._jyotaiFlag; }
            set { this._jyotaiFlag = value; }
        }

        #endregion

        #region 出荷区分

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>Y.Higuchi 2010/08/27</create>
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
        /// <create>Y.Higuchi 2010/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public string NonyusakiCD
        {
            get { return this._nonyusakiCD; }
            set { this._nonyusakiCD = value; }
        }

        #endregion
        
        #region 納入先名称

        /// --------------------------------------------------
        /// <summary>
        /// 納入先名称
        /// </summary>
        /// <create>Y.Higuchi 2010/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public string NonyusakiName
        {
            get { return this._nonyusakiName; }
            set { this._nonyusakiName = value; }
        }

        #endregion

        #region 便

        /// --------------------------------------------------
        /// <summary>
        /// 便
        /// </summary>
        /// <create>Y.Higuchi 2010/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Ship
        {
            get { return this._ship; }
            set { this._ship = value; }
        }

        #endregion

        #region 権限ID

        /// --------------------------------------------------
        /// <summary>
        /// 権限ID
        /// </summary>
        /// <create>Y.Higuchi 2010/09/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public string RoleID
        {
            get { return this._roleID; }
            set { this._roleID = value; }
        }

        #endregion

        #region パレットNo

        /// --------------------------------------------------
        /// <summary>
        /// パレットNo
        /// </summary>
        /// <create>M.Tsutsumi 2011/03/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public string PalletNo
        {
            get
            {
                return this._palletNo;
            }
            set
            {
                this._palletNo = value;
            }
        }

        #endregion

        #region BoxNo

        /// --------------------------------------------------
        /// <summary>
        /// BoxNo
        /// </summary>
        /// <create>M.Tsutsumi 2011/03/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public string BoxNo
        {
            get
            {
                return this._boxNo;
            }
            set
            {
                this._boxNo = value;
            }
        }

        #endregion

        #region TagNo

        /// --------------------------------------------------
        /// <summary>
        /// TagNo
        /// </summary>
        /// <create>M.Tsutsumi 2011/03/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TagNo
        {
            get
            {
                return this._tagNo;
            }
            set
            {
                this._tagNo = value;
            }
        }

        #endregion

        #region 物件管理No

        /// --------------------------------------------------
        /// <summary>
        /// 物件管理No
        /// </summary>
        /// <create>K.Saeki 2012/03/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public string BukkenNo
        {
            get { return this._bukkenNo; }
            set { this._bukkenNo = value; }
        }

        #endregion

        #region 物件名称

        /// --------------------------------------------------
        /// <summary>
        /// 物件名称
        /// </summary>
        /// <create>K.Saeki 2012/03/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public string BukkenName
        {
            get { return this._bukkenName; }
            set { this._bukkenName = value; }
        }

        #endregion

        #region 選択グループCD

        /// --------------------------------------------------
        /// <summary>
        /// 選択グループCD
        /// </summary>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public string SelectGroupCD { get; set; }

        #endregion

        #region 選択肢名称

        /// --------------------------------------------------
        /// <summary>
        /// 選択肢名称
        /// </summary>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ItemName { get; set; }

        #endregion

        #region メール状態

        /// --------------------------------------------------
        /// <summary>
        /// メール状態
        /// </summary>
        /// <create>R.Katsuo 2017/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public string MailStatus { get; set; }

        #endregion

        #region 日付(From)

        /// --------------------------------------------------
        /// <summary>
        /// 日付(From)
        /// </summary>
        /// <create>R.Katsuo 2017/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public DateTime DateFrom { get; set; }

        #endregion

        #region 日付(To)

        /// --------------------------------------------------
        /// <summary>
        /// 日付(To)
        /// </summary>
        /// <create>R.Katsuo 2017/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public DateTime DateTo { get; set; }

        #endregion

        #region リスト区分

        /// --------------------------------------------------
        /// <summary>
        /// リスト区分
        /// </summary>
        /// <create>T.Sakiori 2017/09/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ListFlag { get; set; }

        #endregion

        #region 文字列：梱包
        /// --------------------------------------------------
        /// <summary>
        /// 文字列:梱包 (梱包情報保守のときのみ使用)
        /// </summary>
        /// <create>D.Okumura 2018/10/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TextKonpo
        {
            get { return this._textKonpo; }
            set { this._textKonpo = value; }
        }
        #endregion

        #region 文字列：梱包
        /// --------------------------------------------------
        /// <summary>
        /// 文字列:登録 (梱包情報保守のときのみ使用)
        /// </summary>
        /// <create>D.Okumura 2018/10/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TextTouroku
        {
            get { return this._textTouroku; }
            set { this._textTouroku = value; }
        }
        #endregion

        #region 品名(和文)
        /// --------------------------------------------------
        /// <summary>
        /// 品名(和文)
        /// </summary>
        /// <create>S.Furugo 2018/10/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public string PartNameJa
        {
            get { return this._partNameJa; }
            set { this._partNameJa = value; }
        }
        #endregion

        #region 型式
        /// --------------------------------------------------
        /// <summary>
        /// 型式
        /// </summary>
        /// <create>S.Furugo 2018/10/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Type
        {
            get { return this._type; }
            set { this._type = value; }
        }
        #endregion

        #region 品名
        /// --------------------------------------------------
        /// <summary>
        /// 品名
        /// </summary>
        /// <create>S.Furugo 2018/10/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public string PartName
        {
            get { return this._partName; }
            set { this._partName = value; }
        }
        #endregion

        #region 国内/国外フラグ
        /// --------------------------------------------------
        /// <summary>
        /// 国内/国外フラグ
        /// </summary>
        /// <create>T.nakata 2018/11/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public string KokunaigaiFlag
        {
            get { return this._kokunaigaiFlag; }
            set { this._kokunaigaiFlag = value; }
        }
        #endregion

        #region 運送会社名
        /// --------------------------------------------------
        /// <summary>
        /// 運送会社名
        /// </summary>
        /// <create>T.nakata 2018/11/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public string UnsokaishaName
        {
            get { return this._unsokaishaName; }
            set { this._unsokaishaName = value; }
        }
        #endregion

        #region 運送会社CD
        /// --------------------------------------------------
        /// <summary>
        /// 運送会社CD
        /// </summary>
        /// <create>T.Nakata 2018/11/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public string UnsoKaishaNo
        {
            get { return this._unsoKaishaNo; }
            set { this._unsoKaishaNo = value; }
        }
        #endregion

        #region 期
        /// --------------------------------------------------
        /// <summary>
        /// 期
        /// </summary>
        /// <create>H.Tsuji 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public string EcsQuota
        {
            get { return this._ecsQuota; }
            set { this._ecsQuota = value; }
        }
        #endregion

        #region EcsNo.
        /// --------------------------------------------------
        /// <summary>
        /// EcsNo.
        /// </summary>
        /// <create>H.Tsuji 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public string EcsNo
        {
            get { return this._ecsNo; }
            set { this._ecsNo = value; }
        }
        #endregion

        #region 製番
        /// --------------------------------------------------
        /// <summary>
        /// 製番
        /// </summary>
        /// <create>H.Tsuji 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Seiban
        {
            get { return this._seiban; }
            set { this._seiban = value; }
        }
        #endregion

        #region CODE
        /// --------------------------------------------------
        /// <summary>
        /// CODE
        /// </summary>
        /// <create>H.Tsuji 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Code
        {
            get { return this._code; }
            set { this._code = value; }
        }
        #endregion

        #region ARNo
        /// --------------------------------------------------
        /// <summary>
        /// ARNo
        /// </summary>
        /// <create>H.Tsuji 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ARNo
        {
            get { return this._arNo; }
            set { this._arNo = value; }
        }
        #endregion

        #region 機種
        /// --------------------------------------------------
        /// <summary>
        /// 機種
        /// </summary>
        /// <create>H.Tsuji 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Kishu
        {
            get { return this._kishu; }
            set { this._kishu = value; }
        }
        #endregion

        #region 管理区分
        /// --------------------------------------------------
        /// <summary>
        /// 管理区分
        /// </summary>
        /// <create>H.Tsuji 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public string KanriFlag
        {
            get { return this._kanriFlag; }
            set { this._kanriFlag = value; }
        }
        #endregion

        #region 荷受CD
        /// --------------------------------------------------
        /// <summary>
        /// 荷受CD
        /// </summary>
        /// <create>H.Tsuji 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ConsignCD
        {
            get { return this._consignCD; }
            set { this._consignCD = value; }
        }
        #endregion

        #region 荷受名称
        /// --------------------------------------------------
        /// <summary>
        /// 荷受名称
        /// </summary>
        /// <create>H.Tsuji 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ConsignName
        {
            get { return this._consignName; }
            set { this._consignName = value; }
        }
        #endregion

        #region 中国向けフラグ
        /// --------------------------------------------------
        /// <summary>
        /// 中国向けフラグ
        /// </summary>
        /// <create>H.Tsuji 2018/12/06</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ChinaFlag
        {
            get { return this._chinaFlag; }
            set { this._chinaFlag = value; }
        }
        #endregion

        #region 配送先CD
        /// --------------------------------------------------
        /// <summary>
        /// 配送先CD
        /// </summary>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public string DeliverCD
        {
            get { return this._deliverCD; }
            set { this._deliverCD = value; }
        }
        #endregion

        #region 配送先名称
        /// --------------------------------------------------
        /// <summary>
        /// 配送先名称
        /// </summary>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public string DeliverName
        {
            get { return this._deliverName; }
            set { this._deliverName = value; }
        }
        #endregion

        #region 荷姿表送信対象フラグ
        /// --------------------------------------------------
        /// <summary>
        /// 荷姿表送信対象フラグ
        /// </summary>
        /// <create>H.Tsuji 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public string MailPackingFlag
        {
            get { return this._mailPackingFlag; }
            set { this._mailPackingFlag = value; }
        }
        #endregion

        #region ProjectNo
        /// --------------------------------------------------
        /// <summary>
        /// ProjectNo
        /// </summary>
        /// <create>H.Tsuji 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ProjectNo
        {
            get { return this._projectNo; }
            set { this._projectNo = value; }
        }
        #endregion

        #region TAG連携送信対象フラグ
        /// --------------------------------------------------
        /// <summary>
        /// TAG連携送信対象フラグ
        /// </summary>
        /// <create>H.Tajimi 2019/08/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public string MailTagRenkeiFlag
        {
            get { return this._mailTagRenkeiFlag; }
            set { this._mailTagRenkeiFlag = value; }
        }
        #endregion

        #region 社内外フラグ

        /// --------------------------------------------------
        /// <summary>
        /// 社内外フラグ
        /// </summary>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShanaigaiFlag
        {
            get { return this._shanaigaiFlag; }
            set { this._shanaigaiFlag = value; }
        }

        #endregion

        #region 出荷元コード

        /// --------------------------------------------------
        /// <summary>
        /// 出荷元コード
        /// </summary>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShipFromCd
        {
            get { return this._shipFromCd; }
            set { this._shipFromCd = value; }
        }

        #endregion

        #region 出荷元名称

        /// --------------------------------------------------
        /// <summary>
        /// 出荷元名称
        /// </summary>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShipFromName
        {
            get { return this._shipFromName; }
            set { this._shipFromName = value; }
        }

        #endregion

        #region 未使用フラグ

        /// --------------------------------------------------
        /// <summary>
        /// 未使用フラグ
        /// </summary>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public string UnusedFlag
        {
            get { return this._unusedFlag; }
            set { this._unusedFlag = value; }
        }

        #endregion

        #region 受注No
        /// --------------------------------------------------
        /// <summary>
        /// 受注No
        /// </summary>
        /// <create>J.Chen 2022/09/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public string JuchuNo
        {
            get { return this._juchuNo; }
            set { this._juchuNo = value; }
        }
        #endregion

    }
}
