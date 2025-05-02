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

namespace SMS.T01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 手配明細分割
    /// </summary>
    /// <create>S.Furugo 2018/11/16</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class TehaiMeisaiBunkatsu : SystemBase.Forms.CustomOrderForm
    {
        #region 数量クラス
        /// --------------------------------------------------
        /// <summary>
        /// 数量クラス
        /// </summary>
        /// <create>D.Naito 2018/11/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public class Qty
        {
            /// <summary>
            /// 手配数
            /// </summary>
            public decimal Tehai { get; set; }
            /// <summary>
            /// 発注数
            /// </summary>
            public decimal Hacchu { get; set; }
            /// <summary>
            /// 出荷数
            /// </summary>
            public decimal Shukka { get; set; }
            /// <summary>
            /// 入荷数
            /// </summary>
            public decimal Arrival { get; set; }
            /// <summary>
            /// 組立数
            /// </summary>
            public decimal Assy { get; set; }


            /// --------------------------------------------------
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <create>D.Naito 2018/12/03</create>
            /// <update></update>
            /// --------------------------------------------------
            public Qty()
            {
                this.Tehai = 0m;
                this.Hacchu = 0m;
                this.Shukka = 0m;
                this.Arrival = 0m;
                this.Assy = 0m;
            }

            /// --------------------------------------------------
            /// <summary>
            /// コピー
            /// </summary>
            /// <create>D.Naito 2018/12/03</create>
            /// <update></update>
            /// --------------------------------------------------
            public Qty Copy()
            {
                Qty val = this.Clone();
                val.Tehai = this.Tehai;
                val.Hacchu = this.Hacchu;
                val.Shukka = this.Shukka;
                val.Arrival = this.Arrival;
                val.Assy = this.Assy;
                return val;
            }

            /// --------------------------------------------------
            /// <summary>
            /// クローン
            /// </summary>
            /// <create>D.Naito 2018/12/03</create>
            /// <update></update>
            /// --------------------------------------------------
            public Qty Clone()
            {
                return (Qty)this.MemberwiseClone();
            }
        }
        #endregion

        /// <summary>
        /// 手配区分
        /// </summary>
        private string _TehaiFlag = TEHAI_FLAG.DEFAULT_VALUE1;
        /// <summary>
        /// 総数
        /// </summary>
        private Qty Sum = new Qty();
        /// <summary>
        /// 分割元数
        /// </summary>
        public Qty Src = new Qty();
        /// <summary>
        /// 分割先数
        /// </summary>
        public Qty Dst = new Qty();

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ(デザイナ用)
        /// </summary>
        /// <create>D.Naito 2018/11/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public TehaiMeisaiBunkatsu()
            : this(null, null, null)
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <create>D.Naito 2018/11/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public TehaiMeisaiBunkatsu(UserInfo userInfo)
            : this(userInfo, null, null)
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="qty">数量</param>
        /// <create>D.Naito 2018/11/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public TehaiMeisaiBunkatsu(UserInfo userInfo, string TehaiFlg, Qty src)
            : base(userInfo)
        {
            InitializeComponent();
            this.Title = ComDefine.TITLE_T0100011;

            _TehaiFlag = TehaiFlg;
            this.Sum = src.Copy();
            this.Src = src.Copy();
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>S.Furugo 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // コントロールの初期化
                this.txtTehaiQtySum.Text = this.Sum.Tehai.ToString();
                this.txtHacchuQtySum.Text = this.Sum.Hacchu.ToString();
                this.txtShukkaQtySum.Text = this.Sum.Shukka.ToString();

                this.txtTehaiQtySource.Text = this.Src.Tehai.ToString();
                this.txtHacchuQtySource.Text = this.Src.Hacchu.ToString();
                this.txtShukkaQtySource.Text = this.Src.Shukka.ToString();

                this.txtTehaiQtySource_Leave(null, new EventArgs());
                this.txtHacchuQtySource_Leave(null, new EventArgs());
                this.txtShukkaQtySource_Leave(null, new EventArgs());

                txtHacchuQtySource.Enabled = _TehaiFlag.Equals(TEHAI_FLAG.ORDERED_VALUE1);
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
        /// <createS.Furugo 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカス
                this.txtTehaiQtySum.Focus();
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
        /// 手配数が変更の時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Naito 2018/11/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private void txtTehaiQtySource_Leave(object sender, EventArgs e)
        {
            this.Src.Tehai = DSWUtil.UtilConvert.ToDecimal(this.txtTehaiQtySource.Text, 0m);
            this.Dst.Tehai = this.Sum.Tehai - this.Src.Tehai;

            this.txtTehaiQtyDestination.Text = this.Dst.Tehai.ToString();

        }
        /// --------------------------------------------------
        /// <summary>
        /// 発注数が変更の時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Naito 2018/11/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private void txtHacchuQtySource_Leave(object sender, EventArgs e)
        {
            this.Src.Hacchu = DSWUtil.UtilConvert.ToDecimal(this.txtHacchuQtySource.Text, 0m);
            this.Dst.Hacchu = this.Sum.Hacchu - this.Src.Hacchu;

            this.txtHacchuQtyDestination.Text = this.Dst.Hacchu.ToString();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 出荷数が変更の時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Naito 2018/11/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private void txtShukkaQtySource_Leave(object sender, EventArgs e)
        {
            this.Src.Shukka = DSWUtil.UtilConvert.ToDecimal(this.txtShukkaQtySource.Text, 0m);
            this.Dst.Shukka = this.Sum.Shukka - this.Src.Shukka;

            this.txtShukkaQtyDestination.Text = this.Dst.Shukka.ToString();
        }

        #endregion

        #region ファンクション
        /// --------------------------------------------------
        /// <summary>
        /// F01押下イベント(決定ボタン)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Naito 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            try
            {
                // 再算出
                this.txtTehaiQtySource_Leave(txtTehaiQtySource, new EventArgs());
                this.txtHacchuQtySource_Leave(txtHacchuQtySource, new EventArgs());
                this.txtShukkaQtySource_Leave(txtShukkaQtySource, new EventArgs());

                #region ---入力チェック
                // 手配数入力チェック
                if (this.Src.Tehai <= 0m || this.Dst.Tehai <= 0m)
                {
                    if (this.Src.Tehai <= 0m)
                        // {0:手配数}を0より大きい値を入力して下さい。
                        this.ShowMessage("T0100011003", lblTehaiQtySum.Text, "0");
                    else if (this.Dst.Tehai <= 0m)
                        // {0:手配数}を{1:手配数合計}より小さい値を入力して下さい。
                        this.ShowMessage("T0100011002", lblTehaiQtySum.Text, txtTehaiQtySum.Text);
                    else
                        // {0:手配数}の入力が正しくありません。
                        this.ShowMessage("T0100011001", lblTehaiQtySum.Text);
                    this.txtTehaiQtySource.Focus();
                    return;
                }

                // 発注数
                if (this.txtHacchuQtySource.Enabled && (this.Src.Hacchu < 0m || this.Dst.Hacchu < 0m))
                {
                    if (this.Dst.Hacchu < 0m)
                        // {0:発注数}を{1:発注数合計}より小さい値を入力して下さい。
                        this.ShowMessage("T0100011002", lblHacchuQtySum.Text, txtHacchuQtySum.Text);
                    else
                        // {0:発注数}の入力が正しくありません。
                        this.ShowMessage("T0100011001", lblHacchuQtySum.Text);
                    this.txtHacchuQtySource.Focus();
                    return;
                }

                // 出荷数
                if (this.Src.Shukka < 0m || this.Dst.Shukka < 0m)
                {
                    if (this.Dst.Shukka < 0m)
                        // {0:出荷数}を{1:発注数合計}より小さい値を入力して下さい。
                        this.ShowMessage("T0100011002", lblShukkaQtySum.Text, txtShukkaQtySum.Text);
                    else
                        // {0:出荷数}の入力が正しくありません。
                        this.ShowMessage("T0100011001", lblShukkaQtySum.Text);
                    this.txtShukkaQtySource.Focus();
                    return;
                }
                #endregion

                #region --- 数量算出
                // 入荷数
                if (this.Src.Hacchu < this.Src.Arrival)
                {
                    this.Dst.Arrival = this.Src.Arrival - this.Src.Hacchu;
                    this.Src.Arrival = this.Src.Arrival;
                }
                else
                {
                    this.Dst.Arrival = 0m;
                }

                // 組立数
                if (this.Src.Tehai < this.Src.Assy)
                {
                    this.Dst.Assy = this.Src.Assy - this.Src.Tehai;
                    this.Src.Assy = this.Src.Tehai;
                }
                else
                {
                    this.Dst.Assy = 0m;
                }
                #endregion

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F12押下イベント(Closeボタン)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Naito 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F12Button_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
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
