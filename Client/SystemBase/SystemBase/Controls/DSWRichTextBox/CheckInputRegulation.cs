using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DSWControl.DSWRichTextBox
{
    /// --------------------------------------------------
    /// <summary>
    /// 入力規制クラス
    /// </summary>
    /// <create>D.Okumura 2019/06/13</create>
    /// <update></update>
    /// --------------------------------------------------
#if (!DEBUG)
    [System.Diagnostics.DebuggerStepThrough()]
#endif
    internal class CheckInputRegulation : AbstractCheckString
    {
        private bool _multiline = false;

        /// --------------------------------------------------
        /// <summary>
        /// 対象を使用可能にするかどうか
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isInputRegulation;

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public CheckInputRegulation()
        {
            this._isInputRegulation = true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="isInputRegulation">対象を使用可能にするかどうか</param>
        /// <param name="multiline">複数行かどうか</param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public CheckInputRegulation(bool isInputRegulation, bool multiline)
        {
            this._isInputRegulation = isInputRegulation;
            this._multiline = multiline;
        }

        /// --------------------------------------------------
        /// <summary>
        /// チェック開始のメソッド
        /// </summary>
        /// <param name="targetString">検査対象文字列</param>
        /// <param name="targetType">検査種別</param>
        /// <returns>検査済み文字列</returns>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update>sakiori 2008/07/02</update>
        /// --------------------------------------------------
        public override string StartCheck(string targetString, Dictionary<int, string> targetType)
        {
            string value = string.Empty;
            string regularExpression = string.Empty;

            if (targetType.TryGetValue(0, out value))
            {
                regularExpression = CreateRegularExpression(value, _isInputRegulation);
            }
            if (string.IsNullOrEmpty(regularExpression))
            {
                return targetString;
            }
            return CheckTarget(targetString, regularExpression);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 入力文字を検査対象の正規表現文字列で検査する
        /// </summary>
        /// <param name="targetString">検査対象文字列</param>
        /// <param name="regularExpression">正規表現文字列</param>
        /// <returns>検査済み文字列</returns>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update>sakiori 2008/07/02</update>
        /// --------------------------------------------------
        private string CheckTarget(string targetString, string regularExpression)
        {
            StringBuilder retStr = new StringBuilder();

            for (int i = 0; i < targetString.Length; i++)
            {
                if (Regex.IsMatch(targetString[i].ToString(), regularExpression, RegexOptions.None))
                {
                    retStr.Append(targetString[i]);
                }
            }
            return retStr.ToString();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 入力された規制対象から正規表現を作成する
        /// </summary>
        /// <param name="targetType">検査種別</param>
        /// <param name="isUse">使用可にするのか不可にするのかのフラグ</param>
        /// <returns>正規表現文字列</returns>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update>sakiori 2008/12/17</update>
        /// --------------------------------------------------
        private string CreateRegularExpression(string targetType, bool isUse)
        {
            string retStr = string.Empty;
            if (isUse)
            {
                for (int i = 0; i < targetType.Length; i++)
                {
                    switch (targetType[i])
                    {
                        case 'A':   // 全角：大文字アルファベット
                            retStr += AddRegExpression(retStr, "[Ａ-Ｚ]");
                            break;
                        case 'B':   // 全角：小文字アルファベット
                            retStr += AddRegExpression(retStr, "[ａ-ｚ]");
                            break;
                        case 'N':   // 全角：数字
                            retStr += AddRegExpression(retStr, "[０-９]");
                            break;
                        case 'R':   // 全角：数字および関連記号
                            retStr += AddRegExpression(retStr, "[０-９＋－＄％￥，．]");
                            break;
                        case 'L':   // 全角：記号
                            retStr += AddRegExpression(retStr, "[^一-龠ぁ-んァ-ヴ０-９!-~｡-ﾟＡ-Ｚａ-ｚ]");
                            break;
                        case 'K':   // 全角：カタカナ 
                            retStr += AddRegExpression(retStr, "[ァ-ヴー]");
                            break;
                        case 'S':   // 全角：スペース 
                            retStr += AddRegExpression(retStr, "[　]");
                            break;
                        //case 'Z':   // 全角文字（記号なし）
                        //    retStr += AddRegExpression(retStr, "[一-龥ぁ-んァ-ヴ０-９]");
                        //    break;
                        case 'H':   // ひらがな
                            retStr += AddRegExpression(retStr, "[ぁ-んー]");
                            break;
                        case 'a':   // 半角：大文字アルファベット
                            retStr += AddRegExpression(retStr, "[A-Z]");
                            break;
                        case 'b':   // 半角：小文字アルファベット
                            retStr += AddRegExpression(retStr, "[a-z]");
                            break;
                        case 'n':   // 半角：数字
                            retStr += AddRegExpression(retStr, "[0-9]");
                            break;
                        case 'r':   // 半角：数字および数字関連記号
                            retStr += AddRegExpression(retStr, "[-0-9+$%\\,.]");
                            break;
                        case 'l':   // 半角：記号
                            retStr += AddRegExpression(retStr, "[｡-･!-/:-@[-`{-~]");
                            break;
                        case 'k':   // 半角：カタカナ
                            retStr += AddRegExpression(retStr, "[ｦ-ﾟ]");
                            break;
                        case 's':   // 半角：スペース
                            retStr += AddRegExpression(retStr, "[ ]");
                            break;
                        case 'z':   // 半角文字（記号なし）
                            retStr += AddRegExpression(retStr, "[ｦ-ﾟ0-9A-Za-z]");
                            break;
                        case 'F':   // サロゲート文字
                            retStr += AddRegExpression(retStr, "[\uD800-\uDBFF\uDC00-\uDFFF]");
                            break;
                        default:
                            break;
                    }
                }

                if (this._multiline)
                {
                    retStr += AddRegExpression(retStr, "[\r\n]");
                }
            }
            else
            {
                for (int i = 0; i < targetType.Length; i++)
                {
                    switch (targetType[i])
                    {
                        case 'A':   // 全角：大文字アルファベット
                            retStr += "Ａ-Ｚ"; break;
                        case 'B':   // 全角：小文字アルファベット
                            retStr += "ａ-ｚ"; break;
                        case 'N':   // 全角：数字
                            retStr += "０-９"; break;
                        case 'R':   // 全角：数字および関連記号
                            retStr += "０-９＋－＄％￥，．"; break;
                        case 'L':   // 全角：記号
                            retStr = "^一-龠Ａ-Ｚａ-ｚぁ-んァ-ヴ０-９!-~｡-ﾟ" + retStr; break;
                        case 'K':   // 全角：カタカナ
                            retStr += "ァ-ヴー"; break;
                        case 'S':   // 全角：スペース
                            retStr += "　"; break;
                        case 'H':   // ひらがな
                            retStr += "ぁ-んー"; break;
                        case 'a':   // 半角：大文字アルファベット
                            retStr += "A-Z"; break;
                        case 'b':   // 半角：小文字アルファベット
                            retStr += "a-z"; break;
                        case 'n':   // 半角：数字
                            retStr += "0-9"; break;
                        case 'r':   // 半角：数字および数字関連記号
                            retStr += "0-9-+$%\\,."; break;
                        case 'l':   // 半角：記号
                            retStr += "｡-･!-/:-@[-`{-~"; break;
                        case 'k':   // 半角：カタカナ
                            retStr += "ｦ-ﾟ"; break;
                        case 's':   // 半角：スペース
                            retStr += " "; break;
                        case 'z':   // 半角文字(記号無し)
                            retStr += "ｦ-ﾟ0-9A-Za-z"; break;
                        case 'F':   // サロゲート文字
                            retStr += "\uD800-\uDBFF\uDC00-\uDFFF"; break;
                        default:
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(retStr))
                {
                    retStr = "[^" + retStr + "]";
                }
            }
            return retStr;
        }
    }
}
