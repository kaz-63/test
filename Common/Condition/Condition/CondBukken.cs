using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;

namespace Condition
{
    /// --------------------------------------------------
    /// <summary>
    /// 物件データ用コンディション
    /// </summary>
    /// <create>T.Sakiori 2012/04/05</create>
    /// <update></update>
    /// --------------------------------------------------
    public class CondBukken : CondBase
    {

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>K.Tsutsumi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondBukken()
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
        public CondBukken(LoginInfo target)
            : base(target)
        {
        }

        #endregion

        #region 出荷区分

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShukkaFlag { get; set; }

        #endregion

        #region 物件名

        /// --------------------------------------------------
        /// <summary>
        /// 物件名
        /// </summary>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public string BukkenName { get; set; }

        #endregion
    }
}
