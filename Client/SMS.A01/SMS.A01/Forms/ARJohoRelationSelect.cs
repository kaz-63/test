using System;
using System.Data;
using System.Windows.Forms;
using Commons;
using DSWControl.DSWFunctionButton;
using SystemBase.Util;
using WsConnection.WebRefA01;
using SMS.A01.Properties;

namespace SMS.A01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// AR情報関連付け選択
    /// </summary>
    /// <create>D.Okumura 2020/01/10</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ARJohoRelationSelect : SystemBase.Forms.CustomOrderForm
    {

        public string ListFlagSelected { get; private set; }
        protected string MotoArNo { get; set; }

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="motoArNo">元ARNo</param>
        /// <param name="datas">M_COMMONのLIST_FLAG_KEKKA</param>
        /// <create>D.Okumura 2020/01/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public ARJohoRelationSelect(UserInfo userInfo, string motoArNo, DataRow[] datas)
            : base(userInfo, ComDefine.TITLE_A0100031)
        {
            InitializeComponent();
            this.MotoArNo = motoArNo;
            this.SetupButtons(datas);
        }

        #endregion

        #region 初期化
        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>D.Okumura 2020/01/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

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
        /// ボタンをセットアップする
        /// </summary>
        /// <param name="datas">M_COMMONのLIST_FLAG_KEKKA</param>
        /// <create>D.Okumura 2020/01/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetupButtons(DataRow[] datas)
        {
            foreach (var row in datas)
            {
                var value1 = ComFunc.GetFld(row, Def_M_COMMON.VALUE1);
                var button = new DSWFunctionButton();
                button.Name = "btnNewAr" + value1;
                button.Text = string.Format(Resources.ARJohoRelationSelect_ButtonText, value1);
                button.Margin = new Padding(30, 10, 30, 10);
                button.Tag = value1; //クリックイベントで使用する
                button.Click += new EventHandler(button_Click);
                button.FlatStyle = FlatStyle.System;
                flpButtons.Controls.Add(button);
            }

        }

        /// --------------------------------------------------
        /// <summary>
        /// ボタンクリックイベント
        /// </summary>
        /// <param name="sender">発生元(ボタン、Tagに対象のリストフラグを保持すること)</param>
        /// <param name="e">イベント</param>
        /// <create>D.Okumura 2020/01/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private void button_Click(object sender, EventArgs e)
        {
            var button = sender as DSWFunctionButton;
            if (button == null || (button.Tag as string) == null)
                return;
            this.ListFlagSelected = button.Tag as string;
            this.DialogResult = DialogResult.OK; //画面を閉じる
        }
        #endregion

        #region イベント

        #region 閉じるボタン

        /// --------------------------------------------------
        /// <summary>
        ///閉じるボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2020/01/10</create>
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
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #endregion

    }
}
