using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Commons;
using WsConnection;
using DSWUtil;

using SMS.P02.Forms;

namespace WinApp
{
    public partial class Form1 : SystemBase.Forms.BaseLogin
    {
        public Form1()
            : base(new UserInfo(), "起動用フォーム")
        {
            InitializeComponent();
        }

        protected override void InitializeLoadControl()
        {

            base.InitializeLoadControl();
            this.txtUserID.Text = "system";
            this.txtUserID.MaxLength = 10;
            this.txtPassword.Text = "sysadmin5";
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
                    if (this.rdoP0200010.Checked)
                    {
                        this.ShowP0200010();
                    }
                    else if (this.rdoP0200030.Checked)
                    {
                        this.ShowP0200030();
                    }
                    else if (this.rdoP0200040.Checked)
                    {
                        this.ShowP0200040();
                    }
                    else if (this.rdoP0200050.Checked)
                    {
                        this.ShowP0200050();
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

        private void ShowP0200010()
        {
            using (SystemBase.Forms.CustomOrderSearchDialog frm = new NonyusakiIchiran(this.UserInfo, "0", "本体納入先", "S"))
            {
                this.Hide();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    DataRow dr = frm.SelectedRowData;
                    this.SetMessage(SystemBase.Controls.MessageImageType.None, dr[Def_M_NONYUSAKI.NONYUSAKI_CD].ToString() + ":" + dr[Def_M_NONYUSAKI.NONYUSAKI_NAME].ToString() + ":" + dr[Def_M_NONYUSAKI.SHIP].ToString());
                }

                this.Show();
                this.btnClose.Focus();
            }
        }

        private void ShowP0200030()
        {
            using (SystemBase.Forms.CustomOrderSearchDialog frm = new KojiShikibetsuIchiran(this.UserInfo, "1", "社内", "KJ"))
            {
                this.Hide();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    DataRow dr = frm.SelectedRowData;
                    this.SetMessage(SystemBase.Controls.MessageImageType.None, dr[Def_T_KIWAKU.KOJI_NO].ToString() + ":"+dr[Def_T_KIWAKU.KOJI_NAME].ToString() +":" +dr[Def_T_KIWAKU.SHIP].ToString());
                }
                this.Show();
                this.btnClose.Focus();
            }
        }

        private void ShowP0200040()
        {
            using (SystemBase.Forms.CustomOrderSearchDialog frm = new BukkenMeiIchiran(this.UserInfo, "0", string.Empty))
            {
                this.Hide();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    DataRow dr = frm.SelectedRowData;
                    this.SetMessage(SystemBase.Controls.MessageImageType.None, dr[Def_M_BUKKEN.BUKKEN_NAME].ToString() + ":" + dr[Def_M_BUKKEN.BUKKEN_NO].ToString());
                }
                this.Show();
                this.btnClose.Focus();
            }
        }

        private void ShowP0200050()
        {
            //using (var frm = new RirekiShokai(this.UserInfo, string.Empty, string.Empty, ComDefine.TITLE_P0200050))
            using (var frm = new RirekiShokai(this.UserInfo, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty))
            {
                this.Hide();
                frm.ShowDialog();
                this.Show();
                this.btnClose.Focus();
            }
        }
    }
}
