using SystemBase.Excel;
using System.Data;
using System;
using XlsxCreatorHelper;
using Commons;
using System.Linq;
using WsConnection;
using WsConnection.WebRefAttachFile;
using System.IO;
using System.Drawing;
using System.Globalization;
using DSWUtil;
using SMS.E01.Properties;

namespace SMS.E01
{
    /// --------------------------------------------------
    /// <summary>
    /// 荷姿表Excel出力
    /// </summary>
    /// <create>H.Tajimi 2018/11/29</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ExportPacking : BaseExport
    {
        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// ヘッダ開始行
        /// </summary>
        /// <create>H.Tajimi 2018/11/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int HEADERS_START_ROW = 3 - 1;
        /// --------------------------------------------------
        /// <summary>
        /// データ印字開始行
        /// </summary>
        /// <create>H.Tajimi 2018/11/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int DATA_START_ROW = 6 - 1;
        /// --------------------------------------------------
        /// <summary>
        /// データ印字開始行2
        /// </summary>
        /// <create>T.SASAYAMA 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int DATA_START_ROW2 = 8 - 1;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>H.Tajimi 2018/11/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public ExportPacking()
            : base()
        {
        }

        #endregion

        #region ExportExcel

        /// --------------------------------------------------
        /// <summary>
        /// 荷姿表Excelの出力
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="dt"></param>
        /// <param name="msgID"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/12/04</create>
        /// <update>H.Tajimi 2020/04/14 出荷元追加</update>
        /// <update>K.Harada 2022/03/07 EXCEL出力列変更</update>
        /// --------------------------------------------------
        public bool ExportExcel(string filePath, DataTable dt, out string msgID, out string[] args)
        {
            msgID = string.Empty;
            args = null;
            try
            {
                // ディレクトリ作成
                try
                {
                    var dirPath = Path.GetDirectoryName(filePath);
                    // フォルダの存在チェック
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                }
                catch{}

                // ファイル削除
                try
                {
                    if (File.Exists(filePath))
                    {
                        UtilFile.DeleteFile(filePath);
                    }
                }
                catch{}

                bool ret = false;
                var xlsx = new XlsxCreator();
                xlsx.OpenBook(filePath, Path.Combine(ComDefine.DOWNLOAD_DIR, ComDefine.EXCEL_FILE_PACKING_TEMPLATE));

                // シート名設定
                xlsx.SheetName = Resources.ExportPacking_SheetNamePacking;

                var expInfoCollection = new ExportInfoCollection();

                // 出荷日の出力
                xlsx.Pos(3, HEADERS_START_ROW).Str = ComFunc.GetFld(dt, 0, Def_T_PACKING.SYUKKA_DATE);

                // 備考
                expInfoCollection.Add(Def_T_PACKING_MEISAI.NOTE, string.Empty);
                // NO
                expInfoCollection.Add(ComDefine.FLD_ROW_INDEX, string.Empty);
                // 出荷元
                expInfoCollection.Add(Def_M_SHIP_FROM.SHIP_FROM_NAME, string.Empty);
                // 運送会社
                expInfoCollection.Add(Def_M_UNSOKAISHA.UNSOKAISHA_NAME, string.Empty);
                // インボイス番号
                expInfoCollection.Add(Def_T_PACKING.INVOICE_NO, string.Empty);
                // アイテム名
                expInfoCollection.Add(Def_M_NONYUSAKI.NONYUSAKI_NAME, string.Empty);
                // 製番
                expInfoCollection.Add(Def_T_PACKING_MEISAI.SEIBAN_CODE, string.Empty);
                // AR番号
                expInfoCollection.Add(Def_T_PACKING_MEISAI.AR_NO, string.Empty);
                // 技連番号
                expInfoCollection.Add(Def_T_PACKING_MEISAI.ECS_NO, string.Empty);
                // 宛先
                expInfoCollection.Add(Def_T_PACKING_MEISAI.ATTN, string.Empty);
                // カートン数
                expInfoCollection.Add(ComDefine.FLD_CARTON_QTY, string.Empty);
                // カートンNo
                expInfoCollection.Add(ComDefine.FLD_CARTON_NO, string.Empty);
                // パレット数
                expInfoCollection.Add(ComDefine.FLD_PALLET_QTY, string.Empty);
                // 梱包No.
                expInfoCollection.Add(ComDefine.FLD_KONPO_NO, string.Empty);
                // 寸法L
                expInfoCollection.Add(Def_T_PACKING_MEISAI.SIZE_L, string.Empty);
                // 寸法W
                expInfoCollection.Add(Def_T_PACKING_MEISAI.SIZE_W, string.Empty);
                // 寸法H
                expInfoCollection.Add(Def_T_PACKING_MEISAI.SIZE_H, string.Empty);
                // 重量
                expInfoCollection.Add(Def_T_PACKING_MEISAI.GRWT, string.Empty);
                // 製品名
                expInfoCollection.Add(Def_T_PACKING_MEISAI.PRODUCT_NAME, string.Empty);

                expInfoCollection.IsHeader = false;
                expInfoCollection.IsLine = true;
                expInfoCollection.StartCol = 0;
                expInfoCollection.DataStartRow = DATA_START_ROW;

                // Excel出力
                ret = this.PusSimpleExcel(xlsx, dt, expInfoCollection, ref msgID, ref args);

                // データの罫線出力
                ret = this.PutSimpleExcelPutDataLine(xlsx, dt, expInfoCollection, ref msgID, ref args, 1);

                //// テンプレートシートの削除
                //xlsx.DelSheet(xlsx.SheetNo2(SHEET_NAME_TEMPLATE_PACKING), 1);

                xlsx.CloseBook(true);

                // Excelファイルの存在で戻り値設定
                if (ret)
                {
                    ret = File.Exists(filePath);
                }

                if (!ret)
                {
                    // Excel出力に失敗しました。
                    msgID = "A7777777001";
                }

                return true;
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

        #region ExportExcel出荷計画

        /// --------------------------------------------------
        /// <summary>
        /// 出荷計画Excelの出力
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="dt"></param>
        /// <param name="msgID"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <create>T.SASAYAMA 2023/08/24</create>
        /// --------------------------------------------------
        public bool ExportExcelShukka(string filePath, DataTable shukkaDt, DataTable revDt, out string msgID, out string[] args)
        {
            msgID = string.Empty;
            args = null;
            try
            {
                // ディレクトリ作成
                try
                {
                    var dirPath = Path.GetDirectoryName(filePath);
                    // フォルダの存在チェック
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                }
                catch { }

                // ファイル削除
                try
                {
                    if (File.Exists(filePath))
                    {
                        UtilFile.DeleteFile(filePath);
                    }
                }
                catch { }

                bool ret = false;
                var xlsx = new XlsxCreator();
                xlsx.OpenBook(filePath, Path.Combine(ComDefine.DOWNLOAD_DIR, ComDefine.EXCEL_FILE_SHUKKAKEIKAKU_TEMPLATE));

                // シート名設定
                xlsx.SheetNo = 0;
                //xlsx.SheetName = "1枚目";

                var expInfoCollection = new ExportInfoCollection();

                // 出荷日の出力
                xlsx.Pos(0, 0).Str = ComFunc.GetFld(shukkaDt, 0, Def_M_BUKKEN.BUKKEN_NAME);
                xlsx.Pos(0, 2).Str = ComFunc.GetFld(revDt, 0, Def_M_MAIL_SEND_RIREKI.BUKKEN_REV);
                xlsx.Pos(0, 3).Str = ComFunc.GetFld(shukkaDt, 0, Def_M_NONYUSAKI.SHIP_SEIBAN);

                // BUKKEN_REVのソート
                if (revDt != null && revDt.Rows.Count > 0)
                {
                    revDt = revDt.AsEnumerable().Reverse().CopyToDataTable();
                }

                // 処理フラグ
                expInfoCollection.Add(ComDefine.FLD_EXCEL_SHORI_FLAG, string.Empty);
                // 便名
                expInfoCollection.Add(Def_M_NONYUSAKI.SHIP, string.Empty);
                // 有償or無償
                expInfoCollection.Add(Def_M_NONYUSAKI.ESTIMATE_FLAG, string.Empty);
                // AIRSHIP
                expInfoCollection.Add(Def_M_NONYUSAKI.TRANSPORT_FLAG, string.Empty);
                // 出荷元
                expInfoCollection.Add(Def_M_NONYUSAKI.SHIP_FROM, string.Empty);
                // 出荷先
                expInfoCollection.Add(Def_M_NONYUSAKI.SHIP_TO, string.Empty);
                // 出荷日
                expInfoCollection.Add(Def_M_NONYUSAKI.SHIP_DATE, string.Empty, "M/d(ddd)");

                //var exportInfo = new ExportInfo();
                //exportInfo.DataColName = "SHIP_DATE";
                //exportInfo.DataType = ExportDataType.DateTime;
                //exportInfo.DateTimeFormat = "MM/dd"; // 必要なフォーマット
                //expInfoCollection.Add(exportInfo);

                // 製番
                expInfoCollection.Add(Def_M_NONYUSAKI.SEIBAN, string.Empty);
                // 機種
                expInfoCollection.Add(Def_M_NONYUSAKI.KISHU, string.Empty);
                // 内容
                expInfoCollection.Add(Def_M_NONYUSAKI.NAIYO, string.Empty);
                // 到着予定日
                expInfoCollection.Add(Def_M_NONYUSAKI.TOUCHAKUYOTEI_DATE, string.Empty, "M/d(ddd)");
                // 機械Parts
                expInfoCollection.Add(Def_M_NONYUSAKI.KIKAI_PARTS, string.Empty);
                // 制御Parts
                expInfoCollection.Add(Def_M_NONYUSAKI.SEIGYO_PARTS, string.Empty);
                // 備考
                expInfoCollection.Add(Def_M_NONYUSAKI.BIKO, string.Empty);
                // 処理フラグ（前回値）
                expInfoCollection.Add(Def_M_NONYUSAKI.LAST_SYORI_FLAG, string.Empty);

                expInfoCollection.IsHeader = false;
                expInfoCollection.IsLine = true;
                expInfoCollection.StartCol = 0;
                expInfoCollection.DataStartRow = DATA_START_ROW2;

                // Excel出力
                ret = this.PusSimpleExcel(xlsx, shukkaDt, expInfoCollection, ref msgID, ref args);

                // データの罫線出力
                ret = this.PutSimpleExcelPutDataLine(xlsx, shukkaDt, expInfoCollection, ref msgID, ref args, 0);

                // 2つ目のシートの追加
                //xlsx.AddSheet(1, -1);
                xlsx.SheetNo = 1;
                //xlsx.SheetName = "2枚目";

                expInfoCollection = new ExportInfoCollection();

                //セルの結合処理
                //for (int i = 0; i < revDt.Rows.Count; i++)
                //{
                //    int rowNumber = i + 1; 
                //    string cellRange = "C" + rowNumber.ToString() + ":F" + rowNumber.ToString();
                //    xlsx.Cell(cellRange).Attr.Joint = true;
                //    // フォントサイズを12に設定
                //    xlsx.Cell(cellRange).Attr.FontPoint = 12;
                //}

                // 備考
                expInfoCollection.Add(Def_M_MAIL_SEND_RIREKI.BUKKEN_REV, string.Empty);
                expInfoCollection.Add(Def_M_MAIL_SEND_RIREKI.UPDATE_DATE, string.Empty, "M/d");
                expInfoCollection.Add(Def_M_MAIL_SEND_RIREKI.ASSIGN_COMMENT, string.Empty);
                expInfoCollection.Add(Def_M_USER.USER_NAME, string.Empty);

                //expInfoCollection[0].DataType = ExportDataType.Other;

                expInfoCollection.IsHeader = false;
                expInfoCollection.IsLine = true;
                expInfoCollection.StartCol = 0;
                expInfoCollection.DataStartRow = 0;

                // Excel出力
                ret = this.PusSimpleExcel(xlsx, revDt, expInfoCollection, ref msgID, ref args);

                // データの罫線出力
                ret = this.PutSimpleExcelPutDataLineThin(xlsx, revDt, expInfoCollection, ref msgID, ref args, 0);

                // テンプレートシートの削除
                //xlsx.DelSheet(xlsx.SheetNo2(SHEET_NAME_TEMPLATE_PACKING), 1);

                xlsx.CloseBook(true);

                // Excelファイルの存在で戻り値設定
                if (ret)
                {
                    ret = File.Exists(filePath);
                }

                if (!ret)
                {
                    // Excel出力に失敗しました。
                    msgID = "A7777777001";
                }

                return true;
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

        #region 出力処理

        /// --------------------------------------------------
        /// <summary>
        /// 出力処理
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <param name="dt">出力データ</param>
        /// <param name="expInfoCollection">パラメータ</param>
        /// <param name="msgID">エラーメッセージID</param>
        /// <param name="args">エラーメッセージパラメータ</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool PusSimpleExcel(XlsxCreator xlsx, DataTable dt, ExportInfoCollection expInfoCollection, ref string msgID, ref string[] args)
        {
            ExportInfoCollection putExpInfoCollection;
            if (!this.PutSimpleExcelCreateTargetList(dt, expInfoCollection, out putExpInfoCollection, ref msgID, ref args))
            {
                return false;
            }

            // データの出力
            putExpInfoCollection.IsHeader = expInfoCollection.IsHeader;
            putExpInfoCollection.IsLine = expInfoCollection.IsLine;
            putExpInfoCollection.DataStartRow = expInfoCollection.DataStartRow;
            putExpInfoCollection.StartCol = expInfoCollection.StartCol;
            if (!this.PutSimpleExcelPutData(xlsx, dt, putExpInfoCollection, ref msgID, ref args))
            {
                return false;
            }
            return true;
        }

        #endregion

        #region データ出力

        /// --------------------------------------------------
        /// <summary>
        /// データ出力
        /// </summary>
        /// <param name="xls">XlsxCreator</param>
        /// <param name="dt">出力データ</param>
        /// <param name="expInfoCollection">エクスポート情報</param>
        /// <param name="msgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool PutSimpleExcelPutData(XlsxCreator xlsx, DataTable dt, ExportInfoCollection expInfoCollection, ref string msgID, ref string[] args)
        {
            int col = expInfoCollection.StartCol;
            int row = expInfoCollection.DataStartRow;
            foreach (DataRow dr in dt.Rows)
            {
                col = expInfoCollection.StartCol;
                foreach (var expInfo in expInfoCollection)
                {
                    if (string.IsNullOrEmpty(expInfo.DataColName)) continue;
                    if (!string.IsNullOrEmpty(expInfo.DateTimeFormat))
                    {
                        expInfo.DataType = ExportDataType.DateTime;
                    }
                    if (expInfo.DataColName == Def_M_MAIL_SEND_RIREKI.BUKKEN_REV)
                    {
                        expInfo.DataType = ExportDataType.Other;
                    }
                    switch (expInfo.DataType)
                    {
                        case ExportDataType.String:
                            //if (expInfo.DataColName == Def_M_USER.USER_NAME)
                            //{
                            //    xlsx.Pos(col + 3, row).Str = ComFunc.GetFld(dr, expInfo.DataColName);
                            //}
                            //else
                            {
                                xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, expInfo.DataColName);
                            }
                            break;
                        case ExportDataType.DateTime:
                            if (ComFunc.GetFldObject(dr, expInfo.DataColName) != DBNull.Value)
                            {
                                xlsx.Pos(col, row).Value = ComFunc.GetFldToDateTime(dr, expInfo.DataColName);
                            }
                            break;
                        case ExportDataType.Other:
                            xlsx.Pos(col, row).Value = ComFunc.GetFldObject(dr, expInfo.DataColName);
                            break;
                        default:
                            break;
                    }
                    xlsx.Pos(col, row).Attr.OverReturn = false;
                    col++;
                }
                row++;
            }
            return true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// データの罫線出力
        /// </summary>
        /// <param name="xlsx">ExcelCreator</param>
        /// <param name="dt">出力データ</param>
        /// <param name="expInfoCollection">パラメータ</param>
        /// <param name="msgID">エラーメッセージID</param>
        /// <param name="args">エラーメッセージパラメータ</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool PutSimpleExcelPutDataLine(XlsxCreator xlsx, DataTable dt, ExportInfoCollection expInfoCollection, ref string msgID, ref string[] args, int startCol)
        {
            if (!expInfoCollection.IsLine) return true;
            int colStart = startCol;
            int rowStart = expInfoCollection.DataStartRow;

            var size = xlsx.MaxData(xlMaxEndPoint.xarMaxPoint);
            int colEnd = size.Width;
            int rowEnd = size.Height;

            xlsx.Pos(colStart, rowStart, colEnd, rowEnd).Attr.Box(xlBoxType.xbtLtc, xlBorderStyle.xbsThin, xlColor.xclBlack);
            xlsx.Pos(colStart, rowStart, colEnd, rowEnd).Attr.Box(xlBoxType.xbtBox, xlBorderStyle.xbsMedium, xlColor.xclBlack);

            return true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// データの罫線出力 細線
        /// </summary>
        /// <param name="xlsx">ExcelCreator</param>
        /// <param name="dt">出力データ</param>
        /// <param name="expInfoCollection">パラメータ</param>
        /// <param name="msgID">エラーメッセージID</param>
        /// <param name="args">エラーメッセージパラメータ</param>
        /// <returns></returns>
        /// <create>T.SASAYAMA 2023/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool PutSimpleExcelPutDataLineThin(XlsxCreator xlsx, DataTable dt, ExportInfoCollection expInfoCollection, ref string msgID, ref string[] args, int startCol)
        {
            if (!expInfoCollection.IsLine) return true;
            int colStart = startCol;
            int rowStart = expInfoCollection.DataStartRow;

            var size = xlsx.MaxData(xlMaxEndPoint.xarMaxPoint);
            int colEnd = size.Width;
            int rowEnd = size.Height;

            xlsx.Pos(colStart, rowStart, colEnd, rowEnd).Attr.Box(xlBoxType.xbtLtc, xlBorderStyle.xbsThin, xlColor.xclBlack);
            xlsx.Pos(colStart, rowStart, colEnd, rowEnd).Attr.Box(xlBoxType.xbtBox, xlBorderStyle.xbsThin, xlColor.xclBlack);

            return true;
        }

        #endregion
    }
}
