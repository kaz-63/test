using System;

using Commons;
using SystemBase.Excel;
using SMS.E01.Properties;
using XlsxCreatorHelper;
using System.IO;
using System.Data;
using System.Threading;

namespace SMS.E01
{
    /// --------------------------------------------------
    /// <summary>
    /// 配送先Excel出力クラス
    /// </summary>
    /// <create>J.Chen 2024/08/01</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ExportHaisosakiHoshu : BaseExportXlsx
    {
        #region 定数
        /// --------------------------------------------------
        /// <summary>
        /// データ印字開始行
        /// </summary>
        /// <create>J.Chen 2024/08/01</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int DATASTARTROW_PARTSLIST = 1;

        #endregion

        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>J.Chen 2024/08/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public ExportHaisosakiHoshu()
            : base()
        {
        }

        #endregion

        #region 担当者Excelの出力

        /// --------------------------------------------------
        /// <summary>
        /// 配送先Excelの出力
        /// </summary>
        /// <param name="filePath">ファイル名</param>
        /// <param name="dt">出力データ</param>
        /// <param name="msgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>J.Chen 2024/08/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool ExportExcel(string filePath, System.Data.DataSet ds)
        {
            try
            {
                string colName = string.Empty;
                string colCaption = string.Empty;
                int CurrentDataRow = DATASTARTROW_PARTSLIST;

                // xlsxファイル処理開始
                using (var xlsx = new XlsxCreator())
                {
                    var openRet = xlsx.OpenBook(filePath,
                        Path.Combine(ComDefine.DOWNLOAD_DIR, ComDefine.EXCEL_FILE_DELIVER_TEMPLATE));
                    if (openRet < 0)
                        return false;

                    bool ret = true;
                    var tbl = ds.Tables[Def_M_DELIVER.Name].Rows[0];
                    // シートタイトル
                    string deliverName = ComFunc.GetFld(tbl, Def_M_DELIVER.NAME, "");
                    string sheetName = string.Format(Resources.ExportDeliver_TemplateSheetName, deliverName);

                    // レイアウトコピー
                    this.ExportExcel_Layout(xlsx, sheetName);
                    // ヘッダ翻訳を適用
                    this.ExportExcel_FillTemplateHeader(xlsx);
                    // DETAILに記述
                    if (ret)
                    {
                        var rows = ds.Tables[Def_M_DELIVER.Name].Rows;
                        xlsx.RowCopy(CurrentDataRow, CurrentDataRow);
                        xlsx.RowInsert(CurrentDataRow, rows.Count);
                        foreach (DataRow dr in ds.Tables[Def_M_DELIVER.Name].Rows)
                        {
                            xlsx.RowPaste(CurrentDataRow);
                            ret = this.ExportExcel_Deliver_Data(xlsx, dr, CurrentDataRow);
                            CurrentDataRow++;
                        }
                        xlsx.RowDelete(CurrentDataRow, 2);
                    }

                    // xlsxファイル処理終了
                    xlsx.CloseBook(true);

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
        /// ヘッダーへ翻訳文を適用
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <create>J.Chen 2024/08/01</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ExportExcel_FillTemplateHeader(XlsxCreator xlsx)
        {
            // タイトル行列位置設定
            int row = DATASTARTROW_PARTSLIST - 1;
            int col = 0;

            // 名称
            xlsx.Pos(col, row).Str = Resources.ExportHaisosakiHoshu_Name;
            col++;
            // 住所
            xlsx.Pos(col, row).Str = Resources.ExportHaisosakiHoshu_Address;
            col++;
            // TEL
            xlsx.Pos(col, row).Str = Resources.ExportHaisosakiHoshu_Tel1;
            col++;
            // TEL(2)
            xlsx.Pos(col, row).Str = Resources.ExportHaisosakiHoshu_Tel2;
            col++;
            // FAX
            xlsx.Pos(col, row).Str = Resources.ExportHaisosakiHoshu_Fax;
            col++;
            // 出荷物
            xlsx.Pos(col, row).Str = Resources.ExportHaisosakiHoshu_ShippingItem;
            col++;
            // 出荷区分
            xlsx.Pos(col, row).Str = Resources.ExportHaisosakiHoshu_ShippingType;
            col++;
            // 出荷先担当者
            xlsx.Pos(col, row).Str = Resources.ExportHaisosakiHoshu_ShippingContact;
            col++;
            // 受取不可日通常
            xlsx.Pos(col, row).Str = Resources.ExportHaisosakiHoshu_UnavailNorm;
            col++;
            // 受取不可日正月連休
            xlsx.Pos(col, row).Str = Resources.ExportHaisosakiHoshu_UnavailNy;
            col++;
            // 受取不可日5月連休
            xlsx.Pos(col, row).Str = Resources.ExportHaisosakiHoshu_UnavailMay;
            col++;
            // 受取不可日8月連休
            xlsx.Pos(col, row).Str = Resources.ExportHaisosakiHoshu_UnavailAug;
            col++;
            // 並び順
            xlsx.Pos(col, row).Str = Resources.ExportHaisosakiHoshu_SortNo;
            col++;

        }

        /// --------------------------------------------------
        /// <summary>
        /// 明細出力
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <param name="drData">明細情報</param>
        /// <create>J.Chen 2024/08/01</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ExportExcel_Deliver_Data(XlsxCreator xlsx, DataRow dr, int currentRow)
        {
            // 行列位置設定
            int row = currentRow;
            int col = 0;


            // 名称
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_M_DELIVER.NAME);
            col++;
            // 住所
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_M_DELIVER.ADDRESS);
            col++;
            // TEL
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_M_DELIVER.TEL1);
            col++;
            // TEL(2)
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_M_DELIVER.TEL2);
            col++;
            // FAX
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_M_DELIVER.FAX);
            col++;
            // 出荷物
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_M_DELIVER.SHIPPING_ITEM);
            col++;
            // 出荷区分
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_M_DELIVER.SHIPPING_TYPE);
            col++;
            // 出荷先担当者
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_M_DELIVER.SHIPPING_CONTACT);
            col++;
            // 受取不可日通常
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_M_DELIVER.UNAVAIL_NORM);
            col++;
            // 受取不可日正月連休
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_M_DELIVER.UNAVAIL_NY);
            col++;
            // 受取不可日5月連休
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_M_DELIVER.UNAVAIL_MAY);
            col++;
            // 受取不可日8月連休
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_M_DELIVER.UNAVAIL_AUG);
            col++;
            // 並び順
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_M_DELIVER.SORT_NO);

            xlsx.Pos(0, row, col, row).Attr.Box(xlBoxType.xbtLtc, xlBorderStyle.xbsThin, xlColor.xclBlack);

            return true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// テンプレート用シートからレイアウトのコピー
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <create>J.Chen 2024/08/01</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ExportExcel_Layout(XlsxCreator xlsx, string SheetName)
        {
            // テンプレート用シートからレイアウトのコピー
            var pos = xlsx.SheetNo2(Resources.ExportDeliver_TemplateSheetName);
            xlsx.CopySheet(pos, 0, SheetName);
            xlsx.DelSheet(xlsx.SheetNo2(Resources.ExportDeliver_TemplateSheetName), 1);
            xlsx.SheetNo = xlsx.SheetNo2(SheetName);
        }

        #endregion
    }
}
