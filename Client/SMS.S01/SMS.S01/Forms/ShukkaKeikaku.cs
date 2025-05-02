using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using DSWUtil;
using DSWControl.DSWComboBox;
using WsConnection;
using WsConnection.WebRefCommon;
using WsConnection.WebRefAttachFile;
using Commons;
using SystemBase.Controls;
using SystemBase.Util;
using GrapeCity.Win.ElTabelle;
using ElTabelleHelper;
using XlsCreatorHelper;

using WsConnection.WebRefS01;
using SMS.P02.Forms;
using SMS.E01;
using XlsxCreatorHelper;
using SMS.S01.Properties;
using GrapeCity.Win.ElTabelle.Editors;
using System.Linq;

namespace SMS.S01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 出荷計画取込
    /// </summary>
    /// <create>H.Tajimi 2018/10/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ShukkaKeikaku : SystemBase.Forms.CustomOrderForm
    {
        #region フィールド

        /// --------------------------------------------------
        /// <summary>
        /// 出荷計画Excel状態格納用 DataTable
        /// </summary>
        /// <create>H.Tajimi 2018/10/31</create>
        /// <update></update>
        /// --------------------------------------------------
        DataTable _dtShippingPlanExcelType = null;
        /// --------------------------------------------------
        /// <summary>
        /// 有償/無償フラグ値格納用 DataTable
        /// </summary>
        /// <create>H.Tajimi 2018/10/31</create>
        /// <update></update>
        /// --------------------------------------------------
        DataTable _dtEstimate = null;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷元マスタ DataTable
        /// </summary>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        DataTable _dtShipFrom = null;
        /// --------------------------------------------------
        /// <summary>
        /// フィールド長リスト
        /// </summary>
        /// <create>D.Okumura 2020/09/15</create>
        /// <update></update>
        /// --------------------------------------------------
        Dictionary<string, int> _listFieldLength = null;
        /// --------------------------------------------------
        /// <summary>
        /// 物件名
        /// </summary>
        /// <create>R.Miyoshi 2023/07/14</create>
        /// --------------------------------------------------
        public string BukkenNameText { get; set; }
        /// --------------------------------------------------
        /// <summary>
        /// 更新中
        /// </summary>
        /// <create>T.SASAYAMA 2023/08/02</create>
        /// --------------------------------------------------
        public bool isUpdating = false;
        /// --------------------------------------------------
        /// <summary>
        /// 制御フラグ
        /// </summary>
        /// <create>T.SASAYAMA 2023/08/03</create>
        /// <update></update>
        public bool isChanged = false;
        /// --------------------------------------------------
        /// <summary>
        /// 制御フラグ
        /// </summary>
        /// <create>T.SASAYAMA 2023/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isDeleteRecord = false;
        /// --------------------------------------------------
        /// <summary>
        /// 検索時や取込時のシートを保存するためのテーブル
        /// </summary>
        /// <create>T.SASAYAMA 2023/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable preValuesTable;
        /// --------------------------------------------------
        /// <summary>
        /// 一時保存するためのテーブル
        /// </summary>
        /// <create>T.SASAYAMA 2023/09/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable tempTable;
        /// --------------------------------------------------
        /// <summary>
        /// コピーしてるか判定
        /// </summary>
        /// <create>T.SASAYAMA 2023/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool isCellCopied = false;
        /// --------------------------------------------------
        /// <summary>
        /// コピー行
        /// </summary>
        /// <create>T.SASAYAMA 2023/09/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private int CopyRow;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷先選択肢
        /// </summary>
        /// <create>T.SASAYAMA 2023/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private List<string> shukkaOptions;
        /// --------------------------------------------------
        /// <summary>
        /// 物件名
        /// </summary>
        /// <create>T.SASAYAMA 2023/08/23</create>
        /// --------------------------------------------------
        public string BukkenName { get; set; }
        /// --------------------------------------------------
        /// <summary>
        /// 物件管理No
        /// </summary>
        /// <create>T.SASAYAMA 2023/08/24</create>
        /// --------------------------------------------------
        public string BukkenNo { get; set; }
        /// --------------------------------------------------
        /// <summary>
        /// 荷受CD
        /// </summary>
        /// <create>T.SASAYAMA 2023/08/23</create>
        /// --------------------------------------------------
        public string ConsignCD { get; set; }
        /// --------------------------------------------------
        /// <summary>
        /// 運賃梱包製番
        /// </summary>
        /// <create>T.SASAYAMA 2023/08/23</create>
        /// --------------------------------------------------
        public string ShipSeiban { get; set; }
        /// --------------------------------------------------
        /// <summary>
        /// 出荷計画メール用ユーザマスタ
        /// </summary>
        /// <create>Y.Gwon 2023/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtPlanningUser = null;
        /// --------------------------------------------------
        /// <summary>
        /// ログインユーザのメールアドレス
        /// </summary>
        /// <create>Y.Gwon 2023/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _mailAddress = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// メール送信日付
        /// </summary>
        /// <create>Y.Gwon 2023/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _mailDate = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 荷受先
        /// </summary>
        /// <create>T.SASAYAMA 2023/08/29</create>
        /// --------------------------------------------------
        public string ConsignName { get; set; }
        /// --------------------------------------------------
        /// <summary>
        /// リビジョン
        /// </summary>
        /// <create>T.SASAYAMA 2023/08/29</create>
        /// --------------------------------------------------
        public string Revision { get; set; }
        /// --------------------------------------------------
        /// <summary>
        /// リビジョンのバージョン
        /// </summary>
        /// <create>T.SASAYAMA 2023/08/29</create>
        /// --------------------------------------------------
        public string revVersion { get; set; }
        /// --------------------------------------------------
        /// <summary>
        /// 外部連携モード
        /// </summary>
        /// <create>J.Chen 2023/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _dialogMode = false;
        /// --------------------------------------------------
        /// <summary>
        /// 物件名 - 外部連携用
        /// </summary>
        /// <create>J.Chen 2023/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _bukkenName = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 運賃・梱包製番 - 外部連携用
        /// </summary>
        /// <create>J.Chen 2023/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _shipSeiban = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 荷受先CD - 外部連携用
        /// </summary>
        /// <create>J.Chen 2023/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _consignCD = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 新規かどうか
        /// </summary>
        /// <create>J.Chen 2023/10/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isNewData = false;
        /// --------------------------------------------------
        /// <summary>
        /// 排他中かどうか
        /// </summary>
        /// <create>J.Chen 2024/01/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isHaita = false;
        /// --------------------------------------------------
        /// <summary>
        /// 読み取り専用かどうか
        /// </summary>
        /// <create>J.Chen 2024/01/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isReadOnly = false;
        /// --------------------------------------------------
        /// <summary>
        /// シートカラム数
        /// </summary>
        /// <create>J.Chen 2024/01/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private int colIndexMax;

        #endregion

        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// 物件名が設定されている行位置
        /// </summary>
        /// <create>H.Tajimi 2018/10/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_BUKKEN_NAME_ROW_POS = 0;
        /// --------------------------------------------------
        /// <summary>
        /// 運賃・梱包 製番が設定されている行位置
        /// </summary>
        /// <create>H.Tajimi 2018/10/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_SHIP_SEIBAN_ROW_POS = 3;
        /// --------------------------------------------------
        /// <summary>
        /// 案件管理No.が設定されている行位置
        /// </summary>
        /// <create>H.Tajimi 2018/10/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_SHIP_NO_ROW_POS = 3;
        /// --------------------------------------------------
        /// <summary>
        /// データ開始行
        /// </summary>
        /// <create>H.Tajimi 2018/10/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_DATA_START_IDX = 7;
        /// --------------------------------------------------
        ///         /// --------------------------------------------------
        /// <summary>
        /// 行操作（コピー・貼付）のセル範囲開始位置
        /// </summary>
        /// <create>H.Tajimi 2015/11/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_COPY_START = 2;
        /// --------------------------------------------------
        /// <summary>
        /// 行操作（コピー・貼付）のセル範囲終端位置
        /// </summary>
        /// <create>H.Tajimi 2015/11/20</create>
        /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
        /// <update>T.Nakata 2018/11/13 手配業務対応</update>
        /// <update>H.Iimuro 2022/10/04 図面/型式２の追加対応</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_COPY_END = 15;
        /// --------------------------------------------------
        /// <summary>
        /// Rev初期値
        /// </summary>
        /// <create>J.Chen 2024/01/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string REV_INITIAL_VALUE = "000";
       
        #endregion

        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// Excelの文字属性タイプ
        /// </summary>
        /// <create>H.Tajimi 2018/10/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum ExcelInputAttrType
        {
            ShoriFlag = 0,
            AlphaNum = 1,
            WideString = 2,
            Numeric = 3,
            EstimateFlag = 4,
            Date = 5,
        }

        /// --------------------------------------------------
        /// <summary>
        /// 処理フラグ
        /// </summary>
        /// <create>H.Tajimi 2018/10/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum ShoriFlag
        {
            /// --------------------------------------------------
            /// <summary>
            /// 変更なし
            /// </summary>
            /// <create>H.Tajimi 2018/10/30</create>
            /// <update></update>
            /// --------------------------------------------------
            NoChange = 0,
            /// --------------------------------------------------
            /// <summary>
            /// 追加/更新
            /// </summary>
            /// <create>H.Tajimi 2018/10/30</create>
            /// <update></update>
            /// --------------------------------------------------
            InsOrUpd = 1,
            /// --------------------------------------------------
            /// <summary>
            /// 削除
            /// </summary>
            /// <create>H.Tajimi 2018/10/30</create>
            /// <update></update>
            /// --------------------------------------------------
            Del = 9,
        }

        /// --------------------------------------------------
        /// <summary>
        /// 有償/無償フラグ
        /// </summary>
        /// <create>T.SASAYAMA 2023/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum EstimateFlag
        {
            /// --------------------------------------------------
            /// <summary>
            /// 削除2
            /// </summary>
            /// <create>T.SASAYAMA 2023/08/09</create>
            /// <update></update>
            /// --------------------------------------------------
            Mushou = 0,
            /// --------------------------------------------------
            /// <summary>
            /// 有償
            /// </summary>
            /// <create>T.SASAYAMA 2023/08/29</create>
            /// <update></update>
            /// --------------------------------------------------
            Yushou = 1,
        }

        /// --------------------------------------------------
        /// <summary>
        /// 列インデックス
        /// </summary>
        /// <create>T.SASAYAMA 2023/08/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum SHEET_COL
        {
            SHORI_FLAG = 0,         // 処理フラグ
            SHIP,                   // 便名
            ESTIMATE_FLAG,          // 有償/無償
            TRANSPORT_FLAG,         // AIR/SHIP
            SHIP_FROM,              // 出荷元
            SHIP_TO,                // 出荷先
            SHIP_DATE,              // 出荷日
            //SHIP_SEIBAN,          // 運賃・梱包
            SEIBAN,                 // 製番
            KISHU,                  // 機種
            NAIYO,                  // 内容
            TOUCHAKUYOTEI_DATE,     // 到着予定日
            KIKAI_PARTS,            // 機械パーツ
            SEIGYO_PARTS,           // 制御パーツ
            BIKO,                   // 備考
            BUKKEN_NAME,            // 物件名
            SHIP_NO,                // 
            SHIP_SEIBAN,            // 
            SHIP_FROM_CD,           // 
            CONSIGN_CD,             // 荷受先
        }

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
        /// <create>H.Tajimi 2018/10/30</create>
        /// <update>J.Chen 2023/08/29 外部連携</update>
        /// <update></update>
        /// --------------------------------------------------
        public ShukkaKeikaku(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
            : base(userInfo, menuCategoryID, menuItemID, title)
        {
            InitializeComponent();
            this._bukkenName = string.Empty;
            this._shipSeiban = string.Empty;
            this._consignCD = string.Empty;
            this._dialogMode = false;
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ - 外部連携用
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="title">画面タイトル</param>
        /// <param name="ecsQuota"></param>
        /// <param name="ecsNo"></param>
        /// <create>J.Chen 2023/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public ShukkaKeikaku(UserInfo userInfo, string title, string bukkenName, string shipSeiban, string consignCD)
            : base(userInfo, title)
        {
            InitializeComponent();
            this._bukkenName = bukkenName;
            this._shipSeiban = shipSeiban;
            this._consignCD = consignCD;
            this._dialogMode = true;
        }
        
        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>H.Tajimi 2018/10/30</create>
        /// <update>H.Tajimi 2020/04/14 出荷元マスタチェック追加</update>
        /// <update>D.Okumura 2020/09/15 運賃・梱包 製番のチェックを7桁→汎用マスタ設定値へ変更</update>
        /// <update>R.Miyoshi 2023/07/14 物件名、荷受先の各cboの初期化追加</update>
        /// <update>J.Chen 2023/08/29 外部連携</update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();
            try
            {
                // フォームの設定
                this.IsCloseQuestion = true;

                // 物件名コンボボックスの初期化
                this.InitializeComboBukken();

                // 荷受先名コンボボックスの初期化
                this.InitializeComboNiukesaki();

                // シートの初期化
                this.InitializeSheet(this.shtMeisai);

                // 汎用マスタ取得
                // 出荷計画Excel状態
                this._dtShippingPlanExcelType = this.GetCommon(SHIPPING_PLAN_EXCEL_TYPE.GROUPCD).Tables[Def_M_COMMON.Name];
                // 有償/無償フラグ
                this._dtEstimate = this.GetCommon(ESTIMATE_FLAG.GROUPCD).Tables[Def_M_COMMON.Name];
                // フィールド長
                this._listFieldLength = this.GetCommon(FIELD_LENGTH.GROUPCD, Def_M_COMMON.ITEM_CD, row => ComFunc.GetFldToInt32(row, Def_M_COMMON.VALUE1));
                
                // 出荷元マスタ取得
                var cond = new CondS01(this.UserInfo);
                var conn = new ConnS01();
                cond.UnusedFlag = UNUSED_FLAG.USED_VALUE1;
                var ds = conn.GetInitShukkaKeikaku(cond);
                this._dtShipFrom = ds.Tables[Def_M_SHIP_FROM.Name];

                this.EditMode = SystemBase.EditMode.Insert;

                if (this._dialogMode)
                {
                    // 検索条件設定
                    this.cboBukkenNameTorikomi.Text = this._bukkenName;
                    this.txtShipSeiban.Text = this._shipSeiban;
                    this.cboNiukesaki.SelectedValue = this._consignCD;
                    if (this.cboNiukesaki.SelectedValue != null || !string.IsNullOrEmpty(this.txtShipSeiban.Text))
                    {
                        this.btnKaishi_Click(this, new EventArgs());
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
        /// フォームが初めて表示されるたびにコントロールの初期化するメソッド
        /// </summary>
        /// <create>H.Tajimi 2018/10/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();
            try
            {
                // 初期フォーカスの設定
                this.btnReference.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region シート初期化

        /// --------------------------------------------------
        /// <summary>
        /// シート初期化
        /// </summary>
        /// <param name="sheet">ElTabelleSheet</param>
        /// <create>H.Tajimi 2018/10/30</create>
        /// <update>R.Miyoshi Seiban以下シート項目追加</update>
        /// --------------------------------------------------
        protected override void InitializeSheet(Sheet sheet)
        {
            base.InitializeSheet(sheet);
            sheet.AllowUserToAddRows = true;
            sheet.KeepHighlighted = true;
            //sheet.SelectionType = SelectionType.Single;
            //sheet.ViewMode = ViewMode.Row;
            try
            {
                int colIndex = 0;
                var txtEditor = ElTabelleSheetHelper.NewTextEditor();
                var dateEditor = ElTabelleSheetHelper.NewDateEditor();

                txtEditor.MaxLength = 500;
                dateEditor.Format = new DateFormat("yyyy/MM/dd", string.Empty, string.Empty);
                dateEditor.DisplayFormat = new DateDisplayFormat("yy/MM/dd (ddd)", string.Empty, string.Empty);

                dateEditor.ShowDropDown = Visibility.ShowAlways;

                //dateEditor.Format = new GrapeCity.Win.ElTabelle.DateFormat("yyyy/MM", "", "");
                this.SetElTabelleColumn(sheet, colIndex, Resources.ShukkaKeikaku_ShoriFlag, false, false, ComDefine.FLD_EXCEL_SHORI_FLAG, CreateSuperiorComboEditor(SHIPPING_PLAN_EXCEL_TYPE.GROUPCD, true), 120);
                this.shtMeisai.Columns[colIndex].Enabled = true;
                colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.ShukkaKeikaku_Ship, false, false, Def_M_NONYUSAKI.SHIP, txtEditor, 100);
                this.shtMeisai.Columns[colIndex].Enabled = true;
                colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.ShukkaKeikaku_EstimateFlag, false, false, Def_M_NONYUSAKI.ESTIMATE_FLAG, CreateSuperiorEstimateEditor(Def_M_NONYUSAKI.ESTIMATE_FLAG), 80);
                this.shtMeisai.Columns[colIndex].Enabled = true;
                colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.ShukkaKeikaku_TransportFlag, false, false, Def_M_NONYUSAKI.TRANSPORT_FLAG, CreateSuperiorComboEditor(AIRSHIP_FLAG.GROUPCD, false), 120);
                this.shtMeisai.Columns[colIndex].Enabled = true;
                colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.ShukkaKeikaku_ShukkaFrom, false, false, Def_M_NONYUSAKI.SHIP_FROM, InitializeComboShukkamoto(), 120);
                this.shtMeisai.Columns[colIndex].Enabled = true;
                colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.ShukkaKeikaku_ShukkaTo, false, false, Def_M_NONYUSAKI.SHIP_TO, InitializeComboShukkasaki(), 80);
                this.shtMeisai.Columns[colIndex].Enabled = true;
                colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.ShukkaKeikaku_ShukkaDate, false, false, Def_M_NONYUSAKI.SHIP_DATE, dateEditor, 100);
                this.shtMeisai.Columns[colIndex].Enabled = true;
                colIndex++;
                //this.SetElTabelleColumn(sheet, colIndex, Resources.ShukkaKeikaku_ShipSeiban, false, true, Def_M_NONYUSAKI.SHIP_SEIBAN, txtEditor, 100);
                //this.shtMeisai.Columns[colIndex].Enabled = false;
                //colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.ShukkaKeikaku_Seiban, false, false, Def_M_NONYUSAKI.SEIBAN, txtEditor, 100);
                this.shtMeisai.Columns[colIndex].Enabled = true;
                colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.ShukkaKeikaku_Model, false, false, Def_M_NONYUSAKI.KISHU, txtEditor, 100);
                this.shtMeisai.Columns[colIndex].Enabled = true;
                colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.ShukkaKeikaku_Naiyo, false, false, Def_M_NONYUSAKI.NAIYO, txtEditor, 100);
                this.shtMeisai.Columns[colIndex].Enabled = true;
                colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.ShukkaKeikaku_TouchakuyoteiDate, false, false, Def_M_NONYUSAKI.TOUCHAKUYOTEI_DATE, dateEditor, 100);
                this.shtMeisai.Columns[colIndex].Enabled = true;
                //this.shtMeisai.Columns[colIndex].Editor = "yy/mm/dd(ddd)";
                colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.ShukkaKeikaku_KikaiParts, false, false, Def_M_NONYUSAKI.KIKAI_PARTS, txtEditor, 100);
                this.shtMeisai.Columns[colIndex].Enabled = true;
                colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.ShukkaKeikaku_SeigyoParts, false, false, Def_M_NONYUSAKI.SEIGYO_PARTS, txtEditor, 100);
                this.shtMeisai.Columns[colIndex].Enabled = true;
                colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.ShukkaKeikaku_Remarks, false, false, Def_M_NONYUSAKI.BIKO, txtEditor, 100);
                this.shtMeisai.Columns[colIndex].Enabled = true;
                colIndex++;
                                
                // 非表示列
                this.SetElTabelleColumn(sheet, colIndex++, Def_M_BUKKEN.BUKKEN_NAME, true, false, Def_M_BUKKEN.BUKKEN_NAME, txtEditor, 40);
                //this.SetElTabelleColumn(sheet, colIndex++, ComDefine.FLD_EXCEL_SHORI_FLAG, true, false, ComDefine.FLD_EXCEL_SHORI_FLAG, txtEditor, 40);
                //this.SetElTabelleColumn(sheet, colIndex++, Def_M_NONYUSAKI.ESTIMATE_FLAG, true, false, Def_M_NONYUSAKI.ESTIMATE_FLAG, txtEditor, 40);
                this.SetElTabelleColumn(sheet, colIndex++, Def_M_NONYUSAKI.SHIP_NO, true, false, Def_M_NONYUSAKI.SHIP_NO, txtEditor, 40);
                this.SetElTabelleColumn(sheet, colIndex++, Def_M_NONYUSAKI.SHIP_SEIBAN, true, false, Def_M_NONYUSAKI.SHIP_SEIBAN, txtEditor, 40);
                this.SetElTabelleColumn(sheet, colIndex++, Def_M_NONYUSAKI.SHIP_FROM_CD, true, false, Def_M_NONYUSAKI.SHIP_FROM_CD, txtEditor, 40);
                this.SetElTabelleColumn(sheet, colIndex++, Def_M_NONYUSAKI.CONSIGN_CD, true, false, Def_M_NONYUSAKI.CONSIGN_CD, txtEditor, 40);
                //this.SetElTabelleColumn(sheet, colIndex++, "COPY_FLAG", true, false, null, txtEditor, 40);

                // グリッド線
                sheet.GridLine = new GridLine(GridLineStyle.Thin, Color.DarkGray);
                // Disable時の設定
                for (int i = 0; i < sheet.Columns.Count; i++)
                {
                    sheet.Columns[i].DisabledBackColor = Color.FromArgb(223, 223, 223);
                    sheet.Columns[i].DisabledForeColor = Color.Black;
                }
                this.shtMeisai.Enabled = false;
                this.colIndexMax = colIndex;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion
            
        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア
        /// </summary>
        /// <create>H.Tajimi 2018/10/31</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // クリア
                this.cboBukkenNameTorikomi.SelectedIndex = -1;
                this.cboBukkenNameTorikomi.Text = null;
                this.cboNiukesaki.SelectedIndex = -1;
                this.cboNiukesaki.Text = null;
                this.txtShipSeiban.Clear();
                this.cboBukkenNameTorikomi.Enabled = true;
                // グリッドのクリア
                this.SheetClear();
                // 明細データ
                this.txtExcel.Text = string.Empty;

                // 検索条件のロック解除
                this.grpSearch.Enabled = true;
                // ファンクションボタンの切り替え
                this.ChangeFunctionButton(false);
                // フォーカス移動
                this.btnReference.Focus();

                if (this.BukkenNameText != null)
                {
                    this.cboBukkenName.Text = this.BukkenNameText;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region コンボボックスの初期化

        #region 物件名コンボボックスの初期化

        /// --------------------------------------------------
        /// <summary>
        /// 物件名コンボボックスの初期化
        /// </summary>
        /// <create>R.Miyoshi 2023/07/14</create>
        /// <update>J.Chen 2023/09/05</update>
        /// --------------------------------------------------
        private void InitializeComboBukken()
        {
            var conn = new ConnS01();
            var cond = new CondS01(this.UserInfo);
            var ds = conn.GetBukkenName();
            if (!UtilData.ExistsData(ds, Def_M_BUKKEN.Name))
            {
                return;
            }

            // 物件名コンボボックスの初期化
            var dt = ds.Tables[Def_M_BUKKEN.Name];

            this.cboBukkenNameTorikomi.ValueMember = Def_M_BUKKEN.BUKKEN_NO;
            this.cboBukkenNameTorikomi.DisplayMember = Def_M_BUKKEN.BUKKEN_NAME;
            this.cboBukkenNameTorikomi.DataSource = dt;
                        
            if (UtilData.ExistsData(dt))
            {
                this.cboBukkenNameTorikomi.SelectedValue = decimal.MinValue;
            }
            else
            {
                this.cboBukkenNameTorikomi.SelectedIndex = -1;
            }
        }

        #endregion

        #region 荷受先名コンボボックスの初期化

        /// --------------------------------------------------
        /// <summary>
        /// 荷受先名コンボボックスの初期化
        /// </summary>
        /// <create>R.Miyoshi 2023/07/18</create>
        /// --------------------------------------------------
        private void InitializeComboNiukesaki()
        {
            var conn = new ConnS01();
            var cond = new CondS01(this.UserInfo);
            var ds = conn.GetConsignList();
            if (!UtilData.ExistsData(ds, Def_M_CONSIGN.Name))
            {
                return;
            }

            // 物件名コンボボックスの初期化
            var dt = ds.Tables[Def_M_CONSIGN.Name];

            this.cboNiukesaki.ValueMember = Def_M_CONSIGN.CONSIGN_CD;
            this.cboNiukesaki.DisplayMember = Def_M_CONSIGN.NAME;
            this.cboNiukesaki.DataSource = dt;
                       
            if (UtilData.ExistsData(dt))
            {
                this.cboNiukesaki.SelectedValue = decimal.MinValue;
            }
            else
            {
                this.cboNiukesaki.SelectedIndex = -1;
            }
        }
        
        #endregion

        #region 明細用コンボボックス作成(データバインド)
        /// --------------------------------------------------
        /// <summary>
        /// 明細用コンボボックス作成(データバインド)
        /// </summary>
        /// <param name="GROUP_CD"></param>
        /// <param name="isValue"></param>
        /// <returns>SuperiorComboEditor</returns>
        /// <create>T.SASAYMA 2023/07/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private SuperiorComboEditor CreateSuperiorComboEditor(string GROUP_CD, bool isValue)
        {
            if (isValue)
            {
                var cboEditor = ElTabelleSheetHelper.NewSuperiorComboEditor(this.GetCommon(GROUP_CD).Tables[Def_M_COMMON.Name],
                Def_M_COMMON.ITEM_NAME, Def_M_COMMON.VALUE1, true);
                cboEditor.ValueAsIndex = false;
                return cboEditor;
            }
            else
            {
                var cboEditor = ElTabelleSheetHelper.NewSuperiorComboEditor(this.GetCommon(GROUP_CD).Tables[Def_M_COMMON.Name],
                Def_M_COMMON.ITEM_NAME, Def_M_COMMON.ITEM_NAME, true);
                cboEditor.ValueAsIndex = false;
                return cboEditor;
            }
        }
        #endregion

        #region 明細用コンボボックス作成(データバインド)
        /// --------------------------------------------------
        /// <summary>
        /// 明細用コンボボックス作成(データバインド)
        /// </summary>
        /// <param name="GROUP_CD"></param>
        /// <param name="isValue"></param>
        /// <returns>SuperiorComboEditor</returns>
        /// <create>T.SASAYMA 2023/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private SuperiorComboEditor CreateSuperiorEstimateEditor(string GROUP_CD)
        {
            DataTable dt = this.GetCommon(GROUP_CD).Tables[Def_M_COMMON.Name];

            // 最後の行を削除
            if (dt.Rows.Count > 0)
            {
                dt.Rows.RemoveAt(dt.Rows.Count - 1);
            }

            var cboEditor = ElTabelleSheetHelper.NewSuperiorComboEditor(dt,
                        Def_M_COMMON.ITEM_NAME, Def_M_COMMON.ITEM_NAME, true);
            cboEditor.ValueAsIndex = true;
            return cboEditor;
        }
        #endregion

        #region 出荷元コンボボックスの初期化

        /// --------------------------------------------------
        /// <summary>
        /// 出荷元コンボボックスの初期化
        /// </summary>
        /// <create>T.SASAYAMA 2023/07/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private SuperiorComboEditor InitializeComboShukkamoto()
        {
            var conn = new ConnS01();
            var ds = conn.GetShukkamoto();

            // 出荷元コンボボックスの初期化
            var dt = ds.Tables[Def_M_SHIP_FROM.Name];

            // シートのコンボボックスセット
            var cboEditor = ElTabelleSheetHelper.NewSuperiorComboEditor(dt,
            Def_M_SHIP_FROM.SHIP_FROM_NAME, Def_M_SHIP_FROM.SHIP_FROM_NAME, true);
            cboEditor.ValueAsIndex = false;

            return cboEditor;
        }

        #endregion

        #region 出荷先コンボボックスの初期化

        /// --------------------------------------------------
        /// <summary>
        /// 出荷元コンボボックスの初期化
        /// </summary>
        /// <create>T.SASAYAMA 2023/07/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private SuperiorComboEditor InitializeComboShukkasaki()
        {
            var conn = new ConnS01();
            var ds = conn.GetShukkasaki();

            // 出荷元コンボボックスの初期化
            var dt = ds.Tables[Def_M_SELECT_ITEM.Name];

            shukkaOptions = GetComboBoxOptions(dt, Def_M_SELECT_ITEM.ITEM_NAME);

            // シートのコンボボックスセット
            var cboEditor = ElTabelleSheetHelper.NewSuperiorComboEditor(dt,
            Def_M_SELECT_ITEM.ITEM_NAME, Def_M_SELECT_ITEM.ITEM_NAME, true);
            cboEditor.ValueAsIndex = false;

            return cboEditor;
        }

        #endregion

        #region コンボボックスの選択肢取得

        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスの選択肢取得
        /// </summary>
        /// <returns>List<string></returns>
        /// <create>T.SASAYAMA 2023/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private List<string> GetComboBoxOptions(DataTable dt, string columnName)
        {
            var options = new List<string>();

            foreach (DataRow row in dt.Rows)
            {
                string option = row[columnName].ToString();
                if (!string.IsNullOrEmpty(option))
                {
                    options.Add(option);
                }
            }

            return options;
        }

        #endregion

        #endregion

        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>H.Tajimi 2018/10/31</create>
        /// <update>J.Chen 2024/02/21 ReadOnlyモード解除</update>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            // グリッドクリア
            try
            {
                this.shtMeisai.Redraw = false;
                if (0 < this.shtMeisai.MaxRows)
                {
                    this.shtMeisai.TopLeft = new Position(0, this.shtMeisai.TopLeft.Row);
                }
                this.shtMeisai.DataSource = null;
                this.shtMeisai.MaxRows = 0;
                this.shtMeisai.Enabled = false;
                this.isCellCopied = false;
                this.txtRevision.Text = null;
                this.txtBoxAssign.Text = null;

                this._isReadOnly = false;
                this.txtShipSeiban.ReadOnly = false;
                this.txtBoxAssign.ReadOnly = false;
                this.cboNiukesaki.Enabled = true;
                try
                {
                    this.shtMeisai.ActivePosition = new Position(0, 0);
                }
                catch
                { }
            }
            finally
            {
                this.shtMeisai.Redraw = true;
            }
        }

        #endregion

        #region ファンクションバーのEnabled切替

        ///// --------------------------------------------------
        ///// <summary>
        ///// ファンクションバーのEnabled切替
        ///// </summary>
        ///// <param name="isEnabled">Enabled状態</param>
        ///// <create>H.Tajimi 2018/10/31</create>
        ///// <update></update>
        ///// --------------------------------------------------
        //private void ChangeFunctionButton(bool isEnabled)
        //{
        //    this.fbrFunction.F01Button.Enabled = isEnabled;
        //    this.fbrFunction.F06Button.Enabled = isEnabled;
        //}

        /// --------------------------------------------------
        /// <summary>
        /// ファンクションバーのEnabled切替
        /// </summary>
        /// <param name="isEnabled">Enabled状態</param>
        /// <create>T.SASAYAMA 2023/08/22</create>
        /// <update>J.Chen 2023/08/29 外部連携</update>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeFunctionButton(bool isSearchMode)
        {
            if (this.shtMeisai.Enabled == true)
            {
                this.fbrFunction.F01Button.Enabled = true;
                this.fbrFunction.F03Button.Enabled = true;
                this.fbrFunction.F06Button.Enabled = true && !this._dialogMode;
                this.fbrFunction.F07Button.Enabled = true && !this._dialogMode;
                this.fbrFunction.F08Button.Enabled = true;
                this.fbrFunction.F09Button.Enabled = true;
            }
            else if (this.shtMeisai.Enabled == false)
            {
                this.fbrFunction.F01Button.Enabled = false;
                this.fbrFunction.F03Button.Enabled = false;
                this.fbrFunction.F06Button.Enabled = false;
                this.fbrFunction.F08Button.Enabled = false;
                this.fbrFunction.F09Button.Enabled = false;
            }
            if (isSearchMode == true)
            {
                this.btnSearch.Enabled = false;
                this.fbrFunction.F02Button.Enabled = true;
                this.fbrFunction.F10Button.Enabled = true;
                this.fbrFunction.F11Button.Enabled = true;
                this.lblSeiban.Enabled = true;
                this.lblBoxAssign.Enabled = true;
            }
            else if (isSearchMode == false)
            {
                this.btnSearch.Enabled = true;
                this.fbrFunction.F02Button.Enabled = false;
                this.fbrFunction.F10Button.Enabled = false;
                this.fbrFunction.F11Button.Enabled = false;
                this.lblSeiban.Enabled = false;
                this.lblBoxAssign.Enabled = false;
            }

        }

        #endregion

        #region 納入先のデータテーブル

        /// --------------------------------------------------
        /// <summary>
        /// 納入先のデータテーブル
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/10/31</create>
        /// <update>H.Tajimi 2020/04/14 出荷元マスタチェック追加</update>
        /// --------------------------------------------------
        private DataTable GetSchemeNonyusaki()
        {
            DataTable dt = new DataTable(Def_M_NONYUSAKI.Name);
            dt.Columns.Add(Def_M_BUKKEN.BUKKEN_NAME, typeof(string));
            dt.Columns.Add(ComDefine.FLD_EXCEL_SHORI_FLAG, typeof(string));
            dt.Columns.Add(ComDefine.FLD_EXCEL_SHORI_FLAG_NAME, typeof(string));
            dt.Columns.Add(Def_M_NONYUSAKI.SHIP, typeof(string));
            dt.Columns.Add(Def_M_NONYUSAKI.ESTIMATE_FLAG, typeof(string));
            dt.Columns.Add(ComDefine.FLD_ESTIMATE_FLAG_NAME, typeof(string));
            dt.Columns.Add(Def_M_NONYUSAKI.TRANSPORT_FLAG, typeof(string));
            dt.Columns.Add(Def_M_NONYUSAKI.SHIP_FROM, typeof(string));
            dt.Columns.Add(Def_M_NONYUSAKI.SHIP_TO, typeof(string));
            dt.Columns.Add(Def_M_NONYUSAKI.SHIP_DATE, typeof(string));
            dt.Columns.Add(Def_M_NONYUSAKI.SHIP_NO, typeof(string));
            dt.Columns.Add(Def_M_NONYUSAKI.SHIP_SEIBAN, typeof(string));
            dt.Columns.Add(Def_M_NONYUSAKI.SHIP_FROM_CD, typeof(string));
            dt.Columns.Add(Def_M_NONYUSAKI.SEIBAN, typeof(string));
            dt.Columns.Add(Def_M_NONYUSAKI.KISHU, typeof(string));
            dt.Columns.Add(Def_M_NONYUSAKI.NAIYO, typeof(string));
            dt.Columns.Add(Def_M_NONYUSAKI.TOUCHAKUYOTEI_DATE, typeof(string)); 
            dt.Columns.Add(Def_M_NONYUSAKI.KIKAI_PARTS, typeof(string));
            dt.Columns.Add(Def_M_NONYUSAKI.SEIGYO_PARTS, typeof(string));
            dt.Columns.Add(Def_M_NONYUSAKI.BIKO, typeof(string));
            
            return dt;
        }

        #endregion

        #region 取込処理

        #region 取込制御部

        /// --------------------------------------------------
        /// <summary>
        /// 取込処理
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/10/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ExecuteImport()
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                // 明細データ(Excel)の入力チェック
                if (string.IsNullOrEmpty(this.txtExcel.Text))
                {
                    // 出荷計画Data(Excel)のFilesが選択されていません。
                    this.ShowMessage("S0100010001");
                    return false;
                }
                // ファイル存在チェック
                if (!File.Exists(this.txtExcel.Text))
                {
                    // 出荷計画Data(Excel)のFilesが存在しません。
                    this.ShowMessage("S0100010002");
                    return false;
                }

                Cursor.Current = Cursors.WaitCursor;
                var dt = this.GetSchemeNonyusaki();
                var dtMessage = ComFunc.GetSchemeMultiMessage();
                bool ret = false;
                var bukkenName = string.Empty;
                var shipSeiban = string.Empty;
                if (Path.GetExtension(this.txtExcel.Text) == ".xls")
                {
                    ret = this.GetExcelDataXls(this.txtExcel.Text, dt, dtMessage, ref bukkenName, ref shipSeiban);
                }
                else
                {
                    ret = this.GetExcelDataXlsx(this.txtExcel.Text, dt, dtMessage, ref bukkenName, ref shipSeiban);
                }
                if (0 < dt.Rows.Count)
                {
                    ret = true;
                    this.cboBukkenNameTorikomi.Text = bukkenName;
                    this.txtShipSeiban.Text = shipSeiban;
                }
                this.shtMeisai.DataSource = dt;

                //現在のシート値を保存
                this.preValuesTable = GetAllRowValuesInTable(this.shtMeisai);


                if (0 < dtMessage.Rows.Count)
                {
                    // 取込出来ないデータがありました。\r\nエラーがあった行は表示されていません。\r\n※エラーの一覧は右クリックでクリップボードにコピーできます。
                    this.ShowMultiMessage(dtMessage, "S0100020033");
                }
                else
                {
                    this.cboBukkenNameTorikomi.Enabled = false;
                    this.shtMeisai.Enabled = true;
                }

                this.ChangeFunctionButton(false);
                this.SetTableDisabled();
                this.fbrFunction.F06Button.Enabled = true;
                this.fbrFunction.F07Button.Enabled = true;

                return ret;
            }
            catch (Exception ex)
            {
                this.shtMeisai.Redraw = true;
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
            finally
            {
                Cursor.Current = preCursor;
            }
        }

        #endregion

        #region 取込実行部

        /// --------------------------------------------------
        /// <summary>
        /// 取込実行部(xls)
        /// </summary>
        /// <param name="filePath">Excelファイルパス</param>
        /// <param name="dt">取り込んだデータ</param>
        /// <param name="dtMessage">エラーメッセージテーブル</param>
        /// <param name="bukkenName">物件名</param>
        /// <param name="shipSeiban">運賃・梱包 製番</param>
        /// <returns>エラーがあったかどうか</returns>
        /// <create>H.Tajimi 2018/10/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool GetExcelDataXls(string filePath, DataTable dt, DataTable dtMessage, ref string bukkenName, ref string shipSeiban)
        {
            using (var xls = new XlsCreator())
            {
                try
                {
                    xls.ReadBook(filePath);
                    int maxRow = xls.MaxData(xlPoint.ptMaxPoint).Height;
                    return this.CheckExcelData(dt, dtMessage, maxRow, xls, false, ref bukkenName, ref shipSeiban);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    xls.CloseBook(false);
                }
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 取込実行部(xlsx)
        /// </summary>
        /// <param name="filePath">Excelファイルパス</param>
        /// <param name="dt">取り込んだデータ</param>
        /// <param name="dtMessage">エラーメッセージテーブル</param>
        /// <param name="bukkenName">物件名</param>
        /// <param name="shipSeiban">運賃・梱包 製番</param>
        /// <returns>エラーがあったかどうか</returns>
        /// <create>H.Tajimi 2018/10/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool GetExcelDataXlsx(string filePath, DataTable dt, DataTable dtMessage, ref string bukkenName, ref string shipSeiban)
        {
            using (var xls = new XlsxCreator())
            {
                try
                {
                    xls.ReadBook(filePath);
                    int maxRow = xls.MaxData(xlMaxEndPoint.xarMaxPoint).Height;
                    return this.CheckExcelData(dt, dtMessage, maxRow, xls, true, ref bukkenName, ref shipSeiban);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    xls.CloseBook(false);
                }
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 値チェック
        /// </summary>
        /// <param name="dt">取り込んだデータ</param>
        /// <param name="dtMessage">エラーメッセージテーブル</param>
        /// <param name="maxRow">最大行位置</param>
        /// <param name="obj">xlsオブジェクト</param>
        /// <param name="isXlsx">xlsxかどうか</param>
        /// <param name="bukkenName">物件名</param>
        /// <param name="shipSeiban">運賃・梱包 製番</param>
        /// <returns>エラーがあったかどうか</returns>
        /// <create>H.Tajimi 2018/10/31</create>
        /// <update>H.Tajimi 2020/04/14 出荷元マスタチェック追加</update>
        /// <update>H.Kawasaki 2020/09/17 運賃・梱包 製番のチェックを7桁→汎用マスタ設定値へ変更</update>
        /// <update>J.Chen 2024/08/19 出荷先桁数変更</update>
        /// --------------------------------------------------
        private bool CheckExcelData(DataTable dt, DataTable dtMessage, int maxRow, object obj, bool isXlsx, ref string bukkenName, ref string shipSeiban)
        {
            Func<int, int, string> fncGetStr = (colIndex, rowIndex) =>
            {
                if (isXlsx)
                {
                    return (obj as XlsxCreator).Pos(colIndex, rowIndex).Str;
                }
                else
                {
                    return (obj as XlsCreator).Pos(colIndex, rowIndex).Str;
                }
            };
            Func<int, int, object> fncGetVal = (colIndex, rowIndex) =>
            {
                if (isXlsx)
                {
                    return (obj as XlsxCreator).Pos(colIndex, rowIndex).Value;
                }
                else
                {
                    return (obj as XlsCreator).Pos(colIndex, rowIndex).Value;
                }
            };

            if (maxRow < 1)
            {
                // 出荷計画Data(Excel)のFilesにDataがありません。
                this.ShowMessage("S0100010003");
                return false;
            }

            bool ret = true;
            string itemName = string.Empty;
            string field = string.Empty;
            int col = 0;
            int row = 0;
            int checkLen = 0;
            DataRow dr = dt.NewRow();
            bool isAddData = true;
            bool is1st = true;
            HashSet<string> shipName = new HashSet<string>();

            // 物件名のチェック
            itemName = Resources.ShukkaKeikaku_BukkenName;
            row = SHEET_BUKKEN_NAME_ROW_POS;
            col = 0;
            checkLen = 60;
            field = Def_M_BUKKEN.BUKKEN_NAME;
            if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, true, true))
            {
                isAddData = false;
            }
            else
            {
                bukkenName = fncGetStr(col, row);
            }

            // 運賃・梱包 製番のチェック
            itemName = Resources.ShukkaKeikaku_ShipSeiban;
            row = SHEET_SHIP_SEIBAN_ROW_POS;
            col = 0;
            checkLen = this._listFieldLength[FIELD_LENGTH.SHIP_SEIBAN_NAME];
            field = Def_M_NONYUSAKI.SHIP_SEIBAN;
            if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.AlphaNum, dtMessage, true, true, true))
            {
                isAddData = false;
            }
            else
            {
                shipSeiban = fncGetStr(col, row);
            }

            // 案件管理Noは空文字登録される


            for (row = SHEET_DATA_START_IDX; row <= maxRow; row++)
            {
                if (string.IsNullOrEmpty(fncGetStr(0, row)))
                {
                    break;
                }

                if (!is1st)
                {
                    dr = dt.NewRow();
                    isAddData = true;
                }

                // 処理フラグのチェック
                itemName = Resources.ShukkaKeikaku_ShoriFlag;
                col = 0;
                checkLen = 1;
                field = ComDefine.FLD_EXCEL_SHORI_FLAG;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.ShoriFlag, dtMessage, false, true, true))
                {
                    isAddData = false;
                }
                else
                {
                    var value = fncGetStr(col, row);
                    foreach (DataRow drShipping in this._dtShippingPlanExcelType.Rows)
                    {
                        if (ComFunc.GetFld(drShipping, Def_M_COMMON.VALUE1) == value)
                        {
                            dr[ComDefine.FLD_EXCEL_SHORI_FLAG_NAME] = ComFunc.GetFld(drShipping, Def_M_COMMON.ITEM_NAME);
                            break;
                        }
                    }
                }

                // 便のチェック
                itemName = Resources.ShukkaKeikaku_Ship;
                col = 1;
                checkLen = 10;
                field = Def_M_NONYUSAKI.SHIP;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, true, true))
                {
                    isAddData = false;
                }
                else
                {
                    string value = fncGetStr(1, row);

                    // 既に同じ値が存在する場合、falseを返す
                    if (!shipName.Add(value))
                    {
                        // {0}行目の便名が重複しています。
                        ComFunc.AddMultiMessage(dtMessage, "S0100010017", (row + 1).ToString());
                        isAddData = false;
                    }
                }

                var test = fncGetStr(col, row);

                // 有償/無償
                itemName = Resources.ShukkaKeikaku_EstimateFlag;
                col = 2;
                checkLen = -1;
                field = ComDefine.FLD_ESTIMATE_FLAG_NAME;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.EstimateFlag, dtMessage, true, true, false))
                {
                    isAddData = false;
                }
                else
                {
                    var value = fncGetStr(col, row);
                    foreach (DataRow drEstimate in this._dtEstimate.Rows)
                    {
                        if (ComFunc.GetFld(drEstimate, Def_M_COMMON.ITEM_NAME) == value)
                        {
                            dr[Def_M_NONYUSAKI.ESTIMATE_FLAG] = ComFunc.GetFld(drEstimate, Def_M_COMMON.VALUE1);
                            break;
                        }
                    }
                }

                // AIR/SHIP
                itemName = Resources.ShukkaKeikaku_TransportFlag;
                col = 3;
                checkLen = 20;
                field = Def_M_NONYUSAKI.TRANSPORT_FLAG;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, false, true))
                {
                    isAddData = false;
                }

                // 出荷元
                itemName = Resources.ShukkaKeikaku_ShukkaFrom;
                col = 4;
                checkLen = 10;
                field = Def_M_NONYUSAKI.SHIP_FROM;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, false, true))
                {
                    isAddData = false;
                }
                else
                {
                    // 出荷元マスタに存在するかどうかチェック
                    bool isFind = false;
                    if (UtilData.ExistsData(this._dtShipFrom))
                    {
                        foreach (DataRow drShipFrom in this._dtShipFrom.Rows)
                        {
                            if (ComFunc.GetFld(drShipFrom, Def_M_SHIP_FROM.SHIP_FROM_NAME) == ComFunc.GetFld(dr, Def_M_NONYUSAKI.SHIP_FROM))
                            {
                                isFind = true;
                                dr[Def_M_NONYUSAKI.SHIP_FROM_CD] = ComFunc.GetFld(drShipFrom, Def_M_SHIP_FROM.SHIP_FROM_NO);
                                break;
                            }
                        }
                    }
                    if (!isFind)
                    {
                        // {0}行目の{1}は現在使われていない出荷元です。
                        ComFunc.AddMultiMessage(dtMessage, "S0100010013", (row + 1).ToString(), itemName);
                        isAddData = false;
                    }
                }

                // 出荷先
                itemName = Resources.ShukkaKeikaku_ShukkaTo;
                col = 5;
                checkLen = 20;
                field = Def_M_NONYUSAKI.SHIP_TO;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, true, true))
                {
                    isAddData = false;
                }
                else
                {
                    string value = fncGetStr(5, row); // 5列目の値を取得

                    // 値がoptionsリストに存在しない場合、エラーメッセージを表示してfalseを返す
                    if (!shukkaOptions.Contains(value))
                    {
                        // {0}行目の{1}は現在使われていない出荷先です。
                        ComFunc.AddMultiMessage(dtMessage, "S0100010018", (row + 1).ToString(), Resources.ShukkaKeikaku_ShukkaTo);
                        isAddData = false;
                    }
                }

                // 出荷日
                itemName = Resources.ShukkaKeikaku_ShukkaDate;
                col = 6;
                checkLen = 15;
                field = Def_M_NONYUSAKI.SHIP_DATE;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.Date, dtMessage, true, false, true))
                {
                    isAddData = false;
                }

                // 製番
                itemName = Resources.ShukkaKeikaku_Seiban;
                col = 7;
                checkLen = 500;
                field = Def_M_NONYUSAKI.SEIBAN;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, false, true))
                {
                    isAddData = false;
                }

                // 機種
                itemName = Resources.ShukkaKeikaku_Model;
                col = 8;
                checkLen = 500;
                field = Def_M_NONYUSAKI.KISHU;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, false, true))
                {
                    isAddData = false;
                }

                // 内容
                itemName = Resources.ShukkaKeikaku_Naiyo;
                col = 9;
                checkLen = 500;
                field = Def_M_NONYUSAKI.NAIYO;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, false, true))
                {
                    isAddData = false;
                }

                // 到着予定日
                itemName = Resources.ShukkaKeikaku_TouchakuyoteiDate;
                col = 10;
                checkLen = 15;
                field = Def_M_NONYUSAKI.TOUCHAKUYOTEI_DATE;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.Date, dtMessage, true, false, true))
                {
                    isAddData = false;
                }

                // 機械Parts
                itemName = Resources.ShukkaKeikaku_KikaiParts;
                col = 11;
                checkLen = 500;
                field = Def_M_NONYUSAKI.KIKAI_PARTS;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, false, true))
                {
                    isAddData = false;
                }

                // 制御Parts
                itemName = Resources.ShukkaKeikaku_SeigyoParts;
                col = 12;
                checkLen = 500;
                field = Def_M_NONYUSAKI.SEIGYO_PARTS;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, false, true))
                {
                    isAddData = false;
                }

                // 備考
                itemName = Resources.ShukkaKeikaku_Remarks;
                col = 13;
                checkLen = 500;
                field = Def_M_NONYUSAKI.BIKO;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, false, true))
                {
                    isAddData = false;
                }
                              
                if (isAddData)
                {
                    dt.Rows.Add(dr);
                }
                else
                {
                    ret = false;
                }
                is1st = false;
            }
            // 表示データがない場合は失敗とする。
            if (dt.Rows.Count == 0)
            {
                ret = false;
            }
            return ret;
        }

        #endregion

        #region 取込チェック処理

        /// --------------------------------------------------
        /// <summary>
        /// 取込チェック処理
        /// </summary>
        /// <param name="target">対象文字列</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="dr">取込データのデータロウ</param>
        /// <param name="field">データロウのカラム名</param>
        /// <param name="checkLen">最大バイトサイズ</param>
        /// <param name="itemName">取込対象項目名</param>
        /// <param name="attrType">文字属性</param>
        /// <param name="dtMessage">エラーテーブル</param>
        /// <param name="isString">文字列かどうか</param>
        /// <param name="isNecessary">必須かどうか</param>
        /// <param name="isCheckLen">列長チェックするかどうか</param>
        /// <returns>true:エラーなし/false:エラーあり</returns>
        /// <create>H.Tajimi 2018/10/30</create>
        /// --------------------------------------------------
        private bool CheckAndSetExcelData(string target, int rowIndex, DataRow dr, string field, int checkLen, string itemName, ExcelInputAttrType attrType, DataTable dtMessage, bool isString, bool isNecessary, bool isCheckLen)
        {
            object retVal = DBNull.Value;
            bool ret = true;
            bool isNullDate = false;
            string target2 = string.Empty;
            if (attrType == ExcelInputAttrType.Date)
            {
                try
                {
                    if (string.IsNullOrEmpty(target))
                    {
                        isNullDate = true;
                    }
                    else
                    {
                        target2 = DateTime.FromOADate(UtilConvert.ToDouble(target)).Date.ToString("yyyy/MM/dd");
                    }
                    
                }
                catch
                {
                    // 日付として有効でない場合は以降の評価をしても無駄なのでここでエラー終了する
                    // {0}行目の{1}が日付に変換できませんでした。
                    ComFunc.AddMultiMessage(dtMessage, "S0100010004", (rowIndex + 1).ToString(), itemName);
                    return false;
                }
            }
            else
            {
                target2 = target;
            }
            if (!this.CheckByteLength(target2, rowIndex, ref retVal, checkLen, itemName, dtMessage, isString, isCheckLen))
            {
                ret = false;
            }
            if (!isNullDate)
            {
                if (!CheckRegulation(target2, rowIndex, itemName, attrType, dtMessage))
                {
                    ret = false;
                }
            }
            if (isNecessary)
            {
                // 必須チェック
                if (string.IsNullOrEmpty(target2))
                {
                    // {0}行目の{1}が入力されていません。
                    ComFunc.AddMultiMessage(dtMessage, "S0100020008", (rowIndex + 1).ToString(), itemName);
                    ret = false;
                }
            }
            if (ret)
            {
                dr[field] = retVal;
            }
            return ret;
        }

        #endregion

        #region 最大バイトサイズチェック

        /// --------------------------------------------------
        /// <summary>
        /// 最大バイトサイズチェック
        /// </summary>
        /// <param name="target">対象文字列</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="value">取り込んだ値</param>
        /// <param name="checkLen">最大バイトサイズ</param>
        /// <param name="itemName">取込対象項目名</param>
        /// <param name="dtMessage">エラーテーブル</param>
        /// <param name="isString">文字列かどうか</param>
        /// <param name="isCheckLen">列長チェックするかどうか</param>
        /// <returns>true:エラーなし/false:エラーあり</returns>
        /// <create>H.Tajimi 2018/10/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckByteLength(string target, int rowIndex, ref object value, int checkLen, string itemName, DataTable dtMessage, bool isString, bool isCheckLen)
        {
            if (isString)
            {
                return this.CheckAndSetStringByteLength(target, rowIndex, ref value, checkLen, itemName, dtMessage, isCheckLen);
            }
            else
            {
                return this.CheckAndSetIntLength(target, rowIndex, ref value, checkLen, itemName, dtMessage, isCheckLen);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 文字列の最大バイトサイズチェック
        /// </summary>
        /// <param name="target">対象文字列</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="value">取り込んだ値</param>
        /// <param name="checkLen">最大バイトサイズ</param>
        /// <param name="itemName">取込対象項目名</param>
        /// <param name="dtMessage">エラーテーブル</param>
        /// <param name="isCheckLen">列長チェックするかどうか</param>
        /// <returns>true:エラーなし/false:エラーあり</returns>
        /// <create>H.Tajimi 2018/10/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckAndSetStringByteLength(string target, int rowIndex, ref object value, int checkLen, string itemName, DataTable dtMessage, bool isCheckLen)
        {
            if (isCheckLen && checkLen < UtilString.GetByteCount(target))
            {
                // {0}行目の{1}が登録できる文字数を超えています。
                ComFunc.AddMultiMessage(dtMessage, "S0100020010", (rowIndex + 1).ToString(), itemName);
                return false;
            }
            if (!string.IsNullOrEmpty(target))
            {
                value = target;
            }
            return true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 数値の最大バイトサイズチェック
        /// </summary>
        /// <param name="target">対象文字列</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="value">取り込んだ値</param>
        /// <param name="checkLen">最大バイトサイズ</param>
        /// <param name="itemName">取込対象項目名</param>
        /// <param name="dtMessage">エラーテーブル</param>
        /// <param name="isCheckLen">列長チェックするかどうか</param>
        /// <returns>true:エラーなし/false:エラーあり</returns>
        /// <create>H.Tajimi 2018/10/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckAndSetIntLength(string target, int rowIndex, ref object value, int checkLen, string itemName, DataTable dtMessage, bool isCheckLen)
        {
            bool isNullOrEmpty = false;
            if (string.IsNullOrEmpty(target))
            {
                isNullOrEmpty = true;
                target = "0";
            }
            string[] number = target.Split('.');
            string decimalsStr = string.Empty;
            if (1 < number.Length)
            {
                decimalsStr = number[1];
            }
            if (0 < UtilString.GetByteCount(decimalsStr))
            {
                // {0}行目の{1}が整数値以外が入力されています。
                ComFunc.AddMultiMessage(dtMessage, "S0100020030", (rowIndex + 1).ToString(), itemName);
                return false;
            }
            int result;
            if (!Int32.TryParse(target, System.Globalization.NumberStyles.Number, null, out result))
            {
                // {0}行目の{1}が数値に変換できませんでした。
                ComFunc.AddMultiMessage(dtMessage, "S0100020031", (rowIndex + 1).ToString(), itemName);
                return false;
            }
            string checkStr = result.ToString();
            if (isCheckLen && checkLen < UtilString.GetByteCount(checkStr))
            {
                // {0}行目の{1}が登録できる文字数を超えています。
                ComFunc.AddMultiMessage(dtMessage, "S0100020010", (rowIndex + 1).ToString(), itemName);
                return false;
            }
            if (!isNullOrEmpty)
            {
                value = result;
            }
            return true;
        }

        #endregion

        #region 入力値の文字属性チェック

        /// --------------------------------------------------
        /// <summary>
        /// 入力値の文字属性チェック
        /// </summary>
        /// <param name="target">対象文字列</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="itemName">取込対象項目名</param>
        /// <param name="attrType">文字属性</param>
        /// <param name="dtMessage">エラーテーブル</param>
        /// <returns>true:エラーなし/false:エラーあり</returns>
        /// <create>H.Tajimi 2018/10/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckRegulation(string target, int rowIndex, string itemName, ExcelInputAttrType attrType, DataTable dtMessage)
        {
            ComRegulation regulation = new ComRegulation();
            bool isUse = false;
            string targetType = string.Empty;
            string msgExtend = string.Empty;

            switch (attrType)
            {
                case ExcelInputAttrType.ShoriFlag:
                    targetType += ComRegulation.REGULATION_NARROW_NUMERIC_ONLY;
                    isUse = true;
                    msgExtend = Resources.ShukkaKeikaku_Numeric;
                    break;
                case ExcelInputAttrType.AlphaNum:
                    targetType += ComRegulation.REGULATION_NARROW_ALPHA_LOW;
                    targetType += ComRegulation.REGULATION_NARROW_ALPHA_UP;
                    targetType += ComRegulation.REGULATION_NARROW_NUMERIC_ONLY;
                    targetType += ComRegulation.REGULATION_NARROW_SIGN;
                    targetType += ComRegulation.REGULATION_NARROW_SPACE;
                    isUse = true;
                    msgExtend = Resources.ShukkaKeikakuMeisai_Singlebyte;
                    break;
                case ExcelInputAttrType.WideString:
                    targetType += ComRegulation.REGULATION_SURROGATE;
                    isUse = false;
                    msgExtend = Resources.ShukkaKeikaku_PlatformDependentChar;
                    break;
                case ExcelInputAttrType.Numeric:
                    targetType += ComRegulation.REGULATION_NARROW_NUMERIC_SIGN;
                    isUse = true;
                    msgExtend = Resources.ShukkaKeikaku_Numeric;
                    break;
                case ExcelInputAttrType.EstimateFlag:
                    targetType += ComRegulation.REGULATION_SURROGATE;
                    isUse = false;
                    msgExtend = Resources.ShukkaKeikaku_PlatformDependentChar;
                    break;
                case ExcelInputAttrType.Date:
                    DateTime result;
                    if (!DateTime.TryParse(target, out result))
                    {
                        // {0}行目の{1}が日付に変換できませんでした。
                        ComFunc.AddMultiMessage(dtMessage, "S0100010004", (rowIndex + 1).ToString(), itemName);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                default:
                    break;
            }
            if (!regulation.CheckRegularString(target, targetType, isUse, false))
            {
                // {0}行目の{1}が登録できない文字を含んでいます。{2}
                ComFunc.AddMultiMessage(dtMessage, "S0100020029", (rowIndex + 1).ToString(), itemName, msgExtend);
                return false;
            }

            if (attrType == ExcelInputAttrType.ShoriFlag)
            {
                var shoriFlag = Convert.ToInt32(target);
                if (shoriFlag == (int)ShoriFlag.NoChange || shoriFlag == (int)ShoriFlag.InsOrUpd || shoriFlag == (int)ShoriFlag.Del)
                {
                    return true;
                }
                else
                {
                    // {0}行目の{1}が登録できない文字を含んでいます。({2})以外は使用できません。
                    ComFunc.AddMultiMessage(dtMessage, "S0100010005", (rowIndex + 1).ToString(), itemName
                        , string.Format("{0}/{1}/{2}", (int)ShoriFlag.NoChange, (int)ShoriFlag.InsOrUpd, (int)ShoriFlag.Del));
                    return false;
                }
            }
            else if (attrType == ExcelInputAttrType.EstimateFlag)
            {
                var retTmp = false;
                foreach (DataRow dr in this._dtEstimate.Rows)
                {
                    if (ComFunc.GetFld(dr, Def_M_COMMON.ITEM_NAME) == target)
                    {
                        retTmp = true;
                        break;
                    }
                }
                if (!retTmp)
                {
                    var lstArgs = new List<string>();
                    // 最後の行を削除
                    if (_dtEstimate.Rows.Count > 0)
                    {
                        _dtEstimate.Rows.RemoveAt(_dtEstimate.Rows.Count - 1);
                    }
                    foreach (DataRow dr in this._dtEstimate.Rows)
                    {
                        lstArgs.Add(ComFunc.GetFld(dr, Def_M_COMMON.ITEM_NAME));
                    }
                    // {0}行目の{1}が登録できない文字を含んでいます。({2})以外は使用できません。
                    ComFunc.AddMultiMessage(dtMessage, "S0100010005", (rowIndex + 1).ToString(), itemName
                        , string.Join("/", lstArgs.ToArray()));
                    return false;
                }
            }
            return true;
        }
        
        #endregion

        #endregion

        #region 編集内容実行

        #region 制御メソッド

        /// --------------------------------------------------
        /// <summary>
        /// 編集内容実行制御処理
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/10/31</create>
        /// <update>J.Chen 2023/08/29 外部連携</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            var ret = base.RunEdit();
            try
            {
                if (ret)
                {
                    if (this._dialogMode)
                    {
                        // 終了時のメッセージを表示しない
                        this.IsChangedCloseQuestion = false;
                        this.IsCloseQuestion = false;
                        // 外部連携モードは、登録完了で終了
                        this.DialogResult = DialogResult.OK;
                    }
                    // 再検索
                    else
                    {
                        if (!this.RunSearchExec())
                        {
                            this.DisplayClear();
                            this.cboBukkenNameTorikomi.Focus();
                        }
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region 登録処理

        /// --------------------------------------------------
        /// <summary>
        /// 登録処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>H.Tajimi 2018/10/31</create>
        /// <update>T.SASAYAMA 2023/08/08 行削除処理の追加</update>
        /// <update>J.Chen 2023/10/04 データ登録・更新処理修正</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                this.IsRunEditAfterClear = false;
                var cond = new CondS01(this.UserInfo);
                var conn = new ConnS01();
                var ds = new DataSet();
                DataTable dt = (this.shtMeisai.DataSource as DataTable).Copy();
                if (dt == null) return false;
                ds.Tables.Add(dt.Copy());

                string errMsgID;
                string[] args;

                //物件入力チェック
                if (cboBukkenNameTorikomi.SelectedValue != null)
                {
                    cond.BukkenName = this.cboBukkenNameTorikomi.Text;
                }
                else
                {
                    //物件が選択されていません。
                    this.ShowMessage("S0100010015");
                    return false;
                }
                //SHIP製番入力チェック
                if (!string.IsNullOrEmpty(txtShipSeiban.Text))
                {
                    cond.ShipSeiban = this.txtShipSeiban.Text;
                }
                else
                {
                    //運賃・梱包製番が入力されていません。
                    this.ShowMessage("S0100010016");
                    return false;
                }
                //荷受け先入力チェック
                if (cboNiukesaki.SelectedValue != null)
                {
                    cond.ConsignName = this.cboNiukesaki.SelectedValue.ToString();
                }
                else
                {
                    //荷受先が選択されていません。
                    this.ShowMessage("S0100010014");
                    return false;
                }

                if (!this.CheckInput(this.shtMeisai))
                    return false;

                // 新規データ抽出
                DataTable dtInsert = dt.GetChanges(DataRowState.Added);
                if (dtInsert == null)
                {
                    dtInsert = dt.Clone();
                }
                dtInsert.TableName = ComDefine.DTTBL_INSERT;

                // 更新データ抽出
                DataTable dtUpdate = dt.GetChanges(DataRowState.Modified);
                //if (ConsignCD != cond.ConsignName || ShipSeiban != cond.ShipSeiban)
                //{
                //    dtUpdate = dt;
                //}
                if (dtUpdate == null)
                {
                    dtUpdate = dt.Clone();
                }
                // dtInsert から SHIP 値を取得
                HashSet<string> shipValuesdtInsert = new HashSet<string>(dtInsert.AsEnumerable().Select(row => row.Field<string>(Def_M_NONYUSAKI.SHIP)));
                // dtUpdate に含まれているSHIP値を取得
                HashSet<string> shipValuesdtUpdate = new HashSet<string>(dtUpdate.AsEnumerable().Select(row => row.Field<string>(Def_M_NONYUSAKI.SHIP)));
                // dtUpdate と dtInsert の SHIP 値を統合した HashSet を作成
                HashSet<string> combinedShipValues = new HashSet<string>(shipValuesdtUpdate.Concat(shipValuesdtInsert));

                //foreach (DataRow dr in dt.Rows)
                //{
                //    var drShipSeiban = ComFunc.GetFld(dr, Def_M_NONYUSAKI.SHIP_SEIBAN);
                //    var drConsignCD = ComFunc.GetFld(dr, Def_M_NONYUSAKI.CONSIGN_CD);
                //    var drShip = ComFunc.GetFld(dr, Def_M_NONYUSAKI.SHIP);
                //    // 条件に一致しない場合に行を追加
                //    if (drShipSeiban != cond.ShipSeiban || drConsignCD != cond.ConsignName)
                //    {
                //        if (!combinedShipValues.Contains(drShip) && !string.IsNullOrEmpty(drShip))
                //        {
                //            var shoriFlag = ComFunc.GetFld(dr, ComDefine.FLD_EXCEL_SHORI_FLAG);
                //            if (!string.IsNullOrEmpty(shoriFlag) && Convert.ToInt16(shoriFlag) == (int)ShoriFlag.NoChange)
                //            {
                //                dr[ComDefine.FLD_EXCEL_SHORI_FLAG] = (int)ShoriFlag.InsOrUpd;
                //            }
                //            dtUpdate.ImportRow(dr);
                //        }
                //    }
                //}

                dtUpdate.TableName = ComDefine.DTTBL_UPDATE;

                // 削除データ抽出
                DataTable dtDelete = dt.Clone();
                dtDelete.TableName = ComDefine.DTTBL_DELETE;
                DataView dv = dt.DefaultView;
                dv.RowStateFilter = DataViewRowState.Deleted | DataViewRowState.ModifiedOriginal;
                for (int i = 0; i < dv.Count; i++)
                {
                    if (dv[i].Row.RowState == DataRowState.Deleted)
                    {
                        DataRow dr = dtDelete.NewRow();
                        for (int j = 0; j < dtDelete.Columns.Count; j++)
                        {
                            dr[j] = dv[i][j];
                        }
                        dtDelete.Rows.Add(dr);
                    }
                }

                foreach (DataRow dr in dtInsert.Rows.Cast<DataRow>().ToArray())
                {
                    var shoriFlag = ComFunc.GetFld(dr, ComDefine.FLD_EXCEL_SHORI_FLAG);
                    if (!string.IsNullOrEmpty(shoriFlag) && Convert.ToInt16(shoriFlag) == (int)ShoriFlag.NoChange)
                    {
                        dtInsert.Rows.Remove(dr);
                    }
                }

                foreach (DataRow dr in dtUpdate.Rows.Cast<DataRow>().ToArray())
                {
                    var shoriFlag = ComFunc.GetFld(dr, ComDefine.FLD_EXCEL_SHORI_FLAG);
                    if (!string.IsNullOrEmpty(shoriFlag) && Convert.ToInt16(shoriFlag) == (int)ShoriFlag.NoChange)
                    {
                        dtUpdate.Rows.Remove(dr);
                    }
                }

                ds.Tables.Add(dtInsert);
                ds.Tables.Add(dtUpdate);
                ds.Tables.Add(dtDelete);

                if (!conn.InsShukkaKeikaku(cond, ds, out errMsgID, out args))
                {
                    this.ShowMessage(errMsgID, args);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region 保存用入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 保存用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>T.SASAYAMA 2023/08/30</create>
        /// --------------------------------------------------
        public bool CheckInput(Sheet sheet)
        {
            for (int row = 0; row < sheet.MaxRows - 1; row++)
            {
                //出荷日までが必須
                for (int col = 0; col < (int)SHEET_COL.SEIBAN; col++)
                {
                    var ShoriValue = shtMeisai[(int)SHEET_COL.SHORI_FLAG, row].Value;
                    if (ShoriValue != null)
                        if (Convert.ToInt16(ShoriValue) == 0) continue;
                    if (col == (int)SHEET_COL.TRANSPORT_FLAG) continue;
                    // セルの値をチェック
                    if (string.IsNullOrEmpty(Convert.ToString(sheet[col, row].Value)))
                    {
                        string colName = null;
                        switch (col)
                        {
                            case 0: colName = Resources.ShukkaKeikaku_ShoriFlag; break;
                            case 1: colName = Resources.ShukkaKeikaku_Ship; break;
                            case 2: colName = Resources.ShukkaKeikaku_EstimateFlag; break;
                            case 3: colName = Resources.ShukkaKeikaku_TransportFlag; break;
                            case 4: colName = Resources.ShukkaKeikaku_ShukkaFrom; break;
                            case 5: colName = Resources.ShukkaKeikaku_ShukkaTo; break;
                            case 6: colName = Resources.ShukkaKeikaku_ShukkaDate; break;
                        }

                        // {0}行目の{1}を入力して下さい。
                        this.ShowMessage("T0100010005", (row + 1).ToString(), colName);
                        this.shtMeisai.ActivePosition = new Position(col, row);
                        this.shtMeisai.Focus();
                        return false;
                    }
                }
            }
            return true;
        }

        #endregion

        #endregion

        #region イベント

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F1ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/10/31</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F01Button_Click(sender, e);
            try
            {
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F2ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.SASAYAMA 2023/08/22</create>
        /// <update>J.Chen 2023/10/04 送信制御追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F02Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F02Button_Click(sender, e);
            try
            {
                CondS01 cond = new CondS01(this.UserInfo);

                //SHIP製番入力チェック
                if (string.IsNullOrEmpty(txtShipSeiban.Text))
                {
                    this.ShowMessage("S0100010016");
                    return;
                }
                //荷受け先入力チェック
                if (cboNiukesaki.SelectedValue == null)
                {
                    //荷受先が選択されていません。
                    this.ShowMessage("S0100010014");
                    return;
                }


                if (!CheckSheetChanged(this.shtMeisai) || _isNewData)
                {
                    // 内容が変更されているため、メール送信できません。
                    this.ShowMessage("S0100010023");
                    return;
                }
                //メール送信処理
                this.RunMail();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F3ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.SASAYAMA 2023/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F03Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F03Button_Click(sender, e);
            try
            {
                // 行がない場合は処理を抜ける
                if (this.shtMeisai.Rows.Count < 1)
                {
                    return;
                }

                //選択されているセル範囲をすべて取得する
                Range[] objRanges = shtMeisai.GetBlocks(BlocksType.Selection);

                // 2012/05/07 K.Tsutsumi Add 履歴
                bool isDeleted = false;
                // ↑
                foreach (Range range in objRanges)
                {
                    // 行追加時の最終行の場合は処理を抜ける。
                    if ((this.shtMeisai.AllowUserToAddRows) &&
                       ((this.shtMeisai.Rows.Count - 1 == range.TopRow) || (this.shtMeisai.Rows.Count - 1 == range.BottomRow)))
                    {
                        // 行削除を行う場合は、最終行を含まないようにして下さい。
                        this.ShowMessage("S0100020040");
                        return;
                    }

                    //// Range内行ループ
                    //for (int row = range.TopRow; row <= range.BottomRow; row++)
                    //{
                    //    // 状態区分チェック
                    //    int jyotaiFlag = UtilConvert.ToInt32(this.shtMeisai[SHEET_COL_JYOTAI_FLAG, row].Value, 0);
                    //    if (UtilConvert.ToInt32(JYOTAI_FLAG.BOXZUMI_VALUE1) <= jyotaiFlag)
                    //    {
                    //        // 選択行には削除できない状態のものが含まれます。\r\n集荷済の状態まで戻した後に削除して下さい。
                    //        this.ShowMessage("S0100020037");
                    //        return;
                    //    }
                    //    // 2012/05/07 K.Tsutsumi Add 履歴
                    //    else if (this.shtMeisai[SHEET_COL_TAGNO, row].Value != null
                    //        && string.IsNullOrEmpty(this.shtMeisai[SHEET_COL_TAGNO, row].Value.ToString().Trim()) == false)
                    //    {
                    //        // 削除フラグ ON
                    //        isDeleted = true;
                    //    }
                    //}
                }

                // 選択行を削除してもよろしいですか？
                if (this.ShowMessage("S0100020038") == DialogResult.OK)
                {
                    // 2012/05/07 K.Tsutsumi Add 履歴
                    this._isDeleteRecord = this._isDeleteRecord || isDeleted;
                    // ↑

                    this.shtMeisai.Redraw = false;
                    for (int lp = objRanges.Length - 1; lp >= 0; lp--)
                    {
                        // 行数を求める
                        int rowCount = objRanges[lp].BottomRow - objRanges[lp].TopRow + 1;
                        // 行削除
                        this.shtMeisai.RemoveRow(objRanges[lp].TopRow, rowCount, false);
                        // 行削除 (DataTable)
                        for (int i = 0; i < rowCount; i++)
                        {
                            int removeIndex = objRanges[lp].TopRow;

                            if (removeIndex < preValuesTable.Rows.Count)
                            {
                                preValuesTable.Rows.RemoveAt(removeIndex);
                            }
                        }
                    }
                    this.shtMeisai.Redraw = true;
                }
                // ↑
            }
            catch (Exception ex)
            {
                // 2011/03/07 K.Tsutsumi Add 複数行対応
                this.shtMeisai.Redraw = true;
                // ↑
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F6ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/10/31</create>
        /// <update>J.Chen 2024/01/17 排他処理追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                if (this.ShowMessage("A9999999053") != DialogResult.OK) return;
                // クリア
                //this.cboBukkenNameTorikomi.SelectedIndex = -1;
                //this.cboBukkenNameTorikomi.SelectedItem = null;
                //this.cboNiukesaki.SelectedIndex = -1;
                //this.txtShipSeiban.Clear();
                this.cboBukkenNameTorikomi.Enabled = true;
                // グリッドのクリア
                this.SheetClear();
                // 検索条件のロック解除
                this.grpSearch.Enabled = true;
                // ファンクションボタンの切替
                this.ChangeFunctionButton(false);
                // フォーカス移動
                this.btnStart.Focus();
                // 排他クリア
                HaitaClear();
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
        /// <create>H.Tajimi 2018/10/31</create>
        /// <update>J.Chen 2024/01/17 排他処理追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                if (this.ShowMessage("A9999999001") != DialogResult.OK) return;
                // 排他クリア
                HaitaClear();
                this.DisplayClear();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F08ボタンクリック(行コピー)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.SASAYAMA 2023/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F08Button_Click(object sender, EventArgs e)
        {
            if (shtMeisai.Enabled == true)
            {
                var selectionType = this.shtMeisai.SelectionType;
                base.fbrFunction_F08Button_Click(sender, e);
                try
                {
                    int row = this.shtMeisai.ActivePosition.Row;
                    this.CopyRow = row;
                    if (row < 0) return;
                    this.shtMeisai.SelectionType = SelectionType.Range;
                    this.shtMeisai.CellRange = new Range(SHEET_COL_COPY_START, row, SHEET_COL_COPY_END, row);
                    this.shtMeisai.UIReSelection(this.shtMeisai.CellRange);
                    this.shtMeisai.UICopy();
                }
                catch (Exception ex)
                {
                    CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                }
                finally
                {
                    this.shtMeisai.SelectionType = selectionType;
                    this.isCellCopied = true;
                }
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F09ボタンクリック(行貼り付け)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.SASAYAMA 2023/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F09Button_Click(object sender, EventArgs e)
        {
            if (this.isCellCopied)
            {
                var selectionType = this.shtMeisai.SelectionType;
                base.fbrFunction_F09Button_Click(sender, e);
                try
                {
                    //選択行を取得する・選択行がない場合処理を終了する
                    int row = this.shtMeisai.ActivePosition.Row;
                    if (row < 0) return;
                    //if (this.shtMeisai.Rows.Count - 1 == row)
                    //{
                    //    //行削除を行う場合は、最終行を含まないようにしてください。
                    //    this.ShowMessage("S0100020040");
                    //    return;
                    //}

                    //選択行の一つしたに新規行を挿入
                    int add_row = row + 1;
                    var rows = shtMeisai.MaxRows;
                    this.shtMeisai.Redraw = false;
                    this.shtMeisai.InsertRow(add_row, false);
                    shtMeisai.ValueChanged -= shtMeisai_ValueChanged;

                    //コピー行の上に貼り付けた場合
                    if (row < CopyRow)
                    {
                        this.CopyRow += 1;
                        this.shtMeisai.SelectionType = SelectionType.Range;
                        this.shtMeisai.CellRange = new Range(SHEET_COL_COPY_START, CopyRow, SHEET_COL_COPY_END, CopyRow);
                        this.shtMeisai.UIReSelection(this.shtMeisai.CellRange);
                        this.shtMeisai.UICopy();
                    }

                    //新しい行に選択範囲を設定
                    this.shtMeisai.SelectionType = SelectionType.Range;
                    this.shtMeisai.CellRange = new Range(SHEET_COL_COPY_START, add_row, SHEET_COL_COPY_END, add_row);

                    //選択範囲に貼り付けを行う
                    this.shtMeisai.UIReSelection(this.shtMeisai.CellRange);
                    this.shtMeisai.UIPaste();

                    //新規行の便名の設定
                    string oldShipMent = null;
                    if (this.shtMeisai[(int)SHEET_COL.SHIP, row].Value != null)
                    {
                        oldShipMent = this.shtMeisai[(int)SHEET_COL.SHIP, row].Value.ToString();
                        string newShipMent = null;
                        if (!string.IsNullOrEmpty(oldShipMent))
                        {
                            var match = System.Text.RegularExpressions.Regex.Match(oldShipMent, @"(\d+)$");
                            if (match.Success)
                            {
                                int number;
                                if (int.TryParse(match.Value, out number))
                                {
                                    newShipMent = oldShipMent.Substring(0, oldShipMent.Length - match.Value.Length) + (number + 1).ToString();
                                }
                            }
                        }
                        this.shtMeisai[(int)SHEET_COL.SHIP, add_row].Value = newShipMent;
                        for (int rowIndex = 0; rowIndex < rows; rowIndex++)
                        {
                            // 現在の行自体とは比較しない
                            if (rowIndex != add_row && object.Equals(shtMeisai[(int)SHEET_COL.SHIP, rowIndex].Value, newShipMent))
                            {
                                // 便名が重複しています。
                                this.ShowMessage("T0100060007");
                                shtMeisai[(int)SHEET_COL.SHIP, add_row].Text = null;
                            }
                        }
                    }

                    //新規行の出荷日の設定
                    DateTime? oldShipDate = this.shtMeisai[(int)SHEET_COL.SHIP_DATE, row].Value as DateTime?;
                    DateTime? newShipDate;
                    if (oldShipDate != null)
                    {
                        newShipDate = oldShipDate.Value.AddDays(7);
                        this.shtMeisai[(int)SHEET_COL.SHIP_DATE, add_row].Value = newShipDate;
                    }
                    else
                    {
                        this.shtMeisai[(int)SHEET_COL.SHIP_DATE, add_row].Value = null;
                    }

                    //新規行の到着予定日の設定
                    shtMeisai[(int)SHEET_COL.TOUCHAKUYOTEI_DATE, add_row].Value = SetArrivalDate(add_row);
                    shtMeisai[(int)SHEET_COL.SHORI_FLAG, add_row].Value = "1";

                    // 色設定
                    var value = shtMeisai[(int)SHEET_COL.ESTIMATE_FLAG, add_row].Value;
                    var gratiscolor = ComDefine.GRATIS_COLOR;
                    var onerouscolor = ComDefine.ONEROUS_COLOR;
                    var ntlcolor = ComDefine.NEUTRAL_COLOR;

                    if (value != null && Convert.ToInt32(value) == (int)EstimateFlag.Mushou)
                    {
                        //無償色に設定する
                        SetupRowColor(this.shtMeisai.Rows[add_row], gratiscolor, this.shtMeisai.GridLine, Borders.All);
                    }
                    else if (value != null && Convert.ToInt32(value) == (int)EstimateFlag.Yushou)
                    {
                        //有償色に設定する
                        SetupRowColor(this.shtMeisai.Rows[add_row], onerouscolor, this.shtMeisai.GridLine, Borders.All);
                    }
                    else
                    {
                        // 色を初期値に戻す
                        SetupRowColor(this.shtMeisai.Rows[add_row], ntlcolor, this.shtMeisai.GridLine, Borders.All);
                    }


                }
                catch (Exception ex)
                {
                    CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                }
                finally
                {
                    this.preValuesTable = GetAllRowValuesInTable(this.shtMeisai);
                    this.shtMeisai.SelectionType = selectionType;
                    this.shtMeisai.Redraw = true;
                    shtMeisai.ValueChanged += shtMeisai_ValueChanged;
                }
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F10ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Gwon 2023/08/10</create>
        /// <update>J.Chen 2023/10/17 旧データ対応のため</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F10Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F10Button_Click(sender, e);
            try
            {
                //SHIP製番入力チェック
                if (string.IsNullOrEmpty(txtShipSeiban.Text))
                {
                    this.ShowMessage("S0100010016");
                    return;
                }
                //荷受け先入力チェック
                if (cboNiukesaki.SelectedValue == null)
                {
                    //荷受先が選択されていません。
                    this.ShowMessage("S0100010014");
                    return;
                }

                if (!CheckSheetChanged(this.shtMeisai))
                {
                    // 内容が変更されているため、Excel出力できません。
                    this.ShowMessage("S0100010021");
                    return;
                }
                else if (preValuesTable.Rows.Count < 2)
                {
                    // 出荷計画Data(Excel)のFilesにDataがありません。
                    this.ShowMessage("S0100010022");
                    return;
                }
                this.RunExcelOnly();
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
        /// <create>Y.Gwon 2023/08/10</create>
        /// <update>J.Chen 2023/10/17 旧データ対応のため</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F11Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F11Button_Click(sender, e);

            var conn = new ConnS01();
            var cond = new CondS01(this.UserInfo);

            //SHIP製番入力チェック
            if (string.IsNullOrEmpty(txtShipSeiban.Text))
            {
                this.ShowMessage("S0100010016");
                return;
            }
            //荷受け先入力チェック
            if (cboNiukesaki.SelectedValue == null)
            {
                //荷受先が選択されていません。
                this.ShowMessage("S0100010014");
                return;
            }

            var rConsignCD = !string.IsNullOrEmpty(ConsignCD) ? ConsignCD : this.cboNiukesaki.SelectedValue.ToString();
            var rShipSeiban = !string.IsNullOrEmpty(ShipSeiban) ? ShipSeiban : this.txtShipSeiban.Text;

            using (var f = new MailSousinRireki(this.UserInfo, BukkenNo, rConsignCD, rShipSeiban))
            {
                f.ShowDialog();
            }
        }


        #endregion

        #region ボタンクリック

        #region 参照ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 参照ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/10/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnReference_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            try
            {
                ofdExcel.FileName = this.txtExcel.Text;
                ofdExcel.Title = Resources.ShukkaKeikaku_RefDlgTitle;
                if (ofdExcel.ShowDialog() == DialogResult.OK)
                {
                    this.txtExcel.Text = ofdExcel.FileName;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }
        
        #endregion

        #region 取込ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 取込ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/10/31</create>
        /// <update>J.Chen 2024/08/07 取込時排他クリア追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private void btnStart_Click(object sender, EventArgs e)
        {
            // 取込を実行しますか？
            if (this.ShowMessage("A9999999087") != DialogResult.OK) return;

            this.ClearMessage();
            this.SheetClear();
            this.InitializeSheet(this.shtMeisai);

            // 荷受先クリア
            this.cboNiukesaki.SelectedIndex = -1;
            this.cboNiukesaki.Text = null;

            try
            {
                if (!this.ExecuteImport())
                {
                    return;
                }

                // 検索ボタン無効
                this.btnSearch.Enabled = false;
                // 排他クリア
                HaitaClear();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }
        
        #endregion

        #region 開始ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 開始ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.SASAYAMA 2023/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnKaishi_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            this.SheetClear();
            this.shtMeisai.Redraw = false;
            this.InitializeSheet(this.shtMeisai);
            try
            {
                this.RunSearch();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion
                       
        #endregion

        #region シートイベント

        #region シート値変更

        /// --------------------------------------------------
        /// <summary>
        /// シート値変更時のイベント処理
        /// </summary>
        /// <returns></returns>
        /// <create>T.SASAYAMA 2023/08/03</create>
        /// <update>J.Chen 2023/09/28 有償無償色付け</update>
        /// <update></update>
        /// --------------------------------------------------
        private void shtMeisai_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (isUpdating) return;
            try
            {
                Sheet sheet = sender as Sheet;
                int row = e.Position.Row;
                var rows = shtMeisai.MaxRows;
                int column = e.Position.Column;
                var columns = shtMeisai.MaxColumns;
                int getRow = 0;
                isUpdating = true;
                this.shtMeisai.Redraw = false;

                switch (column)
                {
                    case (int)SHEET_COL.ESTIMATE_FLAG:
                        {
                            var value = shtMeisai[column, row].Value;
                            var gratiscolor = ComDefine.GRATIS_COLOR;
                            var onerouscolor = ComDefine.ONEROUS_COLOR;
                            var ntlcolor = ComDefine.NEUTRAL_COLOR;

                            if (value != null && Convert.ToInt32(value) == (int)EstimateFlag.Mushou)
                            {
                                //無償色に設定する
                                SetupRowColor(this.shtMeisai.Rows[row], gratiscolor, this.shtMeisai.GridLine, Borders.All);
                            }
                            else if (value != null && Convert.ToInt32(value) == (int)EstimateFlag.Yushou)
                            {
                                //有償色に設定する
                                SetupRowColor(this.shtMeisai.Rows[row], onerouscolor, this.shtMeisai.GridLine, Borders.All);
                            }
                            else
                            {
                                // 色を初期値に戻す
                                SetupRowColor(this.shtMeisai.Rows[row], ntlcolor, this.shtMeisai.GridLine, Borders.All);
                            }
                            break;
                        }
                    case (int)SHEET_COL.SHORI_FLAG:
                        {
                            var value = shtMeisai[column, row].Value;
                            var valueEF = shtMeisai[(int)SHEET_COL.ESTIMATE_FLAG, row].Value;
                            var gratiscolor = ComDefine.GRATIS_COLOR;
                            var onerouscolor = ComDefine.ONEROUS_COLOR;
                            if (value != null && Convert.ToInt32(value) == (int)ShoriFlag.Del)
                            {
                                SetRowDisabled(row);
                            }
                            else if (valueEF != null && Convert.ToInt32(valueEF) == (int)EstimateFlag.Mushou)
                            {
                                //無償色に設定する
                                SetupRowColor(this.shtMeisai.Rows[row], gratiscolor, this.shtMeisai.GridLine, Borders.All);
                            }
                            else if (valueEF != null && Convert.ToInt32(valueEF) == (int)EstimateFlag.Yushou)
                            {
                                //有償色に設定する
                                SetupRowColor(this.shtMeisai.Rows[row], onerouscolor, this.shtMeisai.GridLine, Borders.All);
                            }
                            else
                            {
                                SetRowDefault(row);
                            }
                            return;
                        }
                    case (int)SHEET_COL.SHIP:
                        {
                            var newShip = sheet[(int)SHEET_COL.SHIP, row].Value;
                            if (newShip == null)
                            {
                                break;
                            }
                            for (int rowIndex = 0; rowIndex < rows; rowIndex++)
                            {
                                // 現在の行自体とは比較しない
                                if (rowIndex != row && object.Equals(sheet[(int)SHEET_COL.SHIP, rowIndex].Value, newShip))
                                {
                                    // 便名が重複しています。
                                    this.ShowMessage("T0100060007");
                                    sheet[(int)SHEET_COL.SHIP, row].Text = null;
                                    return;
                                }
                            }
                            break;
                        }
                    case (int)SHEET_COL.SHIP_DATE:
                        {
                            // 到着予定日に新しい日付を設定
                            sheet[(int)SHEET_COL.TOUCHAKUYOTEI_DATE, row].Value = SetArrivalDate(row);
                            break;
                        }
                }

                // 新規行編集時のための処理
                if (preValuesTable.Rows.Count <= row)
                {
                    getRow = preValuesTable.Rows.Count - 1;
                }
                else
                {
                    getRow = row;
                }

                // 今の行をdatarowにする
                DataRow nowRowValue = GetSelectedRowValuesInDataRow(this.shtMeisai, row);

                // 検索・取込時に保存したテーブルから行を取り出す
                DataRow previousRowValue = preValuesTable.Rows[getRow];

                bool hasChanged = false;
                //行のSHIP列から比較する
                for (int i = 1; i < columns; i++)
                {
                    var sheetnow = nowRowValue[i].ToString();
                    var sheetpre = previousRowValue[i].ToString();

                    if (sheetnow == null && sheetpre != null)
                    {
                        hasChanged = true;
                        break;
                    }
                    else if (sheetnow != null && !sheetnow.Equals(sheetpre))
                    {
                        hasChanged = true;
                        break;
                    }
                }

                if (hasChanged)
                {
                    // 値が変わった場合は、処理フラグを変更/追加に設定する
                    sheet[(int)SHEET_COL.SHORI_FLAG, row].Value = "1";
                }
                else
                {
                    // 値が変わっていない場合は、処理フラグを変更なしに設定する
                    sheet[(int)SHEET_COL.SHORI_FLAG, row].Value = "0";
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                isUpdating = false;
                this.shtMeisai.Redraw = true;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// シート値をDataTable型に格納する
        /// </summary>
        /// <returns>DataTable</returns>
        /// <create>T.SASAYAMA 2023/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetAllRowValuesInTable(Sheet sheet)
        {
            try
            {
                DataTable allRowValues = new DataTable();

                for (int column = 0; column < sheet.Columns.Count; column++)
                {
                    allRowValues.Columns.Add(column.ToString());
                }

                for (int row = 0; row < sheet.Rows.Count; row++)
                {
                    DataRow currentRow = allRowValues.NewRow();

                    for (int column = 0; column < sheet.Columns.Count; column++)
                    {
                        currentRow[column.ToString()] = sheet[column, row].Value;
                    }

                    allRowValues.Rows.Add(currentRow);
                }

                return allRowValues;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 選択した行のシート値をDataRow型に格納する
        /// </summary>
        /// <returns>DataRow</returns>
        /// <create>T.SASAYAMA 2023/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataRow GetSelectedRowValuesInDataRow(Sheet sheet, int selectedRow)
        {
            try
            {
                DataTable dt = new DataTable();

                for (int column = 0; column < sheet.Columns.Count; column++)
                {
                    dt.Columns.Add(column.ToString());
                }

                DataRow currentRow = dt.NewRow();

                for (int column = 0; column < sheet.Columns.Count; column++)
                {
                    currentRow[column.ToString()] = sheet[column, selectedRow].Value;
                }

                return currentRow;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 処理フラグが削除時の処理

        /// --------------------------------------------------
        /// <summary>
        /// シート全体から処理フラグが削除の行を非活性にする
        /// </summary>
        /// <create>T.SASAYAMA 2023/08/03</create>
        /// <update>J.Chen 2023/09/28 有償無償色付け</update>
        /// <update></update>
        /// --------------------------------------------------
        private void SetTableDisabled()
        {
            var rows = shtMeisai.MaxRows;      // 行数を取得
            var columns = shtMeisai.MaxColumns; // 列数を取得

            for (int rowIndex = 0; rowIndex < rows; rowIndex++)
            {
                var valueEF = shtMeisai[(int)SHEET_COL.ESTIMATE_FLAG, rowIndex].Value;
                var value = shtMeisai[0, rowIndex].Value;
                var gratiscolor = ComDefine.GRATIS_COLOR;
                var onerouscolor = ComDefine.ONEROUS_COLOR;

                if (valueEF != null && Convert.ToInt32(valueEF) == (int)EstimateFlag.Mushou)
                {
                    //無償色に設定する
                    SetupRowColor(this.shtMeisai.Rows[rowIndex], gratiscolor, this.shtMeisai.GridLine, Borders.All);
                }
                else if (valueEF != null && Convert.ToInt32(valueEF) == (int)EstimateFlag.Yushou)
                {
                    //有償色に設定する
                    SetupRowColor(this.shtMeisai.Rows[rowIndex], onerouscolor, this.shtMeisai.GridLine, Borders.All);
                }

                if (value != null && Convert.ToInt32(value) == (int)ShoriFlag.Del)
                {
                    SetRowDisabled(rowIndex);
                }
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// シート全体をデフォルト状態に戻す
        /// </summary>
        /// <create>T.SASAYAMA 2023/08/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetSheetDefault()
        {
            var rows = shtMeisai.MaxRows;
            for (int rowIndex = 0; rowIndex < rows; rowIndex++)
            {
                SetRowDefault(rowIndex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 受け取った行を非活性にする
        /// </summary>
        /// <create>T.SASAYAMA 2023/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetRowDisabled(int row)
        {
            var columns = shtMeisai.MaxColumns;
            var grycolor = ComDefine.GRY_COLOR;

            //色をグレーに設定する
            SetupRowColor(this.shtMeisai.Rows[row], grycolor, this.shtMeisai.GridLine, Borders.All);

            for (int colIndex = 1; colIndex < columns; colIndex++) // 0列目は除く
            {
                shtMeisai[colIndex, row].Enabled = false;  // セルを非活性にする
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 受け取った行を初期設定にする
        /// </summary>
        /// <create>T.SASAYAMA 2023/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetRowDefault(int row)
        {
            var columns = shtMeisai.MaxColumns;
            var ntlcolor = ComDefine.NEUTRAL_COLOR;

            // 色を初期値に戻す
            SetupRowColor(this.shtMeisai.Rows[row], ntlcolor, this.shtMeisai.GridLine, Borders.All);

            for (int colIndex = 1; colIndex < columns; colIndex++) // 0列目は除く
            {
                shtMeisai[colIndex, row].Enabled = true;  // セルを活性にする
            }
        }

        #endregion

        #region 到着日付を計算するメソッド

        /// --------------------------------------------------
        /// <summary>
        /// 受け取った行を計算する
        /// </summary>
        /// <create>T.SASAYAMA 2023/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private DateTime? SetArrivalDate(int row)
        {
            if (this.shtMeisai[(int)SHEET_COL.TRANSPORT_FLAG, row].Value == null)
            {
                return null;
            }
            DateTime? shipDate = shtMeisai[(int)SHEET_COL.SHIP_DATE, row].Value as DateTime?;

            if (!shipDate.HasValue)
            {
                return null;
            }

            DateTime ArrivalDate = shipDate.Value;
            int daysAdded = 0;
            int addDays = 0;
            var airShip = shtMeisai[(int)SHEET_COL.TRANSPORT_FLAG, row].Value.ToString();
            addDays = airShip == "AIR" ? 3 : (airShip == "SHIP" ? 14 : 1);

            while (daysAdded < addDays)
            {
                ArrivalDate = ArrivalDate.AddDays(1); // 1日追加

                if (ArrivalDate.DayOfWeek != System.DayOfWeek.Saturday && ArrivalDate.DayOfWeek != System.DayOfWeek.Sunday)
                {
                    // 土曜日や日曜日でなければカウントアップ
                    daysAdded++;
                }
            }
            return ArrivalDate;
        }
        

        #endregion

        #region セル編集終了時

        /// --------------------------------------------------
        /// <summary>
        /// セル編集時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.SASAYAMA 2023/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtMeisai_EnterEdit(object sender, EnterEditEventArgs e)
        {
            this.isCellCopied = false;
        }

        #endregion

        #region キーダウン

        /// --------------------------------------------------
        /// <summary>
        /// シート上でのキーダウン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.SASAYAMA 2023/08/10</create>
        /// <update>J.Chen 2024/01/17 コピペ用ショートカットキーを廃止</update>
        /// <update></update>
        /// --------------------------------------------------
        //private void KeyDownOnSheet(object sender, KeyEventArgs e)
        //{
        //    // CTRL + Cの組み合わせを検出
        //    if (e.Control && e.KeyCode == Keys.C)
        //    {
        //        fbrFunction_F08Button_Click(sender, e);
        //        e.Handled = true; // このキーの組み合わせに関連する他の動作を抑止
        //    }

        //    // CTRL + Vの組み合わせを検出
        //    if (e.Control && e.KeyCode == Keys.V)
        //    {
        //        fbrFunction_F09Button_Click(sender, e);
        //        e.Handled = true; // このキーの組み合わせに関連する他の動作を抑止
        //    }
        //}

        #endregion

        #region データバインドエラー

        /// --------------------------------------------------
        /// <summary>
        /// データバインドエラーイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.SASAYAMA 2023/08/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtMeisai_BindingError(object sender, BindingErrorEventArgs e)
        {
            try
            {
                Sheet sheet = sender as Sheet;
                int col = e.Position.Column;
                int row = e.Position.Row;

                if (this.IsSheetBindingInvalidate(e))
                {
                    switch (col)
                    {
                        case (int)SHEET_COL.TRANSPORT_FLAG:
                        case (int)SHEET_COL.SHIP_FROM:
                        case (int)SHEET_COL.SHIP_TO:
                        case (int)SHEET_COL.ESTIMATE_FLAG:
                            // コンボボックスに項目が存在しない場合、リストに追加する
                            SuperiorComboEditor cbo = sheet[col, row].Editor as SuperiorComboEditor;
                            cbo = AddItemSuperiorComboEditor(cbo, e.Value.ToString());
                            sheet[col, row].Value = e.Value;

                            e.Ignore = true;
                            break;
                        default:
                            break;
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
        /// 明細用コンボボックスアイテム追加
        /// </summary>
        /// <param name="cbo">コンボボックス</param>
        /// <param name="addValue">追加文字列</param>
        /// <returns>コンボボックス</returns>
        /// <create>T.SASAYAMA 2023/08/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private SuperiorComboEditor AddItemSuperiorComboEditor(SuperiorComboEditor cbo, string addValue)
        {
            try
            {
                // cbo.Items.Contains() / cbo.Items.IndexOf()ではヒットしないので自前で検索
                bool isExists = false;
                foreach (ComboItem item in cbo.Items)
                {
                    if (item.Value.ToString().Equals(addValue))
                    {
                        isExists = true;
                        break;
                    }
                }
                if (!isExists)
                    cbo.Items.Add(new ComboItem(0, null, addValue, string.Empty, addValue));
                return cbo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 背景色の設定

        /// --------------------------------------------------
        /// <summary>
        /// 行の背景色、及び前景色を変更する
        /// </summary>
        /// <param name="row">列</param>
        /// <param name="input">入力文字列</param>
        /// <param name="baseGridLine">基準となる罫線設定</param>
        /// <param name="border">罫線設定</param>
        /// <create>T.SASAYAMA 2023/08/09</create>
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

        #endregion

        #region 荷受先コンボボックスの選択変更イベント

        /// --------------------------------------------------
        /// <summary>
        /// 荷受先コンボボックスの選択変更イベント
        /// </summary>
        /// <create>T.SASAYAMA 2023/08/03</create>
        /// <update>J.Chen 2023/10/17 旧データ対応のため</update>
        /// <update></update>
        /// --------------------------------------------------
        private void cboNiukesaki_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.shtMeisai.Enabled)
                {
                    var rows = shtMeisai.MaxRows; // 行数を取得
                    var selectedValueObj = this.cboNiukesaki.SelectedValue;
                    this.shtMeisai.Redraw = false;
                    this.isUpdating = true;

                    // selectedValueがnullまたは空白の場合、メソッドを終了
                    if (selectedValueObj == null || string.IsNullOrEmpty(selectedValueObj.ToString()))
                    {
                        return;
                    }

                    var selectedValue = selectedValueObj.ToString(); // 選択された値を取得

                    for (int rowIndex = 0; rowIndex < rows; rowIndex++)
                    {
                        var consignValue = shtMeisai[(int)SHEET_COL.CONSIGN_CD, rowIndex].Value;
                        var shoriFlagValue = shtMeisai[(int)SHEET_COL.SHORI_FLAG, rowIndex].Value;

                        // shoriFlagValueがnullまたは2の場合、次のループへ
                        if (shoriFlagValue == null || Convert.ToInt32(shoriFlagValue) == (int)ShoriFlag.Del)
                        {
                            continue;
                        }

                        // consignValueがnullまたは空白の場合、次のループへ
                        if (consignValue == null || string.IsNullOrEmpty(consignValue.ToString()))
                        {
                            if (_dialogMode)
                            {
                                consignValue = "";
                            }
                            else
                            {
                                continue;
                            }
                        }

                        var value = consignValue.ToString();

                        if (value != selectedValue)
                        {
                            shtMeisai[(int)SHEET_COL.SHORI_FLAG, rowIndex].Value = "1";
                        }
                        else
                        {
                            shtMeisai[(int)SHEET_COL.SHORI_FLAG, rowIndex].Value = "0";
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.shtMeisai.Redraw = true;
                this.isUpdating = false;
            }
        }

        #endregion

        #region 運賃・梱包製番コンボボックスの値変更イベント

        /// --------------------------------------------------
        /// <summary>
        /// 運賃・梱包製番コンボボックスの値変更イベント
        /// </summary>
        /// <createJ.Chen 2023/10/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private void txtShipSeiban_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.shtMeisai.Enabled)
                {
                    var rows = shtMeisai.MaxRows; // 行数を取得
                    var shipSeibanText = this.txtShipSeiban.Text;
                    this.shtMeisai.Redraw = false;
                    this.isUpdating = true;

                    // shipSeibanTextがnullまたは空白の場合、メソッドを終了
                    if (string.IsNullOrEmpty(shipSeibanText))
                    {
                        return;
                    }

                    for (int rowIndex = 0; rowIndex < rows; rowIndex++)
                    {
                        var shipSeibanValue = shtMeisai[(int)SHEET_COL.SHIP_SEIBAN, rowIndex].Value;
                        var shoriFlagValue = shtMeisai[(int)SHEET_COL.SHORI_FLAG, rowIndex].Value;

                        // shoriFlagValueがnullまたは削除の場合、次のループへ
                        if (shoriFlagValue == null || Convert.ToInt32(shoriFlagValue) == (int)ShoriFlag.Del)
                        {
                            continue;
                        }

                        // shipSeibanValueがnullまたは空白の場合、次のループへ
                        if (shipSeibanValue == null || string.IsNullOrEmpty(shipSeibanValue.ToString()))
                        {
                            continue;
                        }

                        var value = shipSeibanValue.ToString();

                        if (value != shipSeibanText)
                        {
                            shtMeisai[(int)SHEET_COL.SHORI_FLAG, rowIndex].Value = "1";
                        }
                        else
                        {
                            shtMeisai[(int)SHEET_COL.SHORI_FLAG, rowIndex].Value = "0";
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.shtMeisai.Redraw = true;
                this.isUpdating = false;
            }
        }

        #endregion

        #endregion

        #region シート変更確認

        /// --------------------------------------------------
        /// <summary>
        /// シート変更確認
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>T.SASAYAMA 2023/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected bool CheckSheetChanged(Sheet sheet)
        {
            DataTable dt = GetAllRowValuesInTable(this.shtMeisai);
            // データテーブルとシートの行数が同じか確認
            if (dt.Rows.Count != tempTable.Rows.Count) return false;

            // 各行、各列を確認
            for (int row = 0; row < tempTable.Rows.Count; row++)
            {
                for (int col = 0; col < tempTable.Columns.Count; col++)
                {
                    // Sheetの値
                    var sheetValue = dt.Rows[row][col].ToString();

                    // DataTableの値
                    var tableValue = tempTable.Rows[row][col].ToString();

                    if (sheetValue == null && tableValue == null)
                    {
                        continue; // 両方nullなら次に
                    }

                    // 値の比較
                    if (sheetValue == null || tableValue == null || !sheetValue.Equals(tableValue))
                    {
                        return false; // 異なる値が見つかればfalse
                    }
                }
            }
            return true; // すべての値が同じだった
        }
        #endregion

        #endregion

        #region 検索処理

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理制御部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>T.SASAYAMA 2023/07/25</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearch()
        {
            return base.RunSearch();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理実行部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>T.SASAYAMA 2023/07/25</create>
        /// <update>J.Chen 2023/10/04 検索時、運賃・梱包製番空欄ok</update>
        /// <update>J.Chen 2023/10/17 旧データ対応のため</update>
        /// <update>J.Chen 2024/01/17 Rev初期値変更、排他処理追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                CondS01 cond = new CondS01(this.UserInfo);
                ConnS01 conn = new ConnS01();
                this.SheetClear();
                //物件入力チェック
                if (cboBukkenNameTorikomi.SelectedValue != null)
                {
                    cond.BukkenName = this.cboBukkenNameTorikomi.Text;
                    BukkenName = cond.BukkenName;
                }
                else
                {
                    //物件が選択されていません。
                    this.ShowMessage("S0100010015");
                    return false;
                }
                //SHIP製番入力チェック
                if (!string.IsNullOrEmpty(txtShipSeiban.Text))
                {
                    cond.ShipSeiban = this.txtShipSeiban.Text;
                    ShipSeiban = cond.ShipSeiban;
                }
                //else
                //{
                //    //運賃・梱包製番が入力されていません。
                //    this.ShowMessage("S0100010016");
                //    return false;
                //}
                //荷受け先入力チェック
                if (cboNiukesaki.SelectedValue != null)
                {
                    cond.ConsignCD = this.cboNiukesaki.SelectedValue.ToString();
                    ConsignCD = cond.ConsignCD;
                    ConsignName = this.cboNiukesaki.Text;
                }
                else if (_dialogMode)
                {
                    //旧データ対応のため、ダイアログモードの場合、荷受先NULLでもok
                }
                else
                {
                    //荷受先が選択されていません。
                    this.ShowMessage("S0100010014");
                    return false;
                }

                // 排他確認
                DataTable dtHaita = conn.GetHaitaData(cond);
                if (dtHaita.Rows != null && dtHaita.Rows.Count > 0 && !_isHaita)
                {
                    string haitaUserID = ComFunc.GetFld(dtHaita, 0, Def_M_NONYUSAKI.HAITA_USER_ID);
                    string haitaUserName = ComFunc.GetFld(dtHaita, 0, Def_M_NONYUSAKI.HAITA_USER_NAME);

                    // 同一ユーザーでない場合
                    if (this.UserInfo.UserID != haitaUserID)
                    {
                        // このデータは編集中です。\r\n現在の使用者は'{0}'です。\r\n読み取り専用で続行しますか？
                        DialogResult ret = this.ShowMessage("S0100010024", haitaUserName);
                        if (ret == DialogResult.No)
                        {
                            return false;
                        }
                        _isReadOnly = true;
                    }                   
                }

                DataSet ds = conn.GetNonyusaki(cond);

                //if (!ComFunc.IsExistsData(ds, Def_M_NONYUSAKI.Name))
                //{
                //    // 該当する手配明細はありません。
                //    this.ShowMessage("T0100060001");
                //    return false;
                //}

                var dtBukken = ds.Tables[Def_M_BUKKEN.Name];
                BukkenNo = ComFunc.GetFld(dtBukken, 0, Def_M_BUKKEN.BUKKEN_NO);
                var dtNonyusaki = ds.Tables[Def_M_NONYUSAKI.Name];
                if (dtNonyusaki != null && dtNonyusaki.Rows.Count > 0)
                {
                    _isNewData = false;
                    //if (string.IsNullOrEmpty(txtShipSeiban.Text))
                    {
                        this.txtShipSeiban.Text = ComFunc.GetFld(dtNonyusaki, 0, Def_M_NONYUSAKI.SHIP_SEIBAN);
                        cond.ShipSeiban = this.txtShipSeiban.Text;
                        ShipSeiban = cond.ShipSeiban;
                    }
                }
                else 
                {
                    _isNewData = true;
                }

                this.shtMeisai.DataSource = dtNonyusaki;
                var revTable = ds.Tables[Def_M_MAIL_SEND_RIREKI.Name];


                if (revTable != null && revTable.Rows.Count > 0)
                {
                    this.Revision = ComFunc.GetFld(revTable, 0, Def_M_MAIL_SEND_RIREKI.BUKKEN_REV);
                    this.txtRevision.Text = this.Revision;
                    this.txtBoxAssign.Text = ComFunc.GetFld(revTable, 0, Def_M_MAIL_SEND_RIREKI.ASSIGN_COMMENT);
                    this.revVersion = ComFunc.GetFldToDateTime(revTable, 0, Def_M_MAIL_SEND_RIREKI.VERSION).ToString("yyyy/MM/dd HH:mm:ss");
                }
                else
                {
                    this.txtRevision.Text = "";
                    this.Revision = null;
                    this.revVersion = null;
                }

                this.preValuesTable = GetAllRowValuesInTable(this.shtMeisai);
                this.tempTable = preValuesTable.Copy();

                this.cboBukkenNameTorikomi.Enabled = false;
                this.shtMeisai.Enabled = true;

                this.ChangeFunctionButton(true);
                this.SetTableDisabled();

                // 読み取り専用の場合
                this.ReadOnlyMode();

                // 排他登録
                if (!_isReadOnly)
                {
                    int result = conn.UpdateHaita(cond);
                    _isHaita = true;
                }

                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }



        #endregion

        #region メール送信/Excel出力

        #region 制御メソッド(出荷計画表作成＋メール送信)

        /// --------------------------------------------------
        /// <summary>
        /// メール送信実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Gwon 2023/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RunMail()
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                // 確認メッセージ
                // 画面の内容でメール送信します。\r\nよろしいですか？
                if (!this.ShowMessage("S0100050020").Equals(DialogResult.OK))
                {
                    return false;
                }

                // 入力チェック
                if (!this.CheckInputMail())
                {
                    // 入力チェックNG
                    return false;
                }

                // メール送信実行
                Cursor.Current = Cursors.WaitCursor;

                if (this.ExportExcel(true, ComDefine.PLANNING_EXCEL_OUTPUT_DIR, this.ReplaceMailContents(true, ComDefine.EXCEL_FILE_SHUKKA_KEIKAKU)))
                {
                    // メール送信しました。
                    this.ShowMessage("S0100050026");
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
            finally
            {
                Cursor.Current = preCursor;
            }
            return true;
        }

        #endregion

        #region メール送信用入力チェック及びリビジョン登録/更新

        /// --------------------------------------------------
        /// <summary>
        /// メール送信用入力チェック及びリビジョン登録/更新
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Gwon 2023/08/28</create>
        /// <update>J.Chen 2024/01/17 Rev初期値変更</update>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckInputMail()
        {
            try
            {
                var condS01 = new CondS01(this.UserInfo);
                var connS01 = new ConnS01();
                string errMsgID;
                string[] args;
                condS01.Version = this.revVersion;
                condS01.BukkenName = this.cboBukkenNameTorikomi.Text;

                //物件管理NO
                condS01.BukkenNO = this.BukkenNo;
                //SHIP製番
                condS01.ShipSeiban = this.ShipSeiban;
                //荷受け先
                condS01.ConsignCD = this.ConsignCD;
                //AssignComment
                condS01.AssignComment = this.txtBoxAssign.Text;

                //Rev
                if (!string.IsNullOrEmpty(this.txtRevision.Text))
                {
                    int intValue = int.Parse(this.txtRevision.Text);
                    int resultValue = intValue + 1;
                    string resultString = resultValue.ToString("D3");
                    condS01.BukkenRev = resultString;
                }
                else
                {
                    ////Revを入力して下さい。
                    //this.ShowMessage("S0100050022");
                    //return false;
                    condS01.BukkenRev = REV_INITIAL_VALUE;
                }

                // メールアドレスチェック
                var conn = new ConnCommon();
                var cond = new CondCommon(this.UserInfo);
                cond.ConsignCD = this.ConsignCD;
                var ds = conn.CheckPlanningMail(cond);

                if (string.IsNullOrEmpty(UtilData.GetFld(ds, Def_M_USER.Name, 0, Def_M_USER.MAIL_ADDRESS)))
                {
                    // 担当者にMailAddressが設定されていません。
                    this.ShowMessage("A0100010010");
                    return false;
                }

                if (!UtilData.ExistsData(ds, Def_M_CONSIGN_MAIL.Name))
                {
                    // 出荷計画表メール送信対象者が設定されていません。
                    this.ShowMessage("S0100010019");
                    return false;
                }

                //メールアドレスリスト(宛先)
                this._dtPlanningUser = ds.Tables[Def_M_CONSIGN_MAIL.Name];
                this._mailAddress = UtilData.GetFld(ds, Def_M_USER.Name, 0, Def_M_USER.MAIL_ADDRESS);

                //リビジョン登録(既存リビジョンがある場合は更新)
                if (!connS01.InsertRevision(condS01, out errMsgID, out args))
                {
                    this.ShowMessage(errMsgID, args);
                    return false;
                }

                DataSet dsTmp = connS01.GetNonyusaki(condS01);
                var revTable = dsTmp.Tables[Def_M_MAIL_SEND_RIREKI.Name];
                if (revTable != null && revTable.Rows.Count > 0)
                {
                    this.Revision = ComFunc.GetFld(revTable, 0, Def_M_MAIL_SEND_RIREKI.BUKKEN_REV);
                    this.txtRevision.Text = this.Revision;
                    this.txtBoxAssign.Text = ComFunc.GetFld(revTable, 0, Def_M_MAIL_SEND_RIREKI.ASSIGN_COMMENT);
                    this.revVersion = ComFunc.GetFldToDateTime(revTable, 0, Def_M_MAIL_SEND_RIREKI.VERSION).ToString("yyyy/MM/dd HH:mm:ss");
                }
                else
                {
                    this.Revision = null;
                    this.revVersion = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region 制御メソッド(Excel出力のみ)

        /// --------------------------------------------------
        /// <summary>
        /// Excel出力のみ
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>T.SASAYAMA 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RunExcelOnly()
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                using (SaveFileDialog frm = new SaveFileDialog())
                {
                    frm.Title = Resources.ShukkaKeikaku_sfdExcel_Title;
                    frm.Filter = Resources.ShukkaKeikaku_sdfExcel_Filter;

                    ////frm.FileName = BukkenName + "DAIFUKU MECHATRONICS SINGAPORE PTE LTD.(GF Fab 7H)";
                    frm.FileName = this.ReplaceMailContents(true, ComDefine.EXCEL_FILE_SHUKKA_KEIKAKU);

                    if (0 < this.shtMeisai.MaxRows && frm.ShowDialog() != DialogResult.OK) return false;

                    // Export Excel file
                    Cursor.Current = Cursors.WaitCursor;
                    var path = Path.GetDirectoryName(frm.FileName);
                    var fileName = Path.GetFileName(frm.FileName);
                    if (this.ExportExcel(false, path, fileName))
                    {
                        // 出荷計画表ExcelFilesを出力しました。
                        this.ShowMessage("S0100010020");
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
            finally
            {
                Cursor.Current = preCursor;
            }
            return true;
        }

        #endregion

        #region Excel出力

        /// --------------------------------------------------
        /// <summary>
        /// Excel出力
        /// </summary>
        /// <param name="isMail">true:Excel+Mail, false:Excel only</param>
        /// <param name="path">保存フォルダ</param>
        /// <param name="fileName">保存ファイル名</param>
        /// <returns></returns>
        /// <create>T.SASAYAMA 2023/08/24</create>
        /// --------------------------------------------------
        private bool ExportExcel(bool isMail, string path, string fileName)
        {
            bool ret = false;
            string mailID = string.Empty;
            var filePath = Path.Combine(path, fileName);
            try
            {
                //エクセルファイル臨時置き場であるローカルディレクトリを削除
                this.ClearDirectory(ComDefine.PLANNING_EXCEL_OUTPUT_DIR);

                // Excelファイルダウンロード
                if (!this.ExcelTemplateFileDownload())
                {
                    // テンプレートファイルのダウンロードに失敗しました。
                    this.ShowMessage("S0100050024");
                    return false;
                }

                var conn = new ConnS01();
                var cond = new CondS01(this.UserInfo);
                cond.BukkenNO = BukkenNo;
                cond.ConsignCD = ConsignCD;
                cond.ShipSeiban = ShipSeiban;
                cond.UpdateUserID = this.UserInfo.UserID;
                cond.UpdateUserName = this.UserInfo.UserName;

                string errMsgID;
                string[] args;

                // Excel出力データ取得
                DataTable shukkaDt = (this.shtMeisai.DataSource as DataTable).Copy();
                DataSet ds = conn.GetMailSousinRireki(cond);
                DataTable revDt = ds.Tables[Def_M_MAIL_SEND_RIREKI.Name];

                //BUKKEN_REVの書き換え
                foreach (DataRow row in revDt.Rows)
                {
                    row[Def_M_MAIL_SEND_RIREKI.BUKKEN_REV] = "Rev." + row[Def_M_MAIL_SEND_RIREKI.BUKKEN_REV].ToString();
                }
                //ESTIMATE_FLAGの書き換え
                foreach (DataRow row in shukkaDt.Rows)
                {
                    if (this.UserInfo.Language == "US")
                    {
                        if (Convert.ToInt16(row[Def_M_NONYUSAKI.ESTIMATE_FLAG]) == Convert.ToInt16(EstimateFlag.Mushou))
                        {
                            row[Def_M_NONYUSAKI.ESTIMATE_FLAG] = "Non-commercial";

                        }
                        else if (Convert.ToInt16(row[Def_M_NONYUSAKI.ESTIMATE_FLAG]) == Convert.ToInt16(EstimateFlag.Yushou))
                        {
                            row[Def_M_NONYUSAKI.ESTIMATE_FLAG] = "Commercial";
                        }

                        if (Convert.ToInt16(row[Def_M_NONYUSAKI.LAST_SYORI_FLAG]) == Convert.ToInt16(SHIPPING_PLAN_EXCEL_TYPE.NONE_VALUE1))
                        {
                            row[Def_M_NONYUSAKI.LAST_SYORI_FLAG] = "None";
                        }
                        else if (Convert.ToInt16(row[Def_M_NONYUSAKI.LAST_SYORI_FLAG]) == Convert.ToInt16(SHIPPING_PLAN_EXCEL_TYPE.MODIFY_VALUE1))
                        {
                            row[Def_M_NONYUSAKI.LAST_SYORI_FLAG] = "Add/Edit";
                        }
                        else if (Convert.ToInt16(row[Def_M_NONYUSAKI.LAST_SYORI_FLAG]) == Convert.ToInt16(SHIPPING_PLAN_EXCEL_TYPE.DELETE_VALUE1))
                        {
                            row[Def_M_NONYUSAKI.LAST_SYORI_FLAG] = "Delete";
                        }
                    }
                    else
                    {
                        if (Convert.ToInt16(row[Def_M_NONYUSAKI.ESTIMATE_FLAG]) == Convert.ToInt16(EstimateFlag.Mushou))
                        {
                            row[Def_M_NONYUSAKI.ESTIMATE_FLAG] = "無償";

                        }
                        else if (Convert.ToInt16(row[Def_M_NONYUSAKI.ESTIMATE_FLAG]) == Convert.ToInt16(EstimateFlag.Yushou))
                        {
                            row[Def_M_NONYUSAKI.ESTIMATE_FLAG] = "有償";
                        }

                        if (Convert.ToInt16(row[Def_M_NONYUSAKI.LAST_SYORI_FLAG]) == Convert.ToInt16(SHIPPING_PLAN_EXCEL_TYPE.NONE_VALUE1))
                        {
                            row[Def_M_NONYUSAKI.LAST_SYORI_FLAG] = "変更なし";
                        }
                        else if (Convert.ToInt16(row[Def_M_NONYUSAKI.LAST_SYORI_FLAG]) == Convert.ToInt16(SHIPPING_PLAN_EXCEL_TYPE.MODIFY_VALUE1))
                        {
                            row[Def_M_NONYUSAKI.LAST_SYORI_FLAG] = "追加/変更";
                        }
                        else if (Convert.ToInt16(row[Def_M_NONYUSAKI.LAST_SYORI_FLAG]) == Convert.ToInt16(SHIPPING_PLAN_EXCEL_TYPE.DELETE_VALUE1))
                        {
                            row[Def_M_NONYUSAKI.LAST_SYORI_FLAG] = "削除";
                        }
                    }
                }

                //for (int rowIndex = 0; rowIndex < dtPacking.Rows.Count; rowIndex++)
                //{
                //    UtilData.SetFld(dtPacking, rowIndex, ComDefine.FLD_ROW_INDEX, rowIndex + 1);
                //    UtilData.SetFld(dtPacking, rowIndex, "", "");
                //    UtilData.SetFld(dtPacking, rowIndex, Def_T_PACKING.PACKING_REV, "");
                //}

                // Excel出力
                var export = new ExportPacking();
                export.ExportExcelShukka(filePath, shukkaDt, revDt, out errMsgID, out args);
                if (!string.IsNullOrEmpty(errMsgID))
                {
                    // エラーメッセージ出力
                    this.ShowMessage(errMsgID, args);
                    return false;
                }

                // Excel出力のみであれば処理を抜ける
                if (!isMail) return true;

                // MAIL_ID採番
                mailID = conn.GetMailID(cond, out errMsgID, out args);
                if (string.IsNullOrEmpty(mailID))
                {
                    this.ShowMessage(errMsgID, args);
                    return false;
                }

                // ファイルアップロード
                if (!this.PlanningFileUpload(filePath, fileName, mailID))
                {
                    // アップロードに失敗しました。
                    this.ShowMessage("S0100050025");
                    return false;
                }

                // メールデータ作成
                var dsTmp = new DataSet();
                dsTmp.Merge(this.CreateMailData(mailID, fileName));

                // DB更新
                if (!conn.UpdPlanning(cond, dsTmp, out errMsgID, out args))
                {
                    this.ShowMessage(errMsgID, args);
                    return false;
                }

                ret = true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (!ret)
                {
                    this.PlanningFileDelete(fileName, mailID);
                }
            }
            return ret;
        }

        #endregion

        #region ファイルダウンロード

        /// --------------------------------------------------
        /// <summary>
        /// ファイルダウンロード
        /// </summary>
        /// <returns></returns>
        /// <create>T.SASAYAMA 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ExcelTemplateFileDownload()
        {
            bool ret = false;
            try
            {
                var conn = new ConnAttachFile();
                var dlPackage = new FileDownloadPackage();

                dlPackage.FileName = ComDefine.EXCEL_FILE_SHUKKAKEIKAKU_TEMPLATE;
                dlPackage.FileType = FileType.Template;

                // TemplateファイルのDL
                var retFile = conn.FileDownload(dlPackage);
                if (retFile.IsExistsFile)
                {
                    if (!Directory.Exists(ComDefine.DOWNLOAD_DIR))
                    {
                        Directory.CreateDirectory(ComDefine.DOWNLOAD_DIR);
                    }
                    using (var fs = new FileStream(Path.Combine(ComDefine.DOWNLOAD_DIR, ComDefine.EXCEL_FILE_SHUKKAKEIKAKU_TEMPLATE), FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(retFile.FileData, 0, retFile.FileData.Length);
                        fs.Close();
                    }
                    ret = true;
                }
                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ファイルアップロード

        /// --------------------------------------------------
        /// <summary>
        /// ファイルアップロード
        /// </summary>
        /// <param name="filePath">アップロード元ファイル</param>
        /// <param name="fileName">アップロード先ファイル名</param>
        /// <param name="mailID">メールID</param>
        /// <returns></returns>
        /// <create>Y.Gwon 2023/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool PlanningFileUpload(string filePath, string fileName, string mailID)
        {
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Open))
                {
                    var conn = new ConnAttachFile();
                    var updPackage = new FileUploadPackage();

                    var data = new byte[fs.Length];
                    fs.Position = 0;
                    fs.Read(data, 0, (int)fs.Length);
                    updPackage.FileData = data;
                    updPackage.FileName = fileName;
                    updPackage.FileType = FileType.Attachments;
                    updPackage.FolderName = mailID;
                    updPackage.GirenType = GirenType.None;

                    var updResult = conn.FileUpload(updPackage);
                    if (!updResult.IsSuccess)
                    {
                        // 失敗
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region メール登録用データ作成

        /// --------------------------------------------------
        /// <summary>
        /// メール登録用データ作成
        /// </summary>
        /// <returns></returns>
        /// <create>Y.Gwon 2023/08/28</create>
        /// --------------------------------------------------
        private DataTable CreateMailData(string mailID, string fileName)
        {
            // メールテンプレートの内容を取得
            ConnAttachFile attachFile = new ConnAttachFile(this.UserInfo.Language);
            string title = attachFile.GetMailTemplate(MAIL_FILE.SHUKKA_KEIKAKU_LIST_TITLE_VALUE1);
            string naiyo = attachFile.GetMailTemplate(MAIL_FILE.SHUKKA_KEIKAKU_LIST_VALUE1);

            var dt = ComFunc.GetSchemeMail();
            var dr = dt.NewRow();
            dr.SetField<object>(Def_T_MAIL.MAIL_ID, mailID);
            dr.SetField<object>(Def_T_MAIL.MAIL_SEND, this._mailAddress);
            dr.SetField<object>(Def_T_MAIL.MAIL_SEND_DISPLAY, this.UserInfo.UserName);
            dr.SetField<object>(Def_T_MAIL.MAIL_TO, ComFunc.GetMailUser(this._dtPlanningUser, MAIL_ADDRESS_FLAG.TO_VALUE1, Def_M_USER.MAIL_ADDRESS));
            dr.SetField<object>(Def_T_MAIL.MAIL_TO_DISPLAY, ComFunc.GetMailUser(this._dtPlanningUser, MAIL_ADDRESS_FLAG.TO_VALUE1, Def_M_USER.USER_NAME));
            dr.SetField<object>(Def_T_MAIL.MAIL_CC, ComFunc.GetMailUser(this._dtPlanningUser, MAIL_ADDRESS_FLAG.CC_VALUE1, Def_M_USER.MAIL_ADDRESS));
            dr.SetField<object>(Def_T_MAIL.MAIL_CC_DISPLAY, ComFunc.GetMailUser(this._dtPlanningUser, MAIL_ADDRESS_FLAG.CC_VALUE1, Def_M_USER.USER_NAME));
            dr.SetField<object>(Def_T_MAIL.MAIL_BCC, ComFunc.GetMailUser(this._dtPlanningUser, MAIL_ADDRESS_FLAG.BCC_VALUE1, Def_M_USER.MAIL_ADDRESS));
            dr.SetField<object>(Def_T_MAIL.MAIL_BCC_DISPLAY, ComFunc.GetMailUser(this._dtPlanningUser, MAIL_ADDRESS_FLAG.BCC_VALUE1, Def_M_USER.USER_NAME));
            dr.SetField<object>(Def_T_MAIL.TITLE, this.ReplaceMailContents(false, title));
            dr.SetField<object>(Def_T_MAIL.NAIYO, this.ReplaceMailContents(false, naiyo));
            dr.SetField<object>(Def_T_MAIL.MAIL_STATUS, MAIL_STATUS.MI_VALUE1);
            dr.SetField<object>(Def_T_MAIL.RETRY_COUNT, 0);
            dr.SetField<object>(Def_T_MAIL.APPENDIX_FILE_PATH, Path.Combine(mailID, fileName));
            dt.Rows.Add(dr);

            return dt;
        }

        #endregion

        #region テンプレートのデータを置換

        /// --------------------------------------------------
        /// <summary>
        /// テンプレートのデータを置換
        /// </summary>
        /// <param name="mailContents"></param>
        /// <returns></returns>
        /// <create>Y.Gwon 2023/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private string ReplaceMailContents(bool isFile, string mailContents)
        {
            //エクセルファイル名置換の場合
            if (isFile)
            {
                //ファイル名に使用できない文字を取得
                char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();

                //エクセルファイル名取得
                mailContents = string.Format(mailContents
                    , BukkenName, ConsignName, Revision, DateTime.Now);

                //メール送信日付取得
                this._mailDate = DateTime.Now.ToString("yyyy/MM/dd");

                //ファイル名として使用できない文字が存在した場合、自動省略
                while (true)
                {
                    if (mailContents.IndexOfAny(invalidChars) >= 0)
                    {
                        string invalidString = mailContents[mailContents.IndexOfAny(invalidChars)].ToString();
                        mailContents = mailContents.Replace(invalidString, "");
                    }
                    else break;
                }

                return mailContents;
            }

            return mailContents
                .Replace(MAIL_RESERVE.BUKKEN_NAME_VALUE1, this.BukkenName)
                .Replace(MAIL_RESERVE.NIUKE_SAKI_VALUE1, this.ConsignName)
                .Replace(MAIL_RESERVE.REVISION_VALUE1, this.Revision)
                .Replace(MAIL_RESERVE.ASSIGN_COMMENT_VALUE1, this.txtBoxAssign.Text.Replace("\r\n", "\r\n\t\t"))
                .Replace(MAIL_RESERVE.SENDER_VALUE1, this.UserInfo.UserName)
                .Replace(MAIL_RESERVE.MAIL_SOUSIN_DATE_VALUE1, this._mailDate);
        }

        #endregion

        #region ファイル削除

        /// --------------------------------------------------
        /// <summary>
        /// ファイル削除
        /// </summary>
        /// <param name="fileName">アップロード先ファイル名</param>
        /// <param name="mailID">メールID</param>
        /// <returns></returns>
        /// <create>Y.Gwon 2023/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool PlanningFileDelete(string fileName, string mailID)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(mailID))
            {
                return true;
            }

            try
            {
                var conn = new ConnAttachFile();
                var delPackage = new FileDeletePackage();

                delPackage.FileName = fileName;
                delPackage.FileType = FileType.Attachments;
                delPackage.FolderName = mailID;
                delPackage.GirenType = GirenType.None;

                var result = conn.FileDelete(delPackage);
                if (!result.IsSuccess)
                {
                    // 失敗
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ディレクトリ削除

        /// --------------------------------------------------
        /// <summary>
        /// エクセルファイル臨時置き場であるローカルディレクトリを削除
        /// </summary>
        /// <param name="dirPath">ディレクトリパス</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Gwon 2023/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool ClearDirectory(string dirPath)
        {
            try
            {
                if (Directory.Exists(dirPath))
                {
                    Directory.Delete(dirPath, true);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #endregion

        #region 入力キー制御

        /// --------------------------------------------------
        /// <summary>
        /// textBox1
        /// </summary>
        /// <create>T.SASAYAMA 2023/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 数字と制御キー（バックスペースなど）を除いて入力をキャンセル
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // 入力をキャンセル
            }
        }

        #endregion

        #region 排他クリア

        /// --------------------------------------------------
        /// <summary>
        /// 排他クリア
        /// </summary>
        /// <create>J.Chen 2024/01/17</create>
        /// <update>J.Chen 2024/08/07 クリア対象は案件→USERに変更</update>
        /// <update></update>
        /// --------------------------------------------------
        private void HaitaClear()
        {
            // 排他中であれば
            if (_isHaita)
            {
                // 排他クリア
                CondS01 cond = new CondS01(this.UserInfo);
                ConnS01 conn = new ConnS01();
                //cond.BukkenName = this.cboBukkenNameTorikomi.Text;
                //cond.ConsignCD = this.ConsignCD;
                int result = conn.UpdateNullHaita(cond);
                _isHaita = false;
            }
        }

        #endregion

        #region 画面終了

        /// --------------------------------------------------
        /// <summary>
        /// 画面終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>J.Chen 2024/01/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ShukkaKeikaku_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 排他クリア
            HaitaClear();
        }

        #endregion

        #region 読み取り専用状態

        /// --------------------------------------------------
        /// <summary>
        /// 読み取り専用状態
        /// </summary>
        /// <create>J.Chen 2024/01/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ReadOnlyMode()
        {
            if (_isReadOnly == true)
            {
                this.cboNiukesaki.Enabled = false;

                this.fbrFunction.F01Button.Enabled = false;
                this.fbrFunction.F02Button.Enabled = false;
                this.fbrFunction.F03Button.Enabled = false;
                this.fbrFunction.F08Button.Enabled = false;
                this.fbrFunction.F09Button.Enabled = false;

                for (int i = 0; i < this.colIndexMax; i++)
                {
                    this.shtMeisai.Columns[i].Enabled = false;
                }

                this.txtShipSeiban.ReadOnly = true;
                this.txtBoxAssign.ReadOnly = true;

            }

        }

        #endregion

    }
}
