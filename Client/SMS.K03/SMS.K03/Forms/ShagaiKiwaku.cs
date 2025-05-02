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

namespace SMS.K03.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 社外木枠梱包明細登録
    /// </summary>
    /// <create>M.Tsutsumi 2010/08/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ShagaiKiwaku : Kiwaku
    {

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="menuCategoryID">メニュー種別ID</param>
        /// <param name="menuItemID">メニュー項目ID</param>
        /// <param name="title">画面タイトル</param>
        /// <create>M.Tsutsumi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public ShagaiKiwaku(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
            : base(userInfo, menuCategoryID, menuItemID, title)
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// --------------------------------------------------
        /// <summary>
        /// 登録区分
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override string TorokuFlag
        {
            get
            {
                return TOROKU_FLAG.GAI_VALUE1;
            }
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // ここにコントロールの初期化を記述する。
                // ベースでDisplayClearの呼出しは行われています。

                // 登録処理のメッセージを変更する場合はセット
                // 登録確認メッセージ
                //this.MsgInsertConfirm = "";
                // 登録完了メッセージ
                //this.MsgInsertEnd = "";

                // 修正処理のメッセージを変更する場合はセット
                // 修正確認メッセージ
                //this.MsgUpdateConfirm = "";
                // 修正完了メッセージ
                //this.MsgUpdateEnd = "";

                // 削除処理のメッセージを変更する場合はセット
                // 削除確認メッセージ
                //this.MsgDeleteConfirm = "";
                // 削除完了メッセージ
                //this.MsgDeleteEnd = "";
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

        }

        #endregion

    }
}
