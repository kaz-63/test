using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using DSWUtil;
using Commons;

namespace SMSResident
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Threading.Mutex mutex;
            string appName = Application.ProductName;

            // Windows 2000 以降のみグローバル・ミューテックス利用可能
            OperatingSystem os = Environment.OSVersion;
            if ((os.Platform == PlatformID.Win32NT) && (5 <= os.Version.Major))
            {
                appName = @"Global\" + appName;
            }

            try
            {
                // ミューテックス生成
                mutex = new System.Threading.Mutex(false, appName);
            }
            catch (ApplicationException)
            {
                // グローバル・ミューテックスによる多重起動禁止
                return;
            }

            bool isRun = false;
            try
            {
                // Mutex の所有権を要求
                if (!mutex.WaitOne(0, false))
                {
#if(!DEBUG)
                    // デバッグ時は多重起動OK
                    return;
#endif
                }
                isRun = true;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
#if(!DEBUG)
                // デバッグ時は予期しない例外は発生させる。
                // 予期しない例外処理
                AppExceptionCatcher.StartAppExceptionCatch();
#endif
                // ログインユーザー情報
                UserInfo userInfo = new UserInfo();
                userInfo.UserID = " ";
                userInfo.UserName = " ";
                userInfo.PcName = DSWUtil.UtilSystem.GetUserInfo(false).MachineName;
                userInfo.IPAddress = DSWUtil.UtilNet.GetHostIPAddressString();
                userInfo.Language = ComDefine.RESIDENT_LANG;

                Application.Run(new SMSResident.Forms.ResidentForm(userInfo, ComDefine.RESIDENT_CATEGORY_ID, ComDefine.RESIDENT_MENUITEM_ID, ComDefine.RESIDENT_TITLE));
            }
            finally
            {
                if (isRun)
                {
                    mutex.ReleaseMutex();
                }
                mutex.Close();
            }
        }
    }
}
