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

using WsConnection.WebRefI02;
using SMS.I02.Properties;

namespace SMS.I02.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 取込エラー詳細(集荷)
    /// </summary>
    /// <create>T.Wakamatsu 2013/08/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class HandyDataErrorMeisai : SystemBase.Forms.CustomOrderForm
    {
        #region Fields

        private string _tempID = string.Empty;
        private string _workUserID = string.Empty;
        private string _torikomiFlag = string.Empty;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="tempID">一時取込ID</param>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public HandyDataErrorMeisai(UserInfo userInfo, DataRow dr, string title)
            : base(userInfo, title)
        {
            InitializeComponent();

            _tempID = ComFunc.GetFld(dr, Def_T_BUHIN_TEMPWORK.TEMP_ID);
            _workUserID = ComFunc.GetFld(dr, Def_T_BUHIN_TEMPWORK.WORK_USER_ID);
            _torikomiFlag = ComFunc.GetFld(dr, Def_T_BUHIN_TEMPWORK.TORIKOMI_FLAG);
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // デフォルトはDelete
                this.EditMode = SystemBase.EditMode.Delete;

                // シートの初期化
                this.InitializeSheet(this.shtMeisai);
                this.shtMeisai.KeepHighlighted = true;
                if (_torikomiFlag == ZAIKO_TORIKOMI_FLAG.KANRYO_VALUE1)
                    this.shtMeisai.Columns[1].Hidden = true;
                this.RunSearch();

                // シートのタイトルを設定
                shtMeisai.ColumnHeaders[0].Caption = Resources.HandyDataErrorMeisai_Work;
                shtMeisai.ColumnHeaders[1].Caption = Resources.HandyDataErrorMeisai_Worker;
                shtMeisai.ColumnHeaders[2].Caption = Resources.HandyDataErrorMeisai_Location;
                shtMeisai.ColumnHeaders[3].Caption = Resources.HandyDataErrorMeisai_No;
                shtMeisai.ColumnHeaders[4].Caption = Resources.HandyDataErrorMeisai_Result;
                shtMeisai.ColumnHeaders[5].Caption = Resources.HandyDataErrorMeisai_ErrorDetail;

                // フォームの設定
                this.IsCloseQuestion = true;
                this.IsRunEditAfterClear = false;
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
        /// <create>T.Wakamatsu 2013/08/22</create>
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
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
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
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                ConnI02 conn = new ConnI02();
                CondI02 cond = new CondI02(this.UserInfo);

                cond.TempID = this._tempID;
                cond.TorikomiFlag = this._torikomiFlag;
                DataSet ds = conn.GetTempworkMeisai(cond);

                if (ComFunc.IsExistsTable(ds, Def_T_BUHIN_TEMPWORK_MEISAI.Name))
                {
                    this.shtMeisai.DataSource = ds.Tables[Def_T_BUHIN_TEMPWORK_MEISAI.Name];
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

        #region 編集内容実行

        #region 制御メソッド

        /// --------------------------------------------------
        /// <summary>
        /// 編集内容実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            bool ret = base.RunEdit();
            try
            {
                if (ret)
                {
                    // 画面を閉じる
                    this.DialogResult = DialogResult.OK;
                    // メッセージが出ないようにする。
                    this.IsCloseQuestion = false;
                    this.Close();
                }
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
        /// <create>T.Wakamatsu 2013/09/20</create>
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
        /// <create>T.Wakamatsu 2013/09/20</create>
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
        /// <create>T.Wakamatsu 2013/09/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditDelete()
        {
            try
            {
                bool ret = false;

                ConnI02 conn = new ConnI02();
                CondI02 cond = new CondI02(this.UserInfo);
                cond.TempID = this._tempID;
                cond.TorikomiFlag = this._torikomiFlag;
                cond.WorkUserID = cond.LoginInfo.UserID;

                this.shtMeisai.UpdateData();
                DataTable dtDel = (this.shtMeisai.DataSource as DataTable).Copy();

                for (int i = 0; i < dtDel.Rows.Count; i++)
                {
                    DataRow dr = dtDel.Rows[i];

                    if (dr.RowState == DataRowState.Deleted)
                        dr.RejectChanges();
                    else
                        dr.Delete();
                }
                dtDel.AcceptChanges();

                if (dtDel.Rows.Count == 0)
                {
                    // 更新対象となる行がありません。
                    this.ShowMessage("I0200010034");
                    return false;
                }

                string errMsgID;
                string[] args;
                ret = conn.DelTempWorkMeisai(cond, dtDel, out errMsgID, out args);
                if (!string.IsNullOrEmpty(errMsgID))
                {
                    if (ComFunc.IsVersionError(errMsgID))
                    {
                        this.RunSearch();
                    }
                    this.ShowMessage(errMsgID, args);
                    return false;
                }

                return ret;
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
        /// F1ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/09/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F01Button_Click(sender, e);
            try
            {
                RunSearch();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F3ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/09/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F03Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F03Button_Click(sender, e);
            try
            {
                int row = this.shtMeisai.ActivePosition.Row;
                if (this.shtMeisai.MaxRows > 0)
                {
                    // 選択行を削除してもよろしいですか？ダイアログ表示
                    if (this.ShowMessage("I0200010033") == DialogResult.OK)
                    {
                        this.shtMeisai.RemoveRow(row, false);
                        this.shtMeisai.UpdateData();
                        this.shtMeisai.Focus();
                    }
                }
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
        /// <create>T.Wakamatsu 2013/08/22</create>
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
