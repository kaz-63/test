using System;
using System.Collections.Generic;
using System.Text;
using Commons;

namespace Condition
{
    /// --------------------------------------------------
    /// <summary>
    /// 共通処理用コンディション
    /// </summary>
    /// <create>Y.Higuchi 2010/04/19</create>
    /// <update></update>
    /// --------------------------------------------------
    [System.Xml.Serialization.SoapType()]
    [System.Serializable()]
    public class CondCommon : CondBase
    {
        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// ユーザーID
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _userID = null;

        /// --------------------------------------------------
        /// <summary>
        /// パスワード
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _password = null;

        /// --------------------------------------------------
        /// <summary>
        /// PC名
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _pcName = null;

        /// --------------------------------------------------
        /// <summary>
        /// 端末ロール有効/無効
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _terminalRole = null;

        /// --------------------------------------------------
        /// <summary>
        /// ゲスト端末有効/無効
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _terminalGuest = null;

        /// --------------------------------------------------
        /// <summary>
        /// メッセージID
        /// </summary>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _messageID = null;

        /// --------------------------------------------------
        /// <summary>
        /// メッセージID配列
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private string[] _arrayMessageID = null;

        /// --------------------------------------------------
        /// <summary>
        /// メニュー区分
        /// </summary>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _menuItemFlag = null;

        /// --------------------------------------------------
        /// <summary>
        /// グループコード
        /// </summary>
        /// <create>Y.Higuchi 2010/04/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _groupCD = null;

        /// --------------------------------------------------
        /// <summary>
        /// 項目コード
        /// </summary>
        /// <create>Y.Higuchi 2010/04/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _itemCD = null;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>K.Tsutsumi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondCommon()
            : this(new LoginInfo())
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target">ログインユーザー情報</param>
        /// <create>K.Tsutsumi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public CondCommon(LoginInfo target)
            : base(target)
        {
        }
        #endregion

        #region ユーザーID

        /// --------------------------------------------------
        /// <summary>
        /// ユーザーID
        /// </summary>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public string UserID 
        {
            get { return this._userID; }
            set { this._userID = value; }
        }

        #endregion

        #region パスワード

        /// --------------------------------------------------
        /// <summary>
        /// パスワード
        /// </summary>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Password 
        {
            get { return this._password; }
            set { this._password = value; }
        }

        #endregion

        #region PC名

        /// --------------------------------------------------
        /// <summary>
        /// PC名
        /// </summary>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public string PcName 
        {
            get { return this._pcName; }
            set { this._pcName = value; }
        }

        #endregion

        #region 端末ロール有効/無効

        /// --------------------------------------------------
        /// <summary>
        /// 端末ロール有効/無効
        /// </summary>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TerminalRole
        {
            get { return this._terminalRole; }
            set { this._terminalRole = value; }
        }

        #endregion

        #region ゲスト端末有効/無効

        /// --------------------------------------------------
        /// <summary>
        /// ゲスト端末有効/無効
        /// </summary>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TerminalGuest
        {
            get { return this._terminalGuest; }
            set { this._terminalGuest = value; }
        }

        #endregion
       
        #region メニュー区分

        /// --------------------------------------------------
        /// <summary>
        /// メニュー区分
        /// </summary>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public string MenuItemFlag
        {
            get { return this._menuItemFlag; }
            set { this._menuItemFlag = value; }
        }

        #endregion

        #region メッセージID

        /// --------------------------------------------------
        /// <summary>
        /// メッセージID
        /// </summary>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public string MessageID
        {
            get { return this._messageID; }
            set { this._messageID = value; }
        }

        #endregion

        #region メッセージID配列

        /// --------------------------------------------------
        /// <summary>
        /// メッセージID配列
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public string[] ArrayMessageID
        {
            get { return this._arrayMessageID; }
            set { this._arrayMessageID = value; }
        }

        #endregion

        #region グループコード

        /// --------------------------------------------------
        /// <summary>
        /// グループコード
        /// </summary>
        /// <create>Y.Higuchi 2010/04/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public string GroupCD
        {
            get { return this._groupCD; }
            set { this._groupCD = value; }
        }

        #endregion

        #region 項目コード

        /// --------------------------------------------------
        /// <summary>
        /// 項目コード
        /// </summary>
        /// <create>Y.Higuchi 2010/04/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ItemCD
        {
            get { return this._itemCD; }
            set { this._itemCD = value; }
        }

        #endregion

        #region 物件管理No

        /// --------------------------------------------------
        /// <summary>
        /// 物件管理No
        /// </summary>
        /// <create>T.Sakiori 2017/09/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public string BukkenNo { get; set; }

        #endregion

        #region 荷受CD

        /// --------------------------------------------------
        /// <summary>
        /// 荷受CD
        /// </summary>
        /// <create>Y.Gwon 2023/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ConsignCD { get; set; }

        #endregion

        #region メール区分

        /// --------------------------------------------------
        /// <summary>
        /// メール区分
        /// </summary>
        /// <create>T.Sakiori 2017/09/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public string MailKbn { get; set; }

        #endregion

        #region リスト区分

        /// --------------------------------------------------
        /// <summary>
        /// リスト区分
        /// </summary>
        /// <create>T.Sakiori 2017/09/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ListFlag { get; set; }

        #endregion
    }
}
