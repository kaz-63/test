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
    /// パーツ便出荷まとめExcel出力クラス
    /// </summary>
    /// <create>T.Nakata 2018/12/21</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ExportPartsMatomeList : BaseExport
    {
        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>H.Tajimi 2018/11/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public ExportPartsMatomeList()
            : base()
        {
        }

        #endregion

        #region ExportExcel

        /// --------------------------------------------------
        /// <summary>
        /// ShippingDocumentExcelExcelの出力
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="dt"></param>
        /// <param name="msgID"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/12/04</create>
        /// <update>J.Chen 2022/12/21 TAG便名追加</update>
        /// <update></update>
        /// --------------------------------------------------
        public bool ExportExcel(string FilePath, DataSet ds, out string msgID, out string[] args)
        {
            msgID = string.Empty;
            args = null;
            try
            {
                // ディレクトリ作成
                try
                {
                    var dirPath = Path.GetDirectoryName(FilePath);
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
                    if (File.Exists(FilePath))
                    {
                        UtilFile.DeleteFile(FilePath);
                    }
                }
                catch{}

                bool ret = true;
                var xlsx = new XlsxCreator();

                // Excelオープン
                if (xlsx.OpenBook(FilePath, Path.Combine(ComDefine.DOWNLOAD_DIR, ComDefine.EXCEL_FILE_TEMP_SUMMARY_OF_PARTSSHIPMENT)) < 0)
                {
                    // Excel出力に失敗しました。
                    msgID = "A7777777001";
                    return false;
                }

                DataTable dtDataD = ds.Tables[ComDefine.DTTBL_SIPPING_D];
                dtDataD.Columns.Add("DUMMY1");

                int sumval = 0;
                foreach (DataRow dr in dtDataD.Rows)
                {
                    string tmpval = ComFunc.GetFld(dr, ComDefine.FLD_PRICE);
                    int tmpdata = (tmpval == string.Empty ? 0 : int.Parse(tmpval));
                    sumval += tmpdata;
                }

                // 最終更新日
                xlsx.Cell("**today").Value = DateTime.Now;
                // タイトル
                xlsx.Cell("**title").Str = string.Format(Resources.ExportPartsMatomeList_Title, ComFunc.GetFld(dtDataD, 0, Def_M_NONYUSAKI.NONYUSAKI_NAME));
                // INVOICE VALUE
                xlsx.Cell("**sum").Str = sumval.ToString();

                var expInfoCollection = new ExportInfoCollection();

                // 便名
                expInfoCollection.Add(Def_M_NONYUSAKI.SHIP, string.Empty);
                // 出荷日
                expInfoCollection.Add(Def_T_PACKING.SYUKKA_DATE, string.Empty, "M/d");
                // Invoice No
                expInfoCollection.Add(Def_T_PACKING.INVOICE_NO, string.Empty);
                // 出荷先
                expInfoCollection.Add(Def_T_PACKING_MEISAI.ATTN, string.Empty);
                // Tag No.
                expInfoCollection.Add(Def_T_SHUKKA_MEISAI.TAG_NO, string.Empty);
                // 製番
                expInfoCollection.Add(Def_T_SHUKKA_MEISAI.SEIBAN, string.Empty);
                // CODE
                expInfoCollection.Add(Def_T_SHUKKA_MEISAI.CODE, string.Empty);
                // 図面追番
                expInfoCollection.Add(Def_T_SHUKKA_MEISAI.ZUMEN_OIBAN, string.Empty);
                // 納入先
                expInfoCollection.Add(Def_M_NONYUSAKI.NONYUSAKI_NAME, string.Empty);
                // 出荷便
                expInfoCollection.Add(Def_M_NONYUSAKI.SHIP, string.Empty);
                // Area
                expInfoCollection.Add(Def_T_SHUKKA_MEISAI.AREA, string.Empty);
                // Floor
                expInfoCollection.Add(Def_T_SHUKKA_MEISAI.FLOOR, string.Empty);
                // 機種
                expInfoCollection.Add(Def_T_SHUKKA_MEISAI.KISHU, string.Empty);
                // ST No.
                expInfoCollection.Add(Def_T_SHUKKA_MEISAI.ST_NO, string.Empty);
                // 品名(和文）
                expInfoCollection.Add(Def_T_SHUKKA_MEISAI.HINMEI_JP, string.Empty);
                // 品名
                expInfoCollection.Add(Def_T_SHUKKA_MEISAI.HINMEI, string.Empty);
                // 図番/形式
                expInfoCollection.Add(Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI, string.Empty);
                // 区割　No.
                expInfoCollection.Add(Def_T_SHUKKA_MEISAI.KUWARI_NO, string.Empty);
                // 数量
                expInfoCollection.Add(Def_T_SHUKKA_MEISAI.NUM, string.Empty);
                // 取付場所
                expInfoCollection.Add("DUMMY1", string.Empty);
                // Box No.
                expInfoCollection.Add(Def_T_SHUKKA_MEISAI.BOX_NO, string.Empty);
                // Pallet No.
                expInfoCollection.Add(Def_T_SHUKKA_MEISAI.PALLET_NO, string.Empty);
                // 金額
                expInfoCollection.Add(ComDefine.FLD_PRICE, string.Empty);
                // Inv付加名
                expInfoCollection.Add(Def_T_TEHAI_MEISAI.HINMEI_INV, string.Empty);
                // TAG便名
                expInfoCollection.Add(Def_T_SHUKKA_MEISAI.TAG_SHIP, string.Empty);

                expInfoCollection.IsHeader = false;
                expInfoCollection.IsLine = true;
                expInfoCollection.StartCol = 0;
                expInfoCollection.DataStartRow = 3;

                // 行調整
                const string copysrc = "A4:Y4";
                for (int i = 1; i < dtDataD.Rows.Count; i++)
                {
                    xlsx.Cell(copysrc).Copy("A"+(4+i).ToString());
                }

                // Excel出力
                ret = this.PusSimpleExcel(xlsx, dtDataD, expInfoCollection, ref msgID, ref args);

                xlsx.CloseBook(true);

                // Excelファイルの存在で戻り値設定
                if (ret)
                {
                    if (!File.Exists(FilePath))
                    {   // Excel出力に失敗しました。
                        msgID = "A7777777001";
                    }
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
        /// <create>T.Nakata 2018/12/21</create>
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
        /// <create>T.Nakata 2018/12/21</create>
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
                    switch (expInfo.DataType)
                    {
                        case ExportDataType.String:
                            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, expInfo.DataColName);
                            break;
                        case ExportDataType.DateTime:
                            if (ComFunc.GetFldObject(dr, expInfo.DataColName) != DBNull.Value)
                            {
                                xlsx.Pos(col, row).Value = ComFunc.GetFldToDateTime(dr, expInfo.DataColName).ToString(expInfo.DateTimeFormat);
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
        /// <create>T.Nakata 2018/12/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool PutSimpleExcelPutDataLine(XlsxCreator xlsx, DataTable dt, ExportInfoCollection expInfoCollection, xlBorderStyle StyleLtc, xlBorderStyle StyleBox, ref string msgID, ref string[] args)
        {
            if (!expInfoCollection.IsLine) return true;
            int colStart = expInfoCollection.StartCol;
            int rowStart = expInfoCollection.DataStartRow;

            var size = xlsx.MaxData(xlMaxEndPoint.xarMaxPoint);
            int colEnd = size.Width;
            int rowEnd = size.Height;

            xlsx.Pos(colStart, rowStart, colEnd, rowEnd).Attr.Box(xlBoxType.xbtLtc, StyleLtc, xlColor.xclBlack);
            xlsx.Pos(colStart, rowStart, colEnd, rowEnd).Attr.Box(xlBoxType.xbtBox, StyleBox, xlColor.xclBlack);

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
        /// <create>T.Nakata 2018/12/21</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual bool PutSimpleExcelPutHeader(XlsxCreator xlsx, DataTable dt, ExportInfoCollection expInfoCollection, ref string msgID, ref string[] args)
        {
            if (!expInfoCollection.IsHeader) return true;
            int col = expInfoCollection.StartCol;
            int row = expInfoCollection.HeaderStartRow;
            foreach (var expInfo in expInfoCollection)
            {
                xlsx.Pos(col, row).Value = expInfo.Caption;
                xlsx.Pos(col, row).Attr.PosHorz = xlPosHorz.xphCenter;
                col++;
            }
            return true;
        }

        #endregion
    }
}
