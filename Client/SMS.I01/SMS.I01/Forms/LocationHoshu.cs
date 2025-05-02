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

using WsConnection.WebRefMaster;
using WsConnection.WebRefI01;
using SMS.I01.Properties;

namespace SMS.I01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// ロケーションマスタ保守
    /// </summary>
    /// <create>T.Wakamatsu 2013/07/29</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class LocationHoshu : SystemBase.Forms.CustomOrderForm
    {
        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// 画面の表示モード
        /// </summary>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DisplayMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 初期
            /// </summary>
            /// <create>T.Wakamatsu 2013/07/30</create>
            /// <update></update>
            /// --------------------------------------------------
            Initialize,
            /// --------------------------------------------------
            /// <summary>
            /// 検索後
            /// </summary>
            /// <create>T.Wakamatsu 2013/07/30</create>
            /// <update></update>
            /// --------------------------------------------------
            EndSearch,
            /// --------------------------------------------------
            /// <summary>
            /// 登録
            /// </summary>
            /// <create>T.Wakamatsu 2013/07/30</create>
            /// <update></update>
            /// --------------------------------------------------
            Insert,
            /// --------------------------------------------------
            /// <summary>
            /// 変更
            /// </summary>
            /// <create>T.Wakamatsu 2013/07/30</create>
            /// <update></update>
            /// --------------------------------------------------
            Update,
            /// --------------------------------------------------
            /// <summary>
            /// 削除
            /// </summary>
            /// <create>T.Wakamatsu 2013/07/30</create>
            /// <update></update>
            /// --------------------------------------------------
            Delete,
            /// --------------------------------------------------
            /// <summary>
            /// 編集
            /// </summary>
            /// <create>T.Wakamatsu 2013/07/30</create>
            /// <update></update>
            /// --------------------------------------------------
            Edit,
            /// --------------------------------------------------
            /// <summary>
            /// 検索結果クリア
            /// </summary>
            /// <create>H.Tajimi 2015/12/01</create>
            /// <update></update>
            /// --------------------------------------------------
            ResultClear,
        }

        /// --------------------------------------------------
        /// <summary>
        /// データ選択時の状態
        /// </summary>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DataSelectType
        {
            /// --------------------------------------------------
            /// <summary>
            /// 登録
            /// </summary>
            /// <create>T.Wakamatsu 2013/07/30</create>
            /// <update></update>
            /// --------------------------------------------------
            Insert,
            /// --------------------------------------------------
            /// <summary>
            /// 変更
            /// </summary>
            /// <create>T.Wakamatsu 2013/07/30</create>
            /// <update></update>
            /// --------------------------------------------------
            Update,
            /// --------------------------------------------------
            /// <summary>
            /// 削除
            /// </summary>
            /// <create>T.Wakamatsu 2013/07/30</create>
            /// <update></update>
            /// --------------------------------------------------
            Delete,
        }

        #endregion

        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分のカラムインデックス
        /// </summary>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SHUKKA_FLAG = 3;
        /// --------------------------------------------------
        /// <summary>
        /// 物件管理Noのカラムインデックス
        /// </summary>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_BUKKEN_NO = 4;
        /// --------------------------------------------------
        /// <summary>
        /// ロケーションのカラムインデックス
        /// </summary>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_LOCATION = 2;
        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 0;

        #endregion

        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 表示時に使用したデータ
        /// </summary>
        /// <create>T.Wakamatsu 2013/07/30</create>
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
        /// <create>T.Wakamatsu 2013/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public LocationHoshu(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
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
                shtResult.ColumnHeaders[0].Caption = Resources.LocationHoshu_ShippingDivision;
                shtResult.ColumnHeaders[1].Caption = Resources.LocationHoshu_PropertyName;
                shtResult.ColumnHeaders[2].Caption = Resources.LocationHoshu_Location;

                // コンボボックスの初期化
                this.MakeCmbBox(this.cboSearchShukkaFlag, SHUKKA_FLAG.GROUPCD);
                this.MakeCmbBox(this.cboShukkaFlag, SHUKKA_FLAG.GROUPCD);

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
        /// <create>K.Saeki 2012/03/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                this.cboSearchShukkaFlag.Focus();
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
        /// <create>K.Saeki 2012/03/23</create>
        /// <update>H.Tajimi 2015/12/08 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // 2015/12/08 H.Tajimi クリア処理追加
                this.cboSearchShukkaFlag.SelectedValue = SHUKKA_FLAG.DEFAULT_VALUE1;
                this.txtSearchBukkenName.Text = string.Empty;
                this.txtSearchLocation.Text = string.Empty;
                // ↑
                // グリッド
                this.SheetClear();
                // 登録情報部分のクリア
                this.DisplayClearEdit(DisplayMode.Initialize);
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
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                if (this.EditMode == SystemBase.EditMode.Delete)
                {
                    // 特にチェックする項目なし
                }
                else
                {
                    // 物件名チェック
                    if (string.IsNullOrEmpty(this.txtBukkenNo.Text))
                    {
                        // 物件名を入力して下さい。
                        this.ShowMessage("I0100010003");
                        this.btnBukkenIchiran.Focus();
                        return false;
                    }
                    // ロケーションチェック
                    if (string.IsNullOrEmpty(this.txtLocation.Text))
                    {
                        // ロケーションを入力して下さい。
                        this.ShowMessage("I0100020001");
                        this.txtLocation.Focus();
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

        /// --------------------------------------------------
        /// <summary>
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                // 物件名チェック
                if (string.IsNullOrEmpty(this.txtSearchBukkenNo.Text))
                {
                    // 物件名を入力して下さい。
                    this.ShowMessage("I0100010003");
                    this.btnBukkenIchiran.Focus();
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

        #region 検索処理

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理制御部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>T.Wakamatsu 2013/07/30</create>
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
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                // モード切り替え(初期状態に戻す)
                this.ChangeMode(DisplayMode.Initialize);
                ConnI01 conn = new ConnI01();
                WsConnection.WebRefI01.CondI01 cond = new CondI01(this.UserInfo);
                cond.ShukkaFlag = this.cboSearchShukkaFlag.SelectedValue.ToString();
                cond.BukkenNo = this.txtSearchBukkenNo.Text;
                cond.Location = this.txtSearchLocation.Text;
                DataSet ds = conn.GetLocationLikeSearch(cond);

                if (!ComFunc.IsExistsData(ds, Def_M_LOCATION.Name))
                {
                    // シートのクリア
                    this.SheetClear();
                    // 該当のロケーションは存在しません。
                    this.ShowMessage("I0100010004");
                    return false;
                }

                this.shtResult.DataSource = ds;
                this.shtResult.DataMember = Def_M_LOCATION.Name;
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
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            bool ret = base.RunEdit();
            try
            {
                if (ret)
                {
                    this.DisplayClearEdit(DisplayMode.Edit);
                    // とりあえず検索
                    if (!string.IsNullOrEmpty(txtSearchBukkenNo.Text))
                    {
                        this.RunSearch();
                    }
                    else
                    {
                        // モード切り替え
                        this.ChangeMode(DisplayMode.Initialize);
                    }
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
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                this.SetEditData(this._dtDispData);

                ConnI01 conn = new ConnI01();
                CondI01 cond = new CondI01(this.UserInfo);
                cond.ShukkaFlag = ComFunc.GetFld(this._dtDispData, 0, Def_M_LOCATION.SHUKKA_FLAG);
                cond.BukkenNo = ComFunc.GetFld(this._dtDispData, 0, Def_M_LOCATION.BUKKEN_NO);
                cond.Location = ComFunc.GetFld(this._dtDispData, 0, Def_M_LOCATION.LOCATION);

                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtDispData.Copy());

                string errMsgID;
                string[] args;
                if (!conn.InsLocationData(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID))
                    {
                        this.DisplayClearEdit(DisplayMode.Insert);
                        this.RunSearch();
                    }
                    else if (errMsgID == "I0100010001")
                    {
                        if (!string.IsNullOrEmpty(txtSearchBukkenNo.Text))
                        {
                            this.RunSearch();
                        }
                        this.ChangeMode(DisplayMode.Insert);
                        this.txtLocation.Focus();
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
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                this.SetEditData(this._dtDispData);

                ConnI01 conn = new ConnI01();
                CondI01 cond = new CondI01(this.UserInfo);
                cond.ShukkaFlag = ComFunc.GetFld(this._dtDispData, 0, Def_M_LOCATION.SHUKKA_FLAG);
                cond.BukkenNo = ComFunc.GetFld(this._dtDispData, 0, Def_M_LOCATION.BUKKEN_NO);
                cond.Location = ComFunc.GetFld(this._dtDispData, 0, Def_M_LOCATION.LOCATION);

                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtDispData.Copy());

                string errMsgID;
                string[] args;
                if (!conn.UpdLocationData(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID)
                        || errMsgID == "I0100010002")
                    {
                        this.DisplayClearEdit(DisplayMode.Update);
                        this.RunSearch();
                    }
                    else if (errMsgID == "I0100010001")
                    {
                        if (!string.IsNullOrEmpty(txtSearchBukkenNo.Text))
                        {
                            this.RunSearch();
                        }
                        this.ChangeMode(DisplayMode.Update);
                        this.txtLocation.Focus();
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
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditDelete()
        {
            try
            {
                this.SetEditData(this._dtDispData);

                ConnI01 conn = new ConnI01();
                CondI01 cond = new CondI01(this.UserInfo);
                cond.ShukkaFlag = ComFunc.GetFld(this._dtDispData, 0, Def_M_LOCATION.SHUKKA_FLAG);
                cond.BukkenNo = ComFunc.GetFld(this._dtDispData, 0, Def_M_LOCATION.BUKKEN_NO);
                cond.Location = ComFunc.GetFld(this._dtDispData, 0, Def_M_LOCATION.LOCATION);

                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtDispData.Copy());

                string errMsgID;
                string[] args;
                if (!conn.DelLocationData(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID)
                        || errMsgID == "I0100010002")
                    {
                        this.DisplayClearEdit(DisplayMode.Delete);
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
        /// <create>T.Wakamatsu 2013/07/30</create>
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
        /// F6ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update>H.Tajimi 2015/12/08 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                // 画面をクリアします。\r\nよろしいですか？
                if (ShowMessage("A9999999053") != DialogResult.OK) return;
                this.SheetClear();
                this.DisplayClearEdit(DisplayMode.ResultClear);
                this.ChangeMode(DisplayMode.ResultClear);
                this.txtSearchBukkenName.Focus();
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
        /// <create>H.Tajimi 2015/12/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                // 画面をクリアします。\r\nよろしいですか？
                if (ShowMessage("A9999999001") != DialogResult.OK) return;
                this.DisplayClear();
                this.cboSearchShukkaFlag.Focus();
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
        /// <create>T.Wakamatsu 2013/07/30</create>
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
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update>H.Tajimi 2015/11/30 納入先(User)選択を早くする</update>
        /// <update></update>
        /// --------------------------------------------------
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                // 2015/11/30 H.Tajimi 物件名一覧画面表示
                if (!this.ShowBukkenMeiIchiran())
                {
                    this.txtSearchBukkenName.Focus();
                }
                // ↑
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分コンボボックス変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/07/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboSearchShukkaFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtSearchBukkenNo.Text = "";
            this.txtSearchBukkenName.Text = "";
        }

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分コンボボックス変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/07/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboShukkaFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtBukkenNo.Text = "";
            this.txtBukkenName.Text = "";
        }

        #region 登録ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 登録ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>K.Saeki 2012/03/26</create>
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
        /// <create>K.Saeki 2012/03/26</create>
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
        /// <create>K.Saeki 2012/03/26</create>
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

        #endregion

        #region 物件一覧

        /// --------------------------------------------------
        /// <summary>
        /// 物件一覧ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnBukkenIchiran_Click(object sender, EventArgs e)
        {
            try
            {
                string shukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
                string bukkenName = this.txtBukkenName.Text;
                using (var frm = new SMS.P02.Forms.BukkenMeiIchiran(this.UserInfo, shukkaFlag, bukkenName))
                {
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        this.txtBukkenNo.Text = ComFunc.GetFld(frm.SelectedRowData, Def_M_BUKKEN.BUKKEN_NO);
                        this.txtBukkenName.Text = ComFunc.GetFld(frm.SelectedRowData, Def_M_BUKKEN.BUKKEN_NAME);
                    }
                    this.txtLocation.Focus();
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>T.Wakamatsu 2013/07/30</create>
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
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update>H.Tajimi 2015/12/02 System全般/Clearボタン押下時の範囲</update>
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
                        // 2015/12/02 H.Tajimi クリア無効
                        this.fbrFunction.F06Button.Enabled = false;
                        // ↑
                        break;
                    case DisplayMode.EndSearch:
                        // ----- 検索後 -----
                        this.grpSearch.Enabled = true;
                        this.btnInsert.Enabled = true;
                        this.btnUpdate.Enabled = true;
                        this.btnDelete.Enabled = true;
                        this.grpEdit.Enabled = false;
                        this.fbrFunction.F01Button.Enabled = false;
                        // 2015/12/02 H.Tajimi クリア有効
                        this.fbrFunction.F06Button.Enabled = true;
                        // ↑
                        break;
                    case DisplayMode.Insert:
                        // ----- 登録 -----
                        this.grpSearch.Enabled = false;
                        this.grpEdit.Enabled = true;
                        this.cboShukkaFlag.Enabled = true;
                        this.btnBukkenIchiran.Enabled = true;
                        this.txtLocation.Enabled = true;
                        this.fbrFunction.F01Button.Enabled = true;
                        // 2015/12/02 H.Tajimi クリア有効
                        this.fbrFunction.F06Button.Enabled = true;
                        // ↑
                        break;
                    case DisplayMode.Update:
                        // ----- 変更 -----
                        this.grpSearch.Enabled = false;
                        this.grpEdit.Enabled = true;
                        this.cboShukkaFlag.Enabled = false;
                        this.txtLocation.Enabled = true;
                        this.fbrFunction.F01Button.Enabled = true;
                        // 2015/12/02 H.Tajimi クリア有効
                        this.fbrFunction.F06Button.Enabled = true;
                        // ↑
                        break;
                    case DisplayMode.Delete:
                        // ----- 削除 -----
                        this.grpSearch.Enabled = false;
                        this.grpEdit.Enabled = true;
                        this.cboShukkaFlag.Enabled = false;
                        this.btnBukkenIchiran.Enabled = false;
                        this.txtLocation.Enabled = false;
                        this.fbrFunction.F01Button.Enabled = true;
                        // 2015/12/02 H.Tajimi クリア有効
                        this.fbrFunction.F06Button.Enabled = true;
                        // ↑
                        break;
                    // 2015/12/02 H.Tajimi 検索結果クリア時のシート、ボタン制御
                    case DisplayMode.ResultClear:
                        // ----- 検索結果クリア -----
                        this.grpSearch.Enabled = true;
                        this.btnInsert.Enabled = true;
                        this.btnUpdate.Enabled = false;
                        this.btnDelete.Enabled = false;
                        this.grpEdit.Enabled = false;
                        this.fbrFunction.F01Button.Enabled = false;
                        this.fbrFunction.F06Button.Enabled = false;
                        break;
                    // ↑
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
        /// <param name="mode">画面の表示モード</param>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update>H.Tajimi 2015/12/08 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        private void DisplayClearEdit(DisplayMode mode)
        {
            try
            {
                // 2015/12/08 H.Tajimi クリア処理追加
                if (mode == DisplayMode.ResultClear)
                {
                    this.cboShukkaFlag.SelectedValue = SHUKKA_FLAG.DEFAULT_VALUE1;
                    this.txtBukkenName.Text = string.Empty;
                }
                // ↑
                this.txtLocation.Text = string.Empty;
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
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckInputDataSelect(DataSelectType selectType)
        {
            try
            {
                if (this.shtResult.ActivePosition.Row < 0 && selectType != DataSelectType.Insert)
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
        /// <create>T.Wakamatsu 2013/07/30</create>
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
                                this.cboShukkaFlag.Focus();
                                break;
                            case DataSelectType.Update:
                                this.EditMode = SystemBase.EditMode.Update;
                                // モード切り替え
                                this.ChangeMode(DisplayMode.Update);
                                this.txtLocation.Focus();
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
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RunDataSelectExec(DataSelectType selectType)
        {
            try
            {
                if (selectType == DataSelectType.Insert)
                {
                    // ----- 登録 -----
                    this._dtDispData = this.GetSchemeLocation();
                    // 1行追加しておく
                    this._dtDispData.Rows.Add(this._dtDispData.NewRow());
                }
                else
                {
                    // ----- 変更、削除 -----
                    ConnI01 conn = new ConnI01();
                    CondI01 cond = new CondI01(this.UserInfo);
                    cond.ShukkaFlag = this.shtResult[SHEET_COL_SHUKKA_FLAG, shtResult.ActivePosition.Row].Text;
                    cond.BukkenNo = this.shtResult[SHEET_COL_BUKKEN_NO, shtResult.ActivePosition.Row].Text;
                    cond.Location = this.shtResult[SHEET_COL_LOCATION, shtResult.ActivePosition.Row].Text;
                    DataSet ds = conn.GetLocation(cond);
                    if (!ComFunc.IsExistsData(ds, Def_M_LOCATION.Name))
                    {
                        // 既に削除された物件名です。
                        this.ShowMessage("I0100010002");
                        // 消えてるのがあったから取り敢えず検索
                        this.RunSearch();
                        return false;
                    }
                    this._dtDispData = ds.Tables[Def_M_LOCATION.Name];
                }
                // 表示データ設定
                if (!string.IsNullOrEmpty(ComFunc.GetFld(this._dtDispData, 0, Def_M_LOCATION.SHUKKA_FLAG)))
                {
                    this.cboShukkaFlag.SelectedValue = ComFunc.GetFld(this._dtDispData, 0, Def_M_LOCATION.SHUKKA_FLAG);
                    this.txtBukkenNo.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_LOCATION.BUKKEN_NO);
                    this.txtBukkenName.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.BUKKEN_NAME);
                    this.txtLocation.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_LOCATION.LOCATION);
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

        #region ロケーションマスタのデータテーブル作成

        /// --------------------------------------------------
        /// <summary>
        /// ロケーションマスタのデータテーブル作成
        /// </summary>
        /// <returns>ロケーションマスタのデータテーブル</returns>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSchemeLocation()
        {
            try
            {
                DataTable dt = new DataTable(Def_M_LOCATION.Name);
                dt.Columns.Add(Def_M_LOCATION.SHUKKA_FLAG, typeof(string));
                dt.Columns.Add(Def_M_LOCATION.BUKKEN_NO, typeof(decimal));
                dt.Columns.Add(Def_M_LOCATION.LOCATION, typeof(string));
                dt.Columns.Add(Def_M_LOCATION.CREATE_DATE, typeof(object));
                dt.Columns.Add(Def_M_LOCATION.CREATE_USER_ID, typeof(string));
                dt.Columns.Add(Def_M_LOCATION.CREATE_USER_NAME, typeof(string));
                dt.Columns.Add(Def_M_LOCATION.UPDATE_DATE, typeof(object));
                dt.Columns.Add(Def_M_LOCATION.UPDATE_USER_ID, typeof(string));
                dt.Columns.Add(Def_M_LOCATION.UPDATE_USER_NAME, typeof(string));
                dt.Columns.Add(Def_M_LOCATION.VERSION, typeof(object));
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
        /// <param name="dt">ロケーションマスタデータテーブル</param>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
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
                dr[Def_M_LOCATION.SHUKKA_FLAG] = this.cboShukkaFlag.SelectedValue.ToString();
                if (!string.IsNullOrEmpty(this.txtBukkenNo.Text))
                {
                    dr[Def_M_LOCATION.BUKKEN_NO] = UtilConvert.ToDecimal(this.txtBukkenNo.Text);
                }
                dr[Def_M_LOCATION.LOCATION] = this.txtLocation.Text;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 画面表示

        #region 物件名一覧画面表示

        /// --------------------------------------------------
        /// <summary>
        /// 物件名一覧画面表示
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2015/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ShowBukkenMeiIchiran()
        {
            string shukkaFlag = this.cboSearchShukkaFlag.SelectedValue.ToString();
            string bukkenName = this.txtSearchBukkenName.Text;
            using (var frm = new SMS.P02.Forms.BukkenMeiIchiran(this.UserInfo, shukkaFlag, bukkenName, true))
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    DataRow dr = frm.SelectedRowData;
                    if (dr == null) return false;
                    // 選択データを設定
                    this.txtSearchBukkenNo.Text = ComFunc.GetFld(dr, Def_M_BUKKEN.BUKKEN_NO);
                    this.txtSearchBukkenName.Text = ComFunc.GetFld(dr, Def_M_BUKKEN.BUKKEN_NAME);
                    return this.RunSearch();
                }
            }
            return false;
        }

        #endregion

        #endregion

    }
}
