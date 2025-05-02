using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Commons;
using SystemBase.Util;
using WsConnection;
using SMS.Z99.Properties;

namespace SMS.Z99.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 物件名マスタ作成ツール
    /// </summary>
    /// <create>T.Sakiori 2012/04/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class BukkenCreate : SystemBase.Forms.CustomOrderForm
    {
        #region Fields

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>T.Sakiori 2012/04/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public BukkenCreate()
            : base(new UserInfo(), string.Empty)
        {
            InitializeComponent();
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示されるたびにコントロールの初期化するメソッド
        /// </summary>
        /// <create>T.Sakiori 2012/04/09</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカスの設定
                this.btnStart.Focus();
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
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            try
            {
                var sb = new StringBuilder();
                sb.ApdL(Resources.BukkenCreate_PropertyMasterCreate1);
                sb.ApdL(Resources.BukkenCreate_PropertyMasterCreate2);
                sb.ApdL(Resources.BukkenCreate_PropertyMasterCreate3);
                if (CustomMsgBoxEx.ShowQuestion(this, sb.ToString(), MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return false;
                }

                this.btnStart.Enabled = false;
                this.btnExit.Enabled = false;

                var conn = new ConnZ99();
                string errMsgID;
                string[] args;
                if (!conn.InsBukken(out errMsgID, out args))
                {
                    if (!string.IsNullOrEmpty(errMsgID))
                    {
                        this.ShowMessage(errMsgID, args);
                    }
                    return false;
                }

                this.lblSubTitle.Text = Resources.BukkenCreate_SuccessfulCompletion;
                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
            finally
            {
                this.btnStart.Enabled = true;
                this.btnExit.Enabled = true;
            }
        }

        #endregion

        #endregion

        #region イベント

        /// --------------------------------------------------
        /// <summary>
        /// 開始ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2012/04/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                this.RunEdit();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 終了ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2012/04/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnExit_Click(object sender, EventArgs e)
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

        #region 入力登録、Excel登録用のデータテーブル

        /// --------------------------------------------------
        /// <summary>
        /// 入力登録、Excel登録用のデータテーブル
        /// </summary>
        /// <returns>データテーブル</returns>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSchemeShukkaMeisai()
        {
            DataTable dt = new DataTable(Def_T_SHUKKA_MEISAI.Name);
            dt.Columns.Add(Def_M_NONYUSAKI.NONYUSAKI_CD, typeof(string));
            dt.Columns.Add(Def_M_NONYUSAKI.NONYUSAKI_NAME, typeof(string));
            dt.Columns.Add(Def_M_NONYUSAKI.SHIP, typeof(string));
            dt.Columns.Add(Def_M_NONYUSAKI.KANRI_FLAG, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.TAG_NO, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.AR_NO, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.SEIBAN, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.CODE, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.ZUMEN_OIBAN, typeof(decimal));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.AREA, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.FLOOR, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.KISHU, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.ST_NO, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.HINMEI_JP, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.HINMEI, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.KUWARI_NO, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.NUM, typeof(decimal));

            return dt;
        }

        #endregion
    }
}
