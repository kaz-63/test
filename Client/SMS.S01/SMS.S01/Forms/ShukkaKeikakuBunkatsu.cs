using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Commons;
using SystemBase.Util;
using DSWUtil;

namespace SMS.S01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 明細分割
    /// </summary>
    /// <create>T.Nakata 2018/11/16</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ShukkaKeikakuBunkatsu : SystemBase.Forms.CustomOrderForm
    {
        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 手配明細 数量
        /// </summary>
        /// <create>T.Nakata 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _num = string.Empty;

        #endregion

        #region Properties

        /// --------------------------------------------------
        /// <summary>
        /// 分割元手配数量
        /// </summary>
        /// <create>T.Nakata 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public string NumSource
        {
            get { return txtNumSource.Text; }
        }
        /// --------------------------------------------------
        /// <summary>
        /// 分割先手配数量
        /// </summary>
        /// <create>T.Nakata 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public string NumDestination
        {
            get { return txtNumDestination.Text; }
        }
        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ(デザイナ用)
        /// </summary>
        /// <create>T.Nakata 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public ShukkaKeikakuBunkatsu()
            : this(null, null)
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <create>T.Nakata 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public ShukkaKeikakuBunkatsu(UserInfo userInfo)
            : this(userInfo, null)
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="ship">出荷便</param>
        /// <create>T.Nakata 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public ShukkaKeikakuBunkatsu(UserInfo userInfo, string Num)
            : base(userInfo)
        {
            InitializeComponent();
            this.Title = ComDefine.TITLE_S0100022;
            this._num = Num;
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>T.Nakata 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                if (string.IsNullOrEmpty(this._num)) this.Close();

                this.IsCloseQuestion = false;

                // コントロールの初期化
                this.txtNumSum.Text = this._num;
                this.txtNumSource.Text = string.Empty;
                this.txtNumDestination.Text = string.Empty;

                this.txtNumSum.Enabled = false;
                this.txtNumSource.Enabled = true;
                this.txtNumDestination.Enabled = false;
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
        /// <create>T.Nakata 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカス
                this.txtNumSource.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion
        
        #region イベント

        #region 分割元テキストボックスが変更
        /// --------------------------------------------------
        /// <summary>
        /// 数量が変更の時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private void txtNumSource_KeyUp(object sender, KeyEventArgs e)
        {
            textchenge();
        }
        /// --------------------------------------------------
        /// <summary>
        /// 数量が変更の時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private void txtNumSource_ValueChanged(object sender, EventArgs e)
        {
            textchenge();
        }
        /// --------------------------------------------------
        /// <summary>
        /// 数量が変更の時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private void txtNumSource_Leave(object sender, EventArgs e)
        {
            textchenge();
        }
        /// --------------------------------------------------
        /// <summary>
        /// 数量が変更の時
        /// </summary>
        /// <create>T.Nakata 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private void textchenge()
        {
            try
            {
                if (!string.IsNullOrEmpty(this.txtNumSource.Text))
                {
                    this.ClearMessage();

                    int splitNumSrc = 0;
                    int NumSum = int.Parse(this._num);

                    splitNumSrc = int.Parse(this.txtNumSource.Text);
                    if (splitNumSrc > (NumSum - 1))
                    {
                        this.txtNumSource.Text = (NumSum - 1).ToString();
                        splitNumSrc = (NumSum - 1);
                        this.txtNumSource.SelectionStart = this.txtNumSource.Text.Length;
                    }
                    this.txtNumDestination.Text = (NumSum - splitNumSrc).ToString();
                }
                else
                {
                    this.txtNumDestination.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }
        #endregion

        #region 決定ボタンクリック
        /// --------------------------------------------------
        /// <summary>
        /// 決定ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDecision_Click(object sender, EventArgs e)
        {
            fbrFunction_F01Button_Click(sender, e);
        }
        /// --------------------------------------------------
        /// <summary>
        /// F1処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            try
            {
                int NumSum = string.IsNullOrEmpty(txtNumSum.Text) ? 0 : int.Parse(txtNumSum.Text);
                int NumSrc = string.IsNullOrEmpty(txtNumSource.Text) ? 0 : int.Parse(txtNumSource.Text);
                int NumDist = string.IsNullOrEmpty(txtNumDestination.Text) ? 0 : int.Parse(txtNumDestination.Text);

                if ((NumSrc <= 0)
                    || (NumDist <= 0)
                    || (NumSum < NumSrc))
                {
                    // 数量の入力が正しくありません。
                    this.ShowMessage("A9999999067");
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }
        #endregion

        #region Closeボタンクリック
        /// --------------------------------------------------
        /// <summary>
        /// Closeボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }
        #endregion

        #endregion

    }
}
