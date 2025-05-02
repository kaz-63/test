using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using Commons;
using DSWUtil;
using SMS.E01.Properties;
using SystemBase.Excel;
using XlsxCreatorHelper;

namespace SMS.E01
{
    /// --------------------------------------------------
    /// <summary>
    /// ShippingDocumentExcel出力クラス
    /// </summary>
    /// <create>T.Nakata 2018/12/21</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ExportShippingDocument : BaseExport
    {
        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// INVOICE:SHAPE用変数
        /// </summary>
        /// <create>T.Nakata 2018/12/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string VAL_VISIBLE = "**attached_sheet_row_visible";
        /// --------------------------------------------------
        /// <summary>
        /// INVOICE:表示調整用変数
        /// </summary>
        /// <create>T.Nakata 2018/12/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string VAL_HIDDEN = "**attached_sheet_row_hidden";
        /// --------------------------------------------------
        /// <summary>
        /// Attachedeシート(無償):開始行
        /// </summary>
        /// <create>T.Nakata 2018/12/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private int DATA_START_ROW_ATTACHED1 = 4;//0オリジン
        /// --------------------------------------------------
        /// <summary>
        /// Attachedeシート(有償):開始行
        /// </summary>
        /// <create>T.Nakata 2018/12/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private int DATA_START_ROW_ATTACHED2 = 3;//0オリジン
        /// --------------------------------------------------
        /// <summary>
        /// SETTING KEY Ref Suffix
        /// </summary>
        /// <create>M.Shimizu 2020/08/25 ShippingDocumentのExcel改修</create>
        /// <update></update>
        /// --------------------------------------------------
        private static readonly string SETTING_KEY_REF_SUFFIX = "Ref Suffix";
        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>H.Tajimi 2018/11/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public ExportShippingDocument()
            : base()
        {
        }

        #endregion

        #region ExportExcel

        /// --------------------------------------------------
        /// <summary>
        /// ShippingDocumentExcelExcelの出力
        /// </summary>
        /// <param name="filePath">出力ファイル</param>
        /// <param name="ds">出力データ</param>
        /// <param name="isKokugai">国外/国内</param>
        /// <param name="msgID">エラーメッセージID</param>
        /// <param name="args">エラーメッセージパラメータ</param>
        /// <returns>true:成功/false:エラー</returns>
        /// <create>T.Nakata 2018/12/21</create>
        /// <update>M.Shimizu 2020/08/25 ShippingDocumentのExcel改修</update>
        /// <update>D.Okumura 2020/10/02 EFA_SMS-138 有償版 Shipping Document の現地通貨略称表示領域対応</update>
        /// <update>D.Okumura 2021/01/15 EFA_SMS-184 複数PO No対応</update>
        /// <update>J.Chen 2022/11/21 無償版 SHIPデータ出力追加</update>
        /// --------------------------------------------------
        public bool ExportExcel(string filePath, DataSet ds, bool isKokugai, out string msgID, out string[] args)
        {
            msgID = string.Empty;
            args = null;

            try
            {
                if (filePath.Length < 3)
                {
                    // Excel出力に失敗しました。
                    msgID = "A7777777001";
                    return false;
                }

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

                var ret = true;
                var xlsx = new XlsxCreator();

                // 共通使用テーブル
                var dtShukka = ds.Tables[ComDefine.DTTBL_SIPPING_H];   // 出荷明細3

                // Excelオープン
                if (xlsx.OpenBook(filePath, Path.Combine(ComDefine.DOWNLOAD_DIR, ComDefine.EXCEL_FILE_TEMP_INVOICE)) < 0)
                {
                    // Excel出力に失敗しました。
                    msgID = "A7777777001";
                    return false;
                }

                if (isKokugai)
                {
                    // 国外のみ使用
                    var dtInvoice = ds.Tables[ComDefine.DTTBL_SIPPING_A];       // 帳票関連
                    var dtConsign = ds.Tables[ComDefine.DTTBL_SIPPING_B];       // 荷受マスタ
                    var dtPacking = ds.Tables[ComDefine.DTTBL_SIPPING_E];       // 荷姿明細
                    var dtAtached = ds.Tables[ComDefine.DTTBL_SIPPING_F];       // 出荷明細2
                    var dtUnsokaisha = ds.Tables[ComDefine.DTTBL_SIPPING_G];    // 運送会社マスタ
                    var dtTotalEveryPoNo = ds.Tables[ComDefine.DTTBL_SIPPING_I]; // PONo 一覧

                    // 1行しか無いのでROWを出す
                    var drInvoice = dtInvoice.Rows[0];
                    var drConsignee = dtConsign.Rows[0];
                    var drUnsokaisha = dtUnsokaisha.Rows[0];

                    // ===== INVOICE =====
                    var isInvoice = false;
                    var isPacking = ComFunc.GetFld(drUnsokaisha, Def_M_UNSOKAISHA.PACKINGLIST_FLAG) == UNSO_PACKINGLIST_FLAG.NECESSARY_VALUE1;
                    var isAttached = false;
                    var estimateFlag = ComFunc.GetFld(drInvoice, Def_M_NONYUSAKI.ESTIMATE_FLAG);
                    estimateFlag = string.IsNullOrEmpty(estimateFlag) ? ESTIMATE_FLAG.GRATIS_VALUE1 : estimateFlag;
                    var isGratis = (estimateFlag == ESTIMATE_FLAG.GRATIS_VALUE1);
                    
                    if (isGratis)
                    {
                        if (dtAtached.Rows.Count > 20)
                        {
                            isAttached = true;
                        }
                        // ARNoない場合、SHIPデータを出力します
                        if (string.IsNullOrEmpty(ComFunc.GetFld(dtAtached.Rows[0], ComDefine.FLD_LIST_AR_NO).Trim()))
                        {
                            dtAtached.AsEnumerable().Select(r => r[ComDefine.FLD_LIST_AR_NO] = ComFunc.GetFld(dtPacking.Rows[0], Def_M_NONYUSAKI.SHIP)).ToList();
                        }
                    }
                    else
                    {
                        isAttached = true;
                    }

                    // --- INVOICE ---
                    if (ComFunc.GetFld(drUnsokaisha, Def_M_UNSOKAISHA.INVOICE_FLAG) == UNSO_INVOICE_FLAG.NECESSARY_VALUE1)
                    {
                        isInvoice = true;

                        ret = ret & this.ExportExcelInvoice(xlsx, drInvoice, dtPacking, dtAtached, dtTotalEveryPoNo,
                                                            isGratis, isAttached, isPacking, ref msgID, ref args);

                        if (isAttached)
                        {
                            ret = ret & this.ExportExcelAttached(xlsx, dtAtached, isGratis, drInvoice, 
                                                                 ref msgID, ref args);
                        }
                    }
                    else
                    {   // 不要シート削除
                        this.DelSheet(xlsx, Resources.ExportShippingDocument_SheetNameOnerousInvoice);          // 有償Invoice
                        this.DelSheet(xlsx, Resources.ExportShippingDocument_SheetNameGratisInvoice);           // 無償Invoice
                        this.DelSheet(xlsx, Resources.ExportShippingDocument_SheetNameGratisInvoiceAttached);   // 無償Attached
                    }

                    if (isGratis || (!isInvoice && !isPacking))
                    {   // 不要シート削除
                        this.DelSheet(xlsx, Resources.ExportShippingDocument_SheetNameOnerousAttached);         // 有償Attached
                    }

                    // --- 輸出通関確認書 ---
                    if (ComFunc.GetFld(drUnsokaisha, Def_M_UNSOKAISHA.EXPORTCONFIRM_FLAG) == UNSO_EXPORTCONFIRM_FLAG.NECESSARY_VALUE1)
                    {
                        ret = ret & this.ExportExcelExportConfirm(xlsx, drInvoice, drUnsokaisha);
                    }
                    else
                    {   // 不要シート削除
                        this.DelSheet(xlsx, Resources.ExportShippingDocument_SheetNameExportConfirmation);
                    }

                    // --- 中国輸入時記載事項 ---
                    if (ComFunc.GetFld(drConsignee, Def_M_CONSIGN.CHINA_FLAG) == CHINA_FLAG.ON_VALUE1)
                    {
                        // 中国輸入時記載事項のシートが存在しない場合でもエラーとしない
                        if (!this.ExportExcelImportDescriptionChina(xlsx, drConsignee))
                        {
                            // 不要シート削除
                            this.DelSheet(xlsx, Resources.ExportShippingDocument_SheetNameImportDescriptionChina);
                        }
                    }
                    else
                    {   // 不要シート削除
                        this.DelSheet(xlsx, Resources.ExportShippingDocument_SheetNameImportDescriptionChina);
                    }
                }
                else
                {   // 不要シート削除
                    this.DelSheet(xlsx, Resources.ExportShippingDocument_SheetNameGratisInvoice);           // 無償Invoice
                    this.DelSheet(xlsx, Resources.ExportShippingDocument_SheetNameGratisInvoiceAttached);   // 無償InvoiceAttached
                    this.DelSheet(xlsx, Resources.ExportShippingDocument_SheetNameOnerousInvoice);          // 有償Invoice
                    this.DelSheet(xlsx, Resources.ExportShippingDocument_SheetNameOnerousAttached);         // 有償Attached
                    this.DelSheet(xlsx, Resources.ExportShippingDocument_SheetNameImportDescriptionChina);  // 中国輸入時記載事項
                    this.DelSheet(xlsx, Resources.ExportShippingDocument_SheetNameExportConfirmation);      // 輸出通関確認書
                }

                // --- 出荷明細 ---
                ret = ret & this.ExportExcelShukkaMeisai(xlsx, dtShukka, ref msgID, ref args);

                this.DelSheet(xlsx, "settings");
                xlsx.CloseBook(true);

                // Excelファイルの存在で戻り値設定
                if (ret)
                {
                    if (!File.Exists(filePath))
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

        #region INVOICE:シート処理

        /// --------------------------------------------------
        /// <summary>
        /// 無償/有償INVOICEシート
        /// </summary>
        /// <param name="xlsx">XLSXオブジェクト</param>
        /// <param name="drInvoice">Invoiceデータ</param>
        /// <param name="dtPacking">Packingデータ</param>
        /// <param name="dtTotalEveryPoNo">PO#ごとの集計データ</param>
        /// <param name="dtAtached">Atachedデータ</param>
        /// <param name="isGratis">無償/有償</param>
        /// <param name="isAttached">Attached有無</param>
        /// <param name="isPacking">Packing有無</param>
        /// <param name="msgID">エラー発生時のメッセージID</param>
        /// <param name="args">メッセージ置換文字列</param>
        /// <returns>true:成功/false:エラー</returns>
        /// <create>T.Nakata 2018/12/21</create>
        /// <update>M.Shimizu 2020/05/28 EFA_SMS-80 対応（無償Packing Listに「PC/PCS」「N/W」を設定）</update>
        /// <update>M.Shimizu 2020/08/25 ShippingDocumentのExcel改修</update>
        /// <update>D.Okumura 2021/01/15 EFA_SMS-184 複数PO No対応</update>
        /// <update>H.Iimuro 2022/10/07 ShippingDocumentのExcel改修</update>
        /// --------------------------------------------------
        private bool ExportExcelInvoice(XlsxCreator xlsx, DataRow drInvoice, DataTable dtPacking, DataTable dtAtached, 
                DataTable dtTotalEveryPoNo, bool isGratis, bool isAttached, bool isPacking, ref string msgID, ref string[] args)
        {
            var sheetName = string.Empty;
            var sheetNameSave = string.Empty;
            var refSuffix = GetSettingData(xlsx, SETTING_KEY_REF_SUFFIX);

            if (isGratis)
            {
                sheetName = Resources.ExportShippingDocument_SheetNameGratisInvoice;
                sheetNameSave = Resources.ExportShippingDocument_SheetName_G_INVOICE;
            }
            else
            {
                sheetName = Resources.ExportShippingDocument_SheetNameOnerousInvoice;
                sheetNameSave = Resources.ExportShippingDocument_SheetName_O_INVOICE;
            }

            // シートの設定
            xlsx.SheetNo = xlsx.SheetNo2(sheetName);
            xlsx.SheetName = sheetNameSave;

            // CONSIGNED TO
            xlsx.Cell("**" + ComDefine.FLD_CONSIGNED_TO).Str = ComFunc.GetFld(drInvoice, ComDefine.FLD_CONSIGNED_TO);
            // INVOICE NO.
            xlsx.Cell("**" + ComDefine.FLD_INVOICE_NO).Str = ComFunc.GetFld(drInvoice, ComDefine.FLD_INVOICE_NO);
            // DATE
            xlsx.Cell("**" + ComDefine.FLD_DATE).Value = ComFunc.GetFldToDateTime(drInvoice, ComDefine.FLD_DATE);
            // PARTS FOR									
            xlsx.Cell("**" + ComDefine.FLD_PARTS_FOR).Str = ComFunc.GetFld(drInvoice, ComDefine.FLD_PARTS_FOR);
            // REF.
            xlsx.Cell("**" + ComDefine.FLD_REF).Str = ComFunc.GetFld(drInvoice, ComDefine.FLD_REF) + refSuffix;
            // 貿易条件
            xlsx.Cell("**" + ComDefine.FLD_TERMS).Str = ComFunc.GetFld(drInvoice, ComDefine.FLD_TERMS);
            // TOTAL(QTY)
            xlsx.Cell("**" + ComDefine.FLD_TOTAL_CARTON).Value = ComFunc.GetFldToDecimal(drInvoice, ComDefine.FLD_TOTAL_CARTON);
            // TOTAL(QTY単位)
            xlsx.Cell("**" + ComDefine.FLD_TOTAL_CARTON_NAME).Str = ComFunc.GetFld(drInvoice, ComDefine.FLD_TOTAL_CARTON_NAME);
            // TOTAL(YEN)
            decimal total = ComFunc.GetFldToDecimal(drInvoice, ComDefine.FLD_TOTAL);
            xlsx.Cell("**" + ComDefine.FLD_TOTAL).Value = total;
            // DELIVERY TO
            xlsx.Cell("**" + ComDefine.FLD_DELIVERY_TO).Str = ComFunc.GetFld(drInvoice, ComDefine.FLD_DELIVERY_TO);
            // 発行者
            xlsx.Cell("**" + ComDefine.FLD_CREATOR).Str = ComFunc.GetFld(drInvoice, ComDefine.FLD_CREATOR);
            // CASE MARK
            xlsx.Cell("**" + ComDefine.FLD_CASEMARK).Str = ComFunc.GetFld(drInvoice, ComDefine.FLD_CASEMARK);

            if (!isGratis)
            {
                // PO#
                xlsx.Cell("**" + ComDefine.FLD_PO_NO).Str = ComFunc.GetFld(drInvoice, ComDefine.FLD_PO_NO);
                // PLC
                xlsx.Cell("**" + ComDefine.FLD_PLC).Str = ComFunc.GetFld(drInvoice, ComDefine.FLD_PLC);
                // REF.O/#
                xlsx.Cell("**" + ComDefine.FLD_INTERNAL_PO_NO).Str = ComFunc.GetFld(drInvoice, ComDefine.FLD_INTERNAL_PO_NO);
                // タイトル
                xlsx.Cell("**" + ComDefine.FLD_TITLE).Str = ComFunc.GetFld(drInvoice, ComDefine.FLD_TITLE);
                // 出荷先
                xlsx.Cell("**" + ComDefine.FLD_SHIP_TO).Str = ComFunc.GetFld(drInvoice, ComDefine.FLD_SHIP_TO);
                // 便
                xlsx.Cell("**" + ComDefine.FLD_SHIP_NAME).Str = ComFunc.GetFld(drInvoice, ComDefine.FLD_SHIP_NAME);
                // 運賃計算基礎重量
                xlsx.Cell("**" + ComDefine.FLD_TOTAL_GRWT).Str = ComFunc.GetFld(drInvoice, ComDefine.FLD_TOTAL_GRWT);

                //PO# LIST NO + 金額
                int count = 1;
                foreach (DataRow drTotalEveryPono in dtTotalEveryPoNo.Rows)
                {
                    // PO# LIST No
                    xlsx.Cell("**" + ComDefine.FLD_LIST_PO_NO + count).Str = ComFunc.GetFld(drTotalEveryPono, ComDefine.FLD_LIST_PO_NO);
                    // PO# LIST 金額
                    xlsx.Cell("**" + ComDefine.FLD_LIST_PO_TOTAL + count).Value = ComFunc.GetFldToDecimal(drTotalEveryPono, ComDefine.FLD_LIST_PO_TOTAL);
                    count++;
                }

            }

            // 運送会社マスタ.PackingList有無が "有" の場合、packing case list の内容を出力（明細は不要行を削除するので、先に出力する）
            if (isPacking)
            {
                this.ExportPackingCaseList(xlsx, dtPacking, isGratis, ref msgID, ref args);
            }


            

            // ===== 明細 =====
            Size sPos;
            Size ePos;

            if (dtAtached.Rows.Count > 20)
            {
                sPos = xlsx.GetVarNamePos(VAL_HIDDEN, 0);
                ePos = xlsx.GetVarNamePos(VAL_HIDDEN, 1);
            }
            else
            {
                sPos = xlsx.GetVarNamePos(VAL_VISIBLE, 0);
                ePos = xlsx.GetVarNamePos(VAL_VISIBLE, 1);

                // リスト作成
                int count = 1;

                foreach (DataRow drAtached in dtAtached.Rows)
                {
                    string n = count.ToString();

                    // NO.
                    xlsx.Cell("**" + ComDefine.FLD_LIST_NO + n).Value = ComFunc.GetFld(drAtached, ComDefine.FLD_LIST_NO);
                    // DESCRIPTION
                    xlsx.Cell("**" + ComDefine.FLD_LIST_DESCRIPTION + n).Str = ComFunc.GetFld(drAtached, ComDefine.FLD_LIST_DESCRIPTION);
                    // PARTS NO.
                    xlsx.Cell("**" + ComDefine.FLD_LIST_PARTS + ".No" + n).Str = ComFunc.GetFld(drAtached, ComDefine.FLD_LIST_PARTS);
                    // PARTS2 NO.
                    xlsx.Cell("**" + ComDefine.FLD_LIST_PARTS2 + ".No" + n).Str = ComFunc.GetFld(drAtached, ComDefine.FLD_LIST_PARTS2);
                    // AR NO.
                    xlsx.Cell("**" + ComDefine.FLD_LIST_AR_NO + n).Str = ComFunc.GetFld(drAtached, ComDefine.FLD_LIST_AR_NO);
                    // CASE NO.
                    xlsx.Cell("**" + ComDefine.FLD_LIST_CASE_NO + n).Value = ComFunc.GetFld(drAtached, ComDefine.FLD_LIST_CASE_NO);
                    // N/W
                    xlsx.Cell("**" + ComDefine.FLD_LIST_NET_WEIGHT + n).Value = ComFunc.GetFldToDecimal(drAtached, ComDefine.FLD_LIST_NET_WEIGHT);
                    // MADE IN.
                    xlsx.Cell("**" + ComDefine.FLD_MADE_IN + n).Str = ComFunc.GetFld(drAtached, ComDefine.FLD_MADE_IN);
                    // QTY
                    xlsx.Cell("**" + ComDefine.FLD_LIST_QTY + n).Value = ComFunc.GetFldToDecimal(drAtached, ComDefine.FLD_LIST_QTY);
                    //QTY_NAME
                    xlsx.Cell("**" + ComDefine.FLD_LIST_QTY_NAME + n).Value = ComFunc.GetFld(drAtached, ComDefine.FLD_LIST_QTY_NAME);
                    // UNIT PRICE
                    xlsx.Cell("**" + ComDefine.FLD_LIST_UNIT_PRICE + n).Value = ComFunc.GetFldToDecimal(drAtached, ComDefine.FLD_LIST_UNIT_PRICE);
                    // TOTAL
                    xlsx.Cell("**" + ComDefine.FLD_LIST_TOTAL + n).Value = ComFunc.GetFldToDecimal(drAtached, ComDefine.FLD_LIST_TOTAL);
                    // QTY UNIT NAME
                    xlsx.Cell("**" + ComDefine.FLD_LIST_QTY_NAME + n).Str = ComFunc.GetFld(drAtached, ComDefine.FLD_LIST_QTY_NAME);
                    
                    count++;
                }
            }

            // 不要行削除
            if (sPos.Height >= 0 && ePos.Height >= 0)
            {
                int posCount = (ePos.Height - sPos.Height) + 1;
                xlsx.RowDelete(sPos.Height, posCount);
            }

            // ===== 不要シート削除 =====
            if (isGratis)
            {   // 無償
                this.DelSheet(xlsx, Resources.ExportShippingDocument_SheetNameOnerousInvoice);

                if (!isAttached)
                {
                    this.DelSheet(xlsx, Resources.ExportShippingDocument_SheetNameGratisInvoiceAttached);
                }
            }
            else
            {   // 有償
                this.DelSheet(xlsx, Resources.ExportShippingDocument_SheetNameGratisInvoice);
                this.DelSheet(xlsx, Resources.ExportShippingDocument_SheetNameGratisInvoiceAttached);
            }

            return true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// packing case list の内容を出力します
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <param name="dtPacking">Packingデータ</param>
        /// <param name="isGratis">無償/有償</param>
        /// <param name="msgID">エラー発生時のメッセージID</param>
        /// <param name="args">メッセージ置換文字列</param>
        /// <returns>true:成功</returns>
        /// <create>M.Shimizu 2020/08/25 ShippingDocumentのExcel改修</create>
        /// <update>D.Okumura 2021/01/15 PakingList出力行数</update>
        /// <update>D.Okumura 2021/01/15 EFA_SMS-184 複数PO No対応</update>
        /// --------------------------------------------------
        private bool ExportPackingCaseList(XlsxCreator xlsx, DataTable dtPacking, bool isGratis, ref string msgID, ref string[] args)
        {
            // 開始行
            const string VAL_PACKING_LIST_CNO = "**packinglist_cno";
            const string VAL_PACKING_LIST_MM = "**packinglist_mm";
            const string VAL_PACKING_LIST_NW = "**packinglist_nw";
            const string VAL_PACKING_LIST_GW = "**packinglist_gw";
            const string VAL_PACKING_LIST_M3 = "**packinglist_m3";
            
            // C/No. 列
            var colCno = xlsx.GetVarNamePos(VAL_PACKING_LIST_CNO, 0);
            // M/M(cm) 列
            var colMm = xlsx.GetVarNamePos(VAL_PACKING_LIST_MM, 0);
            // N/W(kg) 列
            var colNw = xlsx.GetVarNamePos(VAL_PACKING_LIST_NW, 0);
            // G/W(kg) 列
            var colGw = xlsx.GetVarNamePos(VAL_PACKING_LIST_GW, 0);
            // M3 列
            var colM3 = xlsx.GetVarNamePos(VAL_PACKING_LIST_M3, 0);
            // 最初の行を決定する
            var formats = new[] {
                    colCno,
                    colMm,
                    colNw,
                    colM3,
                };
            var row = formats.Select(item => item.Height).Where(w => w != -1).FirstOrDefault();
            
            // 出力
            foreach (DataRow dr in dtPacking.Rows)
            {
                // C/No.
                if (colCno.Width != -1)
                    xlsx.Pos(colCno.Width, row).Value = ComFunc.GetFldObject(dr, Def_T_PACKING_MEISAI.CT_NO);
                // M/M(cm)
                if (colMm.Width != -1)
                    xlsx.Pos(colMm.Width, row).Str = ComFunc.GetFld(dr, ComDefine.FLD_SUNPO);
                // N/W(kg)
                if (colNw.Width != -1)
                    xlsx.Pos(colNw.Width, row).Value = ComFunc.GetFldObject(dr, ComDefine.FLD_NW);
                // G/W(kg)
                if (colGw.Width != -1)
                    xlsx.Pos(colGw.Width, row).Value = ComFunc.GetFldObject(dr, ComDefine.FLD_GW);
                // M3
                if (colM3.Width != -1)
                    xlsx.Pos(colM3.Width, row).Value = ComFunc.GetFldObject(dr, ComDefine.FLD_M3);

                row++;
            }

            return true;
        }

        #endregion

        #region Attached:シート処理

        /// --------------------------------------------------
        /// <summary>
        /// 無償/有償Attachedシート
        /// </summary>
        /// <param name="xlsx">XLSXオブジェクト</param>
        /// <param name="dtAtached">Atachedデータ</param>
        /// <param name="isGratis">有償/無償</param>
        /// <param name="drInvoice">Invoiceシートデータ</param>
        /// <param name="msgID">エラー発生時のメッセージ</param>
        /// <param name="args">エラーメッセージのパラメータ</param>
        /// <returns>true:成功/false:エラー</returns>
        /// <create>T.Nakata 2018/12/21</create>
        /// <update>M.Shimizu 2020/08/25 ShippingDocumentのExcel改修</update>
        /// <update>D.Okumura 2020/10/02 EFA_SMS-138 有償版 Shipping Document の現地通貨略称表示領域対応</update>
        /// <update>H.Iimuro 2022/10/07 ShippingDocumentのExcel改修</update>
        /// --------------------------------------------------
        private bool ExportExcelAttached(XlsxCreator xlsx, DataTable dtAtached, bool isGratis, DataRow drInvoice, ref string msgID, ref string[] args)
        {
            var ret = false;
            var sheetName = string.Empty;
            var sheetNameSave = string.Empty;
            var expInfoCollection = new ExportInfoCollection();

            if (isGratis)
            {   // +++ 無償 +++
                sheetName = Resources.ExportShippingDocument_SheetNameGratisInvoiceAttached;
                sheetNameSave = Resources.ExportShippingDocument_SheetNameAttachedSheet;

                // NO
                expInfoCollection.Add(ComDefine.FLD_LIST_NO, string.Empty);
                // DESCRIPTION
                expInfoCollection.Add(ComDefine.FLD_LIST_DESCRIPTION, string.Empty);
                // PARTS NO.1
                expInfoCollection.Add(ComDefine.FLD_LIST_PARTS, string.Empty);
                // PARTS NO.2
                expInfoCollection.Add(ComDefine.FLD_LIST_PARTS2, string.Empty);
                // AR NO.
                expInfoCollection.Add(ComDefine.FLD_LIST_AR_NO, string.Empty);
                // CASE NO.
                expInfoCollection.Add(ComDefine.FLD_LIST_CASE_NO, string.Empty);
                // N/W
                expInfoCollection.Add(ComDefine.FLD_LIST_NET_WEIGHT, string.Empty);
                // MADE IN
                expInfoCollection.Add(ComDefine.FLD_MADE_IN, string.Empty);
                // QTY
                expInfoCollection.Add(ComDefine.FLD_LIST_QTY, string.Empty);
                // QTY NAME
                expInfoCollection.Add(ComDefine.FLD_LIST_QTY_NAME, string.Empty);
                // UNIT PRICE
                expInfoCollection.Add(ComDefine.FLD_LIST_UNIT_PRICE, string.Empty);
                // TOTAL
                expInfoCollection.Add(ComDefine.FLD_LIST_TOTAL, string.Empty);

                expInfoCollection.IsHeader = false;
                expInfoCollection.IsLine = true;
                expInfoCollection.StartCol = 0;
                expInfoCollection.DataStartRow = DATA_START_ROW_ATTACHED1;
            }
            else
            {   // +++ 有償 +++
                sheetName = Resources.ExportShippingDocument_SheetNameOnerousAttached;
                sheetNameSave = Resources.ExportShippingDocument_SheetNameAttachedSheet;

                // NO
                expInfoCollection.Add(ComDefine.FLD_LIST_NO, string.Empty);
                // DESCRIPTION
                expInfoCollection.Add(ComDefine.FLD_LIST_DESCRIPTION, string.Empty);
                // PARTS NO.1
                expInfoCollection.Add(ComDefine.FLD_LIST_PARTS, string.Empty);
                // PARTS NO.2
                expInfoCollection.Add(ComDefine.FLD_LIST_PARTS2, string.Empty);
                // Tag NO.
                expInfoCollection.Add(Def_T_SHUKKA_MEISAI.TAG_NO, string.Empty);
                // QTY
                expInfoCollection.Add(ComDefine.FLD_LIST_QTY, string.Empty);
                // QTY UNIT NAME
                expInfoCollection.Add(ComDefine.FLD_LIST_QTY_NAME, string.Empty);
                // PO No
                expInfoCollection.Add(ComDefine.FLD_LIST_PO_NO, string.Empty);
                // CASE NO.
                expInfoCollection.Add(ComDefine.FLD_LIST_CASE_NO, string.Empty);
                // N/W
                expInfoCollection.Add(ComDefine.FLD_LIST_NET_WEIGHT, string.Empty);
                // MADE IN
                expInfoCollection.Add(ComDefine.FLD_MADE_IN, string.Empty);
                // UNIT PRICE
                expInfoCollection.Add(ComDefine.FLD_LIST_UNIT_PRICE, string.Empty);
                // TOTAL
                expInfoCollection.Add(ComDefine.FLD_LIST_TOTAL, string.Empty);

                expInfoCollection.IsHeader = false;
                expInfoCollection.IsLine = true;
                expInfoCollection.StartCol = 0;
                expInfoCollection.DataStartRow = DATA_START_ROW_ATTACHED2;
                
            }

            // シート設定
            xlsx.SheetNo = xlsx.SheetNo2(sheetName);
            xlsx.SheetName = sheetNameSave;

            // 行調整
            xlsx.RowInsert(expInfoCollection.DataStartRow + 1, dtAtached.Rows.Count - 1);

            for (int i = 1; i < dtAtached.Rows.Count; i++)
            {
                xlsx.RowCopy(expInfoCollection.DataStartRow, i + expInfoCollection.DataStartRow);
            }

            // PLC(有償のみ)
            xlsx.Cell("**" + ComDefine.FLD_PLC).Str = ComFunc.GetFld(drInvoice, ComDefine.FLD_PLC);
            // INVOICE NO.
            xlsx.Cell("**" + ComDefine.FLD_INVOICE_NO).Str = ComFunc.GetFld(drInvoice, ComDefine.FLD_INVOICE_NO);
            // TOTAL(YEN)
            xlsx.Cell("**" + ComDefine.FLD_TOTAL).Value = ComFunc.GetFldToDecimal(drInvoice, ComDefine.FLD_TOTAL);

            // Excel出力
            ret = this.PusSimpleExcel(xlsx, dtAtached, expInfoCollection, ref msgID, ref args);

            // テンプレート側の罫線を利用するため、罫線出力は行わない。テンプレート側では行挿入により罫線の書式が設定されるように注意すること。
            // // データの罫線出力
            //ret = this.PutSimpleExcelPutDataLine(xlsx, dtAtached, expInfoCollection,
            //                                     xlBorderStyle.xbsThin, xlBorderStyle.xbsMedium, ref msgID, ref args);
            return ret;
        }

        #endregion

        #region 輸出通関確認書:シート処理

        /// --------------------------------------------------
        /// <summary>
        /// 輸出通関確認書
        /// </summary>
        /// <param name="xlsx">XLSXオブジェクト</param>
        /// <param name="drInvoice">Invoiceデータ</param>
        /// <param name="drUnsokaisha">運送会社データ</param>
        /// <returns>true:成功</returns>
        /// <create>T.Nakata 2018/12/21</create>
        /// <update>M.Shimizu 2020/05/28 EFA_SMS-81 対応（輸出通関確認書に納入先名を出力）</update>
        /// --------------------------------------------------
        private bool ExportExcelExportConfirm(XlsxCreator xlsx, DataRow drInvoice, DataRow drUnsokaisha)
        {
            const string VAL_UNSOKAISYA_NAME = "**unsokaisya_name";
            const string VAL_TERMS_EX_WORKS  = "**terms_ex_works";
            const string VAL_TERMS_FOB = "**terms_fob";
            const string VAL_TERMS_CIF = "**terms_cif";
            const string VAL_TERMS_DDU = "**terms_ddu";
            const string VAL_TERMS_DDP = "**terms_ddp";

            xlsx.SheetNo = xlsx.SheetNo2(Resources.ExportShippingDocument_SheetNameExportConfirmation);

            // unsokaisya_name
            xlsx.Cell(VAL_UNSOKAISYA_NAME).Str = ComFunc.GetFld(drUnsokaisha, Def_M_UNSOKAISHA.UNSOKAISHA_NAME);
            // date
            xlsx.Cell("**" + ComDefine.FLD_DATE).Value = ComFunc.GetFldToDateTime(drInvoice, ComDefine.FLD_DATE);
            // creator
            xlsx.Cell("**" + ComDefine.FLD_CREATOR).Str = ComFunc.GetFld(drInvoice, ComDefine.FLD_CREATOR);
            // invoice_no
            xlsx.Cell("**" + ComDefine.FLD_INVOICE_NO).Str = ComFunc.GetFld(drInvoice, ComDefine.FLD_INVOICE_NO);
            // parts_for
            xlsx.Cell("**" + ComDefine.FLD_PARTS_FOR).Str = ComFunc.GetFld(drInvoice, ComDefine.FLD_PARTS_FOR);

            //----- 貿易条件 -----
            const string TRADE_TERMS_FLAG_BASE = "□";
            string terms = ComFunc.GetFld(drInvoice, Def_T_PACKING.TRADE_TERMS_FLAG);
            // Ex-works
            var cell = xlsx.Cell(VAL_TERMS_EX_WORKS);
            cell.Str = TRADE_TERMS_FLAG_BASE;
            if (terms == TRADE_TERMS_FLAG.EX_WORKS_VALUE1) this.DrawingTradeTermsFlag(cell);
            // FOB
            cell = xlsx.Cell(VAL_TERMS_FOB);
            cell.Str = TRADE_TERMS_FLAG_BASE;
            if (terms == TRADE_TERMS_FLAG.FOB_VALUE1) this.DrawingTradeTermsFlag(cell);
            // CIF
            cell = xlsx.Cell(VAL_TERMS_CIF);
            cell.Str = TRADE_TERMS_FLAG_BASE;
            if (terms == TRADE_TERMS_FLAG.CIF_VALUE1) this.DrawingTradeTermsFlag(cell);
            // DDU
            cell = xlsx.Cell(VAL_TERMS_DDU);
            cell.Str = TRADE_TERMS_FLAG_BASE;
            if (terms == TRADE_TERMS_FLAG.DDU_VALUE1) this.DrawingTradeTermsFlag(cell);
            // DDP
            cell = xlsx.Cell(VAL_TERMS_DDP);
            cell.Str = TRADE_TERMS_FLAG_BASE;
            if (terms == TRADE_TERMS_FLAG.DDP_VALUE1) this.DrawingTradeTermsFlag(cell);

            return true;
        }

        #endregion

        #region 中国輸入時記載事項:シート処理

        /// --------------------------------------------------
        /// <summary>
        /// 中国輸入時記載事項
        /// </summary>
        /// <param name="xlsx">XLSXオブジェクト</param>
        /// <param name="drDataB"></param>
        /// <returns></returns>
        /// <create>T.Nakata 2018/12/22</create>
        /// <update>M.Shimizu 2020/08/25 ShippingDocumentのExcel改修</update>
        /// --------------------------------------------------
        private bool ExportExcelImportDescriptionChina(XlsxCreator xlsx, DataRow drConsignee)
        {
            const string VAL_CONSIGNEE_NAME = "**consignee_name";
            const string VAL_CONSIGNEE_ADDRESS = "**consignee_address";
            const string VAL_CONSIGNEE_TEL1 = "**consignee_tel1";
            const string VAL_CONSIGNEE_USCI_CODE = "**consignee_usci_code";

            // 中国輸入時記載事項のシートが存在しない場合、処理を行わない
            int sheetNo = xlsx.SheetNo2(Resources.ExportShippingDocument_SheetNameImportDescriptionChina);

            if (sheetNo < 0)
            {
                return false;
            }
            else
            {
                xlsx.SheetNo = sheetNo;
            }

            // consignee_name
            xlsx.Cell(VAL_CONSIGNEE_NAME).Str = ComFunc.GetFld(drConsignee, Def_M_CONSIGN.NAME);
            // consignee_address
            xlsx.Cell(VAL_CONSIGNEE_ADDRESS).Str = ComFunc.GetFld(drConsignee, Def_M_CONSIGN.ADDRESS);
            // consignee_tel1
            xlsx.Cell(VAL_CONSIGNEE_TEL1).Str = ComFunc.GetFld(drConsignee, Def_M_CONSIGN.TEL1);
            // consignee_usci_code
            xlsx.Cell(VAL_CONSIGNEE_USCI_CODE).Str = ComFunc.GetFld(drConsignee, Def_M_CONSIGN.USCI_CD);

            return true;
        }

        #endregion

        #region 出荷明細:シート処理

        /// --------------------------------------------------
        /// <summary>
        /// 出荷明細
        /// </summary>
        /// <param name="xlsx">XLSXオブジェクト</param>
        /// <param name="dtDataH"></param>
        /// <param name="msgID"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <create>T.Nakata 2018/12/21</create>
        /// <update>J.Chen 2022/10/14 列移動・追加</update>
        /// <update>J.Chen 2022/12/19 TAG便名追加</update>
        /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
        /// --------------------------------------------------
        private bool ExportExcelShukkaMeisai(XlsxCreator xlsx, DataTable dtDataH, ref string msgID, ref string[] args)
        {
            try
            {
                string colName = string.Empty;
                string colCaption = string.Empty;
                string dateTimeFormat = "yyyy/MM/dd";
                ExportInfoCollection expInfoCollection = new ExportInfoCollection();
                // Tag No.
                colName = Def_T_SHUKKA_MEISAI.TAG_NO;
                colCaption = Resources.ExportShukkaMeisai_TagNo;
                expInfoCollection.Add(colName, colCaption);
                // 製番
                colName = Def_T_SHUKKA_MEISAI.SEIBAN;
                colCaption = Resources.ExportShukkaMeisai_ProductNumber;
                expInfoCollection.Add(colName, colCaption);
                // CODE
                colName = Def_T_SHUKKA_MEISAI.CODE;
                colCaption = Resources.ExportShukkaMeisai_CODE;
                expInfoCollection.Add(colName, colCaption);
                // 図面追番
                colName = Def_T_SHUKKA_MEISAI.ZUMEN_OIBAN;
                colCaption = Resources.ExportShukkaMeisai_DrawingSerialNumber;
                expInfoCollection.Add(colName, colCaption);
                // 納入先
                colName = Def_M_NONYUSAKI.NONYUSAKI_NAME;
                colCaption = Resources.ExportShukkaMeisai_DeliveryDestination;
                expInfoCollection.Add(colName, colCaption);
                // 出荷便
                colName = Def_M_NONYUSAKI.SHIP;
                colCaption = Resources.ExportShukkaMeisai_Ship;
                expInfoCollection.Add(colName, colCaption);
                // Area
                colName = Def_T_SHUKKA_MEISAI.AREA;
                colCaption = Resources.ExportShukkaMeisai_Area;
                expInfoCollection.Add(colName, colCaption);
                // Floor
                colName = Def_T_SHUKKA_MEISAI.FLOOR;
                colCaption = Resources.ExportShukkaMeisai_Floor;
                expInfoCollection.Add(colName, colCaption);
                // 機種
                colName = Def_T_SHUKKA_MEISAI.KISHU;
                colCaption = Resources.ExportShukkaMeisai_Model;
                expInfoCollection.Add(colName, colCaption);
                // ST-No.
                colName = Def_T_SHUKKA_MEISAI.ST_NO;
                colCaption = Resources.ExportShukkaMeisai_STNo;
                expInfoCollection.Add(colName, colCaption);
                // 品名(和文)
                colName = Def_T_SHUKKA_MEISAI.HINMEI_JP;
                colCaption = Resources.ExportShukkaMeisai_JpName;
                expInfoCollection.Add(colName, colCaption);
                // 品名
                colName = Def_T_SHUKKA_MEISAI.HINMEI;
                colCaption = Resources.ExportShukkaMeisai_Name;
                expInfoCollection.Add(colName, colCaption);
                // 図面/形式
                colName = Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI;
                colCaption = Resources.ExportShukkaMeisai_DrawingNumberFormat;
                expInfoCollection.Add(colName, colCaption);
                // 図面/形式2
                colName = Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI2;
                colCaption = Resources.ExportShukkaMeisai_DrawingNumberFormat2;
                expInfoCollection.Add(colName, colCaption);
                // 区割 No.
                colName = Def_T_SHUKKA_MEISAI.KUWARI_NO;
                colCaption = Resources.ExportShukkaMeisai_SectioningNo;
                expInfoCollection.Add(colName, colCaption);
                // 数量
                colName = Def_T_SHUKKA_MEISAI.NUM;
                colCaption = Resources.ExportShukkaMeisai_Quantity;
                expInfoCollection.Add(colName, colCaption);
                // GRWT
                colName = Def_T_SHUKKA_MEISAI.GRWT;
                colCaption = Resources.ExportShukkaMeisai_GRWT;
                expInfoCollection.Add(colName, colCaption);
                // MAKER
                colName = Def_T_TEHAI_MEISAI.MAKER;
                colCaption = Resources.ExportShukkaMeisai_Maker;
                expInfoCollection.Add(colName, colCaption);
                // Free1
                colName = Def_T_SHUKKA_MEISAI.FREE1;
                colCaption = Resources.ExportShukkaMeisai_Free1;
                expInfoCollection.Add(colName, colCaption);
                // Free2
                colName = Def_T_SHUKKA_MEISAI.FREE2;
                colCaption = Resources.ExportShukkaMeisai_Free2;
                expInfoCollection.Add(colName, colCaption);
                // Inv付加名
                colName = Def_T_TEHAI_MEISAI.HINMEI_INV;
                colCaption = Resources.ExportShukkaMeisai_INVName;
                expInfoCollection.Add(colName, colCaption);
                // 備考1
                colName = Def_T_TEHAI_MEISAI.NOTE;
                colCaption = Resources.ExportShukkaMeisai_Note;
                expInfoCollection.Add(colName, colCaption);
                // 備考2
                colName = Def_T_TEHAI_MEISAI.NOTE2;
                colCaption = Resources.ExportShukkaMeisai_Note2;
                expInfoCollection.Add(colName, colCaption);
                // 備考3
                colName = Def_T_TEHAI_MEISAI.NOTE3;
                colCaption = Resources.ExportShukkaMeisai_Note3;
                expInfoCollection.Add(colName, colCaption);
                // 通関確認状態
                colName = Def_T_TEHAI_MEISAI.CUSTOMS_STATUS;
                colCaption = Resources.ExportShukkaMeisai_CustomsStatus;
                expInfoCollection.Add(colName, colCaption);
                // Box No.
                colName = Def_T_SHUKKA_MEISAI.BOX_NO;
                colCaption = Resources.ExportShukkaMeisai_BoxNo;
                expInfoCollection.Add(colName, colCaption);
                // パレット No.
                colName = Def_T_SHUKKA_MEISAI.PALLET_NO;
                colCaption = Resources.ExportShukkaMeisai_PalletNo;
                expInfoCollection.Add(colName, colCaption);
                // 木枠 No.
                colName = ComDefine.FLD_KIWAKU_NO;
                colCaption = Resources.ExportShukkaMeisai_WoodFrameNo;
                expInfoCollection.Add(colName, colCaption);
                // TAG_SHIP
                colName = Def_T_SHUKKA_MEISAI.TAG_SHIP;
                colCaption = Resources.ExportShukkaMeisai_TagShip;
                expInfoCollection.Add(colName, colCaption);
                // AR No.
                colName = Def_T_SHUKKA_MEISAI.AR_NO;
                colCaption = Resources.ExportShukkaMeisai_ARNo;
                expInfoCollection.Add(colName, colCaption);
                // 集荷日付
                colName = Def_T_SHUKKA_MEISAI.SHUKA_DATE;
                colCaption = Resources.ExportShukkaMeisai_PickUpDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // Box梱包日付
                colName = Def_T_SHUKKA_MEISAI.BOXKONPO_DATE;
                colCaption = Resources.ExportShukkaMeisai_BoxPackingDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // パレット梱包日付
                colName = Def_T_SHUKKA_MEISAI.PALLETKONPO_DATE;
                colCaption = Resources.ExportShukkaMeisai_PalletPackingDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 木枠梱包日付
                colName = Def_T_SHUKKA_MEISAI.KIWAKUKONPO_DATE;
                colCaption = Resources.ExportShukkaMeisai_FramePackingDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 出荷日付
                colName = Def_T_SHUKKA_MEISAI.SHUKKA_DATE;
                colCaption = Resources.ExportShukkaMeisai_ShippingDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 運送会社
                colName = Def_T_SHUKKA_MEISAI.UNSOKAISHA_NAME;
                colCaption = Resources.ExportShukkaMeisai_ShippingCompany;
                expInfoCollection.Add(colName, colCaption);
                // インボイスNo.
                colName = Def_T_SHUKKA_MEISAI.INVOICE_NO;
                colCaption = Resources.ExportShukkaMeisai_InvoiceNo;
                expInfoCollection.Add(colName, colCaption);
                // 送り状No.
                colName = Def_T_SHUKKA_MEISAI.OKURIJYO_NO;
                colCaption = Resources.ExportShukkaMeisai_OkurijoNo;
                expInfoCollection.Add(colName, colCaption);
                // BLNo.
                colName = Def_T_SHUKKA_MEISAI.BL_NO;
                colCaption = Resources.ExportShukkaMeisai_BLNo;
                expInfoCollection.Add(colName, colCaption);
                // 受入日付
                colName = Def_T_SHUKKA_MEISAI.UKEIRE_DATE;
                colCaption = Resources.ExportShukkaMeisai_AccessionDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 受入担当者
                colName = Def_T_SHUKKA_MEISAI.UKEIRE_USER_NAME;
                colCaption = Resources.ExportShukkaMeisai_AccessionUser;
                expInfoCollection.Add(colName, colCaption);
                //// 備考
                //colName = Def_T_SHUKKA_MEISAI.BIKO;
                //colCaption = Resources.ExportShukkaMeisai_Memo;
                //expInfoCollection.Add(colName, colCaption);
                // M_NO
                colName = Def_T_SHUKKA_MEISAI.M_NO;
                colCaption = Resources.ExportShukkaMeisai_MNo;
                expInfoCollection.Add(colName, colCaption);
                // TEHAI_RENKEI_NO
                colName = Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO;
                colCaption = Resources.ExportShukkaMeisai_TEHAI_RENKEI_NO;
                expInfoCollection.Add(colName, colCaption);

                expInfoCollection.IsHeader = true;
                expInfoCollection.IsLine = false;
                expInfoCollection.StartCol = 0;
                expInfoCollection.HeaderStartRow = 0;
                expInfoCollection.DataStartRow = 1;

                // シート追加
                xlsx.AddSheet(1);
                // シート設定
                xlsx.SheetNo = (xlsx.SheetCount - 1);
                xlsx.SheetName = Resources.ExportShippingDocument_SheetNameShukkameisai;

                // Excel出力
                bool ret = this.PutSimpleExcelPutHeader(xlsx, dtDataH, expInfoCollection, ref msgID, ref args);
                ret = ret & this.PusSimpleExcel(xlsx, dtDataH, expInfoCollection, ref msgID, ref args);

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

        #region 特殊入力

        /// --------------------------------------------------
        /// <summary>
        /// 貿易条件のチェックマーク記載
        /// </summary>
        /// <param name="xlsx">XLSXオブジェクト</param>
        /// <param name="ValName"></param>
        /// <create>T.Nakata 2018/12/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private void DrawingTradeTermsFlag(XlsCell cell)
        {
            cell.Drawing.LineColor = System.Drawing.Color.Red;
            cell.Drawing.LineWeight = 4;
            cell.Drawing.AddLine(xlShapeType.xltLine, 11, 5, 13.7, 8.7);
            cell.Drawing.AddLine(xlShapeType.xltLine, 13.4, 8.4, 19, 1);
        }

        #endregion

        #region 出力処理

        /// --------------------------------------------------
        /// <summary>
        /// 出力処理
        /// </summary>
        /// <param name="xlsx">XLSXオブジェクト</param>
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
        /// <param name="xls">XLSXオブジェクト</param>
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
                                xlsx.Pos(col, row).Str = ComFunc.GetFldToDateTime(dr, expInfo.DataColName).ToString(expInfo.DateTimeFormat);
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
        /// <param name="xlsx">XLSXオブジェクト</param>
        /// <param name="dt">出力データ</param>
        /// <param name="expInfoCollection">パラメータ</param>
        /// <param name="msgID">エラーメッセージID</param>
        /// <param name="args">エラーメッセージパラメータ</param>
        /// <returns></returns>
        /// <create>T.Nakata 2018/12/21</create>
        /// <update>D.Okumura 2020/10/02 罫線が上書きされてしまう問題を修正</update>
        /// --------------------------------------------------
        private bool PutSimpleExcelPutDataLine(XlsxCreator xlsx, DataTable dt, ExportInfoCollection expInfoCollection, xlBorderStyle StyleLtc, xlBorderStyle StyleBox, ref string msgID, ref string[] args)
        {
            if (!expInfoCollection.IsLine)
                return true;
            int colStart = expInfoCollection.StartCol;
            int rowStart = expInfoCollection.DataStartRow;

            int colEnd = colStart + expInfoCollection.Count - 1;
            int rowEnd = rowStart + dt.Rows.Count - 1;

            xlsx.Pos(colStart, rowStart, colEnd, rowEnd).Attr.Box(xlBoxType.xbtLtc, StyleLtc, xlColor.xclBlack);
            xlsx.Pos(colStart, rowStart, colEnd, rowEnd).Attr.Box(xlBoxType.xbtBox, StyleBox, xlColor.xclBlack);

            return true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// ヘッダ出力
        /// </summary>
        /// <param name="xls">XLSXオブジェクト</param>
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

        #region Excel処理

        /// --------------------------------------------------
        /// <summary>
        /// 指定されたシートを削除します
        /// </summary>
        /// <param name="xlsx">XLSXオブジェクト</param>
        /// <param name="sheetName">シート名</param>
        /// <create>M.Shimizu 2020/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private void DelSheet(XlsxCreator xlsx, string sheetName)
        {
            xlsx.DelSheet(xlsx.SheetNo2(sheetName), 1);
        }

        /// --------------------------------------------------
        /// <summary>
        /// settingシートから指定されたキーの値を取得します
        /// </summary>
        /// <param name="xlsx">XLSXオブジェクト</param>
        /// <param name="key">キー</param>
        /// <returns>設定値</returns>
        /// <create>M.Shimizu 2020/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private object GetSettingData(XlsxCreator xlsx, string key)
        {
            // setting シート
            xlsx.SheetNo = xlsx.SheetNo2("setting");

            // 最終行の取得
            var endPoint = xlsx.MaxArea(xlMaxEndPoint.xarEndPoint);

            // キーが設定されている値を取得
            foreach (int row in Enumerable.Range(1, endPoint.Height))
            {
                if (xlsx.Pos(0, row).Value.ToString() == key)
                {
                    return xlsx.Pos(1, row).Value;
                }
            }

            return xlsx.Cell(key).Value;
        }

        #endregion
    }
}
