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
using WsConnection.WebRefP02;
using WsConnection.WebRefK01;

namespace SMS.S01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細登録
    /// </summary>
    /// <create>Y.Higuchi 2010/07/06</create>
    /// <update>2023/12/06 R.Kubota TAG発行・照会画面と同じのTAG状態集計を追加</update>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ShukkaKeikakuMeisai : SystemBase.Forms.CustomOrderForm
    {
        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// TagNo.のカラム
        /// </summary>
        /// <create>Y.Higuchi 2010/07/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TAGNO = 1;
        /// --------------------------------------------------
        /// <summary>
        /// ARNo.のカラム
        /// </summary>
        /// <create>T.Nakata 2018/11/14</create>
        /// <update>H.Iimuro 2022/10/04 図面/型式２の追加対応</update>
        /// <update>J.Chen 2024/11/08 通関確認状態追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ARNO = 21;
        /// --------------------------------------------------
        /// <summary>
        /// 数量のカラム
        /// </summary>
        /// <create>T.Nakata 2018/11/14</create>
        /// <update>H.Iimuro 2022/10/04 図面/型式２の追加対応</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_NUM = 14;
        /// --------------------------------------------------
        /// <summary>
        /// 連携No.のカラム
        /// </summary>
        /// <create>T.Nakata 2018/11/14</create>
        /// <update>H.Iimuro 2022/10/04 図面/型式２の追加対応</update>
        /// <update>J.Chen 2024/11/08 通関確認状態追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_RENKEINO = 22;
        /// --------------------------------------------------
        /// <summary>
        /// 状態区分のカラム
        /// </summary>
        /// <create>Y.Higuchi 2010/07/07</create>
        /// <update>H.Tajimi 2015/11/20 備考対応</update>
        /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
        /// <update>T.Nakata 2018/11/13 手配業務対応</update>
        /// <update>H.Iimuro 2022/10/04 図面/型式２の追加対応</update>
        /// <update>J.Chen 2024/11/08 通関確認状態追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_JYOTAI_FLAG = 37;
        /// --------------------------------------------------
        /// <summary>
        /// 図番/型式のカラム
        /// </summary>
        /// <create>H.Tajimi 2019/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ZUMEN_KEISHIKI = 11;
        /// --------------------------------------------------
        /// <summary>
        /// 写真のカラム
        /// </summary>
        /// <create>H.Tajimi 2019/08/27</create>
        /// <update>H.Iimuro 2022/10/04 図面/型式２の追加対応</update>
        /// <update>J.Chen 2024/11/08 通関確認状態追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_PICTURE = 39;
        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>Y.Higuchi 2010/09/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 3;
        /// --------------------------------------------------
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
        /// <update>J.Chen 2024/11/08 通関確認状態追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_COPY_END = 22;
        /// --------------------------------------------------
        /// <summary>
        /// 検索を行う
        /// </summary>
        /// <create>H.Tajimi 2015/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int MODE_SEARCH = 1;
        /// --------------------------------------------------
        /// <summary>
        /// Excel取り込みを行う
        /// </summary>
        /// <create>H.Tajimi 2015/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int MODE_IMPORT_EXCEL = 2;
        /// --------------------------------------------------
        /// <summary>
        /// 品名のカラム
        /// </summary>
        /// <create>J.Chen 2023/02/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_HINME = 10;

        #endregion

        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// グリッドのEnabledを切り替える対象のカラム   
        /// </summary>
        /// <create>Y.Higuchi 2010/07/07</create>
        /// <update>K.Tsutsumi 2012/04/25</update>
        /// <update>H.Tajimi 2015/11/20 備考対応</update>
        /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
        /// <update>T.Nakata 2018/11/13 手配業務対応</update>
        /// <update>H.Iimuro 2022/10/04 図面/型式２の追加対応</update>
        /// <update>J.Chen 2024/11/08 通関確認状態追加</update>
        /// --------------------------------------------------
        private int[] _inputColumns = new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 };
        // ↑
        /// --------------------------------------------------
        /// <summary>
        /// グリッドの表示/非表示を切り替える対象のカラム
        /// </summary>
        /// <create>Y.Higuchi 2010/07/07</create>
        /// <update>H.Tajimi 2015/11/20 備考対応 途中にカラム挿入したので更新</update>
        /// <update>H.Tajimi 2015/12/09 M-NO対応 途中にカラム挿入したので更新</update>
        /// <update>T.Nakata 2018/11/13 手配業務対応</update>
        /// <update>H.Iimuro 2022/10/04 図面/型式２の追加対応</update>
        /// <update>J.Chen 2024/11/08 通関確認状態追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private int[] _hiddenColumns = new int[] { 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36 };
         /// --------------------------------------------------
        /// <summary>
        /// 完了納入先への明細追加フラグ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isAddDetailNonyusaki = false;
        /// --------------------------------------------------
        /// <summary>
        /// 完了ARへの明細追加フラグ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isAddDetailAR = false;
        /// --------------------------------------------------
        /// <summary>
        /// 納入先・便の存在時の明細追加フラグ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isAddDetailExists = false;
        /// --------------------------------------------------
        /// <summary>
        /// T_ARが存在しなくても続行するフラグ
        /// </summary>
        /// <create>K.Tsutsumi 2011/03/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isConfirmNoAR = false;

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分を"本体"へ戻すか、現状維持かを制御するフラグ
        /// </summary>
        /// <create>K.Tsutsumi 2012/04/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isContinueShukkaFlag = false;

        /// --------------------------------------------------
        /// <summary>
        /// 一度登録したレコードを削除したかどうかを制御するフラグ
        /// </summary>
        /// <create>K.Tsutsumi 2012/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isDeleteRecord = false;

        /// --------------------------------------------------
        /// <summary>
        /// TAG登録連携情報
        /// </summary>
        /// <create>T.Nakata 2018/11/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private TagRenkeiData _dtTagRenkeiData;

        /// --------------------------------------------------
        /// <summary>
        /// TAG連携フラグ
        /// </summary>
        /// <create>T.Nakata 2018/11/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isTagRenkei;

        /// --------------------------------------------------
        /// <summary>
        /// 連携後データ
        /// </summary>
        /// <create>H.Tajimi 2019/08/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _associatedData = null;

        /// --------------------------------------------------
        /// <summary>
        /// TAG連携結果
        /// </summary>
        /// <create>H.Tajimi 2019/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private DialogResult _tagRenkeiResult = DialogResult.Cancel;

        /// --------------------------------------------------
        /// <summary>
        /// 初回起動
        /// </summary>
        /// <create>J.Chen 2023/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isShukkaKeikaku = false;

        #endregion

        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// Excelの文字属性タイプ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum ExcelInputAttrType
        {
            TagNo = 0,
            AlphaNum = 1,
            WideString = 2,
            Numeric = 3,
            Float = 4,
        }

        #endregion

        #region プロパティ

        /// --------------------------------------------------
        /// <summary>
        /// TAG連携データ
        /// </summary>
        /// <create>H.Tajimi 2019/08/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataTable AssociatedData
        {
            get { return this._associatedData; }
            private set { this._associatedData = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// TAG連携結果
        /// </summary>
        /// <create>H.Tajimi 2019/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public DialogResult TagRenkeiResult
        {
            get { return this._tagRenkeiResult; }
            private set { this._tagRenkeiResult = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// インスタンス
        /// </summary>
        /// <create>J.Chen 2023/08/28</create>
        /// --------------------------------------------------
        public static ShukkaKeikakuMeisai ShukkaKeikakuMeisaiInstance { get; set; }
        /// --------------------------------------------------
        /// <summary>
        /// 物件名
        /// </summary>
        /// <create>J.Chen 2023/08/28</create>
        /// --------------------------------------------------
        public string BukkenNameText { get; set; }
        /// --------------------------------------------------
        /// <summary>
        /// 便
        /// </summary>
        /// <create>J.Chen 2023/08/28</create>
        /// --------------------------------------------------
        public string ShipText { get; set; }

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
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update>T.Nakata 2018/11/15 TAG連携対応</update>
        /// <update></update>
        /// --------------------------------------------------
        public ShukkaKeikakuMeisai(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
            : base(userInfo, menuCategoryID, menuItemID, title)
        {
            this._isTagRenkei = false;
            this._dtTagRenkeiData = null;

            InitializeComponent();
        }
        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="menuCategoryID">メニュー種別ID</param>
        /// <param name="menuItemID">メニュー項目ID</param>
        /// <param name="title">画面タイトル</param>
        /// <create>T.Nakata 2018/11/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public ShukkaKeikakuMeisai(UserInfo userInfo, string menuCategoryID, string menuItemID, string title, TagRenkeiData TagRenkeiData)
            : base(userInfo, menuCategoryID, menuItemID, title)
        {
            if (TagRenkeiData != null)
            {
                this._isTagRenkei = true;
                this._dtTagRenkeiData = TagRenkeiData;
            }
            else
            {
                this._isTagRenkei = false;
                this._dtTagRenkeiData = null;
            }

            InitializeComponent();
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update>H.Tsuji 2019/08/01 Enterキー入力時のアクティブセル移動方向を制御</update>
        /// <update>H.Tajimi 2019/08/09 写真管理方式変更</update>
        /// <update>H.Iimuro 2022/10/04 図面/型式２の追加対応</update>
        /// <update>J.Chen 2022/12/19 TAG便名追加</update>
        /// <update>J.Chen 2024/11/08 通関確認状態追加</update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // フォームの設定
                this.IsCloseQuestion = true;
                // シートの初期化
                this.InitializeSheet(this.shtMeisai);
                // シートのタイトルを設定
                shtMeisai.ColumnHeaders[0].Caption = Resources.ShukkaKeikakuMeisai_State;
                shtMeisai.ColumnHeaders[1].Caption = Resources.ShukkaKeikakuMeisai_TagNo;
                shtMeisai.ColumnHeaders[2].Caption = Resources.ShukkaKeikakuMeisai_ProductNo;
                shtMeisai.ColumnHeaders[3].Caption = Resources.ShukkaKeikakuMeisai_Code;
                shtMeisai.ColumnHeaders[4].Caption = Resources.ShukkaKeikakuMeisai_DrawingAdditionalNo;
                shtMeisai.ColumnHeaders[5].Caption = Resources.ShukkaKeikakuMeisai_Area;
                shtMeisai.ColumnHeaders[6].Caption = Resources.ShukkaKeikakuMeisai_Floor;
                shtMeisai.ColumnHeaders[7].Caption = Resources.ShukkaKeikakuMeisai_Model;
                shtMeisai.ColumnHeaders[8].Caption = Resources.ShukkaKeikakuMeisai_MNo;
                shtMeisai.ColumnHeaders[9].Caption = Resources.ShukkaKeikakuMeisai_JpName;
                shtMeisai.ColumnHeaders[10].Caption = Resources.ShukkaKeikakuMeisai_Name;
                shtMeisai.ColumnHeaders[11].Caption = Resources.ShukkaKeikakuMeisai_DrawingNoFormat;
                shtMeisai.ColumnHeaders[12].Caption = Resources.ShukkaKeikakuMeisai_ZUMEN_KEISHIKI2;
                shtMeisai.ColumnHeaders[13].Caption = Resources.ShukkaKeikakuMeisai_SectioningNo;
                shtMeisai.ColumnHeaders[14].Caption = Resources.ShukkaKeikakuMeisai_Quantity;
                shtMeisai.ColumnHeaders[15].Caption = Resources.ShukkaKeikakuMeisai_Grwt;
                shtMeisai.ColumnHeaders[16].Caption = Resources.ShukkaKeikakuMeisai_Free1;
                shtMeisai.ColumnHeaders[17].Caption = Resources.ShukkaKeikakuMeisai_Free2;
                shtMeisai.ColumnHeaders[18].Caption = Resources.ShukkaKeikakuMeisai_Memo;
                shtMeisai.ColumnHeaders[19].Caption = Resources.ShukkaKeikakuMeisai_CustomsStatus;
                shtMeisai.ColumnHeaders[20].Caption = Resources.ShukkaKeikakuMeisai_STNo;
                shtMeisai.ColumnHeaders[21].Caption = Resources.ShukkaKeikakuMeisai_ArNo;
                shtMeisai.ColumnHeaders[22].Caption = Resources.ShukkaKeikakuMeisai_TehaiRenkeiNo;
                shtMeisai.ColumnHeaders[23].Caption = Resources.ShukkaKeikakuMeisai_AssemblyDate;
                shtMeisai.ColumnHeaders[24].Caption = Resources.ShukkaKeikakuMeisai_BoxNo;
                shtMeisai.ColumnHeaders[25].Caption = Resources.ShukkaKeikakuMeisai_PalletNo;
                shtMeisai.ColumnHeaders[26].Caption = Resources.ShukkaKeikakuMeisai_WoodFrameNo;
                shtMeisai.ColumnHeaders[27].Caption = Resources.ShukkaKeikakuMeisai_BoxPackingDate;
                shtMeisai.ColumnHeaders[28].Caption = Resources.ShukkaKeikakuMeisai_PalletPackingDate;
                shtMeisai.ColumnHeaders[29].Caption = Resources.ShukkaKeikakuMeisai_WoodFramePackingDate;
                shtMeisai.ColumnHeaders[30].Caption = Resources.ShukkaKeikakuMeisai_ShippingDate;
                shtMeisai.ColumnHeaders[31].Caption = Resources.ShukkaKeikakuMeisai_ShippingCompany;
                shtMeisai.ColumnHeaders[32].Caption = Resources.ShukkaKeikakuMeisai_InvoiceNo;
                shtMeisai.ColumnHeaders[33].Caption = Resources.ShukkaKeikakuMeisai_OkurijyoNo;
                shtMeisai.ColumnHeaders[34].Caption = Resources.ShukkaKeikakuMeisai_BLNo;
                shtMeisai.ColumnHeaders[35].Caption = Resources.ShukkaKeikakuMeisai_AcceptanceDate;
                shtMeisai.ColumnHeaders[36].Caption = Resources.ShukkaKeikakuMeisai_AcceptanceStaff;
                shtMeisai.ColumnHeaders[38].Caption = Resources.ShukkaKeikakuMeisai_Version;
                shtMeisai.ColumnHeaders[39].Caption = Resources.ShukkaKeikakuMeisai_Picture;
                shtMeisai.Columns[39].AlignHorizontal = AlignHorizontal.Center;
                shtMeisai.Columns[39].AlignVertical = AlignVertical.Middle;
                shtMeisai.ColumnHeaders[40].Caption = Resources.ShukkaKeikakuMeisai_SipFromName;
                shtMeisai.ColumnHeaders[41].Caption = Resources.ShukkaKeikakuMeisai_TagShip;
                // コンボボックスの初期化
                this.MakeCmbBox(this.cboShukkaFlag, SHUKKA_FLAG.GROUPCD);
                this.MakeCmbBox(this.cboDispSelect, DISP_SELECT.GROUPCD);

                if (this._isTagRenkei == false)
                {
                    // モード切替
                    this.ChangeMode();
                }
                else
                {
                    // TAG連携時初期処理
                    InitializeTagrenkeiControl();
                }

                // Enterキー操作ラジオボタン初期化
                // ラジオボタンはチェックするとTabStopも自動でtrueとなるため、その都度falseにする必要がある
                this.rdoRight.Checked = true;
                this.rdoRight.TabStop = false;
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
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update>J.Chen 2023/08/29 外部連携</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカスの設定
                this.rdoInsert.Focus();

                if (_isShukkaKeikaku)
                {
                    this.rdoView.Focus();
                    this.rdoView.Checked = true;
                    ChangeMode();
                    this.btnStart_Click(null, null);
                }
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
        /// 画面クリア処理
        /// </summary>
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update>H.Tajimi 2015/11/30 納入先選択のUI改善</update>
        /// <update>H.Tajimi 2015/12/08 System全般/Clearボタン押下時の範囲</update>
        /// <update>J.Chen 2023/08/29 外部連携</update>
        /// <update>2023/12/06 R.Kubota TAG発行・照会画面と同じのTAG状態集計を追加</update>
        /// <update>J.Chen 2024/10/02 「引渡/集荷済」のTAG状態集計を追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // ----- クリア -----
                // グリッドのクリア
                this.SheetClear();
                // フラグのクリア
                this._isAddDetailNonyusaki = false;
                this._isAddDetailAR = false;
                this._isAddDetailExists = false;
                // 2011/03/08 K.Tsutsumi Add T_ARが存在しなくても続行可能
                this._isConfirmNoAR = false;
                // ↑
                this.txtHikiwatashiShuka.Text = "0/0";
                this.txtHikiwatashi.Text = "0/0";
                this.txtShuka.Text = "0/0";
                this.txtBoxKonpo.Text = "0/0";
                this.txtPalletKonpo.Text = "0/0";
                // ↑
                // 2012/05/07 K.Tsutsumi Add 履歴
                this._isDeleteRecord = false;
                // ↑
                // 2015/11/30 H.Tajimi 納入先選択のUI改善
                if (this.EditMode == SystemBase.EditMode.View)
                {
                    // 照会モード時のみ、検索条件のロックと連動させていないため個別に状態を変更
                    this.ChangeEnabledForViewMode(true);
                }
                // ↑
                // 処理モード
                this.rdoInsert.Checked = true;
                // 明細データ
                this.txtExcel.Text = string.Empty;

                // 出荷区分
                // 2012/04/24 K.Tsutsumi Add 保存後は出荷区分はクリアせず、現状維持
                if (this._isContinueShukkaFlag == false)
                {
                    // ↑
                    if (0 < this.cboShukkaFlag.Items.Count)
                    {
                        this.cboShukkaFlag.SelectedValue = SHUKKA_FLAG.DEFAULT_VALUE1;
                    }
                    else
                    {
                        this.cboShukkaFlag.SelectedIndex = -1;
                    }
                    // 2012/05/07 K.Tsutsumi Add 保存後は出荷区分はクリアせず、現状維持
                }
                // ↑

                // 2015/12/08 H.Tajimi 全項目をクリアする
                // 2013/09/04 納入先・便・管理No.は入力値を保持する。
                // 納入先
                //this.txtNonyusakiName.Text = string.Empty;
                // 便
                //this.txtShip.Text = string.Empty;
                // 管理No.
                //this.txtNonyusakiCD.Text = string.Empty;
                // 納入先
                this.txtNonyusakiName.Text = string.Empty;
                // 便
                this.txtShip.Text = string.Empty;
                // 管理No.
                this.txtNonyusakiCD.Text = string.Empty;
                // ↑

                // AR No.
                this.txtARNo.Text = string.Empty;
                // 表示選択
                if (0 < this.cboDispSelect.Items.Count)
                {
                    this.cboDispSelect.SelectedValue = DISP_SELECT.DEFAULT_VALUE1;
                }
                else
                {
                    this.cboDispSelect.SelectedIndex = -1;
                }
                // 検索条件のロック解除
                this.grpSearch.Enabled = true;
                // ファンクションボタンの切替
                this.ChangeFunctionButton(false);
                // フォーカス移動
                this.rdoInsert.Focus();

                this.txtNonyusakiName.Clear();
                this.txtShip.Clear();

                this.txtNonyusakiName.Text = this.BukkenNameText ?? this.txtNonyusakiName.Text;
                this.txtShip.Text = this.ShipText ?? this.txtShip.Text;

                if (this.BukkenNameText != null && this.ShipText != null)
                {
                    this.BukkenNameText = null;
                    this.ShipText = null;
                    _isShukkaKeikaku = true;
                }

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update>J.Chen 2023/02/22 品名半角チェック</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                if (this.shtMeisai.EditState) return false;
                // 2012/04/25 K.Tsutsumi Delete TagNo自動採番
                //List<string> tagNoList = new List<string>();
                // ↑
                DataTable dt = this.shtMeisai.DataSource as DataTable;
                if (dt == null) return false;
                DataTable dtCheck = dt.Copy();
                if (dtCheck.Rows.Count == 0)
                {
                    // 明細が入力されていません。
                    this.ShowMessage("A9999999028");
                    return false;
                }
                dtCheck.AcceptChanges();
                // 2012/04/25 K.Tsutsumi Delete TagNo自動採番
                //for (int i = 0; i < dtCheck.Rows.Count; i++)
                //{
                //    string noFormatTagNo = ComFunc.GetFld(dtCheck, i, Def_T_SHUKKA_MEISAI.TAG_NO);
                //    string tagNo = this.GetFormatTagNo(noFormatTagNo);
                //    // フォーマット後一応再セット
                //    dtCheck.Rows[i][Def_T_SHUKKA_MEISAI.TAG_NO] = tagNo;
                //    if (string.IsNullOrEmpty(tagNo))
                //    {
                //        // {0}行目のTagNo.が入力されていません。
                //        this.ShowMessage("S0100020027", (i + 1).ToString(), this.shtMeisai.ColumnHeaders[SHEET_COL_TAGNO].Caption);
                //        this.shtMeisai.ActivePosition = new Position(SHEET_COL_TAGNO, i);
                //        this.shtMeisai.Focus();
                //        return false;
                //    }
                //    else if (tagNoList.Contains(tagNo))
                //    {
                //        // {0}行目のTagNo.[{1}]は存在しています。
                //        this.ShowMessage("S0100020002", (i + 1).ToString(), ComFunc.GetFld(dtCheck, i, Def_T_SHUKKA_MEISAI.TAG_NO));
                //        this.shtMeisai.ActivePosition = new Position(SHEET_COL_TAGNO, i);
                //        this.shtMeisai.Focus();
                //        return false;
                //    }
                //    tagNoList.Add(tagNo);
                //}
                // ↑

                // ↓ 2018/11/14 T.Nakata 複数AR対応
                if (this.cboShukkaFlag.SelectedValue != null && this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.AR_VALUE1)
                {
                    for (int i = 0; i < this.shtMeisai.Rows.Count-1; i++ )
                    {
                        bool IsError = false;
                        if (this.shtMeisai[SHEET_COL_ARNO, i].Value != null)
                        {
                            string check = this.shtMeisai[SHEET_COL_ARNO, i].Text.Trim();
                            int arval = 0;
                            if (check.Length != 6 || !check.StartsWith(this.lblAR.Text) || !int.TryParse(check.Substring(2, 4), out arval))
                            {
                                IsError = true;
                            }
                        }
                        else
                        {
                            IsError = true;
                        }
                        if(IsError)
                        {
                            if (this.rdoInsert.Checked || this.rdoExcel.Checked)
                            {
                                // {0}行目のAR Noの入力が不正です。
                                this.ShowMessage("S0100020046", (i + 1).ToString());
                                this.shtMeisai.ActivePosition = new Position(SHEET_COL_ARNO, i);
                                this.shtMeisai.Focus();
                                return false;
                            }
                            else
                            {
                                this.shtMeisai[SHEET_COL_ARNO, i].Text = this.lblAR.Text + this.txtARNo.Text;
                            }
                        }
                    }
                }
                // ↑ 2018/11/14 T.Nakata 複数AR対応

                // 品名半角チェック
                for (int i = 0; i < this.shtMeisai.Rows.Count - 1; i++)
                {
                    if (this.shtMeisai[SHEET_COL_HINME, i].Value != null)
                    {
                        string check = this.shtMeisai[SHEET_COL_HINME, i].Text.Trim();
                        if (!System.Text.RegularExpressions.Regex.IsMatch(Convert.ToString(check), @"^[ -~]+$"))
                        {
                            if (this.rdoInsert.Checked || this.rdoExcel.Checked || this.rdoUpdate.Checked)
                            {
                                // {0}行目の品名を半角英数字記号のみ入力してください。
                                this.ShowMessage("S0100020055", (i + 1).ToString());
                                this.shtMeisai.ActivePosition = new Position(SHEET_COL_HINME, i);
                                this.shtMeisai.Focus();
                                return false;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                ret = false;
            }
            return ret;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update>K.Tsutsumi 2012/04/23</update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                // ----- 検索用入力チェック -----
                // 2012/04/23 K.Tsutsumi Change 履歴ボタン追加
                //// 納入先チェック
                //if (string.IsNullOrEmpty(this.txtNonyusakiName.Text))
                //{
                //    // 納入先を入力して下さい。
                //    this.ShowMessage("A9999999016");
                //    this.txtNonyusakiName.Focus();
                //    return false;
                //}
                //// 便入力チェック
                //if (this.txtShip.Enabled && string.IsNullOrEmpty(this.txtShip.Text))
                //{
                //    // 便を入力して下さい。
                //    this.ShowMessage("A9999999017");
                //    this.txtShip.Focus();
                //    return false;
                //}
                //// AR No.チェック
                //if (this.txtARNo.Enabled && string.IsNullOrEmpty(this.txtARNo.Text))
                //{
                //    // AR No.を入力してください。
                //    this.ShowMessage("A9999999018");
                //    this.txtARNo.Focus();
                //    return false;
                //}
                if (this.CheckInputSerch_Header() == false)
                {
                    return false;
                }
                // ↑
                ret = this.CheckNonyusakiAndAR(true);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                ret = false;
            }
            return ret;
        }

        /// --------------------------------------------------
        /// <summary>
        /// ヘッダ入力チェック
        /// </summary>
        /// <returns>true:OK false:NG</returns>
        /// <create>K.Tsutsumi 2012/04/24</create>
        /// <update>H.Tajimi 2015/11/30 納入先(User)選択を早くする</update>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckInputSerch_Header()
        {
            bool ret = false;
            try
            {
                // 納入先CDチェック
                if (string.IsNullOrEmpty(this.txtNonyusakiCD.Text))
                {
                    // 納入先一覧から納入先を選択して下さい。
                    this.ShowMessage("S0100020041");
                    // 2015/11/30 H.Tajimi 一覧ボタン非表示
                    if (this.txtNonyusakiName.Enabled)
                    {
                        this.txtNonyusakiName.Focus();
                    }
                    // ↑
                    return false;
                }
                // ↑

                // AR No.チェック
                if (this.txtARNo.Enabled && string.IsNullOrEmpty(this.txtARNo.Text))
                {
                    // AR No.を入力してください。
                    this.ShowMessage("A9999999018");
                    this.txtARNo.Focus();
                    return false;
                }

                ret = true;
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
        /// 検索処理制御部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>Y.Higuchi 2010/07/06</create>
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
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update>K.Tsutsumi 2012/04/25</update>
        /// <update>H.Tajimi 2019/08/27 ファイル管理方式変更</update>
        /// <update>2023/12/06 R.Kubota TAG発行・照会画面と同じのTAG状態集計を追加</update>
        /// <update>2024/08/06 J.Jeong 出荷区分がARの場合のARNo追加</update>
        /// <update>J.Chen 2024/10/02 「引渡/集荷済」のTAG状態集計を追加</update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                if (this.rdoUpdate.Checked || this.rdoDelete.Checked || this.rdoView.Checked)
                {
                    CondS01 cond = this.GetCondition();
                    ConnS01 conn = new ConnS01();
                    // 変更・削除・照会
                    DataSet ds = conn.GetShukkaMeisai(cond);
                    if (!ComFunc.IsExistsData(ds, Def_T_SHUKKA_MEISAI.Name))
                    {
                        // 該当の明細は存在しません。
                        this.ShowMessage("A9999999022");
                        return false;
                    }
                    try
                    {
                        this.shtMeisai.Redraw = false;
                        this.shtMeisai.DataSource = ds.Tables[Def_T_SHUKKA_MEISAI.Name];
                        // 2012/04/25 K.Tsutsumi Delete TagNo列は、常時Disable状態（よって、不要）
                        //if (rdoUpdate.Checked)
                        //{
                        //    for (int i = 0; i < this.shtMeisai.MaxRows; i++)
                        //    {
                        //        if (!string.IsNullOrEmpty(this.shtMeisai[SHEET_COL_TAGNO, i].Text))
                        //        {
                        //            this.shtMeisai[SHEET_COL_TAGNO, i].Enabled = false;
                        //        }
                        //    }
                        //}
                        // ↑
                        for (int i = 0; i < this.shtMeisai.MaxRows; i++)
                        {
                            this.shtMeisai[SHEET_COL_ZUMEN_KEISHIKI, i].Tag = this.shtMeisai[SHEET_COL_ZUMEN_KEISHIKI, i].Text;
                        }

                        // TAG状態集計
                        DataSet ds2;
                        CondK01 condK01 = new CondK01(this.UserInfo);
                        WsConnection.ConnK01 connK01 = new ConnK01();
                        string errMsgID;
                        string kanriNo;

                        this.txtHikiwatashiShuka.Text = "0/0";
                        this.txtHikiwatashi.Text = "0/0";
                        this.txtShuka.Text = "0/0";
                        this.txtBoxKonpo.Text = "0/0";
                        this.txtPalletKonpo.Text = "0/0";
                        // ↑

                        condK01.ShukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
                        condK01.NounyusakiCD = this.txtNonyusakiCD.Text;
                        condK01.DisplaySelect = this.cboDispSelect.SelectedValue.ToString();
                        condK01.ARNo = cond.ARNo;

                        bool ret = connK01.GetShukaData(condK01, out ds2, out errMsgID, out kanriNo);

                        if (ComFunc.IsExistsData(ds2, ComDefine.DTTBL_PROGRESS) == true)
                        {
                            DataTable dt = ds2.Tables[ComDefine.DTTBL_PROGRESS];

                            int countShuka = ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_SHUKA);
                            int countHikiwatashiNoShuka = ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_HIKIWATASHI_NO_SHUKA);
                            int totalCountShukaHikiwatashi = countShuka + countHikiwatashiNoShuka;

                            this.txtHikiwatashiShuka.Text = totalCountShukaHikiwatashi.ToString("###,##0") + "/" + ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_ALL).ToString("###,##0");
                            this.txtHikiwatashi.Text = ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_HIKIWATASHI).ToString("###,##0") + "/" + ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_ALL).ToString("###,##0");
                            this.txtShuka.Text = ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_SHUKA).ToString("###,##0") + "/" + ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_ALL).ToString("###,##0");
                            this.txtBoxKonpo.Text = ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_BOXKONPO).ToString("###,##0") + "/" + ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_ALL).ToString("###,##0");
                            this.txtPalletKonpo.Text = ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_PALLETKONPO).ToString("###,##0") + "/" + ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_ALL).ToString("###,##0");
                        }
                    }
                    finally
                    {
                        this.shtMeisai.Redraw = true;
                    }
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

        #region 編集内容実行

        #region 制御メソッド

        /// --------------------------------------------------
        /// <summary>
        /// 編集内容実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update>T.Nakata 2018/11/16 TAG連携対応</update>
        /// <update>H.Tajimi 2019/08/16 TAG連携メール通知対応</update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            // 2012/04/24 K.Tsutsumi Add 保存後は出荷区分をクリアせず、現状維持
            this._isContinueShukkaFlag = true;
            // ↑
            bool ret = base.RunEdit();
            try
            {
                // 2018/11/16 T.Nakata TAG連携対応
                if (ret && this.rdoInsert.Checked && this._isTagRenkei)
                {
                    // TAG連携時正常終了した場合は画面を閉じる
                    this.IsCloseQuestion = false;
                    this.TagRenkeiResult = DialogResult.OK;
                    this.Close();
                }
                // ↑
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
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update>H.Tajimi 2019/08/16 TAG連携メール通知対応</update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                // 登録処理
                if (!this.CheckNonyusakiAndAR(false)) return false;

                CondS01 cond = this.GetCondition();
                ConnS01 conn = new ConnS01();
                DataSet ds = new DataSet();
                DataTable dt = this.shtMeisai.DataSource as DataTable;
                if (dt == null) return false;
                ds.Tables.Add(dt.Copy());

                string errMsgID;
                string[] args;
                if (!conn.InsShukkaKeikakuMeisai(cond, ds, out errMsgID, out args))
                {
                    if (!string.IsNullOrEmpty(errMsgID))
                    {
                        if (errMsgID == "S0100020002" && args != null)
                        {
                            this.shtMeisai.ActivePosition = new Position(SHEET_COL_TAGNO, UtilConvert.ToInt32(args[0]) - 1);
                            this.shtMeisai.Focus();
                        }
                        if (ComFunc.IsVersionError(errMsgID))
                        {
                            this.RunSearch();
                        }
                        if (errMsgID == "A9999999020")
                        {
                            // 「AR No.が存在しません。」のメッセージを書き換える。
                            // 他端末でAR情報が削除されまています。
                            errMsgID = "S0100020035";
                        }
                        this.ShowMessage(errMsgID, args);
                    }
                    return false;
                }
                else
                {
                    if (this._isTagRenkei)
                    {
                        this.AssociatedData = dt.Copy();
                    }
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

        #region 修正処理

        /// --------------------------------------------------
        /// <summary>
        /// 修正処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update>H.Tajimi 2015/11/18 Free1,Free2の列内同一チェック無効</update>
        /// <update>K.Tsutsumi 2018/02/04 複数AR対応による不具合修正</update>
        /// <update>J.Chen 2023/02/14 木枠梱包済みデータは修正不可に変更</update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                // 修正処理
                DataTable dt = (this.shtMeisai.DataSource as DataTable).Copy();
                // 新規データ抽出
                DataTable dtInsert = dt.GetChanges(DataRowState.Added);
                if (dtInsert == null)
                {
                    dtInsert = dt.Clone();
                }
                dtInsert.TableName = ComDefine.DTTBL_INSERT;
                // DB側のInsert処理は、dtInsertのARNo.を出荷明細テーブルに書き込むようになっているので
                // 編集時には、txtARNoの値をdtInsertに設定する必要がある。
                var shukkaFlag = SHUKKA_FLAG.NORMAL_VALUE1;
                if (this.cboShukkaFlag.SelectedValue != null)
                {
                    shukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
                }
                if (shukkaFlag == SHUKKA_FLAG.AR_VALUE1)
                {
                    foreach (DataRow dr in dtInsert.Rows)
                    {
                        dr.SetField(Def_T_SHUKKA_MEISAI.AR_NO, this.lblAR.Text + txtARNo.Text);
                    }
                }

                // 更新データ抽出
                DataTable dtUpdate = dt.GetChanges(DataRowState.Modified);
                if (dtUpdate == null)
                {
                    dtUpdate = dt.Clone();
                }
                dtUpdate.TableName = ComDefine.DTTBL_UPDATE;

                foreach (DataRow dr in dtUpdate.Rows)
                {
                    if (!string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.KOJI_NO)))
                    {
                        // 木枠梱包済のデータが含まれているため、更新できませんでした。
                        this.ShowMessage("S0100020054");
                        return false;
                    }
                }

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
                DataSet ds = new DataSet();
                ds.Tables.Add(dtInsert);
                ds.Tables.Add(dtUpdate);
                ds.Tables.Add(dtDelete);

                CondS01 cond = this.GetCondition();
                ConnS01 conn = new ConnS01();

                string errMsgID;
                string[] args;
                if (!conn.UpdShukkaKeikakuMeisai(cond, ds, out errMsgID, out args))
                {
                    if (!string.IsNullOrEmpty(errMsgID))
                    {
                        if (errMsgID == "S0100020002" && args != null)
                        {
                            string tagNo = string.Empty;
                            if (1 < args.Length)
                            {
                                tagNo = args[1];
                            }
                            for (int i = 0; i < this.shtMeisai.MaxRows; i++)
                            {
                                if (this.shtMeisai[SHEET_COL_TAGNO, i].Text == tagNo)
                                {
                                    args[0] = (i + 1).ToString();
                                }
                            }
                            this.shtMeisai.ActivePosition = new Position(SHEET_COL_TAGNO, UtilConvert.ToInt32(args[0]) - 1);
                            this.shtMeisai.Focus();
                        }
                        // ↓ 2018/11/19 T.Nakata 複数AR対応
                        else if (errMsgID == "S0100020047" && args != null)
                        {
                            this.shtMeisai.Focus();
                        }
                        else if (errMsgID == "S0100020048" && args != null)
                        {
                            this.shtMeisai.ActivePosition = new Position(SHEET_COL_NUM, UtilConvert.ToInt32(args[0]));
                            this.shtMeisai.Focus();
                        }
                        // ↑
                        if (ComFunc.IsVersionError(errMsgID))
                        {
                            this._isConfirmNoAR = true;
                            this.RunSearch();
                        }
                        this.ShowMessage(errMsgID, args);
                    }
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

        #region 削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditDelete()
        {
            try
            {
                // 削除処理
                DataTable dt = (this.shtMeisai.DataSource as DataTable).Copy();
                dt.TableName = ComDefine.DTTBL_DELETE;
                // 削除データ
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                CondS01 cond = this.GetCondition();
                ConnS01 conn = new ConnS01();

                string errMsgID;
                string[] args;
                if (!conn.DelShukkaKeikakuMeisai(cond, ds, out errMsgID, out args))
                {
                    if (!string.IsNullOrEmpty(errMsgID))
                    {
                        if (errMsgID == "S0100020002" && args != null)
                        {
                            this.shtMeisai.ActivePosition = new Position(SHEET_COL_TAGNO, UtilConvert.ToInt32(args[0]) - 1);
                            this.shtMeisai.Focus();
                        }
                        if (ComFunc.IsVersionError(errMsgID))
                        {
                            this.RunSearch();
                        }
                        this.ShowMessage(errMsgID, args);
                    }
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

        #endregion

        #region イベント

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F1ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update>T.SASAYAMA 2023/06/30</update>
        /// <update>J.Chen 2023/11/09 TAG登録ロックデータ取得条件修正</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            try
            {
                ConnP02 conn = new ConnP02();
                CondNonyusaki cond = new CondNonyusaki(this.UserInfo);
                cond.Ship = this.txtShip.Text;
                cond.NonyusakiName = this.txtNonyusakiName.Text;
                cond.ShukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
                DataSet ds = conn.GetNonyusakiIchiran(cond);
                var lockFlag = ComFunc.GetFld(ds, Def_M_NONYUSAKI.Name, 0, Def_M_NONYUSAKI.LOCK_FLAG);
                if (lockFlag == "1" && this.rdoView.Checked == false)
                {
                    this.ShowMessage("S0100020056");
                    return;
                }
                        if (this.shtMeisai.EditState) return;
                        this.ClearMessage();
                        base.fbrFunction_F01Button_Click(sender, e);
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
        /// <create>T.Nakata 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F02Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F02Button_Click(sender, e);
            try
            {
                // 行がないか行追加時の最終行(行番号が*)の場合は処理を抜ける。
                if (this.shtMeisai.Rows.Count == 0 || this.shtMeisai.Rows.Count == this.shtMeisai.ActivePosition.Row)
                {
                    // 明細が入力されていません。
                    this.ShowMessage("A9999999028");
                    this.shtMeisai.Focus();
                    return;
                }

                //DataTable dt = GetTehaiMeisaiNewInput();
                int row = shtMeisai.ActivePosition.Row;

                string Num = shtMeisai[SHEET_COL_NUM, row].Text;

                // 手配数が1以下の場合、何もしない(これ以上分割できないため)
                if (DSWUtil.UtilConvert.ToDecimal(Num) <= 1)
                {
                    // 明細が入力されていません。
                    this.ShowMessage("A9999999028");
                    this.shtMeisai.Focus();
                    return;
                }

                //// 該当行がTAG連携済みの場合、何もしない。
                //if (shtMeisai[SHEET_COL_RENKEINO, row].Text == "1")
                //{
                //     // 明細が入力されていません。
                //    this.ShowMessage("A9999999028");    // TAG連携済みです。
                //    this.shtMeisai.Focus();
                //    return;
                //}

                using (ShukkaKeikakuBunkatsu frm = new ShukkaKeikakuBunkatsu(this.UserInfo, Num))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        // 対象行をコピーする処理(一括Copyと同じ処理)
                        if (row < 0) return;
                        this.shtMeisai.SelectionType = SelectionType.Range;
                        this.shtMeisai.CellRange = new Range(SHEET_COL_COPY_START, row, SHEET_COL_COPY_END, row);
                        this.shtMeisai.UIReSelection(this.shtMeisai.CellRange);
                        this.shtMeisai.UICopy();

                        // コピーーしたレコードを新しい行に貼り付け
                        this.shtMeisai.InsertRow(row + 1, false);
                        this.shtMeisai.SelectionType = SelectionType.Range;
                        this.shtMeisai.CellRange = new Range(SHEET_COL_COPY_START, row + 1, SHEET_COL_COPY_END, row + 1);
                        this.shtMeisai.UIReSelection(this.shtMeisai.CellRange);
                        this.shtMeisai.UIPaste();

                        // 分割元の数量を対象行へ
                        this.shtMeisai[SHEET_COL_NUM, row].Text = frm.NumSource;

                        // 分割先の数量を新しい行へ
                        this.shtMeisai[SHEET_COL_NUM, row + 1].Text = frm.NumDestination;
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
        /// F3ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update>T.SASAYAMA 2023/07/14 引渡ファイル対応</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F03Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F03Button_Click(sender, e);
            try
            {
                // 2011/03/07 K.Tsutsumi Change 複数行対応
                //// 行がないか行追加時の最終行の場合は処理を抜ける。
                //if (this.shtMeisai.Rows.Count < 1 ||
                //            (this.shtMeisai.AllowUserToAddRows &&
                //            this.shtMeisai.Rows.Count - 1 == this.shtMeisai.ActivePosition.Row))
                //{
                //    return;
                //}

                //// 状態区分チェック
                //int jyotaiFlag = UtilConvert.ToInt32(this.shtMeisai[SHEET_COL_JYOTAI_FLAG, this.shtMeisai.ActivePosition.Row].Value, 0);
                //if (UtilConvert.ToInt32(JYOTAI_FLAG.BOXZUMI_VALUE1) <= jyotaiFlag)
                //{
                //    // 削除できない状態です。\r\n集荷済の状態まで戻した後に削除してください。
                //    this.ShowMessage("S0100020023");
                //    return;
                //}
                //// TagNo.[{0}]を削除してもよろしいですか？
                //string tagNo = this.shtMeisai[SHEET_COL_TAGNO, this.shtMeisai.ActivePosition.Row].Text;
                //if (this.ShowMessage("S0100020001", tagNo) == DialogResult.OK)
                //{
                //    this.shtMeisai.Redraw = false;
                //    this.shtMeisai.RemoveRow(this.shtMeisai.ActivePosition.Row, false);
                //    this.shtMeisai.Redraw = true;
                //}

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

                    // Range内行ループ
                    for (int row = range.TopRow; row <= range.BottomRow; row++)
                    {
                        // 状態区分チェック
                        int jyotaiFlag = UtilConvert.ToInt32(this.shtMeisai[SHEET_COL_JYOTAI_FLAG, row].Value, 0);
                        if (UtilConvert.ToInt32(JYOTAI_FLAG.BOXZUMI_VALUE1) <= jyotaiFlag && UtilConvert.ToInt32(DISP_JYOTAI_FLAG.HIKIWATASHIZUMI_VALUE1) != jyotaiFlag)
                        {
                            // 選択行には削除できない状態のものが含まれます。\r\n集荷済の状態まで戻した後に削除して下さい。
                            this.ShowMessage("S0100020037");
                            return;
                        }
                        // 2012/05/07 K.Tsutsumi Add 履歴
                        else if (this.shtMeisai[SHEET_COL_TAGNO, row].Value != null
                            && string.IsNullOrEmpty(this.shtMeisai[SHEET_COL_TAGNO, row].Value.ToString().Trim()) == false)
                        {
                            // 削除フラグ ON
                            isDeleted = true;
                        }
                        // ↑
                    }
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
        /// F5ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F05Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F05Button_Click(sender, e);
            try
            {
                this.SheetCellCopy(this.shtMeisai);
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
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update>H.Tajimi 2015/12/08 System全般/Clearボタン押下時の範囲</update>
        /// <update>2023/12/06 R.Kubota TAG発行・照会画面と同じのTAG状態集計を追加</update>
        /// <update>J.Chen 2024/10/02 「引渡/集荷済」のTAG状態集計を追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                if (ShowMessage("A9999999053") != DialogResult.OK) return;
                // グリッドのクリア
                this.SheetClear();
                // フラグのクリア
                this._isAddDetailNonyusaki = false;
                this._isAddDetailAR = false;
                this._isAddDetailExists = false;
                this._isConfirmNoAR = false;
                this._isDeleteRecord = false;
                this._isContinueShukkaFlag = false;

                this.txtHikiwatashiShuka.Text = "0/0";
                this.txtHikiwatashi.Text = "0/0";
                this.txtShuka.Text = "0/0";
                this.txtBoxKonpo.Text = "0/0";
                this.txtPalletKonpo.Text = "0/0";

                if (this.EditMode == SystemBase.EditMode.View)
                {
                    // 照会モード時のみ、検索条件のロックと連動させていないため個別に状態を変更
                    this.ChangeEnabledForViewMode(true);
                }
                // 検索条件のロック解除
                this.grpSearch.Enabled = true;
                // ファンクションボタンの切替
                this.ChangeFunctionButton(false);
                // フォーカス移動
                this.txtNonyusakiName.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F07ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2015/12/01</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                if (ShowMessage("A9999999001") != DialogResult.OK) return;
                // 2012/04/24 K.Tsutsumi Add 保存後は出荷区分はクリアせず、現状維持
                this._isContinueShukkaFlag = false;
                // ↑
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
        /// <create>Y.Higuchi 2010/10/27</create>
        /// <update>H.Tajimi 2015/11/20 SelectionTypeをシートに設定している値に戻すよう変更</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F08Button_Click(object sender, EventArgs e)
        {
            var selectionType = this.shtMeisai.SelectionType;
            base.fbrFunction_F08Button_Click(sender, e);
            try
            {
                int row = this.shtMeisai.ActivePosition.Row;
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
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F09ボタンクリック(行貼り付け)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/10/27</create>
        /// <update>H.Tajimi 2015/11/20 SelectionTypeをシートに設定している値に戻すよう変更</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F09Button_Click(object sender, EventArgs e)
        {
            var selectionType = this.shtMeisai.SelectionType;
            base.fbrFunction_F09Button_Click(sender, e);
            try
            {
                int row = this.shtMeisai.ActivePosition.Row;
                if (row < 0) return;
                this.shtMeisai.SelectionType = SelectionType.Range;
                this.shtMeisai.CellRange = new Range(SHEET_COL_COPY_START, row, SHEET_COL_COPY_END, row);
                this.shtMeisai.UIReSelection(this.shtMeisai.CellRange);
                this.shtMeisai.UIPaste();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.shtMeisai.SelectionType = selectionType;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F10ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update>K.Tsutsumi 2019/02/06 64 bit OSにてF10イベントが２回発生する不具合を対応</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F10Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F10Button_Click(sender, e);
            try
            {
                // 画面に貼り付けてある SaveFileDialog を使用するとボタン連打でStackOverFlowが発生する
                using (SaveFileDialog frm = new SaveFileDialog())
                {
                    frm.Title = Resources.ShukkaKeikakuMeisai_sdfExcel_Ttitle;
                    frm.Filter = Resources.ShukkaKeikakuMeisai_sdfExcel_Filter;
                    frm.FileName = ComDefine.EXCEL_FILE_SHUKKA_MEISAI;
                    if (0 < this.shtMeisai.MaxRows && frm.ShowDialog() != DialogResult.OK) return;
                    // Excel出力処理
                    DataTable dtExport = (this.shtMeisai.DataSource as DataTable).Copy();
                    ExportShukkaMeisai export = new ExportShukkaMeisai();
                    string msgID;
                    string[] args;
                    export.ExportExcel(frm.FileName, dtExport, out msgID, out args);
                    if (!string.IsNullOrEmpty(msgID))
                    {
                        this.ShowMessage(msgID, args);
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
        /// <create>K.Tsutsumi 2012/04/24</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F11Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F11Button_Click(sender, e);
            try
            {
                if (this.CheckInputSerch_Header() == false)
                {
                    return;
                }
                string shukkaFlag = SHUKKA_FLAG.NORMAL_VALUE1;
                string nonyusakiCd = this.txtNonyusakiCD.Text;
                string bukkenName = this.txtNonyusakiName.Text;
                string ship = this.txtShip.Text;
                string arNo = this.txtARNo.Text;
                if (this.cboShukkaFlag.SelectedValue != null)
                {
                    shukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
                }

                using (RirekiShokai frm = new RirekiShokai(this.UserInfo, GAMEN_FLAG.S0200020_VALUE1, shukkaFlag, nonyusakiCd, bukkenName, ship, arNo))
                {
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F12ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F12Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F12Button_Click(sender, e);
            try
            {
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 処理モード

        /// --------------------------------------------------
        /// <summary>
        /// 入力登録ラジオボタンのCheckedChangedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoInsert_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoInsert.Checked)
            {
                this.ClearMessage();
                this.ChangeMode();
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// Excel登録ラジオボタンのCheckedChangedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoExcel_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoExcel.Checked)
            {
                this.ClearMessage();
                this.ChangeMode();
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 変更ラジオボタンのCheckedChangedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoUpdate.Checked)
            {
                this.ClearMessage();
                this.ChangeMode();
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 削除ラジオボタンのCheckedChangedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoDelete_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoDelete.Checked)
            {
                this.ClearMessage();
                this.ChangeMode();
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 照会ラジオボタンのCheckedChangedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update>H.Tajimi 2015/11/30 納入先選択のUI改善</update>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoView_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoView.Checked)
            {
                this.ClearMessage();
                this.ChangeMode();
            }
            else
            {
                // グリッドクリア
                this.SheetClear();
                // 2015/11/30 H.Tajimi 納入先選択のUI改善
                this.ChangeEnabledForViewMode(true);
                // ↑
            }
        }

        #endregion

        #region 選択ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 選択ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                ofdExcel.FileName = this.txtExcel.Text;
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

        #region 開始ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 開始ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/08</create>
        /// <update>H.Tajimi 2015/11/30 納入先選択のUI改善</update>
        /// <update>H.Tajimi 2015/11/30 納入先選択のUI改善</update>
        /// <update>R.Kubota 2023/12/22 「開始」ボタン押下でTAG状態集計のクリア</update>
        /// <update>J.Chen 2024/10/02 「引渡/集荷済」のTAG状態集計を追加</update>
        /// --------------------------------------------------
        private void btnStart_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            // グリッドクリア
            this.SheetClear();

            this.txtHikiwatashiShuka.Text = "0/0";
            this.txtHikiwatashi.Text = "0/0";
            this.txtShuka.Text = "0/0";
            this.txtBoxKonpo.Text = "0/0";
            this.txtPalletKonpo.Text = "0/0";

            if (this.rdoInsert.Checked)
            {
                // ----- 入力登録 -----
                // 2015/11/30 H.Tajimi 納入先選択のUI改善
                if (!this.ShowNonyusakiIchiran(MODE_SEARCH))
                {
                    this.txtNonyusakiName.Focus();
                    return;
                }
                // ↑
                // グリッド
                this.shtMeisai.DataSource = this.GetSchemeShukkaMeisai();
                this.shtMeisai.AllowUserToAddRows = true;
                // 検索条件のロック
                this.grpSearch.Enabled = false;
            }
            else if (this.rdoExcel.Checked)
            {
                // ----- Excel登録 -----
                // 2015/11/30 H.Tajimi 納入先選択のUI改善
                if (!this.ShowNonyusakiIchiran(MODE_IMPORT_EXCEL))
                {
                    this.txtNonyusakiName.Focus();
                    return;
                }
                // ↑
                // グリッド
                this.shtMeisai.AllowUserToAddRows = false;
                // 検索条件のロック
                this.grpSearch.Enabled = false;
            }
            else if (this.rdoUpdate.Checked)
            {
                // ----- 変更 -----
                // 2015/11/30 H.Tajimi 納入先選択のUI改善
                if (!this.ShowNonyusakiIchiran(MODE_SEARCH))
                {
                    this.txtNonyusakiName.Focus();
                    return;
                }
                // ↑
                // グリッド
                this.shtMeisai.AllowUserToAddRows = true;
                // 検索条件のロック
                this.grpSearch.Enabled = false;
            }
            else if (this.rdoDelete.Checked)
            {
                // ----- 削除 -----
                // 2015/11/30 H.Tajimi 納入先選択のUI改善
                if (!this.ShowNonyusakiIchiran(MODE_SEARCH))
                {
                    this.txtNonyusakiName.Focus();
                    return;
                }
                // ↑
                // グリッド
                this.shtMeisai.AllowUserToAddRows = false;
                // 検索条件のロック
                this.grpSearch.Enabled = false;
            }
            else
            {
                // ----- 照会 -----
                // 2015/11/30 H.Tajimi 納入先選択のUI改善
                if (!this.ShowNonyusakiIchiran(MODE_SEARCH))
                {
                    this.txtNonyusakiName.Focus();
                    return;
                }
                // ↑
                // グリッド
                this.shtMeisai.AllowUserToAddRows = false;
                // 検索条件のロック
                this.grpSearch.Enabled = true;
                // 2015/11/30 H.Tajimi 納入先選択のUI改善
                this.ChangeEnabledForViewMode(false);
                // ↑
            }
            this.shtMeisai.Enabled = true;
            // ファンクションボタンの切替
            this.ChangeFunctionButton(true);
            // フォーカス
            this.shtMeisai.Focus();
        }

        #endregion

        #region 出荷区分

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分コンボボックスSelectedIndexChangedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboShukkaFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ChangeShukkaFlag();
        }

        #endregion

        #region 明細グリッド

        #region ClippingData

        /// --------------------------------------------------
        /// <summary>
        /// シートのクリップボード操作が行われたときに発生します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/22</create>
        /// <update>N.Ikari 2022/06/03 STEP14</update>
        /// --------------------------------------------------
        private void shtMeisai_ClippingData(object sender, ClippingDataEventArgs e)
        {
            try
            {
                switch (e.ClippingAction)
                {
                    case ClippingAction.Paste:
                        switch (this.shtMeisai.ActivePosition.Column)
                        {
                            case SHEET_COL_TAGNO:
                                // TagNo.列の貼り付けはキャンセルする。
                                e.Cancel = true;
                                break;
                            case SHEET_COL_RENKEINO:
                                // 連携No.列の貼り付けはキャンセルする。
                                e.Cancel = true;
                                break;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region LeaveEdit

        /// --------------------------------------------------
        /// <summary>
        /// セルが非編集モードに入る場合に発生します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/07</create>
        /// <update>K.Tsutsumi 2012/04/24</update>
        /// <update>H.Tajimi 2019/08/27 ファイル管理方式変更</update>
        /// <update>2022/06/01 STEP14</update>
        /// --------------------------------------------------
        private void shtMeisai_LeaveEdit(object sender, LeaveEditEventArgs e)
        {
            try
            {
                // 2012/04/24 K.Tsutsumi Delete TAGNo列は常時Disable
                //if (this.shtMeisai.ActivePosition.Column != SHEET_COL_TAGNO || string.IsNullOrEmpty(this.shtMeisai.ActiveCell.Text))
                //{
                //    return;
                //}
                //this.ClearMessage();
                //this.shtMeisai.ActiveCell.Text = this.GetFormatTagNo(this.shtMeisai.ActiveCell.Text);
                //this.shtMeisai.Refresh();
                //for (int i = 0; i < this.shtMeisai.Rows.Count; i++)
                //{
                //    if (i == this.shtMeisai.ActivePosition.Row) continue;
                //    if (this.shtMeisai[SHEET_COL_TAGNO, i].Text == this.shtMeisai.ActiveCell.Text)
                //    {
                //        // TagNo.[{0}]は既に存在します。
                //        this.ShowMessage("S0100020002", (this.shtMeisai.ActivePosition.Row + 1).ToString(), this.shtMeisai.ActiveCell.Text);
                //        e.Cancel = true;
                //        break;
                //    }
                //}
                // ↑
                if (this.rdoExcel.Checked || this.rdoDelete.Checked || this.rdoView.Checked)
                {
                    return;
                }
                int rowIndex = this.shtMeisai.ActivePosition.Row;
                switch (this.shtMeisai.ActivePosition.Column)
                {
                    case SHEET_COL_ZUMEN_KEISHIKI:
                        this.ClearMessage();
                        if (string.IsNullOrEmpty(this.shtMeisai.ActiveCell.Text))
                        {
                            this.shtMeisai[SHEET_COL_PICTURE, rowIndex].Text = ComDefine.NOT_EXISTS_PICTURE_VALUE;
                            this.shtMeisai[SHEET_COL_ZUMEN_KEISHIKI, rowIndex].Tag = this.shtMeisai.ActiveCell.Text;
                            return;
                        }

                        if ((this.shtMeisai[SHEET_COL_ZUMEN_KEISHIKI, rowIndex].Tag as string) != this.shtMeisai.ActiveCell.Text)
                        {
                            // 図番/形式が変更になった場合は写真有無を更新する
                            var cond = new CondS01(this.UserInfo);
                            var conn = new ConnS01();
                            cond.ZumenKeishiki = this.shtMeisai.ActiveCell.Text;
                            var dt = conn.GetExistsPicture(cond);
                            if (UtilData.ExistsData(dt))
                            {
                                this.shtMeisai[SHEET_COL_PICTURE, rowIndex].Text = ComFunc.GetFld(dt, 0, ComDefine.FLD_EXISTS_PICTURE);
                                this.shtMeisai[SHEET_COL_ZUMEN_KEISHIKI, rowIndex].Tag = this.shtMeisai.ActiveCell.Text;
                            }
                        }
                        break;
                    case SHEET_COL_RENKEINO:
                        this.ClearMessage();
                        if (!string.IsNullOrEmpty(this.shtMeisai.ActiveCell.Text))
                        {
                            // 連携Noの出荷先とTAG連携の出荷先を確認する
                            var cond = this.GetCondition();
                            var conn = new ConnS01();
                            if (cond.ShukkaFlag != SHUKKA_FLAG.NORMAL_VALUE1) { break; }
                            var dt = conn.GetShipToForTehaiNo(cond, this.shtMeisai.ActiveCell.Text);
                            if (!UtilData.ExistsData(dt))
                            {
                                this.ShowMessage("S0100020053");
                                this.shtMeisai.ActiveCell.Text = "";
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                e.Cancel = true;
            }
        }

        #endregion

        #endregion

        #region Enterキー操作のラジオボタン切替

        /// --------------------------------------------------
        /// <summary>
        /// Enterキー操作のラジオボタン切替
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsuji 2019/07/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoKeyAction_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                // メッセージクリア
                this.ClearMessage();

                // ショートカットキーを登録する
                this.shtMeisai.ShortCuts.Remove(Keys.Enter);

                // ラジオボタンはチェックするとTabStopも自動でtrueとなるため、その都度falseにする必要がある
                if (this.rdoRight.Checked)
                {
                    this.shtMeisai.ShortCuts.Add(Keys.Enter, new KeyAction[] { KeyAction.NextCellWithWrap });
                    this.rdoRight.TabStop = false;
                }
                else
                {
                    this.shtMeisai.ShortCuts.Add(Keys.Enter, new KeyAction[] { KeyAction.EndEdit, KeyAction.NextRow });
                    this.rdoDown.TabStop = false;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #endregion

        #region モード切り替え操作

        /// --------------------------------------------------
        /// <summary>
        /// モード変更時の切り替え処理
        /// </summary>
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update>T.Nakata 2018/11/14</update>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeMode()
        {
            try
            {
                if (this.rdoInsert.Checked)
                {
                    // ----- 入力登録 -----
                    // 画面の編集モード変更
                    this.EditMode = SystemBase.EditMode.Insert;
                    // 入力コントロールの切り替え
                    this.lblARNo.ChildPanel.Enabled = false;
                    this.txtARNo.Text = string.Empty;
                    this.txtExcel.Text = string.Empty;
                    this.txtExcel.Enabled = false;
                    this.cboDispSelect.SelectedValue = DISP_SELECT.DEFAULT_VALUE1;
                    this.cboDispSelect.Enabled = false;
                    // ボタンの切り替え
                    this.btnSelect.Enabled = false;
                    this.btnStart.Enabled = true;
                    // グリッド
                    this.ChangeSheetInputColumns(true);
                    this.ChangeSheetHeddinColumns(true);
                }
                else if (this.rdoExcel.Checked)
                {
                    // ----- Excel登録 -----
                    // 画面の編集モード変更
                    this.EditMode = SystemBase.EditMode.Insert;
                    // 入力コントロールの切り替え
                    this.lblARNo.ChildPanel.Enabled = false;
                    this.txtARNo.Text = string.Empty;
                    this.txtExcel.Text = string.Empty;
                    this.txtExcel.Enabled = true;
                    this.txtExcel.BackColor = SystemColors.Control;
                    this.cboDispSelect.SelectedValue = DISP_SELECT.DEFAULT_VALUE1;
                    this.cboDispSelect.Enabled = false;
                    // ボタンの切り替え
                    this.btnSelect.Enabled = true;
                    this.btnStart.Enabled = true;
                    // グリッド
                    this.ChangeSheetInputColumns(false);
                    this.ChangeSheetHeddinColumns(true);
                }
                else if (this.rdoUpdate.Checked)
                {
                    // ----- 変更 -----
                    // 画面の編集モード変更
                    this.EditMode = SystemBase.EditMode.Update;
                    // 入力コントロールの切り替え
                    this.lblARNo.ChildPanel.Enabled = true;
                    this.txtExcel.Text = string.Empty;
                    this.txtExcel.Enabled = false;
                    this.cboDispSelect.SelectedValue = DISP_SELECT.DEFAULT_VALUE1;
                    this.cboDispSelect.Enabled = false;
                    // ボタンの切り替え
                    this.btnSelect.Enabled = false;
                    this.btnStart.Enabled = true;
                    // グリッド
                    this.ChangeSheetInputColumns(true);
                    this.ChangeSheetHeddinColumns(false);
                }
                else if (this.rdoDelete.Checked)
                {
                    // ----- 削除 -----
                    // 画面の編集モード変更
                    this.EditMode = SystemBase.EditMode.Delete;
                    // 入力コントロールの切り替え
                    this.lblARNo.ChildPanel.Enabled = true;
                    this.txtExcel.Text = string.Empty;
                    this.txtExcel.Enabled = false;
                    this.cboDispSelect.SelectedValue = DISP_SELECT.DEFAULT_VALUE1;
                    this.cboDispSelect.Enabled = false;
                    // ボタンの切り替え
                    this.btnSelect.Enabled = false;
                    this.btnStart.Enabled = true;
                    // グリッド
                    this.ChangeSheetInputColumns(false);
                    this.ChangeSheetHeddinColumns(false);
                }
                else
                {
                    // ----- 照会 -----
                    // 画面の編集モード変更
                    this.EditMode = SystemBase.EditMode.View;
                    // 入力コントロールの切り替え
                    this.lblARNo.ChildPanel.Enabled = true;
                    this.txtExcel.Text = string.Empty;
                    this.txtExcel.Enabled = false;
                    if (0 < cboDispSelect.Items.Count)
                    {
                        this.cboDispSelect.SelectedValue = DISP_SELECT.DEFAULT_VALUE1;
                    }
                    else
                    {
                        this.cboDispSelect.SelectedIndex = -1;
                    }
                    this.cboDispSelect.Enabled = true;
                    // ボタンの切り替え
                    this.btnSelect.Enabled = false;
                    this.btnStart.Enabled = true;
                    // グリッド
                    this.ChangeSheetInputColumns(false);
                    this.ChangeSheetHeddinColumns(false);
                }
                // ファンクションボタンの切替
                this.ChangeFunctionButton(false);
                // AR No.グリッドの表示切替
                this.ChangeSheetHeddinColumnARNo();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// ファンクションバーのEnabled切替
        /// </summary>
        /// <param name="isEnabled">Enabled状態</param>
        /// <create>Y.Higuchi 2010/07/07</create>
        /// <update>H.Tajimi 2015/12/08 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeFunctionButton(bool isEnabled)
        {
            if (this.rdoInsert.Checked)
            {
                // ----- 入力登録 -----
                this.fbrFunction.F01Button.Enabled = isEnabled;
                this.fbrFunction.F02Button.Enabled = isEnabled;
                this.fbrFunction.F03Button.Enabled = isEnabled;
                this.fbrFunction.F05Button.Enabled = isEnabled;
                // 2015/12/08 H.Tajimi Clearボタン
                this.fbrFunction.F06Button.Enabled = isEnabled;
                // ↑
                this.fbrFunction.F08Button.Enabled = isEnabled;
                this.fbrFunction.F09Button.Enabled = isEnabled;
                this.fbrFunction.F10Button.Enabled = false;
                // 2012/04/24 K.Tsutsumi Add 履歴ボタン
                this.fbrFunction.F11Button.Enabled = isEnabled;
                // ↑
            }
            else if (this.rdoExcel.Checked)
            {
                // ----- Excel登録 -----
                this.fbrFunction.F01Button.Enabled = isEnabled;
                this.fbrFunction.F02Button.Enabled = false;
                this.fbrFunction.F03Button.Enabled = isEnabled;
                this.fbrFunction.F05Button.Enabled = false;
                // 2015/12/08 H.Tajimi Clearボタン
                this.fbrFunction.F06Button.Enabled = isEnabled;
                // ↑
                this.fbrFunction.F08Button.Enabled = false;
                this.fbrFunction.F09Button.Enabled = false;
                this.fbrFunction.F10Button.Enabled = false;
                // 2012/04/24 K.Tsutsumi Add 履歴ボタン
                this.fbrFunction.F11Button.Enabled = isEnabled;
                // ↑
            }
            else if (this.rdoUpdate.Checked)
            {
                // ----- 変更 -----
                this.fbrFunction.F01Button.Enabled = isEnabled;
                this.fbrFunction.F02Button.Enabled = isEnabled;
                this.fbrFunction.F03Button.Enabled = isEnabled;
                this.fbrFunction.F05Button.Enabled = isEnabled;
                // 2015/12/08 H.Tajimi Clearボタン
                this.fbrFunction.F06Button.Enabled = isEnabled;
                // ↑
                this.fbrFunction.F08Button.Enabled = isEnabled;
                this.fbrFunction.F09Button.Enabled = isEnabled;
                this.fbrFunction.F10Button.Enabled = false;
                // 2012/04/24 K.Tsutsumi Add 履歴ボタン
                this.fbrFunction.F11Button.Enabled = isEnabled;
                // ↑
            }
            else if (this.rdoDelete.Checked)
            {
                // ----- 削除 -----
                this.fbrFunction.F01Button.Enabled = isEnabled;
                this.fbrFunction.F02Button.Enabled = false;
                this.fbrFunction.F03Button.Enabled = false;
                this.fbrFunction.F05Button.Enabled = false;
                // 2015/12/08 H.Tajimi Clearボタン
                this.fbrFunction.F06Button.Enabled = isEnabled;
                // ↑
                this.fbrFunction.F08Button.Enabled = false;
                this.fbrFunction.F09Button.Enabled = false;
                this.fbrFunction.F10Button.Enabled = false;
                // 2012/04/24 K.Tsutsumi Add 履歴ボタン
                this.fbrFunction.F11Button.Enabled = isEnabled;
                // ↑
            }
            else
            {
                // ----- 照会 -----
                this.fbrFunction.F01Button.Enabled = false;
                this.fbrFunction.F02Button.Enabled = false;
                this.fbrFunction.F03Button.Enabled = false;
                this.fbrFunction.F05Button.Enabled = false;
                // 2015/12/08 H.Tajimi Clearボタン
                this.fbrFunction.F06Button.Enabled = isEnabled;
                // ↑
                this.fbrFunction.F08Button.Enabled = false;
                this.fbrFunction.F09Button.Enabled = false;
                this.fbrFunction.F10Button.Enabled = isEnabled;
                // 2012/04/24 K.Tsutsumi Add 履歴ボタン
                this.fbrFunction.F11Button.Enabled = isEnabled;
                // ↑
            }
        }

        #endregion

        #region グリッドの切り替え

        /// --------------------------------------------------
        /// <summary>
        /// グリッドの入力列のEnabled切替
        /// </summary>
        /// <param name="isEnabled">Enabledの値</param>
        /// <create>Y.Higuchi 2010/07/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeSheetInputColumns(bool isEnabled)
        {
            foreach (int index in this._inputColumns)
            {
                this.shtMeisai.Columns[index].Enabled = isEnabled;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// グリッドの表示/非表示の切替
        /// </summary>
        /// <param name="isHidden">非表示かどうか</param>
        /// <create>Y.Higuchi 2010/07/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeSheetHeddinColumns(bool isHidden)
        {
            this.shtMeisai.Redraw = false;
            foreach (int index in this._hiddenColumns)
            {
                this.shtMeisai.Columns[index].Hidden = isHidden;
            }
            this.shtMeisai.Redraw = true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// AR No.グリッドの表示/非表示の切替
        /// </summary>
        /// <param name="isHidden">非表示かどうか</param>
        /// <create>T.Nakata 2018/11/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeSheetHeddinColumnARNo()
        {
            this.shtMeisai.Redraw = false;

            if (this.rdoInsert.Checked || this.rdoExcel.Checked)
            {
                if (this.cboShukkaFlag.SelectedValue != null && this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1)
                {
                    this.shtMeisai.Columns[SHEET_COL_ARNO].Hidden = true;
                }
                else
                {
                    this.shtMeisai.Columns[SHEET_COL_ARNO].Hidden = false;
                }
            }
            else
            {
                this.shtMeisai.Columns[SHEET_COL_ARNO].Hidden = true;
            }

            this.shtMeisai.Redraw = true;
        }
        #endregion

        #region 出荷区分切替

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分の切替
        /// </summary>
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update>K.Tsutsumi 2012/04/24</update>
        /// <update>K.Tsutsumi 2012/05/14</update>
        /// <update>H.Tajimi 2015/11/30 納入先(User)選択を早くする</update>
        /// <update>J.Chen 2023/08/29 外部連携</update>
        /// --------------------------------------------------
        private void ChangeShukkaFlag()
        {

            // 管理No.
            this.txtNonyusakiCD.Text = string.Empty;

            if (!_isShukkaKeikaku)
            {
                // 納入先名
                this.txtNonyusakiName.Text = string.Empty;
                // 便
                this.txtShip.Text = string.Empty;
            }

            // AR No.
            this.txtARNo.Text = string.Empty;

            // 2018/11/15 T.Nakata 複数AR対応
            ChangeSheetHeddinColumnARNo();
            // ↑

            if (this.cboShukkaFlag.SelectedValue == null || this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1)
            {
                // ----- 本体 -----
                // AR No.
                this.txtARNo.Enabled = false;
                // 2015/11/30 H.Tajimi 便を有効にする
                this.txtShip.Enabled = true;
                // ↑
            }
            else
            {
                // ----- AR -----
                // AR No.
                this.txtARNo.Enabled = true;
                // 2015/11/30 H.Tajimi 便を無効にする
                this.txtShip.Enabled = false;
                // ↑
            }
        }

        #endregion

        #region 項コピー処理

        /// --------------------------------------------------
        /// <summary>
        /// 項コピー処理
        /// </summary>
        /// <param name="sheet"></param>
        /// <create>Y.Higuchi 2010/07/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetCellCopy(Sheet sheet)
        {
            if (sheet.ActivePosition.Row < 1 || !sheet.ActiveCell.Enabled || sheet.ActiveCell.Lock) return;
            int targetCol = sheet.ActivePosition.Column;
            int targetRow = sheet.ActivePosition.Row - 1;
            sheet.ActiveCell.Value = sheet[targetCol, targetRow].Value;
            sheet.EditState = true;
        }

        #endregion

        #region 入力登録、Excel登録用のデータテーブル

        /// --------------------------------------------------
        /// <summary>
        /// 入力登録、Excel登録用のデータテーブル
        /// </summary>
        /// <returns>データテーブル</returns>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update>T.Sakiori 2014/10/30 FREE列追加</update>
        /// <update>H.Tajimi 2015/11/20 備考列追加</update>
        /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
        /// <update>T.Nakata 2018/11/14 手配業務対応</update>
        /// <update>H.Iimuro 2022/10/04 図面/型式２の追加対応</update>
        /// <update>J.Chen 2024/11/08 通関確認状態追加</update>
        /// --------------------------------------------------
        private DataTable GetSchemeShukkaMeisai()
        {
            DataTable dt = new DataTable(Def_T_SHUKKA_MEISAI.Name);
            dt.Columns.Add(Def_M_NONYUSAKI.NONYUSAKI_CD, typeof(string));
            dt.Columns.Add(Def_M_NONYUSAKI.NONYUSAKI_NAME, typeof(string));
            dt.Columns.Add(Def_M_NONYUSAKI.SHIP, typeof(string));
            dt.Columns.Add(Def_M_NONYUSAKI.KANRI_FLAG, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.TAG_NO, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.AR_NO, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.SEIBAN, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.CODE, typeof(string));
            // 2012/04/25 K.Tsutsumi Change 型(Decimal→nvarchar)
            //dt.Columns.Add(Def_T_SHUKKA_MEISAI.ZUMEN_OIBAN, typeof(decimal));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.ZUMEN_OIBAN, typeof(string));
            // ↑
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.AREA, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.FLOOR, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.KISHU, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.ST_NO, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.HINMEI_JP, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.HINMEI, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI2, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.KUWARI_NO, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.NUM, typeof(decimal));
            // 2014/10/30 T.Sakiori FREE列追加
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.FREE1, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.FREE2, typeof(string));
            // ↑
            // 2015/11/20 H.Tajimi 備考列追加
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.BIKO, typeof(string));
            // ↑
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.CUSTOMS_STATUS, typeof(string));
            // ↑
            // 2015/12/09 H.Tajimi M_NO列追加
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.M_NO, typeof(string));
            // ↑
            // 2018/11/14 T.nakata GRWT/TEHAI_RENKEI_NO列追加
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.GRWT, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO, typeof(string));
            // ↑

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
        /// <create>Y.Higuchi 2010/07/07</create>
        /// <update>K.Tsutsumi 2012/04/25</update>
        /// <update>H.Tajimi 2019/08/27 ファイル管理方式変更</update>
        /// --------------------------------------------------
        private bool ExecuteImport()
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                // 明細データ(Excel)の入力チェック
                if (string.IsNullOrEmpty(this.txtExcel.Text))
                {
                    // 明細データ(Excel)のファイルが選択されていません。
                    this.ShowMessage("S0100020003");
                    return false;
                }
                // ファイル存在チェック
                if (!File.Exists(this.txtExcel.Text))
                {
                    // 明細データ(Excel)のファイルが存在しません。
                    this.ShowMessage("S0100020004");
                    return false;
                }
                // 2012/04/25 K.Tsutsumi Change 納入先は事前に登録されている（よって、必ず選択する必要がある）
                //// AR No.入力チェック
                //if ((this.cboShukkaFlag.SelectedValue != null && this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.AR_VALUE1) &&
                //        string.IsNullOrEmpty(this.txtARNo.Text))
                //{
                //    // AR No.を入力してください。
                //    this.ShowMessage("A9999999018");
                //    this.txtARNo.Focus();
                //    return false;
                //}
                if (this.CheckInputSerch_Header() == false)
                {
                    return false;
                }
                // ↑

                Cursor.Current = Cursors.WaitCursor;

                // 2012/04/25 K.Tsutsumi Delete 納入先は事前に登録されている（よって、必ず選択する必要がある）
                //// 納入先コードクリア処理
                //if (string.IsNullOrEmpty(this.txtNonyusakiName.Text))
                //{
                //    this.txtNonyusakiCD.Text = string.Empty;
                //}
                // ↑
                DataTable dt = this.GetSchemeShukkaMeisai();
                dt.Columns.Add(ComDefine.FLD_EXISTS_PICTURE, typeof(string));
                DataTable dtMessage = ComFunc.GetSchemeMultiMessage();
                string nonyusakiName = this.txtNonyusakiName.Text;
                string ship = this.txtShip.Text;
                bool ret = false;
                if (Path.GetExtension(this.txtExcel.Text) == ".xls")
                {
                    ret = this.GetExcelDataXls(this.txtExcel.Text, dt, dtMessage, ref nonyusakiName, ref ship);
                }
                else
                {
                    ret = this.GetExcelDataXlsx(this.txtExcel.Text, dt, dtMessage, ref nonyusakiName, ref ship);
                }
                if (0 < dt.Rows.Count)
                {
                    ret = true;
                    // 写真有無取得
                    var cond = new CondS01(this.UserInfo);
                    var conn = new ConnS01();
                    var dtSearch = dt.Copy();
                    dt = conn.GetExistsPictureFromDataTable(cond, dtSearch);
                }
                // 2012/04/25 K.Tsutsumi Delete 納入先は事前に登録されている（よって、必ず選択する）
                //this.txtNonyusakiName.Text = nonyusakiName;
                //if (this.cboShukkaFlag.SelectedValue == null || this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1)
                //{
                //    this.txtShip.Text = ship;
                //}
                // ↑

                this.shtMeisai.DataSource = dt;
                if (0 < dtMessage.Rows.Count)
                {
                    // 取込出来ないデータがありました。\r\nエラーがあった行は表示されていません。\r\n※エラーの一覧は右クリックでクリップボードにコピーできます。
                    this.ShowMultiMessage(dtMessage, "S0100020033");
                }

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
        /// 取込実行部
        /// </summary>
        /// <param name="filePath">Excelファイルパス</param>
        /// <param name="dt">取り込んだデータ</param>
        /// <param name="dtMessage">エラーメッセージテーブル</param>
        /// <param name="nonyusakiName">納入先</param>
        /// <param name="ship">出荷便</param>
        /// <returns>エラーがあったかどうか</returns>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update>T.Sakiori 2014/10/30 FREE列の追加 Floor列の桁数変更</update>
        /// <update>H.Tajimi 2015/11/18 Free1,Free2の列内同一チェック無効</update>
        /// <update>H.Tajimi 2015/11/20 備考列追加</update>
        /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
        /// <update>K.Tsutsumi 2019/01/17 ST-NO/M-No 16桁化</update>
        /// <update>K.Tsutsumi 2018/01/22 備考30->60</update>
        /// <update>T.Nukaga 2021/04/02 機種16→30 定数で設定</update>
        /// <update>J.Chen 2023/02/22 桁数変更対応</update>
        /// --------------------------------------------------
        private bool GetExcelDataXls(string filePath, DataTable dt, DataTable dtMessage, ref string nonyusakiName, ref string ship)
        {
            using (XlsCreator xls = new XlsCreator())
            {
                try
                {
                    xls.ReadBook(filePath);
                    int maxRow = xls.MaxData(xlPoint.ptMaxPoint).Height;
                    if (maxRow < 1)
                    {
                        // 明細データ(Excel)のファイルにデータがありません。
                        this.ShowMessage("S0100020006");
                        return false;
                    }
                    bool ret = true;
                    string itemName = string.Empty;
                    string field = string.Empty;
                    int col = 0;
                    int checkLen = 0;

                    for (int i = 1; i <= maxRow; i++)
                    {
                        DataRow dr = dt.NewRow();
                        bool isAddData = true;
                        // 2012/04/25 K.Tsutsumi Delete TagNo自動採番（よって、Excelからは読み取らない）
                        //// TagNo.のチェック
                        //itemName = "TagNo.";
                        //col = 0;
                        //checkLen = 5;
                        //field = Def_T_SHUKKA_MEISAI.TAG_NO;
                        //if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, itemName, ExcelInputAttrType.TagNo, dtMessage, true, true))
                        //{
                        //    isAddData = false;
                        //}
                        //string tagNo = ComFunc.GetFld(dr, field);
                        //if (isAddData && !string.IsNullOrEmpty(tagNo))
                        //{
                        //    tagNo = tagNo.ToUpper().PadLeft(5, '0');
                        //    foreach (DataRow drCheck in dt.Rows)
                        //    {
                        //        if (tagNo == ComFunc.GetFld(drCheck, field))
                        //        {
                        //            // {0}行目のTagNo.[{1}]がファイル内で重複しています。
                        //            ComFunc.AddMultiMessage(dtMessage, "S0100020009", (i + 1).ToString(), tagNo);
                        //            isAddData = false;
                        //            break;
                        //        }
                        //    }
                        //    if (isAddData)
                        //    {
                        //        dr[field] = tagNo;
                        //    }
                        //}
                        // ↑

                        // 製番のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_ProductNo;
                        col = 1;
                        // 2012/04/25 K.Tsutsumi Change Size(7 → 12)
                        //checkLen = 7;
                        checkLen = 12;
                        // ↑
                        field = Def_T_SHUKKA_MEISAI.SEIBAN;
                        // 2012/04/25 K.Tsutsumi Change 全角対応
                        //if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, itemName, ExcelInputAttrType.AlphaNum, dtMessage, true, false))
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        // ↑
                        {
                            isAddData = false;
                        }
                        // Codeのチェック
                        itemName = Resources.ShukkaKeikakuMeisai_Code;
                        col = 2;
                        checkLen = 3;
                        field = Def_T_SHUKKA_MEISAI.CODE;
                        // 2012/04/25 K.Tsutsumi Change 全角対応
                        //if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, itemName, ExcelInputAttrType.AlphaNum, dtMessage, true, false))
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        // ↑
                        {
                            isAddData = false;
                        }
                        // 図面追番のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_DrawingAdditionalNo;
                        col = 3;
                        // 2012/04/25 K.Tsutsumi Change Size(4 → 12)
                        //checkLen = 4;
                        checkLen = 12;
                        // ↑
                        field = Def_T_SHUKKA_MEISAI.ZUMEN_OIBAN;
                        // 2012/04/25 K.Tsutsumi Change 全角対応
                        //if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, itemName, ExcelInputAttrType.Numeric, dtMessage, false, false))
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        // ↑
                        {
                            isAddData = false;
                        }
                        // 納入先のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_DeliveryDestination;
                        col = 4;
                        checkLen = 60;
                        field = Def_M_NONYUSAKI.NONYUSAKI_NAME;
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, true))
                        {
                            isAddData = false;
                        }
                        // 出荷便のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_ShippingFlights;
                        col = 5;
                        // 2012/04/25 K.Tsutsumi Change Size(5 → 10)
                        //checkLen = 5;
                        checkLen = 10;
                        // ↑
                        field = Def_M_NONYUSAKI.SHIP;
                        if (this.cboShukkaFlag.SelectedValue != null && this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.AR_VALUE1)
                        {
                            // AR
                            // 2012/04/25 K.Tsutsumi Change 全角対応
                            //if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, itemName, ExcelInputAttrType.AlphaNum, dtMessage, true, false))
                            if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                            // ↑
                            {
                                isAddData = false;
                            }
                            if (!string.IsNullOrEmpty(ComFunc.GetFld(dr, field)))
                            {
                                // {0}行目に出荷便が入力されています。
                                ComFunc.AddMultiMessage(dtMessage, "S0100020032", (i + 1).ToString());
                            }
                        }
                        else
                        {
                            // 本体
                            // 2012/04/25 K.Tsutsumi Change 全角対応
                            //if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, itemName, ExcelInputAttrType.AlphaNum, dtMessage, true, true))
                            if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, true))
                            // ↑
                            {
                                isAddData = false;
                            }
                        }
                        // Areaのチェック
                        itemName = Resources.ShukkaKeikakuMeisai_Area;
                        col = 6;
                        // 2012/04/25 K.Tsutsumi Change Size(12 → 15)
                        //checkLen = 12;
                        checkLen = 20;
                        // ↑
                        field = Def_T_SHUKKA_MEISAI.AREA;
                        // 2012/04/25 K.Tsutsumi Change 全角対応
                        //if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, itemName, ExcelInputAttrType.AlphaNum, dtMessage, true, false))
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        // ↑
                        {
                            isAddData = false;
                        }
                        // Floorのチェック
                        itemName = Resources.ShukkaKeikakuMeisai_Floor;
                        col = 7;
                        // 2014/10/30 T.Sakiori Change Size(12 → 8)
                        //checkLen = 20;
                        checkLen = 20;
                        // ↑
                        field = Def_T_SHUKKA_MEISAI.FLOOR;
                        // 2012/04/25 K.Tsutsumi Change 全角対応
                        //if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, itemName, ExcelInputAttrType.AlphaNum, dtMessage, true, false))
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        // ↑
                        {
                            isAddData = false;
                        }
                        // 機種のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_Model;
                        col = 8;
                        checkLen = ComDefine.MAX_BYTE_LENGTH_KISHU;
                        field = Def_T_SHUKKA_MEISAI.KISHU;
                        // 2011/03/01 K.Tsutsumi Change 全角可能
                        //if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, itemName, ExcelInputAttrType.AlphaNum, dtMessage, true, false))
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        // ↑
                        {
                            isAddData = false;
                        }
                        // ST-No.のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_STNo;
                        col = 9;
                        // 2019/01/17 ST-NO/M-No 16桁化
                        //checkLen = 9;
                        checkLen = 16;
                        field = Def_T_SHUKKA_MEISAI.ST_NO;
                        // 2012/04/25 K.Tsutsumi Change 全角対応
                        //if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, itemName, ExcelInputAttrType.AlphaNum, dtMessage, true, false))
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        // ↑
                        {
                            isAddData = false;
                        }
                        // 品名(和文)のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_JpName;
                        col = 10;
                        checkLen = 100;
                        field = Def_T_SHUKKA_MEISAI.HINMEI_JP;
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        {
                            isAddData = false;
                        }
                        // 品名のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_Name;
                        col = 11;
                        checkLen = 100;
                        field = Def_T_SHUKKA_MEISAI.HINMEI;
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.AlphaNum, dtMessage, true, false))
                        {
                            isAddData = false;
                        }
                        // 図面/形式のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_DrawingNoFormat;
                        col = 12;
                        checkLen = 100;
                        field = Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI;
                        // 2012/04/25 K.Tsutsumi Change 全角対応
                        //if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, itemName, ExcelInputAttrType.AlphaNum, dtMessage, true, false))
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        // ↑
                        {
                            isAddData = false;
                        }
                        // 区割No.のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_DivisionNo;
                        col = 13;
                        checkLen = 8;
                        field = Def_T_SHUKKA_MEISAI.KUWARI_NO;
                        // 2012/04/25 K.Tsutsumi Change 全角対応
                        //if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, itemName, ExcelInputAttrType.AlphaNum, dtMessage, true, false))
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        // ↑
                        {
                            isAddData = false;
                        }
                        // 数量のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_Quantity;
                        col = 14;
                        checkLen = 5;
                        field = Def_T_SHUKKA_MEISAI.NUM;
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.Numeric, dtMessage, false, false))
                        {
                            isAddData = false;
                        }
                        // Free1のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_Free1;
                        col = 15;
                        checkLen = 8;
                        field = Def_T_SHUKKA_MEISAI.FREE1;
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        {
                            isAddData = false;
                        }
                        // Free2のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_Free2;
                        col = 16;
                        checkLen = 30;
                        field = Def_T_SHUKKA_MEISAI.FREE2;
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        {
                            isAddData = false;
                        }
                        // 2015/11/20 H.Tajimi 備考列追加
                        // 備考のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_Remarks;
                        col = 17;
                        //checkLen = 30;
                        checkLen = 60;
                        field = Def_T_SHUKKA_MEISAI.BIKO;
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        {
                            isAddData = false;
                        }
                        // ↑
                        // 2015/12/09 H.Tajimi M_NO列追加
                        // M-Noのチェック
                        itemName = Resources.ShukkaKeikakuMeisai_MNo;
                        col = 18;
                        // 2019/01/17 ST-NO/M-No 16桁化
                        //checkLen = 9;
                        checkLen = 40;
                        field = Def_T_SHUKKA_MEISAI.M_NO;
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        {
                            isAddData = false;
                        }
                        // ↑
                        // 2018/11/14 T.Nakata 重量/ARNo/連携No追加
                        //// 重量のチェック
                        //itemName = Resources.ShukkaKeikakuMeisai_Grwt;
                        //col = 19;
                        //checkLen = 5;
                        //int checkLenL = 2;
                        //field = Def_T_SHUKKA_MEISAI.GRWT;
                        //if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, checkLenL, itemName, ExcelInputAttrType.Float, dtMessage, false, false))
                        //{
                        //    isAddData = false;
                        //}
                        // AR Noのチェック
                        itemName = Resources.ShukkaKeikakuMeisai_ArNo;
                        col = 19;
                        checkLen = 6;
                        field = Def_T_SHUKKA_MEISAI.AR_NO;
                        if (this.cboShukkaFlag.SelectedValue != null && this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.AR_VALUE1)
                        {
                            if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.AlphaNum, dtMessage, true, true))
                            {
                                isAddData = false;
                            }
                        }
                        else
                        {
                            if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.AlphaNum, dtMessage, true, false))
                            {
                                isAddData = false;
                            }
                            if (!string.IsNullOrEmpty(ComFunc.GetFld(dr, field)))
                            {
                                // {0}行目にAR Noが入力されています。
                                ComFunc.AddMultiMessage(dtMessage, "S0100020044", (i + 1).ToString());
                                isAddData = false;
                            }
                        }
                        // 連携Noのチェック
                        itemName = Resources.ShukkaKeikakuMeisai_TehaiRenkeiNo;
                        col = 20;
                        checkLen = 8;
                        field = Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO;
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.AlphaNum, dtMessage, true, false))
                        {
                            isAddData = false;
                        }
                        // ↑

                        if (isAddData)
                        {
                            // 2012/04/25 K.Tsutsumi Delete 納入先は事前に登録されている（不要）
                            //if (string.IsNullOrEmpty(nonyusakiName) && string.IsNullOrEmpty(ship))
                            //{
                            //    nonyusakiName = ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_NAME);
                            //    ship = ComFunc.GetFld(dr, Def_M_NONYUSAKI.SHIP);
                            //}
                            // ↑
                            if (nonyusakiName == ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_NAME) &&
                                    ship == ComFunc.GetFld(dr, Def_M_NONYUSAKI.SHIP))
                            {
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                // {0}行目に異なる納入先、便のデータが存在します。
                                ComFunc.AddMultiMessage(dtMessage, "S0100020007", (i + 1).ToString());
                            }
                        }
                        else
                        {
                            ret = false;
                        }
                    }
                    // 表示データがない場合は失敗とする。
                    if (dt.Rows.Count == 0)
                    {
                        ret = false;
                    }
                    return ret;
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
        /// 取込実行部
        /// </summary>
        /// <param name="filePath">Excelファイルパス</param>
        /// <param name="dt">取り込んだデータ</param>
        /// <param name="dtMessage">エラーメッセージテーブル</param>
        /// <param name="nonyusakiName">納入先</param>
        /// <param name="ship">出荷便</param>
        /// <returns>エラーがあったかどうか</returns>
        /// <create>T.Sakiori 2015/06/09 Xlsx用に作成(dynamicがFramework4.0からだった。。。)</create>
        /// <update>H.Tajimi 2015/11/18 Free1,Free2の列内同一チェック無効</update>
        /// <update>H.Tajimi 2015/11/20 備考列追加</update>
        /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
        /// <update>T.Nukaga 2021/04/02 機種16→30、定数で定義</update>
        /// <update>R.Sumi 2022/03/02 図番形式の桁数30→40</update>
        /// <update>J.Chen 2023/02/22 桁数変更対応</update>
        /// --------------------------------------------------
        private bool GetExcelDataXlsx(string filePath, DataTable dt, DataTable dtMessage, ref string nonyusakiName, ref string ship)
        {
            using (var xls = new XlsxCreator())
            {
                try
                {
                    xls.ReadBook(filePath);
                    int maxRow = xls.MaxData(xlMaxEndPoint.xarMaxPoint).Height;
                    if (maxRow < 1)
                    {
                        // 明細データ(Excel)のファイルにデータがありません。
                        this.ShowMessage("S0100020006");
                        return false;
                    }
                    bool ret = true;
                    string itemName = string.Empty;
                    string field = string.Empty;
                    int col = 0;
                    int checkLen = 0;

                    for (int i = 1; i <= maxRow; i++)
                    {
                        DataRow dr = dt.NewRow();
                        bool isAddData = true;

                        // 製番のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_ProductNo;
                        col = 1;
                        checkLen = 12;
                        field = Def_T_SHUKKA_MEISAI.SEIBAN;
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        {
                            isAddData = false;
                        }
                        // Codeのチェック
                        itemName = Resources.ShukkaKeikakuMeisai_Code;
                        col = 2;
                        checkLen = 3;
                        field = Def_T_SHUKKA_MEISAI.CODE;
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        {
                            isAddData = false;
                        }
                        // 図面追番のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_DrawingAdditionalNo;
                        col = 3;
                        checkLen = 12;
                        field = Def_T_SHUKKA_MEISAI.ZUMEN_OIBAN;
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        {
                            isAddData = false;
                        }
                        // 納入先のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_DeliveryDestination;
                        col = 4;
                        checkLen = 60;
                        field = Def_M_NONYUSAKI.NONYUSAKI_NAME;
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, true))
                        {
                            isAddData = false;
                        }
                        // 出荷便のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_ShippingFlights;
                        col = 5;
                        checkLen = 10;
                        field = Def_M_NONYUSAKI.SHIP;
                        if (this.cboShukkaFlag.SelectedValue != null && this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.AR_VALUE1)
                        {
                            // AR
                            if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                            {
                                isAddData = false;
                            }
                            if (!string.IsNullOrEmpty(ComFunc.GetFld(dr, field)))
                            {
                                // {0}行目に出荷便が入力されています。
                                ComFunc.AddMultiMessage(dtMessage, "S0100020032", (i + 1).ToString());
                            }
                        }
                        else
                        {
                            // 本体
                            if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, true))
                            {
                                isAddData = false;
                            }
                        }
                        // Areaのチェック
                        itemName = Resources.ShukkaKeikakuMeisai_Area;
                        col = 6;
                        checkLen = 20;
                        field = Def_T_SHUKKA_MEISAI.AREA;
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        {
                            isAddData = false;
                        }
                        // Floorのチェック
                        itemName = Resources.ShukkaKeikakuMeisai_Floor;
                        col = 7;
                        checkLen = 20;
                        field = Def_T_SHUKKA_MEISAI.FLOOR;
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        {
                            isAddData = false;
                        }
                        // 機種のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_Model;
                        col = 8;
                        checkLen = ComDefine.MAX_BYTE_LENGTH_KISHU;
                        field = Def_T_SHUKKA_MEISAI.KISHU;
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        {
                            isAddData = false;
                        }
                        // ST-No.のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_STNo;
                        col = 9;
                        checkLen = 9;
                        field = Def_T_SHUKKA_MEISAI.ST_NO;
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        {
                            isAddData = false;
                        }
                        // 品名(和文)のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_JpName;
                        col = 10;
                        checkLen = 100;
                        field = Def_T_SHUKKA_MEISAI.HINMEI_JP;
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        {
                            isAddData = false;
                        }
                        // 品名のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_Name;
                        col = 11;
                        checkLen = 100;
                        field = Def_T_SHUKKA_MEISAI.HINMEI;
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.AlphaNum, dtMessage, true, false))
                        {
                            isAddData = false;
                        }
                        // 図面/形式のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_DrawingNoFormat;
                        col = 12;
                        checkLen = 100;
                        field = Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI;
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        {
                            isAddData = false;
                        }
                        // 区割No.のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_DivisionNo;
                        col = 13;
                        checkLen = 8;
                        field = Def_T_SHUKKA_MEISAI.KUWARI_NO;
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        {
                            isAddData = false;
                        }
                        // 数量のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_Quantity;
                        col = 14;
                        checkLen = 5;
                        field = Def_T_SHUKKA_MEISAI.NUM;
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.Numeric, dtMessage, false, false))
                        {
                            isAddData = false;
                        }
                        // Free1のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_Free1;
                        col = 15;
                        checkLen = 8;
                        field = Def_T_SHUKKA_MEISAI.FREE1;
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        {
                            isAddData = false;
                        }
                        // Free2のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_Free2;
                        col = 16;
                        checkLen = 30;
                        field = Def_T_SHUKKA_MEISAI.FREE2;
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        {
                            isAddData = false;
                        }
                        // 2015/11/20 H.Tajimi 備考列追加
                        // 備考のチェック
                        itemName = Resources.ShukkaKeikakuMeisai_Remarks;
                        col = 17;
                        checkLen = 30;
                        field = Def_T_SHUKKA_MEISAI.BIKO;
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        {
                            isAddData = false;
                        }
                        // ↑
                        // 2015/12/09 H.Tajimi M_NO列追加
                        // M_NOのチェック
                        itemName = Resources.ShukkaKeikakuMeisai_MNo;
                        col = 18;
                        checkLen = 40;
                        field = Def_T_SHUKKA_MEISAI.M_NO;
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.WideString, dtMessage, true, false))
                        {
                            isAddData = false;
                        }
                        // ↑
                        // 2018/11/14 T.Nakata 重量/ARNo/連携No追加
                        //// 重量のチェック
                        //itemName = Resources.ShukkaKeikakuMeisai_Grwt;
                        //col = 19;
                        //checkLen = 5;
                        //int checkLenL = 2;
                        //field = Def_T_SHUKKA_MEISAI.GRWT;
                        //if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, checkLenL, itemName, ExcelInputAttrType.Float, dtMessage, false, false))
                        //{
                        //    isAddData = false;
                        //}
                        // AR Noのチェック
                        itemName = Resources.ShukkaKeikakuMeisai_ArNo;
                        col = 19;
                        checkLen = 6;
                        field = Def_T_SHUKKA_MEISAI.AR_NO;
                        if (this.cboShukkaFlag.SelectedValue != null && this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.AR_VALUE1)
                        {
                            if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.AlphaNum, dtMessage, true, true))
                            {
                                isAddData = false;
                            }
                        }
                        else
                        {
                            if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.AlphaNum, dtMessage, true, false))
                            {
                                isAddData = false;
                            }
                            if (!string.IsNullOrEmpty(ComFunc.GetFld(dr, field)))
                            {
                                // {0}行目にAR Noが入力されています。
                                ComFunc.AddMultiMessage(dtMessage, "S0100020044", (i + 1).ToString());
                            }
                        }
                        // 連携Noのチェック
                        itemName = Resources.ShukkaKeikakuMeisai_TehaiRenkeiNo;
                        col = 20;
                        checkLen = 8;
                        field = Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO;
                        if (!this.CheckAndSetExcelData(xls.Pos(col, i).Str, i, dr, field, checkLen, 0, itemName, ExcelInputAttrType.AlphaNum, dtMessage, true, false))
                        {
                            isAddData = false;
                        }
                        // ↑
                        
                        if (isAddData)
                        {
                            if (nonyusakiName == ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_NAME) &&
                                    ship == ComFunc.GetFld(dr, Def_M_NONYUSAKI.SHIP))
                            {
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                // {0}行目に異なる納入先、便のデータが存在します。
                                ComFunc.AddMultiMessage(dtMessage, "S0100020007", (i + 1).ToString());
                            }
                        }
                        else
                        {
                            ret = false;
                        }
                    }
                    // 表示データがない場合は失敗とする。
                    if (dt.Rows.Count == 0)
                    {
                        ret = false;
                    }
                    return ret;
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
        /// <param name="checkLenU">上位最大バイトサイズ</param>
        /// <param name="checkLenL">下位最大バイトサイズ</param>
        /// <param name="itemName">取込対象項目名</param>
        /// <param name="attrType">文字属性</param>
        /// <param name="dtMessage">エラーテーブル</param>
        /// <param name="isString">文字列かどうか</param>
        /// <param name="isNecessary">必須かどうか</param>
        /// <returns>true:エラーなし/false:エラーあり</returns>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update>T.Nakata 2018/11/14 浮動小数点対応</update>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckAndSetExcelData(string target, int rowIndex, DataRow dr, string field, int checkLenU, int checkLenL, string itemName, ExcelInputAttrType attrType, DataTable dtMessage, bool isString, bool isNecessary)
        {
            object retVal = DBNull.Value;
            bool ret = true;
            if (!this.CheckByteLength(target, rowIndex, ref retVal, checkLenU, checkLenL, itemName, dtMessage, isString, attrType))
            {
                ret = false;
            }
            if (!CheckRegulation(target, rowIndex, itemName, attrType, dtMessage))
            {
                ret = false;
            }
            if (isNecessary)
            {
                // 必須チェック
                if (string.IsNullOrEmpty(target))
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
        /// <param name="attrType">文字属性</param>
        /// <returns>true:エラーなし/false:エラーあり</returns>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update>T.Nakata 2018/11/14 浮動小数点対応</update>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckByteLength(string target, int rowIndex, ref object value, int checkLenU, int checkLenL, string itemName, DataTable dtMessage, bool isString, ExcelInputAttrType attrType)
        {
            if (isString)
            {
                return this.CheckAndSetStringByteLength(target, rowIndex, ref value, checkLenU, itemName, dtMessage);
            }
            else
            {
                if (attrType == ExcelInputAttrType.Float)
                {
                    return this.CheckAndSetFloatLength(target, rowIndex, ref value, checkLenU, checkLenL, itemName, dtMessage);
                }
                else
                {
                    return this.CheckAndSetIntLength(target, rowIndex, ref value, checkLenU, itemName, dtMessage);
                }
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
        /// <returns>true:エラーなし/false:エラーあり</returns>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckAndSetStringByteLength(string target, int rowIndex, ref object value, int checkLen, string itemName, DataTable dtMessage)
        {
            if (checkLen < UtilString.GetByteCount(target))
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
        /// <returns>true:エラーなし/false:エラーあり</returns>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckAndSetIntLength(string target, int rowIndex, ref object value, int checkLen, string itemName, DataTable dtMessage)
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
            if (checkLen < UtilString.GetByteCount(checkStr))
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

        /// --------------------------------------------------
        /// <summary>
        /// 数値の最大バイトサイズチェック
        /// </summary>
        /// <param name="target">対象文字列</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="value">取り込んだ値</param>
        /// <param name="checkLenU">上位最大バイトサイズ</param>
        /// <param name="checkLenL">下位最大バイトサイズ</param>
        /// <param name="itemName">取込対象項目名</param>
        /// <param name="dtMessage">エラーテーブル</param>
        /// <returns>true:エラーなし/false:エラーあり</returns>
        /// <create>T.Nakata 2018/11/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckAndSetFloatLength(string target, int rowIndex, ref object value, int checkLenU, int checkLenL, string itemName, DataTable dtMessage)
        {
            bool isNullOrEmpty = false;
            if (string.IsNullOrEmpty(target))
            {
                isNullOrEmpty = true;
                target = "0";
            }
            string[] number = target.Split('.');
            string decimalsStr = string.Empty;
            if (number.Length > 2)
            {
                // {0}行目の{1}が数値以外が入力されています。
                ComFunc.AddMultiMessage(dtMessage, "S0100020045", (rowIndex + 1).ToString(), itemName);
                return false;
            }
            float result;
            if (!float.TryParse(target, System.Globalization.NumberStyles.Float, null, out result))
            {
                // {0}行目の{1}が数値に変換できませんでした。
                ComFunc.AddMultiMessage(dtMessage, "S0100020031", (rowIndex + 1).ToString(), itemName);
                return false;
            }
            if (checkLenU < UtilString.GetByteCount(number[0]))
            {
                // {0}行目の{1}が登録できる文字数を超えています。
                ComFunc.AddMultiMessage(dtMessage, "S0100020010", (rowIndex + 1).ToString(), itemName);
                return false;
            }
            if (number.Length == 2)
            {
                if (checkLenL < UtilString.GetByteCount(number[1]))
                {
                    // {0}行目の{1}が登録できる文字数を超えています。
                    ComFunc.AddMultiMessage(dtMessage, "S0100020010", (rowIndex + 1).ToString(), itemName);
                    return false;
                }
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
        /// <create>Y.Higuchi 2010/07/16</create>
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
                case ExcelInputAttrType.TagNo:
                    targetType += ComRegulation.REGULATION_NARROW_ALPHA_UP;
                    targetType += ComRegulation.REGULATION_NARROW_NUMERIC_ONLY;
                    isUse = true;
                    msgExtend = Resources.ShukkaKeikakuMeisai_UpperAndNumber;
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
                    msgExtend = Resources.ShukkaKeikakuMeisai_PlatformDependentChar;
                    break;
                case ExcelInputAttrType.Numeric:
                case ExcelInputAttrType.Float:
                    targetType += ComRegulation.REGULATION_NARROW_NUMERIC_SIGN;
                    isUse = true;
                    msgExtend = Resources.ShukkaKeikakuMeisai_Numeric;
                    break;
                default:
                    break;
            }
            if (!regulation.CheckRegularString(target, targetType, isUse, false))
            {
                // {0}行目の{1}が登録できない文字を含んでいます。{2}
                ComFunc.AddMultiMessage(dtMessage, "S0100020029", (rowIndex + 1).ToString(), itemName, msgExtend);
                return false;
            }
            return true;
        }

        #endregion

        #endregion

        #region コンディション取得

        /// --------------------------------------------------
        /// <summary>
        /// コンディションの取得
        /// </summary>
        /// <returns>コンディション</returns>
        /// <create>Y.Higuchi 2010/07/09</create>
        /// <update>K.Tsutsumi 2012/05/07</update>
        /// --------------------------------------------------
        private CondS01 GetCondition()
        {
            CondS01 cond = new CondS01(this.UserInfo);
            cond.ModeInsert = this.rdoInsert.Checked;
            cond.ModeExcel = this.rdoExcel.Checked;
            cond.ModeUpdate = this.rdoUpdate.Checked;
            cond.ModeDelete = this.rdoDelete.Checked;
            cond.ModeView = this.rdoView.Checked;
            if (this.cboShukkaFlag.SelectedValue != null)
            {
                cond.ShukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
            }
            else
            {
                cond.ShukkaFlag = SHUKKA_FLAG.NORMAL_VALUE1;
            }
            // 2012/04/24 K.Tsutsumi Delete キーでは無くなった
            //cond.NonyusakiName = this.txtNonyusakiName.Text;
            // ↑
            if (!string.IsNullOrEmpty(this.txtNonyusakiCD.Text))
            {
                cond.NonyusakiCD = this.txtNonyusakiCD.Text;
            }
            if (cond.ShukkaFlag == SHUKKA_FLAG.NORMAL_VALUE1)
            {
                cond.Ship = this.txtShip.Text;
            }
            else
            {
                cond.ARNo = string.Empty;
                if (!this.rdoInsert.Checked && !this.rdoExcel.Checked)
                {
                    cond.ARNo = this.lblAR.Text + this.txtARNo.Text;
                }
                else
                {
                    DataTable dt = this.shtMeisai.DataSource as DataTable;
                    if (dt != null)
                    {
                        dt = dt.Copy();
                        foreach (DataRow dr in dt.Rows)
                        {
                            string tgtArNo = ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.AR_NO);
                            if (!string.IsNullOrEmpty(cond.ARNo))
                            {
                                if (cond.ARNo.IndexOf(tgtArNo) != -1) continue;
                                cond.ARNo += ",";
                            }
                            cond.ARNo += ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.AR_NO);
                        }
                    }
                }
            }
            if (this.cboDispSelect.Enabled && this.cboDispSelect.SelectedValue != null)
            {
                cond.DispSelect = this.cboDispSelect.SelectedValue.ToString();
            }

            // 2012/05/07 K.Tsutsumi Add 履歴
            // 操作区分
            if (this.rdoInsert.Checked)
            {
                // 入力登録
                cond.OperationFlag = OPERATION_FLAG.S0100010_REGIST_VALUE1;
            }
            else if (this.rdoExcel.Checked)
            {
                // Excel登録
                cond.OperationFlag = OPERATION_FLAG.S0100010_REGIST_EXCEL_VALUE1;
            }
            else if (this.rdoUpdate.Checked)
            {
                if (this._isDeleteRecord == true)
                {
                    // 行削除
                    cond.OperationFlag = OPERATION_FLAG.S0100010_DELETE_ROW_VALUE1;
                }
                else
                {
                    // 修正
                    cond.OperationFlag = OPERATION_FLAG.S0100010_EDIT_VALUE1;
                }
            }
            else if (this.rdoDelete.Checked)
            {
                // 削除
                cond.OperationFlag = OPERATION_FLAG.S0100010_DELETE_VALUE1;
            }
            // 更新PC名
            cond.UpdatePCName = UtilSystem.GetUserInfo(false).MachineName;
            // ↑

            return cond;
        }

        #endregion

        #region 納入先、便、AR No.チェック

        /// --------------------------------------------------
        /// <summary>
        /// 納入先、便、AR No.チェック
        /// </summary>
        /// <param name="isSearch">検索時かどうか</param>
        /// <returns>true:OK/false:NG</returns>
        /// <create>Y.Higuchi 2010/07/09</create>
        /// <update>H.Tajimi 2015/11/18 Free1,Free2の列内同一チェック無効</update>
        /// <update>H.Tajimi 2015/11/30 納入先(User)選択を早くする</update>
        /// <update>T.Nakata 2018/11/14 手配業務対応</update>
        /// --------------------------------------------------
        private bool CheckNonyusakiAndAR(bool isSearch)
        {
            DataSet ds;
            // 2012/04/24 K.Tsutsumi Delete 納入先は事前に登録されている（よって、納入先コードは画面から渡される）
            //string nonyusakiCD;
            // ↑
            string errMsgID;
            string[] args;
            CondS01 cond = this.GetCondition();
            ConnS01 conn = new ConnS01();

            DataTable dt_shtMeisai = this.shtMeisai.DataSource as DataTable;
            if (dt_shtMeisai != null)
            {
                dt_shtMeisai = dt_shtMeisai.Copy();
            }

            // 入力登録の場合のデータ取得&チェック
            // 2012/04/24 K.Tsutsumi Change 納入先は事前に登録されている（よって、納入先コードは画面から渡される）
            //if (!conn.CheckDisplayCondition(cond, out ds, out nonyusakiCD, out errMsgID, out args))
            if (!conn.CheckDisplayCondition(cond, out ds, out errMsgID, out args))
            // ↑
            {
                if (isSearch)
                {
                    // 2011/03/08 K.Tsutsumi Change T_ARが存在しなくても続行可能
                    //if (errMsgID == "A9999999020" || errMsgID == "S0100020018")
                    if (errMsgID == "S0100020018")
                    // ↑
                    {
                        this.txtARNo.Focus();
                    }
                    else
                    {
                        // 2015/11/30 H.Tajimi 納入先にフォーカス設定
                        this.txtNonyusakiName.Focus();
                        // ↑
                    }
                }
                else
                {
                    // 2011/03/08 K.Tsutsumi Delete T_ARが存在しなくても続行可能
                    //if (errMsgID == "A9999999020")
                    //{
                    //    // 「AR No.が存在しません。」のメッセージを書き換える。
                    //    // 他端末でAR情報が削除されまています。
                    //    errMsgID = "S0100020035";
                    //}
                    // ↑
                    this.shtMeisai.Focus();
                }

                // ↓ 2018/11/19 T.Nakata 複数AR対応
                this.ShowMessage(errMsgID, args);
                // ↑
                return false;
            }

            if (this.rdoInsert.Checked || this.rdoExcel.Checked)
            {
                // 入力登録、Excel登録のチェック

                // 納入先チェック
                if (ds != null && ds.Tables.Contains(Def_M_NONYUSAKI.Name) && 0 < ds.Tables[Def_M_NONYUSAKI.Name].Rows.Count)
                {
                    // 本体
                    if (cond.ShukkaFlag == SHUKKA_FLAG.NORMAL_VALUE1)
                    {
                        if (!this._isAddDetailNonyusaki && ComFunc.GetFld(ds, Def_M_NONYUSAKI.Name, 0, Def_M_NONYUSAKI.KANRI_FLAG) == KANRI_FLAG.KANRYO_VALUE1)
                        {
                            // 完了納入先となっています。新たな明細発生ですか？\r\n"OK"で完了が解除されます。
                            if (this.ShowMessage("S0100020012") != DialogResult.OK)
                            {
                                // 2015/11/30 H.Tajimi 納入先にフォーカス設定
                                this.txtNonyusakiName.Focus();
                                // ↑
                                return false;
                            }
                            this._isAddDetailNonyusaki = true;
                            this._isAddDetailExists = true;
                        }
                        else if (!this._isAddDetailExists)
                        {
                            // 2012/05/07 K.Tsutsumi Delete 事前に納入先と便を選択するので確認する必要がない
                            //// 納入先・便は既に存在しています。明細追加ですか？
                            //if (this.ShowMessage("S0100020013") != DialogResult.OK)
                            //{
                            //    // 2012/04/24 K.Tsutsumi Change 納入先と便は常時Disable状態
                            //    //this.txtNonyusakiName.Focus();
                            //    this.btnListSelect.Focus();
                            //    // ↑
                            //    return false;
                            //}
                            // ↑
                            this._isAddDetailExists = true;
                        }
                    }
                }

                // AR情報チェック
                if (cond.ShukkaFlag == SHUKKA_FLAG.AR_VALUE1)
                {
                    // 2011/03/08 K.Tsutsumi Change T_ARが存在しなくても続行可能
                    //if (!this._isAddDetailAR && ComFunc.GetFld(ds, Def_T_AR.Name, 0, Def_T_AR.JYOKYO_FLAG) == JYOKYO_FLAG.KANRYO_VALUE1)
                    //{
                    //    // 完了AR No.となっています。　新たな明細発生ですか？\r\n"OK"で完了が解除されます。
                    //    if (this.ShowMessage("S0100020015") != DialogResult.OK)
                    //    {
                    //        this.txtNonyusakiName.Focus();
                    //        return false;
                    //    }
                    //    this._isAddDetailAR = true;
                    //}

                    if (dt_shtMeisai != null && dt_shtMeisai.Rows.Count > 0)
                    {
                        string tmpStr = string.Empty;
                        DataTable dtMessage = ComFunc.GetSchemeMultiMessage();
                        foreach (DataRow dr in dt_shtMeisai.Rows)
                        {
                            string TableName = Def_T_AR.Name + "-" + ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.AR_NO);

                            if (ComFunc.IsExistsData(ds, TableName) == true)
                            {
                                if (!this._isAddDetailAR && ComFunc.GetFld(ds, TableName, 0, Def_T_AR.JYOKYO_FLAG) == JYOKYO_FLAG.KANRYO_VALUE1)
                                {
                                    string tgtArNo = ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.AR_NO);
                                    if (tmpStr.IndexOf(tgtArNo) < 0)
                                    {
                                        // ・{0}
                                        ComFunc.AddMultiMessage(dtMessage, "S0100020051", tgtArNo);
                                        tmpStr += (tgtArNo + ",");
                                    }
                                }
                            }
                            // ↑
                        }
                        if (dtMessage.Rows.Count > 0)
                        {
                            // 下記の完了AR No.が選択されています。　新たな明細発生ですか？\r\n"OK"で完了が解除されます。\r\n※AR No.の一覧は右ClickでClipboardにCopyできます。
                            if (this.ShowMultiMessage(dtMessage, "S0100020050") != DialogResult.OK)
                            {
                                if (isSearch == true)
                                {
                                    this.txtNonyusakiName.Focus();
                                }
                                else
                                {
                                    this.shtMeisai.Focus();
                                }
                                return false;
                            }
                            this._isAddDetailAR = true;
                        }
                    }
                }
            }

            // 2011/03/10 K.Tsutsumi Add T_ARが存在しなくても続行可能
            if (dt_shtMeisai != null && dt_shtMeisai.Rows.Count > 0 && cond.ShukkaFlag == SHUKKA_FLAG.AR_VALUE1)
            {
                string tmpStr = string.Empty;
                DataTable dtMessage = ComFunc.GetSchemeMultiMessage();
                foreach (DataRow dr in dt_shtMeisai.Rows)
                {
                    string TableName = Def_T_AR.Name + "-" + ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.AR_NO);
                    if (ComFunc.IsExistsData(ds, TableName) == false)
                    {
                        if (this._isConfirmNoAR == false)
                        {
                            string tgtArNo = ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.AR_NO);
                            if (tmpStr.IndexOf(tgtArNo) < 0)
                            {
                                // ・{0}
                                ComFunc.AddMultiMessage(dtMessage, "S0100020051", tgtArNo);
                                tmpStr += (tgtArNo + ",");
                            }
                        }
                    }
                }
                if (dtMessage.Rows.Count > 0)
                {
                    // 下記AR No.のAR情報が登録されていません。\r\nこのまま処理を続けますか？\r\n※AR No.の一覧は右ClickでClipboardにCopyできます。
                    if (this.ShowMultiMessage(dtMessage, "S0100020052") != DialogResult.OK)
                    {
                        if (isSearch == true)
                        {
                            this.txtARNo.Focus();
                        }
                        else
                        {
                            this.shtMeisai.Focus();
                        }
                        return false;
                    }
                    this._isConfirmNoAR = true;
                }
            }
            else
            {
                this._isConfirmNoAR = false;
            }
            // ↑

            // 納入先コードを取得した値で更新
            this.txtNonyusakiCD.Text = ComFunc.GetFld(ds, Def_M_NONYUSAKI.Name, 0, Def_M_NONYUSAKI.NONYUSAKI_CD);
            return true;
        }

        #endregion

        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>Y.Higuchi 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            // グリッドクリア
            this.shtMeisai.Redraw = false;
            // 最も左上に表示されているセルの設定
            if (0 < this.shtMeisai.MaxRows)
            {
                this.shtMeisai.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtMeisai.TopLeft.Row);
            }
            this.shtMeisai.AllowUserToAddRows = false;
            this.shtMeisai.DataSource = null;
            this.shtMeisai.MaxRows = 0;
            this.shtMeisai.Enabled = false;
            this.shtMeisai.Redraw = true;
        }

        #endregion

        #region TagNo.のゼロ埋め

        /// --------------------------------------------------
        /// <summary>
        /// TagNo.のゼロ埋め
        /// </summary>
        /// <param name="tagNo">TagNo.</param>
        /// <returns>ゼロ埋めのTagNo.</returns>
        /// <create>Y.Higuchi 2010/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private string GetFormatTagNo(string tagNo)
        {
            if (tagNo == null) return string.Empty;
            int maxLen = 5;
            GrapeCity.Win.ElTabelle.Editors.TextEditor editor = this.shtMeisai.Columns[SHEET_COL_TAGNO].Editor as GrapeCity.Win.ElTabelle.Editors.TextEditor;
            if (editor != null)
            {
                maxLen = editor.MaxLength;
            }
            return tagNo.PadLeft(maxLen, '0');
        }
        #endregion

        #region 画面表示

        #region 納入先一覧画面表示

        /// --------------------------------------------------
        /// <summary>
        /// 納入先一覧画面表示
        /// </summary>
        /// <param name="mode">納入先一覧画面からデータ取得後に行う処理モード</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2015/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ShowNonyusakiIchiran(int mode)
        {
            string shukkaFlag = string.Empty;
            if (this.cboShukkaFlag.SelectedValue != null)
            {
                shukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
            }
            string nonyusakiName = this.txtNonyusakiName.Text;
            string ship = this.txtShip.Text;
            using (NonyusakiIchiran frm = new NonyusakiIchiran(this.UserInfo, shukkaFlag, nonyusakiName, ship, true))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    DataRow dr = frm.SelectedRowData;
                    if (dr == null) return false;

                    //ロック状態の場合編集不可
                    var lockFlag = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LOCK_FLAG);
                    if (lockFlag == "1" && this.rdoView.Checked == false)
                    {
                        this.ShowMessage("S0100020056");
                        return false;
                    }

                    // 選択データを設定
                    this.txtNonyusakiCD.Text = ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_CD);
                    this.txtNonyusakiName.Text = ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_NAME);
                    this.txtShip.Text = ComFunc.GetFld(dr, Def_M_NONYUSAKI.SHIP);

                    if (mode == MODE_SEARCH)
                    {
                        return this.RunSearch();
                    }
                    else if (mode == MODE_IMPORT_EXCEL)
                    {
                        return this.ExecuteImport();
                    }
                }
            }
            return false;
        }

        #endregion

        #endregion

        #region 照会モード時固有の検索条件ロック

        /// --------------------------------------------------
        /// <summary>
        /// 照会モード時固有の検索条件ロック
        /// </summary>
        /// <param name="isEnabled">Enabledにするかどうか</param>
        /// <create>H.Tajimi 2015/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeEnabledForViewMode(bool isEnabled)
        {
            if (isEnabled)
            {
                this.txtNonyusakiName.Enabled = true;
                if (this.cboShukkaFlag.SelectedValue != null && this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.AR_VALUE1)
                {
                    this.txtShip.Enabled = false;
                }
                else
                {
                    this.txtShip.Enabled = true;
                }
            }
            else
            {
                this.txtNonyusakiName.Enabled = false;
                this.txtShip.Enabled = false;
            }
        }
        
        #endregion

        #region TAG連携時初期処理

        /// --------------------------------------------------
        /// <summary>
        /// TAG連携時初期処理
        /// </summary>
        /// <create>T.Nakata 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private void InitializeTagrenkeiControl()
        {
            // 検索フィールド設定
            this.cboShukkaFlag.SelectedValue = int.Parse(_dtTagRenkeiData.ShukkaFlag);
            this.txtNonyusakiName.Text = _dtTagRenkeiData.BukkenName;
            this.txtShip.Text = _dtTagRenkeiData.ShipName;
            this.txtNonyusakiCD.Text = _dtTagRenkeiData.NonyusakiCD;

            // フォーム設定
            this.rdoInsert.Enabled = true;
            this.rdoExcel.Enabled = false;
            this.rdoUpdate.Enabled = false;
            this.rdoDelete.Enabled = false;
            this.rdoView.Enabled = false;
            this.rdoInsert.Checked = true;
            ChangeMode();
            this.btnStart.Enabled = false;
            this.grpSearch.Enabled = false;

            this.ChangeFunctionButton(true);
            this.fbrFunction.F06Button.Enabled = false;
            this.fbrFunction.F07Button.Enabled = false;

            // データ設定
            this.SheetClear();

            this.shtMeisai.Redraw = false;
            this.shtMeisai.DataSource = _dtTagRenkeiData.dtTagRenkeiList.Copy();
            this.shtMeisai.AllowUserToAddRows = true;
            this.shtMeisai.Redraw = true;
            this.shtMeisai.Enabled = true;
            this.shtMeisai.Focus();
        }
        
        #endregion
    }
}
