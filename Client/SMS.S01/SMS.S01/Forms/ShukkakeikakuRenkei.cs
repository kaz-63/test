using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using Commons;
using DSWUtil;
using GrapeCity.Win.ElTabelle;
using SMS.S01.Properties;
using SystemBase.Util;
using WsConnection;
using WsConnection.WebRefCommon;
using WsConnection.WebRefS01;

namespace SMS.S01.Forms
{

    /// --------------------------------------------------
    /// <summary>
    /// TAG登録連携
    /// </summary>
    /// <create>T.Nakata 2018/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ShukkakeikakuRenkei : SystemBase.Forms.CustomOrderForm
    {
        #region Enum
        /// --------------------------------------------------
        /// <summary>
        /// 画面の表示モード
        /// </summary>
        /// <create>T.Nakata 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DisplayMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 初期
            /// </summary>
            /// <create>T.Nakata 2018/11/07</create>
            /// <update></update>
            /// --------------------------------------------------
            Initialize,
            /// --------------------------------------------------
            /// <summary>
            /// 物件リスト設定後
            /// </summary>
            /// <create>T.Nakata 2018/11/07</create>
            /// <update></update>
            /// --------------------------------------------------
            SetBukkenList,
            /// --------------------------------------------------
            /// <summary>
            /// 便リスト設定後
            /// </summary>
            /// <create>T.Nakata 2018/11/07</create>
            /// <update></update>
            /// --------------------------------------------------
            SetShipList,
            /// --------------------------------------------------
            /// <summary>
            /// 検索後
            /// </summary>
            /// <create>T.Nakata 2018/11/07</create>
            /// <update></update>
            /// --------------------------------------------------
            EndSearch,
        }

        /// --------------------------------------------------
        /// <summary>
        /// TAG連携メールチェック結果
        /// </summary>
        /// <create>H.Tajimi 2019/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum CheckMailResult
        {
            OK = 0,
            Ignore = 1,
            Error = 2,
            Exception = 3,
        }
        #endregion

        #region 定数
        /// --------------------------------------------------
        /// <summary>
        /// チェックボックスのカラム番号
        /// </summary>
        /// <create>T.Nakata 2018/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_RENKEI = 0;
        /// --------------------------------------------------
        /// <summary>
        /// ECS Noのカラム番号
        /// </summary>
        /// <create>T.Nakata 2018/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ECSNO = 1;
        /// --------------------------------------------------
        /// <summary>
        /// AR Noのカラム番号
        /// </summary>
        /// <create>T.Nakata 2018/11/08</create>
        /// <update>J.Jeong 2024/08/15 数字変更</update>
        /// --------------------------------------------------
        private const int SHEET_COL_ARNO = 3;
        /// --------------------------------------------------
        /// <summary>
        /// 残数のカラム番号
        /// </summary>
        /// <create>T.Nakata 2018/11/08</create>
        /// <update>J.Jeong 2024/08/15 数字変更</update>
        /// --------------------------------------------------
        private const int SHEET_COL_ZAN_QTY = 8;
        /// --------------------------------------------------
        /// <summary>
        /// TAG登録可能数のカラム番号
        /// </summary>
        /// <create>T.Nakata 2018/11/08</create>
        /// <update>J.Jeong 2024/08/15 数字変更</update>
        /// --------------------------------------------------
        private const int SHEET_COL_TAG_TOUROKU_MAX = 9;
        /// --------------------------------------------------
        /// <summary>
        /// TAG登録数のカラム番号
        /// </summary>
        /// <create>T.Nakata 2018/11/08</create>
        /// <update>J.Jeong 2024/08/15 数字変更</update>
        /// --------------------------------------------------
        private const int SHEET_COL_TAG_TOUROKU = 10;
        /// --------------------------------------------------
        /// <summary>
        /// バージョンのカラム番号
        /// </summary>
        /// <create>T.Nakata 2018/11/08</create>
        /// <update>J.Jeong 2024/08/15 数字変更</update>>
        /// --------------------------------------------------
        private const int SHEET_COL_VERSION = 12;
        /// --------------------------------------------------
        /// <summary>
        /// 手配連携Noのカラム番号
        /// </summary>
        /// <create>T.Nakata 2018/11/08</create>
        /// <update>J.Jeong 2024/08/15 数字変更</update>
        /// --------------------------------------------------
        private const int SHEET_COL_TEHAI_RENKEI_NO = 13;
        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>T.Nakata 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 0;
        /// --------------------------------------------------
        /// <summary>
        /// チェックボックスONの値
        /// </summary>
        /// <create>T.Nakata 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int CHECK_ON = 1;
        /// --------------------------------------------------
        /// <summary>
        /// チェックボックスOFFの値
        /// </summary>
        /// <create>T.Nakata 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int CHECK_OFF = 0;
        #endregion

        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 退避データ
        /// </summary>
        /// <create>T.Nakata 2018/11/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtTagRenkeiListData = null;
        
        // ↓↓↓ M.Shimizu 2020/07/02 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）
        /// --------------------------------------------------
        /// <summary>
        /// メール送信判定
        /// </summary>
        /// <create>M.Shimizu 2020/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isMailSend = false;
        // ↑↑↑ M.Shimizu 2020/07/02 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）

        /// --------------------------------------------------
        /// <summary>
        /// ログインユーザのメールアドレス
        /// </summary>
        /// <create>H.Tajimi 2019/08/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _mailAddress = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// TAG連携メール用ユーザマスタ
        /// </summary>
        /// <create>H.Tajimi 2019/08/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtTagRenkeiUser = null;
        /// --------------------------------------------------
        /// <summary>
        /// インスタンス
        /// </summary>
        /// <create>R.Sumi 2022/02/28</create>
        /// --------------------------------------------------
        public static ShukkakeikakuRenkei ShukkakeikakuRenkeiInstance { get; set; }
        /// --------------------------------------------------
        /// <summary>
        /// 物件名
        /// </summary>
        /// <create>R.Sumi 2022/02/28</create>
        /// --------------------------------------------------
        public string BukkenNameText { get; set; }
        /// --------------------------------------------------
        /// <summary>
        /// EcsNo
        /// </summary>
        /// <create>R.Sumi 2022/02/28</create>
        /// --------------------------------------------------
        public string EcsNoText { get; set; }
        /// --------------------------------------------------
        /// <summary>
        /// 製番+CODE
        /// </summary>
        /// <create>R.Sumi 2022/02/28</create>
        /// --------------------------------------------------
        public string SeibanCodeText { get; set; }
        /// --------------------------------------------------
        /// <summary>
        /// 検索条件ARNo
        /// </summary>
        /// <create>N.Kawamura 2022/04/28</create>
        /// --------------------------------------------------
        public string txtARNo { get; set; }
        /// --------------------------------------------------
        /// <summary>
        /// 絞込条件ARNo
        /// </summary>
        /// <create>N.Kawamura 2022/04/28</create>
        /// --------------------------------------------------
        public string fcboARNo { get; set; }

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
        /// <create>T.Nakata 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public ShukkakeikakuRenkei(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>T.Nakata 2018/11/07</create>
        /// <update>H.Tajimi 2019/08/16 TAG連携メール通知対応</update>
        /// <update>2022/05/19 STEP14</update>
　　　　　　　/// <update>J.Jeong 2024/08/15 手配No.追加</update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // フォームの設定
                this.IsCloseQuestion = true;
                this.IsRunEditAfterClear = false;

                // [検索条件]出荷区分コンボボックス
                this.MakeCmbBox(this.cboShukkaFlag, SHUKKA_FLAG.GROUPCD);

                if (this.txtARNo == "" && this.fcboARNo == "")
                {
                    //出荷区分を「本体」に切り替える
                    this.cboShukkaFlag.SelectedValue = 0;
                }
                else if (this.txtARNo != null || this.fcboARNo != null)
                {
                    //出荷区分を「AR」に切り替える
                    this.cboShukkaFlag.SelectedValue = 1;
                }

                // TAG連携メール通知警告表示
                this.ShowInfoIfTagRenkeiErrorOccured();

                // フォームの状態を初期化
                this.ChangeMode(DisplayMode.Initialize);

                // シート設定
                this.InitializeSheet(shtTagRenkeiList);
                this.shtTagRenkeiList.KeepHighlighted = true;
                this.shtTagRenkeiList.RowHeaders.AllowResize = false;

                // シートのタイトルを設定
                shtTagRenkeiList.ColumnHeaders[0].Caption = "";
                shtTagRenkeiList.ColumnHeaders[1].Caption = Resources.ShukkakeikakuRenkei_ECSNo;
                shtTagRenkeiList.ColumnHeaders[2].Caption = Resources.ShukkakeikakuRenkei_TehaiNo;
                shtTagRenkeiList.ColumnHeaders[3].Caption = Resources.ShukkakeikakuRenkei_ARNo;
                shtTagRenkeiList.ColumnHeaders[4].Caption = Resources.ShukkakeikakuRenkei_Zuban;
                shtTagRenkeiList.ColumnHeaders[5].Caption = Resources.ShukkakeikakuRenkei_Hinmei_jp;
                shtTagRenkeiList.ColumnHeaders[6].Caption = Resources.ShukkakeikakuRenkei_Hinmei;
                shtTagRenkeiList.ColumnHeaders[7].Caption = Resources.ShukkakeikakuRenkei_Syukkasu;
                shtTagRenkeiList.ColumnHeaders[8].Caption = Resources.ShukkakeikakuRenkei_Zansu;
                shtTagRenkeiList.ColumnHeaders[9].Caption = Resources.ShukkakeikakuRenkei_TagMax;
                shtTagRenkeiList.ColumnHeaders[10].Caption = Resources.ShukkakeikakuRenkei_TagNum;
                shtTagRenkeiList.ColumnHeaders[11].Caption = Resources.ShukkakeikakuRenkei_SMeisaiNum;
                shtTagRenkeiList.ColumnHeaders[12].Caption = Resources.ShukkakeikakuRenkei_Version;
                shtTagRenkeiList.ColumnHeaders[13].Caption = Resources.ShukkakeikakuRenkei_TehaiRenkeiNo;
                shtTagRenkeiList.ColumnHeaders[14].Caption = Resources.ShukkakeikakuRenkei_TagAllow;

                // シートのEnterキー挙動変更
                shtTagRenkeiList.ShortCuts.Add(Keys.Enter, new[] { KeyAction.NextRow });
                shtTagRenkeiList.ShortCuts.Add(Keys.Enter | Keys.Shift, new[] { KeyAction.PrevRow });
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
        /// <create>H.Tsunamura 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカスを設定する。
                this.cboShukkaFlag.Focus();
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
        /// <create>T.Nakata 2018/11/08</create>
        /// <update>R.Sumi 2022/03/01</update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                if (this.BukkenNameText != null)
                {
                    this.cboBukkenmei.Text = this.BukkenNameText;
                }

                if (this.EcsNoText == null)
                {
                    this.txtECS.Clear();
                }
                else
                {
                    this.txtECS.Text = this.EcsNoText;
                }

                if (this.SeibanCodeText == null)
                {
                    this.txtSeiban.Clear();
                }
                else
                {
                    this.txtSeiban.Text = this.SeibanCodeText;
                }

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 全画面クリア処理
        /// </summary>
        /// <create>T.Nakata 2018/11/08</create>
        /// <update>2022/04/19 STEP14</update>
        /// --------------------------------------------------
        private void DisplayClearAll()
        {
            this.DisplayClear();

            try
            {
                if (0 < this.shtTagRenkeiList.MaxRows)
                {
                    this.shtTagRenkeiList.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtTagRenkeiList.TopLeft.Row);
                }
                this.shtTagRenkeiList.DataSource = null;
                this.shtTagRenkeiList.MaxRows = 0;
                this.shtTagRenkeiList.Enabled = false;

                this.ChangeMode(DisplayMode.Initialize);
                this.cboShukkaFlag.SelectedIndex = 0;
                this.cboShukkaFlag.Focus();
                if (this.cboBukkenmei.DataSource != null)
                {
                    if (this.cboBukkenmei.Items.Count > 0) this.cboBukkenmei.SelectedIndex = 0;
                }
                if (this.cboShip.DataSource != null)
                {
                    if (this.cboShip.Items.Count > 0) this.cboShip.SelectedIndex = 0;
                }
                this.txtNounyusakiCD.Text = string.Empty;

                this.txtECS.Clear();
                this.txtSeiban.Clear();

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
        /// <create>H.Tsunamura 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                return ret;
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
        /// <create>T.Nakata 2018/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                // コントロールの値が入っているかどうかのチェック
                if (this.cboShukkaFlag.SelectedIndex == -1)
                {
                    // 出荷区分一覧から出荷区分を選択して下さい。
                    this.ShowMessage("S0100023001");
                    this.cboShukkaFlag.Focus();
                    return false;
                }
                if (this.cboBukkenmei.SelectedIndex == -1)
                {
                    // 物件名一覧から物件を選択して下さい。
                    this.ShowMessage("S0100023002");
                    this.cboBukkenmei.Focus();
                    return false;
                }
                if (this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1)
                {
                    if (this.cboShip.SelectedIndex == -1)
                    {
                        // 便一覧から便を選択して下さい。
                        this.ShowMessage("S0100023003");
                        this.cboShip.Focus();
                        return false;
                    }
                }
                else
                {
                    // ARの場合
                    if (string.IsNullOrEmpty(this.txtNounyusakiCD.Text))
                    {
                        // 該当納入先がありません。AR情報の「ITEM新規登録」で登録してください。
                        this.ShowMessage("K0300010015");
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

        #region 検索処理

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理制御部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>H.Tsunamura 2010/06/28</create>
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
        /// <create>T.Nakata 2018/11/08</create>
        /// <update>2022/05/25 STEP14</update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                CondS01 condS01 = new CondS01(this.UserInfo);
                WsConnection.ConnS01 connS01 = new ConnS01();
                DataSet ds;

                // 納入マスタより有償/無償情報を取得
                // 出荷先情報の取得を追加(2022/05/25)
                if (this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1)
                {
                    var item = this.cboShip.SelectedItem as DataRowView;
                    condS01.EstimateFlag = item == null ? null : ComFunc.GetFld(item.Row, Def_M_NONYUSAKI.ESTIMATE_FLAG, null);
                    condS01.ShipTo = item == null ? null : ComFunc.GetFld(item.Row, Def_M_NONYUSAKI.SHIP_TO, null);

                    // 本体なのに出荷先登録されていない場合
                    if (string.IsNullOrEmpty(condS01.ShipTo))
                    {
                        // TAG登録可能な手配明細はありません。
                        this.ShowMessage("S0100023004");
                        return false;
                    }
                }
                else
                {
                    condS01.EstimateFlag = null;
                    condS01.ShipTo = null;
                }

                condS01.ShukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
                condS01.BukkenNO = this.cboBukkenmei.SelectedValue.ToString();
                condS01.EcsNoList = this.txtECS.Text;
                condS01.SibanCodeList = this.txtSeiban.Text;

                ds = connS01.GetTagRenkeiList(condS01);
                if (!ComFunc.IsExistsData(ds, Def_T_TEHAI_MEISAI.Name))
                {
                    // TAG登録可能な手配明細はありません。
                    this.ShowMessage("S0100023004");
                    return false;
                }

                this._dtTagRenkeiListData = ds.Tables[Def_T_TEHAI_MEISAI.Name].Copy();
                this.shtTagRenkeiList.DataSource = ds.Tables[Def_T_TEHAI_MEISAI.Name];
                // 最も左上に表示されているセルの設定
                if (0 < this.shtTagRenkeiList.MaxRows)
                {
                    this.shtTagRenkeiList.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtTagRenkeiList.TopLeft.Row);
                }
                this.ChangeMode(DisplayMode.EndSearch);

                // 複数の出荷先チェック
                DataTable dt = ds.Tables[Def_T_TEHAI_MEISAI.Name];
                var syukkaList = dt.AsEnumerable().Select(p => ComFunc.GetFld(p, Def_T_TEHAI_MEISAI.SYUKKA_SAKI).Trim()).Distinct().OrderBy(p => p).ToList();
                if (syukkaList.Count > 1)
                {
                    // 一度の連携で複数の出荷先が混在している
                    this.ShowMessage("S0100023011");
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

        #region イベント

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F1ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/11/08</create>
        /// <update>D.Okumura 2019/01/25 M-Noへ連携されない不具合を修正</update>
        /// <update>K.Tsutsumi 2019/02/09 納入先のチェックを追加</update>
        /// <update>H.Tajimi 2019/08/16 TAG連携メール通知対応</update>
        /// <update>J.Chen 2022/12/21 図番型式2追加</update>
        /// <update>J.Jeong 2024/08/15 手配No.追加</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F01Button_Click(sender, e);
            try
            {
                this.ClearMessage();

                string errMsgID;
                string[] args;
                // メール送信用入力チェック
                var renkeiResult = this.CheckTagRenkeiMail(false, out errMsgID, out args);
                if (!string.IsNullOrEmpty(errMsgID))
                {
                    this.ShowMessage(errMsgID, args);
                    return;
                }
                else
                {
                    if (renkeiResult == CheckMailResult.Exception)
                    {
                        return;
                    }
                }

                DataTable dtTagRenkeiList = new DataTable(Def_T_SHUKKA_MEISAI.Name);

                // TAG連携リストのカラム設定
                // 連携先は出荷明細の為カラム名は出荷明細を基本とする
                //----- 手配明細 -----
                dtTagRenkeiList.Columns.Add(Def_T_SHUKKA_MEISAI.TAG_NO);             // TAGNO.(TAG入力側でのDB処理にて使用)
                dtTagRenkeiList.Columns.Add(Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO);    // 連携No.
                dtTagRenkeiList.Columns.Add(Def_T_SHUKKA_MEISAI.ZUMEN_OIBAN);        // 図面追番
                dtTagRenkeiList.Columns.Add(Def_T_SHUKKA_MEISAI.FLOOR);              // Floor
                dtTagRenkeiList.Columns.Add(Def_T_SHUKKA_MEISAI.HINMEI_JP);          // 品名(和文)
                dtTagRenkeiList.Columns.Add(Def_T_SHUKKA_MEISAI.HINMEI);             // 品名
                dtTagRenkeiList.Columns.Add(Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI);     // 図番/型式
                dtTagRenkeiList.Columns.Add(Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI2);    // 図番/型式2
                dtTagRenkeiList.Columns.Add(Def_T_SHUKKA_MEISAI.FREE1);              // Free1
                dtTagRenkeiList.Columns.Add(Def_T_SHUKKA_MEISAI.FREE2);              // Free2
                dtTagRenkeiList.Columns.Add(Def_T_SHUKKA_MEISAI.BIKO);               // NOTE⇒BIKO
                dtTagRenkeiList.Columns.Add(Def_T_SHUKKA_MEISAI.ST_NO);              // ST No
                dtTagRenkeiList.Columns.Add(Def_T_SHUKKA_MEISAI.M_NO);               // ST No⇒M No
                dtTagRenkeiList.Columns.Add(Def_T_SHUKKA_MEISAI.VERSION, Type.GetType("System.DateTime")); // バージョン
                //----- 技連マスタ -----
                dtTagRenkeiList.Columns.Add(Def_T_SHUKKA_MEISAI.AREA);      // ECS No⇒Area
                dtTagRenkeiList.Columns.Add(Def_T_SHUKKA_MEISAI.SEIBAN);    // 製番
                dtTagRenkeiList.Columns.Add(Def_T_SHUKKA_MEISAI.CODE);      // CODE
                dtTagRenkeiList.Columns.Add(Def_T_SHUKKA_MEISAI.KISHU);     // 機種
                dtTagRenkeiList.Columns.Add(Def_T_SHUKKA_MEISAI.AR_NO);     // ARNo
                //----- その他 -----
                dtTagRenkeiList.Columns.Add(Def_T_SHUKKA_MEISAI.NUM, Type.GetType("System.Int32")); // 数量
                dtTagRenkeiList.Columns.Add(Def_T_TEHAI_MEISAI.SYUKKA_SAKI);                        // 出荷先
                dtTagRenkeiList.Columns.Add(Def_T_TEHAI_SKS_WORK.TEHAI_NO);

                for (int i = 0; i < this.shtTagRenkeiList.MaxRows; i++)
                {
                    // TAG登録：空白チェック
                    if ((this.shtTagRenkeiList.Columns[SHEET_COL_TAG_TOUROKU].ValueBlock[i] == null)
                        || string.IsNullOrEmpty(this.shtTagRenkeiList.Columns[SHEET_COL_TAG_TOUROKU].ValueBlock[i].ToString()))
                    {
                        continue;
                    }
                    // TAG登録：入力チェック
                    int TagNum = Convert.ToInt32(this.shtTagRenkeiList.Columns[SHEET_COL_TAG_TOUROKU].ValueBlock[i]);
                    int TagMax = Convert.ToInt32(this.shtTagRenkeiList.Columns[SHEET_COL_TAG_TOUROKU_MAX].ValueBlock[i]);
                    int ZanNum = Convert.ToInt32(this.shtTagRenkeiList.Columns[SHEET_COL_ZAN_QTY].ValueBlock[i]);
                    string ARNoStr = this.shtTagRenkeiList.Columns[SHEET_COL_ARNO].TextBlock[i];
                    ARNoStr = string.IsNullOrEmpty(ARNoStr) ? null : ARNoStr.Trim();
                    // TAG登録：1未満の場合はスキップ
                    if (TagNum < 1)
                        continue;
                    if (this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1)
                    {
                        if (TagMax < TagNum)
                        {
                            // TAG登録数はTAG登録可能数以下で入力して下さい。
                            this.ShowMessage("S0100023007");
                            this.shtTagRenkeiList.ActivePosition = new GrapeCity.Win.ElTabelle.Position(SHEET_COL_TAG_TOUROKU, i);
                            return;
                        }
                    }
                    else
                    {
                        if (ZanNum < TagNum)
                        {
                            // TAG登録数は残数以下で入力して下さい。
                            this.ShowMessage("S0100023008");
                            this.shtTagRenkeiList.ActivePosition = new GrapeCity.Win.ElTabelle.Position(SHEET_COL_TAG_TOUROKU, i);
                            return;
                        }
                    }
                    DataRow dr = dtTagRenkeiList.NewRow();
                    dr[Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO] = ComFunc.GetFldObject(this._dtTagRenkeiListData, i, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO);
                    dr[Def_T_SHUKKA_MEISAI.ZUMEN_OIBAN] = ComFunc.GetFldObject(this._dtTagRenkeiListData, i, Def_T_TEHAI_MEISAI.ZUMEN_OIBAN);
                    dr[Def_T_SHUKKA_MEISAI.FLOOR] = ComFunc.GetFldObject(this._dtTagRenkeiListData, i, Def_T_TEHAI_MEISAI.FLOOR);
                    dr[Def_T_SHUKKA_MEISAI.HINMEI_JP] = ComFunc.GetFldObject(this._dtTagRenkeiListData, i, Def_T_TEHAI_MEISAI.HINMEI_JP);
                    dr[Def_T_SHUKKA_MEISAI.HINMEI] = ComFunc.GetFldObject(this._dtTagRenkeiListData, i, Def_T_TEHAI_MEISAI.HINMEI);
                    dr[Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI] = ComFunc.GetFldObject(this._dtTagRenkeiListData, i, Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI);
                    dr[Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI2] = ComFunc.GetFldObject(this._dtTagRenkeiListData, i, Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI2);
                    dr[Def_T_SHUKKA_MEISAI.FREE1] = ComFunc.GetFldObject(this._dtTagRenkeiListData, i, Def_T_TEHAI_MEISAI.FREE1);
                    dr[Def_T_SHUKKA_MEISAI.FREE2] = ComFunc.GetFldObject(this._dtTagRenkeiListData, i, Def_T_TEHAI_MEISAI.FREE2);
                    dr[Def_T_SHUKKA_MEISAI.BIKO] = ComFunc.GetFldObject(this._dtTagRenkeiListData, i, Def_T_TEHAI_MEISAI.NOTE);
                    var hinmeiInv = ComFunc.GetFldObject(this._dtTagRenkeiListData, i, Def_T_TEHAI_MEISAI.HINMEI_INV);
                    var stNo = ComFunc.GetFldObject(this._dtTagRenkeiListData, i, Def_T_TEHAI_MEISAI.ST_NO);
                    dr[Def_T_SHUKKA_MEISAI.ST_NO] = stNo;
                    dr[Def_T_SHUKKA_MEISAI.M_NO] = string.IsNullOrEmpty(hinmeiInv as string) ? stNo : hinmeiInv;
                    dr[Def_T_SHUKKA_MEISAI.VERSION] = ComFunc.GetFldObject(this._dtTagRenkeiListData, i, Def_T_TEHAI_MEISAI.VERSION);
                    dr[Def_T_SHUKKA_MEISAI.AREA] = ComFunc.GetFldObject(this._dtTagRenkeiListData, i, Def_M_ECS.ECS_NO);
                    dr[Def_T_SHUKKA_MEISAI.SEIBAN] = ComFunc.GetFldObject(this._dtTagRenkeiListData, i, Def_M_ECS.SEIBAN);
                    dr[Def_T_SHUKKA_MEISAI.CODE] = ComFunc.GetFldObject(this._dtTagRenkeiListData, i, Def_M_ECS.CODE);
                    dr[Def_T_SHUKKA_MEISAI.KISHU] = ComFunc.GetFldObject(this._dtTagRenkeiListData, i, Def_M_ECS.KISHU);
                    dr[Def_T_SHUKKA_MEISAI.AR_NO] = ComFunc.GetFldObject(this._dtTagRenkeiListData, i, Def_M_ECS.AR_NO);
                    dr[Def_T_SHUKKA_MEISAI.NUM] = TagNum;
                    dr[Def_T_TEHAI_MEISAI.SYUKKA_SAKI] = ComFunc.GetFldObject(this._dtTagRenkeiListData, i, Def_T_TEHAI_MEISAI.SYUKKA_SAKI);
                    dr[Def_T_TEHAI_SKS_WORK.TEHAI_NO] = ComFunc.GetFldObject(this._dtTagRenkeiListData, i, Def_T_TEHAI_SKS_WORK.TEHAI_NO);
                    dtTagRenkeiList.Rows.Add(dr);
                }

                // 連携対象データ出荷先確認
                var syukkaList = dtTagRenkeiList.AsEnumerable().Select(p => ComFunc.GetFld(p, Def_T_TEHAI_MEISAI.SYUKKA_SAKI).Trim()).Distinct().OrderBy(p => p).ToList();
                if (syukkaList.Count > 1)
                {
                    // 一度の連携で複数の出荷先が混在している
                    this.ShowMessage("S0100023011");
                    return;
                }

                // 連携対象データ有無確認
                if (dtTagRenkeiList.Rows.Count < 1)
                {
                    // TAG登録数が入力されていません。
                    this.ShowMessage("S0100023006");
                    return;
                }

                // 納入先チェック
                WsConnection.WebRefMaster.CondNonyusaki condNonyusaki = new WsConnection.WebRefMaster.CondNonyusaki(this.UserInfo);
                condNonyusaki.ShukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
                condNonyusaki.NonyusakiCD = this.txtNounyusakiCD.Text;

                WsConnection.ConnMaster connMaster = new WsConnection.ConnMaster();
                var dsMaster = connMaster.GetNonyusaki(condNonyusaki);
                if (!UtilData.ExistsData(dsMaster, Def_M_NONYUSAKI.Name))
                {
                    // 選択中の便は別の端末で削除されました。
                    this.ShowMessage("S0100023009");
                    return;
                }

                CondS01 condS01 = new CondS01(this.UserInfo);
                WsConnection.ConnS01 connS01 = new ConnS01();

                DataSet ds = new DataSet();
                ds.Tables.Add(dtTagRenkeiList.Copy());

                if (!connS01.UpdTehaimeisaiVersionData(condS01, ds, out errMsgID, out args))
                {
                    this.ShowMessage(errMsgID);
                    return;
                }

                this._dtTagRenkeiListData = connS01.GetAndLockTagRenkeiList(condS01, ds, false);//データテーブルを更新

                string ShipName = string.Empty;
                if (this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1)
                {
                    this.cboShipData.SelectedIndex = this.cboShip.SelectedIndex;
                    ShipName = this.cboShipData.Text;
                }
                TagRenkeiData TagRenkeiData = new TagRenkeiData(dtTagRenkeiList.Copy(),
                                                                this.cboShukkaFlag.SelectedValue.ToString(), ShipName,
                                                                this.cboBukkenmei.Text, this.txtNounyusakiCD.Text);

                //TAG登録・変更画面を新規モードで開く
                ShukkaKeikakuMeisai ShukkaKeikakuMeisaiForm = new ShukkaKeikakuMeisai(this.UserInfo,
                                                                      ComDefine.FLD_CATEGORY_ID_S0100020,
                                                                      ComDefine.FLD_MENU_ID_S0100020,
                                                                      ComDefine.TITLE_S0100020, TagRenkeiData);
                DialogResult d_retVal = ShukkaKeikakuMeisaiForm.ShowDialog();
                if (ShukkaKeikakuMeisaiForm.TagRenkeiResult == DialogResult.OK)
                {
                    this.RunMail(ShukkaKeikakuMeisaiForm.AssociatedData);
                }

                // グリッドクリア
                this.SheetClear();
                // 再検索
                base.RunSearch();
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
        /// <create>T.Nakata 2018/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F04Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F04Button_Click(sender, e);
            try
            {
                this.ClearMessage();
                // チェック数確認
                if (this.RenkeiCheckCount() <= 0)
                {
                    // 選択されている手配明細がありません。
                    this.ShowMessage("S0100023005");
                    return;
                }

                this.shtTagRenkeiList.Redraw = false;
                for (int i = 0; i < this.shtTagRenkeiList.MaxRows; i++)
                {
                    if ((this.shtTagRenkeiList.Columns[SHEET_COL_RENKEI].ValueBlock[i] != null)
                      && (Convert.ToInt32(this.shtTagRenkeiList.Columns[SHEET_COL_RENKEI].ValueBlock[i]) == CHECK_ON))
                    {
                        this.shtTagRenkeiList.CellRange = new GrapeCity.Win.ElTabelle.Range(SHEET_COL_TAG_TOUROKU, i, SHEET_COL_TAG_TOUROKU, i);
                        if (this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1)
                            this.shtTagRenkeiList.CellValue = this.shtTagRenkeiList.Columns[SHEET_COL_TAG_TOUROKU_MAX].ValueBlock[i].ToString();
                        else
                            this.shtTagRenkeiList.CellValue = this.shtTagRenkeiList.Columns[SHEET_COL_ZAN_QTY].ValueBlock[i].ToString();
                    }
                }
                this.shtTagRenkeiList.Redraw = true;
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
        /// <create>T.Nakata 2018/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                this.ClearMessage();
                if (this.ShowMessage("A9999999053") != DialogResult.OK)
                {
                    return;
                }
                // グリッド
                this.SheetClear();

                this.ChangeMode(DisplayMode.Initialize);
                this.btnDisp.Enabled = true;
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
        /// <create>T.Nakata 2018/11/08</create>
        /// <update>2022/04/19 STEP14</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                // クリアしてもいいですか？ダイアログ表示
                if (this.ShowMessage("A9999999001") != DialogResult.OK)
                {
                    return;
                }
                this.BukkenNameText = null;
                this.DisplayClearAll();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        # region ボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// 開始クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDisp_Click(object sender, EventArgs e)
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

        /// --------------------------------------------------
        /// <summary>
        /// 全選択クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnAllCheck_Click(object sender, EventArgs e)
        {
            this.shtTagRenkeiList.Redraw = false;

            this.SetAllRenkeiCheck(true);

            this.shtTagRenkeiList.Redraw = true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 全選択解除クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnAllNotCheck_Click(object sender, EventArgs e)
        {
            this.shtTagRenkeiList.Redraw = false;

            this.SetAllRenkeiCheck(false);

            this.shtTagRenkeiList.Redraw = true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 範囲選択されたグリッドをチェックします。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnRangeCheck_Click(object sender, EventArgs e)
        {
            this.shtTagRenkeiList.Redraw = false;

            this.SetRangeRenkeiCheck(true);

            this.shtTagRenkeiList.Redraw = true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 範囲選択されたグリッドからチェックをはずします。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnRangeNotCheck_Click(object sender, EventArgs e)
        {
            this.shtTagRenkeiList.Redraw = false;

            this.SetRangeRenkeiCheck(false);

            this.shtTagRenkeiList.Redraw = true;
        }

        # endregion

        #region シートのクリア

        /// --------------------------------------------------
        /// <summary>
        /// シートのクリア
        /// </summary>
        /// <create>T.Nakata 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            this.shtTagRenkeiList.Redraw = false;
            // 最も左上に表示されているセルの設定
            if (0 < this.shtTagRenkeiList.MaxRows)
            {
                this.shtTagRenkeiList.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtTagRenkeiList.TopLeft.Row);
            }
            this.shtTagRenkeiList.DataSource = null;
            this.shtTagRenkeiList.MaxRows = 0;
            this.shtTagRenkeiList.Enabled = false;
            this.shtTagRenkeiList.Redraw = true;
        }

        #endregion

        #region コンボボックス選択処理
        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/11/07</create>
        /// <update>2022/04/19 STEP14</update>
        /// --------------------------------------------------
        private void cboShukkaFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ChangeMode(DisplayMode.Initialize);
            this.cboBukkenmei.Enabled = false;
            if (this.cboBukkenmei.DataSource != null)
            {
                this.cboBukkenmei.DataSource = null;
                this.cboBukkenmei.Items.Add(new object());
                this.cboBukkenmei.Items.Clear();
                this.txtNounyusakiCD.Text = string.Empty;
            } 
            this.btnDisp.Enabled = false;
            this.ChangeShukkaFlag();

            if (this.BukkenNameText != null)
            {
                String str = this.cboBukkenmei.Text;
                this.cboBukkenmei.Text = this.BukkenNameText;
                if (this.cboBukkenmei.SelectedIndex == 0)
                {
                    this.cboBukkenmei.SelectedIndex = 1;
                    this.cboBukkenmei.Text = str;
                }
            }

            // 出荷区分がARなら便は無効化
            if (this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.AR_VALUE1)
            {
                this.cboShip.Enabled = false;
                if (this.cboShip.DataSource != null)
                {
                    this.cboShip.DataSource = null;
                    this.cboShip.Items.Add(new object());
                    this.cboShip.Items.Clear();
                    this.cboShipData.DataSource = null;
                    this.cboShipData.Items.Add(new object());
                    this.cboShipData.Items.Clear();
                }
                this.btnDisp.Enabled = true;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 物件選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboBukkenmei_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1)
            {
                this.cboShip.Enabled = false;
                if (this.cboShip.DataSource != null)
                {
                    this.cboShip.DataSource = null;
                    this.cboShip.Items.Add(new object());
                    this.cboShip.Items.Clear();
                    this.cboShipData.DataSource = null;
                    this.cboShipData.Items.Add(new object());
                    this.cboShipData.Items.Clear();
                }
                this.btnDisp.Enabled = false;
            }
            this.ChangeBukken();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 便選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboShip_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.AR_VALUE1)
            {
                this.txtNounyusakiCD.Text = string.Empty;
                return;
            }
            this.cboShipData.SelectedIndex = this.cboShip.SelectedIndex;
            this.txtNounyusakiCD.Text = string.Empty;
            if (this.cboShipData.SelectedValue != null)
            {
                this.txtNounyusakiCD.Text = this.cboShipData.SelectedValue.ToString();
            }
        }

        #endregion

        #endregion

        #region 出荷区分切替

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分の切替
        /// </summary>
        /// <create>T.Nakata 2018/11/07</create>
        /// <update>2022/05/19 STEP14</update>
        /// --------------------------------------------------
        private void ChangeShukkaFlag()
        {
            try
            {
                if (this.cboShukkaFlag.DataSource == null) return;

                CondS01 condS01 = new CondS01(this.UserInfo);
                WsConnection.ConnS01 connS01 = new ConnS01();
                condS01.ShukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
                DataSet ds = connS01.GetBukkenName(condS01);

                if (UtilData.ExistsData(ds, Def_M_BUKKEN.Name))
                {
                    this.cboBukkenmei.DisplayMember = Def_M_BUKKEN.BUKKEN_NAME;
                    this.cboBukkenmei.ValueMember = Def_M_BUKKEN.BUKKEN_NO;
                    this.cboBukkenmei.DataSource = ds.Tables[Def_M_BUKKEN.Name];
                    this.cboBukkenmei.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 物件切替

        /// --------------------------------------------------
        /// <summary>
        /// 物件の切替
        /// </summary>
        /// <create>T.Nakata 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeBukken()
        {
            try
            {
                if (this.cboShukkaFlag.DataSource == null
                    || this.cboBukkenmei.DataSource == null) return;

                CondS01 condS01 = new CondS01(this.UserInfo);
                WsConnection.ConnS01 connS01 = new ConnS01();
                condS01.ShukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
                condS01.BukkenNO = this.cboBukkenmei.SelectedValue.ToString();
                DataSet ds = connS01.GetShipList(condS01);

                if (UtilData.ExistsData(ds, Def_M_NONYUSAKI.Name))
                {
                    if (this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1)
                    {
                        this.cboShipData.DisplayMember = Def_M_NONYUSAKI.SHIP;
                        this.cboShipData.ValueMember = Def_M_NONYUSAKI.NONYUSAKI_CD;
                        this.cboShipData.DataSource = ds.Tables[Def_M_NONYUSAKI.Name].Copy();

                        this.cboShip.DisplayMember = "DISP_" + Def_M_NONYUSAKI.SHIP;
                        this.cboShip.ValueMember = Def_M_NONYUSAKI.NONYUSAKI_CD;
                        this.cboShip.DataSource = ds.Tables[Def_M_NONYUSAKI.Name].Copy();
                        this.cboShip.Enabled = true;

                        this.btnDisp.Enabled = true;
                    }
                    else
                    {
                        this.txtNounyusakiCD.Text = ComFunc.GetFld(ds, Def_M_NONYUSAKI.Name, 0, Def_M_NONYUSAKI.NONYUSAKI_CD);
                    }
                }
                else
                {
                    this.txtNounyusakiCD.Text = string.Empty;
                }

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region RENKEI列のチェックボックスON/OFF制御用（フィルタリングを考慮）

        /// --------------------------------------------------
        /// <summary>
        /// 全選択・全選択解除時のRENKEI列チェックボックスON/OFF設定
        /// </summary>
        /// <param name="isChecked">RENKEI列チェックボックスをONするかどうか</param>
        /// <create>T.Nakata 2018/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetAllRenkeiCheck(bool isChecked)
        {
            var ranges = this.GetValidRanges(new GrapeCity.Win.ElTabelle.Range[] { new GrapeCity.Win.ElTabelle.Range(SHEET_COL_RENKEI, 0, SHEET_COL_RENKEI, this.shtTagRenkeiList.MaxRows - 1) });
            this.SetCellRangeValueForCheckBox(ranges, isChecked);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 範囲選択・範囲選択解除時のRENKEI列チェックボックスON/OFF設定
        /// </summary>
        /// <param name="isChecked">RENKEI列チェックボックスをONするかどうか</param>
        /// <create>T.Nakata 2018/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetRangeRenkeiCheck(bool isChecked)
        {
            var ranges = this.GetValidRanges(this.shtTagRenkeiList.GetBlocks(GrapeCity.Win.ElTabelle.BlocksType.Selection));
            this.SetCellRangeValueForCheckBox(ranges, isChecked);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定Rangeのチェックボックス値設定
        /// </summary>
        /// <param name="ranges">Range配列</param>
        /// <param name="isCheck">チェックボックスをONするかどうか</param>
        /// <create>H.Tajimi 2015/11/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetCellRangeValueForCheckBox(GrapeCity.Win.ElTabelle.Range[] ranges, bool isChecked)
        {
            foreach (var range in ranges)
            {
                this.shtTagRenkeiList.CellRange = range;
                this.shtTagRenkeiList.CellValue = isChecked ? CHECK_ON : CHECK_OFF;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定Rangeで有効となっているRange配列を取得
        /// </summary>
        /// <param name="ranges">Range配列</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2015/11/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private GrapeCity.Win.ElTabelle.Range[] GetValidRanges(GrapeCity.Win.ElTabelle.Range[] ranges)
        {
            var lstRanges = new List<GrapeCity.Win.ElTabelle.Range>();
            foreach (var range in ranges)
            {
                for (int rowIndex = range.TopRow; rowIndex <= range.BottomRow; rowIndex++)
                {
                    if (IsValidRow(rowIndex))
                    {
                        lstRanges.Add(new Range(0, rowIndex, 0, rowIndex));
                    }
                }
            }
            return lstRanges.ToArray();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定行が有効かどうか
        /// </summary>
        /// <remarks>
        /// El TablledのAutoFilterは行の高さを0にすることでフィルタリングを
        /// 実現しているので、行の高さでフィルタリングされているかどうか判定
        /// </remarks>
        /// <param name="rowIndex">行インデックス</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2015/11/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool IsValidRow(int rowIndex)
        {
            if (this.shtTagRenkeiList.MaxRows - 1 < rowIndex)
            {
                return false;
            }

            if (this.shtTagRenkeiList.Rows[rowIndex].Height < 1)
            {
                return false;
            }

            return true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// チェックがついている数を取得
        /// </summary>
        /// <returns></returns>
        /// <create>T.Nakata 2018/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private int RenkeiCheckCount()
        {
            if (this.shtTagRenkeiList.MaxRows <= 0) return 0;
            int retVal = 0;
            for (int i = 0; i < this.shtTagRenkeiList.MaxRows; i++)
            {
                if (this.shtTagRenkeiList.Columns[SHEET_COL_RENKEI].ValueBlock[i] != null)
                {
                    if (Convert.ToInt32(this.shtTagRenkeiList.Columns[SHEET_COL_RENKEI].ValueBlock[i]) == CHECK_ON) retVal++;
                }
            }
            return retVal;
        }

        #endregion

        #region モード切り替え

        /// --------------------------------------------------
        /// <summary>
        /// モード切替
        /// </summary>
        /// <param name="mode">画面の表示モード</param>
        /// <create>T.Nakata 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeMode(DisplayMode mode)
        {
            try
            {
                switch (mode)
                {
                    case DisplayMode.Initialize:
                        // ----- 初期 -----
                        this.grpSearch.Enabled = true;
                        this.btnAllCheck.Enabled = false;
                        this.btnAllNotCheck.Enabled = false;
                        this.btnRangeCheck.Enabled = false;
                        this.btnRangeNotCheck.Enabled = false;
                        this.fbrFunction.F01Button.Enabled = false;
                        this.fbrFunction.F04Button.Enabled = false;
                        this.fbrFunction.F06Button.Enabled = false;
                        this.shtTagRenkeiList.Enabled = false;
                        break;
                    case DisplayMode.SetBukkenList:
                        // ----- 物件リスト設定後 -----
                        break;
                    case DisplayMode.SetShipList:
                        // ----- 便リスト設定後 -----
                        break;
                    case DisplayMode.EndSearch:
                        // ----- 検索後 -----
                        this.grpSearch.Enabled = false;
                        this.btnDisp.Enabled = false;
                        this.btnAllCheck.Enabled = true;
                        this.btnAllNotCheck.Enabled = true;
                        this.btnRangeCheck.Enabled = true;
                        this.btnRangeNotCheck.Enabled = true;
                        this.fbrFunction.F01Button.Enabled = true;
                        this.fbrFunction.F04Button.Enabled = true;
                        this.fbrFunction.F06Button.Enabled = true;
                        this.shtTagRenkeiList.Enabled = true;
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

        #region メール関連

        #region メール送信制御部

        /// --------------------------------------------------
        /// <summary>
        /// メール送信制御部
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RunMail(DataTable dt)
        {
            var ret = false;
            try
            {
                // 入力チェック
                var renkeiResult = this.CheckInputMail();
                if (renkeiResult == CheckMailResult.OK)
                {
                    ret = this.RunMailExec(dt);
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

        #region メール送信実行部

        /// --------------------------------------------------
        /// <summary>
        /// メール送信実行部
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RunMailExec(DataTable dt)
        {
            try
            {
                var conn = new ConnS01();
                var cond = new CondS01(this.UserInfo);
                cond.UpdateUserID = this.UserInfo.UserID;
                cond.UpdateUserName = this.UserInfo.UserName;

                string errMsgID;
                string[] args;

                // メールデータ作成
                var ds = new DataSet();
                ds.Merge(this.CreateMailData(dt));

                // DB更新
                if (!conn.InsTagRenkeiMail(cond, ds, out errMsgID, out args))
                {
                    this.ShowMessage(errMsgID, args);
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

        #region メール送信用入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// メール送信用入力チェック
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/08/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private CheckMailResult CheckInputMail()
        {
            CheckMailResult ret = CheckMailResult.Error;
            try
            {
                string errMsgID;
                string[] errArgs;

                ret = this.CheckTagRenkeiMail(true, out errMsgID, out errArgs);
                if (!string.IsNullOrEmpty(errMsgID))
                {
                    this.ShowMessage(errMsgID, errArgs);
                    return CheckMailResult.Error;
                }
                return ret;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return CheckMailResult.Exception;
            }
        }
        
        #endregion

        #region メール登録用データ作成

        /// --------------------------------------------------
        /// <summary>
        /// メール登録用データ作成
        /// </summary>
        /// <param name="dt">データ</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/08/17</create>
        /// <update>M.Shimizu 2020/06/01 EFA_SMS-91 メール通知のWebService化</update>
        /// <update>M.Shimizu 2020/07/02 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）</update>
        /// --------------------------------------------------
        private DataTable CreateMailData(DataTable dt)
        {
            // ↓↓↓ M.Shimizu 2020/07/02 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）
            // タグ連携イベントが'1'でない場合、メール送信データは作成しない
            // if (string.IsNullOrEmpty(this._templateFolder))
            if (!this._isMailSend)
            {
                return null;
            }
            // ↑↑↑ M.Shimizu 2020/07/02 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）

            // ↓↓↓ M.Shimizu 2020/06/01 EFA_SMS-91 メール通知のWebService化
            // メールテンプレートの内容を取得
            ConnAttachFile attachFile = new ConnAttachFile(this.UserInfo.Language);
            string title = attachFile.GetMailTemplate(GetTitleFileName());
            string naiyo = attachFile.GetMailTemplate(GetNaiyoFileName());
            // ↑↑↑ M.Shimizu 2020/06/01 EFA_SMS-91 メール通知のWebService化

            var dtMail = ComFunc.GetSchemeMail();
            var dr = dtMail.NewRow();
            dr.SetField<object>(Def_T_MAIL.MAIL_SEND, this._mailAddress);
            dr.SetField<object>(Def_T_MAIL.MAIL_SEND_DISPLAY, this.UserInfo.UserName);
            dr.SetField<object>(Def_T_MAIL.MAIL_TO, ComFunc.GetMailUser(this._dtTagRenkeiUser, Def_M_USER.MAIL_ADDRESS));
            dr.SetField<object>(Def_T_MAIL.MAIL_TO_DISPLAY, ComFunc.GetMailUser(this._dtTagRenkeiUser, Def_M_USER.USER_NAME));
            dr.SetField<object>(Def_T_MAIL.MAIL_CC, DBNull.Value);
            dr.SetField<object>(Def_T_MAIL.MAIL_CC_DISPLAY, DBNull.Value);
            dr.SetField<object>(Def_T_MAIL.MAIL_BCC, DBNull.Value);
            dr.SetField<object>(Def_T_MAIL.MAIL_BCC_DISPLAY, DBNull.Value);
            // ↓↓↓ M.Shimizu 2020/06/01 EFA_SMS-91 メール通知のWebService化
            dr.SetField<object>(Def_T_MAIL.TITLE, this.ReplaceMailContents(title, dt));
            dr.SetField<object>(Def_T_MAIL.NAIYO, this.ReplaceMailContents(naiyo, dt));
            // ↑↑↑ M.Shimizu 2020/06/01 EFA_SMS-91 メール通知のWebService化
            dr.SetField<object>(Def_T_MAIL.MAIL_STATUS, MAIL_STATUS.MI_VALUE1);
            dr.SetField<object>(Def_T_MAIL.RETRY_COUNT, 0);
            dtMail.Rows.Add(dr);

            return dtMail;
        }
        
        #endregion

        #region メールテンプレートファイル名の取得

        /// --------------------------------------------------
        /// <summary>
        /// タイトルのメールテンプレートファイル名を取得します
        /// </summary>
        /// <returns>メールテンプレートファイル名</returns>
        /// <create>M.Shimizu 2020/06/012</create>
        /// <update></update>
        /// --------------------------------------------------
        private string GetTitleFileName()
        {
            if (this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1)
            {
                return MAIL_FILE.ASSOCIATION_REGULAR_TITLE_VALUE1;
            }

            return MAIL_FILE.ASSOCIATION_AR_TITLE_VALUE1;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 内容のメールテンプレートファイル名を取得します
        /// </summary>
        /// <returns>メールテンプレートファイル名</returns>
        /// <create>M.Shimizu 2020/06/012</create>
        /// <update></update>
        /// --------------------------------------------------
        private string GetNaiyoFileName()
        {
            if (this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1)
            {
                return MAIL_FILE.ASSOCIATION_REGULAR_VALUE1;
            }

            return MAIL_FILE.ASSOCIATION_AR_VALUE1;
        }

        #endregion

        #region 予約語の差替

        /// --------------------------------------------------
        /// <summary>
        /// 予約語を差し替える
        /// </summary>
        /// <param name="mailContents"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private string ReplaceMailContents(string mailContents, DataTable dt)
        {
            return mailContents
                .Replace(MAIL_RESERVE.SHUKKA_FLAG_VALUE1, this.cboShukkaFlag.Text)
                .Replace(MAIL_RESERVE.NONYUSAKI_NAME_VALUE1, this.cboBukkenmei.Text)
                .Replace(MAIL_RESERVE.SHIP_VALUE1, this.cboShip.Text)
                .Replace(MAIL_RESERVE.AR_NO_HLIST_VALUE1, this.GetARNoList("/", dt))
                .Replace(MAIL_RESERVE.AR_NO_VLIST_VALUE1, this.GetARNoList("\r\n", dt))
                .Replace(MAIL_RESERVE.AR_CREATE_USER_VALUE1, this.UserInfo.UserName);
        }

        #endregion

        #region ARNo.を配列で取得

        /// --------------------------------------------------
        /// <summary>
        /// AR No.をセパレータ区切りの配列で取得
        /// </summary>
        /// <param name="separator">セパレータ</param>
        /// <param name="dt">ARNoを含むDataTable</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private string GetARNoList(string separator, DataTable dt)
        {
            return string.Join(separator, dt.AsEnumerable().Select(x => x.Field<string>(Def_T_SHUKKA_MEISAI.AR_NO)).Distinct().ToArray());
        }

        #endregion

        #region TAG連携メールのエラーがあれば情報メッセージで表示

        /// --------------------------------------------------
        /// <summary>
        /// TAG連携メールのエラーがあれば情報メッセージで表示
        /// </summary>
        /// <create>H.Tajimi 2019/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ShowInfoIfTagRenkeiErrorOccured()
        {
            string errMsgID;
            string[] errArgs;

            // TAG連携メール登録チェック
            var ret = this.CheckTagRenkeiMail(false, out errMsgID, out errArgs);
            if (!string.IsNullOrEmpty(errMsgID))
            {
                this.ShowMessage(SystemBase.Controls.MessageImageType.Information, errMsgID, errArgs);
            }
        }
        
        #endregion

        #region TAG連携メールのエラーチェック

        /// --------------------------------------------------
        /// <summary>
        /// TAG連携メールのエラーチェック
        /// </summary>
        /// <param name="needsSet">取得した情報をメンバ変数に保持するかどうか</param>
        /// <param name="errMsgID">エラーメッセージID</param>
        /// <param name="errArgs">エラーメッセージ引数</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/08/21</create>
        /// <update>M.Shimizu 2020/07/02 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）</update>
        /// --------------------------------------------------
        private CheckMailResult CheckTagRenkeiMail(bool needsSet, out string errMsgID, out string[] errArgs)
        {
            errMsgID = string.Empty;
            errArgs = null;
            try
            {
                // メールアドレスチェック
                var conn = new ConnCommon();
                var cond = new CondCommon(this.UserInfo);
                var ds = conn.CheckTagRenkeiMail(cond);

                if (UtilData.GetFld(ds, Def_M_MAIL_SETTING.Name, 0, Def_M_MAIL_SETTING.TAG_RENKEI_EVENT) != TAG_RENKEI_EVENT.YES_VALUE1)
                {
                    // TAG連携通知しない場合はここで終了
                    return CheckMailResult.Ignore;
                }               

                if (string.IsNullOrEmpty(UtilData.GetFld(ds, Def_M_USER.Name, 0, Def_M_USER.MAIL_ADDRESS)))
                {
                    // 担当者にMailAddressが設定されていません。
                    errMsgID = "A9999999081";
                    return CheckMailResult.Error;
                }

                if (!UtilData.ExistsData(ds, ComDefine.DT_TAG_RENKEI_M_USER))
                {
                    // TAG連携メール送信対象者が設定されていません。
                    errMsgID = "S0100023010";
                    return CheckMailResult.Error;
                }

                if (needsSet)
                {
                    this._dtTagRenkeiUser = ds.Tables[ComDefine.DT_TAG_RENKEI_M_USER];
                    // ↓↓↓ M.Shimizu 2020/07/02 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）
                    // メール送信判定はタグ連携イベントで判定する
                    // this._templateFolder = UtilData.GetFld(ds, Def_M_MAIL_SETTING.Name, 0, Def_M_MAIL_SETTING.TEMPLATE_FOLDER);
                    this._isMailSend = true;
                    // ↑↑↑ M.Shimizu 2020/07/02 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）
                    this._mailAddress = UtilData.GetFld(ds, Def_M_USER.Name, 0, Def_M_USER.MAIL_ADDRESS);
                }

                return CheckMailResult.OK;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return CheckMailResult.Exception;
            }
        }

        #endregion

        #endregion
    }

    /// --------------------------------------------------
    /// <summary>
    /// TAG連携用データ
    /// </summary>
    /// <create>T.Nakata 2018/11/16</create>
    /// <update></update>
    /// --------------------------------------------------
    public class TagRenkeiData
    {
        private DataTable _dtTagRenkeiList { get; set; }
        public DataTable dtTagRenkeiList { get { return _dtTagRenkeiList; } }

        private string _ShukkaFlag { get; set; }
        public string ShukkaFlag { get { return _ShukkaFlag; } }

        private string _ShipName { get; set; }
        public string ShipName { get { return _ShipName; } }

        private string _BukkenName { get; set; }
        public string BukkenName { get { return _BukkenName; } }

        private string _NonyusakiCD { get; set; }
        public string NonyusakiCD { get { return _NonyusakiCD; } }

        public TagRenkeiData(DataTable dtTagRenkeiList, string ShukkaFlag, string ShipName, string BukkenName, string NonyusakiCD)
        {
            this._dtTagRenkeiList = dtTagRenkeiList;
            this._ShukkaFlag = ShukkaFlag;
            this._ShipName = ShipName;
            this._BukkenName = BukkenName;
            this._NonyusakiCD = NonyusakiCD;
        }
    }
}
