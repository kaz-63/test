using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Media;
using System.Text;
using System.Windows.Forms;

using DSWUtil;
using DSWControl.DSWComboBox;
using DSWControl.DSWLabel;
using DSWControl.DSWTextBox;
using WsConnection;
using WsConnection.WebRefCommon;
using Commons;
using SystemBase.Controls;
using GrapeCity.Win.ElTabelle;
using ElTabelleHelper;
using SystemBase.Util;
using SystemBase.Properties;

namespace SystemBase.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// トラックバース管理システムベースフォーム
    /// </summary>
    /// <create>R.Katsuo 2010/01/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class BaseForm : Form
    {

        #region フィールド

        /// --------------------------------------------------
        /// <summary>
        /// CloseBoxEnabledフィールド
        /// </summary>
        /// <create>Y.Higuchi 2009/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _closeBoxEnabled = true;
        /// --------------------------------------------------
        /// <summary>
        /// IsDirtyフィールド
        /// </summary>
        /// <create>Y.Higuchi 2009/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isDirty = false;
        /// --------------------------------------------------
        /// <summary>
        /// IsChangedCloseQuestionフィールド
        /// </summary>
        /// <create>Y.Higuchi 2009/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isChangedCloseQuestion = false;
        /// --------------------------------------------------
        /// <summary>
        /// IsCloseQuestionフィールド
        /// </summary>
        /// <create>Y.Higuchi 2009/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isCloseQuestion = false;
        /// --------------------------------------------------
        /// <summary>
        /// IsEnterNextControlフィールド
        /// </summary>
        /// <create>Y.Higuchi 2009/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isEnterNextControl = true;
        /// --------------------------------------------------
        /// <summary>
        /// IsEnterNextControlForSheetフィールド
        /// </summary>
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isEnterNextControlForSheet = false;
        /// --------------------------------------------------
        /// <summary>
        /// ログインユーザー情報管理クラス
        /// </summary>
        /// <create>R.Katsuo 2010/01/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private UserInfo _userInfo = null;

        /// --------------------------------------------------
        /// <summary>
        /// メニュー種別ID
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _menuCategoryID = null;

        /// --------------------------------------------------
        /// <summary>
        /// メニュー項目ID
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _menuItemID = null;

        /// --------------------------------------------------
        /// <summary>
        /// 画面タイトル
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _title = null;

        /// --------------------------------------------------
        /// <summary>
        /// 画面編集モード
        /// </summary>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private EditMode _editMode = EditMode.None;

        #endregion

        #region プロパティ

        /// --------------------------------------------------
        /// <summary>
        /// ウィンドウの閉じるボタンが有効かどうか
        /// </summary>
        /// <create>Y.Higuchi 2008/12/25</create>
        /// <update></update>
        /// --------------------------------------------------
        [Category("ウィンドウ スタイル")]
        [Description("ウィンドウの閉じるボタンが有効かどうかを示します。")]
        [DefaultValue(true), SettingsBindable(true)]
        public virtual bool CloseBoxEnabled
        {
            get { return this._closeBoxEnabled; }
            set { this._closeBoxEnabled = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// フォームを閉じる時に変更されていれば確認メッセージを表示する場合の変更フラグ
        /// </summary>
        /// <create>Y.Higuchi 2008/12/25</create>
        /// <update></update>
        /// --------------------------------------------------
        [Browsable(false)]
        protected virtual bool IsDirty
        {
            get { return this._isDirty; }
            set { this._isDirty = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// フォームを閉じる時にIsDirtyプロパティがTrueであれば確認メッセージを表示かどうか。
        /// </summary>
        /// <create>Y.Higuchi 2008/12/25</create>
        /// <update></update>
        /// --------------------------------------------------
        [Category("カスタム動作")]
        [Description("フォームを閉じる時にIsChangeプロパティがTrueであれば確認メッセージを表示かどうか。")]
        [DefaultValue(false), SettingsBindable(true)]
        public virtual bool IsChangedCloseQuestion
        {
            get { return this._isChangedCloseQuestion; }
            set { this._isChangedCloseQuestion = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// フォームを閉じる時に確認メッセージを表示かどうか。IsChangedCloseQuestionプロパティが優先されます。
        /// </summary>
        /// <create>Y.Higuchi 2008/12/25</create>
        /// <update></update>
        /// --------------------------------------------------
        [Category("カスタム動作")]
        [Description("フォームを閉じる時に確認メッセージを表示かどうか。IsChangedCloseQuestionプロパティが優先されます。")]
        [DefaultValue(false), SettingsBindable(true)]
        public virtual bool IsCloseQuestion
        {
            get { return this._isCloseQuestion; }
            set { this._isCloseQuestion = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// Enterキー押下時、次のコントロールにフォーカスを移すかどうか。
        /// </summary>
        /// <create>Y.Higuchi 2008/12/25</create>
        /// <update></update>
        /// --------------------------------------------------
        [Category("カスタム動作")]
        [Description("Enterキー押下時、次のコントロールにフォーカスを移すかどうか。")]
        [DefaultValue(true), SettingsBindable(true)]
        public virtual bool IsEnterNextControl
        {
            get { return this._isEnterNextControl; }
            set { this._isEnterNextControl = value; }
        }

        [Category("カスタム動作")]
        [Description("SheetにフォーカスがありEnterキー押下時、次のコントロールにフォーカスを移すかどうか。\r\nIsEnterNextControlがTrueの時に有効")]
        [DefaultValue(false), SettingsBindable(true)]
        public virtual bool IsEnterNextControlForSheet
        {
            get { return this._isEnterNextControlForSheet; }
            set { this._isEnterNextControlForSheet = value; }
        }

        #region Overrides

        /// --------------------------------------------------
        /// <summary>
        /// コントロールの作成時に必要な情報をカプセル化します。
        /// </summary>
        /// <create>Y.Higuchi 2008/12/25</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (this.CloseBoxEnabled != true)
                {
                    cp.ClassStyle = cp.ClassStyle | DSWUtil.Win32Message.CS_NOCLOSE;
                }
                return cp;
            }
        }
        #endregion

        /// --------------------------------------------------
        /// <summary>
        /// ログインユーザー情報を取得します。
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        [Browsable(false)]
        public UserInfo UserInfo
        {
            get { return this._userInfo; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 画面タイトルを取得または設定します。
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        [Browsable(false)]
        public string Title
        {
            get { return this._title; }
            set { this._title = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// メニュー種別ID
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        [Browsable(false)]
        public string MenuCategoryID
        {
            get { return this._menuCategoryID; }
            set { this._menuCategoryID = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// メニュー項目ID
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        [Browsable(false)]
        public string MenuItemID
        {
            get { return this._menuItemID; }
            set { this._menuItemID = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 画面編集モード
        /// </summary>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        [Browsable(false)]
        protected virtual EditMode EditMode
        {
            get { return this._editMode; }
            set { this._editMode = value; }
        }

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ（デザイナ用）
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private BaseForm()
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
        public BaseForm(UserInfo userInfo)
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
        public BaseForm(UserInfo userInfo, string title)
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
        public BaseForm(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
            : base()
        {
            this.Icon = ComResource.App;
            InitializeComponent();
            this._userInfo = userInfo;
            this.MenuCategoryID = menuCategoryID;
            this.MenuItemID = menuItemID;
            this.Title = title;
        }

        #endregion

        #region イベント

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前に発生します。
        /// </summary>
        /// <param name="e">EventArgs</param>
        /// <create>Y.Higuchi 2008/12/25</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                base.OnLoad(e);
                this.FormControlsChangeStyle(this);
                this.InitializeLoadControl();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// フォームが閉じている間に発生します。 
        /// </summary>
        /// <param name="e">CancelEventArgs</param>
        /// <create>Y.Higuchi 2008/12/25</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                base.OnClosing(e);
                DialogResult ret = DialogResult.OK;
                if (this.IsChangedCloseQuestion && this.IsDirty)
                {
                    ret = CustomMsgBoxEx.ShowQuestion(this, this.GetChangedCloseQuestionMessage(), MessageBoxButtons.OKCancel, MessageBoxDefaultButton.Button1);
                }
                else if (this.IsCloseQuestion)
                {
                    ret = CustomMsgBoxEx.ShowQuestion(this, this.GetCloseQuestionMessage(), MessageBoxButtons.OKCancel, MessageBoxDefaultButton.Button1);
                }

                if (ret.Equals(DialogResult.Cancel))
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// キーが押されると発生します。 
        /// </summary>
        /// <param name="e">KeyEventArgs</param>
        /// <create>Y.Higuchi 2008/12/25</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Enter && this.IsEnterNextControl)
            {
                DSWLabel lbl = this.ActiveControl as DSWLabel;
                if (lbl != null)
                {
                    this.InnerSelectNextControl(lbl.ActiveControl);
                }
                else
                {
                    this.InnerSelectNextControl(this.ActiveControl);
                }
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示されるたびに発生します。 
        /// </summary>
        /// <param name="e">EventArgs</param>
        /// <create>Y.Higuchi 2008/12/25</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.InitializeShownControl();
        }

        /// --------------------------------------------------
        /// <summary>
        /// コマンド キーを処理します。
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/07/22</create>
        /// <update>K.Tsutsumi 2019/02/06 64 bit OSにてF10イベントが２回発生する不具合を対応</update>
        /// --------------------------------------------------
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // 定数宣言
            const int WM_SYSKEYDOWN = 0x104;
            const int WM_SYSKEYUP = 0x105;

            if (keyData == Keys.F10)
            {
                if (msg.Msg == WM_SYSKEYUP)
                {
                    // アプリケーションキーとしての動作を殺してFunctionButtonが反応するようにする。
                    this.OnKeyUp(new KeyEventArgs(keyData));
                }
                else if (msg.Msg == WM_SYSKEYDOWN)
                {
                    // アプリケーションキーとしての動作を殺してFunctionButtonが反応するようにする。
                    this.OnKeyDown(new KeyEventArgs(keyData));
                }
                return true;
            }
            else if (keyData == Keys.F6 && this.ActiveControl.ImeMode != ImeMode.Disable)
            {
                if (msg.Msg == WM_SYSKEYUP)
                {
                    // FunctionButtonが反応するようにする。
                    this.OnKeyUp(new KeyEventArgs(keyData));
                }
                else if (msg.Msg == WM_SYSKEYDOWN)
                {
                    // FunctionButtonが反応するようにする。
                    this.OnKeyDown(new KeyEventArgs(keyData));
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// --------------------------------------------------
        /// <summary>
        /// コントロールのデータバインドを設定する。
        /// </summary>
        /// <param name="ctrl">Control</param>
        /// <create>R.Katsuo 2010/01/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private void FormControlsChangeStyle(Control ctrl)
        {
            foreach (Control child in ctrl.Controls)
            {
                this.FormControlsChangeStyle(child);
            }
            foreach (Binding bind in ctrl.DataBindings)
            {
                System.Reflection.PropertyInfo propertyinfo = ctrl.GetType().GetProperty(bind.PropertyName);
                propertyinfo.SetValue(ctrl, ((System.Configuration.ApplicationSettingsBase)bind.DataSource)[bind.BindingMemberInfo.BindingField], null);
            }
        }

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F1ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {

        }

        /// --------------------------------------------------
        /// <summary>
        /// F2ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void fbrFunction_F02Button_Click(object sender, EventArgs e)
        {

        }

        /// --------------------------------------------------
        /// <summary>
        /// F3ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void fbrFunction_F03Button_Click(object sender, EventArgs e)
        {

        }

        /// --------------------------------------------------
        /// <summary>
        /// F4ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void fbrFunction_F04Button_Click(object sender, EventArgs e)
        {

        }

        /// --------------------------------------------------
        /// <summary>
        /// F5ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void fbrFunction_F05Button_Click(object sender, EventArgs e)
        {

        }

        /// --------------------------------------------------
        /// <summary>
        /// F6ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {

        }

        /// --------------------------------------------------
        /// <summary>
        /// F7ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {

        }

        /// --------------------------------------------------
        /// <summary>
        /// F8ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void fbrFunction_F08Button_Click(object sender, EventArgs e)
        {

        }

        /// --------------------------------------------------
        /// <summary>
        /// F9ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void fbrFunction_F09Button_Click(object sender, EventArgs e)
        {

        }

        /// --------------------------------------------------
        /// <summary>
        /// F10ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void fbrFunction_F10Button_Click(object sender, EventArgs e)
        {

        }

        /// --------------------------------------------------
        /// <summary>
        /// F11ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void fbrFunction_F11Button_Click(object sender, EventArgs e)
        {

        }

        /// --------------------------------------------------
        /// <summary>
        /// F12ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void fbrFunction_F12Button_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }
        #endregion

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>Y.Higuchi 2008/12/25</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void InitializeLoadControl()
        {
            // タイトル設定
            this.Text = this.Title;
            this.lblTitle.Text = this.Text;

            // ログイン情報設定
            if (this.UserInfo != null)
            {
                this.txtRoleName.Text = this.UserInfo.RoleName;
                this.txtUserName.Text = this.UserInfo.UserName;
            }

            // ステータスのRenderer設定
            sspStatus.Renderer = new ToolStripProfessionalRenderer(new CustomProfessionalRenderer());

            // ステータスバーのEnabled設定
            this.ChangeFunctionButtonEnabled();

            // 画面クリア
            this.DisplayClear();
        }

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示されるたびにコントロールの初期化するメソッド
        /// </summary>
        /// <create>Y.Higuchi 2008/12/25</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void InitializeShownControl()
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// シートの初期化を行うメソッド
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <create>R.Katsuo 2010/01/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void InitializeSheet(Sheet sheet)
        {
            ElTabelleSheetHelper.InitializeSheet(sheet);

            //シート表示設定
            sheet.KeepHighlighted = false;
            sheet.ExitOnLastChar = false;
            sheet.DataAutoCellEditor = false;
            sheet.ProcessTabKey = false;


            //ショートカットキーの設定
            ElTabelleSheetHelper.DeleteShortcut(sheet);
            if (!sheet.ShortCuts.ContainsKey(Keys.Return))
            {
                sheet.ShortCuts.Add(Keys.Return, new KeyAction[] { KeyAction.NextCellWithWrap });
            }
            // ヘッダ色を設定
            ElTabelleSheetHelper.SetCornerHeaderColor(sheet, pnlTitle.BackColor, Color.White);
            ElTabelleSheetHelper.SetColumnHeaderColor(sheet, pnlTitle.BackColor, Color.White);
            ElTabelleSheetHelper.SetRowHeaderColor(sheet, pnlMain.BackColor, Color.Black);
            sheet.Rows.SetAllRowsHeight(22);
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
        protected virtual void DisplayClear()
        {
            this.ClearMessage();
        }

        #endregion

        #region ファンクションEnabled切替

        /// --------------------------------------------------
        /// <summary>
        /// ファンクションEnabled切替
        /// </summary>
        /// <create>Y.Higuchi 2010/05/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void ChangeFunctionButtonEnabled()
        {
            try
            {
                // テキストが未入力のボタンは非活性にする。
                this.fbrFunction.F01Button.Enabled = !string.IsNullOrEmpty(this.fbrFunction.F01ButtonText);
                this.fbrFunction.F02Button.Enabled = !string.IsNullOrEmpty(this.fbrFunction.F02ButtonText);
                this.fbrFunction.F03Button.Enabled = !string.IsNullOrEmpty(this.fbrFunction.F03ButtonText);
                this.fbrFunction.F04Button.Enabled = !string.IsNullOrEmpty(this.fbrFunction.F04ButtonText);
                this.fbrFunction.F05Button.Enabled = !string.IsNullOrEmpty(this.fbrFunction.F05ButtonText);
                this.fbrFunction.F06Button.Enabled = !string.IsNullOrEmpty(this.fbrFunction.F06ButtonText);
                this.fbrFunction.F07Button.Enabled = !string.IsNullOrEmpty(this.fbrFunction.F07ButtonText);
                this.fbrFunction.F08Button.Enabled = !string.IsNullOrEmpty(this.fbrFunction.F08ButtonText);
                this.fbrFunction.F09Button.Enabled = !string.IsNullOrEmpty(this.fbrFunction.F09ButtonText);
                this.fbrFunction.F10Button.Enabled = !string.IsNullOrEmpty(this.fbrFunction.F10ButtonText);
                this.fbrFunction.F11Button.Enabled = !string.IsNullOrEmpty(this.fbrFunction.F11ButtonText);
                this.fbrFunction.F12Button.Enabled = !string.IsNullOrEmpty(this.fbrFunction.F12ButtonText);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region メッセージ表示

        /// --------------------------------------------------
        /// <summary>
        /// メッセージ表示処理
        /// </summary>
        /// <param name="messageID">メッセージID</param>
        /// <returns>DialogResult</returns>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual DialogResult ShowMessage(string messageID)
        {
            return this.ShowMessage(messageID, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージ表示処理
        /// </summary>
        /// <param name="messageID">メッセージID</param>
        /// <param name="args">可変メッセージ</param>
        /// <returns>DialogResult</returns>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual DialogResult ShowMessage(string messageID, params string[] args)
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
                MessageImageType msgType = this.GetMessageImageType(ComFunc.GetFld(dt, 0, Def_M_MESSAGE.MESSAGE_LEVEL));
                string notFoundMsg = ComDefine.MSG_NOTFOUND_TEXT + "【" + messageID  + "】";
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

        #region 複数メッセージ用(メッセージボックスのみ対応)

        /// --------------------------------------------------
        /// <summary>
        /// メッセージ表示処理
        /// </summary>
        /// <param name="dtMessage">複数メッセージデータテーブル</param>
        /// <param name="messageID">メッセージID</param>
        /// <returns>DialogResult</returns>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual DialogResult ShowMultiMessage(DataTable dtMessage, string messageID)
        {
            return this.ShowMultiMessage(dtMessage, messageID, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージ表示処理
        /// </summary>
        /// <param name="dtMessage">複数メッセージデータテーブル</param>
        /// <param name="messageID">メッセージID</param>
        /// <param name="args">可変メッセージ</param>
        /// <returns>DialogResult</returns>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual DialogResult ShowMultiMessage(DataTable dtMessage, string messageID, params string[] args)
        {
            try
            {
                DataTable dtDistinct = ComFunc.GetDistinctMessageID(dtMessage);

                DataSet ds = this.GetMultiMessage(dtDistinct, messageID);
                // 取得失敗
                if (ds.Tables[Def_M_MESSAGE.Name] == null)
                {
                    this.ClearMessage();
                    return DialogResult.Cancel;
                }
                // メッセージとアイコンを設定
                DataTable dt = ds.Tables[Def_M_MESSAGE.Name];
                MessageImageType msgType = this.GetMessageImageType(ComFunc.GetFld(dt, 0, Def_M_MESSAGE.MESSAGE_LEVEL));
                string notFoundMsg = ComDefine.MSG_NOTFOUND_TEXT + "【" + messageID + "】";
                string msg = ComFunc.GetFld(dt, 0, Def_M_MESSAGE.MESSAGE, notFoundMsg);

                StringBuilder sbDetail = new StringBuilder();
                if (ds.Tables.Contains(ComDefine.DTTBL_MULTIRESULT))
                {
                    DataTable dtResult = ds.Tables[ComDefine.DTTBL_MULTIRESULT];
                    for (int i = 0; i < dtMessage.Rows.Count; i++)
                    {
                        DataRow[] drs = dtResult.Select("MESSAGE_ID = '" + ComFunc.GetFld(dtMessage, i, Def_M_MESSAGE.MESSAGE_ID) + "'");
                        if (drs == null || drs.Length < 1) continue;
                        string multiMsg = ComFunc.GetFld(drs[0], Def_M_MESSAGE.MESSAGE);

                        string[] multiArgs = null;
                        object parameter = ComFunc.GetFldObject(dtMessage, i, ComDefine.FLD_MESSAGE_PARAMETER);
                        if (parameter != null && parameter.GetType() == typeof(string[]))
                        {
                            multiArgs = (string[])parameter;
                        }

                        if (i == dtMessage.Rows.Count - 1)
                        {
                            sbDetail.Append(this.FormatMessage(multiMsg, multiArgs));
                        }
                        else
                        {
                            sbDetail.AppendLine(this.FormatMessage(multiMsg, multiArgs));
                        }
                    }
                }

                MessageBoxButtons msgButtonFlag = this.GetMessageBoxButtons(ComFunc.GetFld(dt, 0, Def_M_MESSAGE.BUTTON_FLAG));
                MessageBoxDefaultButton msgDefaultButton = this.GetMessageBoxDefaultButton(ComFunc.GetFld(dt, 0, Def_M_MESSAGE.DEFAULT_BUTTON));
                if (sbDetail.Length < 1)
                {
                    return this.MsgBox(msgType, msgButtonFlag, msgDefaultButton, msg, args);
                }
                return this.MultiMsgBox(msgType, msgButtonFlag, msgDefaultButton, sbDetail.ToString(), msg, args);

            }
            catch (Exception)
            {
                return DialogResult.Cancel;
            }
        }


        #endregion

        #region メッセージラベル用

        /// --------------------------------------------------
        /// <summary>
        /// メッセージ表示領域にメッセージを表示する。
        /// </summary>
        /// <param name="msgType">メッセージ種類</param>
        /// <param name="message">メッセージ</param>
        /// <create>R.Katsuo 2010/01/20</create>
        /// <update>Y.Higuchi 2010/04/26</update>
        /// --------------------------------------------------
        protected virtual void MsgLabel(MessageImageType msgType, string message)
        {
            this.MsgLabel(msgType, message, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージ表示領域にメッセージを表示する。
        /// </summary>
        /// <param name="msgType">メッセージ種類</param>
        /// <param name="message">メッセージ</param>
        /// <param name="args">可変メッセージ</param>
        /// <create>R.Katsuo 2010/01/20</create>
        /// <update>Y.Higuchi 2010/04/26</update>
        /// --------------------------------------------------
        protected virtual void MsgLabel(MessageImageType msgType, string message, params string[] args)
        {
            try
            {
                string msg = this.FormatMessage(message, args);
                SoundType type = SoundType.None;
                // メッセージ表示
                switch (msgType)
                {
                    case MessageImageType.None:
                        type = SoundType.None;
                        break;
                    case MessageImageType.Error:
                        type = SoundType.Chord;
                        break;
                    case MessageImageType.Information:
                        type = SoundType.Ding;
                        break;
                    case MessageImageType.Question:
                        type = SoundType.Ding;
                        break;
                    case MessageImageType.Warning:
                        type = SoundType.Chord;
                        break;
                    default:
                        type = SoundType.None;
                        break;
                }
                // メッセージ表示
                this.SetMessage(msgType, msg);
                MessageSound.PlaySound(type);

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスを表示する。(詳細付き)
        /// </summary>
        /// <param name="msgType">メッセージ種類</param>
        /// <param name="msgBoxButtons">表示するボタン</param>
        /// <param name="msgBoxDefaultButton">デフォルトボタン</param>
        /// <param name="detail">詳細メッセージ</param>
        /// <param name="message">メッセージ</param>
        /// <returns>DialogResult</returns>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual DialogResult MultiMsgBox(MessageImageType msgType, MessageBoxButtons msgBoxButtons, MessageBoxDefaultButton msgBoxDefaultButton, string detail, string message)
        {
            return this.MultiMsgBox(msgType, msgBoxButtons, msgBoxDefaultButton, detail, message, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスを表示する。(詳細付き)
        /// </summary>
        /// <param name="msgType">メッセージ種類</param>
        /// <param name="msgBoxButtons">表示するボタン</param>
        /// <param name="msgBoxDefaultButton">デフォルトボタン</param>
        /// <param name="detail">詳細メッセージ</param>
        /// <param name="message">メッセージ</param>
        /// <param name="args">可変メッセージ</param>
        /// <returns>DialogResult</returns>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual DialogResult MultiMsgBox(MessageImageType msgType, MessageBoxButtons msgBoxButtons, MessageBoxDefaultButton msgBoxDefaultButton, string detail, string message, params string[] args)
        {
            try
            {
                string msg = this.FormatMessage(message, args);
                // メッセージ表示
                switch (msgType)
                {
                    case MessageImageType.None:
                        return DialogResult.None;
                    case MessageImageType.Error:
                        return CustomMsgBoxEx.ShowError(this, msg, detail, msgBoxButtons, msgBoxDefaultButton);
                    case MessageImageType.Information:
                        return CustomMsgBoxEx.ShowInformation(this, msg, detail, msgBoxButtons, msgBoxDefaultButton);
                    case MessageImageType.Question:
                        return CustomMsgBoxEx.ShowQuestion(this, msg, detail, msgBoxButtons, msgBoxDefaultButton);
                    case MessageImageType.Warning:
                        return CustomMsgBoxEx.ShowWarning(this, msg, detail, msgBoxButtons, msgBoxDefaultButton);
                    default:
                        return DialogResult.Cancel;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            return DialogResult.Cancel;
        }
        #endregion

        #region メッセージボックス用

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスを表示する。
        /// </summary>
        /// <param name="msgType">メッセージ種類</param>
        /// <param name="msgBoxButtons">表示するボタン</param>
        /// <param name="msgBoxDefaultButton">デフォルトボタン</param>
        /// <param name="message">メッセージ</param>
        /// <returns>DialogResult</returns>
        /// <create>R.Katsuo 2010/01/20</create>
        /// <update>Y.Higuchi 2010/04/26</update>
        /// --------------------------------------------------
        protected virtual DialogResult MsgBox(MessageImageType msgType, MessageBoxButtons msgBoxButtons, MessageBoxDefaultButton msgBoxDefaultButton, string message)
        {
            return this.MsgBox(msgType, msgBoxButtons, msgBoxDefaultButton, message, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスを表示する。
        /// </summary>
        /// <param name="msgType">メッセージ種類</param>
        /// <param name="msgBoxButtons">表示するボタン</param>
        /// <param name="msgBoxDefaultButton">デフォルトボタン</param>
        /// <param name="message">メッセージ</param>
        /// <param name="args">可変メッセージ</param>
        /// <returns>DialogResult</returns>
        /// <create>R.Katsuo 2010/01/20</create>
        /// <update>Y.Higuchi 2010/04/26</update>
        /// --------------------------------------------------
        protected virtual DialogResult MsgBox(MessageImageType msgType, MessageBoxButtons msgBoxButtons, MessageBoxDefaultButton msgBoxDefaultButton, string message, params string[] args)
        {
            try
            {
                string msg = this.FormatMessage(message, args);
                // メッセージ表示
                switch (msgType)
                {
                    case MessageImageType.None:
                        return DialogResult.None;
                    case MessageImageType.Error:
                        return CustomMsgBoxEx.ShowError(this, msg, msgBoxButtons, msgBoxDefaultButton);
                    case MessageImageType.Information:
                        return CustomMsgBoxEx.ShowInformation(this, msg, msgBoxButtons, msgBoxDefaultButton);
                    case MessageImageType.Question:
                        return CustomMsgBoxEx.ShowQuestion(this, msg, msgBoxButtons, msgBoxDefaultButton);
                    case MessageImageType.Warning:
                        return CustomMsgBoxEx.ShowWarning(this, msg, msgBoxButtons, msgBoxDefaultButton);
                    default:
                        return DialogResult.Cancel;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            return DialogResult.Cancel;
        }

        #endregion

        /// --------------------------------------------------
        /// <summary>
        /// メッセージを取得する。
        /// </summary>
        /// <param name="messageID">メッセージID</param>
        /// <returns></returns>
        /// <create>R.Katsuo 2010/01/20</create>
        /// <update>Y.Higuchi 2010/04/26</update>
        /// --------------------------------------------------
        protected DataSet GetMessage(string messageID)
        {
            try
            {
                ConnCommon conn = new ConnCommon();
                CondCommon cond = new CondCommon(this.UserInfo);
                cond.MessageID = messageID;
                return conn.GetMessage(cond);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージを取得する。
        /// </summary>
        /// <param name="dtMessage">複数メッセージデータテーブル</param>
        /// <param name="messageID">メッセージID</param>
        /// <returns>メッセージデータ</returns>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        protected DataSet GetMultiMessage(DataTable dtMessage, string messageID)
        {
            try
            {
                ConnCommon conn = new ConnCommon();
                CondCommon cond = new CondCommon(this.UserInfo);
                cond.MessageID = messageID;
                if (dtMessage != null && dtMessage.TableName == ComDefine.DTTBL_MULTIMESSAGE && 0 < dtMessage.Rows.Count)
                {
                    string[] arrayMessageID = new string[0];
                    foreach (DataRow dr in dtMessage.Rows)
                    {
                        Array.Resize<string>(ref arrayMessageID, arrayMessageID.Length + 1);
                        arrayMessageID[arrayMessageID.Length - 1] = ComFunc.GetFld(dr, Def_M_MESSAGE.MESSAGE_ID);
                    }
                    cond.ArrayMessageID = arrayMessageID;
                }
                return conn.GetMultiMessage(cond);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 取得したメッセージのフォーマットを行う。
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="args">可変メッセージ</param>
        /// <returns>フォーマットしたメッセージ</returns>
        /// <create>R.Katsuo 2010/01/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected string FormatMessage(string message, params string[] args)
        {
            try
            {
                // 改行コードチェック
                if (message.Contains("\\r\\n"))
                {
                    message = message.Replace("\\r\\n", "\r\n");
                }

                // 可変文字列チェック
                if (args != null && 0 < args.Length)
                {
                    message = string.Format(message, args);
                }

                return message;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージアイコンタイプを取得する。
        /// </summary>
        /// <param name="msgType">MESSAGE_LEVEL</param>
        /// <returns>MessageImageType</returns>
        /// <create>R.Katsuo 2010/01/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected MessageImageType GetMessageImageType(string msgType)
        {
            try
            {
                return (MessageImageType)UtilConvert.ToInt32(msgType);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスのボタンを取得する
        /// </summary>
        /// <param name="msgBoxButtons">BUTTON_FLAG</param>
        /// <returns>MessageBoxButtons</returns>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected MessageBoxButtons GetMessageBoxButtons(string msgBoxButtons)
        {
            try
            {
                return (MessageBoxButtons)UtilConvert.ToInt32(msgBoxButtons);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージボックスのデフォルトボタンを取得する
        /// </summary>
        /// <param name="msgBoxDefaultButton">DEFAULT_BUTTON</param>
        /// <returns>MessageBoxDefaultButton</returns>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected MessageBoxDefaultButton GetMessageBoxDefaultButton(string msgBoxDefaultButton)
        {
            try
            {
                return (MessageBoxDefaultButton)UtilConvert.ToInt32(msgBoxDefaultButton);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        #endregion

        #region メッセージ追加表示

        /// --------------------------------------------------
        /// <summary>
        /// メッセージ追加表示処理
        /// </summary>
        /// <param name="messageID">メッセージID</param>
        /// <returns>DialogResult</returns>
        /// <create>J.Chen 2024/01/18</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual DialogResult AppendMessage(string messageID)
        {
            return this.AppendMessage(messageID, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージ追加表示処理
        /// </summary>
        /// <param name="messageID">メッセージID</param>
        /// <param name="args">可変メッセージ</param>
        /// <returns>DialogResult</returns>
        /// <create>J.Chen 2024/01/18</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual DialogResult AppendMessage(string messageID, params string[] args)
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
                MessageImageType msgType = this.GetMessageImageType(ComFunc.GetFld(dt, 0, Def_M_MESSAGE.MESSAGE_LEVEL));
                string notFoundMsg = ComDefine.MSG_NOTFOUND_TEXT + "【" + messageID + "】";
                string msg = ComFunc.GetFld(dt, 0, Def_M_MESSAGE.MESSAGE, notFoundMsg);
                string msgFlag = ComFunc.GetFld(dt, 0, Def_M_MESSAGE.MESSAGE_FLAG, MESSAGE_FLAG.LABEL_VALUE1);

                if (msgFlag.Equals(MESSAGE_FLAG.LABEL_VALUE1))
                {
                    this.AppendMsgLabel(msgType, msg, args);
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

        /// --------------------------------------------------
        /// <summary>
        /// メッセージ表示領域にメッセージを表示する。（メッセージ追加用）
        /// </summary>
        /// <param name="msgType">メッセージ種類</param>
        /// <param name="message">メッセージ</param>
        /// <param name="args">可変メッセージ</param>
        /// <create>J.Chen 2024/01/18</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void AppendMsgLabel(MessageImageType msgType, string message, params string[] args)
        {
            try
            {
                string msg = this.FormatMessage(message, args);
                SoundType type = SoundType.None;
                // メッセージ表示
                switch (msgType)
                {
                    case MessageImageType.None:
                        type = SoundType.None;
                        break;
                    case MessageImageType.Error:
                        type = SoundType.Chord;
                        break;
                    case MessageImageType.Information:
                        type = SoundType.Ding;
                        break;
                    case MessageImageType.Question:
                        type = SoundType.Ding;
                        break;
                    case MessageImageType.Warning:
                        type = SoundType.Chord;
                        break;
                    default:
                        type = SoundType.None;
                        break;
                }
                // メッセージ表示
                this.AddMessage(msgType, msg);
                MessageSound.PlaySound(type);

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region コンボボックス

        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスのDataSourceを作成する。
        /// 既定の ValueMember はVALUE1、DisplayMemberはITEMNAME。
        /// </summary>
        /// <param name="cmbBox">コンボボックス</param>
        /// <param name="groupCD">グループコード</param>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update>Y.Higuchi 2010/02/26</update>
        /// --------------------------------------------------
        protected virtual void MakeCmbBox(DSWComboBox cmbBox, string groupCD)
        {
            MakeCmbBox(cmbBox, groupCD, false);
        }

        /// --------------------------------------------------
        /// <summary>コンボボックスのDataSourceを作成する。
        /// 既定の ValueMember はVALUE1、DisplayMemberはITEMNAME。
        /// </summary>
        /// <param name="cmbBox">コンボボックス</param>
        /// <param name="groupCD">グループコード</param>
        /// <param name="isInsertSpace">true:空白行挿入/false:空白行無</param>
        /// <create>Y.Higuchi 2010/02/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void MakeCmbBox(DSWComboBox cmbBox, string groupCD, bool isInsertSpace)
        {
            try
            {
                this.MakeCmbBox(cmbBox, groupCD, Def_M_COMMON.VALUE1, Def_M_COMMON.ITEM_NAME, isInsertSpace);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスのDataSourceを作成する。
        /// </summary>
        /// <param name="cmbBox">コンボボックス</param>
        /// <param name="groupCD">グループコード</param>
        /// <param name="valueMember">実際の値として使用したい列名</param>
        /// <param name="displayMember">表示したい列名</param>
        /// <param name="sortType">ソートタイプ</param>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update>Y.Higuchi 2010/04/19</update>
        /// --------------------------------------------------
        protected void MakeCmbBox(DSWComboBox cmbBox, string groupCD, string valueMember, string displayMember)
        {
            this.MakeCmbBox(cmbBox, groupCD, valueMember, displayMember, false);
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスのDataSourceを作成する。
        /// </summary>
        /// <param name="cmbBox">コンボボックス</param>
        /// <param name="groupCD">グループコード</param>
        /// <param name="valueMember">実際の値として使用したい列名</param>
        /// <param name="displayMember">表示したい列名</param>
        /// <param name="isInsertSpace">true:空白行挿入/false:空白行無</param>
        /// <create>Y.Higuchi 2010/02/26</create>
        /// <update>Y.Higuchi 2010/04/19</update>
        /// --------------------------------------------------
        protected void MakeCmbBox(DSWComboBox cmbBox, string groupCD, string valueMember, string displayMember, bool isInsertSpace)
        {
            try
            {
                DataSet ds = this.GetCommon(groupCD);

                // DataSet取得失敗
                if (ds.Tables[0] == null || ds.Tables[Def_M_COMMON.Name].Rows.Count == 0)
                {
                    return;
                }
                MakeCmbBox(cmbBox, ds.Tables[0], valueMember, displayMember, Def_M_COMMON.DEFAULT_VALUE, isInsertSpace);

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスのDataSourceを作成する。
        /// </summary>
        /// <param name="cmbBox">コンボボックス</param>
        /// <param name="dt">データソースに使用するデータテーブル</param>
        /// <param name="valueMember">実際の値として使用したい列名</param>
        /// <param name="displayMember">表示したい列名</param>
        /// <param name="defaultMember">デフォルト値を判定する列名</param>
        /// <param name="isInsertSpace">true:空白行挿入/false:空白行無</param>
        /// <create>Y.Higuchi 2010/04/27</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void MakeCmbBox(DSWComboBox cmbBox, DataTable dt, string valueMember, string displayMember, string defaultMember, bool isInsertSpace)
        {
            try
            {
                if (dt == null || dt.Rows.Count < 1)
                {
                    return;
                }
                DataTable dtSource = dt.Copy();

                if (isInsertSpace)
                {
                    dtSource.Rows.InsertAt(dtSource.NewRow(), 0);
                }

                string defaultValue = string.Empty;
                if (dtSource.Columns.Contains(defaultMember) && dtSource.Columns.Contains(valueMember))
                {
                    foreach (DataRow dr in dtSource.Rows)
                    {
                        if (ComFunc.GetFld(dr, defaultMember).Equals(DEFAULT_VALUE.ENABLE_VALUE1))
                        {
                            defaultValue = ComFunc.GetFld(dr, valueMember);
                            break;
                        }
                    }

                }
                // DataTableをコンボボックスに設定
                cmbBox.ValueMember = valueMember;
                cmbBox.DisplayMember = displayMember;
                cmbBox.DataSource = dtSource;
                cmbBox.SelectedValue = defaultValue;

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

        }

        /// --------------------------------------------------
        /// <summary>
        /// 汎用マスタのデータを取得する。
        /// </summary>
        /// <param name="groupCD">グループコード</param>
        /// <returns>DataSet</returns>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update>Y.Higuchi 2010/04/27</update>
        /// --------------------------------------------------
        protected virtual DataSet GetCommon(string groupCD)
        {
            try
            {
                CondCommon cond = new CondCommon(this.UserInfo);
                cond.GroupCD = groupCD;

                ConnCommon conn = new ConnCommon();
                return conn.GetCommon(cond);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 確認メッセージ

        /// --------------------------------------------------
        /// <summary>
        /// フォームを閉じる時に変更されていれば確認メッセージを表示する場合のメッセージ
        /// </summary>
        /// <returns>メッセージ</returns>
        /// <create>Y.Higuchi 2008/12/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual string GetChangedCloseQuestionMessage()
        {
            return SystemBase.Properties.Resources.BaseForm_ChangedCloseQuestion;
        }

        /// --------------------------------------------------
        /// <summary>
        /// フォームを閉じる時に確認メッセージを表示する場合のメッセージ
        /// </summary>
        /// <returns>メッセージ</returns>
        /// <create>Y.Higuchi 2008/12/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual string GetCloseQuestionMessage()
        {
            return SystemBase.Properties.Resources.BaseForm_CloseQuestion;
        }

        #endregion

        #region Message

        /// --------------------------------------------------
        /// <summary>
        /// メッセージ表示エリアのメッセージをクリアする。
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        protected void ClearMessage()
        {
            this.lblMessage.ClearMessage();
            this.lblMessage.Refresh();
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージ表示エリアにメッセージを設定する。
        /// </summary>
        /// <param name="messageImage">アイコンタイプ</param>
        /// <param name="message">メッセージ</param>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        protected void SetMessage(MessageImageType messageImage, string message)
        {
            this.lblMessage.SetMessage(messageImage, message);
            this.lblMessage.Refresh();
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージ表示エリアにメッセージを追加する。
        /// </summary>
        /// <param name="messageImage">アイコンタイプ</param>
        /// <param name="message">メッセージ</param>
        /// <create>J.Chen 2024/01/18</create>
        /// <update></update>
        /// --------------------------------------------------
        protected void AddMessage(MessageImageType messageImage, string message)
        {
            this.lblMessage.AddMessage(messageImage, message);
            this.lblMessage.Refresh();
        }

        #endregion

        #region ステータスラベル

        /// --------------------------------------------------
        /// <summary>
        /// ステータスメッセージのクリア
        /// </summary>
        /// <create>Y.Higuchi 2010/01/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected void ClearStatusMessage()
        {
            this.lblStatusMessage.Text = string.Empty;
        }

        /// --------------------------------------------------
        /// <summary>
        /// ステータスメッセージの設定
        /// </summary>
        /// <param name="msg">メッセージ</param>
        /// <create>Y.Higuchi 2010/01/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public void SetStatusMessage(string msg)
        {
            this.lblStatusMessage.Text = msg;
        }

        #endregion

        #region シートの列設定

        /// --------------------------------------------------
        /// <summary>
        /// El Tabelle Sheetの列設定用メソッド
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <param name="colIndex">列インデックス</param>
        /// <param name="title">列ヘッダ文字列</param>
        /// <param name="isHidden">列非表示(true:非表示/false:表示)</param>
        /// <param name="isLock">列ロック(true:ロック/false:ロックしない)</param>
        /// <create>Y.Higuchi 2010/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void SetElTabelleColumn(Sheet sheet, int colIndex, string title, bool isHidden, bool isLock)
        {
            this.SetElTabelleColumn(sheet, colIndex, title, isHidden, isLock, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// El Tabelle Sheetの列設定用メソッド
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <param name="colIndex">列インデックス</param>
        /// <param name="title">列ヘッダ文字列</param>
        /// <param name="isHidden">列非表示(true:非表示/false:表示)</param>
        /// <param name="isLock">列ロック(true:ロック/false:ロックしない)</param>
        /// <param name="width">列幅</param>
        /// <create>Y.Higuchi 2010/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void SetElTabelleColumn(Sheet sheet, int colIndex, string title, bool isHidden, bool isLock, int width)
        {
            this.SetElTabelleColumn(sheet, colIndex, title, isHidden, isLock, null, width);
        }

        /// --------------------------------------------------
        /// <summary>
        /// El Tabelle Sheetの列設定用メソッド
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <param name="colIndex">列インデックス</param>
        /// <param name="title">列ヘッダ文字列</param>
        /// <param name="isHidden">列非表示(true:非表示/false:表示)</param>
        /// <param name="isLock">列ロック(true:ロック/false:ロックしない)</param>
        /// <param name="dataField">データフィールド</param>
        /// <create>Y.Higuchi 2010/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void SetElTabelleColumn(Sheet sheet, int colIndex, string title, bool isHidden, bool isLock, string dataField)
        {
            this.SetElTabelleColumn(sheet, colIndex, title, isHidden, isLock, dataField, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// El Tabelle Sheetの列設定用メソッド
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <param name="colIndex">列インデックス</param>
        /// <param name="title">列ヘッダ文字列</param>
        /// <param name="isHidden">列非表示(true:非表示/false:表示)</param>
        /// <param name="isLock">列ロック(true:ロック/false:ロックしない)</param>
        /// <param name="dataField">データフィールド</param>
        /// <param name="width">列幅</param>
        /// <create>Y.Higuchi 2010/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void SetElTabelleColumn(Sheet sheet, int colIndex, string title, bool isHidden, bool isLock, string dataField, int width)
        {
            this.SetElTabelleColumn(sheet, colIndex, title, isHidden, isLock, dataField, null, width);
        }

        /// --------------------------------------------------
        /// <summary>
        /// El Tabelle Sheetの列設定用メソッド
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <param name="colIndex">列インデックス</param>
        /// <param name="title">列ヘッダ文字列</param>
        /// <param name="isHidden">列非表示(true:非表示/false:表示)</param>
        /// <param name="isLock">列ロック(true:ロック/false:ロックしない)</param>
        /// <param name="dataField">データフィールド</param>
        /// <param name="editor">列のEditor</param>
        /// <create>Y.Higuchi 2010/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void SetElTabelleColumn(Sheet sheet, int colIndex, string title, bool isHidden, bool isLock, string dataField, GrapeCity.Win.ElTabelle.Editors.GridEditor editor)
        {
            this.SetElTabelleColumn(sheet, colIndex, title, isHidden, isLock, dataField, editor, -1);
        }

        /// --------------------------------------------------
        /// <summary>
        /// El Tabelle Sheetの列設定用メソッド
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <param name="colIndex">列インデックス</param>
        /// <param name="title">列ヘッダ文字列</param>
        /// <param name="isHidden">列非表示(true:非表示/false:表示)</param>
        /// <param name="isLock">列ロック(true:ロック/false:ロックしない)</param>
        /// <param name="dataField">データフィールド</param>
        /// <param name="editor">列のEditor</param>
        /// <param name="width">列幅</param>
        /// <create>Y.Higuchi 2010/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void SetElTabelleColumn(Sheet sheet, int colIndex, string title, bool isHidden, bool isLock, string dataField, GrapeCity.Win.ElTabelle.Editors.GridEditor editor, int width)
        {
            // インデックスが有効範囲か判定
            if (sheet.Columns.Count <= colIndex)
            {
                // インデックスが有効範囲で無い場合は追加
                sheet.MaxColumns = colIndex + 1;
            }

            // Editorの設定
            if (editor != null)
            {
                sheet.Columns[colIndex].Editor = editor;
            }

            // DataFieldの設定
            if (!string.IsNullOrEmpty(dataField))
            {
                sheet.Columns[colIndex].DataField = dataField;
            }

            // 列幅の設定
            if (0 < width)
            {
                sheet.Columns[colIndex].Width = width;
            }

            // キャプションの設定
            sheet.ColumnHeaders[colIndex].Caption = title;
            // 列の表示/非表示の設定
            sheet.Columns[colIndex].Hidden = isHidden;
            // 列のロックの設定
            sheet.Columns[colIndex].Lock = isLock;
        }

        #endregion

        #region ログイン情報更新用(通常は使用しない)

        /// --------------------------------------------------
        /// <summary>
        /// ログイン情報セット用メソッド
        /// ※通常は使用しないでください。
        /// </summary>
        /// <param name="userInfo">ログイン情報</param>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual void SetUserInfo(UserInfo userInfo)
        {
            this._userInfo = userInfo;
        }

        #endregion

        #region フォーカス移動

        /// --------------------------------------------------
        /// <summary>
        /// 対象コントロールの次のコントロールにフォーカスを移動する。
        /// </summary>
        /// <param name="ctrl">対象コントロール</param>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void InnerSelectNextControl(Control ctrl)
        {
            try
            {
                if (!this.IsEnterNextControlForSheet && ctrl as Sheet != null) return;
                if (ctrl as TextBoxBase != null)
                {
                    if ((ctrl as TextBoxBase).Multiline == true)
                    {
                        TextBox txt = ctrl as TextBox;
                        DSWTextBox dswTxt = ctrl as DSWTextBox;
                        if (txt != null && txt.AcceptsReturn)
                        {
                            return;
                        }
                        else if (dswTxt != null && dswTxt.AcceptsReturn)
                        {
                            return;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                this.SelectNextControl(ctrl, true, true, true, true);
            }
            catch(Exception){}
        }

        #endregion

        #region タイトルの再設定

        /// --------------------------------------------------
        /// <summary>
        /// タイトルの再設定
        /// </summary>
        /// <create>T.Sakiori 2012/05/08</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void ResetTitle()
        {
            // タイトル設定
            this.Text = this.Title;
            this.lblTitle.Text = this.Text;
        }

        #endregion

        #region バージョン情報表示

        /// --------------------------------------------------
        /// <summary>
        /// バージョン情報表示
        /// </summary>
        /// <create>J.Chen 2024/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private void lblStatusMessage_Click(object sender, EventArgs e)
        {
            if (this.lblTitle.Text != Resources.BaseSDIMenu_Menu) return;

            try
            {
                // バージョン情報表示
                VersionJoho frm = new VersionJoho(this.UserInfo, this.lblStatusMessage.Text);
                frm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

        }

        #endregion
    }
}
