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
    /// 取込エラー詳細(集荷)
    /// </summary>
    /// <create>Y.Higuchi 2010/08/10</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class HandyDataErrorMeisaiShuka : SystemBase.Forms.CustomOrderForm
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
        public HandyDataErrorMeisaiShuka(UserInfo userInfo, string tempID)
            : base(userInfo, ComDefine.TITLE_K0400020)
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
                shtMeisai.ColumnHeaders[0].Caption = Resources.HandyDataErrorMeisaiShuka_MarkingTagNo;
                shtMeisai.ColumnHeaders[1].Caption = Resources.HandyDataErrorMeisaiShuka_Result;
                shtMeisai.ColumnHeaders[2].Caption = Resources.HandyDataErrorMeisaiShuka_ErrorDetail;
                shtMeisai.ColumnHeaders[3].Caption = Resources.HandyDataErrorMeisaiShuka_DeliveryDestination;
                shtMeisai.ColumnHeaders[4].Caption = Resources.HandyDataErrorMeisaiShuka_ShipARNo;
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

        #region RowFilling

        /// --------------------------------------------------
        /// <summary>
        /// データソースに DataRow が追加または削除され、シートの行が追加または削除される前に発生するイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>J.Chen 2024/11/11</create>
        /// <update></update>
        private void shtMeisai_RowFilling(object sender, RowFillingEventArgs e)
        {
            var sheet = sender as Sheet;
            if (sheet == null)
            {
                return;
            }

            if (e.DestRow != -1 && e.OperationMode == OperationMode.Add)
            {
                // NG色(行単位)の設定
                var row = e.DestRow;
                var dt = sheet.DataSource as DataTable;

                //取込み結果取得
                var result = shtMeisaiGetSourceData<string>(dt, e.SourceRow, ComDefine.FLD_RESULT_STRING);
                if (result == RESULT.NG_NAME)     //取込み結果がNGの場合
                {
                    var color = ComDefine.ERROR_COLOR;
                    SetupRowColor(sheet.Rows[row], color, sheet.GridLine, Borders.All);
                }
                else
                {
                    sheet.Rows[row].BackColor = Color.Empty;
                    sheet.Rows[row].ForeColor = Color.Empty;
                    sheet.Rows[row].DisabledBackColor = Color.FromArgb(223, 223, 223);
                    sheet.Rows[row].DisabledForeColor = Color.Black;
                }
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// データソースに該当する列名の情報を取得する
        /// </summary>
        /// <typeparam name="T">値の種類</typeparam>
        /// <param name="dt">データソース</param>
        /// <param name="sourceRow">行データ(生データ)</param>
        /// <param name="colName">列名称</param>
        /// <returns>セルの値</returns>
        /// <create>J.Chen 2024/11/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private static T shtMeisaiGetSourceData<T>(DataTable dt, object sourceRow, string colName)
        {
            int offset = dt.Columns[colName].Ordinal;
            object data = ((object[])sourceRow)[offset];
            return (T)data;
        }

        #endregion

        #region シートのBackColor/ForeColor変更関連

        /// --------------------------------------------------
        /// <summary>
        /// 行の背景色、及び前景色を変更する
        /// </summary>
        /// <param name="row">列</param>
        /// <param name="input">入力文字列</param>
        /// <param name="baseGridLine">基準となる罫線設定</param>
        /// <param name="border">罫線設定</param>
        /// <create>J.Chen 2024/11/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetupRowColor(Row row, string input, GridLine baseGridLine, Borders border)
        {
            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            var cols = input.Split(',');
            if (cols.Length < 2)
            {
                return;
            }

            var backcolor = ComFunc.GetColorFromRgb(cols[1]);
            if (backcolor != null)
            {
                row.BackColor = backcolor ?? row.BackColor;
                row.DisabledBackColor = backcolor ?? row.BackColor;
                // 背景初期を設定すると、罫線がなくなってしまうため、手動設定する
                row.SetBorder(new BorderLine()
                {
                    Color = baseGridLine.Color,
                    LineStyle = (BorderLineStyle)Enum.Parse(typeof(BorderLineStyle), Enum.GetName(typeof(GridLineStyle), baseGridLine.Style)),
                }, border);
            }

            var forecolor = ComFunc.GetColorFromRgb(cols[0]);
            if (forecolor != null)
            {
                row.ForeColor = forecolor ?? row.ForeColor;
                row.DisabledForeColor = forecolor ?? row.ForeColor;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 範囲の背景色、及び前景色を変更する
        /// </summary>
        /// <param name="range">範囲</param>
        /// <param name="input">入力文字列</param>
        /// <param name="baseGridLine">基準となる罫線設定</param>
        /// <param name="border">罫線設定</param>
        /// <create>J.Chen 2024/11/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetupRangeColor(Sheet sheet, Range range, string input, GridLine baseGridLine, Borders border)
        {
            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            var cols = input.Split(',');
            if (cols.Length < 2)
            {
                return;
            }

            var backcolor = ComFunc.GetColorFromRgb(cols[1]);
            if (backcolor != null)
            {
                sheet.SetCellInfo(range, CellInfo.BackColor, backcolor.Value);
                sheet.SetCellInfo(range, CellInfo.DisabledBackColor, backcolor.Value);
                // 背景初期を設定すると、罫線がなくなってしまうため、手動設定する
                sheet.SetBorder(range, new BorderLine()
                {
                    Color = baseGridLine.Color,
                    LineStyle = (BorderLineStyle)Enum.Parse(typeof(BorderLineStyle), Enum.GetName(typeof(GridLineStyle), baseGridLine.Style)),
                }, border);
            }

            var forecolor = ComFunc.GetColorFromRgb(cols[0]);
            if (forecolor != null)
            {
                sheet.SetCellInfo(range, CellInfo.ForeColor, forecolor.Value);
                sheet.SetCellInfo(range, CellInfo.DisabledForeColor, forecolor.Value);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// セルの背景色、及び前景色を変更する
        /// </summary>
        /// <param name="cell">セル</param>
        /// <param name="input">入力文字列</param>
        /// <param name="baseGridLine">基準となる罫線設定</param>
        /// <param name="border">罫線設定</param>
        /// <create>J.Chen 2024/11/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetupCellColor(Cell cell, string input, GridLine baseGridLine, Borders border)
        {
            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            var cols = input.Split(',');
            if (cols.Length < 2)
            {
                return;
            }

            var backcolor = ComFunc.GetColorFromRgb(cols[1]);
            if (backcolor != null)
            {
                cell.BackColor = backcolor ?? cell.BackColor;
                cell.DisabledBackColor = backcolor ?? cell.BackColor;
                // 背景初期を設定すると、罫線がなくなってしまうため、手動設定する
                cell.SetBorder(new BorderLine()
                {
                    Color = baseGridLine.Color,
                    LineStyle = (BorderLineStyle)Enum.Parse(typeof(BorderLineStyle), Enum.GetName(typeof(GridLineStyle), baseGridLine.Style)),
                }, border);
            }

            var forecolor = ComFunc.GetColorFromRgb(cols[0]);
            if (forecolor != null)
            {
                cell.ForeColor = forecolor ?? cell.ForeColor;
                cell.DisabledForeColor = forecolor ?? cell.ForeColor;
            }
        }

        #endregion

        #endregion
    }
}
