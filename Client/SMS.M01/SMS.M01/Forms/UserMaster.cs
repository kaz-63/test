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
using SMS.E01;

namespace SMS.M01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 担当者マスタメンテナンス
    /// </summary>
    /// <create>Y.Higuchi 2010/08/24</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class UserMaster : SystemBase.Forms.CustomOrderForm
    {
        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// 画面の表示モード
        /// </summary>
        /// <create>Y.Higuchi 2010/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DisplayMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 初期
            /// </summary>
            /// <create>Y.Higuchi 2010/08/24</create>
            /// <update></update>
            /// --------------------------------------------------
            Initialize,
            /// --------------------------------------------------
            /// <summary>
            /// 検索後
            /// </summary>
            /// <create>Y.Higuchi 2010/08/24</create>
            /// <update></update>
            /// --------------------------------------------------
            EndSearch,
            /// --------------------------------------------------
            /// <summary>
            /// 登録
            /// </summary>
            /// <create>Y.Higuchi 2010/08/24</create>
            /// <update></update>
            /// --------------------------------------------------
            Insert,
            /// --------------------------------------------------
            /// <summary>
            /// 変更
            /// </summary>
            /// <create>Y.Higuchi 2010/08/24</create>
            /// <update></update>
            /// --------------------------------------------------
            Update,
            /// --------------------------------------------------
            /// <summary>
            /// 削除
            /// </summary>
            /// <create>Y.Higuchi 2010/08/24</create>
            /// <update></update>
            /// --------------------------------------------------
            Delete,
        }

        /// --------------------------------------------------
        /// <summary>
        /// データ選択時の状態
        /// </summary>
        /// <create>Y.Higuchi 2010/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DataSelectType
        {
            /// --------------------------------------------------
            /// <summary>
            /// 登録
            /// </summary>
            /// <create>Y.Higuchi 2010/08/24</create>
            /// <update></update>
            /// --------------------------------------------------
            Insert,
            /// --------------------------------------------------
            /// <summary>
            /// 変更
            /// </summary>
            /// <create>Y.Higuchi 2010/08/24</create>
            /// <update></update>
            /// --------------------------------------------------
            Update,
            /// --------------------------------------------------
            /// <summary>
            /// 削除
            /// </summary>
            /// <create>Y.Higuchi 2010/08/24</create>
            /// <update></update>
            /// --------------------------------------------------
            Delete,
        }

        #endregion

        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// ユーザーコードのカラムインデックス
        /// </summary>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_USER_ID = 0;
        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>Y.Higuchi 2010/09/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 0;

        #endregion

        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 表示時に使用したデータ
        /// </summary>
        /// <create>Y.Higuchi 2010/08/25</create>
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
        /// <create>Y.Higuchi 2010/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public UserMaster(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>Y.Higuchi 2010/08/24</create>
        /// <update>R.Katsuo 2017/09/05 メールアドレス変更権限追加</update>
        /// <update>H.Tajimi 2018/09/05 多言語対応</update>
        /// <update>H.Tajimi 2018/10/16 FE要望対応</update>
        /// <update>H.Tsuji 2018/12/11 荷姿表送信対象フラグ追加</update>
        /// <update>H.Tajimi 2019/08/14 TAG連携送信対象フラグ追加</update>
        /// <update>J.Chen 2024/01/31 計画取込一括設定追加</update>
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
                // 入力サイズの設定
                if (this.UserInfo != null && this.UserInfo.SysInfo != null)
                {
                    this.txtSearchUserID.MaxLength = this.UserInfo.SysInfo.MaxUserID;
                    this.txtUserID.MaxLength = this.UserInfo.SysInfo.MaxUserID;
                    this.txtNewPassword.MaxLength = this.UserInfo.SysInfo.MaxPassword;
                    this.txtConfirmPassword.MaxLength = this.UserInfo.SysInfo.MaxPassword;
                }
                // グリッド
                this.InitializeSheet(this.shtResult);
                this.shtResult.KeepHighlighted = true;
                // シートのタイトルを設定
                shtResult.ColumnHeaders[0].Caption = Resources.UserMaster_StaffCode;
                shtResult.ColumnHeaders[1].Caption = Resources.UserMaster_StaffName;
                shtResult.ColumnHeaders[2].Caption = Resources.UserMaster_MailAddress;
                shtResult.ColumnHeaders[3].Caption = Resources.UserMaster_ChangeAuthority;
                shtResult.ColumnHeaders[4].Caption = Resources.UserMaster_MailPackingFlag;
                shtResult.ColumnHeaders[5].Caption = Resources.UserMaster_MailTagRenkeiFlag;
                shtResult.ColumnHeaders[6].Caption = Resources.UserMaster_StaffKbn;
                shtResult.ColumnHeaders[7].Caption = Resources.UserMaster_MailShukkakeikakuFlag;
                shtResult.ColumnHeaders[8].Caption = Resources.UserMaster_Authority;
                shtResult.ColumnHeaders[9].Caption = Resources.UserMaster_Remarks;
                // 権限コンボボックス
                ConnMaster conn = new ConnMaster();
                CondRole cond = new CondRole(this.UserInfo);
                cond.RoleFlag = ROLE_FLAG.USER_VALUE1;
                DataSet ds = conn.GetRole(cond);
                this.cboRole.DisplayMember = Def_M_ROLE.ROLE_NAME;
                this.cboRole.ValueMember = Def_M_ROLE.ROLE_ID;
                this.cboRole.DataSource = ds.Tables[Def_M_ROLE.Name];
                // メールアドレス変更権限コンボボックス
                ConnCommon connCom = new ConnCommon();
                CondCommon condCom = new CondCommon(this.UserInfo);
                condCom.GroupCD = Def_M_USER.MAIL_CHANGE_ROLE;
                DataSet dsCom = connCom.GetCommon(condCom);
                this.cboMailChangeRole.DisplayMember = Def_M_COMMON.ITEM_NAME;
                this.cboMailChangeRole.ValueMember = Def_M_COMMON.VALUE1;
                this.cboMailChangeRole.DataSource = dsCom.Tables[Def_M_COMMON.Name];
                {
                    ConnCommon connCom2 = new ConnCommon();
                    CondCommon condCom2 = new CondCommon(this.UserInfo);
                    condCom.GroupCD = STAFF_KBN.GROUPCD;
                    DataSet dsCom2 = connCom.GetCommon(condCom);
                    this.cboStaffKbn.DisplayMember = Def_M_COMMON.ITEM_NAME;
                    this.cboStaffKbn.ValueMember = Def_M_COMMON.VALUE1;
                    this.cboStaffKbn.DataSource = dsCom2.Tables[Def_M_COMMON.Name];
                }
                // 荷姿表送信対象フラグコンボボックス
                this.MakeCmbBox(this.cboSearchMailPackingFlag, DISP_MAIL_PACKING_FLAG.GROUPCD);
                this.MakeCmbBox(this.cboMailPackingFlag, MAIL_PACKING_FLAG.GROUPCD);
                // TAG送信対象フラグコンボボックス
                this.MakeCmbBox(this.cboSearchMailTagRenkeiFlag, DISP_MAIL_TAG_RENKEI_FLAG.GROUPCD);
                this.MakeCmbBox(this.cboMailTagRenkeiFlag, MAIL_TAG_RENKEI_FLAG.GROUPCD);
                // 計画取込一括設定フラグコンボボックス
                this.MakeCmbBox(this.cboMailShukkakeikakuFlag, MAIL_SHUKKAKEIKAKU_FLAG.GROUPCD);

                // 登録情報クリア
                DisplayClearEdit();

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
        /// <create>Y.Higuchi 2010/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                this.txtSearchUserID.Focus();
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
        /// <create>Y.Higuchi 2010/08/24</create>
        /// <update>H.Tsuji 2018/12/11 荷姿表送信対象フラグ追加</update>
        /// <update>H.Tajimi 2019/08/14 TAG連携送信対象フラグ追加</update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                this.txtSearchUserID.Text = string.Empty;
                this.txtSearchUserName.Text = string.Empty;
                this.cboSearchMailPackingFlag.SelectedValue = DISP_MAIL_PACKING_FLAG.DEFAULT_VALUE1;
                this.cboSearchMailTagRenkeiFlag.SelectedValue = DISP_MAIL_TAG_RENKEI_FLAG.DEFAULT_VALUE1;
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
        /// <create>Y.Higuchi 2010/08/24</create>
        /// <update>R.Katsuo 2017/09/05 メールアドレス変更権限チェックの追加</update>
        /// <update>H.Tajimi 2018/10/16 FE要望対応</update>
        /// <update>H.Tsuji 2018/12/11 荷姿表送信対象フラグ追加</update>
        /// <update>H.Tajimi 2019/08/14 TAG連携送信対象フラグ追加</update>
        /// <update>J.Chen 2024/01/31 計画取込一括設定追加</update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                // ユーザーコードチェック
                if (string.IsNullOrEmpty(this.txtUserID.Text))
                {
                    // ユーザーコードを入力して下さい。
                    this.ShowMessage("M0100020001");
                    this.txtUserID.Focus();
                    return false;
                }

                // 削除時はここまで来ればOK
                if (this.EditMode == SystemBase.EditMode.Delete)
                {
                    return true;
                }

                // ユーザー名チェック
                if (string.IsNullOrEmpty(this.txtEditUserName.Text))
                {
                    // ユーザー名を入力して下さい。
                    this.ShowMessage("M0100020002");
                    this.txtUserID.Focus();
                    return false;
                }
                // パスワードチェック
                string errMsgID;
                string[] args = null;
                ret = ComFunc.CheckInputPassword(string.Empty, this.txtNewPassword.Text, this.txtConfirmPassword.Text, this.UserInfo.SysInfo.MinPassword, this.UserInfo.SysInfo.MaxPassword, this.UserInfo.SysInfo.PasswordCheck, true, out errMsgID, out args);
                if (!ret)
                {
                    if (string.IsNullOrEmpty(errMsgID))
                    {
                        CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT);
                        return false;
                    }
                    this.ShowMessage(errMsgID, args);
                    // フォーカスをセットする
                    this.ErrorSetFocus(errMsgID);
                    return false;
                }
                // 権限チェック
                if (this.cboRole.SelectedIndex < 0)
                {
                    // 権限を選択して下さい。
                    this.ShowMessage("M0100020007");
                    this.cboRole.Focus();
                    return false;
                }
                // メールアドレスチェック
                if (!string.IsNullOrEmpty(this.txtMailAddress.Text))
                {
                    if (!Regex.IsMatch(this.txtMailAddress.Text, @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$", RegexOptions.IgnoreCase))
                    {
                        // メールアドレスの形式が正しくありません。
                        this.ShowMessage("M0100020012");
                        this.txtMailAddress.Focus();
                        return false;
                    }
                }
                // メールアドレス変更権限チェック
                if (this.cboMailChangeRole.SelectedIndex < 0)
                {
                    // メールアドレス変更権限を選択して下さい。
                    this.ShowMessage("M0100020010");
                    this.cboMailChangeRole.Focus();
                    return false;
                }
                // 荷姿表送信対象チェック
                if (this.cboMailPackingFlag.SelectedIndex < 0)
                {
                    // 荷姿表送信対象を選択して下さい。
                    this.ShowMessage("M0100020014");
                    this.cboMailPackingFlag.Focus();
                    return false;
                }
                if (this.cboMailPackingFlag.SelectedValue.ToString() == MAIL_PACKING_FLAG.TARGET_VALUE1
                    && string.IsNullOrEmpty(this.txtMailAddress.Text))
                {
                    // 荷姿表送信対象者はMailAddressを入力して下さい。
                    this.ShowMessage("M0100020015");
                    this.txtMailAddress.Focus();
                    return false;
                }
                // TAG連携送信対象チェック
                if (this.cboMailTagRenkeiFlag.SelectedIndex < 0)
                {
                    // TAG連携送信対象を選択して下さい。
                    this.ShowMessage("M0100020023");
                    this.cboMailTagRenkeiFlag.Focus();
                    return false;
                }
                if (this.cboMailTagRenkeiFlag.SelectedValue.ToString() == MAIL_TAG_RENKEI_FLAG.TARGET_VALUE1
                    && string.IsNullOrEmpty(this.txtMailAddress.Text))
                {
                    // TAG連携送信対象者はMailAddressを入力して下さい。
                    this.ShowMessage("M0100020024");
                    this.txtMailAddress.Focus();
                    return false;
                }
                // スタッフ区分チェック
                if (this.cboStaffKbn.SelectedIndex < 0)
                {
                    // スタッフ区分を選択して下さい。
                    this.ShowMessage("M0100020013");
                    this.cboStaffKbn.Focus();
                    return false;
                }
                // 計画取込一括設定対象チェック
                if (this.cboMailShukkakeikakuFlag.SelectedIndex < 0)
                {
                    // 計画取込一括設定を選択して下さい。
                    this.ShowMessage("M0100020026");
                    this.cboMailShukkakeikakuFlag.Focus();
                    return false;
                }
                if (this.cboMailShukkakeikakuFlag.SelectedValue.ToString() == MAIL_SHUKKAKEIKAKU_FLAG.TARGET_VALUE1
                    && string.IsNullOrEmpty(this.txtMailAddress.Text))
                {
                    // 計画取込一括設定対象者はMailAddressを入力して下さい。
                    this.ShowMessage("M0100020027");
                    this.txtMailAddress.Focus();
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
        /// <create>Y.Higuchi 2010/08/24</create>
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
        /// <create>Y.Higuchi 2010/08/24</create>
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
        /// <create>Y.Higuchi 2010/08/24</create>
        /// <update>H.Tajimi 2018/09/05 多言語対応</update>
        /// <update>H.Tsuji 2018/12/11 荷姿表送信対象フラグ追加</update>
        /// <update>H.Tajimi 2019/08/14 TAG連携送信対象フラグ追加</update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                // モード切り替え(初期状態に戻す)
                this.ChangeMode(DisplayMode.Initialize);
                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.UserID = this.txtSearchUserID.Text;
                cond.UserName = this.txtSearchUserName.Text;
                cond.MailPackingFlag = this.cboSearchMailPackingFlag.SelectedValue.ToString();
                cond.MailTagRenkeiFlag = this.cboSearchMailTagRenkeiFlag.SelectedValue.ToString();
                DataSet ds = conn.GetUserLikeSearch(cond);

                if (!ComFunc.IsExistsData(ds, Def_M_USER.Name))
                {
                    // シートのクリア
                    this.SheetClear();
                    // 該当する明細は存在しません。
                    this.ShowMessage("A9999999022");
                    return false;
                }

                this.shtResult.DataSource = ds;
                this.shtResult.DataMember = Def_M_USER.Name;
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
        /// <create>Y.Higuchi 2010/08/24</create>
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
        /// <create>Y.Higuchi 2010/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                this.SetEditData(this._dtDispData);

                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.UserID = ComFunc.GetFld(this._dtDispData, 0, Def_M_USER.USER_ID);

                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtDispData.Copy());

                string errMsgID;
                string[] args;
                if (!conn.InsUserData(cond, ds, out errMsgID, out args))
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
        /// <create>Y.Higuchi 2010/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                this.SetEditData(this._dtDispData);

                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.UserID = ComFunc.GetFld(this._dtDispData, 0, Def_M_USER.USER_ID);

                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtDispData.Copy());

                string errMsgID;
                string[] args;
                if (!conn.UpdUserData(cond, ds, out errMsgID, out args))
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
        /// <create>Y.Higuchi 2010/08/24</create>
        /// <update>H.Tsuji 2019/06/24 メール送信対象者の削除チェック</update>
        /// --------------------------------------------------
        protected override bool RunEditDelete()
        {
            try
            {
                this.SetEditData(this._dtDispData);

                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.UserID = ComFunc.GetFld(this._dtDispData, 0, Def_M_USER.USER_ID);

                bool isMultiError = false;
                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtDispData.Copy());
                ds.Tables.Add(ComFunc.GetSchemeMultiMessage());
                if (!conn.DelUserData(cond, ref ds, out isMultiError))
                {
                    DataTable dtMessage = ds.Tables[ComDefine.DTTBL_MULTIMESSAGE];
                    if (isMultiError)
                    {
                        // メール通知を受けるユーザーは削除することができません。
                        this.ShowMultiMessage(dtMessage, "M0100020016");
                    }
                    else
                    {
                        string errMsgID = ComFunc.GetFld(dtMessage, 0, Def_M_MESSAGE.MESSAGE_ID);
                        string[] args = (string[])ComFunc.GetFldObject(dtMessage, 0, ComDefine.FLD_MESSAGE_PARAMETER);
                        if (ComFunc.IsVersionError(errMsgID))
                        {
                            this.DisplayClearEdit();
                            this.RunSearch();
                        }
                        this.ShowMessage(errMsgID, args);
                    }
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
        /// <create>Y.Higuchi 2010/08/24</create>
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
        /// <create>H.Tajimi 2015/12/08</create>
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
                this.txtSearchUserID.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F09ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/07/03</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F09Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F09Button_Click(sender, e);
            try
            {
                using (var frm = new SaveFileDialog())
                {
                    frm.Title = Resources.UserMaster_sdfExcel_Title;
                    frm.Filter = Resources.UserMaster_sdfExcel_Filter;
                    frm.FileName = ComDefine.EXCEL_FILE_USER_MASTER;
                    if (0 < this.shtResult.MaxRows && frm.ShowDialog() != DialogResult.OK) return;

                    // Excel出力処理
                    var dtExport = (this.shtResult.DataSource as DataSet).Tables[Def_M_USER.Name].Copy();
                    var export = new ExportUserMaster();
                    string msgID;
                    string[] args;
                    export.ExportExcel(frm.FileName, dtExport, out msgID, out args);
                    if (!string.IsNullOrEmpty(msgID))
                    {
                        this.ShowMessage(msgID, args);
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
        /// F12ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/08/24</create>
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
        /// <create>Y.Higuchi 2010/08/24</create>
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
        /// <create>Y.Higuchi 2010/08/24</create>
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
        /// <create>Y.Higuchi 2010/08/24</create>
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
        /// <create>Y.Higuchi 2010/08/24</create>
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
        /// <create>Y.Higuchi 2010/08/24</create>
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
        /// <create>Y.Higuchi 2010/08/24</create>
        /// <update>R.Katsuo 2017/09/05 メールアドレス関係のコントロール追加</update>
        /// <update>H.Tajimi 2018/10/16 FE要望対応</update>
        /// <update>H.Tsuji 2018/12/11 荷姿表送信対象フラグ追加</update>
        /// <update>H.Tajimi 2019/08/14 TAG連携送信対象フラグ追加</update>
        /// <update>J.Chen 2024/01/31 計画取込一括設定追加</update>
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
                        this.fbrFunction.F09Button.Enabled = false;
                        break;
                    case DisplayMode.EndSearch:
                        // ----- 検索後 -----
                        this.grpSearch.Enabled = true;
                        this.btnInsert.Enabled = true;
                        this.btnUpdate.Enabled = true;
                        this.btnDelete.Enabled = true;
                        this.grpEdit.Enabled = false;
                        this.fbrFunction.F01Button.Enabled = false;
                        this.fbrFunction.F09Button.Enabled = true;
                        break;
                    case DisplayMode.Insert:
                        // ----- 登録 -----
                        this.grpSearch.Enabled = false;
                        this.grpEdit.Enabled = true;
                        this.txtUserID.Enabled = true;
                        this.txtEditUserName.Enabled = true;
                        this.txtNewPassword.Enabled = true;
                        this.txtConfirmPassword.Enabled = true;
                        this.cboRole.Enabled = true;
                        this.txtMailAddress.Enabled = true;
                        this.cboMailChangeRole.Enabled = true;
                        this.cboMailPackingFlag.Enabled = true;
                        this.cboMailTagRenkeiFlag.Enabled = true;
                        this.cboStaffKbn.Enabled = true;
                        this.cboMailShukkakeikakuFlag.Enabled = true;
                        this.txtUserNote.Enabled = true;
                        this.fbrFunction.F01Button.Enabled = true;
                        break;
                    case DisplayMode.Update:
                        // ----- 変更 -----
                        this.grpSearch.Enabled = false;
                        this.grpEdit.Enabled = true;
                        this.txtUserID.Enabled = false;
                        this.txtEditUserName.Enabled = true;
                        this.txtNewPassword.Enabled = true;
                        this.txtConfirmPassword.Enabled = true;
                        this.cboRole.Enabled = true;
                        this.txtMailAddress.Enabled = true;
                        this.cboMailChangeRole.Enabled = true;
                        this.cboMailPackingFlag.Enabled = true;
                        this.cboMailTagRenkeiFlag.Enabled = true;
                        this.cboStaffKbn.Enabled = true;
                        this.cboMailShukkakeikakuFlag.Enabled = true;
                        this.txtUserNote.Enabled = true;
                        this.fbrFunction.F01Button.Enabled = true;
                        break;
                    case DisplayMode.Delete:
                        // ----- 削除 -----
                        this.grpSearch.Enabled = false;
                        this.grpEdit.Enabled = true;
                        this.txtUserID.Enabled = false;
                        this.txtEditUserName.Enabled = false;
                        this.txtNewPassword.Enabled = false;
                        this.txtConfirmPassword.Enabled = false;
                        this.cboRole.Enabled = false;
                        this.txtMailAddress.Enabled = false;
                        this.cboMailChangeRole.Enabled = false;
                        this.cboMailPackingFlag.Enabled = false;
                        this.cboMailTagRenkeiFlag.Enabled = false;
                        this.cboStaffKbn.Enabled = false;
                        this.cboMailShukkakeikakuFlag.Enabled = false;
                        this.txtUserNote.Enabled = false;
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
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update>R.Katsuo 2017/09/05 メールアドレス関係のコントロール追加</update>
        /// <update>H.Tajimi 2018/10/16 FE要望対応</update>
        /// <update>H.Tsuji 2018/12/11 荷姿表送信対象フラグ追加</update>
        /// <update>H.Tajimi 2019/08/14 TAG連携送信対象フラグ追加</update>
        /// <update>J.Chen 2024/01/31 計画取込一括設定追加</update>
        /// --------------------------------------------------
        private void DisplayClearEdit()
        {
            try
            {
                this.txtUserID.Text = string.Empty;
                this.txtEditUserName.Text = string.Empty;
                this.txtNewPassword.Text = string.Empty;
                this.txtConfirmPassword.Text = string.Empty;
                this.cboRole.Text = null;
                this.cboRole.SelectedIndex = -1;
                this.txtMailAddress.Text = string.Empty;
                this.cboMailChangeRole.Text = null;
                this.cboMailChangeRole.SelectedIndex = -1;
                this.cboMailPackingFlag.Text = null;
                this.cboMailPackingFlag.SelectedIndex = -1;
                this.cboMailTagRenkeiFlag.Text = null;
                this.cboMailTagRenkeiFlag.SelectedIndex = -1;
                this.cboStaffKbn.Text= null;
                this.cboStaffKbn.SelectedIndex = -1;
                this.cboMailShukkakeikakuFlag.Text = null;
                this.cboMailShukkakeikakuFlag.SelectedIndex = -1;
                this.txtUserNote.Text = string.Empty;
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
        /// <create>Y.Higuchi 2010/08/25</create>
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
        /// <create>Y.Higuchi 2010/08/25</create>
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
                                this.txtUserID.Focus();
                                break;
                            case DataSelectType.Update:
                                this.EditMode = SystemBase.EditMode.Update;
                                // モード切り替え
                                this.ChangeMode(DisplayMode.Update);
                                this.txtEditUserName.Focus();
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
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update>R.Katsuo 2017/09/05 メールアドレス関係のコントロール追加</update>
        /// <update>H.Tajimi 2018/09/05 多言語対応</update>
        /// <update>H.Tajimi 2019/08/14 TAG連携送信対象フラグ追加</update>
        /// <update>J.Chen 2024/01/31 計画取込一括設定追加</update>
        /// --------------------------------------------------
        private bool RunDataSelectExec(DataSelectType selectType)
        {
            try
            {
                if (selectType == DataSelectType.Insert)
                {
                    // ----- 登録 -----
                    this._dtDispData = this.GetSchemeUser();
                    // 1行追加しておく
                    this._dtDispData.Rows.Add(this._dtDispData.NewRow());
                }
                else
                {
                    // ----- 変更、削除 -----
                    ConnM01 conn = new ConnM01();
                    CondM01 cond = new CondM01(this.UserInfo);
                    cond.UserID = this.shtResult[SHEET_COL_USER_ID, shtResult.ActivePosition.Row].Text;
                    DataSet ds = conn.GetUser(cond);
                    if (!ComFunc.IsExistsData(ds, Def_M_USER.Name))
                    {
                        // 既に削除されたユーザーコードです。
                        this.ShowMessage("M01000200005");
                        // 消えてるのがあったから取り敢えず検索しとけ
                        this.RunSearch();
                        return false;
                    }
                    this._dtDispData = ds.Tables[Def_M_USER.Name];
                }
                // 表示データ設定
                this.txtUserID.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_USER.USER_ID);
                this.txtEditUserName.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_USER.USER_NAME);
                this.txtNewPassword.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_USER.PASSWORD);
                this.txtConfirmPassword.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_USER.PASSWORD);
                this.cboRole.SelectedValue = ComFunc.GetFld(this._dtDispData, 0, Def_M_USER.ROLE_ID);
                this.txtMailAddress.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_USER.MAIL_ADDRESS);
                this.cboMailChangeRole.SelectedValue = ComFunc.GetFld(this._dtDispData, 0, Def_M_USER.MAIL_CHANGE_ROLE);
                this.cboMailPackingFlag.SelectedValue = ComFunc.GetFld(this._dtDispData, 0, Def_M_USER.MAIL_PACKING_FLAG);
                this.cboMailTagRenkeiFlag.SelectedValue = ComFunc.GetFld(this._dtDispData, 0, Def_M_USER.MAIL_TAG_RENKEI_FLAG);
                this.cboStaffKbn.SelectedValue = ComFunc.GetFld(this._dtDispData, 0, Def_M_USER.STAFF_KBN);
                this.cboMailShukkakeikakuFlag.SelectedValue = ComFunc.GetFld(this._dtDispData, 0, Def_M_USER.MAIL_SHUKKAKEIKAKU_FLAG);
                this.txtUserNote.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_USER.USER_NOTE);
                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region ユーザーマスタのデータテーブル作成

        /// --------------------------------------------------
        /// <summary>
        /// ユーザーマスタのデータテーブル作成
        /// </summary>
        /// <returns>ユーザーマスタのデータテーブル</returns>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update>R.Katsuo 2017/09/05 メールアドレス関係追加</update>
        /// <update>H.Tsuji 2018/12/11 荷姿表送信対象フラグ追加</update>
        /// <update>H.Tajimi 2019/08/14 TAG連携送信対象フラグ追加</update>
        /// <update>J.Chen 2024/01/31 計画取込一括設定追加</update>
        /// --------------------------------------------------
        private DataTable GetSchemeUser()
        {
            try
            {
                DataTable dt = new DataTable(Def_M_USER.Name);
                dt.Columns.Add(Def_M_USER.USER_ID, typeof(string));
                dt.Columns.Add(Def_M_USER.USER_NAME, typeof(string));
                dt.Columns.Add(Def_M_USER.PASSWORD, typeof(string));
                dt.Columns.Add(Def_M_USER.ROLE_ID, typeof(string));
                dt.Columns.Add(Def_M_USER.USER_NOTE, typeof(string));
                dt.Columns.Add(Def_M_USER.USER_FLAG, typeof(string));
                dt.Columns.Add(Def_M_USER.PASSWORD_CHANGE_DATE, typeof(object));
                dt.Columns.Add(Def_M_USER.VERSION, typeof(object));
                dt.Columns.Add(Def_M_USER.MAIL_ADDRESS, typeof(string));
                dt.Columns.Add(Def_M_USER.MAIL_CHANGE_ROLE, typeof(string));
                dt.Columns.Add(Def_M_USER.MAIL_PACKING_FLAG, typeof(string));
                dt.Columns.Add(Def_M_USER.MAIL_TAG_RENKEI_FLAG, typeof(string));
                dt.Columns.Add(Def_M_USER.STAFF_KBN, typeof(string));
                dt.Columns.Add(Def_M_USER.MAIL_SHUKKAKEIKAKU_FLAG, typeof(string));
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
        /// <param name="dt">ユーザーマスタデータテーブル</param>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update>R.Katsuo 2017/09/05 メールアドレス関係追加</update>
        /// <update>H.Tajimi 2018/10/16 FE要望</update>
        /// <update>H.Tsuji 2018/12/11 荷姿表送信対象フラグ追加</update>
        /// <update>H.Tajimi 2019/08/14 TAG連携送信対象フラグ追加</update>
        /// <update>J.Chen 2024/01/31 計画取込一括設定追加</update>
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
                dr[Def_M_USER.USER_ID] = this.txtUserID.Text;
                dr[Def_M_USER.USER_NAME] = this.txtEditUserName.Text;
                dr[Def_M_USER.PASSWORD] = this.txtNewPassword.Text;
                dr[Def_M_USER.ROLE_ID] = this.cboRole.SelectedValue;
                dr[Def_M_USER.USER_NOTE] = this.txtUserNote.Text;
                dr[Def_M_USER.MAIL_ADDRESS] = this.txtMailAddress.Text;
                dr[Def_M_USER.MAIL_CHANGE_ROLE] = this.cboMailChangeRole.SelectedValue;
                dr[Def_M_USER.MAIL_PACKING_FLAG] = this.cboMailPackingFlag.SelectedValue;
                dr[Def_M_USER.MAIL_TAG_RENKEI_FLAG] = this.cboMailTagRenkeiFlag.SelectedValue;
                dr[Def_M_USER.STAFF_KBN] = this.cboStaffKbn.SelectedValue;
                dr[Def_M_USER.MAIL_SHUKKAKEIKAKU_FLAG] = this.cboMailShukkakeikakuFlag.SelectedValue;
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
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void ErrorSetFocus(string msgID)
        {
            switch (ComFunc.GetPasswordErrorType(msgID))
            {
                case PasswordErrorType.New:
                    this.txtNewPassword.Focus();
                    break;
                case PasswordErrorType.Confirm:
                    this.txtConfirmPassword.Focus();
                    break;
            }
        }

        #endregion

    }
}
