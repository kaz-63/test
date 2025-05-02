using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
    /// ハンディデータ取込
    /// </summary>
    /// <create>T.Wakamatsu 2013/08/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class HandyDataImport : SystemBase.Forms.CustomOrderForm
    {
        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 0;

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
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public HandyDataImport(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // フォームの設定
                this.IsCloseQuestion = true;
                // シートの初期化
                this.InitializeSheet(this.shtMeisai);
                this.shtMeisai.KeepHighlighted = true;

                // シートのタイトルを設定
                shtMeisai.ColumnHeaders[0].Caption = Resources.HandyDataImport_CaptureDate;
                shtMeisai.ColumnHeaders[1].Caption = Resources.HandyDataImport_Work;
                shtMeisai.ColumnHeaders[2].Caption = Resources.HandyDataImport_Worker;
                shtMeisai.ColumnHeaders[3].Caption = Resources.HandyDataImport_Location;
                shtMeisai.ColumnHeaders[4].Caption = Resources.HandyDataImport_No;
                shtMeisai.ColumnHeaders[5].Caption = Resources.HandyDataImport_ErrorNumber;

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
                this.shtMeisai.Enabled = false;
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
        /// 検索処理制御部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>T.Wakamatsu 2013/08/22</create>
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
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                ConnI02 conn = new ConnI02();
                CondI02 cond = new CondI02(this.UserInfo);
                cond.StatusFlag = STATUS_FLAG.ERROR_VALUE1;

                DataSet ds = conn.GetTempwork(cond);

                this.shtMeisai.Enabled = false;
                if (ComFunc.IsExistsTable(ds, Def_T_BUHIN_TEMPWORK.Name))
                {
                    this.shtMeisai.DataSource = ds.Tables[Def_T_BUHIN_TEMPWORK.Name];
                    // 最も左上に表示されているセルの設定
                    if (0 < this.shtMeisai.MaxRows)
                    {
                        this.shtMeisai.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtMeisai.TopLeft.Row);
                        this.shtMeisai.Enabled = true;
                    }
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
        /// F1ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            try
            {
                // 取込
                this.ImportData();
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
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F03Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F03Button_Click(sender, e);
            try
            {
                // 破棄
                this.DestructionImportData();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F4ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F04Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F04Button_Click(sender, e);
            try
            {
                // 再試行
                this.RetryImportData();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F5ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F05Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F05Button_Click(sender, e);
            try
            {
                this.RunSearch();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F8ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F08Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F08Button_Click(sender, e);
            try
            {
                DataRow dr = this.GetSelectSheetRowData();
                if (dr == null)
                {
                    // データが選択されていません。
                    this.ShowMessage("A9999999019");
                    return;
                }
                SystemBase.Forms.CustomOrderForm frm = null;
                try
                {
                    string torikomiFlag = ComFunc.GetFld(dr, Def_T_BUHIN_TEMPWORK.TORIKOMI_FLAG);
                    if (torikomiFlag == ZAIKO_TORIKOMI_FLAG.LOCATION_VALUE1 || torikomiFlag == ZAIKO_TORIKOMI_FLAG.KANRYO_VALUE1)
                    {
                        // 入出庫
                        frm = new HandyDataErrorMeisai(this.UserInfo, dr, ComDefine.TITLE_I0200020);
                    }
                    else
                    {
                        // 棚卸
                        frm = new HandyDataErrorMeisai(this.UserInfo, dr, ComDefine.TITLE_I0200030);
                    }
                    frm.Icon = this.Icon;
                    frm.ShowDialog(this);
                }
                finally
                {
                    if (frm != null && !frm.IsDisposed)
                    {
                        frm.Dispose();
                    }
                }
                this.RunSearch();
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

        #region グリッドの選択行のDataRow取得

        /// --------------------------------------------------
        /// <summary>
        /// グリッドの選択行のDataRow取得
        /// </summary>
        /// <returns></returns>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataRow GetSelectSheetRowData()
        {
            try
            {
                DataTable dt = this.shtMeisai.DataSource as DataTable;
                int rowIndex = this.shtMeisai.ActivePosition.Row;
                if (dt == null || this.shtMeisai.MaxRows == 0 || dt.Rows.Count <= rowIndex) return null;
                return dt.Rows[rowIndex];

            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region 画面操作モード切り替え

        /// --------------------------------------------------
        /// <summary>
        /// 操作モードの変更(取込操作時に操作不可にする為)
        /// </summary>
        /// <param name="isEnabled">Enabled</param>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeModeAllOperation(bool isEnabled)
        {
            try
            {
                this.pnlMain.Enabled = isEnabled;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 取込

        /// --------------------------------------------------
        /// <summary>
        /// 取込処理
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ImportData()
        {
            try
            {
                using (ImportProgress frm = new ImportProgress(this.UserInfo))
                {
                    frm.Icon = this.Icon;
                    frm.ShowDialog(this);
                }
                // 再表示
                this.RunSearch();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.ChangeModeAllOperation(true);
            }
        }

        #endregion

        #region 破棄

        /// --------------------------------------------------
        /// <summary>
        /// 選択データ破棄
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void DestructionImportData()
        {
            try
            {
                DataRow dr = this.GetSelectSheetRowData();
                if (dr == null)
                {
                    // データが選択されていません。
                    this.ShowMessage("A9999999019");
                    return;
                }
                // 選択データを破棄します。\r\nよろしいですか？
                if (this.ShowMessage("K0400010028") != DialogResult.OK) return;

                ConnI02 conn = new ConnI02();
                CondI02 cond = new CondI02(this.UserInfo);
                cond.TempID = ComFunc.GetFld(dr, Def_T_BUHIN_TEMPWORK.TEMP_ID);
                cond.TorikomiFlag = ComFunc.GetFld(dr, Def_T_BUHIN_TEMPWORK.TORIKOMI_FLAG);
                DataTable dtMessage = ComFunc.GetSchemeMultiMessage();
                bool ret = conn.DestroyData(cond, ref dtMessage);
                if (ret)
                {
                    // 破棄しました。
                    this.ShowMessage("K0400010023");
                }
                else if (0 < dtMessage.Rows.Count)
                {
                    if (dtMessage.Rows.Count == 1)
                    {
                        string messageID = ComFunc.GetFld(dtMessage, 0, Def_M_MESSAGE.MESSAGE_ID);
                        string[] args = (string[])ComFunc.GetFldObject(dtMessage, 0, ComDefine.FLD_MESSAGE_PARAMETER);
                        this.ShowMessage(messageID, args);
                    }
                    else
                    {
                        // 処理に失敗しました。
                        this.ShowMultiMessage(dtMessage, "K0400010031");
                    }
                }
                else
                {
                    // 処理に失敗しました。
                    this.ShowMessage("K0400010031");
                }

                // 再表示
                this.RunSearch();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 再試行

        /// --------------------------------------------------
        /// <summary>
        /// 選択データ再試行
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RetryImportData()
        {
            try
            {
                DataRow dr = this.GetSelectSheetRowData();
                if (dr == null)
                {
                    // データが選択されていません。
                    this.ShowMessage("A9999999019");
                    return;
                }
                // 選択データを再試行します。\r\nよろしいですか？
                if (this.ShowMessage("K0400010029") != DialogResult.OK) return;

                string tempID = ComFunc.GetFld(dr, Def_T_BUHIN_TEMPWORK.TEMP_ID);

                ConnI02 conn = new ConnI02();
                CondI02 cond = new CondI02(this.UserInfo);
                cond.TempID = tempID;
                // 棚卸日付はクライアント側日付で登録する。
                cond.InventDate = DateTime.Today.ToString("yyyy/MM/dd");
                DataTable dtMessage = ComFunc.GetSchemeMultiMessage();
                bool ret = conn.ImportRetry(cond, ref dtMessage);
                if (ret)
                {
                    // 取込みました。
                    this.ShowMessage("K0400010030");
                }
                else if (0 < dtMessage.Rows.Count)
                {
                    if (dtMessage.Rows.Count == 1)
                    {
                        string messageID = ComFunc.GetFld(dtMessage, 0, Def_M_MESSAGE.MESSAGE_ID);
                        string[] args = (string[])ComFunc.GetFldObject(dtMessage, 0, ComDefine.FLD_MESSAGE_PARAMETER);
                        this.ShowMessage(messageID, args);
                    }
                    else
                    {
                        // 処理に失敗しました。
                        this.ShowMultiMessage(dtMessage, "K0400010031");
                    }
                }
                else
                {
                    // 取込時にエラーがありました。確認して下さい。
                    this.ShowMessage("K0400010032");
                }
                // 再表示
                this.RunSearch();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion
    }
}
