using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Commons;
using ElTabelleHelper;
using GrapeCity.Win.ElTabelle;
using GrapeCity.Win.ElTabelle.Editors;
using MultiRowTabelle;
using SMS.E01;
using SMS.P02.Forms;
using SystemBase.Util;
using WsConnection;
using WsConnection.WebRefA01;
using WsConnection.WebRefCommon;
using DSWUtil;
using SMS.A01.Properties;
using System.Collections.Generic;

namespace SMS.A01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// AR情報登録
    /// </summary>
    /// <create>M.Tsutsumi 2010/08/10</create>
    /// <update>M.Tsutsumi 2011/02/16</update>
    /// --------------------------------------------------
    public partial class ARJoho : SystemBase.Forms.CustomOrderForm
    {

        #region 定義

        /// --------------------------------------------------
        /// <summary>
        /// MultiRowの１行の行数
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_ROW_BLOCK = 2;

        /// --------------------------------------------------
        /// <summary>
        /// 編集ボタン
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_DETAIL_BUTTON = 0;

        /// --------------------------------------------------
        /// <summary>
        /// 照会ボタン
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_REF_BUTTON = 1;

        /// --------------------------------------------------
        /// <summary>
        /// 元ARNo
        /// </summary>
        /// <create>D.Okumura 2019/12/16 STEP12 AR7000番運用対応</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_MOTO_ARNO = 7;

        /// --------------------------------------------------
        /// <summary>
        /// 結果ARNo
        /// </summary>
        /// <create>D.Okumura 2019/12/16 STEP12 AR7000番運用対応</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_KEKKA_ARNO = 8;

        /// --------------------------------------------------
        /// <summary>
        /// 結果ARボタン
        /// </summary>
        /// <create>D.Okumura 2019/12/16 STEP12 AR7000番運用対応</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_KEKKA_BUTTON = 9;

        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名のフィールド名
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string FLD_LIST_FLAG_NAME = "LIST_FLAG_NAME";

        /// --------------------------------------------------
        /// <summary>
        /// 状況区分名のフィールド名
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string FLD_JYOKYO_FLAG_NAME = "JYOKYO_FLAG_NAME";

        /// --------------------------------------------------
        /// <summary>
        /// 不具合内容のフィールド名(文字数制限なし)
        /// </summary>
        /// <create>M.Tsutsumi 2010/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string FLD_HUGUAI_ALL = "HUGUAI_ALL";

        /// --------------------------------------------------
        /// <summary>
        /// 対策内容のフィールド名(文字数制限なし)
        /// </summary>
        /// <create>M.Tsutsumi 2010/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string FLD_TAISAKU_ALL = "TAISAKU_ALL";

        /// --------------------------------------------------
        /// <summary>
        /// 備考のフィールド名(文字数制限なし)
        /// </summary>
        /// <create>M.Tsutsumi 2010/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string FLD_BIKO_ALL = "BIKO_ALL";

        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>Y.Higuchi 2010/09/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 5;

        #endregion

        #region Enum

        private enum ElTabelleColumnType
        {
            Text = 0,
            Number = 1,
            Button = 2
        }

        #endregion

        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// まるちろうのフォーマット
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private MultiRowFormat _multiRowFormat = null;

        /// --------------------------------------------------
        /// <summary>
        /// 納入先の管理区分が未完の時はfalse、完了の時はtrue
        /// 処理を分けるのに使用する
        /// </summary>
        /// <create>M.Tsutsumi 2010/09/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _nonyusakiComplete = false;

        //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36

        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名０(デフォルト)
        /// </summary>
        /// <create>M.Tsutsumi 2011/02/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _listFlagName0 = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名１(デフォルト)
        /// </summary>
        /// <create>M.Tsutsumi 2011/02/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _listFlagName1 = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名２(デフォルト)
        /// </summary>
        /// <create>M.Tsutsumi 2011/02/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _listFlagName2 = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名３(デフォルト)
        /// </summary>
        /// <create>M.Tsutsumi 2011/02/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _listFlagName3 = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名４(デフォルト)
        /// </summary>
        /// <create>M.Tsutsumi 2011/02/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _listFlagName4 = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名５(デフォルト)
        /// </summary>
        /// <create>M.Tsutsumi 2011/02/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _listFlagName5 = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名６(デフォルト)
        /// </summary>
        /// <create>M.Tsutsumi 2011/02/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _listFlagName6 = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名７(デフォルト)
        /// </summary>
        /// <create>M.Tsutsumi 2011/02/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _listFlagName7 = string.Empty;

        // @@@ ↑

        /// --------------------------------------------------
        /// <summary>
        /// 物件管理No
        /// </summary>
        /// <create>T.Sakiori 2012/04/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _bukkenNo = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 表示中のモードレスダイアログの数
        /// </summary>
        /// <create>T.Sakiori 2012/04/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private int _modelessNum = 0;
        /// --------------------------------------------------
        /// <summary>
        /// AR対応費用取得データ
        /// </summary>
        /// <create>H.Tajimi 2018/10/16</create>
        /// <update></update>
        /// --------------------------------------------------
        DataTable _dtARCost = null;

        /// --------------------------------------------------
        /// <summary>
        /// 進捗管理のインスタンス
        /// </summary>
        /// <create>Y.Nakasato 2019/07/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private ShinchokuKanri _instShinchokuKanri = null;

        /// --------------------------------------------------
        /// <summary>
        /// リスト区分結果の一覧
        /// </summary>
        /// <create>D.Okumura 2019/12/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private Dictionary<string, DataRow> _listFlagKekka;


        /// --------------------------------------------------
        /// <summary>
        /// リスト区分通知の一覧
        /// </summary>
        /// <create>D.Okumura 2020/01/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private Dictionary<string, DataRow> _listFlagNotify;
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
        /// <create>M.Tsutsumi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public ARJoho(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
            : base(userInfo, menuCategoryID, menuItemID, title)
        {
            InitializeComponent();
        }

        #endregion

        #region 初期化

        #region InitializeLoadControl

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/10</create>
        /// <update>D.Okumura 2019/06/18 添付ファイル追加対応(技連4-5)</update>
        /// <update>T.Nukaga 2019/11/20 AR7000番運用対応</update>
        /// <update>D.Okumura 2020/01/23 AR7000番運用明細登録警告対応</update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // ここにコントロールの初期化を記述する。
                // ベースでDisplayClearの呼出しは行われています。

                // 登録処理のメッセージを変更する場合はセット
                // 登録確認メッセージ
                //this.MsgInsertConfirm = "";
                // 登録完了メッセージ
                //this.MsgInsertEnd = "";

                // 修正処理のメッセージを変更する場合はセット
                // 修正確認メッセージ
                //this.MsgUpdateConfirm = "";
                // 修正完了メッセージ
                //this.MsgUpdateEnd = "";

                // 削除処理のメッセージを変更する場合はセット
                // 削除確認メッセージ
                //this.MsgDeleteConfirm = "";
                // 削除完了メッセージ
                //this.MsgDeleteEnd = "";

                // フォームの設定
                this.IsCloseQuestion = true;

                // 処理モードの初期化
                InitializeMode();

                // シートの初期化
                this.InitializeSheet(shtMeisai);
                shtMeisai.EditType = EditType.Default;
                shtMeisai.AllowUserToAddRows = false;
                shtMeisai.MultiRowAllowUserToAddRows = false;

                // シートのタイトルを設定
                int index = 2;
                shtMeisai.ColumnHeaders[index++, 0].Caption = Resources.ARJoho_ARNo;
                shtMeisai.ColumnHeaders[index++, 0].Caption = Resources.ARJoho_Situation;
                shtMeisai.ColumnHeaders[index, 0].Caption = Resources.ARJoho_ModifiedDate;
                shtMeisai.ColumnHeaders[index++, 1].Caption = Resources.ARJoho_ModifiedBy;
                shtMeisai.ColumnHeaders[index, 0].Caption = Resources.ARJoho_OccurrenceDate;
                shtMeisai.ColumnHeaders[index++, 1].Caption = Resources.ARJoho_ContactedBy;
                shtMeisai.ColumnHeaders[index, 0].Caption = Resources.ARJoho_Model;
                shtMeisai.ColumnHeaders[index++, 1].Caption = Resources.ARJoho_Unit;

                shtMeisai.ColumnHeaders[index++, 0].Caption = Resources.ARJoho_MotoARNo;
                shtMeisai.ColumnHeaders[index++, 0].Caption = Resources.ARJoho_KekkaARNo;
                shtMeisai.ColumnHeaders[index++, 0].Caption = "";

                shtMeisai.ColumnHeaders[index++, 0].Caption = Resources.ARJoho_DefectContent;
                shtMeisai.ColumnHeaders[index++, 0].Caption = Resources.ARJoho_Countermeasures;
                shtMeisai.ColumnHeaders[index++, 0].Caption = Resources.ARJoho_DesiredArrivalDate;
                shtMeisai.ColumnHeaders[index++, 0].Caption = Resources.ARJoho_ProcessingSection;
                shtMeisai.ColumnHeaders[index++, 0].Caption = Resources.ARJoho_EngineeringCommunicationNo;
                index++; //技連(2列目)
                index++; //技連(3列目)

                shtMeisai.ColumnHeaders[index, 0].Caption = Resources.ARJoho_Local;
                shtMeisai.ColumnHeaders[index++, 1].Caption = Resources.ARJoho_ArrangeDestination;
                shtMeisai.ColumnHeaders[index++, 1].Caption = Resources.ARJoho_SettingDeliveryTime;
                shtMeisai.ColumnHeaders[index++, 1].Caption = Resources.ARJoho_ShippingDate;
                shtMeisai.ColumnHeaders[index++, 1].Caption = Resources.ARJoho_FactoryShipmentDate;
                shtMeisai.ColumnHeaders[index++, 1].Caption = Resources.ARJoho_ShippingMethod;

                shtMeisai.ColumnHeaders[index, 0].Caption = Resources.ARJoho_Japan;
                shtMeisai.ColumnHeaders[index++, 1].Caption = Resources.ARJoho_SettingDeliveryTime;
                shtMeisai.ColumnHeaders[index++, 1].Caption = Resources.ARJoho_ShippingDate;
                shtMeisai.ColumnHeaders[index++, 1].Caption = Resources.ARJoho_FactoryShipmentDate;
                shtMeisai.ColumnHeaders[index++, 1].Caption = Resources.ARJoho_ShippingCompany;
                shtMeisai.ColumnHeaders[index++, 1].Caption = Resources.ARJoho_InvoiceNo;

                shtMeisai.ColumnHeaders[index++, 0].Caption = Resources.ARJoho_GMSIssueNo;
                shtMeisai.ColumnHeaders[index++, 0].Caption = Resources.ARJoho_SpecificationsContact;
                shtMeisai.ColumnHeaders[index++, 0].Caption = Resources.ARJoho_Reamrks;

                // テキストの初期化
                this.InitializeText();

                // コンボボックスの初期化
                this.InitializeCombo();

                // ラジオボタンの初期化
                this.InitializeRadioButton();

                // AR関連付け初期データ取得
                this._listFlagKekka = this.GetCommon(LIST_FLAG_KEKKA.GROUPCD, Def_M_COMMON.VALUE1, row => row);
                this._listFlagNotify = this.GetCommon(LIST_FLAG_NOTICE_NEW.GROUPCD, Def_M_COMMON.VALUE1, row => row);

                // モード切替
                this.ChangeMode();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

        }

        #endregion

        #region InitializeShownControl

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示されるたびにコントロールの初期化するメソッド
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // ここに初期フォーカスの設定を記述する。
                // フォーカス設定
                this.rdoUpdate.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region Sheet初期化

        /// --------------------------------------------------
        /// <summary>
        /// Sheet初期化
        /// </summary>
        /// <param name="sheet"></param>
        /// <create>M.Tsutsumi 2010/08/10</create>
        /// <update>D.Okumura 2019/06/18 添付ファイル追加対応(技連4-5)</update>
        /// <update>T.Nukaga 2019/11/20 AR7000番運用対応</update>
        /// --------------------------------------------------
        protected override void InitializeSheet(Sheet sheet)
        {
            base.InitializeSheet(sheet);

            try
            {
                // MultiRowのフォーマット作成
                this.SetMultiRowFormat(true);

                Color orangeIro = Color.Tomato;
                Color genchiIro = Color.YellowGreen;
                Color japanIro = Color.LightPink;
                Color foreColor = Color.Blue;
                int index = 13;
                ElTabelleSheetHelper.SetColumnHeaderColor(sheet, orangeIro, Color.White, index++);
                ElTabelleSheetHelper.SetColumnHeaderColor(sheet, orangeIro, Color.White, index++);
                ElTabelleSheetHelper.SetColumnHeaderColor(sheet, orangeIro, Color.White, index++);
                ElTabelleSheetHelper.SetColumnHeaderColor(sheet, orangeIro, Color.White, index++);
                ElTabelleSheetHelper.SetColumnHeaderColor(sheet, genchiIro, foreColor, index, 0);
                ElTabelleSheetHelper.SetColumnHeaderColor(sheet, orangeIro, Color.White, index++, 1);
                ElTabelleSheetHelper.SetColumnHeaderColor(sheet, genchiIro, foreColor, index, 0);
                ElTabelleSheetHelper.SetColumnHeaderColor(sheet, orangeIro, Color.White, index++, 1);
                ElTabelleSheetHelper.SetColumnHeaderColor(sheet, genchiIro, foreColor, index, 0);
                ElTabelleSheetHelper.SetColumnHeaderColor(sheet, orangeIro, Color.White, index++, 1);
                ElTabelleSheetHelper.SetColumnHeaderColor(sheet, genchiIro, foreColor, index, 0);
                ElTabelleSheetHelper.SetColumnHeaderColor(sheet, orangeIro, Color.White, index++, 1);
                ElTabelleSheetHelper.SetColumnHeaderColor(sheet, genchiIro, foreColor, index, 0);
                ElTabelleSheetHelper.SetColumnHeaderColor(sheet, orangeIro, Color.White, index++, 1);
                ElTabelleSheetHelper.SetColumnHeaderColor(sheet, japanIro, foreColor, index, 0);
                ElTabelleSheetHelper.SetColumnHeaderColor(sheet, orangeIro, Color.White, index++, 1);
                ElTabelleSheetHelper.SetColumnHeaderColor(sheet, japanIro, foreColor, index, 0);
                ElTabelleSheetHelper.SetColumnHeaderColor(sheet, orangeIro, Color.White, index++, 1);
                ElTabelleSheetHelper.SetColumnHeaderColor(sheet, japanIro, foreColor, index, 0);
                ElTabelleSheetHelper.SetColumnHeaderColor(sheet, orangeIro, Color.White, index++, 1);
                ElTabelleSheetHelper.SetColumnHeaderColor(sheet, japanIro, foreColor, index, 0);
                ElTabelleSheetHelper.SetColumnHeaderColor(sheet, orangeIro, Color.White, index++, 1);
                ElTabelleSheetHelper.SetColumnHeaderColor(sheet, japanIro, foreColor, index, 0);
                ElTabelleSheetHelper.SetColumnHeaderColor(sheet, orangeIro, Color.White, index++, 1);
                ElTabelleSheetHelper.SetColumnHeaderColor(sheet, orangeIro, Color.White, index++);
                ElTabelleSheetHelper.SetColumnHeaderColor(sheet, orangeIro, Color.White, index++);
                ElTabelleSheetHelper.SetColumnHeaderColor(sheet, orangeIro, Color.White, index++);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// MultiRowのフォーマット作成
        /// </summary>
        /// <param name="editBtnEnabled"></param>
        /// <create>M.Tsutsumi 2010/09/17</create>
        /// <update>D.Okumura 2019/06/18 添付ファイル追加対応(技連4-5)</update>
        /// <update>T.Nukaga 2019/11/20 AR7000番運用対応</update>
        /// --------------------------------------------------
        private void SetMultiRowFormat(bool editBtnEnabled)
        {
            try
            {
                // フォーマット管理クラス生成
                this._multiRowFormat = new MultiRowFormat();

                // １レコード内の行数指定
                this._multiRowFormat.SetRowNum(SHEET_ROW_BLOCK);

                // セルの設定
                int index = 0;
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Button, index++, 0, 1, 2, Resources.ARJoho_ElTabelleColumnEdit, editBtnEnabled, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Button, index++, 0, 1, 2, Resources.ARJoho_ElTabelleColumnReference, true, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_T_AR.AR_NO, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, FLD_JYOKYO_FLAG_NAME, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index, 0, 1, 1, Def_T_AR.UPDATE_DATE, false, false, MultiRowCell.GetEnterKeyActionArray(KeyAction.NextRow));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 1, 1, 1, Def_T_AR.UPDATE_USER_NAME, false, false, MultiRowCell.GetEnterKeyActionArray(KeyAction.PrevRow, KeyAction.NextCell));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index, 0, 1, 1, Def_T_AR.HASSEI_DATE, false, false, MultiRowCell.GetEnterKeyActionArray(KeyAction.NextRow));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 1, 1, 1, Def_T_AR.RENRAKUSHA, false, false, MultiRowCell.GetEnterKeyActionArray(KeyAction.PrevRow, KeyAction.NextCell));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index, 0, 1, 1, Def_T_AR.KISHU, false, false, MultiRowCell.GetEnterKeyActionArray(KeyAction.NextRow));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 1, 1, 1, Def_T_AR.GOKI, false, false, MultiRowCell.GetEnterKeyActionArray(KeyAction.PrevRow, KeyAction.NextCell));

                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_T_AR.MOTO_AR_NO, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, ComDefine.FLD_KEKKA_AR_NO, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Button, index++, 0, 1, 2, "...", true, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));

                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_T_AR.HUGUAI, false, true, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_T_AR.TAISAKU, false, true, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_T_AR.GENBA_TOTYAKUKIBOU_DATE, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_T_AR.TAIO_BUSHO, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index, 0, 1, 1, Def_T_AR.GIREN_NO_1, false, false, MultiRowCell.GetEnterKeyActionArray(KeyAction.NextRow));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 1, 1, 1, Def_T_AR.GIREN_NO_2, false, false, MultiRowCell.GetEnterKeyActionArray(KeyAction.PrevRow, KeyAction.NextCell));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index, 0, 1, 1, Def_T_AR.GIREN_NO_3, false, false, MultiRowCell.GetEnterKeyActionArray(KeyAction.NextRow));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 1, 1, 1, Def_T_AR.GIREN_NO_4, false, false, MultiRowCell.GetEnterKeyActionArray(KeyAction.PrevRow, KeyAction.NextCell));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index, 0, 1, 1, Def_T_AR.GIREN_NO_5, false, false, MultiRowCell.GetEnterKeyActionArray(KeyAction.NextRow));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 1, 1, 1, "", false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_T_AR.GENCHI_TEHAISAKI, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_T_AR.GENCHI_SETTEINOKI_DATE, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_T_AR.GENCHI_SHUKKAYOTEI_DATE, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_T_AR.GENCHI_KOJYOSHUKKA_DATE, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_T_AR.SHUKKAHOHO, false, true, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_T_AR.JP_SETTEINOKI_DATE, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_T_AR.JP_SHUKKAYOTEI_DATE, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_T_AR.JP_KOJYOSHUKKA_DATE, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_T_AR.JP_UNSOKAISHA_NAME, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_T_AR.JP_OKURIJYO_NO, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_T_AR.GMS_HAKKO_NO, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_T_AR.SHIYORENRAKU_NO, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_T_AR.BIKO, false, true, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));

                // 非表示セルの設定
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_T_AR.NONYUSAKI_CD, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_M_NONYUSAKI.NONYUSAKI_NAME, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_T_AR.LIST_FLAG, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, FLD_LIST_FLAG_NAME, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, FLD_HUGUAI_ALL, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, FLD_TAISAKU_ALL, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, FLD_BIKO_ALL, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_M_NONYUSAKI.LIST_FLAG_NAME0, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_M_NONYUSAKI.LIST_FLAG_NAME1, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_M_NONYUSAKI.LIST_FLAG_NAME2, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_M_NONYUSAKI.LIST_FLAG_NAME3, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_M_NONYUSAKI.LIST_FLAG_NAME4, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_M_NONYUSAKI.LIST_FLAG_NAME5, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_M_NONYUSAKI.LIST_FLAG_NAME6, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_M_NONYUSAKI.LIST_FLAG_NAME7, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                // @@@ ↑
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_T_AR.HASSEI_YOUIN, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_T_AR.REFERENCE_NO_1, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, index++, 0, 1, 2, Def_T_AR.REFERENCE_NO_2, false, false, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Text初期化

        /// --------------------------------------------------
        /// <summary>
        /// Text初期化
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/10</create>
        /// <update>M.Tsutsumi 2011/02/16</update>
        /// --------------------------------------------------
        private void InitializeText()
        {
            try
            {
                txtNonyusakiCd.ImeMode = ImeMode.Disable;
                txtNonyusakiCd.InputRegulation = "abnls";
                txtNonyusakiCd.IsInputRegulation = true;
                txtNonyusakiCd.MaxByteLengthMode = true;
                txtNonyusakiCd.MaxLength = 4;

                //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36
                txtListFlagName0.ImeMode = ImeMode.Hiragana;
                txtListFlagName0.InputRegulation = "F";
                txtListFlagName0.IsInputRegulation = false;
                txtListFlagName0.MaxByteLengthMode = true;
                txtListFlagName0.MaxLength = 40;

                txtListFlagName1.ImeMode = ImeMode.Hiragana;
                txtListFlagName1.InputRegulation = "F";
                txtListFlagName1.IsInputRegulation = false;
                txtListFlagName1.MaxByteLengthMode = true;
                txtListFlagName1.MaxLength = 40;

                txtListFlagName2.ImeMode = ImeMode.Hiragana;
                txtListFlagName2.InputRegulation = "F";
                txtListFlagName2.IsInputRegulation = false;
                txtListFlagName2.MaxByteLengthMode = true;
                txtListFlagName2.MaxLength = 40;

                txtListFlagName3.ImeMode = ImeMode.Hiragana;
                txtListFlagName3.InputRegulation = "F";
                txtListFlagName3.IsInputRegulation = false;
                txtListFlagName3.MaxByteLengthMode = true;
                txtListFlagName3.MaxLength = 40;

                txtListFlagName4.ImeMode = ImeMode.Hiragana;
                txtListFlagName4.InputRegulation = "F";
                txtListFlagName4.IsInputRegulation = false;
                txtListFlagName4.MaxByteLengthMode = true;
                txtListFlagName4.MaxLength = 40;

                txtListFlagName5.ImeMode = ImeMode.Hiragana;
                txtListFlagName5.InputRegulation = "F";
                txtListFlagName5.IsInputRegulation = false;
                txtListFlagName5.MaxByteLengthMode = true;
                txtListFlagName5.MaxLength = 40;

                txtListFlagName6.ImeMode = ImeMode.Hiragana;
                txtListFlagName6.InputRegulation = "F";
                txtListFlagName6.IsInputRegulation = false;
                txtListFlagName6.MaxByteLengthMode = true;
                txtListFlagName6.MaxLength = 40;

                txtListFlagName7.ImeMode = ImeMode.Hiragana;
                txtListFlagName7.InputRegulation = "F";
                txtListFlagName7.IsInputRegulation = false;
                txtListFlagName7.MaxByteLengthMode = true;
                txtListFlagName7.MaxLength = 40;
                // @@@ ↑

                txtTaisaku.ImeMode = ImeMode.Hiragana;
                //txtTaisaku.InputRegulation = "ABRLKSHFabrlks";
                txtTaisaku.InputRegulation = "F";
                txtTaisaku.IsInputRegulation = false;
                txtTaisaku.MaxByteLengthMode = true;
                txtTaisaku.MaxLength = 1000;

                txtGirenNo.ImeMode = ImeMode.Disable;
                txtGirenNo.InputRegulation = "abnls";
                txtGirenNo.IsInputRegulation = true;
                txtGirenNo.MaxByteLengthMode = true;
                txtGirenNo.MaxLength = 22;

                txtGenchiTehaisaki.ImeMode = ImeMode.Hiragana;
                //txtGenchiTehaisaki.InputRegulation = "ABRLKSHFabrlks";
                txtGenchiTehaisaki.InputRegulation = "F";
                txtGenchiTehaisaki.IsInputRegulation = false;
                txtGenchiTehaisaki.MaxByteLengthMode = true;
                txtGenchiTehaisaki.MaxLength = 20;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region Combo初期化

        /// --------------------------------------------------
        /// <summary>
        /// Combo初期化
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private void InitializeCombo()
        {
            try
            {
                cboJyokyoFlag.DropDownStyle = ComboBoxStyle.DropDownList;
                this.MakeCmbBox(cboJyokyoFlag, JYOKYO_FLAG_AR.GROUPCD);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region RadioButton初期化

        /// --------------------------------------------------
        /// <summary>
        /// RadioButton初期化
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/20</create>
        /// <update>M.Tsutsumi 2011/02/16</update>
        /// --------------------------------------------------
        private void InitializeRadioButton()
        {
            try
            {
                // ラジオボタン設定
                // (選ばれたくない時があるからcheckは自分で制御)
                this.rdoListFlag0.AutoCheck = false;
                this.rdoListFlag1.AutoCheck = this.rdoListFlag0.AutoCheck;
                this.rdoListFlag2.AutoCheck = this.rdoListFlag0.AutoCheck;
                this.rdoListFlag3.AutoCheck = this.rdoListFlag0.AutoCheck;
                this.rdoListFlag4.AutoCheck = this.rdoListFlag0.AutoCheck;
                this.rdoListFlag5.AutoCheck = this.rdoListFlag0.AutoCheck;
                this.rdoListFlag6.AutoCheck = this.rdoListFlag0.AutoCheck;
                this.rdoListFlag7.AutoCheck = this.rdoListFlag0.AutoCheck;

                // リスト区分セット
                CondCommon cond = new CondCommon(this.UserInfo);
                ConnCommon conn = new ConnCommon();

                cond.GroupCD = LIST_FLAG.GROUPCD;
                DataSet ds = conn.GetCommon(cond);

                if (ds == null || !ds.Tables.Contains(Def_M_COMMON.Name) ||
                                  ds.Tables[Def_M_COMMON.Name].Rows.Count < 1)
                {
                    return;
                }

                DataTable dt = ds.Tables[Def_M_COMMON.Name];
                // @@@ 2011/02/16 M.Tsutsumi Change Step2 No.36
                //int row = 0;
                DataRow[] drs;
                // @@@ ↑

                // @@@ 2011/02/16 M.Tsutsumi Change Step2 No.36
                //// [0]製造⇒現地
                //if (row + 1 <= dt.Rows.Count)
                //{
                //    rdoListFlag0.Text = ComFunc.GetFld(dt, row, Def_M_COMMON.ITEM_NAME);
                //    row += 1;
                //}
                //// [1]設計⇒現地
                //if (row + 1 <= dt.Rows.Count)
                //{
                //    rdoListFlag1.Text = ComFunc.GetFld(dt, row, Def_M_COMMON.ITEM_NAME);
                //    row += 1;
                //}
                //// [2]現場⇒工場
                //if (row + 1 <= dt.Rows.Count)
                //{
                //    rdoListFlag2.Text = ComFunc.GetFld(dt, row, Def_M_COMMON.ITEM_NAME);
                //    row += 1;
                //}
                //// [3]現場⇒工場(手配以外)
                //if (row + 1 <= dt.Rows.Count)
                //{
                //    rdoListFlag3.Text = ComFunc.GetFld(dt, row, Def_M_COMMON.ITEM_NAME);
                //    row += 1;
                //}
                //// [4]現場⇒現地工場
                //if (row + 1 <= dt.Rows.Count)
                //{
                //    rdoListFlag4.Text = ComFunc.GetFld(dt, row, Def_M_COMMON.ITEM_NAME);
                //    row += 1;
                //}
                //// [5]現地工場⇒現場
                //if (row + 1 <= dt.Rows.Count)
                //{
                //    rdoListFlag5.Text = ComFunc.GetFld(dt, row, Def_M_COMMON.ITEM_NAME);
                //    row += 1;
                //}
                // [0]製造⇒現地
                drs = dt.Select(Def_M_COMMON.ITEM_CD + " = '" + LIST_FLAG.FLAG_0_NAME + "'");
                if (0 < drs.Length)
                {
                    this._listFlagName0 = ComFunc.GetFld(drs[0], Def_M_COMMON.ITEM_NAME).Trim();
                }
                // [1]設計⇒現地
                drs = dt.Select(Def_M_COMMON.ITEM_CD + " = '" + LIST_FLAG.FLAG_1_NAME + "'");
                if (0 < drs.Length)
                {
                    this._listFlagName1 = ComFunc.GetFld(drs[0], Def_M_COMMON.ITEM_NAME).Trim();
                }
                // [2]現場⇒工場
                drs = dt.Select(Def_M_COMMON.ITEM_CD + " = '" + LIST_FLAG.FLAG_2_NAME + "'");
                if (0 < drs.Length)
                {
                    this._listFlagName2 = ComFunc.GetFld(drs[0], Def_M_COMMON.ITEM_NAME).Trim();
                }
                // [3]現場⇒工場(手配以外)
                drs = dt.Select(Def_M_COMMON.ITEM_CD + " = '" + LIST_FLAG.FLAG_3_NAME + "'");
                if (0 < drs.Length)
                {
                    this._listFlagName3 = ComFunc.GetFld(drs[0], Def_M_COMMON.ITEM_NAME).Trim();
                }
                // [4]現場⇒現地工場
                drs = dt.Select(Def_M_COMMON.ITEM_CD + " = '" + LIST_FLAG.FLAG_4_NAME + "'");
                if (0 < drs.Length)
                {
                    this._listFlagName4 = ComFunc.GetFld(drs[0], Def_M_COMMON.ITEM_NAME).Trim();
                }
                // [5]現地工場⇒現場
                drs = dt.Select(Def_M_COMMON.ITEM_CD + " = '" + LIST_FLAG.FLAG_5_NAME + "'");
                if (0 < drs.Length)
                {
                    this._listFlagName5 = ComFunc.GetFld(drs[0], Def_M_COMMON.ITEM_NAME).Trim();
                }
                // [6]
                drs = dt.Select(Def_M_COMMON.ITEM_CD + " = '" + LIST_FLAG.FLAG_6_NAME + "'");
                if (0 < drs.Length)
                {
                    this._listFlagName6 = ComFunc.GetFld(drs[0], Def_M_COMMON.ITEM_NAME).Trim();
                }
                // [7]
                drs = dt.Select(Def_M_COMMON.ITEM_CD + " = '" + LIST_FLAG.FLAG_7_NAME + "'");
                if (0 < drs.Length)
                {
                    this._listFlagName7 = ComFunc.GetFld(drs[0], Def_M_COMMON.ITEM_NAME).Trim();
                }
                // 表示
                txtListFlagName0.Text = this._listFlagName0;
                txtListFlagName1.Text = this._listFlagName1;
                txtListFlagName2.Text = this._listFlagName2;
                txtListFlagName3.Text = this._listFlagName3;
                txtListFlagName4.Text = this._listFlagName4;
                txtListFlagName5.Text = this._listFlagName5;
                txtListFlagName6.Text = this._listFlagName6;
                txtListFlagName7.Text = this._listFlagName7;
                // @@@ ↑
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 処理モードの初期化
        /// --------------------------------------------------
        /// <summary>
        /// 処理モードの初期化
        /// </summary>
        /// <create>H.Tsunamura 2010/10/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private void InitializeMode()
        {

            if (!ComDefine.ROLE_KANRISYA.Contains(this.UserInfo.RoleID))
            {
                this.rdoInsert.Enabled = false;
            }

        }
        #endregion

        #endregion

        #region 終了処理

        /// --------------------------------------------------
        /// <summary>
        /// 終了処理
        /// </summary>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2012/04/25</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (0 < this._modelessNum)
            {
                e.Cancel = true;
                return;
            }
            base.OnClosing(e);
            if (this._dtARCost != null)
            {
                this._dtARCost.Dispose();
                this._dtARCost = null;
            }
        }

        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/10</create>
        /// <update>M.Tsutsumi 2011/02/16</update>
        /// <update>H.Tajimi 2018/10/15 FE要望対応</update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // ベースでClearMessageの呼出しは行われています。
                // 処理モード
                this.rdoUpdate.Checked = true;

                // コントロール
                // 納入先
                this.txtNonyusakiName.Text = string.Empty;
                // 管理No
                this.txtNonyusakiCd.Text = string.Empty;
                // リスト選択
                this.rdoListFlag0.Checked = true;
                // 検索項目
                this.SearchClear();
                // グリッド
                this.SheetClear();

                // 変数クリア
                this._nonyusakiComplete = false;
                if (this._dtARCost != null)
                {
                    this._dtARCost.Dispose();
                    this._dtARCost = null;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #region 検索項目のクリア

        /// --------------------------------------------------
        /// <summary>
        /// 検索項目のクリア
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SearchClear()
        {
            try
            {
                this.txtNonyusakiName.Text = string.Empty;
                this.txtNonyusakiCd.Text = string.Empty;
                // 状況選択
                this.cboJyokyoFlag.SelectedValue = JYOKYO_FLAG_AR.DEFAULT_VALUE1;
                // 対策内容
                this.txtTaisaku.Text = string.Empty;
                // 技連NO
                this.txtGirenNo.Text = string.Empty;
                // 手配先
                this.txtGenchiTehaisaki.Text = string.Empty;

                //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36
                this.txtListFlagName0.Text = this._listFlagName0;
                this.txtListFlagName1.Text = this._listFlagName1;
                this.txtListFlagName2.Text = this._listFlagName2;
                this.txtListFlagName3.Text = this._listFlagName3;
                this.txtListFlagName4.Text = this._listFlagName4;
                this.txtListFlagName5.Text = this._listFlagName5;
                this.txtListFlagName6.Text = this._listFlagName6;
                this.txtListFlagName7.Text = this._listFlagName7;
                // @@@ ↑
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/11</create>
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
            this.shtMeisai.MultiRowAllClear();
            this.shtMeisai.DataSource = null;
            this.shtMeisai.MaxRows = 0;
            this.shtMeisai.Enabled = false;
            this.shtMeisai.Redraw = true;
        }

        #endregion

        #endregion

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>M.Tsutsumi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                // 検索用入力チェック
                // 納入先チェック
                if (string.IsNullOrEmpty(this.txtNonyusakiName.Text))
                {
                    // 納入先一覧から納入先を選択して下さい。
                    this.ShowMessage("A0100010008");
                    this.txtNonyusakiName.Focus();
                    return false;
                }
                // 存在チェック
                ret = this.CheckNonyusaki();
                if (!ret)
                {
                    this.txtNonyusakiName.Focus();
                    return false;
                }
                //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36
                // リスト区分の選択チェック
                ret = this.CheckListFlag();
                if (!ret)
                {
                    this.txtNonyusakiName.Focus();
                    return false;
                }
                // @@@ ↑
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
        /// <create>M.Tsutsumi 2010/08/12</create>
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
        /// <create>M.Tsutsumi 2010/08/12</create>
        /// <update>H.Tajimi 2018/10/15 FE要望対応</update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                CondA1 cond = this.GetCondition();
                ConnA01 conn = new ConnA01();

                DataSet ds = conn.GetArDataList(cond);
                if (ds == null || !ds.Tables.Contains(Def_T_AR.Name) ||
                                  ds.Tables[Def_T_AR.Name].Rows.Count < 1)
                {
                    // 該当する明細は存在しません。
                    this.ShowMessage("A9999999022");
                    this.SheetClear();
                    return false;
                }
                // データの設定
                try
                {
                    this.shtMeisai.Redraw = false;
                    // まるちろうのフォーマット作り直し
                    bool editBtnEnabled = true;
                    if (this.rdoInsert.Checked)
                    {
                        // 新規登録
                        editBtnEnabled = false;
                    }
                    else
                    {
                        // 編集
                        editBtnEnabled = !this._nonyusakiComplete;
                    }
                    this.SetMultiRowFormat(editBtnEnabled);

                    this.shtMeisai.SetMultiRowDataSource(ds.Tables[Def_T_AR.Name], this._multiRowFormat);
                    this.shtMeisai.Enabled = true;
                    // 最も左上に表示されているセルの設定
                    if (0 < this.shtMeisai.MaxRows)
                    {
                        this.shtMeisai.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtMeisai.TopLeft.Row);
                    }
                    // AR対応費用の取得データ
                    if (this._dtARCost != null)
                    {
                        this._dtARCost.Dispose();
                        this._dtARCost = null;
                    }
                    this._dtARCost = ds.Tables[Def_T_AR_COST.Name];
                }
                finally
                {
                    this.shtMeisai.Redraw = true;
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

        #region イベント

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F04(進捗管理)ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/04</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F04Button_Click(object sender, EventArgs e)
        {
            if (_instShinchokuKanri != null)
            {
                _instShinchokuKanri.Activate();
                return;
            }
            base.fbrFunction_F04Button_Click(sender, e);
            try
            {
                this.ClearMessage();

                // 号機数取得
                var conn = new ConnA01();
                var cond = this.GetCondition();
                int shinchokuKanriGokiNum = conn.GetGokiNum(cond);

                // 検索用入力チェック
                // 納入先チェック
                if (string.IsNullOrEmpty(this.txtNonyusakiName.Text))
                {
                    // 納入先一覧から納入先を選択して下さい。
                    this.ShowMessage("A0100010008");
                    this.txtNonyusakiName.Focus();
                    return;
                }
                // 存在チェック
                if (!this.CheckNonyusaki())
                {
                    this.txtNonyusakiName.Focus();
                    return;
                }
                // 員数表存在チェック  
                if (shinchokuKanriGokiNum < 1)
                {
                    this.ShowMessage("A0100010014");
                    this.txtNonyusakiName.Focus();
                    return;
                }

                // ＡＲ情報進捗管理画面
                _instShinchokuKanri = new ShinchokuKanri(this.UserInfo, ComDefine.TITLE_A0100050, this.txtNonyusakiName.Text, this._bukkenNo, this.txtNonyusakiCd.Text);
                _instShinchokuKanri.Icon = this.Icon;
                _instShinchokuKanri.FormClosed += new FormClosedEventHandler(ARJohoMeisai_ShinchokuKanri_FormClosed);
                _instShinchokuKanri.Show();
                this._modelessNum++;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F05(員数表)ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/04</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F05Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F05Button_Click(sender, e);
            try
            {
                this.ClearMessage();

                // 検索用入力チェック
                // 納入先チェック
                if (string.IsNullOrEmpty(this.txtNonyusakiName.Text))
                {
                    // 納入先一覧から納入先を選択して下さい。
                    this.ShowMessage("A0100010008");
                    this.txtNonyusakiName.Focus();
                    return;
                }
                // 存在チェック
                if (!this.CheckNonyusaki())
                {
                    this.txtNonyusakiName.Focus();
                    return;
                }

                // 完了納入先チェック
                if (this._nonyusakiComplete)
                {
                    // 完了納入先となっています。管理者に確認して下さい。
                    this.ShowMessage("A9999999047");
                    return;
                }

                // ＡＲ情報員数表取込
                var frm = new ShinchokuInzuhyoImport(this.UserInfo, ComDefine.TITLE_A0100040, this.txtNonyusakiName.Text, this.txtNonyusakiCd.Text);
                frm.Icon = this.Icon;
                frm.Show(this);
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
        /// <create>H.Tajimi 2018/10/16</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                // 入力チェック
                if (!this.CheckInputSearch())
                {
                    return;
                }

                var cond = this.GetCondition();
                var conn = new ConnA01();
                string errMsgID;
                if (!conn.ExistNonyusakiAndHiyouExcel(cond, out errMsgID))
                {
                    this.ShowMessage(errMsgID);
                    return;
                }

                string fileName;
                using (SaveFileDialog frm = new SaveFileDialog())
                {
                    frm.FileName = ComDefine.EXCEL_FILE_AR_COST;
                    frm.Title = Resources.ARJoho_ExcelFileAR;
                    frm.Filter = Resources.ARJoho_ExcelFileARFilter;
                    if (frm.ShowDialog() != DialogResult.OK) return;
                    fileName = frm.FileName;
                }

                // 納入先マスタが存在するか確認
                if (!conn.IsExistenceNonyusaki(cond))
                {
                    // 該当する納入先はありません。
                    this.ShowMessage("A9999999044");
                    return;
                }

                var dt = conn.GetAllARCostListData(cond);
                if (!UtilData.ExistsData(dt))
                {
                    // 出力するデータがありません。
                    this.ShowMessage("A0100010004");
                    return;
                }

                // Excel出力処理
                var export = new ExportAllARCost();
                string msgID;
                string[] args;
                export.ExportExcel(fileName, dt, out msgID, out args);
                if (!string.IsNullOrEmpty(msgID))
                {
                    this.ShowMessage(msgID, args);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F08ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/10/16</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F08Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F08Button_Click(sender, e);
            try
            {
                if (this._dtARCost == null || this._dtARCost.Rows.Count <= 0)
                {
                    // 出力するデータがありません。
                    this.ShowMessage("A0100010004");
                    return;
                }
                DataTable dtExport = this._dtARCost.Copy();

                SaveFileDialog frm = new SaveFileDialog();
                frm.FileName = ComDefine.EXCEL_FILE_AR_COST;
                frm.Title = Resources.ARJoho_ComDefineExcelFileAR;
                frm.Filter = Resources.ARJoho_ComDefineExcelFileARFilter;
                if (0 < this.shtMeisai.MaxRows && frm.ShowDialog() != DialogResult.OK) return;

                // Excel出力処理
                var export = new ExportARCost();
                string msgID;
                string[] args;
                export.ExportExcel(frm.FileName, dtExport, out msgID, out args);
                if (!string.IsNullOrEmpty(msgID))
                {
                    this.ShowMessage(msgID, args);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F09ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2012/04/25</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F09Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F09Button_Click(sender, e);
            try
            {
                // 入力チェック
                if (!this.CheckInputSearch())
                {
                    return;
                }

                var cond = this.GetCondition();
                var conn = new ConnA01();
                string errMsgID;
                if (!conn.ExistNonyusakiAndExcle(cond, out errMsgID))
                {
                    this.ShowMessage(errMsgID);
                    return;
                }

                string fileName;
                using (SaveFileDialog frm = new SaveFileDialog())
                {
                    frm.FileName = ComDefine.EXCEL_FILE_AR;
                    frm.Title = Resources.ARJoho_ExcelFileAR;
                    frm.Filter = Resources.ARJoho_ExcelFileARFilter;
                    if (frm.ShowDialog() != DialogResult.OK) return;
                    fileName = frm.FileName;
                }

                // 納入先マスタが存在するか確認
                if (!conn.IsExistenceNonyusaki(cond))
                {
                    // 該当する納入先はありません。
                    this.ShowMessage("A9999999044");
                    return;
                }

                var dt = conn.GetAllARListData(cond);
                if (!UtilData.ExistsData(dt))
                {
                    // 出力するデータがありません。
                    this.ShowMessage("A0100010004");
                    return;
                }

                // Excel出力処理
                var export = new ExportAllARJoho();
                string msgID;
                string[] args;
                export.ExportExcel(fileName, dt, out msgID, out args);
                if (!string.IsNullOrEmpty(msgID))
                {
                    this.ShowMessage(msgID, args);
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
        /// <create>M.Tsutsumi 2010/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F10Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F10Button_Click(sender, e);
            try
            {
                DataTable dtExport = this.shtMeisai.GetMultiRowGetDataSource();
                if (dtExport == null || dtExport.Rows.Count <= 0)
                {
                    // 出力するデータがありません。
                    this.ShowMessage("A0100010004");
                    return;
                }

                SaveFileDialog frm = new SaveFileDialog();
                frm.FileName = ComDefine.EXCEL_FILE_AR;
                // @@@ 2011/02/24 M.Tsutsumi Change 
                //frm.Title = "明細Excelファイル保存";
                //frm.Filter = "Excelファイル|*.xls";
                frm.Title = Resources.ARJoho_ComDefineExcelFileAR;
                frm.Filter = Resources.ARJoho_ComDefineExcelFileARFilter;
                // @@@ ↑
                if (0 < this.shtMeisai.MaxRows && frm.ShowDialog() != DialogResult.OK) return;

                // Excel出力処理
                ExportARJoho export = new ExportARJoho();
                string msgID;
                string[] args;
                export.ExportExcel(frm.FileName, dtExport, out msgID, out args);
                if (!string.IsNullOrEmpty(msgID))
                {
                    this.ShowMessage(msgID, args);
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
        /// <create>M.Tsutsumi 2010/08/27</create>
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
        /// 新規登録ラジオボタンのCheckedChangedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/08/11</create>
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
        /// 編集ラジオボタンのCheckedChangedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/08/11</create>
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

        #endregion

        #region リスト選択

        #region rdoListFlag0

        /// --------------------------------------------------
        /// <summary>
        /// CheckedChangedイベント(選ばれたくない時があるからcheckを自分で制御)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2011/02/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoListFlag0_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.rdoListFlag0.Checked)
                {
                    return;
                }

                this.rdoListFlag1.Checked = false;
                this.rdoListFlag2.Checked = false;
                this.rdoListFlag3.Checked = false;
                this.rdoListFlag4.Checked = false;
                this.rdoListFlag5.Checked = false;
                this.rdoListFlag6.Checked = false;
                this.rdoListFlag7.Checked = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// Clickイベント(選ばれたくない時があるからcheckを自分で制御)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2011/02/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoListFlag0_Click(object sender, EventArgs e)
        {
            try
            {
                // リスト区分名がセットされている時だけOK
                if (string.IsNullOrEmpty(this.txtListFlagName0.Text))
                {
                    return;
                }
                this.rdoListFlag0.Checked = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region rdoListFlag1

        /// --------------------------------------------------
        /// <summary>
        /// CheckedChangedイベント(選ばれたくない時があるからcheckを自分で制御)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2011/02/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoListFlag1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.rdoListFlag1.Checked)
                {
                    return;
                }
                this.rdoListFlag0.Checked = false;
                this.rdoListFlag2.Checked = false;
                this.rdoListFlag3.Checked = false;
                this.rdoListFlag4.Checked = false;
                this.rdoListFlag5.Checked = false;
                this.rdoListFlag6.Checked = false;
                this.rdoListFlag7.Checked = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// Clickイベント(選ばれたくない時があるからcheckを自分で制御)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2011/02/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoListFlag1_Click(object sender, EventArgs e)
        {
            try
            {
                // リスト区分名がセットされている時だけOK
                if (string.IsNullOrEmpty(this.txtListFlagName1.Text))
                {
                    return;
                }
                this.rdoListFlag1.Checked = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region rdoListFlag2

        /// --------------------------------------------------
        /// <summary>
        /// CheckedChangedイベント(選ばれたくない時があるからcheckを自分で制御)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2011/02/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoListFlag2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.rdoListFlag2.Checked)
                {
                    return;
                }
                this.rdoListFlag0.Checked = false;
                this.rdoListFlag1.Checked = false;
                this.rdoListFlag3.Checked = false;
                this.rdoListFlag4.Checked = false;
                this.rdoListFlag5.Checked = false;
                this.rdoListFlag6.Checked = false;
                this.rdoListFlag7.Checked = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// Clickイベント(選ばれたくない時があるからcheckを自分で制御)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2011/02/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoListFlag2_Click(object sender, EventArgs e)
        {
            try
            {
                // リスト区分名がセットされている時だけOK
                if (string.IsNullOrEmpty(this.txtListFlagName2.Text))
                {
                    return;
                }
                this.rdoListFlag2.Checked = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region rdoListFlag3

        /// --------------------------------------------------
        /// <summary>
        /// CheckedChangedイベント(選ばれたくない時があるからcheckを自分で制御)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2011/02/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoListFlag3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.rdoListFlag3.Checked)
                {
                    return;
                }
                this.rdoListFlag0.Checked = false;
                this.rdoListFlag1.Checked = false;
                this.rdoListFlag2.Checked = false;
                this.rdoListFlag4.Checked = false;
                this.rdoListFlag5.Checked = false;
                this.rdoListFlag6.Checked = false;
                this.rdoListFlag7.Checked = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// Clickイベント(選ばれたくない時があるからcheckを自分で制御)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2011/02/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoListFlag3_Click(object sender, EventArgs e)
        {
            try
            {
                // リスト区分名がセットされている時だけOK
                if (string.IsNullOrEmpty(this.txtListFlagName3.Text))
                {
                    return;
                }
                this.rdoListFlag3.Checked = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region rdoListFlag4

        /// --------------------------------------------------
        /// <summary>
        /// CheckedChangedイベント(選ばれたくない時があるからcheckを自分で制御)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2011/02/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoListFlag4_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.rdoListFlag4.Checked)
                {
                    return;
                }
                this.rdoListFlag0.Checked = false;
                this.rdoListFlag1.Checked = false;
                this.rdoListFlag2.Checked = false;
                this.rdoListFlag3.Checked = false;
                this.rdoListFlag5.Checked = false;
                this.rdoListFlag6.Checked = false;
                this.rdoListFlag7.Checked = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// Clickイベント(選ばれたくない時があるからcheckを自分で制御)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2011/02/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoListFlag4_Click(object sender, EventArgs e)
        {
            try
            {
                // リスト区分名がセットされている時だけOK
                if (string.IsNullOrEmpty(this.txtListFlagName4.Text))
                {
                    return;
                }
                this.rdoListFlag4.Checked = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region rdoListFlag5

        /// --------------------------------------------------
        /// <summary>
        /// CheckedChangedイベント(選ばれたくない時があるからcheckを自分で制御)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2011/02/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoListFlag5_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.rdoListFlag5.Checked)
                {
                    return;
                }
                this.rdoListFlag0.Checked = false;
                this.rdoListFlag1.Checked = false;
                this.rdoListFlag2.Checked = false;
                this.rdoListFlag3.Checked = false;
                this.rdoListFlag4.Checked = false;
                this.rdoListFlag6.Checked = false;
                this.rdoListFlag7.Checked = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// Clickイベント(選ばれたくない時があるからcheckを自分で制御)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2011/02/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoListFlag5_Click(object sender, EventArgs e)
        {
            try
            {
                // リスト区分名がセットされている時だけOK
                if (string.IsNullOrEmpty(this.txtListFlagName5.Text))
                {
                    return;
                }
                this.rdoListFlag5.Checked = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region rdoListFlag6

        /// --------------------------------------------------
        /// <summary>
        /// CheckedChangedイベント(選ばれたくない時があるからcheckを自分で制御)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2011/02/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoListFlag6_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.rdoListFlag6.Checked)
                {
                    return;
                }
                this.rdoListFlag0.Checked = false;
                this.rdoListFlag1.Checked = false;
                this.rdoListFlag2.Checked = false;
                this.rdoListFlag3.Checked = false;
                this.rdoListFlag4.Checked = false;
                this.rdoListFlag5.Checked = false;
                this.rdoListFlag7.Checked = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// Clickイベント(選ばれたくない時があるからcheckを自分で制御)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2011/02/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoListFlag6_Click(object sender, EventArgs e)
        {
            try
            {
                // リスト区分名がセットされている時だけOK
                if (string.IsNullOrEmpty(this.txtListFlagName6.Text))
                {
                    return;
                }
                this.rdoListFlag6.Checked = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region rdoListFlag7

        /// --------------------------------------------------
        /// <summary>
        /// CheckedChangedイベント(選ばれたくない時があるからcheckを自分で制御)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2011/02/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoListFlag7_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.rdoListFlag7.Checked)
                {
                    return;
                }
                this.rdoListFlag0.Checked = false;
                this.rdoListFlag1.Checked = false;
                this.rdoListFlag2.Checked = false;
                this.rdoListFlag3.Checked = false;
                this.rdoListFlag4.Checked = false;
                this.rdoListFlag5.Checked = false;
                this.rdoListFlag6.Checked = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// Clickイベント(選ばれたくない時があるからcheckを自分で制御)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2011/02/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoListFlag7_Click(object sender, EventArgs e)
        {
            try
            {
                // リスト区分名がセットされている時だけOK
                if (string.IsNullOrEmpty(this.txtListFlagName7.Text))
                {
                    return;
                }
                this.rdoListFlag7.Checked = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 一覧選択ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 一覧選択ボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnListSelect_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                string shukkaFlag = SHUKKA_FLAG.AR_VALUE1;
                string nonyusakiName = this.txtNonyusakiName.Text;

                if (this.rdoInsert.Checked)
                {
                    using (BukkenMeiIchiran frm = new BukkenMeiIchiran(this.UserInfo, shukkaFlag, nonyusakiName))
                    {
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            DataRow dr = frm.SelectedRowData;
                            if (dr == null) return;
                            // 選択データを設定
                            this.txtNonyusakiName.Text = ComFunc.GetFld(dr, Def_M_BUKKEN.BUKKEN_NAME);
                            this._bukkenNo = ComFunc.GetFld(dr, Def_M_BUKKEN.BUKKEN_NO);
                            // フォーカス移動
                            this.SelectNextControl(this.ActiveControl, true, true, true, true);
                        }
                    }
                }
                else if (this.rdoUpdate.Checked)
                {
                    using (NonyusakiIchiran frm = new NonyusakiIchiran(this.UserInfo, shukkaFlag, nonyusakiName, null))
                    {
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            DataRow dr = frm.SelectedRowData;
                            if (dr == null) return;
                            // 選択データを設定
                            this.txtNonyusakiName.Text = ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_NAME);
                            this.txtNonyusakiCd.Text = ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_CD);
                            //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36
                            this.txtListFlagName0.Text = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME0, this._listFlagName0);
                            this.txtListFlagName1.Text = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME1, this._listFlagName1);
                            this.txtListFlagName2.Text = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME2, this._listFlagName2);
                            this.txtListFlagName3.Text = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME3, this._listFlagName3);
                            this.txtListFlagName4.Text = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME4, this._listFlagName4);
                            this.txtListFlagName5.Text = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME5, this._listFlagName5);
                            this.txtListFlagName6.Text = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME6, this._listFlagName6);
                            this.txtListFlagName7.Text = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME7, this._listFlagName7);
                            this.rdoListFlag0.Checked = true;
                            // @@@ ↑
                            this._bukkenNo = ComFunc.GetFld(dr, Def_M_NONYUSAKI.BUKKEN_NO);
                            // フォーカス移動
                            this.SelectNextControl(this.ActiveControl, true, true, true, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 明細登録ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 明細登録ボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/08/10</create>
        /// <update>R.Katsuo 2017/09/14 メール送信関係のチェック処理を追加</update>
        /// <update>T.Nukaga 2019/11/20 STEP12 AR7000番運用対応 </update>
        /// --------------------------------------------------
        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();

                // 検索用チェック
                if (!this.CheckInputSearch())
                {
                    return;
                }

                // 完了納入先チェック
                if (this._nonyusakiComplete == true)
                {
                    // 完了納入先となっています。管理者に確認して下さい。
                    this.ShowMessage("A9999999047");
                    return;
                }

                // 画面設定値取得
                //@@@ 2011/02/16 M.Tsutsumi Change Step2 No.36
                //string nonyusakiCd = this.txtNonyusakiCd.Text;
                //string nonyusakiName = this.txtNonyusakiName.Text;
                //string listFlag = this.GetListFlag();
                //string arNo = string.Empty;
                CondA1 cond = new CondA1(this.UserInfo);
                cond.NonyusakiCD = this.txtNonyusakiCd.Text;
                cond.NonyusakiName = this.txtNonyusakiName.Text;
                cond.ListFlag = this.GetListFlag();
                cond.ArNo = string.Empty;
                cond.ListFlagName0 = this.txtListFlagName0.Text;
                cond.ListFlagName1 = this.txtListFlagName1.Text;
                cond.ListFlagName2 = this.txtListFlagName2.Text;
                cond.ListFlagName3 = this.txtListFlagName3.Text;
                cond.ListFlagName4 = this.txtListFlagName4.Text;
                cond.ListFlagName5 = this.txtListFlagName5.Text;
                cond.ListFlagName6 = this.txtListFlagName6.Text;
                cond.ListFlagName7 = this.txtListFlagName7.Text;
                // @@@ ↑

                // メール送信チェック
                if (!this.CheckMailSend(cond, true))
                {
                    return;
                }

                // AR7000番運用時、警告ダイアログ表示
                if (!CheckARRelation())
                {
                    return;
                }

                // AR情報明細登録
                this.ShowARJohoMeisai(SystemBase.EditMode.Insert, cond, null);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 表示ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 表示ボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/08/10</create>
        /// <update>>T.Nukaga 2019/11/20 AR7000番運用対応</update>
        /// --------------------------------------------------
        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();

                this.SetShtMeisaiColumnVisible();
                
                if (this.RunSearch())
                {
                    // 検索条件のロック
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region HELPボタン

        /// --------------------------------------------------
        /// <summary>
        /// HELPボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnHelp_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();

                // ヘルプ表示
                ARJohoHelp frm = new ARJohoHelp(this.UserInfo);
                frm.Show();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 子画面が閉じた時の処理

        /// --------------------------------------------------
        /// <summary>
        /// 子画面が閉じた時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2012/04/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ARJohoMeisai_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this._modelessNum--;
                if ((sender as Form).DialogResult == DialogResult.OK
                    && this.rdoUpdate.Checked)
                {
                    this.SheetResearch((sender as ARJohoMeisai).Condition);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 子画面(進捗管理)が閉じた時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ARJohoMeisai_ShinchokuKanri_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_instShinchokuKanri != null)
            {
                _instShinchokuKanri = null;
                ARJohoMeisai_FormClosed(sender, e);
            }
        }

        #endregion

        #region グリッド

        #region CellNotify

        /// --------------------------------------------------
        /// <summary>
        /// セルのイベントが発生したときに発生します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/08/11</create>
        /// <update>D.Okumura 2019/11/25 AR画面表示処理追加</update>
        /// --------------------------------------------------
        private void shtMeisai_CellNotify(object sender, CellNotifyEventArgs e)
        {
            try
            {
                this.shtMeisai.CellPosition = e.Position;
                if ((this.shtMeisai.CellEditor as ButtonEditor) != null)
                {
                    bool isShowDialog = false;
                    bool isKekkaAr = false;
                    SystemBase.EditMode editMode = SystemBase.EditMode.None;

                    // セルのイベント処理
                    switch (e.Name)
                    {
                        case CellNotifyEvents.Click:
                            switch (e.Position.Column)
                            {
                                case SHEET_COL_DETAIL_BUTTON:
                                    // AR情報明細登録
                                    isShowDialog = true;
                                    editMode = SystemBase.EditMode.Update;
                                    break;
                                case SHEET_COL_REF_BUTTON:
                                    // AR情報明細登録
                                    isShowDialog = true;
                                    editMode = SystemBase.EditMode.View;
                                    break;
                                case SHEET_COL_KEKKA_BUTTON:
                                    // AR結果ボタン
                                    isShowDialog = true;
                                    isKekkaAr = true;
                                    editMode = SystemBase.EditMode.View; //暫定
                                    break;
                            }

                            if (isShowDialog && editMode != SystemBase.EditMode.None)
                            {
                                DataTable dt = this.shtMeisai.GetMultiRowGetDataSource();
                                int row = e.Position.Row / SHEET_ROW_BLOCK;
                                if (dt == null || dt.Rows.Count <= row) return;
                                //@@@ 2011/02/16 M.Tsutsumi Change Step2 No.36
                                //string nonyusakiCd = ComFunc.GetFld(dt, row, Def_T_AR.NONYUSAKI_CD);
                                //string nonyusakiName = ComFunc.GetFld(dt, row, Def_M_NONYUSAKI.NONYUSAKI_NAME);
                                //string listFlag = ComFunc.GetFld(dt, row, Def_T_AR.LIST_FLAG);
                                //string listFlagName = ComFunc.GetFld(dt, row, FLD_LIST_FLAG_NAME);
                                //string arNo = ComFunc.GetFld(dt, row, Def_T_AR.AR_NO);
                                CondA1 cond = new CondA1(this.UserInfo);
                                cond.NonyusakiCD = ComFunc.GetFld(dt, row, Def_T_AR.NONYUSAKI_CD);
                                cond.NonyusakiName = ComFunc.GetFld(dt, row, Def_M_NONYUSAKI.NONYUSAKI_NAME);
                                cond.ListFlag = ComFunc.GetFld(dt, row, Def_T_AR.LIST_FLAG);
                                cond.ArNo = ComFunc.GetFld(dt, row, Def_T_AR.AR_NO);
                                cond.ListFlagName0 = ComFunc.GetFld(dt, row, Def_M_NONYUSAKI.LIST_FLAG_NAME0);
                                cond.ListFlagName1 = ComFunc.GetFld(dt, row, Def_M_NONYUSAKI.LIST_FLAG_NAME1);
                                cond.ListFlagName2 = ComFunc.GetFld(dt, row, Def_M_NONYUSAKI.LIST_FLAG_NAME2);
                                cond.ListFlagName3 = ComFunc.GetFld(dt, row, Def_M_NONYUSAKI.LIST_FLAG_NAME3);
                                cond.ListFlagName4 = ComFunc.GetFld(dt, row, Def_M_NONYUSAKI.LIST_FLAG_NAME4);
                                cond.ListFlagName5 = ComFunc.GetFld(dt, row, Def_M_NONYUSAKI.LIST_FLAG_NAME5);
                                cond.ListFlagName6 = ComFunc.GetFld(dt, row, Def_M_NONYUSAKI.LIST_FLAG_NAME6);
                                cond.ListFlagName7 = ComFunc.GetFld(dt, row, Def_M_NONYUSAKI.LIST_FLAG_NAME7);
                                // @@@ ↑

                                if (editMode == SystemBase.EditMode.Update)
                                {
                                    // メール送信チェック
                                    if (!this.CheckMailSend(cond, false))
                                    {
                                        return;
                                    }
                                }
                                if (isKekkaAr)
                                {
                                    string kekka = ComFunc.GetFld(dt, row, ComDefine.FLD_KEKKA_AR_NO);
                                    if (string.IsNullOrEmpty(kekka))
                                    {
                                        // 結果ARの記載がない場合は新規登録画面を開く
                                        string motoAr = ComFunc.GetFld(dt, row, Def_T_AR.AR_NO);
                                        cond.ArNo = string.Empty; //空白を設定する
                                        var motoList = this._listFlagKekka.Values
                                            .Where(w => ComFunc.GetFld(w, Def_M_COMMON.VALUE2) == LIST_FLAG_KEKKA_VALUE2.REF_MOTO_AR_VALUE1)
                                            .ToArray();
                                        if (motoList.Length > 1)
                                        {
                                            // 複数件ある場合は選択肢を表示する
                                            using (var form = new ARJohoRelationSelect(this.UserInfo, motoAr, motoList))
                                            {
                                                if (form.ShowDialog(this) == DialogResult.OK)
                                                {
                                                    cond.ListFlag = form.ListFlagSelected;
                                                    this.ShowARJohoMeisai(SystemBase.EditMode.Insert, cond, motoAr);
                                                }
                                            }
                                        }
                                        else if (motoList.Length > 0)
                                        {
                                            // １件のみならそのまま登録へ遷移する
                                            cond.ListFlag = ComFunc.GetFld(motoList.FirstOrDefault(), Def_M_COMMON.VALUE1);
                                            this.ShowARJohoMeisai(SystemBase.EditMode.Insert, cond, motoAr);
                                        }
                                    }
                                    else
                                    {
                                        // 結果ARの記載がある場合は参照画面を開く
                                        cond.ArNo = kekka;
                                        cond.ListFlag = kekka.Substring(ComDefine.PREFIX_ARNO.Length, 1);
                                        this.ShowARJohoMeisai(SystemBase.EditMode.View, cond, null);
                                    }
                                }
                                else
                                {
                                    this.ShowARJohoMeisai(editMode, cond, null);
                                }
                            }
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


        #region 明細画面表示処理
        /// --------------------------------------------------
        /// <summary>
        /// 明細画面表示処理(外部呼出し)
        /// </summary>
        /// <param name="userInfo">ログイン情報</param>
        /// <param name="editMode">編集モード</param>
        /// <param name="nonyusakiCd">納入先コード</param>
        /// <param name="arno">ARNo</param>
        /// <param name="msgId">エラーメッセージ</param>
        /// <param name="msgArgs">エラーメッセージ</param>
        /// <returns>明細フォーム</returns>
        /// <create>D.Okumura 2020/01/28 AR画面表示処理追加</create>
        /// <update></update>
        /// --------------------------------------------------
        public static ARJohoMeisai ShowARJohoMeisai(UserInfo userInfo, SystemBase.EditMode editMode, string nonyusakiCd, string arno, out string msgId, out string[] msgArgs)
        {
            var conn = new ConnA01();
            var cond = new CondA1(userInfo);
            cond.NonyusakiCD = nonyusakiCd;
            cond.ShukkaFlag = SHUKKA_FLAG.AR_VALUE1;
            cond.UpdateUserID = userInfo.UserID;

            // 納入先取得
            DataSet ds = conn.GetNonyusaki(cond);
            if (!ComFunc.IsExistsData(ds, Def_M_NONYUSAKI.Name))
            {
                // 該当する納入先はありません。
                msgId = "A9999999044";
                msgArgs = new string[0];
                return null;
            }
            DataRow dr = ds.Tables[Def_M_NONYUSAKI.Name].Rows[0];
            // 管理区分チェック
            string kanriFlag = ComFunc.GetFld(ds, Def_M_NONYUSAKI.Name, 0, Def_M_NONYUSAKI.KANRI_FLAG);
            if (kanriFlag == KANRI_FLAG.KANRYO_VALUE1 && editMode == SystemBase.EditMode.Insert)
            {
                // 完了納入先となっています。管理者に確認して下さい。
                msgId = "A9999999047";
                msgArgs = new string[0];
                return null;
            }
            cond.BukkenNo = ComFunc.GetFld(dr, Def_M_NONYUSAKI.BUKKEN_NO);
            cond.ListFlagName0 = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME0);
            cond.ListFlagName1 = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME1);
            cond.ListFlagName2 = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME2);
            cond.ListFlagName3 = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME3);
            cond.ListFlagName4 = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME4);
            cond.ListFlagName5 = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME5);
            cond.ListFlagName6 = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME6);
            cond.ListFlagName7 = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME7);
            cond.NonyusakiName = ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_NAME);
            cond.NonyusakiCD = ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_CD);
            // ARNOを設定
            cond.ListFlag = arno.Substring(2, 1);
            cond.ArNo = arno;

            msgId = "";
            msgArgs = new string[0];
            return ShowARJohoMeisai(editMode, cond);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 明細画面表示処理(外部呼出し)
        /// </summary>
        /// <param name="editMode">編集モード</param>
        /// <param name="cond">条件</param>
        /// <returns>明細フォーム</returns>
        /// <create>D.Okumura 2019/11/25 AR画面表示処理追加</create>
        /// <update></update>
        /// --------------------------------------------------
        public static ARJohoMeisai ShowARJohoMeisai(SystemBase.EditMode editMode, CondA1 cond)
        {
            // 一覧画面を開かれている画面一覧から検索する
            ARJoho rootForm = null;
            foreach (var form in Application.OpenForms)
            {
                if (!(form is ARJoho))
                    continue;
                rootForm = form as ARJoho;
                break;
            }
            if (rootForm == null)
                return null;
            return rootForm.ShowARJohoMeisai(editMode, cond, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 明細画面表示処理
        /// </summary>
        /// <param name="editMode">編集モード</param>
        /// <param name="cond">条件</param>
        /// <param name="motoArNo">元ARNo(NULL許容)</param>
        /// <returns>明細フォーム</returns>
        /// <create>D.Okumura 2019/11/25 AR画面表示処理追加</create>
        /// <update></update>
        /// --------------------------------------------------
        protected ARJohoMeisai ShowARJohoMeisai(SystemBase.EditMode editMode, CondA1 cond, string motoArNo)
        {
            string title;
            switch (editMode)
            {
                case SystemBase.EditMode.Insert:
                    title = Resources.ARJoho_SubTitleInsert;
                    break;
                case SystemBase.EditMode.Update:
                    title = Resources.ARJoho_SubTitleUpdate;
                    break;
                case SystemBase.EditMode.View:
                    title = Resources.ARJoho_SubTitleView;
                    break;
                default:
                    title = string.Empty;
                    break;
            }
            // リスト区分名を設定
            string listFlagName;
            if (cond.ListFlag == LIST_FLAG.FLAG_0_VALUE1)
                listFlagName = cond.ListFlagName0;
            else if (cond.ListFlag == LIST_FLAG.FLAG_1_VALUE1)
                listFlagName = cond.ListFlagName1;
            else if (cond.ListFlag == LIST_FLAG.FLAG_2_VALUE1)
                listFlagName = cond.ListFlagName2;
            else if (cond.ListFlag == LIST_FLAG.FLAG_3_VALUE1)
                listFlagName = cond.ListFlagName3;
            else if (cond.ListFlag == LIST_FLAG.FLAG_4_VALUE1)
                listFlagName = cond.ListFlagName4;
            else if (cond.ListFlag == LIST_FLAG.FLAG_5_VALUE1)
                listFlagName = cond.ListFlagName5;
            else if (cond.ListFlag == LIST_FLAG.FLAG_6_VALUE1)
                listFlagName = cond.ListFlagName6;
            else if (cond.ListFlag == LIST_FLAG.FLAG_7_VALUE1)
                listFlagName = cond.ListFlagName7;
            else
                listFlagName = string.Empty;
            // リスト区分表示名称を整形
            listFlagName = string.Format("[{0}]{1}", cond.ListFlag, listFlagName);

#if false
            // 一覧画面を開かれている画面一覧から検索する
            ARJohoMeisai rootForm = null;
            foreach (var form in Application.OpenForms)
            {
                if (!(form is ARJohoMeisai))
                    continue;
                var meisaiForm = form as ARJohoMeisai;
                if (meisaiForm == null)
                    continue;
                var check = meisaiForm.Condition;
                if (check == null)
                    continue;
                if (meisaiForm.Title != ComDefine.TITLE_A0100020 + title)
                    continue;
                if (cond.BukkenNo != check.BukkenNo)
                    continue;
                if (cond.NonyusakiCD != check.NonyusakiCD)
                    continue;
                if (cond.ArNo != check.ArNo)
                    continue;
                if (cond.ListFlag != check.ListFlag)
                    continue;
                rootForm = meisaiForm;
                break;
            }
            if (rootForm != null && editMode != SystemBase.EditMode.Insert)
            {
                rootForm.Activate();
                return rootForm;
            }

#endif
            var frm = new ARJohoMeisai(this.UserInfo, editMode, ComDefine.TITLE_A0100020 + title, cond, listFlagName, this._bukkenNo, motoArNo);
            frm.Icon = this.Icon;
            frm.FormClosed += new FormClosedEventHandler(ARJohoMeisai_FormClosed);
            frm.Show();
            this._modelessNum++;
            return frm;
        }
        #endregion

        #region 複数行表示のフォーマット設定

        /// --------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="colType"></param>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <param name="colSpan"></param>
        /// <param name="rowSpan"></param>
        /// <param name="dataField"></param>
        /// <param name="isEnabled"></param>
        /// <param name="isMultiLine">TextEditorのみ有効</param>
        /// <param name="actions"></param>
        /// <create>M.Tsutsumi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetFormat(MultiRowFormat format, ElTabelleColumnType colType, int col, int row, int colSpan, int rowSpan, string dataField, bool isEnabled, bool isMultiLine, KeyAction[] actions)
        {
            string buttonText = string.Empty;
            if (colType == ElTabelleColumnType.Button)
            {
                buttonText = dataField;
                dataField = "";
            }

            MultiRowCell cell = new MultiRowCell(col, colSpan, rowSpan, dataField, actions);

            switch (colType)
            {
                case ElTabelleColumnType.Text:
                    TextEditor textEditor = ElTabelleSheetHelper.NewTextEditor();
                    textEditor.MultiLine = isMultiLine;
                    textEditor.WordWrap = false;
                    cell.Editor = textEditor;
                    break;
                case ElTabelleColumnType.Number:
                    NumberEditor numberEditor = ElTabelleSheetHelper.NewNumberEditor();
                    numberEditor.DisplayFormat = new NumberFormat("###,###,###", string.Empty, string.Empty, "-", string.Empty, "-", string.Empty);
                    cell.Editor = numberEditor;
                    break;
                case ElTabelleColumnType.Button:
                    ButtonEditor buttonEditor = ElTabelleSheetHelper.NewButtonEditor();
                    buttonEditor.Text = buttonText;
                    cell.Editor = buttonEditor;
                    break;
            }
            cell.Enabled = isEnabled;
            format.Rows[row].Cells.Add(cell);
        }

        #endregion

        #region モード切替操作

        /// --------------------------------------------------
        /// <summary>
        /// モード切替操作
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/10</create>
        /// <update>M.Tsutsumi 2011/02/16</update>
        /// --------------------------------------------------
        private void ChangeMode()
        {
            try
            {
                if (this.rdoInsert.Checked)
                {
                    // 新規登録
                    // 画面の編集モード変更
                    this.EditMode = SystemBase.EditMode.Insert;
                    // ボタンの切替
                    this.btnView.Enabled = false;
                    //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36
                    // リスト選択
                    this.txtListFlagName0.ReadOnly = false;
                    this.txtListFlagName1.ReadOnly = false;
                    this.txtListFlagName2.ReadOnly = false;
                    this.txtListFlagName3.ReadOnly = false;
                    this.txtListFlagName4.ReadOnly = false;
                    this.txtListFlagName5.ReadOnly = false;
                    this.txtListFlagName6.ReadOnly = false;
                    this.txtListFlagName7.ReadOnly = false;
                    this.txtListFlagName0.BackColor = SystemColors.Window;
                    this.txtListFlagName1.BackColor = SystemColors.Window;
                    this.txtListFlagName2.BackColor = SystemColors.Window;
                    this.txtListFlagName3.BackColor = SystemColors.Window;
                    this.txtListFlagName4.BackColor = SystemColors.Window;
                    this.txtListFlagName5.BackColor = SystemColors.Window;
                    this.txtListFlagName6.BackColor = SystemColors.Window;
                    this.txtListFlagName7.BackColor = SystemColors.Window;
                    // @@@ ↑
                    // 検索項目
                    this.SearchClear();
                    this.cboJyokyoFlag.Enabled = false;
                    this.txtTaisaku.Enabled = false;
                    this.txtGirenNo.Enabled = false;
                    this.txtGenchiTehaisaki.Enabled = false;
                    // シートクリア
                    this.SheetClear();
                    // ファンクションボタンの切替
                    this.ChangeFunctionButton(false);

                    this._bukkenNo = string.Empty;
                }
                else if (this.rdoUpdate.Checked)
                {
                    // 編集
                    // 画面の編集モード変更
                    this.EditMode = SystemBase.EditMode.Update;
                    // ボタンの切替
                    this.btnView.Enabled = true;
                    //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36
                    // リスト選択
                    this.txtListFlagName0.ReadOnly = true;
                    this.txtListFlagName1.ReadOnly = true;
                    this.txtListFlagName2.ReadOnly = true;
                    this.txtListFlagName3.ReadOnly = true;
                    this.txtListFlagName4.ReadOnly = true;
                    this.txtListFlagName5.ReadOnly = true;
                    this.txtListFlagName6.ReadOnly = true;
                    this.txtListFlagName7.ReadOnly = true;
                    this.txtListFlagName0.BackColor = SystemColors.Control;
                    this.txtListFlagName1.BackColor = SystemColors.Control;
                    this.txtListFlagName2.BackColor = SystemColors.Control;
                    this.txtListFlagName3.BackColor = SystemColors.Control;
                    this.txtListFlagName4.BackColor = SystemColors.Control;
                    this.txtListFlagName5.BackColor = SystemColors.Control;
                    this.txtListFlagName6.BackColor = SystemColors.Control;
                    this.txtListFlagName7.BackColor = SystemColors.Control;
                    // @@@ ↑
                    // 検索項目
                    this.txtNonyusakiName.Text = string.Empty;
                    this.txtNonyusakiCd.Text = string.Empty;
                    this.cboJyokyoFlag.Enabled = true;
                    this.txtTaisaku.Enabled = true;
                    this.txtGirenNo.Enabled = true;
                    this.txtGenchiTehaisaki.Enabled = true;
                    // ファンクションボタンの切替
                    this.ChangeFunctionButton(true);

                    this._bukkenNo = string.Empty;
                }
                else
                {
                }
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
        /// <param name="isEnabled"></param>
        /// <create>M.Tsutsumi 2010/08/27</create>
        /// <update>H.Tajimi 2018/10/15 FE要望対応</update>
        /// <update>D.Okumura 2019/08/15 AR進捗対応</update>
        /// --------------------------------------------------
        private void ChangeFunctionButton(bool isEnabled)
        {
            // Excelボタン
            //if (!isEnabled)
            //{
            //    this.fbrFunction.F10Button.Text = string.Empty;
            //}
            //else
            //{
            //    this.fbrFunction.F10Button.Text = "Excel";
            //}
            this.fbrFunction.F04Button.Enabled = isEnabled;
            this.fbrFunction.F05Button.Enabled = isEnabled;
            this.fbrFunction.F07Button.Enabled = isEnabled;
            this.fbrFunction.F08Button.Enabled = isEnabled;
            this.fbrFunction.F09Button.Enabled = isEnabled;
            this.fbrFunction.F10Button.Enabled = isEnabled;
        }

        #endregion

        #region コンディション取得

        /// --------------------------------------------------
        /// <summary>
        /// コンディション取得
        /// </summary>
        /// <returns>コンディション</returns>
        /// <create>M.Tsutsumi 2010/08/12</create>
        /// <update>H.Tajimi 2018/10/15 FE要望</update>
        /// --------------------------------------------------
        private CondA1 GetCondition()
        {
            CondA1 cond = new CondA1(this.UserInfo);

            cond.NonyusakiCD = this.txtNonyusakiCd.Text;
            cond.ShukkaFlag = SHUKKA_FLAG.AR_VALUE1;
            cond.ListFlag = GetListFlag();
            cond.JyokyoFlag = string.Empty;

            if (this.cboJyokyoFlag.Enabled &&
                this.cboJyokyoFlag.SelectedIndex != -1)
            {
                cond.JyokyoFlagAR = this.cboJyokyoFlag.SelectedValue.ToString();
            }
            else
            {
                cond.JyokyoFlagAR = string.Empty;
            }

            if (this.txtTaisaku.Enabled &&
                !string.IsNullOrEmpty(this.txtTaisaku.Text))
            {
                cond.Taisaku = this.txtTaisaku.Text;
            }

            if (this.txtGirenNo.Enabled &&
                !string.IsNullOrEmpty(this.txtGirenNo.Text))
            {
                cond.GirenNo = this.txtGirenNo.Text;
            }

            if (this.txtGenchiTehaisaki.Enabled &&
                !string.IsNullOrEmpty(this.txtGenchiTehaisaki.Text))
            {
                cond.GenchiTehaisaki = this.txtGenchiTehaisaki.Text;
            }

            cond.UpdateUserID = this.UserInfo.UserID;

            return cond;
        }

        #region リスト選択

        /// --------------------------------------------------
        /// <summary>
        /// リスト選択の設定値取得
        /// </summary>
        /// <returns></returns>
        /// <create>M.Tsutsumi 2010/08/20</create>
        /// <update>M.Tsutsumi 2011/02/16</update>
        /// --------------------------------------------------
        private string GetListFlag()
        {
            string listFlag;

            if (rdoListFlag1.Checked)
            {
                // [1]設計⇒現地
                listFlag = LIST_FLAG.FLAG_1_VALUE1;
            }
            else if (rdoListFlag2.Checked)
            {
                // [2]現場⇒工場
                listFlag = LIST_FLAG.FLAG_2_VALUE1;
            }
            else if (rdoListFlag3.Checked)
            {
                // [3]現場⇒工場(手配以外)
                listFlag = LIST_FLAG.FLAG_3_VALUE1;
            }
            else if (rdoListFlag4.Checked)
            {
                // [4]現場⇒現地工場
                listFlag = LIST_FLAG.FLAG_4_VALUE1;
            }
            else if (rdoListFlag5.Checked)
            {
                // [5]現地工場⇒現場
                listFlag = LIST_FLAG.FLAG_5_VALUE1;
            }
            //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36
            else if (rdoListFlag6.Checked)
            {
                // [6]
                listFlag = LIST_FLAG.FLAG_6_VALUE1;
            }
            else if (rdoListFlag7.Checked)
            {
                // [7]
                listFlag = LIST_FLAG.FLAG_7_VALUE1;
            }
            // @@@ ↑
            else
            {
                // [0]製造⇒現地
                listFlag = LIST_FLAG.FLAG_0_VALUE1;
            }

            return listFlag;
        }


        /// --------------------------------------------------
        /// <summary>
        /// ListFlagに紐づく名称を取得
        /// </summary>
        /// <param name="listFlag">リスト区分</param>
        /// <returns>リスト区分名称</returns>
        /// <create>D.Okumura 2019/12/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private string GetListFlagName(string listFlag)
        {
            if (listFlag == LIST_FLAG.FLAG_0_VALUE1)
                return this.txtListFlagName0.Text;
            else if (listFlag == LIST_FLAG.FLAG_1_VALUE1)
                return this.txtListFlagName1.Text;
            else if (listFlag == LIST_FLAG.FLAG_2_VALUE1)
                return this.txtListFlagName2.Text;
            else if (listFlag == LIST_FLAG.FLAG_3_VALUE1)
                return this.txtListFlagName3.Text;
            else if (listFlag == LIST_FLAG.FLAG_4_VALUE1)
                return this.txtListFlagName4.Text;
            else if (listFlag == LIST_FLAG.FLAG_5_VALUE1)
                return this.txtListFlagName5.Text;
            else if (listFlag == LIST_FLAG.FLAG_6_VALUE1)
                return this.txtListFlagName6.Text;
            else if (listFlag == LIST_FLAG.FLAG_7_VALUE1)
                return this.txtListFlagName7.Text;
            return string.Empty;
        }
        #endregion

        #endregion

        #region 納入先マスタ存在チェック

        /// --------------------------------------------------
        /// <summary>
        /// 納入先マスタ存在チェック
        /// </summary>
        /// <returns></returns>
        /// <create>M.Tsutsumi 2010/08/12</create>
        /// <update>M.Tsutsumi 2011/02/16</update>
        /// --------------------------------------------------
        private bool CheckNonyusaki()
        {
            CondA1 cond = this.GetCondition();
            ConnA01 conn = new ConnA01();

            // 初期化
            this._nonyusakiComplete = false;

            if (this.rdoUpdate.Checked)
            {
                // 編集
                if (!conn.IsExistenceNonyusaki(cond))
                {
                    // 該当する納入先はありません。
                    this.ShowMessage("A9999999044");
                    return false;
                }

                // 納入先取得
                DataSet ds = conn.GetNonyusaki(cond);
                if (ds == null || !ds.Tables.Contains(Def_M_NONYUSAKI.Name) ||
                                    ds.Tables[Def_M_NONYUSAKI.Name].Rows.Count < 1)
                {
                    // 該当する納入先はありません。
                    this.ShowMessage("A9999999044");
                    return false;
                }

                // 管理区分チェック
                string kanriFlag = ComFunc.GetFld(ds, Def_M_NONYUSAKI.Name, 0, Def_M_NONYUSAKI.KANRI_FLAG);
                if (kanriFlag == KANRI_FLAG.KANRYO_VALUE1)
                {
                    this._nonyusakiComplete = true;
                }

                this.txtNonyusakiCd.Text = ComFunc.GetFld(ds, Def_M_NONYUSAKI.Name, 0, Def_M_NONYUSAKI.NONYUSAKI_CD);

                //@@@ 2011/03/04 M.Tsutsumi Add 
                // リスト区分再設定
                this.txtListFlagName0.Text = ComFunc.GetFld(ds, Def_M_NONYUSAKI.Name, 0, Def_M_NONYUSAKI.LIST_FLAG_NAME0);
                this.txtListFlagName1.Text = ComFunc.GetFld(ds, Def_M_NONYUSAKI.Name, 0, Def_M_NONYUSAKI.LIST_FLAG_NAME1);
                this.txtListFlagName2.Text = ComFunc.GetFld(ds, Def_M_NONYUSAKI.Name, 0, Def_M_NONYUSAKI.LIST_FLAG_NAME2);
                this.txtListFlagName3.Text = ComFunc.GetFld(ds, Def_M_NONYUSAKI.Name, 0, Def_M_NONYUSAKI.LIST_FLAG_NAME3);
                this.txtListFlagName4.Text = ComFunc.GetFld(ds, Def_M_NONYUSAKI.Name, 0, Def_M_NONYUSAKI.LIST_FLAG_NAME4);
                this.txtListFlagName5.Text = ComFunc.GetFld(ds, Def_M_NONYUSAKI.Name, 0, Def_M_NONYUSAKI.LIST_FLAG_NAME5);
                this.txtListFlagName6.Text = ComFunc.GetFld(ds, Def_M_NONYUSAKI.Name, 0, Def_M_NONYUSAKI.LIST_FLAG_NAME6);
                this.txtListFlagName7.Text = ComFunc.GetFld(ds, Def_M_NONYUSAKI.Name, 0, Def_M_NONYUSAKI.LIST_FLAG_NAME7);
                // @@@ ↑
            }
            return true;
        }

        #endregion

        #region リスト選択チェック

        /// --------------------------------------------------
        /// <summary>
        /// リスト区分の選択チェック
        /// </summary>
        /// <returns></returns>
        /// <create>M.Tsutsumi 2011/02/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckListFlag()
        {
            if (this.rdoListFlag0.Checked)
            {
                if (string.IsNullOrEmpty(this.txtListFlagName0.Text))
                {
                    // List区分名称の無いものは選択できません。
                    this.ShowMessage("A0100010006");
                    return false;
                }
            }
            else if (this.rdoListFlag1.Checked)
            {
                if (string.IsNullOrEmpty(this.txtListFlagName1.Text))
                {
                    // List区分名称の無いものは選択できません。
                    this.ShowMessage("A0100010006");
                    return false;
                }
            }
            else if (this.rdoListFlag2.Checked)
            {
                if (string.IsNullOrEmpty(this.txtListFlagName2.Text))
                {
                    // List区分名称の無いものは選択できません。
                    this.ShowMessage("A0100010006");
                    return false;
                }
            }
            else if (this.rdoListFlag3.Checked)
            {
                if (string.IsNullOrEmpty(this.txtListFlagName3.Text))
                {
                    // List区分名称の無いものは選択できません。
                    this.ShowMessage("A0100010006");
                    return false;
                }
            }
            else if (this.rdoListFlag4.Checked)
            {
                if (string.IsNullOrEmpty(this.txtListFlagName4.Text))
                {
                    // List区分名称の無いものは選択できません。
                    this.ShowMessage("A0100010006");
                    return false;
                }
            }
            else if (this.rdoListFlag5.Checked)
            {
                if (string.IsNullOrEmpty(this.txtListFlagName5.Text))
                {
                    // List区分名称の無いものは選択できません。
                    this.ShowMessage("A0100010006");
                    return false;
                }
            }
            else if (this.rdoListFlag6.Checked)
            {
                if (string.IsNullOrEmpty(this.txtListFlagName6.Text))
                {
                    // List区分名称の無いものは選択できません。
                    this.ShowMessage("A0100010006");
                    return false;
                }
            }
            else if (this.rdoListFlag7.Checked)
            {
                if (string.IsNullOrEmpty(this.txtListFlagName7.Text))
                {
                    // List区分名称の無いものは選択できません。
                    this.ShowMessage("A0100010006");
                    return false;
                }
            }
            else
            {
                // List区分が選択されていません。
                this.ShowMessage("A0100010007");
                return false;
            }

            return true;
        }

        #endregion

        #region Excel出力
        #endregion

        #region 検索結果の再描画

        /// --------------------------------------------------
        /// <summary>
        /// 検索結果の再描画
        /// </summary>
        /// <create>T.Sakiori 2012/04/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetResearch(CondA1 cond)
        {
            try
            {
                this.RunSearch();

                var dt = this.shtMeisai.GetMultiRowGetDataSource();
                if (dt == null)
                {
                    return;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (ComFunc.GetFld(dt, i, Def_T_AR.NONYUSAKI_CD) == cond.NonyusakiCD
                        && ComFunc.GetFld(dt, i, Def_M_NONYUSAKI.NONYUSAKI_NAME) == cond.NonyusakiName
                        && ComFunc.GetFld(dt, i, Def_T_AR.LIST_FLAG) == cond.ListFlag
                        && ComFunc.GetFld(dt, i, Def_T_AR.AR_NO) == cond.ArNo
                        && ComFunc.GetFld(dt, i, Def_M_NONYUSAKI.LIST_FLAG_NAME0) == cond.ListFlagName0
                        && ComFunc.GetFld(dt, i, Def_M_NONYUSAKI.LIST_FLAG_NAME1) == cond.ListFlagName1
                        && ComFunc.GetFld(dt, i, Def_M_NONYUSAKI.LIST_FLAG_NAME2) == cond.ListFlagName2
                        && ComFunc.GetFld(dt, i, Def_M_NONYUSAKI.LIST_FLAG_NAME3) == cond.ListFlagName3
                        && ComFunc.GetFld(dt, i, Def_M_NONYUSAKI.LIST_FLAG_NAME4) == cond.ListFlagName4
                        && ComFunc.GetFld(dt, i, Def_M_NONYUSAKI.LIST_FLAG_NAME5) == cond.ListFlagName5
                        && ComFunc.GetFld(dt, i, Def_M_NONYUSAKI.LIST_FLAG_NAME6) == cond.ListFlagName6
                        && ComFunc.GetFld(dt, i, Def_M_NONYUSAKI.LIST_FLAG_NAME7) == cond.ListFlagName7)
                    {
                        this.shtMeisai.Redraw = false;
                        this.shtMeisai.TopLeft = new Position(0, i * SHEET_ROW_BLOCK);
                        this.shtMeisai.ActivePosition = new Position(0, i * SHEET_ROW_BLOCK);
                        this.shtMeisai.Redraw = true;
                    }
                    this.shtMeisai.Focus();
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region メール情報チェック

        /// --------------------------------------------------
        /// <summary>
        /// メール情報チェック
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="isToroku">明細登録ならtrue、編集ならfalse</param>
        /// <returns></returns>
        /// <create>R.Katsuo 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckMailSend(CondA1 cond, bool isToroku)
        {
            try
            {
                cond.IsToroku = isToroku;
                cond.UpdateUserID = this.UserInfo.UserID;
                cond.BukkenNo = this._bukkenNo;
                
                string errMsgID;
                string[] args;
                var ds = new ConnA01().GetSendMailInfo(cond, out errMsgID, out args);
                if (!string.IsNullOrEmpty(errMsgID))
                {
                    this.ShowMessage(errMsgID, args);
                    return false;
                }

                // 2018/09/06 H.Tajimi 使用している形跡がないためコメントアウト
                //cond.MailAddress = UtilData.GetFld(ds, Def_M_USER.Name, 0, Def_M_USER.MAIL_ADDRESS);
                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region AR7000番台チェック
        /// --------------------------------------------------
        /// <summary>
        /// AR7000番台運用かチェック
        /// </summary>
        /// <returns>true:チェックOK/false:チェックNG</returns>
        /// <create>T.Nukaga 2019/11/20 AR7000番運用対応</create>
        /// <update>D.Okumura 2020/01/23 AR7000番運用明細登録警告対応</update>
        /// --------------------------------------------------
        private bool CheckARRelation()
        {
            string selFlg = GetListFlag();
            // 対象が見つからない場合チェックOKとする
            if (!_listFlagNotify.ContainsKey(selFlg))
            {
                return true;
            }
            var listItem = _listFlagNotify[selFlg];
            string value2 = ComFunc.GetFld(listItem, Def_M_COMMON.VALUE2);
            string value3 = ComFunc.GetFld(listItem, Def_M_COMMON.VALUE3);
            string name = ComFunc.GetFld(listItem, Def_M_COMMON.ITEM_NAME);
            // 元AR運用対象外の場合チェックOKとする
            if (value2 != LIST_FLAG_KEKKA_VALUE2.REF_MOTO_AR_VALUE1 || string.IsNullOrEmpty(GetListFlagName(value3)))
            {
                return true;
            }

            // AR7000番運用対象です。明細登録を行いますか？
            DialogResult ret = this.ShowMessage("A0100010015", name, value3);
            if (ret == DialogResult.No)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 一覧の列表示切替
        /// </summary>
        /// <create>T.Nukaga 2019/11/21 STEP12 AR7000番運用対応</create>
        /// <update>D.Okumura 2019/12/16 STEP12 AR7000番運用動的対応</update>
        /// --------------------------------------------------
        private void SetShtMeisaiColumnVisible()
        {
            string selFlg = GetListFlag();

            if (!_listFlagKekka.ContainsKey(selFlg))
            {
                shtMeisai.Columns[SHEET_COL_MOTO_ARNO].Hidden = true;
                shtMeisai.Columns[SHEET_COL_KEKKA_ARNO].Hidden = true;
                shtMeisai.Columns[SHEET_COL_KEKKA_BUTTON].Hidden = true;
                return;
            }
            var listItem = _listFlagKekka[selFlg];
            string value1 = ComFunc.GetFld(listItem, Def_M_COMMON.VALUE1);
            string value2 = ComFunc.GetFld(listItem, Def_M_COMMON.VALUE2);
            string value3 = ComFunc.GetFld(listItem, Def_M_COMMON.VALUE3);
            if (value2 == LIST_FLAG_KEKKA_VALUE2.REF_KEKKA_AR_VALUE1 && !string.IsNullOrEmpty(GetListFlagName(value1)))
            {
                shtMeisai.Columns[SHEET_COL_MOTO_ARNO].Hidden = true;
                shtMeisai.Columns[SHEET_COL_KEKKA_ARNO].Hidden = false;
                shtMeisai.Columns[SHEET_COL_KEKKA_BUTTON].Hidden = false;
            }
            else if (value2 == LIST_FLAG_KEKKA_VALUE2.REF_MOTO_AR_VALUE1 && !string.IsNullOrEmpty(GetListFlagName(value3)))
            {
                shtMeisai.Columns[SHEET_COL_MOTO_ARNO].Hidden = false;
                shtMeisai.Columns[SHEET_COL_KEKKA_ARNO].Hidden = true;
                shtMeisai.Columns[SHEET_COL_KEKKA_BUTTON].Hidden = true;
            }
            else
            {
                shtMeisai.Columns[SHEET_COL_MOTO_ARNO].Hidden = true;
                shtMeisai.Columns[SHEET_COL_KEKKA_ARNO].Hidden = true;
                shtMeisai.Columns[SHEET_COL_KEKKA_BUTTON].Hidden = true;
            }
        }

        #endregion
    }
}
