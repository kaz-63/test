using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using SystemBase;
using SystemBase.Forms;
using SystemBase.Util;
using Commons;
using DSWUtil;

namespace SMS
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {

            // カルチャの設定
            var currentCulture = System.Globalization.CultureInfo.CurrentCulture;
            // SMS.exe.configにカスタム言語設定がある場合その設定に従う
            if (!string.IsNullOrEmpty(Properties.Settings.Default.CustomCulture))
                currentCulture = System.Globalization.CultureInfo.GetCultureInfo(Properties.Settings.Default.CustomCulture);
            var uiCulture = ComFunc.GetCorrectedCultureInfo(currentCulture);
            ComFunc.SetThreadCultureInfo(uiCulture);

            // 多重起動禁止
            if (ComFunc.IsAlreadyStarting())
            {
#if(!DEBUG)
                // デバッグ時は多重起動OK
                return;
#endif
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#if(!DEBUG)
            // デバッグ時は予期しない例外は発生させる。
            // 予期しない例外処理
            AppExceptionCatcher.StartAppExceptionCatch();
#endif
            //// ハンディActiveXの登録
            //string sysPath = Environment.GetFolderPath(Environment.SpecialFolder.System);
            //string dllPath = System.IO.Path.Combine(sysPath, "CommLibX.dll");
            //if (!System.IO.File.Exists(dllPath))
            //{
            //    string curPath = System.IO.Path.Combine(Application.StartupPath, "CommLibX.dll");
            //    try
            //    {
            //        System.IO.File.Copy(curPath, dllPath);
            //    }
            //    catch { }
            //}
            //try
            //{
            //    System.Diagnostics.Process.Start("regsvr32", "/s " + dllPath);
            //}
            //catch { }

            // ログインユーザー情報
            UserInfo userInfo = null;

            // 読込みDLLの設定
            LoadDllCollection.Add("SMS.*.dll");

            // スプラッシュ画面起動
            DialogResult result = DialogResult.Cancel;
            using (BaseSplash frm = new CustomSplash())
            {
                frm.CultureInfo = uiCulture;
                result = frm.ShowDialog();
                // ログインユーザー情報
                userInfo = frm.UserInfo;
            }

            // 起動チェック
            if (result != DialogResult.OK || userInfo == null)
            {
                CustomMsgBoxEx.ShowError(ComDefine.MSG_CONNECTION_ERROR);
                return;
            }

            //Application.Run(new Form1(userInfo, "", "", ""));
            // アプリケーション起動
            if (userInfo.SysInfo.EnabledLogin == ENABLED_LOGIN.ENABLE_VALUE1)
            {
                // ログイン起動
                //Application.Run(new Form1(userInfo, "", "", ""));
                Application.Run(new BaseLogin(userInfo));
            }
            else
            {
                // メニュー起動
                Application.Run(new BaseSDIMenu(userInfo));
            }
        }
    }
}
