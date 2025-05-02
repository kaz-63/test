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

using SystemBase;
using SMS.U01.Forms;


namespace WinApp
{
    public partial class Form1 : SystemBase.Forms.BaseLogin
    {
        #region コンストラクタ
        public Form1()
            : base(new UserInfo(), "起動用フォーム")
        {
            InitializeComponent();
        }
        #endregion

        #region 初期化

        protected override void InitializeLoadControl()
        {

            base.InitializeLoadControl();
            this.InitializeExecuteClass();
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

        #endregion

        #region ログイン処理

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

            return base.ExecuteLogin(userID, password,ref errorMsgID);
        }

        #endregion

        #region Control Events

        protected override void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.CheckInput()) return;
                string errorMsgID = string.Empty;
                if (this.ExecuteLogin(this.txtUserID.Text, this.txtPassword.Text, ref errorMsgID))
                {
                    DataTable dt = (DataTable)this.cboClass.DataSource;
                    int index = this.cboClass.SelectedIndex;
                    TypeLoader loader = new TypeLoader();
                    string className = ComFunc.GetFld(dt, index, "ClassName");
                    string categoryID = ComFunc.GetFld(dt, index, "CategoryID");
                    string itemID = ComFunc.GetFld(dt, index, "ItemID");
                    string title = ComFunc.GetFld(dt, index, "Title");

                    Type type = loader.GetClassType(className, "SMS.*.dll");
                    object obj = type.InvokeMember(null,
                                                   System.Reflection.BindingFlags.CreateInstance,
                                                   null,
                                                   null,
                                                   new object[] { this.UserInfo, categoryID, itemID, title });
                    // BaseFormにキャスト
                    SystemBase.Forms.BaseForm frm = obj as SystemBase.Forms.BaseForm;
                    // キャスト出来なければ処理を抜ける
                    if (frm == null) return;

                    this.Hide();
                    frm.ShowDialog();
                    this.Show();
                    this.btnClose.Focus();
                    frm.Dispose();
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

        #endregion

        #region プロジェクト単位で変更する箇所

        private void InitializeExecuteClass()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("ClassName", typeof(string));
            dt.Columns.Add("CategoryID", typeof(string));
            dt.Columns.Add("ItemID", typeof(string));

            string categoryID = "U01";
            string nameSpace = "SMS." + categoryID + ".Forms.";
            // U0100010 : 受入情報登録
            {
                DataRow dr = dt.NewRow();
                dr["Title"] = "受入情報登録";
                dr["ClassName"] = nameSpace + "UkeireSettei";
                dr["CategoryID"] = categoryID;
                dr["ItemID"] = "U0100010";
                dt.Rows.Add(dr);
            }
            // U0100020 : 受入情報明細
            {
                DataRow dr = dt.NewRow();
                dr["Title"] = "受入情報明細";
                dr["ClassName"] = nameSpace + "ShinchokuToiawaseMeisai";
                dr["CategoryID"] = categoryID;
                dr["ItemID"] = "U0100020";
                dt.Rows.Add(dr);
            }

            // データソースの設定
            this.cboClass.DisplayMember = "Title";
            this.cboClass.ValueMember = "ClassName";
            this.cboClass.DataSource = dt;
        }

        #endregion

    }
}
