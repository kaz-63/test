using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

using Condition;
using Commons;
using DSWUtil;
using DSWUtil.DbUtil;

//// --------------------------------------------------
/// <summary>
/// 共通処理クラス（データアクセス層）
/// FW系の処理はこのクラスにまとめる
/// </summary>
/// <create>Y.Higuchi 2010/04/15</create>
/// <update></update>
/// --------------------------------------------------
public class WsCommonImpl : WsBaseImpl
{
    #region Fields

    private const string REG_EXPRESSION_ALPHA_ALL = "[A-Za-z]";
    private const string REG_EXPRESSION_NUM = "[0-9]";
    private const string REG_EXPRESSION_ALPHA_LOWER = "[a-z]";
    private const string REG_EXPRESSION_ALPHA_UPPER = "[A-Z]";
    private const string REG_EXPRESSION_SIGN = "[｡-･!-/:-@[-`{-~]";

    #endregion

    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/04/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsCommonImpl()
        : base()
    {
    }

    #endregion

    #region SELECT

    #region システムパラメーター

    /// --------------------------------------------------
    /// <summary>
    /// システムパラメーター取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>Y.Higuchi 2010/04/23</create>
    /// <update>H.Tajimi 2018/09/03 多言語対応</update>
    /// <update>H.Tajimi 2019/01/08 木枠梱包業務改善</update>
    /// <update>D.Okumura 2019/07/17 AR進捗対応</update>
    /// --------------------------------------------------
    public DataSet GetSystemParameter(DatabaseHelper dbHelper, CondCommon cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT ");
            sb.ApdL("       MSP.TERMINAL_ROLE");
            sb.ApdL("     , MSP.TERMINAL_GUEST");
            sb.ApdL("     , MSP.ENABLED_LOGIN");
            sb.ApdL("     , MSP.DEFAULT_USER_ID");
            sb.ApdL("     , MSP.MAX_USER_ID");
            sb.ApdL("     , MSP.MAX_PASSWORD");
            sb.ApdL("     , MSP.MIN_PASSWORD");
            sb.ApdL("     , MSP.MY_PASSWORD_CHANGE");
            sb.ApdL("     , MSP.PASSWORD_EXPIRATION");
            sb.ApdL("     , MSP.EXPIRATION_FLAG");
            sb.ApdL("     , MSP.EXPIRATION");
            sb.ApdL("     , MSP.EXPIRATION_WARNING_FLAG");
            sb.ApdL("     , MSP.EXPIRATION_WARNING");
            sb.ApdL("     , MSP.DUPLICATION_PAST_PASSWORD");
            sb.ApdL("     , MSP.PASSWORD_CHECK");
            sb.ApdL("     , MSP.KIWAKU_KONPO_MOVE_SHIP_FLAG");
            sb.ApdL("     , MSP.SEPARATOR_ITEM");
            sb.ApdL("     , MSP.SEPARATOR_RANGE");
            sb.ApdL("     , MSP.CALCULATION_RATE");
            sb.ApdL("     , MU.USER_NAME");
            sb.ApdL("     , MU.ROLE_ID");
            sb.ApdL("     , MR.ROLE_NAME");
            sb.ApdL("  FROM M_SYSTEM_PARAMETER MSP");
            sb.ApdL("  LEFT JOIN M_USER MU ON MSP.DEFAULT_USER_ID = MU.USER_ID");
            sb.ApdL("  LEFT JOIN M_ROLE MR ON MU.ROLE_ID = MR.ROLE_ID");
            sb.ApdN("                     AND MR.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");

            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));

            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_SYSTEM_PARAMETER.Name);

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region ログイン関係

    /// --------------------------------------------------
    /// <summary>
    /// ログインユーザー取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">取得条件</param>
    /// <param name="isAutoLogin">自動ログインかどうか(true:自動、false:手動)</param>
    /// <returns></returns>
    /// <create>Y.Higuchi 2010/04/23</create>
    /// <update>H.Tajimi 2018/09/05 多言語対応</update>
    /// --------------------------------------------------
    public DataSet GetLoginUser(DatabaseHelper dbHelper, CondCommon cond, bool isAutoLogin)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();

            sb.ApdL("SELECT ");
            sb.ApdL("        MU.USER_ID");
            sb.ApdL("       ,MU.USER_NAME");
            sb.ApdL("       ,MU.USER_FLAG");
            sb.ApdL("       ,MU.PASSWORD_CHANGE_DATE");
            sb.ApdL("       ,MR.ROLE_ID");
            sb.ApdL("       ,MR.ROLE_NAME");
            sb.ApdL("       ,MR.ROLE_FLAG");
            sb.ApdL("  FROM ");
            sb.ApdL("       M_ROLE MR");
            sb.ApdL("       LEFT JOIN M_USER MU ON MU.ROLE_ID = MR.ROLE_ID");
            sb.ApdN("                          AND MR.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  WHERE");
            sb.ApdN("        MU.USER_ID = ").ApdN(this.BindPrefix).ApdL("USER_ID");
            if (!isAutoLogin)
            {
                sb.ApdN("    AND MU.PASSWORD = ").ApdN(this.BindPrefix).ApdL("PASSWORD");
                paramCollection.Add(dbHelper.NewDbParameter("PASSWORD", cond.Password));
            }

            paramCollection.Add(dbHelper.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(dbHelper.NewDbParameter("USER_ID", cond.UserID));

            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_USER.Name);

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region メニュー取得

    /// --------------------------------------------------
    /// <summary>
    /// メニュー取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">検索条件</param>
    /// <returns>メニュー</returns>
    /// <create>Y.Higuchi 2010/04/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetMenu(DatabaseHelper dbHelper, CondCommon cond)
    {
        try
        {
            DataTable dt = new DataTable();

            if (cond.TerminalRole == TERMINAL_ROLE.ENABLE_VALUE1)
            {
                // 端末取得
                dt = GetMenuGetTerminal(dbHelper, cond, true).Tables[0];
                if (cond.TerminalGuest == TERMINAL_GUEST.ENABLE_VALUE1 && dt.Rows.Count < 1)
                {
                    // ゲスト端末が有効かつ端末が取得出来なかった場合はゲスト端末取得
                    dt = GetMenuGetTerminal(dbHelper, cond, false).Tables[0];
                }
            }
            // メニュー取得
            return this.GetMenuGetData(dbHelper, cond, dt);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 端末取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">検索条件</param>
    /// <param name="isNormal">通常端末かどうか</param>
    /// <returns></returns>
    /// <create>Y.Higuchi 2010/04/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetMenuGetTerminal(DatabaseHelper dbHelper, CondCommon cond, bool isNormal)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            string terminalFlag = TERMINAL_FLAG.GUEST_VALUE1;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("        TERMINAL_NO");
            sb.ApdL("       ,TERMINAL_NAME");
            sb.ApdL("       ,TERMINAL_PCNAME");
            sb.ApdL("       ,ROLE_ID");
            sb.ApdL("       ,TERMINAL_FLAG");
            sb.ApdL("  FROM ");
            sb.ApdL("       M_TERMINAL");
            sb.ApdL(" WHERE ");
            sb.ApdN("       TERMINAL_FLAG = ").ApdN(this.BindPrefix).ApdL("TERMINAL_FLAG");
            if (isNormal)
            {
                terminalFlag = TERMINAL_FLAG.NORMAL_VALUE1;
                sb.ApdN("   AND TERMINAL_PCNAME = ").ApdN(this.BindPrefix).ApdL("TERMINAL_PCNAME");
                // バインド変数設定
                paramCollection.Add(dbHelper.NewDbParameter("TERMINAL_PCNAME", cond.PcName));
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TERMINAL_NO");

            // バインド変数設定
            paramCollection.Add(dbHelper.NewDbParameter("TERMINAL_FLAG", terminalFlag));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_TERMINAL.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// メニュー取得実行部分
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">検索条件</param>
    /// <param name="dt">端末情報</param>
    /// <returns>メニュー</returns>
    /// <create>Y.Higuchi 2010/04/23</create>
    /// <update>H.Tajimi 2018/09/05 多言語対応</update>
    /// --------------------------------------------------
    public DataSet GetMenuGetData(DatabaseHelper dbHelper, CondCommon cond, DataTable dt)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();

            // SQL文
            sb.ApdL("SELECT MRM.MENU_CATEGORY_ID");
            sb.ApdL("     , MMC.MENU_CATEGORY_NAME");
            sb.ApdL("     , MMC.DISP_NO AS MMC_DISP_NO");
            sb.ApdL("     , MRM.MENU_ITEM_ID");
            sb.ApdL("     , MMI.MENU_ITEM_NAME");
            sb.ApdL("     , MMI.DISP_NO AS MMI_DISP_NO");
            sb.ApdL("     , MMI.DISP_TITLE");
            sb.ApdL("     , MMI.CLASS_NAME");
            sb.ApdL("     , MMI.MENU_ITEM_FLAG");
            sb.ApdL("     , MMI.MENU_ITEM_IMAGE");
            sb.ApdL("  FROM M_USER MU");
            sb.ApdL(" INNER JOIN M_ROLE MR ON MR.ROLE_ID = MU.ROLE_ID");
            sb.ApdN("                     AND MR.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL(" INNER JOIN M_ROLE_MAP MRM ON MRM.ROLE_ID = MR.ROLE_ID");
            sb.ApdL(" INNER JOIN M_MENU_ITEM MMI ON MMI.MENU_CATEGORY_ID = MRM.MENU_CATEGORY_ID");
            sb.ApdL("                           AND MMI.MENU_ITEM_ID = MRM.MENU_ITEM_ID");
            sb.ApdN("                           AND MMI.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL(" INNER JOIN M_MENU_CATEGORY MMC ON MMC.MENU_CATEGORY_ID = MMI.MENU_CATEGORY_ID");
            sb.ApdN("                               AND MMC.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdN(" WHERE MU.USER_ID = ").ApdN(this.BindPrefix).ApdL("USER_ID");
            sb.ApdN("   AND MMI.MENU_ITEM_FLAG = ").ApdN(this.BindPrefix).ApdL("MENU_ITEM_FLAG");
            if (cond.TerminalRole == TERMINAL_ROLE.ENABLE_VALUE1)
            {
                sb.ApdL("INTERSECT");
                sb.ApdL("SELECT MRM.MENU_CATEGORY_ID");
                sb.ApdL("     , MMC.MENU_CATEGORY_NAME");
                sb.ApdL("     , MMC.DISP_NO AS MMC_DISP_NO");
                sb.ApdL("     , MRM.MENU_ITEM_ID");
                sb.ApdL("     , MMI.MENU_ITEM_NAME");
                sb.ApdL("     , MMI.DISP_NO AS MMI_DISP_NO");
                sb.ApdL("     , MMI.DISP_TITLE");
                sb.ApdL("     , MMI.CLASS_NAME");
                sb.ApdL("     , MMI.MENU_ITEM_FLAG");
                sb.ApdL("     , MMI.MENU_ITEM_IMAGE");
                sb.ApdL("  FROM M_TERMINAL MT");
                sb.ApdL(" INNER JOIN M_ROLE MR ON MR.ROLE_ID = MT.ROLE_ID");
                sb.ApdL("                     AND MR.LANG = ").ApdN(this.BindPrefix).ApdL("LANG2");
                sb.ApdL(" INNER JOIN M_ROLE_MAP MRM ON MRM.ROLE_ID = MR.ROLE_ID");
                sb.ApdL(" INNER JOIN M_MENU_ITEM MMI ON MMI.MENU_CATEGORY_ID = MRM.MENU_CATEGORY_ID");
                sb.ApdL("                           AND MMI.MENU_ITEM_ID = MRM.MENU_ITEM_ID");
                sb.ApdL("                           AND MMI.LANG = ").ApdN(this.BindPrefix).ApdL("LANG2");
                sb.ApdL(" INNER JOIN M_MENU_CATEGORY MMC ON MMC.MENU_CATEGORY_ID = MMI.MENU_CATEGORY_ID");
                sb.ApdL("                               AND MMC.LANG = ").ApdN(this.BindPrefix).ApdL("LANG2");
                sb.ApdN(" WHERE MT.TERMINAL_NO = ").ApdN(this.BindPrefix).ApdL("TERMINAL_NO");
                // バインド変数設定
                paramCollection.Add(dbHelper.NewDbParameter("LANG2", cond.LoginInfo.Language));
                paramCollection.Add(dbHelper.NewDbParameter("TERMINAL_NO", ComFunc.GetFld(dt, 0, Def_M_TERMINAL.TERMINAL_NO)));
            }
            sb.ApdL(" ORDER BY MMC_DISP_NO");
            sb.ApdL("        , MENU_CATEGORY_ID");
            sb.ApdL("        , MMI_DISP_NO");
            sb.ApdL("        , MENU_ITEM_ID");

            // バインド変数設定
            paramCollection.Add(dbHelper.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(dbHelper.NewDbParameter("USER_ID", cond.UserID));
            paramCollection.Add(dbHelper.NewDbParameter("MENU_ITEM_FLAG", cond.MenuItemFlag));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, ComDefine.DTTBL_MENU);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region メッセージ取得

    /// --------------------------------------------------
    /// <summary>
    /// メッセージ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">検索条件</param>
    /// <returns>メッセージ</returns>
    /// <create>Y.Higuchi 2010/04/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetMessage(DatabaseHelper dbHelper, CondCommon cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("        MESSAGE_ID");
            sb.ApdL("       ,MESSAGE_FLAG");
            sb.ApdL("       ,MESSAGE_LEVEL");
            sb.ApdL("       ,MESSAGE");
            sb.ApdL("       ,BUTTON_FLAG");
            sb.ApdL("       ,DEFAULT_BUTTON");
            sb.ApdL("  FROM ");
            sb.ApdL("       M_MESSAGE");
            sb.ApdL(" WHERE ");
            sb.ApdN("       MESSAGE_ID = ").ApdN(this.BindPrefix).ApdL("MESSAGE_ID");
            sb.ApdN("       AND LANG = ").ApdN(this.BindPrefix).ApdL("LANG");

            // バインド変数設定
            paramCollection.Add(dbHelper.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(dbHelper.NewDbParameter("MESSAGE_ID", cond.MessageID));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_MESSAGE.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 複数メッセージ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">検索条件</param>
    /// <returns>メッセージ</returns>
    /// <create>Y.Higuchi 2010/07/16</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetMultiMessage(DatabaseHelper dbHelper, CondCommon cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("        MESSAGE_ID");
            sb.ApdL("       ,MESSAGE_FLAG");
            sb.ApdL("       ,MESSAGE_LEVEL");
            sb.ApdL("       ,MESSAGE");
            sb.ApdL("       ,BUTTON_FLAG");
            sb.ApdL("       ,DEFAULT_BUTTON");
            sb.ApdL("  FROM ");
            sb.ApdL("       M_MESSAGE");
            sb.ApdL(" WHERE ");
            sb.ApdN("       LANG = ").ApdN(this.BindPrefix).ApdL("LANG");

            if (cond.ArrayMessageID != null && 0 < cond.ArrayMessageID.Length)
            {
                sb.ApdL("    AND ( 1=0 ");
                for (int i = 0; i < cond.ArrayMessageID.Length; i++)
                {
                    sb.ApdN("    OR MESSAGE_ID = ").ApdN(this.BindPrefix).ApdL("MESSAGE_ID_" + i.ToString());
                    paramCollection.Add(dbHelper.NewDbParameter("MESSAGE_ID_" + i.ToString(), cond.ArrayMessageID[i]));
                }
                sb.ApdL(" )");
            }

            // バインド変数設定
            paramCollection.Add(dbHelper.NewDbParameter("LANG", cond.LoginInfo.Language));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return dt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 汎用マスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// 汎用マスタ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">検索条件</param>
    /// <returns></returns>
    /// <create>Y.Higuchi 2010/04/27</create>
    /// <update>D.Okumura 2018/08/31</update>
    /// <remarks>
    /// 多言語化対応済みのため、読出し側コードでは意識しなくてもよい
    /// </remarks>
    /// --------------------------------------------------
    public DataSet GetCommon(DatabaseHelper dbHelper, CondCommon cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("        GROUP_CD");
            sb.ApdL("       ,GROUP_NAME");
            sb.ApdL("       ,ITEM_CD");
            sb.ApdL("       ,ITEM_NAME");
            sb.ApdL("       ,VALUE1");
            sb.ApdL("       ,VALUE2");
            sb.ApdL("       ,VALUE3");
            sb.ApdL("       ,DISP_NO");
            sb.ApdL("       ,DEFAULT_VALUE");
            sb.ApdL("  FROM ");
            sb.ApdL("       M_COMMON");
            sb.ApdL(" WHERE 1 = 1");
            sb.ApdN("       AND LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            paramCollection.Add(dbHelper.NewDbParameter("LANG", cond.LoginInfo.Language));
            
            if (!string.IsNullOrEmpty(cond.GroupCD))
            {
                sb.ApdN("   AND GROUP_CD = ").ApdN(this.BindPrefix).ApdL("GROUP_CD");
                // バインド変数設定
                paramCollection.Add(dbHelper.NewDbParameter("GROUP_CD", cond.GroupCD));
            }
            if (!string.IsNullOrEmpty(cond.ItemCD))
            {
                sb.ApdN("   AND ITEM_CD = ").ApdN(this.BindPrefix).ApdL("ITEM_CD");
                // バインド変数設定
                paramCollection.Add(dbHelper.NewDbParameter("ITEM_CD", cond.ItemCD));
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("        GROUP_CD");
            sb.ApdL("       ,DISP_NO");
            sb.ApdL("       ,VALUE1");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_COMMON.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region パスワードチェック

    /// --------------------------------------------------
    /// <summary>
    /// パスワードチェック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">パスワード変更コンディション</param>
    /// <param name="minPassword">パスワードの最小バイト数</param>
    /// <param name="maxPassword">パスワードの最大バイト数</param>
    /// <param name="passwordCheck">パスワードチェック</param>
    /// <param name="duplicationPastPassword">過去パスワード重複機能</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">メッセージのパラメーター</param>
    /// <returns></returns>
    /// <create>Y.Higuchi 2010/07/02</create>
    /// <update>H.Tajimi 2018/09/03 多言語対応</update>
    /// --------------------------------------------------
    public bool CheckInputPassword(DatabaseHelper dbHelper, CondUserPassword cond, int minPassword, int maxPassword, string passwordCheck, string duplicationPastPassword, out string errMsgID, out string[] args)
    {
        try
        {
            // ----- 初期化 -----
            errMsgID = string.Empty;
            args = null;

            var condCommon = new CondCommon(cond.LoginInfo);
            DataTable dtSysParam = GetSystemParameter(dbHelper, condCommon).Tables[Def_M_SYSTEM_PARAMETER.Name];

            // ----- パスワードチェック -----
            // 一応サーバーでもチェックする。
            if (!ComFunc.CheckInputPassword(cond.Password, cond.NewPassword, cond.ConfirmPassword, minPassword, maxPassword, passwordCheck, false, out errMsgID, out args))
            {
                return false;
            }

            // ----- 現在パスワードチェック -----
            CondCommon condCom = new CondCommon(cond.LoginInfo);
            condCom.UserID = cond.UserID;
            condCom.Password = cond.Password;
            condCom.LoginInfo = cond.LoginInfo;
            DataSet ds = GetLoginUser(dbHelper, condCom, false);
            if (ds == null || !ds.Tables.Contains(Def_M_USER.Name) || ds.Tables[Def_M_USER.Name].Rows.Count < 1)
            {
                // パスワードが違います。
                errMsgID = "FW010010015";
                return false;
            }

            // ----- パスワード重複チェック -----
            if (duplicationPastPassword == DUPLICATION_PAST_PASSWORD.ENABLE_VALUE1)
            {
                if (!this.CheckPastPassword(dbHelper, cond))
                {
                    // 過去に使用したパスワードなので使用できません。
                    errMsgID = "FW010010016";
                    return false;
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #region パスワード重複チェック

    /// --------------------------------------------------
    /// <summary>
    /// パスワード重複チェック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">パスワード変更コンディション</param>
    /// <returns>true:重複なし/false:重複有り</returns>
    /// <create>Y.Higuchi 2010/07/02</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool CheckPastPassword(DatabaseHelper dbHelper, CondUserPassword cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("       COUNT(1) AS CNT");
            sb.ApdL("  FROM ");
            sb.ApdL("       M_PAST_PASSWORD");
            sb.ApdL(" WHERE ");
            sb.ApdL("       1 = 1");
            if (cond.UserID != null)
            {
                sb.ApdN("   AND USER_ID = ").ApdN(this.BindPrefix).ApdL("USER_ID");
                paramCollection.Add(iNewParam.NewDbParameter("USER_ID", cond.UserID));
            }
            if (cond.NewPassword != null)
            {
                sb.ApdN("   AND PASSWORD = ").ApdN(this.BindPrefix).ApdL("PASSWORD");
                paramCollection.Add(iNewParam.NewDbParameter("PASSWORD", cond.NewPassword));
            }

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);
            if (0 == ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_CNT))
            {
                return true;
            }
            return false;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region メール関連

    #region メール登録前の確認

    /// --------------------------------------------------
    /// <summary>
    /// メール登録前の確認
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">共通コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet CheckMail(DatabaseHelper dbHelper, CondCommon cond)
    {
        try
        {
            var ds = new DataSet();
            ds.Tables.Add(this.GetMailSetting(dbHelper));
            ds.Tables.Add(this.ExistsBukkenMail(dbHelper, cond));
            ds.Tables.Add(this.GetMailAddress(dbHelper, cond));

            if (string.IsNullOrEmpty(cond.BukkenNo))
            {
                ds.Tables.Add(this.GetBukkenMailMeisai(dbHelper, cond, false));
            }
            else
            {
                ds.Tables.Add(this.GetBukken(dbHelper, cond));
                var dtBukkenMailMeisai = this.GetBukkenMailMeisai(dbHelper, cond, true);
                if (!UtilData.ExistsData(dtBukkenMailMeisai))
                {
                    dtBukkenMailMeisai = this.GetBukkenMailMeisai(dbHelper, cond, false);
                }
                ds.Tables.Add(dtBukkenMailMeisai);
            }
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #region メール設定の取得

    /// --------------------------------------------------
    /// <summary>
    /// メール設定の取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/20</create>
    /// <update>H.Tajimi 2019/08/16 TAG連携メール通知対応</update>
    /// --------------------------------------------------
    public DataTable GetMailSetting(DatabaseHelper dbHelper)
    {
        try
        {
            var dt = new DataTable(Def_M_MAIL_SETTING.Name);
            var sb = new StringBuilder();

            sb.ApdL("SELECT BUKKEN_ADD_EVENT");
            sb.ApdL("     , AR_ADD_EVENT");
            sb.ApdL("     , AR_UPDATE_EVENT");
            sb.ApdL("     , TAG_RENKEI_EVENT");
            sb.ApdL("     , TEMPLATE_FOLDER");
            sb.ApdL("  FROM M_MAIL_SETTING");

            dbHelper.Fill(sb.ToString(), dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 物件メールマスタの存在確認

    /// --------------------------------------------------
    /// <summary>
    /// 物件メールマスタの存在確認
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">共通コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable ExistsBukkenMail(DatabaseHelper dbHelper, CondCommon cond)
    {
        try
        {
            var dt = new DataTable(Def_M_BUKKEN_MAIL.Name);
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT COUNT(1) AS CNT");
            sb.ApdL("  FROM M_BUKKEN_MAIL");
            sb.ApdL(" WHERE SHUKKA_FLAG = @SHUKKA_FLAG");
            if (string.IsNullOrEmpty(cond.BukkenNo))
            {
                sb.ApdL("   AND MAIL_KBN = @MAIL_KBN");
                pc.Add(iNewParam.NewDbParameter("MAIL_KBN", MAIL_KBN.BUKKEN_VALUE1));
            }
            else
            {
                sb.ApdL("   AND BUKKEN_NO = @BUKKEN_NO");
                sb.ApdL("   AND MAIL_KBN = @MAIL_KBN");
                pc.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));
                pc.Add(iNewParam.NewDbParameter("MAIL_KBN", MAIL_KBN.COMMON_VALUE1));
            }

            pc.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));

            dbHelper.Fill(sb.ToString(), pc, dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region ログインユーザーのメールアドレス取得

    /// --------------------------------------------------
    /// <summary>
    /// ログインユーザーのメールアドレス取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">共通コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetMailAddress(DatabaseHelper dbHelper, CondCommon cond)
    {
        try
        {
            var dt = new DataTable(Def_M_USER.Name);
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT MAIL_ADDRESS");
            sb.ApdL("  FROM M_USER");
            sb.ApdL(" WHERE USER_ID = @USER_ID");

            pc.Add(iNewParam.NewDbParameter("USER_ID", cond.LoginInfo.UserID));

            dbHelper.Fill(sb.ToString(), pc, dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 物件名マスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// 物件名マスタ取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">Common用コンディション</param>
    /// <returns>データテーブル</returns>
    /// <create>R.Katsuo 2017/11/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetBukken(DatabaseHelper dbHelper, CondCommon cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_M_BUKKEN.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT *");
            sb.ApdL("  FROM M_BUKKEN");
            sb.ApdN(" WHERE M_BUKKEN.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND M_BUKKEN.BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 物件メール明細マスタ＋メールアドレスの取得

    /// --------------------------------------------------
    /// <summary>
    /// 物件メール明細マスタ＋メールアドレスの取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">共通コンディション</param>
    /// <param name="useListFlag">リストフラグを使って検索するか</param>
    /// <returns></returns>
    /// <create>R.Katsuo 2017/11/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetBukkenMailMeisai(DatabaseHelper dbHelper, CondCommon cond, bool useListFlag)
    {
        try
        {
            var dt = new DataTable(Def_M_BUKKEN_MAIL_MEISAI.Name);
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT M_BUKKEN_MAIL_MEISAI.USER_ID");
            sb.ApdL("     , M_USER.USER_NAME");
            sb.ApdL("     , M_USER.MAIL_ADDRESS");
            sb.ApdL("  FROM M_BUKKEN_MAIL");
            sb.ApdL(" INNER JOIN M_BUKKEN_MAIL_MEISAI");
            sb.ApdL("    ON M_BUKKEN_MAIL_MEISAI.MAIL_HEADER_ID = M_BUKKEN_MAIL.MAIL_HEADER_ID");
            sb.ApdL("  LEFT JOIN M_USER");
            sb.ApdL("    ON M_USER.USER_ID = M_BUKKEN_MAIL_MEISAI.USER_ID");

            sb.ApdL(" WHERE M_BUKKEN_MAIL.SHUKKA_FLAG = @SHUKKA_FLAG");
            if (string.IsNullOrEmpty(cond.BukkenNo))
            {
                sb.ApdL("   AND M_BUKKEN_MAIL.MAIL_KBN = @MAIL_KBN");
                pc.Add(iNewParam.NewDbParameter("MAIL_KBN", MAIL_KBN.BUKKEN_VALUE1));
            }
            else
            {
                if (!useListFlag)
                {
                    sb.ApdL("   AND M_BUKKEN_MAIL.BUKKEN_NO = @BUKKEN_NO");
                    sb.ApdL("   AND M_BUKKEN_MAIL.MAIL_KBN = @MAIL_KBN");
                    pc.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));
                    pc.Add(iNewParam.NewDbParameter("MAIL_KBN", MAIL_KBN.COMMON_VALUE1));
                }
                else
                {
                    sb.ApdL("   AND M_BUKKEN_MAIL.BUKKEN_NO = @BUKKEN_NO");
                    sb.ApdL("   AND M_BUKKEN_MAIL.MAIL_KBN = @MAIL_KBN");
                    sb.ApdL("   AND M_BUKKEN_MAIL.LIST_FLAG = @LIST_FLAG");
                    pc.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));
                    pc.Add(iNewParam.NewDbParameter("MAIL_KBN", MAIL_KBN.ARLIST_VALUE1));
                    pc.Add(iNewParam.NewDbParameter("LIST_FLAG", cond.ListFlag));
                }
            }

            pc.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));

            dbHelper.Fill(sb.ToString(), pc, dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region メール登録前の確認(荷姿表)

    /// --------------------------------------------------
    /// <summary>
    /// メール登録前の確認(荷姿表)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">共通コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/12/04</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet CheckPackingMail(DatabaseHelper dbHelper, CondCommon cond)
    {
        try
        {
            var ds = new DataSet();
            ds.Tables.Add(this.GetMailSetting(dbHelper));
            ds.Tables.Add(this.GetMailAddress(dbHelper, cond));
            ds.Tables.Add(this.GetPackingMailAddress(dbHelper, cond));
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #region メールアドレスの取得(荷姿表Excelの宛先)

    /// --------------------------------------------------
    /// <summary>
    /// メールアドレスの取得(荷姿表Excelの宛先)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">共通コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/12/04</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetPackingMailAddress(DatabaseHelper dbHelper, CondCommon cond)
    {
        try
        {
            var dt = new DataTable(ComDefine.DT_PACKING_M_USER);
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT M_USER.USER_ID");
            sb.ApdL("     , M_USER.USER_NAME");
            sb.ApdL("     , M_USER.MAIL_ADDRESS");
            sb.ApdL("     , M_USER.MAIL_PACKING_FLAG");
            sb.ApdL("  FROM M_USER");
            sb.ApdN(" WHERE M_USER.MAIL_PACKING_FLAG = ").ApdN(this.BindPrefix).ApdL("MAIL_PACKING_FLAG");

            pc.Add(iNewParam.NewDbParameter("MAIL_PACKING_FLAG", MAIL_PACKING_FLAG.TARGET_VALUE1));

            dbHelper.Fill(sb.ToString(), pc, dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region メール登録前の確認(TAG連携)

    /// --------------------------------------------------
    /// <summary>
    /// メール登録前の確認(TAG連携)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">共通コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/08/17</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet CheckTagRenkeiMail(DatabaseHelper dbHelper, CondCommon cond)
    {
        try
        {
            var ds = new DataSet();
            ds.Tables.Add(this.GetMailSetting(dbHelper));
            ds.Tables.Add(this.GetMailAddress(dbHelper, cond));
            ds.Tables.Add(this.GetTagRenkeiMailAddress(dbHelper, cond));
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #region メールアドレスの取得(TAG連携の宛先)

    /// --------------------------------------------------
    /// <summary>
    /// メールアドレスの取得(TAG連携の宛先)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">共通コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/08/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTagRenkeiMailAddress(DatabaseHelper dbHelper, CondCommon cond)
    {
        try
        {
            var dt = new DataTable(ComDefine.DT_TAG_RENKEI_M_USER);
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT M_USER.USER_ID");
            sb.ApdL("     , M_USER.USER_NAME");
            sb.ApdL("     , M_USER.MAIL_ADDRESS");
            sb.ApdL("     , M_USER.MAIL_TAG_RENKEI_FLAG");
            sb.ApdL("  FROM M_USER");
            sb.ApdN(" WHERE M_USER.MAIL_TAG_RENKEI_FLAG = ").ApdN(this.BindPrefix).ApdL("MAIL_TAG_RENKEI_FLAG");

            pc.Add(iNewParam.NewDbParameter("MAIL_TAG_RENKEI_FLAG", MAIL_TAG_RENKEI_FLAG.TARGET_VALUE1));

            dbHelper.Fill(sb.ToString(), pc, dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region メール登録前の確認(出荷計画表)

    /// --------------------------------------------------
    /// <summary>
    /// メール登録前の確認(出荷計画表)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">共通コンディション</param>
    /// <returns></returns>
    /// <create>Y.Gwon 2023/08/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet CheckPlanningMail(DatabaseHelper dbHelper, CondCommon cond)
    {
        try
        {
            var ds = new DataSet();
            //ds.Tables.Add(this.GetMailSetting(dbHelper));
            ds.Tables.Add(this.GetMailAddress(dbHelper, cond));
            ds.Tables.Add(this.GetPlanningMailAddress(dbHelper, cond));
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region メールアドレスの取得(荷姿表Excelの宛先)

    /// --------------------------------------------------
    /// <summary>
    /// メールアドレスの取得(出荷計画表Excelの宛先)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">共通コンディション</param>
    /// <returns></returns>
    /// <create>Y.Gwon 2023/08/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetPlanningMailAddress(DatabaseHelper dbHelper, CondCommon cond)
    {
        try
        {
            var dt = new DataTable(Def_M_CONSIGN_MAIL.Name);
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT M_USER.USER_ID");
            sb.ApdL("     , M_USER.USER_NAME");
            sb.ApdL("     , M_USER.MAIL_ADDRESS");
            sb.ApdL("     , M_CONSIGN_MAIL_MEISAI.MAIL_ADDRESS_FLAG");
            sb.ApdL("  FROM M_CONSIGN_MAIL");
            sb.ApdL(" INNER JOIN M_CONSIGN_MAIL_MEISAI");
            sb.ApdL("    ON M_CONSIGN_MAIL_MEISAI.MAIL_HEADER_ID = M_CONSIGN_MAIL.MAIL_HEADER_ID");
            sb.ApdL("  LEFT JOIN M_USER");
            sb.ApdL("    ON M_USER.USER_ID = M_CONSIGN_MAIL_MEISAI.USER_ID");
            sb.ApdN(" WHERE M_CONSIGN_MAIL.CONSIGN_CD = ").ApdN(this.BindPrefix).ApdL("CONSIGN_CD");
            sb.ApdL(" ORDER BY M_CONSIGN_MAIL_MEISAI.MAIL_ADDRESS_FLAG");
            sb.ApdL("        , M_CONSIGN_MAIL_MEISAI.ORDER_NO");

            pc.Add(iNewParam.NewDbParameter("CONSIGN_CD", cond.ConsignCD));

            dbHelper.Fill(sb.ToString(), pc, dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 登録に必要なメールデータ取得

    /// --------------------------------------------------
    /// <summary>
    /// 登録に必要なメールデータ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">共通コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetMailData(DatabaseHelper dbHelper, CondCommon cond)
    {
        try
        {
            var dt = new DataTable(Def_M_BUKKEN_MAIL.Name);
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT M_BUKKEN_MAIL_MEISAI.MAIL_ADDRESS_FLAG");
            sb.ApdL("     , M_BUKKEN_MAIL_MEISAI.ORDER_NO");
            sb.ApdL("     , M_BUKKEN_MAIL_MEISAI.USER_ID");
            sb.ApdL("     , M_USER.USER_NAME");
            sb.ApdL("     , M_USER.MAIL_ADDRESS");
            sb.ApdL("  FROM M_BUKKEN_MAIL");
            sb.ApdL("  LEFT JOIN M_BUKKEN_MAIL_MEISAI");
            sb.ApdL("    ON M_BUKKEN_MAIL_MEISAI.MAIL_HEADER_ID = M_BUKKEN_MAIL.MAIL_HEADER_ID");
            sb.ApdL("  LEFT JOIN M_USER");
            sb.ApdL("    ON M_USER.USER_ID = M_BUKKEN_MAIL_MEISAI.USER_ID");
            sb.ApdL(" WHERE SHUKKA_FLAG = @SHUKKA_FLAG");
            if (cond.MailKbn != MAIL_KBN.BUKKEN_VALUE1)
            {
                sb.ApdL("   AND BUKKEN_NO = @BUKKEN_NO");
                pc.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));
            }
            sb.ApdL("   AND MAIL_KBN = @MAIL_KBN");
            if (cond.MailKbn == MAIL_KBN.ARLIST_VALUE1)
            {
                sb.ApdL("   AND LIST_FLAG = @LIST_FLAG");
                pc.Add(iNewParam.NewDbParameter("LIST_FLAG", cond.ListFlag));
            }
            sb.ApdL(" ORDER BY MAIL_ADDRESS_FLAG");
            sb.ApdL("        , ORDER_NO");

            pc.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));
            pc.Add(iNewParam.NewDbParameter("MAIL_KBN", cond.MailKbn));

            dbHelper.Fill(sb.ToString(), pc, dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region メールデータの登録

    /// --------------------------------------------------
    /// <summary>
    /// メールデータの登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">ユーザー情報</param>
    /// <param name="dr">登録データ</param>
    /// <param name="errMsgId">エラーメッセージ</param>
    /// <param name="args">エラーメッセージ引数</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/21</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool SaveMail(DatabaseHelper dbHelper, CondCommon cond, DataRow dr, ref string errMsgId, ref string[] args)
    {
        try
        {
            // メールID採番
            var condSms = new CondSms(cond.LoginInfo);
            condSms.SaibanFlag = SAIBAN_FLAG.MAIL_ID_VALUE1;
            condSms.LoginInfo = cond.LoginInfo;

            string mailId;
            using (var impl = new WsSmsImpl())
            {
                if (!impl.GetSaiban(dbHelper, condSms, out mailId, out errMsgId))
                {
                    return false;
                }
            }

            dr.SetField<object>(Def_T_MAIL.MAIL_ID, mailId);
            // メールデータ登録
            this.InsMail(dbHelper, dr, cond);

            return true;
        }
        catch (Exception ex)
        {
            // 一意制約違反チェック
            if (this.IsDbDuplicationError(ex))
            {
                // 既に登録されているMailIDです。
                errMsgId = "A9999999056";
                return false;
            } throw new Exception(ex.Message, ex);
        }
    }

    #region メールデータの登録

    /// --------------------------------------------------
    /// <summary>
    /// メールデータの登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dr">登録データ</param>
    /// <param name="cond">ユーザー情報</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/20</create>
    /// <update>H.Tajimi 2018/12/05 添付ファイル対応</update>
    /// --------------------------------------------------
    public int InsMail(DatabaseHelper dbHelper, DataRow dr, CondCommon cond)
    {
        try
        {
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("INSERT");
            sb.ApdL("  INTO T_MAIL");
            sb.ApdL("VALUES (");
            sb.ApdL("       @MAIL_ID");
            sb.ApdL("     , @MAIL_SEND");
            sb.ApdL("     , @MAIL_SEND_DISPLAY");
            sb.ApdL("     , @MAIL_TO");
            sb.ApdL("     , @MAIL_TO_DISPLAY");
            sb.ApdL("     , @MAIL_CC");
            sb.ApdL("     , @MAIL_CC_DISPLAY");
            sb.ApdL("     , @MAIL_BCC");
            sb.ApdL("     , @MAIL_BCC_DISPLAY");
            sb.ApdL("     , @TITLE");
            sb.ApdL("     , @NAIYO");
            sb.ApdL("     , @MAIL_STATUS");
            sb.ApdL("     , @RETRY_COUNT");
            sb.ApdL("     , @REASON");
            sb.ApdL("     , SYSDATETIME()");
            sb.ApdL("     , @CREATE_USER_ID");
            sb.ApdL("     , @CREATE_USER_NAME");
            sb.ApdL("     , SYSDATETIME()");
            sb.ApdL("     , @UPDATE_USER_ID");
            sb.ApdL("     , @UPDATE_USER_NAME");
            sb.ApdL("     , SYSDATETIME()");
            sb.ApdL("     , @LANG");
            sb.ApdL("     , @APPENDIX_FILE_PATH");
            sb.ApdL("     , @DISP_REASON");
            sb.ApdL(")");

            pc.Add(iNewParam.NewDbParameter("MAIL_ID", ComFunc.GetFldObject(dr, Def_T_MAIL.MAIL_ID)));
            pc.Add(iNewParam.NewDbParameter("MAIL_SEND", ComFunc.GetFldObject(dr, Def_T_MAIL.MAIL_SEND)));
            pc.Add(iNewParam.NewDbParameter("MAIL_SEND_DISPLAY", ComFunc.GetFldObject(dr, Def_T_MAIL.MAIL_SEND_DISPLAY)));
            pc.Add(iNewParam.NewDbParameter("MAIL_TO", ComFunc.GetFldObject(dr, Def_T_MAIL.MAIL_TO)));
            pc.Add(iNewParam.NewDbParameter("MAIL_TO_DISPLAY", ComFunc.GetFldObject(dr, Def_T_MAIL.MAIL_TO_DISPLAY)));
            pc.Add(iNewParam.NewDbParameter("MAIL_CC", ComFunc.GetFldObject(dr, Def_T_MAIL.MAIL_CC)));
            pc.Add(iNewParam.NewDbParameter("MAIL_CC_DISPLAY", ComFunc.GetFldObject(dr, Def_T_MAIL.MAIL_CC_DISPLAY)));
            pc.Add(iNewParam.NewDbParameter("MAIL_BCC", ComFunc.GetFldObject(dr, Def_T_MAIL.MAIL_BCC)));
            pc.Add(iNewParam.NewDbParameter("MAIL_BCC_DISPLAY", ComFunc.GetFldObject(dr, Def_T_MAIL.MAIL_BCC_DISPLAY)));
            pc.Add(iNewParam.NewDbParameter("TITLE", ComFunc.GetFldObject(dr, Def_T_MAIL.TITLE)));
            pc.Add(iNewParam.NewDbParameter("NAIYO", ComFunc.GetFldObject(dr, Def_T_MAIL.NAIYO)));
            pc.Add(iNewParam.NewDbParameter("MAIL_STATUS", ComFunc.GetFldObject(dr, Def_T_MAIL.MAIL_STATUS)));
            pc.Add(iNewParam.NewDbParameter("RETRY_COUNT", ComFunc.GetFldObject(dr, Def_T_MAIL.RETRY_COUNT)));
            pc.Add(iNewParam.NewDbParameter("REASON", ComFunc.GetFldObject(dr, Def_T_MAIL.REASON)));
            pc.Add(iNewParam.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
            pc.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
            pc.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            pc.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            pc.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            pc.Add(iNewParam.NewDbParameter("APPENDIX_FILE_PATH", ComFunc.GetFldObject(dr, Def_T_MAIL.APPENDIX_FILE_PATH)));
            pc.Add(iNewParam.NewDbParameter("DISP_REASON", ComFunc.GetFldObject(dr, Def_T_MAIL.DISP_REASON)));

            int rec = dbHelper.ExecuteNonQuery(sb.ToString(), pc);
            return rec;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region メールデータの登録（確認）

    /// --------------------------------------------------
    /// <summary>
    /// メールデータの登録（確認）
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">ユーザー情報</param>
    /// <param name="dr">登録データ</param>
    /// <param name="mailIdTemp">事前取得したメールID</param>
    /// <param name="filePath">アップロード先パス</param>
    /// <param name="errMsgId">エラーメッセージ</param>
    /// <param name="args">エラーメッセージ引数</param>
    /// <returns></returns>
    /// <create>J.Chen 2024/08/06</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool SaveMailWithCheck(DatabaseHelper dbHelper, CondCommon cond, DataRow dr, string mailIdTemp, string filePath, ref string errMsgId, ref string[] args)
    {
        try
        {
            // メールID採番
            var condSms = new CondSms(cond.LoginInfo);
            condSms.SaibanFlag = SAIBAN_FLAG.MAIL_ID_VALUE1;
            condSms.LoginInfo = cond.LoginInfo;

            string mailId;
            using (var impl = new WsSmsImpl())
            {
                if (!impl.GetSaiban(dbHelper, condSms, out mailId, out errMsgId))
                {
                    return false;
                }
            }

            dr.SetField<object>(Def_T_MAIL.MAIL_ID, mailId);

            if (mailId == mailIdTemp)
            {
                dr.SetField<object>(Def_T_MAIL.APPENDIX_FILE_PATH, filePath);
            }

            // メールデータ登録
            this.InsMail(dbHelper, dr, cond);

            return true;
        }
        catch (Exception ex)
        {
            // 一意制約違反チェック
            if (this.IsDbDuplicationError(ex))
            {
                // 既に登録されているMailIDです。
                errMsgId = "A9999999056";
                return false;
            } throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #endregion

    #endregion

    #region INSERT

    #region 過去パスワードマスタ登録処理

    /// --------------------------------------------------
    /// <summary>
    /// 過去パスワードマスタ登録処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">パスワード変更コンディション</param>
    /// <returns>影響を及ぼしたレコード数</returns>
    /// <create>Y.Higuchi 2010/07/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsPastPassword(DatabaseHelper dbHelper, CondUserPassword cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO M_PAST_PASSWORD");
            sb.ApdL("(");
            sb.ApdL("       USER_ID");
            sb.ApdL("     , PASSWORD");
            sb.ApdL("     , CREATE_USER");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , VERSION");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PASSWORD");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdL(")");

            paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("USER_ID", cond.UserID));
            paramCollection.Add(iNewParam.NewDbParameter("PASSWORD", cond.NewPassword));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER", this.GetCreateUserID(cond)));

            // SQL実行
            record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            return record;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region UPDATE

    #region パスワード更新

    /// --------------------------------------------------
    /// <summary>
    /// パスワード更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">パスワード変更コンディション</param>
    /// <returns>影響を及ぼしたレコード数</returns>
    /// <create>Y.Higuchi 2010/07/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdUserPassword(DatabaseHelper dbHelper, CondUserPassword cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE M_USER ");
            sb.ApdL("SET");
            sb.ApdN("       PASSWORD = ").ApdN(this.BindPrefix).ApdL("PASSWORD");
            sb.ApdN("     , PASSWORD_CHANGE_DATE = CONVERT(NVARCHAR, ").ApdN(SysTimestamp).ApdL(", 111)");
            sb.ApdN("     , UPDATE_USER = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(SysTimestamp);
            sb.ApdN("     , VERSION = ").ApdL(SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       USER_ID = ").ApdN(this.BindPrefix).ApdL("USER_ID");

            paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PASSWORD", cond.NewPassword));
            paramCollection.Add(iNewParam.NewDbParameter("USER_ID", cond.UserID));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER", this.GetUpdateUserID(cond)));

            // SQL実行
            record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            return record;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

}
