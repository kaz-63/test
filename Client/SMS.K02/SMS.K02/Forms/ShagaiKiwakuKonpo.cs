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

namespace SMS.K02.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 社外木枠梱包登録
    /// </summary>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ShagaiKiwakuKonpo : KiwakuKonpo
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
        /// <create>Y.Higuchi 2010/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public ShagaiKiwakuKonpo(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>Y.Higuchi 2010/07/30</create>
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
        /// <create>Y.Higuchi 2010/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // ----- メッセージの設定 -----
                // 工事識別NOを入力して下さい。
                this.SetDispMessageID(DispMessageType.CheckInputSearchKojiNo, "A9999999029");
                // 出荷済です。
                this.SetDispMessageID(DispMessageType.Shukkazumi, "A9999999031");
                // 他端末で削除されています。
                this.SetDispMessageID(DispMessageType.CheckEditDataDelete, "A9999999026");
                // 出荷済です。編集出来ません。
                this.SetDispMessageID(DispMessageType.CheckEditShukkazumi, "A9999999032");
                // 存在チェック
                this.SetDispMessageID(DispMessageType.CheckEditNoData, "A9999999030");
                // 梱包完了です。編集できません。
                this.SetDispMessageID(DispMessageType.CheckEditKonpokanryo, "A9999999033");
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

        }

        #endregion
    }
}
