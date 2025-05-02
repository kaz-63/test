using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DSWUtil;
using DSWControl.DSWComboBox;
using WsConnection;
using WsConnection.WebRefCommon;
using Commons;
using SystemBase.Controls;
using GrapeCity.Win.ElTabelle;
using ElTabelleHelper;

namespace SMS
{
    /// --------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <create>$Creator$</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class CustomSearchDialog : SystemBase.Forms.CustomOrderSearchDialog
    {
        // TODO : メッセージ表示時はthis.ShowMessage()を使用してください。
        // TODO : 不要なコンストラクタは削除してください。
        // TODO : $Creator$を「作成者 作成日」で置換してください。

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="menuCategoryID">メニュー種別ID</param>
        /// <param name="menuItemID">メニュー項目ID</param>
        /// <param name="title">画面タイトル</param>
        /// <create>$Creator$</create>
        /// <update></update>
        /// --------------------------------------------------
        public CustomSearchDialog()
            : base()
        {
            InitializeComponent();
            // 画面タイトル設定
            // TODO : ここで画面タイトルを設定します。
            this.Title = "";
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="menuCategoryID">メニュー種別ID</param>
        /// <param name="menuItemID">メニュー項目ID</param>
        /// <param name="title">画面タイトル</param>
        /// <create>$Creator$</create>
        /// <update></update>
        /// --------------------------------------------------
        public CustomSearchDialog(UserInfo userInfo)
            : base(userInfo)
        {
            InitializeComponent();
            // 画面タイトル設定
            // TODO : ここで画面タイトルを設定します。
            this.Title = "";
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="menuCategoryID">メニュー種別ID</param>
        /// <param name="menuItemID">メニュー項目ID</param>
        /// <param name="title">画面タイトル</param>
        /// <create>$Creator$</create>
        /// <update></update>
        /// --------------------------------------------------
        public CustomSearchDialog(UserInfo userInfo, string title)
            : base(userInfo, title)
        {
            InitializeComponent();
            // 画面タイトル設定
            // TODO : ここで画面タイトルを設定します。
            this.Title = "";
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>$Creator$</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // TODO : ここにコントロールの初期化を記述する。
                // ベースでDisplayClear()の呼出しは行われています。

                // TODO : ここにシートの列設定を記述する。
                // ベースでInitializeSheet()の呼び出しは行われています。

            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

        }

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示されるたびにコントロールの初期化するメソッド
        /// </summary>
        /// <create>$Creator$</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // TODO : ここに初期フォーカスの設定を記述する。
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>$Creator$</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // TODO : ベースでClearMessageの呼出しは行われています。
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>$Creator$</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                // TODO : 検索用入力チェック
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                ret = false;
            }
            return ret;
        }

        #endregion

        #region 検索処理

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理制御部
        /// </summary>
        /// <create>$Creator$</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void RunSearch()
        {
            base.RunSearch();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理実行部
        /// </summary>
        /// <create>$Creator$</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void RunSearchExec()
        {
            // TODO : ここに検索処理を記述する。
            if (this.IsSearchAll)
            {
                // 全件検索の場合
            }
            else
            {
                // 検索の場合
            }
        }

        #endregion

        #region 選択行取得

        /// --------------------------------------------------
        /// <summary>
        /// 選択行取得
        /// </summary>
        /// <param name="rowIndex">選択行インデックス</param>
        /// <returns>選択行のDataRow</returns>
        /// <create>$Creator$</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override DataRow GetSelectedRowData(int rowIndex)
        {
            DataRow dr = base.GetSelectedRowData(rowIndex);
            try
            {
                // TODO : ここで選択行のデータを設定を変更する。
                // shtResultのDataSourceにDataSetかDataTableを設定している場合は変更不要です。
                // rowIndexには未選択時には-1が設定されています。
                return dr;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return null;
            }
        }

        #endregion
    }
}
