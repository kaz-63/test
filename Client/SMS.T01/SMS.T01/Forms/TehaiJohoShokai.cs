using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SystemBase.Forms;

using DSWUtil;
using DSWControl.DSWComboBox;
using WsConnection;
using WsConnection.WebRefCommon;
using Commons;
using SystemBase.Controls;
using GrapeCity.Win.ElTabelle;
using ElTabelleHelper;
using SMS.T01.Properties;
using SMS.E01;
using SMS.S01;
using SystemBase.Util;
using WsConnection.WebRefT01;
using SMS.P02.Forms;
using WsConnection.WebRefAttachFile;

namespace SMS.T01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 手配情報照会
    /// </summary>
    /// <create>H.Tajimi 2019/08/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class TehaiJohoShokai : SystemBase.Forms.CustomOrderForm
    {
        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// 画面表示モード
        /// </summary>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DisplayMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 初期
            /// </summary>
            /// <create>H.Tajimi 2019/08/28</create>
            /// <update></update>
            /// --------------------------------------------------
            Initialize,
            /// --------------------------------------------------
            /// <summary>
            /// 検索後
            /// </summary>
            /// <create>H.Tajimi 2019/08/28</create>
            /// <update></update>
            /// --------------------------------------------------
            EndSearch,
        }

        #endregion

        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_TOPLEFT_COL = 0;

        /// --------------------------------------------------
        /// <summary>
        /// 列定義
        /// </summary>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update>K.Tsutsumi 2019/09/07 写真表示</update>
        /// <update>T.Nukaga 2019/11/18 返却品対応、32に返却品を追加し以降の番号を1繰り下げ</update>
        /// <update>2022/04/19 STEP14</update>
        /// <update>Y.Shioshi 2022/05/09 STEP14列追加対応、追加列以降の番号を1繰り下げ</update>
        /// <update>H.Iimuro 2022/10/04 図面/型式２の並び替え対応</update>
        /// <update>J.Chen 2024/10/29 変更履歴追加</update>
        /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
        /// --------------------------------------------------
        private const int SHEET_TEHAI_RENKEI_NO = 0;
        private const int SHEET_BUKKEN_NAME = 1;
        private const int SHEET_SETTEI_DATE = 2;
        private const int SHEET_NONYU_JYOTAI = 3;
        private const int SHEET_NOHIN_SAKI = 4;
        private const int SHEET_SYUKKA_SAKI = 5;
        private const int SHEET_SEIBAN = 6;
        private const int SHEET_CODE = 7;
        private const int SHEET_SHIP_AR = 8;
        private const int SHEET_ECS_NO = 9;
        private const int SHEET_ZUMEN_OIBAN = 10;

        private const int SHEET_FLOOR = 11;
        private const int SHEET_KISHU = 12;
        private const int SHEET_ST_NO = 13;
        private const int SHEET_HINMEI_JP = 14;
        private const int SHEET_HINMEI = 15;
        private const int SHEET_HINMEI_INV = 16;
        private const int SHEET_ZUMEN_KEISHIKI = 17;
        private const int SHEET_ZUMEN_KEISHIKI2 = 18;
        private const int SHEET_KUWARI_NO = 19;
        private const int SHEET_TEHAI_QTY = 20;
        private const int SHEET_FREE1 = 21;

        private const int SHEET_FREE2 = 22;
        private const int SHEET_GRWT = 23;
        private const int SHEET_QUANTITY_UNIT_NAME = 24;
        private const int SHEET_NOTE1 = 25;
        private const int SHEET_NOTE2 = 26;
        private const int SHEET_NOTE3 = 27;
        private const int SHEET_CUSTOMS_STATUS = 28;
        private const int SHEET_MAKER = 29;
        private const int SHEET_UNIT_PRICE = 30;
        private const int SHEET_TEHAI_NO = 31;

        private const int SHEET_TEHAI_FLAG_NAME = 32;
        private const int SHEET_HENKYAKUHIN_FLAG_NAME = 33;
        private const int SHEET_TEHAI_NYUKA_FLAG_NAME = 34;
        private const int SHEET_TEHAI_ASSY_FLAG_NAME = 35;
        private const int SHEET_TEHAI_TAG_TOUROKU_FLAG_NAME = 3;
        private const int SHEET_TEHAI_SYUKKA_FLAG_NAME = 37;
        private const int SHEET_ESTIMATE_FLAG_NAME = 38;
        private const int SHEET_PO_NO = 39;
        private const int SHEET_JYOTAI_NAME = 40;
        private const int SHEET_SHUKKA_DATE = 41;
        private const int SHEET_TAG_NO = 42;

        private const int SHEET_AREA = 43;
        private const int SHEET_M_NO = 44;
        private const int SHEET_SHUKA_DATE = 45;
        private const int SHEET_BOX_NO = 46;
        private const int SHEET_BOXKONPO_DATE = 47;
        private const int SHEET_PALLET_NO = 48;
        private const int SHEET_PALLETKONPO_DATE = 49;
        private const int SHEET_SHIP = 50;
        private const int SHEET_CASE_NO = 51;
        private const int SHEET_KIWAKUKONPO_DATE = 52;

        private const int SHEET_UNSOKAISHA_NAME = 53;
        private const int SHEET_INVOICE_NO = 54;
        private const int SHEET_OKURIJYO_NO = 55;
        private const int SHEET_BL_NO = 56;
        private const int SHEET_UKEIRE_DATE = 57;
        private const int SHEET_UKEIRE_USER_NAME = 58;
        private const int SHEET_ECS_QUOTA = 59;
        private const int SHEET_EXISTS_PICTURE = 60;
        private const int SHEET_TEHAI_RIREKI = 61;
        private const int SHEET_CREATE_USER_NAME = 62;
        private const int SHEET_CREATE_DATE = 63;

        private const int SHEET_TEHAI_NYUKA_FLAG = 74;

        private const int SHEET_COL_FILE_NAME1 = 75;
        private const int SHEET_COL_SAVE_DIR1 = 76;
        private const int SHEET_COL_FILE_NAME2 = 77;
        private const int SHEET_COL_SAVE_DIR2 = 78;

        private const int SHEET_TAG_TOUROKU_MAX = 79;
        private const int SHEET_HENKYAKUHIN_FLAG = 80;
        private const int SHEET_COLOR04 = 81;
        private const int SHEET_TEHAI_FLAG = 82;
        private const int SHEET_ASSY_OYA_ZAN = 83;


        


        /// --------------------------------------------------
        /// <summary>
        /// 物件名のデフォルト値
        /// </summary>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update>T.Nukaga 2019/11/08 デフォルト値を全て(AR)から全て(AR未出荷)に変更</update>
        /// <update>2022/04/21 デフォルト値を全て(AR未出荷)から全てに変更</update>
        /// --------------------------------------------------
        private const string BUKKEN_NAME_DEFAULT_VALUE = ComDefine.COMBO_ALL_VALUE;

        /// --------------------------------------------------
        /// <summary>
        /// タブ選択のデフォルト値
        /// </summary>
        /// <create>T.Nukaga 2019/11/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int TAB_DEFAULT_SELECTED_INDEX = 0;

        #endregion

        #region フィールド

        /// --------------------------------------------------
        /// <summary>
        /// AutoFilter対象列
        /// </summary>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update>T.Nukaga 2019/11/18 返却品管理対応 返却品列追加</update>
        /// <update>R.Sumi 2022/02/24 STEP14 要望No.1対応　全項目にプルダウン追加</update>
        /// <update>Y.Shioshi 2022/05/19 STEP14列追加対応</update>
        /// <update>H.Iimuro 2022/10/04 図面/型式２の並び替え対応</update>
        /// <update>J.Chen 2024/10/29 変更履歴追加</update>
        /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
        /// --------------------------------------------------
        private int[] _autoFilterColumns = new int[] {
            SHEET_TEHAI_RENKEI_NO, SHEET_BUKKEN_NAME, SHEET_SETTEI_DATE, SHEET_NONYU_JYOTAI, SHEET_NOHIN_SAKI, SHEET_SYUKKA_SAKI, 
            SHEET_SEIBAN, SHEET_CODE, SHEET_SHIP_AR, SHEET_ECS_NO, SHEET_ZUMEN_OIBAN, SHEET_FLOOR, SHEET_KISHU, SHEET_ST_NO,
            SHEET_HINMEI_JP, SHEET_HINMEI, SHEET_HINMEI_INV, SHEET_ZUMEN_KEISHIKI, SHEET_ZUMEN_KEISHIKI2, SHEET_KUWARI_NO, SHEET_TEHAI_QTY,
            SHEET_FREE1, SHEET_FREE2, SHEET_GRWT, SHEET_QUANTITY_UNIT_NAME, SHEET_NOTE1, SHEET_NOTE2, 
            SHEET_NOTE3, SHEET_CUSTOMS_STATUS, SHEET_MAKER, SHEET_UNIT_PRICE, SHEET_TEHAI_NO, SHEET_TEHAI_FLAG_NAME, SHEET_HENKYAKUHIN_FLAG_NAME,
            SHEET_TEHAI_NYUKA_FLAG_NAME, SHEET_TEHAI_ASSY_FLAG_NAME, SHEET_TEHAI_TAG_TOUROKU_FLAG_NAME, SHEET_TEHAI_SYUKKA_FLAG_NAME, 
            SHEET_ESTIMATE_FLAG_NAME, SHEET_PO_NO, SHEET_JYOTAI_NAME, SHEET_SHUKKA_DATE, SHEET_TAG_NO, SHEET_AREA, SHEET_M_NO, 
            SHEET_SHUKA_DATE, SHEET_BOX_NO, SHEET_BOXKONPO_DATE, SHEET_PALLET_NO, SHEET_PALLETKONPO_DATE, SHEET_SHIP,  
            SHEET_CASE_NO, SHEET_KIWAKUKONPO_DATE, SHEET_UNSOKAISHA_NAME, SHEET_INVOICE_NO, SHEET_OKURIJYO_NO,
            SHEET_BL_NO, SHEET_UKEIRE_DATE, SHEET_UKEIRE_USER_NAME, SHEET_ECS_QUOTA, SHEET_EXISTS_PICTURE, SHEET_TEHAI_RIREKI,
            SHEET_CREATE_USER_NAME, SHEET_CREATE_DATE,
            SHEET_NOHIN_SAKI,SHEET_SYUKKA_SAKI,SHEET_SEIBAN,SHEET_CODE,SHEET_SHIP_AR,SHEET_ECS_NO,SHEET_SHIP,
            SHEET_ZUMEN_OIBAN,SHEET_TEHAI_FLAG_NAME,SHEET_ESTIMATE_FLAG_NAME, SHEET_INVOICE_NO,SHEET_ECS_QUOTA
        };
        /// --------------------------------------------------
        /// <summary>
        /// データ絞込用クラス
        /// </summary>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataFilter _filter = new DataFilter();
        /// --------------------------------------------------
        /// <summary>
        /// フィルターを再設定するかどうか
        /// </summary>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _needsSetFilter = false;
        /// --------------------------------------------------
        /// <summary>
        /// アイドル発生待ちになったかどうか
        /// </summary>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _idleStart = false;
        /// --------------------------------------------------
        /// <summary>
        /// 一覧選択で選択した納入先コード（複数）
        /// </summary>
        /// <create>K.Tsutsumi 2019/09/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private string[] _nonyusakiCDs = null;
        /// --------------------------------------------------
        /// <summary>
        /// Iniファイルのセクション名プレフィックス
        /// </summary>
        /// <create>H.Tsuji 2019/09/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private readonly string _iniSectionPrefix = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// オブジェクト変数
        /// </summary>
        /// <create>R.Sumi 2022/02/28</create>
        /// --------------------------------------------------
        private static TehaiJohoShokai _tehaiJohoShokaForm;


        //【Step_1_1】手配情報照会画面の権限制御 2022/10/13（TW-Tsuji）
        //
        //　今回作成した、メニューの権限チェッククラス（MenuAuthorityCheck）を
        //　メンバ変数として定義する.
        protected SystemBase.MenuAuthorityCheck _menuAuthCheck;
        //
        //　単価（JPY）列について、表示しない様にしたか否かのフラグ。
        //　Excel出力時に利用する.
        protected bool _flgUnitPriceDisable = false;

        /// --------------------------------------------------
        /// <summary>
        /// 更新比較用dt
        /// </summary>
        /// <create>J.Chen 2024/10/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _tempDt = null;

        /// --------------------------------------------------
        /// <summary>
        /// 編集可能かどうか
        /// </summary>
        /// <create>J.Chen 2024/12/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isEditable = false;


        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="menuCategoryID">メニュー種別ID</param>
        /// <param name="menuItemID">メニュー項目ID</param>
        /// <param name="title">画面タイトル</param>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update>H.Tsuji 2019/07/29 グリッド幅のユーザー設定</update>
        /// --------------------------------------------------
        public TehaiJohoShokai(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
            : base(userInfo, menuCategoryID, menuItemID, title)
        {
            InitializeComponent();
            this._iniSectionPrefix = this.GetType().FullName;

            //【Step_1_1】手配情報照会画面の権限制御 2022/10/13（TW-Tsuji）
            //
            //　今回作成した、メニューの権限チェッククラス（MenuAuthorityCheck）を
            //　インスタンス化.
            this._menuAuthCheck = new SystemBase.MenuAuthorityCheck(userInfo);

        }
        #endregion


        #region ゲッター/セッター
        /// --------------------------------------------------
        /// <summary>
        /// インスタンス
        /// </summary>
        /// <create>R.Sumi 2022/02/28</create>
        /// --------------------------------------------------
        public static TehaiJohoShokai TehaiJohoShokaiInstance
        {
            get
            {
                return _tehaiJohoShokaForm;
            }
            set
            {
                _tehaiJohoShokaForm = value;
            }
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update>T.Zhou 2023/12/01 登録日時、設定納期の初期化メソッドを呼ぶ追加</update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                this.InitializeComboBox();
                this.InitializeComboDate();
                this.DisplayClear();
                this.ChangeMode(DisplayMode.Initialize);
                // シートの初期化
                this.InitializeSheet(this.shtMeisai);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// シートの初期化
        /// </summary>
        /// <param name="sheet">シートオブジェクト</param>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update>H.Tsuji 2019/09/09 グリッド幅のユーザー設定</update>
        /// <update>T.Nukaga 2019/11/18 返却品管理対応</update>
        /// <update>2022/04/19 STEP14</update>
        /// <update>Y.Shioshi 2022/05/09 STEP14 列追加対応</update>
        /// <update>H.Iimuro 2022/10/04 図面/型式２の並び替え対応</update>
        /// <update>T.Zhou 2023/12/07 登録日時、設定納期の初期化メソッド追加</update>
        /// <update>J.Chen 2024/10/29 変更履歴追加</update>
        /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
        /// --------------------------------------------------
        protected override void InitializeSheet(Sheet sheet)
        {
            base.InitializeSheet(sheet);
            try
            {
                // El TabelleのAutoFilterは行の高さを0とすることでフィルタリング状態を作り出しており
                // ユーザ操作により行の高さ変更が行われると、フィルタリング状態と表示データの不整合が
                // 発生する可能性があるため、ユーザ操作による行の高さ変更を不可とする
                this.shtMeisai.RowHeaders.AllowResize = false;

                // シートのタイトルを設定
                int colIndex = 0;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_CooperationNo;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_BukkenName;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_RequestedDue;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_DeliveryState;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_DeliveryDestination;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_ShippingDestination;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_ProductNo;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_Code;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_ShipARNo;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_ECSNo;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_DrawingAdditionalNo;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_Floor;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_Model;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_STNo;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_JPName;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_Name;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_InvName;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_PartNo;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_PartNo2;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_SectioningNo;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_ArrangementQty;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_Free1;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_Free2;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_GWRT;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_QuantityUnitName;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_Note1;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_Note2;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_Note3;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_CustomsStatus;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_Maker;

                //【Step_1_1】手配情報照会画面の権限制御 2022/10/13（TW-Tsuji）
                //
                //　単価（JPY）列について、以下のユーザは参照できない様、制御する.
                //　　　以下の権限（M_ROLE）を想定
                //　　　　管理部User        ROLE_ID=008 土山倉庫
                //　　　　パートナー管理者  ROLE_ID=012 パートナー管理者
                //　
                //　　　メニュー識別ID: T01　　 （手配）
                //　    メニュー項目ID: T0190030（手配情報照会の4桁目 '9' で決め打ち）
                if (this._menuAuthCheck.ExistsRoleAndRolemap("T01", "T0190030") == true)
                {
                    sheet.Columns[colIndex].DataField = "";     //データフィールドプロパティをクリアして単価（JPY）を非表示
                    _flgUnitPriceDisable = true;                //フラグをONにしておく（Excel出力時に参照）
                    this.shtMeisai.Columns[colIndex].DataField = "";

                }
                //---修正ここまで

                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_JPYUnitPrice;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_TehaiNo;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_TehaiFlagName;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_HenkyakuhinFlagName;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_StockSituation;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_AssemblySituation;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_TagRegistrationSituation;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_ShippingSituation;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_Compensation;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_PONo;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_State;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_ShippingDate;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_TagNo;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_Area;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_MNo;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_ShukaDate;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_BoxNo;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_BoxPackingDate;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_PalletNo;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_PalletPackingDate;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_WoodFrameShip;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_CNo;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_WoodFramePackingDate;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_ShippingCompany;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_InvoiceNo;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_OkurijyoNo;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_BLNo;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_AcceptanceDate;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_AcceptanceStaff;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_ECSQuota;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_Picture;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_TehaiRireki;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_CreateUserName;
                sheet.ColumnHeaders[colIndex++].Caption = Resources.TehaiJohoShokai_CreateDate;

                this.SetEnableAutoFilter(false);

                // 設定ファイルからグリッド幅を読み込む
                this.LoadGridSetting(this.shtMeisai, this._iniSectionPrefix);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示されるたびにコントロールの初期化するメソッド
        /// </summary>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                this.cboBukkenName.Focus();
                
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region コンボボックスの初期化

        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスの初期化
        /// </summary>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update>K.Tsutsumi 2019/09/11 「全て(AR)」と「全て(AR未出荷)」の設定はローカル側へ移動</update>
        /// <update>T.Nukaga 2019/11/08 検索条件追加対応「全て(AR未出荷)」、「全て(AR)」の順に入れ替え</update>
        /// <update>R.Sumi 2022/02/25 検索条件の物件名に「全て」を追加</update>
        /// --------------------------------------------------
        private void InitializeComboBox()
        {
            try
            {
                var cond = new CondT01(this.UserInfo);
                var conn = new ConnT01();

                var ds = conn.GetInitTehaiJohoShokai(cond);

                // 物件名コンボボックスの初期化
                if (UtilData.ExistsData(ds, Def_M_PROJECT.Name))
                {
                    var dt = ds.Tables[Def_M_PROJECT.Name];

                    // 先頭に「全て(AR未出荷)」、「全て(AR)」追加
                    {
                        // 全ての追加
                        var dr = dt.NewRow();
                        dr[Def_M_PROJECT.PROJECT_NO] = ComDefine.COMBO_ALL_VALUE;
                        dr[Def_M_PROJECT.BUKKEN_NAME] = ComDefine.COMBO_ALL_DISP;
                        dt.Rows.InsertAt(dr, 0);
                    }
                    {
                        // 全て(AR 未出荷)の追加
                        var dr = dt.NewRow();
                        dr[Def_M_PROJECT.PROJECT_NO] = ComDefine.COMBO_ALL_MISHUKKA_AR_VALUE;
                        dr[Def_M_PROJECT.BUKKEN_NAME] = ComDefine.COMBO_ALL_MISHUKKA_AR_DISP;
                        dt.Rows.InsertAt(dr, 1);
                    }
                    {
                        // 全て(AR)の追加
                        var dr = dt.NewRow();
                        dr[Def_M_PROJECT.PROJECT_NO] = ComDefine.COMBO_ALL_AR_VALUE;
                        dr[Def_M_PROJECT.BUKKEN_NAME] = ComDefine.COMBO_ALL_AR_DISP;
                        dt.Rows.InsertAt(dr, 2);
                    }
                    dt.AcceptChanges();
                }

                this.MakeCmbBox(this.cboBukkenName, ds.Tables[Def_M_PROJECT.Name], Def_M_PROJECT.PROJECT_NO, Def_M_PROJECT.BUKKEN_NAME, BUKKEN_NAME_DEFAULT_VALUE, false);
                // 出荷区分コンボボックスの初期化
                this.MakeCmbBox(this.cboShukkaFlag, ds.Tables[SHUKKA_FLAG.GROUPCD], Def_M_COMMON.VALUE1, Def_M_COMMON.ITEM_NAME, Def_M_COMMON.DEFAULT_VALUE, true);
                // 表示選択
                this.MakeCmbBox(this.cboDispSelect, DISP_SELECT.GROUPCD);

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 日付の初期化

        /// --------------------------------------------------
        /// <summary>
        /// 日付の初期化
        /// </summary>
        /// <create>T.Zhou 2023/12/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void InitializeComboDate()
        {

            // 半年前～当日の日時を計算します
            DateTime currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            DateTime fiveMonthsAgo = currentDate.AddMonths(-5);
            DateTime firstDayOfFiveMonthsAgo = new DateTime(fiveMonthsAgo.Year, fiveMonthsAgo.Month, 1);

            // DateTimePickerコントロールのValueプロパティに設定します
            // 登録日時
            mpCreateDateStart.Value = firstDayOfFiveMonthsAgo;
            mpCreateDateEnd.Value =  currentDate;
            // 設定納期
            mpSetteiDateStart.Value = firstDayOfFiveMonthsAgo;
            mpSetteiDateEnd.Value =  currentDate;
        }

        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update>T.Zhou 2023/12/07</update>
        /// <update>J.Chen 2024/10/28 再検索時条件をクリアしないように変更</update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                if (EditMode == SystemBase.EditMode.Update) return;

                this._filter.ClearFilter();

                this.SearchConditionsClear();
                this.SheetClear();

                this.ChangeMode(DisplayMode.Initialize);

                //this.InitializeComboDate();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索条件クリア
        /// </summary>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update>D.Okumura 2019/11/25 絞り込み条件のクリア処理を追加</update>
        /// <update>T.Zhou 2023/12/01 品名、図番型式、登録日時、設定納期追加</update>
        /// --------------------------------------------------
        private void SearchConditionsClear()
        {
            try
            {
                // 物件名
                if (0 < this.cboBukkenName.Items.Count)
                {
                    this.cboBukkenName.SelectedValue = BUKKEN_NAME_DEFAULT_VALUE;
                    this.ChangeBukkenName();
                }
                else
                {
                    this.cboBukkenName.SelectedIndex = -1;
                    // 出荷区分
                    if (0 < this.cboShukkaFlag.Items.Count)
                    {
                        this.cboShukkaFlag.SelectedValue = SHUKKA_FLAG.DEFAULT_VALUE1;
                        this.ChangeShukkaFlag();
                    }
                    else
                    {
                        this.cboShukkaFlag.SelectedIndex = -1;
                    }
                }
                // 表示選択
                if (0 < this.cboDispSelect.Items.Count)
                {
                    this.cboDispSelect.SelectedValue = DISP_SELECT.DEFAULT_VALUE1;
                }
                else
                {
                    this.cboDispSelect.SelectedIndex = -1;
                }

                // 日付クリア
                // クリアする前に、日付初期化
                this.InitializeComboDate();
                this.chkCreateDate.Checked = false;
                this.chkSetteiDate.Checked = false;
                this.mpCreateDateStart.Enabled = false;
                this.mpCreateDateEnd.Enabled = false;
                this.mpSetteiDateStart.Enabled = false;
                this.mpSetteiDateEnd.Enabled = false;

                this.txtEcsQuota.Clear();
                this.txtEcsNo.Clear();
                this.txtARNo.Clear();
                this.txtHinmei.Clear();
                this.txtZumenKeishiki.Clear();
                this.chkAllShip.Checked = false;
                this.btnRefShip.Enabled = true;
                this.txtSelectShip.Clear();
                this._nonyusakiCDs = null;
                this.txtSeiban.Clear();
                this.txtCode.Clear();
                this.txtKiwakuNo.Clear();
                this.txtCaseNo.Clear();
                this.txtTagNo.Clear();
                this.txtBoxNo.Clear();
                this.txtPalletNo.Clear();

                // 進捗件数
                this.txtShuka.Text = "0/0";
                this.txtBoxKonpo.Text = "0/0";
                this.txtPalletKonpo.Text = "0/0";
                this.txtKiwakuKonpo.Text = "0/0";

                // 絞り込み条件
                foreach (var cbo in new DSWComboBox[] {
                    this.fcboARNo,
                    this.fcboBukkenName,
                    this.fcboCode,
                    this.fcboEcsNo,
                    this.fcboEcsQuota,
                    this.fcboEstimateFlag,
                    this.fcboInvoiceNo,
                    this.fcboNonyusaki,
                    this.fcboSeiban,
                    this.fcboShip,
                    this.fcboShukkaFlag,
                    this.fcboShukkasaki,
                    this.fcboTehaiFlag,
                    this.fcboZumenOiban,
                })
                {
                    cbo.Text = null;
                    cbo.SelectedIndex = -1;
                }

                this.txtDispShip.Text = string.Empty;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// シートのクリア
        /// </summary>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            this.shtMeisai.Redraw = false;
            // 最も左上に表示されているセルの設定
            if (0 < this.shtMeisai.Rows.Count)
            {
                this.shtMeisai.TopLeft = new Position(SHEET_TOPLEFT_COL, this.shtMeisai.TopLeft.Row);
            }
            this.shtMeisai.DataSource = null;
            this.shtMeisai.MaxRows = 0;
            this.shtMeisai.Redraw = true;
        }

        #endregion

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update>K.Tsutsumi 2020/01/03 メッセージ未指定を修正</update>
        /// <update>K.Tsutsumi 2020/01/03 検索条件のチェック追加</update>
        /// <update>R.Sumit 2022/02/25 STEP14 物件名に「全て」を表示</update>
        /// <update>T.Zhou 2023/12/07 検索条件追加</update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                if (this.cboBukkenName.SelectedIndex < 0)
                {
                    // 物件名を選択して下さい。
                    this.ShowMessage("T0100030006");
                    return false;
                }

                // 全てAR、全てAR(未出荷)が選択されている場合のチェック
                var projectName = this.cboBukkenName.SelectedValue.ToString();
                if ((projectName == ComDefine.COMBO_ALL_MISHUKKA_AR_VALUE) || (projectName == ComDefine.COMBO_ALL_AR_VALUE))
                {
                    // 出荷区分が選択されていない場合
                    if (this.cboShukkaFlag.SelectedIndex < 0)
                    {
                        // 出荷区分をARにする必要があります。
                        this.ShowMessage("T0100030007");
                        return false;
                    }
                    
                    var shukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
                    // AR以外が選択されていた場合
                    if (shukkaFlag != SHUKKA_FLAG.AR_VALUE1)
                    {
                        // 出荷区分をARにする必要があります。
                        this.ShowMessage("T0100030007");
                        return false;
                    }

                }

                /*
                 * 2022/02/25 STEP14_2_2
                 * 物件名が全ての場合
                 */
                if (projectName == ComDefine.COMBO_ALL_VALUE)
                {
                    //「期」と「出荷区分」が入力、選択されていない場合
                    if (this.txtEcsQuota.Text == "" || this.cboShukkaFlag.SelectedIndex == 0 || this.cboShukkaFlag.SelectedIndex == -1) 
                    {
                        //「ECS No」が入力されていない場合
                        if (this.txtEcsNo.Text == "" && this.txtSeiban.Text == "" && this.txtHinmei.Text == "" && this.txtZumenKeishiki.Text == "" && !this.chkCreateDate.Checked && !this.chkSetteiDate.Checked)
                        {
                            // 物件名が「全て」の場合、期と出荷区分、ECS No.、製番、品名、図番/型式、登録日時、または設定納期のいずれかを必ず入力してください。
                            this.ShowMessage("T0100030009");
                            return false;
                        }
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                ret = false;
            }
            return ret;
        }

        #endregion

        #region 検索処理
        /// --------------------------------------------------
        /// <summary>
        /// 検索処理制御部(表示位置復元あり)
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <param name="pos">以前の検索位置</param>
        /// <create>J.Chen 2024/10/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RunSearch(Position pos)
        {
            var result = this.RunSearch();
            if (result)
            {
                // 検索が成功し、件数が所定以上なら、位置を復元する
                if (this.shtMeisai.Rows.Count > pos.Row)
                {
                    this.shtMeisai.Enabled = true;
                    this.shtMeisai.TopLeft = pos;
                }
            }
            return result;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理実行部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update>T.Zhou 2023/12/07 検索条件追加</update>
        /// <update>J.Chen 2024/10/29 変更履歴保存時チェック用dt追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                this.shtMeisai.Redraw = false;

                var cond = new CondT01(this.UserInfo);
                var conn = new ConnT01();

                cond.ProjectNo = this.cboBukkenName.SelectedValue.ToString();
                cond.EcsQuota = this.txtEcsQuota.Text;
                cond.EcsNo = this.txtEcsNo.Text;
                cond.Hinmei = this.txtHinmei.Text;
                cond.ZumenKeishiki = this.txtZumenKeishiki.Text;

                if (chkCreateDate.Checked)
                {
                    DateTime createDateStart = this.mpCreateDateStart.Value;
                    DateTime createDateEnd = new DateTime(mpCreateDateEnd.Value.Year, mpCreateDateEnd.Value.Month, DateTime.DaysInMonth(mpCreateDateEnd.Value.Year, mpCreateDateEnd.Value.Month));


                    // 開始日が終了日より後の場合
                    if (createDateStart > createDateEnd)
                    {
                        this.ShowMessage("T0100030015");
                        return false;
                    }

                    // 範囲が一年を超えるかチェック
                    if ((createDateEnd - createDateStart).TotalDays > 365)
                    {
                        this.ShowMessage("T0100030017");
                        return false;
                    }

                    cond.CreateDateStart = createDateStart.ToString("yyyy/MM/dd");
                    cond.CreateDateEnd = createDateEnd.ToString("yyyy/MM/dd");
                }

                if (chkSetteiDate.Checked)
                {
                    DateTime setteiDateStart = this.mpSetteiDateStart.Value;
                    DateTime setteiDateEnd = new DateTime(mpSetteiDateEnd.Value.Year, mpSetteiDateEnd.Value.Month, DateTime.DaysInMonth(mpSetteiDateEnd.Value.Year, mpSetteiDateEnd.Value.Month));


                    // 開始日が終了日より後の場合
                    if (setteiDateStart > setteiDateEnd)
                    {
                        this.ShowMessage("T0100030016");
                        return false;
                    }

                    // 範囲が一年を超えるかチェック
                    if ((setteiDateEnd - setteiDateStart).TotalDays > 365)
                    {
                        this.ShowMessage("T0100030018");
                        return false;
                    }

                    cond.SetteiDateStart = setteiDateStart.ToString("yyyy/MM/dd");
                    cond.SetteiDateEnd = setteiDateEnd.ToString("yyyy/MM/dd");
                }

                if (this.cboShukkaFlag.SelectedIndex > -1 && !string.IsNullOrEmpty(this.cboShukkaFlag.Text))
                {
                    cond.ShukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
                }
                cond.ARNo = this.GetTextWithPrefix(this.txtARNo, this.lblAR.Text);
                if (this.chkAllShip.Checked)
                {
                    // 全便 = 納入先指定なし
                    cond.NonyusakiCDs = null;
                }
                else
                {
                    // 選択しているものを設定
                    cond.NonyusakiCDs = this._nonyusakiCDs;
                }
                cond.Seiban = this.txtSeiban.Text;
                cond.Code = this.txtCode.Text;
                cond.KiwakuShip = this.txtKiwakuNo.Text;
                cond.CaseNo = this.txtCaseNo.Text;
                cond.TagNo = this.txtTagNo.Text;
                cond.BoxNo = this.GetTextWithPrefixAndPadLeft(this.txtBoxNo, this.lblBox.Text, this.txtBoxNo.MaxLength);
                cond.PalletNo = this.GetTextWithPrefixAndPadLeft(this.txtPalletNo, this.lblPallet.Text, this.txtPalletNo.MaxLength);
                if (this.cboDispSelect.Enabled)
                {
                    cond.DispSelect = this.cboDispSelect.SelectedValue.ToString();
                }

                this.txtShuka.Text = "0/0";
                this.txtBoxKonpo.Text = "0/0";
                this.txtPalletKonpo.Text = "0/0";
                this.txtKiwakuKonpo.Text = "0/0";

                var ds = conn.GetTehaiJohoShokai(cond);
                if (!ComFunc.IsExistsData(ds, Def_T_TEHAI_MEISAI.Name))
                {
                    // シートのクリア
                    this.SheetClear();
                    // 該当する手配明細はありません。
                    this.ShowMessage("T0100030002");
                    return false;
                }

                // 「品名」と「図番/型式」を含む条件で検索し、表示件数が10000件を超える場合、エラーメッセージを表示
                if (!string.IsNullOrEmpty(txtHinmei.Text) || !string.IsNullOrEmpty(txtZumenKeishiki.Text))
                {
                    if (ds.Tables[Def_T_TEHAI_MEISAI.Name].Rows.Count > 10000)
                    {
                        this.ShowMessage("T0100030019");
                        return false;
                    }

                }     

                this._filter.ClearFilter();
                this._needsSetFilter = true;

                this.shtMeisai.DataSource = ds.Tables[Def_T_TEHAI_MEISAI.Name];

                if (this.cboShukkaFlag.SelectedIndex > -1)
                {
                    if (this.cboShukkaFlag.SelectedValue.ToString() != SHUKKA_FLAG.AR_VALUE1)
                    {
                        var ships = ds.Tables[Def_T_TEHAI_MEISAI.Name].AsEnumerable()
                            .Where(x => !string.IsNullOrEmpty(ComFunc.GetFld(x, Def_M_NONYUSAKI.SHIP).Trim()))
                            .Select(x => ComFunc.GetFld(x, Def_M_NONYUSAKI.SHIP)).Distinct().ToArray();

                        this.txtDispShip.Text = string.Join(", ", ships);
                    }
                    else
                    {
                        this.txtDispShip.Text = string.Empty;
                    }
                }

                // 最も左上に表示されているセルの設定
                if (0 < this.shtMeisai.Rows.Count && this.shtMeisai.TopLeft.Row > -1)
                {
                    this.shtMeisai.TopLeft = new Position(SHEET_TOPLEFT_COL, this.shtMeisai.TopLeft.Row);
                }

                // 進捗
                if (ComFunc.IsExistsData(ds, ComDefine.DTTBL_PROGRESS) == true)
                {
                    DataTable dt = ds.Tables[ComDefine.DTTBL_PROGRESS];

                    this.txtShuka.Text = ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_SHUKA).ToString("###,##0") + "/" + ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_ALL).ToString("###,##0");
                    this.txtBoxKonpo.Text = ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_BOXKONPO).ToString("###,##0") + "/" + ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_ALL).ToString("###,##0");
                    this.txtPalletKonpo.Text = ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_PALLETKONPO).ToString("###,##0") + "/" + ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_ALL).ToString("###,##0");
                    this.txtKiwakuKonpo.Text = ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_KIWAKUKONPO).ToString("###,##0") + "/" + ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_ALL).ToString("###,##0");
                }

                this.ChangeMode(DisplayMode.EndSearch);

                // データの表示
                this.tabSearch.SelectedTab = this.tbpFilter;
                this.ShowData();

                _tempDt = (this.shtMeisai.DataSource as DataTable).Copy();

                this.shtMeisai.Redraw = true;

                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region モード切替

        /// --------------------------------------------------
        /// <summary>
        /// モード切替
        /// </summary>
        /// <param name="mode">画面の表示モード</param>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update>T.Nukaga 2019/11/28 タブの初期選択処理追加</update>
        /// <update>J.Chen 2024/10/29 履歴更新ボタン追加、更新制限追加</update>
        /// --------------------------------------------------
        private void ChangeMode(DisplayMode mode)
        {
            try
            {
                switch (mode)
                {
                    case DisplayMode.Initialize:
                        // ----- 初期 -----
                        this.tabSearch.Enabled = true;
                        this.shtMeisai.Enabled = false;
                        this.fbrFunction.F06Button.Enabled = false;

                        this.EditMode = SystemBase.EditMode.None;
                        this.fbrFunction.F01Button.Enabled = false;
                        this.fbrFunction.F02Button.Enabled = false;
                        this.fbrFunction.F08Button.Enabled = false;
                        this.fbrFunction.F10Button.Enabled = false;

                        //【Step_1_1】手配情報照会画面の権限制御 2022/10/13（TW-Tsuji）
                        //
                        //　ファンクションキー（F1、F3～F5）の表示制御を行う
                        //　今回作成した、メニューの権限チェッククラス（MenuAuthorityCheck）を
                        //　利用してファンクションキーの表示を切り替える
                        //
                        //【F1】はDisableのため何もしない
                        //this.fbrFunction.F01Button.Enabled = this._menuAuthCheck.IsMenuEnabled("T01", "T0100010");      //【F1】手配情報明細
                        this.fbrFunction.F03Button.Enabled = this._menuAuthCheck.IsMenuEnabled("T01", "T0100020");      //【F3】SKS手配明細
                        this.fbrFunction.F04Button.Enabled = this._menuAuthCheck.IsMenuEnabled("T01", "T0100060");      //【F4】手配入荷検品
                        this.fbrFunction.F05Button.Enabled = this._menuAuthCheck.IsMenuEnabled("S01", "S0100023");      //【F5】TAG登録連携
                        //---修正ここまで

                        this.tabSearch.SelectedIndex = TAB_DEFAULT_SELECTED_INDEX;
                        break;
                    case DisplayMode.EndSearch:
                        // ----- 検索後 -----
                        this.tabSearch.Enabled = true; //検索後も条件を変更して再検索できるようにする
                        this.shtMeisai.Enabled = true;
                        this.fbrFunction.F06Button.Enabled = true;

                        this.EditMode = SystemBase.EditMode.View;
                        this.fbrFunction.F01Button.Enabled = true;
                        //this.fbrFunction.F02Button.Enabled = true;
                        this.fbrFunction.F10Button.Enabled = true;

                        //【Step_1_1】手配情報照会画面の権限制御 2022/10/13（TW-Tsuji）
                        //
                        //　ファンクションキー（F1、F3～F5）の表示制御を行う
                        //　今回作成した、メニューの権限チェッククラス（MenuAuthorityCheck）を
                        //　利用してファンクションキーの表示を切り替える
                        //
                        this.fbrFunction.F01Button.Enabled = this._menuAuthCheck.IsMenuEnabled("T01", "T0100010");      //【F1】手配情報明細
                        this.fbrFunction.F03Button.Enabled = this._menuAuthCheck.IsMenuEnabled("T01", "T0100020");      //【F3】SKS手配明細
                        this.fbrFunction.F04Button.Enabled = this._menuAuthCheck.IsMenuEnabled("T01", "T0100060");      //【F4】手配入荷検品
                        this.fbrFunction.F05Button.Enabled = this._menuAuthCheck.IsMenuEnabled("S01", "S0100023");      //【F5】TAG登録連携
                        //---修正ここまで

                        // 物件名「全て」、「全て(AR)、「全て(AR 未出荷)」」の場合、履歴更新できないようにする
                        if (this.cboBukkenName.SelectedValue.ToString() == ComDefine.COMBO_ALL_VALUE
                            || this.cboBukkenName.SelectedValue.ToString() == ComDefine.COMBO_ALL_AR_VALUE
                            || this.cboBukkenName.SelectedValue.ToString() == ComDefine.COMBO_ALL_MISHUKKA_AR_VALUE)
                        {
                            // 物件名が「全て」の場合、履歴の更新はできません。
                            this.ShowMessage("T0100030020");
                            //var txtEditor = ElTabelleSheetHelper.NewTextEditor();
                            //txtEditor.ReadOnly = true;
                            //this.shtMeisai.Columns[(int)SHEET_TEHAI_RIREKI].Editor = txtEditor;
                            this.fbrFunction.F08Button.Enabled = false;

                            _isEditable = false;
                        }
                        else 
                        {
                            var txtEditor = ElTabelleSheetHelper.NewTextEditor();
                            txtEditor.ReadOnly = false;
                            txtEditor.MaxLength = 500;
                            this.shtMeisai.Columns[(int)SHEET_TEHAI_RIREKI].Editor = txtEditor;
                            this.fbrFunction.F08Button.Enabled = true;

                            _isEditable = true;
                        }
                        

                        break;
                    default:
                        break;
                }
                this.fbrFunction.F07Button.Enabled = true;
                this.fbrFunction.F12Button.Enabled = true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region シートのBackColor/ForeColor変更関連

        /// --------------------------------------------------
        /// <summary>
        /// 行の背景色、及び前景色を変更する
        /// </summary>
        /// <param name="row">列</param>
        /// <param name="input">入力文字列</param>
        /// <param name="baseGridLine">基準となる罫線設定</param>
        /// <param name="border">罫線設定</param>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetupRowColor(Row row, string input, GridLine baseGridLine, Borders border)
        {
            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            var cols = input.Split(',');
            if (cols.Length < 2)
            {
                return;
            }

            var backcolor = ComFunc.GetColorFromRgb(cols[1]);
            if (backcolor != null)
            {
                row.BackColor = backcolor ?? row.BackColor;
                row.DisabledBackColor = backcolor ?? row.BackColor;
                // 背景初期を設定すると、罫線がなくなってしまうため、手動設定する
                row.SetBorder(new BorderLine()
                {
                    Color = baseGridLine.Color,
                    LineStyle = (BorderLineStyle)Enum.Parse(typeof(BorderLineStyle), Enum.GetName(typeof(GridLineStyle), baseGridLine.Style)),
                }, border);
            }

            var forecolor = ComFunc.GetColorFromRgb(cols[0]);
            if (forecolor != null)
            {
                row.ForeColor = forecolor ?? row.ForeColor;
                row.DisabledForeColor = forecolor ?? row.ForeColor;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 範囲の背景色、及び前景色を変更する
        /// </summary>
        /// <param name="range">範囲</param>
        /// <param name="input">入力文字列</param>
        /// <param name="baseGridLine">基準となる罫線設定</param>
        /// <param name="border">罫線設定</param>
        /// <create>D.Okumura 2019/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetupRangeColor(Sheet sheet, Range range, string input, GridLine baseGridLine, Borders border)
        {
            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            var cols = input.Split(',');
            if (cols.Length < 2)
            {
                return;
            }

            var backcolor = ComFunc.GetColorFromRgb(cols[1]);
            if (backcolor != null)
            {
                sheet.SetCellInfo(range, CellInfo.BackColor, backcolor.Value);
                sheet.SetCellInfo(range, CellInfo.DisabledBackColor, backcolor.Value);
                // 背景初期を設定すると、罫線がなくなってしまうため、手動設定する
                sheet.SetBorder(range,new BorderLine()
                {
                    Color = baseGridLine.Color,
                    LineStyle = (BorderLineStyle)Enum.Parse(typeof(BorderLineStyle), Enum.GetName(typeof(GridLineStyle), baseGridLine.Style)),
                }, border);
            }

            var forecolor = ComFunc.GetColorFromRgb(cols[0]);
            if (forecolor != null)
            {
                sheet.SetCellInfo(range, CellInfo.ForeColor, forecolor.Value);
                sheet.SetCellInfo(range, CellInfo.DisabledForeColor, forecolor.Value);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// セルの背景色、及び前景色を変更する
        /// </summary>
        /// <param name="cell">セル</param>
        /// <param name="input">入力文字列</param>
        /// <param name="baseGridLine">基準となる罫線設定</param>
        /// <param name="border">罫線設定</param>
        /// <create>K.Tsutsumi 2019/09/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetupCellColor(Cell cell, string input, GridLine baseGridLine, Borders border)
        {
            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            var cols = input.Split(',');
            if (cols.Length < 2)
            {
                return;
            }

            var backcolor = ComFunc.GetColorFromRgb(cols[1]);
            if (backcolor != null)
            {
                cell.BackColor = backcolor ?? cell.BackColor;
                cell.DisabledBackColor = backcolor ?? cell.BackColor;
                // 背景初期を設定すると、罫線がなくなってしまうため、手動設定する
                cell.SetBorder(new BorderLine()
                {
                    Color = baseGridLine.Color,
                    LineStyle = (BorderLineStyle)Enum.Parse(typeof(BorderLineStyle), Enum.GetName(typeof(GridLineStyle), baseGridLine.Style)),
                }, border);
            }

            var forecolor = ComFunc.GetColorFromRgb(cols[0]);
            if (forecolor != null)
            {
                cell.ForeColor = forecolor ?? cell.ForeColor;
                cell.DisabledForeColor = forecolor ?? cell.ForeColor;
            }
        }

        #endregion

        #region 物件名切替

        /// --------------------------------------------------
        /// <summary>
        /// 物件名切替
        /// </summary>
        /// <create>H.Tajimi 2019/08/30</create>
        /// <update>K.Tsutsumi 2020/01/03 出荷区分の切り替えも強制的に呼び出す</update>
        /// --------------------------------------------------
        private void ChangeBukkenName()
        {
            if (this.cboBukkenName.SelectedIndex < 0)
            {
                return;
            }

            if (this.cboBukkenName.SelectedValue.ToString() == ComDefine.COMBO_ALL_AR_VALUE
             || this.cboBukkenName.SelectedValue.ToString() == ComDefine.COMBO_ALL_MISHUKKA_AR_VALUE)
            {
                this.cboShukkaFlag.SelectedValue = SHUKKA_FLAG.AR_VALUE1;
            }
            else
            {
                this.cboShukkaFlag.SelectedValue = string.Empty;
            }

            // 強制的に変更しているので、出荷区分の切り替えも強制的に呼び出す
            this.ChangeShukkaFlag();
        }

        #endregion

        #region 出荷区分切替

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分切替
        /// </summary>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update>K.Tsutsumi 2020/01/03 ARの場合は、全便を解除する</update>
        /// --------------------------------------------------
        private void ChangeShukkaFlag()
        {
            if (this.cboShukkaFlag.SelectedIndex < 0)
            {
                return;
            }

            // 便
            this.txtSelectShip.Text = string.Empty;
            // ARNo.
            this.txtARNo.Text = string.Empty;

            if (this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1)
            {
                // ----- 本体 -----
                bool isAR = false;
                // 全便対象
                this.chkAllShip.Enabled = !isAR;
                // 便参照
                this.btnRefShip.Enabled = !isAR;
                // AR No.
                this.txtARNo.Enabled = isAR;
            }
            else if (this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.AR_VALUE1)
            {
                // ----- AR -----
                bool isAR = true;
                // 全便対象
                this.chkAllShip.Enabled = !isAR;
                this.chkAllShip.Checked = !isAR;
                // 便参照
                this.btnRefShip.Enabled = !isAR;
                // AR No.
                this.txtARNo.Enabled = isAR;
            }
            else
            {
                bool isAR = false;
                // 全便対象
                this.chkAllShip.Enabled = !isAR;
                // 便参照
                this.btnRefShip.Enabled = !isAR;
                // AR No.
                this.txtARNo.Enabled = isAR;
            }
        }

        #endregion

        #region AutoFilter設定

        /// --------------------------------------------------
        /// <summary>
        /// AutoFilter設定
        /// </summary>
        /// <param name="isForceClear">強制的にAutoFilter設定をクリアするかどうか</param>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetEnableAutoFilter(bool isForceClear)
        {
            try
            {
                this.shtMeisai.Redraw = false;
                foreach (int col in this._autoFilterColumns)
                {
                    this.SetEnableAutoFilter(isForceClear, col);
                }
            }
            finally
            {
                this.shtMeisai.Redraw = true;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// AutoFilter設定 - 列指定
        /// </summary>
        /// <param name="isForceClear">強制的にAutoFilter設定をクリアするかどうか</param>
        /// <param name="col">列番号</param>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetEnableAutoFilter(bool isForceClear, int col)
        {
            if (isForceClear)
            {
                this.shtMeisai.ColumnHeaders[col].DropDown = null;
            }
            else
            {
                if (this.shtMeisai.ColumnHeaders[col].DropDown == null)
                {
                    var headerDropDown = new HeaderDropDown();
                    headerDropDown.EnableAutoFilter = true;
                    headerDropDown.EnableAutoSort = false;  // ソート系の項目は非表示
                    this.shtMeisai.ColumnHeaders[col].DropDown = headerDropDown;
                }
                else
                {
                    this.shtMeisai.ColumnHeaders[col].DropDown.EnableAutoFilter = true;
                    this.shtMeisai.ColumnHeaders[col].DropDown.EnableAutoSort = false;  // ソート系の項目は非表示
                }
            }
        }

        #endregion

        #region 絞込条件コンボボックスセット

        /// --------------------------------------------------
        /// <summary>
        /// 絞込条件コンボボックスセット
        /// </summary>
        /// <param name="dt"></param>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update>D.Okumura 2019/11/25 選択状態を維持</update>
        /// --------------------------------------------------
        private void SetFilter(DataTable dt)
        {
            if (dt == null)
            {
                return;
            }

            if (this.tabSearch.SelectedTab == this.tbpFilter && this._needsSetFilter)
            {
                this.SetComboDataSourceExKeep(this.fcboARNo, dt, Def_T_SHUKKA_MEISAI.AR_NO);
                this.SetComboDataSourceExKeep(this.fcboBukkenName, dt, Def_M_PROJECT.BUKKEN_NAME);
                this.SetComboDataSourceExKeep(this.fcboCode, dt, Def_T_SHUKKA_MEISAI.CODE);
                this.SetComboDataSourceExKeep(this.fcboEcsNo, dt, Def_M_ECS.ECS_NO);
                this.SetComboDataSourceExKeep(this.fcboEcsQuota, dt, Def_M_ECS.ECS_QUOTA);
                this.SetComboDataSourceExKeep(this.fcboEstimateFlag, dt, ComDefine.FLD_ESTIMATE_FLAG_NAME);
                this.SetComboDataSourceExKeep(this.fcboInvoiceNo, dt, Def_T_SHUKKA_MEISAI.INVOICE_NO);
                this.SetComboDataSourceExKeep(this.fcboNonyusaki, dt, Def_T_TEHAI_MEISAI.NOUHIN_SAKI);
                this.SetComboDataSourceExKeep(this.fcboSeiban, dt, Def_T_SHUKKA_MEISAI.SEIBAN);
                this.SetComboDataSourceExKeep(this.fcboShip, dt, Def_M_NONYUSAKI.SHIP);
                this.SetComboDataSourceExKeep(this.fcboShukkaFlag, dt, ComDefine.FLD_TEHAI_SYUKKA_FLAG_NAME);
                this.SetComboDataSourceExKeep(this.fcboShukkasaki, dt, Def_T_TEHAI_MEISAI.SYUKKA_SAKI);
                this.SetComboDataSourceExKeep(this.fcboTehaiFlag, dt, ComDefine.FLD_TEHAI_FLAG_NAME);
                this.SetComboDataSourceExKeep(this.fcboZumenOiban, dt, Def_T_SHUKKA_MEISAI.ZUMEN_OIBAN);

                this._needsSetFilter = true;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスにデータソースを設定する(「全て」付き)
        /// </summary>
        /// <param name="ctrl">コンボボックス</param>
        /// <param name="dt">データソース</param>
        /// <param name="field">抽出するフィールド</param>
        /// <create>D.Okumura 2019/11/25 選択を維持</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetComboDataSourceExKeep(ComboBox ctrl, DataTable dt, string field)
        {
            object value = ctrl.SelectedItem;
            base.SetComboDataSourceEx(ctrl, dt, field, true);
            if (value != null && ctrl.Items.Contains(value))
            {
                ctrl.SelectedItem = value;
            }
        }
        #endregion

        #region データの絞込

        /// --------------------------------------------------
        /// <summary>
        /// データの絞込の入り口
        /// </summary>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private void CallIdle()
        {
            if (this._idleStart)
            {
                return;
            }
            this._idleStart = true;
            Application.Idle += new EventHandler(Application_Idle);
        }

        /// --------------------------------------------------
        /// <summary>
        /// フィルターとフッターの設定を行う
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private void Application_Idle(object sender, EventArgs e)
        {
            if (!this.tabSearch.Enabled)
            {
                return;
            }

            Sheet sheet = this.shtMeisai;

            try
            {
                this._idleStart = false;
                Application.Idle -= new EventHandler(Application_Idle);

                sheet.Redraw = false;

                if (this.tabSearch.SelectedTab == this.tbpFilter)
                {
                    if (!UtilData.ExistsData(sheet.DataSource as DataTable))
                    {
                        return;
                    }

                    this._filter.SetFilterFromText(sheet.DataSource, Def_T_SHUKKA_MEISAI.AR_NO, this.fcboARNo, true);
                    this._filter.SetFilterFromText(sheet.DataSource, Def_M_PROJECT.BUKKEN_NAME, this.fcboBukkenName, true);
                    this._filter.SetFilterFromText(sheet.DataSource, Def_T_SHUKKA_MEISAI.CODE, this.fcboCode, true);
                    this._filter.SetFilterFromText(sheet.DataSource, Def_M_ECS.ECS_NO, this.fcboEcsNo, true);
                    this._filter.SetFilterFromText(sheet.DataSource, Def_M_ECS.ECS_QUOTA, this.fcboEcsQuota, true);
                    this._filter.SetFilterFromText(sheet.DataSource, ComDefine.FLD_ESTIMATE_FLAG_NAME, this.fcboEstimateFlag, true);
                    this._filter.SetFilterFromText(sheet.DataSource, Def_T_SHUKKA_MEISAI.INVOICE_NO, this.fcboInvoiceNo, true);
                    this._filter.SetFilterFromText(sheet.DataSource, Def_T_TEHAI_MEISAI.NOUHIN_SAKI, this.fcboNonyusaki, true);
                    this._filter.SetFilterFromText(sheet.DataSource, Def_T_SHUKKA_MEISAI.SEIBAN, this.fcboSeiban, true);
                    this._filter.SetFilterFromText(sheet.DataSource, Def_M_NONYUSAKI.SHIP, this.fcboShip, true);
                    this._filter.SetFilterFromText(sheet.DataSource, ComDefine.FLD_TEHAI_SYUKKA_FLAG_NAME, this.fcboShukkaFlag, true);
                    this._filter.SetFilterFromText(sheet.DataSource, Def_T_TEHAI_MEISAI.SYUKKA_SAKI, this.fcboShukkasaki, true);
                    this._filter.SetFilterFromText(sheet.DataSource, ComDefine.FLD_TEHAI_FLAG_NAME, this.fcboTehaiFlag, true);
                    this._filter.SetFilterFromText(sheet.DataSource, Def_T_SHUKKA_MEISAI.ZUMEN_OIBAN, this.fcboZumenOiban, true);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                sheet.Redraw = true;
            }
        }
        
        #endregion

        #region データ表示

        /// --------------------------------------------------
        /// <summary>
        /// データ表示
        /// </summary>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ShowData()
        {
            if (this.tabSearch.SelectedTab == this.tbpFilter)
            {
                var dt = this.shtMeisai.DataSource as DataTable;
                if (UtilData.ExistsData(dt))
                {
                    // 絞込条件セット
                    this.SetFilter(dt);
                }
            }
            this.CallIdle();
        }

        #endregion

        #region 画面表示

        #region 納入先一覧画面表示

        /// --------------------------------------------------
        /// <summary>
        /// 納入先一覧画面表示
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ShowNonyusakiIchiran()
        {
            // 納入先一覧は本体固定で表示
            string shukkaFlag = SHUKKA_FLAG.NORMAL_VALUE1;
            string nonyusakiName = this.cboBukkenName.Text;
            using (var frm = new NonyusakiIchiranEx(this.UserInfo, shukkaFlag, nonyusakiName, string.Empty, true, this.chkAllShip.Checked))
            {
                var ret = frm.ShowDialog(this);
                if (ret == DialogResult.OK)
                {
                    this.ClearMessage();
                    var drCollection = frm.SelectedRowsData;
                    if (drCollection == null) return false;
                    // 選択データを設定
                    this.txtSelectShip.Text = string.Join(", ", drCollection.Select(x => ComFunc.GetFld(x, Def_M_NONYUSAKI.SHIP)).ToArray());
                    this._nonyusakiCDs = drCollection.Select(x => ComFunc.GetFld(x, Def_M_NONYUSAKI.NONYUSAKI_CD)).ToArray();
                }
            }
            return false;
        }

        #endregion

        #endregion

        #region 写真表示

        /// --------------------------------------------------
        /// <summary>
        /// 写真表示処理
        /// </summary>
        /// <param name="saveDir">格納フォルダ</param>
        /// <param name="fileName">ファイル名</param>
        /// <returns>OK/NG</returns>
        /// <remarks>出荷区分が本体の場合は、AR No.はstring.emptyでOK</remarks>
        /// <create>K.Tsutsumi 2019/09/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool FileDownload(string saveDir, string fileName)
        {
            try
            {
                ConnAttachFile conn = new ConnAttachFile();
                FileDownloadPackage package = new FileDownloadPackage();

                package.FileName = fileName;
                package.FileType = FileType.TagPictures;
                package.FolderName = saveDir;

                FileDownloadResult result = conn.FileDownload(package);
                if (!result.IsExistsFile)
                {
                    return false;
                }

                if (!Directory.Exists(ComDefine.DOWNLOAD_DIR))
                {
                    Directory.CreateDirectory(ComDefine.DOWNLOAD_DIR);
                }
                var dir = Path.Combine(ComDefine.DOWNLOAD_DIR, saveDir);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                //var downloadedFileName = Path.Combine(ComDefine.DOWNLOAD_DIR, shukkaFlag + nonyusakiCd + fileName);
                var downloadedFileName = Path.Combine(dir, fileName);
                if (File.Exists(downloadedFileName))
                {
                    // 同一ファイルが存在する場合は削除する
                    File.Delete(downloadedFileName);
                }
                using (FileStream fs = new FileStream(downloadedFileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(result.FileData, 0, result.FileData.Length);
                    fs.Close();
                }
                // ファイルを関連付けられたアプリケーションで開く
                using (Process p = new Process())
                {
                    p.StartInfo.FileName = downloadedFileName;
                    p.Start();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion


        #region イベント

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F1ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F01Button_Click(sender, e);

            try
            {
                // 検索結果がなければ処理を抜ける
                if (this.shtMeisai.MaxRows < 1) return;

                var row = this.shtMeisai.ActivePosition.Row;
                var ecsQuota = UtilConvert.ToInt32(this.shtMeisai[SHEET_ECS_QUOTA, row].Text);
                var ecsNo = this.shtMeisai[SHEET_ECS_NO, row].Text;

                using (var frm = new TehaiMeisai(this.UserInfo, ComDefine.TITLE_T0100010, ecsQuota, ecsNo))
                {
                    if (DialogResult.OK == frm.ShowDialog())
                    {
                        // 再検索を行いますか？
                        if (DialogResult.OK == this.ShowMessage("T0100030003"))
                        {
                            // 再検索を行う
                            this.btnStart_Click(sender, e);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }


        /// --------------------------------------------------
        /// <summary>
        /// F3ボタンクリック(連携状況ボタンクリック) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>R.Sumi 2022/02/25</create>
        /// <update>H.Iimuro 2022/10/24 開く画面をモードレスダイアログに変更</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F03Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F03Button_Click(sender, e);
            try
            {
                CondT01 cond = new CondT01(this.UserInfo);
                List<string> listRenkeiNo = new List<string>();
                listRenkeiNo.Clear();

                this.ClearMessage();
                //ここに既に表示されているかを判定し
                //表示されているなら元の画面を一度閉じて、新しく開く
                foreach (Form form in Application.OpenForms)
                {
                    BaseForm frm = form as BaseForm;
                    if (frm != null && frm.MenuCategoryID.Equals("T01") && frm.MenuItemID.Equals("T0100020"))
                    {
                        //if (this.ShowMessage("T0100030010") != DialogResult.OK) return;
                        if (frm.WindowState == FormWindowState.Minimized)
                        {
                            frm.WindowState = FormWindowState.Normal;
                        }
                        // 画面をTopへ
                        frm.Activate();
                        return;
                        //// 画面をclose
                        //frm.Dispose();
                        //break;
                    }
                }
                // 連携状況画面を新規で開く
                var workFrm = new TehaiRenkei(this.UserInfo, "T01", "T0100020", ComDefine.TITLE_T0100020);
                TehaiRenkei.TehaiRenkeiInstance = workFrm;
                TehaiRenkei.TehaiRenkeiInstance.BukkenNameText = this.cboBukkenName.Text;
                TehaiRenkei.TehaiRenkeiInstance.EcsNoText = this.txtEcsNo.Text;
                TehaiRenkei.TehaiRenkeiInstance.SeibanText = this.txtSeiban.Text;
                TehaiRenkei.TehaiRenkeiInstance.CodeText = this.txtCode.Text;

                // 絞り込み検索のデータ取得
                if (this.fcboBukkenName.SelectedIndex != 0 && this.fcboBukkenName.Text != "")
                {
                    TehaiRenkei.TehaiRenkeiInstance.BukkenNameText = this.fcboBukkenName.Text;
                }

                if (this.fcboEcsNo.SelectedIndex != 0 && this.fcboEcsNo.Text != "")
                {
                    TehaiRenkei.TehaiRenkeiInstance.EcsNoText = this.fcboEcsNo.Text;
                }

                if (this.fcboSeiban.SelectedIndex != 0 && this.fcboSeiban.Text != "")
                {
                    TehaiRenkei.TehaiRenkeiInstance.SeibanText = this.fcboSeiban.Text;
                }

                if (this.fcboCode.SelectedIndex != 0 && this.fcboCode.Text != "")
                {
                    TehaiRenkei.TehaiRenkeiInstance.CodeText = this.fcboCode.Text;
                }

                // フォームの表示
                TehaiRenkei.TehaiRenkeiInstance.Show();
                TehaiRenkei.TehaiRenkeiInstance.Activate();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F4ボタンクリック(入荷検品登録ボタンクリック) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>R.Sumi 2022/02/28</create>
        /// <update>H.Iimuro 2022/10/24 開く画面をモードレスダイアログに変更</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F04Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F04Button_Click(sender, e);
            try
            {
                CondT01 cond = new CondT01(this.UserInfo);
                List<string> listRenkeiNo = new List<string>();
                listRenkeiNo.Clear();

                this.ClearMessage();
                //ここに既に表示されているかを判定し
                //表示されているなら元の画面を一度閉じて、新しく開く
                foreach (Form form in Application.OpenForms)
                {
                    BaseForm frm = form as BaseForm;
                    if (frm != null && frm.MenuCategoryID.Equals("T01") && frm.MenuItemID.Equals("T0100060"))
                    {
                        if (frm.WindowState == FormWindowState.Minimized)
                        {
                            frm.WindowState = FormWindowState.Normal;
                        }
                        // 画面をTopへ
                        frm.Activate();
                        return;
                    }
                }
                // 入荷検品登録画面を新規で開く
                var workFrm = new TehaiKenpin(this.UserInfo, "T01", "T0100060", ComDefine.TITLE_T0100060);
                TehaiKenpin.TehaiKenpinInstance = workFrm;
                TehaiKenpin.TehaiKenpinInstance.BukkenNameText = this.cboBukkenName.Text;
                TehaiKenpin.TehaiKenpinInstance.EcsNoText = this.txtEcsNo.Text;
                TehaiKenpin.TehaiKenpinInstance.SeibanText = this.txtSeiban.Text;
                TehaiKenpin.TehaiKenpinInstance.CodeText = this.txtCode.Text;
                TehaiKenpin.TehaiKenpinInstance.ARNoText = this.txtARNo.Text;

                // 絞り込み検索のデータ取得
                if (this.fcboBukkenName.SelectedIndex != 0 && this.fcboBukkenName.Text != "")
                {
                    TehaiKenpin.TehaiKenpinInstance.BukkenNameText = this.fcboBukkenName.Text;
                }

                if (this.fcboEcsNo.SelectedIndex != 0 && this.fcboEcsNo.Text != "")
                {
                    TehaiKenpin.TehaiKenpinInstance.EcsNoText = this.fcboEcsNo.Text;
                }

                if (this.fcboSeiban.SelectedIndex != 0 && this.fcboSeiban.Text != "")
                {
                    TehaiKenpin.TehaiKenpinInstance.SeibanText = this.fcboSeiban.Text;
                }

                if (this.fcboCode.SelectedIndex != 0 && this.fcboCode.Text != "")
                {
                    TehaiKenpin.TehaiKenpinInstance.CodeText = this.fcboCode.Text;
                }

                if (this.fcboARNo.SelectedIndex != 0 && this.fcboARNo.Text != "")
                {
                    TehaiKenpin.TehaiKenpinInstance.ARNoText = this.fcboARNo.Text.Replace("AR", "");
                }

                // フォームの表示
                TehaiKenpin.TehaiKenpinInstance.Show();
                TehaiKenpin.TehaiKenpinInstance.Activate();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F5ボタンクリック(TAG登録ボタンクリック) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>R.Sumi 2022/02/28</create>
        /// <update>N.Ikari 2022/05/16 絞り込み検索データの取得修正</update>
        /// <update>H.Iimuro 2022/10/24 開く画面をモードレスダイアログに変更</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F05Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F05Button_Click(sender, e);
            try
            {
                CondT01 cond = new CondT01(this.UserInfo);
                List<string> listRenkeiNo = new List<string>();
                listRenkeiNo.Clear();

                this.ClearMessage();

                //ここに既に表示されているかを判定し
                //表示されているなら元の画面を一度閉じて、新しく開く
                foreach (Form form in Application.OpenForms)
                {
                    BaseForm frm = form as BaseForm;
                    if (frm != null && frm.MenuCategoryID.Equals("S01") && frm.MenuItemID.Equals("S0100023"))
                    {
                        if (frm.WindowState == FormWindowState.Minimized)
                        {
                            frm.WindowState = FormWindowState.Normal;
                        }
                        // 画面をTopへ
                        frm.Activate();
                        return;
                    }
                }
                // TAG登録画面を新規で開く
                var workFrm = new SMS.S01.Forms.ShukkakeikakuRenkei(this.UserInfo, "S01", "S0100023", ComDefine.TITLE_S0100023);
                SMS.S01.Forms.ShukkakeikakuRenkei.ShukkakeikakuRenkeiInstance = workFrm;
                SMS.S01.Forms.ShukkakeikakuRenkei.ShukkakeikakuRenkeiInstance.BukkenNameText = this.cboBukkenName.Text;
                SMS.S01.Forms.ShukkakeikakuRenkei.ShukkakeikakuRenkeiInstance.EcsNoText = this.txtEcsNo.Text;
                SMS.S01.Forms.ShukkakeikakuRenkei.ShukkakeikakuRenkeiInstance.txtARNo = this.txtARNo.Text;
                SMS.S01.Forms.ShukkakeikakuRenkei.ShukkakeikakuRenkeiInstance.fcboARNo = this.fcboARNo.Text;
                SMS.S01.Forms.ShukkakeikakuRenkei.ShukkakeikakuRenkeiInstance.SeibanCodeText = this.txtSeiban.Text + this.txtCode.Text;
                // 絞り込み検索のデータ取得
                if (this.fcboBukkenName.SelectedIndex != 0 && this.fcboBukkenName.Text != "")
                {
                    SMS.S01.Forms.ShukkakeikakuRenkei.ShukkakeikakuRenkeiInstance.BukkenNameText = this.fcboBukkenName.Text;
                }

                if (this.fcboEcsNo.SelectedIndex != 0 && this.fcboEcsNo.Text != "")
                {
                    SMS.S01.Forms.ShukkakeikakuRenkei.ShukkakeikakuRenkeiInstance.EcsNoText = this.fcboEcsNo.Text;
                }

                if (this.fcboSeiban.SelectedIndex != 0 || this.fcboCode.SelectedIndex != 0)
                {
                    SMS.S01.Forms.ShukkakeikakuRenkei.ShukkakeikakuRenkeiInstance.SeibanCodeText = this.fcboSeiban.Text + this.txtCode.Text;
                }
                if (this.fcboSeiban.SelectedIndex != 0 && this.fcboCode.SelectedIndex != 0)
                {
                    SMS.S01.Forms.ShukkakeikakuRenkei.ShukkakeikakuRenkeiInstance.SeibanCodeText = this.fcboSeiban.Text + this.fcboCode.Text;
                }

                // フォームの表示
                SMS.S01.Forms.ShukkakeikakuRenkei.ShukkakeikakuRenkeiInstance.Show();
                SMS.S01.Forms.ShukkakeikakuRenkei.ShukkakeikakuRenkeiInstance.Activate();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }


        /// --------------------------------------------------
        /// <summary>
        /// F6ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                if (this.ShowMessage("A9999999053") != DialogResult.OK) return;
                // 検索結果クリア
                this.SheetClear();
                // 進捗件数
                this.txtShuka.Text = "0/0";
                this.txtBoxKonpo.Text = "0/0";
                this.txtPalletKonpo.Text = "0/0";
                this.txtKiwakuKonpo.Text = "0/0";

                this.txtDispShip.Text = string.Empty;

                this.ChangeMode(DisplayMode.Initialize);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F7ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                // クリアしてもいいですか？ダイアログ表示
                if (this.ShowMessage("A9999999001") != DialogResult.OK) return;
                this.SheetClear();
                this.DisplayClear();
                this.ChangeMode(DisplayMode.Initialize);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F8ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>J.Chen 2024/10/25</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F08Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F08Button_Click(sender, e);
            try
            {
                EditMode = SystemBase.EditMode.Update;
                var pos = this.shtMeisai.TopLeft;
                if (RunEdit())
                {
                    // 処理が成功したとき、再検索を行う
                    RunSearch(pos);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F9ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/12/12</create>
        /// <update>D.Okumura 2020/01/06 関連付けなしの際にエラーメッセージを表示するように修正</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F09Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F09Button_Click(sender, e);
            try
            {

                ConnAttachFile conn = new ConnAttachFile();
                FileDownloadPackage package = new FileDownloadPackage();

                package.FileName = ComDefine.LEGEND_FILENAME1;
                package.FileType = FileType.Legend;
                package.FolderName = this.UserInfo.Language;

                FileDownloadResult result = conn.FileDownload(package);
                if (!result.IsExistsFile)
                {
                    return;
                }

                if (!Directory.Exists(ComDefine.DOWNLOAD_DIR))
                {
                    Directory.CreateDirectory(ComDefine.DOWNLOAD_DIR);
                }
                var dir = Path.Combine(ComDefine.DOWNLOAD_DIR, ComDefine.DOWNLOAD_DIR_LEGEND);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                var downloadedFileName = Path.Combine(dir, ComDefine.LEGEND_FILENAME1);
                if (File.Exists(downloadedFileName))
                {
                    // 同一ファイルが存在する場合は削除する
                    File.Delete(downloadedFileName);
                }
                File.WriteAllBytes(downloadedFileName, result.FileData);
                // ファイルを関連付けられたアプリケーションで開く
                using (Process p = new Process())
                {
                    p.StartInfo.FileName = downloadedFileName;
                    p.Start();
                }
            }
            catch (Win32Exception ex)
            {
                // ERROR_NO_ASSOCIATION: 関連付けなし
                if (ex.NativeErrorCode == 0x483)
                {
                    // Excelがinstallされていません。
                    this.ShowMessage("T0100030008");
                }
                else
                {
                    CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F10ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update>J.Chen 2022/06/30 Excel出力制限設置</update>
        /// <update>JB.Shin 2022/09/30 Excel出力ファイル名、保存先設定修正</update>
        /// <update>J.Chen 2022/11/08 Excel出力ファイル名手動修正対応追加</update>
        /// <update>J.Chen 2024/10/29 変更履歴出力追加、出力時変更チェック追加</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F10Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F10Button_Click(sender, e);
            try
            {

                if (_isEditable)
                {
                    DataTable dtUpdCheck = (this.shtMeisai.DataSource as DataTable).Copy();
                    DataTable dtUpdate = this.GetDataTehaiShokaiMeisaiFilter(dtUpdCheck);   // 更新データ抽出

                    if (dtUpdate != null)
                    {
                        // 内容が変更されているため、Excel出力できません。
                        this.ShowMessage("S0100070004");
                        return;
                    }
                }

                // 物件名取得
                var projectName = this.cboBukkenName.Text;


                // 期取得
                var kiNum = this.txtEcsQuota.Text;
                var dateTime = DateTime.Now.ToString("yyyyMMdd");

                var fileName = "";
                var fileNameSub_1 = "";
                var fileNameSub_2 = "";
                string path = "";
                string newPath = "";
                int fileNameLength = 0;
                int pathLength = 0;

                //ファイル名に使用できない文字を取得
                char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();

                if (projectName.IndexOfAny(invalidChars) >= 0)
                {
                    // ファイル名に利用できない文字が含まれています。
                    this.ShowMessage("T0100030014");
                }

                // 3万件以下の場合は正常処理
                if (this.shtMeisai.MaxRows <= 30000)
                {
                    // 画面に貼り付けてある SaveFileDialog を使用するとボタン連打でStackOverFlowが発生する
                    using (SaveFileDialog frm = new SaveFileDialog())
                    {
                        frm.Title = Resources.TehaiShokai_sfdExcel_Title;
                        frm.Filter = Resources.TehaiShokai_sfdExcel_Filter;
                        fileName = ComDefine.EXCEL_FILE_TEHAI_MEISAI;
                        fileNameSub_1 = fileName.Substring(0, 4);
                        fileNameSub_2 = fileName.Substring(4, 5);
                        frm.FileName = fileNameSub_1 + projectName + " " + kiNum + dateTime + fileNameSub_2;
                        if (0 < this.shtMeisai.MaxRows && frm.ShowDialog() != DialogResult.OK) return;
                        // Excel出力処理
                        DataTable dtExport = (this.shtMeisai.DataSource as DataTable).Copy();
                        ExportTehaiMeisai export = new ExportTehaiMeisai();
                        string msgID;
                        string[] args;

                        //【Step_1_1】手配情報照会画面の権限制御 2022/10/17（TW-Tsuji）
                        //　単価（JPY）列の出力をフラグで切り替える.
                        export.IsUnitPriceDisable = _flgUnitPriceDisable;

                        export.ExportExcel(frm.FileName, dtExport, out msgID, out args);
                        if (!string.IsNullOrEmpty(msgID))
                        {
                            this.ShowMessage(msgID, args);
                        }
                    }
                }
                else if (this.shtMeisai.MaxRows > 30000) // 3万件以上の場合 1万行ことにファイルを分割する
                {
                    // 印刷最大行数
                    int maxExportRow = 10000;

                    // 8万件超えると8000
                    if (this.shtMeisai.MaxRows > 80000)
                    {
                        maxExportRow = 8000;
                    }
                    
                    if (this.shtMeisai.MaxRows > 100000)
                    {
                        this.ShowMessage("T0100030013");
                        return;
                    }
                    // ファイル数
                    int fileCnt = this.shtMeisai.MaxRows / maxExportRow;


                    DataTable dt = (this.shtMeisai.DataSource as DataTable).Copy();
                    DataTable tdtest = dt.Clone();

                    for (int rowsCnt = 0, num = 1; rowsCnt < this.shtMeisai.MaxRows; rowsCnt++)
                    {
                        if (num <= fileCnt)
                        {
                            if (rowsCnt < maxExportRow * num)
                            {
                                tdtest.ImportRow(dt.Rows[rowsCnt]);

                                if (rowsCnt == maxExportRow * num - 1)
                                {
                                    // 画面に貼り付けてある SaveFileDialog を使用するとボタン連打でStackOverFlowが発生する
                                    using (SaveFileDialog frm = new SaveFileDialog())
                                    {
                                        frm.Title = Resources.TehaiShokai_sfdExcel_Title;
                                        frm.Filter = Resources.TehaiShokai_sfdExcel_Filter;
                                        fileName = ComDefine.EXCEL_FILE_TEHAI_MEISAI;
                                        fileNameSub_1 = fileName.Substring(0, 4);
                                        fileNameSub_2 = fileName.Substring(4, 5);
                                        frm.FileName = fileNameSub_1 + projectName + " " + kiNum + dateTime + "_" + num + fileNameSub_2;

                                        // 初回のみ保存先設定
                                        if (num == 1)
                                        {
                                            fileNameLength = frm.FileName.Length;
                                            if (0 < this.shtMeisai.MaxRows && frm.ShowDialog() != DialogResult.OK) return;

                                        }
                                        //二回目からファイル名をパスを含めて保存
                                        else
                                        {
                                            //frm.FileName = newPath + frm.FileName;
                                            frm.FileName = newPath + "_" + num + fileNameSub_2;
                                        }
       
                                        
                                        // Excel出力処理
                                        //DataTable dtExport = (this.shtMeisai.DataSource as DataTable).Copy();
                                        ExportTehaiMeisai export = new ExportTehaiMeisai();
                                        string msgID;
                                        string[] args;

                                        //【Step_1_1】手配情報照会画面の権限制御 2022/10/17（TW-Tsuji）
                                        //　単価（JPY）列の出力をフラグで切り替える.
                                        export.IsUnitPriceDisable = _flgUnitPriceDisable;

                                        export.ExportExcel(frm.FileName, tdtest, out msgID, out args);
                                        
                                        //最初excelExportと同時に最初保存先の取得
                                        if (num == 1)
                                        {
                                            export.ExportExcel(frm.FileName, tdtest, out msgID, out args);
                                            path = Path.GetFullPath(frm.FileName);
                                            pathLength = path.Length;

                                            //最初保存先
                                            if (path.EndsWith("_1.xlsx"))
                                            {
                                                newPath = path.Substring(0, pathLength - 7);
                                            }
                                            else 
                                            {
                                                newPath = path.Substring(0, pathLength - 5);
                                            }
                                            //newPath = path.Substring(0, pathLength - fileNameLength);

                                        }
                                        else
                                        {
                                            export.ExportExcel(frm.FileName, tdtest, out msgID, out args);
                                        }
                                        
                                        if (!string.IsNullOrEmpty(msgID))
                                        {
                                            // 出力成功以外のメッセージ
                                            if (msgID != "E0100150001")
                                            {
                                                this.ShowMessage(msgID, args);
                                            }
                                        }
                                        tdtest = null;
                                        tdtest = dt.Clone();
                                    }

                                    num++;
                                }
                            }
                        }
                        else
                        {
                            tdtest.ImportRow(dt.Rows[rowsCnt]);
                            if (rowsCnt == this.shtMeisai.MaxRows - 1)
                            {
                                // 画面に貼り付けてある SaveFileDialog を使用するとボタン連打でStackOverFlowが発生する
                                using (SaveFileDialog frm = new SaveFileDialog())
                                {
                                    frm.Title = Resources.TehaiShokai_sfdExcel_Title;
                                    frm.Filter = Resources.TehaiShokai_sfdExcel_Filter;
                                    
                                    fileName = ComDefine.EXCEL_FILE_TEHAI_MEISAI;
                                    fileNameSub_1 = fileName.Substring(0, 4);
                                    fileNameSub_2 = fileName.Substring(4, 5);
                                    var newFileName = fileNameSub_1 + projectName + " " + kiNum + dateTime + "_" + num + fileNameSub_2;

                                    //frm.FileName = newPath + newFileName;
                                    frm.FileName = newPath + "_" + num + fileNameSub_2;

                                    // Excel出力処理
                                    //DataTable dtExport = (this.shtMeisai.DataSource as DataTable).Copy();
                                    ExportTehaiMeisai export = new ExportTehaiMeisai();
                                    string msgID;
                                    string[] args;
                                    
                                    //【Step_1_1】手配情報照会画面の権限制御 2022/10/17（TW-Tsuji）
                                    //　単価（JPY）列の出力をフラグで切り替える.
                                    export.IsUnitPriceDisable = _flgUnitPriceDisable;

                                    export.ExportExcel(frm.FileName, tdtest, out msgID, out args);
                                    if (!string.IsNullOrEmpty(msgID))
                                    {
                                        this.ShowMessage(msgID, args);
                                    }
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F11ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F11Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F11Button_Click(sender, e);
            try
            {
                // 検索結果が存在しない場合は処理を抜ける。
                if (this.shtMeisai.Rows.Count == 0)
                {
                    // 明細が入力されていません。
                    this.ShowMessage("A9999999028");
                    this.shtMeisai.Focus();
                    return;
                }

                // カーソル位置の行取得
                var row = this.shtMeisai.ActivePosition.Row;
                // 写真がアップロードされていない場合は処理を抜ける。
                var fileName1 = this.shtMeisai[SHEET_COL_FILE_NAME1, row].Text;
                var fileName2 = this.shtMeisai[SHEET_COL_FILE_NAME2, row].Text;
                if (string.IsNullOrEmpty(fileName1) && string.IsNullOrEmpty(fileName2))
                {
                    // {0}行目の写真は登録されていません。
                    this.ShowMessage("T0100030004", new string[] { (row + 1).ToString() });
                    this.shtMeisai.Focus();
                    return;
                }

                if (!string.IsNullOrEmpty(fileName1))
                {
                    // ダウンロード1
                    var saveDir = this.shtMeisai[SHEET_COL_SAVE_DIR1, row].Text;
                    if (!this.FileDownload(saveDir, fileName1))
                    {
                        // 写真のダウンロードに失敗しました。
                        this.ShowMessage("T0100030005");
                        this.shtMeisai.Focus();
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(fileName2))
                {
                    // ダウンロード2
                    var saveDir = this.shtMeisai[SHEET_COL_SAVE_DIR1, row].Text;
                    if (!this.FileDownload(saveDir, fileName2))
                    {
                        // 写真のダウンロードに失敗しました。
                        this.ShowMessage("T0100030005");
                        this.shtMeisai.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region ボタン

        #region 開始ボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// 開始ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                this.RunSearch();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }
        
        #endregion

        #region 便参照ボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// 便参照ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnRefShip_Click(object sender, EventArgs e)
        {
            try
            {
                this.ShowNonyusakiIchiran();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }
        
        #endregion
        
        #endregion

        #region コンボボックス

        #region 物件名変更

        /// --------------------------------------------------
        /// <summary>
        /// 物件名変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/08/30</create>
        /// <update>K.Tsutsumi 2019/09/07 手入力からの確定で変化しないためイベント変更</update>
        /// --------------------------------------------------
        private void cboBukkenName_SelectionChangeCommitted(object sender, EventArgs e)
        {
//            this.ChangeBukkenName();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 物件名変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>K.Tsutsumi 2019/09/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboBukkenName_SelectedValueChanged(object sender, EventArgs e)
        {
            this.ChangeBukkenName();
        }

        #endregion

        #region 出荷区分変更

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboShukkaFlag_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.ChangeShukkaFlag();
        }
        
        #endregion
        
        #endregion

        #region フィルター用コンボボックス

        /// --------------------------------------------------
        /// <summary>
        /// フィルター用コンボボックス
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ComboBox_Filter(object sender, EventArgs e)
        {
            try
            {
                this.CallIdle();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region シートイベント

        #region RowFilling

        /// --------------------------------------------------
        /// <summary>
        /// データソースに DataRow が追加または削除され、シートの行が追加または削除される前に発生するイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update>C.Hsieh 2019/11/15 手配情報照会背景色対応</update>
        /// <update>D.Okumura 2019/12/11 手配情報照会背景色対応</update>
        /// <update>D.Okumura 2020/01/06 AssyParts対応</update>
        /// <update>R.Sumi 2022/03/16 出荷数が0の時にグレーアウト表示させる対応</update>
        /// <update>Y.Gwon 2023/07/31 返却品が「返却対象」の場合に紫色に表示させる対応</update>
        /// <update>J.Chen 2023/09/28 返却品の区分「保留」「通関確認中」は「返却対象」と同じ処理とする</update>
        /// --------------------------------------------------
        private void shtMeisai_RowFilling(object sender, RowFillingEventArgs e)
        {
            var sheet = sender as Sheet;
            if (sheet == null)
            {
                return;
            }

            if (e.DestRow != -1 && e.OperationMode == OperationMode.Add)
            {
                // 手配色(行単位)の設定
                var row = e.DestRow;
                var dt = sheet.DataSource as DataTable;

                //出荷数取得
                var shukkaQty = shtMeisaiGetSourceData<decimal>(dt, e.SourceRow, Def_T_TEHAI_MEISAI.SHUKKA_QTY);
                var henkyakuFlag = shtMeisaiGetSourceData<string>(dt, e.SourceRow, Def_T_TEHAI_MEISAI.HENKYAKUHIN_FLAG);
                if (shukkaQty == 0)     //出荷数0の時、濃いグレーで表示します。
                {
                    var color = ComDefine.GRY_COLOR;
                    SetupRowColor(sheet.Rows[row], color, sheet.GridLine, Borders.All);
                    if (henkyakuFlag != HENKYAKUHIN_FLAG.DEFAULT_VALUE1)
                    {
                        //返却品が「返却対象」の場合、特定列の範囲だけ紫色で表示します
                        var colorPurple = ComDefine.PURPLE_COLOR;
                        var headRange = new Range(SHEET_TEHAI_RENKEI_NO, row, SHEET_SETTEI_DATE, row);
                        var tailRange = new Range(SHEET_FREE1, row, SHEET_ASSY_OYA_ZAN, row);
                        this.SetupRangeColor(sheet, headRange, colorPurple, sheet.GridLine, Borders.All);
                        this.SetupRangeColor(sheet, tailRange, colorPurple, sheet.GridLine, Borders.All);
                    }
                }
                else 
                {
                    var color = shtMeisaiGetSourceData<string>(dt, e.SourceRow, ComDefine.FLD_ESTIMATE_COLOR);
                    SetupRowColor(sheet.Rows[row], color, sheet.GridLine, Borders.All);
                }

                

                var tehaiFlag = shtMeisaiGetSourceData<string>(dt, e.SourceRow, Def_T_TEHAI_MEISAI.TEHAI_FLAG);
                var tagTourokuMax = shtMeisaiGetSourceData<decimal>(dt, e.SourceRow, ComDefine.FLD_TAG_TOUROKU_MAX);
                var assyOyaZan = shtMeisaiGetSourceData<decimal?>(dt, e.SourceRow, ComDefine.FLD_ASSY_OYA_ZAN);
                // 手配色(セル)単位の設定
                if (( tagTourokuMax > 0
                   // 社内調達・SKS　Skipで返却品の場合は無条件(TAG登録可能数が必ず0となるため)に色付けを行う
                   || tehaiFlag == TEHAI_FLAG.SKS_SKIP_VALUE1 || tehaiFlag == TEHAI_FLAG.SURPLUS_VALUE1
                   )
                   // 親Assyがない、または親の組み立て残数が0より大きいとき
                    && (assyOyaZan == null || assyOyaZan.Value > 0)
                )
                {
                    var henkyakuhinFlag = shtMeisaiGetSourceData<string>(dt, e.SourceRow, Def_T_TEHAI_MEISAI.HENKYAKUHIN_FLAG);
                    string cellColor = "";
                    if (henkyakuhinFlag != HENKYAKUHIN_FLAG.DEFAULT_VALUE1)
                        cellColor = shtMeisaiGetSourceData<string>(dt, e.SourceRow, TEHAI_COLOR.COLOR04_NAME);
                    else
                        cellColor = shtMeisaiGetSourceData<string>(dt, e.SourceRow, TEHAI_COLOR.COLOR02_NAME);
                    var range = new Range(SHEET_NONYU_JYOTAI, row, SHEET_TEHAI_QTY, row);
                    this.SetupRangeColor(sheet, range, cellColor, sheet.GridLine, Borders.All);
                }
            }
        }
        /// --------------------------------------------------
        /// <summary>
        /// データソースに該当する列名の情報を取得する
        /// </summary>
        /// <typeparam name="T">値の種類</typeparam>
        /// <param name="dt">データソース</param>
        /// <param name="sourceRow">行データ(生データ)</param>
        /// <param name="colName">列名称</param>
        /// <returns>セルの値</returns>
        /// <create>D.Okumura 2019/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private static T shtMeisaiGetSourceData<T>(DataTable dt, object sourceRow, string colName)
        {
            int offset = dt.Columns[colName].Ordinal;
            object data = ((object[])sourceRow)[offset];
            return (T)data;
        }
        
        #endregion

        #endregion

        #region タブイベント

        /// --------------------------------------------------
        /// <summary>
        /// タブページの切替
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private void tabSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabSearch.SelectedTab == this.tbpFilter)
            {
                this.ShowData();
            }
        }

        #endregion

        #region チェックボックス

        /// --------------------------------------------------
        /// <summary>
        /// 全便対象
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>K.Tsutsumi 2019/09/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private void chkAllShip_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllShip.Checked)
            {
                // 全便対象
                this.btnRefShip.Enabled = false;
                this.lblSelectShip.Text = string.Empty;
                this._nonyusakiCDs = null;
            }
            else
            {
                this.btnRefShip.Enabled = true;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Zhou 2023/12/04</create>
        /// <update></update>
        private void chkCreateDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCreateDate.Checked)
            {
                // 登録日時
                this.mpCreateDateStart.Enabled = true;
                this.mpCreateDateEnd.Enabled = true;
            }
            else
            {
                this.mpCreateDateStart.Enabled = false;
                this.mpCreateDateEnd.Enabled = false;

            }
        }


        /// --------------------------------------------------
        /// <summary>
        /// 設定納期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Zhou 2023/12/04</create>
        /// <update></update>
        private void chkSetteiDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSetteiDate.Checked)
            {
                // 設定納期
                this.mpSetteiDateStart.Enabled = true;       
                this.mpSetteiDateEnd.Enabled = true;

            }
            else
            {
                this.mpSetteiDateStart.Enabled = false;
                this.mpSetteiDateEnd.Enabled = false;
            }

        }



        #endregion

        #region フォーム

        #region OnClosing

        /// --------------------------------------------------
        /// <summary>
        /// OnClosing
        /// </summary>
        /// <param name="e">CancelEventArgs</param>
        /// <create>H.Tsuji 2019/09/09</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                base.OnClosing(e);

                // フォームを閉じる前にグリッド幅を設定ファイルに保存する
                if (!e.Cancel)
                {
                    if (!this.SaveGridSetting(this.shtMeisai, this._iniSectionPrefix))
                    {
                        this.ShowMessage("A9999999076");
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #endregion

        #endregion

        #region 編集内容実行

        #region 制御メソッド

        /// --------------------------------------------------
        /// <summary>
        /// 編集内容実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>J.Chen 2024/10/25</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            bool ret = base.RunEdit();
            try
            {
                if (ret)
                {
                    // グリッドクリア
                    this.SheetClear();
                }
                EditMode = SystemBase.EditMode.None; //ファンクションキー押下時に決定するため、状態を元に戻す
                return ret;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region コンディション取得

        /// --------------------------------------------------
        /// <summary>
        /// コンディションの取得
        /// </summary>
        /// <returns>コンディション</returns>
        /// <create>J.Chen 2024/10/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondT01 GetCondition()
        {
            CondT01 cond = new CondT01(this.UserInfo);

            return cond;
        }

        #endregion

        #region 修正処理

        /// --------------------------------------------------
        /// <summary>
        /// 修正処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>J.Chen 2024/10/25</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                DataTable dt = (this.shtMeisai.DataSource as DataTable).Copy();
                DataTable dtUpdate = this.GetDataTehaiShokaiMeisaiFilter(dt);   // 更新データ抽出

                if (dt == null || dtUpdate == null)
                {
                    // 更新対象となる行がありません。
                    this.ShowMessage("S0100070003");
                    return false;
                }

                DataSet ds = new DataSet();
                ds.Tables.Add(dtUpdate);

                // DB更新
                CondT01 cond = this.GetCondition();
                ConnT01 conn = new ConnT01();
                string errMsgID;
                string[] args;

                if (!conn.UpdTehaiJohoRireki(cond, ds, out errMsgID, out args))
                {
                    if (!string.IsNullOrEmpty(errMsgID))
                    {
                        this.ShowMessage(errMsgID, args);
                    }
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
            finally
            {
                this.shtMeisai.Redraw = true;
            }
        }

        #endregion

        #region 登録データ取得
        /// --------------------------------------------------
        /// <summary>
        /// 手配情報取得(フィルター抽出)
        /// </summary>
        /// <param name="dtSrc">取得元明細情報</param>
        /// <param name="state">抽出条件</param>
        /// <returns>納入先マスタテーブル</returns>
        /// <create>J.Chen 2024/10/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetDataTehaiShokaiMeisaiFilter(DataTable dtSrc)
        {
            try
            {
                DataTable dt = dtSrc.Clone();
                string name = ComDefine.DTTBL_UPDATE;

                foreach (DataRow row in dtSrc.Rows)
                {
                    DataRow matchingRow = null;
                    foreach (DataRow tempRow in _tempDt.Rows)
                    {
                        bool renkeiNoMatches = row[Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO].Equals(tempRow[Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO]);
                        bool tagNoMatches = (row[Def_T_SHUKKA_MEISAI.TAG_NO] == DBNull.Value && tempRow[Def_T_SHUKKA_MEISAI.TAG_NO] == DBNull.Value) ||
                                              (row[Def_T_SHUKKA_MEISAI.TAG_NO] != DBNull.Value && row[Def_T_SHUKKA_MEISAI.TAG_NO].Equals(tempRow[Def_T_SHUKKA_MEISAI.TAG_NO]));

                        if (renkeiNoMatches && tagNoMatches)
                        {
                            matchingRow = tempRow;
                            break;
                        }
                    }

                    if (matchingRow != null)
                    {
                        if (!row[Def_T_TEHAI_MEISAI.TEHAI_RIREKI].Equals(matchingRow[Def_T_TEHAI_MEISAI.TEHAI_RIREKI]))
                        {
                            dt.ImportRow(row);
                        }
                    }
                }

                if (dt.Rows.Count == 0) return null;
                dt.TableName = name;

                return dt;
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
