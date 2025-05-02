using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Commons
{
    /// --------------------------------------------------
    /// <summary>
    /// 入力規制クラス
    /// </summary>
    /// <create>Y.Higuchi 2010/07/16</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ComRegulation
    {
        #region 定数

        #region 入力文字属性の種別定義

        /// --------------------------------------------------
        /// <summary>
        /// 全角：大文字アルファベット
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public const string REGULATION_WIDE_ALPHA_UP = "A";
        /// --------------------------------------------------
        /// <summary>
        /// 半角：大文字アルファベット
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public const string REGULATION_NARROW_ALPHA_UP = "a";
        /// --------------------------------------------------
        /// <summary>
        /// 全角：小文字アルファベット
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public const string REGULATION_WIDE_ALPHA_LOW = "B";
        /// --------------------------------------------------
        /// <summary>
        /// 半角：小文字アルファベット
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public const string REGULATION_NARROW_ALPHA_LOW = "b";
        /// --------------------------------------------------
        /// <summary>
        /// 全角：数字
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public const string REGULATION_WIDE_NUMERIC_ONLY = "N";
        /// --------------------------------------------------
        /// <summary>
        /// 半角：数字
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public const string REGULATION_NARROW_NUMERIC_ONLY = "n";
        /// --------------------------------------------------
        /// <summary>
        /// 全角：数字および数字関連記号
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public const string REGULATION_WIDE_NUMERIC_SIGN = "R";
        /// --------------------------------------------------
        /// <summary>
        /// 半角：数字および数字関連記号
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public const string REGULATION_NARROW_NUMERIC_SIGN = "r";
        /// --------------------------------------------------
        /// <summary>
        /// 全角：記号
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public const string REGULATION_WIDE_SIGN = "L";
        /// --------------------------------------------------
        /// <summary>
        /// 半角：記号
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public const string REGULATION_NARROW_SIGN = "l";
        /// --------------------------------------------------
        /// <summary>
        /// 全角：カタカナ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public const string REGULATION_WIDE_KATAKANA = "K";
        /// --------------------------------------------------
        /// <summary>
        /// 半角：カタカナ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public const string REGULATION_NARROW_KATAKANA = "k";
        /// --------------------------------------------------
        /// <summary>
        /// 全角：スペース
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public const string REGULATION_WIDE_SPACE = "S";
        /// --------------------------------------------------
        /// <summary>
        /// 半角：スペース
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public const string REGULATION_NARROW_SPACE = "s";
        /// --------------------------------------------------
        /// <summary>
        /// 半角文字(記号なし)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public const string REGULATION_NARROW_CHAR = "z";
        /// --------------------------------------------------
        /// <summary>
        /// ひらがな
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public const string REGULATION_HIRAGANA = "H";
        /// --------------------------------------------------
        /// <summary>
        /// サロゲート文字
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public const string REGULATION_SURROGATE = "F";

        #endregion

        #region 入力文字属性の正規表現定義

        /// --------------------------------------------------
        /// <summary>
        /// 全角：大文字アルファベット
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string EXPRESSION_WIDE_ALPHA_UP = "Ａ-Ｚ";
        /// --------------------------------------------------
        /// <summary>
        /// 半角：大文字アルファベット
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string EXPRESSION_NARROW_ALPHA_UP = "A-Z";
        /// --------------------------------------------------
        /// <summary>
        /// 全角：小文字アルファベット
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string EXPRESSION_WIDE_ALPHA_LOW = "ａ-ｚ";
        /// --------------------------------------------------
        /// <summary>
        /// 半角：小文字アルファベット
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string EXPRESSION_NARROW_ALPHA_LOW = "a-z";
        /// --------------------------------------------------
        /// <summary>
        /// 全角：数字
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string EXPRESSION_WIDE_NUMERIC_ONLY = "０-９";
        /// --------------------------------------------------
        /// <summary>
        /// 半角：数字
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string EXPRESSION_NARROW_NUMERIC_ONLY = "0-9";
        /// --------------------------------------------------
        /// <summary>
        /// 全角：数字および数字関連記号
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string EXPRESSION_WIDE_NUMERIC_SIGN = "０-９＋－＄％￥，．";
        /// --------------------------------------------------
        /// <summary>
        /// 半角：数字および数字関連記号
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string EXPRESSION_NARROW_NUMERIC_SIGN = "-0-9+$%\\,.";
        /// --------------------------------------------------
        /// <summary>
        /// 全角：記号
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string EXPRESSION_WIDE_SIGN = "^一-龠ぁ-んァ-ヴ０-９!-~｡-ﾟＡ-Ｚａ-ｚ";
        /// --------------------------------------------------
        /// <summary>
        /// 半角：記号
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string EXPRESSION_NARROW_SIGN = "｡-･!-/:-@[-`{-~";
        /// --------------------------------------------------
        /// <summary>
        /// 全角：カタカナ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string EXPRESSION_WIDE_KATAKANA = "ァ-ヴー";
        /// --------------------------------------------------
        /// <summary>
        /// 半角：カタカナ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string EXPRESSION_NARROW_KATAKANA = "ｦ-ﾟ";
        /// --------------------------------------------------
        /// <summary>
        /// 全角：スペース
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string EXPRESSION_WIDE_SPACE = "　";
        /// --------------------------------------------------
        /// <summary>
        /// 半角：スペース
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string EXPRESSION_NARROW_SPACE = " ";
        /// --------------------------------------------------
        /// <summary>
        /// 半角文字(記号なし)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string EXPRESSION_NARROW_CHAR = "ｦ-ﾟ0-9A-Za-z";
        /// --------------------------------------------------
        /// <summary>
        /// ひらがな
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string EXPRESSION_HIRAGANA = "ぁ-んー";
        /// --------------------------------------------------
        /// <summary>
        /// サロゲート文字
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string EXPRESSION_SURROGATE = "\uD800-\uDBFF\uDC00-\uDFFF";

        #endregion

        #endregion

        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public ComRegulation()
        {
        }

        #endregion

        #region 種別から正規表現を取得

        /// --------------------------------------------------
        /// <summary>
        /// 種別から正規表現を取得
        /// </summary>
        /// <param name="targetType">種別</param>
        /// <param name="isUse">使用可にするのか不可にするのかのフラグ</param>
        /// <param name="isMultiLine">複数行かどうか</param>
        /// <returns>正規表現文字列</returns>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private string CreateRegularExpression(string targetType, bool isUse, bool isMultiLine)
        {
            string ret = string.Empty;
            List<string> regulations = new List<string>();
            foreach (char targetChar in targetType.ToCharArray())
            {
                switch (targetChar.ToString())
                {
                    case REGULATION_WIDE_ALPHA_UP:
                        // 全角：大文字アルファベット
                        regulations.Add(EXPRESSION_WIDE_ALPHA_UP);
                        break;
                    case REGULATION_NARROW_ALPHA_UP:
                        // 半角：大文字アルファベット
                        regulations.Add(EXPRESSION_NARROW_ALPHA_UP);
                        break;
                    case REGULATION_WIDE_ALPHA_LOW:
                        // 全角：小文字アルファベット
                        regulations.Add(EXPRESSION_WIDE_ALPHA_LOW);
                        break;
                    case REGULATION_NARROW_ALPHA_LOW:
                        // 半角：小文字アルファベット
                        regulations.Add(EXPRESSION_NARROW_ALPHA_LOW);
                        break;
                    case REGULATION_WIDE_NUMERIC_ONLY:
                        // 全角：数字
                        regulations.Add(EXPRESSION_WIDE_NUMERIC_ONLY);
                        break;
                    case REGULATION_NARROW_NUMERIC_ONLY:
                        // 半角：数字
                        regulations.Add(EXPRESSION_NARROW_NUMERIC_ONLY);
                        break;
                    case REGULATION_WIDE_NUMERIC_SIGN:
                        // 全角：数字および数字関連記号
                        regulations.Add(EXPRESSION_WIDE_NUMERIC_SIGN);
                        break;
                    case REGULATION_NARROW_NUMERIC_SIGN:
                        // 半角：数字および数字関連記号
                        regulations.Add(EXPRESSION_NARROW_NUMERIC_SIGN);
                        break;
                    case REGULATION_WIDE_SIGN:
                        // 全角：記号
                        regulations.Add(EXPRESSION_WIDE_SIGN + Environment.NewLine);
                        break;
                    case REGULATION_NARROW_SIGN:
                        // 半角：記号
                        regulations.Add(EXPRESSION_NARROW_SIGN);
                        break;
                    case REGULATION_WIDE_KATAKANA:
                        // 全角：カタカナ
                        regulations.Add(EXPRESSION_WIDE_KATAKANA);
                        break;
                    case REGULATION_NARROW_KATAKANA:
                        // 半角：カタカナ
                        regulations.Add(EXPRESSION_NARROW_KATAKANA);
                        break;
                    case REGULATION_WIDE_SPACE:
                        // 全角：スペース
                        regulations.Add(EXPRESSION_WIDE_SPACE);
                        break;
                    case REGULATION_NARROW_SPACE:
                        // 半角：スペース
                        regulations.Add(EXPRESSION_NARROW_SPACE);
                        break;
                    case REGULATION_NARROW_CHAR:
                        // 半角文字(記号なし)
                        regulations.Add(EXPRESSION_NARROW_CHAR);
                        break;
                    case REGULATION_HIRAGANA:
                        // ひらがな
                        regulations.Add(EXPRESSION_HIRAGANA);
                        break;
                    case REGULATION_SURROGATE:
                        // サロゲート文字
                        regulations.Add(EXPRESSION_SURROGATE);
                        break;
                    default:
                        break;
                }
            }
            if (isUse && isMultiLine)
            {
                regulations.Add(Environment.NewLine);
            }
            ret = "[" + string.Join("]|[", regulations.ToArray()) + "]";
            return ret;
        }

        #endregion

        #region 入力規制にマッチする文字列の取得

        /// --------------------------------------------------
        /// <summary>
        /// 入力規制にマッチする文字列の取得
        /// </summary>
        /// <param name="targetString">検査対象文字列</param>
        /// <param name="targetType">入力規制の種類</param>
        /// <param name="isUse">使用可にするのか不可にするのかのフラグ</param>
        /// <param name="isMultiline">複数行かどうか</param>
        /// <returns>検査済み文字列</returns>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public string GetRegularString(string targetString, string targetType, bool isUse, bool isMultiline)
        {
            string regularExpression = string.Empty;

            regularExpression = this.CreateRegularExpression(targetType, isUse, isMultiline);

            if (string.IsNullOrEmpty(regularExpression))
            {
                return targetString;
            }
            StringBuilder ret = new StringBuilder();

            foreach (char targetChar in targetString.ToCharArray())
            {
                if (Regex.IsMatch(targetChar.ToString(), regularExpression, RegexOptions.None) == isUse)
                {
                    ret.Append(targetChar);
                }
            }
            return ret.ToString();
        }

        #endregion

        #region 入力規制にマッチするのみかどうかのチェック

        /// --------------------------------------------------
        /// <summary>
        /// 入力規制にマッチするのみかどうかのチェック
        /// </summary>
        /// <param name="targetString">検査対象文字列</param>
        /// <param name="targetType">入力規制の種類</param>
        /// <param name="isUse">使用可にするのか不可にするのかのフラグ</param>
        /// <param name="isMultiline">複数行かどうか</param>
        /// <returns>true：マッチするもののみ/false:マッチするもの以外が含まれる</returns>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool CheckRegularString(string targetString, string targetType, bool isUse, bool isMultiline)
        {
            string regularExpression = string.Empty;

            regularExpression = this.CreateRegularExpression(targetType, isUse, isMultiline);

            if (string.IsNullOrEmpty(regularExpression))
            {
                return !isUse;
            }
            StringBuilder ret = new StringBuilder();

            foreach (char targetChar in targetString.ToCharArray())
            {
                if (Regex.IsMatch(targetChar.ToString(), regularExpression, RegexOptions.None) != isUse)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion
    }
}
