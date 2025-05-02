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
using System.Text;
using System.Collections.Generic;

namespace SMS.A01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// ＡＲ情報進捗管理画面
    /// </summary>
    /// <create>Y.Nakasato 2019/07/03</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ShinchokuKanri : SystemBase.Forms.CustomOrderForm
    {

        #region 定義

        /// --------------------------------------------------
        /// <summary>
        /// エクセル出力用テーブル名
        /// </summary>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string EXCEL_TABLENAME = "EXCEL_OUT_TABLE";

        /// --------------------------------------------------
        /// <summary>
        /// ElTableSheet：セル幅
        /// </summary>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_WIGHT = 80;

        /// --------------------------------------------------
        /// <summary>
        /// ElTableSheet：セル高
        /// </summary>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_HEIGHT = 50;

        #endregion

        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// ElTableSheet：更新セル背景色
        /// </summary>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private Color? _sheetUpdateBackColor;
        /// --------------------------------------------------
        /// <summary>
        /// ElTableSheet：更新セル前景色
        /// </summary>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private Color? _sheetUpdateForeColor;
        /// --------------------------------------------------
        /// <summary>
        /// ElTableSheet：更新日付け範囲
        /// </summary>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private int _sheetUpdateRecentDay;

        /// --------------------------------------------------
        /// <summary>
        /// 物件管理No
        /// </summary>
        /// <create>Y.Nakasato 2019/07/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _bukkenNo = string.Empty;

        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>Y.Nakasato 2019/07/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _nonyusakiCD = "";

        /// --------------------------------------------------
        /// <summary>
        /// 納入先名
        /// </summary>
        /// <create>Y.Nakasato 2019/07/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _nonyusakiName = "";

        /// --------------------------------------------------
        /// <summary>
        /// 変更履歴画面
        /// </summary>
        /// <create>Y.Nakasato 2019/07/18</create>
        /// <update></update>
        /// --------------------------------------------------
        ShinchokuKanriHenkoRireki _instHenkoRireki = null;

        /// --------------------------------------------------
        /// <summary>
        /// Excel出力用データテーブル
        /// </summary>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtExcelOutList = null;

        /// --------------------------------------------------
        /// <summary>
        /// 再検索用レンジ保存
        /// </summary>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private Range[] _SheetSelectRanges = null;

        /// --------------------------------------------------
        /// <summary>
        /// AR通知用
        /// </summary>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtARSendList = null;

        /// --------------------------------------------------
        /// <summary>
        /// 検索結果保存用
        /// </summary>
        /// <create>T.Nakata 2019/08/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtSearchData = null;

        /// --------------------------------------------------
        /// <summary>
        /// 日付名称用
        /// </summary>
        /// <create>D.Okumura 2019/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private IDictionary<string, string> _dicArDateKindName;
        /// --------------------------------------------------
        /// <summary>
        /// 日付名称ソート用
        /// </summary>
        /// <create>D.Okumura 2019/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private IDictionary<string, int> _dicArDateKindOrder;
        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ(デザイナ用)
        /// </summary>
        /// <create>D.Okumura 2020/01/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private ShinchokuKanri()
            : base(new UserInfo(), "")
        {
            InitializeComponent();
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="txtNonyusakiName"></param>
        /// <param name="bukkenNo"></param>
        /// <create>Y.Nakasato 2019/07/04</create>
        /// <update></update>
        /// --------------------------------------------------
        public ShinchokuKanri(UserInfo userInfo, string title, string txtNonyusakiName, string bukkenNo, string nonyusakiCD)
            : base(userInfo, title)
        {
            InitializeComponent();
            this._bukkenNo = bukkenNo;
            this._nonyusakiCD = nonyusakiCD;
            this._nonyusakiName = this.txtNonyusakiName.Text = txtNonyusakiName;
        }

        #endregion

        #region 初期化

        #region InitializeLoadControl

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update>D.Okumura 2019/08/23 汎用マスタの並び順に従うように修正</update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // フォームの設定
                this.IsCloseQuestion = false;

                // シートの初期化
                this.InitializeSheet(shtSintyoku);

                // コンボボックスの初期化
                this.InitializeCombo();

                // ファンクションボタン
                this.fbrFunction.F10Button.Enabled = true;
                this.fbrFunction.F11Button.Enabled = true;
                this.fbrFunction.F12Button.Enabled = true;

                // コンテキストメニュー
                this.toolStripMenuItemDateSite.Enabled = true;
                this.toolStripMenuItemDateLocal.Enabled = true;
                this.toolStripMenuItemDateJapan.Enabled = true;

                // 凡例
                DataTable dt = this.GetCommon(AR_HANREI.GROUPCD).Tables[Def_M_COMMON.Name];
                var arNoHanrei = ComFunc.GetFld(dt.AsEnumerable().FirstOrDefault(w => string.Equals(ComFunc.GetFld(w, Def_M_COMMON.ITEM_CD), AR_HANREI.AR_NO_RANGE_NAME)), Def_M_COMMON.ITEM_NAME);
                this.SetToolTip(txtArno, arNoHanrei);

                // 背景色
                DataTable dtColor = this.GetCommon(AR_SHINCHOKU_SETTING.GROUPCD).Tables[Def_M_COMMON.Name];
                var drRecent = dtColor.AsEnumerable().FirstOrDefault(w => string.Equals(ComFunc.GetFld(w, Def_M_COMMON.ITEM_CD), AR_SHINCHOKU_SETTING.RECENT_NAME));
                var recentColors = ComFunc.GetFld(drRecent, Def_M_COMMON.VALUE3).Split(',');
                this._sheetUpdateForeColor = ComFunc.GetColorFromRgb(recentColors[0]);
                this._sheetUpdateBackColor = recentColors.Length > 1 ? ComFunc.GetColorFromRgb(recentColors[1]) : null;
                this._sheetUpdateRecentDay = UtilConvert.ToInt32(ComFunc.GetFld(drRecent, Def_M_COMMON.VALUE1, AR_SHINCHOKU_SETTING.RECENT_VALUE1));

                // 日付名称等取得
                var dicDateKind = this.GetCommon(AR_DATE_KIND_DISP_FLAG.GROUPCD, Def_M_COMMON.VALUE1, (v) => v);
                this._dicArDateKindName = dicDateKind.ToDictionary(k => k.Key, v => ComFunc.GetFld(v.Value, Def_M_COMMON.ITEM_NAME));
                this._dicArDateKindOrder = dicDateKind.ToDictionary(k => k.Key, v => ComFunc.GetFldToInt32(v.Value, Def_M_COMMON.DISP_NO));
                // コンテキストメニュー並べ替え
                this.contextMenuGrid.Items.Clear();
                var menu = new[] {
                    new {Menu = this.toolStripMenuItemDateJapan, DispType = AR_DATE_KIND_DISP_FLAG.JP_VALUE1},
                    new {Menu = this.toolStripMenuItemDateLocal, DispType = AR_DATE_KIND_DISP_FLAG.LOCAL_VALUE1},
                    new {Menu = this.toolStripMenuItemDateSite, DispType = AR_DATE_KIND_DISP_FLAG.SITE_REQ_VALUE1},
                }.OrderBy(w => this._dicArDateKindOrder[w.DispType]).Select(w => w.Menu).ToArray();
                this.contextMenuGrid.Items.AddRange(menu);

                // ARリスト初期化
                this._dtARSendList = this.SetDtSendArList();
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
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                this.cboDisplay.Focus();
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
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void InitializeCombo()
        {
            try
            {
                this.MakeCmbBox(cboJyokyoFlag, JYOKYO_FLAG_AR.GROUPCD);
                this.MakeCmbBox(cboDisplay, AR_DATE_KIND_DISP_FLAG.GROUPCD);
                this.MakeCmbBox(cboKikan, AR_DATE_KIND_DISP_FLAG.GROUPCD);
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
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeSheet(Sheet sheet)
        {
            base.InitializeSheet(sheet);

            // コーナーヘッダー設定
            sheet.CornerHeaders[0, 0].Caption = Resources.ShinchokuKanri_ARNO;

            // オプション設定
            sheet.Enabled = false;
            sheet.AllowUserToAddRows = false;
            sheet.KeepHighlighted = true;
            sheet.SelectionType = SelectionType.MultipleRanges;
            sheet.ViewMode = ViewMode.Default;
            sheet.EditType = EditType.ReadOnly;
       
            // Enterキーを下のセルに移動するよう変更
            sheet.ShortCuts.Remove(Keys.Enter);
            sheet.ShortCuts.Add(Keys.Enter, new KeyAction[] { KeyAction.EndEdit, KeyAction.NextRow });
        }

        #endregion


        #endregion

        #region 終了処理

        /// --------------------------------------------------
        /// <summary>
        /// 終了処理
        /// </summary>
        /// <param name="e"></param>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (this._instHenkoRireki != null)
            {   // 履歴変更画面が表示されていたら閉じない
                e.Cancel = true;
                this._instHenkoRireki.Activate();
                return;
            }

            // AR送信情報があればメール送信を行う
            if (this._dtARSendList.Rows.Count > 0)
            {   // メール情報登録処理
                if (!this.SetMailInfo())
                {   // NGの場合は閉じない
                    e.Cancel = true;
                    return;
                }
            }
            base.OnClosing(e);
        }

        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // 画面表示
                this.cboDisplay.SelectedValue = AR_DATE_KIND_DISP_FLAG.DEFAULT_VALUE1;
                // 件数
                this.txtCount.Text = string.Empty;
                // 検索項目
                this.SearchClear();
                // シート
                this.SheetClear();
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
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SearchClear()
        {
            try
            {
                // 状態選択
                this.cboJyokyoFlag.SelectedValue = JYOKYO_FLAG_AR.DEFAULT_VALUE1;
                // 期間(日付)
                this.cboKikan.SelectedValue = AR_DATE_KIND_DISP_FLAG.DEFAULT_VALUE1;

                // 期間(カレンダー)
                this.dtpKikanFrom.Value = null;
                this.dtpKikanTo.Value = null;

                // 機種
                this.txtKishu.Text = string.Empty;
                // 号機
                this.txtGoki.Text = string.Empty;

                grpSearch.Enabled = true;
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
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            this.shtSintyoku.Redraw = false;
            this.shtSintyoku.MaxRows = 0;
            this.shtSintyoku.MaxColumns = 0;
            this.shtSintyoku.DataSource = null;
            this.shtSintyoku.Enabled = false;
            this.shtSintyoku.Redraw = true;

            _dtExcelOutList = null;
            _dtSearchData = null;
        }

        #endregion

        #endregion

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                // 期間大小チェック
                if (this.dtpKikanFrom.Value != null && this.dtpKikanTo.Value != null)
                {
                    DateTime? timeFrom = this.dtpKikanFrom.Value as DateTime?;
                    DateTime? timeTo = this.dtpKikanTo.Value as DateTime?;
                    if (timeFrom > timeTo)
                    {   // 期間の範囲が不正です。期間を確認してください。
                        this.ShowMessage("A9999999078");
                        return false;
                    }
                }

                // ARNo
                if (!ComFunc.CheckARNo(txtArno.Text, this.UserInfo.SysInfo.SeparatorRange))
                {
                    // ARNoの入力が不正です。確認してください。
                    this.ShowMessage("A9999999077");
                    return false;
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

        #region コンディション取得

        /// --------------------------------------------------
        /// <summary>
        /// コンディション取得
        /// </summary>
        /// <returns>コンディション</returns>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondA1 GetCondition()
        {
            CondA1 cond = new CondA1(this.UserInfo);

            cond.UpdateUserID = this.UserInfo.UserID;
            cond.UpdateUserName = this.UserInfo.UserName;

            cond.SeparatorItem = this.UserInfo.SysInfo.SeparatorItem;
            cond.SeparatorRange = this.UserInfo.SysInfo.SeparatorRange;
            cond.BukkenNo = this._bukkenNo;
            cond.NonyusakiCD = this._nonyusakiCD;
            cond.NonyusakiName = this._nonyusakiName;
            cond.ArNo = this.txtArno.Text;
            cond.Kishu = this.txtKishu.Text;
            cond.Goki = this.txtGoki.Text;
            cond.DateKubun = this.cboKikan.SelectedValue as string;
            cond.UpdateDateFrom = dtpKikanFrom.Value as DateTime?;
            cond.UpdateDateTo = dtpKikanTo.Value as DateTime?;
            cond.JyokyoFlagAR = this.cboJyokyoFlag.SelectedValue as string;

            return cond;
        }

        #endregion


        #region 検索処理

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理制御部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearch()
        {
            this.ClearMessage();
            return base.RunSearch();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理実行部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                this.lblDisplay.Enabled = false;
                this.grpSearch.Enabled = false;
                this.txtCount.Clear();

                if (_dtSearchData == null)
                {
                    CondA1 cond = this.GetCondition();
                    ConnA01 conn = new ConnA01();
                    _dtSearchData = conn.GetArShinchokuList(cond);
                    if (_dtSearchData == null || _dtSearchData.Rows.Count < 1)
                    {   // 進捗Dataがありません。
                        this.ShowMessage("A0100050003");
                        this.SheetClear();
                        return false;
                    }
                }
                // データの設定
                this.SetShinchokuSheet(this.shtSintyoku, _dtSearchData);

                // 選択範囲が保存されていれば反映する
                if (_SheetSelectRanges != null)
                {
                    bool initflag = false;
                    try
                    {
                        foreach (Range objRange in _SheetSelectRanges)
                        {
                            if (!initflag)
                            {   // 初期状態で[0,0]のセルが選択されている為1つ目の選択は範囲変更で対応する
                                this.shtSintyoku.UIReSelection(objRange);
                                initflag = true;
                            }
                            else // 二つ目以降は範囲追加
                                this.shtSintyoku.UIAddSelection(objRange);
                        }
                    }
                    catch
                    {   // 範囲選択が不可であった場合選択を解除する。(左上セルが選択される)
                        this.shtSintyoku.UIClearAllSelection();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
            finally
            {
                _SheetSelectRanges = null; // 保存範囲を初期化
                this.grpSearch.Enabled = true;
                this.lblDisplay.Enabled = true;
            }
        }

        #endregion

        #region イベント

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F01(編集)ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2020/01/28</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            // 基底処理は呼び出さない
            //base.fbrFunction_F01Button_Click(sender, e);
            try
            {
                ShowARMeisai(SystemBase.EditMode.Update);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }
        /// --------------------------------------------------
        /// <summary>
        /// F02(照会)ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2020/01/28</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F02Button_Click(object sender, EventArgs e)
        {
            //base.fbrFunction_F02Button_Click(sender, e);
            try
            {
                ShowARMeisai(SystemBase.EditMode.View);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }
        /// --------------------------------------------------
        /// <summary>
        /// F10(Excel)ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/18</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F10Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F10Button_Click(sender, e);
            try
            {
                if (_dtExcelOutList == null || _dtExcelOutList.Rows.Count <= 0)
                {   // 出力するDataがありません。検索してから実行してください。
                    this.ShowMessage("A0100050006");
                    return;
                }

                SaveFileDialog frm = new SaveFileDialog();
                frm.FileName = ComDefine.EXCEL_FILE_AR_SHINCHOKU;
                frm.Filter = Resources.ShinchokuRireki_RefDlgFilter;
                if (frm.ShowDialog() != DialogResult.OK) return;

                // Excel出力処理
                if (this.ExcelOutput(frm.FileName))
                {   // 進捗DataのExcelFileを出力しました。
                    this.ShowMessage("A0100050007");
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F11(履歴)ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/18</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F11Button_Click(object sender, EventArgs e)
        {
            if (_instHenkoRireki != null)
            {
                _instHenkoRireki.Activate();
                return;
            }
            base.fbrFunction_F04Button_Click(sender, e);
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

                // 変更履歴画面
                _instHenkoRireki = new ShinchokuKanriHenkoRireki(this.UserInfo, ComDefine.TITLE_A0100060, this._nonyusakiCD, this.txtNonyusakiName.Text);
                _instHenkoRireki.Icon = this.Icon;
                _instHenkoRireki.FormClosed += new FormClosedEventHandler(HenkoRireki_FormClosed);
                _instHenkoRireki.Show();
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
        /// <create>Y.Nakasato 2019/07/18</create>
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

        #region 子画面が閉じた時の処理

        /// --------------------------------------------------
        /// <summary>
        /// 子画面(進捗管理)が閉じた時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private void HenkoRireki_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_instHenkoRireki != null)
            {
                this._instHenkoRireki = null;
            }
        }

        #endregion

        #region 表示ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 機種一覧ボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnKishu_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                using (var form = new ShinchokuKishuIchiran(this.UserInfo, ComDefine.TITLE_A0100052, this._nonyusakiCD, this.txtKishu.Text))
                {
                    form.Icon = this.Icon;
                    var result = form.ShowDialog(this);
                    if (result == DialogResult.OK)
                    {
                        this.txtKishu.Text = form.Kishu;
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
        /// 号機一覧ボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnGoki_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                using (var form = new ShinchokuGokiIchiran(this.UserInfo, ComDefine.TITLE_A0100053, this._nonyusakiCD, this.txtGoki.Text))
                {
                    form.Icon = this.Icon;
                    var result = form.ShowDialog(this);
                    if (result == DialogResult.OK)
                    {
                        this.txtGoki.Text = form.Goki;
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }
        #endregion

        #region AR明細表示
        /// --------------------------------------------------
        /// <summary>
        /// AR明細画面表示処理
        /// </summary>
        /// <param name="mode">編集モード</param>
        /// <create>D.Okumura 2020/01/28</create>
        /// <update>M.Shimizu 2020/04/17 AR進捗・関連付け対応</update>
        /// --------------------------------------------------
        private void ShowARMeisai(SystemBase.EditMode mode)
        {
            Range objRange = this.shtSintyoku.GetBlocks(BlocksType.Selection).FirstOrDefault();
            if (objRange == null || objRange.IsEmpty)
            {
                // 進捗Dataがありません。
                this.ShowMessage("A0100050003");
                return;
            }
            string arno = this.shtSintyoku.RowHeaders[objRange.TopRow].Caption;

            // 既に表示されてるAR明細画面を取得
            ARJohoMeisai　arJohoMeisai　= GetARJohoMeisai(this._nonyusakiCD, arno, mode);

            // 既にAR明細画面が表示されていた場合、新しいAR明細画面を生成せず、既存のAR明細画面を最前面にする。
            if (arJohoMeisai != null)
            {
                arJohoMeisai.Activate();
                return;
            }

            string msgId;
            string[] msgArgs;
            ARJohoMeisai form = ARJoho.ShowARJohoMeisai(this.UserInfo, mode, this._nonyusakiCD, arno, out msgId, out msgArgs);
            if (form == null)
            {
                this.ShowMessage(msgId, msgArgs);
            }
            else
            {
                form.FormClosed += (sender, e) =>
                {
                    this._dtSearchData = null;
                    this.RunSearch();
                };
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 納入先、ARNoが一致するAR明細画面を取得します
        /// モードで一致するAR明細画面があれば優先します
        /// </summary>
        /// <param name="nonyusakiCd"></param>
        /// <param name="arNo"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        /// <create>M.Shimizu 2020/04/17 AR進捗・関連付け対応</create>
        /// <update></update>
        /// --------------------------------------------------
        private ARJohoMeisai GetARJohoMeisai(string nonyusakiCd, string arNo, SystemBase.EditMode mode)
        {
            List<ARJohoMeisai> arJohoMeisaiList = new List<ARJohoMeisai>();

            // 納入先、ARNoが一致するAR明細画面を取得
            foreach (var openForm in Application.OpenForms)
            {
                ARJohoMeisai form = openForm as ARJohoMeisai;

                if (form != null
                    && form.NonyusakiCd == nonyusakiCd
                    && form.ArNo == arNo)
                {
                    arJohoMeisaiList.Add(form);
                }
            }

            // 納入先、ARNoで一致するAR明細画面がない場合は、処理を終了する
            if (!arJohoMeisaiList.Any())
            {
                return null;
            }

            // モードで一致するAR明細画面を取得
            ARJohoMeisai arJohoMeisai = arJohoMeisaiList.Where(x => x.Mode == mode).FirstOrDefault();

            if (arJohoMeisai != null)
            {
                return arJohoMeisai;
            }
            else
            {
                // モードで一致しなければ、先頭のAR明細画面
                return arJohoMeisaiList.FirstOrDefault();
            }
        }
        #endregion

        #region 画面表示切り替わり時の処理

        /// --------------------------------------------------
        /// <summary>
        /// 画面表示切り替わり時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update>D.Okumura 2019/08/22 画面表示切替で選択状態を維持する</update>
        /// --------------------------------------------------
        private void cboDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ElTableSheet右クリックメニューの項目有効化
            string DispType = this.cboDisplay.SelectedValue.ToString();
            toolStripMenuItemDateSite.Enabled = (DispType == AR_DATE_KIND_DISP_FLAG.ALL_VALUE1 || DispType == AR_DATE_KIND_DISP_FLAG.SITE_REQ_VALUE1);
            toolStripMenuItemDateLocal.Enabled = (DispType == AR_DATE_KIND_DISP_FLAG.ALL_VALUE1 || DispType == AR_DATE_KIND_DISP_FLAG.LOCAL_VALUE1);
            toolStripMenuItemDateJapan.Enabled = (DispType == AR_DATE_KIND_DISP_FLAG.ALL_VALUE1 || DispType == AR_DATE_KIND_DISP_FLAG.JP_VALUE1);

            // 検索再実行
            if (_dtSearchData != null)
            {
                this._SheetSelectRanges = this.shtSintyoku.Rows.Count > 0 ? this.shtSintyoku.GetBlocks(BlocksType.Selection) : null;
                this.RunSearch();
            }
        }

        #endregion


        #region 検索ボタン
        
        /// --------------------------------------------------
        /// <summary>
        /// 検索開始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnView_Click(object sender, EventArgs e)
        {
            _dtSearchData = null;
            this.RunSearch();
        }

        #endregion


        #region コンテキストメニュー

        /// --------------------------------------------------
        /// <summary>
        /// 右クリックメニュー：オープン
        /// </summary>
        /// <returns></returns>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void contextMenuGrid_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// 右クリックメニュー：現場到着希望日
        /// </summary>
        /// <returns></returns>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void toolStripMenuItemDateSite_Click(object sender, EventArgs e)
        {
            this.ContextMenuProc(AR_DATE_KIND_DISP_FLAG.SITE_REQ_VALUE1);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 右クリックメニュー：現場出荷予定日
        /// </summary>
        /// <returns></returns>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void toolStripMenuItemDateLocal_Click(object sender, EventArgs e)
        {
            this.ContextMenuProc(AR_DATE_KIND_DISP_FLAG.LOCAL_VALUE1);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 右クリックメニュー：日本出荷予定日
        /// </summary>
        /// <returns></returns>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void toolStripMenuItemDateJapan_Click(object sender, EventArgs e)
        {
            this.ContextMenuProc(AR_DATE_KIND_DISP_FLAG.JP_VALUE1);
        }

        #endregion

        #endregion

        #region ElTableSheet関連

        /// --------------------------------------------------
        /// <summary>
        /// 進捗リスト作成/Excel出力用データテーブル作成
        /// </summary>
        /// <returns></returns>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update>D.Okumura 2019/08/23 汎用マスタの並び順に従うように修正</update>
        /// --------------------------------------------------
        private bool SetShinchokuSheet(Sheet sheet, DataTable dt)
        {
            try
            {
                sheet.Redraw = false;
                sheet.MaxColumns = 0;
                sheet.MaxRows = 0;
                _dtExcelOutList = null;
                // Cell内表示項目定義(ソートあり)
                var list = new[] {
                    new {DispType = AR_DATE_KIND_DISP_FLAG.SITE_REQ_VALUE1, FieldName = Def_T_AR_SHINCHOKU.DATE_SITE_REQ, Label = Resources.ShinchokuRireki_SITE_REQ_Label},
                    new {DispType = AR_DATE_KIND_DISP_FLAG.JP_VALUE1, FieldName = Def_T_AR_SHINCHOKU.DATE_JP, Label = Resources.ShinchokuRireki_JP_Label},
                    new {DispType = AR_DATE_KIND_DISP_FLAG.LOCAL_VALUE1, FieldName = Def_T_AR_SHINCHOKU.DATE_LOCAL, Label = Resources.ShinchokuRireki_LOCAL_Label},
                }.OrderBy(w => this._dicArDateKindOrder[w.DispType]).ToArray();

                // 号機一覧
                DataTable dtDistinctGoki = dt.DefaultView.ToTable(true, Def_T_AR_SHINCHOKU.GOKI);
                if (dtDistinctGoki.Rows.Count <= 0)
                {   // 進捗Dataがありません。
                    this.ShowMessage("A0100050003");
                    return false;
                }
                DataRow[] drDistinctGoki = dtDistinctGoki.Select("", Def_T_AR_SHINCHOKU.GOKI + " ASC");

                // ARNo一覧
                DataTable dtDistinctAR = dt.DefaultView.ToTable(true, Def_T_AR_SHINCHOKU.AR_NO);
                if (dtDistinctAR.Rows.Count <= 0)
                {   // 進捗Dataがありません。
                    this.ShowMessage("A0100050003");
                    return false;
                }
                DataRow[] drDistinctAR = dtDistinctAR.Select("", Def_T_AR_SHINCHOKU.AR_NO + " ASC");

                // Excel出力用データテーブル初期化
                _dtExcelOutList = new DataTable(EXCEL_TABLENAME);
                _dtExcelOutList.Columns.Add(Resources.ShinchokuKanri_ARNO, typeof(string));

                // 号機分列追加
                foreach (DataRow dr in drDistinctGoki)
                {   // ヘッダー設定
                    sheet.InsertColumn(sheet.MaxColumns, false);
                    int index = sheet.MaxColumns - 1;
                    Header HeadObj = sheet.ColumnHeaders[index];
                    HeadObj.Caption = dr[Def_T_AR_SHINCHOKU.GOKI].ToString();
                    HeadObj.AlignHorizontal = AlignHorizontal.Center;
                    HeadObj.AlignVertical = AlignVertical.Middle;
                    // サイズ設定
                    sheet.Columns[index].Width = SHEET_WIGHT;

                    // Excel出力用：カラムヘッダに号機名を設定する
                    _dtExcelOutList.Columns.Add(HeadObj.Caption, typeof(string));
                }

                // 行追加処理
                foreach (DataRow dr in drDistinctAR)
                {   // ヘッダー設定
                    sheet.InsertRow(sheet.MaxRows, false);
                    int index = sheet.MaxRows - 1;
                    Header HeadObj = sheet.RowHeaders[index];
                    HeadObj.Caption = dr[Def_T_AR_SHINCHOKU.AR_NO].ToString();
                    HeadObj.AlignHorizontal = AlignHorizontal.Center;
                    HeadObj.AlignVertical = AlignVertical.Middle;
                    // サイズ設定
                    sheet.Rows[index].Height = SHEET_HEIGHT;

                    // Excel出力用
                    DataRow excelDr = _dtExcelOutList.NewRow();
                    excelDr[0] = HeadObj.Caption;//1列目にARNoを設定する

                    // 号機毎進捗状況
                    for (int ColCount = 0; ColCount < sheet.Columns.Count; ColCount++)
                    {
                        string tmpGoki = sheet.ColumnHeaders[ColCount].Caption;
                        string expression = Def_T_AR_SHINCHOKU.GOKI + " = '" + tmpGoki + "' and " + Def_T_AR_SHINCHOKU.AR_NO + " = '" + dr[Def_T_AR_SHINCHOKU.AR_NO].ToString() + "'";
                        DataRow drShinchoku = dt.Select(expression).AsEnumerable().FirstOrDefault();
                        if (drShinchoku == null)
                            continue;

                        StringBuilder tmpStr = new StringBuilder();
                        string DispType = this.cboDisplay.SelectedValue.ToString();
                        foreach (var item in list)
                        {
                            // 画面表示：全て/指定項目以外はスキップする
                            if (!AR_DATE_KIND_DISP_FLAG.ALL_VALUE1.Equals(DispType) && !item.DispType.Equals(DispType))
                                continue;
                            // 文字列を作成する
                            SetShinchokuData(drShinchoku[item.FieldName], item.Label, tmpStr);
                        }
                        if (tmpStr.Length > 0)
                        {   // 進捗日付があればセルに格納する
                            Cell objCell = sheet[ColCount, index];

                            // エディタ設定
                            TextEditor objTextEditor = new TextEditor();
                            objTextEditor.MultiLine = true;
                            objCell.AlignHorizontal = AlignHorizontal.Center;
                            objCell.AlignVertical = AlignVertical.Middle;
                            objCell.Editor = objTextEditor;
                            objCell.Text = tmpStr.ToString();

                            // 更新日付により背景色付け
                            DateTime tgtDate = DateTime.Now.Date.AddDays(-1 * (this._sheetUpdateRecentDay - 1));
                            DateTime? upDate = drShinchoku[Def_T_AR_SHINCHOKU.UPDATE_DATE] as DateTime?;
                            if (tgtDate <= upDate)
                            {   // 背景色を変えると罫線が消えるので再度罫線を描く
                                if (this._sheetUpdateBackColor != null)
                                    objCell.BackColor = (Color)this._sheetUpdateBackColor;
                                if (this._sheetUpdateForeColor != null)
                                    objCell.ForeColor = (Color)this._sheetUpdateForeColor;
                                objCell.SetBorder(new BorderLine(Color.DarkGray, BorderLineStyle.Thin), Borders.All);
                            }

                            // Excel出力用
                            excelDr[ColCount + 1] = tmpStr;
                        }
                    }
                    // Excel出力用AR進捗追加
                    _dtExcelOutList.Rows.Add(excelDr);
                }
                sheet.Enabled = true;

                // 件数表示
                this.txtCount.Text = drDistinctAR.Length.ToString();
            }
            finally
            {
                sheet.Redraw = true;
            }
            return true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 進捗情報取得
        /// </summary>
        /// <param name="rowdata">日付情報</param>
        /// <param name="label">ラベルフォーマット 0:日付</param>
        /// <param name="celltxt">出力情報</param>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update>D.Okumura 2019/08/23 フォーマットをリソースへ移動</update>
        /// --------------------------------------------------
        private void SetShinchokuData(object rowdata, string label, StringBuilder celltxt)
        {
            if (celltxt.Length > 0)
                celltxt.ApdL();

            if (rowdata != DBNull.Value)
            {
                DateTime dTime;
                string dateStr = rowdata.ToString();
                if (string.IsNullOrEmpty(dateStr) || !DateTime.TryParse(dateStr, out dTime))
                {
                    celltxt.Append(Resources.ShinchokuRireki_EMPTY_Label);
                }
                else
                {
                    celltxt.AppendFormat(label, dTime);
                }
            }
            else
            {
                celltxt.Append(Resources.ShinchokuRireki_EMPTY_Label);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンテキストメニュー処理
        /// </summary>
        /// <returns></returns>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update>D.Okumura 2020/06/01 メール送信Webサービス化対応</update>
        /// --------------------------------------------------
        private void ContextMenuProc(string DispType)
        {
            try
            {
                this.ClearMessage();
                this._SheetSelectRanges = null;

                DataTable dt = new DataTable(Def_T_AR_SHINCHOKU_RIREKI.Name);
                dt.Columns.Add(Def_T_AR_SHINCHOKU_RIREKI.NONYUSAKI_CD, typeof(string));
                dt.Columns.Add(Def_T_AR_SHINCHOKU_RIREKI.AR_NO, typeof(string));
                dt.Columns.Add(Def_T_AR_SHINCHOKU_RIREKI.GOKI, typeof(string));
                dt.Columns.Add(Def_T_AR_SHINCHOKU_RIREKI.DATE_KIND, typeof(string));

                // 選択範囲のデータ整理
                Range[] objRanges;
                objRanges = this.shtSintyoku.GetBlocks(BlocksType.Selection);
                foreach (Range objRange in objRanges)
                {
                    for (int rowIndex = objRange.TopRow; rowIndex <= objRange.BottomRow; rowIndex++)
                    {
                        for (int colIndex = objRange.LeftColumn; colIndex <= objRange.RightColumn; colIndex++)
                        {
                            if (!string.IsNullOrEmpty(this.shtSintyoku[colIndex, rowIndex].Text))
                            {
                                DataRow addDr = dt.NewRow();
                                addDr[Def_T_AR_SHINCHOKU_RIREKI.NONYUSAKI_CD] = _nonyusakiCD;
                                addDr[Def_T_AR_SHINCHOKU_RIREKI.AR_NO] = this.shtSintyoku.RowHeaders[rowIndex].Caption;
                                addDr[Def_T_AR_SHINCHOKU_RIREKI.GOKI] = this.shtSintyoku.ColumnHeaders[colIndex].Caption;
                                addDr[Def_T_AR_SHINCHOKU_RIREKI.DATE_KIND] = DispType;
                                dt.Rows.Add(addDr);
                            }
                        }
                    }
                }
                if (dt.Rows.Count <= 0)
                {   // 日付設定対象のCellが選択されていません。内容の記載のあるCellを選択してください。
                    this.ShowMessage("A0100050004");
                    return;
                }

                // メール設定取得(4:AR進捗通知(Default))
                if (this.GetMailInfo(string.Empty) == null)
                {
                    return;
                }

                // 日付登録ダイアログ表示
                bool reSearchFlag = false;
                string formTitle = string.Empty;
                if (_dicArDateKindName.ContainsKey(DispType))
                    formTitle = _dicArDateKindName[DispType];
                using (var form = new ShinchokuKanriDateTouroku(this.UserInfo, formTitle, dt, this._bukkenNo))
                {
                    form.Icon = this.Icon;
                    var result = form.ShowDialog(this);

                    // 結果表示
                    if (result.Equals(DialogResult.OK))
                    {   // 現場到着日時である場合通知先一覧情報にAR番号を追加する。(存在しない場合のみ)
                        if (DispType == AR_DATE_KIND_DISP_FLAG.SITE_REQ_VALUE1)
                        {
                            DataTable dtDistinctAR = dt.DefaultView.ToTable(true, Def_T_AR_SHINCHOKU_RIREKI.AR_NO);
                            if (dtDistinctAR.Rows.Count > 0)
                            {
                                foreach (DataRow inDr in dtDistinctAR.Rows)
                                {
                                    DataRow[] tmpDr = this._dtARSendList.Select(ComDefine.FLD_ARSHINCHOKU_ARNO + " = '" + inDr[Def_T_AR_SHINCHOKU.AR_NO] + "'");
                                    if (!(tmpDr != null && tmpDr.Length > 0))
                                    {   // 更新されたARNoが送信用ARリストに無ければ追加する
                                        DataRow addDr = this._dtARSendList.NewRow();
                                        addDr[ComDefine.FLD_ARSHINCHOKU_ARNO] = inDr[Def_T_AR_SHINCHOKU.AR_NO];
                                        this._dtARSendList.Rows.Add(addDr);
                                    }
                                }
                            }
                        }

                        // 保存しました。
                        this.ShowMessage("A9999999045");
                        reSearchFlag = true;
                    }
                    else if (result.Equals(DialogResult.Retry))
                    {
                        this.ShowMessage(form.ErrMsgID, form.ErrMsgArgs);
                        reSearchFlag = true;
                    }
                    else
                    {   // DialogResult.Cancel
                        if (!string.IsNullOrEmpty(form.ErrMsgID))
                        {   // 日付登録側でエラーが積まれていたらここで表示する
                            this.ShowMessage(form.ErrMsgID, form.ErrMsgArgs);
                        }
                    }
                }

                // 再検索処理
                if (reSearchFlag)
                {
                    this._dtSearchData = null;
                    this._SheetSelectRanges = objRanges;
                    this.RunSearchExec();
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region Excel出力

        /// --------------------------------------------------
        /// <summary>
        /// Excel出力
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ExcelOutput(string filePath)
        {
            bool retVal = false;
            try
            {
                string msgID = string.Empty;
                string[] args = null;

                if (_dtExcelOutList != null && _dtExcelOutList.Rows.Count > 0)
                {
                    ExportShinchokuKanri export = new ExportShinchokuKanri();
                    retVal = export.ExportExcel(filePath, _dtExcelOutList, out msgID, out args);
                }
                else
                {   // 出力するDataがありません。検索してから実行してください。
                    msgID = "A0100050006";
                }

                if (!string.IsNullOrEmpty(msgID))
                {
                    this.ShowMessage(msgID, args);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            return retVal;
        }

        #endregion

        #region 納入先マスタ存在チェック
        /// --------------------------------------------------
        /// <summary>
        /// 納入先マスタ存在チェック
        /// </summary>
        /// <returns></returns>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckNonyusaki()
        {
            CondA1 cond = this.GetCondition();
            ConnA01 conn = new ConnA01();

            cond.NonyusakiCD = this._nonyusakiCD;

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
                //this._nonyusakiComplete = true;
            }

            return true;
        }
        #endregion

        #region DataTable用

        #region ARリスト初期化
        /// --------------------------------------------------
        /// <summary>
        /// ARリスト初期化
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable SetDtSendArList()
        {
            DataTable dt = new DataTable(ComDefine.DTTBL_ARSHINCHOKU_MAILINFO);
            dt.Columns.Add(ComDefine.FLD_ARSHINCHOKU_ARNO, typeof(string));
            return dt;
        }
        #endregion

        #endregion

        #region メール関連

        #region メール情報取得
        /// --------------------------------------------------
        /// <summary>
        /// メール情報取得
        /// </summary>
        /// <param name="bukkenNo">物件No</param>
        /// <param name="dsMail">メール送信先設定</param>
        /// <returns>メール送信先設定(nullの場合、取得失敗)</returns>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update>D.Okumura 2020/06/01 メール送信Webサービス化対応</update>
        /// --------------------------------------------------
        private DataSet GetMailInfo(string bukkenNo)
        {
            try
            {
                CondA1 cond = this.GetCondition();
                ConnA01 conn = new ConnA01();

                if (bukkenNo != null)
                {
                    cond.BukkenNo = bukkenNo;
                }
                string errMsgID = string.Empty;
                string[] args = new string[0];
                DataSet dsMail = conn.GetArShinchokuMailInfo(cond, out errMsgID, out args);
                if (!string.IsNullOrEmpty(errMsgID))
                {
                    this.ShowMessage(errMsgID, args);
                    return null;
                }
                if (dsMail == null || dsMail.Tables.Count <= 0 || dsMail.Tables[0].Rows.Count <= 0)
                {
                    // 基本通知設定で送信先が登録されていません。
                    this.ShowMessage("A9999999057");
                    return null;
                }

                return dsMail;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return null;
            }
        }
        #endregion

        #region メール情報登録
        /// --------------------------------------------------
        /// <summary>
        /// メール情報登録
        /// </summary>
        /// <returns></returns>
        /// <create>T.Nakata 2019/07/22</create>
        /// <update>D.Okumura 2020/06/01 メール送信Webサービス化対応</update>
        /// --------------------------------------------------
        private bool SetMailInfo()
        {
            try
            {
                bool ret = false;
                string errMsgID = string.Empty;
                string[] args = new string[0];
                if (UtilData.ExistsData(this._dtARSendList))
                {
                    CondA1 cond = this.GetCondition();
                    ConnA01 conn = new ConnA01();
                    DataSet dsMailInfo = new DataSet();
                    dsMailInfo.Tables.Add(_dtARSendList.Copy());
                    ret = conn.SendArShinchokuMail(cond, dsMailInfo, out errMsgID, out args);
                }
                if (!string.IsNullOrEmpty(errMsgID))
                    this.ShowMessage(errMsgID, args);
                return ret;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }
        #endregion

        #endregion
    }
}
