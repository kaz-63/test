using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Commons;
using DSWUtil;
using WsConnection;
using WsConnection.WebRefCommon;

namespace SystemBase
{
    /// --------------------------------------------------
    /// <summary>
    /// ベースメソッドクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/07/01</create>
    /// <update></update>
    /// --------------------------------------------------
    public class BaseFunc
    {
        #region パスワード有効期限

        /// --------------------------------------------------
        /// <summary>
        /// パスワード有効期限チェック
        /// </summary>
        /// <param name="userInfo">ユーザー情報</param>
        /// <param name="errorMsgID">エラーメッセージID</param>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool CheckUserPassExpiration(UserInfo userInfo, ref string errorMsgID)
        {
            try
            {
                if (userInfo.SysInfo.PasswordExpiration != PASSWORD_EXPIRATION.ENABLE_VALUE1) return true;

                // 有効期限を求める
                DateTime changeDate = userInfo.PasswordChangeDate;
                string expirationFlag = userInfo.SysInfo.ExpirationFlag;
                int expiration = userInfo.SysInfo.Expiration;
                DateTime addDate = CalcUserPassExpiration(changeDate, expirationFlag, expiration);
                ConnCommon conn = new ConnCommon();
                DateTime nowDate = conn.GetNowDateTime();
                if (addDate.Date <= nowDate.Date)
                {
                    // パスワードの有効期限が切れています。\r\n管理者に連絡してください。
                    errorMsgID = "FW010010004";
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// パスワード有効期限計算
        /// </summary>
        /// <param name="changeDate">パスワード変更日</param>
        /// <param name="expirationFlag">パスワード有効期限フラグ</param>
        /// <param name="expiration">パスワード有効期限</param>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DateTime CalcUserPassExpiration(DateTime changeDate, string expirationFlag, int expiration)
        {
            try
            {
                DateTime ret = changeDate;

                if (expirationFlag == EXPIRATION_FLAG.YEAR_VALUE1)
                {
                    ret = changeDate.AddYears(expiration);
                }
                else if (expirationFlag == EXPIRATION_FLAG.MONTH_VALUE1)
                {
                    ret = changeDate.AddMonths(expiration);
                }
                else if (expirationFlag == EXPIRATION_FLAG.DAY_VALUE1)
                {
                    ret = changeDate.AddDays(expiration);
                }

                return ret;
            }
            catch (Exception)
            {
                return changeDate;
            }
        }

        #endregion

        #region パスワード有効期限

        /// --------------------------------------------------
        /// <summary>
        /// パスワード有効期限警告チェック
        /// </summary>
        /// <param name="userInfo">ユーザー情報</param>
        /// <param name="errorMsgID">エラーメッセージID</param>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool CheckUserPassExpirationWarning(UserInfo userInfo, ref string errorMsgID,ref int days)
        {
            try
            {
                if (userInfo.SysInfo.EnabledLogin != ENABLED_LOGIN.ENABLE_VALUE1 ||
                        userInfo.SysInfo.PasswordExpiration != PASSWORD_EXPIRATION.ENABLE_VALUE1)
                {
                    return true;
                }

                // 有効期限を求める
                DateTime changeDate = userInfo.PasswordChangeDate;
                string expirationFlag = userInfo.SysInfo.ExpirationFlag;
                int expiration = userInfo.SysInfo.Expiration;
                DateTime expirationDate = CalcUserPassExpiration(changeDate, expirationFlag, expiration);
                // 警告期限を求める
                string expirationWarningFlag = userInfo.SysInfo.ExpirationWarningFlag;
                int expirationWarning = userInfo.SysInfo.ExpirationWarning;
                DateTime addDate = CalcUserPassExpirationWarning(expirationDate, expirationWarningFlag, expirationWarning);
                ConnCommon conn = new ConnCommon();
                DateTime nowDate = conn.GetNowDateTime();
                if (addDate.Date <= nowDate.Date)
                {
                    // パスワードの有効期限が残り{0}日です。パスワードを変更してください。
                    errorMsgID = "FW010010005";
                    TimeSpan diff = expirationDate.Date - nowDate.Date;
                    days = diff.Days;
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// パスワード有効期限警告計算
        /// </summary>
        /// <param name="expirationDate">有効期限日</param>
        /// <param name="expirationWarningFlag">パスワード期限警告フラグ</param>
        /// <param name="expirationWarning">パスワード期限警告期間</param>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DateTime CalcUserPassExpirationWarning(DateTime expirationDate, string expirationWarningFlag, int expirationWarning)
        {
            try
            {
                DateTime ret = expirationDate;

                if (expirationWarningFlag == EXPIRATION_WARNING_FLAG.YEAR_VALUE1)
                {
                    ret = expirationDate.AddYears(expirationWarning * -1);
                }
                else if (expirationWarningFlag == EXPIRATION_WARNING_FLAG.MONTH_VALUE1)
                {
                    ret = expirationDate.AddMonths(expirationWarning * -1);
                }
                else if (expirationWarningFlag == EXPIRATION_WARNING_FLAG.DAY_VALUE1)
                {
                    ret = expirationDate.AddDays(expirationWarning * -1);
                }

                return ret;
            }
            catch (Exception)
            {
                return expirationDate;
            }
        }

        #endregion

        #region アプリケーション初期化情報取得

        /// --------------------------------------------------
        /// <summary>
        /// アプリケーション初期化情報取得
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update>H.Tajimi 2018/09/05 多言語対応</update>
        /// <update>D.Okumura 2019/07/17 AR進捗対応</update>
        /// --------------------------------------------------
        public static bool GetSystemInitializeData(ref UserInfo userInfo)
        {
            try
            {

                // システムパラメーター取得
                ConnCommon conn = new ConnCommon();
                var condCommon = new CondCommon(userInfo);
                DataSet ds = conn.GetSystemParameter(condCommon);

                if (ds == null || !ds.Tables.Contains(Def_M_SYSTEM_PARAMETER.Name) || ds.Tables[Def_M_SYSTEM_PARAMETER.Name].Rows.Count < 1)
                {
                    return false;
                }

                // ログインユーザー情報にPC情報を設定する。
                if (userInfo == null || userInfo.SysInfo == null)
                {
                    return false;
                }
                userInfo.PcName = UtilSystem.GetUserInfo(false).MachineName;
                userInfo.IPAddress = UtilNet.GetHostIPAddressString();

                // システムパラメーター設定
                DataTable dt = ds.Tables[Def_M_SYSTEM_PARAMETER.Name];
                userInfo.SysInfo.EnabledLogin = ComFunc.GetFld(dt, 0, Def_M_SYSTEM_PARAMETER.ENABLED_LOGIN);
                userInfo.SysInfo.TerminalRole = ComFunc.GetFld(dt, 0, Def_M_SYSTEM_PARAMETER.TERMINAL_ROLE);
                userInfo.SysInfo.TerminalGuest = ComFunc.GetFld(dt, 0, Def_M_SYSTEM_PARAMETER.TERMINAL_GUEST);
                userInfo.SysInfo.MaxUserID = ComFunc.GetFldToInt32(dt, 0, Def_M_SYSTEM_PARAMETER.MAX_USER_ID);
                userInfo.SysInfo.MaxPassword = ComFunc.GetFldToInt32(dt, 0, Def_M_SYSTEM_PARAMETER.MAX_PASSWORD);
                userInfo.SysInfo.MinPassword = ComFunc.GetFldToInt32(dt, 0, Def_M_SYSTEM_PARAMETER.MIN_PASSWORD);
                userInfo.SysInfo.MyPasswordChange = ComFunc.GetFld(dt, 0, Def_M_SYSTEM_PARAMETER.MY_PASSWORD_CHANGE);
                userInfo.SysInfo.PasswordExpiration = ComFunc.GetFld(dt, 0, Def_M_SYSTEM_PARAMETER.PASSWORD_EXPIRATION);
                userInfo.SysInfo.ExpirationFlag = ComFunc.GetFld(dt, 0, Def_M_SYSTEM_PARAMETER.EXPIRATION_FLAG);
                userInfo.SysInfo.Expiration = ComFunc.GetFldToInt32(dt, 0, Def_M_SYSTEM_PARAMETER.EXPIRATION);
                userInfo.SysInfo.ExpirationWarningFlag = ComFunc.GetFld(dt, 0, Def_M_SYSTEM_PARAMETER.EXPIRATION_WARNING_FLAG);
                userInfo.SysInfo.ExpirationWarning = ComFunc.GetFldToInt32(dt, 0, Def_M_SYSTEM_PARAMETER.EXPIRATION_WARNING);
                userInfo.SysInfo.DuplicationPastPassword = ComFunc.GetFld(dt, 0, Def_M_SYSTEM_PARAMETER.DUPLICATION_PAST_PASSWORD);
                userInfo.SysInfo.PasswordCheck = ComFunc.GetFld(dt, 0, Def_M_SYSTEM_PARAMETER.PASSWORD_CHECK);
                userInfo.SysInfo.KiwakuKonpoMoveShipFlag = ComFunc.GetFld(dt, 0, Def_M_SYSTEM_PARAMETER.KIWAKU_KONPO_MOVE_SHIP_FLAG);
                userInfo.SysInfo.SeparatorItem = ComFunc.GetFld(dt, 0, Def_M_SYSTEM_PARAMETER.SEPARATOR_ITEM, " ")[0];
                userInfo.SysInfo.SeparatorRange = ComFunc.GetFld(dt, 0, Def_M_SYSTEM_PARAMETER.SEPARATOR_RANGE, " ")[0];
                userInfo.SysInfo.calculationRate = ComFunc.GetFld(dt, 0, Def_M_SYSTEM_PARAMETER.CALCULATION_RATE);

                // ログイン機能無効時のデフォルト設定取得
                if (userInfo.SysInfo.EnabledLogin == ENABLED_LOGIN.DISABLE_VALUE1)
                {
                    userInfo.UserID = ComFunc.GetFld(dt, 0, Def_M_SYSTEM_PARAMETER.DEFAULT_USER_ID);
                    userInfo.UserName = ComFunc.GetFld(dt, 0, Def_M_USER.USER_NAME);
                    userInfo.RoleID = ComFunc.GetFld(dt, 0, Def_M_ROLE.ROLE_ID);
                    userInfo.RoleName = ComFunc.GetFld(dt, 0, Def_M_ROLE.ROLE_NAME);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

    }
}
