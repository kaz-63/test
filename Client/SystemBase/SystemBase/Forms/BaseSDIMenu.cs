using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DSWUtil;
using DSWControl;
using WsConnection;
using WsConnection.WebRefCommon;
using Commons;
using SystemBase.Controls;
using SystemBase.Util;
using SystemBase;
using SystemBase.Properties;

namespace SystemBase.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// SDIメニューベースクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/04/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class BaseSDIMenu : CustomOrderForm
    {

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public BaseSDIMenu(UserInfo userInfo)
            // 2011/02/21 K.Tsutsumi Change カタカナ禁止
            //: this(userInfo, "メニュー")
            : this(userInfo, Resources.BaseSDIMenu_Menu)
            // ↑
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="title">画面タイトル</param>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public BaseSDIMenu(UserInfo userInfo, string title)
            : base(userInfo, title)
        {
            InitializeComponent();
        }

        #endregion

        #region イベント

        #region ログアウトボタン

        /// --------------------------------------------------
        /// <summary>
        /// ログアウトボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void btnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                // ログイン画面呼出
                this.DialogResult = DialogResult.OK;
                this.CloseForms();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region パスワード変更ボタン

        /// --------------------------------------------------
        /// <summary>
        /// パスワード変更ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnPasswordChange_Click(object sender, EventArgs e)
        {
            try
            {
                using (PasswordChangeForm frm = new PasswordChangeForm(this.UserInfo))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        // パスワード有効期限警告
                        this.SetUserPassExpirationWarning();
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 閉じるボタン

        /// --------------------------------------------------
        /// <summary>
        /// 閉じるボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                // アプリケーションを終了します。\r\nよろしいですか？
                if (this.ShowMessage("FW010020001") != DialogResult.OK) return;
                // ログイン画面呼出
                this.DialogResult = DialogResult.No;
                this.CloseForms();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region メニュー

        /// --------------------------------------------------
        /// <summary>
        /// メニュー選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <create>Y.Higuchi 2010/05/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private void mnuMenu_MenuItemClick(object sender, DSWControl.MenuItemEventArgs args)
        {
            try
            {
                // DataRowが取得できなければ処理を抜ける
                DataRow dr = args.Data as DataRow;
                if (dr == null) return;

                ReflectionShow(dr);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            this.SetStatusMessage(Resources.BaseSDIMenu_Version + ComFunc.GetAppVersion());

            if (this.UserInfo.SysInfo.EnabledLogin == ENABLED_LOGIN.DISABLE_VALUE1)
            {
                this.btnLogout.Visible = false;
            }

            DataSet ds = this.GetMenu();

            if (!ds.Tables.Contains(ComDefine.DTTBL_MENU) && ds.Tables[ComDefine.DTTBL_MENU].Rows.Count < 1)
            {
                return;
            }

            // ----- 画像の読込み -----
            // 入力フォーム
            this.MenuImageAdd(this.mnuMenu, ComDefine.MENU_IMAGE_FORM, ComResource.Form);
            // 照会フォーム
            this.MenuImageAdd(this.mnuMenu,ComDefine.MENU_IMAGE_SEARCH_FORM,ComResource.SearchForm);
            // マスタメンテナンス
            this.MenuImageAdd(this.mnuMenu, ComDefine.MENU_IMAGE_MASTER, ComResource.Master);
            // 検索ダイアログ
            this.MenuImageAdd(this.mnuMenu, ComDefine.MENU_IMAGE_SEARCH, ComResource.Search);
            // 印刷フォーム
            this.MenuImageAdd(this.mnuMenu, ComDefine.MENU_IMAGE_PRINTER, ComResource.Printer);
            // 印刷設定フォームフォーム
            this.MenuImageAdd(this.mnuMenu, ComDefine.MENU_IMAGE_PRINTER_SETTING, ComResource.PrinterSetting);
            // その他
            this.MenuImageAdd(this.mnuMenu,ComDefine.MENU_IMAGE_BATCH,ComResource.Batch);
            
            // メニュー種別
            mnuMenu.CategoryBindSetting.IDMember = Def_M_MENU_CATEGORY.MENU_CATEGORY_ID;
            mnuMenu.CategoryBindSetting.TextMember = Def_M_MENU_CATEGORY.MENU_CATEGORY_NAME;
            // メニュー項目
            mnuMenu.ItemBindSetting.IDMember = Def_M_MENU_ITEM.MENU_ITEM_ID;
            mnuMenu.ItemBindSetting.TextMember = Def_M_MENU_ITEM.MENU_ITEM_NAME;
            mnuMenu.ItemBindSetting.ValueMember = Def_M_MENU_ITEM.CLASS_NAME;
            mnuMenu.ItemBindSetting.ImageMember = Def_M_MENU_ITEM.MENU_ITEM_IMAGE;
            // メニュー作成
            mnuMenu.SetMenuBinding(ds.Tables[ComDefine.DTTBL_MENU]);

            // パスワード有効期限警告
            this.SetUserPassExpirationWarning();

            // パスワード変更ボタンの設定
            if (this.UserInfo.SysInfo.EnabledLogin == ENABLED_LOGIN.ENABLE_VALUE1 &&
                    this.UserInfo.SysInfo.MyPasswordChange == MY_PASSWORD_CHANGE.ENABLE_VALUE1)
            {
                this.btnPasswordChange.Visible = true;
            }
            else
            {
                this.btnPasswordChange.Visible = false;
            }
        }

        #region メニューイメージ追加

        /// --------------------------------------------------
        /// <summary>
        /// メニューイメージ追加
        /// </summary>
        /// <param name="menu">DSWMenu</param>
        /// <param name="key">イメージの名前</param>
        /// <param name="img">Image</param>
        /// <create>Y.Higuchi 2010/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private void MenuImageAdd(DSWMenu menu, string key, Image img)
        {
            try
            {
                MenuImage menuImage = new MenuImage();
                menuImage.Name = key;
                menuImage.Image = img;
                menu.MenuImages.Add(menuImage);
            }
            catch{}
        }

        #endregion

        #endregion

        #region メニューデータ取得

        /// --------------------------------------------------
        /// <summary>
        /// メニューデータ取得
        /// </summary>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update>H.Tajimi 2018/09/05 多言語対応</update>
        /// --------------------------------------------------
        protected virtual DataSet GetMenu()
        {
            try
            {
                ConnCommon conn = new ConnCommon();
                CondCommon cond = new CondCommon(this.UserInfo);

                // 検索条件設定
                cond.UserID = this.UserInfo.UserID;
                cond.PcName = this.UserInfo.PcName;
                cond.MenuItemFlag = MENU_ITEM_FLAG.PC_VALUE1;
                cond.TerminalRole = this.UserInfo.SysInfo.TerminalRole;
                cond.TerminalGuest = this.UserInfo.SysInfo.TerminalGuest;

                return conn.GetMenu(cond);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return null;
            }
        }

        #endregion

        #region 画面起動

        /// --------------------------------------------------
        /// <summary>
        /// 画面起動
        /// </summary>
        /// <param name="dr">選択メニューDataRow</param>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void ReflectionShow(DataRow dr)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                string menuCategoryID = ComFunc.GetFld(dr, Def_M_MENU_CATEGORY.MENU_CATEGORY_ID);
                string menuItemID = ComFunc.GetFld(dr, Def_M_MENU_ITEM.MENU_ITEM_ID);
                string dispTitle = ComFunc.GetFld(dr, Def_M_MENU_ITEM.DISP_TITLE);
                string className = ComFunc.GetFld(dr, Def_M_MENU_ITEM.CLASS_NAME);
                string menuItemImage = ComFunc.GetFld(dr,Def_M_MENU_ITEM.MENU_ITEM_IMAGE);
                TypeLoader loader = new TypeLoader(Application.StartupPath);
                Type type = loader.GetClassType(className, LoadDllCollection.ToArray());

                foreach(Form openFrm in Application.OpenForms)
                {
                    BaseForm baseFrm = openFrm as BaseForm;
                    if (baseFrm != null && baseFrm.MenuCategoryID.Equals(menuCategoryID) && baseFrm.MenuItemID.Equals(menuItemID))
                    {
                        if (openFrm.WindowState == FormWindowState.Minimized)
                        {
                            openFrm.WindowState = FormWindowState.Normal;
                        }
                        openFrm.Activate();
                        return;
                    }
                }

                // クラスが取得出来ない場合は処理を抜ける
                if (type == null) return;

                object obj = type.InvokeMember(null, 
                                               System.Reflection.BindingFlags.CreateInstance, 
                                               null, 
                                               null, 
                                               new object[] { this.UserInfo, menuCategoryID, menuItemID, dispTitle });
                // BaseFormにキャスト
                BaseForm frm = obj as BaseForm;
                // キャスト出来なければ処理を抜ける
                if (frm == null) return;


                if (!string.IsNullOrEmpty(menuItemImage) && this.mnuMenu.MenuImages != null)
                {
                    if (this.mnuMenu.MenuImages.Contains(menuItemImage))
                    {
                        Bitmap bmp = new Bitmap(this.mnuMenu.MenuImages[menuItemImage].Image);
                        frm.Icon = Icon.FromHandle(bmp.GetHicon());
                    }
                }

                // フォームの表示
                frm.Show();
                frm.Activate();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        #region ログインフォーム以外閉じる

        /// --------------------------------------------------
        /// <summary>
        /// ログインフォーム以外閉じる
        /// </summary>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void CloseForms()
        {
            try
            {
                for (int i = Application.OpenForms.Count - 1; 0 <= i; i--)
                {
                    Form frm = Application.OpenForms[i];
                    if (frm as BaseLogin != null)
                    {
                        continue;
                    }
                    if (!frm.IsDisposed)
                    {
                        BaseForm baseFrm = frm as BaseForm;
                        if(baseFrm!=null)
                        {
                            // 問答無用
                            baseFrm.IsChangedCloseQuestion = false;
                            baseFrm.IsCloseQuestion = false;
                        }
                        frm.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region パスワード有効期限警告

        /// --------------------------------------------------
        /// <summary>
        /// パスワード有効期限警告
        /// </summary>
        /// <create>Y.Higuchi 2010/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void SetUserPassExpirationWarning()
        {
            this.ClearMessage();
            Application.Idle += new EventHandler(Application_Idle);
        }

        /// --------------------------------------------------
        /// <summary>
        /// パスワード有効期限警告(タイミングを遅らせる)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2012/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private void Application_Idle(object sender, EventArgs e)
        {
            Application.Idle -= new EventHandler(Application_Idle);

            string errorMsgID = string.Empty;
            int days = 0;
            if (!BaseFunc.CheckUserPassExpirationWarning(this.UserInfo, ref errorMsgID, ref days))
            {
                this.ShowMessage(errorMsgID, days.ToString());
            }
        }

        #endregion

    }
}
