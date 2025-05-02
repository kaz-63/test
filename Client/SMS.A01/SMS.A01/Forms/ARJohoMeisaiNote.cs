using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using DSWUtil;
using DSWControl.DSWComboBox;
using WsConnection;
using WsConnection.WebRefCommon;
using Commons;
using SystemBase.Controls;
using SystemBase.Util;
using GrapeCity.Win.ElTabelle;
using ElTabelleHelper;

using WsConnection.WebRefA01;
using WsConnection.WebRefAttachFile;
using System.Diagnostics;
using SMS.P02.Forms;
using SMS.A01.Properties;
using GrapeCity.Win.ElTabelle.Editors;
using DSWControl.DSWRichTextBox;

namespace SMS.A01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// AR情報明細詳細
    /// </summary>
    /// <create>D.Okumura 2019/06/13</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ARJohoMeisaiNote : SystemBase.Forms.CustomOrderForm
    {

        #region Fields
        private string _listFlagName = string.Empty;
        private int _maxLength;
        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="menuCategoryID">メニュー種別ID</param>
        /// <param name="menuItemID">メニュー項目ID</param>
        /// <param name="title">画面タイトル</param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public ARJohoMeisaiNote(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
            : base(userInfo, menuCategoryID, menuItemID, title)
        {
            InitializeComponent();
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="nonyusakiCd">納入先コード</param>
        /// <param name="nonyusakiName">納入先名称</param>
        /// <param name="listFlag">リスト</param>
        /// <param name="filePath">RTF情報</param>
        /// <param name="maxLength">最大長</param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public ARJohoMeisaiNote(UserInfo userInfo, SystemBase.EditMode editMode, string title, CondA1 cond, string listFlagName, string filePath, int maxLength)
            : base(userInfo, title)
        {
            this.EditMode = editMode;
            this._listFlagName = listFlagName;
            this.Condition = cond;
            this.FilePath = filePath;
            this._maxLength = maxLength;


            InitializeComponent();
        }

        #endregion

        #region プロパティ

        /// --------------------------------------------------
        /// <summary>
        /// 表示する際に使用した条件
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public CondA1 Condition { get; private set; }

        /// --------------------------------------------------
        /// <summary>
        /// 反映するデータ
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// <remarks>
        /// OKボタン押下時に設定される。
        /// 画面表示時まで保持し、画面を閉じた後に反映される。
        /// NULLの場合は、キャンセルされたものとしてみなすこと。
        /// </remarks>
        /// --------------------------------------------------
        public string FilePath { get; private set; }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // ここにコントロールの初期化を記述する。
                // ベースでDisplayClearの呼出しは行われています。
                
                this.txtArNo.Text = Condition.ArNo;
                this.txtListFlag.Text = _listFlagName;
                this.txtNonyusakiName.Text = Condition.NonyusakiName;

                //前画面から情報を引き継ぐ
                this.rtbTextBox.LanguageOption = RichTextBoxLanguageOptions.UIFonts;
                this.rtbTextBox.LoadFile(this.FilePath);
                File.Delete(this.FilePath);
                this.rtbTextBox.ClearUndo();
                this.rtbTextBox.MaxLength = this._maxLength;

                // 各種メッセージは不要
                this.MsgInsertConfirm = string.Empty;
                this.MsgInsertEnd = string.Empty;
                this.MsgUpdateConfirm = string.Empty;
                this.MsgUpdateEnd = string.Empty;

                // モード切替
                this.ChangeMode();
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
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカスの設定
                switch (this.EditMode)
                {
                    case SystemBase.EditMode.Insert:
                    case SystemBase.EditMode.Update:
                        this.rtbTextBox.Focus();
                        break;
                    case SystemBase.EditMode.View:
                    default:
                        this.fbrFunction.F12Button.Focus();
                        break;
                }
                // 戻り値をここでリセットする
                this.FilePath = null;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion //初期化

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {

                this.rtbTextBox.Clear();
                this.rtbTextBox.ClearUndo();

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion //画面クリア

        #region イベント

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F1ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/06/13</create>
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
        /// <create>D.Okumura 2019/06/13</create>
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

        #endregion //ファンクションボタンクリック

        #region 貼り付けイベント
        /// --------------------------------------------------
        /// <summary>
        /// 貼り付けイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rtbTextBox_Pasted(object sender, DSWRichTextBoxPasteEventArgs e)
        {
            NotifyPasteMessage(this.Text, e);
        }
        /// --------------------------------------------------
        /// <summary>
        /// 貼り付けメッセージ
        /// </summary>
        /// <param name="controlName">名称</param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void NotifyPasteMessage(string controlName, DSWRichTextBoxPasteEventArgs e)
        {
            switch (e.Status)
            {
                case DSWRichTextBoxPasteEventStatus.ErrorInvalidInputReguration:
                    //ありえない
                    break;
                case DSWRichTextBoxPasteEventStatus.ErrorLengthOver:
                    // もともとの仕様で文字数オーバーはメッセージなどを出していないので踏襲してメッセージなどは出さない
                    break;
                case DSWRichTextBoxPasteEventStatus.ErrorNoData:
                    // 貼り付け可能な画像又はTextがありません。
                    this.ShowMessage("A9999999069", controlName);
                    break;
                case DSWRichTextBoxPasteEventStatus.ErrorNoEnoughMemory:
                    // 貼り付け操作中にMemory不足が発生しました。Applicationを再起動してください。
                    this.ShowMessage("A9999999072", controlName);
                    break;
                case DSWRichTextBoxPasteEventStatus.Error:
                    // 貼り付け操作中に予期せぬエラーが発生しました。再試行してください。
                    this.ShowMessage("A9999999071", controlName);
                    break;
                case DSWRichTextBoxPasteEventStatus.ErrorSizeOver:
                    // 添付するFileSizeが大きすぎます。
                    this.ShowMessage("A9999999074", controlName);
                    break;
                case DSWRichTextBoxPasteEventStatus.Success:
                    this.ClearMessage();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region クローズ処理
        /// --------------------------------------------------
        /// <summary>
        /// クローズ処理
        /// </summary>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/06/24</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnClosing(CancelEventArgs e)
        {
            if (!e.Cancel)
            {
                // リッチテキストメモリ不足対応：クリップボード内に大きなデータ(画像など)があるときは警告する
                var hasImage = rtbTextBox.HasClipboardDataFromRichText();
                if (hasImage)
                {
                    // 更新でのCloseがあるので、クローズについて質問するかどうかで判断する。
                    // 質問ありの場合はYes/No/Cancel動作とし、質問なしの場合はYes/Noのみとする。
                    var result = this.ShowMessage(IsCloseQuestion ? "A9999999073" : "A9999999075");
                    switch (result)
                    {
                        case DialogResult.Yes:
                            try
                            {
                                Clipboard.Clear();
                            }
                            catch { }
                            IsCloseQuestion = false;
                            break;
                        case DialogResult.No:
                            IsCloseQuestion = false;
                            break;
                        case DialogResult.Cancel:
                        default:
                            e.Cancel = true;
                            return;
                    }
                }

            }
            base.OnClosing(e);
        }
        #endregion

        #endregion //イベント

        #region 編集処理
        /// --------------------------------------------------
        /// <summary>
        /// 編集処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            bool result = base.RunEdit();
            // 処理が成功したら画面を閉じる
            if (result)
            {
                this.IsCloseQuestion = false;
                this.Close();
            }
            return result;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 登録処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            var path = Path.GetTempFileName();
            this.rtbTextBox.SaveFile(path, RichTextBoxStreamType.RichText);
            this.FilePath = path;
            return true;
        }
        /// --------------------------------------------------
        /// <summary>
        /// 編集処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            var path = Path.GetTempFileName();
            this.rtbTextBox.SaveFile(path, RichTextBoxStreamType.RichText);
            this.FilePath = path;
            return true;
        }
        #endregion //編集処理

        #region モード切替操作
        /// --------------------------------------------------
        /// <summary>
        /// モード切替操作
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeMode()
        {
            switch (this.EditMode)
            {
                case SystemBase.EditMode.Insert:
                case SystemBase.EditMode.Update:
                    this.rtbTextBox.ReadOnly = false;
                    this.fbrFunction.F01Button.Enabled = true;
                    this.IsCloseQuestion = true;
                    break;
                case SystemBase.EditMode.View:
                    this.rtbTextBox.ReadOnly = true;
                    this.fbrFunction.F01Button.Enabled = false;
                    this.IsCloseQuestion = false;
                    break;
                default:
                    break;
            }
        }
        #endregion

    }
}
