using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using DSWUtil;
using DSWControl.DSWComboBox;
using WsConnection;
using WsConnection.WebRefCommon;
using Commons;
using SystemBase.Controls;
using SystemBase.Util;
using GrapeCity.Win.ElTabelle;
using GrapeCity.Win.ElTabelle.Editors;
using ElTabelleHelper;

using MultiRowTabelle;

using WsConnection.WebRefK03;
using SMS.P02.Forms;
using WsConnection.WebRefAttachFile;
using SMS.K03.Properties;

namespace SMS.K03.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 木枠梱包明細登録
    /// </summary>
    /// <create>M.Tsutsumi 2010/07/13</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class Kiwaku : SystemBase.Forms.CustomOrderForm
    {

        #region 定義

        /// --------------------------------------------------
        /// <summary>
        /// CASEMARKのファイル名バイトサイズ
        /// </summary>
        /// <create>M.Tsutsumi 2010/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int CASEMARK_FILE_NAME_SIZE = 25;

        /// --------------------------------------------------
        /// <summary>
        /// まるちろうのブロック行数
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_ROW_BLOCK = 2;

        /// --------------------------------------------------
        /// <summary>
        /// C/NOセル
        /// </summary>
        /// <create>M.Tsutsumi 2010/07/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_CASE_NO = 0;

        /// --------------------------------------------------
        /// <summary>
        /// DIMENSION(L)セル
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/02</create>
        /// <update>H.Tajimi 2015/11/25 ケースナンバーの欠番対応</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_DIMENSION_L = 4;

        /// --------------------------------------------------
        /// <summary>
        /// DIMENSION(W)セル
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_DIMENSION_W = 5;

        /// --------------------------------------------------
        /// <summary>
        /// DIMENSION(H)セル
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_DIMENSION_H = 6;

        /// --------------------------------------------------
        /// <summary>
        /// MMNETセル
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_MMNET = 7;

        /// --------------------------------------------------
        /// <summary>
        /// 木材重量セル
        /// </summary>
        /// <create>M.Tsutsumi 2010/09/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_MOKUZAI_JYURYO = 8;

        /// --------------------------------------------------
        /// <summary>
        /// 印刷C/NOセル
        /// </summary>
        /// <create>H.Tajimi 2015/11/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_PRINT_CASE_NO = 9;

        /// --------------------------------------------------
        /// <summary>
        /// 工事識別管理NOセル
        /// </summary>
        /// <create>M.Tsutsumi 2010/09/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_KOJI_NO = 10;

        /// --------------------------------------------------
        /// <summary>
        /// 内部管理用キーセル
        /// </summary>
        /// <create>M.Tsutsumi 2010/09/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_CASE_ID = 11;

        /// --------------------------------------------------
        /// <summary>
        /// 出荷日付セル
        /// </summary>
        /// <create>M.Tsutsumi 2010/07/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SHUKKA_DATE = 22;

        /// --------------------------------------------------
        /// <summary>
        /// パレットNO(1)セル
        /// </summary>
        /// <create>M.Tsutsumi 2010/07/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_PALLET_NO_1 = 12;

        /// --------------------------------------------------
        /// <summary>
        /// パレットNO(2)セル
        /// </summary>
        /// <create>M.Tsutsumi 2010/07/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_PALLET_NO_2 = 13;

        /// --------------------------------------------------
        /// <summary>
        /// パレットNO(3)セル
        /// </summary>
        /// <create>M.Tsutsumi 2010/07/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_PALLET_NO_3 = 14;

        /// --------------------------------------------------
        /// <summary>
        /// パレットNO(4)セル
        /// </summary>
        /// <create>M.Tsutsumi 2010/07/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_PALLET_NO_4 = 15;

        /// --------------------------------------------------
        /// <summary>
        /// パレットNO(5)セル
        /// </summary>
        /// <create>M.Tsutsumi 2010/07/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_PALLET_NO_5 = 16;

        /// --------------------------------------------------
        /// <summary>
        /// パレットNO(6)セル
        /// </summary>
        /// <create>M.Tsutsumi 2010/07/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_PALLET_NO_6 = 17;

        /// --------------------------------------------------
        /// <summary>
        /// パレットNO(7)セル
        /// </summary>
        /// <create>M.Tsutsumi 2010/07/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_PALLET_NO_7 = 18;

        /// --------------------------------------------------
        /// <summary>
        /// パレットNO(8)セル
        /// </summary>
        /// <create>M.Tsutsumi 2010/07/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_PALLET_NO_8 = 19;

        /// --------------------------------------------------
        /// <summary>
        /// パレットNO(9)セル
        /// </summary>
        /// <create>M.Tsutsumi 2010/07/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_PALLET_NO_9 = 20;

        /// --------------------------------------------------
        /// <summary>
        /// パレットNO(10)セル
        /// </summary>
        /// <create>M.Tsutsumi 2010/07/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_PALLET_NO_10 = 21;

        /// --------------------------------------------------
        /// <summary>
        /// NET/Wセル
        /// </summary>
        /// <create>M.Tsutsumi 2010/09/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_NET_W = 24;

        /// --------------------------------------------------
        /// <summary>
        /// GROSS/Wセル
        /// </summary>
        /// <create>M.Tsutsumi 2010/09/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_GROSS_W = 25;

        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>Y.Higuchi 2010/09/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 1;

        #endregion

        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// ElTabelleのカラムタイプ
        /// </summary>
        /// <create>M.Tsutsumi 2010/09/15</create>
        /// <update></update>
        /// --------------------------------------------------
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
        /// <create>M.Tsutsumi 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private MultiRowFormat _multiRowFormat = null;

        /// --------------------------------------------------
        /// <summary>
        /// 木枠データ(取得待避用)
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtKiwaku = null;

        /// --------------------------------------------------
        /// <summary>
        /// 木枠明細データ(取得待避用)
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtKiwakuMeisai = null;

        /// --------------------------------------------------
        /// <summary>
        /// 社外用木枠明細データ(取得待避用)
        /// </summary>
        /// <create>M.Tsutsumi 2010/09/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtShagaiKiwakuMeisai = null;

        /// --------------------------------------------------
        /// <summary>
        /// 再描画フラグ
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _displayRedraw = false;

        /// --------------------------------------------------
        /// <summary>
        /// CASEMARKFILE
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _delFileName = string.Empty;

        // デフォルトメッセージID
        private string _defMsgInsertEnd = string.Empty;
        private string _defMsgUpdateEnd = string.Empty;

        /// --------------------------------------------------
        /// <summary>
        /// 物件管理No
        /// </summary>
        /// <create>H.Tajimi 2018/12/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _bukkenNo = string.Empty;

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
        /// <create>M.Tsutsumi 2010/07/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public Kiwaku(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
            : base(userInfo, menuCategoryID, menuItemID, title)
        {
            InitializeComponent();
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ（デザイナ用）
        /// </summary>
        /// <create>M.Tsutsumi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private Kiwaku()
            : this(new UserInfo(), "", "", "")
        {
        }

        #endregion

        #region Properties

        /// --------------------------------------------------
        /// <summary>
        /// 登録区分
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual string TorokuFlag
        {
            get { return TOROKU_FLAG.NAI_VALUE1; }
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>M.Tsutsumi 2010/07/13</create>
        /// <update></update>
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

                // メッセージを待避
                // 登録完了メッセージ
                this._defMsgInsertEnd = this.MsgInsertEnd;
                // 修正完了メッセージ
                this._defMsgUpdateEnd = this.MsgUpdateEnd;

                // フォームの設定
                this.IsCloseQuestion = true;

                // シートの初期化
                this.InitializeSheet(this.shtMeisai);

                // シートのタイトルを設定
                shtMeisai.ColumnHeaders[0, 0].Caption = Resources.Kiwaku_CNo;
                shtMeisai.ColumnHeaders[1, 0].Caption = Resources.Kiwaku_PkgStyle;
                shtMeisai.ColumnHeaders[2, 0].Caption = Resources.Kiwaku_Item;
                shtMeisai.ColumnHeaders[3, 0].Caption = Resources.Kiwaku_Description;
                shtMeisai.ColumnHeaders[4, 0].Caption = Resources.Kiwaku_Dimension;
                shtMeisai.ColumnHeaders[4, 1].Caption = Resources.Kiwaku_L;
                shtMeisai.ColumnHeaders[5, 1].Caption = Resources.Kiwaku_W;
                shtMeisai.ColumnHeaders[6, 1].Caption = Resources.Kiwaku_H;
                shtMeisai.ColumnHeaders[7, 0].Caption = Resources.Kiwaku_Mment;
                shtMeisai.ColumnHeaders[8, 0].Caption = Resources.Kiwaku_WoodWeight;
                shtMeisai.ColumnHeaders[9, 0].Caption = Resources.Kiwaku_PrintCNo;

                // テキストの初期化
                this.InitializeText();

                // モード切替
                this.ChangeMode();
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
        /// <create>M.Tsutsumi 2010/07/13</create>
        /// <update>K.Tsutsumi 2018/01/20 AR対応</update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // ここに初期フォーカスの設定を記述する。
                // フォーカス設定
                this.rdoInsertEx.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #region Sheet初期化

        /// --------------------------------------------------
        /// <summary>
        /// Sheet初期化
        /// </summary>
        /// <param name="sheet"></param>
        /// <create>M.Tsutsumi 2010/07/13</create>
        /// <update>H.Tajimi 2015/11/24 Description対応</update>
        /// <update>H.Tajimi 2015/11/25 ケースナンバーの欠番対応</update>
        /// <update>H.Tajimi 2015/11/25 ケースナンバーの欠番対応</update>
        /// <update>2023/12/12 J.Chen 小数点第2位まで対応、桁数変更</update>
        /// --------------------------------------------------
        protected override void InitializeSheet(Sheet sheet)
        {
            base.InitializeSheet(sheet);

            try
            {
                // フォーマット管理クラス生成
                this._multiRowFormat = new MultiRowFormat();

                // １レコード内の行数指定
                this._multiRowFormat.SetRowNum(2);

                // セルの設定
                int colIndex = 0;
                // 2015/11/25 H.Tajimi ケースナンバーの欠番対応 Enableをfalse->true、MaxLengthをなし->3に変更
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Number, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.CASE_NO, "###,###,###", true, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions), 3);
                // ↑
                // 2015/11/25 H.Tajimi 印刷C/NO追加及び、カラム位置を変数使用に変更
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.STYLE, "Aa9@", true, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions), 5);
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.ITEM, "Aa9@", true, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions), 30);
                // 2015/11/24 H.Tajimi Description対応 Enableをtrue->false、MaxLengthを30->なしに変更
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex, 0, 1, 1, Def_T_KIWAKU_MEISAI.DESCRIPTION_1, "Aa9@", false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                // ↑
                // 2023/12/12 J.Chen MaxLengthを30->100に変更
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 1, 1, 1, Def_T_KIWAKU_MEISAI.DESCRIPTION_2, "Aa9@", true, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions), 100);
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Number, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.DIMENSION_L, "#,###", true, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions), 4);
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Number, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.DIMENSION_W, "#,###", true, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions), 4);
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Number, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.DIMENSION_H, "#,###", true, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions), 4);
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Number, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.MMNET, "###,##0.000", false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                // 2023/12/12 J.Chen 小数点第2位まで対応に変更
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Number, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.MOKUZAI_JYURYO, "##,##0.00", true, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions), 5);
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.PRINT_CASE_NO, "^Ｔ", true, ImeMode.Off, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions), 14, true);

                // 非表示セルの設定
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.KOJI_NO, null, false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.CASE_ID, null, false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.PALLET_NO_1, null, false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.PALLET_NO_2, null, false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.PALLET_NO_3, null, false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.PALLET_NO_4, null, false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.PALLET_NO_5, null, false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.PALLET_NO_6, null, false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.PALLET_NO_7, null, false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.PALLET_NO_8, null, false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.PALLET_NO_9, null, false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.PALLET_NO_10, null, false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.SHUKKA_DATE, null, false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.VERSION, null, false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                // 2023/12/12 J.Chen 小数点第2位まで対応に変更
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Number, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.NET_W, "##,##0.00", false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Number, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.GROSS_W, "##,##0.00", false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.SHUKKA_USER_ID, null, false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.SHUKKA_USER_NAME, null, false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                // 2015/11/25 H.Tajimi 印刷C/NO追加及び、カラム位置を変数使用に変更
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region Text初期化

        /// --------------------------------------------------
        /// <summary>
        /// Text初期化
        /// </summary>
        /// <create>M.Tsutsumi 2010/07/30</create>
        /// <update>H.Tajimi 2015/11/17 工事Noを物件名と同様の入力規制にする</update>
        /// <update>J.Chen 2023/02/22 工事No桁数40→60に変更</update>
        /// --------------------------------------------------
        private void InitializeText()
        {
            try
            {
                txtKojiName.ImeMode = ImeMode.Off;
                txtKojiName.InputRegulation = "F";
                txtKojiName.IsInputRegulation = false;
                txtKojiName.MaxByteLengthMode = true;
                txtKojiName.MaxLength = 60;
                txtKojiName.ProhibitionChar = null;

                txtShip.ImeMode = ImeMode.Off;
                txtShip.InputRegulation = "F";
                txtShip.IsInputRegulation = false;
                txtShip.MaxByteLengthMode = true;
                txtShip.MaxLength = 10;

                txtKojiNo.ImeMode = ImeMode.Disable;
                txtKojiNo.InputRegulation = "n";
                txtKojiNo.IsInputRegulation = true;
                txtKojiNo.MaxByteLengthMode = true;
                txtKojiNo.MaxLength = 4;

                txtCaseMark.ImeMode = ImeMode.Disable;
                txtCaseMark.InputRegulation = "abnls";
                txtCaseMark.IsInputRegulation = true;
                txtCaseMark.MaxByteLengthMode = true;
                txtCaseMark.MaxLength = 25;

                txtDeliveryNo.ImeMode = ImeMode.Disable;
                txtDeliveryNo.InputRegulation = "abnls";
                txtDeliveryNo.IsInputRegulation = true;
                txtDeliveryNo.MaxByteLengthMode = true;
                txtDeliveryNo.MaxLength = 20;

                txtPortOfDestination.ImeMode = ImeMode.Disable;
                txtPortOfDestination.InputRegulation = "abnls";
                txtPortOfDestination.IsInputRegulation = true;
                txtPortOfDestination.MaxByteLengthMode = true;
                txtPortOfDestination.MaxLength = 20;

                txtAirBoat.ImeMode = ImeMode.Disable;
                txtAirBoat.InputRegulation = "abnls";
                txtAirBoat.IsInputRegulation = true;
                txtAirBoat.MaxByteLengthMode = true;
                txtAirBoat.MaxLength = 20;

                txtDeliveryDate.ImeMode = ImeMode.Disable;
                txtDeliveryDate.InputRegulation = "abnls";
                txtDeliveryDate.IsInputRegulation = true;
                txtDeliveryDate.MaxByteLengthMode = true;
                txtDeliveryDate.MaxLength = 20;

                txtDeliveryPoint.ImeMode = ImeMode.Disable;
                txtDeliveryPoint.InputRegulation = "abnls";
                txtDeliveryPoint.IsInputRegulation = true;
                txtDeliveryPoint.MaxByteLengthMode = true;
                txtDeliveryPoint.MaxLength = 20;

                txtFactory.ImeMode = ImeMode.Disable;
                txtFactory.InputRegulation = "abnls";
                txtFactory.IsInputRegulation = true;
                txtFactory.MaxByteLengthMode = true;
                txtFactory.MaxLength = 20;

                txtRemarks.ImeMode = ImeMode.Disable;
                txtRemarks.InputRegulation = "abnls";
                txtRemarks.IsInputRegulation = true;
                txtRemarks.MaxByteLengthMode = true;
                txtRemarks.MaxLength = 20;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>M.Tsutsumi 2010/07/13</create>
        /// <update>H.Tajimi 2018/12/25 木枠梱包業務改善</update>
        /// <update>K.Tsutsumi 2018/01/20 AR対応</update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // ベースでClearMessageの呼出しは行われています。
                // 処理モード
                this.rdoInsertEx.Checked = true;
                // 工事識別NO
                this.txtKojiName.Text = string.Empty;
                // 便
                this.txtShip.Text = string.Empty;
                // 管理No
                this.txtKojiNo.Text = string.Empty;
                // 物件管理No
                this._bukkenNo = string.Empty;
                // 入力項目のクリア
                this.DisplayPartClear();

                // 検索条件のロック解除
                this.ChangeEnableViewMode(false);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 入力部分だけクリア
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private void DisplayPartClear()
        {
            try
            {
                // データテーブルクリア
                this._dtKiwaku = null;
                this._dtKiwakuMeisai = null;
                this._dtShagaiKiwakuMeisai = null;

                // (A)CASEMARK
                this.txtCaseMark.Text = string.Empty;
                //this.picCaseMark.Image.Dispose();
                this.picCaseMark.Image = null;
                // 作業状況
                this.txtSagyoFlag.Text = string.Empty;
                // (B)DELIVERY NO
                this.txtDeliveryNo.Text = string.Empty;
                // (C)PORT OF DESTINATION
                this.txtPortOfDestination.Text = string.Empty;
                // (D)AIR / BOAT
                this.txtAirBoat.Text = string.Empty;
                // (E)DELIVERY DATE
                this.txtDeliveryDate.Text = string.Empty;
                // (F)DELIVERY POINT
                this.txtDeliveryPoint.Text = string.Empty;
                // (G)FACTORY
                this.txtFactory.Text = string.Empty;
                // *REMARKS
                this.txtRemarks.Text = string.Empty;
                // グリッド
                this.SheetClear();

                // 変数
                this._delFileName = null;

                // 再描画指示変数
                this._displayRedraw = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>M.Tsutsumi 2010/07/26</create>
        /// <update>H.Tajimi 2015/11/25 ケースナンバーの欠番対応</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                // 編集内容実行時の入力チェック
                DataTable dt = this.shtMeisai.GetMultiRowGetDataSource();

                if (this.EditMode != SystemBase.EditMode.Delete)
                {
                    if (dt == null ||
                        dt.Rows.Count == 0)
                    {
                        // 明細が入力されていません。
                        this.ShowMessage("A9999999028");
                        return false;
                    }

                    // 2015/11/26 H.Tajimi ケースナンバーの欠番対応
                    var arrayCaseNo = dt.AsEnumerable().Select(x => ComFunc.GetFld(x, Def_T_KIWAKU_MEISAI.CASE_NO)).ToArray();
                    var arrayPrintCaseNo = dt.AsEnumerable().Select(x => ComFunc.GetFld(x, Def_T_KIWAKU_MEISAI.PRINT_CASE_NO)).ToArray();
                    // ↑

                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        // 2015/11/25 H.Tajimi ケースナンバーの欠番対応
                        // ケースナンバー
                        if (string.IsNullOrEmpty(ComFunc.GetFld(dt, i, Def_T_KIWAKU_MEISAI.CASE_NO)))
                        {
                            // {0}行目の{1}が入力されていません。
                            this.ShowMessage("A9999999043", (i + 1).ToString(), Resources.Kiwaku_CNo);
                            return false;
                        }

                        // 他の行に同一ケースナンバーを持つデータが存在するかどうか
                        var retDupCNoIndex = this.GetDuplicateDataIndex(arrayCaseNo, ComFunc.GetFld(dt, i, Def_T_KIWAKU_MEISAI.CASE_NO), i);
                        if (retDupCNoIndex != -1) 
                        {
                            // {0}が{1}行目と{2}行目で重複しています。変更して下さい。
                            this.ShowMessage("K0300010012", Resources.Kiwaku_CNo, (i + 1).ToString(), (retDupCNoIndex + 1).ToString());
                            return false;
                        }

                        // 印刷ケースナンバー
                        if (string.IsNullOrEmpty(ComFunc.GetFld(dt, i, Def_T_KIWAKU_MEISAI.PRINT_CASE_NO)))
                        {
                            // {0}行目の{1}が入力されていません。
                            this.ShowMessage("A9999999043", (i + 1).ToString(), Resources.Kiwaku_PrintCNo);
                            return false;
                        }

                        // 他の行に同一印刷ケースナンバーを持つデータが存在するかどうか
                        var retDupPrintCNoIndex = this.GetDuplicateDataIndex(arrayPrintCaseNo, ComFunc.GetFld(dt, i, Def_T_KIWAKU_MEISAI.PRINT_CASE_NO), i);
                        if (retDupPrintCNoIndex != -1)
                        {
                            // {0}が{1}行目と{2}行目で重複しています。変更して下さい。
                            this.ShowMessage("K0300010012", Resources.Kiwaku_PrintCNo, (i + 1).ToString(), (retDupPrintCNoIndex + 1).ToString());
                            return false;
                        }
                        // ↑

                        // STYLE
                        if (string.IsNullOrEmpty(ComFunc.GetFld(dt, i, Def_T_KIWAKU_MEISAI.STYLE)))
                        {
                            // {0}行目の{1}が入力されていません。
                            this.ShowMessage("A9999999043", (i + 1).ToString(), Resources.Kiwaku_Style);
                            return false;
                        }

                        // DIMENSION_L
                        if (string.IsNullOrEmpty(ComFunc.GetFld(dt, i, Def_T_KIWAKU_MEISAI.DIMENSION_L)))
                        {
                            // {0}行目の{1}が入力されていません。
                            this.ShowMessage("A9999999043", (i + 1).ToString(), Resources.Kiwaku_DimensionL);
                            return false;
                        }

                        // DIMENSION_W
                        if (string.IsNullOrEmpty(ComFunc.GetFld(dt, i, Def_T_KIWAKU_MEISAI.DIMENSION_W)))
                        {
                            // {0}行目の{1}が入力されていません。
                            this.ShowMessage("A9999999043", (i + 1).ToString(), Resources.Kiwaku_DimensionW);
                            return false;
                        }

                        // DIMENSION_H
                        if (string.IsNullOrEmpty(ComFunc.GetFld(dt, i, Def_T_KIWAKU_MEISAI.DIMENSION_H)))
                        {
                            // {0}行目の{1}が入力されていません。
                            this.ShowMessage("A9999999043", (i + 1).ToString(), Resources.Kiwaku_DimensionH);
                            return false;
                        }

                        // 木材重量
                        if (string.IsNullOrEmpty(ComFunc.GetFld(dt, i, Def_T_KIWAKU_MEISAI.MOKUZAI_JYURYO)))
                        {
                            // {0}行目の{1}が入力されていません。
                            this.ShowMessage("A9999999043", (i + 1).ToString(), Resources.Kiwaku_WoodWeight);
                            return false;
                        }
                    }
                }
                else
                {
                    if (dt == null ||
                        dt.Rows.Count == 0)
                    {
                        // 明細がない(通常ありえないが、かまわない)
                        return true;
                    }
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        // 出荷日
                        if (!string.IsNullOrEmpty(ComFunc.GetFld(dt, i, Def_T_KIWAKU_MEISAI.SHUKKA_DATE)))
                        {
                            // 出荷済みなので明細を削除できません。
                            this.ShowMessage("A9999999041");
                            return false;
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
        /// <create>M.Tsutsumi 2010/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                // 検索用入力チェック
                // 工事識別NOチェック
                if (string.IsNullOrEmpty(this.txtKojiName.Text))
                {
                    // 工事識別NOを入力して下さい。
                    this.ShowMessage("A9999999029");
                    this.txtKojiName.Focus();
                    return false;
                }
                ret = this.CheckKiwaku();
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
        /// 登録用入力チェック
        /// ※処理選択で「登録」が選ばれていた場合の「開始」ボタン押下時チェック
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/12/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckInputForInsert()
        {
            bool ret = false;
            try
            {
                // 便入力チェック
                if (string.IsNullOrEmpty(this.txtShip.Text))
                {
                    // 便を入力して下さい。
                    this.ShowMessage("A9999999017");
                    this.txtShip.Focus();
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
        /// <create>M.Tsutsumi 2010/07/26</create>
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
        /// <create>M.Tsutsumi 2010/07/26</create>
        /// <update>H.Tajimi 2019/09/10 本体用とAR用の登録を分離</update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                string errMsgID;
                string[] args;

                // 画面の入力部分をクリア
                this.DisplayPartClear();

                // 登録
                if (this.rdoInsertRegular.Checked || this.rdoInsertEx.Checked || this.rdoInsertAR.Checked)
                {
                    // データの設定
                    try
                    {
                        this.shtMeisai.Redraw = false;
                        DataTable dt = GetSchemaKiwakuMeisai();
                        //DataRow dr = dt.NewRow();
                        //dr[Def_T_KIWAKU_MEISAI.CASE_NO] = 1;
                        //dt.Rows.Add(dr);
                        this.shtMeisai.SetMultiRowDataSource(dt, this._multiRowFormat);
                        this.shtMeisai.TransformEditor = true;
                    }
                    finally
                    {
                        this.shtMeisai.Redraw = true;
                    }
                }
                // 変更・削除
                else if (this.rdoUpdate.Checked || this.rdoDelete.Checked)
                {
                    CondK03 cond = this.GetCondition();
                    ConnK03 conn = new ConnK03();

                    DataSet ds = conn.GetKiwaku(cond, out errMsgID, out args);
                    if (!string.IsNullOrEmpty(errMsgID))
                    {
                        this.ShowMessage(errMsgID, args);
                        return false;
                    }
                    if (ds == null || !ds.Tables.Contains(Def_T_KIWAKU.Name) ||
                                      !ds.Tables.Contains(Def_T_KIWAKU_MEISAI.Name) ||
                                      ds.Tables[Def_T_KIWAKU.Name].Rows.Count < 1)
                    {
                        // 該当の明細は存在しません。
                        this.ShowMessage("A9999999022");
                        return false;
                    }
                    // データの設定
                    try
                    {
                        this._dtKiwaku = ds.Tables[Def_T_KIWAKU.Name].Copy();

                        this.txtKojiName.Text = ComFunc.GetFld(this._dtKiwaku, 0, Def_T_KIWAKU.KOJI_NAME);
                        this.txtShip.Text = ComFunc.GetFld(this._dtKiwaku, 0, Def_T_KIWAKU.SHIP);
                        this.txtKojiNo.Text = ComFunc.GetFld(this._dtKiwaku, 0, Def_T_KIWAKU.KOJI_NO);
                        this.txtCaseMark.Text = ComFunc.GetFld(this._dtKiwaku, 0, Def_T_KIWAKU.CASE_MARK_FILE);
                        this.txtSagyoFlag.Text = ComFunc.GetFld(this._dtKiwaku, 0, "SAGYO_FLAG_NAME");
                        this.txtDeliveryNo.Text = ComFunc.GetFld(this._dtKiwaku, 0, Def_T_KIWAKU.DELIVERY_NO);
                        this.txtPortOfDestination.Text = ComFunc.GetFld(this._dtKiwaku, 0, Def_T_KIWAKU.PORT_OF_DESTINATION);
                        this.txtAirBoat.Text = ComFunc.GetFld(this._dtKiwaku, 0, Def_T_KIWAKU.AIR_BOAT);
                        this.txtDeliveryDate.Text = ComFunc.GetFld(this._dtKiwaku, 0, Def_T_KIWAKU.DELIVERY_DATE);
                        this.txtDeliveryPoint.Text = ComFunc.GetFld(this._dtKiwaku, 0, Def_T_KIWAKU.DELIVERY_POINT);
                        this.txtFactory.Text = ComFunc.GetFld(this._dtKiwaku, 0, Def_T_KIWAKU.FACTORY);
                        this.txtRemarks.Text = ComFunc.GetFld(this._dtKiwaku, 0, Def_T_KIWAKU.REMARKS);

                        // ファイル名待避
                        this._delFileName = ComFunc.GetFld(this._dtKiwaku, 0, Def_T_KIWAKU.CASE_MARK_FILE);

                        string kojiNo = ComFunc.GetFld(this._dtKiwaku, 0, Def_T_KIWAKU.KOJI_NO);
                        string fileName = ComFunc.GetFld(this._dtKiwaku, 0, Def_T_KIWAKU.CASE_MARK_FILE);
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            this.FileDownload(kojiNo, fileName);
                        }
                        //this.picCaseMark.Image = new Bitmap(this.FileDownload(kojiNo, fileName));

                        this.shtMeisai.Redraw = false;
                        this.shtMeisai.SetMultiRowDataSource(ds.Tables[Def_T_KIWAKU_MEISAI.Name], this._multiRowFormat);
                        this._dtKiwakuMeisai = ds.Tables[Def_T_KIWAKU_MEISAI.Name].Copy();
                        this.shtMeisai.TransformEditor = true;
                        this.shtMeisai.Invalidate();

                        if (ds.Tables.Contains(Def_T_SHAGAI_KIWAKU_MEISAI.Name))
                        {
                            // 社外用木枠明細データ待避
                            this._dtShagaiKiwakuMeisai = ds.Tables[Def_T_SHAGAI_KIWAKU_MEISAI.Name].Copy();
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
                this.SheetClear();
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
        /// <create>M.Tsutsumi 2010/08/02</create>
        /// <update>K.Tsutsumi 2018/01/20 AR対応</update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            bool ret = base.RunEdit();
            try
            {
                // ここに登録処理後の画面制御を記述(フォーカス等)
                // ※正常登録時はDisplayClear()は呼出し済です。
                if (!ret && this._displayRedraw)
                {
                    // バージョンエラー時、再描画
                    this.DisplayRefresh();
                }

                // 正常終了
                if (ret)
                {
                    this.rdoInsertEx.Focus();
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
        /// <create>M.Tsutsumi 2010/07/26</create>
        /// <update>H.Tajimi 2018/12/25 木枠梱包業務改善</update>
        /// <update>K.Tsutsumi 2018/01/20 AR対応</update>
        /// <update>H.Tajimi 2019/09/10 本体用とAR用の登録を分離</update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                // 登録完了メッセージをデフォルトに戻す。
                this.MsgInsertEnd = this._defMsgInsertEnd;

                // 登録処理
                if (!this.CheckKiwaku()) return false;

                CondK03 cond = this.GetCondition();
                if (this.rdoInsertEx.Checked)
                {
                    cond.ShukkaFlag = SHUKKA_FLAG.NORMAL_VALUE1;
                }
                else if (this.rdoInsertRegular.Checked)
                {
                    cond.ShukkaFlag = SHUKKA_FLAG.NORMAL_VALUE1;
                    cond.KiwakuInsertType = KIWAKU_INSERT_TYPE.REGULAR_VALUE1;
                }
                else if (this.rdoInsertAR.Checked)
                {
                    cond.ShukkaFlag = SHUKKA_FLAG.AR_VALUE1;
                    cond.KiwakuInsertType = KIWAKU_INSERT_TYPE.AR_VALUE1;
                }
                cond.BukkenNo = this._bukkenNo;
                ConnK03 conn = new ConnK03();
                DataSet ds = new DataSet();
                DataTable dt;

                // 木枠
                dt = this.GetKiwakuNewInput();
                if (dt == null) return false;
                ds.Tables.Add(dt.Copy());

                // 木枠明細
                dt = this.shtMeisai.GetMultiRowGetDataSource();
                if (dt == null) return false;
                ds.Tables.Add(dt.Copy());

                // DB登録
                string errMsgID;
                string[] args;
                string kojiNo;
                if (!conn.InsKiwaku(cond, ds, out kojiNo, out errMsgID, out args))
                {
                    if (!string.IsNullOrEmpty(errMsgID))
                    {
                        this.ShowMessage(errMsgID, args);
                        // バージョンエラー？
                        if (ComFunc.IsVersionError(errMsgID))
                        {
                            this._displayRedraw = true;
                        }
                    }
                    return false;
                }

                // イメージファイルアップロード
                if (this.picCaseMark.Image != null)
                {
                    string fileName = ComFunc.GetFld(ds, Def_T_KIWAKU.Name, 0, Def_T_KIWAKU.CASE_MARK_FILE);
                    if (!FileUpload(kojiNo, fileName, null))
                    {
                        // ファイルのアップロードに失敗しました。\r\n再度保存してください。
                        this.ShowMessage("A9999999039");
                        // 状態としては、エラーにしない。
                        // 登録完了メッセージ変更
                        this.MsgInsertEnd = "A9999999046";
                        return true;
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
        /// <create>M.Tsutsumi 2010/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                // 修正完了メッセージをデフォルトに戻す。
                this.MsgUpdateEnd = this._defMsgUpdateEnd;

                // 修正処理
                if (!this.CheckKiwaku()) return false;

                // 木枠データ
                if (!this.GetKiwakuUpdInput())
                {
                    return false;
                }

                // 木枠明細データ
                DataTable dt = this.shtMeisai.GetMultiRowGetDataSource();
                dt.TableName = ComDefine.DTTBL_INSERT;

                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtKiwaku.Copy());
                ds.Tables.Add(this._dtKiwakuMeisai.Copy());
                ds.Tables.Add(dt.Copy());

                CondK03 cond = this.GetCondition();
                ConnK03 conn = new ConnK03();

                // DB更新
                string errMsgID;
                string[] args;
                if (!conn.UpdKiwaku(cond, ds, out errMsgID, out args))
                {
                    if (!string.IsNullOrEmpty(errMsgID))
                    {
                        this.ShowMessage(errMsgID, args);
                        // バージョンエラー？
                        if (ComFunc.IsVersionError(errMsgID))
                        {
                            this._displayRedraw = true;
                        }
                    }
                    return false;
                }

                // イメージファイルアップロード
                string kojiNo = ComFunc.GetFld(this._dtKiwaku, 0, Def_T_KIWAKU.KOJI_NO);
                if (this.picCaseMark.Image != null)
                {
                    string fileName = ComFunc.GetFld(this._dtKiwaku, 0, Def_T_KIWAKU.CASE_MARK_FILE);
                    if (!FileUpload(kojiNo, fileName, this._delFileName))
                    {
                        // ファイルのアップロードに失敗しました。\r\n再度保存してください。
                        this.ShowMessage("A9999999039");
                        // 状態としては、エラーにしない。
                        // 修正完了メッセージ変更
                        this.MsgUpdateEnd = "A9999999046";
                        return true;
                    }
                }
                else if (!string.IsNullOrEmpty(this._delFileName))
                {
                    if (!FileDelete(kojiNo, this._delFileName))
                    {
                        // エラーでも非通知
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

        #region 削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>M.Tsutsumi 2010/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditDelete()
        {
            try
            {
                // 削除処理
                if (!this.CheckKiwaku()) return false;

                if (this._dtKiwaku.Rows.Count == 0)
                {
                    return false;
                }

                //DataTable dt = this.shtMeisai.GetMultiRowGetDataSource();
                DataTable dt = this._dtKiwakuMeisai.Copy();
                dt.TableName = ComDefine.DTTBL_DELETE;

                // 削除データ
                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtKiwaku.Copy());
                ds.Tables.Add(dt);

                CondK03 cond = this.GetCondition();
                ConnK03 conn = new ConnK03();

                // DB削除
                string errMsgID;
                string[] args;
                if (!conn.DelKiwaku(cond, ds, out errMsgID, out args))
                {
                    if (!string.IsNullOrEmpty(errMsgID))
                    {
                        this.ShowMessage(errMsgID, args);
                        // バージョンエラー？
                        if (ComFunc.IsVersionError(errMsgID))
                        {
                            this._displayRedraw = true;
                        }
                    }
                    return false;
                }

                // イメージファイルデリート
                string kojiNo = ComFunc.GetFld(this._dtKiwaku, 0, Def_T_KIWAKU.KOJI_NO);
                string delFileName = ComFunc.GetFld(this._dtKiwaku, 0, Def_T_KIWAKU.CASE_MARK_FILE);
                if (!FileDelete(kojiNo, delFileName))
                {
                    // エラーでも非通知
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
        /// F1ボタンクリック[保存]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/08/03</create>
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
        /// F3ボタンクリック[行削除]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F03Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F03Button_Click(sender, e);
            try
            {
                // 行がないか行追加時の最終行の場合は処理を抜ける。
                if ((this.shtMeisai.Rows.Count / SHEET_ROW_BLOCK) < SHEET_ROW_BLOCK ||
                        (this.shtMeisai.MultiRowAllowUserToAddRows &&
                        this.shtMeisai.Rows.Count - SHEET_ROW_BLOCK == this.shtMeisai.ActivePosition.Row))
                {
                    this.shtMeisai.Focus();
                    return;
                }
                // 状態チェック
                string shukkaDate = this.shtMeisai[SHEET_COL_SHUKKA_DATE, this.shtMeisai.ActivePosition.Row].Text;
                if (!string.IsNullOrEmpty(shukkaDate))
                {
                    // 出荷済なので明細を削除出来ません。
                    this.ShowMessage("A9999999041");
                    this.shtMeisai.Focus();
                    return;
                }
                if (!string.IsNullOrEmpty(this.shtMeisai[SHEET_COL_PALLET_NO_1, this.shtMeisai.ActivePosition.Row].Text) ||
                    !string.IsNullOrEmpty(this.shtMeisai[SHEET_COL_PALLET_NO_2, this.shtMeisai.ActivePosition.Row].Text) ||
                    !string.IsNullOrEmpty(this.shtMeisai[SHEET_COL_PALLET_NO_3, this.shtMeisai.ActivePosition.Row].Text) ||
                    !string.IsNullOrEmpty(this.shtMeisai[SHEET_COL_PALLET_NO_4, this.shtMeisai.ActivePosition.Row].Text) ||
                    !string.IsNullOrEmpty(this.shtMeisai[SHEET_COL_PALLET_NO_5, this.shtMeisai.ActivePosition.Row].Text) ||
                    !string.IsNullOrEmpty(this.shtMeisai[SHEET_COL_PALLET_NO_6, this.shtMeisai.ActivePosition.Row].Text) ||
                    !string.IsNullOrEmpty(this.shtMeisai[SHEET_COL_PALLET_NO_7, this.shtMeisai.ActivePosition.Row].Text) ||
                    !string.IsNullOrEmpty(this.shtMeisai[SHEET_COL_PALLET_NO_8, this.shtMeisai.ActivePosition.Row].Text) ||
                    !string.IsNullOrEmpty(this.shtMeisai[SHEET_COL_PALLET_NO_9, this.shtMeisai.ActivePosition.Row].Text) ||
                    !string.IsNullOrEmpty(this.shtMeisai[SHEET_COL_PALLET_NO_10, this.shtMeisai.ActivePosition.Row].Text))
                {
                    // パレット登録を解除して下さい。
                    this.ShowMessage("A9999999036");
                    this.shtMeisai.Focus();
                    return;
                }
                if (this._dtShagaiKiwakuMeisai != null)
                {
                    string kojiNo = this.shtMeisai[SHEET_COL_KOJI_NO, this.shtMeisai.ActivePosition.Row].Text;
                    string caseId = this.shtMeisai[SHEET_COL_CASE_ID, this.shtMeisai.ActivePosition.Row].Text;
                    DataRow[] drs = this._dtShagaiKiwakuMeisai.Select(Def_T_SHAGAI_KIWAKU_MEISAI.KOJI_NO + " = " + kojiNo +
                                                            " AND " + Def_T_SHAGAI_KIWAKU_MEISAI.CASE_ID + " = " + caseId);
                    if (drs != null && 0 < drs.Length)
                    {
                        // 現品TagNo登録を解除してください。
                        this.ShowMessage("A9999999048");
                        this.shtMeisai.Focus();
                        return;
                    }
                }
                // C/NO[{0}]を削除してもよろしいですか？
                string caseNo = this.shtMeisai[SHEET_COL_CASE_NO, this.shtMeisai.ActivePosition.Row].Text;
                if (this.ShowMessage("A9999999037", caseNo) == DialogResult.OK)
                {
                    this.shtMeisai.Redraw = false;
                    this.shtMeisai.MultiRowRemove(this.shtMeisai.ActivePosition.Row);
                    this.shtMeisai.Redraw = true;
                }
                this.shtMeisai.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F5ボタンクリック[項コピー]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F05Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F05Button_Click(sender, e);
            try
            {
                this.SheetCellCopy(this.shtMeisai);
                this.shtMeisai.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F6ボタンクリック[クリア]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/08/03</create>
        /// <update>H.Tajimi 2015/12/08 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                if (ShowMessage("A9999999053") != DialogResult.OK) return;
                // 入力項目のクリア
                this.DisplayPartClear();
                // 検索条件のロック解除
                this.ChangeEnableViewMode(false);
                this.txtKojiName.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F7ボタンクリック[All Clear]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2015/12/01</create>
        /// <update>K.Tsutsumi 2018/01/20 AR対応</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                if (ShowMessage("A9999999001") != DialogResult.OK) return;
                this.DisplayClear();
                this.rdoInsertEx.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F8ボタンクリック[補完C/No]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2015/11/25</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F08Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F08Button_Click(sender, e);
            try
            {
                string ship = this.txtShip.Text;
                using (PrintCNoInput frm = new PrintCNoInput(this.UserInfo, ship)) 
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        var tempShip = frm.Ship;
                        if (!string.IsNullOrEmpty(tempShip))
                        {
                            for (int row = 0; row <= this.shtMeisai.Rows.Count - (SHEET_ROW_BLOCK * 2); row = row + SHEET_ROW_BLOCK)
                            {
                                if (!string.IsNullOrEmpty(this.shtMeisai[SHEET_COL_CASE_NO, row].Text))
                                {
                                    // C/NOが設定されていた場合は、C/NOを便と連結して印刷C/NOに設定する
                                    var caseNo = this.shtMeisai[SHEET_COL_CASE_NO, row].Text;
                                    this.shtMeisai[SHEET_COL_PRINT_CASE_NO, row].Text = tempShip + "-" + caseNo;
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
        /// F12ボタンクリック[閉じる]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F12Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
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
        /// 登録(本体便まとめ)ラジオボタンのCheckedChangedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>K.Tsutsumi 2018/01/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoInsertEx_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoInsertEx.Checked)
            {
                this.ClearMessage();
                this.ChangeMode();
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 登録(本体)ラジオボタンのCheckedChangedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/07/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoInsertRegular_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoInsertRegular.Checked)
            {
                this.ClearMessage();
                this.ChangeMode();
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 登録(AR)ラジオボタンのCheckedChangedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create> 2019/09/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoInsertAR_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoInsertAR.Checked)
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
        /// <create>M.Tsutsumi 2010/07/13</create>
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
        /// <create>M.Tsutsumi 2010/07/13</create>
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

        #endregion

        #region 開始ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 開始ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/07/13</create>
        /// <update>H.Tajimi 2015/11/30 納入先(User)選択を早くする</update>
        /// <update>H.Tajimi 2018/12/25 木枠梱包業務改善</update>
        /// <update>K.Tsutsumi 2018/01/20 AR対応</update>
        /// <update>H.Tajimi 2019/09/10 本体用とAR用の登録を分離</update>
        /// --------------------------------------------------
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();

                // 2015/11/30 H.Tajimi 登録時以外は工事識別一覧画面を表示
                bool ret = false;
                if (this.rdoInsertEx.Checked)
                {
                    ret = this.CheckInputForInsert();
                    if (!ret)
                    {
                        return;
                    }

                    // 物件名一覧画面表示
                    ret = this.ShowBukkenMeiIchiran();
                    if (!ret)
                    {
                        return;
                    }
                }
                else if (this.rdoInsertRegular.Checked || this.rdoInsertAR.Checked)
                {
                    ret = this.CheckInputForInsert();
                    if (!ret)
                    {
                        return;
                    }

                    // 納入先一覧画面表示
                    ret = this.ShowNonyusakiIchiran();
                    if (!ret)
                    {
                        return;
                    }
                }
                else
                {
                    // 工事識別一覧画面表示
                    ret = this.ShowKojiShikibetsuIchiran();
                    if (!ret)
                    {
                        this.txtKojiName.Focus();
                        return;
                    }
                }
                if (ret)
                // ↑
                {
                    // 検索条件のロック
                    this.ChangeEnableViewMode(true);

                    if (this.EditMode == SystemBase.EditMode.Delete)
                    {
                        // テキスト
                        this.txtDeliveryNo.ReadOnly = true;
                        this.txtPortOfDestination.ReadOnly = true;
                        this.txtAirBoat.ReadOnly = true;
                        this.txtDeliveryDate.ReadOnly = true;
                        this.txtDeliveryPoint.ReadOnly = true;
                        this.txtFactory.ReadOnly = true;
                        this.txtRemarks.ReadOnly = true;
                        // 色へんに変わるから自分で設定
                        this.txtDeliveryNo.BackColor = SystemColors.Control;
                        this.txtPortOfDestination.BackColor = SystemColors.Control;
                        this.txtAirBoat.BackColor = SystemColors.Control;
                        this.txtDeliveryDate.BackColor = SystemColors.Control;
                        this.txtDeliveryPoint.BackColor = SystemColors.Control;
                        this.txtFactory.BackColor = SystemColors.Control;
                        this.txtRemarks.BackColor = SystemColors.Control;
                    }
                    else
                    {
                        // テキスト
                        this.txtDeliveryNo.ReadOnly = false;
                        this.txtPortOfDestination.ReadOnly = false;
                        this.txtAirBoat.ReadOnly = false;
                        this.txtDeliveryDate.ReadOnly = false;
                        this.txtDeliveryPoint.ReadOnly = false;
                        this.txtFactory.ReadOnly = false;
                        this.txtRemarks.ReadOnly = false;
                        // 色へんに変わるから自分で設定
                        this.txtDeliveryNo.BackColor = SystemColors.Window;
                        this.txtPortOfDestination.BackColor = SystemColors.Window;
                        this.txtAirBoat.BackColor = SystemColors.Window;
                        this.txtDeliveryDate.BackColor = SystemColors.Window;
                        this.txtDeliveryPoint.BackColor = SystemColors.Window;
                        this.txtFactory.BackColor = SystemColors.Window;
                        this.txtRemarks.BackColor = SystemColors.Window;
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
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
        /// <create>M.Tsutsumi 2010/07/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnSelect_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            try
            {
                OpenFileDialog frm = new OpenFileDialog();

                frm.Filter = Resources.Kiwaku_ImageFile;
                //@@@ 2011/02/24 M.Tsutsumi Add 
                frm.Title = Resources.Kiwaku_FileOpen;
                // @@@ ↑
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                ComRegulation comReg = new ComRegulation();
                if (!comReg.CheckRegularString(frm.SafeFileName, txtCaseMark.InputRegulation, txtCaseMark.IsInputRegulation, false))
                {
                    // 使用出来ない文字が含まれています。
                    return;
                }
                int nameSize = UtilString.GetByteCount(frm.SafeFileName);
                if (CASEMARK_FILE_NAME_SIZE < nameSize)
                {
                    // ファイル名が長すぎます。
                    return;
                }

                string fileFullName = frm.FileName;
                string fileName = frm.SafeFileName;
                this.txtCaseMark.Text = fileName;

                if (string.IsNullOrEmpty(fileFullName))
                {
                    return;
                }
                if (this.picCaseMark.Image != null)
                {
                    // 前回のイメージ破棄
                    //this.picCaseMark.Image.Dispose();
                    this.picCaseMark.Image = null;
                }
                this.picCaseMark.Image = Image.FromFile(fileFullName);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 削除ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 削除ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            try
            {
                // {0}を削除してもよろしいですか？
                if (this.ShowMessage("A9999999042", "CASE MARK FILE") == DialogResult.OK)
                {
                    this.txtCaseMark.Text = string.Empty;
                    this.picCaseMark.Image = null;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region グリッド

        #region RowRemoved

        /// --------------------------------------------------
        /// <summary>
        /// グリッドの行削除後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/08/02</create>
        /// <update>H.Tajimi 2015/11/25 ケースナンバーの欠番対応</update>
        /// <update></update>
        /// --------------------------------------------------
        private void shtMeisai_RowRemoved(object sender, RowRemovedEventArgs e)
        {
            try
            {
                // 2015/11/25 H.Tajimi ケースナンバーの欠番対応
                // ケースナンバーの自動採番を止めるが空処理でイベントは残しておく
                //// CASE_NOの付け直し
                //// なぜか、行削除後のイベントなのに、行削除前の行数がくるんだ。
                //for (int i = 0; i < this.shtMeisai.Rows.Count - (SHEET_ROW_BLOCK * 2); i = i + SHEET_ROW_BLOCK)
                //{
                //    this.shtMeisai[SHEET_COL_CASE_NO, i].Text = (i / SHEET_ROW_BLOCK + 1).ToString();
                //}
                // ↑
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
        /// グリッドの値が変わった時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/08/02</create>
        /// <update>H.Tajimi 2015/11/25 ケースナンバー欠番対応</update>
        /// <update>2023/12/12 J.Chen 小数点第2位まで対応修正</update>
        /// <update></update>
        /// --------------------------------------------------
        private void shtMeisai_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            try
            {
                int row = e.Position.Row;
                int col = e.Position.Column;

                switch (col)
                {
                    case SHEET_COL_CASE_NO:
                    case SHEET_COL_MMNET:
                        // 何もしない
                        break;
                    default:
                        // 2015/11/25 H.Tajimi ケースナンバー欠番対応 入力可能にしたため採番処理を削除
                        //// CASE_NOが付いていない場合は採番する。
                        //decimal caseNo = DSWUtil.UtilConvert.ToDecimal(this.shtMeisai[SHEET_COL_CASE_NO, row].Text);
                        //if (caseNo == 0)
                        //{
                        //    this.shtMeisai[SHEET_COL_CASE_NO, row].Text = (row / SHEET_ROW_BLOCK + 1).ToString();
                        //}
                        // ↑
                        break;
                }

                if (col == SHEET_COL_DIMENSION_L ||
                    col == SHEET_COL_DIMENSION_W ||
                    col == SHEET_COL_DIMENSION_H)
                {
                    // MMNET = L/100 * W/100 * H/100
                    decimal dimensionL = DSWUtil.UtilConvert.ToDecimal(this.shtMeisai[SHEET_COL_DIMENSION_L, row].Text);
                    decimal dimensionW = DSWUtil.UtilConvert.ToDecimal(this.shtMeisai[SHEET_COL_DIMENSION_W, row].Text);
                    decimal dimensionH = DSWUtil.UtilConvert.ToDecimal(this.shtMeisai[SHEET_COL_DIMENSION_H, row].Text);
                    decimal mmnet = (dimensionL / 100) * (dimensionW / 100) * (dimensionH / 100);
                    this.shtMeisai[SHEET_COL_MMNET, row].Text = mmnet.ToString("###,##0.000");
                }
                if (col == SHEET_COL_MOKUZAI_JYURYO)
                {
                    // NET_W = GROSS_W - 木材重量
                    decimal mokuzaiJyuryo = DSWUtil.UtilConvert.ToDecimal(this.shtMeisai[SHEET_COL_MOKUZAI_JYURYO, row].Text);
                    decimal grossW = DSWUtil.UtilConvert.ToDecimal(this.shtMeisai[SHEET_COL_GROSS_W, row].Text);
                    decimal netW = grossW - mokuzaiJyuryo;
                    // 2023/12/12 J.Chen 小数点第2位まで対応に変更
                    this.shtMeisai[SHEET_COL_NET_W, row].Text = netW.ToString("##,##0.00");
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

        #region モード切替操作

        /// --------------------------------------------------
        /// <summary>
        /// モード変更時の切り替え処理
        /// </summary>
        /// <create>M.Tsutsumi 2010/07/13</create>
        /// <update>K.Tsutsumi 2018/01/20 AR対応</update>
        /// <update>H.Tajimi 2019/09/10 本体用とAR用の登録を分離</update>
        /// --------------------------------------------------
        private void ChangeMode()
        {
            try
            {
                if (this.rdoInsertRegular.Checked || this.rdoInsertEx.Checked || this.rdoInsertAR.Checked)
                {
                    // 登録
                    // 画面の編集モード変更
                    this.EditMode = SystemBase.EditMode.Insert;
                    // 入力コントロールの切替
                    // ボタンの切替
                    this.btnListSelect.Enabled = false;
                    this.btnStart.Enabled = true;
                    this.btnSelect.Enabled = true;
                    this.btnDelete.Enabled = true;
                    //this.fbrFunction.F01Button.Enabled = true;
                    //this.fbrFunction.F03Button.Enabled = false;
                    //this.fbrFunction.F05Button.Enabled = false;
                    this.fbrFunction.F07Button.Enabled = true;
                    // グリッド
                    this.shtMeisai.EditType = EditType.Default;
                    this.shtMeisai.AllowUserToAddRows = false;
                    //this.shtMeisai.MultiRowAllowUserToAddRows = false;
                    this.shtMeisai.MultiRowAllowUserToAddRows = true;
                    // ファンクションボタンの切替
                    this.ChangeFunctionButton(true);
                }
                else if (this.rdoUpdate.Checked)
                {
                    // 変更
                    // 画面の編集モード変更
                    this.EditMode = SystemBase.EditMode.Update;
                    // 入力コントロールの切替
                    // ボタンの切替
                    this.btnListSelect.Enabled = true;
                    this.btnStart.Enabled = true;
                    this.btnSelect.Enabled = true;
                    this.btnDelete.Enabled = true;
                    //this.fbrFunction.F01Button.Enabled = true;
                    //this.fbrFunction.F03Button.Enabled = true;
                    //this.fbrFunction.F05Button.Enabled = true;
                    this.fbrFunction.F07Button.Enabled = true;
                    // グリッド
                    this.shtMeisai.EditType = EditType.Default;
                    this.shtMeisai.AllowUserToAddRows = false;
                    this.shtMeisai.MultiRowAllowUserToAddRows = true;
                    // ファンクションボタンの切替
                    this.ChangeFunctionButton(false);
                }
                else if (this.rdoDelete.Checked)
                {
                    // 削除
                    // 画面の編集モード変更
                    this.EditMode = SystemBase.EditMode.Delete;
                    // 入力コントロールの切替
                    // ボタンの切替
                    this.btnListSelect.Enabled = true;
                    this.btnStart.Enabled = true;
                    this.btnSelect.Enabled = false;
                    this.btnDelete.Enabled = false;
                    //this.fbrFunction.F01Button.Enabled = true;
                    //this.fbrFunction.F03Button.Enabled = false;
                    //this.fbrFunction.F05Button.Enabled = false;
                    this.fbrFunction.F07Button.Enabled = true;
                    // グリッド
                    this.shtMeisai.EditType = EditType.ReadOnly;
                    this.shtMeisai.AllowUserToAddRows = false;
                    this.shtMeisai.MultiRowAllowUserToAddRows = false;
                    // ファンクションボタンの切替
                    this.ChangeFunctionButton(false);
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
        /// ファンクションバーのEnabled切り替え
        /// </summary>
        /// <param name="isEnabled"></param>
        /// <create>M.Tsutsumi 2010/07/13</create>
        /// <update>K.Tsutsumi 2018/01/20 AR対応</update>
        /// <update>H.Tajimi 2019/09/10 本体用とAR用の登録を分離</update>
        /// --------------------------------------------------
        private void ChangeFunctionButton(bool isEnabled)
        {
            if (this.rdoInsertRegular.Checked || this.rdoInsertEx.Checked || this.rdoInsertAR.Checked)
            {
                // 登録
            }
            else if (this.rdoUpdate.Checked)
            {
                // 変更
            }
            else if (this.rdoDelete.Checked)
            {
                // 削除
            }
            else
            {
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// コントロールのEnabled切替
        /// </summary>
        /// <param name="isView"></param>
        /// <create>M.Tsutsumi 2010/08/02</create>
        /// <update>H.Tajimi 2015/11/25 ケースナンバーの欠番対応</update>
        /// <update>H.Tajimi 2015/12/02 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeEnableViewMode(bool isView)
        {
            // 表示コントロールのロック/ロック解除
            this.pnlKiwaku.Enabled = isView;
            this.shtMeisai.Enabled = isView;
            // 検索条件のロック/ロック解除
            this.grpSearch.Enabled = !isView;
            // 保存ボタン
            this.fbrFunction.F01Button.Enabled = isView;
            // 行削除・項コピーボタン
            if (isView && this.EditMode != SystemBase.EditMode.Delete)
            {
                this.fbrFunction.F03Button.Enabled = true;
                this.fbrFunction.F05Button.Enabled = true;
                // 2015/12/02 H.Tajimi クリア有効
                this.fbrFunction.F06Button.Enabled = true;
                // ↑
            	// 2015/11/25 H.Tajimi ケースナンバーの欠番対応
                this.fbrFunction.F08Button.Enabled = true;
                // ↑
            }
            else
            {
                this.fbrFunction.F03Button.Enabled = false;
                this.fbrFunction.F05Button.Enabled = false;
                // 2015/12/02 H.Tajimi クリア有効
                this.fbrFunction.F06Button.Enabled = isView;
                // ↑
	            // 2015/11/25 H.Tajimi ケースナンバーの欠番対応
                this.fbrFunction.F08Button.Enabled = false;
                // ↑
            }
        }

        #endregion

        #region コンディション取得

        /// --------------------------------------------------
        /// <summary>
        /// コンディションの取得
        /// </summary>
        /// <returns>コンディション</returns>
        /// <create>M.Tsutsumi 2010/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondK03 GetCondition()
        {
            CondK03 cond = new CondK03(this.UserInfo);

            cond.KojiName = this.txtKojiName.Text;
            cond.Ship = this.txtShip.Text;
            cond.torokuFlag = this.TorokuFlag;

            return cond;
        }

        #endregion

        #region 木枠データチェック

        /// --------------------------------------------------
        /// <summary>
        /// 木枠データチェック
        /// </summary>
        /// <returns></returns>
        /// <create>M.Tsutsumi 2010/09/15</create>
        /// <update>K.Tsutsumi 2018/01/20 AR対応</update>
        /// <update>H.Tajimi 2019/09/10 本体用とAR用の登録を分離</update>
        /// --------------------------------------------------
        private bool CheckKiwaku()
        {
            CondK03 cond = this.GetCondition();
            ConnK03 conn = new ConnK03();

            if (this.rdoInsertRegular.Checked || this.rdoInsertEx.Checked || this.rdoInsertAR.Checked)
            {
                // 登録のチェック
                if (conn.CheckExistenceKiwaku(cond))
                {
                    // 既に登録されています。
                    this.ShowMessage("A9999999038");
                    return false;
                }
            }
            else
            {
                // 更新、削除のチェック
            }

            return true;
        }

        #endregion

        #region 項コピー処理

        /// --------------------------------------------------
        /// <summary>
        /// 項コピー処理
        /// </summary>
        /// <param name="sheet"></param>
        /// <create>M.Tsutsumi 2010/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetCellCopy(Sheet sheet)
        {
            if (sheet.ActivePosition.Row < 1 || !sheet.ActiveCell.Enabled || sheet.ActiveCell.Lock) return;
            int targetCol = sheet.ActivePosition.Column;
            int targetRow = sheet.ActivePosition.Row - SHEET_ROW_BLOCK;
            sheet.ActiveCell.Value = sheet[targetCol, targetRow].Value;
            sheet.EditState = true;
        }

        #endregion

        #region 登録用のデータテーブル

        /// --------------------------------------------------
        /// <summary>
        /// 木枠のデータテーブル作成
        /// </summary>
        /// <returns></returns>
        /// <create>M.Tsutsumi 2010/07/27</create>
        /// <update>H.Tajimi 2018/12/25 木枠梱包業務改善</update>
        /// <update>K.Tsutsumi 2019/01/13 工事識別No.36進数化</update>
        /// <update>H.Tajimi 2019/09/10 本体用とAR用の登録を分離</update>
        /// --------------------------------------------------
        private DataTable GetSchemaKiwaku()
        {
            DataTable dt = new DataTable(Def_T_KIWAKU.Name);
            dt.Columns.Add(Def_T_KIWAKU.KOJI_NO, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.KOJI_NAME, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.SHIP, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.TOROKU_FLAG, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.CASE_MARK_FILE, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.DELIVERY_NO, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.PORT_OF_DESTINATION, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.AIR_BOAT, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.DELIVERY_DATE, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.DELIVERY_POINT, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.FACTORY, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.REMARKS, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.SAGYO_FLAG, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.SHUKKA_DATE, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.SHUKKA_USER_ID, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.SHUKKA_USER_NAME, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.UNSOKAISHA_NAME, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.INVOICE_NO, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.OKURIJYO_NO, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.SHUKKA_FLAG, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.NONYUSAKI_CD, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.INSERT_TYPE, typeof(string));

            return dt;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 木枠明細のデータテーブル作成
        /// </summary>
        /// <returns></returns>
        /// <create>M.Tsutsumi 2010/07/27</create>
        /// <update>K.Tsutsumi 2019/01/13 工事識別No.36進数化</update>
        /// --------------------------------------------------
        private DataTable GetSchemaKiwakuMeisai()
        {
            DataTable dt = new DataTable(Def_T_KIWAKU_MEISAI.Name);
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.KOJI_NO, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.CASE_ID, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.CASE_NO, typeof(decimal));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.STYLE, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.ITEM, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.DESCRIPTION_1, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.DESCRIPTION_2, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.DIMENSION_L, typeof(decimal));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.DIMENSION_W, typeof(decimal));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.DIMENSION_H, typeof(decimal));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.MMNET, typeof(decimal));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.NET_W, typeof(decimal));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.GROSS_W, typeof(decimal));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.MOKUZAI_JYURYO, typeof(decimal));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.PALLET_NO_1, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.PALLET_NO_2, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.PALLET_NO_3, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.PALLET_NO_4, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.PALLET_NO_5, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.PALLET_NO_6, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.PALLET_NO_7, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.PALLET_NO_8, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.PALLET_NO_9, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.PALLET_NO_10, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.SHUKKA_DATE, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.SHUKKA_USER_ID, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.SHUKKA_USER_NAME, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.VERSION, typeof(string));
            // 2015/11/25 H.Tajimi ケースナンバーの欠番対応
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.PRINT_CASE_NO, typeof(string));
            // ↑

            return dt;
        }

        #endregion

        #region 登録データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 画面から木枠データ取得
        /// </summary>
        /// <returns></returns>
        /// <create>M.Tsutsumi 2010/08/02</create>
        /// <update>H.Tajimi 2018/12/25 木枠梱包業務改善</update>
        /// <update>K.Tsutsumi 2018/01/20 AR対応</update>
        /// <update>H.Tajimi 2019/09/10 本体用とAR用の登録を分離</update>
        /// --------------------------------------------------
        private DataTable GetKiwakuNewInput()
        {
            DataTable dt = GetSchemaKiwaku();

            DataRow dr = dt.NewRow();

            dr[Def_T_KIWAKU.KOJI_NAME] = this.txtKojiName.Text;
            dr[Def_T_KIWAKU.SHIP] = this.txtShip.Text;
            dr[Def_T_KIWAKU.TOROKU_FLAG] = TorokuFlag;
            dr[Def_T_KIWAKU.SAGYO_FLAG] = SAGYO_FLAG.KIWAKUMEISAI_VALUE1;
            dr[Def_T_KIWAKU.SHUKKA_DATE] = null;
            dr[Def_T_KIWAKU.SHUKKA_USER_ID] = null;
            dr[Def_T_KIWAKU.SHUKKA_USER_NAME] = null;
            dr[Def_T_KIWAKU.UNSOKAISHA_NAME] = null;
            dr[Def_T_KIWAKU.INVOICE_NO] = null;
            dr[Def_T_KIWAKU.OKURIJYO_NO] = null;
            if (this.rdoInsertRegular.Checked)
            {
                dr[Def_T_KIWAKU.SHUKKA_FLAG] = null;
                dr[Def_T_KIWAKU.INSERT_TYPE] = KIWAKU_INSERT_TYPE.REGULAR_VALUE1;
            }
            else if (this.rdoInsertAR.Checked)
            {
                dr[Def_T_KIWAKU.SHUKKA_FLAG] = SHUKKA_FLAG.AR_VALUE1;
                dr[Def_T_KIWAKU.INSERT_TYPE] = KIWAKU_INSERT_TYPE.AR_VALUE1;
            }
            else
            {
                dr[Def_T_KIWAKU.SHUKKA_FLAG] = SHUKKA_FLAG.NORMAL_VALUE1;
                dr[Def_T_KIWAKU.INSERT_TYPE] = null;
            }

            this.GetKiwakuInput(dr);

            dt.Rows.Add(dr);

            return dt;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 画面から木枠データ取得
        /// </summary>
        /// <returns></returns>
        /// <create>M.Tsutsumi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool GetKiwakuUpdInput()
        {
            if (this._dtKiwaku.Rows.Count == 0)
            {
                return false;
            }

            // 画面情報取得
            this.GetKiwakuInput(this._dtKiwaku.Rows[0]);

            return true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 画面から木枠データ取得
        /// </summary>
        /// <returns></returns>
        /// <create>M.Tsutsumi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private void GetKiwakuInput(DataRow dr)
        {
            if (!string.IsNullOrEmpty(this.txtCaseMark.Text))
            {
                dr[Def_T_KIWAKU.CASE_MARK_FILE] = this.txtCaseMark.Text;
            }
            else
            {
                dr[Def_T_KIWAKU.CASE_MARK_FILE] = null;
            }
            if (!string.IsNullOrEmpty(this.txtDeliveryNo.Text))
            {
                dr[Def_T_KIWAKU.DELIVERY_NO] = this.txtDeliveryNo.Text;
            }
            else
            {
                dr[Def_T_KIWAKU.DELIVERY_NO] = null;
            }
            if (!string.IsNullOrEmpty(this.txtPortOfDestination.Text))
            {
                dr[Def_T_KIWAKU.PORT_OF_DESTINATION] = this.txtPortOfDestination.Text;
            }
            else
            {
                dr[Def_T_KIWAKU.PORT_OF_DESTINATION] = null;
            }
            if (!string.IsNullOrEmpty(this.txtAirBoat.Text))
            {
                dr[Def_T_KIWAKU.AIR_BOAT] = this.txtAirBoat.Text;
            }
            else
            {
                dr[Def_T_KIWAKU.AIR_BOAT] = null;
            }
            if (!string.IsNullOrEmpty(this.txtDeliveryDate.Text))
            {
                dr[Def_T_KIWAKU.DELIVERY_DATE] = this.txtDeliveryDate.Text;
            }
            else
            {
                dr[Def_T_KIWAKU.DELIVERY_DATE] = null;
            }
            if (!string.IsNullOrEmpty(this.txtDeliveryPoint.Text))
            {
                dr[Def_T_KIWAKU.DELIVERY_POINT] = this.txtDeliveryPoint.Text;
            }
            else
            {
                dr[Def_T_KIWAKU.DELIVERY_POINT] = null;
            }
            if (!string.IsNullOrEmpty(this.txtFactory.Text))
            {
                dr[Def_T_KIWAKU.FACTORY] = this.txtFactory.Text;
            }
            else
            {
                dr[Def_T_KIWAKU.FACTORY] = null;
            }
            if (!string.IsNullOrEmpty(this.txtRemarks.Text))
            {
                dr[Def_T_KIWAKU.REMARKS] = this.txtRemarks.Text;
            }
            else
            {
                dr[Def_T_KIWAKU.REMARKS] = null;
            }
        }

        #endregion

        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>M.Tsutsumi 2010/07/27</create>
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
            this.shtMeisai.Redraw = true;
        }

        #endregion

        #region 複数行表示のフォーマット設定

        /// --------------------------------------------------
        /// <summary>
        /// 複数表表示のフォーマット設定
        /// </summary>
        /// <param name="format">MultiRowFormat</param>
        /// <param name="colType">グリッドに追加する列のタイプ</param>
        /// <param name="col">列のインデックス</param>
        /// <param name="row">行のインデックス</param>
        /// <param name="colSpan">列の連結数</param>
        /// <param name="rowSpan">行の連結数</param>
        /// <param name="dataField">データフィールド</param>
        /// <param name="digit">フォーマット文</param>
        /// <param name="isEnabled">セルが操作可能かどうか</param>
        /// <param name="imeMode">IMEモード</param>
        /// <param name="actions">Enterキー押下時のアクション</param>
        /// <create>M.Tsutsumi 2010/07/28</create>
        /// <update>M.Tsutsumi 2010/09/02</update>
        /// --------------------------------------------------
        private void SetFormat(MultiRowFormat format, ElTabelleColumnType colType, int col, int row, int colSpan, int rowSpan, string dataField, string digit, bool isEnabled, ImeMode imeMode, KeyAction[] actions)
        {
            MultiRowCell cell = new MultiRowCell(col, colSpan, rowSpan, dataField, actions);

            switch (colType)
            {
                case ElTabelleColumnType.Text:
                    TextEditor textEditor = ElTabelleSheetHelper.NewTextEditor();
                    if (!string.IsNullOrEmpty(digit))
                    {
                        textEditor.Format = digit;
                    }
                    cell.Editor = textEditor;
                    break;
                case ElTabelleColumnType.Number:
                    NumberEditor numberEditor = ElTabelleSheetHelper.NewNumberEditor();
                    if (string.IsNullOrEmpty(digit))
                    {
                        numberEditor.Format = new NumberFormat("###,###,###", string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                        numberEditor.DisplayFormat = new NumberFormat("###,###,###", string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                    }
                    else
                    {
                        numberEditor.Format = new NumberFormat(digit, string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                        numberEditor.DisplayFormat = new NumberFormat(digit, string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                    }
                    cell.Editor = numberEditor;
                    break;
                case ElTabelleColumnType.Button:
                    ButtonEditor buttonEditor = ElTabelleSheetHelper.NewButtonEditor();
                    buttonEditor.Text = null;
                    cell.Editor = buttonEditor;
                    break;
            }
            cell.ImeMode = imeMode;
            cell.Enabled = isEnabled;
            format.Rows[row].Cells.Add(cell);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 複数表表示のフォーマット設定
        /// </summary>
        /// <param name="format">MultiRowFormat</param>
        /// <param name="colType">グリッドに追加する列のタイプ</param>
        /// <param name="col">列のインデックス</param>
        /// <param name="row">行のインデックス</param>
        /// <param name="colSpan">列の連結数</param>
        /// <param name="rowSpan">行の連結数</param>
        /// <param name="dataField">データフィールド</param>
        /// <param name="digit">フォーマット文</param>
        /// <param name="isEnabled">セルが操作可能かどうか</param>
        /// <param name="imeMode">IMEモード</param>
        /// <param name="actions">Enterキー押下時のアクション</param>
        /// <param name="maxLength">MaxLength</param>
        /// <create>M.Tsutsumi 2010/08/02</create>
        /// <update>M.Tsutsumi 2010/09/02</update>
        /// --------------------------------------------------
        private void SetFormat(MultiRowFormat format, ElTabelleColumnType colType, int col, int row, int colSpan, int rowSpan, string dataField, string digit, bool isEnabled, ImeMode imeMode, KeyAction[] actions, int maxLength)
        {
            MultiRowCell cell = new MultiRowCell(col, colSpan, rowSpan, dataField, actions);

            switch (colType)
            {
                case ElTabelleColumnType.Text:
                    TextEditor textEditor = ElTabelleSheetHelper.NewTextEditor();
                    if (!string.IsNullOrEmpty(digit))
                    {
                        textEditor.Format = digit;
                    }
                    textEditor.MaxLength = maxLength;
                    cell.Editor = textEditor;
                    break;
                case ElTabelleColumnType.Number:
                    NumberEditor numberEditor = ElTabelleSheetHelper.NewNumberEditor();
                    if (string.IsNullOrEmpty(digit))
                    {
                        numberEditor.Format = new NumberFormat("###,###,###", string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                        numberEditor.DisplayFormat = new NumberFormat("###,###,###", string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                    }
                    else
                    {
                        numberEditor.Format = new NumberFormat(digit, string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                        numberEditor.DisplayFormat = new NumberFormat(digit, string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                    }
                    if (maxLength != 0)
                    {
                        string maxValue = "9";
                        maxValue = maxValue.PadLeft(maxLength, '9');
                        numberEditor.MaxValue = DSWUtil.UtilConvert.ToDecimal(maxValue);
                        numberEditor.MinValue = 0;
                    }
                    cell.Editor = numberEditor;
                    break;
                case ElTabelleColumnType.Button:
                    ButtonEditor buttonEditor = ElTabelleSheetHelper.NewButtonEditor();
                    buttonEditor.Text = null;
                    cell.Editor = buttonEditor;
                    break;
            }
            cell.ImeMode = imeMode;
            cell.Enabled = isEnabled;
            format.Rows[row].Cells.Add(cell);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 複数表表示のフォーマット設定
        /// </summary>
        /// <param name="format">MultiRowFormat</param>
        /// <param name="colType">グリッドに追加する列のタイプ</param>
        /// <param name="col">列のインデックス</param>
        /// <param name="row">行のインデックス</param>
        /// <param name="colSpan">列の連結数</param>
        /// <param name="rowSpan">行の連結数</param>
        /// <param name="dataField">データフィールド</param>
        /// <param name="digit">フォーマット文</param>
        /// <param name="isEnabled">セルが操作可能かどうか</param>
        /// <param name="imeMode">IMEモード</param>
        /// <param name="actions">Enterキー押下時のアクション</param>
        /// <param name="maxLength">MaxLength</param>
        /// <param name="isLengthAsByte">最大文字数をバイト単位と文字単位のどちらで指定するか</param>
        /// <create>H.Tajimi 2015/11/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetFormat(MultiRowFormat format, ElTabelleColumnType colType, int col, int row, int colSpan, int rowSpan, string dataField, string digit, bool isEnabled, ImeMode imeMode, KeyAction[] actions, int maxLength, bool isLengthAsByte)
        {
            MultiRowCell cell = new MultiRowCell(col, colSpan, rowSpan, dataField, actions);

            switch (colType)
            {
                case ElTabelleColumnType.Text:
                    TextEditor textEditor = ElTabelleSheetHelper.NewTextEditor();
                    if (!string.IsNullOrEmpty(digit))
                    {
                        textEditor.Format = digit;
                    }
                    textEditor.MaxLength = maxLength;
                    textEditor.LengthAsByte = isLengthAsByte;
                    cell.Editor = textEditor;
                    break;
                case ElTabelleColumnType.Number:
                    NumberEditor numberEditor = ElTabelleSheetHelper.NewNumberEditor();
                    if (string.IsNullOrEmpty(digit))
                    {
                        numberEditor.Format = new NumberFormat("###,###,###", string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                        numberEditor.DisplayFormat = new NumberFormat("###,###,###", string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                    }
                    else
                    {
                        numberEditor.Format = new NumberFormat(digit, string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                        numberEditor.DisplayFormat = new NumberFormat(digit, string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                    }
                    if (maxLength != 0)
                    {
                        string maxValue = "9";
                        maxValue = maxValue.PadLeft(maxLength, '9');
                        numberEditor.MaxValue = DSWUtil.UtilConvert.ToDecimal(maxValue);
                        numberEditor.MinValue = 0;
                    }
                    cell.Editor = numberEditor;
                    break;
                case ElTabelleColumnType.Button:
                    ButtonEditor buttonEditor = ElTabelleSheetHelper.NewButtonEditor();
                    buttonEditor.Text = null;
                    cell.Editor = buttonEditor;
                    break;
            }
            cell.ImeMode = imeMode;
            cell.Enabled = isEnabled;
            format.Rows[row].Cells.Add(cell);
        }

        #endregion

        #region 画面再表示(再検索)

        /// --------------------------------------------------
        /// <summary>
        /// 画面再表示(再検索)
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void DisplayRefresh()
        {
            // 再描画
            if (!this.RunSearch())
            {
                // 再描画時に失敗したので画面をクリアする。
                this.DisplayClear();
            }
        }

        #endregion

        #region 画像ファイル処理

        #region Upload

        /// --------------------------------------------------
        /// <summary>
        /// ファイルのアップロード処理
        /// </summary>
        /// <param name="kojiNo"></param>
        /// <param name="fileName"></param>
        /// <param name="delFileName"></param>
        /// <returns></returns>
        /// <create>M.Tsutsumi 2010/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool FileUpload(string kojiNo, string fileName, string delFileName)
        {
            try
            {
                using (Stream strm = new MemoryStream())
                {
                    this.picCaseMark.Image.Save(strm, System.Drawing.Imaging.ImageFormat.Png);

                    ConnAttachFile conn = new ConnAttachFile();
                    FileUploadPackage package = new FileUploadPackage();

                    byte[] data = new byte[strm.Length];
                    strm.Position = 0;
                    strm.Read(data, 0, (int)strm.Length);
                    package.FileData = data;
                    package.FileName = fileName;
                    package.DeleteFileName = delFileName;
                    package.FileType = FileType.CaseMark;
                    package.KojiNo = kojiNo;

                    FileUploadResult result = conn.FileUpload(package);
                    if (!result.IsSuccess)
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

        #region Delete

        /// --------------------------------------------------
        /// <summary>
        /// ファイルのデリート処理
        /// </summary>
        /// <param name="kojiNo"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <create>M.Tsutsumi 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool FileDelete(string kojiNo, string fileName)
        {
            try
            {
                ConnAttachFile conn = new ConnAttachFile();
                FileDeletePackage package = new FileDeletePackage();

                package.FileName = fileName;
                package.FileType = FileType.CaseMark;
                package.KojiNo = kojiNo;

                FileDeleteResult result = conn.FileDelete(package);
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

        #region Download

        /// --------------------------------------------------
        /// <summary>
        /// ファイルのダウンロード処理
        /// </summary>
        /// <param name="kojiNo"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <create>M.Tsutsumi 2010/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool FileDownload(string kojiNo, string fileName)
        {
            try
            {
                ConnAttachFile conn = new ConnAttachFile();
                FileDownloadPackage package = new FileDownloadPackage();

                package.FileName = fileName;
                package.FileType = FileType.CaseMark;
                package.KojiNo = kojiNo;

                FileDownloadResult result = conn.FileDownload(package);
                if (!result.IsExistsFile)
                {
                    // ファイルがありません。
                    return false;
                }
                using (MemoryStream memStrm = new MemoryStream(result.FileData))
                {
                    Bitmap bmp = null;
                    bmp = (Bitmap)Bitmap.FromStream(memStrm);
                    if (bmp == null)
                    {
                        // null
                        return false;
                    }
                    this.picCaseMark.Image = bmp;
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 重複チェック

        /// --------------------------------------------------
        /// <summary>
        /// 重複データのインデックス取得
        /// </summary>
        /// <remarks>
        /// 配列の中から引数に指定された値を検索し、見つかった場合は
        /// その値の配列内でのインデックスを返します。
        /// 見つからなかった場合は、-1を返します。
        /// </remarks>
        /// <param name="array">重複チェック対象データの配列</param>
        /// <param name="value">重複チェックしたい値</param>
        /// <param name="index">重複チェックしたい値のリスト内のインデックス</param>
        /// <returns>重複データのインデックス</returns>
        /// <create>H.Tajimi 2015/11/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private int GetDuplicateDataIndex(string[] array, string value, int index)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (i != index && array[i] == value)
                {
                    return i;
                }
            }
            return -1;
        }

        #endregion

        #region 画面表示

        #region 工事識別一覧画面表示

        /// --------------------------------------------------
        /// <summary>
        /// 工事識別一覧画面表示
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2015/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ShowKojiShikibetsuIchiran()
        {
            string torokuFlag = this.TorokuFlag;
            string kojiName = this.txtKojiName.Text;
            string ship = this.txtShip.Text;
            using (KojiShikibetsuIchiran frm = new KojiShikibetsuIchiran(this.UserInfo, torokuFlag, kojiName, ship, true))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    DataRow dr = frm.SelectedRowData;
                    if (dr == null) return false;
                    // 選択データを設定
                    this.txtKojiName.Text = ComFunc.GetFld(dr, Def_T_KIWAKU.KOJI_NAME);
                    this.txtShip.Text = ComFunc.GetFld(dr, Def_T_KIWAKU.SHIP);
                    this.txtKojiNo.Text = ComFunc.GetFld(dr, Def_T_KIWAKU.KOJI_NO);
                    return this.RunSearch();
                }
            }
            return false;
        }

        #endregion

        #region 物件名一覧画面表示

        /// --------------------------------------------------
        /// <summary>
        /// 物件名一覧画面表示
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/12/25</create>
        /// <update>K.Tsutsumi 2018/01/20 AR対応</update>
        /// --------------------------------------------------
        private bool ShowBukkenMeiIchiran()
        {
            string shukkaFlag = SHUKKA_FLAG.NORMAL_VALUE1;
            string bukkenName = this.txtKojiName.Text;
            using (var frm = new BukkenMeiIchiran(this.UserInfo, shukkaFlag, bukkenName, true))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    DataRow dr = frm.SelectedRowData;
                    if (dr == null) return false;
                    // 選択データを設定
                    this.txtKojiName.Text = ComFunc.GetFld(dr, Def_M_BUKKEN.BUKKEN_NAME);
                    this._bukkenNo = ComFunc.GetFld(dr, Def_M_BUKKEN.BUKKEN_NO);
                    return this.RunSearch();
                }
            }
            return false;
        }

        #endregion

        #region 納入先一覧画面表示

        /// --------------------------------------------------
        /// <summary>
        /// 納入先一覧画面表示
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/09/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ShowNonyusakiIchiran()
        {
            // 登録(AR)の場合は、便を納入先一覧の検索条件とはしないし、選択結果を画面に反映もさせない
            var isAR = false;
            if (this.rdoInsertAR.Checked)
            {
                isAR = true;
            }

            var nonyusakiName = this.txtKojiName.Text;
            var shukkaFlag = SHUKKA_FLAG.NORMAL_VALUE1;
            var ship = this.txtShip.Text;
            if (isAR)
            {
                // 登録(AR)の場合は、便を検索条件には含めない
                shukkaFlag = SHUKKA_FLAG.AR_VALUE1;
                ship = string.Empty;
            }

            // 納入先一覧を表示
            using (NonyusakiIchiran frm = new NonyusakiIchiran(this.UserInfo, shukkaFlag, nonyusakiName, ship, true))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    DataRow dr = frm.SelectedRowData;
                    if (dr == null) return false;
                    // 選択データを設定
                    this._bukkenNo = ComFunc.GetFld(dr, Def_M_NONYUSAKI.BUKKEN_NO);
                    this.txtKojiName.Text = ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_NAME);
                    if (!isAR)
                    {
                        this.txtShip.Text = ComFunc.GetFld(dr, Def_M_NONYUSAKI.SHIP);
                    }
                    return this.RunSearch();
                }
            }
            return false;
        }

        #endregion

        #endregion
    }
}
