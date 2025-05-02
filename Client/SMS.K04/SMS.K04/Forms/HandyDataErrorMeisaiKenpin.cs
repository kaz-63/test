using System;
using System.Data;
using System.Windows.Forms;

using WsConnection;
using Commons;
using SystemBase.Util;

using WsConnection.WebRefK04;
using SMS.K04.Properties;
using SMS.E01;

namespace SMS.K04.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 取込エラー詳細(検品)
    /// </summary>
    /// <create>H.Tajimi 2018/10/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class HandyDataErrorMeisaiKenpin : SystemBase.Forms.CustomOrderForm
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
        /// <create>H.Tajimi 2018/10/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public HandyDataErrorMeisaiKenpin(UserInfo userInfo, string tempID)
            : base(userInfo, ComDefine.TITLE_K0400050)
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
        /// <create>H.Tajimi 2018/10/25</create>
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
                shtMeisai.ColumnHeaders[0].Caption = Resources.HandyDataErrorMeisaiKenpin_TehaiNo;
                shtMeisai.ColumnHeaders[1].Caption = Resources.HandyDataErrorMeisaiKenpin_Result;
                shtMeisai.ColumnHeaders[2].Caption = Resources.HandyDataErrorMeisaiKenpin_ErrotDetail;
                shtMeisai.ColumnHeaders[3].Caption = Resources.HandyDataErrorMeisaiKenpin_DeliveryDestination;
                shtMeisai.ColumnHeaders[4].Caption = Resources.HandyDataErrorMeisaiKenpin_ShipARNo;
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
        /// <create>H.Tajimi 2018/10/25</create>
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
        /// <create>H.Tajimi 2018/10/25</create>
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
        /// <create>H.Tajimi 2018/10/25</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                ConnK04 conn = new ConnK04();
                CondK04 cond = new CondK04(this.UserInfo);

                cond.TempID = this._tempID;
                DataSet ds = conn.GetTempworkMeisaiKenpin(cond);

                if (ComFunc.IsExistsTable(ds, Def_T_TEMPWORK_MEISAI.Name))
                {
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
        /// F4ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsuji 2020/06/17</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F04Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
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
        /// <create>T.SASAYAMA 2023/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F05Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F05Button_Click(sender, e);
            try
            {
                // 再試行
                this.RetryImportHoryuData();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F10ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsuji 2020/06/17</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F10Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F10Button_Click(sender, e);
            try
            {
                // 画面に貼り付けてある SaveFileDialog を使用するとボタン連打でStackOverFlowが発生する
                using (SaveFileDialog frm = new SaveFileDialog())
                {
                    frm.Title = Resources.HandyDataErrorMeisaiKenpin_sdfExcel_Title;
                    frm.Filter = Resources.HandyDataErrorMeisaiKenpin_sdfExcel_Filter;
                    frm.FileName = ComDefine.EXCEL_FILE_HANDYDATAERRORMEISAI_KENPIN;
                    if (0 < this.shtMeisai.MaxRows && frm.ShowDialog() != DialogResult.OK) return;

                    // NGレコードだけ抽出する
                    DataTable dtExport = (this.shtMeisai.DataSource as DataTable)
                        .AsEnumerable()
                        .Where(x => (string)x[ComDefine.FLD_RESULT_STRING] == RESULT.NG_NAME)
                        .Select(x => x)
                        .CopyToDataTable();

                    // Excel出力処理
                    ExportHandyDataErrorMeisaiKenpin export = new ExportHandyDataErrorMeisaiKenpin();
                    string msgID;
                    string[] args;
                    export.ExportExcel(frm.FileName, dtExport, out msgID, out args);
                    if (!string.IsNullOrEmpty(msgID))
                    {
                        this.ShowMessage(msgID, args);
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
        /// <create>H.Tajimi 2018/10/25</create>
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

        #region データテーブル取得

        /// --------------------------------------------------
        /// <summary>
        /// 一時作業データテーブル
        /// </summary>
        /// <param name="tableName">データテーブル名</param>
        /// <returns></returns>
        /// <create>H.Tsuji 2020/06/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSchemeTempwork(string tableName)
        {
            DataTable dt = new DataTable(tableName);
            dt.Columns.Add(Def_T_TEMPWORK.TEMP_ID, typeof(string));
            dt.Columns.Add(Def_T_TEMPWORK.TORIKOMI_DATE, typeof(DateTime));
            dt.Columns.Add(Def_T_TEMPWORK.TORIKOMI_FLAG, typeof(string));
            dt.Columns.Add(Def_T_TEMPWORK.DATA_NO, typeof(string));
            dt.Columns.Add(Def_T_TEMPWORK.ERROR_NUM, typeof(decimal));
            dt.Columns.Add(Def_T_TEMPWORK.STATUS_FLAG, typeof(string));
            return dt;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 一時作業明細データテーブル
        /// </summary>
        /// <param name="tableName">データテーブル名</param>
        /// <returns></returns>
        /// <create>H.Tsuji 2020/06/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSchemeTempworkMeisai(string tableName)
        {
            DataTable dt = new DataTable(tableName);
            dt.Columns.Add(Def_T_TEMPWORK_MEISAI.TEMP_ID, typeof(string));
            dt.Columns.Add(Def_T_TEMPWORK_MEISAI.ROW_NO, typeof(decimal));
            dt.Columns.Add(Def_T_TEMPWORK_MEISAI.RESULT, typeof(string));
            dt.Columns.Add(Def_T_TEMPWORK_MEISAI.PALLET_NO, typeof(string));
            dt.Columns.Add(Def_T_TEMPWORK_MEISAI.BOX_NO, typeof(string));
            dt.Columns.Add(Def_T_TEMPWORK_MEISAI.DATA_NO, typeof(string));
            dt.Columns.Add(Def_T_TEMPWORK_MEISAI.DESCRIPTION, typeof(string));
            dt.Columns.Add(Def_T_TEMPWORK_MEISAI.TEHAI_NO, typeof(string));
            dt.Columns.Add(Def_T_TEMPWORK_MEISAI.NYUKA_QTY, typeof(decimal));
            dt.Columns.Add(Def_T_TEMPWORK_MEISAI.WEIGHT, typeof(decimal));
            dt.Columns.Add(Def_T_TEMPWORK_MEISAI.HANDY_LOGIN_ID, typeof(string));
            return dt;
        }

        #endregion

        #region OKデータ再試行

        /// --------------------------------------------------
        /// <summary>
        /// OKデータ再試行
        /// </summary>
        /// <create>H.Tsuji 2020/06/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RetryImportData()
        {
            try
            {
                // OKレコードだけの一時取込明細データを作成する
                decimal rowCounter = 1;
                DataTable dtSheet = this.shtMeisai.DataSource as DataTable;
                DataTable dtOK = this.GetSchemeTempworkMeisai(Def_T_TEMPWORK_MEISAI.Name);
                foreach (DataRow dr in dtSheet.Rows)
                {
                    if ((string)dr[ComDefine.FLD_RESULT_STRING] != RESULT.OK_NAME) continue;

                    var drOK = dtOK.NewRow();
                    // 登録する際に一時取込IDを採番しなおすのでここでは仮IDとしてそのままセットする
                    drOK[Def_T_TEMPWORK_MEISAI.TEMP_ID] = dr[Def_T_TEMPWORK_MEISAI.TEMP_ID];
                    drOK[Def_T_TEMPWORK_MEISAI.ROW_NO] = rowCounter++;
                    drOK[Def_T_TEMPWORK_MEISAI.RESULT] = dr[Def_T_TEMPWORK_MEISAI.RESULT];
                    drOK[Def_T_TEMPWORK_MEISAI.TEHAI_NO] = dr[Def_T_TEMPWORK_MEISAI.TEHAI_NO];
                    drOK[Def_T_TEMPWORK_MEISAI.NYUKA_QTY] = dr[Def_T_TEMPWORK_MEISAI.NYUKA_QTY];
                    drOK[Def_T_TEMPWORK_MEISAI.HANDY_LOGIN_ID] = dr[Def_T_TEMPWORK_MEISAI.HANDY_LOGIN_ID];
                    dtOK.Rows.Add(drOK);
                }
                // OKレコードがなければメッセージを出して終了する
                if (dtOK.Rows.Count == 0)
                {
                    // 結果がOKの行が存在しません。
                    this.ShowMessage("K0400050001");
                    return;
                }

                // 結果がOKの行を対象に再試行を行います。\r\nよろしいですか？
                if (this.ShowMessage("K0400050002") != DialogResult.OK) return;

                // 一時取込データ作成
                DataTable dtTempWork = this.GetSchemeTempwork(Def_T_TEMPWORK.Name);
                DataRow drTempWork = dtTempWork.NewRow();
                drTempWork[Def_T_TEMPWORK.TEMP_ID] = ComFunc.GetFld(dtOK, 0, Def_T_TEMPWORK_MEISAI.TEMP_ID);
                drTempWork[Def_T_TEMPWORK.TORIKOMI_FLAG] = TORIKOMI_FLAG.KENPIN_VALUE1;
                drTempWork[Def_T_TEMPWORK.DATA_NO] = ComFunc.GetFld(dtOK, 0, Def_T_TEMPWORK_MEISAI.TEHAI_NO);
                drTempWork[Def_T_TEMPWORK.ERROR_NUM] = 0;
                drTempWork[Def_T_TEMPWORK.STATUS_FLAG] = STATUS_FLAG.MITORIKOMI_VALUE1;
                dtTempWork.Rows.Add(drTempWork);

                // データセットに格納
                DataSet ds = new DataSet();
                ds.Tables.Add(dtTempWork);
                ds.Tables.Add(dtOK);

                // OK分の検品データ取込を再試行する
                ConnK04 conn = new ConnK04();
                CondK04 cond = new CondK04(this.UserInfo);
                cond.TempID = ComFunc.GetFld(drTempWork, Def_T_TEMPWORK.TEMP_ID);
                DataTable dtMessage = ComFunc.GetSchemeMultiMessage();
                bool ret = conn.ImportRetryKenpin(cond, ds, ref dtMessage);
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

        #region 保留データ再試行

        /// --------------------------------------------------
        /// <summary>
        /// 保留データ再試行
        /// </summary>
        /// <create>T.SASAYAMA 2023/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RetryImportHoryuData()
        {
            try
            {
                // 保留レコードだけの一時取込明細データを作成する
                decimal rowCounter = 1;
                DataTable dtSheet = this.shtMeisai.DataSource as DataTable;
                DataTable dtHoryu = this.GetSchemeTempworkMeisai(Def_T_TEMPWORK_MEISAI.Name);
                foreach (DataRow dr in dtSheet.Rows)
                {
                    if ((string)dr[ComDefine.FLD_RESULT] != ComDefine.HORYU_VALUE2) continue;

                    var drHoryu = dtHoryu.NewRow();
                    // 登録する際に一時取込IDを採番しなおすのでここでは仮IDとしてそのままセットする
                    drHoryu[Def_T_TEMPWORK_MEISAI.TEMP_ID] = dr[Def_T_TEMPWORK_MEISAI.TEMP_ID];
                    drHoryu[Def_T_TEMPWORK_MEISAI.ROW_NO] = rowCounter++;
                    drHoryu[Def_T_TEMPWORK_MEISAI.RESULT] = dr[Def_T_TEMPWORK_MEISAI.RESULT];
                    drHoryu[Def_T_TEMPWORK_MEISAI.TEHAI_NO] = dr[Def_T_TEMPWORK_MEISAI.TEHAI_NO];
                    drHoryu[Def_T_TEMPWORK_MEISAI.NYUKA_QTY] = dr[Def_T_TEMPWORK_MEISAI.NYUKA_QTY];
                    drHoryu[Def_T_TEMPWORK_MEISAI.HANDY_LOGIN_ID] = dr[Def_T_TEMPWORK_MEISAI.HANDY_LOGIN_ID];
                    dtHoryu.Rows.Add(drHoryu);
                }
                // 保留レコードがなければメッセージを出して終了する
                if (dtHoryu.Rows.Count == 0)
                {
                    // 結果が保留の行が存在しません。
                    this.ShowMessage("K0400050003");
                    return;
                }

                // 結果が保留の行を対象に再試行を行います。\r\nよろしいですか？
                if (this.ShowMessage("K0400050004") != DialogResult.OK) return;

                // 一時取込データ作成
                DataTable dtTempWork = this.GetSchemeTempwork(Def_T_TEMPWORK.Name);
                DataRow drTempWork = dtTempWork.NewRow();
                drTempWork[Def_T_TEMPWORK.TEMP_ID] = ComFunc.GetFld(dtHoryu, 0, Def_T_TEMPWORK_MEISAI.TEMP_ID);
                drTempWork[Def_T_TEMPWORK.TORIKOMI_FLAG] = TORIKOMI_FLAG.KENPIN_VALUE1;
                drTempWork[Def_T_TEMPWORK.DATA_NO] = ComFunc.GetFld(dtHoryu, 0, Def_T_TEMPWORK_MEISAI.TEHAI_NO);
                drTempWork[Def_T_TEMPWORK.ERROR_NUM] = 0;
                drTempWork[Def_T_TEMPWORK.STATUS_FLAG] = STATUS_FLAG.MITORIKOMI_VALUE1;
                dtTempWork.Rows.Add(drTempWork);

                // データセットに格納
                DataSet ds = new DataSet();
                ds.Tables.Add(dtTempWork);
                ds.Tables.Add(dtHoryu);

                // 保留分の検品データ取込を再試行する
                ConnK04 conn = new ConnK04();
                CondK04 cond = new CondK04(this.UserInfo);
                cond.HoryuRetry = true;
                cond.TempID = ComFunc.GetFld(drTempWork, Def_T_TEMPWORK.TEMP_ID);
                DataTable dtMessage = ComFunc.GetSchemeMultiMessage();
                bool ret = conn.ImportRetryKenpin(cond, ds, ref dtMessage);
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
