using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;
using WsConnection;
using System.Data;
using WsConnection.WebRefCommon;
using SystemBase.Util;
using WsConnection.WebRefM01;

namespace SystemBase
{
    /// --------------------------------------------------
    /// <summary>
    /// メニューの権限チェッククラス
    /// </summary>
    /// <create>TW-Tsuji 2012/10/12</create>
    /// <update></update>
    /// <notes>
    /// プログラム内で各機能の利用権限有無をチェックする
    /// ためのクラスを作成.
    /// メインメニュー表示時にBaseSDIMenuクラス内で権限
    /// 情報を抽出しているが、データ流用が難しく新規作成.
    /// </notes>>
    /// --------------------------------------------------
    public class MenuAuthorityCheck
    {
        protected UserInfo _userInfo;
        protected bool _getMenuAuthFlag;
        protected DataSet _menuDataSet;
        //protected DSWControl.DSWMenu _mnuMenu;    //メニューコントロールは未使用

        #region コンストラクタ
        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>TW-Tsuji 2012/10/12</create>
        /// <update></update>
        /// <notes>
        /// クラスがインスタンス化された時の初期化.
        /// </notes>
        /// --------------------------------------------------
        public MenuAuthorityCheck(UserInfo userInfo)
        {
            //ユーザ情報をクラスメンバに保持
            this._userInfo = userInfo;

            //データ取得フラグを初期化
            this._getMenuAuthFlag = false;

            //メニュー用のデータセットをインスタンス化
            this._menuDataSet = new DataSet();

            //メニューコントロールは未使用
            //_mnuMenu = new DSWControl.DSWMenu();

            //考慮の必要はあるが、とりあえずコンストラクタから初期化（GetMenu）を呼んでおく
            bool rtn = this.GetMenu();
            if (rtn == false)
            {
                //権限が取れなかった場合も、そのままスルー
            }
        }
        #endregion


        #region 指定された権限が付与されているかを返す関数(static)
        /// --------------------------------------------------
        /// <summary>
        /// 権限が利用可能かを返す（引数：カテゴリ、メニューアイテム）
        /// </summary>
        /// <returns></returns>
        /// <create>TW-Tsuji 2022/10/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool ExistsRoleAndRolemap(UserInfo userInfo, string menuCategoryId, string menuItemId)
        {
            var conn = new ConnM01();
            var cond = new CondM01(userInfo);

            cond.UserID = userInfo.UserID;          //ユーザID（このユーザ）
            cond.JyotaiFlag = menuCategoryId;       //状態フラグを流用して、カテゴリIDを渡す
            cond.KanriFlag = menuItemId;            //管理フラグを流用して、アイテムIDを渡す

            if (conn.ExistsRoleAndRolemap(cond))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region メニューデータ取得・初期化
        /// --------------------------------------------------
        /// <summary>
        /// メニューデータ取得
        /// </summary>
        /// <returns></returns>
        /// <create>TW-Tsuji 2022/10/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool GetMenu()
        {
            try
            {
                //メニュー表示用で使用しているコントロールをそのまま呼び出す
                ConnCommon conn = new ConnCommon();
                CondCommon cond = new WsConnection.WebRefCommon.CondCommon(this._userInfo);

                // 検索条件設定
                cond.UserID = this._userInfo.UserID;
                cond.PcName = this._userInfo.PcName;
                cond.MenuItemFlag = MENU_ITEM_FLAG.PC_VALUE1;
                cond.TerminalRole = this._userInfo.SysInfo.TerminalRole;
                cond.TerminalGuest = this._userInfo.SysInfo.TerminalGuest;

                // メニュー・権限情報の取得
                DataSet ds = conn.GetMenu(cond);

                //エラーチェック
                if (ds.Tables[ComDefine.DTTBL_MENU].Rows.Count < 1)
                {
                    //メニューアイテムが無い（データ異常でもフラグだけ返す）
                    return false;
                }
                if (!ds.Tables.Contains(ComDefine.DTTBL_MENU))
                {
                    //メニューが違う？（データ異常でもフラグだけ返す）
                    return false;
                }

                //クラスメンバのデータセットに保存
                this._menuDataSet = ds;

                //権限データ取得フラグをつけておく
                this._getMenuAuthFlag = true;

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region 指定された権限が付与されているかを返す関数
        /// --------------------------------------------------
        /// <summary>
        /// 権限が利用可能かを返す（引数：カテゴリ、メニューアイテム）
        /// </summary>
        /// <returns></returns>
        /// <create>TW-Tsuji 2022/10/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool ExistsRoleAndRolemap(string menuCategoryId, string menuItemId)
        {
            var conn = new ConnM01();
            var cond = new CondM01(this._userInfo);

            cond.UserID = this._userInfo.UserID;    //ユーザID（このユーザ）
            cond.JyotaiFlag = menuCategoryId;       //状態フラグを流用して、カテゴリIDを渡す
            cond.KanriFlag = menuItemId;            //管理フラグを流用して、アイテムIDを渡す

            if (conn.ExistsRoleAndRolemap(cond))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion


        #region メニューに権限が付与されているかを返す関数
        /// --------------------------------------------------
        /// <summary>
        /// 権限が利用可能かを返す（引数：カテゴリ、メニューアイテム）
        /// </summary>
        /// <returns></returns>
        /// <create>TW-Tsuji 2022/10/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool IsMenuEnabled(string menuCategoryId, string menuItemId)
        {
            if (this._getMenuAuthFlag == true)
            {
                //メニューアイテムID（MENU_ITEM_ID）を検索する
                return _searchMenuItemID(menuCategoryId, menuItemId);
            }
            else
            {
                //メニューが初期化されていない場合（とりあえず false を返しておく）
                return false;
            }
        }
        #endregion

        #region 【Private】メニューアイテムからメニューを検索する
        /// --------------------------------------------------
        /// <summary>
        /// 権限が利用可能かを返す（引数：カテゴリ、メニューアイテム）
        /// </summary>
        /// <returns></returns>
        /// <create>TW-Tsuji 2022/10/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _searchMenuItemID(string menuCategoryId, string menuItemId)
        {
            foreach (DataRow dr in _menuDataSet.Tables[ComDefine.DTTBL_MENU].Rows)
            {
                if (menuCategoryId.Equals(dr[0].ToString()) && menuItemId.Equals( dr[3].ToString()))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
