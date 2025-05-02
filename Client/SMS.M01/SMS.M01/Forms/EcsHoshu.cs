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

using SMS.M01.Properties;
using SystemBase.Util;
using WsConnection.WebRefM01;

namespace SMS.M01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 技連保守
    /// </summary>
    /// <create>H.Tsuji 2018/11/06</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class EcsHoshu : SystemBase.Forms.CustomOrderForm
    {
        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// 画面の表示モード
        /// </summary>
        /// <create>H.Tsuji 2018/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DisplayMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 初期
            /// </summary>
            /// <create>H.Tsuji 2018/11/08</create>
            /// <update></update>
            /// --------------------------------------------------
            Initialize,
            /// --------------------------------------------------
            /// <summary>
            /// 検索後
            /// </summary>
            /// <create>H.Tsuji 2018/11/08</create>
            /// <update></update>
            /// --------------------------------------------------
            EndSearch,
            /// --------------------------------------------------
            /// <summary>
            /// 登録
            /// </summary>
            /// <create>H.Tsuji 2018/11/08</create>
            /// <update></update>
            /// --------------------------------------------------
            Insert,
            /// --------------------------------------------------
            /// <summary>
            /// 変更
            /// </summary>
            /// <create>H.Tsuji 2018/11/08</create>
            /// <update></update>
            /// --------------------------------------------------
            Update,
            /// --------------------------------------------------
            /// <summary>
            /// 削除
            /// </summary>
            /// <create>H.Tsuji 2018/11/08</create>
            /// <update></update>
            /// --------------------------------------------------
            Delete,
        }

        /// --------------------------------------------------
        /// <summary>
        /// データ選択時の状態
        /// </summary>
        /// <create>H.Tsuji 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DataSelectType
        {
            /// --------------------------------------------------
            /// <summary>
            /// 登録
            /// </summary>
            /// <create>H.Tsuji 2018/11/22</create>
            /// <update></update>
            /// --------------------------------------------------
            Insert,
            /// --------------------------------------------------
            /// <summary>
            /// 変更
            /// </summary>
            /// <create>H.Tsuji 2018/11/22</create>
            /// <update></update>
            /// --------------------------------------------------
            Update,
            /// --------------------------------------------------
            /// <summary>
            /// 削除
            /// </summary>
            /// <create>H.Tsuji 2018/11/22</create>
            /// <update></update>
            /// --------------------------------------------------
            Delete,
        }

        #endregion

        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// 期のカラムインデックス
        /// </summary>
        /// <create>H.Tsuji 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ECS_QUOTA = 0;
        /// --------------------------------------------------
        /// <summary>
        /// ECS No.のカラムインデックス
        /// </summary>
        /// <create>H.Tsuji 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ECS_NO = 1;
        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>H.Tsuji 2018/11/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 0;

        #endregion

        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 表示時に使用したデータ
        /// </summary>
        /// <create>H.Tsuji 2018/11/22</create>
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
        /// <create>H.Tsuji 2018/11/06</create>
        /// <update></update>
        /// --------------------------------------------------
        public EcsHoshu(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>H.Tsuji 2018/11/06</create>
        /// <update>K.Tsutsumi 2019/02/12 画面クリア時にコンボボックスの内容を再読み込みするように修正</update>
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
                shtResult.ColumnHeaders[0].Caption = Resources.EcsHoshu_Quota;
                shtResult.ColumnHeaders[1].Caption = Resources.EcsHoshu_EcsNo;
                shtResult.ColumnHeaders[2].Caption = Resources.EcsHoshu_BukkenName;
                shtResult.ColumnHeaders[3].Caption = Resources.EcsHoshu_Seiban;
                shtResult.ColumnHeaders[4].Caption = Resources.EcsHoshu_Code;
                shtResult.ColumnHeaders[5].Caption = Resources.EcsHoshu_ArNo;
                shtResult.ColumnHeaders[6].Caption = Resources.EcsHoshu_Kishu;
                shtResult.ColumnHeaders[7].Caption = Resources.EcsHoshu_KanriFlag;

                // コンボボックスの初期化
                this.MakeCmbBox(this.cboSearchKanriFlag, DISP_KANRI_FLAG.GROUPCD);
                this.MakeCmbBox(this.cboKanriFlag, KANRI_FLAG.GROUPCD);

                // モード切替
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
        /// <create>H.Tsuji 2018/11/06</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                this.txtSearchQuota.Focus();
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
        /// <create>H.Tsuji 2018/11/06</create>
        /// <update>K.Tsutsumi 2019/02/12 画面クリア時にコンボボックスの内容を再読み込みするように修正</update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                this.ShowProjectList();
                this.ShowKishuList();

                this.txtSearchQuota.Text = string.Empty;
                this.txtSearchEcsNo.Text = string.Empty;
                this.cboSearchBukkenName.SelectedIndex = -1;
                this.txtSearchSeiban.Text = string.Empty;
                this.txtSearchCode.Text = string.Empty;
                this.txtSearchArNo.Text = string.Empty;
                this.txtSearchKishu.Text = string.Empty;
                this.cboSearchKanriFlag.SelectedValue = DISP_KANRI_FLAG.DEFAULT_VALUE1;
                // グリッド
                this.SheetClear();
                // 登録情報部分のクリア
                this.DisplayClearEdit();
                // モード切替
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
        /// <create>H.Tsuji 2018/11/06</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                // 期チェック
                if (string.IsNullOrEmpty(this.txtQuota.Text))
                {
                    // 期を入力して下さい。
                    this.ShowMessage("M0100170001");
                    this.txtQuota.Focus();
                    return false;
                }
                // ECS No.チェック
                if (string.IsNullOrEmpty(this.txtEcsNo.Text))
                {
                    // ECS No.を入力して下さい。
                    this.ShowMessage("M0100170002");
                    this.txtEcsNo.Focus();
                    return false;
                }
                // 削除時はここまで来ればOK
                if (this.EditMode == SystemBase.EditMode.Delete)
                {
                    return true;
                }
                // 物件名チェック
                if (string.IsNullOrEmpty(this.cboBukkenName.Text))
                {
                    // 物件名を入力して下さい。
                    this.ShowMessage("M0100170003");
                    this.cboBukkenName.Focus();
                    return false;
                }
                // 物件名の存在チェック
                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.BukkenName = this.cboBukkenName.Text;
                DataSet ds = conn.GetProject(cond);
                if (!ComFunc.IsExistsData(ds, Def_M_PROJECT.Name))
                {
                    // 該当する物件名はありません。
                    this.ShowMessage("M0100170008");
                    this.cboBukkenName.Focus();
                    return false;
                }

                // 製番チェック
                if (string.IsNullOrEmpty(this.txtSeiban.Text))
                {
                    // 製番を入力して下さい。
                    this.ShowMessage("M0100170004");
                    this.txtSeiban.Focus();
                    return false;
                }
                // CODEチェック
                if (string.IsNullOrEmpty(this.txtCode.Text))
                {
                    // CODEを入力して下さい。
                    this.ShowMessage("M0100170005");
                    this.txtCode.Focus();
                    return false;
                }
                // 機種チェック
                if (string.IsNullOrEmpty(this.cboKishu.Text))
                {
                    // 機種を入力して下さい。
                    this.ShowMessage("M0100170011");
                    this.cboKishu.Focus();
                    return false;
                }
                if (Encoding.GetEncoding("Shift-JIS").GetByteCount(this.cboKishu.Text) > ComDefine.MAX_BYTE_LENGTH_KISHU)
                {
                    // {0}には{1}Byte以下で入力して下さい。
                    string[] param = new string[] { this.lblKishu.Text, ComDefine.MAX_BYTE_LENGTH_KISHU.ToString() };
                    this.ShowMessage("A9999999052", param);
                    this.cboKishu.Focus();
                    return false;
                }
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                ret = false;
            }
            return ret;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>H.Tsuji 2018/11/06</create>
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
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
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
        /// <create>H.Tsuji 2018/11/06</create>
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
        /// <create>H.Tsuji 2018/11/06</create>
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
                cond.EcsQuota = this.txtSearchQuota.Text;
                cond.EcsNo = this.txtSearchEcsNo.Text;
                cond.BukkenName = this.cboSearchBukkenName.Text;
                cond.Seiban = this.txtSearchSeiban.Text;
                cond.Code = this.txtSearchCode.Text;
                if (string.IsNullOrEmpty(this.txtSearchArNo.Text))
                {
                    cond.ARNo = this.txtSearchArNo.Text;
                }
                else
                {
                    cond.ARNo = ComDefine.PREFIX_ARNO + this.txtSearchArNo.Text;
                }
                cond.Kishu = this.txtSearchKishu.Text;
                cond.KanriFlag = this.cboSearchKanriFlag.SelectedValue.ToString();
                DataSet ds = conn.GetEcsLikeSearch(cond);

                if (!ComFunc.IsExistsData(ds, Def_M_ECS.Name))
                {
                    // シートのクリア
                    this.SheetClear();
                    // 該当する技連はありません。
                    this.ShowMessage("M0100170010");
                    return false;
                }

                this.shtResult.DataSource = ds;
                this.shtResult.DataMember = Def_M_ECS.Name;

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
        /// <create>H.Tsuji 2018/11/22</create>
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
        /// <create>H.Tsuji 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                this.SetEditData(this._dtDispData);
                if (string.IsNullOrEmpty(ComFunc.GetFld(this._dtDispData, 0, Def_M_PROJECT.PROJECT_NO)))
                {
                    // 該当する物件名はありません。
                    this.ShowMessage("M0100170008");
                    return false;
                }

                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.EcsQuota = ComFunc.GetFld(this._dtDispData, 0, Def_M_ECS.ECS_QUOTA);
                cond.EcsNo = ComFunc.GetFld(this._dtDispData, 0, Def_M_ECS.ECS_NO);
                cond.BukkenName = this.cboBukkenName.Text;

                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtDispData.Copy());

                string errMsgID;
                string[] args;
                if (!conn.InsEcsData(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID))
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
        /// <create>H.Tsuji 2018/11/06</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                this.SetEditData(this._dtDispData);
                if (string.IsNullOrEmpty(ComFunc.GetFld(this._dtDispData, 0, Def_M_PROJECT.PROJECT_NO)))
                {
                    // 該当する物件名はありません。
                    this.ShowMessage("M0100170008");
                    return false;
                }

                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.EcsQuota = ComFunc.GetFld(this._dtDispData, 0, Def_M_ECS.ECS_QUOTA);
                cond.EcsNo = ComFunc.GetFld(this._dtDispData, 0, Def_M_ECS.ECS_NO);
                cond.BukkenName = this.cboBukkenName.Text;

                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtDispData.Copy());

                string errMsgID;
                string[] args;
                if (!conn.UpdEcsData(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID))
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
        /// <create>H.Tsuji 2018/11/06</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditDelete()
        {
            try
            {
                this.SetEditData(this._dtDispData);
                if (string.IsNullOrEmpty(ComFunc.GetFld(this._dtDispData, 0, Def_M_PROJECT.PROJECT_NO)))
                {
                    // 該当する物件名はありません。
                    this.ShowMessage("M0100170008");
                    return false;
                }

                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.EcsQuota = ComFunc.GetFld(this._dtDispData, 0, Def_M_ECS.ECS_QUOTA);
                cond.EcsNo = ComFunc.GetFld(this._dtDispData, 0, Def_M_ECS.ECS_NO);
                cond.BukkenName = this.cboBukkenName.Text;

                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtDispData.Copy());

                string errMsgID;
                string[] args;
                if (!conn.DelEcsData(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID))
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
        /// <create>H.Tsuji 2018/11/06</create>
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
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F7ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsuji 2018/11/06</create>
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
                this.txtSearchQuota.Focus();
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F12ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsuji 2018/11/06</create>
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
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
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
        /// <create>H.Tsuji 2018/11/21</create>
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
        /// <create>H.Tsuji 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                this.DisplayClearEdit();
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
        /// <create>H.Tsuji 2018/11/21</create>
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
        /// <create>H.Tsuji 2018/11/21</create>
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

        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>H.Tsuji 2018/11/06</create>
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
        /// <create>H.Tsuji 2018/11/06</create>
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
                        this.txtQuota.Enabled = true;
                        this.txtEcsNo.Enabled = true;
                        this.fbrFunction.F01Button.Enabled = true;
                        break;
                    case DisplayMode.Update:
                        // ----- 変更 -----
                        this.grpSearch.Enabled = false;
                        this.grpEdit.Enabled = true;
                        this.txtQuota.Enabled = false;
                        this.txtEcsNo.Enabled = false;
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

        #endregion

        #region 登録情報クリア

        /// --------------------------------------------------
        /// <summary>
        /// 登録情報クリア
        /// </summary>
        /// <create>H.Tsuji 2018/11/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private void DisplayClearEdit()
        {
            try
            {
                this.txtQuota.Text = string.Empty;
                this.txtEcsNo.Text = string.Empty;
                this.txtArNo.Text = string.Empty;
                this.cboBukkenName.SelectedIndex = -1;
                this.txtSeiban.Text = string.Empty;
                this.txtCode.Text = string.Empty;
                this.cboKishu.SelectedIndex = -1;
                this.cboKanriFlag.SelectedValue = KANRI_FLAG.DEFAULT_VALUE1;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 画面表示

        /// --------------------------------------------------
        /// <summary>
        /// プロジェクト一覧表示(検索・編集グループの2つ)
        /// </summary>
        /// <create>H.Tsuji 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ShowProjectList()
        {
            // プロジェクトマスタに登録されている物件名をコンボボックスにセット
            ConnM01 conn = new ConnM01();
            CondM01 cond = new CondM01(this.UserInfo);

            DataSet ds = conn.GetProject(cond);
            string tableName = Def_M_PROJECT.Name;

            DataTable dt = ds.Tables[tableName];
            this.MakeCmbBox(this.cboSearchBukkenName, dt, Def_M_PROJECT.BUKKEN_NAME, Def_M_PROJECT.BUKKEN_NAME, string.Empty, true);
            this.MakeCmbBox(this.cboBukkenName, dt, Def_M_PROJECT.BUKKEN_NAME, Def_M_PROJECT.BUKKEN_NAME, string.Empty, true);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 機種一覧表示
        /// </summary>
        /// <create>H.Tsuji 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ShowKishuList()
        {
            // 機種をコンボボックスにセット
            ConnM01 conn = new ConnM01();
            CondM01 cond = new CondM01(this.UserInfo);
            cond.SelectGroupCD = Commons.SELECT_GROUP_CD.KISHU_VALUE1;

            DataSet ds = conn.GetSelectItemForKishu(cond);
            string tableName = Def_M_SELECT_ITEM.Name;

            DataTable dt = ds.Tables[tableName];
            this.MakeCmbBox(this.cboKishu, dt, Def_M_SELECT_ITEM.ITEM_NAME, Def_M_SELECT_ITEM.ITEM_NAME, string.Empty, true);
        }

        #endregion

        #region データ選択

        /// --------------------------------------------------
        /// <summary>
        /// データ選択時のチェック
        /// </summary>
        /// <param name="selectType">データ選択時の状態</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tsuji 2018/11/22</create>
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
        /// <create>H.Tsuji 2018/11/22</create>
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
                                this.txtQuota.Focus();
                                break;
                            case DataSelectType.Update:
                                this.EditMode = SystemBase.EditMode.Update;
                                // モード切り替え
                                this.ChangeMode(DisplayMode.Update);
                                this.txtArNo.Focus();
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
        /// <create>H.Tsuji 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RunDataSelectExec(DataSelectType selectType)
        {
            try
            {
                if (selectType == DataSelectType.Insert)
                {
                    // ----- 登録 -----
                    this._dtDispData = this.GetSchemaEcs();
                    // 1行追加しておく
                    this._dtDispData.Rows.Add(this._dtDispData.NewRow());
                }
                else
                {
                    // ----- 変更、削除 -----
                    ConnM01 conn = new ConnM01();
                    CondM01 cond = new CondM01(this.UserInfo);
                    cond.EcsQuota = this.shtResult[SHEET_COL_ECS_QUOTA, shtResult.ActivePosition.Row].Text;
                    cond.EcsNo = this.shtResult[SHEET_COL_ECS_NO, shtResult.ActivePosition.Row].Text;

                    DataSet ds = conn.GetEcs(cond);
                    if (!ComFunc.IsExistsData(ds, Def_M_ECS.Name))
                    {
                        // 既に削除された技連です。
                        this.ShowMessage("M0100170009");
                        // 消えてるのがあったから取り敢えず検索
                        this.RunSearch();
                        return false;
                    }
                    this._dtDispData = ds.Tables[Def_M_ECS.Name];
                    // 表示データ設定
                    this.txtQuota.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_ECS.ECS_QUOTA);
                    this.txtEcsNo.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_ECS.ECS_NO);
                    this.txtArNo.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_ECS.AR_NO);
                    if (!string.IsNullOrEmpty(this.txtArNo.Text))
                    {
                        // 先頭「AR」切り取り
                        this.txtArNo.Text = this.txtArNo.Text.Remove(0, ComDefine.PREFIX_ARNO.Length);
                    }
                    this.cboBukkenName.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.BUKKEN_NAME);
                    this.txtSeiban.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_ECS.SEIBAN);
                    this.txtCode.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_ECS.CODE);
                    this.cboKishu.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_ECS.KISHU);
                    this.cboKanriFlag.SelectedValue = ComFunc.GetFld(this._dtDispData, 0, Def_M_ECS.KANRI_FLAG);
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

        #region 技連保守のデータテーブル作成
        /// --------------------------------------------------
        /// <summary>
        /// 技連保守のデータテーブル作成
        /// </summary>
        /// <returns>技連保守のデータテーブル</returns>
        /// <create>H.Tsuji 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSchemaEcs()
        {
            try
            {
                DataTable dt = new DataTable(Def_M_ECS.Name);
                dt.Columns.Add(Def_M_ECS.ECS_QUOTA, typeof(string));
                dt.Columns.Add(Def_M_ECS.ECS_NO, typeof(string));
                dt.Columns.Add(Def_M_ECS.PROJECT_NO, typeof(string));
                dt.Columns.Add(Def_M_PROJECT.BUKKEN_NAME, typeof(string));
                dt.Columns.Add(Def_M_ECS.AR_NO, typeof(string));
                dt.Columns.Add(Def_M_ECS.SEIBAN, typeof(string));
                dt.Columns.Add(Def_M_ECS.CODE, typeof(string));
                dt.Columns.Add(Def_M_ECS.KISHU, typeof(string));
                dt.Columns.Add(Def_M_ECS.SEIBAN_CODE, typeof(string));
                dt.Columns.Add(Def_M_ECS.KANRI_FLAG, typeof(string));
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
        /// <param name="dt">技連マスタ登録データ</param>
        /// <create>H.Tsuji 2018/11/22</create>
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
                dr[Def_M_ECS.ECS_QUOTA] = this.txtQuota.Text;
                dr[Def_M_ECS.ECS_NO] = this.txtEcsNo.Text;
                if (string.IsNullOrEmpty(this.txtArNo.Text))
                {
                    dr[Def_M_ECS.AR_NO] = DBNull.Value;
                }
                else
                {
                    dr[Def_M_ECS.AR_NO] = ComDefine.PREFIX_ARNO + this.txtArNo.Text;
                }
                dr[Def_M_ECS.SEIBAN] = this.txtSeiban.Text;
                dr[Def_M_ECS.CODE] = this.txtCode.Text;
                dr[Def_M_ECS.SEIBAN_CODE] = this.txtSeiban.Text + this.txtCode.Text;
                if (string.IsNullOrEmpty(this.cboKishu.Text))
                {
                    dr[Def_M_ECS.KISHU] = DBNull.Value;
                }
                else
                {
                    dr[Def_M_ECS.KISHU] = this.cboKishu.Text;
                }
                dr[Def_M_ECS.KANRI_FLAG] = this.cboKanriFlag.SelectedValue;

                // 物件名からプロジェクトNo取得
                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.BukkenName = this.cboBukkenName.Text;
                DataSet ds = conn.GetProject(cond);
                dr[Def_M_ECS.PROJECT_NO] = ComFunc.GetFld(ds, Def_M_PROJECT.Name, 0, Def_M_PROJECT.PROJECT_NO);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }
        #endregion

    }
}
