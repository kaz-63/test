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
using SystemBase.Properties;
using System.Reflection;

namespace SystemBase.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// バージョン情報
    /// </summary>
    /// <create>J.Chen 2024/09/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class VersionJoho : CustomOrderForm
    {
        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <create>J.Chen 2024/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public VersionJoho(UserInfo userInfo, string version)
            : base(userInfo, Resources.VersionJoho_Title)
        {
            InitializeComponent();
            this.lblVersion.Text = version;

            Assembly assembly = Assembly.Load("SMS");
            AssemblyCopyrightAttribute copyright = (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyCopyrightAttribute));

            this.lblCopyright.Text = copyright.Copyright;
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>J.Chen 2024/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

        }

        #endregion

        #region イベント

        #region 閉じるボタン

        /// --------------------------------------------------
        /// <summary>
        ///閉じるボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #endregion

    }
}
