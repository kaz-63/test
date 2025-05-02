using System;
using System.Windows.Forms;
using SystemBase.Properties;

namespace SystemBase.Util
{
    /// --------------------------------------------------
    /// <summary>
    /// 拡張メッセージボックスクラス
    /// </summary>
    /// <create>Y.Higuchi 2008/12/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public class CustomMsgBoxEx
    {

        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private CustomMsgBoxEx()
        {
        }

        #endregion

        #region Show Methods

        #region Show(string text)

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text)
        {
            return InnerShowDialog(null, text, "", false, "", 0, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, int timeoutSecond)
        {
            return InnerShowDialog(null, text, "", false, "", timeoutSecond, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region Show(string text, string caption)

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption)
        {
            return InnerShowDialog(null, text, caption, false, "", 0, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, string detail)
        {
            return InnerShowDialog(null, text, caption, true, detail, 0, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, int timeoutSecond)
        {
            return InnerShowDialog(null, text, caption, false, "", timeoutSecond, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, string detail, int timeoutSecond)
        {
            return InnerShowDialog(null, text, caption, true, detail, timeoutSecond, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region Show(string text, string caption, MessageBoxButtons buttons)

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        {
            return InnerShowDialog(null, text, caption, false, "", 0, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, string detail, MessageBoxButtons buttons)
        {
            return InnerShowDialog(null, text, caption, true, detail, 0, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, int timeoutSecond, MessageBoxButtons buttons)
        {
            return InnerShowDialog(null, text, caption, false, "", timeoutSecond, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, string detail, int timeoutSecond, MessageBoxButtons buttons)
        {
            return InnerShowDialog(null, text, caption, true, detail, timeoutSecond, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return InnerShowDialog(null, text, caption, false, "", 0, buttons, icon, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, string detail, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return InnerShowDialog(null, text, caption, true, detail, 0, buttons, icon, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, int timeoutSecond, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return InnerShowDialog(null, text, caption, false, "", timeoutSecond, buttons, icon, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return InnerShowDialog(null, text, caption, true, detail, timeoutSecond, buttons, icon, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowDialog(null, text, caption, false, "", 0, buttons, icon, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, string detail, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowDialog(null, text, caption, true, detail, 0, buttons, icon, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, int timeoutSecond, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowDialog(null, text, caption, false, "", timeoutSecond, buttons, icon, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowDialog(null, text, caption, true, detail, timeoutSecond, buttons, icon, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath)

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowDialog(null, text, caption, false, "", 0, buttons, icon, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, string detail, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowDialog(null, text, caption, true, detail, 0, buttons, icon, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, int timeoutSecond, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowDialog(null, text, caption, false, "", timeoutSecond, buttons, icon, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowDialog(null, text, caption, true, detail, timeoutSecond, buttons, icon, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowDialog(null, text, caption, false, "", 0, buttons, icon, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, string detail, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowDialog(null, text, caption, true, detail, 0, buttons, icon, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, int timeoutSecond, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowDialog(null, text, caption, false, "", timeoutSecond, buttons, icon, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowDialog(null, text, caption, true, detail, timeoutSecond, buttons, icon, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        #endregion

        #region Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowDialog(null, text, caption, false, "", 0, buttons, icon, defaultButton, true, helpFilePath, navigator, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, string detail, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowDialog(null, text, caption, true, detail, 0, buttons, icon, defaultButton, true, helpFilePath, navigator, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, int timeoutSecond, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowDialog(null, text, caption, false, "", timeoutSecond, buttons, icon, defaultButton, true, helpFilePath, navigator, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowDialog(null, text, caption, true, detail, timeoutSecond, buttons, icon, defaultButton, true, helpFilePath, navigator, null);
        }

        #endregion

        #region Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowDialog(null, text, caption, false, "", 0, buttons, icon, defaultButton, true, helpFilePath, navigator, param);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, string detail, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowDialog(null, text, caption, true, detail, 0, buttons, icon, defaultButton, true, helpFilePath, navigator, param);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, int timeoutSecond, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowDialog(null, text, caption, false, "", timeoutSecond, buttons, icon, defaultButton, true, helpFilePath, navigator, param);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(string text, string caption, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowDialog(null, text, caption, true, detail, timeoutSecond, buttons, icon, defaultButton, true, helpFilePath, navigator, param);
        }

        #endregion

        #region Show(IWin32Window owner, string text)

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text)
        {
            return InnerShowDialog(owner, text, "", false, "", 0, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, int timeoutSecond)
        {
            return InnerShowDialog(owner, text, "", false, "", timeoutSecond, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region Show(IWin32Window owner, string text, string caption)

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption)
        {
            return InnerShowDialog(owner, text, caption, false, "", 0, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, string detail)
        {
            return InnerShowDialog(owner, text, caption, true, detail, 0, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, int timeoutSecond)
        {
            return InnerShowDialog(owner, text, caption, false, "", timeoutSecond, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, string detail, int timeoutSecond)
        {
            return InnerShowDialog(owner, text, caption, true, detail, timeoutSecond, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
        {
            return InnerShowDialog(owner, text, caption, false, "", 0, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, string detail, MessageBoxButtons buttons)
        {
            return InnerShowDialog(owner, text, caption, true, detail, 0, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, int timeoutSecond, MessageBoxButtons buttons)
        {
            return InnerShowDialog(owner, text, caption, false, "", timeoutSecond, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, string detail, int timeoutSecond, MessageBoxButtons buttons)
        {
            return InnerShowDialog(owner, text, caption, true, detail, timeoutSecond, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return InnerShowDialog(owner, text, caption, false, "", 0, buttons, icon, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, string detail, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return InnerShowDialog(owner, text, caption, true, detail, 0, buttons, icon, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, int timeoutSecond, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return InnerShowDialog(owner, text, caption, false, "", timeoutSecond, buttons, icon, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return InnerShowDialog(owner, text, caption, true, detail, timeoutSecond, buttons, icon, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowDialog(owner, text, caption, false, "", 0, buttons, icon, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, string detail, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowDialog(owner, text, caption, true, detail, 0, buttons, icon, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, int timeoutSecond, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowDialog(owner, text, caption, false, "", timeoutSecond, buttons, icon, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowDialog(owner, text, caption, true, detail, timeoutSecond, buttons, icon, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath)

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowDialog(owner, text, caption, false, "", 0, buttons, icon, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, string detail, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowDialog(owner, text, caption, true, detail, 0, buttons, icon, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, int timeoutSecond, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowDialog(owner, text, caption, false, "", timeoutSecond, buttons, icon, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowDialog(owner, text, caption, true, detail, timeoutSecond, buttons, icon, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }
        #endregion

        #region Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowDialog(owner, text, caption, false, "", 0, buttons, icon, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, string detail, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowDialog(owner, text, caption, true, detail, 0, buttons, icon, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, int timeoutSecond, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowDialog(owner, text, caption, false, "", timeoutSecond, buttons, icon, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowDialog(owner, text, caption, true, detail, timeoutSecond, buttons, icon, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        #endregion

        #region Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowDialog(owner, text, caption, false, "", 0, buttons, icon, defaultButton, true, helpFilePath, navigator, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, string detail, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowDialog(owner, text, caption, true, detail, 0, buttons, icon, defaultButton, true, helpFilePath, navigator, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, int timeoutSecond, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowDialog(owner, text, caption, false, "", timeoutSecond, buttons, icon, defaultButton, true, helpFilePath, navigator, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowDialog(owner, text, caption, true, detail, timeoutSecond, buttons, icon, defaultButton, true, helpFilePath, navigator, null);
        }

        #endregion

        #region Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowDialog(owner, text, caption, false, "", 0, buttons, icon, defaultButton, true, helpFilePath, navigator, param);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, string detail, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowDialog(owner, text, caption, true, detail, 0, buttons, icon, defaultButton, true, helpFilePath, navigator, param);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, int timeoutSecond, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowDialog(owner, text, caption, false, "", timeoutSecond, buttons, icon, defaultButton, true, helpFilePath, navigator, param);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult Show(IWin32Window owner, string text, string caption, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowDialog(owner, text, caption, true, detail, timeoutSecond, buttons, icon, defaultButton, true, helpFilePath, navigator, param);
        }

        #endregion

        #endregion

        #region ShowError Methods

        #region ShowError(string text)

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="exception">例外情報</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(Exception exception)
        {
            return InnerShowErrorDialog(null, exception.Message, true, "", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null, exception);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text)
        {
            return InnerShowErrorDialog(null, text, false, "", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text, string detail)
        {
            return InnerShowErrorDialog(null, text, true, detail, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text, Exception exception)
        {
            return InnerShowErrorDialog(null, text, true, "", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null, exception);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text, string detail, Exception exception)
        {
            return InnerShowErrorDialog(null, text, true, detail, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null, exception);
        }

        #endregion

        #region ShowError(string text, MessageBoxButtons buttons)

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(Exception exception, MessageBoxButtons buttons)
        {
            return InnerShowErrorDialog(null, exception.Message, true, "", buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null, exception);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text, MessageBoxButtons buttons)
        {
            return InnerShowErrorDialog(null, text, false, "", buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text, string detail, MessageBoxButtons buttons)
        {
            return InnerShowErrorDialog(null, text, true, detail, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text, Exception exception, MessageBoxButtons buttons)
        {
            return InnerShowErrorDialog(null, text, true, "", buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null, exception);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text, string detail, Exception exception, MessageBoxButtons buttons)
        {
            return InnerShowErrorDialog(null, text, true, detail, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null, exception);
        }

        #endregion

        #region ShowError(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowErrorDialog(null, exception.Message, true, "", buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null, exception);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowErrorDialog(null, text, false, "", buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowErrorDialog(null, text, true, detail, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text, Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowErrorDialog(null, text, true, "", buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null, exception);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text, string detail, Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowErrorDialog(null, text, true, detail, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null, exception);
        }

        #endregion

        #region ShowError(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowErrorDialog(null, exception.Message, true, "", buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null, exception);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowErrorDialog(null, text, false, "", buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowErrorDialog(null, text, true, detail, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text, Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowErrorDialog(null, text, true, "", buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null, exception);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text, string detail, Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowErrorDialog(null, text, true, detail, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null, exception);
        }

        #endregion

        #region ShowError(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowErrorDialog(null, exception.Message, true, "", buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword, exception);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowErrorDialog(null, text, false, "", buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowErrorDialog(null, text, true, detail, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text, Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowErrorDialog(null, text, true, "", buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword, exception);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text, string detail, Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowErrorDialog(null, text, true, detail, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword, exception);
        }

        #endregion

        #region ShowError(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowErrorDialog(null, exception.Message, true, "", buttons, defaultButton, true, helpFilePath, navigator, null, exception);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowErrorDialog(null, text, false, "", buttons, defaultButton, true, helpFilePath, navigator, null, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowErrorDialog(null, text, true, detail, buttons, defaultButton, true, helpFilePath, navigator, null, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text, Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowErrorDialog(null, text, true, "", buttons, defaultButton, true, helpFilePath, navigator, null, exception);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text, string detail, Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowErrorDialog(null, text, true, detail, buttons, defaultButton, true, helpFilePath, navigator, null, exception);
        }

        #endregion

        #region ShowError(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowErrorDialog(null, exception.Message, true, "", buttons, defaultButton, true, helpFilePath, navigator, param, exception);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowErrorDialog(null, text, false, "", buttons, defaultButton, true, helpFilePath, navigator, param, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowErrorDialog(null, text, true, detail, buttons, defaultButton, true, helpFilePath, navigator, param, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text, Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowErrorDialog(null, text, true, "", buttons, defaultButton, true, helpFilePath, navigator, param, exception);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(string text, string detail, Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowErrorDialog(null, text, true, detail, buttons, defaultButton, true, helpFilePath, navigator, param, exception);
        }

        #endregion

        #region ShowError(IWin32Window owner, string text)

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="exception">例外情報</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, Exception exception)
        {
            return InnerShowErrorDialog(owner, exception.Message, true, "", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null, exception);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text)
        {
            return InnerShowErrorDialog(owner, text, false, "", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text, string detail)
        {
            return InnerShowErrorDialog(owner, text, true, detail, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text, Exception exception)
        {
            return InnerShowErrorDialog(owner, text, true, "", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null, exception);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text, string detail, Exception exception)
        {
            return InnerShowErrorDialog(owner, text, true, detail, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null, exception);
        }

        #endregion

        #region ShowError(IWin32Window owner, string text, MessageBoxButtons buttons)

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, Exception exception, MessageBoxButtons buttons)
        {
            return InnerShowErrorDialog(owner, exception.Message, true, "", buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null, exception);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text, MessageBoxButtons buttons)
        {
            return InnerShowErrorDialog(owner, text, false, "", buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text, string detail, MessageBoxButtons buttons)
        {
            return InnerShowErrorDialog(owner, text, true, detail, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text, Exception exception, MessageBoxButtons buttons)
        {
            return InnerShowErrorDialog(owner, text, true, "", buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null, exception);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text, string detail, Exception exception, MessageBoxButtons buttons)
        {
            return InnerShowErrorDialog(owner, text, true, detail, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null, exception);
        }

        #endregion

        #region ShowError(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowErrorDialog(owner, exception.Message, true, "", buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null, exception);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowErrorDialog(owner, text, false, "", buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowErrorDialog(owner, text, true, detail, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text, Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowErrorDialog(owner, text, true, "", buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null, exception);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text, string detail, Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowErrorDialog(owner, text, true, detail, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null, exception);
        }

        #endregion

        #region ShowError(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowErrorDialog(owner, exception.Message, true, "", buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null, exception);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowErrorDialog(owner, text, false, "", buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowErrorDialog(owner, text, true, detail, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text, Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowErrorDialog(owner, text, true, "", buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null, exception);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text, string detail, Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowErrorDialog(owner, text, true, detail, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null, exception);
        }
        #endregion

        #region ShowError(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowErrorDialog(owner, exception.Message, true, "", buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword, exception);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowErrorDialog(owner, text, false, "", buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowErrorDialog(owner, text, true, detail, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text, Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowErrorDialog(owner, text, true, "", buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword, exception);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text, string detail, Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowErrorDialog(owner, text, true, detail, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword, exception);
        }

        #endregion

        #region ShowError(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowErrorDialog(owner, exception.Message, true, "", buttons, defaultButton, true, helpFilePath, navigator, null, exception);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowErrorDialog(owner, text, false, "", buttons, defaultButton, true, helpFilePath, navigator, null, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowErrorDialog(owner, text, true, detail, buttons, defaultButton, true, helpFilePath, navigator, null, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text, Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowErrorDialog(owner, text, true, "", buttons, defaultButton, true, helpFilePath, navigator, null, exception);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text, string detail, Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowErrorDialog(owner, text, true, detail, buttons, defaultButton, true, helpFilePath, navigator, null, exception);
        }

        #endregion

        #region ShowError(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowErrorDialog(owner, exception.Message, true, "", buttons, defaultButton, true, helpFilePath, navigator, param, exception);
        }


        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowErrorDialog(owner, text, false, "", buttons, defaultButton, true, helpFilePath, navigator, param, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowErrorDialog(owner, text, true, detail, buttons, defaultButton, true, helpFilePath, navigator, param, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text, Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowErrorDialog(owner, text, true, "", buttons, defaultButton, true, helpFilePath, navigator, param, exception);
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="exception">例外情報</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowError(IWin32Window owner, string text, string detail, Exception exception, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowErrorDialog(owner, text, true, detail, buttons, defaultButton, true, helpFilePath, navigator, param, exception);
        }

        #endregion

        #endregion

        #region ShowInformation Methods

        #region ShowInformation(string text)

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text)
        {
            return InnerShowInformationDialog(null, text, false, "", 0, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text, string detail)
        {
            return InnerShowInformationDialog(null, text, true, detail, 0, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text, int timeoutSecond)
        {
            return InnerShowInformationDialog(null, text, false, "", timeoutSecond, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text, string detail, int timeoutSecond)
        {
            return InnerShowInformationDialog(null, text, true, detail, timeoutSecond, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region ShowInformation(string text, MessageBoxButtons buttons)

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text, MessageBoxButtons buttons)
        {
            return InnerShowInformationDialog(null, text, false, "", 0, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text, string detail, MessageBoxButtons buttons)
        {
            return InnerShowInformationDialog(null, text, true, detail, 0, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text, int timeoutSecond, MessageBoxButtons buttons)
        {
            return InnerShowInformationDialog(null, text, false, "", timeoutSecond, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text, string detail, int timeoutSecond, MessageBoxButtons buttons)
        {
            return InnerShowInformationDialog(null, text, true, detail, timeoutSecond, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region ShowInformation(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowInformationDialog(null, text, false, "", 0, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowInformationDialog(null, text, true, detail, 0, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowInformationDialog(null, text, false, "", timeoutSecond, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowInformationDialog(null, text, true, detail, timeoutSecond, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region ShowInformation(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowInformationDialog(null, text, false, "", 0, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowInformationDialog(null, text, true, detail, 0, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowInformationDialog(null, text, false, "", timeoutSecond, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowInformationDialog(null, text, true, detail, timeoutSecond, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region ShowInformation(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowInformationDialog(null, text, false, "", 0, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowInformationDialog(null, text, true, detail, 0, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowInformationDialog(null, text, false, "", timeoutSecond, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowInformationDialog(null, text, true, detail, timeoutSecond, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        #endregion

        #region ShowInformation(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowInformationDialog(null, text, false, "", 0, buttons, defaultButton, true, helpFilePath, navigator, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowInformationDialog(null, text, true, detail, 0, buttons, defaultButton, true, helpFilePath, navigator, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowInformationDialog(null, text, false, "", timeoutSecond, buttons, defaultButton, true, helpFilePath, navigator, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowInformationDialog(null, text, true, detail, timeoutSecond, buttons, defaultButton, true, helpFilePath, navigator, null);
        }

        #endregion

        #region ShowInformation(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowInformationDialog(null, text, false, "", 0, buttons, defaultButton, true, helpFilePath, navigator, param);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowInformationDialog(null, text, true, detail, 0, buttons, defaultButton, true, helpFilePath, navigator, param);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowInformationDialog(null, text, false, "", timeoutSecond, buttons, defaultButton, true, helpFilePath, navigator, param);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowInformationDialog(null, text, true, detail, timeoutSecond, buttons, defaultButton, true, helpFilePath, navigator, param);
        }

        #endregion

        #region ShowInformation(IWin32Window owner, string text)

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text)
        {
            return InnerShowInformationDialog(owner, text, false, "", 0, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text, string detail)
        {
            return InnerShowInformationDialog(owner, text, true, detail, 0, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text, int timeoutSecond)
        {
            return InnerShowInformationDialog(owner, text, false, "", timeoutSecond, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text, string detail, int timeoutSecond)
        {
            return InnerShowInformationDialog(owner, text, true, detail, timeoutSecond, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region ShowInformation(IWin32Window owner, string text, MessageBoxButtons buttons)

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text, MessageBoxButtons buttons)
        {
            return InnerShowInformationDialog(owner, text, false, "", 0, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text, string detail, MessageBoxButtons buttons)
        {
            return InnerShowInformationDialog(owner, text, true, detail, 0, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text, int timeoutSecond, MessageBoxButtons buttons)
        {
            return InnerShowInformationDialog(owner, text, false, "", timeoutSecond, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text, string detail, int timeoutSecond, MessageBoxButtons buttons)
        {
            return InnerShowInformationDialog(owner, text, true, detail, timeoutSecond, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region ShowInformation(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowInformationDialog(owner, text, false, "", 0, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowInformationDialog(owner, text, true, detail, 0, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowInformationDialog(owner, text, false, "", timeoutSecond, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowInformationDialog(owner, text, true, detail, timeoutSecond, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region ShowInformation(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowInformationDialog(owner, text, false, "", 0, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowInformationDialog(owner, text, true, detail, 0, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowInformationDialog(owner, text, false, "", timeoutSecond, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowInformationDialog(owner, text, true, detail, timeoutSecond, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }
        #endregion

        #region ShowInformation(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowInformationDialog(owner, text, false, "", 0, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowInformationDialog(owner, text, true, detail, 0, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowInformationDialog(owner, text, false, "", timeoutSecond, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowInformationDialog(owner, text, true, detail, timeoutSecond, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        #endregion

        #region ShowInformation(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowInformationDialog(owner, text, false, "", 0, buttons, defaultButton, true, helpFilePath, navigator, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowInformationDialog(owner, text, true, detail, 0, buttons, defaultButton, true, helpFilePath, navigator, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowInformationDialog(owner, text, false, "", timeoutSecond, buttons, defaultButton, true, helpFilePath, navigator, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowInformationDialog(owner, text, true, detail, timeoutSecond, buttons, defaultButton, true, helpFilePath, navigator, null);
        }

        #endregion

        #region ShowInformation(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowInformationDialog(owner, text, false, "", 0, buttons, defaultButton, true, helpFilePath, navigator, param);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowInformationDialog(owner, text, true, detail, 0, buttons, defaultButton, true, helpFilePath, navigator, param);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowInformationDialog(owner, text, false, "", timeoutSecond, buttons, defaultButton, true, helpFilePath, navigator, param);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        public static DialogResult ShowInformation(IWin32Window owner, string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowInformationDialog(owner, text, true, detail, timeoutSecond, buttons, defaultButton, true, helpFilePath, navigator, param);
        }

        #endregion

        #endregion

        #region ShowQuestion Methods

        #region ShowQuestion(string text)

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text)
        {
            return InnerShowQuestionDialog(null, text, false, "", 0, MessageBoxButtons.OKCancel, MessageBoxDefaultButton.Button2, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text, string detail)
        {
            return InnerShowQuestionDialog(null, text, true, detail, 0, MessageBoxButtons.OKCancel, MessageBoxDefaultButton.Button2, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text, int timeoutSecond)
        {
            return InnerShowQuestionDialog(null, text, false, "", timeoutSecond, MessageBoxButtons.OKCancel, MessageBoxDefaultButton.Button2, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text, string detail, int timeoutSecond)
        {
            return InnerShowQuestionDialog(null, text, true, detail, timeoutSecond, MessageBoxButtons.OKCancel, MessageBoxDefaultButton.Button2, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region ShowQuestion(string text, MessageBoxButtons buttons)

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text, MessageBoxButtons buttons)
        {
            return InnerShowQuestionDialog(null, text, false, "", 0, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text, string detail, MessageBoxButtons buttons)
        {
            return InnerShowQuestionDialog(null, text, true, detail, 0, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text, int timeoutSecond, MessageBoxButtons buttons)
        {
            return InnerShowQuestionDialog(null, text, false, "", timeoutSecond, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text, string detail, int timeoutSecond, MessageBoxButtons buttons)
        {
            return InnerShowQuestionDialog(null, text, true, detail, timeoutSecond, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region ShowQuestion(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowQuestionDialog(null, text, false, "", 0, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowQuestionDialog(null, text, true, detail, 0, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowQuestionDialog(null, text, false, "", timeoutSecond, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowQuestionDialog(null, text, true, detail, timeoutSecond, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region ShowQuestion(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowQuestionDialog(null, text, false, "", 0, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowQuestionDialog(null, text, true, detail, 0, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowQuestionDialog(null, text, false, "", timeoutSecond, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowQuestionDialog(null, text, true, detail, timeoutSecond, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region ShowQuestion(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowQuestionDialog(null, text, false, "", 0, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowQuestionDialog(null, text, true, detail, 0, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowQuestionDialog(null, text, false, "", timeoutSecond, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowQuestionDialog(null, text, true, detail, timeoutSecond, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        #endregion

        #region ShowQuestion(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowQuestionDialog(null, text, false, "", 0, buttons, defaultButton, true, helpFilePath, navigator, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowQuestionDialog(null, text, true, detail, 0, buttons, defaultButton, true, helpFilePath, navigator, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowQuestionDialog(null, text, false, "", timeoutSecond, buttons, defaultButton, true, helpFilePath, navigator, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowQuestionDialog(null, text, true, detail, timeoutSecond, buttons, defaultButton, true, helpFilePath, navigator, null);
        }

        #endregion

        #region ShowQuestion(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowQuestionDialog(null, text, false, "", 0, buttons, defaultButton, true, helpFilePath, navigator, param);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowQuestionDialog(null, text, true, detail, 0, buttons, defaultButton, true, helpFilePath, navigator, param);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowQuestionDialog(null, text, false, "", timeoutSecond, buttons, defaultButton, true, helpFilePath, navigator, param);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowQuestionDialog(null, text, true, detail, timeoutSecond, buttons, defaultButton, true, helpFilePath, navigator, param);
        }

        #endregion

        #region ShowQuestion(IWin32Window owner, string text)

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text)
        {
            return InnerShowQuestionDialog(owner, text, false, "", 0, MessageBoxButtons.OKCancel, MessageBoxDefaultButton.Button2, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text, string detail)
        {
            return InnerShowQuestionDialog(owner, text, true, detail, 0, MessageBoxButtons.OKCancel, MessageBoxDefaultButton.Button2, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text, int timeoutSecond)
        {
            return InnerShowQuestionDialog(owner, text, false, "", timeoutSecond, MessageBoxButtons.OKCancel, MessageBoxDefaultButton.Button2, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text, string detail, int timeoutSecond)
        {
            return InnerShowQuestionDialog(owner, text, true, detail, timeoutSecond, MessageBoxButtons.OKCancel, MessageBoxDefaultButton.Button2, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region ShowQuestion(IWin32Window owner, string text, MessageBoxButtons buttons)

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text, MessageBoxButtons buttons)
        {
            return InnerShowQuestionDialog(owner, text, false, "", 0, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text, string detail, MessageBoxButtons buttons)
        {
            return InnerShowQuestionDialog(owner, text, true, detail, 0, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text, int timeoutSecond, MessageBoxButtons buttons)
        {
            return InnerShowQuestionDialog(owner, text, false, "", timeoutSecond, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text, string detail, int timeoutSecond, MessageBoxButtons buttons)
        {
            return InnerShowQuestionDialog(owner, text, true, detail, timeoutSecond, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region ShowQuestion(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowQuestionDialog(owner, text, false, "", 0, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowQuestionDialog(owner, text, true, detail, 0, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowQuestionDialog(owner, text, false, "", timeoutSecond, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowQuestionDialog(owner, text, true, detail, timeoutSecond, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region ShowQuestion(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowQuestionDialog(owner, text, false, "", 0, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowQuestionDialog(owner, text, true, detail, 0, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowQuestionDialog(owner, text, false, "", timeoutSecond, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowQuestionDialog(owner, text, true, detail, timeoutSecond, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }
        #endregion

        #region ShowQuestion(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowQuestionDialog(owner, text, false, "", 0, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowQuestionDialog(owner, text, true, detail, 0, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowQuestionDialog(owner, text, false, "", timeoutSecond, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowQuestionDialog(owner, text, true, detail, timeoutSecond, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        #endregion

        #region ShowQuestion(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowQuestionDialog(owner, text, false, "", 0, buttons, defaultButton, true, helpFilePath, navigator, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowQuestionDialog(owner, text, true, detail, 0, buttons, defaultButton, true, helpFilePath, navigator, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowQuestionDialog(owner, text, false, "", timeoutSecond, buttons, defaultButton, true, helpFilePath, navigator, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowQuestionDialog(owner, text, true, detail, timeoutSecond, buttons, defaultButton, true, helpFilePath, navigator, null);
        }

        #endregion

        #region ShowQuestion(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowQuestionDialog(owner, text, false, "", 0, buttons, defaultButton, true, helpFilePath, navigator, param);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowQuestionDialog(owner, text, true, detail, 0, buttons, defaultButton, true, helpFilePath, navigator, param);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowQuestionDialog(owner, text, false, "", timeoutSecond, buttons, defaultButton, true, helpFilePath, navigator, param);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowQuestion(IWin32Window owner, string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowQuestionDialog(owner, text, true, detail, timeoutSecond, buttons, defaultButton, true, helpFilePath, navigator, param);
        }

        #endregion

        #endregion

        #region ShowWarning Methods

        #region ShowWarning(string text)

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text)
        {
            return InnerShowWarningDialog(null, text, false, "", 0, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text, string detail)
        {
            return InnerShowWarningDialog(null, text, true, detail, 0, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text, int timeoutSecond)
        {
            return InnerShowWarningDialog(null, text, false, "", timeoutSecond, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text, string detail, int timeoutSecond)
        {
            return InnerShowWarningDialog(null, text, true, detail, timeoutSecond, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region ShowWarning(string text, MessageBoxButtons buttons)

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text, MessageBoxButtons buttons)
        {
            return InnerShowWarningDialog(null, text, false, "", 0, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text, string detail, MessageBoxButtons buttons)
        {
            return InnerShowWarningDialog(null, text, true, detail, 0, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text, int timeoutSecond, MessageBoxButtons buttons)
        {
            return InnerShowWarningDialog(null, text, false, "", timeoutSecond, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text, string detail, int timeoutSecond, MessageBoxButtons buttons)
        {
            return InnerShowWarningDialog(null, text, true, detail, timeoutSecond, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region ShowWarning(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowWarningDialog(null, text, false, "", 0, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowWarningDialog(null, text, true, detail, 0, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowWarningDialog(null, text, false, "", timeoutSecond, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowWarningDialog(null, text, true, detail, timeoutSecond, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region ShowWarning(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowWarningDialog(null, text, false, "", 0, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowWarningDialog(null, text, true, detail, 0, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowWarningDialog(null, text, false, "", timeoutSecond, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowWarningDialog(null, text, true, detail, timeoutSecond, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region ShowWarning(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowWarningDialog(null, text, false, "", 0, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowWarningDialog(null, text, true, detail, 0, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowWarningDialog(null, text, false, "", timeoutSecond, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowWarningDialog(null, text, true, detail, timeoutSecond, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        #endregion

        #region ShowWarning(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowWarningDialog(null, text, false, "", 0, buttons, defaultButton, true, helpFilePath, navigator, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowWarningDialog(null, text, true, detail, 0, buttons, defaultButton, true, helpFilePath, navigator, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowWarningDialog(null, text, false, "", timeoutSecond, buttons, defaultButton, true, helpFilePath, navigator, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowWarningDialog(null, text, true, detail, timeoutSecond, buttons, defaultButton, true, helpFilePath, navigator, null);
        }

        #endregion

        #region ShowWarning(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowWarningDialog(null, text, false, "", 0, buttons, defaultButton, true, helpFilePath, navigator, param);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowWarningDialog(null, text, true, detail, 0, buttons, defaultButton, true, helpFilePath, navigator, param);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowWarningDialog(null, text, false, "", timeoutSecond, buttons, defaultButton, true, helpFilePath, navigator, param);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowWarningDialog(null, text, true, detail, timeoutSecond, buttons, defaultButton, true, helpFilePath, navigator, param);
        }

        #endregion

        #region ShowWarning(IWin32Window owner, string text)

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text)
        {
            return InnerShowWarningDialog(owner, text, false, "", 0, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text, string detail)
        {
            return InnerShowWarningDialog(owner, text, true, detail, 0, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text, int timeoutSecond)
        {
            return InnerShowWarningDialog(owner, text, false, "", timeoutSecond, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text, string detail, int timeoutSecond)
        {
            return InnerShowWarningDialog(owner, text, true, detail, timeoutSecond, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region ShowWarning(IWin32Window owner, string text, MessageBoxButtons buttons)

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text, MessageBoxButtons buttons)
        {
            return InnerShowWarningDialog(owner, text, false, "", 0, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text, string detail, MessageBoxButtons buttons)
        {
            return InnerShowWarningDialog(owner, text, true, detail, 0, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text, int timeoutSecond, MessageBoxButtons buttons)
        {
            return InnerShowWarningDialog(owner, text, false, "", timeoutSecond, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text, string detail, int timeoutSecond, MessageBoxButtons buttons)
        {
            return InnerShowWarningDialog(owner, text, true, detail, timeoutSecond, buttons, MessageBoxDefaultButton.Button1, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region ShowWarning(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowWarningDialog(owner, text, false, "", 0, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowWarningDialog(owner, text, true, detail, 0, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowWarningDialog(owner, text, false, "", timeoutSecond, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return InnerShowWarningDialog(owner, text, true, detail, timeoutSecond, buttons, defaultButton, false, "", HelpNavigator.TableOfContents, null);
        }

        #endregion

        #region ShowWarning(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowWarningDialog(owner, text, false, "", 0, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowWarningDialog(owner, text, true, detail, 0, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowWarningDialog(owner, text, false, "", timeoutSecond, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath)
        {
            return InnerShowWarningDialog(owner, text, true, detail, timeoutSecond, buttons, defaultButton, true, helpFilePath, HelpNavigator.TableOfContents, null);
        }
        #endregion

        #region ShowWarning(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowWarningDialog(owner, text, false, "", 0, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowWarningDialog(owner, text, true, detail, 0, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowWarningDialog(owner, text, false, "", timeoutSecond, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプ キーワード。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, string keyword)
        {
            return InnerShowWarningDialog(owner, text, true, detail, timeoutSecond, buttons, defaultButton, true, helpFilePath, HelpNavigator.KeywordIndex, keyword);
        }

        #endregion

        #region ShowWarning(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowWarningDialog(owner, text, false, "", 0, buttons, defaultButton, true, helpFilePath, navigator, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowWarningDialog(owner, text, true, detail, 0, buttons, defaultButton, true, helpFilePath, navigator, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowWarningDialog(owner, text, false, "", timeoutSecond, buttons, defaultButton, true, helpFilePath, navigator, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator)
        {
            return InnerShowWarningDialog(owner, text, true, detail, timeoutSecond, buttons, defaultButton, true, helpFilePath, navigator, null);
        }

        #endregion

        #region ShowWarning(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowWarningDialog(owner, text, false, "", 0, buttons, defaultButton, true, helpFilePath, navigator, param);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text, string detail, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowWarningDialog(owner, text, true, detail, 0, buttons, defaultButton, true, helpFilePath, navigator, param);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowWarningDialog(owner, text, false, "", timeoutSecond, buttons, defaultButton, true, helpFilePath, navigator, param);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DialogResult ShowWarning(IWin32Window owner, string text, string detail, int timeoutSecond, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, string helpFilePath, HelpNavigator navigator, object param)
        {
            return InnerShowWarningDialog(owner, text, true, detail, timeoutSecond, buttons, defaultButton, true, helpFilePath, navigator, param);
        }

        #endregion

        #endregion

        #region Protected Internal Methods

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスの表示
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="showDetail">メッセージボックスの詳細を表示するかどうか。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="showHelpButton">メッセージボックスの[ヘルプ]ボタンを表示するかどうか。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected internal static DialogResult InnerShowDialog(IWin32Window owner, string text, string caption,
                                                                bool showDetail, string detail, int timeoutSecond,
                                                                MessageBoxButtons buttons, MessageBoxIcon icon,
                                                                MessageBoxDefaultButton defaultButton,
                                                                bool showHelpButton, string helpFilePath,
                                                                HelpNavigator navigator, object param)
        {
            DialogResult dlgResult;
            using (MessageBoxEx frm = new MessageBoxEx(owner, text, caption,
                    showDetail, detail, timeoutSecond, buttons, icon, defaultButton,
                    showHelpButton, helpFilePath, navigator, param, false, null))
            {
                dlgResult = frm.ShowDialog(owner);
            }
            return dlgResult;
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージボックスの表示、タイトルは「エラー」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="showDetail">メッセージボックスの詳細を表示するかどうか。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="showHelpButton">メッセージボックスの[ヘルプ]ボタンを表示するかどうか。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <param name="exception">例外情報</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected internal static DialogResult InnerShowErrorDialog(IWin32Window owner, string text,
                                                                        bool showDetail, string detail, 
                                                                        MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, 
                                                                        bool showHelpButton, string helpFilePath, 
                                                                        HelpNavigator navigator, object param, Exception exception)
        {
            DialogResult dlgResult;
            // 2011/02/28 K.Tsutsumi Change カタカナ禁止
            //using (MessageBoxEx frm = new MessageBoxEx(owner, text, "エラー",
            //        showDetail, detail, 0, buttons, MessageBoxIcon.Error, defaultButton,
            //        showHelpButton, helpFilePath, navigator, param, true, exception))
            using (MessageBoxEx frm = new MessageBoxEx(owner, text, Resources.CustomMsgBoxEx_Error,
                    showDetail, detail, 0, buttons, MessageBoxIcon.Error, defaultButton,
                    showHelpButton, helpFilePath, navigator, param, true, exception))
            // ↑
            {
                dlgResult = frm.ShowDialog(owner);
            }
            return dlgResult;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="showDetail">メッセージボックスの詳細を表示するかどうか。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="showHelpButton">メッセージボックスの[ヘルプ]ボタンを表示するかどうか。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected internal static DialogResult InnerShowInfomationDialog(IWin32Window owner, string text,
                                                                        bool showDetail, string detail, int timeoutSecond,
                                                                        MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton,
                                                                        bool showHelpButton, string helpFilePath,
                                                                        HelpNavigator navigator, object param)
        {
            DialogResult dlgResult;
            using (MessageBoxEx frm = new MessageBoxEx(owner, text, Resources.CustomMsgBoxEx_Information,
                    showDetail, detail, timeoutSecond, buttons, MessageBoxIcon.Information, defaultButton,
                    showHelpButton, helpFilePath, navigator, param, false, null))
            {
                dlgResult = frm.ShowDialog(owner);
            }
            return dlgResult;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 情報メッセージボックスの表示、タイトルは「情報」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="showDetail">メッセージボックスの詳細を表示するかどうか。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="showHelpButton">メッセージボックスの[ヘルプ]ボタンを表示するかどうか。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update>T.Sakiori 2009/08/17</update>
        /// --------------------------------------------------
        protected internal static DialogResult InnerShowInformationDialog(IWin32Window owner, string text,
                                                                        bool showDetail, string detail, int timeoutSecond,
                                                                        MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton,
                                                                        bool showHelpButton, string helpFilePath,
                                                                        HelpNavigator navigator, object param)
        {
            DialogResult dlgResult;
            using (MessageBoxEx frm = new MessageBoxEx(owner, text, Resources.CustomMsgBoxEx_Information,
                    showDetail, detail, timeoutSecond, buttons, MessageBoxIcon.Information, defaultButton,
                    showHelpButton, helpFilePath, navigator, param, false, null))
            {
                dlgResult = frm.ShowDialog(owner);
            }
            return dlgResult;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 確認メッセージボックスの表示、タイトルは「確認」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="showDetail">メッセージボックスの詳細を表示するかどうか。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="showHelpButton">メッセージボックスの[ヘルプ]ボタンを表示するかどうか。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected internal static DialogResult InnerShowQuestionDialog(IWin32Window owner, string text,
                                                                        bool showDetail, string detail, int timeoutSecond,
                                                                        MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton,
                                                                        bool showHelpButton, string helpFilePath,
                                                                        HelpNavigator navigator, object param)
        {
            DialogResult dlgResult;
            using (MessageBoxEx frm = new MessageBoxEx(owner, text, Resources.CustomMsgBoxEx_Confirmation,
                    showDetail, detail, timeoutSecond, buttons, MessageBoxIcon.Question, defaultButton,
                    showHelpButton, helpFilePath, navigator, param, false, null))
            {
                dlgResult = frm.ShowDialog(owner);
            }
            return dlgResult;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 警告メッセージボックスの表示、タイトルは「警告」
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="showDetail">メッセージボックスの詳細を表示するかどうか。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="showHelpButton">メッセージボックスの[ヘルプ]ボタンを表示するかどうか。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <returns>DialogResult 値の1つ。</returns>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected internal static DialogResult InnerShowWarningDialog(IWin32Window owner, string text,
                                                                        bool showDetail, string detail, int timeoutSecond,
                                                                        MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton,
                                                                        bool showHelpButton, string helpFilePath,
                                                                        HelpNavigator navigator, object param)
        {
            DialogResult dlgResult;
            using (MessageBoxEx frm = new MessageBoxEx(owner, text, Resources.CustomMsgBoxEx_Warning,
                    showDetail, detail, timeoutSecond, buttons, MessageBoxIcon.Warning, defaultButton,
                    showHelpButton, helpFilePath, navigator, param, false, null))
            {
                dlgResult = frm.ShowDialog(owner);
            }
            return dlgResult;
        }

        #endregion
    }
}
