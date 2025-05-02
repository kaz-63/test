using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;

namespace Condition
{
    /// --------------------------------------------------
    /// <summary>
    /// T01用コンディション
    /// </summary>
    /// <create>S.Furugo 2018/10/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public class CondT01 : CondBase
    {

        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 期
        /// </summary>
        /// <create>S.Furugo 2018/10/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _ecsQuota = null;
        /// --------------------------------------------------
        /// <summary>
        /// ECS No.
        /// </summary>
        /// <create>S.Furugo 2018/10/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _ecsNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// 連携No.
        /// </summary>
        /// <create>S.Furugo 2018/11/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _tehaiRenkeiNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// 設定納期範囲
        /// </summary>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _assyRange = null;
        /// --------------------------------------------------
        /// <summary>
        /// 設定納期
        /// </summary>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _setteiDate = null;
        /// --------------------------------------------------
        /// <summary>
        /// 物件名
        /// </summary>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _bukkenName = null;
        /// --------------------------------------------------
        /// <summary>
        /// 製番
        /// </summary>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _seiban = null;
        /// --------------------------------------------------
        /// <summary>
        /// CODE
        /// </summary>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _code = null;
        /// --------------------------------------------------
        /// <summary>
        /// AR No.
        /// </summary>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _arNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _shukkaFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// 納品先
        /// </summary>
        /// <create>S.Furugo 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _nouhinsaki = null;
        /// --------------------------------------------------
        /// <summary>
        /// 手配No.
        /// </summary>
        /// <create>S.Furugo 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _tehaiNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// プロジェクトNo.
        /// </summary>
        /// <create>S.Furugo 2018/11/2</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _projectNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷先
        /// </summary>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _shukkasaki = null;
        /// --------------------------------------------------
        /// <summary>
        /// 手配区分
        /// </summary>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _tehaikubun = null;
        /// --------------------------------------------------
        /// <summary>
        /// 有償/無償
        /// </summary>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _yusho = null;
        /// --------------------------------------------------
        /// <summary>
        /// 見積状況
        /// </summary>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _mitsumorizyokyo = null;
        /// --------------------------------------------------
        /// <summary>
        /// 見積No.
        /// </summary>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _mitsumoriNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// PO No.
        /// </summary>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _poNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// 選択グループコード
        /// </summary>
        /// <create>D.Naito 2018/11/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _selectGroupCode = null;
        /// --------------------------------------------------
        /// <summary>
        /// 品名(和文)
        /// </summary>
        /// <create>D.Naito 2018/11/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _hinmeiJp = null;
        /// --------------------------------------------------
        /// <summary>
        /// 品名
        /// </summary>
        /// <create>H.Tsuji 2019/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _hinmei = null;
        /// --------------------------------------------------
        /// <summary>
        /// 図番/型式
        /// </summary>
        /// <create>D.Naito 2018/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _partsCode = null;
        /// --------------------------------------------------
        /// <summary>
        /// 追番
        /// </summary>
        /// <create>S.Furugo 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _oiban = null;
        /// --------------------------------------------------
        /// <summary>
        /// 入荷状況
        /// </summary>
        /// <create>S.Furugo 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _nyukaFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// 組立状況
        /// </summary>
        /// <create>S.Furugo 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _assyFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// TAG登録状況
        /// </summary>
        /// <create>S.Furugo 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _tagTourokuFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// SKS連携対象
        /// </summary>
        /// <create>H.Tajimi 2019/01/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _tehaiSKSRenkeiFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// 図番型式
        /// </summary>
        /// <create>H.Tajimi 2019/01/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _zumenKeishiki = null;
        /// --------------------------------------------------
        /// <summary>
        /// 図番型式2
        /// </summary>
        /// <create>H.Tsuji 2019/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _zumenKeishiki2 = null;
        /// --------------------------------------------------
        /// <summary>
        /// 手配種別
        /// </summary>
        /// <create>H.Tajimi 2019/01/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _tehaiKindFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// 便
        /// </summary>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update>K.Tsutsumi 2019/09/07 複数対応</update>
        /// --------------------------------------------------
        private string[] _nonyusakiCDs = null;
        /// --------------------------------------------------
        /// <summary>
        /// 木枠便
        /// </summary>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _kiwakuShip = null;
        /// --------------------------------------------------
        /// <summary>
        /// CaseNo
        /// </summary>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _caseNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// Tag No.
        /// </summary>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _tagNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// Box No.
        /// </summary>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _boxNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// Pallet No.
        /// </summary>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _palletNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// 表示選択
        /// </summary>
        /// <create>K.Tsutsumi 2019/09/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _dispSelect = null;
        /// --------------------------------------------------
        /// <summary>
        /// 返却品フラグ
        /// </summary>
        /// <create>T.Nukaga 2019/11/18 STEP12 返却品管理対応</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _henkykakuhinFlag = null;

        /// --------------------------------------------------
        /// <summary>
        /// 検索モード
        /// </summary>
        /// <create>K.Tsutsumi 2020/05/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _tehaiNyukaSearchMode = null;

        /// --------------------------------------------------
        /// <summary>
        /// Assy No
        /// </summary>
        /// <create>Y.Shioshi 2022/05/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _assyNo = null;

        /// --------------------------------------------------
        /// <summary>
        /// Estimate Mode
        /// </summary>
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _estimateMode = null;

        /// --------------------------------------------------
        /// <summary>
        /// 登録日時Start
        /// </summary>
        /// <create>T.Zhou 2023/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _createDateStart = null;

        /// --------------------------------------------------
        /// <summary>
        /// 登録日時End
        /// </summary>
        /// <create>T.Zhou 2023/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _createDateEnd = null;

        /// --------------------------------------------------
        /// <summary>
        /// 設定納期Start
        /// </summary>
        /// <create>T.Zhou 2023/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _setteiDateStart = null;

        /// --------------------------------------------------
        /// <summary>
        /// 設定納期End
        /// </summary>
        /// <create>T.Zhou 2023/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _setteiDateEnd = null;

        /// --------------------------------------------------
        /// <summary>
        /// 履歴更新フラグ
        /// </summary>
        /// <create>J.Chen 2024/10/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _rirekiUpdate = false;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>S.Furugo 2018/10/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondT01()
            : this(new LoginInfo())
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target">ログインユーザー情報</param>
        /// <create>S.Furugo 2018/10/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public CondT01(LoginInfo target)
            : base(target)
        {
        }

        #endregion

        #region ECS No.
        
        /// --------------------------------------------------
        /// <summary>
        /// ECS No.
        /// </summary>
        /// <create>S.Furugo 2018/10/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public string EcsNo
        {
            get { return this._ecsNo; }
            set { this._ecsNo = value; }
        }

        #endregion

        #region 期

        /// --------------------------------------------------
        /// <summary>
        /// 期
        /// </summary>
        /// <create>S.Furugo 2018/10/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public string EcsQuota
        {
            get { return this._ecsQuota; }
            set { this._ecsQuota = value; }
        }

        #endregion

        #region 連携No.

        /// --------------------------------------------------
        /// <summary>
        /// 連携No.
        /// </summary>
        /// <create>S.Furugo 2018/11/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TehaiRenkeiNo
        {
            get { return this._tehaiRenkeiNo; }
            set { this._tehaiRenkeiNo = value; }
        }

        #endregion

        #region 設定納期範囲

        /// --------------------------------------------------
        /// <summary>
        /// 設定納期範囲
        /// </summary>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public string AssyRange
        {
            get { return this._assyRange; }
            set { this._assyRange = value; }
        }

        #endregion

        #region 設定納期

        /// --------------------------------------------------
        /// <summary>
        /// 設定納期
        /// </summary>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public string SetteiDate
        {
            get { return this._setteiDate; }
            set { this._setteiDate = value; }
        }

        #endregion

        #region 物件名

        /// --------------------------------------------------
        /// <summary>
        /// 物件名
        /// </summary>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public string BukkenName
        {
            get { return this._bukkenName; }
            set { this._bukkenName = value; }
        }

        #endregion

        #region 製番

        /// --------------------------------------------------
        /// <summary>
        /// 製番
        /// </summary>
        /// <create>H.Tajimi 2018/11/07</create>
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
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Code
        {
            get { return this._code; }
            set { this._code = value; }
        }

        #endregion

        #region AR No.

        /// --------------------------------------------------
        /// <summary>
        /// AR No.
        /// </summary>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ARNo
        {
            get { return this._arNo; }
            set { this._arNo = value; }
        }

        #endregion

        #region 出荷区分

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShukkaFlag
        {
            get { return this._shukkaFlag; }
            set { this._shukkaFlag = value; }
        }

        #endregion

        #region 納品先

        /// --------------------------------------------------
        /// <summary>
        /// 納品先
        /// </summary>
        /// <create>S.Furugo 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Nouhinsaki
        {
            get { return this._nouhinsaki; }
            set { this._nouhinsaki = value; }
        }

        #endregion

        #region 手配No.

        /// --------------------------------------------------
        /// <summary>
        /// 手配No.
        /// </summary>
        /// <create>S.Furugo 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TehaiNo
        {
            get { return this._tehaiNo; }
            set { this._tehaiNo = value; }
        }

        #endregion

        #region プロジェクトNo.

        /// --------------------------------------------------
        /// <summary>
        /// プロジェクトNo.
        /// </summary>
        /// <create>S.Furugo 2018/11/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ProjectNo
        {
            get { return this._projectNo; }
            set { this._projectNo = value; }
        }

        #endregion

        #region 出荷先

        /// --------------------------------------------------
        /// <summary>
        /// 出荷先
        /// </summary>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Shukkasaki
        {
            get { return this._shukkasaki; }
            set { this._shukkasaki = value; }
        }

        #endregion

        #region 手配区分

        /// --------------------------------------------------
        /// <summary>
        /// 手配区分
        /// </summary>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TehaiKubun
        {
            get { return this._tehaikubun; }
            set { this._tehaikubun = value; }
        }

        #endregion

        #region 有償/無償

        /// --------------------------------------------------
        /// <summary>
        /// 有償/無償
        /// </summary>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Yusho
        {
            get { return this._yusho; }
            set { this._yusho = value; }
        }

        #endregion

        #region 見積状況

        /// --------------------------------------------------
        /// <summary>
        /// 見積状況
        /// </summary>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Mitsumorizyokyo
        {
            get { return this._mitsumorizyokyo; }
            set { this._mitsumorizyokyo = value; }
        }

        #endregion

        #region 見積No.

        /// --------------------------------------------------
        /// <summary>
        /// 見積No.
        /// </summary>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public string MitsumoriNo
        {
            get { return this._mitsumoriNo; }
            set { this._mitsumoriNo = value; }
        }

        #endregion

        #region PO No.

        /// --------------------------------------------------
        /// <summary>
        /// PO No.
        /// </summary>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public string PONo
        {
            get { return this._poNo; }
            set { this._poNo = value; }
        }

        #endregion

        #region 選択グループコード

        /// --------------------------------------------------
        /// <summary>
        /// 選択グループコード
        /// </summary>
        /// <create>D.Naito 2018/11/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string SelectGroupCode
        {
            get { return this._selectGroupCode; }
            set { this._selectGroupCode = value; }
        }

        #endregion

        #region 品名(和文)

        /// --------------------------------------------------
        /// <summary>
        /// 品名(和文)
        /// </summary>
        /// <create>D.Naito 2018/11/30</create>
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
        /// <create>H.Tsuji 2019/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Hinmei
        {
            get { return this._hinmei; }
            set { this._hinmei = value; }
        }

        #endregion

        #region 図番/型式

        /// --------------------------------------------------
        /// <summary>
        /// 図番/型式
        /// </summary>
        /// <create>D.Naito 2018/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public string PartsCode
        {
            get { return this._partsCode; }
            set { this._partsCode = value; }
        }

        #endregion

        #region 追番

        /// --------------------------------------------------
        /// <summary>
        /// 追番
        /// </summary>
        /// <create>S.Furugo 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Oiban
        {
            get { return this._oiban; }
            set { this._oiban = value; }
        }

        #endregion

        #region 入荷状況

        /// --------------------------------------------------
        /// <summary>
        /// 入荷状況
        /// </summary>
        /// <create>S.Furugo 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public string NyukaFlag
        {
            get { return this._nyukaFlag; }
            set { this._nyukaFlag = value; }
        }

        #endregion

        #region 組立状況

        /// --------------------------------------------------
        /// <summary>
        /// 組立状況
        /// </summary>
        /// <create>S.Furugo 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public string AssyFlag
        {
            get { return this._assyFlag; }
            set { this._assyFlag = value; }
        }

        #endregion

        #region TAG登録状況

        /// --------------------------------------------------
        /// <summary>
        /// TAG登録状況
        /// </summary>
        /// <create>S.Furugo 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TagTourokuFlag
        {
            get { return this._tagTourokuFlag; }
            set { this._tagTourokuFlag = value; }
        }

        #endregion

        #region SKS連携対象

        /// --------------------------------------------------
        /// <summary>
        /// SKS連携対象
        /// </summary>
        /// <create>H.Tajimi 2019/01/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TehaiSKSRenkeiFlag
        {
            get { return this._tehaiSKSRenkeiFlag; }
            set { this._tehaiSKSRenkeiFlag = value; }
        }

        #endregion

        #region 図番型式

        /// --------------------------------------------------
        /// <summary>
        /// 図番型式
        /// </summary>
        /// <create>H.Tajimi 2019/01/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ZumenKeishiki
        {
            get { return this._zumenKeishiki; }
            set { this._zumenKeishiki = value; }
        }

        #endregion

        #region 図番型式2

        /// --------------------------------------------------
        /// <summary>
        /// 図番型式2
        /// </summary>
        /// <create>H.Tsuji 2019/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ZumenKeishiki2
        {
            get { return this._zumenKeishiki2; }
            set { this._zumenKeishiki2 = value; }
        }

        #endregion

        #region 手配種別

        /// --------------------------------------------------
        /// <summary>
        /// 手配種別
        /// </summary>
        /// <create>H.Tajimi 2019/01/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TehaiKindFlag
        {
            get { return this._tehaiKindFlag; }
            set { this._tehaiKindFlag = value; }
        }

        #endregion

        #region 便

        /// --------------------------------------------------
        /// <summary>
        /// 便
        /// </summary>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update>K.Tsutsumi 2019/09/07 複数対応</update>
        /// --------------------------------------------------
        public string[] NonyusakiCDs
        {
            get { return this._nonyusakiCDs; }
            set { this._nonyusakiCDs = value; }
        }

        #endregion

        #region CaseNo

        /// --------------------------------------------------
        /// <summary>
        /// CaseNo
        /// </summary>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public string CaseNo
        {
            get { return this._caseNo; }
            set { this._caseNo = value; }
        }

        #endregion

        #region Tag No

        /// --------------------------------------------------
        /// <summary>
        /// Tag No.
        /// </summary>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TagNo
        {
            get { return this._tagNo; }
            set { this._tagNo = value; }
        }

        #endregion

        #region Box No

        /// --------------------------------------------------
        /// <summary>
        /// Box No.
        /// </summary>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public string BoxNo
        {
            get { return this._boxNo; }
            set { this._boxNo = value; }
        }

        #endregion

        #region Pallet No

        /// --------------------------------------------------
        /// <summary>
        /// Pallet No.
        /// </summary>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public string PalletNo
        {
            get { return this._palletNo; }
            set { this._palletNo = value; }
        }

        #endregion

        #region 木枠便

        /// --------------------------------------------------
        /// <summary>
        /// 木枠便
        /// </summary>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public string KiwakuShip
        {
            get { return this._kiwakuShip; }
            set { this._kiwakuShip = value; }
        }

        #endregion

        #region 表示選択

        /// --------------------------------------------------
        /// <summary>
        /// 表示選択
        /// </summary>
        /// <create>K.Tsutsumi 2019/09/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public string DispSelect
        {
            get { return this._dispSelect; }
            set { this._dispSelect = value; }
        }

        #endregion

        #region 返却品

        /// --------------------------------------------------
        /// <summary>
        /// 返却品フラグ
        /// </summary>
        /// <create>T.Nukaga 2019/11/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public string HenkyakuhinFlag
        {
            get { return this._henkykakuhinFlag; }
            set { this._henkykakuhinFlag = value; }
        }

        #endregion

        #region 検索モード

        /// --------------------------------------------------
        /// <summary>
        /// 検索モード
        /// </summary>
        /// <create>K.Tsutsumi 2020/05/31</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TehaiNyukaSearchMode
        {
            get { return this._tehaiNyukaSearchMode; }
            set { this._tehaiNyukaSearchMode = value; }
        }

        #endregion

        #region Assy No

        /// --------------------------------------------------
        /// <summary>
        /// Assy No
        /// </summary>
        /// <create>Y.Shioshi 2022/05/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public string AssyNo
        {
            get { return this._assyNo; }
            set { this._assyNo = value; }
        }

        #endregion

        #region Estimate Mode

        /// --------------------------------------------------
        /// <summary>
        /// Estimate Mode
        /// </summary>
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Estimate_Mode
        {
            get { return this._estimateMode; }
            set { this._estimateMode = value; }
        }

        #endregion

        #region 登録日時Start

        /// --------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <create>T.Zhou 2023/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public string CreateDateStart
        {
            get { return this._createDateStart; }
            set { this._createDateStart = value; }
        }

        #endregion

        #region 登録日時End

        /// --------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <create>T.Zhou 2023/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public string CreateDateEnd
        {
            get { return this._createDateEnd; }
            set { this._createDateEnd = value; }
        }

        #endregion

        #region 設定納期Start

        /// --------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <create>T.Zhou 2023/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public string SetteiDateStart
        {
            get { return this._setteiDateStart; }
            set { this._setteiDateStart = value; }
        }

        #endregion

        #region 設定納期End

        /// --------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <create>T.Zhou 2023/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public string SetteiDateEnd
        {
            get { return this._setteiDateEnd; }
            set { this._setteiDateEnd = value; }
        }

        #endregion

        #region 履歴更新フラグ

        /// --------------------------------------------------
        /// <summary>
        /// 履歴更新フラグ
        /// </summary>
        /// <create>J.Chen 2024/10/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool RirekiUpdate
        {
            get { return this._rirekiUpdate; }
            set { this._rirekiUpdate = value; }
        }

        #endregion

    }
}
