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
    /// 共通通知設定
    /// </summary>
    /// <create>T.Sakiori 2017/09/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class CommonNotify : SystemBase.Forms.CustomOrderForm
    {
        #region フィールド

        /// --------------------------------------------------
        /// <summary>
        /// 物件管理No
        /// </summary>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _bukkenNo;
        /// --------------------------------------------------
        /// <summary>
        /// 物件名称
        /// </summary>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _bukkenName;
        /// --------------------------------------------------
        /// <summary>
        /// メールヘッダID
        /// </summary>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _mailHeaderId;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// 共通通知設定
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="menuCategoryID">メニュー種別ID</param>
        /// <param name="menuItemID">メニュー項目ID</param>
        /// <param name="title">画面タイトル</param>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public CommonNotify(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
            : base(userInfo, menuCategoryID, menuItemID, title)
        {
            InitializeComponent();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 共通通知設定
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="bukkenNo">物件管理No</param>
        /// <param name="bukkenName">物件名称</param>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public CommonNotify(UserInfo userInfo, string bukkenNo, string bukkenName)
            : base(userInfo)
        {
            InitializeComponent();

            // 画面タイトル設定
            this.Title = ComDefine.TITLE_M0100090;

            this._bukkenNo = bukkenNo;
            this._bukkenName = bukkenName;
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>T.Sakiori 2017/09/14</create>
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
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                this.txtBukkenNo.Focus();
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
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                this.txtBukkenNo.Text = this._bukkenNo;
                this.txtBukkenName.Text = this._bukkenName;

                this.txtTo.Text = string.Empty;
                this.txtCc.Text = string.Empty;
                this.txtBcc.Text = string.Empty;

                this.RunSearch();
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
        /// <create>K.Saeki 2012/03/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                var conn = new ConnM01();
                var cond = new CondM01(this.UserInfo);
                cond.BukkenNo = this._bukkenNo;
                var dt = conn.GetCommonNotify(cond);

                this.txtTo.Text = ComFunc.GetMailUser(dt, MAIL_ADDRESS_FLAG.TO_VALUE1, Def_M_USER.USER_NAME);
                this.txtCc.Text = ComFunc.GetMailUser(dt, MAIL_ADDRESS_FLAG.CC_VALUE1, Def_M_USER.USER_NAME);
                this.txtBcc.Text = ComFunc.GetMailUser(dt, MAIL_ADDRESS_FLAG.BCC_VALUE1, Def_M_USER.USER_NAME);

                this.txtTo.Tag = this.GetUserId(dt, MAIL_ADDRESS_FLAG.TO_VALUE1);
                this.txtCc.Tag = this.GetUserId(dt, MAIL_ADDRESS_FLAG.CC_VALUE1);
                this.txtBcc.Tag = this.GetUserId(dt, MAIL_ADDRESS_FLAG.BCC_VALUE1);

                this._mailHeaderId = UtilData.GetFld(dt, 0, Def_M_BUKKEN_MAIL.MAIL_HEADER_ID);

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
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                if (string.IsNullOrEmpty(this.txtTo.Text))
                {
                    //送信先(TO)を選択してください。
                    this.ShowMessage("A9999999058");
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
        /// <create>K.Saeki 2012/03/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                var ds = new DataSet();

                // 物件メールマスタ
                {
                    var dt = new DataTable(Def_M_BUKKEN_MAIL.Name);
                    dt.Columns.Add(Def_M_BUKKEN_MAIL.SHUKKA_FLAG);
                    dt.Columns.Add(Def_M_BUKKEN_MAIL.BUKKEN_NO);
                    dt.Columns.Add(Def_M_BUKKEN_MAIL.MAIL_KBN);
                    dt.Columns.Add(Def_M_BUKKEN_MAIL.LIST_FLAG);
                    dt.Columns.Add(Def_M_BUKKEN_MAIL.MAIL_HEADER_ID);

                    dt.Rows.Add(SHUKKA_FLAG.AR_VALUE1
                        , this._bukkenNo
                        , MAIL_KBN.COMMON_VALUE1
                        , DBNull.Value
                        , this._mailHeaderId);
                    ds.Tables.Add(dt);
                }
                // 物件メール明細マスタ
                {
                    var dt = new DataTable(Def_M_BUKKEN_MAIL_MEISAI.Name);
                    dt.Columns.Add(Def_M_BUKKEN_MAIL_MEISAI.MAIL_ADDRESS_FLAG);
                    dt.Columns.Add(Def_M_BUKKEN_MAIL_MEISAI.ORDER_NO);
                    dt.Columns.Add(Def_M_BUKKEN_MAIL_MEISAI.USER_ID);
                    dt.Columns.Add(Def_M_BUKKEN_MAIL_MEISAI.MAIL_HEADER_ID);

                    int idx = 0;
                    (this.txtTo.Tag as List<string>).ForEach(x => dt.Rows.Add(MAIL_ADDRESS_FLAG.TO_VALUE1, idx++, x, string.Empty));
                    idx = 0;
                    (this.txtCc.Tag as List<string>).ForEach(x => dt.Rows.Add(MAIL_ADDRESS_FLAG.CC_VALUE1, idx++, x, string.Empty));
                    idx = 0;
                    (this.txtBcc.Tag as List<string>).ForEach(x => dt.Rows.Add(MAIL_ADDRESS_FLAG.BCC_VALUE1, idx++, x, string.Empty));
                    ds.Tables.Add(dt);
                }

                var conn = new ConnM01();
                var cond = new CondM01(this.UserInfo);
                string errMsgID;
                string[] args;
                if (!conn.SaveCommonNotify(cond, ds, out errMsgID, out args))
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

        #endregion
    }
}
