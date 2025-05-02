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
using GrapeCity.Win.ElTabelle;
using ElTabelleHelper;

namespace SystemBase.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 一括コピー設定ダイアログ
    /// </summary>
    /// <create>H.Tajimi 2018/09/10</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class SheetCellBulkCopyDialog : SystemBase.Forms.CustomOrderForm
    {
        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 設定済の内容を上書きするかどうか
        /// </summary>
        /// <create>H.Tajimi 2018/09/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isOverwrite = false;
        /// --------------------------------------------------
        /// <summary>
        /// 末尾まで上書きするかどうか
        /// </summary>
        /// <create>H.Tajimi 2018/09/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isToEnd = false;
        /// --------------------------------------------------
        /// <summary>
        /// コピー行数
        /// </summary>
        /// <create>H.Tajimi 2018/09/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private int _count = 0;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="caption">列タイトル</param>
        /// <param name="text">値</param>
        /// <param name="isOverwrite">設定済の内容を上書きするかどうか</param>
        /// <param name="isToEnd">末尾まで一括コピーを行うかどうか</param>
        /// <param name="count">末尾まで一括コピーを行わない場合に一括コピー先対象行数</param>
        /// <remarks>
        /// isToEndがtrueの場合は、countの内容は無視されます
        /// </remarks>
        /// <create>H.Tajimi 2018/09/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public SheetCellBulkCopyDialog(UserInfo userInfo, string caption, string text, bool isOverwrite, bool isToEnd, int count)
            : base(userInfo)
        {
            InitializeComponent();
            //TODO:画面タイトルは既存の他画面のものを使用しているので、多言語対応が落ち着いたらComDefineに定義が必要
            // 画面タイトル設定
            this.Title = ComDefine.TITLE_P0200060;

            this.lblReference.Text = caption;
            this.txtReference.Text = text;

            this.IsOverwrite = isOverwrite;
            this.IsToEnd = isToEnd;
            if (this.IsToEnd)
            {
                this.Count = 0;
            }
            else
            {
                this.Count = count;
            }
        }

        #endregion

        #region プロパティ

        /// --------------------------------------------------
        /// <summary>
        /// 設定済の内容を上書きするかどうか
        /// </summary>
        /// <create>H.Tajimi 2018/09/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool IsOverwrite 
        {
            get { return this._isOverwrite; }
            private set { this._isOverwrite = value; }
        }
        /// --------------------------------------------------
        /// <summary>
        /// 末尾まで上書きするかどうか
        /// </summary>
        /// <create>H.Tajimi 2018/09/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool IsToEnd
        {
            get { return this._isToEnd; }
            private set { this._isToEnd = value; }
        }
        /// --------------------------------------------------
        /// <summary>
        /// コピー行数
        /// </summary>
        /// <create>H.Tajimi 2018/09/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public int Count
        {
            get { return this._count; }
            private set { this._count = value; }
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>H.Tajimi 2018/09/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();
            try
            {
                // 何があっても問い合わせを行わずに閉じる
                this.IsChangedCloseQuestion = false;
                this.IsCloseQuestion = false;

                // 照会固定
                this.EditMode = EditMode.View;

                // テキストの初期化
                this.InitializeText();

                this.chkOverwrite.Checked = this._isOverwrite;
                this.chkToEnd.Checked = this._isToEnd;
                this.EnableControlChanged(this._isToEnd);
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
        /// <create>H.Tajimi 2018/09/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // フォーカス設定
                this.chkOverwrite.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #region Text初期化

        /// --------------------------------------------------
        /// <summary>
        /// Text初期化
        /// </summary>
        /// <create>H.Tajimi 2018/09/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private void InitializeText()
        {
            try
            {
                this.txtReference.ReadOnly = true;

                this.txtCount.AllowEmpty = true;
                this.txtCount.AllowNegative = false;
                this.txtCount.IntLength = 9;
                this.txtCount.MaxLength = 9;
                this.txtCount.IsUseMinValue = true;
                this.txtCount.IsUseMaxValue = true;
                this.txtCount.MinValue = 1;
                this.txtCount.MaxValue = 999999999;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #endregion

        #region コントロール制御

        /// --------------------------------------------------
        /// <summary>
        /// コントロール制御
        /// </summary>
        /// <param name="isToEnd"></param>
        /// <create>H.Tajimi 2018/09/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private void EnableControlChanged(bool isToEnd)
        {
            if (isToEnd)
            {
                this.txtCount.Clear();
                this.lblCount.Enabled = false;
            }
            else
            {
                this.txtCount.Value = 1;
                this.lblCount.Enabled = true;
            }
        }

        #endregion

        #region イベント

        #region Button

        #region 選択

        /// --------------------------------------------------
        /// <summary>
        /// 選択ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/09/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                this.IsOverwrite = this.chkOverwrite.Checked;
                this.IsToEnd = this.chkToEnd.Checked;
                if (!this._isToEnd)
                {
                    this.Count = UtilConvert.ToInt32(this.txtCount.Value);
                }
                else
                {
                    this.Count = 0;
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }
        
        #endregion

        #region 閉じる

        /// --------------------------------------------------
        /// <summary>
        /// 閉じるボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/09/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }
        
        #endregion

        #endregion

        #region CheckBox

        /// --------------------------------------------------
        /// <summary>
        /// 末尾までコピーチェックボックスの状態変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/09/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private void chkToEnd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.EnableControlChanged(this.chkToEnd.Checked);
                return;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #endregion
    }
}
