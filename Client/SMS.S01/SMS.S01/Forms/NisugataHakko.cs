using System;
using System.Collections.Generic;
using System.Linq;
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
using GrapeCity.Win.ElTabelle.Editors;
using ElTabelleHelper;

using WsConnection.WebRefS01;
using SMS.S01.Properties;
using SMS.E01;

namespace SMS.S01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 荷姿表登録
    /// </summary>
    /// <create>T.Nakata 2018/11/21</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class NisugataHakko : SystemBase.Forms.CustomOrderForm
    {

        #region enum

        /// --------------------------------------------------
        /// <summary>
        /// DB取得時のフィールドカラム番号
        /// </summary>
        /// <create>T.Nakata 2018/12/03</create>
        /// <update>H.Tajimi 2020/04/14 出荷元追加</update>
        /// --------------------------------------------------
        enum DB_FIELD
        {
            KOKUNAI_GAI_FLAG = 0,
            BUKKEN_NO,
            CANCEL_FLAG,
            DOUKON_FLAG,
            SHUKKA_FLAG,
            NONYUSAKI_CD,
            PACKING_NO,
            CT_QTY,
            INVOICE_NO,
            SYUKKA_DATE,
            HAKKO_FLAG,
            UNSOKAISHA_CD,
            CONSIGN_CD,
            CONSIGN_ATTN,
            DELIVER_CD,
            DELIVER_ATTN,
            PACKING_MAIL_SUBJECT,
            PACKING_REV,
            VERSION,
            NO,
            CT_NO,
            FORM_STYLE_FLAG,
            SIZE_L,
            SIZE_W,
            SIZE_H,
            GRWT,
            PRODUCT_NAME,
            ATTN,
            NOTE,
            PL_TYPE,
            CASE_NO,
            BOX_NO,
            PALLET_NO,
            ECS_NO,
            AR_NO,
            SEIBAN_CODE,
            SHIP,
            KONPO_NO,
            SHIP_FROM_CD,
            GYOUNO,
        }

        /// --------------------------------------------------
        /// <summary>
        /// シートインデックス
        /// </summary>
        /// <create>T.Nakata 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum SHEET_COL
        {
            CAN = 0,
            KOKUNAI,
            UNSOKAISYA,
            INVOICE,
            AR,
            ITEM,
            SHIP,
            ARNO,
            SEIBAN_CODE,
            ECSNO,
            ATESAKI,
            KONPOUNO,
            DOUKON,
            CTQTY,
            CTNO,
            PL_TYPE,
            TEIKEI,
            SIZE_L,
            SIZE_W,
            SIZE_H,
            GRWT,
            PRODUCT_NAME,
            NOTE,
            NOUNYU_CD,
            HAKKOU,
            GYOUNO,
        }

        #endregion

        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// 行番号のシートカラム名
        /// </summary>
        /// <create>T.Nakata 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string SHEET_COL_NAME_GYOUNO = "GYOUNO";
        /// --------------------------------------------------
        /// <summary>
        /// 梱包NoのDBカラム名
        /// </summary>
        /// <create>T.Nakata 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string DB_COL_NAME_KONPONO = "KONPO_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>T.Nakata 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int TOPLEFT_COL = 0;
        /// --------------------------------------------------
        /// <summary>
        /// 行操作（コピー・貼付）のセル範囲開始位置
        /// </summary>
        /// <create>T.Nakata 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int COPY_START = 0;
        /// --------------------------------------------------
        /// <summary>
        /// 行操作（コピー・貼付）のセル範囲終端位置
        /// </summary>
        /// <create>T.Nakata 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int COPY_END = 21;
        /// --------------------------------------------------
        /// <summary>
        /// 宛先最大文字数
        /// </summary>
        /// <create>T.Nakata 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int ATTN_MAX_LEN = 30;

        #endregion

        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 追加/変更時グリッドのEnabledを切り替える対象のカラム
        /// </summary>
        /// <create>T.Nakata 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private int[] _enableMOD = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 };
        /// --------------------------------------------------
        /// <summary>
        /// シートの変更イベント停止フラグ
        /// </summary>
        /// <create>T.Nakata 2018/12/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _IsSheetChangeEventDisable = false;
        /// --------------------------------------------------
        /// <summary>
        /// Item列変更管理フラグ
        /// </summary>
        /// <create>K.Tsutsumi 2019/03/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isItemValueChanged = false;
        /// --------------------------------------------------
        /// <summary>
        /// Ship列変更管理フラグ
        /// </summary>
        /// <create>K.Tsutsumi 2019/03/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isShipValueChanged = false;
        /// --------------------------------------------------
        /// <summary>
        /// 運送会社リスト(国内)
        /// </summary>
        /// <create>T.Nakata 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtUnsokaisyaIn = null;
        /// --------------------------------------------------
        /// <summary>
        /// 運送会社リスト(国外)
        /// </summary>
        /// <create>T.Nakata 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtUnsokaisyaOut = null;
        /// --------------------------------------------------
        /// <summary>
        /// 宛先リスト
        /// </summary>
        /// <create>T.Nakata 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtAtesaki = null;
        /// --------------------------------------------------
        /// <summary>
        /// 定形リスト
        /// </summary>
        /// <create>T.Nakata 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtTeikei = null;
        /// --------------------------------------------------
        /// <summary>
        /// PL_TYPEリスト
        /// </summary>
        /// <create>T.Nakata 2018/12/4</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtPlType = null;
        /// --------------------------------------------------
        /// <summary>
        /// 納入先情報保存用
        /// </summary>
        /// <create>K.Tsutsumi 2019/03/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private Dictionary<string, DataTable> dcNonyusaki = new Dictionary<string, DataTable>();
        /// --------------------------------------------------
        /// <summary>
        /// 物件リスト(本体)
        /// </summary>
        /// <create>T.Nakata 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtBukken = null;
        /// --------------------------------------------------
        /// <summary>
        /// 物件リスト(AR)
        /// </summary>
        /// <create>T.Nakata 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtBukkenAR = null;
        /// --------------------------------------------------
        /// <summary>
        /// 一覧表示用データ退避
        /// </summary>
        /// <create>T.Nakata 2018/11/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtSheet = null;
        /// --------------------------------------------------
        /// <summary>
        /// ログインユーザのメールアドレス
        /// </summary>
        /// <create>H.Tajimi 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _mailAddress = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 荷姿メール用ユーザマスタ
        /// </summary>
        /// <create>H.Tajimi 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtPackingUser = null;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷元マスタ
        /// </summary>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtShipFrom = null;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷元マスタ(全てを含む)
        /// </summary>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtShipFromContainsAll = null;
        /// --------------------------------------------------
        /// <summary>
        /// 検索条件
        /// </summary>
        /// <create>H.Tajimi 2020/04/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondS01 _condSearch = null;

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
        /// <create>T.Nakata 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public NisugataHakko(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
            : base(userInfo, menuCategoryID, menuItemID, title)
        {
            InitializeComponent();
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>T.Nakata 2018/11/21</create>
        /// <update>H.Tsuji 2019/08/01 Enterキー入力時のアクティブセル移動方向を制御</update>
        /// <update>H.Tajimi 2020/04/14 検索条件に出荷元、並び順追加</update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                // フォームの設定
                this.IsCloseQuestion = true;

                // シートの初期化
                this.InitializeSheet(this.shtNisugata);
                this.shtNisugata.DataBindingMode = DataBindingMode.Tight;

                // シートのタイトルを設定
                int i = 0;
                this.shtNisugata.ColumnHeaders[i++].Caption = Resources.NisugataHakko_Cancel;
                this.shtNisugata.ColumnHeaders[i++].Caption = Resources.NisugataHakko_Kokunai;
                this.shtNisugata.ColumnHeaders[i++].Caption = Resources.NisugataHakko_UnsoKaisha;
                this.shtNisugata.ColumnHeaders[i++].Caption = Resources.NisugataHakko_InvoiceNo;
                this.shtNisugata.ColumnHeaders[i++].Caption = Resources.NisugataHakko_AR;
                this.shtNisugata.ColumnHeaders[i++].Caption = Resources.NisugataHakko_Item;
                this.shtNisugata.ColumnHeaders[i++].Caption = Resources.NisugataHakko_Ship;
                this.shtNisugata.ColumnHeaders[i++].Caption = Resources.NisugataHakko_ARNo;
                this.shtNisugata.ColumnHeaders[i++].Caption = Resources.NisugataHakko_SeibanCode;
                this.shtNisugata.ColumnHeaders[i++].Caption = Resources.NisugataHakko_ECSNo;
                this.shtNisugata.ColumnHeaders[i++].Caption = Resources.NisugataHakko_Atesaki;
                this.shtNisugata.ColumnHeaders[i++].Caption = Resources.NisugataHakko_KonpoNo;
                this.shtNisugata.ColumnHeaders[i++].Caption = Resources.NisugataHakko_Doukon;
                this.shtNisugata.ColumnHeaders[i++].Caption = Resources.NisugataHakko_CTQty;
                this.shtNisugata.ColumnHeaders[i++].Caption = Resources.NisugataHakko_CTNo;
                this.shtNisugata.ColumnHeaders[i++].Caption = Resources.NisugataHakko_PLType;
                this.shtNisugata.ColumnHeaders[i++].Caption = Resources.NisugataHakko_Teikei;
                this.shtNisugata.ColumnHeaders[i++].Caption = Resources.NisugataHakko_SizeL;
                this.shtNisugata.ColumnHeaders[i++].Caption = Resources.NisugataHakko_SizeW;
                this.shtNisugata.ColumnHeaders[i++].Caption = Resources.NisugataHakko_SizeH;
                this.shtNisugata.ColumnHeaders[i++].Caption = Resources.NisugataHakko_GRWT;
                this.shtNisugata.ColumnHeaders[i++].Caption = Resources.NisugataHakko_Seihinmei;
                this.shtNisugata.ColumnHeaders[i++].Caption = Resources.NisugataHakko_Bikou;
                this.shtNisugata.ColumnHeaders[i++].Caption = Resources.NisugataHakko_NounyusakiCD;
                this.shtNisugata.ColumnHeaders[i++].Caption = Resources.NisugataHakko_Hakkou;

                // 初期で取得できるマスタはここで取得する
                var conn = new ConnS01();
                var result = conn.S0100050_GetInit(new CondS01(this.UserInfo));
                // コンボボックスの初期化
                this.MakeCmbBox(this.cboMailTitle, result.Tables[PACKING_MAIL_SUBJECT.GROUPCD], Def_M_COMMON.VALUE1, Def_M_COMMON.ITEM_NAME, Def_M_COMMON.DEFAULT_VALUE, false);
                this.MakeCmbBox(this.scboSortOrder, result.Tables[NISUGATA_HAKKO_SORT.GROUPCD], Def_M_COMMON.VALUE1, Def_M_COMMON.ITEM_NAME, Def_M_COMMON.DEFAULT_VALUE, false);

                // データ取得
                this._dtUnsokaisyaIn = result.Tables[Def_M_UNSOKAISHA.Name + KOKUNAI_GAI_FLAG.NAI_VALUE1].Copy();//運送会社(国内)
                this._dtUnsokaisyaOut = result.Tables[Def_M_UNSOKAISHA.Name + KOKUNAI_GAI_FLAG.GAI_VALUE1].Copy();//運送会社(国外)
                this._dtBukkenAR = result.Tables[Def_M_BUKKEN.Name].Copy(); //物件一覧(AR)
                this._dtShipFrom = result.Tables[Def_M_SHIP_FROM.Name].Copy();  //出荷元
                this._dtShipFromContainsAll = this._dtShipFrom.Copy();  //出荷元(全てを含む)
                var drShipFrom = this._dtShipFromContainsAll.NewRow();
                UtilData.SetFld(drShipFrom, Def_M_SHIP_FROM.SHIP_FROM_NO, ComDefine.COMBO_FIRST_VALUE);
                UtilData.SetFld(drShipFrom, Def_M_SHIP_FROM.SHIP_FROM_NAME, ComDefine.COMBO_ALL_DISP);
                this._dtShipFromContainsAll.Rows.InsertAt(drShipFrom, 0);

                // 並び順コンボボックスの初期化
                this.ClearSortOrder();
                // 出荷元コンボボックスの初期化
                this.SetComboShipFrom();

                this.shtNisugata.MaxRows = 1;

                //+++ 宛先 +++
                this._dtAtesaki = result.Tables[Def_M_SELECT_ITEM.Name].Copy();//宛先
                this.SetColumnTextHistory((int)SHEET_COL.ATESAKI, this._dtAtesaki, Def_M_SELECT_ITEM.ITEM_NAME);

                // 定形
                this._dtTeikei = result.Tables[FORM_STYLE_FLAG.GROUPCD].Copy(); //定形
                this.SetElTabelleColumn(this.shtNisugata, (int)SHEET_COL.TEIKEI, Resources.NisugataHakko_Teikei, false, false, Def_T_PACKING_MEISAI.FORM_STYLE_FLAG, this.InitColumnCombo(this._dtTeikei, Def_M_COMMON.ITEM_NAME, Def_M_COMMON.VALUE1, false, false), 81);

                // PL Type
                this._dtPlType = result.Tables[PL_TYPE.GROUPCD].Copy(); //パレットタイプ
                this.SetElTabelleColumn(this.shtNisugata, (int)SHEET_COL.PL_TYPE, Resources.NisugataHakko_PLType, false, false, Def_T_PACKING_MEISAI.PL_TYPE, this.InitColumnCombo(this._dtPlType, Def_M_COMMON.ITEM_NAME, Def_M_COMMON.VALUE1, false, false), 81);

                this.shtNisugata.MaxRows = 0;

                // Enterキー操作ラジオボタン初期化
                // ラジオボタンはチェックするとTabStopも自動でtrueとなるため、その都度falseにする必要がある
                this.rdoRight.Checked = true;
                this.rdoRight.TabStop = false;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                Cursor.Current = Cursors.Arrow;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示されるたびにコントロールの初期化するメソッド
        /// </summary>
        /// <create>T.Nakata 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカスの設定
                this.rdoInsert.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region クリア

        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>T.Nakata 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            // グリッドクリア
            // 最も左上に表示されているセルの設定
            if (0 < this.shtNisugata.MaxRows)
            {
                this.shtNisugata.TopLeft = new Position(TOPLEFT_COL, this.shtNisugata.TopLeft.Row);
            }
            this.shtNisugata.DataSource = null;
            this.shtNisugata.AllowUserToAddRows = false;
            this.shtNisugata.MaxRows = 0;
            this.shtNisugata.Enabled = false;

            // 保持データのクリア
            _dtSheet = null;
            dcNonyusaki.Clear();
        }

        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>T.Nakata 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // モード初期化
                this.rdoInsert.Checked = true;

                // 出荷日のクリア(翌日)
                this.dtpShukkaDateFrom.Value = DateTime.Today.AddDays(1);

                // グリッドのクリア
                this.SheetClear();

                // モード切替
                this.ChangeMode();

                this.cboMailTitle.Enabled = false;
                this.txtRev.Enabled = false;
                this.cboMailTitle.SelectedValue = DISP_SELECT.DEFAULT_VALUE1;

                // フォーカス移動
                this.rdoInsert.Focus();
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
        /// <create>K.Tsutsumi 2019/03/13</create>
        /// <update>H.Tajimi 2020/04/14 検索条件に出荷元、並び順追加</update>
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
                    // グリッドのEnable切り替え
                    this.ChangeSheetColumnEnabled(this._enableMOD, true);
                    this.scboSortOrder.Enabled = false;
                    this.ClearSortOrder();
                }
                else if (this.rdoDelete.Checked)
                {
                    // ----- 削除 -----
                    // 画面の編集モード変更
                    this.EditMode = SystemBase.EditMode.Delete;
                    // グリッドのEnable切り替え
                    this.ChangeSheetColumnEnabled(this._enableMOD, false);
                    this.scboSortOrder.Enabled = false;
                    this.ClearSortOrder();
                }
                else
                {
                    // ----- 照会 -----
                    // 画面の編集モード変更
                    this.EditMode = SystemBase.EditMode.View;
                    // グリッドのEnable切り替え
                    this.ChangeSheetColumnEnabled(this._enableMOD, false);
                    this.scboSortOrder.Enabled = true;
                    this.ClearSortOrder();
                }
                // ファンクションボタンの切替
                this.ChangeFunctionButton(false);
                // 検索条件有効化
                this.grpSearch.Enabled = true;
                // 出荷元コンボボックス設定
                this.SetComboShipFrom();
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
        /// <create>K.Tsutsumi 2019/03/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeFunctionButton(bool isEnabled)
        {
            if (this.rdoInsert.Checked)
            {
                // ----- 追加/変更 -----
                this.fbrFunction.F01Button.Enabled = isEnabled;
                this.fbrFunction.F02Button.Enabled = false;
                this.fbrFunction.F03Button.Enabled = isEnabled;
                this.fbrFunction.F04Button.Enabled = isEnabled;
                this.fbrFunction.F05Button.Enabled = isEnabled;
                this.fbrFunction.F06Button.Enabled = isEnabled;
                this.fbrFunction.F09Button.Enabled = isEnabled;
                this.fbrFunction.F10Button.Enabled = false;
            }
            else if (this.rdoDelete.Checked)
            {
                // ----- 削除 -----
                this.fbrFunction.F01Button.Enabled = isEnabled;
                this.fbrFunction.F02Button.Enabled = false;
                this.fbrFunction.F03Button.Enabled = false;
                this.fbrFunction.F04Button.Enabled = false;
                this.fbrFunction.F05Button.Enabled = false;
                this.fbrFunction.F06Button.Enabled = isEnabled;
                this.fbrFunction.F09Button.Enabled = false;
                this.fbrFunction.F10Button.Enabled = false;
            }
            else
            {
                // ----- 参照 -----
                this.fbrFunction.F01Button.Enabled = false;
                this.fbrFunction.F02Button.Enabled = isEnabled;
                this.fbrFunction.F03Button.Enabled = false;
                this.fbrFunction.F04Button.Enabled = false;
                this.fbrFunction.F05Button.Enabled = false;
                this.fbrFunction.F06Button.Enabled = isEnabled;
                this.fbrFunction.F09Button.Enabled = false;
                this.fbrFunction.F10Button.Enabled = isEnabled;
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
        /// <create>T.Nakata 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.shtNisugata.EditState) return;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

            base.fbrFunction_F01Button_Click(sender, e);
        }

        /// --------------------------------------------------
        /// <summary>
        /// F2ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F02Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F02Button_Click(sender, e);
            try
            {
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
        /// <create>T.Nakata 2018/11/22</create>
        /// <update>K.Tsutsumi 2019/03/11 Rangeの取得不具合対応</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F03Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F03Button_Click(sender, e);
            try
            {                
                // 行がない場合は処理を抜ける
                if (this.shtNisugata.Rows.Count < 1)
                {
                    return;
                }

                //選択されているセル範囲をすべて取得する(表の下から処理したいので、RangeをTopRowの降順で取得する)
                var objRanges = shtNisugata.GetBlocks(BlocksType.Selection).OrderByDescending(x => x.TopRow);

                //bool isDeleted = false;
                foreach (Range range in objRanges)
                {
                    // 行追加時の最終行の場合は処理を抜ける。
                    if ((this.shtNisugata.AllowUserToAddRows) &&
                       ((this.shtNisugata.Rows.Count - 1 == range.TopRow) || (this.shtNisugata.Rows.Count - 1 == range.BottomRow)))
                    {
                        // 行削除を行う場合は、最終行を含まないようにして下さい。
                        this.ShowMessage("S0100020040");
                        return;
                    }

                    //// Range内行ループ
                    //for (int row = range.TopRow; row <= range.BottomRow; row++)
                    //{
                    //    // 発行状態チェック
                    //    if (this.shtNisugata[HAKKOU, row].Value != null)
                    //    {
                    //        int HakkouFlag = UtilConvert.ToInt32(this.shtNisugata[HAKKOU, row].Value, 0);
                    //        if (UtilConvert.ToInt32(HAKKO_FLAG.COMP_VALUE1) == HakkouFlag)
                    //        {
                    //            // 選択行には発行済みの荷姿が含まれており削除できません。
                    //            this.ShowMessage("S0100020037");
                    //            return;
                    //        }
                    //    }
                    //}
                }

                // 選択行を削除してもよろしいですか？
                if (this.ShowMessage("S0100020038") == DialogResult.OK)
                {
                    try
                    {
                        this.shtNisugata.Redraw = false;
                        foreach (Range range in objRanges)
                        {
                            // 行数を求める
                            int rowCount = range.BottomRow - range.TopRow + 1;
                            // 行削除
                            this.shtNisugata.RemoveRow(range.TopRow, rowCount, false);
                        }
                    }
                    finally
                    {
                        this.shtNisugata.Redraw = true;
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
        /// F4ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/11/22</create>
        /// <update>K.Tsutsumi 2019/03/11 複数行対応</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F04Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F04Button_Click(sender, e);
            try
            {
                // 行がない場合は処理を抜ける
                if (this.shtNisugata.Rows.Count < 1)
                {
                    return;
                }
                //選択されているセル範囲をすべて取得する(表の下から処理したいので、RangeをTopRowの降順で取得する)
                var objRanges = this.shtNisugata.GetBlocks(BlocksType.Selection).OrderByDescending(x => x.TopRow);

                foreach (Range range in objRanges)
                {
                    // 行追加時の最終行の場合は処理を抜ける。
                    if ((this.shtNisugata.AllowUserToAddRows) &&
                       ((this.shtNisugata.Rows.Count - 1 == range.TopRow) || (this.shtNisugata.Rows.Count - 1 == range.BottomRow)))
                    {
                        // 行複製を行う場合は、最終行を含まないようにして下さい。
                        this.ShowMessage("S0100050031");
                        return;
                    }
                }

                this.shtNisugata.Redraw = false;
                // CellNotify, ValueChanged イベント 抑制
                this._IsSheetChangeEventDisable = true;

                foreach (Range range in objRanges)
                {
                    for (int row = range.BottomRow; row >= range.TopRow; row--)
                    {
                        this.SheetRowCopy(row);
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                // CellNotify, ValueChanged イベント 抑制解除
                this._IsSheetChangeEventDisable = false;
                this.shtNisugata.Redraw = true;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F5ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/11/22</create>
        /// <update>K.Tsutsumi 2019/03/14 複数行対応</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F05Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F05Button_Click(sender, e);
            try
            {
                // 行がないまたはコピー元がない場合は処理を抜ける
                if (this.shtNisugata.Rows.Count < 2)
                {
                    return;
                }

                if (this.shtNisugata.ActivePosition.Row < 1)
                {
                    // 項Copyを行う場合は、先頭行を含まないようにして下さい。
                    this.ShowMessage("S0100050039");
                    return;
                }

                int col = this.shtNisugata.ActivePosition.Column;
                int srcRow = this.shtNisugata.ActivePosition.Row - 1;
                
                // 無効セル/ロックセルの場合は処理しない
                if (!this.shtNisugata[col, srcRow].Enabled || this.shtNisugata[col, srcRow].Lock) return;

                // 取得元セルが無効セルの場合は処理しない
                if (col != (int)SHEET_COL.SHIP && !this.shtNisugata[col, srcRow].Enabled) return;

                //選択されているセル範囲をすべて取得する(RangeをTopRowの昇順で取得する)
                var objRanges = this.shtNisugata.GetBlocks(BlocksType.Selection).OrderBy(x => x.TopRow);

                foreach (Range range in objRanges)
                {
                    // 項コピー時の最終行の場合は処理を抜ける。
                    if (range.TopRow < 1)
                    {
                        // 項Copyを行う場合は、先頭行を含まないようにして下さい。
                        this.ShowMessage("S0100050039");
                        return;
                    }
                }

                try
                {
                    this.shtNisugata.Redraw = false;
                    // CellNotify, ValueChanged イベント 抑制
                    this._IsSheetChangeEventDisable = true;

                    foreach (Range range in objRanges)
                    {
                        for (int row = range.TopRow; row <= range.BottomRow; row++)
                        {
                            this.SheetCellCopy(col, srcRow, row);
                        }
                    }
                }
                finally
                {
                    // CellNotify, ValueChanged イベント 抑制解除
                    this._IsSheetChangeEventDisable = false;
                    this.shtNisugata.Redraw = true;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F6ボタンクリック(Clear)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/11/22</create>
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
                this.cboMailTitle.Enabled = false;
                this.txtRev.Enabled = false;
                this.cboMailTitle.SelectedValue = DISP_SELECT.DEFAULT_VALUE1;

                // 検索条件のロック解除
                this.grpSearch.Enabled = true;
                // ファンクションボタンの切替
                this.ChangeFunctionButton(false);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F7ボタンクリック(All Clear)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                if (ShowMessage("A9999999001") != DialogResult.OK) return;

                try
                {
                    this.shtNisugata.Redraw = false;
                    
                    // 画面クリア処理
                    this.DisplayClear();
                }
                finally
                {
                    this.shtNisugata.Redraw = true;
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
        /// <create>K.Tsutsumi 2019/03/11</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F09Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F09Button_Click(sender, e);
            try
            {
                // 行がない場合は処理を抜ける
                if (this.shtNisugata.Rows.Count < 1)
                {
                    return;
                }

                this.shtNisugata.Redraw = false;
                var srcRow = this.shtNisugata.ActivePosition.Row;
                var srcCol = this.shtNisugata.ActivePosition.Column;

                switch (srcCol)
                {
                    case (int)SHEET_COL.KOKUNAI:
                    case (int)SHEET_COL.UNSOKAISYA:
                    case (int)SHEET_COL.INVOICE:
                    case (int)SHEET_COL.ATESAKI:
                    case (int)SHEET_COL.PL_TYPE:
                        //同一便への同時反映
                        this.CopyToSameShipmentNumber(srcRow, srcCol);
                        break;

                    case (int)SHEET_COL.CTQTY:
                    case (int)SHEET_COL.CTNO:
                        var srcAR = SHUKKA_FLAG.NORMAL_VALUE1;                       
                        if (this.shtNisugata[(int)SHEET_COL.AR, srcRow].Value != null) srcAR = int.Parse(this.shtNisugata[(int)SHEET_COL.AR, srcRow].Value.ToString()) == 0 ? SHUKKA_FLAG.NORMAL_VALUE1 : SHUKKA_FLAG.AR_VALUE1;
                        // ARの場合は便という概念がない、また同梱もそんざいするので、AR単位というわけでもないということで、この機能は使えない。
                        if (srcAR == SHUKKA_FLAG.AR_VALUE1) break;
                        // そもそもITEMとSHIPが確定していないということは論外である。
                        if (this.shtNisugata[(int)SHEET_COL.ITEM, srcRow].Value == null || this.shtNisugata[(int)SHEET_COL.SHIP, srcRow].Value == null) break;
                        var srcProject = this.shtNisugata[(int)SHEET_COL.ITEM, srcRow].Value.ToString();
                        var srcShip = this.shtNisugata[(int)SHEET_COL.SHIP, srcRow].Value.ToString();

                        DataTable dt = (DataTable)this.shtNisugata.DataSource;
                        var count = dt.AsEnumerable().Where(x => (string.IsNullOrEmpty(ComFunc.GetFld(x, Def_M_NONYUSAKI.SHUKKA_FLAG)) || ComFunc.GetFld(x, Def_M_NONYUSAKI.SHUKKA_FLAG) == SHUKKA_FLAG.NORMAL_VALUE1) && ComFunc.GetFld(x, Def_M_NONYUSAKI.BUKKEN_NO) == srcProject && ComFunc.GetFld(x, Def_M_NONYUSAKI.SHIP) == srcShip).Count();

                        this.shtNisugata[(int)SHEET_COL.CTQTY, srcRow].Value = count;
                        //同一便への同時反映
                        this.CopyToSameShipmentNumber(srcRow, srcCol);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.shtNisugata.Redraw = true;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F10ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>K.Tsutsumi 2019/03/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F10Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F10Button_Click(sender, e);
            try
            {
                this.RunExcelOnly();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region ラジオボタン

        /// --------------------------------------------------
        /// <summary>
        /// 追加/変更ラジオボタンのCheckedChangedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/11/21</create>
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
        /// 削除ラジオボタンのCheckedChangedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/11/21</create>
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
        /// 参照ラジオボタンのCheckedChangedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoView_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoView.Checked)
            {
                this.ClearMessage();
                this.ChangeMode();
            }
        }

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
                this.shtNisugata.ShortCuts.Remove(Keys.Enter);
                
                // ラジオボタンはチェックするとTabStopも自動でtrueとなるため、その都度falseにする必要がある
                if (this.rdoRight.Checked)
                {
                    this.shtNisugata.ShortCuts.Add(Keys.Enter, new KeyAction[] { KeyAction.NextCellWithWrap });
                    this.rdoRight.TabStop = false;
                }
                else
                {
                    this.shtNisugata.ShortCuts.Add(Keys.Enter, new KeyAction[] { KeyAction.EndEdit, KeyAction.NextRow });
                    this.rdoDown.TabStop = false;
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
        /// <create>K.Tsutsumi 2019/03/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();

                // ValueChanged Event 抑制
                this._IsSheetChangeEventDisable = true;

                // 検索制御
                bool ret = this.RunSearch();
                if (!ret)
                {
                    // フォーカス移動
                    this.dtpShukkaDateFrom.Focus();
                    return;
                }
                // 検索条件を無効化
                this.grpSearch.Enabled = false;
                // 表の有効化
                this.shtNisugata.Enabled = true;
                // ファンクションボタンの切替
                this.ChangeFunctionButton(true);
                // 照会のみ
                if (this.rdoView.Checked)
                {
                    this.cboMailTitle.Enabled = true;
                    this.txtRev.Enabled = true;
                }
                // フォーカス
                this.shtNisugata.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                // ValueChanged Event 抑制解除
                this._IsSheetChangeEventDisable = false;
                Cursor.Current = Cursors.Arrow;
            }
        }

        #endregion

        #region シート

        #region Notify

        /// --------------------------------------------------
        /// <summary>
        /// シートイベント処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>K.Tsutsumi 2019/03/14</create>
        /// <update></update>
        /// <remarks>値の取得には ActiveCell を使用している -> 変更後の値が取得したい</remarks>
        /// --------------------------------------------------
        private void shtNisugata_CellNotify(object sender, CellNotifyEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("shtNisugata_CellNotify(col, row)(" + e.Position.Column.ToString() + ", " + e.Position.Row.ToString() + ") : " + e.Name + ", " + e.CellEvent.ToString());
            try
            {
                if (this._IsSheetChangeEventDisable) return;
                if (!this.rdoInsert.Checked) return;

                int row = e.Position.Row;

                switch (e.Name)
                {
                    #region CheckedChanged イベント処理

                    case CellNotifyEvents.CheckedChanged:
                        // チェックボックスイベント処理
                        bool isChecked = int.Parse(this.shtNisugata.ActiveCell.Value.ToString()) == 0 ? false : true;
                        switch (e.Position.Column)
                        {
                            case (int)SHEET_COL.CAN:
                                {// CAN列
                                    try
                                    {
                                        // CellNotify, ValueChanged Event 抑制
                                        this._IsSheetChangeEventDisable = true;
                                        //+++++ Enable切り替え +++++
                                        this.shtNisugata_CheckedChanged_CAN(row, isChecked);
                                        // 操作性向上の為、フォーカス移動
                                        this.shtNisugata.ActivePosition = new Position(e.Position.Column + 1, e.Position.Row);
                                        this.shtNisugata.ActivePosition = new Position(e.Position.Column, e.Position.Row);
                                    }
                                    finally
                                    {
                                        // CellNotify, ValueChanged Event 解除
                                        this._IsSheetChangeEventDisable = false;
                                    }
                                }
                                break;
                            case (int)SHEET_COL.KOKUNAI:
                                {// 国内列
                                    //+++++ 運送会社切り替え +++++
                                    this.shtNisugata_CheckedChanged_Kokunai(row, isChecked);
                                    // 操作性向上の為、フォーカス移動
                                    this.shtNisugata.ActivePosition = new Position(e.Position.Column + 1, e.Position.Row);
                                    this.shtNisugata.ActivePosition = new Position(e.Position.Column, e.Position.Row);
                                }
                                break;
                            case (int)SHEET_COL.AR:
                                {// AR列
                                    try
                                    {
                                        // CellNotify, ValueChanged Event 抑制
                                        this._IsSheetChangeEventDisable = true;
                                        this.shtNisugata_CheckedChanged_AR(row, isChecked);
                                        
                                    }
                                    finally
                                    {
                                        // CellNotify, ValueChanged Event 抑制解除
                                        this._IsSheetChangeEventDisable = false;
                                    }
                                    // 操作性向上の為、フォーカス移動
                                    this.shtNisugata.ActivePosition = new Position(e.Position.Column + 1, e.Position.Row);
                                    this.shtNisugata.ActivePosition = new Position(e.Position.Column, e.Position.Row);
                                }
                                break;
                            case (int)SHEET_COL.DOUKON:
                                {// 同梱列
                                    //+++++ Enable切り替え +++++
                                    this.shtNisugata_CheckedChanged_Doukon(row, isChecked);
                                }
                                break;
                        }
                        break;
                    #endregion

                    #region TextChanged イベント処理

                    case CellNotifyEvents.TextChanged:                        
                        switch (e.Position.Column)
                        {
                            case (int)SHEET_COL.ITEM:
                                this._isItemValueChanged = true;
                                break;
                            case (int)SHEET_COL.SHIP:
                                this._isShipValueChanged = true;
                                break;
                            default:
                                break;
                        }                        
                        break;
                    #endregion

                    #region SelectedIndexChanged イベント処理

                    case CellNotifyEvents.SelectedIndexChanged:
                        switch (e.Position.Column)
                        {
                            case (int)SHEET_COL.ITEM:
                                //try
                                //{
                                //    // CellNotify, ValueChanged Event 抑制
                                //    this._IsSheetChangeEventDisable = true;
                                //    this.shtNisugata_ValueChanged_Item(row, this.shtNisugata.ActiveCell.Value);
                                //}
                                //finally
                                //{
                                //    // CellNotify, ValueChanged Event 抑制解除
                                //    this._IsSheetChangeEventDisable = false;
                                //}
                                this._isItemValueChanged = true;
                                break;

                            case (int)SHEET_COL.SHIP:
                                //try
                                //{
                                //    // CellNotify, ValueChanged Event 抑制
                                //    this._IsSheetChangeEventDisable = true;
                                //    this.shtNisugata_ValueChanged_Ship(row, this.shtNisugata.ActiveCell.Value);
                                //}
                                //finally
                                //{
                                //    // CellNotify, ValueChanged Event 抑制解除
                                //    this._IsSheetChangeEventDisable = false;
                                //}
                                this._isShipValueChanged = true;
                                break;

                            case (int)SHEET_COL.TEIKEI:
                                try
                                {
                                    // CellNotify, ValueChanged Event 抑制
                                    this._IsSheetChangeEventDisable = true;
                                    this.shtNisugata_ValueChanged_FormStyleFlag(row, this.shtNisugata.ActiveCell.Value);
                                }
                                finally
                                {
                                    // CellNotify, ValueChanged Event 抑制解除
                                    this._IsSheetChangeEventDisable = false;
                                }
                                break;

                            default:
                                break;
                        }
                        break;
                    #endregion

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region LeaveCell

        /// --------------------------------------------------
        /// <summary>
        /// シートのイベント(LeaveCell)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>K.Tsutsumi 2019/03/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtNisugata_LeaveCell(object sender, LeaveCellEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("shtNisugata_LeaveCell(newCol, newRow)(" + e.NewPosition.Column.ToString() + ", " + e.NewPosition.Row.ToString() + ") : " + this._IsSheetChangeEventDisable.ToString() + ", " + e.MoveStatus.ToString());
            try
            {
                Sheet sheet = sender as Sheet;
                AddNoneItemSuperiorComboEditor(sheet, sheet.ActivePosition.Row);

                if (this._IsSheetChangeEventDisable) return;

                if (this.shtNisugata.ActivePosition.Column == (int)SHEET_COL.ITEM ||
                    this.shtNisugata.ActivePosition.Column == (int)SHEET_COL.SHIP)
                {

                    if (!this._isItemValueChanged || !this._isShipValueChanged)
                    {

                        // コンボボックス内のアイテム文字列を入力した形にする。
                        if (!string.IsNullOrEmpty(this.shtNisugata.ActiveCell.Text))
                        {
                            var editor = this.shtNisugata.ActiveCell.Editor as SuperiorComboEditor;
                            var dt = editor.DataSource as DataTable;
                            string search = this.shtNisugata.ActiveCell.Text;
                            var dr = dt.AsEnumerable().Where(d => UtilData.GetFld(d, editor.DisplayMember).StartsWith(search)).FirstOrDefault();
                            if (dr != null)
                            {
                                this.shtNisugata.ActiveCell.Text = UtilData.GetFld(dr, editor.DisplayMember);
                                this.shtNisugata.ActiveCell.Value = UtilData.GetFld(dr, editor.ValueMember);
                            }
                            else
                            {
                                this.shtNisugata.ActiveCell.Value = null;
                            }
                        }
                    }

                    // 各種 ValueChanged イベント呼び出し
                    if (this.shtNisugata.ActivePosition.Column == (int)SHEET_COL.ITEM && this._isItemValueChanged)
                    {
                        this.shtNisugata_ValueChanged_Item(this.shtNisugata.ActivePosition.Row, this.shtNisugata.ActiveCell.Value);
                    }
                    else if (this.shtNisugata.ActivePosition.Column == (int)SHEET_COL.SHIP && this._isShipValueChanged)
                    {
                        this.shtNisugata_ValueChanged_Ship(this.shtNisugata.ActivePosition.Row, this.shtNisugata.ActiveCell.Value);
                    }
                
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this._isItemValueChanged = false;
                this._isShipValueChanged = false;
            }
        }

        #endregion

        #region Leave

        /// --------------------------------------------------
        /// <summary>
        /// Leave Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>K.Tsutsumi 2019/03/23</create>
        /// <update></update>
        /// <remarks>LeaveCellと同様の処理。きれいにしたかったけど時間がなかった</remarks>
        /// --------------------------------------------------
        private void shtNisugata_Leave(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("shtNisugata_Leave(activeCol, activeRow)(" + this.shtNisugata.ActivePosition.Column.ToString() + ", " + this.shtNisugata.ActivePosition.Row.ToString() + ")");
            try
            {
                if (this._IsSheetChangeEventDisable) return;

                if (this.shtNisugata.ActivePosition.Column == (int)SHEET_COL.ITEM ||
                    this.shtNisugata.ActivePosition.Column == (int)SHEET_COL.SHIP)
                {

                    if (!this._isItemValueChanged || !this._isShipValueChanged)
                    {

                        // コンボボックス内のアイテム文字列を入力した形にする。
                        if (!string.IsNullOrEmpty(this.shtNisugata.ActiveCell.Text))
                        {
                            var editor = this.shtNisugata.ActiveCell.Editor as SuperiorComboEditor;
                            var dt = editor.DataSource as DataTable;
                            string search = this.shtNisugata.ActiveCell.Text;
                            var dr = dt.AsEnumerable().Where(d => UtilData.GetFld(d, editor.DisplayMember).StartsWith(search)).FirstOrDefault();
                            if (dr != null)
                            {
                                this.shtNisugata.ActiveCell.Text = UtilData.GetFld(dr, editor.DisplayMember);
                                this.shtNisugata.ActiveCell.Value = UtilData.GetFld(dr, editor.ValueMember);
                            }
                            else
                            {
                                this.shtNisugata.ActiveCell.Value = null;
                            }
                        }
                    }

                    // 各種 ValueChanged イベント呼び出し
                    if (this.shtNisugata.ActivePosition.Column == (int)SHEET_COL.ITEM && this._isItemValueChanged)
                    {
                        this.shtNisugata_ValueChanged_Item(this.shtNisugata.ActivePosition.Row, this.shtNisugata.ActiveCell.Value);
                    }
                    else if (this.shtNisugata.ActivePosition.Column == (int)SHEET_COL.SHIP && this._isShipValueChanged)
                    {
                        this.shtNisugata_ValueChanged_Ship(this.shtNisugata.ActivePosition.Row, this.shtNisugata.ActiveCell.Value);
                    }

                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this._isItemValueChanged = false;
                this._isShipValueChanged = false;
            }
        }

        #endregion

        #region RowInserted

        /// --------------------------------------------------
        /// <summary>
        /// シートへの行追加時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtNisugata_RowInserted(object sender, RowInsertedEventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("shtNisugata_RowInserted(start, count)(" + e.Span.Start.ToString() + ", " + e.Span.Count.ToString() + ")");

                int row = e.Span.Start;
                // 新規行は、CAN off/ 国外/ 本体 とする

                // 運送会社
                this.SetComboUnsokaisya(row, KOKUNAI_GAI_FLAG.GAI_VALUE1);

                // AR - Item - Ship
                this.shtNisugata_CheckedChanged_AR(row, false);

                // 宛先
                this.SetComboAtesaki(row);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region ValueChanged

        /// --------------------------------------------------
        /// <summary>
        /// セル値変更時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtNisugata_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("shtNisugata_ValueChanged(col, row)(" + e.Position.Column.ToString() + ", " + e.Position.Row.ToString() + ") :" + this._IsSheetChangeEventDisable.ToString());
            if (this._IsSheetChangeEventDisable) return;
            if (!this.rdoInsert.Checked) return;

            int row = e.Position.Row;
            switch (e.Position.Column)
            {
                //case (int)SHEET_COL.ITEM:
                //    this.shtNisugata_ValueChanged_Item(row, this.shtNisugata[(int)SHEET_COL.ITEM, row].Value);
                //    break;
                //case (int)SHEET_COL.SHIP:
                //    this.shtNisugata_ValueChanged_Ship(row, this.shtNisugata[(int)SHEET_COL.SHIP, row].Value);
                //    break;

                case (int)SHEET_COL.KONPOUNO:
                    string changedValue = string.Empty;
                    if (this.shtNisugata[(int)SHEET_COL.KONPOUNO, row].Value != null) changedValue = this.shtNisugata[(int)SHEET_COL.KONPOUNO, row].Value.ToString();
                    if (string.IsNullOrEmpty(changedValue)) break;

                    string shukkaFlag = SHUKKA_FLAG.NORMAL_VALUE1;
                    if (this.shtNisugata[(int)SHEET_COL.AR, row].Value != null) shukkaFlag = this.shtNisugata[(int)SHEET_COL.AR, row].Value.ToString();
                    string bukkenNo = string.Empty;
                    if (this.shtNisugata[(int)SHEET_COL.ITEM, row].Value != null) bukkenNo = this.shtNisugata[(int)SHEET_COL.ITEM, row].Value.ToString();
                    string ship = string.Empty;
                    if (this.shtNisugata[(int)SHEET_COL.SHIP, row].Value != null) ship = this.shtNisugata[(int)SHEET_COL.SHIP, row].Value.ToString();
                    string arNo = string.Empty;
                    if (this.shtNisugata[(int)SHEET_COL.ARNO, row].Value != null) arNo = this.shtNisugata[(int)SHEET_COL.ARNO, row].Value.ToString();
                    if (shukkaFlag == SHUKKA_FLAG.AR_VALUE1)
                    {
                        // AR
                        if (string.IsNullOrEmpty(bukkenNo) || string.IsNullOrEmpty(arNo))
                        {
                            // 梱包No.を入力する前に、Item、AR No.を先に確定させて下さい。
                            this.ShowMessage("S0100050032");
                            this.shtNisugata[(int)SHEET_COL.KONPOUNO, row].Value = null;
                            break;
                        }
                    }
                    else
                    {
                        // 本体
                        if (string.IsNullOrEmpty(bukkenNo) || string.IsNullOrEmpty(ship))
                        {
                            // 梱包No.を入力する前に、Item、Shipを先に確定させて下さい。
                            this.ShowMessage("S0100050033");
                            this.shtNisugata[(int)SHEET_COL.KONPOUNO, row].Value = null;
                            break;
                        }
                    }
                    string nonyusakiCd = string.Empty;
                    if (this.shtNisugata[(int)SHEET_COL.NOUNYU_CD, row].Value != null) nonyusakiCd = this.shtNisugata[(int)SHEET_COL.NOUNYU_CD, row].Value.ToString();


                    try
                    {
                        // CellNotify, ValueChanged Event 抑制
                        this._IsSheetChangeEventDisable = true;
                        this.shtNisugata_ValueChanged_KonpouNo(row, shukkaFlag, nonyusakiCd, arNo, changedValue);
                    }
                    finally
                    {

                        // CellNotify, ValueChanged Event 抑制解除
                        this._IsSheetChangeEventDisable = false;
                    }
                    break;

            }
        }

        #endregion

        #region RowFilling

        /// --------------------------------------------------
        /// <summary>
        /// データソース設定時の行追加時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>データソースにDataRowが追加または削除され、シートの行が追加または削除された後に該当行が表示されるか、または、コード上からセルの次のプロパティが参照された後に発生します。</remarks>
        /// <create>T.Nakata 2018/11/26</create>
        /// <update>H.Tajimi 2020/04/20 荷姿表を入力途中で保存できるように変更</update>
        /// --------------------------------------------------
        private void shtNisugata_RowFilling(object sender, RowFillingEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("shtNisugata_RowFilling(SourceRow, DestRow)(" + e.SourceRow.ToString() + ", " + e.DestRow.ToString() + "):" + e.OperationMode.ToString());

            if (e.OperationMode != OperationMode.Add) return;

            try
            {
                int row = e.DestRow;

                //----- データ取得 -----
                var shukkaFlag = ((object[])e.SourceRow)[(int)DB_FIELD.SHUKKA_FLAG].ToString();
                var cancelFlag = int.Parse(((object[])e.SourceRow)[(int)DB_FIELD.CANCEL_FLAG].ToString()) == 0 ? false : true;
                int bukkenNo = -1;
                if (((object[])e.SourceRow)[(int)DB_FIELD.BUKKEN_NO] != null)
                {
                    bukkenNo = int.Parse(((object[])e.SourceRow)[(int)DB_FIELD.BUKKEN_NO].ToString());
                }
                // 新規行は国外なのでデフォルトは国外とする
                string kokunaigaiFlag = KOKUNAI_GAI_FLAG.GAI_VALUE1;
                if (((object[])e.SourceRow)[(int)DB_FIELD.KOKUNAI_GAI_FLAG] != null)
                {
                    kokunaigaiFlag = ((object[])e.SourceRow)[(int)DB_FIELD.KOKUNAI_GAI_FLAG].ToString();
                }

                //----- コンボボックス操作 -----
                //+++ 運送会社 +++
                this.SetComboUnsokaisya(row, kokunaigaiFlag);
                //+++ 物件名 +++
                this.SetComboItem(row, shukkaFlag);
                //+++ SHIP +++
                if (bukkenNo != -1)
                {
                    this.SetComboShip(row, shukkaFlag, bukkenNo);
                }

                // 宛先
                this.SetComboAtesaki(row);

                //----- Enable操作 -----
                if (this.rdoInsert.Checked)
                {
                    // Cancel による Enable設定
                    this.shtNisugata_CheckedChanged_CAN(row, cancelFlag);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region BindingError

        /// --------------------------------------------------
        /// <summary>
        /// データバインド時のエラー処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>K.Tsutsumi 2019/03/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtNisugata_BindingError(object sender, BindingErrorEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("shtNisugata_BindingError(" + e.Position.Column.ToString() + ", " + e.Position.Row.ToString() + ")[" + e.Value.ToString() + "] : " + e.ErrorMessage);
            try
            {
                Sheet sheet = sender as Sheet;
                int col = e.Position.Column;
                int row = e.Position.Row;

                if (this.IsSheetBindingInvalidate(e))
                {
                    switch (col)
                    {
                        case (int)SHEET_COL.ATESAKI:
                            // コンボボックスに項目が存在しない場合、リストに追加する
                            SuperiorComboEditor cbo = sheet[col, row].Editor as SuperiorComboEditor;
                            bool isExists = false;
                            foreach (ComboItem item in cbo.Items)
                            {
                                if (item.Value.ToString().Equals(e.Value.ToString()))
                                {
                                    isExists = true;
                                    break;
                                }
                            }
                            if (!isExists)
                                cbo.Items.Add(new ComboItem(0, null, e.Value.ToString(), string.Empty, e.Value.ToString()));
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

        #endregion

        #endregion

        #endregion

        #region シート操作

        #region 項コピー処理

        /// --------------------------------------------------
        /// <summary>
        /// 項コピー処理
        /// </summary>
        /// <param name="sheet"></param>
        /// <create>T.Nakata 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetCellCopy(int col, int srcRow, int dstRow)
        {
            try
            {
                var sheet = this.shtNisugata;

                // AR/Item/Shipセルの場合はブロックで処理する
                if ((col == (int)SHEET_COL.AR)
                    || (col == (int)SHEET_COL.ITEM)
                    || (col == (int)SHEET_COL.SHIP)
                    )
                {
                    if (sheet[(int)SHEET_COL.NOUNYU_CD, srcRow].Value != null && sheet[(int)SHEET_COL.NOUNYU_CD, srcRow].Value.ToString() != string.Empty)
                    {
                        bool isCheckedSrcAR = false;
                        if (sheet[(int)SHEET_COL.AR, srcRow].Value != null) isCheckedSrcAR = int.Parse(sheet[(int)SHEET_COL.AR, srcRow].Value.ToString()) == 0 ? false : true;
                        bool isCheckedDstAR = false;
                        if (sheet[(int)SHEET_COL.AR, dstRow].Value != null) isCheckedDstAR = int.Parse(sheet[(int)SHEET_COL.AR, dstRow].Value.ToString()) == 0 ? false : true;
                        if (isCheckedSrcAR != isCheckedDstAR)
                        {
                            sheet[(int)SHEET_COL.AR, dstRow].Value = sheet[(int)SHEET_COL.AR, srcRow].Value;
                            this.shtNisugata_CheckedChanged_AR(dstRow, isCheckedSrcAR);
                        }

                        var SrcVal = (sheet[(int)SHEET_COL.ITEM, srcRow].Value == null ? string.Empty : sheet[(int)SHEET_COL.ITEM, srcRow].Value.ToString());
                        var DstVal = (sheet[(int)SHEET_COL.ITEM, dstRow].Value == null ? string.Empty : sheet[(int)SHEET_COL.ITEM, dstRow].Value.ToString());
                        if (SrcVal != DstVal)
                        {
                            sheet[(int)SHEET_COL.ITEM, dstRow].Value = sheet[(int)SHEET_COL.ITEM, srcRow].Value;
                            this.shtNisugata_ValueChanged_Item(dstRow, sheet[(int)SHEET_COL.ITEM, dstRow].Value);
                        }

                        SrcVal = (sheet[(int)SHEET_COL.SHIP, srcRow].Value == null ? string.Empty : sheet[(int)SHEET_COL.SHIP, srcRow].Value.ToString());
                        DstVal = (sheet[(int)SHEET_COL.SHIP, dstRow].Value == null ? string.Empty : sheet[(int)SHEET_COL.SHIP, dstRow].Value.ToString());
                        if (SrcVal != DstVal)
                        {
                            sheet[(int)SHEET_COL.SHIP, dstRow].Value = sheet[(int)SHEET_COL.SHIP, srcRow].Value;
                            this.shtNisugata_ValueChanged_Ship(dstRow, sheet[(int)SHEET_COL.SHIP, dstRow].Value);
                        }
                    }
                }
                // 国内外/運送会社セルの場合ブロックで処理する
                else if ((col == (int)SHEET_COL.KOKUNAI)
                    || (col == (int)SHEET_COL.UNSOKAISYA)
                    )
                {
                    bool isCheckedSrcKokunai = false;
                    if (sheet[(int)SHEET_COL.KOKUNAI, srcRow].Value != null) isCheckedSrcKokunai = int.Parse(sheet[(int)SHEET_COL.KOKUNAI, srcRow].Value.ToString()) == 0 ? false : true;
                    bool isCheckedDstKokunai = false;
                    if (sheet[(int)SHEET_COL.KOKUNAI, dstRow].Value != null) isCheckedDstKokunai = int.Parse(sheet[(int)SHEET_COL.KOKUNAI, dstRow].Value.ToString()) == 0 ? false : true;
                    if (isCheckedSrcKokunai != isCheckedDstKokunai)
                    {
                        sheet[(int)SHEET_COL.KOKUNAI, dstRow].Value = sheet[(int)SHEET_COL.KOKUNAI, srcRow].Value;
                        this.shtNisugata_CheckedChanged_Kokunai(dstRow, isCheckedSrcKokunai);
                    }
                    var SrcVal = (sheet[(int)SHEET_COL.UNSOKAISYA, srcRow].Value == null ? string.Empty : sheet[(int)SHEET_COL.UNSOKAISYA, srcRow].Value.ToString());
                    var DstVal = (sheet[(int)SHEET_COL.UNSOKAISYA, dstRow].Value == null ? string.Empty : sheet[(int)SHEET_COL.UNSOKAISYA, dstRow].Value.ToString());
                    if (SrcVal != DstVal)
                    {
                        sheet[(int)SHEET_COL.UNSOKAISYA, dstRow].Value = sheet[(int)SHEET_COL.UNSOKAISYA, srcRow].Value;
                    }
                }
                // その他セル
                else
                {
                    // ターゲットセルから値を取得
                    sheet[col, dstRow].Value = sheet[col, srcRow].Value;

                    if (col == (int)SHEET_COL.CAN)
                    {
                        bool isCheckedCan = false;
                        if (sheet[col, srcRow].Value != null) isCheckedCan = int.Parse(sheet[col, srcRow].Value.ToString()) == 0 ? false : true;
                        this.shtNisugata_CheckedChanged_CAN(dstRow, isCheckedCan);
                    }
                    else if (col == (int)SHEET_COL.TEIKEI)
                    {
                        this.shtNisugata_ValueChanged_FormStyleFlag(dstRow, sheet[col, srcRow].Value);
                    }
                    else if (col == (int)SHEET_COL.DOUKON)
                    {
                        bool isCheckedDoukon = false;
                        if (sheet[col, srcRow].Value != null) isCheckedDoukon = int.Parse(sheet[col, srcRow].Value.ToString()) == 0 ? false : true;
                        this.shtNisugata_CheckedChanged_Doukon(dstRow, isCheckedDoukon);
                    }

                }

                //sheet.EditState = true;//操作性を考慮し荷姿では事後に編集モードとはしない
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 行コピー処理

        /// --------------------------------------------------
        /// <summary>
        /// 行コピー処理
        /// </summary>
        /// <param name="sheet"></param>
        /// <create>T.Nakata 2018/11/28</create>
        /// <update>K.Tsutsumi 2019/03/11 複数行対応</update>
        /// --------------------------------------------------
        private void SheetRowCopy(int srcRow)
        {
            var sheet = this.shtNisugata;

            int src_row = srcRow;
            int dst_row = src_row + 1;
            if (src_row < 0 || src_row > (sheet.Rows.Count - 2)) return;

            //===== 1行追加 =====
            sheet.InsertRow(dst_row, false);

            //===== 行全体をコピー =====
            this.shtNisugata.Copy(new GrapeCity.Win.ElTabelle.Range((int)SHEET_COL.KOKUNAI, src_row, sheet.Columns.Count - 1, src_row),
                new GrapeCity.Win.ElTabelle.Position((int)SHEET_COL.KOKUNAI, dst_row),
                GrapeCity.Win.ElTabelle.DataTransferMode.DataOnly);


            //===== 不要な情報を削除 =====
            // 梱包No.
            sheet[(int)SHEET_COL.KONPOUNO, dst_row].Value = null;
            // 備考
            sheet[(int)SHEET_COL.NOTE, dst_row].Value = null;
            // GRWT
            sheet[(int)SHEET_COL.GRWT, dst_row].Value = null;
            // 発行状態
            sheet[(int)SHEET_COL.HAKKOU, dst_row].Value = null;

            // 運送会社
            bool isCheckedKokunai = false;
            if (sheet[(int)SHEET_COL.KOKUNAI, src_row].Value != null) isCheckedKokunai = int.Parse(sheet[(int)SHEET_COL.KOKUNAI, src_row].Value.ToString()) == 0 ? false : true;
            this.shtNisugata_CheckedChanged_Kokunai(dst_row, isCheckedKokunai);
            // 1度消えてしまうので、もう1度コピー
            sheet[(int)SHEET_COL.UNSOKAISYA, dst_row].Value = sheet[(int)SHEET_COL.UNSOKAISYA, src_row].Value;

            // AR
            bool isCheckedAR = false;
            if (sheet[(int)SHEET_COL.AR, src_row].Value != null) isCheckedAR = int.Parse(sheet[(int)SHEET_COL.AR, src_row].Value.ToString()) == 0 ? false : true;
            this.shtNisugata_CheckedChanged_AR(dst_row, isCheckedAR);
            // 1度消えてしまうので、もう1度コピー
            sheet[(int)SHEET_COL.ITEM, dst_row].Value = sheet[(int)SHEET_COL.ITEM, src_row].Value;
            // ITEM
            this.shtNisugata_ValueChanged_Item(dst_row, sheet[(int)SHEET_COL.ITEM, src_row].Value);

            // SHIP以降            
            if (!isCheckedAR)
            {
                // 1度消えてしまうので、もう1度コピー
                sheet[(int)SHEET_COL.SHIP, dst_row].Value = sheet[(int)SHEET_COL.SHIP, src_row].Value;

                // SHIP
                this.shtNisugata_ValueChanged_Ship(dst_row, sheet[(int)SHEET_COL.SHIP, dst_row].Value);

                // 定形
                this.shtNisugata_ValueChanged_FormStyleFlag(dst_row, sheet[(int)SHEET_COL.TEIKEI, dst_row].Value);
            }
            else
            {
                // 同梱
                bool isCheckedDoukon = false;
                if (sheet[(int)SHEET_COL.DOUKON, src_row].Value != null) isCheckedDoukon = int.Parse(sheet[(int)SHEET_COL.DOUKON, src_row].Value.ToString()) == 0 ? false : true;
                this.shtNisugata_CheckedChanged_Doukon(dst_row, isCheckedDoukon);
            }
        }

        #endregion

        #region グリッドEnabled切替

        /// --------------------------------------------------
        /// <summary>
        /// グリッドのEnabled切替
        /// </summary>
        /// <param name="Range">指定列</param>
        /// <param name="Row">指定行</param>
        /// <param name="isEnabled">Enabledの値</param>
        /// <create>T.Nakata 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeSheetColumnEnabled(int[] Range, bool isEnabled)
        {
            try
            {
                foreach (int index in Range)
                {
                    this.shtNisugata.Columns[index].Enabled = isEnabled;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #region FormStylFlage

        /// --------------------------------------------------
        /// <summary>
        /// FormStylFlageのEnable操作
        /// </summary>
        /// <param name="row"></param>
        /// <param name="enabled"></param>
        /// <create>K.Tsutsumi 2019/03/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetEnabled_FormStyleFlag(int row, bool enabled)
        {
            try
            {
                // SIZE列
                this.shtNisugata[(int)SHEET_COL.SIZE_W, row].Enabled = enabled;
                this.shtNisugata[(int)SHEET_COL.SIZE_L, row].Enabled = enabled;
                this.shtNisugata[(int)SHEET_COL.SIZE_H, row].Enabled = enabled;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #endregion

        #region 各列 CheckedChanged Event

        #region CAN

        /// --------------------------------------------------
        /// <summary>
        /// CAN列チェック時Enable切り替え
        /// </summary>
        /// <param name="Row">指定行</param>
        /// <param name="isChecked">Checkedの値</param>
        /// <create>K.Tsutsumi 2019/03/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtNisugata_CheckedChanged_CAN(int row, bool isChecked)
        {
            try
            {
                // 国内
                this.shtNisugata[(int)SHEET_COL.KOKUNAI, row].Enabled = !isChecked;
                // 運送会社
                this.shtNisugata[(int)SHEET_COL.UNSOKAISYA,row].Enabled = !isChecked;
                // Invoice no.
                this.shtNisugata[(int)SHEET_COL.INVOICE,row].Enabled = !isChecked;
                // AR
                this.shtNisugata[(int)SHEET_COL.AR,row].Enabled = !isChecked;
                // ITEM
                this.shtNisugata[(int)SHEET_COL.ITEM,row].Enabled = !isChecked;
                // 製番
                this.shtNisugata[(int)SHEET_COL.SEIBAN_CODE, row].Enabled = !isChecked;
                // ECS
                this.shtNisugata[(int)SHEET_COL.ECSNO,row].Enabled = !isChecked;
                // 宛先
                this.shtNisugata[(int)SHEET_COL.ATESAKI,row].Enabled = !isChecked;
                // 梱包No.
                this.shtNisugata[(int)SHEET_COL.KONPOUNO,row].Enabled = !isChecked;
                // 製品名
                this.shtNisugata[(int)SHEET_COL.PRODUCT_NAME,row].Enabled = !isChecked;
                // 備考
                this.shtNisugata[(int)SHEET_COL.NOTE,row].Enabled = !isChecked;
                if (false == isChecked)
                {
                    // 復帰は面倒だが、現在値から調整
                    bool isCheckedAR = false;
                    if (this.shtNisugata[(int)SHEET_COL.AR, row].Value != null) isCheckedAR = int.Parse(this.shtNisugata[(int)SHEET_COL.AR, row].Value.ToString()) == 0 ? false : true;
                    if (!isCheckedAR)
                    {
                        // 本体
                        // SHIP
                        if (this.shtNisugata[(int)SHEET_COL.SHIP, row].Editor is SuperiorComboEditor)
                        {
                            var editor = this.shtNisugata[(int)SHEET_COL.SHIP, row].Editor as SuperiorComboEditor;
                            if (UtilData.ExistsData(editor.DataSource as DataTable))
                            {
                                this.shtNisugata[(int)SHEET_COL.SHIP, row].Enabled = true;
                            }
                            else
                            {
                                this.shtNisugata[(int)SHEET_COL.SHIP, row].Enabled = false;
                            }
                        }
                        // AR No.
                        this.shtNisugata[(int)SHEET_COL.ARNO, row].Enabled = false;
                        // 同梱
                        this.shtNisugata[(int)SHEET_COL.DOUKON, row].Enabled = false;
                        // CT QTY
                        this.shtNisugata[(int)SHEET_COL.CTQTY, row].Enabled = true;
                        // CT No.
                        this.shtNisugata[(int)SHEET_COL.CTNO, row].Enabled = true;
                        // PL TYPE
                        this.shtNisugata[(int)SHEET_COL.PL_TYPE, row].Enabled = true;
                        // 定形
                        this.shtNisugata[(int)SHEET_COL.TEIKEI, row].Enabled = true;
                        // SizeL, SizeW, SizeH
                        this.SetEnabled_FormStyleFlag(row, !this.IsStandardSize(this.shtNisugata[(int)SHEET_COL.TEIKEI, row].Value));
                        // GRWT
                        this.shtNisugata[(int)SHEET_COL.GRWT, row].Enabled = true;

                    }
                    else
                    {
                        // AR
                        // SHIP
                        this.shtNisugata[(int)SHEET_COL.SHIP, row].Enabled = false;
                        // AR No.
                        this.shtNisugata[(int)SHEET_COL.ARNO, row].Enabled = true;
                        // 同梱
                        this.shtNisugata[(int)SHEET_COL.DOUKON, row].Enabled = true;
                        var isCheckedDoukon = int.Parse(this.shtNisugata[(int)SHEET_COL.DOUKON, row].Value.ToString()) == 0 ? false : true;
                        if (isCheckedDoukon)
                        {
                            // 同梱
                            // CT QTY
                            this.shtNisugata[(int)SHEET_COL.CTQTY, row].Enabled = false;
                            // CT No.
                            this.shtNisugata[(int)SHEET_COL.CTNO, row].Enabled = false;
                            // PL TYPE
                            this.shtNisugata[(int)SHEET_COL.PL_TYPE, row].Enabled = false;
                            // 定形
                            this.shtNisugata[(int)SHEET_COL.TEIKEI, row].Enabled = false;
                            // SizeL, SizeW, SizeH
                            this.SetEnabled_FormStyleFlag(row, false);
                            // GRWT列
                            this.shtNisugata[(int)SHEET_COL.GRWT, row].Enabled = false;
                        }
                        else
                        {
                            // 個別
                            // CT QTY
                            this.shtNisugata[(int)SHEET_COL.CTQTY, row].Enabled = true;
                            // CT No.
                            this.shtNisugata[(int)SHEET_COL.CTNO, row].Enabled = true;
                            // PL TYPE
                            this.shtNisugata[(int)SHEET_COL.PL_TYPE, row].Enabled = true;
                            // 定形
                            this.shtNisugata[(int)SHEET_COL.TEIKEI, row].Enabled = true;
                            // SizeL, SizeW, SizeH
                            this.SetEnabled_FormStyleFlag(row, !this.IsStandardSize(this.shtNisugata[(int)SHEET_COL.TEIKEI, row].Value));
                            // GRWT列
                            this.shtNisugata[(int)SHEET_COL.GRWT, row].Enabled = true;
                        }

                    }
                }
                else
                {
                    // CANCELの場合は、強制的に Enable操作する。

                    // SHIP
                    this.shtNisugata[(int)SHEET_COL.SHIP, row].Enabled = false;
                    // AR No.
                    this.shtNisugata[(int)SHEET_COL.ARNO, row].Enabled = false;
                    // 同梱
                    this.shtNisugata[(int)SHEET_COL.DOUKON, row].Enabled = false;
                    // CT QTY
                    this.shtNisugata[(int)SHEET_COL.CTQTY, row].Enabled = false;
                    // CT No.
                    this.shtNisugata[(int)SHEET_COL.CTNO, row].Enabled = false;
                    // PL TYPE
                    this.shtNisugata[(int)SHEET_COL.PL_TYPE, row].Enabled = false;
                    // 定形
                    this.shtNisugata[(int)SHEET_COL.TEIKEI, row].Enabled = false;
                    // SIZE L
                    this.shtNisugata[(int)SHEET_COL.SIZE_L, row].Enabled = false;
                    // SIZE W
                    this.shtNisugata[(int)SHEET_COL.SIZE_W, row].Enabled = false;
                    // SIZE H
                    this.shtNisugata[(int)SHEET_COL.SIZE_H, row].Enabled = false;
                    // GRWT
                    this.shtNisugata[(int)SHEET_COL.GRWT, row].Enabled = false;
                }

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 国内/外

        /// --------------------------------------------------
        /// <summary>
        /// 国内列チェック時イベント処理
        /// </summary>
        /// <param name="Row">指定行</param>
        /// <param name="isChecked">Checkedの値</param>
        /// <create>K.Tsutsumi 2019/03/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtNisugata_CheckedChanged_Kokunai(int row, bool isChecked)
        {
            var kokunaigaiFlag = KOKUNAI_GAI_FLAG.GAI_VALUE1;
            if (isChecked) kokunaigaiFlag = KOKUNAI_GAI_FLAG.NAI_VALUE1;

            this.SetComboUnsokaisya(row, kokunaigaiFlag);
        }

        #endregion

        #region AR

        /// --------------------------------------------------
        /// <summary>
        /// AR列チェック時イベント処理
        /// </summary>
        /// <param name="Row">指定行</param>
        /// <param name="isChecked">Checkedの値</param>
        /// <create>K.Tsutsumi 2019/03/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtNisugata_CheckedChanged_AR(int row, bool isChecked)
        {
            try
            {
                var shukkaFlag = SHUKKA_FLAG.NORMAL_VALUE1;
                if (isChecked)
                {
                    // 本体 -> AR
                    shukkaFlag = SHUKKA_FLAG.AR_VALUE1;
                    // AR No.列 Enable on
                    this.shtNisugata[(int)SHEET_COL.ARNO, row].Enabled = true;
                    // 同梱列 Enable on
                    this.shtNisugata[(int)SHEET_COL.DOUKON, row].Enabled = true;
                }
                else
                {
                    // AR -> 本体
                    // AR No.列 クリア & Enable off
                    this.shtNisugata[(int)SHEET_COL.ARNO, row].Enabled = false;
                    // 同梱列 クリア & Enable on
                    if (this.shtNisugata.Rows.Count - 1 > row) this.shtNisugata[(int)SHEET_COL.DOUKON, row].Value = 0;
                    this.shtNisugata[(int)SHEET_COL.DOUKON, row].Enabled = false;
                }
                // ITEM列
                this.SetComboItem(row, shukkaFlag);
                // ITEM列が未設定になるため、SHIP列 クリア & Enable (新規行の場合は、Value値を変更しないこと)
                if (this.shtNisugata.Rows.Count - 1 > row) this.shtNisugata[(int)SHEET_COL.SHIP, row].Value = null;
                this.shtNisugata[(int)SHEET_COL.SHIP, row].Enabled = false;
                // 納入先CD列 ITEM & SHIP が未選択になるため、クリア(新規行の場合は、Value値を変更しないこと)
                if (this.shtNisugata.Rows.Count - 1 > row) this.shtNisugata[(int)SHEET_COL.NOUNYU_CD, row].Value = null;
                // CT QTY列
                this.shtNisugata[(int)SHEET_COL.CTQTY, row].Enabled = true;
                // CT No.列
                this.shtNisugata[(int)SHEET_COL.CTNO, row].Enabled = true;
                // PL TYPE列
                this.shtNisugata[(int)SHEET_COL.PL_TYPE, row].Enabled = true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 同梱

        /// --------------------------------------------------
        /// <summary>
        /// 同梱列チェック時イベント処理
        /// </summary>
        /// <param name="Row">指定行</param>
        /// <param name="isEnabled">Enabledの値</param>
        /// <create>K.Tsutsumi 2019/03/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtNisugata_CheckedChanged_Doukon(int row, bool isEnabled)
        { 
            try
            {
                // CT QTY列
                this.shtNisugata[(int)SHEET_COL.CTQTY, row].Enabled = !isEnabled;
                // CT No.列
                this.shtNisugata[(int)SHEET_COL.CTNO, row].Enabled = !isEnabled;
                // PL TYPE列
                this.shtNisugata[(int)SHEET_COL.PL_TYPE, row].Enabled = !isEnabled;
                // FORM STYLE列
                this.shtNisugata[(int)SHEET_COL.TEIKEI, row].Enabled = !isEnabled;
                if (isEnabled)
                {
                    // SIZE L列
                    this.shtNisugata[(int)SHEET_COL.SIZE_L, row].Enabled = !isEnabled;
                    // SIZE W列
                    this.shtNisugata[(int)SHEET_COL.SIZE_W, row].Enabled = !isEnabled;
                    // SIZE H列
                    this.shtNisugata[(int)SHEET_COL.SIZE_H, row].Enabled = !isEnabled;
                }
                else
                {
                    // 復帰は、FORM STYLE列に影響される
                    // SizeL, SizeW, SizeH
                    this.SetEnabled_FormStyleFlag(row, !this.IsStandardSize(this.shtNisugata[(int)SHEET_COL.TEIKEI, row].Value));
                }
                // GRWT列
                this.shtNisugata[(int)SHEET_COL.GRWT, row].Enabled = !isEnabled;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #endregion

        #region 各列 ValueChanged Event

        #region Item

        /// --------------------------------------------------
        /// <summary>
        /// Item列 ValueChangeイベント処理
        /// </summary>
        /// <param name="row">対象行</param>
        /// <param name="changedValue">変更後の値</param>
        /// <create>K.Tsutsumi 2019/03/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtNisugata_ValueChanged_Item(int row, object changedValue)
        {
            try
            {
                var shukkaFlag = SHUKKA_FLAG.NORMAL_VALUE1;
                if (this.shtNisugata[(int)SHEET_COL.AR, row].Value != null) shukkaFlag = this.shtNisugata[(int)SHEET_COL.AR, row].Value.ToString();
                if (shukkaFlag == SHUKKA_FLAG.NORMAL_VALUE1)
                {
                    if (changedValue != null)
                    {
                        // 物件が確定している
                        // SHIP列 コンボボックス再設定 & Enable on
                        var bukkenNo = int.Parse(changedValue.ToString());
                        this.SetComboShip(row, shukkaFlag, bukkenNo);
                        this.shtNisugata[(int)SHEET_COL.SHIP, row].Enabled = true;
                    }
                    else
                    {
                        // 物件が確定していない                    
                        // SHIP列 クリア & Enable off
                        this.shtNisugata[(int)SHEET_COL.SHIP, row].Value = null;
                        this.shtNisugata[(int)SHEET_COL.SHIP, row].Enabled = false;
                        // 納入先列 クリア
                        this.shtNisugata[(int)SHEET_COL.SHIP, row].Value = null;
                    }
                }
                else
                {
                    // AR
                    if (changedValue != null)
                    {
                        // 物件が確定している
                        this.SetNonyusakiCd_AR(row, int.Parse(changedValue.ToString()));
                    }
                    else
                    {
                        // 物件が確定していない
                        // 納入先列 クリア
                        this.shtNisugata[(int)SHEET_COL.SHIP, row].Value = null;
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region Ship

        /// --------------------------------------------------
        /// <summary>
        /// Shp列 ValueChangedイベント処理
        /// </summary>
        /// <param name="row">対象行</param>
        /// <param name="changedValue">変更後の値</param>
        /// <create>K.Tsutsumi 2019/03/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtNisugata_ValueChanged_Ship(int row, object changedValue)
        {   
            try
            {
                var ship = string.Empty;
                if (changedValue != null) ship = changedValue.ToString();
                var bukkenNo = int.Parse(this.shtNisugata[(int)SHEET_COL.ITEM, row].Value.ToString());
                this.SetNonyusakiCd_Regular(row, bukkenNo, ship);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region KonpouNo

        /// --------------------------------------------------
        /// <summary>
        /// KonpouNo列 ValueChangedイベント処理
        /// </summary>
        /// <param name="row"></param>
        /// <param name="shukkaFlag"></param>
        /// <param name="nonyusakiCd"></param>
        /// <param name="ARNo"></param>
        /// <param name="changedValue"></param>
        /// <create>K.Tsutsumi 2019/03/19</create>
        /// <update>K.Tsutsumi 2020/04/26 潜在バグ ケースNoを4桁以上で入力すると落ちる</update>
        /// <update>K.Tsutsumi 2020/11/10 cond.ARNoの設定値が間違っていた不具合を修正</update>
        /// --------------------------------------------------
        private void shtNisugata_ValueChanged_KonpouNo(int row, string shukkaFlag, string nonyusakiCd, string ARNo, string changedValue)
        {
            try
            {
                this.ClearMessage();

                CondS01 cond = new CondS01(this.UserInfo);
                ConnS01 conn = new ConnS01();
                string errMsgID = string.Empty;
                string[] args = null;
                cond.ShukkaFlag = shukkaFlag;
                cond.NonyusakiCD = nonyusakiCd;
                cond.ARNo = ARNo;

                if (changedValue.StartsWith(ComDefine.PREFIX_BOXNO))
                {
                    // Box No.
                    cond.BoxNo = changedValue;
                    bool ret = conn.S0100050_GetBoxListManage(cond, out errMsgID, out args);
                    if (!ret)
                    {
                        this.ShowMessage(errMsgID, args);
                        this.shtNisugata[(int)SHEET_COL.KONPOUNO, row].Value = null;
                    }

                }
                else if (changedValue.StartsWith(ComDefine.PREFIX_PALLETNO))
                {
                    // Pallet No.
                    cond.PalletNo = changedValue;
                    bool ret = conn.S0100050_GetPalletListManage(cond, out errMsgID, out args);
                    if (!ret)
                    {
                        this.ShowMessage(errMsgID, args);
                        this.shtNisugata[(int)SHEET_COL.KONPOUNO, row].Value = null;
                    }
                }
                else
                { 
                    // 木枠 Case No.
                    int konponoVal;
                    if (int.TryParse(changedValue, out konponoVal))
                    {
                        if (konponoVal < 1000)
                        {
                            cond.CaseNo = changedValue;
                            var ds = conn.S0100050_GetKiwakuMeisai(cond);
                            if (!UtilData.ExistsData(ds, Def_T_KIWAKU_MEISAI.Name))
                            {
                                // Case No.は存在しません。
                                this.ShowMessage("S0100050036");
                            }
                            else
                            {
                                var dt = ds.Tables[Def_T_KIWAKU_MEISAI.Name];
                                this.shtNisugata[(int)SHEET_COL.PL_TYPE, row].Value = PL_TYPE.CRATE_VALUE1;
                                this.shtNisugata[(int)SHEET_COL.TEIKEI, row].Value = FORM_STYLE_FLAG.FORM_0_VALUE1;
                                this.shtNisugata[(int)SHEET_COL.SIZE_L, row].Value = UtilData.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.DIMENSION_L);
                                this.shtNisugata[(int)SHEET_COL.SIZE_W, row].Value = UtilData.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.DIMENSION_W);
                                this.shtNisugata[(int)SHEET_COL.SIZE_H, row].Value = UtilData.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.DIMENSION_H);
                                this.shtNisugata[(int)SHEET_COL.GRWT, row].Value = UtilData.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.GROSS_W);
                            }
                        }
                        else
                        {
                            // Case No.は存在しません。
                            this.ShowMessage("S0100050036");
                        }
                    }
                    else
                    {
                        // Case No.は存在しません。
                        this.ShowMessage("S0100050036");
                    }
                }

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region FormStyleFlag

        /// --------------------------------------------------
        /// <summary>
        /// FormStyleの選択肢が変更された場合の処理
        /// </summary>
        /// <param name="row">指定行</param>
        /// <param name="changedValue">選択後の値</param>
        /// <param name="isStopDraw">再描画</param>
        /// <create>D.Okumura 2018/11/22</create>
        /// <update>K.Tsutsumi 2019/03/11 全体的に遅い</update>
        /// <remarks>
        /// 定形の場合は値を表示しますが、定形外の場合は値をクリアしません。
        /// </remarks>
        /// --------------------------------------------------
        private void shtNisugata_ValueChanged_FormStyleFlag(int row, object changedValue)
        {
            try
            {

                int sizeL;
                int sizeW;
                int sizeH;
                var isStandard = IsStandardSize(changedValue, out sizeL, out sizeW, out sizeH);

                if (isStandard)
                {
                    // 定形なら値を表示する
                    this.shtNisugata[(int)SHEET_COL.SIZE_L, row].Value = sizeL;
                    this.shtNisugata[(int)SHEET_COL.SIZE_W, row].Value = sizeW;
                    this.shtNisugata[(int)SHEET_COL.SIZE_H, row].Value = sizeH;
                }

                // Enable操作
                this.SetEnabled_FormStyleFlag(row, !isStandard);

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #endregion

        #region コンボボックス作成

        /// --------------------------------------------------
        /// <summary>
        /// コンボボックス設定
        /// </summary>
        /// <param name="Col">列</param>
        /// <param name="Row">行</param>
        /// <param name="dt">セットするデータテーブル</param>
        /// <param name="key1">DataRow取得キー(Text用)</param>
        /// <param name="key2">DataRow取得キー(Value用)</param>
        /// <param name="AutoSelect">AutoSelect有効/無効</param>
        /// <create>K.Tsutsumi 2019/03/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetComboDt(int Col, int Row, DataTable dt, string KeyText, string keyValue, bool AutoSelect, bool Editable)
        {
            try
            {
                // コンボボックスセット
                SuperiorComboEditor objCombo = new SuperiorComboEditor();
                if (dt != null)
                {
                    objCombo.DataSource = dt.Copy();
                    objCombo.DisplayMember = KeyText;
                    objCombo.ValueMember = keyValue;
                    //objCombo.AutoSelect = AutoSelect;
                    objCombo.AutoDropDown = AutoSelect;
                    objCombo.Editable = Editable;
                    objCombo.ValueAsIndex = false;

                    this.shtNisugata[Col, Row].Value = null;
                    this.shtNisugata[Col, Row].Editor = objCombo;
                }
                else
                {
                    objCombo.Items.Add("");
                    this.shtNisugata[Col, Row].Editor = objCombo;
                    this.shtNisugata[Col, Row].Value = null;
                    this.shtNisugata[Col, Row].Enabled = false;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 列単位で同一のコンボボックス設定
        /// </summary>
        /// <param name="dt">セットするデータテーブル</param>
        /// <param name="displayMember">DataRow取得キー(Text用)</param>
        /// <param name="valueMember">DataRow取得キー(Value用)</param>
        /// <param name="isAutoSelect">AutoSelect有効/無効</param>
        /// <param name="isEditable"></param>
        /// <create>K.Tsutsumi 2019/03/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private SuperiorComboEditor InitColumnCombo(DataTable dt, string displayMember, string valueMember, bool isAutoSelect, bool isEditable)
        {
            // コンボボックスセット
            var objCombo = ElTabelleSheetHelper.NewSuperiorComboEditor(dt, displayMember, valueMember);
            objCombo.AutoSelect = isAutoSelect;
            objCombo.Editable = isEditable;
            objCombo.ValueAsIndex = false;
            return objCombo;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 列単位で同一の履歴リスト
        /// </summary>
        /// <param name="col">列</param>
        /// <param name="dt">セットするデータテーブル</param>
        /// <create>K.Tsutsumi 2019/03/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetColumnTextHistory(int col, DataTable dt, string valueMember)
        {
            try
            {
                // ヒストリー
                TextEditor objText = this.shtNisugata.Columns[col].Editor as TextEditor;
                if (objText != null)
                {
                    if (dt.Rows.Count > objText.MaxHistoryCount)
                    {
                        // 設定値より大きい場合は、一応レコード数に合わせる
                        objText.MaxHistoryCount = dt.Rows.Count;
                    }
                    var list = new List<string>();
                    foreach (DataRow dr in dt.Rows)
                    {                        
                        list.Insert(list.Count, UtilData.GetFld(dr, valueMember));
	                }
                    objText.HistoryList = list.ToArray();
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 運送会社用コンボボックス設定
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="kokunaigaiFlag">国内外フラグ</param>
        /// <create>K.Tsutsumi 2019/03/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetComboUnsokaisya(int row, string kokunaigaiFlag)
        {
            try
            {
                // コンボボックスセット
                if (kokunaigaiFlag == KOKUNAI_GAI_FLAG.GAI_VALUE1)
                {
                    this.SetComboDt((int)SHEET_COL.UNSOKAISYA, row, this._dtUnsokaisyaOut, Def_M_UNSOKAISHA.UNSOKAISHA_NAME, Def_M_UNSOKAISHA.UNSOKAISHA_NO, false, false);
                }
                else
                {
                    this.SetComboDt((int)SHEET_COL.UNSOKAISYA, row, this._dtUnsokaisyaIn, Def_M_UNSOKAISHA.UNSOKAISHA_NAME, Def_M_UNSOKAISHA.UNSOKAISHA_NO, false, false);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// Item用コンボボックス設定
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="shukkaFlag">出荷区分</param>
        /// <create>K.Tsutsumi 2019/03/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetComboItem(int row, string shukkaFlag)
        {
            try
            {
                if (shukkaFlag == SHUKKA_FLAG.NORMAL_VALUE1)
                {
                    // 納入先一覧取得
                    this.SetComboDt((int)SHEET_COL.ITEM, row, this._dtBukken, Def_M_BUKKEN.BUKKEN_NAME, Def_M_BUKKEN.BUKKEN_NO, true, true);
                }
                else
                {
                    // AR
                    this.SetComboDt((int)SHEET_COL.ITEM, row, this._dtBukkenAR, Def_M_BUKKEN.BUKKEN_NAME, Def_M_BUKKEN.BUKKEN_NO, true, true);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// Ship用コンボボックス設定
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="shipDate">出荷日</param>
        /// <param name="bukkenNo">物件管理No</param>
        /// <create>K.Tsutsumi 2019/03/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetComboShip(int row, string shukkaFlag, int bukkenNo)
        {
            try
            {
                // 納入先一覧取得
                if (shukkaFlag == SHUKKA_FLAG.NORMAL_VALUE1)
                {
                    // コンボボックスセット
                    this.SetComboDt((int)SHEET_COL.SHIP, row, this.dcNonyusaki[bukkenNo.ToString()], Def_M_NONYUSAKI.SHIP, Def_M_NONYUSAKI.SHIP, true, true);
                }
                else
                {
                    // コンボボックスセット
                    this.SetComboDt((int)SHEET_COL.SHIP, row, null, Def_M_NONYUSAKI.SHIP, Def_M_NONYUSAKI.NONYUSAKI_CD, true, false);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 宛先用コンボボックス設定
        /// </summary>
        /// <param name="row"></param>
        /// <create>K.Tsutsumi 2019/03/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetComboAtesaki(int row)
        {
            try
            {
                // コンボボックスセット
                this.SetComboDt((int)SHEET_COL.ATESAKI, row, this._dtAtesaki, Def_M_SELECT_ITEM.ITEM_NAME, Def_M_SELECT_ITEM.ITEM_NAME, false, true);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 明細用コンボボックスアイテム追加(現在表示しているテキストがリストに存在しない場合)
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <param name="row">行数</param>
        /// <returns>コンボボックス</returns>
        /// <create>D.Naito 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private void AddNoneItemSuperiorComboEditor(Sheet sheet, int row)
        {
            try
            {
                // 納品先
                if (sheet[(int)SHEET_COL.ATESAKI, row].Value == null)
                {
                    SuperiorComboEditor cbo = sheet[(int)SHEET_COL.ATESAKI, row].Editor as SuperiorComboEditor;
                    cbo = AddItemSuperiorComboEditor(cbo, sheet[(int)SHEET_COL.ATESAKI, row].Text);
                    sheet[(int)SHEET_COL.ATESAKI, row].Value = sheet[(int)SHEET_COL.ATESAKI, row].Text;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 明細用コンボボックスアイテム追加
        /// </summary>
        /// <param name="cbo">コンボボックス</param>
        /// <param name="addValue">追加文字列</param>
        /// <returns>コンボボックス</returns>
        /// <create>D.Naito 2018/12/04</create>
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

        /// --------------------------------------------------
        /// <summary>
        /// PLType用コンボボックス設定
        /// </summary>
        /// <param name="row"></param>
        /// <create>K.Tsutsumi 2019/03/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetComboPlType(int row)
        {
            try
            {
                // コンボボックスセット
                this.SetComboDt((int)SHEET_COL.PL_TYPE, row, this._dtPlType, Def_M_COMMON.ITEM_NAME, Def_M_COMMON.VALUE1, false, false);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return;
            }
        }

        #endregion

        #region スキーマ作成

        /// --------------------------------------------------
        /// <summary>
        /// スキーマ作成
        /// </summary>
        /// <returns></returns>
        /// <create>T.Nakata 2018/11/29</create>
        /// <update>H.Tajimi 2020/04/14 出荷元追加</update>
        /// --------------------------------------------------
        private DataTable SetDefaultTable()
        {
            DataTable dt = new DataTable(Def_T_PACKING_MEISAI.Name);
            dt.Columns.Add(Def_M_UNSOKAISHA.KOKUNAI_GAI_FLAG, typeof(string));
            dt.Columns.Add(Def_M_BUKKEN.BUKKEN_NO, typeof(decimal));
            dt.Columns.Add(Def_T_PACKING_MEISAI.CANCEL_FLAG, typeof(string));
            dt.Columns.Add(Def_T_PACKING_MEISAI.DOUKON_FLAG, typeof(string));
            dt.Columns.Add(Def_T_PACKING_MEISAI.SHUKKA_FLAG, typeof(string));
            dt.Columns.Add(Def_T_PACKING_MEISAI.NONYUSAKI_CD, typeof(string));

            dt.Columns.Add(Def_T_PACKING.PACKING_NO, typeof(string));
            dt.Columns.Add(Def_T_PACKING.CT_QTY, typeof(decimal));
            dt.Columns.Add(Def_T_PACKING.INVOICE_NO, typeof(string));
            dt.Columns.Add(Def_T_PACKING.SYUKKA_DATE, typeof(string));
            dt.Columns.Add(Def_T_PACKING.HAKKO_FLAG, typeof(string));
            dt.Columns.Add(Def_T_PACKING.UNSOKAISHA_CD, typeof(string));
            dt.Columns.Add(Def_T_PACKING.CONSIGN_CD, typeof(string));
            dt.Columns.Add(Def_T_PACKING.CONSIGN_ATTN, typeof(string));
            dt.Columns.Add(Def_T_PACKING.DELIVER_CD, typeof(string));
            dt.Columns.Add(Def_T_PACKING.DELIVER_ATTN, typeof(string));
            dt.Columns.Add(Def_T_PACKING.PACKING_MAIL_SUBJECT, typeof(string));
            dt.Columns.Add(Def_T_PACKING.PACKING_REV, typeof(decimal));
            dt.Columns.Add(Def_T_PACKING.VERSION, Type.GetType("System.DateTime"));
            dt.Columns.Add(Def_T_PACKING_MEISAI.NO, typeof(decimal));
            dt.Columns.Add(Def_T_PACKING_MEISAI.CT_NO, typeof(decimal));
            dt.Columns.Add(Def_T_PACKING_MEISAI.FORM_STYLE_FLAG, typeof(string));
            dt.Columns.Add(Def_T_PACKING_MEISAI.SIZE_L, typeof(decimal));
            dt.Columns.Add(Def_T_PACKING_MEISAI.SIZE_W, typeof(decimal));
            dt.Columns.Add(Def_T_PACKING_MEISAI.SIZE_H, typeof(decimal));
            dt.Columns.Add(Def_T_PACKING_MEISAI.GRWT, typeof(decimal));
            dt.Columns.Add(Def_T_PACKING_MEISAI.PRODUCT_NAME, typeof(string));
            dt.Columns.Add(Def_T_PACKING_MEISAI.ATTN, typeof(string));
            dt.Columns.Add(Def_T_PACKING_MEISAI.NOTE, typeof(string));
            dt.Columns.Add(Def_T_PACKING_MEISAI.PL_TYPE, typeof(string));
            dt.Columns.Add(Def_T_PACKING_MEISAI.CASE_NO, typeof(decimal));
            dt.Columns.Add(Def_T_PACKING_MEISAI.BOX_NO, typeof(string));
            dt.Columns.Add(Def_T_PACKING_MEISAI.PALLET_NO, typeof(string));
            dt.Columns.Add(Def_T_PACKING_MEISAI.ECS_NO, typeof(string));
            dt.Columns.Add(Def_T_PACKING_MEISAI.AR_NO, typeof(string));
            dt.Columns.Add(Def_T_PACKING_MEISAI.SEIBAN_CODE, typeof(string));

            dt.Columns.Add(Def_M_NONYUSAKI.SHIP, typeof(string));
            dt.Columns.Add(DB_COL_NAME_KONPONO, typeof(string));
            dt.Columns.Add(Def_T_PACKING.SHIP_FROM_CD, typeof(string));
            return dt;
        }

        #endregion

        #region 同一便への同時反映

        /// --------------------------------------------------
        /// <summary>
        /// 指定行の値を使って、同一便の情報を更新する
        /// </summary>
        /// <param name="srcRow"></param>
        /// <param name="col"></param>
        /// <create>K.Tsutsumi 2019/03/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private void CopyToSameShipmentNumber(int srcRow, int col)
        {

            // 値取得
            var srcAR = this.shtNisugata[(int)SHEET_COL.AR, srcRow].Value == null ? SHUKKA_FLAG.NORMAL_VALUE1 : this.shtNisugata[(int)SHEET_COL.AR, srcRow].Value.ToString();
            var srcProject = this.shtNisugata[(int)SHEET_COL.ITEM, srcRow].Value == null ? string.Empty : this.shtNisugata[(int)SHEET_COL.ITEM, srcRow].Value.ToString();
            var srcShip = this.shtNisugata[(int)SHEET_COL.SHIP, srcRow].Value == null ? string.Empty : this.shtNisugata[(int)SHEET_COL.SHIP, srcRow].Value.ToString();
            int cartonNo = 1;

            for (int row = 0; row < this.shtNisugata.MaxRows - 1; row++)
            {
                var dstAR = this.shtNisugata[(int)SHEET_COL.AR, row].Value == null ? SHUKKA_FLAG.NORMAL_VALUE1 : this.shtNisugata[(int)SHEET_COL.AR, row].Value.ToString();
                var dstProject = this.shtNisugata[(int)SHEET_COL.ITEM, row].Value == null ? string.Empty : this.shtNisugata[(int)SHEET_COL.ITEM, row].Value.ToString();
                var dstShip = this.shtNisugata[(int)SHEET_COL.SHIP, row].Value == null ? string.Empty : this.shtNisugata[(int)SHEET_COL.SHIP, row].Value.ToString();

                if (srcAR == dstAR && srcProject == dstProject && srcShip == dstShip && srcRow != row)
                {
                    // 値を設定
                    switch (col)
                    {
                        case (int)SHEET_COL.KOKUNAI:
                        case (int)SHEET_COL.UNSOKAISYA:
                            this.shtNisugata[(int)SHEET_COL.KOKUNAI, row].Value = this.shtNisugata[(int)SHEET_COL.KOKUNAI, srcRow].Value;
                            bool isCheckedKokunai = false;
                            if (this.shtNisugata[(int)SHEET_COL.KOKUNAI, row].Value != null) isCheckedKokunai = int.Parse(this.shtNisugata[(int)SHEET_COL.KOKUNAI, row].Value.ToString()) == 0 ? false : true;
                            this.shtNisugata_CheckedChanged_Kokunai(row, isCheckedKokunai);
                            this.shtNisugata[(int)SHEET_COL.UNSOKAISYA, row].Value = this.shtNisugata[(int)SHEET_COL.UNSOKAISYA, srcRow].Value;
                            break;
                        case (int)SHEET_COL.CTNO:
                        case (int)SHEET_COL.CTQTY:
                            this.shtNisugata[(int)SHEET_COL.CTQTY, row].Value = this.shtNisugata[(int)SHEET_COL.CTQTY, srcRow].Value;
                            this.shtNisugata[(int)SHEET_COL.CTNO, row].Value = cartonNo++;
                            break;
                        default:
                            this.shtNisugata[col, row].Value = this.shtNisugata[col, srcRow].Value;
                            break;
                    }
                }
                else if (srcAR == dstAR && srcProject == dstProject && srcShip == dstShip && srcRow == row)
                { 
                    // 値を設定
                    switch (col)
                    {
                        case (int)SHEET_COL.CTNO:
                        case (int)SHEET_COL.CTQTY:
                            this.shtNisugata[(int)SHEET_COL.CTNO, row].Value = cartonNo++;
                            break;
                        default:
                            break;
                    }

                }
            }
        }

        #endregion

        #endregion

        #region 検索制御

        #region 検索チェック

        /// --------------------------------------------------
        /// <summary>
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>K.Tsutsumi 2019/03/13</create>
        /// <update>H.Tajimi 2020/04/14 検索条件に出荷元、並び順追加</update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                // 出荷日
                if (this.dtpShukkaDateFrom.IsNull || string.IsNullOrEmpty(this.dtpShukkaDateFrom.Text))
                {
                    // 出荷日を入力してください。
                    this.ShowMessage("S0100050030");
                    return false;
                }
                // 出荷元
                if (this.scboShipFrom.SelectedValue == null || string.IsNullOrEmpty(this.scboShipFrom.Text))
                {
                    // 出荷元を選択してください。
                    this.ShowMessage("S0100050041");
                    return false;
                }
                // 並び順
                if (this.scboSortOrder.Enabled && string.IsNullOrEmpty(this.scboSortOrder.Text))
                {
                    // 並び順を選択してください。
                    this.ShowMessage("S0100050042");
                    return false;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
            return ret;
        }

        #endregion

        #region 検索処理実行部

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理実行部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>K.Tsutsumi 2019/03/13</create>
        /// <update>H.Tajimi 2020/04/14 検索条件に出荷元、並び順追加</update>
        /// <update>2022/05/12 STEP14</update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                var shukkaDate = this.dtpShukkaDateFrom.Text;

                // 荷姿一覧取得
                CondS01 cond = new CondS01(this.UserInfo);
                ConnS01 conn = new ConnS01();

                cond.ShipDate = shukkaDate;
                cond.ShukkaFlag = SHUKKA_FLAG.NORMAL_VALUE1;
                if (this.scboShipFrom.SelectedValue != null && this.scboShipFrom.SelectedValue.ToString() != ComDefine.COMBO_FIRST_VALUE)
                {
                    cond.ShipFromCD = this.scboShipFrom.SelectedValue.ToString();
                }
                if (this.scboSortOrder.Enabled)
                {
                    cond.NisugataSortOrder = this.scboSortOrder.SelectedValue.ToString();
                }
                this._condSearch = cond;
                DataSet ds = conn.S0100050_GetDisp(cond);
                DataTable dtNisugata = null;
                if (this._dtBukken != null) this._dtBukken.Dispose();
                // 物件名一覧
                this._dtBukken = ds.Tables[Def_M_BUKKEN.Name].Copy();
                // 納入先一覧
                this.MakeNonyusakiPerBukken(this._dtBukken, ds.Tables[Def_M_NONYUSAKI.Name]);
                // 表示データ
                if (UtilData.ExistsData(ds, Def_T_PACKING_MEISAI.Name))
                {
                    dtNisugata = ds.Tables[Def_T_PACKING_MEISAI.Name];
                }

                //==================================================
                // 2022/05/12
                // 荷姿入力補助機能修正
                //==================================================

                if (this.rdoInsert.Checked)
                {
                    // ----- 追加/変更 -----
                    if (dtNisugata == null || dtNisugata.Rows.Count == 0)
                    {   
                        dtNisugata = SetDefaultTable();
                    }

                    // 登録候補取得
                    DataTable dtNisugataTemp = this.GetNisugataBaseList(shukkaDate);

                    if (dtNisugataTemp != null)
                    {
                        //dtNisugata = dtNisugataTemp;

                        // 候補件数
                        int count = 0;
                        foreach (DataRow dr in dtNisugataTemp.Rows)
                        {
                            DataRow dr_new = dtNisugata.NewRow();
                            dr_new.ItemArray = dr.ItemArray;

                            // KONPO_NO重複チェック
                            string konpono = dr.Field<string>(DB_COL_NAME_KONPONO);
                            DataRow[] dr_count = dtNisugata.Select(DB_COL_NAME_KONPONO + " = '" + konpono + "'");

                            // KONPO_NOなしデータの重複チェック
                            string bukenno = dr.Field<decimal>(Def_M_NONYUSAKI.BUKKEN_NO).ToString();
                            string ship = dr.Field<string>(Def_M_NONYUSAKI.SHIP);
                            string sqlStr = Def_M_NONYUSAKI.BUKKEN_NO + " = '" + bukenno + "' AND " + Def_M_NONYUSAKI.SHIP + " = '" + ship + "' AND " +
                                "(" + DB_COL_NAME_KONPONO + " IS NULL" + " OR " + DB_COL_NAME_KONPONO + " = '')";
                            DataRow[] dr_count_null = dtNisugata.Select(sqlStr);

                            // 既に登録済みの梱包
                            if (dr_count != null && dr_count.Length > 0)
                            {

                            }
                            else 
                            {
                                if (string.IsNullOrEmpty(konpono))
                                {
                                    if (dr_count_null != null && dr_count_null.Length > 0)
                                    {
                                        continue;
                                    }
                                }
                                // 未登録の梱包
                                // 最終行に追加
                                dtNisugata.Rows.Add(dr_new);
                                count++;
                            }
                            

                        }
                        // 追加する候補があればメッセージ表示
                        if (count > 0)
                        {
                            // 出荷計画から荷姿情報を取得しました。
                            this.ShowMessage("S0100050029");
                        }
                        
                    }

                    //==================================================

                    this._dtSheet = dtNisugata.Copy();

                    // シートに設定するDataTableに"行番号"列を追加
                    dtNisugata.Columns.Add(SHEET_COL_NAME_GYOUNO, typeof(decimal));

                    this.shtNisugata.DataSource = dtNisugata;
                    this.shtNisugata.AllowUserToAddRows = true;

                }
                else
                {
                    // ----- 削除 or 照会 -----
                    // グリッド
                    if (dtNisugata == null) dtNisugata = SetDefaultTable();
                    if (dtNisugata.Rows.Count < 1)
                    {
                        //該当する荷姿情報はありません。
                        this.ShowMessage("S0100050028");
                        return false;
                    }
                    this._dtSheet = dtNisugata.Copy();
                    this.shtNisugata.DataSource = dtNisugata;
                    this.shtNisugata.AllowUserToAddRows = false;
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

        #region 指定出荷日の納入情報一覧取得

        /// --------------------------------------------------
        /// <summary>
        /// 指定出荷日の納入情報一覧取得
        /// </summary>
        /// <param name="ShukkaDate">出荷日</param>
        /// <returns>指定出荷日の納入情報一覧取得</returns>
        /// <create>T.Nakata 2018/11/26</create>
        /// <update>H.Tajimi 2020/04/14 検索条件に出荷元、並び順追加</update>
        /// --------------------------------------------------
        private DataTable GetNisugataBaseList(String ShukkaDate)
        {
            try
            {
                CondS01 cond = new CondS01(this.UserInfo);
                ConnS01 conn = new ConnS01();

                cond.ShukkaFlag = SHUKKA_FLAG.NORMAL_VALUE1;
                cond.ShipDate = ShukkaDate;
                cond.ShipFromCD = this._condSearch.ShipFromCD;
                cond.NisugataSortOrder = this._condSearch.NisugataSortOrder;

                DataSet ds = conn.GetNisugataBaseData(cond);
                if (!ComFunc.IsExistsData(ds, Def_T_PACKING_MEISAI.Name))
                {
                    // 取得NG
                    return null;
                }
                return ds.Tables[Def_T_PACKING_MEISAI.Name].Copy();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return null;
            }
        }

        #endregion

        #endregion

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>T.Nakata 2018/11/29</create>
        /// <update>K.Tsutsumi 2019/03/22 梱包No.は最初からわからない</update>
        /// <update>H.Tajimi 2020/04/21 荷姿表を入力途中で保存可能とする</update>
        /// <update>K.Tsutsumi 2020/04/26 木枠のケースNo.の桁数チェックの不等号修正</update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                if (!this.rdoInsert.Checked) return true;


                DataTable dtSheet = this.shtNisugata.DataSource as DataTable;
                if (dtSheet == null)
                {
                    // 明細が入力されていません。
                    this.ShowMessage("A9999999028");
                    return false;
                }
                if (dtSheet.Rows.Count <= 0)
                {
                    // 明細が入力されていません。
                    this.ShowMessage("A9999999028");
                    return false;
                }

                //===== 入力チェック =====
                for (int i = 0; i < (this.shtNisugata.Rows.Count - 1); i++)
                {
                    string ShukkaFlag = (this.shtNisugata[(int)SHEET_COL.AR, i].Value == null ? SHUKKA_FLAG.NORMAL_VALUE1 : this.shtNisugata[(int)SHEET_COL.AR, i].Value.ToString());
                    ShukkaFlag = (ShukkaFlag == SHUKKA_FLAG.NORMAL_VALUE1 ? SHUKKA_FLAG.NORMAL_VALUE1 : SHUKKA_FLAG.AR_VALUE1);
                    int DoukonFlag = (this.shtNisugata[(int)SHEET_COL.DOUKON, i].Value == null ? 0 : int.Parse(this.shtNisugata[(int)SHEET_COL.DOUKON, i].Value.ToString()));


                    // 行番号を設定
                    this.shtNisugata[(int)SHEET_COL.GYOUNO, i].Value = (i + 1);

                    // チェックボックスNULL処理
                    if (this.shtNisugata[(int)SHEET_COL.CAN, i].Value == null)
                        this.shtNisugata[(int)SHEET_COL.CAN, i].Value = 0;
                    if (this.shtNisugata[(int)SHEET_COL.KOKUNAI, i].Value == null)
                        this.shtNisugata[(int)SHEET_COL.KOKUNAI, i].Value = 0;
                    if (this.shtNisugata[(int)SHEET_COL.AR, i].Value == null)
                        this.shtNisugata[(int)SHEET_COL.AR, i].Value = 0;
                    if (this.shtNisugata[(int)SHEET_COL.DOUKON, i].Value == null)
                        this.shtNisugata[(int)SHEET_COL.DOUKON, i].Value = 0;

                    // 整合確認
                    // 2020/04/21 荷姿表を入力途中で保存可能とする ↓
                    //if (this.shtNisugata[(int)SHEET_COL.UNSOKAISYA, i].Value == null)
                    //{
                    //    // {0}行目の運送会社が入力されていません。
                    //    this.ShowMessage("S0100050001", (i + 1).ToString());
                    //    this.shtNisugata.ActivePosition = new Position((int)SHEET_COL.UNSOKAISYA, i);
                    //    this.shtNisugata.Focus();
                    //    return false;
                    //}
                    //if (this.shtNisugata[(int)SHEET_COL.INVOICE, i].Value == null)
                    //{
                    //    // {0}行目のInvoice No.が入力されていません。
                    //    this.ShowMessage("S0100050002", (i + 1).ToString());
                    //    this.shtNisugata.ActivePosition = new Position((int)SHEET_COL.INVOICE, i);
                    //    this.shtNisugata.Focus();
                    //    return false;
                    //}
                    //if (this.shtNisugata[(int)SHEET_COL.ITEM, i].Value == null)
                    //{
                    //    // {0}行目のItemが入力されていません。
                    //    this.ShowMessage("S0100050003", (i + 1).ToString());
                    //    this.shtNisugata.ActivePosition = new Position((int)SHEET_COL.ITEM, i);
                    //    this.shtNisugata.Focus();
                    //    return false;
                    //}
                    // 2020/04/21 荷姿表を入力途中で保存可能とする ↑
                    if (ShukkaFlag == SHUKKA_FLAG.NORMAL_VALUE1)
                    {
                        // 2020/04/21 荷姿表を入力途中で保存可能とする ↓
                        //if (this.shtNisugata[(int)SHEET_COL.SHIP, i].Value == null)
                        //{
                        //    // {0}行目のShipが入力されていません。
                        //    this.ShowMessage("S0100050004", (i + 1).ToString());
                        //    this.shtNisugata.ActivePosition = new Position((int)SHEET_COL.ITEM, i);
                        //    this.shtNisugata.Focus();
                        //    return false;
                        //}
                        // 2020/04/21 荷姿表を入力途中で保存可能とする ↑
                    }
                    else
                    {
                        //if (this.shtNisugata[(int)SHEET_COL.ECSNO, i].Value == null)
                        //{
                        //    // {0}行目のECS No.が入力されていません。
                        //    this.ShowMessage("S0100050005", (i + 1).ToString());
                        //    this.shtNisugata.ActivePosition = new Position((int)SHEET_COL.ECSNO, i);
                        //    this.shtNisugata.Focus();
                        //    return false;
                        //}
                    }
                    // 2020/04/21 荷姿表を入力途中で保存可能とする ↓
                    //if ((this.shtNisugata[(int)SHEET_COL.ATESAKI, i].Value == null)
                    //    && (string.IsNullOrEmpty(this.shtNisugata[(int)SHEET_COL.ATESAKI, i].Text)))
                    //{
                    //    // {0}行目の宛先が入力されていません。
                    //    this.ShowMessage("S0100050006", (i + 1).ToString());
                    //    this.shtNisugata.ActivePosition = new Position((int)SHEET_COL.ATESAKI, i);
                    //    this.shtNisugata.Focus();
                    //    return false;
                    //}
                    // 2020/04/21 荷姿表を入力途中で保存可能とする ↑
                    if (this.shtNisugata[(int)SHEET_COL.KONPOUNO, i].Value == null)
                    {
                        //// {0}行目の梱包No.が入力されていません。
                        //this.ShowMessage("S0100050007", (i + 1).ToString());
                        //this.shtNisugata.ActivePosition = new Position((int)SHEET_COL.KONPOUNO, i);
                        //this.shtNisugata.Focus();
                        //return false;
                    }
                    else
                    {   // C/Noチェック
                        string konpono = this.shtNisugata[(int)SHEET_COL.KONPOUNO, i].Value.ToString();
                        if (!((konpono.Substring(0, 1) == "P") || (konpono.Substring(0, 1) == "B")))
                        {
                            int konponoVal;
                            if (int.TryParse(konpono, out konponoVal))
                            {
                                if (konponoVal >= 1000)
                                {
                                    // {0}行目の梱包No.の入力値に誤りがあります。(C/No:数値3桁)
                                    this.ShowMessage("S0100050012", (i + 1).ToString());
                                    this.shtNisugata.ActivePosition = new Position((int)SHEET_COL.KONPOUNO, i);
                                    this.shtNisugata.Focus();
                                    return false;
                                }
                            }
                            else
                            {
                                // {0}行目の梱包No.の入力値に誤りがあります。(C/No:数値3桁)
                                this.ShowMessage("S0100050012", (i + 1).ToString());
                                this.shtNisugata.ActivePosition = new Position((int)SHEET_COL.KONPOUNO, i);
                                this.shtNisugata.Focus();
                                return false;
                            }
                        }
                    }
                    if (DoukonFlag == 0)
                    {
                        // 2020/04/21 荷姿表を入力途中で保存可能とする ↓
                        //if (this.shtNisugata[(int)SHEET_COL.CTQTY, i].Value == null)
                        //{
                        //    // {0}行目のCT Qtyが入力されていません。
                        //    this.ShowMessage("S0100050008", (i + 1).ToString());
                        //    this.shtNisugata.ActivePosition = new Position((int)SHEET_COL.CTQTY, i);
                        //    this.shtNisugata.Focus();
                        //    return false;
                        //}
                        //if (this.shtNisugata[(int)SHEET_COL.CTNO, i].Value == null)
                        //{
                        //    // {0}行目のCT No.が入力されていません。
                        //    this.ShowMessage("S0100050009", (i + 1).ToString());
                        //    this.shtNisugata.ActivePosition = new Position((int)SHEET_COL.CTNO, i);
                        //    this.shtNisugata.Focus();
                        //    return false;
                        //}
                        //if (this.shtNisugata[(int)SHEET_COL.TEIKEI, i].Value == null)
                        //{
                        //    // {0}行目の定形が入力されていません。
                        //    this.ShowMessage("S0100050010", (i + 1).ToString());
                        //    this.shtNisugata.ActivePosition = new Position((int)SHEET_COL.TEIKEI, i);
                        //    this.shtNisugata.Focus();
                        //    return false;
                        //}
                        //if (this.shtNisugata[(int)SHEET_COL.NOUNYU_CD, i].Value == null)
                        //{
                        //    // {0}行目のItemもしくはShipが確定していません。
                        //    this.ShowMessage("S0100050011", (i + 1).ToString());
                        //    this.shtNisugata.ActivePosition = new Position((int)SHEET_COL.ITEM, i);
                        //    this.shtNisugata.Focus();
                        //    return false;
                        //}
                        // 2020/04/21 荷姿表を入力途中で保存可能とする ↑
                    }
                }
                // シートのデータをデータソースに反映する。
                this.shtNisugata.UpdateData();

                //===== データ整合確認 =====
                DataTable dtCheck = (this.shtNisugata.DataSource as DataTable).Copy();
                dtCheck.AcceptChanges();

                // 行数が設定されてない行は除外
                DataRow[] rows_del = dtCheck.Select(SHEET_COL_NAME_GYOUNO + " IS NULL");
                if (rows_del != null && rows_del.Length > 0)
                    Array.ForEach<DataRow>(rows_del, row => dtCheck.Rows.Remove(row));
                // キャンセル/同梱は除外
                DataRow[] rows_cancel = dtCheck.Select(Def_T_PACKING_MEISAI.CANCEL_FLAG + "='1' OR " + Def_T_PACKING_MEISAI.DOUKON_FLAG + "='1'");
                if (rows_cancel != null && rows_cancel.Length > 0)
                    Array.ForEach<DataRow>(rows_cancel, row => dtCheck.Rows.Remove(row));


                // CT No.重複チェック & CT No.整合チェック
                DataTable dtDistinctInv = dtCheck.DefaultView.ToTable(true, Def_T_PACKING.UNSOKAISHA_CD, Def_T_PACKING.INVOICE_NO, Def_T_PACKING_MEISAI.CT_NO);
                DataView dvDistinctInv = new DataView(dtDistinctInv);
                dvDistinctInv.Sort = Def_T_PACKING.UNSOKAISHA_CD + "," + Def_T_PACKING.INVOICE_NO + "," + Def_T_PACKING_MEISAI.CT_NO;
                dtDistinctInv = dvDistinctInv.ToTable();
                Dictionary<string, int> dcCheckCTNo = new Dictionary<string, int>();
                foreach (DataRow dr in dtDistinctInv.Rows)
                {
                    // 2020/04/21 荷姿表を入力途中で保存可能とする ↓
                    // 運航会社、Invoice No、CT_NOの全てが入力されているデータを対象とする
                    if (string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_PACKING.UNSOKAISHA_CD))
                     || string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_PACKING.INVOICE_NO))
                     || string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_PACKING_MEISAI.CT_NO)))
                    {
                        continue;
                    }
                    // 2020/04/21 荷姿表を入力途中で保存可能とする ↑

                    //+++++ CT No.重複チェック +++++
                    string tmpStr = Def_T_PACKING.UNSOKAISHA_CD + " = '" + dr[Def_T_PACKING.UNSOKAISHA_CD] + "' AND "
                                   + Def_T_PACKING.INVOICE_NO + " = '" + dr[Def_T_PACKING.INVOICE_NO] + "' AND "
                                   + Def_T_PACKING_MEISAI.CT_NO + " = " + dr[Def_T_PACKING_MEISAI.CT_NO];
                    int Count = (int)dtCheck.Compute("COUNT(" + Def_T_PACKING.INVOICE_NO + ")", tmpStr);
                    if (Count > 1)
                    {
                        DataTable dtMessage = ComFunc.GetSchemeMultiMessage();
                        DataRow[] s_dr = dtCheck.Select(tmpStr, SHEET_COL_NAME_GYOUNO);
                        foreach (DataRow ng_dr in s_dr)
                        {
                            // {0}行目 運送会社:{1} INVOICE:{2} CT No.:{3}
                            string Gyou = ng_dr[SHEET_COL_NAME_GYOUNO].ToString();
                            string Unso = ng_dr[Def_T_PACKING.UNSOKAISHA_CD].ToString(); ;
                            string Inv = ng_dr[Def_T_PACKING.INVOICE_NO].ToString();
                            string CTNo = ng_dr[Def_T_PACKING_MEISAI.CT_NO].ToString();
                            ComFunc.AddMultiMessage(dtMessage, "S0100050014", Gyou, Unso, Inv, CTNo);
                        }
                        // CT Noが重複しています。\r\n下記行数の入力値を確認して下さい。
                        this.ShowMultiMessage(dtMessage, "S0100050013");
                        int row = int.Parse(s_dr[0][SHEET_COL_NAME_GYOUNO].ToString());
                        this.shtNisugata.ActivePosition = new Position((int)SHEET_COL.CTNO, row - 1);
                        this.shtNisugata.Focus();
                        return false;
                    }
                    //+++++ CT No.整合チェック +++++
                    string key = dr[Def_T_PACKING.UNSOKAISHA_CD] + "-" + dr[Def_T_PACKING.INVOICE_NO];
                    if (dcCheckCTNo.ContainsKey(key))
                        dcCheckCTNo[key]++;
                    else
                        dcCheckCTNo[key] = 1;
                    int SrcNum = int.Parse(dr[Def_T_PACKING_MEISAI.CT_NO].ToString());
                    if (SrcNum != dcCheckCTNo[key])
                    {
                        // {0}行目のCT No.に不整値が入力されています。1からの連番で入力して下さい。
                        DataRow[] s_dr = dtCheck.Select(tmpStr);
                        int row = int.Parse(s_dr[0][SHEET_COL_NAME_GYOUNO].ToString());
                        this.ShowMessage("S0100050019", row.ToString());
                        this.shtNisugata.ActivePosition = new Position((int)SHEET_COL.CTNO, row - 1);
                        this.shtNisugata.Focus();
                        return false;
                    }
                }
                // CT Qty不一致チェック
                dtDistinctInv = dtCheck.DefaultView.ToTable(true, Def_T_PACKING.UNSOKAISHA_CD, Def_T_PACKING.INVOICE_NO, Def_T_PACKING.CT_QTY);
                foreach (DataRow dr in dtDistinctInv.Rows)
                {
                    // 2020/04/21 荷姿表を入力途中で保存可能とする ↓
                    // 運航会社、Invoice No、CT Qtyの全てが入力されているデータを対象とする
                    if (string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_PACKING.UNSOKAISHA_CD))
                     || string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_PACKING.INVOICE_NO))
                     || string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_PACKING.CT_QTY)))
                    {
                        continue;
                    }
                    // 2020/04/21 荷姿表を入力途中で保存可能とする ↑

                    //+++++ CT Qty不一致チェック +++++
                    // 2020/04/21 荷姿表を入力途中で保存可能とする ↓
                    // CT Qty入力済のみ対象とする
                    string baseTmpStr = Def_T_PACKING.UNSOKAISHA_CD + " = '" + dr[Def_T_PACKING.UNSOKAISHA_CD] + "' AND "
                                      + Def_T_PACKING.INVOICE_NO + " = '" + dr[Def_T_PACKING.INVOICE_NO] + "'";
                    string tmpStr = baseTmpStr + " AND "
                                  + Def_T_PACKING.CT_QTY + " IS NOT NULL";
                    // 2020/04/21 荷姿表を入力途中で保存可能とする ↑
                    if ((dtDistinctInv.Select(tmpStr)).Length > 1)
                    {
                        DataTable dtMessage = ComFunc.GetSchemeMultiMessage();
                        DataRow[] s_dr = dtCheck.Select(tmpStr, SHEET_COL_NAME_GYOUNO);
                        foreach (DataRow ng_dr in s_dr)
                        {
                            // {0}行目 運送会社:{1} INVOICE:{2} CT Qty:{3}
                            string Gyou = ng_dr[SHEET_COL_NAME_GYOUNO].ToString();
                            string Unso = ng_dr[Def_T_PACKING.UNSOKAISHA_CD].ToString(); ;
                            string Inv = ng_dr[Def_T_PACKING.INVOICE_NO].ToString();
                            string CTQty = ng_dr[Def_T_PACKING.CT_QTY].ToString();
                            ComFunc.AddMultiMessage(dtMessage, "S0100050016", Gyou, Unso, Inv, CTQty);
                        }
                        // CT Qtyが一致していません。\r\n下記行数の入力値を確認して下さい。
                        this.ShowMultiMessage(dtMessage, "S0100050015");
                        int row = int.Parse(s_dr[0][SHEET_COL_NAME_GYOUNO].ToString());
                        this.shtNisugata.ActivePosition = new Position((int)SHEET_COL.CTQTY, row - 1);
                        this.shtNisugata.Focus();
                        return false;
                    }
                    //+++++ CT Qty < CT Noチェック +++++
                    // 2020/04/21 荷姿表を入力途中で保存可能とする ↓
                    // CT No入力済のみ対象とする
                    string tmpStr2 = baseTmpStr + " AND "
                                   + Def_T_PACKING_MEISAI.CT_NO + " IS NOT NULL";
                    var maxCTNo = dtCheck.Compute("MAX(" + Def_T_PACKING_MEISAI.CT_NO + ")", tmpStr2);
                    if (maxCTNo == null || maxCTNo == DBNull.Value)
                    {
                        // 最大CT Noがとれないということは、入力済CT Noがないので以下の処理は続行不可
                        continue;
                    }
                    int CtNoMax = int.Parse(maxCTNo.ToString());
                    // 2020/04/21 荷姿表を入力途中で保存可能とする ↑
                    int CtQty = int.Parse(dr[Def_T_PACKING.CT_QTY].ToString());
                    if (CtNoMax > CtQty)
                    {
                        // Invoice No.{0}のCT QtyにCT No.の最大値未満の値が入力されています。
                        DataRow[] s_dr = dtCheck.Select(baseTmpStr);
                        int row = int.Parse(s_dr[0][SHEET_COL_NAME_GYOUNO].ToString());
                        this.ShowMessage("S0100050027", dr[Def_T_PACKING.INVOICE_NO].ToString());
                        this.shtNisugata.ActivePosition = new Position((int)SHEET_COL.CTQTY, row - 1);
                        this.shtNisugata.Focus();
                        return false;
                    }
                }

                //チェック用テーブル再構築
                dtCheck = dtSheet.Copy();
                dtCheck.AcceptChanges();

                // 行数が設定されてない行は除外
                rows_del = dtCheck.Select(SHEET_COL_NAME_GYOUNO + " IS NULL");
                if (rows_del != null && rows_del.Length > 0)
                    Array.ForEach<DataRow>(rows_del, row => dtCheck.Rows.Remove(row));
                // キャンセルは除外
                rows_cancel = dtCheck.Select(Def_T_PACKING_MEISAI.CANCEL_FLAG + "='1'");
                if (rows_cancel != null && rows_cancel.Length > 0)
                    Array.ForEach<DataRow>(rows_cancel, row => dtCheck.Rows.Remove(row));


                // 同梱チェック
                dtDistinctInv = dtCheck.DefaultView.ToTable(true, Def_T_PACKING.UNSOKAISHA_CD, Def_T_PACKING.INVOICE_NO);
                foreach (DataRow dr in dtDistinctInv.Rows)
                {
                    // 2020/04/21 荷姿表を入力途中で保存可能とする ↓
                    // 運航会社、Invoice Noの全てが入力されているデータを対象とする
                    if (string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_PACKING.UNSOKAISHA_CD))
                     || string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_PACKING.INVOICE_NO)))
                    {
                        continue;
                    }
                    // 2020/04/21 荷姿表を入力途中で保存可能とする ↑

                    //+++++ 同梱チェック +++++
                    string tmpStr = Def_T_PACKING.UNSOKAISHA_CD + " = '" + dr[Def_T_PACKING.UNSOKAISHA_CD] + "' AND "
                                  + Def_T_PACKING.INVOICE_NO + " = '" + dr[Def_T_PACKING.INVOICE_NO] + "' AND "
                                  + Def_T_PACKING_MEISAI.DOUKON_FLAG + " = '0'";
                    int Count = (int)dtCheck.Compute("COUNT(INVOICE_NO)", tmpStr);
                    if (Count == 0)
                    {
                        DataTable dtMessage = ComFunc.GetSchemeMultiMessage();
                        tmpStr = Def_T_PACKING.UNSOKAISHA_CD + " = '" + dr[Def_T_PACKING.UNSOKAISHA_CD] + "' AND "
                               + Def_T_PACKING.INVOICE_NO + " = '" + dr[Def_T_PACKING.INVOICE_NO] + "'";
                        DataRow[] s_dr = dtCheck.Select(tmpStr, SHEET_COL_NAME_GYOUNO);
                        foreach (DataRow ng_dr in s_dr)
                        {
                            // {0}行目 運送会社:{1} INVOICE:{2} 同梱:{3}
                            string Gyou = ng_dr[SHEET_COL_NAME_GYOUNO].ToString();
                            string Unso = ng_dr[Def_T_PACKING.UNSOKAISHA_CD].ToString(); ;
                            string Inv = ng_dr[Def_T_PACKING.INVOICE_NO].ToString();
                            string Dokon = ng_dr[Def_T_PACKING_MEISAI.DOUKON_FLAG].ToString();
                            ComFunc.AddMultiMessage(dtMessage, "S0100050018", Gyou, Unso, Inv, (Dokon == DOUKON_FLAG.OFF_VALUE1 ? DOUKON_FLAG.OFF_NAME : DOUKON_FLAG.ON_NAME));
                        }
                        // 全ての荷姿情報が同梱となっています。\r\n下記行数の入力値を確認して下さい。
                        this.ShowMultiMessage(dtMessage, "S0100050017");
                        int row = int.Parse(s_dr[0][SHEET_COL_NAME_GYOUNO].ToString());
                        this.shtNisugata.ActivePosition = new Position((int)SHEET_COL.DOUKON, row - 1);
                        this.shtNisugata.Focus();
                        return false;
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

        #endregion

        #region 編集内容実行制御処理

        /// --------------------------------------------------
        /// <summary>
        /// 編集内容実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>T.Nakata 2018/11/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            this.ClearMessage();

            this.IsRunEditAfterClear = false;

            bool ret = base.RunEdit();
            try
            {
                if (ret)
                {
                    // グリッドのクリア
                    this.SheetClear();

                    // 検索条件のロック解除
                    this.grpSearch.Enabled = true;
                    // ファンクションボタンの切替
                    this.ChangeFunctionButton(false);
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

        #region 登録/変更処理

        /// --------------------------------------------------
        /// <summary>
        /// 登録/変更処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>T.Nakata 2018/11/29</create>
        /// <update>H.Tajimi 2020/04/14 出荷元追加</update>
        /// <update>H.Tajimi 2020/04/21 荷姿表を入力途中で保存可能とする</update>
        /// <update>2022/05/12 STEP14</update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                //===== シート上でのデータ整理 =====
                for (int i = 0; i < this.shtNisugata.Rows.Count - 1; i++)
                {
                    // キャンセルフラグ
                    if (this.shtNisugata[(int)SHEET_COL.CAN, i].Value == null) this.shtNisugata[(int)SHEET_COL.CAN, i].Value = 0;

                    // AR
                    if (this.shtNisugata[(int)SHEET_COL.AR, i].Value == null) this.shtNisugata[(int)SHEET_COL.AR, i].Value = 0;

                    // 同梱フラグ
                    if (this.shtNisugata[(int)SHEET_COL.DOUKON, i].Value == null)
                    {
                        this.shtNisugata[(int)SHEET_COL.DOUKON, i].Value = 0;
                    }
                    else
                    {
                        int DoukonFlag = int.Parse(this.shtNisugata[(int)SHEET_COL.DOUKON, i].Value.ToString());
                        if (DoukonFlag == 1)
                        {
                            if (i > 0)
                            {   // 同梱時は1行上からデータをコピーする
                                this.shtNisugata[(int)SHEET_COL.CTNO, i].Value = this.shtNisugata[(int)SHEET_COL.CTNO, i - 1].Value;
                                this.shtNisugata[(int)SHEET_COL.KONPOUNO, i].Value = this.shtNisugata[(int)SHEET_COL.KONPOUNO, i - 1].Value;
                                this.shtNisugata[(int)SHEET_COL.PL_TYPE, i].Value = this.shtNisugata[(int)SHEET_COL.PL_TYPE, i - 1].Value;
                                this.shtNisugata[(int)SHEET_COL.TEIKEI, i].Value = this.shtNisugata[(int)SHEET_COL.TEIKEI, i - 1].Value;
                                this.shtNisugata[(int)SHEET_COL.SIZE_L, i].Value = this.shtNisugata[(int)SHEET_COL.SIZE_L, i - 1].Value;
                                this.shtNisugata[(int)SHEET_COL.SIZE_W, i].Value = this.shtNisugata[(int)SHEET_COL.SIZE_W, i - 1].Value;
                                this.shtNisugata[(int)SHEET_COL.SIZE_H, i].Value = this.shtNisugata[(int)SHEET_COL.SIZE_H, i - 1].Value;
                                this.shtNisugata[(int)SHEET_COL.GRWT, i].Value = this.shtNisugata[(int)SHEET_COL.GRWT, i - 1].Value;
                            }
                            else
                            {   // 不整合
                                return false;
                            }
                        }
                    }

                    // 宛先
                    if (this.shtNisugata[(int)SHEET_COL.ATESAKI, i].Value == null)
                    {   // コンボボックスに無い宛先の場合はコンボボックスに追加し設定する
                        string tmp = this.shtNisugata[(int)SHEET_COL.ATESAKI, i].Text;
                        if (!string.IsNullOrEmpty(tmp))
                        {
                            SuperiorComboEditor ObjCmb = this.shtNisugata[(int)SHEET_COL.ATESAKI, i].Editor as SuperiorComboEditor;
                            ObjCmb.Items.Add(new ComboItem(0, null, tmp, "", tmp));
                            this.shtNisugata[(int)SHEET_COL.ATESAKI, i].Value = tmp;
                        }
                    }

                    // 定形
                    if (this.shtNisugata[(int)SHEET_COL.TEIKEI, i].Value != null)
                    {
                        string formStyle = this.shtNisugata[(int)SHEET_COL.TEIKEI, i].Value.ToString();
                        string teikei_size = ComFunc.GetFld(
                                _dtTeikei.AsEnumerable()
                                .FirstOrDefault(w => ComFunc.GetFld(w, Def_M_COMMON.VALUE1, "") == formStyle)
                            , Def_M_COMMON.VALUE2, "");
                        if (teikei_size != null)
                        {
                            string[] sizetmp = teikei_size.Split('x');
                            if (sizetmp.Length == 3)
                            {
                                int sizeL, sizeW, sizeH;
                                if (int.TryParse(sizetmp[0], out sizeL))
                                    this.shtNisugata[(int)SHEET_COL.SIZE_L, i].Value = sizeL;
                                if (int.TryParse(sizetmp[1], out sizeW))
                                    this.shtNisugata[(int)SHEET_COL.SIZE_W, i].Value = sizeW;
                                if (int.TryParse(sizetmp[2], out sizeH))
                                    this.shtNisugata[(int)SHEET_COL.SIZE_H, i].Value = sizeH;
                            }
                        }
                    }

                    //　P/L Type
                    if (this.shtNisugata[(int)SHEET_COL.PL_TYPE, i].Value == null)
                    {
                        //　PL Typeのデフォルト値を取得する
                        var defaultPlTypeRow = _dtPlType.AsEnumerable().FirstOrDefault(w => ComFunc.GetFld(w, Def_M_COMMON.DEFAULT_VALUE, "") == DEFAULT_VALUE.ENABLE_VALUE1);
                        var defaultPlType = ComFunc.GetFld(defaultPlTypeRow, Def_M_COMMON.VALUE1, null);
                        this.shtNisugata[(int)SHEET_COL.PL_TYPE, i].Value = defaultPlType == null ? PL_TYPE.DEFAULT_VALUE1 : defaultPlType;
                    }
                }

                // シートのデータをデータソースに反映する。
                this.shtNisugata.UpdateData();
                //===== データソース上でのデータ整理 =====
                DataTable dt = (this.shtNisugata.DataSource as DataTable).Copy();
                dt.Columns.Remove(SHEET_COL_NAME_GYOUNO);//行番号列を削除
                dt.AcceptChanges();

                Dictionary<string, int> dcPM_No = new Dictionary<string, int>();
                foreach (DataRow dr in dt.Rows)
                {
                    // 基本情報を設定
                    dr[Def_T_PACKING.SYUKKA_DATE] = this.dtpShukkaDateFrom.Text;
                    dr[Def_T_PACKING.SHIP_FROM_CD] = this.scboShipFrom.SelectedValue.ToString();

                    // 梱包Noを設定
                    dr[Def_T_PACKING_MEISAI.CASE_NO] = DBNull.Value;
                    dr[Def_T_PACKING_MEISAI.PALLET_NO] = DBNull.Value;
                    dr[Def_T_PACKING_MEISAI.BOX_NO] = DBNull.Value;
                    string KonpoNo = ComFunc.GetFld(dr, "KONPO_NO");
                    if (!string.IsNullOrEmpty(KonpoNo))
                    {
                        if (KonpoNo.Substring(0, 1) == "P")
                        {
                            dr[Def_T_PACKING_MEISAI.PALLET_NO] = KonpoNo;
                        }
                        else if (KonpoNo.Substring(0, 1) == "B")
                        {
                            dr[Def_T_PACKING_MEISAI.BOX_NO] = KonpoNo;
                        }
                        else
                        {
                            dr[Def_T_PACKING_MEISAI.CASE_NO] = KonpoNo;
                        }
                    }

                    string InvoiceNo = ComFunc.GetFld(dr, Def_T_PACKING.INVOICE_NO);

                    // 連番設定
                    if (!dcPM_No.ContainsKey(InvoiceNo))
                        dcPM_No[InvoiceNo] = 1;
                    else
                        dcPM_No[InvoiceNo]++;
                    dr[Def_T_PACKING_MEISAI.NO] = dcPM_No[InvoiceNo];


                    // 開始時の情報にて荷姿CDの存在確認
                    string SqlStr = string.Empty;
                    if (string.IsNullOrEmpty(InvoiceNo))
                    {
                        SqlStr = Def_T_PACKING.INVOICE_NO + " IS NULL" + " AND " + Def_T_PACKING.PACKING_NO + " IS NOT NULL";
                    }
                    else
                    {
                        SqlStr = Def_T_PACKING.INVOICE_NO + " = '" + InvoiceNo + "' AND " + Def_T_PACKING.PACKING_NO + " IS NOT NULL";
                    }

                    DataRow[] s_dr = _dtSheet.Select(SqlStr);
                    if (s_dr != null && s_dr.Length > 0)
                    {   // 登録済みの荷姿情報を設定する
                        dr[Def_T_PACKING.PACKING_NO] = ComFunc.GetFldObject(s_dr[0], Def_T_PACKING.PACKING_NO, DBNull.Value);
                        dr[Def_T_PACKING.HAKKO_FLAG] = ComFunc.GetFldObject(s_dr[0], Def_T_PACKING.HAKKO_FLAG, DBNull.Value);
                        dr[Def_T_PACKING.CONSIGN_CD] = ComFunc.GetFldObject(s_dr[0], Def_T_PACKING.CONSIGN_CD, DBNull.Value);
                        dr[Def_T_PACKING.CONSIGN_ATTN] = ComFunc.GetFldObject(s_dr[0], Def_T_PACKING.CONSIGN_ATTN, DBNull.Value);
                        dr[Def_T_PACKING.DELIVER_CD] = ComFunc.GetFldObject(s_dr[0], Def_T_PACKING.DELIVER_CD, DBNull.Value);
                        dr[Def_T_PACKING.DELIVER_ATTN] = ComFunc.GetFldObject(s_dr[0], Def_T_PACKING.DELIVER_ATTN, DBNull.Value);
                        dr[Def_T_PACKING.PACKING_MAIL_SUBJECT] = ComFunc.GetFldObject(s_dr[0], Def_T_PACKING.PACKING_MAIL_SUBJECT, DBNull.Value);
                        dr[Def_T_PACKING.PACKING_REV] = ComFunc.GetFldObject(s_dr[0], Def_T_PACKING.PACKING_REV, DBNull.Value);
                        dr[Def_T_PACKING.VERSION] = ComFunc.GetFldObject(s_dr[0], Def_T_PACKING.VERSION, DBNull.Value);
                    }
                    else
                    {   // 初期値を設定する
                        dr[Def_T_PACKING.PACKING_NO] = DBNull.Value;
                        dr[Def_T_PACKING.HAKKO_FLAG] = HAKKO_FLAG.PROC_VALUE1;
                        dr[Def_T_PACKING.CONSIGN_CD] = DBNull.Value;
                        dr[Def_T_PACKING.CONSIGN_ATTN] = DBNull.Value;
                        dr[Def_T_PACKING.DELIVER_CD] = DBNull.Value;
                        dr[Def_T_PACKING.DELIVER_ATTN] = DBNull.Value;
                        dr[Def_T_PACKING.PACKING_MAIL_SUBJECT] = PACKING_MAIL_SUBJECT.DEFAULT_VALUE1;
                        dr[Def_T_PACKING.PACKING_REV] = 0;
                        dr[Def_T_PACKING.VERSION] = DBNull.Value;
                    }
                }

                //===== Invoice単位でデータを纏めなおす =====
                DataSet ds = new DataSet();
                DataTable dtDistinctInvoice = dt.DefaultView.ToTable(true, Def_T_PACKING.INVOICE_NO);
                foreach (DataRow dr in dtDistinctInvoice.Rows)
                {
                    string tgt_invoice = ComFunc.GetFld(dr, Def_T_PACKING.INVOICE_NO);
                    DataTable dt_invoice = this.SetDefaultTable();

                    string SqlStr = string.Empty;
                    if (!string.IsNullOrEmpty(tgt_invoice))
                    {
                        SqlStr = Def_T_PACKING.INVOICE_NO + " = '" + tgt_invoice + "'";
                    }
                    else
                    {
                        SqlStr = Def_T_PACKING.INVOICE_NO + " IS NULL";
                    }
                    DataRow[] s_dr = dt.Select(SqlStr);
                    if (s_dr != null && s_dr.Length > 0)
                    {
                        for (int i = 0; i < s_dr.Length; i++)
                        {
                            DataRow dr_invoice = dt_invoice.NewRow();
                            dr_invoice.ItemArray = s_dr[i].ItemArray;
                            dt_invoice.Rows.Add(dr_invoice);
                        }
                        dt_invoice.TableName = "T_INV_" + tgt_invoice;
                        ds.Tables.Add(dt_invoice.Copy());
                    }
                }

                //===== 削除データを抽出 =====
                DataTable dtDel = new DataTable(ComDefine.DTTBL_DELETE);
                dtDel.Columns.Add(Def_T_PACKING.PACKING_NO, typeof(string));
                if (_dtSheet.Rows.Count > 0)
                {
                    DataTable dtDistinctPNo = _dtSheet.DefaultView.ToTable(true, Def_T_PACKING.PACKING_NO);
                    foreach (DataRow dr in dtDistinctPNo.Rows)
                    {
                        string tgt_PNo = ComFunc.GetFld(dr, Def_T_PACKING.PACKING_NO);
                        //if (string.IsNullOrEmpty(tgt_PNo)) break;

                        //==================================================
                        // 2022/05/12
                        // 荷姿入力補助機能修正する時変更
                        //==================================================
                        if (string.IsNullOrEmpty(tgt_PNo)) continue;
                        //==================================================

                        string SqlStr = Def_T_PACKING.PACKING_NO + " = '" + tgt_PNo + "'";
                        DataRow[] s_dr = dt.Select(SqlStr);
                        if (!(s_dr != null && s_dr.Length > 0))
                        {
                            DataRow dr_del = dtDel.NewRow();
                            dr_del[Def_T_PACKING.PACKING_NO] = tgt_PNo;
                            dtDel.Rows.Add(dr_del);
                        }
                    }
                }
                ds.Tables.Add(dtDel.Copy());

                CondS01 cond = new CondS01(this.UserInfo);
                ConnS01 conn = new ConnS01();
                string errMsgID;
                string[] args;
                cond.ShipDate = this.dtpShukkaDateFrom.Text;
                cond.ShipFromCD = this._condSearch.ShipFromCD;
                if (!conn.UpdNisugata(cond, ds, out errMsgID, out args))
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
        /// <create>T.Nakata 2018/11/29</create>
        /// <update>H.Tajimi 2020/04/14 検索条件に出荷元、並び順追加</update>
        /// --------------------------------------------------
        protected override bool RunEditDelete()
        {
            try
            {
                if (_dtSheet.Rows.Count > 0)
                {
                    DataSet ds = new DataSet();
                    ds.Tables.Add(_dtSheet.Copy());

                    DataTable dtDistinctPNo = _dtSheet.DefaultView.ToTable(true, Def_T_PACKING.PACKING_NO);
                    dtDistinctPNo.TableName = ComDefine.DTTBL_DELETE;
                    ds.Tables.Add(dtDistinctPNo.Copy());

                    CondS01 cond = new CondS01(this.UserInfo);
                    ConnS01 conn = new ConnS01();
                    string errMsgID;
                    string[] args;
                    cond.ShipDate = this.dtpShukkaDateFrom.Text;
                    cond.ShipFromCD = this._condSearch.ShipFromCD;
                    if (!conn.DelNisugata(cond, ds, out errMsgID, out args))
                    {
                        if (!string.IsNullOrEmpty(errMsgID))
                        {
                            this.ShowMessage(errMsgID, args);
                        }
                        return false;
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

        #region メール送信実行

        #region 制御メソッド(荷姿表作成＋メール送信)

        /// --------------------------------------------------
        /// <summary>
        /// メール送信実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>H.Tajimi 2018/12/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RunMail()
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                // 入力チェック
                if (!this.CheckInputMail())
                {
                    // 入力チェックNG
                    return false;
                }

                // 確認メッセージ
                // 画面の内容でメール送信します。\r\nよろしいですか？
                if (!this.ShowMessage("S0100050020").Equals(DialogResult.OK))
                {
                    return false;
                }

                // メール送信実行
                Cursor.Current = Cursors.WaitCursor;
                if (this.ExportExcel(true, ComDefine.PACKING_EXCEL_OUTPUT_DIR, ComDefine.EXCEL_FILE_PACKING))
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

        #region 制御メソッド(荷姿表作成のみ)

        /// --------------------------------------------------
        /// <summary>
        /// 荷姿表作成のみ実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>K.Tsutsumi 2019/03/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RunExcelOnly()
        {
            Cursor preCursor = Cursor.Current;
            try
            {                
                using (SaveFileDialog frm = new SaveFileDialog())
                {
                    frm.Title = Resources.NisugataHakko_sfdExcel_Title;
                    frm.Filter = Resources.NisugataHakko_sfdExcel_Filter;
                    frm.FileName = ComDefine.EXCEL_FILE_PACKING;
                    if (0 < this.shtNisugata.MaxRows && frm.ShowDialog() != DialogResult.OK) return false;

                    // Export Excel file
                    Cursor.Current = Cursors.WaitCursor;
                    var path = Path.GetDirectoryName(frm.FileName);
                    var fileName = Path.GetFileName(frm.FileName);
                    if (this.ExportExcel(false, path, fileName))
                    {
                        // 荷姿表ExcelFilesを出力しました。
                        this.ShowMessage("S0100050040");
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

        #region メール送信用入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// メール送信用入力チェック
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/12/03</create>
        /// <update>M.Shimizu 2020/07/02 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）</update>
        /// --------------------------------------------------
        private bool CheckInputMail()
        {
            bool ret = false;
            try
            {
                // メールタイトルチェック
                if (this.cboMailTitle.SelectedIndex < 0)
                {
                    // Mail Titleを選択して下さい。
                    this.ShowMessage("S0100050021");
                    this.cboMailTitle.Focus();
                    return false;
                }

                // Revチェック
                if (string.IsNullOrEmpty(this.txtRev.Text))
                {
                    // Revを入力して下さい。
                    this.ShowMessage("S0100050022");
                    this.txtRev.Focus();
                    return false;
                }

                // メールアドレスチェック
                var conn = new ConnCommon();
                var cond = new CondCommon(this.UserInfo);
                var ds = conn.CheckPackingMail(cond);
                if (string.IsNullOrEmpty(UtilData.GetFld(ds, Def_M_USER.Name, 0, Def_M_USER.MAIL_ADDRESS)))
                {
                    // 担当者にMailAddressが設定されていません。
                    this.ShowMessage("A0100010010");
                    return false;
                }

                if (!UtilData.ExistsData(ds, ComDefine.DT_PACKING_M_USER))
                {
                    // 荷姿表メール送信対象者が設定されていません。
                    this.ShowMessage("S0100050023");
                    return false;
                }

                this._dtPackingUser = ds.Tables[ComDefine.DT_PACKING_M_USER];
                this._mailAddress = UtilData.GetFld(ds, Def_M_USER.Name, 0, Def_M_USER.MAIL_ADDRESS);
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

        #region Excel出力

        /// --------------------------------------------------
        /// <summary>
        /// Excel出力
        /// </summary>
        /// <param name="isMail">true:Excel+Mail, false:Excel only</param>
        /// <param name="path">保存フォルダ</param>
        /// <param name="fileName">保存ファイル名</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/12/10</create>
        /// <update>K.Tsutsumi 2019/03/23 Excel出力のみに対応</update>
        /// <update>H.Tajimi 2020/04/14 検索条件に出荷元、並び順追加</update>
        /// --------------------------------------------------
        private bool ExportExcel(bool isMail, string path, string fileName)
        {
            bool ret = false;
            string mailID = string.Empty;
            var filePath = Path.Combine(path, fileName);
            try
            {
                // Excelファイルダウンロード
                if (!this.ExcelTemplateFileDownload())
                {
                    // テンプレートファイルのダウンロードに失敗しました。
                    this.ShowMessage("S0100050024");
                    return false;
                }

                var conn = new ConnS01();
                var cond = new CondS01(this.UserInfo);
                cond.UpdateUserID = this.UserInfo.UserID;
                cond.UpdateUserName = this.UserInfo.UserName;
                cond.ShipDate = this.dtpShukkaDateFrom.Text;
                cond.ShipFromCD = this._condSearch.ShipFromCD;
                cond.NisugataSortOrder = this._condSearch.NisugataSortOrder;

                string errMsgID;
                string[] args;

                // Excel出力データ取得
                var dsExcel = conn.GetNisugataExcelData(cond);
                if (!UtilData.ExistsData(dsExcel, Def_T_PACKING.Name))
                {
                    // 出力するDataがありません。
                    this.ShowMessage("A0100010004");
                    return false;
                }

                var dtPacking = dsExcel.Tables[Def_T_PACKING.Name];
                for (int rowIndex = 0; rowIndex < dtPacking.Rows.Count; rowIndex++)
                {
                    UtilData.SetFld(dtPacking, rowIndex, ComDefine.FLD_ROW_INDEX, rowIndex + 1);
                    UtilData.SetFld(dtPacking, rowIndex, Def_T_PACKING.PACKING_MAIL_SUBJECT, this.cboMailTitle.SelectedValue.ToString());
                    UtilData.SetFld(dtPacking, rowIndex, Def_T_PACKING.PACKING_REV, this.txtRev.Text);
                }

                // Excel出力
                var export = new ExportPacking();
                export.ExportExcel(filePath, dtPacking, out errMsgID, out args);
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
                if (!this.PackingFileUpload(filePath, fileName, mailID))
                {
                    // アップロードに失敗しました。
                    this.ShowMessage("S0100050025");
                    return false;
                }

                // メールデータ作成
                var ds = new DataSet();
                ds.Merge(this.CreateMailData(mailID, fileName));
                ds.Merge(dtPacking);

                // DB更新
                if (!conn.UpdPackingForExcelData(cond, ds, out errMsgID, out args))
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
                    this.PackingFileDelete(fileName, mailID);
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
        /// <create>H.Tajimi 2018/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ExcelTemplateFileDownload()
        {
            bool ret = false;
            try
            {
                var conn = new ConnAttachFile();
                var dlPackage = new FileDownloadPackage();

                dlPackage.FileName = ComDefine.EXCEL_FILE_PACKING_TEMPLATE;
                dlPackage.FileType = FileType.Template;

                // TemplateファイルのDL
                var retFile = conn.FileDownload(dlPackage);
                if (retFile.IsExistsFile)
                {
                    if (!Directory.Exists(ComDefine.DOWNLOAD_DIR))
                    {
                        Directory.CreateDirectory(ComDefine.DOWNLOAD_DIR);
                    }
                    using (var fs = new FileStream(Path.Combine(ComDefine.DOWNLOAD_DIR, ComDefine.EXCEL_FILE_PACKING_TEMPLATE), FileMode.Create, FileAccess.Write))
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
        /// <create>H.Tajimi 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool PackingFileUpload(string filePath, string fileName, string mailID)
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

        #region ファイル削除

        /// --------------------------------------------------
        /// <summary>
        /// ファイル削除
        /// </summary>
        /// <param name="fileName">アップロード先ファイル名</param>
        /// <param name="mailID">メールID</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool PackingFileDelete(string fileName, string mailID)
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

        #region メール登録用データ作成

        /// --------------------------------------------------
        /// <summary>
        /// メール登録用データ作成
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/12/04</create>
        /// <update>M.Shimizu 2020/06/01 EFA_SMS-91 メール通知のWebService化</update>
        /// <update>M.Shimizu 2020/07/02 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）</update>
        /// --------------------------------------------------
        private DataTable CreateMailData(string mailID, string fileName)
        {
            // ↓↓↓ M.Shimizu 2020/06/01 EFA_SMS-91 メール通知のWebService化
            // メールテンプレートの内容を取得
            ConnAttachFile attachFile = new ConnAttachFile(this.UserInfo.Language);
            string title = attachFile.GetMailTemplate(MAIL_FILE.PACKING_LIST_TITLE_VALUE1);
            string naiyo = attachFile.GetMailTemplate(MAIL_FILE.PACKING_LIST_VALUE1);
            // ↑↑↑ M.Shimizu 2020/06/01 EFA_SMS-91 メール通知のWebService化

            var dt = ComFunc.GetSchemeMail();
            var dr = dt.NewRow();
            dr.SetField<object>(Def_T_MAIL.MAIL_ID, mailID);
            dr.SetField<object>(Def_T_MAIL.MAIL_SEND, this._mailAddress);
            dr.SetField<object>(Def_T_MAIL.MAIL_SEND_DISPLAY, this.UserInfo.UserName);
            dr.SetField<object>(Def_T_MAIL.MAIL_TO, ComFunc.GetMailUser(this._dtPackingUser, Def_M_USER.MAIL_ADDRESS));
            dr.SetField<object>(Def_T_MAIL.MAIL_TO_DISPLAY, ComFunc.GetMailUser(this._dtPackingUser, Def_M_USER.USER_NAME));
            dr.SetField<object>(Def_T_MAIL.MAIL_CC, DBNull.Value);
            dr.SetField<object>(Def_T_MAIL.MAIL_CC_DISPLAY, DBNull.Value);
            dr.SetField<object>(Def_T_MAIL.MAIL_BCC, DBNull.Value);
            dr.SetField<object>(Def_T_MAIL.MAIL_BCC_DISPLAY, DBNull.Value);
            // ↓↓↓ M.Shimizu 2020/06/01 EFA_SMS-91 メール通知のWebService化
            dr.SetField<object>(Def_T_MAIL.TITLE, this.ReplaceMailContents(title));
            dr.SetField<object>(Def_T_MAIL.NAIYO, this.ReplaceMailContents(naiyo));
            // ↑↑↑ M.Shimizu 2020/06/01 EFA_SMS-91 メール通知のWebService化
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
        /// <create>H.Tajimi 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private string ReplaceMailContents(string mailContents)
        {
            return mailContents
                .Replace(MAIL_RESERVE.SHUKKA_DATE_VALUE1, this.dtpShukkaDateFrom.Text)
                .Replace(MAIL_RESERVE.REVISION_VALUE1, this.txtRev.Text)
                .Replace(MAIL_RESERVE.MAIL_TITLE_VALUE1, this.cboMailTitle.Text)
                .Replace(MAIL_RESERVE.SENDER_VALUE1, this.UserInfo.UserName);
        }

        #endregion

        #endregion

        #region 納入先(本体)を物件単位に管理

        /// --------------------------------------------------
        /// <summary>
        /// 納入先(本体)を物件単位に管理する
        /// </summary>
        /// <param name="dtBukken">物件一覧</param>
        /// <param name="dtNonyusaki">納入先一覧</param>
        /// <create>K.Tsutsumi 2019/03/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private void MakeNonyusakiPerBukken(DataTable dtBukken, DataTable dtNonyusaki)
        {
            // クリア
            this.dcNonyusaki.Clear();

            foreach (DataRow drBukken in dtBukken.Rows)
            {
                var key = UtilData.GetFld(drBukken, Def_M_NONYUSAKI.BUKKEN_NO);
                var filterExpression = Def_M_NONYUSAKI.BUKKEN_NO + " = " + key;
                var sort = Def_M_NONYUSAKI.SHIP;
                var drs = dtNonyusaki.Select(filterExpression, sort);
                var dt = dtNonyusaki.Clone();
                foreach (DataRow dr in drs)
                {
                    dt.Rows.Add(dr.ItemArray);
                }
                this.dcNonyusaki.Add(key, dt);
            }

        }

        #endregion

        #region 納入先CD設定

        /// --------------------------------------------------
        /// <summary>
        /// SHIP確定により、納入先CDが確定する場合
        /// </summary>
        /// <param name="row">対象行</param>
        /// <param name="bukkenNo">選択された物件No</param>
        /// <param name="ship">選択された便</param>
        /// <create>K.Tsutsumi 2019/03/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetNonyusakiCd_Regular(int row, int bukkenNo, string ship)
        {
            var key = bukkenNo.ToString();
            var filterExpression = Def_M_NONYUSAKI.SHIP + " = '" + ship + "'";
            var drs = this.dcNonyusaki[key].Select(filterExpression);
            if (drs.Length == 0)
            {
                // 存在しない場合
                this.shtNisugata[(int)SHEET_COL.NOUNYU_CD, row].Value = null;
                System.Diagnostics.Debug.WriteLine("納入先未確定");
            }
            else
            {
                // 存在した場合
                var nonyusakiCd = UtilData.GetFld(drs[0], Def_M_NONYUSAKI.NONYUSAKI_CD);
                this.shtNisugata[(int)SHEET_COL.NOUNYU_CD, row].Value = nonyusakiCd;
                System.Diagnostics.Debug.WriteLine("納入先確定:" + nonyusakiCd);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// AR用物件確定により、納入先CDが確定する場合
        /// </summary>
        /// <param name="row">対象行</param>
        /// <param name="bukkenNo">選択された物件</param>
        /// <create>K.Tsutsumi 2019/03/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetNonyusakiCd_AR(int row, int bukkenNo)
        {
            var filterExpression = Def_M_NONYUSAKI.BUKKEN_NO + " = " + bukkenNo.ToString();
            var drs = this._dtBukkenAR.Select(filterExpression);
            if (drs.Length == 0)
            {
                // 存在しない場合
                this.shtNisugata[(int)SHEET_COL.NOUNYU_CD, row].Value = null;
                System.Diagnostics.Debug.WriteLine("納入先未確定");
            }
            else
            {
                // 存在した場合
                var nonyusakiCd = UtilData.GetFld(drs[0], Def_M_NONYUSAKI.NONYUSAKI_CD);
                this.shtNisugata[(int)SHEET_COL.NOUNYU_CD, row].Value = nonyusakiCd;
                System.Diagnostics.Debug.WriteLine("納入先確定:" + nonyusakiCd);
            }
        }

        #endregion

        #region 定形サイズ指定かどうかを調べる

        /// --------------------------------------------------
        /// <summary>
        /// 定形サイズ指定かどうかを調べる
        /// </summary>
        /// <param name="value">選択されている梱包材</param>
        /// <returns>true:定形 false:定形外</returns>
        /// <create>K.Tsutsumi 2019/03/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool IsStandardSize(object value)
        {
            int sizeL;
            int sizeW;
            int sizeH;

            return IsStandardSize(value, out sizeL, out sizeW, out sizeH);

        }

        /// --------------------------------------------------
        /// <summary>
        /// 定形サイズ指定かどうかを調べる
        /// </summary>
        /// <param name="value">選択されている梱包材</param>
        /// <param name="sizeL">定型サイズのLength</param>
        /// <param name="sizeW">定型サイズのWidth</param>
        /// <param name="sizeH">定型サイズのHeight</param>
        /// <returns>true:定形 false:定形外</returns>
        /// <create>K.Tsutsumi 2019/03/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool IsStandardSize(object value, out int sizeL, out int sizeW, out int sizeH)
        {
            sizeL = 0;
            sizeW = 0;
            sizeH = 0;

            // 定形ではないと判断
            if (value == null) return false;

            //定形のサイズを取得する
            var item = _dtTeikei.AsEnumerable().FirstOrDefault(w => ComFunc.GetFld(w, Def_M_COMMON.VALUE1) == value.ToString());
            var size = ComFunc.GetFld(item, Def_M_COMMON.VALUE2, "").Split('x');

            // 定形ではないと判断
            if (size.Length < 3) return false;
                
            // 定型サイズを表示
            sizeL = UtilConvert.ToInt32(size[0]);
            sizeW = UtilConvert.ToInt32(size[1]);
            sizeH = UtilConvert.ToInt32(size[2]);

            return true;
        }

        #endregion

        #region 出荷元コンボボックス設定

        /// --------------------------------------------------
        /// <summary>
        /// 出荷元コンボボックス設定
        /// </summary>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetComboShipFrom()
        {
            try
            {
                if (this.rdoDelete.Checked || this.rdoView.Checked)
                {
                    this.MakeCmbBox(this.scboShipFrom, this._dtShipFromContainsAll, Def_M_SHIP_FROM.SHIP_FROM_NO, Def_M_SHIP_FROM.SHIP_FROM_NAME, ComDefine.COMBO_FIRST_VALUE, false);
                }
                else
                {
                    this.MakeCmbBox(this.scboShipFrom, this._dtShipFrom, Def_M_SHIP_FROM.SHIP_FROM_NO, Def_M_SHIP_FROM.SHIP_FROM_NAME, string.Empty, false);
                }

                if (UtilData.ExistsData(this.scboShipFrom.DataSource as DataTable))
                {
                    this.scboShipFrom.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 並び順クリア

        /// --------------------------------------------------
        /// <summary>
        /// 並び順クリア
        /// </summary>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ClearSortOrder()
        {
            try
            {
                if (this.rdoInsert.Checked || this.rdoDelete.Checked)
                {
                    this.scboSortOrder.SelectedIndex = -1;
                    this.scboSortOrder.SelectedIndex = -1;
                }
                else
                {
                    this.scboSortOrder.SelectedValue = NISUGATA_HAKKO_SORT.DEFAULT_VALUE1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion
    }
}