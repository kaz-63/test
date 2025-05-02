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

using WsConnection.WebRefS02;
using SMS.S02.Properties;

namespace SMS.S02.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 出荷情報明細
    /// </summary>
    /// <create>Y.Higuchi 2010/07/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ShukkaJohoMeisai : SystemBase.Forms.CustomOrderForm
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
        /// <param name="userInfo">ユーザー情報</param>
        /// <param name="shukkaFlag">出荷区分</param>
        /// <param name="nonyusakiCD">納入先コード</param>
        /// <param name="nonyusakiName">納入先名称</param>
        /// <param name="ship">出荷便</param>
        /// <param name="arNo">AR No.</param>
        /// <param name="boxNo">BoxNo.</param>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public ShukkaJohoMeisai(UserInfo userInfo, string shukkaFlag, string nonyusakiCD, string nonyusakiName, string ship,string boxNo)
            : base(userInfo, ComDefine.TITLE_S0200020)
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
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update>J.Chen 2024/11/20 通関確認状態追加</update>
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
                shtMeisai.ColumnHeaders[0].Caption = Resources.ShukkaJohoMeisai_TagNo;
                shtMeisai.ColumnHeaders[1].Caption = Resources.ShukkaJohoMeisai_ProductNo;
                shtMeisai.ColumnHeaders[2].Caption = Resources.ShukkaJohoMeisai_Code;
                shtMeisai.ColumnHeaders[3].Caption = Resources.ShukkaJohoMeisai_DrawingAdditionalNo;
                shtMeisai.ColumnHeaders[4].Caption = Resources.ShukkaJohoMeisai_Area;
                shtMeisai.ColumnHeaders[5].Caption = Resources.ShukkaJohoMeisai_Floor;
                shtMeisai.ColumnHeaders[6].Caption = Resources.ShukkaJohoMeisai_Model;
                shtMeisai.ColumnHeaders[7].Caption = Resources.ShukkaJohoMeisai_MNo;
                shtMeisai.ColumnHeaders[8].Caption = Resources.ShukkaJohoMeisai_JpName;
                shtMeisai.ColumnHeaders[9].Caption = Resources.ShukkaJohoMeisai_Name;
                shtMeisai.ColumnHeaders[10].Caption = Resources.ShukkaJohoMeisai_DrawingNoFormat;
                shtMeisai.ColumnHeaders[11].Caption = Resources.ShukkaJohoMeisai_SectioningNo;
                shtMeisai.ColumnHeaders[12].Caption = Resources.ShukkaJohoMeisai_Quantity;
                shtMeisai.ColumnHeaders[13].Caption = Resources.ShukkaJohoMeisai_Memo;
                shtMeisai.ColumnHeaders[14].Caption = Resources.ShukkaJohoMeisai_CustomsStatus;
                shtMeisai.ColumnHeaders[15].Caption = Resources.ShukkaJohoMeisai_STNo;
                shtMeisai.ColumnHeaders[16].Caption = Resources.ShukkaJohoMeisai_ARNo;
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
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカスの設定
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
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                this.txtBoxNo.Text = string.Empty;
                this.txtNonyusakiName.Text = string.Empty;
                this.txtShip.Text = string.Empty;
                // グリッドクリア
                this.shtMeisai.Redraw = false;
                this.shtMeisai.DataSource = null;
                this.shtMeisai.MaxRows = 0;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.shtMeisai.Redraw = true;
            }
        }

        #endregion

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>Y.Higuchi 2010/07/27</create>
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
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                // ----- 検索用入力チェック -----
                if (string.IsNullOrEmpty(this._shukkaFlag))
                {
                    // 出荷区分が不正です。
                    this.ShowMessage("S0200020001");
                    return false;
                }
                if (string.IsNullOrEmpty(this._nonyusakiCD))
                {
                    // 納入先の管理No.が不正です。
                    this.ShowMessage("S0200020002");
                    return false;
                }
                if (string.IsNullOrEmpty(this._boxNo))
                {
                    // BoxNo.が不正です。
                    this.ShowMessage("S0200020003");
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
        /// <create>Y.Higuchi 2010/07/27</create>
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
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                CondS02 cond = new CondS02(this.UserInfo, ComDefine.BUTTON_TEXT_KONPOZUMI, ComDefine.BUTTON_TEXT_SHUKKA, ComDefine.BUTTON_TEXT_DETAIL);
                ConnS02 conn = new ConnS02();

                cond.ShukkaFlag = this._shukkaFlag;
                cond.NonyusakiCD = this._nonyusakiCD;
                cond.BoxNo = this._boxNo;

                DataSet ds = conn.GetBoxMeisai(cond);

                if (!ComFunc.IsExistsData(ds, Def_T_SHUKKA_MEISAI.Name))
                {
                    // 該当の明細は存在しません。
                    this.ShowMessage("A9999999022");
                    return false;
                }

                this.txtNonyusakiName.Text = ComFunc.GetFld(ds,Def_T_SHUKKA_MEISAI.Name,0,Def_M_NONYUSAKI.NONYUSAKI_NAME);
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

        #region 編集内容実行

        #region 制御メソッド

        /// --------------------------------------------------
        /// <summary>
        /// 編集内容実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Higuchi 2010/07/27</create>
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

        #region 登録処理

        /// --------------------------------------------------
        /// <summary>
        /// 登録処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                return true;
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
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region 削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditDelete()
        {
            try
            {
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
        /// F12ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/27</create>
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
