using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Commons;
using DSWUtil;
using SMS.P02.Forms;
using SystemBase.Util;
using WsConnection;
using WsConnection.WebRefM01;

namespace SMS.M01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 通知基本設定
    /// </summary>
    /// <create>T.Sakiori 2017/09/06</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class BasicNotify : SystemBase.Forms.CustomOrderForm
    {
        #region フィールド

        /// --------------------------------------------------
        /// <summary>
        /// 物件追加通知メールヘッダID
        /// </summary>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _mailHeaderId;

        /// --------------------------------------------------
        /// <summary>
        /// 進捗管理通知メールヘッダID
        /// </summary>
        /// <create>Y.Nakasato 2019/07/08 AR進捗通知対応</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _shinchokuMailHeaderId;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// 通知基本設定
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="menuCategoryID">メニュー種別ID</param>
        /// <param name="menuItemID">メニュー項目ID</param>
        /// <param name="title">画面タイトル</param>
        /// <create>T.Sakiori 2017/09/06</create>
        /// <update></update>
        /// --------------------------------------------------
        public BasicNotify(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>T.Sakiori 2017/09/06</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // フォームの設定
                this.IsCloseQuestion = true;
                this.IsRunEditAfterClear = true;
                this.EditMode = SystemBase.EditMode.Insert;
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
        /// <create>T.Sakiori 2017/09/06</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                this.chkBukkenAddEvent.Focus();
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
        /// <create>T.Sakiori 2017/09/06</create>
        /// <update>Y.Nakasato 2019/07/08 AR進捗通知対応</update>
        /// <update>H.Tajimi 2019/08/16 TAG連携通知対応</update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                this.chkBukkenAddEvent.Checked = false;
                this.chkARAddEvent.Checked = false;
                this.chkARUpdateEvent.Checked = false;
                this.chkRenkeiCompEvent.Checked = false;

                this.txtTo.Text = string.Empty;
                this.txtCc.Text = string.Empty;
                this.txtBcc.Text = string.Empty;
                this.txtShinchokuTo.Text = string.Empty;
                this.txtShinchokuCc.Text = string.Empty;
                this.txtShinchokuBcc.Text = string.Empty;

                this.RunSearch();

                this.chkBukkenAddEvent.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 検索処理

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理実行部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update>Y.Nakasato 2019/07/08 AR進捗通知対応</update>
        /// <update>H.Tajimi 2019/08/16 TAG連携通知対応</update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                var conn = new ConnM01();
                var ds = conn.GetBasicNotify(new CondM01(this.UserInfo));

                this.chkBukkenAddEvent.Checked = UtilData.GetFldToBoolean(ds, Def_M_MAIL_SETTING.Name, 0, Def_M_MAIL_SETTING.BUKKEN_ADD_EVENT);
                this.chkARAddEvent.Checked = UtilData.GetFldToBoolean(ds, Def_M_MAIL_SETTING.Name, 0, Def_M_MAIL_SETTING.AR_ADD_EVENT);
                this.chkARUpdateEvent.Checked = UtilData.GetFldToBoolean(ds, Def_M_MAIL_SETTING.Name, 0, Def_M_MAIL_SETTING.AR_UPDATE_EVENT);
                this.chkRenkeiCompEvent.Checked = UtilData.GetFldToBoolean(ds, Def_M_MAIL_SETTING.Name, 0, Def_M_MAIL_SETTING.TAG_RENKEI_EVENT);

                this.txtTo.Text = this.GetMailUser(ds.Tables[Def_M_BUKKEN_MAIL.Name], MAIL_ADDRESS_FLAG.TO_VALUE1, MAIL_KBN.BUKKEN_VALUE1, Def_M_USER.USER_NAME);
                this.txtCc.Text = this.GetMailUser(ds.Tables[Def_M_BUKKEN_MAIL.Name], MAIL_ADDRESS_FLAG.CC_VALUE1, MAIL_KBN.BUKKEN_VALUE1, Def_M_USER.USER_NAME);
                this.txtBcc.Text = this.GetMailUser(ds.Tables[Def_M_BUKKEN_MAIL.Name], MAIL_ADDRESS_FLAG.BCC_VALUE1, MAIL_KBN.BUKKEN_VALUE1, Def_M_USER.USER_NAME);

                this.txtTo.Tag = this.GetUserId(ds.Tables[Def_M_BUKKEN_MAIL.Name], MAIL_ADDRESS_FLAG.TO_VALUE1, MAIL_KBN.BUKKEN_VALUE1);
                this.txtCc.Tag = this.GetUserId(ds.Tables[Def_M_BUKKEN_MAIL.Name], MAIL_ADDRESS_FLAG.CC_VALUE1, MAIL_KBN.BUKKEN_VALUE1);
                this.txtBcc.Tag = this.GetUserId(ds.Tables[Def_M_BUKKEN_MAIL.Name], MAIL_ADDRESS_FLAG.BCC_VALUE1, MAIL_KBN.BUKKEN_VALUE1);

                this._mailHeaderId = this.GetMailHeaderId(ds.Tables[Def_M_BUKKEN_MAIL.Name], MAIL_KBN.BUKKEN_VALUE1);

                this.txtShinchokuTo.Text = this.GetMailUser(ds.Tables[Def_M_BUKKEN_MAIL.Name], MAIL_ADDRESS_FLAG.TO_VALUE1, MAIL_KBN.ARSHINCHOKU_VALUE1, Def_M_USER.USER_NAME);
                this.txtShinchokuCc.Text = this.GetMailUser(ds.Tables[Def_M_BUKKEN_MAIL.Name], MAIL_ADDRESS_FLAG.CC_VALUE1, MAIL_KBN.ARSHINCHOKU_VALUE1, Def_M_USER.USER_NAME);
                this.txtShinchokuBcc.Text = this.GetMailUser(ds.Tables[Def_M_BUKKEN_MAIL.Name], MAIL_ADDRESS_FLAG.BCC_VALUE1, MAIL_KBN.ARSHINCHOKU_VALUE1, Def_M_USER.USER_NAME);

                this.txtShinchokuTo.Tag = this.GetUserId(ds.Tables[Def_M_BUKKEN_MAIL.Name], MAIL_ADDRESS_FLAG.TO_VALUE1, MAIL_KBN.ARSHINCHOKU_VALUE1);
                this.txtShinchokuCc.Tag = this.GetUserId(ds.Tables[Def_M_BUKKEN_MAIL.Name], MAIL_ADDRESS_FLAG.CC_VALUE1, MAIL_KBN.ARSHINCHOKU_VALUE1);
                this.txtShinchokuBcc.Tag = this.GetUserId(ds.Tables[Def_M_BUKKEN_MAIL.Name], MAIL_ADDRESS_FLAG.BCC_VALUE1, MAIL_KBN.ARSHINCHOKU_VALUE1);

                this._shinchokuMailHeaderId = this.GetMailHeaderId(ds.Tables[Def_M_BUKKEN_MAIL.Name], MAIL_KBN.ARSHINCHOKU_VALUE1);

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

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>T.Sakiori 2017/09/22</create>
        /// <update>Y.Nakasato 2019/07/08 AR進捗通知対応</update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                if (string.IsNullOrEmpty(this.txtTo.Text))
                {
                    //物件追加通知の送信先(TO)を選択してください。
                    this.ShowMessage("M0100080002", grpEdit.Text);
                    this.txtTo.Focus();
                    return false;
                }

                if (string.IsNullOrEmpty(this.txtShinchokuTo.Text))
                {
                    //進捗管理通知の送信先(TO)を選択してください。
                    this.ShowMessage("M0100080002", grpEditShinchoku.Text);
                    this.txtShinchokuTo.Focus();
                    return false;
                }

                var conn = new ConnM01();
                var cond = new CondM01(this.UserInfo);
                if (!conn.ExistsMailChangeRole(cond))
                {
                    // MailAddress変更権限を持っていないため設定できません。
                    this.ShowMessage("A9999999059");
                    this.txtTo.Focus();
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

        #region 登録処理

        /// --------------------------------------------------
        /// <summary>
        /// 登録処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update>Y.Nakasato 2019/07/08 AR進捗通知対応</update>
        /// <update>H.Tajimi 2019/08/16 TAG連携通知対応</update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                var ds = new DataSet();

                // メール設定マスタ
                {
                    var dt = new DataTable(Def_M_MAIL_SETTING.Name);
                    dt.Columns.Add(Def_M_MAIL_SETTING.BUKKEN_ADD_EVENT);
                    dt.Columns.Add(Def_M_MAIL_SETTING.AR_ADD_EVENT);
                    dt.Columns.Add(Def_M_MAIL_SETTING.AR_UPDATE_EVENT);
                    dt.Columns.Add(Def_M_MAIL_SETTING.TAG_RENKEI_EVENT);

                    dt.Rows.Add(this.chkBukkenAddEvent.Checked ? BUKKEN_ADD_EVENT.YES_VALUE1 : BUKKEN_ADD_EVENT.NO_VALUE1
                        , this.chkARAddEvent.Checked ? AR_ADD_EVENT.YES_VALUE1 : AR_ADD_EVENT.NO_VALUE1
                        , this.chkARUpdateEvent.Checked ? AR_UPDATE_EVENT.YES_VALUE1 : AR_UPDATE_EVENT.NO_VALUE1
                        , this.chkRenkeiCompEvent.Checked ? TAG_RENKEI_EVENT.YES_VALUE1 : TAG_RENKEI_EVENT.NO_VALUE1);
                    ds.Tables.Add(dt);
                }
                // 物件メールマスタ
                {
                    var dt = new DataTable(Def_M_BUKKEN_MAIL.Name);
                    dt.Columns.Add(Def_M_BUKKEN_MAIL.MAIL_KBN);
                    dt.Columns.Add(Def_M_BUKKEN_MAIL.MAIL_HEADER_ID);

                    dt.Rows.Add(MAIL_KBN.BUKKEN_VALUE1, this._mailHeaderId);
                    dt.Rows.Add(MAIL_KBN.ARSHINCHOKU_VALUE1, this._shinchokuMailHeaderId);
                    ds.Tables.Add(dt);
                }
                // 物件メール明細マスタ
                {
                    var dt = new DataTable(Def_M_BUKKEN_MAIL_MEISAI.Name);
                    dt.Columns.Add(Def_M_BUKKEN_MAIL_MEISAI.MAIL_ADDRESS_FLAG);
                    dt.Columns.Add(Def_M_BUKKEN_MAIL_MEISAI.ORDER_NO);
                    dt.Columns.Add(Def_M_BUKKEN_MAIL_MEISAI.USER_ID);
                    dt.Columns.Add(Def_M_BUKKEN_MAIL_MEISAI.MAIL_HEADER_ID);
                    // サーバー側でMAIL_HEADER_IDを更新するため、連結させるためのキーを保持する(このフィールドはDBへ反映されない)
                    dt.Columns.Add(Def_M_BUKKEN_MAIL.MAIL_KBN);

                    // 物件追加通知設定
                    int idx = 0;
                    (this.txtTo.Tag as List<string>).ForEach(x => dt.Rows.Add(MAIL_ADDRESS_FLAG.TO_VALUE1, idx++, x, string.Empty, MAIL_KBN.BUKKEN_VALUE1));
                    idx = 0;
                    (this.txtCc.Tag as List<string>).ForEach(x => dt.Rows.Add(MAIL_ADDRESS_FLAG.CC_VALUE1, idx++, x, string.Empty, MAIL_KBN.BUKKEN_VALUE1));
                    idx = 0;
                    (this.txtBcc.Tag as List<string>).ForEach(x => dt.Rows.Add(MAIL_ADDRESS_FLAG.BCC_VALUE1, idx++, x, string.Empty, MAIL_KBN.BUKKEN_VALUE1));

                    // 進捗管理通知設定
                    idx = 0;
                    (this.txtShinchokuTo.Tag as List<string>).ForEach(x => dt.Rows.Add(MAIL_ADDRESS_FLAG.TO_VALUE1, idx++, x, string.Empty, MAIL_KBN.ARSHINCHOKU_VALUE1));
                    idx = 0;
                    (this.txtShinchokuCc.Tag as List<string>).ForEach(x => dt.Rows.Add(MAIL_ADDRESS_FLAG.CC_VALUE1, idx++, x, string.Empty, MAIL_KBN.ARSHINCHOKU_VALUE1));
                    idx = 0;
                    (this.txtShinchokuBcc.Tag as List<string>).ForEach(x => dt.Rows.Add(MAIL_ADDRESS_FLAG.BCC_VALUE1, idx++, x, string.Empty, MAIL_KBN.ARSHINCHOKU_VALUE1));
                    ds.Tables.Add(dt);
                }

                var conn = new ConnM01();
                var cond = new CondM01(this.UserInfo);
                string errMsgID;
                string[] args;
                if (!conn.SaveBasicNotify(cond, ds, out errMsgID, out args))
                {
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
        /// F07ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2017/09/14</create>
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
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 参照

        /// --------------------------------------------------
        /// <summary>
        /// TO 参照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnTo_Click(object sender, EventArgs e)
        {
            using (var f = new SenderSetting(this.UserInfo, this.txtTo.Tag, MAIL_ADDRESS_FLAG.TO_NAME))
            {
                if (f.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                this.txtTo.Text = this.GetUserName(f.SendData);
                this.txtTo.Tag = this.GetUserId(f.SendData);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// CC 参照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnCc_Click(object sender, EventArgs e)
        {
            using (var f = new SenderSetting(this.UserInfo, this.txtCc.Tag, MAIL_ADDRESS_FLAG.CC_NAME))
            {
                if (f.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                this.txtCc.Text = this.GetUserName(f.SendData);
                this.txtCc.Tag = this.GetUserId(f.SendData);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// BCC 参照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnBcc_Click(object sender, EventArgs e)
        {
            using (var f = new SenderSetting(this.UserInfo, this.txtBcc.Tag, MAIL_ADDRESS_FLAG.BCC_NAME))
            {
                if (f.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                this.txtBcc.Text = this.GetUserName(f.SendData);
                this.txtBcc.Tag = this.GetUserId(f.SendData);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// TO(進捗管理通知) 参照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnShinchokuTo_Click(object sender, EventArgs e)
        {
            using (var f = new SenderSetting(this.UserInfo, this.txtShinchokuTo.Tag, MAIL_ADDRESS_FLAG.TO_NAME))
            {
                if (f.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                this.txtShinchokuTo.Text = this.GetUserName(f.SendData);
                this.txtShinchokuTo.Tag = this.GetUserId(f.SendData);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// CC(進捗管理通知) 参照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnShinchokuCc_Click(object sender, EventArgs e)
        {
            using (var f = new SenderSetting(this.UserInfo, this.txtShinchokuCc.Tag, MAIL_ADDRESS_FLAG.CC_NAME))
            {
                if (f.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                this.txtShinchokuCc.Text = this.GetUserName(f.SendData);
                this.txtShinchokuCc.Tag = this.GetUserId(f.SendData);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// BCC(進捗管理通知) 参照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnShinchokuBcc_Click(object sender, EventArgs e)
        {
            using (var f = new SenderSetting(this.UserInfo, this.txtShinchokuBcc.Tag, MAIL_ADDRESS_FLAG.BCC_NAME))
            {
                if (f.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                this.txtShinchokuBcc.Text = this.GetUserName(f.SendData);
                this.txtShinchokuBcc.Tag = this.GetUserId(f.SendData);
            }
        }


        #endregion

        #endregion

        #region ユーザー名、ユーザーID取得

        /// --------------------------------------------------
        /// <summary>
        /// 表示用ユーザー名取得
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private string GetUserName(DataTable dt)
        {
            return string.Join(", ", dt.AsEnumerable()
                .Select(x => UtilData.GetFld(x, Def_M_USER.USER_NAME))
                .ToArray());
        }

        /// --------------------------------------------------
        /// <summary>
        /// 内部保持用ユーザーID取得
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private List<string> GetUserId(DataTable dt)
        {
            return dt.AsEnumerable()
              .Select(x => UtilData.GetFld(x, Def_M_BUKKEN_MAIL_MEISAI.USER_ID)).ToList();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 内部保持用ユーザーID取得
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="mailAddressFlag"></param>
        /// <returns></returns>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private List<string> GetUserId(DataTable dt, string mailAddressFlag)
        {
            return dt.AsEnumerable()
                .Where(x => UtilData.GetFld(x, Def_M_BUKKEN_MAIL_MEISAI.MAIL_ADDRESS_FLAG) == mailAddressFlag)
                .Select(x => UtilData.GetFld(x, Def_M_BUKKEN_MAIL_MEISAI.USER_ID)).ToList();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 内部保持用ユーザーID取得(区分指定あり版)
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="mailAddressFlag"></param>
        /// <param name="kbn"></param>
        /// <returns></returns>
        /// <create>Y.Nakasato 2019/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private List<string> GetUserId(DataTable dt, string mailAddressFlag, string kbn)
        {
            return dt.AsEnumerable()
                .Where(x => UtilData.GetFld(x, Def_M_BUKKEN_MAIL_MEISAI.MAIL_ADDRESS_FLAG) == mailAddressFlag)
                .Where(x => x.Field<string>(Def_M_BUKKEN_MAIL.MAIL_KBN) == kbn)
                .Select(x => UtilData.GetFld(x, Def_M_BUKKEN_MAIL_MEISAI.USER_ID)).ToList();
        }

        /// --------------------------------------------------
        /// <summary>
        /// メールヘッダ取得
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="kbn"></param>
        /// <returns></returns>
        /// <create>Y.Nakasato 2019/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private string GetMailHeaderId(DataTable dt, string kbn)
        {
            string res = dt.AsEnumerable()
                .Where(x => x.Field<string>(Def_M_BUKKEN_MAIL.MAIL_KBN) == kbn)
                .Select(x => UtilData.GetFld(x, Def_M_BUKKEN_MAIL_MEISAI.MAIL_HEADER_ID)).FirstOrDefault();

            return (res == null ? "" : res);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メール用の結合したユーザー情報取得(区分指定あり版)
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="mailAddressFlag"></param>
        /// <param name="kbn"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        /// <create>Y.Nakasato 2019/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private string GetMailUser(DataTable dt, string mailAddressFlag, string kbn, string field)
        {
            return string.Join(", ", dt.AsEnumerable()
                .Where(x => x.Field<string>(Def_M_BUKKEN_MAIL_MEISAI.MAIL_ADDRESS_FLAG) == mailAddressFlag)
                .Where(x => x.Field<string>(Def_M_BUKKEN_MAIL.MAIL_KBN) == kbn)
                .Select(x => x.Field<string>(field)).ToArray());
        }

        #endregion

    }
}
