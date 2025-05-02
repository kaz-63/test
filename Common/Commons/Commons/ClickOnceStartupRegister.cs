using System;
using System.IO;
using System.Windows.Forms;

namespace Commons
{
    /// --------------------------------------------------
    /// <summary>
    /// ClickOnceをスタートアップに登録する為のクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/06/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ClickOnceStartupRegister
    {
        /// --------------------------------------------------
        /// <summary>
        /// ClickOnceをスタートアップに登録する
        /// </summary>
        /// <create>Y.Higuchi 2010/06/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static void Register()
        {
            try
            {
                string startupPath = Environment.GetFolderPath( Environment.SpecialFolder.Startup);
                string startAppPath = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
                string clickOnceFile = Path.GetFileNameWithoutExtension( Application.ExecutablePath) + ".appref-ms";

                // スタートアップにあるファイルのフルパス
                startupPath = Path.Combine(startupPath, clickOnceFile);
                // スタートメニューにあるファイルのフルパス
                startAppPath = Path.Combine(startAppPath, Application.CompanyName);
                startAppPath = Path.Combine(startAppPath, clickOnceFile);

                if (File.Exists(startAppPath) && !File.Exists(startupPath))
                {
                    File.Copy(startAppPath, startupPath, true);
                }
            }
            catch (Exception) { }
        }

    }
}
