using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using SMS;
using SystemBase.Forms;
using Commons;

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

            // スプラッシュ画面起動
            UserInfo userInfo = new UserInfo();
            DialogResult result = DialogResult.Cancel;
            using (BaseSplash frm = new CustomSplash())
            {
                result = frm.ShowDialog();
                // ログインユーザー情報
                userInfo = frm.UserInfo;
            }

            Application.Run(new Form1());
        }
    }
}
