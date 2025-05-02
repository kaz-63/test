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

using WsConnection.WebRefK04;
using SMS.K04.Properties;

namespace SMS.K04.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 取込エラー詳細(Box梱包)
    /// </summary>
    /// <create>Y.Higuchi 2010/08/10</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class HandyDataErrorMeisaiBox : SystemBase.Forms.CustomOrderForm
    {
        #region Fields

        private string _tempID = string.Empty;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="tempID">一時取込ID</param>
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public HandyDataErrorMeisaiBox(UserInfo userInfo, string tempID)
            : base(userInfo, ComDefine.TITLE_K0400030)
        {
            InitializeComponent();
            this._tempID = tempID;
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // シートの初期化
                this.InitializeSheet(this.shtMeisai);
                this.shtMeisai.KeepHighlighted = true;
                this.RunSearch();

                // シートのタイトルを設定
                shtMeisai.ColumnHeaders[0].Caption = Resources.HandyDataErrorMeisaiBox_MarkingTagNo;
                shtMeisai.ColumnHeaders[1].Caption = Resources.HandyDataErrorMeisaiBox_Result;
                shtMeisai.ColumnHeaders[2].Caption = Resources.HandyDataErrorMeisaiBox_ErrotDetail;
                shtMeisai.ColumnHeaders[3].Caption = Resources.HandyDataErrorMeisaiBox_DeliveryDestination;
                shtMeisai.ColumnHeaders[4].Caption = Resources.HandyDataErrorMeisaiBox_ShipARNo;
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
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
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
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // BoxNoクリア
                this.txtBoxNo.Text = string.Empty;
                // グリッドクリア
                this.shtMeisai.Redraw = false;
                this.shtMeisai.DataSource = null;
                this.shtMeisai.MaxRows = 0;
                this.shtMeisai.Redraw = true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 検索処理

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理実行部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                ConnK04 conn = new ConnK04();
                CondK04 cond = new CondK04(this.UserInfo);

                cond.TempID = this._tempID;
                DataSet ds = conn.GetTempworkMeisai(cond);

                if (ComFunc.IsExistsTable(ds, Def_T_TEMPWORK_MEISAI.Name))
                {
                    this.txtBoxNo.Text = ComFunc.GetFld(ds, Def_T_TEMPWORK_MEISAI.Name, 0, Def_T_TEMPWORK_MEISAI.BOX_NO);
                    this.shtMeisai.DataSource = ds.Tables[Def_T_TEMPWORK_MEISAI.Name];
                }

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
        /// <create>Y.Higuchi 2010/08/10</create>
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
