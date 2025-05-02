using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace DSWControl.DSWRichTextBox
{
    /// --------------------------------------------------
    /// <summary>
    /// リッチテキストボックス v5.0
    /// </summary>
    /// <remarks>
    /// リッチテキストボックスを最新にする改善対応
    /// https://stackoverflow.com/questions/34358088/windows-forms-richtextbox-loses-table-background-colours/34358642#34358642
    /// 
    /// 以下の問題に対処するため、バージョンアップする
    /// - Windows 8にておいてリッチテキストへの入力が極端に遅い
    /// - Windows 10においてのWordなどからの画像貼り付けができない問題を修正
    /// </remarks>
    /// <create>D.Okumura 2019/06/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class RichTextBox5 : RichTextBox
    {
        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>D.Okumura 2019/06/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public RichTextBox5()
        {
            InitializeComponent();
        }

        /// --------------------------------------------------
        /// <summary>
        /// リッチテキスト用パラメータ
        /// </summary>
        /// <create>D.Okumura 2019/06/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override CreateParams CreateParams
        {
            get
            {
                if (moduleHandle == IntPtr.Zero)
                {
                    moduleHandle = LoadLibrary("msftedit.dll");
                    if ((long)moduleHandle < 0x20)
                        throw new Win32Exception(Marshal.GetLastWin32Error(), "Could not load Msftedit.dll");
                }
                var cp = base.CreateParams;
                cp.ClassName = "RichEdit50W";
                return cp;
            }
        }
        /// --------------------------------------------------
        /// <summary>
        /// ハンドル
        /// </summary>
        /// <create>D.Okumura 2019/06/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private static IntPtr moduleHandle;

        /// --------------------------------------------------
        /// <summary>
        /// ライブラリロード
        /// </summary>
        /// <param name="lpFileName">名称</param>
        /// <returns>ハンドル</returns>
        /// <create>D.Okumura 2019/06/26</create>
        /// <update></update>
        /// --------------------------------------------------
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr LoadLibrary(string lpFileName);
    }
}
