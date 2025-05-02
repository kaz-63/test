using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

using DSWUtil;
using Commons;
using XlsCreatorHelper;

namespace SystemBase.Excel
{
    /// --------------------------------------------------
    /// <summary>
    /// Excel出力ベースクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/07/20</create>
    /// <update></update>
    /// --------------------------------------------------
    public class BaseExport
    {
        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected BaseExport()
        {
        }

        #endregion

        #region Excel出力(外部アクセス用)

        /// --------------------------------------------------
        /// <summary>
        /// Excel出力
        /// </summary>
        /// <param name="filePath">ファイル名</param>
        /// <param name="dt">出力データ</param>
        /// <param name="expInfoCollection">エクスポート情報</param>
        /// <param name="msgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public virtual bool ExportExcel(string filePath, DataTable dt, ExportInfoCollection expInfoCollection, out string msgID, out string[] args)
        {
            msgID = string.Empty;
            args = null;
            try
            {
                bool ret = this.PutSimpleExcel(filePath, dt, expInfoCollection, ref msgID, ref args);
                if (!ret && string.IsNullOrEmpty(msgID))
                {
                    // Excel出力に失敗しました。
                    msgID = "A7777777001";
                }
                return ret;
            }
            catch (Exception)
            {
                if (string.IsNullOrEmpty(msgID))
                {
                    // Excel出力に失敗しました。
                    msgID = "A7777777001";
                }
                return false;
            }
        }

        #endregion

        #region Excel出力(内部処理)

        /// --------------------------------------------------
        /// <summary>
        /// Excel出力(内部処理)
        /// </summary>
        /// <param name="filePath">ファイル名</param>
        /// <param name="dt">出力データ</param>
        /// <param name="expInfoCollection">エクスポート情報</param>
        /// <param name="msgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual bool PutSimpleExcel(string filePath, DataTable dt, ExportInfoCollection expInfoCollection, ref string msgID, ref string[] args)
        {
            try
            {
                using (XlsCreator xls = new XlsCreator())
                {
                    if (File.Exists(filePath))
                    {
                        try
                        {
                            File.Delete(filePath);
                        }
                        catch (IOException ioEx)
                        {
                            // {0}\r\nファイルが削除出来ませんでした。
                            msgID = "A7777777002";
                            args = new string[] { ioEx.Message };
                            return false;
                        }
                    }
                    xls.CreateBook(filePath, 1, xlVersion.ver2003);

                    ExportInfoCollection putExpInfoCollection;
                    if (!this.PutSimpleExcelCreateTargetList(dt, expInfoCollection, out putExpInfoCollection, ref msgID, ref args))
                    {
                        return false;
                    }
                    // ヘッダの出力
                    if (!this.PutSimpleExcelPutHeader(xls, dt, putExpInfoCollection, ref msgID, ref args))
                    {
                        return false;
                    }
                    // データの出力
                    if (!this.PutSimpleExcelPutData(xls, dt, putExpInfoCollection, ref msgID, ref args))
                    {
                        return false;
                    }
                    // ヘッダの罫線
                    if (!this.PutSimpleExcelPutHeaderLine(xls, dt, putExpInfoCollection, ref msgID, ref args))
                    {
                        return false;
                    }
                    // データの罫線
                    if (!this.PutSimpleExcelPutDataLine(xls, dt, putExpInfoCollection, ref msgID, ref args))
                    {
                        return false;
                    }

                    xls.CloseBook(true);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 出力用ExportInfoCollectionの作成
        /// </summary>
        /// <param name="dt">出力データ</param>
        /// <param name="expInfoCollection">エクスポート情報</param>
        /// <param name="putCollection">出力用エクスポート情報</param>
        /// <param name="msgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update>J.Chen 2023/12/15 Decimal追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual bool PutSimpleExcelCreateTargetList(DataTable dt, ExportInfoCollection expInfoCollection, out ExportInfoCollection putCollection, ref string msgID, ref string[] args)
        {
            putCollection = new ExportInfoCollection();
            foreach (var item in expInfoCollection)
            {
                ExportInfo expInfo = item.Clone();
                putCollection.Add(expInfo);
                if (!dt.Columns.Contains(expInfo.DataColName))
                {
                    expInfo.DataColName = string.Empty;
                    continue;
                }
                DataColumn col = dt.Columns[expInfo.DataColName];
                // データタイプ
                if (col.DataType == typeof(DateTime) || col.DataType == typeof(TimeSpan) || col.DataType == typeof(DateTimeOffset))
                {
                    expInfo.DataType = ExportDataType.DateTime;
                }
                else if (col.DataType == typeof(string))
                {
                    expInfo.DataType = ExportDataType.String;
                }
                else if (expInfo.DataType == ExportDataType.Decimal)
                {
                    expInfo.DataType = ExportDataType.Decimal;
                }
                else
                {
                    expInfo.DataType = ExportDataType.Other;
                }
            }

            return true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// ヘッダ出力
        /// </summary>
        /// <param name="xls">XlsCreator</param>
        /// <param name="dt">出力データ</param>
        /// <param name="expInfoCollection">エクスポート情報</param>
        /// <param name="msgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual bool PutSimpleExcelPutHeader(XlsCreator xls, DataTable dt, ExportInfoCollection expInfoCollection, ref string msgID, ref string[] args)
        {
            if (!expInfoCollection.IsHeader) return true;
            int col = expInfoCollection.StartCol;
            int row = expInfoCollection.HeaderStartRow;
            foreach (var expInfo in expInfoCollection)
            {
                xls.Pos(col, row).Value = expInfo.Caption;
                xls.Pos(col, row).Attr.PosHorz = xlPosHorz.phCenter;
                col++;
            }
            return true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// データ出力
        /// </summary>
        /// <param name="xls">XlsCreator</param>
        /// <param name="dt">出力データ</param>
        /// <param name="expInfoCollection">エクスポート情報</param>
        /// <param name="msgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update>J.Chen 2023/12/15 Decimal追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual bool PutSimpleExcelPutData(XlsCreator xls, DataTable dt, ExportInfoCollection expInfoCollection, ref string msgID, ref string[] args)
        {
            int col = expInfoCollection.StartCol;
            int row = expInfoCollection.DataStartRow;
            foreach (DataRow dr in dt.Rows)
            {
                col = expInfoCollection.StartCol;
                foreach (var expInfo in expInfoCollection)
                {
                    if (string.IsNullOrEmpty(expInfo.DataColName)) continue;
                    switch (expInfo.DataType)
                    {
                        case ExportDataType.String:
                            xls.Pos(col, row).Str = ComFunc.GetFld(dr, expInfo.DataColName);
                            break;
                        case ExportDataType.DateTime:
                            if (ComFunc.GetFldObject(dr, expInfo.DataColName) != DBNull.Value)
                            {
                                xls.Pos(col, row).Str = ComFunc.GetFldToDateTime(dr, expInfo.DataColName).ToString(expInfo.DateTimeFormat);
                            }
                            break;
                        case ExportDataType.Other:
                            xls.Pos(col, row).Value = ComFunc.GetFldObject(dr, expInfo.DataColName);
                            break;
                        case ExportDataType.Decimal:
                            // 小数点第2位まで表示
                            decimal value = ComFunc.GetFldToDecimal(dr, expInfo.DataColName);
                            xls.Pos(col, row).Value = (value == 0) ? "" : value.ToString("#,0.00");
                            break;
                        default:
                            break;
                    }
                    xls.Pos(col, row).Attr.OverReturn = false;
                    col++;
                }
                row++;
            }
            return true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// ヘッダ罫線出力
        /// </summary>
        /// <param name="xls">XlsCreator</param>
        /// <param name="dt">出力データ</param>
        /// <param name="expInfoCollection">エクスポート情報</param>
        /// <param name="msgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual bool PutSimpleExcelPutHeaderLine(XlsCreator xls, DataTable dt, ExportInfoCollection expInfoCollection, ref string msgID, ref string[] args)
        {
            if (!expInfoCollection.IsHeader || !expInfoCollection.IsLine) return true;
            int colStart = expInfoCollection.StartCol;
            int rowStart = expInfoCollection.HeaderStartRow;

            int colEnd = xls.MaxData(xlPoint.ptMaxPoint).Width;
            int rowEnd = expInfoCollection.HeaderStartRow;
            xls.Pos(colStart, rowStart, colEnd, rowEnd).Attr.Box(xlBoxType.btLtc, xlLineStyle.lsNormal, xlColor.xcBlack);
            return true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// データ罫線出力
        /// </summary>
        /// <param name="xls">XlsCreator</param>
        /// <param name="dt">出力データ</param>
        /// <param name="expInfoCollection">エクスポート情報</param>
        /// <param name="msgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual bool PutSimpleExcelPutDataLine(XlsCreator xls, DataTable dt, ExportInfoCollection expInfoCollection, ref string msgID, ref string[] args)
        {
            if (!expInfoCollection.IsLine) return true;
            int colStart = expInfoCollection.StartCol;
            int rowStart = expInfoCollection.DataStartRow;

            Size size = xls.MaxData(xlPoint.ptMaxPoint);
            int colEnd = size.Width;
            int rowEnd = size.Height;
            xls.Pos(colStart, rowStart, colEnd, rowEnd).Attr.Box(xlBoxType.btLtc, xlLineStyle.lsNormal, xlColor.xcBlack);
            return true;
        }

        #endregion
    }
}
