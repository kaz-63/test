using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;

namespace Condition
{
    /// --------------------------------------------------
    /// <summary>
    /// K02コンディション
    /// </summary>
    /// <create>H.Tsunamura 2010/06/28</create>
    /// <update></update>
    /// --------------------------------------------------
    [System.Xml.Serialization.SoapType()]
    [System.Serializable()]
    public class CondK02 : CondBase
    {
        #region Fields

        // 採番区分
        private string _saibanFlag;
        // 個数
        private int _count;
        // 印刷フラグ
        private bool _isPreview = false;
        // 工事識別NO
        private string _kojiName;
        // 便
        private string _ship = null;
        // 発行選択
        private string _hakkoSelect;
        // 登録区分
        private string _torokuFlag = null;
        // 工事識別NO
        private string _kojiNo = null;
        // 内部管理用キー
        private string _caseID = null;
        // 作業区分
        private string _sagyoFlag = null;
        // パレットNo.
        private string _palletNo = null;
        // BOXNO.
        private string _boxNo = null;
        // 出荷区分
        private string _shukkaFlag = null;
        // 納入先コード
        private string _nonyusakiCD = null;
        // Tag No.
        private string _tagNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// AR No.
        /// </summary>
        /// <create>Y.Higuchi 2010/09/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _arNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// 木枠梱包明細登録時に自動で便間移動実施フラグ
        /// </summary>
        /// <create>H.Tajimi 2019/01/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _kiwakuKonpoMoveShipFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// 木枠登録時の納入先コード
        /// </summary>
        /// <create>H.Tajimi 2019/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _kiwakuNonyusakiCd = null;
        /// --------------------------------------------------
        /// <summary>
        /// 木枠登録時の登録種別
        /// </summary>
        /// <create>H.Tajimi 2019/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _kiwakuInsertType = null;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>K.Tsutsumi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondK02()
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
        public CondK02(LoginInfo target)
            : base(target)
        {
        }

        #endregion

        #region 採番区分

        /// --------------------------------------------------
        /// <summary>
        /// 採番区分
        /// </summary>
        /// <create>H.Tsunamura 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public string SaibanFlag
        {
            get { return this._saibanFlag; }
            set { this._saibanFlag = value; }
        }

        #endregion

        #region 個数

        /// --------------------------------------------------
        /// <summary>
        /// BOXラベル・パレットラベルの取得数
        /// </summary>
        /// <create>H.Tsunamura 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public int Count
        {
            get { return this._count; }
            set { this._count = value; }
        }

        #endregion

        #region 個数

        /// --------------------------------------------------
        /// <summary>
        /// BOXラベル・パレットラベルの印刷フラグ(true:プレビュー false:印刷)
        /// </summary>
        /// <create>H.Tsunamura 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool IsPreview
        {
            get { return this._isPreview; }
            set { this._isPreview = value; }
        }

        #endregion

        #region 工事識別名称

        /// --------------------------------------------------
        /// <summary>
        /// 工事識別名
        /// </summary>
        /// <create>H.Tsunamura 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public string KojiName
        {
            get { return this._kojiName; }
            set { this._kojiName = value; }
        }

        #endregion

        #region 便

        /// --------------------------------------------------
        /// <summary>
        /// 便
        /// </summary>
        /// <create>H.Tsunamura 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Ship
        {
            get { return this._ship; }
            set { this._ship = value; }
        }

        #endregion

        #region 発行選択

        /// --------------------------------------------------
        /// <summary>
        /// 発行選択
        /// </summary>
        /// <create>H.Tsunamura 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public string HakkoSelect
        {
            get { return this._hakkoSelect; }
            set { this._hakkoSelect = value; }
        }

        #endregion

        #region 登録区分

        /// --------------------------------------------------
        /// <summary>
        /// 登録区分
        /// </summary>
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TorokuFlag
        {
            get { return this._torokuFlag; }
            set { this._torokuFlag = value; }
        }

        #endregion

        #region 工事識別NO

        /// --------------------------------------------------
        /// <summary>
        /// 工事識別NO
        /// </summary>
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public string KojiNo
        {
            get { return this._kojiNo; }
            set { this._kojiNo = value; }
        }

        #endregion

        #region 内部管理用キー

        /// --------------------------------------------------
        /// <summary>
        /// 内部管理用キー
        /// </summary>
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public string CaseID
        {
            get { return this._caseID; }
            set { this._caseID = value; }
        }

        #endregion

        #region 作業区分

        /// --------------------------------------------------
        /// <summary>
        /// 作業区分
        /// </summary>
        /// <create>Y.Higuchi 2010/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public string SagyoFlag
        {
            get { return this._sagyoFlag; }
            set { this._sagyoFlag = value; }
        }

        #endregion

        #region BOXNo.

        /// --------------------------------------------------
        /// <summary>
        /// BOXNo.
        /// </summary>
        /// <create>H.Tsunamura 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public string BoxNo
        {
            get { return this._boxNo; }
            set { this._boxNo = value; }
        }

        #endregion

        #region パレットNo.

        /// --------------------------------------------------
        /// <summary>
        /// パレットNo.
        /// </summary>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public string PalletNo
        {
            get { return this._palletNo; }
            set { this._palletNo = value; }
        }

        #endregion

        #region 出荷区分

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>Y.Higuchi 2010/08/02</create>
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
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public string NonyusakiCD
        {
            get { return this._nonyusakiCD; }
            set { this._nonyusakiCD = value; }
        }

        #endregion

        #region Tag No.

        /// --------------------------------------------------
        /// <summary>
        /// Tag No.
        /// </summary>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TagNo
        {
            get { return this._tagNo; }
            set { this._tagNo = value; }
        }

        #endregion

        #region AR No.

        /// --------------------------------------------------
        /// <summary>
        /// AR No.
        /// </summary>
        /// <create>Y.Higuchi 2010/09/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ARNo
        {
            get { return this._arNo; }
            set { this._arNo = value; }
        }

        #endregion

        #region 木枠梱包明細登録時に自動で便間移動実施フラグ

        /// --------------------------------------------------
        /// <summary>
        /// 木枠梱包明細登録時に自動で便間移動実施フラグ
        /// </summary>
        /// <create>H.Tajimi 2019/01/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public string KiwakuKonpoMoveShipFlag
        {
            get { return this._kiwakuKonpoMoveShipFlag; }
            set { this._kiwakuKonpoMoveShipFlag = value; }
        }

        #endregion

        #region 木枠登録時の納入先コード

        /// --------------------------------------------------
        /// <summary>
        /// 木枠登録時の納入先コード
        /// </summary>
        /// <create>H.Tajimi 2019/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public string KiwakuNonyusakiCD
        {
            get { return this._kiwakuNonyusakiCd; }
            set { this._kiwakuNonyusakiCd = value; }
        }

        #endregion

        #region 木枠登録時の登録種別

        /// --------------------------------------------------
        /// <summary>
        /// 木枠登録時の登録種別
        /// </summary>
        /// <create>H.Tajimi 2019/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public string KiwakuInsertType
        {
            get { return this._kiwakuInsertType; }
            set { this._kiwakuInsertType = value; }
        }

        #endregion
    }
}
