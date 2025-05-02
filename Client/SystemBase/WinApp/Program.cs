using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using SystemBase;
using SystemBase.Forms;
using Commons;
using DSWUtil;

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
            // 多重起動禁止
            System.Threading.Mutex mutex = new System.Threading.Mutex(false, Application.ProductName);
            // Mutex の所有権を要求
            if (!mutex.WaitOne(0, false))
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

            // ログインユーザー情報
            UserInfo userInfo = null;

            // 読込みDLLの設定
            LoadDllCollection.Add("SMS.*.dll");

            // スプラッシュ画面起動
            DialogResult result = DialogResult.Cancel;
            using (BaseSplash frm = new BaseSplash())
            {
                result = frm.ShowDialog();
                // ログインユーザー情報
                userInfo = frm.UserInfo;
            }

            // 起動チェック
            if (result != DialogResult.OK || userInfo == null)
            {
                MsgBoxEx.ShowError(ComDefine.MSG_CONNECTION_ERROR);
                return;
            }

            // アプリケーション起動
            if (userInfo.SysInfo.EnabledLogin == ENABLED_LOGIN.ENABLE_VALUE1)
            {
                // ログイン起動
                Application.Run(new BaseLogin(userInfo));
            }
            else
            {
                // メニュー起動
                Application.Run(new BaseSDIMenu(userInfo));
            }
            mutex.ReleaseMutex();
        }
    }
}
