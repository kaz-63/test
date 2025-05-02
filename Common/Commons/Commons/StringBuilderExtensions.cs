using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commons
{
    /// --------------------------------------------------
    /// <summary>
    /// StringBuilderの拡張メソッド
    /// </summary>
    /// <create>Y.Higuchi 2010/08/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public static class StringBuilderExtensions
    {
        /// --------------------------------------------------
        /// <summary>
        /// AppendLineと同じ機能
        /// </summary>
        /// <param name="target">StringBuilder</param>
        /// <param name="value">追加する文字列</param>
        /// <returns>StringBuilder</returns>
        /// <create>Y.Higuchi 2010/08/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public static StringBuilder ApdL(this StringBuilder target, string value)
        {
            return target.AppendLine(value);
        }
        /// --------------------------------------------------
        /// <summary>
        /// AppendLineと同じ機能
        /// </summary>
        /// <param name="target">StringBuilder</param>
        /// <returns>StringBuilder</returns>
        /// <create>D.Okumura 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static StringBuilder ApdL(this StringBuilder target)
        {
            return target.AppendLine();
        }

        /// --------------------------------------------------
        /// <summary>
        /// Appendと同じ機能(文字列のみ)
        /// </summary>
        /// <param name="target">StringBuilder</param>
        /// <param name="value">追加する文字列</param>
        /// <returns>StringBuilder</returns>
        /// <create>Y.Higuchi 2010/08/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public static StringBuilder ApdN(this StringBuilder target, string value)
        {
            return target.Append(value);
        }

        /// --------------------------------------------------
        /// <summary>
        /// AppendFormatと同じ機能
        /// </summary>
        /// <param name="target">StringBuilder</param>
        /// <param name="format">複合書式指定文字列</param>
        /// <param name="args">書式指定するオブジェクトの配列</param>
        /// <returns>StringBuilder</returns>
        /// <create>Y.Higuchi 2010/08/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public static StringBuilder ApdF(this StringBuilder target, string format, params object[] args)
        {
            return target.AppendFormat(format, args);
        }
    }
}
