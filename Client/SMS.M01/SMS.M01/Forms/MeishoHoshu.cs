using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

using WsConnection.WebRefM01;
using WsConnection.WebRefMaster;
using SMS.M01.Properties;

namespace SMS.M01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 名称マスタ保守
    /// </summary>
    /// <create>T.Sakiori 2012/04/05</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class MeishoHoshu : SystemBase.Forms.CustomOrderForm
    {
        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// 画面の表示モード
        /// </summary>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DisplayMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 初期
            /// </summary>
            /// <create>T.Sakiori 2012/04/05</create>
            /// <update></update>
            /// --------------------------------------------------
            Initialize,
            /// --------------------------------------------------
            /// <summary>
            /// 検索後
            /// </summary>
            /// <create>T.Sakiori 2012/04/05</create>
            /// <update></update>
            /// --------------------------------------------------
            EndSearch,
            /// --------------------------------------------------
            /// <summary>
            /// 登録
            /// </summary>
            /// <create>T.Sakiori 2012/04/05</create>
            /// <update></update>
            /// --------------------------------------------------
            Insert,
            /// --------------------------------------------------
            /// <summary>
            /// 変更
            /// </summary>
            /// <create>T.Sakiori 2012/04/05</create>
            /// <update></update>
            /// --------------------------------------------------
            Update,
            /// --------------------------------------------------
            /// <summary>
            /// 削除
            /// </summary>
            /// <create>T.Sakiori 2012/04/05</create>
            /// <update></update>
            /// --------------------------------------------------
            Delete,
        }

        /// --------------------------------------------------
        /// <summary>
        /// データ選択時の状態
        /// </summary>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DataSelectType
        {
            /// --------------------------------------------------
            /// <summary>
            /// 登録
            /// </summary>
            /// <create>T.Sakiori 2012/04/05</create>
            /// <update></update>
            /// --------------------------------------------------
            Insert,
            /// --------------------------------------------------
            /// <summary>
            /// 変更
            /// </summary>
            /// <create>T.Sakiori 2012/04/05</create>
            /// <update></update>
            /// --------------------------------------------------
            Update,
            /// --------------------------------------------------
            /// <summary>
            /// 削除
            /// </summary>
            /// <create>T.Sakiori 2012/04/05</create>
            /// <update></update>
            /// --------------------------------------------------
            Delete,
        }

        #endregion

        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 0;
        /// --------------------------------------------------
        /// <summary>
        /// 名称の列インデックス
        /// </summary>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ITEM_NAME = 0;
        /// --------------------------------------------------
        /// <summary>
        /// 選択グループCDの列インデックス
        /// </summary>
        /// <create>T.Sakiori 2012/04/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SELECT_GROUP_CD = 1;

        #endregion

        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 表示時に使用したデータ
        /// </summary>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtDispData = null;

        /// --------------------------------------------------
        /// <summary>
        /// 選択グループコード
        /// </summary>
        /// <create>H.Tsuji 2018/12/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtSelectGroupData = null;

        /// --------------------------------------------------
        /// <summary>
        /// コントロール初期化終了フラグ
        /// </summary>
        /// <create>H.Tsuji 2018/12/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _initializeCtrlFinished = false;

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
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public MeishoHoshu(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update>H.Tsuji 2018/11/30 種類コンボボックスに変更</update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();
            try
            {
                // フォームの設定
                this.IsCloseQuestion = true;
                this.IsRunEditAfterClear = false;
                // シートの初期化
                this.InitializeSheet(this.shtResult);
                this.shtResult.KeepHighlighted = true;

                // シートのタイトルを設定
                shtResult.ColumnHeaders[0].Caption = Resources.MeishoHoshu_Name;

                // コンボボックスの初期化
                this.MakeCmbBox(this.cboSearchType, SELECT_GROUP_CD.GROUPCD);
                this.MakeCmbBox(this.cboType, SELECT_GROUP_CD.GROUPCD);

                // モード切り替え
                this.ChangeMode(DisplayMode.Initialize);
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
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update>H.Tsuji 2018/11/30 種類コンボボックスに変更</update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                this.cboSearchType.Focus();
                this._initializeCtrlFinished = true;
                
                // 種類コンボボックス最大桁数用のテーブルセット
                this._dtSelectGroupData = this.GetCommon(SELECT_GROUP_CD.GROUPCD).Tables[Def_M_COMMON.Name];

                // 名称(検索・登録)の初期化
                this.txtItemName.MaxLength = ComFunc.GetFldToInt32(this._dtSelectGroupData, 0, Def_M_COMMON.VALUE2);
                this.txtItemName.Text = string.Empty;
                this.txtRegItemName.MaxLength = ComFunc.GetFldToInt32(this._dtSelectGroupData, 0, Def_M_COMMON.VALUE2);
                this.txtRegItemName.Text = string.Empty;
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
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update>H.Tsuji 2018/11/30 種類コンボボックスに変更</update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                this.cboSearchType.SelectedValue = SELECT_GROUP_CD.DEFAULT_VALUE1;
                this.txtItemName.Text = string.Empty;
                // グリッド
                this.SheetClear();
                // 登録情報部分のクリア
                this.DisplayClearEdit();
                // モード切り替え
                this.ChangeMode(DisplayMode.Initialize);
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
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update>H.Tsuji 2018/11/30 種類コンボボックスに変更</update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                // 名称チェック
                if (string.IsNullOrEmpty(this.txtRegItemName.Text))
                {
                    // 名称を入力してください。
                    this.ShowMessage("M0100070003");
                    this.txtRegItemName.Focus();
                    return false;
                }
                if (Encoding.GetEncoding("Shift-JIS").GetByteCount(this.txtRegItemName.Text) > this.txtRegItemName.MaxLength)
                {
                    // {0}には{1}Byte以下で入力して下さい。
                    string[] param = new string[] { this.lblRegItemName.Text, this.txtRegItemName.MaxLength.ToString() };
                    this.ShowMessage("A9999999052", param);
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

        /// --------------------------------------------------
        /// <summary>
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
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
        /// <create>T.Sakiori 2012/04/05</create>
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
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update>H.Tsuji 2018/11/30 種類コンボボックスに変更</update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                // モード切り替え(初期状態に戻す)
                this.ChangeMode(DisplayMode.Initialize);
                var conn = new ConnM01();
                var cond = new CondM01(this.UserInfo);
                cond.SelectGroupCD = this.cboSearchType.SelectedValue.ToString();
                cond.ItemName = this.txtItemName.Text;
                DataSet ds = conn.GetSelectItem(cond);

                if (!ComFunc.IsExistsData(ds, Def_M_SELECT_ITEM.Name))
                {
                    // シートのクリア
                    this.SheetClear();
                    // 該当名称はありません。
                    this.ShowMessage("M0100070001");
                    return false;
                }

                this.shtResult.DataSource = ds;
                this.shtResult.DataMember = Def_M_SELECT_ITEM.Name;
                // モード切り替え
                this.ChangeMode(DisplayMode.EndSearch);
                this.shtResult.Enabled = true;
                // 最も左上に表示されているセルの設定
                if (0 < this.shtResult.MaxRows)
                {
                    this.shtResult.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtResult.TopLeft.Row);
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

        #region 編集内容実行

        #region 制御メソッド

        /// --------------------------------------------------
        /// <summary>
        /// 編集内容実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            try
            {
                bool ret = base.RunEdit();
                if (ret)
                {
                    this.DisplayClearEdit();
                    // とりあえず検索
                    this.RunSearch();
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
        /// <create>T.Sakiori 2012/04/06</create>
        /// <update>H.Tsuji 2018/11/30 種類コンボボックスに変更</update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.SelectGroupCD = this.cboType.SelectedValue.ToString();
                cond.ItemName = this.txtRegItemName.Text;

                DataSet ds = new DataSet();
                ds.Tables.Add(this.SetEditData(this._dtDispData));

                string errMsgID;
                string[] args;
                if (!conn.InsSelectItemData(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID))
                    {
                        this.DisplayClearEdit();
                        this.RunSearch();
                    }
                    else if (errMsgID == "M0100070004")
                    {
                        this.RunSearch();
                        this.ChangeMode(DisplayMode.Insert);
                        this.txtRegItemName.Focus();
                    }
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

        #region 修正処理

        /// --------------------------------------------------
        /// <summary>
        /// 修正処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.SelectGroupCD = ComFunc.GetFld(this._dtDispData, 0, Def_M_SELECT_ITEM.SELECT_GROUP_CD);
                cond.ItemName = ComFunc.GetFld(this._dtDispData, 0, Def_M_SELECT_ITEM.ITEM_NAME);
                //下記のような VERSION の渡し方は、標準時間関係の情報が消えてしまうので行ってはいけない
                //cond.Version = ComFunc.GetFldToDateTime(this._dtDispData, 0, Def_M_SELECT_ITEM.VERSION).ToString("yyyy/MM/dd HH:mm:ss.fffffff");

                DataSet ds = new DataSet();
                ds.Tables.Add(this.SetEditData(this._dtDispData));
                UtilData.SetFld(ds, Def_M_SELECT_ITEM.Name, 0, Def_M_SELECT_ITEM.VERSION, ComFunc.GetFldToDateTime(this._dtDispData, 0, Def_M_SELECT_ITEM.VERSION));

                string errMsgID;
                string[] args;
                if (!conn.UpdSelectItemData(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID)
                        || errMsgID == "M0100070005")
                    {
                        this.DisplayClearEdit();
                        this.RunSearch();
                    }
                    else if (errMsgID == "M0100070004")
                    {
                        this.RunSearch();
                        var dt = (this.shtResult.DataSource as DataSet).Tables[Def_M_SELECT_ITEM.Name];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (cond.SelectGroupCD == ComFunc.GetFld(dt, i, Def_M_SELECT_ITEM.SELECT_GROUP_CD)
                                && cond.ItemName == ComFunc.GetFld(dt, i, Def_M_SELECT_ITEM.ITEM_NAME))
                            {
                                this.shtResult.TopLeft = new Position(0, i);
                                this.shtResult.ActivePosition = new Position(0, i);
                            }
                        }
                        this.ChangeMode(DisplayMode.Update);
                        this.txtRegItemName.Focus();
                    }
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

        #region 削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditDelete()
        {
            try
            {
                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.SelectGroupCD = ComFunc.GetFld(this._dtDispData, 0, Def_M_SELECT_ITEM.SELECT_GROUP_CD);
                cond.ItemName = ComFunc.GetFld(this._dtDispData, 0, Def_M_SELECT_ITEM.ITEM_NAME);
                //下記のような VERSION の渡し方は、標準時間関係の情報が消えてしまうので行ってはいけない
                //cond.Version = ComFunc.GetFldToDateTime(this._dtDispData, 0, Def_M_SELECT_ITEM.VERSION).ToString("yyyy/MM/dd HH:mm:ss.fffffff");

                DataSet ds = new DataSet();
                ds.Tables.Add(this.SetEditData(this._dtDispData));
                UtilData.SetFld(ds, Def_M_SELECT_ITEM.Name, 0, Def_M_SELECT_ITEM.VERSION, ComFunc.GetFldToDateTime(this._dtDispData, 0, Def_M_SELECT_ITEM.VERSION));

                string errMsgID;
                string[] args;
                if (!conn.DelSelectItemData(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID)
                        || errMsgID == "M0100070005")
                    {
                        this.DisplayClearEdit();
                        this.RunSearch();
                    }
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

        #endregion

        #region イベント

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F7ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2015/12/08</create>
        /// <update>H.Tsuji 2018/11/30 種類コンボボックスに変更</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                // 画面をクリアします。\r\nよろしいですか？
                if (ShowMessage("A9999999001") != DialogResult.OK) return;
                this.DisplayClear();
                this.cboSearchType.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 検索ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 検索ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnSearch_Click(object sender, EventArgs e)
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

        #region 登録ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 登録ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                this.RunDataSelect(DataSelectType.Insert);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 変更ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 変更ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                this.RunDataSelect(DataSelectType.Update);
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
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                this.RunDataSelect(DataSelectType.Delete);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 種類コンボボックス

        /// --------------------------------------------------
        /// <summary>
        /// 種類コンボボックス(登録)切替
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsuji 2018/12/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboSearchType_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (this._initializeCtrlFinished)
                {
                    this.txtItemName.Text = string.Empty;
                    this.txtItemName.MaxLength = this.GetMeishoMaxLength(this.cboSearchType.SelectedValue.ToString());
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 種類コンボボックス(登録)切替
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsuji 2018/12/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboType_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (this._initializeCtrlFinished)
                {
                    this.txtRegItemName.Text = string.Empty;
                    this.txtRegItemName.MaxLength = this.GetMeishoMaxLength(this.cboType.SelectedValue.ToString());
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #endregion

        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            // グリッドクリア
            this.shtResult.Redraw = false;
            // 最も左上に表示されているセルの設定
            if (0 < this.shtResult.MaxRows)
            {
                this.shtResult.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtResult.TopLeft.Row);
            }
            this.shtResult.DataSource = null;
            this.shtResult.MaxRows = 0;
            this.shtResult.Enabled = false;
            this.shtResult.Redraw = true;
        }

        #endregion

        #region モード切り替え

        /// --------------------------------------------------
        /// <summary>
        /// モード切替
        /// </summary>
        /// <param name="mode">画面の表示モード</param>
        /// <create>T.Sakiori 2012/04/05</create>
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
                        this.btnInsert.Enabled = true;
                        this.btnUpdate.Enabled = false;
                        this.btnDelete.Enabled = false;
                        this.grpEdit.Enabled = false;
                        this.fbrFunction.F01Button.Enabled = false;
                        break;
                    case DisplayMode.EndSearch:
                        // ----- 検索後 -----
                        this.grpSearch.Enabled = true;
                        this.btnInsert.Enabled = true;
                        this.btnUpdate.Enabled = true;
                        this.btnDelete.Enabled = true;
                        this.grpEdit.Enabled = false;
                        this.fbrFunction.F01Button.Enabled = false;
                        break;
                    case DisplayMode.Insert:
                        // ----- 登録 -----
                        this.grpSearch.Enabled = false;
                        this.grpEdit.Enabled = true;
                        this.cboType.Enabled = true;
                        this.txtRegItemName.Enabled = true;
                        this.fbrFunction.F01Button.Enabled = true;
                        break;
                    case DisplayMode.Update:
                        // ----- 変更 -----
                        this.grpSearch.Enabled = false;
                        this.grpEdit.Enabled = true;
                        this.cboType.Enabled = false;
                        this.txtRegItemName.Enabled = true;
                        this.fbrFunction.F01Button.Enabled = true;
                        break;
                    case DisplayMode.Delete:
                        // ----- 削除 -----
                        this.grpSearch.Enabled = false;
                        this.grpEdit.Enabled = true;
                        this.cboType.Enabled = false;
                        this.txtRegItemName.Enabled = false;
                        this.fbrFunction.F01Button.Enabled = true;
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

        #region 登録情報クリア

        /// --------------------------------------------------
        /// <summary>
        /// 登録情報クリア
        /// </summary>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void DisplayClearEdit()
        {
            try
            {
                this.cboType.SelectedValue = SELECT_GROUP_CD.DEFAULT_VALUE1;
                this.txtRegItemName.Text = string.Empty;
                this.txtRegItemName.MaxLength = this.GetMeishoMaxLength(SELECT_GROUP_CD.DEFAULT_VALUE1);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region データ選択

        /// --------------------------------------------------
        /// <summary>
        /// データ選択時のチェック
        /// </summary>
        /// <param name="selectType">データ選択時の状態</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckInputDataSelect(DataSelectType selectType)
        {
            try
            {
                if (this.shtResult.ActivePosition.Row < 0
                    && selectType != DataSelectType.Insert)
                {
                    // データが選択されていません。
                    this.ShowMessage("A9999999019");
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

        /// --------------------------------------------------
        /// <summary>
        /// データ選択制御部
        /// </summary>
        /// <param name="selectType">データ選択時の状態</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update>H.Tsuji 2018/11/30 種類コンボボックスに変更</update>
        /// --------------------------------------------------
        private bool RunDataSelect(DataSelectType selectType)
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                if (this.CheckInputDataSelect(selectType))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    bool ret = this.RunDataSelectExec(selectType);

                    if (ret)
                    {
                        switch (selectType)
                        {
                            case DataSelectType.Insert:
                                this.EditMode = SystemBase.EditMode.Insert;
                                // モード切り替え
                                this.ChangeMode(DisplayMode.Insert);
                                this.cboType.Focus();
                                break;
                            case DataSelectType.Update:
                                this.EditMode = SystemBase.EditMode.Update;
                                // モード切り替え
                                this.ChangeMode(DisplayMode.Update);
                                this.txtRegItemName.Focus();
                                break;
                            case DataSelectType.Delete:
                                this.EditMode = SystemBase.EditMode.Delete;
                                // モード切り替え
                                this.ChangeMode(DisplayMode.Delete);
                                this.fbrFunction.F01Button.Focus();
                                break;
                            default:
                                break;
                        }
                    }

                    return ret;
                }
                return false;
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
        }

        /// --------------------------------------------------
        /// <summary>
        /// データ選択実行部
        /// </summary>
        /// <param name="selectType">データ選択時の状態</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update>H.Tsuji 2018/11/30 種類コンボボックスに変更</update>
        /// --------------------------------------------------
        private bool RunDataSelectExec(DataSelectType selectType)
        {
            try
            {
                if (selectType == DataSelectType.Insert)
                {
                    // ----- 登録 -----
                    this._dtDispData = this.GetSchemeSelectItem();
                    // 1行追加しておく
                    this._dtDispData.Rows.Add(this._dtDispData.NewRow());
                }
                else
                {
                    // ----- 変更、削除 -----
                    var conn = new ConnM01();
                    var cond = new CondM01(this.UserInfo);
                    cond.ItemName = this.shtResult[SHEET_COL_ITEM_NAME, this.shtResult.ActivePosition.Row].Text;
                    cond.SelectGroupCD = this.shtResult[SHEET_COL_SELECT_GROUP_CD, this.shtResult.ActivePosition.Row].Text;
                    DataSet ds = conn.GetSelectItem(cond);
                    if (!ComFunc.IsExistsData(ds, Def_M_SELECT_ITEM.Name))
                    {
                        // 他端末で削除されています。
                        this.ShowMessage("A9999999026");
                        // 消えてるのがあったから取り敢えず検索しとけ
                        this.RunSearch();
                        return false;
                    }
                    this._dtDispData = ds.Tables[Def_M_SELECT_ITEM.Name];
                }

                // 表示データ設定
                string typeSelectedValue = ComFunc.GetFld(this._dtDispData, 0, Def_M_SELECT_ITEM.SELECT_GROUP_CD);
                if (!string.IsNullOrEmpty(typeSelectedValue))
                {
                    this.cboType.SelectedValue = typeSelectedValue;
                }
                this.txtRegItemName.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_SELECT_ITEM.ITEM_NAME);
                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region 名称マスタのデータテーブル作成

        /// --------------------------------------------------
        /// <summary>
        /// 名称マスタのデータテーブル作成
        /// </summary>
        /// <returns>名称マスタのデータテーブル</returns>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSchemeSelectItem()
        {
            try
            {
                var dt = new DataTable(Def_M_SELECT_ITEM.Name);
                dt.Columns.Add(Def_M_SELECT_ITEM.SELECT_GROUP_CD, typeof(string));
                dt.Columns.Add(Def_M_SELECT_ITEM.ITEM_NAME, typeof(string));
                dt.Columns.Add(Def_M_SELECT_ITEM.VERSION, typeof(object));
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 登録データの取得

        /// --------------------------------------------------
        /// <summary>
        /// 登録データの取得
        /// </summary>
        /// <param name="dt">名称マスタデータテーブル</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update>H.Tsuji 2018/11/30 種類コンボボックスに変更</update>
        /// --------------------------------------------------
        private DataTable SetEditData(DataTable dt)
        {
            try
            {
                DataTable ret = dt.Clone();
                DataRow dr = ret.NewRow();
                dr[Def_M_SELECT_ITEM.SELECT_GROUP_CD] = this.cboType.SelectedValue;
                dr[Def_M_SELECT_ITEM.ITEM_NAME] = this.txtRegItemName.Text;
                ret.Rows.Add(dr);
                return ret;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return null;
            }
        }

        #endregion

        #region 名称の最大桁数の取得

        /// --------------------------------------------------
        /// <summary>
        /// 名称の最大桁数の取得
        /// </summary>
        /// <param name="comValue1">汎用マスタ列値</param>
        /// <returns>名称の最大桁数</returns>
        /// <create>H.Tsuji 2018/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private int GetMeishoMaxLength(string comValue1)
        {
            try
            {
                int length = 0;
                if (this._initializeCtrlFinished)
                {
                    string[] args = { Def_M_COMMON.VALUE1, comValue1 };
                    DataRow[] dRows = this._dtSelectGroupData.Select(string.Format("{0} = '{1}'", args));
                    length = ComFunc.GetFldToInt32(dRows[0], Def_M_COMMON.VALUE2);
                }
                return length;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return 0;
            }
        }

        #endregion
    }
}
