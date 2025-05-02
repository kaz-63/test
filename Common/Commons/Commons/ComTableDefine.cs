using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commons
{
    #region メッセージマスタ(M_MESSAGE)

    /// --------------------------------------------------
    /// <summary>
    /// メッセージマスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_MESSAGE
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// メッセージID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MESSAGE_ID = "MESSAGE_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 言語種別
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LANG = "LANG";
        /// --------------------------------------------------
        /// <summary>
        /// メッセージ区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MESSAGE_FLAG = "MESSAGE_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// メッセージレベル
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MESSAGE_LEVEL = "MESSAGE_LEVEL";
        /// --------------------------------------------------
        /// <summary>
        /// メッセージ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MESSAGE = "MESSAGE";
        /// --------------------------------------------------
        /// <summary>
        /// ボタン区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BUTTON_FLAG = "BUTTON_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// デフォルトボタン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DEFAULT_BUTTON = "DEFAULT_BUTTON";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_MESSAGE"; }
        }

        #endregion
    }

    #endregion

    #region 汎用マスタ(M_COMMON)

    /// --------------------------------------------------
    /// <summary>
    /// 汎用マスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_COMMON
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// グループコード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GROUP_CD = "GROUP_CD";
        /// --------------------------------------------------
        /// <summary>
        /// グループ名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GROUP_NAME = "GROUP_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 項目コード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ITEM_CD = "ITEM_CD";
        /// --------------------------------------------------
        /// <summary>
        /// 言語種別
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LANG = "LANG";
        /// --------------------------------------------------
        /// <summary>
        /// 項目名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ITEM_NAME = "ITEM_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 値1
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VALUE1 = "VALUE1";
        /// --------------------------------------------------
        /// <summary>
        /// 値2
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VALUE2 = "VALUE2";
        /// --------------------------------------------------
        /// <summary>
        /// 値3
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VALUE3 = "VALUE3";
        /// --------------------------------------------------
        /// <summary>
        /// 表示順
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DISP_NO = "DISP_NO";
        /// --------------------------------------------------
        /// <summary>
        /// デフォルト値
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DEFAULT_VALUE = "DEFAULT_VALUE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_COMMON"; }
        }

        #endregion
    }

    #endregion

    #region システムパラメーターマスタ(M_SYSTEM_PARAMETER)

    /// --------------------------------------------------
    /// <summary>
    /// システムパラメーターマスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_SYSTEM_PARAMETER
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 端末ロール有効
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TERMINAL_ROLE = "TERMINAL_ROLE";
        /// --------------------------------------------------
        /// <summary>
        /// ゲスト端末有効
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TERMINAL_GUEST = "TERMINAL_GUEST";
        /// --------------------------------------------------
        /// <summary>
        /// ログイン機能
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ENABLED_LOGIN = "ENABLED_LOGIN";
        /// --------------------------------------------------
        /// <summary>
        /// デフォルトユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DEFAULT_USER_ID = "DEFAULT_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// ユーザーID最大バイト数
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAX_USER_ID = "MAX_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// パスワード最大バイト数
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAX_PASSWORD = "MAX_PASSWORD";
        /// --------------------------------------------------
        /// <summary>
        /// パスワード最小バイト数
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MIN_PASSWORD = "MIN_PASSWORD";
        /// --------------------------------------------------
        /// <summary>
        /// パスワード変更機能
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MY_PASSWORD_CHANGE = "MY_PASSWORD_CHANGE";
        /// --------------------------------------------------
        /// <summary>
        /// パスワード有効期限有無
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PASSWORD_EXPIRATION = "PASSWORD_EXPIRATION";
        /// --------------------------------------------------
        /// <summary>
        /// パスワード有効期限フラグ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string EXPIRATION_FLAG = "EXPIRATION_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// パスワード有効期限日数
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string EXPIRATION = "EXPIRATION";
        /// --------------------------------------------------
        /// <summary>
        /// パスワード期限警告フラグ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string EXPIRATION_WARNING_FLAG = "EXPIRATION_WARNING_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// パスワード期限警告期間
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string EXPIRATION_WARNING = "EXPIRATION_WARNING";
        /// --------------------------------------------------
        /// <summary>
        /// 過去パスワード重複機能
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DUPLICATION_PAST_PASSWORD = "DUPLICATION_PAST_PASSWORD";
        /// --------------------------------------------------
        /// <summary>
        /// パスワードチェック
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PASSWORD_CHECK = "PASSWORD_CHECK";
        /// --------------------------------------------------
        /// <summary>
        /// 便間移動同時実施フラグ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KIWAKU_KONPO_MOVE_SHIP_FLAG = "KIWAKU_KONPO_MOVE_SHIP_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 範囲区切り文字
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SEPARATOR_RANGE = "SEPARATOR_RANGE";
        /// --------------------------------------------------
        /// <summary>
        /// 項目区切り文字
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SEPARATOR_ITEM = "SEPARATOR_ITEM";
        /// --------------------------------------------------
        /// <summary>
        /// 単価計算利率
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CALCULATION_RATE = "CALCULATION_RATE";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_SYSTEM_PARAMETER"; }
        }

        #endregion
    }

    #endregion

    #region ユーザーマスタ(M_USER)

    /// --------------------------------------------------
    /// <summary>
    /// ユーザーマスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_USER
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string USER_ID = "USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string USER_NAME = "USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// パスワード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PASSWORD = "PASSWORD";
        /// --------------------------------------------------
        /// <summary>
        /// 権限ID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ROLE_ID = "ROLE_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 備考
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string USER_NOTE = "USER_NOTE";
        /// --------------------------------------------------
        /// <summary>
        /// ユーザー区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string USER_FLAG = "USER_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// パスワード変更日
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PASSWORD_CHANGE_DATE = "PASSWORD_CHANGE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーコード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER = "CREATE_USER";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーコード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER = "UPDATE_USER";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";
        /// --------------------------------------------------
        /// <summary>
        /// メールアドレス
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAIL_ADDRESS = "MAIL_ADDRESS";
        /// --------------------------------------------------
        /// <summary>
        /// メールアドレス変更権限
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAIL_CHANGE_ROLE = "MAIL_CHANGE_ROLE";
        /// --------------------------------------------------
        /// <summary>
        /// 社員区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string STAFF_KBN = "STAFF_KBN";
        /// --------------------------------------------------
        /// <summary>
        /// 荷姿表送信対象フラグ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAIL_PACKING_FLAG = "MAIL_PACKING_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// TAG連携送信対象フラグ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAIL_TAG_RENKEI_FLAG = "MAIL_TAG_RENKEI_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 計画取込一括設定対象フラグ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAIL_SHUKKAKEIKAKU_FLAG = "MAIL_SHUKKAKEIKAKU_FLAG";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_USER"; }
        }

        #endregion
    }

    #endregion

    #region 過去パスワードマスタ(M_PAST_PASSWORD)

    /// --------------------------------------------------
    /// <summary>
    /// 過去パスワードマスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_PAST_PASSWORD
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string USER_ID = "USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// パスワード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PASSWORD = "PASSWORD";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーコード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER = "CREATE_USER";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_PAST_PASSWORD"; }
        }

        #endregion
    }

    #endregion

    #region 端末マスタ(M_TERMINAL)

    /// --------------------------------------------------
    /// <summary>
    /// 端末マスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_TERMINAL
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 端末No
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TERMINAL_NO = "TERMINAL_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 端末名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TERMINAL_NAME = "TERMINAL_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 端末のPC名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TERMINAL_PCNAME = "TERMINAL_PCNAME";
        /// --------------------------------------------------
        /// <summary>
        /// 権限ID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ROLE_ID = "ROLE_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 端末種別
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TERMINAL_FLAG = "TERMINAL_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_TERMINAL"; }
        }

        #endregion
    }

    #endregion

    #region 権限マスタ(M_ROLE)

    /// --------------------------------------------------
    /// <summary>
    /// 権限マスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_ROLE
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 権限ID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ROLE_ID = "ROLE_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 言語種別
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LANG = "LANG";
        /// --------------------------------------------------
        /// <summary>
        /// 権限名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ROLE_NAME = "ROLE_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 権限種別
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ROLE_FLAG = "ROLE_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// ユーザー削除フラグ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string USER_DELETE_FLAG = "USER_DELETE_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_ROLE"; }
        }

        #endregion
    }

    #endregion

    #region 権限マップマスタ(M_ROLE_MAP)

    /// --------------------------------------------------
    /// <summary>
    /// 権限マップマスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_ROLE_MAP
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 権限ID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ROLE_ID = "ROLE_ID";
        /// --------------------------------------------------
        /// <summary>
        /// メニュー種別ID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MENU_CATEGORY_ID = "MENU_CATEGORY_ID";
        /// --------------------------------------------------
        /// <summary>
        /// メニュー項目ID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MENU_ITEM_ID = "MENU_ITEM_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_ROLE_MAP"; }
        }

        #endregion
    }

    #endregion

    #region メニュー種別マスタ(M_MENU_CATEGORY)

    /// --------------------------------------------------
    /// <summary>
    /// メニュー種別マスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_MENU_CATEGORY
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// メニュー種別ID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MENU_CATEGORY_ID = "MENU_CATEGORY_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 言語種別
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LANG = "LANG";
        /// --------------------------------------------------
        /// <summary>
        /// メニュー種別名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MENU_CATEGORY_NAME = "MENU_CATEGORY_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 表示順
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DISP_NO = "DISP_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_MENU_CATEGORY"; }
        }

        #endregion
    }

    #endregion

    #region メニュー項目マスタ(M_MENU_ITEM)

    /// --------------------------------------------------
    /// <summary>
    /// メニュー項目マスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_MENU_ITEM
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// メニュー種別ID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MENU_CATEGORY_ID = "MENU_CATEGORY_ID";
        /// --------------------------------------------------
        /// <summary>
        /// メニュー項目ID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MENU_ITEM_ID = "MENU_ITEM_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 言語種別
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LANG = "LANG";
        /// --------------------------------------------------
        /// <summary>
        /// メニュー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MENU_ITEM_NAME = "MENU_ITEM_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 表示順
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DISP_NO = "DISP_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 画面タイトル
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DISP_TITLE = "DISP_TITLE";
        /// --------------------------------------------------
        /// <summary>
        /// クラス名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CLASS_NAME = "CLASS_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// メニュー区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MENU_ITEM_FLAG = "MENU_ITEM_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// メニュー項目画像
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MENU_ITEM_IMAGE = "MENU_ITEM_IMAGE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_MENU_ITEM"; }
        }

        #endregion
    }

    #endregion

    #region 締めマスタ(M_SHIME)

    /// --------------------------------------------------
    /// <summary>
    /// 締めマスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_SHIME
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 処理フラグ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHORI_FLAG = "SHORI_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 前回日次処理日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NICHIJI_DATE = "NICHIJI_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 前回月次処理日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GETSUJI_DATE = "GETSUJI_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// ファイルバックアップパス
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FILE_BACKUP_PATH = "FILE_BACKUP_PATH";
        /// --------------------------------------------------
        /// <summary>
        /// エクスポートバックアップパス
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string EXP_BACKUP_PATH = "EXP_BACKUP_PATH";
        /// --------------------------------------------------
        /// <summary>
        /// 保持期間
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HOJIKIKAN = "HOJIKIKAN";
        /// --------------------------------------------------
        /// <summary>
        /// 開始時間
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string START_TIME = "START_TIME";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_SHIME"; }
        }

        #endregion
    }

    #endregion

    #region 汎用採番マスタ(M_SAIBAN)

    /// --------------------------------------------------
    /// <summary>
    /// 汎用採番マスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_SAIBAN
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 採番コード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SAIBAN_CD = "SAIBAN_CD";
        /// --------------------------------------------------
        /// <summary>
        /// 現在番号
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CURRENT_NO = "CURRENT_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 最小値
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MINVALUE = "MINVALUE";
        /// --------------------------------------------------
        /// <summary>
        /// 最大値
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAXVALUE = "MAXVALUE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_SAIBAN"; }
        }

        #endregion
    }

    #endregion

    #region 物件名マスタ(M_BUKKEN)

    /// --------------------------------------------------
    /// <summary>
    /// 物件名マスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_BUKKEN
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_FLAG = "SHUKKA_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 物件管理NO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BUKKEN_NO = "BUKKEN_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 物件名称
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BUKKEN_NAME = "BUKKEN_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 発行済TagNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ISSUED_TAG_NO = "ISSUED_TAG_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 保守日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAINTE_DATE = "MAINTE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 保守ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAINTE_USER_ID = "MAINTE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 保守ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAINTE_USER_NAME = "MAINTE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";
        /// --------------------------------------------------
        /// <summary>
        /// 保守バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAINTE_VERSION = "MAINTE_VERSION";
        /// --------------------------------------------------
        /// <summary>
        /// メール通知運用
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAIL_NOTIFY = "MAIL_NOTIFY";
        /// --------------------------------------------------
        /// <summary>
        /// ProjectNo
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PROJECT_NO = "PROJECT_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 受注No
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string JUCHU_NO = "JUCHU_NO";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_BUKKEN"; }
        }

        #endregion
    }

    #endregion

    #region 納入先マスタ(M_NONYUSAKI)

    /// --------------------------------------------------
    /// <summary>
    /// 納入先マスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_NONYUSAKI
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_FLAG = "SHUKKA_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 物件管理NO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BUKKEN_NO = "BUKKEN_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NONYUSAKI_CD = "NONYUSAKI_CD";
        /// --------------------------------------------------
        /// <summary>
        /// 納入先
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NONYUSAKI_NAME = "NONYUSAKI_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷便
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHIP = "SHIP";
        /// --------------------------------------------------
        /// <summary>
        /// 管理区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KANRI_FLAG = "KANRI_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 保守日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAINTE_DATE = "MAINTE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 保守ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAINTE_USER_ID = "MAINTE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 保守ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAINTE_USER_NAME = "MAINTE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名称0
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LIST_FLAG_NAME0 = "LIST_FLAG_NAME0";
        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名称1
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LIST_FLAG_NAME1 = "LIST_FLAG_NAME1";
        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名称2
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LIST_FLAG_NAME2 = "LIST_FLAG_NAME2";
        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名称3
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LIST_FLAG_NAME3 = "LIST_FLAG_NAME3";
        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名称4
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LIST_FLAG_NAME4 = "LIST_FLAG_NAME4";
        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名称5
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LIST_FLAG_NAME5 = "LIST_FLAG_NAME5";
        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名称6
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LIST_FLAG_NAME6 = "LIST_FLAG_NAME6";
        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名称7
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LIST_FLAG_NAME7 = "LIST_FLAG_NAME7";
        /// --------------------------------------------------
        /// <summary>
        /// 除外フラグ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string REMOVE_FLAG = "REMOVE_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";
        /// --------------------------------------------------
        /// <summary>
        /// 運送区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TRANSPORT_FLAG = "TRANSPORT_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 有償・無償
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ESTIMATE_FLAG = "ESTIMATE_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷日
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHIP_DATE = "SHIP_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷元
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHIP_FROM = "SHIP_FROM";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷先
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHIP_TO = "SHIP_TO";
        /// --------------------------------------------------
        /// <summary>
        /// 案件管理No
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHIP_NO = "SHIP_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 運賃梱包製番
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHIP_SEIBAN = "SHIP_SEIBAN";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷元CD
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHIP_FROM_CD = "SHIP_FROM_CD";
        /// --------------------------------------------------
        /// <summary>
        /// 処理フラグ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SYORI_FlAG = "SYORI_FlAG";
        /// --------------------------------------------------
        /// <summary>
        /// 製番
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SEIBAN = "SEIBAN";
        /// --------------------------------------------------
        /// <summary>
        /// 機種
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KISHU = "KISHU";
        /// --------------------------------------------------
        /// <summary>
        /// 内容
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NAIYO = "NAIYO";
        /// --------------------------------------------------
        /// <summary>
        /// 到着予定日
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TOUCHAKUYOTEI_DATE = "TOUCHAKUYOTEI_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 機械Parts
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KIKAI_PARTS = "KIKAI_PARTS";
        /// --------------------------------------------------
        /// <summary>
        /// 制御Parts
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SEIGYO_PARTS = "SEIGYO_PARTS";
        /// --------------------------------------------------
        /// <summary>
        /// 備考
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BIKO = "BIKO";
        /// --------------------------------------------------
        /// <summary>
        /// 荷受CD
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CONSIGN_CD = "CONSIGN_CD";
        /// --------------------------------------------------
        /// <summary>
        /// 処理フラグ（前回値）
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LAST_SYORI_FLAG = "LAST_SYORI_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// TAG登録フラグ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LOCK_FLAG = "LOCK_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 排他ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HAITA_USER_ID = "HAITA_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 排他ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HAITA_USER_NAME = "HAITA_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 排他日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HAITA_DATE = "HAITA_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 現場用状態フラグ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GENBA_YO_STATUS_FLAG = "GENBA_YO_STATUS_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 現場用物量
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GENBA_YO_BUTSURYO = "GENBA_YO_BUTSURYO";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_NONYUSAKI"; }
        }

        #endregion
    }

    #endregion

    #region 名称マスタ(M_SELECT_ITEM)

    /// --------------------------------------------------
    /// <summary>
    /// 名称マスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_SELECT_ITEM
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 選択グループCD
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SELECT_GROUP_CD = "SELECT_GROUP_CD";
        /// --------------------------------------------------
        /// <summary>
        /// 選択肢名称
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ITEM_NAME = "ITEM_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 保守日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAINTE_DATE = "MAINTE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 保守ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAINTE_USER_ID = "MAINTE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 保守ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAINTE_USER_NAME = "MAINTE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_SELECT_ITEM"; }
        }

        #endregion
    }

    #endregion

    #region 出荷明細データ(T_SHUKKA_MEISAI)

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_SHUKKA_MEISAI
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_FLAG = "SHUKKA_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NONYUSAKI_CD = "NONYUSAKI_CD";
        /// --------------------------------------------------
        /// <summary>
        /// TagNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TAG_NO = "TAG_NO";
        /// --------------------------------------------------
        /// <summary>
        /// ARNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string AR_NO = "AR_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 製番
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SEIBAN = "SEIBAN";
        /// --------------------------------------------------
        /// <summary>
        /// CODE
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CODE = "CODE";
        /// --------------------------------------------------
        /// <summary>
        /// 図面追番
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ZUMEN_OIBAN = "ZUMEN_OIBAN";
        /// --------------------------------------------------
        /// <summary>
        /// Area
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string AREA = "AREA";
        /// --------------------------------------------------
        /// <summary>
        /// Floor
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLOOR = "FLOOR";
        /// --------------------------------------------------
        /// <summary>
        /// 機種
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KISHU = "KISHU";
        /// --------------------------------------------------
        /// <summary>
        /// STNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ST_NO = "ST_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 品名(和文)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HINMEI_JP = "HINMEI_JP";
        /// --------------------------------------------------
        /// <summary>
        /// 品名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HINMEI = "HINMEI";
        /// --------------------------------------------------
        /// <summary>
        /// 図面/形式
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ZUMEN_KEISHIKI = "ZUMEN_KEISHIKI";
        /// --------------------------------------------------
        /// <summary>
        /// 区割NO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KUWARI_NO = "KUWARI_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 数量
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NUM = "NUM";
        /// --------------------------------------------------
        /// <summary>
        /// 状態区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string JYOTAI_FLAG = "JYOTAI_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// TAG発行区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TAGHAKKO_FLAG = "TAGHAKKO_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// TAG発行日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TAGHAKKO_DATE = "TAGHAKKO_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 集荷日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKA_DATE = "SHUKA_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// BOXNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BOX_NO = "BOX_NO";
        /// --------------------------------------------------
        /// <summary>
        /// BOX梱包日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BOXKONPO_DATE = "BOXKONPO_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// パレットNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PALLET_NO = "PALLET_NO";
        /// --------------------------------------------------
        /// <summary>
        /// パレット梱包日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PALLETKONPO_DATE = "PALLETKONPO_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 工事識別管理NO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KOJI_NO = "KOJI_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 木枠梱包内部管理キー
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CASE_ID = "CASE_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 木枠梱包日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KIWAKUKONPO_DATE = "KIWAKUKONPO_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_DATE = "SHUKKA_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_USER_ID = "SHUKKA_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_USER_NAME = "SHUKKA_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 運送会社
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UNSOKAISHA_NAME = "UNSOKAISHA_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// インボイスNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string INVOICE_NO = "INVOICE_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 送り状NO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string OKURIJYO_NO = "OKURIJYO_NO";
        /// --------------------------------------------------
        /// <summary>
        /// BLNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BL_NO = "BL_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 受入日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UKEIRE_DATE = "UKEIRE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 受入ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UKEIRE_USER_ID = "UKEIRE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 受入ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UKEIRE_USER_NAME = "UKEIRE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";
        /// --------------------------------------------------
        /// <summary>
        /// フリー１
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FREE1 = "FREE1";
        /// --------------------------------------------------
        /// <summary>
        /// フリー２
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FREE2 = "FREE2";
        /// --------------------------------------------------
        /// <summary>
        /// タグ用納入先コード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TAG_NONYUSAKI_CD = "TAG_NONYUSAKI_CD";
        /// --------------------------------------------------
        /// <summary>
        /// メモ（備考）
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BIKO = "BIKO";
        /// --------------------------------------------------
        /// <summary>
        /// MNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string M_NO = "M_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 重量
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GRWT = "GRWT";
        /// --------------------------------------------------
        /// <summary>
        /// 手配連携No
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEHAI_RENKEI_NO = "TEHAI_RENKEI_NO";
        /// --------------------------------------------------
        /// <summary>
        /// ファイル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FILE_NAME = "FILE_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 図番/型式2
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ZUMEN_KEISHIKI2 = "ZUMEN_KEISHIKI2";
        /// --------------------------------------------------
        /// <summary>
        /// TAG便名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TAG_SHIP = "TAG_SHIP";
        /// --------------------------------------------------
        /// <summary>
        /// 引渡日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HIKIWATASHI_DATE = "HIKIWATASHI_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 手配更新履歴
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEHAI_RIREKI = "TEHAI_RIREKI";
        /// --------------------------------------------------
        /// <summary>
        /// 通関確認状態
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CUSTOMS_STATUS = "CUSTOMS_STATUS";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_SHUKKA_MEISAI"; }
        }

        #endregion
    }

    #endregion

    #region ＢＯＸリスト管理データ(T_BOXLIST_MANAGE)

    /// --------------------------------------------------
    /// <summary>
    /// ＢＯＸリスト管理データ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_BOXLIST_MANAGE
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_FLAG = "SHUKKA_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NONYUSAKI_CD = "NONYUSAKI_CD";
        /// --------------------------------------------------
        /// <summary>
        /// BOXNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BOX_NO = "BOX_NO";
        /// --------------------------------------------------
        /// <summary>
        /// リスト発行区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LISTHAKKO_FLAG = "LISTHAKKO_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// リスト発行日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LISTHAKKO_DATE = "LISTHAKKO_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// リスト発行ユーザ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LISTHAKKO_USER = "LISTHAKKO_USER";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_DATE = "SHUKKA_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_USER_ID = "SHUKKA_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_USER_NAME = "SHUKKA_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 受入日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UKEIRE_DATE = "UKEIRE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 受入ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UKEIRE_USER_ID = "UKEIRE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 受入ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UKEIRE_USER_NAME = "UKEIRE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 管理区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KANRI_FLAG = "KANRI_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";
        /// --------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BOX_NO_NUM = "BOX_NO_NUM";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_BOXLIST_MANAGE"; }
        }

        #endregion
    }

    #endregion

    #region パレットリスト管理データ(T_PALLETLIST_MANAGE)

    /// --------------------------------------------------
    /// <summary>
    /// パレットリスト管理データ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_PALLETLIST_MANAGE
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_FLAG = "SHUKKA_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NONYUSAKI_CD = "NONYUSAKI_CD";
        /// --------------------------------------------------
        /// <summary>
        /// パレットNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PALLET_NO = "PALLET_NO";
        /// --------------------------------------------------
        /// <summary>
        /// リスト発行区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LISTHAKKO_FLAG = "LISTHAKKO_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// リスト発行日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LISTHAKKO_DATE = "LISTHAKKO_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// リスト発行ユーザ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LISTHAKKO_USER = "LISTHAKKO_USER";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_DATE = "SHUKKA_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_USER_ID = "SHUKKA_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_USER_NAME = "SHUKKA_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 受入日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UKEIRE_DATE = "UKEIRE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 受入ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UKEIRE_USER_ID = "UKEIRE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 受入ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UKEIRE_USER_NAME = "UKEIRE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 管理区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KANRI_FLAG = "KANRI_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";
        /// --------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PALLET_NO_NUM = "PALLET_NO_NUM";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_PALLETLIST_MANAGE"; }
        }

        #endregion
    }

    #endregion

    #region 木枠データ(T_KIWAKU)

    /// --------------------------------------------------
    /// <summary>
    /// 木枠データ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_KIWAKU
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 工事識別管理NO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KOJI_NO = "KOJI_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 工事識別名称
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KOJI_NAME = "KOJI_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷便
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHIP = "SHIP";
        /// --------------------------------------------------
        /// <summary>
        /// 登録区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TOROKU_FLAG = "TOROKU_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// (A)CASE MARK
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CASE_MARK_FILE = "CASE_MARK_FILE";
        /// --------------------------------------------------
        /// <summary>
        /// (B)DELIVERY NO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DELIVERY_NO = "DELIVERY_NO";
        /// --------------------------------------------------
        /// <summary>
        /// (C)PORT OF DESTINATION
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PORT_OF_DESTINATION = "PORT_OF_DESTINATION";
        /// --------------------------------------------------
        /// <summary>
        /// (D)AIR/BOAT
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string AIR_BOAT = "AIR_BOAT";
        /// --------------------------------------------------
        /// <summary>
        /// (E)DELIVERY DATE
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DELIVERY_DATE = "DELIVERY_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// (F)DELIVERY POINT
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DELIVERY_POINT = "DELIVERY_POINT";
        /// --------------------------------------------------
        /// <summary>
        /// (G)FACTORY
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FACTORY = "FACTORY";
        /// --------------------------------------------------
        /// <summary>
        /// REMARKS
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string REMARKS = "REMARKS";
        /// --------------------------------------------------
        /// <summary>
        /// 作業区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SAGYO_FLAG = "SAGYO_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_DATE = "SHUKKA_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_USER_ID = "SHUKKA_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_USER_NAME = "SHUKKA_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 運送会社
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UNSOKAISHA_NAME = "UNSOKAISHA_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// インボイスNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string INVOICE_NO = "INVOICE_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 送り状NO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string OKURIJYO_NO = "OKURIJYO_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_FLAG = "SHUKKA_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NONYUSAKI_CD = "NONYUSAKI_CD";
        /// --------------------------------------------------
        /// <summary>
        /// 登録種別
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string INSERT_TYPE = "INSERT_TYPE";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_KIWAKU"; }
        }

        #endregion
    }

    #endregion

    #region 木枠明細データ(T_KIWAKU_MEISAI)

    /// --------------------------------------------------
    /// <summary>
    /// 木枠明細データ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_KIWAKU_MEISAI
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 工事識別管理NO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KOJI_NO = "KOJI_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 内部管理用キー
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CASE_ID = "CASE_ID";
        /// --------------------------------------------------
        /// <summary>
        /// C/NO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CASE_NO = "CASE_NO";
        /// --------------------------------------------------
        /// <summary>
        /// STYLE
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string STYLE = "STYLE";
        /// --------------------------------------------------
        /// <summary>
        /// ITEM
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ITEM = "ITEM";
        /// --------------------------------------------------
        /// <summary>
        /// DESCRIPTION(1)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DESCRIPTION_1 = "DESCRIPTION_1";
        /// --------------------------------------------------
        /// <summary>
        /// DESCRIPTION(2)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DESCRIPTION_2 = "DESCRIPTION_2";
        /// --------------------------------------------------
        /// <summary>
        /// DIMENSION(L)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DIMENSION_L = "DIMENSION_L";
        /// --------------------------------------------------
        /// <summary>
        /// DIMENSION(W)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DIMENSION_W = "DIMENSION_W";
        /// --------------------------------------------------
        /// <summary>
        /// DIMENSION(H)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DIMENSION_H = "DIMENSION_H";
        /// --------------------------------------------------
        /// <summary>
        /// MMNET
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MMNET = "MMNET";
        /// --------------------------------------------------
        /// <summary>
        /// NET/W
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NET_W = "NET_W";
        /// --------------------------------------------------
        /// <summary>
        /// GROSS/W
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GROSS_W = "GROSS_W";
        /// --------------------------------------------------
        /// <summary>
        /// 木材重量
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MOKUZAI_JYURYO = "MOKUZAI_JYURYO";
        /// --------------------------------------------------
        /// <summary>
        /// パレットNO(1)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PALLET_NO_1 = "PALLET_NO_1";
        /// --------------------------------------------------
        /// <summary>
        /// パレットNO(2)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PALLET_NO_2 = "PALLET_NO_2";
        /// --------------------------------------------------
        /// <summary>
        /// パレットNO(3)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PALLET_NO_3 = "PALLET_NO_3";
        /// --------------------------------------------------
        /// <summary>
        /// パレットNO(4)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PALLET_NO_4 = "PALLET_NO_4";
        /// --------------------------------------------------
        /// <summary>
        /// パレットNO(5)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PALLET_NO_5 = "PALLET_NO_5";
        /// --------------------------------------------------
        /// <summary>
        /// パレットNO(6)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PALLET_NO_6 = "PALLET_NO_6";
        /// --------------------------------------------------
        /// <summary>
        /// パレットNO(7)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PALLET_NO_7 = "PALLET_NO_7";
        /// --------------------------------------------------
        /// <summary>
        /// パレットNO(8)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PALLET_NO_8 = "PALLET_NO_8";
        /// --------------------------------------------------
        /// <summary>
        /// パレットNO(9)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PALLET_NO_9 = "PALLET_NO_9";
        /// --------------------------------------------------
        /// <summary>
        /// パレットNO(10)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PALLET_NO_10 = "PALLET_NO_10";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_DATE = "SHUKKA_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_USER_ID = "SHUKKA_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_USER_NAME = "SHUKKA_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";
        /// --------------------------------------------------
        /// <summary>
        /// 印刷C/No
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PRINT_CASE_NO = "PRINT_CASE_NO";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_KIWAKU_MEISAI"; }
        }

        #endregion
    }

    #endregion

    #region 社外用木枠明細データ(T_SHAGAI_KIWAKU_MEISAI)

    /// --------------------------------------------------
    /// <summary>
    /// 社外用木枠明細データ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_SHAGAI_KIWAKU_MEISAI
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 工事識別管理NO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KOJI_NO = "KOJI_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 内部管理用キー
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CASE_ID = "CASE_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_FLAG = "SHUKKA_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NONYUSAKI_CD = "NONYUSAKI_CD";
        /// --------------------------------------------------
        /// <summary>
        /// TAGNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TAG_NO = "TAG_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_SHAGAI_KIWAKU_MEISAI"; }
        }

        #endregion
    }

    #endregion

    #region ＡＲ情報データ(T_AR)

    /// --------------------------------------------------
    /// <summary>
    /// ＡＲ情報データ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_AR
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NONYUSAKI_CD = "NONYUSAKI_CD";
        /// --------------------------------------------------
        /// <summary>
        /// リスト区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LIST_FLAG = "LIST_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// ARNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string AR_NO = "AR_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 状況区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string JYOKYO_FLAG = "JYOKYO_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 発生日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HASSEI_DATE = "HASSEI_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 連絡者
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string RENRAKUSHA = "RENRAKUSHA";
        /// --------------------------------------------------
        /// <summary>
        /// 機種
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KISHU = "KISHU";
        /// --------------------------------------------------
        /// <summary>
        /// 号機
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GOKI = "GOKI";
        /// --------------------------------------------------
        /// <summary>
        /// 現場到着希望日
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GENBA_TOTYAKUKIBOU_DATE = "GENBA_TOTYAKUKIBOU_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 不具合内容
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HUGUAI = "HUGUAI";
        /// --------------------------------------------------
        /// <summary>
        /// 対策内容
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TAISAKU = "TAISAKU";
        /// --------------------------------------------------
        /// <summary>
        /// 備考
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BIKO = "BIKO";
        /// --------------------------------------------------
        /// <summary>
        /// 現地・手配先
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GENCHI_TEHAISAKI = "GENCHI_TEHAISAKI";
        /// --------------------------------------------------
        /// <summary>
        /// 現地・設定納期
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GENCHI_SETTEINOKI_DATE = "GENCHI_SETTEINOKI_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 現地・出荷予定日
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GENCHI_SHUKKAYOTEI_DATE = "GENCHI_SHUKKAYOTEI_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 現地・工場出荷日
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GENCHI_KOJYOSHUKKA_DATE = "GENCHI_KOJYOSHUKKA_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷方法
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKAHOHO = "SHUKKAHOHO";
        /// --------------------------------------------------
        /// <summary>
        /// 日本・設定納期
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string JP_SETTEINOKI_DATE = "JP_SETTEINOKI_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 日本・出荷予定日
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string JP_SHUKKAYOTEI_DATE = "JP_SHUKKAYOTEI_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 日本・工場出荷日
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string JP_KOJYOSHUKKA_DATE = "JP_KOJYOSHUKKA_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 日本・運送会社
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string JP_UNSOKAISHA_NAME = "JP_UNSOKAISHA_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 日本・送り状NO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string JP_OKURIJYO_NO = "JP_OKURIJYO_NO";
        /// --------------------------------------------------
        /// <summary>
        /// GMS発行NO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GMS_HAKKO_NO = "GMS_HAKKO_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 仕様連絡NO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHIYORENRAKU_NO = "SHIYORENRAKU_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 対応部署
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TAIO_BUSHO = "TAIO_BUSHO";
        /// --------------------------------------------------
        /// <summary>
        /// 技連NO(1)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GIREN_NO_1 = "GIREN_NO_1";
        /// --------------------------------------------------
        /// <summary>
        /// 技連(1)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GIREN_FILE_1 = "GIREN_FILE_1";
        /// --------------------------------------------------
        /// <summary>
        /// 技連NO(2)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GIREN_NO_2 = "GIREN_NO_2";
        /// --------------------------------------------------
        /// <summary>
        /// 技連(2)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GIREN_FILE_2 = "GIREN_FILE_2";
        /// --------------------------------------------------
        /// <summary>
        /// 技連NO(3)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GIREN_NO_3 = "GIREN_NO_3";
        /// --------------------------------------------------
        /// <summary>
        /// 技連(3)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GIREN_FILE_3 = "GIREN_FILE_3";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_DATE = "SHUKKA_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_USER_ID = "SHUKKA_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_USER_NAME = "SHUKKA_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 受入日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UKEIRE_DATE = "UKEIRE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 受入ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UKEIRE_USER_ID = "UKEIRE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 受入ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UKEIRE_USER_NAME = "UKEIRE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// ロックユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LOCK_USER_ID = "LOCK_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// ロック開始日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LOCK_STARTDATE = "LOCK_STARTDATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";
        /// --------------------------------------------------
        /// <summary>
        /// 発生要因
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HASSEI_YOUIN = "HASSEI_YOUIN";
        /// --------------------------------------------------
        /// <summary>
        /// 参考資料NO(1)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string REFERENCE_NO_1 = "REFERENCE_NO_1";
        /// --------------------------------------------------
        /// <summary>
        /// 参考資料(1)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string REFERENCE_FILE_1 = "REFERENCE_FILE_1";
        /// --------------------------------------------------
        /// <summary>
        /// 参考資料NO(2)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string REFERENCE_NO_2 = "REFERENCE_NO_2";
        /// --------------------------------------------------
        /// <summary>
        /// 参考資料(2)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string REFERENCE_FILE_2 = "REFERENCE_FILE_2";
        /// --------------------------------------------------
        /// <summary>
        /// 技連NO(4)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GIREN_NO_4 = "GIREN_NO_4";
        /// --------------------------------------------------
        /// <summary>
        /// 技連(4)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GIREN_FILE_4 = "GIREN_FILE_4";
        /// --------------------------------------------------
        /// <summary>
        /// 技連NO(5)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GIREN_NO_5 = "GIREN_NO_5";
        /// --------------------------------------------------
        /// <summary>
        /// 技連(5)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GIREN_FILE_5 = "GIREN_FILE_5";
        /// --------------------------------------------------
        /// <summary>
        /// 進捗管理有無
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHINCHOKU_FLAG = "SHINCHOKU_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 元ARNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MOTO_AR_NO = "MOTO_AR_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 現地・出荷状況区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GENCHI_SHUKKAJYOKYO_FLAG = "GENCHI_SHUKKAJYOKYO_FLAG";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_AR"; }
        }

        #endregion
    }

    #endregion

    #region 履歴データ(T_RIREKI)

    /// --------------------------------------------------
    /// <summary>
    /// 履歴データ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_RIREKI
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 画面区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GAMEN_FLAG = "GAMEN_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_FLAG = "SHUKKA_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NONYUSAKI_CD = "NONYUSAKI_CD";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷便
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHIP = "SHIP";
        /// --------------------------------------------------
        /// <summary>
        /// ARNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string AR_NO = "AR_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 物件管理NO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BUKKEN_NO = "BUKKEN_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 操作区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string OPERATION_FLAG = "OPERATION_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 更新PC名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_PC_NAME = "UPDATE_PC_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_RIREKI"; }
        }

        #endregion
    }

    #endregion

    #region ＡＲ対応費用(T_AR_COST)

    /// --------------------------------------------------
    /// <summary>
    /// ＡＲ対応費用
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_AR_COST
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NONYUSAKI_CD = "NONYUSAKI_CD";
        /// --------------------------------------------------
        /// <summary>
        /// リスト区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LIST_FLAG = "LIST_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// ARNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string AR_NO = "AR_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 行番号
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LINE_NO = "LINE_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 項目コード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ITEM_CD = "ITEM_CD";
        /// --------------------------------------------------
        /// <summary>
        /// 作業時間
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string WORK_TIME = "WORK_TIME";
        /// --------------------------------------------------
        /// <summary>
        /// 人員
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string WORKERS = "WORKERS";
        /// --------------------------------------------------
        /// <summary>
        /// 台数
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NUMBER = "NUMBER";
        /// --------------------------------------------------
        /// <summary>
        /// 単価
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string RATE = "RATE";
        /// --------------------------------------------------
        /// <summary>
        /// 合計
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TOTAL = "TOTAL";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_AR_COST"; }
        }

        #endregion
    }

    #endregion

    #region ＡＲ添付ファイル(T_AR_FILE)

    /// --------------------------------------------------
    /// <summary>
    /// ＡＲ添付ファイル
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_AR_FILE
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NONYUSAKI_CD = "NONYUSAKI_CD";
        /// --------------------------------------------------
        /// <summary>
        /// リスト区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LIST_FLAG = "LIST_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// ARNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string AR_NO = "AR_NO";
        /// --------------------------------------------------
        /// <summary>
        /// ファイル種別
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FILE_KIND = "FILE_KIND";
        /// --------------------------------------------------
        /// <summary>
        /// 位置
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string POSITION = "POSITION";
        /// --------------------------------------------------
        /// <summary>
        /// ファイル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FILE_NAME = "FILE_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_AR_FILE"; }
        }

        #endregion
    }

    #endregion

    #region ＡＲ号機データ(T_AR_GOKI)

    /// --------------------------------------------------
    /// <summary>
    /// ＡＲ号機データ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_AR_GOKI
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NONYUSAKI_CD = "NONYUSAKI_CD";
        /// --------------------------------------------------
        /// <summary>
        /// 号機
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GOKI = "GOKI";
        /// --------------------------------------------------
        /// <summary>
        /// 機種
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KISHU = "KISHU";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_AR_GOKI"; }
        }

        #endregion
    }

    #endregion

    #region 一時取込データ(ＡＲ号機)(T_AR_GOKI_TEMPWORK)

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込データ(ＡＲ号機)
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_AR_GOKI_TEMPWORK
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 一時取込ID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEMP_ID = "TEMP_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NONYUSAKI_CD = "NONYUSAKI_CD";
        /// --------------------------------------------------
        /// <summary>
        /// 号機
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GOKI = "GOKI";
        /// --------------------------------------------------
        /// <summary>
        /// 機種
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KISHU = "KISHU";
        /// --------------------------------------------------
        /// <summary>
        /// フラグ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLAG = "FLAG";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_AR_GOKI_TEMPWORK"; }
        }

        #endregion
    }

    #endregion

    #region ＡＲ進捗データ(T_AR_SHINCHOKU)

    /// --------------------------------------------------
    /// <summary>
    /// ＡＲ進捗データ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_AR_SHINCHOKU
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NONYUSAKI_CD = "NONYUSAKI_CD";
        /// --------------------------------------------------
        /// <summary>
        /// リスト区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LIST_FLAG = "LIST_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// ARNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string AR_NO = "AR_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 号機
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GOKI = "GOKI";
        /// --------------------------------------------------
        /// <summary>
        /// 現地到着希望日
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DATE_SITE_REQ = "DATE_SITE_REQ";
        /// --------------------------------------------------
        /// <summary>
        /// 現地出荷予定日
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DATE_LOCAL = "DATE_LOCAL";
        /// --------------------------------------------------
        /// <summary>
        /// 日本出荷予定日
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DATE_JP = "DATE_JP";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_AR_SHINCHOKU"; }
        }

        #endregion
    }

    #endregion

    #region ＡＲ進捗データ履歴(T_AR_SHINCHOKU_RIREKI)

    /// --------------------------------------------------
    /// <summary>
    /// ＡＲ進捗データ履歴
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_AR_SHINCHOKU_RIREKI
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NONYUSAKI_CD = "NONYUSAKI_CD";
        /// --------------------------------------------------
        /// <summary>
        /// リスト区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LIST_FLAG = "LIST_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// ARNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string AR_NO = "AR_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 号機
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GOKI = "GOKI";
        /// --------------------------------------------------
        /// <summary>
        /// 日付種別
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DATE_KIND = "DATE_KIND";
        /// --------------------------------------------------
        /// <summary>
        /// 変更前日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DATE_BEFORE = "DATE_BEFORE";
        /// --------------------------------------------------
        /// <summary>
        /// 変更後日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DATE_AFTER = "DATE_AFTER";
        /// --------------------------------------------------
        /// <summary>
        /// 備考
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NOTE = "NOTE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_AR_SHINCHOKU_RIREKI"; }
        }

        #endregion
    }

    #endregion

    #region 一時取込データ(T_TEMPWORK)

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込データ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_TEMPWORK
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 一時取込ID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEMP_ID = "TEMP_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 取込日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TORIKOMI_DATE = "TORIKOMI_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 取込区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TORIKOMI_FLAG = "TORIKOMI_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 取込NO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DATA_NO = "DATA_NO";
        /// --------------------------------------------------
        /// <summary>
        /// エラー数
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ERROR_NUM = "ERROR_NUM";
        /// --------------------------------------------------
        /// <summary>
        /// 状態区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string STATUS_FLAG = "STATUS_FLAG";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_TEMPWORK"; }
        }

        #endregion
    }

    #endregion

    #region 一時取込明細データ(T_TEMPWORK_MEISAI)

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込明細データ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_TEMPWORK_MEISAI
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 一時取込ID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEMP_ID = "TEMP_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 行No
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ROW_NO = "ROW_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 結果
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string RESULT = "RESULT";
        /// --------------------------------------------------
        /// <summary>
        /// パレットNo
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PALLET_NO = "PALLET_NO";
        /// --------------------------------------------------
        /// <summary>
        /// BoxNo
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BOX_NO = "BOX_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 取込No
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DATA_NO = "DATA_NO";
        /// --------------------------------------------------
        /// <summary>
        /// エラー内容
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DESCRIPTION = "DESCRIPTION";
        /// --------------------------------------------------
        /// <summary>
        /// 納入先
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NONYUSAKI_NAME = "NONYUSAKI_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// ARNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string AR_NO = "AR_NO";
        /// --------------------------------------------------
        /// <summary>
        /// SHIP
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHIP = "SHIP";
        /// --------------------------------------------------
        /// <summary>
        /// 手配No
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEHAI_NO = "TEHAI_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 入荷数
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NYUKA_QTY = "NYUKA_QTY";
        /// --------------------------------------------------
        /// <summary>
        /// 重量
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string WEIGHT = "WEIGHT";
        /// --------------------------------------------------
        /// <summary>
        /// ハンディログインID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HANDY_LOGIN_ID = "HANDY_LOGIN_ID";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_TEMPWORK_MEISAI"; }
        }

        #endregion
    }

    #endregion

    #region 一時物件名マスタ(M_TEMPWORK_BUKKEN)

    /// --------------------------------------------------
    /// <summary>
    /// 一時物件名マスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_TEMPWORK_BUKKEN
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_FLAG = "SHUKKA_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 物件名称
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BUKKEN_NAME = "BUKKEN_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 発行済TagNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ISSUED_TAG_NO = "ISSUED_TAG_NO";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_TEMPWORK_BUKKEN"; }
        }

        #endregion
    }

    #endregion

    #region ロケーションマスタ(M_LOCATION)

    /// --------------------------------------------------
    /// <summary>
    /// ロケーションマスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_LOCATION
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_FLAG = "SHUKKA_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 物件管理NO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BUKKEN_NO = "BUKKEN_NO";
        /// --------------------------------------------------
        /// <summary>
        /// ロケーション
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LOCATION = "LOCATION";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_LOCATION"; }
        }

        #endregion
    }

    #endregion

    #region 在庫データ(T_STOCK)

    /// --------------------------------------------------
    /// <summary>
    /// 在庫データ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_STOCK
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// TAGCode
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TAG_CODE = "TAG_CODE";
        /// --------------------------------------------------
        /// <summary>
        /// 物件管理NO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BUKKEN_NO = "BUKKEN_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_FLAG = "SHUKKA_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NONYUSAKI_CD = "NONYUSAKI_CD";
        /// --------------------------------------------------
        /// <summary>
        /// TagNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TAG_NO = "TAG_NO";
        /// --------------------------------------------------
        /// <summary>
        /// BOXNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BOX_NO = "BOX_NO";
        /// --------------------------------------------------
        /// <summary>
        /// ロケーション
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LOCATION = "LOCATION";
        /// --------------------------------------------------
        /// <summary>
        /// 状態区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string STATUS = "STATUS";
        /// --------------------------------------------------
        /// <summary>
        /// 在庫日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string STOCK_DATE = "STOCK_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_STOCK"; }
        }

        #endregion
    }

    #endregion

    #region 棚卸データ(T_INVENT)

    /// --------------------------------------------------
    /// <summary>
    /// 棚卸データ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_INVENT
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 棚卸実施日
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string INVENT_DATE = "INVENT_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_FLAG = "SHUKKA_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 物件管理NO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BUKKEN_NO = "BUKKEN_NO";
        /// --------------------------------------------------
        /// <summary>
        /// ロケーション
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LOCATION = "LOCATION";
        /// --------------------------------------------------
        /// <summary>
        /// TAGCode
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TAG_CODE = "TAG_CODE";
        /// --------------------------------------------------
        /// <summary>
        /// 完了区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KANRYO_FLAG = "KANRYO_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NONYUSAKI_CD = "NONYUSAKI_CD";
        /// --------------------------------------------------
        /// <summary>
        /// TagNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TAG_NO = "TAG_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 作業ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string WORK_USER_ID = "WORK_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_INVENT"; }
        }

        #endregion
    }

    #endregion

    #region 実績データ(T_JISSEKI)

    /// --------------------------------------------------
    /// <summary>
    /// 実績データ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_JISSEKI
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 実績登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string JISSEKI_DATE = "JISSEKI_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 作業区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SAGYO_FLAG = "SAGYO_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// TAGCode
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TAG_CODE = "TAG_CODE";
        /// --------------------------------------------------
        /// <summary>
        /// 作業ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string WORK_USER_ID = "WORK_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 作業ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string WORK_USER_NAME = "WORK_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 物件管理NO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BUKKEN_NO = "BUKKEN_NO";
        /// --------------------------------------------------
        /// <summary>
        /// ロケーション
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LOCATION = "LOCATION";
        /// --------------------------------------------------
        /// <summary>
        /// 状態区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string STATUS = "STATUS";
        /// --------------------------------------------------
        /// <summary>
        /// 在庫日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string STOCK_DATE = "STOCK_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_FLAG = "SHUKKA_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NONYUSAKI_CD = "NONYUSAKI_CD";
        /// --------------------------------------------------
        /// <summary>
        /// 納入先
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NONYUSAKI_NAME = "NONYUSAKI_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// TagNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TAG_NO = "TAG_NO";
        /// --------------------------------------------------
        /// <summary>
        /// ARNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string AR_NO = "AR_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 製番
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SEIBAN = "SEIBAN";
        /// --------------------------------------------------
        /// <summary>
        /// CODE
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CODE = "CODE";
        /// --------------------------------------------------
        /// <summary>
        /// 図面追番
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ZUMEN_OIBAN = "ZUMEN_OIBAN";
        /// --------------------------------------------------
        /// <summary>
        /// Area
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string AREA = "AREA";
        /// --------------------------------------------------
        /// <summary>
        /// Floor
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLOOR = "FLOOR";
        /// --------------------------------------------------
        /// <summary>
        /// 機種
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KISHU = "KISHU";
        /// --------------------------------------------------
        /// <summary>
        /// STNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ST_NO = "ST_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 品名(和文)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HINMEI_JP = "HINMEI_JP";
        /// --------------------------------------------------
        /// <summary>
        /// 品名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HINMEI = "HINMEI";
        /// --------------------------------------------------
        /// <summary>
        /// 図面/形式
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ZUMEN_KEISHIKI = "ZUMEN_KEISHIKI";
        /// --------------------------------------------------
        /// <summary>
        /// 区割NO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KUWARI_NO = "KUWARI_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 数量
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NUM = "NUM";
        /// --------------------------------------------------
        /// <summary>
        /// 状態区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string JYOTAI_FLAG = "JYOTAI_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// TAG発行区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TAGHAKKO_FLAG = "TAGHAKKO_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// TAG発行日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TAGHAKKO_DATE = "TAGHAKKO_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 集荷日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKA_DATE = "SHUKA_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// BOXNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BOX_NO = "BOX_NO";
        /// --------------------------------------------------
        /// <summary>
        /// BOX梱包日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BOXKONPO_DATE = "BOXKONPO_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// パレットNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PALLET_NO = "PALLET_NO";
        /// --------------------------------------------------
        /// <summary>
        /// パレット梱包日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PALLETKONPO_DATE = "PALLETKONPO_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 工事識別管理NO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KOJI_NO = "KOJI_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 木枠梱包内部管理キー
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CASE_ID = "CASE_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 木枠梱包日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KIWAKUKONPO_DATE = "KIWAKUKONPO_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_DATE = "SHUKKA_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_USER_ID = "SHUKKA_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_USER_NAME = "SHUKKA_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 運送会社
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UNSOKAISHA_NAME = "UNSOKAISHA_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// インボイスNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string INVOICE_NO = "INVOICE_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 送り状NO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string OKURIJYO_NO = "OKURIJYO_NO";
        /// --------------------------------------------------
        /// <summary>
        /// BLNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BL_NO = "BL_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 受入日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UKEIRE_DATE = "UKEIRE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 受入ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UKEIRE_USER_ID = "UKEIRE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 受入ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UKEIRE_USER_NAME = "UKEIRE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// BIKO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BIKO = "BIKO";
        /// --------------------------------------------------
        /// <summary>
        /// MNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string M_NO = "M_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 通関確認状態
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CUSTOMS_STATUS = "CUSTOMS_STATUS";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_JISSEKI"; }
        }

        #endregion
    }

    #endregion

    #region 一時取込データ（部品管理用）(T_BUHIN_TEMPWORK)

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込データ（部品管理用）
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_BUHIN_TEMPWORK
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 一時取込ID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEMP_ID = "TEMP_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 取込区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TORIKOMI_FLAG = "TORIKOMI_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 作業ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string WORK_USER_ID = "WORK_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 取込日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TORIKOMI_DATE = "TORIKOMI_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// ロケーション
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LOCATION = "LOCATION";
        /// --------------------------------------------------
        /// <summary>
        /// 取込No
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string STOCK_NO = "STOCK_NO";
        /// --------------------------------------------------
        /// <summary>
        /// エラー数
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ERROR_NUM = "ERROR_NUM";
        /// --------------------------------------------------
        /// <summary>
        /// 状態区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string STATUS_FLAG = "STATUS_FLAG";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_BUHIN_TEMPWORK"; }
        }

        #endregion
    }

    #endregion

    #region 一時取込明細データ（部品管理用）(T_BUHIN_TEMPWORK_MEISAI)

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込明細データ（部品管理用）
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_BUHIN_TEMPWORK_MEISAI
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 一時取込ID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEMP_ID = "TEMP_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 取込区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TORIKOMI_FLAG = "TORIKOMI_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 行No
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ROW_NO = "ROW_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 作業ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string WORK_USER_ID = "WORK_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 結果
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string RESULT = "RESULT";
        /// --------------------------------------------------
        /// <summary>
        /// 作業日付
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string WORK_DATE = "WORK_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// ロケーション
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LOCATION = "LOCATION";
        /// --------------------------------------------------
        /// <summary>
        /// 取込No
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string STOCK_NO = "STOCK_NO";
        /// --------------------------------------------------
        /// <summary>
        /// エラー内容
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DESCRIPTION = "DESCRIPTION";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_FLAG = "SHUKKA_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 物件管理NO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BUKKEN_NO = "BUKKEN_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NONYUSAKI_CD = "NONYUSAKI_CD";
        /// --------------------------------------------------
        /// <summary>
        /// TagNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TAG_NO = "TAG_NO";
        /// --------------------------------------------------
        /// <summary>
        /// BOXNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BOX_NO = "BOX_NO";
        /// --------------------------------------------------
        /// <summary>
        /// パレットNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PALLET_NO = "PALLET_NO";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_BUHIN_TEMPWORK_MEISAI"; }
        }

        #endregion
    }

    #endregion

    #region メールデータ(T_MAIL)

    /// --------------------------------------------------
    /// <summary>
    /// メールデータ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_MAIL
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// メールID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAIL_ID = "MAIL_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 送信者
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAIL_SEND = "MAIL_SEND";
        /// --------------------------------------------------
        /// <summary>
        /// 送信者名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAIL_SEND_DISPLAY = "MAIL_SEND_DISPLAY";
        /// --------------------------------------------------
        /// <summary>
        /// 宛先
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAIL_TO = "MAIL_TO";
        /// --------------------------------------------------
        /// <summary>
        /// 宛先表示名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAIL_TO_DISPLAY = "MAIL_TO_DISPLAY";
        /// --------------------------------------------------
        /// <summary>
        /// CC
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAIL_CC = "MAIL_CC";
        /// --------------------------------------------------
        /// <summary>
        /// CC表示名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAIL_CC_DISPLAY = "MAIL_CC_DISPLAY";
        /// --------------------------------------------------
        /// <summary>
        /// BCC
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAIL_BCC = "MAIL_BCC";
        /// --------------------------------------------------
        /// <summary>
        /// BCC表示名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAIL_BCC_DISPLAY = "MAIL_BCC_DISPLAY";
        /// --------------------------------------------------
        /// <summary>
        /// 件名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TITLE = "TITLE";
        /// --------------------------------------------------
        /// <summary>
        /// 本文
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NAIYO = "NAIYO";
        /// --------------------------------------------------
        /// <summary>
        /// 状態
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAIL_STATUS = "MAIL_STATUS";
        /// --------------------------------------------------
        /// <summary>
        /// 失敗回数
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string RETRY_COUNT = "RETRY_COUNT";
        /// --------------------------------------------------
        /// <summary>
        /// 失敗理由
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string REASON = "REASON";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";
        /// --------------------------------------------------
        /// <summary>
        /// 言語
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LANG = "LANG";
        /// --------------------------------------------------
        /// <summary>
        /// 添付ファイル
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string APPENDIX_FILE_PATH = "APPENDIX_FILE_PATH";
        /// --------------------------------------------------
        /// <summary>
        /// 表示用失敗理由
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DISP_REASON = "DISP_REASON";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_MAIL"; }
        }

        #endregion
    }

    #endregion

    #region 物件メールマスタ(M_BUKKEN_MAIL)

    /// --------------------------------------------------
    /// <summary>
    /// 物件メールマスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_BUKKEN_MAIL
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_FLAG = "SHUKKA_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 物件管理NO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BUKKEN_NO = "BUKKEN_NO";
        /// --------------------------------------------------
        /// <summary>
        /// メール区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAIL_KBN = "MAIL_KBN";
        /// --------------------------------------------------
        /// <summary>
        /// リスト区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LIST_FLAG = "LIST_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// メールヘッダID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAIL_HEADER_ID = "MAIL_HEADER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_BUKKEN_MAIL"; }
        }

        #endregion
    }

    #endregion

    #region 物件メール明細マスタ(M_BUKKEN_MAIL_MEISAI)

    /// --------------------------------------------------
    /// <summary>
    /// 物件メール明細マスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_BUKKEN_MAIL_MEISAI
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// メールヘッダID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAIL_HEADER_ID = "MAIL_HEADER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// メールフラグ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAIL_ADDRESS_FLAG = "MAIL_ADDRESS_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// オーダー順
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ORDER_NO = "ORDER_NO";
        /// --------------------------------------------------
        /// <summary>
        /// ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string USER_ID = "USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_BUKKEN_MAIL_MEISAI"; }
        }

        #endregion
    }

    #endregion

    #region メール設定マスタ(M_MAIL_SETTING)

    /// --------------------------------------------------
    /// <summary>
    /// メール設定マスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_MAIL_SETTING
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// メール再送信上限回数
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAIL_RETRY = "MAIL_RETRY";
        /// --------------------------------------------------
        /// <summary>
        /// メール送信間隔(秒)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAIL_SPAN = "MAIL_SPAN";
        /// --------------------------------------------------
        /// <summary>
        /// SMTPサーバー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SMTP_SERVER = "SMTP_SERVER";
        /// --------------------------------------------------
        /// <summary>
        /// SMTPポート
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SMTP_PORT = "SMTP_PORT";
        /// --------------------------------------------------
        /// <summary>
        /// 物件追加イベント
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BUKKEN_ADD_EVENT = "BUKKEN_ADD_EVENT";
        /// --------------------------------------------------
        /// <summary>
        /// AR追加イベント
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string AR_ADD_EVENT = "AR_ADD_EVENT";
        /// --------------------------------------------------
        /// <summary>
        /// AR更新イベント
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string AR_UPDATE_EVENT = "AR_UPDATE_EVENT";
        /// --------------------------------------------------
        /// <summary>
        /// メールテンプレートフォルダ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEMPLATE_FOLDER = "TEMPLATE_FOLDER";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";
        /// --------------------------------------------------
        /// <summary>
        /// TAG連携イベント
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TAG_RENKEI_EVENT = "TAG_RENKEI_EVENT";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_MAIL_SETTING"; }
        }

        #endregion
    }

    #endregion

    #region 荷受マスタ(M_CONSIGN)

    /// --------------------------------------------------
    /// <summary>
    /// 荷受マスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_CONSIGN
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 荷受CD
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CONSIGN_CD = "CONSIGN_CD";
        /// --------------------------------------------------
        /// <summary>
        /// 名称
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NAME = "NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 住所
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ADDRESS = "ADDRESS";
        /// --------------------------------------------------
        /// <summary>
        /// TEL1
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEL1 = "TEL1";
        /// --------------------------------------------------
        /// <summary>
        /// TEL2
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEL2 = "TEL2";
        /// --------------------------------------------------
        /// <summary>
        /// FAX
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FAX = "FAX";
        /// --------------------------------------------------
        /// <summary>
        /// 中国向けフラグ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CHINA_FLAG = "CHINA_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// USCI CODE
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string USCI_CD = "USCI_CD";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 保守日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAINTE_DATE = "MAINTE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 保守ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAINTE_USER_ID = "MAINTE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 保守ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAINTE_USER_NAME = "MAINTE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";
        /// --------------------------------------------------
        /// <summary>
        /// 並び順
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SORT_NO = "SORT_NO";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_CONSIGN"; }
        }

        #endregion
    }

    #endregion

    #region 配送先マスタ(M_DELIVER)

    /// --------------------------------------------------
    /// <summary>
    /// 配送先マスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_DELIVER
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 配送先CD
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DELIVER_CD = "DELIVER_CD";
        /// --------------------------------------------------
        /// <summary>
        /// 名称
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NAME = "NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 住所
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ADDRESS = "ADDRESS";
        /// --------------------------------------------------
        /// <summary>
        /// TEL
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEL1 = "TEL1";
        /// --------------------------------------------------
        /// <summary>
        /// TEL2
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEL2 = "TEL2";
        /// --------------------------------------------------
        /// <summary>
        /// FAX
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FAX = "FAX";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 保守日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAINTE_DATE = "MAINTE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 保守ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAINTE_USER_ID = "MAINTE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 保守ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAINTE_USER_NAME = "MAINTE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";
        /// --------------------------------------------------
        /// <summary>
        /// 並び順
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SORT_NO = "SORT_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷物
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHIPPING_ITEM = "SHIPPING_ITEM";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHIPPING_TYPE = "SHIPPING_TYPE";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷先担当者
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHIPPING_CONTACT = "SHIPPING_CONTACT";
        /// --------------------------------------------------
        /// <summary>
        /// 受取不可日通常
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UNAVAIL_NORM = "UNAVAIL_NORM";
        /// --------------------------------------------------
        /// <summary>
        /// 受取不可日正月連休
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UNAVAIL_NY = "UNAVAIL_NY";
        /// --------------------------------------------------
        /// <summary>
        /// 受取不可日5月連休
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UNAVAIL_MAY = "UNAVAIL_MAY";
        /// --------------------------------------------------
        /// <summary>
        /// 受取不可日8月連休
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UNAVAIL_AUG = "UNAVAIL_AUG";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_DELIVER"; }
        }

        #endregion
    }

    #endregion

    #region 技連マスタ(M_ECS)

    /// --------------------------------------------------
    /// <summary>
    /// 技連マスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_ECS
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 期
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ECS_QUOTA = "ECS_QUOTA";
        /// --------------------------------------------------
        /// <summary>
        /// ECSNo
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ECS_NO = "ECS_NO";
        /// --------------------------------------------------
        /// <summary>
        /// プロジェクトNo
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PROJECT_NO = "PROJECT_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 製番
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SEIBAN = "SEIBAN";
        /// --------------------------------------------------
        /// <summary>
        /// CODE
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CODE = "CODE";
        /// --------------------------------------------------
        /// <summary>
        /// 機種
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KISHU = "KISHU";
        /// --------------------------------------------------
        /// <summary>
        /// 製番CODE
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SEIBAN_CODE = "SEIBAN_CODE";
        /// --------------------------------------------------
        /// <summary>
        /// ARNo
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string AR_NO = "AR_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 管理区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KANRI_FLAG = "KANRI_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 保守日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAINTE_DATE = "MAINTE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 保守ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAINTE_USER_ID = "MAINTE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 保守ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAINTE_USER_NAME = "MAINTE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_ECS"; }
        }

        #endregion
    }

    #endregion

    #region パーツ名翻訳マスタ(M_PARTS_NAME)

    /// --------------------------------------------------
    /// <summary>
    /// パーツ名翻訳マスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_PARTS_NAME
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 型式
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PARTS_CD = "PARTS_CD";
        /// --------------------------------------------------
        /// <summary>
        /// 品名(和文)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HINMEI_JP = "HINMEI_JP";
        /// --------------------------------------------------
        /// <summary>
        /// 品名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HINMEI = "HINMEI";
        /// --------------------------------------------------
        /// <summary>
        /// INV付加名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HINMEI_INV = "HINMEI_INV";
        /// --------------------------------------------------
        /// <summary>
        /// Maker
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAKER = "MAKER";
        /// --------------------------------------------------
        /// <summary>
        /// 原産国
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FREE2 = "FREE2";
        /// --------------------------------------------------
        /// <summary>
        /// 取引先
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SUPPLIER = "SUPPLIER";
        /// --------------------------------------------------
        /// <summary>
        /// 備考
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NOTE = "NOTE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";
        /// --------------------------------------------------
        /// <summary>
        /// 通関確認状態
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CUSTOMS_STATUS = "CUSTOMS_STATUS";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_PARTS_NAME"; }
        }

        #endregion
    }

    #endregion

    #region 運送会社マスタ(M_UNSOKAISHA)

    /// --------------------------------------------------
    /// <summary>
    /// 運送会社マスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_UNSOKAISHA
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 運送会社CD
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UNSOKAISHA_NO = "UNSOKAISHA_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 国内外
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KOKUNAI_GAI_FLAG = "KOKUNAI_GAI_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 名称
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UNSOKAISHA_NAME = "UNSOKAISHA_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// Invoice有無
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string INVOICE_FLAG = "INVOICE_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// PackingList有無
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PACKINGLIST_FLAG = "PACKINGLIST_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 輸出通関確認書有無
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string EXPORTCONFIRM_FLAG = "EXPORTCONFIRM_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 輸出通関確認書宛名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string EXPORTCONFIRM_ATTN = "EXPORTCONFIRM_ATTN";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 保守日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAINTE_DATE = "MAINTE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 保守ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAINTE_USER_ID = "MAINTE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 保守ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAINTE_USER_NAME = "MAINTE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";
        /// --------------------------------------------------
        /// <summary>
        /// 並び順
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SORT_NO = "SORT_NO";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_UNSOKAISHA"; }
        }

        #endregion
    }

    #endregion

    #region 荷姿(T_PACKING)

    /// --------------------------------------------------
    /// <summary>
    /// 荷姿
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_PACKING
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 荷姿CD
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PACKING_NO = "PACKING_NO";
        /// --------------------------------------------------
        /// <summary>
        /// CTQty
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CT_QTY = "CT_QTY";
        /// --------------------------------------------------
        /// <summary>
        /// InvoiceNo
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string INVOICE_NO = "INVOICE_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷日
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SYUKKA_DATE = "SYUKKA_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 発行状態
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HAKKO_FLAG = "HAKKO_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 運送会社CD
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UNSOKAISHA_CD = "UNSOKAISHA_CD";
        /// --------------------------------------------------
        /// <summary>
        /// 荷受CD
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CONSIGN_CD = "CONSIGN_CD";
        /// --------------------------------------------------
        /// <summary>
        /// 荷受宛名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CONSIGN_ATTN = "CONSIGN_ATTN";
        /// --------------------------------------------------
        /// <summary>
        /// 配送先CD
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DELIVER_CD = "DELIVER_CD";
        /// --------------------------------------------------
        /// <summary>
        /// 配送先宛名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DELIVER_ATTN = "DELIVER_ATTN";
        /// --------------------------------------------------
        /// <summary>
        /// 荷姿メール件名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PACKING_MAIL_SUBJECT = "PACKING_MAIL_SUBJECT";
        /// --------------------------------------------------
        /// <summary>
        /// 荷姿Rev
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PACKING_REV = "PACKING_REV";
        /// --------------------------------------------------
        /// <summary>
        /// 内部見積番号
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string INTERNAL_PO_NO = "INTERNAL_PO_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 貿易条件
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TRADE_TERMS_FLAG = "TRADE_TERMS_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 貿易条件付加情報
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TRADE_TERMS_ATTR = "TRADE_TERMS_ATTR";
        /// --------------------------------------------------
        /// <summary>
        /// タイトル
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SUBJECT = "SUBJECT";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷元CD
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHIP_FROM_CD = "SHIP_FROM_CD";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_PACKING"; }
        }

        #endregion
    }

    #endregion

    #region 荷姿明細(T_PACKING_MEISAI)

    /// --------------------------------------------------
    /// <summary>
    /// 荷姿明細
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_PACKING_MEISAI
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 荷姿CD
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PACKING_NO = "PACKING_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 連番
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NO = "NO";
        /// --------------------------------------------------
        /// <summary>
        /// CTNo
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CT_NO = "CT_NO";
        /// --------------------------------------------------
        /// <summary>
        /// キャンセルフラグ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CANCEL_FLAG = "CANCEL_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// AR便(出荷区分)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_FLAG = "SHUKKA_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NONYUSAKI_CD = "NONYUSAKI_CD";
        /// --------------------------------------------------
        /// <summary>
        /// ARNo
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string AR_NO = "AR_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 技連No
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ECS_NO = "ECS_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 製番CODE
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SEIBAN_CODE = "SEIBAN_CODE";
        /// --------------------------------------------------
        /// <summary>
        /// 同梱
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DOUKON_FLAG = "DOUKON_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 定形
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FORM_STYLE_FLAG = "FORM_STYLE_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// SizeL
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SIZE_L = "SIZE_L";
        /// --------------------------------------------------
        /// <summary>
        /// SizeW
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SIZE_W = "SIZE_W";
        /// --------------------------------------------------
        /// <summary>
        /// SizeH
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SIZE_H = "SIZE_H";
        /// --------------------------------------------------
        /// <summary>
        /// GRWT
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GRWT = "GRWT";
        /// --------------------------------------------------
        /// <summary>
        /// 製品名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PRODUCT_NAME = "PRODUCT_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 宛先
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ATTN = "ATTN";
        /// --------------------------------------------------
        /// <summary>
        /// 備考
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NOTE = "NOTE";
        /// --------------------------------------------------
        /// <summary>
        /// PLType
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PL_TYPE = "PL_TYPE";
        /// --------------------------------------------------
        /// <summary>
        /// C/No
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CASE_NO = "CASE_NO";
        /// --------------------------------------------------
        /// <summary>
        /// パレットNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PALLET_NO = "PALLET_NO";
        /// --------------------------------------------------
        /// <summary>
        /// BOXNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BOX_NO = "BOX_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_PACKING_MEISAI"; }
        }

        #endregion
    }

    #endregion

    #region 手配明細見積(T_TEHAI_ESTIMATE)

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細見積
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_TEHAI_ESTIMATE
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 見積No
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ESTIMATE_NO = "ESTIMATE_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 名称
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NAME = "NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 通貨
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CURRENCY_FLAG = "CURRENCY_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// ER(JPY)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string RATE_JPY = "RATE_JPY";
        /// --------------------------------------------------
        /// <summary>
        /// 販管
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SALSES_PER = "SALSES_PER";
        /// --------------------------------------------------
        /// <summary>
        /// 運賃
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ROB_PER = "ROB_PER";
        /// --------------------------------------------------
        /// <summary>
        /// PONo.
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PO_NO = "PO_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";
        /// --------------------------------------------------
        /// <summary>
        /// PO金額
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PO_AMOUNT = "PO_AMOUNT";
        /// --------------------------------------------------
        /// <summary>
        /// 仕切り金額
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PARTITION_AMOUNT = "PARTITION_AMOUNT";
        /// --------------------------------------------------
        /// <summary>
        /// 仕切りレート
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string RATE_PARTITION = "RATE_PARTITION";
        /// --------------------------------------------------
        /// <summary>
        /// 売上予定月
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PROJECTED_SALES_MONTH = "PROJECTED_SALES_MONTH";
        /// --------------------------------------------------
        /// <summary>
        /// Mail Title
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAIL_TITLE = "MAIL_TITLE";
        /// --------------------------------------------------
        /// <summary>
        /// Rev
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string REV = "REV";
        /// --------------------------------------------------
        /// <summary>
        /// 荷受CD
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CONSIGN_CD = "CONSIGN_CD";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_TEHAI_ESTIMATE"; }
        }

        #endregion
    }

    #endregion

    #region 手配明細(T_TEHAI_MEISAI)

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_TEHAI_MEISAI
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 連携No
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEHAI_RENKEI_NO = "TEHAI_RENKEI_NO";
        /// --------------------------------------------------
        /// <summary>
        /// ECS 期
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ECS_QUOTA = "ECS_QUOTA";
        /// --------------------------------------------------
        /// <summary>
        /// ECSNo
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ECS_NO = "ECS_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 設定納期
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SETTEI_DATE = "SETTEI_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 納品先
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NOUHIN_SAKI = "NOUHIN_SAKI";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷先
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SYUKKA_SAKI = "SYUKKA_SAKI";
        /// --------------------------------------------------
        /// <summary>
        /// 図面追番
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ZUMEN_OIBAN = "ZUMEN_OIBAN";
        /// --------------------------------------------------
        /// <summary>
        /// Floor
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLOOR = "FLOOR";
        /// --------------------------------------------------
        /// <summary>
        /// STNo
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ST_NO = "ST_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 品名(和文)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HINMEI_JP = "HINMEI_JP";
        /// --------------------------------------------------
        /// <summary>
        /// 品名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HINMEI = "HINMEI";
        /// --------------------------------------------------
        /// <summary>
        /// INV 付加名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HINMEI_INV = "HINMEI_INV";
        /// --------------------------------------------------
        /// <summary>
        /// 図番/型式
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ZUMEN_KEISHIKI = "ZUMEN_KEISHIKI";
        /// --------------------------------------------------
        /// <summary>
        /// 図番/型式検索
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ZUMEN_KEISHIKI_S = "ZUMEN_KEISHIKI_S";
        /// --------------------------------------------------
        /// <summary>
        /// 手配数
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEHAI_QTY = "TEHAI_QTY";
        /// --------------------------------------------------
        /// <summary>
        /// 手配区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEHAI_FLAG = "TEHAI_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 手配種別
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEHAI_KIND_FLAG = "TEHAI_KIND_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 発注数
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HACCHU_QTY = "HACCHU_QTY";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷数
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_QTY = "SHUKKA_QTY";
        /// --------------------------------------------------
        /// <summary>
        /// Free1
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FREE1 = "FREE1";
        /// --------------------------------------------------
        /// <summary>
        /// Free2
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FREE2 = "FREE2";
        /// --------------------------------------------------
        /// <summary>
        /// 数量単位
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string QUANTITY_UNIT = "QUANTITY_UNIT";
        /// --------------------------------------------------
        /// <summary>
        /// 図番/型式2
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ZUMEN_KEISHIKI2 = "ZUMEN_KEISHIKI2";
        /// --------------------------------------------------
        /// <summary>
        /// 備考
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NOTE = "NOTE";
        /// --------------------------------------------------
        /// <summary>
        /// Maker
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAKER = "MAKER";
        /// --------------------------------------------------
        /// <summary>
        /// 単価(JPY)
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UNIT_PRICE = "UNIT_PRICE";
        /// --------------------------------------------------
        /// <summary>
        /// INVOICE単価
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string INVOICE_UNIT_PRICE = "INVOICE_UNIT_PRICE";
        /// --------------------------------------------------
        /// <summary>
        /// 入荷数
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ARRIVAL_QTY = "ARRIVAL_QTY";
        /// --------------------------------------------------
        /// <summary>
        /// 組立数
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ASSY_QTY = "ASSY_QTY";
        /// --------------------------------------------------
        /// <summary>
        /// 有償
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ESTIMATE_FLAG = "ESTIMATE_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 見積No
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ESTIMATE_NO = "ESTIMATE_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";
        /// --------------------------------------------------
        /// <summary>
        /// 組立No
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ASSY_NO = "ASSY_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 備考2
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NOTE2 = "NOTE2";
        /// --------------------------------------------------
        /// <summary>
        /// 備考3
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NOTE3 = "NOTE3";
        /// --------------------------------------------------
        /// <summary>
        /// 返却品
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HENKYAKUHIN_FLAG = "HENKYAKUHIN_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 販管費レート
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SGA_RATE = "SGA_RATE";
        /// --------------------------------------------------
        /// <summary>
        /// 表示順
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DISP_NO = "DISP_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 運賃レート
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHIPPING_RATE = "SHIPPING_RATE";
        /// --------------------------------------------------
        /// <summary>
        /// 見積更新履歴
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ESTIMATE_RIREKI = "ESTIMATE_RIREKI";
        /// --------------------------------------------------
        /// <summary>
        /// Invoice Value
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string INVOICE_VALUE = "INVOICE_VALUE";
        /// --------------------------------------------------
        /// <summary>
        /// 手配更新履歴
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEHAI_RIREKI = "TEHAI_RIREKI";
        /// --------------------------------------------------
        /// <summary>
        /// 通関確認状態
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CUSTOMS_STATUS = "CUSTOMS_STATUS";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_TEHAI_MEISAI"; }
        }

        #endregion
    }

    #endregion

    #region 手配SKS連携(T_TEHAI_MEISAI_SKS)

    /// --------------------------------------------------
    /// <summary>
    /// 手配SKS連携
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_TEHAI_MEISAI_SKS
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 連携No
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEHAI_RENKEI_NO = "TEHAI_RENKEI_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 手配No
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEHAI_NO = "TEHAI_NO";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_TEHAI_MEISAI_SKS"; }
        }

        #endregion
    }

    #endregion

    #region SKS手配明細(T_TEHAI_SKS)

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_TEHAI_SKS
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 手配No
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEHAI_NO = "TEHAI_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 手配数量
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEHAI_QTY = "TEHAI_QTY";
        /// --------------------------------------------------
        /// <summary>
        /// 手配単価
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEHAI_UNIT_PRICE = "TEHAI_UNIT_PRICE";
        /// --------------------------------------------------
        /// <summary>
        /// 回答納期
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KAITO_DATE = "KAITO_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// SKS連携日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SKS_UPDATE_DATE = "SKS_UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 図面追番
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ZUMEN_OIBAN = "ZUMEN_OIBAN";
        /// --------------------------------------------------
        /// <summary>
        /// 発注状態
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HACCHU_ZYOTAI_FLAG = "HACCHU_ZYOTAI_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 製番CODE
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SEIBAN_CODE = "SEIBAN_CODE";
        /// --------------------------------------------------
        /// <summary>
        /// 品番
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HINBAN = "HINBAN";
        /// --------------------------------------------------
        /// <summary>
        /// PDM作業名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PDM_WORK_NAME = "PDM_WORK_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 型式
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KATASHIKI = "KATASHIKI";
        /// --------------------------------------------------
        /// <summary>
        /// 技連No
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ECS_NO = "ECS_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 検品有無
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KENPIN_UMU = "KENPIN_UMU";
        /// --------------------------------------------------
        /// <summary>
        /// 入荷数
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ARRIVAL_QTY = "ARRIVAL_QTY";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";
        /// --------------------------------------------------
        /// <summary>
        /// 発注フラグ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HACCHU_FLAG = "HACCHU_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 受入日
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UKEIRE_DATE = "UKEIRE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 検収日
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KENSHU_DATE = "KENSHU_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 前回発注フラグ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ZENKAI_HACCHU_FLAG = "ZENKAI_HACCHU_FLAG";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_TEHAI_SKS"; }
        }

        #endregion
    }

    #endregion

    #region SKS手配明細WORK(T_TEHAI_SKS_WORK)

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細WORK
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_TEHAI_SKS_WORK
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 製番CODE
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SEIBAN_CODE = "SEIBAN_CODE";
        /// --------------------------------------------------
        /// <summary>
        /// 品番
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HINBAN = "HINBAN";
        /// --------------------------------------------------
        /// <summary>
        /// PDM作業名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PDM_WORK_NAME = "PDM_WORK_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 手配No
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEHAI_NO = "TEHAI_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 手配数量
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEHAI_QTY = "TEHAI_QTY";
        /// --------------------------------------------------
        /// <summary>
        /// 型式
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KATASHIKI = "KATASHIKI";
        /// --------------------------------------------------
        /// <summary>
        /// 回答納期
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KAITO_DATE = "KAITO_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 手配単価
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEHAI_UNIT_PRICE = "TEHAI_UNIT_PRICE";
        /// --------------------------------------------------
        /// <summary>
        /// 技連No
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ECS_NO = "ECS_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 追番
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ZUMEN_OIBAN = "ZUMEN_OIBAN";
        /// --------------------------------------------------
        /// <summary>
        /// 発注状態フラグ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HACCHU_ZYOTAI_FLAG = "HACCHU_ZYOTAI_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 図番型式検索
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ZUMEN_KEISHIKI_S = "ZUMEN_KEISHIKI_S";
        /// --------------------------------------------------
        /// <summary>
        /// SKS連携日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// Customer名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CUSTOMER_NAME = "CUSTOMER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 納入場所
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NONYUBASHO = "NONYUBASHO";
        /// --------------------------------------------------
        /// <summary>
        /// 注文書品目名称
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CHUMONSHO_HINMOKU = "CHUMONSHO_HINMOKU";
        /// --------------------------------------------------
        /// <summary>
        /// 発注フラグ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HACCHU_FLAG = "HACCHU_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 受入日
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UKEIRE_DATE = "UKEIRE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 検収日
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KENSHU_DATE = "KENSHU_DATE";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_TEHAI_SKS_WORK"; }
        }

        #endregion
    }

    #endregion

    #region SKS連携マスタ(M_SKS)

    /// --------------------------------------------------
    /// <summary>
    /// SKS連携マスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_SKS
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 処理フラグ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHORI_FLAG = "SHORI_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// SKS最終連携日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LASTEST_DATE = "LASTEST_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 開始時間
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string START_TIME = "START_TIME";
        /// --------------------------------------------------
        /// <summary>
        /// SKS連携フォルダパス
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SKS_FOLDER = "SKS_FOLDER";
        /// --------------------------------------------------
        /// <summary>
        /// SKS連携ファイル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SKS_FILE_NAME = "SKS_FILE_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// SKS連携ログ出力先フォルダ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SKS_LOG_FOLDER = "SKS_LOG_FOLDER";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_SKS"; }
        }

        #endregion
    }

    #endregion

    #region Projectマスタ(M_PROJECT)

    /// --------------------------------------------------
    /// <summary>
    /// Projectマスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_PROJECT
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// ProjectNo
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PROJECT_NO = "PROJECT_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 物件名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BUKKEN_NAME = "BUKKEN_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 保守日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAINTE_DATE = "MAINTE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 保守ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAINTE_USER_ID = "MAINTE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 保守ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAINTE_USER_NAME = "MAINTE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";
        /// --------------------------------------------------
        /// <summary>
        /// 受注No
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string JUCHU_NO = "JUCHU_NO";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_PROJECT"; }
        }

        #endregion
    }

    #endregion

    #region 図番/型式画像ファイル管理データ(T_MANAGE_ZUMEN_KEISHIKI)

    /// --------------------------------------------------
    /// <summary>
    /// 図番/型式画像ファイル管理データ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_MANAGE_ZUMEN_KEISHIKI
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 図番/型式
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ZUMEN_KEISHIKI = "ZUMEN_KEISHIKI";
        /// --------------------------------------------------
        /// <summary>
        /// ファイル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FILE_NAME = "FILE_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 格納ディレクトリ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SAVE_DIR = "SAVE_DIR";
        /// --------------------------------------------------
        /// <summary>
        /// 画像登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_MANAGE_ZUMEN_KEISHIKI"; }
        }

        #endregion
    }

    #endregion

    #region ハンディ用履歴データ(T_HANDY_RIREKI)

    /// --------------------------------------------------
    /// <summary>
    /// ハンディ用履歴データ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_T_HANDY_RIREKI
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// ハンディ操作区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HANDY_OPERATION_FLAG = "HANDY_OPERATION_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// ハンディログインID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HANDY_LOGIN_ID = "HANDY_LOGIN_ID";
        /// --------------------------------------------------
        /// <summary>
        /// プロジェクトNo
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PROJECT_NO = "PROJECT_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKA_FLAG = "SHUKKA_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 物件管理NO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BUKKEN_NO = "BUKKEN_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 物件名称
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BUKKEN_NAME = "BUKKEN_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NONYUSAKI_CD = "NONYUSAKI_CD";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷便
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHIP = "SHIP";
        /// --------------------------------------------------
        /// <summary>
        /// ARNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string AR_NO = "AR_NO";
        /// --------------------------------------------------
        /// <summary>
        /// TagNO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TAG_NO = "TAG_NO";
        /// --------------------------------------------------
        /// <summary>
        /// BoxNo
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BOX_NO = "BOX_NO";
        /// --------------------------------------------------
        /// <summary>
        /// パレットNo
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PALLET_NO = "PALLET_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 重量
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string WEIGHT = "WEIGHT";
        /// --------------------------------------------------
        /// <summary>
        /// 手配No
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEHAI_NO = "TEHAI_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 入荷検品数
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NYUKA_QTY = "NYUKA_QTY";
        /// --------------------------------------------------
        /// <summary>
        /// 取込日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string IMPORT_DATE = "IMPORT_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 取込ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string IMPORT_USER_ID = "IMPORT_USER_ID";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "T_HANDY_RIREKI"; }
        }

        #endregion
    }

    #endregion

    #region 出荷元マスタ(M_SHIP_FROM)

    /// --------------------------------------------------
    /// <summary>
    /// 出荷元マスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_SHIP_FROM
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 出荷元CD
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHIP_FROM_NO = "SHIP_FROM_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 社内外フラグ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHANAIGAI_FLAG = "SHANAIGAI_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 名称
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHIP_FROM_NAME = "SHIP_FROM_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 非表示フラグ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UNUSED_FLAG = "UNUSED_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 表示順
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DISP_NO = "DISP_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_SHIP_FROM"; }
        }

        #endregion
    }

    #endregion

    #region 荷受メールマスタ(M_CONSIGN_MAIL)

    /// --------------------------------------------------
    /// <summary>
    /// 荷受メールマスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_CONSIGN_MAIL
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 荷受CD
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CONSIGN_CD = "CONSIGN_CD";
        /// --------------------------------------------------
        /// <summary>
        /// メールヘッダID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAIL_HEADER_ID = "MAIL_HEADER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_CONSIGN_MAIL"; }
        }

        #endregion
    }

    #endregion

    #region 荷受メール明細マスタ(M_CONSIGN_MAIL_MEISAI)

    /// --------------------------------------------------
    /// <summary>
    /// 荷受メール明細マスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_CONSIGN_MAIL_MEISAI
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// メールヘッダID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAIL_HEADER_ID = "MAIL_HEADER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// メールフラグ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string MAIL_ADDRESS_FLAG = "MAIL_ADDRESS_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// オーダー順
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ORDER_NO = "ORDER_NO";
        /// --------------------------------------------------
        /// <summary>
        /// ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string USER_ID = "USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_CONSIGN_MAIL_MEISAI"; }
        }

        #endregion
    }

    #endregion

    #region メール送信履歴マスタ(M_MAIL_SEND_RIREKI)

    /// --------------------------------------------------
    /// <summary>
    /// メール送信履歴マスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_MAIL_SEND_RIREKI
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 物件管理NO
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BUKKEN_NO = "BUKKEN_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 荷受CD
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CONSIGN_CD = "CONSIGN_CD";
        /// --------------------------------------------------
        /// <summary>
        /// 運賃梱包製番
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHIP_SEIBAN = "SHIP_SEIBAN";
        /// --------------------------------------------------
        /// <summary>
        /// 物件Rev
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BUKKEN_REV = "BUKKEN_REV";
        /// --------------------------------------------------
        /// <summary>
        /// アサインコメント
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ASSIGN_COMMENT = "ASSIGN_COMMENT";
        /// --------------------------------------------------
        /// <summary>
        /// 登録日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_DATE = "CREATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CREATE_USER_NAME = "CREATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_MAIL_SEND_RIREKI"; }
        }

        #endregion
    }

    #endregion

    #region 手配明細履歴マスタ(M_TEHAI_MEISAI_RIREKI)

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細履歴マスタ
    /// </summary>
    /// <create>AutoCodeGenerator 2024/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public class Def_M_TEHAI_MEISAI_RIREKI
    {
        #region フィールド名

        /// --------------------------------------------------
        /// <summary>
        /// ECS 期
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ECS_QUOTA = "ECS_QUOTA";
        /// --------------------------------------------------
        /// <summary>
        /// ECSNo
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ECS_NO = "ECS_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 連携No
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TEHAI_RENKEI_NO = "TEHAI_RENKEI_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 更新フラグ
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string STATUS_FLAG = "STATUS_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 更新日時
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_DATE = "UPDATE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        /// --------------------------------------------------
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UPDATE_USER_NAME = "UPDATE_USER_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string VERSION = "VERSION";

        #endregion

        #region テーブル名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>AutoCodeGenerator 2024/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string Name
        {
            get { return "M_TEHAI_MEISAI_RIREKI"; }
        }

        #endregion
    }

    #endregion

}