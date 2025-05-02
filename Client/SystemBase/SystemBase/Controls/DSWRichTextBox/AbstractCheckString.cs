using System;
using System.Collections.Generic;
using System.Text;

namespace DSWControl.DSWRichTextBox
{
#if (!DEBUG)
    [System.Diagnostics.DebuggerStepThrough()]
#endif
    internal abstract class AbstractCheckString
    {
        public abstract string StartCheck(string targetString, Dictionary<int, string> targetType);

        /// --------------------------------------------------
        /// <summary>
        /// 正規表現追加
        /// </summary>
        /// <param name="str">検査対象文字列</param>
        /// <param name="addStr">追加する正規表現</param>
        /// <returns>修正された正規表現</returns>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual string AddRegExpression(string str, string addStr)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return "|" + addStr;
            }
            else
            {
                return addStr;
            }
        }
    }
}
