using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace DSWControl.DSWRichTextBox
{
    /// --------------------------------------------------
    /// <summary>
    /// 禁止文字クラス
    /// </summary>
    /// <create>sakiori 2008/07/02</create>
    /// <update></update>
    /// --------------------------------------------------
#if (!DEBUG)
    [System.Diagnostics.DebuggerStepThrough()]
#endif
    internal class CheckProphitionChar : AbstractCheckString
    {
        #region Constructors
        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>T.Sakiori 2008/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public CheckProphitionChar() { }
        #endregion

        #region Override Methods
        /// --------------------------------------------------
        /// <summary>
        /// チェック開始
        /// </summary>
        /// <param name="targetString">検査対象文字列</param>
        /// <param name="targetType">検査種別</param>
        /// <returns>検査済み文字列</returns>
        /// <create>T.Sakiori 2008/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public override string StartCheck(string targetString, Dictionary<int, string> targetType)
        {
            string regExpression = string.Empty;

            if (targetType != null)
            {
                foreach (KeyValuePair<int, string> var in targetType)
                {
                    // 直接指定の場合
                    if (var.Key >= 0)
                    {
                        regExpression += AddRegExpression(regExpression, CreateRegularExpression(0, var.Value));
                    }
                    // 正規表現の場合
                    else
                    {
                        regExpression += AddRegExpression(regExpression, CreateRegularExpression(1, var.Value));
                    }
                }
            }
            if (string.IsNullOrEmpty(regExpression))
            {
                return targetString;
            }
            return CheckTarget(targetString, regExpression);
        }
        #endregion

        #region Private Methods
        /// --------------------------------------------------
        /// <summary>
        /// 入力文字を検査対象の正規表現文字列で検査する
        /// </summary>
        /// <param name="targetString">検査対象文字列</param>
        /// <param name="regularExpression">正規表現文字列</param>
        /// <returns>検査済み文字列</returns>
        /// <create>T.Sakiori 2008/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string CheckTarget(string targetString, string regularExpression)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Regex.Replace(targetString, regularExpression, string.Empty, RegexOptions.None));
            return sb.ToString();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 入力された規制対象から正規表現を作成する
        /// </summary>
        /// <param name="targetType">検査種別</param>
        /// <param name="banChar">禁止文字列</param>
        /// <returns>正規表現文字列</returns>
        /// <create>T.Sakiori 2008/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string CreateRegularExpression(int targetType, string banChar)
        {
            string retSrt = string.Empty;
            // 直接指定の場合
            if (targetType == 0)
            {
                // エスケープ処理
                banChar = banChar.Replace(@"\", @"\\");
                banChar = banChar.Replace(@".", @"\.");
                banChar = banChar.Replace(@"$", @"\$");
                banChar = banChar.Replace(@"^", @"\^");
                banChar = banChar.Replace(@"{", @"\{");
                banChar = banChar.Replace(@"}", @"\}");
                banChar = banChar.Replace(@"[", @"\[");
                banChar = banChar.Replace(@"]", @"\]");
                banChar = banChar.Replace(@"(", @"\(");
                banChar = banChar.Replace(@")", @"\)");
                banChar = banChar.Replace(@"|", @"\|");
                banChar = banChar.Replace(@"*", @"\*");
                banChar = banChar.Replace(@"+", @"\+");
                banChar = banChar.Replace(@"?", @"\?");

                retSrt += banChar;
            }
            // 正規表現の場合
            else
            {
                retSrt += banChar;
            }
            return retSrt;
        }
        #endregion
    }
}
