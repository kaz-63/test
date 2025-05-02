using System;
using System.Data;
using System.Windows.Forms;
using DSWUtil;
using WsConnection;
using Commons;
using WsConnection.WebRefK02;
using ActiveReportsHelper;
using SystemBase.Util;

namespace SMS.K02.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// ラベル印刷
    /// </summary>
    /// <create>H.Tsunamura 2010/07/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class LabelInsatsu : SystemBase.Forms.CustomOrderForm
    {
        #region 定数
        // 1ページに印刷する最大数
        private int MAX_NUM = 20;
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
        /// <create>H.Tsunamura 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public LabelInsatsu(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>H.Tsunamura 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // フォームの設定
                this.IsCloseQuestion = true;
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
        /// <create>H.Tsunamura 2010/07/15</create>
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

        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>H.Tsunamura 2010/07/15</create>
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
        /// <create>H.Tsunamura 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                ret = false;
            }
            return ret;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>H.Tsunamura 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                // コントロールの値が入っているかどうかのチェック
                if (string.IsNullOrEmpty(this.txtNum.Text) || UtilConvert.ToInt32(this.txtNum.Text) == 0)
                {
                    // 印刷する枚数を入力して下さい。
                    this.ShowMessage("K0200010001");
                    return false;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                ret = false;
            }
            return ret;
        }

        #endregion

        #region 検索処理

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理制御部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>H.Tsunamura 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearch()
        {
            return base.RunSearch();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理実行部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>H.Tsunamura 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            return true;
        }

        #endregion

        #region イベント

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F1ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            try
            {
                this.fbrFunction.F01Button.Enabled = false;
                this.fbrFunction.F02Button.Enabled = false;

                base.fbrFunction_F01Button_Click(sender, e);

                if (this.RunSearch())
                {
                    printLabel(false);
                }

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.fbrFunction.F01Button.Enabled = true;
                this.fbrFunction.F02Button.Enabled = true;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F2ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F02Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            try
            {
                this.fbrFunction.F01Button.Enabled = false;
                this.fbrFunction.F02Button.Enabled = false;

                base.fbrFunction_F02Button_Click(sender, e);

                if (this.RunSearch())
                {
                    printLabel(true);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.fbrFunction.F01Button.Enabled = true;
                this.fbrFunction.F02Button.Enabled = true;
            }
        }

        #endregion

        /// --------------------------------------------------
        /// <summary>
        /// それぞれのラベルを印刷します。
        /// </summary>
        /// <param name="isPreview">true:プレビュー/false:印刷</param>
        /// <create>H.Tsunamura 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private void printLabel(bool isPreview)
        {
            try
            {
                if (!isPreview)
                {
                    // 印刷してよろしいですか？ダイアログ
                    if (this.ShowMessage("A9999999035") != DialogResult.OK)
                    {
                        return;
                    }
                }


                CondK02 cond = new CondK02(this.UserInfo);
                WsConnection.ConnK02 connK02 = new ConnK02();
                cond.IsPreview = isPreview;
                // 採番フラグの設定
                if (this.rdoBox.Checked)
                {
                    cond.SaibanFlag = SAIBAN_FLAG.BOX_NO_VALUE1;
                }
                else
                {
                    cond.SaibanFlag = SAIBAN_FLAG.PALLET_NO_VALUE1;
                }

                // 取得するNoの個数
                cond.Count = UtilConvert.ToInt32(txtNum.Text) * MAX_NUM;

                DataTable noList = new DataTable();
                string errMsgID;

                var sw = System.Diagnostics.Stopwatch.StartNew();
                bool result = connK02.GetNoList(cond, out noList, out errMsgID);
                System.Diagnostics.Debug.WriteLine(sw.ElapsedMilliseconds);

                if (!result)
                {
                    this.ShowMessage(errMsgID);
                    return;
                }

                object obj = null;
                if (cond.SaibanFlag == SAIBAN_FLAG.BOX_NO_VALUE1)
                {
                    // BOXラベル印刷
                    obj = ReportHelper.GetReport(ComDefine.REPORT_R0100030_CLASS_NAME, noList);
                }
                else
                {
                    // パレットラベル印刷
                    obj = ReportHelper.GetReport(ComDefine.REPORT_R0100040_CLASS_NAME, noList);
                }

                if (isPreview)
                {
                    PreviewPrinterSetting pps = new PreviewPrinterSetting();
                    pps.Landscape = true;
                    pps.PaperKind = System.Drawing.Printing.PaperKind.A4;
                    // プレビュー
                    ReportHelper.Preview(this, obj, pps);

                    return;
                }

                ReportHelper.Print(LocalSetting.GetNormalPrinter(), obj);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return;
            }
        }
        #endregion
    }
}
