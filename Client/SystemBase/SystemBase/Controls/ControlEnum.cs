using System;
using System.Collections.Generic;
using System.Text;

namespace SystemBase.Controls
{
    /// --------------------------------------------------
    /// <summary>
    /// メッセージのアイコンタイプ
    /// </summary>
    /// <create>R.Katsuo 2010/01/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public enum MessageImageType
    {
        /// --------------------------------------------------
        /// <summary>
        /// アイコンなし
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        None = -1,

        /// --------------------------------------------------
        /// <summary>
        /// エラーアイコン
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        Error = 16,

        /// --------------------------------------------------
        /// <summary>
        /// 情報アイコン
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        Information = 64,

        /// --------------------------------------------------
        /// <summary>
        /// 確認アイコン
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        Question = 32,

        /// --------------------------------------------------
        /// <summary>
        /// 警告アイコン
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        Warning = 48,
    }
}
