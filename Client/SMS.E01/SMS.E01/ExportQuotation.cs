using System;

using Commons;
using SystemBase.Excel;
using SMS.E01.Properties;
using XlsxCreatorHelper;
using System.IO;
using System.Data;
using System.Threading;
using Microsoft.Office.Interop.Excel;
using DSWUtil;

namespace SMS.E01
{
    public class ExportQuotation : BaseExport
    {
        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>J.Chen 2024/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public ExportQuotation()
            : base()
        {
        }

        #endregion

        #region 見積書Excel出力

        /// --------------------------------------------------
        /// <summary>
        /// 見積書Excelの出力
        /// </summary>
        /// <param name="filePath">ファイル名</param>
        /// <param name="dt">出力データ</param>
        /// <param name="msgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>J.Chen 2024/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool ExportExcel(string filePath, System.Data.DataSet ds)
        {
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

                // xlsxファイル処理開始
                using (var xlsx = new XlsxCreator())
                {
                    var openRet = xlsx.OpenBook(filePath,
                        Path.Combine(ComDefine.DOWNLOAD_DIR, ComDefine.EXCEL_FILE_QUOTATION_TEMPLATE));
                    if (openRet < 0)
                        return false;

                    // シートタイトル
                    string sheetName = Resources.ExportQuotation_SheetNameQuotation;

                    // レイアウトコピー
                    this.ExportExcel_Layout(xlsx, sheetName);
                    // 見積書データ反映
                    this.ExportExcel_FillTemplateData(xlsx, ds.Tables[ComDefine.DTTBL_QUOTATION_TABLE].Rows[0]);

                    // xlsxファイル処理終了
                    xlsx.CloseBook(true);

                }

                // テンプレートExcelのRemarks情報を書き換える
                ReplaceForRemarks(filePath, ds.Tables[ComDefine.DTTBL_QUOTATION_TABLE].Rows[0]);

                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// データ代入
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <param name="drHead">データ情報</param>
        /// <create>J.Chen 2024/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ExportExcel_FillTemplateData(XlsxCreator xlsx, DataRow drEstimate)
        {
            string dataTmp = "";


            // 見積No
            dataTmp = xlsx.Cell("I9").Str;
            xlsx.Cell("I9").Str = dataTmp.Replace("**estimateNo", ComFunc.GetFld(drEstimate, Def_T_TEHAI_ESTIMATE.ESTIMATE_NO));

            // 荷受先
            xlsx.Cell("**niukesaki").Str = ComFunc.GetFld(drEstimate, ComDefine.FLD_CONSIGN_NAME);

            // 見積タイトル
            dataTmp = xlsx.Cell("A12").Str;
            dataTmp = dataTmp.Replace("**niukesaki", ComFunc.GetFld(drEstimate, ComDefine.FLD_CONSIGN_NAME));
            dataTmp = dataTmp.Replace("**estimateTitle", ComFunc.GetFld(drEstimate, Def_T_TEHAI_ESTIMATE.NAME));
            xlsx.Cell("A12").Str = dataTmp;

            dataTmp = xlsx.Cell("A15").Str;
            dataTmp = dataTmp.Replace("**niukesaki", ComFunc.GetFld(drEstimate, ComDefine.FLD_CONSIGN_NAME));
            dataTmp = dataTmp.Replace("**estimateTitle", ComFunc.GetFld(drEstimate, Def_T_TEHAI_ESTIMATE.NAME));
            xlsx.Cell("A15").Str = dataTmp;

            //dataTmp = xlsx.Cell("B35").Str;
            //dataTmp = dataTmp.Replace("**niukesaki", ComFunc.GetFld(drEstimate, ComDefine.FLD_CONSIGN_NAME));
            //dataTmp = dataTmp.Replace("**estimateTitle", ComFunc.GetFld(drEstimate, Def_T_TEHAI_ESTIMATE.NAME));
            //xlsx.Cell("B35").Str = dataTmp;

            // レート
            xlsx.Cell("**rateJPY").Value = ComFunc.GetFldObject(drEstimate, Def_T_TEHAI_ESTIMATE.RATE_JPY);

            // 通貨
            xlsx.Cell("**currency").Str = ComFunc.GetFld(drEstimate, ComDefine.FLD_CURRENCY_FLAG_NAME);
            dataTmp = xlsx.Cell("I23").Str;
            xlsx.Cell("I23").Str = dataTmp.Replace("**currency", ComFunc.GetFld(drEstimate, ComDefine.FLD_CURRENCY_FLAG_NAME));
            //dataTmp = xlsx.Cell("B34").Str;
            //xlsx.Cell("B34").Str = dataTmp.Replace("**currency", ComFunc.GetFld(drEstimate, ComDefine.FLD_CURRENCY_FLAG_NAME));

            // 発行日
            xlsx.Cell("**issueDate").Value = ComFunc.GetFld(drEstimate, ComDefine.FLD_ISSUE_DATE);

            // 発行年月
            dataTmp = xlsx.Cell("H18").Str;
            xlsx.Cell("H18").Str = dataTmp.Replace("**issueYearMonth", ComFunc.GetFld(drEstimate, ComDefine.FLD_ISSUE_YEARMONTH));

            // Parts費
            xlsx.Cell("**partsCost").Value = ComFunc.GetFldToDecimal(drEstimate, ComDefine.FLD_SUM_RMB);

            // 運賃
            xlsx.Cell("**rob").Value = ComFunc.GetFldToDecimal(drEstimate, ComDefine.FLD_SUM_ROB);

            // PO金額
            xlsx.Cell("**total_po_cost").Value = ComFunc.GetFldToDecimal(drEstimate, ComDefine.FLD_ROB_SUM_RMB);

            //// 出荷国
            //dataTmp = xlsx.Cell("B39").Str;
            //xlsx.Cell("B39").Str = dataTmp.Replace("**shippingLocation", ComFunc.GetFld(drEstimate, ComDefine.FLD_SHIPPING_LOCATION));

            return true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// テンプレート用シートからレイアウトのコピー
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <create>J.Chen 2024/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ExportExcel_Layout(XlsxCreator xlsx, string SheetName)
        {
            // テンプレート用シートからレイアウトのコピー
            var pos = xlsx.SheetNo2(Resources.ExportQuotation_SheetNameQuotation);
            xlsx.CopySheet(pos, 0, SheetName);
            xlsx.DelSheet(xlsx.SheetNo2(Resources.ExportQuotation_SheetNameQuotation), 1);
            xlsx.SheetNo = xlsx.SheetNo2(SheetName);
        }

        #endregion

        #region 見積書ExcelをPDFへ変換

        /// --------------------------------------------------
        /// <summary>
        /// 見積書ExcelをPDFへ変換
        /// </summary>
        /// <param name="excelFilePath">Excelファイルパス</param>
        /// <create>J.Chen 2024/01/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool ConvertExcelToPdf(string excelFilePath)
        {
            // PDFファイルパスへ変換
            string pdfFilePath = Path.ChangeExtension(excelFilePath, "pdf");

            Application excelApp = null;

            try
            {
                // ディレクトリ作成
                try
                {
                    var dirPath = Path.GetDirectoryName(pdfFilePath);
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
                    if (File.Exists(pdfFilePath))
                    {
                        UtilFile.DeleteFile(pdfFilePath);
                    }
                }
                catch { }

                // Excelアプリケーションを起動
                excelApp = new Application();

                if (excelApp == null)
                {
                    // エクセルがインストールされていない場合、falseを返す
                    return false;
                }

                // Excelファイルを開く
                Workbook workbook = excelApp.Workbooks.Open(excelFilePath, Type.Missing, Type.Missing,
                                                                Type.Missing, Type.Missing, Type.Missing,
                                                                Type.Missing, Type.Missing, Type.Missing,
                                                                Type.Missing, Type.Missing, Type.Missing,
                                                                Type.Missing, Type.Missing, Type.Missing);

                // PDFに変換して保存
                workbook.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, pdfFilePath,
                                            XlFixedFormatQuality.xlQualityStandard, Type.Missing,
                                            Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                            Type.Missing);

                // Excelを閉じる
                workbook.Close(false, Type.Missing, Type.Missing);
            }
            catch
            {
                // エラーが発生した場合、falseを返す
                return false;
            }
            finally
            {
                // Excelアプリケーションを終了し、オブジェクトの解放
                if (excelApp != null)
                {
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                }
            }

            // 成功時にtrueを返す
            return true;
        }

        #endregion

        #region REMARKS情報変換

        /// --------------------------------------------------
        /// <summary>
        /// REMARKS情報変換
        /// </summary>
        /// <param name="drHead">データ情報</param>
        /// <param name="excelFilePath">Excelファイルパス</param>
        /// <create>J.Chen 2024/02/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool ReplaceForRemarks(string excelFilePath, DataRow drEstimate)
        {

            Application excelApp = null;

            try
            {

                // Excelアプリケーションを起動
                excelApp = new Application();

                if (excelApp == null)
                {
                    // エクセルがインストールされていない場合、falseを返す
                    return false;
                }

                // Excelファイルを開く
                Workbook workbook = excelApp.Workbooks.Open(excelFilePath, Type.Missing, Type.Missing,
                                                                Type.Missing, Type.Missing, Type.Missing,
                                                                Type.Missing, Type.Missing, Type.Missing,
                                                                Type.Missing, Type.Missing, Type.Missing,
                                                                Type.Missing, Type.Missing, Type.Missing);

                // ワークシートを取得（例: シート1
                Worksheet worksheet = (Worksheet)workbook.Sheets[1];

                // テンプレートExcelで使用されたテキストボックスの名称（変更した場合、こちらも修正が必要）
                Shape textBox = worksheet.Shapes.Item("TextBox 3");

                // テキストボックスのテキストを取得
                string currentText = textBox.TextFrame.Characters(Type.Missing, Type.Missing).Text;

                // 通貨
                currentText = currentText.Replace("**currency", ComFunc.GetFld(drEstimate, ComDefine.FLD_CURRENCY_FLAG_NAME));

                // 見積タイトル
                currentText = currentText.Replace("**niukesaki", ComFunc.GetFld(drEstimate, ComDefine.FLD_CONSIGN_NAME));
                currentText = currentText.Replace("**estimateTitle", ComFunc.GetFld(drEstimate, Def_T_TEHAI_ESTIMATE.NAME));

                // 出荷国
                currentText = currentText.Replace("**shippingLocation", ComFunc.GetFld(drEstimate, ComDefine.FLD_SHIPPING_LOCATION));

                // テキストボックスに代入
                textBox.TextFrame.Characters(Type.Missing, Type.Missing).Text = currentText;

                // Excelを保存して、閉じる
                workbook.Close(true, Type.Missing, Type.Missing);
            }
            catch
            {
                // エラーが発生した場合、falseを返す
                return false;
            }
            finally
            {
                // Excelアプリケーションを終了し、オブジェクトの解放
                if (excelApp != null)
                {
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                }
            }

            // 成功時にtrueを返す
            return true;
        }

        #endregion

    }
}
