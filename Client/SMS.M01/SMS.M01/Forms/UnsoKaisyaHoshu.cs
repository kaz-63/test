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
using WsConnection.WebRefM01;
using System.Text.RegularExpressions;
using SMS.M01.Properties;

namespace SMS.M01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 運送会社マスタメンテナンス
    /// </summary>
    /// <create>T.nakata 2018/11/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class UnsoKaisyaHoshu : SystemBase.Forms.CustomOrderForm
    {
        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// 画面の表示モード
        /// </summary>
        /// <create>T.Nakata 2018/11/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DisplayMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 初期
            /// </summary>
            /// <create>T.Nakata 2018/11/02</create>
            /// <update></update>
            /// --------------------------------------------------
            Initialize,
            /// --------------------------------------------------
            /// <summary>
            /// 検索後
            /// </summary>
            /// <create>T.Nakata 2018/11/02</create>
            /// <update></update>
            /// --------------------------------------------------
            EndSearch,
            /// --------------------------------------------------
            /// <summary>
            /// 登録
            /// </summary>
            /// <create>T.Nakata 2018/11/02</create>
            /// <update></update>
            /// --------------------------------------------------
            Insert,
            /// --------------------------------------------------
            /// <summary>
            /// 変更
            /// </summary>
            /// <create>T.Nakata 2018/11/02</create>
            /// <update></update>
            /// --------------------------------------------------
            Update,
            /// --------------------------------------------------
            /// <summary>
            /// 削除
            /// </summary>
            /// <create>T.Nakata 2018/11/02</create>
            /// <update></update>
            /// --------------------------------------------------
            Delete,
        }

        /// --------------------------------------------------
        /// <summary>
        /// データ選択時の状態
        /// </summary>
        /// <create>T.Nakata 2018/11/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DataSelectType
        {
            /// --------------------------------------------------
            /// <summary>
            /// 登録
            /// </summary>
            /// <create>T.Nakata 2018/11/02</create>
            /// <update></update>
            /// --------------------------------------------------
            Insert,
            /// --------------------------------------------------
            /// <summary>
            /// 変更
            /// </summary>
            /// <create>T.Nakata 2018/11/02</create>
            /// <update></update>
            /// --------------------------------------------------
            Update,
            /// --------------------------------------------------
            /// <summary>
            /// 削除
            /// </summary>
            /// <create>T.Nakata 2018/11/02</create>
            /// <update></update>
            /// --------------------------------------------------
            Delete,
        }

        #endregion

        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// 運送会社CDのカラムインデックス
        /// </summary>
        /// <create>T.Nakata 2018/11/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_UNSOKAISHA_ID = 0;
        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>T.Nakata 2018/11/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 0;

        #endregion

        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 表示時に使用したデータ
        /// </summary>
        /// <create>T.Nakata 2018/11/02</create>
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
        /// <create>T.Nakata 2018/11/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public UnsoKaisyaHoshu(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>T.Nakata 2018/11/02</create>
        /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // ----- コントロールの初期化 -----
                // フォーム
                this.IsCloseQuestion = true;
                this.IsRunEditAfterClear = false;

                // グリッド
                this.InitializeSheet(this.shtResult);
                this.shtResult.KeepHighlighted = true;

                // シートのタイトルを設定
                shtResult.ColumnHeaders[0].Caption = Resources.UnsoKaisyaHoshu_UnsokaishaNo;
                shtResult.ColumnHeaders[1].Caption = Resources.UnsoKaisyaHoshu_KokunaiGaiFlag;
                shtResult.ColumnHeaders[2].Caption = Resources.UnsoKaisyaHoshu_UnsokaishaName;
                shtResult.ColumnHeaders[3].Caption = Resources.UnsoKaisyaHoshu_InvoiceFlag;
                shtResult.ColumnHeaders[4].Caption = Resources.UnsoKaisyaHoshu_PackingListFlag;
                shtResult.ColumnHeaders[5].Caption = Resources.UnsoKaisyaHoshu_TuukanFlag;
                shtResult.ColumnHeaders[6].Caption = Resources.UnsoKaisyaHoshu_TuukanAttn;
                shtResult.ColumnHeaders[7].Caption = Resources.UnsoKaisyaHoshu_SortNo;

                // [検索条件]国内外コンボボックス
                ConnCommon connCom = new ConnCommon();
                CondCommon condCom = new CondCommon(this.UserInfo);
                condCom.GroupCD = DISP_KOKUNAI_GAI_FLAG.GROUPCD;
                DataSet dsCom = connCom.GetCommon(condCom);
                this.cboSearchKokunaigai.DisplayMember = Def_M_COMMON.ITEM_NAME;
                this.cboSearchKokunaigai.ValueMember = Def_M_COMMON.VALUE1;
                this.cboSearchKokunaigai.DataSource = dsCom.Tables[Def_M_COMMON.Name];

                // [登録]国内外コンボボックス
                condCom.GroupCD = KOKUNAI_GAI_FLAG.GROUPCD;
                dsCom = connCom.GetCommon(condCom);
                this.cboKokunaigai.DisplayMember = Def_M_COMMON.ITEM_NAME;
                this.cboKokunaigai.ValueMember = Def_M_COMMON.VALUE1;
                this.cboKokunaigai.DataSource = dsCom.Tables[Def_M_COMMON.Name];

                // Invoiceコンボボックス
                condCom.GroupCD = UNSO_INVOICE_FLAG.GROUPCD;
                dsCom = connCom.GetCommon(condCom);
                this.cboInvoice.DisplayMember = Def_M_COMMON.ITEM_NAME;
                this.cboInvoice.ValueMember = Def_M_COMMON.VALUE1;
                this.cboInvoice.DataSource = dsCom.Tables[Def_M_COMMON.Name];

                // PackingListコンボボックス
                condCom.GroupCD = UNSO_PACKINGLIST_FLAG.GROUPCD;
                dsCom = connCom.GetCommon(condCom);
                this.cboPackingList.DisplayMember = Def_M_COMMON.ITEM_NAME;
                this.cboPackingList.ValueMember = Def_M_COMMON.VALUE1;
                this.cboPackingList.DataSource = dsCom.Tables[Def_M_COMMON.Name];

                // 輸出通関確認書コンボボックス
                condCom.GroupCD = UNSO_EXPORTCONFIRM_FLAG.GROUPCD;
                dsCom = connCom.GetCommon(condCom);
                this.cboTuukanFlag.DisplayMember = Def_M_COMMON.ITEM_NAME;
                this.cboTuukanFlag.ValueMember = Def_M_COMMON.VALUE1;
                this.cboTuukanFlag.DataSource = dsCom.Tables[Def_M_COMMON.Name];

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
        /// <create>T.Nakata 2018/11/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                this.cboSearchKokunaigai.Focus();
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
        /// <create>T.Nakata 2018/11/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                this.cboSearchKokunaigai.SelectedIndex = (this.cboSearchKokunaigai.Items.Count == 0) ? -1 : 0;
                this.txtSearchUnsokaisyaName.Text = string.Empty;
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
        /// <create>T.Nakata 2018/11/02</create>
        /// <update>D.Okumura 2019/01/23 国内の場合の入力条件変更対応</update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                // 名称チェック
                if (string.IsNullOrEmpty(this.txtUnsokaisyaName.Text))
                {
                    // 名称を入力して下さい。
                    this.ShowMessage("M0100160005");
                    this.txtUnsokaisyaName.Focus();
                    return false;
                }

                // 輸出通関確認書 宛先チェック
                if (KOKUNAI_GAI_FLAG.GAI_VALUE1.Equals(this.cboKokunaigai.SelectedValue)
                    && UNSO_EXPORTCONFIRM_FLAG.NECESSARY_VALUE1.Equals(this.cboTuukanFlag.SelectedValue)
                    )
                {
                    if (string.IsNullOrEmpty(this.txtTuukanAttn.Text))
                    {
                        // 輸出通関確認書 宛先を入力して下さい。
                        this.ShowMessage("M0100160006");
                        this.txtTuukanAttn.Focus();
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
        /// <create>T.Nakata 2018/11/02</create>
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
        /// <create>T.Nakata 2018/11/02</create>
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
        /// <create>T.Nakata 2018/11/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                // モード切り替え(初期状態に戻す)
                this.ChangeMode(DisplayMode.Initialize);
                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                if (!this.cboSearchKokunaigai.SelectedValue.Equals(DISP_KOKUNAI_GAI_FLAG.ALL_VALUE1))
                {
                    cond.KokunaigaiFlag = this.cboSearchKokunaigai.SelectedValue.ToString();
                }
                cond.UnsokaishaName = this.txtSearchUnsokaisyaName.Text;
                DataSet ds = conn.GetUnsokaisyaLikeSearch(cond);

                if (!ComFunc.IsExistsData(ds, Def_M_UNSOKAISHA.Name))
                {
                    // シートのクリア
                    this.SheetClear();
                    // 該当する運送会社はありません。
                    this.ShowMessage("M0100160004");
                    return false;
                }

                this.shtResult.DataSource = ds;
                this.shtResult.DataMember = Def_M_UNSOKAISHA.Name;
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
        /// <create>T.Nakata 2018/11/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            bool ret = base.RunEdit();
            try
            {
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
        /// <create>T.Nakata 2018/11/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                this.SetEditData(this._dtDispData);

                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.KokunaigaiFlag = ComFunc.GetFld(this._dtDispData, 0, Def_M_UNSOKAISHA.KOKUNAI_GAI_FLAG);
                cond.UnsokaishaName = ComFunc.GetFld(this._dtDispData, 0, Def_M_UNSOKAISHA.UNSOKAISHA_NAME);

                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtDispData.Copy());

                string errMsgID;
                string[] args;
                if (!conn.InsUnsokaisyaData(cond, ds, out errMsgID, out args))
                {
                    if (errMsgID == "A9999999027")// 一意制約違反
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
        /// <create>T.Nakata 2018/11/05</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                this.SetEditData(this._dtDispData);

                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.UnsoKaishaNo = ComFunc.GetFld(this._dtDispData, 0, Def_M_UNSOKAISHA.UNSOKAISHA_NO);
                cond.KokunaigaiFlag = ComFunc.GetFld(this._dtDispData, 0, Def_M_UNSOKAISHA.KOKUNAI_GAI_FLAG);
                cond.UnsokaishaName = ComFunc.GetFld(this._dtDispData, 0, Def_M_UNSOKAISHA.UNSOKAISHA_NAME);

                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtDispData.Copy());

                string errMsgID;
                string[] args;
                if (!conn.UpdUnsokaisyaData(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID) // バージョンエラー
                        || (errMsgID == "A9999999027"))  // 一意制約違反or運送会社CD無し
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
        /// <create>T.Nakata 2018/11/05</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditDelete()
        {
            try
            {
                this.SetEditData(this._dtDispData);

                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.UnsoKaishaNo = ComFunc.GetFld(this._dtDispData, 0, Def_M_UNSOKAISHA.UNSOKAISHA_NO);

                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtDispData.Copy());

                string errMsgID;
                string[] args;
                if (!conn.DelUnsokaisyaData(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID) // バージョンエラー
                        || (errMsgID == "A9999999027"))  // 運送会社CD無し
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
        /// <create>T.Nakata 2018/11/02</create>
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
        /// <create>T.Nakata 2018/11/02</create>
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
                this.lblSearchKokunaigai.Focus();
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
        /// <create>T.Nakata 2018/11/02</create>
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
        /// <create>T.Nakata 2018/11/02</create>
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
        /// <create>T.Nakata 2018/11/02</create>
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
        /// <create>T.Nakata 2018/11/02</create>
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
        /// <create>T.nakata 2018/11/02</create>
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

        #region 国内外コンボボックス
        /// --------------------------------------------------
        /// <summary>
        /// 国内外選択イベント
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">イベント</param>
        /// <create>D.Okumura 2019/01/23 国内の場合の入力条件変更対応</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboKokunaigai_SelectedValueChanged(object sender, EventArgs e)
        {
            var combo = sender as DSWComboBox;
            // 無効の場合は何もしない
            if (combo == null || !combo.Enabled)
                return;
            var selectedValue = combo.SelectedValue as string;
            if (selectedValue == null)
                return;
            // 国内の場合、名称以外は不要
            var isEnabled = selectedValue == KOKUNAI_GAI_FLAG.GAI_VALUE1;
            lblInvoice.Enabled = isEnabled;
            lblPackingList.Enabled = isEnabled;
            lblTuukanFlag.Enabled = isEnabled;
            lblTuukanAttn.Enabled = isEnabled;
            // 通関の判定を行う
            cboTuukanFlag_SelectedValueChanged(this.cboTuukanFlag, e);
        }
        #endregion

        #region 輸出通関確認書コンボボックス
        /// --------------------------------------------------
        /// <summary>
        /// 輸出通関確認書選択イベント
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">イベント</param>
        /// <create>D.Okumura 2019/01/23 国内の場合の入力条件変更対応</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboTuukanFlag_SelectedValueChanged(object sender, EventArgs e)
        {
            var combo = sender as DSWComboBox;
            // 無効の場合は何もしない
            if (combo == null || !combo.Enabled)
                return;
            var selectedValue = combo.SelectedValue as string;
            if (selectedValue == null)
                return;
            // 輸出通関確認書が不要の時宛名の入力は不要
            var isEnabled = selectedValue == UNSO_EXPORTCONFIRM_FLAG.NECESSARY_VALUE1;
            lblTuukanAttn.Enabled = isEnabled;
        }
        #endregion

        #endregion //イベント

        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>T.Nakata 2018/11/02</create>
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
        /// <create>T.Nakata 2018/11/02</create>
        /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
        /// <update>D.Okumura 2019/01/23 国内の場合の入力条件変更対応</update>
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
                        this.btnInsert.Enabled = false;
                        this.btnUpdate.Enabled = false;
                        this.btnDelete.Enabled = false;
                        this.grpEdit.Enabled = true;
                        this.cboKokunaigai.Enabled = true;
                        this.cboInvoice.Enabled = true;
                        this.cboTuukanFlag.Enabled = true;
                        this.txtUnsokaisyaName.Enabled = true;
                        this.cboPackingList.Enabled = true;
                        this.txtTuukanAttn.Enabled = true;
                        this.numSortNo.Enabled = true;
                        this.txtUnsokaisyaNo.Enabled = false;
                        this.txtVersion.Enabled = false;
                        this.fbrFunction.F01Button.Enabled = true;
                        break;
                    case DisplayMode.Update:
                        // ----- 変更 -----
                        this.grpSearch.Enabled = false;
                        this.btnInsert.Enabled = false;
                        this.btnUpdate.Enabled = false;
                        this.btnDelete.Enabled = false;
                        this.grpEdit.Enabled = true;
                        this.cboKokunaigai.Enabled = true;
                        this.cboInvoice.Enabled = true;
                        this.cboTuukanFlag.Enabled = true;
                        this.txtUnsokaisyaName.Enabled = true;
                        this.cboPackingList.Enabled = true;
                        this.txtTuukanAttn.Enabled = true;
                        this.numSortNo.Enabled = true;
                        this.txtUnsokaisyaNo.Enabled = false;
                        this.txtVersion.Enabled = false;
                        this.fbrFunction.F01Button.Enabled = true;
                        break;
                    case DisplayMode.Delete:
                        // ----- 削除 -----
                        this.grpSearch.Enabled = false;
                        this.btnInsert.Enabled = false;
                        this.btnUpdate.Enabled = false;
                        this.btnDelete.Enabled = false;
                        this.grpEdit.Enabled = false;
                        this.cboKokunaigai.Enabled = false;
                        this.cboInvoice.Enabled = false;
                        this.cboTuukanFlag.Enabled = false;
                        this.txtUnsokaisyaName.Enabled = false;
                        this.cboPackingList.Enabled = false;
                        this.txtTuukanAttn.Enabled = false;
                        this.numSortNo.Enabled = false;
                        this.txtUnsokaisyaNo.Enabled = false;
                        this.txtVersion.Enabled = false;
                        this.fbrFunction.F01Button.Enabled = true;
                        break;
                    default:
                        break;
                }
                cboKokunaigai_SelectedValueChanged(this.cboKokunaigai, new EventArgs());
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
        /// <create>T.Nakata 2018/11/02</create>
        /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
        /// --------------------------------------------------
        private void DisplayClearEdit()
        {
            try
            {
                this.cboKokunaigai.SelectedIndex = (this.cboKokunaigai.Items.Count == 0) ? -1 : 0;
                this.cboInvoice.SelectedIndex = (this.cboInvoice.Items.Count == 0) ? -1 : 0;
                this.cboTuukanFlag.SelectedIndex = (this.cboTuukanFlag.Items.Count == 0) ? -1 : 0;
                this.cboPackingList.SelectedIndex = (this.cboPackingList.Items.Count == 0) ? -1 : 0;
                this.txtUnsokaisyaName.Text = string.Empty;
                this.txtTuukanAttn.Text = string.Empty;
                this.txtUnsokaisyaNo.Text = string.Empty;
                this.txtVersion.Text = string.Empty;
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
        /// <create>T.Nakata 2018/11/02</create>
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
        /// <create>T.Nakata 2018/11/02</create>
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
                                this.cboKokunaigai.Focus();
                                break;
                            case DataSelectType.Update:
                                this.EditMode = SystemBase.EditMode.Update;
                                // モード切り替え
                                this.ChangeMode(DisplayMode.Update);
                                this.cboKokunaigai.Focus();
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
        /// <create>T.Nakata 2018/11/02</create>
        /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
        /// --------------------------------------------------
        private bool RunDataSelectExec(DataSelectType selectType)
        {
            try
            {
                if (selectType == DataSelectType.Insert)
                {
                    // ----- 登録 -----
                    this._dtDispData = this.GetUnsokaisyaData();
                    // 1行追加しておく
                    this._dtDispData.Rows.Add(this._dtDispData.NewRow());
                }
                else
                {
                    // ----- 変更、削除 -----
                    ConnM01 conn = new ConnM01();
                    CondM01 cond = new CondM01(this.UserInfo);
                    cond.UnsoKaishaNo = this.shtResult[SHEET_COL_UNSOKAISHA_ID, shtResult.ActivePosition.Row].Text;
                    DataSet ds = conn.GetUnsokaisya(cond);
                    if (!ComFunc.IsExistsData(ds, Def_M_UNSOKAISHA.Name))
                    {
                        // 既に削除されている運送会社です。
                        this.ShowMessage("M0100160002");
                        this.RunSearch();
                        return false;
                    }
                    this._dtDispData = ds.Tables[Def_M_UNSOKAISHA.Name];

                    // 表示データ設定
                    this.txtUnsokaisyaNo.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_UNSOKAISHA.UNSOKAISHA_NO);
                    this.txtVersion.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_UNSOKAISHA.VERSION);
                    this.cboKokunaigai.SelectedValue = ComFunc.GetFld(this._dtDispData, 0, Def_M_UNSOKAISHA.KOKUNAI_GAI_FLAG);
                    this.txtUnsokaisyaName.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_UNSOKAISHA.UNSOKAISHA_NAME);
                    this.cboInvoice.SelectedValue = ComFunc.GetFld(this._dtDispData, 0, Def_M_UNSOKAISHA.INVOICE_FLAG);
                    this.cboPackingList.SelectedValue = ComFunc.GetFld(this._dtDispData, 0, Def_M_UNSOKAISHA.PACKINGLIST_FLAG);
                    this.cboTuukanFlag.SelectedValue = ComFunc.GetFld(this._dtDispData, 0, Def_M_UNSOKAISHA.EXPORTCONFIRM_FLAG);
                    this.txtTuukanAttn.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_UNSOKAISHA.EXPORTCONFIRM_ATTN);
                    this.numSortNo.Value = ComFunc.GetFldToDecimal(this._dtDispData, 0, Def_M_UNSOKAISHA.SORT_NO);
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

        #region 運送会社マスタのデータテーブル作成

        /// --------------------------------------------------
        /// <summary>
        /// 運送会社マスタのデータテーブル作成
        /// </summary>
        /// <returns>運送会社マスタのデータテーブル</returns>
        /// <create>T.Nakata 2018/11/02</create>
        /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
        /// --------------------------------------------------
        private DataTable GetUnsokaisyaData()
        {
            try
            {
                DataTable dt = new DataTable(Def_M_UNSOKAISHA.Name);
                dt.Columns.Add(Def_M_UNSOKAISHA.UNSOKAISHA_NO, typeof(string));
                dt.Columns.Add(Def_M_UNSOKAISHA.KOKUNAI_GAI_FLAG, typeof(string));
                dt.Columns.Add(Def_M_UNSOKAISHA.UNSOKAISHA_NAME, typeof(string));
                dt.Columns.Add(Def_M_UNSOKAISHA.INVOICE_FLAG, typeof(string));
                dt.Columns.Add(Def_M_UNSOKAISHA.PACKINGLIST_FLAG, typeof(string));
                dt.Columns.Add(Def_M_UNSOKAISHA.EXPORTCONFIRM_FLAG, typeof(string));
                dt.Columns.Add(Def_M_UNSOKAISHA.EXPORTCONFIRM_ATTN, typeof(string));
                dt.Columns.Add(Def_M_UNSOKAISHA.SORT_NO, typeof(decimal));
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
        /// <param name="dt">運送会社マスタデータテーブル</param>
        /// <create>T.Nakata 2018/11/02</create>
        /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
        /// <update>D.Okumura 2019/01/23 国内の場合の入力条件変更対応</update>
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
                dr[Def_M_UNSOKAISHA.KOKUNAI_GAI_FLAG] = this.cboKokunaigai.SelectedValue;
                dr[Def_M_UNSOKAISHA.UNSOKAISHA_NAME] = this.txtUnsokaisyaName.Text;
                if (this.cboKokunaigai.SelectedValue as string == KOKUNAI_GAI_FLAG.GAI_VALUE1)
                {
                    dr[Def_M_UNSOKAISHA.INVOICE_FLAG] = this.cboInvoice.SelectedValue;
                    dr[Def_M_UNSOKAISHA.PACKINGLIST_FLAG] = this.cboPackingList.SelectedValue;
                    dr[Def_M_UNSOKAISHA.EXPORTCONFIRM_FLAG] = this.cboTuukanFlag.SelectedValue;
                }
                else
                {
                    dr[Def_M_UNSOKAISHA.INVOICE_FLAG] = UNSO_INVOICE_FLAG.UNNECESSARY_VALUE1;
                    dr[Def_M_UNSOKAISHA.PACKINGLIST_FLAG] = UNSO_PACKINGLIST_FLAG.UNNECESSARY_VALUE1;
                    dr[Def_M_UNSOKAISHA.EXPORTCONFIRM_FLAG] = UNSO_EXPORTCONFIRM_FLAG.UNNECESSARY_VALUE1;
                }
                dr[Def_M_UNSOKAISHA.EXPORTCONFIRM_ATTN] = this.txtTuukanAttn.Text;
                dr[Def_M_UNSOKAISHA.SORT_NO] = this.numSortNo.Value;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region エラー時のセットフォーカス

        /// --------------------------------------------------
        /// <summary>
        /// エラー時のセットフォーカス
        /// </summary>
        /// <param name="msgID">メッセージID</param>
        /// <create>T.Nakata 2018/11/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void ErrorSetFocus(string msgID)
        {
        }

        #endregion
    }
}
