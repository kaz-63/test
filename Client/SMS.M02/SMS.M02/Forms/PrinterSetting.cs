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
using SystemBase.Util;

namespace SMS.M02.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// プリンタ設定
    /// </summary>
    /// <create>Y.Higuchi 2010/08/24</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class PrinterSetting : SystemBase.Forms.CustomOrderForm
    {
        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="menuCategoryID">メニュー種別ID</param>
        /// <param name="menuItemID">メニュー項目ID</param>
        /// <param name="title">画面タイトル</param>
        /// <create>Y.Higuchi 2010/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public PrinterSetting(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
            : base(userInfo, menuCategoryID, menuItemID, title)
        {
            InitializeComponent();
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>Y.Higuchi 2010/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // フォーム
                this.EditMode = SystemBase.EditMode.Update;
                this.IsCloseQuestion = true;
                // コントロールの初期化
                int prnCnt = System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count;
                string[] normalPrinter = new string[prnCnt];
                string[] tagPrinter = new string[prnCnt];
                for (int i = 0; i < System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count; i++)
                {
                    string prnName = System.Drawing.Printing.PrinterSettings.InstalledPrinters[i];
                    normalPrinter[i] = prnName;
                    tagPrinter[i] = prnName;
                }
                this.cboNormalPrinter.DataSource = normalPrinter;
                this.cboTagPrinter.DataSource = tagPrinter;
                this.cboNormalPrinter.SelectedIndex = this.GetPrinterIndex(normalPrinter, LocalSetting.GetNormalPrinter());
                this.cboTagPrinter.SelectedIndex = this.GetPrinterIndex(tagPrinter, LocalSetting.GetTAGPrinter());
                
                // 修正確認メッセージ
                // 画面の内容で保存します。\r\nよろしいですか？
                this.MsgUpdateConfirm = "A9999999011";
                // 修正完了メッセージ
                this.MsgUpdateEnd = "A9999999045";
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
        /// <create>Y.Higuchi 2010/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                this.cboNormalPrinter.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 編集内容実行

        #region 制御メソッド

        /// --------------------------------------------------
        /// <summary>
        /// 編集内容実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Higuchi 2010/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            bool ret = base.RunEdit();
            try
            {
                return ret;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region 修正処理

        /// --------------------------------------------------
        /// <summary>
        /// 修正処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Higuchi 2010/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                LocalSetting.SetNormalPrinter(this.GetSelectPrinterName(this.cboNormalPrinter));
                LocalSetting.SetTAGPrinter(this.GetSelectPrinterName(this.cboTagPrinter));
                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
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
        /// <create>Y.Higuchi 2010/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F01Button_Click(sender, e);
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
        /// <create>Y.Higuchi 2010/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F12Button_Click(object sender, EventArgs e)
        {
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

        #endregion

        #region プリンタ関係

        /// --------------------------------------------------
        /// <summary>
        /// プリンタのインデックスを取得(一覧に無い場合はデフォルトプリンタを返す)
        /// </summary>
        /// <param name="printers">プリンタの一覧</param>
        /// <param name="targetName">対象プリンタ</param>
        /// <returns>インデックス</returns>
        /// <create>Y.Higuchi 2010/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private int GetPrinterIndex(string[] printers, string targetName)
        {
            try
            {
                int ret = -1;
                ret = Array.IndexOf<string>(printers, targetName);
                if (ret < 0)
                {
                    // ----- デフォルトプリンタを取得する。 -----
                    //PrintDocumentの作成
                    System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
                    ret = Array.IndexOf<string>(printers, pd.PrinterSettings.PrinterName);
                }
                return ret;
            }
            catch
            {
                return -1;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 対象コンボボックスの選択されているプリンタ名を取得(未選択時はデフォルトプリンタ)
        /// </summary>
        /// <param name="cbo">対象コンボボックス</param>
        /// <returns>プリンタ名</returns>
        /// <create>Y.Higuchi 2010/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private string GetSelectPrinterName(ComboBox cbo)
        {
            try
            {
                string prnName = cbo.Text;
                if (string.IsNullOrEmpty(prnName))
                {
                    // ----- デフォルトプリンタを取得する。 -----
                    //PrintDocumentの作成
                    System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
                    prnName = pd.PrinterSettings.PrinterName;
                }
                return prnName;
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion
    }
}
