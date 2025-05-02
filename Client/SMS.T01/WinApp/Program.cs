using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WinApp
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            DSWUtil.AppExceptionCatcher.StartAppExceptionCatch();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
