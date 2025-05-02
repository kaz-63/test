using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DataDynamics.ActiveReports;
using SystemBase;

namespace ActiveReportsHelper
{
    /// --------------------------------------------------
    /// <summary>
    /// レポートの作成、印刷、プレビューを行うためのクラス
    /// </summary>
    /// <create> 2010/06/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ReportHelper
    {
        #region Reportクラス取得

        /// --------------------------------------------------
        /// <summary>
        /// 帳票クラス名よりインスタンスを作成
        /// ※実行階層のDLLを全て読み込むのでクラス名(フル)は一意でなければ駄目です。
        /// </summary>
        /// <param name="reportClassName">帳票クラス名(フルで指定すること。)</param>
        /// <returns>ActiveReportsインスタンス</returns>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static object GetReport(string reportClassName)
        {
            TypeLoader loader = new TypeLoader();
            Type repType = loader.GetClassType(reportClassName, "*.dll");
            // クラスが取得出来ない場合は処理を抜ける
            if (repType == null) return null;

            object obj = repType.InvokeMember(null,
                                           System.Reflection.BindingFlags.CreateInstance,
                                           null,
                                           null,
                                           null);
            // BaseFormにキャスト
            return obj as ActiveReport;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 帳票クラス名よりインスタンスを作成
        /// ※実行階層のDLLを全て読み込むのでクラス名(フル)は一意でなければ駄目です。
        /// </summary>
        /// <param name="reportClassName">帳票クラス名(フルで指定すること。)</param>
        /// <param name="documentName">ドキュメント名</param>
        /// <returns>ActiveReportsインスタンス</returns>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static object GetReport(string reportClassName, string documentName)
        {
            object report = GetReport(reportClassName);
            SetDocumentName(ref report, documentName);
            return report;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 帳票クラス名よりインスタンスを作成
        /// ※実行階層のDLLを全て読み込むのでクラス名(フル)は一意でなければ駄目です。
        /// </summary>
        /// <param name="reportClassName">帳票クラス名(フルで指定すること。)</param>
        /// <param name="dt">DataSourceに使用するDataTable</param>
        /// <returns>ActiveReportsインスタンス</returns>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static object GetReport(string reportClassName, DataTable dt)
        {
            object report = GetReport(reportClassName);
            SetDataSource(ref report, dt);
            return report;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 帳票クラス名よりインスタンスを作成
        /// ※実行階層のDLLを全て読み込むのでクラス名(フル)は一意でなければ駄目です。
        /// </summary>
        /// <param name="reportClassName">帳票クラス名(フルで指定すること。)</param>
        /// <param name="documentName">ドキュメント名</param>
        /// <param name="dt">DataSourceに使用するDataTable</param>
        /// <returns>ActiveReportsインスタンス</returns>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static object GetReport(string reportClassName, string documentName, DataTable dt)
        {
            object report = GetReport(reportClassName);
            SetDataSource(ref report, dt);
            SetDocumentName(ref report, documentName);
            return report;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 帳票クラス名よりインスタンスを作成
        /// ※実行階層のDLLを全て読み込むのでクラス名(フル)は一意でなければ駄目です。
        /// </summary>
        /// <param name="reportClassName">帳票クラス名(フルで指定すること。)</param>
        /// <param name="ds">DataSourceに使用するDataSet</param>
        /// <param name="dataMember">DataMemberに設定する値</param>
        /// <returns>ActiveReportsインスタンス</returns>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static object GetReport(string reportClassName, DataSet ds, string dataMember)
        {
            object report = GetReport(reportClassName);
            SetDataSource(ref report, ds, dataMember);
            return report;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 帳票クラス名よりインスタンスを作成
        /// ※実行階層のDLLを全て読み込むのでクラス名(フル)は一意でなければ駄目です。
        /// </summary>
        /// <param name="reportClassName">帳票クラス名(フルで指定すること。)</param>
        /// <param name="documentName">ドキュメント名</param>
        /// <param name="ds">DataSourceに使用するDataSet</param>
        /// <param name="dataMember">DataMemberに設定する値</param>
        /// <returns>ActiveReportsインスタンス</returns>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static object GetReport(string reportClassName, string documentName, DataSet ds, string dataMember)
        {
            object report = GetReport(reportClassName);
            SetDataSource(ref report, ds, dataMember);
            SetDocumentName(ref report, documentName);
            return report;
        }

        #endregion

        #region ドキュメント名設定

        /// --------------------------------------------------
        /// <summary>
        /// ドキュメント名の設定
        /// </summary>
        /// <param name="report">ActiveReportsインスタンス</param>
        /// <param name="documentName">ドキュメント名</param>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static void SetDocumentName(ref object report, string documentName)
        {
            ActiveReport rep = report as ActiveReport;
            if (rep == null) return;
            rep.Document.Name = documentName;
        }

        #endregion

        #region DataSourceの設定

        /// --------------------------------------------------
        /// <summary>
        /// DataSourceの設定
        /// </summary>
        /// <param name="report">ActiveReportsインスタンス</param>
        /// <param name="dt">DataSourceに使用するDataTable</param>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static void SetDataSource(ref object report, DataTable dt)
        {
            ActiveReport rep = report as ActiveReport;
            if (rep == null) return;
            rep.DataSource = dt;
        }

        /// --------------------------------------------------
        /// <summary>
        /// DataSourceの設定
        /// </summary>
        /// <param name="report">ActiveReportsインスタンス</param>
        /// <param name="ds">DataSourceに使用するDataSet</param>
        /// <param name="dataMember">DataMemberに設定する値</param>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static void SetDataSource(ref object report, DataSet ds, string dataMember)
        {
            ActiveReport rep = report as ActiveReport;
            if (rep == null) return;
            rep.DataMember = dataMember;
            rep.DataSource = ds;
        }

        #endregion

        #region 印刷

        #region プリンタ固定

        /// --------------------------------------------------
        /// <summary>
        /// 印刷
        /// </summary>
        /// <param name="report">ActiveReportsインスタンス</param>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static void Print(object report)
        {
            Print(report, false, false, false);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 印刷
        /// </summary>
        /// <param name="reports">ActiveReportsインスタンスの配列</param>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static void Print(object[] reports)
        {
            Print(reports, false, false, false);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 印刷
        /// </summary>
        /// <param name="report">ActiveReportsインスタンス</param>
        /// <param name="showPrintDialog">Trueに設定すると、ドキュメントをプリンタに送信する前に、ユーザーがプリンタオプションを設定できる、印刷ダイアログを表示します。プリンタダイアログは、現在のDocument.Printerの設定で初期化されます。Falseに設定すると、指定したDocument.Printerの設定でプリンタにドキュメントが送信されます。</param>
        /// <param name="showPrintProgressDialog">Trueに設定すると、ユーザーが印刷ジョブを停止できる進行状況ダイアログが表示されます。</param>
        /// <param name="usePrintingThread">印刷が個別のスレッドにおいて行われるかどうかを指定します。</param>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static void Print(object report, bool showPrintDialog, bool showPrintProgressDialog, bool usePrintingThread)
        {
            Print(string.Empty, report, showPrintDialog, showPrintProgressDialog, usePrintingThread);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 印刷
        /// </summary>
        /// <param name="reports">ActiveReportsインスタンスの配列</param>
        /// <param name="showPrintDialog">Trueに設定すると、ドキュメントをプリンタに送信する前に、ユーザーがプリンタオプションを設定できる、印刷ダイアログを表示します。プリンタダイアログは、現在のDocument.Printerの設定で初期化されます。Falseに設定すると、指定したDocument.Printerの設定でプリンタにドキュメントが送信されます。</param>
        /// <param name="showPrintProgressDialog">Trueに設定すると、ユーザーが印刷ジョブを停止できる進行状況ダイアログが表示されます。</param>
        /// <param name="usePrintingThread">印刷が個別のスレッドにおいて行われるかどうかを指定します。</param>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static void Print(object[] reports, bool showPrintDialog, bool showPrintProgressDialog, bool usePrintingThread)
        {
            Print(string.Empty, reports, showPrintDialog, showPrintProgressDialog, usePrintingThread);
        }

        #endregion

        #region プリンタ切替

        /// --------------------------------------------------
        /// <summary>
        /// 印刷
        /// </summary>
        /// <param name="printerName">使用するプリンタ名</param>
        /// <param name="report">ActiveReportsインスタンス</param>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static void Print(string printerName, object report)
        {
            Print(printerName, report, false, false, false);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 印刷
        /// </summary>
        /// <param name="printerName">使用するプリンタ名</param>
        /// <param name="reports">ActiveReportsインスタンスの配列</param>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static void Print(string printerName, object[] reports)
        {
            Print(printerName, reports, false, false, false);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 印刷
        /// </summary>
        /// <param name="printerName">使用するプリンタ名</param>
        /// <param name="report">ActiveReportsインスタンス</param>
        /// <param name="showPrintDialog">Trueに設定すると、ドキュメントをプリンタに送信する前に、ユーザーがプリンタオプションを設定できる、印刷ダイアログを表示します。プリンタダイアログは、現在のDocument.Printerの設定で初期化されます。Falseに設定すると、指定したDocument.Printerの設定でプリンタにドキュメントが送信されます。</param>
        /// <param name="showPrintProgressDialog">Trueに設定すると、ユーザーが印刷ジョブを停止できる進行状況ダイアログが表示されます。</param>
        /// <param name="usePrintingThread">印刷が個別のスレッドにおいて行われるかどうかを指定します。</param>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static void Print(string printerName, object report, bool showPrintDialog, bool showPrintProgressDialog, bool usePrintingThread)
        {
            object[] reports = new object[] { report };
            Print(printerName, reports, false, false, false);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 印刷
        /// </summary>
        /// <param name="printerName">使用するプリンタ名</param>
        /// <param name="reports">ActiveReportsインスタンスの配列</param>
        /// <param name="showPrintDialog">Trueに設定すると、ドキュメントをプリンタに送信する前に、ユーザーがプリンタオプションを設定できる、印刷ダイアログを表示します。プリンタダイアログは、現在のDocument.Printerの設定で初期化されます。Falseに設定すると、指定したDocument.Printerの設定でプリンタにドキュメントが送信されます。</param>
        /// <param name="showPrintProgressDialog">Trueに設定すると、ユーザーが印刷ジョブを停止できる進行状況ダイアログが表示されます。</param>
        /// <param name="usePrintingThread">印刷が個別のスレッドにおいて行われるかどうかを指定します。</param>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static void Print(string printerName, object[] reports, bool showPrintDialog, bool showPrintProgressDialog, bool usePrintingThread)
        {
            ActiveReport allRep = null;
            foreach (object report in reports)
            {
                ActiveReport rep = report as ActiveReport;
                if (allRep == null)
                {
                    allRep = rep;
                }
                if (rep == null) return;
                if (!string.IsNullOrEmpty(printerName))
                {
                    rep.Document.Printer.PrinterSettings.PrinterName = printerName;
                }
                rep.Run(false);
                if (!rep.Equals(allRep))
                {
                    allRep.Document.Pages.AddRange(rep.Document.Pages);
                }
                //rep.Document.Print(showPrintDialog, showPrintProgressDialog, usePrintingThread);
            }
            allRep.Document.Print(showPrintDialog, showPrintProgressDialog, usePrintingThread);
        }

        #endregion

        #endregion

        #region プレビュー

        #region プリンタ固定

        /// --------------------------------------------------
        /// <summary>
        /// プレビュー
        /// </summary>
        /// <param name="owner">親ウィンドウ</param>
        /// <param name="report">ActiveReportsインスタンス</param>
        /// <returns>true:印刷した/false:印刷していない</returns>
        /// <create>Y.Higuchi 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool Preview(IWin32Window owner, object report)
        {
            object[] reports = new object[] { report };
            return Preview(owner, reports);
        }

        /// --------------------------------------------------
        /// <summary>
        /// プレビュー
        /// </summary>
        /// <param name="owner">親ウィンドウ</param>
        /// <param name="report">ActiveReportsインスタンス</param>
        /// <param name="showPrintButton">印刷ボタンを表示するかどうか</param>
        /// <returns>true:印刷した/false:印刷していない</returns>
        /// <create>Y.Higuchi 2010/07/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool Preview(IWin32Window owner, object report, bool showPrintButton)
        {
            object[] reports = new object[] { report };
            return Preview(owner, reports, showPrintButton);
        }

        /// --------------------------------------------------
        /// <summary>
        /// プレビュー(印刷ボタンなし)
        /// </summary>
        /// <param name="owner">親ウィンドウ</param>
        /// <param name="report">ActiveReportsインスタンス</param>
        /// <param name="prinSetting">プレビュー用プリンタ設定</param>
        /// <returns>true:印刷した/false:印刷していない</returns>
        /// <create>Y.Higuchi 2010/07/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool Preview(IWin32Window owner, object report, PreviewPrinterSetting prinSetting)
        {
            object[] reports = new object[] { report };
            return Preview(owner, reports, false, prinSetting);
        }

        /// --------------------------------------------------
        /// <summary>
        /// プレビュー
        /// </summary>
        /// <param name="owner">親ウィンドウ</param>
        /// <param name="report">ActiveReportsインスタンス</param>
        /// <param name="showPrintButton">印刷ボタンを表示するかどうか</param>
        /// <param name="prinSetting">プレビュー用プリンタ設定</param>
        /// <returns>true:印刷した/false:印刷していない</returns>
        /// <create>Y.Higuchi 2010/07/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool Preview(IWin32Window owner, object report, bool showPrintButton, PreviewPrinterSetting prinSetting)
        {
            object[] reports = new object[] { report };
            return Preview(owner, reports, showPrintButton, prinSetting);
        }

        /// --------------------------------------------------
        /// <summary>
        /// プレビュー(印刷ボタンなし)
        /// </summary>
        /// <param name="owner">親ウィンドウ</param>
        /// <param name="reports">ActiveReportsインスタンスの配列</param>
        /// <returns>true:印刷した/false:印刷していない</returns>
        /// <create>Y.Higuchi 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool Preview(IWin32Window owner, object[] reports)
        {
            return Preview(owner, string.Empty, reports);
        }

        /// --------------------------------------------------
        /// <summary>
        /// プレビュー
        /// </summary>
        /// <param name="owner">親ウィンドウ</param>
        /// <param name="reports">ActiveReportsインスタンスの配列</param>
        /// <param name="showPrintButton">印刷ボタンを表示するかどうか</param>
        /// <returns>true:印刷した/false:印刷していない</returns>
        /// <create>Y.Higuchi 2010/07/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool Preview(IWin32Window owner, object[] reports, bool showPrintButton)
        {
            return Preview(owner, string.Empty, reports, showPrintButton);
        }

        /// --------------------------------------------------
        /// <summary>
        /// プレビュー(印刷ボタンなし)
        /// </summary>
        /// <param name="owner">親ウィンドウ</param>
        /// <param name="reports">ActiveReportsインスタンスの配列</param>
        /// <param name="prinSetting">プレビュー用プリンタ設定</param>
        /// <returns>true:印刷した/false:印刷していない</returns>
        /// <create>Y.Higuchi 2010/07/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool Preview(IWin32Window owner, object[] reports, PreviewPrinterSetting prinSetting)
        {
            return Preview(owner, string.Empty, reports, false, prinSetting);
        }

        /// --------------------------------------------------
        /// <summary>
        /// プレビュー
        /// </summary>
        /// <param name="owner">親ウィンドウ</param>
        /// <param name="reports">ActiveReportsインスタンスの配列</param>
        /// <param name="showPrintButton">印刷ボタンを表示するかどうか</param>
        /// <param name="prinSetting">プレビュー用プリンタ設定</param>
        /// <returns>true:印刷した/false:印刷していない</returns>
        /// <create>Y.Higuchi 2010/07/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool Preview(IWin32Window owner, object[] reports, bool showPrintButton, PreviewPrinterSetting prinSetting)
        {
            return Preview(owner, string.Empty, reports, showPrintButton, prinSetting);
        }

        #endregion

        #region プリンタ切替

        /// --------------------------------------------------
        /// <summary>
        /// プレビュー(印刷ボタンなし)
        /// </summary>
        /// <param name="owner">親ウィンドウ</param>
        /// <param name="printerName">使用するプリンタ名</param>
        /// <param name="report">ActiveReportsインスタンス</param>
        /// <returns>true:印刷した/false:印刷していない</returns>
        /// <create>Y.Higuchi 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool Preview(IWin32Window owner, string printerName, object report)
        {
            object[] reports = new object[] { report };
            return Preview(owner, printerName, reports);
        }

        /// --------------------------------------------------
        /// <summary>
        /// プレビュー
        /// </summary>
        /// <param name="owner">親ウィンドウ</param>
        /// <param name="printerName">使用するプリンタ名</param>
        /// <param name="report">ActiveReportsインスタンス</param>
        /// <param name="showPrintButton">印刷ボタンを表示するかどうか</param>
        /// <returns>true:印刷した/false:印刷していない</returns>
        /// <create>Y.Higuchi 2010/07/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool Preview(IWin32Window owner, string printerName, object report, bool showPrintButton)
        {
            object[] reports = new object[] { report };
            return Preview(owner, printerName, reports, showPrintButton);
        }

        /// --------------------------------------------------
        /// <summary>
        /// プレビュー(印刷ボタンなし)
        /// </summary>
        /// <param name="owner">親ウィンドウ</param>
        /// <param name="printerName">使用するプリンタ名</param>
        /// <param name="report">ActiveReportsインスタンス</param>
        /// <param name="prinSetting">プレビュー用プリンタ設定</param>
        /// <returns>true:印刷した/false:印刷していない</returns>
        /// <create>Y.Higuchi 2010/07/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool Preview(IWin32Window owner, string printerName, object report, PreviewPrinterSetting prinSetting)
        {
            object[] reports = new object[] { report };
            return Preview(owner, printerName, reports, false, prinSetting);
        }

        /// --------------------------------------------------
        /// <summary>
        /// プレビュー
        /// </summary>
        /// <param name="owner">親ウィンドウ</param>
        /// <param name="printerName">使用するプリンタ名</param>
        /// <param name="report">ActiveReportsインスタンス</param>
        /// <param name="showPrintButton">印刷ボタンを表示するかどうか</param>
        /// <param name="prinSetting">プレビュー用プリンタ設定</param>
        /// <returns>true:印刷した/false:印刷していない</returns>
        /// <create>Y.Higuchi 2010/07/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool Preview(IWin32Window owner, string printerName, object report, bool showPrintButton, PreviewPrinterSetting prinSetting)
        {
            object[] reports = new object[] { report };
            return Preview(owner, printerName, reports, showPrintButton, prinSetting);
        }

        /// --------------------------------------------------
        /// <summary>
        /// プレビュー(印刷ボタンなし)
        /// </summary>
        /// <param name="owner">親ウィンドウ</param>
        /// <param name="printerName">使用するプリンタ名</param>
        /// <param name="reports">ActiveReportsインスタンスの配列</param>
        /// <returns>true:印刷した/false:印刷していない</returns>
        /// <create>Y.Higuchi 2010/07/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool Preview(IWin32Window owner, string printerName, object[] reports)
        {
            return Preview(owner, printerName, reports, false);
        }

        /// --------------------------------------------------
        /// <summary>
        /// プレビュー
        /// </summary>
        /// <param name="owner">親ウィンドウ</param>
        /// <param name="printerName">使用するプリンタ名</param>
        /// <param name="reports">ActiveReportsインスタンスの配列</param>
        /// <param name="showPrintButton">印刷ボタンを表示するかどうか</param>
        /// <returns>true:印刷した/false:印刷していない</returns>
        /// <create>Y.Higuchi 2010/07/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool Preview(IWin32Window owner, string printerName, object[] reports, bool showPrintButton)
        {
            return Preview(owner, printerName, reports, false, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// プレビュー(印刷ボタンなし)
        /// </summary>
        /// <param name="owner">親ウィンドウ</param>
        /// <param name="printerName">使用するプリンタ名</param>
        /// <param name="reports">ActiveReportsインスタンスの配列</param>
        /// <param name="prinSetting">プレビュー用プリンタ設定</param>
        /// <returns>true:印刷した/false:印刷していない</returns>
        /// <create>Y.Higuchi 2010/07/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool Preview(IWin32Window owner, string printerName, object[] reports, PreviewPrinterSetting prinSetting)
        {
            return Preview(owner, printerName, reports, false, prinSetting);
        }

        /// --------------------------------------------------
        /// <summary>
        /// プレビュー
        /// </summary>
        /// <param name="owner">親ウィンドウ</param>
        /// <param name="printerName">使用するプリンタ名</param>
        /// <param name="reports">ActiveReportsインスタンスの配列</param>
        /// <param name="showPrintButton">印刷ボタンを表示するかどうか</param>
        /// <param name="prinSetting">プレビュー用プリンタ設定</param>
        /// <returns>true:印刷した/false:印刷していない</returns>
        /// <create>Y.Higuchi 2010/07/07</create>
        /// <update>Y.Higuchi 2010/07/09</update>
        /// --------------------------------------------------
        public static bool Preview(IWin32Window owner, string printerName, object[] reports, bool showPrintButton, PreviewPrinterSetting prinSetting)
        {
            using (ReportPreview frm = new ReportPreview())
            {
                bool isExists = false;
                if (prinSetting != null)
                {
                    frm.Document.Printer.PaperKind = prinSetting.PaperKind;
                    frm.Document.Printer.Landscape = prinSetting.Landscape;
                }
                foreach (object report in reports)
                {
                    ActiveReport rep = report as ActiveReport;
                    if (rep == null) continue;
                    if (!string.IsNullOrEmpty(printerName))
                    {
                        rep.Document.Printer.PrinterSettings.PrinterName = printerName;
                    }
                    rep.Run(false);
                    frm.Document.Pages.AddRange(rep.Document.Pages);
                    isExists = true;
                }
                if (isExists)
                {
                    frm.ShowPrintButton = showPrintButton;
                    if (!string.IsNullOrEmpty(printerName))
                    {
                        frm.Document.Printer.PrinterSettings.PrinterName = printerName;
                    }
                    frm.ShowDialog(owner);
                }
                return frm.IsPrint;
            }
        }

        #endregion

        #endregion
    }
}
