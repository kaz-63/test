using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Commons;
using DSWUtil;
using GrapeCity.Win.ElTabelle;
using SystemBase.Util;
using WsConnection;
using WsConnection.WebRefCommon;
using WsConnection.WebRefM01;
using SMS.M01.Properties;

namespace SMS.M01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 物件名マスタ保守
    /// </summary>
    /// <create>K.Saeki 2012/03/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class BukkenMeiHoshu : SystemBase.Forms.CustomOrderForm
    {
        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// 画面の表示モード
        /// </summary>
        /// <create>K.Saeki 2012/03/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DisplayMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 初期
            /// </summary>
            /// <create>K.Saeki 2012/03/23</create>
            /// <update></update>
            /// --------------------------------------------------
            Initialize,
            /// --------------------------------------------------
            /// <summary>
            /// 検索後
            /// </summary>
            /// <create>K.Saeki 2012/03/23</create>
            /// <update></update>
            /// --------------------------------------------------
            EndSearch,
            /// --------------------------------------------------
            /// <summary>
            /// 登録
            /// </summary>
            /// <create>K.Saeki 2012/03/23</create>
            /// <update></update>
            /// --------------------------------------------------
            Insert,
            /// --------------------------------------------------
            /// <summary>
            /// 変更
            /// </summary>
            /// <create>K.Saeki 2012/03/23</create>
            /// <update></update>
            /// --------------------------------------------------
            Update,
            /// --------------------------------------------------
            /// <summary>
            /// 削除
            /// </summary>
            /// <create>K.Saeki 2012/03/23</create>
            /// <update></update>
            /// --------------------------------------------------
            Delete,
            /// --------------------------------------------------
            /// <summary>
            /// 編集
            /// </summary>
            /// <create>K.Saeki 2012/03/27</create>
            /// <update></update>
            /// --------------------------------------------------
            Edit,
        }

        /// --------------------------------------------------
        /// <summary>
        /// データ選択時の状態
        /// </summary>
        /// <create>K.Saeki 2012/03/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DataSelectType
        {
            /// --------------------------------------------------
            /// <summary>
            /// 登録
            /// </summary>
            /// <create>K.Saeki 2012/03/23</create>
            /// <update></update>
            /// --------------------------------------------------
            Insert,
            /// --------------------------------------------------
            /// <summary>
            /// 変更
            /// </summary>
            /// <create>K.Saeki 2012/03/23</create>
            /// <update></update>
            /// --------------------------------------------------
            Update,
            /// --------------------------------------------------
            /// <summary>
            /// 削除
            /// </summary>
            /// <create>K.Saeki 2012/03/23</create>
            /// <update></update>
            /// --------------------------------------------------
            Delete,
            /// --------------------------------------------------
            /// <summary>
            /// メール設定
            /// </summary>
            /// <create>T.Sakiori 2017/09/14</create>
            /// <update></update>
            /// --------------------------------------------------
            Mail,
        }

        #endregion

        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分のカラムインデックス
        /// </summary>
        /// <create>K.Saeki 2012/03/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SHUKKA_FLAG = 4;
        /// --------------------------------------------------
        /// <summary>
        /// 物件管理Noのカラムインデックス
        /// </summary>
        /// <create>K.Saeki 2012/03/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_BUKKEN_NO = 3;
        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>K.Saeki 2012/03/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 0;

        #endregion

        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 表示時に使用したデータ
        /// </summary>
        /// <create>K.Saeki 2012/03/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtDispData = null;
        // ↓↓↓ M.Shimizu 2020/07/03 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）
        /// --------------------------------------------------
        /// <summary>
        /// メール送信判定
        /// </summary>
        /// <create>M.Shimizu 2020/07/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isMailSend = false;
        // ↑↑↑ M.Shimizu 2020/07/03 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）
        /// --------------------------------------------------
        /// <summary>
        /// メールアドレス
        /// </summary>
        /// <create>T.Sakiori 2017/09/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _mailAddress = string.Empty;

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
        /// <create>K.Saeki 2012/03/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public BukkenMeiHoshu(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>K.Saeki 2012/03/23</create>
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
                shtResult.ColumnHeaders[0].Caption = Resources.BukkenMeiHoshu_ShippingDivision;
                shtResult.ColumnHeaders[1].Caption = Resources.BukkenMeiHoshu_PropertyName;
                shtResult.ColumnHeaders[2].Caption = Resources.BukkenMeiHoshu_TagNo;
                shtResult.ColumnHeaders[3].Caption = Resources.BukkenMeiHoshu_ManagementNo;
                shtResult.ColumnHeaders[4].Caption = Resources.BukkenMeiHoshu_ShippingDivisionHidden;
                shtResult.ColumnHeaders[5].Caption = Resources.BukkenMeiHoshu_MailNotificationInvestment;

                // コンボボックスの初期化
                this.MakeCmbBox(this.cboSearchShukkaFlag, SHUKKA_FLAG.GROUPCD);
                this.MakeCmbBox(this.cboShukkaFlag, SHUKKA_FLAG.GROUPCD);
                this.MakeCmbBox(this.cboMailNotify, MAIL_NOTIFY.GROUPCD);

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
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                this.cboSearchShukkaFlag.SelectedValue = SHUKKA_FLAG.DEFAULT_VALUE1;
                this.txtSearchBukkenName.Text = string.Empty;
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
        /// <create>K.Saeki 2012/03/23</create>
        /// <update>M.Shimizu 2020/07/03 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）</update>
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
                    if (string.IsNullOrEmpty(this.txtBukkenName.Text))
                    {
                        // 物件名を入力して下さい。
                        this.ShowMessage("M0100060002");
                        this.txtBukkenName.Focus();
                        return false;
                    }

                    if (this.EditMode == SystemBase.EditMode.Insert
                        && this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.AR_VALUE1)
                    {
                        var conn = new ConnCommon();
                        var cond = new CondCommon(this.UserInfo);
                        var ds = conn.CheckMail(cond);

                        if (UtilData.GetFld(ds, Def_M_MAIL_SETTING.Name, 0, Def_M_MAIL_SETTING.BUKKEN_ADD_EVENT) == BUKKEN_ADD_EVENT.YES_VALUE1)
                        {
                            if (UtilData.GetFldToInt32(ds, Def_M_BUKKEN_MAIL.Name, 0, ComDefine.FLD_CNT) == 0)
                            {
                                // 基本通知設定で送信先が登録されていません。
                                this.ShowMessage("A9999999057");
                                return false;
                            }
                            if (string.IsNullOrEmpty(UtilData.GetFld(ds, Def_M_USER.Name, 0, Def_M_USER.MAIL_ADDRESS)))
                            {
                                // 担当者にMailAddressが設定されていません。
                                this.ShowMessage("A0100010010");
                                return false;
                            }
                            foreach (DataRow dr in ds.Tables[Def_M_BUKKEN_MAIL_MEISAI.Name].Rows)
                            {
                                if (string.IsNullOrEmpty(UtilData.GetFld(dr, Def_M_USER.MAIL_ADDRESS)))
                                {
                                    // MailAddress未設定のUserがいます。【ID：{0}、Name：{1}】
                                    this.ShowMessage("A9999999060", UtilData.GetFld(dr, Def_M_USER.USER_ID), UtilData.GetFld(dr, Def_M_USER.USER_NAME));
                                    return false;
                                }
                            }

                            // ↓↓↓ M.Shimizu 2020/07/03 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）
                            // メール送信判定は物件追加イベントで判定する
                            // this._templateFolder = UtilData.GetFld(ds, Def_M_MAIL_SETTING.Name, 0, Def_M_MAIL_SETTING.TEMPLATE_FOLDER);
                            this._isMailSend = true;
                            // ↑↑↑ M.Shimizu 2020/07/03 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）

                            this._mailAddress = UtilData.GetFld(ds, Def_M_USER.Name, 0, Def_M_USER.MAIL_ADDRESS);
                        }
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
        /// <create>K.Saeki 2012/03/23</create>
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
        /// <create>K.Saeki 2012/03/23</create>
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
        /// <create>K.Saeki 2012/03/23</create>
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
                cond.ShukkaFlag = this.cboSearchShukkaFlag.SelectedValue.ToString();
                cond.BukkenName = this.txtSearchBukkenName.Text;
                DataSet ds = conn.GetBukkenLikeSearch(cond);

                if (!ComFunc.IsExistsData(ds, Def_M_BUKKEN.Name))
                {
                    // シートのクリア
                    this.SheetClear();
                    // 該当物件名はありません。
                    this.ShowMessage("M0100060001");
                    return false;
                }

                this.shtResult.DataSource = ds;
                this.shtResult.DataMember = Def_M_BUKKEN.Name;
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
        /// <create>K.Saeki 2012/03/23</create>
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
        /// <create>K.Saeki 2012/03/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                this.SetEditData(this._dtDispData);

                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.ShukkaFlag = ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.SHUKKA_FLAG);
                cond.BukkenName = ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.BUKKEN_NAME);

                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtDispData.Copy());

                // メールデータ追加
                if (this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.AR_VALUE1)
                {
                    var ret = this.CreateMailData();
                    if (ret != null)
                    {
                        ds.Tables.Add(ret);
                    }
                }

                string errMsgID;
                string[] args;
                if (!conn.InsBukkenData(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID))
                    {
                        this.DisplayClearEdit(DisplayMode.Insert);
                        this.RunSearch();
                    }
                    else if (errMsgID == "M0100060003")
                    {
                        this.RunSearch();
                        this.ChangeMode(DisplayMode.Insert);
                        this.txtBukkenName.Focus();
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
        /// <create>K.Saeki 2012/03/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                this.SetEditData(this._dtDispData);

                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.ShukkaFlag = ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.SHUKKA_FLAG);
                cond.BukkenName = ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.BUKKEN_NAME);
                cond.BukkenNo = ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.BUKKEN_NO);

                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtDispData.Copy());

                string errMsgID;
                string[] args;
                if (!conn.UpdBukkenData(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID)
                        || errMsgID == "M0100060004")
                    {
                        this.DisplayClearEdit(DisplayMode.Update);
                        this.RunSearch();
                    }
                    else if (errMsgID == "M0100060003")
                    {
                        this.RunSearch();
                        var dt = (this.shtResult.DataSource as DataSet).Tables[Def_M_BUKKEN.Name];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (cond.ShukkaFlag == ComFunc.GetFld(dt, i, Def_M_BUKKEN.SHUKKA_FLAG)
                                && cond.BukkenNo == ComFunc.GetFld(dt, i, Def_M_BUKKEN.BUKKEN_NO))
                            {
                                this.shtResult.TopLeft = new Position(0, i);
                                this.shtResult.ActivePosition = new Position(0, i);
                            }
                        }
                        this.ChangeMode(DisplayMode.Update);
                        this.txtBukkenName.Focus();
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
        /// <create>K.Saeki 2012/03/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditDelete()
        {
            try
            {
                this.SetEditData(this._dtDispData);

                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.ShukkaFlag = ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.SHUKKA_FLAG);
                cond.BukkenName = ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.BUKKEN_NAME);
                cond.BukkenNo = ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.BUKKEN_NO);

                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtDispData.Copy());

                string errMsgID;
                string[] args;
                if (!conn.DelBukkenData(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID)
                        || errMsgID == "M0100060004")
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
        /// <create>K.Saeki 2012/03/23</create>
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
        /// <create>K.Saeki 2012/03/23</create>
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
        /// <create>K.Saeki 2012/03/26</create>
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

        #region 共通メールボタン

        /// --------------------------------------------------
        /// <summary>
        /// 共通メールボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2017/09/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnCommonNotify_Click(object sender, EventArgs e)
        {
            if (!this.RunDataSelect(DataSelectType.Mail))
            {
                return;
            }

            var conn = new ConnM01();
            var cond = new CondM01(this.UserInfo);
            if (!conn.ExistsMailChangeRole(cond))
            {
                // MailAddress変更権限を持っていないため設定できません。
                this.ShowMessage("A9999999059");
                return;
            }

            using (var f = new CommonNotify(this.UserInfo, ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.BUKKEN_NO), ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.BUKKEN_NAME)))
            {
                f.ShowDialog();
            }
        }

        #endregion

        #region ARList名単位メール設定ボタン

        /// --------------------------------------------------
        /// <summary>
        /// ARList名単位メール設定ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2017/09/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnARListNotify_Click(object sender, EventArgs e)
        {
            if (!this.RunDataSelect(DataSelectType.Mail))
            {
                return;
            }

            var conn = new ConnM01();
            var cond = new CondM01(this.UserInfo);
            cond.BukkenNo = ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.BUKKEN_NO);
            if (!conn.ExistsBukkenMail(cond))
            {
                this.ShowMessage("M0100060006");
                return;
            }
            if (!conn.ExistsMailChangeRole(cond))
            {
                // MailAddress変更権限を持っていないため設定できません。
                this.ShowMessage("A9999999059");
                return;
            }

            using (var f = new ARListNotify(this.UserInfo, ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.BUKKEN_NO), ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.BUKKEN_NAME)))
            {
                f.ShowDialog();
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 進捗管理Mail設定ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/04</create>
        /// <update>D.Okumura 2019/08/07 メッセージを変更(M0100120008→A9999999057)</update>
        /// --------------------------------------------------
        private void btnShinchokuKanriNotify_Click(object sender, EventArgs e)
        {
            if (!this.RunDataSelect(DataSelectType.Mail))
            {
                return;
            }

            var conn = new ConnM01();
            var cond = new CondM01(this.UserInfo);
            cond.BukkenNo = ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.BUKKEN_NO);
            if (!conn.ExistsShinchokuKanriMail(cond))
            {
                // 基本通知設定で送信先が登録されていません。
                this.ShowMessage("A9999999057");
                return;
            }
            if (!conn.ExistsMailChangeRole(cond))
            {
                // MailAddress変更権限を持っていないため設定できません。
                this.ShowMessage("A9999999059");
                return;
            }

            using (var f = new ShinchokuNotify(this.UserInfo, ComDefine.TITLE_M0100180, ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.BUKKEN_NO), ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.BUKKEN_NAME)))
            {
                f.ShowDialog();
            }
        }

        #endregion

        #region 出荷区分コンボボックス変更時

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分コンボボックス変更時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>R.Katsuo 2017/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboShukkaFlag_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.ChangeShukkaFlag();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分コンボボックス変更時に処理
        /// </summary>
        /// <create>R.Katsuo 2017/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeShukkaFlag()
        {
            if (this.cboShukkaFlag.SelectedIndex <= -1)
            {
                return;
            }
            if (this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1)
            {
                this.cboMailNotify.SelectedValue = MAIL_NOTIFY.STOP_VALUE1;
                this.cboMailNotify.Enabled = false;
            }
            else
            {
                this.cboMailNotify.SelectedValue = MAIL_NOTIFY.STOP_VALUE1;
                this.cboMailNotify.Enabled = true;
            }
        }

        #endregion
        
        #endregion

        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>K.Saeki 2012/03/23</create>
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
        /// <create>K.Saeki 2012/03/23</create>
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
                        this.cboShukkaFlag.Enabled = true;
                        this.txtBukkenName.Enabled = true;
                        this.fbrFunction.F01Button.Enabled = true;
                        break;
                    case DisplayMode.Update:
                        // ----- 変更 -----
                        this.grpSearch.Enabled = false;
                        this.grpEdit.Enabled = true;
                        this.cboShukkaFlag.Enabled = false;
                        this.txtBukkenName.Enabled = true;
                        this.fbrFunction.F01Button.Enabled = true;
                        break;
                    case DisplayMode.Delete:
                        // ----- 削除 -----
                        this.grpSearch.Enabled = false;
                        this.grpEdit.Enabled = true;
                        this.cboShukkaFlag.Enabled = false;
                        this.txtBukkenName.Enabled = false;
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
        /// <param name="mode">画面の表示モード</param>
        /// <create>K.Saeki 2012/03/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private void DisplayClearEdit(DisplayMode mode)
        {
            try
            {
                if (mode == DisplayMode.Initialize)
                {
                    this.cboShukkaFlag.SelectedValue = SHUKKA_FLAG.DEFAULT_VALUE1;
                    this.ChangeShukkaFlag();
                    this.cboMailNotify.SelectedValue = MAIL_NOTIFY.DEFAULT_VALUE1;
                }
                this.txtBukkenName.Text = string.Empty;
                this.txtIssuedTagNo.Text = string.Empty;
                this.txtBukkenNo.Text = string.Empty;
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
        /// <create>K.Saeki 2012/03/26</create>
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
        /// <create>K.Saeki 2012/03/26</create>
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
                                this.txtBukkenName.Focus();
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
        /// <create>K.Saeki 2012/03/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RunDataSelectExec(DataSelectType selectType)
        {
            try
            {
                if (selectType == DataSelectType.Insert)
                {
                    // ----- 登録 -----
                    this._dtDispData = this.GetSchemeBukken();
                    // 1行追加しておく
                    this._dtDispData.Rows.Add(this._dtDispData.NewRow());
                    this.ChangeShukkaFlag();
                }
                else
                {
                    // ----- 変更、削除 -----
                    ConnM01 conn = new ConnM01();
                    CondM01 cond = new CondM01(this.UserInfo);
                    cond.ShukkaFlag = this.shtResult[SHEET_COL_SHUKKA_FLAG, shtResult.ActivePosition.Row].Text;
                    cond.BukkenNo = this.shtResult[SHEET_COL_BUKKEN_NO, shtResult.ActivePosition.Row].Text;
                    DataSet ds = conn.GetBukken(cond);
                    if (!ComFunc.IsExistsData(ds, Def_M_BUKKEN.Name))
                    {
                        // 既に削除された物件名です。
                        this.ShowMessage("M0100060004");
                        // 消えてるのがあったから取り敢えず検索
                        this.RunSearch();
                        return false;
                    }
                    this._dtDispData = ds.Tables[Def_M_BUKKEN.Name];
                }

                if (selectType != DataSelectType.Mail)
                {
                    // 表示データ設定
                    if (!string.IsNullOrEmpty(ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.SHUKKA_FLAG)))
                    {
                        this.cboShukkaFlag.SelectedValue = ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.SHUKKA_FLAG);
                    }
                    this.txtBukkenName.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.BUKKEN_NAME);
                    this.txtIssuedTagNo.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.ISSUED_TAG_NO);
                    this.txtBukkenNo.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.BUKKEN_NO);
                    this.ChangeShukkaFlag();
                    if (!string.IsNullOrEmpty(ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.MAIL_NOTIFY)))
                    {
                        this.cboMailNotify.SelectedValue = ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.MAIL_NOTIFY);
                    }
                }
                else
                {
                    if (ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.SHUKKA_FLAG) == SHUKKA_FLAG.NORMAL_VALUE1)
                    {
                        // 出荷区分が本体のDataは設定できません。
                        this.ShowMessage("M0100060007");
                        return false;
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

        #endregion

        #region 物件マスタのデータテーブル作成

        /// --------------------------------------------------
        /// <summary>
        /// 物件マスタのデータテーブル作成
        /// </summary>
        /// <returns>物件マスタのデータテーブル</returns>
        /// <create>K.Saeki 2012/03/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSchemeBukken()
        {
            try
            {
                DataTable dt = new DataTable(Def_M_BUKKEN.Name);
                dt.Columns.Add(Def_M_BUKKEN.SHUKKA_FLAG, typeof(string));
                dt.Columns.Add(Def_M_BUKKEN.BUKKEN_NO, typeof(decimal));
                dt.Columns.Add(Def_M_BUKKEN.BUKKEN_NAME, typeof(string));
                dt.Columns.Add(Def_M_BUKKEN.ISSUED_TAG_NO, typeof(decimal));
                dt.Columns.Add(Def_M_BUKKEN.CREATE_DATE, typeof(object));
                dt.Columns.Add(Def_M_BUKKEN.CREATE_USER_ID, typeof(string));
                dt.Columns.Add(Def_M_BUKKEN.CREATE_USER_NAME, typeof(string));
                dt.Columns.Add(Def_M_BUKKEN.UPDATE_DATE, typeof(object));
                dt.Columns.Add(Def_M_BUKKEN.UPDATE_USER_ID, typeof(string));
                dt.Columns.Add(Def_M_BUKKEN.UPDATE_USER_NAME, typeof(string));
                dt.Columns.Add(Def_M_BUKKEN.MAINTE_DATE, typeof(object));
                dt.Columns.Add(Def_M_BUKKEN.MAINTE_USER_ID, typeof(string));
                dt.Columns.Add(Def_M_BUKKEN.MAINTE_USER_NAME, typeof(string));
                dt.Columns.Add(Def_M_BUKKEN.VERSION, typeof(object));
                dt.Columns.Add(Def_M_BUKKEN.MAINTE_VERSION, typeof(object));
                dt.Columns.Add(Def_M_BUKKEN.MAIL_NOTIFY, typeof(object));
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
        /// <param name="dt">物件マスタデータテーブル</param>
        /// <create>K.Saeki 2012/03/26</create>
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
                dr[Def_M_BUKKEN.SHUKKA_FLAG] = this.cboShukkaFlag.SelectedValue.ToString();
                dr[Def_M_BUKKEN.BUKKEN_NAME] = this.txtBukkenName.Text;
                if (!string.IsNullOrEmpty(this.txtIssuedTagNo.Text))
                {
                    dr[Def_M_BUKKEN.ISSUED_TAG_NO] = UtilConvert.ToDecimal(this.txtIssuedTagNo.Text);
                }
                if (!string.IsNullOrEmpty(this.txtBukkenNo.Text))
                {
                    dr[Def_M_BUKKEN.BUKKEN_NO] = UtilConvert.ToDecimal(this.txtBukkenNo.Text);
                }
                dr[Def_M_BUKKEN.MAIL_NOTIFY] = this.cboMailNotify.SelectedValue;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region メール関連

        /// --------------------------------------------------
        /// <summary>
        /// メール登録用データ作成
        /// </summary>
        /// <returns></returns>
        /// <create>T.Sakiori 2017/09/22</create>
        /// <update>M.Shimizu 2020/07/03 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）</update>
        /// --------------------------------------------------
        public DataTable CreateMailData()
        {
            // ↓↓↓ M.Shimizu 2020/07/03 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）
            // 物件追加イベントが'1'でない場合、メール送信データは作成しない
            // if (string.IsNullOrEmpty(this._templateFolder))
            if (!this._isMailSend)
            {
                return null;
            }

            // メールテンプレートの内容を取得
            ConnAttachFile attachFile = new ConnAttachFile(this.UserInfo.Language);
            string title = attachFile.GetMailTemplate(MAIL_FILE.NEW_TITLE_VALUE1);
            string naiyo = attachFile.GetMailTemplate(MAIL_FILE.NEW_VALUE1);
            // ↑↑↑ M.Shimizu 2020/07/03 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）

            var conn = new ConnCommon();
            var cond = new CondCommon(this.UserInfo);
            cond.MailKbn = MAIL_KBN.BUKKEN_VALUE1;
            var ret = conn.GetMailData(cond);

            var dt = new DataTable(Def_T_MAIL.Name);
            dt.Columns.Add(Def_T_MAIL.MAIL_ID);
            dt.Columns.Add(Def_T_MAIL.MAIL_SEND);
            dt.Columns.Add(Def_T_MAIL.MAIL_SEND_DISPLAY);
            dt.Columns.Add(Def_T_MAIL.MAIL_TO);
            dt.Columns.Add(Def_T_MAIL.MAIL_TO_DISPLAY);
            dt.Columns.Add(Def_T_MAIL.MAIL_CC);
            dt.Columns.Add(Def_T_MAIL.MAIL_CC_DISPLAY);
            dt.Columns.Add(Def_T_MAIL.MAIL_BCC);
            dt.Columns.Add(Def_T_MAIL.MAIL_BCC_DISPLAY);
            dt.Columns.Add(Def_T_MAIL.TITLE);
            dt.Columns.Add(Def_T_MAIL.NAIYO);
            dt.Columns.Add(Def_T_MAIL.MAIL_STATUS);
            dt.Columns.Add(Def_T_MAIL.RETRY_COUNT);

            var dr = dt.NewRow();
            dr.SetField<object>(Def_T_MAIL.MAIL_SEND, this._mailAddress);
            dr.SetField<object>(Def_T_MAIL.MAIL_SEND_DISPLAY, this.UserInfo.UserName);
            dr.SetField<object>(Def_T_MAIL.MAIL_TO, ComFunc.GetMailUser(ret, MAIL_ADDRESS_FLAG.TO_VALUE1, Def_M_USER.MAIL_ADDRESS));
            dr.SetField<object>(Def_T_MAIL.MAIL_TO_DISPLAY, ComFunc.GetMailUser(ret, MAIL_ADDRESS_FLAG.TO_VALUE1, Def_M_USER.USER_NAME));
            dr.SetField<object>(Def_T_MAIL.MAIL_CC, ComFunc.GetMailUser(ret, MAIL_ADDRESS_FLAG.CC_VALUE1, Def_M_USER.MAIL_ADDRESS));
            dr.SetField<object>(Def_T_MAIL.MAIL_CC_DISPLAY, ComFunc.GetMailUser(ret, MAIL_ADDRESS_FLAG.CC_VALUE1, Def_M_USER.USER_NAME));
            dr.SetField<object>(Def_T_MAIL.MAIL_BCC, ComFunc.GetMailUser(ret, MAIL_ADDRESS_FLAG.BCC_VALUE1, Def_M_USER.MAIL_ADDRESS));
            dr.SetField<object>(Def_T_MAIL.MAIL_BCC_DISPLAY, ComFunc.GetMailUser(ret, MAIL_ADDRESS_FLAG.BCC_VALUE1, Def_M_USER.USER_NAME));
            // ↓↓↓ M.Shimizu 2020/07/03 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）
            dr.SetField<object>(Def_T_MAIL.TITLE, this.MailReplace(title));
            dr.SetField<object>(Def_T_MAIL.NAIYO, this.MailReplace(naiyo));
            // ↑↑↑ M.Shimizu 2020/07/03 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）
            dr.SetField<object>(Def_T_MAIL.MAIL_STATUS, MAIL_STATUS.MI_VALUE1);
            dr.SetField<object>(Def_T_MAIL.RETRY_COUNT, 0);
            dt.Rows.Add(dr);

            return dt;
        }

        /// --------------------------------------------------
        /// <summary>
        /// テンプレートのデータを置換
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <create>T.Sakiori 2017/09/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private string MailReplace(string source)
        {
            // 物件管理NoはDBで採番した後に置換
            return source
                .Replace(MAIL_RESERVE.BUKKEN_NAME_VALUE1, this.txtBukkenName.Text)
                //.Replace(MAIL_RESERVE.BUKKEN_NO_VALUE1, this.txtBukkenNo.Text)
                .Replace(MAIL_RESERVE.BUKKEN_CREATE_USER_VALUE1, this.UserInfo.UserName);
        }

        #endregion

    }
}
