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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // ハンディActiveXの登録
            string sysPath = Environment.GetFolderPath(Environment.SpecialFolder.System);
            string dllPath = System.IO.Path.Combine(sysPath, "CommLibX.dll");
            if (System.IO.File.Exists(dllPath))
            {
                System.Diagnostics.Process.Start("regsvr32", "/s " + dllPath);
            }
            else
            {
                string curPath = System.IO.Path.Combine(Application.StartupPath, "CommLibX.dll");
                System.IO.File.Copy(curPath, dllPath);
                System.Diagnostics.Process.Start("regsvr32", "/s " + dllPath);
            }

            Application.Run(new Form1());
        }
    }
}
