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
    /// PackingListExcel出力クラス
    /// </summary>
    /// <create>T.Sakiori 2012/04/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ExportPackingList : BaseExport
    {
        #region フィールド

        /// --------------------------------------------------
        /// <summary>
        /// PackingList別、N/W項目で重量があるデータの行数
        /// </summary>
        /// <create>Y.Gwon 2023/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private int nwCount = 0;
        /// --------------------------------------------------
        /// <summary>
        /// PackingListでN/W項目名のRow位置
        /// </summary>
        /// <create>Y.Gwon 2023/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private int nwRow = 11;
        /// --------------------------------------------------
        /// <summary>
        /// N/W重量データチェック開始フラグ
        /// </summary>
        /// <create>Y.Gwon 2023/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool nwFlag = false;

        #endregion

        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// 1ページあたりの出力する明細数
        /// </summary>
        /// <create>T.Sakiori 2012/04/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int MAXDATAROWS_KONPOMEISAI = 23;
        private const int MAXDATAROWS_MASTERPACKINGLIST = 19;
        private const int MAXDATAROWS_PACKINGLIST = 21;
        /// --------------------------------------------------
        /// <summary>
        /// 1明細を出力するために使用する行数
        /// </summary>
        /// <create>K.Tsutsumi 2012/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int BLOCKROWS_KONPOMEISAI = 2;
        private const int BLOCKROWS_MASTERPACKINGLIST = 2;
        private const int BLOCKROWS_PACKINGLIST = 2;
        /// --------------------------------------------------
        /// <summary>
        /// データ印字開始行
        /// </summary>
        /// <create>K.Tsutsumi 2012/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int DATASTARTROW_KONPOMEISAI = 18 - 1;
        private const int DATASTARTROW_MASTERPACKINGLIST = 18 - 1;
        private const int DATASTARTROW_PACKINGLIST = 14 - 1;
        /// --------------------------------------------------
        /// <summary>
        /// ヘッダーに使用される行数
        /// </summary>
        /// <create>T.Sakiori 2012/04/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int HEADERSTARTROW_KONPOMEISAI = 16 - 1;
        private const int HEADERSTARTROW_MASTERPACKINGLIST = 16 - 1;
        private const int HEADERSTARTROW_PACKINGLIST = 12 - 1;
        /// --------------------------------------------------
        /// <summary>
        /// １ページの行数
        /// </summary>
        /// <create>T.Sakiori 2012/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int PAGEROWS_KONPOMEISAI = 65;
        private const int PAGEROWS_MASTERPACKINGLIST = 57;
        private const int PAGEROWS_PACKINGLIST = 55;
        /// --------------------------------------------------
        /// <summary>
        /// 明細出力行の行の高さ
        /// </summary>
        /// <create>K.Tsutsumi 2012/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int ROWHEIGHT_KONPOMEISAI = 12;
        private const int ROWHEIGHT_MASTERPACKINGLIST = 15;
        private const int ROWHEIGHT_PACKINGLIST = 15;
        /// --------------------------------------------------
        /// <summary>
        /// フッター出力開始行
        /// </summary>
        /// <create>K.Tsutsumi 2012/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int FOOTERSTARTROW_KONPOMEISAI = 64 - 1;
        private const int FOOTERSTARTROW_MASTERPACKINGLIST = 56 - 1;

        /// --------------------------------------------------
        /// <summary>
        /// テンプレート内の各シート名
        /// </summary>
        /// <create>K.Tsutsumi 2012/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string SHEET_NAME_TEMPLATE_KONPOMEISAI = "template_梱包明細書";
        private const string SHEET_NAME_TEMPLATE_MASTERPACKINGLIST = "template_MPL";
        private const string SHEET_NAME_TEMPLATE_PACKINGLIST = "template_PL";


        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>T.Sakiori 2012/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public ExportPackingList()
            : base()
        {
        }

        #endregion

        #region ExportExcel

        /// --------------------------------------------------
        /// <summary>
        /// PackingListExcelの出力
        /// </summary>
        /// <param name="filePath">ファイル名</param>
        /// <param name="dt">出力データ</param>
        /// <param name="msgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2012/04/23</create>
        /// <update>D.Okumura 2019/08/27 木枠まとめ対応</update>
        /// --------------------------------------------------
        public bool ExportExcel(string filePath, DataRow dr, DataSet ds, out string msgID, out string[] args)
        {
            msgID = string.Empty;
            args = null;
            string caseMarkFilePath = null;
            try
            {
                string kojiNo = ComFunc.GetFld(dr, Def_T_KIWAKU.KOJI_NO);
                string fileName = ComFunc.GetFld(dr, Def_T_KIWAKU.CASE_MARK_FILE);
                caseMarkFilePath = Path.Combine(ComDefine.DOWNLOAD_DIR, fileName);

                // CASEマークのDL
                var conn = new ConnAttachFile();
                FileDownloadPackage package = new FileDownloadPackage();

                package.FileName = fileName;
                package.FileType = FileType.CaseMark;
                package.KojiNo = kojiNo;
                var ret = conn.FileDownload(package);
                if (ret.IsExistsFile)
                {
                    using (FileStream fs = new FileStream(caseMarkFilePath, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(ret.FileData, 0, ret.FileData.Length);
                        fs.Close();
                    }
                }
                // 帳票出力
                var result = this.ExportExcel(filePath, dr, ds, kojiNo, caseMarkFilePath, out msgID, out args);

                return result;
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
            finally
            {
                if (File.Exists(caseMarkFilePath))
                {
                    File.Delete(caseMarkFilePath);
                }
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// PackingListExcelの出力
        /// </summary>
        /// <param name="filePath">ファイル名</param>
        /// <param name="dt">出力データ</param>
        /// <param name="kojiNo">工事識別No(Nullの場合、工事識別番号でデータの抽出を行わない)</param>
        /// <param name="caseMarkImagePath">CASE MARKデータ(null許容)</param>
        /// <param name="msgID">メッセージID</param>
        /// <param name="args">パラメータ</param>
        /// <returns>成功/失敗</returns>
        /// <create>T.Sakiori 2012/04/23</create>
        /// <update>D.Okumura 2019/08/27 木枠まとめ対応</update>
        /// --------------------------------------------------
        public bool ExportExcel(string filePath, DataRow dr, DataSet ds, string kojiNo, string caseMarkFilePath, out string msgID, out string[] args)
        {
            msgID = string.Empty;
            args = null;
            try
            {

                var xlsx = new XlsxCreator();
                xlsx.OpenBook(filePath, Path.Combine(ComDefine.DOWNLOAD_DIR, ComDefine.EXCEL_FILE_TEMPLATE));

                // 2012/07/05 K.Tsutsumi Add 関数化
                bool bolRet = true;

                if (bolRet == true)
                {
                    // 梱包明細書
                    bolRet = this.ExportExcel_KonpoMeisai(xlsx, kojiNo, filePath, dr, ds, caseMarkFilePath, out msgID, out args);
                }

                if (bolRet == true)
                {
                    // Master Packing List
                    bolRet = this.ExportExcel_MasterPackingList(xlsx, kojiNo, filePath, dr, ds, caseMarkFilePath, out msgID, out args);
                }

                if (bolRet == true)
                {
                    // Packing List
                    bolRet = this.ExportExcel_PackingList(xlsx, kojiNo, filePath, ds, caseMarkFilePath, out msgID, out args);
                }
                // ↑

                if (bolRet == true)
                {
                    // 出荷明細
                    bolRet = this.ExportExcel_ShukkaMeisai(xlsx, ds.Tables[Def_T_SHUKKA_MEISAI.Name], out msgID, out args);
                }

                // テンプレートシートの削除
                xlsx.DelSheet(xlsx.SheetNo2(SHEET_NAME_TEMPLATE_KONPOMEISAI),1);
                xlsx.DelSheet(xlsx.SheetNo2(SHEET_NAME_TEMPLATE_MASTERPACKINGLIST), 1);
                xlsx.DelSheet(xlsx.SheetNo2(SHEET_NAME_TEMPLATE_PACKINGLIST), 1);

                xlsx.CloseBook(true);

                // 2012/07/05 K.Tsutsumi Change 関数化
                //// 木枠明細Excelファイルを出力しました。
                //msgID = "E0100030001";
                //args = null;
                if (bolRet)
                {
                    // 木枠明細Excelファイルを出力しました。
                    msgID = "E0100030001";
                    args = null;
                }
                else
                {
                    if (string.IsNullOrEmpty(msgID))
                    {
                        // Excel出力に失敗しました。
                        msgID = "A7777777001";
                    }
                }
                // ↑

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

        #region 梱包明細書

        /// --------------------------------------------------
        /// <summary>
        ///     梱包明細書の出力
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <param name="kojiNo">工事識別管理No</param>
        /// <param name="filePath">Excelファイル保存先</param>
        /// <param name="drKiwaku">木枠情報</param>
        /// <param name="ds">出力データ</param>
        /// <param name="caseMarkImagePath">CASE MARKデータ</param>
        /// <param name="msgID">エラーメッセージID</param>
        /// <param name="args">エラーメッセージパラメータ</param>
        /// <returns>True:OK False:NG</returns>
        /// <create>K.Tsutsumi 2012/07/05</create>
        /// <update>D.Okumura 2019/08/27 木枠まとめ対応</update>
        /// --------------------------------------------------
        private bool ExportExcel_KonpoMeisai(XlsxCreator xlsx, string kojiNo, string filePath, DataRow drKiwaku, DataSet ds, string caseMarkImagePath, out string msgID, out string[] args)
        {
            msgID = string.Empty;
            args = null;
            try
            {
                // 変数初期化
                ExportShareInfo exInfo = new ExportShareInfo();
                exInfo.PageNo = 0;
                exInfo.LineNo = PAGEROWS_KONPOMEISAI + 1;
                exInfo.KojiNo = kojiNo;
                exInfo.CaseMarkImagePath = caseMarkImagePath;

                var dataKojiNo = ds.Tables[Def_T_KIWAKU.KOJI_NO]
                    .AsEnumerable()
                    .Where(p => exInfo.KojiNo == null || ComFunc.GetFld(p, Def_T_KIWAKU.KOJI_NO) == exInfo.KojiNo)
                    .Select((p, i) => new { p, i });

                if (dataKojiNo.Count() == 0)
                {
                    // データが存在しなければ処理を抜ける
                    return true;
                }

                xlsx.ScaleMode = xlScaleMode.xsmPixel;
                // CASE MARKを張り付ける領域のサイズを取得する
                exInfo.RowHeight = 23 * 7;
                exInfo.ColWidth = 17 * 19;
                // 張り付ける画像のサイズを取得する
                if (File.Exists(exInfo.CaseMarkImagePath))
                {
                    using (var bmp = new Bitmap(exInfo.CaseMarkImagePath))
                    {
                        exInfo.BmpHeight = bmp.Height;
                        exInfo.BmpWidth = bmp.Width;
                    }
                    //exInfo.Rate = exInfo.BmpHeight / exInfo.BmpWidth;
                    exInfo.Rate = exInfo.BmpWidth / exInfo.BmpHeight;
                }
                else
                {
                    exInfo.BmpHeight = 0;
                    exInfo.BmpWidth = 0;
                    exInfo.Rate = 1;
                }

                foreach (var item in dataKojiNo)
                {
                    this.ExportExcel_KonpoMeisai_Data(xlsx, exInfo, drKiwaku, item.p);
                }

                // 合計行出力
                this.ExportExcel_KonpoMeisai_Footer(xlsx, exInfo);

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

        /// --------------------------------------------------
        /// <summary>
        /// 1明細出力
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <param name="exInfo">ExportShareInfo</param>
        /// <param name="drHead">ヘッダ情報</param>
        /// <param name="drData">明細情報</param>
        /// <create>K.Tsutsumi 2012/07/05</create>
        /// <update>H.Tajimi 2015/11/26 ケースナンバーの欠番対応</update>
        /// <update>J.Chen 2023/12/13 小数点第2位まで対応修正</update>
        /// <update></update>
        /// --------------------------------------------------
        private void ExportExcel_KonpoMeisai_Data(XlsxCreator xlsx, ExportShareInfo exInfo, DataRow drHead, DataRow drData)
        {
            try
            {
                // 改ページチェック
                this.ExportExcel_KonpoMeisai_IsNextPage(xlsx, exInfo, drHead);

                // 行出力
                int row = PAGEROWS_KONPOMEISAI * (exInfo.PageNo - 1) + DATASTARTROW_KONPOMEISAI + (exInfo.LineNo - 1) * BLOCKROWS_KONPOMEISAI;

                // C/NO
                // 2015/11/26 H.Tajimi 印刷C/NOを使用するよう変更
                xlsx.Pos(0, row).Str = ComFunc.GetFld(drData, Def_T_KIWAKU_MEISAI.PRINT_CASE_NO);
                // ↑

                // STYLE
                xlsx.Pos(4, row + 1).Str = ComFunc.GetFld(drData, Def_T_KIWAKU_MEISAI.STYLE);

                // ITEM
                string[] itemName = UtilString.DivideString(drData[Def_T_KIWAKU_MEISAI.ITEM].ToString(), 15);
                xlsx.Pos(7, row).Str = itemName[0];
                xlsx.Pos(7, row + 1).Str = itemName[1];

                // DESCRIPTION
                xlsx.Pos(13, row).Str = ComFunc.GetFld(drData, Def_T_KIWAKU_MEISAI.DESCRIPTION_1);
                xlsx.Pos(13, row + 1).Str = ComFunc.GetFld(drData, Def_T_KIWAKU_MEISAI.DESCRIPTION_2);

                // Q'TY
                xlsx.Pos(24, row + 1).Value = ComFunc.GetFldToInt32(drData, "QTY");
                exInfo.TotalQty++;

                // DIMENSION L
                xlsx.Pos(26, row + 1).Value = ComFunc.GetFldToInt32(drData, Def_T_KIWAKU_MEISAI.DIMENSION_L);

                // DIMENSION W
                xlsx.Pos(28, row + 1).Value = ComFunc.GetFldToInt32(drData, Def_T_KIWAKU_MEISAI.DIMENSION_W);

                // DIMENSION H
                xlsx.Pos(30, row + 1).Value = ComFunc.GetFldToInt32(drData, Def_T_KIWAKU_MEISAI.DIMENSION_H);

                // M'MNET
                xlsx.Pos(32, row + 1).Value = ComFunc.GetFldToDecimal(drData, Def_T_KIWAKU_MEISAI.MMNET);
                exInfo.TotalMMNet += ComFunc.GetFldToDecimal(drData, Def_T_KIWAKU_MEISAI.MMNET);

                // NET/W.
                xlsx.Pos(37, row + 1).Value = ComFunc.GetFldToDecimal(drData, Def_T_KIWAKU_MEISAI.NET_W);
                exInfo.TotalNetW += ComFunc.GetFldToDecimal(drData, Def_T_KIWAKU_MEISAI.NET_W);

                // GROSS/W.
                xlsx.Pos(41, row + 1).Value = ComFunc.GetFldToDecimal(drData, Def_T_KIWAKU_MEISAI.GROSS_W);
                exInfo.TotalGrossW += ComFunc.GetFldToDecimal(drData, Def_T_KIWAKU_MEISAI.GROSS_W);

                // 行更新
                exInfo.LineNo++;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 改ページチェック
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <param name="exInfo">ExportShareInfo</param>
        /// <param name="drHead">ヘッダ情報</param>
        /// <create>K.Tsutsumi 2012/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ExportExcel_KonpoMeisai_IsNextPage(XlsxCreator xlsx, ExportShareInfo exInfo, DataRow drHead)
        {
            try
            {
                // 改ページが必要か？
                if (exInfo.LineNo > MAXDATAROWS_KONPOMEISAI)
                {
                    // 次ページのヘッダ出力
                    this.ExportExcel_KonpoMeisai_Header(xlsx, exInfo, drHead);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// ヘッダ出力
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <param name="exInfo">ExportShareInfo</param>
        /// <param name="drHead">ヘッダ情報</param>
        /// <create>K.Tsutsumi 2012/07/05</create>
        /// <update>D.Okumura 2019/08/27 木枠まとめ対応</update>
        /// --------------------------------------------------
        private void ExportExcel_KonpoMeisai_Header(XlsxCreator xlsx, ExportShareInfo exInfo, DataRow drHead)
        {
            try
            {
                // レイアウトコピー
                this.ExportExcel_KonpoMeisai_Layout(xlsx, exInfo);

                int sy = 0;
                // タイトル
                xlsx.Pos(14, sy).Str = Resources.ExportPackingList_Title;

                // ページ番号
                sy = PAGEROWS_KONPOMEISAI * exInfo.PageNo;
                xlsx.Pos(40, sy).Str = string.Format(Resources.ExportPackingList_PageNo, exInfo.PageNo + 1);

                // ヘッダーに必要情報を出力する
                // 工事識別 NO. 項目名
                xlsx.Pos(0, sy + 3).Str = Resources.ExportPackingList_KojiName;
                // 工事識別 NO.
                xlsx.Pos(9, sy + 3).Str = ComFunc.GetFld(drHead, Def_T_KIWAKU.KOJI_NAME);
                // 便
                xlsx.Pos(16, sy + 4).Str = string.Format(Resources.ExportPackingList_KiwakuShip, ComFunc.GetFld(drHead, Def_T_KIWAKU.SHIP));
                // (B)DELIVERY NO
                xlsx.Pos(34, sy + 6).Str = ComFunc.GetFld(drHead, Def_T_KIWAKU.DELIVERY_NO);
                // (C)PORT OF DESTINATION
                xlsx.Pos(34, sy + 8).Str = ComFunc.GetFld(drHead, Def_T_KIWAKU.PORT_OF_DESTINATION);
                // (D)AIR/BOAT
                xlsx.Pos(34, sy + 9).Str = ComFunc.GetFld(drHead, Def_T_KIWAKU.AIR_BOAT);
                // (E)DELIVERY DATE
                xlsx.Pos(34, sy + 10).Str = ComFunc.GetFld(drHead, Def_T_KIWAKU.DELIVERY_DATE);
                // (F)DELIVERY POINT
                xlsx.Pos(34, sy + 11).Str = ComFunc.GetFld(drHead, Def_T_KIWAKU.DELIVERY_POINT);
                // (G)FACTORY
                xlsx.Pos(34, sy + 12).Str = ComFunc.GetFld(drHead, Def_T_KIWAKU.FACTORY);
                // *REMARKS
                xlsx.Pos(34, sy + 13).Str = ComFunc.GetFld(drHead, Def_T_KIWAKU.REMARKS);

                // CASE MARK画像
                if (File.Exists(exInfo.CaseMarkImagePath))
                {
                    if (exInfo.RowHeight * exInfo.Rate < exInfo.ColWidth)
                    {
                        xlsx.Pos(1, sy + 7, 19, sy + 13).Drawing.AddImage(exInfo.CaseMarkImagePath, 0, 1, 0, 0, exInfo.RowHeight * exInfo.Rate, exInfo.RowHeight);
                    }
                    else if (exInfo.ColWidth * (1 / exInfo.Rate) < exInfo.RowHeight)
                    {
                        xlsx.Pos(1, sy + 7, 19, sy + 13).Drawing.AddImage(exInfo.CaseMarkImagePath, 0, 1, 0, 0, exInfo.ColWidth, exInfo.ColWidth * (1 / exInfo.Rate));
                    }
                }

                // ページ更新
                exInfo.PageNo++;
                exInfo.LineNo = 1;

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// フッター出力
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <param name="exInfo">ExportShareInfo</param>
        /// <create>K.Tsutsumi 2012/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ExportExcel_KonpoMeisai_Footer(XlsxCreator xlsx, ExportShareInfo exInfo)
        {
            try
            {

                // 合計を記述
                int row = PAGEROWS_KONPOMEISAI * (exInfo.PageNo - 1) + FOOTERSTARTROW_KONPOMEISAI;

                // 罫線
                xlsx.Pos(0, row, 44, row + 1).Attr.Box(xlBoxType.xbtBox, xlBorderStyle.xbsThin, xlColor.xclBlack);
                xlsx.Pos(4, row, 4, row + 1).Attr.LineLeft(xlBorderStyle.xbsThin, xlColor.xclBlack);
                xlsx.Pos(24, row, 24, row + 1).Attr.LineLeft(xlBorderStyle.xbsThin, xlColor.xclBlack);
                xlsx.Pos(26, row, 26, row + 1).Attr.LineLeft(xlBorderStyle.xbsThin, xlColor.xclBlack);
                xlsx.Pos(32, row, 32, row + 1).Attr.LineLeft(xlBorderStyle.xbsThin, xlColor.xclBlack);
                xlsx.Pos(37, row, 37, row + 1).Attr.LineLeft(xlBorderStyle.xbsThin, xlColor.xclBlack);
                xlsx.Pos(41, row, 41, row + 1).Attr.LineLeft(xlBorderStyle.xbsThin, xlColor.xclBlack);

                // 合計
                xlsx.Pos(0, row).Str = Resources.ExportPackingList_Sum;
                xlsx.Pos(24, row).Value = exInfo.TotalQty;
                xlsx.Pos(32, row).Value = exInfo.TotalMMNet;
                xlsx.Pos(37, row).Value = exInfo.TotalNetW;
                xlsx.Pos(41, row).Value = exInfo.TotalGrossW;

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// レイアウトコピー
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <param name="exInfo">ExportShareInfo</param>
        /// <create>K.Tsutsumi 2012/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ExportExcel_KonpoMeisai_Layout(XlsxCreator xlsx, ExportShareInfo exInfo)
        {
            try
            {
                if (exInfo.PageNo == 0)
                {
                    // テンプレートシートのコピー
                    xlsx.CopySheet(xlsx.SheetNo2(SHEET_NAME_TEMPLATE_KONPOMEISAI), xlsx.SheetCount, Resources.ExportPackingList_SheetNameKonpoMeisai);
                    xlsx.SheetNo = xlsx.SheetNo2(Resources.ExportPackingList_SheetNameKonpoMeisai);

                }
                else
                {
                    int sy = PAGEROWS_KONPOMEISAI * exInfo.PageNo;

                    // テンプレート用シートからレイアウトのコピー
                    xlsx.SheetNo = xlsx.SheetNo2(SHEET_NAME_TEMPLATE_KONPOMEISAI);
                    xlsx.Pos(0, 0, 44, PAGEROWS_KONPOMEISAI - 1).Copy();
                    // レイアウトを次ページ領域にコピー
                    xlsx.SheetNo = xlsx.SheetNo2(Resources.ExportPackingList_SheetNameKonpoMeisai);

                    xlsx.Pos(0, sy).Paste();
                    xlsx.RowPaste(sy);

                    // 高さの調整も行う
                    xlsx.Pos(0, sy + 2).RowHeight = 12.75;
                    xlsx.Pos(0, sy + 5).RowHeight = 13.5;
                    xlsx.Pos(0, sy + 14).RowHeight = 13.5;
                    for (int j = sy + HEADERSTARTROW_KONPOMEISAI; j < sy + HEADERSTARTROW_KONPOMEISAI + (MAXDATAROWS_KONPOMEISAI + 1) * BLOCKROWS_KONPOMEISAI; j++)
                    {
                        xlsx.Pos(0, j).RowHeight = ROWHEIGHT_KONPOMEISAI;
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        #endregion

        #region Master Packing List

        /// --------------------------------------------------
        /// <summary>
        ///     Master Packing Listの出力
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <param name="kojiNo">工事識別管理No</param>
        /// <param name="filePath">Excelファイル保存先</param>
        /// <param name="drKiwaku">木枠情報</param>
        /// <param name="ds">出力データ</param>
        /// <param name="caseMarkImagePath">CASE MARKデータ</param>
        /// <param name="msgID">エラーメッセージID</param>
        /// <param name="args">エラーメッセージパラメータ</param>
        /// <returns>True:OK False:NG</returns>
        /// <create>K.Tsutsumi 2012/07/05</create>
        /// <update>D.Okumura 2019/08/27 木枠まとめ対応</update>
        /// --------------------------------------------------
        private bool ExportExcel_MasterPackingList(XlsxCreator xlsx, string kojiNo, string filePath, DataRow drKiwaku, DataSet ds, string caseMarkImagePath, out string msgID, out string[] args)
        {
            msgID = string.Empty;
            args = null;
            try
            {
                // 変数初期化
                ExportShareInfo exInfo = new ExportShareInfo();
                exInfo.PageNo = 0;
                exInfo.LineNo = PAGEROWS_MASTERPACKINGLIST + 1;
                exInfo.KojiNo = kojiNo;
                exInfo.CaseMarkImagePath = caseMarkImagePath;

                var dataKojiNo = ds.Tables[Def_T_KIWAKU.KOJI_NO]
                    .AsEnumerable()
                    .Where(p => exInfo.KojiNo == null || ComFunc.GetFld(p, Def_T_KIWAKU.KOJI_NO) == exInfo.KojiNo)
                    .Select((p, i) => new { p, i });

                // 総ページ数を計算する
                // データ数 / 1ページの行数
                exInfo.TotalPageNo = (dataKojiNo.Count() + 1) / MAXDATAROWS_MASTERPACKINGLIST;
                if (((dataKojiNo.Count() + 1) % MAXDATAROWS_MASTERPACKINGLIST) != 0)
                {
                    exInfo.TotalPageNo++;
                }

                if (dataKojiNo.Count() == 0)
                {
                    // データが存在しなければ処理を抜ける
                    return true;
                }

                xlsx.ScaleMode = xlScaleMode.xsmPixel;
                // CASE MARKを張り付ける領域のサイズを取得する
                exInfo.RowHeight = 16 * 10;
                exInfo.ColWidth = 17 * 19;
                // 張り付ける画像のサイズを取得する
                if (File.Exists(exInfo.CaseMarkImagePath))
                {
                    using (var bmp = new Bitmap(exInfo.CaseMarkImagePath))
                    {
                        exInfo.BmpHeight = bmp.Height;
                        exInfo.BmpWidth = bmp.Width;
                    }
                    //exInfo.Rate = exInfo.BmpHeight / exInfo.BmpWidth;
                    exInfo.Rate = exInfo.BmpWidth / exInfo.BmpHeight;
                }
                else
                {
                    exInfo.BmpHeight = 0;
                    exInfo.BmpWidth = 0;
                    exInfo.Rate = 1;
                }

                foreach (var item in dataKojiNo)
                {
                    if (string.IsNullOrEmpty(exInfo.PrintDate) == true)
                    {
                        // 印刷日付退避
                        exInfo.PrintDate = "DATE:" + ComFunc.GetFldToDateTime(item.p, "PRINT_DATE").ToString("dd-MMM-yyyy", DateTimeFormatInfo.InvariantInfo);
                    }
                    this.ExportExcel_MasterPackingList_Data(xlsx, exInfo, drKiwaku, item.p);
                }

                // 合計行出力
                this.ExportExcel_MasterPackingList_Footer(xlsx, exInfo);

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

        /// --------------------------------------------------
        /// <summary>
        /// 1明細出力
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <param name="exInfo">ExportShareInfo</param>
        /// <param name="drHead">ヘッダ情報</param>
        /// <param name="drData">明細情報</param>
        /// <create>K.Tsutsumi 2012/07/05</create>
        /// <update>H.Tajimi 2015/11/26 ケースナンバーの欠番対応</update>
        /// <update>J.Chen 2023/12/13 小数点第2位まで対応修正</update>
        /// <update></update>
        /// --------------------------------------------------
        private void ExportExcel_MasterPackingList_Data(XlsxCreator xlsx, ExportShareInfo exInfo, DataRow drHead, DataRow drData)
        {
            try
            {
                // 改ページチェック
                this.ExportExcel_MasterPackingList_IsNextPage(xlsx, exInfo, drHead);

                // 行出力
                int row = PAGEROWS_MASTERPACKINGLIST * (exInfo.PageNo - 1) + DATASTARTROW_MASTERPACKINGLIST + (exInfo.LineNo - 1) * BLOCKROWS_MASTERPACKINGLIST;

                // C/NO
                // 2015/11/26 H.Tajimi 印刷C/NOを出力するよう変更
                xlsx.Pos(0, row).Str = ComFunc.GetFld(drData, Def_T_KIWAKU_MEISAI.PRINT_CASE_NO);
                // ↑

                // STYLE
                xlsx.Pos(6, row).Str = ComFunc.GetFld(drData, Def_T_KIWAKU_MEISAI.STYLE);

                // DESCRIPTION
                xlsx.Pos(10, row).Str = string.Format("{0}X{1}X{2}CM",
                UtilString.PadLeft(ComFunc.GetFldToInt32(drData, Def_T_KIWAKU_MEISAI.DIMENSION_L).ToString("#,0"), 6, ' '),
                UtilString.PadLeft(ComFunc.GetFldToInt32(drData, Def_T_KIWAKU_MEISAI.DIMENSION_W).ToString("#,0"), 6, ' '),
                UtilString.PadLeft(ComFunc.GetFldToInt32(drData, Def_T_KIWAKU_MEISAI.DIMENSION_H).ToString("#,0"), 6, ' '));                

                // Q/TY
                xlsx.Pos(21, row).Value = ComFunc.GetFldToInt32(drData, "QTY");
                exInfo.TotalQty++;

                // NET(KG)
                xlsx.Pos(24, row).Value = ComFunc.GetFldToDecimal(drData, Def_T_KIWAKU_MEISAI.NET_W);
                exInfo.TotalNetW += ComFunc.GetFldToDecimal(drData, Def_T_KIWAKU_MEISAI.NET_W);

                // GROSS(KG)
                xlsx.Pos(30, row).Value = ComFunc.GetFldToDecimal(drData, Def_T_KIWAKU_MEISAI.GROSS_W);
                exInfo.TotalGrossW += ComFunc.GetFldToDecimal(drData, Def_T_KIWAKU_MEISAI.GROSS_W);

                // M3
                xlsx.Pos(37, row).Value = ComFunc.GetFldToDecimal(drData, Def_T_KIWAKU_MEISAI.MMNET);
                exInfo.TotalMMNet += ComFunc.GetFldToDecimal(drData, Def_T_KIWAKU_MEISAI.MMNET);

                // 行更新
                exInfo.LineNo++;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 改ページチェック
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <param name="exInfo">ExportShareInfo</param>
        /// <param name="drHead">ヘッダ情報</param>
        /// <create>K.Tsutsumi 2012/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ExportExcel_MasterPackingList_IsNextPage(XlsxCreator xlsx, ExportShareInfo exInfo, DataRow drHead)
        {
            try
            {
                // 改ページが必要か？
                if (exInfo.LineNo > MAXDATAROWS_MASTERPACKINGLIST)
                {
                    // 次ページのヘッダ出力
                    this.ExportExcel_MasterPackingList_Header(xlsx, exInfo, drHead);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// ヘッダ出力
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <param name="exInfo">ExportShareInfo</param>
        /// <param name="drHead">ヘッダ情報</param>
        /// <create>K.Tsutsumi 2012/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ExportExcel_MasterPackingList_Header(XlsxCreator xlsx, ExportShareInfo exInfo, DataRow drHead)
        {
            try
            {
                // レイアウトコピー
                this.ExportExcel_MasterPackingList_Layout(xlsx, exInfo);

                int sy = 0;

                // ページ番号
                sy = PAGEROWS_MASTERPACKINGLIST * exInfo.PageNo;
                xlsx.Pos(38, sy).Str = string.Format("PAGE: {0} / {1}", exInfo.PageNo + 1, exInfo.TotalPageNo);

                // 日付
                xlsx.Pos(33, sy + 14).Str = exInfo.PrintDate;

                // CASE MARK画像
                if (File.Exists(exInfo.CaseMarkImagePath))
                {
                    if (exInfo.RowHeight * exInfo.Rate < exInfo.ColWidth)
                    {
                        xlsx.Pos(13, sy + 3, 31, sy + 12).Drawing.AddImage(exInfo.CaseMarkImagePath, 0, 0, 0, 0, exInfo.RowHeight * exInfo.Rate, exInfo.RowHeight);
                    }
                    else if (exInfo.ColWidth * (1 / exInfo.Rate) < exInfo.RowHeight)
                    {
                        xlsx.Pos(13, sy + 3, 31, sy + 12).Drawing.AddImage(exInfo.CaseMarkImagePath, 0, 0, 0, 0, exInfo.ColWidth, exInfo.ColWidth * (1 / exInfo.Rate));
                    }
                }

                // ページ更新
                exInfo.PageNo++;
                exInfo.LineNo = 1;

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// フッター出力
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <param name="exInfo">ExportShareInfo</param>
        /// <create>K.Tsutsumi 2012/07/05</create>
        /// <update>J.Chen 2023/12/13 小数点第2位まで対応修正</update>
        /// <update></update>
        /// --------------------------------------------------
        private void ExportExcel_MasterPackingList_Footer(XlsxCreator xlsx, ExportShareInfo exInfo)
        {
            try
            {

                // 合計を記述
                int row = PAGEROWS_MASTERPACKINGLIST * (exInfo.PageNo - 1) + FOOTERSTARTROW_MASTERPACKINGLIST;

                // 罫線
                xlsx.Pos(0, row + 1, 44, row + 1).Attr.LineBottom(xlBorderStyle.xbsThin, xlColor.xclBlack);
                xlsx.Pos(0, row, 0, row + 1).Attr.LineLeft(xlBorderStyle.xbsThin, xlColor.xclBlack);
                xlsx.Pos(44, row, 44, row + 1).Attr.LineRight(xlBorderStyle.xbsThin, xlColor.xclBlack);
                xlsx.Pos(6, row, 6, row + 1).Attr.LineLeft(xlBorderStyle.xbsThin, xlColor.xclBlack);
                xlsx.Pos(10, row, 10, row + 1).Attr.LineLeft(xlBorderStyle.xbsThin, xlColor.xclBlack);
                xlsx.Pos(21, row, 21, row + 1).Attr.LineLeft(xlBorderStyle.xbsThin, xlColor.xclBlack);
                xlsx.Pos(24, row, 24, row + 1).Attr.LineLeft(xlBorderStyle.xbsThin, xlColor.xclBlack);
                xlsx.Pos(30, row, 30, row + 1).Attr.LineLeft(xlBorderStyle.xbsThin, xlColor.xclBlack);
                xlsx.Pos(37, row, 37, row + 1).Attr.LineLeft(xlBorderStyle.xbsThin, xlColor.xclBlack);

                // 合計
                xlsx.Pos(0, row).Str = "TOTAL";
                xlsx.Pos(21, row).Value = exInfo.TotalQty.ToString("#,0");
                xlsx.Pos(24, row).Value = exInfo.TotalNetW.ToString("#,0.00");
                xlsx.Pos(30, row).Value = exInfo.TotalGrossW.ToString("#,0.00");
                xlsx.Pos(37, row).Value = exInfo.TotalMMNet.ToString("#,0.000");

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// レイアウトコピー
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <param name="exInfo">ExportShareInfo</param>
        /// <create>K.Tsutsumi 2012/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ExportExcel_MasterPackingList_Layout(XlsxCreator xlsx, ExportShareInfo exInfo)
        {
            try
            {
                if (exInfo.PageNo == 0)
                {
                    // テンプレートシートのコピー
                    xlsx.CopySheet(xlsx.SheetNo2(SHEET_NAME_TEMPLATE_MASTERPACKINGLIST), xlsx.SheetCount, Resources.ExportPackingList_SheetNameMasterPackingList);
                    xlsx.SheetNo = xlsx.SheetNo2(Resources.ExportPackingList_SheetNameMasterPackingList);
                }
                else
                {

                    int sy = FOOTERSTARTROW_MASTERPACKINGLIST * exInfo.PageNo;
                    xlsx.Pos(0, sy, 44, sy).Attr.LineTop(xlBorderStyle.xbsThin, xlColor.xclBlack);

                    // テンプレート用シートからレイアウトのコピー
                    xlsx.SheetNo = xlsx.SheetNo2(SHEET_NAME_TEMPLATE_MASTERPACKINGLIST);
                    xlsx.Pos(0, 0, 44, PAGEROWS_MASTERPACKINGLIST - 1).Copy();
                    // レイアウトを次ページ領域にコピー
                    xlsx.SheetNo = xlsx.SheetNo2(Resources.ExportPackingList_SheetNameMasterPackingList);

                    sy = PAGEROWS_MASTERPACKINGLIST * exInfo.PageNo;
                    xlsx.Pos(0, sy).Paste();

                    // 高さの調整も行う
                    for (int j = sy + DATASTARTROW_MASTERPACKINGLIST; j < sy + DATASTARTROW_MASTERPACKINGLIST + (MAXDATAROWS_MASTERPACKINGLIST + 1) * BLOCKROWS_MASTERPACKINGLIST; j++)
                    {
                        xlsx.Pos(0, j).RowHeight = ROWHEIGHT_MASTERPACKINGLIST;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region PackingList

        /// --------------------------------------------------
        /// <summary>
        ///     Packing Listの出力
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <param name="kojiNo">工事識別管理No</param>
        /// <param name="filePath">Excelファイル保存先</param>
        /// <param name="ds">出力データ</param>
        /// <param name="caseMarkImagePath">CASE MARKデータ</param>
        /// <param name="msgID">エラーメッセージID</param>
        /// <param name="args">エラーメッセージパラメータ</param>
        /// <returns>True:OK False:NG</returns>
        /// <create>K.Tsutsumi 2012/07/05</create>
        /// <update>D.Okumura 2019/08/27 木枠まとめ対応</update>
        /// <update>Y.Gwon 2023/08/21 N/W項目にデータなしの時項目名削除対応</update>
        /// --------------------------------------------------
        private bool ExportExcel_PackingList(XlsxCreator xlsx, string kojiNo, string filePath, DataSet ds, string caseMarkImagePath, out string msgID, out string[] args)
        {
            msgID = string.Empty;
            args = null;
            try
            {

                // 変数初期化
                ExportShareInfo exInfo = new ExportShareInfo();
                exInfo.PageNo = 0;
                exInfo.LineNo = PAGEROWS_PACKINGLIST + 1;
                exInfo.KojiNo = kojiNo;
                exInfo.CaseMarkImagePath = caseMarkImagePath;

                var dataKojiNo = ds.Tables[Def_T_KIWAKU.KOJI_NO]
                    .AsEnumerable()
                    .Where(p => exInfo.KojiNo == null || ComFunc.GetFld(p, Def_T_KIWAKU.KOJI_NO) == exInfo.KojiNo)
                    .Select((p, i) => new { p, i });

                xlsx.ScaleMode = xlScaleMode.xsmPixel;
                // CASE MARKを張り付ける領域のサイズを取得する
                exInfo.RowHeight = 25 * 6;
                exInfo.ColWidth = 17 * 19;
                // 張り付ける画像のサイズを取得する
                if (File.Exists(exInfo.CaseMarkImagePath))
                {
                    using (var bmp = new Bitmap(exInfo.CaseMarkImagePath))
                    {
                        exInfo.BmpHeight = bmp.Height;
                        exInfo.BmpWidth = bmp.Width;
                    }
                    //exInfo.Rate = exInfo.BmpHeight / exInfo.BmpWidth;
                    exInfo.Rate = exInfo.BmpWidth / exInfo.BmpHeight;
                }
                else
                {
                    exInfo.BmpHeight = 0;
                    exInfo.BmpWidth = 0;
                    exInfo.Rate = 1;
                }

                // ケース単位処理
                foreach (var item in dataKojiNo)
                {
                    // 変数初期化
                    string prnDate = string.Empty;
                    exInfo.CasePageNo = 0;
                    exInfo.LineNo = MAXDATAROWS_PACKINGLIST + 1;
                    var caseKojiNo = kojiNo == null ? ComFunc.GetFld(item.p, Def_T_KIWAKU.KOJI_NO) : exInfo.KojiNo;

                    var dataShukkaMeisai = ds.Tables[Def_T_SHUKKA_MEISAI.Name]
                        .AsEnumerable()
                        .Where(p => (ComFunc.GetFld(p, Def_T_KIWAKU.KOJI_NO) == caseKojiNo)
                            && ComFunc.GetFld(p, Def_T_KIWAKU_MEISAI.CASE_NO) == ComFunc.GetFld(item.p, Def_T_KIWAKU_MEISAI.CASE_NO))
                        .Select((p, i) => new { p, i });

                    // 総ページ数を計算する
                    // (データ数 + END) / 1ページの行数
                    exInfo.TotalPageNo = (dataShukkaMeisai.Count() + 1) / MAXDATAROWS_PACKINGLIST;
                    if (((dataShukkaMeisai.Count() + 1) % MAXDATAROWS_PACKINGLIST) != 0)
                    {
                        exInfo.TotalPageNo++;
                    }

                    // 明細がなければ次のデータを表示
                    if (dataShukkaMeisai.Count() == 0)
                    {
                        continue;
                    }
           
                    // DETAILに記述
                    foreach (var drSM in dataShukkaMeisai)
                    {
                        this.ExportExcel_PackingList_Data(xlsx, exInfo, item.p, drSM.p);
                    }

                    // ENDの記述
                    this.ExportExcel_PackingList_Footer(xlsx, exInfo, item.p);
                }

                //一番最後のPackingListの重量データ有無チェック
                if (this.nwCount == 0)
                {
                    xlsx.Pos(39, this.nwRow).Value = "";
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

        /// --------------------------------------------------
        /// <summary>
        /// １明細出力
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <param name="exInfo">ExportShareInfo</param>
        /// <param name="drHead">ヘッダ情報</param>
        /// <param name="drData">明細情報</param>
        /// <create>K.Tsutsumi 2012/07/05</create>
        /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
        /// <update>T.Okuda 2023/7/20 N/W追加</update>
        /// <update>Y.Gwon 2023/8/21 N/W項目にデータなしの時項目名削除対応</update>
        /// <update>J.Chen 2023/12/13 小数点第2位まで対応修正</update>
        /// <update></update>
        /// --------------------------------------------------
        private void ExportExcel_PackingList_Data(XlsxCreator xlsx, ExportShareInfo exInfo, DataRow drHead, DataRow drData)
        {
            try
            {
                // 改ページチェック
                this.ExportExcel_PackingList_IsNextPage(xlsx, exInfo, drHead);

                // 行出力
                int row = PAGEROWS_PACKINGLIST * (exInfo.PageNo - 1) + DATASTARTROW_PACKINGLIST + (exInfo.LineNo - 1) * BLOCKROWS_PACKINGLIST;

                // ITEM
                // 2015/12/09 H.Tajimi M_NO対応
                var columnName = Def_T_SHUKKA_MEISAI.M_NO;
                if (!string.IsNullOrEmpty(ComFunc.GetFld(drData, columnName)))
                {
                    xlsx.Pos(0, row).Str = ComFunc.GetFld(drData, columnName);
                }
                // ↑
                else
                {
                    xlsx.Pos(0, row).Str = ComFunc.GetFld(drData, Def_T_SHUKKA_MEISAI.KISHU);
                }
                // TAG
                xlsx.Pos(8, row).Str = ComFunc.GetFld(drData, Def_T_SHUKKA_MEISAI.TAG_NO);
                // DESCRIPTION
                xlsx.Pos(11, row).Str = ComFunc.GetFld(drData, Def_T_SHUKKA_MEISAI.HINMEI);
                xlsx.Pos(29, row).Str = ComFunc.GetFld(drData, Def_T_SHUKKA_MEISAI.BOX_NO);
                xlsx.Pos(33, row).Str = ComFunc.GetFld(drData, Def_T_SHUKKA_MEISAI.FLOOR);
                xlsx.Pos(11, row + 1).Str = ComFunc.GetFld(drData, Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI);
                // N/W
                if (!string.IsNullOrEmpty(ComFunc.GetFld(drData, Def_T_SHUKKA_MEISAI.GRWT)))
                {
                    xlsx.Pos(39, row).Value = String.Format("{0:#,0.00}", ComFunc.GetFldToDecimal(drData, Def_T_SHUKKA_MEISAI.GRWT));
                    xlsx.Pos(43, row).Str = "kg";
                    //該当行が重量データがあると1増加
                    this.nwCount++;
                }
                // QTY
                xlsx.Pos(45, row).Value = ComFunc.GetFldToInt32(drData, Def_T_SHUKKA_MEISAI.NUM);
                if (ComFunc.GetFldToInt32(drData, Def_T_SHUKKA_MEISAI.NUM) == 1)
                {
                    xlsx.Pos(49, row).Str = "PCE";
                }
                else
                {
                    xlsx.Pos(49, row).Str = "PCS";
                }

                // 行更新
                exInfo.LineNo++;

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 改ページチェック
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <param name="exInfo">ExportShareInfo</param>
        /// <param name="drHead">ヘッダ情報</param>
        /// <create>K.Tsutsumi 2012/07/05</create>
        /// <update>Y.Gwon 2023/8/21 N/W項目にデータなしの時項目名削除対応</update>
        /// --------------------------------------------------
        private void ExportExcel_PackingList_IsNextPage(XlsxCreator xlsx, ExportShareInfo exInfo, DataRow drHead)
        {
            try
            {
                // 改ページが必要か？
                if (exInfo.LineNo > MAXDATAROWS_PACKINGLIST)
                {
                    // 次ページのヘッダ出力
                    this.ExportExcel_PackingList_Header(xlsx, exInfo, drHead);

                    //２番目のページから以前ページの重量データ有無チェック
                    if (this.nwFlag)
                    {
                        //以前ページに重量データなしの場合、項目名を消す
                        if (this.nwCount == 0)
                        {
                            xlsx.Pos(39, this.nwRow).Value = "";
                        }

                        //次のページの項目名のRow位置
                        this.nwRow += 55;
                        this.nwCount = 0;
                    }
                    //N/W重量データチェック開始
                    else
                    {
                        this.nwFlag = true;
                    }
                    
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// ヘッダ出力
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <param name="exInfo">ExportShareInfo</param>
        /// <param name="drHead">ヘッダ情報</param>
        /// <create>K.Tsutsumi 2012/07/05</create>
        /// <update>H.Tajimi 2015/11/26 ケースナンバーの欠番対応</update>
        /// <update>D.Okumura 2019/08/27 木枠まとめ対応</update>
        /// <update>J.Chen 2023/12/13 小数点第2位まで対応修正</update>
        /// --------------------------------------------------
        private void ExportExcel_PackingList_Header(XlsxCreator xlsx, ExportShareInfo exInfo, DataRow drHead)
        {
            try
            {
                // レイアウトコピー
                this.ExportExcel_PackingList_Layout(xlsx, exInfo);

                int sy = 0;

                // ページ番号
                sy = PAGEROWS_PACKINGLIST * exInfo.PageNo;
                xlsx.Pos(0, sy).Str = string.Format("PAGE: {0} / {1}", exInfo.CasePageNo + 1, exInfo.TotalPageNo);

                // ヘッダーに必要情報を出力する
                // C/NO
                // 2015/11/26 H.Tajimi 印刷C/NOを使用するよう変更
                xlsx.Pos(31, sy + 4).Str = ComFunc.GetFld(drHead, Def_T_KIWAKU_MEISAI.PRINT_CASE_NO);
                // ↑
                // STYLE
                xlsx.Pos(31, sy + 5).Str = ComFunc.GetFld(drHead, Def_T_KIWAKU_MEISAI.STYLE);
                // NET
                xlsx.Pos(31, sy + 6).Value = ComFunc.GetFldToDecimal(drHead, Def_T_KIWAKU_MEISAI.NET_W).ToString("#,0.00"); 
                // GROSS
                xlsx.Pos(31, sy + 7).Value = ComFunc.GetFldToDecimal(drHead, Def_T_KIWAKU_MEISAI.GROSS_W).ToString("#,0.00");
                // M'MNET
                xlsx.Pos(31, sy + 8).Value = ComFunc.GetFldToDecimal(drHead, Def_T_KIWAKU_MEISAI.MMNET).ToString("#,0.000");
                // DIMENSION
                xlsx.Pos(31, sy + 9).Str = ComFunc.GetFldToInt32(drHead, Def_T_KIWAKU_MEISAI.DIMENSION_L).ToString("#,0") + " X "
                    + ComFunc.GetFldToInt32(drHead, Def_T_KIWAKU_MEISAI.DIMENSION_W).ToString("#,0") + " X "
                    + ComFunc.GetFldToInt32(drHead, Def_T_KIWAKU_MEISAI.DIMENSION_H).ToString("#,0");
                // 日付
                xlsx.Pos(30, sy + 10).Str = "DATE:" + ComFunc.GetFldToDateTime(drHead, "PRINT_DATE").ToString("dd-MMM-yyyy", DateTimeFormatInfo.InvariantInfo);
                // ↑

                // CASE MARK画像
                if (!string.IsNullOrEmpty(exInfo.CaseMarkImagePath))
                {
                    if (exInfo.RowHeight * exInfo.Rate < exInfo.ColWidth)
                    {
                        xlsx.Pos(1, sy + 4, 19, sy + 9).Drawing.AddImage(exInfo.CaseMarkImagePath, 0, 0, 0, 0, exInfo.RowHeight * exInfo.Rate, exInfo.RowHeight);
                    }
                    else if (exInfo.ColWidth * (1 / exInfo.Rate) < exInfo.RowHeight)
                    {
                        xlsx.Pos(1, sy + 4, 19, sy + 9).Drawing.AddImage(exInfo.CaseMarkImagePath, 0, 0, 0, 0, exInfo.ColWidth, exInfo.ColWidth * (1 / exInfo.Rate));
                    }
                }

                // ページ更新
                exInfo.PageNo++;
                exInfo.CasePageNo++;
                exInfo.LineNo = 1;

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// フッター出力
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <param name="exInfo">ExportShareInfo</param>
        /// <param name="drHead">ヘッダ情報</param>
        /// <create>K.Tsutsumi 2012/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ExportExcel_PackingList_Footer(XlsxCreator xlsx, ExportShareInfo exInfo, DataRow drHead)
        {
            try
            {
                // 改ページチェック
                this.ExportExcel_PackingList_IsNextPage(xlsx, exInfo, drHead);

                // ENDを記述
                int row = PAGEROWS_PACKINGLIST * (exInfo.PageNo - 1) + DATASTARTROW_PACKINGLIST + (exInfo.LineNo - 1) * BLOCKROWS_PACKINGLIST;
                xlsx.Pos(11, row, 28, row).Attr.Joint = false;
                xlsx.Pos(29, row, 32, row).Attr.Joint = false;
                xlsx.Pos(33, row, 38, row).Attr.Joint = false;
                xlsx.Pos(11, row + 1, 38, row + 1).Attr.Joint = false;
                xlsx.Pos(11, row, 38, row + 1).Attr.Joint = true;
                xlsx.Pos(11, row).Attr.FontPoint = 22;
                xlsx.Pos(11, row).Attr.PosHorz = xlPosHorz.xphCenter;
                xlsx.Pos(11, row).Attr.PosVert = xlPosVert.xpvCenter;
                xlsx.Pos(11, row).Str = "--- END ---";

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// テンプレート用シートからレイアウトのコピー
        /// </summary>
        /// <param name="xlsx">XlsxCreator</param>
        /// <param name="exInfo">ExportShareInfo</param>
        /// <create>K.Tsutsumi 2012/07/05</create>
        /// <update>T.Okuda 2023/7/20 N/W追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private void ExportExcel_PackingList_Layout(XlsxCreator xlsx, ExportShareInfo exInfo)
        {
            try
            {
                if (exInfo.PageNo == 0)
                {
                    // テンプレートシートのコピー
                    xlsx.CopySheet(xlsx.SheetNo2(SHEET_NAME_TEMPLATE_PACKINGLIST), xlsx.SheetCount, Resources.ExportPackingList_SheetNamePackingList);
                    xlsx.SheetNo = xlsx.SheetNo2(Resources.ExportPackingList_SheetNamePackingList);
                }
                else
                {
                    // テンプレート用シートからレイアウトのコピー
                    xlsx.SheetNo = xlsx.SheetNo2(SHEET_NAME_TEMPLATE_PACKINGLIST);
                    xlsx.Pos(0, 0, 50, PAGEROWS_PACKINGLIST - 1).Copy();
                    // レイアウトを次ページ領域にコピー
                    xlsx.SheetNo = xlsx.SheetNo2(Resources.ExportPackingList_SheetNamePackingList);

                    int sy = PAGEROWS_PACKINGLIST * exInfo.PageNo;
                    xlsx.Pos(0, sy).Paste();
                    
                    // 高さの調整も行う
                    xlsx.Pos(0, sy).RowHeight = 14.25;
                    xlsx.Pos(0, sy + 1).RowHeight = ROWHEIGHT_PACKINGLIST;
                    xlsx.Pos(0, sy + 2).RowHeight = ROWHEIGHT_PACKINGLIST;
                    xlsx.Pos(0, sy + 3).RowHeight = 12;
                    xlsx.Pos(0, sy + 11).RowHeight = ROWHEIGHT_PACKINGLIST;
                    xlsx.Pos(0, sy + 12).RowHeight = ROWHEIGHT_PACKINGLIST;
                    for (int j = sy + HEADERSTARTROW_PACKINGLIST; j < sy + HEADERSTARTROW_PACKINGLIST + MAXDATAROWS_PACKINGLIST * BLOCKROWS_PACKINGLIST; j++)
                    {
                        xlsx.Pos(0, j).RowHeight = ROWHEIGHT_PACKINGLIST;
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }



        #endregion

        #region 出荷明細

        /// --------------------------------------------------
        /// <summary>
        /// 出荷明細の出力
        /// </summary>
        /// <param name="xlsx">ExcelCreator</param>
        /// <param name="dt">出力データ</param>
        /// <param name="msgID">エラーメッセージID</param>
        /// <param name="args">エラーメッセージパラメータ</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2014/02/19</create>
        /// <update>H.Tajimi 2015/11/18 Free1、Free2をExcel出力対象列に追加</update>
        /// <update>H.Tajimi 2015/11/20 備考列追加</update>
        /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
        /// <update>H.Tsuji 2019/01/25 重量列、連携No.列追加</update>
        /// <update>H.Tajimi 2019/09/09 印刷C/NO対応</update>
        /// <update>J.Chen 2023/01/04 INV付加名、TAG便名追加</update>
        /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
        /// --------------------------------------------------
        private bool ExportExcel_ShukkaMeisai(XlsxCreator xlsx, DataTable dt, out string msgID, out string[] args)
        {
            msgID = string.Empty;
            args = null;
            try
            {
                string colName = string.Empty;
                string colCaption = string.Empty;
                string dateTimeFormat = "yyyy/MM/dd";
                var expInfoCollection = new ExportInfoCollection();
                // Tag No.
                colName = Def_T_SHUKKA_MEISAI.TAG_NO;
                colCaption = Resources.ExportPackingList_TagNo;
                expInfoCollection.Add(colName, colCaption);
                // 製番
                colName = Def_T_SHUKKA_MEISAI.SEIBAN;
                colCaption = Resources.ExportPackingList_ProductNumber;
                expInfoCollection.Add(colName, colCaption);
                // CODE
                colName = Def_T_SHUKKA_MEISAI.CODE;
                colCaption = Resources.ExportPackingList_Code;
                expInfoCollection.Add(colName, colCaption);
                // 図面追番
                colName = Def_T_SHUKKA_MEISAI.ZUMEN_OIBAN;
                colCaption = Resources.ExportPackingList_DrawingSerialNumber;
                expInfoCollection.Add(colName, colCaption);
                // 納入先
                colName = Def_M_NONYUSAKI.NONYUSAKI_NAME;
                colCaption = Resources.ExportPackingList_DeliveryDestination;
                expInfoCollection.Add(colName, colCaption);
                // 出荷便
                colName = Def_M_NONYUSAKI.SHIP;
                colCaption = Resources.ExportPackingList_Ship;
                expInfoCollection.Add(colName, colCaption);
                // Area
                colName = Def_T_SHUKKA_MEISAI.AREA;
                colCaption = Resources.ExportPackingList_Area;
                expInfoCollection.Add(colName, colCaption);
                // Floor
                colName = Def_T_SHUKKA_MEISAI.FLOOR;
                colCaption = Resources.ExportPackingList_Floor;
                expInfoCollection.Add(colName, colCaption);
                // 機種
                colName = Def_T_SHUKKA_MEISAI.KISHU;
                colCaption = Resources.ExportPackingList_Model;
                expInfoCollection.Add(colName, colCaption);
                // ST-No.
                colName = Def_T_SHUKKA_MEISAI.ST_NO;
                colCaption = Resources.ExportPackingList_STNo;
                expInfoCollection.Add(colName, colCaption);
                // 品名(和文)
                colName = Def_T_SHUKKA_MEISAI.HINMEI_JP;
                colCaption = Resources.ExportPackingList_JpName;
                expInfoCollection.Add(colName, colCaption);
                // 品名
                colName = Def_T_SHUKKA_MEISAI.HINMEI;
                colCaption = Resources.ExportPackingList_Name;
                expInfoCollection.Add(colName, colCaption);
                // 図面/形式
                colName = Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI;
                colCaption = Resources.ExportPackingList_DrawingNumberFormat;
                expInfoCollection.Add(colName, colCaption);
                // 区割 No.
                colName = Def_T_SHUKKA_MEISAI.KUWARI_NO;
                colCaption = Resources.ExportPackingList_SectioningNo;
                expInfoCollection.Add(colName, colCaption);
                // 数量
                colName = Def_T_SHUKKA_MEISAI.NUM;
                colCaption = Resources.ExportPackingList_Quantity;
                expInfoCollection.Add(colName, colCaption);
                // 出荷便
                colName = Def_M_NONYUSAKI.SHIP;
                colCaption = Resources.ExportPackingList_Ship;
                expInfoCollection.Add(colName, colCaption);
                // Box No.
                colName = Def_T_SHUKKA_MEISAI.BOX_NO;
                colCaption = Resources.ExportPackingList_BoxNo;
                expInfoCollection.Add(colName, colCaption);
                // パレット No.
                colName = Def_T_SHUKKA_MEISAI.PALLET_NO;
                // 2011/02/18 K.Tsutsumi Change No32
                //colCaption = "パレット No.";
                colCaption = Resources.ExportPackingList_PalletNo;
                // ↑
                expInfoCollection.Add(colName, colCaption);
                // 木枠 No.
                colName = Def_T_KIWAKU_MEISAI.PRINT_CASE_NO;
                colCaption = Resources.ExportPackingList_WoodFrameNo;
                expInfoCollection.Add(colName, colCaption);
                // AR No.
                colName = Def_T_SHUKKA_MEISAI.AR_NO;
                colCaption = Resources.ExportPackingList_ARNo;
                expInfoCollection.Add(colName, colCaption);
                // 集荷日付
                colName = Def_T_SHUKKA_MEISAI.SHUKA_DATE;
                colCaption = Resources.ExportPackingList_PickUpDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // Box梱包日付
                colName = Def_T_SHUKKA_MEISAI.BOXKONPO_DATE;
                colCaption = Resources.ExportPackingList_BoxPackingDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // パレット梱包日付
                colName = Def_T_SHUKKA_MEISAI.PALLETKONPO_DATE;
                // 2011/02/18 K.Tsutsumi Change 
                //colCaption = "パレット梱包日付";
                colCaption = Resources.ExportPackingList_PalletPackingDate;
                // ↑
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 木枠梱包日付
                colName = Def_T_SHUKKA_MEISAI.KIWAKUKONPO_DATE;
                colCaption = Resources.ExportPackingList_FramePackingDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 出荷日付
                colName = Def_T_SHUKKA_MEISAI.SHUKKA_DATE;
                colCaption = Resources.ExportPackingList_ShippingDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 運送会社
                colName = Def_T_SHUKKA_MEISAI.UNSOKAISHA_NAME;
                colCaption = Resources.ExportPackingList_ShippingCompany;
                expInfoCollection.Add(colName, colCaption);
                // インボイスNo.
                colName = Def_T_SHUKKA_MEISAI.INVOICE_NO;
                // 2011/02/18 K.Tsutsumi Change 
                //colCaption = "インボイスNo.";
                colCaption = Resources.ExportPackingList_InvoiceNo;
                // ↑
                expInfoCollection.Add(colName, colCaption);
                // 送り状No.
                colName = Def_T_SHUKKA_MEISAI.OKURIJYO_NO;
                colCaption = Resources.ExportPackingList_OkurijoNo;
                expInfoCollection.Add(colName, colCaption);
                // BLNo.
                colName = Def_T_SHUKKA_MEISAI.BL_NO;
                colCaption = Resources.ExportPackingList_BLNo;
                expInfoCollection.Add(colName, colCaption);
                // 受入日付
                colName = Def_T_SHUKKA_MEISAI.UKEIRE_DATE;
                colCaption = Resources.ExportPackingList_AccessionDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 受入担当者
                colName = Def_T_SHUKKA_MEISAI.UKEIRE_USER_NAME;
                colCaption = Resources.ExportPackingList_AccessionUser;
                expInfoCollection.Add(colName, colCaption);
                // 2015/11/18 H.Tajimi Free1,Free2を列追加
                // Free1
                colName = Def_T_SHUKKA_MEISAI.FREE1;
                colCaption = Resources.ExportPackingList_Free1;
                expInfoCollection.Add(colName, colCaption);
                // Free2
                colName = Def_T_SHUKKA_MEISAI.FREE2;
                colCaption = Resources.ExportPackingList_Free2;
                expInfoCollection.Add(colName, colCaption);
                // ↑
                // 2015/11/20 H.Tajimi 備考列追加
                colName = Def_T_SHUKKA_MEISAI.BIKO;
                colCaption = Resources.ExportPackingList_Memo;
                expInfoCollection.Add(colName, colCaption);
                // ↑
                colName = Def_T_SHUKKA_MEISAI.CUSTOMS_STATUS;
                colCaption = Resources.ExportPackingList_CustomsStatus;
                expInfoCollection.Add(colName, colCaption);
                // ↑
                // 2015/12/09 H.Tajimi M_NO列追加
                colName = Def_T_SHUKKA_MEISAI.M_NO;
                colCaption = Resources.ExportPackingList_MNo;
                expInfoCollection.Add(colName, colCaption);
                // ↑
                // 重量
                colName = Def_T_SHUKKA_MEISAI.GRWT;
                colCaption = Resources.ExportPackingList_Weight;
                expInfoCollection.Add(colName, colCaption);
                // 連携NO.
                colName = Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO;
                colCaption = Resources.ExportPackingList_TehaiRenkeiNo;
                expInfoCollection.Add(colName, colCaption);
                // INV付加名
                colName = Def_T_TEHAI_MEISAI.HINMEI_INV;
                colCaption = Resources.ExportPackingList_INVName;
                expInfoCollection.Add(colName, colCaption);
                // TAG便名
                colName = Def_T_SHUKKA_MEISAI.TAG_SHIP;
                colCaption = Resources.ExportPackingList_TagShip;
                expInfoCollection.Add(colName, colCaption);

                bool ret = this.PutSimpleExcel(xlsx, dt, expInfoCollection, ref msgID, ref args);
                if (ret)
                {
                    // 出荷明細Excelファイルを出力しました。
                    msgID = "E0100020001";
                    args = null;
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

        /// --------------------------------------------------
        /// <summary>
        /// 出力処理
        /// </summary>
        /// <param name="xlsx">ExcelCreator</param>
        /// <param name="dt">出力データ</param>
        /// <param name="expInfoCollection">パラメータ</param>
        /// <param name="msgID">エラーメッセージID</param>
        /// <param name="args">エラーメッセージパラメータ</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2014/02/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool PutSimpleExcel(XlsxCreator xlsx, DataTable dt, ExportInfoCollection expInfoCollection, ref string msgID, ref string[] args)
        {
            xlsx.AddSheet(1, 6);
            xlsx.SheetNo = 6;
            xlsx.SheetName = Resources.ExportPackingList_DeliveryDetails;

            ExportInfoCollection putExpInfoCollection;
            if (!this.PutSimpleExcelCreateTargetList(dt, expInfoCollection, out putExpInfoCollection, ref msgID, ref args))
            {
                return false;
            }
            // ヘッダの出力
            if (!this.PutSimpleExcelPutHeader(xlsx, dt, putExpInfoCollection, ref msgID, ref args))
            {
                return false;
            }
            // データの出力
            if (!this.PutSimpleExcelPutData(xlsx, dt, putExpInfoCollection, ref msgID, ref args))
            {
                return false;
            }
            // ヘッダの罫線
            if (!this.PutSimpleExcelPutHeaderLine(xlsx, dt, putExpInfoCollection, ref msgID, ref args))
            {
                return false;
            }
            // データの罫線
            if (!this.PutSimpleExcelPutDataLine(xlsx, dt, putExpInfoCollection, ref msgID, ref args))
            {
                return false;
            }

            return true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// ヘッダーの出力
        /// </summary>
        /// <param name="xlsx">ExcelCreator</param>
        /// <param name="dt">出力データ</param>
        /// <param name="expInfoCollection">パラメータ</param>
        /// <param name="msgID">エラーメッセージID</param>
        /// <param name="args">エラーメッセージパラメータ</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2014/02/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool PutSimpleExcelPutHeader(XlsxCreator xlsx, DataTable dt, ExportInfoCollection expInfoCollection, ref string msgID, ref string[] args)
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

        /// --------------------------------------------------
        /// <summary>
        /// データの出力
        /// </summary>
        /// <param name="xlsx">ExcelCreator</param>
        /// <param name="dt">出力データ</param>
        /// <param name="expInfoCollection">パラメータ</param>
        /// <param name="msgID">エラーメッセージID</param>
        /// <param name="args">エラーメッセージパラメータ</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2014/02/19</create>
        /// <update>J.Chen 2023/12/13 小数点第2位まで対応修正</update>
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
                            if (expInfo.DataColName == Def_T_SHUKKA_MEISAI.GRWT)
                            {
                                // 重量の場合、小数点第2位まで表示
                                decimal value = ComFunc.GetFldToDecimal(dr, expInfo.DataColName);
                                xlsx.Pos(col, row).Value = (value == 0) ? "" : value.ToString("#,0.00");
                            }
                            else
                            {
                                xlsx.Pos(col, row).Value = ComFunc.GetFldObject(dr, expInfo.DataColName);
                            }
                            
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
        /// ヘッダーの罫線出力
        /// </summary>
        /// <param name="xlsx">ExcelCreator</param>
        /// <param name="dt">出力データ</param>
        /// <param name="expInfoCollection">パラメータ</param>
        /// <param name="msgID">エラーメッセージID</param>
        /// <param name="args">エラーメッセージパラメータ</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2014/02/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool PutSimpleExcelPutHeaderLine(XlsxCreator xlsx, DataTable dt, ExportInfoCollection expInfoCollection, ref string msgID, ref string[] args)
        {
            if (!expInfoCollection.IsHeader || !expInfoCollection.IsLine) return true;
            int colStart = expInfoCollection.StartCol;
            int rowStart = expInfoCollection.HeaderStartRow;
            
            int colEnd = xlsx.MaxData(xlMaxEndPoint.xarMaxPoint).Width;
            int rowEnd = expInfoCollection.HeaderStartRow;
            xlsx.Pos(colStart, rowStart, colEnd, rowEnd).Attr.Box(xlBoxType.xbtLtc, xlBorderStyle.xbsThin, xlColor.xclBlack);
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
        /// <create>T.Sakiori 2014/02/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool PutSimpleExcelPutDataLine(XlsxCreator xlsx, DataTable dt, ExportInfoCollection expInfoCollection, ref string msgID, ref string[] args)
        {
            if (!expInfoCollection.IsLine) return true;
            int colStart = expInfoCollection.StartCol;
            int rowStart = expInfoCollection.DataStartRow;

            Size size = xlsx.MaxData(xlMaxEndPoint.xarMaxPoint);
            int colEnd = size.Width;
            int rowEnd = size.Height;
            xlsx.Pos(colStart, rowStart, colEnd, rowEnd).Attr.Box(xlBoxType.xbtLtc, xlBorderStyle.xbsThin, xlColor.xclBlack);
            return true;
        }


        #endregion
    }

    #region ExportShareInfo
    
    /// --------------------------------------------------
    /// <summary>
    /// 共通データクラス
    /// </summary>
    /// <create>K.Tsutsumi 2012/07/05</create>
    /// <update>D.Okumura 2019/08/27 木枠まとめ対応</update>
    /// <update>J.Chen 2023/12/13 小数点第2位まで対応修正</update>
    /// --------------------------------------------------
    public class ExportShareInfo
    {

        #region 変数

        #region 共通

        private int _lineNo = 0;
        private int _pageNo = 0;
        private int _totalPageNo = 0;
        private string _kojiNo = string.Empty;
        private string _caseMarkImagePath = null;
        private double _rowHeight = 0;
        private double _colWidth = 0;
        private double _rate = 0;
        private double _bmpHeight = 0;
        private double _bmpWidth = 0;

        #endregion

        #region 梱包明細書専用

        private int _totalQty = 0;
        private decimal _totalMMNet = 0;
        private decimal _totalNetW = 0;
        private decimal _totalGrossW = 0;


        #endregion

        #region Master Packing List専用

        private string _printDate = string.Empty;

        #endregion

        #region Packing List専用

        private int _casePageNo = 0;

        #endregion

        #endregion

        #region プロパティ

        #region 共通

        #region 行数
        public int LineNo
        {
            get { return this._lineNo; }
            set { this._lineNo = value; }
        }
        #endregion
		 
        #region ページ数
        public int PageNo
        {
            get { return this._pageNo; }
            set { this._pageNo = value; }
        }
        #endregion

        #region 総ページ数
        public int TotalPageNo
        {
            get { return this._totalPageNo; }
            set { this._totalPageNo = value; }
        }
        #endregion

        #region 工事識別管理No
        public string KojiNo
        {
            get { return this._kojiNo; }
            set { this._kojiNo = value; }
        }
        #endregion

        #region CASE MARK Path
        public string CaseMarkImagePath
        {
            get { return this._caseMarkImagePath; }
            set { this._caseMarkImagePath = value; }
        }
        #endregion

        #region 行の高さ
        public double RowHeight
        {
            get { return this._rowHeight; }
            set { this._rowHeight = value; }
        }
        #endregion

        #region 列の幅
        public double ColWidth
        {
            get { return this._colWidth; }
            set { this._colWidth = value; }
        }
        #endregion

        #region イメージの比率
        public double Rate
        {
            get { return this._rate; }
            set { this._rate = value; }
        }
        #endregion

        #region イメージの高さ
        public double BmpHeight
        {
            get { return this._bmpHeight; }
            set { this._bmpHeight = value; }
        }
        #endregion

        #region イメージの幅
        public double BmpWidth
        {
            get { return this._bmpWidth; }
            set { this._bmpWidth = value; }
        }
        #endregion

        #endregion

        #region 梱包明細書専用

        #region QTY合計

        public int TotalQty
        {
            get { return this._totalQty; }
            set { this._totalQty = value; }
        }

        #endregion

        #region M'MNET合計

        public decimal TotalMMNet
        {
            get { return this._totalMMNet; }
            set { this._totalMMNet = value; }
        }

        #endregion

        #region NET/W合計

        public decimal TotalNetW
        {
            get { return this._totalNetW; }
            set { this._totalNetW = value; }
        }

        #endregion

        #region GROSS/W合計

        public decimal TotalGrossW
        {
            get { return this._totalGrossW; }
            set { this._totalGrossW = value; }
        }

        #endregion

        #endregion

        #region Master Packing List専用

        #region 印刷日付

        public string PrintDate
        {
            get { return this._printDate; }
            set { this._printDate = value; }
        }

        #endregion

        #endregion

        #region PackingList専用

        #region ケース単位ページ

        public int CasePageNo
        {
            get { return this._casePageNo; }
            set { this._casePageNo = value; }
        }

        #endregion

        #endregion

        #endregion

    }
    #endregion

}
