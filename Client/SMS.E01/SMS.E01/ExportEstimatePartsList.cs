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
    public class ExportEstimatePartsList : BaseExport
    {
        #region 定数
        /// --------------------------------------------------
        /// <summary>
        /// データ印字開始行
        /// </summary>
        /// <create>S.Furugo 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int DATASTARTROW_PARTSLIST = 5 - 1;
        /// --------------------------------------------------
        /// <summary>
        /// データ印字開始列
        /// </summary>
        /// <create>S.Furugo 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int DATASTARTCOLMN_PARTSLIST = 0;
        /// --------------------------------------------------
        /// <summary>
        /// ヘッダーに使用される行数
        /// </summary>
        /// <create>S.Furugo 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int HEADERENDROW_PARTSLIST = 3;
        /// --------------------------------------------------
        /// <summary>
        /// 明細出力行の行の高さ
        /// </summary>
        /// <create>S.Furugo 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int ROWHEIGHT_PARTSLIST = 15;
        /// --------------------------------------------------
        /// <summary>
        /// 明細行頭番号初期
        /// </summary>
        /// <create>S.Furugo 2018/12/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int DATACELL_ROWTOP = 5;
        #endregion

        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>S.Furugo 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public ExportEstimatePartsList()
            : base()
        {
        }

        #endregion

        #region 有償支給部品Excel出力

        /// --------------------------------------------------
        /// <summary>
        /// 有償支給部品Excelの出力
        /// </summary>
        /// <param name="filePath">ファイル名</param>
        /// <param name="dt">出力データ</param>
        /// <param name="msgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>S.Furugo 2018/12/10</create>
        /// <update>D.Okumura 2019/03/11 翻訳文が適用されない問題を修正</update>
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
                        Path.Combine(ComDefine.DOWNLOAD_DIR, ComDefine.EXCEL_FILE_TEMP_ESTIMATE_PARTS));
                    if (openRet < 0)
                        return false;

                    bool ret = true;
                    var tbl = ds.Tables[Def_T_TEHAI_ESTIMATE.Name].Rows[0];
                    // シートタイトル
                    string estimateName = ComFunc.GetFld(tbl, Def_T_TEHAI_ESTIMATE.NAME, "");
                    string sheetName = string.Format(Resources.ExportEstimatePartsList_ListSheetName, estimateName);

                    // レイアウトコピー
                    this.ExportExcel_Layout(xlsx, sheetName);
                    // 集計値反映
                    this.ExportExcel_FillTemplateSum(xlsx, ds.Tables[ComDefine.DTTBL_TEHAI_ESTIMATE_SUM].Rows[0]);
                    // ヘッダ翻訳を適用
                    this.ExportExcel_FillTemplateHeader(xlsx);
                    // ヘッダ部記述
                    ret = this.ExportExcel_FillTemplate(xlsx, tbl, estimateName);
                    // DETAILに記述
                    if (ret)
                    {
                        var rows = ds.Tables[Def_T_TEHAI_MEISAI.Name].Rows;
                        xlsx.RowCopy(CurrentDataRow, CurrentDataRow);
                        xlsx.RowInsert(CurrentDataRow, rows.Count);
                        foreach (DataRow dr in ds.Tables[Def_T_TEHAI_MEISAI.Name].Rows)
                        {
                            xlsx.RowPaste(CurrentDataRow);
                            ret = this.ExportExcel_PartsList_Data(xlsx, dr, CurrentDataRow);
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
        /// <create>D.Okumura 2019/03/11</create>
        /// <update>J.Chen 2022/05/23 STEP14</update>
        /// <update>J.Jeong 2024/07/16 出荷数を発注数に変更</update>
        /// <update>J.Chen 2024/11/15 通関確認状態追加</update>
        /// --------------------------------------------------
        private void ExportExcel_FillTemplateHeader(XlsxCreator xlsx)
        {

            // 販管費on
            xlsx.Cell("Y3").Str = Resources.ExportEstimatePartsList_SellingAndGeneralExpensesEnabled;
            // 運賃梱包費on
            xlsx.Cell("AB3").Str = Resources.ExportEstimatePartsList_FarePackingCostEnabled;

            // タイトル行列位置設定
            int row = DATASTARTROW_PARTSLIST - 1;
            int col = DATASTARTCOLMN_PARTSLIST;
            // 連携No
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_TehaiRenkeiNo;
            col++;
            // 物件名
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_BukkenMei;
            col++;
            // 製番
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_ProductNumber;
            col++;
            // CODE
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_CODE;
            col++;
            // 追番
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_DrawingSerialNumber;
            col++;
            // AR No.
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_ARNo;
            col++;
            // ECS No.
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_ECSNo;
            col++;
            // Floor
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_Floor;
            col++;
            // 機種
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_Model;
            col++;
            // ST-No.
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_STNo;
            col++;
            // 品名(和名)
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_JpName;
            col++;
            // 品名
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_Name;
            col++;
            // INV 付加名
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_INVName;
            col++;
            // 図番/型式
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_DrawingNumberFormat;
            col++;
            // 出荷数
            //xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_ShukkaQty;
            // 発注数
            xlsx.Pos(col, row).Str = Resources.ExportMitsumoriMeisai_HacchuQty;
            col++;
            // Free1
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_Free1;
            col++;
            // Free2
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_Free2;
            col++;;
            // 数量単位
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_QuantityUnit;
            col++;
            // 図番/型式2
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_DrawingNumberFormat2;
            col++;
            // 備考
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_Note;
            col++;
            // 通関確認状態
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_CustomsStatus;
            col++;
            // Maker
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_Maker;
            col++;
            // 手配No
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_TehaiNo;
            col++;
            // 単価(JPY)
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_UnitPrice;
            col++;
            // 単価*販管費
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_UnitPriceSalse;
            col++;
            // 単価RMB
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_UnitPriceRmb;
            col++;
            // パーツ費 Total RMB
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_SumRmb;
            col++;
            // 運賃込 単価 RMB
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_RobUnitPriceRmb;
            col++;
            // 運賃込 Total RMB
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_RobSumRmb;
            col++;
            // PoNo
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_PoNo;
            col++;
            // 出荷便名
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_ShippingFlights;
            col++;
            // TagNo
            xlsx.Pos(col, row).Str = Resources.ExportEstimatePartsList_TagNo;
            col++;



        }

        /// --------------------------------------------------
        /// <summary>
        /// ヘッダ出力
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <param name="drHead">ヘッダ情報</param>
        /// <param name="title">タイトル情報</param>
        /// <create>S.Furugo 2018/12/11</create>
        /// <update>D.Okumura 2019/03/11 翻訳文が適用されない問題を修正</update>
        /// <update>J.Chen 2024/03/01 仕切りレート追加</update>
        /// --------------------------------------------------
        private bool ExportExcel_FillTemplate(XlsxCreator xlsx, DataRow drHead, string title)
        {

            // ヘッダーに必要情報を出力する
            // タイトル
            xlsx.Cell("**title").Str = string.Format(Resources.ExportEstimatePartsList_ListSheetTitle, title);

            // 販管費(%)
            var salesPer = ComFunc.GetFldToDecimal(drHead, Def_T_TEHAI_ESTIMATE.SALSES_PER);
            xlsx.Cell("**sales_per").Value = (salesPer + 100) / 100m;

            // ER(JPY)
            xlsx.Cell("**er_rate").Attr.Format = "######.000";
            xlsx.Cell("**er_rate").Value = ComFunc.GetFldObject(drHead, Def_T_TEHAI_ESTIMATE.RATE_JPY);

            // 通貨
            xlsx.Cell("**currency").Str = ComFunc.GetFld(drHead, ComDefine.FLD_CURRENCY_FLAG_NAME);

            // 運賃(%)
            var robPer = ComFunc.GetFldToDecimal(drHead, Def_T_TEHAI_ESTIMATE.ROB_PER);
            xlsx.Cell("**rob_rate").Value = (robPer + 100) / 100m;

            // 仕切りレート
            xlsx.Cell("**whl_rate").Attr.Format = "##,###,###,##0.000";
            xlsx.Cell("**whl_rate").Value = ComFunc.GetFldObject(drHead, Def_T_TEHAI_ESTIMATE.RATE_PARTITION);

            return true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 集計値出力
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <param name="drHead">集計情報</param>
        /// <create>S.Furugo 2018/12/11</create>
        /// <update>D.Okumura 2019/03/11 翻訳文を適用</update>
        /// <update>J.Chen 2023/12/20 仕切り金額追加</update>
        /// --------------------------------------------------
        private bool ExportExcel_FillTemplateSum(XlsxCreator xlsx, DataRow drSum)
        {

            // Parts費
            xlsx.Cell("**total_parts_cost_title").Str = Resources.ExportEstimatePartsList_PartsCost;
            xlsx.Cell("**total_parts_cost").Value = ComFunc.GetFldToDecimal(drSum, ComDefine.FLD_SUM_RMB);
            // PO金額
            xlsx.Cell("**total_po_cost_title").Str = Resources.ExportEstimatePartsList_POCost;
            xlsx.Cell("**total_po_cost").Value = ComFunc.GetFldToDecimal(drSum, ComDefine.FLD_ROB_SUM_RMB);
            // 運賃
            xlsx.Cell("**total_rob_cost_title").Str = Resources.ExportEstimatePartsList_Rob;
            xlsx.Cell("**total_rob_cost").Value = ComFunc.GetFldToDecimal(drSum, ComDefine.FLD_SUM_ROB);
            // 仕切り金額
            xlsx.Cell("**total_pamount_cost_title").Str = Resources.ExportEstimatePartsList_PAmountCost;
            xlsx.Cell("**total_pamount_cost").Value = ComFunc.GetFldToDecimal(drSum, ComDefine.FLD_PAMOUNT_SUM_RMB);
            return true;
        }


        /// --------------------------------------------------
        /// <summary>
        /// 明細出力
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <param name="drData">明細情報</param>
        /// <create>S.Furugo 2018/12/11</create>
        /// <update>J.Chen 2022/05/23 STEP14</update>
        /// <update>J.Jeong 2024/07/16 出荷数を発注数に変更</update>
        /// <update>J.Chen 2024/11/15 通関確認状態追加</update>
        /// --------------------------------------------------
        private bool ExportExcel_PartsList_Data(XlsxCreator xlsx, DataRow dr, int currentRow)
        {
            // 行列位置設定
            int row = currentRow;
            int col = DATASTARTCOLMN_PARTSLIST;

            // 連携No
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO);
            col++;

            // 物件名
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_M_PROJECT.BUKKEN_NAME);
            col++;

            // 製番
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_M_ECS.SEIBAN);
            col++;

            // CODE
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_M_ECS.CODE);
            col++;

            // 追番
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.ZUMEN_OIBAN);
            col++;

            // AR No.
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_M_ECS.AR_NO);
            col++;

            // ECS No.
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.ECS_NO);
            col++;

            // Floor
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.FLOOR);
            col++;

            // 機種
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_M_ECS.KISHU);
            col++;

            // ST-No.
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.ST_NO);
            col++;

            // 品名(和文)
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.HINMEI_JP);
            col++;

            // 品名
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.HINMEI);
            col++;

            // INV 付加名
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.HINMEI_INV);
            col++;

            // 図番/形式
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI);
            col++;

            // 出荷数
            //xlsx.Pos(col, row).Value = ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.SHUKKA_QTY);
            //col++;

            // 発注数
            if (Convert.ToInt32(ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.HACCHU_QTY)) == 0 && ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.TEHAI_FLAG) == "8")
            {
               xlsx.Pos(col, row).Value = ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.SHUKKA_QTY);
            }else{
               xlsx.Pos(col, row).Value = ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.HACCHU_QTY);
            }
            col++;

            // Free1
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.FREE1);
            col++;

            // Free2
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.FREE2);
            col++;

            // 数量単位
            xlsx.Pos(col, row).Value = ComFunc.GetFld(dr, ComDefine.FLD_QUANTITY_UNIT_NAME);
            col++;

            // 図番/形式2
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI2);
            col++;

            // 備考
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.NOTE);
            col++;

            // 通関確認状態
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.CUSTOMS_STATUS);
            col++;

            // Maker
            xlsx.Pos(col, row).Str = ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.MAKER);
            col++;

            // 手配No
            xlsx.Pos(col, row).Value = ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI_SKS.TEHAI_NO);
            col++;

            // 単価(JPY)
            xlsx.Pos(col, row).Value = ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.UNIT_PRICE);
            col++;

            // 単価*販管費
            xlsx.Pos(col, row).Value = ComFunc.GetFldObject(dr, ComDefine.FLD_UNIT_PRICE_SALSE);
            col++;

            // 単価RMB
            xlsx.Pos(col, row).Value = ComFunc.GetFldObject(dr, ComDefine.FLD_UNIT_PRICE_RMB);
            col++;

            // パーツ費 Total RMB
            xlsx.Pos(col, row).Value = ComFunc.GetFldObject(dr, ComDefine.FLD_SUM_RMB);
            col++;

            // 運賃込 単価 RMB
            xlsx.Pos(col, row).Value = ComFunc.GetFldObject(dr, ComDefine.FLD_ROB_UNIT_PRICE_RMB);
            col++;

            // 運賃込 Total RMB
            xlsx.Pos(col, row).Value = ComFunc.GetFldObject(dr, ComDefine.FLD_ROB_SUM_RMB);
            col++;

            // PoNo
            xlsx.Pos(col, row).Value = ComFunc.GetFldObject(dr, null);
            col++;

            // 出荷便名
            xlsx.Pos(col, row).Value = ComFunc.GetFldObject(dr, null);
            col++;

            // TagNo
            xlsx.Pos(col, row).Value = ComFunc.GetFldObject(dr, null);

            xlsx.Pos(DATASTARTCOLMN_PARTSLIST, row, col, row).Attr.Box(xlBoxType.xbtLtc, xlBorderStyle.xbsThin, xlColor.xclBlack);

            return true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// テンプレート用シートからレイアウトのコピー
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <create>S.Furugo 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ExportExcel_Layout(XlsxCreator xlsx, string SheetName)
        {
            // テンプレート用シートからレイアウトのコピー
            var pos = xlsx.SheetNo2(Resources.ExportEstimatePartsList_ListTemplateSheetName);
            xlsx.CopySheet(pos, 0, SheetName);
            xlsx.DelSheet(xlsx.SheetNo2(Resources.ExportEstimatePartsList_ListTemplateSheetName), 1);
            xlsx.SheetNo = xlsx.SheetNo2(SheetName);
        }

        #endregion

    }
}
