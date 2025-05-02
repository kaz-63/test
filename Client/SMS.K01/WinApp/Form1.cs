using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Commons;
using WsConnection;
using DSWUtil;

using SMS.K01.Forms;

namespace WinApp
{
    public partial class Form1 : SystemBase.Forms.BaseLogin
    {
        private string _categoryID ;
        private string _itemID;
        private string _title;

        public Form1()
            : base(new UserInfo(),"起動用フォーム")
        {
            InitializeComponent();
        }

        protected override void InitializeLoadControl()
        {

            base.InitializeLoadControl();
            _categoryID = "K01";
            _itemID = "K0100010";
            _title = "TAG 発行・照会";
            this.txtUserID.Text = "system";
            this.txtUserID.MaxLength = 10;
            this.txtPassword.Text = "sysadmin2";
            this.txtPassword.MaxLength = 10;
        }

        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();
            this.btnLogin.Focus();



        }

        protected override bool ExecuteLogin(string userID, string password, ref string errorMsgID)
        {
            // システムパラメーター取得
            ConnCommon conn = new ConnCommon();
            DataSet ds = conn.GetSystemParameter();

            if (ds != null && ds.Tables.Contains(Def_M_SYSTEM_PARAMETER.Name) && 0 < ds.Tables[Def_M_SYSTEM_PARAMETER.Name].Rows.Count)
            {
            }
            else
            {
                // サーバー接続失敗
                MessageBox.Show("失敗");
                return false;
            }
            // 初期化処理中

            // ログインユーザー情報にPC情報を設定する。
            this.UserInfo.PcName = UtilSystem.GetUserInfo(false).MachineName;
            this.UserInfo.IPAddress = UtilNet.GetHostIPAddressString();

            // システムパラメーター設定
            DataTable dt = ds.Tables[Def_M_SYSTEM_PARAMETER.Name];
            this.UserInfo.SysInfo.EnabledLogin = ComFunc.GetFld(dt, 0, Def_M_SYSTEM_PARAMETER.ENABLED_LOGIN);
            this.UserInfo.SysInfo.TerminalRole = ComFunc.GetFld(dt, 0, Def_M_SYSTEM_PARAMETER.TERMINAL_ROLE);
            this.UserInfo.SysInfo.TerminalGuest = ComFunc.GetFld(dt, 0, Def_M_SYSTEM_PARAMETER.TERMINAL_GUEST);
            this.UserInfo.SysInfo.MaxUserID = UtilConvert.ToInt32(ComFunc.GetFld(dt, 0, Def_M_SYSTEM_PARAMETER.MAX_USER_ID));
            this.UserInfo.SysInfo.MaxPassword = UtilConvert.ToInt32(ComFunc.GetFld(dt, 0, Def_M_SYSTEM_PARAMETER.MAX_PASSWORD));
            // ログイン機能無効時のデフォルト設定取得
            if (this.UserInfo.SysInfo.EnabledLogin == ENABLED_LOGIN.DISABLE_VALUE1)
            {
                this.UserInfo.UserID = ComFunc.GetFld(dt, 0, Def_M_SYSTEM_PARAMETER.DEFAULT_USER_ID);
                this.UserInfo.UserName = ComFunc.GetFld(dt, 0, Def_M_USER.USER_NAME);
                this.UserInfo.RoleID = ComFunc.GetFld(dt, 0, Def_M_ROLE.ROLE_ID);
                this.UserInfo.RoleName = ComFunc.GetFld(dt, 0, Def_M_ROLE.ROLE_NAME);
            }

            // ユーザー情報設定
            if (0 < dt.Rows.Count)
            {
                this.UserInfo.UserID = ComFunc.GetFld(dt, 0, Def_M_USER.USER_ID);
                this.UserInfo.UserName = ComFunc.GetFld(dt, 0, Def_M_USER.USER_NAME);
                this.UserInfo.RoleID = ComFunc.GetFld(dt, 0, Def_M_ROLE.ROLE_ID);
                this.UserInfo.RoleName = ComFunc.GetFld(dt, 0, Def_M_ROLE.ROLE_NAME);
            }

            return base.ExecuteLogin(userID, password, ref errorMsgID);
        }
        protected override void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.CheckInput()) return;
                string errorMsgID = string.Empty;
                if (this.ExecuteLogin(this.txtUserID.Text, this.txtPassword.Text, ref errorMsgID))
                {
                    using (SystemBase.Forms.CustomOrderForm frm = new ShukaKaishi(this.UserInfo, _categoryID, _itemID, _title))
                    {
                        this.Hide();
                        frm.ShowDialog();
                        this.Show();
                        this.btnClose.Focus();
                    }
                }
                else
                {
                    this.ShowMessage(errorMsgID);
                }
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

        }
    }
}
