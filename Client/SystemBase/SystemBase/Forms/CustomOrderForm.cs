using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Media;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using DSWUtil;
using DSWControl.DSWComboBox;
using WsConnection;
using WsConnection.WebRefCommon;
using Commons;
using SystemBase.Controls;
using SystemBase.Util;
using GrapeCity.Win.ElTabelle;
using GrapeCity.Win.ElTabelle.Editors;
using ElTabelleHelper;
using SystemBase.Properties;

namespace SystemBase.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 製番カスタマイズ用ベースフォーム
    /// </summary>
    /// <create>Y.Higuchi 2010/04/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class CustomOrderForm : BaseForm
    {

        #region Delegate

        /// --------------------------------------------------
        /// <summary>
        /// 登録/修正/削除処理用デリゲート
        /// </summary>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private delegate bool RunEditExcecDelegate();

        #endregion

        #region フィールド

        /// --------------------------------------------------
        /// <summary>
        /// 登録処理確認メッセージID
        /// </summary>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _msgInsertConfirm = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 登録処理完了メッセージID
        /// </summary>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _msgInsertEnd = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 修正処理確認メッセージID
        /// </summary>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _msgUpdateConfirm = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 修正処理完了メッセージID
        /// </summary>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _msgUpdateEnd = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 削除処理確認メッセージID
        /// </summary>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _msgDeleteConfirm = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 削除処理完了メッセージID
        /// </summary>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _msgDeleteEnd = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// RunEdit終了後画面をクリアするかどうか
        /// </summary>
        /// <create>Y.Higuchi 2010/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isRunEditAfterClear = true;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ（デザイナ用）
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private CustomOrderForm()
            : this(new UserInfo(), "", "", "")
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <create>R.Katsuo 2010/01/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public CustomOrderForm(UserInfo userInfo)
            : this(userInfo, "", "", "")
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="title">画面タイトル</param>
        /// <create>R.Katsuo 2010/01/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public CustomOrderForm(UserInfo userInfo, string title)
            : this(userInfo, "", "", title)
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="menuCategoryID">メニュー種別ID</param>
        /// <param name="menuItemID">メニュー項目ID</param>
        /// <param name="title">画面タイトル</param>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public CustomOrderForm(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
            : base(userInfo, menuCategoryID, menuItemID, title)
        {
            InitializeComponent();
        }

        #endregion

        #region プロパティ

        /// --------------------------------------------------
        /// <summary>
        /// 登録処理確認メッセージID
        /// </summary>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        [Browsable(false)]
        protected string MsgInsertConfirm
        {
            get { return this._msgInsertConfirm; }
            set { this._msgInsertConfirm = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 登録完了メッセージID
        /// </summary>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        [Browsable(false)]
        protected string MsgInsertEnd
        {
            get { return this._msgInsertEnd; }
            set { this._msgInsertEnd = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 修正確認メッセージID
        /// </summary>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        [Browsable(false)]
        protected string MsgUpdateConfirm
        {
            get { return this._msgUpdateConfirm; }
            set { this._msgUpdateConfirm = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 修正完了メッセージID
        /// </summary>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        [Browsable(false)]
        protected string MsgUpdateEnd
        {
            get { return this._msgUpdateEnd; }
            set { this._msgUpdateEnd = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 削除確認メッセージID
        /// </summary>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        [Browsable(false)]
        protected string MsgDeleteConfirm
        {
            get { return this._msgDeleteConfirm; }
            set { this._msgDeleteConfirm = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 削除完了メッセージID
        /// </summary>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        [Browsable(false)]
        protected string MsgDeleteEnd
        {
            get { return this._msgDeleteEnd; }
            set { this._msgDeleteEnd = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// RunEdit終了後画面をクリアするかどうか
        /// </summary>
        /// <create>Y.Higuchi 2010/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        [Category("カスタム動作")]
        [Description("RunEdit終了後画面をクリアするかどうか")]
        [DefaultValue(true), SettingsBindable(true)]
        public virtual bool IsRunEditAfterClear
        {
            get { return this._isRunEditAfterClear; }
            set { this._isRunEditAfterClear = value; }
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // ベースでDisplayClearの呼出しは行われています。

                // 登録確認メッセージ
                this.MsgInsertConfirm = "FW010040001";
                // 登録完了メッセージ
                this.MsgInsertEnd = "FW010040002";

                // 修正確認メッセージ
                this.MsgUpdateConfirm = "FW010040003";
                // 修正完了メッセージ
                this.MsgUpdateEnd = "FW010040004";

                // 削除確認メッセージ
                this.MsgDeleteConfirm = "FW010040005";
                // 削除完了メッセージ
                this.MsgDeleteEnd = "FW010040006";
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

        }

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示されるたびにコントロールの初期化するメソッド
        /// </summary>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();
            try
            {
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// シートの初期化を行うメソッド
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeSheet(Sheet sheet)
        {
            base.InitializeSheet(sheet);
            try
            {
                // グリッド線
                sheet.GridLine = new GridLine(GridLineStyle.Thin, Color.DarkGray);
                // Disable時の設定
                for (int i = 0; i < sheet.Columns.Count; i++)
                {
                    sheet.Columns[i].DisabledBackColor = Color.FromArgb(223, 223, 223);
                    sheet.Columns[i].DisabledForeColor = Color.Black;
                }
                sheet.KeyDown += new KeyEventHandler(sheet_KeyDown);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual bool CheckInput()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual bool CheckInputSearch()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region 検索処理

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理制御部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual bool RunSearch()
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                // 検索処理用入力チェック
                if (!this.CheckInputSearch())
                {
                    // 入力チェックNG
                    return false;
                }
                Cursor.Current = Cursors.WaitCursor;
                return this.RunSearchExec();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
            finally
            {
                Cursor.Current = preCursor;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理実行部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual bool RunSearchExec()
        {
            return true;
        }


        #endregion

        #region 編集内容実行

        #region 制御メソッド

        /// --------------------------------------------------
        /// <summary>
        /// 編集内容実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual bool RunEdit()
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                string confirmMsg = string.Empty;
                string endEditMsg = string.Empty;
                RunEditExcecDelegate delegateFunc = null;

                // 入力チェック
                if (!this.CheckInput())
                {
                    // 入力チェックNG
                    return false;
                }

                switch (this.EditMode)
                {
                    case EditMode.Insert:
                        // 登録処理
                        confirmMsg = this.MsgInsertConfirm;
                        delegateFunc = new RunEditExcecDelegate(this.RunEditInsert);
                        break;
                    case EditMode.Update:
                        // 修正処理
                        confirmMsg = this.MsgUpdateConfirm;
                        delegateFunc = new RunEditExcecDelegate(this.RunEditUpdate);
                        break;
                    case EditMode.Delete:
                        // 削除処理
                        confirmMsg = this.MsgDeleteConfirm;
                        delegateFunc = new RunEditExcecDelegate(this.RunEditDelete);
                        break;
                    default:
                        return false;
                }
                // 確認メッセージ
                if (!string.IsNullOrEmpty(confirmMsg))
                {
                    if (!this.ShowMessage(confirmMsg).Equals(DialogResult.OK))
                    {
                        return false;
                    }
                }
                Cursor.Current = Cursors.WaitCursor;
                // 処理実行
                bool ret = delegateFunc();
                if (ret)
                {

                    switch (this.EditMode)
                    {
                        case EditMode.Insert:
                            // 登録処理
                            endEditMsg = this.MsgInsertEnd;
                            break;
                        case EditMode.Update:
                            // 修正処理
                            endEditMsg = this.MsgUpdateEnd;
                            break;
                        case EditMode.Delete:
                            // 削除処理
                            endEditMsg = this.MsgDeleteEnd;
                            break;
                        default:
                            return false;
                    }
                    // 正常処理時
                    if (this.IsRunEditAfterClear)
                    {
                        this.DisplayClear();
                    }
                    if (!string.IsNullOrEmpty(endEditMsg))
                    {
                        this.ShowMessage(endEditMsg);
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
            finally
            {
                Cursor.Current = preCursor;
            }
        }

        #endregion

        #region 登録処理

        /// --------------------------------------------------
        /// <summary>
        /// 登録処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual bool RunEditInsert()
        {
            return true;
        }

        #endregion

        #region 修正処理

        /// --------------------------------------------------
        /// <summary>
        /// 修正処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual bool RunEditUpdate()
        {
            return true;
        }

        #endregion

        #region 削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual bool RunEditDelete()
        {
            return true;
        }

        #endregion

        #endregion

        #region イベント

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F1ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F01Button_Click(sender, e);
            try
            {
                switch (this.EditMode)
                {
                    case EditMode.None:
                        break;
                    case EditMode.Insert:
                    case EditMode.Update:
                    case EditMode.Delete:
                        this.RunEdit();
                        break;
                    case EditMode.View:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F2ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F02Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F02Button_Click(sender, e);
            try
            {
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F3ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F03Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F03Button_Click(sender, e);
            try
            {
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F4ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F04Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F04Button_Click(sender, e);
            try
            {
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F5ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F05Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F05Button_Click(sender, e);
            try
            {
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F6ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F7ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F8ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F08Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F08Button_Click(sender, e);
            try
            {
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F9ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F09Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F09Button_Click(sender, e);
            try
            {
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F10ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F10Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F10Button_Click(sender, e);
            try
            {
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F11ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F11Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F11Button_Click(sender, e);
            try
            {
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F12ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F12Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F12Button_Click(sender, e);
            try
            {
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region El Tabelle Sheet

        #region KeyDown

        /// --------------------------------------------------
        /// <summary>
        /// コントロールにフォーカスがあるときにキーが押されると発生します。 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private void sheet_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                Sheet sheet = sender as Sheet;
                if (sheet == null) return;
                if (sheet.ActiveCell == null) return;
                ButtonEditor btnEditor = sheet.ActiveCell.Editor as ButtonEditor;
                SuperiorComboEditor scbEditor = sheet.ActiveCell.Editor as SuperiorComboEditor;
                if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    if (btnEditor != null)
                    {
                        // ボタンであればDeleteキーを無効にする。
                        // 無効にしないとキャプションが消えるらしい
                        e.Handled = true;
                    }
                    else if (scbEditor != null && !scbEditor.Editable)
                    {
                        // 拡張コンボボックスでドロップダウンであればDeleteキーを無効にする。
                        // 無効にしないとSelectedIndexが-1になるらしい
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #endregion

        #endregion

        #region 製番追加分

        /// --------------------------------------------------
        /// <summary>
        /// ログイン情報をユーザー情報からコピーする。
        /// </summary>
        /// <param name="target">コンディションクラスかLoginInfoプロパティ</param>
        /// <create>Y.Higuchi 2010/06/29</create>
        /// <update></update>
        /// --------------------------------------------------
        [ObsoleteAttribute("このメソッドは削除されます。Conditionクラスのコンストラクタにて同様の処理を行います。", false)]
        protected virtual void SetCondBase_LoginInfo(object target)
        {
            try
            {
                if (target == null) return;
                System.Reflection.PropertyInfo loginInfoPropertyInfo = null;
                object loginInfo = null;
                Type loginInfoType = null;
                if (target.GetType().Name.StartsWith(ComDefine.CONDITION_CLASSNAME_STARTWITH))
                {
                    // コンディションクラスの場合の処理
                    // ログイン情報プロパティのチェック
                    loginInfoPropertyInfo = target.GetType().GetProperty(ComDefine.LOGININFO_PROPERTY_NAME);
                    if (loginInfoPropertyInfo != null)
                    {
                        loginInfo = loginInfoPropertyInfo.GetValue(target, null);
                        loginInfoType = loginInfoPropertyInfo.PropertyType;
                    }
                }
                else if (target.GetType().Name.Equals(ComDefine.LOGININFO_TYPENAME))
                {
                    // ログイン情報の場合の処理
                    loginInfo = target;
                    loginInfoType = target.GetType();
                }

                if (loginInfoType == null) return;

                if (loginInfo == null)
                {
                    // ログイン情報がNULLの場合、インスタンス化する。
                    loginInfo = loginInfoType.InvokeMember(null,
                                                   System.Reflection.BindingFlags.CreateInstance,
                                                   null,
                                                   null,
                                                   null);
                }

                Type userInfoType = this.UserInfo.GetType();
                // ログイン情報のプロパティで走査
                foreach (System.Reflection.PropertyInfo item in loginInfo.GetType().GetProperties())
                {
                    // ユーザー情報にログイン情報と同じプロパティがあるか判定
                    System.Reflection.PropertyInfo pi = userInfoType.GetProperty(item.Name);
                    if (item != null && pi.PropertyType == item.PropertyType)
                    {
                        try
                        {
                            // 同名、同型のプロパティがある。
                            item.SetValue(loginInfo, pi.GetValue(this.UserInfo, null), null);
                        }
                        catch (Exception) { }
                    }
                }
                if (loginInfoPropertyInfo != null)
                {
                    // コンディションクラスだった場合、プロパティ値を書き戻す。
                    loginInfoPropertyInfo.SetValue(target, loginInfo, null);
                }
            }
            catch (Exception) { }
        }


        #region SheetのInvalidateエラー判定
        /// --------------------------------------------------
        /// <summary>
        /// SheetのBindingErrorにおいてInvalidateエラーが発生したかどうかを判定
        /// </summary>
        /// <param name="e">BindingErrorイベントで引き渡される情報</param>
        /// <returns>値が不正であるかどうかの判定</returns>
        /// <remarks>
        /// El Tabelle SheetでコンボボックスなどのValue値が不正の際に
        /// 発生するエラーをハンドリングするための判定。
        /// 本来はエラーコード等で判定すべきだが、BindingErrorEventArgsに
        /// エラーメッセージしか含まれていないため、この手法で判断する。
        /// </remarks>
        /// <create>D.Okumura 2019/01/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected bool IsSheetBindingInvalidate(BindingErrorEventArgs e)
        {
            const string ErrorMsg_ValueIsInvalidate = "Value is Invalidate!";
            return e.ErrorMessage.Equals(ErrorMsg_ValueIsInvalidate);
        }
        #endregion

        #region 必要に応じて改行コード(CR+LF,LF)を削除した文字列を返却する

        /// --------------------------------------------------
        /// <summary>
        /// 必要に応じて改行コード(CR+LF,LF)を削除した文字列を返却する
        /// </summary>
        /// <param name="sheet">Sheet</param>
        /// <param name="str">文字列</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/01/25</create>
        /// <update></update>
        /// <remarks>K.Tsutsumi 2018/01/26 現段階では使用しないこととする</remarks>
        /// --------------------------------------------------
        protected virtual string GetTextRemoveNewLineIfNeed(Sheet sheet, string str)
        {
            str = str.Replace("\r", "\r\n").Replace("\n", "\r\n").Replace("\r\r\n\r\n", "\r\n");
            // 複数行不可の場合は、改行コードを削除する
            if (sheet.ActiveCell.Editor is GrapeCity.Win.ElTabelle.Editors.TextEditor
                && !(sheet.ActiveCell.Editor as GrapeCity.Win.ElTabelle.Editors.TextEditor).MultiLine)
            {
                str = str.Replace("\r\n", string.Empty);
            }
            return str;
        }

        #endregion

        #region グリッド幅のユーザー設定関連

        #region セクション名をセットする

        /// --------------------------------------------------
        /// <summary>
        /// セクション名をセットする
        /// </summary>
        /// <param name="sht">シート</param>
        /// <param name="sectionPrefix">セクション名のプレフィックス</param>
        /// <returns>セクション名</returns>
        /// <create>H.Tsuji 2019/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected string SetSectionName(Sheet sht, string sectionPrefix)
        {
            return sectionPrefix + "_" + sht.Name;
        }

        #endregion

        #region 設定ファイルからグリッド幅を読み込む

        /// --------------------------------------------------
        /// <summary>
        /// 設定ファイルからグリッド幅を読み込む
        /// </summary>
        /// <param name="sht">シート</param>
        /// <param name="sectionPrefix">セクション名のプレフィックス</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tsuji 2019/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected bool LoadGridSetting(Sheet sht, string sectionPrefix)
        {
            // セクション名をセットする
            string section = this.SetSectionName(sht, sectionPrefix);

            // 設定ファイルから指定したセクションの内容を読み込む
            DataTable dt = GridSetting.LoadSection(section);
            if (dt.TableName != section || dt.Rows.Count == 0)
            {
                return false;
            }

            DataRow dr = dt.Rows[0];
            
            // DataFieldとセクションキーが一致するグリッド幅を更新する
            for (int colIndex = 0; colIndex < sht.Columns.Count; colIndex++)
            {
                Column col = sht.Columns[colIndex];
                if (dt.Columns.Contains(col.DataField))
                {
                    int width;
                    if (int.TryParse(dr[col.DataField].ToString(), out width))
                    {
                        col.Width = width;
                    }
                }
            }

            return true;
        }

        #endregion

        #region グリッド幅を設定ファイルに書き込む

        /// --------------------------------------------------
        /// <summary>
        /// グリッド幅を設定ファイルに書き込む
        /// </summary>
        /// <param name="sht">シート</param>
        /// <param name="sectionPrefix">セクション名のプレフィックス</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tsuji 2019/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected bool SaveGridSetting(Sheet sht, string sectionPrefix)
        {
            // セクション名をセットする
            string section = this.SetSectionName(sht, sectionPrefix);

            DataTable dt = new DataTable(section);

            // セクションデータのカラム定義
            for (int colIndex = 0; colIndex < sht.Columns.Count; colIndex++)
            {
                Column col = sht.Columns[colIndex];
                dt.Columns.Add(col.DataField, typeof(int));
            }

            // セクションデータをセット
            DataRow dr = dt.NewRow();
            for (int colIndex = 0; colIndex < sht.Columns.Count; colIndex++)
            {
                Column col = sht.Columns[colIndex];
                dr[colIndex] = col.Width;
            }
            dt.Rows.Add(dr);

            return GridSetting.SaveSection(section, dt);
        }

        #endregion

        #endregion

        #region ツールチップの設定
        /// --------------------------------------------------
        /// <summary>
        /// ツールチップの設定
        /// </summary>
        /// <param name="control">設定対象のコントロール</param>
        /// <param name="text">表示内容</param>
        /// <create>D.Okumura 2019/08/06</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void SetToolTip(Control control, string text)
        {
            this.toolTip.SetToolTip(control, text);
        }
        #endregion

        #region メッセージ表示処理

        /// --------------------------------------------------
        /// <summary>
        /// メッセージ表示処理
        /// </summary>
        /// <param name="msgType">メッセージアイコンタイプ</param>
        /// <param name="messageID">メッセージID</param>
        /// <param name="args">可変メッセージ</param>
        /// <returns>DialogResult</returns>
        /// <create>H.Tajimi 2019/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual DialogResult ShowMessage(MessageImageType msgType, string messageID, params string[] args)
        {
            try
            {
                DataSet ds = this.GetMessage(messageID);
                // 取得失敗
                if (ds.Tables[Def_M_MESSAGE.Name] == null)
                {
                    this.ClearMessage();
                    return DialogResult.Cancel;
                }
                // メッセージとアイコンを設定
                DataTable dt = ds.Tables[Def_M_MESSAGE.Name];
                string notFoundMsg = ComDefine.MSG_NOTFOUND_TEXT + "【" + messageID + "】";
                string msg = ComFunc.GetFld(dt, 0, Def_M_MESSAGE.MESSAGE, notFoundMsg);
                string msgFlag = ComFunc.GetFld(dt, 0, Def_M_MESSAGE.MESSAGE_FLAG, MESSAGE_FLAG.LABEL_VALUE1);

                if (msgFlag.Equals(MESSAGE_FLAG.LABEL_VALUE1))
                {
                    this.MsgLabel(msgType, msg, args);
                }
                else if (msgFlag.Equals(MESSAGE_FLAG.MESSAGEBOX_VALUE1))
                {
                    MessageBoxButtons msgButtonFlag = this.GetMessageBoxButtons(ComFunc.GetFld(dt, 0, Def_M_MESSAGE.BUTTON_FLAG));
                    MessageBoxDefaultButton msgDefaultButton = this.GetMessageBoxDefaultButton(ComFunc.GetFld(dt, 0, Def_M_MESSAGE.DEFAULT_BUTTON));
                    return this.MsgBox(msgType, msgButtonFlag, msgDefaultButton, msg, args);
                }
                return DialogResult.OK;
            }
            catch (Exception)
            {
                return DialogResult.Cancel;
            }
        }

        #endregion

        #region 汎用マスタ取得処理
        /// --------------------------------------------------
        /// <summary>
        /// 汎用マスタのデータを取得する。
        /// </summary>
        /// <typeparam name="T">値の型</typeparam>
        /// <param name="groupCD">グループコード</param>
        /// <param name="keyField">キーにするフィールド</param>
        /// <param name="valueDelegate">DataRowから値を取得する</param>
        /// <returns>Dictionary(keyField, T)</returns>
        /// <create>D.Okumura 2019/08/26</create>
        /// <update></update>
        /// <exception cref="ArgumentException">keyFieldに重複するレコードが含まれる</exception>
        /// --------------------------------------------------
        protected virtual Dictionary<string, T> GetCommon<T>(string groupCD, string keyField, Func<DataRow, T> valueDelegate)
        {
            try
            {
                return this.GetCommon(groupCD)
                    .Tables[Def_M_COMMON.Name].AsEnumerable()
                    .ToDictionary(k => ComFunc.GetFld(k, keyField), valueDelegate)
                    ;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 汎用マスタのデータを取得する。
        /// </summary>
        /// <param name="groupCD">グループコード</param>
        /// <returns>Dictionary(Value1, ItemName)</returns>
        /// <create>D.Okumura 2019/08/26</create>
        /// <update></update>
        /// <remarks>Value1に重複がない場合のみ使用可能</remarks>
        /// <exception cref="ArgumentException">Value1に重複するレコードが含まれる</exception>
        /// --------------------------------------------------
        protected virtual Dictionary<string, string> GetCommonItemNameByValue1(string groupCD)
        {
            try
            {
                return this.GetCommon(groupCD, Def_M_COMMON.VALUE1, (v) => ComFunc.GetFld(v, Def_M_COMMON.ITEM_NAME));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region TextBoxの内容にプレフィクスを付加して取得

        /// --------------------------------------------------
        /// <summary>
        /// TextBoxの内容にプレフィクスを付加して取得
        /// </summary>
        /// <remarks>
        /// ARNo、BoxNo、PalletNoなどプレフィクスを付加する
        /// 必要がある内容の場合に使用
        /// </remarks>
        /// <param name="target">TextBox</param>
        /// <param name="prefix">プレフィクス</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual string GetTextWithPrefix(DSWControl.DSWTextBox.DSWTextBox target, string prefix)
        {
            return this.GetTextWithPrefixAndPadLeft(target, prefix, 0);
        }

        /// --------------------------------------------------
        /// <summary>
        /// TextBoxの内容にプレフィクスを付加して取得
        /// </summary>
        /// <remarks>
        /// ARNo、BoxNo、PalletNoなどプレフィクスを付加
        /// 且つ、前ゼロ埋めも必要な場合に使用
        /// </remarks>
        /// <param name="target">TextBox</param>
        /// <param name="prefix">プレフィクス</param>
        /// <param name="totalWidth">整形後長</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual string GetTextWithPrefixAndPadLeft(DSWControl.DSWTextBox.DSWTextBox target, string prefix, int totalWidth)
        {
            return this.GetTextWithPrefixAndPadLeft(target, prefix, totalWidth, '0');
        }

        /// --------------------------------------------------
        /// <summary>
        /// TextBoxの内容にプレフィクスを付加して取得
        /// </summary>
        /// <remarks>
        /// ARNo、BoxNo、PalletNoなどプレフィクスを付加
        /// 且つ、パディングも必要な場合に使用
        /// </remarks>
        /// <param name="target">TextBox</param>
        /// <param name="prefix">プレフィクス</param>
        /// <param name="totalWidth">整形後長</param>
        /// <param name="paddingChar">埋め込み文字</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual string GetTextWithPrefixAndPadLeft(DSWControl.DSWTextBox.DSWTextBox target, string prefix, int totalWidth, char paddingChar)
        {
            if (!target.Enabled)
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(target.Text))
            {
                return string.Empty;
            }

            if (totalWidth < 1)
            {
                return prefix + target.Text;
            }
            else
            {
                return prefix + target.Text.PadLeft(totalWidth, paddingChar);
            }
        }

        #endregion

        #region コンボボックスにデータソースを設定する

        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスにデータソースを設定する(「全て」付き)
        /// </summary>
        /// <param name="ctrl">コンボボックス</param>
        /// <param name="dt">データソース</param>
        /// <param name="field">抽出するフィールド</param>
        /// <create>T.Sakiori 2012/09/21</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void SetComboDataSourceEx(ComboBox ctrl, DataTable dt, string field)
        {
            this.SetComboDataSourceEx(ctrl, dt, field, true);
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスにデータソースを設定する
        /// </summary>
        /// <param name="ctrl">コンボボックス</param>
        /// <param name="dt">データソース</param>
        /// <param name="field">抽出するフィールド</param>
        /// <param name="addAll">「全て」を追加するかどうか</param>
        /// <create>T.Sakiori 2012/09/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void SetComboDataSourceEx(ComboBox ctrl, DataTable dt, string field, bool addAll)
        {
            if (dt == null || !UtilData.ExistsData(dt))
            {
                if (addAll)
                {
                    List<string> tmp = new List<string>();
                    tmp.Add(ComDefine.COMBO_ALL_DISP);
                    ctrl.DataSource = tmp;
                }
                return;
            }
            var list = dt.AsEnumerable().Select(p => ComFunc.GetFld(p, field).Trim()).Distinct().OrderBy(p => p).ToList();
            if (addAll)
            {
                list.Insert(0, ComDefine.COMBO_ALL_DISP);
            }
            ctrl.DataSource = list;
        }
        
        #endregion

        #region フォルダ選択ダイアログ
        /// --------------------------------------------------
        /// <summary>
        /// フォルダ選択ダイアログ
        /// </summary>
        /// <param name="form">親フォーム</param>
        /// <param name="title">ファイルダイアログタイトル(null許容)</param>
        /// <param name="initialDirectory">前回選択フォルダ(null許容)</param>
        /// <returns>選択されたフォルダパス(nullの場合、キャンセルボタン押下)</returns>
        /// <create>D.Okumura 2020/09/03</create>
        /// <update></update>
        /// --------------------------------------------------
        protected string ShowSaveFolderDialog(IWin32Window form, string title, string initialDirectory)
        {
            using (var ofd = new SaveFileDialog() {
                FileName = Resources.CustomOrderForm_ShowSaveFolderDialogFileName,
                Filter = Resources.CustomOrderForm_ShowSaveFolderDialogFilter,
                CheckFileExists = false,
                AddExtension = false,
            })
            {
                if (!string.IsNullOrEmpty(title))
                    ofd.Title = title;
                if (!string.IsNullOrEmpty(initialDirectory))
                    ofd.InitialDirectory = initialDirectory;
                // ダイアログを表示
                var result = (form == null) ? ofd.ShowDialog() : ofd.ShowDialog(form);
                if (result == DialogResult.OK)
                {
                    var path = System.IO.Path.GetDirectoryName(ofd.FileName);
                    return path;
                }
                else
                {
                    return null;
                }
                
            }
            
        }
        #endregion
        #endregion

    }
}
