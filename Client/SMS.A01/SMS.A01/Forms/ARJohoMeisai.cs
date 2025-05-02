using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Linq;

using DSWUtil;
using DSWControl.DSWComboBox;
using WsConnection;
using WsConnection.WebRefCommon;
using Commons;
using SystemBase.Controls;
using SystemBase.Util;
using GrapeCity.Win.ElTabelle;
using ElTabelleHelper;

using WsConnection.WebRefA01;
using WsConnection.WebRefAttachFile;
using System.Diagnostics;
using SMS.P02.Forms;
using SMS.A01.Properties;
using GrapeCity.Win.ElTabelle.Editors;
using DSWControl.DSWRichTextBox;
using Ionic.Zip;

namespace SMS.A01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// AR情報明細登録
    /// </summary>
    /// <create>M.Tsutsumi 2010/08/19</create>
    /// <update></update>
    /// <remarks>
    /// この画面はテーブルレイアウトパネルでレイアウトされています。
    /// コントロールを追加される場合は、各テーブルのプロパティ(右上に出る三角)から
    /// 行および列の編集を選択して行を追加してコントロールを追加してください。
    /// 注意点:
    ///     テーブルレイアウト内のコントロールは基本的にDockプロパティを指定しています。
    ///     隙間なく配置したい場合はMarginに0を指定してください。
    ///     順序を変更する場合はDockプロパティをNoneへ変更してから移動させてください。
    /// セル結合:
    ///     セル結合を行う場合はテーブルレイアウトパネル上のコントロールのプロパティに
    ///     ColmunSpanやRowSpanといったプロパティを使ってください。
    /// <pre>
    /// +-------------------(tblContent)----------------------+
    /// |+---------------------------------------------------+|
    /// ||                 tblNonyusaki                      ||
    /// |+---------------------------------------------------+|
    /// |+----------------------++---------------------------+|
    /// ||        tblJokyo      ||       tblGenchi           ||
    /// ||                      ||                           ||
    /// |+----------------------++---------------------------+|
    /// +-------------------(tblContent)----------------------+
    /// </pre>
    /// </remarks>
    /// --------------------------------------------------
    public partial class ARJohoMeisai : SystemBase.Forms.CustomOrderForm
    {
        #region 定数

        private const int GIREN_FILE_NAME_SIZE = 35;
        private const int REF_FILE_NAME_SIZE = GIREN_FILE_NAME_SIZE;
        private const string ARCOST_CHECK_ON = "1";

        #region Sheet関連

        private const int SHEET_COL_ITEM = 0;
        private const int SHEET_COL_WORK_TIME = 1;
        private const int SHEET_COL_WORKERS = 2;
        private const int SHEET_COL_NUMBER = 3;
        private const int SHEET_COL_RATE = 4;
        private const int SHEET_COL_TOTAL = 5;

        #endregion

        #endregion

        #region Fields

        private string _nonyusakiCd = string.Empty;
        private string _nonyusakiName = string.Empty;
        private string _listFlag = string.Empty;
        private string _listFlagName = string.Empty;
        private string _arNo = string.Empty;

        // AR情報データ待避
        private DataTable _dtAR = null;

        // DBに保存されている技連File名
        private string _dbGirenFile1 = string.Empty;
        private string _dbGirenFile2 = string.Empty;
        private string _dbGirenFile3 = string.Empty;
        private string _dbGirenFile4 = string.Empty;
        private string _dbGirenFile5 = string.Empty;

        // 新たに添付された技連FullPath名
        private string _attachGirenFullPath1 = string.Empty;
        private string _attachGirenFullPath2 = string.Empty;
        private string _attachGirenFullPath3 = string.Empty;
        private string _attachGirenFullPath4 = string.Empty;
        private string _attachGirenFullPath5 = string.Empty;

        // 新たに添付された技連File名
        private string _attachGirenFile1 = string.Empty;
        private string _attachGirenFile2 = string.Empty;
        private string _attachGirenFile3 = string.Empty;
        private string _attachGirenFile4 = string.Empty;
        private string _attachGirenFile5 = string.Empty;

        // 削除する技連File名
        private string _delGirenFile1 = string.Empty;
        private string _delGirenFile2 = string.Empty;
        private string _delGirenFile3 = string.Empty;
        private string _delGirenFile4 = string.Empty;
        private string _delGirenFile5 = string.Empty;

        // DBに保存されている参考File名
        private string _dbRefFile1 = string.Empty;
        private string _dbRefFile2 = string.Empty;

        // 新たに添付された参考FullPath名
        private string _attachRefFullPath1 = string.Empty;
        private string _attachRefFullPath2 = string.Empty;

        // 新たに添付された参考File名
        private string _attachRefFile1 = string.Empty;
        private string _attachRefFile2 = string.Empty;

        // 削除する参考File名
        private string _delRefFile1 = string.Empty;
        private string _delRefFile2 = string.Empty;

        // デフォルトメッセージID
        private string _defMsgInsertEnd = string.Empty;
        private string _defMsgUpdateEnd = string.Empty;
        private string _defMsgInsertConfirm = string.Empty;
        private string _defMsgUpdateConfirm = string.Empty;

        //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36
        // リスト区分名称
        private string _listFlagName0 = string.Empty;
        private string _listFlagName1 = string.Empty;
        private string _listFlagName2 = string.Empty;
        private string _listFlagName3 = string.Empty;
        private string _listFlagName4 = string.Empty;
        private string _listFlagName5 = string.Empty;
        private string _listFlagName6 = string.Empty;
        private string _listFlagName7 = string.Empty;
        // @@@ ↑

        // 対象となる元AR番号のリスト区分
        private string _targetMotoListFlag = string.Empty;
        private string _targetMotoName = string.Empty;

        // 物件管理No
        private string _bukkenNo = string.Empty;

        // 該当納入先コードに登録されている号機数
        private int _gokiNum = 0;

        // 機種リスト
        private readonly List<string> _kishuList = new List<string>();

        // 汎用マスタ(リスト区分_進捗)
        private DataTable _dtArListFlagShinchoku = null;

        // AR進捗利用有無デフォルト値
        private string _cboShinchokuRiyouFlagDefaultValue = null;

        // AR進捗データ
        private DataTable _dtARShinchokuData = null;

        // 進捗管理対象号機リスト
        private readonly List<string> _gokiShinchokuList = new List<string>();

        // 進捗管理対象号機リスト(日付あり)
        private readonly List<string> _gokiShinchokuDateList = new List<string>();

        // セパレータ
        private readonly char _separator;
        private readonly char _separatorRange;

        // 添付するファイルリスト
        private List<string> attachments = new List<string>();

        // メール通知必要か
        private bool _isNotify = false;

        #region 詳細フォーム
        /// --------------------------------------------------
        /// <summary>
        /// AR情報詳細
        /// 0: 不具合内容
        /// 1: 対策内容
        /// 2: 備考
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private readonly ARJohoMeisaiNote[] _detailForms = new ARJohoMeisaiNote[3];
        #endregion

        /// --------------------------------------------------
        /// <summary>
        /// スタッフ区分
        /// </summary>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _staffKbn = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// AR対応費用チェック要否フラグ
        /// </summary>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _isChkARCost = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 指示文言
        /// </summary>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _shijiMongon = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 元ARNo
        /// </summary>
        /// <create>D.Okumura 2020/01/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _motoArNo = null;

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
        /// <create>M.Tsutsumi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        [Obsolete()]
        public ARJohoMeisai(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
            : base(userInfo, menuCategoryID, menuItemID, title)
        {
            InitializeComponent();
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="editMode">編集モード</param>
        /// <param name="title">画面タイトル</param>
        /// <param name="cond">画面パラメータ(納入先コード,納入先名称,リスト区分,ARNo,リスト区分名称(0-7))</param>
        /// <param name="listFlagName">リスト表示名</param>
        /// <param name="bukkenNo">物件管理No</param>
        /// <create>M.Tsutsumi 2010/08/19</create>
        /// <update>D.Okumura 2019/07/22 進捗対応、区切り文字を取得</update>
        /// --------------------------------------------------
        public ARJohoMeisai(UserInfo userInfo, SystemBase.EditMode editMode, string title, CondA1 cond, string listFlagName, string bukkenNo, string motoArNo)
            : base(userInfo, title)
        {
            this.EditMode = editMode;
            this._listFlagName = listFlagName;
            // @@@ 2011/02/16 M.Tsutsumi Change Conditionもらうからついでに。
            //this._nonyusakiCd = nonyusakiCd;
            //this._nonyusakiName = nonyusakiName;
            //this._listFlag = listFlag;
            //this._arNo = arNo;
            this._nonyusakiCd = cond.NonyusakiCD;
            if (string.IsNullOrEmpty(this._nonyusakiCd))
            {
                this._nonyusakiCd = null;
            }
            this._nonyusakiName = cond.NonyusakiName;
            this._listFlag = cond.ListFlag;
            this._arNo = cond.ArNo;
            // @@@ ↑

            //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36
            this._listFlagName0 = cond.ListFlagName0;
            this._listFlagName1 = cond.ListFlagName1;
            this._listFlagName2 = cond.ListFlagName2;
            this._listFlagName3 = cond.ListFlagName3;
            this._listFlagName4 = cond.ListFlagName4;
            this._listFlagName5 = cond.ListFlagName5;
            this._listFlagName6 = cond.ListFlagName6;
            this._listFlagName7 = cond.ListFlagName7;
            // @@@ ↑

            this._bukkenNo = bukkenNo;
            //this._mailAddress = cond.MailAddress;

            this.Condition = cond;

            this._separator = userInfo.SysInfo.SeparatorItem;
            this._separatorRange = userInfo.SysInfo.SeparatorRange;

            this._motoArNo = motoArNo;

            InitializeComponent();
        }

        #endregion

        #region プロパティ

        /// --------------------------------------------------
        /// <summary>
        /// 表示する際に使用した条件
        /// </summary>
        /// <create>T.Sakiori 2012/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public CondA1 Condition { get; private set; }

        /// --------------------------------------------------
        /// <summary>
        /// 納入先
        /// </summary>
        /// <create>M.Shimizu 2020/04/17 AR進捗・関連付け対応</create>
        /// <update></update>
        /// --------------------------------------------------
        public string NonyusakiCd { get { return this._nonyusakiCd; } }

        /// --------------------------------------------------
        /// <summary>
        /// ARNo
        /// </summary>
        /// <create>M.Shimizu 2020/04/17 AR進捗・関連付け対応</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ArNo { get { return this._arNo; } }

        /// --------------------------------------------------
        /// <summary>
        /// 編集モード
        /// </summary>
        /// <create>M.Shimizu 2020/04/17 AR進捗・関連付け対応</create>
        /// <update></update>
        /// --------------------------------------------------
        public SystemBase.EditMode Mode { get { return this.EditMode; } }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/19</create>
        /// <update>H.Tajimi 2018/10/15 FE要望対応</update>
        /// <update>D.Okumura 2019/07/22 進捗対応</update>
        /// <update>T.Nukaga 2019/11/21 STEP12 AR7000番運用対応</update>
        /// <update>M.Shimizu 2020/04/07 AR進捗・関連付け対応</update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // ここにコントロールの初期化を記述する。
                // ベースでDisplayClearの呼出しは行われています。

                // 号機数取得
                if (this._nonyusakiCd == null)
                {
                    this._gokiNum = 0;
                }
                else
                {
                    var conn = new ConnA01();
                    this._gokiNum = conn.GetGokiNum(this.Condition);
                }

                // 汎用マスタ(リスト区分_進捗)
                this._dtArListFlagShinchoku = GetCommon(AR_LIST_FLAG_SHINCHOKU.GROUPCD).Tables[Def_M_COMMON.Name];

                // メッセージを待避
                // 登録完了メッセージ
                this._defMsgInsertEnd = this.MsgInsertEnd;
                // 修正完了メッセージ
                this._defMsgUpdateEnd = this.MsgUpdateEnd;
                // 登録確認メッセージ
                this._defMsgInsertConfirm = this.MsgInsertConfirm;
                // 修正確認メッセージ
                this._defMsgUpdateConfirm = this.MsgUpdateConfirm;

                // 終了時メッセージ ON
                if (this.EditMode != SystemBase.EditMode.View)
                {
                    this.IsCloseQuestion = true;
                }

                // ラベル色
                this.InitializeLabel();

                // テキストの初期化
                this.InitializeText();

                // 日付の初期化
                this.InitializeDateTimePicker();

                // コンボボックスの初期化
                this.InitializeCombo();

                // 元ARNo.、結果ARNo.表示切替
                this.InitializeARRelation();

                // 指示文言の取得
                var dtListFlag = this.GetCommonListFlag();
                this._isChkARCost = ComFunc.GetFld(dtListFlag, 0, Def_M_COMMON.VALUE2);
                this._shijiMongon = ComFunc.GetFld(dtListFlag, 0, Def_M_COMMON.VALUE3);
                this.lblShijiMongon.Text = this._shijiMongon;

                // スタッフ区分の取得
                this._staffKbn = this.GetStaffKbn();

                // シートの初期化
                this.InitializeSheet(this.shtARCost);

                // ボタンの初期化
                this.InitializeButton();

                //// 状況
                //this.cboJyokyoFlag.SelectedValue = JYOKYO_FLAG.DEFAULT_VALUE1;

                // 注:RunSearchの中でモードを切替てるので、検索とモード切替の処理は入れ替えてはダメ！
                // 検索
                this.RunSearch();

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
        /// <create>M.Tsutsumi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカスの設定
                if (this.EditMode == SystemBase.EditMode.Insert)
                {
                    this.cboJyokyoFlag.Focus();
                }
                else if (this.EditMode == SystemBase.EditMode.Update)
                {
                    this.cboJyokyoFlag.Focus();
                }
                else
                {
                    this.fbrFunction.F12Button.Focus();
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #region Text初期化

        /// --------------------------------------------------
        /// <summary>
        /// Text初期化
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/19</create>
        /// <update>H.Tajimi 2015/12/01 不具合内容と対策内容を1000byteから2000byteに拡張</update>
        /// <update>H.Tajimi 2018/10/12 FE要望対応</update>
        /// <update>D.Okumura 2019/06/18 添付ファイル対応</update>
        /// <update>D.Okumura 2019/06/18 リッチテキスト対応</update>
        /// <update>T.Nukaga 2019/11/22 STEP12 AR7000番運用対応</update>
        /// --------------------------------------------------
        private void InitializeText()
        {
            // 連絡者
            this.txtRenrakusha.ImeMode = ImeMode.Hiragana;
            this.txtRenrakusha.InputRegulation = "F";
            this.txtRenrakusha.IsInputRegulation = false;
            this.txtRenrakusha.MaxByteLengthMode = true;
            this.txtRenrakusha.MaxLength = 20;
            // 元ARNO
            this.txtMotoArNo.ImeMode = ImeMode.Disable;
            this.txtMotoArNo.InputRegulation = "abn";
            this.txtMotoArNo.IsInputRegulation = true;
            this.txtMotoArNo.MaxByteLengthMode = true;
            this.txtMotoArNo.MaxLength = 4;
            // 不具合内容
            this.txtHuguai.ImeMode = ImeMode.Hiragana;
            this.txtHuguai.InputRegulation = "F";
            this.txtHuguai.IsInputRegulation = false;
            this.txtHuguai.MaxByteLengthMode = true;
            this.txtHuguai.MaxLength = 2000;
            this.txtHuguai.Multiline = true;
            this.txtHuguai.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
            // 対策内容
            this.txtTaisaku.ImeMode = ImeMode.Hiragana;
            this.txtTaisaku.InputRegulation = "F";
            this.txtTaisaku.IsInputRegulation = false;
            this.txtTaisaku.MaxByteLengthMode = true;
            this.txtTaisaku.MaxLength = 2000;
            this.txtTaisaku.Multiline = true;
            this.txtTaisaku.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
            // 手配先
            this.txtGenchiTehaisaki.ImeMode = ImeMode.Hiragana;
            this.txtGenchiTehaisaki.InputRegulation = "F";
            this.txtGenchiTehaisaki.IsInputRegulation = false;
            this.txtGenchiTehaisaki.MaxByteLengthMode = true;
            this.txtGenchiTehaisaki.MaxLength = 20;
            // 出荷方法
            this.txtShukkahoho.ImeMode = ImeMode.Hiragana;
            this.txtShukkahoho.InputRegulation = "F";
            this.txtShukkahoho.IsInputRegulation = false;
            this.txtShukkahoho.MaxByteLengthMode = true;
            this.txtShukkahoho.MaxLength = 40;
            this.txtShukkahoho.ScrollBars = ScrollBars.Both;
            // 運送会社
            this.txtJpUnsokaishaName.ImeMode = ImeMode.Hiragana;
            this.txtJpUnsokaishaName.InputRegulation = "F";
            this.txtJpUnsokaishaName.IsInputRegulation = false;
            this.txtJpUnsokaishaName.MaxByteLengthMode = true;
            this.txtJpUnsokaishaName.MaxLength = 10;
            // 送り状No
            this.txtJpOkurijyoNo.ImeMode = ImeMode.Disable;
            this.txtJpOkurijyoNo.InputRegulation = "abnls";
            this.txtJpOkurijyoNo.IsInputRegulation = true;
            this.txtJpOkurijyoNo.MaxByteLengthMode = true;
            this.txtJpOkurijyoNo.MaxLength = 15;
            // 対応部署
            this.txtTaioBusho.ImeMode = ImeMode.Hiragana;
            this.txtTaioBusho.InputRegulation = "F";
            this.txtTaioBusho.IsInputRegulation = false;
            this.txtTaioBusho.MaxByteLengthMode = true;
            this.txtTaioBusho.MaxLength = 10;
            // GMS発行No
            this.txtGmsHakkoNo.ImeMode = ImeMode.Disable;
            this.txtGmsHakkoNo.InputRegulation = "abnls";
            this.txtGmsHakkoNo.IsInputRegulation = true;
            this.txtGmsHakkoNo.MaxByteLengthMode = true;
            this.txtGmsHakkoNo.MaxLength = 15;
            // 仕様連絡No
            this.txtShiyorenrakuNo.ImeMode = ImeMode.Disable;
            this.txtShiyorenrakuNo.InputRegulation = "abnls";
            this.txtShiyorenrakuNo.IsInputRegulation = true;
            this.txtShiyorenrakuNo.MaxByteLengthMode = true;
            this.txtShiyorenrakuNo.MaxLength = 10;
            // 技連No
            this.txtGirenNo1.ImeMode = ImeMode.Disable;
            this.txtGirenNo1.InputRegulation = "abnls";
            this.txtGirenNo1.IsInputRegulation = true;
            this.txtGirenNo1.MaxByteLengthMode = true;
            this.txtGirenNo1.MaxLength = 22;
            this.txtGirenNo2.ImeMode = this.txtGirenNo1.ImeMode;
            this.txtGirenNo2.InputRegulation = this.txtGirenNo1.InputRegulation;
            this.txtGirenNo2.IsInputRegulation = this.txtGirenNo1.IsInputRegulation;
            this.txtGirenNo2.MaxByteLengthMode = this.txtGirenNo1.MaxByteLengthMode;
            this.txtGirenNo2.MaxLength = this.txtGirenNo1.MaxLength;
            this.txtGirenNo3.ImeMode = this.txtGirenNo1.ImeMode;
            this.txtGirenNo3.InputRegulation = this.txtGirenNo1.InputRegulation;
            this.txtGirenNo3.IsInputRegulation = this.txtGirenNo1.IsInputRegulation;
            this.txtGirenNo3.MaxByteLengthMode = this.txtGirenNo1.MaxByteLengthMode;
            this.txtGirenNo3.MaxLength = this.txtGirenNo1.MaxLength;
            this.txtGirenNo4.ImeMode = this.txtGirenNo1.ImeMode;
            this.txtGirenNo4.InputRegulation = this.txtGirenNo1.InputRegulation;
            this.txtGirenNo4.IsInputRegulation = this.txtGirenNo1.IsInputRegulation;
            this.txtGirenNo4.MaxByteLengthMode = this.txtGirenNo1.MaxByteLengthMode;
            this.txtGirenNo4.MaxLength = this.txtGirenNo1.MaxLength;
            this.txtGirenNo5.ImeMode = this.txtGirenNo1.ImeMode;
            this.txtGirenNo5.InputRegulation = this.txtGirenNo1.InputRegulation;
            this.txtGirenNo5.IsInputRegulation = this.txtGirenNo1.IsInputRegulation;
            this.txtGirenNo5.MaxByteLengthMode = this.txtGirenNo1.MaxByteLengthMode;
            this.txtGirenNo5.MaxLength = this.txtGirenNo1.MaxLength;
            // 参考資料
            this.txtRefNo1.ImeMode = this.txtGirenNo1.ImeMode;
            this.txtRefNo1.InputRegulation = this.txtGirenNo1.InputRegulation;
            this.txtRefNo1.IsInputRegulation = this.txtGirenNo1.IsInputRegulation;
            this.txtRefNo1.MaxByteLengthMode = this.txtGirenNo1.MaxByteLengthMode;
            this.txtRefNo1.MaxLength = this.txtGirenNo1.MaxLength;
            this.txtRefNo2.ImeMode = this.txtGirenNo1.ImeMode;
            this.txtRefNo2.InputRegulation = this.txtGirenNo1.InputRegulation;
            this.txtRefNo2.IsInputRegulation = this.txtGirenNo1.IsInputRegulation;
            this.txtRefNo2.MaxByteLengthMode = this.txtGirenNo1.MaxByteLengthMode;
            this.txtRefNo2.MaxLength = this.txtGirenNo1.MaxLength;
            // 備考
            this.txtBiko.ImeMode = ImeMode.Hiragana;
            this.txtBiko.InputRegulation = "F";
            this.txtBiko.IsInputRegulation = false;
            this.txtBiko.MaxByteLengthMode = true;
            this.txtBiko.MaxLength = 1000;
            this.txtBiko.Multiline = true;
            this.txtBiko.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
        }

        #endregion

        #region DateTimePicker初期化

        /// --------------------------------------------------
        /// <summary>
        /// DateTimePicker初期化
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private void InitializeDateTimePicker()
        {
            // 発生日
            this.dtpHasseiDate.Value = DateTime.Now;
            // 現場到着希望日
            this.dtpGenbaTotyakukibouDate.Value = DateTime.Now;
            // 現地・設定納期
            this.dtpGenchiSetteinokiDate.Value = DateTime.Now;
            // 現地・出荷予定日
            this.dtpGenchiShukkayoteiDate.Value = DateTime.Now;
            // 現地・工場出荷日
            this.dtpGenchiKojyoshukkaDate.Value = DateTime.Now;
            // 日本・設定納期
            this.dtpJpSetteinokiDate.Value = DateTime.Now;
            // 日本・出荷予定日
            this.dtpJpShukkayoteiDate.Value = DateTime.Now;
            // 日本・工場出荷日
            this.dtpJpKojyoshukkaDate.Value = DateTime.Now;
        }

        #endregion

        #region Combo初期化

        /// --------------------------------------------------
        /// <summary>
        /// Combo初期化
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/19</create>
        /// <update>H.Tajimi 2018/10/12 FE要望対応</update>
        /// <update>D.Okumura 2019/07/22 進捗対応</update>
        /// <update>D.Okumura 2020/01/23 進捗利用初期値反映対応</update>
        /// <update>M.Shimizu 2020/04/07 AR進捗・関連付け対応</update>
        /// --------------------------------------------------
        private void InitializeCombo()
        {
            try
            {
                cboJyokyoFlag.DropDownStyle = ComboBoxStyle.DropDownList;
                this.MakeCmbBox(cboJyokyoFlag, "JYOKYO_FLAG");
                // 進捗利用
                this.cboShinchokuRiyouFlag.DropDownStyle = ComboBoxStyle.DropDownList;
                this.MakeCmbBox(this.cboShinchokuRiyouFlag, "AR_SHINCHOKU_FLAG");
                this._cboShinchokuRiyouFlagDefaultValue = this.GetDefaultShinchokuRiyou();

                var cond = new CondA1(this.UserInfo);
                var conn = new ConnA01();
                cond.SelectGroupCode = SELECT_GROUP_CD.KISHU_VALUE1;
                this.cboKishu.ValueMember = Def_M_SELECT_ITEM.SELECT_GROUP_CD;
                this.cboKishu.DisplayMember = Def_M_SELECT_ITEM.ITEM_NAME;
                this.cboKishu.DataSource = conn.GetSelectItem(cond);
                cond.SelectGroupCode = SELECT_GROUP_CD.GOKI_VALUE1;
                this.cboGoki.ValueMember = Def_M_SELECT_ITEM.SELECT_GROUP_CD;
                this.cboGoki.DisplayMember = Def_M_SELECT_ITEM.ITEM_NAME;
                this.cboGoki.DataSource = conn.GetSelectItem(cond);

                this.cboHasseiFactor.DropDownStyle = ComboBoxStyle.DropDown;
                this.cboHasseiFactor.ValueMember = Def_M_COMMON.VALUE1;
                this.cboHasseiFactor.DisplayMember = Def_M_COMMON.ITEM_NAME;
                var groupCd = HASSEI_YOUIN_AR_1.GROUPCD.Substring(0, HASSEI_YOUIN_AR_1.GROUPCD.Length - 1);
                groupCd += this._listFlag;
                this.cboHasseiFactor.DataSource = this.GetCommon(groupCd).Tables[Def_M_COMMON.Name];
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region Label初期化

        /// --------------------------------------------------
        /// <summary>
        /// Label初期化
        /// </summary>
        /// <create>M.Tsutsumi 2010/09/15</create>
        /// <update>H.Tajimi 2018/10/12 FE要望対応</update>
        /// <update>N.Kawamura 2022/10/06 出荷状況自動反映機能追加</update>
        /// --------------------------------------------------
        private void InitializeLabel()
        {
            Color orangeIro = Color.Tomato;
            Color genchiIro = Color.YellowGreen;
            Color japanIro = Color.LightPink;
            Color foreColor = Color.Blue;

            // 現地
            lblGenchi.BackColor = genchiIro;
            lblGenchi.ForeColor = foreColor;
            // 手配先
            lblGenchiTehaisaki.NormalBackColor = genchiIro;
            lblGenchiTehaisaki.ForeColor = Color.White;
            // 現地・設定納期
            lblGenchiSetteinokiDate.NormalBackColor = orangeIro;
            lblGenchiSetteinokiDate.ForeColor = Color.White;
            // 現地・出荷予定日
            lblGenchiShukkayoteiDate.NormalBackColor = orangeIro;
            lblGenchiShukkayoteiDate.ForeColor = Color.White;
            // 現地・工場出荷日
            lblGenchiKojyoshukkaDate.NormalBackColor = orangeIro;
            lblGenchiKojyoshukkaDate.ForeColor = Color.White;
            // 現地・出荷方法
            lblShukkahoho.NormalBackColor = orangeIro;
            lblShukkahoho.ForeColor = Color.White;
            // 日本
            lblJp.BackColor = japanIro;
            lblJp.ForeColor = foreColor;
            // 日本・設定納期
            lblJpSetteinokiDate.NormalBackColor = orangeIro;
            lblJpSetteinokiDate.ForeColor = Color.White;
            // 日本・出荷予定日
            lblJpShukkayoteiDate.NormalBackColor = orangeIro;
            lblJpShukkayoteiDate.ForeColor = Color.White;
            // 日本・工場出荷日
            lblJpKojyoshukkaDate.NormalBackColor = orangeIro;
            lblJpKojyoshukkaDate.ForeColor = Color.White;
            // 日本・運送会社
            lblJpUnsokaishaName.NormalBackColor = orangeIro;
            lblJpUnsokaishaName.ForeColor = Color.White;
            // 日本・送り状NO
            lblJpOkurijyoNo.NormalBackColor = orangeIro;
            lblJpOkurijyoNo.ForeColor = Color.White;
            // 対応部署
            lblTaioBusho.NormalBackColor = orangeIro;
            lblTaioBusho.ForeColor = Color.White;
            // GMS発行No
            lblGmsHakkoNo.NormalBackColor = orangeIro;
            lblGmsHakkoNo.ForeColor = Color.White;
            // 仕様連絡No
            lblShiyorenrakuNo.NormalBackColor = orangeIro;
            lblShiyorenrakuNo.ForeColor = Color.White;
            // 出荷状況
            lblShukkaJokyo.NormalBackColor = orangeIro;
            lblShukkaJokyo.ForeColor = Color.White;
            // 日本出荷状況
            lblJpShukkaJokyo.NormalBackColor = orangeIro;
            lblJpShukkaJokyo.ForeColor = Color.White;
            // 技連No
            lblGiren.NormalBackColor = orangeIro;
            lblGiren.ForeColor = Color.White;
            // 参考資料
            lblReference.NormalBackColor = orangeIro;
            lblReference.ForeColor = Color.White;
            // 備考
            lblBiko.BackColor = orangeIro;
            lblBiko.ForeColor = Color.White;
        }

        #endregion

        #region Sheet初期化

        /// --------------------------------------------------
        /// <summary>
        /// シートの初期化を行うメソッド
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeSheet(Sheet sheet)
        {
            base.InitializeSheet(sheet);
            // Enterキーを下のセルではなく次のセルへ移動するよう変更
            sheet.ShortCuts.Remove(Keys.Enter);
            sheet.ShortCuts.Add(Keys.Enter, new KeyAction[] { KeyAction.EndEdit, KeyAction.NextCell });
            sheet.KeepHighlighted = true;
            sheet.SelectionType = SelectionType.Single;
            sheet.ViewMode = ViewMode.Default;
            try
            {
                int colIndex = 0;
                var cboEditor = this.GetCommonComboEditor(AR_COST_ITEM.GROUPCD);
                this.SetElTabelleColumn(sheet, colIndex, Resources.ARJohoMeisai_Item, false, false, Def_T_AR_COST.ITEM_CD, cboEditor, 110);
                colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.ARJohoMeisai_WorkTime, false, false, Def_T_AR_COST.WORK_TIME, this.GetNumberEditor("#0.0", 0.0m, 99.9m), 80);
                colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.ARJohoMeisai_Workers, false, false, Def_T_AR_COST.WORKERS, this.GetNumberEditor("#0", 0m, 99m), 50);
                colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.ARJohoMeisai_Number, false, false, Def_T_AR_COST.NUMBER, this.GetNumberEditor("##0", 0m, 999m), 50);
                colIndex++;
                if (this._staffKbn == STAFF_KBN.STAFF_VALUE1)
                {
                    this.SetElTabelleColumn(sheet, colIndex, Resources.ARJohoMeisai_Rate, false, false, Def_T_AR_COST.RATE, this.GetNumberEditor("#,##0", 0m, 9999m), 95);
                    colIndex++;
                    var numTotalEditor = this.GetNumberEditor("##,###,###,##0.0", 0m, 99999999999.9m);
                    numTotalEditor.ReadOnly = true;
                    this.SetElTabelleColumn(sheet, colIndex, Resources.ARJohoMeisai_Total, false, false, Def_T_AR_COST.TOTAL, numTotalEditor, 90);
                    colIndex++;
                }
                else
                {
                    this.SetElTabelleColumn(sheet, colIndex, Resources.ARJohoMeisai_Rate, true, true, Def_T_AR_COST.RATE, this.GetNumberEditor("#,##0", 0m, 9999m), 95);
                    colIndex++;
                    var numTotalEditor = this.GetNumberEditor("##,###,###,##0.0", 0m, 99999999999.9m);
                    numTotalEditor.ReadOnly = true;
                    this.SetElTabelleColumn(sheet, colIndex, Resources.ARJohoMeisai_Total, true, true, Def_T_AR_COST.TOTAL, numTotalEditor, 90);
                    colIndex++;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region ボタン初期化

        /// --------------------------------------------------
        /// <summary>
        /// ボタン初期化
        /// </summary>
        /// <create>Y.Nakasato 2019/07/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private void InitializeButton()
        {
            this.btnGokiList.Enabled = false;
        }

        #endregion


        #region AR関連付け初期化
        /// --------------------------------------------------
        /// <summary>
        /// AR関連付け初期化
        /// </summary>
        /// <create>D.Okumura 2019/12/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private void InitializeARRelation()
        {
            try
            {
                // 初期化
                this.lblKekkaArNo.Visible = false;
                this.lblMotoArNo.Visible = false;
                this.fbrFunction.F04Button.Enabled = false; //解除ボタンは原則として無効とする
                this._targetMotoListFlag = string.Empty;

                // 汎用マスタ取得
                var list = this.GetCommon(LIST_FLAG_KEKKA.GROUPCD, Def_M_COMMON.VALUE1, row => row);
                if (!list.ContainsKey(this._listFlag))
                {
                    // 該当する汎用マスタがない場合は関連付けなしとする
                    return;
                }
                // 判定条件取得
                var listItem = list[this._listFlag];
                string value1 = ComFunc.GetFld(listItem, Def_M_COMMON.VALUE1);
                string value2 = ComFunc.GetFld(listItem, Def_M_COMMON.VALUE2);
                string value3 = ComFunc.GetFld(listItem, Def_M_COMMON.VALUE3);
                if (value2 == LIST_FLAG_KEKKA_VALUE2.REF_KEKKA_AR_VALUE1 && !string.IsNullOrEmpty(GetListFlagName(value1)))
                {
                    // 2:結果AR かつ 現在のList区分名が空白以外
                    this.lblKekkaArNo.Visible = true;
                    this.lblMotoArNo.Visible = false;
                    this._targetMotoListFlag = string.Empty;
                    this._targetMotoName = ComFunc.GetFld(listItem, Def_M_COMMON.ITEM_NAME);
                }
                else if (value2 == LIST_FLAG_KEKKA_VALUE2.REF_MOTO_AR_VALUE1 && !string.IsNullOrEmpty(GetListFlagName(value3)))
                {
                    // 1:元AR かつ 参照先List区分名が空白以外
                    this.lblKekkaArNo.Visible = false;
                    // 新規登録で元AR番号未設定の場合
                    if (this.EditMode == SystemBase.EditMode.Insert)
                    {
                        if (string.IsNullOrEmpty(this._motoArNo))
                        {
                            // 一覧画面の明細登録から遷移の場合は、非表示とする
                            this.lblMotoArNo.Visible = false;
                            this.lblMotoArNo.IsNecessary = false;
                        }
                        else
                        {
                            // 一覧の関連付けボタンからの場合は元AR番号を表示し、必須とする
                            this.lblMotoArNo.Visible = true;
                            this.lblMotoArNo.IsNecessary = true;
                        }
                    }
                    else
                    {
                        // そのほかの場合、元AR番号を表示し、任意とする
                        this.lblMotoArNo.Visible = true;
                        this.lblMotoArNo.IsNecessary = false;
                    }
                    this._targetMotoListFlag = value3;
                    this._targetMotoName = ComFunc.GetFld(listItem, Def_M_COMMON.ITEM_NAME);
                }
                else
                {
                    // その他
                    return;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
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
                return this._listFlagName0;
            else if (listFlag == LIST_FLAG.FLAG_1_VALUE1)
                return this._listFlagName1;
            else if (listFlag == LIST_FLAG.FLAG_2_VALUE1)
                return this._listFlagName2;
            else if (listFlag == LIST_FLAG.FLAG_3_VALUE1)
                return this._listFlagName3;
            else if (listFlag == LIST_FLAG.FLAG_4_VALUE1)
                return this._listFlagName4;
            else if (listFlag == LIST_FLAG.FLAG_5_VALUE1)
                return this._listFlagName5;
            else if (listFlag == LIST_FLAG.FLAG_6_VALUE1)
                return this._listFlagName6;
            else if (listFlag == LIST_FLAG.FLAG_7_VALUE1)
                return this._listFlagName7;
            return string.Empty;
        }
        #endregion
        #endregion

        #region コンボボックスエディタ(汎用マスタより生成)

        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスエディタ
        /// </summary>
        /// <param name="groupCd">グループコード</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private SuperiorComboEditor GetCommonComboEditor(string groupCd)
        {
            var cboEditor = ElTabelleSheetHelper.NewSuperiorComboEditor();
            cboEditor.ValueMember = Def_M_COMMON.VALUE1;
            cboEditor.DisplayMember = Def_M_COMMON.ITEM_NAME;
            cboEditor.ValueAsIndex = false;
            cboEditor.Editable = false;
            var dt = this.GetCommon(groupCd).Tables[Def_M_COMMON.Name];
            dt.Rows.InsertAt(dt.NewRow(), 0);
            cboEditor.DataSource = dt;
            return cboEditor;
        }

        #endregion

        #region 数値型エディタ

        /// --------------------------------------------------
        /// <summary>
        /// 数値型エディタ
        /// </summary>
        /// <param name="digit">表示フォーマット</param>
        /// <param name="minValue">最小値</param>
        /// <param name="maxValue">最大値</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private NumberEditor GetNumberEditor(string digit, decimal minValue, decimal maxValue)
        {
            var numEditor = ElTabelleSheetHelper.NewNumberEditor();
            numEditor.DisplayFormat = new NumberFormat(digit, string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
            numEditor.Format = new NumberFormat(digit, string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
            numEditor.SpinOnKeys = false;
            numEditor.MinValue = minValue;
            numEditor.MaxValue = maxValue;
            numEditor.SpinIncrement = 0;
            return numEditor;
        }

        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/20</create>
        /// <update>H.Tajimi 2018/10/15 FE要望対応</update>
        /// <update>D.Okumura 2019/06/18 添付ファイル対応</update>
        /// <create>Y.Nakasato 2019/07/12 AR進捗対応</create>
        /// <update>T.Nukaga 2019/11/21 AR7000番運用対応</update>
        /// <update>N.Kawamura 2022/10/06 出荷状況自動反映機能追加</update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // ベースでClearMessageの呼出しは行われています。
                // 納入先(User)
                this.txtNonyusakiName.Text = string.Empty;
                // リスト
                this.txtListFlag.Text = string.Empty;
                // AR NO
                this.txtArNo.Text = string.Empty;
                // 状況
                this.cboJyokyoFlag.SelectedValue = JYOKYO_FLAG.DEFAULT_VALUE1;
                // 発生日
                this.dtpHasseiDate.Value = DateTime.Now;
                // 連絡者
                this.txtRenrakusha.Text = string.Empty;
                // 進捗管理利用
                this.cboShinchokuRiyouFlag.SelectedIndex = -1;
                // 機種
                this.cboKishu.SelectedIndex = -1;
                // 号機
                this.cboGoki.SelectedIndex = -1;
                // 現場到着希望日
                this.dtpGenbaTotyakukibouDate.Value = null;
                // 発生原因
                this.cboHasseiFactor.SelectedIndex = -1;
                // 不具合内容
                this.txtHuguai.Clear();
                // 対策内容
                this.txtTaisaku.Clear();
                // 現地・手配先
                this.txtGenchiTehaisaki.Text = string.Empty;
                // 現地・設定納期
                this.dtpGenchiSetteinokiDate.Value = null;
                // 現地・出荷予定日
                this.dtpGenchiShukkayoteiDate.Value = null;
                // 現地・工場出荷日
                this.dtpGenchiKojyoshukkaDate.Value = null;
                // 現地・出荷方法
                this.txtShukkahoho.Text = string.Empty;
                // 日本・設定納期
                this.dtpJpSetteinokiDate.Value = null;
                // 日本・出荷予定日
                this.dtpJpShukkayoteiDate.Value = null;
                // 日本・工場出荷日
                this.dtpJpKojyoshukkaDate.Value = null;
                // 日本・運送会社
                this.txtJpUnsokaishaName.Text = string.Empty;
                // 日本・送り状No
                this.txtJpOkurijyoNo.Text = string.Empty;
                // 対応部署
                this.txtTaioBusho.Text = string.Empty;
                // GMS発行No
                this.txtGmsHakkoNo.Text = string.Empty;
                // 仕様連絡No
                this.txtShiyorenrakuNo.Text = string.Empty;
                // 出荷状況
                this.txtShukkaJokyo.Text = string.Empty;
                // 日本出荷状況
                this.txtJpShukkaJokyo.Text = string.Empty;
                // 技連No
                this.txtGirenNo1.Text = string.Empty;
                this.txtGirenFile1.Text = string.Empty;
                this.txtGirenNo2.Text = string.Empty;
                this.txtGirenFile2.Text = string.Empty;
                this.txtGirenNo3.Text = string.Empty;
                this.txtGirenFile3.Text = string.Empty;
                this.txtGirenNo4.Text = string.Empty;
                this.txtGirenFile4.Text = string.Empty;
                this.txtGirenNo5.Text = string.Empty;
                this.txtGirenFile5.Text = string.Empty;
                // 参考資料
                this.txtRefNo1.Text = string.Empty;
                this.txtRefFile1.Text = string.Empty;
                this.txtRefNo2.Text = string.Empty;
                this.txtRefFile2.Text = string.Empty;
                // 備考
                this.txtBiko.Clear();
                // 元ARNo.
                this.txtMotoArNo.Text = string.Empty;
                // 結果ARNo.
                this.txtKekkaArNo.Text = string.Empty;

                // 変数
                this._dbGirenFile1 = string.Empty;
                this._dbGirenFile2 = string.Empty;
                this._dbGirenFile3 = string.Empty;
                this._dbGirenFile4 = string.Empty;
                this._dbGirenFile5 = string.Empty;
                this._attachGirenFile1 = string.Empty;
                this._attachGirenFile2 = string.Empty;
                this._attachGirenFile3 = string.Empty;
                this._attachGirenFile4 = string.Empty;
                this._attachGirenFile5 = string.Empty;
                this._attachGirenFullPath1 = string.Empty;
                this._attachGirenFullPath2 = string.Empty;
                this._attachGirenFullPath3 = string.Empty;
                this._attachGirenFullPath4 = string.Empty;
                this._attachGirenFullPath5 = string.Empty;
                this._delGirenFile1 = string.Empty;
                this._delGirenFile2 = string.Empty;
                this._delGirenFile3 = string.Empty;
                this._delGirenFile4 = string.Empty;
                this._delGirenFile5 = string.Empty;
                this._dbRefFile1 = string.Empty;
                this._dbRefFile2 = string.Empty;
                this._attachRefFile1 = string.Empty;
                this._attachRefFile2 = string.Empty;
                this._attachRefFullPath1 = string.Empty;
                this._attachRefFullPath2 = string.Empty;
                this._delRefFile1 = string.Empty;
                this._delRefFile2 = string.Empty;
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
        /// <create>M.Tsutsumi 2010/08/20</create>
        /// <update>K.Tsutsumi 2018/02/04 単価が入力できない社員が更新できないため、原価グリッドの行単位入力チェックは行わない</update>
        /// <update>D.Okumura 2019/06/20 添付ファイル対応</update>
        /// <update>D.Okumura 2019/07/23 AR進捗対応</update>
        /// <update>T.Nukaga 2019/11/22 STEP12 AR7000番運用対応</update>
        /// <update>J.Chen 2024/08/22 メール通知フラグ取得</update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                var msgConfirm = ComFunc.GetSchemeMultiMessage();

                // 編集内容実行時の入力チェック
                if (this.dtpHasseiDate.Value == null)
                {
                    // 発生日付を入力してください。
                    this.ShowMessage("A0100020007");
                    return false;
                }

                // AR7000番対応での元ARNo入力時
                if (!string.IsNullOrEmpty(this._targetMotoListFlag))
                {
                    if (!ComFunc.CheckARNo(this.txtMotoArNo.Text, this.UserInfo.SysInfo.SeparatorRange))
                    {
                        // 元ARNoの入力が不正です。確認してください。
                        this.ShowMessage("A0100020025");
                        return false;
                    }
                    // 元AR欄が表示状態の場合は必須入力とする
                    if (!string.IsNullOrEmpty(this.txtMotoArNo.Text) && !this.txtMotoArNo.Text.StartsWith(this._targetMotoListFlag))
                    {
                        // 元ARNOは{0}000番台を入力してください。
                        this.ShowMessage("A0100020028", this._targetMotoListFlag, this._targetMotoName);
                        return false;
                    }
                    // 必須入力チェック
                    if (this.lblMotoArNo.IsNecessary && string.IsNullOrEmpty(this.txtMotoArNo.Text))
                    {
                        // 元ARNOは{0}000番台を入力してください。
                        this.ShowMessage("A0100020028", this._targetMotoListFlag, this._targetMotoName);
                        return false;
                    }
                }

                // 状況の選択チェック
                if (this.cboJyokyoFlag.SelectedValue == null)
                {
                    // 状況を選択してください。
                    this.ShowMessage("A0100020012");
                    return false;
                }

                // 進捗管理しない場合
                if ((this._gokiNum == 0)
                 || string.Equals(AR_SHINCHOKU_FLAG.OFF_VALUE1, this.cboShinchokuRiyouFlag.SelectedValue))
                {
                    // 機種
                    if ((string.IsNullOrEmpty(this.cboKishu.Text) == false) && (Encoding.GetEncoding("Shift-JIS").GetByteCount(this.cboKishu.Text) > ComDefine.MAX_BYTE_LENGTH_KISHU))
                    {
                        // {0}には{1}Byte以下で入力して下さい。
                        string[] param = new string[] { this.lblKishu.Text, ComDefine.MAX_BYTE_LENGTH_KISHU.ToString() };
                        this.ShowMessage("A9999999052", param);
                        return false;
                    }

                    // 号機
                    if ((string.IsNullOrEmpty(this.cboGoki.Text) == false) && (Encoding.GetEncoding("Shift-JIS").GetByteCount(this.cboGoki.Text) > ComDefine.MAX_BYTE_LENGTH_GOKI))
                    {
                        // {0}には{1}Byte以下で入力して下さい。
                        string[] param = new string[] { this.lblGoki.Text, ComDefine.MAX_BYTE_LENGTH_GOKI.ToString() };
                        this.ShowMessage("A9999999052", param);
                        return false;
                    }
                }
                // 発生原因
                if ((string.IsNullOrEmpty(this.cboHasseiFactor.Text) == false) && (Encoding.GetEncoding("Shift-JIS").GetByteCount(this.cboHasseiFactor.Text) > ComDefine.MAX_BYTE_LENGTH_HASSEI_YOUIN))
                {
                    // {0}には{1}Byte以下で入力して下さい。
                    string[] param = new string[] { this.lblHasseiFactor.Text, ComDefine.MAX_BYTE_LENGTH_HASSEI_YOUIN.ToString() };
                    this.ShowMessage("A9999999052", param);
                    return false;
                }

                // 子画面を確認し、開いていたら画面を閉じない
                for (var i = 0; i < _detailForms.Length; i++)
                {
                    if (_detailForms[i] == null)
                        continue;
                    _detailForms[i].Activate();
                    this.ShowMessage("A0100020017", _detailForms[i].Text);
                    return false;
                }

                // 不具合要因: 添付ファイル
                if (!this.txtHuguai.CheckCompressedSize(ComDefine.GIREN_FILE_MAX_SIZE, true))
                {
                    //{0}に貼り付け可能なData容量を超えています。Data量を削減してください。
                    this.ShowMessage("A0100020014", lblHuguai.Text);
                    return false;
                }

                // 対策内容:添付ファイル
                if (!this.txtTaisaku.CheckCompressedSize(ComDefine.GIREN_FILE_MAX_SIZE, true))
                {
                    //{0}に貼り付け可能なData容量を超えています。Data量を削減してください。
                    this.ShowMessage("A0100020014", lblTaisaku.Text);
                    return false;
                }

                // 備考: 添付ファイル
                if (!this.txtBiko.CheckCompressedSize(ComDefine.GIREN_FILE_MAX_SIZE, true))
                {
                    //{0}に貼り付け可能なData容量を超えています。Data量を削減してください。
                    this.ShowMessage("A0100020014", lblBiko.Text);
                    return false;
                }

                if (this.EditMode == SystemBase.EditMode.Insert || this.EditMode == SystemBase.EditMode.Update)
                {
                    bool isError = true;

                    // 入力チェックがなくなるため、削除
                    //string[] param = null;

                    for (int rowIndex = 0; rowIndex < this.shtARCost.Rows.Count; rowIndex++)
                    {
                        if (this.shtARCost[SHEET_COL_ITEM, rowIndex].Value != null)
                        {
                            // isErrorは、１行でもITEM欄が選択されているかどうかの確認なので、選択されているのがわかった時点でループを抜ける
                            isError = false;

                            // 下記のチェックは、単価が入力できない社員が更新できないため、原価グリッドの行単位入力チェックは行わない
                            //for (int colIndex = SHEET_COL_WORK_TIME; colIndex < SHEET_COL_TOTAL; colIndex++)
                            //{
                            //    if (!this.shtARCost.Columns[colIndex].Hidden)
                            //    {
                            //        if (this.shtARCost[colIndex, rowIndex].Value == null
                            //         || DSWUtil.UtilConvert.ToDecimal(this.shtARCost[colIndex, rowIndex].Value) == 0.0m)
                            //        {
                            //            param = new string[] { (rowIndex + 1).ToString(), this.shtARCost.ColumnHeaders[colIndex].Caption };
                            //            break;
                            //        }
                            //    }
                            //}
                            break;
                        }
                    }

                    // 入力チェックがなくなるため削除
                    //if (param != null)
                    //{
                    //    this.ShowMessage("A9999999043", param);
                    //    return false;
                    //}

                    if (this._isChkARCost == ARCOST_CHECK_ON && this.cboJyokyoFlag.SelectedValue.ToString() == JYOKYO_FLAG.KANRYO_VALUE1)
                    {
                        if (isError)
                        {
                            // 状況を完了にするためには1つ以上のAR対応費用を入力してください
                            this.ShowMessage("A0100020013");
                            return false;
                        }
                    }

                    bool isCheckShinchoku = (this._gokiNum > 0) && (string.Equals(this.cboShinchokuRiyouFlag.SelectedValue, AR_SHINCHOKU_FLAG.ON_VALUE1));
                    // メール通知設定を確認(ARおよび進捗)
                    {
                        var conn = new ConnA01();
                        var cond = new CondA1(this.UserInfo);
                        cond.BukkenNo = this._bukkenNo;
                        cond.UpdateUserID = this.UserInfo.UserID;
                        cond.IsToroku = this.EditMode == SystemBase.EditMode.Insert;
                        cond.ListFlag = this._listFlag;
                        cond.IsShinchoku = isCheckShinchoku;
                        
                        string errMsgId;
                        string[] errArgs;
                        conn.GetArMailInfo(cond, out errMsgId, out errArgs, out _isNotify);
                        if (!string.IsNullOrEmpty(errMsgId))
                        {
                            this.ShowMessage(errMsgId, errArgs);
                            return false;
                        }
                    }
                    // 進捗管理する場合
                    if (isCheckShinchoku)
                    {
                        // 機種号機整合性チェック
                        if ((!string.IsNullOrEmpty(this.cboKishu.Text) && (!string.IsNullOrEmpty(this.cboGoki.Text)))
                         && (!CheckKishuGoki()))
                        {
                            // 機種に含まれない号機が選択されています。
                            ComFunc.AddMultiMessage(msgConfirm, "A0100020019");
                        }

                        // 既存登録がない場合のみチェックする
                        if (this._gokiShinchokuList.Count > 0 && this.EditMode != SystemBase.EditMode.Insert)
                        {
                            // 新規号機の警告
                            string[] inputGokiArray = this.cboGoki.Tag as string[] ?? new string[0];
                            if (inputGokiArray.Length > 0
                             && (inputGokiArray.Except<string>(this._gokiShinchokuList).Count() > 0))
                            {
                                // 号機が追加されていますが、進捗管理へ日付は反映されません。進捗管理から日付の設定を行ってください。
                                ComFunc.AddMultiMessage(msgConfirm, "A0100020022");
                            }

                            // 日付変更の警告
                            if (!string.Equals(ComFunc.GetFld(this._dtAR, 0, Def_T_AR.GENBA_TOTYAKUKIBOU_DATE).Trim(), this.dtpGenbaTotyakukibouDate.Text.Trim())
                                || !string.Equals(ComFunc.GetFld(this._dtAR, 0, Def_T_AR.GENCHI_SHUKKAYOTEI_DATE).Trim(), this.dtpGenchiShukkayoteiDate.Text.Trim())
                                || !string.Equals(ComFunc.GetFld(this._dtAR, 0, Def_T_AR.JP_SHUKKAYOTEI_DATE).Trim(), this.dtpJpShukkayoteiDate.Text.Trim())
                                )
                            {
                                // 日付が変更されていますが、進捗管理へ反映されません。進捗管理から日付の設定を行ってください。
                                // また、進捗管理から日付が変更された際は、上書きされます。
                                ComFunc.AddMultiMessage(msgConfirm, "A0100020020");
                            }
                        }
                    }
                }

                // マルチメッセージ対応
                if (msgConfirm.Rows.Count > 0)
                {
                    // 確認メッセージを上書きする
                    string confirmMsg = "";
                    switch (this.EditMode)
                    {
                        case SystemBase.EditMode.Insert:    // 登録処理
                            confirmMsg = this._defMsgInsertConfirm;
                            this.MsgInsertConfirm = string.Empty;
                            break;
                        case SystemBase.EditMode.Update:    // 修正処理
                            confirmMsg = this._defMsgUpdateConfirm;
                            this.MsgUpdateConfirm = string.Empty;
                            break;
                    }

                    var result = this.ShowMultiMessage(msgConfirm, confirmMsg);
                    ret &= result.Equals(DialogResult.OK);
                }
                else
                {
                    // 確認メッセージを元に戻す
                    switch (this.EditMode)
                    {
                        case SystemBase.EditMode.Insert:    // 登録処理
                            this.MsgInsertConfirm = this._defMsgInsertConfirm;
                            break;
                        case SystemBase.EditMode.Update:    // 修正処理
                            this.MsgUpdateConfirm = this._defMsgUpdateConfirm;
                            break;
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
        /// <create>M.Tsutsumi 2010/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                // 検索用入力チェック
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
        /// <create>M.Tsutsumi 2010/08/20</create>
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
        /// <create>M.Tsutsumi 2010/08/20</create>
        /// <update>H.Tajimi 2018/10/15 FE要望対応</update>
        /// <update>D.Okumura 2019/06/18 添付ファイル対応</update>
        /// <create>Y.Nakasato 2019/07/12 AR進捗対応</create>
        /// <update>T.Nukaga 2019/11/22 STEP12 AR7000番運用対応</update>
        /// <update>D.Okumura 2020/01/23 進捗利用初期値反映対応</update>
        /// <update>N.Kawamura 2022/10/06 出荷状況自動反映機能追加</update>
        /// <update>J,Chen 2022/11/0 出荷状況自動反映(多言語対応)</update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                // 画面クリア
                this.DisplayClear();

                // データ設定
                this.txtNonyusakiName.Text = this._nonyusakiName;
                this.txtListFlag.Text = this._listFlagName;
                this.txtArNo.Text = this._arNo;

                CondA1 cond = this.GetCondition();
                ConnA01 conn = new ConnA01();

                // 進捗管理有無、納入先未登録の判定
                if (this._gokiNum == 0 || string.IsNullOrEmpty(cond.NonyusakiCD))
                {
                    this.cboShinchokuRiyouFlag.SelectedValue = AR_SHINCHOKU_FLAG.OFF_VALUE1;
                    this.cboShinchokuRiyouFlag.Enabled = false;
                }
                else
                {
                    this.cboShinchokuRiyouFlag.SelectedIndexChanged += new System.EventHandler(this.cboShinchokuRiyouFlag_SelectedIndexChanged);

                    // 機種リスト作成
                    using (var dt = conn.GetKishu(cond))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            this._kishuList.Add(ComFunc.GetFld(dr, Def_T_AR_GOKI.KISHU));
                        }
                    }
                    this.cboShinchokuRiyouFlag.SelectedValue = this._cboShinchokuRiyouFlagDefaultValue; //初期値へ変更する
                    this.cboShinchokuRiyouFlag.Enabled = true;

                    // 初期値へ変更したので、変更処理を呼び出す
                    this.ChangeShinshokuRiyou();
                }


                // 明細編集・照会
                if (this.EditMode != SystemBase.EditMode.Insert)
                {
                    DataSet ds = new DataSet();
                    if (this.EditMode == SystemBase.EditMode.Update)
                    {
                        // 明細編集
                        string errMsgID = string.Empty;
                        bool ret = conn.GetARandInterLock(cond, out ds, out errMsgID);
                        if (!ret)
                        {
                            // 画面タイトルの切替
                            this.Title = ComDefine.TITLE_A0100020 + Resources.ARJohoMeisai_ItemDisplay;
                            this.ResetTitle();

                            this.ShowMessage(errMsgID);
                            //return false;
                            // 参照モードに切替
                            this.EditMode = SystemBase.EditMode.View;
                            // InitializeShownControl前にモードが切り替わらないから
                            this.ActiveControl = this.fbrFunction.F12Button;

                            // 閉じる時にメッセージを表示しない
                            this.IsCloseQuestion = false;
                        }
                        else
                        {
                            // AR情報データ待避
                            this._dtAR = ds.Tables[Def_T_AR.Name].Copy();
                        }
                    }
                    if (this.EditMode == SystemBase.EditMode.View)
                    {
                        // 明細照会
                        ds = conn.GetARData(cond);
                    }

                    if (ds == null || !ds.Tables.Contains(Def_T_AR.Name) ||
                                      ds.Tables[Def_T_AR.Name].Rows.Count < 1)
                    {
                        // 該当の明細は存在しません。
                        this.ShowMessage("A9999999022");
                        return false;
                    }
                    // データ設定
                    DataRow dr = ds.Tables[Def_T_AR.Name].Rows[0];
                   
                    if (this._gokiNum > 0)
                    {
                        // 進捗管理"する"
                        if (AR_SHINCHOKU_FLAG.ON_VALUE1.Equals(ComFunc.GetFld(dr, Def_T_AR.SHINCHOKU_FLAG)))
                            this.cboShinchokuRiyouFlag.SelectedValue = AR_SHINCHOKU_FLAG.ON_VALUE1;
                        // 進捗管理"しない"
                        else
                            this.cboShinchokuRiyouFlag.SelectedValue = AR_SHINCHOKU_FLAG.OFF_VALUE1;
                    }

                    DataRow drJyotai = conn.GetARData(cond).Tables[Def_T_SHUKKA_MEISAI.Name].Rows[0];

                    this.cboJyokyoFlag.SelectedValue = ComFunc.GetFld(dr, Def_T_AR.JYOKYO_FLAG);
                    this.dtpHasseiDate.Value = ComFunc.GetFld(dr, Def_T_AR.HASSEI_DATE);
                    this.txtRenrakusha.Text = ComFunc.GetFld(dr, Def_T_AR.RENRAKUSHA);
                    this.cboKishu.Text = ComFunc.GetFld(dr, Def_T_AR.KISHU);
                    this.dtpGenbaTotyakukibouDate.Value = ComFunc.GetFld(dr, Def_T_AR.GENBA_TOTYAKUKIBOU_DATE);
                    this.cboHasseiFactor.Text = ComFunc.GetFld(dr, Def_T_AR.HASSEI_YOUIN);
                    this.txtHuguai.Text = ComFunc.GetFld(dr, Def_T_AR.HUGUAI);
                    this.txtTaisaku.Text = ComFunc.GetFld(dr, Def_T_AR.TAISAKU);
                    this.txtGenchiTehaisaki.Text = ComFunc.GetFld(dr, Def_T_AR.GENCHI_TEHAISAKI);
                    this.dtpGenchiSetteinokiDate.Value = ComFunc.GetFld(dr, Def_T_AR.GENCHI_SETTEINOKI_DATE);
                    this.dtpGenchiShukkayoteiDate.Value = ComFunc.GetFld(dr, Def_T_AR.GENCHI_SHUKKAYOTEI_DATE);
                    this.dtpGenchiKojyoshukkaDate.Value = ComFunc.GetFld(dr, Def_T_AR.GENCHI_KOJYOSHUKKA_DATE);
                    this.dtpJpSetteinokiDate.Value = ComFunc.GetFld(dr, Def_T_AR.JP_SETTEINOKI_DATE);
                    this.dtpJpShukkayoteiDate.Value = ComFunc.GetFld(dr, Def_T_AR.JP_SHUKKAYOTEI_DATE);
                    this.dtpJpKojyoshukkaDate.Value = ComFunc.GetFld(dr, Def_T_AR.JP_KOJYOSHUKKA_DATE);
                    this.txtShukkahoho.Text = ComFunc.GetFld(dr, Def_T_AR.SHUKKAHOHO);
                    this.txtJpUnsokaishaName.Text = ComFunc.GetFld(dr, Def_T_AR.JP_UNSOKAISHA_NAME);
                    this.txtJpOkurijyoNo.Text = ComFunc.GetFld(dr, Def_T_AR.JP_OKURIJYO_NO);
                    this.txtTaioBusho.Text = ComFunc.GetFld(dr, Def_T_AR.TAIO_BUSHO);
                    this.txtGmsHakkoNo.Text = ComFunc.GetFld(dr, Def_T_AR.GMS_HAKKO_NO);
                    this.txtShiyorenrakuNo.Text = ComFunc.GetFld(dr, Def_T_AR.SHIYORENRAKU_NO);
                    this.txtShukkaJokyo.Text = ComFunc.GetFld(dr, Def_T_AR.GENCHI_SHUKKAJYOKYO_FLAG);
                    this.txtJpShukkaJokyo.Text = ComFunc.GetFld(drJyotai, Def_M_COMMON.ITEM_NAME);
                    this.txtGirenNo1.Text = ComFunc.GetFld(dr, Def_T_AR.GIREN_NO_1);
                    this.txtGirenFile1.Text = ComFunc.GetFld(dr, Def_T_AR.GIREN_FILE_1);
                    this.txtGirenNo2.Text = ComFunc.GetFld(dr, Def_T_AR.GIREN_NO_2);
                    this.txtGirenFile2.Text = ComFunc.GetFld(dr, Def_T_AR.GIREN_FILE_2);
                    this.txtGirenNo3.Text = ComFunc.GetFld(dr, Def_T_AR.GIREN_NO_3);
                    this.txtGirenFile3.Text = ComFunc.GetFld(dr, Def_T_AR.GIREN_FILE_3);
                    this.txtGirenNo4.Text = ComFunc.GetFld(dr, Def_T_AR.GIREN_NO_4);
                    this.txtGirenFile4.Text = ComFunc.GetFld(dr, Def_T_AR.GIREN_FILE_4);
                    this.txtGirenNo5.Text = ComFunc.GetFld(dr, Def_T_AR.GIREN_NO_5);
                    this.txtGirenFile5.Text = ComFunc.GetFld(dr, Def_T_AR.GIREN_FILE_5);
                    this.txtRefNo1.Text = ComFunc.GetFld(dr, Def_T_AR.REFERENCE_NO_1);
                    this.txtRefFile1.Text = ComFunc.GetFld(dr, Def_T_AR.REFERENCE_FILE_1);
                    this.txtRefNo2.Text = ComFunc.GetFld(dr, Def_T_AR.REFERENCE_NO_2);
                    this.txtRefFile2.Text = ComFunc.GetFld(dr, Def_T_AR.REFERENCE_FILE_2);
                    // AR関連付けデータ設定
                    // 元ARに入力がある場合は必須入力にする
                    this.txtMotoArNo.Text = ComFunc.GetFld(dr, Def_T_AR.MOTO_AR_NO).Replace(ComDefine.PREFIX_ARNO, "");
                    this.lblMotoArNo.IsNecessary = !string.IsNullOrEmpty(this.txtMotoArNo.Text);
                    this.fbrFunction.F04Button.Enabled = !string.IsNullOrEmpty(this.txtMotoArNo.Text) && this.EditMode == SystemBase.EditMode.Update;
                    if (this.lblKekkaArLabel.Visible)
                    {
                        this.txtKekkaArNo.Text = ComFunc.GetFld(dr, ComDefine.FLD_KEKKA_AR_NO).Replace(ComDefine.PREFIX_ARNO,"");
                        this.btnKekkaArRef.Enabled = !string.IsNullOrEmpty(this.txtKekkaArNo.Text);
                    }

                    // AR進捗データ
                    if (this._gokiNum > 0 && UtilData.ExistsData(ds, Def_T_AR_SHINCHOKU.Name))
                    {
                        // AR進捗情報収集
                        this._dtARShinchokuData = ds.Tables[Def_T_AR_SHINCHOKU.Name].Copy();

                        // 号機リスト生成
                        foreach (DataRow row in this._dtARShinchokuData.Rows)
                        {
                            this._gokiShinchokuList.Add(ComFunc.GetFld(row, Def_T_AR_SHINCHOKU.GOKI));

                            DateTime temp;
                            if ((DateTime.TryParse(ComFunc.GetFld(row, Def_T_AR_SHINCHOKU.DATE_SITE_REQ), out temp))
                             || (DateTime.TryParse(ComFunc.GetFld(row, Def_T_AR_SHINCHOKU.DATE_LOCAL), out temp))
                             || (DateTime.TryParse(ComFunc.GetFld(row, Def_T_AR_SHINCHOKU.DATE_JP), out temp)))
                            {
                                this._gokiShinchokuDateList.Add(ComFunc.GetFld(row, Def_T_AR_SHINCHOKU.GOKI));
                            }
                        }
                        // 選択済み号機リスト
                        this.cboGoki_SetGokiList(this._gokiShinchokuList.ToArray());
                    }
                    else
                    {
                        this.cboGoki.Tag = null;
                        this.cboGoki.Text = ComFunc.GetFld(dr, Def_T_AR.GOKI);
                    }

                    // AR費用
                    if (UtilData.ExistsData(ds, Def_T_AR_COST.Name))
                    {
                        var dtCost = ds.Tables[Def_T_AR_COST.Name];
                        foreach (DataRow drCost in dtCost.Rows)
                        {
                            var itemCd = ComFunc.GetFldObject(drCost, Def_T_AR_COST.ITEM_CD);
                            var rowIndex = ComFunc.GetFldToInt32(drCost, Def_T_AR_COST.LINE_NO) - 1;
                            if (itemCd != null)
                            {
                                this.shtARCost[SHEET_COL_ITEM, rowIndex].Value = ComFunc.GetFld(drCost, Def_T_AR_COST.ITEM_CD);
                            }
                            this.shtARCost[SHEET_COL_WORK_TIME, rowIndex].Text = ComFunc.GetFld(drCost, Def_T_AR_COST.WORK_TIME);
                            this.shtARCost[SHEET_COL_WORKERS, rowIndex].Text = ComFunc.GetFld(drCost, Def_T_AR_COST.WORKERS);
                            this.shtARCost[SHEET_COL_NUMBER, rowIndex].Text = ComFunc.GetFld(drCost, Def_T_AR_COST.NUMBER);
                            if (ComFunc.GetFldObject(drCost, Def_T_AR_COST.RATE) != null)
                            {
                                this.shtARCost[SHEET_COL_RATE, rowIndex].Text = ComFunc.GetFld(drCost, Def_T_AR_COST.RATE);
                            }
                            if (ComFunc.GetFldObject(drCost, Def_T_AR_COST.TOTAL) != null)
                            {
                                this.shtARCost[SHEET_COL_TOTAL, rowIndex].Text = ComFunc.GetFld(drCost, Def_T_AR_COST.TOTAL);
                            }
                        }
                    }
                    this.txtBiko.Text = ComFunc.GetFld(dr, Def_T_AR.BIKO);
                    // AR添付ファイル
                    // テキスト等は反映済みの状態でコールすること
                    var dtFile = ds.Tables[Def_T_AR_FILE.Name];
                    txtHuguai.GetFilesFromDataTable(dtFile, AR_FILE_TYPE.HUGUAI_VALUE1);
                    txtTaisaku.GetFilesFromDataTable(dtFile, AR_FILE_TYPE.TAISAKU_VALUE1);
                    txtBiko.GetFilesFromDataTable(dtFile, AR_FILE_TYPE.BIKO_VALUE1);

                    // 変数へ
                    this._dbGirenFile1 = ComFunc.GetFld(dr, Def_T_AR.GIREN_FILE_1);
                    this._dbGirenFile2 = ComFunc.GetFld(dr, Def_T_AR.GIREN_FILE_2);
                    this._dbGirenFile3 = ComFunc.GetFld(dr, Def_T_AR.GIREN_FILE_3);
                    this._dbGirenFile4 = ComFunc.GetFld(dr, Def_T_AR.GIREN_FILE_4);
                    this._dbGirenFile5 = ComFunc.GetFld(dr, Def_T_AR.GIREN_FILE_5);
                    this._dbRefFile1 = ComFunc.GetFld(dr, Def_T_AR.REFERENCE_FILE_1);
                    this._dbRefFile2 = ComFunc.GetFld(dr, Def_T_AR.REFERENCE_FILE_2);
                    // ボタン設定
                    bool isEnable;
                    isEnable = !string.IsNullOrEmpty(this._dbGirenFile1);
                    this.ChangeEnableGirenDownloadButton(GirenType.No1, isEnable);
                    this.ChangeEnableGirenDeleteButton(GirenType.No1, false);
                    if (this.EditMode == SystemBase.EditMode.Update)
                    {
                        this.ChangeEnableGirenDeleteButton(GirenType.No1, isEnable);
                    }
                    isEnable = !string.IsNullOrEmpty(this._dbGirenFile2);
                    this.ChangeEnableGirenDownloadButton(GirenType.No2, isEnable);
                    this.ChangeEnableGirenDeleteButton(GirenType.No2, false);
                    if (this.EditMode == SystemBase.EditMode.Update)
                    {
                        this.ChangeEnableGirenDeleteButton(GirenType.No2, isEnable);
                    }
                    isEnable = !string.IsNullOrEmpty(this._dbGirenFile3);
                    this.ChangeEnableGirenDownloadButton(GirenType.No3, isEnable);
                    this.ChangeEnableGirenDeleteButton(GirenType.No3, false);
                    if (this.EditMode == SystemBase.EditMode.Update)
                    {
                        this.ChangeEnableGirenDeleteButton(GirenType.No3, isEnable);
                    }
                    isEnable = !string.IsNullOrEmpty(this._dbGirenFile4);
                    this.ChangeEnableGirenDownloadButton(GirenType.No4, isEnable);
                    this.ChangeEnableGirenDeleteButton(GirenType.No4, false);
                    if (this.EditMode == SystemBase.EditMode.Update)
                    {
                        this.ChangeEnableGirenDeleteButton(GirenType.No4, isEnable);
                    }
                    isEnable = !string.IsNullOrEmpty(this._dbGirenFile5);
                    this.ChangeEnableGirenDownloadButton(GirenType.No5, isEnable);
                    this.ChangeEnableGirenDeleteButton(GirenType.No5, false);
                    if (this.EditMode == SystemBase.EditMode.Update)
                    {
                        this.ChangeEnableGirenDeleteButton(GirenType.No5, isEnable);
                    }
                    isEnable = !string.IsNullOrEmpty(this._dbRefFile1);
                    this.ChangeEnableRefDownloadButton(GirenType.RefNo1, isEnable);
                    this.ChangeEnableRefDeleteButton(GirenType.RefNo1, false);
                    if (this.EditMode == SystemBase.EditMode.Update)
                    {
                        this.ChangeEnableRefDeleteButton(GirenType.RefNo1, isEnable);
                    }
                    isEnable = !string.IsNullOrEmpty(this._dbRefFile2);
                    this.ChangeEnableRefDownloadButton(GirenType.RefNo2, isEnable);
                    this.ChangeEnableRefDeleteButton(GirenType.RefNo2, false);
                    if (this.EditMode == SystemBase.EditMode.Update)
                    {
                        this.ChangeEnableRefDeleteButton(GirenType.RefNo2, isEnable);
                    }

                    // 画像のダウンロード
                    // 複数件のエラーを表示できないため、最初のメッセージのみ表示させる
                    var isSuccessDownload = true;
                    // 編集・新規の場合、A0100020015: {0}内のファイル取得に失敗しました。画像が欠落するため再度画面を開いてください。
                    // 照会の場合、A0100020016: {0}内のファイル取得に失敗しました。一部画像が表示されません。
                    var msgId = this.EditMode == SystemBase.EditMode.View ? "A0100020016" : "A0100020015";
                    try
                    {
                        // 不具合内容
                        if (!DownloadImage(this.txtHuguai) && isSuccessDownload)
                        {
                            ShowMessage(msgId, this.lblHuguai.Text);
                            isSuccessDownload = false;
                        }
                        // 対策内容
                        if (!DownloadImage(this.txtTaisaku) && isSuccessDownload)
                        {
                            ShowMessage(msgId, this.lblTaisaku.Text);
                            isSuccessDownload = false;
                        }
                        // 備考
                        if (!DownloadImage(this.txtBiko) && isSuccessDownload)
                        {
                            ShowMessage(msgId, this.lblBiko.Text);
                            isSuccessDownload = false;
                        }
                    }
                    catch (OutOfMemoryException)
                    {
                        // 貼り付け操作中にMemory不足が発生しました。Applicationを再起動してください。
                        this.ShowMessage("A9999999072");
                    }
                }
                // 明細新規登録
                else
                {
                    // ボタン設定
                    this.ChangeEnableGirenDownloadButton(GirenType.No1, false);
                    this.ChangeEnableGirenDeleteButton(GirenType.No1, false);
                    this.ChangeEnableGirenDownloadButton(GirenType.No2, false);
                    this.ChangeEnableGirenDeleteButton(GirenType.No2, false);
                    this.ChangeEnableGirenDownloadButton(GirenType.No3, false);
                    this.ChangeEnableGirenDeleteButton(GirenType.No3, false);
                    this.ChangeEnableGirenDownloadButton(GirenType.No4, false);
                    this.ChangeEnableGirenDeleteButton(GirenType.No4, false);
                    this.ChangeEnableGirenDownloadButton(GirenType.No5, false);
                    this.ChangeEnableGirenDeleteButton(GirenType.No5, false);
                    this.ChangeEnableRefDownloadButton(GirenType.RefNo1, false);
                    this.ChangeEnableRefDeleteButton(GirenType.RefNo1, false);
                    this.ChangeEnableRefDownloadButton(GirenType.RefNo2, false);
                    this.ChangeEnableRefDeleteButton(GirenType.RefNo2, false);
                    DataSet ds = conn.GetNonyusaki(cond);
                    if (ds == null || !ds.Tables.Contains(Def_M_NONYUSAKI.Name) ||
                                       ds.Tables[Def_M_NONYUSAKI.Name].Rows.Count < 1)
                    {
                        return true;
                    }
                    // データ設定
                    DataRow dr = ds.Tables[Def_M_NONYUSAKI.Name].Rows[0];
                    this._nonyusakiCd = ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_CD);

                    // AR関連付けの初期値設定
                    if (this.lblMotoArNo.Visible && !string.IsNullOrEmpty(this._motoArNo))
                    {
                        this.txtMotoArNo.Text = this._motoArNo.Replace(ComDefine.PREFIX_ARNO, "");
                        this.txtMotoArNo.ReadOnly = true;
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

        /// --------------------------------------------------
        /// <summary>
        /// ファイルダウンロード処理
        /// </summary>
        /// <param name="txt">ARJoho用リッチテキストボックス</param>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <exception cref="OutOfMemoryException">貼り付け中のメモリ不足</exception>
        /// <create>D.Okumura 2019/06/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool DownloadImage(SMS.A01.Controls.ARJohoRichTextBox txt)
        {
            var result = txt.Download();
            // 無条件に画像をロードする
            txt.LoadImages();
            return result;
        }

        #endregion

        #region 編集内容実行

        #region 制御メソッド

        /// --------------------------------------------------
        /// <summary>
        /// 編集内容実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>M.Tsutsumi 2010/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            bool ret = base.RunEdit();
            try
            {
                // ここに登録処理後の画面制御を記述(フォーカス等)
                // ※正常登録時はDisplayClear()は呼出し済です。
                if (ret)
                {
                    // 正常終了
                    this.DialogResult = DialogResult.OK;
                    // 終了時メッセージ OFF
                    this.IsCloseQuestion = false;
                    this.Close();
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
        /// <create>M.Tsutsumi 2010/08/25</create>
        /// <update>H.Tajimi 2018/10/15 FE要望対応</update>
        /// <update>D.Okumura 2019/06/18 添付ファイル対応</update>
        /// <update>D.Okumura 2020/06/01 メール送信Webサービス化対応</update>
        /// <update>J.Chen 2024/08/06 メールに添付ファイル追加</update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                // 登録完了メッセージをデフォルトに戻す。
                this.MsgInsertEnd = this._defMsgInsertEnd;

                // 登録処理
                CondA1 cond = this.GetCondition();
                ConnA01 conn = new ConnA01();
                DataSet ds = new DataSet();

                // 画面情報取得
                DataTable dt = this.GetDisplay(Def_T_AR.Name);
                if (dt == null) return false;
                ds.Tables.Add(dt);
                DataTable dtShinchokuAdd = this.GetDisplayShinchokuAdd(ComDefine.DTTBL_ARSHINCHOKU_ADD);
                ds.Merge(dtShinchokuAdd);
                DataTable dtShinchokuDel = this.GetDisplayShinchokuDelete(ComDefine.DTTBL_ARSHINCHOKU_DEL);
                ds.Merge(dtShinchokuDel);
                DataTable dtCost = this.GetDisplayCost(Def_T_AR_COST.Name);
                ds.Merge(dtCost);
                DataTable dtFile = this.GetDisplayFile(Def_T_AR_FILE.Name);
                ds.Merge(dtFile);

                // メールデータ取得
                string errMsgID = string.Empty;
                string[] errArgs = new string[0];

                // DB登録
                string nonyusakiCd;
                string arNo;
                string[] args;

                // メール通知必要および添付ファイルある場合、ファイルを取得しアップロードする
                if (_isNotify && CheckAttachmentFileNonEmpty())
                {
                    string nonyusakiName = this.txtNonyusakiName.Text;

                    //ファイル名に使用できない文字を取得
                    char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();

                    if (nonyusakiName.IndexOfAny(invalidChars) >= 0)
                    {
                        foreach (char invalidChar in invalidChars)
                        {
                            nonyusakiName = nonyusakiName.Replace(invalidChar.ToString(), "");
                        }
                        //// ファイル名に利用できない文字が含まれています。\r\n続行する場合、無効な文字は空白文字に置き換えられます。\r\n続行しますか？
                        //DialogResult retSelect = this.ShowMessage("T0100040022");
                        //if (retSelect == DialogResult.No)
                        //{
                        //    return false;
                        //}
                    }


                    // 添付ファイルcopy
                    if (!this.AttachmentFileCopy())
                    {
                        // ファイルのアップロードに失敗しました。\r\n再度保存して下さい。
                        this.ShowMessage("A9999999039");
                        // エラーにはしない
                        // 登録完了メッセージ変更
                        this.MsgInsertEnd = "";
                    }

                    // AR_No取得
                    string arNoTemp = conn.GetARNoWithoutUpdate(cond, out errMsgID, out args);
                    if (string.IsNullOrEmpty(arNoTemp))
                    {
                        this.ShowMessage(errMsgID, args);
                        return false;
                    }

                    // zipファイル名
                    string compressedFileName = string.Format(ComDefine.ATTACHMENT_FILE_AR_FOR_ZIP
                            , nonyusakiName
                            , arNoTemp
                            , DateTime.Now.ToString("yyyyMMdd")
                            );
                    string compressedFilePath = Path.Combine(ComDefine.AR_OUTPUT_DIR, compressedFileName);

                    // ファイルの圧縮
                    if (!CompressFilesAsAttachments(attachments, compressedFilePath))
                    {
                        // アップロードに失敗しました。
                        this.ShowMessage("S0100050025");
                        return false;
                    }

                    // ファイルサイズチェック
                    FileInfo fi = new FileInfo(compressedFilePath);
                    long fileSize = fi.Length;
                    bool hasAttachment = true;

                    // 設定した最大サイズ取得
                    DataTable dtCom = this.GetCommon(ATTACHMENT_FILE.GROUPCD).Tables[Def_M_COMMON.Name];
                    var maxFileSize = ComFunc.GetFld(dtCom.AsEnumerable().FirstOrDefault(w => string.Equals(ComFunc.GetFld(w, Def_M_COMMON.ITEM_CD), ATTACHMENT_FILE.ATTACHMENT_MAX_SIZE_NAME)), Def_M_COMMON.VALUE1);
                    long longMaxFileSize;
                    long.TryParse(maxFileSize, out longMaxFileSize);

                    if (longMaxFileSize < fileSize)
                    {
                        if (this.ShowMessage("A0100010017") != DialogResult.Yes) return false;
                        hasAttachment = false;
                    }

                    if (hasAttachment)
                    {
                        // MAIL_ID取得
                        string mailID = conn.GetMailIDWithoutUpdate(cond, out errMsgID, out args);
                        if (string.IsNullOrEmpty(mailID))
                        {
                            this.ShowMessage(errMsgID, args);
                            return false;
                        }
                        cond.MailIDTemp = mailID;
                        cond.ArNoTemp = arNoTemp;

                        // ファイルアップロード(ZIP)
                        if (!this.ArFileUpload(compressedFilePath, compressedFileName, mailID))
                        {
                            // アップロードに失敗しました。
                            this.ShowMessage("S0100050025");
                            return false;
                        }

                        // ファイルパス取得
                        string filePath = ComDefine.WEB_DATA_DIR_ROOT;
                        // 添付ファイルのルート
                        filePath = Path.Combine(filePath, ComDefine.WEB_DATA_DIR_NAME_ATTACHMENTS);
                        // フォルダ名付与
                        if (!string.IsNullOrEmpty(mailID))
                        {
                            filePath = Path.Combine(filePath, mailID);
                        }
                        // ファイル名付与
                        if (!string.IsNullOrEmpty(compressedFileName))
                        {
                            filePath = Path.Combine(filePath, compressedFileName);
                        }
                        cond.FilePathTemp = filePath;
                    }
                }

                if (!conn.InsAR(cond, ds, out nonyusakiCd, out arNo, out errMsgID, out args))
                {
                    this.ShowMessage(errMsgID, args);
                    return false;
                }

                // uploadは成否に関わらずtrue固定
                // 技連ファイルupload
                if (!this.GirenFileUpdate(nonyusakiCd, arNo))
                {
                    // ファイルのアップロードに失敗しました。\r\n再度保存して下さい。
                    this.ShowMessage("A9999999039");
                    // エラーにはしない
                    // 登録完了メッセージ変更
                    this.MsgInsertEnd = "";
                }

                // 参考ファイルupload
                if (!this.RefFileUpdate(nonyusakiCd, arNo))
                {
                    // ファイルのアップロードに失敗しました。\r\n再度保存して下さい。
                    this.ShowMessage("A9999999039");
                    // エラーにはしない
                    // 登録完了メッセージ変更
                    this.MsgInsertEnd = "";
                }
                // リッチテキストファイル
                if (!this.RtfFileUpload(nonyusakiCd, arNo))
                {
                    // ファイルのアップロードに失敗しました。\r\n再度、保存して下さい。
                    this.ShowMessage("A9999999039");
                    // エラーにはしない
                    // 修正完了メッセージ変更
                    this.MsgUpdateEnd = "";
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
        /// <create>M.Tsutsumi 2010/08/25</create>
        /// <update>H.Tajimi 2018/10/15 FE要望対応</update>
        /// <update>D.Okumura 2019/06/18 添付ファイル対応</update>
        /// <update>D.Okumura 2019/07/30 AR進捗対応</update>
        /// <update>D.Okumura 2020/06/01 メール送信Webサービス化対応</update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                // 修正完了メッセージをデフォルトに戻す。
                this.MsgUpdateEnd = this._defMsgUpdateEnd;

                // 修正処理
                CondA1 cond = this.GetCondition();
                ConnA01 conn = new ConnA01();
                DataSet ds = new DataSet();

                // 画面情報取得
                DataTable dt = this.GetDisplay(ComDefine.DTTBL_UPDATE);
                if (dt == null) return false;
                ds.Tables.Add(dt);
                DataTable dtShinchokuAdd = this.GetDisplayShinchokuAdd(ComDefine.DTTBL_ARSHINCHOKU_ADD);
                ds.Merge(dtShinchokuAdd);
                DataTable dtShinchokuDel = this.GetDisplayShinchokuDelete(ComDefine.DTTBL_ARSHINCHOKU_DEL);
                ds.Merge(dtShinchokuDel);
                DataTable dtCost = this.GetDisplayCost(Def_T_AR_COST.Name);
                ds.Merge(dtCost);
                DataTable dtFile = this.GetDisplayFile(Def_T_AR_FILE.Name);
                ds.Merge(dtFile);

                dt = this._dtAR.Copy();
                ds.Tables.Add(dt);

                string errMsgID = string.Empty;
                // DB更新
                string[] args;
                // 2010/10/29 H.Tsunamura Add↓
                if (!conn.ChackMeisaiData(cond, ds, out errMsgID, out args))
                {
                    if (errMsgID == "A0100020003" || errMsgID == "A0100020008")
                    {
                        if (this.ShowMessage(errMsgID, args) != DialogResult.OK)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        this.ShowMessage(errMsgID, args);
                        return false;
                    }
                }
                // 2010/10/29 H.Tsunamura Add↑

                if (!conn.UpdAR(cond, ds, out errMsgID, out args))
                {
                    this.ShowMessage(errMsgID, args);
                    return false;
                }

                // 技連ファイルupload
                if (!this.GirenFileUpdate(this._nonyusakiCd, this._arNo))
                {
                    // ファイルのアップロードに失敗しました。\r\n再度、保存して下さい。
                    this.ShowMessage("A9999999039");
                    // エラーにはしない
                    // 修正完了メッセージ変更
                    this.MsgUpdateEnd = "";
                }
                // 参考資料ファイルupload
                if (!this.RefFileUpdate(this._nonyusakiCd, this._arNo))
                {
                    // ファイルのアップロードに失敗しました。\r\n再度、保存して下さい。
                    this.ShowMessage("A9999999039");
                    // エラーにはしない
                    // 修正完了メッセージ変更
                    this.MsgUpdateEnd = "";
                }
                // リッチテキストファイル
                if (!this.RtfFileUpload(this._nonyusakiCd, this._arNo))
                {
                    // ファイルのアップロードに失敗しました。\r\n再度、保存して下さい。
                    this.ShowMessage("A9999999039");
                    // エラーにはしない
                    // 修正完了メッセージ変更
                    this.MsgUpdateEnd = "";
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
        /// <create>M.Tsutsumi 2010/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditDelete()
        {
            try
            {
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
        /// <create>M.Tsutsumi 2010/09/01</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
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
        /// F4ボタンクリック:AR関連付け解除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2020/01/15</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F04Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F04Button_Click(sender, e);
            try
            {
                if (!this.lblMotoArNo.Visible)
                    return;
                if (!this.lblMotoArNo.IsNecessary)
                    return;
                this.lblMotoArNo.IsNecessary = false;
                this.txtMotoArNo.Clear();
                this.fbrFunction.F04Button.Enabled = false;
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
        /// <create>T.Sakiori 2012/04/27</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F11Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F11Button_Click(sender, e);
            try
            {
                using (var frm = new RirekiShokai(this.UserInfo, GAMEN_FLAG.A0100010_VALUE1, SHUKKA_FLAG.AR_VALUE1, this._nonyusakiCd, this._nonyusakiName, string.Empty, this._arNo.Substring(2)))
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
        /// <create>M.Tsutsumi 2010/09/01</create>
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

        #region HELPボタン

        /// --------------------------------------------------
        /// <summary>
        /// HELPボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnHelp_Click(object sender, EventArgs e)
        {
            try
            {
                // メッセージクリア
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

        #region 削除ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 削除ボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDelete1_Click(object sender, EventArgs e)
        {
            try
            {
                // メッセージクリア
                this.ClearMessage();

                // 表示クリア
                this.txtGirenFile1.Text = string.Empty;
                // 変数
                if (!string.IsNullOrEmpty(this._dbGirenFile1))
                {
                    this._delGirenFile1 = this._dbGirenFile1;
                    this._dbGirenFile1 = string.Empty;
                }
                this._attachGirenFile1 = string.Empty;
                this._attachGirenFullPath1 = string.Empty;
                // ボタン切替
                this.ChangeEnableGirenDeleteButton(GirenType.No1, false);
                this.ChangeEnableGirenDownloadButton(GirenType.No1, false);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 削除ボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDelete2_Click(object sender, EventArgs e)
        {
            try
            {
                // メッセージクリア
                this.ClearMessage();

                // 表示クリア
                this.txtGirenFile2.Text = string.Empty;
                // 変数
                if (!string.IsNullOrEmpty(this._dbGirenFile2))
                {
                    this._delGirenFile2 = this._dbGirenFile2;
                    this._dbGirenFile2 = string.Empty;
                }
                this._attachGirenFile2 = string.Empty;
                this._attachGirenFullPath2 = string.Empty;
                // ボタン切替
                this.ChangeEnableGirenDeleteButton(GirenType.No2, false);
                this.ChangeEnableGirenDownloadButton(GirenType.No2, false);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 削除ボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDelete3_Click(object sender, EventArgs e)
        {
            try
            {
                // メッセージクリア
                this.ClearMessage();

                // 表示クリア
                this.txtGirenFile3.Text = string.Empty;
                // 変数
                if (!string.IsNullOrEmpty(this._dbGirenFile3))
                {
                    this._delGirenFile3 = this._dbGirenFile3;
                    this._dbGirenFile3 = string.Empty;
                }
                this._attachGirenFile3 = string.Empty;
                this._attachGirenFullPath3 = string.Empty;
                // ボタン切替
                this.ChangeEnableGirenDeleteButton(GirenType.No3, false);
                this.ChangeEnableGirenDownloadButton(GirenType.No3, false);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 削除ボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/06/18 添付ファイル対応</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDelete4_Click(object sender, EventArgs e)
        {
            try
            {
                // メッセージクリア
                this.ClearMessage();

                // 表示クリア
                this.txtGirenFile4.Text = string.Empty;
                // 変数
                if (!string.IsNullOrEmpty(this._dbGirenFile4))
                {
                    this._delGirenFile4 = this._dbGirenFile4;
                    this._dbGirenFile4 = string.Empty;
                }
                this._attachGirenFile4 = string.Empty;
                this._attachGirenFullPath4 = string.Empty;
                // ボタン切替
                this.ChangeEnableGirenDeleteButton(GirenType.No4, false);
                this.ChangeEnableGirenDownloadButton(GirenType.No4, false);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 削除ボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/06/18 添付ファイル対応</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDelete5_Click(object sender, EventArgs e)
        {
            try
            {
                // メッセージクリア
                this.ClearMessage();

                // 表示クリア
                this.txtGirenFile5.Text = string.Empty;
                // 変数
                if (!string.IsNullOrEmpty(this._dbGirenFile5))
                {
                    this._delGirenFile5 = this._dbGirenFile5;
                    this._dbGirenFile5 = string.Empty;
                }
                this._attachGirenFile5 = string.Empty;
                this._attachGirenFullPath5 = string.Empty;
                // ボタン切替
                this.ChangeEnableGirenDeleteButton(GirenType.No5, false);
                this.ChangeEnableGirenDownloadButton(GirenType.No5, false);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 削除ボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDeleteRef1_Click(object sender, EventArgs e)
        {
            try
            {
                // メッセージクリア
                this.ClearMessage();

                // 表示クリア
                this.txtRefFile1.Text = string.Empty;
                // 変数
                if (!string.IsNullOrEmpty(this._dbRefFile1))
                {
                    this._delRefFile1 = this._dbRefFile1;
                    this._dbRefFile1 = string.Empty;
                }
                this._attachRefFile1 = string.Empty;
                this._attachRefFullPath1 = string.Empty;
                // ボタン切替
                this.ChangeEnableRefDeleteButton(GirenType.RefNo1, false);
                this.ChangeEnableRefDownloadButton(GirenType.RefNo1, false);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 削除ボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDeleteRef2_Click(object sender, EventArgs e)
        {
            try
            {
                // メッセージクリア
                this.ClearMessage();

                // 表示クリア
                this.txtRefFile2.Text = string.Empty;
                // 変数
                if (!string.IsNullOrEmpty(this._dbRefFile2))
                {
                    this._delRefFile2 = this._dbRefFile2;
                    this._dbRefFile2 = string.Empty;
                }
                this._attachRefFile2 = string.Empty;
                this._attachRefFullPath2 = string.Empty;
                // ボタン切替
                this.ChangeEnableRefDeleteButton(GirenType.RefNo2, false);
                this.ChangeEnableRefDownloadButton(GirenType.RefNo2, false);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 添付ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 添付ボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnAttach1_Click(object sender, EventArgs e)
        {
            try
            {
                // メッセージクリア
                this.ClearMessage();

                string fileName = string.Empty;
                string fullPathName = string.Empty;
                if (this.GirenFileOpen(ref fileName, ref fullPathName))
                {
                    // 表示
                    this.txtGirenFile1.Text = fileName;
                    // 変数
                    this._attachGirenFile1 = fileName;
                    this._attachGirenFullPath1 = fullPathName;
                    if (!string.IsNullOrEmpty(this._dbGirenFile1))
                    {
                        this._delGirenFile1 = this._dbGirenFile1;
                        this._dbGirenFile1 = string.Empty;
                    }
                    // ボタン切替
                    this.ChangeEnableGirenDeleteButton(GirenType.No1, true);
                    this.ChangeEnableGirenDownloadButton(GirenType.No1, false);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 添付ボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnAttach2_Click(object sender, EventArgs e)
        {
            try
            {
                // メッセージクリア
                this.ClearMessage();

                string fileName = string.Empty;
                string fullPathName = string.Empty;
                if (this.GirenFileOpen(ref fileName, ref fullPathName))
                {
                    // 表示
                    this.txtGirenFile2.Text = fileName;
                    // 変数
                    this._attachGirenFile2 = fileName;
                    this._attachGirenFullPath2 = fullPathName;
                    if (!string.IsNullOrEmpty(this._dbGirenFile2))
                    {
                        this._delGirenFile2 = this._dbGirenFile2;
                        this._dbGirenFile2 = string.Empty;
                    }
                    // ボタン切替
                    this.ChangeEnableGirenDeleteButton(GirenType.No2, true);
                    this.ChangeEnableGirenDownloadButton(GirenType.No2, false);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 添付ボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnAttach3_Click(object sender, EventArgs e)
        {
            try
            {
                // メッセージクリア
                this.ClearMessage();

                string fileName = string.Empty;
                string fullPathName = string.Empty;
                if (this.GirenFileOpen(ref fileName, ref fullPathName))
                {
                    // 表示
                    this.txtGirenFile3.Text = fileName;
                    // 変数
                    this._attachGirenFile3 = fileName;
                    this._attachGirenFullPath3 = fullPathName;
                    if (!string.IsNullOrEmpty(this._dbGirenFile3))
                    {
                        this._delGirenFile3 = this._dbGirenFile3;
                        this._dbGirenFile3 = string.Empty;
                    }
                    // ボタン切替
                    this.ChangeEnableGirenDeleteButton(GirenType.No3, true);
                    this.ChangeEnableGirenDownloadButton(GirenType.No3, false);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 添付ボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/06/18 添付ファイル対応</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnAttach4_Click(object sender, EventArgs e)
        {
            try
            {
                // メッセージクリア
                this.ClearMessage();

                string fileName = string.Empty;
                string fullPathName = string.Empty;
                if (this.GirenFileOpen(ref fileName, ref fullPathName))
                {
                    // 表示
                    this.txtGirenFile4.Text = fileName;
                    // 変数
                    this._attachGirenFile4 = fileName;
                    this._attachGirenFullPath4 = fullPathName;
                    if (!string.IsNullOrEmpty(this._dbGirenFile4))
                    {
                        this._delGirenFile4 = this._dbGirenFile4;
                        this._dbGirenFile4 = string.Empty;
                    }
                    // ボタン切替
                    this.ChangeEnableGirenDeleteButton(GirenType.No4, true);
                    this.ChangeEnableGirenDownloadButton(GirenType.No4, false);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 添付ボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/06/18 添付ファイル対応</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnAttach5_Click(object sender, EventArgs e)
        {
            try
            {
                // メッセージクリア
                this.ClearMessage();

                string fileName = string.Empty;
                string fullPathName = string.Empty;
                if (this.GirenFileOpen(ref fileName, ref fullPathName))
                {
                    // 表示
                    this.txtGirenFile5.Text = fileName;
                    // 変数
                    this._attachGirenFile5 = fileName;
                    this._attachGirenFullPath5 = fullPathName;
                    if (!string.IsNullOrEmpty(this._dbGirenFile5))
                    {
                        this._delGirenFile5 = this._dbGirenFile5;
                        this._dbGirenFile5 = string.Empty;
                    }
                    // ボタン切替
                    this.ChangeEnableGirenDeleteButton(GirenType.No5, true);
                    this.ChangeEnableGirenDownloadButton(GirenType.No5, false);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 添付ボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnAttachRef1_Click(object sender, EventArgs e)
        {
            try
            {
                // メッセージクリア
                this.ClearMessage();

                string fileName = string.Empty;
                string fullPathName = string.Empty;
                if (this.RefFileOpen(ref fileName, ref fullPathName))
                {
                    // 表示
                    this.txtRefFile1.Text = fileName;
                    // 変数
                    this._attachRefFile1 = fileName;
                    this._attachRefFullPath1 = fullPathName;
                    if (!string.IsNullOrEmpty(this._dbRefFile1))
                    {
                        this._delRefFile1 = this._dbRefFile1;
                        this._dbRefFile1 = string.Empty;
                    }
                    // ボタン切替
                    this.ChangeEnableRefDeleteButton(GirenType.RefNo1, true);
                    this.ChangeEnableRefDownloadButton(GirenType.RefNo1, false);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 添付ボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnAttachRef2_Click(object sender, EventArgs e)
        {
            try
            {
                // メッセージクリア
                this.ClearMessage();

                string fileName = string.Empty;
                string fullPathName = string.Empty;
                if (this.RefFileOpen(ref fileName, ref fullPathName))
                {
                    // 表示
                    this.txtRefFile2.Text = fileName;
                    // 変数
                    this._attachRefFile2 = fileName;
                    this._attachRefFullPath2 = fullPathName;
                    if (!string.IsNullOrEmpty(this._dbRefFile2))
                    {
                        this._delRefFile2 = this._dbRefFile2;
                        this._dbRefFile2 = string.Empty;
                    }
                    // ボタン切替
                    this.ChangeEnableRefDeleteButton(GirenType.RefNo2, true);
                    this.ChangeEnableRefDownloadButton(GirenType.RefNo2, false);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region ＤＬボタン

        /// --------------------------------------------------
        /// <summary>
        /// ＤＬボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDownload1_Click(object sender, EventArgs e)
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                // メッセージクリア
                this.ClearMessage();

                string fileName = this._dbGirenFile1;
                if (string.IsNullOrEmpty(fileName))
                {
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                // ファイルダウンロード
                if (!this.GirenFileDownload(fileName, GirenType.No1, false))
                {
                    // 失敗
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                Cursor.Current = preCursor;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// ＤＬボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDownload2_Click(object sender, EventArgs e)
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                // メッセージクリア
                this.ClearMessage();

                string fileName = this._dbGirenFile2;
                if (string.IsNullOrEmpty(fileName))
                {
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                // ファイルダウンロード
                if (!GirenFileDownload(fileName, GirenType.No2, false))
                {
                    // 失敗
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                Cursor.Current = preCursor;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// ＤＬボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDownload3_Click(object sender, EventArgs e)
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                // メッセージクリア
                this.ClearMessage();

                string fileName = this._dbGirenFile3;
                if (string.IsNullOrEmpty(fileName))
                {
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                // ファイルダウンロード
                if (!GirenFileDownload(fileName, GirenType.No3, false))
                {
                    // 失敗
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                Cursor.Current = preCursor;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// ＤＬボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/06/18 添付ファイル対応</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDownload4_Click(object sender, EventArgs e)
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                // メッセージクリア
                this.ClearMessage();

                string fileName = this._dbGirenFile4;
                if (string.IsNullOrEmpty(fileName))
                {
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                // ファイルダウンロード
                if (!GirenFileDownload(fileName, GirenType.No4, false))
                {
                    // 失敗
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                Cursor.Current = preCursor;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// ＤＬボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/06/18 添付ファイル対応</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDownload5_Click(object sender, EventArgs e)
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                // メッセージクリア
                this.ClearMessage();

                string fileName = this._dbGirenFile5;
                if (string.IsNullOrEmpty(fileName))
                {
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                // ファイルダウンロード
                if (!GirenFileDownload(fileName, GirenType.No5, false))
                {
                    // 失敗
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                Cursor.Current = preCursor;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// ＤＬボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDownloadRef1_Click(object sender, EventArgs e)
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                // メッセージクリア
                this.ClearMessage();

                string fileName = this._dbRefFile1;
                if (string.IsNullOrEmpty(fileName))
                {
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                // ファイルダウンロード
                if (!this.RefFileDownload(fileName, GirenType.RefNo1, false))
                {
                    // 失敗
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                Cursor.Current = preCursor;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// ＤＬボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDownloadRef2_Click(object sender, EventArgs e)
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                // メッセージクリア
                this.ClearMessage();

                string fileName = this._dbRefFile2;
                if (string.IsNullOrEmpty(fileName))
                {
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                // ファイルダウンロード
                if (!RefFileDownload(fileName, GirenType.RefNo2, false))
                {
                    // 失敗
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                Cursor.Current = preCursor;
            }
        }

        #endregion

        #region 開くボタン

        /// --------------------------------------------------
        /// <summary>
        /// 開くボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2012/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnOpen1_Click(object sender, EventArgs e)
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                // メッセージクリア
                this.ClearMessage();

                string fileName = this._dbGirenFile1;
                if (string.IsNullOrEmpty(fileName))
                {
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                // ファイルダウンロード
                if (!this.GirenFileDownload(fileName, GirenType.No1, true))
                {
                    // 失敗
                    return;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                Cursor.Current = preCursor;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 開くボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2012/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnOpen2_Click(object sender, EventArgs e)
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                // メッセージクリア
                this.ClearMessage();

                string fileName = this._dbGirenFile2;
                if (string.IsNullOrEmpty(fileName))
                {
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                // ファイルダウンロード
                if (!GirenFileDownload(fileName, GirenType.No2, true))
                {
                    // 失敗
                    return;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                Cursor.Current = preCursor;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 開くボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2012/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnOpen3_Click(object sender, EventArgs e)
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                // メッセージクリア
                this.ClearMessage();

                string fileName = this._dbGirenFile3;
                if (string.IsNullOrEmpty(fileName))
                {
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                // ファイルダウンロード
                if (!GirenFileDownload(fileName, GirenType.No3, true))
                {
                    // 失敗
                    return;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                Cursor.Current = preCursor;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 開くボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/06/18 添付ファイル対応</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnOpen4_Click(object sender, EventArgs e)
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                // メッセージクリア
                this.ClearMessage();

                string fileName = this._dbGirenFile4;
                if (string.IsNullOrEmpty(fileName))
                {
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                // ファイルダウンロード
                if (!GirenFileDownload(fileName, GirenType.No4, true))
                {
                    // 失敗
                    return;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                Cursor.Current = preCursor;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 開くボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/06/18 添付ファイル対応</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnOpen5_Click(object sender, EventArgs e)
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                // メッセージクリア
                this.ClearMessage();

                string fileName = this._dbGirenFile5;
                if (string.IsNullOrEmpty(fileName))
                {
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                // ファイルダウンロード
                if (!GirenFileDownload(fileName, GirenType.No5, true))
                {
                    // 失敗
                    return;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                Cursor.Current = preCursor;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 開くボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnOpenRef1_Click(object sender, EventArgs e)
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                // メッセージクリア
                this.ClearMessage();

                string fileName = this._dbRefFile1;
                if (string.IsNullOrEmpty(fileName))
                {
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                // ファイルダウンロード
                if (!this.RefFileDownload(fileName, GirenType.RefNo1, true))
                {
                    // 失敗
                    return;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                Cursor.Current = preCursor;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 開くボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnOpenRef2_Click(object sender, EventArgs e)
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                // メッセージクリア
                this.ClearMessage();

                string fileName = this._dbRefFile2;
                if (string.IsNullOrEmpty(fileName))
                {
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                // ファイルダウンロード
                if (!RefFileDownload(fileName, GirenType.RefNo2, true))
                {
                    // 失敗
                    return;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                Cursor.Current = preCursor;
            }
        }

        #endregion

        #region Sheet

        #region ValueChanged

        /// --------------------------------------------------
        /// <summary>
        /// グリッドの値が変わったときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtARCost_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            try
            {
                int row = e.Position.Row;
                int col = e.Position.Column;
                switch (col)
                {
                    case SHEET_COL_WORK_TIME:
                    case SHEET_COL_WORKERS:
                    case SHEET_COL_NUMBER:
                    case SHEET_COL_RATE:
                        var workTime = DSWUtil.UtilConvert.ToDecimal(this.shtARCost[SHEET_COL_WORK_TIME, row].Text);
                        var workers = DSWUtil.UtilConvert.ToDecimal(this.shtARCost[SHEET_COL_WORKERS, row].Text);
                        var number = DSWUtil.UtilConvert.ToDecimal(this.shtARCost[SHEET_COL_NUMBER, row].Text);
                        var rate = DSWUtil.UtilConvert.ToDecimal(this.shtARCost[SHEET_COL_RATE, row].Text);
                        decimal total = workTime * workers * number * rate;
                        this.shtARCost[SHEET_COL_TOTAL, row].Text = total.ToString();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #region CellNotify

        /// --------------------------------------------------
        /// <summary>
        /// セルのイベントが発生したときに発生します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtARCost_CellNotify(object sender, CellNotifyEventArgs e)
        {
            try
            {
                int row = e.Position.Row;
                int col = e.Position.Column;
                if (col != SHEET_COL_ITEM)
                {
                    return;
                }
                switch (e.Name)
                {
                    case CellNotifyEvents.SelectedIndexChanged:
                        var value = this.shtARCost.ActiveCell.Value;
                        if (value == null || string.IsNullOrEmpty(value.ToString()))
                        {
                            this.shtARCost[SHEET_COL_WORK_TIME, row].Value = null;
                            this.shtARCost[SHEET_COL_WORKERS, row].Value = null;
                            this.shtARCost[SHEET_COL_NUMBER, row].Value = null;
                            this.shtARCost[SHEET_COL_RATE, row].Value = null;
                            this.shtARCost[SHEET_COL_TOTAL, row].Value = null;
                        }
                        break;
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

        #endregion

        #endregion

        #region リッチテキスト貼り付けエラー

        /// --------------------------------------------------
        /// <summary>
        /// 不具合リッチテキスト貼り付けエラー
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">イベント</param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void txtHuguai_Pasted(object sender, DSWRichTextBoxPasteEventArgs e)
        {
            NotifyPasteMessage(lblHuguai.Text, e);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 対策リッチテキスト貼り付けエラー
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">イベント</param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void txtTaisaku_Pasted(object sender, DSWRichTextBoxPasteEventArgs e)
        {
            NotifyPasteMessage(lblTaisaku.Text, e);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 備考リッチテキスト貼り付けエラー
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">イベント</param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void txtBiko_Pasted(object sender, DSWRichTextBoxPasteEventArgs e)
        {
            NotifyPasteMessage(lblBiko.Text, e);
        }
        /// --------------------------------------------------
        /// <summary>
        /// 貼り付けメッセージ
        /// </summary>
        /// <param name="controlName">名称</param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void NotifyPasteMessage(string controlName, DSWRichTextBoxPasteEventArgs e)
        {
            switch (e.Status)
            {
                case DSWRichTextBoxPasteEventStatus.ErrorInvalidInputReguration:
                    //ありえない
                    break;
                case DSWRichTextBoxPasteEventStatus.ErrorLengthOver:
                    // もともとの仕様で文字数オーバーはメッセージなどを出していないので踏襲してメッセージなどは出さない
                    break;
                case DSWRichTextBoxPasteEventStatus.ErrorNoData:
                    // 貼り付け可能な画像又はTextがありません。
                    this.ShowMessage("A9999999069", controlName);
                    break;
                case DSWRichTextBoxPasteEventStatus.ErrorNoEnoughMemory:
                    // 貼り付け操作中にMemory不足が発生しました。Applicationを再起動してください。
                    this.ShowMessage("A9999999072", controlName);
                    break;
                case DSWRichTextBoxPasteEventStatus.Error:
                    // 貼り付け操作中に予期せぬエラーが発生しました。再試行してください。
                    this.ShowMessage("A9999999071", controlName);
                    break;
                case DSWRichTextBoxPasteEventStatus.ErrorSizeOver:
                    // 添付するFileSizeが大きすぎます。
                    this.ShowMessage("A9999999074", controlName);
                    break;
                case DSWRichTextBoxPasteEventStatus.Success:
                    this.ClearMessage();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region ラベル参照ボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// 不具合参照クリック
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">イベント</param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnHuguai_Click(object sender, EventArgs e)
        {
            DetailWindowShow(this.lblHuguai.Text, 0, this.txtHuguai);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 対策内容参照ボタンクリック
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">イベント</param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnTaisaku_Click(object sender, EventArgs e)
        {
            DetailWindowShow(this.lblTaisaku.Text, 1, this.txtTaisaku);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 備考参照ボタンクリック
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">イベント</param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnBiko_Click(object sender, EventArgs e)
        {
            DetailWindowShow(this.lblBiko.Text, 2, this.txtBiko);
        }
        /// --------------------------------------------------
        /// <summary>
        /// AR情報詳細画面を表示する処理
        /// </summary>
        /// <param name="label"></param>
        /// <param name="form"></param>
        /// <param name="target"></param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void DetailWindowShow(string labelName, int index, DSWRichTextBox target)
        {
            if (_detailForms[index] != null)
            {
                _detailForms[index].Activate();
                return;
            }
            string title = labelName;
            if (this.EditMode != SystemBase.EditMode.View)
            {
                target.ReadOnly = true;
            }
            var path = Path.GetTempFileName();
            target.SaveFile(path, RichTextBoxStreamType.RichText);
            _detailForms[index] = new ARJohoMeisaiNote(this.UserInfo, this.EditMode, title, this.Condition, this._listFlagName, path, target.MaxLength);
            _detailForms[index].Icon = this.Icon.Clone() as Icon;
            // フォームが閉じられたときのイベントを設定（ラムダ式）
            _detailForms[index].FormClosed += (sender, e) => this.DetailWindowClosed(sender as ARJohoMeisaiNote, target, index);
            // フォームを表示
            _detailForms[index].Show();
        }
        /// --------------------------------------------------
        /// <summary>
        /// AR情報詳細画面が閉じられたときに実行
        /// </summary>
        /// <param name="form"></param>
        /// <param name="target"></param>
        /// <param name="index"></param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void DetailWindowClosed(ARJohoMeisaiNote form, DSWRichTextBox target, int index)
        {
            // すでにクローズ済みの場合は処理しない
            if (_detailForms[index] == null)
                return;
            if (this.EditMode != SystemBase.EditMode.View)
            {
                target.ReadOnly = false;
                if (form.FilePath != null)
                {
                    var path = form.FilePath;
                    try
                    {
                        target.LoadFile(path);
                    }
                    catch
                    {
                        // 貼り付け操作中に予期せぬErrorが発生しました。再試行してください。
                        this.ShowMessage("A9999999071");
                    }
                    try
                    {
                        File.Delete(path);
                    }
                    catch { }
                }
            }
            //上位メソッドから参照される可能性があるため、Disposeしない
            //Application.DoEvents();
            //_detailForms[index].Dispose();
            _detailForms[index] = null;
        }
        #endregion

        #region 号機一覧ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 号機一覧ボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnGokiList_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            // 機種が未選択の場合はエラー
            if (this.cboKishu.SelectedIndex < 0)
            {
                // 機種が選択されていません。
                this.ShowMessage("A0100020018");
                return;
            }
            using (var form = new ARJohoMeisaiGokiIchiran(
                                    this.UserInfo, 
                                    ComDefine.TITLE_A0100022, 
                                    this.cboKishu.Text, 
                                    this.cboGoki.Tag as string[], 
                                    this._nonyusakiCd, 
                                    this._gokiShinchokuDateList.ToArray(), 
                                    this.EditMode))
            {
                form.Icon = this.Icon;
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    this.cboGoki_SetGokiList(form.SelectedGoki);
                }
            }
        }

        #endregion

        #region 進捗管理利用コンボボックス

        /// --------------------------------------------------
        /// <summary>
        /// 進捗管理利用コンボボックス変化イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/08/01</create>
        /// <update>M.Shimizu 2020/04/07 AR進捗・関連付け対応</update>
        /// --------------------------------------------------
        private void cboShinchokuRiyouFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 進捗管理なしのとき、何もしない
            if (this._gokiNum < 1)
                return;
            // 未選択のとき何もしない
            if (this.cboShinchokuRiyouFlag.SelectedIndex == -1)
                return;

            this.ClearMessage();
            // する ⇒ しない
            if ((string.Equals(this.cboShinchokuRiyouFlag.SelectedValue, AR_SHINCHOKU_FLAG.OFF_VALUE1))
             && (this._gokiShinchokuDateList.Count > 0))
            {
                // 進捗管理に日付が設定されています。解除してもよいですか？
                if (this.ShowMessage("A0100020021") != DialogResult.Yes)
                {
                    this.cboShinchokuRiyouFlag.SelectedIndexChanged -= this.cboShinchokuRiyouFlag_SelectedIndexChanged;
                    this.cboShinchokuRiyouFlag.SelectedValue = AR_SHINCHOKU_FLAG.ON_VALUE1;
                    this.cboShinchokuRiyouFlag.SelectedIndexChanged += this.cboShinchokuRiyouFlag_SelectedIndexChanged;
                    return;
                }
                else
                {
                    // クリア
                    this.cboGoki.Tag = new string[0];
                    this._gokiShinchokuDateList.Clear();
                }
            }
            this.ChangeShinshokuRiyou();
        }

        #endregion


        #region 結果AR参照ボタン
        /// --------------------------------------------------
        /// <summary>
        /// 結果AR参照ボタンクリックイベント
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">イベント</param>
        /// <create>D.Okumura 2019/11/25</create>
        /// <update></update>
        /// <remarks>
        /// ARJoho画面の機能を用いて画面を開く。
        /// そのため、本画面では子画面の管理は行わない。
        /// </remarks>
        /// --------------------------------------------------
        private void btnKekkaArRef_Click(object sender, EventArgs e)
        {
            string kekkaArNo = txtKekkaArNo.Text;
            if (string.IsNullOrEmpty(kekkaArNo))
                return;
            // 画面を開く
            CondA1 cond = new CondA1(this.UserInfo);
            cond.NonyusakiCD = this._nonyusakiCd;
            cond.NonyusakiName = this._nonyusakiName;
            cond.BukkenNo = this._bukkenNo;
            cond.ListFlagName0 = this._listFlagName0;
            cond.ListFlagName1 = this._listFlagName1;
            cond.ListFlagName2 = this._listFlagName2;
            cond.ListFlagName3 = this._listFlagName3;
            cond.ListFlagName4 = this._listFlagName4;
            cond.ListFlagName5 = this._listFlagName5;
            cond.ListFlagName6 = this._listFlagName6;
            cond.ListFlagName7 = this._listFlagName7;

            cond.ListFlag = kekkaArNo[0].ToString();
            cond.ArNo = "AR" + kekkaArNo;
            ARJoho.ShowARJohoMeisai(SystemBase.EditMode.View, cond);
        }
        #endregion
        
        #endregion //イベント

        #region モード切替操作

        /// --------------------------------------------------
        /// <summary>
        /// モード切替操作
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeMode()
        {
            try
            {
                if (this.EditMode == SystemBase.EditMode.Insert)
                {
                    // 登録
                    this.ChangeEnableViewMode(true);
                    this.cboJyokyoFlag.SelectedValue = JYOKYO_FLAG.MISHORI_VALUE1;
                    this.fbrFunction.F11Button.Enabled = false;
                }
                else if (this.EditMode == SystemBase.EditMode.Update)
                {
                    // 更新
                    this.ChangeEnableViewMode(true);
                }
                else if (this.EditMode == SystemBase.EditMode.View)
                {
                    // 照会
                    this.ChangeEnableViewMode(false);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// コントロールのEnabled切替
        /// </summary>
        /// <param name="isView">Enabled状態</param>
        /// <create>M.Tsutsumi 2010/08/19</create>
        /// <update>H.Tajimi 2018/10/15 FE要望対応</update>
        /// <update>D.Okumura 2019/06/18 添付ファイル対応</update>
        /// <update>D.Okumura 2019/08/05 AR進捗対応</update>
        /// <update>T.Nukaga 2019/11/25 AR7000番運用対応</update>
        /// <update>D.Okumura 2020/01/28 日付入力制御を変更</update>
        /// <update>M.Shimizu 2020/04/07 AR進捗・関連付け対応</update>
        /// --------------------------------------------------
        private void ChangeEnableViewMode(bool isView)
        {
            // 入力コントロールの切替
            this.cboJyokyoFlag.Enabled = isView;
            this.dtpHasseiDate.Enabled = isView;
            this.txtRenrakusha.ReadOnly = !isView;
            this.cboShinchokuRiyouFlag.Enabled = isView && this._gokiNum != 0;
            this.cboKishu.Enabled = isView;
            this.cboGoki.Enabled = isView && !CanUseInzuhyo();
            this.dtpGenbaTotyakukibouDate.Enabled = isView;
            this.cboHasseiFactor.Enabled = isView;
            this.txtHuguai.ReadOnly = !isView;
            this.txtHuguai.TabStop = isView;
            this.txtTaisaku.ReadOnly = !isView;
            this.txtTaisaku.TabStop = isView;
            this.txtGenchiTehaisaki.ReadOnly = !isView;
            this.dtpGenchiSetteinokiDate.Enabled = isView;
            this.dtpGenchiShukkayoteiDate.Enabled = IsEnableShukkayoteiDate();
            this.dtpGenchiKojyoshukkaDate.Enabled = isView;
            this.txtShukkahoho.ReadOnly = !isView;
            this.dtpJpSetteinokiDate.Enabled = isView;
            this.dtpJpShukkayoteiDate.Enabled = IsEnableShukkayoteiDate();
            this.dtpJpKojyoshukkaDate.Enabled = isView;
            this.txtJpUnsokaishaName.ReadOnly = !isView;
            this.txtJpOkurijyoNo.ReadOnly = !isView;
            this.txtTaioBusho.ReadOnly = !isView;
            this.txtGmsHakkoNo.ReadOnly = !isView;
            this.txtShiyorenrakuNo.ReadOnly = !isView;
            this.txtShukkaJokyo.ReadOnly = !isView;
            this.txtGirenNo1.ReadOnly = !isView;
            this.txtGirenNo1.TabStop = isView;
            this.btnAttach1.Enabled = isView;
            this.txtGirenNo2.ReadOnly = !isView;
            this.txtGirenNo2.TabStop = isView;
            this.btnAttach2.Enabled = isView;
            this.txtGirenNo3.ReadOnly = !isView;
            this.txtGirenNo3.TabStop = isView;
            this.btnAttach3.Enabled = isView;
            this.txtGirenNo4.ReadOnly = !isView;
            this.txtGirenNo4.TabStop = isView;
            this.btnAttach4.Enabled = isView;
            this.txtGirenNo5.ReadOnly = !isView;
            this.txtGirenNo5.TabStop = isView;
            this.btnAttach5.Enabled = isView;
            this.txtRefNo1.ReadOnly = !isView;
            this.txtRefNo1.TabStop = isView;
            this.btnAttachRef1.Enabled = isView;
            this.txtRefNo2.ReadOnly = !isView;
            this.txtRefNo2.TabStop = isView;
            this.btnAttachRef2.Enabled = isView;
            //this.shtARCost.Enabled = isView;
            if (isView)
            {
                this.shtARCost.EditType = EditType.AlwaysEdit;
            }
            else
            {
                this.shtARCost.EditType = EditType.ReadOnly;
            }
            this.txtBiko.ReadOnly = !isView;
            this.txtBiko.TabStop = isView;
            this.txtMotoArNo.ReadOnly = !isView;
            this.txtMotoArNo.TabStop = isView;
            // 技連ファイルは常にReadOnly = true
            //this.txtGirenFile1.ReadOnly = !isView;
            //this.txtGirenFile2.ReadOnly = !isView;
            //this.txtGirenFile3.ReadOnly = !isView;
            // 削除・DLのボタンは、それぞれの処理で設定する。
            //this.btnDelete1.Enabled = isView;
            //this.btnDownload1.Enabled = isView;
            //this.btnDelete2.Enabled = isView;
            //this.btnDownload2.Enabled = isView;
            //this.btnDelete3.Enabled = isView;
            //this.btnDownload3.Enabled = isView;

            // 保存ボタン
            this.fbrFunction.F01Button.Enabled = isView;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 号機へ号機リストを設定
        /// </summary>
        /// <param name="array">設定する配列</param>
        /// <create>D.Okumura 2019/08/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboGoki_SetGokiList(string[] array)
        {
            this.cboGoki.DataSource = null;
            this.cboGoki.Tag = array;
            this.cboGoki.Items.Clear();
            this.cboGoki.Items.Add(ComFunc.GokiArrayToString(array, this._separator, this._separatorRange) ?? string.Empty);
            this.cboGoki.SelectedIndex = 0;
            this.cboGoki.Enabled = false;
        }
        /// --------------------------------------------------
        /// <summary>
        /// 削除ボタンのEnabled切替
        /// </summary>
        /// <param name="girenType"></param>
        /// <param name="isView"></param>
        /// <create>M.Tsutsumi 2010/09/13</create>
        /// <update>D.Okumura 2019/06/18 添付ファイル対応</update>
        /// --------------------------------------------------
        private void ChangeEnableGirenDeleteButton(GirenType girenType, bool isView)
        {
            switch (girenType)
            {
                case GirenType.No1:
                    this.btnDelete1.Enabled = isView;
                    break;
                case GirenType.No2:
                    this.btnDelete2.Enabled = isView;
                    break;
                case GirenType.No3:
                    this.btnDelete3.Enabled = isView;
                    break;
                case GirenType.No4:
                    this.btnDelete4.Enabled = isView;
                    break;
                case GirenType.No5:
                    this.btnDelete5.Enabled = isView;
                    break;
                default:
                    break;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// DLボタンのEnabled切替
        /// </summary>
        /// <param name="girenType"></param>
        /// <param name="isView"></param>
        /// <create>M.Tsutsumi 2010/09/13</create>
        /// <update>D.Okumura 2019/06/18 添付ファイル対応</update>
        /// --------------------------------------------------
        private void ChangeEnableGirenDownloadButton(GirenType girenType, bool isView)
        {
            switch (girenType)
            {
                case GirenType.No1:
                    this.btnDownload1.Enabled = isView;
                    this.btnOpen1.Enabled = isView;
                    break;
                case GirenType.No2:
                    this.btnDownload2.Enabled = isView;
                    this.btnOpen2.Enabled = isView;
                    break;
                case GirenType.No3:
                    this.btnDownload3.Enabled = isView;
                    this.btnOpen3.Enabled = isView;
                    break;
                case GirenType.No4:
                    this.btnDownload4.Enabled = isView;
                    this.btnOpen4.Enabled = isView;
                    break;
                case GirenType.No5:
                    this.btnDownload5.Enabled = isView;
                    this.btnOpen5.Enabled = isView;
                    break;
                default:
                    break;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 削除ボタンのEnabled切替
        /// </summary>
        /// <param name="refType"></param>
        /// <param name="isView"></param>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeEnableRefDeleteButton(GirenType refType, bool isView)
        {
            switch (refType)
            {
                case GirenType.RefNo1:
                    this.btnDeleteRef1.Enabled = isView;
                    break;
                case GirenType.RefNo2:
                    this.btnDeleteRef2.Enabled = isView;
                    break;
                default:
                    break;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// DLボタンのEnabled切替
        /// </summary>
        /// <param name="refType"></param>
        /// <param name="isView"></param>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeEnableRefDownloadButton(GirenType refType, bool isView)
        {
            switch (refType)
            {
                case GirenType.RefNo1:
                    this.btnDownloadRef1.Enabled = isView;
                    this.btnOpenRef1.Enabled = isView;
                    break;
                case GirenType.RefNo2:
                    this.btnDownloadRef2.Enabled = isView;
                    this.btnOpenRef2.Enabled = isView;
                    break;
                default:
                    break;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 進捗管理利用変更処理
        /// </summary>
        /// <create>Y.Nakasato 2019/08/01</create>
        /// <update>D.Okumura 2020/01/28 日付入力制御を変更</update>
        /// <update>M.Shimizu 2020/04/07 AR進捗・関連付け対応</update>
        /// --------------------------------------------------
        private void ChangeShinshokuRiyou()
        {
            // 員数表を使用する
            if (CanUseInzuhyo())
            {
                // 機種コンボボックス設定
                this.cboKishu.DataSource = null;
                this.cboKishu.Items.Clear();
                this.cboKishu.Items.AddRange(this._kishuList.ToArray());
                this.cboKishu.DropDownStyle = ComboBoxStyle.DropDownList;

                // 号機コンボボックス設定
                this.cboGoki.DataSource = null;
                this.cboGoki.Items.Clear();
                if (this.cboGoki.Tag != null)
                {
                    this.cboGoki.Items.Add(ComFunc.GokiArrayToString(this.cboGoki.Tag as string[], this._separator, this._separatorRange));
                    this.cboGoki.SelectedIndex = 0;
                }
                this.cboGoki.DropDownStyle = ComboBoxStyle.DropDown;
                this.cboGoki.Text = string.Empty;
                this.cboGoki.Enabled = false;

                // 号機一覧ボタン有効化
                this.btnGokiList.Enabled = true;
            }
            // 員数表を使用しない
            else
            {
                var cond = new CondA1(this.UserInfo);
                var conn = new ConnA01();

                // 機種コンボボックス設定
                cond.SelectGroupCode = SELECT_GROUP_CD.KISHU_VALUE1;
                this.cboKishu.DataSource = null;
                this.cboKishu.Items.Clear();
                this.cboKishu.ValueMember = Def_M_SELECT_ITEM.SELECT_GROUP_CD;
                this.cboKishu.DisplayMember = Def_M_SELECT_ITEM.ITEM_NAME;
                this.cboKishu.DataSource = conn.GetSelectItem(cond);
                this.cboKishu.DropDownStyle = ComboBoxStyle.DropDown;

                // 号機コンボボックス設定
                this.cboGoki.DataSource = null;
                this.cboGoki.Items.Clear();
                cond.SelectGroupCode = SELECT_GROUP_CD.GOKI_VALUE1;
                this.cboGoki.ValueMember = Def_M_SELECT_ITEM.SELECT_GROUP_CD;
                this.cboGoki.DisplayMember = Def_M_SELECT_ITEM.ITEM_NAME;
                this.cboGoki.DataSource = conn.GetSelectItem(cond);
                this.cboGoki.DropDownStyle = ComboBoxStyle.DropDown;
                this.cboGoki.Enabled = true;

                // 号機一覧ボタン有効化
                this.btnGokiList.Enabled = false;
            }

            // 「現地.出荷予定日」「現地.出荷予定日」の制御
            bool isEnable = IsEnableShukkayoteiDate();
            this.dtpGenchiShukkayoteiDate.Enabled = isEnable;
            this.dtpJpShukkayoteiDate.Enabled = isEnable;

            this.cboKishu.SelectedIndex = -1;
            this.cboGoki.SelectedIndex = -1;
        }
        #endregion

        #region コンディション取得

        /// --------------------------------------------------
        /// <summary>
        /// コンディションの取得
        /// </summary>
        /// <returns>コンディション</returns>
        /// <create>M.Tsutsumi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondA1 GetCondition()
        {
            CondA1 cond = new CondA1(this.UserInfo);

            cond.NonyusakiCD = this._nonyusakiCd;
            cond.NonyusakiName = this._nonyusakiName;
            cond.ListFlag = this._listFlag;
            cond.ArNo = this._arNo;

            //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36
            // リスト区分名称
            cond.ListFlagName0 = this._listFlagName0;
            cond.ListFlagName1 = this._listFlagName1;
            cond.ListFlagName2 = this._listFlagName2;
            cond.ListFlagName3 = this._listFlagName3;
            cond.ListFlagName4 = this._listFlagName4;
            cond.ListFlagName5 = this._listFlagName5;
            cond.ListFlagName6 = this._listFlagName6;
            cond.ListFlagName7 = this._listFlagName7;
            // @@@ ↑

            // 出荷フラグ = 1:AR
            cond.ShukkaFlag = SHUKKA_FLAG.AR_VALUE1;
            cond.BukkenNo = this._bukkenNo;
            if (string.IsNullOrEmpty(cond.BukkenNo))
            {
                cond.BukkenNo = null;
            }

            return cond;
        }

        #endregion

        #region 登録用のデータテーブル

        /// --------------------------------------------------
        /// <summary>
        /// AR情報データの登録用スキーマー
        /// </summary>
        /// <param name="tblName">テーブル名</param>
        /// <returns></returns>
        /// <create>M.Tsutsumi 2010/08/25</create>
        /// <update>K.Tsutsumi 2015/09/07 Change 納入先コード３６進化</update>
        /// <update>H.Tajimi 2018/10/15 FE要望対応</update>
        /// <update>D.Okumura 2019/06/18 添付ファイル対応</update>
        /// <update>T.Nukaga 2019/11/21 STEP12 AR7000番運用対応</update>
        /// <update>N.Kawamura 2022/10/06 出荷状況自動反映機能追加</update>
        /// --------------------------------------------------
        private DataTable GetSchemaAR(string tblName)
        {
            DataTable dt = new DataTable(tblName);

            dt.Columns.Add(Def_T_AR.NONYUSAKI_CD, typeof(string));
            dt.Columns.Add(Def_T_AR.LIST_FLAG, typeof(string));
            dt.Columns.Add(Def_T_AR.AR_NO, typeof(string));
            dt.Columns.Add(Def_T_AR.JYOKYO_FLAG, typeof(string));
            dt.Columns.Add(Def_T_AR.HASSEI_DATE, typeof(string));
            dt.Columns.Add(Def_T_AR.RENRAKUSHA, typeof(string));
            dt.Columns.Add(Def_T_AR.KISHU, typeof(string));
            dt.Columns.Add(Def_T_AR.GOKI, typeof(string));
            dt.Columns.Add(Def_T_AR.GENBA_TOTYAKUKIBOU_DATE, typeof(string));
            dt.Columns.Add(Def_T_AR.HUGUAI, typeof(string));
            dt.Columns.Add(Def_T_AR.TAISAKU, typeof(string));
            dt.Columns.Add(Def_T_AR.BIKO, typeof(string));
            dt.Columns.Add(Def_T_AR.GENCHI_TEHAISAKI, typeof(string));
            dt.Columns.Add(Def_T_AR.GENCHI_SETTEINOKI_DATE, typeof(string));
            dt.Columns.Add(Def_T_AR.GENCHI_SHUKKAYOTEI_DATE, typeof(string));
            dt.Columns.Add(Def_T_AR.GENCHI_KOJYOSHUKKA_DATE, typeof(string));
            dt.Columns.Add(Def_T_AR.SHUKKAHOHO, typeof(string));
            dt.Columns.Add(Def_T_AR.JP_SETTEINOKI_DATE, typeof(string));
            dt.Columns.Add(Def_T_AR.JP_SHUKKAYOTEI_DATE, typeof(string));
            dt.Columns.Add(Def_T_AR.JP_KOJYOSHUKKA_DATE, typeof(string));
            dt.Columns.Add(Def_T_AR.JP_UNSOKAISHA_NAME, typeof(string));
            dt.Columns.Add(Def_T_AR.JP_OKURIJYO_NO, typeof(string));
            dt.Columns.Add(Def_T_AR.GMS_HAKKO_NO, typeof(string));
            dt.Columns.Add(Def_T_AR.SHIYORENRAKU_NO, typeof(string));
            dt.Columns.Add(Def_T_AR.TAIO_BUSHO, typeof(string));
            dt.Columns.Add(Def_T_AR.GIREN_NO_1, typeof(string));
            dt.Columns.Add(Def_T_AR.GIREN_FILE_1, typeof(string));
            dt.Columns.Add(Def_T_AR.GIREN_NO_2, typeof(string));
            dt.Columns.Add(Def_T_AR.GIREN_FILE_2, typeof(string));
            dt.Columns.Add(Def_T_AR.GIREN_NO_3, typeof(string));
            dt.Columns.Add(Def_T_AR.GIREN_FILE_3, typeof(string));
            dt.Columns.Add(Def_T_AR.HASSEI_YOUIN, typeof(string));
            dt.Columns.Add(Def_T_AR.REFERENCE_NO_1, typeof(string));
            dt.Columns.Add(Def_T_AR.REFERENCE_FILE_1, typeof(string));
            dt.Columns.Add(Def_T_AR.REFERENCE_NO_2, typeof(string));
            dt.Columns.Add(Def_T_AR.REFERENCE_FILE_2, typeof(string));
            dt.Columns.Add(Def_T_AR.SHUKKA_DATE, typeof(string));
            dt.Columns.Add(Def_T_AR.SHUKKA_USER_ID, typeof(string));
            dt.Columns.Add(Def_T_AR.SHUKKA_USER_NAME, typeof(string));
            dt.Columns.Add(Def_T_AR.UKEIRE_DATE, typeof(string));
            dt.Columns.Add(Def_T_AR.UKEIRE_USER_ID, typeof(string));
            dt.Columns.Add(Def_T_AR.UKEIRE_USER_NAME, typeof(string));
            dt.Columns.Add(Def_T_AR.LOCK_USER_ID, typeof(string));
            dt.Columns.Add(Def_T_AR.LOCK_STARTDATE, typeof(string));
            dt.Columns.Add(Def_T_AR.CREATE_DATE, typeof(string));
            dt.Columns.Add(Def_T_AR.CREATE_USER_ID, typeof(string));
            dt.Columns.Add(Def_T_AR.CREATE_USER_NAME, typeof(string));
            dt.Columns.Add(Def_T_AR.UPDATE_DATE, typeof(string));
            dt.Columns.Add(Def_T_AR.UPDATE_USER_ID, typeof(string));
            dt.Columns.Add(Def_T_AR.UPDATE_USER_NAME, typeof(string));
            dt.Columns.Add(Def_T_AR.VERSION, typeof(string));
            dt.Columns.Add(Def_M_NONYUSAKI.BUKKEN_NO, typeof(string));
            dt.Columns.Add(Def_T_AR.GIREN_NO_4, typeof(string));
            dt.Columns.Add(Def_T_AR.GIREN_FILE_4, typeof(string));
            dt.Columns.Add(Def_T_AR.GIREN_NO_5, typeof(string));
            dt.Columns.Add(Def_T_AR.GIREN_FILE_5, typeof(string));
            dt.Columns.Add(Def_T_AR.SHINCHOKU_FLAG, typeof(string));
            dt.Columns.Add(Def_T_AR.MOTO_AR_NO, typeof(string));
            dt.Columns.Add(Def_T_AR.GENCHI_SHUKKAJYOKYO_FLAG, typeof(string));

            return dt;
        }
        
        /// --------------------------------------------------
        /// <summary>
        /// AR進捗情報データの登録用スキーマー
        /// </summary>
        /// <param name="tblName">テーブル名</param>
        /// <returns></returns>
        /// <create>D.Okumura 2019/08/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSchemaARShinchoku(string tblName)
        {
            var dt = new DataTable(tblName);

            dt.Columns.Add(Def_T_AR_SHINCHOKU.NONYUSAKI_CD, typeof(string));
            dt.Columns.Add(Def_T_AR_SHINCHOKU.LIST_FLAG, typeof(string));
            dt.Columns.Add(Def_T_AR_SHINCHOKU.AR_NO, typeof(string));
            dt.Columns.Add(Def_T_AR_SHINCHOKU.GOKI, typeof(string));
            dt.Columns.Add(Def_T_AR_SHINCHOKU.DATE_SITE_REQ, typeof(string));
            dt.Columns.Add(Def_T_AR_SHINCHOKU.DATE_LOCAL, typeof(string));
            dt.Columns.Add(Def_T_AR_SHINCHOKU.DATE_JP, typeof(string));
            dt.Columns.Add(Def_T_AR_SHINCHOKU.CREATE_DATE, typeof(string));
            dt.Columns.Add(Def_T_AR_SHINCHOKU.CREATE_USER_ID, typeof(string));
            dt.Columns.Add(Def_T_AR_SHINCHOKU.CREATE_USER_NAME, typeof(string));
            dt.Columns.Add(Def_T_AR_SHINCHOKU.UPDATE_DATE, typeof(string));
            dt.Columns.Add(Def_T_AR_SHINCHOKU.UPDATE_USER_ID, typeof(string));
            dt.Columns.Add(Def_T_AR_SHINCHOKU.UPDATE_USER_NAME, typeof(string));
            dt.Columns.Add(Def_T_AR_SHINCHOKU.VERSION, typeof(string));
            return dt;
        }

        /// --------------------------------------------------
        /// <summary>
        /// AR対応費用情報データの登録用スキーマー
        /// </summary>
        /// <param name="tblName">テーブル名</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSchemaARCost(string tblName)
        {
            var dt = new DataTable(tblName);

            dt.Columns.Add(Def_T_AR_COST.NONYUSAKI_CD, typeof(string));
            dt.Columns.Add(Def_T_AR_COST.LIST_FLAG, typeof(string));
            dt.Columns.Add(Def_T_AR_COST.AR_NO, typeof(string));
            dt.Columns.Add(Def_T_AR_COST.LINE_NO, typeof(string));
            dt.Columns.Add(Def_T_AR_COST.ITEM_CD, typeof(string));
            dt.Columns.Add(Def_T_AR_COST.WORK_TIME, typeof(string));
            dt.Columns.Add(Def_T_AR_COST.WORKERS, typeof(string));
            dt.Columns.Add(Def_T_AR_COST.NUMBER, typeof(string));
            dt.Columns.Add(Def_T_AR_COST.RATE, typeof(string));
            dt.Columns.Add(Def_T_AR_COST.TOTAL, typeof(string));
            dt.Columns.Add(Def_T_AR_COST.CREATE_DATE, typeof(string));
            dt.Columns.Add(Def_T_AR_COST.CREATE_USER_ID, typeof(string));
            dt.Columns.Add(Def_T_AR_COST.CREATE_USER_NAME, typeof(string));

            return dt;
        }

        /// --------------------------------------------------
        /// <summary>
        /// AR添付ファイルデータの登録用スキーマー
        /// </summary>
        /// <param name="tblName">テーブル名</param>
        /// <returns></returns>
        /// <create>D.Okumura 2019/06/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSchemaARFile(string tblName)
        {
            var dt = new DataTable(tblName);

            dt.Columns.Add(Def_T_AR_FILE.NONYUSAKI_CD, typeof(string));
            dt.Columns.Add(Def_T_AR_FILE.LIST_FLAG, typeof(string));
            dt.Columns.Add(Def_T_AR_FILE.AR_NO, typeof(string));
            dt.Columns.Add(Def_T_AR_FILE.FILE_KIND, typeof(string));
            dt.Columns.Add(Def_T_AR_FILE.POSITION, typeof(string));
            dt.Columns.Add(Def_T_AR_FILE.FILE_NAME, typeof(string));
            dt.Columns.Add(Def_T_AR_FILE.CREATE_DATE, typeof(string));
            dt.Columns.Add(Def_T_AR_FILE.CREATE_USER_ID, typeof(string));
            dt.Columns.Add(Def_T_AR_FILE.CREATE_USER_NAME, typeof(string));

            return dt;
        }

        #endregion

        #region 画面入力データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 画面入力データ取得
        /// </summary>
        /// <param name="tblName">テーブル名</param>
        /// <returns></returns>
        /// <create>M.Tsutsumi 2010/08/25</create>
        /// <update>H.Tajimi 2018/10/15 FE要望対応</update>
        /// <update>D.Okumura 2019/06/18 添付ファイル対応</update>
        /// <update>Y.Nakasato 2019/07/22 進捗対応</update>
        /// <update>T.Nukaga 2019/11/21 STEP12 AR7000番運用対応</update>
        /// <update>N.Kawamura 2022/10/06 出荷状況自動反映機能追加</update>
        /// --------------------------------------------------
        private DataTable GetDisplay(string tblName)
        {
            DataTable dt = GetSchemaAR(tblName);

            DataRow dr = dt.NewRow();

            dr[Def_T_AR.NONYUSAKI_CD] = this._nonyusakiCd;
            dr[Def_T_AR.LIST_FLAG] = this._listFlag;
            dr[Def_T_AR.AR_NO] = this._arNo;
            dr[Def_T_AR.JYOKYO_FLAG] = this.cboJyokyoFlag.SelectedValue;
            dr[Def_T_AR.HASSEI_DATE] = this.dtpHasseiDate.Text;
            if (!string.IsNullOrEmpty(this.txtRenrakusha.Text))
            {
                dr[Def_T_AR.RENRAKUSHA] = this.txtRenrakusha.Text;
            }
            if (!string.IsNullOrEmpty(this.cboShinchokuRiyouFlag.Text))
            {
                dr[Def_T_AR.SHINCHOKU_FLAG] = this.cboShinchokuRiyouFlag.SelectedValue;
            }
            if (!string.IsNullOrEmpty(this.cboKishu.Text))
            {
                dr[Def_T_AR.KISHU] = this.cboKishu.Text;
            }
            if (!string.IsNullOrEmpty(this.cboGoki.Text))
            {
                dr[Def_T_AR.GOKI] = UtilString.SubstringForByte(this.cboGoki.Text, 0, ComDefine.MAX_BYTE_LENGTH_GOKI);
            }
            if (this.dtpGenbaTotyakukibouDate.Value != null)
            {
                dr[Def_T_AR.GENBA_TOTYAKUKIBOU_DATE] = this.dtpGenbaTotyakukibouDate.Text;
            }
            if (!string.IsNullOrEmpty(this.cboHasseiFactor.Text))
            {
                dr[Def_T_AR.HASSEI_YOUIN] = this.cboHasseiFactor.Text;
            }
            if (!string.IsNullOrEmpty(this.txtHuguai.Text))
            {
                dr[Def_T_AR.HUGUAI] = this.txtHuguai.Text;
            }
            if (!string.IsNullOrEmpty(this.txtTaisaku.Text))
            {
                dr[Def_T_AR.TAISAKU] = this.txtTaisaku.Text;
            }
            if (!string.IsNullOrEmpty(this.txtGenchiTehaisaki.Text))
            {
                dr[Def_T_AR.GENCHI_TEHAISAKI] = this.txtGenchiTehaisaki.Text;
            }
            if (this.dtpGenchiSetteinokiDate.Value != null)
            {
                dr[Def_T_AR.GENCHI_SETTEINOKI_DATE] = this.dtpGenchiSetteinokiDate.Text;
            }
            if (this.dtpGenchiShukkayoteiDate.Value != null)
            {
                dr[Def_T_AR.GENCHI_SHUKKAYOTEI_DATE] = this.dtpGenchiShukkayoteiDate.Text;
            }
            if (this.dtpGenchiKojyoshukkaDate.Value != null)
            {
                dr[Def_T_AR.GENCHI_KOJYOSHUKKA_DATE] = this.dtpGenchiKojyoshukkaDate.Text;
            }
            if (!string.IsNullOrEmpty(this.txtShukkahoho.Text))
            {
                dr[Def_T_AR.SHUKKAHOHO] = this.txtShukkahoho.Text;
            }
            if (this.dtpJpSetteinokiDate.Value != null)
            {
                dr[Def_T_AR.JP_SETTEINOKI_DATE] = this.dtpJpSetteinokiDate.Text;
            }
            if (this.dtpJpShukkayoteiDate.Value != null)
            {
                dr[Def_T_AR.JP_SHUKKAYOTEI_DATE] = this.dtpJpShukkayoteiDate.Text;
            }
            if (this.dtpJpKojyoshukkaDate.Value != null)
            {
                dr[Def_T_AR.JP_KOJYOSHUKKA_DATE] = this.dtpJpKojyoshukkaDate.Text;
            }
            if (!string.IsNullOrEmpty(this.txtJpUnsokaishaName.Text))
            {
                dr[Def_T_AR.JP_UNSOKAISHA_NAME] = this.txtJpUnsokaishaName.Text;
            }
            if (!string.IsNullOrEmpty(this.txtJpOkurijyoNo.Text))
            {
                dr[Def_T_AR.JP_OKURIJYO_NO] = this.txtJpOkurijyoNo.Text;
            }
            if (!string.IsNullOrEmpty(this.txtTaioBusho.Text))
            {
                dr[Def_T_AR.TAIO_BUSHO] = this.txtTaioBusho.Text;
            }
            if (!string.IsNullOrEmpty(this.txtGmsHakkoNo.Text))
            {
                dr[Def_T_AR.GMS_HAKKO_NO] = this.txtGmsHakkoNo.Text;
            }
            if (!string.IsNullOrEmpty(this.txtShiyorenrakuNo.Text))
            {
                dr[Def_T_AR.SHIYORENRAKU_NO] = this.txtShiyorenrakuNo.Text;
            }
            if (!string.IsNullOrEmpty(this.txtGirenNo1.Text))
            {
                dr[Def_T_AR.GIREN_NO_1] = this.txtGirenNo1.Text;
            }
            if (!string.IsNullOrEmpty(this.txtGirenFile1.Text))
            {
                dr[Def_T_AR.GIREN_FILE_1] = this.txtGirenFile1.Text;
            }
            if (!string.IsNullOrEmpty(this.txtGirenNo2.Text))
            {
                dr[Def_T_AR.GIREN_NO_2] = this.txtGirenNo2.Text;
            }
            if (!string.IsNullOrEmpty(this.txtGirenFile2.Text))
            {
                dr[Def_T_AR.GIREN_FILE_2] = this.txtGirenFile2.Text;
            }
            if (!string.IsNullOrEmpty(this.txtGirenNo3.Text))
            {
                dr[Def_T_AR.GIREN_NO_3] = this.txtGirenNo3.Text;
            }
            if (!string.IsNullOrEmpty(this.txtGirenFile3.Text))
            {
                dr[Def_T_AR.GIREN_FILE_3] = this.txtGirenFile3.Text;
            }
            if (!string.IsNullOrEmpty(this.txtGirenNo4.Text))
            {
                dr[Def_T_AR.GIREN_NO_4] = this.txtGirenNo4.Text;
            }
            if (!string.IsNullOrEmpty(this.txtGirenFile4.Text))
            {
                dr[Def_T_AR.GIREN_FILE_4] = this.txtGirenFile4.Text;
            }
            if (!string.IsNullOrEmpty(this.txtGirenNo5.Text))
            {
                dr[Def_T_AR.GIREN_NO_5] = this.txtGirenNo5.Text;
            }
            if (!string.IsNullOrEmpty(this.txtGirenFile5.Text))
            {
                dr[Def_T_AR.GIREN_FILE_5] = this.txtGirenFile5.Text;
            }
            if (!string.IsNullOrEmpty(this.txtRefNo1.Text))
            {
                dr[Def_T_AR.REFERENCE_NO_1] = this.txtRefNo1.Text;
            }
            if (!string.IsNullOrEmpty(this.txtRefFile1.Text))
            {
                dr[Def_T_AR.REFERENCE_FILE_1] = this.txtRefFile1.Text;
            }
            if (!string.IsNullOrEmpty(this.txtRefNo2.Text))
            {
                dr[Def_T_AR.REFERENCE_NO_2] = this.txtRefNo2.Text;
            }
            if (!string.IsNullOrEmpty(this.txtRefFile2.Text))
            {
                dr[Def_T_AR.REFERENCE_FILE_2] = this.txtRefFile2.Text;
            }
            if (!string.IsNullOrEmpty(this.txtBiko.Text))
            {
                dr[Def_T_AR.BIKO] = this.txtBiko.Text;
            }
            if (!string.IsNullOrEmpty(this.txtMotoArNo.Text))
            {
                dr[Def_T_AR.MOTO_AR_NO] = this.lblMotoArLabel.Text + this.txtMotoArNo.Text;
            }
            if (!string.IsNullOrEmpty(this.txtShukkaJokyo.Text))
            {
                dr[Def_T_AR.GENCHI_SHUKKAJYOKYO_FLAG] = this.txtShukkaJokyo.Text;
            }

            dt.Rows.Add(dr);

            return dt;
        }

        /// --------------------------------------------------
        /// <summary>
        /// AR進捗DBの更新データ取得
        /// </summary>
        /// <returns>データテーブル</returns>
        /// <create>Y.Nakasato 2019/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetDisplayShinchokuAdd(string tableName)
        {
            var dt = GetSchemaARShinchoku(tableName);

            // 進捗管理する場合
            if ((this._gokiNum > 0) && string.Equals(AR_SHINCHOKU_FLAG.ON_VALUE1, this.cboShinchokuRiyouFlag.SelectedValue))
            {
                string[] inputGokiArray = this.cboGoki.Tag as string[] ?? new string[0];

                // 追加
                var exceptGokiArray = inputGokiArray.Except<string>(this._gokiShinchokuList).ToArray();
                foreach (string exceptGoki in exceptGokiArray)
                {
                    var dr = dt.NewRow();
                    dr[Def_T_AR_SHINCHOKU.NONYUSAKI_CD] = this._nonyusakiCd;
                    dr[Def_T_AR_SHINCHOKU.LIST_FLAG] = this._listFlag;
                    dr[Def_T_AR_SHINCHOKU.AR_NO] = this._arNo; // サーバー処理で上書きされる
                    dr[Def_T_AR_SHINCHOKU.GOKI] = exceptGoki;
                    // 既に登録がある場合は、日付は空白にする
                    if (this._gokiShinchokuList.Count > 0)
                    {
                        dr[Def_T_AR_SHINCHOKU.DATE_SITE_REQ] = string.Empty;
                        dr[Def_T_AR_SHINCHOKU.DATE_LOCAL] = string.Empty;
                        dr[Def_T_AR_SHINCHOKU.DATE_JP] = string.Empty;
                    }
                    else
                    {
                        dr[Def_T_AR_SHINCHOKU.DATE_SITE_REQ] = this.dtpGenbaTotyakukibouDate.Text;
                        dr[Def_T_AR_SHINCHOKU.DATE_LOCAL] = this.dtpGenchiShukkayoteiDate.Text;
                        dr[Def_T_AR_SHINCHOKU.DATE_JP] = this.dtpJpShukkayoteiDate.Text;
                    }
                    dt.Rows.Add(dr);
                }

            }
            // 全部進捗管理しない場合
            else
            {
                // 処理なし
            }
            return dt;
        }

        /// --------------------------------------------------
        /// <summary>
        /// AR進捗DBの削除データ取得
        /// </summary>
        /// <returns>データテーブル</returns>
        /// <create>Y.Nakasato 2019/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetDisplayShinchokuDelete(string tableName)
        {
            var dt = GetSchemaARShinchoku(tableName);
            // 取得データがない場合
            if (this._dtARShinchokuData == null)
                return dt;
            // 進捗管理する場合
            if ((this._gokiNum > 0) && string.Equals(AR_SHINCHOKU_FLAG.ON_VALUE1, this.cboShinchokuRiyouFlag.SelectedValue))
            {
                string[] inputGokiArray = this.cboGoki.Tag as string[] ?? new string[0];

                // 削除
                foreach (DataRow drDel in this._dtARShinchokuData.AsEnumerable().Where(w => !inputGokiArray.Contains(ComFunc.GetFld(w, Def_T_AR_SHINCHOKU.GOKI))))
                {
                    dt.ImportRow(drDel);
                }
            }
            // 全部進捗管理しない場合
            else
            {
                // 削除
                foreach (DataRow drDel in this._dtARShinchokuData.AsEnumerable())
                {
                    dt.ImportRow(drDel);
                }
            }
            return dt;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 画面入力データ取得(対応費用)
        /// </summary>
        /// <param name="tblName">テーブル名</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetDisplayCost(string tblName)
        {
            var dt = GetSchemaARCost(tblName);

            for (int rowIndex = 0; rowIndex < this.shtARCost.Rows.Count; rowIndex++)
            {
                var item = this.shtARCost[SHEET_COL_ITEM, rowIndex].Value;
                if (item != null)
                {
                    var dr = dt.NewRow();
                    dr[Def_T_AR_COST.NONYUSAKI_CD] = this._nonyusakiCd;
                    dr[Def_T_AR_COST.LIST_FLAG] = this._listFlag;
                    dr[Def_T_AR_COST.AR_NO] = this._arNo;
                    dr[Def_T_AR_COST.LINE_NO] = rowIndex + 1;
                    dr[Def_T_AR_COST.ITEM_CD] = this.shtARCost[SHEET_COL_ITEM, rowIndex].Value;
                    dr[Def_T_AR_COST.WORK_TIME] = DSWUtil.UtilConvert.ToDecimal(this.shtARCost[SHEET_COL_WORK_TIME, rowIndex].Text);
                    dr[Def_T_AR_COST.WORKERS] = DSWUtil.UtilConvert.ToDecimal(this.shtARCost[SHEET_COL_WORKERS, rowIndex].Text);
                    dr[Def_T_AR_COST.NUMBER] = DSWUtil.UtilConvert.ToDecimal(this.shtARCost[SHEET_COL_NUMBER, rowIndex].Text);
                    if (this._staffKbn == STAFF_KBN.STAFF_VALUE1)
                    {
                        dr[Def_T_AR_COST.RATE] = DSWUtil.UtilConvert.ToDecimal(this.shtARCost[SHEET_COL_RATE, rowIndex].Text);
                        dr[Def_T_AR_COST.TOTAL] = DSWUtil.UtilConvert.ToDecimal(this.shtARCost[SHEET_COL_TOTAL, rowIndex].Text);
                    }
                    else
                    {
                        dr[Def_T_AR_COST.RATE] = DBNull.Value;
                        dr[Def_T_AR_COST.TOTAL] = DBNull.Value;
                    }
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 画面入力データ取得(添付ファイル)
        /// </summary>
        /// <param name="tblName">テーブル名</param>
        /// <returns></returns>
        /// <create>D.Okumura 2019/06/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetDisplayFile(string tblName)
        {
            var dt = GetSchemaARFile(tblName);
            txtHuguai.SetupFilesToDataTable(dt, this._nonyusakiCd, this._listFlag, this._arNo, AR_FILE_TYPE.HUGUAI_VALUE1);
            txtTaisaku.SetupFilesToDataTable(dt, this._nonyusakiCd, this._listFlag, this._arNo, AR_FILE_TYPE.TAISAKU_VALUE1);
            txtBiko.SetupFilesToDataTable(dt, this._nonyusakiCd, this._listFlag, this._arNo, AR_FILE_TYPE.BIKO_VALUE1);
            return dt;
        }

        #endregion

        #region クローズ処理

        /// --------------------------------------------------
        /// <summary>
        /// クローズ処理
        /// </summary>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2010/08/25</create>
        /// <update>D.Okumura 2019/06/13</update>
        /// <update>D.Okumura 2019/06/24 リッチテキストメモリ不足対応</update>
        /// <update>D.Okumura 2019/06/26 リッチテキストメモリ不足負荷軽減対応</update>
        /// --------------------------------------------------
        protected override void OnClosing(CancelEventArgs e)
        {
            if (e.Cancel != true)
            {
                // 子画面を確認し、開いていたら画面を閉じない
                for (var i = 0; i < _detailForms.Length; i++)
                {
                    if (_detailForms[i] == null)
                        continue;
                    e.Cancel = true;
                    _detailForms[i].Activate();
                    return;
                }
                // リッチテキストメモリ不足対応：クリップボード内に大きなデータ(画像など)があるときは警告する
                var hasImage = txtHuguai.HasClipboardDataFromRichText()
                            || txtTaisaku.HasClipboardDataFromRichText()
                            || txtBiko.HasClipboardDataFromRichText()
                            ;
                if (hasImage)
                {
                    // 更新でのCloseがあるので、クローズについて質問するかどうかで判断する。
                    // 質問ありの場合はYes/No/Cancel動作とし、質問なしの場合はYes/Noのみとする。
                    var result = this.ShowMessage(IsCloseQuestion ? "A9999999073" : "A9999999075");
                    switch (result)
                    {
                        case DialogResult.Yes:
                            try
                            {
                                Clipboard.Clear();
                            }
                            catch { }
                            IsCloseQuestion = false;
                            break;
                        case DialogResult.No:
                            IsCloseQuestion = false;
                            break;
                        case DialogResult.Cancel:
                        default:
                            e.Cancel = true;
                            return;
                    }
                }
            }
            base.OnClosing(e);

            try
            {
                if (e.Cancel != true)
                {
                    // インターロック 解除
                    if (EditMode == SystemBase.EditMode.Update)
                    {
                        // コンディション取得
                        CondA1 cond = this.GetCondition();

                        ConnA01 conn = new ConnA01();
                        bool ret = conn.ARInterUnLock(cond, _dtAR);
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 機種号機整合性チェック

        /// --------------------------------------------------
        /// <summary>
        /// 設定されている機種と号機の整合性をチェックする
        /// </summary>
        /// <returns>チェック結果(true:正常、false:異常)</returns>
        /// <create>Y.Nakasato 2019/08/01</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckKishuGoki()
        {
            try
            {
                bool result = true;
                var cond = GetCondition();
                var conn = new ConnA01();
                cond.ArNo = this._arNo;
                cond.Kishu = this.cboKishu.Text;

                var dt = conn.GetGoki(cond);
                List<ComFunc.GokiInfoList> gokiInfoList = new List<ComFunc.GokiInfoList>();
                int dmy = 0;
                ComFunc.CreateGokiInfoListFromDt(dt, gokiInfoList, out dmy);
                string[] inputGokiArray = this.cboGoki.Tag as string[] ?? new string[0];
                if ((inputGokiArray.Length == 0)
                 || ((gokiInfoList.Count == 0) && (inputGokiArray.Length > 0)))
                {
                    result = false;
                }
                else
                {
                    string[] gokiInfoArray = gokiInfoList.ToArray().Select(x => x.Goki).ToArray();
                    if (inputGokiArray.Except<string>(gokiInfoArray).Count() > 0)
                    {
                        result = false;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region 技連Noファイル処理

        #region ファイル添付(選択)

        /// --------------------------------------------------
        /// <summary>
        /// ファイル添付(ファイル選択)
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="fullPathName">ファイル名(フルパス)</param>
        /// <returns></returns>
        /// <create>M.Tsutsumi 2010/09/01</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool GirenFileOpen(ref string fileName, ref string fullPathName)
        {
            try
            {
                fileName = string.Empty;

                OpenFileDialog frm = new OpenFileDialog();

                frm.Filter = Resources.ARJohoMeisai_Filter;
                frm.Title = Resources.ARJohoMeisai_FileOpen;
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    return false;
                }

                // ファイルサイズチェック
                FileInfo fi = new FileInfo(frm.FileName);
                long fileSize = fi.Length;
                if (ComDefine.GIREN_FILE_MAX_SIZE < fileSize)
                {
                    // 添付するファイルサイズが大きすぎます。
                    this.ShowMessage("A0100020005");
                    return false;
                }
                int nameSize = UtilString.GetByteCount(frm.SafeFileName);
                if (GIREN_FILE_NAME_SIZE < nameSize)
                {
                    // ファイル名が長すぎます。
                    this.ShowMessage("A0100020006");
                    return false;
                }

                fileName = frm.SafeFileName;
                fullPathName = frm.FileName;

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ファイル更新

        /// --------------------------------------------------
        /// <summary>
        /// ファイル更新
        /// </summary>
        /// <param name="nonyusakiCd">納入先コード</param>
        /// <param name="arNo">ARNo</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>M.Tsutsumi 2010/09/01</create>
        /// <update>D.Okumura 2019/06/18 添付ファイル対応</update>
        /// --------------------------------------------------
        private bool GirenFileUpdate(string nonyusakiCd, string arNo)
        {
            try
            {
                // 技連1
                if (!this.GirenFileUpdate(this._attachGirenFullPath1, this._attachGirenFile1, this._delGirenFile1, nonyusakiCd, arNo, GirenType.No1))
                {
                    return false;
                }

                // 技連2
                if (!this.GirenFileUpdate(this._attachGirenFullPath2, this._attachGirenFile2, this._delGirenFile2, nonyusakiCd, arNo, GirenType.No2))
                {
                    return false;
                }

                // 技連3
                if (!this.GirenFileUpdate(this._attachGirenFullPath3, this._attachGirenFile3, this._delGirenFile3, nonyusakiCd, arNo, GirenType.No3))
                {
                    return false;
                }

                // 技連4
                if (!this.GirenFileUpdate(this._attachGirenFullPath4, this._attachGirenFile4, this._delGirenFile4, nonyusakiCd, arNo, GirenType.No4))
                {
                    return false;
                }

                // 技連5
                if (!this.GirenFileUpdate(this._attachGirenFullPath5, this._attachGirenFile5, this._delGirenFile5, nonyusakiCd, arNo, GirenType.No5))
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                //throw new Exception(ex.Message, ex);
                return false;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// ファイル更新
        /// </summary>
        /// <param name="fileName">アップロードファイル</param>
        /// <param name="delFileName">デリートファイル</param>
        /// <param name="nonyusakiCd">納入先コード</param>
        /// <param name="arNo">ARNo</param>
        /// <param name="girenType">技連タイプ</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>M.Tsutsumi 2010/09/01</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool GirenFileUpdate(string fullPathName, string fileName, string delFileName, string nonyusakiCd, string arNo, GirenType girenType)
        {
            try
            {
                bool ret = true;

                if (!string.IsNullOrEmpty(fullPathName) && !string.IsNullOrEmpty(fileName))
                {
                    // 新しいファイルあり
                    ret = this.GirenFileUpload(fullPathName, fileName, delFileName, nonyusakiCd, arNo, girenType);
                }
                else
                {
                    // 新しいファイルなし
                    if (!string.IsNullOrEmpty(delFileName))
                    {
                        // 削除するファイルあり
                        ret = this.GirenFileDelete(delFileName, nonyusakiCd, arNo, girenType);
                    }
                }

                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ファイル(download)

        /// --------------------------------------------------
        /// <summary>
        /// ファイル download処理
        /// </summary>
        /// <param name="fileName">ダウンロードファイル名</param>
        /// <param name="girenType">技連タイプ</param>
        /// <param name="noDialog">ダイアログを表示するかどうか</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>M.Tsutsumi 2010/09/01</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool GirenFileDownload(string fileName, GirenType girenType, bool noDialog)
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                // 保存パス指定
                SaveFileDialog frm = new SaveFileDialog();
                if (!noDialog)
                {
                    // 2012/07/06 K.Tsutsumi Change
                    //// @@@ 2011/02/24 M.Tsutsumi Change 
                    ////frm.Filter = "PDFファイル(*.PDF)|*.PDF";
                    //frm.Filter = "PDF Files(*.PDF)|*.PDF";
                    //// @@@ ↑
                    frm.Filter = Resources.ARJohoMeisai_Filter;
                    // ↑
                    frm.FileName = fileName;
                    if (frm.ShowDialog() != DialogResult.OK)
                    {
                        return false;
                    }
                    if (string.IsNullOrEmpty(frm.FileName))
                    {
                        // ファイル名なし
                        return false;
                    }
                }
                else
                {
                    frm.FileName = Path.Combine(ComDefine.DOWNLOAD_DIR, fileName);
                    if (!Directory.Exists(ComDefine.DOWNLOAD_DIR))
                    {
                        Directory.CreateDirectory(ComDefine.DOWNLOAD_DIR);
                    }
                }

                // ここで変更しないと保存ダイアログを出すとデフォルトに戻される。
                Cursor.Current = Cursors.WaitCursor;

                ConnAttachFile conn = new ConnAttachFile();
                FileDownloadPackage package = new FileDownloadPackage();

                package.FileName = fileName;
                package.FileType = FileType.ARGiren;
                package.NonyusakiCD = this._nonyusakiCd;
                package.ARNo = this._arNo;
                package.GirenType = girenType;

                FileDownloadResult result = conn.FileDownload(package);
                if (!result.IsExistsFile)
                {
                    // ダウンロードするファイルが削除されています。
                    this.ShowMessage("A0100020010");
                    return false;
                }

                // ファイル保存
                using (Stream strm = new FileStream(frm.FileName, FileMode.Create))
                {
                    strm.Write(result.FileData, 0, result.FileData.Length);
                    strm.Close();
                }

                if (noDialog)
                {
                    Process.Start(frm.FileName);
                }
                else
                {
                    // ファイルのダウンロードが完了しました。
                    this.ShowMessage("A0100020009");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                Cursor.Current = preCursor;
            }
        }

        #endregion

        #region ファイル(upload)

        /// --------------------------------------------------
        /// <summary>
        /// ファイル upload処理
        /// </summary>
        /// <param name="fileName">アップロードファイル</param>
        /// <param name="delFileName">デリートファイル</param>
        /// <param name="nonyusakiCd">納入先コード</param>
        /// <param name="arNo">ARNo</param>
        /// <param name="girenType">技連タイプ</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>M.Tsutsumi 2010/09/01</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool GirenFileUpload(string fullPathName, string fileName, string delFileName, string nonyusakiCd, string arNo, GirenType girenType)
        {
            try
            {
                using (Stream strm = new FileStream(fullPathName, FileMode.Open))
                {
                    ConnAttachFile conn = new ConnAttachFile();
                    FileUploadPackage package = new FileUploadPackage();

                    byte[] data = new byte[strm.Length];
                    strm.Position = 0;
                    strm.Read(data, 0, (int)strm.Length);
                    package.FileData = data;
                    package.FileName = fileName;
                    package.DeleteFileName = delFileName;
                    package.FileType = FileType.ARGiren;
                    package.NonyusakiCD = nonyusakiCd;
                    package.ARNo = arNo;
                    package.GirenType = girenType;

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

        #region ファイル(delete)

        /// --------------------------------------------------
        /// <summary>
        /// ファイル delete処理
        /// </summary>
        /// <param name="fileName">デリートファイル</param>
        /// <param name="nonyusakiCd">納入先コード</param>
        /// <param name="arNo">ARNo</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>M.Tsutsumi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool GirenFileDelete(string fileName, string nonyusakiCd, string arNo, GirenType girenType)
        {
            try
            {
                ConnAttachFile conn = new ConnAttachFile();
                FileDeletePackage package = new FileDeletePackage();

                package.FileName = fileName;
                package.FileType = FileType.ARGiren;
                package.NonyusakiCD = nonyusakiCd;
                package.ARNo = arNo;
                package.GirenType = girenType;

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

        #endregion

        #region 参考資料Noファイル処理

        #region ファイル添付(選択)

        /// --------------------------------------------------
        /// <summary>
        /// ファイル添付(ファイル選択)
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="fullPathName">ファイル名(フルパス)</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RefFileOpen(ref string fileName, ref string fullPathName)
        {
            try
            {
                fileName = string.Empty;

                OpenFileDialog frm = new OpenFileDialog();

                frm.Filter = Resources.ARJohoMeisai_RefFilter;
                frm.Title = Resources.ARJohoMeisai_FileOpen;
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    return false;
                }

                // ファイルサイズチェック
                FileInfo fi = new FileInfo(frm.FileName);
                long fileSize = fi.Length;
                if (ComDefine.REF_FILE_MAX_SIZE < fileSize)
                {
                    // 添付するファイルサイズが大きすぎます。
                    this.ShowMessage("A0100020005");
                    return false;
                }
                int nameSize = UtilString.GetByteCount(frm.SafeFileName);
                if (REF_FILE_NAME_SIZE < nameSize)
                {
                    // ファイル名が長すぎます。
                    this.ShowMessage("A0100020006");
                    return false;
                }

                fileName = frm.SafeFileName;
                fullPathName = frm.FileName;

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ファイル更新

        /// --------------------------------------------------
        /// <summary>
        /// ファイル更新
        /// </summary>
        /// <param name="nonyusakiCd">納入先コード</param>
        /// <param name="arNo">ARNo</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RefFileUpdate(string nonyusakiCd, string arNo)
        {
            try
            {
                // 参考資料1
                if (!this.RefFileUpdate(this._attachRefFullPath1, this._attachRefFile1, this._delRefFile1, nonyusakiCd, arNo, GirenType.RefNo1))
                {
                    return false;
                }

                // 参考資料2
                if (!this.RefFileUpdate(this._attachRefFullPath2, this._attachRefFile2, this._delRefFile2, nonyusakiCd, arNo, GirenType.RefNo2))
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                //throw new Exception(ex.Message, ex);
                return false;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// ファイル更新
        /// </summary>
        /// <param name="fileName">アップロードファイル</param>
        /// <param name="delFileName">デリートファイル</param>
        /// <param name="nonyusakiCd">納入先コード</param>
        /// <param name="arNo">ARNo</param>
        /// <param name="refType">技連タイプ</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RefFileUpdate(string fullPathName, string fileName, string delFileName, string nonyusakiCd, string arNo, GirenType refType)
        {
            try
            {
                bool ret = true;

                if (!string.IsNullOrEmpty(fullPathName) && !string.IsNullOrEmpty(fileName))
                {
                    // 新しいファイルあり
                    ret = this.RefFileUpload(fullPathName, fileName, delFileName, nonyusakiCd, arNo, refType);
                }
                else
                {
                    // 新しいファイルなし
                    if (!string.IsNullOrEmpty(delFileName))
                    {
                        // 削除するファイルあり
                        ret = this.RefFileDelete(delFileName, nonyusakiCd, arNo, refType);
                    }
                }

                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// AR添付ファイルデータのアップロード
        /// </summary>
        /// <param name="nonyusakiCd">納入先コード</param>
        /// <param name="arno">ARNo</param>
        /// <returns>アップロード結果</returns>
        /// <create>D.Okumura 2019/06/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RtfFileUpload(string nonyusakiCd, string arno)
        {
            var result = true;
            foreach (var txt in new[] { txtHuguai, txtTaisaku, txtBiko })
            {
                var ret = txt.Upload(nonyusakiCd, arno);
                if (!ret)
                    result = false;
            }
            return result;
        }

        #endregion

        #region ファイル(download)

        /// --------------------------------------------------
        /// <summary>
        /// ファイル download処理
        /// </summary>
        /// <param name="fileName">ダウンロードファイル名</param>
        /// <param name="girenType">参考資料タイプ</param>
        /// <param name="noDialog">ダイアログを表示するかどうか</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RefFileDownload(string fileName, GirenType girenType, bool noDialog)
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                // 保存パス指定
                SaveFileDialog frm = new SaveFileDialog();
                if (!noDialog)
                {
                    frm.Filter = Resources.ARJohoMeisai_Filter;
                    frm.FileName = fileName;
                    if (frm.ShowDialog() != DialogResult.OK)
                    {
                        return false;
                    }
                    if (string.IsNullOrEmpty(frm.FileName))
                    {
                        // ファイル名なし
                        return false;
                    }
                }
                else
                {
                    frm.FileName = Path.Combine(ComDefine.DOWNLOAD_DIR, fileName);
                    if (!Directory.Exists(ComDefine.DOWNLOAD_DIR))
                    {
                        Directory.CreateDirectory(ComDefine.DOWNLOAD_DIR);
                    }
                }

                // ここで変更しないと保存ダイアログを出すとデフォルトに戻される。
                Cursor.Current = Cursors.WaitCursor;

                ConnAttachFile conn = new ConnAttachFile();
                FileDownloadPackage package = new FileDownloadPackage();

                package.FileName = fileName;
                package.FileType = FileType.ARRef;
                package.NonyusakiCD = this._nonyusakiCd;
                package.ARNo = this._arNo;
                package.GirenType = girenType;

                FileDownloadResult result = conn.FileDownload(package);
                if (!result.IsExistsFile)
                {
                    // ダウンロードするファイルが削除されています。
                    this.ShowMessage("A0100020010");
                    return false;
                }

                // ファイル保存
                using (Stream strm = new FileStream(frm.FileName, FileMode.Create))
                {
                    strm.Write(result.FileData, 0, result.FileData.Length);
                    strm.Close();
                }

                if (noDialog)
                {
                    Process.Start(frm.FileName);
                }
                else
                {
                    // ファイルのダウンロードが完了しました。
                    this.ShowMessage("A0100020009");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                Cursor.Current = preCursor;
            }
        }

        #endregion

        #region ファイル(upload)

        /// --------------------------------------------------
        /// <summary>
        /// ファイル upload処理
        /// </summary>
        /// <param name="fileName">アップロードファイル</param>
        /// <param name="delFileName">デリートファイル</param>
        /// <param name="nonyusakiCd">納入先コード</param>
        /// <param name="arNo">ARNo</param>
        /// <param name="girenType">参考資料タイプ</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RefFileUpload(string fullPathName, string fileName, string delFileName, string nonyusakiCd, string arNo, GirenType girenType)
        {
            try
            {
                using (Stream strm = new FileStream(fullPathName, FileMode.Open))
                {
                    ConnAttachFile conn = new ConnAttachFile();
                    FileUploadPackage package = new FileUploadPackage();

                    byte[] data = new byte[strm.Length];
                    strm.Position = 0;
                    strm.Read(data, 0, (int)strm.Length);
                    package.FileData = data;
                    package.FileName = fileName;
                    package.DeleteFileName = delFileName;
                    package.FileType = FileType.ARRef;
                    package.NonyusakiCD = nonyusakiCd;
                    package.ARNo = arNo;
                    package.GirenType = girenType;

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

        #region ファイル(delete)

        /// --------------------------------------------------
        /// <summary>
        /// ファイル delete処理
        /// </summary>
        /// <param name="fileName">デリートファイル</param>
        /// <param name="nonyusakiCd">納入先コード</param>
        /// <param name="arNo">ARNo</param>
        /// <param name="girenType">参考資料タイプ</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RefFileDelete(string fileName, string nonyusakiCd, string arNo, GirenType girenType)
        {
            try
            {
                ConnAttachFile conn = new ConnAttachFile();
                FileDeletePackage package = new FileDeletePackage();

                package.FileName = fileName;
                package.FileType = FileType.ARRef;
                package.NonyusakiCD = nonyusakiCd;
                package.ARNo = arNo;
                package.GirenType = girenType;

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
        
        #endregion

        #region 指示文言取得

        /// --------------------------------------------------
        /// <summary>
        /// 指示文言取得
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetCommonListFlag()
        {
            var cond = new CondA1(this.UserInfo);
            cond.ListFlag = this._listFlag;
            var dt = new ConnA01().GetCommonListFlag(cond);
            return dt;
        }

        #endregion

        #region スタッフ区分取得

        /// --------------------------------------------------
        /// <summary>
        /// スタッフ区分取得
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private string GetStaffKbn()
        {
            try
            {
                var conn = new ConnA01();
                var cond = new CondA1(this.UserInfo);
                cond.UpdateUserID = this.UserInfo.UserID;
                var dt = new ConnA01().GetUser(cond);
                if (!UtilData.ExistsData(dt))
                {
                    return STAFF_KBN.STAFF_VALUE1;
                }
                else
                {
                    return ComFunc.GetFld(dt, 0, Def_M_USER.STAFF_KBN);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return STAFF_KBN.STAFF_VALUE1;
            }
        }

        #endregion

        #region 出荷予定日の制御

        /// --------------------------------------------------
        /// <summary>
        /// 出荷予定日の有効無効を判定します
        /// </summary>
        /// <returns>true:有効 false:無効</returns>
        /// <create>M.Shimizu 2020/04/10 AR進捗・関連付け対応</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool IsEnableShukkayoteiDate()
        {
            switch (this.EditMode)
            {
                case SystemBase.EditMode.Insert:
                    // 登録は有効
                    return true;
                case SystemBase.EditMode.Update:
                    // 編集は進捗管理利用しない場合は有効、進捗管理利用する場合は無効
                    return AR_SHINCHOKU_FLAG.OFF_VALUE1.Equals(cboShinchokuRiyouFlag.SelectedValue);
                case SystemBase.EditMode.View:
                    // 照会は無効
                    return false;
                default:
                    return true;
            }
        }

        #endregion

        #region 進捗管理利用の初期値

        /// --------------------------------------------------
        /// <summary>
        /// 進捗管理利用のデフォルト値を取得します
        /// </summary>
        /// <returns>"0":進捗管理しない　"1":進捗管理する</returns>
        /// <create>M.Shimizu 2020/04/07 AR進捗・関連付け対応</create>
        /// <update></update>
        /// --------------------------------------------------
        private string GetDefaultShinchokuRiyou()
        {
            // 員数表が取り込まれていない場合は、進捗管理利用を「しない」
            if (_gokiNum == 0)
            {
                return AR_SHINCHOKU_FLAG.OFF_VALUE1;
            }

            // リスト区分で引き当て、初期値を取得
            string value = _dtArListFlagShinchoku.AsEnumerable()
                .Where(row => ComFunc.GetFld(row, Def_M_COMMON.ITEM_CD).Equals("FLAG_" + _listFlag))
                .Select(row => ComFunc.GetFld(row, Def_M_COMMON.VALUE3))
                .FirstOrDefault();

            // マスタの設定値を返却。マスタが設定されていない場合は、進捗管理利用を「しない」
            return string.IsNullOrEmpty(value) ? AR_SHINCHOKU_FLAG.OFF_VALUE1 : value;
        }

        #endregion

        #region 機種、号機設定

        /// --------------------------------------------------
        /// <summary>
        /// 員数表を使用するか判定
        /// </summary>
        /// <returns>true:員数表 false:機種一覧</returns>
        /// <create>M.Shimizu 2020/04/07 AR進捗・関連付け対応</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CanUseInzuhyo()
        {
            // 員数表が取り込まれていない場合は、機種一覧を使用
            if (_gokiNum == 0)
            {
                return false;
            }

            // 進捗管理利用
            bool isShinchokuRiyou = AR_SHINCHOKU_FLAG.ON_VALUE1.Equals(cboShinchokuRiyouFlag.SelectedValue);

            // リスト区分で引き当て、進捗管理利用の選択によって取得項目を切り替える
            string value = _dtArListFlagShinchoku.AsEnumerable()
                .Where(row => ComFunc.GetFld(row, Def_M_COMMON.ITEM_CD).Equals("FLAG_" + _listFlag))
                .Select(row => ComFunc.GetFld(row, isShinchokuRiyou ? Def_M_COMMON.VALUE2 : Def_M_COMMON.VALUE1))
                .FirstOrDefault();

            // "1"の場合は員数表、"1"以外の場合は機種一覧
            return AR_SHINCHOKU_FLAG.ON_VALUE1.Equals(value);
        }

        #endregion

        #region メール添付ファイル一時保存

        /// --------------------------------------------------
        /// <summary>
        /// メール添付ファイル一時保存
        /// </summary>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>J.Chen 2024/08/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool AttachmentFileCopy()
        {
            try
            {
                // フォルダクリア
                if (!AttachmentFileDel())
                {
                    return false;
                }

                // 添付ファイルリスト初期化
                attachments = new List<string>();

                // 技連1
                if (!this.AttachmentFileCopy(this._attachGirenFullPath1, this._attachGirenFile1))
                {
                    return false;
                }

                // 技連2
                if (!this.AttachmentFileCopy(this._attachGirenFullPath2, this._attachGirenFile2))
                {
                    return false;
                }

                // 技連3
                if (!this.AttachmentFileCopy(this._attachGirenFullPath3, this._attachGirenFile3))
                {
                    return false;
                }

                // 技連4
                if (!this.AttachmentFileCopy(this._attachGirenFullPath4, this._attachGirenFile4))
                {
                    return false;
                }

                // 技連5
                if (!this.AttachmentFileCopy(this._attachGirenFullPath5, this._attachGirenFile5))
                {
                    return false;
                }

                // 参考資料1
                if (!this.AttachmentFileCopy(this._attachRefFullPath1, this._attachRefFile1))
                {
                    return false;
                }

                // 参考資料2
                if (!this.AttachmentFileCopy(this._attachRefFullPath2, this._attachRefFile2))
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                //throw new Exception(ex.Message, ex);
                return false;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// メール添付ファイル一時保存（コピー処理）
        /// </summary>
        /// <param name="fullPathName">元ファイルパス</param>
        /// <param name="fileName">アップロードファイル</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>J.Chen 2024/08/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool AttachmentFileCopy(string fullPathName, string fileName)
        {
            try
            {
                bool ret = true;
                string tempPathName = ComDefine.AR_OUTPUT_DIR;

                if (!string.IsNullOrEmpty(fullPathName) && !string.IsNullOrEmpty(fileName))
                {
                    // ファイルをコピー
                    tempPathName = Path.Combine(tempPathName, fileName);
                    File.Copy(fullPathName, tempPathName, true);
                    
                    // 添付ファイルリストに追加
                    attachments.Add(tempPathName);
                }
                else
                {
                }

                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// メール添付ファイル一時フォルダクリア
        /// </summary>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>J.Chen 2024/08/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool AttachmentFileDel()
        {
            try
            {
                bool ret = true;
                string tempPathName = ComDefine.AR_OUTPUT_DIR;

                // コピー先フォルダ内のファイルをクリアする
                if (Directory.Exists(tempPathName))
                {
                    // フォルダ内のすべてのファイルを取得
                    string[] files = Directory.GetFiles(tempPathName);

                    // 各ファイルを削除
                    foreach (string file in files)
                    {
                        File.Delete(file);
                    }
                }
                else
                {
                    // フォルダが存在しない場合は新しく作成
                    Directory.CreateDirectory(tempPathName);
                }

                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ファイル圧縮

        /// --------------------------------------------------
        /// <summary>
        /// ファイル圧縮
        /// </summary>
        /// <param name="sourceFilePaths">対象ファイルパス</param>
        /// <param name="compressedFilePath">保存パス</param>
        /// <returns></returns>
        /// <create>J.Chen 2024/08/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CompressFilesAsAttachments(List<string> sourceFilePaths, string compressedFilePath)
        {
            try
            {
                using (ZipFile zip = new ZipFile())
                {
                    // 古い形式が有効のため、ProvisionalAlternateEncodingを使用します
                    zip.ProvisionalAlternateEncoding = System.Text.Encoding.GetEncoding("shift_jis");
                    foreach (string sourceFilePath in sourceFilePaths)
                    {
                        zip.AddFile(sourceFilePath, "");
                    }

                    // 圧縮ファイルを保存
                    zip.Save(compressedFilePath);

                    // 成功時にtrueを返す
                    return true;
                }
            }
            catch (Exception ex)
            {
                // 失敗時にfalseを返す
                return false;
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
        /// <create>J.Chen 2024/08/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ArFileUpload(string filePath, string fileName, string mailID)
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

        #region 添付ファイル存在確認

        /// --------------------------------------------------
        /// <summary>
        /// 添付ファイル存在確認
        /// </summary>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>J.Chen 2024/08/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckAttachmentFileNonEmpty()
        {
            string[][] pairs = new string[][]
            {
                new string[] { this._attachGirenFullPath1, this._attachGirenFile1 },
                new string[] { this._attachGirenFullPath2, this._attachGirenFile2 },
                new string[] { this._attachGirenFullPath3, this._attachGirenFile3 },
                new string[] { this._attachGirenFullPath4, this._attachGirenFile4 },
                new string[] { this._attachGirenFullPath5, this._attachGirenFile5 },
                new string[] { this._attachRefFullPath1, this._attachRefFile1 },
                new string[] { this._attachRefFullPath2, this._attachRefFile2 }
            };
            // 対象の両方ともnullまたは空白でないかをチェック
            foreach (string[] pair in pairs)
            {
                if (!string.IsNullOrEmpty(pair[0]) && !string.IsNullOrEmpty(pair[1]))
                {
                    return true;
                }
            }


            return false;
        }

        #endregion
    }
}
