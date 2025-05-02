using System;
using System.Data;

using DSWUtil;
using WsConnection;
using Commons;
using WsConnection.WebRefU01;
using SystemBase.Util;
using SMS.U01.Properties;


namespace SMS.U01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 受入情報明細
    /// </summary>
    /// <create>H.Tsunamura 2010/08/20</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ShinchokuToiawaseMeisai : SystemBase.Forms.CustomOrderForm
    {
        #region Fields
        private string _shukkaFlag = string.Empty;
        private string _nonyusakiCD = string.Empty;
        private string _nonyusakiName = string.Empty;
        private string _ship = string.Empty;
        private string _boxNo = string.Empty;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="shukkaFlag"></param>
        /// <param name="nonyusakiCD"></param>
        /// <param name="nonyusakiName"></param>
        /// <param name="ship"></param>
        /// <param name="arNo"></param>
        /// <param name="boxNo"></param>
        /// <create>H.Tsunamura 2010/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public ShinchokuToiawaseMeisai(UserInfo userInfo, string shukkaFlag, string nonyusakiCD, string nonyusakiName, string ship, string boxNo)
            : base(userInfo, ComDefine.TITLE_U0100020)
        {
            this._shukkaFlag = shukkaFlag;
            this._nonyusakiCD = nonyusakiCD;
            this._nonyusakiName = nonyusakiName;
            this._ship = ship;
            this._boxNo = boxNo;
            InitializeComponent();
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>H.Tsunamura 2010/08/20</create>
        /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // シートの初期化
                this.InitializeSheet(this.shtMeisai);
                // シートのタイトルを設定
                shtMeisai.ColumnHeaders[0].Caption = Resources.ShinchokuToiawaseMeisai_TagNo;
                shtMeisai.ColumnHeaders[1].Caption = Resources.ShinchokuToiawaseMeisai_ProductNo;
                shtMeisai.ColumnHeaders[2].Caption = Resources.ShinchokuToiawaseMeisai_Code;
                shtMeisai.ColumnHeaders[3].Caption = Resources.ShinchokuToiawaseMeisai_DrawingAdditionalNo;
                shtMeisai.ColumnHeaders[4].Caption = Resources.ShinchokuToiawaseMeisai_Area;
                shtMeisai.ColumnHeaders[5].Caption = Resources.ShinchokuToiawaseMeisai_Floor;
                shtMeisai.ColumnHeaders[6].Caption = Resources.ShinchokuToiawaseMeisai_Model;
                shtMeisai.ColumnHeaders[7].Caption = Resources.ShinchokuToiawaseMeisai_MNo;
                shtMeisai.ColumnHeaders[8].Caption = Resources.ShinchokuToiawaseMeisai_JpName;
                shtMeisai.ColumnHeaders[9].Caption = Resources.ShinchokuToiawaseMeisai_Name;
                shtMeisai.ColumnHeaders[10].Caption = Resources.ShinchokuToiawaseMeisai_DrawingNoFormat;
                shtMeisai.ColumnHeaders[11].Caption = Resources.ShinchokuToiawaseMeisai_SectioningNo;
                shtMeisai.ColumnHeaders[12].Caption = Resources.ShinchokuToiawaseMeisai_Quantity;
                shtMeisai.ColumnHeaders[13].Caption = Resources.ShinchokuToiawaseMeisai_Memo;
                shtMeisai.ColumnHeaders[14].Caption = Resources.ShinchokuToiawaseMeisai_CustomsStatus;
                shtMeisai.ColumnHeaders[15].Caption = Resources.ShinchokuToiawaseMeisai_STNo;
                shtMeisai.ColumnHeaders[16].Caption = Resources.ShinchokuToiawaseMeisai_ARNo;
                // 初期値の設定
                this.txtBoxNo.Text = this._boxNo;
                this.txtNonyusakiName.Text = this._nonyusakiName;
                this.txtShip.Text = this._ship;
                this.RunSearch();
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
        /// <create>H.Tsunamura 2010/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // フォーカスの設定
                this.shtMeisai.Focus();
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
        /// <create>H.Tsunamura 2010/08/20</create>
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
        /// <create>H.Tsunamura 2010/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
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
        /// <create>H.Tsunamura 2010/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
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
        /// <create>H.Tsunamura 2010/08/20</create>
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
        /// <create>H.Tsunamura 2010/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                CondU01 cond = new CondU01(this.UserInfo, ComDefine.BUTTON_TEXT_KONPOZUMI, ComDefine.BUTTON_TEXT_SHUKKA, ComDefine.BUTTON_TEXT_UKEIRE, ComDefine.BUTTON_TEXT_DETAIL);
                ConnU01 conn = new ConnU01();

                cond.ShukkaFlag = this._shukkaFlag;
                cond.NonyusakiCD = this._nonyusakiCD;
                cond.UkeireNo = this._boxNo;

                DataSet ds = conn.GetBoxMeisai(cond);

                if (!ComFunc.IsExistsData(ds, Def_T_SHUKKA_MEISAI.Name))
                {
                    // 該当の明細は存在しません。
                    this.ShowMessage("A9999999022");
                    return false;
                }

                this.txtNonyusakiName.Text = ComFunc.GetFld(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_M_NONYUSAKI.NONYUSAKI_NAME);
                this.txtShip.Text = ComFunc.GetFld(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_M_NONYUSAKI.SHIP);
                this.shtMeisai.DataSource = ds.Tables[Def_T_SHUKKA_MEISAI.Name];

                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion


        #region イベント

        #region ファンクションボタンクリック
        /// --------------------------------------------------
        /// <summary>
        /// F12ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/08/20</create>
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
    }
}
