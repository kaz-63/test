using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;


namespace Condition
{
    /// --------------------------------------------------
    /// <summary>
    /// S01用コンディション
    /// </summary>
    /// <create>Y.Higuchi 2010/07/08</create>
    /// <update>K.Tsutsumi 2012/04/24</update>
    /// <update>K.Tsutsumi 2012/05/07</update>
    /// <update>J.Chen 2022/05/25 STEP14</update>
    /// <update>R.Miyoshi 2023/07/14 物件名/荷受先追加</update>
    /// <update>T.SASAYAMA 2023/07/28 運賃梱包製番追加</update>
    /// --------------------------------------------------
    public class CondS01 : CondBase
    {
        #region Fields

        // 入力登録フラグ
        private bool _modeInsert = false;
        // Excel登録フラグ
        private bool _modeExcel = false;
        // 変更フラグ
        private bool _modeUpdate = false;
        // 削除フラグ
        private bool _modeDelete = false;
        // 照会フラグ
        private bool _modeView = false;
        // 出荷区分
        private string _shukkaFlag = null;
        // 納入先コード
        private string _nonyusakiCD = null;
        // 出荷便
        private string _ship = null;
        // AR No.
        private string _arNo = null;
        // 表示選択
        private string _dispSelect = null;
        // 2012/04/24 K.Tsutsumi Add TagNo自動採番
        // 物件管理No
        private string _bukkenNO = null;
        // ↑
        // 2012/05/07 K.Tsutsumi Add 履歴
        // 操作区分
        private string _operationFlag = null;
        // 更新PC名
        private string _updatePCName = null;
        // ↑
        // 製番+CODE(カンマ区切りで複数)
        private string _sibanCodeList = null;
        // ECS No(カンマ区切りで複数)
        private string _ecsNoList = null;
        // 有償/無償フラグ
        private string _estimateFlag = null;
        // ARフラグ
        private bool _isAR = false;
        // 国内外フラグ
        private string _kokunaigaiFlag = null;
        // 出荷日
        private string _shipDate = null;
        // 選択グループCD
        private string _selectGroupCD = null;
        // グループコード
        private string _groupCD = null;
        // 図番/型式
        private string _zumenKeishiki = null;
        // 非表示フラグ
        private string _unusedFlag = null;
        // 出荷元CD
        private string _shipFromCD = null;
        // 出荷元
        private string _shipFrom = null;
        // 荷姿並び順
        private string _nisugataSortOrder = null;
        // 出荷先
        private string _shipTo = null;
        // 物件名
        private string _bukkenName = null;
        //荷受CD
        private string _consignCD = null;
        // 荷受先名称
        private string _consignName = null;
        // 運賃梱包製番
        private string _shipSeiban = null;
        // 物件Rev
        private string _bukkenRev = null;
        // アサインコメント
        private string _assignComment = null;
        // 出荷日（START）
        private string _shipDateStart = null;
        // 出荷日（END）
        private string _shipDateEnd = null;
        // 製番
        private string _seiban = null;
        // 機種
        private string _kishu = null;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>K.Tsutsumi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public CondS01()
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
        public CondS01(LoginInfo target)
            : base(target)
        {
        }

        #endregion

        #region 入力登録フラグ

        /// --------------------------------------------------
        /// <summary>
        /// 入力登録フラグ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool ModeInsert
        {
            get { return this._modeInsert; }
            set { this._modeInsert = value; }
        }

        #endregion

        #region Excel登録フラグ

        /// --------------------------------------------------
        /// <summary>
        /// Excel登録フラグ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool ModeExcel
        {
            get { return this._modeExcel; }
            set { this._modeExcel = value; }
        }

        #endregion

        #region 更新フラグ

        /// --------------------------------------------------
        /// <summary>
        /// 更新フラグ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool ModeUpdate
        {
            get { return this._modeUpdate; }
            set { this._modeUpdate = value; }
        }

        #endregion

        #region 削除フラグ

        /// --------------------------------------------------
        /// <summary>
        /// 削除フラグ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool ModeDelete
        {
            get { return this._modeDelete; }
            set { this._modeDelete = value; }
        }

        #endregion

        #region 照会フラグ

        /// --------------------------------------------------
        /// <summary>
        /// 照会フラグ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool ModeView
        {
            get { return this._modeView; }
            set { this._modeView = value; }
        }

        #endregion

        #region 出荷区分

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>Y.Higuchi 2010/07/08</create>
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
        /// <create>Y.Higuchi 2010/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public string NonyusakiCD
        {
            get { return this._nonyusakiCD; }
            set { this._nonyusakiCD = value; }
        }

        #endregion

        #region 出荷便

        /// --------------------------------------------------
        /// <summary>
        /// 出荷便
        /// </summary>
        /// <create>Y.Higuchi 2010/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Ship
        {
            get { return this._ship; }
            set { this._ship = value; }
        }

        #endregion

        #region AR No.

        /// --------------------------------------------------
        /// <summary>
        /// AR No.
        /// </summary>
        /// <create>Y.Higuchi 2010/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ARNo
        {
            get { return this._arNo; }
            set { this._arNo = value; }
        }

        #endregion

        #region 表示選択

        /// --------------------------------------------------
        /// <summary>
        /// 表示選択
        /// </summary>
        /// <create>Y.Higuchi 2010/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public string DispSelect
        {
            get { return this._dispSelect; }
            set { this._dispSelect = value; }
        }

        #endregion

        #region 物件管理No

        /// --------------------------------------------------
        /// <summary>
        /// 物件管理No
        /// </summary>
        /// <create>K.Tsutsumi 2012/04/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public string BukkenNO
        {
            get { return this._bukkenNO; }
            set { this._bukkenNO = value; }
        }

        #endregion

        #region 操作区分
        /// --------------------------------------------------
        /// <summary>
        /// 操作区分
        /// </summary>
        /// <create>K.Tsutsumi 2012/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public string OperationFlag
        {
            get { return this._operationFlag; }
            set { this._operationFlag = value; }
        }
        #endregion

        #region 更新PC名
        /// --------------------------------------------------
        /// <summary>
        /// 更新PC名
        /// </summary>
        /// <create>K.Tsutsumi 2012/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public string UpdatePCName
        {
            get { return this._updatePCName; }
            set { this._updatePCName = value; }
        }
        #endregion

        #region 削除パレットNo

        /// --------------------------------------------------
        /// <summary>
        /// 削除パレットNo
        /// </summary>
        /// <create>T.Wakamatsu 2016/01/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public string RemovePalletNo { get; set; }

        #endregion

        #region 削除ボックスNo

        /// --------------------------------------------------
        /// <summary>
        /// 削除ボックスNo
        /// </summary>
        /// <create>T.Wakamatsu 2016/01/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public string RemoveBoxNo { get; set; }

        #endregion

        #region 移動元工事識別管理No

        /// --------------------------------------------------
        /// <summary>
        /// 移動元工事識別管理No
        /// </summary>
        /// <create>T.Wakamatsu 2016/01/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public string KojiNoOrig { get; set; }

        #endregion

        #region 移動元木枠梱包内部管理キー

        /// --------------------------------------------------
        /// <summary>
        /// 移動元木枠梱包内部管理キー
        /// </summary>
        /// <create>T.Wakamatsu 2016/01/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public string CaseIdOrig { get; set; }

        #endregion

        #region 移動元納入先コード

        /// --------------------------------------------------
        /// <summary>
        /// 移動元納入先コード
        /// </summary>
        /// <create>T.Wakamatsu 2016/01/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public string NonyusakiCDOrig { get; set; }

        #endregion

        #region 移動先納入先コード

        /// --------------------------------------------------
        /// <summary>
        /// 移動先納入先コード
        /// </summary>
        /// <create>T.Wakamatsu 2016/01/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public string NonyusakiCDDest { get; set; }

        #endregion

        #region 移動先工事識別管理No

        /// --------------------------------------------------
        /// <summary>
        /// 移動先工事識別管理No
        /// </summary>
        /// <create>T.Wakamatsu 2016/01/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public string KojiNoDest { get; set; }

        #endregion

        #region 移動先木枠梱包内部管理キー

        /// --------------------------------------------------
        /// <summary>
        /// 移動先木枠梱包内部管理キー
        /// </summary>
        /// <create>T.Wakamatsu 2016/01/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public string CaseIdDest { get; set; }

        #endregion

        #region パレットNo

        /// --------------------------------------------------
        /// <summary>
        /// パレットNo
        /// </summary>
        /// <create>T.Wakamatsu 2016/01/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public string PalletNo { get; set; }

        #endregion

        #region ボックスNo

        /// --------------------------------------------------
        /// <summary>
        /// ボックスNo
        /// </summary>
        /// <create>T.Wakamatsu 2016/01/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public string BoxNo { get; set; }

        #endregion

        #region タグNo

        /// --------------------------------------------------
        /// <summary>
        /// タグNo
        /// </summary>
        /// <create>T.Wakamatsu 2016/01/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TagNo { get; set; }

        #endregion

        #region 更新単位

        /// --------------------------------------------------
        /// <summary>
        /// 更新単位
        /// </summary>
        /// <create>T.Wakamatsu 2016/01/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public string UpdateTani { get; set; }

        #endregion

        #region C/NO

        /// --------------------------------------------------
        /// <summary>
        /// C/NO
        /// </summary>
        /// <create>T.Wakamatsu 2016/03/31</create>
        /// <update></update>
        /// --------------------------------------------------
        public string CaseNo { get; set; }

        #endregion

        #region Print C/NO

        /// --------------------------------------------------
        /// <summary>
        /// Print C/NO
        /// </summary>
        /// <create>T.Wakamatsu 2016/03/31</create>
        /// <update></update>
        /// --------------------------------------------------
        public string PrintCaseNo { get; set; }

        #endregion

        #region 移動対象木枠梱包内部管理キー

        /// --------------------------------------------------
        /// <summary>
        /// 移動対象木枠梱包内部管理キー
        /// </summary>
        /// <create>T.Wakamatsu 2016/04/06</create>
        /// <update></update>
        /// --------------------------------------------------
        public string CaseId { get; set; }

        #endregion

        #region 製番+CODE(カンマ区切りで複数)

        /// --------------------------------------------------
        /// <summary>
        /// 製番+CODE(カンマ区切りで複数)
        /// </summary>
        /// <create>T.Nakata 2018/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public string SibanCodeList
        {
            get { return this._sibanCodeList; }
            set { this._sibanCodeList = value; }
        }

        #endregion

        #region ECS No(カンマ区切りで複数)

        /// --------------------------------------------------
        /// <summary>
        /// ECS No(カンマ区切りで複数)
        /// </summary>
        /// <create>T.Nakata 2018/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public string EcsNoList
        {
            get { return this._ecsNoList; }
            set { this._ecsNoList = value; }
        }

        #endregion

        #region 有償/無償フラグ

        /// --------------------------------------------------
        /// <summary>
        /// 有償/無償フラグ
        /// </summary>
        /// <create>T.Nakata 2018/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public string EstimateFlag
        {
            get { return this._estimateFlag; }
            set { this._estimateFlag = value; }
        }

        #endregion

        #region 国内外フラグ

        /// --------------------------------------------------
        /// <summary>
        /// 国内外フラグ
        /// </summary>
        /// <create>T.Nakata 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public string KokunaigaiFlag
        {
            get { return this._kokunaigaiFlag; }
            set { this._kokunaigaiFlag = value; }
        }

        #endregion

        #region 出荷日

        /// --------------------------------------------------
        /// <summary>
        /// 出荷日
        /// </summary>
        /// <create>T.Nakata 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShipDate
        {
            get { return this._shipDate; }
            set { this._shipDate = value; }
        }

        #endregion

        #region 選択グループCD

        /// --------------------------------------------------
        /// <summary>
        /// 選択グループCD
        /// </summary>
        /// <create>T.Nakata 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public string SelectGroupCD
        {
            get { return this._selectGroupCD; }
            set { this._selectGroupCD = value; }
        }

        #endregion

        #region グループコード

        /// --------------------------------------------------
        /// <summary>
        /// グループコード
        /// </summary>
        /// <create>T.Nakata 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public string GroupCD
        {
            get { return this._groupCD; }
            set { this._groupCD = value; }
        }

        #endregion

        #region 図番/形式

        /// --------------------------------------------------
        /// <summary>
        /// 図番/形式
        /// </summary>
        /// <create>H.Tajimi 2019/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ZumenKeishiki
        {
            get { return this._zumenKeishiki; }
            set { this._zumenKeishiki = value; }
        }

        #endregion

        #region 表示区分

        /// --------------------------------------------------
        /// <summary>
        /// 表示区分
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

        #region 出荷元CD

        /// --------------------------------------------------
        /// <summary>
        /// 出荷元CD
        /// </summary>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShipFromCD
        {
            get { return this._shipFromCD; }
            set { this._shipFromCD = value; }
        }
        
        #endregion

        #region 出荷元CD

        /// --------------------------------------------------
        /// <summary>
        /// 出荷元CD
        /// </summary>
        /// <create>J.Chen 2023/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShipFrom
        {
            get { return this._shipFrom; }
            set { this._shipFrom = value; }
        }

        #endregion

        #region 荷姿表並び順

        /// --------------------------------------------------
        /// <summary>
        /// 荷姿表並び順
        /// </summary>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public string NisugataSortOrder
        {
            get { return this._nisugataSortOrder; }
            set { this._nisugataSortOrder = value; }
        }

        #endregion

        #region 出荷先

        /// --------------------------------------------------
        /// <summary>
        /// 出荷先
        /// </summary>
        /// <create>J.Chen 2022/05/25 STEP14</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShipTo
        {
            get { return this._shipTo; }
            set { this._shipTo = value; }
        }

        #endregion

        #region 荷受CD

        /// --------------------------------------------------
        /// <summary>
        /// 荷受CD
        /// </summary>
        /// <create>Y.Gwon 2023/08/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ConsignCD
        {
            get { return this._consignCD; }
            set { this._consignCD = value; }
        }

        #endregion

        #region 物件名

        /// --------------------------------------------------
        /// <summary>
        /// 物件名
        /// </summary>
        /// <create>R.Miyoshi 2023/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public string BukkenName
        {
            get { return this._bukkenName; }
            set { this._bukkenName = value; }
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

        #region 運賃梱包製番

        /// --------------------------------------------------
        /// <summary>
        /// 運賃梱包製番
        /// </summary>
        /// <create>T.SASAYAMA 2023/07/28</create>
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

        #region 物件Rev

        /// --------------------------------------------------
        /// <summary>
        /// 物件Rev
        /// </summary>
        /// <create>T.SASAYAMA 2023/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public string BukkenRev
        {
            get
            {
                return this._bukkenRev;
            }
            set
            {
                this._bukkenRev = value;
            }
        }

        #endregion

        #region アサインコメント

        /// --------------------------------------------------
        /// <summary>
        /// アサインコメント
        /// </summary>
        /// <create>T.SASAYAMA 2023/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public string AssignComment
        {
            get
            {
                return this._assignComment;
            }
            set
            {
                this._assignComment = value;
            }
        }
        #endregion

        #region 出荷日（START）

        /// --------------------------------------------------
        /// <summary>
        /// 出荷日（START）
        /// </summary>
        /// <create>J.Chen 2023/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShipDateStart
        {
            get { return this._shipDateStart; }
            set { this._shipDateStart = value; }
        }

        #endregion

        #region 出荷日（END）

        /// --------------------------------------------------
        /// <summary>
        /// 出荷日（END）
        /// </summary>
        /// <create>J.Chen 2023/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShipDateEnd
        {
            get { return this._shipDateEnd; }
            set { this._shipDateEnd = value; }
        }

        #endregion

        #region 製番

        /// --------------------------------------------------
        /// <summary>
        /// 製番
        /// </summary>
        /// <create>J.Chen 2023/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Seiban
        {
            get { return this._seiban; }
            set { this._seiban = value; }
        }

        #endregion

        #region 機種

        /// --------------------------------------------------
        /// <summary>
        /// 機種
        /// </summary>
        /// <create>J.Chen 2023/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Kishu
        {
            get { return this._kishu; }
            set { this._kishu = value; }
        }

        #endregion
    }
}
