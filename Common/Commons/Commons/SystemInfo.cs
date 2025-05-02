using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commons
{
    /// --------------------------------------------------
    /// <summary>
    /// システム情報管理クラス
    /// </summary>
    /// <create>Y.Higuchi 2010/04/19</create>
    /// <update></update>
    /// --------------------------------------------------
    [System.Xml.Serialization.SoapType()]
    [System.Serializable()]
    public class SystemInfo
    {
        #region Fields

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
        /// ログイン機能有効/無効
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _enabledLogin = null;

        /// --------------------------------------------------
        /// <summary>
        /// ユーザーID最大バイト数
        /// </summary>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private int _maxUserID;

        /// --------------------------------------------------
        /// <summary>
        /// パスワード最大バイト数
        /// </summary>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private int _maxPassword;

        /// --------------------------------------------------
        /// <summary>
        /// パスワード最小バイト数
        /// </summary>
        /// <create>Y.Higuchi 2010/06/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private int _minPassword;

        /// --------------------------------------------------
        /// <summary>
        /// パスワード変更機能
        /// </summary>
        /// <create>Y.Higuchi 2010/06/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _myPasswordChange;

        /// --------------------------------------------------
        /// <summary>
        /// パスワード有効期限有無
        /// </summary>
        /// <create>Y.Higuchi 2010/06/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _passwordExpiration;

        /// --------------------------------------------------
        /// <summary>
        /// パスワード有効期間フラグ
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _expirationFlag;

        /// --------------------------------------------------
        /// <summary>
        /// パスワード有効期限期間
        /// </summary>
        /// <create>Y.Higuchi 2010/06/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private int _expiration;

        /// --------------------------------------------------
        /// <summary>
        /// パスワード期限警告フラグ
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _expirationWarningFlag;

        /// --------------------------------------------------
        /// <summary>
        /// パスワード期限警告期間
        /// </summary>
        /// <create>Y.Higuchi 2010/06/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private int _expirationWarning;

        /// --------------------------------------------------
        /// <summary>
        /// 過去パスワード重複機能
        /// </summary>
        /// <create>Y.Higuchi 2010/06/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _duplicationPastPassword;

        /// --------------------------------------------------
        /// <summary>
        /// パスワードチェックフラグ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _passwordCheck;

        /// --------------------------------------------------
        /// <summary>
        /// 木枠梱包明細登録時に自動で便間移動実施フラグ
        /// </summary>
        /// <create>H.Tajimi 2019/01/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _kiwakuKonpoMoveShipFlag = null;

        /// --------------------------------------------------
        /// <summary>
        /// 範囲の区切り文字
        /// </summary>
        /// <create>D.Okumura 2019/07/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private char _separatorRange;

        /// --------------------------------------------------
        /// <summary>
        /// 項目の区切り文字
        /// </summary>
        /// <create>D.Okumura 2019/07/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private char _separatorItem;

        /// --------------------------------------------------
        /// <summary>
        /// 単価計算利率
        /// </summary>
        /// <create>J.Chen 2021/06/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _calculationRate;

        #endregion

        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public SystemInfo()
        {
        }

        #endregion

        #region 端末ロール有効/無効

        /// --------------------------------------------------
        /// <summary>
        /// 端末ロール有効/無効
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
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
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TerminalGuest
        {
            get { return this._terminalGuest; }
            set { this._terminalGuest = value; }
        }

        #endregion

        #region ログイン機能有効/無効

        /// --------------------------------------------------
        /// <summary>
        /// ログイン機能有効/無効
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public string EnabledLogin
        {
            get { return this._enabledLogin; }
            set { this._enabledLogin = value; }
        }

        #endregion

        #region ユーザーID最大バイト数

        /// --------------------------------------------------
        /// <summary>
        /// ユーザーID最大バイト数
        /// </summary>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public int MaxUserID
        {
            get { return this._maxUserID; }
            set { this._maxUserID = value; }
        }

        #endregion

        #region パスワード最大バイト数

        /// --------------------------------------------------
        /// <summary>
        /// パスワード最大バイト数
        /// </summary>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public int MaxPassword
        {
            get { return this._maxPassword; }
            set { this._maxPassword = value; }
        }

        #endregion

        #region パスワード最小バイト数

        /// --------------------------------------------------
        /// <summary>
        /// パスワード最小バイト数
        /// </summary>
        /// <create>Y.Higuchi 2010/06/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public int MinPassword
        {
            get { return this._minPassword; }
            set { this._minPassword = value; }
        }

        #endregion

        #region パスワード変更機能

        /// --------------------------------------------------
        /// <summary>
        /// パスワード変更機能
        /// </summary>
        /// <create>Y.Higuchi 2010/06/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public string MyPasswordChange
        {
            get { return this._myPasswordChange; }
            set { this._myPasswordChange = value; }
        }

        #endregion

        #region パスワード有効期限有無

        /// --------------------------------------------------
        /// <summary>
        /// パスワード有効期限有無
        /// </summary>
        /// <create>Y.Higuchi 2010/06/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public string PasswordExpiration
        {
            get { return this._passwordExpiration; }
            set { this._passwordExpiration = value; }
        }

        #endregion

        #region パスワード有効期限フラグ

        /// --------------------------------------------------
        /// <summary>
        /// パスワード有効期限フラグ
        /// </summary>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ExpirationFlag
        {
            get { return this._expirationFlag; }
            set { this._expirationFlag = value; }
        }

        #endregion

        #region パスワード有効期限期間

        /// --------------------------------------------------
        /// <summary>
        /// パスワード有効期限期間
        /// </summary>
        /// <create>Y.Higuchi 2010/06/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public int Expiration
        {
            get { return this._expiration; }
            set { this._expiration = value; }
        }

        #endregion

        #region パスワード期限警告フラグ

        /// --------------------------------------------------
        /// <summary>
        /// パスワード期限警告フラグ
        /// </summary>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ExpirationWarningFlag
        {
            get { return this._expirationWarningFlag; }
            set { this._expirationWarningFlag = value; }
        }

        #endregion

        #region パスワード期限警告期間

        /// --------------------------------------------------
        /// <summary>
        /// パスワード期限警告期間
        /// </summary>
        /// <create>Y.Higuchi 2010/06/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public int ExpirationWarning
        {
            get { return this._expirationWarning; }
            set { this._expirationWarning = value; }
        }

        #endregion

        #region 過去パスワード重複機能

        /// --------------------------------------------------
        /// <summary>
        /// 過去パスワード重複機能
        /// </summary>
        /// <create>Y.Higuchi 2010/06/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public string DuplicationPastPassword
        {
            get { return this._duplicationPastPassword; }
            set { this._duplicationPastPassword = value; }
        }

        #endregion

        #region パスワードチェックフラグ

        /// --------------------------------------------------
        /// <summary>
        /// パスワードチェックフラグ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public string PasswordCheck
        {
            get { return this._passwordCheck; }
            set { this._passwordCheck = value; }
        }

        #endregion

        #region 製番追加分

        #region 木枠梱包明細登録時に自動で便間移動実施フラグ

        /// --------------------------------------------------
        /// <summary>
        /// 木枠梱包明細登録時に自動で便間移動実施フラグ
        /// </summary>
        /// <create>H.Tajimi 2019/01/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public string KiwakuKonpoMoveShipFlag
        {
            get { return this._kiwakuKonpoMoveShipFlag; }
            set { this._kiwakuKonpoMoveShipFlag = value; }
        }

        #endregion

        #region 区切り文字設定

        /// --------------------------------------------------
        /// <summary>
        /// 範囲の区切り文字
        /// </summary>
        /// <create>D.Okumura 2019/07/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public char SeparatorRange
        {
            get { return this._separatorRange; }
            set { this._separatorRange = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 項目の区切り文字
        /// </summary>
        /// <create>D.Okumura 2019/07/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public char SeparatorItem
        {
            get { return this._separatorItem; }
            set { this._separatorItem = value; }
        }

        #endregion

        #endregion

        #region Assy単価計算

        #region 単価計算利率

        /// --------------------------------------------------
        /// <summary>
        /// 単価計算利率
        /// </summary>
        /// <create>J.Chen 2022/06/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public string calculationRate
        {
            get { return this._calculationRate; }
            set { this._calculationRate = value; }
        }

        #endregion

        #endregion
    }
}
