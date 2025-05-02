using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;

namespace Condition
{
    /// --------------------------------------------------
    /// <summary>
    /// P02用コンディション
    /// </summary>
    /// <create>T.Sakiori 2012/04/16</create>
    /// <update></update>
    /// --------------------------------------------------
    public class CondP02 : CondBase
    {
        
        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>K.Tsutsumi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondP02()
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
        public CondP02(LoginInfo target)
            : base(target)
        {
        }

        #endregion

        #region Value2

        /// --------------------------------------------------
        /// <summary>
        /// Value2
        /// </summary>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Value2 { get; set; }

        #endregion

        #region 画面区分

        /// --------------------------------------------------
        /// <summary>
        /// 画面区分
        /// </summary>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public string GamenFlag { get; set; }

        #endregion

        #region 出荷区分

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShukkaFlag { get; set; }

        #endregion

        #region 納入先コード

        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public string NonyusakiCd { get; set; }

        #endregion

        #region ARNo

        /// --------------------------------------------------
        /// <summary>
        /// ARNo
        /// </summary>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ARNo { get; set; }

        #endregion

        #region 操作区分

        /// --------------------------------------------------
        /// <summary>
        /// 操作区分
        /// </summary>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public string OperationFlag { get; set; }

        #endregion

        #region 更新日時(開始)

        /// --------------------------------------------------
        /// <summary>
        /// 更新日時(開始)
        /// </summary>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public object UpdateDateFrom { get; set; }

        #endregion

        #region 更新日時(終了)

        /// --------------------------------------------------
        /// <summary>
        /// 更新日時(終了)
        /// </summary>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public object UpdateDateTo { get; set; }

        #endregion

        #region 担当者名

        /// --------------------------------------------------
        /// <summary>
        /// 担当者名
        /// </summary>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public string UserName { get; set; }

        #endregion
    }
}
