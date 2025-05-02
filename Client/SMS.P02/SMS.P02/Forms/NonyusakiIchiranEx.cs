using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

using WsConnection.WebRefP02;
using GrapeCity.Win.ElTabelle.Editors;
using SMS.P02.Properties;

namespace SMS.P02.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 納入先一覧（複数選択）
    /// </summary>
    /// <remarks>
    /// ダブルクリック時の動作、選択データ格納先が異なりNonyusakiIchiranを
    /// 継承すると制御が煩雑になってしまうため、CustomOrderSearchDialogを
    /// 継承して、画面デザイン・処理はNonyusakiIchiranをコピーして作成
    /// </remarks>
    /// <create>H.Tajimi 2015/12/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class NonyusakiIchiranEx : SystemBase.Forms.CustomOrderSearchDialog
    {
        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// シートで使用する選択の列インデックス
        /// </summary>
        /// <create>H.Tajimi 2015/12/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int COL_SELECT_CHK = 0;

        /// --------------------------------------------------
        /// <summary>
        /// 選択の列の初期値
        /// </summary>
        /// <create>H.Tajimi 2015/12/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private const bool DEFAULT_SELECT_VALUE = false;

        #endregion

        #region Fields

        private string _shukkaFlag = string.Empty;
        private string _nonyusakiName = string.Empty;
        private string _ship = string.Empty;
        private string _tagShip = string.Empty;
        private string _gamenFlag = string.Empty;
        private bool _isInitSearch = false;
        private bool _isAllShip = false;
        private EnumerableRowCollection<DataRow> _drCollection = null;

        #endregion

        #region プロパティ

        /// --------------------------------------------------
        /// <summary>
        /// 選択チェックボックスがチェックされている行
        /// </summary>
        /// <create>H.Tajimi 2015/12/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public EnumerableRowCollection<DataRow> SelectedRowsData
        {
            get { return this._drCollection; }
            set { this._drCollection = value; }
        }

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ(デザイナ用)
        /// </summary>
        /// <create>H.Tajimi 2015/12/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private NonyusakiIchiranEx()
            : base()
        {
            InitializeComponent();
            // 画面タイトル設定
            this.Title = ComDefine.TITLE_P0200010;
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="shukkaFlag">出荷区分</param>
        /// <param name="nonyusakiName">納入先</param>
        /// <param name="ship">出荷便</param>
        /// <create>H.Tajimi 2015/12/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public NonyusakiIchiranEx(UserInfo userInfo, string shukkaFlag, string nonyusakiName, string ship)
            : this(userInfo, shukkaFlag, nonyusakiName, ship, string.Empty)
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="shukkaFlag">出荷区分</param>
        /// <param name="nonyusakiName">納入先</param>
        /// <param name="ship">出荷便</param>
        /// <param name="gamenFlag">画面区分</param>
        /// <create>H.Tajimi 2015/12/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public NonyusakiIchiranEx(UserInfo userInfo, string shukkaFlag, string nonyusakiName, string ship, string gamenFlag)
            : this(userInfo, shukkaFlag, nonyusakiName, ship, gamenFlag, false)
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="shukkaFlag">出荷区分</param>
        /// <param name="nonyusakiName">納入先</param>
        /// <param name="ship">出荷便</param>
        /// <param name="isInitSearch">初期検索を行うかどうか</param>
        /// <create>H.Tajimi 2015/12/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public NonyusakiIchiranEx(UserInfo userInfo, string shukkaFlag, string nonyusakiName, string ship, bool isInitSearch)
            : this(userInfo, shukkaFlag, nonyusakiName, ship, string.Empty, isInitSearch)
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="shukkaFlag">出荷区分</param>
        /// <param name="nonyusakiName">納入先</param>
        /// <param name="ship">出荷便</param>
        /// <param name="isInitSearch">初期検索を行うかどうか</param>
        /// <param name="isAllShip">全便かどうか</param>
        /// <create>H.Tajimi 2015/12/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public NonyusakiIchiranEx(UserInfo userInfo, string shukkaFlag, string nonyusakiName, string ship, bool isInitSearch, bool isAllShip)
            : this(userInfo, shukkaFlag, nonyusakiName, ship, string.Empty, isInitSearch, isAllShip) 
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="shukkaFlag">出荷区分</param>
        /// <param name="nonyusakiName">納入先</param>
        /// <param name="ship">出荷便</param>
        /// <param name="gamenFlag">画面区分</param>
        /// <param name="isInitSearch">初期検索を行うかどうか</param>
        /// <create>H.Tajimi 2015/12/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public NonyusakiIchiranEx(UserInfo userInfo, string shukkaFlag, string nonyusakiName, string ship, string gamenFlag, bool isInitSearch)
            : this(userInfo, shukkaFlag, nonyusakiName, ship, gamenFlag, isInitSearch, false) 
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="shukkaFlag">出荷区分</param>
        /// <param name="nonyusakiName">納入先</param>
        /// <param name="ship">出荷便</param>
        /// <param name="gamenFlag">画面区分</param>
        /// <param name="isInitSearch">初期検索を行うかどうか</param>
        /// <param name="isAllShip">全便かどうか</param>
        /// <create>H.Tajimi 2015/12/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public NonyusakiIchiranEx(UserInfo userInfo, string shukkaFlag, string nonyusakiName, string ship, string gamenFlag, bool isInitSearch, bool isAllShip)
            : base(userInfo)
        {
            InitializeComponent();
            // 画面タイトル設定
            this.Title = ComDefine.TITLE_P0200010;

            // 出荷区分は本体固定
            this._shukkaFlag = SHUKKA_FLAG.NORMAL_VALUE1;
            this._nonyusakiName = nonyusakiName;
            this._ship = ship;
            this._gamenFlag = gamenFlag;
            this._isInitSearch = isInitSearch;
            this._isAllShip = isAllShip;
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="shukkaFlag">出荷区分</param>
        /// <param name="nonyusakiName">納入先</param>
        /// <param name="ship">出荷便</param>
        /// <param name="tagship">TAG便</param>
        /// <param name="gamenFlag">画面区分</param>
        /// <param name="isInitSearch">初期検索を行うかどうか</param>
        /// <param name="isAllShip">全便かどうか</param>
        /// <create>T.SASAYAMA 2023/06/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public NonyusakiIchiranEx(UserInfo userInfo, string shukkaFlag, string nonyusakiName, string ship, string tagShip, string gamenFlag, bool isInitSearch, bool isAllShip)
            : base(userInfo)
        {
            InitializeComponent();
            // 画面タイトル設定
            this.Title = ComDefine.TITLE_P0200010;

            // 出荷区分は本体固定
            this._shukkaFlag = SHUKKA_FLAG.NORMAL_VALUE1;
            this._nonyusakiName = nonyusakiName;
            this._ship = ship;
            this._tagShip = tagShip;
            this._gamenFlag = gamenFlag;
            this._isInitSearch = isInitSearch;
            this._isAllShip = isAllShip;
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>H.Tajimi 2015/12/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // コントロールの初期化
                // アイコンの設定
                this.Icon = ComFunc.BitmapToIcon(ComResource.Search);
                // 納入先
                this.txtNonyusakiName.Text = this._nonyusakiName;

                // 便
                this.txtShip.Text = this._ship;

                // 出荷区分
                this.MakeCmbBox(this.cboShukkaFlag, SHUKKA_FLAG.GROUPCD);
                this.cboShukkaFlag.SelectedValue = this._shukkaFlag;
                if (this._shukkaFlag == SHUKKA_FLAG.AR_VALUE1)
                {
                    this.txtShip.Text = string.Empty;
                    this.txtShip.Enabled = false;
                }
                this.lblShukkaFlag.Visible = false;

                // 管理区分
                this.MakeCmbBox(this.cboKanriFlag, KANRI_FLAG.GROUPCD, true);
                if (!string.IsNullOrEmpty(this._gamenFlag))
                {
                    this.lblKanriFlag.Visible = false;
                }

                // シートの設定
                this.shtResult.EditType = EditType.Default;
                this.shtResult.AllowUserToAddRows = false;
                this.shtResult.AllowUserToDeleteRows = false;
                this.shtResult.AllowClipboard = false;
                int colIndex = 0;
                // シートの列設定
                // 選択
                this.SetElTabelleColumnCheckBox(colIndex, Resources.NonyusakiIchiranEx_Choice, ComDefine.FLD_SELECT_CHK, 30, true);
                colIndex++;
                //納入先
                this.SetElTabelleColumnText(colIndex, Resources.NonyusakiIchiranEx_DeliveryDestination, Def_M_NONYUSAKI.NONYUSAKI_NAME, 360, false);
                colIndex++;
                //便
                this.SetElTabelleColumnText(colIndex, Resources.NonyusakiIchiranEx_Ship, Def_M_NONYUSAKI.SHIP, 70, false);
                colIndex++;
                //納入先コード
                this.SetElTabelleColumnText(colIndex, Resources.NonyusakiIchiranEx_ManagementNo, Def_M_NONYUSAKI.NONYUSAKI_CD, 50, false);
                colIndex++;
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
        /// <create>H.Tajimi 2015/12/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカス
                this.txtNonyusakiName.Focus();

                var initSearchMode = InitSearchMode.NONE;
                if (this._shukkaFlag == SHUKKA_FLAG.AR_VALUE1)
                {
                    // ARの場合は、納入先または便入力時は条件検索。それ以外は全件検索
                    if (this._isInitSearch && (!string.IsNullOrEmpty(this._nonyusakiName) || !string.IsNullOrEmpty(this._ship)))
                    {
                        // 条件検索
                        initSearchMode = InitSearchMode.CONDITION_SEARCH;
                    }
                    else
                    {
                        // 全件検索
                        initSearchMode = InitSearchMode.ALL_SEARCH;
                    }
                }
                else
                {
                    // 本体の場合は、納入先または便入力時は条件検索。それ以外は何もしない
                    // STEP16 TAG便入力時にも条件検索を行うよう変更。
                    if (this._isInitSearch && (!string.IsNullOrEmpty(this._nonyusakiName) || !string.IsNullOrEmpty(this._ship) || !string.IsNullOrEmpty(this._tagShip)))
                    {
                        // 条件検索
                        initSearchMode = InitSearchMode.CONDITION_SEARCH;
                    }
                }

                switch (initSearchMode)
                {
                    case InitSearchMode.ALL_SEARCH:
                        this.IsSearchAll = true;
                        this.RunSearch();
                        break;

                    case InitSearchMode.CONDITION_SEARCH:
                        this.IsSearchAll = false;
                        var ret = this.RunSearch();
                        if (ret && this._isInitSearch)
                        {
                            var ds = this.shtResult.DataSource as DataSet;
                            if (ds != null && ds.Tables.Contains(this.shtResult.DataMember) && ds.Tables[this.shtResult.DataMember].Rows.Count > 0)
                            {
                                var dt = ds.Tables[this.shtResult.DataMember];
                                if (this._isAllShip)
                                {
                                    var bukkenNo = UtilData.GetFld(dt, 0, Def_M_BUKKEN.BUKKEN_NO);
                                    if (!dt.AsEnumerable().Any(x => ComFunc.GetFld(x, Def_M_BUKKEN.BUKKEN_NO) != bukkenNo))
                                    {
                                        // 全ての物件NOが同じ場合は
                                        foreach (DataRow dr in dt.Rows)
                                        {
                                            dr[ComDefine.FLD_SELECT_CHK] = true;
                                        }
                                        this.btnSelect.PerformClick();
                                    }
                                }
                                else
                                {
                                    if (dt.Rows.Count == 1)
                                    {
                                        UtilData.SetFld(dt, 0, ComDefine.FLD_SELECT_CHK, true);
                                        this.btnSelect.PerformClick();
                                    }
                                }
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
            }
            finally
            {
                this._isInitSearch = false;
                this._isAllShip = false;
            }
        }

        #endregion

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>H.Tajimi 2015/12/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
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
        /// 検索処理制御部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>H.Tajimi 2015/12/07</create>
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
        /// <create>H.Tajimi 2015/12/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                this.shtResult.DataSource = null;
                this.shtResult.MaxRows = 0;
                CondNonyusaki cond = new CondNonyusaki(this.UserInfo);
                // 共通
                cond.ShukkaFlag = this._shukkaFlag;
                cond.TagShip = this._tagShip;

                if (!this.IsSearchAll)
                {
                    // 検索の場合
                    cond.KanriFlag = this.cboKanriFlag.SelectedValue.ToString();
                    cond.NonyusakiName = this.txtNonyusakiName.Text;
                    if (this._shukkaFlag == SHUKKA_FLAG.NORMAL_VALUE1)
                    {
                        cond.Ship = this.txtShip.Text;
                    }
                }
                if (!string.IsNullOrEmpty(this._gamenFlag))
                {
                    cond.GamenFlag = this._gamenFlag;
                }

                ConnP02 conn = new ConnP02();
                DataSet ds = string.IsNullOrEmpty(this._gamenFlag) ? conn.GetNonyusakiIchiran(cond) : conn.GetRirekiBukkenIchiran(cond);

                if (!ds.Tables.Contains(Def_M_NONYUSAKI.Name) || ds.Tables[Def_M_NONYUSAKI.Name].Rows.Count < 1)
                {
                    // 該当する納入先はありません。
                    this.ShowMessage("P0200010001");
                    return false;
                }
                else
                {
                    // 選択チェックボックス用の列を追加して、グリッドにセットする
                    var newDs = ds.Clone();
                    newDs.Tables[Def_M_NONYUSAKI.Name].Columns.Add(ComDefine.FLD_SELECT_CHK, typeof(bool));
                    foreach (DataRow drSource in ds.Tables[Def_M_NONYUSAKI.Name].Rows)
                    {
                        var drDest = newDs.Tables[Def_M_NONYUSAKI.Name].NewRow();
                        // 取得データを新しいテーブルにコピー
                        foreach (DataColumn column in ds.Tables[Def_M_NONYUSAKI.Name].Columns)
                        {
                            drDest[column.ColumnName] = drSource[column.ColumnName];
                        }
                        drDest[ComDefine.FLD_SELECT_CHK] = DEFAULT_SELECT_VALUE;
                        newDs.Tables[Def_M_NONYUSAKI.Name].Rows.Add(drDest);
                    }
                    this.shtResult.DataMember = Def_M_NONYUSAKI.Name;
                    this.shtResult.DataSource = newDs;
                }
                this.shtResult.Focus();
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

        #region テキストボックス

        /// --------------------------------------------------
        /// <summary>
        /// 納入先(User)のキーダウン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2015/12/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void txtNonyusakiName_KeyDown(object sender, KeyEventArgs e)
        {
            // Enterキーが押下された時は、検索ボタンにフォーカス遷移する
            if ((e.KeyCode == Keys.Enter) && !e.Alt && !e.Control)
            {
                this.btnSearch.Focus();
            }
        }

        #endregion

        #region 選択

        /// --------------------------------------------------
        /// <summary>
        /// 選択ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2015/12/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                var ds = this.shtResult.DataSource as DataSet;
                if (ds == null || !ds.Tables.Contains(this.shtResult.DataMember))
                {
                    // 検索結果の行が選択されていません。
                    this.ShowMessage("FW010050001");
                }

                var dtResult = ds.Tables[this.shtResult.DataMember];
                if (dtResult.Rows.Count < 1)
                {
                    // 検索結果の行が選択されていません。
                    this.ShowMessage("FW010050001");
                }
                else
                {
                    // チェックされている行を取得
                    var drcChecked = dtResult.AsEnumerable().Where(x => ComFunc.GetFldToBool(x, ComDefine.FLD_SELECT_CHK) == true);
                    if (drcChecked.Count() < 1)
                    {
                        // 検索結果の行が選択されていません。
                        this.ShowMessage("FW010050001");
                    }
                    else
                    {
                        // チェックされている行に異なる物件NOが含まれていないかどうか
                        var bukkenNo = drcChecked.Select(x => ComFunc.GetFld(x, Def_M_BUKKEN.BUKKEN_NO)).FirstOrDefault();
                        var rowIndex = this.GetSelectedDifferentBukkenNo(dtResult, bukkenNo);
                        if (rowIndex > 0)
                        {
                            // {0}行目に異なる納入先が選択されています。
                            this.ShowMessage("P0200060001", (rowIndex + 1).ToString());
                            this.shtResult.ActivePosition = new Position(COL_SELECT_CHK, rowIndex);
                            this.shtResult.Focus();
                        }
                        else
                        {
                            this.SelectedRowsData = drcChecked;
                            this.DialogResult = DialogResult.OK;
                            this.Close();
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

        #endregion

        #region Sheetの列設定

        /// --------------------------------------------------
        /// <summary>
        /// テキスト用列設定
        /// </summary>
        /// <remarks>
        /// Enabledを設定すると文字色がグレーアウトされるため
        /// ReadOnlyとLockプロパティで変更不可にする
        /// </remarks>
        /// <param name="colIndex">列のインデックス</param>
        /// <param name="title">列ヘッダタイトル</param>
        /// <param name="dataField">データフィールド</param>
        /// <param name="width">列幅</param>
        /// <param name="isEnabled">列が操作可能かどうか</param>
        /// <create>H.Tajimi 2015/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetElTabelleColumnText(int colIndex, string title, string dataField, int width, bool isEnabled)
        {
            TextEditor editor = ElTabelleSheetHelper.NewTextEditor();
            editor.ReadOnly = !isEnabled;
            this.SetElTabelleColumn(this.shtResult, colIndex, title, false, true, dataField, editor, width);
            this.shtResult.Columns[colIndex].Lock = !isEnabled;
        }

        /// --------------------------------------------------
        /// <summary>
        /// チェックボックス型用列設定
        /// </summary>
        /// <param name="colIndex">列のインデックス</param>
        /// <param name="title">列ヘッダタイトル</param>
        /// <param name="dataField">データフィールド</param>
        /// <param name="width">列幅</param>
        /// <param name="isEnabled">列が操作可能かどうか</param>
        /// <create>H.Tajimi 2015/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetElTabelleColumnCheckBox(int colIndex, string title, string dataField, int width, bool isEnabled)
        {
            CheckBoxEditor editor = ElTabelleSheetHelper.NewCheckBokEditor();
            editor.Text = null;
            this.SetElTabelleColumn(this.shtResult, colIndex, title, false, true, dataField, editor, width);
            this.shtResult.Columns[colIndex].AlignHorizontal = AlignHorizontal.Center;
            this.shtResult.Columns[colIndex].AlignVertical = AlignVertical.Middle;
            this.shtResult.Columns[colIndex].Lock = false;
            this.shtResult.Columns[colIndex].Enabled = isEnabled;
        }

        #endregion

        #region 異なる物件NOが選択されていないかチェック

        /// --------------------------------------------------
        /// <summary>
        /// 指定した物件NOと異なる物件NOの行インデックスを取得
        /// </summary>
        /// <param name="dt">検索元のDataTable</param>
        /// <param name="bukkenNo">物件NO</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2015/12/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private int GetSelectedDifferentBukkenNo(DataTable dt, string bukkenNo)
        {
            int ret = 0;
            for (int rowIndex = 0; rowIndex <= dt.Rows.Count - 1; rowIndex++)
            {
                if (ComFunc.GetFldToBool(dt.Rows[rowIndex], ComDefine.FLD_SELECT_CHK))
                {
                    if (ComFunc.GetFld(dt.Rows[rowIndex], Def_M_BUKKEN.BUKKEN_NO) != bukkenNo)
                    {
                        return rowIndex;
                    }
                }
            }
            return ret;
        }

        #endregion
    }
}
