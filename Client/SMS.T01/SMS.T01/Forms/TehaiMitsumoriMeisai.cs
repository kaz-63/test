using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DSWUtil;
using DSWControl.DSWComboBox;
using WsConnection;
using WsConnection.WebRefCommon;
using Commons;
using System.Linq;
using SystemBase.Controls;
using GrapeCity.Win.ElTabelle;
using ElTabelleHelper;
using WsConnection.WebRefT01;
using SMS.T01.Properties;
using SystemBase.Util;
using SMS.E01;

namespace SMS.T01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 手配見積明細
    /// </summary>
    /// <create>S.Furugo 2018/11/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class TehaiMitsumoriMeisai : SystemBase.Forms.CustomOrderForm
    {

        #region Fields
        /// --------------------------------------------------
        /// <summary>
        /// 退避データ
        /// </summary>
        /// <create>S.Furugo 2018/11/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtTehaiMitsumoriListData = null;
        /// --------------------------------------------------
        /// <summary>
        /// 追加モードフラグ
        /// </summary>
        /// --------------------------------------------------
        private bool _addModeFlag = false;
        /// --------------------------------------------------
        /// <summary>
        /// 見積状態
        /// </summary>
        /// <create>D.Okumura 2018/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private EstimateMode EstimateStatus { get; set; }
        /// --------------------------------------------------
        /// <summary>
        /// 見積チェック用 - 図番配列
        /// </summary>
        /// <create>J.Chen 2024/04/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private List<string> listZuban = new List<string>();
        /// --------------------------------------------------
        /// <summary>
        /// 見積チェック用 - 品名配列
        /// </summary>
        /// <create>J.Chen 2024/04/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private List<string> listHinmei = new List<string>();
        /// --------------------------------------------------
        /// <summary>
        /// 見積チェック用 - 単価配列
        /// </summary>
        /// <create>J.Chen 2024/04/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private List<double?> listUnitPrice = new List<double?>();
        /// --------------------------------------------------
        /// <summary>
        /// 履歴更新フラグ
        /// </summary>
        /// <create>J.Chen 2024/10/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isRirekiUpdate = false;
        /// --------------------------------------------------
        /// <summary>
        /// 更新比較用dt
        /// </summary>
        /// <create>J.Chen 2024/10/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _tempDt = null;

        #endregion

        #region Enum
        /// --------------------------------------------------
        /// <summary>
        /// 画面の表示モード
        /// </summary>
        /// <create>S.Furugo 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DisplayMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 初期
            /// </summary>
            /// <create>S.Furugo 2018/11/22</create>
            /// <update></update>
            /// --------------------------------------------------
            Initialize,
            /// --------------------------------------------------
            /// <summary>
            /// 検索後
            /// </summary>
            /// <create>S.Furugo 2018/11/22</create>
            /// <update></update>
            /// --------------------------------------------------
            EndSearch,
        }

        /// --------------------------------------------------
        /// <summary>
        /// 見積モード
        /// </summary>
        /// <create>D.Okumura 2018/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        protected enum EstimateMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 未設定
            /// </summary>
            /// <create>D.Okumura 2018/12/19</create>
            /// <update></update>
            /// --------------------------------------------------
            Neutral,
            /// --------------------------------------------------
            /// <summary>
            /// 無償
            /// </summary>
            /// <create>D.Okumura 2018/12/19</create>
            /// <update></update>
            /// --------------------------------------------------
            Gratis,
            /// --------------------------------------------------
            /// <summary>
            /// 有償
            /// </summary>
            /// <create>D.Okumura 2018/12/19</create>
            /// <update></update>
            /// --------------------------------------------------
            Onerous,
        }
        #endregion

        #region 定数

        #region 列定義

        /// --------------------------------------------------
        /// <summary>
        /// チェックボックスの列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_CHECK = 0;
        /// --------------------------------------------------
        /// <summary>
        /// 手配連携No.の列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/11/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TEHAI_RENKEI_NO = 1;
        /// --------------------------------------------------
        /// <summary>
        /// 設定納期の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SETTEI_DATE = 2;
        /// --------------------------------------------------
        /// <summary>
        /// 納品先の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_NOHIN_SAKI = 3;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷先の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SYUKKA_SAKI = 4;
        /// --------------------------------------------------
        /// <summary>
        /// 製番の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SEIBAN = 5;
        /// --------------------------------------------------
        /// <summary>
        /// CODEの列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_CODE = 6;
        /// --------------------------------------------------
        /// <summary>
        /// 追番の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ZUMEN_OIBAN = 7;
        /// --------------------------------------------------
        /// <summary>
        /// AR No.の列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_AR_NO = 8;
        /// --------------------------------------------------
        /// <summary>
        /// ECS no.の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ECS_NO = 9;
        /// --------------------------------------------------
        /// <summary>
        /// Floorの列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_FLOOR = 10;
        /// --------------------------------------------------
        /// <summary>
        /// 機種の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_KISHU = 11;
        /// --------------------------------------------------
        /// <summary>
        /// ST No.の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ST_NO = 12;
        /// --------------------------------------------------
        /// <summary>
        /// 品名(和文)の列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_HINMEI_JP = 13;
        /// --------------------------------------------------
        /// <summary>
        /// 品名の列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_HINMEI = 14;
        /// --------------------------------------------------
        /// <summary>
        /// 品名（INV）の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_HINMEI_INV = 15;
        /// --------------------------------------------------
        /// <summary>
        /// 図番/型式の列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ZUMEN_KEISHIKI = 16;
        /// --------------------------------------------------
        /// <summary>
        /// 手配数の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TEHAI_QTY = 17;
        /// --------------------------------------------------
        /// <summary>
        /// 手配区分の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TEHAI_FLAG_NAME = 18;
        /// --------------------------------------------------
        /// <summary>
        /// 発注数の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_HACCHU_QTY = 19;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷数の列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SHUKKA_QTY = 20;
        /// --------------------------------------------------
        /// <summary>
        /// Free1の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_FREE1 = 21;
        /// --------------------------------------------------
        /// <summary>
        /// Free2の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_FREE2 = 22;
        /// --------------------------------------------------
        /// <summary>
        /// 数量単位の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_QUANTITY_UNIT = 23;
        /// --------------------------------------------------
        /// <summary>
        /// 図番/型式2の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ZUMEN_KEISHIKI2 = 24;
        /// --------------------------------------------------
        /// <summary>
        /// 備考の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_NOTE = 25;
        /// --------------------------------------------------
        /// <summary>
        /// 通関確認状態の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_CUSTOMS_STATUS = 26;
        /// --------------------------------------------------
        /// <summary>
        /// Makerの列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update>J.Chen 2024/11/08 通関確認状態追加のため、インデックス変更（+1）</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_MAKER = 27;
        /// --------------------------------------------------
        /// <summary>
        /// 手配No.の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update>J.Chen 2024/11/08 通関確認状態追加のため、インデックス変更（+1）</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TEHAI_NO = 28;
        /// --------------------------------------------------
        /// <summary>
        /// 単価(JPY)の列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/12/05</create>
        /// <update>J.Chen 2024/02/07 項目追加のため、インデックス変更（27→28）</update>
        /// <update>J.Chen 2024/11/08 通関確認状態追加のため、インデックス変更（+1）</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_UNIT_PRICE = 29;
        /// --------------------------------------------------
        /// <summary>
        /// 有償/無償の列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update>J.Chen 2024/02/07 項目追加のため、インデックス変更（28→29）</update>
        /// <update>J.Chen 2024/11/08 通関確認状態追加のため、インデックス変更（+1）</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ESTIMATE_FLAG_NAME = 30;
        /// --------------------------------------------------
        /// <summary>
        /// 通貨の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update>J.Chen 2024/11/08 通関確認状態追加のため、インデックス変更（+1）</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TSUUKA = 31;
        /// --------------------------------------------------
        /// <summary>
        /// ER（JPY）の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update>J.Chen 2024/11/08 通関確認状態追加のため、インデックス変更（+1）</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ER_JPY = 32;
        /// --------------------------------------------------
        /// <summary>
        /// 販管費（％）の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update>J.Chen 2024/11/08 通関確認状態追加のため、インデックス変更（+1）</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_HANKANHI = 33;
        /// --------------------------------------------------
        /// <summary>
        /// 運賃（％）の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update>J.Chen 2024/11/08 通関確認状態追加のため、インデックス変更（+1）</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_UNCHIN = 34;
        /// --------------------------------------------------
        /// <summary>
        /// Inv単価の列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/11/27</create>
        /// <update>J.Chen 2024/02/07 項目追加のため、インデックス変更（33→34）</update>
        /// <update>J.Chen 2024/11/08 通関確認状態追加のため、インデックス変更（+1）</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_INV_UNIT_PRICE = 35;
        /// --------------------------------------------------
        /// <summary>
        /// INV Valueの列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update>J.Chen 2024/11/08 通関確認状態追加のため、インデックス変更（+1）</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_INV_VALUE = 36;
        /// --------------------------------------------------
        /// <summary>
        /// 見積No.の列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update>J.Chen 2024/02/07 項目追加のため、インデックス変更（34→36）</update>
        /// <update>J.Chen 2024/11/08 通関確認状態追加のため、インデックス変更（+1）</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ESTIMATE_NO = 37;
        /// --------------------------------------------------
        /// <summary>
        /// 見積名称の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update>J.Chen 2024/11/08 通関確認状態追加のため、インデックス変更（+1）</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ESTIMATE_NAME = 38;
        /// --------------------------------------------------
        /// <summary>
        /// PO No.の列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update>J.Chen 2024/02/07 項目追加のため、インデックス変更（36→38）</update>
        /// <update>J.Chen 2024/11/08 通関確認状態追加のため、インデックス変更（+1）</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_PO_NO = 39;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷制限の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update>J.Chen 2024/11/08 通関確認状態追加のため、インデックス変更（+1）</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SHUKKASEIGEN = 40;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷状況の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update>J.Chen 2024/11/08 通関確認状態追加のため、インデックス変更（+1）</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SHUKKAJYOTAI = 41;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷日の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update>J.Chen 2024/11/08 通関確認状態追加のため、インデックス変更（+1）</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SHUKKA_DATE = 42;
        /// --------------------------------------------------
        /// <summary>
        /// 便名の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update>J.Chen 2024/11/08 通関確認状態追加のため、インデックス変更（+1）</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SHIP = 43;
        /// --------------------------------------------------
        /// <summary>
        /// TAG No.の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update>J.Chen 2024/11/08 通関確認状態追加のため、インデックス変更（+1）</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TAG_NO = 44;
        /// --------------------------------------------------
        /// <summary>
        /// Invoice No.の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update>J.Chen 2024/11/08 通関確認状態追加のため、インデックス変更（+1）</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_INVOICE_NO = 45;
        /// --------------------------------------------------
        /// <summary>
        /// 変更履歴の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/10/29</create>
        /// <update>J.Chen 2024/11/08 通関確認状態追加のため、インデックス変更（+1）</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ESTIMATE_RIREKI = 46;
        /// --------------------------------------------------
        /// <summary>
        /// バージョンの列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/12/10</create>
        /// <update>J.Chen 2024/02/07 項目追加のため、インデックス変更（37→45）</update>
        /// <update>J.Chen 2024/10/29 変更履歴追加のため、インデックス変更（+1）</update>
        /// <update>J.Chen 2024/11/08 通関確認状態追加のため、インデックス変更（+1）</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_VERSION = 47;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷済み明細件数の列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update>J.Chen 2024/02/07 項目追加のため、インデックス変更（38→46）</update>
        /// <update>J.Chen 2024/10/29 変更履歴追加のため、インデックス変更（+1）</update>
        /// <update>J.Chen 2024/11/08 通関確認状態追加のため、インデックス変更（+1）</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_CNT = 48;
        /// --------------------------------------------------
        /// <summary>
        /// 見積状態
        /// </summary>
        /// <create>D.Okumura 2018/12/19</create>
        /// <update>J.Chen 2024/02/07 項目追加のため、インデックス変更（39→47）</update>
        /// <update>J.Chen 2024/10/29 変更履歴追加のため、インデックス変更（+1）</update>
        /// <update>J.Chen 2024/11/08 通関確認状態追加のため、インデックス変更（+1）</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ESTIMATE_FLAG = 49;
        /// --------------------------------------------------
        /// <summary>
        /// 見積状態色
        /// </summary>
        /// <create>D.Okumura 2018/12/19</create>
        /// <update>J.Chen 2024/02/07 項目追加のため、インデックス変更（40→48）</update>
        /// <update>J.Chen 2024/10/29 変更履歴追加のため、インデックス変更（+1）</update>
        /// <update>J.Chen 2024/11/08 通関確認状態追加のため、インデックス変更（+1）</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ESTIMATE_COLOR = 50;
        /// --------------------------------------------------
        /// <summary>
        /// 期の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/07</create>
        /// <update>J.Chen 2024/10/29 変更履歴追加のため、インデックス変更（+1）</update>
        /// <update>J.Chen 2024/11/08 通関確認状態追加のため、インデックス変更（+1）</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ECS_QUOTA = 51;

        #endregion

        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 0;
        /// --------------------------------------------------
        /// <summary>
        /// チェックボックスONの値
        /// </summary>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int CHECK_ON = 1;
        private const string CHECK_ON_STR = "1";
        /// --------------------------------------------------
        /// <summary>
        /// チェックボックスOFFの値
        /// </summary>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int CHECK_OFF = 0;
        /// --------------------------------------------------
        /// <summary>
        /// 行ヘッダー色（DAD9EE）
        /// </summary>
        /// <create>J.Chen 2024/02/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private Color ROWHEADERS_COLOR = Color.FromArgb(255, 218, 217, 238);
        #endregion

        #region フィルター

        /// --------------------------------------------------
        /// <summary>
        /// AutoFilter対象列
        /// </summary>
        /// <create>J.Chen 2024/02/09</create>
        /// <update>J.Chen 2024/10/29 履歴更新列追加</update>
        /// <update>J.Chen 2024/11/08 通関確認状態追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private int[] _autoFilterColumns = new int[] {
            SHEET_COL_TEHAI_RENKEI_NO, SHEET_COL_SETTEI_DATE, SHEET_COL_NOHIN_SAKI, SHEET_COL_SYUKKA_SAKI, 
            SHEET_COL_SEIBAN, SHEET_COL_CODE, SHEET_COL_ZUMEN_OIBAN, SHEET_COL_AR_NO, SHEET_COL_ECS_NO, SHEET_COL_FLOOR, 
            SHEET_COL_KISHU, SHEET_COL_ST_NO, SHEET_COL_HINMEI_JP, SHEET_COL_HINMEI, SHEET_COL_HINMEI_INV, SHEET_COL_ZUMEN_KEISHIKI, 
            SHEET_COL_TEHAI_QTY, SHEET_COL_TEHAI_FLAG_NAME, SHEET_COL_HACCHU_QTY, SHEET_COL_SHUKKA_QTY, SHEET_COL_FREE1, SHEET_COL_FREE2,
            SHEET_COL_QUANTITY_UNIT, SHEET_COL_ZUMEN_KEISHIKI2, SHEET_COL_NOTE, SHEET_COL_CUSTOMS_STATUS, SHEET_COL_MAKER, SHEET_COL_TEHAI_NO, SHEET_COL_UNIT_PRICE, 
            SHEET_COL_ESTIMATE_FLAG_NAME, SHEET_COL_TSUUKA, SHEET_COL_ER_JPY, SHEET_COL_HANKANHI, SHEET_COL_UNCHIN, SHEET_COL_INV_UNIT_PRICE, 
            SHEET_COL_INV_VALUE, SHEET_COL_ESTIMATE_NO, SHEET_COL_ESTIMATE_NAME, SHEET_COL_PO_NO, SHEET_COL_SHUKKASEIGEN, 
            SHEET_COL_SHUKKAJYOTAI, SHEET_COL_SHUKKA_DATE, SHEET_COL_SHIP, SHEET_COL_TAG_NO, SHEET_COL_INVOICE_NO, SHEET_COL_ESTIMATE_RIREKI
        };

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ(デザイナ用) 
        /// </summary>
        /// <create>D.Okumura 2018/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        [Obsolete("For designer", true)]
        public TehaiMitsumoriMeisai()
            : this(null, false, null)
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="menuCategoryID">メニュー種別ID</param>
        /// <param name="menuItemID">メニュー項目ID</param>
        /// <param name="title">画面タイトル</param>
        /// <create>S.Furugo 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public TehaiMitsumoriMeisai(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
            : base(userInfo, menuCategoryID, menuItemID, title)
        {
            InitializeComponent();
        }
        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ(追加モード用)
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="addFlag">追加モードフラグ</param>
        /// <param name="title">画面タイトル</param>
        /// --------------------------------------------------
        public TehaiMitsumoriMeisai(UserInfo userInfo, bool addFlag, string title)
            : this(userInfo, null, null, title)
        {
            this._addModeFlag = addFlag;
        }

        #endregion

        #region 初期化

        #region コントロール初期化
        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>S.Furugo 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();
            
            try
            {
                // フォームの設定
                this.IsCloseQuestion = true;
                this.IsRunEditAfterClear = false;

                // 編集モードの設定: F1は見積ボタンなので、基底で何もさせない
                this.EditMode = SystemBase.EditMode.None;

                // コンボボックス
                // フォームクリア処理内で設定

                // フォームの状態を初期化
                this.DisplayClearAll();

                // シート設定
                this.InitializeSheet(shtTehaiMeisai);
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

        }

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示されるたびにコントロールの初期化するメソッド
        /// </summary>
        /// <create>S.Furugo 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                this.cboBukkenName.Focus();
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }
        #endregion

        #region シート初期化

        /// --------------------------------------------------
        /// <summary>
        /// シートの初期化
        /// </summary>
        /// <param name="sheet"></param>
        /// <create>D.Okumura 2018/12/19</create>
        /// <update>J.Chen 2024/02/09 表示データ追加</update>
        /// <update>J.Chen 2024/10/29 履歴更新列追加</update>
        /// <update>J.Chen 2024/11/08 通関確認状態追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeSheet(Sheet sheet)
        {
            base.InitializeSheet(sheet);
            sheet.KeepHighlighted = true;
            sheet.RowHeaders.AllowResize = false;

            // シートのタイトルを設定
            sheet.ColumnHeaders[0].Caption = "";
            sheet.ColumnHeaders[1].Caption = Resources.TehaiMitsumoriMeisai_RenkaiNo;
            sheet.ColumnHeaders[2].Caption = Resources.TehaiMitsumoriMeisai_SetteiDate;
            sheet.ColumnHeaders[3].Caption = Resources.TehaiMitsumoriMeisai_Nouhinsaki;
            sheet.ColumnHeaders[4].Caption = Resources.TehaiMitsumoriMeisai_Shukkasaki;
            sheet.ColumnHeaders[5].Caption = Resources.TehaiMitsumoriMeisai_Seiban;
            sheet.ColumnHeaders[6].Caption = Resources.TehaiMitsumoriMeisai_Code;
            sheet.ColumnHeaders[7].Caption = Resources.TehaiMitsumoriMeisai_Oiban;
            sheet.ColumnHeaders[8].Caption = Resources.TehaiMitsumoriMeisai_ARNo;
            sheet.ColumnHeaders[9].Caption = Resources.TehaiMitsumoriMeisai_ECSNo;
            sheet.ColumnHeaders[10].Caption = Resources.TehaiMitsumoriMeisai_Floor;
            sheet.ColumnHeaders[11].Caption = Resources.TehaiMitsumoriMeisai_Kishu;
            sheet.ColumnHeaders[12].Caption = Resources.TehaiMitsumoriMeisai_STNo;
            sheet.ColumnHeaders[13].Caption = Resources.TehaiMitsumoriMeisai_HinmeiJP;
            sheet.ColumnHeaders[14].Caption = Resources.TehaiMitsumoriMeisai_Hinmei;
            sheet.ColumnHeaders[15].Caption = Resources.TehaiMitsumoriMeisai_HinmeiINV;
            sheet.ColumnHeaders[16].Caption = Resources.TehaiMitsumoriMeisai_ZumenKeishiki;
            sheet.ColumnHeaders[17].Caption = Resources.TehaiMitsumoriMeisai_TehaiQty;
            sheet.ColumnHeaders[18].Caption = Resources.TehaiMitsumoriMeisai_TehaiFlag;
            sheet.ColumnHeaders[19].Caption = Resources.TehaiMitsumoriMeisai_HacchuQty;
            sheet.ColumnHeaders[20].Caption = Resources.TehaiMitsumoriMeisai_ShukkaQty;
            sheet.ColumnHeaders[21].Caption = Resources.TehaiMitsumoriMeisai_Free1;
            sheet.ColumnHeaders[22].Caption = Resources.TehaiMitsumoriMeisai_Free2;
            sheet.ColumnHeaders[23].Caption = Resources.TehaiMitsumoriMeisai_QuantityUnit;
            sheet.ColumnHeaders[24].Caption = Resources.TehaiMitsumoriMeisai_ZumenKeishiki2;
            sheet.ColumnHeaders[25].Caption = Resources.TehaiMitsumoriMeisai_Note;
            sheet.ColumnHeaders[26].Caption = Resources.TehaiMitsumoriMeisai_CustomsStatus;
            sheet.ColumnHeaders[27].Caption = Resources.TehaiMitsumoriMeisai_Maker;
            sheet.ColumnHeaders[28].Caption = Resources.TehaiMitsumoriMeisai_TehaiNo;
            sheet.ColumnHeaders[29].Caption = Resources.TehaiMitsumoriMeisai_UnitPrice;
            sheet.ColumnHeaders[30].Caption = Resources.TehaiMitsumoriMeisai_EstimateFlag;
            sheet.ColumnHeaders[31].Caption = Resources.TehaiMitsumoriMeisai_CurencyFlag;
            sheet.ColumnHeaders[32].Caption = Resources.TehaiMitsumoriMeisai_RateJPY;
            sheet.ColumnHeaders[33].Caption = Resources.TehaiMitsumoriMeisai_SalsesPer;
            sheet.ColumnHeaders[34].Caption = Resources.TehaiMitsumoriMeisai_RobPer;
            sheet.ColumnHeaders[35].Caption = Resources.TehaiMitsumoriMeisai_InvUnitPrice;
            sheet.ColumnHeaders[36].Caption = Resources.TehaiMitsumoriMeisai_InvoiceValue;
            sheet.ColumnHeaders[37].Caption = Resources.TehaiMitsumoriMeisai_EstimateNo;
            sheet.ColumnHeaders[38].Caption = Resources.TehaiMitsumoriMeisai_EstimateName;
            sheet.ColumnHeaders[39].Caption = Resources.TehaiMitsumoriMeisai_PONo;
            sheet.ColumnHeaders[40].Caption = Resources.TehaiMitsumoriMeisai_HenkyakuhinFlag;
            sheet.ColumnHeaders[41].Caption = Resources.TehaiMitsumoriMeisai_JyotaiName;
            sheet.ColumnHeaders[42].Caption = Resources.TehaiMitsumoriMeisai_ShukkaDate;
            sheet.ColumnHeaders[43].Caption = Resources.TehaiMitsumoriMeisai_Ship;
            sheet.ColumnHeaders[44].Caption = Resources.TehaiMitsumoriMeisai_TagNo;
            sheet.ColumnHeaders[45].Caption = Resources.TehaiMitsumoriMeisai_InvoiceNo;
            sheet.ColumnHeaders[46].Caption = Resources.TehaiMitsumoriMeisai_EstimateRireki;

            this.SetEnableAutoFilter(false);
        }
        #endregion

        #region コンボボックスの設定

        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスの設定
        /// </summary>
        /// <create>S.Furugo 2018/11/16</create>
        /// <update>J.Chen 2024/02/13 表示選択追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private void SetComboBox()
        {
            // プロジェクトマスタより物件一覧取得
            ConnT01 connT01 = new ConnT01();

            DataSet ds = connT01.GetBukkenName();
            DataTable dt = ds.Tables[Def_M_PROJECT.Name];


            if (!this._addModeFlag)
            {
                if (UtilData.ExistsData(ds, Def_M_PROJECT.Name))
                {
                    // 先頭行に「全て」を追加
                    DataRow dr = dt.NewRow();

                    dr[Def_M_PROJECT.PROJECT_NO] = Resources.cboAllNo;
                    dr[Def_M_PROJECT.BUKKEN_NAME] = Resources.cboAll;
                    dt.Rows.InsertAt(dr, 0);

                }
            }

            this.cboBukkenName.DisplayMember = Def_M_PROJECT.BUKKEN_NAME;
            this.cboBukkenName.ValueMember = Def_M_PROJECT.PROJECT_NO;
            this.cboBukkenName.DataSource = dt;

            // 汎用マスタから一覧取得
            this.MakeCmbBox(this.cboTehaikubun, DISP_TEHAI_FLAG.GROUPCD);
            this.MakeCmbBox(this.cboYusho, _addModeFlag ? ESTIMATE_FLAG.GROUPCD : DISP_ESTIMATE_FLAG.GROUPCD);
            this.MakeCmbBox(this.cboMitsumorizyokyo, ESTIMATE_STATUS_FLAG.GROUPCD);
            this.MakeCmbBox(this.cboDispSelect, DISP_ESTIMATE_SORT.GROUPCD);

            // 追加モードの場合は初期値を設定する
            if (_addModeFlag)
            {
                this.cboYusho.SelectedValue = ESTIMATE_FLAG.ONEROUS_VALUE1;
                this.cboMitsumorizyokyo.SelectedValue = ESTIMATE_STATUS_FLAG.NONE_VALUE1;
            }
        }

        #endregion


        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>S.Furugo 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                this.SheetClear();

                this.ChangeMode(DisplayMode.Initialize);

            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 全画面クリア処理
        /// </summary>
        /// <create>S.Furugo 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private void DisplayClearAll()
        {
            this.DisplayClear();

            try
            {
                // 検索条件をクリア
                txtSeiban.Text = string.Empty;
                txtCode.Text = string.Empty;
                txtEcsNo.Text = string.Empty;
                txtARNo.Text = string.Empty; ;
                txtNouhinsaki.Text = string.Empty;
                txtShukkasaki.Text = string.Empty;
                txtMitsumoriNo.Text = string.Empty;
                txtPONo.Text = string.Empty;

                // コンボボックスの状態を更新
                SetComboBox();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>S.Furugo 2018/11/22</create>
        /// <update>J.Chen 2024/10/29 履歴更新処理追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                bool isSelected = false;
                for (int i = 0; i < this.shtTehaiMeisai.MaxRows; i++)
                {
                    var checkstate = this.shtTehaiMeisai[SHEET_COL_CHECK, i].Value;
                    if (checkstate == null || checkstate.ToString() != CHECK_ON_STR)
                        continue;
                    isSelected = true;
                    var lineName = (i + 1).ToString();

                    // 履歴更新の場合、以下処理を省略
                    if (_isRirekiUpdate) continue;

                    switch (EstimateStatus)
                    {
                        case EstimateMode.Onerous: //有償
                            // 無償でかつ、出荷済み
                            if (this.shtTehaiMeisai[SHEET_COL_ESTIMATE_FLAG, i].Value as string == ESTIMATE_FLAG.GRATIS_VALUE1
                                && int.Parse(this.shtTehaiMeisai[SHEET_COL_CNT, i].Text) >= 1)
                            {
                                // {0}行目に有償/無償が"無償"かつ出荷済み明細を含んでいます。
                                this.ShowMessage("T0100050005", lineName);
                                return false;
                            }
                            break;
                        case EstimateMode.Gratis: //無償
                            // 有償でかつ、出荷済み
                            if (this.shtTehaiMeisai[SHEET_COL_ESTIMATE_FLAG, i].Text == ESTIMATE_FLAG.ONEROUS_VALUE1
                                && int.Parse(this.shtTehaiMeisai[SHEET_COL_CNT, i].Text) >= 1)
                            {
                                // {0}行目に有償/無償が"有償"かつ出荷済み明細を含んでいます。
                                this.ShowMessage("T0100050007", lineName);
                                return false;
                            }
                            // 有償でかつ、出荷済み
                            if (this.shtTehaiMeisai[SHEET_COL_ESTIMATE_FLAG, i].Text == ESTIMATE_FLAG.ONEROUS_VALUE1
                                && !string.IsNullOrEmpty(this.shtTehaiMeisai[SHEET_COL_ESTIMATE_NO, i].Text))
                            {
                                // {0}行目に有償/無償が"有償"かつ見積Noがセットされている明細があります。
                                this.ShowMessage("T0100050006", lineName);
                                return false;
                            }
                            break;
                        default:
                            //ありえない
                            return false;
                    }

                }
                // 未選択
                if (!isSelected)
                {
                    // 選択されている手配明細がありません。
                    this.ShowMessage("T0100060004");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                ret = false;
            }
            return ret;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>S.Furugo 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                if ((cboBukkenName.Text == Resources.cboAll || cboBukkenName.SelectedValue == null)
                    && string.IsNullOrEmpty(txtSeiban.Text)
                    && string.IsNullOrEmpty(txtCode.Text)
                    && string.IsNullOrEmpty(txtEcsNo.Text)
                    && string.IsNullOrEmpty(txtARNo.Text)
                    && string.IsNullOrEmpty(txtNouhinsaki.Text)
                    && string.IsNullOrEmpty(txtShukkasaki.Text)
                    && (cboTehaikubun.SelectedValue as string == DISP_TEHAI_FLAG.ALL_VALUE1 || cboTehaikubun.SelectedValue as string == DISP_TEHAI_FLAG.ALL_EXCEPT_CANCEL_VALUE1)
                    && (cboYusho.SelectedValue as string == DISP_ESTIMATE_FLAG.ALL_VALUE1)
                    && (cboMitsumorizyokyo.SelectedValue as string == ESTIMATE_STATUS_FLAG.ALL_VALUE1)
                    && string.IsNullOrEmpty(txtMitsumoriNo.Text)
                    && string.IsNullOrEmpty(txtPONo.Text))
                {
                    // シートのクリア
                    this.SheetClear();
                    // 検索条件を入力して下さい。
                    this.ShowMessage("T0100050001");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                ret = false;
            }
            return ret;
        }

        #endregion

        #region 検索処理
        /// --------------------------------------------------
        /// <summary>
        /// 検索処理制御部(表示位置復元あり)
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <param name="pos">以前の検索位置</param>
        /// <create>D.Okumura 2019/02/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RunSearch(Position pos)
        {
            var result = this.RunSearch();
            if (result)
            {
                // 検索が成功し、件数が所定以上なら、位置を復元する
                if (this.shtTehaiMeisai.Rows.Count > pos.Row)
                {
                    this.shtTehaiMeisai.TopLeft = pos;
                }
            }
            return result;
        }
        /// --------------------------------------------------
        /// <summary>
        /// 検索処理制御部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>S.Furugo 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearch()
        {
            return base.RunSearch();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理実行部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update>J.Chen 表示選択（並び順）追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                this.shtTehaiMeisai.Redraw = false;

                CondT01 cond = new CondT01(this.UserInfo);
                ConnT01 conn = new ConnT01();

                if (cboBukkenName.Text != Resources.cboAll
                    && cboBukkenName.SelectedValue != null)
                {
                    cond.ProjectNo = cboBukkenName.SelectedValue.ToString();
                }
                else
                {
                    cond.ProjectNo = null;
                }

                cond.Seiban = txtSeiban.Text;
                cond.Code = txtCode.Text;
                cond.EcsNo = txtEcsNo.Text;

                // ARNo
                if (!string.IsNullOrEmpty(txtARNo.Text))
                    cond.ARNo = this.lblAR.Text + this.txtARNo.Text;
                else
                    cond.ARNo = null;

                // 納入先・出荷先
                cond.Nouhinsaki = txtNouhinsaki.Text;
                cond.Shukkasaki = txtShukkasaki.Text;

                // 手配区分
                cond.TehaiKubun = cboTehaikubun.SelectedValue as string;
                // 有償・無償
                cond.Yusho = cboYusho.SelectedValue as string;
                // 見積状況
                cond.Mitsumorizyokyo = cboMitsumorizyokyo.SelectedValue as string;

                cond.MitsumoriNo = txtMitsumoriNo.Text;
                cond.PONo = txtPONo.Text;

                DataSet ds = conn.GetTehaiMitsumoriMeisai(cond);
                if (!ComFunc.IsExistsData(ds, Def_T_TEHAI_MEISAI.Name))
                {
                    // シートのクリア
                    this.SheetClear();
                    // 該当する手配明細はありません。
                    this.ShowMessage("T0100060001");
                    return false;
                }

                this._dtTehaiMitsumoriListData = ds.Tables[Def_T_TEHAI_MEISAI.Name].Copy();
                this.shtTehaiMeisai.DataSource = ds.Tables[Def_T_TEHAI_MEISAI.Name];
                // 最も左上に表示されているセルの設定
                if (0 < this.shtTehaiMeisai.MaxRows)
                {
                    this.shtTehaiMeisai.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtTehaiMeisai.TopLeft.Row);
                }

                _tempDt = (this.shtTehaiMeisai.DataSource as DataTable).Copy();

                this.ChangeMode(DisplayMode.EndSearch);

                this.ChangeSort();

                return true;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
            finally
            {
                this.shtTehaiMeisai.Redraw = true;
            }
        }

        #endregion

        #region 編集内容実行

        #region 制御メソッド

        /// --------------------------------------------------
        /// <summary>
        /// 編集内容実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>S.Furugo 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            bool ret = base.RunEdit();
            try
            {
                if (ret)
                {
                    // グリッドクリア
                    this.SheetClear();
                    // 再検索
                    this.RunSearch();
                }
                EditMode = SystemBase.EditMode.None; //ファンクションキー押下時に決定するため、状態を元に戻す
                return ret;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region 登録処理

        /// --------------------------------------------------
        /// <summary>
        /// 登録処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>S.Furugo 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {

                return true;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region 修正処理

        /// --------------------------------------------------
        /// <summary>
        /// 修正処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>S.Furugo 2018/11/22</create>
        /// <update>J.Chen 2024/10/29 履歴更新処理追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                // 更新用テーブル作成
                DataTable dtTehaiMitsumoriList = new DataTable(Def_T_TEHAI_MEISAI.Name);
                dtTehaiMitsumoriList.Columns.Add(Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO);
                dtTehaiMitsumoriList.Columns.Add(Def_T_TEHAI_MEISAI.ESTIMATE_FLAG);
                dtTehaiMitsumoriList.Columns.Add(Def_T_TEHAI_MEISAI.VERSION);
                dtTehaiMitsumoriList.Columns.Add(Def_T_TEHAI_MEISAI.ESTIMATE_RIREKI);


                this.shtTehaiMeisai.Redraw = false;
                for (int i = 0; i < this.shtTehaiMeisai.MaxRows; i++)
                {
                    // 未選択はスキップ
                    var checkstate = this.shtTehaiMeisai[SHEET_COL_CHECK, i].Value;
                    if (checkstate == null || checkstate.ToString() != CHECK_ON_STR)
                        continue;
                    // 更新用情報を追加
                    DataRow dr = dtTehaiMitsumoriList.NewRow();
                    dr[Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO] = this.shtTehaiMeisai[SHEET_COL_TEHAI_RENKEI_NO, i].Value;
                    dr[Def_T_TEHAI_MEISAI.ESTIMATE_FLAG] = EstimateStatus == EstimateMode.Gratis ? ESTIMATE_FLAG.GRATIS_VALUE1 : ESTIMATE_FLAG.ONEROUS_VALUE1;
                    dr[Def_T_TEHAI_MEISAI.VERSION] = this.shtTehaiMeisai[SHEET_COL_VERSION, i].Value;
                    dr[Def_T_TEHAI_MEISAI.ESTIMATE_RIREKI] = this.shtTehaiMeisai[SHEET_COL_ESTIMATE_RIREKI, i].Value;

                    dtTehaiMitsumoriList.Rows.Add(dr);
                }

                // 保存処理
                CondT01 cond = new CondT01(this.UserInfo);
                ConnT01 conn = new ConnT01();

                // 履歴更新かどうか
                cond.RirekiUpdate = _isRirekiUpdate;
                _isRirekiUpdate = false;

                DataSet ds = new DataSet();
                ds.Tables.Add(dtTehaiMitsumoriList);
                string errMsgID;
                string[] args;
                if (!conn.UpdTehaiMitsumoriMeisaiVersionData(cond, ds, out errMsgID, out args))
                {
                    this.ShowMessage(errMsgID, args);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
            finally
            {
                this.shtTehaiMeisai.Redraw = true;
            }
        }

        #endregion

        #region 削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>S.Furugo 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditDelete()
        {
            try
            {

                return true;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #endregion

        #region イベント

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F1ボタンクリック(見積ボタンクリック) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/22</create>
        /// <update>R.Kubota 2023/12/20 見積チェック追加</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F01Button_Click(sender, e);
            try
            {
                CondT01 cond = new CondT01(this.UserInfo);
                List<string> listRenkeiNo = new List<string>();
                listRenkeiNo.Clear();

                listZuban.Clear();
                listHinmei.Clear();
                listUnitPrice.Clear();

                //ヘッダーの背景色をクリア
                this.shtTehaiMeisai.RowHeaders.BackColor = ROWHEADERS_COLOR;

                this.ClearMessage();
                // チェック数確認
                if (this.CheckCount() <= 0)
                {
                    // 選択されている手配明細がありません。
                    this.ShowMessage("T0100060004");
                    return;
                }

                this.shtTehaiMeisai.Redraw = false;
                for (int i = 0; i < this.shtTehaiMeisai.MaxRows; i++)
                {
                    if ((this.shtTehaiMeisai.Columns[SHEET_COL_CHECK].ValueBlock[i] != null)
                        && (Convert.ToInt32(this.shtTehaiMeisai.Columns[SHEET_COL_CHECK].ValueBlock[i]) == CHECK_ON))
                    {
                        if (this.shtTehaiMeisai.Columns[SHEET_COL_ESTIMATE_FLAG].TextBlock[i] == ESTIMATE_FLAG.GRATIS_VALUE1)
                        {
                            // {0}行目の明細に無償が含まれています。
                            this.ShowMessage("T0100050002", (i + 1).ToString());
                            return;
                        }
                        else if (this.shtTehaiMeisai.Columns[SHEET_COL_ESTIMATE_FLAG].TextBlock[i] == ESTIMATE_FLAG.NEUTRAL_VALUE1)
                        {
                            // {0}行目の明細に"未指定"が含まれています。
                            this.ShowMessage("T0100050008", (i + 1).ToString());
                            return;
                        }

                        if (!string.IsNullOrEmpty((string)this.shtTehaiMeisai.Columns[SHEET_COL_ESTIMATE_NO].ValueBlock[i]))
                        {
                            // {0}行目の明細に見積Noがセットされています。
                            this.ShowMessage("T0100050003", (i + 1).ToString());
                            return;
                        }
                        if (Convert.ToInt32(this.shtTehaiMeisai.Columns[(int)SHEET_COL_CNT].ValueBlock[i]) >= 1)
                        {
                            // {0}行目の明細に出荷済みの明細を含んでいます。
                            this.ShowMessage("T0100050004", (i + 1).ToString());
                            return;
                        }

                        // 選択された行の図番、品名と単価のチェックを行う
                        if (!this.mitsumoriCheck(i))
                        {
                            return;
                        }
                    }
                }
                // 選択されている行を取得する
                var list = GetSelectedRows();
                // 手配見積画面を新規で開く
                using (var frm = new TehaiMitsumori(this.UserInfo, null, null, ComDefine.TITLE_T0100040))
                {
                    frm.StartEdit(list);
                    frm.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.shtTehaiMeisai.Redraw = true;
            }
            // 再検索を行う
            var pos = this.shtTehaiMeisai.TopLeft;
            DisplayClear();
            RunSearch(pos);
        }

        /// --------------------------------------------------
        /// <summary>
        /// F2(有償)ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/22</create>
        /// <update>D.Okumura 2019/02/22 再検索実行処理追加</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F02Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F02Button_Click(sender, e);
            try
            {
                EstimateStatus = EstimateMode.Onerous;
                EditMode = SystemBase.EditMode.Update;
                var pos = this.shtTehaiMeisai.TopLeft;
                if (RunEdit())
                {
                    // 処理が成功したとき、再検索を行う
                    RunSearch(pos);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F3(無償)ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/22</create>
        /// <update>D.Okumura 2019/02/22 再検索実行処理追加</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F03Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F03Button_Click(sender, e);
            try
            {
                EstimateStatus = EstimateMode.Gratis;
                EditMode = SystemBase.EditMode.Update;
                var pos = this.shtTehaiMeisai.TopLeft;
                if (RunEdit())
                {
                    // 処理が成功したとき、再検索を行う
                    RunSearch(pos);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F4(編集)ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/22</create>
        /// <update>D.Okumura 2019/02/22 再検索実行処理追加</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F04Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F04Button_Click(sender, e);
            try
            {
                string estimateNo;
                this.ClearMessage();

                if (string.IsNullOrEmpty((string)this.shtTehaiMeisai.Columns[(int)SHEET_COL_ESTIMATE_NO].ValueBlock[(int)this.shtTehaiMeisai.ActivePosition.Row]))
                {
                    // カーソルのある行の見積No.が未設定です。
                    this.ShowMessage("T0100050009");
                    return;
                }

                estimateNo = (string)this.shtTehaiMeisai.Columns[(int)SHEET_COL_ESTIMATE_NO].ValueBlock[(int)this.shtTehaiMeisai.ActivePosition.Row];

                // 手配見積画面(編集)を開く
                using (var frm = new TehaiMitsumori(this.UserInfo, null, null, ComDefine.TITLE_T0100040))
                {
                    frm.StartEdit(estimateNo);
                    frm.ShowDialog(this);
                }
                // 再検索を行う
                var pos = this.shtTehaiMeisai.TopLeft;
                DisplayClear();
                RunSearch(pos);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F5(手配明細)ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>J.Chen 2024/02/06</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F05Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F05Button_Click(sender, e);
            try
            {
                // 検索結果がなければ処理を抜ける
                if (this.shtTehaiMeisai.MaxRows < 1) return;

                // チェック数確認
                if (this.CheckCount() <= 0)
                {
                    // 選択されている手配明細がありません。
                    this.ShowMessage("T0100060004");
                    return;
                }

                //１件以上選択
                if (CheckCount() > 1)
                {
                    // 
                    this.ShowMessage("T0100050011");
                    return;
                }

                // チェックした行番号を取得
                ArrayList rows = CheckIndex();
                int row = (int)rows[0];

                var ecsQuota = UtilConvert.ToInt32(this.shtTehaiMeisai[SHEET_COL_ECS_QUOTA, row].Text);
                var ecsNo = this.shtTehaiMeisai[SHEET_COL_ECS_NO, row].Text;

                using (var frm = new TehaiMeisai(this.UserInfo, ComDefine.TITLE_T0100010, ecsQuota, ecsNo))
                {
                    if (DialogResult.OK == frm.ShowDialog())
                    {
                        // 再検索を行いますか？
                        if (DialogResult.OK == this.ShowMessage("T0100030003"))
                        {
                            // 再検索を行う
                            this.btnStart_Click(sender, e);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F6(Clear)ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                this.ClearMessage();
                if (this.ShowMessage("A9999999053") != DialogResult.OK)
                {
                    return;
                }
                DisplayClear();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F7(All Clear)ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                // クリアしてもいいですか？ダイアログ表示
                if (this.ShowMessage("A9999999001") != DialogResult.OK)
                {
                    return;
                }
                this.DisplayClearAll();
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F8(履歴更新)ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>J.Chen 2024/10/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F08Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F08Button_Click(sender, e);
            try
            {
                EditMode = SystemBase.EditMode.Update;
                _isRirekiUpdate = true;
                var pos = this.shtTehaiMeisai.TopLeft;
                if (RunEdit())
                {
                    // 処理が成功したとき、再検索を行う
                    RunSearch(pos);
                }
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F9(追加)ボタンクリック 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/22</create>
        /// <update>J.Chen 2024/03/14 見積チェック追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F09Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F09Button_Click(sender, e);
            try
            {
                //ヘッダーの背景色をクリア
                this.shtTehaiMeisai.RowHeaders.BackColor = ROWHEADERS_COLOR;

                // 見積チェック用リストクリア
                listZuban.Clear();
                listHinmei.Clear();
                listUnitPrice.Clear();

                // チェックしたレコードを元画面(手配見積画面)へ引き渡す
                if (this.CheckCount() <= 0)
                {
                    // 選択されている手配明細がありません。
                    this.ShowMessage("T0100060004");
                    return;
                }

                var parent = this.Owner as TehaiMitsumori;
                if (parent == null)
                    parent = this.ParentForm as TehaiMitsumori;
                if (parent == null)
                    return;
                // 選択されている行を取得する
                var list = GetSelectedRows();
                if (list.Count < 1)
                {
                    // 選択されている手配明細がありません。
                    this.ShowMessage("T0100060004");
                    return;
                }

                // 選択された行の図番、品名と単価のチェックを行う
                for (int i = 0; i < this.shtTehaiMeisai.MaxRows; i++)
                {
                    if ((this.shtTehaiMeisai.Columns[SHEET_COL_CHECK].ValueBlock[i] != null)
                        && (Convert.ToInt32(this.shtTehaiMeisai.Columns[SHEET_COL_CHECK].ValueBlock[i]) == CHECK_ON))
                    {
                        if (!this.mitsumoriCheck(i))
                        {
                            return;
                        }
                    }
                }

                // コールバック
                string msgId;
                string[] msgParams;
                if (!parent.AddDataRow(list, out msgId, out msgParams))
                {
                    // エラーがある場合はメッセージを表示して終了する
                    this.ShowMessage(msgId, msgParams);
                    return;
                }
                // 閉じる際のメッセージを抑止する
                IsCloseQuestion = false;
                this.Close();

            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }
        /// --------------------------------------------------
        /// <summary>
        /// F10(Excel)ボタンクリック 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>J.Chen 2024/02/13</create>
        /// <update>J.Chen 2024/10/30 更新履歴出力追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F10Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F10Button_Click(sender, e);
            try
            {
                if (this.cboBukkenName.SelectedValue.ToString() != "0000")
                {
                    DataTable dtUpdCheck = (this.shtTehaiMeisai.DataSource as DataTable).Copy();
                    DataTable dtUpdate = this.GetDataTehaiMitsumoriMeisaiFilter(dtUpdCheck);   // 更新データ抽出

                    if (dtUpdate != null)
                    {
                        // 内容が変更されているため、Excel出力できません。
                        this.ShowMessage("S0100070004");
                        return;
                    }
                }
                

                using (SaveFileDialog frm = new SaveFileDialog())
                {
                    frm.Title = Resources.MitsumoriMeisai_sfdExcel_Title;
                    frm.Filter = Resources.MitsumoriMeisai_sfdExcel_Filter;
                    frm.FileName = string.Format(ComDefine.EXCEL_FILE_MITSUMORIMEISAI
                        , this.cboBukkenName.Text
                        );
                    if (0 < this.shtTehaiMeisai.MaxRows && frm.ShowDialog() != DialogResult.OK) return;
                    // Excel出力処理
                    DataTable dtExport = (this.shtTehaiMeisai.DataSource as DataTable).Copy();

                    ExportMitsumoriMeisai export = new ExportMitsumoriMeisai();
                    string msgID;
                    string[] args;

                    export.ExportExcel(frm.FileName, dtExport, out msgID, out args);
                    if (!string.IsNullOrEmpty(msgID))
                    {
                        this.ShowMessage(msgID, args);
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// チェックの入った行を取得する
        /// </summary>
        /// <returns>選択行</returns>
        /// <create>D.Okumura 2018/12/21</create>
        /// <update></update>
        /// <remarks>
        /// シートから業務キー(手配連携No)を元にデータソース検索する
        /// </remarks>
        /// --------------------------------------------------
        private List<DataRow> GetSelectedRows()
        {
            var sheet = this.shtTehaiMeisai;
            var list = new List<DataRow>();
            // 安全のためデータがあることを確認してから処理を行う
            if (sheet.MaxRows < 1)
            {
                return list;
            }
            var dt = sheet.DataSource as DataTable;
            for (int i = 0; i < sheet.MaxRows; i++)
            {
                // チェックボックスの状態
                var checkdata = sheet.Columns[SHEET_COL_CHECK].ValueBlock[i];
                if (checkdata == null || (double)checkdata != CHECK_ON)
                    continue;
                var renkeiNo = sheet.Columns[SHEET_COL_TEHAI_RENKEI_NO].ValueBlock[i];
                // 行を見つけ出す
                var baseRow = dt.AsEnumerable().FirstOrDefault(w => w[Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO] == renkeiNo);
                if (baseRow == null)
                    continue; //通常あり得ない
                list.Add(baseRow);
            }
            return list;
        }

        /// --------------------------------------------------
        /// <summary>
        /// F12ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F12Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F12Button_Click(sender, e);
            try
            {
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region ボタンクリック
        /// --------------------------------------------------
        /// <summary>
        /// 開始クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                this.RunSearch();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 全選択クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnAllSelect_Click(object sender, EventArgs e)
        {
            this.shtTehaiMeisai.Redraw = false;

            this.SetAllCheck(true);

            this.shtTehaiMeisai.Redraw = true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 全選択解除クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnAllRelease_Click(object sender, EventArgs e)
        {
            this.shtTehaiMeisai.Redraw = false;

            this.SetAllCheck(false);

            this.shtTehaiMeisai.Redraw = true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 範囲選択されたグリッドをチェックします。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnRangeSelect_Click(object sender, EventArgs e)
        {
            this.shtTehaiMeisai.Redraw = false;

            this.SetRangeCheck(true);

            this.shtTehaiMeisai.Redraw = true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 範囲選択されたグリッドからチェックをはずします。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnRangeRelease_Click(object sender, EventArgs e)
        {
            this.shtTehaiMeisai.Redraw = false;

            this.SetRangeCheck(false);

            this.shtTehaiMeisai.Redraw = true;
        }

        #endregion

        #region シートのクリア

        /// --------------------------------------------------
        /// <summary>
        /// シートのクリア
        /// </summary>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            this.shtTehaiMeisai.Redraw = false;
            // 最も左上に表示されているセルの設定
            if (0 < this.shtTehaiMeisai.MaxRows)
            {
                this.shtTehaiMeisai.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtTehaiMeisai.TopLeft.Row);
            }
            this.shtTehaiMeisai.DataSource = null;
            this.shtTehaiMeisai.MaxRows = 0;
            this.shtTehaiMeisai.Enabled = false;
            this.shtTehaiMeisai.Redraw = true;
        }

        #endregion

        #region シートイベント

        /// --------------------------------------------------
        /// <summary>
        /// データソースに DataRow が追加または削除され、シートの行が追加または削除される前に発生するイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2018/12/19</create>
        /// <update>J.Chen 2024/02/09 出荷制限、出荷済の色を追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private void shtTehaiMeisai_RowFilling(object sender, RowFillingEventArgs e)
        {
            var sheet = sender as Sheet;
            if (sheet == null)
                return;

            if (e.DestRow != -1 && e.OperationMode == OperationMode.Add)
            {
                var row = e.DestRow;
                var dt = sheet.DataSource as DataTable;
                var col = string.Empty;

                var henkyakuhinFlag = ComFunc.GetFldToInt32(dt, row, Def_T_TEHAI_MEISAI.HENKYAKUHIN_FLAG);
                var shukkazumiCNT = ComFunc.GetFldToInt32(dt, row, ComDefine.FLD_CNT);
                var offset = dt.Columns[ComDefine.FLD_ESTIMATE_COLOR].Ordinal;

                if (henkyakuhinFlag > 0)
                {
                    // 出荷制限（返却品）の場合
                    col = ComDefine.PURPLE_COLOR;
                }
                else if (shukkazumiCNT >= 1)
                {
                    // 出荷済の場合
                    col = ComDefine.BLUE_COLOR;
                }
                else
                {
                    col = ((object[])e.SourceRow)[offset] as string;
                }

                SetupRowColor(sheet.Rows[row], col, sheet.GridLine, Borders.All);
            }
        }
        /// --------------------------------------------------
        /// <summary>
        /// 行の背景色を変更する
        /// </summary>
        /// <param name="row">列</param>
        /// <param name="input">入力文字列</param>
        /// <param name="baseGridLine">基準となる罫線設定</param>
        /// <param name="border">罫線設定</param>
        /// <create>D.Okumura 2018/12/20</create>
        /// <update>D.Okumura 2019/01/08 罫線が消えてしまう問題を修正</update>
        /// --------------------------------------------------
        private static void SetupRowColor(Row row, string input, GridLine baseGridLine, Borders border)
        {
            if (string.IsNullOrEmpty(input))
                return;
            var cols = input.Split(',');
            if (cols.Length < 2)
                return;

            var backcolor = GetColorFromRgb(cols[1]);
            if (backcolor != null)
            {
                row.BackColor = backcolor ?? row.BackColor;
                row.DisabledBackColor = backcolor ?? row.BackColor;
                // 背景初期を設定すると、罫線がなくなってしまうため、手動設定する
                row.SetBorder(new BorderLine()
                {
                    Color = baseGridLine.Color,
                    LineStyle = (BorderLineStyle)Enum.Parse(typeof(BorderLineStyle), Enum.GetName(typeof(GridLineStyle), baseGridLine.Style)),
                }, border);
            }

            var forecolor = GetColorFromRgb(cols[0]);
            if (forecolor != null)
            {
                row.ForeColor = forecolor ?? row.ForeColor;
                row.DisabledForeColor = forecolor ?? row.ForeColor;
            }
        }
        /// --------------------------------------------------
        /// <summary>
        /// RRGGBB形式の文字列からColorオブジェクトを生成する
        /// </summary>
        /// <param name="input">RGB文字列</param>
        /// <returns>色</returns>
        /// <create>D.Okumura 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private static Color? GetColorFromRgb(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            int val;
            if (!int.TryParse(input, System.Globalization.NumberStyles.HexNumber, null, out val))
                return null;
            return Color.FromArgb(val);
        }
        #endregion

        #endregion

        #region モード切り替え

        /// --------------------------------------------------
        /// <summary>
        /// モード切替
        /// </summary>
        /// <param name="mode">画面の表示モード</param>
        /// <create>S.Furugo 2018/11/22</create>
        /// <update>J.Chen 2024/02/06 項目追加</update>
        /// <update>J.Chen 2024/10/29 履歴更新追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeMode(DisplayMode mode)
        {
            try
            {
                switch (mode)
                {
                    case DisplayMode.Initialize:
                        // ----- 初期 -----
                        this.lblBukkenName.Enabled = true;
                        this.lblSeiban.Enabled = true;
                        this.lblCode.Enabled = true;
                        this.lblEcsNo.Enabled = true;
                        this.lblARNo.Enabled = true;
                        this.lblNouhinsaki.Enabled = true;
                        this.lblShukkasaki.Enabled = true;
                        this.lblTehaikubun.Enabled = true;
                        this.lblYusho.Enabled = true;
                        this.lblMitsumorizyokyo.Enabled = true;
                        this.lblMitsumoriNo.Enabled = true;
                        this.lblPONo.Enabled = true;
                        this.btnStart.Enabled = true;
                        this.lblYusho.Enabled = !_addModeFlag;
                        this.lblMitsumorizyokyo.Enabled = !_addModeFlag;
                        this.lblMitsumoriNo.Enabled = !_addModeFlag;
                        this.lblPONo.Enabled = !_addModeFlag;
                        this.shtTehaiMeisai.Enabled = false;
                        this.btnAllSelect.Enabled = false;
                        this.btnAllRelease.Enabled = false;
                        this.btnRangeSelect.Enabled = false;
                        this.btnRangeRelease.Enabled = false;
                        this.fbrFunction.F01Button.Enabled = false;
                        this.fbrFunction.F02Button.Enabled = false;
                        this.fbrFunction.F03Button.Enabled = false;
                        this.fbrFunction.F04Button.Enabled = false;
                        this.fbrFunction.F05Button.Enabled = false;
                        this.fbrFunction.F06Button.Enabled = false;
                        this.fbrFunction.F08Button.Enabled = false;
                        this.fbrFunction.F09Button.Enabled = false;
                        this.fbrFunction.F10Button.Enabled = false;
                        break;
                    case DisplayMode.EndSearch:
                        // ----- 検索後 -----
                        this.lblBukkenName.Enabled = false;
                        this.lblSeiban.Enabled = false;
                        this.lblCode.Enabled = false;
                        this.lblEcsNo.Enabled = false;
                        this.lblARNo.Enabled = false;
                        this.lblNouhinsaki.Enabled = false;
                        this.lblShukkasaki.Enabled = false;
                        this.lblTehaikubun.Enabled = false;
                        this.lblYusho.Enabled = false;
                        this.lblMitsumorizyokyo.Enabled = false;
                        this.lblMitsumoriNo.Enabled = false;
                        this.lblPONo.Enabled = false;
                        this.btnStart.Enabled = false;
                        this.shtTehaiMeisai.Enabled = true;
                        this.btnAllSelect.Enabled = true;
                        this.btnAllRelease.Enabled = true;
                        this.btnRangeSelect.Enabled = true;
                        this.btnRangeRelease.Enabled = true;
                        this.fbrFunction.F01Button.Enabled = !_addModeFlag;
                        this.fbrFunction.F02Button.Enabled = !_addModeFlag;
                        this.fbrFunction.F03Button.Enabled = !_addModeFlag;
                        this.fbrFunction.F04Button.Enabled = !_addModeFlag;
                        this.fbrFunction.F05Button.Enabled = !_addModeFlag;
                        this.fbrFunction.F06Button.Enabled = true;
                        this.fbrFunction.F08Button.Enabled = !_addModeFlag;
                        this.fbrFunction.F09Button.Enabled = _addModeFlag;
                        this.fbrFunction.F10Button.Enabled = !_addModeFlag;

                        // 物件名「全て」の場合、履歴更新できないようにする
                        if (this.cboBukkenName.SelectedValue.ToString() == "0000")
                        {
                            // 物件名が「全て」の場合、履歴の更新はできません。
                            this.ShowMessage("T0100030020");
                            var txtEditor = ElTabelleSheetHelper.NewTextEditor();
                            txtEditor.ReadOnly = true;
                            this.shtTehaiMeisai.Columns[(int)SHEET_COL_ESTIMATE_RIREKI].Editor = txtEditor;
                            this.fbrFunction.F08Button.Enabled = false;
                        }
                        else
                        {
                            var txtEditor = ElTabelleSheetHelper.NewTextEditor();
                            txtEditor.ReadOnly = false;
                            txtEditor.MaxLength = 500;
                            this.shtTehaiMeisai.Columns[(int)SHEET_COL_ESTIMATE_RIREKI].Editor = txtEditor;
                            this.fbrFunction.F08Button.Enabled = true;
                        }

                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region CHECK列のチェックボックスON/OFF制御用（フィルタリングを考慮）

        /// --------------------------------------------------
        /// <summary>
        /// 全選択・全選択解除時のCHECK列チェックボックスON/OFF設定
        /// </summary>
        /// <param name="isChecked">CHECK列チェックボックスをONするかどうか</param>
        /// <create>S.Furugo 2018/11/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetAllCheck(bool isChecked)
        {
            var ranges = this.GetValidRanges(new GrapeCity.Win.ElTabelle.Range[] { new GrapeCity.Win.ElTabelle.Range(SHEET_COL_CHECK, 0, SHEET_COL_CHECK, this.shtTehaiMeisai.MaxRows - 1) });
            this.SetCellRangeValueForCheckBox(ranges, isChecked);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 範囲選択・範囲選択解除時のCHECK列チェックボックスON/OFF設定
        /// </summary>
        /// <param name="isChecked">CHECK列チェックボックスをONするかどうか</param>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetRangeCheck(bool isChecked)
        {
            var ranges = this.GetValidRanges(this.shtTehaiMeisai.GetBlocks(GrapeCity.Win.ElTabelle.BlocksType.Selection));
            this.SetCellRangeValueForCheckBox(ranges, isChecked);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定Rangeのチェックボックス値設定
        /// </summary>
        /// <param name="ranges">Range配列</param>
        /// <param name="isCheck">チェックボックスをONするかどうか</param>
        /// <create>H.Tajimi 2015/11/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetCellRangeValueForCheckBox(GrapeCity.Win.ElTabelle.Range[] ranges, bool isChecked)
        {
            foreach (var range in ranges)
            {
                this.shtTehaiMeisai.CellRange = range;
                this.shtTehaiMeisai.CellValue = isChecked ? CHECK_ON : CHECK_OFF;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定Rangeで有効となっているRange配列を取得
        /// </summary>
        /// <param name="ranges">Range配列</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2015/11/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private GrapeCity.Win.ElTabelle.Range[] GetValidRanges(GrapeCity.Win.ElTabelle.Range[] ranges)
        {
            var lstRanges = new List<GrapeCity.Win.ElTabelle.Range>();
            foreach (var range in ranges)
            {
                for (int rowIndex = range.TopRow; rowIndex <= range.BottomRow; rowIndex++)
                {
                    if (IsValidRow(rowIndex))
                    {
                        lstRanges.Add(new Range(0, rowIndex, 0, rowIndex));
                    }
                }
            }
            return lstRanges.ToArray();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定行が有効かどうか
        /// </summary>
        /// <remarks>
        /// El TablledのAutoFilterは行の高さを0にすることでフィルタリングを
        /// 実現しているので、行の高さでフィルタリングされているかどうか判定
        /// </remarks>
        /// <param name="rowIndex">行インデックス</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2015/11/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool IsValidRow(int rowIndex)
        {
            if (this.shtTehaiMeisai.MaxRows - 1 < rowIndex)
            {
                return false;
            }

            if (this.shtTehaiMeisai.Rows[rowIndex].Height < 1)
            {
                return false;
            }

            return true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// チェックがついている数を取得
        /// </summary>
        /// <returns></returns>
        /// <create>S.Furugo 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private int CheckCount()
        {
            if (this.shtTehaiMeisai.MaxRows <= 0) return 0;
            int ret = 0;
            for (int i = 0; i < this.shtTehaiMeisai.MaxRows; i++)
            {
                if (this.shtTehaiMeisai.Columns[SHEET_COL_CHECK].ValueBlock[i] != null)
                {
                    if (Convert.ToInt32(this.shtTehaiMeisai.Columns[SHEET_COL_CHECK].ValueBlock[i]) == CHECK_ON) ret++;
                }
            }
            return ret;
        }

        /// --------------------------------------------------
        /// <summary>
        /// チェックがついている行番号を取得
        /// </summary>
        /// <returns></returns>
        /// <create>J.Chen 2024/02/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private ArrayList CheckIndex()
        {
            ArrayList indexList = new ArrayList();
            for (int i = 0; i < this.shtTehaiMeisai.MaxRows; i++)
            {
                // 未選択はスキップ
                var checkstate = this.shtTehaiMeisai[SHEET_COL_CHECK, i].Value;
                if (checkstate == null || checkstate.ToString() != CHECK_ON_STR)
                    continue;
                else if (checkstate != null && checkstate.ToString() == CHECK_ON_STR)
                {
                    indexList.Add(i);
                }
            }

            return indexList;
        }

        #endregion

        #region AutoFilter設定

        /// --------------------------------------------------
        /// <summary>
        /// AutoFilter設定
        /// </summary>
        /// <param name="isForceClear">強制的にAutoFilter設定をクリアするかどうか</param>
        /// <create>J.Chen 2024/02/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetEnableAutoFilter(bool isForceClear)
        {
            try
            {
                this.shtTehaiMeisai.Redraw = false;
                foreach (int col in this._autoFilterColumns)
                {
                    this.SetEnableAutoFilter(isForceClear, col);
                }
            }
            finally
            {
                this.shtTehaiMeisai.Redraw = true;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// AutoFilter設定 - 列指定
        /// </summary>
        /// <param name="isForceClear">強制的にAutoFilter設定をクリアするかどうか</param>
        /// <param name="col">列番号</param>
        /// <create>J.Chen 2024/02/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetEnableAutoFilter(bool isForceClear, int col)
        {
            if (isForceClear)
            {
                this.shtTehaiMeisai.ColumnHeaders[col].DropDown = null;
            }
            else
            {
                if (this.shtTehaiMeisai.ColumnHeaders[col].DropDown == null)
                {
                    var headerDropDown = new HeaderDropDown();
                    headerDropDown.EnableAutoFilter = true;
                    headerDropDown.EnableAutoSort = false;  // ソート系の項目は非表示
                    this.shtTehaiMeisai.ColumnHeaders[col].DropDown = headerDropDown;
                }
                else
                {
                    this.shtTehaiMeisai.ColumnHeaders[col].DropDown.EnableAutoFilter = true;
                    this.shtTehaiMeisai.ColumnHeaders[col].DropDown.EnableAutoSort = false;  // ソート系の項目は非表示
                }
            }
        }

        #endregion

        #region 表示選択変更時

        /// --------------------------------------------------
        /// <summary>
        /// 表示選択変更時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>J.Chen 2024/02/013</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboDispSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ChangeSort();
        }

        #endregion 表示選択変更時

        #region 並び順切り替え

        /// --------------------------------------------------
        /// <summary>
        /// 並び順切替
        /// </summary>
        /// <create>J.Chen 2024/02/013</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeSort()
        {
            DataTable dt = this.shtTehaiMeisai.DataSource as DataTable;

            if (dt == null) return;

            DataView view = new DataView(dt);

            if (this.cboDispSelect.SelectedIndex == Int32.Parse(DISP_ESTIMATE_SORT.DEFAULT_VALUE1))
            {
                view.Sort = Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO + " ASC";
            }
            if (this.cboDispSelect.SelectedIndex == Int32.Parse(DISP_ESTIMATE_SORT.ESTIMATE_NO_ASC_VALUE1))
            {
                view.Sort = Def_T_TEHAI_MEISAI.ESTIMATE_NO + " ASC";
            }
            if (this.cboDispSelect.SelectedIndex == Int32.Parse(DISP_ESTIMATE_SORT.ESTIMATE_NO_DESC_VALUE1))
            {
                view.Sort = Def_T_TEHAI_MEISAI.ESTIMATE_NO + " DESC";
            }
            if (this.cboDispSelect.SelectedIndex == Int32.Parse(DISP_ESTIMATE_SORT.SHIP_DATE_ASC_VALUE1))
            {
                view.Sort = Def_T_SHUKKA_MEISAI.SHUKKA_DATE + " ASC";
            }
            if (this.cboDispSelect.SelectedIndex == Int32.Parse(DISP_ESTIMATE_SORT.SHIP_DATE_DESC_VALUE1))
            {
                view.Sort = Def_T_SHUKKA_MEISAI.SHUKKA_DATE + " DESC";
            }

            this.shtTehaiMeisai.DataSource = view.ToTable();
        }

        #endregion

        #region 見積チェック

        /// --------------------------------------------------
        /// <summary>
        /// 見積チェック
        /// </summary>
        /// <create>R.Kubota 2023/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool mitsumoriCheck(int i)
        {
            // 図番
            string zuban;
            // 品名
            string hinmei;
            // 単価
            double? unitPrice;

            //図番と品名と単価に値を代入
            zuban = (string)this.shtTehaiMeisai.Columns[SHEET_COL_ZUMEN_KEISHIKI].ValueBlock[i];
            hinmei = (string)this.shtTehaiMeisai.Columns[SHEET_COL_HINMEI].ValueBlock[i];
            unitPrice = (double?)this.shtTehaiMeisai.Columns[SHEET_COL_UNIT_PRICE].ValueBlock[i];

            //図番と品名と単価を配列に保存
            listZuban.Add(zuban != null ? zuban : "");
            listHinmei.Add(hinmei != null ? hinmei : "");
            listUnitPrice.Add(unitPrice != null ? unitPrice : 0.0);

            // 各図番に対して一意な品名と単価の組み合わせを保持するDictionaryを作成
            Dictionary<string, KeyValuePair<string, double?>> uniqueValues = new Dictionary<string, KeyValuePair<string, double?>>();

            //図番配列の個数分繰り返す
            for (int j = 0; j < listZuban.Count; j++)
            {
                string key = listZuban[j];

                // Dictionaryに重複する図番が存在する場合
                if (uniqueValues.ContainsKey(key))
                {
                    var existingValue = uniqueValues[key];

                    //登録済みの組み合わせと異なる場合はエラーメッセージを返す
                    if (existingValue.Key != listHinmei[j] || existingValue.Value != listUnitPrice[j])
                    {
                        //対象の行のヘッダーを赤くする
                        this.shtTehaiMeisai.RowHeaders[i].BackColor = Color.Red;
                        this.shtTehaiMeisai.ActivePosition = new Position(1, i);
                        // 同型式品で単価・品名が一致しない項目があるため見積もりを作成できません。同型式品の単価と品名を統一してください。
                        this.ShowMessage("T0100050010");
                        return false;
                    }
                }
                // Dictionaryに重複する図番が存在しない場合、その図番と品名・単価の新しい組み合わせを登録
                else
                {
                    uniqueValues[key] = new KeyValuePair<string, double?>(listHinmei[j], listUnitPrice[j]);
                }
            }

            return true;
        }

        #endregion

        #region 登録データ取得
        /// --------------------------------------------------
        /// <summary>
        /// 手配見積情報取得(フィルター抽出)
        /// </summary>
        /// <param name="dtSrc">取得元明細情報</param>
        /// <param name="state">抽出条件</param>
        /// <returns>納入先マスタテーブル</returns>
        /// <create>J.Chen 2024/10/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetDataTehaiMitsumoriMeisaiFilter(DataTable dtSrc)
        {
            try
            {
                DataTable dt = dtSrc.Clone();
                string name = ComDefine.DTTBL_UPDATE;

                foreach (DataRow row in dtSrc.Rows)
                {
                    DataRow matchingRow = null;
                    foreach (DataRow tempRow in _tempDt.Rows)
                    {
                        bool renkeiNoMatches = row[Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO].Equals(tempRow[Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO]);

                        if (renkeiNoMatches)
                        {
                            matchingRow = tempRow;
                            break;
                        }
                    }

                    if (matchingRow != null)
                    {
                        if (!row[Def_T_TEHAI_MEISAI.ESTIMATE_RIREKI].Equals(matchingRow[Def_T_TEHAI_MEISAI.ESTIMATE_RIREKI]))
                        {
                            dt.ImportRow(row);
                        }
                    }
                }

                if (dt.Rows.Count == 0) return null;
                dt.TableName = name;

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

    }
}
