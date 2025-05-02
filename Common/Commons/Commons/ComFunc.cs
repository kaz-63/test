using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using DSWUtil;
using System.Collections.Generic;

namespace Commons
{
    #region Enum

    /// --------------------------------------------------
    /// <summary>
    /// パスワードのエラー発生箇所
    /// </summary>
    /// <create>Y.Higuchi 2010/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public enum PasswordErrorType
    {
        /// --------------------------------------------------
        /// <summary>
        /// 対象外
        /// </summary>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        None,
        /// --------------------------------------------------
        /// <summary>
        /// 旧パスワード
        /// </summary>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        Old,
        /// --------------------------------------------------
        /// <summary>
        /// 新パスワード
        /// </summary>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        New,
        /// --------------------------------------------------
        /// <summary>
        /// 確認用パスワード
        /// </summary>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        Confirm,
    }

    #endregion

    /// --------------------------------------------------
    /// <summary>
    /// 共通メソッドクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/04/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public static class ComFunc
    {
        #region Fields

        private const string REG_EXPRESSION_ALPHA_ALL = "[A-Za-z]";
        private const string REG_EXPRESSION_NUM = "[0-9]";
        private const string REG_EXPRESSION_ALPHA_LOWER = "[a-z]";
        private const string REG_EXPRESSION_ALPHA_UPPER = "[A-Z]";
        private const string REG_EXPRESSION_SIGN = "[｡-･!-/:-@[-`{-~]";

        /// --------------------------------------------------
        /// <summary>
        /// 暗号鍵
        /// </summary>
        /// <create>Y.Higuchi 2010/04/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private static readonly byte[] _rgbKey = Encoding.UTF8.GetBytes("gB0'9e3#KpPpPWQwvRh(>DCxIw|Oq?><");

        /// --------------------------------------------------
        /// <summary>
        /// 初期ベクター
        /// </summary>
        /// <create>Y.Higuchi 2010/04/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private static readonly byte[] _rgbIV = Encoding.UTF8.GetBytes("A9b'!Hn}1^@jwQMK");

        #endregion

        #region 二重起動チェック

        /// --------------------------------------------------
        /// <summary>
        /// 二重起動チェック
        /// </summary>
        /// <returns>true:既に起動している/false:起動していない</returns>
        /// <create>Y.Higuchi 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool IsAlreadyStarting()
        {
            // ClickOnceではMutexを使用した二重起動チェックが失敗する(？)為、下記の様にプロセス名で判断する。
            string processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            return System.Diagnostics.Process.GetProcessesByName(processName).Length > 1;
        }

        #endregion

        #region バージョン取得

        /// --------------------------------------------------
        /// <summary>
        /// バージョン取得(ClickOnceの場合はClickOnceのバージョンを取得する。)
        /// </summary>
        /// <returns>バージョン</returns>
        /// <create>Y.Higuchi 2010/09/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string GetAppVersion()
        {
            try
            {
                // とりあえず製品バージョンをセット
                string versionString = System.Windows.Forms.Application.ProductVersion;
                if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                {
                    // ClickOnceでインストールされている場合はClickOnceのバージョンをセット
                    Version version = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;
                    versionString = string.Empty;
                    versionString += version.Major.ToString() + ".";
                    versionString += version.Minor.ToString() + ".";
                    versionString += version.Build.ToString() + ".";
                    versionString += version.Revision.ToString();
                }
                return versionString;
            }
            catch
            {
                return System.Windows.Forms.Application.ProductVersion;
            }
        }

        #endregion

        #region データセット値取得

        #region 文字列取得

        /// --------------------------------------------------
        /// <summary>
        /// データセットアクセス
        /// </summary>
        /// <param name="ds">データセット</param>
        /// <param name="tableName">データテーブル名</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <returns>文字列（取得出来なかった場合はString.Emptyを返す）</returns>
        /// <create>Y.Higuchi 2010/04/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string GetFld(DataSet ds, string tableName, int rowIndex, string columnName)
        {
            return GetFld(ds, tableName, rowIndex, columnName, string.Empty);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データセットアクセス
        /// </summary>
        /// <param name="ds">データセット</param>
        /// <param name="tableName">データテーブル名</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <param name="defaultValue">取得出来なかった場合の戻り値</param>
        /// <returns>文字列</returns>
        /// <create>Y.Higuchi 2010/04/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string GetFld(DataSet ds, string tableName, int rowIndex, string columnName, string defaultValue)
        {
            try
            {
                if (ds == null || !ds.Tables.Contains(tableName))
                {
                    return defaultValue;
                }
                return GetFld(ds.Tables[tableName], rowIndex, columnName, defaultValue);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// データテーブルアクセス
        /// </summary>
        /// <param name="dt">データテーブル</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <returns>文字列（取得出来なかった場合はString.Emptyを返す）</returns>
        /// <create>Y.Higuchi 2010/04/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string GetFld(DataTable dt, int rowIndex, string columnName)
        {
            return GetFld(dt, rowIndex, columnName, string.Empty);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データテーブルアクセス
        /// </summary>
        /// <param name="dt">データテーブル</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <param name="defaultValue">取得出来なかった場合の戻り値</param>
        /// <returns>文字列</returns>
        /// <create>Y.Higuchi 2010/04/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string GetFld(DataTable dt, int rowIndex, string columnName, string defaultValue)
        {
            try
            {
                if (dt == null || !dt.Columns.Contains(columnName) || dt.Rows.Count <= rowIndex)
                {
                    return defaultValue;
                }

                return GetFld(dt.Rows[rowIndex], columnName, defaultValue);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// データロウアクセス
        /// </summary>
        /// <param name="dr">データロウ</param>
        /// <param name="columnName">列名</param>
        /// <returns>文字列（取得出来なかった場合はString.Emptyを返す）</returns>
        /// <create>Y.Higuchi 2010/04/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string GetFld(DataRow dr, string columnName)
        {
            return GetFld(dr, columnName, string.Empty);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データロウアクセス
        /// </summary>
        /// <param name="dr">データロウ</param>
        /// <param name="columnName">列名</param>
        /// <param name="defaultValue">取得出来なかった場合の戻り値</param>
        /// <returns>文字列</returns>
        /// <create>Y.Higuchi 2010/04/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string GetFld(DataRow dr, string columnName, string defaultValue)
        {
            try
            {
                if (dr == null)
                {
                    return defaultValue;
                }
                return dr[columnName].ToString();
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        #endregion

        #region オブジェクト取得

        /// --------------------------------------------------
        /// <summary>
        /// データセットアクセス
        /// </summary>
        /// <param name="ds">データセット</param>
        /// <param name="tableName">データテーブル名</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <returns>オブジェクト（取得出来なかった場合はDBNull.Value）</returns>
        /// <create>Y.Higuchi 2010/04/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public static object GetFldObject(DataSet ds, string tableName, int rowIndex, string columnName)
        {
            return GetFldObject(ds, tableName, rowIndex, columnName, DBNull.Value);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データセットアクセス
        /// </summary>
        /// <param name="ds">データセット</param>
        /// <param name="tableName">データテーブル名</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <param name="defaultValue">取得出来なかった場合の戻り値</param>
        /// <returns>オブジェクト</returns>
        /// <create>Y.Higuchi 2010/04/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public static object GetFldObject(DataSet ds, string tableName, int rowIndex, string columnName, object defaultValue)
        {
            try
            {
                if (ds == null || !ds.Tables.Contains(tableName))
                {
                    return defaultValue;
                }
                return GetFldObject(ds.Tables[tableName], rowIndex, columnName, defaultValue);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// データテーブルアクセス
        /// </summary>
        /// <param name="dt">データテーブル</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <returns>オブジェクト（取得出来なかった場合はDBNull.Valueを返す）</returns>
        /// <create>Y.Higuchi 2010/04/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public static object GetFldObject(DataTable dt, int rowIndex, string columnName)
        {
            return GetFldObject(dt, rowIndex, columnName, DBNull.Value);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データテーブルアクセス
        /// </summary>
        /// <param name="dt">データテーブル</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <param name="defaultValue">取得出来なかった場合の戻り値</param>
        /// <returns>オブジェクト</returns>
        /// <create>Y.Higuchi 2010/04/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public static object GetFldObject(DataTable dt, int rowIndex, string columnName, object defaultValue)
        {
            try
            {
                if (dt == null || !dt.Columns.Contains(columnName) || dt.Rows.Count <= rowIndex)
                {
                    return defaultValue;
                }
                return GetFldObject(dt.Rows[rowIndex], columnName, defaultValue);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// データロウアクセス
        /// </summary>
        /// <param name="dr">データロウ</param>
        /// <param name="columnName">列名</param>
        /// <returns>オブジェクト（取得出来なかった場合はDBNull.Valueを返す）</returns>
        /// <create>Y.Higuchi 2010/04/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public static object GetFldObject(DataRow dr, string columnName)
        {
            return GetFldObject(dr, columnName, DBNull.Value);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データロウアクセス
        /// </summary>
        /// <param name="dr">データロウ</param>
        /// <param name="columnName">列名</param>
        /// <param name="defaultValue">取得出来なかった場合の戻り値</param>
        /// <returns>オブジェクト</returns>
        /// <create>Y.Higuchi 2010/04/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public static object GetFldObject(DataRow dr, string columnName, object defaultValue)
        {
            try
            {
                if (dr == null)
                {
                    return defaultValue;

                }
                if (dr[columnName] == DBNull.Value)
                {
                    return defaultValue;
                }
                return dr[columnName];
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        #endregion

        #region Bool取得

        /// --------------------------------------------------
        /// <summary>
        /// データセットアクセス
        /// </summary>
        /// <param name="ds">データセット</param>
        /// <param name="tableName">データテーブル名</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <returns>オブジェクト（取得出来なかった場合はfalse）</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool GetFldToBool(DataSet ds, string tableName, int rowIndex, string columnName)
        {
            return GetFldToBool(ds, tableName, rowIndex, columnName, false);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データセットアクセス
        /// </summary>
        /// <param name="ds">データセット</param>
        /// <param name="tableName">データテーブル名</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <param name="defaultValue">取得出来なかった場合の戻り値</param>
        /// <returns>オブジェクト</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool GetFldToBool(DataSet ds, string tableName, int rowIndex, string columnName, bool defaultValue)
        {
            try
            {
                if (ds == null || !ds.Tables.Contains(tableName))
                {
                    return defaultValue;
                }
                return GetFldToBool(ds.Tables[tableName], rowIndex, columnName, defaultValue);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// データテーブルアクセス
        /// </summary>
        /// <param name="dt">データテーブル</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <returns>オブジェクト（取得出来なかった場合は0を返す）</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool GetFldToBool(DataTable dt, int rowIndex, string columnName)
        {
            return GetFldToBool(dt, rowIndex, columnName, false);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データテーブルアクセス
        /// </summary>
        /// <param name="dt">データテーブル</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <param name="defaultValue">取得出来なかった場合の戻り値</param>
        /// <returns>オブジェクト</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool GetFldToBool(DataTable dt, int rowIndex, string columnName, bool defaultValue)
        {
            try
            {
                if (dt == null || !dt.Columns.Contains(columnName) || dt.Rows.Count <= rowIndex)
                {
                    return defaultValue;
                }
                return GetFldToBool(dt.Rows[rowIndex], columnName, defaultValue);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// データロウアクセス
        /// </summary>
        /// <param name="dr">データロウ</param>
        /// <param name="columnName">列名</param>
        /// <returns>オブジェクト（取得出来なかった場合はfalseを返す）</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool GetFldToBool(DataRow dr, string columnName)
        {
            return GetFldToBool(dr, columnName, false);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データロウアクセス
        /// </summary>
        /// <param name="dr">データロウ</param>
        /// <param name="columnName">列名</param>
        /// <param name="defaultValue">取得出来なかった場合の戻り値</param>
        /// <returns>オブジェクト</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool GetFldToBool(DataRow dr, string columnName, bool defaultValue)
        {
            try
            {
                if (dr == null)
                {
                    return defaultValue;
                }
                return UtilConvert.ToBoolean(dr[columnName]);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        #endregion

        #region DateTime取得

        /// --------------------------------------------------
        /// <summary>
        /// データセットアクセス
        /// </summary>
        /// <param name="ds">データセット</param>
        /// <param name="tableName">データテーブル名</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <returns>オブジェクト（取得出来なかった場合はfalse）</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DateTime GetFldToDateTime(DataSet ds, string tableName, int rowIndex, string columnName)
        {
            return GetFldToDateTime(ds, tableName, rowIndex, columnName, DateTime.MinValue);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データセットアクセス
        /// </summary>
        /// <param name="ds">データセット</param>
        /// <param name="tableName">データテーブル名</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <param name="defaultValue">取得出来なかった場合の戻り値</param>
        /// <returns>オブジェクト</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DateTime GetFldToDateTime(DataSet ds, string tableName, int rowIndex, string columnName, DateTime defaultValue)
        {
            try
            {
                if (ds == null || !ds.Tables.Contains(tableName))
                {
                    return defaultValue;
                }
                return GetFldToDateTime(ds.Tables[tableName], rowIndex, columnName, defaultValue);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// データテーブルアクセス
        /// </summary>
        /// <param name="dt">データテーブル</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <returns>オブジェクト（取得出来なかった場合は0を返す）</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DateTime GetFldToDateTime(DataTable dt, int rowIndex, string columnName)
        {
            return GetFldToDateTime(dt, rowIndex, columnName, DateTime.MinValue);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データテーブルアクセス
        /// </summary>
        /// <param name="dt">データテーブル</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <param name="defaultValue">取得出来なかった場合の戻り値</param>
        /// <returns>オブジェクト</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DateTime GetFldToDateTime(DataTable dt, int rowIndex, string columnName, DateTime defaultValue)
        {
            try
            {
                if (dt == null || !dt.Columns.Contains(columnName) || dt.Rows.Count <= rowIndex)
                {
                    return defaultValue;
                }
                return GetFldToDateTime(dt.Rows[rowIndex], columnName, defaultValue);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// データロウアクセス
        /// </summary>
        /// <param name="dr">データロウ</param>
        /// <param name="columnName">列名</param>
        /// <returns>オブジェクト（取得出来なかった場合はfalseを返す）</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DateTime GetFldToDateTime(DataRow dr, string columnName)
        {
            return GetFldToDateTime(dr, columnName, DateTime.MinValue);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データロウアクセス
        /// </summary>
        /// <param name="dr">データロウ</param>
        /// <param name="columnName">列名</param>
        /// <param name="defaultValue">取得出来なかった場合の戻り値</param>
        /// <returns>オブジェクト</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DateTime GetFldToDateTime(DataRow dr, string columnName, DateTime defaultValue)
        {
            try
            {
                if (dr == null)
                {
                    return defaultValue;
                }
                return UtilConvert.ToDateTime(dr[columnName], defaultValue);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        #endregion

        #region Int32取得

        /// --------------------------------------------------
        /// <summary>
        /// データセットアクセス
        /// </summary>
        /// <param name="ds">データセット</param>
        /// <param name="tableName">データテーブル名</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <returns>オブジェクト（取得出来なかった場合は0）</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static int GetFldToInt32(DataSet ds, string tableName, int rowIndex, string columnName)
        {
            return GetFldToInt32(ds, tableName, rowIndex, columnName, 0);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データセットアクセス
        /// </summary>
        /// <param name="ds">データセット</param>
        /// <param name="tableName">データテーブル名</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <param name="style">使用可能な書式を示す。</param>
        /// <returns>オブジェクト</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static int GetFldToInt32(DataSet ds, string tableName, int rowIndex, string columnName, NumberStyles style)
        {
            return GetFldToInt32(ds.Tables[tableName], rowIndex, columnName, style, 0);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データセットアクセス
        /// </summary>
        /// <param name="ds">データセット</param>
        /// <param name="tableName">データテーブル名</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <param name="defaultValue">取得出来なかった場合の戻り値</param>
        /// <returns>オブジェクト</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static int GetFldToInt32(DataSet ds, string tableName, int rowIndex, string columnName, int defaultValue)
        {
            return GetFldToInt32(ds.Tables[tableName], rowIndex, columnName, NumberStyles.Number, defaultValue);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データセットアクセス
        /// </summary>
        /// <param name="ds">データセット</param>
        /// <param name="tableName">データテーブル名</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <param name="style">使用可能な書式を示す。</param>
        /// <param name="defaultValue">取得出来なかった場合の戻り値</param>
        /// <returns>オブジェクト</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static int GetFldToInt32(DataSet ds, string tableName, int rowIndex, string columnName, NumberStyles style, int defaultValue)
        {
            try
            {
                if (ds == null || !ds.Tables.Contains(tableName))
                {
                    return defaultValue;
                }
                return GetFldToInt32(ds.Tables[tableName], rowIndex, columnName, style, defaultValue);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// データテーブルアクセス
        /// </summary>
        /// <param name="dt">データテーブル</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <returns>オブジェクト（取得出来なかった場合は0を返す）</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static int GetFldToInt32(DataTable dt, int rowIndex, string columnName)
        {
            return GetFldToInt32(dt, rowIndex, columnName, 0);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データテーブルアクセス
        /// </summary>
        /// <param name="dt">データテーブル</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <param name="style">使用可能な書式を示す。</param>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static int GetFldToInt32(DataTable dt, int rowIndex, string columnName, NumberStyles style)
        {
            return GetFldToInt32(dt, rowIndex, columnName, style, 0);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データテーブルアクセス
        /// </summary>
        /// <param name="dt">データテーブル</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <param name="defaultValue">取得出来なかった場合の戻り値</param>
        /// <returns>オブジェクト</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static int GetFldToInt32(DataTable dt, int rowIndex, string columnName, int defaultValue)
        {
            return GetFldToInt32(dt.Rows[rowIndex], columnName, NumberStyles.Number, defaultValue);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データテーブルアクセス
        /// </summary>
        /// <param name="dt">データテーブル</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <param name="style">使用可能な書式を示す。</param>
        /// <param name="defaultValue">取得出来なかった場合の戻り値</param>
        /// <returns>オブジェクト</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static int GetFldToInt32(DataTable dt, int rowIndex, string columnName, NumberStyles style, int defaultValue)
        {
            try
            {
                if (dt == null || !dt.Columns.Contains(columnName) || dt.Rows.Count <= rowIndex)
                {
                    return defaultValue;
                }
                return GetFldToInt32(dt.Rows[rowIndex], columnName, style, defaultValue);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// データロウアクセス
        /// </summary>
        /// <param name="dr">データロウ</param>
        /// <param name="columnName">列名</param>
        /// <returns>オブジェクト（取得出来なかった場合は0を返す）</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static int GetFldToInt32(DataRow dr, string columnName)
        {
            return GetFldToInt32(dr, columnName, 0);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データロウアクセス
        /// </summary>
        /// <param name="dr">データロウ</param>
        /// <param name="columnName">列名</param>
        /// <param name="style">使用可能な書式を示す。</param>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static int GetFldToInt32(DataRow dr, string columnName, NumberStyles style)
        {
            return GetFldToInt32(dr, columnName, style, 0);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データロウアクセス
        /// </summary>
        /// <param name="dr">データロウ</param>
        /// <param name="columnName">列名</param>
        /// <param name="defaultValue">取得出来なかった場合の戻り値</param>
        /// <returns>オブジェクト</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static int GetFldToInt32(DataRow dr, string columnName, int defaultValue)
        {
            return UtilConvert.ToInt32(dr[columnName].ToString(), NumberStyles.Number, defaultValue);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データロウアクセス
        /// </summary>
        /// <param name="dr">データロウ</param>
        /// <param name="columnName">列名</param>
        /// <param name="style">使用可能な書式を示す。</param>
        /// <param name="defaultValue">取得出来なかった場合の戻り値</param>
        /// <returns>オブジェクト</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static int GetFldToInt32(DataRow dr, string columnName, NumberStyles style, int defaultValue)
        {
            try
            {
                if (dr == null)
                {
                    return defaultValue;
                }
                return UtilConvert.ToInt32(dr[columnName].ToString(), style, defaultValue);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        #endregion

        #region Decimal取得

        /// --------------------------------------------------
        /// <summary>
        /// データセットアクセス
        /// </summary>
        /// <param name="ds">データセット</param>
        /// <param name="tableName">データテーブル名</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <returns>オブジェクト（取得出来なかった場合は0）</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static decimal GetFldToDecimal(DataSet ds, string tableName, int rowIndex, string columnName)
        {
            return GetFldToDecimal(ds, tableName, rowIndex, columnName, 0M);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データセットアクセス
        /// </summary>
        /// <param name="ds">データセット</param>
        /// <param name="tableName">データテーブル名</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <param name="style">使用可能な書式を示す。</param>
        /// <returns>オブジェクト</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static decimal GetFldToDecimal(DataSet ds, string tableName, int rowIndex, string columnName, NumberStyles style)
        {
            return GetFldToDecimal(ds.Tables[tableName], rowIndex, columnName, style, 0);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データセットアクセス
        /// </summary>
        /// <param name="ds">データセット</param>
        /// <param name="tableName">データテーブル名</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <param name="defaultValue">取得出来なかった場合の戻り値</param>
        /// <returns>オブジェクト</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static decimal GetFldToDecimal(DataSet ds, string tableName, int rowIndex, string columnName, decimal defaultValue)
        {
            return GetFldToDecimal(ds.Tables[tableName], rowIndex, columnName, NumberStyles.Number, defaultValue);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データセットアクセス
        /// </summary>
        /// <param name="ds">データセット</param>
        /// <param name="tableName">データテーブル名</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <param name="style">使用可能な書式を示す。</param>
        /// <param name="defaultValue">取得出来なかった場合の戻り値</param>
        /// <returns>オブジェクト</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static decimal GetFldToDecimal(DataSet ds, string tableName, int rowIndex, string columnName, NumberStyles style, decimal defaultValue)
        {
            try
            {
                if (ds == null || !ds.Tables.Contains(tableName))
                {
                    return defaultValue;
                }
                return GetFldToDecimal(ds.Tables[tableName], rowIndex, columnName, style, defaultValue);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// データテーブルアクセス
        /// </summary>
        /// <param name="dt">データテーブル</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <returns>オブジェクト（取得出来なかった場合は0を返す）</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static decimal GetFldToDecimal(DataTable dt, int rowIndex, string columnName)
        {
            return GetFldToDecimal(dt, rowIndex, columnName, 0M);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データテーブルアクセス
        /// </summary>
        /// <param name="dt">データテーブル</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <param name="style">使用可能な書式を示す。</param>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static decimal GetFldToDecimal(DataTable dt, int rowIndex, string columnName, NumberStyles style)
        {
            return GetFldToDecimal(dt, rowIndex, columnName, style, 0);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データテーブルアクセス
        /// </summary>
        /// <param name="dt">データテーブル</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <param name="defaultValue">取得出来なかった場合の戻り値</param>
        /// <returns>オブジェクト</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static decimal GetFldToDecimal(DataTable dt, int rowIndex, string columnName, decimal defaultValue)
        {
            return GetFldToDecimal(dt.Rows[rowIndex], columnName, NumberStyles.Number, defaultValue);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データテーブルアクセス
        /// </summary>
        /// <param name="dt">データテーブル</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnName">列名</param>
        /// <param name="style">使用可能な書式を示す。</param>
        /// <param name="defaultValue">取得出来なかった場合の戻り値</param>
        /// <returns>オブジェクト</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static decimal GetFldToDecimal(DataTable dt, int rowIndex, string columnName, NumberStyles style, decimal defaultValue)
        {
            try
            {
                if (dt == null || !dt.Columns.Contains(columnName) || dt.Rows.Count <= rowIndex)
                {
                    return defaultValue;
                }
                return GetFldToDecimal(dt.Rows[rowIndex], columnName, style, defaultValue);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// データロウアクセス
        /// </summary>
        /// <param name="dr">データロウ</param>
        /// <param name="columnName">列名</param>
        /// <returns>オブジェクト（取得出来なかった場合は0を返す）</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static decimal GetFldToDecimal(DataRow dr, string columnName)
        {
            return GetFldToDecimal(dr, columnName, 0M);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データロウアクセス
        /// </summary>
        /// <param name="dr">データロウ</param>
        /// <param name="columnName">列名</param>
        /// <param name="style">使用可能な書式を示す。</param>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static decimal GetFldToDecimal(DataRow dr, string columnName, NumberStyles style)
        {
            return GetFldToDecimal(dr, columnName, style, 0);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データロウアクセス
        /// </summary>
        /// <param name="dr">データロウ</param>
        /// <param name="columnName">列名</param>
        /// <param name="defaultValue">取得出来なかった場合の戻り値</param>
        /// <returns>オブジェクト</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static decimal GetFldToDecimal(DataRow dr, string columnName, decimal defaultValue)
        {
            return UtilConvert.ToDecimal(dr[columnName].ToString(), NumberStyles.Number, defaultValue);
        }

        /// --------------------------------------------------
        /// <summary>
        /// データロウアクセス
        /// </summary>
        /// <param name="dr">データロウ</param>
        /// <param name="columnName">列名</param>
        /// <param name="style">使用可能な書式を示す。</param>
        /// <param name="defaultValue">取得出来なかった場合の戻り値</param>
        /// <returns>オブジェクト</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static decimal GetFldToDecimal(DataRow dr, string columnName, NumberStyles style, decimal defaultValue)
        {
            try
            {
                if (dr == null)
                {
                    return defaultValue;
                }
                return UtilConvert.ToDecimal(dr[columnName].ToString(), style, defaultValue);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        #endregion

        #endregion

        #region パスワードチェック

        /// --------------------------------------------------
        /// <summary>
        /// パスワードチェック(DBにアクセスしないものだけ)
        /// </summary>
        /// <param name="oldPassword">現在のパスワード</param>
        /// <param name="newPassword">新しいパスワード</param>
        /// <param name="confirmPassword">パスワード確認入力</param>
        /// <param name="minPassword">パスワード最小バイト数</param>
        /// <param name="maxPassword">パスワード最大バイト数</param>
        /// <param name="passwordCheck">パスワードチェック</param>
        /// ><param name="isNewPassword">新規登録時のパスワード(oldPasswordを無視する。)</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">メッセージのパラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool CheckInputPassword(string oldPassword, string newPassword, string confirmPassword, int minPassword, int maxPassword, string passwordCheck, bool isNewPassword, out string errMsgID, out string[] args)
        {
            try
            {
                // ----- 初期化 -----
                errMsgID = string.Empty;
                args = null;

                // ----- 入力チェック -----
                if (!CheckInputPassword_Input(oldPassword, newPassword, confirmPassword, isNewPassword,ref errMsgID, ref args))
                {
                    return false;
                }

                // ----- パスワード最小桁チェック -----
                if (0 < minPassword)
                {
                    if (!CheckInputPassword_MinLength(minPassword, oldPassword, newPassword, confirmPassword, isNewPassword, ref errMsgID, ref args))
                    {
                        return false;
                    }
                }

                // ----- パスワード最大桁チェック -----
                if (!CheckInputPassword_MaxLength(maxPassword, oldPassword, newPassword, confirmPassword, isNewPassword, ref errMsgID, ref args))
                {
                    return false;
                }

                // ----- 確認入力チェック -----
                if (newPassword != confirmPassword)
                {
                    // 確認のパスワードが間違っていました。パスワードと確認入力が一致するようにしてください。
                    errMsgID = "FW010010006";
                    return false;
                }

                // ----- パスワードルールチェック -----
                if (!CheckInputPassword_Detail(passwordCheck, newPassword, ref errMsgID, ref args))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <param name="oldPassword">現在のパスワード</param>
        /// <param name="newPassword">新しいパスワード</param>
        /// <param name="confirmPassword">パスワード確認入力</param>
        /// ><param name="isNewPassword">新規登録時のパスワード(oldPasswordを無視する。)</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">メッセージのパラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private static bool CheckInputPassword_Input(string oldPassword, string newPassword, string confirmPassword, bool isNewPassword, ref string errMsgID, ref string[] args)
        {
            if (!isNewPassword)
            {
                // 現在のパスワード入力チェック
                if (string.IsNullOrEmpty(oldPassword))
                {
                    // 現在のパスワードが入力されていません。
                    errMsgID = "FW010010007";
                    return false;
                }
            }
            // 新しいパスワード入力チェック
            if (string.IsNullOrEmpty(newPassword))
            {
                // 新しいパスワードが入力されていません。
                errMsgID = "FW010010008";
                return false;
            }
            // パスワード確認入力入力チェック
            if (string.IsNullOrEmpty(confirmPassword))
            {
                // パスワード確認入力が入力されていません。
                errMsgID = "FW010010009";
                return false;
            }
            return true;
        }

        #endregion

        #region パスワード最小桁チェック

        /// --------------------------------------------------
        /// <summary>
        /// パスワード最小桁チェック
        /// </summary>
        /// <param name="minLength">パスワード最小バイト数</param>
        /// <param name="oldPassword">現在のパスワード</param>
        /// <param name="newPassword">新しいパスワード</param>
        /// <param name="confirmPassword">パスワード確認入力</param>
        /// ><param name="isNewPassword">新規登録時のパスワード(oldPasswordを無視する。)</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">メッセージのパラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private static bool CheckInputPassword_MinLength(int minLength, string oldPassword, string newPassword, string confirmPassword, bool isNewPassword, ref string errMsgID, ref string[] args)
        {
            // 新しいパスワード入力チェック
            if (UtilString.GetByteCount(newPassword) < minLength)
            {
                // 新しいパスワードに{0}桁以上入力してください。
                errMsgID = "FW010010010";
                args = new string[] { minLength.ToString() };
                return false;
            }
            return true;
        }

        #endregion

        #region パスワード最大桁チェック

        /// --------------------------------------------------
        /// <summary>
        /// パスワード最大桁チェック
        /// </summary>
        /// <param name="maxLength">パスワード最大バイト数</param>
        /// <param name="oldPassword">現在のパスワード</param>
        /// <param name="newPassword">新しいパスワード</param>
        /// <param name="confirmPassword">パスワード確認入力</param>
        /// ><param name="isNewPassword">新規登録時のパスワード(oldPasswordを無視する。)</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">メッセージのパラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private static bool CheckInputPassword_MaxLength(int maxLength, string oldPassword, string newPassword, string confirmPassword, bool isNewPassword, ref string errMsgID, ref string[] args)
        {
            if (!isNewPassword)
            {
                // 現在のパスワード入力チェック
                if (maxLength < UtilString.GetByteCount(oldPassword))
                {
                    // 現在のパスワードは{0}桁以下にしてください。
                    errMsgID = "FW010010011";
                    args = new string[] { maxLength.ToString() };
                    return false;
                }
            }
            // 新しいパスワード入力チェック
            if (maxLength < UtilString.GetByteCount(newPassword))
            {
                // 新しいパスワードは{0}桁以下にしてください。
                errMsgID = "FW010010012";
                args = new string[] { maxLength.ToString() };
                return false;
            }
            return true;
        }

        #endregion

        #region パスワードルールチェック

        /// --------------------------------------------------
        /// <summary>
        /// パスワードルールチェック
        /// </summary>
        /// <param name="passwordCheck">パスワードチェック</param>
        /// <param name="newPassword">新しいパスワード</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">メッセージのパラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private static bool CheckInputPassword_Detail(string passwordCheck, string newPassword, ref string errMsgID, ref string[] args)
        {
            if (passwordCheck == PASSWORD_CHECK.ALPHANUM_VALUE1)
            {
                // ----- 英数チェック -----
                int check = 0;
                // 英字チェック
                if (Regex.IsMatch(newPassword, REG_EXPRESSION_ALPHA_ALL)) check++;
                // 数字チェック
                if (Regex.IsMatch(newPassword, REG_EXPRESSION_NUM)) check++;
                if (check < 2)
                {
                    // アルファベットと数字を使用したパスワードを入力してください。
                    errMsgID = "FW010010013";
                    return false;
                }
            }
            else if (passwordCheck == PASSWORD_CHECK.ALPHANUMSIGN_VALUE1)
            {
                // ----- 英数記号 -----
                int check = 0;
                // 英字(小文字)チェック
                if (Regex.IsMatch(newPassword, REG_EXPRESSION_ALPHA_LOWER)) check++;
                // 英字(大文字)チェック
                if (Regex.IsMatch(newPassword, REG_EXPRESSION_ALPHA_UPPER)) check++;
                // 数字チェック
                if (Regex.IsMatch(newPassword, REG_EXPRESSION_NUM)) check++;
                // 記号チェック
                if (Regex.IsMatch(newPassword, REG_EXPRESSION_SIGN)) check++;
                if (check < 3)
                {
                    // 英小文字、英大文字、数字、記号のうち３種類を使用してパスワードを入力してください。
                    errMsgID = "FW010010014";
                    return false;
                }
            }
            // 上記以外はとりあえずOK
            return true;
        }

        #endregion

        #region エラー発生箇所

        /// --------------------------------------------------
        /// <summary>
        /// エラー発生箇所
        /// </summary>
        /// <param name="msgID">メッセージID</param>
        /// <returns>パスワードのエラー発生箇所</returns>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public static PasswordErrorType GetPasswordErrorType(string msgID)
        {
            PasswordErrorType ret = PasswordErrorType.None;
            switch (msgID)
            {
                case "FW010010007":
                case "FW010010011":
                case "FW010010015":
                    ret = PasswordErrorType.Old;
                    break;
                case "FW010010008":
                case "FW010010010":
                case "FW010010012":
                case "FW010010013":
                case "FW010010014":
                case "FW010010016":
                    ret = PasswordErrorType.New;
                    break;
                case "FW010010006":
                case "FW010010009":
                    ret = PasswordErrorType.Confirm;
                    break;
            }
            return ret;
        }

        #endregion

        #endregion

        #region データセットのチェック

        /// --------------------------------------------------
        /// <summary>
        /// DataSet内に指定テーブルが存在するかどうか
        /// </summary>
        /// <param name="ds">指定DataSet</param>
        /// <param name="tableName">データテーブル名</param>
        /// <returns>true:テーブルがある/false:テーブルが無い</returns>
        /// <create>Y.Higuchi 2010/07/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool IsExistsTable(DataSet ds, string tableName)
        {
            if (ds == null || !ds.Tables.Contains(tableName))
            {
                return false;
            }
            return true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// DataSet内の指定テーブルにデータが存在するかどうか
        /// </summary>
        /// <param name="ds">対象DataSet</param>
        /// <param name="tableName">データテーブル名</param>
        /// <returns>true:データがある/false:データが無い</returns>
        /// <create>Y.Higuchi 2010/07/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool IsExistsData(DataSet ds, string tableName)
        {
            if (ds == null || !ds.Tables.Contains(tableName) || ds.Tables[tableName].Rows.Count < 1)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region AR No.からリスト区分を取得

        /// --------------------------------------------------
        /// <summary>
        /// AR No.からリスト区分を取得
        /// </summary>
        /// <param name="arNo">AR No.</param>
        /// <returns>リスト区分</returns>
        /// <create>Y.Higuchi 2010/07/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string GetListFlag(string arNo)
        {
            try
            {
                return UtilString.Substring(arNo, 2, 1);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        #endregion

        #region AR No.の範囲指定をチェック
        /// --------------------------------------------------
        /// <summary>
        /// AR No.の範囲指定をチェック
        /// </summary>
        /// <param name="arNos">AR No.テキストボックスの文字列('-'も含めて全て)</param>
        /// <param name="separatorRange">範囲を表す分割文字("-"とか)</param>
        /// <returns>チェック結果(true:正常(string.emptyの場合も含む) false:異常)</returns>
        /// <create>Y.Nakasato 2019/07/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool CheckARNo(string arNos, char separatorRange)
        {
            try
            {
                // 文字列なしなら正常
                if (string.IsNullOrEmpty(arNos))
                {
                    return true;
                }

                bool res = true;
                // セパレータで分割してみる
                string[] arnos = arNos.Split(separatorRange);
                int[] iarrnos = new int[]{};
                // ２個を超える場合は異常
                if (arnos.Length > 2)
                {
                    res = false;
                }
                else
                {
                    for (int i = 0; i < arnos.Length ; i++)
                    {
                        Array.Resize(ref iarrnos, iarrnos.Length + 1);
                        if ((!int.TryParse(arnos[i], out iarrnos[i]))  // 数値ではない
                         || (arnos[i].Length != 4))                    // ４桁ではない
                        {
                            res = false;
                            break;
                        }
                    }
                    if (arnos.Length == 2)
                    {
                        // 大小比較
                        if (iarrnos[0] > iarrnos[1])
                        {
                            res = false;
                        }
                    }
                }

                return res;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region 複数メッセージ用

        #region 複数メッセージテーブル取得

        /// --------------------------------------------------
        /// <summary>
        /// 複数メッセージテーブル取得
        /// </summary>
        /// <returns>DataTable</returns>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DataTable GetSchemeMultiMessage()
        {
            DataTable dt = new DataTable(ComDefine.DTTBL_MULTIMESSAGE);
            dt.Columns.Add(Def_M_MESSAGE.MESSAGE_ID, typeof(string));
            dt.Columns.Add(ComDefine.FLD_MESSAGE_PARAMETER, typeof(string[]));
            return dt;
        }

        #endregion

        #region 複数メッセージテーブル追加

        /// --------------------------------------------------
        /// <summary>
        /// エラーテーブルにメッセージ内容を追加
        /// </summary>
        /// <param name="dtMessage">エラーテーブル</param>
        /// <param name="messageID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public static void AddMultiMessage(DataTable dtMessage, string messageID, params string[] args)
        {
            DataRow dr = dtMessage.NewRow();
            dr[Def_M_MESSAGE.MESSAGE_ID] = messageID;
            if (args == null)
            {
                dr[ComDefine.FLD_MESSAGE_PARAMETER] = new string[] { };
            }
            else
            {
                dr[ComDefine.FLD_MESSAGE_PARAMETER] = args;
            }
            dtMessage.Rows.Add(dr);
        }

        #endregion

        #region 複数メッセージテーブルのメッセージIDでのグループ化

        /// --------------------------------------------------
        /// <summary>
        /// 複数メッセージテーブルのメッセージIDでのグループ化
        /// </summary>
        /// <param name="dtMessage">対象複数メッセージテーブル</param>
        /// <returns>グループ化したDataTable</returns>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DataTable GetDistinctMessageID(DataTable dtMessage)
        {
            return dtMessage.DefaultView.ToTable(ComDefine.DTTBL_MULTIMESSAGE, true, Def_M_MESSAGE.MESSAGE_ID);
        }

        #endregion

        #endregion

        #region バージョンチェックエラーかどうか判定

        /// --------------------------------------------------
        /// <summary>
        /// バージョンエラーのメッセージIDかどうか判定(バージョンエラーであればtrue)
        /// </summary>
        /// <param name="messageID">メッセージID</param>
        /// <returns>true:バージョンエラー/false:バージョンエラー以外</returns>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool IsVersionError(string messageID)
        {
            string[] errIDs = new string[] { "S0100020020", 
                                            "S0100020021", 
                                            "K0400010002", 
                                            "K0300010006", 
                                            "K0300020006", 
                                            "A9999999026", 
                                            "A9999999027", 
                                            "S0100020028" };

            if (-1 < Array.IndexOf<string>(errIDs, messageID))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region 現品TagNo(10桁)と出荷区分、納入先コード、TagNoの変換

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分、納入先コード、TagNoから現品TagNoを取得
        /// </summary>
        /// <param name="shukkaFlag">出荷区分</param>
        /// <param name="nonyusakiCD">納入先コード</param>
        /// <param name="tagNo">TagNo</param>
        /// <returns>現品TagNo</returns>
        /// <create>Y.Higuchi 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string GetGenpinTagNo(string shukkaFlag, string nonyusakiCD, string tagNo)
        {
            string ret = string.Empty;
            ret += shukkaFlag;
            ret += nonyusakiCD.PadLeft(4, '0');
            ret += tagNo;
            return ret;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 現品TagNoから出荷区分、納入先コード、TagNoを取得
        /// </summary>
        /// <param name="genpinTagNo">現品TagNo</param>
        /// <param name="shukkaFlag">出荷区分</param>
        /// <param name="nonyusakiCD">納入先コード</param>
        /// <param name="tagNo">TagNo</param>
        /// <create>Y.Higuchi 2010/08/03</create>
        /// <update>K.Tsutsumi 2015/09/07 Change 納入先コード36進化</update>
        /// --------------------------------------------------
        public static void AnalyzeGenpinTagNo(string genpinTagNo, out string shukkaFlag, out string nonyusakiCD, out string tagNo)
        {
            shukkaFlag = UtilString.Substring(genpinTagNo, 0, 1);
            nonyusakiCD = UtilString.Substring(genpinTagNo, 1, 4);
            tagNo = UtilString.Substring(genpinTagNo, 5);
        }

        #endregion

        #region Bitmap→Icon

        /// --------------------------------------------------
        /// <summary>
        /// BitmapをIconに変換(一応用意している。)
        /// </summary>
        /// <param name="bmp">Bitmapクラス</param>
        /// <returns>Icon</returns>
        /// <create>Y.Higuchi 2010/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static Icon BitmapToIcon(Bitmap bmp)
        {
            return Icon.FromHandle(bmp.GetHicon());
        }

        #endregion
        
        #region 複合化

        /// --------------------------------------------------
        /// <summary>
        /// 暗号化された文字列を複合化する。
        /// </summary>
        /// <param name="decryptingString">暗号化文字列</param>
        /// <returns>複合化文字列</returns>
        /// <create>Y.Higuchi 2010/04/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string DecryptByAes(string decryptingString)
        {
            try
            {
                string decryptedString = string.Empty;
                byte[] decryptingBytes = Convert.FromBase64String(decryptingString);
                RijndaelManaged aes = new RijndaelManaged();
                using (MemoryStream memStrm = new MemoryStream())
                using (CryptoStream cryptoStrm = new CryptoStream(memStrm, aes.CreateDecryptor(_rgbKey, _rgbIV), CryptoStreamMode.Write))
                {
                    cryptoStrm.Write(decryptingBytes, 0, decryptingBytes.Length);
                    cryptoStrm.FlushFinalBlock();
                    byte[] decriptedBytes = memStrm.ToArray();
                    decryptedString = Encoding.UTF8.GetString(decriptedBytes);
                }
                return decryptedString;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        #endregion

        #region Windows認証情報

        /// --------------------------------------------------
        /// <summary>
        /// Windows認証情報
        /// </summary>
        /// <param name="domain">ドメイン</param>
        /// <param name="userName">ユーザー名</param>
        /// <param name="password">パスワード</param>
        /// <create>Y.Higuchi 2010/09/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static void GetClientCredential(out string domain, out string userName, out string password)
        {
            try
            {
                domain = string.Empty;
                userName = string.Empty;
                password = string.Empty;

                domain = ComFunc.DecryptByAes(ComResource.Domain);
                userName = ComFunc.DecryptByAes(ComResource.UserName);
                password = ComFunc.DecryptByAes(ComResource.Password);
            }
            catch
            {
                domain = string.Empty;
                userName = string.Empty;
                password = string.Empty;
            }
        }

        #endregion

        #region 文字列分割（多言語簡易対応）
        /// --------------------------------------------------
        /// <summary>
        /// 指定したバイト位置できった文字列配列を取得する。
        /// 戻り値の配列の数は必ず2です。
        /// 文字情報が失われた場合は、後ろの文字列に追加されます。
        /// </summary>
        /// <param name="value">分割文字列</param>
        /// <param name="dividePos">分割位置(バイト位置)</param>
        /// <returns>分割した文字列</returns>
        /// <create>T.Sakiori 2011/03/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string[] DivideStringEx(string value, int dividePos)
        {
            Font fixFont = new Font("ＭＳ ゴシック", 9);
            Size oneByteSize = System.Windows.Forms.TextRenderer.MeasureText("0", fixFont);
            int byteCount = 0;
            string[] ret = new string[2];
            for (int i = 0; i < value.Length; i++)
            {
                Size byteSize = System.Windows.Forms.TextRenderer.MeasureText(value[i].ToString(), fixFont);
                byteCount += byteSize.Width == oneByteSize.Width ? 1 : 2;
                if (byteCount <= dividePos)
                {
                    ret[0] += value[i];
                }
                else
                {
                    ret[1] += value[i];
                }
            }
            return ret;
        }
        #endregion

        #region メール用
        #region メール用の結合したユーザー情報取得

        /// --------------------------------------------------
        /// <summary>
        /// メール用の結合したユーザー情報取得
        /// </summary>
        /// <param name="dt">物件メール明細</param>
        /// <param name="mailAddressFlag">抽出するメールフラグ</param>
        /// <param name="field">抽出する項目</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2017/09/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string GetMailUser(DataTable dt, string mailAddressFlag, string field)
        {
            return string.Join(", ", dt.AsEnumerable().Where(x => x.Field<string>(Def_M_BUKKEN_MAIL_MEISAI.MAIL_ADDRESS_FLAG) == mailAddressFlag).Select(x => x.Field<string>(field)).ToArray());
        }

        /// --------------------------------------------------
        /// <summary>
        /// メール用の結合したユーザー情報取得
        /// </summary>
        /// <param name="dt">指定フィールドを含むDataTable</param>
        /// <param name="field">抽出する項目</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string GetMailUser(DataTable dt, string field)
        {
            return string.Join(", ", dt.AsEnumerable().Select(x => x.Field<string>(field)).ToArray());
        }

        #endregion

        #region メールデータスキーマ作成
        /// --------------------------------------------------
        /// <summary>
        /// メールデータスキーマ作成
        /// </summary>
        /// <returns>データテーブル(T_MAIL)</returns>
        /// <create>D.Okumura 2019/07/31</create>
        /// <update>D.Okumura 2020/05/29 スキーマ共通化</update>
        /// --------------------------------------------------
        public static DataTable GetSchemeMail()
        {
            var dtMail = new DataTable(Def_T_MAIL.Name);
            dtMail.Columns.Add(Def_T_MAIL.MAIL_ID);
            dtMail.Columns.Add(Def_T_MAIL.MAIL_SEND);
            dtMail.Columns.Add(Def_T_MAIL.MAIL_SEND_DISPLAY);
            dtMail.Columns.Add(Def_T_MAIL.MAIL_TO);
            dtMail.Columns.Add(Def_T_MAIL.MAIL_TO_DISPLAY);
            dtMail.Columns.Add(Def_T_MAIL.MAIL_CC);
            dtMail.Columns.Add(Def_T_MAIL.MAIL_CC_DISPLAY);
            dtMail.Columns.Add(Def_T_MAIL.MAIL_BCC);
            dtMail.Columns.Add(Def_T_MAIL.MAIL_BCC_DISPLAY);
            dtMail.Columns.Add(Def_T_MAIL.TITLE);
            dtMail.Columns.Add(Def_T_MAIL.NAIYO);
            dtMail.Columns.Add(Def_T_MAIL.MAIL_STATUS);
            dtMail.Columns.Add(Def_T_MAIL.RETRY_COUNT);

            dtMail.Columns.Add(Def_T_MAIL.APPENDIX_FILE_PATH);
            return dtMail;
        }
        #endregion
        #endregion // メール用

        #region DBの言語列で使用する言語コードを取得

        /// --------------------------------------------------
        /// <summary>
        /// DBの言語列で使用する言語コードを取得
        /// </summary>
        /// <remarks>
        /// カルチャからDBの言語列で使用する言語コードに変換
        /// </remarks>
        /// <param name="cultureInfo">カルチャ</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string GetDBLangCode(System.Globalization.CultureInfo cultureInfo)
        {
            // Resources.DbLanguageを参照
            return Commons.Properties.Resources.ResourceManager.GetString("DbLanguage", cultureInfo);
        }

        #endregion

        #region 言語コードをカルチャに変換

        /// --------------------------------------------------
        /// <summary>
        /// 指定言語コードからカルチャを取得
        /// </summary>
        /// <param name="lang">言語コード(JP、USなど)</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        public static System.Globalization.CultureInfo ConvertLangToCultureInfo(string lang)
        {
            var region = new System.Globalization.RegionInfo(lang);
            foreach (var cultureInfo in System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.SpecificCultures))
            {
                var tmpRegion = new System.Globalization.RegionInfo(cultureInfo.Name);
                if (tmpRegion.GeoId == region.GeoId)
                {
                    return cultureInfo;
                }
            }

            // 変換できなかった時は仕方ないので英語を返却
            return new System.Globalization.CultureInfo("en-US");
        }

        #endregion

        #region 補正後のカルチャを取得

        /// --------------------------------------------------
        /// <summary>
        /// 補正後のカルチャを取得
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/09/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static System.Globalization.CultureInfo GetCorrectedCultureInfo(System.Globalization.CultureInfo cultureInfo)
        {
            switch (cultureInfo.TwoLetterISOLanguageName)
            {
                case "ja":
                case "zh":
                case "ko":
                    // 日本語、中国語(簡体字、繁体字、台湾)、韓国語は日本語に丸める
                    return System.Globalization.CultureInfo.GetCultureInfo("ja-JP");
                default:
                    return System.Globalization.CultureInfo.GetCultureInfo("en-US");
            }
        }

        #endregion

        #region 現在のスレッドにカルチャ情報を設定する
        /// --------------------------------------------------
        /// <summary>
        /// 現在のスレッドにカルチャ情報を設定する
        /// </summary>
        /// <param name="userInfo">画面情報</param>
        /// <create>D.Okumura 2018/09/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static void SetThreadCultureInfo(UserInfo userInfo)
        {
            SetThreadCultureInfo(userInfo.CultureInfo);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 現在のスレッドにカルチャ情報を設定する
        /// </summary>
        /// <param name="cultureInfo">カルチャ情報</param>
        /// <create>D.Okumura 2018/09/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static void SetThreadCultureInfo(System.Globalization.CultureInfo cultureInfo)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
            //System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
        }
        #endregion

        #region PDM作業名から図面型式あいまい検索用へ変換

        /// --------------------------------------------------
        /// <summary>
        /// PDM作業名から図面型式あいまい検索用へ変換
        /// ※大文字・全角化(注意喚起のためにObsolete属性付加しているだけで削除予定なし)
        /// </summary>
        /// <param name="pdmWorkName">PDM作業名</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/11/27</create>
        /// <update></update>
        /// --------------------------------------------------
        [ObsoleteAttribute("このメソッドはサーバー上で動作する場合のみ使用可です(常駐などのプロセス、WebServiceで動作するDB層)", false)]
        public static string ConvertPDMWorkNameToZumenKeishikiS(string pdmWorkName)
        {
            return UtilConvert.StrConv(pdmWorkName, (Conversion.Wide | Conversion.Uppercase));
        }

        #endregion

        #region 技連No取得(補正後)

        /// --------------------------------------------------
        /// <summary>
        /// 技連No取得(補正後)
        /// </summary>
        /// <param name="dr">DataRow</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/11/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string GetCorrectedEcsNo(DataRow dr)
        {
            var ret = GetFld(dr, Def_T_TEHAI_SKS_WORK.ECS_NO);
            if (string.IsNullOrEmpty(ret))
            {
                return ret;
            }
            else
            {
                if (ret.Length < 8)
                {
                    return ret;
                }
                else
                {
                    return ret.Substring(0, 7);
                }
            }
        }

        #endregion

        #region 指定ファイルの中身を取得

        /// --------------------------------------------------
        /// <summary>
        /// 指定ファイルの中身を取得
        /// </summary>
        /// <param name="folderPath">フォルダパス</param>
        /// <param name="fileName">ファイル名</param>
        /// <param name="language">言語コード</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/12/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string GetFileContents(string folderPath, string fileName, string language)
        {
            var filePath = folderPath;
            if (!string.IsNullOrEmpty(language))
            {
                filePath = Path.Combine(filePath, language);
            }
            filePath = Path.Combine(filePath, fileName);
            return File.ReadAllText(filePath);
        }

        #endregion

        #region 号機リスト処理関連

        /// --------------------------------------------------
        /// <summary>
        /// 号機リスト
        /// </summary>
        /// <create>Y.Nakasato 2019/07/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public class GokiInfoList
        {
            /// --------------------------------------------------
            /// <summary>
            /// 号機
            /// </summary>
            /// <create>Y.Nakasato 2019/07/29</create>
            /// <update></update>
            /// --------------------------------------------------
            public string Goki { get; set; }
            /// --------------------------------------------------
            /// <summary>
            /// 所属機種
            /// </summary>
            /// <create>Y.Nakasato 2019/07/29</create>
            /// <update></update>
            /// --------------------------------------------------
            public string Kishu { get; set; }
            /// --------------------------------------------------
            /// <summary>
            /// 選択状態
            /// </summary>
            /// <create>Y.Nakasato 2019/07/29</create>
            /// <update></update>
            /// --------------------------------------------------
            public bool Selected { get; set; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 号機リスト生成
        /// </summary>
        /// <param name="dt">DataTable(T_AR_GOKI)</param>
        /// <param name="infoList">生成するリストオブジェクト</param>
        /// <param name="maxlen">号機文字列最大長(バイト)</param>
        /// <returns></returns>
        /// <create>Y.Nakasato 2019/07/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public static int CreateGokiInfoListFromDt(DataTable dt, List<GokiInfoList> infoList, out int maxlen)
        {
            int listNum = 0;
            maxlen = 0;

            foreach (DataRow dr in dt.Rows)
            {
                if (!string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_AR_GOKI.GOKI)))
                {
                    GokiInfoList gokiinfo = new GokiInfoList();
                    gokiinfo.Kishu = ComFunc.GetFld(dr, Def_T_AR_GOKI.KISHU);
                    gokiinfo.Goki = ComFunc.GetFld(dr, Def_T_AR_GOKI.GOKI);
                    infoList.Add(gokiinfo);
                    listNum++;
                    var len = UtilString.GetByteCount(gokiinfo.Goki);
                    if (maxlen < len)
                    {
                        maxlen = len;
                    }
                }
            }
            return listNum;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 選択号機リストと一致するアイテムを設定する
        /// </summary>
        /// <param name="infoList">編集するリストオブジェクト</param>
        /// <param name="gokiList">選択号機リスト</param>
        /// <param name="forceAdd">一致するアイテムが存在しない場合強制的にリストに追加する</param>
        /// <param name="maxlen">強制的に追加した号機の文字列最大長</param>
        /// <returns>強制的に追加した号機数</returns>
        /// <create>Y.Nakasato 2019/07/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public static int CheckGokiInfoSelected(List<GokiInfoList> infoList, string[] gokiList, bool forceAdd, out int maxlen)
        {

            int forceAddNum = 0;
            maxlen = 0;

            if (gokiList == null)
                return 0;

            List<string> gokiListRemain = gokiList.ToList();

            foreach (var goki in infoList)
            {
                if (Array.IndexOf(gokiList, goki.Goki) != -1)
                {
                    goki.Selected = true;
                    gokiListRemain.Remove(goki.Goki);
                }
            }

            // リストに合致する号機がなかった場合、強制追加
            if (forceAdd)
            {
                foreach (var goki in gokiListRemain)
                {
                    GokiInfoList gokiinfo = new GokiInfoList { };
                    gokiinfo.Kishu = null;
                    gokiinfo.Goki = goki;
                    gokiinfo.Selected = true;
                    infoList.Add(gokiinfo);
                    forceAddNum++;
                    var len = UtilString.GetByteCount(gokiinfo.Goki);
                    if (maxlen < len)
                    {
                        maxlen = len;
                    }
                }
            }

            return forceAddNum;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 号機名分割処理
        /// </summary>
        /// <param name="org"></param>
        /// <param name="name"></param>
        /// <param name="num"></param>
        /// <returns>連番付きならtrue 、それ以外ならfalse</returns>
        /// <create>Y.Nakasato 2019/07/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool GetNameAndNum(string org, out string name, out string num)
        {
            name = string.Empty;
            num = string.Empty;
            int keta = 0;
            try
            {
                if (string.IsNullOrEmpty(org))
                {
                    return false;
                }
                // 後方からパース
                for (; (org.Length != keta) && char.IsDigit(org[org.Length - keta - 1])
                      ; keta++) { }

                // 全て数値か数値なしならエラー
                if ((org.Length == keta) || (keta == 0))
                {
                    return false;
                }

                // 文字と連番で分割
                name = org.Substring(0, org.Length - keta);
                num = org.Substring(org.Length - keta, keta); ;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 号機配列(範囲セパレータ無し)を文字列(範囲セパレータ付き)に変換
        /// </summary>
        /// <param name="gokiArray"></param>
        /// <param name="separator"></param>
        /// <param name="separatorRange"></param>
        /// <returns></returns>
        /// <create>Y.Nakasato 2019/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string GokiArrayToString(string[] gokiArray, char separator, char separatorRange)
        {
            if (gokiArray == null || gokiArray.Length < 1)
                return string.Empty;
            
            // 号機文字列生成
            string fstGoki = string.Empty;
            string fstNum = string.Empty;
            string preNum = string.Empty;
            int ifstNum = -1;
            int ipreNum = -1;
            List<string> listGoki = new List<string>();

            foreach (var goki in gokiArray)
            {
                string nowGoki = string.Empty;
                string nowNum = string.Empty;
                int inowNum = -1;

                // 今回連番付き
                if ((GetNameAndNum(goki, out nowGoki, out nowNum))
                 && (int.TryParse(nowNum, out inowNum)))
                {
                    // 連番の始まり(暫定)
                    if (ifstNum == -1)
                    {
                        fstGoki = nowGoki;
                        fstNum = preNum = nowNum;
                        ifstNum = ipreNum = inowNum;
                    }
                    // 連番継続
                    else if ((fstGoki == nowGoki)
                          && (preNum.Length == nowNum.Length)
                          && ((ipreNum + 1) == inowNum))
                    {
                        preNum = nowNum;
                        ipreNum = inowNum;
                    }
                    // 連番終了
                    else
                    {
                        CreateGokiRenban(listGoki, fstGoki, fstNum, preNum, ifstNum, ipreNum, separatorRange);

                        // 連番の始まり(暫定)
                        fstGoki = nowGoki;
                        fstNum = preNum = nowNum;
                        ifstNum = ipreNum = inowNum;
                    }
                }
                // 今回連番なし
                else
                {
                    // 連番終了
                    if (ifstNum != -1)
                    {
                        CreateGokiRenban(listGoki, fstGoki, fstNum, preNum, ifstNum, ipreNum, separatorRange);
                    }
                    fstGoki = fstNum = preNum = string.Empty;
                    ifstNum = ipreNum = -1;
                    listGoki.Add(goki);
                }
            }

            // 連番終了
            if (ifstNum != -1)
            {
                CreateGokiRenban(listGoki, fstGoki, fstNum, preNum, ifstNum, ipreNum, separatorRange);
            }

            return string.Join(separator.ToString(), listGoki.ToArray());
        }

        /// --------------------------------------------------
        /// <summary>
        /// 号機文字列(範囲セパレータ付き)を号機配列(範囲セパレータ無し)に変換
        /// </summary>
        /// <param name="gokiString">号機文字列(範囲セパレータ付き)</param>
        /// <param name="separator"></param>
        /// <param name="separatorRange"></param>
        /// <returns>号機配列(範囲セパレータ無し)かnull</returns>
        /// <create>Y.Nakasato 2019/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string[] GokiStringToArray(string gokiString, char separator, char separatorRange)
        {
            if (string.IsNullOrEmpty(gokiString))
                return null;

            string[] gokiArray = gokiString.Split(separator);
            List<string> gokiList = new List<string>();
            string gokiName, startNum;
            int istartNum, iendNum;
            foreach (var goki in gokiArray)
            {
                var gokiArrayTemp = goki.Split(separatorRange);

                if (gokiArrayTemp.Length == 1)
                {
                    gokiList.Add(gokiArrayTemp[0]);
                }
                else if ((gokiArrayTemp.Length == 2)
                      && (ComFunc.GetNameAndNum(gokiArrayTemp[0], out gokiName, out startNum))
                      && (startNum.Length == gokiArrayTemp[1].Length)
                      && (int.TryParse(startNum, out istartNum))
                      && (int.TryParse(gokiArrayTemp[1], out iendNum))
                      && (istartNum <= iendNum))
                {
                    for (int i = 0; i < (iendNum - istartNum + 1); i++)
                    {
                        gokiList.Add(gokiName + (istartNum + i).ToString().PadLeft(startNum.Length, '0'));
                    }

                }
            }
            return gokiList.Count > 0 ? gokiList.ToArray() : null;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 号機連番の文字列生成
        /// </summary>
        /// <param name="listGoki"></param>
        /// <param name="fstGoki"></param>
        /// <param name="fstNum"></param>
        /// <param name="preNum"></param>
        /// <param name="ifstNum"></param>
        /// <param name="ipreNum"></param>
        /// <param name="separatorRange"></param>
        /// <create>Y.Nakasato 2019/07/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private static void CreateGokiRenban(List<string> listGoki, string fstGoki, string fstNum, string preNum, int ifstNum, int ipreNum, char separatorRange)
        {
            // 結局連番しなかった
            if (ifstNum == ipreNum)
            {
                listGoki.Add(fstGoki + fstNum);
            }
            // 連番した
            else
            {
                listGoki.Add(fstGoki + fstNum + separatorRange + preNum);
            }
        }

        #endregion

        #region RRGGBB形式からColorオブジェクト生成

        /// --------------------------------------------------
        /// <summary>
        /// RRGGBB形式の文字列からColorオブジェクトを生成する
        /// </summary>
        /// <param name="input">RGB文字列</param>
        /// <returns>色</returns>
        /// <create>D.Okumura 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public static Color? GetColorFromRgb(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            int val;
            if (!int.TryParse(input, System.Globalization.NumberStyles.HexNumber, null, out val))
                return null;
            return Color.FromArgb(val);
        }

        #endregion // RRGGBB形式からColorオブジェクト生成

        #region ファイルパス関連

        /// --------------------------------------------------
        /// <summary>
        /// 正常なファイル名を取得する
        /// </summary>
        /// <param name="input">ファイル名</param>
        /// <param name="replace">不正な文字の置換先文字列</param>
        /// <returns>色</returns>
        /// <create>D.Okumura 2020/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string GetSafeFileName(string input, string replace)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            var contains = Path.GetInvalidFileNameChars().Where(w => input.Contains(w)).ToArray();
            foreach (var invalid in contains)
            {
                input = input.Replace(invalid.ToString(), replace);
            }
            return input;
        }
        /// --------------------------------------------------
        /// <summary>
        /// ファイルを作成しても例外が発生しない安全なファイルパスを取得(一時ファイル作成あり)
        /// </summary>
        /// <param name="path">ファイルを作成するディレクトリ</param>
        /// <param name="fileName">ファイル名</param>
        /// <param name="postFileName">省略時に付与する名称(例:…)</param>
        /// <exception cref="PathTooLongException">ファイルパスが長すぎるため省略できなかった</exception>
        /// <returns>ファイルパス</returns>
        /// <remarks>
        /// ファイル名称の最長が見当たらないので、文字を削りながら例外の発生しない値を見つける。
        /// </remarks>
        /// <create>D.Okumura 2020/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string GetSafePathLength(string path, string fileName, string postFileName)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(fileName))
                return null;
            var ext = Path.GetExtension(fileName);
            var filename = Path.GetFileNameWithoutExtension(fileName);
            var startLength = filename.Length;
            var targetFilePath = "";
            for (var i = startLength; i > 1; i--)
            {
                if (i == startLength)
                {
                    targetFilePath = Path.Combine(path, fileName.Substring(0, i) + ext);
                }
                else
                {
                    targetFilePath = Path.Combine(path, fileName.Substring(0, i) + postFileName + ext);
                }
                if (File.Exists(targetFilePath))
                    return targetFilePath;
                // フォルダがない場合は作成する
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                try
                {
                    // ファイルを一時的に作成する
                    using (File.Create(targetFilePath)) { }
                    // 削除を試みてダメなら、あきらめる
                    for (int c = 0; c < 5; c++)
                    {
                        try
                        {
                            File.Delete(targetFilePath);
                            break;
                        }
                        catch
                        {
                            // 握りつぶす
                            System.Threading.Thread.Sleep(500);
                        }
                    }
                    return targetFilePath;
                }
                catch (PathTooLongException)
                {
                    if (i == startLength && postFileName.Length > 0)
                    {
                        i -= postFileName.Length;
                    }
                }
            }
            // あきらめる
            throw new PathTooLongException();
        }
        #endregion // ファイルパス関連

    }
}
