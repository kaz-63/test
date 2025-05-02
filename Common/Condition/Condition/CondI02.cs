using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;

namespace Condition
{
    /// --------------------------------------------------
    /// <summary>
    /// I02用コンディション
    /// </summary>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public class CondI02 : CondBase
    {
        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 一時取込ID
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _tempID = null;
        /// --------------------------------------------------
        /// <summary>
        /// 作業者
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _workUserID = null;
        /// --------------------------------------------------
        /// <summary>
        /// 取込区分
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _torikomiFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// 状態区分
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _statusFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// 取込日時
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private DateTime _torikomiDate = new DateTime(0);
        /// --------------------------------------------------
        /// <summary>
        /// エラー数
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private int _errorNum = 0;
        /// --------------------------------------------------
        /// <summary>
        /// 状態区分
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _result = null;
        /// --------------------------------------------------
        /// <summary>
        /// 棚卸年月日
        /// </summary>
        /// <create>T.Wakamatsu 2013/09/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _inventDate = null;

        /// --------------------------------------------------
        /// <summary>
        /// メッセージID
        /// </summary>
        /// <create>K.Tsutsumi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _messageID = null;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>K.Tsutsumi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondI02()
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
        public CondI02(LoginInfo target)
            : base(target)
        {
        }

        #endregion

        #region 一時取込ID

        /// --------------------------------------------------
        /// <summary>
        /// 一時取込ID
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TempID
        {
            get { return this._tempID; }
            set { this._tempID = value; }
        }

        #endregion

        #region 作業者

        /// --------------------------------------------------
        /// <summary>
        /// 作業者
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public string WorkUserID
        {
            get { return this._workUserID; }
            set { this._workUserID = value; }
        }

        #endregion

        #region 取込区分

        /// --------------------------------------------------
        /// <summary>
        /// 取込区分
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TorikomiFlag
        {
            get { return this._torikomiFlag; }
            set { this._torikomiFlag = value; }
        }

        #endregion

        #region 状態区分

        /// --------------------------------------------------
        /// <summary>
        /// 状態区分
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public string StatusFlag
        {
            get { return this._statusFlag; }
            set { this._statusFlag = value; }
        }

        #endregion

        #region 取込日時

        /// --------------------------------------------------
        /// <summary>
        /// 取込日時
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public DateTime TorikomiDate
        {
            get { return this._torikomiDate; }
            set { this._torikomiDate = value; }
        }

        #endregion

        #region エラー数

        /// --------------------------------------------------
        /// <summary>
        /// エラー数
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public int ErrorNum
        {
            get { return this._errorNum; }
            set { this._errorNum = value; }
        }

        #endregion

        #region 状態区分

        /// --------------------------------------------------
        /// <summary>
        /// 状態区分
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Result
        {
            get { return this._result; }
            set { this._result = value; }
        }

        #endregion

        #region 棚卸年月日

        /// --------------------------------------------------
        /// <summary>
        /// 作業者
        /// </summary>
        /// <create>T.Wakamatsu 2013/09/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public string InventDate
        {
            get { return this._inventDate; }
            set { this._inventDate = value; }
        }

        #endregion

        #region メッセージID

        /// --------------------------------------------------
        /// <summary>
        /// メッセージID
        /// </summary>
        /// <create>K.Tsutsumi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public string MessageID
        {
            get { return this._messageID; }
            set { this._messageID = value; }
        }

        #endregion

    }
}
