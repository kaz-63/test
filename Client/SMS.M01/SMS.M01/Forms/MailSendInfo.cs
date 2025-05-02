using System;
using System.Data;
using Commons;
using DSWUtil;
using SystemBase.Util;
using WsConnection;
using WsConnection.WebRefM01;
using System.Linq;
using WsConnection.WebRefCommon;
using SMS.M01.Properties;

namespace SMS.M01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// メール送信情報メンテナンス
    /// </summary>
    /// <create>R.Katsuo 2017/09/13</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class MailSendInfo : SystemBase.Forms.CustomOrderForm
    {
        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// 通知基本設定
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="menuCategoryID">メニュー種別ID</param>
        /// <param name="menuItemID">メニュー項目ID</param>
        /// <param name="title">画面タイトル</param>
        /// <create>R.Katsuo 2017/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public MailSendInfo(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>R.Katsuo 2017/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            try
            {
                // フォームの設定
                this.IsCloseQuestion = true;
                this.IsRunEditAfterClear = false;

                // グリッド
                this.InitializeSheet(this.shtMeisai);
                this.shtMeisai.KeepHighlighted = true;
                this.shtMeisai.EditType = GrapeCity.Win.ElTabelle.EditType.ReadOnly;
                this.shtMeisai.SelectionType = GrapeCity.Win.ElTabelle.SelectionType.Single;
                this.shtMeisai.ViewMode = GrapeCity.Win.ElTabelle.ViewMode.Row;

                // シートのタイトルを設定
                shtMeisai.ColumnHeaders[0].Caption = Resources.MailSendInfo_Subject;
                shtMeisai.ColumnHeaders[1].Caption = Resources.MailSendInfo_State;
                shtMeisai.ColumnHeaders[2].Caption = Resources.MailSendInfo_FailureCount;
                shtMeisai.ColumnHeaders[3].Caption = Resources.MailSendInfo_FailureReason;
                shtMeisai.ColumnHeaders[4].Caption = Resources.MailSendInfo_To;
                shtMeisai.ColumnHeaders[5].Caption = Resources.MailSendInfo_Cc;
                shtMeisai.ColumnHeaders[6].Caption = Resources.MailSendInfo_RegisterdDate;
                shtMeisai.ColumnHeaders[7].Caption = Resources.MailSendInfo_DateModified;

                // 状態コンボボックス
                ConnCommon conn = new ConnCommon();
                CondCommon cond = new CondCommon(this.UserInfo);
                cond.GroupCD = MAIL_STATUS.GROUPCD;
                DataSet ds = conn.GetCommon(cond);
                this.cboMailStatus.DisplayMember = Def_M_COMMON.ITEM_NAME;
                this.cboMailStatus.ValueMember = Def_M_COMMON.VALUE1;
                this.cboMailStatus.DataSource = ds.Tables[Def_M_COMMON.Name];

                base.InitializeLoadControl();
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
        /// <create>R.Katsuo 2017/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                this.cboMailStatus.Focus();
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
        /// <create>R.Katsuo 2017/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                this.cboMailStatus.SelectedIndex = 0;
                this.dtpDateFrom.Value = DateTime.Today;
                this.dtpDateTo.Value = DateTime.Today;
                this.SheetClear();

                this.fbrFunction.F01Button.Enabled = false;
                this.fbrFunction.F04Button.Enabled = false;
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
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>R.Katsuo 2017/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                if (this.cboMailStatus.SelectedIndex <= -1)
                {
                    // 状態を選択してください。
                    this.ShowMessage("M0100110002");
                    this.cboMailStatus.Focus();
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
        /// 入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>R.Katsuo 2017/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                // 選択行チェック
                if (this.shtMeisai.MaxRows <= 0
                    || this.shtMeisai.ActivePosition.Row <= -1)
                {
                    // 一覧から対象を選択してください。
                    this.ShowMessage("M0100110001");
                    this.shtMeisai.Focus();
                    return false;
                }

                var dr = (this.shtMeisai.DataSource as DataTable).Rows[this.shtMeisai.ActivePosition.Row];
                var mailStatus = UtilData.GetFld(dr, Def_T_MAIL.MAIL_STATUS);
                if (this.EditMode == SystemBase.EditMode.Insert)
                {
                    if (mailStatus == MAIL_STATUS.MI_VALUE1)
                    {
                        // 未送信のため、再送信できません。
                        this.ShowMessage("M0100110006");
                        this.shtMeisai.Focus();
                        return false;
                    }
                }
                else
                {
                    if (mailStatus != MAIL_STATUS.MI_VALUE1)
                    {
                        // 既に処理済みのため、強制終了できません。
                        this.ShowMessage("M0100110007");
                        this.shtMeisai.Focus();
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
        /// 検索処理実行部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>R.Katsuo 2017/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                var cond = new CondM01(this.UserInfo);
                cond.MailStatus = this.cboMailStatus.SelectedValue.ToString();
                cond.DateFrom = this.dtpDateFrom.Value.Date;
                var e = this.dtpDateTo.Value.Date;
                cond.DateTo = new DateTime(e.Year, e.Month, e.Day, 23, 59, 59, 999);

                var conn = new ConnM01();
                var ds = conn.GetMail(cond);
                var dt = ds.Tables[Def_T_MAIL.Name];

                // 対象データがない場合はメッセージ
                if (dt.Rows.Count == 0)
                {
                    // 該当するMail送信情報はありません。
                    this.ShowMessage("M0100110005");
                    this.cboMailStatus.Focus();
                    this.SheetClear();
                    return false;
                }

                this.shtMeisai.Redraw = false;
                this.shtMeisai.DataSource = dt;
                this.shtMeisai.Redraw = true;
                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
            finally
            {
                bool isOK = 0 < this.shtMeisai.MaxRows;
                this.fbrFunction.F01Button.Enabled = isOK;
                this.fbrFunction.F04Button.Enabled = isOK;
                this.shtMeisai.Enabled = isOK;
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
        /// <create>R.Katsuo 2017/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            bool ret = base.RunEdit();
            try
            {
                if (ret)
                {
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

        #region 登録データの取得

        /// --------------------------------------------------
        /// <summary>
        /// 登録データの取得
        /// </summary>
        /// <create>R.Katsuo 2017/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable SetEditData()
        {
            var dtSource = this.shtMeisai.DataSource as DataTable;
            var dt = dtSource.Clone();
            dt.Rows.Add(dtSource.Rows[this.shtMeisai.ActivePosition.Row].ItemArray);
            return dt;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 保存用コンディション取得
        /// </summary>
        /// <returns></returns>
        /// <create>R.Katsuo 2017/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondM01 GetCondition()
        {
            var cond = new CondM01(this.UserInfo);
            cond.CreateUserID = this.UserInfo.UserID;
            cond.UpdateUserID = this.UserInfo.UserID;
            cond.CreateUserName = this.UserInfo.UserName;
            cond.UpdateUserName = this.UserInfo.UserName;
            return cond;
        }

        #endregion

        #region 登録処理

        /// --------------------------------------------------
        /// <summary>
        /// 登録処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>R.Katsuo 2017/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                string errMsgID;
                string[] args;
                var ret = new ConnM01().InsMailRetry(this.GetCondition(), this.SetEditData(), out errMsgID, out args);
                if (!ret)
                {
                    if (ComFunc.IsVersionError(errMsgID))
                    {
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

        #region 更新処理

        /// --------------------------------------------------
        /// <summary>
        /// 更新処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>R.Katsuo 2017/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                string errMsgID;
                string[] args;
                var ret = new ConnM01().UpdMailAbort(this.GetCondition(), this.SetEditData(), out errMsgID, out args);
                if (!ret)
                {
                    if (ComFunc.IsVersionError(errMsgID))
                    {
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

        #region 表示ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 表示ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>R.Katsuo 2017/09/14</create>
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

        #endregion

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F1ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>R.Katsuo 2017/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            //base.fbrFunction_F01Button_Click(sender, e);
            try
            {
                this.ClearMessage();
                this.EditMode = SystemBase.EditMode.Insert;
                this.RunEdit();
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
        /// <create>R.Katsuo 2017/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F04Button_Click(object sender, EventArgs e)
        {
            //base.fbrFunction_F04Button_Click(sender, e);
            try
            {
                this.ClearMessage();
                this.EditMode = SystemBase.EditMode.Update;
                this.RunEdit();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F08ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>R.Katsuo 2017/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                // 画面をクリアします。\r\nよろしいですか？
                if (ShowMessage("A9999999001") != System.Windows.Forms.DialogResult.OK) return;
                this.DisplayClear();
                this.cboMailStatus.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #endregion

        #region シートのクリア

        /// --------------------------------------------------
        /// <summary>
        /// シートのクリア
        /// </summary>
        /// <create>R.Katsuo 2017/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            this.shtMeisai.Redraw = false;
            if (0 < this.shtMeisai.MaxRows)
            {
                this.shtMeisai.TopLeft = new GrapeCity.Win.ElTabelle.Position(0, 0);
            }
            this.shtMeisai.DataSource = null;
            this.shtMeisai.MaxRows = 0;
            this.shtMeisai.Enabled = false;
            this.shtMeisai.Redraw = true;
        }

        #endregion

    }
}
