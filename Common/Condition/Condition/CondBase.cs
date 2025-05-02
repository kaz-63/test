using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;

namespace Condition
{
    /// --------------------------------------------------
    /// <summary>
    /// コンディションベースクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/06/28</create>
    /// <update></update>
    /// --------------------------------------------------
    [System.Xml.Serialization.SoapType()]
    [System.Serializable()]
    public class CondBase : ICloneable
    {

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>K.Tsutsumi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondBase()
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
        public CondBase(LoginInfo target)
        {
            this.SetLoginfoFromUserInfo(target);
        }

        #endregion

        #region Fields

        // ログイン情報
        private LoginInfo _loginInfo = new LoginInfo();
        //登録日時
        private object _createDate = null;
        //登録ユーザーID
        private string _createUserID = null;
        //登録ユーザー名
        private string _createUserName = null;
        //更新日時
        private object _updateDate = null;
        //更新ユーザーID
        private string _updateUserID = null;
        //更新ユーザー名
        private string _updateUserName = null;
        //保守日時
        private object _mainteDate = null;
        //保守ユーザーID
        private string _mainteUserID = null;
        //保守ユーザー名
        private string _mainteUserName = null;
        //バージョン
        private string _version = null;
        #endregion

        #region ログイン情報

        /// --------------------------------------------------
        /// <summary>
        /// ログイン情報
        /// </summary>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public LoginInfo LoginInfo
        {
            get { return this._loginInfo; }
            set { this._loginInfo = value; }
        }

        #endregion

        #region 登録日時

        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public object CreateDate
        {
            get
            {
                return _createDate;
            }
            set
            {
                _createDate = value;
            }
        }

        #endregion

        #region 登録ユーザーID

        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string CreateUserID
        {
            get
            {
                return _createUserID;
            }
            set
            {
                _createUserID = value;
            }
        }

        #endregion

        #region 登録ユーザー名

        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string CreateUserName
        {
            get
            {
                return _createUserName;
            }
            set
            {
                _createUserName = value;
            }
        }

        #endregion

        #region 更新日時

        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public object UpdateDate
        {
            get
            {
                return _updateDate;
            }
            set
            {
                _updateDate = value;
            }
        }

        #endregion

        #region 更新ユーザーID

        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string UpdateUserID
        {
            get
            {
                return _updateUserID;
            }
            set
            {
                _updateUserID = value;
            }
        }

        #endregion

        #region 更新ユーザー名

        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string UpdateUserName
        {
            get
            {
                return _updateUserName;
            }
            set
            {
                _updateUserName = value;
            }
        }

        #endregion

        #region 保守日時

        /// --------------------------------------------------
        /// <summary>
        /// 保守日時
        /// </summary>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public object MainteDate
        {
            get
            {
                return _mainteDate;
            }
            set
            {
                _mainteDate = value;
            }
        }

        #endregion

        #region 保守ユーザーID

        /// --------------------------------------------------
        /// <summary>
        /// 保守ユーザーID
        /// </summary>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string MainteUserID
        {
            get
            {
                return _mainteUserID;
            }
            set
            {
                _mainteUserID = value;
            }
        }

        #endregion

        #region 保守ユーザー名

        /// --------------------------------------------------
        /// <summary>
        /// 保守ユーザー名
        /// </summary>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string MainteUserName
        {
            get
            {
                return _mainteUserName;
            }
            set
            {
                _mainteUserName = value;
            }
        }

        #endregion

        #region バージョン

        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Version
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
            }
        }

        #endregion

        #region Clone

        /// --------------------------------------------------
        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public CondBase Clone()
        {
            CondBase clone = (CondBase)this.MemberwiseClone();
            clone.LoginInfo = this.LoginInfo.Clone();

            return clone;
        }

        #region ICloneable メンバ

        /// --------------------------------------------------
        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion

        #endregion

        #region プロパティのコピー

        /// --------------------------------------------------
        /// <summary>
        /// ログイン情報をユーザー情報からコピーする。
        /// </summary>
        /// <param name="target">コンディションクラスかUserInfoプロパティ</param>
        /// <create>K.Tsutsumi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void SetLoginfoFromUserInfo(object target)
        {
            try
            {
                if (this.LoginInfo == null) return;
                //object loginInfo = null;
                if (this.LoginInfo.GetType().Name.Equals(ComDefine.LOGININFO_TYPENAME))
                {
                    // ログイン情報の場合の処理
                    this.LoginInfo = (LoginInfo)target;
                    return;
                }

                Type userInfoType = target.GetType();
                // ログイン情報のプロパティで走査
                foreach (System.Reflection.PropertyInfo item in this.LoginInfo.GetType().GetProperties())
                {
                    // ユーザー情報にログイン情報と同じプロパティがあるか判定
                    System.Reflection.PropertyInfo pi = userInfoType.GetProperty(item.Name);
                    if (item != null && pi.PropertyType == item.PropertyType)
                    {
                        try
                        {
                            // 同名、同型のプロパティがある。
                            item.SetValue(this.LoginInfo, pi.GetValue(target, null), null);
                        }
                        catch (Exception) { }
                    }
                }
            }
            catch (Exception) { }
        }

        #endregion

    }
}
