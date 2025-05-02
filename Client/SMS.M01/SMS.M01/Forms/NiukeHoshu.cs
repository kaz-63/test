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
using GrapeCity.Win.ElTabelle;
using ElTabelleHelper;

using SystemBase.Util;
using SMS.M01.Properties;
using WsConnection.WebRefM01;
using WsConnection.WebRefMaster;

namespace SMS.M01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 荷受保守
    /// </summary>
    /// <create>H.Tsuji 2018/12/04</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class NiukeHoshu : SystemBase.Forms.CustomOrderForm
    {
        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// 画面の表示モード
        /// </summary>
        /// <create>H.Tsuji 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DisplayMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 初期
            /// </summary>
            /// <create>H.Tsuji 2018/12/04</create>
            /// <update></update>
            /// --------------------------------------------------
            Initialize,
            /// --------------------------------------------------
            /// <summary>
            /// 検索後
            /// </summary>
            /// <create>H.Tsuji 2018/12/04</create>
            /// <update></update>
            /// --------------------------------------------------
            EndSearch,
            /// --------------------------------------------------
            /// <summary>
            /// 登録
            /// </summary>
            /// <create>H.Tsuji 2018/12/04</create>
            /// <update></update>
            /// --------------------------------------------------
            Insert,
            /// --------------------------------------------------
            /// <summary>
            /// 変更
            /// </summary>
            /// <create>H.Tsuji 2018/12/04</create>
            /// <update></update>
            /// --------------------------------------------------
            Update,
            /// --------------------------------------------------
            /// <summary>
            /// 削除
            /// </summary>
            /// <create>H.Tsuji 2018/12/04</create>
            /// <update></update>
            /// --------------------------------------------------
            Delete,
        }

        /// --------------------------------------------------
        /// <summary>
        /// データ選択時の状態
        /// </summary>
        /// <create>H.Tsuji 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DataSelectType
        {
            /// --------------------------------------------------
            /// <summary>
            /// 登録
            /// </summary>
            /// <create>H.Tsuji 2018/12/04</create>
            /// <update></update>
            /// --------------------------------------------------
            Insert,
            /// --------------------------------------------------
            /// <summary>
            /// 変更
            /// </summary>
            /// <create>H.Tsuji 2018/12/04</create>
            /// <update></update>
            /// --------------------------------------------------
            Update,
            /// --------------------------------------------------
            /// <summary>
            /// 削除
            /// </summary>
            /// <create>H.Tsuji 2018/12/04</create>
            /// <update></update>
            /// --------------------------------------------------
            Delete,
            /// --------------------------------------------------
            /// <summary>
            /// メール設定
            /// </summary>
            /// <create>Y.Gwon 2023/07/05</create>
            /// <update></update>
            /// --------------------------------------------------
            Mail,
        }

        #endregion

        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>H.Tsuji 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 0;
        /// --------------------------------------------------
        /// <summary>
        /// 名称の列インデックス
        /// </summary>
        /// <create>H.Tsuji 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_CONSIGN_NAME = 0;
        /// --------------------------------------------------
        /// <summary>
        /// 荷受CDの列インデックス
        /// </summary>
        /// <create>H.Tsuji 2018/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_CONSIGN_CD = 0;

        #endregion

        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 表示時に使用したデータ
        /// </summary>
        /// <create>H.Tsuji 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtDispData = null;

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
        /// <create>H.Tsuji 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        public NiukeHoshu(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>H.Tsuji 2018/12/04</create>
        /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
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
                shtResult.ColumnHeaders[1].Caption = Resources.NiukeHoshu_Name;
                shtResult.ColumnHeaders[2].Caption = Resources.NiukeHoshu_Address;
                shtResult.ColumnHeaders[3].Caption = Resources.NiukeHoshu_Tel1;
                shtResult.ColumnHeaders[4].Caption = Resources.NiukeHoshu_Tel2;
                shtResult.ColumnHeaders[5].Caption = Resources.NiukeHoshu_Fax;
                shtResult.ColumnHeaders[6].Caption = Resources.NiukeHoshu_ChinaFlag;
                shtResult.ColumnHeaders[7].Caption = Resources.NiukeHoshu_UsciCd;
                shtResult.ColumnHeaders[8].Caption = Resources.NiukeHoshu_SortNo;

                // コンボボックスの初期化
                this.MakeCmbBox(this.cboChina, CHINA_FLAG.GROUPCD);

                // 住所欄の行数(ATTN用に一行確保する)
                this.txtAddress.MaxLineCount = ComDefine.MAX_LINE_LENGTH_CONSIGNEE - 1;

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
        /// <create>H.Tsuji 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                this.txtSearchName.Focus();
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
        /// <create>H.Tsuji 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                this.txtSearchName.Text = string.Empty;

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
        /// <create>H.Tsuji 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                // 名称入力チェック
                if (string.IsNullOrEmpty(this.txtName.Text))
                {
                    // 名称を入力して下さい。
                    this.ShowMessage("M0100140002");
                    this.txtName.Focus();
                    return false;
                }

                // 住所入力チェック
                if (string.IsNullOrEmpty(this.txtAddress.Text))
                {
                    // 住所を入力して下さい。
                    this.ShowMessage("M0100140003");
                    this.txtAddress.Focus();
                    return false;
                }

                // 登録行数チェック
                int lineLength = this.txtAddress.Lines.Length
                    + this.txtTel.Lines.Length
                    + this.txtTel2.Lines.Length
                    + this.txtFax.Lines.Length;
                if (lineLength >= ComDefine.MAX_LINE_LENGTH_CONSIGNEE)
                {
                    // {0}から{1}まで{2}行以内で設定して下さい。
                    string[] args = { this.lblAddress.Text, this.lblFax.Text, (ComDefine.MAX_LINE_LENGTH_CONSIGNEE - 1).ToString() };
                    this.ShowMessage("M0100140004", args);
                    this.txtAddress.Focus();
                    return false;
                }

                // USCI入力チェック
                if (this.cboChina.SelectedValue.ToString() == CHINA_FLAG.ON_VALUE1
                    && string.IsNullOrEmpty(this.txtUSCI.Text))
                {
                    // USCIを入力して下さい。
                    this.ShowMessage("M0100140005");
                    this.txtUSCI.Focus();
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
        /// <create>H.Tsuji 2018/12/04</create>
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
        /// <create>H.Tsuji 2018/12/04</create>
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
        /// <create>H.Tsuji 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                // モード切り替え(初期状態に戻す)
                this.ChangeMode(DisplayMode.Initialize);

                // 荷受情報の検索
                var conn = new ConnM01();
                var cond = new CondM01(this.UserInfo);
                cond.ConsignName = this.txtSearchName.Text;
                DataSet ds = conn.GetConsignLikeSearch(cond);
                if (!ComFunc.IsExistsData(ds, Def_M_CONSIGN.Name))
                {
                    // シートのクリア
                    this.SheetClear();
                    // 該当する荷受情報はありません。
                    this.ShowMessage("M0100140001");
                    return false;
                }

                this.shtResult.DataSource = ds;
                this.shtResult.DataMember = Def_M_CONSIGN.Name;
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
        /// <create>H.Tsuji 2018/12/04</create>
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
        /// <create>H.Tsuji 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                this.SetEditData(this._dtDispData);

                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.ConsignName = this.txtName.Text;

                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtDispData.Copy());

                string errMsgID;
                string[] args;
                if (!conn.InsConsignData(cond, ds, out errMsgID, out args))
                {
                    if (errMsgID == "A9999999027" || errMsgID == "A9999999062")
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

        #region 修正処理

        /// --------------------------------------------------
        /// <summary>
        /// 修正処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>H.Tsuji 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                this.SetEditData(this._dtDispData);

                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.ConsignCD = ComFunc.GetFld(this._dtDispData, 0, Def_M_CONSIGN.CONSIGN_CD);

                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtDispData.Copy());

                string errMsgID;
                string[] args;
                if (!conn.UpdConsignData(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID) || errMsgID == "M0100140006")
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

        #region 削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>H.Tsuji 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditDelete()
        {
            try
            {
                this.SetEditData(this._dtDispData);

                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.ConsignCD = ComFunc.GetFld(this._dtDispData, 0, Def_M_CONSIGN.CONSIGN_CD);

                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtDispData.Copy());

                string errMsgID;
                string[] args;
                if (!conn.DelConsignData(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID)
                        || errMsgID == "M0100140006"
                        || errMsgID == "M0100140007")
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
        /// F1ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsuji 2018/12/04</create>
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
        /// F7ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsuji 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                // 画面をクリアします。\r\nよろしいですか？
                if (ShowMessage("A9999999001") != DialogResult.OK) return;
                this.DisplayClear();
                this.txtSearchName.Focus();
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
        /// <create>H.Tsuji 2018/12/04</create>
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

        #region 検索ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 検索ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsuji 2018/12/05</create>
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
        /// <create>H.Tsuji 2018/12/05</create>
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
        /// <create>H.Tsuji 2018/12/05</create>
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
        /// <create>H.Tsuji 2018/12/05</create>
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

        #region 計画取込Mail通知先設定

        /// <summary>
        /// 計画取込Mail通知先設定ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Gwon 2023/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnKeikakuTorikomiNotify_Click(object sender, EventArgs e)
        {
            if (!this.RunDataSelect(DataSelectType.Mail))
            {
                return;
            }

            var para01 = ComFunc.GetFld(this._dtDispData, 0, Def_M_CONSIGN.NAME);
            var para02 = ComFunc.GetFld(this._dtDispData, 0, Def_M_CONSIGN.CONSIGN_CD);

            using (var f = new KeikakuTorikomiNotify(this.UserInfo, para01, para02))
            {
                f.ShowDialog();
            }

        }

        #endregion

        #region 中国向け

        /// --------------------------------------------------
        /// <summary>
        /// 中国向け
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsuji 2018/12/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboChina_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.ChangeChinaFlag();
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
        /// <create>H.Tsuji 2018/12/04</create>
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
        /// <create>H.Tsuji 2018/12/04</create>
        /// <update>K.Tsutsumi 2018/01/22 並び順</update>
        /// <update>Y.Gwon 2023/07/05 計画取込Mail通知先追加</update>
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
                        this.btnKeikakuTorikomiNotify.Enabled = false;
                        this.grpEdit.Enabled = false;
                        this.fbrFunction.F01Button.Enabled = false;
                        break;
                    case DisplayMode.EndSearch:
                        // ----- 検索後 -----
                        this.grpSearch.Enabled = true;
                        this.btnInsert.Enabled = true;
                        this.btnUpdate.Enabled = true;
                        this.btnDelete.Enabled = true;
                        this.btnKeikakuTorikomiNotify.Enabled = true;
                        this.grpEdit.Enabled = false;
                        this.fbrFunction.F01Button.Enabled = false;
                        break;
                    case DisplayMode.Insert:
                        // ----- 登録 -----
                        this.grpSearch.Enabled = false;
                        this.grpEdit.Enabled = true;
                        this.txtName.Enabled = true;
                        this.txtAddress.Enabled = true;
                        this.txtTel.Enabled = true;
                        this.txtTel2.Enabled = true;
                        this.txtFax.Enabled = true;
                        this.cboChina.Enabled = true;
                        this.txtUSCI.Enabled = (this.cboChina.SelectedValue.ToString() == CHINA_FLAG.ON_VALUE1);
                        this.numSortNo.Enabled = true;
                        this.fbrFunction.F01Button.Enabled = true;
                        break;
                    case DisplayMode.Update:
                        // ----- 変更 -----
                        this.grpSearch.Enabled = false;
                        this.grpEdit.Enabled = true;
                        this.txtName.Enabled = true;
                        this.txtAddress.Enabled = true;
                        this.txtTel.Enabled = true;
                        this.txtTel2.Enabled = true;
                        this.txtFax.Enabled = true;
                        this.cboChina.Enabled = true;
                        this.txtUSCI.Enabled = (this.cboChina.SelectedValue.ToString() == CHINA_FLAG.ON_VALUE1);
                        this.numSortNo.Enabled = true;
                        this.fbrFunction.F01Button.Enabled = true;
                        break;
                    case DisplayMode.Delete:
                        // ----- 削除 -----
                        this.grpSearch.Enabled = false;
                        this.grpEdit.Enabled = false;
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

        #region 中国向けの切替

        /// --------------------------------------------------
        /// <summary>
        /// 中国向け切替の切替
        /// </summary>
        /// <create>H.Tsuji 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeChinaFlag()
        {
            if (this.cboChina.SelectedValue.ToString() == CHINA_FLAG.OFF_VALUE1)
            {
                // ----- その他 -----
                this.txtUSCI.Enabled = false;
                this.txtUSCI.Text = string.Empty;
            }
            else
            {
                // ----- 中国向け -----
                this.txtUSCI.Enabled = true;
            }
        }

        #endregion

        #endregion

        #region 登録情報クリア

        /// --------------------------------------------------
        /// <summary>
        /// 登録情報クリア
        /// </summary>
        /// <create>H.Tsuji 2018/12/04</create>
        /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
        /// --------------------------------------------------
        private void DisplayClearEdit()
        {
            try
            {
                this.txtName.Text = string.Empty;
                this.txtAddress.Text = string.Empty;
                this.txtTel.Text = string.Empty;
                this.txtTel2.Text = string.Empty;
                this.txtFax.Text = string.Empty;
                this.cboChina.SelectedValue = CHINA_FLAG.DEFAULT_VALUE1;
                this.txtUSCI.Text = string.Empty;
                this.numSortNo.Clear();
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
        /// <create>H.Tsuji 2018/12/05</create>
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
        /// <create>H.Tsuji 2018/12/05</create>
        /// <update></update>
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
                                this.txtName.Focus();
                                break;
                            case DataSelectType.Update:
                                this.EditMode = SystemBase.EditMode.Update;
                                // モード切り替え
                                this.ChangeMode(DisplayMode.Update);
                                this.txtName.Focus();
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
        /// <create>H.Tsuji 2018/12/05</create>
        /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
        /// --------------------------------------------------
        private bool RunDataSelectExec(DataSelectType selectType)
        {
            try
            {
                if (selectType == DataSelectType.Insert)
                {
                    // ----- 登録 -----
                    this._dtDispData = this.GetSchemeConsign();
                    // 1行追加しておく
                    this._dtDispData.Rows.Add(this._dtDispData.NewRow());
                }
                else
                {
                    // ----- 変更、削除 -----
                    var conn = new ConnM01();
                    var cond = new CondM01(this.UserInfo);
                    cond.ConsignCD = this.shtResult[SHEET_COL_CONSIGN_CD, this.shtResult.ActivePosition.Row].Text;
                    DataSet ds = conn.GetConsign(cond);
                    if (!ComFunc.IsExistsData(ds, Def_M_CONSIGN.Name))
                    {
                        // 他端末で削除されています。
                        this.ShowMessage("A9999999026");
                        // 消えてるのがあったから取り敢えず検索しとけ
                        this.RunSearch();
                        return false;
                    }
                    this._dtDispData = ds.Tables[Def_M_CONSIGN.Name];
                }

                // 表示データ設定
                this.txtName.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_CONSIGN.NAME);
                this.txtAddress.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_CONSIGN.ADDRESS);
                this.txtTel.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_CONSIGN.TEL1);
                this.txtTel2.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_CONSIGN.TEL2);
                this.txtFax.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_CONSIGN.FAX);
                string chinaFlag = ComFunc.GetFld(this._dtDispData, 0, Def_M_CONSIGN.CHINA_FLAG);
                if (string.IsNullOrEmpty(chinaFlag))
                {
                    this.cboChina.SelectedValue = CHINA_FLAG.DEFAULT_VALUE1;
                }
                else
                {
                    this.cboChina.SelectedValue = chinaFlag;
                }
                this.txtUSCI.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_CONSIGN.USCI_CD);
                this.numSortNo.Value = ComFunc.GetFldToDecimal(this._dtDispData, 0, Def_M_CONSIGN.SORT_NO);

                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region 荷受マスタのデータテーブル作成

        /// --------------------------------------------------
        /// <summary>
        /// 荷受マスタのデータテーブル作成
        /// </summary>
        /// <returns>荷受マスタのデータテーブル</returns>
        /// <create>H.Tsuji 2018/12/05</create>
        /// <update>K.Tsutsumi 2018/01/22 並び順</update>
        /// --------------------------------------------------
        private DataTable GetSchemeConsign()
        {
            try
            {
                var dt = new DataTable(Def_M_CONSIGN.Name);
                dt.Columns.Add(Def_M_CONSIGN.CONSIGN_CD, typeof(string));
                dt.Columns.Add(Def_M_CONSIGN.NAME, typeof(string));
                dt.Columns.Add(Def_M_CONSIGN.ADDRESS, typeof(string));
                dt.Columns.Add(Def_M_CONSIGN.TEL1, typeof(string));
                dt.Columns.Add(Def_M_CONSIGN.TEL2, typeof(string));
                dt.Columns.Add(Def_M_CONSIGN.FAX, typeof(string));
                dt.Columns.Add(Def_M_CONSIGN.CHINA_FLAG, typeof(string));
                dt.Columns.Add(Def_M_CONSIGN.USCI_CD, typeof(string));
                dt.Columns.Add(Def_M_CONSIGN.VERSION, typeof(object));
                dt.Columns.Add(Def_M_CONSIGN.SORT_NO, typeof(decimal));
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
        /// <param name="dt">荷受マスタデータテーブル</param>
        /// <returns></returns>
        /// <create>H.Tsuji 2018/12/05</create>
        /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
        /// --------------------------------------------------
        private void SetEditData(DataTable dt)
        {
            try
            {
                DataRow dr;
                if (0 < dt.Rows.Count)
                {
                    dr = dt.Rows[0];
                }
                else
                {
                    dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }
                // 名称
                dr[Def_M_CONSIGN.NAME] = this.txtName.Text;
                // 住所
                dr[Def_M_CONSIGN.ADDRESS] = this.txtAddress.Text;
                // TEL
                if (string.IsNullOrEmpty(this.txtTel.Text))
                {
                    dr[Def_M_CONSIGN.TEL1] = DBNull.Value;
                }
                else
                {
                    dr[Def_M_CONSIGN.TEL1] = this.txtTel.Text;
                }
                // TEL2
                if (string.IsNullOrEmpty(this.txtTel2.Text))
                {
                    dr[Def_M_CONSIGN.TEL2] = DBNull.Value;
                }
                else
                {
                    dr[Def_M_CONSIGN.TEL2] = this.txtTel2.Text;
                }
                // FAX
                if (string.IsNullOrEmpty(this.txtFax.Text))
                {
                    dr[Def_M_CONSIGN.FAX] = DBNull.Value;
                }
                else
                {
                    dr[Def_M_CONSIGN.FAX] = this.txtFax.Text;
                }

                // 中国向け
                dr[Def_M_CONSIGN.CHINA_FLAG] = this.cboChina.SelectedValue;

                // USCI
                if (string.IsNullOrEmpty(this.txtUSCI.Text))
                {
                    dr[Def_M_CONSIGN.USCI_CD] = DBNull.Value;
                }
                else
                {
                    dr[Def_M_CONSIGN.USCI_CD] = this.txtUSCI.Text;
                }

                // 並び順
                dr[Def_M_CONSIGN.SORT_NO] = this.numSortNo.Value;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion


    }
}
