using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;

namespace WsConnection.WebRefS02
{
    /// --------------------------------------------------
    /// <summary>
    /// CondS02クラス
    /// </summary>
    /// <create>AutoCodeGenerator 2018/09/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class CondS02 : CondBase
    {
        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>AutoCodeGenerator 2018/09/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondS02()
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target">ユーザー情報</param>
        /// <param name="textKonpozumi">文字列:梱包済み</param>
        /// <param name="textShukka">文字列:出荷</param>
        /// <param name="textShosai">文字列:詳細</param>
        /// <create>AutoCodeGenerator 2018/09/28</create>
        /// <update>D.Okumura 2018/10/10 多言語化対応</update>
        /// --------------------------------------------------
        public CondS02(UserInfo target, string textKonpozumi, string textShukka, string textShosai)
            : base(target)
        {
            this.TextShukka = textShukka;
            this.TextKonpozumi = textKonpozumi;
            this.TextShosai =textShosai;
        }

        #endregion
    }

    /// --------------------------------------------------
    /// <summary>
    /// CondBaseクラス
    /// </summary>
    /// <create>AutoCodeGenerator 2018/09/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class CondBase
    {
        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>AutoCodeGenerator 2018/09/28</create>
        /// <update></update>
        /// --------------------------------------------------
        protected CondBase()
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target">ユーザー情報</param>
        /// <create>AutoCodeGenerator 2018/09/28</create>
        /// <update></update>
        /// --------------------------------------------------
        protected CondBase(UserInfo target)
        {
            this.SetLoginfoFromUserInfo(target);
        }

        #endregion

        #region プロパティのコピー

        /// --------------------------------------------------
        /// <summary>
        /// ログイン情報をユーザー情報からコピーする。
        /// </summary>
        /// <param name="target">ユーザー情報</param>
        /// <create>AutoCodeGenerator 2018/09/28</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void SetLoginfoFromUserInfo(object target)
        {
            try
            {
                if (target == null) return;
                if (this.LoginInfo == null) this.LoginInfo = new LoginInfo();
                if (target.GetType().Name.Equals(ComDefine.LOGININFO_TYPENAME))
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
