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
    /// パーツ便出荷物量実績Excel出力クラス
    /// </summary>
    /// <create>T.Nakata 2018/12/21</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ExportPartsJissekiList : BaseExport
    {
        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>H.Tajimi 2018/11/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public ExportPartsJissekiList()
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
        /// <update>J.Chen 2023/01/04 Inv付加名追加</update>
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
                if (xlsx.OpenBook(FilePath, Path.Combine(ComDefine.DOWNLOAD_DIR, ComDefine.EXCEL_FILE_TEMP_QUANTITY_OF_PARTSSHIPMENT)) < 0)
                {
                    // Excel出力に失敗しました。
                    msgID = "A7777777001";
                    return false;
                }

                DataTable dtDataE = ds.Tables[ComDefine.DTTBL_SIPPING_E];

                var expInfoCollection = new ExportInfoCollection();

                // Project名
                expInfoCollection.Add(Def_M_NONYUSAKI.NONYUSAKI_NAME, string.Empty);
                // 便名
                expInfoCollection.Add(Def_M_NONYUSAKI.SHIP, string.Empty);
                // 有償or無償
                expInfoCollection.Add(ComDefine.FLD_DISP_ESTIMATE_FLAG, string.Empty);
                // AIR SHIP
                expInfoCollection.Add(Def_M_NONYUSAKI.TRANSPORT_FLAG, string.Empty);
                // 出荷元
                expInfoCollection.Add(Def_M_NONYUSAKI.SHIP_FROM, string.Empty);
                // 出荷先
                expInfoCollection.Add(Def_M_NONYUSAKI.SHIP_TO, string.Empty);
                // 出荷日
                expInfoCollection.Add(Def_M_NONYUSAKI.SHIP_DATE, string.Empty);
                // ｶｰﾄﾝ No.
                expInfoCollection.Add(Def_T_PACKING_MEISAI.CT_NO, string.Empty);
                // 種類
                expInfoCollection.Add(ComDefine.FLD_DISP_PL_TYPE, string.Empty);
                // 寸法(LxWxH)
                expInfoCollection.Add(ComDefine.FLD_SUNPO, string.Empty);
                // G/W
                expInfoCollection.Add(Def_T_PACKING_MEISAI.GRWT, string.Empty);
                // L
                expInfoCollection.Add(Def_T_PACKING_MEISAI.SIZE_L, string.Empty);
                // W
                expInfoCollection.Add(Def_T_PACKING_MEISAI.SIZE_W, string.Empty);
                // H
                expInfoCollection.Add(Def_T_PACKING_MEISAI.SIZE_H, string.Empty);
                // M3
                expInfoCollection.Add(ComDefine.FLD_M3, string.Empty);
                // 乙仲
                expInfoCollection.Add(ComDefine.FLD_OTUNAKA, string.Empty);
                // Inv付加名
                expInfoCollection.Add(Def_T_TEHAI_MEISAI.HINMEI_INV, string.Empty);
                // TAG便名
                expInfoCollection.Add(Def_T_SHUKKA_MEISAI.TAG_SHIP, string.Empty);

                expInfoCollection.IsHeader = false;
                expInfoCollection.IsLine = true;
                expInfoCollection.StartCol = 0;
                expInfoCollection.DataStartRow = 5;

                // 行調整
                xlsx.RowInsert(expInfoCollection.DataStartRow + 1, dtDataE.Rows.Count - 1);
                for (int i = 1; i < dtDataE.Rows.Count; i++)
                {
                    xlsx.RowCopy(expInfoCollection.DataStartRow, i + expInfoCollection.DataStartRow);
                }

                // Excel出力
                ret = this.PusSimpleExcel(xlsx, dtDataE, expInfoCollection, ref msgID, ref args);

                // データの罫線出力
                ret = this.PutSimpleExcelPutDataLine(xlsx, dtDataE, expInfoCollection,
                                                     xlBorderStyle.xbsThin, xlBorderStyle.xbsThin, ref msgID, ref args);

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
